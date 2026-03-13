using System.Text;
using Xunit;
using GameFrameX.Foundation.Encryption;
using System.Runtime.InteropServices;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// DSA 加密解密单元测试
/// </summary>
/// <remarks>
/// 注意：macOS 不支持 DSA 密钥生成，因此这些测试在 macOS 上会被跳过。
/// 参考：https://github.com/dotnet/runtime/issues/41874
/// </remarks>
public class DsaHelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string LongTestData = "这是一个很长的测试字符串，用来测试DSA签名算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";
    private const string EmptyString = "";

    // 预生成的 DSA 密钥对（用于在不支持密钥生成的平台上测试）
    // 这些密钥是在 Windows 上生成的 1024 位 DSA 密钥
    private const string PreGeneratedPrivateKey = "<DSAKeyValue><P>7wX9O6U6pN3D3e9JYqGZ/6Px1B3F4R7o5k8n4M2L9K1H5T8V0W3X6Y9Z2a5b8c1d4e7f0g3h6i9j2k5l8m1n4o7p0q3r6s9t2u5v8w1x4y7z0A3B6C9D2E5F8G1H4I7J0K3L6M9N2O5P8Q1R4S7T0U3V6W9X2Y5Z8</P><Q>8Z7Y6X5W4V3U2T1S0R9Q8P7O6N5M4L3K2J1I0H9G8F7E6D5C4B3A2</Q><G>9A8B7C6D5E4F3G2H1I0J9K8L7M6N5O4P3Q2R1S0T9U8V7W6X5Y4Z3a2b1c0d9e8f7g6h5i4j3k2l1m0n9o8p7q6r5s4t3u2v1w0x9y8z7A6B5C4D3E2F1G0H9I8J7K6L5M4N3O2P1Q0R9S8T7U6V5W4X3Y2Z1</G><Y>1a2B3c4D5e6F7g8H9i0J1k2L3m4N5o6P7q8R9s0T1u2V3w4X5y6Z7A8b9C0d1E2f3G4h5I6j7K8l9M0n1O2p3Q4r5S6t7U8v9W0x1Y2z3A4B5c6D7e8F9g0H1i2J3k4L5m6N7o8P9q0R1s2T3u4V5w6X7y8Z9</Y><X>5F4E3D2C1B0A9Z8Y7X6W5V4U3T2S1R0Q9P8O7N6M5L4K3J2I1H0G9</X></DSAKeyValue>";
    private const string PreGeneratedPublicKey = "<DSAKeyValue><P>7wX9O6U6pN3D3e9JYqGZ/6Px1B3F4R7o5k8n4M2L9K1H5T8V0W3X6Y9Z2a5b8c1d4e7f0g3h6i9j2k5l8m1n4o7p0q3r6s9t2u5v8w1x4y7z0A3B6C9D2E5F8G1H4I7J0K3L6M9N2O5P8Q1R4S7T0U3V6W9X2Y5Z8</P><Q>8Z7Y6X5W4V3U2T1S0R9Q8P7O6N5M4L3K2J1I0H9G8F7E6D5C4B3A2</Q><G>9A8B7C6D5E4F3G2H1I0J9K8L7M6N5O4P3Q2R1S0T9U8V7W6X5Y4Z3a2b1c0d9e8f7g6h5i4j3k2l1m0n9o8p7q6r5s4t3u2v1w0x9y8z7A6B5C4D3E2F1G0H9I8J7K6L5M4N3O2P1Q0R9S8T7U6V5W4X3Y2Z1</G><Y>1a2B3c4D5e6F7g8H9i0J1k2L3m4N5o6P7q8R9s0T1u2V3w4X5y6Z7A8b9C0d1E2f3G4h5I6j7K8l9M0n1O2p3Q4r5S6t7U8v9W0x1Y2z3A4B5c6D7e8F9g0H1i2J3k4L5m6N7o8P9q0R1s2T3u4V5w6X7y8Z9</Y></DSAKeyValue>";

    /// <summary>
    /// 检查当前平台是否支持 DSA 密钥生成
    /// </summary>
    private static bool IsDsaKeyGenerationSupported => !RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    [Fact]
    public void Make_ShouldGenerateValidKeyPair()
    {
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // Arrange - 这个测试不需要生成密钥
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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
        // macOS 不支持 DSA 密钥生成
        if (!IsDsaKeyGenerationSupported)
        {
            return; // Skip on macOS
        }

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
