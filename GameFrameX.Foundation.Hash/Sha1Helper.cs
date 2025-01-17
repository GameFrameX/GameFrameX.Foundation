using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// SHA-1 哈希算法工具类。
/// 提供字符串、字节数组和文件的SHA-1哈希值计算和验证功能。
/// SHA-1生成一个160位(20字节)的哈希值,通常表示为40个十六进制数字。
/// 注意:SHA-1已不再被认为是加密安全的,建议在安全要求较高的场景使用SHA-256或更高强度的算法。
/// </summary>
public static class Sha1Helper
{
    /// <summary>
    /// 计算字符串的 SHA-1 哈希值。
    /// 使用指定的编码(默认UTF-8)将字符串转换为字节数组后计算哈希值。
    /// </summary>
    /// <param name="input">要计算哈希值的字符串</param>
    /// <param name="encoding">字符串编码方式，默认为 UTF8</param>
    /// <returns>40个字符的十六进制字符串形式的哈希值。如果输入为空则返回空字符串。</returns>
    public static string ComputeHash(string input, Encoding encoding = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        encoding ??= Encoding.UTF8;
        return ComputeHash(encoding.GetBytes(input));
    }

    /// <summary>
    /// 计算字节数组的 SHA-1 哈希值。
    /// 直接对字节数组进行哈希计算。
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组</param>
    /// <returns>40个字符的十六进制字符串形式的哈希值。如果输入为null或空数组则返回空字符串。</returns>
    public static string ComputeHash(byte[] buffer)
    {
        if (buffer == null || buffer.Length == 0)
        {
            return string.Empty;
        }

        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(buffer);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 计算文件的 SHA-1 哈希值。
    /// 通过文件流读取文件内容并计算其哈希值。
    /// </summary>
    /// <param name="filePath">要计算哈希值的文件的完整路径</param>
    /// <returns>40个字符的十六进制字符串形式的哈希值。如果文件不存在则返回空字符串。</returns>
    public static string ComputeFileHash(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        using var sha1 = SHA1.Create();
        using var fs = File.OpenRead(filePath);
        var hash = sha1.ComputeHash(fs);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 验证字符串的 SHA-1 哈希值是否匹配。
    /// 将输入字符串按指定编码计算哈希值,并与给定的哈希值比较。
    /// </summary>
    /// <param name="input">要验证的原始字符串</param>
    /// <param name="hash">预期的哈希值(40个十六进制字符)</param>
    /// <param name="encoding">字符串编码方式，默认为 UTF8</param>
    /// <returns>如果计算得到的哈希值与给定的哈希值匹配(忽略大小写)则返回true，否则返回false</returns>
    public static bool VerifyHash(string input, string hash, Encoding encoding = null)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(hash))
        {
            return false;
        }

        var computedHash = ComputeHash(input, encoding);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证字节数组的 SHA-1 哈希值是否匹配。
    /// 计算字节数组的哈希值并与给定的哈希值比较。
    /// </summary>
    /// <param name="buffer">要验证的原始字节数组</param>
    /// <param name="hash">预期的哈希值(40个十六进制字符)</param>
    /// <returns>如果计算得到的哈希值与给定的哈希值匹配(忽略大小写)则返回true，否则返回false</returns>
    public static bool VerifyHash(byte[] buffer, string hash)
    {
        if (buffer == null || buffer.Length == 0 || string.IsNullOrEmpty(hash))
        {
            return false;
        }

        var computedHash = ComputeHash(buffer);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证文件的 SHA-1 哈希值是否匹配。
    /// 计算文件的哈希值并与给定的哈希值比较。
    /// </summary>
    /// <param name="filePath">要验证的文件的完整路径</param>
    /// <param name="hash">预期的哈希值(40个十六进制字符)</param>
    /// <returns>如果文件的哈希值与给定的哈希值匹配(忽略大小写)则返回true，否则返回false。文件不存在时返回false。</returns>
    public static bool VerifyFileHash(string filePath, string hash)
    {
        if (!File.Exists(filePath) || string.IsNullOrEmpty(hash))
        {
            return false;
        }

        var computedHash = ComputeFileHash(filePath);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }
} 