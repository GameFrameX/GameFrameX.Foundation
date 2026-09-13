// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 Apache License 2.0 许可证分发，
//  This project is distributed under the Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:    https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Idempotency.Localization;
using GameFrameX.Foundation.Localization.Core;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 幂等协调器：副作用调用方的唯一入口，编排占位、判定与回放 / The idempotency coordinator: the single entry point for side-effect callers, orchestrating occupation, judgement and replay.
/// </summary>
/// <remarks>
/// 组合幂等协作点（存储、时钟、选项），对一次占位请求给出最终裁决；幂等键与请求摘要是调用方侧工具的产物，由请求对象携带传入。
/// <para>
/// <b>判定语义表（<see cref="BeginAsync"/> 的全部分支）：</b>
/// <list type="table">
/// <item><term>无存量记录</term><description><see cref="IdempotencyDecisionKind.Execute"/>：占位为执行中。</description></item>
/// <item><term>已完成，摘要相同</term><description><see cref="IdempotencyDecisionKind.Replay"/>：携带首次响应回放，不得再次执行。</description></item>
/// <item><term>已完成，摘要不同</term><description><see cref="IdempotencyDecisionKind.Conflict"/>：键冲突，拒绝执行。</description></item>
/// <item><term>已失败，策略为回放错误</term><description><see cref="IdempotencyDecisionKind.Replay"/>：存量记录状态为失败，调用方映射回原错误。</description></item>
/// <item><term>已失败，策略为重新执行</term><description><see cref="IdempotencyDecisionKind.Execute"/>：覆盖占位后再次执行。</description></item>
/// <item><term>执行中，等待不超时</term><description>等待并发占位方结束后按上述规则重判。</description></item>
/// <item><term>执行中，等待超时</term><description><see cref="IdempotencyDecisionKind.Busy"/>：并发占位方长时间未结束，稍后重试。</description></item>
/// <item><term>任意状态，已过期</term><description>视为无记录，<see cref="IdempotencyDecisionKind.Execute"/>：新占位覆盖旧记录。</description></item>
/// </list>
/// </para>
/// <para>
/// <b>占位抢占重判：</b><see cref="IdempotencyDecisionKind.Execute"/> 分支会立即尝试占位；
/// 占位被并发方抢先时按上述语义重判一次，仍被抢占则返回 <see cref="IdempotencyDecisionKind.Busy"/>。
/// </para>
/// <para>
/// 调用方约定：<see cref="BeginAsync"/> 返回 <see cref="IdempotencyDecisionKind.Execute"/> 后业务才可执行，
/// 执行结束必须调用 <see cref="CompleteAsync"/> 或 <see cref="FailAsync"/> 落定记录，否则记录将保持执行中直至过期。
/// </para>
/// <para>
/// Composes the idempotency collaboration points (store, clock, options) and delivers the final verdict for one
/// occupation request; the idempotency key and request digest are products of caller-side tools and arrive
/// carried by the request object.
/// <b>Decision-semantics table (all branches of <see cref="BeginAsync"/>):</b>
/// <list type="table">
/// <item><term>No existing record</term><description><see cref="IdempotencyDecisionKind.Execute"/>: occupy as processing.</description></item>
/// <item><term>Completed, identical digest</term><description><see cref="IdempotencyDecisionKind.Replay"/>: replay with the first response; do not execute again.</description></item>
/// <item><term>Completed, different digest</term><description><see cref="IdempotencyDecisionKind.Conflict"/>: key conflict; refuse to execute.</description></item>
/// <item><term>Failed, policy is replay-error</term><description><see cref="IdempotencyDecisionKind.Replay"/>: the stored record is failed; the caller maps it back to the original error.</description></item>
/// <item><term>Failed, policy is re-execute</term><description><see cref="IdempotencyDecisionKind.Execute"/>: overwrite-occupy and execute again.</description></item>
/// <item><term>Processing, wait not timed out</term><description>Wait for the concurrent occupant to settle, then re-judge by the rules above.</description></item>
/// <item><term>Processing, wait timed out</term><description><see cref="IdempotencyDecisionKind.Busy"/>: the concurrent occupant did not settle in time; retry later.</description></item>
/// <item><term>Any status, expired</term><description>Treated as no record; <see cref="IdempotencyDecisionKind.Execute"/>: a new occupation overwrites the old record.</description></item>
/// </list>
/// <b>Occupation-preemption re-judgement:</b> the <see cref="IdempotencyDecisionKind.Execute"/> branch attempts the
/// occupation immediately; if a concurrent party preempts it, the semantics are re-judged once, and a second
/// preemption returns <see cref="IdempotencyDecisionKind.Busy"/>.
/// Caller convention: the business may only execute after <see cref="BeginAsync"/> returns
/// <see cref="IdempotencyDecisionKind.Execute"/>, and after execution it must call <see cref="CompleteAsync"/> or
/// <see cref="FailAsync"/> to settle the record — otherwise the record stays processing until it expires.
/// </para>
/// </remarks>
public sealed class IdempotencyCoordinator
{
    /// <summary>
    /// 等待并发占位方时的轮询间隔（毫秒）/ The polling interval in milliseconds while waiting on a concurrent occupant.
    /// </summary>
    private const int ConcurrentWaitPollIntervalMilliseconds = 25;

