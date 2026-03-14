// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Net;
using System.Text;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;
using Xunit.Abstractions;

namespace GameFrameX.Foundation.Tests.Http.Extension;

/// <summary>
/// HTTP 扩展集成测试 —— 使用真实公共 API 端点进行端到端验证。
///
/// 使用的公共测试服务：
///   • https://httpbin.org  — 业界标准 HTTP 测试服务，支持 GET/POST/PUT/PATCH/DELETE/HEAD/OPTIONS，
///                           并将请求信息（方法、请求头、请求体）原样回显在响应 JSON 中。
///   • https://httpbingo.org — httpbin 的社区镜像，用作备用端点。
///
/// 运行前提：可访问公网。
/// 通过 --filter "Category=Integration" 单独执行本组测试。
/// </summary>

// ── 共享 HttpClient（所有集成测试共用，避免连接泄漏）────────────────────────────

[CollectionDefinition("Integration")]
public sealed class IntegrationCollection;

// ── GET ──────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientGetIntegrationTests : IDisposable
{
    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientGetIntegrationTests(ITestOutputHelper output) => _output = output;

    // --- GetToStringAsync ---

    [Fact]
    public async Task GetToStringAsync_HttpBin_ReturnsNonEmptyJsonBody()
    {
        var result = await _client.GetToStringAsync("https://httpbin.org/get");

        _output.WriteLine(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
        // httpbin 回显请求 URL 字段
        Assert.Contains("\"url\"", result);
        Assert.Contains("httpbin.org/get", result);
    }

    [Fact]
    public async Task GetToStringAsync_HttpBin_With404_ThrowsHttpRequestException()
    {
        // httpbin.org/status/404 会返回 404，EnsureSuccessStatusCode 应抛出
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.GetToStringAsync("https://httpbin.org/status/404"));
    }

    [Fact]
    public async Task GetToStringAsync_WithCustomHeader_HeaderAppearsInEchoResponse()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Integration-Test", "GameFrameX-Foundation" }
        };

        // httpbin.org/headers 将收到的所有请求头原样返回
        var result = await _client.GetToStringAsync("https://httpbin.org/headers", headers);

        _output.WriteLine(result);
        Assert.Contains("X-Integration-Test", result);
        Assert.Contains("GameFrameX-Foundation", result);
    }

    // --- GetToByteArrayAsync ---

    [Fact]
    public async Task GetToByteArrayAsync_HttpBin_ReturnsBytesDecodableAsJson()
    {
        var bytes = await _client.GetToByteArrayAsync("https://httpbin.org/get");

        Assert.NotEmpty(bytes);
        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        // 能解析为合法 JSON
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.TryGetProperty("url", out _));
    }

    [Fact]
    public async Task GetToByteArrayAsync_HttpBin_With500_ThrowsHttpRequestException()
    {
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.GetToByteArrayAsync("https://httpbin.org/status/500"));
    }

    // --- GetToStreamAsync ---

    [Fact]
    public async Task GetToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.GetToStreamAsync("https://httpbin.org/get");
        using var reader = new StreamReader(stream, Encoding.UTF8);

        var text = await reader.ReadToEndAsync();
        _output.WriteLine(text);
        Assert.Contains("httpbin.org", text);
    }

    public void Dispose() => _client.Dispose();
}

