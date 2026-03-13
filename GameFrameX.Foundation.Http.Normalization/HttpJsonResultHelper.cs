using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Logger;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 提供用于处理HTTP JSON结果的辅助方法和常量。
/// </summary>
public static partial class HttpJsonResultHelper
{
    #region 错误码常量

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

/// <summary>
/// 提供用于处理HTTP JSON结果的扩展方法。
/// </summary>
public static partial class HttpJsonResultHelper
{
    /// <summary>
    /// 将JSON字符串转换为HttpJsonResultData对象
    /// </summary>
    /// <typeparam name="T">泛型参数T，表示要反序列化的目标类型，必须是引用类型且包含无参构造函数</typeparam>
    /// <param name="jsonResult">需要转换的JSON字符串</param>
    /// <returns>返回转换后的HttpJsonResultData对象，包含反序列化结果和状态信息</returns>
    /// <remarks>
    /// 该方法会:
    /// 1. 尝试将JSON字符串反序列化为HttpJsonResult对象
    /// 2. 根据响应码(Code)判断请求是否成功
    /// 3. 如果成功(Code=0)，则将Data字段反序列化为泛型类型T
    /// 4. 如果失败，则保留错误信息，Data字段为默认值
    /// </remarks>
    public static HttpJsonResultData<T> ToHttpJsonResultData<T>(this string jsonResult) where T : class, new()
    {
        HttpJsonResultData<T> resultData = new HttpJsonResultData<T>
        {
            IsSuccess = false,
        };
        try
        {
            // 反序列化JSON字符串为HttpJsonResult对象
            var httpJsonResult = JsonHelper.Deserialize<HttpJsonResult>(jsonResult);
            // 检查响应码是否表示成功
            if (httpJsonResult.Code != SuccessCode)
            {
                resultData.Code = httpJsonResult.Code;
                resultData.Message = httpJsonResult.Message;
                return resultData; // 返回默认的失败结果
            }

            resultData.IsSuccess = true; // 设置成功标志
            // 反序列化数据部分，如果数据为空则返回类型T的默认实例
            resultData.Data = string.IsNullOrEmpty(httpJsonResult.Data) ? new T() : JsonHelper.Deserialize<T>(httpJsonResult.Data);
        }
        catch (Exception e)
        {
            // 捕获并输出异常信息
            LogHelper.Fatal(e, "JSON Deserialize Error {error}");
        }

        return resultData; // 返回结果数据
    }
}
