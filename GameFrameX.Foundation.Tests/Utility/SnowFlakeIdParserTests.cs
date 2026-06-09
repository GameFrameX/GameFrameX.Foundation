using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

public sealed class SnowFlakeIdParserTests
{
    private const long TestBaseTime = IdWorker.DefaultBaseTime;

    private static IdWorker CreateWorker(long workerId = 5, long dataCenterId = 3)
    {
        return new IdWorker(workerId, dataCenterId, TestBaseTime);
    }

    [Fact]
    public void Parse_KnownId_ShouldExtractCorrectComponents()
    {
        var worker = CreateWorker(workerId: 5, dataCenterId: 3);
        var id = worker.NextId();

        var info = SnowFlakeIdParser.Parse(id, TestBaseTime);

        Assert.Equal(id, info.Id);
        Assert.Equal(5, info.WorkerId);
        Assert.Equal(3, info.DataCenterId);
        Assert.InRange(info.Sequence, 0, 4095);
    }

    [Fact]
    public void Parse_GeneratedId_TimestampShouldMatch()
    {
        var fixedTime = TestBaseTime + 60000L;
        using (TimeSystem.StubCurrentTime(fixedTime))
        {
            var worker = CreateWorker();
            var id = worker.NextId();
            var info = SnowFlakeIdParser.Parse(id, TestBaseTime);

            var expectedTimestamp = DateTimeOffset.FromUnixTimeMilliseconds(fixedTime);
            Assert.InRange(info.Timestamp, expectedTimestamp.AddMilliseconds(-1), expectedTimestamp.AddMilliseconds(1));
        }
    }

    [Fact]
    public void Parse_GeneratedIds_SequenceShouldIncrement()
    {
        var fixedTime = TestBaseTime + 60000L;
        using (TimeSystem.StubCurrentTime(fixedTime))
        {
            var worker = CreateWorker();
            var id1 = worker.NextId();
            var id2 = worker.NextId();

            var info1 = SnowFlakeIdParser.Parse(id1, TestBaseTime);
            var info2 = SnowFlakeIdParser.Parse(id2, TestBaseTime);

            Assert.Equal(0, info1.Sequence);
            Assert.Equal(1, info2.Sequence);
        }
    }

    [Fact]
    public void Parse_WithDefaultBaseTime_ShouldWork()
    {
        var worker = CreateWorker();
        var id = worker.NextId();

        var info = SnowFlakeIdParser.Parse(id);

        Assert.Equal(id, info.Id);
        Assert.Equal(5, info.WorkerId);
        Assert.Equal(3, info.DataCenterId);
    }

    [Fact]
    public void Parse_WithCustomBaseTime_ShouldUseProvidedBaseTime()
    {
        var customBaseTime = 1700000000000L;
        var worker = new IdWorker(7, 2, customBaseTime);
        var id = worker.NextId();

        var info = SnowFlakeIdParser.Parse(id, customBaseTime);

        Assert.Equal(id, info.Id);
        Assert.Equal(7, info.WorkerId);
        Assert.Equal(2, info.DataCenterId);
        Assert.True(info.Timestamp > DateTimeOffset.FromUnixTimeMilliseconds(customBaseTime));
    }

    [Fact]
    public void IdWorker_ParseId_ShouldMatchStaticParser()
    {
        var worker = CreateWorker();
        var id = worker.NextId();

        var fromInstance = worker.ParseId(id);
        var fromStatic = SnowFlakeIdParser.Parse(id, TestBaseTime);

        Assert.Equal(fromStatic.Id, fromInstance.Id);
        Assert.Equal(fromStatic.Timestamp, fromInstance.Timestamp);
        Assert.Equal(fromStatic.WorkerId, fromInstance.WorkerId);
        Assert.Equal(fromStatic.DataCenterId, fromInstance.DataCenterId);
        Assert.Equal(fromStatic.Sequence, fromInstance.Sequence);
    }

    [Fact]
    public void Parse_AllFields_AreWithinValidRanges()
    {
        var worker = CreateWorker();
        var id = worker.NextId();
        var info = SnowFlakeIdParser.Parse(id);

        Assert.InRange(info.WorkerId, 0, 31);
        Assert.InRange(info.DataCenterId, 0, 31);
        Assert.InRange(info.Sequence, 0, 4095);
        Assert.True(info.Id > 0);
    }
}
