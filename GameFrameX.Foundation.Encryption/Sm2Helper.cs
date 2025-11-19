using GameFrameX.Foundation.Encryption.Sm;
using System;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// SM2加密算法工具类
/// </summary>
public static class Sm2Helper
{
    /// <summary>
    /// 使用公钥加密字符串数据
    /// </summary>
    /// <param name="publicKeyString">十六进制格式的公钥字符串，不能为null或空字符串</param>
    /// <param name="dataString">待加密的原文字符串，支持空字符串</param>
    /// <returns>加密后的密文字符串，如果输入为空字符串则返回空字符串</returns>
    /// <exception cref="ArgumentNullException">当publicKeyString为null时抛出</exception>
    /// <exception cref="ArgumentException">当publicKeyString为空字符串或格式无效时抛出</exception>
    public static string Encrypt(string publicKeyString, string dataString)
    {
        ArgumentNullException.ThrowIfNull(publicKeyString, nameof(publicKeyString));
        ArgumentException.ThrowIfNullOrWhiteSpace(publicKeyString, nameof(publicKeyString));

        return Sm2Util.Encrypt(publicKeyString, dataString);
    }

    /// <summary>
    /// 使用私钥解密字符串数据
    /// </summary>
    /// <param name="privateKeyString">十六进制格式的私钥字符串，不能为null或空字符串</param>
    /// <param name="encryptedDataString">待解密的密文字符串，支持空字符串</param>
    /// <returns>解密后的原文字符串，如果输入为空字符串则返回空字符串</returns>
    /// <exception cref="ArgumentNullException">当privateKeyString为null时抛出</exception>
    /// <exception cref="ArgumentException">当privateKeyString为空字符串或格式无效时抛出</exception>
    public static string Decrypt(string privateKeyString, string encryptedDataString)
    {
        ArgumentNullException.ThrowIfNull(privateKeyString, nameof(privateKeyString));
        ArgumentException.ThrowIfNullOrWhiteSpace(privateKeyString, nameof(privateKeyString));

        return Sm2Util.Decrypt(privateKeyString, encryptedDataString);
    }

    /// <summary>
    /// 生成SM2密钥对
    /// 生成一对新的公私钥对,并将其以十六进制字符串格式输出
    /// </summary>
    /// <param name="publicKey">输出参数,生成的公钥十六进制字符串</param>
    /// <param name="privateKey">输出参数,生成的私钥十六进制字符串</param>
    public static void GenerateKeyPair(out string publicKey, out string privateKey)
    {
        Sm2Util.GenerateKeyPair(out publicKey, out privateKey);
    }

    /// <summary>
    /// 使用公钥加密字节数组数据
    /// </summary>
    /// <param name="publicKey">公钥字节数组，不能为null或空数组</param>
    /// <param name="data">待加密的原文字节数组，支持null或空数组</param>
    /// <returns>加密后的密文字符串,包含C1、C2、C3三部分，如果data为空则返回空字符串</returns>
    /// <exception cref="ArgumentNullException">当publicKey为null时抛出</exception>
    /// <exception cref="ArgumentException">当publicKey为空数组时抛出</exception>
    public static string Encrypt(byte[] publicKey, byte[] data)
    {
        ArgumentNullException.ThrowIfNull(publicKey, nameof(publicKey));
        if (publicKey.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PublicKeyCannotBeEmpty), nameof(publicKey));
        }

        return Sm2Util.Encrypt(publicKey, data);
    }

    /// <summary>
    /// 使用私钥解密字节数组数据
    /// </summary>
    /// <param name="privateKey">私钥字节数组，不能为null或空数组</param>
    /// <param name="encryptedData">待解密的密文字节数组，支持null或空数组</param>
    /// <returns>解密后的原文字节数组，如果encryptedData为空则返回空数组</returns>
    /// <exception cref="ArgumentNullException">当privateKey为null时抛出</exception>
    /// <exception cref="ArgumentException">当privateKey为空数组时抛出</exception>
    public static byte[] Decrypt(byte[] privateKey, byte[] encryptedData)
    {
        ArgumentNullException.ThrowIfNull(privateKey, nameof(privateKey));
        if (privateKey.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PrivateKeyCannotBeEmpty), nameof(privateKey));
        }

        return Sm2Util.Decrypt(privateKey, encryptedData);
    }
}