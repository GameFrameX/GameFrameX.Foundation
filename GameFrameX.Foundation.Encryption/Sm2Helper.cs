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
using System;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// SM2加密算法工具类，提供SM2非对称加密算法的加密、解密和密钥对生成功能。
/// </summary>
/// <remarks>
/// SM2 encryption algorithm utility class, providing encryption, decryption, and key pair generation using SM2 asymmetric encryption algorithm.
/// </remarks>
public static class Sm2Helper
{
    /// <summary>
    /// 使用公钥加密字符串数据。
    /// </summary>
    /// <remarks>
    /// Encrypts string data using the public key.
    /// </remarks>
    /// <param name="publicKeyString">十六进制格式的公钥字符串，不能为null或空字符串 / Hexadecimal format public key string, cannot be null or empty</param>
    /// <param name="dataString">待加密的原文字符串，支持空字符串 / Plain text string to encrypt, supports empty string</param>
    /// <returns>加密后的密文字符串，如果输入为空字符串则返回空字符串 / Encrypted cipher text string, returns empty string if input is empty</returns>
    /// <exception cref="ArgumentNullException">当publicKeyString为null时抛出 / Thrown when publicKeyString is null</exception>
    /// <exception cref="ArgumentException">当publicKeyString为空字符串或格式无效时抛出 / Thrown when publicKeyString is empty or has invalid format</exception>
    public static string Encrypt(string publicKeyString, string dataString)
    {
        ArgumentNullException.ThrowIfNull(publicKeyString, nameof(publicKeyString));
        ArgumentException.ThrowIfNullOrWhiteSpace(publicKeyString, nameof(publicKeyString));

        return Sm2Util.Encrypt(publicKeyString, dataString);
    }

    /// <summary>
    /// 使用私钥解密字符串数据。
    /// </summary>
    /// <remarks>
    /// Decrypts string data using the private key.
    /// </remarks>
    /// <param name="privateKeyString">十六进制格式的私钥字符串，不能为null或空字符串 / Hexadecimal format private key string, cannot be null or empty</param>
    /// <param name="encryptedDataString">待解密的密文字符串，支持空字符串 / Cipher text string to decrypt, supports empty string</param>
    /// <returns>解密后的原文字符串，如果输入为空字符串则返回空字符串 / Decrypted plain text string, returns empty string if input is empty</returns>
    /// <exception cref="ArgumentNullException">当privateKeyString为null时抛出 / Thrown when privateKeyString is null</exception>
    /// <exception cref="ArgumentException">当privateKeyString为空字符串或格式无效时抛出 / Thrown when privateKeyString is empty or has invalid format</exception>
    public static string Decrypt(string privateKeyString, string encryptedDataString)
    {
        ArgumentNullException.ThrowIfNull(privateKeyString, nameof(privateKeyString));
        ArgumentException.ThrowIfNullOrWhiteSpace(privateKeyString, nameof(privateKeyString));

        return Sm2Util.Decrypt(privateKeyString, encryptedDataString);
    }

    /// <summary>
    /// 生成SM2密钥对。
    /// 生成一对新的公私钥对，并将其以十六进制字符串格式输出。
    /// </summary>
    /// <remarks>
    /// Generates an SM2 key pair.
    /// Generates a new public-private key pair and outputs it in hexadecimal string format.
    /// </remarks>
    /// <param name="publicKey">输出参数，生成的公钥十六进制字符串 / Output parameter, generated public key hexadecimal string</param>
    /// <param name="privateKey">输出参数，生成的私钥十六进制字符串 / Output parameter, generated private key hexadecimal string</param>
    public static void GenerateKeyPair(out string publicKey, out string privateKey)
    {
        Sm2Util.GenerateKeyPair(out publicKey, out privateKey);
    }

    /// <summary>
    /// 使用公钥加密字节数组数据。
    /// </summary>
    /// <remarks>
    /// Encrypts byte array data using the public key.
    /// </remarks>
    /// <param name="publicKey">公钥字节数组，不能为null或空数组 / Public key byte array, cannot be null or empty</param>
    /// <param name="data">待加密的原文字节数组，支持null或空数组 / Plain text byte array to encrypt, supports null or empty array</param>
    /// <returns>加密后的密文字符串，包含C1、C2、C3三部分，如果data为空则返回空字符串 / Encrypted cipher text string containing C1, C2, C3 parts, returns empty string if data is empty</returns>
    /// <exception cref="ArgumentNullException">当publicKey为null时抛出 / Thrown when publicKey is null</exception>
    /// <exception cref="ArgumentException">当publicKey为空数组时抛出 / Thrown when publicKey is empty array</exception>
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
    /// 使用私钥解密字节数组数据。
    /// </summary>
    /// <remarks>
    /// Decrypts byte array data using the private key.
    /// </remarks>
    /// <param name="privateKey">私钥字节数组，不能为null或空数组 / Private key byte array, cannot be null or empty</param>
    /// <param name="encryptedData">待解密的密文字节数组，支持null或空数组 / Cipher text byte array to decrypt, supports null or empty array</param>
    /// <returns>解密后的原文字节数组，如果encryptedData为空则返回空数组 / Decrypted plain text byte array, returns empty array if encryptedData is empty</returns>
    /// <exception cref="ArgumentNullException">当privateKey为null时抛出 / Thrown when privateKey is null</exception>
    /// <exception cref="ArgumentException">当privateKey为空数组时抛出 / Thrown when privateKey is empty array</exception>
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