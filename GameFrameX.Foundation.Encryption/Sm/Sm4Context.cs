namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM4算法上下文类
/// </summary>
internal sealed class Sm4Context
{
    /// <summary>
    /// 加密/解密模式
    /// </summary>
    public int Mode;

    /// <summary>
    /// 轮密钥
    /// </summary>
    public long[] Sk;

    /// <summary>
    /// 是否使用填充
    /// </summary>
    public bool IsPadding;

    /// <summary>
    /// 构造函数
    /// </summary>
    public Sm4Context()
    {
        this.Mode = 1;
        this.IsPadding = true;
        this.Sk = new long[32];
    }
}