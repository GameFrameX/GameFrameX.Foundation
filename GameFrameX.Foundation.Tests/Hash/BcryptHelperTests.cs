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
    /// <see cref="BcryptHelper"/> 单元测试（含 72 字节硬限制边界）。
    /// </summary>
    public class BcryptHelperTests
    {
        private const string Password = "P@ssw0rd-测试-🔑";
        private const int FastWorkFactor = 4;

        #region Hash

        [Fact]
        public void Hash_DefaultReturnsPhcFormat()
        {
            string phc = BcryptHelper.Hash(Password, FastWorkFactor);

            Assert.StartsWith("$2", phc);
            Assert.Contains("$", phc.Substring(3));
        }

        [Fact]
        public void Hash_RandomSaltProducesDifferentOutput()
        {
            string a = BcryptHelper.Hash(Password, FastWorkFactor);
            string b = BcryptHelper.Hash(Password, FastWorkFactor);

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Hash_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => BcryptHelper.Hash(null!));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(32)]
        public void Hash_WorkFactorOutOfRange_Throws(int workFactor)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BcryptHelper.Hash(Password, workFactor));
        }

        [Fact]
        public void Hash_Over72Bytes_Throws()
        {
            string longPwd = new string('a', 73); // 73 ASCII bytes

            var ex = Assert.Throws<ArgumentException>(() => BcryptHelper.Hash(longPwd, FastWorkFactor));
            Assert.Equal("password", ex.ParamName);
        }

        [Fact]
        public void Hash_Exactly72Bytes_Succeeds()
        {
            string pwd72 = new string('a', 72);

            string phc = BcryptHelper.Hash(pwd72, FastWorkFactor);

            Assert.True(BcryptHelper.Verify(pwd72, phc));
        }

        #endregion

        #region Verify

        [Fact]
        public void Verify_CorrectPassword_ReturnsTrue()
        {
            string phc = BcryptHelper.Hash(Password, FastWorkFactor);

            Assert.True(BcryptHelper.Verify(Password, phc));
        }

        [Fact]
        public void Verify_WrongPassword_ReturnsFalse()
        {
            string phc = BcryptHelper.Hash(Password, FastWorkFactor);

            Assert.False(BcryptHelper.Verify("wrong-password", phc));
        }

        [Theory]
        [InlineData(null)]
        public void Verify_NullStoredHash_Throws(string stored)
        {
            Assert.Throws<ArgumentNullException>(() => BcryptHelper.Verify(Password, stored!));
        }

        [Fact]
        public void Verify_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => BcryptHelper.Verify(null!, "$2a$04$abcdefghijklmnopqrstuu"));
        }

        [Theory]
        [InlineData("not-a-bcrypt-string")]
        [InlineData("")]
        [InlineData("$2a$04$")] // 截断
        [InlineData("$pbkdf2-sha256$1000$x$y")] // 跨算法前缀
        public void Verify_MalformedOrForeignStoredHash_ReturnsFalse(string stored)
        {
            Assert.False(BcryptHelper.Verify(Password, stored));
        }

        [Fact]
        public void Verify_Over72Bytes_ReturnsFalse()
        {
            string phc = BcryptHelper.Hash("short", FastWorkFactor);
            string longPwd = new string('a', 73);

            Assert.False(BcryptHelper.Verify(longPwd, phc));
        }

        [Fact]
        public void Verify_RepeatedConsistent()
        {
            string phc = BcryptHelper.Hash(Password, FastWorkFactor);

            for (int i = 0; i < 5; i++)
            {
                Assert.True(BcryptHelper.Verify(Password, phc));
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
            string phc = BcryptHelper.Hash(pwd, FastWorkFactor);

            Assert.True(BcryptHelper.Verify(pwd, phc));
        }

        #endregion
    }
}
