using System;
using System.Buffers.Binary;
using System.Text;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字节数组的大端字节序（BigEndian）读写扩展方法。
/// </summary>
public static partial class ByteExtensions
{
    #region Write BigEndian

    /// <summary>
    /// 将一个16位有符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteShortBigEndianValue(this byte[] buffer, short value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ShortSize);
        BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ShortSize;
    }

    /// <summary>
    /// 将一个16位无符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteUShortBigEndianValue(this byte[] buffer, ushort value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UShortSize);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UShortSize;
    }

    /// <summary>
    /// 将一个32位有符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteIntBigEndianValue(this byte[] buffer, int value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.IntSize);
        BinaryPrimitives.WriteInt32BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.IntSize;
    }

    /// <summary>
    /// 将一个32位无符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteUIntBigEndianValue(this byte[] buffer, uint value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UIntSize);
        BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UIntSize;
    }

    /// <summary>
    /// 将一个64位有符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteLongBigEndianValue(this byte[] buffer, long value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.LongSize);
        BinaryPrimitives.WriteInt64BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.LongSize;
    }

    /// <summary>
    /// 将一个64位无符号整数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteULongBigEndianValue(this byte[] buffer, ulong value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ULongSize);
        BinaryPrimitives.WriteUInt64BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ULongSize;
    }

    /// <summary>
    /// 将一个单精度浮点数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteFloatBigEndianValue(this byte[] buffer, float value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.FloatSize);
        BinaryPrimitives.WriteSingleBigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.FloatSize;
    }

    /// <summary>
    /// 将一个双精度浮点数以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteDoubleBigEndianValue(this byte[] buffer, double value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.DoubleSize);
        BinaryPrimitives.WriteDoubleBigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.DoubleSize;
    }

    /// <summary>
    /// 将一个字节数组（带长度前缀）以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值，不能为 null。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 或 <paramref name="value"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteBytesBigEndianValue(this byte[] buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + value.Length + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                                                  LocalizationService.GetString(LocalizationKeys.Exceptions.BufferTooSmall, offset + value.Length + ConstBaseTypeSize.IntSize, buffer.Length));
        }

        buffer.WriteIntBigEndianValue(value.Length, ref offset);
        value.AsSpan().CopyTo(buffer.AsSpan(offset, value.Length));
        offset += value.Length;
    }

    /// <summary>
    /// 将一个字符串以大端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteStringBigEndianValue(this byte[] buffer, string value, ref int offset)
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
        WriteShortBigEndianValue(buffer, (short)len, ref offset);
        offset += len;
    }

    #endregion

    #region Read BigEndian

    /// <summary>
    /// 从字节数组中以大端字节序读取16位有符号整数，并将偏移量前移。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的16位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static short ReadShortBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ShortSize);
        var value = BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以大端字节序读取16位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的16位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static ushort ReadUShortBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UShortSize);
        var value = BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以大端字节序读取32位有符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static int ReadIntBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.IntSize);
        var value = BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以大端字节序读取32位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static uint ReadUIntBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UIntSize);
        var value = BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以大端字节序读取64位有符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static long ReadLongBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.LongSize);
        var value = BinaryPrimitives.ReadInt64BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以大端字节序读取64位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static ulong ReadULongBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ULongSize);
        var value = BinaryPrimitives.ReadUInt64BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节缓冲区中以大端字节序读取单精度浮点数，并更新偏移量。
    /// </summary>
    /// <param name="buffer">包含了要读取数据的字节缓冲区。</param>
    /// <param name="offset">读取数据的起始位置，该方法会更新该值。</param>
    /// <returns>从字节缓冲区中读取的单精度浮点数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static float ReadFloatBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.FloatSize);
        var value = BinaryPrimitives.ReadSingleBigEndian(buffer.AsSpan(offset));
        offset += ConstBaseTypeSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 从指定偏移量以大端字节序读取双精度浮点数。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加双精度浮点数的字节数。</param>
    /// <returns>返回从缓冲区读取的双精度浮点数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static double ReadDoubleBigEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.DoubleSize);
        var value = BinaryPrimitives.ReadDoubleBigEndian(buffer.AsSpan(offset));
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }

    /// <summary>
    /// 从指定偏移量开始读取指定长度的字节数组。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量。</param>
    /// <param name="len">需要读取的字节数组长度。</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static byte[] ReadBytesBigEndianValue(this byte[] buffer, int offset, int len)
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
    /// 从指定偏移量开始读取指定长度的字节数组。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量。</param>
    /// <param name="len">需要读取的字节数组长度。</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static byte[] ReadBytesBigEndianValue(this byte[] buffer, ref int offset, int len)
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
    /// 从指定偏移量开始读取字节数组，长度作为 int 类型数据在字节数组的开头（大端字节序）。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加读取的字节长度以及 int 类型长度。</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static byte[] ReadBytesBigEndianValue(this byte[] buffer, ref int offset)
    {
        var len = ReadIntBigEndianValue(buffer, ref offset);

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
    /// 从字节数组中以指定偏移量读取字符串（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的字符串，若读取长度小于等于0或偏移量超出数组长度，返回空字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static string ReadStringBigEndianValue(this byte[] buffer, ref int offset)
    {
        var len = ReadShortBigEndianValue(buffer, ref offset);

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
