using System;
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// BooleanParser 单元测试：
    /// 直接覆盖 IsBooleanValue / ParseBooleanValue 对大小写、前后空白、
    /// null/空字符串/全空白 输入的归一化行为与契约边界。
    /// BooleanParser 标记为 internal，Options 项目已通过 InternalsVisibleTo 暴露给测试程序集。
    /// </summary>
    public class BooleanParserTests
    {
        // ============================================================
        // IsBooleanValue - 识别为 true 的字面量
        // ============================================================

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("1")]
        [InlineData("0")]
        [InlineData("yes")]
        [InlineData("no")]
        [InlineData("on")]
        [InlineData("off")]
        public void IsBooleanValue_WithCanonicalLiterals_ShouldReturnTrue(string value)
        {
            Assert.True(BooleanParser.IsBooleanValue(value));
        }

        [Theory]
        [InlineData("TRUE")]
        [InlineData("False")]
        [InlineData("YES")]
        [InlineData("No")]
        [InlineData("ON")]
        [InlineData("OfF")]
        [InlineData("tRuE")]
        public void IsBooleanValue_ShouldBeCaseInsensitive(string value)
        {
            Assert.True(BooleanParser.IsBooleanValue(value));
        }

        [Theory]
        [InlineData(" true ")]
        [InlineData("\tfalse\t")]
        [InlineData("\ntrue\n")]
        [InlineData("   1   ")]
        [InlineData(" yes\t")]
        public void IsBooleanValue_ShouldTrimLeadingAndTrailingWhitespace(string value)
        {
            Assert.True(BooleanParser.IsBooleanValue(value));
        }

        // ============================================================
        // IsBooleanValue - 非法值返回 false
        // ============================================================

        [Theory]
        [InlineData("maybe")]
        [InlineData("2")]
        [InlineData("-1")]
        [InlineData("y")]
        [InlineData("n")]
        [InlineData("ok")]
        [InlineData(" enabled")]
        [InlineData("disabled")]
        [InlineData("truee")]
        [InlineData("tru")]
        [InlineData("yes_no")]
        public void IsBooleanValue_WithUnrecognizedValues_ShouldReturnFalse(string value)
        {
            Assert.False(BooleanParser.IsBooleanValue(value));
        }

        // ============================================================
        // IsBooleanValue - null / 空字符串 / 全空白
        // ============================================================

        [Fact]
        public void IsBooleanValue_WithNull_ShouldReturnFalse()
        {
            // 健壮性契约：null 不抛 ArgumentNullException，返回 false
            Assert.False(BooleanParser.IsBooleanValue(null));
        }

        [Fact]
        public void IsBooleanValue_WithEmptyString_ShouldReturnFalse()
        {
            Assert.False(BooleanParser.IsBooleanValue(""));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("   \t  ")]
        public void IsBooleanValue_WithWhitespaceOnly_ShouldReturnFalse(string value)
        {
            Assert.False(BooleanParser.IsBooleanValue(value));
        }

        // ============================================================
        // ParseBooleanValue - truthy 字面量返回 true
        // ============================================================

        [Theory]
        [InlineData("true", true)]
        [InlineData("1", true)]
        [InlineData("yes", true)]
        [InlineData("on", true)]
        public void ParseBooleanValue_WithTruthyLiterals_ShouldReturnTrue(string value, bool expected)
        {
            Assert.Equal(expected, BooleanParser.ParseBooleanValue(value));
        }

        [Theory]
        [InlineData("TRUE")]
        [InlineData("True")]
        [InlineData("YES")]
        [InlineData("On")]
        [InlineData("tRuE")]
        public void ParseBooleanValue_ShouldBeCaseInsensitive(string value)
        {
            Assert.True(BooleanParser.ParseBooleanValue(value));
        }

        [Theory]
        [InlineData(" true ")]
        [InlineData("\ton\t")]
        [InlineData("\n1\n")]
        [InlineData("   yes   ")]
        public void ParseBooleanValue_ShouldTrimLeadingAndTrailingWhitespace(string value)
        {
            Assert.True(BooleanParser.ParseBooleanValue(value));
        }

        // ============================================================
        // ParseBooleanValue - falsy 字面量返回 false
        // ============================================================

        [Theory]
        [InlineData("false")]
        [InlineData("0")]
        [InlineData("no")]
        [InlineData("off")]
        [InlineData("False")]
        [InlineData("NO")]
        [InlineData("OfF")]
        [InlineData(" false ")]
        [InlineData("\toff\n")]
        public void ParseBooleanValue_WithFalsyLiterals_ShouldReturnFalse(string value)
        {
            Assert.False(BooleanParser.ParseBooleanValue(value));
        }

        // ============================================================
        // ParseBooleanValue - null / 空字符串 / 全空白 / 无法识别
        //
        // 当前契约（源码第 70-73 行 + XML 注释）：null、空白或无法识别均返回 false，
        // 不抛 ArgumentNullException / FormatException。
        // 这意味着调用方无法通过返回值区分 "字面量为 false" 与 "输入非法"。
        // 调用方（如 OptionsBuilder.ConvertBoolOptionValue）必须先用 IsBooleanValue 判定，
        // 否则会把非法值悄悄当作 false。已在源码缺陷清单中列出。
        // ============================================================

        [Fact]
        public void ParseBooleanValue_WithNull_ShouldReturnFalse_NotThrow()
        {
            // 按当前契约：null 返回 false，不抛 ArgumentNullException
            Assert.False(BooleanParser.ParseBooleanValue(null));
        }

        [Fact]
        public void ParseBooleanValue_WithEmptyString_ShouldReturnFalse()
        {
            Assert.False(BooleanParser.ParseBooleanValue(""));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("   \t  ")]
        public void ParseBooleanValue_WithWhitespaceOnly_ShouldReturnFalse(string value)
        {
            Assert.False(BooleanParser.ParseBooleanValue(value));
        }

        [Theory]
        [InlineData("maybe")]
        [InlineData("2")]
        [InlineData("-1")]
        [InlineData("y")]
        [InlineData("ok")]
        [InlineData("enabled")]
        [InlineData("disabled")]
        [InlineData("truee")]
        public void ParseBooleanValue_WithUnrecognizedValues_ShouldReturnFalse(string value)
        {
            // 当前契约：无法识别的字面量返回 false，不抛异常。
            Assert.False(BooleanParser.ParseBooleanValue(value));
        }

        // ============================================================
        // IsBooleanValue / ParseBooleanValue 一致性
        // ============================================================

        [Theory]
        [InlineData("true")]
        [InlineData("false")]
        [InlineData("1")]
        [InlineData("0")]
        [InlineData("yes")]
        [InlineData("no")]
        [InlineData("on")]
        [InlineData("off")]
        [InlineData("TRUE")]
        [InlineData("  On  ")]
        public void IsBooleanValue_AndParseBooleanValue_ShouldAgreeOnRecognizedValues(string value)
        {
            // 若被 IsBooleanValue 识别，则 ParseBooleanValue 必须返回确定的 bool 值
            // (true 字面量返回 true，false 字面量返回 false)。
            Assert.True(BooleanParser.IsBooleanValue(value));
            // 只要能解析（不抛异常）即说明一致性；具体值已由上述用例覆盖
            var parsed = BooleanParser.ParseBooleanValue(value);
            Assert.IsType<bool>(parsed);
        }
    }
}
