using System.Net;
using System.Text;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension;

// ── 共享伪造 Handler ──────────────────────────────────────────────────────────

/// <summary>
/// 通用伪造 HttpMessageHandler：拦截所有 SendAsync 调用，记录请求，返回预配置响应。
/// </summary>
internal sealed class FakeHttpMessageHandler : HttpMessageHandler
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string ResponseContent { get; set; } = string.Empty;
    public HttpRequestMessage? CapturedRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CapturedRequest = request;
        var response = new HttpResponseMessage(StatusCode)
        {
            Content = new StringContent(ResponseContent, Encoding.UTF8, "application/json")
        };
        return Task.FromResult(response);
    }
}

/// <summary>
/// 专用于 OPTIONS 的伪造 Handler：支持在 Content Headers 中写入 Allow 列表。
/// </summary>
internal sealed class FakeOptionsHttpMessageHandler : HttpMessageHandler
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public string[] AllowedMethods { get; set; } = [];
    public HttpRequestMessage? CapturedRequest { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CapturedRequest = request;
        var response = new HttpResponseMessage(StatusCode)
        {
            Content = new StringContent(string.Empty)
        };
        foreach (var method in AllowedMethods)
        {
            response.Content.Headers.Allow.Add(method);
        }
        return Task.FromResult(response);
    }
}

// ── GET ──────────────────────────────────────────────────────────────────────

public sealed class HttpClientGetExtensionTests : IDisposable
{
    private readonly FakeHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientGetExtensionTests() => _client = new HttpClient(_handler);

    // --- GetToStringAsync ---

    [Fact]
    public async Task GetToStringAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).GetToStringAsync("http://test.com"));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.GetToStringAsync(url));

    [Fact]
    public async Task GetToStringAsync_Success_SendsGetMethod()
    {
        await _client.GetToStringAsync("http://test.com");

        Assert.Equal(HttpMethod.Get, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task GetToStringAsync_Success_ReturnsResponseBody()
    {
        _handler.ResponseContent = "hello world";

        var result = await _client.GetToStringAsync("http://test.com");

        Assert.Equal("hello world", result);
    }

    [Fact]
    public async Task GetToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.NotFound;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.GetToStringAsync("http://test.com"));
    }

    [Fact]
    public async Task GetToStringAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.GetToStringAsync("http://test.com",
            new Dictionary<string, string> { { "X-Custom", "abc" } });

        Assert.Equal("abc",
            _handler.CapturedRequest!.Headers.GetValues("X-Custom").Single());
    }

    // --- GetToByteArrayAsync ---

    [Fact]
    public async Task GetToByteArrayAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).GetToByteArrayAsync("http://test.com"));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetToByteArrayAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.GetToByteArrayAsync(url));

    [Fact]
    public async Task GetToByteArrayAsync_Success_ReturnsResponseBytes()
    {
        _handler.ResponseContent = "binary";

        var result = await _client.GetToByteArrayAsync("http://test.com");

        Assert.Equal("binary", Encoding.UTF8.GetString(result));
    }

    [Fact]
    public async Task GetToByteArrayAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.InternalServerError;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.GetToByteArrayAsync("http://test.com"));
    }

    [Fact]
    public async Task GetToByteArrayAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.GetToByteArrayAsync("http://test.com",
            new Dictionary<string, string> { { "Accept-Language", "zh-CN" } });

        Assert.True(_handler.CapturedRequest!.Headers.Contains("Accept-Language"));
    }

    // --- GetToStreamAsync ---

    [Fact]
    public async Task GetToStreamAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).GetToStreamAsync("http://test.com"));

    [Fact]
    public async Task GetToStreamAsync_Success_ReturnsReadableStream()
    {
        _handler.ResponseContent = "stream-data";

        await using var stream = await _client.GetToStreamAsync("http://test.com");
        using var reader = new StreamReader(stream);

        Assert.Equal("stream-data", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task GetToStreamAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.Forbidden;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.GetToStreamAsync("http://test.com"));
    }

    public void Dispose() => _client.Dispose();
}

// ── POST ─────────────────────────────────────────────────────────────────────

public sealed class HttpClientPostExtensionTests : IDisposable
{
    private record Payload(string Name);

    private readonly FakeHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientPostExtensionTests() => _client = new HttpClient(_handler);

    // --- PostJsonToStringAsync ---

