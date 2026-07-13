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

using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Hash;
using Xunit;

namespace GameFrameX.Foundation.Tests.Hash
{
    /// <summary>
    /// <see cref="Pbkdf2Helper"/> 单元测试（含严格边界与 BCL 交叉验证）。
    /// </summary>
    public class Pbkdf2HelperTests
    {
        private const string Password = "P@ssw0rd-测试-🔑";

        #region Hash

        [Fact]
        public void Hash_DefaultReturnsPhcFormat()
        {
            string phc = Pbkdf2Helper.Hash(Password);

            Assert.StartsWith("$pbkdf2-sha256$", phc);
            string[] parts = phc.Split('$');
            // parts[0]="", [1]=pbkdf2-sha256, [2]=iter, [3]=salt, [4]=hash
            Assert.Equal(5, parts.Length);
            Assert.Equal("pbkdf2-sha256", parts[1]);
            Assert.Equal(Pbkdf2Helper.DefaultIterations.ToString(), parts[2]);
        }

        [Fact]
        public void Hash_RandomSaltProducesDifferentOutput()
        {
            string a = Pbkdf2Helper.Hash(Password, 1000, 16);
            string b = Pbkdf2Helper.Hash(Password, 1000, 16);

            Assert.NotEqual(a, b);
            Assert.True(Pbkdf2Helper.Verify(Password, a));
            Assert.True(Pbkdf2Helper.Verify(Password, b));
        }

        [Fact]
        public void Hash_WithExplicitSaltIsDeterministic()
        {
            byte[] password = Encoding.UTF8.GetBytes(Password);
            byte[] salt = Encoding.UTF8.GetBytes("0123456789abcdef");

            string a = Pbkdf2Helper.Hash(password, salt, 1000, 16);
            string b = Pbkdf2Helper.Hash(password, salt, 1000, 16);

            Assert.Equal(a, b);
        }

        [Fact]
        public void Hash_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Pbkdf2Helper.Hash((string)null!));
            Assert.Throws<ArgumentNullException>(() => Pbkdf2Helper.Hash(null, 1000, 16));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Hash_InvalidIterations_Throws(int iterations)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Pbkdf2Helper.Hash(Password, iterations, 16));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Hash_InvalidOutputBytes_Throws(int outputBytes)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Pbkdf2Helper.Hash(Password, 1000, outputBytes));
        }

        #endregion

        #region Verify

        [Fact]
        public void Verify_CorrectPassword_ReturnsTrue()
        {
            string phc = Pbkdf2Helper.Hash(Password, 1000, 16);

            Assert.True(Pbkdf2Helper.Verify(Password, phc));
        }

        [Fact]
        public void Verify_WrongPassword_ReturnsFalse()
        {
            string phc = Pbkdf2Helper.Hash(Password, 1000, 16);

            Assert.False(Pbkdf2Helper.Verify("wrong-password", phc));
        }

        [Theory]
        [InlineData(null)]
        public void Verify_NullStoredHash_Throws(string stored)
        {
            Assert.Throws<ArgumentNullException>(() => Pbkdf2Helper.Verify(Password, stored!));
        }

        [Fact]
        public void Verify_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Pbkdf2Helper.Verify((string)null!, "$pbkdf2-sha256$1000$x$x"));
        }

        [Theory]
        // 截断 / 缺段
        [InlineData("$pbkdf2-sha256$1000$xxx")]
        // 未知前缀
        [InlineData("$unknown$1000$x$y")]
        // 非法迭代值
        [InlineData("$pbkdf2-sha256$notanumber$x$y")]
        // base64 非法
        [InlineData("$pbkdf2-sha256$1000$@@@notbase64@@@$AAAA")]
        // 空字符串
        [InlineData("")]
        // bcrypt 串（跨算法）
        [InlineData("$2a$10$abcdefghijklmnopqrstuv123456789012345678901234567890123456")]
        public void Verify_MalformedOrForeignStoredHash_ReturnsFalse(string stored)
        {
            Assert.False(Pbkdf2Helper.Verify(Password, stored));
        }

        [Fact]
        public void Verify_RepeatedConsistent()
        {
            string phc = Pbkdf2Helper.Hash(Password, 1000, 16);

            for (int i = 0; i < 5; i++)
            {
                Assert.True(Pbkdf2Helper.Verify(Password, phc));
            }
        }

        [Fact]
        public void Verify_CrossValidatesWithBclRfc2898()
        {
            byte[] password = Encoding.UTF8.GetBytes(Password);
            byte[] salt = Encoding.UTF8.GetBytes("saltsalt12345678");
            const int Iterations = 5000;
            const int OutputBytes = 24;

            string phc = Pbkdf2Helper.Hash(password, salt, Iterations, OutputBytes);

            byte[] expected = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, OutputBytes);
            string[] parts = phc.Split('$');
            byte[] actual = Convert.FromBase64String(parts[4]);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Boundary

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("🌟🎉密码Passw0rd!@#")]
        public void RoundTrip_SpecialPasswords_Succeeds(string pwd)
        {
            string phc = Pbkdf2Helper.Hash(pwd, 1000, 16);

            Assert.True(Pbkdf2Helper.Verify(pwd, phc));
        }

        [Fact]
        public void RoundTrip_OneMegaBytePassword_Succeeds()
        {
            string pwd = new string('x', 1_048_576);

            string phc = Pbkdf2Helper.Hash(pwd, 100, 16);

            Assert.True(Pbkdf2Helper.Verify(pwd, phc));
        }

        [Fact]
        public void RoundTrip_MinimalIterations_Succeeds()
        {
            string phc = Pbkdf2Helper.Hash(Password, 1, 16);

            Assert.True(Pbkdf2Helper.Verify(Password, phc));
        }

        #endregion
    }
}
