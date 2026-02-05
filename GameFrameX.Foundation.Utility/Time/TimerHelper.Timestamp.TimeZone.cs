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

using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Utility.Localization;

namespace GameFrameX.Foundation.Utility;

public partial class TimerHelper
{
    /// <summary>
    /// 毫秒转时间
    /// </summary>
    /// <param name="utcTimestampMilliseconds">毫秒时间戳。</param>
    /// <param name="utc">是否使用UTC时间。</param>
    /// <returns>转换后的时间。如果utc为false，则返回当前时区 (<see cref="CurrentTimeZone"/>) 的时间。</returns>
    public static DateTime TimeStampMillisecondToDateTime(long utcTimestampMilliseconds, bool utc = false)
    {
        var dateTime = EpochUtc.AddMilliseconds(utcTimestampMilliseconds);
        if (utc)
        {
            return dateTime;
        }

        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, CurrentTimeZone);
    }

    /// <summary>
    /// 秒时间戳转时间
    /// </summary>
    /// <param name="utcTimestampSeconds">秒时间戳。</param>
    /// <param name="utc">是否使用UTC时间。</param>
    /// <returns>转换后的时间。如果utc为false，则返回当前时区 (<see cref="CurrentTimeZone"/>) 的时间。</returns>
    public static DateTime TimestampSecondToDateTime(long utcTimestampSeconds, bool utc = false)
    {
        var dateTime = EpochUtc.AddSeconds(utcTimestampSeconds);
        if (utc)
        {
            return dateTime;
        }

        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, CurrentTimeZone);
    }

    /// <summary>
    /// 将给定的时间戳转换为相对于EpochLocal的 TimeSpan 对象。
    /// </summary>
    /// <param name="timestamp">自1970年1月1日午夜以来经过的秒数。</param>
    /// <returns>一个 TimeSpan 对象，表示从EpochLocal到给定时间戳的间隔。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当时间戳超出有效范围时抛出此异常</exception>
    public static TimeSpan TimeSpanWithTimeZoneTimestamp(long timestamp)
    {
        if (timestamp < -62135596800L || timestamp > 253402300799L)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), LocalizationService.GetString(LocalizationKeys.Exceptions.TimestampOutOfRange));
        }

        // 直接将秒数转换为TimeSpan
        return TimeSpan.FromSeconds(timestamp);
    }
}
