using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Week.cs / Week.UTC.cs / Week.TimeZone.cs 单元测试：
    /// 覆盖 IsSameWeek（多重载）、GetDayOfWeekTime、GetStartTimeOfWeek/GetEndTimeOfWeek、
    /// GetWeek/GetNextWeek 系列 UTC 与 TimeZone 成对方法、GetChinaDayOfWeekWithTimeZone。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperWeekAdditionalTests : IDisposable
    {
        // 2024-06-15 是星期六（DayOfWeek.Saturday）
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

        public TimerHelperWeekAdditionalTests()
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

        #region IsSameWeek(DateTime, DateTime)

        [Fact]
        public void IsSameWeek_SameWeekDays_ShouldReturnTrue()
        {
            // 2024-06-10 (周一) ~ 2024-06-16 (周日) 同周
            var monday = new DateTime(2024, 6, 10);
            var sunday = new DateTime(2024, 6, 16);

            Assert.True(TimerHelper.IsSameWeek(monday, sunday));
        }

        [Fact]
        public void IsSameWeek_CrossWeekBoundary_ShouldReturnFalse()
        {
            var sunday = new DateTime(2024, 6, 16);
            var nextMonday = new DateTime(2024, 6, 17);

            Assert.False(TimerHelper.IsSameWeek(sunday, nextMonday));
        }

        [Fact]
        public void IsSameWeek_MondayAndPreviousSunday_ShouldReturnFalse()
        {
            // 周一属于本周，上周日属于上一周
            var sunday = new DateTime(2024, 6, 16);
            var monday = new DateTime(2024, 6, 10);

            // 6/10 是周一（本周），6/16 是周日（本周）；上一周的周日是 6/9
            var prevSunday = new DateTime(2024, 6, 9);

            Assert.False(TimerHelper.IsSameWeek(prevSunday, monday));
        }

        [Fact]
        public void IsSameWeek_WhenEqual_ShouldReturnTrue()
        {
            var date = new DateTime(2024, 6, 12);

            Assert.True(TimerHelper.IsSameWeek(date, date));
        }

        [Fact]
        public void IsSameWeek_CrossYearBoundary_ShouldReturnFalse()
        {
            // 2024-12-30 是周一，2025-01-05 是周日 → 不同年但跨年周
            var date1 = new DateTime(2024, 12, 30);
            var date2 = new DateTime(2025, 1, 5);

            // 两者在同一周内（2024-12-30 周一 到 2025-01-05 周日）
            Assert.True(TimerHelper.IsSameWeek(date1, date2));
        }

        #endregion

        #region IsSameWeek(DateTime, bool isUtc)

        [Fact]
        public void IsSameWeek_WithDateTime_SameWeek_ShouldReturnTrue()
        {
            // FixedUtcNow 是 2024-06-15（周六），与 2024-06-10（周一）同周
            Assert.True(TimerHelper.IsSameWeek(new DateTime(2024, 6, 10), true));
        }

        [Fact]
        public void IsSameWeek_WithDateTime_DifferentWeek_ShouldReturnFalse()
        {
            // 上一周
            Assert.False(TimerHelper.IsSameWeek(new DateTime(2024, 6, 1), true));
        }

        [Fact]
        public void IsSameWeek_WithDateTime_UsingTimeZone_ShouldReturnTrue()
        {
            Assert.True(TimerHelper.IsSameWeek(new DateTime(2024, 6, 16), false));
        }

        #endregion

        #region IsSameWeek(long ticks, bool isUtc)

        [Fact]
        public void IsSameWeek_WithTicks_SameWeek_ShouldReturnTrue()
        {
            var ticks = new DateTime(2024, 6, 12).Ticks;

            Assert.True(TimerHelper.IsSameWeek(ticks, true));
        }

        [Fact]
        public void IsSameWeek_WithTicks_DifferentWeek_ShouldReturnFalse()
        {
            var ticks = new DateTime(2024, 5, 1).Ticks;

            Assert.False(TimerHelper.IsSameWeek(ticks, true));
        }

        #endregion

        #region GetDayOfWeekTime

        [Fact]
        public void GetDayOfWeekTime_ShouldReturnCorrectDay()
        {
            var wednesday = new DateTime(2024, 6, 12, 10, 30, 0);

            var monday = TimerHelper.GetDayOfWeekTime(wednesday, DayOfWeek.Monday);
            var friday = TimerHelper.GetDayOfWeekTime(wednesday, DayOfWeek.Friday);

            Assert.Equal(new DateTime(2024, 6, 10, 10, 30, 0), monday);
            Assert.Equal(new DateTime(2024, 6, 14, 10, 30, 0), friday);
        }

        [Fact]
        public void GetDayOfWeekTime_WhenSameDay_ShouldReturnSameDate()
        {
            var wednesday = new DateTime(2024, 6, 12, 10, 30, 0);

            Assert.Equal(wednesday, TimerHelper.GetDayOfWeekTime(wednesday, DayOfWeek.Wednesday));
        }

        [Fact]
        public void GetDayOfWeekTime_FromCurrentTime_ShouldReturnDayOfCurrentWeek()
        {
            // FixedUtcNow=2024-06-15 (Saturday)
            var result = TimerHelper.GetDayOfWeekTimeWithUtc(DayOfWeek.Monday);

            Assert.Equal(new DateTime(2024, 6, 10), result.Date);
        }

        #endregion

        #region GetStartTimeOfWeek / GetEndTimeOfWeek

        [Fact]
        public void GetStartTimeOfWeek_ShouldReturnMondayMidnight()
        {
            // 2024-06-12 是周三
            var wednesday = new DateTime(2024, 6, 12, 14, 30, 0);

            Assert.Equal(new DateTime(2024, 6, 10, 0, 0, 0),
                TimerHelper.GetStartTimeOfWeek(wednesday));
        }

        [Fact]
        public void GetStartTimeOfWeek_WhenSunday_ShouldReturnPreviousMonday()
        {
            // 2024-06-16 是周日
            var sunday = new DateTime(2024, 6, 16, 0, 0, 0);

            Assert.Equal(new DateTime(2024, 6, 10, 0, 0, 0),
                TimerHelper.GetStartTimeOfWeek(sunday));
        }

        [Fact]
        public void GetEndTimeOfWeek_ShouldReturnSundayLastSecond()
        {
            var wednesday = new DateTime(2024, 6, 12, 14, 30, 0);

            Assert.Equal(new DateTime(2024, 6, 16, 23, 59, 59),
                TimerHelper.GetEndTimeOfWeek(wednesday));
        }

        [Fact]
        public void GetEndTimeOfWeek_WhenSunday_ShouldReturnSameDayLastSecond()
        {
            var sunday = new DateTime(2024, 6, 16, 14, 30, 0);

            Assert.Equal(new DateTime(2024, 6, 16, 23, 59, 59),
                TimerHelper.GetEndTimeOfWeek(sunday));
        }

        #endregion

        #region GetWeek/GetNextWeek WithUtc

        [Fact]
        public void GetWeekStartTimeWithUtc_ShouldReturnMonday()
        {
            // FixedUtcNow=2024-06-15 Saturday → Monday=2024-06-10
            Assert.Equal(new DateTime(2024, 6, 10, 0, 0, 0), TimerHelper.GetWeekStartTimeWithUtc());
        }

        [Fact]
        public void GetWeekEndTimeWithUtc_ShouldReturnSundayLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 16, 23, 59, 59), TimerHelper.GetWeekEndTimeWithUtc());
        }

        [Fact]
        public void GetWeekStartTimestampWithUtc_ShouldMatchWeekStart()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 10, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetWeekStartTimestampWithUtc());
        }

        [Fact]
        public void GetWeekEndTimestampWithUtc_ShouldMatchWeekEnd()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 16, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetWeekEndTimestampWithUtc());
        }

        [Fact]
        public void GetNextWeekStartTimeWithUtc_ShouldReturnNextMonday()
        {
            Assert.Equal(new DateTime(2024, 6, 17, 0, 0, 0), TimerHelper.GetNextWeekStartTimeWithUtc());
        }

        [Fact]
        public void GetNextWeekEndTimeWithUtc_ShouldReturnNextSundayLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 23, 23, 59, 59), TimerHelper.GetNextWeekEndTimeWithUtc());
        }

        [Fact]
        public void GetNextWeekStartTimestampWithUtc_ShouldMatchNextMonday()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 17, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetNextWeekStartTimestampWithUtc());
        }

        [Fact]
        public void GetNextWeekEndTimestampWithUtc_ShouldMatchNextSundayLastSecond()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 6, 23, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetNextWeekEndTimestampWithUtc());
        }

        [Fact]
        public void GetStartTimestampOfWeekWithUtc_ShouldMatchSpecifiedMonday()
        {
            var wednesday = new DateTime(2024, 6, 12);
            var expected = new DateTimeOffset(new DateTime(2024, 6, 10, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetStartTimestampOfWeekWithUtc(wednesday));
        }

        [Fact]
        public void GetEndTimestampOfWeekWithUtc_ShouldMatchSpecifiedSundayLastSecond()
        {
            var wednesday = new DateTime(2024, 6, 12);
            var expected = new DateTimeOffset(new DateTime(2024, 6, 16, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetEndTimestampOfWeekWithUtc(wednesday));
        }

        #endregion

        #region GetWeek/GetNextWeek WithTimeZone

        [Fact]
        public void GetWeekStartTimeWithTimeZone_ShouldReturnMonday()
        {
            Assert.Equal(new DateTime(2024, 6, 10, 0, 0, 0), TimerHelper.GetWeekStartTimeWithTimeZone());
        }

        [Fact]
        public void GetWeekEndTimeWithTimeZone_ShouldReturnSundayLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 16, 23, 59, 59), TimerHelper.GetWeekEndTimeWithTimeZone());
        }

        [Fact]
        public void GetNextWeekStartTimeWithTimeZone_ShouldReturnNextMonday()
        {
            Assert.Equal(new DateTime(2024, 6, 17, 0, 0, 0), TimerHelper.GetNextWeekStartTimeWithTimeZone());
        }

        [Fact]
        public void GetNextWeekEndTimeWithTimeZone_ShouldReturnNextSundayLastSecond()
        {
            Assert.Equal(new DateTime(2024, 6, 23, 23, 59, 59), TimerHelper.GetNextWeekEndTimeWithTimeZone());
        }

        [Fact]
        public void GetWeekStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetWeekStartTimestampWithUtc(),
                TimerHelper.GetWeekStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetWeekEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetWeekEndTimestampWithUtc(),
                TimerHelper.GetWeekEndTimestampWithTimeZone());
        }

        [Fact]
        public void GetNextWeekStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetNextWeekStartTimestampWithUtc(),
                TimerHelper.GetNextWeekStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetNextWeekEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetNextWeekEndTimestampWithUtc(),
                TimerHelper.GetNextWeekEndTimestampWithTimeZone());
        }

        [Fact]
        public void GetStartTimestampOfWeek_WithUtc_ShouldEqualUtcVersion()
        {
            var wednesday = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfWeekWithUtc(wednesday),
                TimerHelper.GetStartTimestampOfWeek(wednesday));
        }

        [Fact]
        public void GetEndTimestampOfWeek_WithUtc_ShouldEqualUtcVersion()
        {
            var wednesday = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetEndTimestampOfWeekWithUtc(wednesday),
                TimerHelper.GetEndTimestampOfWeek(wednesday));
        }

        [Fact]
        public void GetStartTimestampOfWeekWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            var wednesday = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfWeekWithUtc(wednesday),
                TimerHelper.GetStartTimestampOfWeekWithTimeZone(wednesday));
        }

        [Fact]
        public void GetEndTimestampOfWeekWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            var wednesday = new DateTime(2024, 6, 12, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetEndTimestampOfWeekWithUtc(wednesday),
                TimerHelper.GetEndTimestampOfWeekWithTimeZone(wednesday));
        }

        #endregion

        #region GetChinaDayOfWeekWithTimeZone

        [Fact]
        public void GetChinaDayOfWeekWithTimeZone_WithDate_ShouldReturnNonEmptyString()
        {
            var monday = new DateTime(2024, 6, 10);

            var result = TimerHelper.GetChinaDayOfWeekWithTimeZone(monday);

            Assert.False(string.IsNullOrEmpty(result));
        }

        [Theory]
        [InlineData(2024, 6, 10)]
        [InlineData(2024, 6, 11)]
        [InlineData(2024, 6, 12)]
        [InlineData(2024, 6, 13)]
        [InlineData(2024, 6, 14)]
        [InlineData(2024, 6, 15)]
        [InlineData(2024, 6, 16)]
        public void GetChinaDayOfWeekWithTimeZone_AllDaysOfWeek_ShouldReturnNonEmpty(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var result = TimerHelper.GetChinaDayOfWeekWithTimeZone(date);

            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact]
        public void GetChinaDayOfWeekWithTimeZone_NoParam_ShouldReturnNonEmptyString()
        {
            var result = TimerHelper.GetChinaDayOfWeekWithTimeZone();

            Assert.False(string.IsNullOrEmpty(result));
        }

        #endregion

        #region GetDayOfWeekTimeWithTimeZone

        [Fact]
        public void GetDayOfWeekTimeWithTimeZone_ShouldReturnDayOfCurrentWeek()
        {
            // FixedUtcNow=2024-06-15 Saturday → Monday=2024-06-10
            var result = TimerHelper.GetDayOfWeekTimeWithTimeZone(DayOfWeek.Monday);

            Assert.Equal(new DateTime(2024, 6, 10), result.Date);
        }

        #endregion
    }
}
