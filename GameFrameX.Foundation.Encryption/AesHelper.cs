using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// AES 加密解密
/// </summary>
public static class AesHelper
{
    /// <summary>
    /// 使用 AES 算法加密字符串（高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法）
    /// AES加密过程:
    /// 1. 检查输入参数的有效性
    /// 2. 将输入字符串转换为UTF8字节数组
    /// 3. 调用字节数组加密方法进行加密
    /// 4. 将加密结果转换为Base64字符串返回
    /// </summary>
    /// <param name="encryptString">待加密的明文字符串</param>
    /// <param name="encryptKey">加密密钥，用于生成加密所需的密钥</param>
    /// <returns>加密后的 Base64 编码字符串</returns>
    /// <exception cref="ArgumentException">当明文或密钥为空时抛出异常</exception>
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
    /// 使用 AES 算法加密字节数组（高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法）
    /// AES加密过程:
    /// 1. 检查输入参数的有效性
    /// 2. 使用固定的IV(初始化向量)和Salt值
    /// 3. 通过PBKDF2(RFC2898)算法从密钥派生出加密密钥
    /// 4. 创建AES加密器并使用CryptoStream进行加密
    /// 5. 返回加密后的字节数组
    /// </summary>
    /// <param name="encryptByte">待加密的明文字节数组</param>
    /// <param name="encryptKey">加密密钥，用于生成加密所需的密钥</param>
    /// <returns>加密后的字节数组</returns>
    /// <exception cref="ArgumentNullException">当明文字节数组为null时抛出异常</exception>
    /// <exception cref="ArgumentException">当明文字节数组为空或密钥为空时抛出异常</exception>
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

        byte[] encryptedBytes = null;
        // 初始化向量，用于CBC模式
        var iv = new byte[] { 224, 131, 122, 101, 37, 254, 33, 17, 19, 28, 212, 130, 45, 65, 43, 32, };
        // 用于密钥派生的盐值
        var salt = new byte[] { 234, 231, 123, 100, 87, 254, 123, 17, 89, 18, 230, 13, 45, 65, 43, 32, };
        using (var aesProvider = Aes.Create())
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // 使用PBKDF2算法派生密钥，迭代1000次
                    using (var pdb = new Rfc2898DeriveBytes(encryptKey, salt, 1000, HashAlgorithmName.SHA256))
                    {
                        var transform = aesProvider.CreateEncryptor(pdb.GetBytes(32), iv);
                        using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(encryptByte, 0, encryptByte.Length);
                            cryptoStream.FlushFinalBlock();
                            encryptedBytes = memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        return encryptedBytes;
    }


    /// <summary>
    /// 使用 AES 算法解密字符串（高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法）
    /// 解密过程:
    /// 1. 检查输入参数的有效性
    /// 2. 将Base64字符串转换为字节数组
    /// 3. 调用字节数组解密方法进行解密
    /// 4. 将解密结果转换为UTF8字符串返回
    /// </summary>
    /// <param name="decryptString">待解密的 Base64 编码字符串</param>
    /// <param name="decryptKey">解密密钥，必须与加密时使用的密钥相同</param>
    /// <returns>解密后的明文字符串</returns>
    /// <exception cref="ArgumentException">当密文或密钥为空时抛出异常</exception>
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

        return Encoding.UTF8.GetString(AesDecrypt(Convert.FromBase64String(decryptString), decryptKey));
    }


    /// <summary>
    /// 使用 AES 算法解密字节数组（高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法）
    /// 解密过程:
    /// 1. 检查输入参数的有效性
    /// 2. 使用与加密相同的IV和Salt值
    /// 3. 通过PBKDF2(RFC2898)算法从密钥派生出解密密钥
    /// 4. 创建AES解密器并使用CryptoStream进行解密
    /// 5. 返回解密后的字节数组
    /// </summary>
    /// <param name="decryptByte">待解密的密文字节数组</param>
    /// <param name="decryptKey">解密密钥，必须与加密时使用的密钥相同</param>
    /// <returns>解密后的明文字节数组</returns>
    /// <exception cref="ArgumentNullException">当密文字节数组为null时抛出异常</exception>
    /// <exception cref="ArgumentException">当密文字节数组为空或密钥为空时抛出异常</exception>
    public static byte[] AesDecrypt(byte[] decryptByte, string decryptKey)
    {
        if (decryptByte == null)
        {
            throw new ArgumentNullException(nameof(decryptByte), @"Cipher text byte array cannot be null");
        }

        if (decryptByte.Length == 0)
        {
            throw new ArgumentException("Cipher text byte array cannot be empty", nameof(decryptByte));
        }

        if (string.IsNullOrEmpty(decryptKey))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.DecryptionKeyCannotBeNullOrEmpty), nameof(decryptKey));
        }

        byte[] decryptedBytes = null;
        // 初始化向量，必须与加密时使用的IV相同
        var iv = new byte[] { 224, 131, 122, 101, 37, 254, 33, 17, 19, 28, 212, 130, 45, 65, 43, 32, };
        // 用于密钥派生的盐值，必须与加密时使用的Salt相同
        var salt = new byte[] { 234, 231, 123, 100, 87, 254, 123, 17, 89, 18, 230, 13, 45, 65, 43, 32, };
        using (var aesProvider = Aes.Create())
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    // 使用PBKDF2算法派生密钥，参数必须与加密时相同
                    using (var pdb = new Rfc2898DeriveBytes(decryptKey, salt, 1000, HashAlgorithmName.SHA256))
                    {
                        var transform = aesProvider.CreateDecryptor(pdb.GetBytes(32), iv);
                        using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(decryptByte, 0, decryptByte.Length);
                            cryptoStream.FlushFinalBlock();
                            decryptedBytes = memoryStream.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        return decryptedBytes;
    }
}