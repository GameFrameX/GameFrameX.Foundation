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

using System.Text.Json.Serialization;
using GameFrameX.Foundation.Json;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 消息返回统一结构。
/// 该类用于封装HTTP请求的返回结果，提供统一的结构以便于处理和解析响应数据。
/// </summary>
/// <remarks>
/// Unified message response structure.
/// This class is used to encapsulate HTTP request response results, providing a unified structure for easy processing and parsing of response data.
/// </remarks>
/// <typeparam name="T">消息类型，表示返回的数据对象的类型 / Message type representing the type of the returned data object</typeparam>
public sealed class HttpJsonResultData<T> : IHttpJsonResult
{
    /// <summary>
    /// 获取是否成功。
    /// 根据响应码自动判断，Code为0时返回true，其他值返回false。
    /// 该属性为快捷判断属性，不参与序列化。
    /// </summary>
    /// <remarks>
    /// Gets whether the request is successful.
    /// Automatically determined by the response code. Returns true when Code is 0, false for other values.
    /// This property is for quick judgment and does not participate in serialization.
    /// </remarks>
    /// <value>如果成功则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if successful; otherwise <c>false</c></value>
    [JsonIgnore]
    public bool IsSuccess => Code == HttpJsonResultConstants.SuccessCode;

    /// <summary>
    /// 获取或设置响应码。
    /// 表示请求的处理结果，为0表示成功，其他值表示不同的错误类型。
    /// </summary>
    /// <remarks>
    /// Gets or sets the response code.
    /// Indicates the request processing result. 0 indicates success, other values indicate different error types.
    /// </remarks>
    /// <value>响应码 / Response code</value>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// 获取或设置错误消息。
    /// 表示请求的处理结果，为null表示成功，其他值表示不同的错误类型结果。
    /// </summary>
    /// <remarks>
    /// Gets or sets the error message.
    /// Indicates the request processing result. null indicates success, other values indicate different error type results.
    /// </remarks>
    /// <value>错误消息 / Error message</value>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// 获取或设置数据对象。
    /// 包含请求成功时返回的数据，类型为T。
    /// 如果请求失败，可能为默认值或null。
    /// </summary>
    /// <remarks>
    /// Gets or sets the data object.
    /// Contains the data returned when the request succeeds, of type T.
    /// May be the default value or null if the request fails.
    /// </remarks>
    /// <value>数据对象 / Data object</value>
    [JsonPropertyName("data")]
    public T Data { get; set; }

    /// <summary>
    /// 显式实现接口的Data属性。
    /// 将泛型数据序列化为JSON字符串，用于统一接口访问。
    /// </summary>
    /// <remarks>
    /// Explicit implementation of the interface's Data property.
    /// Serializes generic data to a JSON string for unified interface access.
    /// </remarks>
    /// <value>序列化后的JSON字符串 / Serialized JSON string</value>
    [JsonIgnore]
    string IHttpJsonResult.Data => Data == null ? null : JsonHelper.Serialize(Data);

    /// <summary>
    /// 将当前对象序列化为JSON字符串。
    /// 使用JsonHelper进行序列化，保持中文字符和Emoji不被转义。
    /// </summary>
    /// <remarks>
    /// Serializes the current object to a JSON string.
    /// Uses JsonHelper for serialization, keeping Chinese characters and Emoji unescaped.
    /// </remarks>
    /// <returns>JSON格式的字符串表示 / JSON format string representation</returns>
    public override string ToString()
    {
        return JsonHelper.Serialize(this);
    }

