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

    /// <summary>
    /// 获取链路追踪标识（TrackId），用于在一次请求的响应、日志与下游调用之间建立关联。
    /// <para>成功或失败响应均可携带；为空表示未启用追踪。该属性不参与 <see cref="IsSuccess"/> 判定。</para>
    /// </summary>
    /// <remarks>
    /// Gets the track identifier (TrackId) used to correlate a request across its response, logs, and downstream calls.
    /// <para>May be present on both success and failure responses; empty when tracking is not enabled. This property does not participate in <see cref="IsSuccess"/> evaluation.</para>
    /// </remarks>
    /// <value>链路追踪标识 / Track identifier</value>
    string TrackId { get; }

    /// <summary>
    /// 获取稳定业务错误码。失败响应承载具体业务原因，成功为空。与 <see cref="Code"/> 正交。
    /// </summary>
    /// <remarks>Gets the stable business error code; carries the business reason on failure, empty on success. Orthogonal to <see cref="Code"/>.</remarks>
    /// <value>稳定业务错误码 / Stable business error code</value>
    string ErrorCode { get; }

    /// <summary>
    /// 获取响应类型（如 success、warning、error），补充 <see cref="Code"/> 无法表达的状态语义。
    /// </summary>
    /// <remarks>Gets the response type (e.g. success/warning/error), supplementing status semantics beyond <see cref="Code"/>.</remarks>
    /// <value>响应类型 / Response type</value>
    string Type { get; }

    /// <summary>
    /// 获取响应生成的 UTC 时间戳（秒）。
    /// </summary>
    /// <remarks>Gets the UTC timestamp (seconds) at which the response was generated.</remarks>
    /// <value>UTC 时间戳（秒） / UTC timestamp in seconds</value>
    long Time { get; }

    /// <summary>
    /// 获取附加数据对象，用于承载主数据之外的额外元信息。
    /// <para>成功或失败响应均可携带；为 <c>null</c> 时不参与序列化。</para>
    /// </summary>
    /// <remarks>Gets the extras object carrying additional metadata beyond the main data. May be present on both success and failure responses; not serialized when <c>null</c>.</remarks>
    /// <value>附加数据 / Extras</value>
    object Extras { get; }
}