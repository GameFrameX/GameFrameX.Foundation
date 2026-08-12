using System.Text;
using GameFrameX.Foundation.Hash;
using Xunit;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// HashHelper 边界条件单元测试
/// </summary>
/// <remarks>
/// 覆盖无效枚举、null 输入、非法格式字符串及 CancellationToken 取消等边界场景。
/// </remarks>
public class HashHelperBoundaryTests
{
    private const HashAlgorithmKind InvalidAlgorithm = (HashAlgorithmKind)999;

    // ==================== Compute - 无效枚举 ====================

    [Fact]
    public void Compute_InvalidAlgorithm_StringOverload_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var algorithm = InvalidAlgorithm;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => HashHelper.Compute(algorithm, "x"));
    }

    [Fact]
    public void Compute_InvalidAlgorithm_BytesOverload_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var algorithm = InvalidAlgorithm;
        var input = Encoding.UTF8.GetBytes("x");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => HashHelper.Compute(algorithm, input));
    }

    [Fact]
    public void Compute_InvalidAlgorithm_StreamOverload_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var algorithm = InvalidAlgorithm;
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("x")))
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => HashHelper.Compute(algorithm, stream));
        }
    }

    // ==================== ComputeAsync - 无效枚举 ====================

    [Fact]
    public async Task ComputeAsync_InvalidAlgorithm_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var algorithm = InvalidAlgorithm;
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes("x")))
        {
            // Act & Assert — 非异步方法，switch 同步抛出
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => HashHelper.ComputeAsync(algorithm, stream));
        }
    }

    // ==================== IsCryptographicallySecure - 无效枚举 ====================

    [Fact]
    public void IsCryptographicallySecure_InvalidAlgorithm_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var algorithm = InvalidAlgorithm;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => HashHelper.IsCryptographicallySecure(algorithm));
    }

    // ==================== Compute - null 输入 ====================

    [Fact]
    public void Compute_StringOverload_NullInput_ShouldThrowArgumentNullException()
    {
        // Arrange
        var algorithm = HashAlgorithmKind.Sha256;
        string input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.Compute(algorithm, input));
    }

    [Fact]
    public void Compute_BytesOverload_NullInput_ShouldThrowArgumentNullException()
    {
        // Arrange
        var algorithm = HashAlgorithmKind.Sha256;
        byte[] input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.Compute(algorithm, input));
    }

    [Fact]
    public void Compute_StreamOverload_NullInput_ShouldThrowArgumentNullException()
    {
        // Arrange
        var algorithm = HashAlgorithmKind.Sha256;
        Stream input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.Compute(algorithm, input));
    }

    // ==================== ComputeAsync - null 输入 ====================

    [Fact]
    public async Task ComputeAsync_NullInput_ShouldThrowArgumentNullException()
    {
        // Arrange
        var algorithm = HashAlgorithmKind.Sha256;
        Stream input = null;

        // Act & Assert — 非异步方法，参数检查同步抛出
        await Assert.ThrowsAsync<ArgumentNullException>(() => HashHelper.ComputeAsync(algorithm, input));
    }

    // ==================== ComputeAsync - CancellationToken 取消 ====================

    [Fact]
    public async Task ComputeAsync_WithCanceledToken_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var algorithm = HashAlgorithmKind.Sha256;
        var data = Encoding.UTF8.GetBytes("cancel me");

        using (var cts = new CancellationTokenSource())
        {
            cts.Cancel();

            // Act & Assert
            using (var stream = new MemoryStream(data))
            {
                await Assert.ThrowsAnyAsync<OperationCanceledException>(
                    () => HashHelper.ComputeAsync(algorithm, stream, cts.Token));
            }
        }
    }

    // ==================== FixedTimeEquals - null 参数 ====================

    [Fact]
    public void FixedTimeEquals_NullExpected_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] expected = null;
        var actual = new byte[] { 0x01 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.FixedTimeEquals(expected, actual));
    }

    [Fact]
    public void FixedTimeEquals_NullActual_ShouldThrowArgumentNullException()
    {
        // Arrange
        var expected = new byte[] { 0x01 };
        byte[] actual = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.FixedTimeEquals(expected, actual));
    }

    // ==================== FixedTimeEqualsHex - null 参数 ====================

    [Fact]
    public void FixedTimeEqualsHex_NullExpected_ShouldThrowArgumentNullException()
    {
        // Arrange
        string expected = null;
        var actual = "aabb";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.FixedTimeEqualsHex(expected, actual));
    }

    [Fact]
    public void FixedTimeEqualsHex_NullActual_ShouldThrowArgumentNullException()
    {
        // Arrange
        var expected = "aabb";
        string actual = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.FixedTimeEqualsHex(expected, actual));
    }

    // ==================== FixedTimeEqualsBase64 - null 参数 ====================

    [Fact]
    public void FixedTimeEqualsBase64_NullExpected_ShouldThrowArgumentNullException()
    {
        // Arrange
        string expected = null;
        var actual = "dGVzdA==";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.FixedTimeEqualsBase64(expected, actual));
    }

    [Fact]
    public void FixedTimeEqualsBase64_NullActual_ShouldThrowArgumentNullException()
    {
        // Arrange
        var expected = "dGVzdA==";
        string actual = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HashHelper.FixedTimeEqualsBase64(expected, actual));
    }

    // ==================== FixedTimeEqualsHex - 非法 hex ====================

    [Fact]
    public void FixedTimeEqualsHex_InvalidHex_ShouldReturnFalse()
    {
        // Arrange
        var validHex = "aabbcc";
        var invalidHex = "not-hex";

        // Act
        var result1 = HashHelper.FixedTimeEqualsHex(validHex, invalidHex);
        var result2 = HashHelper.FixedTimeEqualsHex(invalidHex, validHex);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    // ==================== FixedTimeEqualsBase64 - 非法 base64 ====================

    [Fact]
    public void FixedTimeEqualsBase64_InvalidBase64_ShouldReturnFalse()
    {
        // Arrange
        var validBase64 = "dGVzdA==";
        var invalidBase64 = "not-base64";

        // Act
        var result1 = HashHelper.FixedTimeEqualsBase64(validBase64, invalidBase64);
        var result2 = HashHelper.FixedTimeEqualsBase64(invalidBase64, validBase64);

        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }
}
