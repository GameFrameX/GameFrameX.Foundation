// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;

namespace GameFrameX.Foundation.Utility.Localization;

/// <summary>
/// Utility 模块本地化资源键常量定义
/// </summary>
/// <remarks>
/// 这个类定义了 Utility 模块中所有可本地化字符串的键常量。
/// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
/// </remarks>
/// <example>
/// <code>
/// // 在代码中使用本地化键常量
/// throw new ArgumentOutOfRangeException(
///     nameof(timestamp),
///     LocalizationService.GetString(LocalizationKeys.Exceptions.TimestampOutOfRange));
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
        /// 时间戳超出范围的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Utility.Exceptions.TimestampOutOfRange
        /// 用途: 当传入的时间戳无法转换为有效的DateTime时使用
        /// </remarks>
        public const string TimestampOutOfRange = "Utility.Exceptions.TimestampOutOfRange";

        /// <summary>
        /// 系统时钟回退的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Utility.Exceptions.ClockMovedBackwards
        /// 用途: 当检测到系统时钟回退时，雪花算法ID生成器抛出此异常
        /// 参数: {0} - 回退的毫秒数
        /// </remarks>
        public const string ClockMovedBackwards = "Utility.Exceptions.ClockMovedBackwards";

        /// <summary>
        /// Worker ID 超出范围的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Utility.Exceptions.WorkerIdOutOfRange
        /// 用途: 当创建IdWorker时传入的WorkerId超出有效范围时使用
        /// 参数: {0} - 最大允许的WorkerId值
        /// </remarks>
        public const string WorkerIdOutOfRange = "Utility.Exceptions.WorkerIdOutOfRange";

        /// <summary>
        /// Datacenter ID 超出范围的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Utility.Exceptions.DatacenterIdOutOfRange
        /// 用途: 当创建IdWorker时传入的DatacenterId超出有效范围时使用
        /// 参数: {0} - 最大允许的DatacenterId值
        /// </remarks>
        public const string DatacenterIdOutOfRange = "Utility.Exceptions.DatacenterIdOutOfRange";
    }
}