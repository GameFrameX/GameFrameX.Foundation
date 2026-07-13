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

using System.Text;

namespace GameFrameX.Foundation.Hash
{
    /// <summary>
    /// bcrypt 密码哈希帮助类。
    /// 基于 <c>BCrypt.Net-Next.StrongName</c>；bcrypt 协议硬限制密码为 72 字节（UTF-8），超长将抛出 <see cref="ArgumentException"/> 以显式暴露截断风险。
    /// </summary>
    /// <remarks>
    /// bcrypt password hashing helper built on BCrypt.Net-Next.StrongName.
    /// bcrypt hard-limits the password to 72 UTF-8 bytes; longer inputs throw ArgumentException
    /// rather than being silently truncated.
    /// </remarks>
    public static class BcryptHelper
    {
        /// <summary>默认工作因子（cost）。</summary>
        public const int DefaultWorkFactor = 12;

        /// <summary>bcrypt 协议硬限制：密码最大 UTF-8 字节数。</summary>
        public const int MaxPasswordBytes = 72;

        /// <summary>
        /// 使用指定工作因子对密码进行哈希（随机盐）。
        /// </summary>
        /// <param name="password">待哈希的密码。</param>
        /// <param name="workFactor">工作因子（cost），范围 [4, 31]。默认 12。</param>
        /// <returns>bcrypt PHC 字符串 <c>$2a$&lt;cost&gt;$&lt;salt+hash&gt;</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="workFactor"/> 不在 [4, 31] 范围内。</exception>
        /// <exception cref="ArgumentException">密码 UTF-8 字节数超过 <see cref="MaxPasswordBytes"/>（72）。</exception>
        public static string Hash(string password, int workFactor = DefaultWorkFactor)
        {
            ArgumentNullException.ThrowIfNull(password);
            ValidateWorkFactor(workFactor);
            EnsurePasswordLength(password);

            return BCrypt.Net.BCrypt.HashPassword(password, workFactor);
        }

        /// <summary>
        /// 校验密码与已存储的 bcrypt PHC 字符串是否匹配（格式非法返回 false）。
        /// </summary>
        /// <param name="password">待校验的密码。</param>
        /// <param name="storedHash">已存储的 bcrypt 字符串。</param>
        /// <returns>匹配返回 true；密码错误、格式非法或密码超过 bcrypt 72 字节硬限制返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 或 <paramref name="storedHash"/> 为 null。</exception>
        /// <remarks>
        /// fail-closed：<see cref="Hash"/> 对超过 72 字节的密码会抛 <see cref="ArgumentException"/>，
        /// 故不可能存在有效的超长密码 storedHash；此处对超长密码直接返回 false，
        /// 与 PBKDF2/scrypt/Argon2id 的 fail-closed 行为保持一致，调用方无需额外捕获。
        /// </remarks>
        public static bool Verify(string password, string storedHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(storedHash);

            // fail-closed:超长密码不可能匹配任何有效 storedHash，直接返回 false。
            if (Encoding.UTF8.GetByteCount(password) > MaxPasswordBytes)
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(password, storedHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                // 长度已在上文拦截，到达此处的 ArgumentException 仅可能来自 salt 解析（空串/格式非法）。
                return false;
            }
        }

        private static void EnsurePasswordLength(string password)
        {
            if (Encoding.UTF8.GetByteCount(password) > MaxPasswordBytes)
            {
                throw new ArgumentException(
                    "密码 UTF-8 字节数超过 bcrypt 协议硬限制 72 字节。 / Password exceeds bcrypt's 72-byte UTF-8 limit.",
                    nameof(password));
            }
        }

        private static void ValidateWorkFactor(int workFactor)
        {
            if (workFactor < 4 || workFactor > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(workFactor), workFactor, "工作因子必须在 [4, 31] 范围内。 / Work factor must be within [4, 31].");
            }
        }
    }
}
