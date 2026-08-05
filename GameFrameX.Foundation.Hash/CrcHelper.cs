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

using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Hash.Localization;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// CRC校验相关的实用函数。
/// 提供CRC32和CRC64两种校验算法的实现。
/// </summary>
/// <remarks>
/// CRC (Cyclic Redundancy Check) utility functions.
/// Provides implementations of both CRC32 and CRC64 checksum algorithms.
/// </remarks>
public static partial class CrcHelper
{
    /// <summary>
    /// 缓存字节数组的长度，用于分块读取大文件。
    /// </summary>
    /// <remarks>
    /// The length of the cached byte array, used for reading large files in chunks.
    /// </remarks>
    private const int CachedBytesLength = 0x1000;

    /// <summary>
    /// 计算二进制流的CRC64值。
    /// </summary>
    /// <remarks>
    /// Calculates the CRC64 checksum of a byte array.
    /// </remarks>
    /// <param name="bytes">要计算的二进制字节数组 / The byte array to calculate</param>
    /// <returns>计算得到的CRC64校验值 / The calculated CRC64 checksum value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null</exception>
    public static ulong GetCrc64(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        var algorithm = new Crc64();
        algorithm.Append(bytes);
        return algorithm.GetCurrentHashAsUInt64();
    }

    /// <summary>
    /// 计算流的CRC64值。
    /// </summary>
    /// <remarks>
    /// Calculates the CRC64 checksum of a stream.
    /// </remarks>
    /// <param name="stream">要计算的数据流 / The stream to calculate</param>
    /// <returns>计算得到的CRC64校验值 / The calculated CRC64 checksum value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="stream"/> 为 null 时抛出 / Thrown when <paramref name="stream"/> is null</exception>
    public static ulong GetCrc64(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        var algorithm = new Crc64();
        algorithm.Append(stream);
        return algorithm.GetCurrentHashAsUInt64();
    }

