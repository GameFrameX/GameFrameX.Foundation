using System.Net.Http.Json;
using System.Text.Json;
using GameFrameX.Foundation.Json;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的POST请求扩展方法
/// </summary>
public static class HttpClientPostExtension
{
    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, JsonHelper.DefaultOptions, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, jsonSerializerOptions, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
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

        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, JsonHelper.DefaultOptions, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, int timeout = 10, CancellationToken cancellationToken = default)
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

        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, jsonSerializerOptions, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, JsonHelper.DefaultOptions, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, jsonSerializerOptions, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
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

        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, JsonHelper.DefaultOptions, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, int timeout = 10, CancellationToken cancellationToken = default)
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

        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, jsonSerializerOptions, cancellationToken);
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, JsonHelper.DefaultOptions, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, jsonSerializerOptions, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
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

        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, JsonHelper.DefaultOptions, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, int timeout = 10, CancellationToken cancellationToken = default)
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

        var response = await httpClient.PostAsJsonAsync<TValue>(url, data, jsonSerializerOptions, cancellationToken);
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将表单数据发送，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="formData">表单数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostFormToStringAsync(this HttpClient httpClient, string url, Dictionary<string, string> formData, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        using var content = new FormUrlEncodedContent(formData);
        var response = await httpClient.PostAsync(url, content, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将表单数据发送，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="formData">表单数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostFormToStringAsync(this HttpClient httpClient, string url, Dictionary<string, string> formData, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
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

        using var content = new FormUrlEncodedContent(formData);
        var response = await httpClient.PostAsync(url, content, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将文件内容发送，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostFileToStringAsync(this HttpClient httpClient, string url, string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));

        using (var fileStream = File.OpenRead(filePath))
        {
            using (var content = new StreamContent(fileStream))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                var response = await httpClient.PostAsync(url, content, cancellationToken);
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
        }
    }

    /// <summary>
    /// 发送POST请求，将文件内容发送，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostFileToStringAsync(this HttpClient httpClient, string url, string filePath, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));

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

        using (var fileStream = File.OpenRead(filePath))
        {
            using (var content = new StreamContent(fileStream))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                var response = await httpClient.PostAsync(url, content, cancellationToken);
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
        }
    }

    /// <summary>
    /// 发送POST请求，将文件内容作为Multipart表单发送，并将响应内容读取为字符串
    /// </summary>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="fileFieldName">文件字段名</param>
    /// <param name="filePath">文件路径</param>
    /// <param name="formData">额外的表单数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    public static async Task<string> PostMultipartFileToStringAsync(this HttpClient httpClient, string url, string fileFieldName, string filePath, Dictionary<string, string>? formData = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(url, nameof(url));
        ArgumentNullException.ThrowIfNull(fileFieldName, nameof(fileFieldName));
        ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));

        using (var multipartContent = new MultipartFormDataContent())
        {
            using (var fileStream = File.OpenRead(filePath))
            {
                using (var fileContent = new StreamContent(fileStream))
                {
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    multipartContent.Add(fileContent, fileFieldName, Path.GetFileName(filePath));

                    if (formData is { Count: > 0 })
                    {
                        foreach (var field in formData)
                        {
                            multipartContent.Add(new StringContent(field.Value), field.Key);
                        }
                    }

                    var response = await httpClient.PostAsync(url, multipartContent, cancellationToken);
                    return await response.Content.ReadAsStringAsync(cancellationToken);
                }
            }
        }
    }
}