using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Year.cs / Year.UTC.cs / Year.TimeZone.cs 单元测试：
    /// 覆盖 GetStartTimeOfYear/GetEndTimeOfYear 及其时间戳重载（UTC 与 TimeZone 两套），
    /// 以及 GetYear/GetNextYear 系列方法（含跨年边界）。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperYearAdditionalTests : IDisposable
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

        public TimerHelperYearAdditionalTests()
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

        #region GetStartTimeOfYear / GetEndTimeOfYear

        [Fact]
        public void GetStartTimeOfYear_ShouldReturnJanuary1stMidnight()
        {
            var date = new DateTime(2024, 6, 15, 14, 30, 0);

            Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0), TimerHelper.GetStartTimeOfYear(date));
        }

        [Fact]
        public void GetEndTimeOfYear_ShouldReturnDecember31stLastSecond()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0);

            Assert.Equal(new DateTime(2024, 12, 31, 23, 59, 59), TimerHelper.GetEndTimeOfYear(date));
        }

        [Fact]
        public void GetEndTimeOfYear_ShouldBeOneSecondBeforeNextYearFirstDay()
        {
            var date = new DateTime(2024, 6, 15);

            Assert.Equal(TimerHelper.GetStartTimeOfYear(date).AddYears(1).AddSeconds(-1),
                TimerHelper.GetEndTimeOfYear(date));
        }

        [Fact]
        public void GetEndTimeOfYear_LeapYear_ShouldHave366Days()
        {
            var leapDate = new DateTime(2024, 6, 15);
            var nonLeapDate = new DateTime(2023, 6, 15);

            var leapDays = (TimerHelper.GetEndTimeOfYear(leapDate) - TimerHelper.GetStartTimeOfYear(leapDate)).TotalDays;
            var nonLeapDays = (TimerHelper.GetEndTimeOfYear(nonLeapDate) - TimerHelper.GetStartTimeOfYear(nonLeapDate)).TotalDays;

            // 闰年 12月31日 23:59:59 - 1月1日 00:00:00 ≈ 365.9999... 天（实际是 365 天 23:59:59）
            // 366天 vs 365天的判断转为整日比较
            Assert.Equal(366, (int)Math.Ceiling(leapDays));
            Assert.Equal(365, (int)Math.Ceiling(nonLeapDays));
        }

        #endregion

        #region GetStartTimestampOfYear / GetEndTimestampOfYear

        [Fact]
        public void GetStartTimestampOfYear_WithUtcKind_ShouldMatchEpochDelta()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var expected = new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetStartTimestampOfYear(date));
        }

        [Fact]
        public void GetEndTimestampOfYear_WithUtcKind_ShouldMatchEpochDelta()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);
            var expected = new DateTimeOffset(new DateTime(2024, 12, 31, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetEndTimestampOfYear(date));
        }

        #endregion

        #region GetStartTimestampOfYearWithUtc / GetEndTimestampOfYearWithUtc

        [Fact]
        public void GetStartTimestampOfYearWithUtc_ShouldMatchYearStart()
        {
            var date = new DateTime(2024, 6, 15);
            var expected = new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetStartTimestampOfYearWithUtc(date));
        }

        [Fact]
        public void GetEndTimestampOfYearWithUtc_ShouldMatchYearEnd()
        {
            var date = new DateTime(2024, 6, 15);
            var expected = new DateTimeOffset(new DateTime(2024, 12, 31, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetEndTimestampOfYearWithUtc(date));
        }

        #endregion

        #region GetStartTimestampOfYearWithTimeZone / GetEndTimestampOfYearWithTimeZone

        [Fact]
        public void GetStartTimestampOfYearWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfYear(date),
                TimerHelper.GetStartTimestampOfYearWithTimeZone(date));
        }

        [Fact]
        public void GetEndTimestampOfYearWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetEndTimestampOfYear(date),
                TimerHelper.GetEndTimestampOfYearWithTimeZone(date));
        }

        [Fact]
        public void GetStartTimestampOfYearWithTimeZone_WithPlus8_ShouldBeAheadOfUtc()
        {
            var zone = TimeZoneInfo.CreateCustomTimeZone("Test+8", TimeSpan.FromHours(8), "Test+8", "Test+8");
            TimerHelper.SetTimeZone(zone);
            var date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc);

            Assert.Equal(TimerHelper.GetStartTimestampOfYear(date) + 28800L,
                TimerHelper.GetStartTimestampOfYearWithTimeZone(date));
        }

        #endregion

        #region GetYear/GetNextYear WithUtc

        [Fact]
        public void GetYearStartTimeWithUtc_ShouldReturnJanuary1st()
        {
            Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                TimerHelper.GetYearStartTimeWithUtc());
        }

        [Fact]
        public void GetYearEndTimeWithUtc_ShouldReturnDecember31stLastSecond()
        {
            Assert.Equal(new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                TimerHelper.GetYearEndTimeWithUtc());
        }

        [Fact]
        public void GetYearStartTimestampWithUtc_ShouldMatchYearStart()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 1, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetYearStartTimestampWithUtc());
        }

        [Fact]
        public void GetYearEndTimestampWithUtc_ShouldMatchYearEnd()
        {
            var expected = new DateTimeOffset(new DateTime(2024, 12, 31, 23, 59, 59), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetYearEndTimestampWithUtc());
        }

        [Fact]
        public void GetNextYearStartTimeWithUtc_ShouldReturnNextYearJanuary1st()
        {
            Assert.Equal(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                TimerHelper.GetNextYearStartTimeWithUtc());
        }

        [Fact]
        public void GetNextYearStartTimestampWithUtc_ShouldMatchNextYearStart()
        {
            var expected = new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0), TimeSpan.Zero).ToUnixTimeSeconds();

            Assert.Equal(expected, TimerHelper.GetNextYearStartTimestampWithUtc());
        }

        #endregion

        #region GetYear/GetNextYear WithTimeZone

        [Fact]
        public void GetYearStartTimeWithTimeZone_ShouldReturnJanuary1st()
        {
            Assert.Equal(new DateTime(2024, 1, 1, 0, 0, 0), TimerHelper.GetYearStartTimeWithTimeZone());
        }

        [Fact]
        public void GetYearEndTime_ShouldReturnDecember31stLastSecond()
        {
            Assert.Equal(new DateTime(2024, 12, 31, 23, 59, 59), TimerHelper.GetYearEndTime());
        }

        [Fact]
        public void GetNextYearStartTimeWithTimeZone_ShouldBeOneYearAhead()
        {
            Assert.Equal(new DateTime(2025, 1, 1, 0, 0, 0),
                TimerHelper.GetNextYearStartTimeWithTimeZone());
        }

        [Fact]
        public void GetYearStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetYearStartTimestampWithUtc(),
                TimerHelper.GetYearStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetYearEndTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetYearEndTimestampWithUtc(),
                TimerHelper.GetYearEndTimestampWithTimeZone());
        }

        [Fact]
        public void GetNextYearStartTimestamp_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetNextYearStartTimestampWithUtc(),
                TimerHelper.GetNextYearStartTimestamp());
        }

        [Fact]
        public void GetNextYearStartTimestampWithTimeZone_WithUtc_ShouldEqualUtcVersion()
        {
            Assert.Equal(TimerHelper.GetNextYearStartTimestampWithUtc(),
                TimerHelper.GetNextYearStartTimestampWithTimeZone());
        }

        [Fact]
        public void GetYearEndTime_WhenFixedAtEndOfYear_ShouldCrossToNextYear()
        {
            // 在年末测试跨年
            TimerHelper.SetTimeProvider(new FakeTimeProvider(new DateTimeOffset(2024, 12, 31, 23, 59, 0, TimeSpan.Zero)));

            Assert.Equal(new DateTime(2024, 12, 31, 23, 59, 59), TimerHelper.GetYearEndTime());

            Assert.Equal(new DateTime(2025, 1, 1, 0, 0, 0),
                TimerHelper.GetNextYearStartTimeWithTimeZone());
        }

        #endregion
    }
}
