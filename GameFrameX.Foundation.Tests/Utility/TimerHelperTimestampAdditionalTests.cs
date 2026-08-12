using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.Timestamp.cs / Timestamp.UTC.cs / Timestamp.TimeZone.cs 单元测试：
    /// 覆盖 TimestampToTicks、TimestampMillisToTicks、TimeStampMillisecondToDateTime、TimestampSecondToDateTime、
    /// TimeSpanWithTimestampUtc(UtcMs)、TimeSpanWithTimestampWithTimeZone(WithTimeZoneMs)，
    /// 含边界、异常、UTC/TimeZone 双分支。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimerHelperTimestampAdditionalTests : IDisposable
    {
        private static readonly TimeZoneInfo UtcPlus8 =
            TimeZoneInfo.CreateCustomTimeZone("Test+8", TimeSpan.FromHours(8), "Test+8", "Test+8");

        public TimerHelperTimestampAdditionalTests()
        {
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        }

        public void Dispose()
        {
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
            TimerHelper.ResetTimeOffset();
        }

        #region TimestampToTicks

        [Fact]
        public void TimestampToTicks_ShouldConvertSecondsToTicks()
        {
            const long timestamp = 1718447400L; // 2024-06-15 10:30:00 UTC

            var expected = timestamp * TimeSpan.TicksPerSecond + TimerHelper.EpochUtc.Ticks;

            Assert.Equal(expected, TimerHelper.TimestampToTicks(timestamp));
        }

        [Fact]
        public void TimestampToTicks_Zero_ShouldReturnEpochTicks()
        {
            Assert.Equal(TimerHelper.EpochUtc.Ticks, TimerHelper.TimestampToTicks(0));
        }

        [Fact]
        public void TimestampToTicks_Negative_ShouldReturnEarlierTicks()
        {
            const long timestamp = -3600L;

            var expected = timestamp * TimeSpan.TicksPerSecond + TimerHelper.EpochUtc.Ticks;

            Assert.Equal(expected, TimerHelper.TimestampToTicks(timestamp));
        }

        [Fact]
        public void TimestampToTicks_ShouldMatchNewDateTime()
        {
            const long timestamp = 1718447400L;

            var ticks = TimerHelper.TimestampToTicks(timestamp);

            Assert.Equal(TimerHelper.EpochUtc.AddSeconds(timestamp), new DateTime(ticks, DateTimeKind.Utc));
        }

        [Theory]
        [InlineData(-62135596801L)]
        [InlineData(253402300800L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void TimestampToTicks_OutOfRange_ShouldThrowArgumentOutOfRangeException(long timestamp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimestampToTicks(timestamp));
        }

        [Theory]
        [InlineData(-62135596800L)]
        [InlineData(253402300799L)]
        public void TimestampToTicks_AtBoundary_ShouldNotThrow(long timestamp)
        {
            var exception = Record.Exception(() => TimerHelper.TimestampToTicks(timestamp));

            Assert.Null(exception);
        }

        #endregion

        #region TimestampMillisToTicks

        [Fact]
        public void TimestampMillisToTicks_ShouldConvertMillisToTicks()
        {
            const long timestamp = 1718447400123L;

            var expected = timestamp * TimeSpan.TicksPerMillisecond + TimerHelper.EpochUtc.Ticks;

            Assert.Equal(expected, TimerHelper.TimestampMillisToTicks(timestamp));
        }

        [Fact]
        public void TimestampMillisToTicks_Zero_ShouldReturnEpochTicks()
        {
            Assert.Equal(TimerHelper.EpochUtc.Ticks, TimerHelper.TimestampMillisToTicks(0));
        }

        [Theory]
        [InlineData(-62135596800001L)]
        [InlineData(253402300800000L)]
        [InlineData(long.MaxValue)]
        public void TimestampMillisToTicks_OutOfRange_ShouldThrow(long timestamp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimestampMillisToTicks(timestamp));
        }

        [Theory]
        [InlineData(-62135596800000L)]
        [InlineData(253402300799999L)]
        public void TimestampMillisToTicks_AtBoundary_ShouldNotThrow(long timestamp)
        {
            var exception = Record.Exception(() => TimerHelper.TimestampMillisToTicks(timestamp));

            Assert.Null(exception);
        }

        #endregion

        #region TimeStampMillisecondToDateTime

        [Fact]
        public void TimeStampMillisecondToDateTime_WithUtc_ShouldReturnUtcDateTime()
        {
            const long timestamp = 1718447400123L;

            var result = TimerHelper.TimeStampMillisecondToDateTime(timestamp, true);

            Assert.Equal(TimerHelper.EpochUtc.AddMilliseconds(timestamp), result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void TimeStampMillisecondToDateTime_WithUtcZoneAndNotUtc_ShouldEqualUtc()
        {
            // CurrentTimeZone=UTC, 不指定 utc 时会 ConvertTimeFromUtc(UTC) → 仍是 UTC 时间
            const long timestamp = 1718447400123L;

            var result = TimerHelper.TimeStampMillisecondToDateTime(timestamp, false);

            Assert.Equal(TimerHelper.EpochUtc.AddMilliseconds(timestamp), result);
        }

        [Fact]
        public void TimeStampMillisecondToDateTime_WithPlus8_ShouldShiftByOffset()
        {
            TimerHelper.SetTimeZone(UtcPlus8);
            const long timestamp = 1718447400000L; // 2024-06-15 10:30:00 UTC

            var utcResult = TimerHelper.TimeStampMillisecondToDateTime(timestamp, true);
            var zoneResult = TimerHelper.TimeStampMillisecondToDateTime(timestamp, false);

            Assert.Equal(utcResult.AddHours(8), zoneResult);
        }

        #endregion

        #region TimestampSecondToDateTime

        [Fact]
        public void TimestampSecondToDateTime_WithUtc_ShouldReturnUtcDateTime()
        {
            const long timestamp = 1718447400L;

            var result = TimerHelper.TimestampSecondToDateTime(timestamp, true);

            Assert.Equal(TimerHelper.EpochUtc.AddSeconds(timestamp), result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void TimestampSecondToDateTime_WithUtcZone_ShouldEqualUtc()
        {
            const long timestamp = 1718447400L;

            var result = TimerHelper.TimestampSecondToDateTime(timestamp, false);

            Assert.Equal(TimerHelper.EpochUtc.AddSeconds(timestamp), result);
        }

        [Fact]
        public void TimestampSecondToDateTime_WithPlus8_ShouldShiftByOffset()
        {
            TimerHelper.SetTimeZone(UtcPlus8);
            const long timestamp = 1718447400L;

            var utcResult = TimerHelper.TimestampSecondToDateTime(timestamp, true);
            var zoneResult = TimerHelper.TimestampSecondToDateTime(timestamp, false);

            Assert.Equal(utcResult.AddHours(8), zoneResult);
        }

        [Fact]
        public void TimestampSecondToDateTime_RoundTrip_WithUtc()
        {
            var original = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
            var ts = new DateTimeOffset(original).ToUnixTimeSeconds();

            var roundTrip = TimerHelper.TimestampSecondToDateTime(ts, true);

            Assert.Equal(original, roundTrip);
        }

        #endregion

        #region TimeSpanWithTimestampUtc / TimeSpanWithTimestampUtcMs

        [Fact]
        public void TimeSpanWithTimestampUtc_ShouldReturnTimeSpanFromSeconds()
        {
            const long timestamp = 3600L;

            Assert.Equal(TimeSpan.FromHours(1), TimerHelper.TimeSpanWithTimestampUtc(timestamp));
        }

        [Fact]
        public void TimeSpanWithTimestampUtc_Zero_ShouldReturnZero()
        {
            Assert.Equal(TimeSpan.Zero, TimerHelper.TimeSpanWithTimestampUtc(0));
        }

        [Fact]
        public void TimeSpanWithTimestampUtc_Negative_ShouldReturnNegative()
        {
            Assert.Equal(TimeSpan.FromHours(-1), TimerHelper.TimeSpanWithTimestampUtc(-3600L));
        }

        [Theory]
        [InlineData(-62135596801L)]
        [InlineData(253402300800L)]
        public void TimeSpanWithTimestampUtc_OutOfRange_ShouldThrow(long timestamp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimeSpanWithTimestampUtc(timestamp));
        }

        [Fact]
        public void TimeSpanWithTimestampUtcMs_ShouldReturnTimeSpanFromMillis()
        {
            const long timestamp = 1500L;

            Assert.Equal(TimeSpan.FromMilliseconds(1500), TimerHelper.TimeSpanWithTimestampUtcMs(timestamp));
        }

        [Theory]
        [InlineData(-62135596800001L)]
        [InlineData(253402300800000L)]
        public void TimeSpanWithTimestampUtcMs_OutOfRange_ShouldThrow(long timestamp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimeSpanWithTimestampUtcMs(timestamp));
        }

        #endregion

        #region TimeSpanWithTimestampWithTimeZone / TimeSpanWithTimestampWithTimeZoneMs

        [Fact]
        public void TimeSpanWithTimestampWithTimeZone_ShouldReturnTimeSpanFromSeconds()
        {
            Assert.Equal(TimeSpan.FromHours(1), TimerHelper.TimeSpanWithTimestampWithTimeZone(3600L));
        }

        [Fact]
        public void TimeSpanWithTimestampWithTimeZone_Zero_ShouldReturnZero()
        {
            Assert.Equal(TimeSpan.Zero, TimerHelper.TimeSpanWithTimestampWithTimeZone(0));
        }

        [Theory]
        [InlineData(-62135596801L)]
        [InlineData(253402300800L)]
        public void TimeSpanWithTimestampWithTimeZone_OutOfRange_ShouldThrow(long timestamp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimeSpanWithTimestampWithTimeZone(timestamp));
        }

        [Fact]
        public void TimeSpanWithTimestampWithTimeZoneMs_ShouldReturnTimeSpanFromMillis()
        {
            Assert.Equal(TimeSpan.FromMilliseconds(1500),
                TimerHelper.TimeSpanWithTimestampWithTimeZoneMs(1500L));
        }

        [Theory]
        [InlineData(-62135596800001L)]
        [InlineData(253402300800000L)]
        public void TimeSpanWithTimestampWithTimeZoneMs_OutOfRange_ShouldThrow(long timestamp)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimeSpanWithTimestampWithTimeZoneMs(timestamp));
        }

        #endregion
    }
}
