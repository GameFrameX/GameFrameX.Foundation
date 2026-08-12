using System;
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// OptionsBuilder 补充单元测试：
    /// 重复参数（后覆盖前契约）、含 = 的值、Unicode/特殊字符/含空格的值，
    /// 以及 ConvertToOptionsDictionary 对重复 key 的覆盖语义。
    /// </summary>
    [Collection(OptionsProviderCollection.Name)]
    public class OptionsBuilderAdditionalTests
    {
        // ============================================================
        // 重复参数：后覆盖前
        // ============================================================

        [Fact]
        public void Build_WithDuplicateSeparatedArgs_ShouldKeepLastValue()
        {
            // 源码契约：ConvertToOptionsDictionary 使用 Dictionary，后写入覆盖先写入。
            // 即 --port 1 --port 2 最终 port = 2（不抛异常、不累加）。
            var args = new[] { "--port", "1", "--port", "2" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal(2, config.Port);
        }

        [Fact]
        public void Build_WithDuplicateKeyValueArgs_ShouldKeepLastValue()
        {
            var args = new[] { "--port=1", "--port=2" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal(2, config.Port);
        }

        [Fact]
        public void Build_WithDuplicateMixedFormat_ShouldKeepLastValue()
        {
            // 混合分离格式与键值对格式，后出现的覆盖先出现的。
            var args = new[] { "--port", "1", "--port=2" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal(2, config.Port);
        }

        [Fact]
        public void Build_WithDuplicateStringArgs_ShouldKeepLastValue()
        {
            var args = new[] { "--host", "first.example.com", "--host", "second.example.com" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("second.example.com", config.Host);
        }

        [Fact]
        public void Build_WithDuplicateBoolArgs_ShouldKeepLastValue()
        {
            // 多次设置 bool 属性，最后一次生效
            var args = new[] { "--debug=true", "--debug=false" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.False(config.Debug);
        }

        // ============================================================
        // 含 = 的值：键值按第一个 = 分割
        // ============================================================

        [Fact]
        public void Build_WithValueContainingEquals_ShouldPreserveEqualsInValue()
        {
            // ApplyKeyValuePair 使用 Split(new[] {'='}, 2)，仅按第一个 = 分割。
            // 期望：key=key, value=a=b
            var args = new[] { "--key=a=b" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("a=b", config.Key);
        }

        [Fact]
        public void Build_WithValueContainingMultipleEquals_ShouldPreserveAllInValue()
        {
            var args = new[] { "--conn=a=b=c" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("a=b=c", config.Conn);
        }

        [Fact]
        public void Build_WithEqualsSeparatedEmptyValue_ShouldSetEmptyString()
        {
            // --key= 应解析为 key=key, value=""
            var args = new[] { "--key=" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("", config.Key);
        }

        [Fact]
        public void Build_WithEqualsSeparatedValueContainingEquals_InSeparatedMode_ShouldStillParse()
        {
            // 即使整体模式是分离格式，键值对形式仍然按 = 分割
            var args = new[] { "--key=a=b" };

            var builder = new OptionsBuilder<DuplicateConfig>(args, BoolArgumentFormat.Separated);
            var config = builder.Build();

            Assert.Equal("a=b", config.Key);
        }

        // ============================================================
        // Unicode / 特殊字符 / 含空格的值
        // ============================================================

        [Fact]
        public void Build_WithUnicodeValue_ShouldPreserveUnicode()
        {
            var args = new[] { "--host", "中文主机.例子" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("中文主机.例子", config.Host);
        }

        [Fact]
        public void Build_WithEmojiValue_ShouldPreserveEmoji()
        {
            var args = new[] { "--host", "server🎮🚀" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("server🎮🚀", config.Host);
        }

        [Fact]
        public void Build_WithValueContainingSpaces_ShouldPreserveSpaces()
        {
            // 命令行参数已在 shell 层拆分，--host "hello world" 传入测试时为两个元素
            var args = new[] { "--host", "hello world" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("hello world", config.Host);
        }

        [Fact]
        public void Build_WithSpecialCharactersValue_ShouldPreserveSpecialCharacters()
        {
            var args = new[] { "--host", "user@host:port/path?query=1#frag" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("user@host:port/path?query=1#frag", config.Host);
        }

        [Fact]
        public void Build_WithJsonLikeValue_ShouldPreserveAsIs()
        {
            // JSON 值中包含 = 和 { }，应原样保留
            var args = new[] { "--key={\"name\":\"value\",\"eq\":\"a=b\"}" };

            var builder = new OptionsBuilder<DuplicateConfig>(args);
            var config = builder.Build();

            Assert.Equal("{\"name\":\"value\",\"eq\":\"a=b\"}", config.Key);
        }

        // ============================================================
        // 测试辅助类
        // ============================================================

        private class DuplicateConfig
        {
            public string Host { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public bool Debug { get; set; } = false;
            public string Key { get; set; } = "default-key";
            public string Conn { get; set; } = "default-conn";
        }
    }
}