    #region Success

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象。
    /// 返回一个Code为0，Message为空，IsSuccess为true的基本成功响应。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating success.
    /// Returns a basic success response with Code 0, empty Message, and IsSuccess as true.
    /// </remarks>
    /// <returns>成功的HttpJsonResultData实例 / A successful HttpJsonResultData instance</returns>
    public static HttpJsonResultData<T> Success()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.SuccessCode,
            Message = string.Empty,
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating success.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string SuccessString()
    {
        return Success().ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象，并包含数据。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating success with data.
    /// </remarks>
    /// <param name="data">成功时返回的数据对象 / Data object to return on success</param>
    /// <returns>成功的HttpJsonResultData实例 / A successful HttpJsonResultData instance</returns>
    public static HttpJsonResultData<T> Success(T data)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.SuccessCode,
            Message = string.Empty,
            Data = data,
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串，并包含数据。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating success with data.
    /// </remarks>
    /// <param name="data">成功时返回的数据对象 / Data object to return on success</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string SuccessString(T data)
    {
        return Success(data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象，包含自定义消息和数据。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating success with custom message and data.
    /// </remarks>
    /// <param name="message">返回消息 / Return message</param>
    /// <param name="data">返回数据 / Return data</param>
    /// <returns>成功的HttpJsonResultData实例 / A successful HttpJsonResultData instance</returns>
    public static HttpJsonResultData<T> Success(string message, T data)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.SuccessCode,
            Message = message,
            Data = data,
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串，包含自定义消息和数据。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating success with custom message and data.
    /// </remarks>
    /// <param name="message">返回消息 / Return message</param>
    /// <param name="data">返回数据 / Return data</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string SuccessString(string message, T data)
    {
        return Success(message, data).ToString();
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象，包含自定义状态码、消息和数据。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating success with custom status code, message, and data.
    /// </remarks>
    /// <param name="code">HTTP状态码 / HTTP status code</param>
    /// <param name="message">返回消息 / Return message</param>
    /// <param name="data">返回数据 / Return data</param>
    /// <returns>成功的HttpJsonResultData实例 / A successful HttpJsonResultData instance</returns>
    public static HttpJsonResultData<T> Success(int code, string message, T data)
    {
        return new HttpJsonResultData<T>
        {
            Code = code,
            Message = message,
            Data = data,
        };
    }

    /// <summary>
    /// 创建一个表示成功的HttpJsonResultData对象的JSON字符串，包含自定义状态码、消息和数据。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating success with custom status code, message, and data.
    /// </remarks>
    /// <param name="code">HTTP状态码 / HTTP status code</param>
    /// <param name="message">返回消息 / Return message</param>
    /// <param name="data">返回数据 / Return data</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating failure with an error message.
    /// Uses default error code -1 to indicate general failure.
    /// </remarks>
    /// <param name="message">失败的详细消息 / Detailed failure message</param>
    /// <returns>失败的HttpJsonResultData实例 / A failed HttpJsonResultData instance</returns>
    public static HttpJsonResultData<T> Fail(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.FailCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating failure.
    /// </remarks>
    /// <param name="message">失败的详细消息 / Detailed failure message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string FailString(string message)
    {
        return Fail(message).ToString();
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象，并包含错误码和错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating failure with error code and error message.
    /// </remarks>
    /// <param name="code">错误码 / Error code</param>
    /// <param name="message">失败的详细消息 / Detailed failure message</param>
    /// <returns>失败的HttpJsonResultData实例 / A failed HttpJsonResultData instance</returns>
    public static HttpJsonResultData<T> Fail(int code, string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = code,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示失败的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating failure.
    /// </remarks>
    /// <param name="code">错误码 / Error code</param>
    /// <param name="message">失败的详细消息 / Detailed failure message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string FailString(int code, string message)
    {
        return Fail(code, message).ToString();
    }

    #endregion

    #region Error

    /// <summary>
    /// 创建一个表示特定错误的HttpJsonResultData对象，并包含错误码和错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating a specific error with error code and error message.
    /// </remarks>
    /// <param name="code">错误码 / Error code</param>
    /// <param name="message">错误消息 / Error message</param>
    /// <returns>包含错误信息的HttpJsonResultData实例 / An HttpJsonResultData instance containing error information</returns>
    public static HttpJsonResultData<T> Error(int code, string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = code,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示特定错误的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating a specific error.
    /// </remarks>
    /// <param name="code">错误码 / Error code</param>
    /// <param name="message">错误消息 / Error message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating validation failure.
    /// Uses HTTP 400 status code to indicate request parameter validation failure.
    /// </remarks>
    /// <returns>验证失败的HttpJsonResultData实例 / An HttpJsonResultData instance indicating validation failure</returns>
    public static HttpJsonResultData<T> ValidationError()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ValidationErrorCode,
            Message = HttpJsonResultConstants.ValidationErrorMsg,
        };
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating validation failure.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string ValidationErrorString()
    {
        return ValidationError().ToString();
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating validation failure with a custom error message.
    /// </remarks>
    /// <param name="message">验证失败的详细消息 / Detailed validation failure message</param>
    /// <returns>验证失败的HttpJsonResultData实例 / An HttpJsonResultData instance indicating validation failure</returns>
    public static HttpJsonResultData<T> ValidationError(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ValidationErrorCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示验证失败的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating validation failure with a custom error message.
    /// </remarks>
    /// <param name="message">验证失败的详细消息 / Detailed validation failure message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating unauthorized access.
    /// Uses HTTP 401 status code to indicate unauthorized access.
    /// </remarks>
    /// <returns>未授权的HttpJsonResultData实例 / An HttpJsonResultData instance indicating unauthorized access</returns>
    public static HttpJsonResultData<T> Unauthorized()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = HttpJsonResultConstants.UnauthorizedMsg,
        };
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating unauthorized access.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string UnauthorizedString()
    {
        return Unauthorized().ToString();
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating unauthorized access with a custom error message.
    /// </remarks>
    /// <param name="message">未授权的详细消息 / Detailed unauthorized message</param>
    /// <returns>未授权的HttpJsonResultData实例 / An HttpJsonResultData instance indicating unauthorized access</returns>
    public static HttpJsonResultData<T> Unauthorized(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示未授权的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating unauthorized access with a custom error message.
    /// </remarks>
    /// <param name="message">未授权的详细消息 / Detailed unauthorized message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating resource not found.
    /// Uses HTTP 404 status code to indicate that the requested resource does not exist.
    /// </remarks>
    /// <returns>未找到的HttpJsonResultData实例 / An HttpJsonResultData instance indicating resource not found</returns>
    public static HttpJsonResultData<T> NotFound()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.NotFoundCode,
            Message = HttpJsonResultConstants.NotFoundMsg,
        };
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating resource not found.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string NotFoundString()
    {
        return NotFound().ToString();
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating resource not found with a custom error message.
    /// </remarks>
    /// <param name="message">资源未找到的详细消息 / Detailed resource not found message</param>
    /// <returns>未找到的HttpJsonResultData实例 / An HttpJsonResultData instance indicating resource not found</returns>
    public static HttpJsonResultData<T> NotFound(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.NotFoundCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示资源未找到的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating resource not found with a custom error message.
    /// </remarks>
    /// <param name="message">资源未找到的详细消息 / Detailed resource not found message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating internal server error.
    /// Uses HTTP 500 status code to indicate an internal server error.
    /// </remarks>
    /// <returns>服务器错误的HttpJsonResultData实例 / An HttpJsonResultData instance indicating server error</returns>
    public static HttpJsonResultData<T> ServerError()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ServerErrorCode,
            Message = HttpJsonResultConstants.ServerErrorMsg,
        };
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating internal server error.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string ServerErrorString()
    {
        return ServerError().ToString();
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating internal server error with a custom error message.
    /// </remarks>
    /// <param name="message">服务器错误的详细消息 / Detailed server error message</param>
    /// <returns>服务器错误的HttpJsonResultData实例 / An HttpJsonResultData instance indicating server error</returns>
    public static HttpJsonResultData<T> ServerError(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ServerErrorCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示服务器内部错误的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating internal server error with a custom error message.
    /// </remarks>
    /// <param name="message">服务器错误的详细消息 / Detailed server error message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating parameter error.
    /// Uses HTTP 403 status code to indicate request parameter error.
    /// </remarks>
    /// <returns>参数错误的HttpJsonResultData实例 / An HttpJsonResultData instance indicating parameter error</returns>
    public static HttpJsonResultData<T> ParamError()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ParamErrorCode,
            Message = HttpJsonResultConstants.ParamErrorMsg,
        };
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating parameter error.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string ParamErrorString()
    {
        return ParamError().ToString();
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating parameter error with a custom error message.
    /// </remarks>
    /// <param name="message">参数错误的详细消息 / Detailed parameter error message</param>
    /// <returns>参数错误的HttpJsonResultData实例 / An HttpJsonResultData instance indicating parameter error</returns>
    public static HttpJsonResultData<T> ParamError(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.ParamErrorCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示参数错误的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating parameter error with a custom error message.
    /// </remarks>
    /// <param name="message">参数错误的详细消息 / Detailed parameter error message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
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
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating illegal request.
    /// Uses HTTP 401 status code to indicate illegal request access.
    /// </remarks>
    /// <returns>非法请求的HttpJsonResultData实例 / An HttpJsonResultData instance indicating illegal request</returns>
    public static HttpJsonResultData<T> Illegal()
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = HttpJsonResultConstants.IllegalMsg,
        };
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象的JSON字符串。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating illegal request.
    /// </remarks>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string IllegalString()
    {
        return Illegal().ToString();
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates an HttpJsonResultData object indicating illegal request with a custom error message.
    /// </remarks>
    /// <param name="message">非法请求的详细消息 / Detailed illegal request message</param>
    /// <returns>非法请求的HttpJsonResultData实例 / An HttpJsonResultData instance indicating illegal request</returns>
    public static HttpJsonResultData<T> Illegal(string message)
    {
        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.UnauthorizedCode,
            Message = message,
        };
    }

    /// <summary>
    /// 创建一个表示非法请求的HttpJsonResultData对象的JSON字符串，并包含自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Creates a JSON string of an HttpJsonResultData object indicating illegal request with a custom error message.
    /// </remarks>
    /// <param name="message">非法请求的详细消息 / Detailed illegal request message</param>
    /// <returns>序列化后的JSON字符串 / Serialized JSON string</returns>
    public static string IllegalString(string message)
    {
        return Illegal(message).ToString();
    }

    #endregion
}
