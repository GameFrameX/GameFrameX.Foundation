using System.Text;
using Xunit;
using GameFrameX.Foundation.Encryption;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// DSA 加密解密单元测试
/// </summary>
public class DsaHelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string LongTestData = "这是一个很长的测试字符串，用来测试DSA签名算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";
    private const string EmptyString = "";

    [Fact]
    public void Make_ShouldGenerateValidKeyPair()
    {
        // Act
        var keyPair = DsaHelper.Make();

        // Assert
        Assert.NotNull(keyPair);
        Assert.True(keyPair.ContainsKey("privatekey"));
        Assert.True(keyPair.ContainsKey("publickey"));
        Assert.NotNull(keyPair["privatekey"]);
        Assert.NotNull(keyPair["publickey"]);
        Assert.NotEmpty(keyPair["privatekey"]);
        Assert.NotEmpty(keyPair["publickey"]);
        Assert.NotEqual(keyPair["privatekey"], keyPair["publickey"]);
    }

    [Fact]
    public void SignData_WithValidPrivateKey_ShouldReturnSignature()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var signature = DsaHelper.SignData(dataBytes, privateKey);

        // Assert
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
    }

    [Fact]
    public void SignData_WithStringData_ShouldReturnBase64Signature()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];

        // Act
        var signature = DsaHelper.SignData(TestData, privateKey);

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
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var signature = DsaHelper.SignData(dataBytes, privateKey);

        // Act
        var isValid = DsaHelper.VerifyData(dataBytes, signature, privateKey);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyData_WithStringData_ShouldReturnTrue()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var signature = DsaHelper.SignData(TestData, privateKey);

        // Act
        var isValid = DsaHelper.VerifyData(TestData, signature, privateKey);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyData_WithInvalidSignature_ShouldReturnFalse()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var validSignature = DsaHelper.SignData(dataBytes, privateKey);
        var invalidSignature = new byte[validSignature.Length];
        // 创建一个无效的签名
        for (int i = 0; i < invalidSignature.Length; i++)
        {
            invalidSignature[i] = (byte)(validSignature[i] ^ 0xFF);
        }

        // Act
        var isValid = DsaHelper.VerifyData(dataBytes, invalidSignature, privateKey);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void VerifyData_WithModifiedData_ShouldReturnFalse()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var originalData = TestData;
        var modifiedData = TestData + "modified";
        var signature = DsaHelper.SignData(originalData, privateKey);

        // Act
        var isValid = DsaHelper.VerifyData(modifiedData, signature, privateKey);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void SignData_WithLongData_ShouldReturnValidSignature()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];

        // Act
        var signature = DsaHelper.SignData(LongTestData, privateKey);
        var isValid = DsaHelper.VerifyData(LongTestData, signature, privateKey);

        // Assert
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
        var signature = DsaHelper.SignData(dataBytes, invalidPrivateKey);

        // Assert
        Assert.Null(signature);
    }

    [Fact]
    public void VerifyData_WithInvalidPrivateKey_ShouldReturnFalse()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var signature = DsaHelper.SignData(dataBytes, privateKey);
        var invalidPrivateKey = "invalid_key";

        // Act
        var isValid = DsaHelper.VerifyData(dataBytes, signature, invalidPrivateKey);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Constructor_WithValidKey_ShouldCreateInstance()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];

        // Act
        var dsaHelper = new DsaHelper(privateKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var signature = dsaHelper.SignData(dataBytes);

        // Assert
        Assert.NotNull(dsaHelper);
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
    }

    [Fact]
    public void InstanceMethods_ShouldWorkCorrectly()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];
        var dsaHelper = new DsaHelper(privateKey);

        // Act
        var signature = dsaHelper.SignData(TestData);
        var isValid = dsaHelper.VerifyData(TestData, signature);

        // Assert
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        Assert.True(isValid);
    }

    [Fact]
    public void SignData_WithEmptyString_ShouldReturnValidSignature()
    {
        // Arrange
        var keyPair = DsaHelper.Make();
        var privateKey = keyPair["privatekey"];

        // Act
        var signature = DsaHelper.SignData(EmptyString, privateKey);
        var isValid = DsaHelper.VerifyData(EmptyString, signature, privateKey);

        // Assert
        Assert.NotNull(signature);
        Assert.NotEmpty(signature);
        Assert.True(isValid);
    }
}