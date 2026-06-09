using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Hash;
using Xunit;

namespace GameFrameX.Foundation.Tests.Hash;

public class HashHighPriorityFeatureTests
{
    private static readonly byte[] Data = Encoding.UTF8.GetBytes("stream hash test 数据");

    [Fact]
    public async Task CryptographicHelpers_StreamAndAsyncStream_ShouldMatchByteArray()
    {
        Assert.Equal(Md5Helper.Hash(Data), Md5Helper.Hash(new MemoryStream(Data)));
        Assert.Equal(Md5Helper.Hash(Data), await Md5Helper.HashAsync(new MemoryStream(Data)));

        Assert.Equal(Sha1Helper.ComputeHash(Data), Sha1Helper.ComputeHash(new MemoryStream(Data)));
        Assert.Equal(Sha1Helper.ComputeHash(Data), await Sha1Helper.ComputeHashAsync(new MemoryStream(Data)));

        Assert.Equal(Sha256Helper.ComputeHash(Data), Sha256Helper.ComputeHash(new MemoryStream(Data)));
        Assert.Equal(Sha256Helper.ComputeHash(Data), await Sha256Helper.ComputeHashAsync(new MemoryStream(Data)));

        Assert.Equal(Sha512Helper.ComputeHash(Data), Sha512Helper.ComputeHash(new MemoryStream(Data)));
        Assert.Equal(Sha512Helper.ComputeHash(Data), await Sha512Helper.ComputeHashAsync(new MemoryStream(Data)));
    }

    [Fact]
    public async Task CryptographicHelpers_AsyncStream_WithCanceledToken_ShouldCancel()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => Sha256Helper.ComputeHashAsync(new MemoryStream(Data), cancellationTokenSource.Token));
    }

    [Fact]
    public async Task CrcHelpers_AsyncStream_ShouldMatchSynchronousStream()
    {
        var expectedCrc32 = CrcHelper.GetCrc32(new MemoryStream(Data));
        var expectedCrc64 = CrcHelper.GetCrc64(new MemoryStream(Data));

        Assert.Equal(expectedCrc32, await CrcHelper.GetCrc32Async(new MemoryStream(Data)));
        Assert.Equal(expectedCrc64, await CrcHelper.GetCrc64Async(new MemoryStream(Data)));
    }

    [Fact]
    public async Task CrcHelpers_AsyncStream_WithCanceledToken_ShouldCancel()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CrcHelper.GetCrc32Async(new MemoryStream(Data), cancellationTokenSource.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => CrcHelper.GetCrc64Async(new MemoryStream(Data), cancellationTokenSource.Token));
    }

    [Theory]
    [InlineData(HashAlgorithmKind.Md5, false)]
    [InlineData(HashAlgorithmKind.Sha1, false)]
    [InlineData(HashAlgorithmKind.Sha256, true)]
    [InlineData(HashAlgorithmKind.Sha512, true)]
    public async Task HashHelper_ComputeOverloads_ShouldMatchExistingHelpers(HashAlgorithmKind algorithm, bool isSecure)
    {
        var expected = algorithm switch
        {
            HashAlgorithmKind.Md5 => Md5Helper.Hash(Data),
            HashAlgorithmKind.Sha1 => Sha1Helper.ComputeHash(Data),
            HashAlgorithmKind.Sha256 => Sha256Helper.ComputeHash(Data),
            HashAlgorithmKind.Sha512 => Sha512Helper.ComputeHash(Data),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm)),
        };

        Assert.Equal(expected, HashHelper.Compute(algorithm, Data));
        Assert.Equal(expected, HashHelper.Compute(algorithm, Encoding.UTF8.GetString(Data)));
        Assert.Equal(expected, HashHelper.Compute(algorithm, new MemoryStream(Data)));
        Assert.Equal(expected, await HashHelper.ComputeAsync(algorithm, new MemoryStream(Data)));
        Assert.Equal(isSecure, HashHelper.IsCryptographicallySecure(algorithm));
    }

    [Fact]
    public void HashHelper_FixedTimeComparison_ShouldHandleBytesHexAndBase64()
    {
        var expected = SHA256.HashData(Data);
        var different = SHA256.HashData(Encoding.UTF8.GetBytes("different"));
        var expectedHex = Convert.ToHexString(expected).ToLowerInvariant();
        var expectedBase64 = Convert.ToBase64String(expected);

        Assert.True(HashHelper.FixedTimeEquals(expected, expected.ToArray()));
        Assert.False(HashHelper.FixedTimeEquals(expected, different));
        Assert.True(HashHelper.FixedTimeEqualsHex(expectedHex, expectedHex.ToUpperInvariant()));
        Assert.False(HashHelper.FixedTimeEqualsHex(expectedHex, "not-hex"));
        Assert.True(HashHelper.FixedTimeEqualsBase64(expectedBase64, expectedBase64));
        Assert.False(HashHelper.FixedTimeEqualsBase64(expectedBase64, "not-base64"));
    }

    [Fact]
    public async Task HmacSha256_StreamAndByteOverloads_ShouldMatchExistingStringOverload()
    {
        const string message = "message";
        const string key = "secret";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var expected = HmacSha256Helper.Hash(message, key);

        Assert.Equal(expected, HmacSha256Helper.Hash(messageBytes, keyBytes));
        Assert.Equal(expected, HmacSha256Helper.Hash(new MemoryStream(messageBytes), keyBytes));
        Assert.Equal(expected, await HmacSha256Helper.HashAsync(new MemoryStream(messageBytes), keyBytes));
        Assert.True(HmacSha256Helper.Verify(message, key, expected));
        Assert.True(HmacSha256Helper.Verify(messageBytes, keyBytes, expected));
        Assert.True(HmacSha256Helper.Verify(new MemoryStream(messageBytes), keyBytes, expected));
        Assert.False(HmacSha256Helper.Verify(messageBytes, keyBytes, Convert.ToBase64String(new byte[32])));
        Assert.False(HmacSha256Helper.Verify(messageBytes, keyBytes, "not-base64"));
    }

    [Fact]
    public async Task HmacSha512_AllOverloads_ShouldMatchBcl()
    {
        const string message = "message";
        const string key = "secret";
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var expected = Convert.ToBase64String(HMACSHA512.HashData(keyBytes, messageBytes));

        Assert.Equal(expected, HmacSha512Helper.Hash(message, key));
        Assert.Equal(expected, HmacSha512Helper.Hash(messageBytes, keyBytes));
        Assert.Equal(expected, HmacSha512Helper.Hash(new MemoryStream(messageBytes), keyBytes));
        Assert.Equal(expected, await HmacSha512Helper.HashAsync(new MemoryStream(messageBytes), keyBytes));
        Assert.True(HmacSha512Helper.Verify(message, key, expected));
        Assert.True(HmacSha512Helper.Verify(messageBytes, keyBytes, expected));
        Assert.True(HmacSha512Helper.Verify(new MemoryStream(messageBytes), keyBytes, expected));
        Assert.False(HmacSha512Helper.Verify(messageBytes, keyBytes, "not-base64"));
    }

    [Fact]
    public async Task HmacHelpers_AsyncStream_WithCanceledToken_ShouldCancel()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var key = Encoding.UTF8.GetBytes("secret");

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => HmacSha256Helper.HashAsync(new MemoryStream(Data), key, cancellationTokenSource.Token));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => HmacSha512Helper.HashAsync(new MemoryStream(Data), key, cancellationTokenSource.Token));
    }
}
