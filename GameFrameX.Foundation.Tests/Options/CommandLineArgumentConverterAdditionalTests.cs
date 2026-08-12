using System;
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// CommandLineArgumentConverter 补充单元测试：
    /// ConvertToStandardFormat 对数组含 null 元素、空字符串元素、
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
        // 数组含 null 元素
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithOnlyNull_ShouldReturnEmptyList()
        {
            var result = _converter.ConvertToStandardFormat(new string[] { null });
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithLeadingNull_ShouldSkipNullAndContinue()
        {
            // [null, "--port", "9090"] → 期望 ["--port", "9090"]
            var args = new string[] { null, "--port", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--port", "9090" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithTrailingNull_ShouldSkipTrailingNull()
        {
            // ["--port", "9090", null] → 期望 ["--port", "9090"]
            var args = new string[] { "--port", "9090", null };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--port", "9090" }, result);
        }

        [Fact(Skip = "已知缺陷:args 中 key 与 value 之间出现 null 时值归属错误,修复需重构 AttachValueIfPresent 索引逻辑,待设计决策")]
        public void ConvertToStandardFormat_WithNullBetweenKeyAndValue_ShouldKeepValueAssociated()
        {
            // 期望行为：null 被跳过，"9090" 继续作为 --port 的值。
            //
            // 当前实现（缺陷）：
            //   ConvertToStandardFormat 第 188-191 行跳过 null，
            //   AttachValueIfPresent 第 317-320 行看到 nextArg==null 时返回 currentIndex+1，
            //   导致 for 循环索引跳过 null，下一个非 null token "9090" 被当作新 token 处理，
            //   被 NormalizeKey 加上 "--" 前缀变成 "--9090"。
            //   最终结果为 ["--port", "--9090"]，这破坏了 key-value 关联。
            //
            // 期望行为：null 不应破坏后续 token 作为前一个 key 的值的可能性。
            var args = new string[] { "--port", null, "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            // 期望 9090 仍作为 --port 的值（而非被当作新 key）
            Assert.Contains("--port", result);
            Assert.Contains("9090", result);
            Assert.DoesNotContain("--9090", result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithMultipleNulls_ShouldSkipAll()
        {
            // [null, null, "--port", "9090", null] → 期望 ["--port", "9090"]
            var args = new string[] { null, null, "--port", "9090", null };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--port", "9090" }, result);
        }

        [Fact]
        public void ConvertToStandardFormat_WithAllNulls_ShouldReturnEmptyList()
        {
            var args = new string[] { null, null, null };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Empty(result);
        }

        // ============================================================
        // 空字符串元素
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

        // ============================================================
        // null + 空字符串组合
        // ============================================================

        [Fact]
        public void ConvertToStandardFormat_WithNullAndEmptyStringMix_ShouldSkipBoth()
        {
            // [null, "", "--port", "9090"] → 期望 ["--port", "9090"]
            var args = new string[] { null, "", "--port", "9090" };

            var result = _converter.ConvertToStandardFormat(args);

            Assert.Equal(new[] { "--port", "9090" }, result);
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
        // 异常包装
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
