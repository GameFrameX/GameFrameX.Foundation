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
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
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

using System.IO.Compression;
using System.Net.Http.Headers;
using GameFrameX.Foundation.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace GameFrameX.Foundation.Logger;

/// <summary>
/// 日志处理器类，提供日志系统的初始化和配置功能
/// </summary>
/// <remarks>
/// Provides initialization and configuration functionality for the logging system.
/// </remarks>
public static class LogHandler
{
    private static bool _isInitSerilogDiagnosis;

    private sealed class LokiGzipHandler : DelegatingHandler
    {
        private readonly CompressionLevel _compressionLevel;

        public LokiGzipHandler(CompressionLevel compressionLevel) : base(new HttpClientHandler())
        {
            _compressionLevel = compressionLevel;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                var bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                using var stream = new MemoryStream();
                await using (var gzipStream = new GZipStream(stream, _compressionLevel, leaveOpen: true))
                {
                    await gzipStream.WriteAsync(bytes, cancellationToken);
                }

                request.Content = new ByteArrayContent(stream.ToArray());
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
                {
                    CharSet = "utf-8",
                };
                request.Content.Headers.ContentEncoding.Add("gzip");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }

    /// <summary>
    /// 启用 Serilog 的自动诊断
    /// </summary>
    /// <remarks>
    /// Enables Serilog's automatic diagnosis for debugging purposes.
    /// </remarks>
    private static void SerilogDiagnosis()
    {
        if (_isInitSerilogDiagnosis)
        {
            return;
        }

        Serilog.Debugging.SelfLog.Enable(message => System.Diagnostics.Debug.WriteLine($"Serilog:SelfLog:{message}"));
        _isInitSerilogDiagnosis = true;
    }

    /// <summary>
    /// 创建并返回一个基础的 Serilog 日志配置。
    /// </summary>
    /// <returns>用于后续扩展的基础 LoggerConfiguration / The base LoggerConfiguration for subsequent extension</returns>
    /// <remarks>
    /// <para>- Enables context enrichment (Enrich.FromLogContext) to easily attach request or business context information to logs.</para>
    /// <para>- Reduces framework log noise: sets Microsoft component log level to Information, ASP.NET Core to Warning, reducing unnecessary output.</para>
    /// <para>This base configuration can be extended with additional write targets, enrichment properties, minimum log levels, etc.</para>
    /// </remarks>
    public static LoggerConfiguration CreateLoggerConfiguration()
    {
        return new LoggerConfiguration()
               // 启用上下文丰富，自动附加 LogContext 中的属性到日志事件
               .Enrich.FromLogContext()
               // 在进入输出 sink 前统一处理敏感结构化属性
               .Enrich.With(new SensitiveLogEventEnricher())
               // 降低框架日志噪声：Microsoft 组件默认输出为 Information
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               // ASP.NET Core 默认仅记录 Warning 及以上级别
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
    }

