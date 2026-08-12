using System.Net;
using System.Text;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// ResponseMessageStream（private nested class in internal HttpClientExtensionHelper）的边界测试。
    /// 通过 public GetToStreamAsync 间接验证：
    /// - 同步 Dispose 释放底层 HttpResponseMessage 的 Content
    /// - 异步 DisposeAsync 释放底层 HttpResponseMessage 的 Content
    /// - Dispose 后读取流抛 ObjectDisposedException
    /// - 非成功状态码时 ReadResponseStreamAsync 的 catch 块释放响应
    /// ResponseMessageStream 实际可见性：private sealed class（嵌套于 internal static class），不可直接访问。
    /// </summary>
    public sealed class ResponseMessageStreamBoundaryTests
    {
        [Fact]
        public async Task GetToStreamAsync_DisposeSync_DisposesUnderlyingContent()
        {
            // Arrange
            var handler = new TrackingContentHandler("stream-data");
            using (var client = new HttpClient(handler))
            {
                // Act
                var stream = await client.GetToStreamAsync("http://test.com");

                // Assert — 流可用
                Assert.True(stream.CanRead);
                stream.Dispose();

                // 同步 Dispose 后，底层 HttpResponseMessage 被释放 → Content 被释放
                Assert.True(handler.TrackedContent!.WasDisposed);
            }
        }

        [Fact]
        public async Task GetToStreamAsync_DisposeAsync_DisposesUnderlyingContent()
        {
            // Arrange
            var handler = new TrackingContentHandler("async-data");
            using (var client = new HttpClient(handler))
            {
                // Act
                var stream = await client.GetToStreamAsync("http://test.com");

                // Assert
                Assert.True(stream.CanRead);
                await stream.DisposeAsync();

                // 异步 Dispose 后，底层 HttpResponseMessage 被释放 → Content 被释放
                Assert.True(handler.TrackedContent!.WasDisposed);
            }
        }

        [Fact]
        public async Task GetToStreamAsync_AfterDispose_ReadThrowsObjectDisposedException()
        {
            // Arrange
            var handler = new TrackingContentHandler("dispose-read");
            using (var client = new HttpClient(handler))
            {
                var stream = await client.GetToStreamAsync("http://test.com");

                // Act
                stream.Dispose();
                var buffer = new byte[4];

                // Assert — Dispose 后底层 MemoryStream 已释放，读取抛 ObjectDisposedException
                Assert.Throws<ObjectDisposedException>(() => stream.Read(buffer, 0, buffer.Length));
            }
        }

        [Fact]
        public async Task GetToStreamAsync_AfterDispose_CanReadReturnsFalse()
        {
            // Arrange
            var handler = new TrackingContentHandler("canread");
            using (var client = new HttpClient(handler))
            {
                var stream = await client.GetToStreamAsync("http://test.com");

                // Act
                Assert.True(stream.CanRead); // Dispose 前可读
                stream.Dispose();

                // Assert — Dispose 后底层 MemoryStream 已释放，CanRead 返回 false
                Assert.False(stream.CanRead);
            }
        }

        [Fact]
        public async Task GetToStreamAsync_WithHeaders_DisposeSync_DisposesUnderlyingContent()
        {
            // Arrange — 使用带 headers 重载，owner (CTS) 也传入 ResponseMessageStream
            var handler = new TrackingContentHandler("headers-stream");
            using (var client = new HttpClient(handler))
            {
                var headers = new Dictionary<string, string> { { "X-Test", "value" } };

                // Act
                var stream = await client.GetToStreamAsync("http://test.com", headers);
                stream.Dispose();

                // Assert — 带 headers 重载的 ResponseMessageStream.Dispose 同时释放 response + owner(CTS) + inner stream
                Assert.True(handler.TrackedContent!.WasDisposed);
            }
        }

        [Fact]
        public async Task GetToStreamAsync_NonSuccessResponse_DisposesResponseImmediately()
        {
            // Arrange — 非成功状态码触发 ReadResponseStreamAsync 的 catch 块，catch 中释放 response + owner
            var handler = new TrackingContentHandler("error-body")
            {
                StatusCode = HttpStatusCode.InternalServerError,
            };
            using (var client = new HttpClient(handler))
            {
                // Act & Assert — EnsureSuccessStatusCode 抛 HttpRequestException
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    client.GetToStreamAsync("http://test.com"));

                // catch 块中 response.Dispose() 已执行 → Content 已释放
                Assert.True(handler.TrackedContent!.WasDisposed);
            }
        }

        // ============================================================
        // 辅助类型
        // ============================================================

        /// <summary>
        /// 能跟踪 Content 是否被释放的 Handler，返回带 TrackingStringContent 的响应。
        /// </summary>
        private sealed class TrackingContentHandler : HttpMessageHandler
        {
            private readonly string _content;

            public TrackingContentHandler(string content)
            {
                _content = content;
            }

            public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
            public TrackingStringContent? TrackedContent { get; private set; }
            public HttpRequestMessage? CapturedRequest { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                CapturedRequest = request;
                TrackedContent = new TrackingStringContent(_content);
                var response = new HttpResponseMessage(StatusCode)
                {
                    Content = TrackedContent,
                };
                return Task.FromResult(response);
            }
        }

        /// <summary>
        /// 继承 StringContent，重写 Dispose(bool) 以跟踪释放状态。
        /// HttpContent.Dispose(bool) 是 protected virtual，可被覆盖。
        /// </summary>
        private sealed class TrackingStringContent : StringContent
        {
            public TrackingStringContent(string content)
                : base(content, Encoding.UTF8, "application/json")
            {
            }

            public bool WasDisposed { get; private set; }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    WasDisposed = true;
                }

                base.Dispose(disposing);
            }
        }
    }
}
