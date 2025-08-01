using System;
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// OptionsProvider 类的单元测试
    /// </summary>
    public class OptionsProviderTests
    {
        /// <summary>
        /// 测试配置类
        /// </summary>
        public class TestConfig
        {
            public string Host { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public bool Debug { get; set; } = false;
        }

        /// <summary>
        /// 测试基本的获取选项功能
        /// </summary>
        [Fact]
        public void GetOptions_WithValidArgs_ShouldReturnConfigObject()
        {
            // 准备测试数据
            var args = new[] { "--host", "example.com", "--port", "9090", "--debug" };
            
            // 初始化选项提供者
            OptionsProvider.Initialize(args);
            
            // 执行测试
            var config = OptionsProvider.GetOptions<TestConfig>();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("example.com", config.Host);
            Assert.Equal(9090, config.Port);
            Assert.True(config.Debug);
        }

        /// <summary>
        /// 测试空参数数组
        /// </summary>
        [Fact]
        public void GetOptions_WithEmptyArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            string[] args = Array.Empty<string>();
            
            // 初始化选项提供者
            OptionsProvider.Initialize(args);
            
            // 执行测试
            var config = OptionsProvider.GetOptions<TestConfig>();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
            Assert.False(config.Debug); // 默认值
        }

        /// <summary>
        /// 测试 null 参数数组
        /// </summary>
        [Fact]
        public void GetOptions_WithNullArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            string[] args = null;
            
            // 初始化选项提供者
            OptionsProvider.Initialize(args);
            
            // 执行测试
            var config = OptionsProvider.GetOptions<TestConfig>();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
            Assert.False(config.Debug); // 默认值
        }

        /// <summary>
        /// 测试自定义 Bool 格式
        /// </summary>
        [Fact]
        public void GetOptions_WithCustomBoolFormat_ShouldHandleCorrectly()
        {
            // 准备测试数据
            var args = new[] { "--debug=true" };
            
            // 创建选项构建器
            var builder = new OptionsBuilder<TestConfig>(args, BoolArgumentFormat.KeyValue);
            
            // 执行测试
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.True(config.Debug);
        }

        /// <summary>
        /// 测试禁用前缀处理
        /// </summary>
        [Fact]
        public void GetOptions_WithDisabledPrefixHandling_ShouldNotAddPrefixes()
        {
            // 准备测试数据
            var args = new[] { "host", "example.com", "port", "9090" };
            
            // 创建选项构建器
            var builder = new OptionsBuilder<TestConfig>(args, ensurePrefixedKeys: false);
            
            // 执行测试
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
        }

        /// <summary>
        /// 测试环境变量
        /// </summary>
        [Fact]
        public void GetOptions_WithEnvironmentVariables_ShouldIncludeThemInConfig()
        {
            // 准备测试数据
            var args = new[] { "--host", "example.com" };
            
            try
            {
                // 设置环境变量
                Environment.SetEnvironmentVariable("PORT", "7070");
                Environment.SetEnvironmentVariable("DEBUG", "true");
                
                // 初始化选项提供者
                OptionsProvider.Initialize(args);
                
                // 执行测试
                var config = OptionsProvider.GetOptions<TestConfig>();
                
                // 验证结果
                Assert.NotNull(config);
                Assert.Equal("example.com", config.Host); // 来自命令行参数
                Assert.Equal(7070, config.Port); // 来自环境变量
                Assert.True(config.Debug); // 来自环境变量
            }
            finally
            {
                // 清理环境变量
                Environment.SetEnvironmentVariable("PORT", null);
                Environment.SetEnvironmentVariable("DEBUG", null);
            }
        }
    }
}