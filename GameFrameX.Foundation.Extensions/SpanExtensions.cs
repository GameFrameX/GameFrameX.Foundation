// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Buffers.Binary;
using System.Net;
using System.Text;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供对Span&lt;byte&gt;的扩展方法，用于高效地读写基本数据类型。
/// </summary>
public static class SpanExtensions
{
    #region WriteSpan

    /// <summary>
    /// 将一个32位无符号整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static void WriteUIntValue(this Span<byte> buffer, uint value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.UIntSize}, Available: {buffer.Length}");
        }

        BinaryPrimitives.WriteUInt32BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.UIntSize;
    }

    /// <summary>
    /// 将一个16位无符号整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static void WriteUShortValue(this Span<byte> buffer, ushort value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.UShortSize}, Available: {buffer.Length}");
        }

        BinaryPrimitives.WriteUInt16BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.UShortSize;
    }

    /// <summary>
    /// 将一个16位有符号整数写入指定的缓冲区，并更新偏移量。
    /// </summary>
    /// <param name="buffer">要写入的缓冲区。</param>
    /// <param name="value">要写入的值。</param>
    /// <param name="offset">要写入值的缓冲区中的偏移量。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static void WriteShortValue(this Span<byte> buffer, short value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.ShortSize}, Available: {buffer.Length}");
        }

        BinaryPrimitives.WriteInt16BigEndian(buffer[offset..], value);
        offset += ConstBaseTypeSize.ShortSize;
    }

    /// <summary>
    /// 将整数值写入到指定的字节跨度中。如果指定的偏移量加上整数大小超过了字节跨度的长度，则抛出异常。
    /// 以网络字节顺序存储整数值。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储整数值。</param>
    /// <param name="value">要写入的整数值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加整数的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteIntValue(this Span<byte> buffer, int value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.IntSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(int*)(ptr + offset) = IPAddress.HostToNetworkOrder(value);
            offset += ConstBaseTypeSize.IntSize;
        }
    }

    /// <summary>
    /// 将长整数值写入到指定的字节跨度中。如果指定的偏移量加上长整数大小超过了字节跨度的长度，则抛出异常。
    /// 以网络字节顺序存储长整数值。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储长整数值。</param>
    /// <param name="value">要写入的长整数值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加长整数的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteLongValue(this Span<byte> buffer, long value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.LongSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(long*)(ptr + offset) = IPAddress.HostToNetworkOrder(value);
            offset += ConstBaseTypeSize.LongSize;
        }
    }

    /// <summary>
    /// 将单精度浮点数写入到指定的字节跨度中。如果指定的偏移量加上浮点数大小超过了字节跨度的长度，则抛出异常。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储浮点数值。</param>
    /// <param name="value">要写入的浮点数值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加浮点数的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteFloatValue(this Span<byte> buffer, float value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.FloatSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(float*)(ptr + offset) = value;
            *(int*)(ptr + offset) = IPAddress.HostToNetworkOrder(*(int*)(ptr + offset));
            offset += ConstBaseTypeSize.FloatSize;
        }
    }

    /// <summary>
    /// 将双精度浮点数写入到指定的字节跨度中。如果指定的偏移量加上浮点数大小超过了字节跨度的长度，则抛出异常。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储双精度浮点数值。</param>
    /// <param name="value">要写入的双精度浮点数值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加双精度浮点数的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteDoubleValue(this Span<byte> buffer, double value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.DoubleSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(double*)(ptr + offset) = value;
            *(long*)(ptr + offset) = IPAddress.HostToNetworkOrder(*(long*)(ptr + offset));
            offset += ConstBaseTypeSize.DoubleSize;
        }
    }

    /// <summary>
    /// 将字节值写入到指定的字节跨度中。如果指定的偏移量加上字节大小超过了字节跨度的长度，则抛出异常。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储字节值。</param>
    /// <param name="value">要写入的字节值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加字节的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteByteValue(this Span<byte> buffer, byte value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.ByteSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(ptr + offset) = value;
            offset += ConstBaseTypeSize.ByteSize;
        }
    }

    /// <summary>
    /// 将有符号字节值写入到指定的字节跨度中。如果指定的偏移量加上字节大小超过了字节跨度的长度，则抛出异常。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储有符号字节值。</param>
    /// <param name="value">要写入的有符号字节值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加字节的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteSByteValue(this Span<byte> buffer, sbyte value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.ByteSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(sbyte*)(ptr + offset) = value;
            offset += ConstBaseTypeSize.ByteSize;
        }
    }

    /// <summary>
    /// 将布尔值写入到指定的字节跨度中。如果指定的偏移量加上布尔值大小超过了字节跨度的长度，则抛出异常。
    /// </summary>
    /// <param name="buffer">字节跨度，用于存储布尔值。</param>
    /// <param name="value">要写入的布尔值。</param>
    /// <param name="offset">写入的起始偏移量，会在调用后增加布尔值的大小。</param>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteBoolValue(this Span<byte> buffer, bool value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.BoolSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {ConstBaseTypeSize.BoolSize}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer)
        {
            *(bool*)(ptr + offset) = value;
            offset += ConstBaseTypeSize.BoolSize;
        }
    }

    /// <summary>
    /// 在给定的偏移量位置，向缓冲区中写入字节序列，不包含长度信息。
    /// </summary>
    /// <param name="buffer">目标字节缓冲区。</param>
    /// <param name="value">需要写入的字节序列。</param>
    /// <param name="offset">字节写入的起始偏移量，写入后更新。</param>
    /// <exception cref="ArgumentNullException">当 value 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteBytesWithoutLength(this Span<byte> buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + value.Length > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), $"Offset is outside the bounds of the buffer. Offset: {offset}, Required: {value.Length}, Available: {buffer.Length}");
        }

        fixed (byte* ptr = buffer, valPtr = value)
        {
            Buffer.MemoryCopy(valPtr, ptr + offset, value.Length, value.Length);
            offset += value.Length;
        }
    }

    /// <summary>
    /// 在给定的偏移量位置，向缓冲区中写入字节序列，包含长度信息。
    /// </summary>
    /// <param name="buffer">目标字节缓冲区。</param>
    /// <param name="value">需要写入的字节序列。</param>
    /// <param name="offset">字节写入的起始偏移量，写入后更新。</param>
    /// <exception cref="ArgumentNullException">当 value 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static void WriteBytesValue(this Span<byte> buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        WriteIntValue(buffer, value.Length, ref offset);

        if (value.Length > 0)
        {
            WriteBytesWithoutLength(buffer, value, ref offset);
        }
    }

    /// <summary>
    /// 在给定的偏移量位置，向缓冲区中写入字符串，包含长度信息。
    /// </summary>
    /// <param name="buffer">目标字节缓冲区。</param>
    /// <param name="value">需要写入的字符串。</param>
    /// <param name="offset">字节写入的起始偏移量，写入后更新。</param>
    /// <exception cref="ArgumentNullException">当 value 为 null 时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static unsafe void WriteStringValue(this Span<byte> buffer, string value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (string.IsNullOrEmpty(value))
        {
            WriteShortValue(buffer, 0, ref offset);
            return;
        }

        var bytes = Encoding.UTF8.GetBytes(value);

        if (bytes.Length > ushort.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"String is too long. Maximum length is {ushort.MaxValue} bytes.");
        }

        WriteShortValue(buffer, (short)bytes.Length, ref offset);
        WriteBytesWithoutLength(buffer, bytes, ref offset);
    }

    #endregion


    #region ReadSpan

    /// <summary>
    /// 从指定的byte缓冲区和偏移量读取一个int值。
    /// </summary>
    /// <param name="buffer">字节缓冲区。</param>
    /// <param name="offset">开始读取的偏移量，读取后将更新此偏移量。</param>
    /// <returns>读取到的int值。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static int ReadIntValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt32BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节缓存区读取一个短整型值（16位）。
    /// </summary>
    /// <param name="buffer">字节缓冲区</param>
    /// <param name="offset">偏移量，读取结束后会更新此偏移量。</param>
    /// <returns>从字节缓存区中读取出的短整型值</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static short ReadShortValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt16BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从Span字节数组中读取32位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的Span字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的32位无符号整数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static uint ReadUIntValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt32BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }


    /// <summary>
    /// 从Span字节数组中读取64位无符号整数，并将偏移量向前移动。
    /// </summary>
    /// <param name="buffer">要读取的Span字节数组。</param>
    /// <param name="offset">引用偏移量。</param>
    /// <returns>返回读取的64位无符号整数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static ulong ReadULongValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt64BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }


    /// <summary>
    /// 从给定的字节缓存区读取一个长整型值（64位）。
    /// </summary>
    /// <param name="buffer">字节缓冲区</param>
    /// <param name="offset">偏移量，读取结束后会更新此偏移量。</param>
    /// <returns>从字节缓存区中读取出的长整型值</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static long ReadLongValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt64BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节缓存区读取一个浮点型值（32位）。
    /// </summary>
    /// <param name="buffer">字节缓冲区</param>
    /// <param name="offset">偏移量，读取结束后会更新此偏移量。</param>
    /// <returns>从字节缓存区中读取出的浮点型值</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static float ReadFloatValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadSingleBigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 使用指定的偏移量从字节跨度中读取浮点数。
    /// </summary>
    /// <param name="buffer">字节跨度。</param>
    /// <param name="offset">开始读取的偏移量，读取后会自动增加。</param>
    /// <returns>读取的浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static double ReadDoubleValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadDoubleBigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }

    /// <summary>
    /// 使用指定的偏移量从字节跨度中读取字节。
    /// </summary>
    /// <param name="buffer">字节跨度。</param>
    /// <param name="offset">开始读取的偏移量，读取后会自动增加。</param>
    /// <returns>读取的字节。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static byte ReadByteValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = buffer[offset];
        offset += ConstBaseTypeSize.ByteSize;
        return value;
    }

    /// <summary>
    /// 使用指定的偏移量从字节跨度中读取字节数组。
    /// </summary>
    /// <param name="buffer">字节跨度。</param>
    /// <param name="offset">开始读取的偏移量，读取后会增加对应的字节数组的长度。</param>
    /// <returns>读取的字节数组。如果长度小于或等于0，返回空数组。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static byte[] ReadBytesValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        var len = ReadIntValue(buffer, ref offset);

        if (len <= 0)
        {
            return Array.Empty<byte>();
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var data = buffer.Slice(offset, len).ToArray();
        offset += len;
        return data;
    }

    /// <summary>
    /// 从给定的字节跨度中读取一个有符号字节并从偏移量处开始更新偏移量。
    /// </summary>
    /// <param name="buffer">要读取的字节跨度。</param>
    /// <param name="offset">开始读取的偏移量。</param>
    /// <returns>返回读取的有符号字节。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static sbyte ReadSByteValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = (sbyte)buffer[offset];
        offset += ConstBaseTypeSize.ByteSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节跨度中读取一个字符串并从偏移量处开始更新偏移量。
    /// </summary>
    /// <param name="buffer">要读取的字节跨度。</param>
    /// <param name="offset">开始读取的偏移量。</param>
    /// <returns>返回读取的字符串。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static string ReadStringValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        var len = ReadShortValue(buffer, ref offset);

        if (len <= 0)
        {
            return string.Empty;
        }

        if (offset + len > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = Encoding.UTF8.GetString(buffer.Slice(offset, len));
        offset += len;
        return value;
    }

    /// <summary>
    /// 从给定的字节跨度中读取一个布尔值并从偏移量处开始更新偏移量。
    /// </summary>
    /// <param name="buffer">要读取的字节跨度。</param>
    /// <param name="offset">开始读取的偏移量。</param>
    /// <returns>返回读取的布尔值。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static bool ReadBoolValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.BoolSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = buffer[offset] != 0;
        offset += ConstBaseTypeSize.BoolSize;
        return value;
    }

    #endregion
}