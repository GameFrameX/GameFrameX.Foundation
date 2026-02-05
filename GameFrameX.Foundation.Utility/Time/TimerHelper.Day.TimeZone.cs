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
    /// 获取今天开始时间
    /// </summary>
    /// <returns>今天零点时间</returns>
    /// <remarks>
    /// 此方法返回当天的零点时间(00:00:00)
    /// 使用 <see cref="GetNowWithTimeZone"/> 获取当前日期的零点时间
    /// 返回的是 <see cref="CurrentTimeZone"/> 时区的时间
    /// </remarks>
    public static DateTime GetTodayStartTime()
    {
        return GetNowWithTimeZone().Date;
    }

    /// <summary>
    /// 获取今天开始时间戳
    /// </summary>
    /// <returns>今天零点时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回当天零点时间的Unix时间戳
    /// 先获取 <see cref="CurrentTimeZone"/> 时区的今天零点时间,然后转换为时间戳
    /// 返回从1970-01-01 00:00:00 UTC开始的秒数
    /// </remarks>
    public static long GetTodayStartTimestamp()
    {
        var date = GetTodayStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取今天开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>今天零点时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// 适用于需要伪造本地时间戳的场景
    /// </remarks>
    public static long GetTodayStartTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetTodayStartTime());
    }

    /// <summary>
    /// 获取今天结束时间
    /// </summary>
    /// <returns>今天23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回当天的最后一秒(23:59:59)
    /// 通过获取明天零点时间然后减去1秒来计算
    /// 返回的是 <see cref="CurrentTimeZone"/> 时区的时间
    /// </remarks>
    public static DateTime GetTodayEndTime()
    {
        return GetTodayStartTime().AddDays(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取今天结束时间戳
    /// </summary>
    /// <returns>今天23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回当天最后一秒的Unix时间戳
    /// 先获取 <see cref="CurrentTimeZone"/> 时区的今天23:59:59,然后转换为时间戳
    /// 返回从1970-01-01 00:00:00 UTC开始的秒数
    /// </remarks>
    public static long GetTodayEndTimestamp()
    {
        var date = GetTodayEndTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取今天结束时间戳（基于设置时区）
    /// </summary>
    /// <returns>今天23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetTodayEndTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetTodayEndTime());
    }

    /// <summary>
    /// 获取指定日期的开始时间
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>指定日期零点时间</returns>
    /// <remarks>
    /// 此方法返回指定日期的零点时间(00:00:00)
    /// 例如:输入2024-01-10 14:30:00,返回2024-01-10 00:00:00
    /// 保持原有时区不变
    /// </remarks>
    public static DateTime GetStartTimeOfDay(DateTime date)
    {
        return date.Date;
    }

    /// <summary>
    /// 获取指定日期的开始时间戳
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>指定日期零点时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回指定日期零点时间的Unix时间戳
    /// 例如:输入2024-01-10 14:30:00,返回2024-01-10 00:00:00的时间戳
    /// 会使用当前时区 (<see cref="CurrentTimeZone"/>) 计算偏移量并将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetStartTimestampOfDay(DateTime date)
    {
        var targetDate = GetStartTimeOfDay(date);
        return DateTimeToUnixTimeSeconds(targetDate);
    }

    /// <summary>
    /// 获取指定日期的开始时间戳（基于设置时区）
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>指定日期零点时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetStartTimestampOfDayWithTimeZone(DateTime date)
    {
        return TimeToSecondsWithTimeZone(GetStartTimeOfDay(date));
    }

    /// <summary>
    /// 获取指定日期的结束时间
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>指定日期23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回指定日期的最后一秒(23:59:59)
    /// 例如:输入2024-01-10 14:30:00,返回2024-01-10 23:59:59
    /// 保持原有时区不变
    /// </remarks>
    public static DateTime GetEndTimeOfDay(DateTime date)
    {
        return date.Date.AddDays(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取指定日期的结束时间戳
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>指定日期23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回指定日期最后一秒的Unix时间戳
    /// 例如:输入2024-01-10 14:30:00,返回2024-01-10 23:59:59的时间戳
    /// 会使用当前时区 (<see cref="CurrentTimeZone"/>) 计算偏移量并将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetEndTimestampOfDay(DateTime date)
    {
        var targetDate = GetEndTimeOfDay(date);
        return DateTimeToUnixTimeSeconds(targetDate);
    }

    /// <summary>
    /// 获取指定日期的结束时间戳（基于设置时区）
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>指定日期23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetEndTimestampOfDayWithTimeZone(DateTime date)
    {
        return TimeToSecondsWithTimeZone(GetEndTimeOfDay(date));
    }

    /// <summary>
    /// 获取明天开始时间
    /// </summary>
    /// <returns>明天零点时间</returns>
    /// <remarks>
    /// 此方法返回明天的零点时间
    /// 例如:当前是2024-01-10,返回2024-01-11 00:00:00
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算时间
    /// </remarks>
    public static DateTime GetTomorrowStartTime()
    {
        return GetTodayStartTime().AddDays(1);
    }

    /// <summary>
    /// 获取明天开始时间戳
    /// </summary>
    /// <returns>明天零点时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回明天零点时间的Unix时间戳
    /// 例如:当前是2024-01-10,返回2024-01-11 00:00:00的时间戳
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算时间
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetTomorrowStartTimestamp()
    {
        var date = GetTomorrowStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取明天开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>明天零点时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetTomorrowStartTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetTomorrowStartTime());
    }

    /// <summary>
    /// 获取明天结束时间
    /// </summary>
    /// <returns>明天23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回明天的最后一秒
    /// 例如:当前是2024-01-10,返回2024-01-11 23:59:59
    /// 使用本地时区计算时间
    /// </remarks>
    public static DateTime GetTomorrowEndTime()
    {
        return DateTime.Today.AddDays(2).AddSeconds(-1);
    }

    /// <summary>
    /// 获取明天结束时间戳
    /// </summary>
    /// <returns>明天23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回明天最后一秒的Unix时间戳
    /// 例如:当前是2024-01-10,返回2024-01-11 23:59:59的时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetTomorrowEndTimestamp()
    {
        return DateTimeToUnixTimeSeconds(GetTomorrowEndTime());
    }

    /// <summary>
    /// 获取明天结束时间戳（基于设置时区）
    /// </summary>
    /// <returns>明天23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetTomorrowEndTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetTomorrowEndTime());
    }

    /// <summary>
    /// 按照当前时区 (<see cref="CurrentTimeZone"/>) 时间判断两个时间戳是否是同一天
    /// </summary>
    /// <param name="timestamp1">时间戳1（Unix秒级时间戳）。例如：1704857400</param>
    /// <param name="timestamp2">时间戳2（Unix秒级时间戳）。例如：1704859200</param>
    /// <returns>如果两个时间戳转换为当前时区 (<see cref="CurrentTimeZone"/>) 时间后是同一天，则返回true；否则返回false</returns>
    /// <remarks>
    /// 此方法会先将UTC时间戳转换为当前时区 (<see cref="CurrentTimeZone"/>) 时间，然后比较是否为同一天
    /// 比较时只考虑年月日，不考虑具体时间
    /// 使用当前时区 (<see cref="CurrentTimeZone"/>) 进行UTC到当前时区时间的转换
    /// </remarks>
    public static bool IsSameDayWithTimeZone(long timestamp1, long timestamp2)
    {
        var time1 = UtcSecondsToTimeZoneDateTime(timestamp1);
        var time2 = UtcSecondsToTimeZoneDateTime(timestamp2);
        return IsSameDay(time1, time2);
    }

    /// <summary>
    /// 获取从指定日期到当前时区 (<see cref="CurrentTimeZone"/>) 日期之间跨越的天数。
    /// </summary>
    /// <param name="startTime">起始日期。</param>
    /// <param name="hour">小时。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDaysWithTimeZone(DateTime startTime, int hour = 0)
    {
        return GetCrossDays(startTime, GetNowWithTimeZone(), hour);
    }

    /// <summary>
    /// 获取两个时间戳之间跨越的天数。
    /// </summary>
    /// <param name="beginTimestamp">起始时间戳,从1970年1月1日以来经过的秒数。</param>
    /// <param name="hour">小时。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDays(long beginTimestamp, int hour = 0)
    {
        var begin = TimestampToDateTime(beginTimestamp);
        return GetCrossDays(begin, hour);
    }

    /// <summary>
    /// 获取两个当前时区 (<see cref="CurrentTimeZone"/>) 时间戳之间的间隔天数
    /// </summary>
    /// <param name="startTimestamp">开始时间戳(秒),UTC时间戳将被转换为当前时区 (<see cref="CurrentTimeZone"/>) 时间</param>
    /// <param name="endTimestamp">结束时间戳(秒),UTC时间戳将被转换为当前时区 (<see cref="CurrentTimeZone"/>) 时间</param>
    /// <returns>间隔天数,如果开始时间晚于结束时间,返回负数</returns>
    /// <remarks>
    /// 此方法会先将UTC时间戳转换为当前时区 (<see cref="CurrentTimeZone"/>) 时间,然后计算两个时间之间的天数差
    /// 计算时会考虑日期的时分秒部分
    /// </remarks>
    public static int GetCrossDaysWithTimeZone(long startTimestamp, long endTimestamp)
    {
        var startTime = UtcSecondsToTimeZoneDateTime(startTimestamp);
        var endTime = UtcSecondsToTimeZoneDateTime(endTimestamp);
        return GetCrossDays(startTime, endTime);
    }

    /// <summary>
    /// 获取当前时区 (<see cref="CurrentTimeZone"/>) 的日期，格式为yyyyMMdd的整数
    /// </summary>
    /// <returns>返回一个8位整数，表示当前时区 (<see cref="CurrentTimeZone"/>) 的日期。例如：20231225表示2023年12月25日</returns>
    /// <remarks>
    /// 此方法将当前时区 (<see cref="CurrentTimeZone"/>) 时间转换为8位数字格式:
    /// - 前4位表示年份
    /// - 中间2位表示月份
    /// - 最后2位表示日期
    /// 使用 <see cref="GetNowWithTimeZone"/> 获取当前时区时间
    /// </remarks>
    public static int CurrentDateWithDay()
    {
        return Convert.ToInt32(GetNowWithTimeZone().ToString("yyyyMMdd"));
    }
}
