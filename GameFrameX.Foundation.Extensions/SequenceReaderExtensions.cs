// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Buffers;
using System.Buffers.Binary;
using System.Text;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供对 <see cref="SequenceReader{T}" /> 类的扩展方法，用于从只读内存中读取数据。
/// </summary>
public static class SequenceReaderExtensions
{
    /// <summary>
    /// 从只读内存中获取一个字节数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out byte value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ByteSize || !reader.TryRead(out var num1))
        {
            return false;
        }

        value = num1;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号短整型数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out ushort value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UShortSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.UShortSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt16BigEndian(span);
        reader.Advance(ConstBaseTypeSize.UShortSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号短整型数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out short value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ShortSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.ShortSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadInt16BigEndian(span);
        reader.Advance(ConstBaseTypeSize.ShortSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号整型数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out uint value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UIntSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.UIntSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt32BigEndian(span);
        reader.Advance(ConstBaseTypeSize.UIntSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号整型数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out int value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.IntSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.IntSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadInt32BigEndian(span);
        reader.Advance(ConstBaseTypeSize.IntSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号长整型数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out ulong value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ULongSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.ULongSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt64BigEndian(span);
        reader.Advance(ConstBaseTypeSize.ULongSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号长整型数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out long value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.LongSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.LongSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadInt64BigEndian(span);
        reader.Advance(ConstBaseTypeSize.LongSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个单精度浮点数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out float value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.FloatSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.FloatSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadSingleBigEndian(span);
        reader.Advance(ConstBaseTypeSize.FloatSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个双精度浮点数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out double value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.DoubleSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.DoubleSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadDoubleBigEndian(span);
        reader.Advance(ConstBaseTypeSize.DoubleSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个布尔值数据。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBigEndianValue(this ref SequenceReader<byte> reader, out bool value)
    {
        value = false;
        if (reader.Remaining < ConstBaseTypeSize.BoolSize || !reader.TryRead(out var num1))
        {
            return false;
        }

        value = num1 != 0;
        return true;
    }

    /// <summary>
    /// 从只读内存中读取一个布尔值数据，并移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBoolValue(this ref SequenceReader<byte> reader, out bool value)
    {
        value = false;
        if (reader.Remaining < ConstBaseTypeSize.BoolSize)
        {
            return false;
        }

        if (!reader.TryRead(out var byteValue))
        {
            return false;
        }

        value = byteValue != 0;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取指定长度的字节数据，并移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="length">读取的长度。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当长度小于0时抛出。</exception>
    public static bool TryReadBytesValue(this ref SequenceReader<byte> reader, int length, out byte[] value)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than or equal to zero.");
        }

        value = new byte[length];
        if (reader.Remaining < length || !reader.TryCopyTo(value))
        {
            return false;
        }

        reader.Advance(length);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取带长度前缀的字节数据，并移动读取位置。
    /// 长度前缀使用 int 类型存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadBytesWithLengthValue(this ref SequenceReader<byte> reader, out byte[] value)
    {
        value = Array.Empty<byte>();

        if (!TryReadBigEndianValue(ref reader, out int length))
        {
            return false;
        }

        if (length <= 0)
        {
            return true;
        }

        return TryReadBytesValue(ref reader, length, out value);
    }

    /// <summary>
    /// 从只读内存中获取字符串数据，并移动读取位置。
    /// 字符串长度前缀使用 short 类型存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadStringValue(this ref SequenceReader<byte> reader, out string value)
    {
        value = string.Empty;

        if (!TryReadBigEndianValue(ref reader, out short length))
        {
            return false;
        }

        if (length <= 0)
        {
            return true;
        }

        if (!TryReadBytesValue(ref reader, length, out byte[] bytes))
        {
            return false;
        }

        value = Encoding.UTF8.GetString(bytes);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取字符串数据，并移动读取位置。
    /// 字符串长度前缀使用 short 类型存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="encoding">字符串编码。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    /// <exception cref="ArgumentNullException">当编码为 null 时抛出。</exception>
    public static bool TryReadStringValue(this ref SequenceReader<byte> reader, Encoding encoding, out string value)
    {
        if (encoding == null)
        {
            throw new ArgumentNullException(nameof(encoding));
        }

        value = string.Empty;

        if (!TryReadBigEndianValue(ref reader, out short length))
        {
            return false;
        }

        if (length <= 0)
        {
            return true;
        }

        if (!TryReadBytesValue(ref reader, length, out byte[] bytes))
        {
            return false;
        }

        value = encoding.GetString(bytes);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取字符串数据，并移动读取位置。
    /// 字符串长度前缀使用 int 类型存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadStringWithIntLengthValue(this ref SequenceReader<byte> reader, out string value)
    {
        value = string.Empty;

        if (!TryReadBigEndianValue(ref reader, out int length))
        {
            return false;
        }

        if (length <= 0)
        {
            return true;
        }

        if (!TryReadBytesValue(ref reader, length, out byte[] bytes))
        {
            return false;
        }

        value = Encoding.UTF8.GetString(bytes);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取字符串数据，并移动读取位置。
    /// 字符串长度前缀使用 int 类型存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="encoding">字符串编码。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    /// <exception cref="ArgumentNullException">当编码为 null 时抛出。</exception>
    public static bool TryReadStringWithIntLengthValue(this ref SequenceReader<byte> reader, Encoding encoding, out string value)
    {
        if (encoding == null)
        {
            throw new ArgumentNullException(nameof(encoding));
        }

        value = string.Empty;

        if (!TryReadBigEndianValue(ref reader, out int length))
        {
            return false;
        }

        if (length <= 0)
        {
            return true;
        }

        if (!TryReadBytesValue(ref reader, length, out byte[] bytes))
        {
            return false;
        }

        value = encoding.GetString(bytes);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个字节数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out byte value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ByteSize || !reader.TryPeek(0, out var num1))
        {
            return false;
        }

        value = num1;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号短整型数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out ushort value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UShortSize || !reader.TryPeek(0, out var num1) || !reader.TryPeek(1, out var num2))
        {
            return false;
        }

        value = (ushort)(num1 * 256U + num2);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号短整型数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out short value)
    {
        value = 0;
        if (!TryPeekBigEndianValue(ref reader, out ushort uValue))
        {
            return false;
        }

        value = (short)uValue;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号整型数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out uint value)
    {
        value = 0U;
        if (reader.Remaining < ConstBaseTypeSize.UIntSize)
        {
            return false;
        }

        var num1 = 0;
        var num2 = (int)System.Math.Pow(256.0, 3.0);
        for (var index = 0; index < 4; ++index)
        {
            if (!reader.TryPeek(index, out var num3))
            {
                return false;
            }

            num1 += num2 * num3;
            num2 /= 256;
        }

        value = (uint)num1;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号整型数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out int value)
    {
        value = 0;
        if (!TryPeekBigEndianValue(ref reader, out uint uValue))
        {
            return false;
        }

        value = (int)uValue;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号长整型数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out ulong value)
    {
        value = 0UL;
        if (reader.Remaining < ConstBaseTypeSize.ULongSize)
        {
            return false;
        }

        long num1 = 0;
        var num2 = (long)System.Math.Pow(256.0, 7.0);
        for (var index = 0; index < 8; ++index)
        {
            if (!reader.TryPeek(index, out var num3))
            {
                return false;
            }

            num1 += num2 * num3;
            num2 /= 256L;
        }

        value = (ulong)num1;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号长整型数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out long value)
    {
        value = 0;
        if (!TryPeekBigEndianValue(ref reader, out ulong uValue))
        {
            return false;
        }

        value = (long)uValue;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个单精度浮点数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out float value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.FloatSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.FloatSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.FloatSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadSingleBigEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个双精度浮点数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out double value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.DoubleSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.DoubleSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.DoubleSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadDoubleBigEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个布尔值数据，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBigEndianValue(this ref SequenceReader<byte> reader, out bool value)
    {
        value = false;
        if (reader.Remaining < ConstBaseTypeSize.BoolSize || !reader.TryPeek(0, out var num1))
        {
            return false;
        }

        value = num1 != 0;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取指定长度的字节数组，但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="length">要读取的字节数。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 length 小于 0 时抛出。</exception>
    public static bool TryPeekBytesValue(this ref SequenceReader<byte> reader, int length, out byte[] value)
    {
        // SequenceReader 是值类型，不需要进行 null 检查

        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be negative.");
        }

        value = null;
        if (reader.Remaining < length)
        {
            return false;
        }

        value = new byte[length];
        var sequence = reader.Sequence.Slice(reader.Position, length);
        sequence.CopyTo(value);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个字符串数据，但不移动读取位置。字符串以 short 类型长度前缀存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekStringValue(this ref SequenceReader<byte> reader, out string value)
    {
        value = null;

        // 首先读取字符串长度（short 类型）
        if (!TryPeekBigEndianValue(ref reader, out short stringLength))
        {
            return false;
        }

        // 检查长度是否合法
        if (stringLength < 0)
        {
            return false;
        }

        // 如果长度为0，返回空字符串
        if (stringLength == 0)
        {
            value = string.Empty;
            return true;
        }

        // 检查剩余数据是否足够
        if (reader.Remaining < ConstBaseTypeSize.ShortSize + stringLength)
        {
            return false;
        }

        // 读取字符串内容
        if (!TryPeekBytesValue(ref reader, ConstBaseTypeSize.ShortSize + stringLength, out byte[] bytes))
        {
            return false;
        }

        // 转换为字符串（跳过长度前缀）
        value = Encoding.UTF8.GetString(bytes, ConstBaseTypeSize.ShortSize, stringLength);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取带有长度前缀的字节数组，但不移动读取位置。字节数组以 int 类型长度前缀存储。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekBytesWithLengthValue(this ref SequenceReader<byte> reader, out byte[] value)
    {
        value = null;

        // 首先读取字节数组长度（int 类型）
        if (!TryPeekBigEndianValue(ref reader, out int bytesLength))
        {
            return false;
        }

        // 检查长度是否合法
        if (bytesLength < 0)
        {
            return false;
        }

        // 如果长度为0，返回空数组
        if (bytesLength == 0)
        {
            value = Array.Empty<byte>();
            return true;
        }

        // 检查剩余数据是否足够
        if (reader.Remaining < ConstBaseTypeSize.IntSize + bytesLength)
        {
            return false;
        }

        // 读取字节数组内容
        if (!TryPeekBytesValue(ref reader, ConstBaseTypeSize.IntSize + bytesLength, out byte[] bytes))
        {
            return false;
        }

        // 提取字节数组（跳过长度前缀）
        value = new byte[bytesLength];
        Array.Copy(bytes, ConstBaseTypeSize.IntSize, value, 0, bytesLength);
        return true;
    }

    #region 小端字节序读取方法

    /// <summary>
    /// 从只读内存中获取一个字节数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out byte value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ByteSize || !reader.TryRead(out var num1))
        {
            return false;
        }

        value = num1;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号短整型数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out ushort value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UShortSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.UShortSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt16LittleEndian(span);
        reader.Advance(ConstBaseTypeSize.UShortSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号短整型数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out short value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ShortSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.ShortSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadInt16LittleEndian(span);
        reader.Advance(ConstBaseTypeSize.ShortSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号整型数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out uint value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UIntSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.UIntSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt32LittleEndian(span);
        reader.Advance(ConstBaseTypeSize.UIntSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号整型数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out int value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.IntSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.IntSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadInt32LittleEndian(span);
        reader.Advance(ConstBaseTypeSize.IntSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号长整型数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out ulong value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ULongSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.ULongSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadUInt64LittleEndian(span);
        reader.Advance(ConstBaseTypeSize.ULongSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号长整型数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out long value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.LongSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.LongSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadInt64LittleEndian(span);
        reader.Advance(ConstBaseTypeSize.LongSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个单精度浮点数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out float value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.FloatSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.FloatSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadSingleLittleEndian(span);
        reader.Advance(ConstBaseTypeSize.FloatSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个双精度浮点数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out double value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.DoubleSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.DoubleSize];
        if (!reader.TryCopyTo(span))
        {
            return false;
        }

        value = BinaryPrimitives.ReadDoubleLittleEndian(span);
        reader.Advance(ConstBaseTypeSize.DoubleSize);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个布尔值数据（小端字节序）。
    /// </summary>
    /// <param name="reader">只读内存读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryReadLittleEndianValue(this ref SequenceReader<byte> reader, out bool value)
    {
        value = false;
        if (reader.Remaining < ConstBaseTypeSize.BoolSize || !reader.TryRead(out var num1))
        {
            return false;
        }

        value = num1 != 0;
        return true;
    }

    #endregion

    #region 小端字节序预览方法

    /// <summary>
    /// 从只读内存中获取一个字节数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out byte value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ByteSize || !reader.TryPeek(0, out var num1))
        {
            return false;
        }

        value = num1;
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号短整型数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out ushort value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UShortSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.UShortSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.UShortSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadUInt16LittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号短整型数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out short value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ShortSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.ShortSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.ShortSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadInt16LittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号整型数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out uint value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.UIntSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.UIntSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.UIntSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadUInt32LittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号整型数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out int value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.IntSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.IntSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.IntSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadInt32LittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个无符号长整型数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out ulong value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.ULongSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.ULongSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.ULongSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadUInt64LittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个有符号长整型数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out long value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.LongSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.LongSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.LongSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadInt64LittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个单精度浮点数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out float value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.FloatSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.FloatSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.FloatSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadSingleLittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个双精度浮点数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out double value)
    {
        value = 0;
        if (reader.Remaining < ConstBaseTypeSize.DoubleSize)
        {
            return false;
        }

        Span<byte> span = stackalloc byte[ConstBaseTypeSize.DoubleSize];
        var sequence = reader.Sequence.Slice(reader.Position, ConstBaseTypeSize.DoubleSize);
        sequence.CopyTo(span);

        value = BinaryPrimitives.ReadDoubleLittleEndian(span);
        return true;
    }

    /// <summary>
    /// 从只读内存中获取一个布尔值数据（小端字节序），但不移动读取位置。
    /// </summary>
    /// <param name="reader">读取器。</param>
    /// <param name="value">结果值。</param>
    /// <returns>读取成功返回 True，否则返回 False。</returns>
    /// <exception cref="ArgumentException">当读取器无效时抛出。</exception>
    public static bool TryPeekLittleEndianValue(this ref SequenceReader<byte> reader, out bool value)
    {
        value = false;
        if (reader.Remaining < ConstBaseTypeSize.BoolSize || !reader.TryPeek(0, out var num1))
        {
            return false;
        }

        value = num1 != 0;
        return true;
    }

    #endregion
}