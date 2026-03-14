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

        /// <summary>
        /// 小时参数超出范围的错误消息
        /// </summary>
        /// <remarks>
        /// 键名: Utility.Exceptions.HourOutOfRange
        /// 用途: 当传入的小时参数不在 0-23 范围内时使用
        /// </remarks>
        public const string HourOutOfRange = "Utility.Exceptions.HourOutOfRange";
    }

    /// <summary>
    /// 星期名称本地化资源键
    /// </summary>
    public static class DayOfWeek
    {
        /// <summary>
        /// 星期日
        /// </summary>
        public const string Sunday = "Utility.DayOfWeek.Sunday";

        /// <summary>
        /// 星期一
        /// </summary>
        public const string Monday = "Utility.DayOfWeek.Monday";

        /// <summary>
        /// 星期二
        /// </summary>
        public const string Tuesday = "Utility.DayOfWeek.Tuesday";

        /// <summary>
        /// 星期三
        /// </summary>
        public const string Wednesday = "Utility.DayOfWeek.Wednesday";

        /// <summary>
        /// 星期四
        /// </summary>
        public const string Thursday = "Utility.DayOfWeek.Thursday";

        /// <summary>
        /// 星期五
        /// </summary>
        public const string Friday = "Utility.DayOfWeek.Friday";

        /// <summary>
        /// 星期六
        /// </summary>
        public const string Saturday = "Utility.DayOfWeek.Saturday";
    }
}