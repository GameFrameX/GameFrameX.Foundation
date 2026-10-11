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

using GameFrameX.Foundation.Logger;
using MongoDB.Driver;
using Serilog;

namespace GameFrameX.Foundation.Logger.MongoDB;

/// <summary>
/// LogOptions 的 MongoDB 组件接入扩展：把 Mongo sink 以外部组件方式插入日志系统。
/// </summary>
/// <remarks>
/// Extensions on <see cref="LogOptions"/> that plug the MongoDB sink into the logging system as an external component.
/// 用法：<code>LogHandler.Create(new LogOptions().UseMongoDb("mongodb://127.0.0.1:27017/logs"))</code>。
/// </remarks>
public static class LogOptionsMongoDbExtensions
{
    /// <summary>
    /// 注册 MongoDB sink（连接串便捷重载）。
    /// </summary>
    /// <param name="logOptions">宿主日志配置选项 / The host log options</param>
    /// <param name="databaseUrl">MongoDB 连接串 / The MongoDB connection string</param>
    /// <param name="collectionName">日志集合名，默认 "log" / The log collection name, default "log"</param>
    /// <returns>传入的 <paramref name="logOptions"/>，支持链式配置 / The same <paramref name="logOptions"/> for fluent chaining</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="logOptions"/>、<paramref name="databaseUrl"/> 或 <paramref name="collectionName"/> 为 null 时抛出 / Thrown when any argument is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="databaseUrl"/> 或 <paramref name="collectionName"/> 为空白字符时抛出 / Thrown when the URL or collection name is whitespace</exception>
    public static LogOptions UseMongoDb(this LogOptions logOptions, string databaseUrl, string collectionName = "log")
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);

        return logOptions.UseMongoDb(sinkOptions =>
        {
            sinkOptions.DatabaseUrl = databaseUrl;
            sinkOptions.CollectionName = collectionName;
        });
    }

    /// <summary>
    /// 注册 MongoDB sink（数据库实例注入重载，优先于连接串，复用宿主连接池）。
    /// </summary>
    /// <param name="logOptions">宿主日志配置选项 / The host log options</param>
    /// <param name="database">外部注入的 MongoDB 数据库实例 / The externally injected MongoDB database instance</param>
    /// <param name="collectionName">日志集合名，默认 "log" / The log collection name, default "log"</param>
    /// <returns>传入的 <paramref name="logOptions"/>，支持链式配置 / The same <paramref name="logOptions"/> for fluent chaining</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="logOptions"/>、<paramref name="database"/> 或 <paramref name="collectionName"/> 为 null 时抛出 / Thrown when any argument is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="collectionName"/> 为空白字符时抛出 / Thrown when the collection name is whitespace</exception>
    public static LogOptions UseMongoDb(this LogOptions logOptions, IMongoDatabase database, string collectionName = "log")
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);

        return logOptions.UseMongoDb(sinkOptions =>
        {
            sinkOptions.Database = database;
            sinkOptions.CollectionName = collectionName;
        });
    }

    /// <summary>
    /// 注册 MongoDB sink（完整配置重载：注入优先级、capped 参数、滚动间隔与底层适配钩子）。
    /// </summary>
    /// <param name="logOptions">宿主日志配置选项 / The host log options</param>
    /// <param name="configure">组件配置回调 / The component configuration callback</param>
    /// <returns>传入的 <paramref name="logOptions"/>，支持链式配置 / The same <paramref name="logOptions"/> for fluent chaining</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="logOptions"/> 或 <paramref name="configure"/> 为 null 时抛出 / Thrown when <paramref name="logOptions"/> or <paramref name="configure"/> is null</exception>
    /// <exception cref="ArgumentException">在 <see cref="LogHandler.Create"/> 装配阶段，当 <see cref="MongoDbSinkOptions.Database"/> 与 <see cref="MongoDbSinkOptions.DatabaseUrl"/> 均未配置时抛出 / Thrown during <see cref="LogHandler.Create"/> sink wiring when neither Database nor DatabaseUrl is configured</exception>
    public static LogOptions UseMongoDb(this LogOptions logOptions, Action<MongoDbSinkOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        ArgumentNullException.ThrowIfNull(configure);

        var sinkOptions = new MongoDbSinkOptions();
        configure(sinkOptions);

        // 向核心包的统一外部 sink 注入点注册装配委托（C77 组件化）。
        logOptions.SinkConfigurators.Add(loggerConfiguration => AddMongoDbSink(loggerConfiguration, logOptions, sinkOptions));

        return logOptions;
    }

    /// <summary>
    /// 装配 MongoDB sink：注入优先、capped 集合参数、滚动间隔与底层适配钩子透传。
    /// </summary>
    /// <param name="loggerConfiguration">Serilog 配置 / The Serilog configuration</param>
    /// <param name="hostOptions">宿主日志配置选项（级别与滚动间隔跟随） / The host log options (level and rolling interval follow it)</param>
    /// <param name="sinkOptions">组件配置选项 / The component options</param>
    private static void AddMongoDbSink(LoggerConfiguration loggerConfiguration, LogOptions hostOptions, MongoDbSinkOptions sinkOptions)
    {
        // 最低级别：组件显式配置优先，否则跟随宿主 LogOptions.LogEventLevel（保持拆分前行为）。
        var minimumLevel = sinkOptions.RestrictedToMinimumLevel ?? hostOptions.LogEventLevel;

        loggerConfiguration.WriteTo.MongoDBBson(
            mongoConfiguration =>
            {
                // 注入优先级（C77）：Database 实例 > 连接串；两者皆空时 fail-fast（lesson P1-7），不静默连本地实例。
                if (sinkOptions.Database != null)
                {
                    mongoConfiguration.SetMongoDatabase(sinkOptions.Database);
                }
                else if (!string.IsNullOrWhiteSpace(sinkOptions.DatabaseUrl))
                {
                    mongoConfiguration.SetConnectionString(sinkOptions.DatabaseUrl);
                }
                else
                {
                    throw new ArgumentException("MongoDB 日志组件需要配置 MongoDbSinkOptions.Database 或 MongoDbSinkOptions.DatabaseUrl 之一。 / Either MongoDbSinkOptions.Database or MongoDbSinkOptions.DatabaseUrl must be configured for the MongoDB log component.");
                }

                mongoConfiguration.SetCollectionName(sinkOptions.CollectionName);
                mongoConfiguration.SetCreateCappedCollection(sinkOptions.CappedMaxSizeMb, sinkOptions.CappedMaxDocuments);

                // 滚动间隔：组件显式配置优先，否则跟随宿主 LogOptions.RollingInterval（枚举值对齐，保持拆分前行为）。
                mongoConfiguration.SetRollingInterval(sinkOptions.RollingInterval ?? (Serilog.Sinks.MongoDB.RollingInterval)hostOptions.RollingInterval);

                // 完整适配面透传：ExpireTTL / BatchPeriod / BatchPostingLimit / InsertManyOptions 等。
                sinkOptions.ConfigureSink?.Invoke(mongoConfiguration);
            },
            restrictedToMinimumLevel: minimumLevel);
    }
}
