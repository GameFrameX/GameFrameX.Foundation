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
    /// <exception cref="ArgumentNullException">当plainText为null时抛出</exception>
    /// <exception cref="ArgumentException">当密钥长度不正确时抛出</exception>
    public string Encrypt_ECB(string plainText)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        
        // Validate key length
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException("Secret key must be 32 characters long when hexString is true (16 bytes in hex)", nameof(secretKey));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException("Secret key must be 16 characters long when hexString is false", nameof(secretKey));
            }
        }
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
        byte[] encrypted = sm4.Sm4_crypt_ecb(ctx, Encoding.UTF8.GetBytes(plainText));

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
    /// <exception cref="ArgumentNullException">当cipherText为null时抛出</exception>
    /// <exception cref="ArgumentException">当密钥长度不正确时抛出</exception>
    public string Decrypt_ECB(string cipherText)
    {
        ArgumentNullException.ThrowIfNull(cipherText);
        
        // Validate key length
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException("Secret key must be 32 characters long when hexString is true (16 bytes in hex)", nameof(secretKey));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException("Secret key must be 16 characters long when hexString is false", nameof(secretKey));
            }
        }
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

        string plainText = Encoding.UTF8.GetString(decrypted);
        return plainText;
    }

    /// <summary>
    /// 使用CBC模式加密字符串
    /// </summary>
    /// <param name="plainText">待加密的明文字符串</param>
    /// <returns>加密后的密文字符串</returns>
    /// <exception cref="ArgumentNullException">当plainText为null时抛出</exception>
    /// <exception cref="ArgumentException">当密钥或IV长度不正确时抛出</exception>
    public string Encrypt_CBC(string plainText)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        
        // Validate key and IV lengths
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException("Secret key must be 32 characters long when hexString is true (16 bytes in hex)", nameof(secretKey));
            }
            if (iv?.Length != 32)
            {
                throw new ArgumentException("IV must be 32 characters long when hexString is true (16 bytes in hex)", nameof(iv));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException("Secret key must be 16 characters long when hexString is false", nameof(secretKey));
            }
            if (iv?.Length != 16)
            {
                throw new ArgumentException("IV must be 16 characters long when hexString is false", nameof(iv));
            }
        }
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
        byte[] encrypted = sm4.Sm4_crypt_cbc(ctx, ivBytes, Encoding.UTF8.GetBytes(plainText));

        string cipherText = Encoding.ASCII.GetString(Hex.Encode(encrypted));
        return cipherText;
    }

    /// <summary>
    /// 使用CBC模式解密字符串
    /// </summary>
    /// <param name="cipherText">待解密的密文字符串</param>
    /// <returns>解密后的明文字符串</returns>
    /// <exception cref="ArgumentNullException">当cipherText为null时抛出</exception>
    /// <exception cref="ArgumentException">当密钥或IV长度不正确时抛出</exception>
    public string Decrypt_CBC(string cipherText)
    {
        ArgumentNullException.ThrowIfNull(cipherText);
        
        // Validate key and IV lengths
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException("Secret key must be 32 characters long when hexString is true (16 bytes in hex)", nameof(secretKey));
            }
            if (iv?.Length != 32)
            {
                throw new ArgumentException("IV must be 32 characters long when hexString is true (16 bytes in hex)", nameof(iv));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException("Secret key must be 16 characters long when hexString is false", nameof(secretKey));
            }
            if (iv?.Length != 16)
            {
                throw new ArgumentException("IV must be 16 characters long when hexString is false", nameof(iv));
            }
        }
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

        string plainText = Encoding.UTF8.GetString(decrypted);
        return plainText;
    }
}