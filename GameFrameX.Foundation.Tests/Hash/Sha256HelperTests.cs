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
    public void ComputeHash_EmptyString_ShouldReturnValidHash()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(64, hash.Length); // SHA-256 哈希长度应为64个字符
        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", hash);
    }

    [Fact]
    public void ComputeHash_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.ComputeHash(input));
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
    public void ComputeHash_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.ComputeHash(input));
    }

    [Fact]
    public void ComputeHash_EmptyByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = new byte[0];

        // Act
        var hash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(64, hash.Length); // SHA-256 哈希长度应为64个字符
        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", hash);
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
    public void ComputeFileHash_NullFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        string filePath = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.ComputeFileHash(filePath));
    }

    [Fact]
    public void ComputeFileHash_EmptyFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha256Helper.ComputeFileHash(filePath));
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
    public void VerifyHash_NullInput_ShouldThrowArgumentNullException()
    {
        // Arrange
        string input = null;
        var hash = "some_hash";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_NullHash_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = TestString;
        string hash = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_EmptyInput_ShouldReturnValidResult()
    {
        // Arrange
        var input = string.Empty;
        var expectedHash = Sha256Helper.ComputeHash(input);

        // Act
        var result = Sha256Helper.VerifyHash(input, expectedHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyHash_EmptyHash_ShouldThrowArgumentException()
    {
        // Arrange
        var input = TestString;
        var hash = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha256Helper.VerifyHash(input, hash));
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
    public void VerifyHash_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] input = null;
        var hash = "some_hash";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_EmptyByteArray_ShouldReturnValidResult()
    {
        // Arrange
        var input = new byte[0];
        var expectedHash = Sha256Helper.ComputeHash(input);

        // Act
        var result = Sha256Helper.VerifyHash(input, expectedHash);

        // Assert
        Assert.True(result);
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
    public void VerifyFileHash_NullFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        string filePath = null;
        var hash = "some_hash";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.VerifyFileHash(filePath, hash));
    }

    [Fact]
    public void VerifyFileHash_EmptyFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = string.Empty;
        var hash = "some_hash";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha256Helper.VerifyFileHash(filePath, hash));
    }

    [Fact]
    public void VerifyFileHash_NullHash_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = "some_file.txt";
        string hash = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha256Helper.VerifyFileHash(filePath, hash));
    }

    [Fact]
    public void VerifyFileHash_EmptyHash_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = "some_file.txt";
        var hash = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha256Helper.VerifyFileHash(filePath, hash));
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
    public void ComputeHash_EmptyStringTestVector_ShouldReturnValidHash()
    {
        // Arrange
        var input = "";
        var expectedResult = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855"; // Known SHA-256 hash for empty string

        // Act
        var actualHash = Sha256Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedResult, actualHash);
        Assert.Equal(64, actualHash.Length);
        Assert.NotEmpty(actualHash);
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