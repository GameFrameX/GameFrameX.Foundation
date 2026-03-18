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
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Diagnostics;
using System.Text;
using Serilog;
using GameFrameX.Foundation.Logger.Internal;

namespace GameFrameX.Foundation.Logger;

/// <summary>
/// 日志帮助类。
/// </summary>
/// <remarks>
/// Provides a series of static methods for recording logs at different levels, including debug, information, warning, and error.
/// Supports outputting logs to the file system and console.
/// </remarks>
public static partial class LogHelper
{
    /// <summary>
    /// 将日志持久化。
    /// </summary>
    /// <remarks>
    /// Closes the logger and flushes all pending log entries to persistent storage.
    /// </remarks>
    public static void FlushAndSave()
    {
        Log.CloseAndFlush();
    }

    /// <summary>
    /// 异步将日志持久化。
    /// </summary>
    /// <remarks>
    /// Asynchronously closes the logger and flushes all pending log entries to persistent storage.
    /// </remarks>
    public static async void CloseAndFlushAsync()
    {
        await Log.CloseAndFlushAsync();
    }

    /// <summary>
    /// 内部日志记录器实例。
    /// </summary>
    /// <remarks>
    /// Internal logger instance.
    /// </remarks>
    private static ILogger _logger;

    /// <summary>
    /// 临时日志记录器实例，用于在正式日志系统初始化前提供日志输出。
    /// </summary>
    /// <remarks>
    /// Temporary logger instance used to provide log output before the formal logging system is initialized.
    /// </remarks>
    private static InternalTempLogger _tempLogger;

    /// <summary>
    /// 用于保护 _logger 字段的锁对象。
    /// </summary>
    /// <remarks>
    /// Lock object used to protect the _logger field.
    /// </remarks>
    private static readonly object _loggerLock = new();

    /// <summary>
    /// 设置日志记录器。
    /// </summary>
    /// <param name="logger">要设置的日志记录器实例 / The logger instance to set</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    public static void SetLogger(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        lock (_loggerLock)
        {
            _tempLogger?.FlushTo(logger);
            _tempLogger?.Dispose();
            _tempLogger = null;

            _logger = logger;
        }
    }

    /// <summary>
    /// 获取当前使用的日志记录器。
    /// </summary>
    /// <returns>返回当前设置的日志记录器，如果未设置则返回临时日志记录器 / Returns the current logger, or a temporary logger if not set</returns>
    private static ILogger GetLogger()
    {
        lock (_loggerLock)
        {
            if (_logger != null)
            {
                return _logger;
            }

            if (_tempLogger == null)
            {
                _tempLogger = new InternalTempLogger();
            }

            return _tempLogger.Logger;
        }
    }

    /// <summary>
    /// 记录带有格式参数的信息消息，只打印到控制台。
    /// </summary>
    /// <param name="message">要记录的信息消息 / The information message to record</param>
    /// <param name="args">消息的格式参数 / Format arguments for the message</param>
    /// <remarks>
    /// Only outputs information to the console, not to the log file.
    /// The output message includes a timestamp.
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