using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 进程内幂等存储测试：并发恰一、过期覆盖、过期清理、完结回读。
/// </summary>
public class InMemoryIdempotencyStoreTests
{
    private const string TestScope = "order";
    private const string TestKey = "key-1";

    private static IdempotencyRecord CreateRecord(string requestDigest, long createdTime, long expiredTime, IdempotencyStatus status = IdempotencyStatus.Processing)
    {
        return new IdempotencyRecord(TestScope, TestKey, requestDigest, status, null, createdTime, expiredTime, null);
    }

    private static IdempotencyRecordIdentifier CreateIdentifier()
    {
        return new IdempotencyRecordIdentifier(TestScope, TestKey);
    }

    [Fact]
    public async Task TryBeginProcessingAsync_OneHundredConcurrentSameKey_ExactlyOneShouldSucceed()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        const int ConcurrencyLevel = 100;

        // 执行：100 个并发占位同键同时间基准
        Task<bool>[] occupyTasks = Enumerable.Range(0, ConcurrencyLevel)
            .Select(_ => Task.Run(() => idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1000, 2000))))
            .ToArray();
        bool[] occupyResults = await Task.WhenAll(occupyTasks);

        // 断言：并发恰一
        Assert.Equal(1, occupyResults.Count(succeeded => succeeded));
        Assert.Equal(ConcurrencyLevel - 1, occupyResults.Count(succeeded => !succeeded));
    }

    [Fact]
    public async Task TryBeginProcessingAsync_ExistingRecordUnexpired_ShouldFail()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1000, 2000)));

        // 执行：新占位的时间基准未越过过期时刻
        bool occupied = await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-beta", 1500, 2500));

        // 断言
        Assert.False(occupied);
    }

    [Fact]
    public async Task TryBeginProcessingAsync_ExistingRecordExpired_ShouldOverwrite()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1000, 2000)));

        // 执行：新占位的时间基准越过过期时刻
        bool occupied = await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-beta", 2001, 3001));

        // 断言：过期记录被覆盖
        Assert.True(occupied);
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(record);
        Assert.Equal("digest-beta", record!.RequestDigest);
        Assert.Equal(2001, record.CreatedTime);
    }

    [Fact]
    public async Task TryBeginProcessingAsync_ExistingFailedRecordUnexpired_ShouldOverwriteForReExecute()
    {
        // 安排：占位后落为失败（未过期）
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1000, 9000)));
        await idempotencyStore.FailAsync(new IdempotencyFailureRequest(CreateIdentifier(), 1200));

        // 执行：失败重执行策略下的新占位（时间基准未过期）
        bool occupied = await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1500, 9500));

        // 断言：失败记录被覆盖为新的执行中记录
        Assert.True(occupied);
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Processing, record!.Status);
        Assert.Equal(1500, record.CreatedTime);
    }

    [Fact]
    public async Task CompleteAsync_ShouldPersistCompletedStatusAndFirstResponse()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1000, 2000)));
        byte[] firstResponseBytes = new byte[] { 1, 2, 3, 4 };

        // 执行
        await idempotencyStore.CompleteAsync(new IdempotencyCompletionRequest(CreateIdentifier(), firstResponseBytes, 1800));

        // 断言：完结回读
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Completed, record!.Status);
        Assert.NotNull(record.FirstResponse);
        Assert.Equal(firstResponseBytes, record.FirstResponse!.Value.ToArray());
        Assert.Equal(1800, record.FinishedTime);
        Assert.Equal(1000, record.CreatedTime);
        Assert.Equal(2000, record.ExpiredTime);
    }

    [Fact]
    public async Task CompleteAsync_MissingRecord_ShouldThrowInvalidOperationException()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();

        // 执行 + 断言
        await Assert.ThrowsAsync<InvalidOperationException>(() => idempotencyStore.CompleteAsync(new IdempotencyCompletionRequest(CreateIdentifier(), new byte[] { 1 }, 1000)));
    }

    [Fact]
    public async Task FailAsync_ShouldPersistFailedStatus()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(CreateRecord("digest-alpha", 1000, 2000)));

        // 执行
        await idempotencyStore.FailAsync(new IdempotencyFailureRequest(CreateIdentifier(), 1700));

        // 断言
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Failed, record!.Status);
        Assert.Equal(1700, record.FinishedTime);
    }

    [Fact]
    public async Task FailAsync_MissingRecord_ShouldThrowInvalidOperationException()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();

        // 执行 + 断言
        await Assert.ThrowsAsync<InvalidOperationException>(() => idempotencyStore.FailAsync(new IdempotencyFailureRequest(CreateIdentifier(), 1000)));
    }

    [Fact]
    public async Task TryGetRecordAsync_MissingRecord_ShouldReturnNull()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();

        // 执行
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());

        // 断言
        Assert.Null(record);
    }

    [Fact]
    public async Task RemoveExpiredRecordsAsync_ShouldRemoveOnlyExpiredRecords()
    {
        // 安排：三条记录，两条过期（不同键）、一条未过期
        InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(new IdempotencyRecord(TestScope, "key-expired-1", "digest", IdempotencyStatus.Completed, null, 1000, 1500, 1200)));
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(new IdempotencyRecord(TestScope, "key-expired-2", "digest", IdempotencyStatus.Failed, null, 1000, 1600, 1300)));
        Assert.True(await idempotencyStore.TryBeginProcessingAsync(new IdempotencyRecord(TestScope, "key-alive", "digest", IdempotencyStatus.Completed, null, 1000, 5000, 1200)));

        // 执行：当前时刻为 1550，仅第一条过期
        int removedCount = await idempotencyStore.RemoveExpiredRecordsAsync(new ExpiredRecordRemovalRequest(1550));

        // 断言
        Assert.Equal(1, removedCount);
        Assert.Null(await idempotencyStore.TryGetRecordAsync(new IdempotencyRecordIdentifier(TestScope, "key-expired-1")));
        Assert.NotNull(await idempotencyStore.TryGetRecordAsync(new IdempotencyRecordIdentifier(TestScope, "key-expired-2")));
        Assert.NotNull(await idempotencyStore.TryGetRecordAsync(new IdempotencyRecordIdentifier(TestScope, "key-alive")));

        // 再推进到 1700：第二条也过期，未过期记录保留
        int secondRemovedCount = await idempotencyStore.RemoveExpiredRecordsAsync(new ExpiredRecordRemovalRequest(1700));
        Assert.Equal(1, secondRemovedCount);
        Assert.NotNull(await idempotencyStore.TryGetRecordAsync(new IdempotencyRecordIdentifier(TestScope, "key-alive")));
    }
}
