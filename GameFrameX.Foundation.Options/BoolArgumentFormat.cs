namespace GameFrameX.Foundation.Options;

/// <summary>
/// Bool类型参数的格式选项
/// </summary>
public enum BoolArgumentFormat
{
    /// <summary>
    /// 标志格式：存在即为true，不存在即为false（如：--verbose）
    /// </summary>
    Flag,

    /// <summary>
    /// 键值对格式：明确指定true/false值（如：--verbose=true）
    /// </summary>
    KeyValue,

    /// <summary>
    /// 分离格式：键和值分开（如：--verbose true）
    /// </summary>
    Separated
}