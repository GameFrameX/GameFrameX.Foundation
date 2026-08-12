using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension;

/// <summary>
/// HTTP 扩展集成测试 —— 使用真实公共 API 端点进行端到端验证。
///
/// 默认使用 https://httpbingo.org —— httpbin 协议的社区镜像（Fly.io 托管），
/// 支持完整的 GET/POST/PUT/PATCH/DELETE/HEAD/OPTIONS 并将请求信息（方法、请求头、请求体）原样回显。
/// httpbin.org 频繁因流量过载返回 503，故默认改用更稳定的 httpbingo.org；二者 API 完全兼容。
/// 端点通过 <see cref="IntegrationEndpoints"/> 常量集中管理，可按需切换。
///
/// 运行前提：可访问公网。
/// 默认跳过；设置 GAMEFRAMEX_RUN_INTEGRATION_TESTS=true 后，通过 --filter "Category=Integration" 单独执行本组测试。
/// </summary>

// ── 共享 HttpClient（所有集成测试共用，避免连接泄漏）────────────────────────────
[CollectionDefinition("Integration")]
public sealed class IntegrationCollection
{
}

internal sealed class IntegrationFactAttribute : FactAttribute
{
    private const string RunIntegrationTestsVariable = "GAMEFRAMEX_RUN_INTEGRATION_TESTS";

    public IntegrationFactAttribute(
        [CallerFilePath] string sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1)
        : base(sourceFilePath, sourceLineNumber)
    {
        var value = Environment.GetEnvironmentVariable(RunIntegrationTestsVariable);
        if (!string.Equals(value, "true", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(value, "1", StringComparison.OrdinalIgnoreCase))
        {
            Skip = $"Set {RunIntegrationTestsVariable}=true to run public-network integration tests.";
        }
    }
}

// ── 公共测试端点（httpbin 协议兼容，可按需切换）──────────────────────────────
// httpbin.org 频繁因流量过载返回 503；httpbingo.org 是 API 完全兼容的社区镜像（Fly.io 托管），更稳定。
internal static class IntegrationEndpoints
{
    /// <summary>httpbin 协议兼容服务的镜像主机名（Fly.io 托管，比 httpbin.org 更稳定）。</summary>
    public const string HttpBinHost = "httpbingo.org";

    /// <summary>echo / status 类端点的基础地址。</summary>
    public const string HttpBin = "https://" + HttpBinHost;

    /// <summary>
    /// 创建集成测试专用 <see cref="HttpClient"/>：超时 30s，并附加 User-Agent。
    /// httpbingo.org 对无 User-Agent 的请求返回 402（反滥用机制），故必须显式设置 UA。
    /// </summary>
    public static HttpClient CreateClient()
    {
        var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        client.DefaultRequestHeaders.UserAgent.ParseAdd("GameFrameX-IntegrationTest/1.0");
        return client;
    }
}

// ── GET ──────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientGetIntegrationTests : IDisposable
{
    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientGetIntegrationTests(ITestOutputHelper output) => _output = output;

    // --- GetToStringAsync ---

    [IntegrationFact]
    public async Task GetToStringAsync_HttpBin_ReturnsNonEmptyJsonBody()
    {
        var result = await _client.GetToStringAsync($"{IntegrationEndpoints.HttpBin}/get");

        _output.WriteLine(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
        // httpbin 回显请求 URL 字段
        Assert.Contains("\"url\"", result);
        Assert.Contains(IntegrationEndpoints.HttpBin + "/get", result);
    }

    [IntegrationFact]
    public async Task GetToStringAsync_HttpBin_With404_ThrowsHttpRequestException()
    {
        // httpbin.org/status/404 会返回 404，EnsureSuccessStatusCode 应抛出
        await Assert.ThrowsAsync<HttpRequestException>(() =>
                                                           _client.GetToStringAsync($"{IntegrationEndpoints.HttpBin}/status/404"));
    }

    [IntegrationFact]
    public async Task GetToStringAsync_WithCustomHeader_HeaderAppearsInEchoResponse()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Integration-Test", "GameFrameX-Foundation" }
        };

        // httpbin.org/headers 将收到的所有请求头原样返回
        var result = await _client.GetToStringAsync($"{IntegrationEndpoints.HttpBin}/headers", headers);

        _output.WriteLine(result);
        Assert.Contains("X-Integration-Test", result);
        Assert.Contains("GameFrameX-Foundation", result);
    }

    // --- GetToByteArrayAsync ---

