namespace GameFrameX.Foundation.Encryption;

/// <summary>
/// XOR加密解密工具类,提供异或运算相关的加密解密功能。
/// 异或运算具有可逆性,使用相同的密钥进行两次异或运算可以还原原始数据。
/// </summary>
public static class XorHelper
{
    /// <summary>
    /// 快速加密的默认长度,用于只加密数据的前220字节以提高性能
    /// </summary>
    internal const int QuickEncryptLength = 220;

    /// <summary>
    /// 将 bytes 使用 code 做异或运算的快速版本。
    /// 只对数据的前QuickEncryptLength字节进行异或运算,适用于需要快速加密的场景。
    /// </summary>
    /// <param name="bytes">原始二进制流。</param>
    /// <param name="code">异或二进制流(密钥)。</param>
    /// <returns>异或后的二进制流。</returns>
    public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
    {
        return GetXorBytes(bytes, 0, QuickEncryptLength, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算的快速版本。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
    /// 只对数据的前QuickEncryptLength字节进行异或运算,适用于需要快速加密且允许修改原数据的场景。
    /// </summary>
    /// <param name="bytes">原始及异或后的二进制流。</param>
    /// <param name="code">异或二进制流(密钥)。</param>
    public static void GetQuickSelfXorBytes(byte[] bytes, byte[] code)
    {
        GetSelfXorBytes(bytes, 0, QuickEncryptLength, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。
    /// 对整个数据进行异或运算加密。
    /// </summary>
    /// <param name="bytes">原始二进制流。</param>
    /// <param name="code">异或二进制流(密钥)。</param>
    /// <returns>异或后的二进制流。如果输入为null则返回null。</returns>
    public static byte[] GetXorBytes(byte[] bytes, byte[] code)
    {
        if (bytes == null)
        {
            return null;
        }

        return GetXorBytes(bytes, 0, bytes.Length, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
    /// 对整个数据进行异或运算加密,直接在原数组上进行修改。
    /// </summary>
    /// <param name="bytes">原始及异或后的二进制流。</param>
    /// <param name="code">异或二进制流(密钥)。</param>
    public static void GetSelfXorBytes(byte[] bytes, byte[] code)
    {
        if (bytes == null)
        {
            return;
        }

        GetSelfXorBytes(bytes, 0, bytes.Length, code);
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。
    /// 可以指定起始位置和长度进行部分加密。
    /// </summary>
    /// <param name="bytes">原始二进制流。</param>
    /// <param name="startIndex">异或计算的开始位置。</param>
    /// <param name="length">异或计算长度。</param>
    /// <param name="code">异或二进制流(密钥)。</param>
    /// <returns>异或后的二进制流。如果输入为null则返回null。</returns>
    public static byte[] GetXorBytes(byte[] bytes, int startIndex, int length, byte[] code)
    {
        if (bytes == null)
        {
            return null;
        }

        var bytesLength = bytes.Length;
        var results = new byte[bytesLength];
        Array.Copy(bytes, 0, results, 0, bytesLength);
        GetSelfXorBytes(results, startIndex, length, code);
        return results;
    }

    /// <summary>
    /// 将 bytes 使用 code 做异或运算。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
    /// 可以指定起始位置和长度进行部分加密,直接在原数组上进行修改。
    /// </summary>
    /// <param name="bytes">原始及异或后的二进制流。</param>
    /// <param name="startIndex">异或计算的开始位置。</param>
    /// <param name="length">异或计算长度。</param>
    /// <param name="code">异或二进制流(密钥)。</param>
    /// <exception cref="Exception">当code为null、长度为0或参数无效时抛出异常。</exception>
    public static void GetSelfXorBytes(byte[] bytes, int startIndex, int length, byte[] code)
    {
        if (bytes == null)
        {
            return;
        }

        if (code == null)
        {
            throw new Exception("Code is invalid.");
        }

        var codeLength = code.Length;
        if (codeLength <= 0)
        {
            throw new Exception("Code length is invalid.");
        }

        if (startIndex < 0 || length < 0 || startIndex + length > bytes.Length)
        {
            throw new Exception("Start index or length is invalid.");
        }

        var codeIndex = startIndex % codeLength;
        for (var i = startIndex; i < length; i++)
        {
            bytes[i] ^= code[codeIndex++];
            codeIndex %= codeLength;
        }
    }
}