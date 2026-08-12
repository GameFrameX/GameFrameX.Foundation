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

namespace GameFrameX.Foundation.Options.Localization;

/// <summary>
/// Options 模块本地化资源键常量定义。
/// 这个类定义了 Options 模块中所有可本地化字符串的键常量。
/// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
/// </summary>
/// <remarks>
/// Options module localization resource key constant definitions.
/// This class defines key constants for all localizable strings in the Options module.
/// Using constants avoids string hardcoding and improves code maintainability and type safety.
/// </remarks>
/// <example>
/// <code>
/// // 在代码中使用本地化键常量
/// // Use localization key constants in code
/// throw new ArgumentException(
///     LocalizationService.GetString(LocalizationKeys.Exceptions.ArgsCannotContainNull),
///     nameof(args));
/// </code>
/// </example>
public static class LocalizationKeys
{
    /// <summary>
    /// 异常消息资源键。
    /// </summary>
    /// <remarks>
    /// Exception message resource keys.
    /// </remarks>
    public static class Exceptions
    {
        /// <summary>
        /// 命令行参数不能包含 null 元素的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for command-line arguments cannot contain null elements.
        /// 键名: Options.Exceptions.ArgsCannotContainNull
        /// 用途: 当命令行参数数组中存在 null 元素时使用
        /// Usage: Used when the command-line argument array contains null elements.
        /// </remarks>
        public const string ArgsCannotContainNull = "Options.Exceptions.ArgsCannotContainNull";

        /// <summary>
        /// 处理命令行参数时发生错误的异常消息。
        /// </summary>
        /// <remarks>
        /// Error message for an error occurred while processing command-line arguments.
        /// 键名: Options.Exceptions.ProcessingError
        /// 用途: 当处理命令行参数过程中出现未知异常时使用
        /// Usage: Used when an unexpected error occurs while processing command-line arguments.
        /// 参数: {0} - 内部异常消息
        /// Parameters: {0} - inner exception message.
        /// </remarks>
        public const string ProcessingError = "Options.Exceptions.ProcessingError";

        /// <summary>
        /// 构建选项时发生错误的异常消息。
        /// </summary>
        /// <remarks>
        /// Error message for an error occurred while building options.
        /// 键名: Options.Exceptions.BuildError
        /// 用途: 当构建选项对象过程中出现未知异常时使用
        /// Usage: Used when an unexpected error occurs while building options.
        /// 参数: {0} - 内部异常消息
        /// Parameters: {0} - inner exception message.
        /// </remarks>
        public const string BuildError = "Options.Exceptions.BuildError";

        /// <summary>
        /// 缺少必需的选项的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for missing required options.
        /// 键名: Options.Exceptions.MissingRequiredOptions
        /// 用途: 当必需的选项未被设置时使用
        /// Usage: Used when required options are not set.
        /// 参数: {0} - 缺失选项名的逗号分隔列表
        /// Parameters: {0} - comma-separated list of missing option names.
        /// </remarks>
        public const string MissingRequiredOptions = "Options.Exceptions.MissingRequiredOptions";

        /// <summary>
        /// 选项值应用失败的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for failed to apply an option value.
        /// 键名: Options.Exceptions.ApplyValueFailed
        /// 用途: 当将选项值写入目标属性失败时使用
        /// Usage: Used when applying an option value to the target property fails.
        /// 参数: {0} - 选项键, {1} - 选项值, {2} - 内部异常消息
        /// Parameters: {0} - option key, {1} - option value, {2} - inner exception message.
        /// </remarks>
        public const string ApplyValueFailed = "Options.Exceptions.ApplyValueFailed";

        /// <summary>
        /// 非字符串选项不能使用空值的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for non-string option cannot use null value.
        /// 键名: Options.Exceptions.NonStringOptionNullValue
        /// 用途: 当非字符串属性接收到空字符串值时使用
        /// Usage: Used when a non-string property receives an empty string value.
        /// </remarks>
        public const string NonStringOptionNullValue = "Options.Exceptions.NonStringOptionNullValue";

        /// <summary>
        /// 布尔值格式无效的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for invalid boolean format.
        /// 键名: Options.Exceptions.InvalidBooleanFormat
        /// 用途: 当布尔属性接收到无法识别的布尔字面量时使用
        /// Usage: Used when a boolean property receives an unrecognized boolean literal.
        /// </remarks>
        public const string InvalidBooleanFormat = "Options.Exceptions.InvalidBooleanFormat";

        /// <summary>
        /// 选项值无法应用到目标属性的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for option value cannot be applied to target property.
        /// 键名: Options.Exceptions.ApplyValueToPropertyFailed
        /// 用途: 当选项值应用到属性抛出 ArgumentException/TargetInvocationException 时使用
        /// Usage: Used when applying an option value throws ArgumentException/TargetInvocationException.
        /// 参数: {0} - 选项键, {1} - 选项值, {2} - 属性名, {3} - 异常消息
        /// Parameters: {0} - option key, {1} - option value, {2} - property name, {3} - exception message.
        /// </remarks>
        public const string ApplyValueToPropertyFailed = "Options.Exceptions.ApplyValueToPropertyFailed";

        /// <summary>
        /// 选项值无法转换为目标类型的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for option value cannot be converted to target type.
        /// 键名: Options.Exceptions.ConvertValueFailed
        /// 用途: 当选项值转换抛出 FormatException/InvalidCastException 时使用
        /// Usage: Used when converting an option value throws FormatException/InvalidCastException.
        /// 参数: {0} - 选项键, {1} - 选项值, {2} - 目标类型名, {3} - 异常消息
        /// Parameters: {0} - option key, {1} - option value, {2} - target type name, {3} - exception message.
        /// </remarks>
        public const string ConvertValueFailed = "Options.Exceptions.ConvertValueFailed";

        /// <summary>
        /// 选项值超出目标类型范围的错误消息。
        /// </summary>
        /// <remarks>
        /// Error message for option value out of target type range.
        /// 键名: Options.Exceptions.ValueOutOfRange
        /// 用途: 当选项值转换抛出 OverflowException 时使用
        /// Usage: Used when converting an option value throws OverflowException.
        /// 参数: {0} - 选项键, {1} - 选项值, {2} - 目标类型名, {3} - 异常消息
        /// Parameters: {0} - option key, {1} - option value, {2} - target type name, {3} - exception message.
        /// </remarks>
        public const string ValueOutOfRange = "Options.Exceptions.ValueOutOfRange";
    }
}
