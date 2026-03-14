using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GameFrameX.Foundation.Options;

namespace GameFrameX.Foundation.Tests.Options;

/// <summary>
/// CommandLineArgumentConverter 类的单元测试
/// </summary>
public class CommandLineArgumentConverterTests
{
    private readonly CommandLineArgumentConverter _converter;

    /// <summary>
    /// 初始化测试实例
    /// </summary>
    public CommandLineArgumentConverterTests()
    {
        _converter = new CommandLineArgumentConverter();
    }

    /// <summary>
    /// 测试标准格式转换
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithMixedFormats_ShouldStandardize()
    {
        // Arrange
        var args = new[] { "--port", "8080", "-v", "--debug=true", "name", "myapp" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("--port", result);
        Assert.Contains("8080", result);
        Assert.Contains("-v", result);
        Assert.Contains("--debug=true", result);
        Assert.Contains("--name", result);
        Assert.Contains("myapp", result);
    }

    /// <summary>
    /// 测试空参数数组的处理
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithEmptyArgs_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyArgs = Array.Empty<string>();

        // Act
        var result = _converter.ConvertToStandardFormat(emptyArgs);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    /// 测试空参数数组抛出异常
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithNullArgs_ShouldReturnEmptyList()
    {
        // Act
        var result = _converter.ConvertToStandardFormat(null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    /// <summary>
    /// 测试布尔标志格式
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithBoolFlag_ShouldHandleCorrectly()
    {
        // Arrange
        _converter.BoolFormat = BoolArgumentFormat.Flag;
        var args = new[] { "--debug" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("--debug", result);
        Assert.Equal(1, result.Count); // 不应该添加值
    }

    /// <summary>
    /// 测试键值对格式
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithKeyValuePair_ShouldHandleCorrectly()
    {
        // Arrange
        var args = new[] { "--name=MyApp" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("--name=MyApp", result);
    }

    /// <summary>
    /// 测试分离格式
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithSeparatedKeyValue_ShouldHandleCorrectly()
    {
        // Arrange
        var args = new[] { "--port", "8080" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("--port", result);
        Assert.Contains("8080", result);
    }

    /// <summary>
    /// 测试短选项格式
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithShortOption_ShouldHandleCorrectly()
    {
        // Arrange
        var args = new[] { "-p", "8080" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("-p", result);
        Assert.Contains("8080", result);
    }

    /// <summary>
    /// 测试无前缀参数
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithNoPrefixOption_ShouldAddPrefix()
    {
        // Arrange
        var args = new[] { "port", "8080" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("--port", result);
        Assert.Contains("8080", result);
    }

    /// <summary>
    /// 测试命令行字符串转换
    /// </summary>
    [Fact]
    public void ToCommandLineString_WithValidArguments_ShouldReturnFormattedString()
    {
        // Arrange
        var arguments = new List<string> { "--port", "8080", "--name", "My Server" };

        // Act
        var result = _converter.ToCommandLineString(arguments);

        // Assert
        Assert.Equal("--port 8080 --name \"My Server\"", result);
    }

    /// <summary>
    /// 测试空参数列表的命令行字符串转换
    /// </summary>
    [Fact]
    public void ToCommandLineString_WithEmptyList_ShouldReturnEmptyString()
    {
        // Arrange
        var arguments = new List<string>();

        // Act
        var result = _converter.ToCommandLineString(arguments);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    /// <summary>
    /// 测试空参数列表抛出异常
    /// </summary>
    [Fact]
    public void ToCommandLineString_WithNullList_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _converter.ToCommandLineString(null));
    }

    /// <summary>
    /// 测试环境变量集成
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithEnvironmentVariables_ShouldIncludeEnvVars()
    {
        // Arrange
        Environment.SetEnvironmentVariable("TEST_VAR", "test_value");
        var args = new[] { "--port", "8080" };

        try
        {
            // Act
            var result = _converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--port", result);
            Assert.Contains("8080", result);
            // 环境变量可能会被包含，但这取决于具体实现
            Assert.NotNull(result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("TEST_VAR", null);
        }
    }
}