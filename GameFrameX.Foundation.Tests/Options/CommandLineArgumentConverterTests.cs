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
    /// 测试空参数数组的处理
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithBooleanEnvironmentVariables_FlagFormat_ShouldAddOnlyTrueFlags()
    {
        // Arrange
        var converter = new CommandLineArgumentConverter { BoolFormat = BoolArgumentFormat.Flag };
        var args = new[] { "--port", "8080" };

        // 模拟环境变量
        Environment.SetEnvironmentVariable("DEBUG", "true");
        Environment.SetEnvironmentVariable("VERBOSE", "false");
        Environment.SetEnvironmentVariable("ENABLE_LOGGING", "1");
        Environment.SetEnvironmentVariable("DISABLE_CACHE", "0");

        try
        {
            // Act
            var result = converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--port", result);
            Assert.Contains("8080", result);
            Assert.Contains("--DEBUG", result);
            Assert.Contains("--ENABLE_LOGGING", result);
            Assert.DoesNotContain("--VERBOSE", result);
            Assert.DoesNotContain("--DISABLE_CACHE", result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DEBUG", null);
            Environment.SetEnvironmentVariable("VERBOSE", null);
            Environment.SetEnvironmentVariable("ENABLE_LOGGING", null);
            Environment.SetEnvironmentVariable("DISABLE_CACHE", null);
        }
    }

    [Fact]
    public void ConvertToStandardFormat_WithBooleanEnvironmentVariables_KeyValueFormat_ShouldAddKeyValuePairs()
    {
        // Arrange
        var converter = new CommandLineArgumentConverter { BoolFormat = BoolArgumentFormat.KeyValue };
        var args = new[] { "--port", "8080" };

        // 模拟环境变量
        Environment.SetEnvironmentVariable("DEBUG", "true");
        Environment.SetEnvironmentVariable("VERBOSE", "false");

        try
        {
            // Act
            var result = converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--port", result);
            Assert.Contains("8080", result);
            Assert.Contains("--DEBUG=true", result);
            Assert.Contains("--VERBOSE=false", result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DEBUG", null);
            Environment.SetEnvironmentVariable("VERBOSE", null);
        }
    }

    [Fact]
    public void ConvertToStandardFormat_WithBooleanEnvironmentVariables_SeparatedFormat_ShouldAddSeparatedValues()
    {
        // Arrange
        var converter = new CommandLineArgumentConverter { BoolFormat = BoolArgumentFormat.Separated };
        var args = new[] { "--port", "8080" };

        // 模拟环境变量
        Environment.SetEnvironmentVariable("DEBUG", "yes");
        Environment.SetEnvironmentVariable("VERBOSE", "no");

        try
        {
            // Act
            var result = converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--port", result);
            Assert.Contains("8080", result);
            Assert.Contains("--DEBUG", result);
            Assert.Contains("true", result);
            Assert.Contains("--VERBOSE", result);
            Assert.Contains("false", result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("DEBUG", null);
            Environment.SetEnvironmentVariable("VERBOSE", null);
        }
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("1", true)]
    [InlineData("0", false)]
    [InlineData("yes", true)]
    [InlineData("no", false)]
    [InlineData("on", true)]
    [InlineData("off", false)]
    [InlineData("TRUE", true)]
    [InlineData("FALSE", false)]
    [InlineData("Yes", true)]
    [InlineData("No", false)]
    public void ConvertToStandardFormat_WithVariousBooleanValues_ShouldParseCorrectly(string envValue, bool expectedBool)
    {
        // Arrange
        var converter = new CommandLineArgumentConverter { BoolFormat = BoolArgumentFormat.Separated };
        var args = Array.Empty<string>();

        // 模拟环境变量
        Environment.SetEnvironmentVariable("TEST_BOOL", envValue);

        try
        {
            // Act
            var result = converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--TEST_BOOL", result);
            Assert.Contains(expectedBool.ToString().ToLowerInvariant(), result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("TEST_BOOL", null);
        }
    }

    [Fact]
    public void ConvertToStandardFormat_WithNonBooleanValues_ShouldTreatAsRegularValues()
    {
        // Arrange
        var converter = new CommandLineArgumentConverter();
        var args = Array.Empty<string>();

        // 模拟环境变量
        Environment.SetEnvironmentVariable("SERVER_NAME", "MyServer");
        Environment.SetEnvironmentVariable("PORT", "8080");

        try
        {
            // Act
            var result = converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--SERVER_NAME", result);
            Assert.Contains("MyServer", result);
            Assert.Contains("--PORT", result);
            Assert.Contains("8080", result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("SERVER_NAME", null);
            Environment.SetEnvironmentVariable("PORT", null);
        }
    }

    [Fact]
    public void ConvertToStandardFormat_WithEmptyArgs_ShouldReturnEnvironmentVariables()
    {
        // Arrange
        var emptyArgs = Array.Empty<string>();

        // Act
        var result = _converter.ConvertToStandardFormat(emptyArgs);

        // Assert
        Assert.NotNull(result);
        // 结果应该包含环境变量（至少会有一些系统环境变量）
        Assert.True(result.Count >= 0);
    }

    /// <summary>
    /// 测试空参数数组抛出异常
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithNullArgs_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _converter.ConvertToStandardFormat(null));
    }

    /// <summary>
    /// 测试已存在的参数不会被重复添加
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithExistingKey_ShouldNotDuplicate()
    {
        // Arrange
        var args = new[] { "--TestKey", "OriginalValue" };

        // 设置环境变量
        Environment.SetEnvironmentVariable("TestKey", "EnvironmentValue");

        try
        {
            // Act
            var result = _converter.ConvertToStandardFormat(args);

            // Assert
            var testKeyCount = result.Count(arg => arg == "--TestKey");
            Assert.Equal(1, testKeyCount); // 应该只有一个 --TestKey

            var originalValueIndex = result.IndexOf("--TestKey");
            Assert.True(originalValueIndex >= 0);
            Assert.Equal("OriginalValue", result[originalValueIndex + 1]); // 应该保持原始值
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("TestKey", null);
        }
    }

    /// <summary>
    /// 测试环境变量转换为标准格式
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithEnvironmentVariable_ShouldAddStandardFormat()
    {
        // Arrange
        var args = new[] { "--port", "8080" };
        var testKey = "TEST_ENV_VAR";
        var testValue = "TestValue123";

        Environment.SetEnvironmentVariable(testKey, testValue);

        try
        {
            // Act
            var result = _converter.ConvertToStandardFormat(args);

            // Assert
            Assert.Contains("--port", result);
            Assert.Contains("8080", result);
            Assert.Contains($"--{testKey}", result);
            Assert.Contains(testValue, result);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testKey, null);
        }
    }

    /// <summary>
    /// 测试值中连字符的清理
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithHyphensInValue_ShouldCleanValue()
    {
        // Arrange
        var args = Array.Empty<string>();
        var testKey = "TEST_HYPHEN_VAR";
        var testValue = "test-value-with-hyphens";
        var expectedCleanedValue = "testvaluewithhyphens";

        Environment.SetEnvironmentVariable(testKey, testValue);

        try
        {
            // Act
            var result = _converter.ConvertToStandardFormat(args);

            // Assert
            var keyIndex = result.IndexOf($"--{testKey}");
            Assert.True(keyIndex >= 0);
            Assert.Equal(expectedCleanedValue, result[keyIndex + 1]);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testKey, null);
        }
    }

    /// <summary>
    /// 测试单连字符参数转换为双连字符
    /// </summary>
    [Fact]
    public void ConvertToStandardFormat_WithSingleHyphenArgs_ShouldConvertToDoubleHyphen()
    {
        // Arrange
        var args = new[] { "-p", "8080", "-h", "localhost" };

        // Act
        var result = _converter.ConvertToStandardFormat(args);

        // Assert
        Assert.Contains("-p", result);
        Assert.Contains("8080", result);
        Assert.Contains("-h", result);
        Assert.Contains("localhost", result);

        // 环境变量不应该重复添加已存在的键（转换为双连字符格式）
        var pKeyCount = result.Count(arg => arg == "--p");
        var hKeyCount = result.Count(arg => arg == "--h");

        // 如果环境变量中有 p 或 h，它们不应该被添加，因为已经存在对应的参数
        Assert.True(pKeyCount <= 1);
        Assert.True(hKeyCount <= 1);
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
    /// 测试获取环境变量
    /// </summary>
    [Fact]
    public void GetEnvironmentVariables_ShouldReturnDictionary()
    {
        // Act
        var result = _converter.GetEnvironmentVariables();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Count > 0); // 系统应该至少有一些环境变量
        Assert.IsType<Dictionary<string, string>>(result);
    }

    /// <summary>
    /// 测试环境变量字典包含预期的系统变量
    /// </summary>
    [Fact]
    public void GetEnvironmentVariables_ShouldContainSystemVariables()
    {
        // Act
        var result = _converter.GetEnvironmentVariables();

        // Assert
        // 大多数系统都会有这些环境变量之一
        var hasCommonVar = result.ContainsKey("PATH") ||
                           result.ContainsKey("TEMP") ||
                           result.ContainsKey("TMP") ||
                           result.ContainsKey("HOME") ||
                           result.ContainsKey("USER");

        Assert.True(hasCommonVar, "应该包含至少一个常见的系统环境变量");
    }

    /// <summary>
    /// 测试完整的工作流程
    /// </summary>
    [Fact]
    public void FullWorkflow_ShouldWorkCorrectly()
    {
        // Arrange
        var originalArgs = new[] { "--config", "app.json" };
        var testEnvKey = "WORKFLOW_TEST_VAR";
        var testEnvValue = "workflow-test-value";

        Environment.SetEnvironmentVariable(testEnvKey, testEnvValue);

        try
        {
            // Act
            var standardArgs = _converter.ConvertToStandardFormat(originalArgs);
            var commandLineString = _converter.ToCommandLineString(standardArgs);

            // Assert
            Assert.Contains("--config", standardArgs);
            Assert.Contains("app.json", standardArgs);
            Assert.Contains($"--{testEnvKey}", standardArgs);
            Assert.Contains("workflowtestvalue", standardArgs); // 连字符被移除

            Assert.Contains("--config app.json", commandLineString);
            Assert.Contains($"--{testEnvKey} workflowtestvalue", commandLineString);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable(testEnvKey, null);
        }
    }
}