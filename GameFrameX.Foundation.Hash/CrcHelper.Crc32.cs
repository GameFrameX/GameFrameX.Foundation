﻿namespace GameFrameX.Foundation.Hash;

public static partial class CrcHelper
{
    /// <summary>
    /// CRC32 算法。
    /// </summary>
    internal sealed class Crc32
    {
        private const int TableLength = 256;
        private const uint DefaultPolynomial = 0xedb88320;
        private const uint DefaultSeed = 0xffffffff;

        private readonly uint m_Seed;
        private readonly uint[] m_Table;
        private uint m_Hash;

        public Crc32() : this(DefaultPolynomial, DefaultSeed)
        {
        }

        public Crc32(uint polynomial, uint seed)
        {
            m_Seed = seed;
            m_Table = InitializeTable(polynomial);
            m_Hash = seed;
        }

        public void Initialize()
        {
            m_Hash = m_Seed;
        }

        public void HashCore(byte[] bytes, int offset, int length)
        {
            m_Hash = CalculateHash(m_Table, m_Hash, bytes, offset, length);
        }

        public uint HashFinal()
        {
            return ~m_Hash;
        }

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