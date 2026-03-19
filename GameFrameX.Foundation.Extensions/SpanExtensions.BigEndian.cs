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

public static partial class SpanExtensions
{
    #region Write BigEndian

    /// <summary>
    /// 将一个32位无符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 32-bit unsigned integer to the specified buffer in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的32位无符号整数值 / The 32-bit unsigned integer value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加4字节 / The starting position for read/write, automatically increments by 4 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteUIntBigEndianValue(this Span<byte> buffer, uint value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.UIntSize, buffer.Length));
        }

        BinaryPrimitives.WriteUInt32BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.UIntSize;
    }

    /// <summary>
    /// 将一个16位无符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 16-bit unsigned integer to the specified buffer in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的16位无符号整数值 / The 16-bit unsigned integer value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加2字节 / The starting position for read/write, automatically increments by 2 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteUShortBigEndianValue(this Span<byte> buffer, ushort value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.UShortSize, buffer.Length));
        }

        BinaryPrimitives.WriteUInt16BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.UShortSize;
    }

    /// <summary>
    /// 将一个16位有符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 16-bit signed integer to the specified buffer in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的16位有符号整数值 / The 16-bit signed integer value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加2字节 / The starting position for read/write, automatically increments by 2 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteShortBigEndianValue(this Span<byte> buffer, short value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.ShortSize, buffer.Length));
        }

        BinaryPrimitives.WriteInt16BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.ShortSize;
    }

    /// <summary>
    /// 将32位有符号整数以大端字节序写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 32-bit signed integer to the specified byte span in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的32位有符号整数值 / The 32-bit signed integer value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加4字节 / The starting position for read/write, automatically increments by 4 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteIntBigEndianValue(this Span<byte> buffer, int value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.IntSize, buffer.Length));
        }

        BinaryPrimitives.WriteInt32BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.IntSize;
    }

    /// <summary>
    /// 将64位有符号整数以大端字节序写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 64-bit signed integer to the specified byte span in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的64位有符号整数值 / The 64-bit signed integer value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加8字节 / The starting position for read/write, automatically increments by 8 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteLongBigEndianValue(this Span<byte> buffer, long value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.LongSize, buffer.Length));
        }

        BinaryPrimitives.WriteInt64BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.LongSize;
    }

    /// <summary>
    /// 将一个64位无符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a 64-bit unsigned integer to the specified buffer in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的64位无符号整数值 / The 64-bit unsigned integer value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加8字节 / The starting position for read/write, automatically increments by 8 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteULongBigEndianValue(this Span<byte> buffer, ulong value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.ULongSize, buffer.Length));
        }

        BinaryPrimitives.WriteUInt64BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.ULongSize;
    }

    /// <summary>
    /// 将单精度浮点数以大端字节序写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a single-precision floating-point number to the specified byte span in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的单精度浮点数值 / The single-precision floating-point value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加4字节 / The starting position for read/write, automatically increments by 4 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteFloatBigEndianValue(this Span<byte> buffer, float value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.FloatSize, buffer.Length));
        }

        BinaryPrimitives.WriteSingleBigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.FloatSize;
    }

    /// <summary>
    /// 将双精度浮点数以大端字节序写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a double-precision floating-point number to the specified byte span in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的双精度浮点数值 / The double-precision floating-point value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加8字节 / The starting position for read/write, automatically increments by 8 bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteDoubleBigEndianValue(this Span<byte> buffer, double value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.DoubleSize, buffer.Length));
        }

        BinaryPrimitives.WriteDoubleBigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.DoubleSize;
    }

    /// <summary>
    /// 在给定的偏移量位置，向缓冲区中写入字节序列，包含长度信息。
    /// </summary>
    /// <remarks>
    /// Writes a byte sequence to the buffer at the given offset position with length information.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的字节数组 / The byte array to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数（4字节长度 + 数据长度）/ The starting position for read/write, automatically increments by the corresponding bytes (4-byte length + data length).</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 null 时抛出 / Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteBytesValue(this Span<byte> buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        WriteIntBigEndianValue(buffer, value.Length, ref offset);

        if (value.Length > 0)
        {
            WriteBytesWithoutLength(buffer, value, ref offset);
        }
    }

    /// <summary>
    /// 在给定的偏移量位置，向缓冲区中写入字符串，包含长度信息。
    /// </summary>
    /// <remarks>
    /// Writes a string to the buffer at the given offset position with length information using UTF-8 encoding.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的字符串，使用 UTF-8 编码 / The string to write using UTF-8 encoding.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数（2字节长度 + UTF-8字节数据）/ The starting position for read/write, automatically increments by the corresponding bytes (2-byte length + UTF-8 data).</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 null 时抛出 / Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数、超出缓冲区边界或字符串过长（超过65535字节）时抛出 / Thrown when <paramref name="offset"/> is negative, exceeds buffer bounds, or string is too long (exceeds 65535 bytes).</exception>
    public static void WriteStringValue(this Span<byte> buffer, string value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (string.IsNullOrEmpty(value))
        {
            WriteShortBigEndianValue(buffer, 0, ref offset);
            return;
        }

        var bytes = Encoding.UTF8.GetBytes(value);

        if (bytes.Length > ushort.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.StringTooLong, ushort.MaxValue));
        }

        WriteShortBigEndianValue(buffer, (short)bytes.Length, ref offset);
        WriteBytesWithoutLength(buffer, bytes, ref offset);
    }

    /// <summary>
    /// 将字符串直接写入指定的缓冲区，不包含长度前缀，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a string directly to the specified buffer without a length prefix and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的字符串 / The string to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出 / Thrown when <paramref name="offset"/> is negative or buffer space is insufficient.</exception>
    public static void WriteStringWithoutLength(this Span<byte> buffer, string value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (value == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var bytes = Encoding.UTF8.GetBytes(value);

        if (offset + bytes.Length > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.BufferTooSmall, offset + bytes.Length, buffer.Length));
        }

        WriteBytesWithoutLength(buffer, bytes, ref offset);
    }

    #endregion

    #region Read BigEndian

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个32位有符号整数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a 32-bit signed integer from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加4字节 / The starting position for read/write, automatically increments by 4 bytes after reading.</param>
    /// <returns>返回读取的32位有符号整数值 / Returns the read 32-bit signed integer value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static int ReadIntBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadInt32BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个16位有符号整数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a 16-bit signed integer from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加2字节 / The starting position for read/write, automatically increments by 2 bytes after reading.</param>
    /// <returns>返回读取的16位有符号整数值 / Returns the read 16-bit signed integer value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static short ReadShortBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadInt16BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个16位无符号整数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a 16-bit unsigned integer from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加2字节 / The starting position for read/write, automatically increments by 2 bytes after reading.</param>
    /// <returns>返回读取的16位无符号整数值 / Returns the read 16-bit unsigned integer value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static ushort ReadUShortBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadUInt16BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个32位无符号整数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a 32-bit unsigned integer from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加4字节 / The starting position for read/write, automatically increments by 4 bytes after reading.</param>
    /// <returns>返回读取的32位无符号整数值 / Returns the read 32-bit unsigned integer value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static uint ReadUIntBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadUInt32BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个64位无符号整数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a 64-bit unsigned integer from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加8字节 / The starting position for read/write, automatically increments by 8 bytes after reading.</param>
    /// <returns>返回读取的64位无符号整数值 / Returns the read 64-bit unsigned integer value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static ulong ReadULongBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadUInt64BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个64位有符号整数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a 64-bit signed integer from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加8字节 / The starting position for read/write, automatically increments by 8 bytes after reading.</param>
    /// <returns>返回读取的64位有符号整数值 / Returns the read 64-bit signed integer value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static long ReadLongBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadInt64BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个单精度浮点数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a single-precision floating-point value from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加4字节 / The starting position for read/write, automatically increments by 4 bytes after reading.</param>
    /// <returns>返回读取的单精度浮点数值 / Returns the read single-precision floating-point value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static float ReadFloatBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadSingleBigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量以大端字节序读取一个双精度浮点数值，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a double-precision floating-point value from the specified byte buffer at the given offset in big-endian byte order and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加8字节 / The starting position for read/write, automatically increments by 8 bytes after reading.</param>
    /// <returns>返回读取的双精度浮点数值 / Returns the read double-precision floating-point value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static double ReadDoubleBigEndianValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadDoubleBigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量读取一个字节数组，包含长度信息。
    /// </summary>
    /// <remarks>
    /// Reads a byte array from the specified byte buffer at the given offset with length information.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数（4字节长度 + 数据长度）/ The starting position for read/write, automatically increments by the corresponding bytes (4-byte length + data length).</param>
    /// <returns>返回读取的字节数组，如果长度为0则返回空数组 / Returns the read byte array; returns an empty array if length is 0.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static byte[] ReadBytesValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        var length = ReadIntBigEndianValue(buffer, ref offset);

        if (length <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + length > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var data = new byte[length];
        buffer.Slice(offset, length).CopyTo(data);
        offset += length;
        return data;
    }

    /// <summary>
    /// 从指定的字节缓冲区和偏移量读取一个字符串，包含长度信息。
    /// </summary>
    /// <remarks>
    /// Reads a string from the specified byte buffer at the given offset with length information using UTF-8 decoding.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数（2字节长度 + UTF-8字节数据）/ The starting position for read/write, automatically increments by the corresponding bytes (2-byte length + UTF-8 data).</param>
    /// <returns>返回读取的字符串，使用 UTF-8 解码，如果长度为0则返回空字符串 / Returns the read string using UTF-8 decoding; returns an empty string if length is 0.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static string ReadStringValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        var length = ReadShortBigEndianValue(buffer, ref offset);

        if (length <= 0)
        {
            return string.Empty;
        }

        if (offset + length > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = Encoding.UTF8.GetString(buffer.Slice(offset, length));
        offset += length;
        return value;
    }

    /// <summary>
    /// 从指定偏移量开始读取指定长度的字节数组。
    /// </summary>
    /// <remarks>
    /// Reads a byte array of specified length starting from the specified offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding bytes.</param>
    /// <param name="len">需要读取的字节数组长度 / The length of the byte array to read.</param>
    /// <returns>返回从缓冲区读取的字节数组，如果长度小于等于0则返回空数组 / Returns the byte array read from the buffer; returns an empty array if length is less than or equal to 0.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static byte[] ReadBytesValue(this Span<byte> buffer, ref int offset, int len)
    {
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
        buffer.Slice(offset, len).CopyTo(data);
        offset += len;
        return data;
    }

    /// <summary>
    /// 从指定偏移量开始读取指定长度的字符串。
    /// </summary>
    /// <remarks>
    /// Reads a string of specified byte length starting from the specified offset using UTF-8 decoding.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding bytes.</param>
    /// <param name="len">要读取的字符串字节长度 / The byte length of the string to read.</param>
    /// <returns>返回读取的字符串，使用 UTF-8 解码，如果长度小于等于0则返回空字符串 / Returns the read string using UTF-8 decoding; returns an empty string if length is less than or equal to 0.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static string ReadStringValue(this Span<byte> buffer, ref int offset, int len)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (len <= 0)
        {
            return string.Empty;
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = Encoding.UTF8.GetString(buffer.Slice(offset, len));
        offset += len;
        return value;
    }

    #endregion
}
