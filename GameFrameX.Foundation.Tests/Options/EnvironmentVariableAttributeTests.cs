using System;
using System.Reflection;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// EnvironmentVariableAttribute 单元测试：
    /// 构造、Name 属性只读性、AttributeUsage 元数据、反射可应用性，以及与 OptionsBuilder 的集成。
    /// </summary>
    public class EnvironmentVariableAttributeTests
    {
        // ============================================================
        // 构造与 Name 属性
        // ============================================================

        [Fact]
        public void Constructor_WithValidName_ShouldStoreName()
        {
            var attr = new EnvironmentVariableAttribute("DATABASE_URL");
            Assert.Equal("DATABASE_URL", attr.Name);
        }

        [Theory]
        [InlineData("API_KEY")]
        [InlineData("MY_VAR_123")]
        [InlineData("gameframex_db")]
        [InlineData("connection-string")]
        public void Constructor_WithVariousNames_ShouldStoreName(string name)
        {
            var attr = new EnvironmentVariableAttribute(name);
            Assert.Equal(name, attr.Name);
        }

        [Fact]
        public void Constructor_WithNullName_ShouldStoreNull()
        {
            var attr = new EnvironmentVariableAttribute(null);
            Assert.Null(attr.Name);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldStoreEmptyString()
        {
            var attr = new EnvironmentVariableAttribute("");
            Assert.Equal("", attr.Name);
        }

        [Fact]
        public void Name_PropertyShouldHaveNoSetter()
        {
            var setter = typeof(EnvironmentVariableAttribute)
                .GetProperty("Name")
                .GetSetMethod();
            Assert.Null(setter);
        }

        // ============================================================
        // AttributeUsage 元数据
        // ============================================================

        [Fact]
        public void AttributeUsage_ShouldTargetPropertyOnly()
        {
            var usage = typeof(EnvironmentVariableAttribute)
                .GetCustomAttribute<AttributeUsageAttribute>();
            Assert.NotNull(usage);
            Assert.Equal(AttributeTargets.Property, usage.ValidOn);
            Assert.False(usage.AllowMultiple);
            Assert.True(usage.Inherited);
        }

        // ============================================================
        // 反射可应用性
        // ============================================================

        [Fact]
        public void Attribute_ShouldBeRetrievableViaReflection()
        {
            var property = typeof(EnvVarAnnotatedConfig)
                .GetProperty(nameof(EnvVarAnnotatedConfig.DatabaseUrl));
            var attr = property.GetCustomAttribute<EnvironmentVariableAttribute>();
            Assert.NotNull(attr);
            Assert.Equal("GFX_ENVVAR_TEST_DB", attr.Name);
        }

        [Fact]
        public void Attribute_CanBeAppliedToMultiplePropertiesOnSameClass()
        {
            var type = typeof(EnvVarAnnotatedConfig);
            var dbProp = type.GetProperty(nameof(EnvVarAnnotatedConfig.DatabaseUrl));
            var portProp = type.GetProperty(nameof(EnvVarAnnotatedConfig.Port));

            Assert.NotNull(dbProp.GetCustomAttribute<EnvironmentVariableAttribute>());
            Assert.NotNull(portProp.GetCustomAttribute<EnvironmentVariableAttribute>());
        }

        // ============================================================
        // 与 OptionsBuilder 集成：环境变量读取
        // ============================================================

        [Fact]
        public void OptionsBuilder_ShouldReadStringValueFromEnvironmentVariable()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", "Server=test;Db=mydb");
                var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(Array.Empty<string>());
                var config = builder.Build(skipValidation: true);
                Assert.Equal("Server=test;Db=mydb", config.DatabaseUrl);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", null);
            }
        }

        [Fact]
        public void OptionsBuilder_WhenEnvVarAbsent_ShouldKeepDefaultNull()
        {
            Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", null);
            var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(Array.Empty<string>());
            var config = builder.Build(skipValidation: true);
            Assert.Null(config.DatabaseUrl);
        }

        [Fact]
        public void OptionsBuilder_ShouldConvertIntEnvVar()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_PORT", "9527");
                var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(Array.Empty<string>());
                var config = builder.Build(skipValidation: true);
                Assert.Equal(9527, config.Port);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_PORT", null);
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
        public void OptionsBuilder_ShouldConvertBoolEnvVar(string rawValue, bool expected)
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DEBUG", rawValue);
                var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(Array.Empty<string>());
                var config = builder.Build(skipValidation: true);
                Assert.Equal(expected, config.Debug);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DEBUG", null);
            }
        }

        [Fact]
        public void OptionsBuilder_WhenEnvVarValueEmpty_ShouldKeepDefault()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", "");
                var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(Array.Empty<string>());
                var config = builder.Build(skipValidation: true);
                Assert.Null(config.DatabaseUrl);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", null);
            }
        }

        [Fact]
        public void OptionsBuilder_CommandLineArgShouldOverrideEnvVar()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", "from-env");
                var args = new[] { "--databaseUrl", "from-args" };
                var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(args);
                var config = builder.Build(skipValidation: true);
                Assert.Equal("from-args", config.DatabaseUrl);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_TEST_DB", null);
            }
        }

        [Fact]
        public void OptionsBuilder_UnmappedEnvVarName_ShouldNotAffectProperty()
        {
            try
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_UNMAPPED", "should-be-ignored");
                var builder = new OptionsBuilder<EnvVarAnnotatedConfig>(Array.Empty<string>());
                var config = builder.Build(skipValidation: true);
                Assert.Null(config.DatabaseUrl);
            }
            finally
            {
                Environment.SetEnvironmentVariable("GFX_ENVVAR_UNMAPPED", null);
            }
        }

        // ============================================================
        // 测试辅助类
        // ============================================================

        private class EnvVarAnnotatedConfig
        {
            [EnvironmentVariable("GFX_ENVVAR_TEST_DB")]
            public string DatabaseUrl { get; set; }

            [EnvironmentVariable("GFX_ENVVAR_TEST_PORT")]
            public int Port { get; set; }

            [EnvironmentVariable("GFX_ENVVAR_TEST_DEBUG")]
            public bool Debug { get; set; }
        }
    }
}
