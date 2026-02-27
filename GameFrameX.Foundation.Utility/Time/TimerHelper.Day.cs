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
    /// 判断两个 <see cref="DateTime"/> 是否表示同一日历日期。
    /// </summary>
    /// <param name="timeA">要比较的第一个时间。</param>
    /// <param name="timeB">要比较的第二个时间。</param>
    /// <returns>如果两个时间在同一天内，则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
    /// <remarks>
    /// 此方法仅比较年、月、日三个部分，忽略时、分、秒、毫秒等时间成分。
    /// 该比较不会进行时区转换，直接使用传入 <see cref="DateTime"/> 的日期值。
    /// </remarks>
    /// <example>
    /// <code>
    /// DateTime morning = new DateTime(2024, 1, 10, 8, 30, 0);
    /// DateTime evening = new DateTime(2024, 1, 10, 20, 45, 30);
    /// bool sameDay1 = TimerHelper.IsSameDay(morning, evening);
    /// Console.WriteLine(sameDay1); // True
    ///
    /// DateTime today = new DateTime(2024, 1, 10, 23, 59, 59);
    /// DateTime tomorrow = new DateTime(2024, 1, 11, 0, 0, 1);
    /// bool sameDay2 = TimerHelper.IsSameDay(today, tomorrow);
    /// Console.WriteLine(sameDay2); // False
    /// </code>
    /// </example>
    /// <seealso cref="DateTime.Date"/>
    /// <seealso cref="DateTime.Year"/>
    /// <seealso cref="DateTime.Month"/>
    /// <seealso cref="DateTime.Day"/>
    public static bool IsSameDay(DateTime timeA, DateTime timeB)
    {
        return timeA.Date.Year == timeB.Date.Year && timeA.Date.Month == timeB.Date.Month && timeA.Date.Day == timeB.Date.Day;
    }

    /// <summary>
    /// 获取两个时间之间的天数差。
    /// </summary>
    /// <param name="startTime">开始时间。</param>
    /// <param name="endTime">结束时间。</param>
    /// <returns>天数差，使用 <see cref="double"/> 表示，可能为负数。</returns>
    /// <remarks>
    /// 返回值来源于 <see cref="TimeSpan.TotalDays"/>，包含小数部分。
    /// 如果 <paramref name="endTime"/> 早于 <paramref name="startTime"/>，结果为负数。
    /// </remarks>
    /// <example>
    /// <code>
    /// DateTime start = new DateTime(2024, 1, 10, 8, 0, 0);
    /// DateTime end = new DateTime(2024, 1, 11, 20, 0, 0);
    /// double days = TimerHelper.GetDaysDifference(start, end);
    /// Console.WriteLine(days); // 1.5
    /// </code>
    /// </example>
    /// <seealso cref="TimeSpan.TotalDays"/>
    public static double GetDaysDifference(DateTime startTime, DateTime endTime)
    {
        return (endTime - startTime).TotalDays;
    }

    /// <summary>
    /// 获取两个日期之间跨越的天数。
    /// </summary>
    /// <param name="startTime">起始时间。</param>
    /// <param name="endTime">结束时间。</param>
    /// <param name="hour">用于判定跨日的小时阈值。</param>
    /// <returns>跨越的天数。</returns>
    /// <remarks>
    /// 此方法先计算两个日期（忽略具体时间部分）之间的天数差，再依据小时阈值调整结果。
    /// 如果 <paramref name="startTime"/> 的小时数小于阈值，则结果加一；
    /// 如果 <paramref name="endTime"/> 的小时数小于阈值，则结果减一。
    /// </remarks>
    /// <example>
    /// <code>
    /// DateTime start = new DateTime(2024, 1, 10, 3, 0, 0);
    /// DateTime end = new DateTime(2024, 1, 11, 2, 0, 0);
    /// int days = TimerHelper.GetCrossDays(start, end, 5);
    /// Console.WriteLine(days); // 0
    /// </code>
    /// </example>
    public static int GetCrossDays(DateTime startTime, DateTime endTime, int hour = 0)
    {
        var days = (int)(endTime.Date - startTime.Date).TotalDays;
        if (startTime.Hour < hour)
        {
            days++;
        }

        if (endTime.Hour < hour)
        {
            days--;
        }

        return days;
    }
}
