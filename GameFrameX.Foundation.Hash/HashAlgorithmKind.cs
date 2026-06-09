namespace GameFrameX.Foundation.Hash;

/// <summary>
/// 支持统一计算入口的哈希算法。
/// </summary>
/// <remarks>
/// Hash algorithms supported by the unified computation entry point.
/// MD5 and SHA-1 are provided only for compatibility and non-security checksums.
/// </remarks>
public enum HashAlgorithmKind
{
    /// <summary>
    /// MD5，仅适用于兼容和非安全校验场景 / MD5, for compatibility and non-security checksum scenarios only.
    /// </summary>
    Md5,

    /// <summary>
    /// SHA-1，仅适用于兼容和非安全校验场景 / SHA-1, for compatibility and non-security checksum scenarios only.
    /// </summary>
    Sha1,

    /// <summary>
    /// SHA-256 / SHA-256.
    /// </summary>
    Sha256,

    /// <summary>
    /// SHA-512 / SHA-512.
    /// </summary>
    Sha512,
}
