using System.Net.Http.Headers;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的HEAD请求扩展方法
/// </summary>
public static class HttpClientHeadExtension
{
    /// <summary>
    /// 发送HEAD请求，并返回响应头集合
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应头集合（调用方可访问 ETag、Content-Length 等强类型属性）</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<HttpResponseHeaders> HeadAsync(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var request = new HttpRequestMessage(HttpMethod.Head, url);
        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response.Headers;
    }

    /// <summary>
    /// 发送HEAD请求，并返回响应头集合
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应头集合（调用方可访问 ETag、Content-Length 等强类型属性）</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<HttpResponseHeaders> HeadAsync(this HttpClient httpClient, string url, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreateHeadRequest(url, headers);
        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
        response.EnsureSuccessStatusCode();
        return response.Headers;
    }

    /// <summary>
    /// 创建HEAD请求消息
    /// </summary>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <returns>HttpRequestMessage实例</returns>
    private static HttpRequestMessage CreateHeadRequest(string url, IDictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Head, url);

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
