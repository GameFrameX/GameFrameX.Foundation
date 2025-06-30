using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// SHA-512 哈希算法工具类
/// </summary>
public static class Sha512Helper
{
    /// <summary>
    /// 计算字符串的 SHA-512 哈希值
    /// </summary>
    /// <param name="input">要计算哈希值的字符串，不能为 null</param>
    /// <param name="encoding">字符串编码方式，默认为 UTF8</param>
    /// <returns>128个字符的十六进制字符串形式的哈希值，空字符串返回对应的哈希值</returns>
    /// <exception cref="ArgumentNullException">当 input 为 null 时抛出</exception>
    public static string ComputeHash(string input, Encoding encoding = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        encoding ??= Encoding.UTF8;
        return ComputeHash(encoding.GetBytes(input));
    }

    /// <summary>
    /// 计算字节数组的 SHA-512 哈希值
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组，不能为 null</param>
    /// <returns>128个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentNullException">当 buffer 为 null 时抛出</exception>
    public static string ComputeHash(byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

        using var sha512 = SHA512.Create();
        var hash = sha512.ComputeHash(buffer);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 计算文件的 SHA-512 哈希值
    /// </summary>
    /// <param name="filePath">文件路径，不能为 null 或空字符串</param>
    /// <returns>128个字符的十六进制字符串形式的哈希值，如果文件不存在则返回空字符串</returns>
    /// <exception cref="ArgumentException">当 filePath 为 null 或空字符串时抛出</exception>
    public static string ComputeFileHash(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));

        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        using var sha512 = SHA512.Create();
        using var fs = File.OpenRead(filePath);
        var hash = sha512.ComputeHash(fs);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 验证字符串的 SHA-512 哈希值是否匹配
    /// </summary>
    /// <param name="input">原始字符串，不能为 null</param>
    /// <param name="hash">要验证的哈希值，不能为 null</param>
    /// <param name="encoding">字符串编码方式，默认为 UTF8</param>
    /// <returns>如果哈希值匹配则返回true，否则返回false</returns>
    /// <exception cref="ArgumentNullException">当 input 或 hash 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 hash 为空字符串时抛出</exception>
    public static bool VerifyHash(string input, string hash, Encoding encoding = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(hash, nameof(hash));
        ArgumentException.ThrowIfNullOrEmpty(hash, nameof(hash));

        var computedHash = ComputeHash(input, encoding);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证字节数组的 SHA-512 哈希值是否匹配
    /// </summary>
    /// <param name="buffer">原始字节数组，不能为 null</param>
    /// <param name="hash">要验证的哈希值，不能为 null 或空字符串</param>
    /// <returns>如果哈希值匹配则返回true，否则返回false</returns>
    /// <exception cref="ArgumentNullException">当 buffer 或 hash 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 hash 为空字符串时抛出</exception>
    public static bool VerifyHash(byte[] buffer, string hash)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentNullException.ThrowIfNull(hash, nameof(hash));
        ArgumentException.ThrowIfNullOrEmpty(hash, nameof(hash));

        var computedHash = ComputeHash(buffer);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证文件的 SHA-512 哈希值是否匹配
    /// </summary>
    /// <param name="filePath">文件路径，不能为 null 或空字符串</param>
    /// <param name="hash">要验证的哈希值，不能为 null 或空字符串</param>
    /// <returns>如果哈希值匹配则返回true，否则返回false</returns>
    /// <exception cref="ArgumentException">当 filePath 或 hash 为 null 或空字符串时抛出</exception>
    public static bool VerifyFileHash(string filePath, string hash)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        ArgumentException.ThrowIfNullOrEmpty(hash, nameof(hash));

        if (!File.Exists(filePath))
        {
            return false;
        }

        var computedHash = ComputeFileHash(filePath);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }
}