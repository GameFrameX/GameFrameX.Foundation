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
    /// <see cref="Argon2idHelper"/> 单元测试（含内存/时间/并行参数边界）。为加速，默认用极小参数；单条 Smoke 用默认参数。
    /// </summary>
    public class Argon2idHelperTests
    {
        private const string Password = "P@ssw0rd-测试-🔑";
        private const int FastMemKB = 16;   // 16 KB，极小但合法（>= 8）
        private const int FastIter = 1;
        private const int FastPar = 1;

        #region Hash

        [Fact]
        public void Hash_DefaultSmokeReturnsPhcFormat()
        {
            string phc = Argon2idHelper.Hash(Password);

            Assert.StartsWith("$argon2id$", phc);
        }

        [Fact]
        public void Hash_SmallParamsReturnsPhcFormat()
        {
            string phc = Argon2idHelper.Hash(Password, FastMemKB, FastIter, FastPar);

            Assert.StartsWith("$argon2id$v=19$m=", phc);
            string[] parts = phc.Split('$');
            // parts[0]="",[1]=argon2id,[2]=v=19,[3]=m=..,t=..,p=..,[4]=salt,[5]=hash
            Assert.Equal(6, parts.Length);
            Assert.Equal("argon2id", parts[1]);
            Assert.StartsWith("v=19", parts[2]);
            Assert.Contains("m=" + FastMemKB, parts[3]);
            Assert.Contains("t=" + FastIter, parts[3]);
            Assert.Contains("p=" + FastPar, parts[3]);
        }

        [Fact]
        public void Hash_RandomSaltProducesDifferentOutput()
        {
            string a = Argon2idHelper.Hash(Password, FastMemKB, FastIter, FastPar);
            string b = Argon2idHelper.Hash(Password, FastMemKB, FastIter, FastPar);

            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Hash_WithExplicitSaltIsDeterministic()
        {
            byte[] password = Encoding.UTF8.GetBytes(Password);
            byte[] salt = Encoding.UTF8.GetBytes("0123456789abcdef");

            string a = Argon2idHelper.Hash(password, salt, FastMemKB, FastIter, FastPar, 16);
            string b = Argon2idHelper.Hash(password, salt, FastMemKB, FastIter, FastPar, 16);

            Assert.Equal(a, b);
        }

        [Fact]
        public void Hash_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Argon2idHelper.Hash(null!));
        }

        [Theory]
        [InlineData(7)]   // < 8
        [InlineData(0)]
        public void Hash_InvalidMemoryKB_Throws(int mem)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Argon2idHelper.Hash(Password, mem, FastIter, FastPar));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Hash_InvalidIterations_Throws(int iter)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Argon2idHelper.Hash(Password, FastMemKB, iter, FastPar));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Hash_InvalidParallelism_Throws(int par)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Argon2idHelper.Hash(Password, FastMemKB, FastIter, par));
        }

        #endregion

        #region Verify

        [Fact]
        public void Verify_CorrectPassword_ReturnsTrue()
        {
            string phc = Argon2idHelper.Hash(Password, FastMemKB, FastIter, FastPar);

            Assert.True(Argon2idHelper.Verify(Password, phc));
        }

        [Fact]
        public void Verify_WrongPassword_ReturnsFalse()
        {
            string phc = Argon2idHelper.Hash(Password, FastMemKB, FastIter, FastPar);

            Assert.False(Argon2idHelper.Verify("wrong-password", phc));
        }

        [Theory]
        [InlineData(null)]
        public void Verify_NullStoredHash_Throws(string stored)
        {
            Assert.Throws<ArgumentNullException>(() => Argon2idHelper.Verify(Password, stored!));
        }

        [Theory]
        [InlineData("$argon2id$v=19$m=16,t=1,p=1$xxx")] // 缺 hash 段
        [InlineData("$unknown$v=19$m=16,t=1,p=1$x$y")]   // 未知前缀
        [InlineData("$argon2id$x=19$m=16,t=1,p=1$x$y")]  // 版本段格式错
        [InlineData("$argon2id$v=19$m=16,t=1$aaa$x$y")]  // 缺 p
        [InlineData("$argon2id$v=19$m=16,t=1,q=1$x$y")]  // 未知键 q
        [InlineData("$argon2id$v=19$m=7,t=1,p=1$x$y")]   // 内存越界
        [InlineData("$argon2id$v=19$m=16,t=0,p=1$x$y")]  // 迭代越界
        [InlineData("$2a$04$abcdefghijklmnopqrstuu123456789012345678901234567890123456")] // 跨算法
        public void Verify_MalformedOrForeignStoredHash_ReturnsFalse(string stored)
        {
            Assert.False(Argon2idHelper.Verify(Password, stored));
        }

        [Fact]
        public void Verify_RepeatedConsistent()
        {
            string phc = Argon2idHelper.Hash(Password, FastMemKB, FastIter, FastPar);

            for (int i = 0; i < 5; i++)
            {
                Assert.True(Argon2idHelper.Verify(Password, phc));
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
            string phc = Argon2idHelper.Hash(pwd, FastMemKB, FastIter, FastPar);

            Assert.True(Argon2idHelper.Verify(pwd, phc));
        }

        [Fact]
        public void RoundTrip_MinimalMemory8_Succeeds()
        {
            string phc = Argon2idHelper.Hash(Password, 8, 1, 1);

            Assert.True(Argon2idHelper.Verify(Password, phc));
        }

        #endregion
    }
}
