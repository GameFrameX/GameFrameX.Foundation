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
using GameFrameX.Foundation.Hash;
using Xunit;

namespace GameFrameX.Foundation.Tests.Hash
{
    /// <summary>
    /// <see cref="ScryptHelper"/> 单元测试（含内存参数边界）。为加速，默认用极小 N；单条 Smoke 用默认 N。
    /// </summary>
    public class ScryptHelperTests
    {
        private const string Password = "P@ssw0rd-测试-🔑";
        private const int FastN = 1024; // 2^10，快
        private const int FastR = 1;
        private const int FastP = 1;

        #region Hash

        [Fact]
        public void Hash_DefaultSmokeReturnsPhcFormat()
        {
            string phc = ScryptHelper.Hash(Password);

            Assert.StartsWith("$scrypt$", phc);
        }

        [Fact]
        public void Hash_SmallParamsReturnsPhcFormat()
        {
            string phc = ScryptHelper.Hash(Password, FastN, FastR, FastP);

            Assert.StartsWith("$scrypt$", phc);
            string[] parts = phc.Split('$');
            // parts[0]="",[1]=scrypt,[2]=N,[3]=r,[4]=p,[5]=salt,[6]=hash
            Assert.Equal(7, parts.Length);
            Assert.Equal("scrypt", parts[1]);
            Assert.Equal(FastN.ToString(), parts[2]);
            Assert.Equal(FastR.ToString(), parts[3]);
            Assert.Equal(FastP.ToString(), parts[4]);
        }

        [Fact]
        public void Hash_RandomSaltProducesDifferentOutput()
        {
            string a = ScryptHelper.Hash(Password, FastN, FastR, FastP);
            string b = ScryptHelper.Hash(Password, FastN, FastR, FastP);

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Hash_WithExplicitSaltIsDeterministic()
        {
            byte[] password = Encoding.UTF8.GetBytes(Password);
            byte[] salt = Encoding.UTF8.GetBytes("0123456789abcdef");

            string a = ScryptHelper.Hash(password, salt, FastN, FastR, FastP, 16);
            string b = ScryptHelper.Hash(password, salt, FastN, FastR, FastP, 16);

            Assert.Equal(a, b);
        }

        [Fact]
        public void Hash_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => ScryptHelper.Hash(null!));
        }

        [Theory]
        [InlineData(3)]   // 非 2 的幂
        [InlineData(1)]   // < 2
        [InlineData(6)]   // 非 2 的幂
        public void Hash_InvalidN_Throws(int n)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ScryptHelper.Hash(Password, n, FastR, FastP));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Hash_InvalidR_Throws(int r)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ScryptHelper.Hash(Password, FastN, r, FastP));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Hash_InvalidP_Throws(int p)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ScryptHelper.Hash(Password, FastN, FastR, p));
        }

        #endregion

        #region Verify

        [Fact]
        public void Verify_CorrectPassword_ReturnsTrue()
        {
            string phc = ScryptHelper.Hash(Password, FastN, FastR, FastP);

            Assert.True(ScryptHelper.Verify(Password, phc));
        }

        [Fact]
        public void Verify_WrongPassword_ReturnsFalse()
        {
            string phc = ScryptHelper.Hash(Password, FastN, FastR, FastP);

            Assert.False(ScryptHelper.Verify("wrong-password", phc));
        }

        [Theory]
        [InlineData(null)]
        public void Verify_NullStoredHash_Throws(string stored)
        {
            Assert.Throws<ArgumentNullException>(() => ScryptHelper.Verify(Password, stored!));
        }

        [Theory]
        [InlineData("$scrypt$1024$1$1$xxx")] // 缺段
        [InlineData("$unknown$1$1$1$x$y")]    // 未知前缀
        [InlineData("$scrypt$notanumber$1$1$x$y")] // N 非法
        [InlineData("$scrypt$3$1$1$x$y")]     // N 非 2 的幂
        [InlineData("$scrypt$1024$0$1$x$y")]  // r<1
        [InlineData("$scrypt$1024$1$0$x$y")]  // p<1
        [InlineData("$2a$04$abcdefghijklmnopqrstuu123456789012345678901234567890123456")] // 跨算法
        public void Verify_MalformedOrForeignStoredHash_ReturnsFalse(string stored)
        {
            Assert.False(ScryptHelper.Verify(Password, stored));
        }

        [Fact]
        public void Verify_RepeatedConsistent()
        {
            string phc = ScryptHelper.Hash(Password, FastN, FastR, FastP);

            for (int i = 0; i < 5; i++)
            {
                Assert.True(ScryptHelper.Verify(Password, phc));
            }
        }

        #endregion

        #region Boundary

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("🌟🎉密码Passw0rd!@#")]
        public void RoundTrip_SpecialPasswords_Succeeds(string pwd)
        {
            string phc = ScryptHelper.Hash(pwd, FastN, FastR, FastP);

            Assert.True(ScryptHelper.Verify(pwd, phc));
        }

        [Fact]
        public void RoundTrip_MinimalN2_Succeeds()
        {
            string phc = ScryptHelper.Hash(Password, 2, 1, 1);

            Assert.True(ScryptHelper.Verify(Password, phc));
        }

        #endregion
    }
}
