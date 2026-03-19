namespace GameFrameX.Foundation.Hash;

public static partial class CrcHelper
{
    /// <summary>
    /// CRC32 算法实现。
    /// </summary>
    /// <remarks>
    /// Implementation of the CRC32 algorithm.
    /// </remarks>
    internal sealed class Crc32
    {
        private const int TableLength = 256;
        private const uint DefaultPolynomial = 0xedb88320;
        private const uint DefaultSeed = 0xffffffff;

        private readonly uint m_Seed;
        private readonly uint[] m_Table;
        private uint m_Hash;

        /// <summary>
        /// 初始化 <see cref="Crc32"/> 类的新实例，使用默认多项式和种子值。
        /// </summary>
        /// <remarks>
        /// Initializes a new instance of the <see cref="Crc32"/> class with default polynomial and seed values.
        /// </remarks>
        public Crc32() : this(DefaultPolynomial, DefaultSeed)
        {
        }

        /// <summary>
        /// 初始化 <see cref="Crc32"/> 类的新实例，使用指定的多项式和种子值。
        /// </summary>
        /// <remarks>
        /// Initializes a new instance of the <see cref="Crc32"/> class with the specified polynomial and seed values.
        /// </remarks>
        /// <param name="polynomial">用于CRC计算的多项式 / The polynomial used for CRC calculation</param>
        /// <param name="seed">用于CRC计算的初始种子值 / The initial seed value for CRC calculation</param>
        public Crc32(uint polynomial, uint seed)
        {
            m_Seed = seed;
            m_Table = InitializeTable(polynomial);
            m_Hash = seed;
        }

        /// <summary>
        /// 重置哈希计算到初始状态。
        /// </summary>
        /// <remarks>
        /// Resets the hash computation to the initial state.
        /// </remarks>
        public void Initialize()
        {
            m_Hash = m_Seed;
        }

        /// <summary>
        /// 对字节数组的指定范围进行哈希计算。
        /// </summary>
        /// <remarks>
        /// Computes the hash for the specified range of the byte array.
        /// </remarks>
        /// <param name="bytes">要计算哈希的字节数组 / The byte array to compute the hash for</param>
        /// <param name="offset">起始偏移量 / The starting offset</param>
        /// <param name="length">要计算的长度 / The length to compute</param>
        public void HashCore(byte[] bytes, int offset, int length)
        {
            m_Hash = CalculateHash(m_Table, m_Hash, bytes, offset, length);
        }

        /// <summary>
        /// 获取最终计算得到的CRC32哈希值。
        /// </summary>
        /// <remarks>
        /// Gets the final computed CRC32 hash value.
        /// </remarks>
        /// <returns>CRC32哈希值 / The CRC32 hash value</returns>
        public uint HashFinal()
        {
            return ~m_Hash;
        }

        /// <summary>
        /// 使用指定的查找表计算字节数组的哈希值。
        /// </summary>
        /// <remarks>
        /// Calculates the hash of a byte array using the specified lookup table.
        /// </remarks>
        /// <param name="table">CRC查找表 / The CRC lookup table</param>
        /// <param name="value">初始哈希值 / The initial hash value</param>
        /// <param name="bytes">要计算哈希的字节数组 / The byte array to compute the hash for</param>
        /// <param name="offset">起始偏移量 / The starting offset</param>
        /// <param name="length">要计算的长度 / The length to compute</param>
        /// <returns>计算得到的哈希值 / The computed hash value</returns>
        private static uint CalculateHash(uint[] table, uint value, byte[] bytes, int offset, int length)
        {
            var last = offset + length;
            for (var i = offset; i < last; i++)
            {
                unchecked
                {
                    value = (value >> 8) ^ table[bytes[i] ^ (value & 0xff)];
                }
            }

            return value;
        }

        /// <summary>
        /// 根据指定的多项式初始化CRC查找表。
        /// </summary>
        /// <remarks>
        /// Initializes the CRC lookup table based on the specified polynomial.
        /// </remarks>
        /// <param name="polynomial">用于生成查找表的多项式 / The polynomial used to generate the lookup table</param>
        /// <returns>初始化后的CRC查找表 / The initialized CRC lookup table</returns>
        private static uint[] InitializeTable(uint polynomial)
        {
            var table = new uint[TableLength];
            for (var i = 0; i < TableLength; i++)
            {
                var entry = (uint)i;
                for (var j = 0; j < 8; j++)
                {
                    if ((entry & 1) == 1)
                    {
                        entry = (entry >> 1) ^ polynomial;
                    }
                    else
                    {
                        entry >>= 1;
                    }
                }

                table[i] = entry;
            }

            return table;
        }
    }
}