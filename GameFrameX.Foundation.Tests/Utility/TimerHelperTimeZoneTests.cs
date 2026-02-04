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
    }
}
