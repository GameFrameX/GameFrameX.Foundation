using System.Net;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// HttpClientOptionsExtension 的补充边界测试：
    /// 专注带 headers 重载的 null/空白参数、null headers 容错、空 headers 容错、
    /// 多 header 应用、负数 timeout、非成功状态等未被现有测试覆盖的路径。
    ///
    /// public 符号签名（共 2 个 public 方法，CreateOptionsRequest 为 private）：
    /// - OptionsAsync(HttpClient, string, CancellationToken)
    /// - OptionsAsync(HttpClient, string, IDictionary&lt;string,string&gt;, int, CancellationToken)
    /// </summary>
    public sealed class HttpClientOptionsExtensionAdditionalTests
    {
        // ============================================================
        // 带 headers 重载 — null/blank 参数校验
        // ============================================================

        [Fact]
        public async Task OptionsAsync_WithHeaders_NullClient_ThrowsArgumentNullException()
        {
            // Act & Assert — 扩展方法首行 ArgumentNullException.ThrowIfNull 抛出
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null!).OptionsAsync(
                    "http://test.com",
                    new Dictionary<string, string>()));
        }

        [Fact]
        public async Task OptionsAsync_WithHeaders_NullUrl_ThrowsArgumentNullException()
        {
            // Arrange
            using (var client = new HttpClient(new OptionsHandler()))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    client.OptionsAsync(
                        null!,
                        new Dictionary<string, string>()));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task OptionsAsync_WithHeaders_BlankUrl_ThrowsArgumentException(string url)
        {
            // Arrange
            using (var client = new HttpClient(new OptionsHandler()))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.OptionsAsync(
                        url,
                        new Dictionary<string, string>()));
            }
        }

        // ============================================================
        // 带 headers 重载 — null/空 headers 容错
        // ============================================================

        [Fact]
        public async Task OptionsAsync_WithHeaders_NullHeaders_SucceedsAndSendsRequest()
        {
            // Arrange
            var handler = new OptionsHandler
            {
                AllowedMethods = new[] { "GET", "POST" },
            };
            using (var client = new HttpClient(handler))
            {
                // Act — null headers 不应崩溃（CreateOptionsRequest 用 is { Count: > 0 } 安全匹配）
                var result = await client.OptionsAsync("http://test.com", null!);

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains("GET", result);
                Assert.Contains("POST", result);
                Assert.Equal(HttpMethod.Options, handler.CapturedRequest!.Method);
            }
        }

        [Fact]
        public async Task OptionsAsync_WithHeaders_EmptyHeaders_SucceedsAndSendsRequest()
        {
            // Arrange
            var handler = new OptionsHandler
            {
                AllowedMethods = new[] { "GET" },
            };
            using (var client = new HttpClient(handler))
            {
                // Act
                var result = await client.OptionsAsync(
                    "http://test.com",
                    new Dictionary<string, string>());

                // Assert
                Assert.Single(result);
                Assert.Contains("GET", result);
            }
        }

        // ============================================================
        // 带 headers 重载 — 成功/失败响应
        // ============================================================

        [Fact]
        public async Task OptionsAsync_WithHeaders_Success_ReturnsAllowedMethods()
        {
            // Arrange
            var handler = new OptionsHandler
            {
                AllowedMethods = new[] { "GET", "POST", "PUT", "DELETE" },
            };
            using (var client = new HttpClient(handler))
            {
                // Act
                var result = await client.OptionsAsync(
                    "http://test.com",
                    new Dictionary<string, string> { { "Origin", "http://example.com" } });

                // Assert
                Assert.Equal(4, result.Count);
            }
        }

        [Fact]
        public async Task OptionsAsync_WithHeaders_NonSuccess_ThrowsHttpRequestException()
        {
            // Arrange
            var handler = new OptionsHandler
            {
                StatusCode = HttpStatusCode.MethodNotAllowed,
            };
            using (var client = new HttpClient(handler))
            {
                // Act & Assert
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    client.OptionsAsync(
                        "http://test.com",
                        new Dictionary<string, string>()));
            }
        }

        // ============================================================
        // 带 headers 重载 — timeout 边界
        // ============================================================

        [Fact]
        public async Task OptionsAsync_WithHeaders_NegativeTimeout_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            using (var client = new HttpClient(new OptionsHandler()))
            {
                // Act & Assert
                await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                    client.OptionsAsync(
                        "http://test.com",
                        new Dictionary<string, string>(),
                        timeout: -1));
            }
        }

        // ============================================================
        // 带 headers 重载 — 多 header 应用到请求
        // ============================================================

        [Fact]
        public async Task OptionsAsync_WithHeaders_MultipleHeaders_AppliedToRequest()
        {
            // Arrange
            var handler = new OptionsHandler
            {
                AllowedMethods = new[] { "GET" },
            };
            using (var client = new HttpClient(handler))
            {
                var headers = new Dictionary<string, string>
                {
                    { "X-Custom-1", "value1" },
                    { "X-Custom-2", "value2" },
                    { "X-Custom-3", "value3" },
                };

                // Act
                await client.OptionsAsync("http://test.com", headers);

                // Assert
                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Custom-1"));
                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Custom-2"));
                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Custom-3"));
                Assert.Equal("value1", handler.CapturedRequest.Headers.GetValues("X-Custom-1").Single());
                Assert.Equal("value2", handler.CapturedRequest.Headers.GetValues("X-Custom-2").Single());
                Assert.Equal("value3", handler.CapturedRequest.Headers.GetValues("X-Custom-3").Single());
            }
        }

        // ============================================================
        // 辅助类型
        // ============================================================

        /// <summary>
        /// OPTIONS 伪造 Handler：支持在 Content Headers 中写入 Allow 列表。
        /// </summary>
        private sealed class OptionsHandler : HttpMessageHandler
        {
            public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
            public string[] AllowedMethods { get; set; } = Array.Empty<string>();
            public HttpRequestMessage? CapturedRequest { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CapturedRequest = request;
                var response = new HttpResponseMessage(StatusCode)
                {
                    Content = new StringContent(string.Empty),
                };
                foreach (var method in AllowedMethods)
                {
                    response.Content.Headers.Allow.Add(method);
                }

                return Task.FromResult(response);
            }
        }
    }
}
