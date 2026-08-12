using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Day.cs / Day.UTC.cs / Day.TimeZone.cs 单元测试：
    /// 覆盖 IsSameDay、GetDaysDifference、GetCrossDays（含 hour 阈值与越界）、
    /// GetStartTimeOfDay/GetEndTimeOfDay 及其时间戳重载、CurrentDateWithUtcDay/WithTimeZone、
    /// GetToday/GetTomorrow 系列 UTC 与 TimeZone 成对方法。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperDayAdditionalTests : IDisposable
    {
        private static readonly TimeZoneInfo UtcPlus8 =
            TimeZoneInfo.CreateCustomTimeZone("Test+8", TimeSpan.FromHours(8), "Test+8", "Test+8");

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

        public TimerHelperDayAdditionalTests()
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

        #region IsSameDay

        [Fact]
        public void IsSameDay_SameDate_ShouldReturnTrue()
        {
            var morning = new DateTime(2024, 1, 10, 8, 30, 0);
            var evening = new DateTime(2024, 1, 10, 23, 59, 59);

            Assert.True(TimerHelper.IsSameDay(morning, evening));
        }

        [Fact]
        public void IsSameDay_DifferentDate_ShouldReturnFalse()
        {
            var today = new DateTime(2024, 1, 10, 23, 59, 59);
            var tomorrow = new DateTime(2024, 1, 11, 0, 0, 0);

            Assert.False(TimerHelper.IsSameDay(today, tomorrow));
        }

        [Fact]
        public void IsSameDay_DifferentYear_ShouldReturnFalse()
        {
            var a = new DateTime(2023, 12, 31, 23, 59, 59);
            var b = new DateTime(2024, 1, 1, 0, 0, 0);

            Assert.False(TimerHelper.IsSameDay(a, b));
        }

        [Fact]
        public void IsSameDay_SameReferenceDate_ShouldReturnTrue()
        {
            var date = new DateTime(2024, 6, 15, 12, 0, 0);

            Assert.True(TimerHelper.IsSameDay(date, date));
        }

        #endregion

        #region GetDaysDifference

        [Fact]
        public void GetDaysDifference_ShouldIncludeDecimalPart()
        {
            var start = new DateTime(2024, 1, 10, 8, 0, 0);
            var end = new DateTime(2024, 1, 11, 20, 0, 0);

            Assert.Equal(1.5, TimerHelper.GetDaysDifference(start, end));
        }

        [Fact]
        public void GetDaysDifference_WhenEndBeforeStart_ShouldReturnNegative()
        {
            var start = new DateTime(2024, 1, 11, 20, 0, 0);
            var end = new DateTime(2024, 1, 10, 8, 0, 0);

            Assert.Equal(-1.5, TimerHelper.GetDaysDifference(start, end));
        }

        [Fact]
        public void GetDaysDifference_WhenEqual_ShouldReturnZero()
        {
            var date = new DateTime(2024, 1, 10, 12, 0, 0);

            Assert.Equal(0d, TimerHelper.GetDaysDifference(date, date));
        }

        #endregion

        #region GetCrossDays

        [Fact]
        public void GetCrossDays_DefaultHour_ShouldReturnCalendarDayDifference()
        {
            var start = new DateTime(2024, 1, 10, 12, 0, 0);
            var end = new DateTime(2024, 1, 13, 12, 0, 0);

            Assert.Equal(3, TimerHelper.GetCrossDays(start, end));
        }

        [Fact]
        public void GetCrossDays_WhenEndTimeBeforeStart_ShouldReturnNegative()
        {
            var start = new DateTime(2024, 1, 13, 12, 0, 0);
            var end = new DateTime(2024, 1, 10, 12, 0, 0);

            Assert.Equal(-3, TimerHelper.GetCrossDays(start, end));
        }

        [Fact]
        public void GetCrossDays_WithHourThreshold_ShouldAdjustByStartHour()
        {
            // hour=5：start.Hour=3 < 5 → days++；end.Hour=4 < 5 → days--。
            // 日期差=1。净效果：1 + 1 - 1 = 1
            var start = new DateTime(2024, 1, 10, 3, 0, 0);
            var end = new DateTime(2024, 1, 11, 4, 0, 0);

            Assert.Equal(1, TimerHelper.GetCrossDays(start, end, 5));
        }

        [Fact]
        public void GetCrossDays_WithHourThreshold_ShouldAdjustByEndHourOnly()
        {
            // start.Hour=8 >= 5（不+1）; end.Hour=4 < 5（-1）。日期差1。净效果 0
            var start = new DateTime(2024, 1, 10, 8, 0, 0);
            var end = new DateTime(2024, 1, 11, 4, 0, 0);

            Assert.Equal(0, TimerHelper.GetCrossDays(start, end, 5));
        }

        [Fact]
        public void GetCrossDays_WithHourThreshold_ShouldAdjustByStartHourOnly()
        {
            // start.Hour=3 < 5 → days++; end.Hour=8 >= 5（不-1）。日期差1。净效果 2
            var start = new DateTime(2024, 1, 10, 3, 0, 0);
            var end = new DateTime(2024, 1, 11, 8, 0, 0);

            Assert.Equal(2, TimerHelper.GetCrossDays(start, end, 5));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(24)]
        [InlineData(-100)]
        [InlineData(100)]
        public void GetCrossDays_WhenHourOutOfRange_ShouldThrowArgumentOutOfRangeException(int hour)
        {
            var start = new DateTime(2024, 1, 10);
            var end = new DateTime(2024, 1, 11);

            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.GetCrossDays(start, end, hour));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(23)]
        public void GetCrossDays_WhenHourAtBoundary_ShouldNotThrow(int hour)
        {
            var start = new DateTime(2024, 1, 10, 12, 0, 0);
            var end = new DateTime(2024, 1, 11, 12, 0, 0);

            var exception = Record.Exception(() => TimerHelper.GetCrossDays(start, end, hour));

            Assert.Null(exception);
        }

        #endregion

        #region GetStartTimeOfDay / GetEndTimeOfDay

        [Fact]
        public void GetStartTimeOfDay_ShouldReturnMidnight()
        {
            var date = new DateTime(2024, 2, 29, 14, 30, 45);

            Assert.Equal(new DateTime(2024, 2, 29, 0, 0, 0), TimerHelper.GetStartTimeOfDay(date));
        }

        [Fact]
        public void GetEndTimeOfDay_ShouldReturnLastSecondOfSameDay()
        {
            var date = new DateTime(2024, 2, 29, 14, 30, 45);

            Assert.Equal(new DateTime(2024, 2, 29, 23, 59, 59), TimerHelper.GetEndTimeOfDay(date));
        }

        [Fact]
        public void GetEndTimeOfDay_ShouldBeOneSecondBeforeNextDayMidnight()
        {
            var date = new DateTime(2024, 12, 31, 0, 0, 0);

            Assert.Equal(TimerHelper.GetStartTimeOfDay(date).AddDays(1).AddSeconds(-1),
                TimerHelper.GetEndTimeOfDay(date));
        }

        #endregion

        #region GetStartTimestampOfDay / GetEndTimestampOfDay (UTC base)

        [Fact]
        public void GetStartTimestampOfDay_WithUtcKind_ShouldMatchEpochDelta()
        {
            // DateTimeToUnixTimeSeconds：Utc Kind 用 0 offset
            var date = new DateTime(2024, 1, 10, 14, 0, 0, DateTimeKind.Utc);

            var expected = new DateTimeOffset(new DateTime(2024, 1, 10, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetStartTimestampOfDay(date));
        }

        [Fact]
        public void GetEndTimestampOfDay_WithUtcKind_ShouldMatchEpochDelta()
        {
            var date = new DateTime(2024, 1, 10, 14, 0, 0, DateTimeKind.Utc);

            var expected = new DateTimeOffset(new DateTime(2024, 1, 10, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetEndTimestampOfDay(date));
        }

        #endregion

        #region GetStartTimestampOfDayWithTimeZone / GetEndTimestampOfDayWithTimeZone

        [Fact]
        public void GetStartTimestampOfDayWithTimeZone_WithUtcZone_ShouldEqualUtcVersion()
        {
            // CurrentTimeZone=UTC，与 GetStartTimestampOfDay(Utc input) 数值应一致
            var date = new DateTime(2024, 1, 10, 14, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfDay(date),
                TimerHelper.GetStartTimestampOfDayWithTimeZone(date));
        }

        [Fact]
        public void GetEndTimestampOfDayWithTimeZone_WithUtcZone_ShouldEqualUtcVersion()
        {
            var date = new DateTime(2024, 1, 10, 14, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetEndTimestampOfDay(date),
                TimerHelper.GetEndTimestampOfDayWithTimeZone(date));
        }

        [Fact]
        public void GetStartTimestampOfDayWithTimeZone_WithPlus8_ShouldBeAheadOfUtcBy8Hours()
        {
            TimerHelper.SetTimeZone(UtcPlus8);
            var date = new DateTime(2024, 1, 10, 14, 0, 0, DateTimeKind.Utc);

            var withUtc = TimerHelper.GetStartTimestampOfDay(date);
            var withZone = TimerHelper.GetStartTimestampOfDayWithTimeZone(date);

            // 时间戳偏移 = +8 小时（28800 秒）
            Assert.Equal(withUtc + 28800L, withZone);
        }

        #endregion

        #region CurrentDateWithUtcDay / CurrentDateWithDayWithTimeZone

        [Fact]
        public void CurrentDateWithUtcDay_ShouldReturnFormattedInt()
        {
            // FakeTimeProvider 固定 UTC=2024-06-15 10:30:45
            Assert.Equal(20240615, TimerHelper.CurrentDateWithUtcDay());
        }

        [Fact]
        public void CurrentDateWithDayWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.CurrentDateWithUtcDay(), TimerHelper.CurrentDateWithDayWithTimeZone());
        }

        [Fact]
        public void CurrentDateWithDayWithTimeZone_WithPlus8_ShouldStillBeSameDay()
        {
            // 2024-06-15 10:30 UTC + 8h = 2024-06-15 18:30 → 同日
            TimerHelper.SetTimeZone(UtcPlus8);

            Assert.Equal(20240615, TimerHelper.CurrentDateWithDayWithTimeZone());
        }

        [Fact]
        public void CurrentDateWithDayWithTimeZone_WithPlus8_AtUtcLateHour_ShouldRollToNextDay()
        {
            // 2024-06-15 16:00 UTC + 8h = 2024-06-16 00:00 → 滚到次日
            TimerHelper.SetTimeZone(UtcPlus8);
            TimerHelper.SetTimeProvider(new FakeTimeProvider(new DateTimeOffset(2024, 6, 15, 16, 0, 0, TimeSpan.Zero)));

            Assert.Equal(20240616, TimerHelper.CurrentDateWithDayWithTimeZone());
        }

        #endregion

        #region GetCrossDaysUtc / GetCrossDaysWithUtc

        [Fact]
        public void GetCrossDaysUtc_ShouldConvertTimestampsAndDelegate()
        {
            // 2024-01-10 12:00 UTC 与 2024-01-11 12:00 UTC
            const long begin = 1704894400L;   // 大致 2024-01-10 12:00 UTC
            const long after = 1704980800L;   // 大致 2024-01-11 12:00 UTC

            var beginDate = TimerHelper.TimestampSecondToDateTime(begin, true);
            var afterDate = TimerHelper.TimestampSecondToDateTime(after, true);
            var expected = TimerHelper.GetCrossDays(beginDate, afterDate, 0);

            Assert.Equal(expected, TimerHelper.GetCrossDaysUtc(begin, after, 0));
        }

        [Fact]
        public void GetCrossDaysWithUtc_FromDateTime_ShouldUseCurrentUtcNow()
        {
            // 固定 UTC = 2024-06-15 10:30:45
            var startTime = new DateTime(2024, 6, 13, 0, 0, 0);

            Assert.Equal(2, TimerHelper.GetCrossDaysWithUtc(startTime, 0));
        }

        [Fact]
        public void GetCrossDaysWithUtc_FromSingleTimestamp_ShouldUseCurrentUtcNow()
        {
            // 2024-06-13 00:00:00 UTC 秒级时间戳
            var ts = new DateTimeOffset(2024, 6, 13, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(2, TimerHelper.GetCrossDaysWithUtc(ts, 0));
        }

        #endregion

        #region GetCrossDaysWithTimeZone

        [Fact]
        public void GetCrossDaysWithTimeZone_FromTwoTimestamps_ShouldUseCurrentTimeZone()
        {
            TimerHelper.SetTimeZone(UtcPlus8);
            var start = new DateTimeOffset(2024, 6, 13, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();
            var end = new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

            // 时间戳转回 DateTime 后调用 GetCrossDays（小时默认 0）
            var startDate = TimerHelper.TimestampSecondToDateTime(start);
            var endDate = TimerHelper.TimestampSecondToDateTime(end);
            var expected = TimerHelper.GetCrossDays(startDate, endDate, 0);

            Assert.Equal(expected, TimerHelper.GetCrossDaysWithTimeZone(start, end, 0));
        }

        [Fact]
        public void GetCrossDaysWithTimeZone_FromDateTime_ShouldUseCurrentZoneNow()
        {
            // 固定 UTC = 2024-06-15 10:30:45 → UTC 下跨 2 天
            var startTime = new DateTime(2024, 6, 13, 0, 0, 0);

            Assert.Equal(2, TimerHelper.GetCrossDaysWithTimeZone(startTime, 0));
        }

        [Fact]
        public void GetCrossDaysWithTimeZone_FromSingleTimestamp_ShouldUseCurrentZoneNow()
        {
            var ts = new DateTimeOffset(2024, 6, 13, 0, 0, 0, TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(2, TimerHelper.GetCrossDaysWithTimeZone(ts, 0));
        }

        #endregion

        #region GetToday/GetTomorrow UTC

        [Fact]
        public void GetTodayStartTimeWithUtc_ShouldReturnMidnightOfFixedDate()
        {
            Assert.Equal(new DateTime(2024, 6, 15, 0, 0, 0), TimerHelper.GetTodayStartTimeWithUtc());
        }

        [Fact]
        public void GetTodayEndTimeWithUtc_ShouldReturnLastSecondOfFixedDate()
        {
            Assert.Equal(new DateTime(2024, 6, 15, 23, 59, 59), TimerHelper.GetTodayEndTimeWithUtc());
        }

        [Fact]
        public void GetTodayEndTimestampWithUtc_ShouldBeStartPlusOneDayMinusOneSecond()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 15, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetTodayEndTimestampWithUtc());
        }

        [Fact]
        public void GetTodayStartTimestampWithUtc_ShouldMatchMidnight()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 15, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetTodayStartTimestampWithUtc());
        }

        [Fact]
        public void GetTomorrowStartTimeWithUtc_ShouldBeNextDayMidnight()
        {
            Assert.Equal(new DateTime(2024, 6, 16, 0, 0, 0), TimerHelper.GetTomorrowStartTimeWithUtc());
        }

        [Fact]
        public void GetTomorrowEndTimeWithUtc_ShouldBeNextDayLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 16, 23, 59, 59), TimerHelper.GetTomorrowEndTimeWithUtc());
        }

        [Fact]
        public void GetTomorrowStartTimestampWithUtc_ShouldMatchTomorrowMidnight()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 16, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetTomorrowStartTimestampWithUtc());
        }

        [Fact]
        public void GetTomorrowEndTimestampWithUtc_ShouldMatchTomorrowLastSecond()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 16, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetTomorrowEndTimestampWithUtc());
        }

        #endregion

        #region GetToday/GetTomorrow TimeZone

        [Fact]
        public void GetTodayStartTimeWithTimeZone_WithUtc_ShouldReturnMidnight()
        {
            Assert.Equal(new DateTime(2024, 6, 15, 0, 0, 0), TimerHelper.GetTodayStartTimeWithTimeZone());
        }

        [Fact]
        public void GetTodayEndTimeWithTimeZone_WithUtc_ShouldReturnLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 15, 23, 59, 59), TimerHelper.GetTodayEndTimeWithTimeZone());
        }

        [Fact]
        public void GetTodayEndTimeWithTimeZone_ShouldBeOneSecondBeforeTomorrowMidnight()
        {
            Assert.Equal(TimerHelper.GetTodayStartTimeWithTimeZone().AddDays(1).AddSeconds(-1),
                TimerHelper.GetTodayEndTimeWithTimeZone());
        }

        [Fact]
        public void GetTomorrowStartTimeWithTimeZone_ShouldBeNextDayMidnight()
        {
            Assert.Equal(new DateTime(2024, 6, 16, 0, 0, 0), TimerHelper.GetTomorrowStartTimeWithTimeZone());
        }

        [Fact]
        public void GetTomorrowEndTimeWithTimeZone_ShouldBeNextDayLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 16, 23, 59, 59), TimerHelper.GetTomorrowEndTimeWithTimeZone());
        }

        [Fact]
        public void GetTodayStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetTodayStartTimestampWithUtc(),
                TimerHelper.GetTodayStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetTodayEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetTodayEndTimestampWithUtc(),
                TimerHelper.GetTodayEndTimestampWithTimeZone());
        }

        [Fact]
        public void GetTomorrowStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetTomorrowStartTimestampWithUtc(),
                TimerHelper.GetTomorrowStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetTomorrowEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetTomorrowEndTimestampWithUtc(),
                TimerHelper.GetTomorrowEndTimestampWithTimeZone());
        }

        #endregion
    }
}
