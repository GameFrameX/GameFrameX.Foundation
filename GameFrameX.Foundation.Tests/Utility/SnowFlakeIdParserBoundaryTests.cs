using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

/// <summary>
/// SnowFlakeIdParser 边界单元测试：覆盖极端 ID（0、long.MaxValue、long.MinValue、-1）解析行为、
/// 自定义 baseTime 往返。位宽：1 符号位 | 41 时间戳 | 5 数据中心 | 5 工作节点 | 12 序列号。
/// </summary>
[Collection("Snowflake")]
public sealed class SnowFlakeIdParserBoundaryTests
{
    [Fact]
    public void Parse_IdZero_AllFieldsShouldBeZero()
    {
        // Arrange
        const long id = 0L;

        // Act
        var info = SnowFlakeIdParser.Parse(id);

        // Assert
        Assert.Equal(0, info.Id);
        Assert.Equal(0, info.WorkerId);
        Assert.Equal(0, info.DataCenterId);
        Assert.Equal(0, info.Sequence);
        // timestamp = (0 >> 22) + DefaultBaseTime = DefaultBaseTime（2025-01-01 00:00:00 UTC）
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(IdWorker.DefaultBaseTime), info.Timestamp);
    }

    [Fact]
    public void Parse_IdMaxValue_AllSubFieldsShouldBeAtBitWidthMax()
    {
        // Arrange
        const long id = long.MaxValue;

        // Act
        var info = SnowFlakeIdParser.Parse(id);

        // Assert
        Assert.Equal(id, info.Id);
        // long.MaxValue 的非符号位全为 1，掩码后各字段取上限
        Assert.Equal(31, info.WorkerId);
        Assert.Equal(31, info.DataCenterId);
        Assert.Equal(4095, info.Sequence);
        // timestamp = (long.MaxValue >> 22) + DefaultBaseTime，约 2094 年，在合法范围内
        var expectedTimestampMs = (long.MaxValue >> 22) + IdWorker.DefaultBaseTime;
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(expectedTimestampMs), info.Timestamp);
    }

    [Fact]
    public void Parse_IdMinusOne_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange — 负数 ID 无意义（雪花 ID 由时间戳 + 位字段构成，恒为非负）
        const long id = -1L;

        // Act & Assert — 负 ID 是非法输入，必须快速失败而非产生 baseTime 之前的无意义时间戳
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => SnowFlakeIdParser.Parse(id));
        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Parse_IdMinValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange — long.MinValue 符号位为 1，是非负约束下最严重的非法输入
        const long id = long.MinValue;

        // Act & Assert — 负 ID 必须被拒绝，不能产生约 1955 年的无意义时间戳
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => SnowFlakeIdParser.Parse(id));
        Assert.Equal("id", ex.ParamName);
    }

    [Fact]
    public void Parse_CustomBaseTime_RoundTrip_ShouldExtractCorrectComponents()
    {
        // Arrange
        const long customBaseTime = 1700000000000L; // 2023-11-14 22:13:20 UTC
        var worker = new IdWorker(7, 2, customBaseTime);

        // Act
        var id = worker.NextId();
        var info = SnowFlakeIdParser.Parse(id, customBaseTime);

        // Assert - 往返：生成 → 解析 → 字段一致
        Assert.Equal(id, info.Id);
        Assert.Equal(7, info.WorkerId);
        Assert.Equal(2, info.DataCenterId);
        Assert.InRange(info.Sequence, 0, 4095);
        Assert.True(info.Timestamp >= DateTimeOffset.FromUnixTimeMilliseconds(customBaseTime));
    }

    [Fact]
    public void Parse_DefaultBaseTime_ShouldMatchExplicitDefaultBaseTime()
    {
        // Arrange
        const long id = 12345L;

        // Act
        var withDefault = SnowFlakeIdParser.Parse(id);
        var withExplicit = SnowFlakeIdParser.Parse(id, IdWorker.DefaultBaseTime);

        // Assert - 两种调用方式结果完全一致
        Assert.Equal(withExplicit.Id, withDefault.Id);
        Assert.Equal(withExplicit.Timestamp, withDefault.Timestamp);
        Assert.Equal(withExplicit.WorkerId, withDefault.WorkerId);
        Assert.Equal(withExplicit.DataCenterId, withDefault.DataCenterId);
        Assert.Equal(withExplicit.Sequence, withDefault.Sequence);
    }
}
