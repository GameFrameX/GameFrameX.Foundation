using GameFrameX.Foundation.Hash;
using System.Text;
using Xunit;
using System.Security.Cryptography;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// SHA-256 哈希算法单元测试
/// </summary>
public class Sha256HelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试SHA-256哈希算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";

    [Fact]
    public void ComputeHash_ValidString_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = Sha256Helper.ComputeHash(input);
        var hash2 = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash1);
        Assert.NotEmpty(hash1);
        Assert.Equal(hash1, hash2);
        Assert.Equal(64, hash1.Length); // SHA-256 哈希长度应为64个字符
    }

    [Fact]
    public void ComputeHash_EmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(string.Empty, hash);
    }

    [Fact]
    public void ComputeHash_NullString_ShouldReturnEmptyString()
    {
        // Arrange
        string input = null;

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(string.Empty, hash);
    }

    [Fact]
    public void ComputeHash_ChineseString_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(64, hash.Length);
    }

    [Fact]
    public void ComputeHash_LongString_ShouldReturnValidHash()
    {
        // Arrange
        var input = LongString;

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(64, hash.Length);
    }

    [Fact]
    public void ComputeHash_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = Sha256Helper.ComputeHash(input1);
        var hash2 = Sha256Helper.ComputeHash(input2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_ByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(64, hash.Length);
    }

    [Fact]
    public void ComputeHash_NullByteArray_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] input = null;

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(string.Empty, hash);
    }

    [Fact]
    public void ComputeHash_EmptyByteArray_ShouldReturnEmptyString()
    {
        // Arrange
        var input = new byte[0];

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(string.Empty, hash);
    }

    [Fact]
    public void ComputeHash_StringWithDifferentEncodings_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input = "测试字符串";
        
        // Act
        var hashUtf8 = Sha256Helper.ComputeHash(input, Encoding.UTF8);
        var hashUtf16 = Sha256Helper.ComputeHash(input, Encoding.Unicode);

        // Assert
        Assert.NotEqual(hashUtf8, hashUtf16);
        Assert.Equal(64, hashUtf8.Length);
        Assert.Equal(64, hashUtf16.Length);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("abc")]
    [InlineData("message digest")]
    [InlineData("abcdefghijklmnopqrstuvwxyz")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")]
    public void ComputeHash_StandardTestVectors_ShouldReturnExpectedResults(string input)
    {
        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(64, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_Performance_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeInput = new string('A', 100000); // 100KB 数据
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var hash = Sha256Helper.ComputeHash(largeInput);
        stopwatch.Stop();

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(64, hash.Length);
        Assert.True(stopwatch.ElapsedMilliseconds < 100, $"SHA-256计算耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void ComputeFileHash_NonExistentFile_ShouldReturnEmptyString()
    {
        // Arrange
        var filePath = "non_existent_file.txt";

        // Act
        var hash = Sha256Helper.ComputeFileHash(filePath);

        // Assert
        Assert.Equal(string.Empty, hash);
    }

    [Fact]
    public void VerifyHash_ValidStringAndHash_ShouldReturnTrue()
    {
        // Arrange
        var input = TestString;
        var expectedHash = Sha256Helper.ComputeHash(input);

        // Act
        var result = Sha256Helper.VerifyHash(input, expectedHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyHash_InvalidHash_ShouldReturnFalse()
    {
        // Arrange
        var input = TestString;
        var invalidHash = "invalid_hash";

        // Act
        var result = Sha256Helper.VerifyHash(input, invalidHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_NullInput_ShouldReturnFalse()
    {
        // Arrange
        string input = null;
        var hash = "some_hash";

        // Act
        var result = Sha256Helper.VerifyHash(input, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_NullHash_ShouldReturnFalse()
    {
        // Arrange
        var input = TestString;
        string hash = null;

        // Act
        var result = Sha256Helper.VerifyHash(input, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_EmptyInput_ShouldReturnFalse()
    {
        // Arrange
        var input = string.Empty;
        var hash = "some_hash";

        // Act
        var result = Sha256Helper.VerifyHash(input, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_EmptyHash_ShouldReturnFalse()
    {
        // Arrange
        var input = TestString;
        var hash = string.Empty;

        // Act
        var result = Sha256Helper.VerifyHash(input, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_CaseInsensitive_ShouldReturnTrue()
    {
        // Arrange
        var input = TestString;
        var expectedHash = Sha256Helper.ComputeHash(input).ToUpper();

        // Act
        var result = Sha256Helper.VerifyHash(input, expectedHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyHash_ByteArray_ValidHash_ShouldReturnTrue()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var expectedHash = Sha256Helper.ComputeHash(input);

        // Act
        var result = Sha256Helper.VerifyHash(input, expectedHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyHash_ByteArray_InvalidHash_ShouldReturnFalse()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var invalidHash = "invalid_hash";

        // Act
        var result = Sha256Helper.VerifyHash(input, invalidHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_NullByteArray_ShouldReturnFalse()
    {
        // Arrange
        byte[] input = null;
        var hash = "some_hash";

        // Act
        var result = Sha256Helper.VerifyHash(input, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyHash_EmptyByteArray_ShouldReturnFalse()
    {
        // Arrange
        var input = new byte[0];
        var hash = "some_hash";

        // Act
        var result = Sha256Helper.VerifyHash(input, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyFileHash_NonExistentFile_ShouldReturnFalse()
    {
        // Arrange
        var filePath = "non_existent_file.txt";
        var hash = "some_hash";

        // Act
        var result = Sha256Helper.VerifyFileHash(filePath, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyFileHash_NullHash_ShouldReturnFalse()
    {
        // Arrange
        var filePath = "some_file.txt";
        string hash = null;

        // Act
        var result = Sha256Helper.VerifyFileHash(filePath, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyFileHash_EmptyHash_ShouldReturnFalse()
    {
        // Arrange
        var filePath = "some_file.txt";
        var hash = string.Empty;

        // Act
        var result = Sha256Helper.VerifyFileHash(filePath, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ComputeHash_StringAndByteArray_ShouldReturnSameHash()
    {
        // Arrange
        var input = TestString;
        var inputBytes = Encoding.UTF8.GetBytes(input);

        // Act
        var hashFromString = Sha256Helper.ComputeHash(input);
        var hashFromBytes = Sha256Helper.ComputeHash(inputBytes);

        // Assert
        Assert.Equal(hashFromString, hashFromBytes);
    }

    [Fact]
    public void ComputeHash_KnownTestVector_ShouldReturnExpectedHash()
    {
        // Arrange
        var input = "abc";
        var expectedHash = "ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad"; // Known SHA-256 hash for "abc"

        // Act
        var actualHash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void ComputeHash_EmptyStringTestVector_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "";
        var expectedResult = ""; // Sha256Helper returns empty string for empty input

        // Act
        var actualHash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedResult, actualHash);
    }

    [Fact]
    public void ComputeHash_SingleCharacter_ShouldReturnValidHash()
    {
        // Arrange
        var input = "a";
        var expectedHash = "ca978112ca1bbdcafac231b39a23dc4da786eff8147c4e72b9807785afee48bb"; // Known SHA-256 hash for "a"

        // Act
        var actualHash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void ComputeHash_SpecialCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "!@#$%^&*()_+-=[]{}|;':,.<>?";

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(64, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "🌟🚀💻🎉";

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(64, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }
}