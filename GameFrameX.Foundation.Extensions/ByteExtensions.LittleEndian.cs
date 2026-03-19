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
using System.Buffers.Binary;
using System.Text;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字节数组的小端字节序（LittleEndian）读写扩展方法。
/// </summary>
/// <remarks>
/// Provides little-endian read/write extension methods for byte arrays.
/// </remarks>
public static partial class ByteExtensions
{
    #region Write LittleEndian

    /// <summary>
    /// 将一个16位有符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 16-bit signed integer to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteShortLittleEndianValue(this byte[] buffer, short value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ShortSize);
        BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ShortSize;
    }

    /// <summary>
    /// 将一个16位无符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 16-bit unsigned integer to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteUShortLittleEndianValue(this byte[] buffer, ushort value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UShortSize);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UShortSize;
    }

    /// <summary>
    /// 将一个32位有符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 32-bit signed integer to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteIntLittleEndianValue(this byte[] buffer, int value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.IntSize);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.IntSize;
    }

    /// <summary>
    /// 将一个32位无符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 32-bit unsigned integer to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteUIntLittleEndianValue(this byte[] buffer, uint value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UIntSize);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UIntSize;
    }

    /// <summary>
    /// 将一个64位有符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 64-bit signed integer to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteLongLittleEndianValue(this byte[] buffer, long value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.LongSize);
        BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.LongSize;
    }

    /// <summary>
    /// 将一个64位无符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 64-bit unsigned integer to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteULongLittleEndianValue(this byte[] buffer, ulong value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ULongSize);
        BinaryPrimitives.WriteUInt64LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ULongSize;
    }

    /// <summary>
    /// 将一个单精度浮点数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a single-precision floating-point number to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteFloatLittleEndianValue(this byte[] buffer, float value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.FloatSize);
        BinaryPrimitives.WriteSingleLittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.FloatSize;
    }

    /// <summary>
    /// 将一个双精度浮点数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a double-precision floating-point number to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteDoubleLittleEndianValue(this byte[] buffer, double value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.DoubleSize);
        BinaryPrimitives.WriteDoubleLittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.DoubleSize;
    }

    /// <summary>
    /// 将一个字节数组（带长度前缀）以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a byte array (with length prefix) to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值，不能为 null / The value to write, cannot be null.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 或 <paramref name="value"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> or <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteBytesLittleEndianValue(this byte[] buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + value.Length + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                                                  LocalizationService.GetString(LocalizationKeys.Exceptions.BufferTooSmall, offset + value.Length + ConstBaseTypeSize.IntSize, buffer.Length));
        }

        buffer.WriteIntLittleEndianValue(value.Length, ref offset);
        value.AsSpan().CopyTo(buffer.AsSpan(offset, value.Length));
        offset += value.Length;
    }

    /// <summary>
    /// 将字节数组直接写入指定的缓冲区，不包含长度前缀，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a byte array directly to the specified buffer without length prefix and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的字节数组，不能为 null / The byte array to write, cannot be null.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 或 <paramref name="value"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> or <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteBytesWithoutLengthLittleEndian(this byte[] buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + value.Length > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                                                  LocalizationService.GetString(LocalizationKeys.Exceptions.BufferTooSmall, offset + value.Length, buffer.Length));
        }

        value.AsSpan().CopyTo(buffer.AsSpan(offset, value.Length));
        offset += value.Length;
    }

    /// <summary>
    /// 将一个字符串以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a string to the specified buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的值 / The value to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteStringLittleEndianValue(this byte[] buffer, string value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (value == null)
        {
            value = string.Empty;
        }

        var len = Encoding.UTF8.GetByteCount(value);

        if (len > short.MaxValue)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.StringLengthExceedMaxValue, len, short.MaxValue));
        }

        if (offset + len + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.BufferTooSmall, offset + len + ConstBaseTypeSize.ShortSize, buffer.Length));
        }

        Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, offset + ConstBaseTypeSize.ShortSize);
        WriteShortLittleEndianValue(buffer, (short)len, ref offset);
        offset += len;
    }

    /// <summary>
    /// 将字符串直接写入指定的缓冲区，不包含长度前缀，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a string directly to the specified buffer without length prefix and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的缓冲区 / The buffer to write to.</param>
    /// <param name="value">要写入的字符串 / The string to write.</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量 / The offset in the buffer to write the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteStringWithoutLengthLittleEndian(this byte[] buffer, string value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (value == null)
        {
            return;
        }

        var len = Encoding.UTF8.GetByteCount(value);

        if (len == 0)
        {
            return;
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.BufferTooSmall, offset + len, buffer.Length));
        }

        Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, offset);
        offset += len;
    }

    #endregion

    #region Read LittleEndian

    /// <summary>
    /// 从字节数组中以小端字节序读取16位有符号整数，并将偏移量前移。
    /// </summary>
    /// <remarks>
    /// Reads a 16-bit signed integer from the byte array in little-endian byte order and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的16位有符号整数 / Returns the read 16-bit signed integer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static short ReadShortLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ShortSize);
        var value = BinaryPrimitives.ReadInt16LittleEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以小端字节序读取16位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <remarks>
    /// Reads a 16-bit unsigned integer from the byte array in little-endian byte order and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的16位无符号整数 / Returns the read 16-bit unsigned integer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static ushort ReadUShortLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UShortSize);
        var value = BinaryPrimitives.ReadUInt16LittleEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以小端字节序读取32位有符号整数，并将偏移量向前移动。
    /// </summary>
    /// <remarks>
    /// Reads a 32-bit signed integer from the byte array in little-endian byte order and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的32位有符号整数 / Returns the read 32-bit signed integer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static int ReadIntLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.IntSize);
        var value = BinaryPrimitives.ReadInt32LittleEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以小端字节序读取32位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <remarks>
    /// Reads a 32-bit unsigned integer from the byte array in little-endian byte order and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的32位无符号整数 / Returns the read 32-bit unsigned integer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static uint ReadUIntLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UIntSize);
        var value = BinaryPrimitives.ReadUInt32LittleEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以小端字节序读取64位有符号整数，并将偏移量向前移动。
    /// </summary>
    /// <remarks>
    /// Reads a 64-bit signed integer from the byte array in little-endian byte order and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的64位有符号整数 / Returns the read 64-bit signed integer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static long ReadLongLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.LongSize);
        var value = BinaryPrimitives.ReadInt64LittleEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以小端字节序读取64位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <remarks>
    /// Reads a 64-bit unsigned integer from the byte array in little-endian byte order and advances the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节数组 / The byte array to read from.</param>
    /// <param name="offset">引用偏移量 / The reference offset.</param>
    /// <returns>返回读取的64位无符号整数 / Returns the read 64-bit unsigned integer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static ulong ReadULongLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ULongSize);
        var value = BinaryPrimitives.ReadUInt64LittleEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节缓冲区中以小端字节序读取浮点数，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a single-precision floating-point number from the given byte buffer in little-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">包含了要读取数据的字节缓冲区 / The byte buffer containing the data to read.</param>
    /// <param name="offset">读取数据的起始位置，该方法会更新该值 / The starting position for reading, this method will update this value.</param>
    /// <returns>从字节缓冲区中读取的浮点数 / The single-precision floating-point number read from the byte buffer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static float ReadFloatLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.FloatSize);
        var value = BinaryPrimitives.ReadSingleLittleEndian(buffer.AsSpan(offset));
        offset += ConstBaseTypeSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 从指定偏移量以小端字节序读取 double 类型数据。
    /// </summary>
    /// <remarks>
    /// Reads a double-precision floating-point number from the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要操作的字节缓冲区 / The byte buffer to operate on.</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加双精度浮点数的字节数 / The starting offset for the operation, automatically increments by the size of a double after the operation.</param>
    /// <returns>返回从缓冲区读取的 double 类型数据 / Returns the double-precision floating-point number read from the buffer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static double ReadDoubleLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.DoubleSize);
        var value = BinaryPrimitives.ReadDoubleLittleEndian(buffer.AsSpan(offset));
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }

    /// <summary>
    /// 从指定偏移量开始读取指定长度的字节数组（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a byte array of the specified length from the specified offset (little-endian byte order).
    /// </remarks>
    /// <param name="buffer">要操作的字节缓冲区 / The byte buffer to operate on.</param>
    /// <param name="offset">操作的起始偏移量 / The starting offset for the operation.</param>
    /// <param name="len">需要读取的字节数组长度 / The length of the byte array to read.</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据 / Returns the byte[] data read from the buffer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static byte[] ReadBytesLittleEndianValue(this byte[] buffer, int offset, int len)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var data = new byte[len];
        buffer.AsSpan(offset, len).CopyTo(data);
        return data;
    }

    /// <summary>
    /// 从指定偏移量开始读取指定长度的字节数组（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a byte array of the specified length from the specified offset and updates the offset (little-endian byte order).
    /// </remarks>
    /// <param name="buffer">要操作的字节缓冲区 / The byte buffer to operate on.</param>
    /// <param name="offset">操作的起始偏移量 / The starting offset for the operation.</param>
    /// <param name="len">需要读取的字节数组长度 / The length of the byte array to read.</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据 / Returns the byte[] data read from the buffer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static byte[] ReadBytesLittleEndianValue(this byte[] buffer, ref int offset, int len)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var data = new byte[len];
        buffer.AsSpan(offset, len).CopyTo(data);
        offset += len;
        return data;
    }

    /// <summary>
    /// 从指定偏移量开始读取字节数组，长度作为 int 类型数据在字节数组的开头（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a byte array from the specified offset, with the length stored as an int at the beginning of the byte array (little-endian byte order).
    /// </remarks>
    /// <param name="buffer">要操作的字节缓冲区 / The byte buffer to operate on.</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加读取的字节长度以及 int 类型长度 / The starting offset for the operation, automatically increments by the read byte length plus int size after the operation.</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据 / Returns the byte[] data read from the buffer.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static byte[] ReadBytesLittleEndianValue(this byte[] buffer, ref int offset)
    {
        var len = ReadIntLittleEndianValue(buffer, ref offset);

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var data = new byte[len];
        buffer.AsSpan(offset, len).CopyTo(data);
        offset += len;
        return data;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取字符串（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a string from the byte array at the specified offset (little-endian byte order).
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组 / The byte array to read data from.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加 / The starting offset for reading, automatically increments after reading.</param>
    /// <returns>读取的字符串，若读取长度小于等于0或偏移量超出数组长度，返回空字符串 / The read string, returns an empty string if the read length is less than or equal to 0 or offset exceeds array length.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static string ReadStringLittleEndianValue(this byte[] buffer, ref int offset)
    {
        var len = ReadShortLittleEndianValue(buffer, ref offset);

        if (len <= 0)
        {
            return string.Empty;
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = Encoding.UTF8.GetString(buffer, offset, len);
        offset += len;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取指定长度的字符串（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a string of the specified length from the byte array at the specified offset (little-endian byte order).
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组 / The byte array to read data from.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加 / The starting offset for reading, automatically increments after reading.</param>
    /// <param name="len">要读取的字符串字节长度 / The byte length of the string to read.</param>
    /// <returns>读取的字符串，若读取长度小于等于0或偏移量超出数组长度，返回空字符串 / The read string, returns an empty string if the read length is less than or equal to 0 or offset exceeds array length.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出 / Thrown when <paramref name="buffer"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or read position exceeds buffer bounds.</exception>
    public static string ReadStringLittleEndianValue(this byte[] buffer, ref int offset, int len)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (len <= 0)
        {
            return string.Empty;
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = Encoding.UTF8.GetString(buffer, offset, len);
        offset += len;
        return value;
    }

    #endregion
}