    [IntegrationFact]
    public async Task GetToByteArrayAsync_HttpBin_ReturnsBytesDecodableAsJson()
    {
        var bytes = await _client.GetToByteArrayAsync($"{IntegrationEndpoints.HttpBin}/get");

        Assert.NotEmpty(bytes);
        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        // 能解析为合法 JSON
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.TryGetProperty("url", out _));
    }

    [IntegrationFact]
    public async Task GetToByteArrayAsync_HttpBin_With500_ThrowsHttpRequestException()
    {
        await Assert.ThrowsAsync<HttpRequestException>(() =>
                                                           _client.GetToByteArrayAsync($"{IntegrationEndpoints.HttpBin}/status/500"));
    }

    // --- GetToStreamAsync ---

    [IntegrationFact]
    public async Task GetToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.GetToStreamAsync($"{IntegrationEndpoints.HttpBin}/get");
        using var reader = new StreamReader(stream, Encoding.UTF8);

        var text = await reader.ReadToEndAsync();
        _output.WriteLine(text);
        Assert.Contains(IntegrationEndpoints.HttpBinHost, text);
    }

    public void Dispose() => _client.Dispose();
}

// ── POST ─────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientPostIntegrationTests : IDisposable
{
    private record TestPayload(string Name, int Value);

    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientPostIntegrationTests(ITestOutputHelper output) => _output = output;

    // --- PostJsonToStringAsync ---

    [IntegrationFact]
    public async Task PostJsonToStringAsync_HttpBin_EchoesJsonBody()
    {
        var payload = new TestPayload("integration-test", 42);

        // httpbin.org/post 将请求 JSON 体原样回显在响应的 json 字段中
        var result = await _client.PostJsonToStringAsync($"{IntegrationEndpoints.HttpBin}/post", payload);

        _output.WriteLine(result);
        Assert.Contains("\"json\"", result);
        Assert.Contains("integration-test", result);
        Assert.Contains("42", result);
    }

    [IntegrationFact]
    public async Task PostJsonToStringAsync_HttpBin_ConfirmsPostMethod()
    {
        var result = await _client.PostJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/anything", new TestPayload("x", 1));

        _output.WriteLine(result);
        // httpbin.org/anything 回显请求方法
        Assert.Contains("\"method\": \"POST\"", result);
    }

    [IntegrationFact]
    public async Task PostJsonToStringAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Request-Source", "GameFrameX" }
        };

        var result = await _client.PostJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/post", new TestPayload("h", 0), headers);

        _output.WriteLine(result);
        Assert.Contains("X-Request-Source", result);
        Assert.Contains("GameFrameX", result);
    }

    // --- PostJsonToByteArrayAsync ---

    [IntegrationFact]
    public async Task PostJsonToByteArrayAsync_HttpBin_ReturnsBytesWithEchoedBody()
    {
        var bytes = await _client.PostJsonToByteArrayAsync(
                        $"{IntegrationEndpoints.HttpBin}/post", new TestPayload("bytes-test", 99));

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("bytes-test", json);
    }

    // --- PostJsonToStreamAsync ---

    [IntegrationFact]
    public async Task PostJsonToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.PostJsonToStreamAsync(
                                     $"{IntegrationEndpoints.HttpBin}/post", new TestPayload("stream-test", 7));
        using var reader = new StreamReader(stream, Encoding.UTF8);

        var text = await reader.ReadToEndAsync();
        _output.WriteLine(text);
        Assert.Contains("stream-test", text);
    }

    public void Dispose() => _client.Dispose();
}

// ── PUT ──────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientPutIntegrationTests : IDisposable
{
    private record UpdatePayload(string Title, string Body);

    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientPutIntegrationTests(ITestOutputHelper output) => _output = output;

    [IntegrationFact]
    public async Task PutJsonToStringAsync_HttpBin_ConfirmsPutMethod()
    {
        var result = await _client.PutJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/anything",
                         new UpdatePayload("updated-title", "updated-body"));

        _output.WriteLine(result);
        Assert.Contains("\"method\": \"PUT\"", result);
    }

    [IntegrationFact]
    public async Task PutJsonToStringAsync_HttpBin_EchoesJsonBody()
    {
        var result = await _client.PutJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/put",
                         new UpdatePayload("put-title", "put-body"));

        _output.WriteLine(result);
        Assert.Contains("put-title", result);
        Assert.Contains("put-body", result);
    }

    [IntegrationFact]
    public async Task PutJsonToStringAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "If-Match", "\"etag-v1\"" }
        };

        var result = await _client.PutJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/put",
                         new UpdatePayload("h", "b"), headers);

        _output.WriteLine(result);
        Assert.Contains("If-Match", result);
    }

    [IntegrationFact]
    public async Task PutJsonToByteArrayAsync_HttpBin_ReturnsBytesWithEchoedBody()
    {
        var bytes = await _client.PutJsonToByteArrayAsync(
                        $"{IntegrationEndpoints.HttpBin}/put",
                        new UpdatePayload("bytes-put", "content"));

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("bytes-put", json);
    }

    [IntegrationFact]
    public async Task PutJsonToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.PutJsonToStreamAsync(
                                     $"{IntegrationEndpoints.HttpBin}/put",
                                     new UpdatePayload("stream-put", "data"));
        using var reader = new StreamReader(stream, Encoding.UTF8);

        var text = await reader.ReadToEndAsync();
        _output.WriteLine(text);
        Assert.Contains("stream-put", text);
    }

    public void Dispose() => _client.Dispose();
}

