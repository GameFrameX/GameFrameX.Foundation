using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// 提供 RSA 加密解密功能。
/// 本类封装了RSA非对称加密算法的常用操作,包括加密、解密、签名和验签等功能。
/// 支持XML格式的密钥对导入导出。
/// </summary>
/// <remarks>
/// 实例方法持有非托管 RSA 资源，使用完毕后请调用 <see cref="Dispose"/> 释放，
/// 或通过 <c>using</c> 语句管理生命周期。
/// </remarks>
public sealed class RsaHelper : IDisposable
{
    /// <summary>
    /// 使用私钥对数据进行签名（SHA256 哈希算法）。
    /// 本方法支持 PKCS#8 与 PKCS#1 两种私钥格式，自动识别并导入。
    /// 签名过程采用 SHA256 哈希算法与 PKCS#1 填充模式，确保数据完整性与不可否认性。
    /// </summary>
    /// <param name="privateKey">Base64 格式的私钥字符串，支持 PKCS#8 与 PKCS#1 两种编码。</param>
    /// <param name="content">待签名的明文字符串，将使用 UTF-8 编码转换为字节数组。</param>
    /// <returns>Base64 格式的签名结果，可直接用于网络传输或持久化存储。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="privateKey"/> 或 <paramref name="content"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当私钥格式非法或签名过程失败时抛出。</exception>
    public static string Sign(string privateKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        using var rsa = RSA.Create();
        try
        {
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        }
        catch (CryptographicException)
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        }

        var data = Encoding.UTF8.GetBytes(content);
        var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 使用公钥验证数据签名（SHA256 哈希算法）。
    /// 本方法支持 SubjectPublicKeyInfo（PKCS#8）与 RSAPublicKey（PKCS#1）两种公钥格式，自动识别并导入。
    /// </summary>
    /// <param name="publicKey">Base64 格式的公钥字符串。</param>
    /// <param name="content">待验签的明文字符串。</param>
    /// <param name="sign">Base64 格式的签名结果。</param>
    /// <returns>验签结果，true 表示签名有效，false 表示签名无效。</returns>
    /// <exception cref="ArgumentException">当任意参数为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当公钥格式非法或验签过程失败时抛出。</exception>
    public static bool Verify(string publicKey, string content, string sign)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(sign, nameof(sign));

        using var rsa = RSA.Create();
        try
        {
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
        }
        catch (CryptographicException)
        {
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        }

        var data = Encoding.UTF8.GetBytes(content);
        var signature = Convert.FromBase64String(sign);
        return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用公钥加密数据（支持 Base64 格式公钥，OAEP-SHA256 填充）。
    /// </summary>
    /// <remarks>
    /// 支持以下两种公钥格式：
    /// 1. SubjectPublicKeyInfo（PKCS#8）格式
    /// 2. RSAPublicKey（PKCS#1）格式
    /// 当数据长度超过密钥限制时自动分块加密。
    /// 填充方式为 OAEP-SHA256（C-05 修复，替代不安全的 PKCS1v1.5）。
    /// </remarks>
    /// <param name="publicKey">Base64 格式的公钥字符串</param>
    /// <param name="content">待加密的明文字符串</param>
    /// <returns>Base64 格式的加密结果</returns>
    /// <exception cref="ArgumentException">当参数为 null 或空字符串时抛出</exception>
    /// <exception cref="CryptographicException">当公钥格式非法或加密失败时抛出</exception>
    public static string EncryptBase64(string publicKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        using var rsa = RSA.Create();
        try
        {
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
        }
        catch (CryptographicException)
        {
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        }

        var dataToEncrypt = Encoding.UTF8.GetBytes(content);

        // OAEP-SHA256 最大明文块：KeySize/8 - 2*32 - 2（C-05 修复）
        int bufferSize = (rsa.KeySize / 8) - 66;
        using var outputStream = new MemoryStream();

        int offset = 0;
        while (offset < dataToEncrypt.Length)
        {
            int currentBlockSize = Math.Min(bufferSize, dataToEncrypt.Length - offset);
            var chunk = new byte[currentBlockSize];
            Array.Copy(dataToEncrypt, offset, chunk, 0, currentBlockSize);
            // C-05 修复：使用 OaepSHA256 替代 Pkcs1（Bleichenbacher 攻击防护）
            var encryptedChunk = rsa.Encrypt(chunk, RSAEncryptionPadding.OaepSHA256);
            outputStream.Write(encryptedChunk, 0, encryptedChunk.Length);
            offset += currentBlockSize;
        }

        return Convert.ToBase64String(outputStream.ToArray());
    }

