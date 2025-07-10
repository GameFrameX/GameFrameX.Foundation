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
    /// 从字节数组中以指定偏移量读取无符号整型。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的无符号整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static uint ReadUInt(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.UIntSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt32BigEndian(buffer[offset..]);
        offset += ConstSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取整型。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static int ReadInt(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.IntSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt32BigEndian(buffer[offset..]);
        offset += ConstSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号长整型。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的无符号长整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static ulong ReadULong(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.ULongSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt64BigEndian(buffer[offset..]);
        offset += ConstSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取长整型。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的长整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static long ReadLong(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.LongSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt64BigEndian(buffer[offset..]);
        offset += ConstSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号短整型。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的无符号短整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static ushort ReadUShort(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.UShortSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadUInt16BigEndian(buffer[offset..]);
        offset += ConstSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取短整型。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的短整型。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static short ReadShort(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.ShortSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadInt16BigEndian(buffer[offset..]);
        offset += ConstSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取单精度浮点数。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的单精度浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static float ReadFloat(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.FloatSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadSingleBigEndian(buffer[offset..]);
        offset += ConstSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取双精度浮点数。
    /// </summary>
    /// <param name="buffer">要从中读取数据的字节数组。</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加。</param>
    /// <returns>读取的双精度浮点数。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量超出缓冲区有效范围时抛出。</exception>
    public static double ReadDouble(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        if (offset > buffer.Length + ConstSize.DoubleSize)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is outside the bounds of the buffer.");
        }

        var value = BinaryPrimitives.ReadDoubleBigEndian(buffer[offset..]);
        offset += ConstSize.DoubleSize;
        return value;
    }
}