    [Fact]
    public async Task PostJsonToStringAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).PostJsonToStringAsync("http://test.com", new Payload("x")));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PostJsonToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.PostJsonToStringAsync(url, new Payload("x")));

    [Fact]
    public async Task PostJsonToStringAsync_Success_SendsPostMethod()
    {
        await _client.PostJsonToStringAsync("http://test.com", new Payload("x"));

        Assert.Equal(HttpMethod.Post, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PostJsonToStringAsync_Success_ReturnsResponseBody()
    {
        _handler.ResponseContent = "created";

        var result = await _client.PostJsonToStringAsync("http://test.com", new Payload("x"));

        Assert.Equal("created", result);
    }

    [Fact]
    public async Task PostJsonToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.BadRequest;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PostJsonToStringAsync("http://test.com", new Payload("x")));
    }

    [Fact]
    public async Task PostJsonToStringAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.PostJsonToStringAsync("http://test.com", new Payload("x"),
            new Dictionary<string, string> { { "Authorization", "Bearer token" } });

        Assert.True(_handler.CapturedRequest!.Headers.Contains("Authorization"));
    }

    // --- PostJsonToByteArrayAsync ---

    [Fact]
    public async Task PostJsonToByteArrayAsync_Success_SendsPostMethod()
    {
        await _client.PostJsonToByteArrayAsync("http://test.com", new Payload("x"));

        Assert.Equal(HttpMethod.Post, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PostJsonToByteArrayAsync_Success_ReturnsResponseBytes()
    {
        _handler.ResponseContent = "ok";

        var result = await _client.PostJsonToByteArrayAsync("http://test.com", new Payload("x"));

        Assert.Equal("ok", Encoding.UTF8.GetString(result));
    }

    [Fact]
    public async Task PostJsonToByteArrayAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.UnprocessableEntity;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PostJsonToByteArrayAsync("http://test.com", new Payload("x")));
    }

    // --- PostJsonToStreamAsync ---

    [Fact]
    public async Task PostJsonToStreamAsync_Success_ReturnsReadableStream()
    {
        _handler.ResponseContent = "stream";

        await using var stream = await _client.PostJsonToStreamAsync("http://test.com", new Payload("x"));
        using var reader = new StreamReader(stream);

        Assert.Equal("stream", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task PostJsonToStreamAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.ServiceUnavailable;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PostJsonToStreamAsync("http://test.com", new Payload("x")));
    }

    public void Dispose() => _client.Dispose();
}

// ── PUT ──────────────────────────────────────────────────────────────────────

public sealed class HttpClientPutExtensionTests : IDisposable
{
    private record Payload(string Value);

    private readonly FakeHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientPutExtensionTests() => _client = new HttpClient(_handler);

    // --- PutJsonToStringAsync ---

