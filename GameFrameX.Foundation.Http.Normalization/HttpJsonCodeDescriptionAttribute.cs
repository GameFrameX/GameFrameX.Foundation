namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 自定义特性，用于为HTTP JSON代码提供描述信息。
/// 该特性可以应用于枚举、字段和属性，以便在序列化或文档生成时提供更详细的上下文信息。
/// </summary>
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
public sealed class HttpJsonCodeDescriptionAttribute : Attribute
{
    /// <summary>
    /// 获取描述信息。
    /// 描述信息用于提供关于HTTP JSON代码的详细说明。
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// 初始化一个新的 HttpJsonCodeDescriptionAttribute 实例。
    /// </summary>
    /// <param name="description">描述信息。</param>
    public HttpJsonCodeDescriptionAttribute(string description)
    {
        Description = description;
    }
}