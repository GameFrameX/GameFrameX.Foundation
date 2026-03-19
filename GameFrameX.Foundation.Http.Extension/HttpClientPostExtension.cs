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

using System.Net.Http.Json;
using System.Text.Json;
using GameFrameX.Foundation.Json;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的POST请求扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for HttpClient POST requests.
/// </remarks>
public static class HttpClientPostExtension
{
    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data and reads the response content as a string.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.PostAsJsonAsync(url, data, JsonHelper.DefaultOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data using custom serialization options and reads the response content as a string.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.PostAsJsonAsync(url, data, jsonSerializerOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data and custom headers, then reads the response content as a string.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePostRequest(url, data, headers, JsonHelper.DefaultOptions);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data, custom headers and serialization options, then reads the response content as a string.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePostRequest(url, data, headers, jsonSerializerOptions);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data and reads the response content as a byte array.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字节数组形式 / The response content as a byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.PostAsJsonAsync(url, data, JsonHelper.DefaultOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data using custom serialization options and reads the response content as a byte array.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字节数组形式 / The response content as a byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.PostAsJsonAsync(url, data, jsonSerializerOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data and custom headers, then reads the response content as a byte array.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字节数组形式 / The response content as a byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePostRequest(url, data, headers, JsonHelper.DefaultOptions);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为字节数组。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data, custom headers and serialization options, then reads the response content as a byte array.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字节数组形式 / The response content as a byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<byte[]> PostJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePostRequest(url, data, headers, jsonSerializerOptions);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data and reads the response content as a stream.
    /// Note: The caller is responsible for disposing the returned stream.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的流形式，调用方需负责释放 / The response content as a stream; the caller must dispose it</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.PostAsJsonAsync(url, data, JsonHelper.DefaultOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data using custom serialization options and reads the response content as a stream.
    /// Note: The caller is responsible for disposing the returned stream.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的流形式，调用方需负责释放 / The response content as a stream; the caller must dispose it</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.PostAsJsonAsync(url, data, jsonSerializerOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data and custom headers, then reads the response content as a stream.
    /// Note: The caller is responsible for disposing the returned stream.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的流形式，调用方需负责释放 / The response content as a stream; the caller must dispose it</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePostRequest(url, data, headers, JsonHelper.DefaultOptions);
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将JSON数据序列化后发送，并将响应内容读取为流。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with JSON serialized data, custom headers and serialization options, then reads the response content as a stream.
    /// Note: The caller is responsible for disposing the returned stream.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的流形式，调用方需负责释放 / The response content as a stream; the caller must dispose it</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<Stream> PostJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePostRequest(url, data, headers, jsonSerializerOptions);
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将表单数据发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with form data and reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="formData">表单数据 / The form data</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostFormToStringAsync(this HttpClient httpClient, string url, IEnumerable<KeyValuePair<string, string>> formData, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var content = new FormUrlEncodedContent(formData);
        using var response = await httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将表单数据发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with form data and custom headers, then reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="formData">表单数据 / The form data</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostFormToStringAsync(this HttpClient httpClient, string url, IEnumerable<KeyValuePair<string, string>> formData, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new FormUrlEncodedContent(formData);

        if (headers is { Count: > 0 })
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将文件内容发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with file content and reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="filePath">文件路径 / The file path</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 或 <paramref name="filePath"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> or <paramref name="filePath"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostFileToStringAsync(this HttpClient httpClient, string url, string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        using var fileStream = File.OpenRead(filePath);
        using var content = new StreamContent(fileStream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        using var response = await httpClient.PostAsync(url, content, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送POST请求，将文件内容发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with file content and custom headers, then reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="filePath">文件路径 / The file path</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 或 <paramref name="filePath"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> or <paramref name="filePath"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostFileToStringAsync(this HttpClient httpClient, string url, string filePath, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var fileStream = File.OpenRead(filePath);
        using var content = new StreamContent(fileStream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = content;

        if (headers is { Count: > 0 })
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送POST请求，将文件内容作为Multipart表单发送，并将响应内容读取为字符串。
    /// </summary>
    /// <remarks>
    /// Sends a POST request with file content as multipart form data and reads the response content as a string.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="fileFieldName">文件字段名 / The file field name</param>
    /// <param name="filePath">文件路径 / The file path</param>
    /// <param name="formData">额外的表单数据 / Additional form data</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应内容的字符串形式 / The response content as a string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/>、<paramref name="fileFieldName"/> 或 <paramref name="filePath"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/>, <paramref name="fileFieldName"/>, or <paramref name="filePath"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
    public static async Task<string> PostMultipartFileToStringAsync(this HttpClient httpClient, string url, string fileFieldName, string filePath, Dictionary<string, string>? formData = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        ArgumentException.ThrowIfNullOrWhiteSpace(fileFieldName, nameof(fileFieldName));
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        using var multipartContent = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        multipartContent.Add(fileContent, fileFieldName, Path.GetFileName(filePath));

        if (formData is { Count: > 0 })
        {
            foreach (var field in formData)
            {
                multipartContent.Add(new StringContent(field.Value), field.Key);
            }
        }

        using var response = await httpClient.PostAsync(url, multipartContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 创建POST请求消息（带JSON内容）。
    /// </summary>
    /// <remarks>
    /// Creates a POST request message with JSON content and custom headers.
    /// </remarks>
    /// <typeparam name="TValue">要发送的数据类型 / The type of data to send</typeparam>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="data">要发送的数据 / The data to send</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项 / The JSON serialization options</param>
    /// <returns>HttpRequestMessage实例 / The HttpRequestMessage instance</returns>
    private static HttpRequestMessage CreatePostRequest<TValue>(string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(data, options: jsonSerializerOptions)
        };

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
