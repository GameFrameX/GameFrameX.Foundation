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