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