using System;
using System.Text;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ByteExtension 扩展类单元测试
/// </summary>
public class ByteExtensionTests
{
    #region ToHex Tests

    [Fact]
    public void ToHex_Byte_ShouldReturnCorrectHexString()
    {
        // Arrange
        byte b = 255;

        // Act
        var result = b.ToHex();

        // Assert
        Assert.Equal("FF", result);
    }

    [Fact]
    public void ToHex_ByteArray_ShouldReturnCorrectHexString()
    {
        // Arrange
        byte[] bytes = { 0x12, 0x34, 0xAB, 0xCD };

        // Act
        var result = bytes.ToHex();

        // Assert
        Assert.Equal("1234ABCD", result);
    }

    [Fact]
    public void ToHex_ByteArray_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.ToHex());
    }

    [Fact]
    public void ToHex_ByteArrayWithFormat_ShouldReturnCorrectFormattedString()
    {
        // Arrange
        byte[] bytes = { 0x12, 0x34 };
        string format = "x2";

        // Act
        var result = bytes.ToHex(format);

        // Assert
        Assert.Equal("1234", result);
    }

    [Fact]
    public void ToHex_ByteArrayWithFormat_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;
        string format = "X2";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.ToHex(format));
    }

    [Fact]
    public void ToHex_ByteArrayWithOffsetAndCount_ShouldReturnCorrectHexString()
    {
        // Arrange
        byte[] bytes = { 0x12, 0x34, 0xAB, 0xCD, 0xEF };
        int offset = 1;
        int count = 3;

        // Act
        var result = bytes.ToHex(offset, count);

        // Assert
        Assert.Equal("34ABCD", result);
    }

    [Fact]
    public void ToHex_ByteArrayWithOffsetAndCount_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;
        int offset = 0;
        int count = 1;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.ToHex(offset, count));
    }

    [Fact]
    public void ToHex_ByteArrayWithOffsetAndCount_InvalidOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] bytes = { 0x12, 0x34 };
        int offset = -1;
        int count = 1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => bytes.ToHex(offset, count));
    }

    #endregion

    #region ToArrayString Tests

    [Fact]
    public void ToArrayString_ShouldReturnCorrectString()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3 };

        // Act
        var result = bytes.ToArrayString();

        // Assert
        Assert.Equal("1 2 3 ", result);
    }

    [Fact]
    public void ToArrayString_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.ToArrayString());
    }

    #endregion

    #region String Conversion Tests

    [Fact]
    public void ToDefaultString_ShouldReturnCorrectString()
    {
        // Arrange
        byte[] bytes = Encoding.Default.GetBytes("Hello");

        // Act
        var result = bytes.ToDefaultString();

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void ToDefaultString_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.ToDefaultString());
    }

    [Fact]
    public void ToUtf8String_ShouldReturnCorrectString()
    {
        // Arrange
        byte[] bytes = Encoding.UTF8.GetBytes("Hello World");

        // Act
        var result = bytes.ToUtf8String();

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void ToUtf8String_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.ToUtf8String());
    }

    #endregion

    #region Write Tests

    [Fact]
    public void WriteUInt_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        uint value = 0x12345678;
        int offset = 0;

        // Act
        buffer.AsSpan().WriteUIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(0x56, buffer[2]);
        Assert.Equal(0x78, buffer[3]);
    }

    [Fact]
    public void WriteUInt_NullBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = null;
        uint value = 123;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteUIntBigEndianValue(value, ref offset));
    }

    [Fact]
    public void WriteInt_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        int value = 0x12345678;
        int offset = 0;

        // Act
        buffer.AsSpan().WriteIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteByte_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        byte value = 0xFF;
        int offset = 0;

        // Act
        buffer.AsSpan().WriteByteValue(value, ref offset);

        // Assert
        Assert.Equal(1, offset);
        Assert.Equal(0xFF, buffer[0]);
    }

    [Fact]
    public void WriteStringBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.WriteStringBigEndianValue(value, ref offset);

        // Assert
        Assert.True(offset > 2); // Should have written length + string bytes
    }

    [Fact]
    public void WriteStringBigEndian_NullString_ShouldWriteEmptyString()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 0;

        // Act
        buffer.WriteStringBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset); // Should write length 0
    }

    #endregion

    #region Read Tests

    [Fact]
    public void ReadUInt_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78, 0x00, 0x00 };
        int offset = 0;

        // Act
        var result = buffer.AsSpan().ReadUIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x12345678u, result);
    }

    [Fact]
    public void ReadUInt_NullBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadUIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadUInt_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34 }; // Only 2 bytes, need 4
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().ReadUIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadInt_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78, 0x00, 0x00 };
        int offset = 0;

        // Act
        var result = buffer.AsSpan().ReadIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadByte_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0xFF, 0x00 };
        int offset = 0;

        // Act
        var result = buffer.AsSpan().ReadByteValue(ref offset);

        // Assert
        Assert.Equal(1, offset);
        Assert.Equal(0xFF, result);
    }

    [Fact]
    public void ReadBytes_WithLength_ShouldReadCorrectBytes()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xCD };
        int offset = 1;
        int length = 3;

        // Act
        var result = buffer.ReadBytesBigEndianValue(offset, length);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(0x34, result[0]);
        Assert.Equal(0x56, result[1]);
        Assert.Equal(0x78, result[2]);
    }

    [Fact]
    public void ReadBytes_WithRefOffset_ShouldReadCorrectBytesAndUpdateOffset()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78 };
        int offset = 0;
        int length = 2;

        // Act
        var result = buffer.ReadBytesBigEndianValue(ref offset, length);

        // Assert
        Assert.Equal(2, offset);
        Assert.Equal(2, result.Length);
        Assert.Equal(0x12, result[0]);
        Assert.Equal(0x34, result[1]);
    }

    [Fact]
    public void ReadStringBigEndian_ShouldReadCorrectString()
    {
        // Arrange
        string testString = "Hello";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.WriteStringBigEndianValue(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadStringBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadBool_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        bool testValue = true;
        int writeOffset = 0;
        buffer.AsSpan().WriteBoolValue(testValue, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.AsSpan().ReadBoolValue(ref readOffset);

        // Assert
        Assert.Equal(testValue, result);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void WriteUInt_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2]; // Too small for uint (4 bytes)
        uint value = 123;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.AsSpan().WriteUIntBigEndianValue(value, ref offset));
    }

    [Fact]
    public void ReadBytes_ZeroLength_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34 };
        int offset = 0;
        int length = 0;

        // Act
        var result = buffer.ReadBytesBigEndianValue(offset, length);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ReadBytes_NegativeLength_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34 };
        int offset = 0;
        int length = -1;

        // Act
        var result = buffer.ReadBytesBigEndianValue(offset, length);

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region Additional Utility Methods Tests

    [Fact]
    public void SequenceEqual_EqualArrays_ShouldReturnTrue()
    {
        // Arrange
        byte[] bytes1 = { 1, 2, 3, 4 };
        byte[] bytes2 = { 1, 2, 3, 4 };

        // Act
        var result = bytes1.SequenceEqual(bytes2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SequenceEqual_DifferentArrays_ShouldReturnFalse()
    {
        // Arrange
        byte[] bytes1 = { 1, 2, 3, 4 };
        byte[] bytes2 = { 1, 2, 3, 5 };

        // Act
        var result = bytes1.SequenceEqual(bytes2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SequenceEqual_DifferentLengths_ShouldReturnFalse()
    {
        // Arrange
        byte[] bytes1 = { 1, 2, 3 };
        byte[] bytes2 = { 1, 2, 3, 4 };

        // Act
        var result = bytes1.SequenceEqual(bytes2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void SequenceEqual_NullArrays_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes1 = null;
        byte[] bytes2 = { 1, 2, 3 };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes1.SequenceEqual(bytes2));
        Assert.Throws<ArgumentNullException>(() => bytes2.SequenceEqual(null));
    }

    [Fact]
    public void Fill_WithValue_ShouldFillEntireArray()
    {
        // Arrange
        byte[] bytes = new byte[5];
        byte value = 0xFF;

        // Act
        bytes.Fill(value);

        // Assert
        Assert.All(bytes, b => Assert.Equal(value, b));
    }

    [Fact]
    public void Fill_WithRange_ShouldFillSpecifiedRange()
    {
        // Arrange
        byte[] bytes = new byte[10];
        byte value = 0xAA;
        int startIndex = 2;
        int count = 5;

        // Act
        bytes.Fill(value, startIndex, count);

        // Assert
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i >= startIndex && i < startIndex + count)
            {
                Assert.Equal(value, bytes[i]);
            }
            else
            {
                Assert.Equal(0, bytes[i]);
            }
        }
    }

    [Fact]
    public void Fill_NullArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] bytes = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bytes.Fill(0xFF));
    }

    [Fact]
    public void Reverse_ShouldReverseArray()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] expected = { 5, 4, 3, 2, 1 };

        // Act
        bytes.Reverse();

        // Assert
        Assert.Equal(expected, bytes);
    }

    [Fact]
    public void Reverse_WithRange_ShouldReverseSpecifiedRange()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5, 6 };
        byte[] expected = { 1, 4, 3, 2, 5, 6 };

        // Act
        bytes.Reverse(1, 3);

        // Assert
        Assert.Equal(expected, bytes);
    }

    [Fact]
    public void ToBase64String_ShouldReturnCorrectBase64()
    {
        // Arrange
        byte[] bytes = { 72, 101, 108, 108, 111 }; // "Hello" in ASCII
        string expected = Convert.ToBase64String(bytes);

        // Act
        var result = bytes.ToBase64String();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToBase64String_WithRange_ShouldReturnCorrectBase64()
    {
        // Arrange
        byte[] bytes = { 0, 72, 101, 108, 108, 111, 0 }; // "Hello" in middle
        string expected = Convert.ToBase64String(bytes, 1, 5);

        // Act
        var result = bytes.ToBase64String(1, 5);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToByteArrayFromBase64_ShouldReturnCorrectBytes()
    {
        // Arrange
        string base64 = "SGVsbG8="; // "Hello" in Base64
        byte[] expected = { 72, 101, 108, 108, 111 };

        // Act
        var result = base64.ToByteArrayFromBase64();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Xor_TwoArrays_ShouldReturnCorrectResult()
    {
        // Arrange
        byte[] bytes1 = { 0xFF, 0x00, 0xAA };
        byte[] bytes2 = { 0x0F, 0xFF, 0x55 };
        byte[] expected = { 0xF0, 0xFF, 0xFF };

        // Act
        var result = bytes1.Xor(bytes2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Xor_WithSingleByte_ShouldReturnCorrectResult()
    {
        // Arrange
        byte[] bytes = { 0xFF, 0x00, 0xAA };
        byte value = 0x55;
        byte[] expected = { 0xAA, 0x55, 0xFF };

        // Act
        var result = bytes.Xor(value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Xor_DifferentLengths_ShouldThrowArgumentException()
    {
        // Arrange
        byte[] bytes1 = { 1, 2, 3 };
        byte[] bytes2 = { 1, 2 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => bytes1.Xor(bytes2));
    }

    [Fact]
    public void IndexOf_ByteArray_ShouldReturnCorrectIndex()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5, 3, 4 };
        byte[] pattern = { 3, 4 };

        // Act
        var result = bytes.IndexOf(pattern);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void IndexOf_SingleByte_ShouldReturnCorrectIndex()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte value = 3;

        // Act
        var result = bytes.IndexOf(value);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void IndexOf_WithStartIndex_ShouldReturnCorrectIndex()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 3, 5 };
        byte value = 3;
        int startIndex = 3;

        // Act
        var result = bytes.IndexOf(value, startIndex);

        // Assert
        Assert.Equal(4, result);
    }

    [Fact]
    public void IndexOf_NotFound_ShouldReturnMinusOne()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte value = 9;

        // Act
        var result = bytes.IndexOf(value);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void StartsWith_CorrectPrefix_ShouldReturnTrue()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] prefix = { 1, 2, 3 };

        // Act
        var result = bytes.StartsWith(prefix);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void StartsWith_IncorrectPrefix_ShouldReturnFalse()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] prefix = { 1, 2, 4 };

        // Act
        var result = bytes.StartsWith(prefix);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EndsWith_CorrectSuffix_ShouldReturnTrue()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] suffix = { 3, 4, 5 };

        // Act
        var result = bytes.EndsWith(suffix);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EndsWith_IncorrectSuffix_ShouldReturnFalse()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] suffix = { 3, 4, 6 };

        // Act
        var result = bytes.EndsWith(suffix);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Concat_MultipleArrays_ShouldReturnConcatenatedArray()
    {
        // Arrange
        byte[] array1 = { 1, 2 };
        byte[] array2 = { 3, 4 };
        byte[] array3 = { 5, 6 };
        byte[] expected = { 1, 2, 3, 4, 5, 6 };

        // Act
        var result = array1.Concat(array2, array3);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Concat_WithNullArrays_ShouldIgnoreNullArrays()
    {
        // Arrange
        byte[] array1 = { 1, 2 };
        byte[] array2 = null;
        byte[] array3 = { 3, 4 };
        byte[] expected = { 1, 2, 3, 4 };

        // Act
        var result = array1.Concat(array2, array3);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubArray_WithStartIndex_ShouldReturnCorrectSubArray()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] expected = { 3, 4, 5 };

        // Act
        var result = bytes.SubArray(2);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubArray_WithStartIndexAndLength_ShouldReturnCorrectSubArray()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3, 4, 5 };
        byte[] expected = { 2, 3, 4 };

        // Act
        var result = bytes.SubArray(1, 3);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SubArray_StartIndexBeyondArray_ShouldReturnEmptyArray()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3 };

        // Act
        var result = bytes.SubArray(5);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SubArray_NegativeParameters_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] bytes = { 1, 2, 3 };

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => bytes.SubArray(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => bytes.SubArray(0, -1));
    }

    #endregion

    #region BigEndian Read/Write Tests

    [Fact]
    public void WriteShortBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        short value = 0x1234;
        int offset = 0;

        // Act
        buffer.WriteShortBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
    }

    [Fact]
    public void ReadShortBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34 };
        int offset = 0;

        // Act
        var result = buffer.ReadShortBigEndianValue(ref offset);

        // Assert
        Assert.Equal(0x1234, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void WriteUShortBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        ushort value = 0x1234;
        int offset = 0;

        // Act
        buffer.WriteUShortBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
    }

    [Fact]
    public void ReadUShortBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34 };
        int offset = 0;

        // Act
        var result = buffer.ReadUShortBigEndianValue(ref offset);

        // Assert
        Assert.Equal(0x1234, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void WriteIntBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        int value = 0x12345678;
        int offset = 0;

        // Act
        buffer.WriteIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(0x56, buffer[2]);
        Assert.Equal(0x78, buffer[3]);
    }

    [Fact]
    public void ReadIntBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78 };
        int offset = 0;

        // Act
        var result = buffer.ReadIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(0x12345678, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteUIntBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        uint value = 0x12345678;
        int offset = 0;

        // Act
        buffer.WriteUIntBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(0x56, buffer[2]);
        Assert.Equal(0x78, buffer[3]);
    }

    [Fact]
    public void ReadUIntBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78 };
        int offset = 0;

        // Act
        var result = buffer.ReadUIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(0x12345678u, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteLongBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        long value = 0x123456789ABCDEF0;
        int offset = 0;

        // Act
        buffer.WriteLongBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(0x56, buffer[2]);
        Assert.Equal(0x78, buffer[3]);
        Assert.Equal(0x9A, buffer[4]);
        Assert.Equal(0xBC, buffer[5]);
        Assert.Equal(0xDE, buffer[6]);
        Assert.Equal(0xF0, buffer[7]);
    }

    [Fact]
    public void ReadLongBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        int offset = 0;

        // Act
        var result = buffer.ReadLongBigEndianValue(ref offset);

        // Assert
        Assert.Equal(0x123456789ABCDEF0, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void WriteULongBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        ulong value = 0x123456789ABCDEF0;
        int offset = 0;

        // Act
        buffer.WriteULongBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        Assert.Equal(0x12, buffer[0]);
        Assert.Equal(0x34, buffer[1]);
        Assert.Equal(0x56, buffer[2]);
        Assert.Equal(0x78, buffer[3]);
        Assert.Equal(0x9A, buffer[4]);
        Assert.Equal(0xBC, buffer[5]);
        Assert.Equal(0xDE, buffer[6]);
        Assert.Equal(0xF0, buffer[7]);
    }

    [Fact]
    public void ReadULongBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        int offset = 0;

        // Act
        var result = buffer.ReadULongBigEndianValue(ref offset);

        // Assert
        Assert.Equal(0x123456789ABCDEF0ul, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void WriteFloatBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        float value = 123.456f;
        int offset = 0;

        // Act
        buffer.WriteFloatBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadFloatBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        float value = 123.456f;
        int writeOffset = 0;
        buffer.WriteFloatBigEndianValue(value, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadFloatBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(value, result, 3);
        Assert.Equal(4, readOffset);
    }

    [Fact]
    public void WriteDoubleBigEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        double value = 123.456789;
        int offset = 0;

        // Act
        buffer.WriteDoubleBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadDoubleBigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        double value = 123.456789;
        int writeOffset = 0;
        buffer.WriteDoubleBigEndianValue(value, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadDoubleBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(value, result, 6);
        Assert.Equal(8, readOffset);
    }

    [Fact]
    public void WriteBytesBigEndian_ShouldWriteLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03 };
        int offset = 0;

        // Act
        buffer.WriteBytesBigEndianValue(value, ref offset);

        // Assert
        Assert.Equal(7, offset); // 4 bytes length + 3 bytes data
        Assert.Equal(0, buffer[0]); // Length is 3 (0x00000003 in big endian)
        Assert.Equal(0, buffer[1]);
        Assert.Equal(0, buffer[2]);
        Assert.Equal(3, buffer[3]);
        Assert.Equal(0x01, buffer[4]);
        Assert.Equal(0x02, buffer[5]);
        Assert.Equal(0x03, buffer[6]);
    }

    [Fact]
    public void ReadBytesBigEndian_WithRefOffset_ShouldReadLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03 };
        int writeOffset = 0;
        buffer.WriteBytesBigEndianValue(value, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadBytesBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(value, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadBytesBigEndian_WithLength_ShouldReadSpecifiedBytes()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 1;
        int length = 3;

        // Act
        var result = buffer.ReadBytesBigEndianValue(offset, length);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(0x02, result[0]);
        Assert.Equal(0x03, result[1]);
        Assert.Equal(0x04, result[2]);
    }

    #endregion

    #region LittleEndian Read/Write Tests

    [Fact]
    public void WriteShortLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        short value = 0x1234;
        int offset = 0;

        // Act
        buffer.WriteShortLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        Assert.Equal(0x34, buffer[0]); // Little endian: least significant byte first
        Assert.Equal(0x12, buffer[1]);
    }

    [Fact]
    public void ReadShortLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x34, 0x12 }; // Little endian
        int offset = 0;

        // Act
        var result = buffer.ReadShortLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(0x1234, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void WriteUShortLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        ushort value = 0x1234;
        int offset = 0;

        // Act
        buffer.WriteUShortLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset);
        Assert.Equal(0x34, buffer[0]); // Little endian
        Assert.Equal(0x12, buffer[1]);
    }

    [Fact]
    public void ReadUShortLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x34, 0x12 }; // Little endian
        int offset = 0;

        // Act
        var result = buffer.ReadUShortLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(0x1234, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void WriteIntLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        int value = 0x12345678;
        int offset = 0;

        // Act
        buffer.WriteIntLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x78, buffer[0]); // Little endian
        Assert.Equal(0x56, buffer[1]);
        Assert.Equal(0x34, buffer[2]);
        Assert.Equal(0x12, buffer[3]);
    }

    [Fact]
    public void ReadIntLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x78, 0x56, 0x34, 0x12 }; // Little endian
        int offset = 0;

        // Act
        var result = buffer.ReadIntLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(0x12345678, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteUIntLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        uint value = 0x12345678;
        int offset = 0;

        // Act
        buffer.WriteUIntLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x78, buffer[0]); // Little endian
        Assert.Equal(0x56, buffer[1]);
        Assert.Equal(0x34, buffer[2]);
        Assert.Equal(0x12, buffer[3]);
    }

    [Fact]
    public void ReadUIntLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0x78, 0x56, 0x34, 0x12 }; // Little endian
        int offset = 0;

        // Act
        var result = buffer.ReadUIntLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(0x12345678u, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void WriteLongLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        long value = 0x123456789ABCDEF0;
        int offset = 0;

        // Act
        buffer.WriteLongLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        Assert.Equal(0xF0, buffer[0]); // Little endian
        Assert.Equal(0xDE, buffer[1]);
        Assert.Equal(0xBC, buffer[2]);
        Assert.Equal(0x9A, buffer[3]);
        Assert.Equal(0x78, buffer[4]);
        Assert.Equal(0x56, buffer[5]);
        Assert.Equal(0x34, buffer[6]);
        Assert.Equal(0x12, buffer[7]);
    }

    [Fact]
    public void ReadLongLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 }; // Little endian
        int offset = 0;

        // Act
        var result = buffer.ReadLongLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(0x123456789ABCDEF0, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void WriteULongLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        ulong value = 0x123456789ABCDEF0;
        int offset = 0;

        // Act
        buffer.WriteULongLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
        Assert.Equal(0xF0, buffer[0]); // Little endian
        Assert.Equal(0xDE, buffer[1]);
        Assert.Equal(0xBC, buffer[2]);
        Assert.Equal(0x9A, buffer[3]);
        Assert.Equal(0x78, buffer[4]);
        Assert.Equal(0x56, buffer[5]);
        Assert.Equal(0x34, buffer[6]);
        Assert.Equal(0x12, buffer[7]);
    }

    [Fact]
    public void ReadULongLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 }; // Little endian
        int offset = 0;

        // Act
        var result = buffer.ReadULongLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(0x123456789ABCDEF0ul, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void WriteFloatLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        float value = 123.456f;
        int offset = 0;

        // Act
        buffer.WriteFloatLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadFloatLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        float value = 123.456f;
        int writeOffset = 0;
        buffer.WriteFloatLittleEndianValue(value, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadFloatLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(value, result, 3);
        Assert.Equal(4, readOffset);
    }

    [Fact]
    public void WriteDoubleLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        double value = 123.456789;
        int offset = 0;

        // Act
        buffer.WriteDoubleLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadDoubleLittleEndian_ShouldReadCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[10];
        double value = 123.456789;
        int writeOffset = 0;
        buffer.WriteDoubleLittleEndianValue(value, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadDoubleLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(value, result, 6);
        Assert.Equal(8, readOffset);
    }

    [Fact]
    public void WriteBytesLittleEndian_ShouldWriteLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03 };
        int offset = 0;

        // Act
        buffer.WriteBytesLittleEndianValue(value, ref offset);

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

    [Fact]
    public void ReadBytesLittleEndian_WithRefOffset_ShouldReadLengthAndBytes()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03 };
        int writeOffset = 0;
        buffer.WriteBytesLittleEndianValue(value, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadBytesLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(value, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadBytesLittleEndian_WithLength_ShouldReadSpecifiedBytes()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 1;
        int length = 3;

        // Act
        var result = buffer.ReadBytesLittleEndianValue(offset, length);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(0x02, result[0]);
        Assert.Equal(0x03, result[1]);
        Assert.Equal(0x04, result[2]);
    }

    [Fact]
    public void WriteStringLittleEndian_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.WriteStringLittleEndianValue(value, ref offset);

        // Assert
        Assert.True(offset > 2); // Should have written length + string bytes
    }

    [Fact]
    public void WriteStringLittleEndian_NullString_ShouldWriteEmptyString()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 0;

        // Act
        buffer.WriteStringLittleEndianValue(value, ref offset);

        // Assert
        Assert.Equal(2, offset); // Should write length 0
    }

    [Fact]
    public void ReadStringLittleEndian_ShouldReadCorrectString()
    {
        // Arrange
        string testString = "Hello世界";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.WriteStringLittleEndianValue(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadStringLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion

    #region Round-trip Tests

    [Fact]
    public void BigEndian_RoundTrip_AllTypes_ShouldPreserveValues()
    {
        // Arrange
        byte[] buffer = new byte[100];
        int offset = 0;

        short shortVal = -12345;
        ushort ushortVal = 54321;
        int intVal = -123456789;
        uint uintVal = 2987654321;
        long longVal = -98765432101234567L;
        ulong ulongVal = 18446744073709551615;
        float floatVal = -123.456f;
        double doubleVal = 123456.789012;
        string stringVal = "Test字符串!@#$%";

        // Act - Write
        buffer.WriteShortBigEndianValue(shortVal, ref offset);
        buffer.WriteUShortBigEndianValue(ushortVal, ref offset);
        buffer.WriteIntBigEndianValue(intVal, ref offset);
        buffer.WriteUIntBigEndianValue(uintVal, ref offset);
        buffer.WriteLongBigEndianValue(longVal, ref offset);
        buffer.WriteULongBigEndianValue(ulongVal, ref offset);
        buffer.WriteFloatBigEndianValue(floatVal, ref offset);
        buffer.WriteDoubleBigEndianValue(doubleVal, ref offset);
        buffer.WriteStringBigEndianValue(stringVal, ref offset);

        // Act - Read
        int readOffset = 0;
        var readShort = buffer.ReadShortBigEndianValue(ref readOffset);
        var readUShort = buffer.ReadUShortBigEndianValue(ref readOffset);
        var readInt = buffer.ReadIntBigEndianValue(ref readOffset);
        var readUInt = buffer.ReadUIntBigEndianValue(ref readOffset);
        var readLong = buffer.ReadLongBigEndianValue(ref readOffset);
        var readULong = buffer.ReadULongBigEndianValue(ref readOffset);
        var readFloat = buffer.ReadFloatBigEndianValue(ref readOffset);
        var readDouble = buffer.ReadDoubleBigEndianValue(ref readOffset);
        var readString = buffer.ReadStringBigEndianValue(ref readOffset);

        // Assert
        Assert.Equal(shortVal, readShort);
        Assert.Equal(ushortVal, readUShort);
        Assert.Equal(intVal, readInt);
        Assert.Equal(uintVal, readUInt);
        Assert.Equal(longVal, readLong);
        Assert.Equal(ulongVal, readULong);
        Assert.Equal(floatVal, readFloat, 3);
        Assert.Equal(doubleVal, readDouble, 6);
        Assert.Equal(stringVal, readString);
        Assert.Equal(offset, readOffset);
    }

    [Fact]
    public void LittleEndian_RoundTrip_AllTypes_ShouldPreserveValues()
    {
        // Arrange
        byte[] buffer = new byte[200];
        int offset = 0;

        short shortVal = -12345;
        ushort ushortVal = 54321;
        int intVal = -123456789;
        uint uintVal = 2987654321;
        long longVal = -98765432101234567L;
        ulong ulongVal = 18446744073709551615;
        float floatVal = -123.456f;
        double doubleVal = 123456.789012;
        byte[] bytesVal = { 0xDE, 0xAD, 0xBE, 0xEF };
        string stringVal = "Test字符串!@#$%";

        // Act - Write
        buffer.WriteShortLittleEndianValue(shortVal, ref offset);
        buffer.WriteUShortLittleEndianValue(ushortVal, ref offset);
        buffer.WriteIntLittleEndianValue(intVal, ref offset);
        buffer.WriteUIntLittleEndianValue(uintVal, ref offset);
        buffer.WriteLongLittleEndianValue(longVal, ref offset);
        buffer.WriteULongLittleEndianValue(ulongVal, ref offset);
        buffer.WriteFloatLittleEndianValue(floatVal, ref offset);
        buffer.WriteDoubleLittleEndianValue(doubleVal, ref offset);
        buffer.WriteBytesLittleEndianValue(bytesVal, ref offset);
        buffer.WriteStringLittleEndianValue(stringVal, ref offset);

        // Act - Read
        int readOffset = 0;
        var readShort = buffer.ReadShortLittleEndianValue(ref readOffset);
        var readUShort = buffer.ReadUShortLittleEndianValue(ref readOffset);
        var readInt = buffer.ReadIntLittleEndianValue(ref readOffset);
        var readUInt = buffer.ReadUIntLittleEndianValue(ref readOffset);
        var readLong = buffer.ReadLongLittleEndianValue(ref readOffset);
        var readULong = buffer.ReadULongLittleEndianValue(ref readOffset);
        var readFloat = buffer.ReadFloatLittleEndianValue(ref readOffset);
        var readDouble = buffer.ReadDoubleLittleEndianValue(ref readOffset);
        var readBytes = buffer.ReadBytesLittleEndianValue(ref readOffset);
        var readString = buffer.ReadStringLittleEndianValue(ref readOffset);

        // Assert
        Assert.Equal(shortVal, readShort);
        Assert.Equal(ushortVal, readUShort);
        Assert.Equal(intVal, readInt);
        Assert.Equal(uintVal, readUInt);
        Assert.Equal(longVal, readLong);
        Assert.Equal(ulongVal, readULong);
        Assert.Equal(floatVal, readFloat, 3);
        Assert.Equal(doubleVal, readDouble, 6);
        Assert.Equal(bytesVal, readBytes);
        Assert.Equal(stringVal, readString);
        Assert.Equal(offset, readOffset);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public void WriteBigEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteIntBigEndianValue(123, ref offset));
    }

    [Fact]
    public void WriteLittleEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteIntLittleEndianValue(123, ref offset));
    }

    [Fact]
    public void WriteBigEndian_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[10];
        int offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteIntBigEndianValue(123, ref offset));
    }

    [Fact]
    public void WriteLittleEndian_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[10];
        int offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteIntLittleEndianValue(123, ref offset));
    }

    [Fact]
    public void WriteBigEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteIntBigEndianValue(123, ref offset));
    }

    [Fact]
    public void WriteLittleEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteIntLittleEndianValue(123, ref offset));
    }

    [Fact]
    public void ReadBigEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.ReadIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadLittleEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.ReadIntLittleEndianValue(ref offset));
    }

    #endregion

    #region WriteBytesWithoutLength Tests

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_ValidData_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act
        buffer.WriteBytesWithoutLengthBigEndian(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x01, buffer[0]);
        Assert.Equal(0x02, buffer[1]);
        Assert.Equal(0x03, buffer[2]);
        Assert.Equal(0x04, buffer[3]);
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        byte[] value = { 0x01, 0x02 };
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteBytesWithoutLengthBigEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteBytesWithoutLengthBigEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02 };
        int offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteBytesWithoutLengthBigEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteBytesWithoutLengthBigEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthBigEndian_EmptyArray_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = Array.Empty<byte>();
        int offset = 5;

        // Act
        buffer.WriteBytesWithoutLengthBigEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_ValidData_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act
        buffer.WriteBytesWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(4, offset);
        Assert.Equal(0x01, buffer[0]);
        Assert.Equal(0x02, buffer[1]);
        Assert.Equal(0x03, buffer[2]);
        Assert.Equal(0x04, buffer[3]);
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        byte[] value = { 0x01, 0x02 };
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteBytesWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = null;
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteBytesWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = { 0x01, 0x02 };
        int offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteBytesWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = new byte[2];
        byte[] value = { 0x01, 0x02, 0x03, 0x04 };
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteBytesWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteBytesWithoutLengthLittleEndian_EmptyArray_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] value = Array.Empty<byte>();
        int offset = 5;

        // Act
        buffer.WriteBytesWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    #endregion

    #region WriteStringWithoutLength Tests

    [Fact]
    public void WriteStringWithoutLengthBigEndian_ValidString_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.WriteStringWithoutLengthBigEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset); // "Hello" is 5 bytes in UTF-8
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        string value = "Hello";
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteStringWithoutLengthBigEndian(value, ref offset));
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_NullString_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 5;

        // Act
        buffer.WriteStringWithoutLengthBigEndian(value, ref offset);

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
        buffer.WriteStringWithoutLengthBigEndian(value, ref offset);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteStringWithoutLengthBigEndian(value, ref offset));
    }

    [Fact]
    public void WriteStringWithoutLengthBigEndian_ChineseCharacters_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "你好世界";
        int offset = 0;

        // Act
        buffer.WriteStringWithoutLengthBigEndian(value, ref offset);

        // Assert
        Assert.True(offset > 0);
        // Verify round-trip
        int readOffset = 0;
        var result = buffer.ReadStringBigEndianValue(ref readOffset, offset);
        Assert.Equal(value, result);
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_ValidString_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.WriteStringWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset); // "Hello" is 5 bytes in UTF-8
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        string value = "Hello";
        int offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.WriteStringWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_NullString_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 5;

        // Act
        buffer.WriteStringWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.Equal(5, offset);
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_EmptyString_ShouldNotModifyOffset()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = string.Empty;
        int offset = 5;

        // Act
        buffer.WriteStringWithoutLengthLittleEndian(value, ref offset);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.WriteStringWithoutLengthLittleEndian(value, ref offset));
    }

    [Fact]
    public void WriteStringWithoutLengthLittleEndian_ChineseCharacters_ShouldWriteCorrectly()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "你好世界";
        int offset = 0;

        // Act
        buffer.WriteStringWithoutLengthLittleEndian(value, ref offset);

        // Assert
        Assert.True(offset > 0);
        // Verify round-trip
        int readOffset = 0;
        var result = buffer.ReadStringLittleEndianValue(ref readOffset, offset);
        Assert.Equal(value, result);
    }

    #endregion

    #region ReadStringWithLength Tests

    [Fact]
    public void ReadStringBigEndianValue_WithLength_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        string testString = "Hello";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.WriteStringWithoutLengthBigEndian(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadStringBigEndianValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadStringBigEndianValue_WithLength_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;
        int len = 5;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.ReadStringBigEndianValue(ref offset, len));
    }

    [Fact]
    public void ReadStringBigEndianValue_WithLength_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = -1;
        int len = 3;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.ReadStringBigEndianValue(ref offset, len));
    }

    [Fact]
    public void ReadStringBigEndianValue_WithLength_ZeroLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 0;
        int len = 0;

        // Act
        var result = buffer.ReadStringBigEndianValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadStringBigEndianValue_WithLength_NegativeLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 0;
        int len = -1;

        // Act
        var result = buffer.ReadStringBigEndianValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadStringBigEndianValue_WithLength_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.ReadStringBigEndianValue(ref offset, len));
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_ValidData_ShouldReadCorrectly()
    {
        // Arrange
        string testString = "Hello";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.WriteStringWithoutLengthLittleEndian(testString, ref writeOffset);

        int readOffset = 0;

        // Act
        var result = buffer.ReadStringLittleEndianValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(testString, result);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_NullBuffer_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] buffer = null;
        int offset = 0;
        int len = 5;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => buffer.ReadStringLittleEndianValue(ref offset, len));
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_NegativeOffset_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = -1;
        int len = 3;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.ReadStringLittleEndianValue(ref offset, len));
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_ZeroLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 0;
        int len = 0;

        // Act
        var result = buffer.ReadStringLittleEndianValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_NegativeLength_ShouldReturnEmptyString()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03, 0x04, 0x05 };
        int offset = 0;
        int len = -1;

        // Act
        var result = buffer.ReadStringLittleEndianValue(ref offset, len);

        // Assert
        Assert.Equal(string.Empty, result);
        Assert.Equal(0, offset);
    }

    [Fact]
    public void ReadStringLittleEndianValue_WithLength_InsufficientBuffer_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        byte[] buffer = { 0x01, 0x02, 0x03 };
        int offset = 0;
        int len = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => buffer.ReadStringLittleEndianValue(ref offset, len));
    }

    #endregion

    #region Round-trip WithoutLength Tests

    [Fact]
    public void RoundTrip_WriteBytesWithoutLengthBigEndian_ReadBytesBigEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        byte[] originalData = { 0xDE, 0xAD, 0xBE, 0xEF, 0x00, 0x11, 0x22 };
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.WriteBytesWithoutLengthBigEndian(originalData, ref writeOffset);
        var readData = buffer.ReadBytesBigEndianValue(ref readOffset, originalData.Length);

        // Assert
        Assert.Equal(originalData, readData);
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
        buffer.WriteBytesWithoutLengthLittleEndian(originalData, ref writeOffset);
        var readData = buffer.ReadBytesLittleEndianValue(ref readOffset, originalData.Length);

        // Assert
        Assert.Equal(originalData, readData);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteStringWithoutLengthBigEndian_ReadStringBigEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Hello, World! 你好世界 🌍";
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.WriteStringWithoutLengthBigEndian(originalString, ref writeOffset);
        var readString = buffer.ReadStringBigEndianValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(originalString, readString);
        Assert.Equal(writeOffset, readOffset);
    }

    [Fact]
    public void RoundTrip_WriteStringWithoutLengthLittleEndian_ReadStringLittleEndianValue_ShouldMatch()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string originalString = "Hello, World! 你好世界 🌍";
        int writeOffset = 0;
        int readOffset = 0;

        // Act
        buffer.WriteStringWithoutLengthLittleEndian(originalString, ref writeOffset);
        var readString = buffer.ReadStringLittleEndianValue(ref readOffset, writeOffset);

        // Assert
        Assert.Equal(originalString, readString);
        Assert.Equal(writeOffset, readOffset);
    }

    #endregion
}