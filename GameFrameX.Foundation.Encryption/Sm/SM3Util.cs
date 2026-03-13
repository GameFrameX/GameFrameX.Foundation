using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM3工具类，提供SM3密码杂凑算法的实现。
/// 该类用于计算数据的SM3摘要值，可用于数据完整性校验和数字签名。
/// </summary>
internal static class SM3Util
{
    /// <summary>
    /// 使用SM3算法计算输入数据的摘要值。
    /// </summary>
    /// <param name="data">要计算摘要的输入字符串。</param>
    /// <returns>返回十六进制格式的SM3摘要值字符串。</returns>
    /// <remarks>
    /// I-01 修复：原方法名 Encrypt 语义错误（SM3 是哈希算法，不是加密算法），重命名为 Hash。
    /// </remarks>
    public static string Hash(string data)
    {
        // W-03 修复：使用 UTF8 代替 Encoding.Default，确保跨平台哈希结果一致
        byte[] msg1 = Encoding.UTF8.GetBytes(data);

        var sm3 = new Sm3Digest();
        sm3.BlockUpdate(msg1, 0, msg1.Length);
        byte[] result = new byte[sm3.GetDigestSize()];
        sm3.DoFinal(result, 0);
        return Encoding.ASCII.GetString(Hex.Encode(result));
    }
}