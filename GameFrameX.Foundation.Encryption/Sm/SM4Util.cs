using System.Text;
using Org.BouncyCastle.Utilities.Encoders;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM4工具类,提供SM4对称加密算法的加密和解密功能
/// </summary>
internal sealed class Sm4Util
{
    /// <summary>
    /// 密钥,长度必须为16字节
    /// </summary>
    public string secretKey = "1814546261730461";

    /// <summary>
    /// 初始化向量
    /// </summary>
    public string iv = "0000000000000000";

    /// <summary>
    /// 是否以十六进制字符串形式处理密钥
    /// </summary>
    public bool hexString = false;

    /// <summary>
    /// 是否为JavaScript兼容模式
    /// </summary>
    public bool forJavascript = false;

    /// <summary>
    /// 使用ECB模式加密字符串
    /// </summary>
    /// <param name="plainText">待加密的明文字符串</param>
    /// <returns>加密后的密文字符串</returns>
    public string Encrypt_ECB(string plainText)
    {
        var ctx = new Sm4Context()
        {
            IsPadding = true,
            Mode = Sm4.Sm4Encrypt
        };

        byte[] keyBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
        }

        var sm4 = new Sm4
        {
            ForJavascript = forJavascript,
        };

        sm4.Sm4SetKeyEnc(ctx, keyBytes);
        byte[] encrypted = sm4.Sm4_crypt_ecb(ctx, Encoding.ASCII.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }

    /// <summary>
    /// 使用ECB模式加密字节数组
    /// </summary>
    /// <param name="plainBytes">待加密的明文字节数组</param>
    /// <param name="keyBytes">密钥字节数组</param>
    /// <returns>加密后的密文字节数组</returns>
    public byte[] Encrypt_ECB(byte[] plainBytes, byte[] keyBytes)
    {
        var ctx = new Sm4Context
        {
            IsPadding = false,
            Mode = Sm4.Sm4Encrypt
        };

        var sm4 = new Sm4
        {
            ForJavascript = forJavascript,
        };

        sm4.Sm4SetKeyEnc(ctx, keyBytes);
        byte[] encrypted = sm4.Sm4_crypt_ecb(ctx, plainBytes);
        return encrypted;

        //return Hex.Encode(encrypted);
    }

    /// <summary>
    /// 使用ECB模式解密字符串
    /// </summary>
    /// <param name="cipherText">待解密的密文字符串</param>
    /// <returns>解密后的明文字符串</returns>
    public string Decrypt_ECB(string cipherText)
    {
        var ctx = new Sm4Context()
        {
            IsPadding = true,
            Mode = Sm4.Sm4Decrypt
        };

        byte[] keyBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
        }

        var sm4 = new Sm4
        {
            ForJavascript = forJavascript,
        };

        sm4.Sm4SetKeyDec(ctx, keyBytes);
        byte[] decrypted = sm4.Sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }

    /// <summary>
    /// 使用CBC模式加密字符串
    /// </summary>
    /// <param name="plainText">待加密的明文字符串</param>
    /// <returns>加密后的密文字符串</returns>
    public string Encrypt_CBC(string plainText)
    {
        var ctx = new Sm4Context()
        {
            IsPadding = true,
            Mode = Sm4.Sm4Encrypt
        };

        byte[] keyBytes;
        byte[] ivBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
            ivBytes = Hex.Decode(iv);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
            ivBytes = Encoding.ASCII.GetBytes(iv);
        }

        var sm4 = new Sm4
        {
            ForJavascript = forJavascript,
        };
        sm4.Sm4SetKeyEnc(ctx, keyBytes);
        byte[] encrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Encoding.ASCII.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }

    /// <summary>
    /// 使用CBC模式解密字符串
    /// </summary>
    /// <param name="cipherText">待解密的密文字符串</param>
    /// <returns>解密后的明文字符串</returns>
    public string Decrypt_CBC(string cipherText)
    {
        var ctx = new Sm4Context()
        {
            IsPadding = true,
            Mode = Sm4.Sm4Decrypt
        };

        byte[] keyBytes;
        byte[] ivBytes;
        if (hexString)
        {
            keyBytes = Hex.Decode(secretKey);
            ivBytes = Hex.Decode(iv);
        }
        else
        {
            keyBytes = Encoding.ASCII.GetBytes(secretKey);
            ivBytes = Encoding.ASCII.GetBytes(iv);
        }

        var sm4 = new Sm4
        {
            ForJavascript = forJavascript,
        };
        sm4.Sm4SetKeyDec(ctx, keyBytes);
        byte[] decrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
        return Encoding.ASCII.GetString(decrypted);
    }
}