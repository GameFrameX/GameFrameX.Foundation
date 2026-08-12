using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// JsonHelper 边界测试：CancellationToken 取消、TryDeserialize/TrySerialize 各重载的 false 路径、
    /// NaN/Infinity 在 JsonTypeInfo 重载中的回退、TrySerialize 对异常输入的处理。
    /// </summary>
    /// <remarks>
    /// Boundary tests for JsonHelper: CancellationToken cancellation, false paths of all
    /// TryDeserialize/TrySerialize overloads, NaN/Infinity fallback under JsonTypeInfo overload,
    /// and TrySerialize behavior on inputs that throw during serialization.
    /// </remarks>
    public class JsonHelperBoundaryTests
    {
        // ============================================================
        // 1. CancellationToken 取消 — JsonTypeInfo 重载
        // （非 TypeInfo 重载已由 JsonHelperTests.SerializeAsyncDeserializeAsync_Stream_ShouldObserveCancellation 覆盖）
        // ============================================================

        /// <summary>
        /// 使用预取消 token 调用 SerializeAsync 的 JsonTypeInfo 重载时应抛出 OperationCanceledException。
        /// </summary>
        [Fact]
        public async Task SerializeAsync_WithJsonTypeInfo_PreCancelledToken_ShouldThrowOperationCanceledException()
        {
            // Arrange
            var model = new BoundaryModel { Id = 1, Name = "cancel" };
            using (var stream = new MemoryStream())
            {
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();

                    // Act + Assert
                    await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                        JsonHelper.SerializeAsync(stream, model, BoundaryTestsJsonContext.Default.BoundaryModel, cts.Token));
                }
            }
        }

        /// <summary>
        /// 使用预取消 token 调用 DeserializeAsync 的 JsonTypeInfo 重载时应抛出 OperationCanceledException。
        /// </summary>
        [Fact]
        public async Task DeserializeAsync_WithJsonTypeInfo_PreCancelledToken_ShouldThrowOperationCanceledException()
        {
            // Arrange
            byte[] bytes = Encoding.UTF8.GetBytes("{\"Id\":1,\"Name\":\"x\"}");
            using (var stream = new MemoryStream(bytes))
            {
                using (var cts = new CancellationTokenSource())
                {
                    cts.Cancel();

                    // Act + Assert
                    await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                        await JsonHelper.DeserializeAsync(stream, BoundaryTestsJsonContext.Default.BoundaryModel, cts.Token));
                }
            }
        }

        // ============================================================
        // 2. TryDeserialize 所有 5 个重载的 false 路径（无效 JSON）
        // ============================================================

        /// <summary>
        /// TryDeserialize&lt;T&gt;(string, out T) 对无效 JSON 返回 false 且 out 为 default。
        /// </summary>
        [Fact]
        public void TryDeserialize_BasicOverload_InvalidJson_ShouldReturnFalse()
        {
            // Arrange
            const string invalidJson = "{bad";

            // Act
            bool result = JsonHelper.TryDeserialize<BoundaryModel>(invalidJson, out BoundaryModel model);

            // Assert
            Assert.False(result);
            Assert.Null(model);
        }

        /// <summary>
        /// TryDeserialize&lt;T&gt;(string, out T, JsonSerializerOptions) 对无效 JSON 返回 false 且 out 为 default。
        /// </summary>
        [Fact]
        public void TryDeserialize_WithOptionsOverload_InvalidJson_ShouldReturnFalse()
        {
            // Arrange
            const string invalidJson = "{bad";
            var options = new JsonSerializerOptions();

            // Act
            bool result = JsonHelper.TryDeserialize<BoundaryModel>(invalidJson, out BoundaryModel model, options);

            // Assert
            Assert.False(result);
            Assert.Null(model);
        }

        /// <summary>
        /// TryDeserialize&lt;T&gt;(string, out T, out JsonHelperError) 对无效 JSON 返回 false 且填充 error。
        /// </summary>
        [Fact]
        public void TryDeserialize_WithErrorOverload_InvalidJson_ShouldReturnFalseAndPopulateError()
        {
            // Arrange - 此重载内部委托给 options+error 版本（使用 DefaultOptions）
            const string invalidJson = "{bad";

            // Act
            bool result = JsonHelper.TryDeserialize<BoundaryModel>(invalidJson, out BoundaryModel model, out JsonHelperError error);

            // Assert
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            Assert.Equal(typeof(JsonException), error.ExceptionType);
        }

        /// <summary>
        /// TryDeserialize&lt;T&gt;(string, out T, JsonSerializerOptions, out JsonHelperError) 对无效 JSON 返回 false 且填充 error。
        /// </summary>
        [Fact]
        public void TryDeserialize_WithOptionsAndErrorOverload_InvalidJson_ShouldReturnFalseAndPopulateError()
        {
            // Arrange
            const string invalidJson = "{bad";
            var options = new JsonSerializerOptions();

            // Act
            bool result = JsonHelper.TryDeserialize<BoundaryModel>(invalidJson, out BoundaryModel model, options, out JsonHelperError error);

            // Assert
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            Assert.Equal(typeof(JsonException), error.ExceptionType);
        }

        /// <summary>
        /// TryDeserialize&lt;T&gt;(string, out T, JsonTypeInfo&lt;T&gt;, out JsonHelperError) 对无效 JSON 返回 false 且填充 error。
        /// </summary>
        [Fact]
        public void TryDeserialize_WithTypeInfoAndErrorOverload_InvalidJson_ShouldReturnFalseAndPopulateError()
        {
            // Arrange
            const string invalidJson = "{bad";

            // Act
            bool result = JsonHelper.TryDeserialize<BoundaryModel>(invalidJson, out BoundaryModel model, BoundaryTestsJsonContext.Default.BoundaryModel, out JsonHelperError error);

            // Assert
            Assert.False(result);
            Assert.Null(model);
            Assert.NotNull(error);
            // 无效 JSON 不含 NaN/Infinity，回退 when 过滤不命中，原始 JsonException 被 TryDeserialize 捕获
            Assert.Equal(typeof(JsonException), error.ExceptionType);
        }

        // ============================================================
        // 3. NaN/Infinity 回退 — Deserialize<T>(string, JsonTypeInfo<T>)
        // （简单重载 Deserialize<T>(string) 已在 JsonHelperAdditionalTests.Deserialize_PreprocessFallback_* 中覆盖）
        // ============================================================

        /// <summary>
        /// 使用 JsonTypeInfo 重载反序列化含裸 NaN 的 JSON（目标属性为 string）应触发预处理回退并返回 "NaN"。
        /// </summary>
        /// <remarks>
        /// Source-gen JsonTypeInfo 默认不含 AllowNamedFloatingPointLiterals，第一次解析在 NaN token 处抛 JsonException；
        /// catch (json 含 "NaN") 触发 PreprocessSpecialFloatingPointValues 将 : NaN 包裹引号 -> : "NaN"，
        /// 第二次解析 Name 字符串属性成功。
        /// </remarks>
        [Fact]
        public void Deserialize_WithJsonTypeInfo_NaNForStringProperty_ShouldFallbackToQuotedAndSucceed()
        {
            // Arrange
            const string json = "{\"Name\": NaN}";

            // Act
            BoundaryModel result = JsonHelper.Deserialize(json, BoundaryTestsJsonContext.Default.BoundaryModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NaN", result.Name);
        }

        /// <summary>
        /// 使用 JsonTypeInfo 重载反序列化含裸 Infinity 的 JSON（目标属性为 string）应触发预处理回退并返回 "Infinity"。
        /// </summary>
        [Fact]
        public void Deserialize_WithJsonTypeInfo_InfinityForStringProperty_ShouldFallbackToQuotedAndSucceed()
        {
            // Arrange
            const string json = "{\"Name\": Infinity}";

            // Act
            BoundaryModel result = JsonHelper.Deserialize(json, BoundaryTestsJsonContext.Default.BoundaryModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Infinity", result.Name);
        }

        /// <summary>
        /// 使用 JsonTypeInfo 重载反序列化含裸 -Infinity 的 JSON（目标属性为 string）应触发预处理回退并返回 "-Infinity"。
        /// </summary>
        [Fact]
        public void Deserialize_WithJsonTypeInfo_NegativeInfinityForStringProperty_ShouldFallbackToQuotedAndSucceed()
        {
            // Arrange
            const string json = "{\"Name\": -Infinity}";

            // Act
            BoundaryModel result = JsonHelper.Deserialize(json, BoundaryTestsJsonContext.Default.BoundaryModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("-Infinity", result.Name);
        }

        /// <summary>
        /// 使用 JsonTypeInfo 重载反序列化 {Id: NaN}（Id 为 int）时，预处理后 Id 仍无法接受字符串 "NaN"，
        /// 回退重试再次抛 JsonException，异常向外传播（不被吞）。
        /// </summary>
        /// <remarks>
        /// 第一次失败：parser 在 NaN token 处抛 JsonException -> catch 命中 (json 含 "NaN") ->
        /// PreprocessSpecialFloatingPointValues -> {"Id": "NaN"} ->
        /// 第二次重试：source-gen 不允许从字符串读取 int -> JsonException 向外传播。
        /// </remarks>
        [Fact]
        public void Deserialize_WithJsonTypeInfo_NaNForIntPropertyFallbackFails_ShouldThrowJsonException()
        {
            // Arrange
            const string json = "{\"Id\": NaN}";

            // Act + Assert
            Assert.Throws<JsonException>(() =>
                JsonHelper.Deserialize(json, BoundaryTestsJsonContext.Default.BoundaryModel));
        }

        // ============================================================
        // 4. TrySerialize 各重载对会抛异常的输入返回 false
        // （DefaultOptions 使用 ReferenceHandler.IgnoreCycles，循环引用不会抛；故使用 getter 抛异常的模型）
        // ============================================================

        /// <summary>
        /// TrySerialize(object, out string) 在序列化对象抛异常时返回 false 且 out 为 null。
        /// </summary>
        [Fact]
        public void TrySerialize_DefaultOverload_WhenGetterThrows_ShouldReturnFalse()
        {
            // Arrange - 使用 DefaultOptions（IgnoreCycles），但 ThrowingModel.Bomb getter 抛异常
            var obj = new ThrowingModel { Id = 1 };

            // Act
            bool result = JsonHelper.TrySerialize(obj, out string json);

            // Assert
            Assert.False(result);
            Assert.Null(json);
        }

        /// <summary>
        /// TrySerialize(object, out string, JsonSerializerOptions) 在序列化对象抛异常时返回 false 且 out 为 null。
        /// </summary>
        [Fact]
        public void TrySerialize_WithOptionsOverload_WhenGetterThrows_ShouldReturnFalse()
        {
            // Arrange
            var obj = new ThrowingModel { Id = 1 };
            var options = new JsonSerializerOptions();

            // Act
            bool result = JsonHelper.TrySerialize(obj, out string json, options);

            // Assert
            Assert.False(result);
            Assert.Null(json);
        }

        /// <summary>
        /// TrySerialize(object, out string, out JsonHelperError) 在序列化对象抛异常时返回 false 且填充 error。
        /// </summary>
        [Fact]
        public void TrySerialize_WithErrorOverload_WhenGetterThrows_ShouldReturnFalseAndPopulateError()
        {
            // Arrange - 此重载内部委托给 options+error 版本（使用 DefaultOptions）
            var obj = new ThrowingModel { Id = 1 };

            // Act
            bool result = JsonHelper.TrySerialize(obj, out string json, out JsonHelperError error);

            // Assert
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
        }

        /// <summary>
        /// TrySerialize(object, out string, JsonSerializerOptions, out JsonHelperError) 在序列化对象抛异常时返回 false 且填充 error。
        /// error.Kind 应为 Serialization（非 JsonException 走 fallbackKind 分支）。
        /// </summary>
        [Fact]
        public void TrySerialize_WithOptionsAndErrorOverload_WhenGetterThrows_ShouldReturnFalseAndPopulateError()
        {
            // Arrange
            var obj = new ThrowingModel { Id = 1 };
            var options = new JsonSerializerOptions();

            // Act
            bool result = JsonHelper.TrySerialize(obj, out string json, options, out JsonHelperError error);

            // Assert
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
            Assert.Equal(JsonHelperErrorKind.Serialization, error.Kind);
            Assert.Equal(typeof(InvalidOperationException), error.ExceptionType);
        }

        /// <summary>
        /// TrySerialize&lt;T&gt;(T, out string, JsonTypeInfo&lt;T&gt;, out JsonHelperError) 在序列化对象抛异常时返回 false 且填充 error。
        /// </summary>
        [Fact]
        public void TrySerialize_WithTypeInfoOverload_WhenGetterThrows_ShouldReturnFalseAndPopulateError()
        {
            // Arrange
            var obj = new ThrowingModel { Id = 1 };

            // Act
            bool result = JsonHelper.TrySerialize(obj, out string json, BoundaryTestsJsonContext.Default.ThrowingModel, out JsonHelperError error);

            // Assert
            Assert.False(result);
            Assert.Null(json);
            Assert.NotNull(error);
            Assert.Equal(JsonHelperErrorKind.Serialization, error.Kind);
        }

        /// <summary>
        /// 使用不含 IgnoreCycles 的自定义选项序列化循环引用对象时，序列化抛异常，
        /// TrySerialize(object, out string, JsonSerializerOptions) 应返回 false。
        /// </summary>
        /// <remarks>
        /// 默认 JsonSerializerOptions 使用 ReferenceHandler.Default，检测到循环时抛 InvalidOperationException。
        /// 与 DefaultOptions（ReferenceHandler.IgnoreCycles）行为不同。
        /// </remarks>
        [Fact]
        public void TrySerialize_WithOptionsOverload_CircularReferenceWithDefaultHandler_ShouldReturnFalse()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            var node = new CircularNode { Id = 1 };
            node.Self = node;

            // Act
            bool result = JsonHelper.TrySerialize(node, out string json, options);

            // Assert
            Assert.False(result);
            Assert.Null(json);
        }

        // ============================================================
        // 测试模型
        // ============================================================

        /// <summary>
        /// 简单测试模型，包含 int Id 与 string Name，用于 TryDeserialize false 路径与 NaN/Infinity 回退测试。
        /// </summary>
        public class BoundaryModel
        {
            /// <summary>整数标识。</summary>
            public int Id { get; set; }

            /// <summary>字符串名称。</summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// 在序列化时会抛异常的模型，用于验证 TrySerialize 的 false 路径。
        /// Bomb 属性 getter 总是抛 InvalidOperationException。
        /// </summary>
        public class ThrowingModel
        {
            /// <summary>整数标识。</summary>
            public int Id { get; set; }

            /// <summary>
            /// 总是抛异常的属性，触发序列化失败。
            /// </summary>
            public string Bomb
            {
                get { throw new InvalidOperationException("Bomb getter always throws"); }
            }
        }

        /// <summary>
        /// 循环引用模型，配合不含 IgnoreCycles 的 JsonSerializerOptions 触发序列化异常。
        /// </summary>
        public class CircularNode
        {
            /// <summary>整数标识。</summary>
            public int Id { get; set; }

            /// <summary>指向自身的循环引用。</summary>
            public CircularNode Self { get; set; }
        }
    }

    /// <summary>
    /// JsonHelperBoundaryTests 使用的源生成 JsonTypeInfo 上下文。
    /// 必须位于命名空间级别（非嵌套），JsonSerializerContext 的 source generator 才会生成实现。
    /// </summary>
    [JsonSerializable(typeof(JsonHelperBoundaryTests.BoundaryModel))]
    [JsonSerializable(typeof(JsonHelperBoundaryTests.ThrowingModel))]
    public partial class BoundaryTestsJsonContext : JsonSerializerContext
    {
    }
}
