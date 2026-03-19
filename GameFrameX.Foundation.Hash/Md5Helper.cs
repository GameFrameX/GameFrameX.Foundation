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

using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// MD5 哈希计算工具类。
/// 提供字符串、流、文件和字节数组的MD5哈希值计算功能。
/// MD5生成一个128位(16字节)的哈希值，通常表示为32个十六进制数字。
/// 注意：MD5已不再被认为是加密安全的，建议在安全要求较高的场景使用SHA-256或更高强度的算法。
/// </summary>
/// <remarks>
/// MD5 hash computation utility class.
/// Provides MD5 hash computation functionality for strings, streams, files, and byte arrays.
/// MD5 generates a 128-bit (16-byte) hash value, typically represented as 32 hexadecimal digits.
/// Note: MD5 is no longer considered cryptographically secure; it is recommended to use SHA-256 or stronger algorithms for security-sensitive scenarios.
/// </remarks>
public static class Md5Helper
{
    /// <summary>
    /// MD5加密服务提供程序的实例。
    /// 使用静态字段缓存实例以提高性能。
    /// </summary>
    /// <remarks>
    /// Instance of the MD5 cryptographic service provider.
    /// Uses a static field to cache the instance for improved performance.
    /// </remarks>
    private static readonly MD5 Md5Cryptography = MD5.Create();

