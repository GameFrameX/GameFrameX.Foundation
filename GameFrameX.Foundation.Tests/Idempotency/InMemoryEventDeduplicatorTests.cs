using System;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 进程内事件去重测试：三态语义（首见、重现、逐出后再通过）与容量校验。
/// </summary>
public class InMemoryEventDeduplicatorTests
{
    [Fact]
    public void TryConsume_FirstSeen_ShouldReturnTrue()
    {
        // 安排
        InMemoryEventDeduplicator eventDeduplicator = new InMemoryEventDeduplicator();

        // 执行 + 断言：首见登记成功
        Assert.True(eventDeduplicator.TryConsume("event-1"));
    }

    [Fact]
    public void TryConsume_DuplicateEventId_ShouldReturnFalse()
    {
        // 安排
        InMemoryEventDeduplicator eventDeduplicator = new InMemoryEventDeduplicator();
        Assert.True(eventDeduplicator.TryConsume("event-1"));

        // 执行 + 断言：重现被拦截
        Assert.False(eventDeduplicator.TryConsume("event-1"));
    }

    [Fact]
    public void TryConsume_EvictedEventId_ShouldPassAgain()
    {
        // 安排：容量 2，先登记两条
        InMemoryEventDeduplicator eventDeduplicator = new InMemoryEventDeduplicator(2);
        Assert.True(eventDeduplicator.TryConsume("event-1"));
        Assert.True(eventDeduplicator.TryConsume("event-2"));

        // 执行：第三条触发插入序逐出最早登记的 event-1
        Assert.True(eventDeduplicator.TryConsume("event-3"));

        // 断言：尽力而为边界——被逐出的旧标识再次通过
        Assert.True(eventDeduplicator.TryConsume("event-1"));
        Assert.False(eventDeduplicator.TryConsume("event-3"));
    }

    [Fact]
    public void Constructor_NonPositiveCapacity_ShouldThrowArgumentException()
    {
        // 执行 + 断言
        Assert.Throws<ArgumentException>(() => new InMemoryEventDeduplicator(0));
        Assert.Throws<ArgumentException>(() => new InMemoryEventDeduplicator(-1));
    }

    [Fact]
    public void TryConsume_BlankEventId_ShouldThrowArgumentException()
    {
        // 安排
        InMemoryEventDeduplicator eventDeduplicator = new InMemoryEventDeduplicator();

        // 执行 + 断言
        Assert.Throws<ArgumentException>(() => eventDeduplicator.TryConsume(" "));
        Assert.Throws<ArgumentException>(() => eventDeduplicator.TryConsume(null!));
    }
}
