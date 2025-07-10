using System;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// NullObject&lt;T&gt; 结构体单元测试
/// </summary>
public class NullObjectTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValue_ShouldSetItem()
    {
        // Arrange
        var value = "test";

        // Act
        var nullObject = new NullObject<string>(value);

        // Assert
        Assert.Equal(value, nullObject.Item);
    }

    [Fact]
    public void Constructor_WithNull_ShouldSetItemToNull()
    {
        // Arrange
        string value = null;

        // Act
        var nullObject = new NullObject<string>(value);

        // Assert
        Assert.Null(nullObject.Item);
    }

    #endregion

    #region Null Property Tests

    [Fact]
    public void Null_ShouldReturnNullObject()
    {
        // Act
        var nullObject = NullObject<string>.Null;

        // Assert
        Assert.Null(nullObject.Item);
    }

    #endregion

    #region CompareTo Object Tests

    [Fact]
    public void CompareTo_Object_WithNullObject_ShouldCompareCorrectly()
    {
        // Arrange
        var nullObject1 = new NullObject<string>("abc");
        var nullObject2 = new NullObject<string>("def");

        // Act
        var result = nullObject1.CompareTo((object)nullObject2);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_Object_WithNonNullObject_ShouldReturnZero()
    {
        // Arrange
        var nullObject = new NullObject<string>("test");
        var other = "test";

        // Act
        var result = nullObject.CompareTo(other);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_Object_WithNullItem_ShouldHandleCorrectly()
    {
        // Arrange
        var nullObject1 = new NullObject<string>(null);
        var nullObject2 = new NullObject<string>("test");

        // Act
        var result = nullObject1.CompareTo((object)nullObject2);

        // Assert
        Assert.Equal(-1, result); // null is less than non-null
    }

    #endregion

    #region CompareTo T Tests

    [Fact]
    public void CompareTo_T_WithComparableType_ShouldCompareCorrectly()
    {
        // Arrange
        var nullObject = new NullObject<int>(5);
        var other = 10;

        // Act
        var result = nullObject.CompareTo(other);

        // Assert
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareTo_T_WithNullItem_ShouldHandleCorrectly()
    {
        // Arrange
        var nullObject = new NullObject<string>(null);
        var other = "test";

        // Act
        var result = nullObject.CompareTo(other);

        // Assert
        Assert.Equal(-1, result); // null is less than non-null
    }

    [Fact]
    public void CompareTo_T_WithNullOther_ShouldHandleCorrectly()
    {
        // Arrange
        var nullObject = new NullObject<string>("test");
        string other = null;

        // Act
        var result = nullObject.CompareTo(other);

        // Assert
        Assert.Equal(1, result); // non-null is greater than null
    }

    [Fact]
    public void CompareTo_Object_WithIncompatibleType_ShouldThrowArgumentException()
    {
        // Arrange
        var nullObject = new NullObject<string>("test");
        var other = 123; // incompatible type

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => nullObject.CompareTo(other));
        Assert.Equal("Object must be of type NullObject<T> or T. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void CompareTo_Object_WithNull_ShouldHandleCorrectly()
    {
        // Arrange
        var nullObject1 = new NullObject<string>("test");
        var nullObject2 = new NullObject<string>(null);

        // Act
        var result1 = nullObject1.CompareTo((object?)null);
        var result2 = nullObject2.CompareTo((object?)null);

        // Assert
        Assert.Equal(1, result1); // non-null is greater than null
        Assert.Equal(0, result2); // null equals null
    }

    #endregion

    #region Equals Tests

    [Fact]
    public void Equals_WithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var nullObject1 = new NullObject<string>("test");
        var nullObject2 = new NullObject<string>("test");

        // Act
        var result = nullObject1.Equals(nullObject2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_WithDifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var nullObject1 = new NullObject<string>("test1");
        var nullObject2 = new NullObject<string>("test2");

        // Act
        var result = nullObject1.Equals(nullObject2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_WithBothNull_ShouldReturnTrue()
    {
        // Arrange
        var nullObject1 = new NullObject<string>(null);
        var nullObject2 = new NullObject<string>(null);

        // Act
        var result = nullObject1.Equals(nullObject2);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_ToT_ShouldReturnItem()
    {
        // Arrange
        var nullObject = new NullObject<string>("test");

        // Act
        string result = nullObject;

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void ImplicitConversion_FromT_ShouldCreateNullObject()
    {
        // Arrange
        var value = "test";

        // Act
        NullObject<string> nullObject = value;

        // Assert
        Assert.Equal(value, nullObject.Item);
    }

    #endregion

    #region ToString Tests

    [Fact]
    public void ToString_WithValue_ShouldReturnValueString()
    {
        // Arrange
        var nullObject = new NullObject<string>("test");

        // Act
        var result = nullObject.ToString();

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void ToString_WithNull_ShouldReturnNULL()
    {
        // Arrange
        var nullObject = new NullObject<string>(null);

        // Act
        var result = nullObject.ToString();

        // Assert
        Assert.Equal("NULL", result);
    }

    #endregion

    #region GetHashCode Tests

    [Fact]
    public void GetHashCode_WithValue_ShouldReturnValueHashCode()
    {
        // Arrange
        var value = "test";
        var nullObject = new NullObject<string>(value);
        var expectedHashCode = EqualityComparer<string>.Default.GetHashCode(value);

        // Act
        var result = nullObject.GetHashCode();

        // Assert
        Assert.Equal(expectedHashCode, result);
    }

    [Fact]
    public void GetHashCode_WithNull_ShouldReturnZero()
    {
        // Arrange
        var nullObject = new NullObject<string>(null);
        var expectedHashCode = EqualityComparer<string>.Default.GetHashCode(null);

        // Act
        var result = nullObject.GetHashCode();

        // Assert
        Assert.Equal(expectedHashCode, result);
    }

    #endregion
}