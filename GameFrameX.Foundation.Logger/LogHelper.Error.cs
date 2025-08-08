// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Diagnostics;
using System.Text;
using Serilog;

namespace GameFrameX.Foundation.Logger;

public static partial class LogHelper
{
    /// <summary>
    /// 记录严重的异常错误
    /// </summary>
    /// <param name="exception">异常对象，包含错误的详细信息</param>
    /// <remarks>
    /// 记录异常作为错误级别的日志，包含异常信息和消息。
    /// 此方法会自动获取默认的日志记录器进行记录。
    /// </remarks>
    public static void Error(Exception exception)
    {
        GetLogger().Error(exception, exception.Message);
    }

    /// <summary>
    /// 使用指定的日志记录器记录异常错误
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">要记录的异常对象</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出</exception>
    public static void Error(ILogger logger, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, string.Empty);
    }

    /// <summary>
    /// 记录带有格式参数的错误消息
    /// </summary>
    /// <param name="message">要记录的错误消息</param>
    /// <param name="args">用于格式化消息的参数数组</param>
    /// <remarks>
    /// 记录错误级别的日志信息，并包含堆栈跟踪信息。
    /// 使用默认的日志记录器进行记录。
    /// </remarks>
    public static void Error(string message, params object[] args)
    {
        var st = new StackTrace(1, true);
        var newMessage = new StringBuilder().Append(string.Format(message, args)).Append('\n').Append(st).ToString();

        GetLogger().Error(newMessage);
    }

    /// <summary>
    /// 记录错误消息模板。
    /// </summary>
    /// <param name="messageTemplate">消息模板。</param>
    /// <remarks>
    /// 使用结构化日志记录错误级别的消息模板。
    /// </remarks>
    public static void Error(string messageTemplate)
    {
        GetLogger().Error(messageTemplate);
    }

    /// <summary>
    /// 记录带有单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的错误消息。
    /// </remarks>
    public static void Error<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Error(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的错误消息。
    /// </remarks>
    public static void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Error(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有三个属性的错误消息。
    /// </remarks>
    public static void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常的错误消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <remarks>
    /// 记录错误级别的异常信息和消息模板。
    /// </remarks>
    public static void Error(Exception exception, string messageTemplate)
    {
        GetLogger().Error(exception, messageTemplate);
    }

    /// <summary>
    /// 记录带有异常和单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 记录错误级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Error<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Error(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 记录错误级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Error(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的错误消息模板。
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
    /// 记录错误级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的错误消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <remarks>
    /// 记录错误级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Error(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有格式参数的错误消息
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="message">要记录的错误消息</param>
    /// <param name="args">用于格式化消息的参数数组</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出</exception>
    public static void Error(ILogger logger, string message, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var st = new StackTrace(1, true);
        var newMessage = new StringBuilder().Append(string.Format(message, args)).Append('\n').Append(st).ToString();
        logger.Error(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录错误级别的消息模板。
    /// </remarks>
    public static void Error(ILogger logger, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的错误消息。
    /// </remarks>
    public static void Error<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的错误消息。
    /// </remarks>
    public static void Error<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的错误消息模板。
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
    /// 使用结构化日志记录带有三个属性的错误消息。
    /// </remarks>
    public static void Error<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录错误级别的异常信息和消息模板。
    /// </remarks>
    public static void Error(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录错误级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Error<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的错误消息模板。
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
    /// 记录错误级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Error<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的错误消息模板。
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
    /// 记录错误级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Error<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录错误级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Error(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 记录带有格式参数的错误消息并同时输出到控制台
    /// </summary>
    /// <param name="message">要记录的错误消息</param>
    /// <param name="args">用于格式化消息的参数数组</param>
    /// <remarks>
    /// 同时将错误信息输出到日志文件和控制台。
    /// 控制台输出使用红色字体以突出显示错误信息。
    /// </remarks>
    public static void ErrorConsole(string message, params object[] args)
    {
        GetLogger().Error(message, args);
        System.Console.ForegroundColor = ConsoleColor.Red;
        Console(message, args);
        System.Console.ResetColor();
    }

    /// <summary>
    /// 记录带有标签的异常错误
    /// </summary>
    /// <param name="tag">用于标识日志来源或分类的标签</param>
    /// <param name="exception">要记录的异常对象</param>
    /// <remarks>
    /// 使用默认的日志记录器记录带有标签的异常信息。
    /// 标签会被添加在日志消息的开头，格式为 [标签]。
    /// </remarks>
    public static void Error(string tag, Exception exception)
    {
        GetLogger().Error(exception, $"[{tag}] {exception}");
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的异常错误
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="tag">用于标识日志来源或分类的标签</param>
    /// <param name="exception">要记录的异常对象</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出</exception>
    public static void Error(ILogger logger, string tag, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, $"[{tag}] {exception}");
    }

    /// <summary>
    /// 记录带有标签的错误消息
    /// </summary>
    /// <param name="tag">用于标识日志来源或分类的标签</param>
    /// <param name="message">要记录的错误消息</param>
    /// <param name="args">用于格式化消息的参数数组</param>
    /// <remarks>
    /// 使用默认的日志记录器记录带有标签的错误消息。
    /// 包含完整的堆栈跟踪信息。
    /// </remarks>
    public static void Error(string tag, string message, params object[] args)
    {
        var st = new StackTrace(1, true);
        var newMessage = ($"[{tag}] {string.Format(message, args)}\n{st}");

        GetLogger().Error(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的错误消息
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="tag">用于标识日志来源或分类的标签</param>
    /// <param name="message">要记录的错误消息</param>
    /// <param name="args">用于格式化消息的参数数组</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出</exception>
    public static void Error(ILogger logger, string tag, string message, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var st = new StackTrace(1, true);
        var newMessage = ($"[{tag}] {string.Format(message, args)}\n{st}");
        logger.Error(newMessage);
    }

    /// <summary>
    /// 记录带有标签的错误消息并输出到控制台
    /// </summary>
    /// <param name="tag">用于标识日志来源或分类的标签</param>
    /// <param name="message">要记录的错误消息</param>
    /// <param name="args">用于格式化消息的参数数组</param>
    /// <remarks>
    /// 同时将错误信息记录到日志文件并以红色字体显示在控制台上。
    /// 消息前会添加标签标识，格式为 [标签]。
    /// </remarks>
    public static void ErrorConsole(string tag, string message, params object[] args)
    {
        Error(tag, message, args);
        System.Console.ForegroundColor = ConsoleColor.Red;
        Console($"[{tag}] {message}", args);
        System.Console.ResetColor();
    }
}