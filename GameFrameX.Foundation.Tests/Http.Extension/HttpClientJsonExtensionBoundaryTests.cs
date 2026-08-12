using System.Net;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// HttpClientJsonExtension 的边界条件测试：
    /// - GetDelay（private）通过 public 重试入口间接覆盖：负 BaseDelay、BackoffFactor&lt;1、BackoffFactor=1 常数延迟、BackoffFactor=3 第 3 次重试 9×BaseDelay
    /// - Summarize（private）通过 public 反序列化/状态码失败入口间接覆盖：&gt;1024 截断、=1024 不截断、非成功状态截断
    /// - RequestOptions 边界：JsonSerializerOptions=null 回退框架默认值、Headers 重复 key 覆盖语义
    /// </summary>
    public sealed class HttpClientJsonExtensionBoundaryTests
    {
        // ============================================================
        // GetDelay 间接测试 — 通过触发重试调用 GetDelay
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_NegativeBaseDelay_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            using (var client = new HttpClient(handler))
            {
                var options = new HttpClientRequestOptions
                {
                    Retry = new HttpClientRetryOptions
                    {
                        MaxRetries = 1,
                        BaseDelay = TimeSpan.FromMilliseconds(-1),
                    },
                };

                // Act & Assert — GetDelay 在重试时检测到 BaseDelay &lt; TimeSpan.Zero 抛 ArgumentOutOfRangeException
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.GetJsonAsync<SampleResponse>("http://example.com/api", options));
            }
        }

        [Fact]
        public async Task GetJsonAsync_BackoffFactorBelowOne_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            using (var client = new HttpClient(handler))
            {
                var options = new HttpClientRequestOptions
                {
                    Retry = new HttpClientRetryOptions
                    {
                        MaxRetries = 1,
                        BaseDelay = TimeSpan.FromMilliseconds(10),
                        BackoffFactor = 0.5D,
                    },
                };

                // Act & Assert — GetDelay 在重试时检测到 BackoffFactor &lt; 1 抛 ArgumentOutOfRangeException
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.GetJsonAsync<SampleResponse>("http://example.com/api", options));
            }
        }

        [Fact]
        public async Task GetJsonAsync_BackoffFactorOne_ProducesConstantDelay()
        {
            // Arrange
            var retryEntries = new List<HttpClientRetryLogEntry>();
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.EnqueueOK("{\"Name\":\"ok\",\"Value\":1}");
            using (var client = new HttpClient(handler))
            {
                var baseDelay = TimeSpan.FromMilliseconds(5);
                var options = new HttpClientRequestOptions
                {
                    Retry = new HttpClientRetryOptions
                    {
                        MaxRetries = 3,
                        BaseDelay = baseDelay,
                        BackoffFactor = 1.0D,
                        OnRetry = retryEntries.Add,
                    },
                };

                // Act
                var result = await client.GetJsonAsync<SampleResponse>("http://example.com/api", options);

                // Assert — BackoffFactor=1.0 时 1^(n-1)=1，每次延迟 = BaseDelay
                Assert.NotNull(result);
                Assert.Equal(3, retryEntries.Count);
                Assert.Equal(baseDelay, retryEntries[0].Delay);
                Assert.Equal(baseDelay, retryEntries[1].Delay);
                Assert.Equal(baseDelay, retryEntries[2].Delay);
            }
        }

        [Fact]
        public async Task GetJsonAsync_BackoffFactorThree_AttemptThree_ProducesNineTimesBaseDelay()
        {
            // Arrange
            var retryEntries = new List<HttpClientRetryLogEntry>();
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.EnqueueOK("{\"Name\":\"ok\",\"Value\":1}");
            using (var client = new HttpClient(handler))
            {
                var baseDelay = TimeSpan.FromMilliseconds(10);
                var options = new HttpClientRequestOptions
                {
                    Retry = new HttpClientRetryOptions
                    {
                        MaxRetries = 3,
                        BaseDelay = baseDelay,
                        BackoffFactor = 3.0D,
                        OnRetry = retryEntries.Add,
                    },
                };

                // Act
                var result = await client.GetJsonAsync<SampleResponse>("http://example.com/api", options);

                // Assert — attempt=1: 3^0=1×BaseDelay; attempt=2: 3^1=3×BaseDelay; attempt=3: 3^2=9×BaseDelay
                Assert.NotNull(result);
                Assert.Equal(3, retryEntries.Count);
                Assert.Equal(TimeSpan.FromMilliseconds(10), retryEntries[0].Delay);
                Assert.Equal(TimeSpan.FromMilliseconds(30), retryEntries[1].Delay);
                Assert.Equal(TimeSpan.FromMilliseconds(90), retryEntries[2].Delay);
            }
        }

        // ============================================================
        // Summarize 截断间接测试 — 通过反序列化失败/状态码失败读取 ResponseSummary
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_InvalidJson_LongerThanLimit_TruncatesResponseSummary()
        {
            // Arrange — 构造 2000 字符的无效 JSON
            var longBody = new string('A', 2000);
            var handler = new QueueHandler();
            handler.EnqueueOK(longBody);
            using (var client = new HttpClient(handler))
            {
                // Act
                var exception = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.GetJsonAsync<SampleResponse>("http://example.com/api"));

                // Assert — Summarize 截断到 ResponseSummaryLimit(1024)
                Assert.NotNull(exception.ResponseSummary);
                Assert.Equal(1024, exception.ResponseSummary!.Length);
                Assert.Equal(longBody[..1024], exception.ResponseSummary);
            }
        }

        [Fact]
        public async Task GetJsonAsync_InvalidJson_ExactlyAtLimit_DoesNotTruncate()
        {
            // Arrange — 构造恰好 1024 字符的无效 JSON
            var exactBody = new string('A', 1024);
            var handler = new QueueHandler();
            handler.EnqueueOK(exactBody);
            using (var client = new HttpClient(handler))
            {
                // Act
                var exception = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.GetJsonAsync<SampleResponse>("http://example.com/api"));

                // Assert — 恰好 1024 字符不截断（Length &lt;= ResponseSummaryLimit）
                Assert.NotNull(exception.ResponseSummary);
                Assert.Equal(1024, exception.ResponseSummary!.Length);
                Assert.Equal(exactBody, exception.ResponseSummary);
            }
        }

        [Fact]
        public async Task GetJsonAsync_NonSuccess_LongBody_TruncatesResponseSummary()
        {
            // Arrange — 构造 1500 字符的响应体，配合非成功状态码
            var longBody = new string('B', 1500);
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.InternalServerError, longBody);
            using (var client = new HttpClient(handler))
            {
                // Act
                var exception = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.GetJsonAsync<SampleResponse>("http://example.com/api"));

                // Assert — CreateStatusException 也通过 Summarize 截断
                Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
                Assert.NotNull(exception.ResponseSummary);
                Assert.Equal(1024, exception.ResponseSummary!.Length);
                Assert.Equal(longBody[..1024], exception.ResponseSummary);
            }
        }

        [Fact]
        public async Task GetJsonAsync_EmptyBody_ResponseSummaryIsNull()
        {
            // Arrange
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.InternalServerError, string.Empty);
            using (var client = new HttpClient(handler))
            {
                // Act
                var exception = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.GetJsonAsync<SampleResponse>("http://example.com/api"));

                // Assert — Summarize 对空字符串直接返回原值
                Assert.Equal(string.Empty, exception.ResponseSummary);
            }
        }

        // ============================================================
        // HttpClientRequestOptions 边界
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_NullJsonSerializerOptions_DeserializesWithFrameworkDefaults()
        {
            // Arrange — 显式设为 null，应回退到框架默认（PascalCase 可匹配）
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"Name\":\"default\",\"Value\":42}");
            using (var client = new HttpClient(handler))
            {
                var options = new HttpClientRequestOptions
                {
                    JsonSerializerOptions = null!,
                };

                // Act
                var result = await client.GetJsonAsync<SampleResponse>("http://example.com/api", options);

                // Assert — null 不导致 NRE，使用框架默认 options 反序列化成功
                Assert.NotNull(result);
                Assert.Equal("default", result!.Name);
                Assert.Equal(42, result.Value);
            }
        }

        [Fact]
        public void RequestOptions_Headers_DuplicateAdd_ShouldThrowArgumentException()
        {
            // Arrange
            var options = new HttpClientRequestOptions();

            // Act & Assert — Dictionary<string,string>.Add 重复 key 抛 ArgumentException（.NET 标准契约）
            options.Headers.Add("Authorization", "Bearer first");
            Assert.Throws<ArgumentException>(() => options.Headers.Add("Authorization", "Bearer second"));
            // 第一次添加的值保留，Add 失败不修改字典
            Assert.Equal("Bearer first", options.Headers["Authorization"]);
        }

        [Fact]
        public void RequestOptions_Headers_Indexer_OverwritesValue()
        {
            // Arrange
            var options = new HttpClientRequestOptions();

            // Act — 索引器赋值是覆盖 header 的正确方式
            options.Headers["Authorization"] = "Bearer first";
            options.Headers["Authorization"] = "Bearer second";

            // Assert
            Assert.Single(options.Headers);
            Assert.Equal("Bearer second", options.Headers["Authorization"]);
        }

        // ============================================================
        // 辅助类型
        // ============================================================

        private sealed class SampleResponse
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        /// <summary>
        /// 队列式 handler：按入队顺序返回响应。
        /// </summary>
        private sealed class QueueHandler : HttpMessageHandler
        {
            private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responses =
                new Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>>();

            public int SendCount { get; private set; }

            public void EnqueueOK(string content)
            {
                Enqueue(HttpStatusCode.OK, content);
            }

            public void Enqueue(HttpStatusCode statusCode, string content)
            {
                _responses.Enqueue((_, _) =>
                    Task.FromResult(MakeResponse(statusCode, content)));
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                SendCount++;
                if (_responses.Count > 0)
                {
                    return _responses.Dequeue()(request, cancellationToken);
                }

                return Task.FromResult(MakeResponse(HttpStatusCode.OK, "{}"));
            }

            private static HttpResponseMessage MakeResponse(HttpStatusCode statusCode, string content)
            {
                return new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(content, Encoding.UTF8, "application/json"),
                };
            }
        }
    }
}
