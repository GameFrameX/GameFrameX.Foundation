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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
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
            throw new ArgumentOutOfRangeException(nameof(offset), LocalizationService.GetString(LocalizationKeys.Exceptions.OffsetOutsideBufferBoundsSimple));
        }

        var value = BinaryPrimitives.ReadDoubleLittleEndian(buffer[offset..]);
        offset += ConstBaseTypeSize.DoubleSize;
        return value;
    }
}