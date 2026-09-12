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
/// 幂等键生成器契约 / The idempotency key generator contract.
/// </summary>
/// <remarks>
/// 外部实现契约：<see cref="NewIdempotencyKey"/> 返回的键必须唯一；
/// 返回值格式不保证稳定，使用方不得对其做任何解析或结构假设。
/// <para>
/// External implementation contract: keys returned by <see cref="NewIdempotencyKey"/> must be unique;
/// the returned format is not guaranteed to be stable, and consumers must not parse it or assume any structure.
/// </para>
/// </remarks>
public interface IIdempotencyKeyGenerator
{
    /// <summary>
    /// 生成一个新的幂等键 / Generates a new idempotency key.
    /// </summary>
    /// <remarks>
    /// 返回值格式不保证稳定、禁止被解析；仅保证唯一性。
    /// <para>The returned format is not guaranteed to be stable and must not be parsed; only uniqueness is guaranteed.</para>
    /// </remarks>
    /// <returns>全局唯一的幂等键 / A globally unique idempotency key.</returns>
    string NewIdempotencyKey();
}