    /// <summary>
    /// 控制台输出模板，用于格式化控制台日志输出。 / Console output template for formatting console log output.
    /// </summary>
    private const string ConsoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}][{LogType}]{Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 控制台输出模板，用于格式化控制台日志输出，包含标签名称。 / Console output template for formatting console log output with tag name.
    /// </summary>
    private const string ConsoleOutputTagNameTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}][{LogType}-{TagName}]{Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 文件输出模板，用于格式化文件日志输出。 / File output template for formatting file log output.
    /// </summary>
    private const string FileOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{LogType}]{Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 文件输出模板，用于格式化文件日志输出，包含标签名称。 / File output template for formatting file log output with tag name.
    /// </summary>
    private const string FileOutputTagNameTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{LogType}-{TagName}]{Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 启动并配置日志系统。
    /// </summary>
    /// <param name="logOptions">日志配置选项，包含日志级别、存储路径等配置信息 / Log configuration options containing log level, storage path, and other configuration information</param>
    /// <param name="isDefault">是否设置为默认配置 / Whether to set as default configuration</param>
    /// <param name="configurationAction">自定义日志配置回调 / Custom log configuration callback</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logOptions"/> 参数为 null 时抛出 / Thrown when <paramref name="logOptions"/> parameter is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="logOptions.LogTagName"/> 为空或仅包含空白字符时抛出 / Thrown when <paramref name="logOptions.LogTagName"/> is empty or contains only whitespace</exception>
    /// <exception cref="DirectoryNotFoundException">日志文件目录不存在且无法创建时抛出 / Thrown when log file directory does not exist and cannot be created</exception>
    /// <exception cref="UnauthorizedAccessException">没有权限创建日志目录或写入日志文件时抛出 / Thrown when there is no permission to create log directory or write log files</exception>
    /// <exception cref="Exception">初始化日志系统过程中发生的其他异常 / Other exceptions that occur during log system initialization</exception>
    /// <returns>配置好的 ILogger 实例 / The configured ILogger instance</returns>
    /// <remarks>
    /// <para>This method initializes the logging system based on the provided <paramref name="logOptions"/> configuration, supporting multiple output methods such as file, console, MongoDB, and Grafana Loki.</para>
    /// <para>The exception types cover parameter validation, directory creation, insufficient permissions, and all possible exceptions during log system initialization.</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// var options = new LogOptions { LogTagName = "App", LogSavePath = "./logs/", IsConsole = true };
    /// ILogger logger = LogHandler.Create(options);
    /// </code>
    /// </example>
    /// <seealso cref="LogOptions"/>
    /// <seealso cref="ILogger"/>
    public static ILogger Create(LogOptions logOptions, bool isDefault = true, Action<LoggerConfiguration> configurationAction = null)
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(logOptions.LogType, nameof(logOptions.LogType));
        SerilogDiagnosis();
        try
        {
            var logPath = ResolveLogPath(logOptions, isDefault);

            var logger = CreateLoggerConfiguration();
            logger.Enrich.WithProperty("TagName", logOptions.LogTagName);
            logger.Enrich.WithProperty("LogType", logOptions.LogType);

            ApplyLoki(logger, logOptions);
            configurationAction?.Invoke(logger);

            var templates = SelectOutputTemplates(logOptions);
            ApplyMongoDb(logger, logOptions);
            ApplyFile(logger, logOptions, logPath, templates.File);
            ApplyMinimumLevel(logger, logOptions);
            ApplyConsole(logger, logOptions, templates.Console);

            var serilog = logger.CreateLogger();
            if (isDefault)
            {
                Log.Logger = serilog;
                LogHelper.SetLogger(serilog);
            }

            return serilog;
        }
        catch (Exception e)
        {
            Log.Error(e, "配置日志系统过程中发生错误");
            throw;
        }
    }

    /// <summary>
    /// 计算最终日志文件路径，必要时创建目录。
    /// </summary>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <param name="isDefault">是否为默认配置（用于触发展示调用） / Whether this is the default configuration</param>
    /// <returns>最终日志文件路径 / Final log file path</returns>
    private static string ResolveLogPath(LogOptions logOptions, bool isDefault)
    {
        // 文件名
        var logFileName = logOptions.LogFileName.IsNotNullOrEmptyOrWhiteSpace() ? logOptions.LogFileName : $"{logOptions.LogTagName ?? logOptions.LogType}_.log";

        // 日志文件存储的路径，默认在应用程序运行目录下的子目录/logs
        var logSavePath = logOptions.LogSavePath ?? "./logs/";
        if (!logSavePath.EndsWith(Path.DirectorySeparatorChar))
        {
            logSavePath += Path.DirectorySeparatorChar;
        }

        logSavePath += logOptions.LogType + Path.DirectorySeparatorChar;

        // 计算最终日志文件路径
        var logPath = Path.Combine(logSavePath, logFileName);
        // 兼容可能的层级目录：始终创建文件所在的目录
        var logFolderPath = Path.GetDirectoryName(logPath) ?? logSavePath;
        if (!Directory.Exists(logFolderPath))
        {
            Directory.CreateDirectory(logFolderPath);
        }

        if (isDefault)
        {
            LogHelper.ShowOption("log configuration information", logOptions);
        }

        return logPath;
    }

    /// <summary>
    /// 选择控制台与文件的输出模板（根据 LogTagName 是否非空切换）。
    /// </summary>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <returns>控制台 / 文件输出模板二元组 / Console and file output template tuple</returns>
    private static (string Console, string File) SelectOutputTemplates(LogOptions logOptions)
    {
        if (logOptions.LogTagName.IsNotNullOrEmptyOrWhiteSpace())
        {
            return (ConsoleOutputTagNameTemplate, FileOutputTagNameTemplate);
        }

        return (ConsoleOutputTemplate, FileOutputTemplate);
    }

    /// <summary>
    /// 装配 Grafana Loki sink（按需启用，含 label / property / 凭证 / 压缩）。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    private static void ApplyLoki(LoggerConfiguration logger, LogOptions logOptions)
    {
        if (!logOptions.IsGrafanaLoki || logOptions.GrafanaLokiLabels == null)
        {
            return;
        }

        ApplyLokiProperties(logger, logOptions);
        var labels = BuildLokiLabels(logOptions);
        WriteLokiSink(logger, logOptions, labels);
    }

    /// <summary>
    /// 将 LogOptions.GrafanaLokiLabels 转换为 Serilog Loki 所需的标签数组。
    /// </summary>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <returns>Loki 标签数组 / Loki label array</returns>
    private static LokiLabel[] BuildLokiLabels(LogOptions logOptions)
    {
        var grafanaLokiLabels = new List<LokiLabel>(logOptions.GrafanaLokiLabels.Count);
        foreach (var kv in logOptions.GrafanaLokiLabels)
        {
            grafanaLokiLabels.Add(new LokiLabel
            {
                Key = kv.Key,
                Value = kv.Value,
            });
        }

        return grafanaLokiLabels.ToArray();
    }

    /// <summary>
    /// 将 LogOptions.GrafanaLokiProperty 中的非空属性 Enrich 到日志事件。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    private static void ApplyLokiProperties(LoggerConfiguration logger, LogOptions logOptions)
    {
        if (logOptions.GrafanaLokiProperty == null)
        {
            return;
        }

        foreach (var property in logOptions.GrafanaLokiProperty)
        {
            if (string.IsNullOrWhiteSpace(property.Key))
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(property.Value))
            {
                continue;
            }

            logger.Enrich.WithProperty(property.Key, property.Value);
        }
    }

    /// <summary>
    /// 写入 Grafana Loki sink；按压缩开关选择是否启用 Gzip 处理器。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <param name="labels">Loki 标签数组 / Loki label array</param>
    private static void WriteLokiSink(LoggerConfiguration logger, LogOptions logOptions, LokiLabel[] labels)
    {
        var credentials = BuildLokiCredentials(logOptions);
        if (logOptions.GrafanaLokiCompressionEnabled)
        {
            logger.WriteTo.GrafanaLoki(
                logOptions.GrafanaLokiUrl,
                labels: labels,
                credentials: credentials,
                batchSizeLimit: 1000,
                period: TimeSpan.FromSeconds(2),
                httpMessageHandler: new LokiGzipHandler(CompressionLevel.Optimal),
                restrictedToMinimumLevel: LogEventLevel.Verbose);
        }
        else
        {
            logger.WriteTo.GrafanaLoki(
                logOptions.GrafanaLokiUrl,
                labels: labels,
                credentials: credentials);
        }
    }

    /// <summary>
    /// 根据用户名/密码构造 LokiCredentials；任一为空返回 null。
    /// </summary>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <returns>Loki 凭证或 null / Loki credentials or null</returns>
    private static LokiCredentials BuildLokiCredentials(LogOptions logOptions)
    {
        if (string.IsNullOrWhiteSpace(logOptions.GrafanaLokiUserName) || string.IsNullOrWhiteSpace(logOptions.GrafanaLokiPassword))
        {
            return null;
        }

        return new LokiCredentials
        {
            Login = logOptions.GrafanaLokiUserName,
            Password = logOptions.GrafanaLokiPassword,
        };
    }

    /// <summary>
    /// 装配 MongoDB sink（按 IsWriteToMongoDb 开关）。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    private static void ApplyMongoDb(LoggerConfiguration logger, LogOptions logOptions)
    {
        if (!logOptions.IsWriteToMongoDb)
        {
            return;
        }

        logger.WriteTo.MongoDBBson(
            logOptions.MongoDbDatabaseUrl,
            logOptions.LogSavePath,
            cappedMaxSizeMb: logOptions.MongoDbCappedMaxSizeMb,
            cappedMaxDocuments: logOptions.MongoDbCappedMaxDocuments,
            rollingInterval: (Serilog.Sinks.MongoDB.RollingInterval)logOptions.RollingInterval,
            restrictedToMinimumLevel: logOptions.LogEventLevel);
    }

    /// <summary>
    /// 装配文件 sink（按 IsWriteToFile 开关）。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <param name="logPath">日志文件路径 / Log file path</param>
    /// <param name="fileOutputTemplate">文件输出模板 / File output template</param>
    private static void ApplyFile(LoggerConfiguration logger, LogOptions logOptions, string logPath, string fileOutputTemplate)
    {
        if (!logOptions.IsWriteToFile)
        {
            return;
        }

        logger.WriteTo.File(logPath,
                            shared: true,
                            restrictedToMinimumLevel: logOptions.LogEventLevel,
                            outputTemplate: fileOutputTemplate,
                            rollingInterval: logOptions.RollingInterval,
                            rollOnFileSizeLimit: logOptions.FileSizeLimitBytes > 0,
                            fileSizeLimitBytes: logOptions.FileSizeLimitBytes);
    }

    /// <summary>
    /// 装配控制台 sink（按 IsConsole 开关）。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    /// <param name="consoleOutputTemplate">控制台输出模板 / Console output template</param>
    private static void ApplyConsole(LoggerConfiguration logger, LogOptions logOptions, string consoleOutputTemplate)
    {
        if (!logOptions.IsConsole)
        {
            return;
        }

        logger.WriteTo.Console(outputTemplate: consoleOutputTemplate,
                               restrictedToMinimumLevel: logOptions.LogEventLevel,
                               theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Literate);
    }

    /// <summary>
    /// 根据 LogOptions.LogEventLevel 设置最低日志级别。
    /// </summary>
    /// <param name="logger">Logger 配置 / Logger configuration</param>
    /// <param name="logOptions">日志配置选项 / Log configuration options</param>
    private static void ApplyMinimumLevel(LoggerConfiguration logger, LogOptions logOptions)
    {
        switch (logOptions.LogEventLevel)
        {
            case LogEventLevel.Verbose:
            {
                logger.MinimumLevel.Verbose();
            }
                break;
            case LogEventLevel.Debug:
            {
                logger.MinimumLevel.Debug();
            }
                break;
            case LogEventLevel.Information:
            {
                logger.MinimumLevel.Information();
            }
                break;
            case LogEventLevel.Warning:
            {
                logger.MinimumLevel.Warning();
            }
                break;
            case LogEventLevel.Error:
            {
                logger.MinimumLevel.Error();
            }
                break;
            case LogEventLevel.Fatal:
            {
                logger.MinimumLevel.Fatal();
            }
                break;
        }
    }
}
