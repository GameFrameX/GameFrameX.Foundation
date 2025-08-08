// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Diagnostics;
using Serilog;

namespace GameFrameX.Foundation.Logger;

public static partial class LogHelper
{
    /// <summary>
    /// 记录严重错误消息。
    /// </summary>
    /// <param name="message">要记录的严重错误消息。</param>
    /// <remarks>
    /// 记录致命错误级别的日志信息，并包含堆栈跟踪信息。
    /// </remarks>
    public static void Fatal(string message)
    {
        var newMessage = ($"{message}\n{new StackTrace(1, true)}");

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 记录带有单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的致命错误消息。
    /// </remarks>
    public static void Fatal<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Fatal(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的致命错误消息。
    /// </remarks>
    public static void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Fatal(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有三个属性的致命错误消息。
    /// </remarks>
    public static void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }


    /// <summary>
    /// 记录带有异常和单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 记录致命错误级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 记录致命错误级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的致命错误消息模板。
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
    /// 记录致命错误级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的致命错误消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <remarks>
    /// 记录致命错误级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 使用指定的日志记录器记录严重错误消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="message">要记录的严重错误消息。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Fatal(ILogger logger, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = ($"{message}\n{new StackTrace(1, true)}");
        logger.Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的致命错误消息。
    /// </remarks>
    public static void Fatal<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的致命错误消息。
    /// </remarks>
    public static void Fatal<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的致命错误消息模板。
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
    /// 使用结构化日志记录带有三个属性的致命错误消息。
    /// </remarks>
    public static void Fatal<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的致命错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录致命错误级别的异常信息和消息模板。
    /// </remarks>
    public static void Fatal(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录致命错误级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Fatal<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的致命错误消息模板。
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
    /// 记录致命错误级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Fatal<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的致命错误消息模板。
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
    /// 记录致命错误级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Fatal<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的致命错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录致命错误级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Fatal(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 记录严重的异常错误。
    /// </summary>
    /// <param name="exception">要记录的异常对象。</param>
    /// <remarks>
    /// 记录异常作为致命错误级别的日志，并包含堆栈跟踪信息。
    /// </remarks>
    public static void Fatal(Exception exception)
    {
        var newMessage = ($"{exception}\n{new StackTrace(1, true)}");

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录严重的异常错误。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="exception">要记录的异常对象。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Fatal(ILogger logger, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = ($"{exception}\n{new StackTrace(1, true)}");
        logger.Fatal(newMessage);
    }

    /// <summary>
    /// 记录带有标签的严重错误消息。
    /// </summary>
    /// <param name="tag">日志标签，用于标识日志来源或分类。</param>
    /// <param name="message">要记录的严重错误消息。</param>
    /// <remarks>
    /// 记录的消息将包含标签前缀和堆栈跟踪信息。
    /// </remarks>
    public static void Fatal(string tag, string message)
    {
        var newMessage = $"[{tag}] {message}\n{new StackTrace(1, true)}";

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的严重错误消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="tag">日志标签，用于标识日志来源或分类。</param>
    /// <param name="message">要记录的严重错误消息。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Fatal(ILogger logger, string tag, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = $"[{tag}] {message}\n{new StackTrace(1, true)}";
        logger.Fatal(newMessage);
    }

    /// <summary>
    /// 记录带有标签的严重异常错误。
    /// </summary>
    /// <param name="tag">日志标签，用于标识日志来源或分类。</param>
    /// <param name="exception">要记录的异常对象。</param>
    /// <remarks>
    /// 记录的异常信息将包含标签前缀和堆栈跟踪信息。
    /// </remarks>
    public static void Fatal(string tag, Exception exception)
    {
        var newMessage = $"[{tag}] {exception}\n{new StackTrace(1, true)}";

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的严重异常错误。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例。</param>
    /// <param name="tag">日志标签，用于标识日志来源或分类。</param>
    /// <param name="exception">要记录的异常对象。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出。</exception>
    public static void Fatal(ILogger logger, string tag, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = $"[{tag}] {exception}\n{new StackTrace(1, true)}";
        logger.Fatal(newMessage);
    }
}