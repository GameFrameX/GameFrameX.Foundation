using System;
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// OptionsBuilder 类的单元测试
    /// </summary>
    public class OptionsBuilderTests
    {
        /// <summary>
        /// 测试配置类
        /// </summary>
        public class TestConfig
        {
            public string Host { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public bool Debug { get; set; } = false;
            public string LogLevel { get; set; } = "info";
            public double Timeout { get; set; } = 30.5;
        }

        /// <summary>
        /// 测试基本的构建功能
        /// </summary>
        [Fact]
        public void Build_WithValidArgs_ShouldReturnConfigObject()
        {
            // 准备测试数据
            var args = new[] { "--host", "example.com", "--port", "9090", "--debug", "true" };
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("example.com", config.Host);
            Assert.Equal(9090, config.Port);
            Assert.True(config.Debug);
            Assert.Equal("info", config.LogLevel); // 默认值
            Assert.Equal(30.5, config.Timeout); // 默认值
        }

        /// <summary>
        /// 测试空参数数组
        /// </summary>
        [Fact]
        public void Build_WithEmptyArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            string[] args = Array.Empty<string>();
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
            Assert.False(config.Debug); // 默认值
            Assert.Equal("info", config.LogLevel); // 默认值
            Assert.Equal(30.5, config.Timeout); // 默认值
        }

        /// <summary>
        /// 测试 null 参数数组
        /// </summary>
        [Fact]
        public void Build_WithNullArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            string[] args = null;
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
            Assert.False(config.Debug); // 默认值
            Assert.Equal("info", config.LogLevel); // 默认值
            Assert.Equal(30.5, config.Timeout); // 默认值
        }

        /// <summary>
        /// 测试环境变量
        /// </summary>
        [Fact]
        public void Build_WithEnvironmentVariables_ShouldIncludeThemInConfig()
        {
            // 准备测试数据
            var args = new[] { "--host", "example.com" };
            
            try
            {
                // 设置环境变量
                Environment.SetEnvironmentVariable("PORT", "7070");
                Environment.SetEnvironmentVariable("DEBUG", "true");
                
                // 执行测试
                var builder = new OptionsBuilder<TestConfig>(args);
                var config = builder.Build();
                
                // 验证结果
                Assert.NotNull(config);
                Assert.Equal("example.com", config.Host); // 来自命令行参数
                Assert.Equal(7070, config.Port); // 来自环境变量
                Assert.True(config.Debug); // 来自环境变量
                Assert.Equal("info", config.LogLevel); // 默认值
                Assert.Equal(30.5, config.Timeout); // 默认值
            }
            finally
            {
                // 清理环境变量
                Environment.SetEnvironmentVariable("PORT", null);
                Environment.SetEnvironmentVariable("DEBUG", null);
            }
        }

        /// <summary>
        /// 测试参数优先级
        /// </summary>
        [Fact]
        public void Build_WithArgsAndEnvironmentVariables_ShouldPrioritizeArgs()
        {
            // 准备测试数据
            var args = new[] { "--port", "9090" };
            
            try
            {
                // 设置环境变量
                Environment.SetEnvironmentVariable("PORT", "7070");
                Environment.SetEnvironmentVariable("HOST", "env.example.com");
                
                // 执行测试
                var builder = new OptionsBuilder<TestConfig>(args);
                var config = builder.Build();
                
                // 验证结果
                Assert.NotNull(config);
                Assert.Equal("env.example.com", config.Host); // 来自环境变量
                Assert.Equal(9090, config.Port); // 来自命令行参数（优先级更高）
                Assert.False(config.Debug); // 默认值
                Assert.Equal("info", config.LogLevel); // 默认值
                Assert.Equal(30.5, config.Timeout); // 默认值
            }
            finally
            {
                // 清理环境变量
                Environment.SetEnvironmentVariable("PORT", null);
                Environment.SetEnvironmentVariable("HOST", null);
            }
        }

        /// <summary>
        /// 测试前缀处理
        /// </summary>
        [Fact]
        public void Build_WithNonPrefixedKeys_ShouldAddPrefixes()
        {
            // 准备测试数据
            var args = new[] { "host", "example.com", "port", "9090" };
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args, ensurePrefixedKeys: true);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("example.com", config.Host);
            Assert.Equal(9090, config.Port);
        }

        /// <summary>
        /// 测试禁用前缀处理
        /// </summary>
        [Fact]
        public void Build_WithDisabledPrefixHandling_ShouldNotAddPrefixes()
        {
            // 准备测试数据
            var args = new[] { "host", "example.com", "port", "9090" };
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args, ensurePrefixedKeys: false);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
        }

        /// <summary>
        /// 测试不同的 Bool 格式
        /// </summary>
        [Fact]
        public void Build_WithDifferentBoolFormats_ShouldHandleCorrectly()
        {
            // 标志格式
            var flagArgs = new[] { "--debug" };
            var flagBuilder = new OptionsBuilder<TestConfig>(flagArgs);
            var flagConfig = flagBuilder.Build();
            Assert.True(flagConfig.Debug);
            
            // 键值对格式
            var keyValueArgs = new[] { "--debug=true" };
            var keyValueBuilder = new OptionsBuilder<TestConfig>(keyValueArgs);
            var keyValueConfig = keyValueBuilder.Build();
            Assert.True(keyValueConfig.Debug);
            
            // 分离格式
            var separatedArgs = new[] { "--debug", "true" };
            var separatedBuilder = new OptionsBuilder<TestConfig>(separatedArgs);
            var separatedConfig = separatedBuilder.Build();
            Assert.True(separatedConfig.Debug);
        }

        /// <summary>
        /// 测试异常处理
        /// </summary>
        [Fact]
        public void Build_WithInvalidArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            var args = new[] { "--port", "invalid" }; // 无效的端口值
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
        }

        /// <summary>
        /// 测试包含空参数
        /// </summary>
        [Fact]
        public void Build_WithEmptyArgValues_ShouldHandleCorrectly()
        {
            // 准备测试数据
            var args = new[] { "--host", "", "--port", "9090" };
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("", config.Host); // 空字符串
            Assert.Equal(9090, config.Port);
        }

        /// <summary>
        /// 测试包含 null 参数
        /// </summary>
        [Fact]
        public void Build_WithNullArgValues_ShouldHandleCorrectly()
        {
            // 准备测试数据
            var args = new[] { "--host", null, "--port", "9090" };
            
            // 执行测试
            var builder = new OptionsBuilder<TestConfig>(args);
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(9090, config.Port);
        }
    }
}