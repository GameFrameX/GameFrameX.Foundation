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

using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// MurmurHash3 计算工具类。
/// MurmurHash是一种非加密型哈希算法，具有高性能和良好的哈希分布特性。
/// 此实现基于MurmurHash3的32位版本。
/// </summary>
/// <remarks>
/// MurmurHash3 computation utility class.
/// MurmurHash is a non-cryptographic hash algorithm with high performance and good hash distribution characteristics.
/// This implementation is based on the 32-bit version of MurmurHash3.
/// </remarks>
public static class MurmurHash3Helper
{
    /// <summary>
    /// 使用 MurmurHash3 算法计算字符串的哈希值。
    /// 将字符串按UTF-8编码转换为字节数组后进行哈希计算。
    /// </summary>
    /// <remarks>
    /// Computes the hash of a string using the MurmurHash3 algorithm.
    /// Converts the string to a byte array using UTF-8 encoding before computing the hash.
    /// </remarks>
    /// <param name="str">要计算哈希值的字符串，不能为null / The string to compute the hash for, cannot be null</param>
    /// <param name="seed">哈希算法的种子值，默认为27。不同的种子值会产生不同的哈希结果 / The seed value for the hash algorithm, defaults to 27. Different seed values produce different hash results</param>
    /// <returns>32位无符号整数形式的哈希值 / The 32-bit hash value as an unsigned integer</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="str"/> 为 null 时抛出 / Thrown when <paramref name="str"/> is null</exception>
    public static uint Hash(string str, uint seed = 27)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        
        var data = Encoding.UTF8.GetBytes(str);
        return Hash(data, (uint)data.Length, seed);
    }

    /// <summary>
    /// 使用 MurmurHash3 算法计算字节数组的哈希值。
    /// 此方法实现了MurmurHash3的核心算法逻辑。
    /// </summary>
    /// <remarks>
    /// Computes the hash of a byte array using the MurmurHash3 algorithm.
    /// This method implements the core algorithm logic of MurmurHash3.
    /// </remarks>
    /// <param name="data">要计算哈希值的字节数组，不能为null / The byte array to compute the hash for, cannot be null</param>
    /// <param name="length">字节数组的有效长度，不能超过数组实际长度 / The valid length of the byte array, cannot exceed the actual array length</param>
    /// <param name="seed">哈希算法的种子值，用于初始化哈希计算 / The seed value for the hash algorithm, used to initialize hash computation</param>
    /// <returns>32位无符号整数形式的哈希值 / The 32-bit hash value as an unsigned integer</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="data"/> 为 null 时抛出 / Thrown when <paramref name="data"/> is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="length"/> 超过数组长度时抛出 / Thrown when <paramref name="length"/> exceeds the array length</exception>
    public static uint Hash(byte[] data, uint length, uint seed)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        
        ArgumentOutOfRangeException.ThrowIfGreaterThan(length, (uint)data.Length, nameof(length));
        // 计算4字节对齐的块数
        var nblocks = length >> 2;

        var h1 = seed;

        // 算法使用的魔数常量
        const uint c1 = 0xcc9e2d51;
        const uint c2 = 0x1b873593;

        //----------
        // 主体部分：每次处理4个字节
        var i = 0;

        for (var j = nblocks; j > 0; --j)
        {
            // 从字节数组中读取一个32位整数
            var k1L = BitConverter.ToUInt32(data, i);

            // 对这4个字节进行哈希混合操作
            k1L *= c1;
            k1L = Rotl32(k1L, 15);
            k1L *= c2;

            h1 ^= k1L;
            h1 = Rotl32(h1, 13);
            h1 = h1 * 5 + 0xe6546b64;

            i += 4;
        }

        //----------
        // 尾部处理：处理剩余的1-3个字节
        nblocks <<= 2;

        uint k1 = 0;

        var tailLength = length & 3;

        // 根据剩余字节数，以不同方式合并到k1中
        if (tailLength == 3)
        {
            k1 ^= (uint)data[2 + nblocks] << 16;
        }

        if (tailLength >= 2)
        {
            k1 ^= (uint)data[1 + nblocks] << 8;
        }

        if (tailLength >= 1)
        {
            k1 ^= data[nblocks];
            k1 *= c1;
            k1 = Rotl32(k1, 15);
            k1 *= c2;
            h1 ^= k1;
        }

        //----------
        // 最终处理：加入长度信息并进行最后的混合
        h1 ^= length;

        h1 = Fmix32(h1);

        return h1;
    }

    /// <summary>
    /// 对哈希值进行最终混合操作。
    /// 通过多次异或、乘法和位移操作增加最终哈希值的随机性。
    /// </summary>
    /// <remarks>
    /// Performs final mixing on the hash value.
    /// Increases the randomness of the final hash value through multiple XOR, multiplication, and bit shift operations.
    /// </remarks>
    /// <param name="h">要混合的哈希值 / The hash value to mix</param>
    /// <returns>混合后的最终哈希值 / The final mixed hash value</returns>
    private static uint Fmix32(uint h)
    {
        h ^= h >> 16;
        h *= 0x85ebca6b;
        h ^= h >> 13;
        h *= 0xc2b2ae35;
        h ^= h >> 16;

        return h;
    }

    /// <summary>
    /// 对32位整数进行循环左移操作。
    /// 循环左移是指将溢出的高位补到低位。
    /// </summary>
    /// <remarks>
    /// Performs a rotate left operation on a 32-bit integer.
    /// Rotate left means the overflowed high bits are moved to the low bits.
    /// </remarks>
    /// <param name="x">要进行循环左移的整数 / The integer to rotate left</param>
    /// <param name="r">左移的位数 / The number of bits to rotate left</param>
    /// <returns>循环左移后的整数 / The rotated integer</returns>
    private static uint Rotl32(uint x, byte r)
    {
        return (x << r) | (x >> (32 - r));
    }
}