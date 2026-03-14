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
/// RSA 加密解密单元测试
/// </summary>
public class RsaHelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string ShortTestData = "Test";
    private const string EmptyString = "";

    [Fact]
    public void Make_ShouldGenerateValidKeyPair()
    {
        // Act
        var keyPair = RsaHelper.Make();

        // Assert
        Assert.NotNull(keyPair);
        Assert.True(keyPair.ContainsKey("privateKey"));
        Assert.True(keyPair.ContainsKey("publicKey"));
        Assert.NotNull(keyPair["privateKey"]);
        Assert.NotNull(keyPair["publicKey"]);
        Assert.NotEmpty(keyPair["privateKey"]);
        Assert.NotEmpty(keyPair["publicKey"]);
        Assert.NotEqual(keyPair["privateKey"], keyPair["publicKey"]);
    }

    [Fact]
    public void Encrypt_WithPublicKey_ShouldReturnBase64String()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];

        // Act
        var encryptedText = RsaHelper.Encrypt(publicKey, ShortTestData);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
        Assert.NotEqual(ShortTestData, encryptedText);
        
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
    public void Decrypt_WithPrivateKey_ShouldReturnOriginalText()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];
        var encryptedText = RsaHelper.Encrypt(publicKey, ShortTestData);

        // Act
        var decryptedText = RsaHelper.Decrypt(privateKey, encryptedText);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
    }

    [Fact]
    public void EncryptDecrypt_Roundtrip_ShouldReturnOriginalText()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];

        // Act
        var encryptedText = RsaHelper.Encrypt(publicKey, ShortTestData);
        var decryptedText = RsaHelper.Decrypt(privateKey, encryptedText);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
    }

    [Fact]
    public void EncryptBytes_WithPublicKey_ShouldReturnEncryptedBytes()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        // Act
        var encryptedBytes = RsaHelper.Encrypt(publicKey, dataBytes);

        // Assert
        Assert.NotNull(encryptedBytes);
        Assert.NotEmpty(encryptedBytes);
        Assert.NotEqual(dataBytes, encryptedBytes);
    }

    [Fact]
    public void DecryptBytes_WithPrivateKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);
        var encryptedBytes = RsaHelper.Encrypt(publicKey, dataBytes);

        // Act
        var decryptedBytes = RsaHelper.Decrypt(privateKey, encryptedBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void SignData_WithPrivateKey_ShouldReturnSignature()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var privateKey = keyPair["privateKey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var signature = RsaHelper.SignData(dataBytes, privateKey);

        // Assert
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
    }

    [Fact]
    public void SignData_WithStringData_ShouldReturnBase64Signature()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var privateKey = keyPair["privateKey"];

        // Act
        var signature = RsaHelper.SignData(TestData, privateKey);

        // Assert
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        
        // 验证是否为有效的Base64字符串
        try
        {
            Convert.FromBase64String(signature);
        }
        catch (FormatException)
        {
            Assert.Fail("签名结果不是有效的Base64字符串");
        }
    }

    [Fact]
    public void VerifyData_WithValidSignature_ShouldReturnTrue()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var signature = RsaHelper.SignData(dataBytes, privateKey);

        // Act
        var isValid = RsaHelper.VerifyData(dataBytes, signature, publicKey);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void RsaVerifyData_WithStringData_ShouldReturnTrue()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];
        var signature = RsaHelper.SignData(TestData, privateKey);

        // Act
        var isValid = RsaHelper.RsaVerifyData(TestData, signature, publicKey);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyData_WithInvalidSignature_ShouldReturnFalse()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var validSignature = RsaHelper.SignData(dataBytes, privateKey);
        var invalidSignature = new byte[validSignature.Length];
        // 创建一个无效的签名
        for (int i = 0; i < invalidSignature.Length; i++)
        {
            invalidSignature[i] = (byte)(validSignature[i] ^ 0xFF);
        }

        // Act
        var isValid = RsaHelper.VerifyData(dataBytes, invalidSignature, publicKey);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void VerifyData_WithModifiedData_ShouldReturnFalse()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var publicKey = keyPair["publicKey"];
        var privateKey = keyPair["privateKey"];
        var originalData = TestData;
        var modifiedData = TestData + "modified";
        var signature = RsaHelper.SignData(originalData, privateKey);

        // Act
        var isValid = RsaHelper.RsaVerifyData(modifiedData, signature, publicKey);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Constructor_WithValidKey_ShouldCreateInstance()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var privateKey = keyPair["privateKey"];

        // Act
        var rsaHelper = new RsaHelper(privateKey);
        var encryptedText = rsaHelper.Encrypt(ShortTestData);

        // Assert
        Assert.NotNull(rsaHelper);
        Assert.NotNull(encryptedText);
        Assert.NotEmpty(encryptedText);
    }

    [Fact]
    public void InstanceMethods_ShouldWorkCorrectly()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var privateKey = keyPair["privateKey"];
        var rsaHelper = new RsaHelper(privateKey);

        // Act
        var encryptedText = rsaHelper.Encrypt(ShortTestData);
        var decryptedText = rsaHelper.Decrypt(encryptedText);
        var signature = rsaHelper.SignData(TestData);
        var isValid = rsaHelper.VerifyData(TestData, signature);

        // Assert
        Assert.Equal(ShortTestData, decryptedText);
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        Assert.True(isValid);
    }

    [Fact]
    public void SignData_WithInvalidPrivateKey_ShouldReturnNull()
    {
        // Arrange
        var invalidPrivateKey = "invalid_key";
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var signature = RsaHelper.SignData(dataBytes, invalidPrivateKey);

        // Assert
        Assert.Null(signature);
    }

    [Fact]
    public void VerifyData_WithInvalidPublicKey_ShouldReturnFalse()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var privateKey = keyPair["privateKey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var signature = RsaHelper.SignData(dataBytes, privateKey);
        var invalidPublicKey = "invalid_key";

        // Act
        var isValid = RsaHelper.VerifyData(dataBytes, signature, invalidPublicKey);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void SignData_WithEmptyString_ShouldReturnValidSignature()
    {
        // Arrange
        var keyPair = RsaHelper.Make();
        var privateKey = keyPair["privateKey"];
        var publicKey = keyPair["publicKey"];

        // Act
        var signature = RsaHelper.SignData(EmptyString, privateKey);
        var isValid = RsaHelper.RsaVerifyData(EmptyString, signature, publicKey);

        // Assert
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        Assert.True(isValid);
    }
}