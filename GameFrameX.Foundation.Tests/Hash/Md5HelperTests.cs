using Xunit;
using GameFrameX.Foundation.Hash;
using System.Text;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// MD5 哈希算法单元测试
/// </summary>
public class Md5HelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试MD5哈希算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";

    [Fact]
    public void ComputeHash_ValidString_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = Md5Helper.Hash(input);
        var hash2 = Md5Helper.Hash(input);

        // Assert
        Assert.NotNull(hash1);
        Assert.NotEmpty(hash1);
        Assert.Equal(hash1, hash2);
        Assert.Equal(32, hash1.Length); // MD5 哈希长度应为32个字符
    }

    [Fact]
    public void ComputeHash_EmptyString_ShouldReturnValidHash()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = Md5Helper.Hash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(32, hash.Length);
    }

    [Fact]
    public void ComputeHash_ChineseString_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = Md5Helper.Hash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(32, hash.Length);
    }

    [Fact]
    public void ComputeHash_LongString_ShouldReturnValidHash()
    {
        // Arrange
        var input = LongString;

        // Act
        var hash = Md5Helper.Hash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(32, hash.Length);
    }

    [Fact]
    public void ComputeHash_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = Md5Helper.Hash(input1);
        var hash2 = Md5Helper.Hash(input2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_ByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);

        // Act
        var hash = Md5Helper.Hash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(32, hash.Length);
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
        var hash = Md5Helper.Hash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(32, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_Performance_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeInput = new string('A', 100000); // 100KB 数据
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var hash = Md5Helper.Hash(largeInput);
        stopwatch.Stop();

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(32, hash.Length);
        Assert.True(stopwatch.ElapsedMilliseconds < 100, $"MD5计算耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void Hash_NullString_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.Hash((string)null));
        Assert.Equal("input", exception.ParamName);
    }

    [Fact]
    public void Hash_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.Hash((byte[])null));
        Assert.Equal("input", exception.ParamName);
    }

    [Fact]
    public void Hash_NullStream_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.Hash((Stream)null));
        Assert.Equal("input", exception.ParamName);
    }

    [Fact]
    public void HashWithSalt_NullInput_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.HashWithSalt(null, "salt"));
        Assert.Equal("input", exception.ParamName);
    }

    [Fact]
    public void HashWithSalt_NullSalt_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.HashWithSalt("input", (string)null));
        Assert.Equal("salt", exception.ParamName);
    }

    [Fact]
    public void HashWithSalt_NullByteArraySalt_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.HashWithSalt("input", (byte[])null));
        Assert.Equal("salt", exception.ParamName);
    }

    [Fact]
    public void IsVerify_NullInput_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.IsVerify(null, "hash"));
        Assert.Equal("input", exception.ParamName);
    }

    [Fact]
    public void IsVerify_NullHash_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.IsVerify("input", null));
        Assert.Equal("hash", exception.ParamName);
    }

    [Fact]
    public void IsVerifyWithSalt_NullInput_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.IsVerifyWithSalt(null, "salt", "hash"));
        Assert.Equal("input", exception.ParamName);
    }

    [Fact]
    public void IsVerifyWithSalt_NullSalt_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.IsVerifyWithSalt("input", null, "hash"));
        Assert.Equal("salt", exception.ParamName);
    }

    [Fact]
    public void IsVerifyWithSalt_NullHash_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.IsVerifyWithSalt("input", "salt", null));
        Assert.Equal("hash", exception.ParamName);
    }

    [Fact]
    public void HashByFilePath_NullPath_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Md5Helper.HashByFilePath(null));
        Assert.Equal("filePath", exception.ParamName);
    }

    [Fact]
    public void HashByFilePath_EmptyPath_ShouldThrowArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => Md5Helper.HashByFilePath(""));
        Assert.Equal("filePath", exception.ParamName);
    }
}