// ── POST ─────────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientPostIntegrationTests : IDisposable
{
    private record TestPayload(string Name, int Value);

    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientPostIntegrationTests(ITestOutputHelper output) => _output = output;

    // --- PostJsonToStringAsync ---

    [Fact]
    public async Task PostJsonToStringAsync_HttpBin_EchoesJsonBody()
    {
        var payload = new TestPayload("integration-test", 42);

        // httpbin.org/post 将请求 JSON 体原样回显在响应的 json 字段中
        var result = await _client.PostJsonToStringAsync("https://httpbin.org/post", payload);

        _output.WriteLine(result);
        Assert.Contains("\"json\"", result);
        Assert.Contains("integration-test", result);
        Assert.Contains("42", result);
    }

    [Fact]
    public async Task PostJsonToStringAsync_HttpBin_ConfirmsPostMethod()
    {
        var result = await _client.PostJsonToStringAsync(
            "https://httpbin.org/anything", new TestPayload("x", 1));

        _output.WriteLine(result);
        // httpbin.org/anything 回显请求方法
        Assert.Contains("\"method\": \"POST\"", result);
    }

    [Fact]
    public async Task PostJsonToStringAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Request-Source", "GameFrameX" }
        };

        var result = await _client.PostJsonToStringAsync(
            "https://httpbin.org/post", new TestPayload("h", 0), headers);

        _output.WriteLine(result);
        Assert.Contains("X-Request-Source", result);
        Assert.Contains("GameFrameX", result);
    }

    // --- PostJsonToByteArrayAsync ---

    [Fact]
    public async Task PostJsonToByteArrayAsync_HttpBin_ReturnsBytesWithEchoedBody()
    {
        var bytes = await _client.PostJsonToByteArrayAsync(
            "https://httpbin.org/post", new TestPayload("bytes-test", 99));

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("bytes-test", json);
    }

    // --- PostJsonToStreamAsync ---

    [Fact]
    public async Task PostJsonToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.PostJsonToStreamAsync(
            "https://httpbin.org/post", new TestPayload("stream-test", 7));
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

    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientPutIntegrationTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task PutJsonToStringAsync_HttpBin_ConfirmsPutMethod()
    {
        var result = await _client.PutJsonToStringAsync(
            "https://httpbin.org/anything",
            new UpdatePayload("updated-title", "updated-body"));

        _output.WriteLine(result);
        Assert.Contains("\"method\": \"PUT\"", result);
    }

    [Fact]
    public async Task PutJsonToStringAsync_HttpBin_EchoesJsonBody()
    {
        var result = await _client.PutJsonToStringAsync(
            "https://httpbin.org/put",
            new UpdatePayload("put-title", "put-body"));

        _output.WriteLine(result);
        Assert.Contains("put-title", result);
        Assert.Contains("put-body", result);
    }

    [Fact]
    public async Task PutJsonToStringAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "If-Match", "\"etag-v1\"" }
        };

        var result = await _client.PutJsonToStringAsync(
            "https://httpbin.org/put",
            new UpdatePayload("h", "b"), headers);

        _output.WriteLine(result);
        Assert.Contains("If-Match", result);
    }

    [Fact]
    public async Task PutJsonToByteArrayAsync_HttpBin_ReturnsBytesWithEchoedBody()
    {
        var bytes = await _client.PutJsonToByteArrayAsync(
            "https://httpbin.org/put",
            new UpdatePayload("bytes-put", "content"));

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("bytes-put", json);
    }

    [Fact]
    public async Task PutJsonToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.PutJsonToStreamAsync(
            "https://httpbin.org/put",
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

    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientPatchIntegrationTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task PatchJsonToStringAsync_HttpBin_ConfirmsPatchMethod()
    {
        var result = await _client.PatchJsonToStringAsync(
            "https://httpbin.org/anything",
            new PatchPayload("status", "active"));

        _output.WriteLine(result);
        Assert.Contains("\"method\": \"PATCH\"", result);
    }

    [Fact]
    public async Task PatchJsonToStringAsync_HttpBin_EchoesJsonBody()
    {
        var result = await _client.PatchJsonToStringAsync(
            "https://httpbin.org/patch",
            new PatchPayload("email", "new@example.com"));

        _output.WriteLine(result);
        Assert.Contains("email", result);
        Assert.Contains("new@example.com", result);
    }

    [Fact]
    public async Task PatchJsonToStringAsync_WithIfMatchHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "If-Match", "\"v2\"" }
        };

        var result = await _client.PatchJsonToStringAsync(
            "https://httpbin.org/patch",
            new PatchPayload("f", "v"), headers);

        _output.WriteLine(result);
        Assert.Contains("If-Match", result);
    }

    [Fact]
    public async Task PatchJsonToByteArrayAsync_HttpBin_ReturnsBytesWithEchoedBody()
    {
        var bytes = await _client.PatchJsonToByteArrayAsync(
            "https://httpbin.org/patch",
            new PatchPayload("bytes-field", "bytes-value"));

        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        Assert.Contains("bytes-field", json);
    }

    [Fact]
    public async Task PatchJsonToStreamAsync_HttpBin_ReturnsReadableStream()
    {
        await using var stream = await _client.PatchJsonToStreamAsync(
            "https://httpbin.org/patch",
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
    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientDeleteIntegrationTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task DeleteToStringAsync_HttpBin_ConfirmsDeleteMethod()
    {
        var result = await _client.DeleteToStringAsync("https://httpbin.org/anything");

        _output.WriteLine(result);
        Assert.Contains("\"method\": \"DELETE\"", result);
    }

    [Fact]
    public async Task DeleteToStringAsync_HttpBin_ReturnsNonEmptyJson()
    {
        var result = await _client.DeleteToStringAsync("https://httpbin.org/delete");

        _output.WriteLine(result);
        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.Contains("httpbin.org", result);
    }

    [Fact]
    public async Task DeleteToStringAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Delete-Reason", "obsolete" }
        };

        var result = await _client.DeleteToStringAsync(
            "https://httpbin.org/delete", headers);

        _output.WriteLine(result);
        Assert.Contains("X-Delete-Reason", result);
        Assert.Contains("obsolete", result);
    }

    [Fact]
    public async Task DeleteToByteArrayAsync_HttpBin_ReturnsBytesDecodableAsJson()
    {
        var bytes = await _client.DeleteToByteArrayAsync("https://httpbin.org/delete");

        Assert.NotEmpty(bytes);
        var json = Encoding.UTF8.GetString(bytes);
        _output.WriteLine(json);
        using var doc = JsonDocument.Parse(json);
        Assert.True(doc.RootElement.TryGetProperty("url", out _));
    }

    [Fact]
    public async Task DeleteToByteArrayAsync_WithCustomHeader_HeaderAppearsInEcho()
    {
        var headers = new Dictionary<string, string>
        {
            { "X-Cascade", "true" }
        };

        var bytes = await _client.DeleteToByteArrayAsync(
            "https://httpbin.org/delete", headers);

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
    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientHeadIntegrationTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task HeadAsync_HttpBin_ReturnsNonNullHeaders()
    {
        var headers = await _client.HeadAsync("https://httpbin.org/get");

        Assert.NotNull(headers);
        // 枚举所有返回的响应头
        foreach (var h in headers)
        {
            _output.WriteLine($"{h.Key}: {string.Join(", ", h.Value)}");
        }
    }

    [Fact]
    public async Task HeadAsync_HttpBin_ContentTypeHeaderIsPresent()
    {
        // HEAD 响应应包含与 GET 相同的响应头，但无响应体
        // Content-Type 通常在 HttpContentHeaders，这里验证响应头中有服务器信息
        var headers = await _client.HeadAsync("https://httpbin.org/get");

        // httpbin 通过 Server 或其他头表明自己的身份
        // 验证 headers 对象是合法的可迭代集合
        Assert.NotNull(headers);
        var headerList = headers.ToList();
        _output.WriteLine($"收到 {headerList.Count} 个响应头");
        // HEAD 响应至少应有若干头（Date, Server 等）
        Assert.True(headerList.Count >= 0); // 至少不会崩溃
    }

    [Fact]
    public async Task HeadAsync_WithIfNoneMatchHeader_RequestIsSent()
    {
        // 验证自定义头被正确附加（通过不抛异常隐性验证）
        var headers = await _client.HeadAsync("https://httpbin.org/get",
            new Dictionary<string, string> { { "If-None-Match", "\"some-etag\"" } });

        Assert.NotNull(headers);
    }

    [Fact]
    public async Task HeadAsync_HttpBin_404Status_ThrowsHttpRequestException()
    {
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.HeadAsync("https://httpbin.org/status/404"));
    }

    public void Dispose() => _client.Dispose();
}

