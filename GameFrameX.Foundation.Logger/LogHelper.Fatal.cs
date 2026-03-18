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
using Serilog;

namespace GameFrameX.Foundation.Logger;

public static partial class LogHelper
{
    /// <summary>
    /// 记录严重错误消息。
    /// </summary>
    /// <param name="message">要记录的严重错误消息 / The fatal error message to record</param>
    /// <remarks>
    /// Records fatal error level log information and includes stack trace information.
    /// </remarks>
    public static void Fatal(string message)
    {
        var newMessage = $"{message}\n{new StackTrace(1, true)}";

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 记录带有单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <remarks>
    /// Uses structured logging to record fatal error level messages with a single property.
    /// </remarks>
    public static void Fatal<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Fatal(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <remarks>
    /// Uses structured logging to record fatal error level messages with two properties.
    /// </remarks>
    public static void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Fatal(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <typeparam name="T2">第三个属性值的类型 / The type of the third property value</typeparam>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <param name="propertyValue2">第三个属性值 / The third property value</param>
    /// <remarks>
    /// Uses structured logging to record fatal error level messages with three properties.
    /// </remarks>
    public static void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }


    /// <summary>
    /// 记录带有异常和单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <remarks>
    /// Records fatal error level exception information and message template with a single property.
    /// </remarks>
    public static void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <remarks>
    /// Records fatal error level exception information and message template with two properties.
    /// </remarks>
    public static void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的致命错误消息模板。
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
    /// Records fatal error level exception information and message template with three properties.
    /// </remarks>
    public static void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的致命错误消息模板。
    /// </summary>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValues">属性值数组 / The property value array</param>
    /// <remarks>
    /// Records fatal error level exception information and message template with multiple properties.
    /// </remarks>
    public static void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Fatal(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 使用指定的日志记录器记录严重错误消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="message">要记录的严重错误消息 / The fatal error message to record</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    public static void Fatal(ILogger logger, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = $"{message}\n{new StackTrace(1, true)}";
        logger.Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Uses structured logging to record fatal error level messages with a single property.
    /// </remarks>
    public static void Fatal<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型 / The type of the first property value</typeparam>
    /// <typeparam name="T1">第二个属性值的类型 / The type of the second property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue0">第一个属性值 / The first property value</param>
    /// <param name="propertyValue1">第二个属性值 / The second property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Uses structured logging to record fatal error level messages with two properties.
    /// </remarks>
    public static void Fatal<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的致命错误消息模板。
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
    /// Uses structured logging to record fatal error level messages with three properties.
    /// </remarks>
    public static void Fatal<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的致命错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records fatal error level exception information and message template.
    /// </remarks>
    public static void Fatal(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的致命错误消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型 / The type of the property value</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValue">属性值 / The property value</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records fatal error level exception information and message template with a single property.
    /// </remarks>
    public static void Fatal<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的致命错误消息模板。
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
    /// Records fatal error level exception information and message template with two properties.
    /// </remarks>
    public static void Fatal<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的致命错误消息模板。
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
    /// Records fatal error level exception information and message template with three properties.
    /// </remarks>
    public static void Fatal<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的致命错误消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">异常信息 / The exception information</param>
    /// <param name="messageTemplate">消息模板 / The message template</param>
    /// <param name="propertyValues">属性值数组 / The property value array</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    /// <remarks>
    /// Records fatal error level exception information and message template with multiple properties.
    /// </remarks>
    public static void Fatal(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Fatal(exception, messageTemplate, propertyValues);
    }

    /// <summary>
    /// 记录严重的异常错误。
    /// </summary>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <remarks>
    /// Records the exception as a fatal error level log and includes stack trace information.
    /// </remarks>
    public static void Fatal(Exception exception)
    {
        var newMessage = $"{exception}\n{new StackTrace(1, true)}";

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录严重的异常错误。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    public static void Fatal(ILogger logger, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = $"{exception}\n{new StackTrace(1, true)}";
        logger.Fatal(newMessage);
    }

    /// <summary>
    /// 记录带有标签的严重错误消息。
    /// </summary>
    /// <param name="tag">日志标签，用于标识日志来源或分类 / The log tag used to identify the log source or category</param>
    /// <param name="message">要记录的严重错误消息 / The fatal error message to record</param>
    /// <remarks>
    /// The recorded message will include a tag prefix and stack trace information.
    /// </remarks>
    public static void Fatal(string tag, string message)
    {
        var newMessage = $"[{tag}] {message}\n{new StackTrace(1, true)}";

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的严重错误消息。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="tag">日志标签，用于标识日志来源或分类 / The log tag used to identify the log source or category</param>
    /// <param name="message">要记录的严重错误消息 / The fatal error message to record</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    public static void Fatal(ILogger logger, string tag, string message)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = $"[{tag}] {message}\n{new StackTrace(1, true)}";
        logger.Fatal(newMessage);
    }

    /// <summary>
    /// 记录带有标签的严重异常错误。
    /// </summary>
    /// <param name="tag">日志标签，用于标识日志来源或分类 / The log tag used to identify the log source or category</param>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <remarks>
    /// The recorded exception information will include a tag prefix and stack trace information.
    /// </remarks>
    public static void Fatal(string tag, Exception exception)
    {
        var newMessage = $"[{tag}] {exception}\n{new StackTrace(1, true)}";

        GetLogger().Fatal(newMessage);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有标签的严重异常错误。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例 / The ILogger instance for logging</param>
    /// <param name="tag">日志标签，用于标识日志来源或分类 / The log tag used to identify the log source or category</param>
    /// <param name="exception">要记录的异常对象 / The exception object to record</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="logger"/> 为 null 时抛出 / Thrown when <paramref name="logger"/> is null</exception>
    public static void Fatal(ILogger logger, string tag, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(logger);
        var newMessage = $"[{tag}] {exception}\n{new StackTrace(1, true)}";
        logger.Fatal(newMessage);
    }
}