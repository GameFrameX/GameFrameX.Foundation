using System.Runtime.CompilerServices;
using System.Text;
using Standart.Hash.xxHash;

namespace GameFrameX.Foundation.Hash;

/// <summary>
/// xxHash 哈希算法工具类。
/// 提供32位、64位和128位哈希值计算功能。
/// xxHash是一种非加密型哈希算法，专注于高性能和高质量的哈希计算。
/// </summary>
public static class XxHashHelper
{
    /// <summary>
    /// 计算给定字节数组的32位哈希值。
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组</param>
    /// <returns>32位无符号整数形式的哈希值</returns>
    public static ulong Hash32(byte[] buffer)
    {
        return xxHash32.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算给定文本的32位哈希值。
    /// 使用UTF-8编码将文本转换为字节数组后计算哈希值。
    /// </summary>
    /// <param name="text">要计算哈希值的文本</param>
    /// <returns>32位无符号整数形式的哈希值</returns>
    public static uint Hash32(string text)
    {
        return xxHash32.ComputeHash(text);
    }

    /// <summary>
    /// 计算给定类型的32位哈希值。
    /// 基于类型的完全限定名计算哈希值。
    /// </summary>
    /// <param name="type">要计算哈希值的类型</param>
    /// <returns>32位无符号整数形式的哈希值</returns>
    public static uint Hash32(Type type)
    {
        return InternalXxHashHelper.Hash32(type);
    }

    /// <summary>
    /// 计算给定泛型类型参数的32位哈希值。
    /// 基于类型的完全限定名计算哈希值。
    /// </summary>
    /// <typeparam name="T">要计算哈希值的泛型类型参数</typeparam>
    /// <returns>32位无符号整数形式的哈希值</returns>
    public static uint Hash32<T>()
    {
        return InternalXxHashHelper.Hash32<T>();
    }

    /// <summary>
    /// 计算给定字节数组的64位哈希值。
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组</param>
    /// <returns>64位无符号整数形式的哈希值</returns>
    public static ulong Hash64(byte[] buffer)
    {
        return xxHash64.ComputeHash(buffer);
    }

    /// <summary>
    /// 计算给定文本的64位哈希值。
    /// 使用UTF-8编码将文本转换为字节数组后计算哈希值。
    /// </summary>
    /// <param name="text">要计算哈希值的文本</param>
    /// <returns>64位无符号整数形式的哈希值</returns>
    public static ulong Hash64(string text)
    {
        return xxHash64.ComputeHash(text);
    }

    /// <summary>
    /// 计算给定类型的64位哈希值。
    /// 基于类型的完全限定名计算哈希值。
    /// </summary>
    /// <param name="type">要计算哈希值的类型</param>
    /// <returns>64位无符号整数形式的哈希值</returns>
    public static ulong Hash64(Type type)
    {
        return InternalXxHashHelper.Hash64(type);
    }

    /// <summary>
    /// 计算给定泛型类型参数的64位哈希值。
    /// 基于类型的完全限定名计算哈希值。
    /// </summary>
    /// <typeparam name="T">要计算哈希值的泛型类型参数</typeparam>
    /// <returns>64位无符号整数形式的哈希值</returns>
    public static ulong Hash64<T>()
    {
        return InternalXxHashHelper.Hash64<T>();
    }

    /// <summary>
    /// 计算给定字节数组的128位哈希值。
    /// 使用数组的全部长度进行计算。
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组</param>
    /// <returns>128位无符号整数形式的哈希值</returns>
    public static uint128 Hash128(byte[] buffer)
    {
        return xxHash128.ComputeHash(buffer, buffer.Length);
    }

    /// <summary>
    /// 判断128位哈希值是否为默认值(全0)。
    /// </summary>
    /// <param name="self">要判断的128位哈希值</param>
    /// <returns>如果高64位和低64位都为0则返回true，否则返回false</returns>
    public static bool IsDefault(uint128 self)
    {
        return self is { high64: 0, low64: 0, };
    }

    /// <summary>
    /// 计算给定字节数组的128位哈希值。
    /// 使用指定的长度进行计算。
    /// </summary>
    /// <param name="buffer">要计算哈希值的字节数组</param>
    /// <param name="length">要参与计算的字节长度</param>
    /// <returns>128位无符号整数形式的哈希值</returns>
    public static uint128 Hash128(byte[] buffer, int length)
    {
        return xxHash128.ComputeHash(buffer, length);
    }

    /// <summary>
    /// 计算给定文本的128位哈希值。
    /// 使用UTF-8编码将文本转换为字节数组后计算哈希值。
    /// </summary>
    /// <param name="text">要计算哈希值的文本</param>
    /// <returns>128位无符号整数形式的哈希值</returns>
    public static uint128 Hash128(string text)
    {
        return xxHash128.ComputeHash(text);
    }

    /// <summary>
    /// 内部xxHash实现帮助类。
    /// 提供32位和64位哈希算法的底层实现。
    /// </summary>
    private static class InternalXxHashHelper
    {
        /// <summary>
        /// 计算32位xxHash值的核心算法。
        /// 直接操作内存指针以获得最佳性能。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe uint Hash32(byte* input, int length, uint seed = 0)
        {
            unchecked
            {
                const uint prime1 = 2654435761u;
                const uint prime2 = 2246822519u;
                const uint prime3 = 3266489917u;
                const uint prime4 = 0668265263u;
                const uint prime5 = 0374761393u;

                var hash = seed + prime5;

                if (length >= 16)
                {
                    var val0 = seed + prime1 + prime2;
                    var val1 = seed + prime2;
                    var val2 = seed + 0;
                    var val3 = seed - prime1;

                    var count = length >> 4;
                    for (var i = 0; i < count; i++)
                    {
                        var pos0 = *(uint*)(input + 0);
                        var pos1 = *(uint*)(input + 4);
                        var pos2 = *(uint*)(input + 8);
                        var pos3 = *(uint*)(input + 12);

                        val0 += pos0 * prime2;
                        val0 = (val0 << 13) | (val0 >> (32 - 13));
                        val0 *= prime1;

                        val1 += pos1 * prime2;
                        val1 = (val1 << 13) | (val1 >> (32 - 13));
                        val1 *= prime1;

                        val2 += pos2 * prime2;
                        val2 = (val2 << 13) | (val2 >> (32 - 13));
                        val2 *= prime1;

                        val3 += pos3 * prime2;
                        val3 = (val3 << 13) | (val3 >> (32 - 13));
                        val3 *= prime1;

                        input += 16;
                    }

                    hash = ((val0 << 01) | (val0 >> (32 - 01))) +
                           ((val1 << 07) | (val1 >> (32 - 07))) +
                           ((val2 << 12) | (val2 >> (32 - 12))) +
                           ((val3 << 18) | (val3 >> (32 - 18)));
                }

                hash += (uint)length;

                length &= 15;
                while (length >= 4)
                {
                    hash += *(uint*)input * prime3;
                    hash = ((hash << 17) | (hash >> (32 - 17))) * prime4;
                    input += 4;
                    length -= 4;
                }

                while (length > 0)
                {
                    hash += *input * prime5;
                    hash = ((hash << 11) | (hash >> (32 - 11))) * prime1;
                    ++input;
                    --length;
                }

                hash ^= hash >> 15;
                hash *= prime2;
                hash ^= hash >> 13;
                hash *= prime3;
                hash ^= hash >> 16;

                return hash;
            }
        }

        /// <summary>
        /// 计算64位xxHash值的核心算法。
        /// 直接操作内存指针以获得最佳性能。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe ulong Hash64(byte* input, int length, uint seed = 0)
        {
            unchecked
            {
                const ulong prime1 = 11400714785074694791ul;
                const ulong prime2 = 14029467366897019727ul;
                const ulong prime3 = 01609587929392839161ul;
                const ulong prime4 = 09650029242287828579ul;
                const ulong prime5 = 02870177450012600261ul;

                var hash = seed + prime5;

                if (length >= 32)
                {
                    var val0 = seed + prime1 + prime2;
                    var val1 = seed + prime2;
                    ulong val2 = seed + 0;
                    var val3 = seed - prime1;

                    var count = length >> 5;
                    for (var i = 0; i < count; i++)
                    {
                        var pos0 = *(ulong*)(input + 0);
                        var pos1 = *(ulong*)(input + 8);
                        var pos2 = *(ulong*)(input + 16);
                        var pos3 = *(ulong*)(input + 24);

                        val0 += pos0 * prime2;
                        val0 = (val0 << 31) | (val0 >> (64 - 31));
                        val0 *= prime1;

                        val1 += pos1 * prime2;
                        val1 = (val1 << 31) | (val1 >> (64 - 31));
                        val1 *= prime1;

                        val2 += pos2 * prime2;
                        val2 = (val2 << 31) | (val2 >> (64 - 31));
                        val2 *= prime1;

                        val3 += pos3 * prime2;
                        val3 = (val3 << 31) | (val3 >> (64 - 31));
                        val3 *= prime1;

                        input += 32;
                    }

                    hash = ((val0 << 01) | (val0 >> (64 - 01))) +
                           ((val1 << 07) | (val1 >> (64 - 07))) +
                           ((val2 << 12) | (val2 >> (64 - 12))) +
                           ((val3 << 18) | (val3 >> (64 - 18)));

                    val0 *= prime2;
                    val0 = (val0 << 31) | (val0 >> (64 - 31));
                    val0 *= prime1;
                    hash ^= val0;
                    hash = hash * prime1 + prime4;

                    val1 *= prime2;
                    val1 = (val1 << 31) | (val1 >> (64 - 31));
                    val1 *= prime1;
                    hash ^= val1;
                    hash = hash * prime1 + prime4;

                    val2 *= prime2;
                    val2 = (val2 << 31) | (val2 >> (64 - 31));
                    val2 *= prime1;
                    hash ^= val2;
                    hash = hash * prime1 + prime4;

                    val3 *= prime2;
                    val3 = (val3 << 31) | (val3 >> (64 - 31));
                    val3 *= prime1;
                    hash ^= val3;
                    hash = hash * prime1 + prime4;
                }

                hash += (ulong)length;

                length &= 31;
                while (length >= 8)
                {
                    var lane = *(ulong*)input * prime2;
                    lane = ((lane << 31) | (lane >> (64 - 31))) * prime1;
                    hash ^= lane;
                    hash = ((hash << 27) | (hash >> (64 - 27))) * prime1 + prime4;
                    input += 8;
                    length -= 8;
                }

                if (length >= 4)
                {
                    hash ^= *(uint*)input * prime1;
                    hash = ((hash << 23) | (hash >> (64 - 23))) * prime2 + prime3;
                    input += 4;
                    length -= 4;
                }

                while (length > 0)
                {
                    hash ^= *input * prime5;
                    hash = ((hash << 11) | (hash >> (64 - 11))) * prime1;
                    ++input;
                    --length;
                }

                hash ^= hash >> 33;
                hash *= prime2;
                hash ^= hash >> 29;
                hash *= prime3;
                hash ^= hash >> 32;

                return hash;
            }
        }

        /// <summary>
        /// 计算字节数组的32位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Hash32(byte[] buffer)
        {
            var length = buffer.Length;
            unsafe
            {
                fixed (byte* pointer = buffer)
                {
                    return Hash32(pointer, length);
                }
            }
        }

        /// <summary>
        /// 计算字符串的32位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Hash32(string text)
        {
            return Hash32(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// 计算类型的32位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Hash32(Type type)
        {
            return Hash32(type.FullName);
        }

        /// <summary>
        /// 计算泛型类型的32位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Hash32<T>()
        {
            return Hash32(typeof(T));
        }

        /// <summary>
        /// 计算字节数组的64位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Hash64(byte[] buffer)
        {
            var length = buffer.Length;
            unsafe
            {
                fixed (byte* pointer = buffer)
                {
                    return Hash64(pointer, length);
                }
            }
        }

        /// <summary>
        /// 计算字符串的64位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Hash64(string text)
        {
            return Hash64(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// 计算类型的64位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Hash64(Type type)
        {
            return Hash64(type.FullName);
        }

        /// <summary>
        /// 计算泛型类型的64位哈希值。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Hash64<T>()
        {
            return Hash64(typeof(T));
        }
    }
}