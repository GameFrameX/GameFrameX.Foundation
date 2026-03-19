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

using System;
using System.Text;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字节和字节数组的扩展方法，用于各种格式的转换操作。
/// </summary>
/// <remarks>
/// Provides extension methods for bytes and byte arrays for various format conversion operations.
/// </remarks>
public static partial class ByteExtensions
{
    #region Private Helpers

    /// <summary>
    /// 验证缓冲区参数的有效性。
    /// </summary>
    /// <remarks>
    /// Validates the buffer parameters for validity.
    /// </remarks>
    /// <param name="buffer">要验证的缓冲区 / The buffer to validate.</param>
    /// <param name="offset">当前偏移量 / The current offset.</param>
    /// <param name="requiredSize">需要写入或读取的字节数 / The number of bytes to write or read.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    internal static void ValidateBounds(byte[] buffer, int offset, int requiredSize)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + requiredSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                                                  LocalizationService.GetString(LocalizationKeys.Exceptions.BufferWriteOutOfRange, offset + requiredSize, buffer.Length));
        }
    }

    #endregion

    #region Conversion Methods

    /// <summary>
    /// 将字节转换为16进制字符串。
    /// </summary>
    /// <remarks>
    /// Converts a byte to a hexadecimal string.
    /// </remarks>
    /// <param name="b">要转换的字节 / The byte to convert.</param>
    /// <returns>16进制字符串 / A hexadecimal string.</returns>
    public static string ToHex(this byte b)
    {
        return b.ToString("X2");
    }

    /// <summary>
    /// 将字节数组转换为字符串，每个字节之间用空格分隔。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a string with each byte separated by a space.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <returns>字符串表示形式 / The string representation.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static string ToArrayString(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        // 预分配容量: 每个字节最多 3 个字符（如 "255"）+ 1 个空格
        var stringBuilder = new StringBuilder(bytes.Length * 4);
        foreach (var b in bytes)
        {
            stringBuilder.Append(b).Append(' ');
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组转换为16进制字符串。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a hexadecimal string.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <returns>16进制字符串 / A hexadecimal string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static string ToHex(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        // 预分配容量: 每个字节 2 个十六进制字符
        var stringBuilder = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            stringBuilder.Append(b.ToString("X2"));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组转换为指定格式的字符串。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a string with the specified format.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <param name="format">格式化字符串 / The format string.</param>
    /// <returns>格式化后的字符串 / The formatted string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static string ToHex(this byte[] bytes, string format)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        // 预分配容量: 假设每个字节最多 4 个字符
        var stringBuilder = new StringBuilder(bytes.Length * 4);
        foreach (var b in bytes)
        {
            stringBuilder.Append(b.ToString(format));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组的指定范围转换为16进制字符串。
    /// </summary>
    /// <remarks>
    /// Converts a specified range of a byte array to a hexadecimal string.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <param name="offset">起始偏移量 / The starting offset.</param>
    /// <param name="count">要转换的字节数 / The number of bytes to convert.</param>
    /// <returns>16进制字符串 / A hexadecimal string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 或 <paramref name="count"/> 超出有效范围时抛出 / Thrown when <paramref name="offset"/> or <paramref name="count"/> is out of valid range.</exception>
    public static string ToHex(this byte[] bytes, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (offset + count > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetCountExceedBufferLength), nameof(count));
        }

        // 预分配容量: 每个字节 2 个十六进制字符
        var stringBuilder = new StringBuilder(count * 2);
        for (var i = offset; i < offset + count; ++i)
        {
            stringBuilder.Append(bytes[i].ToString("X2"));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组转换为系统默认编码的字符串。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a string using the system default encoding.
    /// Uses <see cref="Encoding.Default"/> for encoding conversion. To use UTF-8 encoding, use the <see cref="ToUtf8String(byte[])"/> method.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <returns>系统默认编码的字符串 / A string in system default encoding.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static string ToDefaultString(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return Encoding.Default.GetString(bytes);
    }

    /// <summary>
    /// 将字节数组的指定范围转换为系统默认编码的字符串。
    /// </summary>
    /// <remarks>
    /// Converts a specified range of a byte array to a string using the system default encoding.
    /// Uses <see cref="Encoding.Default"/> for encoding conversion. To use UTF-8 encoding, use the <see cref="ToUtf8String(byte[], int, int)"/> method.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <param name="index">起始偏移量 / The starting offset.</param>
    /// <param name="count">要转换的字节数 / The number of bytes to convert.</param>
    /// <returns>系统默认编码的字符串 / A string in system default encoding.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 或 <paramref name="count"/> 超出有效范围时抛出 / Thrown when <paramref name="index"/> or <paramref name="count"/> is out of valid range.</exception>
    public static string ToDefaultString(this byte[] bytes, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (index + count > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IndexCountExceedBufferLength), nameof(count));
        }

        return Encoding.Default.GetString(bytes, index, count);
    }

    /// <summary>
    /// 将字节数组转换为UTF8编码的字符串。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a UTF-8 encoded string.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <returns>UTF8编码的字符串 / A UTF-8 encoded string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static string ToUtf8String(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// 将字节数组的指定范围转换为UTF8编码的字符串。
    /// </summary>
    /// <remarks>
    /// Converts a specified range of a byte array to a UTF-8 encoded string.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <param name="index">起始偏移量 / The starting offset.</param>
    /// <param name="count">要转换的字节数 / The number of bytes to convert.</param>
    /// <returns>UTF8编码的字符串 / A UTF-8 encoded string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 或 <paramref name="count"/> 超出有效范围时抛出 / Thrown when <paramref name="index"/> or <paramref name="count"/> is out of valid range.</exception>
    public static string ToUtf8String(this byte[] bytes, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (index + count > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IndexCountExceedBufferLength), nameof(count));
        }

        return Encoding.UTF8.GetString(bytes, index, count);
    }

    #endregion

    #region Additional Utility Methods

    /// <summary>
    /// 比较两个字节数组是否相等。
    /// </summary>
    /// <remarks>
    /// Compares two byte arrays for equality.
    /// </remarks>
    /// <param name="bytes1">第一个字节数组 / The first byte array.</param>
    /// <param name="bytes2">第二个字节数组 / The second byte array.</param>
    /// <returns>如果两个字节数组相等则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the byte arrays are equal; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes1"/> 或 <paramref name="bytes2"/> 为 null 时抛出 / Thrown when <paramref name="bytes1"/> or <paramref name="bytes2"/> is null.</exception>
    public static bool SequenceEqual(this byte[] bytes1, byte[] bytes2)
    {
        ArgumentNullException.ThrowIfNull(bytes1, nameof(bytes1));
        ArgumentNullException.ThrowIfNull(bytes2, nameof(bytes2));

        return ((ReadOnlySpan<byte>)bytes1).SequenceEqual(bytes2);
    }

    /// <summary>
    /// 使用指定的值填充字节数组。
    /// </summary>
    /// <remarks>
    /// Fills the byte array with the specified value.
    /// </remarks>
    /// <param name="bytes">要填充的字节数组 / The byte array to fill.</param>
    /// <param name="value">用于填充的值 / The value to fill with.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static void Fill(this byte[] bytes, byte value)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        Array.Fill(bytes, value);
    }

    /// <summary>
    /// 使用指定的值填充字节数组的指定范围。
    /// </summary>
    /// <remarks>
    /// Fills a specified range of the byte array with the specified value.
    /// </remarks>
    /// <param name="bytes">要填充的字节数组 / The byte array to fill.</param>
    /// <param name="value">用于填充的值 / The value to fill with.</param>
    /// <param name="startIndex">填充的起始索引 / The starting index for filling.</param>
    /// <param name="count">要填充的字节数 / The number of bytes to fill.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 或 <paramref name="count"/> 超出有效范围时抛出 / Thrown when <paramref name="startIndex"/> or <paramref name="count"/> is out of valid range.</exception>
    public static void Fill(this byte[] bytes, byte value, int startIndex, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (startIndex + count > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IndexCountExceedBufferLength, bytes.Length, startIndex, count), nameof(count));
        }

        Array.Fill(bytes, value, startIndex, count);
    }

    /// <summary>
    /// 反转字节数组中的字节顺序。
    /// </summary>
    /// <remarks>
    /// Reverses the byte order in the byte array.
    /// </remarks>
    /// <param name="bytes">要反转的字节数组 / The byte array to reverse.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static void Reverse(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        Array.Reverse(bytes);
    }

    /// <summary>
    /// 反转字节数组指定范围内的字节顺序。
    /// </summary>
    /// <remarks>
    /// Reverses the byte order in a specified range of the byte array.
    /// </remarks>
    /// <param name="bytes">要反转的字节数组 / The byte array to reverse.</param>
    /// <param name="index">反转范围的起始索引 / The starting index of the range to reverse.</param>
    /// <param name="length">要反转的字节数 / The number of bytes to reverse.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 或 <paramref name="length"/> 超出有效范围时抛出 / Thrown when <paramref name="index"/> or <paramref name="length"/> is out of valid range.</exception>
    public static void Reverse(this byte[] bytes, int index, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        if (index + length > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IndexCountExceedBufferLength, bytes.Length, index, length), nameof(length));
        }

        Array.Reverse(bytes, index, length);
    }

    /// <summary>
    /// 将字节数组转换为Base64字符串。
    /// </summary>
    /// <remarks>
    /// Converts a byte array to a Base64 string.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <returns>Base64编码的字符串 / A Base64 encoded string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static string ToBase64String(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 将字节数组的指定范围转换为Base64字符串。
    /// </summary>
    /// <remarks>
    /// Converts a specified range of a byte array to a Base64 string.
    /// </remarks>
    /// <param name="bytes">要转换的字节数组 / The byte array to convert.</param>
    /// <param name="offset">起始偏移量 / The starting offset.</param>
    /// <param name="length">要转换的字节数 / The number of bytes to convert.</param>
    /// <returns>Base64编码的字符串 / A Base64 encoded string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 或 <paramref name="length"/> 超出有效范围时抛出 / Thrown when <paramref name="offset"/> or <paramref name="length"/> is out of valid range.</exception>
    public static string ToBase64String(this byte[] bytes, int offset, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        if (offset + length > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IndexCountExceedBufferLength, bytes.Length, offset, length), nameof(length));
        }

        return Convert.ToBase64String(bytes, offset, length);
    }

    /// <summary>
    /// 将 Base64 字符串转换为字节数组。
    /// </summary>
    /// <remarks>
    /// Converts a Base64 string to a byte array.
    /// </remarks>
    /// <param name="base64String">Base64 编码的字符串 / The Base64 encoded string.</param>
    /// <returns>解码后的字节数组。如果输入为空字符串，返回空数组 / The decoded byte array. Returns an empty array if input is an empty string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="base64String"/> 为 null 时抛出 / Thrown when <paramref name="base64String"/> is null.</exception>
    public static byte[] ToByteArrayFromBase64(this string base64String)
    {
        ArgumentNullException.ThrowIfNull(base64String, nameof(base64String));
        if (string.IsNullOrEmpty(base64String))
        {
            return Array.Empty<byte>();
        }

        return Convert.FromBase64String(base64String);
    }

    /// <summary>
    /// 对两个字节数组执行异或操作。
    /// </summary>
    /// <remarks>
    /// Performs an XOR operation on two byte arrays.
    /// </remarks>
    /// <param name="bytes1">第一个字节数组 / The first byte array.</param>
    /// <param name="bytes2">第二个字节数组 / The second byte array.</param>
    /// <returns>异或操作的结果字节数组 / The result byte array from the XOR operation.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes1"/> 或 <paramref name="bytes2"/> 为 null 时抛出 / Thrown when <paramref name="bytes1"/> or <paramref name="bytes2"/> is null.</exception>
    /// <exception cref="ArgumentException">当两个字节数组长度不相等时抛出 / Thrown when the two byte arrays have different lengths.</exception>
    public static byte[] Xor(this byte[] bytes1, byte[] bytes2)
    {
        ArgumentNullException.ThrowIfNull(bytes1, nameof(bytes1));
        ArgumentNullException.ThrowIfNull(bytes2, nameof(bytes2));

        if (bytes1.Length != bytes2.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.ByteArrayLengthMustEqual));
        }

        var result = new byte[bytes1.Length];
        for (int i = 0; i < bytes1.Length; i++)
        {
            result[i] = (byte)(bytes1[i] ^ bytes2[i]);
        }

        return result;
    }

    /// <summary>
    /// 对字节数组与单个字节执行异或操作。
    /// </summary>
    /// <remarks>
    /// Performs an XOR operation on a byte array with a single byte value.
    /// </remarks>
    /// <param name="bytes">字节数组 / The byte array.</param>
    /// <param name="value">用于异或的字节值 / The byte value to XOR with.</param>
    /// <returns>异或操作的结果字节数组 / The result byte array from the XOR operation.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static byte[] Xor(this byte[] bytes, byte value)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        var result = new byte[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            result[i] = (byte)(bytes[i] ^ value);
        }

        return result;
    }

    /// <summary>
    /// 在字节数组中查找指定字节序列的第一个匹配位置。
    /// </summary>
    /// <remarks>
    /// Finds the first occurrence of a specified byte sequence in a byte array.
    /// </remarks>
    /// <param name="bytes">要搜索的字节数组 / The byte array to search.</param>
    /// <param name="pattern">要查找的字节序列 / The byte sequence to find.</param>
    /// <returns>第一个匹配位置的索引，如果未找到则返回 -1 / The index of the first occurrence, or -1 if not found.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="pattern"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> or <paramref name="pattern"/> is null.</exception>
    public static int IndexOf(this byte[] bytes, byte[] pattern)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentNullException.ThrowIfNull(pattern, nameof(pattern));

        if (pattern.Length == 0)
        {
            return 0;
        }

        if (pattern.Length > bytes.Length)
        {
            return -1;
        }

        for (int i = 0; i <= bytes.Length - pattern.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < pattern.Length; j++)
            {
                if (bytes[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 在字节数组中查找指定字节的第一个匹配位置。
    /// </summary>
    /// <remarks>
    /// Finds the first occurrence of a specified byte in a byte array.
    /// </remarks>
    /// <param name="bytes">要搜索的字节数组 / The byte array to search.</param>
    /// <param name="value">要查找的字节值 / The byte value to find.</param>
    /// <returns>第一个匹配位置的索引，如果未找到则返回 -1 / The index of the first occurrence, or -1 if not found.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    public static int IndexOf(this byte[] bytes, byte value)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] == value)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 从指定位置开始在字节数组中查找指定字节的第一个匹配位置。
    /// </summary>
    /// <remarks>
    /// Finds the first occurrence of a specified byte in a byte array starting from a specified position.
    /// </remarks>
    /// <param name="bytes">要搜索的字节数组 / The byte array to search.</param>
    /// <param name="value">要查找的字节值 / The byte value to find.</param>
    /// <param name="startIndex">搜索的起始位置 / The starting position for the search.</param>
    /// <returns>第一个匹配位置的索引，如果未找到则返回 -1 / The index of the first occurrence, or -1 if not found.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 超出有效范围时抛出 / Thrown when <paramref name="startIndex"/> is out of valid range.</exception>
    public static int IndexOf(this byte[] bytes, byte value, int startIndex)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));

        if (startIndex >= bytes.Length)
        {
            return -1;
        }

        for (int i = startIndex; i < bytes.Length; i++)
        {
            if (bytes[i] == value)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// 检查字节数组是否以指定的字节序列开头。
    /// </summary>
    /// <remarks>
    /// Checks whether a byte array starts with the specified byte sequence.
    /// </remarks>
    /// <param name="bytes">要检查的字节数组 / The byte array to check.</param>
    /// <param name="prefix">前缀字节序列 / The prefix byte sequence.</param>
    /// <returns>如果字节数组以指定序列开头则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the byte array starts with the specified sequence; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="prefix"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> or <paramref name="prefix"/> is null.</exception>
    public static bool StartsWith(this byte[] bytes, byte[] prefix)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentNullException.ThrowIfNull(prefix, nameof(prefix));

        if (prefix.Length > bytes.Length)
        {
            return false;
        }

        for (int i = 0; i < prefix.Length; i++)
        {
            if (bytes[i] != prefix[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 检查字节数组是否以指定的字节序列结尾。
    /// </summary>
    /// <remarks>
    /// Checks whether a byte array ends with the specified byte sequence.
    /// </remarks>
    /// <param name="bytes">要检查的字节数组 / The byte array to check.</param>
    /// <param name="suffix">后缀字节序列 / The suffix byte sequence.</param>
    /// <returns>如果字节数组以指定序列结尾则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the byte array ends with the specified sequence; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="suffix"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> or <paramref name="suffix"/> is null.</exception>
    public static bool EndsWith(this byte[] bytes, byte[] suffix)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentNullException.ThrowIfNull(suffix, nameof(suffix));

        if (suffix.Length > bytes.Length)
        {
            return false;
        }

        int startIndex = bytes.Length - suffix.Length;
        for (int i = 0; i < suffix.Length; i++)
        {
            if (bytes[startIndex + i] != suffix[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 将当前字节数组与其他字节数组连接。
    /// </summary>
    /// <remarks>
    /// Concatenates the current byte array with other byte arrays.
    /// </remarks>
    /// <param name="first">第一个字节数组 / The first byte array.</param>
    /// <param name="others">要连接的其他字节数组集合 / The collection of other byte arrays to concatenate.</param>
    /// <returns>连接后的字节数组 / The concatenated byte array.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/> 或 <paramref name="others"/> 为 null 时抛出 / Thrown when <paramref name="first"/> or <paramref name="others"/> is null.</exception>
    public static byte[] Concat(this byte[] first, params byte[][] others)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(others, nameof(others));

        int totalLength = first.Length;
        foreach (var array in others)
        {
            if (array != null)
            {
                totalLength += array.Length;
            }
        }

        var result = new byte[totalLength];
        first.AsSpan().CopyTo(result);
        int offset = first.Length;

        foreach (var array in others)
        {
            if (array != null)
            {
                array.AsSpan().CopyTo(result.AsSpan(offset));
                offset += array.Length;
            }
        }

        return result;
    }

    /// <summary>
    /// 获取字节数组的子数组。
    /// </summary>
    /// <remarks>
    /// Gets a subarray of the byte array.
    /// </remarks>
    /// <param name="bytes">源字节数组 / The source byte array.</param>
    /// <param name="startIndex">起始索引 / The starting index.</param>
    /// <returns>子数组 / The subarray.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 超出有效范围时抛出 / Thrown when <paramref name="startIndex"/> is out of valid range.</exception>
    public static byte[] SubArray(this byte[] bytes, int startIndex)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));

        if (startIndex >= bytes.Length)
        {
            return Array.Empty<byte>();
        }

        return SubArray(bytes, startIndex, bytes.Length - startIndex);
    }

    /// <summary>
    /// 获取字节数组的子数组。
    /// </summary>
    /// <remarks>
    /// Gets a subarray of the byte array with the specified length.
    /// </remarks>
    /// <param name="bytes">源字节数组 / The source byte array.</param>
    /// <param name="startIndex">起始索引 / The starting index.</param>
    /// <param name="length">子数组长度 / The length of the subarray.</param>
    /// <returns>子数组 / The subarray.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 或 <paramref name="length"/> 超出有效范围时抛出 / Thrown when <paramref name="startIndex"/> or <paramref name="length"/> is out of valid range.</exception>
    public static byte[] SubArray(this byte[] bytes, int startIndex, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        if (startIndex + length > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.IndexCountExceedBufferLength, bytes.Length, startIndex, length), nameof(length));
        }

        var result = new byte[length];
        bytes.AsSpan(startIndex, length).CopyTo(result);
        return result;
    }

    #endregion

    #region Primitive Type Read/Write

    /// <summary>
    /// 将一个字节写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a byte to the specified buffer and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteByteValue(this byte[] buffer, byte value, ref int offset)
    {
        ValidateBounds(buffer, offset, 1);
        buffer[offset] = value;
        offset += 1;
    }

    /// <summary>
    /// 从字节数组中读取一个字节，并将偏移量前移。
    /// </summary>
    /// <remarks>
    /// Reads a byte from the byte array and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的字节 / Returns the read byte.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static byte ReadByteValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, 1);
        var value = buffer[offset];
        offset += 1;
        return value;
    }

    /// <summary>
    /// 将一个带符号字节写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a signed byte to the specified buffer and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteSByteValue(this byte[] buffer, sbyte value, ref int offset)
    {
        ValidateBounds(buffer, offset, 1);
        buffer[offset] = (byte)value;
        offset += 1;
    }

    /// <summary>
    /// 从字节数组中读取一个带符号字节，并将偏移量前移。
    /// </summary>
    /// <remarks>
    /// Reads a signed byte from the byte array and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的带符号字节 / Returns the read signed byte.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static sbyte ReadSByteValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, 1);
        var value = (sbyte)buffer[offset];
        offset += 1;
        return value;
    }

    /// <summary>
    /// 将一个布尔值写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a boolean value to the specified buffer and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteBoolValue(this byte[] buffer, bool value, ref int offset)
    {
        ValidateBounds(buffer, offset, 1);
        buffer[offset] = value ? (byte)1 : (byte)0;
        offset += 1;
    }

    /// <summary>
    /// 从字节数组中读取一个布尔值，并将偏移量前移。
    /// </summary>
    /// <remarks>
    /// Reads a boolean value from the byte array and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的布尔值 / Returns the read boolean value.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static bool ReadBoolValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, 1);
        var value = buffer[offset] != 0;
        offset += 1;
        return value;
    }

    #endregion
}
