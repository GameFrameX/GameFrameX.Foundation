using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// 提供统一哈希计算和固定时间比较功能。
/// </summary>
/// <remarks>
/// Provides unified hash computation and fixed-time comparison functionality.
/// </remarks>
public static class HashHelper
{
    /// <summary>
    /// 计算字符串的指定哈希值 / Computes the selected hash for a string.
    /// </summary>
    public static string Compute(HashAlgorithmKind algorithm, string input, Encoding encoding = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        encoding ??= Encoding.UTF8;
        return Compute(algorithm, encoding.GetBytes(input));
    }

    /// <summary>
    /// 计算字节数组的指定哈希值 / Computes the selected hash for a byte array.
    /// </summary>
    public static string Compute(HashAlgorithmKind algorithm, byte[] input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        return algorithm switch
        {
            HashAlgorithmKind.Md5 => Md5Helper.Hash(input),
            HashAlgorithmKind.Sha1 => Sha1Helper.ComputeHash(input),
            HashAlgorithmKind.Sha256 => Sha256Helper.ComputeHash(input),
            HashAlgorithmKind.Sha512 => Sha512Helper.ComputeHash(input),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null),
        };
    }

    /// <summary>
    /// 计算流的指定哈希值 / Computes the selected hash for a stream.
    /// </summary>
    public static string Compute(HashAlgorithmKind algorithm, Stream input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        return algorithm switch
        {
            HashAlgorithmKind.Md5 => Md5Helper.Hash(input),
            HashAlgorithmKind.Sha1 => Sha1Helper.ComputeHash(input),
            HashAlgorithmKind.Sha256 => Sha256Helper.ComputeHash(input),
            HashAlgorithmKind.Sha512 => Sha512Helper.ComputeHash(input),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null),
        };
    }

    /// <summary>
    /// 异步计算流的指定哈希值 / Asynchronously computes the selected hash for a stream.
    /// </summary>
    public static Task<string> ComputeAsync(HashAlgorithmKind algorithm, Stream input, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        return algorithm switch
        {
            HashAlgorithmKind.Md5 => Md5Helper.HashAsync(input, cancellationToken),
            HashAlgorithmKind.Sha1 => Sha1Helper.ComputeHashAsync(input, cancellationToken),
            HashAlgorithmKind.Sha256 => Sha256Helper.ComputeHashAsync(input, cancellationToken),
            HashAlgorithmKind.Sha512 => Sha512Helper.ComputeHashAsync(input, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null),
        };
    }

    /// <summary>
    /// 指示算法是否适用于安全敏感的通用哈希场景 / Indicates whether the algorithm is suitable for security-sensitive general hashing.
    /// </summary>
    public static bool IsCryptographicallySecure(HashAlgorithmKind algorithm)
    {
        return algorithm switch
        {
            HashAlgorithmKind.Md5 or HashAlgorithmKind.Sha1 => false,
            HashAlgorithmKind.Sha256 or HashAlgorithmKind.Sha512 => true,
            _ => throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null),
        };
    }

    /// <summary>
    /// 使用固定时间比较两个字节数组 / Compares two byte arrays in fixed time.
    /// </summary>
    public static bool FixedTimeEquals(byte[] expected, byte[] actual)
    {
        ArgumentNullException.ThrowIfNull(expected, nameof(expected));
        ArgumentNullException.ThrowIfNull(actual, nameof(actual));
        return CryptographicOperations.FixedTimeEquals(expected, actual);
    }

    /// <summary>
    /// 使用固定时间比较两个十六进制字符串 / Compares two hexadecimal strings in fixed time.
    /// </summary>
    public static bool FixedTimeEqualsHex(string expectedHex, string actualHex)
    {
        ArgumentNullException.ThrowIfNull(expectedHex, nameof(expectedHex));
        ArgumentNullException.ThrowIfNull(actualHex, nameof(actualHex));

        try
        {
            return CryptographicOperations.FixedTimeEquals(Convert.FromHexString(expectedHex), Convert.FromHexString(actualHex));
        }
        catch (FormatException)
        {
            return false;
        }
    }

    /// <summary>
    /// 使用固定时间比较两个 Base64 字符串 / Compares two Base64 strings in fixed time.
    /// </summary>
    public static bool FixedTimeEqualsBase64(string expectedBase64, string actualBase64)
    {
        ArgumentNullException.ThrowIfNull(expectedBase64, nameof(expectedBase64));
        ArgumentNullException.ThrowIfNull(actualBase64, nameof(actualBase64));

        try
        {
            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(expectedBase64), Convert.FromBase64String(actualBase64));
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
