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
using Xunit;
using GameFrameX.Foundation.Encryption;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// SM4 加密解密单元测试
/// </summary>
public class Sm4HelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string ShortTestData = "Test";
    private const string LongTestData = "这是一个很长的测试字符串，用来测试SM4加密算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。这个字符串足够长，可以测试分块加密的情况。";
    private const string EmptyString = "";
    private const string TestKey = "0123456789ABCDEF0123456789ABCDEF"; // 32字符的十六进制密钥
    private const string TestIv = "0123456789ABCDEF0123456789ABCDEF"; // 32字符的十六进制IV

    [Fact]
    public void EncryptCbc_WithValidKey_ShouldReturnEncryptedString()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, ShortTestData, hexString: true);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(ShortTestData, encryptedText);
    }

    [Fact]
    public void DecryptCbc_WithValidKey_ShouldReturnOriginalText()
    {
        // Arrange
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, ShortTestData, hexString: true);

        // Act
        var decryptedText = Sm4Helper.DecryptCbc(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
    }

    [Fact]
    public void EncryptEcb_WithValidKey_ShouldReturnEncryptedString()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptEcb(TestKey, ShortTestData, hexString: true);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(ShortTestData, encryptedText);
    }

    [Fact]
    public void DecryptEcb_WithValidKey_ShouldReturnOriginalText()
    {
        // Arrange
        var encryptedText = Sm4Helper.EncryptEcb(TestKey, ShortTestData, hexString: true);

        // Act
        var decryptedText = Sm4Helper.DecryptEcb(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecryptCbc_Roundtrip_ShouldReturnOriginalText()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, TestData, hexString: true);
        var decryptedText = Sm4Helper.DecryptCbc(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(TestData, decryptedText);
    }

    [Fact]
    public void EncryptDecryptEcb_Roundtrip_ShouldReturnOriginalText()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptEcb(TestKey, TestData, hexString: true);
        var decryptedText = Sm4Helper.DecryptEcb(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(TestData, decryptedText);
    }

    [Fact]
    public void EncryptDecryptCbc_WithLongText_ShouldReturnOriginalText()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, LongTestData, hexString: true);
        var decryptedText = Sm4Helper.DecryptCbc(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(LongTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecryptEcb_WithLongText_ShouldReturnOriginalText()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptEcb(TestKey, LongTestData, hexString: true);
        var decryptedText = Sm4Helper.DecryptEcb(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(LongTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecryptCbc_WithEmptyString_ShouldReturnEmptyString()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, EmptyString, hexString: true);
        var decryptedText = Sm4Helper.DecryptCbc(TestKey, encryptedText, hexString: true);

        // Assert
        Assert.Equal(EmptyString, decryptedText);
    }

    [Fact]
    public void EncryptCbcWithIv_WithValidKeyAndIv_ShouldReturnEncryptedString()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, ShortTestData, TestIv, hexString: true);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(ShortTestData, encryptedText);
    }

    [Fact]
    public void DecryptCbcWithIv_WithValidKeyAndIv_ShouldReturnOriginalText()
    {
        // Arrange
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, ShortTestData, TestIv, hexString: true);

        // Act
        var decryptedText = Sm4Helper.DecryptCbc(TestKey, encryptedText, TestIv, hexString: true);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecryptCbcWithIv_Roundtrip_ShouldReturnOriginalText()
    {
        // Act
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, TestData, TestIv, hexString: true);
        var decryptedText = Sm4Helper.DecryptCbc(TestKey, encryptedText, TestIv, hexString: true);

        // Assert
        Assert.Equal(TestData, decryptedText);
    }





    [Fact]
    public void EncryptCbc_WithInvalidKey_ShouldThrowException()
    {
        // Arrange
        var invalidKey = "invalid_key";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.EncryptCbc(invalidKey, TestData, hexString: false));
    }

    [Fact]
    public void DecryptCbc_WithInvalidKey_ShouldThrowException()
    {
        // Arrange
        var encryptedText = Sm4Helper.EncryptCbc(TestKey, TestData, hexString: true);
        var invalidKey = "invalid_key";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.DecryptCbc(invalidKey, encryptedText, hexString: false));
    }

    [Fact]
    public void DecryptCbc_WithInvalidEncryptedData_ShouldThrowException()
    {
        // Arrange
        var invalidEncryptedData = "invalid_encrypted_data";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => Sm4Helper.DecryptCbc(TestKey, invalidEncryptedData, hexString: true));
    }

    [Fact]
    public void EncryptCbc_WithDifferentIvs_ShouldProduceDifferentResults()
    {
        // Arrange
        var iv1 = "0123456789ABCDEF0123456789ABCDEF";
        var iv2 = "FEDCBA9876543210FEDCBA9876543210";

        // Act
        var encrypted1 = Sm4Helper.EncryptCbc(TestKey, TestData, iv1, hexString: true);
        var encrypted2 = Sm4Helper.EncryptCbc(TestKey, TestData, iv2, hexString: true);

        // Assert
        Assert.NotEqual(encrypted1, encrypted2);
        
        // 但使用相应的IV解密后应该都能得到原始数据
        var decrypted1 = Sm4Helper.DecryptCbc(TestKey, encrypted1, iv1, hexString: true);
        var decrypted2 = Sm4Helper.DecryptCbc(TestKey, encrypted2, iv2, hexString: true);
        Assert.Equal(TestData, decrypted1);
        Assert.Equal(TestData, decrypted2);
    }

    [Fact]
    public void EncryptCbc_WithNullKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.EncryptCbc(null, TestData, hexString: true));
    }

    [Fact]
    public void EncryptCbc_WithEmptyKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.EncryptCbc(string.Empty, TestData, hexString: true));
    }

    [Fact]
    public void DecryptCbc_WithNullKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.DecryptCbc(null, TestData, hexString: true));
    }

    [Fact]
    public void DecryptCbc_WithNullData_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.DecryptCbc(TestKey, null, hexString: true));
    }

    [Fact]
    public void EncryptEcb_WithNullKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.EncryptEcb(null, TestData, hexString: true));
    }

    [Fact]
    public void DecryptEcb_WithNullKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.DecryptEcb(null, TestData, hexString: true));
    }

    [Fact]
    public void EncryptEcb_WithNullData_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.EncryptEcb(TestKey, null, hexString: true));
    }

    [Fact]
    public void DecryptEcb_WithNullData_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm4Helper.DecryptEcb(TestKey, null, hexString: true));
    }

    [Fact]
    public void EncryptEcb_WithEmptyKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.EncryptEcb(string.Empty, TestData, hexString: true));
    }

    [Fact]
    public void DecryptEcb_WithEmptyKey_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.DecryptEcb(string.Empty, TestData, hexString: true));
    }

    [Fact]
    public void EncryptCbc_WithInvalidHexKeyLength_ShouldThrowException()
    {
        // Arrange
        var invalidHexKey = "0123456789ABCDEF"; // 16 chars instead of 32 for hex

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.EncryptCbc(invalidHexKey, TestData, hexString: true));
    }

    [Fact]
    public void EncryptEcb_WithInvalidNonHexKeyLength_ShouldThrowException()
    {
        // Arrange
        var invalidKey = "short"; // Less than 16 chars

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm4Helper.EncryptEcb(invalidKey, TestData, hexString: false));
    }

    /// <summary>
    /// 验证字符串是否为有效的十六进制字符串
    /// </summary>
    /// <param name="hex">要验证的字符串</param>
    /// <returns>如果是有效的十六进制字符串返回true，否则返回false</returns>
    private static bool IsValidHexString(string hex)
    {
        if (string.IsNullOrEmpty(hex))
        {
            return false;
        }

        return hex.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
    }
}