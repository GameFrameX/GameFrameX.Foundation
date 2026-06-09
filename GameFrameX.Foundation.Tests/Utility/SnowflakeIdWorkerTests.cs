using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake.WorkerIdProviders;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

public sealed class SnowflakeIdWorkerTests
{
    private static IdWorker CreateWorker(long workerId = 1, long dataCenterId = 1)
    {
        return new IdWorker(workerId, dataCenterId);
    }

    [Fact]
    public void ManualWorkerIdProvider_ShouldReturnConfiguredId()
    {
        var provider = new ManualWorkerIdProvider(5);
        Assert.Equal(5, provider.GetWorkerId());
        Assert.Equal("Manual", provider.Name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    public void ManualWorkerIdProvider_OutOfRange_ShouldThrow(long id)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ManualWorkerIdProvider(id));
    }

    [Fact]
    public void HostNameWorkerIdProvider_ShouldReturn0To31()
    {
        var provider = new HostNameWorkerIdProvider();
        var id = provider.GetWorkerId();
        Assert.InRange(id, 0, 31);
        Assert.Equal("HostName", provider.Name);
    }

    [Fact]
    public void ProcessIdWorkerIdProvider_ShouldReturn0To31()
    {
        var provider = new ProcessIdWorkerIdProvider();
        var id = provider.GetWorkerId();
        Assert.InRange(id, 0, 31);
        Assert.Equal("ProcessId", provider.Name);
    }

    [Fact]
    public void IpAddressWorkerIdProvider_ShouldReturn0To31()
    {
        var provider = new IpAddressWorkerIdProvider();
        var id = provider.GetWorkerId();
        Assert.InRange(id, 0, 31);
        Assert.Equal("IpAddress", provider.Name);
    }

    [Fact]
    public void IdWorker_TotalIdsGenerated_ShouldIncrement()
    {
        var worker = CreateWorker();
        Assert.Equal(0, worker.TotalIdsGenerated);

        worker.NextId();
        worker.NextId();
        worker.NextId();

        Assert.Equal(3, worker.TotalIdsGenerated);
    }

    [Fact]
    public void IdWorker_LastGeneratedId_ShouldMatchLastId()
    {
        var worker = CreateWorker();
        Assert.Equal(0, worker.LastGeneratedId);

        var id1 = worker.NextId();
        Assert.Equal(id1, worker.LastGeneratedId);

        var id2 = worker.NextId();
        Assert.Equal(id2, worker.LastGeneratedId);
    }

    [Fact]
    public void IdWorker_ClockBackwardCount_ShouldIncrement()
    {
        var worker = CreateWorker();
        Assert.Equal(0, worker.ClockBackwardCount);

        var firstId = worker.NextId();
        var pastTime = IdWorker.DefaultBaseTime;
        using (TimeSystem.StubCurrentTime(pastTime))
        {
            Assert.Throws<InvalidSystemClock>(() => worker.NextId());
        }

        Assert.Equal(1, worker.ClockBackwardCount);
    }

    [Fact]
    public void IdWorker_NextId_IsAlwaysPositive()
    {
        var worker = CreateWorker();
        for (int i = 0; i < 100; i++)
        {
            Assert.True(worker.NextId() > 0);
        }
    }

    [Fact]
    public void IdWorker_NextId_IsMonotonicallyIncreasing()
    {
        var worker = CreateWorker();
        var previous = worker.NextId();
        for (int i = 0; i < 100; i++)
        {
            var current = worker.NextId();
            Assert.True(current > previous, $"Expected {current} > {previous}");
            previous = current;
        }
    }

    [Fact]
    public void IdWorker_HighConcurrency_GeneratesUniqueIds()
    {
        var worker = CreateWorker();
        var ids = new System.Collections.Concurrent.ConcurrentDictionary<long, bool>();

        Parallel.For(0, 1000, i =>
        {
            var id = worker.NextId();
            Assert.True(ids.TryAdd(id, true), $"Duplicate ID generated: {id}");
        });

        Assert.Equal(1000, ids.Count);
    }

    [Fact]
    public void IdWorker_Diagnostics_AreConsistent()
    {
        var worker = CreateWorker();
        var count = 50;

        for (int i = 0; i < count; i++)
        {
            worker.NextId();
        }

        Assert.Equal(count, worker.TotalIdsGenerated);
        Assert.True(worker.LastGeneratedId > 0);
        Assert.Equal(0, worker.ClockBackwardCount);
    }
}
