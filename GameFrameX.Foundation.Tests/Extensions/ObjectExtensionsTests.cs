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
    public void CheckRange_Int_ValueEqualToMin_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        int value = 0;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.CheckRange(minValue, maxValue));
    }

    [Fact]
    public void CheckRange_Int_ValueEqualToMax_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        int value = 10;
        int minValue = 0;
        int maxValue = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.CheckRange(minValue, maxValue));
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
    public void IsRange_Int_ValueEqualToMin_ShouldReturnFalse()
    {
        // Arrange
        int value = 0;
        int minValue = 0;
        int maxValue = 10;

        // Act
        var result = value.IsRange(minValue, maxValue);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsRange_Int_ValueEqualToMax_ShouldReturnFalse()
    {
        // Arrange
        int value = 10;
        int minValue = 0;
        int maxValue = 10;

        // Act
        var result = value.IsRange(minValue, maxValue);

        // Assert
        Assert.False(result);
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
    public void CheckRange_UInt_ValueEqualToMin_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        uint value = 0;
        uint minValue = 0;
        uint maxValue = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => value.CheckRange(minValue, maxValue));
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