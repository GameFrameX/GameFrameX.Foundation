using System;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 幂等选项测试：默认值、赋值即校验。
/// </summary>
public class IdempotencyOptionsTests
{
    [Fact]
    public void Constructor_ShouldApplyDefaultValues()
    {
        // 安排 + 执行
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();

        // 断言：默认保留 24 小时、失败回放错误、并发等待 5 秒
        Assert.Equal(86_400_000L, idempotencyOptions.RetentionMilliseconds);
        Assert.Equal(FailedReplayPolicy.ReplayError, idempotencyOptions.FailedReplayPolicy);
        Assert.Equal(5_000L, idempotencyOptions.ConcurrentWaitTimeoutMilliseconds);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void RetentionMilliseconds_NonPositiveAssignment_ShouldThrowArgumentException(long retentionMilliseconds)
    {
        // 安排
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();

        // 执行 + 断言：赋值即校验
        Assert.Throws<ArgumentException>(() => idempotencyOptions.RetentionMilliseconds = retentionMilliseconds);
    }

    [Fact]
    public void ConcurrentWaitTimeoutMilliseconds_NegativeAssignment_ShouldThrowArgumentException()
    {
        // 安排
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();

        // 执行 + 断言：0 合法（立即判忙碌），负数非法
        idempotencyOptions.ConcurrentWaitTimeoutMilliseconds = 0;
        Assert.Throws<ArgumentException>(() => idempotencyOptions.ConcurrentWaitTimeoutMilliseconds = -1);
    }

    [Fact]
    public void RetentionMilliseconds_ValidAssignment_ShouldBeApplied()
    {
        // 安排
        IdempotencyOptions idempotencyOptions = new IdempotencyOptions();

        // 执行
        idempotencyOptions.RetentionMilliseconds = 60_000L;

        // 断言
        Assert.Equal(60_000L, idempotencyOptions.RetentionMilliseconds);
    }
}
