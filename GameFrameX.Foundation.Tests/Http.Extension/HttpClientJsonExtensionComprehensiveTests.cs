using System.Net;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// HttpClientJsonExtension 的补充覆盖测试：
    /// - PutJsonAsync / PatchJsonAsync / DeleteJsonAsync / OptionsJsonAsync
    /// - SendJsonAsync&lt;TResponse&gt; 无 body 重载
    /// - RequestOptions.Timeout、OnRequest / OnResponse / OnRetry hooks
    /// - 自定义 RetryStatusCodes、BackoffFactor 退避计算
    /// - HttpRequestException 传输层异常重试
    /// - 参数 null 校验
    /// 与 HttpClientHighPriorityFeatureTests.cs 互补，不重复覆盖。
    /// </summary>
    public sealed class HttpClientJsonExtensionComprehensiveTests
    {
        // ============================================================
        // PutJsonAsync
        // ============================================================

        [Fact]
        public async Task PutJsonAsync_Success_SendsPutWithSerializedBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"updated\",\"value\":99}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.PutJsonAsync<SamplePayload, SampleResponse>(
                    "http://example.com/items/5",
                    new SamplePayload("payload"));

                Assert.NotNull(result);
                Assert.Equal("updated", result!.Name);
                Assert.Equal(99, result.Value);
                Assert.Equal(HttpMethod.Put, handler.Requests[0].Method);
                Assert.Contains("\"Name\":\"payload\"", handler.RequestBodies[0]);
            }
        }

        [Fact]
        public async Task PutJsonAsync_NonSuccess_ThrowsWithContext()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.Conflict, "{\"err\":\"conflict\"}");
            using (var client = new HttpClient(handler))
            {
                var ex = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.PutJsonAsync<SamplePayload, SampleResponse>(
                        "http://example.com/items/1",
                        new SamplePayload("p")));

                Assert.Equal(HttpStatusCode.Conflict, ex.StatusCode);
                Assert.Contains("conflict", ex.ResponseSummary);
            }
        }

        // ============================================================
        // PatchJsonAsync
        // ============================================================

        [Fact]
        public async Task PatchJsonAsync_Success_SendsPatchWithSerializedBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"patched\",\"value\":3}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.PatchJsonAsync<SamplePayload, SampleResponse>(
                    "http://example.com/items/9",
                    new SamplePayload("change"));

                Assert.NotNull(result);
                Assert.Equal("patched", result!.Name);
                Assert.Equal(3, result.Value);
                Assert.Equal(HttpMethod.Patch, handler.Requests[0].Method);
            }
        }

        // ============================================================
        // DeleteJsonAsync
        // ============================================================

        [Fact]
        public async Task DeleteJsonAsync_Success_SendsDeleteMethod()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"gone\",\"value\":0}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.DeleteJsonAsync<SampleResponse>("http://example.com/items/3");

                Assert.NotNull(result);
                Assert.Equal("gone", result!.Name);
                Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
            }
        }

        [Fact]
        public async Task DeleteJsonAsync_NoContent_ReturnsDefault()
        {
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.NoContent, string.Empty);
            using (var client = new HttpClient(handler))
            {
                var result = await client.DeleteJsonAsync<SampleResponse>("http://example.com/items/3");

                Assert.Null(result);
            }
        }

        // ============================================================
        // OptionsJsonAsync
        // ============================================================

        [Fact]
        public async Task OptionsJsonAsync_Success_SendsOptionsMethod()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"opts\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.OptionsJsonAsync<SampleResponse>("http://example.com/items");

                Assert.NotNull(result);
                Assert.Equal("opts", result!.Name);
                Assert.Equal(HttpMethod.Options, handler.Requests[0].Method);
            }
        }

        // ============================================================
        // SendJsonAsync<TResponse> 无 body 重载
        // ============================================================

        [Fact]
        public async Task SendJsonAsync_NoBody_Get_SendsGetWithoutContent()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"r\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.SendJsonAsync<SampleResponse>(
                    HttpMethod.Get, "http://example.com/api");

                Assert.NotNull(result);
                Assert.Equal("r", result!.Name);
                Assert.Null(handler.Requests[0].Content);
            }
        }

        [Fact]
        public async Task SendJsonAsync_NoBody_Delete_SendsDeleteWithoutContent()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"d\",\"value\":2}");
            using (var client = new HttpClient(handler))
            {
                await client.SendJsonAsync<SampleResponse>(
                    HttpMethod.Delete, "http://example.com/api");

                Assert.Equal(HttpMethod.Delete, handler.Requests[0].Method);
                Assert.Null(handler.Requests[0].Content);
            }
        }

        // ============================================================
        // RequestOptions.Timeout
        // ============================================================

        [Fact]
        public async Task SendJsonAsync_Timeout_TriggersCancellation()
        {
            var handler = new QueueHandler();
            handler.EnqueueDelay = TimeSpan.FromMilliseconds(500);
            handler.EnqueueOK("{\"name\":\"slow\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                    client.SendJsonAsync<SampleResponse>(
                        HttpMethod.Get,
                        "http://example.com/slow",
                        new HttpClientRequestOptions
                        {
                            Timeout = TimeSpan.FromMilliseconds(50),
                        }));
            }
        }

        // ============================================================
        // OnRequest / OnResponse hooks
        // ============================================================

        [Fact]
        public async Task SendJsonAsync_OnRequestAndOnResponse_InvokedForEachAttempt()
        {
            var requestEntries = new List<HttpClientRequestLogEntry>();
            var responseEntries = new List<HttpClientResponseLogEntry>();
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"ok\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                await client.SendJsonAsync<SampleResponse>(
                    HttpMethod.Get,
                    "http://example.com/api",
                    new HttpClientRequestOptions
                    {
                        OnRequest = requestEntries.Add,
                        OnResponse = responseEntries.Add,
                    });

                Assert.Single(requestEntries);
                Assert.Equal(1, requestEntries[0].Attempt);
                Assert.Equal(HttpMethod.Get, requestEntries[0].Method);
                Assert.Single(responseEntries);
                Assert.Equal(HttpStatusCode.OK, responseEntries[0].StatusCode);
                Assert.Equal(1, responseEntries[0].Attempt);
            }
        }

        [Fact]
        public async Task GetJsonAsync_OnResponse_RecordsFailureStatus()
        {
            var responseEntries = new List<HttpClientResponseLogEntry>();
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.InternalServerError, "{}");
            using (var client = new HttpClient(handler))
            {
                await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                    client.GetJsonAsync<SampleResponse>(
                        "http://example.com/api",
                        new HttpClientRequestOptions { OnResponse = responseEntries.Add }));

                Assert.Single(responseEntries);
                Assert.Equal(HttpStatusCode.InternalServerError, responseEntries[0].StatusCode);
            }
        }

        // ============================================================
        // 自定义 RetryStatusCodes
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_CustomRetryStatusCode_RetriesOnCustomCode()
        {
            var retryEntries = new List<HttpClientRetryLogEntry>();
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.NotFound, "{\"err\":\"miss\"}");
            handler.EnqueueOK("{\"name\":\"found\",\"value\":1}");
            var retry = new HttpClientRetryOptions
            {
                MaxRetries = 1,
                BaseDelay = TimeSpan.FromMilliseconds(1),
                OnRetry = retryEntries.Add,
            };
            retry.RetryStatusCodes.Add(HttpStatusCode.NotFound);
            using (var client = new HttpClient(handler))
            {
                var result = await client.GetJsonAsync<SampleResponse>(
                    "http://example.com/maybe",
                    new HttpClientRequestOptions { Retry = retry });

                Assert.NotNull(result);
                Assert.Equal("found", result!.Name);
                Assert.Equal(2, handler.SendCount);
                Assert.Single(retryEntries);
                Assert.Equal(HttpStatusCode.NotFound, retryEntries[0].StatusCode);
            }
        }

        // ============================================================
        // BackoffFactor 退避计算
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_BackoffFactor_ComputesExponentialDelay()
        {
            var retryEntries = new List<HttpClientRetryLogEntry>();
            var handler = new QueueHandler();
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{}");
            handler.EnqueueOK("{\"name\":\"ok\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.GetJsonAsync<SampleResponse>(
                    "http://example.com/backoff",
                    new HttpClientRequestOptions
                    {
                        Retry = new HttpClientRetryOptions
                        {
                            MaxRetries = 2,
                            BaseDelay = TimeSpan.FromMilliseconds(10),
                            BackoffFactor = 3D,
                            OnRetry = retryEntries.Add,
                        },
                    });

                Assert.NotNull(result);
                Assert.Equal(2, retryEntries.Count);
                // 第一次重试：BaseDelay * 3^0 = 10ms
                Assert.Equal(TimeSpan.FromMilliseconds(10), retryEntries[0].Delay);
                // 第二次重试：BaseDelay * 3^1 = 30ms
                Assert.Equal(TimeSpan.FromMilliseconds(30), retryEntries[1].Delay);
            }
        }

        // ============================================================
        // HttpRequestException 传输层异常重试
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_TransportException_RetriesIdempotentMethod()
        {
            var handler = new QueueHandler();
            handler.EnqueueThrow(new HttpRequestException("connection reset"));
            handler.EnqueueOK("{\"name\":\"recovered\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.GetJsonAsync<SampleResponse>(
                    "http://example.com/flaky",
                    new HttpClientRequestOptions
                    {
                        Retry = new HttpClientRetryOptions
                        {
                            MaxRetries = 1,
                            BaseDelay = TimeSpan.FromMilliseconds(1),
                        },
                    });

                Assert.NotNull(result);
                Assert.Equal("recovered", result!.Name);
                Assert.Equal(2, handler.SendCount);
            }
        }

        // ============================================================
        // 参数 null 校验
        // ============================================================

        [Fact]
        public async Task SendJsonAsync_NoBody_NullClient_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null!).SendJsonAsync<SampleResponse>(
                    HttpMethod.Get, "http://x.com"));
        }

        [Fact]
        public async Task SendJsonAsync_NoBody_NullMethod_ThrowsArgumentNullException()
        {
            using (var client = new HttpClient(new QueueHandler()))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    client.SendJsonAsync<SampleResponse>(null!, "http://x.com"));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SendJsonAsync_NoBody_BlankUrl_ThrowsArgumentException(string url)
        {
            using (var client = new HttpClient(new QueueHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.SendJsonAsync<SampleResponse>(HttpMethod.Get, url));
            }
        }

        [Fact]
        public async Task SendJsonAsync_WithBody_NullClient_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null!).SendJsonAsync<SamplePayload, SampleResponse>(
                    HttpMethod.Post, "http://x.com", new SamplePayload("p")));
        }

        [Fact]
        public async Task SendJsonAsync_WithBody_NullMethod_ThrowsArgumentNullException()
        {
            using (var client = new HttpClient(new QueueHandler()))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    client.SendJsonAsync<SamplePayload, SampleResponse>(
                        null!, "http://x.com", new SamplePayload("p")));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task SendJsonAsync_WithBody_BlankUrl_ThrowsArgumentException(string url)
        {
            using (var client = new HttpClient(new QueueHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.SendJsonAsync<SamplePayload, SampleResponse>(
                        HttpMethod.Post, url, new SamplePayload("p")));
            }
        }

        [Fact]
        public async Task GetJsonAsync_NullClient_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null!).GetJsonAsync<SampleResponse>("http://x.com"));
        }

        // ============================================================
        // GetJsonAsync 默认 options(null 时使用默认值)
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_NullOptions_UsesDefaultsAndStillWorks()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"name\":\"default\",\"value\":1}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.GetJsonAsync<SampleResponse>(
                    "http://example.com/api",
                    options: null);

                Assert.NotNull(result);
                Assert.Equal("default", result!.Name);
            }
        }

        // ============================================================
        // 自定义 JsonSerializerOptions 影响反序列化
        // ============================================================

        [Fact]
        public async Task GetJsonAsync_CustomJsonSerializerOptions_AppliesToDeserialization()
        {
            var handler = new QueueHandler();
            // camelCase 序列化的 JSON
            handler.EnqueueOK("{\"name\":\"camel\",\"value\":7}");
            using (var client = new HttpClient(handler))
            {
                var result = await client.GetJsonAsync<SampleResponse>(
                    "http://example.com/api",
                    new HttpClientRequestOptions
                    {
                        JsonSerializerOptions = new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        },
                    });

                Assert.NotNull(result);
                Assert.Equal("camel", result!.Name);
                Assert.Equal(7, result.Value);
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
        /// 队列式 handler：按入队顺序返回响应，支持延迟和异常抛出。
        /// </summary>
        private sealed class QueueHandler : HttpMessageHandler
        {
            private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responses =
                new Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>>();

            public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();
            public List<string> RequestBodies { get; } = new List<string>();
            public int SendCount { get; private set; }
            public TimeSpan? EnqueueDelay { get; set; }

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

                if (EnqueueDelay.HasValue)
                {
                    await Task.Delay(EnqueueDelay.Value, cancellationToken);
                }

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