    /// <summary>
    /// 使用私钥解密数据（支持 Base64 格式私钥，OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="privateKey">Base64 格式的私钥字符串，支持 PKCS#1 与 PKCS#8 两种编码</param>
    /// <param name="content">Base64 格式的加密内容，需与 <see cref="EncryptBase64"/> 方法生成的格式保持一致</param>
    /// <returns>解密后的明文字符串</returns>
    /// <exception cref="ArgumentException">当参数为 null 或空字符串时抛出</exception>
    /// <exception cref="CryptographicException">当私钥格式非法或解密失败时抛出</exception>
    public static string DecryptBase64(string privateKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        using var rsa = RSA.Create();
        try
        {
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        }
        catch (CryptographicException)
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        }

        var dataToDecrypt = Convert.FromBase64String(content);

        int bufferSize = rsa.KeySize / 8;
        using var outputStream = new MemoryStream();

        int offset = 0;
        while (offset < dataToDecrypt.Length)
        {
            int currentBlockSize = Math.Min(bufferSize, dataToDecrypt.Length - offset);
            var chunk = new byte[currentBlockSize];
            Array.Copy(dataToDecrypt, offset, chunk, 0, currentBlockSize);
            // C-05 修复：使用 OaepSHA256
            var decryptedChunk = rsa.Decrypt(chunk, RSAEncryptionPadding.OaepSHA256);
            outputStream.Write(decryptedChunk, 0, decryptedChunk.Length);
            offset += currentBlockSize;
        }

