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
/// 组合全部幂等协作点（存储、时钟、选项、键生成器、摘要器），对一次占位请求给出最终裁决。
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
/// Composes every idempotency collaboration point (store, clock, options, key generator, digester) and delivers
/// the final verdict for one occupation request.
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
    private readonly IIdempotencyKeyGenerator _idempotencyKeyGenerator;
    private readonly IRequestDigester _requestDigester;

    /// <summary>
    /// 初始化 <see cref="IdempotencyCoordinator"/> / Initializes <see cref="IdempotencyCoordinator"/>.
    /// </summary>
    /// <param name="idempotencyStore">幂等记录存储 / The idempotency record store.</param>
    /// <param name="clock">时间源 / The time source.</param>
    /// <param name="idempotencyOptions">幂等选项，<see langword="null"/> 时使用全部默认值 / The idempotency options; <see langword="null"/> uses all defaults.</param>
    /// <param name="idempotencyKeyGenerator">幂等键生成器，<see langword="null"/> 时使用 <see cref="GuidIdempotencyKeyGenerator.Instance"/> / The idempotency key generator; <see langword="null"/> uses <see cref="GuidIdempotencyKeyGenerator.Instance"/>.</param>
    /// <param name="requestDigester">请求摘要器，<see langword="null"/> 时使用 <see cref="Sha256RequestDigester.Instance"/> / The request digester; <see langword="null"/> uses <see cref="Sha256RequestDigester.Instance"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="idempotencyStore"/> 或 <paramref name="clock"/> 为 <see langword="null"/> / <paramref name="idempotencyStore"/> or <paramref name="clock"/> is <see langword="null"/>.</exception>
    public IdempotencyCoordinator(IIdempotencyStore idempotencyStore, IClock clock, IdempotencyOptions? idempotencyOptions = null, IIdempotencyKeyGenerator? idempotencyKeyGenerator = null, IRequestDigester? requestDigester = null)
    {
        _idempotencyStore = idempotencyStore ?? throw new ArgumentNullException(nameof(idempotencyStore), LocalizationService.GetString(LocalizationKeys.Exceptions.StoreCannotBeNull));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock), LocalizationService.GetString(LocalizationKeys.Exceptions.ClockCannotBeNull));
        _idempotencyOptions = idempotencyOptions ?? new IdempotencyOptions();
        _idempotencyKeyGenerator = idempotencyKeyGenerator ?? GuidIdempotencyKeyGenerator.Instance;
        _requestDigester = requestDigester ?? Sha256RequestDigester.Instance;
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
            if (existingRecord == null)
            {
                if (await TryOccupyAsync(identifier, request.RequestDigest, nowTime, cancellationToken))
                {
                    return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
                }

                continue;
            }

            lastSeenRecord = existingRecord;

            // 已过期（当前时刻晚于过期时刻）：视为无记录，新占位覆盖。
            // Expired (current time later than the expiration time): treat as no record and overwrite with a new occupation.
            if (nowTime > existingRecord.ExpiredTime)
            {
                if (await TryOccupyAsync(identifier, request.RequestDigest, nowTime, cancellationToken))
                {
                    return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
                }

                continue;
            }

            if (existingRecord.Status != IdempotencyStatus.Processing)
            {
                IdempotencyDecision? judgedDecision = JudgeFinishedRecord(existingRecord, request.RequestDigest);
                if (judgedDecision != null)
                {
                    return judgedDecision;
                }

                // 失败重放策略为重新执行：覆盖占位。
                // Failed-replay policy is re-execute: overwrite-occupy.
                if (await TryOccupyAsync(identifier, request.RequestDigest, nowTime, cancellationToken))
                {
                    return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
                }

                continue;
            }

            // 执行中：在并发等待超时内周期性重读记录，状态变化即按语义重判。
            // Processing: within the concurrent wait timeout, re-read the record periodically and re-judge
            // by the semantics as soon as the state changes.
            long waitDeadlineTime = nowTime + _idempotencyOptions.ConcurrentWaitTimeoutMilliseconds;
            bool preemptedDuringWait = false;
            while (!preemptedDuringWait)
            {
                long currentNowTime = _clock.UtcNowTime;
                if (currentNowTime >= waitDeadlineTime)
                {
                    return new IdempotencyDecision(IdempotencyDecisionKind.Busy, existingRecord);
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

                if (recheckedRecord == null)
                {
                    if (await TryOccupyAsync(identifier, request.RequestDigest, recheckNowTime, cancellationToken))
                    {
                        return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
                    }

                    preemptedDuringWait = true;
                    continue;
                }

                lastSeenRecord = recheckedRecord;

                if (recheckNowTime > recheckedRecord.ExpiredTime)
                {
                    if (await TryOccupyAsync(identifier, request.RequestDigest, recheckNowTime, cancellationToken))
                    {
                        return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
                    }

                    preemptedDuringWait = true;
                    continue;
                }

                if (recheckedRecord.Status == IdempotencyStatus.Processing)
                {
                    continue;
                }

                IdempotencyDecision? judgedAfterWaitDecision = JudgeFinishedRecord(recheckedRecord, request.RequestDigest);
                if (judgedAfterWaitDecision != null)
                {
                    return judgedAfterWaitDecision;
                }

                if (await TryOccupyAsync(identifier, request.RequestDigest, recheckNowTime, cancellationToken))
                {
                    return new IdempotencyDecision(IdempotencyDecisionKind.Execute);
                }

                preemptedDuringWait = true;
            }
        }

        return new IdempotencyDecision(IdempotencyDecisionKind.Busy, lastSeenRecord);
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
}
