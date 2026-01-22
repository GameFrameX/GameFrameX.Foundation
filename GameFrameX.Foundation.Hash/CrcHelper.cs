using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Hash.Localization;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// CRC校验相关的实用函数。
/// 提供CRC32和CRC64两种校验算法的实现。
/// </summary>
public static partial class CrcHelper
{
    /// <summary>
    /// 缓存字节数组的长度,用于分块读取大文件
    /// </summary>
    private const int CachedBytesLength = 0x1000;

    /// <summary>
    /// 用于缓存读取数据的字节数组
    /// </summary>
    private static readonly byte[] SCachedBytes = new byte[CachedBytesLength];

    /// <summary>
    /// CRC32算法的实例
    /// </summary>
    private static readonly CrcHelper.Crc32 SAlgorithm = new();

    /// <summary>
    /// CRC64算法的实例
    /// </summary>
    private static readonly CrcHelper.Crc64 SAlgorithm64 = new();

    /// <summary>
    /// 计算二进制流的CRC64值
    /// </summary>
    /// <param name="bytes">要计算的二进制字节数组</param>
    /// <returns>计算得到的CRC64校验值</returns>
    /// <exception cref="ArgumentNullException">当bytes参数为null时抛出</exception>
    public static ulong GetCrc64(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
        
        SAlgorithm64.Reset();
        SAlgorithm64.Append(bytes);
        return SAlgorithm64.GetCurrentHashAsUInt64();
    }

    /// <summary>
    /// 计算流的CRC64值
    /// </summary>
    /// <param name="stream">要计算的数据流</param>
    /// <returns>计算得到的CRC64校验值</returns>
    /// <exception cref="ArgumentNullException">当stream参数为null时抛出</exception>
    public static ulong GetCrc64(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        
        SAlgorithm64.Reset();
        SAlgorithm64.Append(stream);
        return SAlgorithm64.GetCurrentHashAsUInt64();
    }

