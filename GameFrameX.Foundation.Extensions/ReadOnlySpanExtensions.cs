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

using System.Buffers.Binary;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供对 ReadOnlySpan&lt;byte&gt; 的扩展方法，用于读取各种数据类型。
/// </summary>
/// <remarks>
/// Provides extension methods for ReadOnlySpan&lt;byte&gt; to read various data types.
/// </remarks>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号整型（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an unsigned integer from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的无符号整型 / The read unsigned integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static uint ReadUIntBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取整型（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an integer from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的整型 / The read integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static int ReadIntBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取无符号长整型（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an unsigned long integer from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的无符号长整型 / The read unsigned long integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static ulong ReadULongBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取长整型（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a long integer from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的长整型 / The read long integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static long ReadLongBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取无符号短整型（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an unsigned short integer from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的无符号短整型 / The read unsigned short integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static ushort ReadUShortBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取短整型（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a short integer from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的短整型 / The read short integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static short ReadShortBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取单精度浮点数（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a single-precision floating-point number from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的单精度浮点数 / The read single-precision floating-point number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static float ReadFloatBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取双精度浮点数（大端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a double-precision floating-point number from the byte array at the specified offset in big-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的双精度浮点数 / The read double-precision floating-point number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static double ReadDoubleBigEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
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
    /// 从字节数组中以指定偏移量读取无符号整型（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an unsigned integer from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的无符号整型 / The read unsigned integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static uint ReadUIntLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UIntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadUInt32LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UIntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取整型（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an integer from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的整型 / The read integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static int ReadIntLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.IntSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadInt32LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.IntSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号长整型（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an unsigned long integer from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的无符号长整型 / The read unsigned long integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static ulong ReadULongLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ULongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadUInt64LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ULongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取长整型（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a long integer from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的长整型 / The read long integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static long ReadLongLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.LongSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadInt64LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.LongSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取无符号短整型（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads an unsigned short integer from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的无符号短整型 / The read unsigned short integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static ushort ReadUShortLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.UShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadUInt16LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.UShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取短整型（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a short integer from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的短整型 / The read short integer.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static short ReadShortLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ShortSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadInt16LittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.ShortSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取单精度浮点数（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a single-precision floating-point number from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的单精度浮点数 / The read single-precision floating-point number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static float ReadFloatLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.FloatSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadSingleLittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.FloatSize;
        return value;
    }

    /// <summary>
    /// 从字节数组中以指定偏移量读取双精度浮点数（小端字节序）。
    /// </summary>
    /// <remarks>
    /// Reads a double-precision floating-point number from the byte array at the specified offset in little-endian byte order.
    /// </remarks>
    /// <param name="buffer">要从中读取数据的字节数组，不能为空 / The byte array to read data from, cannot be empty.</param>
    /// <param name="offset">读取数据的起始偏移量，此偏移量在读取后会自动增加，必须为非负数 / The starting offset for reading data, automatically increments after reading, must be non-negative.</param>
    /// <returns>读取的双精度浮点数 / The read double-precision floating-point number.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当偏移量为负数或超出缓冲区有效范围时抛出 / Thrown when the offset is negative or exceeds the valid buffer range.</exception>
    public static double ReadDoubleLittleEndianValue(this ReadOnlySpan<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.DoubleSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadDoubleLittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }

    /// <summary>
    /// 按指定单元字节长度转换只读字节跨度的字节序，并返回新数组。
    /// </summary>
    /// <param name="buffer">要转换的只读字节跨度。</param>
    /// <param name="elementSize">每个数据单元的字节长度。</param>
    /// <returns>转换后的新字节数组。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="elementSize"/> 小于 1 时抛出。</exception>
    /// <exception cref="ArgumentException">当长度不能被 <paramref name="elementSize"/> 整除时抛出。</exception>
    public static byte[] ConvertEndian(this ReadOnlySpan<byte> buffer, int elementSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(elementSize, 1, nameof(elementSize));

        if (buffer.Length % elementSize != 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetCountExceedBufferLength, buffer.Length, 0, buffer.Length), nameof(buffer));
        }

        var result = buffer.ToArray();
        result.AsSpan().ConvertEndianInPlace(elementSize);
        return result;
    }

    /// <summary>
    /// 按 2 字节单元转换只读字节跨度的字节序，并返回新数组。
    /// </summary>
    /// <param name="buffer">要转换的只读字节跨度。</param>
    /// <returns>转换后的新字节数组。</returns>
    public static byte[] ConvertEndianByInt16(this ReadOnlySpan<byte> buffer)
    {
        return buffer.ConvertEndian(ConstBaseTypeSize.ShortSize);
    }

    /// <summary>
    /// 按 4 字节单元转换只读字节跨度的字节序，并返回新数组。
    /// </summary>
    /// <param name="buffer">要转换的只读字节跨度。</param>
    /// <returns>转换后的新字节数组。</returns>
    public static byte[] ConvertEndianByInt32(this ReadOnlySpan<byte> buffer)
    {
        return buffer.ConvertEndian(ConstBaseTypeSize.IntSize);
    }

    /// <summary>
    /// 按 8 字节单元转换只读字节跨度的字节序，并返回新数组。
    /// </summary>
    /// <param name="buffer">要转换的只读字节跨度。</param>
    /// <returns>转换后的新字节数组。</returns>
    public static byte[] ConvertEndianByInt64(this ReadOnlySpan<byte> buffer)
    {
        return buffer.ConvertEndian(ConstBaseTypeSize.LongSize);
    }

    /// <summary>
    /// 将大端字节序数据转换为小端字节序数据，并返回新数组。
    /// </summary>
    /// <param name="buffer">要转换的只读字节跨度。</param>
    /// <param name="elementSize">每个数据单元的字节长度。</param>
    /// <returns>转换后的小端字节序数组。</returns>
    public static byte[] BigEndianToLittleEndian(this ReadOnlySpan<byte> buffer, int elementSize)
    {
        return buffer.ConvertEndian(elementSize);
    }

    /// <summary>
    /// 将小端字节序数据转换为大端字节序数据，并返回新数组。
    /// </summary>
    /// <param name="buffer">要转换的只读字节跨度。</param>
    /// <param name="elementSize">每个数据单元的字节长度。</param>
    /// <returns>转换后的大端字节序数组。</returns>
    public static byte[] LittleEndianToBigEndian(this ReadOnlySpan<byte> buffer, int elementSize)
    {
        return buffer.ConvertEndian(elementSize);
    }
}
