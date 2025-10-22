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
/// TimeHelper 类的单元测试
/// </summary>
public class TimeHelperTests
{
    /// <summary>
    /// 测试时间常量的正确性
    /// </summary>
    [Fact]
    public void TimeConstants_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local), TimerHelper.EpochLocal);
        Assert.Equal(TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Utc), TimerHelper.EpochUtc);
        Assert.Equal(0L, TimerHelper.TimeOffsetSeconds);
        Assert.Equal(0L, TimerHelper.TimeOffsetMilliseconds);
    }

    /// <summary>
    /// 测试 SetTimeOffset 方法设置秒级偏移
    /// </summary>
    [Fact]
    public void SetTimeOffset_WithSeconds_ShouldSetCorrectOffset()
    {
        // Arrange
        const long offsetSeconds = 3600; // 1 hour

        // Act
        TimerHelper.SetTimeOffset(offsetSeconds, offsetSeconds * 1000);

        // Assert
        Assert.Equal(offsetSeconds, TimerHelper.TimeOffsetSeconds);
        Assert.Equal(offsetSeconds * 1000, TimerHelper.TimeOffsetMilliseconds);

        // Cleanup
        TimerHelper.ResetTimeOffset();
    }

    /// <summary>
    /// 测试 SetTimeOffset 方法设置毫秒级偏移
    /// </summary>
    [Fact]
    public void SetTimeOffset_WithMilliseconds_ShouldSetCorrectOffset()
    {
        // Arrange
        const long offsetMilliseconds = 3600000; // 1 hour in milliseconds

        // Act
        TimerHelper.SetTimeOffset(offsetMilliseconds / 1000, offsetMilliseconds);

        // Assert
        Assert.Equal(offsetMilliseconds / 1000, TimerHelper.TimeOffsetSeconds);
        Assert.Equal(offsetMilliseconds, TimerHelper.TimeOffsetMilliseconds);

        // Cleanup
        TimerHelper.ResetTimeOffset();
    }

    /// <summary>
    /// 测试 ResetTimeOffset 方法
    /// </summary>
    [Fact]
    public void ResetTimeOffset_ShouldResetToZero()
    {
        // Arrange
        TimerHelper.SetTimeOffset(3600, 0);

        // Act
        TimerHelper.ResetTimeOffset();

        // Assert
        Assert.Equal(0L, TimerHelper.TimeOffsetSeconds);
        Assert.Equal(0L, TimerHelper.TimeOffsetMilliseconds);
    }

    /// <summary>
    /// 测试 UnixTimeSeconds 方法
    /// </summary>
    [Fact]
    public void UnixTimeSeconds_ShouldReturnCorrectValue()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var expectedUnixTime = new DateTimeOffset(now).ToUnixTimeSeconds() + TimerHelper.TimeOffsetSeconds;

        // Act
        var actualUnixTime = TimerHelper.UnixTimeSeconds();

        // Assert
        // 允许1秒的误差，因为测试执行需要时间
        Assert.True(System.Math.Abs(actualUnixTime - expectedUnixTime) <= 1);
    }

    /// <summary>
    /// 测试 UnixTimeMilliseconds 方法
    /// </summary>
    [Fact]
    public void UnixTimeMilliseconds_ShouldReturnCorrectValue()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var expectedUnixTime = new DateTimeOffset(now).ToUnixTimeMilliseconds() + TimerHelper.TimeOffsetMilliseconds;

        // Act
        var actualUnixTime = TimerHelper.UnixTimeMilliseconds();

        // Assert
        // 允许1000毫秒的误差，因为测试执行需要时间
        Assert.True(System.Math.Abs(actualUnixTime - expectedUnixTime) <= 1000);
    }

    /// <summary>
    /// 测试 TimeSeconds 方法
    /// </summary>
    [Fact]
    public void TimeSeconds_ShouldReturnCorrectValue()
    {
        // Arrange
        var now = DateTime.Now;
        var expectedTime = new DateTimeOffset(now).ToUnixTimeSeconds() + TimerHelper.TimeOffsetSeconds;

        // Act
        var actualTime = TimerHelper.TimeSecondsWithOffset();

        // Assert
        // 允许1秒的误差
        Assert.True(System.Math.Abs(actualTime - expectedTime) <= 1);
    }

    /// <summary>
    /// 测试 TimeMilliseconds 方法
    /// </summary>
    [Fact]
    public void TimeMilliseconds_ShouldReturnCorrectValue()
    {
        // Arrange
        var now = DateTime.Now;
        var expectedTime = new DateTimeOffset(now).ToUnixTimeMilliseconds() + TimerHelper.TimeOffsetMilliseconds;

        // Act
        var actualTime = TimerHelper.TimeMillisecondsWithOffset();

        // Assert
        // 允许1000毫秒的误差
        Assert.True(System.Math.Abs(actualTime - expectedTime) <= 1000);
    }

    /// <summary>
    /// 测试 TimeToMilliseconds 方法转换DateTime
    /// </summary>
    [Fact]
    public void TimeToMilliseconds_WithDateTime_ShouldReturnCorrectValue()
    {
        // Arrange
        var testTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Local);
        var expected = (long)(testTime - TimerHelper.EpochLocal).TotalMilliseconds;

        // Act
        var actual = TimerHelper.TimeToMilliseconds(testTime);

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// 测试 TimeToSecond 方法转换DateTime
    /// </summary>
    [Fact]
    public void TimeToSecond_WithDateTime_ShouldReturnCorrectValue()
    {
        // Arrange
        var testTime = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Local);
        var expected = (long)(testTime - TimerHelper.EpochLocal).TotalSeconds;

        // Act
        var actual = TimerHelper.TimeToSecond(testTime);

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// 测试 TimestampToTicks 方法
    /// </summary>
    [Fact]
    public void TimestampToTicks_ShouldReturnCorrectValue()
    {
        // Arrange
        const long timestamp = 1672574400; // 2023-01-01 12:00:00 UTC in seconds
        var expectedDateTime = TimerHelper.EpochUtc.AddSeconds(timestamp);
        var expectedTicks = expectedDateTime.Ticks;

        // Act
        var actualTicks = TimerHelper.TimestampToTicks(timestamp);

        // Assert
        Assert.Equal(expectedTicks, actualTicks);
    }

    /// <summary>
    /// 测试 TimestampMillisToTicks 方法
    /// </summary>
    [Fact]
    public void TimestampMillisToTicks_ShouldReturnCorrectValue()
    {
        // Arrange
        const long timestampMillis = 1672574400000; // 2023-01-01 12:00:00 UTC in milliseconds
        var expectedDateTime = TimerHelper.EpochUtc.AddMilliseconds(timestampMillis);
        var expectedTicks = expectedDateTime.Ticks;

        // Act
        var actualTicks = TimerHelper.TimestampMillisToTicks(timestampMillis);

        // Assert
        Assert.Equal(expectedTicks, actualTicks);
    }

    /// <summary>
    /// 测试 TimeSpanWithTimestamp 方法
    /// </summary>
    [Fact]
    public void TimeSpanWithTimestamp_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        const long timestamp = 1672574400; // 2023-01-01 12:00:00 UTC in seconds
        var expectedDateTime = TimerHelper.EpochUtc.AddSeconds(timestamp);
        var expectedTimeSpan = expectedDateTime - TimerHelper.EpochUtc;

        // Act
        var actualTimeSpan = TimerHelper.TimeSpanWithTimestamp(timestamp);

        // Assert
        Assert.Equal(expectedTimeSpan, actualTimeSpan);
    }

    /// <summary>
    /// 测试 TimeSpanLocalWithTimestamp 方法
    /// </summary>
    [Fact]
    public void TimeSpanLocalWithTimestamp_ShouldReturnCorrectTimeSpan()
    {
        // Arrange
        const long timestamp = 1672574400; // seconds
        var expectedDateTime = TimerHelper.EpochLocal.AddSeconds(timestamp);
        var expectedTimeSpan = expectedDateTime - TimerHelper.EpochLocal;

        // Act
        var actualTimeSpan = TimerHelper.TimeSpanLocalWithTimestamp(timestamp);

        // Assert
        Assert.Equal(expectedTimeSpan, actualTimeSpan);
    }

    /// <summary>
    /// 测试 GetTimeDifference 方法比较两个DateTime
    /// </summary>
    [Fact]
    public void GetTimeDifference_WithDateTimes_ShouldReturnCorrectDifference()
    {
        // Arrange
        var startTime = new DateTime(2023, 1, 1, 12, 0, 0);
        var endTime = new DateTime(2023, 1, 1, 13, 30, 45);
        var expectedDifference = endTime - startTime;

        // Act
        var actualDifference = TimerHelper.GetTimeDifference(startTime, endTime);

        // Assert
        Assert.Equal(expectedDifference, actualDifference);
    }

    /// <summary>
    /// 测试 GetTimeDifference 方法比较两个秒级时间戳
    /// </summary>
    [Fact]
    public void GetTimeDifference_WithSecondTimestamps_ShouldReturnCorrectDifference()
    {
        // Arrange
        const long startTimestamp = 1672574400; // 2023-01-01 12:00:00
        const long endTimestamp = 1672579845; // 2023-01-01 13:30:45
        var expectedDifference = TimeSpan.FromSeconds(endTimestamp - startTimestamp);

        // Act
        var actualDifference = TimerHelper.GetTimeDifference(startTimestamp, endTimestamp);

        // Assert
        Assert.Equal(expectedDifference, actualDifference);
    }

    /// <summary>
    /// 测试 GetTimeDifferenceMs 方法比较两个毫秒级时间戳
    /// </summary>
    [Fact]
    public void GetTimeDifference_WithMillisecondTimestamps_ShouldReturnCorrectDifference()
    {
        // Arrange
        const long startTimestamp = 1672574400000; // 2023-01-01 12:00:00
        const long endTimestamp = 1672579845000; // 2023-01-01 13:30:45
        var expectedDifference = TimeSpan.FromMilliseconds(endTimestamp - startTimestamp);

        // Act
        var actualDifference = TimerHelper.GetTimeDifferenceMs(startTimestamp, endTimestamp, true);

        // Assert
        Assert.Equal(expectedDifference, actualDifference);
    }

    /// <summary>
    /// 测试 GetTimeDifferenceFromNow 方法
    /// </summary>
    [Fact]
    public void GetTimeDifferenceFromNow_WithDateTime_ShouldReturnCorrectDifference()
    {
        // Arrange
        var pastTime = DateTime.Now.AddHours(-1);

        // Act
        var difference = TimerHelper.GetTimeDifferenceFromNow(pastTime);

        // Assert
        Assert.True(difference.TotalHours >= 0.9 && difference.TotalHours <= 1.1); // 允许一些误差
    }

    /// <summary>
    /// 测试 GetSecondsDifference 方法
    /// </summary>
    [Fact]
    public void GetSecondsDifference_ShouldReturnCorrectValue()
    {
        // Arrange
        var startTime = new DateTime(2023, 1, 1, 12, 0, 0);
        var endTime = new DateTime(2023, 1, 1, 12, 1, 30); // 90 seconds later

        // Act
        var difference = TimerHelper.GetSecondsDifference(startTime, endTime);

        // Assert
        Assert.Equal(90, difference);
    }

    /// <summary>
    /// 测试 GetMillisecondsDifference 方法
    /// </summary>
    [Fact]
    public void GetMillisecondsDifference_ShouldReturnCorrectValue()
    {
        // Arrange
        var startTime = new DateTime(2023, 1, 1, 12, 0, 0, 0);
        var endTime = new DateTime(2023, 1, 1, 12, 0, 1, 500); // 1500 milliseconds later

        // Act
        var difference = TimerHelper.GetMillisecondsDifference(startTime, endTime);

        // Assert
        Assert.Equal(1500, difference);
    }

    /// <summary>
    /// 测试 GetMinutesDifference 方法
    /// </summary>
    [Fact]
    public void GetMinutesDifference_ShouldReturnCorrectValue()
    {
        // Arrange
        var startTime = new DateTime(2023, 1, 1, 12, 0, 0);
        var endTime = new DateTime(2023, 1, 1, 12, 30, 0); // 30 minutes later

        // Act
        var difference = TimerHelper.GetMinutesDifference(startTime, endTime);

        // Assert
        Assert.Equal(30, difference);
    }

    /// <summary>
    /// 测试 GetHoursDifference 方法
    /// </summary>
    [Fact]
    public void GetHoursDifference_ShouldReturnCorrectValue()
    {
        // Arrange
        var startTime = new DateTime(2023, 1, 1, 12, 0, 0);
        var endTime = new DateTime(2023, 1, 1, 15, 0, 0); // 3 hours later

        // Act
        var difference = TimerHelper.GetHoursDifference(startTime, endTime);

        // Assert
        Assert.Equal(3, difference);
    }

    /// <summary>
    /// 测试 GetDaysDifference 方法
    /// </summary>
    [Fact]
    public void GetDaysDifference_ShouldReturnCorrectValue()
    {
        // Arrange
        var startTime = new DateTime(2023, 1, 1, 12, 0, 0);
        var endTime = new DateTime(2023, 1, 4, 12, 0, 0); // 3 days later

        // Act
        var difference = TimerHelper.GetDaysDifference(startTime, endTime);

        // Assert
        Assert.Equal(3, difference);
    }

    /// <summary>
    /// 测试时间偏移对各种方法的影响
    /// </summary>
    [Fact]
    public void TimeOffset_ShouldAffectTimeMethods()
    {
        // Arrange
        const long offsetSeconds = 3600; // 1 hour
        var originalUnixTime = TimerHelper.UnixTimeSeconds();
        var originalTime = TimerHelper.TimeSecondsWithOffset();

        // Act
        TimerHelper.SetTimeOffset(offsetSeconds, offsetSeconds * 1000);
        var offsetUnixTime = TimerHelper.UnixTimeSeconds();
        var offsetTime = TimerHelper.TimeSecondsWithOffset();

        // Assert
        Assert.Equal((double)offsetSeconds, (double)(offsetUnixTime - originalUnixTime), precision: 1); // 允许1秒误差
        Assert.Equal((double)offsetSeconds, (double)(offsetTime - originalTime), precision: 1); // 允许1秒误差

        // Cleanup
        TimerHelper.ResetTimeOffset();
    }

    /// <summary>
    /// 测试边界情况：负时间戳
    /// </summary>
    [Fact]
    public void TimestampToTicks_WithNegativeTimestamp_ShouldReturnCorrectValue()
    {
        // Arrange
        const long negativeTimestamp = -3600; // 1 hour before epoch
        var expectedDateTime = TimerHelper.EpochUtc.AddSeconds(negativeTimestamp);
        var expectedTicks = expectedDateTime.Ticks;

        // Act
        var actualTicks = TimerHelper.TimestampToTicks(negativeTimestamp);

        // Assert
        Assert.Equal(expectedTicks, actualTicks);
    }

    /// <summary>
    /// 测试边界情况：零时间戳
    /// </summary>
    [Fact]
    public void TimestampToTicks_WithZeroTimestamp_ShouldReturnEpochTicks()
    {
        // Arrange
        const long zeroTimestamp = 0;
        var expectedTicks = TimerHelper.EpochUtc.Ticks;

        // Act
        var actualTicks = TimerHelper.TimestampToTicks(zeroTimestamp);

        // Assert
        Assert.Equal(expectedTicks, actualTicks);
    }
}