    /// <summary>
    /// 异步计算流的 CRC64 值 / Asynchronously calculates the CRC64 checksum of a stream.
    /// </summary>
    public static async Task<ulong> GetCrc64Async(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        cancellationToken.ThrowIfCancellationRequested();

        var cachedBytes = new byte[CachedBytesLength];
        var algorithm = new Crc64();
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(cachedBytes.AsMemory(), cancellationToken).ConfigureAwait(false)) > 0)
        {
            algorithm.Append(cachedBytes.AsSpan(0, bytesRead));
        }

        return algorithm.GetCurrentHashAsUInt64();
    }

    /// <summary>
    /// 计算二进制流的CRC32值。
    /// </summary>
    /// <remarks>
    /// Calculates the CRC32 checksum of a byte array.
    /// </remarks>
    /// <param name="bytes">要计算的二进制字节数组 / The byte array to calculate</param>
    /// <returns>计算得到的CRC32校验值 / The calculated CRC32 checksum value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null</exception>
    public static int GetCrc32(byte[] bytes)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        return GetCrc32(bytes, 0, bytes.Length);
    }

    /// <summary>
    /// 计算二进制流指定范围的CRC32值。
    /// </summary>
    /// <remarks>
    /// Calculates the CRC32 checksum of a specified range in a byte array.
    /// </remarks>
    /// <param name="bytes">要计算的二进制字节数组 / The byte array to calculate</param>
    /// <param name="offset">起始偏移量 / The starting offset</param>
    /// <param name="length">要计算的长度 / The length to calculate</param>
    /// <returns>计算得到的CRC32校验值 / The calculated CRC32 checksum value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="offset"/> 或 <paramref name="length"/> 参数无效时抛出 / Thrown when <paramref name="offset"/> or <paramref name="length"/> is invalid</exception>
    public static int GetCrc32(byte[] bytes, int offset, int length)
    {
        ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));

        if (offset < 0 || length < 0 || offset + length > bytes.Length)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Validation.InvalidDataLength), nameof(offset));
        }

        var algorithm = new Crc32();
        algorithm.HashCore(bytes, offset, length);
        return (int)algorithm.HashFinal();
    }

    /// <summary>
    /// 计算流的CRC32值。
    /// </summary>
    /// <remarks>
    /// Calculates the CRC32 checksum of a stream.
    /// </remarks>
    /// <param name="stream">要计算的数据流 / The stream to calculate</param>
    /// <returns>计算得到的CRC32校验值 / The calculated CRC32 checksum value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="stream"/> 为 null 时抛出 / Thrown when <paramref name="stream"/> is null</exception>
    public static int GetCrc32(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));

        var cachedBytes = new byte[CachedBytesLength];
        var algorithm = new Crc32();
        while (true)
        {
            var bytesRead = stream.Read(cachedBytes, 0, CachedBytesLength);
            if (bytesRead > 0)
            {
                algorithm.HashCore(cachedBytes, 0, bytesRead);
            }
            else
            {
                break;
            }
        }

        return (int)algorithm.HashFinal();
    }

    /// <summary>
    /// 异步计算流的 CRC32 值 / Asynchronously calculates the CRC32 checksum of a stream.
    /// </summary>
    public static async Task<int> GetCrc32Async(Stream stream, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        cancellationToken.ThrowIfCancellationRequested();

        var cachedBytes = new byte[CachedBytesLength];
        var algorithm = new Crc32();
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(cachedBytes.AsMemory(), cancellationToken).ConfigureAwait(false)) > 0)
        {
            algorithm.HashCore(cachedBytes, 0, bytesRead);
        }

        return (int)algorithm.HashFinal();
    }

    /// <summary>
    /// 将CRC32值转换为字节数组。
    /// </summary>
    /// <remarks>
    /// Converts a CRC32 value to a byte array.
    /// </remarks>
    /// <param name="crc32">要转换的CRC32值 / The CRC32 value to convert</param>
    /// <returns>转换后的4字节数组，按大端序排列 / A 4-byte array in big-endian order</returns>
    public static byte[] GetCrc32Bytes(int crc32)
    {
        return new[] { (byte)((crc32 >> 24) & 0xff), (byte)((crc32 >> 16) & 0xff), (byte)((crc32 >> 8) & 0xff), (byte)(crc32 & 0xff), };
    }

    /// <summary>
    /// 将CRC32值转换为字节数组并存入指定数组。
    /// </summary>
    /// <remarks>
    /// Converts a CRC32 value to a byte array and stores it in the specified array.
    /// </remarks>
    /// <param name="crc32">要转换的CRC32值 / The CRC32 value to convert</param>
    /// <param name="bytes">存放结果的目标数组 / The target array to store the result</param>
    public static void GetCrc32Bytes(int crc32, byte[] bytes)
    {
        GetCrc32Bytes(crc32, bytes, 0);
    }

    /// <summary>
    /// 将CRC32值转换为字节数组并存入指定数组的指定位置。
    /// </summary>
    /// <remarks>
    /// Converts a CRC32 value to a byte array and stores it at the specified position in the target array.
    /// </remarks>
    /// <param name="crc32">要转换的CRC32值 / The CRC32 value to convert</param>
    /// <param name="bytes">存放结果的目标数组 / The target array to store the result</param>
    /// <param name="offset">在目标数组中的起始位置 / The starting position in the target array</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="bytes"/> 为 null 时抛出 / Thrown when <paramref name="bytes"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="offset"/> 参数无效或目标数组剩余空间不足4字节时抛出 / Thrown when <paramref name="offset"/> is invalid or the remaining space is less than 4 bytes</exception>
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
    /// 使用指定编码计算流的CRC32值。
    /// </summary>
    /// <remarks>
    /// Calculates the CRC32 checksum of a stream using the specified encoding.
    /// </remarks>
    /// <param name="stream">要计算的数据流 / The stream to calculate</param>
    /// <param name="code">用于编码的字节数组，将与数据进行XOR运算 / The byte array used for encoding, will be XORed with the data</param>
    /// <param name="length">要计算的字节数，如果为负数或超过流长度则使用整个流 / The number of bytes to calculate, uses the entire stream if negative or exceeds stream length</param>
    /// <returns>计算得到的CRC32校验值 / The calculated CRC32 checksum value</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="stream"/> 或 <paramref name="code"/> 为 null 时抛出 / Thrown when <paramref name="stream"/> or <paramref name="code"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="code"/> 长度小于等于0时抛出 / Thrown when <paramref name="code"/> length is less than or equal to 0</exception>
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

        var cachedBytes = new byte[CachedBytesLength];
        var algorithm = new Crc32();
        var codeIndex = 0;
        int bytesRead;
        while ((bytesRead = stream.Read(cachedBytes, 0, CachedBytesLength)) > 0)
        {
            if (length > 0)
            {
                XorWithCode(cachedBytes, bytesRead, length, code, codeLength, ref codeIndex);
                length -= bytesRead;
            }

            algorithm.HashCore(cachedBytes, 0, bytesRead);
        }

        return (int)algorithm.HashFinal();
    }

    /// <summary>
    /// 将 <paramref name="code"/> 循环异或到 <paramref name="buffer"/> 的前 <c>min(bytesRead, length)</c> 个字节，并通过 <paramref name="codeIndex"/> 维护回绕位置。
    /// </summary>
    /// <param name="buffer">目标缓冲区 / The target buffer to XOR into</param>
    /// <param name="bytesRead">本次从流读到的字节数 / Bytes read from the stream in this iteration</param>
    /// <param name="length">剩余可异或字节数（<= 0 时调用方应跳过） / Remaining bytes available to XOR</param>
    /// <param name="code">用于异或的字节码 / The code byte array to XOR with</param>
    /// <param name="codeLength"><paramref name="code"/> 的长度 / Length of the code</param>
    /// <param name="codeIndex">循环异或的当前游标（按引用传递，调用方负责初始化为 0） / Current rotation index, passed by ref</param>
    private static void XorWithCode(byte[] buffer, int bytesRead, int length, byte[] code, int codeLength, ref int codeIndex)
    {
        var count = Math.Min(bytesRead, length);
        for (var i = 0; i < count; i++)
        {
            buffer[i] ^= code[codeIndex];
            codeIndex = (codeIndex + 1) % codeLength;
        }
    }
}
