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
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// SM4加密算法工具类,提供SM4对称加密算法的加密和解密功能
/// </summary>
public static class Sm4Helper
{
    /// <summary>
    /// 使用CBC模式加密字符串数据
    /// </summary>
    /// <param name="keyString">密钥字符串,长度必须为16字节</param>
    /// <param name="dataString">待加密的原文字符串</param>
    /// <param name="iv">初始化向量,可选,默认为全0</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥</param>
    /// <returns>加密后的密文字符串</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出</exception>
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

        return sm4Util.Encrypt_CBC(dataString);
    }

    /// <summary>
    /// 使用CBC模式解密字符串数据
    /// </summary>
    /// <param name="keyString">密钥字符串,长度必须为16字节</param>
    /// <param name="dataString">待解密的密文字符串</param>
    /// <param name="iv">初始化向量,可选,默认为全0</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥</param>
    /// <returns>解密后的原文字符串</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出</exception>
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

        return sm4Util.Decrypt_CBC(dataString);
    }

    /// <summary>
    /// 使用ECB模式加密字符串数据
    /// </summary>
    /// <param name="keyString">密钥字符串,长度必须为16字节</param>
    /// <param name="dataString">待加密的原文字符串</param>
    /// <param name="iv">初始化向量,在ECB模式下不使用,可为null</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥</param>
    /// <returns>加密后的密文字符串</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出</exception>
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

        return sm4Util.Encrypt_ECB(dataString);
    }

    /// <summary>
    /// 使用ECB模式解密字符串数据
    /// </summary>
    /// <param name="keyString">密钥字符串,长度必须为16字节</param>
    /// <param name="dataString">待解密的密文字符串</param>
    /// <param name="iv">初始化向量,在ECB模式下不使用,可为null</param>
    /// <param name="forJavascript">是否为JavaScript兼容模式</param>
    /// <param name="hexString">是否以十六进制字符串形式处理密钥</param>
    /// <returns>解密后的原文字符串</returns>
    /// <exception cref="ArgumentNullException">当keyString或dataString为null时抛出</exception>
    /// <exception cref="ArgumentException">当keyString为空字符串或长度不正确时抛出</exception>
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

        return sm4Util.Decrypt_ECB(dataString);
    }
}