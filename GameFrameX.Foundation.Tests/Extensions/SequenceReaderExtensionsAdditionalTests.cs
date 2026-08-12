using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Text;
using GameFrameX.Foundation.Extensions;
using SequenceReaderExtensions = GameFrameX.Foundation.Extensions.SequenceReaderExtensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// SequenceReaderExtensions 补充覆盖测试：Peek 变体（short/int/long/float/double/bool）、
    /// PeekString/PeekBytesWithLength、零长度边界、空序列。
    /// </summary>
    public class SequenceReaderExtensionsAdditionalTests
    {
        private static SequenceReader<byte> CreateReader(byte[] data)
        {
            var sequence = new ReadOnlySequence<byte>(data);
            return new SequenceReader<byte>(sequence);
        }

        // ============================================================
        // TryPeekBigEndianValue 补充类型
        // ============================================================

        [Fact]
        public void TryPeekBigEndian_Short_ShouldReturnCorrectValue()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0xFF, 0xFE }); // -2 in signed short

            // Act
            var result = SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out short value);

            // Assert
            Assert.True(result);
            Assert.Equal(-2, value);
            Assert.Equal(2, reader.Remaining); // Peek should not advance
        }

        [Fact]
        public void TryPeekBigEndian_Int_ShouldReturnCorrectValue()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x12, 0x34, 0x56, 0x78 });

            // Act
            var result = SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out int value);

            // Assert
            Assert.True(result);
            Assert.Equal(0x12345678, value);
            Assert.Equal(4, reader.Remaining);
        }

        [Fact]
        public void TryPeekBigEndian_Long_ShouldReturnCorrectValue()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 });

            // Act
            var result = SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out long value);

            // Assert
            Assert.True(result);
            Assert.Equal(0x123456789ABCDEF0, value);
            Assert.Equal(8, reader.Remaining);
        }

        [Fact]
        public void TryPeekBigEndian_Float_ShouldReturnCorrectValue()
        {
            // Arrange
            var expected = 3.14f;
            var data = new byte[4];
            BinaryPrimitives.WriteSingleBigEndian(data, expected);
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out float value);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, value, 3);
            Assert.Equal(4, reader.Remaining);
        }

        [Fact]
        public void TryPeekBigEndian_Double_ShouldReturnCorrectValue()
        {
            // Arrange
            var expected = 3.141592653589793;
            var data = new byte[8];
            BinaryPrimitives.WriteDoubleBigEndian(data, expected);
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out double value);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, value, 10);
            Assert.Equal(8, reader.Remaining);
        }

        [Theory]
        [InlineData(new byte[] { 0x00 }, false)]
        [InlineData(new byte[] { 0x01 }, true)]
        [InlineData(new byte[] { 0xFF }, true)]
        public void TryPeekBigEndian_Bool_ShouldReturnCorrectValue(byte[] data, bool expected)
        {
            // Arrange
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out bool value);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, value);
            Assert.Equal(1, reader.Remaining);
        }

        // ============================================================
        // TryPeekBigEndian 不足数据
        // ============================================================

        [Fact]
        public void TryPeekBigEndian_InsufficientData_AllTypes_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x01 });

            // Act & Assert
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out ushort _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out short _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out uint _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out int _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out ulong _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out long _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out float _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out double _));
        }

        // ============================================================
        // TryPeekLittleEndianValue 补充类型
        // ============================================================

        [Fact]
        public void TryPeekLittleEndian_Short_ShouldReturnCorrectValue()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0xFE, 0xFF }); // little endian -2

            // Act
            var result = SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out short value);

            // Assert
            Assert.True(result);
            Assert.Equal(-2, value);
            Assert.Equal(2, reader.Remaining);
        }

        [Fact]
        public void TryPeekLittleEndian_Int_ShouldReturnCorrectValue()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x78, 0x56, 0x34, 0x12 });

            // Act
            var result = SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out int value);

            // Assert
            Assert.True(result);
            Assert.Equal(0x12345678, value);
            Assert.Equal(4, reader.Remaining);
        }

        [Fact]
        public void TryPeekLittleEndian_Long_ShouldReturnCorrectValue()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 });

            // Act
            var result = SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out long value);

            // Assert
            Assert.True(result);
            Assert.Equal(unchecked((long)0x123456789ABCDEF0ul), value);
            Assert.Equal(8, reader.Remaining);
        }

        [Fact]
        public void TryPeekLittleEndian_Float_ShouldReturnCorrectValue()
        {
            // Arrange
            var expected = 2.718f;
            var data = new byte[4];
            BinaryPrimitives.WriteSingleLittleEndian(data, expected);
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out float value);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, value, 3);
            Assert.Equal(4, reader.Remaining);
        }

        [Fact]
        public void TryPeekLittleEndian_Double_ShouldReturnCorrectValue()
        {
            // Arrange
            var expected = 2.718281828459045;
            var data = new byte[8];
            BinaryPrimitives.WriteDoubleLittleEndian(data, expected);
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out double value);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, value, 10);
            Assert.Equal(8, reader.Remaining);
        }

        [Theory]
        [InlineData(new byte[] { 0x00 }, false)]
        [InlineData(new byte[] { 0x01 }, true)]
        public void TryPeekLittleEndian_Bool_ShouldReturnCorrectValue(byte[] data, bool expected)
        {
            // Arrange
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out bool value);

            // Assert
            Assert.True(result);
            Assert.Equal(expected, value);
            Assert.Equal(1, reader.Remaining);
        }

        [Fact]
        public void TryPeekLittleEndian_InsufficientData_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x01 });

            // Act & Assert
            Assert.False(SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out short _));
            Assert.False(SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out int _));
            Assert.False(SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out long _));
            Assert.False(SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out float _));
            Assert.False(SequenceReaderExtensions.TryPeekLittleEndianValue(ref reader, out double _));
        }

        // ============================================================
        // TryPeekStringValue
        // ============================================================

        [Fact]
        public void TryPeekString_ShouldReturnCorrectValue_WithoutAdvancing()
        {
            // Arrange
            var expectedString = "Hello";
            var stringBytes = Encoding.UTF8.GetBytes(expectedString);
            var data = new byte[2 + stringBytes.Length];
            BinaryPrimitives.WriteInt16BigEndian(data, (short)stringBytes.Length);
            stringBytes.CopyTo(data, 2);
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekStringValue(ref reader, out string value);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedString, value);
            Assert.Equal(data.Length, reader.Remaining); // Peek should not advance
        }

        [Fact]
        public void TryPeekString_ZeroLength_ShouldReturnEmptyString()
        {
            // Arrange
            var data = new byte[] { 0x00, 0x00 };
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekStringValue(ref reader, out string value);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public void TryPeekString_InsufficientData_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x00 });

            // Act
            var result = SequenceReaderExtensions.TryPeekStringValue(ref reader, out string value);

            // Assert
            Assert.False(result);
            Assert.Null(value);
        }

        // ============================================================
        // TryPeekBytesWithLengthValue
        // ============================================================

        [Fact]
        public void TryPeekBytesWithLength_ShouldReturnCorrectValue_WithoutAdvancing()
        {
            // Arrange
            var payload = new byte[] { 0xAA, 0xBB, 0xCC };
            var data = new byte[4 + payload.Length];
            BinaryPrimitives.WriteInt32BigEndian(data, payload.Length);
            payload.CopyTo(data, 4);
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekBytesWithLengthValue(ref reader, out byte[] value);

            // Assert
            Assert.True(result);
            Assert.Equal(payload, value);
            Assert.Equal(data.Length, reader.Remaining);
        }

        [Fact]
        public void TryPeekBytesWithLength_ZeroLength_ShouldReturnEmptyArray()
        {
            // Arrange
            var data = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryPeekBytesWithLengthValue(ref reader, out byte[] value);

            // Assert
            Assert.True(result);
            Assert.Empty(value);
        }

        [Fact]
        public void TryPeekBytesWithLength_InsufficientData_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x00 });

            // Act
            var result = SequenceReaderExtensions.TryPeekBytesWithLengthValue(ref reader, out byte[] value);

            // Assert
            Assert.False(result);
        }

        // ============================================================
        // TryReadBytesValue 边界
        // ============================================================

        [Fact]
        public void TryReadBytes_ZeroLength_ShouldReturnEmptyArray()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x01, 0x02 });

            // Act
            var result = SequenceReaderExtensions.TryReadBytesValue(ref reader, 0, out byte[] value);

            // Assert
            Assert.True(result);
            Assert.Empty(value);
            Assert.Equal(2, reader.Remaining); // No advancement
        }

        [Fact]
        public void TryReadBytes_InsufficientData_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x01 });

            // Act
            var result = SequenceReaderExtensions.TryReadBytesValue(ref reader, 5, out byte[] value);

            // Assert
            Assert.False(result);
        }

        // ============================================================
        // TryReadBytesWithLengthValue 边界
        // ============================================================

        [Fact]
        public void TryReadBytesWithLength_ZeroLength_ShouldReturnEmptyArray()
        {
            // Arrange
            var data = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryReadBytesWithLengthValue(ref reader, out byte[] value);

            // Assert
            Assert.True(result);
            Assert.Empty(value);
        }

        [Fact]
        public void TryReadBytesWithLength_InsufficientLengthHeader_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x00, 0x01 });

            // Act
            var result = SequenceReaderExtensions.TryReadBytesWithLengthValue(ref reader, out byte[] value);

            // Assert
            Assert.False(result);
        }

        // ============================================================
        // TryReadStringValue 边界
        // ============================================================

        [Fact]
        public void TryReadString_ZeroLength_ShouldReturnEmptyString()
        {
            // Arrange
            var data = new byte[] { 0x00, 0x00 }; // short length = 0
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryReadStringValue(ref reader, out string value);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, value);
        }

        [Fact]
        public void TryReadStringWithIntLength_ZeroLength_ShouldReturnEmptyString()
        {
            // Arrange
            var data = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            var reader = CreateReader(data);

            // Act
            var result = SequenceReaderExtensions.TryReadStringWithIntLengthValue(ref reader, out string value);

            // Assert
            Assert.True(result);
            Assert.Equal(string.Empty, value);
        }

        // ============================================================
        // TryPeekBytesValue
        // ============================================================

        [Fact]
        public void TryPeekBytes_ShouldReturnCorrectValue_WithoutAdvancing()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x01, 0x02, 0x03, 0x04 });

            // Act
            var result = SequenceReaderExtensions.TryPeekBytesValue(ref reader, 3, out byte[] value);

            // Assert
            Assert.True(result);
            Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, value);
            Assert.Equal(4, reader.Remaining);
        }

        [Fact]
        public void TryPeekBytes_InsufficientData_ShouldReturnFalse()
        {
            // Arrange
            var reader = CreateReader(new byte[] { 0x01 });

            // Act
            var result = SequenceReaderExtensions.TryPeekBytesValue(ref reader, 5, out byte[] value);

            // Assert
            Assert.False(result);
            Assert.Null(value);
        }

        // ============================================================
        // 空序列测试
        // ============================================================

        [Fact]
        public void TryReadBigEndian_EmptySequence_ShouldReturnFalseForAllTypes()
        {
            // Arrange
            var reader = CreateReader(Array.Empty<byte>());

            // Act & Assert
            Assert.False(SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out byte _));
            Assert.False(SequenceReaderExtensions.TryReadBigEndianValue(ref reader, out bool _));
        }

        [Fact]
        public void TryPeekBigEndian_EmptySequence_ShouldReturnFalseForAllTypes()
        {
            // Arrange
            var reader = CreateReader(Array.Empty<byte>());

            // Act & Assert
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out byte _));
            Assert.False(SequenceReaderExtensions.TryPeekBigEndianValue(ref reader, out bool _));
        }
    }
}
