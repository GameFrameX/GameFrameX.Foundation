using System;
using System.IO;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// OptionsProvider 补充单元测试：
    /// 缓存行为（命中 / ClearCache / RemoveFromCache / Initialize 清缓存）、
    /// ParseWithDebug / ParseSilent / PrintOptionsInfo、
    /// SetGlobalDebugMode / IsDebugModeEnabled（GAMEFRAMEX_OPTIONS_DEBUG 环境变量多值、运行时环境推断、优先级）。
    /// </summary>
    [Collection(OptionsProviderCollection.Name)]
    public class OptionsProviderAdditionalTests
    {
        // ============================================================
        // 环境变量保存/恢复辅助
        // ============================================================

        private static string[] CaptureDebugEnvVars()
        {
            return new[]
            {
                Environment.GetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG"),
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT"),
                Environment.GetEnvironmentVariable("ENVIRONMENT")
            };
        }

        private static void RestoreDebugEnvVars(string[] captured)
        {
            Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", captured[0]);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", captured[1]);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", captured[2]);
            Environment.SetEnvironmentVariable("ENVIRONMENT", captured[3]);
        }

        private static void ClearDebugEnvVars()
        {
            Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", null);
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", null);
            Environment.SetEnvironmentVariable("ENVIRONMENT", null);
        }

        // ============================================================
        // 缓存行为
        // ============================================================

        [Fact]
        public void GetOptions_CalledTwice_ShouldReturnSameCachedInstance()
        {
            OptionsProvider.Initialize(new[] { "--host", "cached.example.com" });
            OptionsProvider.ClearCache();

            var first = OptionsProvider.GetOptions<ProviderCacheConfig>(enableDebugOutput: false);
            var second = OptionsProvider.GetOptions<ProviderCacheConfig>(enableDebugOutput: false);

            Assert.Same(first, second);
        }

        [Fact]
        public void ClearCache_ShouldForceRebuildOnNextCall()
        {
            OptionsProvider.Initialize(new[] { "--host", "original.example.com" });
            OptionsProvider.ClearCache();

            var first = OptionsProvider.GetOptions<ProviderCacheConfig>(enableDebugOutput: false);
            OptionsProvider.ClearCache();
            var second = OptionsProvider.GetOptions<ProviderCacheConfig>(enableDebugOutput: false);

            Assert.NotSame(first, second);
            Assert.Equal("original.example.com", first.Host);
            Assert.Equal("original.example.com", second.Host);
        }

        [Fact]
        public void Initialize_ShouldClearExistingCacheEntries()
        {
            OptionsProvider.Initialize(new[] { "--host", "first.example.com" });
            OptionsProvider.ClearCache();
            var first = OptionsProvider.GetOptions<ProviderCacheConfig>(enableDebugOutput: false);

            OptionsProvider.Initialize(new[] { "--host", "second.example.com" });
            var second = OptionsProvider.GetOptions<ProviderCacheConfig>(enableDebugOutput: false);

            Assert.NotSame(first, second);
            Assert.Equal("second.example.com", second.Host);
        }

        [Fact]
        public void RemoveFromCache_ShouldRemoveOnlySpecifiedType()
        {
            OptionsProvider.Initialize(Array.Empty<string>());
            OptionsProvider.ClearCache();

            var configA = OptionsProvider.GetOptions<ProviderCacheConfigA>(enableDebugOutput: false);
            var configB = OptionsProvider.GetOptions<ProviderCacheConfigB>(enableDebugOutput: false);

            OptionsProvider.RemoveFromCache<ProviderCacheConfigA>();

            var newConfigA = OptionsProvider.GetOptions<ProviderCacheConfigA>(enableDebugOutput: false);
            var sameConfigB = OptionsProvider.GetOptions<ProviderCacheConfigB>(enableDebugOutput: false);

            Assert.NotSame(configA, newConfigA);
            Assert.Same(configB, sameConfigB);
        }

        // ============================================================
        // ParseWithDebug / ParseSilent
        // ============================================================

        [Fact]
        public void ParseWithDebug_ShouldReturnParsedOptionsAndForceDebugOutput()
        {
            var originalOut = Console.Out;
            using (var writer = new StringWriter())
            {
                try
                {
                    Console.SetOut(writer);
                    var args = new[] { "--host", "parsedbg.example.com" };
                    var config = OptionsProvider.ParseWithDebug<ProviderCacheConfig>(args);
                    Assert.Equal("parsedbg.example.com", config.Host);
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
        public void ParseSilent_ShouldReturnParsedOptionsWithoutDebugOutput()
        {
            var originalOut = Console.Out;
            using (var writer = new StringWriter())
            {
                try
                {
                    Console.SetOut(writer);
                    var args = new[] { "--host", "silent.example.com" };
                    var config = OptionsProvider.ParseSilent<ProviderCacheConfig>(args);
                    Assert.Equal("silent.example.com", config.Host);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                var output = writer.ToString();
                Assert.DoesNotContain("Command-line parameter", output, StringComparison.Ordinal);
            }
        }

        // ============================================================
        // PrintOptionsInfo
        // ============================================================

        [Fact]
        public void PrintOptionsInfo_WithNull_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                OptionsProvider.PrintOptionsInfo<ProviderCacheConfig>(null));
        }

        [Fact]
        public void PrintOptionsInfo_WithValidOptions_ShouldPrintToConsole()
        {
            var config = new ProviderCacheConfig
            {
                Host = "print.example.com",
                Port = 1234
            };

            var originalOut = Console.Out;
            using (var writer = new StringWriter())
            {
                try
                {
                    Console.SetOut(writer);
                    OptionsProvider.PrintOptionsInfo(config);
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

        // ============================================================
        // SetGlobalDebugMode / IsDebugModeEnabled
        // ============================================================

        [Fact]
        public void SetGlobalDebugMode_TrueThenFalse_ShouldToggle()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();

                OptionsProvider.SetGlobalDebugMode(true);
                Assert.True(OptionsProvider.IsDebugModeEnabled());

                OptionsProvider.SetGlobalDebugMode(false);
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        // --- GAMEFRAMEX_OPTIONS_DEBUG 环境变量解析 ---

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("TRUE", true)]
        [InlineData("1", true)]
        [InlineData("yes", true)]
        [InlineData("on", true)]
        [InlineData("enable", true)]
        [InlineData("enabled", true)]
        [InlineData("0", false)]
        [InlineData("no", false)]
        [InlineData("off", false)]
        [InlineData("disable", false)]
        [InlineData("disabled", false)]
        public void IsDebugModeEnabled_WithOptionsDebugEnvVar_ShouldReturnExpected(string value, bool expected)
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", value);
                Assert.Equal(expected, OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        [Fact]
        public void IsDebugModeEnabled_WithInvalidOptionsDebugValue_ShouldFallThroughToFalse()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", "maybe");
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        [Fact]
        public void IsDebugModeEnabled_WithEmptyOptionsDebugValue_ShouldFallThroughToFalse()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", "");
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        // --- 运行时环境推断 ---

        [Theory]
        [InlineData("ASPNETCORE_ENVIRONMENT", "Development", true)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "dev", true)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "test", true)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "testing", true)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "debug", true)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "Production", false)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "prod", false)]
        [InlineData("ASPNETCORE_ENVIRONMENT", "release", false)]
        [InlineData("DOTNET_ENVIRONMENT", "Development", true)]
        [InlineData("DOTNET_ENVIRONMENT", "test", true)]
        [InlineData("ENVIRONMENT", "debug", true)]
        [InlineData("ENVIRONMENT", "production", false)]
        public void IsDebugModeEnabled_WithRuntimeEnvironment_ShouldReturnExpected(
            string envVarName, string envVarValue, bool expected)
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable(envVarName, envVarValue);
                Assert.Equal(expected, OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        [Fact]
        public void IsDebugModeEnabled_WithUnknownEnvironment_ShouldReturnFalse()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "staging");
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        [Fact]
        public void IsDebugModeEnabled_AspnetCoreTakesPriorityOverDotnetAndEnvironment()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
                // ASPNETCORE_ENVIRONMENT is checked first → Production → false
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        [Fact]
        public void IsDebugModeEnabled_DotnetUsedWhenAspnetCoreAbsent()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                // ASPNETCORE_ENVIRONMENT not set, DOTNET_ENVIRONMENT = Development → true
                Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
                Assert.True(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        // --- 优先级：GAMEFRAMEX_OPTIONS_DEBUG > 运行时环境 ---

        [Fact]
        public void IsDebugModeEnabled_OptionsDebugVarTakesPriorityOverRuntimeEnv()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", "false");
                // Options debug var says false, overriding Development → true
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        [Fact]
        public void IsDebugModeEnabled_NoEnvVarsSet_ShouldReturnFalse()
        {
            var captured = CaptureDebugEnvVars();
            try
            {
                ClearDebugEnvVars();
                Assert.False(OptionsProvider.IsDebugModeEnabled());
            }
            finally
            {
                RestoreDebugEnvVars(captured);
            }
        }

        // ============================================================
        // GetOptions 边界
        // ============================================================

        [Fact]
        public void GetOptions_WithSkipValidation_ShouldNotThrowOnMissingRequired()
        {
            OptionsProvider.Initialize(Array.Empty<string>());
            OptionsProvider.ClearCache();

            var config = OptionsProvider.GetOptions<ProviderRequiredConfig>(
                skipValidation: true, enableDebugOutput: false);
            Assert.Null(config.GfxToken);
        }

        [Fact]
        public void GetOptions_WithoutSkipValidation_MissingRequired_ShouldThrow()
        {
            OptionsProvider.Initialize(Array.Empty<string>());
            OptionsProvider.ClearCache();

            Assert.Throws<ArgumentException>(() =>
                OptionsProvider.GetOptions<ProviderRequiredConfig>(enableDebugOutput: false));
        }

        // ============================================================
        // 测试辅助类
        // ============================================================

        public class ProviderCacheConfig
        {
            public string Host { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public bool Debug { get; set; } = false;
        }

        public class ProviderCacheConfigA
        {
            public string Host { get; set; } = "localhost";
        }

        public class ProviderCacheConfigB
        {
            public int Port { get; set; } = 8080;
        }

        public class ProviderRequiredConfig
        {
            [Option("gfx-provider-token", Required = true)]
            public string GfxToken { get; set; }
        }
    }
}
