namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM4算法上下文类
/// </summary>
internal sealed class Sm4Context
{
    /// <summary>
    /// 加密/解密模式（W-15 修复：改为属性）
    /// </summary>
    public int Mode { get; set; }

    /// <summary>
    /// 轮密钥（W-15 修复：改为属性）
    /// </summary>
    public long[] Sk { get; set; }

    /// <summary>
    /// 是否使用填充（W-15 修复：改为属性）
    /// </summary>
    public bool IsPadding { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    public Sm4Context()
    {
        Mode = 1;
        IsPadding = true;
        Sk = new long[32];
    }
}