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
/// 幂等记录：一次业务意图在存储中的不可变快照 / Idempotency record: an immutable snapshot of one business intent in storage.
/// </summary>
/// <remarks>
/// 由 <c>作用域 + 键</c> 唯一标识，承载占位、判定与回放所需的全部状态。
/// <para>
/// <see cref="FirstResponse"/> 为不透明只读字节：本类型不解释其内容、不提供写入路径；
/// 构造方传入后即完成所有权移交，不得再保留可写引用或修改底层缓冲。
/// </para>
/// <para>构造校验 <see cref="IdempotencyScope"/>、<see cref="IdempotencyKey"/>、<see cref="RequestDigest"/> 非空白，非法时抛出 <see cref="ArgumentException"/>。</para>
/// <para>
/// Uniquely identified by <c>scope + key</c>, it carries all state needed for occupation, judgement and replay.
/// <see cref="FirstResponse"/> is opaque read-only bytes: this type neither interprets its content nor offers a write path;
/// once the constructor receives it, ownership is transferred and the caller must not keep a writable reference or mutate the underlying buffer.
/// The constructor validates that <see cref="IdempotencyScope"/>, <see cref="IdempotencyKey"/> and <see cref="RequestDigest"/> are non-blank,
/// throwing <see cref="ArgumentException"/> otherwise.
/// </para>
/// </remarks>
public sealed class IdempotencyRecord
{
    /// <summary>
    /// 初始化 <see cref="IdempotencyRecord"/> / Initializes <see cref="IdempotencyRecord"/>.
    /// </summary>
    /// <param name="idempotencyScope">幂等作用域，对包不透明的字符串，由使用方定义格式并负责隔离 / The idempotency scope, opaque to this package; the consumer defines its format and is responsible for isolation.</param>
    /// <param name="idempotencyKey">幂等键，标识一次业务意图 / The idempotency key identifying one business intent.</param>
    /// <param name="requestDigest">请求摘要，用于识别同一键下的请求内容是否一致 / The request digest used to tell whether the request content under the same key is identical.</param>
    /// <param name="status">记录状态 / The record status.</param>
    /// <param name="firstResponse">首次响应的不透明只读字节，仅 <see cref="IdempotencyStatus.Completed"/> 时有值；传入后所有权移交本记录 / The opaque read-only bytes of the first response, present only for <see cref="IdempotencyStatus.Completed"/>; ownership transfers to this record once passed in.</param>
    /// <param name="createdTime">记录创建时刻（UTC 毫秒）/ The record creation time (UTC milliseconds).</param>
    /// <param name="expiredTime">记录过期时刻（UTC 毫秒），由协调器按保留时长计算 / The record expiration time (UTC milliseconds), computed by the coordinator from the retention duration.</param>
    /// <param name="finishedTime">执行结束时刻（UTC 毫秒），执行中为 <see langword="null"/> / The execution finish time (UTC milliseconds); <see langword="null"/> while processing.</param>
    /// <exception cref="ArgumentException"><paramref name="idempotencyScope"/>、<paramref name="idempotencyKey"/> 或 <paramref name="requestDigest"/> 为空白 / <paramref name="idempotencyScope"/>, <paramref name="idempotencyKey"/> or <paramref name="requestDigest"/> is blank.</exception>
    public IdempotencyRecord(string idempotencyScope, string idempotencyKey, string requestDigest, IdempotencyStatus status, ReadOnlyMemory<byte>? firstResponse, long createdTime, long expiredTime, long? finishedTime)
    {
        if (string.IsNullOrWhiteSpace(idempotencyScope))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IdempotencyScopeCannotBeBlank), nameof(idempotencyScope));
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IdempotencyKeyCannotBeBlank), nameof(idempotencyKey));
        }

        if (string.IsNullOrWhiteSpace(requestDigest))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.RequestDigestCannotBeBlank), nameof(requestDigest));
        }

        IdempotencyScope = idempotencyScope;
        IdempotencyKey = idempotencyKey;
        RequestDigest = requestDigest;
        Status = status;
        FirstResponse = firstResponse;
        CreatedTime = createdTime;
        ExpiredTime = expiredTime;
        FinishedTime = finishedTime;
    }

    /// <summary>
    /// 幂等作用域，对包不透明的字符串 / The idempotency scope, a string opaque to this package.
    /// </summary>
    /// <remarks>
    /// 格式由使用方定义（例如拼接业务隔离维度），本包不解析、不理解其内部结构；
    /// 作用域之间的隔离由使用方负责。
    /// <para>
    /// The format is consumer-defined (for example by concatenating business isolation dimensions);
    /// this package neither parses it nor understands its internal structure.
    /// Isolation between scopes is the consumer's responsibility.
    /// </para>
    /// </remarks>
    public string IdempotencyScope
    {
        get;
    }

    /// <summary>
    /// 幂等键，标识一次业务意图 / The idempotency key identifying one business intent.
    /// </summary>
    /// <remarks>
    /// 与 <see cref="IdempotencyScope"/> 联合唯一定位一条幂等记录；
    /// 可由 <see cref="IIdempotencyKeyGenerator"/> 生成或由业务键充当。
    /// <para>
    /// Together with <see cref="IdempotencyScope"/> it uniquely locates an idempotency record;
    /// it can be generated by <see cref="IIdempotencyKeyGenerator"/> or provided by a business key.
    /// </para>
    /// </remarks>
    public string IdempotencyKey
    {
        get;
    }

    /// <summary>
    /// 请求摘要 / The request digest.
    /// </summary>
    /// <remarks>
    /// 标识同一键下请求内容的一致性：摘要相同视为同一意图（可重放），不同视为键冲突。
    /// 可由 <see cref="IRequestDigester"/> 对规范化文本计算得出。
    /// <para>
    /// Indicates content consistency under the same key: an identical digest means the same intent (replayable),
    /// a different digest means a key conflict. It can be computed from canonical text by <see cref="IRequestDigester"/>.
    /// </para>
    /// </remarks>
    public string RequestDigest
    {
        get;
    }

    /// <summary>
    /// 记录状态 / The record status.
    /// </summary>
    public IdempotencyStatus Status
    {
        get;
    }

    /// <summary>
    /// 首次响应的不透明只读字节 / The opaque read-only bytes of the first response.
    /// </summary>
    /// <remarks>
    /// 仅 <see cref="IdempotencyStatus.Completed"/> 时有值；本类型不解释内容、无写入路径，
    /// 构造方传入后不得保留可写引用或修改底层缓冲（所有权移交契约）。
    /// <para>
    /// Present only for <see cref="IdempotencyStatus.Completed"/>; this type neither interprets the content
    /// nor offers a write path. After construction the caller must not keep a writable reference or mutate
    /// the underlying buffer (ownership-transfer contract).
    /// </para>
    /// </remarks>
    public ReadOnlyMemory<byte>? FirstResponse
    {
        get;
    }

    /// <summary>
    /// 记录创建时刻（UTC 毫秒）/ The record creation time (UTC milliseconds).
    /// </summary>
    public long CreatedTime
    {
        get;
    }

    /// <summary>
    /// 记录过期时刻（UTC 毫秒）/ The record expiration time (UTC milliseconds).
    /// </summary>
    /// <remarks>
    /// 等于创建时刻加保留时长；当前时刻超过该值后记录视为不存在，可被新占位覆盖。
    /// <para>
    /// Equals the creation time plus the retention duration; once the current time passes this value,
    /// the record is treated as absent and may be overwritten by a new occupation.
    /// </para>
    /// </remarks>
    public long ExpiredTime
    {
        get;
    }

    /// <summary>
    /// 执行结束时刻（UTC 毫秒），执行中为 <see langword="null"/> / The execution finish time (UTC milliseconds); <see langword="null"/> while processing.
    /// </summary>
    /// <remarks>
    /// 记录落为 <see cref="IdempotencyStatus.Completed"/> 或 <see cref="IdempotencyStatus.Failed"/> 的时间。
    /// <para>The time at which the record was settled as <see cref="IdempotencyStatus.Completed"/> or <see cref="IdempotencyStatus.Failed"/>.</para>
    /// </remarks>
    public long? FinishedTime
    {
        get;
    }
}
