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
    /// 判断两个日期是否在同一周
    /// </summary>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>如果是同一周返回true，否则返回false</returns>
    /// <remarks>
    /// 此方法通过计算两个日期分别距离周一的天数差来判断是否在同一周
    /// 假设周一为每周的第一天
    /// 
    /// 算法逻辑：
    /// 1. 计算startTime是周几(1-7)
    /// 2. 计算startTime所在周的周一日期
    /// 3. 计算endTime是周几(1-7)
    /// 4. 计算endTime所在周的周一日期
    /// 5. 比较两个周一日期是否相同
    /// </remarks>
    public static bool IsSameWeek(DateTime startTime, DateTime endTime)
    {
        var startDayOfWeek = (int)startTime.DayOfWeek;
        startDayOfWeek = startDayOfWeek == 0 ? 7 : startDayOfWeek;
        var startWeekMonday = startTime.AddDays(1 - startDayOfWeek).Date;

        var endDayOfWeek = (int)endTime.DayOfWeek;
        endDayOfWeek = endDayOfWeek == 0 ? 7 : endDayOfWeek;
        var endWeekMonday = endTime.AddDays(1 - endDayOfWeek).Date;

        return startWeekMonday == endWeekMonday;
    }

    /// <summary>
    /// 获取指定日期所在周的指定星期几的日期时间
    /// </summary>
    /// <param name="dateTime">指定日期</param>
    /// <param name="day">目标星期几 (DayOfWeek.Sunday 到 DayOfWeek.Saturday)</param>
    /// <returns>计算结果日期时间</returns>
    /// <remarks>
    /// 此方法计算输入日期所在周的对应星期几的日期
    /// 
    /// 计算逻辑：
    /// 1. 获取输入日期的星期数(DayOfWeek)
    /// 2. 计算目标星期与当前星期的差值
    /// 3. 在输入日期上加上差值得到结果
    /// 
    /// 例如：
    /// 如果输入是周三，求周一，则差值为 -2，返回日期减2天
    /// 如果输入是周三，求周五，则差值为 +2，返回日期加2天
    /// 
    /// 注意：
    /// - 这里的周是以周日为起始点(DayOfWeek定义的标准)
    /// - 不会改变时间部分，只改变日期部分
    /// </remarks>
    public static DateTime GetDayOfWeekTime(DateTime dateTime, DayOfWeek day)
    {
        return dateTime.AddDays(day - dateTime.DayOfWeek);
    }
}
