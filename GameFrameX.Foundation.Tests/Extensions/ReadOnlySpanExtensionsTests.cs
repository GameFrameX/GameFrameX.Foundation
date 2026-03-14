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

using System;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ReadOnlySpanExtensions 扩展类单元测试
/// </summary>
public class ReadOnlySpanExtensionsTests
{


















    #region Little Endian Tests

    #region ReadUIntLittleEndianValue Tests

    [Fact]
    public void ReadUIntLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x78, 0x56, 0x34, 0x12, 0x9A, 0xBC }; // 0x12345678 in little-endian
        var offset = 0;
        const uint expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUIntLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUIntLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x78, 0x56, 0x34 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadUIntLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x78, 0x56, 0x34, 0x12 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadIntLittleEndianValue Tests

    [Fact]
    public void ReadIntLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x78, 0x56, 0x34, 0x12, 0x9A, 0xBC }; // 0x12345678 in little-endian
        var offset = 0;
        const int expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadIntLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadIntLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x78, 0x56, 0x34 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadIntLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadIntLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x78, 0x56, 0x34, 0x12 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadIntLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadULongLittleEndianValue Tests

    [Fact]
    public void ReadULongLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12, 0x11 }; // 0x123456789ABCDEF0 in little-endian
        var offset = 0;
        const ulong expected = 0x123456789ABCDEF0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadULongLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadULongLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34 }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadULongLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadULongLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadULongLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadLongLittleEndianValue Tests

    [Fact]
    public void ReadLongLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12, 0x11 }; // 0x123456789ABCDEF0 in little-endian
        var offset = 0;
        const long expected = 0x123456789ABCDEF0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadLongLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadLongLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34 }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadLongLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadLongLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadLongLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadUShortLittleEndianValue Tests

    [Fact]
    public void ReadUShortLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x34, 0x12, 0x56, 0x78 }; // 0x1234 in little-endian
        var offset = 0;
        const ushort expected = 0x1234;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUShortLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadUShortLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x34 }; // Only 1 byte, need 2
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUShortLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadUShortLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x34, 0x12 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUShortLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadShortLittleEndianValue Tests

    [Fact]
    public void ReadShortLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x34, 0x12, 0x56, 0x78 }; // 0x1234 in little-endian
        var offset = 0;
        const short expected = 0x1234;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadShortLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadShortLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x34 }; // Only 1 byte, need 2
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadShortLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadShortLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x34, 0x12 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadShortLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadFloatLittleEndianValue Tests

    [Fact]
    public void ReadFloatLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x00, 0x00, 0x28, 0x42, 0x00 }; // 42.0f in little-endian
        var offset = 0;
        const float expected = 42.0f;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadFloatLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadFloatLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x00, 0x00, 0x28 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadFloatLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadFloatLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x00, 0x00, 0x28, 0x42 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadFloatLittleEndianValue(ref offset));
    }

    #endregion

    #region ReadDoubleLittleEndianValue Tests

    [Fact]
    public void ReadDoubleLittleEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x45, 0x40, 0x00 }; // 42.0 in little-endian
        var offset = 0;
        const double expected = 42.0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadDoubleLittleEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadDoubleLittleEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x45 }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadDoubleLittleEndianValue(ref offset));
    }

    [Fact]
    public void ReadDoubleLittleEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x45, 0x40 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadDoubleLittleEndianValue(ref offset));
    }

    #endregion

    #endregion

    #region Big Endian Tests

    #region ReadUIntBigEndianValue Tests

    [Fact]
    public void ReadUIntBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC }; // 0x12345678 in big-endian
        var offset = 0;
        const uint expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadUIntBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadUIntBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUIntBigEndianValue(ref offset));
    }

    #endregion

    #region ReadIntBigEndianValue Tests

    [Fact]
    public void ReadIntBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC }; // 0x12345678 in big-endian
        var offset = 0;
        const int expected = 0x12345678;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadIntBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadIntBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadIntBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadIntBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadIntBigEndianValue(ref offset));
    }

    #endregion

    #region ReadULongBigEndianValue Tests

    [Fact]
    public void ReadULongBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0x11 }; // 0x123456789ABCDEF0 in big-endian
        var offset = 0;
        const ulong expected = 0x123456789ABCDEF0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadULongBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadULongBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadULongBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadULongBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadULongBigEndianValue(ref offset));
    }

    #endregion

    #region ReadLongBigEndianValue Tests

    [Fact]
    public void ReadLongBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0x11 }; // 0x123456789ABCDEF0 in big-endian
        var offset = 0;
        const long expected = 0x123456789ABCDEF0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadLongBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadLongBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadLongBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadLongBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadLongBigEndianValue(ref offset));
    }

    #endregion

    #region ReadUShortBigEndianValue Tests

    [Fact]
    public void ReadUShortBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 }; // 0x1234 in big-endian
        var offset = 0;
        const ushort expected = 0x1234;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadUShortBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadUShortBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12 }; // Only 1 byte, need 2
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUShortBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadUShortBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadUShortBigEndianValue(ref offset));
    }

    #endregion

    #region ReadShortBigEndianValue Tests

    [Fact]
    public void ReadShortBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34, 0x56, 0x78 }; // 0x1234 in big-endian
        var offset = 0;
        const short expected = 0x1234;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadShortBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(2, offset);
    }

    [Fact]
    public void ReadShortBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12 }; // Only 1 byte, need 2
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadShortBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadShortBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x12, 0x34 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadShortBigEndianValue(ref offset));
    }

    #endregion

    #region ReadFloatBigEndianValue Tests

    [Fact]
    public void ReadFloatBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x42, 0x28, 0x00, 0x00, 0x00 }; // 42.0f in big-endian
        var offset = 0;
        const float expected = 42.0f;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadFloatBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(4, offset);
    }

    [Fact]
    public void ReadFloatBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x42, 0x28, 0x00 }; // Only 3 bytes, need 4
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadFloatBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadFloatBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x42, 0x28, 0x00, 0x00 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadFloatBigEndianValue(ref offset));
    }

    #endregion

    #region ReadDoubleBigEndianValue Tests

    [Fact]
    public void ReadDoubleBigEndianValue_ValidInput_ShouldReturnCorrectValue()
    {
        // Arrange
        var buffer = new byte[] { 0x40, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }; // 42.0 in big-endian
        var offset = 0;
        const double expected = 42.0;

        // Act
        var result = ((ReadOnlySpan<byte>)buffer).ReadDoubleBigEndianValue(ref offset);

        // Assert
        Assert.Equal(expected, result);
        Assert.Equal(8, offset);
    }

    [Fact]
    public void ReadDoubleBigEndianValue_OffsetOutOfBounds_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x40, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00 }; // Only 7 bytes, need 8
        var offset = 0;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadDoubleBigEndianValue(ref offset));
    }

    [Fact]
    public void ReadDoubleBigEndianValue_NegativeOffset_ShouldThrowException()
    {
        // Arrange
        var buffer = new byte[] { 0x40, 0x45, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        var offset = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => ((ReadOnlySpan<byte>)buffer).ReadDoubleBigEndianValue(ref offset));
    }

    #endregion

    #endregion
}