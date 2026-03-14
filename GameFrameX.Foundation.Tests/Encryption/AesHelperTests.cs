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
using System;
using Xunit;
using GameFrameX.Foundation.Encryption;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// AES 加密解密单元测试
/// </summary>
public class AesHelperTests
{
    private const string TestKey = "MySecretKey123456";
    private const string TestPlainText = "Hello, World! 这是一个测试字符串。";
    private const string EmptyString = "";
    private const string LongText = "这是一个很长的测试字符串，用来测试AES加密算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";

    [Fact]
    public void Encrypt_ValidInput_ShouldReturnBase64String()
    {
        // Arrange
        var plainText = TestPlainText;
        var key = TestKey;

        // Act
        var encryptedText = AesHelper.Encrypt(plainText, key);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(plainText, encryptedText);
        
        // 验证是否为有效的Base64字符串
        try
        {
            Convert.FromBase64String(encryptedText);
        }
        catch (FormatException)
        {
            Assert.Fail("加密结果不是有效的Base64字符串");
        }
    }

    [Fact]
    public void Decrypt_ValidInput_ShouldReturnOriginalText()
    {
        // Arrange
        var plainText = TestPlainText;
        var key = TestKey;
        var encryptedText = AesHelper.Encrypt(plainText, key);

        // Act
        var decryptedText = AesHelper.Decrypt(encryptedText, key);

        // Assert
        Assert.Equal(plainText, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_Roundtrip_ShouldReturnOriginalText()
    {
        // Arrange
        var plainText = TestPlainText;
        var key = TestKey;

        // Act
        var encryptedText = AesHelper.Encrypt(plainText, key);
        var decryptedText = AesHelper.Decrypt(encryptedText, key);

        // Assert
        Assert.Equal(plainText, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_LongText_ShouldReturnOriginalText()
    {
        // Arrange
        var plainText = LongText;
        var key = TestKey;

        // Act
        var encryptedText = AesHelper.Encrypt(plainText, key);
        var decryptedText = AesHelper.Decrypt(encryptedText, key);

        // Assert
        Assert.Equal(plainText, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_EmptyString_ShouldThrowException()
    {
        // Arrange
        var plainText = EmptyString;
        var key = TestKey;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AesHelper.Encrypt(plainText, key));
    }

    [Fact]
    public void Encrypt_EmptyKey_ShouldThrowException()
    {
        // Arrange
        var plainText = TestPlainText;
        var key = EmptyString;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AesHelper.Encrypt(plainText, key));
    }

    [Fact]
    public void Encrypt_NullKey_ShouldThrowException()
    {
        // Arrange
        var plainText = TestPlainText;
        string key = null!;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AesHelper.Encrypt(plainText, key!));
    }

    [Fact]
    public void EncryptBytes_ValidInput_ShouldReturnEncryptedBytes()
    {
        // Arrange
        var plainBytes = Encoding.UTF8.GetBytes(TestPlainText);
        var key = TestKey;

        // Act
        var encryptedBytes = AesHelper.Encrypt(plainBytes, key);

        // Assert
        Assert.NotNull(encryptedBytes);
        Assert.NotEmpty(encryptedBytes);
        Assert.NotEqual(plainBytes, encryptedBytes);
    }

    [Fact]
    public void DecryptBytes_ValidInput_ShouldReturnOriginalBytes()
    {
        // Arrange
        var plainBytes = Encoding.UTF8.GetBytes(TestPlainText);
        var key = TestKey;
        var encryptedBytes = AesHelper.Encrypt(plainBytes, key);

        // Act
        var decryptedBytes = AesHelper.Decrypt(encryptedBytes, key);

        // Assert
        Assert.Equal(plainBytes, decryptedBytes);
    }

    [Fact]
    public void EncryptDecryptBytes_Roundtrip_ShouldReturnOriginalBytes()
    {
        // Arrange
        var plainBytes = Encoding.UTF8.GetBytes(TestPlainText);
        var key = TestKey;

        // Act
        var encryptedBytes = AesHelper.Encrypt(plainBytes, key);
        var decryptedBytes = AesHelper.Decrypt(encryptedBytes, key);

        // Assert
        Assert.Equal(plainBytes, decryptedBytes);
    }

    [Fact]
    public void Encrypt_SameInputTwice_ShouldReturnDifferentResults_BothDecryptable()
    {
        // Arrange
        var plainText = TestPlainText;
        var key = TestKey;

        // Act
        var encryptedText1 = AesHelper.Encrypt(plainText, key);
        var encryptedText2 = AesHelper.Encrypt(plainText, key);

        // Assert
        // 由于每次加密都随机生成 Salt 和 IV，相同输入应该产生不同的加密结果（语义安全）
        Assert.NotEqual(encryptedText1, encryptedText2);
        // 但两个加密结果都应该能正确解密
        Assert.Equal(plainText, AesHelper.Decrypt(encryptedText1, key));
        Assert.Equal(plainText, AesHelper.Decrypt(encryptedText2, key));
    }

    [Fact]
    public void Encrypt_DifferentKeys_ShouldReturnDifferentResults()
    {
        // Arrange
        var plainText = TestPlainText;
        var key1 = "Key1";
        var key2 = "Key2";

        // Act
        var encryptedText1 = AesHelper.Encrypt(plainText, key1);
        var encryptedText2 = AesHelper.Encrypt(plainText, key2);

        // Assert
        Assert.NotEqual(encryptedText1, encryptedText2);
    }

    [Theory]
    [InlineData("简单文本")]
    [InlineData("Simple English Text")]
    [InlineData("Mixed 混合 Text 文本 123")]
    [InlineData("Special chars: !@#$%^&*()_+-=[]{}|;':,.<>?")]
    [InlineData("Unicode: 🚀🌟💻🎮🔥")]
    public void EncryptDecrypt_VariousTexts_ShouldReturnOriginalText(string plainText)
    {
        // Arrange
        var key = TestKey;

        // Act
        var encryptedText = AesHelper.Encrypt(plainText, key);
        var decryptedText = AesHelper.Decrypt(encryptedText, key);

        // Assert
        Assert.Equal(plainText, decryptedText);
    }

    [Theory]
    [InlineData("ShortKey")]
    [InlineData("MediumLengthKey123")]
    [InlineData("VeryLongKeyThatExceedsNormalLength123456789")]
    public void EncryptDecrypt_VariousKeyLengths_ShouldWork(string key)
    {
        // Arrange
        var plainText = TestPlainText;

        // Act
        var encryptedText = AesHelper.Encrypt(plainText, key);
        var decryptedText = AesHelper.Decrypt(encryptedText, key);

        // Assert
        Assert.Equal(plainText, decryptedText);
    }

    /// <summary>
    /// 性能测试：测试大量数据的加密解密性能
    /// </summary>
    [Fact]
    public void EncryptDecrypt_LargeData_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeText = new string('A', 10000); // 10KB 数据
        var key = TestKey;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var encryptedText = AesHelper.Encrypt(largeText, key);
        var decryptedText = AesHelper.Decrypt(encryptedText, key);
        stopwatch.Stop();

        // Assert
        Assert.Equal(largeText, decryptedText);
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, $"加密解密耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }
}