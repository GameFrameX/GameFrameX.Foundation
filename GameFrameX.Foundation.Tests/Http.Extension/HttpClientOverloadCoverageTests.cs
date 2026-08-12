using System.Net;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// 补充覆盖 HttpClient 各扩展中未覆盖的重载变体：
    /// - 带 JsonSerializerOptions 参数的 POST 重载
    /// - 带 headers 的流式方法(GetToStreamAsync/PostJsonToStreamAsync/PutJsonToStreamAsync/PatchJsonToStreamAsync with headers)
    /// - timeout 边界(0 / 负数 → ArgumentOutOfRangeException)，间接覆盖 HttpClientExtensionHelper.CreateTimeoutTokenSource
    /// - JSON 请求体的 Content-Type 验证
    /// - 请求 URL 透传
    /// </summary>
    public sealed class HttpClientOverloadCoverageTests
    {
        // ============================================================
        // JsonSerializerOptions 重载 — 验证自定义 options 影响请求体序列化
        // ============================================================

        [Fact]
        public async Task PostJsonToStringAsync_WithJsonSerializerOptions_AppliesCamelCaseToBody()
        {
            var handler = new BodyCapturingHandler();
            using (var client = new HttpClient(handler))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                await client.PostJsonToStringAsync("http://test.com", new SamplePayload("alpha"), options);

                Assert.NotNull(handler.CapturedBody);
                Assert.Contains("\"name\":\"alpha\"", handler.CapturedBody);
            }
        }

        [Fact]
        public async Task PostJsonToByteArrayAsync_WithJsonSerializerOptions_ReturnsByteArray()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "byte-out" };
            using (var client = new HttpClient(handler))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var result = await client.PostJsonToByteArrayAsync(
                    "http://test.com", new SamplePayload("p"), options);

                Assert.Equal("byte-out", Encoding.UTF8.GetString(result));
            }
        }

        [Fact]
        public async Task PostJsonToStreamAsync_WithJsonSerializerOptions_ReturnsReadableStream()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "json-stream" };
            using (var client = new HttpClient(handler))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                await using (var stream = await client.PostJsonToStreamAsync(
                    "http://test.com", new SamplePayload("s"), options))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        Assert.Equal("json-stream", await reader.ReadToEndAsync());
                    }
                }
            }
        }

        [Fact]
        public async Task PostJsonToStringAsync_WithHeadersAndJsonSerializerOptions_AppliesBoth()
        {
            var handler = new BodyCapturingHandler();
            using (var client = new HttpClient(handler))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                await client.PostJsonToStringAsync(
                    "http://test.com",
                    new SamplePayload("dual"),
                    new Dictionary<string, string> { { "X-Mode", "custom-options" } },
                    options);

                Assert.Contains("\"name\":\"dual\"", handler.CapturedBody);
                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Mode"));
            }
        }

        [Fact]
        public async Task PostJsonToByteArrayAsync_WithHeadersAndJsonSerializerOptions_ReturnsBytes()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "ok" };
            using (var client = new HttpClient(handler))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                var result = await client.PostJsonToByteArrayAsync(
                    "http://test.com",
                    new SamplePayload("h"),
                    new Dictionary<string, string> { { "X-Hdr", "v" } },
                    options);

                Assert.Equal("ok", Encoding.UTF8.GetString(result));
                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Hdr"));
            }
        }

        [Fact]
        public async Task PostJsonToStreamAsync_WithHeadersAndJsonSerializerOptions_ReturnsStream()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "s-out" };
            using (var client = new HttpClient(handler))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };

                await using (var stream = await client.PostJsonToStreamAsync(
                    "http://test.com",
                    new SamplePayload("h"),
                    new Dictionary<string, string> { { "X-S", "1" } },
                    options))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        Assert.Equal("s-out", await reader.ReadToEndAsync());
                    }
                }

                Assert.True(handler.CapturedRequest!.Headers.Contains("X-S"));
            }
        }

        // ============================================================
        // JSON 请求体的 Content-Type 验证
        // ============================================================

        [Fact]
        public async Task PostJsonToStringAsync_SetsApplicationJsonContentType()
        {
            var handler = new BodyCapturingHandler();
            using (var client = new HttpClient(handler))
            {
                await client.PostJsonToStringAsync("http://test.com", new SamplePayload("ct"));

                Assert.NotNull(handler.CapturedRequest!.Content);
                Assert.Equal("application/json",
                    handler.CapturedRequest.Content!.Headers.ContentType!.MediaType);
            }
        }

        [Fact]
        public async Task PutJsonToStringAsync_SetsApplicationJsonContentType()
        {
            var handler = new BodyCapturingHandler();
            using (var client = new HttpClient(handler))
            {
                await client.PutJsonToStringAsync("http://test.com", new SamplePayload("put-ct"));

                Assert.Equal("application/json",
                    handler.CapturedRequest!.Content!.Headers.ContentType!.MediaType);
            }
        }

        [Fact]
        public async Task PatchJsonToStringAsync_SetsApplicationJsonContentType()
        {
            var handler = new BodyCapturingHandler();
            using (var client = new HttpClient(handler))
            {
                await client.PatchJsonToStringAsync("http://test.com", new SamplePayload("patch-ct"));

                Assert.Equal("application/json",
                    handler.CapturedRequest!.Content!.Headers.ContentType!.MediaType);
            }
        }

        // ============================================================
        // 请求 URL 透传
        // ============================================================

        [Fact]
        public async Task GetToStringAsync_PreservesRequestUri()
        {
            var handler = new BodyCapturingHandler();
            using (var client = new HttpClient(handler))
            {
                await client.GetToStringAsync("http://example.org/api/items?page=1&size=10");

                Assert.Equal("http://example.org/api/items?page=1&size=10",
                    handler.CapturedRequest!.RequestUri!.ToString());
            }
        }

        // ============================================================
        // 带 headers 的流式方法
        // ============================================================

        [Fact]
        public async Task GetToStreamAsync_WithHeaders_SetsRequestHeadersAndReturnsStream()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "gh-stream" };
            using (var client = new HttpClient(handler))
            {
                await using (var stream = await client.GetToStreamAsync(
                    "http://test.com",
                    new Dictionary<string, string> { { "X-Range", "0-100" } }))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        Assert.Equal("gh-stream", await reader.ReadToEndAsync());
                    }
                }

                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Range"));
            }
        }

        [Fact]
        public async Task PostJsonToStreamAsync_WithHeaders_SendsPostWithHeaders()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "ps" };
            using (var client = new HttpClient(handler))
            {
                await using (var stream = await client.PostJsonToStreamAsync(
                    "http://test.com",
                    new SamplePayload("p"),
                    new Dictionary<string, string> { { "Authorization", "Bearer t" } }))
                {
                    Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
                }

                Assert.True(handler.CapturedRequest!.Headers.Contains("Authorization"));
            }
        }

        [Fact]
        public async Task PutJsonToStreamAsync_WithHeaders_SendsPutWithHeaders()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "pus" };
            using (var client = new HttpClient(handler))
            {
                await using (var stream = await client.PutJsonToStreamAsync(
                    "http://test.com",
                    new SamplePayload("p"),
                    new Dictionary<string, string> { { "X-Put", "1" } }))
                {
                    Assert.Equal(HttpMethod.Put, handler.CapturedRequest!.Method);
                }

                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Put"));
            }
        }

        [Fact]
        public async Task PatchJsonToStreamAsync_WithHeaders_SendsPatchWithHeaders()
        {
            var handler = new BodyCapturingHandler { ResponseContent = "pas" };
            using (var client = new HttpClient(handler))
            {
                await using (var stream = await client.PatchJsonToStreamAsync(
                    "http://test.com",
                    new SamplePayload("p"),
                    new Dictionary<string, string> { { "X-Patch", "1" } }))
                {
                    Assert.Equal(HttpMethod.Patch, handler.CapturedRequest!.Method);
                }

                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Patch"));
            }
        }

        // ============================================================
        // timeout 边界 — 间接测试 HttpClientExtensionHelper.CreateTimeoutTokenSource
        // ============================================================

        [Fact]
        public async Task GetToStringAsync_TimeoutZero_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.GetToStringAsync(
                        "http://test.com",
                        new Dictionary<string, string>(),
                        timeout: 0));
            }
        }

        [Fact]
        public async Task GetToStringAsync_TimeoutNegative_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.GetToStringAsync(
                        "http://test.com",
                        new Dictionary<string, string>(),
                        timeout: -5));
            }
        }

        [Fact]
        public async Task PostJsonToStringAsync_TimeoutZero_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.PostJsonToStringAsync(
                        "http://test.com",
                        new SamplePayload("x"),
                        new Dictionary<string, string>(),
                        timeout: 0));
            }
        }

        [Fact]
        public async Task PutJsonToStringAsync_TimeoutNegative_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.PutJsonToStringAsync(
                        "http://test.com",
                        new SamplePayload("x"),
                        new Dictionary<string, string>(),
                        timeout: -1));
            }
        }

        [Fact]
        public async Task DeleteToStringAsync_TimeoutZero_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.DeleteToStringAsync(
                        "http://test.com",
                        new Dictionary<string, string>(),
                        timeout: 0));
            }
        }

        [Fact]
        public async Task HeadAsync_TimeoutNegative_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.HeadAsync(
                        "http://test.com",
                        new Dictionary<string, string>(),
                        timeout: -3));
            }
        }

        [Fact]
        public async Task OptionsAsync_TimeoutZero_ThrowsArgumentOutOfRangeException()
        {
            using (var client = new HttpClient(new BodyCapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.OptionsAsync(
                        "http://test.com",
                        new Dictionary<string, string>(),
                        timeout: 0));
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

        /// <summary>
        /// 能读取请求体(body)的 mock handler，同时记录 method/headers。
        /// </summary>
        private sealed class BodyCapturingHandler : HttpMessageHandler
        {
            public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
            public string ResponseContent { get; set; } = string.Empty;
            public HttpRequestMessage? CapturedRequest { get; private set; }
            public string? CapturedBody { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CapturedRequest = request;
                CapturedBody = request.Content is null
                    ? null
                    : await request.Content.ReadAsStringAsync(cancellationToken);
                return new HttpResponseMessage(StatusCode)
                {
                    Content = new StringContent(ResponseContent, Encoding.UTF8, "application/json"),
                };
            }
        }
    }
}