    /// <summary>
    /// 计算二进制流的CRC32值
    /// </summary>
    /// <param name="bytes">要计算的二进制字节数组</param>
    /// <returns>计算得到的CRC32校验值</returns>
    /// <exception cref="ArgumentNullException">当bytes参数为null时抛出</exception>
    public static int GetCrc32(byte[] bytes)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes), @"Bytes is invalid.");
        }

        return GetCrc32(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// 计算二进制流指定范围的CRC32值
    /// </summary>
    /// <param name="bytes">要计算的二进制字节数组</param>
    /// <param name="offset">起始偏移量</param>
    /// <param name="length">要计算的长度</param>
    /// <returns>计算得到的CRC32校验值</returns>
    /// <exception cref="ArgumentNullException">当bytes参数为null时抛出</exception>
    /// <exception cref="ArgumentException">当offset或length参数无效时抛出</exception>
    public static int GetCrc32(byte[] bytes, int offset, int length)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes), @"Bytes is invalid.");
        }

        if (offset < 0 || length < 0 || offset + length > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Validation.InvalidDataLength), nameof(offset));
        }

        SAlgorithm.HashCore(bytes, offset, length);
        var result = (int)SAlgorithm.HashFinal();
        SAlgorithm.Initialize();
        return result;
    }

    /// <summary>
    /// 计算流的CRC32值
    /// </summary>
    /// <param name="stream">要计算的数据流</param>
    /// <returns>计算得到的CRC32校验值</returns>
    /// <exception cref="ArgumentNullException">当stream参数为null时抛出</exception>
    public static int GetCrc32(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream), @"Stream is invalid.");
        }

        while (true)
        {
            var bytesRead = stream.Read(SCachedBytes, 0, CachedBytesLength);
            if (bytesRead > 0)
            {
                SAlgorithm.HashCore(SCachedBytes, 0, bytesRead);
            }
            else
            {
                break;
            }
        }

        var result = (int)SAlgorithm.HashFinal();
        SAlgorithm.Initialize();
        Array.Clear(SCachedBytes, 0, CachedBytesLength);
        return result;
    }

    /// <summary>
    /// 将CRC32值转换为字节数组
    /// </summary>
    /// <param name="crc32">要转换的CRC32值</param>
    /// <returns>转换后的4字节数组，按大端序排列</returns>
    public static byte[] GetCrc32Bytes(int crc32)
    {
        return new[] { (byte)((crc32 >> 24) & 0xff), (byte)((crc32 >> 16) & 0xff), (byte)((crc32 >> 8) & 0xff), (byte)(crc32 & 0xff), };
    }

    /// <summary>
    /// 将CRC32值转换为字节数组并存入指定数组
    /// </summary>
    /// <param name="crc32">要转换的CRC32值</param>
    /// <param name="bytes">存放结果的目标数组</param>
    public static void GetCrc32Bytes(int crc32, byte[] bytes)
    {
        GetCrc32Bytes(crc32, bytes, 0);
    }

    /// <summary>
    /// 将CRC32值转换为字节数组并存入指定数组的指定位置
    /// </summary>
    /// <param name="crc32">要转换的CRC32值</param>
    /// <param name="bytes">存放结果的目标数组</param>
    /// <param name="offset">在目标数组中的起始位置</param>
    /// <exception cref="ArgumentNullException">当bytes参数为null时抛出</exception>
    /// <exception cref="ArgumentException">当offset参数无效或目标数组剩余空间不足4字节时抛出</exception>
    public static void GetCrc32Bytes(int crc32, byte[] bytes, int offset)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes), @"Result is invalid.");
        }

        if (offset < 0 || offset + 4 > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Validation.InvalidDataLength), nameof(offset));
        }

        bytes[offset] = (byte)((crc32 >> 24) & 0xff);
        bytes[offset + 1] = (byte)((crc32 >> 16) & 0xff);
        bytes[offset + 2] = (byte)((crc32 >> 8) & 0xff);
        bytes[offset + 3] = (byte)(crc32 & 0xff);
    }

    /// <summary>
    /// 使用指定编码计算流的CRC32值
    /// </summary>
    /// <param name="stream">要计算的数据流</param>
    /// <param name="code">用于编码的字节数组，将与数据进行XOR运算</param>
    /// <param name="length">要计算的字节数，如果为负数或超过流长度则使用整个流</param>
    /// <returns>计算得到的CRC32校验值</returns>
    /// <exception cref="ArgumentNullException">当stream或code参数为null时抛出</exception>
    /// <exception cref="ArgumentException">当code长度小于等于0时抛出</exception>
    internal static int GetCrc32(Stream stream, byte[] code, int length)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream), @"Stream is invalid.");
        }

        if (code == null)
        {
            throw new ArgumentNullException(nameof(code), @"Code is invalid.");
        }

        var codeLength = code.Length;
        if (codeLength <= 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Validation.InvalidDataLength), nameof(codeLength));
        }

        var bytesLength = (int)stream.Length;
        if (length < 0 || length > bytesLength)
        {
            length = bytesLength;
        }

        var codeIndex = 0;
        while (true)
        {
            var bytesRead = stream.Read(SCachedBytes, 0, CachedBytesLength);
            if (bytesRead > 0)
            {
                if (length > 0)
                {
                    for (var i = 0; i < bytesRead && i < length; i++)
                    {
                        SCachedBytes[i] ^= code[codeIndex++];
                        codeIndex %= codeLength;
                    }

                    length -= bytesRead;
                }

                SAlgorithm.HashCore(SCachedBytes, 0, bytesRead);
            }
            else
            {
                break;
            }
        }

        var result = (int)SAlgorithm.HashFinal();
        SAlgorithm.Initialize();
        Array.Clear(SCachedBytes, 0, CachedBytesLength);
        return result;
    }
}