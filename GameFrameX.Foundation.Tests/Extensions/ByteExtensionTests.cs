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
        buffer.WriteByteValue(value, ref offset);

        // Assert
        Assert.Equal(1, offset);
        Assert.Equal(0xFF, buffer[0]);
    }

    [Fact]
    public void WriteString_ShouldWriteCorrectValue()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = "Hello";
        int offset = 0;

        // Act
        buffer.WriteStringValue(value, ref offset);

        // Assert
        Assert.True(offset > 2); // Should have written length + string bytes
    }

    [Fact]
    public void WriteString_NullString_ShouldWriteEmptyString()
    {
        // Arrange
        byte[] buffer = new byte[100];
        string value = null;
        int offset = 0;

        // Act
        buffer.WriteStringValue(value, ref offset);

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
        var result = buffer.ReadByteValue(ref offset);

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
        var result = buffer.ReadBytesValue(offset, length);

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
        var result = buffer.ReadBytesValue(ref offset, length);

        // Assert
        Assert.Equal(2, offset);
        Assert.Equal(2, result.Length);
        Assert.Equal(0x12, result[0]);
        Assert.Equal(0x34, result[1]);
    }

    [Fact]
    public void ReadString_ShouldReadCorrectString()
    {
        // Arrange
        string testString = "Hello";
        byte[] buffer = new byte[100];
        int writeOffset = 0;
        buffer.WriteStringValue(testString, ref writeOffset);
        
        int readOffset = 0;

        // Act
        var result = buffer.ReadStringValue(ref readOffset);

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
        buffer.WriteBoolValue(testValue, ref writeOffset);
        
        int readOffset = 0;

        // Act
        var result = buffer.ReadBoolValue(ref readOffset);

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
        var result = buffer.ReadBytesValue(offset, length);

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
        var result = buffer.ReadBytesValue(offset, length);

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
    public void FromBase64String_ShouldReturnCorrectBytes()
    {
        // Arrange
        string base64 = "SGVsbG8="; // "Hello" in Base64
        byte[] expected = { 72, 101, 108, 108, 111 };

        // Act
        var result = ByteExtensions.FromBase64String(base64);

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
        var result = ByteExtensions.Concat(array1, array2, array3);

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
        var result = ByteExtensions.Concat(array1, array2, array3);

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
}