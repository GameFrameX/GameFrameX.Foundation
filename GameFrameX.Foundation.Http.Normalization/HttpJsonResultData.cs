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
    /// </summary>
    public bool IsSuccess { get; set; } = false;

    /// <summary>
    /// 响应码
    /// 表示请求的处理结果，为0表示成功，其他值表示不同的错误类型。
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 数据对象
    /// 包含请求成功时返回的数据，类型为T。
    /// 如果请求失败，可能为默认值或null。
    /// </summary>
    public T Data { get; set; }
}