using System;
using System.Text;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

public class SpanExtensionsTests
{
    #region Write Tests

    [Fact]
    public void WriteInt_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const int value = 12345;

        // Act
        buffer.AsSpan().WriteIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadIntBigEndianValue(ref readOffset);
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteUInt_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint value = 12345u;

        // Act
        buffer.AsSpan().WriteUIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadUIntBigEndianValue(ref readOffset);
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteShort_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short value = 12345;

        // Act
        buffer.AsSpan().WriteShortBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadShortBigEndianValue(ref readOffset);
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteString_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[100];
        var offset = 0;
        const string value = "Hello, World!";

        // Act
        buffer.AsSpan().WriteStringValue(value, ref offset);

        // Assert
        var readOffset = 0;
        var result = buffer.AsSpan().ReadStringValue(ref readOffset);
        Assert.Equal(value, result);
    }

    #endregion

    #region Read Tests

    [Fact]
    public void ReadInt_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const int expectedValue = 12345;
        buffer.AsSpan().WriteIntBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadInt_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[2]; // Too small for int (4 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadInt_OffsetAtBoundary_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[4];
        var offset = 1; // Only 3 bytes left, but need 4 for int

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadUInt_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint expectedValue = 12345u;
        buffer.AsSpan().WriteUIntBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadUIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUInt_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[2]; // Too small for uint (4 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadUIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadShort_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short expectedValue = 12345;
        buffer.AsSpan().WriteShortBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadShortBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadShort_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[1]; // Too small for short (2 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadShortBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadString_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[100];
        var offset = 0;
        const string expectedValue = "Hello, World!";
        buffer.AsSpan().WriteStringValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void ReadString_EmptyString_ShouldReturnEmpty()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const string expectedValue = "";
        buffer.AsSpan().WriteStringValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void ReadByte_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const byte expectedValue = 123;
        buffer.AsSpan().WriteByteValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadByteValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(1, offset);
    }

    [Fact]
    public void ReadByte_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[0]; // Empty buffer
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadByteValue(ref offset));
    }

    [Fact]
    public void ReadBool_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const bool expectedValue = true;
        buffer.AsSpan().WriteBoolValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadBoolValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(1, offset);
    }

    [Fact]
    public void ReadBytes_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[100];
        var offset = 0;
        var expectedValue = new byte[] { 1, 2, 3, 4, 5 };
        buffer.AsSpan().WriteBytesValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    [Fact]
    public void ReadBytes_EmptyArray_ShouldReturnEmpty()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        var expectedValue = Array.Empty<byte>();
        buffer.AsSpan().WriteBytesValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
    }

    #endregion

    #region Round Trip Tests

    [Fact]
    public void RoundTrip_Int_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const int originalValue = -12345;

        // Act
        buffer.AsSpan().WriteIntBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadIntBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_Float_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const float originalValue = 123.456f;

        // Act
        buffer.AsSpan().WriteFloatBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadFloatBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 6); // 6 decimal places precision
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_Double_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const double originalValue = 123.456789;

        // Act
        buffer.AsSpan().WriteDoubleBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadDoubleBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 10); // 10 decimal places precision
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region Little Endian Write Tests

    [Fact]
    public void WriteIntLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const int value = 0x12345678;

        // Act
        buffer.AsSpan().WriteIntLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        // Little endian: least significant byte first
        Assert.Equal(0x78, buffer[0]);
        Assert.Equal(0x56, buffer[1]);
        Assert.Equal(0x34, buffer[2]);
        Assert.Equal(0x12, buffer[3]);
    }

    [Fact]
    public void WriteUIntLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint value = 0x12345678u;

        // Act
        buffer.AsSpan().WriteUIntLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        // Little endian: least significant byte first
        Assert.Equal(0x78, buffer[0]);
        Assert.Equal(0x56, buffer[1]);
        Assert.Equal(0x34, buffer[2]);
        Assert.Equal(0x12, buffer[3]);
    }

    [Fact]
    public void WriteShortLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short value = 0x1234;

        // Act
        buffer.AsSpan().WriteShortLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        // Little endian: least significant byte first
        Assert.Equal(0x34, buffer[0]);
        Assert.Equal(0x12, buffer[1]);
    }

    [Fact]
    public void WriteUShortLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const ushort value = 0x1234;

        // Act
        buffer.AsSpan().WriteUShortLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        // Little endian: least significant byte first
        Assert.Equal(0x34, buffer[0]);
        Assert.Equal(0x12, buffer[1]);
    }

    [Fact]
    public void WriteLongLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const long value = 0x123456789ABCDEF0L;

        // Act
        buffer.AsSpan().WriteLongLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        // Little endian: least significant byte first
        Assert.Equal(0xF0, buffer[0]);
        Assert.Equal(0xDE, buffer[1]);
        Assert.Equal(0xBC, buffer[2]);
        Assert.Equal(0x9A, buffer[3]);
        Assert.Equal(0x78, buffer[4]);
        Assert.Equal(0x56, buffer[5]);
        Assert.Equal(0x34, buffer[6]);
        Assert.Equal(0x12, buffer[7]);
    }

    [Fact]
    public void WriteULongLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const ulong value = 0x123456789ABCDEF0UL;

        // Act
        buffer.AsSpan().WriteULongLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        // Little endian: least significant byte first
        Assert.Equal(0xF0, buffer[0]);
        Assert.Equal(0xDE, buffer[1]);
        Assert.Equal(0xBC, buffer[2]);
        Assert.Equal(0x9A, buffer[3]);
        Assert.Equal(0x78, buffer[4]);
        Assert.Equal(0x56, buffer[5]);
        Assert.Equal(0x34, buffer[6]);
        Assert.Equal(0x12, buffer[7]);
    }

    [Fact]
    public void WriteFloatLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const float value = 123.456f;

        // Act
        buffer.AsSpan().WriteFloatLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadFloatLittleEndianValue(ref readOffset);
        Assert.Equal(value, result, 6);
    }

    [Fact]
    public void WriteDoubleLittleEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const double value = 123.456789;

        // Act
        buffer.AsSpan().WriteDoubleLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadDoubleLittleEndianValue(ref readOffset);
        Assert.Equal(value, result, 10);
    }

    #endregion

    #region Little Endian Read Tests

    [Fact]
    public void ReadIntLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const int expectedValue = 0x12345678;
        buffer.AsSpan().WriteIntLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadIntLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadIntLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[2]; // Too small for int (4 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadIntLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadUIntLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint expectedValue = 0x12345678u;
        buffer.AsSpan().WriteUIntLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadUIntLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUIntLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[2]; // Too small for uint (4 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadUIntLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadShortLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short expectedValue = 0x1234;
        buffer.AsSpan().WriteShortLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadShortLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadShortLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[1]; // Too small for short (2 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadShortLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadUShortLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const ushort expectedValue = 0x1234;
        buffer.AsSpan().WriteUShortLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadUShortLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadUShortLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[1]; // Too small for ushort (2 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadUShortLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadLongLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const long expectedValue = 0x123456789ABCDEF0L;
        buffer.AsSpan().WriteLongLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadLongLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadLongLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[4]; // Too small for long (8 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadLongLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadULongLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const ulong expectedValue = 0x123456789ABCDEF0UL;
        buffer.AsSpan().WriteULongLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadULongLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadULongLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[4]; // Too small for ulong (8 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadULongLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadFloatLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const float expectedValue = 123.456f;
        buffer.AsSpan().WriteFloatLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadFloatLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result, 6);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadFloatLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[2]; // Too small for float (4 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadFloatLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadDoubleLittleEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const double expectedValue = 123.456789;
        buffer.AsSpan().WriteDoubleLittleEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadDoubleLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result, 10);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadDoubleLittleEndian_OffsetOutOfBounds_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[4]; // Too small for double (8 bytes)
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadDoubleLittleEndianValue(ref offset));
    }

    #endregion

    #region Little Endian Round Trip Tests

    [Fact]
    public void RoundTripLittleEndian_Int_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const int originalValue = -0x12345678;

        // Act
        buffer.AsSpan().WriteIntLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadIntLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_UInt_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const uint originalValue = 0x12345678u;

        // Act
        buffer.AsSpan().WriteUIntLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadUIntLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_Short_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const short originalValue = -0x1234;

        // Act
        buffer.AsSpan().WriteShortLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadShortLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_UShort_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const ushort originalValue = 0x1234;

        // Act
        buffer.AsSpan().WriteUShortLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadUShortLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_Long_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const long originalValue = -0x123456789ABCDEF0L;

        // Act
        buffer.AsSpan().WriteLongLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadLongLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_ULong_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const ulong originalValue = 0x123456789ABCDEF0UL;

        // Act
        buffer.AsSpan().WriteULongLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadULongLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_Float_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const float originalValue = -123.456f;

        // Act
        buffer.AsSpan().WriteFloatLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadFloatLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 6);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripLittleEndian_Double_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const double originalValue = -123.456789;

        // Act
        buffer.AsSpan().WriteDoubleLittleEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadDoubleLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 10);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region Big Endian Write Tests

    [Fact]
    public void WriteUIntBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint value = 0x12345678u;

        // Act
        buffer.AsSpan().WriteUIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(0x56, buffer[2]);
        Assert.Equal(0x78, buffer[3]);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteIntBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const int value = -0x12345678;

        // Act
        buffer.AsSpan().WriteIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteShortBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short value = 0x1234;

        // Act
        buffer.AsSpan().WriteShortBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void WriteUShortBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const ushort value = 0x1234;

        // Act
        buffer.AsSpan().WriteUShortBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void WriteLongBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const long value = 0x123456789ABCDEF0L;

        // Act
        buffer.AsSpan().WriteLongBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
    }

    [Fact]
    public void WriteFloatBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const float value = 123.456f;

        // Act
        buffer.AsSpan().WriteFloatBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteDoubleBigEndian_ValidInput_ShouldWriteCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const double value = 123.456789;

        // Act
        buffer.AsSpan().WriteDoubleBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
    }

    #endregion

    #region Big Endian Read Tests

    [Fact]
    public void ReadIntBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const int expectedValue = -0x12345678;
        buffer.AsSpan().WriteIntBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUIntBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint expectedValue = 0x12345678u;
        buffer.AsSpan().WriteUIntBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadUIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadShortBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short expectedValue = -0x1234;
        buffer.AsSpan().WriteShortBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadShortBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadLongBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const long expectedValue = -0x123456789ABCDEF0L;
        buffer.AsSpan().WriteLongBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadLongBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadULongBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const ulong expectedValue = 0x123456789ABCDEF0UL;
        buffer.AsSpan().WriteULongBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadULongBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadFloatBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const float expectedValue = 123.456f;
        buffer.AsSpan().WriteFloatBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadFloatBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result, 6);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadDoubleBigEndian_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const double expectedValue = 123.456789;
        buffer.AsSpan().WriteDoubleBigEndianValue(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadDoubleBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expectedValue, result, 10);
        Assert.Equal(8, offset);
    }

    #endregion

    #region Big Endian Round Trip Tests

    [Fact]
    public void RoundTripBigEndian_Int_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const int originalValue = -0x12345678;

        // Act
        buffer.AsSpan().WriteIntBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadIntBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_UInt_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const uint originalValue = 0x12345678u;

        // Act
        buffer.AsSpan().WriteUIntBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadUIntBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_Short_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const short originalValue = -0x1234;

        // Act
        buffer.AsSpan().WriteShortBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadShortBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_UShort_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const ushort originalValue = 0x1234;

        // Act
        buffer.AsSpan().WriteUShortBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadUShortBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_Long_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const long originalValue = -0x123456789ABCDEF0L;

        // Act
        buffer.AsSpan().WriteLongBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadLongBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_ULong_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const ulong originalValue = 0x123456789ABCDEF0UL;

        // Act
        buffer.AsSpan().WriteULongBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadULongBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_Float_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const float originalValue = -123.456f;

        // Act
        buffer.AsSpan().WriteFloatBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadFloatBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 6);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTripBigEndian_Double_ShouldPreserveValue()
    {
        // Arrange
        var buffer = new byte[10];
        var writeOffset = 0;
        var readOffset = 0;
        const double originalValue = -123.456789;

        // Act
        buffer.AsSpan().WriteDoubleBigEndianValue(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadDoubleBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 10);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion
}