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
    /// 用于保护 _logger 字段的锁对象
    /// </summary>
    private static readonly object _loggerLock = new();

    /// <summary>
    /// 设置日志记录器
    /// </summary>
    /// <param name="logger">要设置的日志记录器实例</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void SetLogger(ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        lock (_loggerLock)
        {
            _logger = logger;
        }
    }

    /// <summary>
    /// 获取当前使用的日志记录器
    /// </summary>
    /// <returns>返回当前设置的日志记录器，如果未设置则返回Serilog的默认Logger</returns>
    private static ILogger GetLogger()
    {
        lock (_loggerLock)
        {
            return _logger ?? Log.Logger;
        }
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