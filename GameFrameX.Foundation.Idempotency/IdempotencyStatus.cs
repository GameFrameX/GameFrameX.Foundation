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
/// 幂等记录的执行状态 / The execution status of an idempotency record.
/// </summary>
/// <remarks>
/// 状态流转：占位成功后为 <see cref="Processing"/>，业务执行完毕后由调用方落为
/// <see cref="Completed"/>（携带首次响应，供后续重放）或 <see cref="Failed"/>（由失败重放策略决定重试行为）。
/// <para>
/// Status flow: after a successful occupation the record is <see cref="Processing"/>; once the business
/// execution finishes, the caller settles it as <see cref="Completed"/> (carrying the first response for later
/// replay) or <see cref="Failed"/> (retry behavior is decided by the failed-replay policy).
/// </para>
/// </remarks>
public enum IdempotencyStatus
{
    /// <summary>
    /// 占位成功，业务正在执行中 / Occupation succeeded and the business is executing.
    /// </summary>
    Processing = 0,

    /// <summary>
    /// 业务执行成功，首次响应已固化，可安全重放 / The business succeeded; the first response is persisted and safe to replay.
    /// </summary>
    Completed = 1,

    /// <summary>
    /// 业务执行失败，重放行为由 <see cref="FailedReplayPolicy"/> 决定 / The business failed; replay behavior is decided by <see cref="FailedReplayPolicy"/>.
    /// </summary>
    Failed = 2,
}
