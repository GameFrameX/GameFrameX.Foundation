using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// UnicodeJsonEncoder 的覆盖测试：
    /// WillEncode 判定逻辑、Encode 行为（转义 vs 原样）、与默认编码器的差异。
    /// </summary>
    public class UnicodeJsonEncoderTests
    {
        // ============================================================
        // Singleton & 属性
        // ============================================================

        [Fact]
        public void Singleton_ShouldBeUnicodeJsonEncoderInstance()
        {
            Assert.NotNull(UnicodeJsonEncoder.Singleton);
            Assert.IsType<UnicodeJsonEncoder>(UnicodeJsonEncoder.Singleton);
        }

        [Fact]
        public void MaxOutputCharactersPerInputCharacter_ShouldBe6()
        {
            Assert.Equal(6, UnicodeJsonEncoder.Singleton.MaxOutputCharactersPerInputCharacter);
        }

        // ============================================================
        // WillEncode — 需要编码的字符（控制字符、引号、反斜杠）
        // ============================================================

        [Theory]
        [InlineData(0x0000)]
        [InlineData(0x0001)]
        [InlineData(0x0008)]
        [InlineData(0x0009)]
        [InlineData(0x000A)]
        [InlineData(0x0010)]
        [InlineData(0x001F)]
        public void WillEncode_ControlChar_ShouldReturnTrue(int scalar)
        {
            Assert.True(UnicodeJsonEncoder.Singleton.WillEncode(scalar));
        }

        [Fact]
        public void WillEncode_DoubleQuote_ShouldReturnTrue()
        {
            Assert.True(UnicodeJsonEncoder.Singleton.WillEncode(0x0022));
        }

        [Fact]
        public void WillEncode_Backslash_ShouldReturnTrue()
        {
            Assert.True(UnicodeJsonEncoder.Singleton.WillEncode(0x005C));
        }

        // ============================================================
        // WillEncode — 不需要编码的字符
        // ============================================================

        [Theory]
        [InlineData(0x0041)] // 'A'
        [InlineData(0x0061)] // 'a'
        [InlineData(0x0030)] // '0'
        [InlineData(0x0020)] // space
        [InlineData(0x007E)] // '~'
        public void WillEncode_AsciiChar_ShouldReturnFalse(int scalar)
        {
            Assert.False(UnicodeJsonEncoder.Singleton.WillEncode(scalar));
        }

        [Fact]
        public void WillEncode_ForwardSlash_ShouldReturnFalse()
        {
            Assert.False(UnicodeJsonEncoder.Singleton.WillEncode(0x002F));
        }

        [Theory]
        [InlineData(0x4E2D)] // 中
        [InlineData(0x6587)] // 文
        [InlineData(0x6D4B)] // 测
        [InlineData(0x8BD5)] // 试
        public void WillEncode_ChineseChar_ShouldReturnFalse(int scalar)
        {
            Assert.False(UnicodeJsonEncoder.Singleton.WillEncode(scalar));
        }

        [Fact]
        public void WillEncode_SupplementaryPlaneEmoji_ShouldReturnFalse()
        {
            Assert.False(UnicodeJsonEncoder.Singleton.WillEncode(0x1F600));
        }

        [Fact]
        public void WillEncode_SupplementaryPlaneCJK_ShouldReturnFalse()
        {
            Assert.False(UnicodeJsonEncoder.Singleton.WillEncode(0x20000));
        }

        [Fact]
        public void WillEncode_DeleteChar_ShouldReturnFalse()
        {
            Assert.False(UnicodeJsonEncoder.Singleton.WillEncode(0x007F));
        }

        // ============================================================
        // Encode — 不需要转义的字符原样输出
        // ============================================================

        [Fact]
        public void Encode_EmptyString_ShouldReturnEmpty()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode("");
            Assert.Equal("", result);
        }

        [Fact]
        public void Encode_PureAscii_ShouldReturnUnchanged()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode("Hello World 123");
            Assert.Equal("Hello World 123", result);
        }

        [Fact]
        public void Encode_Chinese_ShouldReturnUnchanged()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode("中文测试");
            Assert.Equal("中文测试", result);
        }

        [Fact]
        public void Encode_Emoji_ShouldReturnUnchanged()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode("😀👍🎉");
            Assert.Equal("😀👍🎉", result);
        }

        [Fact]
        public void Encode_MixedAsciiAndChinese_ShouldReturnUnchanged()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode("Hello世界World");
            Assert.Equal("Hello世界World", result);
        }

        [Fact]
        public void Encode_ForwardSlash_ShouldNotBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode("a/b");
            Assert.Equal("a/b", result);
        }

        // ============================================================
        // Encode — 两字符转义序列 (\", \\, \b, \f, \n, \r, \t)
        // 使用 char.ConvertFromUtf32 构造输入，避免源码中嵌入控制字符
        // ============================================================

        [Fact]
        public void Encode_DoubleQuote_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x0022));
            Assert.Equal("\\\"", result);
        }

        [Fact]
        public void Encode_Backslash_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x005C));
            Assert.Equal("\\\\", result);
        }

        [Fact]
        public void Encode_Backspace_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x0008));
            Assert.Equal("\\b", result);
        }

        [Fact]
        public void Encode_FormFeed_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x000C));
            Assert.Equal("\\f", result);
        }

        [Fact]
        public void Encode_Newline_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x000A));
            Assert.Equal("\\n", result);
        }

        [Fact]
        public void Encode_CarriageReturn_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x000D));
            Assert.Equal("\\r", result);
        }

        [Fact]
        public void Encode_Tab_ShouldBeEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x0009));
            Assert.Equal("\\t", result);
        }

        // ============================================================
        // Encode — \u00XX 六字符转义序列（无两字符转义的控制字符）
        // ============================================================

        [Fact]
        public void Encode_NullChar_ShouldBeHexEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x0000));
            Assert.Equal("\\u0000", result);
        }

        [Fact]
        public void Encode_VerticalTab_ShouldBeHexEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x000B));
            Assert.Equal("\\u000b", result);
        }

        [Fact]
        public void Encode_ControlChar01_ShouldBeHexEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x0001));
            Assert.Equal("\\u0001", result);
        }

        [Fact]
        public void Encode_ControlChar1F_ShouldBeHexEscaped()
        {
            string result = UnicodeJsonEncoder.Singleton.Encode(char.ConvertFromUtf32(0x001F));
            Assert.Equal("\\u001f", result);
        }

        // ============================================================
        // Encode — 混合字符串
        // ============================================================

        [Fact]
        public void Encode_MixedChineseAndTab_ShouldEscapeOnlyTab()
        {
            string input = "中文" + char.ConvertFromUtf32(0x0009) + "测试" + char.ConvertFromUtf32(0x000A);
            string result = UnicodeJsonEncoder.Singleton.Encode(input);
            Assert.Equal("中文\\t测试\\n", result);
        }

        [Fact]
        public void Encode_MixedAsciiAndControl_ShouldEscapeOnlyControl()
        {
            string input = "a" + char.ConvertFromUtf32(0x0001) + "b";
            string result = UnicodeJsonEncoder.Singleton.Encode(input);
            Assert.Equal("a\\u0001b", result);
        }

        [Fact]
        public void Encode_MixedEmojiAndTab_ShouldEscapeOnlyTab()
        {
            string input = "😀" + char.ConvertFromUtf32(0x0009) + "👍";
            string result = UnicodeJsonEncoder.Singleton.Encode(input);
            Assert.Equal("😀\\t👍", result);
        }

        [Fact]
        public void Encode_StringWithQuoteInMiddle_ShouldEscapeQuoteOnly()
        {
            string input = "say" + char.ConvertFromUtf32(0x0022) + "hello";
            string result = UnicodeJsonEncoder.Singleton.Encode(input);
            Assert.Equal("say\\\"hello", result);
        }

        [Fact]
        public void Encode_StringWithBackslashAndChinese_ShouldEscapeBackslashOnly()
        {
            string input = "中文" + char.ConvertFromUtf32(0x005C) + "测试";
            string result = UnicodeJsonEncoder.Singleton.Encode(input);
            Assert.Equal("中文\\\\测试", result);
        }

        // ============================================================
        // 与默认 JavaScriptEncoder 的差异
        // ============================================================

        [Fact]
        public void Encode_DefaultEncoderEscapesChinese_UnicodeEncoderDoesNot()
        {
            string input = "中文";

            string defaultResult = JavaScriptEncoder.Default.Encode(input);
            string unicodeResult = UnicodeJsonEncoder.Singleton.Encode(input);

            Assert.NotEqual(defaultResult, unicodeResult);
            Assert.Equal(input, unicodeResult);
            Assert.Contains("\\u", defaultResult);
            Assert.DoesNotContain("\\u", unicodeResult);
        }

        [Fact]
        public void Encode_DefaultEncoderEscapesEmoji_UnicodeEncoderDoesNot()
        {
            string input = "😀";

            string defaultResult = JavaScriptEncoder.Default.Encode(input);
            string unicodeResult = UnicodeJsonEncoder.Singleton.Encode(input);

            Assert.NotEqual(defaultResult, unicodeResult);
            Assert.Equal(input, unicodeResult);
        }

        [Fact]
        public void Encode_BothEncodersEscapeControlChar()
        {
            string input = char.ConvertFromUtf32(0x0001);

            string defaultResult = JavaScriptEncoder.Default.Encode(input);
            string unicodeResult = UnicodeJsonEncoder.Singleton.Encode(input);

            Assert.Equal("\\u0001", defaultResult);
            Assert.Equal("\\u0001", unicodeResult);
        }

        // ============================================================
        // 通过 JSON 序列化的集成测试
        // ============================================================

        [Fact]
        public void Serialization_WithUnicodeEncoder_ChineseNotEscaped()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = UnicodeJsonEncoder.Singleton
            };
            var obj = new { Name = "中文测试" };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("中文测试", json);
            Assert.DoesNotContain("\\u", json);
        }

        [Fact]
        public void Serialization_WithUnicodeEncoder_ControlCharsEscaped()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = UnicodeJsonEncoder.Singleton
            };
            string value = "a" + char.ConvertFromUtf32(0x0001) + "b" + char.ConvertFromUtf32(0x0009) + "c";
            var obj = new { Value = value };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\\u0001", json);
            Assert.Contains("\\t", json);
        }

        [Fact]
        public void Serialization_WithUnicodeEncoder_EmojiNotEscaped()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = UnicodeJsonEncoder.Singleton
            };
            var obj = new { Message = "Hello😀World" };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("😀", json);
        }

        [Fact]
        public void Serialization_WithDefaultEncoder_ChineseEscaped()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Default
            };
            var obj = new { Name = "中文" };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\\u", json);
            Assert.DoesNotContain("中文", json);
        }
    }
}
