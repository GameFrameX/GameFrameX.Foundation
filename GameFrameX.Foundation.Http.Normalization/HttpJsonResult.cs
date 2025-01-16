using Newtonsoft.Json;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP请求的消息响应结构
/// 该类用于封装HTTP请求的响应结果，包括响应码、消息和数据。
/// </summary>
public sealed class HttpJsonResult
{
    /// <summary>
    /// 响应码，0表示成功，其他值表示不同的错误类型。
    /// </summary>
    [JsonProperty(PropertyName = "code")]
    public int Code { get; set; }

    /// <summary>
    /// 响应消息，提供关于请求结果的详细信息。
    /// </summary>
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }

    /// <summary>
    /// 响应数据，包含请求成功时返回的具体数据内容。
    /// </summary>
    [JsonProperty(PropertyName = "data")]
    public string Data { get; set; }

    /// <summary>
    /// 将当前对象序列化为JSON字符串。
    /// </summary>
    /// <returns>JSON格式的字符串表示。</returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象。
    /// </summary>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success()
    {
        return new HttpJsonResult
        {
            Code = 0,
            Message = string.Empty,
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResult对象，并包含错误消息。
    /// </summary>
    /// <param name="message">失败的详细消息。</param>
    /// <returns>失败的HttpJsonResult实例。</returns>
    public static HttpJsonResult Fail(string message)
    {
        return new HttpJsonResult
        {
            Code = -1,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示特定错误的HttpJsonResult对象，并包含错误码和错误消息。
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
    /// 创建一个表示验证失败的HttpJsonResult对象。
    /// </summary>
    /// <returns>验证失败的HttpJsonResult实例。</returns>
    public static HttpJsonResult ValidationError()
    {
        return new HttpJsonResult
        {
            Code = 400,
            Message = "Validation failed.",
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象，并包含数据。
    /// </summary>
    /// <param name="data">成功时返回的数据。</param>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success(object data)
    {
        if (data == null)
        {
            return Success();
        }

        return new HttpJsonResult
        {
            Code = 0,
            Message = string.Empty,
            Data = JsonConvert.SerializeObject(data),
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResult对象，并包含数据。
    /// </summary>
    /// <param name="data">成功时返回的数据。</param>
    /// <returns>成功的HttpJsonResult实例。</returns>
    public static HttpJsonResult Success(string data)
    {
        return new HttpJsonResult
        {
            Code = 0,
            Message = string.Empty,
            Data = data
        };
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
    /// 创建一个表示未授权的HttpJsonResult对象。
    /// </summary>
    /// <returns>未授权的HttpJsonResult实例。</returns>
    public static HttpJsonResult Unauthorized()
    {
        return new HttpJsonResult
        {
            Code = 401,
            Message = "Unauthorized access."
        };
    }

    /// <summary>
    /// 创建一个表示未找到的HttpJsonResult对象。
    /// </summary>
    /// <returns>未找到的HttpJsonResult实例。</returns>
    public static HttpJsonResult NotFound()
    {
        return new HttpJsonResult
        {
            Code = 404,
            Message = "Resource not found."
        };
    }
}