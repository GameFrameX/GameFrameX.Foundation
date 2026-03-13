using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// AES 加密解密
/// </summary>
/// <remarks>
/// 加密输出格式：[Salt(16 字节) | IV(16 字节) | 密文]
/// Salt 和 IV 每次随机生成，与密文拼接存储，解密时自动从密文头部读取。
/// 此格式与旧版本（固定 IV/Salt）不兼容。
/// </remarks>
public static class AesHelper
{
    /// <summary>PBKDF2 迭代次数（符合 OWASP 2023 建议 600,000 次）</summary>
    private const int Pbkdf2Iterations = 600_000;

    /// <summary>Salt 长度（字节）</summary>
    private const int SaltSize = 16;

    /// <summary>IV 长度（字节）</summary>
    private const int IvSize = 16;

    /// <summary>输出头部长度 = Salt + IV</summary>
    private const int HeaderSize = SaltSize + IvSize;

    /// <summary>
    /// 使用 AES 算法加密字符串（输出 Base64 编码）
    /// </summary>
    /// <param name="encryptString">待加密的明文字符串</param>
    /// <param name="encryptKey">加密密钥</param>
    /// <returns>加密后的 Base64 编码字符串，格式为 [Salt(16) | IV(16) | 密文] 的 Base64 表示</returns>
    /// <exception cref="ArgumentException">当明文或密钥为空时抛出</exception>
    public static string Encrypt(string encryptString, string encryptKey)
    {
        if (string.IsNullOrEmpty(encryptString))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PlainTextCannotBeNullOrEmpty), nameof(encryptString));
        }

        if (string.IsNullOrEmpty(encryptKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.EncryptionKeyCannotBeNullOrEmpty), nameof(encryptKey));
        }

        return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(encryptString), encryptKey));
    }

    /// <summary>
    /// 使用 AES-CBC 算法加密字节数组。
    /// 每次加密随机生成 Salt 和 IV，输出格式：[Salt(16 字节) | IV(16 字节) | 密文]。
    /// </summary>
    /// <param name="encryptByte">待加密的明文字节数组</param>
    /// <param name="encryptKey">加密密钥，用于通过 PBKDF2(600,000 次) 派生密钥</param>
    /// <returns>加密后的字节数组，头部包含 Salt 和 IV</returns>
    /// <exception cref="ArgumentNullException">当明文字节数组为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当明文字节数组为空或密钥为空时抛出</exception>
    /// <exception cref="CryptographicException">当加密过程失败时抛出</exception>
    public static byte[] Encrypt(byte[] encryptByte, string encryptKey)
    {
        if (encryptByte == null)
        {
            throw new ArgumentNullException(nameof(encryptByte), @"Plain text byte array cannot be null");
        }

        if (encryptByte.Length == 0)
        {
            throw new ArgumentException("Plain text byte array cannot be empty", nameof(encryptByte));
        }

        if (string.IsNullOrEmpty(encryptKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.EncryptionKeyCannotBeNullOrEmpty), nameof(encryptKey));
        }

        // 每次加密随机生成 Salt 和 IV，确保语义安全（C-02 修复）
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var iv = RandomNumberGenerator.GetBytes(IvSize);

        using var aesProvider = Aes.Create();
        // PBKDF2 迭代次数提升至 600,000（C-03 修复）；使用推荐的静态 Pbkdf2 方法（.NET 10+）
        var keyBytes = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(encryptKey), salt, Pbkdf2Iterations, HashAlgorithmName.SHA256, 32);

        using var encryptor = aesProvider.CreateEncryptor(keyBytes, iv);
        using var memoryStream = new MemoryStream();

        // 将 Salt 和 IV 写入输出头部，解密时从此处读取
        memoryStream.Write(salt, 0, SaltSize);
        memoryStream.Write(iv, 0, IvSize);

        // 使用 CryptoStream 写入密文（C-01 修复：不再捕获异常，让其自然传播）
        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(encryptByte, 0, encryptByte.Length);
            cryptoStream.FlushFinalBlock();
        }

        return memoryStream.ToArray();
    }

    /// <summary>
    /// 使用 AES 算法解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的 Base64 编码字符串（格式：[Salt(16) | IV(16) | 密文] 的 Base64 表示）</param>
    /// <param name="decryptKey">解密密钥，必须与加密时使用的密钥相同</param>
    /// <returns>解密后的明文字符串</returns>
    /// <exception cref="ArgumentException">当密文或密钥为空时抛出</exception>
    /// <exception cref="CryptographicException">当解密失败（如密钥错误或数据被篡改）时抛出</exception>
    public static string Decrypt(string decryptString, string decryptKey)
    {
        if (string.IsNullOrEmpty(decryptString))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.CipherTextCannotBeNullOrEmpty), nameof(decryptString));
        }

        if (string.IsNullOrEmpty(decryptKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.DecryptionKeyCannotBeNullOrEmpty), nameof(decryptKey));
        }

        return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(decryptString), decryptKey));
    }

    /// <summary>
    /// 使用 AES-CBC 算法解密字节数组。
    /// 期望输入格式：[Salt(16 字节) | IV(16 字节) | 密文]，与 <see cref="Encrypt(byte[],string)"/> 输出格式对应。
    /// </summary>
    /// <param name="decryptByte">待解密的密文字节数组，头部须包含 Salt 和 IV</param>
    /// <param name="decryptKey">解密密钥，必须与加密时使用的密钥相同</param>
    /// <returns>解密后的明文字节数组</returns>
    /// <exception cref="ArgumentNullException">当密文字节数组为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当密文长度不足或密钥为空时抛出</exception>
    /// <exception cref="CryptographicException">当解密失败（如密钥错误或数据被篡改）时抛出</exception>
    public static byte[] Decrypt(byte[] decryptByte, string decryptKey)
    {
        if (decryptByte == null)
        {
            throw new ArgumentNullException(nameof(decryptByte), @"Cipher text byte array cannot be null");
        }

        // 最小长度：Salt(16) + IV(16) + 至少一个完整加密块(16)
        if (decryptByte.Length < HeaderSize + 16)
        {
            throw new ArgumentException("Cipher text byte array is too short to be valid.", nameof(decryptByte));
        }

        if (string.IsNullOrEmpty(decryptKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.DecryptionKeyCannotBeNullOrEmpty), nameof(decryptKey));
        }

        // 从密文头部读取 Salt 和 IV（C-02 修复）
        var salt = new byte[SaltSize];
        var iv = new byte[IvSize];
        Array.Copy(decryptByte, 0, salt, 0, SaltSize);
        Array.Copy(decryptByte, SaltSize, iv, 0, IvSize);

        int cipherOffset = HeaderSize;
        int cipherLength = decryptByte.Length - cipherOffset;
        var cipherData = new byte[cipherLength];
        Array.Copy(decryptByte, cipherOffset, cipherData, 0, cipherLength);

        using var aesProvider = Aes.Create();
        // PBKDF2 迭代次数与加密时一致（C-03 修复）；使用推荐的静态 Pbkdf2 方法（.NET 10+）
        var keyBytes = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(decryptKey), salt, Pbkdf2Iterations, HashAlgorithmName.SHA256, 32);

        using var decryptor = aesProvider.CreateDecryptor(keyBytes, iv);
        using var memoryStream = new MemoryStream();

        // C-01 修复：不再捕获异常，解密失败（密钥错误/数据篡改）时自然抛出 CryptographicException
        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
        {
            cryptoStream.Write(cipherData, 0, cipherData.Length);
            cryptoStream.FlushFinalBlock();
        }

        return memoryStream.ToArray();
    }
}
