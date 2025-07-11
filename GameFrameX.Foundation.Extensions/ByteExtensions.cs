using System.Buffers.Binary;
using System.Net;
using System.Text;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字节和字节数组的扩展方法，用于各种格式的转换和读写操作。
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    /// 将字节转换为16进制字符串。
    /// </summary>
    /// <param name="b">要转换的字节。</param>
    /// <returns>16进制字符串。</returns>
    public static string ToHex(this byte b)
    {
        return b.ToString("X2");
    }

    /// <summary>
    /// 将字节数组转换为字符串，每个字节之间用空格分隔。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <returns>字符串表示形式。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static string ToArrayString(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        var stringBuilder = new StringBuilder();
        foreach (var b in bytes)
        {
            stringBuilder.Append(b + " ");
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组转换为16进制字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <returns>16进制字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static string ToHex(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        var stringBuilder = new StringBuilder();
        foreach (var b in bytes)
        {
            stringBuilder.Append(b.ToString("X2"));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组转换为指定格式的字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <param name="format">格式化字符串。</param>
    /// <returns>格式化后的字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static string ToHex(this byte[] bytes, string format)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        var stringBuilder = new StringBuilder();
        foreach (var b in bytes)
        {
            stringBuilder.Append(b.ToString(format));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组的指定范围转换为16进制字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <param name="offset">起始偏移量。</param>
    /// <param name="count">要转换的字节数。</param>
    /// <returns>16进制字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 或 <paramref name="count"/> 超出有效范围时抛出。</exception>
    public static string ToHex(this byte[] bytes, int offset, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (offset + count > bytes.Length)
        {
            throw new ArgumentException("The sum of offset and count is greater than the buffer length.", nameof(count));
        }

        var stringBuilder = new StringBuilder();
        for (var i = offset; i < offset + count; ++i)
        {
            stringBuilder.Append(bytes[i].ToString("X2"));
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 将字节数组转换为默认编码的字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <returns>字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static string ToDefaultString(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return Encoding.Default.GetString(bytes);
    }

    /// <summary>
    /// 将字节数组的指定范围转换为默认编码的字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <param name="index">起始偏移量。</param>
    /// <param name="count">要转换的字节数。</param>
    /// <returns>字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 或 <paramref name="count"/> 超出有效范围时抛出。</exception>
    public static string ToDefaultString(this byte[] bytes, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (index + count > bytes.Length)
        {
            throw new ArgumentException("The sum of index and count is greater than the buffer length.", nameof(count));
        }

        return Encoding.Default.GetString(bytes, index, count);
    }

    /// <summary>
    /// 将字节数组转换为UTF8编码的字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <returns>UTF8编码的字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static string ToUtf8String(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// 将字节数组的指定范围转换为UTF8编码的字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <param name="index">起始偏移量。</param>
    /// <param name="count">要转换的字节数。</param>
    /// <returns>UTF8编码的字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 或 <paramref name="count"/> 超出有效范围时抛出。</exception>
    public static string ToUtf8String(this byte[] bytes, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (index + count > bytes.Length)
        {
            throw new ArgumentException("The sum of index and count is greater than the buffer length.", nameof(count));
        }

        return Encoding.UTF8.GetString(bytes, index, count);
    }

    /// <summary>
    /// 将一个32位无符号整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteUInt(this byte[] buffer, uint value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.UIntSize;
            return;
        }

        BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UIntSize;
    }

    /// <summary>
    /// 将一个32位整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteInt(this byte[] buffer, int value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.IntSize;
            return;
        }

        BinaryPrimitives.WriteInt32BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.IntSize;
    }

    /// <summary>
    /// 将一个8位整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteByte(this byte[] buffer, byte value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.ByteSize;
            return;
        }

        buffer[offset] = value;
        offset += ConstBaseTypeSize.ByteSize;
    }

    /// <summary>
    /// 将一个16位整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteShort(this byte[] buffer, short value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.ShortSize;
            return;
        }

        BinaryPrimitives.WriteInt16BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ShortSize;
    }

    /// <summary>
    /// 将一个16位无符号整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteUShort(this byte[] buffer, ushort value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.UShortSize;
            return;
        }

        BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.UShortSize;
    }

    /// <summary>
    /// 将一个64位整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteLong(this byte[] buffer, long value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.LongSize;
            return;
        }

        BinaryPrimitives.WriteInt64BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.LongSize;
    }

    /// <summary>
    /// 将一个64位无符号整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteULong(this byte[] buffer, ulong value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.ULongSize;
            return;
        }

        BinaryPrimitives.WriteUInt64BigEndian(buffer.AsSpan()[offset..], value);
        offset += ConstBaseTypeSize.ULongSize;
    }

    /// <summary>
    /// 从字节数组中读取16位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的16位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static ushort ReadUShortValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组读取16位有符号整数，并将偏移量前移。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的16位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static short ReadShortValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中读取32位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static uint ReadUIntValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中读取32位有符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static int ReadIntValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中读取64位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位无符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static ulong ReadULongValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = BinaryPrimitives.ReadUInt64BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中读取64位有符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位有符号整数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static long ReadLongValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = BinaryPrimitives.ReadInt64BigEndian(buffer.AsSpan()[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    #region Write

    /// <summary>
    /// 将一个单精度浮点数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static unsafe void WriteFloat(this byte[] buffer, float value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.FloatSize;
            return;
        }

        fixed (byte* ptr = buffer)
        {
            *(float*)(ptr + offset) = value;
            *(int*)(ptr + offset) = IPAddress.HostToNetworkOrder(*(int*)(ptr + offset));
            offset += ConstBaseTypeSize.FloatSize;
        }
    }

    /// <summary>
    /// 将一个双精度浮点数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static unsafe void WriteDouble(this byte[] buffer, double value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.DoubleSize;
            return;
        }

        fixed (byte* ptr = buffer)
        {
            *(double*)(ptr + offset) = value;
            *(long*)(ptr + offset) = IPAddress.HostToNetworkOrder(*(long*)(ptr + offset));
            offset += ConstBaseTypeSize.DoubleSize;
        }
    }


    /// <summary>
    /// 将一个字节数组写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static void WriteBytes(this byte[] buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (value == null)
        {
            buffer.WriteInt(0, ref offset);
            return;
        }

        if (offset + value.Length + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            offset += value.Length + ConstBaseTypeSize.IntSize;
            return;
        }

        buffer.WriteInt(value.Length, ref offset);
        value.AsSpan().CopyTo(buffer.AsSpan(offset, value.Length));
        offset += value.Length;
    }

    /// <summary>
    /// 将一个字节数组写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static unsafe void WriteBytesWithoutLength(this byte[] buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (value == null)
        {
            buffer.WriteInt(0, ref offset);
            return;
        }

        if (offset + value.Length > buffer.Length)
        {
            throw new ArgumentException($"buffer write out of index {offset + value.Length}, {buffer.Length}");
        }

        fixed (byte* ptr = buffer, valPtr = value)
        {
            Buffer.MemoryCopy(valPtr, ptr + offset, value.Length, value.Length);
            offset += value.Length;
        }
    }

    /// <summary>
    /// 将一个字节写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static unsafe void WriteSByte(this byte[] buffer, sbyte value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.SbyteSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.SbyteSize;
            return;
        }

        fixed (byte* ptr = buffer)
        {
            *(sbyte*)(ptr + offset) = value;
            offset += ConstBaseTypeSize.SbyteSize;
        }
    }

    /// <summary>
    /// 将一个字符串写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static unsafe void WriteString(this byte[] buffer, string value, ref int offset)
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
            throw new ArgumentException($"string length exceed short.MaxValue {len}, {short.MaxValue}");
        }


        if (offset + len + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            offset += len + ConstBaseTypeSize.ShortSize;
            return;
        }

        fixed (byte* ptr = buffer)
        {
            Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, offset + ConstBaseTypeSize.ShortSize);
            WriteShort(buffer, (short)len, ref offset);
            offset += len;
        }
    }

    /// <summary>
    /// 将一个布尔值写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数时抛出。</exception>
    public static unsafe void WriteBool(this byte[] buffer, bool value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.BoolSize > buffer.Length)
        {
            offset += ConstBaseTypeSize.BoolSize;
            return;
        }

        fixed (byte* ptr = buffer)
        {
            *(bool*)(ptr + offset) = value;
            offset += ConstBaseTypeSize.BoolSize;
        }
    }

    #endregion

    #region Read

    /// <summary>
    /// 从给定的字节缓冲区中读取浮点数，并更新偏移量。
    /// </summary>
    /// <param name="buffer">包含了要读取数据的字节缓冲区。</param>
    /// <param name="offset">读取数据的起始位置，该方法会更新该值。</param>
    /// <returns>从字节缓冲区中读取的浮点数。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static unsafe float ReadFloatValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        fixed (byte* ptr = buffer)
        {
            *(int*)(ptr + offset) = IPAddress.NetworkToHostOrder(*(int*)(ptr + offset));
            var value = *(float*)(ptr + offset);
            offset += ConstBaseTypeSize.FloatSize;
            return value;
        }
    }

    /// <summary>
    /// 从指定偏移量读取 double 类型数据。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加双精度浮点数的字节数。</param>
    /// <returns>返回从缓冲区读取的 double 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static unsafe double ReadDoubleValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        fixed (byte* ptr = buffer)
        {
            *(long*)(ptr + offset) = IPAddress.NetworkToHostOrder(*(long*)(ptr + offset));
            var value = *(double*)(ptr + offset);
            offset += ConstBaseTypeSize.DoubleSize;
            return value;
        }
    }

    /// <summary>
    /// 从指定偏移量读取 byte 类型数据。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加字节的字节数。</param>
    /// <returns>返回从缓冲区读取的 byte 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static byte ReadByteValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = buffer[offset];
        offset += ConstBaseTypeSize.ByteSize;
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
    public static byte[] ReadBytesValue(this byte[] buffer, int offset, int len)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
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
    public static byte[] ReadBytesValue(this byte[] buffer, ref int offset, int len)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var data = new byte[len];
        buffer.AsSpan(offset, len).CopyTo(data);
        offset += len;
        return data;
    }

    /// <summary>
    /// 从指定偏移量开始读取指定长度的字节数组，长度作为 int 类型数据在字节数组的开头。
    /// </summary>
    /// <param name="buffer">要操作的字节缓冲区。</param>
    /// <param name="offset">操作的起始偏移量，操作完成后，会自动累加读取的字节长度以及 int 类型长度。</param>
    /// <returns>返回从缓冲区读取的 byte[] 类型数据。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static byte[] ReadBytesValue(this byte[] buffer, ref int offset)
    {
        var len = ReadIntValue(buffer, ref offset);

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var data = new byte[len];
        buffer.AsSpan(offset, len).CopyTo(data);
        offset += len;
        return data;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取有符号字节。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的有符号字节。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static sbyte ReadSByteValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.SbyteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = (sbyte)buffer[offset];
        offset += ConstBaseTypeSize.SbyteSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取字符串。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的字符串，若读取长度小于等于0或偏移量超出数组长度，返回空字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static string ReadStringValue(this byte[] buffer, ref int offset)
    {
        var len = ReadShortValue(buffer, ref offset);

        if (len <= 0)
        {
            return string.Empty;
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = Encoding.UTF8.GetString(buffer, offset, len);
        offset += len;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取布尔值。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的布尔值。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="buffer"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或读取位置超出缓冲区边界时抛出。</exception>
    public static bool ReadBoolValue(this byte[] buffer, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(buffer, nameof(buffer));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.BoolSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Buffer read out of index.");
        }

        var value = buffer[offset] != 0;
        offset += ConstBaseTypeSize.BoolSize;
        return value;
    }

    #endregion

    #region Additional Utility Methods

    /// <summary>
    /// 比较两个字节数组是否相等。
    /// </summary>
    /// <param name="bytes1">第一个字节数组。</param>
    /// <param name="bytes2">第二个字节数组。</param>
    /// <returns>如果两个字节数组相等则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes1"/> 或 <paramref name="bytes2"/> 为 null 时抛出。</exception>
    public static bool SequenceEqual(this byte[] bytes1, byte[] bytes2)
    {
        ArgumentNullException.ThrowIfNull(bytes1, nameof(bytes1));
        ArgumentNullException.ThrowIfNull(bytes2, nameof(bytes2));

        if (bytes1.Length != bytes2.Length)
        {
            return false;
        }

        for (int i = 0; i < bytes1.Length; i++)
        {
            if (bytes1[i] != bytes2[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 使用指定的值填充字节数组。
    /// </summary>
    /// <param name="bytes">要填充的字节数组。</param>
    /// <param name="value">用于填充的值。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static void Fill(this byte[] bytes, byte value)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = value;
        }
    }

    /// <summary>
    /// 使用指定的值填充字节数组的指定范围。
    /// </summary>
    /// <param name="bytes">要填充的字节数组。</param>
    /// <param name="value">用于填充的值。</param>
    /// <param name="startIndex">填充的起始索引。</param>
    /// <param name="count">要填充的字节数。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 或 <paramref name="count"/> 超出有效范围时抛出。</exception>
    public static void Fill(this byte[] bytes, byte value, int startIndex, int count)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));

        if (startIndex + count > bytes.Length)
        {
            throw new ArgumentException("The sum of startIndex and count is greater than the buffer length.", nameof(count));
        }

        for (int i = startIndex; i < startIndex + count; i++)
        {
            bytes[i] = value;
        }
    }

    /// <summary>
    /// 反转字节数组中的字节顺序。
    /// </summary>
    /// <param name="bytes">要反转的字节数组。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static void Reverse(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        Array.Reverse(bytes);
    }

    /// <summary>
    /// 反转字节数组指定范围内的字节顺序。
    /// </summary>
    /// <param name="bytes">要反转的字节数组。</param>
    /// <param name="index">反转范围的起始索引。</param>
    /// <param name="length">要反转的字节数。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="index"/> 或 <paramref name="length"/> 超出有效范围时抛出。</exception>
    public static void Reverse(this byte[] bytes, int index, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        if (index + length > bytes.Length)
        {
            throw new ArgumentException("The sum of index and length is greater than the buffer length.", nameof(length));
        }

        Array.Reverse(bytes, index, length);
    }

    /// <summary>
    /// 将字节数组转换为Base64字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <returns>Base64编码的字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    public static string ToBase64String(this byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// 将字节数组的指定范围转换为Base64字符串。
    /// </summary>
    /// <param name="bytes">要转换的字节数组。</param>
    /// <param name="offset">起始偏移量。</param>
    /// <param name="length">要转换的字节数。</param>
    /// <returns>Base64编码的字符串。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 或 <paramref name="length"/> 超出有效范围时抛出。</exception>
    public static string ToBase64String(this byte[] bytes, int offset, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        if (offset + length > bytes.Length)
        {
            throw new ArgumentException("The sum of offset and length is greater than the buffer length.", nameof(length));
        }

        return Convert.ToBase64String(bytes, offset, length);
    }

    /// <summary>
    /// 将Base64字符串转换为字节数组。
    /// </summary>
    /// <param name="base64String">Base64编码的字符串。</param>
    /// <returns>解码后的字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="base64String"/> 为 null 时抛出。</exception>
    public static byte[] FromBase64String(string base64String)
    {
        ArgumentNullException.ThrowIfNull(base64String, nameof(base64String));
        return Convert.FromBase64String(base64String);
    }

    /// <summary>
    /// 对两个字节数组执行异或操作。
    /// </summary>
    /// <param name="bytes1">第一个字节数组。</param>
    /// <param name="bytes2">第二个字节数组。</param>
    /// <returns>异或操作的结果字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes1"/> 或 <paramref name="bytes2"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentException">当两个字节数组长度不相等时抛出。</exception>
    public static byte[] Xor(this byte[] bytes1, byte[] bytes2)
    {
        ArgumentNullException.ThrowIfNull(bytes1, nameof(bytes1));
        ArgumentNullException.ThrowIfNull(bytes2, nameof(bytes2));

        if (bytes1.Length != bytes2.Length)
        {
            throw new ArgumentException("The length of bytes1 and bytes2 must be equal.");
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
    /// <param name="bytes">字节数组。</param>
    /// <param name="value">用于异或的字节值。</param>
    /// <returns>异或操作的结果字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
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
    /// <param name="bytes">要搜索的字节数组。</param>
    /// <param name="pattern">要查找的字节序列。</param>
    /// <returns>第一个匹配位置的索引，如果未找到则返回 -1。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="pattern"/> 为 null 时抛出。</exception>
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
    /// <param name="bytes">要搜索的字节数组。</param>
    /// <param name="value">要查找的字节值。</param>
    /// <returns>第一个匹配位置的索引，如果未找到则返回 -1。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
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
    /// <param name="bytes">要搜索的字节数组。</param>
    /// <param name="value">要查找的字节值。</param>
    /// <param name="startIndex">搜索的起始位置。</param>
    /// <returns>第一个匹配位置的索引，如果未找到则返回 -1。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 超出有效范围时抛出。</exception>
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
    /// <param name="bytes">要检查的字节数组。</param>
    /// <param name="prefix">前缀字节序列。</param>
    /// <returns>如果字节数组以指定序列开头则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="prefix"/> 为 null 时抛出。</exception>
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
    /// <param name="bytes">要检查的字节数组。</param>
    /// <param name="suffix">后缀字节序列。</param>
    /// <returns>如果字节数组以指定序列结尾则返回 true，否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 或 <paramref name="suffix"/> 为 null 时抛出。</exception>
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
    /// 连接多个字节数组。
    /// </summary>
    /// <param name="arrays">要连接的字节数组集合。</param>
    /// <returns>连接后的字节数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="arrays"/> 为 null 时抛出。</exception>
    public static byte[] Concat(params byte[][] arrays)
    {
        ArgumentNullException.ThrowIfNull(arrays, nameof(arrays));

        int totalLength = 0;
        foreach (var array in arrays)
        {
            if (array != null)
            {
                totalLength += array.Length;
            }
        }

        var result = new byte[totalLength];
        int offset = 0;

        foreach (var array in arrays)
        {
            if (array != null)
            {
                array.AsSpan().CopyTo(result.AsSpan(offset, array.Length));
                offset += array.Length;
            }
        }

        return result;
    }

    /// <summary>
    /// 获取字节数组的子数组。
    /// </summary>
    /// <param name="bytes">源字节数组。</param>
    /// <param name="startIndex">起始索引。</param>
    /// <returns>子数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 超出有效范围时抛出。</exception>
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
    /// <param name="bytes">源字节数组。</param>
    /// <param name="startIndex">起始索引。</param>
    /// <param name="length">子数组长度。</param>
    /// <returns>子数组。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="startIndex"/> 或 <paramref name="length"/> 超出有效范围时抛出。</exception>
    public static byte[] SubArray(this byte[] bytes, int startIndex, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        ArgumentOutOfRangeException.ThrowIfNegative(startIndex, nameof(startIndex));
        ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

        if (startIndex + length > bytes.Length)
        {
            throw new ArgumentException("The sum of startIndex and length is greater than the buffer length.", nameof(length));
        }

        var result = new byte[length];
        bytes.AsSpan(startIndex, length).CopyTo(result);
        return result;
    }

    #endregion
}