using System;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// ObjectExtensions 补充覆盖测试：uint/long/ulong/short/ushort 的 IsRange / CheckRange 全路径。
    /// </summary>
    public class ObjectExtensionsAdditionalTests
    {
        // ============================================================
        // uint IsRange / CheckRange
        // ============================================================

        [Theory]
        [InlineData(5u, 0u, 10u, true)]
        [InlineData(0u, 0u, 10u, true)]
        [InlineData(10u, 0u, 10u, true)]
        [InlineData(11u, 0u, 10u, false)]
        public void IsRange_UInt_ShouldEvaluateCorrectly(uint value, uint min, uint max, bool expected)
        {
            Assert.Equal(expected, value.IsRange(min, max));
        }

        [Fact]
        public void IsRange_UInt_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5u.IsRange(10u, 5u));
        }

        [Fact]
        public void CheckRange_UInt_ValueGreaterThanMax_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 11u.CheckRange(0u, 10u));
        }

        [Fact]
        public void CheckRange_UInt_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5u.CheckRange(10u, 5u));
        }

        // ============================================================
        // long IsRange / CheckRange
        // ============================================================

        [Theory]
        [InlineData(5L, 0L, 10L, true)]
        [InlineData(0L, 0L, 10L, true)]
        [InlineData(10L, 0L, 10L, true)]
        [InlineData(-1L, 0L, 10L, false)]
        [InlineData(11L, 0L, 10L, false)]
        public void IsRange_Long_ShouldEvaluateCorrectly(long value, long min, long max, bool expected)
        {
            Assert.Equal(expected, value.IsRange(min, max));
        }

        [Fact]
        public void IsRange_Long_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5L.IsRange(10L, 5L));
        }

        [Fact]
        public void CheckRange_Long_ValueLessThanMin_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => (-1L).CheckRange(0L, 10L));
        }

        [Fact]
        public void CheckRange_Long_ValueGreaterThanMax_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 11L.CheckRange(0L, 10L));
        }

        [Fact]
        public void CheckRange_Long_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5L.CheckRange(10L, 5L));
        }

        // ============================================================
        // ulong IsRange / CheckRange
        // ============================================================

        [Theory]
        [InlineData(5UL, 0UL, 10UL, true)]
        [InlineData(0UL, 0UL, 10UL, true)]
        [InlineData(10UL, 0UL, 10UL, true)]
        [InlineData(11UL, 0UL, 10UL, false)]
        public void IsRange_ULong_ShouldEvaluateCorrectly(ulong value, ulong min, ulong max, bool expected)
        {
            Assert.Equal(expected, value.IsRange(min, max));
        }

        [Fact]
        public void IsRange_ULong_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5UL.IsRange(10UL, 5UL));
        }

        [Fact]
        public void CheckRange_ULong_ValueGreaterThanMax_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 11UL.CheckRange(0UL, 10UL));
        }

        [Fact]
        public void CheckRange_ULong_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 5UL.CheckRange(10UL, 5UL));
        }

        // ============================================================
        // short IsRange / CheckRange
        // ============================================================

        [Theory]
        [InlineData((short)5, (short)0, (short)10, true)]
        [InlineData((short)0, (short)0, (short)10, true)]
        [InlineData((short)10, (short)0, (short)10, true)]
        [InlineData((short)-1, (short)0, (short)10, false)]
        [InlineData((short)11, (short)0, (short)10, false)]
        public void IsRange_Short_ShouldEvaluateCorrectly(short value, short min, short max, bool expected)
        {
            Assert.Equal(expected, value.IsRange(min, max));
        }

        [Fact]
        public void IsRange_Short_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((short)5).IsRange((short)10, (short)5));
        }

        [Fact]
        public void CheckRange_Short_ValueLessThanMin_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((short)-1).CheckRange((short)0, (short)10));
        }

        [Fact]
        public void CheckRange_Short_ValueGreaterThanMax_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((short)11).CheckRange((short)0, (short)10));
        }

        [Fact]
        public void CheckRange_Short_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((short)5).CheckRange((short)10, (short)5));
        }

        // ============================================================
        // ushort IsRange / CheckRange
        // ============================================================

        [Theory]
        [InlineData((ushort)5, (ushort)0, (ushort)10, true)]
        [InlineData((ushort)0, (ushort)0, (ushort)10, true)]
        [InlineData((ushort)10, (ushort)0, (ushort)10, true)]
        [InlineData((ushort)11, (ushort)0, (ushort)10, false)]
        public void IsRange_UShort_ShouldEvaluateCorrectly(ushort value, ushort min, ushort max, bool expected)
        {
            Assert.Equal(expected, value.IsRange(min, max));
        }

        [Fact]
        public void IsRange_UShort_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ushort)5).IsRange((ushort)10, (ushort)5));
        }

        [Fact]
        public void CheckRange_UShort_ValueGreaterThanMax_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ushort)11).CheckRange((ushort)0, (ushort)10));
        }

        [Fact]
        public void CheckRange_UShort_InvalidRange_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ((ushort)5).CheckRange((ushort)10, (ushort)5));
        }

        // ============================================================
        // int IsRange 边界（补充现有测试中缺少的 false 分支）
        // ============================================================

        [Theory]
        [InlineData(5, 0, 10, true)]
        [InlineData(-1, 0, 10, false)]
        [InlineData(11, 0, 10, false)]
        public void IsRange_Int_ShouldEvaluateCorrectly(int value, int min, int max, bool expected)
        {
            Assert.Equal(expected, value.IsRange(min, max));
        }
    }
}
