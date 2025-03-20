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
    /// 记录带有可选格式参数的调试消息。
    /// </summary>
    /// <param name="msg">要记录的调试消息。</param>
    /// <param name="args">消息的可选格式参数。</param>
    /// <remarks>
    /// 用于记录调试级别的日志信息，通常在开发和测试阶段使用。
    /// </remarks>
    public static void Debug(string msg, params object[] args)
    {
        GetLogger().Debug(msg, args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录调试消息
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="msg">要记录的调试消息</param>
    /// <param name="args">消息的格式参数</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void Debug(ILogger logger, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Debug(msg, args);
    }

    /// <summary>
    /// 记录带有可选格式参数的调试消息。并控制台打印
    /// </summary>
    /// <param name="msg">要记录的调试消息。</param>
    /// <param name="args">消息的可选格式参数。</param>
    /// <remarks>
    /// 同时将调试信息输出到日志文件和控制台。
    /// </remarks>
    public static void DebugConsole(string msg, params object[] args)
    {
        Debug(msg, args);
        Console(msg, args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录调试消息并输出到控制台
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="msg">要记录的调试消息</param>
    /// <param name="args">消息的格式参数</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void DebugConsole(ILogger logger, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Debug(msg, args);
        Console(msg, args);
    }

    /// <summary>
    /// 记录带有标签的调试消息。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的调试消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void Debug(string tag, string msg, params object[] args)
    {
        Debug($"[{tag}] {msg}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带标签的调试消息
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的调试消息</param>
    /// <param name="args">消息的格式参数</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void Debug(ILogger logger, string tag, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Debug($"[{tag}] {msg}", args);
    }

    /// <summary>
    /// 记录带有标签的调试消息并输出到控制台。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的调试消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void DebugConsole(string tag, string msg, params object[] args)
    {
        Debug($"[{tag}] {msg}", args);
        Console($"[{tag}] {msg}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带标签的调试消息并输出到控制台
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的调试消息</param>
    /// <param name="args">消息的格式参数</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void DebugConsole(ILogger logger, string tag, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Debug($"[{tag}] {msg}", args);
        Console($"[{tag}] {msg}", args);
    }
}