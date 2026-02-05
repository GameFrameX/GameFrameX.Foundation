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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

namespace GameFrameX.Foundation.Utility;

public partial class TimerHelper
{
    /// <summary>
    /// 获取本月开始时间
    /// </summary>
    /// <returns>本月1号00:00:00的时间</returns>
    /// <remarks>
    /// 此方法返回本月第一天的零点时间
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetMonthStartTime()
    {
        var now = GetNowWithTimeZone();
        return new DateTime(now.Year, now.Month, 1);
    }

    /// <summary>
    /// 获取本月开始时间戳
    /// </summary>
    /// <returns>本月1号00:00:00的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回本月第一天零点时间的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetMonthStartTimestamp()
    {
        var date = GetMonthStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取本月开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>本月1号00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetMonthStartTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetMonthStartTime());
    }

    /// <summary>
    /// 获取本月结束时间
    /// </summary>
    /// <returns>本月最后一天23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回本月最后一天的最后一秒
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetMonthEndTime()
    {
        var now = GetNowWithTimeZone();
        return GetStartTimeOfMonth(now).AddMonths(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取本月结束时间戳
    /// </summary>
    /// <returns>本月最后一天23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回本月最后一天最后一秒的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetMonthEndTimestamp()
    {
        var date = GetMonthEndTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取本月结束时间戳（基于设置时区）
    /// </summary>
    /// <returns>本月最后一天23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetMonthEndTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetMonthEndTime());
    }

    /// <summary>
    /// 获取指定日期所在月的开始时间戳
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在月1号00:00:00的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回指定日期所在月第一天零点时间的Unix时间戳
    /// 会使用当前时区 (<see cref="CurrentTimeZone"/>) 计算偏移量并将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetStartTimestampOfMonth(DateTime date)
    {
        var time = GetStartTimeOfMonth(date);
        return DateTimeToUnixTimeSeconds(time);
    }

    /// <summary>
    /// 获取指定日期所在月的开始时间戳（基于设置时区）
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在月1号00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetStartTimestampOfMonthWithTimeZone(DateTime date)
    {
        return TimeToSecondsWithTimeZone(GetStartTimeOfMonth(date));
    }

    /// <summary>
    /// 获取指定日期所在月的结束时间戳
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在月最后一天23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回指定日期所在月最后一天最后一秒的Unix时间戳
    /// 会使用当前时区 (<see cref="CurrentTimeZone"/>) 计算偏移量并将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetEndTimestampOfMonth(DateTime date)
    {
        var time = GetEndTimeOfMonth(date);
        return DateTimeToUnixTimeSeconds(time);
    }

    /// <summary>
    /// 获取指定日期所在月的结束时间戳（基于设置时区）
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在月最后一天23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetEndTimestampOfMonthWithTimeZone(DateTime date)
    {
        return TimeToSecondsWithTimeZone(GetEndTimeOfMonth(date));
    }

    /// <summary>
    /// 获取下个月开始时间
    /// </summary>
    /// <returns>下个月1号00:00:00的时间</returns>
    /// <remarks>
    /// 此方法返回下个月第一天的零点时间
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetNextMonthStartTime()
    {
        return GetMonthStartTime().AddMonths(1);
    }

    /// <summary>
    /// 获取下个月开始时间戳
    /// </summary>
    /// <returns>下个月1号00:00:00的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回下个月第一天零点时间的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetNextMonthStartTimestamp()
    {
        var date = GetNextMonthStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取下个月开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>下个月1号00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetNextMonthStartTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetNextMonthStartTime());
    }

    /// <summary>
    /// 获取下个月结束时间
    /// </summary>
    /// <returns>下个月最后一天23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回下个月最后一天的最后一秒
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetNextMonthEndTime()
    {
        return GetNextMonthStartTime().AddMonths(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取下个月结束时间戳
    /// </summary>
    /// <returns>下个月最后一天23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回下个月最后一天最后一秒的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetNextMonthEndTimestamp()
    {
        var date = GetNextMonthEndTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取下个月结束时间戳（基于设置时区）
    /// </summary>
    /// <returns>下个月最后一天23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetNextMonthEndTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetNextMonthEndTime());
    }
}
