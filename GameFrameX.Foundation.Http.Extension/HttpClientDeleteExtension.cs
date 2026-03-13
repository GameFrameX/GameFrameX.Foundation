namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的DELETE请求扩展方法
/// </summary>
public static class HttpClientDeleteExtension
{
    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<string> DeleteToStringAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.DeleteAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
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
    /// 发送DELETE请求，并将响应内容读取为字节数组
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<byte[]> DeleteToByteArrayAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.DeleteAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送DELETE请求，并将响应内容读取为字节数组
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
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
    /// 创建DELETE请求消息
    /// </summary>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <returns>HttpRequestMessage实例</returns>
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
