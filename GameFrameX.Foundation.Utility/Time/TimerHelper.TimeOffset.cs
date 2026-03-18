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
/// 时区偏移秒数。用于调整时间计算的偏移量。
/// </summary>
/// <remarks>
/// Time zone offset in seconds. Used to adjust the offset for time calculations.
/// Positive values indicate offset towards the future, negative values indicate offset towards the past.
/// </remarks>
/// <value>时区偏移秒数 / Time zone offset in seconds</value>
public static long TimeOffsetSeconds { get; private set; } = 0;

/// <summary>
/// 时区偏移毫秒数。用于调整时间计算的偏移量。
/// </summary>
/// <remarks>
/// Time zone offset in milliseconds. Used to adjust the offset for time calculations.
/// Positive values indicate offset towards the future, negative values indicate offset towards the past.
/// </remarks>
/// <value>时区偏移毫秒数 / Time zone offset in milliseconds</value>
public static long TimeOffsetMilliseconds { get; private set; } = 0;

/// <summary>
/// 设置时区偏移值。
/// </summary>
/// <remarks>
/// Sets the time zone offset values.
/// This method is used to adjust the baseline for time calculations.
/// For example, to simulate future time, pass a positive number; to simulate past time, pass a negative number.
/// Typically used for debugging and testing scenarios.
/// </remarks>
/// <param name="offsetSeconds">秒级偏移量 / Second-level offset</param>
/// <param name="offsetMilliseconds">毫秒级偏移量 / Millisecond-level offset</param>
public static void SetTimeOffset(long offsetSeconds, long offsetMilliseconds)
{
    TimeOffsetSeconds = offsetSeconds;
    TimeOffsetMilliseconds = offsetMilliseconds;
}

/// <summary>
/// 重置时区偏移值为默认值(0)。
/// </summary>
/// <remarks>
/// Resets the time zone offset values to default (0).
/// This method resets both second-level and millisecond-level offsets to 0,
/// restoring time calculations to the unadjusted state.
/// </remarks>
public static void ResetTimeOffset()
{
    TimeOffsetSeconds = 0;
    TimeOffsetMilliseconds = 0;
}
}