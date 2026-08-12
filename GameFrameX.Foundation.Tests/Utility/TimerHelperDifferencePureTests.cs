using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Difference.cs 基础（纯计算）方法单元测试：
    /// 覆盖 GetTimeDifference、GetSecondsDifference、GetMillisecondsDifference、GetMinutesDifference、
    /// GetHoursDifference、GetAbsoluteSecondsDifference、GetAbsoluteMillisecondsDifference、
    /// 以及时间戳重载。这些方法不依赖 TimeProvider/TimeZone，是纯函数。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperDifferencePureTests
    {
        #region GetTimeDifference

        [Fact]
        public void GetTimeDifference_ShouldReturnEndTimeMinusStartTime()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 13, 30, 45);

            Assert.Equal(TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(30)).Add(TimeSpan.FromSeconds(45)),
                TimerHelper.GetTimeDifference(start, end));
        }

        [Fact]
        public void GetTimeDifference_WhenEndBeforeStart_ShouldReturnNegative()
        {
            var start = new DateTime(2024, 1, 1, 13, 30, 45);
            var end = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(TimeSpan.FromHours(-1).Add(TimeSpan.FromMinutes(-30)).Add(TimeSpan.FromSeconds(-45)),
                TimerHelper.GetTimeDifference(start, end));
        }

        #endregion

        #region GetSecondsDifference(DateTime, DateTime)

        [Fact]
        public void GetSecondsDifference_ShouldTruncateFractionalSeconds()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 12, 0, 1, 500);

            // 1.5 秒被截断为 1
            Assert.Equal(1, TimerHelper.GetSecondsDifference(start, end));
        }

        [Fact]
        public void GetSecondsDifference_WhenEndBeforeStart_ShouldReturnNegative()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 1, 500);
            var end = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(-1, TimerHelper.GetSecondsDifference(start, end));
        }

        [Fact]
        public void GetSecondsDifference_WhenEqual_ShouldReturnZero()
        {
            var date = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(0, TimerHelper.GetSecondsDifference(date, date));
        }

        #endregion

        #region GetMillisecondsDifference(DateTime, DateTime)

        [Fact]
        public void GetMillisecondsDifference_ShouldReturnExactMilliseconds()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 12, 0, 1, 500);

            Assert.Equal(1500, TimerHelper.GetMillisecondsDifference(start, end));
        }

        [Fact]
        public void GetMillisecondsDifference_WhenEndBeforeStart_ShouldReturnNegative()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 1, 500);
            var end = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(-1500, TimerHelper.GetMillisecondsDifference(start, end));
        }

        #endregion

        #region GetMinutesDifference

        [Fact]
        public void GetMinutesDifference_ShouldPreserveFractionalPart()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 12, 30, 0);

            Assert.Equal(30d, TimerHelper.GetMinutesDifference(start, end));
        }

        [Fact]
        public void GetMinutesDifference_WhenHalfMinute_ShouldReturnHalfMinute()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 12, 0, 30);

            Assert.Equal(0.5d, TimerHelper.GetMinutesDifference(start, end));
        }

        [Fact]
        public void GetMinutesDifference_WhenNegative_ShouldReturnNegative()
        {
            var start = new DateTime(2024, 1, 1, 12, 30, 0);
            var end = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(-30d, TimerHelper.GetMinutesDifference(start, end));
        }

        #endregion

        #region GetHoursDifference

        [Fact]
        public void GetHoursDifference_ShouldPreserveFractionalPart()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 15, 0, 0);

            Assert.Equal(3d, TimerHelper.GetHoursDifference(start, end));
        }

        [Fact]
        public void GetHoursDifference_When90Minutes_ShouldReturnOneAndHalf()
        {
            var start = new DateTime(2024, 1, 1, 12, 0, 0);
            var end = new DateTime(2024, 1, 1, 13, 30, 0);

            Assert.Equal(1.5d, TimerHelper.GetHoursDifference(start, end));
        }

        #endregion

        #region GetSecondsDifference(long, long)

        [Fact]
        public void GetSecondsDifference_TimestampOverload_ShouldReturnDifference()
        {
            Assert.Equal(60L, TimerHelper.GetSecondsDifference(1000L, 1060L));
        }

        [Fact]
        public void GetSecondsDifference_TimestampOverload_WhenNegative_ShouldReturnNegative()
        {
            Assert.Equal(-60L, TimerHelper.GetSecondsDifference(1060L, 1000L));
        }

        [Theory]
        [InlineData(long.MinValue, long.MinValue, 0L)]
        [InlineData(0L, long.MaxValue, long.MaxValue)]
        public void GetSecondsDifference_TimestampOverload_ExtremeValues(long a, long b, long expected)
        {
            Assert.Equal(expected, TimerHelper.GetSecondsDifference(a, b));
        }

        #endregion

        #region GetMillisecondsDifference(long, long)

        [Fact]
        public void GetMillisecondsDifference_TimestampOverload_ShouldReturnDifference()
        {
            Assert.Equal(1000L, TimerHelper.GetMillisecondsDifference(2000L, 3000L));
        }

        [Fact]
        public void GetMillisecondsDifference_TimestampOverload_WhenNegative_ShouldReturnNegative()
        {
            Assert.Equal(-1000L, TimerHelper.GetMillisecondsDifference(3000L, 2000L));
        }

        #endregion

        #region GetAbsoluteSecondsDifference / GetAbsoluteMillisecondsDifference

        [Fact]
        public void GetAbsoluteSecondsDifference_ShouldAlwaysBeNonNegative()
        {
            var a = new DateTime(2024, 1, 1, 12, 0, 0);
            var b = new DateTime(2024, 1, 1, 12, 0, 5);

            Assert.Equal(5, TimerHelper.GetAbsoluteSecondsDifference(a, b));
            Assert.Equal(5, TimerHelper.GetAbsoluteSecondsDifference(b, a));
        }

        [Fact]
        public void GetAbsoluteSecondsDifference_WhenEqual_ShouldReturnZero()
        {
            var date = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(0, TimerHelper.GetAbsoluteSecondsDifference(date, date));
        }

        [Fact]
        public void GetAbsoluteMillisecondsDifference_ShouldAlwaysBeNonNegative()
        {
            var a = new DateTime(2024, 1, 1, 12, 0, 0);
            var b = new DateTime(2024, 1, 1, 12, 0, 0, 500);

            Assert.Equal(500, TimerHelper.GetAbsoluteMillisecondsDifference(a, b));
            Assert.Equal(500, TimerHelper.GetAbsoluteMillisecondsDifference(b, a));
        }

        [Fact]
        public void GetAbsoluteMillisecondsDifference_WhenEqual_ShouldReturnZero()
        {
            var date = new DateTime(2024, 1, 1, 12, 0, 0);

            Assert.Equal(0, TimerHelper.GetAbsoluteMillisecondsDifference(date, date));
        }

        #endregion
    }
}
