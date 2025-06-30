using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// 提供 RSA 加密解密功能。
/// 本类封装了RSA非对称加密算法的常用操作,包括加密、解密、签名和验签等功能。
/// 支持XML格式的密钥对导入导出。
/// </summary>
public sealed class RsaHelper
{
    /// <summary>
    /// RSA算法提供程序实例
    /// </summary>
    private readonly RSA _rsa;

    /// <summary>
    /// 使用指定的 RSA 对象初始化 RSA 加密解密对象。
    /// 通过传入已有的RSA实例来构造Helper对象。
    /// </summary>
    /// <param name="rsa">RSA 对象,不能为null。</param>
    /// <exception cref="ArgumentNullException">当<paramref name="rsa"/>为null时抛出。</exception>
    public RsaHelper(RSA rsa)
    {
        ArgumentNullException.ThrowIfNull(rsa, nameof(rsa));
        _rsa = rsa;
    }

    /// <summary>
    /// 使用指定的 XML 格式的密钥初始化 RSA 加密解密对象。
    /// 通过XML格式的密钥字符串构造Helper对象。
    /// </summary>
    /// <param name="key">XML 格式的RSA密钥,可以是公钥或私钥。</param>
    /// <exception cref="ArgumentException">当<paramref name="key"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当密钥格式无效时抛出。</exception>
    public RsaHelper(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        var rsa = RSA.Create();
        rsa.FromXmlString(key);
        _rsa = rsa;
    }

    /// <summary>
    /// 生成 RSA 密钥对。
    /// 生成新的RSA公私钥对,以XML格式返回。
    /// </summary>
    /// <returns>包含私钥和公钥的字典。键为"privateKey"和"publicKey"。</returns>
    /// <exception cref="CryptographicException">当密钥生成失败时抛出。</exception>
    public static Dictionary<string, string> Make()
    {
        var dic = new Dictionary<string, string>();
        var rsa = RSA.Create();
        dic["privateKey"] = rsa.ToXmlString(true);
        dic["publicKey"] = rsa.ToXmlString(false);
        return dic;
    }

