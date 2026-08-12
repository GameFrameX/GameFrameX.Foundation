using System;
using System.Reflection;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// FlagOptionAttribute 单元测试：
    /// 构造与默认值、继承关系、AttributeUsage 元数据、反射可应用性，
    /// 以及与 OptionsBuilder 的布尔标志解析集成（存在即 true / --flag=value / 分离格式 / 各 BoolFormat）。
    /// </summary>
    public class FlagOptionAttributeTests
    {
        // ============================================================
        // 构造与默认值
        // ============================================================

        [Fact]
        public void DefaultConstructor_ShouldSetDefaultValueToFalse()
        {
            var attr = new FlagOptionAttribute();
            Assert.Equal(false, attr.DefaultValue);
        }

        [Fact]
        public void ConstructorWithLongName_ShouldSetLongNameAndDefaultValueToFalse()
        {
            var attr = new FlagOptionAttribute("verbose");
            Assert.Equal("verbose", attr.LongName);
            Assert.Equal(false, attr.DefaultValue);
        }

        [Fact]
        public void ConstructorWithNullLongName_ShouldSetNullLongNameAndDefaultValueToFalse()
        {
            var attr = new FlagOptionAttribute(null);
            Assert.Null(attr.LongName);
            Assert.Equal(false, attr.DefaultValue);
        }

        [Fact]
        public void ConstructorWithEmptyLongName_ShouldSetEmptyLongName()
        {
            var attr = new FlagOptionAttribute("");
            Assert.Equal("", attr.LongName);
            Assert.Equal(false, attr.DefaultValue);
        }

        // ============================================================
        // 继承关系
        // ============================================================

        [Fact]
        public void FlagOptionAttribute_ShouldInheritFromOptionAttribute()
        {
            var attr = new FlagOptionAttribute();
            Assert.IsAssignableFrom<OptionAttribute>(attr);
        }

        [Fact]
        public void FlagOptionAttribute_ShouldAccessInheritedProperties()
        {
            var attr = new FlagOptionAttribute("debug")
            {
                Required = true,
                Description = "Enable debug mode",
                EnvironmentVariable = "DEBUG_FLAG",
                Sensitive = false
            };
            Assert.True(attr.Required);
            Assert.Equal("Enable debug mode", attr.Description);
            Assert.Equal("DEBUG_FLAG", attr.EnvironmentVariable);
            Assert.False(attr.Sensitive);
            Assert.Equal(false, attr.DefaultValue);
        }

        // ============================================================
        // AttributeUsage 元数据
        // ============================================================

        [Fact]
        public void AttributeUsage_ShouldTargetPropertyOnly()
        {
            var usage = typeof(FlagOptionAttribute)
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
        public void Attribute_WithLongName_ShouldBeRetrievableViaReflection()
        {
            var property = typeof(FlagAnnotatedConfig)
                .GetProperty(nameof(FlagAnnotatedConfig.Verbose));
            var attr = property.GetCustomAttribute<FlagOptionAttribute>();
            Assert.NotNull(attr);
            Assert.Equal("verbose", attr.LongName);
        }

        [Fact]
        public void Attribute_ParameterlessConstructor_ShouldHaveNullLongName()
        {
            var property = typeof(FlagAnnotatedConfig)
                .GetProperty(nameof(FlagAnnotatedConfig.DebugMode));
            var attr = property.GetCustomAttribute<FlagOptionAttribute>();
            Assert.NotNull(attr);
            Assert.Null(attr.LongName);
        }

        [Fact]
        public void Attribute_ShouldAlsoBeRetrievableAsOptionAttribute()
        {
            var property = typeof(FlagAnnotatedConfig)
                .GetProperty(nameof(FlagAnnotatedConfig.Verbose));
            // FlagOptionAttribute inherits from OptionAttribute
            var optionAttr = property.GetCustomAttribute<OptionAttribute>();
            Assert.NotNull(optionAttr);
            Assert.Equal("verbose", optionAttr.LongName);
        }

        // ============================================================
        // OptionsBuilder 集成：布尔标志解析
        // ============================================================

        [Fact]
        public void OptionsBuilder_FlagPresent_ShouldBeTrue()
        {
            var args = new[] { "--verbose" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.Verbose);
        }

        [Fact]
        public void OptionsBuilder_FlagAbsent_ShouldBeFalse()
        {
            var args = Array.Empty<string>();
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.False(config.Verbose);
        }

        // --- 键值对格式 (--flag=value) ---

        [Theory]
        [InlineData("true", true)]
        [InlineData("True", true)]
        [InlineData("1", true)]
        [InlineData("yes", true)]
        [InlineData("on", true)]
        [InlineData("false", false)]
        [InlineData("False", false)]
        [InlineData("0", false)]
        [InlineData("no", false)]
        [InlineData("off", false)]
        public void OptionsBuilder_FlagWithKeyValue_ShouldParseCorrectly(string value, bool expected)
        {
            var args = new[] { "--verbose=" + value };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.Equal(expected, config.Verbose);
        }

        [Fact]
        public void OptionsBuilder_FlagWithInvalidKeyValue_ShouldThrow()
        {
            var args = new[] { "--verbose=maybe" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            Assert.Throws<ArgumentException>(() => builder.Build(skipValidation: true));
        }

        // --- Flag 格式行为：--flag 后跟布尔字面量时，字面量被跳过，flag 存在即 true ---

        [Fact]
        public void OptionsBuilder_FlagMode_SeparatedTrue_ShouldBeTrue()
        {
            // Flag 模式下 converter 跳过 "true"，flag 存在 → true
            var args = new[] { "--verbose", "true" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.Verbose);
        }

        [Fact]
        public void OptionsBuilder_FlagMode_SeparatedFalse_ShouldStillBeTrue()
        {
            // Flag 模式下 converter 跳过 "false"，flag 存在 → true
            var args = new[] { "--verbose", "false" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.Verbose);
        }

        // --- Separated 格式行为：--flag true/false 被当作值解析 ---

        [Fact]
        public void OptionsBuilder_SeparatedMode_FlagTrue_ShouldBeTrue()
        {
            var args = new[] { "--verbose", "true" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args, BoolArgumentFormat.Separated);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.Verbose);
        }

        [Fact]
        public void OptionsBuilder_SeparatedMode_FlagFalse_ShouldBeFalse()
        {
            var args = new[] { "--verbose", "false" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args, BoolArgumentFormat.Separated);
            var config = builder.Build(skipValidation: true);
            Assert.False(config.Verbose);
        }

        // --- 参数化构造的 FlagOption（无 LongName）按属性名匹配 ---

        [Fact]
        public void OptionsBuilder_ParameterlessFlag_MatchedByPropertyName()
        {
            var args = new[] { "--debugMode" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.DebugMode);
        }

        [Fact]
        public void OptionsBuilder_ParameterlessFlag_MatchedByHyphenatedName()
        {
            var args = new[] { "--debug-mode" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.DebugMode);
        }

        [Fact]
        public void OptionsBuilder_ParameterlessFlag_Absent_ShouldBeFalse()
        {
            var args = new[] { "--verbose" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.False(config.DebugMode);
        }

        // --- 多标志共存 ---

        [Fact]
        public void OptionsBuilder_MultipleFlags_BothPresent_ShouldBothBeTrue()
        {
            var args = new[] { "--verbose", "--debugMode" };
            var builder = new OptionsBuilder<FlagAnnotatedConfig>(args);
            var config = builder.Build(skipValidation: true);
            Assert.True(config.Verbose);
            Assert.True(config.DebugMode);
        }

        // ============================================================
        // 测试辅助类
        // ============================================================

        private class FlagAnnotatedConfig
        {
            [FlagOption("verbose")]
            public bool Verbose { get; set; }

            [FlagOption]
            public bool DebugMode { get; set; }
        }
    }
}
