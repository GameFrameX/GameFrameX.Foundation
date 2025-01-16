using GameFrameX.Foundation.Encryption.Sm;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// SM2加密算法工具类
/// </summary>
public static class Sm2Helper
{
    /// <summary>
    /// 使用公钥加密字符串数据
    /// </summary>
    /// <param name="publicKeyString">十六进制格式的公钥字符串</param>
    /// <param name="dataString">待加密的原文字符串</param>
    /// <returns>加密后的密文字符串</returns>
    public static string Encrypt(string publicKeyString, string dataString)
    {
        return Sm2Util.Encrypt(publicKeyString, dataString);
    }

    /// <summary>
    /// 使用私钥解密字符串数据
    /// </summary>
    /// <param name="privateKeyString">十六进制格式的私钥字符串</param>
    /// <param name="encryptedDataString">待解密的密文字符串</param>
    /// <returns>解密后的原文字符串</returns>
    public static string Decrypt(string privateKeyString, string encryptedDataString)
    {
        return Sm2Util.Decrypt(privateKeyString, encryptedDataString);
    }

    /// <summary>
    /// 生成SM2密钥对
    /// 生成一对新的公私钥对,并将其以十六进制字符串格式输出
    /// </summary>
    /// <param name="publicKey">输出参数,生成的公钥十六进制字符串</param>
    /// <param name="privateKey">输出参数,生成的私钥十六进制字符串</param>
    public static void GenerateKeyPair(out string publicKey, out string privateKey)
    {
        Sm2Util.GenerateKeyPair(out publicKey, out privateKey);
    }

    /// <summary>
    /// 使用公钥加密字节数组数据
    /// </summary>
    /// <param name="publicKey">公钥字节数组</param>
    /// <param name="data">待加密的原文字节数组</param>
    /// <returns>加密后的密文字符串,包含C1、C2、C3三部分</returns>
    public static string Encrypt(byte[] publicKey, byte[] data)
    {
        return Sm2Util.Encrypt(publicKey, data);
    }

    /// <summary>
    /// 使用私钥解密字节数组数据
    /// </summary>
    /// <param name="privateKey">私钥字节数组</param>
    /// <param name="encryptedData">待解密的密文字节数组</param>
    /// <returns>解密后的原文字节数组</returns>
    public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
    {
        return Sm2Util.Decrypt(privateKey, encryptedData);
    }
}