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

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 时间源契约，本包获取当前时间的唯一入口 / The clock contract — the single entry point through which this package obtains the current time.
/// </summary>
/// <remarks>
/// 返回值为 UTC 毫秒时间戳（自 Unix 纪元 1970-01-01T00:00:00Z 起的毫秒数）。
/// <para>
/// 本包内一切时间点（<c>*Time</c> 字段）均为 <see langword="long"/> 类型的 UTC 毫秒，
/// 不使用任何日期结构类型；测试场景可注入自定义实现控制时间推进，
/// 生产环境默认使用 <see cref="SystemClock"/>。
/// </para>
/// <para>
/// The value is a UTC millisecond timestamp (milliseconds elapsed since the Unix epoch 1970-01-01T00:00:00Z).
/// Every time point in this package (fields named <c>*Time</c>) is a <see langword="long"/> UTC millisecond value;
/// no date structure types are used. Tests may inject a custom implementation to control time advancement,
/// while production defaults to <see cref="SystemClock"/>.
/// </para>
/// </remarks>
public interface IClock
{
    /// <summary>
    /// 获取当前时刻的 UTC 毫秒时间戳 / Gets the current UTC millisecond timestamp.
    /// </summary>
    /// <remarks>
    /// 返回自 Unix 纪元起的 UTC 毫秒数；时间推进语义（真实时钟、偏移时钟、虚拟时钟）由实现负责保证。
    /// <para>
    /// Returns the UTC milliseconds elapsed since the Unix epoch; the time-advancement semantics
    /// (real clock, offset clock, virtual clock) are guaranteed by the implementation.
    /// </para>
    /// </remarks>
    long UtcNowTime
    {
        get;
    }
}
