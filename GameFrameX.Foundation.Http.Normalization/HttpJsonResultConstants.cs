namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP JSON 结果常量定义。
/// 提供统一的错误码和消息常量，用于 HttpJsonResult 和 HttpJsonResultData&lt;T&gt; 的工厂方法。
/// 该类为内部类，仅供程序集内部使用。
/// </summary>
/// <remarks>
/// HTTP JSON result constants definition.
/// Provides unified error codes and message constants for factory methods of HttpJsonResult and HttpJsonResultData&lt;T&gt;.
/// This class is internal and only used within the assembly.
/// </remarks>
internal static class HttpJsonResultConstants
{
    #region 状态码常量

    /// <summary>
    /// 成功状态码。
    /// </summary>
    /// <remarks>
    /// Success status code.
    /// </remarks>
    public const int SuccessCode = 0;

    /// <summary>
    /// 一般性失败状态码。
    /// </summary>
    /// <remarks>
    /// General failure status code.
    /// </remarks>
    public const int FailCode = -1;

    /// <summary>
    /// 验证失败状态码 (HTTP 400)。
    /// </summary>
    /// <remarks>
    /// Validation failure status code (HTTP 400).
    /// </remarks>
    public const int ValidationErrorCode = 400;

    /// <summary>
    /// 未授权状态码 (HTTP 401)。
    /// </summary>
    /// <remarks>
    /// Unauthorized status code (HTTP 401).
    /// </remarks>
    public const int UnauthorizedCode = 401;

    /// <summary>
    /// 参数错误状态码 (HTTP 403)。
    /// </summary>
    /// <remarks>
    /// Parameter error status code (HTTP 403).
    /// </remarks>
    public const int ParamErrorCode = 403;

    /// <summary>
    /// 资源未找到状态码 (HTTP 404)。
    /// </summary>
    /// <remarks>
    /// Resource not found status code (HTTP 404).
    /// </remarks>
    public const int NotFoundCode = 404;

    /// <summary>
    /// 服务器内部错误状态码 (HTTP 500)。
    /// </summary>
    /// <remarks>
    /// Internal server error status code (HTTP 500).
    /// </remarks>
    public const int ServerErrorCode = 500;

    #endregion

    #region 消息常量

    /// <summary>
    /// 验证失败默认消息。
    /// </summary>
    /// <remarks>
    /// Default validation failure message.
    /// </remarks>
    public const string ValidationErrorMsg = "Validation failed.";

    /// <summary>
    /// 未授权默认消息。
    /// </summary>
    /// <remarks>
    /// Default unauthorized message.
    /// </remarks>
    public const string UnauthorizedMsg = "Unauthorized access.";

    /// <summary>
    /// 参数错误默认消息。
    /// </summary>
    /// <remarks>
    /// Default parameter error message.
    /// </remarks>
    public const string ParamErrorMsg = "Parameter error.";

    /// <summary>
    /// 资源未找到默认消息。
    /// </summary>
    /// <remarks>
    /// Default resource not found message.
    /// </remarks>
    public const string NotFoundMsg = "Resource not found.";

    /// <summary>
    /// 服务器内部错误默认消息。
    /// </summary>
    /// <remarks>
    /// Default internal server error message.
    /// </remarks>
    public const string ServerErrorMsg = "Internal server error.";

    /// <summary>
    /// 非法请求默认消息。
    /// </summary>
    /// <remarks>
    /// Default illegal request message.
    /// </remarks>
    public const string IllegalMsg = "Illegal request.";

    #endregion
}
