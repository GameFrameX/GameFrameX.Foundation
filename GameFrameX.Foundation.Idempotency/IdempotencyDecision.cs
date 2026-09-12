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
/// 幂等判定的决策种类 / The decision kinds of an idempotency judgement.
/// </summary>
/// <remarks>
/// 与判定语义表的分支一一对应：<see cref="Execute"/> 允许执行业务；
/// <see cref="Replay"/> 不得再次执行，应回放存量结果；
/// <see cref="Conflict"/> 表示同一键下请求摘要不一致，拒绝执行；
/// <see cref="Busy"/> 表示并发占位方长时间未结束，稍后重试。
/// <para>
/// Each value corresponds to a branch of the decision-semantics table:
/// <see cref="Execute"/> permits business execution; <see cref="Replay"/> forbids re-execution and
/// replays the stored result; <see cref="Conflict"/> means the request digest differs under the same key
/// and execution is refused; <see cref="Busy"/> means the concurrent occupant has not settled within the
/// wait window and the caller should retry later.
/// </para>
/// </remarks>
public enum IdempotencyDecisionKind
{
    /// <summary>
    /// 允许执行业务：占位成功，执行完毕后调用方须落为完成或失败 / Execute the business: the occupation succeeded, and the caller must settle the record as completed or failed afterwards.
    /// </summary>
    Execute = 0,

    /// <summary>
    /// 回放存量结果：不得再次执行业务；依据 <see cref="IdempotencyDecision.ExistingRecord"/> 的状态回放首次响应或原错误 / Replay the stored result: do not execute again; replay the first response or the original error according to the state of <see cref="IdempotencyDecision.ExistingRecord"/>.
    /// </summary>
    Replay = 1,

    /// <summary>
    /// 键冲突：同一键下请求摘要不一致，拒绝执行，由调用方决定报错或换键 / Key conflict: the request digest differs under the same key; execution is refused and the caller decides to error out or switch keys.
    /// </summary>
    Conflict = 2,

    /// <summary>
    /// 并发占位方在等待超时内未结束，本次请求稍后重试 / The concurrent occupant did not settle within the wait timeout; retry this request later.
    /// </summary>
    Busy = 3,
}

/// <summary>
/// 幂等判定结果：协调器对一次占位请求的最终裁决 / The idempotency decision: the coordinator's final verdict for one occupation request.
/// </summary>
/// <remarks>
/// 不可变对象。<see cref="DecisionKind"/> 为 <see cref="IdempotencyDecisionKind.Replay"/>、
/// <see cref="IdempotencyDecisionKind.Conflict"/> 或 <see cref="IdempotencyDecisionKind.Busy"/> 时
/// <see cref="ExistingRecord"/> 非空；为 <see cref="IdempotencyDecisionKind.Execute"/> 时为 <see langword="null"/>。
/// <para>
/// Immutable object. <see cref="ExistingRecord"/> is non-null when <see cref="DecisionKind"/> is
/// <see cref="IdempotencyDecisionKind.Replay"/>, <see cref="IdempotencyDecisionKind.Conflict"/> or
/// <see cref="IdempotencyDecisionKind.Busy"/>, and <see langword="null"/> for <see cref="IdempotencyDecisionKind.Execute"/>.
/// </para>
/// </remarks>
public sealed class IdempotencyDecision
{
    /// <summary>
    /// 初始化 <see cref="IdempotencyDecision"/> / Initializes <see cref="IdempotencyDecision"/>.
    /// </summary>
    /// <param name="decisionKind">决策种类 / The decision kind.</param>
    /// <param name="existingRecord">触发该决策的存量记录，<see cref="IdempotencyDecisionKind.Execute"/> 时为 <see langword="null"/> / The existing record that triggered the decision; <see langword="null"/> for <see cref="IdempotencyDecisionKind.Execute"/>.</param>
    public IdempotencyDecision(IdempotencyDecisionKind decisionKind, IdempotencyRecord? existingRecord = null)
    {
        DecisionKind = decisionKind;
        ExistingRecord = existingRecord;
    }

    /// <summary>
    /// 决策种类 / The decision kind.
    /// </summary>
    public IdempotencyDecisionKind DecisionKind
    {
        get;
    }

    /// <summary>
    /// 触发该决策的存量记录 / The existing record that triggered the decision.
    /// </summary>
    /// <remarks>
    /// 仅 <see cref="IdempotencyDecisionKind.Replay"/>、<see cref="IdempotencyDecisionKind.Conflict"/>、
    /// <see cref="IdempotencyDecisionKind.Busy"/> 时非空：
    /// 回放时从其读取首次响应或失败状态；冲突与忙碌时用于诊断。
    /// <para>
    /// Non-null only for <see cref="IdempotencyDecisionKind.Replay"/>, <see cref="IdempotencyDecisionKind.Conflict"/>
    /// and <see cref="IdempotencyDecisionKind.Busy"/>: on replay the first response or failure state is read from it;
    /// on conflict and busy it is provided for diagnostics.
    /// </para>
    /// </remarks>
    public IdempotencyRecord? ExistingRecord
    {
        get;
    }
}
