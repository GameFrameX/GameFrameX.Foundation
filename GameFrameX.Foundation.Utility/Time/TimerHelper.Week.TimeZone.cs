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
    /// 判断指定时间戳是否与当前时区 (<see cref="CurrentTimeZone"/>) 时间是同一周
    /// </summary>
    /// <param name="ticks">时间刻度(Ticks)</param>
    /// <returns>如果是同一周返回true,否则返回false</returns>
    /// <remarks>
    /// 此方法使用当前时区 (<see cref="CurrentTimeZone"/>) 时间进行比较
    /// 输入的ticks会被转换为DateTime后与当前时区时间比较
    /// </remarks>
    public static bool IsNowSameWeek(long ticks)
    {
        return IsNowSameWeek(new DateTime(ticks));
    }

    /// <summary>
    /// 判断指定日期是否与当前时区 (<see cref="CurrentTimeZone"/>) 时间是同一周
    /// </summary>
    /// <param name="start">要比较的日期</param>
    /// <returns>如果是同一周返回true,否则返回false</returns>
    /// <remarks>
    /// 此方法使用当前时区 (<see cref="CurrentTimeZone"/>) 时间进行比较
    /// 使用 <see cref="GetNowWithTimeZone"/> 获取当前时区时间
    /// </remarks>
    public static bool IsNowSameWeek(DateTime start)
    {
        return IsSameWeek(start, GetNowWithTimeZone());
    }

    /// <summary>
    /// 获取当前时区 (<see cref="CurrentTimeZone"/>) 日期的中文星期表示
    /// </summary>
    /// <returns>返回中文星期字符串，如"星期一"、"星期日"</returns>
    /// <remarks>
    /// 使用 <see cref="GetNowWithTimeZone"/> 获取当前时区时间
    /// 将DayOfWeek枚举转换为对应的中文表示
    /// </remarks>
    public static string GetChinaDayOfWeek()
    {
        return GetChinaDayOfWeek(GetNowWithTimeZone());
    }

    /// <summary>
    /// 获取指定日期的中文星期表示
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>返回中文星期字符串，如"星期一"、"星期日"</returns>
    /// <remarks>
    /// 根据传入日期的DayOfWeek属性返回对应的中文名称
    /// 支持从星期一到星期日的所有映射
    /// </remarks>
    public static string GetChinaDayOfWeek(DateTime date)
    {
        string[] dayOfWeek = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        return dayOfWeek[Convert.ToInt32(date.DayOfWeek.ToString("d"))];
    }

    /// <summary>
    /// 获取本周开始时间
    /// </summary>
    /// <returns>本周周一00:00:00的时间</returns>
    /// <remarks>
    /// 此方法返回本周第一天(周一)的零点时间
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// 如果今天是周日(DayOfWeek=0),会将其视为本周第7天处理
    /// </remarks>
    public static DateTime GetWeekStartTime()
    {
        var now = GetNowWithTimeZone();
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        return now.AddDays(1 - dayOfWeek).Date;
    }

    /// <summary>
    /// 获取本周开始时间戳
    /// </summary>
    /// <returns>本周周一00:00:00的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回本周周一零点时间的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetWeekStartTimestamp()
    {
        var date = GetWeekStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取本周开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>本周周一00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetWeekStartTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetWeekStartTime());
    }

    /// <summary>
    /// 获取本周结束时间
    /// </summary>
    /// <returns>本周周日23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回本周最后一天(周日)的最后一秒
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetWeekEndTime()
    {
        var now = GetNowWithTimeZone();
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        return now.AddDays(7 - dayOfWeek).Date.AddDays(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取本周结束时间戳
    /// </summary>
    /// <returns>本周周日23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回本周周日最后一秒的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetWeekEndTimestamp()
    {
        var date = GetWeekEndTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取本周结束时间戳（基于设置时区）
    /// </summary>
    /// <returns>本周周日23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetWeekEndTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetWeekEndTime());
    }

    /// <summary>
    /// 获取指定日期所在周的开始时间
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在周周一00:00:00的时间</returns>
    /// <remarks>
    /// 此方法返回指定日期所在周的周一零点时间
    /// 保持原有时区不变
    /// </remarks>
    public static DateTime GetStartTimeOfWeek(DateTime date)
    {
        var dayOfWeek = (int)date.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        return date.AddDays(1 - dayOfWeek).Date;
    }

    /// <summary>
    /// 获取指定日期所在周的开始时间戳
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在周周一00:00:00的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回指定日期所在周的周一零点时间的Unix时间戳
    /// 会使用当前时区 (<see cref="CurrentTimeZone"/>) 计算偏移量并将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetStartTimestampOfWeek(DateTime date)
    {
        var time = GetStartTimeOfWeek(date);
        return DateTimeToUnixTimeSeconds(time);
    }

    /// <summary>
    /// 获取指定日期所在周的开始时间戳（基于设置时区）
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在周周一00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetStartTimestampOfWeekWithTimeZone(DateTime date)
    {
        return TimeToSecondsWithTimeZone(GetStartTimeOfWeek(date));
    }

    /// <summary>
    /// 获取指定日期所在周的结束时间
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在周周日23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回指定日期所在周的周日最后一秒
    /// 保持原有时区不变
    /// </remarks>
    public static DateTime GetEndTimeOfWeek(DateTime date)
    {
        var dayOfWeek = (int)date.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        return date.AddDays(7 - dayOfWeek).Date.AddDays(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取指定日期所在周的结束时间戳
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在周周日23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回指定日期所在周的周日最后一秒的Unix时间戳
    /// 例如:输入2024-01-10(周三),返回2024-01-14 23:59:59(周日)的时间戳
    /// 会使用当前时区 (<see cref="CurrentTimeZone"/>) 计算偏移量并将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetEndTimestampOfWeek(DateTime date)
    {
        var time = GetEndTimeOfWeek(date);
        return DateTimeToUnixTimeSeconds(time);
    }

    /// <summary>
    /// 获取指定日期所在周的结束时间戳（基于设置时区）
    /// </summary>
    /// <param name="date">指定日期</param>
    /// <returns>所在周周日23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetEndTimestampOfWeekWithTimeZone(DateTime date)
    {
        return TimeToSecondsWithTimeZone(GetEndTimeOfWeek(date));
    }

    /// <summary>
    /// 获取下周开始时间
    /// </summary>
    /// <returns>下周周一00:00:00的时间</returns>
    /// <remarks>
    /// 此方法返回下周第一天(周一)的零点时间
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetNextWeekStartTime()
    {
        var now = GetNowWithTimeZone();
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        return now.AddDays(1 - dayOfWeek + 7).Date;
    }

    /// <summary>
    /// 获取下周开始时间戳
    /// </summary>
    /// <returns>下周周一00:00:00的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回下周周一零点时间的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetNextWeekStartTimestamp()
    {
        var date = GetNextWeekStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取下周开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>下周周一00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetNextWeekStartTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetNextWeekStartTime());
    }

    /// <summary>
    /// 获取下周结束时间
    /// </summary>
    /// <returns>下周周日23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回下周最后一天(周日)的最后一秒
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetNextWeekEndTime()
    {
        return GetNextWeekStartTime().AddDays(7).AddSeconds(-1);
    }

    /// <summary>
    /// 获取下周结束时间戳
    /// </summary>
    /// <returns>下周周日23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回下周周日最后一秒的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetNextWeekEndTimestamp()
    {
        var date = GetNextWeekEndTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取下周结束时间戳（基于设置时区）
    /// </summary>
    /// <returns>下周周日23:59:59的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetNextWeekEndTimestampWithTimeZone()
    {
        return TimeToSecondsWithTimeZone(GetNextWeekEndTime());
    }
}
