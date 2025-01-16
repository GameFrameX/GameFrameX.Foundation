using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM2加密算法工具类
/// 提供SM2非对称加密算法的加密、解密、密钥对生成等功能
/// </summary>
internal static class Sm2Util
{
    /// <summary>
    /// 使用公钥加密字符串数据
    /// </summary>
    /// <param name="publicKeyString">十六进制格式的公钥字符串</param>
    /// <param name="dataString">待加密的原文字符串</param>
    /// <returns>加密后的密文字符串</returns>
    public static string Encrypt(string publicKeyString, string dataString)
    {
        var publicKey = Hex.Decode(publicKeyString);
        var data = Encoding.UTF8.GetBytes(dataString);
        return Encrypt(publicKey, data);
    }

    /// <summary>
    /// 使用私钥解密字符串数据
    /// </summary>
    /// <param name="privateKeyString">十六进制格式的私钥字符串</param>
    /// <param name="encryptedDataString">待解密的密文字符串</param>
    /// <returns>解密后的原文字符串</returns>
    public static string Decrypt(string privateKeyString, string encryptedDataString)
    {
        var privateKey = Hex.Decode(privateKeyString);
        var encryptedData = Hex.Decode(encryptedDataString);
        var deStr = Sm2Util.Decrypt(privateKey, encryptedData);
        string plainText = Encoding.UTF8.GetString(deStr);
        return plainText;
    }

    /// <summary>
    /// 生成SM2密钥对
    /// 生成一对新的公私钥对,并将其以十六进制字符串格式输出
    /// </summary>
    /// <param name="publicKeyString">输出参数,生成的公钥十六进制字符串</param>
    /// <param name="privateKeyString">输出参数,生成的私钥十六进制字符串</param>
    public static void GenerateKeyPair(out string publicKeyString, out string privateKeyString)
    {
        // 获取SM2算法实例
        Sm2 sm2 = Sm2.Instance;
        // 生成非对称密钥对
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        // 获取私钥参数
        ECPrivateKeyParameters ecPrivateKeyParameters = (ECPrivateKeyParameters)key.Private;
        // 获取公钥参数
        ECPublicKeyParameters ecPublicKeyParameters = (ECPublicKeyParameters)key.Public;
        // 从私钥参数中提取私钥值
        BigInteger privateKey = ecPrivateKeyParameters.D;
        // 从公钥参数中提取公钥点
        ECPoint publicKey = ecPublicKeyParameters.Q;

        // 将公钥点编码为字节数组,再转换为十六进制字符串
        publicKeyString = Encoding.ASCII.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper();
        // 将私钥值转换为字节数组,再转换为十六进制字符串
        privateKeyString = Encoding.ASCII.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper();
    }

    /// <summary>
    /// 使用公钥加密字节数组数据
    /// </summary>
    /// <param name="publicKey">公钥字节数组</param>
    /// <param name="data">待加密的原文字节数组</param>
    /// <returns>加密后的密文字符串,包含C1、C2、C3三部分</returns>
    public static string Encrypt(byte[] publicKey, byte[] data)
    {
        if (null == publicKey || publicKey.Length == 0)
        {
            return null;
        }

        if (data == null || data.Length == 0)
        {
            return null;
        }

        byte[] source = new byte[data.Length];
        Array.Copy(data, 0, source, 0, data.Length);

        var cipher = new Cipher();
        Sm2 sm2 = Sm2.Instance;

        ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

        ECPoint c1 = cipher.Init_enc(sm2, userKey);
        cipher.Encrypt(source);

        byte[] c3 = new byte[32];
        cipher.Dofinal(c3);

        string sc1 = Encoding.ASCII.GetString(Hex.Encode(c1.GetEncoded()));
        string sc2 = Encoding.ASCII.GetString(Hex.Encode(source));
        string sc3 = Encoding.ASCII.GetString(Hex.Encode(c3));

        return (sc1 + sc2 + sc3).ToUpper();
    }

    /// <summary>
    /// 使用私钥解密字节数组数据
    /// </summary>
    /// <param name="privateKey">私钥字节数组</param>
    /// <param name="encryptedData">待解密的密文字节数组</param>
    /// <returns>解密后的原文字节数组</returns>
    public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
    {
        if (null == privateKey || privateKey.Length == 0)
        {
            return null;
        }

        if (encryptedData == null || encryptedData.Length == 0)
        {
            return null;
        }

        string data = Encoding.ASCII.GetString(Hex.Encode(encryptedData));

        byte[] c1Bytes = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(0, 130)));
        int c2Len = encryptedData.Length - 97;
        byte[] c2 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(130, 2 * c2Len)));
        byte[] c3 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(130 + 2 * c2Len, 64)));

        Sm2 sm2 = Sm2.Instance;
        var userD = new BigInteger(1, privateKey);

        ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
        var cipher = new Cipher();
        cipher.Init_dec(userD, c1);
        cipher.Decrypt(c2);
        cipher.Dofinal(c3);

        return c2;
    }
}