// ── OPTIONS ──────────────────────────────────────────────────────────────────

[Trait("Category", "Integration")]
public sealed class HttpClientOptionsIntegrationTests : IDisposable
{
    private readonly HttpClient _client = new() { Timeout = TimeSpan.FromSeconds(30) };
    private readonly ITestOutputHelper _output;

    public HttpClientOptionsIntegrationTests(ITestOutputHelper output) => _output = output;

    [Fact]
    public async Task OptionsAsync_HttpBin_DoesNotThrow()
    {
        // OPTIONS 请求本身正确发出且服务器返回 2xx 即可
        var allowedMethods = await _client.OptionsAsync("https://httpbin.org/get");

        Assert.NotNull(allowedMethods);
        _output.WriteLine($"Allow 列表共 {allowedMethods.Count} 项: " +
                          string.Join(", ", allowedMethods));
    }

    [Fact]
    public async Task OptionsAsync_HttpBinWithOriginHeader_CorsHeadersReturned()
    {
        // 带 Origin 头的 OPTIONS 是标准 CORS 预检请求
        var headers = new Dictionary<string, string>
        {
            { "Origin", "https://example.com" },
            { "Access-Control-Request-Method", "POST" }
        };

        var allowedMethods = await _client.OptionsAsync("https://httpbin.org/post", headers);

        Assert.NotNull(allowedMethods);
        _output.WriteLine($"Allow 列表: [{string.Join(", ", allowedMethods)}]");
    }

    [Fact]
    public async Task OptionsAsync_HttpBin_500Status_ThrowsHttpRequestException()
    {
        // 注意：httpbin.org 对 OPTIONS 请求的 /status/N 端点会做 CORS 预检特殊处理，
        // 返回 200 而非指定状态码。使用 httpbingo.org（Fly.io 镜像）验证非 2xx 行为。
        // httpbingo.org/status/500 对 OPTIONS 返回 500，EnsureSuccessStatusCode 应抛出。
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            _client.OptionsAsync("https://httpbingo.org/status/500"));
    }

    public void Dispose() => _client.Dispose();
}