    /// <summary>
    /// 获取字符串的 MD5 哈希值。
    /// 使用UTF-8编码将字符串转换为字节数组后计算哈希值。
    /// </summary>
    /// <remarks>
    /// Gets the MD5 hash of a string.
    /// Converts the string to a byte array using UTF-8 encoding before computing the hash.
    /// </remarks>
    /// <param name="input">要计算哈希值的字符串，不能为null / The string to compute the hash for, cannot be null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值，默认为false返回小写 / Whether to return uppercase hash, defaults to false for lowercase</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/> 为 null 时抛出 / Thrown when <paramref name="input"/> is null</exception>
    public static string Hash(string input, bool isUpper = false)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        var data = Md5Cryptography.ComputeHash(Encoding.UTF8.GetBytes(input));
        return ToHash(data, isUpper);
    }

    /// <summary>
    /// 获取字符串的加盐 MD5 哈希值。
    /// 将盐值附加到输入字符串后再计算哈希值。
    /// </summary>
    /// <remarks>
    /// Gets the salted MD5 hash of a string.
    /// Appends the salt value to the input string before computing the hash.
    /// </remarks>
    /// <param name="input">要计算哈希值的字符串，不能为null / The string to compute the hash for, cannot be null</param>
    /// <param name="salt">盐值，不能为null / The salt value, cannot be null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值，默认为false返回小写 / Whether to return uppercase hash, defaults to false for lowercase</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/> 或 <paramref name="salt"/> 为 null 时抛出 / Thrown when <paramref name="input"/> or <paramref name="salt"/> is null</exception>
    public static string HashWithSalt(string input, string salt, bool isUpper = false)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(salt, nameof(salt));
        var saltedInput = input + salt;
        return Hash(saltedInput, isUpper);
    }

    /// <summary>
    /// 获取字符串的加盐 MD5 哈希值。
    /// 将盐值以字节数组形式与输入数据合并后计算哈希值。
    /// </summary>
    /// <remarks>
    /// Gets the salted MD5 hash of a string.
    /// Merges the salt as a byte array with the input data before computing the hash.
    /// </remarks>
    /// <param name="input">要计算哈希值的字符串，不能为null / The string to compute the hash for, cannot be null</param>
    /// <param name="salt">盐值字节数组，不能为null / The salt byte array, cannot be null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值，默认为false返回小写 / Whether to return uppercase hash, defaults to false for lowercase</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/> 或 <paramref name="salt"/> 为 null 时抛出 / Thrown when <paramref name="input"/> or <paramref name="salt"/> is null</exception>
    public static string HashWithSalt(string input, byte[] salt, bool isUpper = false)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(salt, nameof(salt));
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var saltedBytes = new byte[inputBytes.Length + salt.Length];
        Buffer.BlockCopy(inputBytes, 0, saltedBytes, 0, inputBytes.Length);
        Buffer.BlockCopy(salt, 0, saltedBytes, inputBytes.Length, salt.Length);
        
        var data = Md5Cryptography.ComputeHash(saltedBytes);
        return ToHash(data, isUpper);
    }

    /// <summary>
    /// 获取字节数组的 MD5 哈希值。
    /// 直接对字节数组计算哈希值。
    /// </summary>
    /// <remarks>
    /// Gets the MD5 hash of a byte array.
    /// Directly computes the hash on the byte array.
    /// </remarks>
    /// <param name="input">要计算哈希值的字节数组，不能为null / The byte array to compute the hash for, cannot be null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值，默认为false返回小写 / Whether to return uppercase hash, defaults to false for lowercase</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/> 为 null 时抛出 / Thrown when <paramref name="input"/> is null</exception>
    public static string Hash(byte[] input, bool isUpper = false)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        var data = Md5Cryptography.ComputeHash(input);
        return ToHash(data, isUpper);
    }

    /// <summary>
    /// 获取流的 MD5 哈希值。
    /// 可用于计算文件流或内存流等数据的哈希值。
    /// </summary>
    /// <remarks>
    /// Gets the MD5 hash of a stream.
    /// Can be used to compute the hash of file streams, memory streams, etc.
    /// </remarks>
    /// <param name="input">要计算哈希值的流，不能为null / The stream to compute the hash for, cannot be null</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/> 为 null 时抛出 / Thrown when <paramref name="input"/> is null</exception>
    public static string Hash(Stream input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        var data = Md5Cryptography.ComputeHash(input);
        return ToHash(data);
    }

    /// <summary>
    /// 验证输入字符串的 MD5 哈希值是否与给定的哈希值一致。
    /// 比较时忽略大小写。
    /// </summary>
    /// <remarks>
    /// Verifies if the MD5 hash of the input string matches the given hash.
    /// Comparison is case-insensitive.
    /// </remarks>
    /// <param name="input">要验证的原始字符串，不能为null / The original string to verify, cannot be null</param>
    /// <param name="hash">要比较的 MD5 哈希值，不能为null / The MD5 hash to compare, cannot be null</param>
    /// <returns>如果哈希值一致，返回 <c>true</c>；否则返回 <c>false</c> / Returns <c>true</c> if the hashes match; otherwise <c>false</c></returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/> 或 <paramref name="hash"/> 为 null 时抛出 / Thrown when <paramref name="input"/> or <paramref name="hash"/> is null</exception>
    public static bool IsVerify(string input, string hash)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(hash, nameof(hash));
        var comparer = StringComparer.OrdinalIgnoreCase;
        return 0 == comparer.Compare(Hash(input), hash);
    }

    /// <summary>
    /// 验证输入字符串的加盐 MD5 哈希值是否与给定的哈希值一致。
    /// 比较时忽略大小写。
    /// </summary>
    /// <remarks>
    /// Verifies if the salted MD5 hash of the input string matches the given hash.
    /// Comparison is case-insensitive.
    /// </remarks>
    /// <param name="input">要验证的原始字符串，不能为null / The original string to verify, cannot be null</param>
    /// <param name="salt">盐值，不能为null / The salt value, cannot be null</param>
    /// <param name="hash">要比较的 MD5 哈希值，不能为null / The MD5 hash to compare, cannot be null</param>
    /// <returns>如果哈希值一致，返回 <c>true</c>；否则返回 <c>false</c> / Returns <c>true</c> if the hashes match; otherwise <c>false</c></returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="input"/>、<paramref name="salt"/> 或 <paramref name="hash"/> 为 null 时抛出 / Thrown when <paramref name="input"/>, <paramref name="salt"/>, or <paramref name="hash"/> is null</exception>
    public static bool IsVerifyWithSalt(string input, string salt, string hash)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentNullException.ThrowIfNull(salt, nameof(salt));
        ArgumentNullException.ThrowIfNull(hash, nameof(hash));
        var comparer = StringComparer.OrdinalIgnoreCase;
        return 0 == comparer.Compare(HashWithSalt(input, salt), hash);
    }

    /// <summary>
    /// 将字节数组转换为十六进制字符串表示的哈希值。
    /// 每个字节转换为两个十六进制字符。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a hexadecimal string representation of the hash.
    /// Each byte is converted to two hexadecimal characters.
    /// </remarks>
    /// <param name="data">要转换的字节数组 / The byte array to convert</param>
    /// <param name="isUpper">是否返回大写形式的哈希值，默认为false返回小写 / Whether to return uppercase hash, defaults to false for lowercase</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    private static string ToHash(byte[] data, bool isUpper = false)
    {
        var sb = new StringBuilder(data.Length * 2);
        var hex = isUpper ? "0123456789ABCDEF" : "0123456789abcdef";
        foreach (var b in data)
        {
            sb.Append(hex[b >> 4]);
            sb.Append(hex[b & 0xF]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取指定文件路径的 MD5 哈希值。
    /// 通过读取文件流计算文件内容的哈希值。
    /// </summary>
    /// <remarks>
    /// Gets the MD5 hash of the specified file path.
    /// Computes the hash of the file content by reading the file stream.
    /// </remarks>
    /// <param name="filePath">文件的完整路径，不能为null或空字符串 / The full path of the file, cannot be null or empty</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值 / A 32-character hexadecimal string hash value</returns>
    /// <exception cref="ArgumentException">当 <paramref name="filePath"/> 为 null 或空字符串时抛出 / Thrown when <paramref name="filePath"/> is null or empty</exception>
    /// <exception cref="FileNotFoundException">如果指定的文件不存在，则抛出此异常 / Thrown when the specified file does not exist</exception>
    public static string HashByFilePath(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        using var file = new FileStream(filePath, FileMode.Open);
        return Hash(file);
    }
}