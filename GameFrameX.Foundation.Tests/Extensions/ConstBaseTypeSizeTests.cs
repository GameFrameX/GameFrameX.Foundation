using System;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ConstBaseTypeSize 常量值测试：验证所有 public const 字段的字节大小值正确，
/// 防止常量被误改。使用反射读取运行时值与期望字面量比较——若源码中常量值被修改，
/// 测试会在运行时失败。
/// </summary>
public class ConstBaseTypeSizeTests
{
    /// <summary>
    /// 验证各个 public const 字段的值与预期字节大小一致。
    /// 使用 nameof 保证字段重命名时编译期即报错。
    /// </summary>
    /// <param name="fieldName">字段名称。</param>
    /// <param name="expectedSize">期望的字节大小。</param>
    [Theory]
    [InlineData(nameof(ConstBaseTypeSize.IntSize), 4)]
    [InlineData(nameof(ConstBaseTypeSize.ShortSize), 2)]
    [InlineData(nameof(ConstBaseTypeSize.LongSize), 8)]
    [InlineData(nameof(ConstBaseTypeSize.FloatSize), 4)]
    [InlineData(nameof(ConstBaseTypeSize.DoubleSize), 8)]
    [InlineData(nameof(ConstBaseTypeSize.ByteSize), 1)]
    [InlineData(nameof(ConstBaseTypeSize.SbyteSize), 1)]
    [InlineData(nameof(ConstBaseTypeSize.BoolSize), 1)]
    [InlineData(nameof(ConstBaseTypeSize.UIntSize), 4)]
    [InlineData(nameof(ConstBaseTypeSize.UShortSize), 2)]
    [InlineData(nameof(ConstBaseTypeSize.ULongSize), 8)]
    [InlineData(nameof(ConstBaseTypeSize.CharSize), 2)]
    [InlineData(nameof(ConstBaseTypeSize.DecimalSize), 16)]
    [InlineData(nameof(ConstBaseTypeSize.DateTimeSize), 8)]
    [InlineData(nameof(ConstBaseTypeSize.GuidSize), 16)]
    [InlineData(nameof(ConstBaseTypeSize.TimeSpanSize), 8)]
    public void ConstField_ShouldHaveExpectedByteSize(string fieldName, int expectedSize)
    {
        // Arrange
        var field = typeof(ConstBaseTypeSize).GetField(fieldName);

        // Act
        Assert.NotNull(field);
        var actualValue = (int)field.GetValue(null);

        // Assert
        Assert.Equal(expectedSize, actualValue);
    }

    /// <summary>
    /// 验证 ConstBaseTypeSize 恰好有 16 个 public const 字段，
    /// 防止字段被意外增删而不被发现。
    /// </summary>
    [Fact]
    public void ConstBaseTypeSize_ShouldHave16ConstFields()
    {
        // Arrange & Act
        var fields = typeof(ConstBaseTypeSize).GetFields();

        // Assert
        Assert.Equal(16, fields.Length);
    }

    /// <summary>
    /// 验证所有字段都是 const（literal）字段，而非普通 static 或实例字段。
    /// </summary>
    [Fact]
    public void AllFields_ShouldBeLiteralConstFields()
    {
        // Arrange & Act
        var fields = typeof(ConstBaseTypeSize).GetFields();

        // Assert — 每个字段都应是 IsLiteral = true（即 C# 的 const）
        Assert.All(fields, field => Assert.True(field.IsLiteral, $"字段 {field.Name} 应为 const (literal)"));
    }

    /// <summary>
    /// 验证所有字段的返回类型都是 int。
    /// </summary>
    [Fact]
    public void AllFields_ShouldBeInt32Type()
    {
        // Arrange & Act
        var fields = typeof(ConstBaseTypeSize).GetFields();

        // Assert
        Assert.All(fields, field => Assert.Equal(typeof(int), field.FieldType));
    }
}
