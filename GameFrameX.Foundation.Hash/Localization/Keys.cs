// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

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