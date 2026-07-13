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

using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash
{
    /// <summary>
    /// PBKDF2-HMAC-SHA256 密码哈希帮助类。
    /// 使用 BCL 内置 <see cref="Rfc2898DeriveBytes"/>，无额外依赖；输出自描述 PHC 字符串以便 <see cref="Verify(byte[], string)"/> 无需额外参数即可校验。
    /// </summary>
    /// <remarks>
    /// PBKDF2-HMAC-SHA256 password hashing helper backed by the BCL Rfc2898DeriveBytes.
    /// Emits a self-describing PHC string so verification needs no extra parameters.
    /// </remarks>
    public static class Pbkdf2Helper
    {
        /// <summary>PHC 字符串前缀。</summary>
        private const string PhcPrefix = "$pbkdf2-sha256$";

        /// <summary>默认迭代次数（OWASP 2023 推荐下限）。</summary>
        public const int DefaultIterations = 600_000;

        /// <summary>默认输出长度（字节）。</summary>
        public const int DefaultOutputBytes = 32;

        /// <summary>默认盐长度（字节）。</summary>
        public const int DefaultSaltBytes = 16;

        /// <summary>
        /// 使用默认安全参数（迭代 600000、输出 32 字节、随机 16 字节盐）对密码进行哈希。
        /// </summary>
        /// <param name="password">待哈希的密码。</param>
        /// <returns>自描述 PHC 字符串 <c>$pbkdf2-sha256$&lt;iter&gt;$&lt;base64-salt&gt;$&lt;base64-hash&gt;</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        public static string Hash(string password)
        {
            ArgumentNullException.ThrowIfNull(password);

            return Hash(Encoding.UTF8.GetBytes(password), RandomNumberGenerator.GetBytes(DefaultSaltBytes), DefaultIterations, DefaultOutputBytes);
        }

        /// <summary>
        /// 按指定参数对密码进行哈希（随机盐）。
        /// </summary>
        /// <param name="password">待哈希的密码。</param>
        /// <param name="iterations">迭代次数，必须 &gt;= 1。</param>
        /// <param name="outputBytes">输出哈希长度（字节），必须 &gt;= 1。</param>
        /// <returns>自描述 PHC 字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="iterations"/> 或 <paramref name="outputBytes"/> 小于 1。</exception>
        public static string Hash(string password, int iterations, int outputBytes)
        {
            ArgumentNullException.ThrowIfNull(password);
            ValidateIterations(iterations);
            ValidateOutputBytes(outputBytes);

            return Hash(Encoding.UTF8.GetBytes(password), RandomNumberGenerator.GetBytes(DefaultSaltBytes), iterations, outputBytes);
        }

        /// <summary>
        /// 按指定盐与参数对密码字节进行哈希（确定性：相同输入产生相同输出）。
        /// </summary>
        /// <param name="password">待哈希的密码字节。</param>
        /// <param name="salt">盐，不得为 null。</param>
        /// <param name="iterations">迭代次数，必须 &gt;= 1。</param>
        /// <param name="outputBytes">输出哈希长度（字节），必须 &gt;= 1。</param>
        /// <returns>自描述 PHC 字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 或 <paramref name="salt"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="iterations"/> 或 <paramref name="outputBytes"/> 小于 1。</exception>
        public static string Hash(byte[] password, byte[] salt, int iterations, int outputBytes)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(salt);
            ValidateIterations(iterations);
            ValidateOutputBytes(outputBytes);

            byte[] derived = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, outputBytes);
            return EncodePhc(iterations, salt, derived);
        }

        /// <summary>
        /// 校验密码与已存储的 PHC 字符串是否匹配（常量时间比较，格式非法返回 false）。
        /// </summary>
        /// <param name="password">待校验的密码。</param>
        /// <param name="storedHash">已存储的 PHC 字符串。</param>
        /// <returns>匹配返回 true；密码错误或格式非法返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 或 <paramref name="storedHash"/> 为 null。</exception>
        public static bool Verify(string password, string storedHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(storedHash);

            return Verify(Encoding.UTF8.GetBytes(password), storedHash);
        }

        /// <summary>
        /// 校验密码字节与已存储的 PHC 字符串是否匹配（常量时间比较，格式非法返回 false）。
        /// </summary>
        /// <param name="password">待校验的密码字节。</param>
        /// <param name="storedHash">已存储的 PHC 字符串。</param>
        /// <returns>匹配返回 true；密码错误或格式非法返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 或 <paramref name="storedHash"/> 为 null。</exception>
        public static bool Verify(byte[] password, string storedHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(storedHash);

            if (!storedHash.StartsWith(PhcPrefix, StringComparison.Ordinal))
            {
                return false;
            }

            // $pbkdf2-sha256$<iter>$<base64-salt>$<base64-hash>
            string[] parts = storedHash.Split('$');
            if (parts.Length != 5 || parts[1] != "pbkdf2-sha256")
            {
                return false;
            }

            if (!int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out int iterations))
            {
                return false;
            }

            byte[] salt;
            byte[] expected;
            try
            {
                salt = Convert.FromBase64String(parts[3]);
                expected = Convert.FromBase64String(parts[4]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (iterations < 1 || expected.Length < 1)
            {
                return false;
            }

            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }

        private static string EncodePhc(int iterations, byte[] salt, byte[] hash)
        {
            return PhcPrefix
                   + iterations.ToString(CultureInfo.InvariantCulture)
                   + "$" + Convert.ToBase64String(salt)
                   + "$" + Convert.ToBase64String(hash);
        }

        private static void ValidateIterations(int iterations)
        {
            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(iterations), iterations, "迭代次数必须 >= 1。 / Iterations must be >= 1.");
            }
        }

        private static void ValidateOutputBytes(int outputBytes)
        {
            if (outputBytes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(outputBytes), outputBytes, "输出长度必须 >= 1 字节。 / Output length must be >= 1 byte.");
            }
        }
    }
}
