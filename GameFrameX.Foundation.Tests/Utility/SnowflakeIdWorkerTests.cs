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
    public void HostNameWorkerIdProvider_ComputeWorkerId_ShouldBeStable()
    {
        var first = HostNameWorkerIdProvider.ComputeWorkerId("gameframex-node-01");
        var second = HostNameWorkerIdProvider.ComputeWorkerId("gameframex-node-01");

        Assert.Equal(first, second);
        Assert.InRange(first, 0, 31);
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

    [Fact]
    public void WorkerIdConflictDetector_RegisterDuplicateNode_ShouldReportConflict()
    {
        var detector = new WorkerIdConflictDetector();

        var first = detector.Register("node-a", 1, 2);
        var second = detector.Register("node-b", 1, 2);

        Assert.False(first.HasConflict);
        Assert.True(second.HasConflict);
        Assert.Equal("node-a", second.ConflictingNodeId);
        Assert.Equal(1, second.DataCenterId);
        Assert.Equal(2, second.WorkerId);
    }

    [Fact]
    public void WorkerIdConflictDetector_RegisterSameNodeAgain_ShouldNotReportConflict()
    {
        var detector = new WorkerIdConflictDetector();

        detector.Register("node-a", 1, 2);
        var result = detector.Register("node-a", 1, 2);

        Assert.False(result.HasConflict);
        Assert.Equal("node-a", result.NodeId);
    }

    [Fact]
    public void WorkerIdConflictDetector_Check_ShouldNotMutateRegistry()
    {
        var detector = new WorkerIdConflictDetector();

        var firstCheck = detector.Check("node-a", 1, 2);
        var secondCheck = detector.Check("node-b", 1, 2);

        Assert.False(firstCheck.HasConflict);
        Assert.False(secondCheck.HasConflict);
        Assert.Empty(detector.GetRegistrations());
    }

    [Fact]
    public void SnowFlakeIdHelper_CheckWorkerIdConflict_ShouldReportRegisteredDuplicate()
    {
        var originalWorkId = SnowFlakeIdHelper.WorkId;
        var originalDataCenterId = SnowFlakeIdHelper.DataCenterId;
        var detector = new WorkerIdConflictDetector();

        try
        {
            SnowFlakeIdHelper.SetWorkerIdProvider(null);
            SnowFlakeIdHelper.SetWorkerIdConflictDetector(detector);
            SnowFlakeIdHelper.WorkId = 2;
            SnowFlakeIdHelper.DataCenterId = 1;

            detector.Register("node-a", 1, 2, "Manual");
            var result = SnowFlakeIdHelper.CheckWorkerIdConflict("node-b");

            Assert.True(result.HasConflict);
            Assert.Equal("node-a", result.ConflictingNodeId);
            Assert.Equal(1, result.DataCenterId);
            Assert.Equal(2, result.WorkerId);
            Assert.Equal("Manual", result.ProviderName);
        }
        finally
        {
            SnowFlakeIdHelper.WorkId = originalWorkId;
            SnowFlakeIdHelper.DataCenterId = originalDataCenterId;
            SnowFlakeIdHelper.SetWorkerIdProvider(null);
            SnowFlakeIdHelper.SetWorkerIdConflictDetector(null);
        }
    }
}
