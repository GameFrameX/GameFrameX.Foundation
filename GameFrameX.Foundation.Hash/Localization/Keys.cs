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

        /// <summary>
        /// bcrypt 工作因子越界的错误消息 / Out-of-range bcrypt work factor error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.WorkFactorOutOfRange
        /// 用途: 当 bcrypt 工作因子(cost)不在 [4, 31] 范围内时使用
        /// </remarks>
        public const string WorkFactorOutOfRange = "Hash.Exceptions.WorkFactorOutOfRange";

        /// <summary>
        /// 迭代次数必须为正数的错误消息 / Iterations must be positive error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.IterationsMustBePositive
        /// 用途: 当 KDF 的迭代次数小于 1 时使用（PBKDF2 / Argon2id 共用）
        /// </remarks>
        public const string IterationsMustBePositive = "Hash.Exceptions.IterationsMustBePositive";

        /// <summary>
        /// 输出长度必须为正的错误消息 / Output length must be positive error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.OutputBytesMustBePositive
        /// 用途: 当输出哈希长度（字节）小于 1 时使用（PBKDF2 / scrypt / Argon2id 共用）
        /// </remarks>
        public const string OutputBytesMustBePositive = "Hash.Exceptions.OutputBytesMustBePositive";

        /// <summary>
        /// scrypt N 必须为 2 的幂的错误消息 / scrypt N must be a power of two error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.ScryptNMustBePowerOfTwo
        /// 用途: 当 scrypt 的 N 参数小于 2 或不为 2 的幂时使用
        /// </remarks>
        public const string ScryptNMustBePowerOfTwo = "Hash.Exceptions.ScryptNMustBePowerOfTwo";

        /// <summary>
        /// scrypt r 必须为正数的错误消息 / scrypt r must be positive error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.ScryptRMustBePositive
        /// 用途: 当 scrypt 的块大小参数 r 小于 1 时使用
        /// </remarks>
        public const string ScryptRMustBePositive = "Hash.Exceptions.ScryptRMustBePositive";

        /// <summary>
        /// scrypt p 必须为正数的错误消息 / scrypt p must be positive error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.ScryptPMustBePositive
        /// 用途: 当 scrypt 的并行参数 p 小于 1 时使用
        /// </remarks>
        public const string ScryptPMustBePositive = "Hash.Exceptions.ScryptPMustBePositive";

        /// <summary>
        /// 不支持的密码哈希算法种类的错误消息 / Unsupported password hash algorithm kind error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.UnsupportedPasswordHashKind
        /// 用途: 当 PasswordHashHelper.Hash 收到未知的 PasswordHashAlgorithmKind 时使用
        /// </remarks>
        public const string UnsupportedPasswordHashKind = "Hash.Exceptions.UnsupportedPasswordHashKind";

        /// <summary>
        /// Argon2 内存开销过小的错误消息 / Argon2 memory cost too small error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.Argon2MemoryTooSmall
        /// 用途: 当 Argon2id 的内存开销（KB）小于 8 时使用
        /// </remarks>
        public const string Argon2MemoryTooSmall = "Hash.Exceptions.Argon2MemoryTooSmall";

        /// <summary>
        /// 并行度必须为正数的错误消息 / Parallelism must be positive error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.ParallelismMustBePositive
        /// 用途: 当 Argon2id 的并行度参数小于 1 时使用
        /// </remarks>
        public const string ParallelismMustBePositive = "Hash.Exceptions.ParallelismMustBePositive";

        /// <summary>
        /// 密码 UTF-8 字节数超过 bcrypt 限制的错误消息 / Password exceeds bcrypt byte limit error message.
        /// </summary>
        /// <remarks>
        /// 键名: Hash.Exceptions.PasswordExceedsBcryptLimit
        /// 用途: 当密码的 UTF-8 字节数超过 bcrypt 协议硬限制 72 字节时使用
        /// </remarks>
        public const string PasswordExceedsBcryptLimit = "Hash.Exceptions.PasswordExceedsBcryptLimit";
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