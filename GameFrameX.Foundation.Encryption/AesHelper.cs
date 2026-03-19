// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// AES 加密解密工具类，提供基于 AES-CBC 算法的加密和解密功能。
/// </summary>
/// <remarks>
/// AES encryption and decryption utility class, providing encryption and decryption based on AES-CBC algorithm.
/// 加密输出格式：[Salt(16 字节) | IV(16 字节) | 密文]
/// Encryption output format: [Salt(16 bytes) | IV(16 bytes) | Ciphertext]
/// Salt 和 IV 每次随机生成，与密文拼接存储，解密时自动从密文头部读取。
/// Salt and IV are randomly generated each time, concatenated with the ciphertext for storage, and automatically read from the ciphertext header during decryption.
/// 此格式与旧版本（固定 IV/Salt）不兼容。
/// This format is incompatible with older versions (fixed IV/Salt).
/// </remarks>
public static class AesHelper
{
    /// <summary>
    /// PBKDF2 迭代次数（符合 OWASP 2023 建议 600,000 次）。
    /// </summary>
    /// <remarks>
    /// PBKDF2 iteration count (compliant with OWASP 2023 recommendation of 600,000).
    /// </remarks>
    private const int Pbkdf2Iterations = 600_000;

    /// <summary>
    /// Salt 长度（字节）。
    /// </summary>
    /// <remarks>
    /// Salt length in bytes.
    /// </remarks>
    private const int SaltSize = 16;

    /// <summary>
    /// IV 长度（字节）。
    /// </summary>
    /// <remarks>
    /// IV length in bytes.
    /// </remarks>
    private const int IvSize = 16;

    /// <summary>
    /// 输出头部长度 = Salt + IV。
    /// </summary>
    /// <remarks>
    /// Output header length = Salt + IV.
    /// </remarks>
    private const int HeaderSize = SaltSize + IvSize;

    /// <summary>
    /// 使用 AES 算法加密字符串（输出 Base64 编码）。
    /// </summary>
    /// <remarks>
    /// Encrypts a string using AES algorithm (output as Base64 encoding).
    /// </remarks>
    /// <param name="encryptString">待加密的明文字符串 / Plain text string to encrypt</param>
    /// <param name="encryptKey">加密密钥 / Encryption key</param>
    /// <returns>加密后的 Base64 编码字符串，格式为 [Salt(16) | IV(16) | 密文] 的 Base64 表示 / Base64 encoded encrypted string, format is Base64 representation of [Salt(16) | IV(16) | Ciphertext]</returns>
    /// <exception cref="ArgumentException">当明文或密钥为空时抛出 / Thrown when plain text or key is empty</exception>
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
    /// <remarks>
    /// Encrypts a byte array using AES-CBC algorithm.
    /// Salt and IV are randomly generated for each encryption, output format: [Salt(16 bytes) | IV(16 bytes) | Ciphertext].
    /// </remarks>
    /// <param name="encryptByte">待加密的明文字节数组 / Plain text byte array to encrypt</param>
    /// <param name="encryptKey">加密密钥，用于通过 PBKDF2(600,000 次) 派生密钥 / Encryption key used to derive key via PBKDF2(600,000 iterations)</param>
    /// <returns>加密后的字节数组，头部包含 Salt 和 IV / Encrypted byte array with Salt and IV in the header</returns>
    /// <exception cref="ArgumentNullException">当明文字节数组为 null 时抛出 / Thrown when plain text byte array is null</exception>
    /// <exception cref="ArgumentException">当明文字节数组为空或密钥为空时抛出 / Thrown when plain text byte array is empty or key is empty</exception>
    /// <exception cref="CryptographicException">当加密过程失败时抛出 / Thrown when encryption process fails</exception>
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
    /// 使用 AES 算法解密字符串。
    /// </summary>
    /// <remarks>
    /// Decrypts a string using AES algorithm.
    /// </remarks>
    /// <param name="decryptString">待解密的 Base64 编码字符串（格式：[Salt(16) | IV(16) | 密文] 的 Base64 表示）/ Base64 encoded string to decrypt (format: Base64 representation of [Salt(16) | IV(16) | Ciphertext])</param>
    /// <param name="decryptKey">解密密钥，必须与加密时使用的密钥相同 / Decryption key, must be the same as the key used for encryption</param>
    /// <returns>解密后的明文字符串 / Decrypted plain text string</returns>
    /// <exception cref="ArgumentException">当密文或密钥为空时抛出 / Thrown when ciphertext or key is empty</exception>
    /// <exception cref="CryptographicException">当解密失败（如密钥错误或数据被篡改）时抛出 / Thrown when decryption fails (e.g., wrong key or data tampered)</exception>
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
    /// <remarks>
    /// Decrypts a byte array using AES-CBC algorithm.
    /// Expected input format: [Salt(16 bytes) | IV(16 bytes) | Ciphertext], corresponding to <see cref="Encrypt(byte[],string)"/> output format.
    /// </remarks>
    /// <param name="decryptByte">待解密的密文字节数组，头部须包含 Salt 和 IV / Ciphertext byte array to decrypt, must contain Salt and IV in the header</param>
    /// <param name="decryptKey">解密密钥，必须与加密时使用的密钥相同 / Decryption key, must be the same as the key used for encryption</param>
    /// <returns>解密后的明文字节数组 / Decrypted plain text byte array</returns>
    /// <exception cref="ArgumentNullException">当密文字节数组为 null 时抛出 / Thrown when ciphertext byte array is null</exception>
    /// <exception cref="ArgumentException">当密文长度不足或密钥为空时抛出 / Thrown when ciphertext length is insufficient or key is empty</exception>
    /// <exception cref="CryptographicException">当解密失败（如密钥错误或数据被篡改）时抛出 / Thrown when decryption fails (e.g., wrong key or data tampered)</exception>
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
