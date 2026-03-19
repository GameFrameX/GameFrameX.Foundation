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
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities.Encoders;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM2加密算法工具类。
/// 提供SM2非对称加密算法的加密、解密、密钥对生成等功能。
/// </summary>
/// <remarks>
/// SM2 encryption algorithm utility class.
/// Provides encryption, decryption, key pair generation and other functions for SM2 asymmetric encryption algorithm.
/// </remarks>
internal static class Sm2Util
{
    /// <summary>
    /// 使用公钥加密字符串数据。
    /// </summary>
    /// <remarks>
    /// Encrypts string data using the public key.
    /// </remarks>
    /// <param name="publicKeyString">十六进制格式的公钥字符串，不能为null或空字符串 / Hexadecimal format public key string, cannot be null or empty</param>
    /// <param name="dataString">待加密的原文字符串，支持null或空字符串 / Plain text string to encrypt, supports null or empty string</param>
    /// <returns>加密后的密文字符串，如果dataString为空则返回空字符串 / Encrypted cipher text string, returns empty string if dataString is empty</returns>
    /// <exception cref="ArgumentNullException">当publicKeyString为null时抛出 / Thrown when publicKeyString is null</exception>
    /// <exception cref="ArgumentException">当publicKeyString为空字符串时抛出 / Thrown when publicKeyString is empty</exception>
    /// <exception cref="FormatException">当publicKeyString不是有效的十六进制格式时抛出 / Thrown when publicKeyString is not valid hexadecimal format</exception>
    public static string Encrypt(string publicKeyString, string dataString)
    {
        if (publicKeyString == null)
        {
            throw new ArgumentNullException(nameof(publicKeyString), LocalizationService.GetString(LocalizationKeys.Exceptions.PublicKeyStringCannotBeNull));
        }

        if (string.IsNullOrWhiteSpace(publicKeyString))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PublicKeyStringCannotBeEmpty), nameof(publicKeyString));
        }

        var publicKey = Hex.Decode(publicKeyString);
        var data = Encoding.UTF8.GetBytes(dataString ?? string.Empty);
        return Encrypt(publicKey, data);
    }

    /// <summary>
    /// 使用私钥解密字符串数据。
    /// </summary>
    /// <remarks>
    /// Decrypts string data using the private key.
    /// </remarks>
    /// <param name="privateKeyString">十六进制格式的私钥字符串，不能为null或空字符串 / Hexadecimal format private key string, cannot be null or empty</param>
    /// <param name="encryptedDataString">待解密的密文字符串，支持null或空字符串 / Cipher text string to decrypt, supports null or empty string</param>
    /// <returns>解密后的原文字符串，如果encryptedDataString为空则返回空字符串 / Decrypted plain text string, returns empty string if encryptedDataString is empty</returns>
    /// <exception cref="ArgumentNullException">当privateKeyString为null时抛出 / Thrown when privateKeyString is null</exception>
    /// <exception cref="ArgumentException">当privateKeyString为空字符串时抛出 / Thrown when privateKeyString is empty</exception>
    /// <exception cref="FormatException">当privateKeyString或encryptedDataString不是有效的十六进制格式时抛出 / Thrown when privateKeyString or encryptedDataString is not valid hexadecimal format</exception>
    public static string Decrypt(string privateKeyString, string encryptedDataString)
    {
        if (privateKeyString == null)
        {
            throw new ArgumentNullException(nameof(privateKeyString), LocalizationService.GetString(LocalizationKeys.Exceptions.PrivateKeyStringCannotBeNull));
        }

        if (string.IsNullOrWhiteSpace(privateKeyString))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PrivateKeyStringCannotBeEmpty), nameof(privateKeyString));
        }

        if (encryptedDataString == string.Empty)
        {
            return string.Empty;
        }
        
        var privateKey = Hex.Decode(privateKeyString);
        var encryptedData = Hex.Decode(encryptedDataString);
        var deStr = Sm2Util.Decrypt(privateKey, encryptedData);
        string plainText = Encoding.UTF8.GetString(deStr);
        return plainText;
    }

    /// <summary>
    /// 生成SM2密钥对。
    /// 生成一对新的公私钥对，并将其以十六进制字符串格式输出。
    /// </summary>
    /// <remarks>
    /// Generates an SM2 key pair.
    /// Generates a new public-private key pair and outputs it in hexadecimal string format.
    /// </remarks>
    /// <param name="publicKeyString">输出参数，生成的公钥十六进制字符串 / Output parameter, generated public key hexadecimal string</param>
    /// <param name="privateKeyString">输出参数，生成的私钥十六进制字符串 / Output parameter, generated private key hexadecimal string</param>
    public static void GenerateKeyPair(out string publicKeyString, out string privateKeyString)
    {
        // 获取SM2算法实例
        Sm2 sm2 = Sm2.Instance;
        // 生成非对称密钥对
        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
        // 获取私钥参数
        ECPrivateKeyParameters ecPrivateKeyParameters = (ECPrivateKeyParameters)key.Private;
        // 获取公钥参数
        ECPublicKeyParameters ecPublicKeyParameters = (ECPublicKeyParameters)key.Public;
        // 从私钥参数中提取私钥值
        BigInteger privateKey = ecPrivateKeyParameters.D;
        // 从公钥参数中提取公钥点
        ECPoint publicKey = ecPublicKeyParameters.Q;

        // 将公钥点编码为字节数组,再转换为十六进制字符串
        publicKeyString = Encoding.ASCII.GetString(Hex.Encode(publicKey.GetEncoded())).ToUpper();
        // 将私钥值转换为字节数组,再转换为十六进制字符串
        privateKeyString = Encoding.ASCII.GetString(Hex.Encode(privateKey.ToByteArray())).ToUpper();
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
        if (publicKey == null)
        {
            throw new ArgumentNullException(nameof(publicKey), LocalizationService.GetString(LocalizationKeys.Exceptions.PublicKeyByteArrayCannotBeNull));
        }

        if (publicKey.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PublicKeyByteArrayCannotBeEmpty), nameof(publicKey));
        }

        if (data == null)
        {
            data = Array.Empty<byte>();
        }
        
        // 特殊处理空数据
        if (data.Length == 0)
        {
            return string.Empty;
        }

        byte[] source = new byte[data.Length];
        Array.Copy(data, 0, source, 0, data.Length);

        var cipher = new Cipher();
        Sm2 sm2 = Sm2.Instance;

        ECPoint userKey = sm2.ecc_curve.DecodePoint(publicKey);

        ECPoint c1 = cipher.Init_enc(sm2, userKey);
        cipher.Encrypt(source);

        byte[] c3 = new byte[32];
        cipher.Dofinal(c3);

        string sc1 = Encoding.ASCII.GetString(Hex.Encode(c1.GetEncoded()));
        string sc2 = Encoding.ASCII.GetString(Hex.Encode(source));
        string sc3 = Encoding.ASCII.GetString(Hex.Encode(c3));

        return (sc1 + sc2 + sc3).ToUpper();
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
        if (privateKey == null)
        {
            throw new ArgumentNullException(nameof(privateKey), LocalizationService.GetString(LocalizationKeys.Exceptions.PrivateKeyByteArrayCannotBeNull));
        }

        if (privateKey.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.PrivateKeyByteArrayCannotBeEmpty), nameof(privateKey));
        }

        if (encryptedData == null)
        {
            return Array.Empty<byte>();
        }
        
        if (encryptedData.Length == 0)
        {
            return Array.Empty<byte>();
        }

        // W-12 修复：添加最小长度校验，避免硬编码偏移量导致的越界
        // 未压缩 EC 点 = 65 字节 = 130 hex 字符；C3 = 32 字节 = 64 hex 字符；最小密文 = 65+1+32 = 98 字节
        const int c1HexLen = 130; // 65 字节未压缩点 × 2
        const int c3HexLen = 64;  // 32 字节 SM3 摘要 × 2
        const int minEncryptedLen = 97; // 65(C1) + 0(C2 最小) + 32(C3)

        if (encryptedData.Length < minEncryptedLen)
        {
            throw new ArgumentException(
                $"Encrypted data is too short: expected at least {minEncryptedLen} bytes, got {encryptedData.Length}.",
                nameof(encryptedData));
        }

        string data = Encoding.ASCII.GetString(Hex.Encode(encryptedData));

        byte[] c1Bytes = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(0, c1HexLen)));
        int c2Len = encryptedData.Length - minEncryptedLen;
        byte[] c2 = c2Len > 0
            ? Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(c1HexLen, 2 * c2Len)))
            : Array.Empty<byte>();
        byte[] c3 = Hex.Decode(Encoding.ASCII.GetBytes(data.Substring(c1HexLen + 2 * c2Len, c3HexLen)));

        Sm2 sm2 = Sm2.Instance;
        var userD = new BigInteger(1, privateKey);

        ECPoint c1 = sm2.ecc_curve.DecodePoint(c1Bytes);
        var cipher = new Cipher();
        cipher.Init_dec(userD, c1);
        cipher.Decrypt(c2);
        cipher.Dofinal(c3);

        return c2;
    }
}