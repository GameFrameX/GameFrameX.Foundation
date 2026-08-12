using System;
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// CommandLineArgumentConverter 补充单元测试：
    /// ConvertToStandardFormat 对数组含 null 元素（应抛 ArgumentException）、空字符串元素、
    /// 仅 "--" 分隔符、null 与空字符串组合等边界的健壮性。
    /// </summary>
    public class CommandLineArgumentConverterAdditionalTests
    {
        private readonly CommandLineArgumentConverter _converter;

        public CommandLineArgumentConverterAdditionalTests()
        {
            _converter = new CommandLineArgumentConverter();
        }

        // ============================================================
        // 数组含 null 元素 —— 必须抛 ArgumentException（防御性：Main 的 args 不会有 null）
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithOnlyNull_ShouldThrowArgumentException()
        {
            // Arrange
            var args = new string[] { null };

            // Act & Assert — null 元素是非法输入，直接拒绝
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        [Fact]
        public void ConvertToStandardFormat_WithLeadingNull_ShouldThrowArgumentException()
        {
            // Arrange
            var args = new string[] { null, "--port", "9090" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        [Fact]
        public void ConvertToStandardFormat_WithTrailingNull_ShouldThrowArgumentException()
        {
            // Arrange
            var args = new string[] { "--port", "9090", null };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        [Fact]
        public void ConvertToStandardFormat_WithNullBetweenKeyAndValue_ShouldThrowArgumentException()
        {
            // Arrange — key 与 value 之间出现 null 同样必须拒绝（避免值归属错误）
            var args = new string[] { "--port", null, "9090" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        [Fact]
        public void ConvertToStandardFormat_WithMultipleNulls_ShouldThrowArgumentException()
        {
            // Arrange
            var args = new string[] { null, null, "--port", "9090", null };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        [Fact]
        public void ConvertToStandardFormat_WithAllNulls_ShouldThrowArgumentException()
        {
            // Arrange
            var args = new string[] { null, null, null };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        // ============================================================
        // 空字符串元素（合法，保留）
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithEmptyStringAsValue_ShouldAppendEmptyStringAfterKey()
        {
            // ["--host", ""] → 期望 ["--host", ""]（空字符串作为 --host 的值）
            var args = new[] { "--host", "" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--host", "" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithLeadingEmptyString_ShouldDropIt()
        {
            // ["", "--port", "9090"] → 空字符串在首位（前面没有 key），应被丢弃
            var args = new[] { "", "--port", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--port", "9090" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithEmptyStringBetweenKeys_ShouldDropIt()
        {
            // ["--host", "value", "", "--port", "9090"]
            // --host 取 value 作为值，"" 前一个是 "value"（非 key），应被丢弃
            var args = new[] { "--host", "value", "", "--port", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--host", "value", "--port", "9090" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithEmptyStringAsValueFollowedByKey_ShouldKeepEmptyAndContinue()
        {
            // ["--host", "", "--port", "9090"]
            // --host 取空串作为值，--port 取 9090 作为值
            var args = new[] { "--host", "", "--port", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--host", "", "--port", "9090" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithOnlyEmptyStrings_ShouldReturnEmptyList()
        {
            var args = new[] { "", "", "" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Empty(result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithNullAndEmptyStringMix_ShouldThrowArgumentException()
        {
            // Arrange — 含 null 元素即拒绝（空字符串本身合法，但 null 不允许）
            var args = new string[] { null, "", "--port", "9090" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _converter.ConvertToStandardFormat(args));
        }

        // ============================================================
        // 仅 "--" 分隔符
        //
        // 说明：POSIX 约定中 "--" 表示后续参数均为位置参数，但当前实现将其
        // 视为普通 option token（IsOptionToken 返回 true），不做特殊处理。
        // 这里按当前实现断言其行为，供未来如引入 POSIX 分隔语义时回归参考。
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithOnlyDoubleDash_ShouldReturnDoubleDash()
        {
            var args = new[] { "--" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithDoubleDashBeforeKey_ShouldKeepBoth()
        {
            // "--" 不被识别为值分隔符，被当作普通 token 处理
            var args = new[] { "--", "--port", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Contains("--", result);
            Assert.Contains("--port", result);
            Assert.Contains("9090", result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithDoubleDashBetweenKeyAndValue_ShouldKeepDoubleDashAsToken()
        {
            // ["--port", "--", "9090"] → "--" 是 option token，不被当作 --port 的值
            var args = new[] { "--port", "--", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            // --port 后跟 option token "--"，不消耗；然后 "--" 后跟 "9090" 作为值
            Assert.Contains("--port", result);
            Assert.Contains("--", result);
            Assert.Contains("9090", result);
        }

        // ============================================================
        // 含 = 的值：第一个 = 之后原样保留
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithEqualsSeparatedValueContainingEquals_ShouldPreserveValue()
        {
            // --key=a=b → key 保持，value=a=b（按第一个 = 分割）
            var args = new[] { "--key=a=b" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Contains("--key=a=b", result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithEqualsSeparatedEmptyValue_ShouldKeepEmptyValue()
        {
            // --key= → key=key, value=""
            var args = new[] { "--key=" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Contains("--key=", result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithKeyWithoutPrefixAndEquals_ShouldAddPrefix()
        {
            // EnsurePrefixedKeys=true，key=xxx=value → --key=value
            _converter.EnsurePrefixedKeys = true;
            var args = new[] { "key=value" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Contains("--key=value", result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithKeyWithoutPrefixAndEquals_DisabledPrefixed_ShouldNotAddPrefix()
        {
            _converter.EnsurePrefixedKeys = false;
            var args = new[] { "key=value" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Contains("key=value", result);
            Assert.DoesNotContain("--key=value", result);
        }

        // ============================================================
        // 整个数组 null / 空数组（合法，返回空列表）
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithNullArgs_ShouldReturnEmptyList()
        {
            var result = _converter.ConvertToStandardFormat(null);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithEmptyArray_ShouldReturnEmptyList()
        {
            var result = _converter.ConvertToStandardFormat(Array.Empty<string>());
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
