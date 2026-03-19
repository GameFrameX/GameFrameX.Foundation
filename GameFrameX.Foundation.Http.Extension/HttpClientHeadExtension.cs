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

using System.Net.Http.Headers;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// HttpClient的HEAD请求扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for HttpClient HEAD requests.
/// </remarks>
public static class HttpClientHeadExtension
{
    /// <summary>
    /// 发送HEAD请求，并返回响应头集合。
    /// </summary>
    /// <remarks>
    /// Sends a HEAD request and returns the response headers collection.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应头集合（调用方可访问 ETag、Content-Length 等强类型属性）/ Response headers collection (caller can access strongly-typed properties like ETag, Content-Length)</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
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
    /// 发送HEAD请求，并返回响应头集合。
    /// </summary>
    /// <remarks>
    /// Sends a HEAD request with custom headers and returns the response headers collection.
    /// </remarks>
    /// <param name="httpClient">HttpClient实例 / The HttpClient instance</param>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <param name="timeout">超时时间(秒)，默认10秒 / Timeout in seconds, default is 10 seconds</param>
    /// <param name="cancellationToken">取消令牌 / The cancellation token</param>
    /// <returns>响应头集合（调用方可访问 ETag、Content-Length 等强类型属性）/ Response headers collection (caller can access strongly-typed properties like ETag, Content-Length)</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="httpClient"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="httpClient"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空字符串或空白字符串时抛出 / Thrown when <paramref name="url"/> is empty or whitespace</exception>
    /// <exception cref="HttpRequestException">当HTTP响应状态码表示失败时抛出 / Thrown when the HTTP response status code indicates failure</exception>
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
    /// 创建HEAD请求消息。
    /// </summary>
    /// <remarks>
    /// Creates a HEAD request message with custom headers.
    /// </remarks>
    /// <param name="url">请求URL / The request URL</param>
    /// <param name="headers">请求头字典 / The request headers dictionary</param>
    /// <returns>HttpRequestMessage实例 / The HttpRequestMessage instance</returns>
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
