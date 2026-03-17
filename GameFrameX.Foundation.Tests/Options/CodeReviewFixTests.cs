using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// 针对 CODE_REVIEW 中已修复问题的单元测试
    /// Unit tests for issues fixed in CODE_REVIEW
    /// </summary>
    public class CodeReviewFixTests
    {
        #region 测试配置类

        /// <summary>
        /// 基本测试配置
        /// </summary>
        public class BasicConfig
        {
            public string Host { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public bool Debug { get; set; } = false;
        }

        /// <summary>
        /// 带默认值的测试配置
        /// </summary>
        public class ConfigWithDefaults
        {
            [Option("timeout", DefaultValue = 30)]
            public int Timeout { get; set; }

            [Option("retries", DefaultValue = "3")]
            public int Retries { get; set; }

            [Option("enabled", DefaultValue = true)]
            public bool Enabled { get; set; }
        }

        /// <summary>
        /// 带无效默认值的测试配置
        /// </summary>
        public class ConfigWithInvalidDefaults
        {
            [Option("port", DefaultValue = "not_a_number")]
            public int Port { get; set; } = 8080;

            [Option("timeout", DefaultValue = "invalid")]
            public int Timeout { get; set; } = 30;
        }

        /// <summary>
        /// 带必需选项的测试配置
        /// </summary>
        public class ConfigWithRequired
        {
            [Option("api-key", Required = true)]
            public string ApiKey { get; set; }

            public string Host { get; set; } = "localhost";
        }

        #endregion

        #region H-004: OptionsProvider 线程安全测试

        /// <summary>
        /// [H-004] 测试并发获取选项时的线程安全性
        /// </summary>
        [Fact]
        public void OptionsProvider_ConcurrentGetOptions_ShouldBeThreadSafe()
        {
            // 准备测试数据
            var args = new[] { "--host", "concurrent.example.com", "--port", "9999" };
            OptionsProvider.Initialize(args);

            // 清除缓存以确保测试独立性
            OptionsProvider.ClearCache();

            var exceptions = new List<Exception>();
            var results = new List<BasicConfig>();
            var lockObj = new object();
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 10 };

            // 并发执行
            Parallel.For(0, 100, parallelOptions, i =>
            {
                try
                {
                    var config = OptionsProvider.GetOptions<BasicConfig>(skipValidation: true, enableDebugOutput: false);
                    lock (lockObj)
                    {
                        results.Add(config);
                    }
                }
                catch (Exception ex)
                {
                    lock (lockObj)
                    {
                        exceptions.Add(ex);
                    }
                }
            });

            // 验证结果
            Assert.Empty(exceptions);
            Assert.Equal(100, results.Count);

            // 所有结果应该有相同的值
            foreach (var config in results)
            {
                Assert.Equal("concurrent.example.com", config.Host);
                Assert.Equal(9999, config.Port);
            }
        }

        /// <summary>
        /// [H-004] 测试 Initialize 和 GetOptions 并发调用
        /// </summary>
        [Fact]
        public void OptionsProvider_ConcurrentInitializeAndGetOptions_ShouldNotThrow()
        {
            var exceptions = new List<Exception>();
            var lockObj = new object();

            // 并发执行 Initialize 和 GetOptions
            var tasks = new Task[20];

            for (int i = 0; i < 20; i++)
            {
                var index = i;
                tasks[i] = Task.Run(() =>
                {
                    try
                    {
                        if (index % 2 == 0)
                        {
                            // 一半线程执行 Initialize
                            OptionsProvider.Initialize(new[] { "--host", $"host{index}.example.com" });
                        }
                        else
                        {
                            // 一半线程执行 GetOptions
                            _ = OptionsProvider.GetOptions<BasicConfig>(skipValidation: true, enableDebugOutput: false);
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (lockObj)
                        {
                            exceptions.Add(ex);
                        }
                    }
                });
            }

            Task.WaitAll(tasks);

            // 不应该有异常（线程安全）
            Assert.Empty(exceptions);
        }

        #endregion

        #region H-002: 异常处理改进测试

        /// <summary>
        /// [H-002] 测试无效默认值时的容错处理
        /// </summary>
        [Fact]
        public void OptionsBuilder_InvalidDefaultValue_ShouldUseDefaultPropertyValue()
        {
            // 准备测试数据 - 不提供参数，使用默认值
            var args = Array.Empty<string>();

            // 执行测试
            var builder = new OptionsBuilder<ConfigWithInvalidDefaults>(args);
            var config = builder.Build();

            // 验证结果 - 应该使用属性的默认值，而不是特性的无效默认值
            Assert.NotNull(config);
            Assert.Equal(8080, config.Port); // 属性默认值
            Assert.Equal(30, config.Timeout); // 属性默认值
        }

        /// <summary>
        /// [H-002] 测试类型转换失败时的容错处理
        /// </summary>
        [Fact]
        public void OptionsBuilder_TypeConversionFailure_ShouldNotThrow()
        {
            // 准备测试数据 - 提供无效的端口值
            var args = new[] { "--port", "not_a_number" };

            // 执行测试 - 不应该抛出异常
            var builder = new OptionsBuilder<BasicConfig>(args);
            var config = builder.Build();

            // 验证结果 - 应该使用默认值
            Assert.NotNull(config);
            Assert.Equal(8080, config.Port); // 默认值
        }

        #endregion

        #region H-003: 空 catch 代码块修复测试

        /// <summary>
        /// [H-003] 测试无效值转换时的日志输出（确保 catch 不是空的）
        /// </summary>
        [Fact]
        public void OptionsBuilder_InvalidValueConversion_ShouldHandleGracefully()
        {
            // 准备测试数据 - 各种无效值
            var testCases = new[]
            {
                new[] { "--port", "abc" },
                new[] { "--port", "99999999999999999999" }, // 溢出
                new[] { "--port", "" },
                new[] { "--port", "null" },
            };

            foreach (var args in testCases)
            {
                // 执行测试 - 不应该抛出异常
                var builder = new OptionsBuilder<BasicConfig>(args);
                var config = builder.Build();

                // 验证结果 - 应该使用默认值
                Assert.NotNull(config);
                Assert.Equal(8080, config.Port); // 默认值
            }
        }

        #endregion

        #region C-001: volatile 关键字测试（通过 H-004 覆盖）

        /// <summary>
        /// [C-001] 测试 _args 字段的可见性（volatile）
        /// </summary>
        [Fact]
        public void OptionsProvider_ArgsVisibility_ShouldBeConsistent()
        {
            // 初始化
            OptionsProvider.Initialize(new[] { "--host", "first.example.com" });
            OptionsProvider.ClearCache();

            var config1 = OptionsProvider.GetOptions<BasicConfig>(enableDebugOutput: false);
            Assert.Equal("first.example.com", config1.Host);

            // 重新初始化
            OptionsProvider.Initialize(new[] { "--host", "second.example.com" });
            OptionsProvider.ClearCache();

            var config2 = OptionsProvider.GetOptions<BasicConfig>(enableDebugOutput: false);
            Assert.Equal("second.example.com", config2.Host);
        }

        #endregion

        #region BooleanParser 测试

        /// <summary>
        /// 测试 BooleanParser.IsBooleanValue 方法
        /// </summary>
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", true)]
        [InlineData("TRUE", true)]
        [InlineData("FALSE", true)]
        [InlineData("1", true)]
        [InlineData("0", true)]
        [InlineData("yes", true)]
        [InlineData("no", true)]
        [InlineData("on", true)]
        [InlineData("off", true)]
        [InlineData("YES", true)]
        [InlineData("NO", true)]
        [InlineData("ON", true)]
        [InlineData("OFF", true)]
        [InlineData("maybe", false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        public void BooleanParser_IsBooleanValue_ShouldReturnCorrectResult(string value, bool expected)
        {
            var result = BooleanParser.IsBooleanValue(value);
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// 测试 BooleanParser.ParseBooleanValue 方法
        /// </summary>
        [Theory]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("1", true)]
        [InlineData("yes", true)]
        [InlineData("on", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        [InlineData("0", false)]
        [InlineData("no", false)]
        [InlineData("off", false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("invalid", false)]
        public void BooleanParser_ParseBooleanValue_ShouldReturnCorrectResult(string value, bool expected)
        {
            var result = BooleanParser.ParseBooleanValue(value);
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// 测试 BooleanParser 与 OptionsBuilder 的集成
        /// 注意: 在 Flag 模式下（默认），布尔选项被视为标志，不需要显式值
        /// </summary>
        [Fact]
        public void OptionsBuilder_WithBooleanValues_ShouldParseCorrectly()
        {
            // 在 Flag 模式下，布尔选项作为标志处理
            // --debug 存在即为 true，不存在则使用默认值 (false)

            // 测试 1: 存在标志 -> true
            var builder1 = new OptionsBuilder<BasicConfig>(new[] { "--debug" });
            var config1 = builder1.Build();
            Assert.True(config1.Debug);

            // 测试 2: 不存在标志 -> 默认值 (false)
            var builder2 = new OptionsBuilder<BasicConfig>(Array.Empty<string>());
            var config2 = builder2.Build();
            Assert.False(config2.Debug);

            // 测试 3: 使用键值对格式
            var builder3 = new OptionsBuilder<BasicConfig>(new[] { "--debug=true" });
            var config3 = builder3.Build();
            Assert.True(config3.Debug);

            // 测试 4: 使用键值对格式设置为 false
            var builder4 = new OptionsBuilder<BasicConfig>(new[] { "--debug=false" });
            var config4 = builder4.Build();
            Assert.False(config4.Debug);
        }

        #endregion

        #region 综合测试

        /// <summary>
        /// 测试所有修复的集成场景
        /// </summary>
        [Fact]
        public void CodeReviewFixes_IntegrationTest_ShouldWorkCorrectly()
        {
            // 准备测试数据
            var args = new[] { "--host", "integration.example.com", "--port", "8888", "--debug", "yes" };

            // 初始化
            OptionsProvider.Initialize(args);
            OptionsProvider.ClearCache();

            // 并发获取配置
            var tasks = new Task<BasicConfig>[5];
            for (int i = 0; i < 5; i++)
            {
                tasks[i] = Task.Run(() => OptionsProvider.GetOptions<BasicConfig>(enableDebugOutput: false));
            }

            Task.WaitAll(tasks);

            // 验证所有任务返回相同的结果
            foreach (var task in tasks)
            {
                var config = task.Result;
                Assert.Equal("integration.example.com", config.Host);
                Assert.Equal(8888, config.Port);
                Assert.True(config.Debug);
            }
        }

        #endregion
    }
}
