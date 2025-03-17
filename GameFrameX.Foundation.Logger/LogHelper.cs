// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Diagnostics;
using System.Text;
using Serilog;

namespace GameFrameX.Foundation.Logger;

/// <summary>
/// 日志帮助类
/// </summary>
/// <remarks>
/// 提供了一系列静态方法用于记录不同级别的日志信息，包括调试信息、普通信息、警告和错误等。
/// 支持将日志输出到文件系统和控制台。
/// </remarks>
public static partial class LogHelper
{
    /// <summary>
    /// 将日志持久化。
    /// </summary>
    /// <remarks>
    /// 关闭日志记录器并将所有待处理的日志条目刷新到持久化存储中。
    /// </remarks>
    public static void FlushAndSave()
    {
        Serilog.Log.CloseAndFlush();
    }

    /// <summary>
    /// 异步将日志持久化。
    /// </summary>
    /// <remarks>
    /// 关闭日志记录器并将所有待处理的日志条目刷新到持久化存储中。
    /// </remarks>
    public static async void CloseAndFlushAsync()
    {
        await Serilog.Log.CloseAndFlushAsync();
    }

    /// <summary>
    /// 内部日志记录器实例
    /// </summary>
    private static ILogger _logger;

    /// <summary>
    /// 设置日志记录器
    /// </summary>
    /// <param name="logger">要设置的日志记录器实例</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void SetLogger(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <summary>
    /// 获取当前使用的日志记录器
    /// </summary>
    /// <returns>返回当前设置的日志记录器，如果未设置则返回Serilog的默认Logger</returns>
    private static ILogger GetLogger()
    {
        return _logger ?? Log.Logger;
    }

    /// <summary>
    /// 记录带有格式参数的信息消息。,只打印到控制台
    /// </summary>
    /// <param name="message">要记录的信息消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 仅将信息输出到控制台，不写入日志文件。
    /// 输出的消息会包含时间戳。
    /// </remarks>
    public static void Console(string message, params object[] args)
    {
        var time = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]";
        if (args is { Length: > 0, })
        {
            System.Console.WriteLine(time + message, args);
        }
        else
        {
            System.Console.WriteLine(time + message);
        }
    }
}