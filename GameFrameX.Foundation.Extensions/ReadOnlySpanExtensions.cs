// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using System.Buffers.Binary;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供对 ReadOnlySpan&lt;byte&gt; 的扩展方法，用于读取各种数据类型
/// </summary>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号整型（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的无符号整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static uint ReadUIntBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取整型（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static int ReadIntBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取无符号长整型（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的无符号长整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static ulong ReadULongBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取长整型（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的长整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static long ReadLongBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取无符号短整型（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的无符号短整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static ushort ReadUShortBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt16BigEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取短整型（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的短整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static short ReadShortBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取单精度浮点数（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的单精度浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static float ReadFloatBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取双精度浮点数（大端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的双精度浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static double ReadDoubleBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取无符号整型（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的无符号整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static uint ReadUIntLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt32LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取整型（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static int ReadIntLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt32LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号长整型（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的无符号长整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static ulong ReadULongLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt64LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取长整型（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的长整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static long ReadLongLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt64LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号短整型（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的无符号短整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static ushort ReadUShortLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取短整型（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的短整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static short ReadShortLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt16LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取单精度浮点数（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的单精度浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static float ReadFloatLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取双精度浮点数（小端字节序）。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。不能为空。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。必须为非负数。</param>
    /// <returns>读取的双精度浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出。</exception>
    public static double ReadDoubleLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadDoubleLittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }
}