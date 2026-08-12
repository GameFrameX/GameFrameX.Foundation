using System;
using System.IO;
using GameFrameX.Foundation.Logger;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger
{
    /// <summary>
    /// LogHelper 控制台显示方法（SetFrameLength / ShowMaxTitle / ShowOption / ShowLineTitle）
    /// 和 LogHelper.Console 方法的覆盖测试。
    /// 覆盖源文件：LogHelper.Console.cs、LogConsole.cs、LogHelper.cs（Console 方法）。
    /// </summary>
    /// <remarks>
    /// LogConsole 为 internal sealed，外部测试无法直接构造。
    /// 通过 LogHelper 暴露的 public 方法间接验证行为（参数校验、输出格式、异常路径）。
    /// </remarks>
    [Collection("LogHelperSerialCollection")]
    public sealed class LogConsolePublicApiTests : IDisposable
    {
        public LogConsolePublicApiTests()
        {
            // 每个测试前重置框架宽度为默认值，避免上一个测试修改后影响当前结果。
            LogHelper.SetFrameLength(108);
        }

        public void Dispose()
        {
            // 恢复默认宽度。
            try
            {
                LogHelper.SetFrameLength(108);
            }
            catch (ArgumentOutOfRangeException)
            {
                // 忽略，确保 Dispose 不抛异常。
            }

            GC.SuppressFinalize(this);
        }

        // ============================================================
        // SetFrameLength
        // ============================================================

        [Fact]
        public void SetFrameLength_WithPositiveValue_ShouldNotThrow()
        {
            LogHelper.SetFrameLength(80);
            // 不抛异常即为通过。
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void SetFrameLength_WithZeroOrNegative_ShouldThrowArgumentOutOfRangeException(int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => LogHelper.SetFrameLength(length));
        }

        [Fact]
        public void SetFrameLength_WithLargeValue_ShouldNotThrow()
        {
            LogHelper.SetFrameLength(500);
        }

        // ============================================================
        // ShowMaxTitle
        // ============================================================

        [Fact]
        public void ShowMaxTitle_WithNullTitle_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.ShowMaxTitle(null!));
        }

        [Fact]
        public void ShowMaxTitle_WithNullTitle2_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.ShowMaxTitle("main", null!));
        }

        [Fact]
        public void ShowMaxTitle_WithNullTitle3_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.ShowMaxTitle("main", "sub", null!));
        }

        [Fact]
        public void ShowMaxTitle_WithSingleTitle_ShouldOutputTopAndBottomBorders()
        {
            var output = CaptureConsole(() => LogHelper.ShowMaxTitle("Hello"));

            Assert.Contains("Hello", output, StringComparison.Ordinal);
            // 顶部双线边框
            Assert.Contains("╔", output, StringComparison.Ordinal);
            Assert.Contains("╗", output, StringComparison.Ordinal);
            // 底部双线边框
            Assert.Contains("╚", output, StringComparison.Ordinal);
            Assert.Contains("╝", output, StringComparison.Ordinal);
            // 左右边框
            Assert.Contains("║", output, StringComparison.Ordinal);
        }

        [Fact]
        public void ShowMaxTitle_WithAllThreeTitles_ShouldOutputAllTitles()
        {
            var output = CaptureConsole(() => LogHelper.ShowMaxTitle("Main", "Sub1", "Sub2"));

            Assert.Contains("Main", output, StringComparison.Ordinal);
            Assert.Contains("Sub1", output, StringComparison.Ordinal);
            Assert.Contains("Sub2", output, StringComparison.Ordinal);
        }

        [Fact]
        public void ShowMaxTitle_WithEmptySubtitles_ShouldOnlyShowMainTitle()
        {
            var output = CaptureConsole(() => LogHelper.ShowMaxTitle("Only", "", ""));

            Assert.Contains("Only", output, StringComparison.Ordinal);
            // 空副标题不应该出现额外的标题行。
            // 主标题占一行，该行有左右两个 ║ 边框字符，所以 ║ 的出现次数为 2。
            var titleLineCount = CountOccurrences(output, "║");
            Assert.Equal(2, titleLineCount);
        }

        [Fact]
        public void ShowMaxTitle_WithChineseText_ShouldCenterCorrectly()
        {
            var output = CaptureConsole(() => LogHelper.ShowMaxTitle("系统启动"));

            Assert.Contains("系统启动", output, StringComparison.Ordinal);
        }

        [Fact]
        public void ShowMaxTitle_WithVeryLongTitle_ShouldStillOutput()
        {
            // 标题超过框架宽度时应使用简化格式
            LogHelper.SetFrameLength(10);
            var longTitle = "This is a very long title that exceeds the frame width significantly";
            var output = CaptureConsole(() => LogHelper.ShowMaxTitle(longTitle));

            Assert.Contains(longTitle, output, StringComparison.Ordinal);
        }

        // ============================================================
        // ShowOption
        // ============================================================

        [Fact]
        public void ShowOption_WithNullTitle_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.ShowOption(null!, "content"));
        }

        [Fact]
        public void ShowOption_WithNullContent_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.ShowOption("title", null!));
        }

        [Fact]
        public void ShowOption_WithValidArgs_ShouldOutputTitleAndContent()
        {
            var output = CaptureConsole(() => LogHelper.ShowOption("数据库", "Server=localhost"));

            Assert.Contains("数据库", output, StringComparison.Ordinal);
            Assert.Contains("Server=localhost", output, StringComparison.Ordinal);
            Assert.Contains("╔", output, StringComparison.Ordinal);
            Assert.Contains("╚", output, StringComparison.Ordinal);
        }

        [Fact]
        public void ShowOption_WithIntegerContent_ShouldCallToString()
        {
            var output = CaptureConsole(() => LogHelper.ShowOption("端口", 3306));

            Assert.Contains("3306", output, StringComparison.Ordinal);
        }

        [Fact]
        public void ShowOption_WithLongTitle_ShouldUseSimpleFormat()
        {
            LogHelper.SetFrameLength(10);
            var longTitle = "AVeryLongConfigurationTitleThatExceedsFrameWidth";
            var output = CaptureConsole(() => LogHelper.ShowOption(longTitle, "value"));

            Assert.Contains(longTitle, output, StringComparison.Ordinal);
            Assert.Contains("value", output, StringComparison.Ordinal);
        }

        // ============================================================
        // ShowLineTitle
        // ============================================================

        [Fact]
        public void ShowLineTitle_WithNullTitle_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.ShowLineTitle(null!));
        }

        [Fact]
        public void ShowLineTitle_WithEmptyString_ShouldOutputPureSeparatorLine()
        {
            var output = CaptureConsole(() => LogHelper.ShowLineTitle(""));

            // 空标题 → 纯分隔线（全部是 ═ 字符 + 空行）
            Assert.Contains("═", output, StringComparison.Ordinal);
            // 不应包含额外的标题文本
            // 输出应该有至少一行分隔线 + 一行空行
            var lines = output.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Assert.True(lines.Length >= 1, "应该至少输出一行分隔线。");
        }

        [Fact]
        public void ShowLineTitle_WithTitle_ShouldEmbedTitleInSeparator()
        {
            var output = CaptureConsole(() => LogHelper.ShowLineTitle("初始化完成"));

            Assert.Contains("初始化完成", output, StringComparison.Ordinal);
            Assert.Contains("═", output, StringComparison.Ordinal);
        }

        [Fact]
        public void ShowLineTitle_WithVeryLongTitle_ShouldUseSimpleFormat()
        {
            LogHelper.SetFrameLength(10);
            var longTitle = "VeryLongSeparatorTitleThatExceedsTheFrameWidthLimitForSure";
            var output = CaptureConsole(() => LogHelper.ShowLineTitle(longTitle));

            Assert.Contains(longTitle, output, StringComparison.Ordinal);
            // 简化格式用 ═══ 包围
            Assert.Contains("═══", output, StringComparison.Ordinal);
        }

        // ============================================================
        // LogHelper.Console 方法（LogHelper.cs 主文件）
        // ============================================================

        [Fact]
        public void Console_WithMessage_ShouldOutputTimestampedLine()
        {
            var output = CaptureConsole(() => LogHelper.Console("hello console"));

            Assert.Contains("hello console", output, StringComparison.Ordinal);
            // 时间戳格式 [yyyy-MM-dd HH:mm:ss.fff]
            Assert.Matches(@"\[\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d{3}\]", output);
        }

        [Fact]
        public void Console_WithMessageAndArgs_ShouldFormatOutput()
        {
            var output = CaptureConsole(() => LogHelper.Console("user {0} logged in", "alice"));

            Assert.Contains("user alice logged in", output, StringComparison.Ordinal);
        }

        [Fact]
        public void Console_WithMultipleArgs_ShouldFormatAll()
        {
            var output = CaptureConsole(() => LogHelper.Console("{0} + {1} = {2}", 1, 2, 3));

            Assert.Contains("1 + 2 = 3", output, StringComparison.Ordinal);
        }

        [Fact]
        public void Console_WithEmptyMessage_ShouldNotThrow()
        {
            var output = CaptureConsole(() => LogHelper.Console(""));

            // 空消息也输出时间戳行
            Assert.Matches(@"\[\d{4}-\d{2}-\d{2}", output);
        }

        // ============================================================
        // 辅助方法
        // ============================================================

        /// <summary>
        /// 捕获 Console.Out 的输出文本，确保测试结束后恢复原始输出流。
        /// </summary>
        private static string CaptureConsole(Action action)
        {
            using (var writer = new StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    action();
                }
                finally
                {
                    Console.SetOut(original);
                }

                return writer.ToString();
            }
        }

        private static int CountOccurrences(string haystack, string needle)
        {
            var count = 0;
            var index = 0;
            while (true)
            {
                index = haystack.IndexOf(needle, index, StringComparison.Ordinal);
                if (index < 0)
                {
                    break;
                }

                count++;
                index += needle.Length;
            }

            return count;
        }
    }
}
