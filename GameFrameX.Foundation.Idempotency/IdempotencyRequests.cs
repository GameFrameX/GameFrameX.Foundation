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
/// 幂等记录标识：作用域与键的组合，唯一定位一条幂等记录 / Idempotency record identifier: the combination of scope and key that uniquely locates a record.
/// </summary>
/// <remarks>
/// 跨层传递的不可变参数对象；后续新增字段只扩展本类，不改变方法签名。
/// 构造校验作用域与键非空白，非法时抛出 <see cref="ArgumentException"/>。
/// <para>
/// An immutable parameter object passed across layers; future fields extend this class only, never method signatures.
/// The constructor validates that the scope and key are non-blank, throwing <see cref="ArgumentException"/> otherwise.
/// </para>
/// </remarks>
public sealed class IdempotencyRecordIdentifier
{
    /// <summary>
    /// 初始化 <see cref="IdempotencyRecordIdentifier"/> / Initializes <see cref="IdempotencyRecordIdentifier"/>.
    /// </summary>
    /// <param name="idempotencyScope">幂等作用域，对包不透明的字符串 / The idempotency scope, a string opaque to this package.</param>
    /// <param name="idempotencyKey">幂等键 / The idempotency key.</param>
    /// <exception cref="ArgumentException"><paramref name="idempotencyScope"/> 或 <paramref name="idempotencyKey"/> 为空白 / <paramref name="idempotencyScope"/> or <paramref name="idempotencyKey"/> is blank.</exception>
    public IdempotencyRecordIdentifier(string idempotencyScope, string idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyScope))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IdempotencyScopeCannotBeBlank), nameof(idempotencyScope));
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IdempotencyKeyCannotBeBlank), nameof(idempotencyKey));
        }

        IdempotencyScope = idempotencyScope;
        IdempotencyKey = idempotencyKey;
    }

    /// <summary>
    /// 幂等作用域，对包不透明的字符串 / The idempotency scope, a string opaque to this package.
    /// </summary>
    public string IdempotencyScope
    {
        get;
    }

    /// <summary>
    /// 幂等键 / The idempotency key.
    /// </summary>
    public string IdempotencyKey
    {
        get;
    }
}

/// <summary>
/// 幂等占位请求：发起一次业务意图的占位判定 / Idempotency begin request: initiates an occupation judgement for one business intent.
/// </summary>
/// <remarks>
/// 跨层传递的不可变参数对象；构造校验全部字符串字段非空白，非法时抛出 <see cref="ArgumentException"/>。
/// <para>
/// An immutable parameter object passed across layers; the constructor validates that every string field is non-blank,
/// throwing <see cref="ArgumentException"/> otherwise.
/// </para>
/// </remarks>
public sealed class IdempotencyBeginRequest
{
    /// <summary>
    /// 初始化 <see cref="IdempotencyBeginRequest"/> / Initializes <see cref="IdempotencyBeginRequest"/>.
    /// </summary>
    /// <param name="idempotencyScope">幂等作用域，对包不透明的字符串 / The idempotency scope, a string opaque to this package.</param>
    /// <param name="idempotencyKey">幂等键 / The idempotency key.</param>
    /// <param name="requestDigest">请求摘要，用于判定同一键下请求内容是否一致 / The request digest used to tell whether the request content under the same key is identical.</param>
    /// <exception cref="ArgumentException"><paramref name="idempotencyScope"/>、<paramref name="idempotencyKey"/> 或 <paramref name="requestDigest"/> 为空白 / <paramref name="idempotencyScope"/>, <paramref name="idempotencyKey"/> or <paramref name="requestDigest"/> is blank.</exception>
    public IdempotencyBeginRequest(string idempotencyScope, string idempotencyKey, string requestDigest)
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
    }

    /// <summary>
    /// 幂等作用域，对包不透明的字符串 / The idempotency scope, a string opaque to this package.
    /// </summary>
    public string IdempotencyScope
    {
        get;
    }

    /// <summary>
    /// 幂等键 / The idempotency key.
    /// </summary>
    public string IdempotencyKey
    {
        get;
    }

    /// <summary>
    /// 请求摘要 / The request digest.
    /// </summary>
    public string RequestDigest
    {
        get;
    }
}

