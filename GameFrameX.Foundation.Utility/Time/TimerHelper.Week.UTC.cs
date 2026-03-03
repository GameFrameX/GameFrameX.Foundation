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

public static partial class TimerHelper
{
    // /// <summary>
    // /// 判断指定Unix时间戳(秒)是否与当前UTC时间是同一周
    // /// </summary>
    // /// <param name="timestampSeconds">Unix时间戳(秒)</param>
    // /// <returns>如果是同一周返回true,否则返回false</returns>
    // /// <remarks>
    // /// 此方法将Unix秒级时间戳转换为UTC DateTime后进行比较
    // /// 适用于跨时区判定是否同周的场景
    // /// </remarks>
    // public static bool IsUnixSameWeekFromTimestamp(long timestampSeconds)
    // {
    //     var dateTime = UtcSecondsToUtcDateTime(timestampSeconds);
    //     return IsNowSameWeekUtc(dateTime);
    // }
    //
    // /// <summary>
    // /// 判断指定Unix时间戳(毫秒)是否与当前UTC时间是同一周
    // /// </summary>
    // /// <param name="timestampMilliseconds">Unix时间戳(毫秒)</param>
    // /// <returns>如果是同一周返回true,否则返回false</returns>
    // /// <remarks>
    // /// 此方法将Unix毫秒级时间戳转换为UTC DateTime后进行比较
    // /// 适用于跨时区判定是否同周的场景
    // /// </remarks>
    // public static bool IsUnixSameWeekFromTimestampMilliseconds(long timestampMilliseconds)
    // {
    //     var dateTime = UtcMillisecondsToUtcDateTime(timestampMilliseconds);
    //     return IsNowSameWeekUtc(dateTime);
    // }

    /// <summary>
    /// 获取本周指定星期几的UTC时间
    /// </summary>
    /// <param name="day">星期几 (DayOfWeek.Sunday 到 DayOfWeek.Saturday)</param>
    /// <returns>本周指定星期几的UTC日期时间</returns>
    /// <remarks>
    /// 此方法基于当前UTC时间计算
    /// 返回的时间部分与当前UTC时间保持一致，仅日期变更为本周对应的星期几
    /// 注意：这里的"本周"是基于UTC时间定义的
    /// </remarks>
    public static DateTime GetDayOfWeekTime(DayOfWeek day)
    {
        return GetDayOfWeekTime(GetNowWithUtc(), day);
    }

    /// <summary>
    /// 获取下周开始时间
    /// </summary>
    /// <returns>下周周一00:00:00的时间</returns>
    /// <remarks>
    /// 此方法返回下周第一天(周一)的零点时间
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetNextWeekStartTimeWithUtc()
    {
        var now = GetNowWithUtc();
        var dayOfWeek = (int)now.DayOfWeek;
        dayOfWeek = dayOfWeek == 0 ? 7 : dayOfWeek;
        return now.AddDays(1 - dayOfWeek + 7).Date;
    }

    /// <summary>
    /// 获取下周开始时间戳（基于设置时区）
    /// </summary>
    /// <returns>下周周一00:00:00的时间戳(秒) + 时区偏移</returns>
    /// <remarks>
    /// 返回值 = 标准Unix时间戳 + 时区偏移秒数
    /// </remarks>
    public static long GetNextWeekStartTimestampWithUtc()
    {
        return DateTimeToUnixTimeSeconds(GetNextWeekStartTimeWithUtc());
    }

    /// <summary>
    /// 获取下周结束时间
    /// </summary>
    /// <returns>下周周日23:59:59的时间</returns>
    /// <remarks>
    /// 此方法返回下周最后一天(周日)的最后一秒
    /// 使用 <see cref="CurrentTimeZone"/> 时区计算
    /// </remarks>
    public static DateTime GetNextWeekEndTimeWithUtc()
    {
        return GetNextWeekStartTimeWithUtc().AddDays(7).AddSeconds(-1);
    }

    /// <summary>
    /// 获取下周结束时间戳
    /// </summary>
    /// <returns>下周周日23:59:59的时间戳(秒)</returns>
    /// <remarks>
    /// 此方法返回下周周日最后一秒的Unix时间戳
    /// 会将时间转换为UTC时间后再计算时间戳
    /// </remarks>
    public static long GetNextWeekEndTimestampWithUtc()
    {
        var date = GetNextWeekEndTimeWithUtc();
        return DateTimeToUnixTimeSeconds(date);
    }
}