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
    /// <summary>
    /// 获取当前UTC时区的日期，格式为yyyyMMdd的整数。
    /// </summary>
    /// <returns>返回一个8位整数，表示当前UTC时区的日期。例如：20231225表示2023年12月25日。</returns>
    /// <remarks>
    /// 此方法将当前UTC时间转换为8位数字格式:
    /// - 前4位表示年份
    /// - 中间2位表示月份
    /// - 最后2位表示日期
    /// 使用DateTime.UtcNow获取UTC时间
    /// </remarks>
    public static int CurrentDateWithUtcDay()
    {
        return Convert.ToInt32(GetNowWithUtc().ToString("yyyyMMdd"));
    }

    /// <summary>
    /// 获取两个UTC时间戳之间跨越的天数（基于UTC时间）。
    /// </summary>
    /// <param name="beginUnixTimestamp">开始时间戳(秒)，从1970年1月1日以来经过的秒数。</param>
    /// <param name="afterUnixTimestamp">结束时间戳(秒)，从1970年1月1日以来经过的秒数。</param>
    /// <param name="hour">小时阈值。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDaysUtc(long beginUnixTimestamp, long afterUnixTimestamp, int hour = 0)
    {
        var begin = TimestampSecondToDateTime(beginUnixTimestamp, true);
        var after = TimestampSecondToDateTime(afterUnixTimestamp, true);
        return GetCrossDays(begin, after, hour);
    }

    /// <summary>
    /// 获取从指定日期到当前UTC日期之间跨越的天数（基于UTC时间）。
    /// </summary>
    /// <param name="startTime">起始日期。</param>
    /// <param name="hour">小时阈值。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDaysWithUtc(DateTime startTime, int hour = 0)
    {
        return GetCrossDays(startTime, GetNowWithUtc(), hour);
    }

    /// <summary>
    /// 获取今天开始时间（基于UTC时间）。
    /// </summary>
    /// <returns>今天零点时间。</returns>
    /// <remarks>
    /// 此方法返回当天的零点时间(00:00:00)。
    /// 使用 <see cref="GetNowWithUtc"/> 获取当前日期的零点时间。
    /// 返回的是UTC时间。
    /// </remarks>
    public static DateTime GetTodayStartTimeWithUtc()
    {
        var dateTime = GetNowWithUtc();
        return dateTime.Date;
    }

    /// <summary>
    /// 获取今天开始时间戳（基于UTC时间）。
    /// </summary>
    /// <returns>今天零点时间戳(秒)。</returns>
    /// <remarks>
    /// 此方法返回当天零点时间的Unix时间戳。
    /// 先获取UTC的今天零点时间,然后转换为时间戳。
    /// 返回从1970-01-01 00:00:00 UTC开始的秒数。
    /// </remarks>
    public static long GetTodayStartTimestampWithUtc()
    {
        var date = GetTodayStartTimeWithUtc();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取今天结束时间（基于UTC时间）。
    /// </summary>
    /// <returns>今天23:59:59的时间。</returns>
    /// <remarks>
    /// 此方法返回当天的最后一秒(23:59:59)。
    /// 通过获取明天零点时间然后减去1秒来计算。
    /// 返回的是UTC时间。
    /// </remarks>
    public static DateTime GetTodayEndTimeWithUtc()
    {
        return GetTodayStartTimeWithUtc().AddDays(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取今天结束时间戳（基于UTC时间）。
    /// </summary>
    /// <returns>今天23:59:59的时间戳(秒)。</returns>
    /// <remarks>
    /// 此方法返回当天最后一秒的Unix时间戳。
    /// 先获取UTC的今天23:59:59,然后转换为时间戳。
    /// 返回从1970-01-01 00:00:00 UTC开始的秒数。
    /// </remarks>
    public static long GetTodayEndTimestampWithUtc()
    {
        var date = GetTodayEndTimeWithUtc();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取明天开始时间（基于UTC时间）。
    /// </summary>
    /// <returns>明天零点时间。</returns>
    /// <remarks>
    /// 此方法返回明天的零点时间。
    /// 例如:当前是2024-01-10,返回2024-01-11 00:00:00。
    /// 使用UTC时间计算。
    /// </remarks>
    public static DateTime GetTomorrowStartTimeWithUtc()
    {
        return GetTodayStartTimeWithUtc().AddDays(1);
    }

    /// <summary>
    /// 获取明天开始时间戳（基于UTC时间）。
    /// </summary>
    /// <returns>明天零点时间戳(秒)。</returns>
    /// <remarks>
    /// 此方法返回明天零点时间的Unix时间戳。
    /// 例如:当前是2024-01-10,返回2024-01-11 00:00:00的时间戳。
    /// 使用UTC时间计算并转换为时间戳。
    /// </remarks>
    public static long GetTomorrowStartTimestampWithUtc()
    {
        var date = GetTomorrowStartTimeWithUtc();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取明天结束时间（基于UTC时间）。
    /// </summary>
    /// <returns>明天23:59:59的时间。</returns>
    /// <remarks>
    /// 此方法返回明天的最后一秒。
    /// 例如:当前是2024-01-10,返回2024-01-11 23:59:59。
    /// 使用UTC时间计算。
    /// </remarks>
    public static DateTime GetTomorrowEndTimeWithUtc()
    {
        return GetNowWithUtc().Date.AddDays(2).AddSeconds(-1);
    }

    /// <summary>
    /// 获取明天结束时间戳（基于UTC时间）。
    /// </summary>
    /// <returns>明天23:59:59的时间戳(秒)。</returns>
    /// <remarks>
    /// 此方法返回明天最后一秒的Unix时间戳。
    /// 例如:当前是2024-01-10,返回2024-01-11 23:59:59的时间戳。
    /// 会将时间转换为UTC时间后再计算时间戳。
    /// </remarks>
    public static long GetTomorrowEndTimestampWithUtc()
    {
        return DateTimeToUnixTimeSeconds(GetTomorrowEndTimeWithUtc());
    }

    /// <summary>
    /// 获取两个时间戳之间跨越的天数（基于UTC时间）。
    /// </summary>
    /// <param name="beginTimestamp">起始时间戳,从1970年1月1日以来经过的秒数。</param>
    /// <param name="hour">小时阈值。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDaysWithUtc(long beginTimestamp, int hour = 0)
    {
        var begin = TimestampSecondToDateTime(beginTimestamp);
        return GetCrossDaysWithUtc(begin, hour);
    }
}
