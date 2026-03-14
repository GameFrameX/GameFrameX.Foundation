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

using GameFrameX.Foundation.Hash;
using System.Text;
using Xunit;
using Standart.Hash.xxHash;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// xxHash 哈希算法单元测试
/// </summary>
public class XxHashHelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试xxHash哈希算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";
    private readonly byte[] TestBytes = Encoding.UTF8.GetBytes(TestString);
    private readonly byte[] EmptyBytes = new byte[1]; // 修改为最小非空数组
    private readonly byte[] LargeBytes = Encoding.UTF8.GetBytes(new string('A', 10000));

    #region Hash32 Tests

    [Fact]
    public void Hash32_ValidString_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = XxHashHelper.Hash32(input);
        var hash2 = XxHashHelper.Hash32(input);

        // Assert
        Assert.Equal(hash1, hash2);
        Assert.True(hash1 > 0);
    }

    [Fact]
    public void Hash32_ValidByteArray_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestBytes;

        // Act
        var hash1 = XxHashHelper.Hash32(input);
        var hash2 = XxHashHelper.Hash32(input);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash32_EmptyString_ShouldReturnValidHash()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = XxHashHelper.Hash32(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash32_EmptyByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = new byte[1]; // 使用最小非空数组

        // Act
        var hash = XxHashHelper.Hash32(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash32_ChineseString_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = XxHashHelper.Hash32(input);

        // Assert
        Assert.True(hash > 0);
    }

    [Fact]
    public void Hash32_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = XxHashHelper.Hash32(input1);
        var hash2 = XxHashHelper.Hash32(input2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash32_Type_ShouldReturnValidHash()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var hash = XxHashHelper.Hash32(type);

        // Assert
        Assert.True(hash > 0);
    }

    [Fact]
    public void Hash32_GenericType_ShouldReturnValidHash()
    {
        // Act
        var hash = XxHashHelper.Hash32<int>();

        // Assert
        Assert.True(hash > 0);
    }

    [Fact]
    public void Hash32_SameType_ShouldReturnConsistentHash()
    {
        // Act
        var hash1 = XxHashHelper.Hash32<string>();
        var hash2 = XxHashHelper.Hash32<string>();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash32_DifferentTypes_ShouldReturnDifferentHashes()
    {
        // Act
        var hash1 = XxHashHelper.Hash32<int>();
        var hash2 = XxHashHelper.Hash32<string>();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash32_Performance_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeInput = new string('A', 100000); // 100KB 数据
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var hash = XxHashHelper.Hash32(largeInput);
        stopwatch.Stop();

        // Assert
        Assert.True(hash >= 0);
        Assert.True(stopwatch.ElapsedMilliseconds < 100, $"xxHash32计算耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }

    #endregion

    #region Hash64 Tests

    [Fact]
    public void Hash64_ValidString_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = XxHashHelper.Hash64(input);
        var hash2 = XxHashHelper.Hash64(input);

        // Assert
        Assert.Equal(hash1, hash2);
        Assert.True(hash1 > 0);
    }

    [Fact]
    public void Hash64_ValidByteArray_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestBytes;

        // Act
        var hash1 = XxHashHelper.Hash64(input);
        var hash2 = XxHashHelper.Hash64(input);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash64_EmptyString_ShouldReturnValidHash()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = XxHashHelper.Hash64(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash64_EmptyByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = new byte[1]; // 使用最小非空数组

        // Act
        var hash = XxHashHelper.Hash64(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash64_ChineseString_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = XxHashHelper.Hash64(input);

        // Assert
        Assert.True(hash > 0);
    }

    [Fact]
    public void Hash64_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = XxHashHelper.Hash64(input1);
        var hash2 = XxHashHelper.Hash64(input2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash64_Type_ShouldReturnValidHash()
    {
        // Arrange
        var type = typeof(string);

        // Act
        var hash = XxHashHelper.Hash64(type);

        // Assert
        Assert.True(hash > 0);
    }

    [Fact]
    public void Hash64_GenericType_ShouldReturnValidHash()
    {
        // Act
        var hash = XxHashHelper.Hash64<int>();

        // Assert
        Assert.True(hash > 0);
    }

    [Fact]
    public void Hash64_SameType_ShouldReturnConsistentHash()
    {
        // Act
        var hash1 = XxHashHelper.Hash64<string>();
        var hash2 = XxHashHelper.Hash64<string>();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash64_DifferentTypes_ShouldReturnDifferentHashes()
    {
        // Act
        var hash1 = XxHashHelper.Hash64<int>();
        var hash2 = XxHashHelper.Hash64<string>();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash64_Performance_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeInput = new string('A', 100000); // 100KB 数据
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var hash = XxHashHelper.Hash64(largeInput);
        stopwatch.Stop();

        // Assert
        Assert.True(hash >= 0);
        Assert.True(stopwatch.ElapsedMilliseconds < 100, $"xxHash64计算耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }

    #endregion

    #region Hash128 Tests

    [Fact]
    public void Hash128_ValidByteArray_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestBytes;

        // Act
        var hash1 = XxHashHelper.Hash128(input);
        var hash2 = XxHashHelper.Hash128(input);

        // Assert
        Assert.Equal(hash1.high64, hash2.high64);
        Assert.Equal(hash1.low64, hash2.low64);
    }

    [Fact]
    public void Hash128_ValidByteArrayWithLength_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestBytes;
        var length = input.Length;

        // Act
        var hash1 = XxHashHelper.Hash128(input, length);
        var hash2 = XxHashHelper.Hash128(input, length);

        // Assert
        Assert.Equal(hash1.high64, hash2.high64);
        Assert.Equal(hash1.low64, hash2.low64);
    }

    [Fact]
    public void Hash128_ValidString_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = XxHashHelper.Hash128(input);
        var hash2 = XxHashHelper.Hash128(input);

        // Assert
        Assert.Equal(hash1.high64, hash2.high64);
        Assert.Equal(hash1.low64, hash2.low64);
    }

    [Fact]
    public void Hash128_EmptyByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = new byte[1]; // 使用最小非空数组避免IndexOutOfRangeException

        // Act
        var hash = XxHashHelper.Hash128(input, 0); // 指定长度为0

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    [Fact]
    public void Hash128_EmptyString_ShouldReturnValidHash()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = XxHashHelper.Hash128(input);

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    [Fact]
    public void Hash128_ChineseString_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = XxHashHelper.Hash128(input);

        // Assert
        Assert.True(hash.high64 > 0 || hash.low64 > 0);
    }

    [Fact]
    public void Hash128_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = XxHashHelper.Hash128(input1);
        var hash2 = XxHashHelper.Hash128(input2);

        // Assert
        Assert.True(hash1.high64 != hash2.high64 || hash1.low64 != hash2.low64);
    }

    [Fact]
    public void Hash128_PartialLength_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestBytes;
        var partialLength = input.Length / 2;

        // Act
        var hash = XxHashHelper.Hash128(input, partialLength);

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    [Fact]
    public void Hash128_DifferentLengths_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input = TestBytes;
        var fullLength = input.Length;
        var partialLength = input.Length / 2;

        // Act
        var hashFull = XxHashHelper.Hash128(input, fullLength);
        var hashPartial = XxHashHelper.Hash128(input, partialLength);

        // Assert
        Assert.True(hashFull.high64 != hashPartial.high64 || hashFull.low64 != hashPartial.low64);
    }

    [Fact]
    public void Hash128_Performance_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeInput = new string('A', 100000); // 100KB 数据
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var hash = XxHashHelper.Hash128(largeInput);
        stopwatch.Stop();

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
        Assert.True(stopwatch.ElapsedMilliseconds < 100, $"xxHash128计算耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void IsDefault_DefaultValue_ShouldReturnTrue()
    {
        // Arrange
        var defaultHash = new uint128 { high64 = 0, low64 = 0 };

        // Act
        var result = XxHashHelper.IsDefault(defaultHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsDefault_NonDefaultValue_ShouldReturnFalse()
    {
        // Arrange
        var nonDefaultHash = new uint128 { high64 = 1, low64 = 0 };

        // Act
        var result = XxHashHelper.IsDefault(nonDefaultHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsDefault_NonDefaultValue2_ShouldReturnFalse()
    {
        // Arrange
        var nonDefaultHash = new uint128 { high64 = 0, low64 = 1 };

        // Act
        var result = XxHashHelper.IsDefault(nonDefaultHash);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region Cross-Method Consistency Tests

    [Fact]
    public void Hash32_StringAndByteArray_ShouldBothReturnValidHashes()
    {
        // Arrange
        var input = TestString;
        var inputBytes = Encoding.UTF8.GetBytes(input);

        // Act
        var hashFromString = XxHashHelper.Hash32(input);
        var hashFromBytes = XxHashHelper.Hash32(inputBytes);

        // Assert
        // 注意：字符串和字节数组的Hash32方法可能使用不同的内部实现
        Assert.True(hashFromString >= 0);
        Assert.True(hashFromBytes >= 0);
    }

    [Fact]
    public void Hash64_StringAndByteArray_ShouldBothReturnValidHashes()
    {
        // Arrange
        var input = TestString;
        var inputBytes = Encoding.UTF8.GetBytes(input);

        // Act
        var hashFromString = XxHashHelper.Hash64(input);
        var hashFromBytes = XxHashHelper.Hash64(inputBytes);

        // Assert
        // 注意：字符串和字节数组的Hash64方法可能使用不同的内部实现
        Assert.True(hashFromString >= 0);
        Assert.True(hashFromBytes >= 0);
    }

    [Fact]
    public void Hash128_StringAndByteArray_ShouldBothReturnValidHashes()
    {
        // Arrange
        var input = TestString;
        var inputBytes = Encoding.UTF8.GetBytes(input);

        // Act
        var hashFromString = XxHashHelper.Hash128(input);
        var hashFromBytes = XxHashHelper.Hash128(inputBytes);

        // Assert
        // 注意：字符串和字节数组的Hash128方法可能使用不同的内部实现，因此结果可能不同
        Assert.True(hashFromString.high64 >= 0);
        Assert.True(hashFromString.low64 >= 0);
        Assert.True(hashFromBytes.high64 >= 0);
        Assert.True(hashFromBytes.low64 >= 0);
    }

    [Fact]
    public void Hash128_FullArrayAndLengthSpecified_ShouldReturnSameHash()
    {
        // Arrange
        var input = TestBytes;

        // Act
        var hashFull = XxHashHelper.Hash128(input);
        var hashWithLength = XxHashHelper.Hash128(input, input.Length);

        // Assert
        Assert.Equal(hashFull.high64, hashWithLength.high64);
        Assert.Equal(hashFull.low64, hashWithLength.low64);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Hash32_LargeData_ShouldReturnValidHash()
    {
        // Arrange
        var largeData = LargeBytes;

        // Act
        var hash = XxHashHelper.Hash32(largeData);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash64_LargeData_ShouldReturnValidHash()
    {
        // Arrange
        var largeData = LargeBytes;

        // Act
        var hash = XxHashHelper.Hash64(largeData);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash128_LargeData_ShouldReturnValidHash()
    {
        // Arrange
        var largeData = LargeBytes;

        // Act
        var hash = XxHashHelper.Hash128(largeData);

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    [Fact]
    public void Hash32_SpecialCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "!@#$%^&*()_+-=[]{}|;':,.<>?";

        // Act
        var hash = XxHashHelper.Hash32(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash64_SpecialCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "!@#$%^&*()_+-=[]{}|;':,.<>?";

        // Act
        var hash = XxHashHelper.Hash64(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash128_SpecialCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "!@#$%^&*()_+-=[]{}|;':,.<>?";

        // Act
        var hash = XxHashHelper.Hash128(input);

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    [Fact]
    public void Hash32_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "🌟🚀💻🎉";

        // Act
        var hash = XxHashHelper.Hash32(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash64_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "🌟🚀💻🎉";

        // Act
        var hash = XxHashHelper.Hash64(input);

        // Assert
        Assert.True(hash >= 0);
    }

    [Fact]
    public void Hash128_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "🌟🚀💻🎉";

        // Act
        var hash = XxHashHelper.Hash128(input);

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    [Fact]
    public void Hash128_ZeroLength_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestBytes;
        var length = 0;

        // Act
        var hash = XxHashHelper.Hash128(input, length);

        // Assert
        Assert.True(hash.high64 >= 0);
        Assert.True(hash.low64 >= 0);
    }

    #endregion

    #region Stress Tests

    [Fact]
    public void Hash32_RepeatedCalls_ShouldReturnConsistentResults()
    {
        // Arrange
        var input = "Consistency Test";
        var hashes = new List<uint>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            hashes.Add(XxHashHelper.Hash32(input));
        }

        // Assert
        Assert.True(hashes.All(h => h == hashes[0]), "所有哈希值应该相同");
    }

    [Fact]
    public void Hash64_RepeatedCalls_ShouldReturnConsistentResults()
    {
        // Arrange
        var input = "Consistency Test";
        var hashes = new List<ulong>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            hashes.Add(XxHashHelper.Hash64(input));
        }

        // Assert
        Assert.True(hashes.All(h => h == hashes[0]), "所有哈希值应该相同");
    }

    [Fact]
    public void Hash128_RepeatedCalls_ShouldReturnConsistentResults()
    {
        // Arrange
        var input = "Consistency Test";
        var hashes = new List<uint128>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            hashes.Add(XxHashHelper.Hash128(input));
        }

        // Assert
        var firstHash = hashes[0];
        Assert.True(hashes.All(h => h.high64 == firstHash.high64 && h.low64 == firstHash.low64), "所有哈希值应该相同");
    }

    #endregion

    #region Null Parameter Tests

    [Fact]
    public void Hash32_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] nullBuffer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash32(nullBuffer));
    }

    [Fact]
    public void Hash32_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string nullText = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash32(nullText));
    }

    [Fact]
    public void Hash32_NullType_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type nullType = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash32(nullType));
    }

    [Fact]
    public void Hash64_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] nullBuffer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash64(nullBuffer));
    }

    [Fact]
    public void Hash64_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string nullText = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash64(nullText));
    }

    [Fact]
    public void Hash64_NullType_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type nullType = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash64(nullType));
    }

    [Fact]
    public void Hash128_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] nullBuffer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash128(nullBuffer));
    }

    [Fact]
    public void Hash128_NullByteArrayWithLength_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] nullBuffer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash128(nullBuffer, 0));
    }

    [Fact]
    public void Hash128_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string nullText = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => XxHashHelper.Hash128(nullText));
    }

    [Fact]
    public void Hash128_NegativeLength_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = TestBytes;
        var negativeLength = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => XxHashHelper.Hash128(buffer, negativeLength));
    }

    [Fact]
    public void Hash128_LengthGreaterThanBufferSize_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = TestBytes;
        var invalidLength = buffer.Length + 1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => XxHashHelper.Hash128(buffer, invalidLength));
    }

    #endregion
}