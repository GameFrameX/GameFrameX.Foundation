using System;
using System.IO;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// OptionsBuilder 静态便捷方法单元测试：
    /// Create / Create(full overload) / CreateFromArgsOnly / CreateFromEnvironmentOnly /
    /// CreateDefault / TryCreate / CreateWithDebug。
    /// </summary>
    public class OptionsBuilderStaticMethodsTests
    {
        // ============================================================
        // Create<T>(args, skipValidation)
        // ============================================================

        [Fact]
        public void Create_WithValidArgs_ShouldReturnOptions()
        {
            var args = new[] { "--host", "prod.example.com", "--port", "443" };
            var config = OptionsBuilder.Create<StaticTestConfig>(args);
            Assert.Equal("prod.example.com", config.Host);
            Assert.Equal(443, config.Port);
        }

        [Fact]
        public void Create_WithNullArgs_ShouldTreatAsEmpty()
        {
            var config = OptionsBuilder.Create<StaticTestConfig>(null);
            Assert.Equal("localhost", config.Host);
            Assert.Equal(8080, config.Port);
        }

        [Fact]
        public void Create_WithEmptyArgs_ShouldReturnDefaults()
        {
            var config = OptionsBuilder.Create<StaticTestConfig>(Array.Empty<string>());
            Assert.Equal("localhost", config.Host);
            Assert.Equal(8080, config.Port);
            Assert.False(config.Verbose);
        }

        [Fact]
        public void Create_WithSkipValidation_ShouldNotThrowOnMissingRequired()
        {
            var args = Array.Empty<string>();
            var config = OptionsBuilder.Create<StaticRequiredConfig>(args, skipValidation: true);
            Assert.Null(config.RequiredKey);
        }

        // ============================================================
        // Create<T>(args, boolFormat, ensurePrefixedKeys, useEnvironmentVariables, skipValidation)
        // ============================================================

        [Fact]
        public void Create_FullOverload_WithKeyValueBoolFormat_ShouldParseBoolean()
        {
            var args = new[] { "--verbose=true" };
            var config = OptionsBuilder.Create<StaticTestConfig>(
                args, BoolArgumentFormat.KeyValue, true, false);
            Assert.True(config.Verbose);
        }

        [Fact]
        public void Create_FullOverload_WithDisabledEnvVars_ShouldIgnoreEnvironment()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", "from-env");
                var args = new[] { "--port", "3000" };
                var config = OptionsBuilder.Create<StaticTestConfig>(
                    args, BoolArgumentFormat.Flag, true, false);
                Assert.Equal("localhost", config.Host);
                Assert.Equal(3000, config.Port);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", null);
            }
        }

        [Fact]
        public void Create_FullOverload_WithDisabledPrefix_ShouldNotMatchUnprefixedKeys()
        {
            var args = new[] { "host", "unprefixed.com" };
            var config = OptionsBuilder.Create<StaticTestConfig>(
                args, BoolArgumentFormat.Flag, false, false);
            Assert.Equal("localhost", config.Host);
        }

        // ============================================================
        // CreateFromArgsOnly
        // ============================================================

        [Fact]
        public void CreateFromArgsOnly_ShouldNotReadEnvironmentVariables()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", "from-env");
                var args = new[] { "--port", "3000" };
                var config = OptionsBuilder.CreateFromArgsOnly<StaticTestConfig>(args);
                Assert.Equal("localhost", config.Host);
                Assert.Equal(3000, config.Port);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", null);
            }
        }

        [Fact]
        public void CreateFromArgsOnly_WithNullArgs_ShouldReturnDefaults()
        {
            var config = OptionsBuilder.CreateFromArgsOnly<StaticTestConfig>(null);
            Assert.Equal("localhost", config.Host);
            Assert.Equal(8080, config.Port);
        }

        // ============================================================
        // CreateFromEnvironmentOnly
        // ============================================================

        [Fact]
        public void CreateFromEnvironmentOnly_ShouldReadEnvVarsOnly()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", "from-env");
                var config = OptionsBuilder.CreateFromEnvironmentOnly<StaticTestConfig>();
                Assert.Equal("from-env", config.Host);
                Assert.Equal(8080, config.Port);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", null);
            }
        }

        [Fact]
        public void CreateFromEnvironmentOnly_WhenNoEnvVarsSet_ShouldReturnDefaults()
        {
            Environment.SetEnvironmentVariable("GFX_STATIC_HOST", null);
            var config = OptionsBuilder.CreateFromEnvironmentOnly<StaticTestConfig>();
            Assert.Equal("localhost", config.Host);
        }

        // ============================================================
        // CreateDefault
        // ============================================================

        [Fact]
        public void CreateDefault_ShouldReturnOnlyDefaults()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", "should-be-ignored");
                var config = OptionsBuilder.CreateDefault<StaticTestConfig>();
                Assert.Equal("localhost", config.Host);
                Assert.Equal(8080, config.Port);
                Assert.False(config.Verbose);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_STATIC_HOST", null);
            }
        }

        // ============================================================
        // TryCreate
        // ============================================================

        [Fact]
        public void TryCreate_WithValidArgs_ShouldReturnTrueAndResult()
        {
            var args = new[] { "--host", "valid.example.com" };
            var ok = OptionsBuilder.TryCreate(args, out StaticTestConfig result, out string error);
            Assert.True(ok);
            Assert.NotNull(result);
            Assert.Equal("valid.example.com", result.Host);
            Assert.Null(error);
        }

        [Fact]
        public void TryCreate_WithInvalidArgs_ShouldReturnFalseWithDefaultAndError()
        {
            var args = new[] { "--port", "not-a-number" };
            var ok = OptionsBuilder.TryCreate(args, out StaticTestConfig result, out string error);
            Assert.False(ok);
            Assert.NotNull(result);
            Assert.Equal("localhost", result.Host);
            Assert.False(string.IsNullOrEmpty(error));
        }

        [Fact]
        public void TryCreate_WithNullArgs_ShouldReturnTrueWithDefaults()
        {
            var ok = OptionsBuilder.TryCreate(null, out StaticTestConfig result, out string error);
            Assert.True(ok);
            Assert.NotNull(result);
            Assert.Equal("localhost", result.Host);
            Assert.Null(error);
        }

        [Fact]
        public void TryCreate_WithMissingRequired_ShouldReturnFalseWithError()
        {
            var args = Array.Empty<string>();
            var ok = OptionsBuilder.TryCreate(args, out StaticRequiredConfig result, out string error);
            Assert.False(ok);
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(error));
        }

        // ============================================================
        // CreateWithDebug
        // ============================================================

        [Fact]
        public void CreateWithDebug_ShouldReturnOptionsAndPrintToConsole()
        {
            var originalOut = Console.Out;
            using (var writer = new StringWriter())
            {
                try
                {
                    Console.SetOut(writer);
                    var args = new[] { "--host", "debug.example.com" };
                    var config = OptionsBuilder.CreateWithDebug<StaticTestConfig>(args);
                    Assert.Equal("debug.example.com", config.Host);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = writer.ToString();
                // PrintParsedOptions shrinks the value column to fit the detected
                // console width in the test environment, so the host value gets
                // line-wrapped and won't appear as a contiguous substring. Verify
                // that the debug print actually ran (title) and rendered the config
                // table (property name "Host" appears in the rows).
                Assert.Contains("Command-line parameter", output, StringComparison.Ordinal);
                Assert.Contains("Host", output, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void CreateWithDebug_WithNullArgs_ShouldReturnDefaults()
        {
            var originalOut = Console.Out;
            using (var writer = new StringWriter())
            {
                try
                {
                    Console.SetOut(writer);
                    var config = OptionsBuilder.CreateWithDebug<StaticTestConfig>(null);
                    Assert.Equal("localhost", config.Host);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }

        // ============================================================
        // 测试辅助类
        // ============================================================

        private class StaticTestConfig
        {
            [EnvironmentVariable("GFX_STATIC_HOST")]
            public string Host { get; set; } = "localhost";

            public int Port { get; set; } = 8080;

            public bool Verbose { get; set; } = false;
        }

        private class StaticRequiredConfig
        {
            [Option("gfx-static-required-key", Required = true)]
            public string RequiredKey { get; set; }
        }
    }
}
