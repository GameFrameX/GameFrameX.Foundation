using System;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ReadOnlySpanExtensions 扩展类单元测试
/// </summary>
public class ReadOnlySpanExtensionsTests
{
    #region ReadUInt Tests

    [Fact]
    public void ReadUInt_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC };
        var offset = 0;
        const uint expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUIntValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUInt_OffsetAtBoundary_ShouldWork()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = 0;
        const uint expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUIntValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUInt_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntValue(ref offset));
    }

    [Fact]
    public void ReadUInt_OffsetTooLarge_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = 1; // Only 3 bytes remaining, need 4

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntValue(ref offset));
    }

    #endregion

    #region ReadInt Tests

    [Fact]
    public void ReadInt_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC };
        var offset = 0;
        const int expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadIntValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadInt_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadIntValue(ref offset));
    }

    #endregion

    #region ReadULong Tests

    [Fact]
    public void ReadULong_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0x11 };
        var offset = 0;
        const ulong expected = 0x123456789ABCDEF0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadULongValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadULong_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadULongValue(ref offset));
    }

    #endregion

    #region ReadLong Tests

    [Fact]
    public void ReadLong_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0x11 };
        var offset = 0;
        const long expected = 0x123456789ABCDEF0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadLongValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadLong_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadLongValue(ref offset));
    }

    #endregion

    #region ReadUShort Tests

    [Fact]
    public void ReadUShort_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = 0;
        const ushort expected = 0x1234;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUShortValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadUShort_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12 }; // Only 1 byte, need 2
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUShortValue(ref offset));
    }

    #endregion

    #region ReadShort Tests

    [Fact]
    public void ReadShort_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = 0;
        const short expected = 0x1234;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadShortValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadShort_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12 }; // Only 1 byte, need 2
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadShortValue(ref offset));
    }

    #endregion

    #region ReadFloat Tests

    [Fact]
    public void ReadFloat_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x42, 0x28, 0x00, 0x00, 0x00 }; // 42.0f in big-endian
        var offset = 0;
        const float expected = 42.0f;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadFloatValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadFloat_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x42, 0x28, 0x00 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadFloatValue(ref offset));
    }

    #endregion

    #region ReadDouble Tests

    [Fact]
    public void ReadDouble_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x40, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; // 42.0 in big-endian
        var offset = 0;
        const double expected = 42.0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadDoubleValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadDouble_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x40, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00 }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadDoubleValue(ref offset));
    }

    #endregion

    #region Negative Offset Tests

    [Fact]
    public void ReadUInt_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntValue(ref offset));
    }

    [Fact]
    public void ReadInt_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadIntValue(ref offset));
    }

    [Fact]
    public void ReadULong_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadULongValue(ref offset));
    }

    [Fact]
    public void ReadLong_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadLongValue(ref offset));
    }

    [Fact]
    public void ReadUShort_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUShortValue(ref offset));
    }

    [Fact]
    public void ReadShort_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadShortValue(ref offset));
    }

    [Fact]
    public void ReadFloat_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x42, 0x28, 0x00, 0x00 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadFloatValue(ref offset));
    }

    [Fact]
    public void ReadDouble_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x40, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadDoubleValue(ref offset));
    }

    #endregion
}