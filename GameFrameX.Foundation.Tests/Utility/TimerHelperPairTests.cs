// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

/// <summary>
/// TimerHelper UTC 和 TimeZone 成对函数的完整单元测试
/// </summary>
public class TimerHelperPairTests : IDisposable
{
    private readonly TimeZoneInfo _testTimeZone;
    private readonly TimeSpan _testOffset;

    public TimerHelperPairTests()
    {
        // 使用 UTC+12 作为测试时区
        _testOffset = TimeSpan.FromHours(12);
        _testTimeZone = TimeZoneInfo.CreateCustomTimeZone("Test+12", _testOffset, "Test+12", "Test+12");
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
    }

    public void Dispose()
    {
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        TimerHelper.ResetTimeOffset();
    }

    #region Current - GetNow

    [Fact]
    public void GetNow_UtcAndTimeZone_ShouldHaveCorrectOffset()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var utcNow = TimerHelper.GetNowWithUtc();
        var zoneNow = TimerHelper.GetNowWithTimeZone();

        // Assert - TimeZone 时间应该比 UTC 时间快12小时
        var diff = zoneNow - utcNow;
        Assert.True(Math.Abs(diff.TotalHours - 12) < 0.1, $"Time difference should be ~12 hours, got {diff.TotalHours}");
    }

    #endregion

    #region Current - CurrentTime

    [Fact]
    public void CurrentTime_UtcAndTimeZone_ShouldReturnValidFormat()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var utcTime = TimerHelper.CurrentTimeWithUtc();
        var zoneTime = TimerHelper.CurrentTimeWithTimeZone();
        var utcTimeString = TimerHelper.CurrentTimeWithUtcFullString();
        var zoneTimeString = TimerHelper.CurrentTimeWithTimeZoneFullString();

        // Assert - 验证格式正确（6位数字或字符串）
        Assert.InRange(utcTime, 0, 235959);
        Assert.InRange(zoneTime, 0, 235959);
        Assert.Matches(@"^\d{6}$", utcTimeString);
        Assert.Matches(@"^\d{6}$", zoneTimeString);
    }

    #endregion

    #region Current - GetElapsedSeconds/GetElapsedMilliseconds

    [Fact]
    public void GetElapsedSeconds_UtcAndTimeZone_ShouldBeConsistent()
    {
        // Arrange - UTC 版本
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        var pastTimestampUtc = TimerHelper.UnixTimeSeconds() - 60;

        // Act - UTC 版本
        var elapsedUtc = TimerHelper.GetElapsedSecondsWithUtc(pastTimestampUtc);

        // Arrange - TimeZone 版本
        TimerHelper.SetTimeZone(_testTimeZone);
        var pastTimestampZone = TimerHelper.UnixTimeSecondsWithTimeZoneOffset() - 60;

        // Act - TimeZone 版本
        var elapsedZone = TimerHelper.GetElapsedSecondsWithTimeZone(pastTimestampZone);

        // Assert
        Assert.True(Math.Abs(elapsedUtc - 60) <= 2, $"UTC elapsed should be ~60, got {elapsedUtc}");
        Assert.True(Math.Abs(elapsedZone - 60) <= 2, $"TimeZone elapsed should be ~60, got {elapsedZone}");
    }

    [Fact]
    public void GetElapsedMilliseconds_UtcAndTimeZone_ShouldBeConsistent()
    {
        // Arrange - UTC 版本
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        var pastTimestampUtc = TimerHelper.UnixTimeMilliseconds() - 60000;

        // Act - UTC 版本
        var elapsedUtc = TimerHelper.GetElapsedMillisecondsWithUtc(pastTimestampUtc);

        // Arrange - TimeZone 版本
        TimerHelper.SetTimeZone(_testTimeZone);
        var pastTimestampZone = TimerHelper.UnixTimeMillisecondsWithTimeZoneOffset() - 60000;

        // Act - TimeZone 版本
        var elapsedZone = TimerHelper.GetElapsedMillisecondsWithTimeZone(pastTimestampZone);

        // Assert
        Assert.True(Math.Abs(elapsedUtc - 60000) <= 1000, $"UTC elapsed should be ~60000, got {elapsedUtc}");
        Assert.True(Math.Abs(elapsedZone - 60000) <= 1000, $"TimeZone elapsed should be ~60000, got {elapsedZone}");
    }

    #endregion

    #region Day - Today

    [Fact]
    public void GetTodayStart_Utc_ShouldReturnZeroHour()
    {
        // Act
        var todayStart = TimerHelper.GetTodayStartTimeWithUtc();

        // Assert
        Assert.Equal(0, todayStart.Hour);
        Assert.Equal(0, todayStart.Minute);
        Assert.Equal(0, todayStart.Second);
    }

    [Fact]
    public void GetTodayStart_TimeZone_ShouldReturnZeroHour()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var todayStart = TimerHelper.GetTodayStartTimeWithTimeZone();

        // Assert
        Assert.Equal(0, todayStart.Hour);
        Assert.Equal(0, todayStart.Minute);
        Assert.Equal(0, todayStart.Second);
    }

    [Fact]
    public void GetTodayEnd_Utc_ShouldReturnLastSecond()
    {
        // Act
        var todayEnd = TimerHelper.GetTodayEndTimeWithUtc();

        // Assert
        Assert.Equal(23, todayEnd.Hour);
        Assert.Equal(59, todayEnd.Minute);
        Assert.Equal(59, todayEnd.Second);
    }

    [Fact]
    public void GetTodayEnd_TimeZone_ShouldReturnLastSecond()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var todayEnd = TimerHelper.GetTodayEndTimeWithTimeZone();

        // Assert
        Assert.Equal(23, todayEnd.Hour);
        Assert.Equal(59, todayEnd.Minute);
        Assert.Equal(59, todayEnd.Second);
    }

    #endregion

    #region Day - Tomorrow

    [Fact]
    public void GetTomorrowStart_Utc_ShouldReturnZeroHour()
    {
        // Act
        var tomorrowStart = TimerHelper.GetTomorrowStartTimeWithUtc();
        var todayStart = TimerHelper.GetTodayStartTimeWithUtc();

        // Assert
        Assert.Equal(0, tomorrowStart.Hour);
        Assert.Equal(1, (tomorrowStart.Date - todayStart.Date).Days);
    }

    [Fact]
    public void GetTomorrowStart_TimeZone_ShouldReturnZeroHour()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var tomorrowStart = TimerHelper.GetTomorrowStartTimeWithTimeZone();
        var todayStart = TimerHelper.GetTodayStartTimeWithTimeZone();

        // Assert
        Assert.Equal(0, tomorrowStart.Hour);
        Assert.Equal(1, (tomorrowStart.Date - todayStart.Date).Days);
    }

    #endregion

    #region Week - UTC Functions

    [Fact]
    public void GetWeekStart_Utc_ShouldReturnMonday()
    {
        // Act
        var weekStart = TimerHelper.GetWeekStartTimeWithUtc();

        // Assert
        Assert.Equal(DayOfWeek.Monday, weekStart.DayOfWeek);
        Assert.Equal(0, weekStart.Hour);
        Assert.Equal(0, weekStart.Minute);
        Assert.Equal(0, weekStart.Second);
    }

    [Fact]
    public void GetWeekEnd_Utc_ShouldReturnSunday()
    {
        // Act
        var weekEnd = TimerHelper.GetWeekEndTimeWithUtc();
        var weekStart = TimerHelper.GetWeekStartTimeWithUtc();

        // Assert
        Assert.Equal(DayOfWeek.Sunday, weekEnd.DayOfWeek);
        Assert.Equal(23, weekEnd.Hour);
        Assert.Equal(59, weekEnd.Minute);
        Assert.Equal(59, weekEnd.Second);
        Assert.Equal(6, (weekEnd.Date - weekStart.Date).Days);
    }

    [Fact]
    public void GetNextWeekStart_Utc_ShouldReturnMonday()
    {
        // Act
        var nextWeekStart = TimerHelper.GetNextWeekStartTimeWithUtc();
        var weekStart = TimerHelper.GetWeekStartTimeWithUtc();

        // Assert
        Assert.Equal(DayOfWeek.Monday, nextWeekStart.DayOfWeek);
        Assert.Equal(7, (nextWeekStart.Date - weekStart.Date).Days);
    }

    [Fact]
    public void GetStartTimestampOfWeek_Utc_ShouldReturnCorrectTimestamp()
    {
        // Arrange
        var testDate = new DateTime(2024, 3, 15, 10, 30, 0, DateTimeKind.Utc); // 周五

        // Act
        var timestamp = TimerHelper.GetStartTimestampOfWeekWithUtc(testDate);
        var weekStart = TimerHelper.GetStartTimeOfWeek(testDate);

        // Assert
        var expectedTimestamp = new DateTimeOffset(weekStart, TimeSpan.Zero).ToUnixTimeSeconds();
        Assert.Equal(expectedTimestamp, timestamp);
        Assert.Equal(DayOfWeek.Monday, weekStart.DayOfWeek);
    }

    [Fact]
    public void GetEndTimestampOfWeek_Utc_ShouldReturnCorrectTimestamp()
    {
        // Arrange
        var testDate = new DateTime(2024, 3, 15, 10, 30, 0, DateTimeKind.Utc); // 周五

        // Act
        var timestamp = TimerHelper.GetEndTimestampOfWeekWithUtc(testDate);
        var weekEnd = TimerHelper.GetEndTimeOfWeek(testDate);

        // Assert
        var expectedTimestamp = new DateTimeOffset(weekEnd, TimeSpan.Zero).ToUnixTimeSeconds();
        Assert.Equal(expectedTimestamp, timestamp);
        Assert.Equal(DayOfWeek.Sunday, weekEnd.DayOfWeek);
    }

    #endregion

    #region Week - TimeZone Functions

    [Fact]
    public void GetWeekStart_TimeZone_ShouldReturnMonday()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var weekStart = TimerHelper.GetWeekStartTimeWithTimeZone();

        // Assert
        Assert.Equal(DayOfWeek.Monday, weekStart.DayOfWeek);
        Assert.Equal(0, weekStart.Hour);
    }

    [Fact]
    public void GetWeekEnd_TimeZone_ShouldReturnSunday()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var weekEnd = TimerHelper.GetWeekEndTimeWithTimeZone();

        // Assert
        Assert.Equal(DayOfWeek.Sunday, weekEnd.DayOfWeek);
        Assert.Equal(23, weekEnd.Hour);
    }

    [Fact]
    public void GetNextWeekStart_TimeZone_ShouldReturnMonday()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var nextWeekStart = TimerHelper.GetNextWeekStartTimeWithTimeZone();
        var weekStart = TimerHelper.GetWeekStartTimeWithTimeZone();

        // Assert
        Assert.Equal(DayOfWeek.Monday, nextWeekStart.DayOfWeek);
        Assert.Equal(7, (nextWeekStart.Date - weekStart.Date).Days);
    }

    [Theory]
    [InlineData(DayOfWeek.Sunday)]
    [InlineData(DayOfWeek.Monday)]
    [InlineData(DayOfWeek.Tuesday)]
    [InlineData(DayOfWeek.Wednesday)]
    [InlineData(DayOfWeek.Thursday)]
    [InlineData(DayOfWeek.Friday)]
    [InlineData(DayOfWeek.Saturday)]
    public void GetDayOfWeekTimeWithTimeZone_AllDays_ShouldReturnCorrectDay(DayOfWeek day)
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var result = TimerHelper.GetDayOfWeekTimeWithTimeZone(day);

        // Assert
        Assert.Equal(day, result.DayOfWeek);
    }

    #endregion

    #region Month - UTC Functions

    [Fact]
    public void GetMonthStart_Utc_ShouldReturnFirstDay()
    {
        // Act
        var monthStart = TimerHelper.GetMonthStartTimeWithUtc();

        // Assert
        Assert.Equal(1, monthStart.Day);
        Assert.Equal(0, monthStart.Hour);
    }

    [Fact]
    public void GetMonthEnd_Utc_ShouldReturnLastDay()
    {
        // Act
        var monthEnd = TimerHelper.GetMonthEndTimeWithUtc();
        var monthStart = TimerHelper.GetMonthStartTimeWithUtc();

        // Assert
        Assert.Equal(23, monthEnd.Hour);
        Assert.Equal(59, monthEnd.Minute);
        Assert.Equal(59, monthEnd.Second);
        Assert.True(monthEnd.Day >= 28, "Last day of month should be at least 28");
    }

    [Fact]
    public void GetNextMonthStart_Utc_ShouldReturnFirstDay()
    {
        // Act
        var nextMonthStart = TimerHelper.GetNextMonthStartTimeWithUtc();

        // Assert
        Assert.Equal(1, nextMonthStart.Day);
        Assert.Equal(0, nextMonthStart.Hour);
    }

    #endregion

    #region Month - TimeZone Functions

    [Fact]
    public void GetMonthStart_TimeZone_ShouldReturnFirstDay()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var monthStart = TimerHelper.GetMonthStartTimeWithTimeZone();

        // Assert
        Assert.Equal(1, monthStart.Day);
        Assert.Equal(0, monthStart.Hour);
    }

    [Fact]
    public void GetMonthEnd_TimeZone_ShouldReturnLastDay()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var monthEnd = TimerHelper.GetMonthEndTimeWithTimeZone();

        // Assert
        Assert.Equal(23, monthEnd.Hour);
        Assert.Equal(59, monthEnd.Minute);
        Assert.Equal(59, monthEnd.Second);
    }

    [Fact]
    public void GetNextMonthStart_TimeZone_ShouldReturnFirstDay()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var nextMonthStart = TimerHelper.GetNextMonthStartTimeWithTimeZone();

        // Assert
        Assert.Equal(1, nextMonthStart.Day);
        Assert.Equal(0, nextMonthStart.Hour);
    }

    #endregion

    #region Year - UTC Functions

    [Fact]
    public void GetYearStart_Utc_ShouldReturnJanuary1st()
    {
        // Act
        var yearStart = TimerHelper.GetYearStartTimeWithUtc();

        // Assert
        Assert.Equal(1, yearStart.Month);
        Assert.Equal(1, yearStart.Day);
        Assert.Equal(0, yearStart.Hour);
    }

    [Fact]
    public void GetYearEnd_Utc_ShouldReturnDecember31st()
    {
        // Act
        var yearEnd = TimerHelper.GetYearEndTimeWithUtc();

        // Assert
        Assert.Equal(12, yearEnd.Month);
        Assert.Equal(31, yearEnd.Day);
        Assert.Equal(23, yearEnd.Hour);
        Assert.Equal(59, yearEnd.Minute);
        Assert.Equal(59, yearEnd.Second);
    }

    [Fact]
    public void GetNextYearStart_Utc_ShouldReturnJanuary1st()
    {
        // Act
        var nextYearStart = TimerHelper.GetNextYearStartTimeWithUtc();
        var yearStart = TimerHelper.GetYearStartTimeWithUtc();

        // Assert
        Assert.Equal(1, nextYearStart.Month);
        Assert.Equal(1, nextYearStart.Day);
        Assert.Equal(1, nextYearStart.Year - yearStart.Year);
    }

    [Fact]
    public void GetStartTimestampOfYear_Utc_ShouldReturnCorrectTimestamp()
    {
        // Arrange
        var testDate = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var timestamp = TimerHelper.GetStartTimestampOfYearWithUtc(testDate);
        var yearStart = TimerHelper.GetStartTimeOfYear(testDate);

        // Assert
        var expectedTimestamp = new DateTimeOffset(yearStart, TimeSpan.Zero).ToUnixTimeSeconds();
        Assert.Equal(expectedTimestamp, timestamp);
    }

    [Fact]
    public void GetEndTimestampOfYear_Utc_ShouldReturnCorrectTimestamp()
    {
        // Arrange
        var testDate = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);

        // Act
        var timestamp = TimerHelper.GetEndTimestampOfYearWithUtc(testDate);
        var yearEnd = TimerHelper.GetEndTimeOfYear(testDate);

        // Assert
        var expectedTimestamp = new DateTimeOffset(yearEnd, TimeSpan.Zero).ToUnixTimeSeconds();
        Assert.Equal(expectedTimestamp, timestamp);
    }

    #endregion

    #region Year - TimeZone Functions

    [Fact]
    public void GetYearStart_TimeZone_ShouldReturnJanuary1st()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var yearStart = TimerHelper.GetYearStartTimeWithTimeZone();

        // Assert
        Assert.Equal(1, yearStart.Month);
        Assert.Equal(1, yearStart.Day);
        Assert.Equal(0, yearStart.Hour);
    }

    [Fact]
    public void GetNextYearStart_TimeZone_ShouldReturnJanuary1st()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);

        // Act
        var nextYearStart = TimerHelper.GetNextYearStartTimeWithTimeZone();
        var yearStart = TimerHelper.GetYearStartTimeWithTimeZone();

        // Assert
        Assert.Equal(1, nextYearStart.Month);
        Assert.Equal(1, nextYearStart.Day);
        Assert.Equal(1, nextYearStart.Year - yearStart.Year);
    }

    #endregion

    #region Difference - TimeDifference

    [Fact]
    public void GetTimeDifferenceWithTimeZone_ShouldReturnCorrectDifference()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);
        var now = TimerHelper.GetNowWithTimeZone();
        var startTime = now.AddHours(-2);
        var endTime = now;
        var startTimestamp = TimerHelper.DateTimeToSecondsWithTimeZone(startTime);
        var endTimestamp = TimerHelper.DateTimeToSecondsWithTimeZone(endTime);

        // Act
        var difference = TimerHelper.GetTimeDifferenceWithTimeZone(startTimestamp, endTimestamp);

        // Assert
        Assert.True(Math.Abs(difference.TotalHours - 2) < 0.1, $"Difference should be ~2 hours, got {difference.TotalHours}");
    }

    [Fact]
    public void GetTimeDifferenceMillisecondWithTimeZone_ShouldReturnCorrectDifference()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);
        var now = TimerHelper.GetNowWithTimeZone();
        var startTime = now.AddMilliseconds(-5000);
        var endTime = now;
        var startTimestamp = TimerHelper.TimeToMillisecondsWithTimeZone(startTime);
        var endTimestamp = TimerHelper.TimeToMillisecondsWithTimeZone(endTime);

        // Act
        var difference = TimerHelper.GetTimeDifferenceMillisecondWithTimeZone(startTimestamp, endTimestamp);

        // Assert
        Assert.True(Math.Abs(difference.TotalSeconds - 5) < 1, $"Difference should be ~5 seconds, got {difference.TotalSeconds}");
    }

    #endregion

    #region Difference - FromNow

    [Fact]
    public void GetTimeDifferenceFromNow_DateTime_ShouldReturnCorrectDifference()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);
        var pastTime = TimerHelper.GetNowWithTimeZone().AddHours(-3);

        // Act
        var difference = TimerHelper.GetTimeDifferenceFromNowWithTimeZone(pastTime);

        // Assert
        Assert.True(Math.Abs(difference.TotalHours - 3) < 0.1, $"Difference should be ~3 hours, got {difference.TotalHours}");
    }

    [Fact]
    public void GetTimeDifferenceFromNow_Timestamp_ShouldReturnCorrectDifference()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);
        var pastTime = TimerHelper.GetNowWithTimeZone().AddMinutes(-30);
        var pastTimestamp = new DateTimeOffset(pastTime, _testOffset).ToUnixTimeSeconds();

        // Act
        var difference = TimerHelper.GetTimeDifferenceFromNowWithTimeZone(pastTimestamp);

        // Assert
        Assert.True(Math.Abs(difference.TotalMinutes - 30) < 1, $"Difference should be ~30 minutes, got {difference.TotalMinutes}");
    }

    [Fact]
    public void GetTimeDifferenceFromNowMs_ShouldReturnCorrectDifference()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);
        var pastTime = TimerHelper.GetNowWithTimeZone().AddSeconds(-45);
        var pastTimestamp = new DateTimeOffset(pastTime, _testOffset).ToUnixTimeMilliseconds();

        // Act
        var difference = TimerHelper.GetTimeDifferenceFromNowMsWithTimeZone(pastTimestamp);

        // Assert
        Assert.True(Math.Abs(difference.TotalSeconds - 45) < 2, $"Difference should be ~45 seconds, got {difference.TotalSeconds}");
    }

    [Fact]
    public void GetElapsedSeconds_DateTime_ShouldReturnCorrectSeconds()
    {
        // Arrange
        TimerHelper.SetTimeZone(_testTimeZone);
        var pastTime = TimerHelper.GetNowWithTimeZone().AddSeconds(-120);

        // Act
        var elapsed = TimerHelper.GetElapsedSecondsWithTimeZone(pastTime);

        // Assert
        Assert.True(Math.Abs(elapsed - 120) <= 2, $"Elapsed should be ~120 seconds, got {elapsed}");
    }

    #endregion

    #region Timestamp - TimeSpan

    [Fact]
    public void TimeSpanWithTimestamp_UtcAndTimeZone_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        const long timestamp = 3600; // 1小时

        // Act
        var timeSpanUtc = TimerHelper.TimeSpanWithTimestampUtc(timestamp);
        var timeSpanZone = TimerHelper.TimeSpanWithTimestampWithTimeZone(timestamp);

        // Assert
        Assert.Equal(TimeSpan.FromHours(1), timeSpanUtc);
        Assert.Equal(TimeSpan.FromHours(1), timeSpanZone);
    }

    [Fact]
    public void TimeSpanWithTimestamp_WithZero_ShouldReturnZero()
    {
        // Arrange
        const long timestamp = 0;

        // Act
        var timeSpanUtc = TimerHelper.TimeSpanWithTimestampUtc(timestamp);
        var timeSpanZone = TimerHelper.TimeSpanWithTimestampWithTimeZone(timestamp);

        // Assert
        Assert.Equal(TimeSpan.Zero, timeSpanUtc);
        Assert.Equal(TimeSpan.Zero, timeSpanZone);
    }

    [Fact]
    public void TimeSpanWithTimestamp_WithOutOfRange_ShouldThrowException()
    {
        // Arrange
        const long invalidTimestamp = long.MinValue;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimeSpanWithTimestampUtc(invalidTimestamp));
        Assert.Throws<ArgumentOutOfRangeException>(() => TimerHelper.TimeSpanWithTimestampWithTimeZone(invalidTimestamp));
    }

    #endregion

}
