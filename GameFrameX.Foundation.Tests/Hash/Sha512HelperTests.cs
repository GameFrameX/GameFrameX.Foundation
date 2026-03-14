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
using System.Security.Cryptography;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// SHA-512 哈希算法单元测试
/// </summary>
public class Sha512HelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试SHA-512哈希算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";

    [Fact]
    public void ComputeHash_ValidString_ShouldReturnConsistentHash()
    {
        // Arrange
        var input = TestString;

        // Act
        var hash1 = Sha512Helper.ComputeHash(input);
        var hash2 = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash1);
        Assert.NotEmpty(hash1);
        Assert.Equal(hash1, hash2);
        Assert.Equal(128, hash1.Length); // SHA-512 哈希长度应为128个字符
    }

    [Fact]
    public void ComputeHash_EmptyString_ShouldReturnValidHash()
    {
        // Arrange
        var input = EmptyString;

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(128, hash.Length);
        Assert.Equal("cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e", hash);
    }

    [Fact]
    public void ComputeHash_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha512Helper.ComputeHash(input));
    }

    [Fact]
    public void ComputeHash_ChineseString_ShouldReturnValidHash()
    {
        // Arrange
        var input = TestStringChinese;

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(128, hash.Length);
    }

    [Fact]
    public void ComputeHash_LongString_ShouldReturnValidHash()
    {
        // Arrange
        var input = LongString;

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(128, hash.Length);
    }

    [Fact]
    public void ComputeHash_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input1 = "Test1";
        var input2 = "Test2";

        // Act
        var hash1 = Sha512Helper.ComputeHash(input1);
        var hash2 = Sha512Helper.ComputeHash(input2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_ByteArray_ShouldReturnValidHash()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(128, hash.Length);
    }

    [Fact]
    public void ComputeHash_NullByteArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha512Helper.ComputeHash(input));
    }

    [Fact]
    public void ComputeHash_EmptyByteArray_ShouldReturnValidResult()
    {
        // Arrange
        var input = new byte[0];

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        Assert.Equal(128, hash.Length);
        Assert.Equal("cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e", hash);
    }

    [Fact]
    public void ComputeHash_StringWithDifferentEncodings_ShouldReturnDifferentHashes()
    {
        // Arrange
        var input = "测试字符串";
        
        // Act
        var hashUtf8 = Sha512Helper.ComputeHash(input, Encoding.UTF8);
        var hashUtf16 = Sha512Helper.ComputeHash(input, Encoding.Unicode);

        // Assert
        Assert.NotEqual(hashUtf8, hashUtf16);
        Assert.Equal(128, hashUtf8.Length);
        Assert.Equal(128, hashUtf16.Length);
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
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(128, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_Performance_ShouldCompleteInReasonableTime()
    {
        // Arrange
        var largeInput = new string('A', 100000); // 100KB 数据
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        // Act
        var hash = Sha512Helper.ComputeHash(largeInput);
        stopwatch.Stop();

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(128, hash.Length);
        Assert.True(stopwatch.ElapsedMilliseconds < 150, $"SHA-512计算耗时过长: {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void ComputeFileHash_NonExistentFile_ShouldReturnEmptyString()
    {
        // Arrange
        var filePath = "non_existent_file.txt";

        // Act
        var hash = Sha512Helper.ComputeFileHash(filePath);

        // Assert
        Assert.Equal(string.Empty, hash);
    }

    [Fact]
    public void VerifyHash_ValidStringAndHash_ShouldReturnTrue()
    {
        // Arrange
        var input = TestString;
        var expectedHash = Sha512Helper.ComputeHash(input);

        // Act
        var result = Sha512Helper.VerifyHash(input, expectedHash);

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
        var result = Sha512Helper.VerifyHash(input, invalidHash);

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
        Assert.Throws<ArgumentNullException>(() => Sha512Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_NullHash_ShouldThrowArgumentNullException()
    {
        // Arrange
        var input = TestString;
        string hash = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Sha512Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_EmptyInput_ShouldReturnValidResult()
    {
        // Arrange
        var input = string.Empty;
        var expectedHash = Sha512Helper.ComputeHash(input);

        // Act
        var result = Sha512Helper.VerifyHash(input, expectedHash);

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
        Assert.Throws<ArgumentException>(() => Sha512Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_CaseInsensitive_ShouldReturnTrue()
    {
        // Arrange
        var input = TestString;
        var expectedHash = Sha512Helper.ComputeHash(input).ToUpper();

        // Act
        var result = Sha512Helper.VerifyHash(input, expectedHash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyHash_ByteArray_ValidHash_ShouldReturnTrue()
    {
        // Arrange
        var input = Encoding.UTF8.GetBytes(TestString);
        var expectedHash = Sha512Helper.ComputeHash(input);

        // Act
        var result = Sha512Helper.VerifyHash(input, expectedHash);

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
        var result = Sha512Helper.VerifyHash(input, invalidHash);

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
        Assert.Throws<ArgumentNullException>(() => Sha512Helper.VerifyHash(input, hash));
    }

    [Fact]
    public void VerifyHash_EmptyByteArray_ShouldReturnValidResult()
    {
        // Arrange
        var input = new byte[0];
        var expectedHash = Sha512Helper.ComputeHash(input);

        // Act
        var result = Sha512Helper.VerifyHash(input, expectedHash);

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
        var result = Sha512Helper.VerifyFileHash(filePath, hash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void VerifyFileHash_NullHash_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = "some_file.txt";
        string hash = null;

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => Sha512Helper.VerifyFileHash(filePath, hash));
    }

    [Fact]
    public void VerifyFileHash_EmptyHash_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = "some_file.txt";
        var hash = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha512Helper.VerifyFileHash(filePath, hash));
    }

    [Fact]
    public void ComputeHash_StringAndByteArray_ShouldReturnSameHash()
    {
        // Arrange
        var input = TestString;
        var inputBytes = Encoding.UTF8.GetBytes(input);

        // Act
        var hashFromString = Sha512Helper.ComputeHash(input);
        var hashFromBytes = Sha512Helper.ComputeHash(inputBytes);

        // Assert
        Assert.Equal(hashFromString, hashFromBytes);
    }

    [Fact]
    public void ComputeHash_KnownTestVector_ShouldReturnExpectedHash()
    {
        // Arrange
        var input = "abc";
        var expectedHash = "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f"; // Known SHA-512 hash for "abc"

        // Act
        var actualHash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void ComputeHash_EmptyStringTestVector_ShouldReturnValidHash()
    {
        // Arrange
        var input = "";
        var expectedResult = "cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e"; // Known SHA-512 hash for empty string

        // Act
        var actualHash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedResult, actualHash);
        Assert.Equal(128, actualHash.Length);
        Assert.NotEmpty(actualHash);
    }

    [Fact]
    public void ComputeHash_SingleCharacter_ShouldReturnValidHash()
    {
        // Arrange
        var input = "a";
        var expectedHash = "1f40fc92da241694750979ee6cf582f2d5d7d28e18335de05abc54d0560e0f5302860c652bf08d560252aa5e74210546f369fbbbce8c12cfc7957b2652fe9a75"; // Known SHA-512 hash for "a"

        // Act
        var actualHash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.Equal(expectedHash, actualHash);
    }

    [Fact]
    public void ComputeHash_SpecialCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "!@#$%^&*()_+-=[]{}|;':,.<>?";

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(128, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var input = "🌟🚀💻🎉";

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(128, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_LargeData_ShouldReturnValidHash()
    {
        // Arrange
        var input = new string('X', 1000000); // 1MB 数据

        // Act
        var hash = Sha512Helper.ComputeHash(input);

        // Assert
        Assert.NotNull(hash);
        Assert.Equal(128, hash.Length);
        Assert.True(hash.All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')), "哈希应只包含十六进制字符");
    }

    [Fact]
    public void ComputeHash_RepeatedCalls_ShouldReturnConsistentResults()
    {
        // Arrange
        var input = "Consistency Test";
        var hashes = new List<string>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            hashes.Add(Sha512Helper.ComputeHash(input));
        }

        // Assert
        Assert.True(hashes.All(h => h == hashes[0]), "所有哈希值应该相同");
        Assert.All(hashes, h => Assert.Equal(128, h.Length));
    }

    [Fact]
    public void ComputeFileHash_NullFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        string filePath = null;

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => Sha512Helper.ComputeFileHash(filePath));
    }

    [Fact]
    public void ComputeFileHash_EmptyFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = string.Empty;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha512Helper.ComputeFileHash(filePath));
    }

    [Fact]
    public void VerifyFileHash_NullFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        string filePath = null;
        var hash = "some_hash";

        // Act & Assert
        Assert.ThrowsAny<ArgumentException>(() => Sha512Helper.VerifyFileHash(filePath, hash));
    }

    [Fact]
    public void VerifyFileHash_EmptyFilePath_ShouldThrowArgumentException()
    {
        // Arrange
        var filePath = string.Empty;
        var hash = "some_hash";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sha512Helper.VerifyFileHash(filePath, hash));
    }
}