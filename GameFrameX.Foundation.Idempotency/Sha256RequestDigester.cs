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

using System.Security.Cryptography;
using GameFrameX.Foundation.Idempotency.Localization;
using GameFrameX.Foundation.Localization.Core;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 基于 SHA-256 的 <see cref="IRequestDigester"/> 默认实现 / The SHA-256-based default implementation of <see cref="IRequestDigester"/>.
/// </summary>
/// <remarks>
/// 对规范化载荷计算 SHA-256 摘要并以 Base64 文本输出；相同输入恒定产生相同输出。
/// 使用一次性哈希实例，线程安全。
/// <para>
/// Computes a SHA-256 digest over the canonicalized payload and emits it as Base64 text;
/// identical inputs always produce identical output. Uses a one-shot hash instance; thread-safe.
/// </para>
/// </remarks>
public sealed class Sha256RequestDigester : IRequestDigester
{
    /// <summary>
    /// <see cref="Sha256RequestDigester"/> 的全局单例实例 / The global singleton instance of <see cref="Sha256RequestDigester"/>.
    /// </summary>
    /// <remarks>
    /// 实现无状态且线程安全，可直接共享。
    /// <para>The implementation is stateless and thread-safe; the instance can be shared freely.</para>
    /// </remarks>
    public static readonly Sha256RequestDigester Instance = new Sha256RequestDigester();

    private Sha256RequestDigester()
    {
    }

    /// <inheritdoc />
    public string ComputeDigest(ReadOnlySpan<byte> canonicalPayload)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            Span<byte> digestDestination = stackalloc byte[32];
            if (!sha256.TryComputeHash(canonicalPayload, digestDestination, out int bytesWritten) || bytesWritten != 32)
            {
                throw new InvalidOperationException(LocalizationService.GetString(LocalizationKeys.Exceptions.DigestComputationFailed));
            }

            return Convert.ToBase64String(digestDestination);
        }
    }
}
