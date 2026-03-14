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

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// SHA-256 哈希算法工具类
/// </summary>
public static class Sha256Helper
{
    /// <summary>
    /// 计算字符串的 SHA-256 哈希值
    /// </summary>
    /// <param name="input">要计算哈希值的字符串，不能为null</param>
    /// <param name="encoding">字符串编码方式，默认为 UTF8</param>
    /// <returns>64个字符的十六进制字符串形式的哈希值。当input为空字符串时返回空字符串的哈希值</returns>
    /// <exception cref="ArgumentNullException">当input为null时抛出</exception>
    public static string ComputeHash(string input, Encoding encoding = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));

        if (string.IsNullOrEmpty(input))
        {
            return ComputeHash(Array.Empty<byte>());
        }

        encoding ??= Encoding.UTF8;
        return ComputeHash(encoding.GetBytes(input));
    }

    /// <summary>
    /// 计算字节数组的 SHA-256 哈希值
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组，不能为null</param>
    /// <returns>64个字符的十六进制字符串形式的哈希值。当buffer为空数组时返回空数组的哈希值</returns>
    /// <exception cref="ArgumentNullException">当buffer为null时抛出</exception>
    public static string ComputeHash(byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));

        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(buffer);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 计算文件的 SHA-256 哈希值
    /// </summary>
    /// <param name="filePath">文件路径，不能为null或空字符串</param>
    /// <returns>64个字符的十六进制字符串形式的哈希值，如果文件不存在则返回空字符串</returns>
    /// <exception cref="ArgumentException">当filePath为null或空字符串时抛出</exception>
    public static string ComputeFileHash(string filePath)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath, nameof(filePath));
        
        if (!File.Exists(filePath))
        {
            return string.Empty;
        }

        using var sha256 = SHA256.Create();
        using var fs = File.OpenRead(filePath);
        var hash = sha256.ComputeHash(fs);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    /// <summary>
    /// 验证字符串的 SHA-256 哈希值是否匹配
    /// </summary>
    /// <param name="input">原始字符串，不能为null</param>
    /// <param name="hash">要验证的哈希值，不能为null或空字符串</param>
    /// <param name="encoding">字符串编码方式，默认为 UTF8</param>
    /// <returns>如果哈希值匹配则返回true，否则返回false</returns>
    /// <exception cref="ArgumentNullException">当input为null时抛出</exception>
    /// <exception cref="ArgumentException">当hash为null或空字符串时抛出</exception>
    public static bool VerifyHash(string input, string hash, Encoding encoding = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        ArgumentException.ThrowIfNullOrEmpty(hash, nameof(hash));

        var computedHash = ComputeHash(input, encoding);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证字节数组的 SHA-256 哈希值是否匹配
    /// </summary>
    /// <param name="buffer">原始字节数组，不能为null</param>
    /// <param name="hash">要验证的哈希值，不能为null或空字符串</param>
    /// <returns>如果哈希值匹配则返回true，否则返回false</returns>
    /// <exception cref="ArgumentNullException">当buffer为null时抛出</exception>
    /// <exception cref="ArgumentException">当hash为null或空字符串时抛出</exception>
    public static bool VerifyHash(byte[] buffer, string hash)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentException.ThrowIfNullOrEmpty(hash, nameof(hash));

        var computedHash = ComputeHash(buffer);
        return string.Equals(computedHash, hash, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 验证文件的 SHA-256 哈希值是否匹配
    /// </summary>
    /// <param name="filePath">文件路径，不能为null或空字符串</param>
    /// <param name="hash">要验证的哈希值，不能为null或空字符串</param>
    /// <returns>如果哈希值匹配则返回true，否则返回false。文件不存在时返回false</returns>
    /// <exception cref="ArgumentException">当filePath或hash为null或空字符串时抛出</exception>
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