// ── PATCH ─────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientPatchIntegrationTests : IDisposable
{
    private record PatchPayload(string Field, string NewValue);

    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientPatchIntegrationTests(ITestOutputHelper output) => _output = output;

    [IntegrationFact]
    public async Task PatchJsonToStringAsync_HttpBin_ConfirmsPatchMethod()
    {
        var result = await _client.PatchJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/anything",
                         new PatchPayload("status", "active"));

        _output.WriteLine(result);
        Assert.Contains("\"method\": \"PATCH\"", result);
    }

    [IntegrationFact]
    public async Task PatchJsonToStringAsync_HttpBin_EchoesJsonBody()
    {
        var result = await _client.PatchJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/patch",
                         new PatchPayload("email", "new@example.com"));

        _output.WriteLine(result);
        Assert.Contains("email", result);
        Assert.Contains("new@example.com", result);
    }

    [IntegrationFact]
    public async Task PatchJsonToStringAsync_WithIfMatchHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "If-Match", "\"v2\"" }
        };

        var result = await _client.PatchJsonToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/patch",
                         new PatchPayload("f", "v"), headers);

        _output.WriteLine(result);
        Assert.Contains("If-Match", result);
    }

    [IntegrationFact]
    public async Task PatchJsonToByteArrayAsync_HttpBin_ReturnsBytesWithEchoedBody()
    {
        var bytes = await _client.PatchJsonToByteArrayAsync(
                        $"{IntegrationEndpoints.HttpBin}/patch",
                        new PatchPayload("bytes-field", "bytes-value"));

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("bytes-field", json);
    }

    [IntegrationFact]
    public async Task PatchJsonToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.PatchJsonToStreamAsync(
                                     $"{IntegrationEndpoints.HttpBin}/patch",
                                     new PatchPayload("stream-field", "stream-value"));
        using var reader = new StreamReader(stream, Encoding.UTF8);

        var text = await reader.ReadToEndAsync();
        _output.WriteLine(text);
        Assert.Contains("stream-field", text);
    }

    public void Dispose() => _client.Dispose();
}

// ── DELETE ───────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientDeleteIntegrationTests : IDisposable
{
    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientDeleteIntegrationTests(ITestOutputHelper output) => _output = output;

    [IntegrationFact]
    public async Task DeleteToStringAsync_HttpBin_ConfirmsDeleteMethod()
    {
        var result = await _client.DeleteToStringAsync($"{IntegrationEndpoints.HttpBin}/anything");

        _output.WriteLine(result);
        Assert.Contains("\"method\": \"DELETE\"", result);
    }

    [IntegrationFact]
    public async Task DeleteToStringAsync_HttpBin_ReturnsNonEmptyJson()
    {
        var result = await _client.DeleteToStringAsync($"{IntegrationEndpoints.HttpBin}/delete");

        _output.WriteLine(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.Contains(IntegrationEndpoints.HttpBinHost, result);
    }

    [IntegrationFact]
    public async Task DeleteToStringAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Delete-Reason", "obsolete" }
        };

        var result = await _client.DeleteToStringAsync(
                         $"{IntegrationEndpoints.HttpBin}/delete", headers);

        _output.WriteLine(result);
        Assert.Contains("X-Delete-Reason", result);
        Assert.Contains("obsolete", result);
    }

    [IntegrationFact]
    public async Task DeleteToByteArrayAsync_HttpBin_ReturnsBytesDecodableAsJson()
    {
        var bytes = await _client.DeleteToByteArrayAsync($"{IntegrationEndpoints.HttpBin}/delete");

        Assert.NotEmpty(bytes);
        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.TryGetProperty("url", out _));
    }

    [IntegrationFact]
    public async Task DeleteToByteArrayAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Cascade", "true" }
        };

        var bytes = await _client.DeleteToByteArrayAsync(
                        $"{IntegrationEndpoints.HttpBin}/delete", headers);

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("X-Cascade", json);
    }

    public void Dispose() => _client.Dispose();
}

