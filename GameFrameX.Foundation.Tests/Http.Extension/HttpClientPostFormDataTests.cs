using System.Net;
using System.Text;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// HttpClientPostExtension 中表单(form)和文件(file/multipart)上传方法的单元测试。
    /// 使用本地 HttpMessageHandler 子类 mock，不依赖网络。
    /// </summary>
    public sealed class HttpClientPostFormDataTests
    {
        // ============================================================
        // PostFormToStringAsync
        // ============================================================

        [Fact]
        public async Task PostFormToStringAsync_NullClient_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient)null!).PostFormToStringAsync(
                    "http://test.com",
                    new List<KeyValuePair<string, string>>()));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task PostFormToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        {
            using (var client = new HttpClient(new CapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.PostFormToStringAsync(url, new List<KeyValuePair<string, string>>()));
            }
        }

        [Fact]
        public async Task PostFormToStringAsync_Success_ReturnsResponseBody()
        {
            var handler = new CapturingHandler
            {
                StatusCode = HttpStatusCode.OK,
                ResponseContent = "form-received",
            };
            using (var client = new HttpClient(handler))
            {
                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("name", "value"),
                    new KeyValuePair<string, string>("key", "val2"),
                };

                var result = await client.PostFormToStringAsync("http://test.com", formData);

                Assert.Equal("form-received", result);
            }
        }

        [Fact]
        public async Task PostFormToStringAsync_SendsPostMethod()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            {
                await client.PostFormToStringAsync(
                    "http://test.com",
                    new List<KeyValuePair<string, string>>());

                Assert.Equal(HttpMethod.Post, handler.CapturedRequest!.Method);
            }
        }

        [Fact]
        public async Task PostFormToStringAsync_SetsFormUrlEncodedContentType()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            {
                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("a", "1"),
                };

                await client.PostFormToStringAsync("http://test.com", formData);

                Assert.NotNull(handler.CapturedRequest!.Content);
                Assert.Equal("application/x-www-form-urlencoded",
                    handler.CapturedRequest.Content!.Headers.ContentType!.MediaType);
            }
        }

        [Fact]
        public async Task PostFormToStringAsync_EncodesFormDataInBody()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            {
                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("name", "value"),
                };

                await client.PostFormToStringAsync("http://test.com", formData);

                Assert.NotNull(handler.CapturedBody);
                Assert.Contains("name=value", handler.CapturedBody);
            }
        }

        [Fact]
        public async Task PostFormToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
        {
            var handler = new CapturingHandler { StatusCode = HttpStatusCode.BadRequest };
            using (var client = new HttpClient(handler))
            {
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    client.PostFormToStringAsync(
                        "http://test.com",
                        new List<KeyValuePair<string, string>>()));
            }
        }

        [Fact]
        public async Task PostFormToStringAsync_WithHeaders_SetsRequestHeaders()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            {
                var formData = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("a", "b"),
                };

                await client.PostFormToStringAsync(
                    "http://test.com",
                    formData,
                    new Dictionary<string, string> { { "X-Custom", "hdr" } });

                Assert.True(handler.CapturedRequest!.Headers.Contains("X-Custom"));
            }
        }

        // ============================================================
        // PostFileToStringAsync
        // ============================================================

        [Fact]
        public async Task PostFileToStringAsync_NullClient_ThrowsArgumentNullException()
        {
            using (var tempFile = new TempFile("file-content"))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    ((HttpClient)null!).PostFileToStringAsync("http://test.com", tempFile.Path));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task PostFileToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        {
            using (var tempFile = new TempFile("data"))
            using (var client = new HttpClient(new CapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.PostFileToStringAsync(url, tempFile.Path));
            }
        }

        [Fact]
        public async Task PostFileToStringAsync_BlankFilePath_ThrowsArgumentException()
        {
            using (var client = new HttpClient(new CapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.PostFileToStringAsync("http://test.com", "   "));
            }
        }

        [Fact]
        public async Task PostFileToStringAsync_Success_ReturnsResponseBodyAndSendsFileContent()
        {
            var handler = new CapturingHandler
            {
                StatusCode = HttpStatusCode.OK,
                ResponseContent = "file-ok",
            };
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("file-body-data"))
            {
                var result = await client.PostFileToStringAsync("http://test.com", tempFile.Path);

                Assert.Equal("file-ok", result);
                Assert.Equal("file-body-data", handler.CapturedBody);
            }
        }

        [Fact]
        public async Task PostFileToStringAsync_SetsOctetStreamContentType()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("payload"))
            {
                await client.PostFileToStringAsync("http://test.com", tempFile.Path);

                Assert.NotNull(handler.CapturedRequest!.Content);
                Assert.Equal("application/octet-stream",
                    handler.CapturedRequest.Content!.Headers.ContentType!.MediaType);
            }
        }

        [Fact]
        public async Task PostFileToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
        {
            var handler = new CapturingHandler { StatusCode = HttpStatusCode.InternalServerError };
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("x"))
            {
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    client.PostFileToStringAsync("http://test.com", tempFile.Path));
            }
        }

        [Fact]
        public async Task PostFileToStringAsync_WithHeaders_SetsRequestHeaders()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("data"))
            {
                await client.PostFileToStringAsync(
                    "http://test.com",
                    tempFile.Path,
                    new Dictionary<string, string> { { "X-Trace-Id", "tid" } });

                Assert.Equal("tid", handler.CapturedRequest!.Headers.GetValues("X-Trace-Id").Single());
            }
        }

        // ============================================================
        // PostMultipartFileToStringAsync
        // ============================================================

        [Fact]
        public async Task PostMultipartFileToStringAsync_NullClient_ThrowsArgumentNullException()
        {
            using (var tempFile = new TempFile("c"))
            {
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    ((HttpClient)null!).PostMultipartFileToStringAsync(
                        "http://test.com", "file", tempFile.Path));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task PostMultipartFileToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        {
            using (var tempFile = new TempFile("c"))
            using (var client = new HttpClient(new CapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.PostMultipartFileToStringAsync(url, "file", tempFile.Path));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task PostMultipartFileToStringAsync_BlankFieldName_ThrowsArgumentException(string field)
        {
            using (var tempFile = new TempFile("c"))
            using (var client = new HttpClient(new CapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.PostMultipartFileToStringAsync("http://test.com", field, tempFile.Path));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task PostMultipartFileToStringAsync_BlankFilePath_ThrowsArgumentException(string path)
        {
            using (var client = new HttpClient(new CapturingHandler()))
            {
                await Assert.ThrowsAsync<ArgumentException>(() =>
                    client.PostMultipartFileToStringAsync("http://test.com", "file", path));
            }
        }

        [Fact]
        public async Task PostMultipartFileToStringAsync_Success_ReturnsResponseBody()
        {
            var handler = new CapturingHandler
            {
                StatusCode = HttpStatusCode.OK,
                ResponseContent = "uploaded",
            };
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("multipart-body"))
            {
                var result = await client.PostMultipartFileToStringAsync(
                    "http://test.com", "uploadFile", tempFile.Path);

                Assert.Equal("uploaded", result);
            }
        }

        [Fact]
        public async Task PostMultipartFileToStringAsync_SendsMultipartContent()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("data"))
            {
                await client.PostMultipartFileToStringAsync(
                    "http://test.com", "fileField", tempFile.Path);

                Assert.NotNull(handler.CapturedRequest!.Content);
                Assert.IsType<MultipartFormDataContent>(handler.CapturedRequest.Content);
            }
        }

        [Fact]
        public async Task PostMultipartFileToStringAsync_WithFormData_IncludesExtraFields()
        {
            var handler = new CapturingHandler();
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("file-data"))
            {
                var formData = new Dictionary<string, string>
                {
                    { "description", "my-file" },
                    { "category", "test" },
                };

                await client.PostMultipartFileToStringAsync(
                    "http://test.com", "upload", tempFile.Path, formData);

                Assert.NotNull(handler.CapturedBody);
                Assert.Contains("my-file", handler.CapturedBody);
                Assert.Contains("test", handler.CapturedBody);
            }
        }

        [Fact]
        public async Task PostMultipartFileToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
        {
            var handler = new CapturingHandler { StatusCode = HttpStatusCode.Forbidden };
            using (var client = new HttpClient(handler))
            using (var tempFile = new TempFile("x"))
            {
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    client.PostMultipartFileToStringAsync("http://test.com", "file", tempFile.Path));
            }
        }

        // ============================================================
        // 辅助：能读取请求体的 Mock Handler
        // ============================================================

        /// <summary>
        /// 捕获请求的 method/body/headers 并返回预配置响应的 HttpMessageHandler。
        /// </summary>
        private sealed class CapturingHandler : HttpMessageHandler
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

        /// <summary>
        /// 临时文件，构造时创建、Dispose 时删除。
        /// </summary>
        private sealed class TempFile : IDisposable
        {
            public string Path { get; }

            public TempFile(string content)
            {
                Path = System.IO.Path.GetTempFileName();
                File.WriteAllText(Path, content);
            }

            public void Dispose()
            {
                try
                {
                    if (File.Exists(Path))
                    {
                        File.Delete(Path);
                    }
                }
                catch
                {
                    // 忽略清理失败
                }
            }
        }
    }
}
