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
/// MD5生成一个128位(16字节)的哈希值,通常表示为32个十六进制数字。
/// 注意:MD5已不再被认为是加密安全的,建议在安全要求较高的场景使用SHA-256或更高强度的算法。
/// </summary>
public static class Md5Helper
{
    /// <summary>
    /// MD5加密服务提供程序的实例。
    /// 使用静态字段缓存实例以提高性能。
    /// </summary>
    private static readonly MD5 Md5Cryptography = MD5.Create();

    /// <summary>
    /// 获取字符串的 MD5 哈希值。
    /// 使用UTF-8编码将字符串转换为字节数组后计算哈希值。
    /// </summary>
    /// <param name="input">要计算哈希值的字符串，不能为null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值,默认为false返回小写</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentNullException">当input为null时抛出</exception>
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
    /// <param name="input">要计算哈希值的字符串，不能为null</param>
    /// <param name="salt">盐值，不能为null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值,默认为false返回小写</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentNullException">当input或salt为null时抛出</exception>
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
    /// <param name="input">要计算哈希值的字符串，不能为null</param>
    /// <param name="salt">盐值字节数组，不能为null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值,默认为false返回小写</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentNullException">当input或salt为null时抛出</exception>
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
    /// <param name="input">要计算哈希值的字节数组，不能为null</param>
    /// <param name="isUpper">是否返回大写形式的哈希值,默认为false返回小写</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentNullException">当input为null时抛出</exception>
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
    /// <param name="input">要计算哈希值的流，不能为null</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentNullException">当input为null时抛出</exception>
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
    /// <param name="input">要验证的原始字符串，不能为null</param>
    /// <param name="hash">要比较的 MD5 哈希值，不能为null</param>
    /// <returns>如果哈希值一致，返回 true；否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当input或hash为null时抛出</exception>
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
    /// <param name="input">要验证的原始字符串，不能为null</param>
    /// <param name="salt">盐值，不能为null</param>
    /// <param name="hash">要比较的 MD5 哈希值，不能为null</param>
    /// <returns>如果哈希值一致，返回 true；否则返回 false</returns>
    /// <exception cref="ArgumentNullException">当input、salt或hash为null时抛出</exception>
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
    /// <param name="data">要转换的字节数组</param>
    /// <param name="isUpper">是否返回大写形式的哈希值,默认为false返回小写</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
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
    /// <param name="filePath">文件的完整路径，不能为null或空字符串</param>
    /// <returns>32个字符的十六进制字符串形式的哈希值</returns>
    /// <exception cref="ArgumentException">当filePath为null或空字符串时抛出</exception>
    /// <exception cref="FileNotFoundException">如果指定的文件不存在，则抛出此异常</exception>
    public static string HashByFilePath(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        using var file = new FileStream(filePath, FileMode.Open);
        return Hash(file);
    }
}