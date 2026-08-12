using System.Net;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// 第一代扩展方法补齐的对称重载测试：
    /// - PUT/PATCH 的 JsonSerializerOptions 重载（对齐 POST 的 4 重载模式）
    /// - DELETE 的 ToStreamAsync（对齐 GET/POST/PUT/PATCH 的三种返回类型）
    /// 验证：传入的 JsonSerializerOptions 生效（CamelCase 改变请求体属性名）、Stream 重载返回可读流。
    /// DefaultOptions 的 PropertyNamingPolicy = null（PascalCase），故 CamelCase 产生可观察差异。
    /// </summary>
    public sealed class HttpClientFirstGenAdditionalOverloadsTests
    {
        private static readonly JsonSerializerOptions CamelCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Fact]
        public async Task PutJsonToStringAsync_WithJsonSerializerOptions_AppliesCamelCaseToBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            using (var client = new HttpClient(handler))
            {
                await client.PutJsonToStringAsync(
                    "http://example.com/x", new SamplePayload("Alice", 7), CamelCase, default);

                Assert.Contains("\"userName\":\"Alice\"", handler.RequestBodies[0]);
                Assert.Contains("\"userValue\":7", handler.RequestBodies[0]);
            }
        }

        [Fact]
        public async Task PutJsonToStringAsync_WithHeadersAndJsonSerializerOptions_AppliesCamelCaseToBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            var headers = new Dictionary<string, string> { ["X-Test"] = "v" };
            using (var client = new HttpClient(handler))
            {
                await client.PutJsonToStringAsync(
                    "http://example.com/x", new SamplePayload("Bob", 9), headers, CamelCase, 10, default);

                Assert.Contains("\"userName\":\"Bob\"", handler.RequestBodies[0]);
                Assert.Contains("X-Test", handler.Requests[0].Headers.Select(h => h.Key));
            }
        }

        [Fact]
        public async Task PatchJsonToStringAsync_WithJsonSerializerOptions_AppliesCamelCaseToBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            using (var client = new HttpClient(handler))
            {
                await client.PatchJsonToStringAsync(
                    "http://example.com/x", new SamplePayload("Carol", 3), CamelCase, default);

                Assert.Equal("PATCH", handler.Requests[0].Method.Method);
                Assert.Contains("\"userName\":\"Carol\"", handler.RequestBodies[0]);
            }
        }

        [Fact]
        public async Task PatchJsonToStringAsync_WithHeadersAndJsonSerializerOptions_AppliesCamelCaseToBody()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{}");
            var headers = new Dictionary<string, string> { ["If-Match"] = "\"v1\"" };
            using (var client = new HttpClient(handler))
            {
                await client.PatchJsonToStringAsync(
                    "http://example.com/x", new SamplePayload("Dan", 5), headers, CamelCase, 10, default);

                Assert.Contains("\"userName\":\"Dan\"", handler.RequestBodies[0]);
            }
        }

        [Fact]
        public async Task PutJsonToByteArrayAsync_WithJsonSerializerOptions_AppliesCamelCaseAndReturnsBytes()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"ok\":true}");
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.PutJsonToByteArrayAsync(
                    "http://example.com/x", new SamplePayload("Eve", 1), CamelCase, default);

                Assert.Contains("\"userName\":\"Eve\"", handler.RequestBodies[0]);
                Assert.Contains("ok", Encoding.UTF8.GetString(bytes));
            }
        }

        [Fact]
        public async Task PatchJsonToByteArrayAsync_WithHeadersAndJsonSerializerOptions_ReturnsBytes()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"ok\":true}");
            var headers = new Dictionary<string, string> { ["X-Test"] = "v" };
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.PatchJsonToByteArrayAsync(
                    "http://example.com/x", new SamplePayload("Fay", 2), headers, CamelCase, 10, default);

                Assert.Contains("\"userName\":\"Fay\"", handler.RequestBodies[0]);
                Assert.NotEmpty(bytes);
            }
        }

        [Fact]
        public async Task PutJsonToStreamAsync_WithJsonSerializerOptions_ReturnsReadableStream()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"streamed\":true}");
            using (var client = new HttpClient(handler))
            {
                using (var stream = await client.PutJsonToStreamAsync(
                    "http://example.com/x", new SamplePayload("Gus", 4), CamelCase, default))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var text = await reader.ReadToEndAsync();
                        Assert.Contains("streamed", text);
                    }
                }

                Assert.Contains("\"userName\":\"Gus\"", handler.RequestBodies[0]);
            }
        }

        [Fact]
        public async Task PatchJsonToStreamAsync_WithHeadersAndJsonSerializerOptions_ReturnsReadableStream()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"streamed\":true}");
            var headers = new Dictionary<string, string> { ["X-Test"] = "v" };
            using (var client = new HttpClient(handler))
            {
                using (var stream = await client.PatchJsonToStreamAsync(
                    "http://example.com/x", new SamplePayload("Hex", 6), headers, CamelCase, 10, default))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var text = await reader.ReadToEndAsync();
                        Assert.Contains("streamed", text);
                    }
                }
            }
        }

        [Fact]
        public async Task DeleteToStreamAsync_ReturnsReadableStream()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"deleted\":true}");
            using (var client = new HttpClient(handler))
            {
                using (var stream = await client.DeleteToStreamAsync("http://example.com/x", default))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var text = await reader.ReadToEndAsync();
                        Assert.Contains("deleted", text);
                    }
                }

                Assert.Equal("DELETE", handler.Requests[0].Method.Method);
            }
        }

        [Fact]
        public async Task DeleteToStreamAsync_WithHeaders_ReturnsReadableStream()
        {
            var handler = new QueueHandler();
            handler.EnqueueOK("{\"deleted\":true}");
            var headers = new Dictionary<string, string> { ["X-Cascade"] = "true" };
            using (var client = new HttpClient(handler))
            {
                using (var stream = await client.DeleteToStreamAsync(
                    "http://example.com/x", headers, 10, default))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var text = await reader.ReadToEndAsync();
                        Assert.Contains("deleted", text);
                    }
                }

                Assert.Contains("X-Cascade", handler.Requests[0].Headers.Select(h => h.Key));
            }
        }

        private sealed class SamplePayload
        {
            public SamplePayload(string userName, int userValue)
            {
                UserName = userName;
                UserValue = userValue;
            }

            public string UserName { get; }

            public int UserValue { get; }
        }

        private sealed class QueueHandler : HttpMessageHandler
        {
            private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responses =
                new Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>>();

            public List<HttpRequestMessage> Requests { get; } = new List<HttpRequestMessage>();

            public List<string> RequestBodies { get; } = new List<string>();

            public void EnqueueOK(string content)
            {
                Enqueue(HttpStatusCode.OK, content);
            }

            public void Enqueue(HttpStatusCode statusCode, string content)
            {
                _responses.Enqueue((request, token) => Task.FromResult(MakeResponse(statusCode, content)));
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
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
