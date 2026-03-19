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

using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Encryption.Localization;

namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// XOR加密解密工具类，提供异或运算相关的加密解密功能。
/// 异或运算具有可逆性，使用相同的密钥进行两次异或运算可以还原原始数据。
/// </summary>
/// <remarks>
/// XOR encryption and decryption utility class, providing encryption and decryption related to XOR operations.
/// XOR operation is reversible; performing XOR operation twice with the same key can restore the original data.
/// ⚠️ 安全声明（I-10）：XOR 循环加密属于轻量级数据混淆手段，
/// 不提供任何密码学安全性保障。已知明文攻击或频率分析可轻易破解。
/// 请勿将此类用于需要保密性的场景，仅适用于防止意外读取的简单混淆。
/// ⚠️ Security Notice (I-10): XOR cyclic encryption is a lightweight data obfuscation method
/// that provides no cryptographic security guarantees. Known-plaintext attacks or frequency analysis can easily break it.
/// Do not use this class for scenarios requiring confidentiality; it is only suitable for simple obfuscation to prevent accidental reading.
/// </remarks>
public static class XorHelper
{
    /// <summary>
    /// 快速加密的默认长度，用于只加密数据的前220字节以提高性能。
    /// </summary>
    /// <remarks>
    /// Default length for quick encryption, used to encrypt only the first 220 bytes of data to improve performance.
    /// </remarks>
    internal const int QuickEncryptLength = 220;

    /// <summary>
    /// 将 bytes 使用 code 做异或运算的快速版本。
    /// 只对数据的前QuickEncryptLength字节进行异或运算,适用于需要快速加密的场景。
    /// </summary>
    /// <remarks>
    /// Quick version of XOR operation on bytes using code.
    /// Only performs XOR operation on the first QuickEncryptLength bytes of data, suitable for scenarios requiring fast encryption.
    /// </remarks>
    /// <param name="bytes">原始二进制流 / Original binary stream</param>
    /// <param name="code">异或二进制流(密钥) / XOR binary stream (key)</param>
    /// <returns>异或后的二进制流 / XORed binary stream</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> or <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度为 0 时抛出 / Thrown when <paramref name="code"/> length is 0</exception>
    public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentNullException.ThrowIfNull(code, nameof(code));
        
