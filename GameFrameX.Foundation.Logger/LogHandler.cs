// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Serilog;
using Serilog.Events;

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
            var logFileName = $"{logOptions.LogType ?? AppDomain.CurrentDomain.FriendlyName}_.log";
            var logSavePath = logOptions.LogSavePath ?? "./logs/";
            if (!Directory.Exists(logSavePath))
            {
                Directory.CreateDirectory(logSavePath);
            }

            var logPath = Path.Combine(logSavePath, logFileName);

            Console.WriteLine("以下为日志配置信息");
            Console.WriteLine(logOptions);
            Console.WriteLine("日志配置信息结束");
            Console.WriteLine();
            var logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                         .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                         .WriteTo.File(logPath, rollingInterval: logOptions.RollingInterval, rollOnFileSizeLimit: logOptions.IsFileSizeLimit, fileSizeLimitBytes: logOptions.FileSizeLimitBytes, retainedFileCountLimit: logOptions.RetainedFileCountLimit);
            configurationAction?.Invoke(logger);
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
                logger.WriteTo.Console();
            }

            var serilog = logger.CreateLogger();
            if (isDefault)
            {
                Serilog.Log.Logger = serilog;
                LogHelper.SetLogger(serilog);
            }

            Console.WriteLine("日志系统配置 结束");
            return serilog;
        }
        catch (Exception e)
        {
            Serilog.Log.Error($"配置日志系统过程中发生错误,异常:{e}");
            throw;
        }
    }
}