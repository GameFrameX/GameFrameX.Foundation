using System.Text.Json.Serialization;
using GameFrameX.Foundation.Json;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 消息返回统一结构
/// 该类用于封装HTTP请求的返回结果，提供统一的结构以便于处理和解析响应数据。
/// </summary>
/// <typeparam name="T">消息类型，表示返回的数据对象的类型。</typeparam>
public sealed class HttpJsonResultData<T>
{
    /// <summary>
    /// 是否成功
    /// 表示请求是否成功执行，成功为true，失败为false。
    /// 该属性为快捷判断属性，只能在程序集内部设置，不参与序列化。
    /// </summary>
    [JsonIgnore]
    public bool IsSuccess { get; internal set; }

    /// <summary>
    /// 响应码
    /// 表示请求的处理结果，为0表示成功，其他值表示不同的错误类型。
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 错误消息,
    /// 表示请求的处理结果，为null表示成功，其他值表示不同的错误类型结果。
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// 数据对象
    /// 包含请求成功时返回的数据，类型为T。
    /// 如果请求失败，可能为默认值或null。
    /// </summary>
    [JsonPropertyName("data")]
    public T Data { get; set; }

    /// <summary>
    /// 将当前对象序列化为JSON字符串。
    /// 使用JsonHelper进行序列化，保持中文字符和Emoji不被转义。
    /// </summary>
    /// <returns>JSON格式的字符串表示。</returns>
    public override string ToString()
    {
        return JsonHelper.Serialize(this);
    }

    #region Success

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象。
    /// 返回一个Code为0，Message为空，IsSuccess为true的基本成功响应。
    /// </summary>
    /// <returns>成功的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Success()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.SuccessCode,
            Message = string.Empty,
            IsSuccess = true
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString()
    {
        return Success().ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象，并包含数据。
    /// </summary>
    /// <param name="data">成功时返回的数据对象。</param>
    /// <returns>成功的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Success(T data)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.SuccessCode,
            Message = string.Empty,
            Data = data,
            IsSuccess = true
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串，并包含数据。
    /// </summary>
    /// <param name="data">成功时返回的数据对象。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(T data)
    {
        return Success(data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象，包含自定义消息和数据。
    /// </summary>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>成功的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Success(string message, T data)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.SuccessCode,
            Message = message,
            Data = data,
            IsSuccess = true
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串，包含自定义消息和数据。
    /// </summary>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(string message, T data)
    {
        return Success(message, data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象，包含自定义状态码、消息和数据。
    /// </summary>
    /// <param name="code">HTTP状态码。</param>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>成功的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Success(int code, string message, T data)
    {
        return new HttpJsonResultData<T>
        {
            Code = code,
            Message = message,
            Data = data,
            IsSuccess = true
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串，包含自定义状态码、消息和数据。
    /// </summary>
    /// <param name="code">HTTP状态码。</param>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(int code, string message, T data)
    {
        return Success(code, message, data).ToString();
    }

    #endregion

    #region Fail

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象，并包含错误消息。
    /// 使用默认错误码-1表示一般性失败。
    /// </summary>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>失败的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Fail(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.FailCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string FailString(string message)
    {
        return Fail(message).ToString();
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象，并包含错误码和错误消息。
    /// </summary>
    /// <param name="code">错误码。</param>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>失败的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Fail(int code, string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = code,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <param name="code">错误码。</param>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string FailString(int code, string message)
    {
        return Fail(code, message).ToString();
    }

    #endregion

    #region Error

    /// <summary>
    /// 创建一个表示特定错误的HttpJsonResultData对象，并包含错误码和错误消息。
    /// </summary>
    /// <param name="code">错误码。</param>
    /// <param name="message">错误消息。</param>
    /// <returns>包含错误信息的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Error(int code, string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = code,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示特定错误的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <param name="code">错误码。</param>
    /// <param name="message">错误消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ErrorString(int code, string message)
    {
        return Error(code, message).ToString();
    }

    #endregion

    #region ValidationError

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象。
    /// 使用HTTP 400状态码表示请求参数验证失败。
    /// </summary>
    /// <returns>验证失败的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> ValidationError()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ValidationErrorCode,
            Message = HttpJsonResultConstants.ValidationErrorMsg,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ValidationErrorString()
    {
        return ValidationError().ToString();
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">验证失败的详细消息。</param>
    /// <returns>验证失败的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> ValidationError(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ValidationErrorCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">验证失败的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ValidationErrorString(string message)
    {
        return ValidationError(message).ToString();
    }

    #endregion

    #region Unauthorized

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象。
    /// 使用HTTP 401状态码表示未经授权的访问。
    /// </summary>
    /// <returns>未授权的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Unauthorized()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = HttpJsonResultConstants.UnauthorizedMsg,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string UnauthorizedString()
    {
        return Unauthorized().ToString();
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">未授权的详细消息。</param>
    /// <returns>未授权的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Unauthorized(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">未授权的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string UnauthorizedString(string message)
    {
        return Unauthorized(message).ToString();
    }

    #endregion

    #region NotFound

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象。
    /// 使用HTTP 404状态码表示请求的资源不存在。
    /// </summary>
    /// <returns>未找到的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> NotFound()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.NotFoundCode,
            Message = HttpJsonResultConstants.NotFoundMsg,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string NotFoundString()
    {
        return NotFound().ToString();
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">资源未找到的详细消息。</param>
    /// <returns>未找到的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> NotFound(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.NotFoundCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">资源未找到的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string NotFoundString(string message)
    {
        return NotFound(message).ToString();
    }

    #endregion

    #region ServerError

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象。
    /// 使用HTTP 500状态码表示服务器内部错误。
    /// </summary>
    /// <returns>服务器错误的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> ServerError()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ServerErrorCode,
            Message = HttpJsonResultConstants.ServerErrorMsg,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ServerErrorString()
    {
        return ServerError().ToString();
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">服务器错误的详细消息。</param>
    /// <returns>服务器错误的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> ServerError(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ServerErrorCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">服务器错误的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ServerErrorString(string message)
    {
        return ServerError(message).ToString();
    }

    #endregion

    #region ParamError

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象。
    /// 使用HTTP 403状态码表示请求参数错误。
    /// </summary>
    /// <returns>参数错误的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> ParamError()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ParamErrorCode,
            Message = HttpJsonResultConstants.ParamErrorMsg,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ParamErrorString()
    {
        return ParamError().ToString();
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">参数错误的详细消息。</param>
    /// <returns>参数错误的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> ParamError(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ParamErrorCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">参数错误的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ParamErrorString(string message)
    {
        return ParamError(message).ToString();
    }

    #endregion

    #region Illegal

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象。
    /// 使用HTTP 401状态码表示非法的请求访问。
    /// </summary>
    /// <returns>非法请求的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Illegal()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = HttpJsonResultConstants.IllegalMsg,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string IllegalString()
    {
        return Illegal().ToString();
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">非法请求的详细消息。</param>
    /// <returns>非法请求的HttpJsonResultData实例。</returns>
    public static HttpJsonResultData<T> Illegal(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = message,
            IsSuccess = false
        };
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">非法请求的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string IllegalString(string message)
    {
        return Illegal(message).ToString();
    }

    #endregion
}
