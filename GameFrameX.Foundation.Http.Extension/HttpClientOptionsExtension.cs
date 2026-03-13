namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的OPTIONS请求扩展方法
/// </summary>
public static class HttpClientOptionsExtension
{
    /// <summary>
    /// 发送OPTIONS请求，并返回服务端支持的HTTP方法列表
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>服务端支持的HTTP方法集合（来自响应头的 Allow 字段）</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
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
    /// 发送OPTIONS请求，并返回服务端支持的HTTP方法列表
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>服务端支持的HTTP方法集合（来自响应头的 Allow 字段）</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
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
    /// 创建OPTIONS请求消息
    /// </summary>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <returns>HttpRequestMessage实例</returns>
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
