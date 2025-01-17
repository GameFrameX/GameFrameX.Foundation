using System.Text;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// MurmurHash3 计算工具类。
/// MurmurHash是一种非加密型哈希算法，具有高性能和良好的哈希分布特性。
/// 此实现基于MurmurHash3的32位版本。
/// </summary>
public static class MurmurHash3Helper
{
    /// <summary>
    /// 使用 MurmurHash3 算法计算字符串的哈希值。
    /// 将字符串按UTF-8编码转换为字节数组后进行哈希计算。
    /// </summary>
    /// <param name="str">要计算哈希值的字符串</param>
    /// <param name="seed">哈希算法的种子值，默认为27。不同的种子值会产生不同的哈希结果。</param>
    /// <returns>32位无符号整数形式的哈希值</returns>
    public static uint Hash(string str, uint seed = 27)
    {
        var data = Encoding.UTF8.GetBytes(str);
        return Hash(data, (uint)data.Length, seed);
    }

    /// <summary>
    /// 使用 MurmurHash3 算法计算字节数组的哈希值。
    /// 此方法实现了MurmurHash3的核心算法逻辑。
    /// </summary>
    /// <param name="data">要计算哈希值的字节数组</param>
    /// <param name="length">字节数组的有效长度</param>
    /// <param name="seed">哈希算法的种子值，用于初始化哈希计算</param>
    /// <returns>32位无符号整数形式的哈希值</returns>
    public static uint Hash(byte[] data, uint length, uint seed)
    {
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
    /// <param name="h">要混合的哈希值</param>
    /// <returns>混合后的最终哈希值</returns>
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
    /// <param name="x">要进行循环左移的整数</param>
    /// <param name="r">左移的位数</param>
    /// <returns>循环左移后的整数</returns>
    private static uint Rotl32(uint x, byte r)
    {
        return (x << r) | (x >> (32 - r));
    }
}