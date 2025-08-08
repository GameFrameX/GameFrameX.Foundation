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
    /// 使用指定的日志记录器记录详细级别的日志消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <remarks>
    /// 在记录日志之前会检查logger参数是否为null。
    /// </remarks>
    public static void Verbose(ILogger logger, string msg)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(msg);
    }

    /// <summary>
    /// 记录详细级别的日志消息。
    /// </summary>
    /// <param name="msg">要记录的详细消息。</param>
    /// <remarks>
    /// 用于记录最详细级别的日志信息，通常用于深入调试和跟踪。
    /// </remarks>
    public static void Verbose(string msg)
    {
        GetLogger().Verbose(msg);
    }

    /// <summary>
    /// 记录带有格式参数的详细级别日志消息。
    /// </summary>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 支持使用格式化字符串记录详细级别的日志信息。
    /// </remarks>
    public static void Verbose(string msg, params object[] args)
    {
        GetLogger().Verbose(msg, args);
    }

    /// <summary>
    /// 记录带有单个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的详细级别消息。
    /// </remarks>
    public static void Verbose<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Verbose(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的详细级别消息。
    /// </remarks>
    public static void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Verbose(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有三个属性的详细级别消息。
    /// </remarks>
    public static void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常的详细级别消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <remarks>
    /// 记录详细级别的异常信息和消息模板。
    /// </remarks>
    public static void Verbose(Exception exception, string messageTemplate)
    {
        GetLogger().Verbose(exception, messageTemplate);
    }

    /// <summary>
    /// 记录带有异常和单个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 记录详细级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Verbose(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 记录详细级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <remarks>
    /// 记录详细级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的详细级别消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <remarks>
    /// 记录详细级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Verbose(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有格式参数的详细级别日志消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 在记录日志之前会检查logger参数是否为null。
    /// </remarks>
    public static void Verbose(ILogger logger, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(msg, args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有单个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的详细级别消息。
    /// </remarks>
    public static void Verbose<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的详细级别消息。
    /// </remarks>
    public static void Verbose<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有三个属性的详细级别消息。
    /// </remarks>
    public static void Verbose<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的详细级别消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录详细级别的异常信息和消息模板。
    /// </remarks>
    public static void Verbose(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录详细级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Verbose<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录详细级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Verbose<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的详细级别消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录详细级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Verbose<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的详细级别消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录详细级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Verbose(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Verbose(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 记录带有格式参数的详细级别日志消息，并同时输出到控制台。
    /// </summary>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 同时将详细信息输出到日志文件和控制台，便于实时查看和调试。
    /// </remarks>
    public static void VerboseConsole(string msg, params object[] args)
    {
        Verbose(msg, args);
        Console(msg, args);
    }

    /// <summary>
    /// 记录带有格式参数的详细级别日志消息，并同时输出到控制台。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 同时将详细信息输出到日志文件和控制台，便于实时查看和调试。
    /// </remarks>
    public static void VerboseConsole(ILogger logger, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Verbose(logger, msg, args);
        Console(msg, args);
    }

    /// <summary>
    /// 记录带有标签的详细级别日志消息。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的详细消息。</param>
    public static void Verbose(string tag, string msg)
    {
        Verbose($"[{tag}] {msg}");
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的详细级别日志消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="tag">日志标签。</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <remarks>
    /// 在记录日志之前会检查logger参数是否为null。
    /// </remarks>
    public static void Verbose(ILogger logger, string tag, string msg)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Verbose(logger, $"[{tag}] {msg}");
    }

    /// <summary>
    /// 记录带有标签和格式参数的详细级别日志消息。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void Verbose(string tag, string msg, params object[] args)
    {
        Verbose($"[{tag}] {msg}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签和格式参数的详细级别日志消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="tag">日志标签。</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 在记录日志之前会检查logger参数是否为null。
    /// </remarks>
    public static void Verbose(ILogger logger, string tag, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Verbose(logger, $"[{tag}] {msg}", args);
    }

    /// <summary>
    /// 记录带有标签和格式参数的详细级别日志消息，并同时输出到控制台。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void VerboseConsole(string tag, string msg, params object[] args)
    {
        Verbose(tag, msg, args);
        Console($"[{tag}] {msg}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签和格式参数的详细级别日志消息，并同时输出到控制台。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="tag">日志标签。</param>
    /// <param name="msg">要记录的详细消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 在记录日志之前会检查logger参数是否为null，并将消息同时输出到日志文件和控制台。
    /// </remarks>
    public static void VerboseConsole(ILogger logger, string tag, string msg, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        Verbose(logger, tag, msg, args);
        Console($"[{tag}] {msg}", args);
    }
}