// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using Serilog;

namespace GameFrameX.Foundation.Logger;

public static partial class LogHelper
{
    /// <summary>
    /// 记录警告消息。
    /// </summary>
    /// <param name="message">要记录的警告消息。</param>
    /// <remarks>
    /// 使用默认日志记录器记录警告级别的日志信息。
    /// </remarks>
    public static void Warning(string message)
    {
        GetLogger().Warning(message);
    }

    /// <summary>
    /// 使用指定的日志记录器记录警告消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="message">要记录的警告消息。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Warning(ILogger logger, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(message);
    }

    /// <summary>
    /// 记录带有格式参数的警告消息。
    /// </summary>
    /// <param name="message">要记录的警告消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 用于记录警告级别的日志信息。
    /// </remarks>
    public static void Warning(string message, params object[] args)
    {
        GetLogger().Warning(message, args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有格式参数的警告消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="message">要记录的警告消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Warning(ILogger logger, string message, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(message, args);
    }

    /// <summary>
    /// 记录带有格式参数的警告消息。
    /// </summary>
    /// <param name="message">要记录的警告消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 同时将警告信息输出到日志文件和控制台。
    /// 控制台输出使用黄色字体以突出显示警告信息。
    /// </remarks>
    public static void WarningConsole(string message, params object[] args)
    {
        Warn(message, args);

        System.Console.ForegroundColor = ConsoleColor.Yellow;
        Console(message, args);
        System.Console.ResetColor();
    }


    /// <summary>
    /// 记录带有标签的警告消息。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的警告消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void Warning(string tag, string message, params object[] args)
    {
        GetLogger().Warning($"[{tag}] {message}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的警告消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="tag">日志标签。</param>
    /// <param name="message">要记录的警告消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Warning(ILogger logger, string tag, string message, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning($"[{tag}] {message}", args);
    }

    /// <summary>
    /// 记录带有标签的警告消息并输出到控制台。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的警告消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void WarningConsole(string tag, string message, params object[] args)
    {
        Warn(tag, message, args);
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        Console($"[{tag}] {message}", args);
        System.Console.ResetColor();
    }
}