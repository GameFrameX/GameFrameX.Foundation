namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的GET请求扩展方法
/// </summary>
public static class HttpClientGetExtension
{
    /// <summary>
    /// 发送GET请求，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<string> GetToStringAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为字符串
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
    public static async Task<string> GetToStringAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateGetRequest(url, headers);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为字节数组
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<byte[]> GetToByteArrayAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为字节数组
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
    public static async Task<byte[]> GetToByteArrayAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateGetRequest(url, headers);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cts.Token);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为流。
    /// 注意：调用方负责释放返回的流。
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式，调用方需负责释放</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<Stream> GetToStreamAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为流。
    /// 注意：调用方负责释放返回的流。
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式，调用方需负责释放</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<Stream> GetToStreamAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateGetRequest(url, headers);
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cts.Token);
    }

    /// <summary>
    /// 创建GET请求消息
    /// </summary>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <returns>HttpRequestMessage实例</returns>
    private static HttpRequestMessage CreateGetRequest(string url, IDictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

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
