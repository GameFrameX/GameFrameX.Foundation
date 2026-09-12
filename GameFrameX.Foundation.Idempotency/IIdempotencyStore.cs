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

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 幂等记录存储契约 / The idempotency record store contract.
/// </summary>
/// <remarks>
/// 承载幂等记录的占位、完结、读取与过期清理，是幂等机制唯一需要持久化的协作点。
/// <para>
/// <b>外部实现契约（并发恰一语义，必须保证）：</b>
/// 同一标识（作用域 + 键）的并发 <see cref="TryBeginProcessingAsync"/> 调用中，
/// 存量记录为执行中或已成功（且未过期）时<b>恰好一个</b>调用返回 <see langword="true"/>，其余全部返回 <see langword="false"/>；
/// 若该保证被破坏（同键双占位成功），将导致同一业务意图被执行两次。
/// </para>
/// <para>
/// <b>覆盖占位的两类法定情形：</b>
/// 存量记录已过期（以传入新记录的创建时刻为当前时间基准，过期时刻早于该基准），或
/// 存量记录为 <see cref="IdempotencyStatus.Failed"/>（供失败重执行策略重新占位）——
/// 此时占位必须以新记录覆盖旧记录并返回 <see langword="true"/>。
/// </para>
/// <para>
/// 生产用途的实现必须跨进程、跨重启保持判定结果；必须支持 <see cref="RemoveExpiredRecordsAsync"/>
/// 语义或声明等价机制（例如数据库的过期索引）；签名扩展只允许通过给参数对象追加字段实现。
/// </para>
/// <para>
/// Carries the occupation, settlement, read and expired-cleanup of idempotency records —
/// the only collaboration point of the idempotency mechanism that requires persistence.
/// <b>External implementation contract (exactly-once concurrency, mandatory):</b> among concurrent
/// <see cref="TryBeginProcessingAsync"/> calls on the same identifier (scope + key), while the existing record is
/// processing or completed (and unexpired), <b>exactly one</b> call returns <see langword="true"/> and all others
/// return <see langword="false"/>; breaking this guarantee (two occupations succeeding on the same key) causes the
/// same business intent to execute twice.
/// <b>Two legal overwrite cases:</b> the existing record has expired (judged against the incoming record's creation
/// time as the current-time base), or the existing record is <see cref="IdempotencyStatus.Failed"/> (for the
/// failed-re-execute policy to re-occupy) — in both cases the store must overwrite the old record with the new one
/// and return <see langword="true"/>.
/// Production implementations must preserve judgements across processes and restarts, and must support the
/// <see cref="RemoveExpiredRecordsAsync"/> semantics or declare an equivalent mechanism (for example an expiry index
/// in a database); signatures may only be extended by appending fields to the parameter objects.
/// </para>
/// </remarks>
public interface IIdempotencyStore
{
    /// <summary>
    /// 尝试以新记录占位幂等标识 / Tries to occupy the idempotency identifier with a new record.
    /// </summary>
    /// <remarks>
    /// 并发恰一：同一标识的并发调用中，存量为执行中或已成功（且未过期）时恰好一个成功。
    /// 两类覆盖占位情形：存量已过期（以 <paramref name="newRecord"/> 的创建时刻为基准）、
    /// 存量为 <see cref="IdempotencyStatus.Failed"/>（失败重执行）——此时新记录覆盖旧记录并成功。
    /// <para>
    /// Exactly-once concurrency: among concurrent calls on the same identifier, while the existing record is
    /// processing or completed (and unexpired), exactly one succeeds.
    /// Two overwrite cases: the existing record has expired (judged against <paramref name="newRecord"/>'s creation
    /// time as the base) or is <see cref="IdempotencyStatus.Failed"/> (failed re-execution) — in both cases the new
    /// record overwrites the old one and the occupation succeeds.
    /// </para>
    /// </remarks>
    /// <param name="newRecord">待占位的新记录，状态应为 <see cref="IdempotencyStatus.Processing"/> / The new record to occupy with; its status should be <see cref="IdempotencyStatus.Processing"/>.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>占位成功返回 <see langword="true"/>；标识已被未过期记录占用返回 <see langword="false"/> / <see langword="true"/> if the occupation succeeds; <see langword="false"/> if the identifier is already held by an unexpired record.</returns>
    Task<bool> TryBeginProcessingAsync(IdempotencyRecord newRecord, CancellationToken cancellationToken = default);

    /// <summary>
    /// 将占位记录落为成功并固化首次响应 / Settles the occupied record as completed and persists the first response.
    /// </summary>
    /// <remarks>
    /// 目标记录不存在时实现应抛出异常；调用方保证在占位成功后才调用本方法。
    /// <para>
    /// Implementations should throw when the target record is absent; the caller guarantees this method is
    /// only invoked after a successful occupation.
    /// </para>
    /// </remarks>
    /// <param name="request">完成请求，含首次响应与完成时刻 / The completion request, carrying the first response and the completion time.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    Task CompleteAsync(IdempotencyCompletionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 将占位记录落为失败 / Settles the occupied record as failed.
    /// </summary>
    /// <remarks>
    /// 目标记录不存在时实现应抛出异常；调用方保证在占位成功后才调用本方法。
    /// <para>
    /// Implementations should throw when the target record is absent; the caller guarantees this method is
    /// only invoked after a successful occupation.
    /// </para>
    /// </remarks>
    /// <param name="request">失败请求，含失败时刻 / The failure request, carrying the failure time.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    Task FailAsync(IdempotencyFailureRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 读取幂等记录，含已过期但尚未被清理的记录 / Reads an idempotency record, including expired records not yet cleaned up.
    /// </summary>
    /// <remarks>
    /// 过期判定不在本方法职责内：过期记录仍会返回，由调用方（协调器）结合当前时刻判定是否视为不存在。
    /// <para>
    /// Expiration judgement is out of this method's scope: expired records are still returned, and the caller
    /// (the coordinator) decides whether to treat them as absent based on the current time.
    /// </para>
    /// </remarks>
    /// <param name="identifier">幂等记录标识 / The idempotency record identifier.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>记录存在时返回记录；不存在时返回 <see langword="null"/> / The record when present; <see langword="null"/> when absent.</returns>
    Task<IdempotencyRecord?> TryGetRecordAsync(IdempotencyRecordIdentifier identifier, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除全部过期记录 / Removes all expired records.
    /// </summary>
    /// <remarks>
    /// 仅删除过期时刻早于请求当前时刻的记录，未过期记录不受影响。
    /// <para>
    /// Removes only records whose expiration time is earlier than the request's current time;
    /// unexpired records are unaffected.
    /// </para>
    /// </remarks>
    /// <param name="request">清理请求，含判定所用的当前时刻 / The removal request, carrying the current time used for the judgement.</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token.</param>
    /// <returns>实际删除的记录条数 / The number of records actually removed.</returns>
    Task<int> RemoveExpiredRecordsAsync(ExpiredRecordRemovalRequest request, CancellationToken cancellationToken = default);
}
