using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// JsonHelper 的补充覆盖测试：未覆盖的重载、fallback 路径、Try* 变体、UTF8 字节变体。
    /// </summary>
    public class JsonHelperAdditionalTests
    {
        // ============================================================
        // Serialize — 未覆盖的重载
        // ============================================================

        [Fact]
        public void Serialize_WithCustomOptions_ShouldUseProvidedOptions()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var obj = new SimpleModel { Id = 1, Name = "test" };
            string json = JsonHelper.Serialize(obj, options);
            Assert.Contains("\"id\":", json);
            Assert.Contains("\"name\":", json);
        }

        [Fact]
        public void Serialize_WithNullOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Serialize(new SimpleModel(), (JsonSerializerOptions)null));
        }

        [Fact]
        public void Serialize_WithTypeInfo_ShouldWork()
        {
            var model = new SimpleModel { Id = 1, Name = "TypeInfo" };
            string json = JsonHelper.Serialize(model, AdditionalTestsJsonContext.Default.SimpleModel);
            Assert.Contains("TypeInfo", json);
        }

        [Fact]
        public void Serialize_WithNullTypeInfo_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Serialize(new SimpleModel(), (JsonTypeInfo<SimpleModel>)null));
        }

        // ============================================================
        // SerializeToUtf8Bytes — 未覆盖的重载
        // ============================================================

        [Fact]
        public void SerializeToUtf8Bytes_WithCustomOptions_ShouldUseProvidedOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var obj = new SimpleModel { Id = 5, Name = "bytes" };
            byte[] bytes = JsonHelper.SerializeToUtf8Bytes(obj, options);
            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 0);
            string json = Encoding.UTF8.GetString(bytes);
            Assert.Contains("\"id\":", json);
        }

        [Fact]
        public void SerializeToUtf8Bytes_WithNullOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => JsonHelper.SerializeToUtf8Bytes(new SimpleModel(), (JsonSerializerOptions)null));
        }

        [Fact]
        public void SerializeToUtf8BytesFormat_ShouldProduceFormattedBytes()
        {
            var obj = new SimpleModel { Id = 1, Name = "formatted" };
            byte[] bytes = JsonHelper.SerializeToUtf8BytesFormat(obj);
            Assert.NotNull(bytes);
            string json = Encoding.UTF8.GetString(bytes);
            Assert.Contains("\n", json);
            Assert.Contains("  ", json);
        }

        [Fact]
        public void SerializeToUtf8Bytes_WithTypeInfo_ShouldWork()
        {
            var model = new SimpleModel { Id = 2, Name = "utf8info" };
            byte[] bytes = JsonHelper.SerializeToUtf8Bytes(model, AdditionalTestsJsonContext.Default.SimpleModel);
            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 0);
        }

        // ============================================================
        // SerializeAsync — 未覆盖的重载
        // ============================================================

        [Fact]
        public async Task SerializeAsync_WithCustomOptions_ShouldWork()
        {
            var obj = new SimpleModel { Id = 1, Name = "async" };
            var options = new JsonSerializerOptions { WriteIndented = true };
            using (var stream = new MemoryStream())
            {
                await JsonHelper.SerializeAsync(stream, obj, options);
                Assert.True(stream.Length > 0);
                stream.Position = 0;
                string json = Encoding.UTF8.GetString(stream.ToArray());
                Assert.Contains("async", json);
            }
        }

        [Fact]
        public async Task SerializeAsync_WithTypeInfo_ShouldWork()
        {
            var model = new SimpleModel { Id = 3, Name = "asyncInfo" };
            using (var stream = new MemoryStream())
            {
                await JsonHelper.SerializeAsync(stream, model, AdditionalTestsJsonContext.Default.SimpleModel);
                Assert.True(stream.Length > 0);
            }
        }

        [Fact]
        public async Task SerializeAsync_WithNullStream_ShouldThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await JsonHelper.SerializeAsync<SimpleModel>(null, new SimpleModel()));
        }

        // ============================================================
        // Deserialize — 未覆盖的重载
        // ============================================================

        [Fact]
        public void Deserialize_WithOptions_ShouldUseProvidedOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            string json = "{\"Id\":10,\"Name\":\"custom\"}";
            var result = JsonHelper.Deserialize<SimpleModel>(json, options);
            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
            Assert.Equal("custom", result.Name);
        }

        [Fact]
        public void Deserialize_WithNullOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Deserialize<SimpleModel>("{}", (JsonSerializerOptions)null));
        }

        [Fact]
        public void Deserialize_WithTypeAndOptions_ShouldWork()
        {
            var options = new JsonSerializerOptions();
            string json = "{\"Id\":20,\"Name\":\"typeopts\"}";
            var result = JsonHelper.Deserialize(json, typeof(SimpleModel), options) as SimpleModel;
            Assert.NotNull(result);
            Assert.Equal(20, result.Id);
            Assert.Equal("typeopts", result.Name);
        }

        [Fact]
        public void Deserialize_WithTypeAndNullOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Deserialize("{}", typeof(SimpleModel), (JsonSerializerOptions)null));
        }

        [Fact]
        public void Deserialize_WithTypeAndNullType_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Deserialize("{}", (Type)null, new JsonSerializerOptions()));
        }

        [Fact]
        public void Deserialize_WithTypeInfo_ShouldWork()
        {
            string json = "{\"Id\":30,\"Name\":\"info\"}";
            var result = JsonHelper.Deserialize(json, AdditionalTestsJsonContext.Default.SimpleModel);
            Assert.NotNull(result);
            Assert.Equal(30, result.Id);
            Assert.Equal("info", result.Name);
        }

        // ============================================================
        // Deserialize — PreprocessSpecialFloatingPointValues fallback 路径
        // ============================================================

        [Fact]
        public void Deserialize_PreprocessFallback_NaNForStringProperty_ShouldConvertToString()
        {
            // JSON 含裸 NaN，但目标属性是 string。
            // 第一次反序列化失败（NaN 不是有效字符串 token），
            // fallback 将 : NaN 包裹引号 -> : "NaN"，第二次成功。
            string json = "{\"Name\": NaN}";
            var result = JsonHelper.Deserialize<SimpleModel>(json);
            Assert.NotNull(result);
            Assert.Equal("NaN", result.Name);
        }

        [Fact]
        public void Deserialize_PreprocessFallback_InfinityForStringProperty_ShouldConvertToString()
        {
            string json = "{\"Name\": Infinity}";
            var result = JsonHelper.Deserialize<SimpleModel>(json);
            Assert.NotNull(result);
            Assert.Equal("Infinity", result.Name);
        }

        [Fact]
        public void Deserialize_PreprocessFallback_NegativeInfinityForStringProperty_ShouldConvertToString()
        {
            string json = "{\"Name\": -Infinity}";
            var result = JsonHelper.Deserialize<SimpleModel>(json);
            Assert.NotNull(result);
            Assert.Equal("-Infinity", result.Name);
        }

        [Fact]
        public void Deserialize_WithType_PreprocessFallback_NaNForStringProperty_ShouldConvertToString()
        {
            string json = "{\"Name\": NaN}";
            var result = JsonHelper.Deserialize(json, typeof(SimpleModel)) as SimpleModel;
            Assert.NotNull(result);
            Assert.Equal("NaN", result.Name);
        }

        // ============================================================
        // DeserializeAsync — 未覆盖的重载
        // ============================================================

        [Fact]
        public async Task DeserializeAsync_WithCustomOptions_ShouldWork()
        {
            string json = "{\"Id\":40,\"Name\":\"stream\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(bytes))
            {
                var options = new JsonSerializerOptions();
                var result = await JsonHelper.DeserializeAsync<SimpleModel>(stream, options);
                Assert.NotNull(result);
                Assert.Equal(40, result.Id);
                Assert.Equal("stream", result.Name);
            }
        }

        [Fact]
        public async Task DeserializeAsync_WithTypeInfo_ShouldWork()
        {
            string json = "{\"Id\":50,\"Name\":\"streamInfo\"}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            using (var stream = new MemoryStream(bytes))
            {
                var result = await JsonHelper.DeserializeAsync(stream, AdditionalTestsJsonContext.Default.SimpleModel);
                Assert.NotNull(result);
                Assert.Equal(50, result.Id);
            }
        }

        [Fact]
        public async Task DeserializeAsync_WithNullStream_ShouldThrowArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await JsonHelper.DeserializeAsync<SimpleModel>(null));
        }

        // ============================================================
        // DeserializeFromUtf8Bytes — 未覆盖的重载
        // ============================================================

        [Fact]
        public void DeserializeFromUtf8Bytes_WithOptions_ShouldWork()
        {
            var obj = new SimpleModel { Id = 60, Name = "bytes" };
            byte[] bytes = Encoding.UTF8.GetBytes(JsonHelper.Serialize(obj));
            var options = new JsonSerializerOptions();
            var result = JsonHelper.DeserializeFromUtf8Bytes<SimpleModel>(bytes, options);
            Assert.NotNull(result);
            Assert.Equal(60, result.Id);
            Assert.Equal("bytes", result.Name);
        }

        [Fact]
        public void DeserializeFromUtf8Bytes_WithNullOptions_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                JsonHelper.DeserializeFromUtf8Bytes<SimpleModel>(new byte[0], (JsonSerializerOptions)null));
        }

        [Fact]
        public void DeserializeFromUtf8Bytes_WithTypeInfo_ShouldWork()
        {
            var model = new SimpleModel { Id = 70, Name = "ti" };
            byte[] bytes = Encoding.UTF8.GetBytes(JsonHelper.Serialize(model));
            var result = JsonHelper.DeserializeFromUtf8Bytes(bytes, AdditionalTestsJsonContext.Default.SimpleModel);
            Assert.NotNull(result);
            Assert.Equal(70, result.Id);
        }

        [Fact]
        public void DeserializeFromUtf8Bytes_WithNullTypeInfo_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                JsonHelper.DeserializeFromUtf8Bytes<SimpleModel>(new byte[0], (JsonTypeInfo<SimpleModel>)null));
        }

        [Fact]
        public void DeserializeFromUtf8Bytes_AllFallbacksFail_ShouldThrowJsonException()
        {
            // 无效 JSON 触发全部 fallback 链：DefaultOptions -> FormatOptions -> preprocess -> DefaultOptions -> FormatOptions
            byte[] invalidBytes = Encoding.UTF8.GetBytes("///not-valid-json///");
            Assert.Throws<JsonException>(() =>
                JsonHelper.DeserializeFromUtf8Bytes<SimpleModel>(invalidBytes));
        }

        [Fact]
        public void DeserializeFromUtf8Bytes_EmptyArray_ShouldThrow()
        {
            // 空字节数组 -> 所有 fallback 路径都失败
            byte[] emptyBytes = new byte[0];
            Assert.ThrowsAny<Exception>(() =>
                JsonHelper.DeserializeFromUtf8Bytes<SimpleModel>(emptyBytes));
        }

        // ============================================================
        // TryDeserialize — 未覆盖的重载
        // ============================================================

        [Fact]
        public void TryDeserialize_WithOptions_ShouldSucceed()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TryDeserialize("{\"Id\":1}", out SimpleModel model, options);
            Assert.True(result);
            Assert.NotNull(model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public void TryDeserialize_WithNullOptions_ShouldReturnFalse()
        {
            bool result = JsonHelper.TryDeserialize("{\"Id\":1}", out SimpleModel model, (JsonSerializerOptions)null);
            Assert.False(result);
            Assert.Null(model);
        }

        [Fact]
        public void TryDeserialize_WithWhitespaceJsonAndOptions_ShouldReturnFalse()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TryDeserialize("   ", out SimpleModel model, options);
            Assert.False(result);
            Assert.Null(model);
        }

        [Fact]
        public void TryDeserialize_WithOptionsAndError_ValidJson_ShouldSucceed()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TryDeserialize("{\"Id\":1,\"Name\":\"ok\"}", out SimpleModel model, options, out JsonHelperError error);
            Assert.True(result);
            Assert.NotNull(model);
            Assert.Equal(1, model.Id);
            Assert.Null(error);
        }

        [Fact]
        public void TryDeserialize_WithOptionsAndError_InvalidJson_ShouldReturnError()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TryDeserialize("invalid", out SimpleModel model, options, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            Assert.Equal(JsonHelperErrorKind.InvalidInput, error.Kind);
        }

        [Fact]
        public void TryDeserialize_WithOptionsAndError_NullOptions_ShouldReturnError()
        {
            bool result = JsonHelper.TryDeserialize("{\"Id\":1}", out SimpleModel model, (JsonSerializerOptions)null, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            Assert.Equal(typeof(ArgumentNullException), error.ExceptionType);
        }

        [Fact]
        public void TryDeserialize_WithOptionsAndError_WhitespaceJson_ShouldReturnError()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TryDeserialize("", out SimpleModel model, options, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            Assert.Equal(JsonHelperErrorKind.InvalidInput, error.Kind);
        }

        [Fact]
        public void TryDeserialize_WithTypeInfoAndError_ValidJson_ShouldSucceed()
        {
            string json = "{\"Id\":80,\"Name\":\"ti\"}";
            bool result = JsonHelper.TryDeserialize(json, out SimpleModel model, AdditionalTestsJsonContext.Default.SimpleModel, out JsonHelperError error);
            Assert.True(result);
            Assert.NotNull(model);
            Assert.Equal(80, model.Id);
            Assert.Null(error);
        }

        [Fact]
        public void TryDeserialize_WithTypeInfoAndError_InvalidJson_ShouldReturnError()
        {
            bool result = JsonHelper.TryDeserialize("invalid", out SimpleModel model, AdditionalTestsJsonContext.Default.SimpleModel, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
        }

        [Fact]
        public void TryDeserialize_WithTypeInfoAndError_NullTypeInfo_ShouldReturnError()
        {
            bool result = JsonHelper.TryDeserialize("{\"Id\":1}", out SimpleModel model, (JsonTypeInfo<SimpleModel>)null, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            Assert.Equal(typeof(ArgumentNullException), error.ExceptionType);
        }

        [Fact]
        public void TryDeserialize_WithError_NoOptions_ShouldUseDefaultOptions()
        {
            // TryDeserialize<T>(string, out T, out JsonHelperError) delegates to the 4-arg version with DefaultOptions
            bool result = JsonHelper.TryDeserialize("{\"Id\":1}", out SimpleModel model, out JsonHelperError error);
            Assert.True(result);
            Assert.NotNull(model);
            Assert.Equal(1, model.Id);
            Assert.Null(error);
        }

        // ============================================================
        // TrySerialize — 未覆盖的重载
        // ============================================================

        [Fact]
        public void TrySerialize_WithOptions_ShouldSucceed()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TrySerialize(new SimpleModel { Id = 1, Name = "x" }, out string json, options);
            Assert.True(result);
            Assert.NotNull(json);
            Assert.Contains("x", json);
        }

        [Fact]
        public void TrySerialize_WithNullOptions_ShouldReturnFalse()
        {
            bool result = JsonHelper.TrySerialize(new SimpleModel(), out string json, (JsonSerializerOptions)null);
            Assert.False(result);
            Assert.Null(json);
        }

        [Fact]
        public void TrySerialize_WithOptionsAndError_ValidObject_ShouldSucceed()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TrySerialize(new SimpleModel { Id = 1 }, out string json, options, out JsonHelperError error);
            Assert.True(result);
            Assert.NotNull(json);
            Assert.Null(error);
        }

        [Fact]
        public void TrySerialize_WithOptionsAndError_NullObj_ShouldReturnError()
        {
            var options = new JsonSerializerOptions();
            bool result = JsonHelper.TrySerialize(null, out string json, options, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
            Assert.Equal(typeof(ArgumentNullException), error.ExceptionType);
        }

        [Fact]
        public void TrySerialize_WithOptionsAndError_NullOptions_ShouldReturnError()
        {
            bool result = JsonHelper.TrySerialize(new SimpleModel(), out string json, (JsonSerializerOptions)null, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
            Assert.Equal(typeof(ArgumentNullException), error.ExceptionType);
        }

        [Fact]
        public void TrySerialize_WithError_UsesDefaultOptions()
        {
            // TrySerialize(object, out string, out JsonHelperError) delegates to the 4-arg version
            bool result = JsonHelper.TrySerialize(new SimpleModel { Id = 1 }, out string json, out JsonHelperError error);
            Assert.True(result);
            Assert.NotNull(json);
            Assert.Null(error);
        }

        [Fact]
        public void TrySerialize_WithError_NullObj_ShouldReturnError()
        {
            bool result = JsonHelper.TrySerialize(null, out string json, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
        }

        [Fact]
        public void TrySerialize_WithTypeInfo_ShouldSucceed()
        {
            var model = new SimpleModel { Id = 1, Name = "ti" };
            bool result = JsonHelper.TrySerialize(model, out string json, AdditionalTestsJsonContext.Default.SimpleModel, out JsonHelperError error);
            Assert.True(result);
            Assert.NotNull(json);
            Assert.Null(error);
        }

        [Fact]
        public void TrySerialize_WithTypeInfo_NullObj_ShouldReturnError()
        {
            bool result = JsonHelper.TrySerialize<SimpleModel>(null, out string json, AdditionalTestsJsonContext.Default.SimpleModel, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
            Assert.Equal(typeof(ArgumentNullException), error.ExceptionType);
        }

        [Fact]
        public void TrySerialize_WithTypeInfo_NullTypeInfo_ShouldReturnError()
        {
            var model = new SimpleModel { Id = 1 };
            bool result = JsonHelper.TrySerialize(model, out string json, (JsonTypeInfo<SimpleModel>)null, out JsonHelperError error);
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
        }

        // ============================================================
        // Options — CreateFormatOptions / FormatOptions.IsReadOnly
        // ============================================================

        [Fact]
        public void CreateFormatOptions_ShouldReturnMutableCopy()
        {
            JsonSerializerOptions copy = JsonHelper.CreateFormatOptions();
            Assert.False(copy.IsReadOnly);
            Assert.True(copy.WriteIndented);

            // Modifying copy should not affect original
            copy.WriteIndented = false;
            Assert.False(copy.WriteIndented);
            Assert.True(JsonHelper.FormatOptions.WriteIndented);
        }

        [Fact]
        public void FormatOptions_ShouldBeReadOnly()
        {
            Assert.True(JsonHelper.FormatOptions.IsReadOnly);
            Assert.Throws<InvalidOperationException>(() => JsonHelper.FormatOptions.WriteIndented = false);
        }

        [Fact]
        public void CreateDefaultOptions_ShouldReturnMutableCopyWithCorrectEncoder()
        {
            JsonSerializerOptions copy = JsonHelper.CreateDefaultOptions();
            Assert.False(copy.IsReadOnly);
            Assert.False(copy.WriteIndented);
            Assert.Same(UnicodeJsonEncoder.Singleton, copy.Encoder);
        }

        [Fact]
        public void CreateFormatOptions_ShouldReturnMutableCopyWithCorrectEncoder()
        {
            JsonSerializerOptions copy = JsonHelper.CreateFormatOptions();
            Assert.False(copy.IsReadOnly);
            Assert.Same(UnicodeJsonEncoder.Singleton, copy.Encoder);
        }

        // ============================================================
        // 循环引用 / 深嵌套
        // ============================================================

        [Fact]
        public void Serialize_CircularReference_ShouldNotThrow()
        {
            // DefaultOptions uses ReferenceHandler.IgnoreCycles
            var node = new CircularModel { Id = 1 };
            node.Self = node;

            string json = JsonHelper.Serialize(node);
            Assert.Contains("\"Id\":1", json);
            // Self should be ignored (null cycle) rather than causing infinite loop
        }

        [Fact]
        public void Serialize_NullValueProperty_ShouldBeIgnored()
        {
            // DefaultOptions has DefaultIgnoreCondition = WhenWritingNull
            var obj = new SimpleModel { Id = 1, Name = null };
            string json = JsonHelper.Serialize(obj);
            Assert.DoesNotContain("\"Name\"", json);
            Assert.Contains("\"Id\":1", json);
        }

        [Fact]
        public void Serialize_DeeplyNestedObject_ShouldWork()
        {
            var root = new NestedModel { Id = 0 };
            var current = root;
            for (int i = 1; i <= 10; i++)
            {
                current.Child = new NestedModel { Id = i };
                current = current.Child;
            }

            string json = JsonHelper.Serialize(root);
            var result = JsonHelper.Deserialize<NestedModel>(json);
            Assert.NotNull(result);
            int depth = 0;
            var node = result;
            while (node != null)
            {
                Assert.Equal(depth, node.Id);
                node = node.Child;
                depth++;
            }
            Assert.Equal(11, depth);
        }

        // ============================================================
        // 测试模型
        // ============================================================

        public class SimpleModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CircularModel
        {
            public int Id { get; set; }
            public CircularModel Self { get; set; }
        }

        public class NestedModel
        {
            public int Id { get; set; }
            public NestedModel Child { get; set; }
        }
    }

    // 必须位于命名空间级别（非嵌套），JsonSerializerContext 的 source generator 才会生成实现
    [JsonSerializable(typeof(JsonHelperAdditionalTests.SimpleModel))]
    public partial class AdditionalTestsJsonContext : JsonSerializerContext
    {
    }
}
