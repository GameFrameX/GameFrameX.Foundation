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

using GameFrameX.Foundation.Hash;
using Xunit;

namespace GameFrameX.Foundation.Tests.Hash
{
    /// <summary>
    /// <see cref="PasswordHashHelper"/> 门面测试：统一调度、PHC 前缀自动识别、推荐判定。
    /// </summary>
    public class PasswordHashHelperTests
    {
        private const string Password = "P@ssw0rd-测试-🔑";

        #region Hash 分发

        [Theory]
        [InlineData(PasswordHashAlgorithmKind.Pbkdf2, "$pbkdf2-sha256$")]
        [InlineData(PasswordHashAlgorithmKind.Bcrypt, "$2")]
        // scrypt / argon2 默认参数较慢，单独 Smoke 覆盖；此处只覆盖快速两种的分发。
        public void Hash_DispatchesByKind_FastAlgorithms(PasswordHashAlgorithmKind kind, string prefix)
        {
            string phc = PasswordHashHelper.Hash(kind, Password);

            Assert.StartsWith(prefix, phc);
        }

        [Fact]
        public void Hash_DispatchesScrypt()
        {
            Assert.StartsWith("$scrypt$", PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Scrypt, Password));
        }

        [Fact]
        public void Hash_DispatchesArgon2id()
        {
            Assert.StartsWith("$argon2id$", PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Argon2id, Password));
        }

        [Fact]
        public void Hash_NullPassword_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Pbkdf2, null!));
        }

        [Fact]
        public void Hash_UnknownKind_Throws()
        {
            var unknown = (PasswordHashAlgorithmKind)999;

            Assert.Throws<ArgumentOutOfRangeException>(() => PasswordHashHelper.Hash(unknown, Password));
        }

        #endregion

        #region Verify 自动识别

        [Fact]
        public void Verify_AutoDetectsEachAlgorithm()
        {
            // 用各 Helper 的极小参数生成，避免默认参数耗时；门面 Verify 仅按前缀分发。
            string pbkdf2 = Pbkdf2Helper.Hash(Password, 100, 16);
            string bcrypt = BcryptHelper.Hash(Password, 4);
            string scrypt = ScryptHelper.Hash(Password, 1024, 1, 1);
            string argon2 = Argon2idHelper.Hash(Password, 16, 1, 1);

            Assert.True(PasswordHashHelper.Verify(Password, pbkdf2));
            Assert.True(PasswordHashHelper.Verify(Password, bcrypt));
            Assert.True(PasswordHashHelper.Verify(Password, scrypt));
            Assert.True(PasswordHashHelper.Verify(Password, argon2));
            Assert.False(PasswordHashHelper.Verify("wrong", pbkdf2));
            Assert.False(PasswordHashHelper.Verify("wrong", bcrypt));
            Assert.False(PasswordHashHelper.Verify("wrong", scrypt));
            Assert.False(PasswordHashHelper.Verify("wrong", argon2));
        }

        [Fact]
        public void Verify_UnknownPrefix_ReturnsFalse()
        {
            Assert.False(PasswordHashHelper.Verify(Password, "$unknown$abcd"));
        }

        [Fact]
        public void Verify_NullArgs_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => PasswordHashHelper.Verify(null!, "$2a$04$x"));
            Assert.Throws<ArgumentNullException>(() => PasswordHashHelper.Verify(Password, null!));
        }

        #endregion

        #region DetectAlgorithm

        [Theory]
        [InlineData("$pbkdf2-sha256$1000$x$y", PasswordHashAlgorithmKind.Pbkdf2)]
        [InlineData("$2a$04$abcdefghijklmnopqrstuu123456789012345678901234567890123456", PasswordHashAlgorithmKind.Bcrypt)]
        [InlineData("$2b$04$abcdefghijklmnopqrstuu123456789012345678901234567890123456", PasswordHashAlgorithmKind.Bcrypt)]
        [InlineData("$2y$04$abcdefghijklmnopqrstuu123456789012345678901234567890123456", PasswordHashAlgorithmKind.Bcrypt)]
        [InlineData("$scrypt$1024$1$1$x$y", PasswordHashAlgorithmKind.Scrypt)]
        [InlineData("$argon2id$v=19$m=16,t=1,p=1$x$y", PasswordHashAlgorithmKind.Argon2id)]
        public void DetectAlgorithm_RecognizesPrefix(string stored, PasswordHashAlgorithmKind expected)
        {
            Assert.Equal(expected, PasswordHashHelper.DetectAlgorithm(stored));
        }

        [Theory]
        [InlineData("$unknown$1$2")]
        [InlineData("")]
        [InlineData("plain-text")]
        public void DetectAlgorithm_UnknownReturnsNull(string stored)
        {
            Assert.Null(PasswordHashHelper.DetectAlgorithm(stored));
        }

        [Fact]
        public void DetectAlgorithm_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => PasswordHashHelper.DetectAlgorithm(null!));
        }

        #endregion

        #region IsRecommended

        [Theory]
        [InlineData(PasswordHashAlgorithmKind.Pbkdf2, false)]
        [InlineData(PasswordHashAlgorithmKind.Bcrypt, false)]
        [InlineData(PasswordHashAlgorithmKind.Scrypt, false)]
        [InlineData(PasswordHashAlgorithmKind.Argon2id, true)]
        public void IsRecommended_OnlyArgon2id(PasswordHashAlgorithmKind kind, bool expected)
        {
            Assert.Equal(expected, PasswordHashHelper.IsRecommended(kind));
        }

        #endregion
    }
}