    private readonly IIdempotencyStore _idempotencyStore;
    private readonly IClock _clock;
    private readonly IdempotencyOptions _idempotencyOptions;

    /// <summary>
    /// 初始化 <see cref="IdempotencyCoordinator"/> / Initializes <see cref="IdempotencyCoordinator"/>.
    /// </summary>
    /// <remarks>
    /// 幂等键与请求摘要由调用方在构造 <see cref="IdempotencyBeginRequest"/> 前生成（可用 <see cref="IIdempotencyKeyGenerator"/>
    /// 与 <see cref="IRequestDigester"/> 等调用方侧工具），协调器只消费请求对象携带的现值。
    /// <para>
    /// The idempotency key and request digest are produced by the caller before constructing
    /// <see cref="IdempotencyBeginRequest"/> (optionally via caller-side tools such as <see cref="IIdempotencyKeyGenerator"/>
    /// and <see cref="IRequestDigester"/>); the coordinator only consumes the values carried by the request object.
    /// </para>
    /// </remarks>
    /// <param name="idempotencyStore">幂等记录存储 / The idempotency record store.</param>
    /// <param name="clock">时间源 / The time source.</param>
    /// <param name="idempotencyOptions">幂等选项，<see langword="null"/> 时使用全部默认值 / The idempotency options; <see langword="null"/> uses all defaults.</param>
    /// <exception cref="ArgumentNullException"><paramref name="idempotencyStore"/> 或 <paramref name="clock"/> 为 <see langword="null"/> / <paramref name="idempotencyStore"/> or <paramref name="clock"/> is <see langword="null"/>.</exception>
    public IdempotencyCoordinator(IIdempotencyStore idempotencyStore, IClock clock, IdempotencyOptions? idempotencyOptions = null)
    {
        _idempotencyStore = idempotencyStore ?? throw new ArgumentNullException(nameof(idempotencyStore), LocalizationService.GetString(LocalizationKeys.Exceptions.StoreCannotBeNull));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock), LocalizationService.GetString(LocalizationKeys.Exceptions.ClockCannotBeNull));
        _idempotencyOptions = idempotencyOptions ?? new IdempotencyOptions();
    }

    /// <summary>
    /// 发起一次幂等占位判定，返回执行、回放、冲突或忙碌决策 / Initiates an idempotency occupation judgement and returns the execute, replay, conflict or busy decision.
    /// </summary>
    /// <remarks>
    /// 判定语义表见类备注。等待执行中的并发占位方时受
    /// <see cref="IdempotencyOptions.ConcurrentWaitTimeoutMilliseconds"/> 约束，期间周期性重读记录、状态变化即重判；
    /// 取消令牌触发时以取消异常向上传播。
    /// <para>
    /// See the class remarks for the decision-semantics table. Waiting on a processing concurrent occupant is bound
    /// by <see cref="IdempotencyOptions.ConcurrentWaitTimeoutMilliseconds"/>; during the wait the record is re-read
    /// periodically and any state change triggers re-judgement. If the cancellation token fires, a cancellation
    /// exception propagates upward.
    /// </para>
    /// </remarks>
    /// <param name="request">占位请求 / The occupation request.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>最终裁决；<see cref="IdempotencyDecisionKind.Execute"/> 表示调用方可执行业务并在结束落定记录 / The final verdict; <see cref="IdempotencyDecisionKind.Execute"/> means the caller may execute the business and settle the record afterwards.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/> / <paramref name="request"/> is <see langword="null"/>.</exception>
    public async Task<IdempotencyDecision> BeginAsync(IdempotencyBeginRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), LocalizationService.GetString(LocalizationKeys.Exceptions.BeginRequestCannotBeNull));
        }

        IdempotencyRecordIdentifier identifier = new IdempotencyRecordIdentifier(request.IdempotencyScope, request.IdempotencyKey);
        IdempotencyRecord? lastSeenRecord = null;

        // 判定轮次上限为 2：占位被并发抢占后重判一次，仍被抢占则判忙碌。
        // The decision round cap is 2: after an occupation is preempted by a concurrent party, re-judge once;
        // a second preemption resolves to busy.
        const int MaxDecisionRounds = 2;
        for (int decisionRound = 0; decisionRound < MaxDecisionRounds; decisionRound++)
        {
            long nowTime = _clock.UtcNowTime;
            IdempotencyRecord? existingRecord = await _idempotencyStore.TryGetRecordAsync(identifier, cancellationToken);
            if (existingRecord != null)
            {
                lastSeenRecord = existingRecord;
            }

            // 无记录 / 已过期 / 已完结：直接按语义落定；执行中：等待并发占位方落定后重判。
            // Absent / expired / finished: settle directly by the semantics; processing: wait for the concurrent
            // occupant to settle and re-judge.
            IdempotencyDecision? decision;
            if (existingRecord == null || nowTime > existingRecord.ExpiredTime || existingRecord.Status != IdempotencyStatus.Processing)
            {
                decision = await TryJudgeSettledOrVacantAsync(identifier, request.RequestDigest, nowTime, existingRecord, cancellationToken);
            }
            else
            {
                ConcurrentWaitResult waitResult = await WaitForConcurrentSettlementAsync(identifier, request.RequestDigest, existingRecord, nowTime, cancellationToken);
                lastSeenRecord = waitResult.LastSeenRecord;
                decision = waitResult.Decision;
            }

            if (decision != null)
            {
                return decision;
            }

            // 占位被并发方抢先：进入下一轮按语义重判。
            // The occupation was preempted by a concurrent party: proceed to the next round and re-judge by the semantics.
        }

        return new IdempotencyDecision(IdempotencyDecisionKind.Busy, lastSeenRecord);
    }

    /// <summary>
    /// 对一次「无记录 / 已过期 / 已完结」的可落定读数给出裁决 / Judges one settle-able read that is absent, expired or finished.
    /// </summary>
    /// <remarks>
    /// 前置条件：<paramref name="existingRecord"/> 为 <see langword="null"/>、已过期或状态非执行中（由调用方保证）。
    /// 已完结记录先按判定语义表裁决（成功回放 / 键冲突 / 失败回放），失败重执行策略与无记录、已过期一样落入覆盖占位；
    /// 占位被并发方抢先时返回 <see langword="null"/>，由调用方进入下一判定轮次重判。
    /// <para>
    /// Precondition: <paramref name="existingRecord"/> is <see langword="null"/>, expired, or not processing (guaranteed by the caller).
    /// A finished record is judged first by the decision-semantics table (completed-replay / key-conflict / failed-replay);
    /// the failed re-execute policy falls through to an overwriting occupation just like an absent or expired record.
    /// When the occupation is preempted by a concurrent party, <see langword="null"/> is returned for the caller to
    /// re-judge in the next decision round.
    /// </para>
    /// </remarks>
    /// <param name="identifier">幂等记录标识 / The idempotency record identifier.</param>
    /// <param name="requestDigest">本次请求的摘要 / The digest of the current request.</param>
    /// <param name="nowTime">读数时刻（UTC 毫秒）/ The time of the read (UTC milliseconds).</param>
    /// <param name="existingRecord">读到的存量记录，可为 <see langword="null"/> / The read existing record; may be <see langword="null"/>.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>最终裁决；占位被并发方抢先时为 <see langword="null"/> / The final verdict; <see langword="null"/> when the occupation is preempted by a concurrent party.</returns>
    private async Task<IdempotencyDecision?> TryJudgeSettledOrVacantAsync(IdempotencyRecordIdentifier identifier, string requestDigest, long nowTime, IdempotencyRecord? existingRecord, CancellationToken cancellationToken)
    {
        // 已完结（前置条件已排除执行中）：成功回放 / 键冲突 / 失败回放在此裁决，失败重执行返回 null 落入覆盖占位。
        // Finished (processing excluded by the precondition): completed-replay / key-conflict / failed-replay settle here;
        // failed re-execute returns null and falls through to the overwriting occupation.
        if (existingRecord != null && nowTime <= existingRecord.ExpiredTime)
        {
            IdempotencyDecision? judgedDecision = JudgeFinishedRecord(existingRecord, requestDigest);
            if (judgedDecision != null)
            {
                return judgedDecision;
            }
        }

        // 无记录 / 已过期 / 失败重执行：视为可占位，新占位覆盖旧记录。
        // Absent / expired / failed re-execute: occupy — a new occupation overwrites the old record.
        if (await TryOccupyAsync(identifier, requestDigest, nowTime, cancellationToken))
        {
            return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
        }

        return null;
    }

    /// <summary>
    /// 等待执行中的并发占位方落定，状态变化即重判 / Waits for the processing concurrent occupant to settle and re-judges on any state change.
    /// </summary>
    /// <remarks>
    /// 在并发等待超时内周期性重读记录；一旦读到可落定状态（无记录 / 已过期 / 已完结）即交由
    /// <see cref="TryJudgeSettledOrVacantAsync"/> 裁决，读数仍为执行中则继续等待下一轮询周期；
    /// 等待超时返回携带触发等待记录的 <see cref="IdempotencyDecisionKind.Busy"/>（不等待轮询间隙外的额外落定）。
    /// 等待期间最后见到的非空记录经 <see cref="ConcurrentWaitResult.LastSeenRecord"/> 回传，供调用方维持「最后读数」语义。
    /// <para>
    /// Within the concurrent wait timeout the record is re-read periodically; as soon as a settle-able state is re-read
    /// (absent / expired / finished) the verdict is delegated to <see cref="TryJudgeSettledOrVacantAsync"/>, while a
    /// still-processing read keeps waiting for the next polling cycle. A wait timeout returns
    /// <see cref="IdempotencyDecisionKind.Busy"/> carrying the record that triggered the wait. The latest non-null
    /// record seen during the wait is passed back via <see cref="ConcurrentWaitResult.LastSeenRecord"/> so the caller
    /// can maintain the last-seen semantics.
    /// </para>
    /// </remarks>
    /// <param name="identifier">幂等记录标识 / The idempotency record identifier.</param>
    /// <param name="requestDigest">本次请求的摘要 / The digest of the current request.</param>
    /// <param name="processingRecord">触发等待的执行中存量记录，超时 Busy 决策携带它 / The processing existing record that triggered the wait; carried by the timeout Busy decision.</param>
    /// <param name="waitStartTime">等待起点时刻（UTC 毫秒），超时截止 = 起点 + 并发等待超时 / The wait start time (UTC milliseconds); the timeout deadline = start + concurrent wait timeout.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>并发等待结果：裁决与等待期间最后见到的非空记录 / The concurrent-wait result: the verdict and the latest non-null record seen during the wait.</returns>
    private async Task<ConcurrentWaitResult> WaitForConcurrentSettlementAsync(IdempotencyRecordIdentifier identifier, string requestDigest, IdempotencyRecord processingRecord, long waitStartTime, CancellationToken cancellationToken)
    {
        long waitDeadlineTime = waitStartTime + _idempotencyOptions.ConcurrentWaitTimeoutMilliseconds;
        IdempotencyRecord? lastSeenDuringWaitRecord = processingRecord;
        while (true)
        {
            long currentNowTime = _clock.UtcNowTime;
            if (currentNowTime >= waitDeadlineTime)
            {
                return new ConcurrentWaitResult(new IdempotencyDecision(IdempotencyDecisionKind.Busy, processingRecord), lastSeenDuringWaitRecord);
            }

            long delayMilliseconds = ConcurrentWaitPollIntervalMilliseconds;
            long remainingMilliseconds = waitDeadlineTime - currentNowTime;
            if (delayMilliseconds > remainingMilliseconds)
            {
                delayMilliseconds = remainingMilliseconds;
            }

            await Task.Delay((int)delayMilliseconds, cancellationToken);

            IdempotencyRecord? recheckedRecord = await _idempotencyStore.TryGetRecordAsync(identifier, cancellationToken);
            long recheckNowTime = _clock.UtcNowTime;
            if (recheckedRecord != null)
            {
                lastSeenDuringWaitRecord = recheckedRecord;
            }

            // 仍为执行中且未过期：继续等待下一轮询周期；否则（无记录 / 已过期 / 已完结）按语义落定。
            // Still processing and unexpired: keep waiting for the next polling cycle; otherwise
            // (absent / expired / finished) settle by the semantics.
            if (recheckedRecord == null || recheckNowTime > recheckedRecord.ExpiredTime || recheckedRecord.Status != IdempotencyStatus.Processing)
            {
                IdempotencyDecision? decision = await TryJudgeSettledOrVacantAsync(identifier, requestDigest, recheckNowTime, recheckedRecord, cancellationToken);
                return new ConcurrentWaitResult(decision, lastSeenDuringWaitRecord);
            }
        }
    }

    /// <summary>
    /// 将占位记录落为成功并固化首次响应 / Settles the occupied record as completed and persists the first response.
    /// </summary>
    /// <remarks>
    /// 完成时刻由协调器时钟取值并覆盖请求中的同名字段，调用方无需自行对时；
    /// 仅应在 <see cref="BeginAsync"/> 返回 <see cref="IdempotencyDecisionKind.Execute"/> 且业务执行成功后调用。
    /// <para>
    /// The completion time is sampled from the coordinator clock and overwrites the request's same-named field,
    /// so callers need no clock alignment of their own; this method should only be called after
    /// <see cref="BeginAsync"/> returned <see cref="IdempotencyDecisionKind.Execute"/> and the business succeeded.
    /// </para>
    /// </remarks>
    /// <param name="request">完成请求 / The completion request.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/> / <paramref name="request"/> is <see langword="null"/>.</exception>
    public Task CompleteAsync(IdempotencyCompletionRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), LocalizationService.GetString(LocalizationKeys.Exceptions.CompletionRequestCannotBeNull));
        }

        IdempotencyCompletionRequest forwardedRequest = new IdempotencyCompletionRequest(request.Identifier, request.FirstResponse, _clock.UtcNowTime);
        return _idempotencyStore.CompleteAsync(forwardedRequest, cancellationToken);
    }

    /// <summary>
    /// 将占位记录落为失败 / Settles the occupied record as failed.
    /// </summary>
    /// <remarks>
    /// 失败时刻由协调器时钟取值并覆盖请求中的同名字段，调用方无需自行对时；
    /// 仅应在 <see cref="BeginAsync"/> 返回 <see cref="IdempotencyDecisionKind.Execute"/> 且业务执行失败后调用。
    /// <para>
    /// The failure time is sampled from the coordinator clock and overwrites the request's same-named field,
    /// so callers need no clock alignment of their own; this method should only be called after
    /// <see cref="BeginAsync"/> returned <see cref="IdempotencyDecisionKind.Execute"/> and the business failed.
    /// </para>
    /// </remarks>
    /// <param name="request">失败请求 / The failure request.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> 为 <see langword="null"/> / <paramref name="request"/> is <see langword="null"/>.</exception>
    public Task FailAsync(IdempotencyFailureRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), LocalizationService.GetString(LocalizationKeys.Exceptions.FailureRequestCannotBeNull));
        }

        IdempotencyFailureRequest forwardedRequest = new IdempotencyFailureRequest(request.Identifier, _clock.UtcNowTime);
        return _idempotencyStore.FailAsync(forwardedRequest, cancellationToken);
    }

    /// <summary>
    /// 判定一条已完结（成功或失败）的存量记录 / Judges a finished (completed or failed) existing record.
    /// </summary>
    /// <remarks>
    /// 对应判定语义表的成功回放、键冲突、失败回放与失败重执行分支。
    /// <para>
    /// Covers the completed-replay, key-conflict, failed-replay and failed-re-execute branches of the
    /// decision-semantics table.
    /// </para>
    /// </remarks>
    /// <param name="existingRecord">已完结的存量记录 / The finished existing record.</param>
    /// <param name="requestDigest">本次请求的摘要 / The digest of the current request.</param>
    /// <returns>回放或冲突决策；失败策略为重新执行时返回 <see langword="null"/>（调用方应覆盖占位）/ The replay or conflict decision; <see langword="null"/> when the failed policy is re-execute (the caller should overwrite-occupy).</returns>
    private IdempotencyDecision? JudgeFinishedRecord(IdempotencyRecord existingRecord, string requestDigest)
    {
        if (existingRecord.Status == IdempotencyStatus.Completed)
        {
            if (string.Equals(existingRecord.RequestDigest, requestDigest, StringComparison.Ordinal))
            {
                return new IdempotencyDecision(IdempotencyDecisionKind.Replay, existingRecord);
            }

            return new IdempotencyDecision(IdempotencyDecisionKind.Conflict, existingRecord);
        }

        if (existingRecord.Status == IdempotencyStatus.Failed)
        {
            if (_idempotencyOptions.FailedReplayPolicy == FailedReplayPolicy.ReplayError)
            {
                return new IdempotencyDecision(IdempotencyDecisionKind.Replay, existingRecord);
            }

            return null;
        }

        return null;
    }

    /// <summary>
    /// 以执行中状态尝试占位幂等标识 / Tries to occupy the idempotency identifier with a processing record.
    /// </summary>
    /// <remarks>
    /// 新记录的过期时刻 = 当前时刻 + 保留时长。
    /// <para>The new record's expiration time = the current time + the retention duration.</para>
    /// </remarks>
    /// <param name="identifier">幂等记录标识 / The idempotency record identifier.</param>
    /// <param name="requestDigest">请求摘要 / The request digest.</param>
    /// <param name="nowTime">当前时刻（UTC 毫秒）/ The current time (UTC milliseconds).</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>占位成功返回 <see langword="true"/>；被并发方抢先返回 <see langword="false"/> / <see langword="true"/> if the occupation succeeds; <see langword="false"/> if preempted by a concurrent party.</returns>
    private async Task<bool> TryOccupyAsync(IdempotencyRecordIdentifier identifier, string requestDigest, long nowTime, CancellationToken cancellationToken)
    {
        IdempotencyRecord newRecord = new IdempotencyRecord(identifier.IdempotencyScope, identifier.IdempotencyKey, requestDigest, IdempotencyStatus.Processing, null, nowTime, nowTime + _idempotencyOptions.RetentionMilliseconds, null);
        return await _idempotencyStore.TryBeginProcessingAsync(newRecord, cancellationToken);
    }

    /// <summary>
    /// 并发等待结果：等待落定的裁决与等待期间最后见到的非空记录 / The concurrent-wait result: the settled verdict and the latest non-null record seen during the wait.
    /// </summary>
    /// <remarks>
    /// 供 <see cref="BeginAsync"/> 维持「最后读数」语义：等待轮询中的非空读数也计入 lastSeenRecord，
    /// 两轮判定均被抢占时的 Busy 决策携带的是全局最后读数，而非等待触发时的旧记录。
    /// <para>
    /// Lets <see cref="BeginAsync"/> maintain the last-seen semantics: non-null reads during the wait polling also
    /// count toward lastSeenRecord, so the Busy decision after both decision rounds are preempted carries the
    /// globally latest read, not the stale record from when the wait was triggered.
    /// </para>
    /// </remarks>
    private sealed class ConcurrentWaitResult
    {
        /// <summary>
        /// 初始化 <see cref="ConcurrentWaitResult"/> / Initializes <see cref="ConcurrentWaitResult"/>.
        /// </summary>
        /// <param name="decision">等待落定的裁决；占位被并发方抢先时为 <see langword="null"/> / The settled wait verdict; <see langword="null"/> when the occupation was preempted by a concurrent party.</param>
        /// <param name="lastSeenRecord">等待期间最后见到的非空记录（含触发等待的记录）/ The latest non-null record seen during the wait, including the one that triggered the wait.</param>
        public ConcurrentWaitResult(IdempotencyDecision? decision, IdempotencyRecord? lastSeenRecord)
        {
            Decision = decision;
            LastSeenRecord = lastSeenRecord;
        }

        /// <summary>
        /// 等待落定的裁决；<see langword="null"/> 表示占位被并发方抢先，调用方应进入下一判定轮次 / The settled wait verdict; <see langword="null"/> means the occupation was preempted and the caller should enter the next decision round.
        /// </summary>
        public IdempotencyDecision? Decision
        {
            get;
        }

        /// <summary>
        /// 等待期间最后见到的非空记录（含触发等待的记录）/ The latest non-null record seen during the wait, including the one that triggered the wait.
        /// </summary>
        public IdempotencyRecord? LastSeenRecord
        {
            get;
        }
    }
}
