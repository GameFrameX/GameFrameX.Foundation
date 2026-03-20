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
/// 提供 <see cref="Span{T}"/> 和 <see cref="ReadOnlySpan{T}"/> 的扩展方法，用于二进制数据的读写操作。
/// </summary>
/// <remarks>
/// Provides extension methods for <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> for binary data read/write operations.
/// <para>该类提供了以下功能 / This class provides the following features:</para>
/// <list type="bullet">
/// <item><description>基础类型（byte、sbyte、bool）的读写 / Read and write operations for basic types (byte, sbyte, bool)</description></item>
/// <item><description>大端字节序（BigEndian）的数值读写 / Big-endian numeric read/write operations</description></item>
/// <item><description>小端字节序（LittleEndian）的数值读写 / Little-endian numeric read/write operations</description></item>
/// <item><description>字节数组和字符串的读写 / Byte array and string read/write operations</description></item>
/// </list>
/// <para>所有方法都支持自动更新偏移量，简化连续读写操作 / All methods support automatic offset updates to simplify consecutive read/write operations.</para>
/// </remarks>
public static partial class SpanExtensions
{
    /// <summary>
    /// 将字节值写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a byte value to the specified byte span and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的字节值 / The byte value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteByteValue(this Span<byte> buffer, byte value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.ByteSize, buffer.Length));
        }

        buffer[offset] = value;
        offset += ConstBaseTypeSize.ByteSize;
    }

    /// <summary>
    /// 将有符号字节值写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a signed byte value to the specified byte span and updates the offset.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的有符号字节值 / The signed byte value to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteSByteValue(this Span<byte> buffer, sbyte value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.ByteSize, buffer.Length));
        }

        buffer[offset] = (byte)value;
        offset += ConstBaseTypeSize.ByteSize;
    }

    /// <summary>
    /// 将布尔值写入到指定的字节跨度中，并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Writes a boolean value to the specified byte span and updates the offset.
    /// Writes 1 for true and 0 for false.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的布尔值，true 写入 1，false 写入 0 / The boolean value to write, writes 1 for true, 0 for false.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after writing.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static void WriteBoolValue(this Span<byte> buffer, bool value, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.BoolSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, ConstBaseTypeSize.BoolSize, buffer.Length));
        }

        buffer[offset] = value ? (byte)1 : (byte)0;
        offset += ConstBaseTypeSize.BoolSize;
    }

    /// <summary>
    /// 在给定的偏移量位置，向缓冲区中写入字节序列，不包含长度信息。
    /// </summary>
    /// <remarks>
    /// Writes a byte sequence to the buffer at the given offset position without length information.
    /// </remarks>
    /// <param name="buffer">要写入的字节跨度 / The byte span to write to.</param>
    /// <param name="value">要写入的字节数组 / The byte array to write.</param>
    /// <param name="offset">读写操作的起始位置，写入后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after writing.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 null 时抛出 / Thrown when <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static unsafe void WriteBytesWithoutLength(this Span<byte> buffer, byte[] value, ref int offset)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + value.Length > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBounds, offset, value.Length, buffer.Length));
        }

        fixed (byte* ptr = buffer, valPtr = value)
        {
            Buffer.MemoryCopy(valPtr, ptr + offset, value.Length, value.Length);
            offset += value.Length;
        }
    }

    /// <summary>
    /// 从给定的字节跨度中读取一个字节并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a byte from the given byte span and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after reading.</param>
    /// <returns>返回读取的字节值 / Returns the read byte value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static byte ReadByteValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = buffer[offset];
        offset += ConstBaseTypeSize.ByteSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节跨度中读取一个有符号字节并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a signed byte from the given byte span and updates the offset.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after reading.</param>
    /// <returns>返回读取的有符号字节值 / Returns the read signed byte value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static sbyte ReadSByteValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.ByteSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = (sbyte)buffer[offset];
        offset += ConstBaseTypeSize.ByteSize;
        return value;
    }

    /// <summary>
    /// 从给定的字节跨度中读取一个布尔值并更新偏移量。
    /// </summary>
    /// <remarks>
    /// Reads a boolean value from the given byte span and updates the offset.
    /// Non-zero values are interpreted as true, zero as false.
    /// </remarks>
    /// <param name="buffer">要读取的字节跨度 / The byte span to read from.</param>
    /// <param name="offset">读写操作的起始位置，读取后会自动增加相应字节数 / The starting position for read/write, automatically increments by the corresponding number of bytes after reading.</param>
    /// <returns>返回读取的布尔值，非零值为 true，零为 false / Returns the read boolean value, non-zero values are true, zero is false.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="offset"/> 为负数或超出缓冲区边界时抛出 / Thrown when <paramref name="offset"/> is negative or exceeds buffer bounds.</exception>
    public static bool ReadBoolValue(this Span<byte> buffer, ref int offset)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset, nameof(offset));

        if (offset + ConstBaseTypeSize.BoolSize > buffer.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(offset),
                LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = buffer[offset] != 0;
        offset += ConstBaseTypeSize.BoolSize;
        return value;
    }

    /// <summary>
    /// 按指定单元字节长度就地转换字节跨度的字节序。
    /// </summary>
    /// <param name="buffer">要转换的字节跨度。</param>
    /// <param name="elementSize">每个数据单元的字节长度。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="elementSize"/> 小于 1 时抛出。</exception>
    /// <exception cref="ArgumentException">当长度不能被 <paramref name="elementSize"/> 整除时抛出。</exception>
    public static void ConvertEndianInPlace(this Span<byte> buffer, int elementSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(elementSize, 1, nameof(elementSize));

        if (buffer.Length <= 1 || elementSize == 1)
        {
            return;
        }

        if (buffer.Length % elementSize != 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetCountExceedBufferLength, buffer.Length, 0, buffer.Length), nameof(buffer));
        }

        for (var i = 0; i < buffer.Length; i += elementSize)
        {
            buffer.Slice(i, elementSize).Reverse();
        }
    }

    /// <summary>
    /// 按 2 字节单元就地转换字节跨度的字节序。
    /// </summary>
    /// <param name="buffer">要转换的字节跨度。</param>
    public static void ConvertEndianByInt16InPlace(this Span<byte> buffer)
    {
        buffer.ConvertEndianInPlace(ConstBaseTypeSize.ShortSize);
    }

    /// <summary>
    /// 按 4 字节单元就地转换字节跨度的字节序。
    /// </summary>
    /// <param name="buffer">要转换的字节跨度。</param>
    public static void ConvertEndianByInt32InPlace(this Span<byte> buffer)
    {
        buffer.ConvertEndianInPlace(ConstBaseTypeSize.IntSize);
    }

    /// <summary>
    /// 按 8 字节单元就地转换字节跨度的字节序。
    /// </summary>
    /// <param name="buffer">要转换的字节跨度。</param>
    public static void ConvertEndianByInt64InPlace(this Span<byte> buffer)
    {
        buffer.ConvertEndianInPlace(ConstBaseTypeSize.LongSize);
    }

    /// <summary>
    /// 将大端字节序数据就地转换为小端字节序数据。
    /// </summary>
    /// <param name="buffer">要转换的字节跨度。</param>
    /// <param name="elementSize">每个数据单元的字节长度。</param>
    public static void BigEndianToLittleEndianInPlace(this Span<byte> buffer, int elementSize)
    {
        buffer.ConvertEndianInPlace(elementSize);
    }

    /// <summary>
    /// 将小端字节序数据就地转换为大端字节序数据。
    /// </summary>
    /// <param name="buffer">要转换的字节跨度。</param>
    /// <param name="elementSize">每个数据单元的字节长度。</param>
    public static void LittleEndianToBigEndianInPlace(this Span<byte> buffer, int elementSize)
    {
        buffer.ConvertEndianInPlace(elementSize);
    }
}
