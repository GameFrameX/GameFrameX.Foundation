using System;
using System.Text;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// SpanExtensions 扩展类单元测试
/// </summary>
public class SpanExtensionTests
{
    #region WriteBytesWithoutLength Tests (BigEndian)

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_ValidData_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act
        buffer.AsSpan().WriteBytesWithoutLength(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x01, buffer[0]);
        Assert.Equal(0x02, buffer[1]);
        Assert.Equal(0x03, buffer[2]);
        Assert.Equal(0x04, buffer[3]);
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.AsSpan().WriteBytesWithoutLength(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02 };
        int offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteBytesWithoutLength(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteBytesWithoutLength(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_EmptyArray_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = Array.Empty<byte>();
        int offset = 5;

        // Act
        buffer.AsSpan().WriteBytesWithoutLength(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    #endregion

    #region WriteStringWithoutLength Tests (BigEndian)

    [Fact]
    public void WriteStringWithoutLengthBigEndian_ValidString_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.AsSpan().WriteStringWithoutLength(value, ref offset);

        // Assert
        Assert.Equal(5, offset); // "Hello" is 5 bytes in UTF-8
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_NullString_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 5;

        // Act
        buffer.AsSpan().WriteStringWithoutLength(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_EmptyString_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = string.Empty;
        int offset = 5;

        // Act
        buffer.AsSpan().WriteStringWithoutLength(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        string value = "Hello";
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteStringWithoutLength(value, ref offset));
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_ChineseCharacters_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "你好世界";
        int offset = 0;

        // Act
        buffer.AsSpan().WriteStringWithoutLength(value, ref offset);

        // Assert
        Assert.True(offset > 0);
        // Verify round-trip
        int readOffset = 0;
        var result = buffer.AsSpan().ReadStringValue(ref readOffset, offset);
        Assert.Equal(value, result);
    }

    #endregion

    #region WriteBytesValue Tests (BigEndian - with length prefix)

    [Fact]
    public void WriteBytesValue_ValidData_ShouldWriteLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03 };
        int offset = 0;

        // Act
        buffer.AsSpan().WriteBytesValue(value, ref offset);

        // Assert
        Assert.Equal(7, offset); // 4 bytes length + 3 bytes data
        // Big endian: length 3 = 0x00000003
        Assert.Equal(0, buffer[0]);
        Assert.Equal(0, buffer[1]);
        Assert.Equal(0, buffer[2]);
        Assert.Equal(3, buffer[3]);
        Assert.Equal(0x01, buffer[4]);
        Assert.Equal(0x02, buffer[5]);
        Assert.Equal(0x03, buffer[6]);
    }

    [Fact]
    public void WriteBytesValue_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.AsSpan().WriteBytesValue(value, ref offset));
    }

    #endregion

    #region WriteStringValue Tests (BigEndian - with length prefix)

    [Fact]
    public void WriteStringValue_ValidString_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.AsSpan().WriteStringValue(value, ref offset);

        // Assert
        Assert.Equal(7, offset); // 2 bytes length + 5 bytes data
    }

    [Fact]
    public void WriteStringValue_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.AsSpan().WriteStringValue(value, ref offset));
    }

    [Fact]
    public void WriteStringValue_EmptyString_ShouldWriteZeroLength()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = string.Empty;
        int offset = 0;

        // Act
        buffer.AsSpan().WriteStringValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset); // 2 bytes for length (0)
    }

    #endregion

    #region ReadBytesValue Tests (BigEndian - with length prefix)

    [Fact]
    public void ReadBytesValue_ValidData_ShouldReadLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0x01, 0x02, 0x03 };
        int writeOffset = 0;
        buffer.AsSpan().WriteBytesValue(originalData, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref readOffset);

        // Assert
        Assert.Equal(originalData, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadBytesValue_ZeroLength_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.AsSpan().WriteBytesValue(Array.Empty<byte>(), ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref readOffset);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region ReadStringValue Tests (BigEndian - with length prefix)

    [Fact]
    public void ReadStringValue_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Hello世界";
        int writeOffset = 0;
        buffer.AsSpan().WriteStringValue(originalString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref readOffset);

        // Assert
        Assert.Equal(originalString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadStringValue_ZeroLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.AsSpan().WriteStringValue(string.Empty, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref readOffset);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region ReadBytesValue(ref offset, len) Tests (BigEndian)

    [Fact]
    public void ReadBytesValue_WithLength_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 1;
        int len = 3;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref offset, len);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(0x02, result[0]);
        Assert.Equal(0x03, result[1]);
        Assert.Equal(0x04, result[2]);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadBytesValue_WithLength_ZeroLength_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref offset, len);

        // Assert
        Assert.Empty(result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadBytesValue_WithLength_NegativeLength_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = -1;

        // Act
        var result = buffer.AsSpan().ReadBytesValue(ref offset, len);

        // Assert
        Assert.Empty(result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadBytesValue_WithLength_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadBytesValue(ref offset, len));
    }

    #endregion

    #region ReadStringValue(ref offset, len) Tests (BigEndian)

    [Fact]
    public void ReadStringValue_WithLength_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        string testString = "Hello";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.AsSpan().WriteStringWithoutLength(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadStringValue_WithLength_ZeroLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadStringValue_WithLength_NegativeLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = -1;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadStringValue_WithLength_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadStringValue(ref offset, len));
    }

    [Fact]
    public void ReadStringValue_WithLength_ChineseCharacters_ShouldReadCorrectly()
    {
        // Arrange
        string testString = "你好世界";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.AsSpan().WriteStringWithoutLength(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region LittleEndian WriteBytesWithoutLength Tests

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_ValidData_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act
        buffer.AsSpan().WriteBytesWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x01, buffer[0]);
        Assert.Equal(0x02, buffer[1]);
        Assert.Equal(0x03, buffer[2]);
        Assert.Equal(0x04, buffer[3]);
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.AsSpan().WriteBytesWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteBytesWithoutLengthLittleEndian(value, ref offset));
    }

    #endregion

    #region LittleEndian WriteStringWithoutLength Tests

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_ValidString_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.AsSpan().WriteStringWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset); // "Hello" is 5 bytes in UTF-8
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_NullString_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 5;

        // Act
        buffer.AsSpan().WriteStringWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        string value = "Hello";
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteStringWithoutLengthLittleEndian(value, ref offset));
    }

    #endregion

    #region LittleEndian WriteBytesValue Tests (with length prefix)

    [Fact]
    public void WriteBytesLittleEndianValue_ValidData_ShouldWriteLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03 };
        int offset = 0;

        // Act
        buffer.AsSpan().WriteBytesLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(7, offset); // 4 bytes length + 3 bytes data
        // Little endian: length 3 = 0x03000000
        Assert.Equal(3, buffer[0]);
        Assert.Equal(0, buffer[1]);
        Assert.Equal(0, buffer[2]);
        Assert.Equal(0, buffer[3]);
        Assert.Equal(0x01, buffer[4]);
        Assert.Equal(0x02, buffer[5]);
        Assert.Equal(0x03, buffer[6]);
    }

    #endregion

    #region LittleEndian WriteStringValue Tests (with length prefix)

    [Fact]
    public void WriteStringLittleEndianValue_ValidString_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.AsSpan().WriteStringLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(7, offset); // 2 bytes length + 5 bytes data
    }

    [Fact]
    public void WriteStringLittleEndianValue_NullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.AsSpan().WriteStringLittleEndianValue(value, ref offset));
    }

    #endregion

    #region LittleEndian ReadBytesValue Tests (with length prefix)

    [Fact]
    public void ReadBytesLittleEndianValue_ValidData_ShouldReadLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0x01, 0x02, 0x03 };
        int writeOffset = 0;
        buffer.AsSpan().WriteBytesLittleEndianValue(originalData, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalData, result);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region LittleEndian ReadStringValue Tests (with length prefix)

    [Fact]
    public void ReadStringLittleEndianValue_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Hello世界";
        int writeOffset = 0;
        buffer.AsSpan().WriteStringLittleEndianValue(originalString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region LittleEndian ReadBytesValue(ref offset, len) Tests

    [Fact]
    public void ReadBytesLittleEndianValue_WithLength_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 1;
        int len = 3;

        // Act
        var result = buffer.AsSpan().ReadBytesLittleEndianValue(ref offset, len);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(0x02, result[0]);
        Assert.Equal(0x03, result[1]);
        Assert.Equal(0x04, result[2]);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadBytesLittleEndianValue_WithLength_ZeroLength_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 0;

        // Act
        var result = buffer.AsSpan().ReadBytesLittleEndianValue(ref offset, len);

        // Assert
        Assert.Empty(result);
        Assert.Equal(0, offset);
    }

    #endregion

    #region LittleEndian ReadStringValue(ref offset, len) Tests

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        string testString = "Hello";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.AsSpan().WriteStringWithoutLengthLittleEndian(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadStringLittleEndianValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_ZeroLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 0;

        // Act
        var result = buffer.AsSpan().ReadStringLittleEndianValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    #endregion

    #region Round-trip Tests

    [Fact]
    public void RoundTrip_WriteBytesWithoutLength_ReadBytesValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x11, 0x22 };
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteBytesWithoutLength(originalData, ref writeOffset);
        var readData = buffer.AsSpan().ReadBytesValue(ref readOffset, originalData.Length);

        // Assert
        Assert.Equal(originalData, readData);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteStringWithoutLength_ReadStringValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Hello, World! 你好世界";
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteStringWithoutLength(originalString, ref writeOffset);
        var readString = buffer.AsSpan().ReadStringValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(originalString, readString);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteBytesWithoutLengthLittleEndian_ReadBytesLittleEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x11, 0x22 };
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteBytesWithoutLengthLittleEndian(originalData, ref writeOffset);
        var readData = buffer.AsSpan().ReadBytesLittleEndianValue(ref readOffset, originalData.Length);

        // Assert
        Assert.Equal(originalData, readData);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteStringWithoutLengthLittleEndian_ReadStringLittleEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Hello, World! 你好世界";
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteStringWithoutLengthLittleEndian(originalString, ref writeOffset);
        var readString = buffer.AsSpan().ReadStringLittleEndianValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(originalString, readString);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteBytesValue_ReadBytesValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0xDE, 0xAD, 0xBE, 0xEF };
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteBytesValue(originalData, ref writeOffset);
        var readData = buffer.AsSpan().ReadBytesValue(ref readOffset);

        // Assert
        Assert.Equal(originalData, readData);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteStringValue_ReadStringValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Test";
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteStringValue(originalString, ref writeOffset);
        var readString = buffer.AsSpan().ReadStringValue(ref readOffset);

        // Assert
        Assert.Equal(originalString, readString);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteBytesLittleEndianValue_ReadBytesLittleEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0xDE, 0xAD, 0xBE, 0xEF };
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteBytesLittleEndianValue(originalData, ref writeOffset);
        var readData = buffer.AsSpan().ReadBytesLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalData, readData);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteStringLittleEndianValue_ReadStringLittleEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Test";
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.AsSpan().WriteStringLittleEndianValue(originalString, ref writeOffset);
        var readString = buffer.AsSpan().ReadStringLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(originalString, readString);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion
}
