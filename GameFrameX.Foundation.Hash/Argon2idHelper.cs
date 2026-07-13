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
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace GameFrameX.Foundation.Hash
{
    /// <summary>
    /// Argon2id 密码哈希帮助类。
    /// 基于 BouncyCastle 的 <c>Argon2BytesGenerator</c>（Argon2 v1.3 / PHC 冠军），同时具备内存硬度与抗侧信道特性，为 OWASP 首选推荐；输出自描述 PHC 字符串。
    /// </summary>
    /// <remarks>
    /// Argon2id password hashing helper backed by BouncyCastle Argon2BytesGenerator (Argon2 v1.3 / PHC winner).
    /// Memory-hard and side-channel resistant; OWASP's first-choice recommendation. Emits a self-describing PHC string.
    /// </remarks>
    public static class Argon2idHelper
    {
        /// <summary>PHC 字符串前缀。</summary>
        private const string PhcPrefix = "$argon2id$";

        /// <summary>Argon2 协议版本（0x13 = 19）。</summary>
        private const int Argon2Version = 0x13;

        /// <summary>默认内存开销（KB，64MB）。</summary>
        public const int DefaultMemoryKB = 65_536;

        /// <summary>默认迭代次数（时间开销）。</summary>
        public const int DefaultIterations = 3;

        /// <summary>默认并行度。</summary>
        public const int DefaultParallelism = 1;

        /// <summary>默认输出长度（字节）。</summary>
        public const int DefaultOutputBytes = 32;

        /// <summary>默认盐长度（字节）。</summary>
        public const int DefaultSaltBytes = 16;

        /// <summary>
        /// 使用默认安全参数（m=64MB、t=3、p=1、输出 32 字节、随机 16 字节盐）对密码进行哈希。
        /// </summary>
        /// <param name="password">待哈希的密码。</param>
        /// <returns>自描述 PHC 字符串 <c>$argon2id$v=19$m=&lt;m&gt;,t=&lt;t&gt;,p=&lt;p&gt;$&lt;base64-salt&gt;$&lt;base64-hash&gt;</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        public static string Hash(string password)
        {
            ArgumentNullException.ThrowIfNull(password);

            return Hash(Encoding.UTF8.GetBytes(password), RandomNumberGenerator.GetBytes(DefaultSaltBytes), DefaultMemoryKB, DefaultIterations, DefaultParallelism, DefaultOutputBytes);
        }

        /// <summary>
        /// 按指定参数对密码进行哈希（随机盐）。
        /// </summary>
        /// <param name="password">待哈希的密码。</param>
        /// <param name="memoryKB">内存开销（KB），必须 &gt;= 8。</param>
        /// <param name="iterations">迭代次数，必须 &gt;= 1。</param>
        /// <param name="parallelism">并行度，必须 &gt;= 1。</param>
        /// <returns>自描述 PHC 字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException">任一参数越界。</exception>
        public static string Hash(string password, int memoryKB, int iterations, int parallelism)
        {
            ArgumentNullException.ThrowIfNull(password);
            ValidateParameters(memoryKB, iterations, parallelism);

            return Hash(Encoding.UTF8.GetBytes(password), RandomNumberGenerator.GetBytes(DefaultSaltBytes), memoryKB, iterations, parallelism, DefaultOutputBytes);
        }

        /// <summary>
        /// 按指定盐与参数对密码字节进行哈希（确定性：相同输入产生相同输出）。
        /// </summary>
        /// <param name="password">待哈希的密码字节。</param>
        /// <param name="salt">盐，不得为 null。</param>
        /// <param name="memoryKB">内存开销（KB），必须 &gt;= 8。</param>
        /// <param name="iterations">迭代次数，必须 &gt;= 1。</param>
        /// <param name="parallelism">并行度，必须 &gt;= 1。</param>
        /// <param name="outputBytes">输出哈希长度（字节），必须 &gt;= 1。</param>
        /// <returns>自描述 PHC 字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="password"/> 或 <paramref name="salt"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException">任一参数越界。</exception>
        public static string Hash(byte[] password, byte[] salt, int memoryKB, int iterations, int parallelism, int outputBytes)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(salt);
            ValidateParameters(memoryKB, iterations, parallelism);
            if (outputBytes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(outputBytes), outputBytes, "输出长度必须 >= 1 字节。 / Output length must be >= 1 byte.");
            }

            Argon2Parameters parameters = new Argon2Parameters.Builder(Argon2Parameters.Argon2id)
                .WithVersion(Argon2Version)
                .WithIterations(iterations)
                .WithMemoryAsKB(memoryKB)
                .WithParallelism(parallelism)
                .WithSalt(salt)
                .Build();

            var generator = new Argon2BytesGenerator();
            generator.Init(parameters);
            byte[] derived = new byte[outputBytes];
            generator.GenerateBytes(password, derived);
            return EncodePhc(memoryKB, iterations, parallelism, salt, derived);
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

            // $argon2id$v=19$m=<m>,t=<t>,p=<p>$<base64-salt>$<base64-hash>
            string[] parts = storedHash.Split('$');
            if (parts.Length != 6 || parts[1] != "argon2id")
            {
                return false;
            }

            // parts[2] = v=19; parts[3] = m=<m>,t=<t>,p=<p>
            if (!TryParseVersion(parts[2], out int version))
            {
                return false;
            }

            if (!TryParseArgon2Params(parts[3], out int memoryKB, out int iterations, out int parallelism))
            {
                return false;
            }

            byte[] salt;
            byte[] expected;
            try
            {
                salt = Convert.FromBase64String(parts[4]);
                expected = Convert.FromBase64String(parts[5]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (memoryKB < 8 || iterations < 1 || parallelism < 1 || expected.Length < 1)
            {
                return false;
            }

            Argon2Parameters parameters = new Argon2Parameters.Builder(Argon2Parameters.Argon2id)
                .WithVersion(version)
                .WithIterations(iterations)
                .WithMemoryAsKB(memoryKB)
                .WithParallelism(parallelism)
                .WithSalt(salt)
                .Build();

            var generator = new Argon2BytesGenerator();
            generator.Init(parameters);
            byte[] actual = new byte[expected.Length];
            generator.GenerateBytes(password, actual);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }

        private static string EncodePhc(int memoryKB, int iterations, int parallelism, byte[] salt, byte[] hash)
        {
            return PhcPrefix
                   + "v=" + Argon2Version.ToString(CultureInfo.InvariantCulture) + "$"
                   + "m=" + memoryKB.ToString(CultureInfo.InvariantCulture)
                   + ",t=" + iterations.ToString(CultureInfo.InvariantCulture)
                   + ",p=" + parallelism.ToString(CultureInfo.InvariantCulture) + "$"
                   + Convert.ToBase64String(salt) + "$"
                   + Convert.ToBase64String(hash);
        }

        private static bool TryParseVersion(string segment, out int version)
        {
            version = 0;
            const string prefix = "v=";
            return segment.StartsWith(prefix, StringComparison.Ordinal)
                   && int.TryParse(segment.AsSpan(prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out version);
        }

        private static bool TryParseArgon2Params(string segment, out int memoryKB, out int iterations, out int parallelism)
        {
            memoryKB = iterations = parallelism = 0;
            string[] kvps = segment.Split(',');
            bool gotM = false, gotT = false, gotP = false;
            foreach (string kvp in kvps)
            {
                string[] pair = kvp.Split('=');
                if (pair.Length != 2)
                {
                    return false;
                }

                string key = pair[0];
                if (!int.TryParse(pair[1], NumberStyles.None, CultureInfo.InvariantCulture, out int val))
                {
                    return false;
                }

                switch (key)
                {
                    case "m":
                        memoryKB = val;
                        gotM = true;
                        break;
                    case "t":
                        iterations = val;
                        gotT = true;
                        break;
                    case "p":
                        parallelism = val;
                        gotP = true;
                        break;
                    default:
                        return false;
                }
            }

            return gotM && gotT && gotP;
        }

        private static void ValidateParameters(int memoryKB, int iterations, int parallelism)
        {
            if (memoryKB < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(memoryKB), memoryKB, "内存开销必须 >= 8 KB。 / Memory cost must be >= 8 KB.");
            }

            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(iterations), iterations, "迭代次数必须 >= 1。 / Iterations must be >= 1.");
            }

            if (parallelism < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(parallelism), parallelism, "并行度必须 >= 1。 / Parallelism must be >= 1.");
            }
        }
    }
}
