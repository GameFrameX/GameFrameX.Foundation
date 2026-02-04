using System;
using GameFrameX.Foundation.Utility;
using Xunit;
using Xunit.Abstractions;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper 时区功能测试
    /// </summary>
    public class TimerHelperTimeZoneTests : IDisposable
    {
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// 构造函数，初始化测试环境
        /// </summary>
        /// <param name="output">测试输出帮助类</param>
        public TimerHelperTimeZoneTests(ITestOutputHelper output)
        {
            _output = output;
            // Ensure default state is UTC before each test
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        }

        /// <summary>
        /// 释放资源，重置测试环境
        /// </summary>
        public void Dispose()
        {
            // Reset to UTC after each test to avoid side effects
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        }

        /// <summary>
        /// 测试默认时区应为 UTC
        /// </summary>
        [Fact]
        public void DefaultTimeZone_ShouldBeUtc()
        {
            Assert.Equal(TimeZoneInfo.Utc.Id, TimerHelper.CurrentTimeZone.Id);
            
            var now = TimerHelper.GetNow();
            var utcNow = DateTime.UtcNow;
            
            // Allow small difference for execution time
            Assert.True((now - utcNow).Duration().TotalSeconds < 1);
        }

        /// <summary>
        /// 测试 SetTimeZone 应正确更新当前时区
        /// </summary>
        [Fact]
        public void SetTimeZone_ShouldUpdateCurrentTimeZone()
        {
            // Use a fixed offset timezone for reliable testing
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");

            TimerHelper.SetTimeZone(customTimeZone);
            Assert.Equal(customTimeZone.Id, TimerHelper.CurrentTimeZone.Id);

            var now = TimerHelper.GetNow();
            var utcNow = DateTime.UtcNow;
            
            // Expected: Now = UtcNow + 8 hours
            var expectedNow = utcNow + offset;
            Assert.True((now - expectedNow).Duration().TotalSeconds < 1);
        }

        /// <summary>
        /// 测试 GetTodayStart 系列函数应遵循时区设置
        /// </summary>
        [Fact]
        public void GetTodayStart_ShouldRespectTimeZone()
        {
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");
            TimerHelper.SetTimeZone(customTimeZone);

            var now = TimerHelper.GetNow();
            var todayStart = TimerHelper.GetTodayStartTime();
            var todayStartTimestamp = TimerHelper.GetTodayStartTimestamp();

            // Verify DateTime parts
            Assert.Equal(now.Date, todayStart);
            Assert.Equal(0, todayStart.Hour);

            // Verify Timestamp
            // Construct expected DateTimeOffset
            var expectedTime = new DateTimeOffset(todayStart, offset);
            Assert.Equal(expectedTime.ToUnixTimeSeconds(), todayStartTimestamp);
        }

        /// <summary>
        /// 测试 GetWeekStart 系列函数应遵循时区设置
        /// </summary>
        [Fact]
        public void GetWeekStart_ShouldRespectTimeZone()
        {
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");
            TimerHelper.SetTimeZone(customTimeZone);

            var now = TimerHelper.GetNow();
            var weekStart = TimerHelper.GetWeekStartTime();
            var weekStartTimestamp = TimerHelper.GetWeekStartTimestamp();

            // Verify it is indeed the start of the week (Monday)
            Assert.Equal(DayOfWeek.Monday, weekStart.DayOfWeek);
            Assert.True(weekStart <= now);
            Assert.True((now - weekStart).TotalDays < 7);
            Assert.Equal(0, weekStart.Hour);

            // Verify Timestamp
            var expectedTime = new DateTimeOffset(weekStart, offset);
            Assert.Equal(expectedTime.ToUnixTimeSeconds(), weekStartTimestamp);
        }

        /// <summary>
        /// 测试 GetMonthStart 系列函数应遵循时区设置
        /// </summary>
        [Fact]
        public void GetMonthStart_ShouldRespectTimeZone()
        {
            var offset = TimeSpan.FromHours(-5); // Test negative offset
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test-5", offset, "Test-5", "Test-5");
            TimerHelper.SetTimeZone(customTimeZone);

            var now = TimerHelper.GetNow();
            var monthStart = TimerHelper.GetMonthStartTime();
            var monthStartTimestamp = TimerHelper.GetMonthStartTimestamp();

            Assert.Equal(now.Year, monthStart.Year);
            Assert.Equal(now.Month, monthStart.Month);
            Assert.Equal(1, monthStart.Day);
            Assert.Equal(0, monthStart.Hour);

            // Verify Timestamp
            var expectedTime = new DateTimeOffset(monthStart, offset);
            Assert.Equal(expectedTime.ToUnixTimeSeconds(), monthStartTimestamp);
        }

        /// <summary>
        /// 测试 GetYearStart 系列函数应遵循时区设置
        /// </summary>
        [Fact]
        public void GetYearStart_ShouldRespectTimeZone()
        {
            var offset = TimeSpan.FromHours(9);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+9", offset, "Test+9", "Test+9");
            TimerHelper.SetTimeZone(customTimeZone);

            var now = TimerHelper.GetNow();
            var yearStart = TimerHelper.GetYearStartTime();
            var yearStartTimestamp = TimerHelper.GetYearStartTimestamp();

            Assert.Equal(now.Year, yearStart.Year);
            Assert.Equal(1, yearStart.Month);
            Assert.Equal(1, yearStart.Day);
            Assert.Equal(0, yearStart.Hour);

            // Verify Timestamp
            var expectedTime = new DateTimeOffset(yearStart, offset);
            Assert.Equal(expectedTime.ToUnixTimeSeconds(), yearStartTimestamp);
        }

        /// <summary>
        /// 测试 Unix 时间戳应保持绝对性（不受时区设置影响，始终基于 UTC）
        /// </summary>
        [Fact]
        public void UnixTimestamp_ShouldBeAbsolute()
        {
            // Timestamp should be the same regardless of timezone setting because it represents absolute time
            
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
            var ts1 = TimerHelper.UnixTimeSeconds();

            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+10", TimeSpan.FromHours(10), "Test+10", "Test+10");
            TimerHelper.SetTimeZone(customTimeZone);
            var ts2 = TimerHelper.UnixTimeSeconds();

            // Allow small difference for execution time
            Assert.True(Math.Abs(ts2 - ts1) < 2);
        }
        [Fact]
        public void SetTimeZone_WithNull_ShouldDefaultToUtc()
        {
            TimerHelper.SetTimeZone((TimeZoneInfo)null);
            Assert.Equal(TimeZoneInfo.Utc.Id, TimerHelper.CurrentTimeZone.Id);
        }

        [Fact]
        public void SetTimeZone_WithInvalidId_ShouldDefaultToUtc()
        {
            TimerHelper.SetTimeZone("InvalidTimeZoneId");
            Assert.Equal(TimeZoneInfo.Utc.Id, TimerHelper.CurrentTimeZone.Id);
        }

        /// <summary>
        /// 测试 UnixTimeSecondsWithTimeZone 和 UnixTimeMillisecondsWithTimeZone 应包含时区偏移
        /// </summary>
        [Fact]
        public void UnixTimeWithTimeZone_ShouldIncludeOffset()
        {
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");
            TimerHelper.SetTimeZone(customTimeZone);

            var tsStandard = TimerHelper.UnixTimeSeconds();
            var tsWithZone = TimerHelper.UnixTimeSecondsWithTimeZone();
            var tsMsStandard = TimerHelper.UnixTimeMilliseconds();
            var tsMsWithZone = TimerHelper.UnixTimeMillisecondsWithTimeZone();

            // 验证秒级时间戳差异接近 8 小时 (28800 秒)
            // 注意：由于执行时间差异，tsStandard 和 tsWithZone 获取的时间点可能略有不同，但差异不应很大
            var diffSeconds = tsWithZone - tsStandard;
            // 允许误差在 2 秒内
            Assert.True(Math.Abs(diffSeconds - 28800) <= 2, $"Expected diff 28800, but got {diffSeconds}");

            // 验证毫秒级时间戳差异接近 8 小时 (28800000 毫秒)
            var diffMilliseconds = tsMsWithZone - tsMsStandard;
            // 允许误差在 100 毫秒内
            Assert.True(Math.Abs(diffMilliseconds - 28800000) <= 100, $"Expected diff 28800000, but got {diffMilliseconds}");
        }

        /// <summary>
        /// 测试 TimeToSecondsWithTimeZone 和 TimeToMillisecondsWithTimeZone 方法
        /// </summary>
        [Fact]
        public void TimeToWithTimeZone_ShouldHandleDifferentKinds()
        {
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");
            TimerHelper.SetTimeZone(customTimeZone);

            var nowUtc = DateTime.UtcNow;
            var nowLocal = DateTime.Now; // System Local
            // 构造一个 Unspecified 时间，假设它就是 CurrentTimeZone 下的时间
            var nowZone = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, customTimeZone); // Kind is Unspecified

            // 1. Test UTC input
            // 预期：UTC 时间戳 + 8 小时
            var expectedSeconds = new DateTimeOffset(nowUtc).ToUnixTimeSeconds() + 28800;
            var actualSecondsUtc = TimerHelper.TimeToSecondsWithTimeZone(nowUtc);
            Assert.Equal(expectedSeconds, actualSecondsUtc);

            // 2. Test Unspecified input (assumed as CurrentTimeZone)
            // nowZone 是 UTC+8 的时间，转回 UTC 应该是 nowUtc
            // 所以结果应该和上面一样
            var actualSecondsZone = TimerHelper.TimeToSecondsWithTimeZone(nowZone);
            Assert.Equal(expectedSeconds, actualSecondsZone);

            // 3. Test System Local input
            // TimerHelper should convert System Local to UTC, then apply CurrentTimeZone offset
            // So result should still be same absolute moment + 8 hours
            // 注意：如果 System Local 和 UTC 转换有微小差异（如 tick 精度），可能会有一点点误差，但在秒级应该是相等的
            var actualSecondsLocal = TimerHelper.TimeToSecondsWithTimeZone(nowLocal);
            // nowLocal 和 nowUtc 代表同一时刻（理想情况下）
            // 允许 1 秒误差
            Assert.True(Math.Abs(actualSecondsLocal - expectedSeconds) <= 1);
        }

        /// <summary>
        /// 测试扩展的 Day/Week/Month/Year 的 WithTimeZone 方法
        /// </summary>
        [Fact]
        public void ExtendedMethods_ShouldIncludeOffset()
        {
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");
            TimerHelper.SetTimeZone(customTimeZone);

            long offsetSeconds = (long)offset.TotalSeconds;

            // 1. Test Day
            var todayStandard = TimerHelper.GetTodayStartTimestamp();
            var todayWithZone = TimerHelper.GetTodayStartTimestampWithTimeZone();
            Assert.Equal(offsetSeconds, todayWithZone - todayStandard);

            // 2. Test Week
            var weekStandard = TimerHelper.GetWeekStartTimestamp();
            var weekWithZone = TimerHelper.GetWeekStartTimestampWithTimeZone();
            Assert.Equal(offsetSeconds, weekWithZone - weekStandard);

            // 3. Test Month
            var monthStandard = TimerHelper.GetMonthStartTimestamp();
            var monthWithZone = TimerHelper.GetMonthStartTimestampWithTimeZone();
            Assert.Equal(offsetSeconds, monthWithZone - monthStandard);

            // 4. Test Year
            var yearStandard = TimerHelper.GetYearStartTimestamp();
            var yearWithZone = TimerHelper.GetYearStartTimestampWithTimeZone();
            Assert.Equal(offsetSeconds, yearWithZone - yearStandard);
            
            // 5. Test specific date (Month End)
            var now = DateTime.UtcNow;
            var monthEndStandard = TimerHelper.GetEndTimestampOfMonth(now);
            var monthEndWithZone = TimerHelper.GetEndTimestampOfMonthWithTimeZone(now);
            Assert.Equal(offsetSeconds, monthEndWithZone - monthEndStandard);
        }

        [Fact]
        public void AllWithTimeZoneMethods_ShouldIncludeOffset()
        {
            var offset = TimeSpan.FromHours(8);
            var customTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+8", offset, "Test+8", "Test+8");
            TimerHelper.SetTimeZone(customTimeZone);
            long offsetSeconds = (long)offset.TotalSeconds;
            var now = DateTime.UtcNow;

            // Day
            Assert.Equal(offsetSeconds, TimerHelper.GetTodayStartTimestampWithTimeZone() - TimerHelper.GetTodayStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetTodayEndTimestampWithTimeZone() - TimerHelper.GetTodayEndTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetStartTimestampOfDayWithTimeZone(now) - TimerHelper.GetStartTimestampOfDay(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetEndTimestampOfDayWithTimeZone(now) - TimerHelper.GetEndTimestampOfDay(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetTomorrowStartTimestampWithTimeZone() - TimerHelper.GetTomorrowStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetTomorrowEndTimestampWithTimeZone() - TimerHelper.GetTomorrowEndTimestamp());

            // Week
            Assert.Equal(offsetSeconds, TimerHelper.GetWeekStartTimestampWithTimeZone() - TimerHelper.GetWeekStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetWeekEndTimestampWithTimeZone() - TimerHelper.GetWeekEndTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetStartTimestampOfWeekWithTimeZone(now) - TimerHelper.GetStartTimestampOfWeek(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetEndTimestampOfWeekWithTimeZone(now) - TimerHelper.GetEndTimestampOfWeek(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetNextWeekStartTimestampWithTimeZone() - TimerHelper.GetNextWeekStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetNextWeekEndTimestampWithTimeZone() - TimerHelper.GetNextWeekEndTimestamp());

            // Month
            Assert.Equal(offsetSeconds, TimerHelper.GetStartTimestampOfMonthWithTimeZone(now) - TimerHelper.GetStartTimestampOfMonth(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetEndTimestampOfMonthWithTimeZone(now) - TimerHelper.GetEndTimestampOfMonth(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetNextMonthStartTimestampWithTimeZone() - TimerHelper.GetNextMonthStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetNextMonthEndTimestampWithTimeZone() - TimerHelper.GetNextMonthEndTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetMonthStartTimestampWithTimeZone() - TimerHelper.GetMonthStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetMonthEndTimestampWithTimeZone() - TimerHelper.GetMonthEndTimestamp());

            // Year
            Assert.Equal(offsetSeconds, TimerHelper.GetYearStartTimestampWithTimeZone() - TimerHelper.GetYearStartTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetYearEndTimestampWithTimeZone() - TimerHelper.GetYearEndTimestamp());
            Assert.Equal(offsetSeconds, TimerHelper.GetStartTimestampOfYearWithTimeZone(now) - TimerHelper.GetStartTimestampOfYear(now));
            Assert.Equal(offsetSeconds, TimerHelper.GetEndTimestampOfYearWithTimeZone(now) - TimerHelper.GetEndTimestampOfYear(now));
        }
    }
}
