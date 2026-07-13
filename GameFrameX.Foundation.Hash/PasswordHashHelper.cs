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

namespace GameFrameX.Foundation.Hash
{
    /// <summary>
    /// 密码哈希统一门面，调度四种 KDF（PBKDF2 / bcrypt / scrypt / Argon2id）。
    /// <see cref="Verify(string, string)"/> 可按已存储字符串的 PHC 前缀自动识别算法，无需显式传入算法种类。
    /// </summary>
    /// <remarks>
    /// Unified facade over the four KDFs (PBKDF2 / bcrypt / scrypt / Argon2id).
    /// Verify auto-detects the algorithm from the stored string's PHC prefix.
    /// </remarks>
    public static class PasswordHashHelper
    {
        /// <summary>
        /// 按指定算法种类使用各自默认安全参数对密码进行哈希。
        /// </summary>
        /// <param name="kind">密码哈希算法种类。</param>
        /// <param name="password">待哈希的密码。</param>
        /// <returns>该算法的自描述 PHC 字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind"/> 不是已知种类；或触发 bcrypt 的 72 字节限制（<see cref="BcryptHelper.MaxPasswordBytes"/>）。</exception>
        public static string Hash(PasswordHashAlgorithmKind kind, string password)
        {
            ArgumentNullException.ThrowIfNull(password);

            switch (kind)
            {
                case PasswordHashAlgorithmKind.Pbkdf2:
                    return Pbkdf2Helper.Hash(password);
                case PasswordHashAlgorithmKind.Bcrypt:
                    return BcryptHelper.Hash(password);
                case PasswordHashAlgorithmKind.Scrypt:
                    return ScryptHelper.Hash(password);
                case PasswordHashAlgorithmKind.Argon2id:
                    return Argon2idHelper.Hash(password);
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, "不支持的密码哈希算法种类。 / Unsupported password hash algorithm kind.");
            }
        }

        /// <summary>
        /// 校验密码与已存储的 PHC 字符串是否匹配。算法按前缀自动识别，未知前缀或格式非法返回 false。
        /// </summary>
        /// <param name="password">待校验的密码。</param>
        /// <param name="storedHash">已存储的 PHC 字符串。</param>
        /// <returns>匹配返回 true；密码错误、前缀未知或格式非法返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 或 <paramref name="storedHash"/> 为 null。</exception>
        /// <exception cref="ArgumentException">存储串为 bcrypt 且密码超过 <see cref="BcryptHelper.MaxPasswordBytes"/> 字节。</exception>
        public static bool Verify(string password, string storedHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(storedHash);

            switch (DetectAlgorithm(storedHash))
            {
                case PasswordHashAlgorithmKind.Pbkdf2:
                    return Pbkdf2Helper.Verify(password, storedHash);
                case PasswordHashAlgorithmKind.Bcrypt:
                    return BcryptHelper.Verify(password, storedHash);
                case PasswordHashAlgorithmKind.Scrypt:
                    return ScryptHelper.Verify(password, storedHash);
                case PasswordHashAlgorithmKind.Argon2id:
                    return Argon2idHelper.Verify(password, storedHash);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断指定算法是否为 OWASP 推荐的首选算法（当前仅 <see cref="PasswordHashAlgorithmKind.Argon2id"/>）。
        /// </summary>
        /// <param name="kind">密码哈希算法种类。</param>
        /// <returns>是推荐算法返回 true，否则 false。</returns>
        public static bool IsRecommended(PasswordHashAlgorithmKind kind)
        {
            return kind == PasswordHashAlgorithmKind.Argon2id;
        }

        /// <summary>
        /// 按 PHC 前缀识别已存储字符串对应的算法种类。
        /// </summary>
        /// <param name="storedHash">已存储的 PHC 字符串。</param>
        /// <returns>识别到的算法；未知前缀返回 null。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storedHash"/> 为 null。</exception>
        public static PasswordHashAlgorithmKind? DetectAlgorithm(string storedHash)
        {
            ArgumentNullException.ThrowIfNull(storedHash);

            if (storedHash.StartsWith("$pbkdf2-sha256$", StringComparison.Ordinal))
            {
                return PasswordHashAlgorithmKind.Pbkdf2;
            }

            if (storedHash.StartsWith("$2a$", StringComparison.Ordinal)
                || storedHash.StartsWith("$2b$", StringComparison.Ordinal)
                || storedHash.StartsWith("$2y$", StringComparison.Ordinal))
            {
                return PasswordHashAlgorithmKind.Bcrypt;
            }

            if (storedHash.StartsWith("$scrypt$", StringComparison.Ordinal))
            {
                return PasswordHashAlgorithmKind.Scrypt;
            }

            if (storedHash.StartsWith("$argon2id$", StringComparison.Ordinal))
            {
                return PasswordHashAlgorithmKind.Argon2id;
            }

            return null;
        }
    }
}
