// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;

namespace GameFrameX.Foundation.Extensions.Localization;

/// <summary>
/// Extensions 模块本地化资源键常量定义
/// </summary>
/// <remarks>
/// 这个类定义了 Extensions 模块中所有可本地化字符串的键常量。
/// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
/// </remarks>
/// <example>
/// <code>
/// // 在代码中使用本地化键常量
/// throw new ArgumentException(
///     nameof(buffer),
///     LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetCountExceedBufferLength));
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
        /// 偏移量和计数超出缓冲区长度的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.OffsetCountExceedBufferLength
        /// 用途: 当尝试从缓冲区读取超出范围的字节时使用
        /// 参数: {0} - 缓冲区长度, {1} - 偏移量, {2} - 计数
        /// </remarks>
        public const string OffsetCountExceedBufferLength = "Extensions.Exceptions.OffsetCountExceedBufferLength";

        /// <summary>
        /// 索引和计数超出缓冲区长度的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.IndexCountExceedBufferLength
        /// 用途: 当尝试从缓冲区索引超出范围时使用
        /// 参数: {0} - 缓冲区长度, {1} - 索引, {2} - 计数
        /// </remarks>
        public const string IndexCountExceedBufferLength = "Extensions.Exceptions.IndexCountExceedBufferLength";

        /// <summary>
        /// 缓冲区写入索引超出范围的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.BufferWriteOutOfRange
        /// 用途: 当尝试写入超出缓冲区范围的索引时使用
        /// 参数: {0} - 缓冲区长度, {1} - 写入位置
        /// </remarks>
        public const string BufferWriteOutOfRange = "Extensions.Exceptions.BufferWriteOutOfRange";

        /// <summary>
        /// 字符串长度超出最大值的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.StringLengthExceedMaxValue
        /// 用途: 当字符串长度超过允许的最大值时使用
        /// 参数: {0} - 实际长度, {1} - 最大长度
        /// </remarks>
        public const string StringLengthExceedMaxValue = "Extensions.Exceptions.StringLengthExceedMaxValue";

        /// <summary>
        /// 字节数组长度必须相等的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.ByteArrayLengthMustEqual
        /// 用途: 当两个字节数组长度不匹配时使用
        /// 参数: {0} - 第一个数组长度, {1} - 第二个数组长度
        /// </remarks>
        public const string ByteArrayLengthMustEqual = "Extensions.Exceptions.ByteArrayLengthMustEqual";

        /// <summary>
        /// 列表不能为空的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.ListCannotBeEmpty
        /// 用途: 当需要非空列表但传入空列表时使用
        /// </remarks>
        public const string ListCannotBeEmpty = "Extensions.Exceptions.ListCannotBeEmpty";

        /// <summary>
        /// 目标类型必须是接口类型的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.TargetTypeMustBeInterface
        /// 用途: 当类型转换的目标类型不是接口时使用
        /// 参数: {0} - 目标类型名称
        /// </remarks>
        public const string TargetTypeMustBeInterface = "Extensions.Exceptions.TargetTypeMustBeInterface";

        /// <summary>
        /// 对象类型不匹配的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Exceptions.ObjectTypeMismatch
        /// 用途: 当对象无法转换为目标类型时使用
        /// 参数: {0} - 对象类型, {1} - 目标类型
        /// </remarks>
        public const string ObjectTypeMismatch = "Extensions.Exceptions.ObjectTypeMismatch";
    }

    /// <summary>
    /// 日志消息资源键
    /// </summary>
    public static class Logs
    {
        /// <summary>
        /// 集合处理完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Logs.CollectionProcessed
        /// 用途: 记录集合处理操作完成
        /// 参数: {0} - 集合类型, {1} - 元素数量
        /// </remarks>
        public const string CollectionProcessed = "Extensions.Logs.CollectionProcessed";

        /// <summary>
        /// 字节转换完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Logs.ByteConversionCompleted
        /// 用途: 记录字节转换操作完成
        /// 参数: {0} - 源格式, {1} - 目标格式, {2} - 字节数
        /// </remarks>
        public const string ByteConversionCompleted = "Extensions.Logs.ByteConversionCompleted";

        /// <summary>
        /// 类型转换完成的日志消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Logs.TypeConversionCompleted
        /// 用途: 记录类型转换操作完成
        /// 参数: {0} - 源类型, {1} - 目标类型
        /// </remarks>
        public const string TypeConversionCompleted = "Extensions.Logs.TypeConversionCompleted";
    }

    /// <summary>
    /// 状态消息资源键
    /// </summary>
    public static class Status
    {
        /// <summary>
        /// 扩展方法初始化成功的状态消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Status.ExtensionsInitialized
        /// 用途: 表示扩展方法模块初始化完成
        /// 参数: {0} - 模块名称
        /// </remarks>
        public const string ExtensionsInitialized = "Extensions.Status.ExtensionsInitialized";

        /// <summary>
        /// 缓冲区操作成功的状态消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Status.BufferOperationSuccessful
        /// 用途: 表示缓冲区操作成功完成
        /// 参数: {0} - 操作类型, {1} - 字节数
        /// </remarks>
        public const string BufferOperationSuccessful = "Extensions.Status.BufferOperationSuccessful";
    }

    /// <summary>
    /// 验证消息资源键
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// 参数不能为null的验证消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Validation.NotNullRequired
        /// 用途: 当参数不允许为null但传入了null时使用
        /// 参数: {0} - 参数名称
        /// </remarks>
        public const string NotNullRequired = "Extensions.Validation.NotNullRequired";

        /// <summary>
        /// 范围验证失败的验证消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Validation.RangeCheckFailed
        /// 用途: 当数值超出允许范围时使用
        /// 参数: {0} - 值, {1} - 最小值, {2} - 最大值
        /// </remarks>
        public const string RangeCheckFailed = "Extensions.Validation.RangeCheckFailed";

        /// <summary>
        /// 类型验证失败的验证消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Validation.TypeCheckFailed
        /// 用途: 当对象不是期望类型时使用
        /// 参数: {0} - 实际类型, {1} - 期望类型
        /// </remarks>
        public const string TypeCheckFailed = "Extensions.Validation.TypeCheckFailed";
    }

    /// <summary>
    /// 性能消息资源键
    /// </summary>
    public static class Performance
    {
        /// <summary>
        /// 操作执行时间的性能消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Performance.OperationExecutionTime
        /// 用途: 记录操作执行时间
        /// 参数: {0} - 操作名称, {1} - 执行时间(毫秒)
        /// </remarks>
        public const string OperationExecutionTime = "Extensions.Performance.OperationExecutionTime";

        /// <summary>
        /// 内存使用情况的性能消息
        /// </summary>
        /// <remarks>
        /// 键名: Extensions.Performance.MemoryUsage
        /// 用途: 记录内存使用情况
        /// 参数: {0} - 操作名称, {1} - 内存使用量(字节)
        /// </remarks>
        public const string MemoryUsage = "Extensions.Performance.MemoryUsage";
    }
}