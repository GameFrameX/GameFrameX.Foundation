using System.Text;
using Xunit;
using GameFrameX.Foundation.Hash;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// MurmurHash3 算法单元测试
/// </summary>
public class MurmurHash3HelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试MurmurHash3算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";

    #region Hash String Tests

    [Fact]
    public void Hash_ValidString_ShouldReturnConsistentValue()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = MurmurHash3Helper.Hash(input);
        var hash2 = MurmurHash3Helper.Hash(input);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash_EmptyString_ShouldReturnValidValue()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = MurmurHash3Helper.Hash(input);

        // Assert
        // 空字符串应该返回一致的哈希值
        var hash2 = MurmurHash3Helper.Hash(input);
        Assert.Equal(hash, hash2);
    }

    [Fact]
    public void Hash_ChineseString_ShouldReturnValidValue()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = MurmurHash3Helper.Hash(input);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_LongString_ShouldReturnValidValue()
    {
        // Arrange
        var input = LongString;

        // Act
        var hash = MurmurHash3Helper.Hash(input);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_DifferentStrings_ShouldReturnDifferentValues()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = MurmurHash3Helper.Hash(input1);
        var hash2 = MurmurHash3Helper.Hash(input2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_WithDifferentSeeds_ShouldReturnDifferentValues()
    {
        // Arrange
        var input = TestString;
        var seed1 = 27u;
        var seed2 = 42u;

        // Act
        var hash1 = MurmurHash3Helper.Hash(input, seed1);
        var hash2 = MurmurHash3Helper.Hash(input, seed2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_WithSameSeed_ShouldReturnSameValue()
    {
        // Arrange
        var input = TestString;
        var seed = 123u;

        // Act
        var hash1 = MurmurHash3Helper.Hash(input, seed);
        var hash2 = MurmurHash3Helper.Hash(input, seed);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => MurmurHash3Helper.Hash(input));
    }

    #endregion

    #region Hash Byte Array Tests

    [Fact]
    public void Hash_ValidByteArray_ShouldReturnConsistentValue()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var length = (uint)input.Length;
        var seed = 27u;

        // Act
        var hash1 = MurmurHash3Helper.Hash(input, length, seed);
        var hash2 = MurmurHash3Helper.Hash(input, length, seed);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash_EmptyByteArray_ShouldReturnValidValue()
    {
        // Arrange
        var input = Array.Empty<byte>();
        var length = 0u;
        var seed = 27u;

        // Act
        var hash = MurmurHash3Helper.Hash(input, length, seed);

        // Assert
        // 空数组应该返回一致的哈希值
        var hash2 = MurmurHash3Helper.Hash(input, length, seed);
        Assert.Equal(hash, hash2);
    }

    [Fact]
    public void Hash_ByteArrayWithDifferentSeeds_ShouldReturnDifferentValues()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var length = (uint)input.Length;
        var seed1 = 27u;
        var seed2 = 42u;

        // Act
        var hash1 = MurmurHash3Helper.Hash(input, length, seed1);
        var hash2 = MurmurHash3Helper.Hash(input, length, seed2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_DifferentByteArrays_ShouldReturnDifferentValues()
    {
        // Arrange
        var input1 = Encoding.UTF8.GetBytes("Test1");
        var input2 = Encoding.UTF8.GetBytes("Test2");
        var seed = 27u;

        // Act
        var hash1 = MurmurHash3Helper.Hash(input1, (uint)input1.Length, seed);
        var hash2 = MurmurHash3Helper.Hash(input2, (uint)input2.Length, seed);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_PartialByteArray_ShouldReturnValidValue()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var partialLength = (uint)(input.Length / 2);
        var seed = 27u;

        // Act
        var hash = MurmurHash3Helper.Hash(input, partialLength, seed);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] input = null;
        var length = 0u;
        var seed = 27u;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => MurmurHash3Helper.Hash(input, length, seed));
    }

    [Fact]
    public void Hash_LengthGreaterThanArrayLength_ShouldThrowArgumentException()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var invalidLength = (uint)(input.Length + 1);
        var seed = 27u;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => MurmurHash3Helper.Hash(input, invalidLength, seed));
        Assert.Equal("length", exception.ParamName);
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void Hash_StringAndByteArray_ShouldReturnSameValue()
    {
        // Arrange
        var stringInput = TestString;
        var byteInput = Encoding.UTF8.GetBytes(TestString);
        var seed = 27u;

        // Act
        var hashFromString = MurmurHash3Helper.Hash(stringInput, seed);
        var hashFromBytes = MurmurHash3Helper.Hash(byteInput, (uint)byteInput.Length, seed);

        // Assert
        Assert.Equal(hashFromString, hashFromBytes);
    }

    [Fact]
    public void Hash_ChineseStringAndByteArray_ShouldReturnSameValue()
    {
        // Arrange
        var stringInput = TestStringChinese;
        var byteInput = Encoding.UTF8.GetBytes(TestStringChinese);
        var seed = 27u;

        // Act
        var hashFromString = MurmurHash3Helper.Hash(stringInput, seed);
        var hashFromBytes = MurmurHash3Helper.Hash(byteInput, (uint)byteInput.Length, seed);

        // Assert
        Assert.Equal(hashFromString, hashFromBytes);
    }

    #endregion

    #region Edge Cases Tests

    [Fact]
    public void Hash_SingleCharacter_ShouldReturnValidValue()
    {
        // Arrange
        var input = "A";
        var seed = 27u;

        // Act
        var hash = MurmurHash3Helper.Hash(input, seed);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_SpecialCharacters_ShouldReturnValidValue()
    {
        // Arrange
        var input = "!@#$%^&*()_+-=[]{}|;':,.<>?";
        var seed = 27u;

        // Act
        var hash = MurmurHash3Helper.Hash(input, seed);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_UnicodeCharacters_ShouldReturnValidValue()
    {
        // Arrange
        var input = "🌟🎉🚀💻🔥";
        var seed = 27u;

        // Act
        var hash = MurmurHash3Helper.Hash(input, seed);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_MaxSeedValue_ShouldReturnValidValue()
    {
        // Arrange
        var input = TestString;
        var seed = uint.MaxValue;

        // Act
        var hash = MurmurHash3Helper.Hash(input, seed);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    [Fact]
    public void Hash_ZeroSeed_ShouldReturnValidValue()
    {
        // Arrange
        var input = TestString;
        var seed = 0u;

        // Act
        var hash = MurmurHash3Helper.Hash(input, seed);

        // Assert
        Assert.NotEqual(0u, hash);
    }

    #endregion

    #region Performance Tests

    [Fact]
    public void Hash_LargeData_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeString = new string('A', 100000); // 100KB of 'A' characters
        var seed = 27u;

        // Act
        var startTime = DateTime.UtcNow;
        var hash = MurmurHash3Helper.Hash(largeString, seed);
        var endTime = DateTime.UtcNow;

        // Assert
        Assert.NotEqual(0u, hash);
        // 确保在合理时间内完成（1秒内）
        Assert.True((endTime - startTime).TotalSeconds < 1.0);
    }

    #endregion
}