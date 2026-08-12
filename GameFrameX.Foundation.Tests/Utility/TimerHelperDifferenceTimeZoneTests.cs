using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Difference.TimeZone.cs / Difference.UTC.cs 单元测试：
    /// 覆盖 GetTimeDifferenceWithTimeZone、GetTimeDifferenceMillisecondsWithTimeZone、
    /// GetTimeDifferenceFromNowWithTimeZone（含 DateTime/long 重载）、GetTimeDifferenceFromNowMillisecondsWithTimeZone、
    /// GetElapsedSecondsWithTimeZone(DateTime)、GetElapsedSecondsWithUtc / GetElapsedMillisecondsWithUtc。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperDifferenceTimeZoneTests : IDisposable
    {
        private static readonly DateTime FixedUtcNow =
            new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

        private sealed class FakeTimeProvider : TimeProvider
        {
            private readonly DateTimeOffset _utcNow;

            public FakeTimeProvider(DateTimeOffset utcNow)
            {
                _utcNow = utcNow;
            }

            public override DateTimeOffset GetUtcNow()
            {
                return _utcNow;
            }
        }

        public TimerHelperDifferenceTimeZoneTests()
        {
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
            TimerHelper.SetTimeProvider(new FakeTimeProvider(new DateTimeOffset(FixedUtcNow)));
        }

        public void Dispose()
        {
            TimerHelper.SetTimeProvider(null);
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
            TimerHelper.ResetTimeOffset();
        }

        #region GetTimeDifferenceWithTimeZone

        [Fact]
        public void GetTimeDifferenceWithTimeZone_ShouldReturnDifference()
        {
            const long start = 1718447400L; // 2024-06-15 10:30:00 UTC
            const long end = 1718447460L;   // +60s

            Assert.Equal(TimeSpan.FromSeconds(60),
                TimerHelper.GetTimeDifferenceWithTimeZone(start, end));
        }

        [Fact]
        public void GetTimeDifferenceWithTimeZone_WhenNegative_ShouldReturnNegative()
        {
            const long start = 1718447460L;
            const long end = 1718447400L;

            Assert.Equal(TimeSpan.FromSeconds(-60),
                TimerHelper.GetTimeDifferenceWithTimeZone(start, end));
        }

        [Fact]
        public void GetTimeDifferenceWithTimeZone_WhenEqual_ShouldReturnZero()
        {
            Assert.Equal(TimeSpan.Zero,
                TimerHelper.GetTimeDifferenceWithTimeZone(1000L, 1000L));
        }

        #endregion

        #region GetTimeDifferenceMillisecondsWithTimeZone

        [Fact]
        public void GetTimeDifferenceMillisecondWithTimeZone_ShouldReturnDifference()
        {
            Assert.Equal(TimeSpan.FromMilliseconds(500),
                TimerHelper.GetTimeDifferenceMillisecondsWithTimeZone(1000L, 1500L));
        }

        [Fact]
        public void GetTimeDifferenceMillisecondWithTimeZone_WhenNegative_ShouldReturnNegative()
        {
            Assert.Equal(TimeSpan.FromMilliseconds(-500),
                TimerHelper.GetTimeDifferenceMillisecondsWithTimeZone(1500L, 1000L));
        }

        #endregion

        #region GetTimeDifferenceFromNowWithTimeZone

        [Fact]
        public void GetTimeDifferenceFromNowWithTimeZone_WithDateTime_ShouldUseFixedNow()
        {
            // FixedUtcNow=2024-06-15 10:30:45; CurrentTimeZone=UTC → now=10:30:45
            var past = new DateTime(2024, 6, 15, 9, 30, 45);

            Assert.Equal(TimeSpan.FromHours(1),
                TimerHelper.GetTimeDifferenceFromNowWithTimeZone(past));
        }

        [Fact]
        public void GetTimeDifferenceFromNowWithTimeZone_FutureTime_ShouldReturnNegative()
        {
            var future = new DateTime(2024, 6, 15, 11, 30, 45);

            Assert.Equal(TimeSpan.FromHours(-1),
                TimerHelper.GetTimeDifferenceFromNowWithTimeZone(future));
        }

        [Fact]
        public void GetTimeDifferenceFromNowWithTimeZone_WithTimestamp_ShouldMatchDateTime()
        {
            var past = new DateTime(2024, 6, 15, 9, 30, 45, DateTimeKind.Utc);
            var ts = new DateTimeOffset(past).ToUnixTimeSeconds();

            Assert.Equal(TimerHelper.GetTimeDifferenceFromNowWithTimeZone(past),
                TimerHelper.GetTimeDifferenceFromNowWithTimeZone(ts));
        }

        #endregion

        #region GetTimeDifferenceFromNowMillisecondsWithTimeZone

        [Fact]
        public void GetTimeDifferenceFromNowMsWithTimeZone_ShouldUseFixedNow()
        {
            var pastMs = new DateTimeOffset(2024, 6, 15, 9, 30, 45, TimeSpan.Zero).ToUnixTimeMilliseconds();

            Assert.Equal(TimeSpan.FromHours(1),
                TimerHelper.GetTimeDifferenceFromNowMillisecondsWithTimeZone(pastMs));
        }

        #endregion

        #region GetElapsedSecondsWithTimeZone(DateTime)

        [Fact]
        public void GetElapsedSecondsWithTimeZone_WithDateTime_ShouldReturnSeconds()
        {
            var past = new DateTime(2024, 6, 15, 9, 30, 45);

            Assert.Equal(3600L, TimerHelper.GetElapsedSecondsWithTimeZone(past));
        }

        [Fact]
        public void GetElapsedSecondsWithTimeZone_FutureTime_ShouldReturnNegative()
        {
            var future = new DateTime(2024, 6, 15, 11, 30, 45);

            Assert.Equal(-3600L, TimerHelper.GetElapsedSecondsWithTimeZone(future));
        }

        #endregion

        #region GetElapsedSecondsWithUtc / GetElapsedMillisecondsWithUtc

        [Fact]
        public void GetElapsedSecondsWithUtc_ShouldReturnElapsedSeconds()
        {
            var pastTs = new DateTimeOffset(2024, 6, 15, 9, 30, 45, TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(3600L, TimerHelper.GetElapsedSecondsWithUtc(pastTs));
        }

        [Fact]
        public void GetElapsedSecondsWithUtc_FutureTimestamp_ShouldReturnNegative()
        {
            var futureTs = new DateTimeOffset(2024, 6, 15, 11, 30, 45, TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(-3600L, TimerHelper.GetElapsedSecondsWithUtc(futureTs));
        }

        [Fact]
        public void GetElapsedMillisecondsWithUtc_ShouldReturnElapsedMilliseconds()
        {
            var pastMs = new DateTimeOffset(2024, 6, 15, 9, 30, 45, TimeSpan.Zero).ToUnixTimeMilliseconds();

            Assert.Equal(3600000L, TimerHelper.GetElapsedMillisecondsWithUtc(pastMs));
        }

        [Fact]
        public void GetElapsedMillisecondsWithUtc_FutureTimestamp_ShouldReturnNegative()
        {
            var futureMs = new DateTimeOffset(2024, 6, 15, 11, 30, 45, TimeSpan.Zero).ToUnixTimeMilliseconds();

            Assert.Equal(-3600000L, TimerHelper.GetElapsedMillisecondsWithUtc(futureMs));
        }

        #endregion
    }
}
