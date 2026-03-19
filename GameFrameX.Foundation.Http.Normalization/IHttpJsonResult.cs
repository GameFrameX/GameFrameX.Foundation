namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP JSON 响应结果的通用接口。
/// <para>
/// 定义了 HTTP 响应的标准结构，包括响应码、消息和数据。
/// </para>
/// </summary>
/// <remarks>
/// Common interface for HTTP JSON response results.
/// <para>
/// Defines the standard structure of HTTP responses, including response code, message, and data.
/// </para>
/// </remarks>
public interface IHttpJsonResult
{
    /// <summary>
    /// 获取响应码，0表示成功，其他值表示不同的错误类型。
    /// </summary>
    /// <remarks>
    /// Gets the response code. 0 indicates success, other values indicate different error types.
    /// </remarks>
    /// <value>响应码 / Response code</value>
    int Code { get; }

    /// <summary>
    /// 获取响应消息，提供关于请求结果的详细信息。
    /// </summary>
    /// <remarks>
    /// Gets the response message that provides detailed information about the request result.
    /// </remarks>
    /// <value>响应消息 / Response message</value>
    string Message { get; }

    /// <summary>
    /// 获取响应数据，包含请求成功时返回的具体数据内容。
    /// </summary>
    /// <remarks>
    /// Gets the response data containing the specific data content returned when the request succeeds.
    /// </remarks>
    /// <value>响应数据 / Response data</value>
    string Data { get; }

    /// <summary>
    /// 获取是否成功，根据响应码自动判断（Code为0时返回true）。
    /// </summary>
    /// <remarks>
    /// Gets whether the request is successful, automatically determined by the response code (returns true when Code is 0).
    /// </remarks>
    /// <value>如果成功则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if successful; otherwise <c>false</c></value>
    bool IsSuccess { get; }
}
