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

using Timer = System.Timers.Timer;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// Timer 扩展方法类。
/// </summary>
/// <remarks>
/// Provides extension methods for the <see cref="System.Timers.Timer"/> class.
/// </remarks>
public static class TimerExtension
{
    /// <summary>
    /// 重置计时器，停止当前计时并重新开始。
    /// </summary>
    /// <remarks>
    /// Resets the timer, stopping the current timing and restarting it.
    /// This method first stops the timer and then restarts it, effectively resetting the timing cycle.
    /// The timer's configuration (such as interval, auto-reset settings, etc.) remains unchanged.
    /// </remarks>
    /// <param name="timer">要重置的计时器实例 / The timer instance to reset.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="timer"/> 为 null 时抛出此异常 / Thrown when <paramref name="timer"/> is null.</exception>
    /// <exception cref="ObjectDisposedException">当 <paramref name="timer"/> 已被释放时抛出此异常 / Thrown when <paramref name="timer"/> has been disposed.</exception>
    /// <example>
    /// <code>
    /// var timer = new Timer(1000); // 1秒间隔 / 1 second interval
    /// timer.Start();
    /// // ... 一段时间后 / ... after some time
    /// timer.Reset(); // 重置计时器，重新开始计时 / Reset the timer and restart timing
    /// </code>
    /// </example>
    public static void Reset(this Timer timer)
    {
        ArgumentNullException.ThrowIfNull(timer, nameof(timer));

        timer.Stop();
        timer.Start();
    }
}