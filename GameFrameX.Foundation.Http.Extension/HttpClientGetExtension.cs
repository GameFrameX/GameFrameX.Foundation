namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的GET请求扩展方法
/// </summary>
public static class HttpClientGetExtension
{
    /// <summary>
    /// 发送GET请求，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">响应数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> GetToStringAsync<TValue>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.GetAsync(url, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">响应数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> GetToStringAsync<TValue>(this HttpClient httpClient, string url, IDictionary<string, string> headers = null, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);
        if (headers is { Count: > 0 })
        {
            foreach (var header in headers)
            {
                if (httpClient.DefaultRequestHeaders.Contains(header.Key))
                {
                    httpClient.DefaultRequestHeaders.Remove(header.Key);
                }

                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await httpClient.GetAsync(url, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">响应数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<byte[]> GetToByteArrayAsync<TValue>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.GetAsync(url, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">响应数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<byte[]> GetToByteArrayAsync<TValue>(this HttpClient httpClient, string url, IDictionary<string, string> headers = null, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);
        if (headers is { Count: > 0 })
        {
            foreach (var header in headers)
            {
                if (httpClient.DefaultRequestHeaders.Contains(header.Key))
                {
                    httpClient.DefaultRequestHeaders.Remove(header.Key);
                }

                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await httpClient.GetAsync(url, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为流
    /// </summary>
    /// <typeparam name="TValue">响应数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<Stream> GetToStreamAsync<TValue>(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.GetAsync(url, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送GET请求，并将响应内容读取为流
    /// </summary>
    /// <typeparam name="TValue">响应数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<Stream> GetToStreamAsync<TValue>(this HttpClient httpClient, string url, IDictionary<string, string> headers = null, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        httpClient.Timeout = TimeSpan.FromSeconds(timeout);
        if (headers is { Count: > 0 })
        {
            foreach (var header in headers)
            {
                if (httpClient.DefaultRequestHeaders.Contains(header.Key))
                {
                    httpClient.DefaultRequestHeaders.Remove(header.Key);
                }

                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url), cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}