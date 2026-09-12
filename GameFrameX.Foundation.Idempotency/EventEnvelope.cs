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

using System.Collections.ObjectModel;
using GameFrameX.Foundation.Idempotency.Localization;
using GameFrameX.Foundation.Localization.Core;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 通用事件信封：跨进程、跨模块传递事件的标准载体 / The universal event envelope: the standard carrier for events crossing processes and modules.
/// </summary>
/// <remarks>
/// 不可变对象。<see cref="EventId"/> 为全局唯一的事件标识，消费端以其去重；
/// <see cref="EventType"/> 与 <see cref="Source"/> 为调用方命名空间字符串，本包不解释其含义；
/// <see cref="Payload"/> 为不透明只读字节，本包不解释、无写入路径。
/// <para>
/// <b>所有权移交契约：</b>构造方传入 <see cref="Payload"/> 后不得保留可写引用或修改底层缓冲；
/// 本类型为避免事件发布热路径上的复制开销，不做构造期防御性拷贝。
/// </para>
/// <para>构造不校验合法性；发布前调用 <see cref="EnsureValid"/> 完成校验，非法字段抛出 <see cref="ArgumentException"/> 并指明字段名。</para>
/// <para>
/// Immutable object. <see cref="EventId"/> is the globally unique event identifier used by consumers for
/// deduplication; <see cref="EventType"/> and <see cref="Source"/> are caller-namespaced strings whose meaning this
/// package does not interpret; <see cref="Payload"/> is opaque read-only bytes — neither interpreted nor writable here.
/// <b>Ownership-transfer contract:</b> after passing in <see cref="Payload"/> the constructor caller must not keep a
/// writable reference or mutate the underlying buffer; to avoid copy overhead on the hot publishing path, this type
/// performs no defensive copy at construction.
/// Construction performs no validation; call <see cref="EnsureValid"/> before publishing — an invalid field throws
/// <see cref="ArgumentException"/> naming the field.
/// </para>
/// </remarks>
public sealed class EventEnvelope
{
    /// <summary>
    /// 初始化 <see cref="EventEnvelope"/>。构造不校验，发布前调用 <see cref="EnsureValid"/> / Initializes <see cref="EventEnvelope"/>. Construction performs no validation; call <see cref="EnsureValid"/> before publishing.
    /// </summary>
    /// <param name="eventId">全局唯一的事件标识，消费端去重键 / The globally unique event identifier used by consumers for deduplication.</param>
    /// <param name="eventType">事件类型，调用方命名空间字符串 / The event type, a caller-namespaced string.</param>
    /// <param name="occurredTime">事件发生时刻（UTC 毫秒）/ The event occurrence time (UTC milliseconds).</param>
    /// <param name="schemaVersion">载荷结构版本，必须不小于 1 / The payload schema version; must be at least 1.</param>
    /// <param name="source">来源模块标识 / The source module identifier.</param>
    /// <param name="correlationId">关联链路键，可为 <see langword="null"/> / The correlation key; may be <see langword="null"/>.</param>
    /// <param name="payload">事件载荷的不透明只读字节；传入后所有权移交 / The opaque read-only bytes of the event payload; ownership transfers once passed in.</param>
    /// <param name="attributes">通用键值元数据；<see langword="null"/> 视为空集合，传入后本信封持有独立快照、外部修改不影响信封 / General key-value metadata; <see langword="null"/> is treated as empty. The envelope keeps an independent snapshot — later external changes do not affect it.</param>
    public EventEnvelope(string eventId, string eventType, long occurredTime, int schemaVersion, string source, string? correlationId, ReadOnlyMemory<byte> payload, IReadOnlyDictionary<string, string>? attributes)
    {
        EventId = eventId;
        EventType = eventType;
        OccurredTime = occurredTime;
        SchemaVersion = schemaVersion;
        Source = source;
        CorrelationId = correlationId;
        Payload = payload;
        Attributes = BuildReadOnlyAttributes(attributes);
    }

