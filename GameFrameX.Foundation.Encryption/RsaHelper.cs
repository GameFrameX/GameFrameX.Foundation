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
    /// 使用私钥对数据进行签名（SHA256 哈希算法）。
    /// 本方法支持 PKCS#8 与 PKCS#1 两种私钥格式，自动识别并导入。
    /// 签名过程采用 SHA256 哈希算法与 PKCS#1 填充模式，确保数据完整性与不可否认性。
    /// </summary>
    /// <remarks>
    /// 调用过程：
    /// 1. 将待签名内容按 UTF-8 编码转为字节数组；
    /// 2. 使用 RSA 私钥对哈希值进行签名；
    /// 3. 返回 Base64 编码的签名结果，便于网络传输与存储。
    /// 
    /// 异常处理：
    /// - 私钥格式非法或导入失败时抛出 <see cref="CryptographicException"/>；
    /// - 参数为 null 或空字符串时抛出 <see cref="ArgumentException"/>。
    /// </remarks>
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
            // 尝试导入 PKCS#8 格式私钥
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        }
        catch (CryptographicException)
        {
            // 尝试导入 PKCS#1 格式私钥
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        }


        var data = Encoding.UTF8.GetBytes(content);
        var signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 使用公钥验证数据签名（SHA256 哈希算法）。
    /// 本方法支持 SubjectPublicKeyInfo（PKCS#8）与 RSAPublicKey（PKCS#1）两种公钥格式，自动识别并导入。
    /// 验签过程采用 SHA256 哈希算法与 PKCS#1 填充模式，确保签名真实性与数据完整性。
    /// </summary>
    /// <remarks>
    /// 调用过程：
    /// 1. 将待验签内容按 UTF-8 编码转为字节数组；
    /// 2. 将 Base64 格式的签名解码为字节数组；
    /// 3. 使用 RSA 公钥对签名进行验证；
    /// 4. 返回验签结果，true 表示签名有效，false 表示签名无效。
    /// 
    /// 异常处理：
    /// - 公钥格式非法或导入失败时抛出 <see cref="CryptographicException"/>；
    /// - 参数为 null 或空字符串时抛出 <see cref="ArgumentException"/>；
    /// - 签名不是合法 Base64 字符串时抛出 <see cref="FormatException"/>。
    /// </remarks>
    /// <param name="publicKey">Base64 格式的公钥字符串，支持 SubjectPublicKeyInfo（PKCS#8）与 RSAPublicKey（PKCS#1）两种编码。</param>
    /// <param name="content">待验签的明文字符串，将使用 UTF-8 编码转换为字节数组。</param>
    /// <param name="sign">Base64 格式的签名结果，需与 <see cref="Sign"/> 方法生成的格式保持一致。</param>
    /// <returns>验签结果，true 表示签名有效，false 表示签名无效。</returns>
    /// <exception cref="ArgumentException">当 <paramref name="publicKey"/>、<paramref name="content"/> 或 <paramref name="sign"/> 为 null 或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当 <paramref name="sign"/> 不是合法的 Base64 字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当公钥格式非法或验签过程失败时抛出。</exception>
    public static bool Verify(string publicKey, string content, string sign)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(sign, nameof(sign));

        using var rsa = RSA.Create();
        try
        {
            // 尝试导入 SubjectPublicKeyInfo 格式公钥
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
        }
        catch (CryptographicException)
        {
            // 尝试导入 RSAPublicKey (PKCS#1) 格式公钥
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        }


        var data = Encoding.UTF8.GetBytes(content);
        var signature = Convert.FromBase64String(sign);
        return rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    /// <summary>
    /// 使用公钥加密数据（支持Base64格式公钥）
    /// </summary>
    /// <remarks>
    /// 本方法支持以下两种公钥格式：
    /// 1. SubjectPublicKeyInfo（PKCS#8）格式
    /// 2. RSAPublicKey（PKCS#1）格式
    /// 当数据长度超过密钥长度限制时，自动采用分块加密。
    /// 加密后的结果采用Base64编码返回，便于网络传输和存储。
    /// </remarks>
    /// <param name="publicKey">Base64 格式的公钥字符串，支持 PKCS#1 与 PKCS#8 两种编码</param>
    /// <param name="content">待加密的明文字符串，将使用 UTF-8 编码转换为字节数组</param>
    /// <returns>Base64 格式的加密结果，可直接用于网络传输或持久化存储</returns>
    /// <exception cref="ArgumentException">当 publicKey 或 content 为 null 或空字符串时抛出</exception>
    /// <exception cref="CryptographicException">当公钥格式非法或加密过程失败时抛出</exception>
    public static string EncryptBase64(string publicKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(publicKey, nameof(publicKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        using var rsa = RSA.Create();
        try
        {
            // 导入公钥
            // 注意：这里假设公钥是 PKCS#8 或 SubjectPublicKeyInfo 格式
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);

            // 将内容转换为字节数组
            var dataToEncrypt = Encoding.UTF8.GetBytes(content);

            // 计算最大加密块大小 (KeySize / 8 - 11 for PKCS1)
            // 1024位密钥 -> 128字节 -> max 117字节
            int bufferSize = (rsa.KeySize / 8) - 11;

            // 使用内存流存储加密后的数据
            using var outputStream = new MemoryStream();

            int offset = 0;
            while (offset < dataToEncrypt.Length)
            {
                int currentBlockSize = Math.Min(bufferSize, dataToEncrypt.Length - offset);
                var chunk = new byte[currentBlockSize];
                Array.Copy(dataToEncrypt, offset, chunk, 0, currentBlockSize);

                var encryptedChunk = rsa.Encrypt(chunk, RSAEncryptionPadding.Pkcs1);
                outputStream.Write(encryptedChunk, 0, encryptedChunk.Length);

                offset += currentBlockSize;
            }

            return Convert.ToBase64String(outputStream.ToArray());
        }
        catch (CryptographicException ex)
        {
            // 如果导入失败，尝试作为 RSAPublicKey (PKCS#1) 导入
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            var dataToEncrypt = Encoding.UTF8.GetBytes(content);

            int bufferSize = (rsa.KeySize / 8) - 11;
            using var outputStream = new MemoryStream();

            int offset = 0;
            while (offset < dataToEncrypt.Length)
            {
                int currentBlockSize = Math.Min(bufferSize, dataToEncrypt.Length - offset);
                var chunk = new byte[currentBlockSize];
                Array.Copy(dataToEncrypt, offset, chunk, 0, currentBlockSize);

                var encryptedChunk = rsa.Encrypt(chunk, RSAEncryptionPadding.Pkcs1);
                outputStream.Write(encryptedChunk, 0, encryptedChunk.Length);

                offset += currentBlockSize;
            }

            return Convert.ToBase64String(outputStream.ToArray());
        }
    }

    /// <summary>
    /// 使用私钥解密数据（支持Base64格式私钥）
    /// </summary>
    /// <remarks>
    /// 本方法支持以下两种私钥格式：
    /// 1. PKCS#8 格式私钥
    /// 2. PKCS#1 格式私钥
    /// 当密文长度超过密钥长度时，自动采用分块解密。
    /// 输入的密文需为Base64编码，解密后返回明文字符串。
    /// </remarks>
    /// <param name="privateKey">Base64 格式的私钥字符串，支持 PKCS#1 与 PKCS#8 两种编码</param>
    /// <param name="content">Base64 格式的加密内容，需与 EncryptBase64 方法生成的格式保持一致</param>
    /// <returns>解密后的明文字符串，使用 UTF-8 编码还原</returns>
    /// <exception cref="ArgumentException">当 privateKey 或 content 为 null 或空字符串时抛出</exception>
    /// <exception cref="FormatException">当 content 不是合法的 Base64 字符串时抛出</exception>
    /// <exception cref="CryptographicException">当私钥格式非法或解密过程失败时抛出</exception>
    public static string DecryptBase64(string privateKey, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(privateKey, nameof(privateKey));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        using var rsa = RSA.Create();
        try
        {
            // 尝试导入 PKCS#8 格式私钥
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        }
        catch (CryptographicException)
        {
            // 尝试导入 PKCS#1 格式私钥
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        }

        var dataToDecrypt = Convert.FromBase64String(content);

        // RSA 解密块大小等于密钥长度（字节）
        int bufferSize = rsa.KeySize / 8;
        using var outputStream = new MemoryStream();

        int offset = 0;
        while (offset < dataToDecrypt.Length)
        {
            int currentBlockSize = Math.Min(bufferSize, dataToDecrypt.Length - offset);
            var chunk = new byte[currentBlockSize];
            Array.Copy(dataToDecrypt, offset, chunk, 0, currentBlockSize);

            var decryptedChunk = rsa.Decrypt(chunk, RSAEncryptionPadding.Pkcs1);
            outputStream.Write(decryptedChunk, 0, decryptedChunk.Length);

            offset += currentBlockSize;
        }

        return Encoding.UTF8.GetString(outputStream.ToArray());
    }

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
    /// <exception cref="ArgumentNullException">当<paramref name="dataToSign"/>为null时抛出。</exception>
    /// <exception cref="ArgumentException">当<paramref name="privateKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败或私钥格式无效时抛出。</exception>
    public static string SignData(string dataToSign, string privateKey)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
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
    /// <exception cref="ArgumentNullException">当<paramref name="dataToSign"/>为null时抛出。</exception>
    /// <exception cref="CryptographicException">当签名操作失败时抛出。</exception>
    public string SignData(string dataToSign)
    {
        ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));

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
    /// <exception cref="ArgumentNullException">当<paramref name="dataToVerify"/>为null时抛出。</exception>
    /// <exception cref="ArgumentException">当<paramref name="signedData"/>或<paramref name="publicKey"/>为null或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当Base64字符串格式无效时抛出。</exception>
    /// <exception cref="CryptographicException">当验证操作失败或公钥格式无效时抛出。</exception>
    public static bool RsaVerifyData(string dataToVerify, string signedData, string publicKey)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
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
    /// <exception cref="ArgumentNullException">当<paramref name="dataToVerify"/>为null时抛出。</exception>
    /// <exception cref="ArgumentException">当<paramref name="signedData"/>为null或空字符串时抛出。</exception>
    /// <exception cref="FormatException">当Base64字符串格式无效时抛出。</exception>
    /// <exception cref="CryptographicException">当验证操作失败时抛出。</exception>
    public bool VerifyData(string dataToVerify, string signedData)
    {
        ArgumentNullException.ThrowIfNull(dataToVerify, nameof(dataToVerify));
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