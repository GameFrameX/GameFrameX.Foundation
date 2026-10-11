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
using MongoDB.Driver;
using Serilog.Events;

namespace GameFrameX.Foundation.Logger.MongoDB;

/// <summary>
/// MongoDB 日志 sink 的组件配置选项，由 <see cref="LogOptionsMongoDbExtensions.UseMongoDb"/> 注册到日志系统。
/// </summary>
/// <remarks>
/// Component configuration for the MongoDB logging sink, registered into the logging system via <see cref="LogOptionsMongoDbExtensions.UseMongoDb"/>.
/// 自 C77 起 Mongo 接入参数从核心包 <c>LogOptions</c> 迁入本组件包，核心包不再依赖 MongoDB.Driver / Serilog.Sinks.MongoDB。
/// </remarks>
public sealed class MongoDbSinkOptions
{
    /// <summary>
    /// MongoDB 连接串；默认为空，需与 <see cref="Database"/> 至少配置其一，否则装配时抛出异常。
    /// </summary>
    /// <remarks>
    /// <para>Connection string for MongoDB; empty by default. At least one of this or <see cref="Database"/> must be configured, otherwise sink wiring throws.</para>
    /// <para>连接串格式：mongodb://[username:password@]host[:port]/database[?options]。</para>
    /// <para>默认值改空为 lesson（CODE_REVIEW-2026-05-18 P1-7）结论：避免静默连接本地实例。</para>
    /// </remarks>
    public string DatabaseUrl { get; set; } = string.Empty;

    /// <summary>
    /// 外部注入的 MongoDB 数据库实例；非空时优先于 <see cref="DatabaseUrl"/>，用于复用宿主已配置的连接池 / TLS / 凭证。
    /// </summary>
    /// <remarks>
    /// An externally injected MongoDB database instance; takes precedence over <see cref="DatabaseUrl"/> when set, reusing the host's connection pool / TLS / credentials.
    /// </remarks>
    public IMongoDatabase Database { get; set; }

    /// <summary>
    /// 日志集合名称，默认为 "log"。 / Log collection name, default is "log".
    /// </summary>
    /// <remarks>
    /// 修正项（C77）：原实现误将文件系统日志目录作为集合名传入 sink，此处固定为集合名语义。
    /// </remarks>
    public string CollectionName { get; set; } = "log";

    /// <summary>
    /// capped 集合的最大总大小（MB），默认为 50。 / Maximum total size (MB) of the capped collection, default is 50.
    /// </summary>
    public long CappedMaxSizeMb { get; set; } = 50;

    /// <summary>
    /// capped 集合的最大文档数，默认为 50000。 / Maximum document count of the capped collection, default is 50000.
    /// </summary>
    public long? CappedMaxDocuments { get; set; } = 50000;

    /// <summary>
    /// 日志滚动间隔；为 null 时跟随宿主 LogOptions.RollingInterval。 / Log rolling interval; falls back to the host LogOptions.RollingInterval when null.
    /// </summary>
    public Serilog.Sinks.MongoDB.RollingInterval? RollingInterval { get; set; }

    /// <summary>
    /// 输出到该 sink 的最低日志级别；为 null 时跟随宿主 LogOptions.LogEventLevel。 / Minimum log level for this sink; falls back to the host LogOptions.LogEventLevel when null.
    /// </summary>
    public LogEventLevel? RestrictedToMinimumLevel { get; set; }

    /// <summary>
    /// 底层 MongoDBSinkConfiguration 的完整适配钩子（TTL、批量周期、批量条数、InsertManyOptions 等）。
    /// </summary>
    /// <remarks>
    /// Full adaptation hook over the underlying MongoDBSinkConfiguration (TTL, batch period, batch limit, InsertManyOptions, etc.).
    /// 例：<code>sinkOptions.ConfigureSink = cfg => cfg.SetExpireTTL(TimeSpan.FromDays(7));</code>
    /// </remarks>
    public Action<Serilog.Sinks.MongoDB.MongoDBSinkConfiguration> ConfigureSink { get; set; }

    /// <summary>
    /// 返回选项的脱敏 JSON 字符串表示形式。 / Returns the redacted JSON string representation of the options.
    /// </summary>
    /// <returns>脱敏后的 JSON 字符串 / The redacted JSON string</returns>
    /// <remarks>
    /// 连接串整体脱敏（仅暴露"已配置"状态），避免凭证进入控制台或日志文件（lesson P0-1 同源约束）。
    /// </remarks>
    public override string ToString()
    {
        // 安全投影：连接串不出现在字符串表示中。
        var safe = new
        {
            CollectionName,
            CappedMaxSizeMb,
            CappedMaxDocuments,
            RollingInterval,
            RestrictedToMinimumLevel,
            HasInjectedDatabase = Database != null,
            HasDatabaseUrl = !string.IsNullOrWhiteSpace(DatabaseUrl),
        };

        return JsonHelper.SerializeFormat(safe);
    }
}
