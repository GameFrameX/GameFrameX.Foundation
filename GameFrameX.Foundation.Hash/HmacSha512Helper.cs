using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// HMAC-SHA512 哈希算法工具类 / HMAC-SHA512 hash algorithm utility class.
/// </summary>
public static class HmacSha512Helper
{
    /// <summary>
    /// 使用 UTF-8 编码的消息和密钥计算 Base64 格式 HMAC-SHA512 / Computes a Base64 HMAC-SHA512 using UTF-8 message and key strings.
    /// </summary>
    public static string Hash(string message, string key)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return Hash(Encoding.UTF8.GetBytes(message), Encoding.UTF8.GetBytes(key));
    }

    /// <summary>
    /// 计算字节数组的 Base64 格式 HMAC-SHA512 / Computes a Base64 HMAC-SHA512 for a byte array.
    /// </summary>
    public static string Hash(byte[] message, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return Convert.ToBase64String(HMACSHA512.HashData(key, message));
    }

    /// <summary>
    /// 计算流的 Base64 格式 HMAC-SHA512 / Computes a Base64 HMAC-SHA512 for a stream.
    /// </summary>
    public static string Hash(Stream message, byte[] key)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        using var hmac = new HMACSHA512(key);
        return Convert.ToBase64String(hmac.ComputeHash(message));
    }

    /// <summary>
    /// 异步计算流的 Base64 格式 HMAC-SHA512 / Asynchronously computes a Base64 HMAC-SHA512 for a stream.
    /// </summary>
    public static async Task<string> HashAsync(Stream message, byte[] key, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        using var hmac = new HMACSHA512(key);
        var hash = await hmac.ComputeHashAsync(message, cancellationToken).ConfigureAwait(false);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// 使用固定时间比较验证 HMAC-SHA512 / Verifies an HMAC-SHA512 using fixed-time comparison.
    /// </summary>
    public static bool Verify(byte[] message, byte[] key, string expectedHash)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(expectedHash, nameof(expectedHash));
        return HashHelper.FixedTimeEqualsBase64(Hash(message, key), expectedHash);
    }

    /// <summary>
    /// 使用固定时间比较验证 UTF-8 字符串的 HMAC-SHA512 / Verifies an HMAC-SHA512 for UTF-8 strings using fixed-time comparison.
    /// </summary>
    public static bool Verify(string message, string key, string expectedHash)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(expectedHash, nameof(expectedHash));
        return HashHelper.FixedTimeEqualsBase64(Hash(message, key), expectedHash);
    }

    /// <summary>
    /// 使用固定时间比较验证流的 HMAC-SHA512 / Verifies an HMAC-SHA512 for a stream using fixed-time comparison.
    /// </summary>
    public static bool Verify(Stream message, byte[] key, string expectedHash)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(expectedHash, nameof(expectedHash));
        return HashHelper.FixedTimeEqualsBase64(Hash(message, key), expectedHash);
    }
}
