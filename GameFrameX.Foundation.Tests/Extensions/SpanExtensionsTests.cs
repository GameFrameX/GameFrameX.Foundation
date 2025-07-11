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
        buffer.AsSpan().WriteIntValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadIntValue(ref readOffset);
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
        buffer.AsSpan().WriteUIntValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadUIntValue(ref readOffset);
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
        buffer.AsSpan().WriteShortValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        var readOffset = 0;
        var result = buffer.AsSpan().ReadShortValue(ref readOffset);
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
        buffer.AsSpan().WriteInt(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadIntValue(ref offset);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadIntValue(ref offset));
    }

    [Fact]
    public void ReadInt_OffsetAtBoundary_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var buffer = new byte[4];
        var offset = 1; // Only 3 bytes left, but need 4 for int

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadIntValue(ref offset));
    }

    [Fact]
    public void ReadUInt_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const uint expectedValue = 12345u;
        buffer.AsSpan().WriteUInt(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadUIntValue(ref offset);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadUIntValue(ref offset));
    }

    [Fact]
    public void ReadShort_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[10];
        var offset = 0;
        const short expectedValue = 12345;
        buffer.AsSpan().WriteShort(expectedValue, ref offset);
        offset = 0;

        // Act
        var result = buffer.AsSpan().ReadShortValue(ref offset);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadShortValue(ref offset));
    }

    [Fact]
    public void ReadString_ValidInput_ShouldReadCorrectly()
    {
        // Arrange
        var buffer = new byte[100];
        var offset = 0;
        const string expectedValue = "Hello, World!";
        buffer.AsSpan().WriteString(expectedValue, ref offset);
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
        buffer.AsSpan().WriteString(expectedValue, ref offset);
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
        buffer.AsSpan().WriteByte(expectedValue, ref offset);
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
        buffer.AsSpan().WriteBool(expectedValue, ref offset);
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
        buffer.AsSpan().WriteBytes(expectedValue, ref offset);
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
        buffer.AsSpan().WriteBytes(expectedValue, ref offset);
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
        buffer.AsSpan().WriteInt(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadIntValue(ref readOffset);

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
        buffer.AsSpan().WriteFloat(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadFloatValue(ref readOffset);

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
        buffer.AsSpan().WriteDouble(originalValue, ref writeOffset);
        var result = buffer.AsSpan().ReadDoubleValue(ref readOffset);

        // Assert
        Assert.Equal(originalValue, result, 10); // 10 decimal places precision
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion
}