    /// <summary>
    /// 使用公钥加密字符串内容。
    /// 将字符串明文用指定的公钥加密为Base64字符串。
    /// </summary>
    /// <param name="publicKey">RSA公钥,XML格式。</param>
    /// <param name="content">待加密的字符串内容。</param>
    /// <returns>加密后的内容，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="publicKey"/>或<paramref name="content"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当加密操作失败或公钥格式无效时抛出。</exception>
    public static string Encrypt(string publicKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Encrypt(publicKey, Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用当前对象的公钥加密字符串内容。
    /// 使用初始化时传入的RSA实例的公钥加密。
    /// </summary>
    /// <param name="content">待加密的字符串内容。</param>
    /// <returns>加密后的内容，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="content"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当加密操作失败时抛出。</exception>
    public string Encrypt(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Encrypt(Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用公钥加密字节数组内容。
    /// 将字节数组用指定的公钥加密。
    /// </summary>
    /// <param name="publicKey">RSA公钥,XML格式。</param>
    /// <param name="content">待加密的字节数组。</param>
    /// <returns>加密后的内容，以字节数组形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="publicKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="ArgumentNullException">当<paramref name="content"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当加密操作失败或公钥格式无效时抛出。</exception>
    public static byte[] Encrypt(string publicKey, byte[] content)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        var rsa = RSA.Create();
        rsa.FromXmlString(publicKey);
        var cipherBytes = rsa.Encrypt(content, RSAEncryptionPadding.Pkcs1);
        return cipherBytes;
    }

    /// <summary>
    /// 使用当前对象的公钥加密字节数组内容。
    /// 使用初始化时传入的RSA实例的公钥加密。
    /// </summary>
    /// <param name="content">待加密的字节数组。</param>
    /// <returns>加密后的内容，以字节数组形式返回。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="content"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当加密操作失败时抛出。</exception>
    public byte[] Encrypt(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        var cipherBytes = _rsa.Encrypt(content, RSAEncryptionPadding.Pkcs1);
        return cipherBytes;
    }

    /// <summary>
    /// 使用私钥解密 Base64 编码的字符串内容。
    /// 将Base64格式的密文用指定的私钥解密为明文字符串。
    /// </summary>
    /// <param name="privateKey">RSA私钥,XML格式。</param>
    /// <param name="content">Base64编码的待解密内容。</param>
    /// <returns>解密后的内容，以字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="privateKey"/>或<paramref name="content"/>为null或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当Base64字符串格式无效时抛出。</exception>
    /// <exception cref="CryptographicException">当解密操作失败或私钥格式无效时抛出。</exception>
    public static string Decrypt(string privateKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Decrypt(privateKey, Convert.FromBase64String(content));
        return Encoding.UTF8.GetString(res);
    }

    /// <summary>
    /// 使用私钥解密字节数组内容。
    /// 将字节数组密文用指定的私钥解密。
    /// </summary>
    /// <param name="privateKey">RSA私钥,XML格式。</param>
    /// <param name="content">待解密的字节数组。</param>
    /// <returns>解密后的内容，以字节数组形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="privateKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="ArgumentNullException">当<paramref name="content"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当解密操作失败或私钥格式无效时抛出。</exception>
    public static byte[] Decrypt(string privateKey, byte[] content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        var rsa = RSA.Create();
        rsa.FromXmlString(privateKey);
        var cipherBytes = rsa.Decrypt(content, RSAEncryptionPadding.Pkcs1);
        return cipherBytes;
    }

    /// <summary>
    /// 使用当前对象的私钥解密 Base64 编码的字符串内容。
    /// 使用初始化时传入的RSA实例的私钥解密Base64密文。
    /// </summary>
    /// <param name="content">Base64编码的待解密内容。</param>
    /// <returns>解密后的内容，以字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="content"/>为null或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当Base64字符串格式无效时抛出。</exception>
    /// <exception cref="CryptographicException">当解密操作失败时抛出。</exception>
    public string Decrypt(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Decrypt(Convert.FromBase64String(content));
        return Encoding.UTF8.GetString(res);
    }

    /// <summary>
    /// 使用当前对象的私钥解密字节数组内容。
    /// 使用初始化时传入的RSA实例的私钥解密。
    /// </summary>
    /// <param name="content">待解密的字节数组。</param>
    /// <returns>解密后的内容，以字节数组形式返回。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="content"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当解密操作失败时抛出。</exception>
    public byte[] Decrypt(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        var bytes = _rsa.Decrypt(content, RSAEncryptionPadding.Pkcs1);
        return bytes;
    }

    /// <summary>
    /// 使用私钥对数据进行签名。
    /// 使用SHA1算法计算数据的哈希值,然后用私钥对哈希值进行签名。
    /// </summary>
    /// <param name="dataToSign">待签名的数据。</param>
    /// <param name="privateKey">RSA私钥,XML格式。</param>
    /// <returns>签名后的数据，以字节数组形式返回。如果签名失败返回null。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dataToSign"/>为null时抛出。</exception>
    /// <exception cref="ArgumentException">当<paramref name="privateKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败或私钥格式无效时抛出。</exception>
    public static byte[] SignData(byte[] dataToSign, string privateKey)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));

        try
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(privateKey);
            return rsa.SignData(dataToSign, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 使用私钥对字符串数据进行签名。
    /// 将字符串转换为UTF8字节数组后进行签名。
    /// </summary>
    /// <param name="dataToSign">待签名的字符串数据。</param>
    /// <param name="privateKey">RSA私钥,XML格式。</param>
    /// <returns>签名后的数据，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="dataToSign"/>或<paramref name="privateKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败或私钥格式无效时抛出。</exception>
    public static string SignData(string dataToSign, string privateKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToSign, nameof(dataToSign));
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));

