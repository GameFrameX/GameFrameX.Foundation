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
    /// 记录带有单个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的警告消息。
    /// </remarks>
    public static void Warning<T>(string messageTemplate, T propertyValue)
    {
        GetLogger().Warning(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有两个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的警告消息。
    /// </remarks>
    public static void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Warning(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有三个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <typeparam name="T2">第三个属性值的类型。</typeparam>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <param name="propertyValue2">第三个属性值。</param>
    /// <remarks>
    /// 使用结构化日志记录带有三个属性的警告消息。
    /// </remarks>
    public static void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常的警告消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <remarks>
    /// 记录警告级别的异常信息和消息模板。
    /// </remarks>
    public static void Warning(Exception exception, string messageTemplate)
    {
        GetLogger().Warning(exception, messageTemplate);
    }

    /// <summary>
    /// 记录带有异常和单个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <remarks>
    /// 记录警告级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
    {
        GetLogger().Warning(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 记录带有异常和两个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <remarks>
    /// 记录警告级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        GetLogger().Warning(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 记录带有异常和三个属性值的警告消息模板。
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
    /// 记录警告级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        GetLogger().Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 记录带有异常和格式参数的警告消息模板。
    /// </summary>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <remarks>
    /// 记录警告级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
    {
        GetLogger().Warning(exception, messageTemplate, propertyValues);
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
    /// 使用指定的日志记录器记录带有单个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有单个属性的警告消息。
    /// </remarks>
    public static void Warning<T>(ILogger logger, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有两个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T0">第一个属性值的类型。</typeparam>
    /// <typeparam name="T1">第二个属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue0">第一个属性值。</param>
    /// <param name="propertyValue1">第二个属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 使用结构化日志记录带有两个属性的警告消息。
    /// </remarks>
    public static void Warning<T0, T1>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有三个属性值的警告消息模板。
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
    /// 使用结构化日志记录带有三个属性的警告消息。
    /// </remarks>
    public static void Warning<T0, T1, T2>(ILogger logger, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常的警告消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录警告级别的异常信息和消息模板。
    /// </remarks>
    public static void Warning(ILogger logger, Exception exception, string messageTemplate)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(exception, messageTemplate);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和单个属性值的警告消息模板。
    /// </summary>
    /// <typeparam name="T">属性值的类型。</typeparam>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValue">属性值。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录警告级别的异常信息和带有单个属性的消息模板。
    /// </remarks>
    public static void Warning<T>(ILogger logger, Exception exception, string messageTemplate, T propertyValue)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(exception, messageTemplate, propertyValue);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和两个属性值的警告消息模板。
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
    /// 记录警告级别的异常信息和带有两个属性的消息模板。
    /// </remarks>
    public static void Warning<T0, T1>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和三个属性值的警告消息模板。
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
    /// 记录警告级别的异常信息和带有三个属性的消息模板。
    /// </remarks>
    public static void Warning<T0, T1, T2>(ILogger logger, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
    }

    /// <summary>
    /// 使用指定的日志记录器记录带有异常和格式参数的警告消息模板。
    /// </summary>
    /// <param name="logger">用于记录日志的ILogger实例</param>
    /// <param name="exception">异常信息。</param>
    /// <param name="messageTemplate">消息模板。</param>
    /// <param name="propertyValues">属性值数组。</param>
    /// <exception cref="ArgumentNullException">当logger参数为null时抛出此异常</exception>
    /// <remarks>
    /// 记录警告级别的异常信息和带有多个属性的消息模板。
    /// </remarks>
    public static void Warning(ILogger logger, Exception exception, string messageTemplate, params object[] propertyValues)
    {
        ArgumentNullException.ThrowIfNull(logger);
        logger.Warning(exception, messageTemplate, propertyValues);
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
        Warning(message, args);
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
        Warning(tag, message, args);
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        Console($"[{tag}] {message}", args);
        System.Console.ResetColor();
    }
}