    [Fact]
    public async Task PutJsonToStringAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).PutJsonToStringAsync("http://test.com", new Payload("v")));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PutJsonToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.PutJsonToStringAsync(url, new Payload("v")));

    [Fact]
    public async Task PutJsonToStringAsync_Success_SendsPutMethod()
    {
        await _client.PutJsonToStringAsync("http://test.com", new Payload("v"));

        Assert.Equal(HttpMethod.Put, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PutJsonToStringAsync_Success_ReturnsResponseBody()
    {
        _handler.ResponseContent = "updated";

        var result = await _client.PutJsonToStringAsync("http://test.com", new Payload("v"));

        Assert.Equal("updated", result);
    }

    [Fact]
    public async Task PutJsonToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.NotFound;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PutJsonToStringAsync("http://test.com", new Payload("v")));
    }

    [Fact]
    public async Task PutJsonToStringAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.PutJsonToStringAsync("http://test.com", new Payload("v"),
            new Dictionary<string, string> { { "X-Trace-Id", "abc123" } });

        Assert.Equal("abc123",
            _handler.CapturedRequest!.Headers.GetValues("X-Trace-Id").Single());
    }

    // --- PutJsonToByteArrayAsync ---

    [Fact]
    public async Task PutJsonToByteArrayAsync_Success_SendsPutMethod()
    {
        await _client.PutJsonToByteArrayAsync("http://test.com", new Payload("v"));

        Assert.Equal(HttpMethod.Put, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PutJsonToByteArrayAsync_Success_ReturnsResponseBytes()
    {
        _handler.ResponseContent = "put-ok";

        var result = await _client.PutJsonToByteArrayAsync("http://test.com", new Payload("v"));

        Assert.Equal("put-ok", Encoding.UTF8.GetString(result));
    }

    [Fact]
    public async Task PutJsonToByteArrayAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.Conflict;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PutJsonToByteArrayAsync("http://test.com", new Payload("v")));
    }

    // --- PutJsonToStreamAsync ---

    [Fact]
    public async Task PutJsonToStreamAsync_Success_SendsPutMethod()
    {
        await using var stream = await _client.PutJsonToStreamAsync("http://test.com", new Payload("v"));

        Assert.Equal(HttpMethod.Put, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PutJsonToStreamAsync_Success_ReturnsReadableStream()
    {
        _handler.ResponseContent = "put-stream";

        await using var stream = await _client.PutJsonToStreamAsync("http://test.com", new Payload("v"));
        using var reader = new StreamReader(stream);

        Assert.Equal("put-stream", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task PutJsonToStreamAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.BadRequest;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PutJsonToStreamAsync("http://test.com", new Payload("v")));
    }

    public void Dispose() => _client.Dispose();
}

// ── PATCH ─────────────────────────────────────────────────────────────────────

public sealed class HttpClientPatchExtensionTests : IDisposable
{
    private record Payload(string Field);

    private readonly FakeHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientPatchExtensionTests() => _client = new HttpClient(_handler);

    // --- PatchJsonToStringAsync ---

    [Fact]
    public async Task PatchJsonToStringAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).PatchJsonToStringAsync("http://test.com", new Payload("f")));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PatchJsonToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.PatchJsonToStringAsync(url, new Payload("f")));

    [Fact]
    public async Task PatchJsonToStringAsync_Success_SendsPatchMethod()
    {
        await _client.PatchJsonToStringAsync("http://test.com", new Payload("f"));

        Assert.Equal(HttpMethod.Patch, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PatchJsonToStringAsync_Success_ReturnsResponseBody()
    {
        _handler.ResponseContent = "patched";

        var result = await _client.PatchJsonToStringAsync("http://test.com", new Payload("f"));

        Assert.Equal("patched", result);
    }

    [Fact]
    public async Task PatchJsonToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.UnprocessableEntity;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PatchJsonToStringAsync("http://test.com", new Payload("f")));
    }

    [Fact]
    public async Task PatchJsonToStringAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.PatchJsonToStringAsync("http://test.com", new Payload("f"),
            new Dictionary<string, string> { { "If-Match", "\"etag-abc\"" } });

        Assert.True(_handler.CapturedRequest!.Headers.Contains("If-Match"));
    }

    // --- PatchJsonToByteArrayAsync ---

    [Fact]
    public async Task PatchJsonToByteArrayAsync_Success_SendsPatchMethod()
    {
        await _client.PatchJsonToByteArrayAsync("http://test.com", new Payload("f"));

        Assert.Equal(HttpMethod.Patch, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PatchJsonToByteArrayAsync_Success_ReturnsResponseBytes()
    {
        _handler.ResponseContent = "patch-bytes";

        var result = await _client.PatchJsonToByteArrayAsync("http://test.com", new Payload("f"));

        Assert.Equal("patch-bytes", Encoding.UTF8.GetString(result));
    }

    [Fact]
    public async Task PatchJsonToByteArrayAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.NotFound;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PatchJsonToByteArrayAsync("http://test.com", new Payload("f")));
    }

    // --- PatchJsonToStreamAsync ---

    [Fact]
    public async Task PatchJsonToStreamAsync_Success_SendsPatchMethod()
    {
        await using var stream = await _client.PatchJsonToStreamAsync("http://test.com", new Payload("f"));

        Assert.Equal(HttpMethod.Patch, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task PatchJsonToStreamAsync_Success_ReturnsReadableStream()
    {
        _handler.ResponseContent = "patch-stream";

        await using var stream = await _client.PatchJsonToStreamAsync("http://test.com", new Payload("f"));
        using var reader = new StreamReader(stream);

        Assert.Equal("patch-stream", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task PatchJsonToStreamAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.Conflict;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.PatchJsonToStreamAsync("http://test.com", new Payload("f")));
    }

    public void Dispose() => _client.Dispose();
}

// ── DELETE ───────────────────────────────────────────────────────────────────

public sealed class HttpClientDeleteExtensionTests : IDisposable
{
    private readonly FakeHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientDeleteExtensionTests() => _client = new HttpClient(_handler);

    // --- DeleteToStringAsync ---

    [Fact]
    public async Task DeleteToStringAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).DeleteToStringAsync("http://test.com"));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task DeleteToStringAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.DeleteToStringAsync(url));

    [Fact]
    public async Task DeleteToStringAsync_Success_SendsDeleteMethod()
    {
        await _client.DeleteToStringAsync("http://test.com");

        Assert.Equal(HttpMethod.Delete, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task DeleteToStringAsync_Success_ReturnsResponseBody()
    {
        _handler.ResponseContent = "deleted";

        var result = await _client.DeleteToStringAsync("http://test.com");

        Assert.Equal("deleted", result);
    }

    [Fact]
    public async Task DeleteToStringAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.NotFound;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.DeleteToStringAsync("http://test.com"));
    }

    [Fact]
    public async Task DeleteToStringAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.DeleteToStringAsync("http://test.com",
            new Dictionary<string, string> { { "X-Reason", "cleanup" } });

        Assert.Equal("cleanup",
            _handler.CapturedRequest!.Headers.GetValues("X-Reason").Single());
    }

    // --- DeleteToByteArrayAsync ---

    [Fact]
    public async Task DeleteToByteArrayAsync_Success_SendsDeleteMethod()
    {
        await _client.DeleteToByteArrayAsync("http://test.com");

        Assert.Equal(HttpMethod.Delete, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task DeleteToByteArrayAsync_Success_ReturnsResponseBytes()
    {
        _handler.ResponseContent = "del-ok";

        var result = await _client.DeleteToByteArrayAsync("http://test.com");

        Assert.Equal("del-ok", Encoding.UTF8.GetString(result));
    }

    [Fact]
    public async Task DeleteToByteArrayAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.Forbidden;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.DeleteToByteArrayAsync("http://test.com"));
    }

    [Fact]
    public async Task DeleteToByteArrayAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.DeleteToByteArrayAsync("http://test.com",
            new Dictionary<string, string> { { "X-Cascade", "true" } });

        Assert.True(_handler.CapturedRequest!.Headers.Contains("X-Cascade"));
    }

    public void Dispose() => _client.Dispose();
}

