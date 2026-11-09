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

using GameFrameX.Foundation.Encryption.Sm;
using System.IO;
using System.Security.Cryptography;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// SM4加密算法工具类，提供SM4对称加密算法的加密和解密功能。
/// </summary>
/// <remarks>
/// SM4 encryption algorithm utility class, providing encryption and decryption using SM4 symmetric encryption algorithm.
/// </remarks>
public static class Sm4Helper
{
    /// <summary>
    /// 使用CBC模式加密字符串数据。
    /// </summary>
    /// <remarks>
    /// Encrypts string data using CBC mode.
    /// </remarks>
    /// <param name="keyString">密钥字符串，长度必须为16字节（非hex模式）或32个hex字符（hex模式） / Key string, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode)</param>
    /// <param name="dataString">待加密的原文字符串 / Plain text string to encrypt</param>
    /// <param name="iv">初始化向量，可选；省略或传固定值时，同一密钥下加密结果是确定性的 / Initialization vector, optional; when omitted or fixed, encryption is deterministic under the same key</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式 / Whether to use JavaScript compatibility mode</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥 / Whether to process key as hexadecimal string</param>
    /// <returns>加密后的密文字符串 / Encrypted cipher text string</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出 / Thrown when keyString or dataString is null</exception>
    [Obsolete("使用固定或省略的IV时，同一密钥下的CBC加密是确定性的，会泄露首块明文相等性。请改用 EncryptCbcWithRandomIv（每次生成随机IV并随密文携带）。此方法仅为兼容存量数据保留。/ CBC with a fixed or omitted IV is deterministic under the same key and leaks equality of the first plaintext block. Use EncryptCbcWithRandomIv instead, which generates a fresh random IV carried with the cipher text. Retained only for compatibility with existing data.")]
    public static string EncryptCbc(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
        ArgumentNullException.ThrowIfNull(keyString);
        ArgumentNullException.ThrowIfNull(dataString);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString, nameof(keyString));

