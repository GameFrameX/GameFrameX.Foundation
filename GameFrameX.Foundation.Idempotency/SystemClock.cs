// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 Apache License 2.0 许可证分发，
//  This project is distributed under the Apache License 2.0,
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
//  CNB Repository:    https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Utility;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 基于系统时钟的默认时间源实现 / Default clock implementation backed by the system clock.
/// </summary>
/// <remarks>
/// 委托 <see cref="TimerHelper.UnixTimeMilliseconds()"/> 获取当前 UTC 毫秒时间戳，
/// 与全生态共享同一时间源；通过 <see cref="TimerHelper.SetTimeProvider"/> 设置的
/// TimeProvider 与时间偏移钩子对本实现同步生效。
/// <para>本包不提供时间戳与日期结构之间的转换 API：调用方按需使用基础类库或 Utility 包的 <see cref="TimerHelper"/> 完成。</para>
/// <para>
/// Delegates to <see cref="TimerHelper.UnixTimeMilliseconds()"/> to obtain the current UTC millisecond timestamp,
/// sharing a single time source with the whole ecosystem; a TimeProvider or time-offset hook configured via
/// <see cref="TimerHelper.SetTimeProvider"/> takes effect on this implementation as well.
/// This package provides no conversion API between timestamps and date structures:
/// callers use the base class library or the Utility package's <see cref="TimerHelper"/> as needed.
/// </para>
/// </remarks>
public sealed class SystemClock : IClock
{
    /// <summary>
    /// <see cref="SystemClock"/> 的全局单例实例 / The global singleton instance of <see cref="SystemClock"/>.
    /// </summary>
    /// <remarks>
    /// 实现无状态且线程安全，可直接共享。
    /// <para>The implementation is stateless and thread-safe; the instance can be shared freely.</para>
    /// </remarks>
    public static readonly SystemClock Instance = new SystemClock();

    private SystemClock()
    {
    }

    /// <inheritdoc />
    public long UtcNowTime
    {
        get
        {
            return TimerHelper.UnixTimeMilliseconds();
        }
    }
}