/// <summary>
/// 幂等完成请求：将占位记录落为成功并固化首次响应 / Idempotency completion request: settles an occupied record as completed and persists the first response.
/// </summary>
/// <remarks>
/// 跨层传递的不可变参数对象。
/// <para><see cref="FirstResponse"/> 为不透明只读字节，构造方传入后所有权移交，不得保留可写引用或修改底层缓冲。</para>
/// <para>
/// An immutable parameter object passed across layers.
/// <see cref="FirstResponse"/> is opaque read-only bytes; ownership transfers to the request once passed in —
/// the caller must not keep a writable reference or mutate the underlying buffer.
/// </para>
/// </remarks>
public sealed class IdempotencyCompletionRequest
{
    /// <summary>
    /// 初始化 <see cref="IdempotencyCompletionRequest"/> / Initializes <see cref="IdempotencyCompletionRequest"/>.
    /// </summary>
    /// <param name="identifier">目标幂等记录标识 / The identifier of the target idempotency record.</param>
    /// <param name="firstResponse">首次响应的不透明只读字节；传入后所有权移交 / The opaque read-only bytes of the first response; ownership transfers once passed in.</param>
    /// <param name="completedTime">完成时刻（UTC 毫秒）。经 <see cref="IdempotencyCoordinator"/> 转发时该字段被协调器时钟覆盖 / The completion time (UTC milliseconds); overwritten by the coordinator clock when forwarded through <see cref="IdempotencyCoordinator"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="identifier"/> 为 <see langword="null"/> / <paramref name="identifier"/> is <see langword="null"/>.</exception>
    public IdempotencyCompletionRequest(IdempotencyRecordIdentifier identifier, ReadOnlyMemory<byte> firstResponse, long completedTime)
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier), LocalizationService.GetString(LocalizationKeys.Exceptions.IdentifierCannotBeNull));
        FirstResponse = firstResponse;
        CompletedTime = completedTime;
    }

    /// <summary>
    /// 目标幂等记录标识 / The identifier of the target idempotency record.
    /// </summary>
    public IdempotencyRecordIdentifier Identifier
    {
        get;
    }

    /// <summary>
    /// 首次响应的不透明只读字节 / The opaque read-only bytes of the first response.
    /// </summary>
    public ReadOnlyMemory<byte> FirstResponse
    {
        get;
    }

    /// <summary>
    /// 完成时刻（UTC 毫秒）/ The completion time (UTC milliseconds).
    /// </summary>
    /// <remarks>
    /// 经 <see cref="IdempotencyCoordinator.CompleteAsync"/> 转发时，该字段被协调器时钟重新取值覆盖，调用方无需自行对时。
    /// <para>
    /// When forwarded through <see cref="IdempotencyCoordinator.CompleteAsync"/> this field is re-sampled from the
    /// coordinator clock, so callers need no clock alignment of their own.
    /// </para>
    /// </remarks>
    public long CompletedTime
    {
        get;
    }
}

/// <summary>
/// 幂等失败请求：将占位记录落为失败 / Idempotency failure request: settles an occupied record as failed.
/// </summary>
/// <remarks>
/// 跨层传递的不可变参数对象。
/// <para>An immutable parameter object passed across layers.</para>
/// </remarks>
public sealed class IdempotencyFailureRequest
{
    /// <summary>
    /// 初始化 <see cref="IdempotencyFailureRequest"/> / Initializes <see cref="IdempotencyFailureRequest"/>.
    /// </summary>
    /// <param name="identifier">目标幂等记录标识 / The identifier of the target idempotency record.</param>
    /// <param name="failedTime">失败时刻（UTC 毫秒）。经 <see cref="IdempotencyCoordinator"/> 转发时该字段被协调器时钟覆盖 / The failure time (UTC milliseconds); overwritten by the coordinator clock when forwarded through <see cref="IdempotencyCoordinator"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="identifier"/> 为 <see langword="null"/> / <paramref name="identifier"/> is <see langword="null"/>.</exception>
    public IdempotencyFailureRequest(IdempotencyRecordIdentifier identifier, long failedTime)
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier), LocalizationService.GetString(LocalizationKeys.Exceptions.IdentifierCannotBeNull));
        FailedTime = failedTime;
    }

    /// <summary>
    /// 目标幂等记录标识 / The identifier of the target idempotency record.
    /// </summary>
    public IdempotencyRecordIdentifier Identifier
    {
        get;
    }

    /// <summary>
    /// 失败时刻（UTC 毫秒）/ The failure time (UTC milliseconds).
    /// </summary>
    /// <remarks>
    /// 经 <see cref="IdempotencyCoordinator.FailAsync"/> 转发时，该字段被协调器时钟重新取值覆盖，调用方无需自行对时。
    /// <para>
    /// When forwarded through <see cref="IdempotencyCoordinator.FailAsync"/> this field is re-sampled from the
    /// coordinator clock, so callers need no clock alignment of their own.
    /// </para>
    /// </remarks>
    public long FailedTime
    {
        get;
    }
}

/// <summary>
/// 过期记录清理请求 / Expired-record removal request.
/// </summary>
/// <remarks>
/// 跨层传递的不可变参数对象；本包不内置清理调度，清理由调用方按需驱动。
/// 预留扩展：批次上限、作用域过滤等字段可在本类上追加，不改变方法签名。
/// <para>
/// An immutable parameter object passed across layers; this package ships no built-in cleanup scheduler —
/// cleanup is driven by the caller on demand. Reserved for extension: fields such as batch limits or scope
/// filters may be appended to this class without changing method signatures.
/// </para>
/// </remarks>
public sealed class ExpiredRecordRemovalRequest
{
    /// <summary>
    /// 初始化 <see cref="ExpiredRecordRemovalRequest"/> / Initializes <see cref="ExpiredRecordRemovalRequest"/>.
    /// </summary>
    /// <param name="nowTime">清理判定所用的当前时刻（UTC 毫秒），仅删除过期时刻早于该值的记录 / The current time (UTC milliseconds) used for the cleanup judgement; only records whose expiration time is earlier than this value are removed.</param>
    public ExpiredRecordRemovalRequest(long nowTime)
    {
        NowTime = nowTime;
    }

    /// <summary>
    /// 清理判定所用的当前时刻（UTC 毫秒）/ The current time (UTC milliseconds) used for the cleanup judgement.
    /// </summary>
    /// <remarks>
    /// 仅删除 <c>ExpiredTime</c> 早于该值的记录；未过期记录不受影响。
    /// <para>Only records whose <c>ExpiredTime</c> is earlier than this value are removed; unexpired records are unaffected.</para>
    /// </remarks>
    public long NowTime
    {
        get;
    }
}
