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
    /// 记录信息消息
    /// </summary>
    /// <param name="message">要记录的信息对象</param>
    /// <remarks>
    /// 将对象转换为字符串后记录为信息级别的日志。
    /// 如果对象为null，将记录"null object"。
    /// </remarks>
    public static void Info(object message)
    {
        GetLogger().Information(message?.ToString() ?? "null object");
    }

    /// <summary>
    /// 记录带有格式参数的信息消息。
    /// </summary>
    /// <param name="message">要记录的信息消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 用于记录一般信息级别的日志。
    /// </remarks>
    public static void Info(string message, params object[] args)
    {
        GetLogger().Information(message, args);
    }

    /// <summary>
    /// 记录信息消息模板。
    /// </summary>
    /// <param name="messageTemplate">消息模板。</param>
    /// <remarks>
    /// 使用结构化日志记录信息级别的消息模板。
    /// </remarks>
    public static void Info(string messageTemplate)
    {
        GetLogger().Information(messageTemplate);
    }

    /// <summary>
    /// 记录带有单个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的信息消息。
    /// </remarks>
    public static void Info<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Information(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的信息消息。
    /// </remarks>
    public static void Info<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Information(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有三个属性的信息消息。
    /// </remarks>
    public static void Info<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常的信息消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <remarks>
    /// 记录信息级别的异常信息和消息模板。
    /// </remarks>
    public static void Info(Exception exception, string messageTemplate)
    {
        GetLogger().Information(exception, messageTemplate);
    }

    /// <summary>
    /// 记录带有异常和单个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 记录信息级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Info<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Information(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 记录信息级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Info<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Information(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的信息消息模板。
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
    /// 记录信息级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Info<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的信息消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <remarks>
    /// 记录信息级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Info(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Information(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有格式参数的信息消息
    /// </summary>
    /// <param name="logger">要使用的日志记录器实例</param>
    /// <param name="message">要记录的信息消息</param>
    /// <param name="args">消息的格式参数</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    public static void Info(ILogger logger, string message, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(message, args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录信息消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录信息级别的消息模板。
    /// </remarks>
    public static void Info(ILogger logger, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有单个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的信息消息。
    /// </remarks>
    public static void Info<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的信息消息。
    /// </remarks>
    public static void Info<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的信息消息模板。
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
    /// 使用结构化日志记录带有三个属性的信息消息。
    /// </remarks>
    public static void Info<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的信息消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录信息级别的异常信息和消息模板。
    /// </remarks>
    public static void Info(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的信息消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录信息级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Info<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的信息消息模板。
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
    /// 记录信息级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Info<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的信息消息模板。
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
    /// 记录信息级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Info<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的信息消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录信息级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Info(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Information(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 记录带有格式参数的信息消息。并控制台打印
    /// </summary>
    /// <param name="message">要记录的信息消息。</param>
    /// <param name="args">消息的格式参数。</param>
    /// <remarks>
    /// 同时将信息输出到日志文件和控制台。
    /// </remarks>
    public static void InfoConsole(string message, params object[] args)
    {
        Info(message, args);
        Console(message, args);
    }

    /// <summary>
    /// 记录信息消息。
    /// </summary>
    /// <param name="msg">要记录的异常对象。</param>
    /// <remarks>
    /// 将异常的消息内容记录为信息级别的日志。
    /// </remarks>
    public static void Info(Exception msg)
    {
        GetLogger().Information(msg.ToString());
    }

    /// <summary>
    /// 使用指定的日志记录器记录异常信息
    /// </summary>
    /// <param name="logger">要使用的日志记录器实例</param>
    /// <param name="exception">要记录的异常对象</param>
    public static void Info(ILogger logger, Exception exception)
    {
        Info(exception.ToString());
    }

    /// <summary>
    /// 使用指定的日志记录器记录异常信息并输出到控制台
    /// </summary>
    /// <param name="logger">要使用的日志记录器实例</param>
    /// <param name="exception">要记录的异常对象</param>
    public static void InfoConsole(ILogger logger, Exception exception)
    {
        Info(exception.ToString());
        Console(exception.ToString());
    }

    /// <summary>
    /// 记录带有标签的信息消息。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的信息消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void Info(string tag, string message, params object[] args)
    {
        Info($"[{tag}] {message}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的信息消息
    /// </summary>
    /// <param name="logger">要使用的日志记录器实例</param>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的信息消息</param>
    /// <param name="args">消息的格式参数</param>
    public static void Info(ILogger logger, string tag, string message, params object[] args)
    {
        Info(logger, $"[{tag}] {message}", args);
    }

    /// <summary>
    /// 记录带有标签的信息消息并输出到控制台。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的信息消息。</param>
    /// <param name="args">消息的格式参数。</param>
    public static void InfoConsole(string tag, string message, params object[] args)
    {
        Info(tag, message, args);
        Console($"[{tag}] {message}", args);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的信息消息并输出到控制台
    /// </summary>
    /// <param name="logger">要使用的日志记录器实例</param>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的信息消息</param>
    /// <param name="args">消息的格式参数</param>
    public static void InfoConsole(ILogger logger, string tag, string message, params object[] args)
    {
        Info(logger, tag, message, args);
        Console($"[{tag}] {message}", args);
    }

    /// <summary>
    /// 记录带有标签的对象信息。
    /// </summary>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的信息对象</param>
    public static void Info(string tag, object message)
    {
        GetLogger().Information($"[{tag}] {message?.ToString() ?? "null object"}");
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的对象信息
    /// </summary>
    /// <param name="logger">要使用的日志记录器实例</param>
    /// <param name="tag">日志标签</param>
    /// <param name="message">要记录的信息对象</param>
    public static void Info(ILogger logger, string tag, object message)
    {
        logger.Information($"[{tag}] {message?.ToString() ?? "null object"}");
    }
}