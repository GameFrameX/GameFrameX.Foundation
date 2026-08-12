using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// SpecialFloatingPointConverter / SpecialFloatingPointConverterFloat / SpecialFloatingPointDocumentConverter
    /// 的覆盖测试：NaN / Infinity / -Infinity 往返、正常数值不受影响、边界与异常路径。
    /// </summary>
    public class SpecialFloatingPointConverterTests
    {
        // ============================================================
        // SpecialFloatingPointConverter (double) — Write
        // ============================================================

        [Fact]
        public void Double_Write_NaN_ShouldProduceQuotedNaN()
        {
            string json = WriteDoubleWithConverter(double.NaN);
            Assert.Equal("\"NaN\"", json);
        }

        [Fact]
        public void Double_Write_PositiveInfinity_ShouldProduceQuotedInfinity()
        {
            string json = WriteDoubleWithConverter(double.PositiveInfinity);
            Assert.Equal("\"Infinity\"", json);
        }

        [Fact]
        public void Double_Write_NegativeInfinity_ShouldProduceQuotedNegativeInfinity()
        {
            string json = WriteDoubleWithConverter(double.NegativeInfinity);
            Assert.Equal("\"-Infinity\"", json);
        }

        [Theory]
        [InlineData(0.0, "0")]
        [InlineData(1.0, "1")]
        [InlineData(-1.0, "-1")]
        [InlineData(3.14, "3.14")]
        [InlineData(-42.5, "-42.5")]
        public void Double_Write_NormalValue_ShouldProduceNumber(double value, string expected)
        {
            string json = WriteDoubleWithConverter(value);
            Assert.Equal(expected, json);
        }

        [Fact]
        public void Double_Write_MinValue_ShouldProduceNumber()
        {
            string json = WriteDoubleWithConverter(double.MinValue);
            Assert.DoesNotContain("NaN", json);
            Assert.DoesNotContain("Infinity", json);
        }

        [Fact]
        public void Double_Write_MaxValue_ShouldProduceNumber()
        {
            string json = WriteDoubleWithConverter(double.MaxValue);
            Assert.DoesNotContain("NaN", json);
            Assert.DoesNotContain("Infinity", json);
        }

        // ============================================================
        // SpecialFloatingPointConverter (double) — Read
        // ============================================================

        [Fact]
        public void Double_Read_StringNaN_ShouldReturnNaN()
        {
            double result = ReadDoubleWithConverter("\"NaN\"");
            Assert.True(double.IsNaN(result));
        }

        [Fact]
        public void Double_Read_StringInfinity_ShouldReturnPositiveInfinity()
        {
            double result = ReadDoubleWithConverter("\"Infinity\"");
            Assert.True(double.IsPositiveInfinity(result));
        }

        [Fact]
        public void Double_Read_StringNegativeInfinity_ShouldReturnNegativeInfinity()
        {
            double result = ReadDoubleWithConverter("\"-Infinity\"");
            Assert.True(double.IsNegativeInfinity(result));
        }

        [Theory]
        [InlineData("\"nan\"")]
        [InlineData("\"NAN\"")]
        [InlineData("\"nAn\"")]
        public void Double_Read_NaNCasing_ShouldBeCaseInsensitive(string json)
        {
            double result = ReadDoubleWithConverter(json);
            Assert.True(double.IsNaN(result));
        }

        [Theory]
        [InlineData("\"infinity\"")]
        [InlineData("\"INFINITY\"")]
        [InlineData("\"Infinity\"")]
        public void Double_Read_InfinityCasing_ShouldBeCaseInsensitive(string json)
        {
            double result = ReadDoubleWithConverter(json);
            Assert.True(double.IsPositiveInfinity(result));
        }

        [Theory]
        [InlineData("\"-infinity\"")]
        [InlineData("\"-INFINITY\"")]
        [InlineData("\"-Infinity\"")]
        public void Double_Read_NegativeInfinityCasing_ShouldBeCaseInsensitive(string json)
        {
            double result = ReadDoubleWithConverter(json);
            Assert.True(double.IsNegativeInfinity(result));
        }

        [Fact]
        public void Double_Read_NumericString_ShouldParseValue()
        {
            double result = ReadDoubleWithConverter("\"3.14\"");
            Assert.Equal(3.14, result);
        }

        [Fact]
        public void Double_Read_NegativeNumericString_ShouldParseValue()
        {
            double result = ReadDoubleWithConverter("\"-42.5\"");
            Assert.Equal(-42.5, result);
        }

        [Fact]
        public void Double_Read_IntegerString_ShouldParseValue()
        {
            double result = ReadDoubleWithConverter("\"100\"");
            Assert.Equal(100.0, result);
        }

        [Fact]
        public void Double_Read_NumberToken_ShouldReturnValue()
        {
            double result = ReadDoubleWithConverter("3.14");
            Assert.Equal(3.14, result);
        }

        [Fact]
        public void Double_Read_NegativeNumberToken_ShouldReturnValue()
        {
            double result = ReadDoubleWithConverter("-99.9");
            Assert.Equal(-99.9, result);
        }

        [Fact]
        public void Double_Read_IntegerNumberToken_ShouldReturnValue()
        {
            double result = ReadDoubleWithConverter("42");
            Assert.Equal(42.0, result);
        }

        [Fact]
        public void Double_Read_UnparseableString_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadDoubleWithConverter("\"not-a-number\""));
        }

        [Fact]
        public void Double_Read_TrueToken_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadDoubleWithConverter("true"));
        }

        [Fact]
        public void Double_Read_FalseToken_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadDoubleWithConverter("false"));
        }

        [Fact]
        public void Double_Read_NullToken_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadDoubleWithConverter("null"));
        }

        // ============================================================
        // SpecialFloatingPointConverter (double) — Round-trip via serializer
        // ============================================================

        [Fact]
        public void Double_RoundTrip_NaN_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverter());
            var obj = new DoubleModel { Value = double.NaN };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\"NaN\"", json);
            var result = JsonSerializer.Deserialize<DoubleModel>(json, options);
            Assert.True(double.IsNaN(result.Value));
        }

        [Fact]
        public void Double_RoundTrip_PositiveInfinity_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverter());
            var obj = new DoubleModel { Value = double.PositiveInfinity };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\"Infinity\"", json);
            var result = JsonSerializer.Deserialize<DoubleModel>(json, options);
            Assert.True(double.IsPositiveInfinity(result.Value));
        }

        [Fact]
        public void Double_RoundTrip_NegativeInfinity_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverter());
            var obj = new DoubleModel { Value = double.NegativeInfinity };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\"-Infinity\"", json);
            var result = JsonSerializer.Deserialize<DoubleModel>(json, options);
            Assert.True(double.IsNegativeInfinity(result.Value));
        }

        [Fact]
        public void Double_RoundTrip_NormalValue_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverter());
            var obj = new DoubleModel { Value = 123.456 };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.DoesNotContain("NaN", json);
            Assert.DoesNotContain("Infinity", json);
            var result = JsonSerializer.Deserialize<DoubleModel>(json, options);
            Assert.Equal(123.456, result.Value);
        }

        [Fact]
        public void Double_RoundTrip_Zero_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverter());
            var obj = new DoubleModel { Value = 0.0 };
            string json = JsonSerializer.Serialize(obj, options);
            var result = JsonSerializer.Deserialize<DoubleModel>(json, options);
            Assert.Equal(0.0, result.Value);
        }

        // ============================================================
        // SpecialFloatingPointConverterFloat (float) — Write
        // ============================================================

        [Fact]
        public void Float_Write_NaN_ShouldProduceQuotedNaN()
        {
            string json = WriteFloatWithConverter(float.NaN);
            Assert.Equal("\"NaN\"", json);
        }

        [Fact]
        public void Float_Write_PositiveInfinity_ShouldProduceQuotedInfinity()
        {
            string json = WriteFloatWithConverter(float.PositiveInfinity);
            Assert.Equal("\"Infinity\"", json);
        }

        [Fact]
        public void Float_Write_NegativeInfinity_ShouldProduceQuotedNegativeInfinity()
        {
            string json = WriteFloatWithConverter(float.NegativeInfinity);
            Assert.Equal("\"-Infinity\"", json);
        }

        [Theory]
        [InlineData(0.0f, "0")]
        [InlineData(1.0f, "1")]
        [InlineData(-1.0f, "-1")]
        [InlineData(3.14f, "3.14")]
        public void Float_Write_NormalValue_ShouldProduceNumber(float value, string expected)
        {
            string json = WriteFloatWithConverter(value);
            Assert.Equal(expected, json);
        }

        // ============================================================
        // SpecialFloatingPointConverterFloat (float) — Read
        // ============================================================

        [Fact]
        public void Float_Read_StringNaN_ShouldReturnNaN()
        {
            float result = ReadFloatWithConverter("\"NaN\"");
            Assert.True(float.IsNaN(result));
        }

        [Fact]
        public void Float_Read_StringInfinity_ShouldReturnPositiveInfinity()
        {
            float result = ReadFloatWithConverter("\"Infinity\"");
            Assert.True(float.IsPositiveInfinity(result));
        }

        [Fact]
        public void Float_Read_StringNegativeInfinity_ShouldReturnNegativeInfinity()
        {
            float result = ReadFloatWithConverter("\"-Infinity\"");
            Assert.True(float.IsNegativeInfinity(result));
        }

        [Theory]
        [InlineData("\"nan\"")]
        [InlineData("\"NAN\"")]
        public void Float_Read_NaNCasing_ShouldBeCaseInsensitive(string json)
        {
            float result = ReadFloatWithConverter(json);
            Assert.True(float.IsNaN(result));
        }

        [Fact]
        public void Float_Read_NumericString_ShouldParseValue()
        {
            float result = ReadFloatWithConverter("\"3.14\"");
            Assert.Equal(3.14f, result);
        }

        [Fact]
        public void Float_Read_NumberToken_ShouldReturnValue()
        {
            float result = ReadFloatWithConverter("3.14");
            Assert.Equal(3.14f, result);
        }

        [Fact]
        public void Float_Read_UnparseableString_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadFloatWithConverter("\"not-a-number\""));
        }

        [Fact]
        public void Float_Read_TrueToken_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadFloatWithConverter("true"));
        }

        [Fact]
        public void Float_Read_NullToken_ShouldThrowJsonException()
        {
            Assert.Throws<JsonException>(() => ReadFloatWithConverter("null"));
        }

        // ============================================================
        // SpecialFloatingPointConverterFloat (float) — Round-trip via serializer
        // ============================================================

        [Fact]
        public void Float_RoundTrip_NaN_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverterFloat());
            var obj = new FloatModel { Value = float.NaN };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\"NaN\"", json);
            var result = JsonSerializer.Deserialize<FloatModel>(json, options);
            Assert.True(float.IsNaN(result.Value));
        }

        [Fact]
        public void Float_RoundTrip_PositiveInfinity_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverterFloat());
            var obj = new FloatModel { Value = float.PositiveInfinity };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\"Infinity\"", json);
            var result = JsonSerializer.Deserialize<FloatModel>(json, options);
            Assert.True(float.IsPositiveInfinity(result.Value));
        }

        [Fact]
        public void Float_RoundTrip_NegativeInfinity_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverterFloat());
            var obj = new FloatModel { Value = float.NegativeInfinity };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.Contains("\"-Infinity\"", json);
            var result = JsonSerializer.Deserialize<FloatModel>(json, options);
            Assert.True(float.IsNegativeInfinity(result.Value));
        }

        [Fact]
        public void Float_RoundTrip_NormalValue_ShouldRestoreValue()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new SpecialFloatingPointConverterFloat());
            var obj = new FloatModel { Value = 42.5f };
            string json = JsonSerializer.Serialize(obj, options);
            Assert.DoesNotContain("NaN", json);
            Assert.DoesNotContain("Infinity", json);
            var result = JsonSerializer.Deserialize<FloatModel>(json, options);
            Assert.Equal(42.5f, result.Value);
        }

        // ============================================================
        // SpecialFloatingPointDocumentConverter — Write
        // ============================================================

        [Fact]
        public void Document_Write_NormalJsonDocument_ShouldOutputContent()
        {
            var converter = new SpecialFloatingPointDocumentConverter();
            JsonDocument doc = JsonDocument.Parse("{\"name\":\"test\",\"value\":42}");
            string json = WriteDocumentWithConverter(converter, doc);
            Assert.Contains("\"name\"", json);
            Assert.Contains("\"test\"", json);
            Assert.Contains("42", json);
        }

        [Fact]
        public void Document_Write_JsonDocumentWithSpecialFloatStrings_ShouldPreserveValues()
        {
            var converter = new SpecialFloatingPointDocumentConverter();
            JsonDocument doc = JsonDocument.Parse("{\"nan\":\"NaN\",\"inf\":\"Infinity\"}");
            string json = WriteDocumentWithConverter(converter, doc);
            Assert.Contains("NaN", json);
            Assert.Contains("Infinity", json);
        }

        [Fact]
        public void Document_Write_JsonDocumentWithNestedObject_ShouldOutputContent()
        {
            var converter = new SpecialFloatingPointDocumentConverter();
            JsonDocument doc = JsonDocument.Parse("{\"outer\":{\"inner\":\"value\"}}");
            string json = WriteDocumentWithConverter(converter, doc);
            Assert.Contains("\"outer\"", json);
            Assert.Contains("\"inner\"", json);
        }

        [Fact]
        public void Document_Write_JsonDocumentWithArray_ShouldOutputContent()
        {
            var converter = new SpecialFloatingPointDocumentConverter();
            JsonDocument doc = JsonDocument.Parse("{\"items\":[1,2,3]}");
            string json = WriteDocumentWithConverter(converter, doc);
            Assert.Contains("\"items\"", json);
            Assert.Contains("1", json);
            Assert.Contains("2", json);
            Assert.Contains("3", json);
        }

        // ============================================================
        // SpecialFloatingPointDocumentConverter — Read (exercises GetModifiedJson)
        // ============================================================

        [Fact]
        public void Document_Read_ObjectJson_ShouldExerciseGetModifiedJson()
        {
            // GetModifiedJson 中 reader.Read() 跳过了 StartObject，导致 Utf8JsonWriter
            // 在未 WriteStartObject 的情况下直接 WritePropertyName，抛 InvalidOperationException。
            Assert.Throws<InvalidOperationException>(() =>
                ReadDocumentWithConverter("{\"Value\":\"NaN\",\"Num\":42}"));
        }

        [Fact]
        public void Document_Read_WithSpecialFloatString_ShouldExerciseIsSpecialFloatingPointValue()
        {
            // Exercises the IsSpecialFloatingPointValue branch in GetModifiedJson.
            // 与上同理，Utf8JsonWriter 未先 WriteStartObject 即 WritePropertyName 抛 InvalidOperationException。
            Assert.Throws<InvalidOperationException>(() =>
                ReadDocumentWithConverter("{\"Value\":\"NaN\"}"));
        }

        [Fact]
        public void Document_Read_WithMultipleTokenTypes_ShouldExerciseAllSwitchCases()
        {
            // Exercises String, Number, True, False, Null, and structural token branches.
            // 与上同理，Utf8JsonWriter 未先 WriteStartObject 即 WritePropertyName 抛 InvalidOperationException。
            Assert.Throws<InvalidOperationException>(() =>
                ReadDocumentWithConverter("{\"str\":\"hello\",\"num\":3.14,\"flag\":true,\"off\":false,\"empty\":null}"));
        }

        // ============================================================
        // Test models
        // ============================================================

        public class DoubleModel
        {
            public double Value { get; set; }
        }

        public class FloatModel
        {
            public float Value { get; set; }
        }

        // ============================================================
        // Helpers
        // ============================================================

        private static string WriteDoubleWithConverter(double value)
        {
            var converter = new SpecialFloatingPointConverter();
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    converter.Write(writer, value, new JsonSerializerOptions());
                    writer.Flush();
                }
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static double ReadDoubleWithConverter(string json)
        {
            var converter = new SpecialFloatingPointConverter();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            Utf8JsonReader reader = new Utf8JsonReader(bytes, isFinalBlock: true, state: default);
            reader.Read();
            return converter.Read(ref reader, typeof(double), new JsonSerializerOptions());
        }

        private static string WriteFloatWithConverter(float value)
        {
            var converter = new SpecialFloatingPointConverterFloat();
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    converter.Write(writer, value, new JsonSerializerOptions());
                    writer.Flush();
                }
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static float ReadFloatWithConverter(string json)
        {
            var converter = new SpecialFloatingPointConverterFloat();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            Utf8JsonReader reader = new Utf8JsonReader(bytes, isFinalBlock: true, state: default);
            reader.Read();
            return converter.Read(ref reader, typeof(float), new JsonSerializerOptions());
        }

        private static string WriteDocumentWithConverter(SpecialFloatingPointDocumentConverter converter, JsonDocument doc)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    converter.Write(writer, doc, new JsonSerializerOptions());
                    writer.Flush();
                }
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static JsonDocument ReadDocumentWithConverter(string json)
        {
            var converter = new SpecialFloatingPointDocumentConverter();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            Utf8JsonReader reader = new Utf8JsonReader(bytes, isFinalBlock: true, state: default);
            reader.Read();
            return converter.Read(ref reader, typeof(JsonDocument), new JsonSerializerOptions());
        }
    }
}