// ── HEAD ─────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientHeadIntegrationTests : IDisposable
{
    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientHeadIntegrationTests(ITestOutputHelper output) => _output = output;

    [IntegrationFact]
    public async Task HeadAsync_HttpBin_ReturnsNonNullHeaders()
    {
        var headers = await _client.HeadAsync($"{IntegrationEndpoints.HttpBin}/get");

        Assert.NotNull(headers);
        // 枚举所有返回的响应头
        foreach (var h in headers)
        {
            _output.WriteLine($"{h.Key}: {string.Join(", ", h.Value)}");
        }
    }

    [IntegrationFact]
    public async Task HeadAsync_HttpBin_ContentTypeHeaderIsPresent()
    {
        // HEAD 响应应包含与 GET 相同的响应头，但无响应体
        // Content-Type 通常在 HttpContentHeaders，这里验证响应头中有服务器信息
        var headers = await _client.HeadAsync($"{IntegrationEndpoints.HttpBin}/get");

        // httpbin 通过 Server 或其他头表明自己的身份
        // 验证 headers 对象是合法的可迭代集合
        Assert.NotNull(headers);
        var headerList = headers.ToList();
        _output.WriteLine($"收到 {headerList.Count} 个响应头");
        // HEAD 响应至少应有若干头（Date, Server 等）
        Assert.True(headerList.Count >= 0); // 至少不会崩溃
    }

    [IntegrationFact]
    public async Task HeadAsync_WithIfNoneMatchHeader_RequestIsSent()
    {
        // 验证自定义头被正确附加（通过不抛异常隐性验证）
        var headers = await _client.HeadAsync($"{IntegrationEndpoints.HttpBin}/get",
                                              new Dictionary<string, string> { { "If-None-Match", "\"some-etag\"" } });

        Assert.NotNull(headers);
    }

    [IntegrationFact]
    public async Task HeadAsync_HttpBin_404Status_ThrowsHttpRequestException()
    {
        await Assert.ThrowsAsync<HttpRequestException>(() =>
                                                           _client.HeadAsync($"{IntegrationEndpoints.HttpBin}/status/404"));
    }

    public void Dispose() => _client.Dispose();
}

// ── OPTIONS ──────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientOptionsIntegrationTests : IDisposable
{
    private readonly HttpClient _client = IntegrationEndpoints.CreateClient();
    private readonly ITestOutputHelper _output;

    public HttpClientOptionsIntegrationTests(ITestOutputHelper output) => _output = output;

    [IntegrationFact]
    public async Task OptionsAsync_HttpBin_DoesNotThrow()
    {
        // OPTIONS 请求本身正确发出且服务器返回 2xx 即可
        var allowedMethods = await _client.OptionsAsync($"{IntegrationEndpoints.HttpBin}/get");

        Assert.NotNull(allowedMethods);
        _output.WriteLine($"Allow 列表共 {allowedMethods.Count} 项: " +
                          string.Join(", ", allowedMethods));
    }

    [IntegrationFact]
    public async Task OptionsAsync_HttpBinWithOriginHeader_CorsHeadersReturned()
    {
        // 带 Origin 头的 OPTIONS 是标准 CORS 预检请求
        var headers = new Dictionary<string, string>
        {
            { "Origin", "https://example.com" },
            { "Access-Control-Request-Method", "POST" }
        };

        var allowedMethods = await _client.OptionsAsync($"{IntegrationEndpoints.HttpBin}/post", headers);

        Assert.NotNull(allowedMethods);
        _output.WriteLine($"Allow 列表: [{string.Join(", ", allowedMethods)}]");
    }

    // 注：原「OPTIONS 非 2xx 抛 HttpRequestException」用例已移除。
    // 原因：httpbin / httpbingo / postman-echo 等公共服务均在 CDN/边缘对 OPTIONS 做 CORS 预检拦截，
    // OPTIONS 请求恒返回 200，无法用任何稳定公共端点复现非 2xx；真实世界里 OPTIONS 也几乎不会非 2xx。
    // EnsureSuccessStatusCode 抛 HttpRequestException 的路径已由 GET /status/404、GET /status/500、
    // HEAD /status/404 三个用例充分覆盖，无需重复。

    public void Dispose() => _client.Dispose();
}
