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
        string other = null!;

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
        // 验证异常消息不为空且包含参数名（支持中英文本地化）
        Assert.NotNull(exception.Message);
        Assert.True(exception.Message.Length > 0);
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void CompareTo_Object_WithNull_ShouldHandleCorrectly()
    {
        // Arrange
        var nullObject1 = new NullObject<string>("test");
        var nullObject2 = new NullObject<string>(null);

        // Act
        var result1 = nullObject1.CompareTo((object)null!);
        var result2 = nullObject2.CompareTo((object)null!);

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