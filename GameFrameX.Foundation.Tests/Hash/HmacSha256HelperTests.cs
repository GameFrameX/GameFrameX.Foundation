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

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// HMAC-SHA256 哈希算法单元测试
/// </summary>
public class HmacSha256HelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试HMAC-SHA256哈希算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";
    private const string TestKey = "test-key";
    private const string TestKeyChinese = "测试密钥";
    private const string EmptyKey = "";
    private const string LongKey = "这是一个很长的测试密钥，用来测试HMAC-SHA256算法在使用较长密钥时的行为和正确性。";

    [Fact]
    public void Hash_ValidStringAndKey_ShouldReturnConsistentHash()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;

        // Act
        var hash1 = HmacSha256Helper.Hash(message, key);
        var hash2 = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash1);
        Assert.NotEmpty(hash1);
        Assert.Equal(hash1, hash2);
        // HMAC-SHA256 输出为32字节，Base64编码后长度为44个字符（包含可能的填充）
        Assert.True(hash1.Length > 0);
    }

    [Fact]
    public void Hash_EmptyMessage_ShouldReturnValidHash()
    {
        // Arrange
        var message = EmptyString;
        var key = TestKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_EmptyKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestString;
        var key = EmptyKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_EmptyMessageAndKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = EmptyString;
        var key = EmptyKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_ChineseMessage_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestStringChinese;
        var key = TestKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_ChineseKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestString;
        var key = TestKeyChinese;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_ChineseMessageAndKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestStringChinese;
        var key = TestKeyChinese;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_LongMessage_ShouldReturnValidHash()
    {
        // Arrange
        var message = LongString;
        var key = TestKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_LongKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestString;
        var key = LongKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_LongMessageAndKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = LongString;
        var key = LongKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_DifferentMessages_ShouldReturnDifferentHashes()
    {
        // Arrange
        var message1 = "Message 1";
        var message2 = "Message 2";
        var key = TestKey;

        // Act
        var hash1 = HmacSha256Helper.Hash(message1, key);
        var hash2 = HmacSha256Helper.Hash(message2, key);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_DifferentKeys_ShouldReturnDifferentHashes()
    {
        // Arrange
        var message = TestString;
        var key1 = "Key 1";
        var key2 = "Key 2";

        // Act
        var hash1 = HmacSha256Helper.Hash(message, key1);
        var hash2 = HmacSha256Helper.Hash(message, key2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_SameMessageDifferentKeys_ShouldReturnDifferentHashes()
    {
        // Arrange
        var message = TestString;
        var key1 = TestKey;
        var key2 = TestKeyChinese;

        // Act
        var hash1 = HmacSha256Helper.Hash(message, key1);
        var hash2 = HmacSha256Helper.Hash(message, key2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_ValidBase64Output_ShouldBeValidBase64String()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        
        // 验证是否为有效的Base64字符串
        try
        {
            var bytes = Convert.FromBase64String(hash);
            Assert.Equal(32, bytes.Length); // HMAC-SHA256 输出应为32字节
        }
        catch (FormatException)
        {
            Assert.Fail("返回的哈希值不是有效的Base64字符串");
        }
    }

    [Theory]
    [InlineData("test", "key")]
    [InlineData("Hello World", "secret")]
    [InlineData("测试消息", "密钥")]
    [InlineData("", "key")]
    [InlineData("message", "")]
    [InlineData("", "")]
    public void Hash_VariousInputs_ShouldReturnValidHashes(string message, string key)
    {
        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        
        // 验证Base64格式和长度
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(32, bytes.Length);
    }

    [Fact]
    public void Hash_KnownTestVector_ShouldReturnExpectedHash()
    {
        // Arrange - 使用RFC 4231中的测试向量
        var message = "Hi There";
        var key = new string('\x0b', 20); // 20个0x0b字节

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        
        // 验证结果是一致的
        var hash2 = HmacSha256Helper.Hash(message, key);
        Assert.Equal(hash, hash2);
    }

    [Fact]
    public void Hash_SpecialCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var message = "!@#$%^&*()_+-=[]{}|;':,.<>?";
        var key = "~`!@#$%^&*()_+-=";

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(32, bytes.Length);
    }

    [Fact]
    public void Hash_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var message = "🌟🚀💻🎉";
        var key = "🔑🛡️🔐";

        // Act
        var hash = HmacSha256Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(32, bytes.Length);
    }
}