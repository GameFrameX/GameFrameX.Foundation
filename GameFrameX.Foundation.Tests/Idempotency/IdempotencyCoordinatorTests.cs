using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 幂等协调器判定语义表测试：8 行语义逐行断言 + 占位抢占重判 + 时间戳取自时钟。
/// </summary>
public class IdempotencyCoordinatorTests
{
    private const string TestScope = "order";
    private const string TestKey = "key-1";
    private const string DigestAlpha = "digest-alpha";
    private const string DigestBeta = "digest-beta";

    private static InMemoryIdempotencyStore CreateStore()
    {
        return new InMemoryIdempotencyStore();
    }

    private static IdempotencyCoordinator CreateCoordinator(InMemoryIdempotencyStore idempotencyStore, FakeClock clock, IdempotencyOptions? idempotencyOptions = null)
    {
        return new IdempotencyCoordinator(idempotencyStore, clock, idempotencyOptions);
    }

    private static IdempotencyRecordIdentifier CreateIdentifier()
    {
        return new IdempotencyRecordIdentifier(TestScope, TestKey);
    }

    // ───── 判定语义表 第 1 行：无存量记录 → Execute ─────

    [Fact]
    public async Task BeginAsync_WithoutExistingRecord_ShouldReturnExecuteAndOccupyProcessing()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock);

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言
        Assert.Equal(IdempotencyDecisionKind.Execute, decision.DecisionKind);
        Assert.Null(decision.ExistingRecord);

        IdempotencyRecord? occupiedRecord = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(occupiedRecord);
        Assert.Equal(IdempotencyStatus.Processing, occupiedRecord!.Status);
        Assert.Equal(DigestAlpha, occupiedRecord.RequestDigest);
        Assert.Null(occupiedRecord.FinishedTime);
        Assert.Equal(occupiedRecord.CreatedTime + IdempotencyOptions.DefaultRetentionMilliseconds, occupiedRecord.ExpiredTime);
    }

    // ───── 判定语义表 第 2 行：已完成 + 摘要相同 → Replay ─────

    [Fact]
    public async Task BeginAsync_CompletedRecordWithSameDigest_ShouldReturnReplayWithFirstResponse()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock);
        byte[] firstResponseBytes = new byte[] { 1, 2, 3 };

        IdempotencyDecision firstDecision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));
        Assert.Equal(IdempotencyDecisionKind.Execute, firstDecision.DecisionKind);
        await coordinator.CompleteAsync(new IdempotencyCompletionRequest(CreateIdentifier(), firstResponseBytes, 0));

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言
        Assert.Equal(IdempotencyDecisionKind.Replay, decision.DecisionKind);
        Assert.NotNull(decision.ExistingRecord);
        Assert.Equal(IdempotencyStatus.Completed, decision.ExistingRecord!.Status);
        Assert.NotNull(decision.ExistingRecord.FirstResponse);
        Assert.Equal(firstResponseBytes, decision.ExistingRecord.FirstResponse!.Value.ToArray());
    }

    // ───── 判定语义表 第 3 行：已完成 + 摘要不同 → Conflict ─────

    [Fact]
    public async Task BeginAsync_CompletedRecordWithDifferentDigest_ShouldReturnConflict()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));
        await coordinator.CompleteAsync(new IdempotencyCompletionRequest(CreateIdentifier(), new byte[] { 1 }, 0));

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestBeta));

        // 断言
        Assert.Equal(IdempotencyDecisionKind.Conflict, decision.DecisionKind);
        Assert.NotNull(decision.ExistingRecord);
        Assert.Equal(IdempotencyStatus.Completed, decision.ExistingRecord!.Status);
    }

    // ───── 判定语义表 第 4 行：已失败 + 策略 ReplayError → Replay ─────

    [Fact]
    public async Task BeginAsync_FailedRecordWithReplayErrorPolicy_ShouldReturnReplayWithFailedRecord()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));
        await coordinator.FailAsync(new IdempotencyFailureRequest(CreateIdentifier(), 0));

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言
        Assert.Equal(IdempotencyDecisionKind.Replay, decision.DecisionKind);
        Assert.NotNull(decision.ExistingRecord);
        Assert.Equal(IdempotencyStatus.Failed, decision.ExistingRecord!.Status);
    }

    // ───── 判定语义表 第 5 行：已失败 + 策略 ReExecute → Execute（覆盖占位） ─────

    [Fact]
    public async Task BeginAsync_FailedRecordWithReExecutePolicy_ShouldReturnExecuteAndOverwrite()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();
        idempotencyOptions.FailedReplayPolicy = FailedReplayPolicy.ReExecute;
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock, idempotencyOptions);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));
        await coordinator.FailAsync(new IdempotencyFailureRequest(CreateIdentifier(), 0));
        IdempotencyRecord? failedRecord = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(failedRecord);

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言：覆盖占位为新的执行中记录
        Assert.Equal(IdempotencyDecisionKind.Execute, decision.DecisionKind);
        IdempotencyRecord? overwrittenRecord = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(overwrittenRecord);
        Assert.Equal(IdempotencyStatus.Processing, overwrittenRecord!.Status);
        Assert.True(overwrittenRecord.CreatedTime >= failedRecord!.CreatedTime);
    }

    // ───── 判定语义表 第 6 行：执行中 + 等待不超时 → 等待后重判 ─────

    [Fact]
    public async Task BeginAsync_ProcessingRecordCompletesDuringWait_ShouldReplayAfterWait()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();
        idempotencyOptions.ConcurrentWaitTimeoutMilliseconds = 3000;
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock, idempotencyOptions);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 执行：并发占位方在等待窗口内落定为成功
        IdempotencyRecordIdentifier identifier = CreateIdentifier();
        _ = Task.Run(async () =>
        {
            await Task.Delay(80);
            await coordinator.CompleteAsync(new IdempotencyCompletionRequest(identifier, new byte[] { 9 }, 0));
        });
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言：等待期间状态变化即重判为回放，而不是等待超时判忙碌
        Assert.Equal(IdempotencyDecisionKind.Replay, decision.DecisionKind);
        Assert.NotNull(decision.ExistingRecord);
        Assert.Equal(IdempotencyStatus.Completed, decision.ExistingRecord!.Status);
    }

    // ───── 判定语义表 第 7 行：执行中 + 等待超时 → Busy ─────

    [Fact]
    public async Task BeginAsync_ProcessingRecordWaitTimeout_ShouldReturnBusy()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();
        idempotencyOptions.ConcurrentWaitTimeoutMilliseconds = 60;
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock, idempotencyOptions);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 执行：占位方始终不落定
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言
        Assert.Equal(IdempotencyDecisionKind.Busy, decision.DecisionKind);
        Assert.NotNull(decision.ExistingRecord);
        Assert.Equal(IdempotencyStatus.Processing, decision.ExistingRecord!.Status);
    }

    // ───── 判定语义表 第 8 行：任意状态 + 已过期 → 视为无记录 → Execute（新占位覆盖） ─────

    [Fact]
    public async Task BeginAsync_ExpiredCompletedRecord_ShouldReturnExecuteAndOverwrite()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();
        idempotencyOptions.RetentionMilliseconds = 1000;
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock, idempotencyOptions);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));
        await coordinator.CompleteAsync(new IdempotencyCompletionRequest(CreateIdentifier(), new byte[] { 1 }, 0));
        IdempotencyRecord? staleRecord = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(staleRecord);

        // 执行：快进时钟越过过期时刻
        clock.AdvanceMilliseconds(idempotencyOptions.RetentionMilliseconds + 1);
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言：过期视为无记录，重新执行并覆盖旧记录
        Assert.Equal(IdempotencyDecisionKind.Execute, decision.DecisionKind);
        Assert.Null(decision.ExistingRecord);
        IdempotencyRecord? overwrittenRecord = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(overwrittenRecord);
        Assert.Equal(IdempotencyStatus.Processing, overwrittenRecord!.Status);
        Assert.True(overwrittenRecord.CreatedTime > staleRecord!.CreatedTime);
    }

    // ───── 占位抢占重判：占位失败后按语义重判一次 ─────

    [Fact]
    public async Task BeginAsync_OccupationPreemptedOnce_ShouldRejudgeAndReplayCompletedRecord()
    {
        // 安排：脚本化存储——首次读取无记录、占位被并发方抢先、重判时对方已落定为成功
        CompletedOnceScriptedStore scriptedStore = new CompletedOnceScriptedStore(DigestAlpha);
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = new IdempotencyCoordinator(scriptedStore, clock);

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言：重判一次生效，回放对方的结果
        Assert.Equal(IdempotencyDecisionKind.Replay, decision.DecisionKind);
        Assert.Equal(1, scriptedStore.BeginProcessingCallCount);
    }

    [Fact]
    public async Task BeginAsync_OccupationPreemptedTwice_ShouldReturnBusy()
    {
        // 安排：脚本化存储——两次读取均无记录、两次占位均被抢先
        AlwaysPreemptedScriptedStore scriptedStore = new AlwaysPreemptedScriptedStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = new IdempotencyCoordinator(scriptedStore, clock);

        // 执行
        IdempotencyDecision decision = await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 断言：占位被抢占后只重判一次，仍被抢占判忙碌
        Assert.Equal(IdempotencyDecisionKind.Busy, decision.DecisionKind);
        Assert.Equal(2, scriptedStore.BeginProcessingCallCount);
    }

    // ───── 完成时刻由协调器时钟取值并覆盖请求字段 ─────

    [Fact]
    public async Task CompleteAsync_ShouldTakeCompletionTimeFromClockInsteadOfRequest()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 执行：请求中的完成时刻为 0，应由时钟覆盖
        long beforeCompleteTime = clock.UtcNowTime;
        await coordinator.CompleteAsync(new IdempotencyCompletionRequest(CreateIdentifier(), new byte[] { 5 }, 0));
        long afterCompleteTime = clock.UtcNowTime;

        // 断言
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Completed, record!.Status);
        Assert.NotNull(record.FinishedTime);
        Assert.InRange(record.FinishedTime!.Value, beforeCompleteTime, afterCompleteTime);
    }

    [Fact]
    public async Task FailAsync_ShouldTakeFailureTimeFromClockInsteadOfRequest()
    {
        // 安排
        InMemoryIdempotencyStore idempotencyStore = CreateStore();
        FakeClock clock = new FakeClock();
        IdempotencyCoordinator coordinator = CreateCoordinator(idempotencyStore, clock);
        await coordinator.BeginAsync(new IdempotencyBeginRequest(TestScope, TestKey, DigestAlpha));

        // 执行
        long beforeFailTime = clock.UtcNowTime;
        await coordinator.FailAsync(new IdempotencyFailureRequest(CreateIdentifier(), 0));
        long afterFailTime = clock.UtcNowTime;

        // 断言
        IdempotencyRecord? record = await idempotencyStore.TryGetRecordAsync(CreateIdentifier());
        Assert.NotNull(record);
        Assert.Equal(IdempotencyStatus.Failed, record!.Status);
        Assert.NotNull(record.FinishedTime);
        Assert.InRange(record.FinishedTime!.Value, beforeFailTime, afterFailTime);
    }

    // ───── 参数校验 ─────

    [Fact]
    public async Task BeginAsync_NullRequest_ShouldThrowArgumentNullException()
    {
        // 安排
        IdempotencyCoordinator coordinator = CreateCoordinator(CreateStore(), new FakeClock());

        // 执行 + 断言
        await Assert.ThrowsAsync<ArgumentNullException>(() => coordinator.BeginAsync(null!));
    }

    /// <summary>
    /// 脚本化存储：首次读取无记录且占位被抢先；重判读取时返回已成功、摘要一致的记录。
    /// </summary>
    private sealed class CompletedOnceScriptedStore : IIdempotencyStore
    {
        private readonly string _requestDigest;
        private int _getRecordCallCount;

        public CompletedOnceScriptedStore(string requestDigest)
        {
            _requestDigest = requestDigest;
        }

        public int BeginProcessingCallCount
        {
            get;
            private set;
        }

        public Task<bool> TryBeginProcessingAsync(IdempotencyRecord newRecord, CancellationToken cancellationToken = default)
        {
            BeginProcessingCallCount++;
            return Task.FromResult(false);
        }

        public Task CompleteAsync(IdempotencyCompletionRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("脚本化存储不支持本操作。");
        }

        public Task FailAsync(IdempotencyFailureRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("脚本化存储不支持本操作。");
        }

        public Task<IdempotencyRecord?> TryGetRecordAsync(IdempotencyRecordIdentifier identifier, CancellationToken cancellationToken = default)
        {
            _getRecordCallCount++;
            if (_getRecordCallCount == 1)
            {
                return Task.FromResult<IdempotencyRecord?>(null);
            }

            IdempotencyRecord completedRecord = new IdempotencyRecord(identifier.IdempotencyScope, identifier.IdempotencyKey, _requestDigest, IdempotencyStatus.Completed, new byte[] { 7 }, 1000, long.MaxValue, 1500);
            return Task.FromResult<IdempotencyRecord?>(completedRecord);
        }

        public Task<int> RemoveExpiredRecordsAsync(ExpiredRecordRemovalRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("脚本化存储不支持本操作。");
        }
    }

    /// <summary>
    /// 脚本化存储：读取始终无记录、占位始终被抢先。
    /// </summary>
    private sealed class AlwaysPreemptedScriptedStore : IIdempotencyStore
    {
        public int BeginProcessingCallCount
        {
            get;
            private set;
        }

        public Task<bool> TryBeginProcessingAsync(IdempotencyRecord newRecord, CancellationToken cancellationToken = default)
        {
            BeginProcessingCallCount++;
            return Task.FromResult(false);
        }

        public Task CompleteAsync(IdempotencyCompletionRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("脚本化存储不支持本操作。");
        }

        public Task FailAsync(IdempotencyFailureRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("脚本化存储不支持本操作。");
        }

        public Task<IdempotencyRecord?> TryGetRecordAsync(IdempotencyRecordIdentifier identifier, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IdempotencyRecord?>(null);
        }

        public Task<int> RemoveExpiredRecordsAsync(ExpiredRecordRemovalRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException("脚本化存储不支持本操作。");
        }
    }
}
