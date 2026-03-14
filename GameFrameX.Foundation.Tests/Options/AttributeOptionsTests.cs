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
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options;

public class AttributeOptionsTests
{
    #region 测试模型类

    private class TestOptions
    {
        [Option("port", DefaultValue = 8080, EnvironmentVariable = "TEST_PORT")]
        public int Port { get; set; }

        [Option(LongName = "host", EnvironmentVariable = "TEST_HOST", DefaultValue = "localhost")]
        public string Host { get; set; }

        [FlagOption(LongName = "verbose")]
        public bool Verbose { get; set; }

        [Option(LongName = "api-key", Required = true)]
        public string ApiKey { get; set; }

        public string NoAttributeProperty { get; set; } = "默认值";
    }

    private class TestOptionsWithMultipleAttributes
    {
        [Option("connection-string", EnvironmentVariable = "TEST_CONNECTION", Required = true)]
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
        var args = new[] { "--connection-string", "Server=localhost;Database=test" };
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
        Assert.Contains("connection-string", exception.Message);
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

    [Fact]
    public void Build_WithEmptyEnvironmentVariableValue_ShouldUseDefaultValue()
    {
        // 安排 - 测试空环境变量值不会导致IndexOutOfRangeException
        var args = new[] { "--api-key", "test-key" }; // 添加必需的api-key
        
        // 设置空的环境变量值
        Environment.SetEnvironmentVariable("TEST_PORT", "");
        Environment.SetEnvironmentVariable("TEST_HOST", "");
        
        var builder = new OptionsBuilder<TestOptions>(args);

        try
        {
            // 执行 - 这里不应该抛出IndexOutOfRangeException
            var options = builder.Build();

            // 断言 - 应该使用默认值，而不是空值
            Assert.Equal(8080, options.Port); // 使用默认值
            Assert.Equal("localhost", options.Host); // 使用默认值
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
    public void Build_WithUndefinedEnvironmentVariableValue_ShouldUseDefaultValue()
    {
        // 安排 - 测试"undefined"环境变量值不会导致IndexOutOfRangeException
        var args = new[] { "--api-key", "test-key" }; // 添加必需的api-key
        
        // 设置"undefined"的环境变量值（模拟某些系统中的情况）
        Environment.SetEnvironmentVariable("TEST_PORT", "undefined");
        Environment.SetEnvironmentVariable("TEST_HOST", "undefined");
        
        var builder = new OptionsBuilder<TestOptions>(args);

        try
        {
            // 执行 - 这里不应该抛出IndexOutOfRangeException
            var options = builder.Build();

            // 断言 - 对于字符串类型，"undefined"会被当作有效值使用
            // 对于数字类型，"undefined"无法解析，应该使用默认值
            Assert.Equal(8080, options.Port); // 无法解析"undefined"为int，使用默认值
            Assert.Equal("undefined", options.Host); // 字符串类型直接使用"undefined"
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
    public void Build_WithNullEnvironmentVariableValue_ShouldUseDefaultValue()
    {
        // 安排 - 测试null环境变量值（通过显式设置为null）
        var args = new[] { "--api-key", "test-key" }; // 添加必需的api-key
        
        // 显式设置环境变量为null（清除）
        Environment.SetEnvironmentVariable("TEST_PORT", null);
        Environment.SetEnvironmentVariable("TEST_HOST", null);
        
        var builder = new OptionsBuilder<TestOptions>(args);

        // 执行 - 这里不应该抛出任何异常
        var options = builder.Build();

        // 断言 - 应该使用默认值
        Assert.Equal(8080, options.Port); // 使用默认值
        Assert.Equal("localhost", options.Host); // 使用默认值
        Assert.Equal("test-key", options.ApiKey);
    }

    [Fact]
    public void Build_WithMixedEmptyAndValidEnvironmentVariables_ShouldHandleCorrectly()
    {
        // 安排 - 测试混合的空值和有效值环境变量
        var args = new[] { "--api-key", "test-key" }; // 添加必需的api-key
        
        // 设置混合的环境变量值
        Environment.SetEnvironmentVariable("TEST_PORT", "9999"); // 有效值
        Environment.SetEnvironmentVariable("TEST_HOST", ""); // 空值
        
        var builder = new OptionsBuilder<TestOptions>(args);

        try
        {
            // 执行 - 这里不应该抛出IndexOutOfRangeException
            var options = builder.Build();

            // 断言
            Assert.Equal(9999, options.Port); // 使用环境变量值
            Assert.Equal("localhost", options.Host); // 空值被忽略，使用默认值
            Assert.Equal("test-key", options.ApiKey);
        }
        finally
        {
            // 清理环境变量
            Environment.SetEnvironmentVariable("TEST_PORT", null);
            Environment.SetEnvironmentVariable("TEST_HOST", null);
        }
    }
}
