using System;
using System.Buffers.Binary;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字节数组的小端字节序（LittleEndian）读写扩展方法。
/// </summary>
public static partial class ByteExtensions
{
    #region Write LittleEndian

    /// <summary>
    /// 将一个16位有符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteShortLittleEndianValue(this byte[] buffer, short value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ShortSize);
        BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ShortSize;
    }

    /// <summary>
    /// 将一个16位无符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteUShortLittleEndianValue(this byte[] buffer, ushort value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UShortSize);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UShortSize;
    }

    /// <summary>
    /// 将一个32位有符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteIntLittleEndianValue(this byte[] buffer, int value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.IntSize);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.IntSize;
    }

    /// <summary>
    /// 将一个32位无符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteUIntLittleEndianValue(this byte[] buffer, uint value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.UIntSize);
        BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UIntSize;
    }

    /// <summary>
    /// 将一个64位有符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteLongLittleEndianValue(this byte[] buffer, long value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.LongSize);
        BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.LongSize;
    }

    /// <summary>
    /// 将一个64位无符号整数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteULongLittleEndianValue(this byte[] buffer, ulong value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.ULongSize);
        BinaryPrimitives.WriteUInt64LittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ULongSize;
    }

    /// <summary>
    /// 将一个单精度浮点数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteFloatLittleEndianValue(this byte[] buffer, float value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.FloatSize);
        BinaryPrimitives.WriteSingleLittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.FloatSize;
    }

    /// <summary>
    /// 将一个双精度浮点数以小端字节序写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或缓冲区空间不足时抛出。</exception>
    public static void WriteDoubleLittleEndianValue(this byte[] buffer, double value, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.DoubleSize);
        BinaryPrimitives.WriteDoubleLittleEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.DoubleSize;
    }

    #endregion

    #region Read LittleEndian

    /// <summary>
    /// 从字节数组中以小端字节序读取16位有符号整数，并将偏移量前移。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的16位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的16位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">包含了要读取数据的字节缓冲区。</param>
    /// <param name="offset">读取数据的起始位置，该方法会更新该值。</param>
    /// <returns>从字节缓冲区中读取的浮点数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
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
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加双精度浮点数的字节数。</param>
    /// <returns>返回从缓冲区读取的 double 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static double ReadDoubleLittleEndianValue(this byte[] buffer, ref int offset)
    {
        ValidateBounds(buffer, offset, ConstBaseTypeSize.DoubleSize);
        var value = BinaryPrimitives.ReadDoubleLittleEndian(buffer.AsSpan(offset));
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }

    #endregion
}
