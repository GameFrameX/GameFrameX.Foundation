using System.Security.Cryptography;
using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// HMAC-SHA256 哈希算法工具类。
/// 提供使用密钥进行HMAC-SHA256哈希计算的功能。
/// </summary>
public static class HmacSha256Helper
{
    /// <summary>
    /// 使用提供的密钥对指定消息进行HMAC-SHA256哈希计算。
    /// </summary>
    /// <param name="message">要进行哈希计算的消息字符串。消息将使用UTF-8编码转换为字节数组。</param>
    /// <param name="key">用于哈希计算的密钥字符串。密钥将使用UTF-8编码转换为字节数组。</param>
    /// <returns>返回Base64编码的哈希值字符串。哈希值为32字节(256位)长。</returns>
    /// <remarks>
    /// HMAC-SHA256是一种基于密钥的哈希消息认证码（HMAC）算法，它结合了SHA-256哈希函数和密钥。
    /// 此方法使用UTF-8编码处理输入字符串，并返回Base64编码的结果以确保输出是可打印的ASCII字符。
    /// </remarks>
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