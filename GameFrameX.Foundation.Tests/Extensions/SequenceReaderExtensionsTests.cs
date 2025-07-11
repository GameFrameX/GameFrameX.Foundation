using System;
using System.Buffers;
using System.Text;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// SequenceReaderExtensions 的单元测试。
/// </summary>
public class SequenceReaderExtensionsTests
{
    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 byte 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_Byte_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x42 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out byte value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x42, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 ushort 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_UShort_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out ushort value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x1234, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 short 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_Short_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out short value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x1234, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 uint 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_UInt_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out uint value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x12345678u, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 int 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_Int_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out int value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x12345678, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 ulong 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_ULong_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out ulong value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x123456789ABCDEF0ul, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 long 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_Long_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out long value);

        // Assert
        Assert.True(result);
        Assert.Equal(0x123456789ABCDEF0, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 float 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_Float_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedValue = 3.14159f;
        var data = new byte[4];
        System.Buffers.Binary.BinaryPrimitives.WriteSingleBigEndian(data, expectedValue);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out float value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedValue, value, 5);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 double 类型。
    /// </summary>
    [Fact]
    public void TryReadBigEndian_Double_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedValue = 3.141592653589793;
        var data = new byte[8];
        System.Buffers.Binary.BinaryPrimitives.WriteDoubleBigEndian(data, expectedValue);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out double value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedValue, value, 10);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBigEndian 方法读取 bool 类型。
    /// </summary>
    [Theory]
    [InlineData(new byte[] { 0x00 }, false)]
    [InlineData(new byte[] { 0x01 }, true)]
    [InlineData(new byte[] { 0xFF }, true)]
    public void TryReadBigEndian_Bool_ShouldReturnCorrectValue(byte[] data, bool expected)
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out bool value);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBool 方法。
    /// </summary>
    [Theory]
    [InlineData(new byte[] { 0x00 }, false)]
    [InlineData(new byte[] { 0x01 }, true)]
    [InlineData(new byte[] { 0xFF }, true)]
    public void TryReadBool_ShouldReturnCorrectValue(byte[] data, bool expected)
    {
        // Arrange
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBool(ref reader, out bool value);

        // Assert
        Assert.True(result);
        Assert.Equal(expected, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBytes 方法。
    /// </summary>
    [Fact]
    public void TryReadBytes_ShouldReturnCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBytes(ref reader, 3, out byte[] value);

        // Assert
        Assert.True(result);
        Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, value);
        Assert.Equal(2, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadBytes 方法在长度为负数时抛出异常。
    /// </summary>
    [Fact]
    public void TryReadBytes_NegativeLength_ShouldThrowException()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act & Assert
        try
        {
            GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBytes(ref reader, -1, out _);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.Equal("length", ex.ParamName);
        }
    }

    /// <summary>
    /// 测试 TryReadBytesWithLength 方法。
    /// </summary>
    [Fact]
    public void TryReadBytesWithLength_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedBytes = new byte[] { 0x01, 0x02, 0x03 };
        var data = new byte[4 + expectedBytes.Length];
        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(data, expectedBytes.Length);
        expectedBytes.CopyTo(data, 4);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBytesWithLength(ref reader, out byte[] value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedBytes, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadString 方法。
    /// </summary>
    [Fact]
    public void TryReadString_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedString = "Hello, World!";
        var stringBytes = Encoding.UTF8.GetBytes(expectedString);
        var data = new byte[2 + stringBytes.Length];
        System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(data, (short)stringBytes.Length);
        stringBytes.CopyTo(data, 2);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadString(ref reader, out string value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedString, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadString 方法使用自定义编码。
    /// </summary>
    [Fact]
    public void TryReadString_WithEncoding_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedString = "Hello, World!";
        var encoding = Encoding.ASCII;
        var stringBytes = encoding.GetBytes(expectedString);
        var data = new byte[2 + stringBytes.Length];
        System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(data, (short)stringBytes.Length);
        stringBytes.CopyTo(data, 2);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadString(ref reader, encoding, out string value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedString, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadString 方法在编码为 null 时抛出异常。
    /// </summary>
    [Fact]
    public void TryReadString_NullEncoding_ShouldThrowException()
    {
        // Arrange
        var data = new byte[] { 0x00, 0x03, 0x41, 0x42, 0x43 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act & Assert
        try
        {
            GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadString(ref reader, null, out _);
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (ArgumentNullException ex)
        {
            Assert.Equal("encoding", ex.ParamName);
        }
    }

    /// <summary>
    /// 测试 TryReadStringWithIntLength 方法。
    /// </summary>
    [Fact]
    public void TryReadStringWithIntLength_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedString = "Hello, World!";
        var stringBytes = Encoding.UTF8.GetBytes(expectedString);
        var data = new byte[4 + stringBytes.Length];
        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(data, stringBytes.Length);
        stringBytes.CopyTo(data, 4);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadStringWithIntLength(ref reader, out string value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedString, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadStringWithIntLength 方法使用自定义编码。
    /// </summary>
    [Fact]
    public void TryReadStringWithIntLength_WithEncoding_ShouldReturnCorrectValue()
    {
        // Arrange
        var expectedString = "Hello, World!";
        var encoding = Encoding.ASCII;
        var stringBytes = encoding.GetBytes(expectedString);
        var data = new byte[4 + stringBytes.Length];
        System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(data, stringBytes.Length);
        stringBytes.CopyTo(data, 4);
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act
        var result = GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadStringWithIntLength(ref reader, encoding, out string value);

        // Assert
        Assert.True(result);
        Assert.Equal(expectedString, value);
        Assert.Equal(0, reader.Remaining);
    }

    /// <summary>
    /// 测试 TryReadStringWithIntLength 方法在编码为 null 时抛出异常。
    /// </summary>
    [Fact]
    public void TryReadStringWithIntLength_NullEncoding_ShouldThrowException()
    {
        // Arrange
        var data = new byte[] { 0x00, 0x00, 0x00, 0x03, 0x41, 0x42, 0x43 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act & Assert
        try
        {
            GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadStringWithIntLength(ref reader, null, out _);
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (ArgumentNullException ex)
        {
            Assert.Equal("encoding", ex.ParamName);
        }
    }

    /// <summary>
    /// 测试数据不足时返回 false。
    /// </summary>
    [Fact]
    public void TryRead_InsufficientData_ShouldReturnFalse()
    {
        // Arrange
        var data = new byte[] { 0x01 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act & Assert
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out ushort _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out short _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out uint _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out int _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out ulong _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out long _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out float _));
        Assert.False(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out double _));
    }

    /// <summary>
    /// 测试 TryPeekBytes 方法在长度为负数时抛出异常。
    /// </summary>
    [Fact]
    public void TryPeekBytes_NegativeLength_ShouldThrowException()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);

        // Act & Assert
        try
        {
            GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryPeekBytes(ref reader, -1, out _);
            Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Assert.Equal("length", ex.ParamName);
        }
    }

    /// <summary>
    /// 测试 TryPeekBigEndian 方法读取各种类型。
    /// </summary>
    [Fact]
    public void TryPeekBigEndian_ShouldNotAdvancePosition()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var sequence = new ReadOnlySequence<byte>(data);
        var reader = new SequenceReader<byte>(sequence);
        var originalRemaining = reader.Remaining;

        // Act & Assert
        Assert.True(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out byte byteValue));
        Assert.Equal(0x12, byteValue);
        Assert.Equal(originalRemaining, reader.Remaining);

        Assert.True(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out ushort ushortValue));
        Assert.Equal(0x1234, ushortValue);
        Assert.Equal(originalRemaining, reader.Remaining);

        Assert.True(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out uint uintValue));
        Assert.Equal(0x12345678u, uintValue);
        Assert.Equal(originalRemaining, reader.Remaining);

        Assert.True(GameFrameX.Foundation.Extensions.SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out ulong ulongValue));
        Assert.Equal(0x123456789ABCDEF0ul, ulongValue);
        Assert.Equal(originalRemaining, reader.Remaining);
    }
}