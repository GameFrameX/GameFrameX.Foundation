namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的DELETE请求扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for HttpClient DELETE requests.
/// </remarks>
public static class HttpClientDeleteExtension
{
    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a DELETE request and reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> DeleteToStringAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.DeleteAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a DELETE request with custom headers and reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> DeleteToStringAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateDeleteRequest(url, headers);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字节数组。
    /// </summary>
    /// <remarks>
    /// Sends a DELETE request and reads the response content as a byte array.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字节数组形式 / The response content as a byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<byte[]> DeleteToByteArrayAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.DeleteAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字节数组。
    /// </summary>
    /// <remarks>
    /// Sends a DELETE request with custom headers and reads the response content as a byte array.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字节数组形式 / The response content as a byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<byte[]> DeleteToByteArrayAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateDeleteRequest(url, headers);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cts.Token);
    }

    /// <summary>
    /// 创建DELETE请求消息。
    /// </summary>
    /// <remarks>
    /// Creates a DELETE request message with custom headers.
    /// </remarks>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <returns>HttpRequestMessage实例 / The HttpRequestMessage instance</returns>
    private static HttpRequestMessage CreateDeleteRequest(string url, IDictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);

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
