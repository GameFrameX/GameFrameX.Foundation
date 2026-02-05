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

using System;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Utility.Localization;

namespace GameFrameX.Foundation.Utility;

public partial class TimerHelper
{
    /// <summary>
    /// 获取今年开始时间 (UTC)
    /// </summary>
    /// <returns>今年1月1号00:00:00的时间 (UTC)</returns>
    /// <remarks>
    /// 此方法返回今年第一天的零点时间
    /// 使用 UTC 时区计算
    /// </remarks>
    public static DateTime GetUtcYearStartTime()
    {
        var now = GetUtcNow();
        return new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }

    /// <summary>
    /// 获取今年开始时间戳 (UTC)
    /// </summary>
    /// <returns>今年1月1号00:00:00的时间戳(秒) (UTC)</returns>
    /// <remarks>
    /// 此方法返回今年第一天零点时间的Unix时间戳
    /// 基于 UTC 时间计算
    /// </remarks>
    public static long GetUtcYearStartTimestamp()
    {
        var date = GetUtcYearStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取今年结束时间 (UTC)
    /// </summary>
    /// <returns>今年12月31号23:59:59的时间 (UTC)</returns>
    /// <remarks>
    /// 此方法返回今年最后一天的最后一秒
    /// 使用 UTC 时区计算
    /// </remarks>
    public static DateTime GetUtcYearEndTime()
    {
        var now = GetUtcNow();
        return GetStartTimeOfYear(now).AddYears(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取今年结束时间戳 (UTC)
    /// </summary>
    /// <returns>今年12月31号23:59:59的时间戳(秒) (UTC)</returns>
    /// <remarks>
    /// 此方法返回今年最后一天最后一秒的Unix时间戳
    /// 基于 UTC 时间计算
    /// </remarks>
    public static long GetUtcYearEndTimestamp()
    {
        var date = GetUtcYearEndTime();
        return DateTimeToUnixTimeSeconds(date);
    }

    /// <summary>
    /// 获取明年开始时间 (UTC)
    /// </summary>
    /// <returns>明年1月1号00:00:00的时间 (UTC)</returns>
    /// <remarks>
    /// 此方法返回明年第一天的零点时间
    /// 使用 UTC 时区计算
    /// </remarks>
    public static DateTime GetUtcNextYearStartTime()
    {
        return GetUtcYearStartTime().AddYears(1);
    }

    /// <summary>
    /// 获取明年开始时间戳 (UTC)
    /// </summary>
    /// <returns>明年1月1号00:00:00的时间戳(秒) (UTC)</returns>
    /// <remarks>
    /// 此方法返回明年第一天零点时间的Unix时间戳
    /// 基于 UTC 时间计算
    /// </remarks>
    public static long GetUtcNextYearStartTimestamp()
    {
        var date = GetUtcNextYearStartTime();
        return DateTimeToUnixTimeSeconds(date);
    }
}
