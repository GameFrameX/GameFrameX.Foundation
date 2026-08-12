using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Month.cs / Month.UTC.cs / Month.TimeZone.cs 单元测试：
    /// 覆盖 GetStartTimeOfMonth/GetEndTimeOfMonth（含闰年/跨年/月末）及其时间戳重载，
    /// 以及 GetMonth/GetNextMonth 系列 UTC 与 TimeZone 成对方法。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperMonthAdditionalTests : IDisposable
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

        public TimerHelperMonthAdditionalTests()
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

        #region GetStartTimeOfMonth / GetEndTimeOfMonth

        [Fact]
        public void GetStartTimeOfMonth_ShouldReturnFirstDayMidnight()
        {
            var date = new DateTime(2024, 2, 29, 14, 30, 0);

            Assert.Equal(new DateTime(2024, 2, 1, 0, 0, 0), TimerHelper.GetStartTimeOfMonth(date));
        }

        [Fact]
        public void GetEndTimeOfMonth_NonLeapFebruary_ShouldReturn28thLastSecond()
        {
            var date = new DateTime(2023, 2, 15, 0, 0, 0);

            Assert.Equal(new DateTime(2023, 2, 28, 23, 59, 59), TimerHelper.GetEndTimeOfMonth(date));
        }

        [Fact]
        public void GetEndTimeOfMonth_LeapFebruary_ShouldReturn29thLastSecond()
        {
            var date = new DateTime(2024, 2, 15, 0, 0, 0);

            Assert.Equal(new DateTime(2024, 2, 29, 23, 59, 59), TimerHelper.GetEndTimeOfMonth(date));
        }

        [Fact]
        public void GetEndTimeOfMonth_December_ShouldReturn31stLastSecond()
        {
            var date = new DateTime(2024, 12, 1, 0, 0, 0);

            Assert.Equal(new DateTime(2024, 12, 31, 23, 59, 59), TimerHelper.GetEndTimeOfMonth(date));
        }

        [Theory]
        [InlineData(2024, 1, 31)]
        [InlineData(2024, 3, 31)]
        [InlineData(2024, 4, 30)]
        [InlineData(2024, 5, 31)]
        [InlineData(2024, 6, 30)]
        [InlineData(2024, 7, 31)]
        [InlineData(2024, 8, 31)]
        [InlineData(2024, 9, 30)]
        [InlineData(2024, 10, 31)]
        [InlineData(2024, 11, 30)]
        [InlineData(2024, 12, 31)]
        public void GetEndTimeOfMonth_ShouldReturnLastDayOfEachMonth(int year, int month, int lastDay)
        {
            var date = new DateTime(year, month, 15);

            Assert.Equal(new DateTime(year, month, lastDay, 23, 59, 59),
                TimerHelper.GetEndTimeOfMonth(date));
        }

        [Fact]
        public void GetEndTimeOfMonth_ShouldBeOneSecondBeforeNextMonthFirstDay()
        {
            var date = new DateTime(2024, 1, 15);

            Assert.Equal(TimerHelper.GetStartTimeOfMonth(date).AddMonths(1).AddSeconds(-1),
                TimerHelper.GetEndTimeOfMonth(date));
        }

        #endregion

        #region GetStartTimestampOfMonth / GetEndTimestampOfMonth

        [Fact]
        public void GetStartTimestampOfMonth_WithUtcKind_ShouldMatchEpochDelta()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var expected = new DateTimeOffset(new DateTime(2024, 6, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetStartTimestampOfMonth(date));
        }

        [Fact]
        public void GetEndTimestampOfMonth_WithUtcKind_ShouldMatchEpochDelta()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var expected = new DateTimeOffset(new DateTime(2024, 6, 30, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetEndTimestampOfMonth(date));
        }

        #endregion

        #region GetStartTimestampOfMonthWithTimeZone / GetEndTimestampOfMonthWithTimeZone

        [Fact]
        public void GetStartTimestampOfMonthWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfMonth(date),
                TimerHelper.GetStartTimestampOfMonthWithTimeZone(date));
        }

        [Fact]
        public void GetEndTimestampOfMonthWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetEndTimestampOfMonth(date),
                TimerHelper.GetEndTimestampOfMonthWithTimeZone(date));
        }

        [Fact]
        public void GetStartTimestampOfMonthWithTimeZone_WithPlus8_ShouldBeAheadOfUtc()
        {
            var zone = TimeZoneInfo.CreateCustomTimeZone("Test+8", TimeSpan.FromHours(8), "Test+8", "Test+8");
            TimerHelper.SetTimeZone(zone);
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfMonth(date) + 28800L,
                TimerHelper.GetStartTimestampOfMonthWithTimeZone(date));
        }

        #endregion

        #region GetMonth/GetNextMonth WithUtc

        [Fact]
        public void GetMonthStartTimeWithUtc_ShouldReturnFirstOfJune()
        {
            Assert.Equal(new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                TimerHelper.GetMonthStartTimeWithUtc());
        }

        [Fact]
        public void GetMonthEndTimeWithUtc_ShouldReturnLastSecondOfJune()
        {
            Assert.Equal(new DateTime(2024, 6, 30, 23, 59, 59, DateTimeKind.Utc),
                TimerHelper.GetMonthEndTimeWithUtc());
        }

        [Fact]
        public void GetMonthStartTimestampWithUtc_ShouldMatchMonthStart()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetMonthStartTimestampWithUtc());
        }

        [Fact]
        public void GetMonthEndTimestampWithUtc_ShouldMatchMonthEnd()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 30, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetMonthEndTimestampWithUtc());
        }

        [Fact]
        public void GetNextMonthStartTimeWithUtc_ShouldReturnFirstOfJuly()
        {
            Assert.Equal(new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                TimerHelper.GetNextMonthStartTimeWithUtc());
        }

        [Fact]
        public void GetNextMonthEndTimeWithUtc_ShouldReturnLastSecondOfJuly()
        {
            Assert.Equal(new DateTime(2024, 7, 31, 23, 59, 59, DateTimeKind.Utc),
                TimerHelper.GetNextMonthEndTimeWithUtc());
        }

        [Fact]
        public void GetNextMonthStartTimestampWithUtc_ShouldMatchNextMonthStart()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 7, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetNextMonthStartTimestampWithUtc());
        }

        [Fact]
        public void GetNextMonthEndTimestampWithUtc_ShouldMatchNextMonthEnd()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 7, 31, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetNextMonthEndTimestampWithUtc());
        }

        #endregion

        #region GetMonth/GetNextMonth WithTimeZone

        [Fact]
        public void GetMonthStartTimeWithTimeZone_WithUtc_ShouldReturnFirstOfMonth()
        {
            Assert.Equal(new DateTime(2024, 6, 1, 0, 0, 0), TimerHelper.GetMonthStartTimeWithTimeZone());
        }

        [Fact]
        public void GetMonthEndTimeWithTimeZone_WithUtc_ShouldReturnLastSecondOfMonth()
        {
            Assert.Equal(new DateTime(2024, 6, 30, 23, 59, 59), TimerHelper.GetMonthEndTimeWithTimeZone());
        }

        [Fact]
        public void GetNextMonthStartTimeWithTimeZone_ShouldBeOneMonthAhead()
        {
            Assert.Equal(TimerHelper.GetMonthStartTimeWithTimeZone().AddMonths(1),
                TimerHelper.GetNextMonthStartTimeWithTimeZone());
        }

        [Fact]
        public void GetNextMonthEndTimeWithTimeZone_ShouldBeOneMonthAhead()
        {
            Assert.Equal(TimerHelper.GetNextMonthStartTimeWithTimeZone().AddMonths(1).AddSeconds(-1),
                TimerHelper.GetNextMonthEndTimeWithTimeZone());
        }

        [Fact]
        public void GetMonthStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetMonthStartTimestampWithUtc(),
                TimerHelper.GetMonthStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetMonthEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetMonthEndTimestampWithUtc(),
                TimerHelper.GetMonthEndTimestampWithTimeZone());
        }

        [Fact]
        public void GetNextMonthStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetNextMonthStartTimestampWithUtc(),
                TimerHelper.GetNextMonthStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetNextMonthEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetNextMonthEndTimestampWithUtc(),
                TimerHelper.GetNextMonthEndTimestampWithTimeZone());
        }

        [Fact]
        public void GetMonthStartTimeWithTimeZone_December_ShouldCrossYearBoundary()
        {
            TimerHelper.SetTimeProvider(new FakeTimeProvider(new DateTimeOffset(2024, 12, 15, 0, 0, 0, TimeSpan.Zero)));

            Assert.Equal(new DateTime(2024, 12, 1, 0, 0, 0), TimerHelper.GetMonthStartTimeWithTimeZone());

            Assert.Equal(new DateTime(2025, 1, 1, 0, 0, 0), TimerHelper.GetNextMonthStartTimeWithTimeZone());
        }

        #endregion
    }
}
