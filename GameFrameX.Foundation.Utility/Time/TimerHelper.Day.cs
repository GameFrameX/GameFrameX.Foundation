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
    /// 判断两个 <see cref="DateTime"/> 对象是否表示同一天。
    /// </summary>
    /// <param name="time1">要比较的第一个时间。例如：2024-01-10 14:30:00</param>
    /// <param name="time2">要比较的第二个时间。例如：2024-01-10 18:45:00</param>
    /// <returns>
    /// 如果两个时间是同一天，则返回 <c>true</c>；否则返回 <c>false</c>。
    /// </returns>
    /// <remarks>
    /// 此方法执行以下比较逻辑：
    /// 1. 使用 <see cref="DateTime.Date"/> 属性获取日期部分（忽略时间部分）
    /// 2. 分别比较年、月、日三个组成部分
    /// 3. 只有当年、月、日都相同时才返回 <c>true</c>
    /// 
    /// 重要特性：
    /// - 忽略具体的时、分、秒、毫秒等时间部分
    /// - 忽略时区差异，直接使用 <see cref="DateTime"/> 中存储的日期值
    /// - 不进行时区转换，基于原始 <see cref="DateTime"/> 值进行比较
    /// 
    /// 性能优化：
    /// - 使用直接的整数比较（Year、Month、Day）
    /// - 避免创建新的 <see cref="DateTime"/> 对象
    /// - 比使用 <c>time1.Date == time2.Date</c> 更高效
    /// 
    /// 适用场景：
    /// - 日程安排和事件管理
    /// - 日志按日期分组
    /// - 统计同一天的数据
    /// - 用户界面中的日期筛选
    /// </remarks>
    /// <example>
    /// <code>
    /// // 同一天的不同时间
    /// DateTime morning = new DateTime(2024, 1, 10, 8, 30, 0);
    /// DateTime evening = new DateTime(2024, 1, 10, 20, 45, 30);
    /// bool sameDay1 = TimerHelper.IsSameDay(morning, evening);
    /// Console.WriteLine($"早晨和晚上是同一天: {sameDay1}"); // True
    /// 
    /// // 不同天的时间
    /// DateTime today = new DateTime(2024, 1, 10, 23, 59, 59);
    /// DateTime tomorrow = new DateTime(2024, 1, 11, 0, 0, 1);
    /// bool sameDay2 = TimerHelper.IsSameDay(today, tomorrow);
    /// Console.WriteLine($"今天和明天是同一天: {sameDay2}"); // False
    /// 
    /// // 跨年比较
    /// DateTime lastYear = new DateTime(2023, 12, 31, 12, 0, 0);
    /// DateTime thisYear = new DateTime(2024, 1, 1, 12, 0, 0);
    /// bool sameDay3 = TimerHelper.IsSameDay(lastYear, thisYear);
    /// Console.WriteLine($"跨年日期是同一天: {sameDay3}"); // False
    /// 
    /// // 实际应用：按日期分组日志
    /// List&lt;DateTime&gt; logTimes = new List&lt;DateTime&gt;
    /// {
    ///     new DateTime(2024, 1, 10, 9, 0, 0),
    ///     new DateTime(2024, 1, 10, 15, 30, 0),
    ///     new DateTime(2024, 1, 11, 10, 0, 0)
    /// };
    /// 
    /// DateTime targetDate = new DateTime(2024, 1, 10);
    /// var sameDayLogs = logTimes.Where(log =&gt; TimerHelper.IsSameDay(log, targetDate)).ToList();
    /// Console.WriteLine($"2024-01-10 的日志数量: {sameDayLogs.Count}"); // 2
    /// </code>
    /// </example>
    /// <seealso cref="DateTime.Date"/>
    /// <seealso cref="DateTime.Year"/>
    /// <seealso cref="DateTime.Month"/>
    /// <seealso cref="DateTime.Day"/>
    public static bool IsSameDay(DateTime time1, DateTime time2)
    {
        return time1.Date.Year == time2.Date.Year && time1.Date.Month == time2.Date.Month && time1.Date.Day == time2.Date.Day;
    }

    /// <summary>
    /// 获取两个时间之间的天数差
    /// </summary>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>天数差（可能为负数）</returns>
    /// <remarks>
    /// 此方法返回两个时间之间的总天数，保留小数部分
    /// 如果endTime早于startTime，返回负数
    /// 返回double类型以保持精度
    /// 适用于需要天级时间差的场景
    /// </remarks>
    public static double GetDaysDifference(DateTime startTime, DateTime endTime)
    {
        return (endTime - startTime).TotalDays;
    }

    /// <summary>
    /// 获取两个日期之间跨越的天数。
    /// </summary>
    /// <param name="startTime">起始日期。</param>
    /// <param name="endTime">结束日期。</param>
    /// <param name="hour">小时。</param>
    /// <returns>跨越的天数。</returns>
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
