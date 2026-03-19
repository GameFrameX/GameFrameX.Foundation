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

using System.Text;
using Org.BouncyCastle.Utilities.Encoders;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM4工具类，提供SM4对称加密算法的加密和解密功能。
/// </summary>
/// <remarks>
/// SM4 utility class, providing encryption and decryption using SM4 symmetric encryption algorithm.
/// </remarks>
internal sealed class Sm4Util
{
    /// <summary>
    /// 密钥，长度必须为 16 字节（非 hex 模式）或 32 个 hex 字符（hex 模式）。
    /// C-06 修复：移除硬编码默认密钥，调用方必须显式赋值，避免使用公开已知的密钥进行加密。
    /// </summary>
    /// <remarks>
    /// Key, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode).
    /// C-06 fix: Removed hardcoded default key, caller must explicitly assign value to avoid using publicly known keys for encryption.
    /// </remarks>
    public string secretKey { get; set; } = null;

    /// <summary>
    /// 初始化向量。
    /// </summary>
    /// <remarks>
    /// Initialization vector.
    /// </remarks>
    public string iv { get; set; } = "0000000000000000";

    /// <summary>
    /// 是否以十六进制字符串形式处理密钥。
    /// </summary>
    /// <remarks>
    /// Whether to process key as hexadecimal string.
    /// </remarks>
    public bool hexString { get; set; } = false;

    /// <summary>
    /// 是否为JavaScript兼容模式。
    /// </summary>
    /// <remarks>
    /// Whether to use JavaScript compatibility mode.
    /// </remarks>
    public bool forJavascript { get; set; } = false;

    /// <summary>
    /// 使用ECB模式加密字符串。
    /// </summary>
    /// <remarks>
    /// Encrypts a string using ECB mode.
    /// </remarks>
    /// <param name="plainText">待加密的明文字符串 / Plain text string to encrypt</param>
    /// <returns>加密后的密文字符串 / Encrypted cipher text string</returns>
    /// <exception cref="ArgumentNullException">当plainText为null时抛出 / Thrown when plainText is null</exception>
    /// <exception cref="ArgumentException">当密钥长度不正确时抛出 / Thrown when key length is incorrect</exception>
    public string Encrypt_ECB(string plainText)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        
        // Validate key length
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(secretKey));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(secretKey));
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
    /// 使用ECB模式加密字节数组。
    /// </summary>
    /// <remarks>
    /// Encrypts a byte array using ECB mode.
    /// </remarks>
    /// <param name="plainBytes">待加密的明文字节数组 / Plain text byte array to encrypt</param>
    /// <param name="keyBytes">密钥字节数组 / Key byte array</param>
    /// <returns>加密后的密文字节数组 / Encrypted cipher text byte array</returns>
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
    }

    /// <summary>
    /// 使用ECB模式解密字符串。
    /// </summary>
    /// <remarks>
    /// Decrypts a string using ECB mode.
    /// </remarks>
    /// <param name="cipherText">待解密的密文字符串 / Cipher text string to decrypt</param>
    /// <returns>解密后的明文字符串 / Decrypted plain text string</returns>
    /// <exception cref="ArgumentNullException">当cipherText为null时抛出 / Thrown when cipherText is null</exception>
    /// <exception cref="ArgumentException">当密钥长度不正确时抛出 / Thrown when key length is incorrect</exception>
    public string Decrypt_ECB(string cipherText)
    {
        ArgumentNullException.ThrowIfNull(cipherText);
        
        // Validate key length
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(secretKey));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(secretKey));
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
    /// 使用CBC模式加密字符串。
    /// </summary>
    /// <remarks>
    /// Encrypts a string using CBC mode.
    /// </remarks>
    /// <param name="plainText">待加密的明文字符串 / Plain text string to encrypt</param>
    /// <returns>加密后的密文字符串 / Encrypted cipher text string</returns>
    /// <exception cref="ArgumentNullException">当plainText为null时抛出 / Thrown when plainText is null</exception>
    /// <exception cref="ArgumentException">当密钥或IV长度不正确时抛出 / Thrown when key or IV length is incorrect</exception>
    public string Encrypt_CBC(string plainText)
    {
        ArgumentNullException.ThrowIfNull(plainText);
        
        // Validate key and IV lengths
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(secretKey));
            }
            if (iv?.Length != 32)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IVMustBe32Characters), nameof(iv));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(secretKey));
            }
            if (iv?.Length != 16)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IVMustBe16Characters), nameof(iv));
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
    /// 使用CBC模式解密字符串。
    /// </summary>
    /// <remarks>
    /// Decrypts a string using CBC mode.
    /// </remarks>
    /// <param name="cipherText">待解密的密文字符串 / Cipher text string to decrypt</param>
    /// <returns>解密后的明文字符串 / Decrypted plain text string</returns>
    /// <exception cref="ArgumentNullException">当cipherText为null时抛出 / Thrown when cipherText is null</exception>
    /// <exception cref="ArgumentException">当密钥或IV长度不正确时抛出 / Thrown when key or IV length is incorrect</exception>
    public string Decrypt_CBC(string cipherText)
    {
        ArgumentNullException.ThrowIfNull(cipherText);
        
        // Validate key and IV lengths
        if (hexString)
        {
            if (secretKey?.Length != 32)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(secretKey));
            }
            if (iv?.Length != 32)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IVMustBe32Characters), nameof(iv));
            }
        }
        else
        {
            if (secretKey?.Length != 16)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(secretKey));
            }
            if (iv?.Length != 16)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IVMustBe16Characters), nameof(iv));
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