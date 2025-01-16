using GameFrameX.Foundation.Encryption.Sm;

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
    public static string EncryptCbc(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
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
    public static string DecryptCbc(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
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
    public static string EncryptEcb(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
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
    public static string DecryptEcb(string keyString, string dataString, string iv = null, bool forJavascript = false, bool hexString = false)
    {
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