    /// <summary>
    /// 全局唯一的事件标识，消费端去重键 / The globally unique event identifier used by consumers for deduplication.
    /// </summary>
    public string EventId
    {
        get;
    }

    /// <summary>
    /// 事件类型，调用方命名空间字符串，本包不解释 / The event type, a caller-namespaced string this package does not interpret.
    /// </summary>
    public string EventType
    {
        get;
    }

    /// <summary>
    /// 事件发生时刻（UTC 毫秒）/ The event occurrence time (UTC milliseconds).
    /// </summary>
    public long OccurredTime
    {
        get;
    }

    /// <summary>
    /// 载荷结构版本，合法值不小于 1 / The payload schema version; valid values are at least 1.
    /// </summary>
    public int SchemaVersion
    {
        get;
    }

    /// <summary>
    /// 来源模块标识 / The source module identifier.
    /// </summary>
    public string Source
    {
        get;
    }

    /// <summary>
    /// 关联链路键，未参与链路时为 <see langword="null"/> / The correlation key; <see langword="null"/> when not part of a chain.
    /// </summary>
    public string? CorrelationId
    {
        get;
    }

    /// <summary>
    /// 事件载荷的不透明只读字节 / The opaque read-only bytes of the event payload.
    /// </summary>
    /// <remarks>
    /// 本包不解释内容、无写入路径；构造方传入后不得保留可写引用或修改底层缓冲（所有权移交契约）。
    /// <para>
    /// This package neither interprets the content nor offers a write path; after construction the caller must not
    /// keep a writable reference or mutate the underlying buffer (ownership-transfer contract).
    /// </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Payload
    {
        get;
    }

    /// <summary>
    /// 通用键值元数据的只读视图 / The read-only view of the general key-value metadata.
    /// </summary>
    /// <remarks>
    /// 与本信封构造入参完全隔离：外部对原字典的后续修改不影响本视图。
    /// <para>
    /// Fully isolated from the constructor input: later external changes to the original dictionary
    /// do not affect this view.
    /// </para>
    /// </remarks>
    public IReadOnlyDictionary<string, string> Attributes
    {
        get;
    }

    /// <summary>
    /// 校验信封全部字段的合法性 / Validates every field of the envelope.
    /// </summary>
    /// <remarks>
    /// 校验分支：<see cref="EventId"/>、<see cref="EventType"/>、<see cref="Source"/> 非空白；
    /// <see cref="SchemaVersion"/> 不小于 1。<see cref="CorrelationId"/> 可为 <see langword="null"/>、
    /// <see cref="Payload"/> 任意长度均合法。发布器在分发前必须调用本方法。
    /// <para>
    /// Validation branches: <see cref="EventId"/>, <see cref="EventType"/> and <see cref="Source"/> must be non-blank;
    /// <see cref="SchemaVersion"/> must be at least 1. <see cref="CorrelationId"/> may be <see langword="null"/>, and
    /// <see cref="Payload"/> of any length is valid. Publishers must call this method before dispatching.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">任一字段非法，异常消息指明字段名 / Any field is invalid; the exception message names the field.</exception>
    public void EnsureValid()
    {
        if (string.IsNullOrWhiteSpace(EventId))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.EventIdCannotBeBlank), nameof(EventId));
        }

        if (string.IsNullOrWhiteSpace(EventType))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.EventTypeCannotBeBlank), nameof(EventType));
        }

        if (string.IsNullOrWhiteSpace(Source))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.SourceCannotBeBlank), nameof(Source));
        }

        if (SchemaVersion < 1)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.SchemaVersionMustBeAtLeastOne), nameof(SchemaVersion));
        }
    }

    private static IReadOnlyDictionary<string, string> BuildReadOnlyAttributes(IReadOnlyDictionary<string, string>? attributes)
    {
        if (attributes == null || attributes.Count == 0)
        {
            return new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        }

        Dictionary<string, string> attributesSnapshot = new Dictionary<string, string>(attributes);
        return new ReadOnlyDictionary<string, string>(attributesSnapshot);
    }
}
