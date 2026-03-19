namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的OPTIONS请求扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for HttpClient OPTIONS requests.
/// </remarks>
public static class HttpClientOptionsExtension
{
    /// <summary>
    /// 发送OPTIONS请求，并返回服务端支持的HTTP方法列表。
    /// </summary>
    /// <remarks>
    /// Sends an OPTIONS request and returns the list of HTTP methods supported by the server.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>服务端支持的HTTP方法集合（来自响应头的 Allow 字段）/ Collection of HTTP methods supported by the server (from the Allow header)</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<IReadOnlyCollection<string>> OptionsAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var request = new HttpRequestMessage(HttpMethod.Options, url);
        using var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response.Content.Headers.Allow.ToList().AsReadOnly();
    }

    /// <summary>
    /// 发送OPTIONS请求，并返回服务端支持的HTTP方法列表。
    /// </summary>
    /// <remarks>
    /// Sends an OPTIONS request with custom headers and returns the list of HTTP methods supported by the server.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>服务端支持的HTTP方法集合（来自响应头的 Allow 字段）/ Collection of HTTP methods supported by the server (from the Allow header)</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<IReadOnlyCollection<string>> OptionsAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateOptionsRequest(url, headers);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return response.Content.Headers.Allow.ToList().AsReadOnly();
    }

    /// <summary>
    /// 创建OPTIONS请求消息。
    /// </summary>
    /// <remarks>
    /// Creates an OPTIONS request message with custom headers.
    /// </remarks>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <returns>HttpRequestMessage实例 / The HttpRequestMessage instance</returns>
    private static HttpRequestMessage CreateOptionsRequest(string url, IDictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Options, url);

        if (headers is { Count: > 0 })
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return request;
    }
}
