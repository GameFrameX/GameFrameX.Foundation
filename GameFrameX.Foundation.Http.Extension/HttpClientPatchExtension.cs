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
/// HttpClient的PATCH请求扩展方法
/// </summary>
public static class HttpClientPatchExtension
{
    /// <summary>
    /// 发送PATCH请求，将JSON数据序列化后发送，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<string> PatchJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.PatchAsJsonAsync(url, data, JsonHelper.DefaultOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// 发送PATCH请求，将JSON数据序列化后发送，并将响应内容读取为字符串
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字符串形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<string> PatchJsonToStringAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePatchRequest(url, data, headers, JsonHelper.DefaultOptions);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cts.Token);
    }

    /// <summary>
    /// 发送PATCH请求，将JSON数据序列化后发送，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<byte[]> PatchJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        using var response = await httpClient.PatchAsJsonAsync(url, data, JsonHelper.DefaultOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cancellationToken);
    }

    /// <summary>
    /// 发送PATCH请求，将JSON数据序列化后发送，并将响应内容读取为字节数组
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的字节数组形式</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<byte[]> PatchJsonToByteArrayAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePatchRequest(url, data, headers, JsonHelper.DefaultOptions);
        using var response = await httpClient.SendAsync(request, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync(cts.Token);
    }

    /// <summary>
    /// 发送PATCH请求，将JSON数据序列化后发送，并将响应内容读取为流。
    /// 注意：调用方负责释放返回的流。
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式，调用方需负责释放</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<Stream> PatchJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.PatchAsJsonAsync(url, data, JsonHelper.DefaultOptions, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }

    /// <summary>
    /// 发送PATCH请求，将JSON数据序列化后发送，并将响应内容读取为流。
    /// 注意：调用方负责释放返回的流。
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="httpClient">HttpClient实例</param>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="timeout">超时时间(秒)，默认10秒</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>响应内容的流形式，调用方需负责释放</returns>
    /// <exception cref="ArgumentNullException">当httpClient或url为null时抛出</exception>
    /// <exception cref="ArgumentException">当url为空字符串或空白字符串时抛出</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出</exception>
    public static async Task<Stream> PatchJsonToStreamAsync<TValue>(this HttpClient httpClient, string url, TValue data, IDictionary<string, string> headers, int timeout = 10, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeout));

        using var request = CreatePatchRequest(url, data, headers, JsonHelper.DefaultOptions);
        // 使用 ResponseHeadersRead 避免将全部响应体缓冲到内存
        // response 的生命周期由返回的 Stream 内部管理（.NET 会在流关闭时释放 response）
        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cts.Token);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cts.Token);
    }

    /// <summary>
    /// 创建PATCH请求消息（带JSON内容）
    /// </summary>
    /// <typeparam name="TValue">要发送的数据类型</typeparam>
    /// <param name="url">请求URL</param>
    /// <param name="data">要发送的数据</param>
    /// <param name="headers">请求头字典</param>
    /// <param name="jsonSerializerOptions">JSON序列化选项</param>
    /// <returns>HttpRequestMessage实例</returns>
    private static HttpRequestMessage CreatePatchRequest<TValue>(string url, TValue data, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, url)
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
