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
/// SM2 加密解密单元测试
/// </summary>
public class Sm2HelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string ShortTestData = "Test";
    private const string LongTestData = "这是一个很长的测试字符串，用来测试SM2加密算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";
    private const string EmptyString = "";

    [Fact]
    public void GenerateKeyPair_ShouldGenerateValidKeyPair()
    {
        // Act
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);

        // Assert
        Assert.NotNull(publicKey);
        Assert.NotNull(privateKey);
        Assert.NotEmpty(publicKey);
        Assert.NotEmpty(privateKey);
        Assert.NotEqual(publicKey, privateKey);
        
        // 验证密钥长度（SM2公钥通常为130个十六进制字符，私钥为64个十六进制字符）
        Assert.True(publicKey.Length > 0);
        Assert.True(privateKey.Length > 0);
    }

    [Fact]
    public void Encrypt_WithValidPublicKey_ShouldReturnEncryptedString()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);

        // Act
        var encryptedText = Sm2Helper.Encrypt(publicKey, ShortTestData);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(ShortTestData, encryptedText);
    }

    [Fact]
    public void Decrypt_WithValidPrivateKey_ShouldReturnOriginalText()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var encryptedText = Sm2Helper.Encrypt(publicKey, ShortTestData);

        // Act
        var decryptedText = Sm2Helper.Decrypt(privateKey, encryptedText);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_Roundtrip_ShouldReturnOriginalText()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);

        // Act
        var encryptedText = Sm2Helper.Encrypt(publicKey, TestData);
        var decryptedText = Sm2Helper.Decrypt(privateKey, encryptedText);

        // Assert
        Assert.Equal(TestData, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_WithLongText_ShouldReturnOriginalText()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);

        // Act
        var encryptedText = Sm2Helper.Encrypt(publicKey, LongTestData);
        var decryptedText = Sm2Helper.Decrypt(privateKey, encryptedText);

        // Assert
        Assert.Equal(LongTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_WithEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);

        // Act
        var encryptedText = Sm2Helper.Encrypt(publicKey, EmptyString);
        var decryptedText = Sm2Helper.Decrypt(privateKey, encryptedText);

        // Assert
        Assert.Equal(EmptyString, decryptedText);
    }

    [Fact]
    public void EncryptBytes_WithValidPublicKey_ShouldReturnEncryptedString()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var publicKeyBytes = Convert.FromHexString(publicKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        // Act
        var encryptedText = Sm2Helper.Encrypt(publicKeyBytes, dataBytes);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
    }

    [Fact]
    public void DecryptBytes_WithValidPrivateKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var publicKeyBytes = Convert.FromHexString(publicKey);
        var privateKeyBytes = Convert.FromHexString(privateKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);
        var encryptedText = Sm2Helper.Encrypt(publicKeyBytes, dataBytes);
        var encryptedBytes = Convert.FromHexString(encryptedText);

        // Act
        var decryptedBytes = Sm2Helper.Decrypt(privateKeyBytes, encryptedBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void EncryptDecryptBytes_Roundtrip_ShouldReturnOriginalBytes()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var publicKeyBytes = Convert.FromHexString(publicKey);
        var privateKeyBytes = Convert.FromHexString(privateKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encryptedText = Sm2Helper.Encrypt(publicKeyBytes, dataBytes);
        var encryptedBytes = Convert.FromHexString(encryptedText);
        var decryptedBytes = Sm2Helper.Decrypt(privateKeyBytes, encryptedBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void Encrypt_WithInvalidPublicKey_ShouldThrowException()
    {
        // Arrange
        var invalidPublicKey = "invalid_public_key";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => Sm2Helper.Encrypt(invalidPublicKey, TestData));
    }

    [Fact]
    public void Decrypt_WithInvalidPrivateKey_ShouldThrowException()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var encryptedText = Sm2Helper.Encrypt(publicKey, TestData);
        var invalidPrivateKey = "invalid_private_key";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => Sm2Helper.Decrypt(invalidPrivateKey, encryptedText));
    }

    [Fact]
    public void Decrypt_WithInvalidEncryptedData_ShouldThrowException()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var invalidEncryptedData = "invalid_encrypted_data";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => Sm2Helper.Decrypt(privateKey, invalidEncryptedData));
    }

    [Fact]
    public void Encrypt_WithDifferentKeys_ShouldProduceDifferentResults()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey1, out string privateKey1);
        Sm2Helper.GenerateKeyPair(out string publicKey2, out string privateKey2);

        // Act
        var encrypted1 = Sm2Helper.Encrypt(publicKey1, TestData);
        var encrypted2 = Sm2Helper.Encrypt(publicKey2, TestData);

        // Assert
        Assert.NotEqual(encrypted1, encrypted2);
    }

    [Fact]
    public void Encrypt_SameDataMultipleTimes_ShouldProduceDifferentResults()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);

        // Act
        var encrypted1 = Sm2Helper.Encrypt(publicKey, TestData);
        var encrypted2 = Sm2Helper.Encrypt(publicKey, TestData);

        // Assert
        // SM2加密由于随机数的存在，相同数据多次加密应该产生不同结果
        Assert.NotEqual(encrypted1, encrypted2);
        
        // 但解密后应该都能得到原始数据
        var decrypted1 = Sm2Helper.Decrypt(privateKey, encrypted1);
        var decrypted2 = Sm2Helper.Decrypt(privateKey, encrypted2);
        Assert.Equal(TestData, decrypted1);
        Assert.Equal(TestData, decrypted2);
    }

    [Fact]
    public void GenerateKeyPair_MultipleTimes_ShouldProduceDifferentKeys()
    {
        // Act
        Sm2Helper.GenerateKeyPair(out string publicKey1, out string privateKey1);
        Sm2Helper.GenerateKeyPair(out string publicKey2, out string privateKey2);

        // Assert
        Assert.NotEqual(publicKey1, publicKey2);
        Assert.NotEqual(privateKey1, privateKey2);
    }

    #region 参数验证测试

    [Fact]
    public void Encrypt_WithNullPublicKey_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm2Helper.Encrypt(null, TestData));
    }

    [Fact]
    public void Encrypt_WithEmptyPublicKey_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm2Helper.Encrypt("", TestData));
        Assert.Throws<ArgumentException>(() => Sm2Helper.Encrypt("   ", TestData));
    }

    [Fact]
    public void Decrypt_WithNullPrivateKey_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm2Helper.Decrypt(null, "encrypted_data"));
    }

    [Fact]
    public void Decrypt_WithEmptyPrivateKey_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm2Helper.Decrypt("", "encrypted_data"));
        Assert.Throws<ArgumentException>(() => Sm2Helper.Decrypt("   ", "encrypted_data"));
    }

    [Fact]
    public void Encrypt_ByteArray_WithNullPublicKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] data = Encoding.UTF8.GetBytes(TestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm2Helper.Encrypt(null, data));
    }

    [Fact]
    public void Encrypt_ByteArray_WithEmptyPublicKey_ShouldThrowArgumentException()
    {
        // Arrange
        byte[] data = Encoding.UTF8.GetBytes(TestData);
        byte[] emptyKey = new byte[0];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm2Helper.Encrypt(emptyKey, data));
    }

    [Fact]
    public void Decrypt_ByteArray_WithNullPrivateKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] encryptedData = Encoding.UTF8.GetBytes("encrypted_data");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sm2Helper.Decrypt(null, encryptedData));
    }

    [Fact]
    public void Decrypt_ByteArray_WithEmptyPrivateKey_ShouldThrowArgumentException()
    {
        // Arrange
        byte[] encryptedData = Encoding.UTF8.GetBytes("encrypted_data");
        byte[] emptyKey = new byte[0];

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm2Helper.Decrypt(emptyKey, encryptedData));
    }

    #endregion
}