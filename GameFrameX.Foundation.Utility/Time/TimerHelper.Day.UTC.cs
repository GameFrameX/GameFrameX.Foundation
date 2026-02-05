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
    /// 按照UTC时间判断两个时间戳是否是同一天
    /// </summary>
    /// <param name="unixTimestampA">时间戳1</param>
    /// <param name="unixTimestampB">时间戳2</param>
    /// <returns>是否是同一天</returns>
    /// <remarks>
    /// 此方法将两个Unix时间戳转换为UTC时间后比较是否为同一天
    /// 比较时只考虑日期部分(年月日),忽略时间部分
    /// 使用UTC时间避免时区转换带来的问题
    /// </remarks>
    public static bool IsUnixSameDay(long unixTimestampA, long unixTimestampB)
    {
        var time1 = UtcSecondsToUtcDateTime(unixTimestampA);
        var time2 = UtcSecondsToUtcDateTime(unixTimestampB);
        return IsSameDay(time1, time2);
    }

    /// <summary>
    /// 获取当前UTC时区的日期，格式为yyyyMMdd的整数
    /// </summary>
    /// <returns>返回一个8位整数，表示当前UTC时区的日期。例如：20231225表示2023年12月25日</returns>
    /// <remarks>
    /// 此方法将当前UTC时间转换为8位数字格式:
    /// - 前4位表示年份
    /// - 中间2位表示月份
    /// - 最后2位表示日期
    /// 使用DateTime.UtcNow获取UTC时间
    /// </remarks>
    public static int CurrentDateWithUtcDay()
    {
        return Convert.ToInt32(GetUtcNow().ToString("yyyyMMdd"));
    }

    /// <summary>
    /// 获取从指定日期到当前UTC日期之间跨越的天数。
    /// </summary>
    /// <param name="startTime">起始日期。</param>
    /// <param name="hour">小时。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDaysUtc(DateTime startTime, int hour = 0)
    {
        return GetCrossDays(startTime, GetUtcNow(), hour);
    }

    /// <summary>
    /// 获取两个UTC时间戳之间跨越的天数。
    /// </summary>
    /// <param name="beginUnixTimestamp">开始时间戳(秒)，从1970年1月1日以来经过的秒数。</param>
    /// <param name="afterUnixTimestamp">结束时间戳(秒)，从1970年1月1日以来经过的秒数。</param>
    /// <param name="hour">小时。</param>
    /// <returns>跨越的天数。</returns>
    public static int GetCrossDaysUtc(long beginUnixTimestamp, long afterUnixTimestamp, int hour = 0)
    {
        var begin = UtcSecondsToUtcDateTime(beginUnixTimestamp);
        var after = UtcSecondsToUtcDateTime(afterUnixTimestamp);
        return GetCrossDays(begin, after, hour);
    }
}