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
    /// 该方法执行以下步骤：
    /// 1. 将输入字符串转换为字节数组
    /// 2. 创建SM3Digest实例并计算摘要
    /// 3. 将结果转换为十六进制字符串
    /// </remarks>
    public static string Encrypt(string data)
    {
        byte[] msg1 = Encoding.Default.GetBytes(data);
        //byte[] key1 = Encoding.Default.GetBytes(secretKey);

        //var keyParameter = new KeyParameter(key1);
        var sm3 = new Sm3Digest();

        //HMac mac = new HMac(sm3); // 带密钥的杂凑算法
        //mac.Init(keyParameter);
        sm3.BlockUpdate(msg1, 0, msg1.Length);
        // byte[] result = new byte[sm3.GetMacSize()];
        byte[] result = new byte[sm3.GetDigestSize()];
        sm3.DoFinal(result, 0);
        return Encoding.ASCII.GetString(Hex.Encode(result));
    }
}