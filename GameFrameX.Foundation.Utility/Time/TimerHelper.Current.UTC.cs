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
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

namespace GameFrameX.Foundation.Utility;

public static partial class TimerHelper
{
    /// <summary>
    /// 获取当前UTC时间，格式为HHmmss的字符串。
    /// </summary>
    /// <remarks>
    /// Gets the current UTC time as a string in HHmmss format.
    /// This method converts the current UTC time to a 6-character time string:
    /// - First 2 digits represent hours (24-hour format)
    /// - Middle 2 digits represent minutes
    /// - Last 2 digits represent seconds
    /// Uses DateTime.UtcNow to get UTC time.
    /// </remarks>
    /// <returns>返回一个6位字符串，表示当前UTC时间。例如：143045表示14:30:45 / Returns a 6-character string representing the current UTC time. For example: 143045 represents 14:30:45</returns>
    public static string CurrentTimeWithUtcFullString()
    {
        return GetNowWithUtc().ToString("HHmmss");
    }

    /// <summary>
    /// 获取当前UTC时间，格式为HHmmss的整数。
    /// </summary>
    /// <remarks>
    /// Gets the current UTC time as an integer in HHmmss format.
    /// This method converts the current UTC time to a 6-digit integer:
    /// - First 2 digits represent hours (24-hour format)
    /// - Middle 2 digits represent minutes
    /// - Last 2 digits represent seconds
    /// Internally calls CurrentTimeWithUtcFullString() to get the string and then converts to integer.
    /// </remarks>
    /// <returns>返回一个6位整数，表示当前UTC时间。例如：143045表示14:30:45 / Returns a 6-digit integer representing the current UTC time. For example: 143045 represents 14:30:45</returns>
    public static int CurrentTimeWithUtc()
    {
        return Convert.ToInt32(CurrentTimeWithUtcFullString());
    }

    /// <summary>
    /// 获取当前UTC时区时间的自定义格式字符串。
    /// </summary>
    /// <remarks>
    /// Gets the current UTC time as a custom formatted string.
    /// This method allows custom time format strings:
    /// - Default format includes year, month, day, hour, minute, second, millisecond, and time zone information
    /// - Other formats can be specified through the format parameter
    /// - Uses DateTime.UtcNow to get UTC time
    /// Supports standard .NET date and time format specifiers.
    /// </remarks>
    /// <param name="format">时间格式字符串，默认为"yyyy-MM-dd HH:mm:ss.fff K" / Time format string, defaults to "yyyy-MM-dd HH:mm:ss.fff K"</param>
    /// <returns>返回指定格式的UTC时间字符串。例如默认格式返回："2023-12-25 06:30:45.123 +00:00" / Returns the UTC time string in the specified format. For example, the default format returns: "2023-12-25 06:30:45.123 +00:00"</returns>
    public static string CurrentDateTimeWithUtcFormat(string format = "yyyy-MM-dd HH:mm:ss.fff K")
    {
        return GetNowWithUtc().ToString(format);
    }

    /// <summary>
    /// 获取当前UTC时间。
    /// </summary>
    /// <remarks>
    /// Gets the current UTC time.
    /// This method returns the current UTC time (Coordinated Universal Time).
    /// Compared to local time, there will be a time zone offset.
    /// Mainly used for scenarios that require a unified time standard.
    /// </remarks>
    /// <returns>当前UTC时间 / The current UTC time</returns>
    public static DateTime GetNowWithUtc()
    {
        return DateTime.UtcNow;
    }
}
