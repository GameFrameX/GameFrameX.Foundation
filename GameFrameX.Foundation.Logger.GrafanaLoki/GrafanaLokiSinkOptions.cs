// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes or liabilities arising from secondary development based on this project
//  本组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository: https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Json;
using Serilog.Events;

namespace GameFrameX.Foundation.Logger.GrafanaLoki;

/// <summary>
/// Grafana Loki 日志 sink 的组件配置选项，由 <see cref="LogOptionsGrafanaLokiExtensions.UseGrafanaLoki"/> 注册到日志系统。
/// </summary>
/// <remarks>
/// Component configuration for the Grafana Loki logging sink, registered into the logging system via <see cref="LogOptionsGrafanaLokiExtensions.UseGrafanaLoki"/>.
/// 自 C77 起 Loki 接入参数从核心包 <c>LogOptions</c> 迁入本组件包，核心包不再依赖 Serilog.Sinks.Grafana.Loki。
/// </remarks>
public sealed class GrafanaLokiSinkOptions
{
    private int _period = 2;

    /// <summary>
    /// GrafanaLoki 服务地址，默认为 http://localhost:3100。 / GrafanaLoki service address, default is http://localhost:3100.
    /// </summary>
    public string Url { get; set; } = "http://localhost:3100";

    /// <summary>
    /// GrafanaLoki 标签字典，键值对转换为 sink 的 Loki 标签。 / GrafanaLoki label dictionary, converted to the sink's Loki labels.
    /// </summary>
    /// <remarks>
    /// <para>设置为 null 时按空标签处理，不装配失败。</para>
    /// <para>Set to null to treat as no labels; sink wiring does not fail.</para>
    /// </remarks>
    public Dictionary<string, string> Labels { get; set; } = new();

    /// <summary>
    /// GrafanaLoki 附加属性字典，非空键值对将 Enrich 到全部日志事件。 / GrafanaLoki additional properties; non-empty pairs are enriched onto all log events.
    /// </summary>
    /// <remarks>
    /// <para>键或值为空白字符的条目会被跳过（语义自原 LogHandler.ApplyLokiProperties 迁移）。</para>
    /// <para>Entries with a whitespace key or value are skipped (semantics migrated from the original LogHandler.ApplyLokiProperties).</para>
    /// </remarks>
    public Dictionary<string, string> Properties { get; set; } = new();

    /// <summary>
    /// GrafanaLoki 用户名，与密码一起构成 Basic 认证凭证。 / GrafanaLoki username, combined with the password for Basic authentication credentials.
    /// </summary>
    /// <remarks>
    /// 用户名或密码任一为空时不启用认证。 / Authentication is disabled when either the username or the password is empty.
    /// </remarks>
    public string UserName { get; set; }

    /// <summary>
    /// GrafanaLoki 密码。 / GrafanaLoki password.
    /// </summary>
    /// <remarks>
    /// <para>敏感字段：<see cref="ToString"/> 输出中固定脱敏为 "***"。</para>
    /// <para>Sensitive field: always redacted to "***" in <see cref="ToString"/> output.</para>
    /// </remarks>
    public string Password { get; set; }

    /// <summary>
    /// 是否启用 GrafanaLoki 数据压缩（Gzip），默认为 true。 / Whether to enable GrafanaLoki payload compression (Gzip), default is true.
    /// </summary>
    /// <remarks>
    /// 启用压缩可以减少网络传输的数据量，提高传输效率。 / Enabling compression reduces the transferred payload size and improves transmission efficiency.
    /// </remarks>
    public bool CompressionEnabled { get; set; } = true;

    /// <summary>
    /// 单次批量推送的最大日志条数，默认为 1000。 / Maximum number of log entries per batch push, default is 1000.
    /// </summary>
    /// <remarks>
    /// 攒满该数量即触发一次推送；未攒满时由 <see cref="Period"/> 周期触发。 / A push is triggered once this count is reached; otherwise it is triggered by the <see cref="Period"/> timer.
    /// </remarks>
    public int BatchSizeLimit { get; set; } = 1000;

    /// <summary>
    /// 内存队列最大长度，默认为 100000。 / Maximum in-memory queue length, default is 100000.
    /// </summary>
    /// <remarks>
    /// 队列写满后新日志将被丢弃，作为背压保护防止内存无限增长。 / When the queue is full, new log entries are dropped as back-pressure to prevent unbounded memory growth.
    /// </remarks>
    public int QueueLimit { get; set; } = 100000;

    /// <summary>
    /// 批量推送周期（秒），默认为 2。 / Batch push period in seconds, default is 2.
    /// </summary>
    /// <remarks>
    /// <para>传入 0 或负值视为无效，强制回退为默认 2 秒（校验逻辑自原 LogOptions.GrafanaLokiPeriod 迁移）。</para>
    /// <para>A value of 0 or negative is invalid and forced back to the default 2 seconds (validation migrated from the original LogOptions.GrafanaLokiPeriod).</para>
    /// </remarks>
    public int Period
    {
        get { return _period; }
        set { _period = value > 0 ? value : 2; }
    }

    /// <summary>
    /// 输出到该 sink 的最低日志级别；为 null 时跟随宿主 LogOptions.LogEventLevel。 / Minimum log level for this sink; falls back to the host LogOptions.LogEventLevel when null.
    /// </summary>
    public LogEventLevel? RestrictedToMinimumLevel { get; set; }

    /// <summary>
    /// 返回选项的脱敏 JSON 字符串表示形式。 / Returns the redacted JSON string representation of the options.
    /// </summary>
    /// <returns>脱敏后的 JSON 字符串 / The redacted JSON string</returns>
    /// <remarks>
    /// 密码非空时固定输出 "***"，避免凭证进入控制台或日志文件（lesson P0-1 同源约束）。 / A non-empty password is always rendered as "***" so credentials never reach the console or log files.
    /// </remarks>
    public override string ToString()
    {
        // 安全投影：密码脱敏，其余为非敏感装配参数。
        var safe = new
        {
            Url,
            Labels,
            Properties,
            UserName,
            Password = string.IsNullOrEmpty(Password) ? null : "***",
            CompressionEnabled,
            BatchSizeLimit,
            QueueLimit,
            Period,
            RestrictedToMinimumLevel,
        };

        return JsonHelper.SerializeFormat(safe);
    }
}
