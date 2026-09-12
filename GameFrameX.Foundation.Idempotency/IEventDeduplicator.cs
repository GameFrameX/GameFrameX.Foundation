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
/// 消费端事件去重契约 / The consumer-side event deduplication contract.
/// </summary>
/// <remarks>
/// 外部实现契约：同一事件标识的 <see cref="TryConsume"/> 调用中<b>恰好一次</b>返回 <see langword="true"/>
/// （首见登记成功），其余全部返回 <see langword="false"/>（重复被拦截）；
/// 有界实现必须声明"逐出后旧标识可能再次通过"的尽力而为语义。
/// 需要跨进程、跨重启的强去重语义时，使用方应替换为持久化实现。
/// <para>
/// External implementation contract: among <see cref="TryConsume"/> calls on the same event identifier,
/// <b>exactly one</b> returns <see langword="true"/> (the first sighting registers successfully) and all others
/// return <see langword="false"/> (duplicates are intercepted); bounded implementations must declare the
/// best-effort semantics that an evicted old identifier may pass again.
/// When strong cross-process, cross-restart deduplication semantics are required, consumers should switch to a
/// persistent implementation.
/// </para>
/// </remarks>
public interface IEventDeduplicator
{
    /// <summary>
    /// 尝试消费一个事件标识：首见登记成功，重复被拦截 / Tries to consume an event identifier: the first sighting registers, duplicates are intercepted.
    /// </summary>
    /// <remarks>
    /// 消费处理入口调用；返回 <see langword="false"/> 表示重复事件，应跳过处理。
    /// <para>
    /// Called at the consumption-processing entry; a <see langword="false"/> return means a duplicate event
    /// whose processing should be skipped.
    /// </para>
    /// </remarks>
    /// <param name="eventId">事件标识 / The event identifier.</param>
    /// <returns>首次见到该标识返回 <see langword="true"/>；重复见到返回 <see langword="false"/> / <see langword="true"/> on the first sighting of the identifier; <see langword="false"/> on subsequent sightings.</returns>
    bool TryConsume(string eventId);
}