        var res = SignData(Encoding.UTF8.GetBytes(dataToSign), privateKey);
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用当前对象的私钥对数据进行签名。
    /// 使用初始化时传入的RSA实例的私钥进行签名。
    /// </summary>
    /// <param name="dataToSign">待签名的数据。</param>
    /// <returns>签名后的数据，以字节数组形式返回。如果签名失败返回null。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dataToSign"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败时抛出。</exception>
    public byte[] SignData(byte[] dataToSign)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));

        try
        {
            return _rsa.SignData(dataToSign, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 使用当前对象的私钥对字符串数据进行签名。
    /// 使用初始化时传入的RSA实例的私钥对UTF8编码的字符串进行签名。
    /// </summary>
    /// <param name="dataToSign">待签名的字符串数据。</param>
    /// <returns>签名后的数据，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当<paramref name="dataToSign"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败时抛出。</exception>
    public string SignData(string dataToSign)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToSign, nameof(dataToSign));

        var res = SignData(Encoding.UTF8.GetBytes(dataToSign));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用公钥验证数据签名。
    /// 验证使用私钥签名的数据是否有效。
    /// </summary>
    /// <param name="dataToVerify">原始数据。</param>
    /// <param name="signedData">签名数据。</param>
    /// <param name="publicKey">RSA公钥,XML格式。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。验证过程出现异常也返回false。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dataToVerify"/>或<paramref name="signedData"/>为null时抛出。</exception>
    /// <exception cref="ArgumentException">当<paramref name="publicKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当验证操作失败或公钥格式无效时抛出。</exception>
    public static bool VerifyData(byte[] dataToVerify, byte[] signedData, string publicKey)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));

        try
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);
            return rsa.VerifyData(dataToVerify, signedData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 使用公钥验证字符串数据的签名。
    /// 验证Base64格式的签名数据是否与原始字符串匹配。
    /// </summary>
    /// <param name="dataToVerify">原始字符串数据。</param>
    /// <param name="signedData">Base64编码的签名数据。</param>
    /// <param name="publicKey">RSA公钥,XML格式。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentException">当任何参数为null或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当Base64字符串格式无效时抛出。</exception>
    /// <exception cref="CryptographicException">当验证操作失败或公钥格式无效时抛出。</exception>
    public static bool RsaVerifyData(string dataToVerify, string signedData, string publicKey)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));

        return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData), publicKey);
    }

    /// <summary>
    /// 使用当前对象的公钥验证数据签名。
    /// 使用初始化时传入的RSA实例的公钥验证签名。
    /// </summary>
    /// <param name="dataToVerify">原始数据。</param>
    /// <param name="signedData">签名数据。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。验证过程出现异常也返回false。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dataToVerify"/>或<paramref name="signedData"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当验证操作失败时抛出。</exception>
    public bool VerifyData(byte[] dataToVerify, byte[] signedData)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));

        try
        {
            return _rsa.VerifyData(dataToVerify, signedData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 使用当前对象的公钥验证字符串数据的签名。
    /// 使用初始化时传入的RSA实例的公钥验证Base64格式的签名。
    /// </summary>
    /// <param name="dataToVerify">原始字符串数据。</param>
    /// <param name="signedData">Base64编码的签名数据。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。验证过程出现异常也返回false。</returns>
    /// <exception cref="ArgumentException">当<paramref name="dataToVerify"/>或<paramref name="signedData"/>为null或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当Base64字符串格式无效时抛出。</exception>
    /// <exception cref="CryptographicException">当验证操作失败时抛出。</exception>
    public bool VerifyData(string dataToVerify, string signedData)
    {
        ArgumentException.ThrowIfNullOrEmpty(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));

        try
        {
            return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData));
        }
        catch
        {
            return false;
        }
    }
}