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

using System.Text;
using GameFrameX.Foundation.Idempotency.Localization;
using GameFrameX.Foundation.Localization.Core;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 请求内容的规范化文本构建器，与哈希算法解耦 / Canonical-text builder for request content, decoupled from hash algorithms.
/// </summary>
/// <remarks>
/// 将键值字段序列化为确定性的规范化文本：键按序号规则升序排列、
/// 跳过排除名单命中的键与值为 <see langword="null"/> 的字段，逐行拼接为 <c>键=值</c> 并以换行符结尾。
/// 相同字段集合不论传入顺序如何，恒定产生相同文本；该文本经编码后交由 <see cref="IRequestDigester"/> 计算摘要。
/// <para>
/// Serializes key-value fields into deterministic canonical text: keys are sorted ascending by ordinal rules,
/// keys on the exclusion list and fields whose value is <see langword="null"/> are skipped, and each remaining
/// field is appended as a <c>key=value</c> line terminated by a newline.
/// The same field set always yields the same text regardless of input order; after encoding, the text is fed to
/// <see cref="IRequestDigester"/> to compute the digest.
/// </para>
/// </remarks>
public static class CanonicalText
{
    /// <summary>
    /// 构建字段集合的规范化文本 / Builds the canonical text of a field collection.
    /// </summary>
    /// <remarks>
    /// 规则：键按 <see cref="string.CompareOrdinal(string, string?)"/> 升序；跳过 <paramref name="excludedFieldNames"/> 命中的键
    /// （例如时间戳、随机数等不应参与摘要的字段）与值为 <see langword="null"/> 的字段；
    /// 每个保留字段输出一行 <c>键=值</c> 并追加换行符。
    /// <para>
    /// Rules: keys are sorted ascending by <see cref="string.CompareOrdinal(string, string?)"/>; keys present in
    /// <paramref name="excludedFieldNames"/> (for example fields like timestamps or random numbers that must not
    /// take part in the digest) and fields whose value is <see langword="null"/> are skipped;
    /// each retained field is emitted as one <c>key=value</c> line plus a trailing newline.
    /// </para>
    /// </remarks>
    /// <param name="fields">参与规范化的字段集合 / The field collection to canonicalize.</param>
    /// <param name="excludedFieldNames">排除字段名集合，<see langword="null"/> 表示不排除任何字段 / The set of excluded field names; <see langword="null"/> excludes nothing.</param>
    /// <returns>规范化文本；字段全被跳过时返回空字符串 / The canonical text; an empty string when every field is skipped.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="fields"/> 为 <see langword="null"/> / <paramref name="fields"/> is <see langword="null"/>.</exception>
    public static string BuildCanonicalText(IEnumerable<KeyValuePair<string, string>> fields, IReadOnlySet<string>? excludedFieldNames = null)
    {
        if (fields == null)
        {
            throw new ArgumentNullException(nameof(fields), LocalizationService.GetString(LocalizationKeys.Exceptions.FieldsCannotBeNull));
        }

        StringBuilder canonicalTextBuilder = new StringBuilder();
        foreach (KeyValuePair<string, string> field in fields.OrderBy(static field => field.Key, StringComparer.Ordinal))
        {
            if (field.Value == null)
            {
                continue;
            }

            if (excludedFieldNames != null && excludedFieldNames.Contains(field.Key))
            {
                continue;
            }

            canonicalTextBuilder.Append(field.Key).Append('=').Append(field.Value).Append('\n');
        }

        return canonicalTextBuilder.ToString();
    }
}