        return Encoding.UTF8.GetString(outputStream.ToArray());
    }

    // ── 实例成员 ─────────────────────────────────────────────────────────────

    /// <summary>RSA算法提供程序实例</summary>
    private readonly RSA _rsa;
    private bool _disposed;

    /// <summary>
    /// 使用指定的 RSA 对象初始化实例。
    /// </summary>
    /// <param name="rsa">RSA 对象，不能为 null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="rsa"/> 为 null 时抛出。</exception>
    public RsaHelper(RSA rsa)
    {
        ArgumentNullException.ThrowIfNull(rsa, nameof(rsa));
        _rsa = rsa;
    }

    /// <summary>
    /// 使用指定的 XML 格式密钥初始化实例。
    /// </summary>
    /// <param name="key">XML 格式的 RSA 密钥，可以是公钥或私钥。</param>
    /// <exception cref="ArgumentException">当 <paramref name="key"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当密钥格式无效时抛出。</exception>
    public RsaHelper(string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        // C-12 修复：构造失败时确保 RSA 实例被释放，不发生资源泄漏
        var rsa = RSA.Create();
        try
        {
            rsa.FromXmlString(key);
        }
        catch
        {
            rsa.Dispose();
            throw;
        }

        _rsa = rsa;
    }

    /// <summary>释放 RSA 非托管资源。</summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _rsa.Dispose();
            _disposed = true;
        }
    }

    /// <summary>
    /// 生成 RSA 密钥对。
    /// </summary>
    /// <returns>包含私钥（"privateKey"）和公钥（"publicKey"）的字典，均为 XML 格式。</returns>
    public static Dictionary<string, string> Make()
    {
        // C-10 修复：使用 using 确保 RSA 实例被释放
        using var rsa = RSA.Create();
        return new Dictionary<string, string>
        {
            ["privateKey"] = rsa.ToXmlString(true),
            ["publicKey"] = rsa.ToXmlString(false)
        };
    }

    /// <summary>
    /// 使用公钥（XML 格式）加密字符串内容（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="publicKey">RSA 公钥，XML 格式。</param>
    /// <param name="content">待加密的字符串内容。</param>
    /// <returns>加密后的内容，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当参数为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当加密失败或公钥格式无效时抛出。</exception>
    public static string Encrypt(string publicKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Encrypt(publicKey, Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用当前实例的公钥加密字符串内容（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="content">待加密的字符串内容。</param>
    /// <returns>加密后的内容，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="content"/> 为 null 或空字符串时抛出。</exception>
    public string Encrypt(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Encrypt(Encoding.UTF8.GetBytes(content));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用公钥（XML 格式）加密字节数组内容（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="publicKey">RSA 公钥，XML 格式。</param>
    /// <param name="content">待加密的字节数组。</param>
    /// <returns>加密后的字节数组。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="publicKey"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 <paramref name="content"/> 为 null 时抛出。</exception>
    /// <exception cref="CryptographicException">当加密失败或公钥格式无效时抛出。</exception>
    public static byte[] Encrypt(string publicKey, byte[] content)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        // C-10 修复：using 确保 RSA 实例被释放
        using var rsa = RSA.Create();
        rsa.FromXmlString(publicKey);
        // C-05 修复：OaepSHA256 替代 Pkcs1
        return rsa.Encrypt(content, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// 使用当前实例的公钥加密字节数组内容（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="content">待加密的字节数组。</param>
    /// <returns>加密后的字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="content"/> 为 null 时抛出。</exception>
    public byte[] Encrypt(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        // C-05 修复：OaepSHA256 替代 Pkcs1
        return _rsa.Encrypt(content, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// 使用私钥（XML 格式）解密 Base64 编码的字符串（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="privateKey">RSA 私钥，XML 格式。</param>
    /// <param name="content">Base64 编码的待解密内容。</param>
    /// <returns>解密后的明文字符串。</returns>
    /// <exception cref="ArgumentException">当参数为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当解密失败或私钥格式无效时抛出。</exception>
    public static string Decrypt(string privateKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Decrypt(privateKey, Convert.FromBase64String(content));
        return Encoding.UTF8.GetString(res);
    }

    /// <summary>
    /// 使用私钥（XML 格式）解密字节数组（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="privateKey">RSA 私钥，XML 格式。</param>
    /// <param name="content">待解密的字节数组。</param>
    /// <returns>解密后的明文字节数组。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="privateKey"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="ArgumentNullException">当 <paramref name="content"/> 为 null 时抛出。</exception>
    /// <exception cref="CryptographicException">当解密失败或私钥格式无效时抛出。</exception>
    public static byte[] Decrypt(string privateKey, byte[] content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        // C-10 修复：using 确保 RSA 实例被释放
        using var rsa = RSA.Create();
        rsa.FromXmlString(privateKey);
        // C-05 修复：OaepSHA256 替代 Pkcs1
        return rsa.Decrypt(content, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// 使用当前实例的私钥解密 Base64 编码的字符串（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="content">Base64 编码的待解密内容。</param>
    /// <returns>解密后的明文字符串。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="content"/> 为 null 或空字符串时抛出。</exception>
    public string Decrypt(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var res = Decrypt(Convert.FromBase64String(content));
        return Encoding.UTF8.GetString(res);
    }

    /// <summary>
    /// 使用当前实例的私钥解密字节数组（OAEP-SHA256 填充）。
    /// </summary>
    /// <param name="content">待解密的字节数组。</param>
    /// <returns>解密后的明文字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="content"/> 为 null 时抛出。</exception>
    public byte[] Decrypt(byte[] content)
    {
        ArgumentNullException.ThrowIfNull(content, nameof(content));

        // C-05 修复：OaepSHA256 替代 Pkcs1
        return _rsa.Decrypt(content, RSAEncryptionPadding.OaepSHA256);
    }

    /// <summary>
    /// 使用私钥（XML 格式）对数据进行签名（SHA256）。
    /// </summary>
    /// <param name="dataToSign">待签名的数据。</param>
    /// <param name="privateKey">RSA 私钥，XML 格式。</param>
    /// <returns>签名后的字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToSign"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当 <paramref name="privateKey"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当签名失败或私钥格式无效时抛出。</exception>
    public static byte[] SignData(byte[] dataToSign, string privateKey)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));

        // C-10 修复：using 确保释放；C-04 修复：SHA256；C-01(同类) 修复：不再静默返回 null
        using var rsa = RSA.Create();
        rsa.FromXmlString(privateKey);
        return rsa.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用私钥（XML 格式）对字符串数据进行签名（SHA256）。
    /// </summary>
    /// <param name="dataToSign">待签名的字符串数据。</param>
    /// <param name="privateKey">RSA 私钥，XML 格式。</param>
    /// <returns>签名后的数据，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToSign"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当 <paramref name="privateKey"/> 为 null 或空字符串时抛出。</exception>
    public static string SignData(string dataToSign, string privateKey)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));

        var res = SignData(Encoding.UTF8.GetBytes(dataToSign), privateKey);
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用当前实例的私钥对数据进行签名（SHA256）。
    /// </summary>
    /// <param name="dataToSign">待签名的数据。</param>
    /// <returns>签名后的字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToSign"/> 为 null 时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败时抛出。</exception>
    public byte[] SignData(byte[] dataToSign)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));

        // C-04 修复：SHA1 → SHA256；C-01(同类) 修复：不再静默返回 null
        return _rsa.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用当前实例的私钥对字符串数据进行签名（SHA256）。
    /// </summary>
    /// <param name="dataToSign">待签名的字符串数据。</param>
    /// <returns>签名后的数据，以 Base64 编码的字符串形式返回。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToSign"/> 为 null 时抛出。</exception>
    public string SignData(string dataToSign)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));

        var res = SignData(Encoding.UTF8.GetBytes(dataToSign));
        return Convert.ToBase64String(res);
    }

    /// <summary>
    /// 使用公钥（XML 格式）验证数据签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">原始数据。</param>
    /// <param name="signedData">签名数据。</param>
    /// <param name="publicKey">RSA 公钥，XML 格式。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToVerify"/> 或 <paramref name="signedData"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当 <paramref name="publicKey"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当公钥格式无效时抛出。</exception>
    public static bool VerifyData(byte[] dataToVerify, byte[] signedData, string publicKey)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));

        // C-10 修复：using；C-04 修复：SHA256
        using var rsa = RSA.Create();
        rsa.FromXmlString(publicKey);
        return rsa.VerifyData(dataToVerify, signedData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用公钥（XML 格式）验证字符串数据的签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">原始字符串数据。</param>
    /// <param name="signedData">Base64 编码的签名数据。</param>
    /// <param name="publicKey">RSA 公钥，XML 格式。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。</returns>
    public static bool RsaVerifyData(string dataToVerify, string signedData, string publicKey)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));

        return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData), publicKey);
    }

    /// <summary>
    /// 使用当前实例的公钥验证数据签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">原始数据。</param>
    /// <param name="signedData">签名数据。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当任意参数为 null 时抛出。</exception>
    public bool VerifyData(byte[] dataToVerify, byte[] signedData)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentNullException.ThrowIfNull(signedData, nameof(signedData));

        // C-04 修复：SHA1 → SHA256
        return _rsa.VerifyData(dataToVerify, signedData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用当前实例的公钥验证字符串数据的签名（SHA256）。
    /// </summary>
    /// <param name="dataToVerify">原始字符串数据。</param>
    /// <param name="signedData">Base64 编码的签名数据。</param>
    /// <returns>如果签名有效则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dataToVerify"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当 <paramref name="signedData"/> 为 null 或空字符串时抛出。</exception>
    public bool VerifyData(string dataToVerify, string signedData)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
        ArgumentException.ThrowIfNullOrEmpty(signedData, nameof(signedData));

        return VerifyData(Encoding.UTF8.GetBytes(dataToVerify), Convert.FromBase64String(signedData));
    }
}
