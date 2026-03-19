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
/// HMAC-SHA256 哈希算法工具类。
/// 提供使用密钥进行HMAC-SHA256哈希计算的功能。
/// </summary>
/// <remarks>
/// HMAC-SHA256 hash algorithm utility class.
/// Provides functionality for computing HMAC-SHA256 hashes using a key.
/// </remarks>
public static class HmacSha256Helper
{
    /// <summary>
    /// 使用提供的密钥对指定消息进行HMAC-SHA256哈希计算。
    /// </summary>
    /// <remarks>
    /// Computes the HMAC-SHA256 hash of the specified message using the provided key.
    /// HMAC-SHA256 is a key-based Hash-based Message Authentication Code (HMAC) algorithm that combines the SHA-256 hash function with a key.
    /// This method processes input strings using UTF-8 encoding and returns a Base64-encoded result to ensure the output is printable ASCII characters.
    /// </remarks>
    /// <param name="message">要进行哈希计算的消息字符串。消息将使用UTF-8编码转换为字节数组 / The message string to hash. The message will be converted to a byte array using UTF-8 encoding</param>
    /// <param name="key">用于哈希计算的密钥字符串。密钥将使用UTF-8编码转换为字节数组 / The key string for hash computation. The key will be converted to a byte array using UTF-8 encoding</param>
    /// <returns>返回Base64编码的哈希值字符串。哈希值为32字节(256位)长 / Returns a Base64-encoded hash string. The hash is 32 bytes (256 bits) long</returns>
    public static string Hash(string message, string key)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        using (var hmac = new HMACSHA256(keyBytes))
        {
            var hashBytes = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}