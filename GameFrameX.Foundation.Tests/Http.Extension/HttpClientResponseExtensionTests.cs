using System.Net;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// HttpClientResponse 系列扩展方法测试（SendResponseAsync / GetResponseAsync / PostResponseAsync 等）。
    /// 验证：非 2xx 不抛、EnsureSuccessStatusCode、As&lt;T&gt; 反序列化、重试共享、headers 收集、
    /// PATCH 非幂等不重试、传输异常重试耗尽后包装为 HttpClientRequestException。
    /// </summary>
    public sealed class HttpClientResponseExtensionTests
    {
        [Fact]
        public async Task GetResponseAsync_Success_ReturnsBodyAndStatus()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"ok\",\"value\":7}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(HttpMethod.Get, response.Method);
                Assert.Equal(1, response.Attempt);
                Assert.Contains("ok", response.Body);
                Assert.Contains("7", response.Body);
            }
        }

        [Fact]
        public async Task GetResponseAsync_NonSuccess_DoesNotThrow()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.NotFound, "{\"err\":\"missing\"}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.Contains("missing", response.Body);
            }
        }

        [Fact]
        public async Task SendResponseAsync_UsesProvidedHttpMethod()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.SendResponseAsync(HttpMethod.Head, "http://example.com/data");

                Assert.Equal(HttpMethod.Head, handler.Requests[0].Method);
                Assert.True(response.IsSuccessStatusCode);
            }
        }

        [Fact]
        public async Task EnsureSuccessStatusCode_OnSuccess_ReturnsSameInstance()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                var ensured = response.EnsureSuccessStatusCode();
                Assert.Same(response, ensured);
            }
        }

        [Fact]
        public async Task EnsureSuccessStatusCode_OnFailure_ThrowsWithContext()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.Conflict, "{\"err\":\"conflict\"}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                var ex = Assert.Throws<HttpClientRequestException>(() => response.EnsureSuccessStatusCode());
                Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
                Assert.Contains("conflict", ex.ResponseSummary);
            }
        }

        [Fact]
        public async Task As_DeserializesBodyToJson()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"alice\",\"value\":42}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                var parsed = response.As<SampleResponse>();
                Assert.NotNull(parsed);
                Assert.Equal("alice", parsed!.Name);
                Assert.Equal(42, parsed.Value);
            }
        }

        [Fact]
        public async Task As_EmptyBody_ReturnsDefault()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK(string.Empty);
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                Assert.True(response.IsSuccessStatusCode);
                var parsed = response.As<SampleResponse>();
                Assert.Null(parsed);
            }
        }

        [Fact]
        public async Task GetResponseAsync_RetriesOn5xx_ThenSucceeds()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.InternalServerError, "{}");
            handler.EnqueueOK("{\"name\":\"recovered\",\"value\":1}");
            var options = new HttpClientRequestOptions
            {
                Retry = { MaxRetries = 2, BaseDelay = TimeSpan.FromMilliseconds(1) }
            };
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data", options);

                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(2, response.Attempt);
                Assert.Equal(2, handler.SendCount);
                Assert.Contains("recovered", response.Body);
            }
        }

        [Fact]
        public async Task GetResponseAsync_HeadersIncludeContentHeaders()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetResponseAsync("http://example.com/data");

                // Content-Type 来自 content headers，应被合并收集
                Assert.True(response.Headers.ContainsKey("Content-Type"));
            }
        }

        [Fact]
        public async Task PostResponseAsync_SendsSerializedBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"echo\",\"value\":5}");
            using (var client = new HttpClient(handler))
            {
                var response = await client.PostResponseAsync("http://example.com/data", new SamplePayload("payload"));

                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
                Assert.Contains("payload", handler.RequestBodies[0]);
            }
        }

        [Fact]
        public async Task PatchResponseAsync_NonSuccess_DoesNotThrow_AndDoesNotRetryByDefault()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.InternalServerError, "{\"err\":\"boom\"}");
            var options = new HttpClientRequestOptions
            {
                Retry = { MaxRetries = 3, BaseDelay = TimeSpan.FromMilliseconds(1) }
            };
            using (var client = new HttpClient(handler))
            {
                var response = await client.PatchResponseAsync(
                    "http://example.com/data", new SamplePayload("p"), options);

                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                Assert.Equal(1, handler.SendCount); // PATCH 非幂等，即使配了 retry 也不重试
                Assert.Contains("boom", response.Body);
            }
        }

        [Fact]
        public async Task PatchResponseAsync_RetriesWhenNonIdempotentRetryAllowed()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.InternalServerError, "{}");
            handler.EnqueueOK("{\"name\":\"ok\",\"value\":1}");
            var options = new HttpClientRequestOptions
            {
                Retry = { MaxRetries = 2, BaseDelay = TimeSpan.FromMilliseconds(1), AllowNonIdempotentRetry = true }
            };
            using (var client = new HttpClient(handler))
            {
                var response = await client.PatchResponseAsync(
                    "http://example.com/data", new SamplePayload("p"), options);

                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(2, response.Attempt);
                Assert.Equal(2, handler.SendCount);
            }
        }

        [Fact]
        public async Task GetResponseAsync_TransportFailureRetriedThenWrapped()
        {
            var handler = new QueueHandler();
            handler.EnqueueThrow(new HttpRequestException("connection refused"));
            handler.EnqueueThrow(new HttpRequestException("connection refused"));
            var options = new HttpClientRequestOptions
            {
                Retry = { MaxRetries = 1, BaseDelay = TimeSpan.FromMilliseconds(1) }
            };
            using (var client = new HttpClient(handler))
            {
                var ex = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.GetResponseAsync("http://example.com/data", options));

                Assert.Equal(2, handler.SendCount); // 初始 1 + 重试 1
                Assert.IsType<HttpRequestException>(ex.RawException);
            }
        }

        // ============================================================
        // 辅助类型
        // ============================================================

        private sealed class SamplePayload
        {
            public SamplePayload(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        private sealed class SampleResponse
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        /// <summary>
        /// 队列式 handler：按入队顺序返回响应，支持异常抛出。
        /// </summary>
        private sealed class QueueHandler : HttpMessageHandler
        {
            private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responses =
                new Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>>();

            public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();
            public List<string> RequestBodies { get; } = new List<string>();
            public int SendCount { get; private set; }

            public void EnqueueOK(string content)
            {
                Enqueue(HttpStatusCode.OK, content);
            }

            public void Enqueue(HttpStatusCode statusCode, string content)
            {
                _responses.Enqueue((request, token) => Task.FromResult(MakeResponse(statusCode, content)));
            }

            public void EnqueueThrow(Exception exception)
            {
                _responses.Enqueue((request, token) => Task.FromException<HttpResponseMessage>(exception));
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                SendCount++;
                Requests.Add(request);
                RequestBodies.Add(request.Content is null
                    ? string.Empty
                    : await request.Content.ReadAsStringAsync(cancellationToken));

                if (_responses.Count > 0)
                {
                    return await _responses.Dequeue()(request, cancellationToken);
                }

                return MakeResponse(HttpStatusCode.OK, "{}");
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
