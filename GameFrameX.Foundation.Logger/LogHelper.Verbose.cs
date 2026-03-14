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