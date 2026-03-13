namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP JSON 结果常量定义
/// 提供统一的错误码和消息常量，用于 HttpJsonResult 和 HttpJsonResultData&lt;T&gt; 的工厂方法。
/// 该类为内部类，仅供程序集内部使用。
/// </summary>
internal static class HttpJsonResultConstants
{
    #region 状态码常量

    /// <summary>
    /// 成功状态码
    /// </summary>
    public const int SuccessCode = 0;

    /// <summary>
    /// 一般性失败状态码
    /// </summary>
    public const int FailCode = -1;

    /// <summary>
    /// 验证失败状态码 (HTTP 400)
    /// </summary>
    public const int ValidationErrorCode = 400;

    /// <summary>
    /// 未授权状态码 (HTTP 401)
    /// </summary>
    public const int UnauthorizedCode = 401;

    /// <summary>
    /// 参数错误状态码 (HTTP 403)
    /// </summary>
    public const int ParamErrorCode = 403;

    /// <summary>
    /// 资源未找到状态码 (HTTP 404)
    /// </summary>
    public const int NotFoundCode = 404;

    /// <summary>
    /// 服务器内部错误状态码 (HTTP 500)
    /// </summary>
    public const int ServerErrorCode = 500;

    #endregion

    #region 消息常量

    /// <summary>
    /// 验证失败默认消息
    /// </summary>
    public const string ValidationErrorMsg = "Validation failed.";

    /// <summary>
    /// 未授权默认消息
    /// </summary>
    public const string UnauthorizedMsg = "Unauthorized access.";

    /// <summary>
    /// 参数错误默认消息
    /// </summary>
    public const string ParamErrorMsg = "Parameter error.";

    /// <summary>
    /// 资源未找到默认消息
    /// </summary>
    public const string NotFoundMsg = "Resource not found.";

    /// <summary>
    /// 服务器内部错误默认消息
    /// </summary>
    public const string ServerErrorMsg = "Internal server error.";

    /// <summary>
    /// 非法请求默认消息
    /// </summary>
    public const string IllegalMsg = "Illegal request.";

    #endregion
}
