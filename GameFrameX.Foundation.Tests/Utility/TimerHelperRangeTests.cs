using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Range.cs 单元测试：覆盖 IsTimeInRange 与 IsTimestampInRange 的闭区间边界。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperRangeTests
    {
        #region IsTimeInRange

        [Fact]
        public void IsTimeInRange_WhenTimeIsInRange_ShouldReturnTrue()
        {
            var start = new DateTime(2024, 1, 10, 0, 0, 0);
            var end = new DateTime(2024, 1, 10, 23, 59, 59);
            var time = new DateTime(2024, 1, 10, 14, 30, 0);

            Assert.True(TimerHelper.IsTimeInRange(time, start, end));
        }

        [Fact]
        public void IsTimeInRange_WhenTimeEqualsStart_ShouldReturnTrue()
        {
            var start = new DateTime(2024, 1, 10, 0, 0, 0);
            var end = new DateTime(2024, 1, 10, 23, 59, 59);

            Assert.True(TimerHelper.IsTimeInRange(start, start, end));
        }

        [Fact]
        public void IsTimeInRange_WhenTimeEqualsEnd_ShouldReturnTrue()
        {
            var start = new DateTime(2024, 1, 10, 0, 0, 0);
            var end = new DateTime(2024, 1, 10, 23, 59, 59);

            Assert.True(TimerHelper.IsTimeInRange(end, start, end));
        }

        [Fact]
        public void IsTimeInRange_WhenTimeBeforeStart_ShouldReturnFalse()
        {
            var start = new DateTime(2024, 1, 10, 0, 0, 0);
            var end = new DateTime(2024, 1, 10, 23, 59, 59);
            var time = new DateTime(2024, 1, 9, 23, 59, 59);

            Assert.False(TimerHelper.IsTimeInRange(time, start, end));
        }

        [Fact]
        public void IsTimeInRange_WhenTimeAfterEnd_ShouldReturnFalse()
        {
            var start = new DateTime(2024, 1, 10, 0, 0, 0);
            var end = new DateTime(2024, 1, 10, 23, 59, 59);
            var time = new DateTime(2024, 1, 11, 0, 0, 0);

            Assert.False(TimerHelper.IsTimeInRange(time, start, end));
        }

        [Fact]
        public void IsTimeInRange_WhenStartEqualsEndAndTimeEquals_ShouldReturnTrue()
        {
            var point = new DateTime(2024, 1, 10, 12, 0, 0);

            Assert.True(TimerHelper.IsTimeInRange(point, point, point));
        }

        [Fact]
        public void IsTimeInRange_WhenStartEqualsEndAndTimeDifferent_ShouldReturnFalse()
        {
            var point = new DateTime(2024, 1, 10, 12, 0, 0);
            var other = new DateTime(2024, 1, 10, 12, 0, 1);

            Assert.False(TimerHelper.IsTimeInRange(other, point, point));
        }

        #endregion

        #region IsTimestampInRange

        [Fact]
        public void IsTimestampInRange_WhenTimestampInRange_ShouldReturnTrue()
        {
            Assert.True(TimerHelper.IsTimestampInRange(150L, 100L, 200L));
        }

        [Fact]
        public void IsTimestampInRange_WhenTimestampEqualsStart_ShouldReturnTrue()
        {
            Assert.True(TimerHelper.IsTimestampInRange(100L, 100L, 200L));
        }

        [Fact]
        public void IsTimestampInRange_WhenTimestampEqualsEnd_ShouldReturnTrue()
        {
            Assert.True(TimerHelper.IsTimestampInRange(200L, 100L, 200L));
        }

        [Fact]
        public void IsTimestampInRange_WhenTimestampBeforeStart_ShouldReturnFalse()
        {
            Assert.False(TimerHelper.IsTimestampInRange(99L, 100L, 200L));
        }

        [Fact]
        public void IsTimestampInRange_WhenTimestampAfterEnd_ShouldReturnFalse()
        {
            Assert.False(TimerHelper.IsTimestampInRange(201L, 100L, 200L));
        }

        [Theory]
        [InlineData(-5L, -10L, 10L, true)]   // 负区间内
        [InlineData(-10L, -10L, 10L, true)]  // 负边界
        [InlineData(-11L, -10L, 10L, false)] // 负区间外
        [InlineData(0L, 0L, 0L, true)]       // 零点
        [InlineData(long.MaxValue, long.MaxValue, long.MaxValue, true)]
        [InlineData(long.MinValue, long.MinValue, long.MaxValue, true)]
        public void IsTimestampInRange_BoundaryCases(long timestamp, long start, long end, bool expected)
        {
            Assert.Equal(expected, TimerHelper.IsTimestampInRange(timestamp, start, end));
        }

        #endregion
    }
}
