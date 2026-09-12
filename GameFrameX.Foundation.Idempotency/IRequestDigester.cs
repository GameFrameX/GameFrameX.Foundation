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
/// 请求摘要计算器契约 / The request digester contract.
/// </summary>
/// <remarks>
/// 外部实现契约：<see cref="ComputeDigest"/> 必须确定性——相同输入恒定产生相同输出；
/// 不同输入实际不发生碰撞。摘要用于判定同一幂等键下请求内容是否一致。
/// <para>
/// External implementation contract: <see cref="ComputeDigest"/> must be deterministic — identical inputs
/// always produce identical output, and distinct inputs do not collide in practice.
/// The digest is used to decide whether the request content under the same idempotency key is identical.
/// </para>
/// </remarks>
public interface IRequestDigester
{
    /// <summary>
    /// 对规范化后的请求载荷计算摘要 / Computes a digest over the canonicalized request payload.
    /// </summary>
    /// <remarks>
    /// 相同输入必须恒定返回相同输出；输出为可比较的文本形式（默认实现为 Base64）。
    /// <para>
    /// Identical inputs must always return identical output; the output is comparable text
    /// (Base64 in the default implementation).
    /// </para>
    /// </remarks>
    /// <param name="canonicalPayload">规范化后的请求载荷字节，通常来自 <see cref="CanonicalText.BuildCanonicalText"/> 的编码结果 / The canonicalized request payload bytes, usually the encoded result of <see cref="CanonicalText.BuildCanonicalText"/>.</param>
    /// <returns>请求摘要文本 / The request digest text.</returns>
    string ComputeDigest(ReadOnlySpan<byte> canonicalPayload);
}
