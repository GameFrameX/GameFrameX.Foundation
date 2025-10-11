using System;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options;

public class AttributeOptionsTests
{
    #region 测试模型类

    private class TestOptions
    {
        [Option('p', "port", DefaultValue = 8080, EnvironmentVariable = "TEST_PORT")]
        [HelpText("应用程序监听的端口")]
        public int Port { get; set; }

        [Option(LongName = "host", EnvironmentVariable = "TEST_HOST", DefaultValue = "localhost")]
        [HelpText("应用程序主机名")]
        public string Host { get; set; }

        [FlagOption(LongName = "verbose")]
        [HelpText("是否启用详细日志")]
        public bool Verbose { get; set; }

        [RequiredOption(LongName = "api-key", Required = true)]
        [HelpText("API密钥")]
        public string ApiKey { get; set; }

        public string NoAttributeProperty { get; set; } = "默认值";
    }

    private class TestOptionsWithMultipleAttributes
    {
        [Option('c', "connection-string", EnvironmentVariable = "TEST_CONNECTION")]
        [RequiredOption(Required = true)]
        [HelpText("数据库连接字符串")]
        public string ConnectionString { get; set; }
    }

    #endregion

    [Fact]
    public void Build_WithDefaultValues_ShouldApplyDefaultValues()
    {
        // 安排
        var args = Array.Empty<string>();
        var builder = new OptionsBuilder<TestOptions>(args);

        // 执行 - 跳过验证，因为我们只想测试默认值
        var options = builder.Build(skipValidation: true);

        // 断言
        Assert.Equal(8080, options.Port);
        Assert.Equal("localhost", options.Host);
        Assert.False(options.Verbose);
        Assert.Null(options.ApiKey); // 虽然是必需的，但在这个测试中我们不验证必需项
        Assert.Equal("默认值", options.NoAttributeProperty);
    }

    [Fact]
    public void Build_WithCommandLineArgs_ShouldOverrideDefaultValues()
    {
        // 安排
        var args = new[] { "--port", "9090", "--host", "example.com", "--verbose", "--api-key", "secret123" };
        var builder = new OptionsBuilder<TestOptions>(args);

        // 执行
        var options = builder.Build();

        // 断言
        Assert.Equal(9090, options.Port);
        Assert.Equal("example.com", options.Host);
        Assert.True(options.Verbose);
        Assert.Equal("secret123", options.ApiKey);
        Assert.Equal("默认值", options.NoAttributeProperty);
    }

    [Fact]
    public void Build_WithShortNameArgs_ShouldMapToProperties()
    {
        // 安排
        var args = new[] { "-p", "5000", "--api-key", "secret123" };
        var builder = new OptionsBuilder<TestOptions>(args);

        // 执行
        var options = builder.Build();

        // 断言
        Assert.Equal(5000, options.Port);
        Assert.Equal("localhost", options.Host); // 默认值
        Assert.False(options.Verbose); // 默认值
        Assert.Equal("secret123", options.ApiKey);
    }

    [Fact]
    public void Build_WithEnvironmentVariables_ShouldApplyValues()
    {
        // 安排
        var args = new[] { "--api-key", "test-key" }; // 添加必需的api-key
        
        // 设置环境变量
        Environment.SetEnvironmentVariable("TEST_PORT", "7070");
        Environment.SetEnvironmentVariable("TEST_HOST", "env-host.com");
        
        var builder = new OptionsBuilder<TestOptions>(args);

        try
        {
            // 执行
            var options = builder.Build();

            // 断言
            Assert.Equal(7070, options.Port);
            Assert.Equal("env-host.com", options.Host);
            Assert.False(options.Verbose); // 默认值
            Assert.Equal("test-key", options.ApiKey);
        }
        finally
        {
            // 清理环境变量
            Environment.SetEnvironmentVariable("TEST_PORT", null);
            Environment.SetEnvironmentVariable("TEST_HOST", null);
        }
    }

    [Fact]
    public void Build_WithCommandLineAndEnvironmentVariables_ShouldPreferCommandLine()
    {
        // 安排
        var args = new[] { "--port", "9090", "--api-key", "test-key" }; // 添加必需的api-key
        
        // 设置环境变量
        Environment.SetEnvironmentVariable("TEST_PORT", "7070");
        Environment.SetEnvironmentVariable("TEST_HOST", "env-host.com");
        
        var builder = new OptionsBuilder<TestOptions>(args);

        try
        {
            // 执行
            var options = builder.Build();

            // 断言
            Assert.Equal(9090, options.Port); // 命令行参数优先
            Assert.Equal("env-host.com", options.Host); // 环境变量
            Assert.False(options.Verbose); // 默认值
            Assert.Equal("test-key", options.ApiKey);
        }
        finally
        {
            // 清理环境变量
            Environment.SetEnvironmentVariable("TEST_PORT", null);
            Environment.SetEnvironmentVariable("TEST_HOST", null);
        }
    }

    [Fact]
    public void Build_WithMissingRequiredOption_ShouldThrowException()
    {
        // 安排
        var args = new[] { "--port", "9090" }; // 缺少必需的 api-key
        var builder = new OptionsBuilder<TestOptions>(args);

        // 执行 & 断言
        var exception = Assert.Throws<ArgumentException>(() => builder.Build());
        Assert.Contains("api-key", exception.Message);
    }

    [Fact]
    public void Build_WithBooleanFlag_ShouldSetToTrue()
    {
        // 安排
        var args = new[] { "--verbose", "--api-key", "secret123" };
        var builder = new OptionsBuilder<TestOptions>(args);

        // 执行
        var options = builder.Build();

        // 断言
        Assert.True(options.Verbose);
    }

    [Fact]
    public void Build_WithMultipleAttributes_ShouldRespectAllAttributes()
    {
        // 安排
        var args = new[] { "-c", "Server=localhost;Database=test" };
        var builder = new OptionsBuilder<TestOptionsWithMultipleAttributes>(args);

        // 执行
        var options = builder.Build();

        // 断言
        Assert.Equal("Server=localhost;Database=test", options.ConnectionString);
    }

    [Fact]
    public void Build_WithMissingRequiredOptionInMultipleAttributes_ShouldThrowException()
    {
        // 安排
        var args = Array.Empty<string>(); // 缺少必需的 connection-string
        var builder = new OptionsBuilder<TestOptionsWithMultipleAttributes>(args);

        // 执行 & 断言
        var exception = Assert.Throws<ArgumentException>(() => builder.Build());
        Assert.Contains("connectionstring", exception.Message);
    }

    [Fact]
    public void Build_WithEnvironmentVariableInMultipleAttributes_ShouldApplyValue()
    {
        // 安排
        var args = Array.Empty<string>();
        
        // 设置环境变量
        Environment.SetEnvironmentVariable("TEST_CONNECTION", "Server=env-server;Database=env-db");
        
        var builder = new OptionsBuilder<TestOptionsWithMultipleAttributes>(args);

        try
        {
            // 执行
            var options = builder.Build();

            // 断言
            Assert.Equal("Server=env-server;Database=env-db", options.ConnectionString);
        }
        finally
        {
            // 清理环境变量
            Environment.SetEnvironmentVariable("TEST_CONNECTION", null);
        }
    }
}