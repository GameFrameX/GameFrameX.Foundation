// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.IO.Compression;
using GameFrameX.Foundation.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.Grafana.Loki.HttpClients;

namespace GameFrameX.Foundation.Logger;

/// <summary>
/// 日志处理器类，提供日志系统的初始化和配置功能
/// </summary>
public static class LogHandler
{
    private static bool _isInitSerilogDiagnosis = false;

    /// <summary>
    /// 启用 Serilog 的自动诊断
    /// </summary>
    private static void SerilogDiagnosis()
    {
        if (_isInitSerilogDiagnosis)
        {
            return;
        }

        Serilog.Debugging.SelfLog.Enable((message) => { Console.WriteLine($"Serilog:SelfLog:{message}"); });
        _isInitSerilogDiagnosis = true;
    }

    /// <summary>
    /// 创建并返回一个基础的 Serilog 日志配置。
    /// </summary>
    /// <remarks>
    /// - 启用上下文丰富（Enrich.FromLogContext），便于在日志中附带请求或业务上下文信息。
    /// - 下调框架日志噪声：将 Microsoft 组件日志级别设为 Information，将 ASP.NET Core 设为 Warning，减少不必要的输出。
    /// 该基础配置可在后续继续追加写入目标、丰富属性、最小日志级别等。
    /// </remarks>
    /// <returns>用于后续扩展的基础 LoggerConfiguration。</returns>
    public static LoggerConfiguration CreateLoggerConfiguration()
    {
        return new LoggerConfiguration()
               // 启用上下文丰富，自动附加 LogContext 中的属性到日志事件
               .Enrich.FromLogContext()
               // 降低框架日志噪声：Microsoft 组件默认输出为 Information
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               // ASP.NET Core 默认仅记录 Warning 及以上级别
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);
    }

    /// <summary>
    /// 启动并配置日志系统
    /// </summary>
    /// <param name="logOptions">日志配置选项，包含日志级别、存储路径等配置信息</param>
    /// <param name="isDefault">是否设置为默认配置</param>
    /// <param name="configurationAction">自定义日志配置回调</param>
    /// <exception cref="ArgumentNullException">当logOptions参数为null时抛出</exception>
    /// <exception cref="Exception">初始化日志系统过程中发生的其他异常</exception>
    public static ILogger Create(LogOptions logOptions, bool isDefault = true, Action<LoggerConfiguration> configurationAction = null)
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        SerilogDiagnosis();
        try
        {
            // 日志文件存储的路径
            var logFileName = $"{(logOptions.LogType ?? AppDomain.CurrentDomain.FriendlyName).ToLower()}_.log";
            var logSavePath = logOptions.LogSavePath ?? "./logs/";
            // 计算最终日志文件路径
            var logPath = Path.Combine(logSavePath, logFileName);
            // 兼容可能的层级目录：始终创建文件所在的目录
            var logFolderPath = Path.GetDirectoryName(logPath) ?? logSavePath;
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }


            // Console.WriteLine("the following is the log configuration information");
            if (isDefault)
            {
                LogHelper.ShowOption("log configuration information", logOptions);
            }

            // Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
            // Console.WriteLine(logOptions);
            // Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            var logger = CreateLoggerConfiguration().Enrich.WithProperty("AppType", logOptions.LogType ?? AppDomain.CurrentDomain.FriendlyName);
            if (!string.IsNullOrEmpty(logOptions.LogTagName))
            {
                logger.Enrich.WithProperty("TagName", logOptions.LogTagName ?? "");
            }


            if (logOptions.IsGrafanaLoki)
            {
                var grafanaLokiLabels = new List<LokiLabel>();
                foreach (var kv in logOptions.GrafanaLokiLabels)
                {
                    var lokiLabel = new LokiLabel
                    {
                        Key = kv.Key,
                        Value = kv.Value,
                    };
                    grafanaLokiLabels.Add(lokiLabel);
                }

                if (logOptions.GrafanaLokiProperty != null)
                {
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

                LokiCredentials lokiCredentials = null;
                if (!string.IsNullOrWhiteSpace(logOptions.GrafanaLokiUserName) && !string.IsNullOrWhiteSpace(logOptions.GrafanaLokiPassword))
                {
                    lokiCredentials = new LokiCredentials
                    {
                        Login = logOptions.GrafanaLokiUserName,
                        Password = logOptions.GrafanaLokiPassword,
                    };
                }

                // 判断是否启用压缩
                if (logOptions.GrafanaLokiCompressionEnabled)
                {
                    // 使用默认压缩数据客户端
                    var lokiGzipHttpClient = new LokiGzipHttpClient(null, CompressionLevel.Optimal);
                    lokiGzipHttpClient.SetCredentials(lokiCredentials);
                    lokiGzipHttpClient.SetTenant(null);
                    // 根据源码的实际参数配置 GrafanaLoki
                    logger.WriteTo.GrafanaLoki(uri: logOptions.GrafanaLokiUrl, grafanaLokiLabels, null, lokiCredentials, null, LogEventLevel.Verbose, 1000, null, TimeSpan.FromSeconds(2), null, lokiGzipHttpClient);
                }
                else
                {
                    logger.WriteTo.GrafanaLoki(logOptions.GrafanaLokiUrl, grafanaLokiLabels, null, lokiCredentials);
                }
            }

            configurationAction?.Invoke(logger);

            string consoleOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}][{TagName}]{Message:lj}{NewLine}{Exception}";
            string fileOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{FriendlyName}] {Message:lj}{NewLine}{Exception}";

            if (logOptions.ConsoleOutputTemplate.IsNotNullOrEmptyOrWhiteSpace())
            {
                consoleOutputTemplate = logOptions.ConsoleOutputTemplate;
            }

            if (logOptions.FileOutputTemplate.IsNotNullOrEmptyOrWhiteSpace())
            {
                fileOutputTemplate = logOptions.FileOutputTemplate;
            }
            if (logOptions.IsWriteToFile)
            {
                logger.WriteTo.File(logPath,
                                    shared: true,
                                    restrictedToMinimumLevel: logOptions.LogEventLevel,
                                    outputTemplate: fileOutputTemplate,
                                    rollingInterval: logOptions.RollingInterval,
                                    rollOnFileSizeLimit: logOptions.FileSizeLimitBytes > 0,
                                    fileSizeLimitBytes: logOptions.FileSizeLimitBytes
                );
            }

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

            if (logOptions.IsConsole)
            {
                logger.WriteTo.Console(outputTemplate: consoleOutputTemplate,
                                       restrictedToMinimumLevel: logOptions.LogEventLevel,
                                       theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Literate);
            }

            var serilog = logger.CreateLogger();
            if (isDefault)
            {
                Serilog.Log.Logger = serilog;
                LogHelper.SetLogger(serilog);
            }

            return serilog;
        }
        catch (Exception e)
        {
            Serilog.Log.Error($"配置日志系统过程中发生错误,异常:{e}");
            throw;
        }
    }
}