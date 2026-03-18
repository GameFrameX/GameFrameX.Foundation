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

public static partial class LogHelper
{
    /// <summary>
    /// 记录严重的异常错误
    /// </summary>
    /// <param name="exception">异常对象，包含错误的详细信息 / The exception object containing detailed error information</param>
    /// <remarks>
    /// Records the exception as an error level log, including exception information and message.
    /// This method automatically obtains the default logger for recording.
    /// </remarks>
    public static void Error(Exception exception)
    {
        GetLogger().Error(exception, exception.Message);
    }

    /// <summary>
    /// 使用指定的日志记录器记录异常错误
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    public static void Error(ILogger logger, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, string.Empty);
    }

    /// <summary>
    /// 记录带有格式参数的错误消息
    /// </summary>
    /// <param name="message">要记录的错误消息 / The error message to record</param>
    /// <param name="args">用于格式化消息的参数数组 / The parameter array for formatting the message</param>
    /// <remarks>
    /// Records error level log information and includes stack trace information.
    /// Uses the default logger for recording.
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
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <remarks>
    /// Uses structured logging to record error level message templates.
    /// </remarks>
    public static void Error(string messageTemplate)
    {
        GetLogger().Error(messageTemplate);
    }

    /// <summary>
    /// 记录带有单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <remarks>
    /// Uses structured logging to record error level messages with a single property.
    /// </remarks>
    public static void Error<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Error(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <remarks>
    /// Uses structured logging to record error level messages with two properties.
    /// </remarks>
    public static void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Error(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <typeparam name="T2">第三个属性值的类型 / The type of the third property value</typeparam>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <param name="propertyValue2">第三个属性值 / The third property value</param>
    /// <remarks>
    /// Uses structured logging to record error level messages with three properties.
    /// </remarks>
    public static void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常的错误消息模板。
    /// </summary>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <remarks>
    /// Records error level exception information and message template.
    /// </remarks>
    public static void Error(Exception exception, string messageTemplate)
    {
        GetLogger().Error(exception, messageTemplate);
    }

    /// <summary>
    /// 记录带有异常和单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <remarks>
    /// Records error level exception information and message template with a single property.
    /// </remarks>
    public static void Error<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Error(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <remarks>
    /// Records error level exception information and message template with two properties.
    /// </remarks>
    public static void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Error(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <typeparam name="T2">第三个属性值的类型 / The type of the third property value</typeparam>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <param name="propertyValue2">第三个属性值 / The third property value</param>
    /// <remarks>
    /// Records error level exception information and message template with three properties.
    /// </remarks>
    public static void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的错误消息模板。
    /// </summary>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValues">属性值数组 / The property value array</param>
    /// <remarks>
    /// Records error level exception information and message template with multiple properties.
    /// </remarks>
    public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Error(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有格式参数的错误消息
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="message">要记录的错误消息 / The error message to record</param>
    /// <param name="args">用于格式化消息的参数数组 / The parameter array for formatting the message</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
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
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Uses structured logging to record error level message templates.
    /// </remarks>
    public static void Error(ILogger logger, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Uses structured logging to record error level messages with a single property.
    /// </remarks>
    public static void Error<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Uses structured logging to record error level messages with two properties.
    /// </remarks>
    public static void Error<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <typeparam name="T2">第三个属性值的类型 / The type of the third property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <param name="propertyValue2">第三个属性值 / The third property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Uses structured logging to record error level messages with three properties.
    /// </remarks>
    public static void Error<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records error level exception information and message template.
    /// </remarks>
    public static void Error(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records error level exception information and message template with a single property.
    /// </remarks>
    public static void Error<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records error level exception information and message template with two properties.
    /// </remarks>
    public static void Error<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <typeparam name="T2">第三个属性值的类型 / The type of the third property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <param name="propertyValue2">第三个属性值 / The third property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records error level exception information and message template with three properties.
    /// </remarks>
    public static void Error<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValues">属性值数组 / The property value array</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records error level exception information and message template with multiple properties.
    /// </remarks>
    public static void Error(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 记录带有格式参数的错误消息并同时输出到控制台
    /// </summary>
    /// <param name="message">要记录的错误消息 / The error message to record</param>
    /// <param name="args">用于格式化消息的参数数组 / The parameter array for formatting the message</param>
    /// <remarks>
    /// Outputs error information to both log file and console.
    /// Console output uses red font to highlight error information.
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
    /// <param name="tag">用于标识日志来源或分类的标签 / The tag used to identify the log source or category</param>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <remarks>
    /// Uses the default logger to record exception information with a tag.
    /// The tag is added at the beginning of the log message in the format [tag].
    /// </remarks>
    public static void Error(string tag, Exception exception)
    {
        GetLogger().Error(exception, $"[{tag}] {exception}");
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的异常错误
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="tag">用于标识日志来源或分类的标签 / The tag used to identify the log source or category</param>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records exception information with a tag using the specified logger.
    /// </remarks>
    public static void Error(ILogger logger, string tag, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Error(exception, $"[{tag}] {exception}");
    }

    /// <summary>
    /// 记录带有标签的错误消息
    /// </summary>
    /// <param name="tag">用于标识日志来源或分类的标签 / The tag used to identify the log source or category</param>
    /// <param name="message">要记录的错误消息 / The error message to record</param>
    /// <param name="args">用于格式化消息的参数数组 / The parameter array for formatting the message</param>
    /// <remarks>
    /// Uses the default logger to record error messages with a tag.
    /// Includes complete stack trace information.
    /// </remarks>
    public static void Error(string tag, string message, params object[] args)
    {
        var st = new StackTrace(1, true);
        var newMessage = $"[{tag}] {string.Format(message, args)}\n{st}";

        GetLogger().Error(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的错误消息
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="tag">用于标识日志来源或分类的标签 / The tag used to identify the log source or category</param>
    /// <param name="message">要记录的错误消息 / The error message to record</param>
    /// <param name="args">用于格式化消息的参数数组 / The parameter array for formatting the message</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records error message with a tag using the specified logger. Includes complete stack trace information.
    /// </remarks>
    public static void Error(ILogger logger, string tag, string message, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var st = new StackTrace(1, true);
        var newMessage = $"[{tag}] {string.Format(message, args)}\n{st}";
        logger.Error(newMessage);
    }

    /// <summary>
    /// 记录带有标签的错误消息并输出到控制台
    /// </summary>
    /// <param name="tag">用于标识日志来源或分类的标签 / The tag used to identify the log source or category</param>
    /// <param name="message">要记录的错误消息 / The error message to record</param>
    /// <param name="args">用于格式化消息的参数数组 / The parameter array for formatting the message</param>
    /// <remarks>
    /// Records error information to log file and displays it on console with red font.
    /// The message is prefixed with a tag identifier in the format [tag].
    /// </remarks>
    public static void ErrorConsole(string tag, string message, params object[] args)
    {
        Error(tag, message, args);
        System.Console.ForegroundColor = ConsoleColor.Red;
        Console($"[{tag}] {message}", args);
        System.Console.ResetColor();
    }
}