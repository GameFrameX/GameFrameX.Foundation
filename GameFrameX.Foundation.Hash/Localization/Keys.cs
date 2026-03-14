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

using System;

namespace GameFrameX.Foundation.Hash.Localization;

/// <summary>
/// Hash 模块本地化资源键常量定义
/// </summary>
/// <remarks>
/// 这个类定义了 Hash 模块中所有可本地化字符串的键常量。
/// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
/// </remarks>
/// <example>
/// <code>
/// // 在代码中使用本地化键常量
/// throw new ArgumentException(
///     nameof(data),
///     LocalizationService.GetString(LocalizationKeys.Exceptions.DataCannotBeNullOrEmpty));
/// </code>
/// </example>
public static class LocalizationKeys
{
    /// <summary>
    /// 异常消息资源键
    /// </summary>
    public static class Exceptions
    {
        /// <summary>
        /// 数据不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.DataCannotBeNullOrEmpty
        /// 用途: 当需要非空数据但传入了空数据时使用
        /// </remarks>
        public const string DataCannotBeNullOrEmpty = "Hash.Exceptions.DataCannotBeNullOrEmpty";

        /// <summary>
        /// 哈希计算失败的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.HashComputationFailed
        /// 用途: 当哈希算法计算过程中发生错误时使用
        /// 参数: {0} - 算法名称, {1} - 错误信息
        /// </remarks>
        public const string HashComputationFailed = "Hash.Exceptions.HashComputationFailed";

        /// <summary>
        /// 不支持的哈希算法的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.UnsupportedHashAlgorithm
        /// 用途: 当使用不支持的哈希算法时使用
        /// 参数: {0} - 算法名称
        /// </remarks>
        public const string UnsupportedHashAlgorithm = "Hash.Exceptions.UnsupportedHashAlgorithm";
    }

    /// <summary>
    /// 日志消息资源键
    /// </summary>
    public static class Logs
    {
        /// <summary>
        /// 哈希计算完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Logs.HashComputationCompleted
        /// 用途: 记录哈希计算操作完成
        /// 参数: {0} - 算法名称, {1} - 数据长度, {2} - 哈希值
        /// </remarks>
        public const string HashComputationCompleted = "Hash.Logs.HashComputationCompleted";

        /// <summary>
        /// CRC计算完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Logs.CrcComputationCompleted
        /// 用途: 记录CRC计算操作完成
        /// 参数: {0} - CRC类型, {1} - 数据长度, {2} - CRC值
        /// </remarks>
        public const string CrcComputationCompleted = "Hash.Logs.CrcComputationCompleted";
    }

    /// <summary>
    /// 状态消息资源键
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// 哈希算法初始化成功的状态消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Status.HashAlgorithmInitialized
        /// 用途: 表示哈希算法初始化完成
        /// 参数: {0} - 算法名称
        /// </remarks>
        public const string HashAlgorithmInitialized = "Hash.Status.HashAlgorithmInitialized";

        /// <summary>
        /// CRC校验成功的状态消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Status.CrcValidationSuccessful
        /// 用途: 表示CRC校验通过
        /// 参数: {0} - CRC类型, {1} - 期望值, {2} - 实际值
        /// </remarks>
        public const string CrcValidationSuccessful = "Hash.Status.CrcValidationSuccessful";
    }

    /// <summary>
    /// 性能消息资源键
    /// </summary>
    public static class Performance
    {
        /// <summary>
        /// 哈希计算性能统计
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Performance.HashComputationTime
        /// 用途: 记录哈希计算的性能统计
        /// 参数: {0} - 算法名称, {1} - 数据长度, {2} - 执行时间(毫秒)
        /// </remarks>
        public const string HashComputationTime = "Hash.Performance.HashComputationTime";

        /// <summary>
        /// 哈希吞吐量统计
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Performance.HashThroughput
        /// 用途: 记录哈希计算的吞吐量
        /// 参数: {0} - 算法名称, {1} - 数据量(MB), {2} - 处理时间(秒)
        /// </remarks>
        public const string HashThroughput = "Hash.Performance.HashThroughput";
    }

    /// <summary>
    /// 验证消息资源键
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// 数据长度验证失败的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Validation.InvalidDataLength
        /// 用途: 当数据长度不符合要求时使用
        /// 参数: {0} - 实际长度, {1} - 期望长度范围
        /// </remarks>
        public const string InvalidDataLength = "Hash.Validation.InvalidDataLength";

        /// <summary>
        /// 哈希值验证失败的消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Validation.HashValueMismatch
        /// 用途: 当计算出的哈希值与期望值不匹配时使用
        /// 参数: {0} - 期望值, {1} - 实际值
        /// </remarks>
        public const string HashValueMismatch = "Hash.Validation.HashValueMismatch";

        /// <summary>
        /// CRC校验失败的消息
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Validation.CrcCheckFailed
        /// 用途: 当CRC校验失败时使用
        /// 参数: {0} - CRC类型, {1} - 期望值, {2} - 实际值
        /// </remarks>
        public const string CrcCheckFailed = "Hash.Validation.CrcCheckFailed";
    }
}