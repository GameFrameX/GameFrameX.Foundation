namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// SM4算法上下文类，保存SM4加密/解密操作所需的状态信息。
/// </summary>
/// <remarks>
/// SM4 algorithm context class, storing state information required for SM4 encryption/decryption operations.
/// </remarks>
internal sealed class Sm4Context
{
    /// <summary>
    /// 加密/解密模式。
    /// </summary>
    /// <remarks>
    /// Encryption/decryption mode.
    /// </remarks>
    public int Mode { get; set; }

    /// <summary>
    /// 轮密钥数组。
    /// </summary>
    /// <remarks>
    /// Round key array.
    /// </remarks>
    public long[] Sk { get; set; }

    /// <summary>
    /// 是否使用填充。
    /// </summary>
    /// <remarks>
    /// Whether to use padding.
    /// </remarks>
    public bool IsPadding { get; set; }

    /// <summary>
    /// 初始化SM4上下文的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the SM4 context.
    /// </remarks>
    public Sm4Context()
    {
        Mode = 1;
        IsPadding = true;
        Sk = new long[32];
    }
}