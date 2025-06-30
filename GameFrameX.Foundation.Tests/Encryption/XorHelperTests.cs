using System.Text;
using Xunit;
using GameFrameX.Foundation.Encryption;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// XOR 加密解密单元测试
/// </summary>
public class XorHelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string ShortTestData = "Test";
    private const string LongTestData = "这是一个很长的测试字符串，用来测试XOR加密算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。XOR是一种简单但有效的加密方法。";
    private const string EmptyString = "";
    private const string TestKey = "MySecretKey123";
    private const string ShortKey = "Key";
    private const string LongKey = "ThisIsAVeryLongSecretKeyForTesting123456789";
    private const string SingleCharKey = "K";

    [Fact]
    public void GetXorBytes_WithValidKey_ShouldReturnEncryptedBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);

        // Assert
        Assert.NotNull(encryptedBytes);
        Assert.NotEmpty(encryptedBytes);
        Assert.NotEqual(dataBytes, encryptedBytes);
    }

    [Fact]
    public void GetXorBytes_TwiceWithSameKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_Roundtrip_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithLongData_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(LongTestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithEmptyData_ShouldReturnEmptyArray()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(EmptyString);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithShortKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(ShortKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithLongKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(LongKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithSingleCharKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(SingleCharKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetQuickXorBytes_WithValidKey_ShouldReturnEncryptedBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        // Act
        var encryptedBytes = XorHelper.GetQuickXorBytes(dataBytes, keyBytes);

        // Assert
        Assert.NotNull(encryptedBytes);
        Assert.NotEmpty(encryptedBytes);
        Assert.Equal(dataBytes.Length, encryptedBytes.Length);
        Assert.NotEqual(dataBytes, encryptedBytes);
    }

    [Fact]
    public void GetQuickXorBytes_TwiceWithSameKey_ShouldReturnOriginalBytes()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        // Act
        var encryptedBytes = XorHelper.GetQuickXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetQuickXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetSelfXorBytes_ShouldModifyOriginalArray()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var originalBytes = Encoding.UTF8.GetBytes(TestData);
        var dataBytes = new byte[originalBytes.Length];
        Array.Copy(originalBytes, dataBytes, originalBytes.Length);

        // Act
        XorHelper.GetSelfXorBytes(dataBytes, keyBytes);
        XorHelper.GetSelfXorBytes(dataBytes, keyBytes); // XOR twice to get original

        // Assert
        Assert.Equal(originalBytes, dataBytes);
    }

    [Fact]
    public void GetXorBytes_WithDifferentKeys_ShouldProduceDifferentResults()
    {
        // Arrange
        var key1Bytes = Encoding.UTF8.GetBytes("Key1");
        var key2Bytes = Encoding.UTF8.GetBytes("Key2");
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encrypted1 = XorHelper.GetXorBytes(dataBytes, key1Bytes);
        var encrypted2 = XorHelper.GetXorBytes(dataBytes, key2Bytes);

        // Assert
        Assert.NotEqual(encrypted1, encrypted2);
    }

    [Fact]
    public void GetXorBytes_SameDataAndKey_ShouldProduceSameResult()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encrypted1 = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var encrypted2 = XorHelper.GetXorBytes(dataBytes, keyBytes);

        // Assert
        // XOR加密是确定性的，相同的密钥和数据应该产生相同的结果
        Assert.Equal(encrypted1, encrypted2);
    }

    [Fact]
    public void GetXorBytes_WithNullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XorHelper.GetXorBytes(dataBytes, null));
    }

    [Fact]
    public void GetXorBytes_WithNullData_ShouldReturnNull()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        // Act
        var result = XorHelper.GetXorBytes(null, keyBytes);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetSelfXorBytes_WithNullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XorHelper.GetSelfXorBytes(dataBytes, null));
    }

    [Fact]
    public void GetSelfXorBytes_WithNullData_ShouldNotThrow()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        // Act & Assert
        // Should not throw, method should handle null gracefully
        XorHelper.GetSelfXorBytes(null, keyBytes);
    }

    [Fact]
    public void GetXorBytes_WithEmptyKey_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyKeyBytes = new byte[0];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => XorHelper.GetXorBytes(dataBytes, emptyKeyBytes));
    }

    [Fact]
    public void GetSelfXorBytes_WithEmptyKey_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyKeyBytes = new byte[0];
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => XorHelper.GetSelfXorBytes(dataBytes, emptyKeyBytes));
    }

    [Fact]
    public void XorOperation_IsReversible()
    {
        // Arrange
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes); // XOR两次应该得到原始数据

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithUnicodeCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var unicodeData = "Hello 世界 🌍 Ñoël";
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(unicodeData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(unicodeData, Encoding.UTF8.GetString(decryptedBytes));
    }

    [Fact]
    public void GetXorBytes_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var specialData = "!@#$%^&*()_+-=[]{}|;':,.<>?/~`";
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(specialData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(specialData, Encoding.UTF8.GetString(decryptedBytes));
    }

    [Fact]
    public void GetXorBytes_KeyLongerThanData_ShouldWork()
    {
        // Arrange
        var shortData = "Hi";
        var longKey = "ThisIsAVeryLongKeyThatIsLongerThanTheData";
        var keyBytes = Encoding.UTF8.GetBytes(longKey);
        var dataBytes = Encoding.UTF8.GetBytes(shortData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(shortData, Encoding.UTF8.GetString(decryptedBytes));
    }

    [Fact]
    public void GetXorBytes_DataLongerThanKey_ShouldWork()
    {
        // Arrange
        var longData = "This is a very long piece of data that is much longer than the key";
        var shortKey = "Key";
        var keyBytes = Encoding.UTF8.GetBytes(shortKey);
        var dataBytes = Encoding.UTF8.GetBytes(longData);

        // Act
        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        // Assert
        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(longData, Encoding.UTF8.GetString(decryptedBytes));
    }
}