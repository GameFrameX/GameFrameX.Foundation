namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP JSON 响应结果的通用接口
/// <para>
/// 定义了 HTTP 响应的标准结构，包括响应码、消息和数据。
/// </para>
/// </summary>
public interface IHttpJsonResult
{
    /// <summary>
    /// 响应码，0表示成功，其他值表示不同的错误类型。
    /// </summary>
    int Code { get; }

    /// <summary>
    /// 响应消息，提供关于请求结果的详细信息。
    /// </summary>
    string Message { get; }

    /// <summary>
    /// 响应数据，包含请求成功时返回的具体数据内容。
    /// </summary>
    string Data { get; }

    /// <summary>
    /// 是否成功，根据响应码自动判断（Code为0时返回true）。
    /// </summary>
    bool IsSuccess { get; }
}