        // Validate key length based on hexString flag
        if (hexString && keyString.Length != 32)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(keyString));
        }
        else if (!hexString && keyString.Length != 16)
        {
            // W-13 修复：补全缩进
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(keyString));
        }

        Sm4Util sm4Util = new Sm4Util
        {
            secretKey = keyString,
            hexString = hexString,
            forJavascript = forJavascript,
        };
        if (iv != null)
        {
            sm4Util.iv = iv;
        }
        else if (hexString)
        {
            // Set default IV for hex string mode (16 bytes = 32 hex characters)
            sm4Util.iv = "00000000000000000000000000000000";
        }

        try
        {
            return sm4Util.Encrypt_CBC(dataString);
        }
        catch (IOException ex)
        {
            throw new CryptographicException(LocalizationService.GetString(LocalizationKeys.Exceptions.Sm4InvalidDataFormat), ex);
        }
    }

    /// <summary>
    /// 使用CBC模式解密字符串数据。
    /// </summary>
    /// <remarks>
    /// Decrypts string data using CBC mode.
    /// </remarks>
    /// <param name="keyString">密钥字符串，长度必须为16字节（非hex模式）或32个hex字符（hex模式） / Key string, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode)</param>
    /// <param name="dataString">待解密的密文字符串 / Cipher text string to decrypt</param>
    /// <param name="iv">初始化向量，可选；须与加密时使用的IV一致 / Initialization vector, optional; must match the IV used for encryption</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式 / Whether to use JavaScript compatibility mode</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥 / Whether to process key as hexadecimal string</param>
    /// <returns>解密后的原文字符串 / Decrypted plain text string</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出 / Thrown when keyString or dataString is null</exception>
    [Obsolete("EncryptCbcWithRandomIv 生成的密文请用 DecryptCbcWithRandomIv 解密（IV 随密文携带）。此方法仅适用于使用显式IV或历史固定IV加密的存量数据。/ Cipher text from EncryptCbcWithRandomIv should be decrypted with DecryptCbcWithRandomIv (IV is carried with the cipher text). This method is only for existing data encrypted with an explicit or the historical fixed IV.")]
    public static string DecryptCbc(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
        ArgumentNullException.ThrowIfNull(keyString);
        ArgumentNullException.ThrowIfNull(dataString);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString, nameof(keyString));

        // Validate key length based on hexString flag
        if (hexString && keyString.Length != 32)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(keyString));
        }
        else if (!hexString && keyString.Length != 16)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(keyString));
        }

        Sm4Util sm4Util = new Sm4Util
        {
            secretKey = keyString,
            hexString = hexString,
            forJavascript = forJavascript,
        };
        if (iv != null)
        {
            sm4Util.iv = iv;
        }
        else if (hexString)
        {
            // Set default IV for hex string mode (16 bytes = 32 hex characters)
            sm4Util.iv = "00000000000000000000000000000000";
        }

        try
        {
            return sm4Util.Decrypt_CBC(dataString);
        }
        catch (IOException ex)
        {
            throw new CryptographicException(LocalizationService.GetString(LocalizationKeys.Exceptions.Sm4InvalidDataFormat), ex);
        }
    }

    /// <summary>
    /// 使用CBC模式加密字符串数据，每次调用生成随机IV并随密文一起返回。
    /// </summary>
    /// <remarks>
    /// Encrypts string data using CBC mode with a fresh random IV generated per call and carried with the cipher text.
    /// 输出格式为十六进制(IV(16字节) + 密文)，请使用 <see cref="DecryptCbcWithRandomIv"/> 解密。
    /// Output format is hex(IV(16 bytes) + cipher text); decrypt with <see cref="DecryptCbcWithRandomIv"/>.
    /// </remarks>
    /// <param name="keyString">密钥字符串，长度必须为16字节（非hex模式）或32个hex字符（hex模式） / Key string, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode)</param>
    /// <param name="dataString">待加密的原文字符串 / Plain text string to encrypt</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式 / Whether to use JavaScript compatibility mode</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥 / Whether to process key as hexadecimal string</param>
    /// <returns>十六进制(IV + 密文)字符串 / Hex string of (IV + cipher text)</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出 / Thrown when keyString or dataString is null</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出 / Thrown when keyString is empty or has incorrect length</exception>
    public static string EncryptCbcWithRandomIv(string keyString, string dataString, bool forJavascript = false, bool hexString = false)
    {
        ArgumentNullException.ThrowIfNull(keyString);
        ArgumentNullException.ThrowIfNull(dataString);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString, nameof(keyString));

        // Validate key length based on hexString flag
        if (hexString && keyString.Length != 32)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(keyString));
        }
        else if (!hexString && keyString.Length != 16)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(keyString));
        }

        Sm4Util sm4Util = new Sm4Util
        {
            secretKey = keyString,
            hexString = hexString,
            forJavascript = forJavascript,
        };

        byte[] ivBytes = RandomNumberGenerator.GetBytes(16);

        try
        {
            return sm4Util.Encrypt_CBC_WithRandomIv(dataString, ivBytes);
        }
        catch (IOException ex)
        {
            throw new CryptographicException(LocalizationService.GetString(LocalizationKeys.Exceptions.Sm4InvalidDataFormat), ex);
        }
    }

    /// <summary>
    /// 解密<see cref="EncryptCbcWithRandomIv"/>生成的密文，从密文前16字节读取IV。
    /// </summary>
    /// <remarks>
    /// Decrypts cipher text produced by <see cref="EncryptCbcWithRandomIv"/>, reading the IV from the first 16 bytes.
    /// </remarks>
    /// <param name="keyString">密钥字符串，长度必须为16字节（非hex模式）或32个hex字符（hex模式） / Key string, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode)</param>
    /// <param name="cipherText">十六进制(IV + 密文)字符串 / Hex string of (IV + cipher text)</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式 / Whether to use JavaScript compatibility mode</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥 / Whether to process key as hexadecimal string</param>
    /// <returns>解密后的原文字符串 / Decrypted plain text string</returns>
    /// <exception cref="ArgumentNullException">当keyString或cipherText为null时抛出 / Thrown when keyString or cipherText is null</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出 / Thrown when keyString is empty or has incorrect length</exception>
    /// <exception cref="CryptographicException">当密文格式非法时抛出 / Thrown when cipher text format is invalid</exception>
    public static string DecryptCbcWithRandomIv(string keyString, string cipherText, bool forJavascript = false, bool hexString = false)
    {
        ArgumentNullException.ThrowIfNull(keyString);
        ArgumentNullException.ThrowIfNull(cipherText);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString, nameof(keyString));

        // Validate key length based on hexString flag
        if (hexString && keyString.Length != 32)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(keyString));
        }
        else if (!hexString && keyString.Length != 16)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(keyString));
        }

        Sm4Util sm4Util = new Sm4Util
        {
            secretKey = keyString,
            hexString = hexString,
            forJavascript = forJavascript,
        };

        try
        {
            return sm4Util.Decrypt_CBC_WithIvPrefix(cipherText);
        }
        catch (IOException ex)
        {
            throw new CryptographicException(LocalizationService.GetString(LocalizationKeys.Exceptions.Sm4InvalidDataFormat), ex);
        }
    }

    /// <summary>
    /// 使用ECB模式加密字符串数据。
    /// </summary>
    /// <remarks>
    /// Encrypts string data using ECB mode.
    /// </remarks>
    /// <param name="keyString">密钥字符串，长度必须为16字节（非hex模式）或32个hex字符（hex模式） / Key string, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode)</param>
    /// <param name="dataString">待加密的原文字符串 / Plain text string to encrypt</param>
    /// <param name="iv">初始化向量，在ECB模式下不使用，可为null / Initialization vector, not used in ECB mode, can be null</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式 / Whether to use JavaScript compatibility mode</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥 / Whether to process key as hexadecimal string</param>
    /// <returns>加密后的密文字符串 / Encrypted cipher text string</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出 / Thrown when keyString or dataString is null</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出 / Thrown when keyString is empty or has incorrect length</exception>
    [Obsolete("ECB mode is not semantically secure. Use EncryptCbc with a random IV instead. This method is retained only for backward compatibility with existing data.")]
    public static string EncryptEcb(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
        ArgumentNullException.ThrowIfNull(keyString);
        ArgumentNullException.ThrowIfNull(dataString);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString, nameof(keyString));

        // Validate key length based on hexString flag
        if (hexString && keyString.Length != 32)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(keyString));
        }
        else if (!hexString && keyString.Length != 16)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(keyString));
        }

        Sm4Util sm4Util = new Sm4Util
        {
            secretKey = keyString,
            hexString = hexString,
            forJavascript = forJavascript,
        };
        if (iv != null)
        {
            sm4Util.iv = iv;
        }

        try
        {
            return sm4Util.Encrypt_ECB(dataString);
        }
        catch (IOException ex)
        {
            throw new CryptographicException(LocalizationService.GetString(LocalizationKeys.Exceptions.Sm4InvalidDataFormat), ex);
        }
    }

    /// <summary>
    /// 使用ECB模式解密字符串数据。
    /// </summary>
    /// <remarks>
    /// Decrypts string data using ECB mode.
    /// </remarks>
    /// <param name="keyString">密钥字符串，长度必须为16字节（非hex模式）或32个hex字符（hex模式） / Key string, must be 16 bytes (non-hex mode) or 32 hex characters (hex mode)</param>
    /// <param name="dataString">待解密的密文字符串 / Cipher text string to decrypt</param>
    /// <param name="iv">初始化向量，在ECB模式下不使用，可为null / Initialization vector, not used in ECB mode, can be null</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式 / Whether to use JavaScript compatibility mode</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥 / Whether to process key as hexadecimal string</param>
    /// <returns>解密后的原文字符串 / Decrypted plain text string</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出 / Thrown when keyString or dataString is null</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出 / Thrown when keyString is empty or has incorrect length</exception>
    [Obsolete("ECB mode is not semantically secure. Use DecryptCbc with a random IV instead. This method is retained only for backward compatibility with existing data.")]
    public static string DecryptEcb(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
        ArgumentNullException.ThrowIfNull(keyString);
        ArgumentNullException.ThrowIfNull(dataString);
        ArgumentException.ThrowIfNullOrWhiteSpace(keyString, nameof(keyString));

        // Validate key length based on hexString flag
        if (hexString && keyString.Length != 32)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe32Characters), nameof(keyString));
        }
        else if (!hexString && keyString.Length != 16)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyMustBe16Characters), nameof(keyString));
        }

        Sm4Util sm4Util = new Sm4Util
        {
            secretKey = keyString,
            hexString = hexString,
            forJavascript = forJavascript,
        };
        if (iv != null)
        {
            sm4Util.iv = iv;
        }

        try
        {
            return sm4Util.Decrypt_ECB(dataString);
        }
        catch (IOException ex)
        {
            throw new CryptographicException(LocalizationService.GetString(LocalizationKeys.Exceptions.Sm4InvalidDataFormat), ex);
        }
    }
}