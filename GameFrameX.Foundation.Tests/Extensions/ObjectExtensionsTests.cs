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
/// ObjectExtensions 扩展类单元测试
/// </summary>
public class ObjectExtensionsTests
{
    #region IsNull Tests

    [Fact]
    public void IsNull_NullObject_ShouldReturnTrue()
    {
        // Arrange
        object obj = null;

        // Act
        var result = obj.IsNull();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNull_NotNullObject_ShouldReturnFalse()
    {
        // Arrange
        var obj = new object();

        // Act
        var result = obj.IsNull();

        // Assert
        Assert.False(result);
    }

    #endregion

    #region IsNotNull Tests

    [Fact]
    public void IsNotNull_NullObject_ShouldReturnFalse()
    {
        // Arrange
        object obj = null;

        // Act
        var result = obj.IsNotNull();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsNotNull_NotNullObject_ShouldReturnTrue()
    {
        // Arrange
        var obj = new object();

        // Act
        var result = obj.IsNotNull();

        // Assert
        Assert.True(result);
    }

    #endregion

    #region ThrowIfNull Tests

    [Fact]
    public void ThrowIfNull_NullObject_ShouldThrowArgumentNullException()
    {
        // Arrange
        object obj = null;
        var paramName = "testParam";

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(paramName));
        Assert.Equal(paramName, exception.ParamName);
    }

    [Fact]
    public void ThrowIfNull_NotNullObject_ShouldNotThrow()
    {
        // Arrange
        var obj = new object();
        var paramName = "testParam";

        // Act & Assert
        var exception = Record.Exception(() => obj.ThrowIfNull(paramName));
        Assert.Null(exception);
    }

    [Fact]
    public void ThrowIfNull_NullParamName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var obj = new object();
        string paramName = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(paramName));
    }

    #endregion

    #region CheckRange Int Tests

    [Fact]
    public void CheckRange_Int_ValidValue_ShouldNotThrow()
    {
        // Arrange
        int value = 5;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    [Fact]
    public void CheckRange_Int_ValueEqualToMin_ShouldNotThrow()
    {
        // Arrange - 闭区间：边界值是有效的
        int value = 0;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    [Fact]
    public void CheckRange_Int_ValueEqualToMax_ShouldNotThrow()
    {
        // Arrange - 闭区间：边界值是有效的
        int value = 10;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    [Fact]
    public void CheckRange_Int_InvalidRange_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        int value = 5;
        int minValue = 10;
        int maxValue = 5;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.CheckRange(minValue, maxValue));
    }

    [Fact]
    public void CheckRange_Int_ValueLessThanMin_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        int value = -1;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.CheckRange(minValue, maxValue));
    }

    [Fact]
    public void CheckRange_Int_ValueGreaterThanMax_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        int value = 11;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.CheckRange(minValue, maxValue));
    }

    #endregion

    #region IsRange Int Tests

    [Fact]
    public void IsRange_Int_ValidValue_ShouldReturnTrue()
    {
        // Arrange
        int value = 5;
        int minValue = 0;
        int maxValue = 10;

        // Act
        var result = value.IsRange(minValue, maxValue);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRange_Int_ValueEqualToMin_ShouldReturnTrue()
    {
        // Arrange - 闭区间：边界值是有效的
        int value = 0;
        int minValue = 0;
        int maxValue = 10;

        // Act
        var result = value.IsRange(minValue, maxValue);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRange_Int_ValueEqualToMax_ShouldReturnTrue()
    {
        // Arrange - 闭区间：边界值是有效的
        int value = 10;
        int minValue = 0;
        int maxValue = 10;

        // Act
        var result = value.IsRange(minValue, maxValue);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsRange_Int_InvalidRange_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        int value = 5;
        int minValue = 10;
        int maxValue = 5;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.IsRange(minValue, maxValue));
    }

    #endregion

    #region CheckRange UInt Tests

    [Fact]
    public void CheckRange_UInt_ValidValue_ShouldNotThrow()
    {
        // Arrange
        uint value = 5;
        uint minValue = 0;
        uint maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    [Fact]
    public void CheckRange_UInt_ValueEqualToMin_ShouldNotThrow()
    {
        // Arrange - 闭区间：边界值是有效的
        uint value = 0;
        uint minValue = 0;
        uint maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    #endregion

    #region CheckRange Long Tests

    [Fact]
    public void CheckRange_Long_ValidValue_ShouldNotThrow()
    {
        // Arrange
        long value = 5;
        long minValue = 0;
        long maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    #endregion

    #region CheckRange ULong Tests

    [Fact]
    public void CheckRange_ULong_ValidValue_ShouldNotThrow()
    {
        // Arrange
        ulong value = 5;
        ulong minValue = 0;
        ulong maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    #endregion

    #region CheckRange Short Tests

    [Fact]
    public void CheckRange_Short_ValidValue_ShouldNotThrow()
    {
        // Arrange
        short value = 5;
        short minValue = 0;
        short maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    #endregion

    #region CheckRange UShort Tests

    [Fact]
    public void CheckRange_UShort_ValidValue_ShouldNotThrow()
    {
        // Arrange
        ushort value = 5;
        ushort minValue = 0;
        ushort maxValue = 10;

        // Act & Assert
        var exception = Record.Exception(() => value.CheckRange(minValue, maxValue));
        Assert.Null(exception);
    }

    #endregion
}