        if (code.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyArrayCannotBeEmpty), nameof(code));
        }
        
        // 取最小值以防止数组越界：当输入数据长度小于QuickEncryptLength时使用实际长度，
        // 当输入数据长度大于等于QuickEncryptLength时使用QuickEncryptLength以保持快速加密的性能优势
        var length = Math.Min(bytes.Length, QuickEncryptLength);
        return GetXorBytes(bytes, 0, length, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算的快速版本。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
    /// 只对数据的前QuickEncryptLength字节进行异或运算,适用于需要快速加密且允许修改原数据的场景。
    /// </summary>
    /// <remarks>
    /// Quick version of XOR operation on bytes using code. This method reuses and overwrites the input bytes as the return value without allocating additional memory.
    /// Only performs XOR operation on the first QuickEncryptLength bytes of data, suitable for scenarios requiring fast encryption and allowing modification of original data.
    /// </remarks>
    /// <param name="bytes">原始及异或后的二进制流 / Original and XORed binary stream</param>
    /// <param name="code">异或二进制流(密钥) / XOR binary stream (key)</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> or <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度为 0 时抛出 / Thrown when <paramref name="code"/> length is 0</exception>
    public static void GetQuickSelfXorBytes(byte[] bytes, byte[] code)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentNullException.ThrowIfNull(code, nameof(code));
        
        if (code.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyArrayCannotBeEmpty), nameof(code));
        }
        
        // 取最小值以防止数组越界：当输入数据长度小于QuickEncryptLength时使用实际长度，
        // 当输入数据长度大于等于QuickEncryptLength时使用QuickEncryptLength以保持快速加密的性能优势
        var length = Math.Min(bytes.Length, QuickEncryptLength);
        GetSelfXorBytes(bytes, 0, length, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。
    /// 对整个数据进行异或运算加密。
    /// </summary>
    /// <remarks>
    /// Performs XOR operation on bytes using code.
    /// Encrypts the entire data with XOR operation.
    /// </remarks>
    /// <param name="bytes">原始二进制流 / Original binary stream</param>
    /// <param name="code">异或二进制流(密钥) / XOR binary stream (key)</param>
    /// <returns>异或后的二进制流。如果输入为null则返回null。 / XORed binary stream. Returns null if input is null.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度为 0 时抛出 / Thrown when <paramref name="code"/> length is 0</exception>
    public static byte[] GetXorBytes(byte[] bytes, byte[] code)
    {
        if (bytes == null)
        {
            return null;
        }
        
        ArgumentNullException.ThrowIfNull(code, nameof(code));
        
        if (code.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyArrayCannotBeEmpty), nameof(code));
        }

        return GetXorBytes(bytes, 0, bytes.Length, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
    /// 对整个数据进行异或运算加密,直接在原数组上进行修改。
    /// </summary>
    /// <remarks>
    /// Performs XOR operation on bytes using code. This method reuses and overwrites the input bytes as the return value without allocating additional memory.
    /// Encrypts the entire data with XOR operation, directly modifying the original array.
    /// </remarks>
    /// <param name="bytes">原始及异或后的二进制流 / Original and XORed binary stream</param>
    /// <param name="code">异或二进制流(密钥) / XOR binary stream (key)</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度为 0 时抛出 / Thrown when <paramref name="code"/> length is 0</exception>
    public static void GetSelfXorBytes(byte[] bytes, byte[] code)
    {
        if (bytes == null)
        {
            return;
        }
        
        ArgumentNullException.ThrowIfNull(code, nameof(code));
        
        if (code.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyArrayCannotBeEmpty), nameof(code));
        }

        GetSelfXorBytes(bytes, 0, bytes.Length, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。
    /// 可以指定起始位置和长度进行部分加密。
    /// </summary>
    /// <remarks>
    /// Performs XOR operation on bytes using code.
    /// Allows specifying start position and length for partial encryption.
    /// </remarks>
    /// <param name="bytes">原始二进制流 / Original binary stream</param>
    /// <param name="startIndex">异或计算的开始位置 / Start position for XOR calculation</param>
    /// <param name="length">异或计算长度 / Length for XOR calculation</param>
    /// <param name="code">异或二进制流(密钥) / XOR binary stream (key)</param>
    /// <returns>异或后的二进制流。如果输入为null则返回null。 / XORed binary stream. Returns null if input is null.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度为 0 时抛出 / Thrown when <paramref name="code"/> length is 0</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 或 <paramref name="length"/> 超出有效范围时抛出 / Thrown when <paramref name="startIndex"/> or <paramref name="length"/> is out of valid range</exception>
    public static byte[] GetXorBytes(byte[] bytes, int startIndex, int length, byte[] code)
    {
        if (bytes == null)
        {
            return null;
        }
        
        ArgumentNullException.ThrowIfNull(code, nameof(code));
        
        if (code.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyArrayCannotBeEmpty), nameof(code));
        }
        
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
        
        if (startIndex + length > bytes.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Start index and length exceed array bounds");
        }

        var bytesLength = bytes.Length;
        var results = new byte[bytesLength];
        Array.Copy(bytes, 0, results, 0, bytesLength);
        GetSelfXorBytes(results, startIndex, length, code);
        return results;
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
    /// 可以指定起始位置和长度进行部分加密,直接在原数组上进行修改。
    /// </summary>
    /// <remarks>
    /// Performs XOR operation on bytes using code. This method reuses and overwrites the input bytes as the return value without allocating additional memory.
    /// Allows specifying start position and length for partial encryption, directly modifying the original array.
    /// </remarks>
    /// <param name="bytes">原始及异或后的二进制流 / Original and XORed binary stream</param>
    /// <param name="startIndex">异或计算的开始位置 / Start position for XOR calculation</param>
    /// <param name="length">异或计算长度 / Length for XOR calculation</param>
    /// <param name="code">异或二进制流(密钥) / XOR binary stream (key)</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度为 0 时抛出 / Thrown when <paramref name="code"/> length is 0</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 或 <paramref name="length"/> 超出有效范围时抛出 / Thrown when <paramref name="startIndex"/> or <paramref name="length"/> is out of valid range</exception>
    public static void GetSelfXorBytes(byte[] bytes, int startIndex, int length, byte[] code)
    {
        if (bytes == null)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(code, nameof(code));

        if (code.Length == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.KeyArrayCannotBeEmpty), nameof(code));
        }

        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));
        
        if (startIndex + length > bytes.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Start index and length exceed array bounds");
        }

        var codeLength = code.Length;
        var codeIndex = startIndex % codeLength;
        for (var i = startIndex; i < startIndex + length; i++)
        {
            bytes[i] ^= code[codeIndex++];
            codeIndex %= codeLength;
        }
    }
}