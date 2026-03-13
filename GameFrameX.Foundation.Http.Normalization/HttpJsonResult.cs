using System.Text.Json.Serialization;
using GameFrameX.Foundation.Json;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP请求的消息响应结构
/// 该类用于封装HTTP请求的响应结果，包括响应码、消息和数据。
/// 提供了一系列静态方法来创建不同状态的响应对象，如成功、失败、错误等。
/// </summary>
public sealed class HttpJsonResult
{
    /// <summary>
    /// 响应码，0表示成功，其他值表示不同的错误类型。
    /// 常见响应码:
    /// 0: 成功
    /// -1: 一般性失败
    /// 400: 验证失败
    /// 401: 未授权
    /// 403: 参数错误
    /// 404: 资源未找到
    /// 500: 服务器内部错误
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 响应消息，提供关于请求结果的详细信息。
    /// 成功时通常为空字符串，失败时包含具体的错误信息。
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// 响应数据，包含请求成功时返回的具体数据内容。
    /// 数据以JSON字符串的形式存储，可以包含任意类型的序列化数据。
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; }

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
    /// 创建一个表示成功的HttpJsonResult对象。
    /// 返回一个Code为0，Message为空的基本成功响应。
    /// </summary>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.SuccessCode,
            Message = string.Empty,
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString()
    {
        return Success().ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象，并包含数据。
    /// 如果数据为null，则返回基本成功响应。
    /// </summary>
    /// <param name="data">成功时返回的数据对象，将被序列化为JSON字符串。</param>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success(object data)
    {
        if (data == null)
        {
            return Success();
        }

        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.SuccessCode,
            Message = string.Empty,
            Data = JsonHelper.Serialize(data),
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象的JSON字符串，并包含数据。
    /// </summary>
    /// <param name="data">成功时返回的数据对象。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(object data)
    {
        return Success(data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象，并包含已序列化的数据字符串。
    /// </summary>
    /// <param name="data">成功时返回的已序列化JSON数据字符串。</param>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success(string data)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.SuccessCode,
            Message = string.Empty,
            Data = data
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象的JSON字符串，并包含已序列化的数据字符串。
    /// </summary>
    /// <param name="data">成功时返回的已序列化JSON数据字符串。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(string data)
    {
        return Success(data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象，包含自定义消息和数据。
    /// 使用默认成功状态码0。
    /// </summary>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success(string message, string data)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.SuccessCode,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象的JSON字符串，包含自定义消息和数据。
    /// </summary>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(string message, string data)
    {
        return Success(message, data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象，并包含数据。
    /// 此方法允许用户自定义返回的状态码和消息，同时提供数据内容。
    /// </summary>
    /// <param name="code">HTTP状态码，表示请求的处理结果。</param>
    /// <param name="message">返回的消息，提供关于请求处理的详细信息。</param>
    /// <param name="data">成功时返回的数据，通常为JSON格式的字符串。</param>
    /// <returns>成功的HttpJsonResult实例，包含指定的状态码、消息和数据。</returns>
    public static HttpJsonResult Success(int code, string message, string data)
    {
        return new HttpJsonResult
        {
            Code = code,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象的JSON字符串，包含自定义状态码、消息和数据。
    /// </summary>
    /// <param name="code">HTTP状态码。</param>
    /// <param name="message">返回消息。</param>
    /// <param name="data">返回数据。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string SuccessString(int code, string message, string data)
    {
        return Success(code, message, data).ToString();
    }

    #endregion

    #region Fail

    /// <summary>
    /// 创建一个表示失败的HttpJsonResult对象，并包含错误消息。
    /// 使用默认错误码-1表示一般性失败。
    /// </summary>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>失败的HttpJsonResult实例。</returns>
    public static HttpJsonResult Fail(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.FailCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string FailString(string message)
    {
        return Fail(message).ToString();
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResult对象，并包含错误码和错误消息。
    /// </summary>
    /// <param name="code">错误码。</param>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>失败的HttpJsonResult实例。</returns>
    public static HttpJsonResult Fail(int code, string message)
    {
        return new HttpJsonResult
        {
            Code = code,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResult对象的JSON字符串。
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
    /// 创建一个表示特定错误的HttpJsonResult对象，并包含错误码和错误消息。
    /// 允许自定义错误码和消息，用于表示特定的错误情况。
    /// </summary>
    /// <param name="code">错误码。</param>
    /// <param name="message">错误消息。</param>
    /// <returns>包含错误信息的HttpJsonResult实例。</returns>
    public static HttpJsonResult Error(int code, string message)
    {
        return new HttpJsonResult
        {
            Code = code,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示特定错误的HttpJsonResult对象的JSON字符串。
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
    /// 创建一个表示验证失败的HttpJsonResult对象。
    /// 使用HTTP 400状态码表示请求参数验证失败。
    /// </summary>
    /// <returns>验证失败的HttpJsonResult实例。</returns>
    public static HttpJsonResult ValidationError()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.ValidationErrorCode,
            Message = HttpJsonResultHelper.ValidationErrorMsg,
        };
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ValidationErrorString()
    {
        return ValidationError().ToString();
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResult对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">验证失败的详细消息。</param>
    /// <returns>验证失败的HttpJsonResult实例。</returns>
    public static HttpJsonResult ValidationError(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.ValidationErrorCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResult对象的JSON字符串，并包含自定义错误消息。
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
    /// 创建一个表示未授权的HttpJsonResult对象。
    /// 使用HTTP 401状态码表示未经授权的访问。
    /// </summary>
    /// <returns>未授权的HttpJsonResult实例。</returns>
    public static HttpJsonResult Unauthorized()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.UnauthorizedCode,
            Message = HttpJsonResultHelper.UnauthorizedMsg
        };
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string UnauthorizedString()
    {
        return Unauthorized().ToString();
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResult对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">未授权的详细消息。</param>
    /// <returns>未授权的HttpJsonResult实例。</returns>
    public static HttpJsonResult Unauthorized(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.UnauthorizedCode,
            Message = message
        };
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResult对象的JSON字符串，并包含自定义错误消息。
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
    /// 创建一个表示资源未找到的HttpJsonResult对象。
    /// 使用HTTP 404状态码表示请求的资源不存在。
    /// </summary>
    /// <returns>未找到的HttpJsonResult实例。</returns>
    public static HttpJsonResult NotFound()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.NotFoundCode,
            Message = HttpJsonResultHelper.NotFoundMsg
        };
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string NotFoundString()
    {
        return NotFound().ToString();
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResult对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">资源未找到的详细消息。</param>
    /// <returns>未找到的HttpJsonResult实例。</returns>
    public static HttpJsonResult NotFound(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.NotFoundCode,
            Message = message
        };
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResult对象的JSON字符串，并包含自定义错误消息。
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
    /// 创建一个表示服务器内部错误的HttpJsonResult对象。
    /// 使用HTTP 500状态码表示服务器内部错误。
    /// </summary>
    /// <returns>服务器错误的HttpJsonResult实例。</returns>
    public static HttpJsonResult ServerError()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.ServerErrorCode,
            Message = HttpJsonResultHelper.ServerErrorMsg
        };
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ServerErrorString()
    {
        return ServerError().ToString();
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResult对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">服务器错误的详细消息。</param>
    /// <returns>服务器错误的HttpJsonResult实例。</returns>
    public static HttpJsonResult ServerError(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.ServerErrorCode,
            Message = message
        };
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResult对象的JSON字符串，并包含自定义错误消息。
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
    /// 创建一个表示参数错误的HttpJsonResult对象。
    /// 使用HTTP 403状态码表示请求参数错误。
    /// </summary>
    /// <returns>参数错误的HttpJsonResult实例。</returns>
    public static HttpJsonResult ParamError()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.ParamErrorCode,
            Message = HttpJsonResultHelper.ParamErrorMsg
        };
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string ParamErrorString()
    {
        return ParamError().ToString();
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResult对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">参数错误的详细消息。</param>
    /// <returns>参数错误的HttpJsonResult实例。</returns>
    public static HttpJsonResult ParamError(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.ParamErrorCode,
            Message = message
        };
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResult对象的JSON字符串，并包含自定义错误消息。
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
    /// 创建一个表示非法请求的HttpJsonResult对象。
    /// 使用HTTP 401状态码表示非法的请求访问。
    /// </summary>
    /// <returns>非法请求的HttpJsonResult实例。</returns>
    public static HttpJsonResult Illegal()
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.UnauthorizedCode,
            Message = HttpJsonResultHelper.IllegalMsg
        };
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResult对象的JSON字符串。
    /// </summary>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string IllegalString()
    {
        return Illegal().ToString();
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResult对象，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">非法请求的详细消息。</param>
    /// <returns>非法请求的HttpJsonResult实例。</returns>
    public static HttpJsonResult Illegal(string message)
    {
        return new HttpJsonResult
        {
            Code = HttpJsonResultHelper.UnauthorizedCode,
            Message = message
        };
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResult对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <param name="message">非法请求的详细消息。</param>
    /// <returns>序列化后的JSON字符串。</returns>
    public static string IllegalString(string message)
    {
        return Illegal(message).ToString();
    }

    #endregion
}
