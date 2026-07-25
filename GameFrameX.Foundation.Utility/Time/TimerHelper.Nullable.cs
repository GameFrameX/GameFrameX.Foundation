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

namespace GameFrameX.Foundation.Utility;

/// <summary>
/// TimerHelper 的 nullable（可空）重载，统一 <see cref="DateTime"/>? / long? 边界的时间转换入口。
/// </summary>
/// <remarks>
/// Nullable overloads for <see cref="TimerHelper"/>. All overloads delegate to the existing non-null
/// implementations; a <c>null</c> input short-circuits to <c>null</c>. The non-null path is byte-for-byte
/// identical to the corresponding base method (zero behavior change).
/// 覆盖下游秒级 / 毫秒级 × 正向 / 反向的全部 nullable 迁移点，避免下游自建并行封装、
/// 消除本地时区偏移混入导致的同一 <see cref="DateTime"/> 得到不同时间戳的问题。
/// </remarks>
public static partial class TimerHelper
{
    /// <summary>
    /// 将可空 <see cref="DateTime"/> 转换为距离纪元时间的秒数。输入为 <c>null</c> 时返回 <c>null</c>。
    /// </summary>
    /// <remarks>
    /// Nullable overload of <see cref="DateTimeToSecond(DateTime, bool)"/>. A <c>null</c> input returns <c>null</c>;
    /// a non-null input is delegated to the base method with identical behavior.
    /// </remarks>
    /// <param name="time">要转换的可空指定时间 / The nullable specified time to convert</param>
    /// <param name="utc">指定使用的纪元时间类型。如果为 <c>true</c>，使用 UTC 纪元时间；如果为 <c>false</c>，使用当前设置时区的纪元时间。默认值为 <c>false</c> / Specifies the type of epoch time to use. If <c>true</c>, uses UTC epoch time; if <c>false</c>, uses the epoch time of the currently set time zone. Default is <c>false</c></param>
    /// <returns>返回可空 <see cref="long"/>，表示指定时间距离相应纪元时间的秒数；输入为 <c>null</c> 时返回 <c>null</c> / A nullable <see cref="long"/> representing the number of seconds from the specified time to the corresponding epoch time; returns <c>null</c> when the input is <c>null</c>.</returns>
    /// <seealso cref="DateTimeToSecond(DateTime, bool)"/>
    public static long? DateTimeToSecond(DateTime? time, bool utc = false)
    {
        if (!time.HasValue)
        {
            return null;
        }

        return DateTimeToSecond(time.Value, utc);
    }

    /// <summary>
    /// 将可空 <see cref="DateTime"/> 转换为距离纪元时间的毫秒数。输入为 <c>null</c> 时返回 <c>null</c>。
    /// </summary>
    /// <remarks>
    /// Nullable overload of <see cref="DateTimeToMilliseconds(DateTime, bool)"/>. A <c>null</c> input returns <c>null</c>;
    /// a non-null input is delegated to the base method with identical behavior.
    /// </remarks>
    /// <param name="time">要转换的可空指定时间 / The nullable specified time to convert</param>
    /// <param name="utc">指定使用的纪元时间类型。如果为 <c>true</c>，使用 UTC 纪元时间；如果为 <c>false</c>，使用当前设置时区的纪元时间。默认值为 <c>false</c> / Specifies the type of epoch time to use. If <c>true</c>, uses UTC epoch time; if <c>false</c>, uses the epoch time of the currently set time zone. Default is <c>false</c></param>
    /// <returns>返回可空 <see cref="long"/>，表示指定时间距离相应纪元时间的毫秒数；输入为 <c>null</c> 时返回 <c>null</c> / A nullable <see cref="long"/> representing the number of milliseconds from the specified time to the corresponding epoch time; returns <c>null</c> when the input is <c>null</c>.</returns>
    /// <seealso cref="DateTimeToMilliseconds(DateTime, bool)"/>
    public static long? DateTimeToMilliseconds(DateTime? time, bool utc = false)
    {
        if (!time.HasValue)
        {
            return null;
        }

        return DateTimeToMilliseconds(time.Value, utc);
    }

    /// <summary>
    /// 将可空秒级 Unix 时间戳转换为 <see cref="DateTime"/>。输入为 <c>null</c> 时返回 <c>null</c>。
    /// </summary>
    /// <remarks>
    /// Nullable overload of <see cref="TimestampSecondToDateTime(long, bool)"/>. A <c>null</c> input returns <c>null</c>;
    /// a non-null input is delegated to the base method with identical behavior.
    /// </remarks>
    /// <param name="utcTimestampSeconds">可空秒时间戳 / The nullable second timestamp</param>
    /// <param name="utc">是否使用UTC时间 / Whether to use UTC time</param>
    /// <returns>转换后的可空时间；输入为 <c>null</c> 时返回 <c>null</c>。如果 utc 为 false，则返回当前时区 (<see cref="CurrentTimeZone"/>) 的时间 / The converted nullable time; returns <c>null</c> when the input is <c>null</c>. If utc is false, returns the time in the current time zone (<see cref="CurrentTimeZone"/>)</returns>
    /// <seealso cref="TimestampSecondToDateTime(long, bool)"/>
    public static DateTime? TimestampSecondToDateTime(long? utcTimestampSeconds, bool utc = false)
    {
        if (!utcTimestampSeconds.HasValue)
        {
            return null;
        }

        return TimestampSecondToDateTime(utcTimestampSeconds.Value, utc);
    }

    /// <summary>
    /// 将可空毫秒级 Unix 时间戳转换为 <see cref="DateTime"/>。输入为 <c>null</c> 时返回 <c>null</c>。
    /// </summary>
    /// <remarks>
    /// Nullable overload of <see cref="TimeStampMillisecondToDateTime(long, bool)"/>. A <c>null</c> input returns <c>null</c>;
    /// a non-null input is delegated to the base method with identical behavior.
    /// </remarks>
    /// <param name="utcTimestampMilliseconds">可空毫秒时间戳 / The nullable millisecond timestamp</param>
    /// <param name="utc">是否使用UTC时间 / Whether to use UTC time</param>
    /// <returns>转换后的可空时间；输入为 <c>null</c> 时返回 <c>null</c>。如果 utc 为 false，则返回当前时区 (<see cref="CurrentTimeZone"/>) 的时间 / The converted nullable time; returns <c>null</c> when the input is <c>null</c>. If utc is false, returns the time in the current time zone (<see cref="CurrentTimeZone"/>)</returns>
    /// <seealso cref="TimeStampMillisecondToDateTime(long, bool)"/>
    public static DateTime? TimeStampMillisecondToDateTime(long? utcTimestampMilliseconds, bool utc = false)
    {
        if (!utcTimestampMilliseconds.HasValue)
        {
            return null;
        }

        return TimeStampMillisecondToDateTime(utcTimestampMilliseconds.Value, utc);
    }
}