// ── HEAD ─────────────────────────────────────────────────────────────────────

public sealed class HttpClientHeadExtensionTests : IDisposable
{
    private readonly FakeHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientHeadExtensionTests() => _client = new HttpClient(_handler);

    [Fact]
    public async Task HeadAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).HeadAsync("http://test.com"));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task HeadAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.HeadAsync(url));

    [Fact]
    public async Task HeadAsync_Success_SendsHeadMethod()
    {
        await _client.HeadAsync("http://test.com");

        Assert.Equal(HttpMethod.Head, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task HeadAsync_Success_ReturnsNonNullHeaders()
    {
        var headers = await _client.HeadAsync("http://test.com");

        Assert.NotNull(headers);
    }

    [Fact]
    public async Task HeadAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.NotFound;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.HeadAsync("http://test.com"));
    }

    [Fact]
    public async Task HeadAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.HeadAsync("http://test.com",
            new Dictionary<string, string> { { "If-None-Match", "\"v1\"" } });

        Assert.True(_handler.CapturedRequest!.Headers.Contains("If-None-Match"));
    }

    public void Dispose() => _client.Dispose();
}

// ── OPTIONS ──────────────────────────────────────────────────────────────────

public sealed class HttpClientOptionsExtensionTests : IDisposable
{
    private readonly FakeOptionsHttpMessageHandler _handler = new();
    private readonly HttpClient _client;

    public HttpClientOptionsExtensionTests() => _client = new HttpClient(_handler);

    [Fact]
    public async Task OptionsAsync_NullClient_ThrowsArgumentNullException()
        => await Assert.ThrowsAsync<ArgumentNullException>(() =>
            ((HttpClient)null!).OptionsAsync("http://test.com"));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task OptionsAsync_BlankUrl_ThrowsArgumentException(string url)
        => await Assert.ThrowsAsync<ArgumentException>(() =>
            _client.OptionsAsync(url));

    [Fact]
    public async Task OptionsAsync_Success_SendsOptionsMethod()
    {
        await _client.OptionsAsync("http://test.com");

        Assert.Equal(HttpMethod.Options, _handler.CapturedRequest!.Method);
    }

    [Fact]
    public async Task OptionsAsync_Success_ReturnsAllowedMethodsFromHeader()
    {
        _handler.AllowedMethods = ["GET", "POST", "PUT"];

        var result = await _client.OptionsAsync("http://test.com");

        Assert.Equal(3, result.Count);
        Assert.Contains("GET", result);
        Assert.Contains("POST", result);
        Assert.Contains("PUT", result);
    }

    [Fact]
    public async Task OptionsAsync_EmptyAllowHeader_ReturnsEmptyCollection()
    {
        _handler.AllowedMethods = [];

        var result = await _client.OptionsAsync("http://test.com");

        Assert.Empty(result);
    }

    [Fact]
    public async Task OptionsAsync_NonSuccessStatus_ThrowsHttpRequestException()
    {
        _handler.StatusCode = HttpStatusCode.MethodNotAllowed;

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.OptionsAsync("http://test.com"));
    }

    [Fact]
    public async Task OptionsAsync_WithHeaders_SetsRequestHeaders()
    {
        await _client.OptionsAsync("http://test.com",
            new Dictionary<string, string> { { "Origin", "http://example.com" } });

        Assert.Equal("http://example.com",
            _handler.CapturedRequest!.Headers.GetValues("Origin").Single());
    }

    public void Dispose() => _client.Dispose();
}
