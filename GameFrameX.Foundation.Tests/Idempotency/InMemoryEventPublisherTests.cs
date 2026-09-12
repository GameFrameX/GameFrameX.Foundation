using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 进程内事件发布器测试：订阅顺序分发、异常传播、非法信封拦截。
/// </summary>
public class InMemoryEventPublisherTests
{
    private static EventEnvelope CreateValidEnvelope(string eventId)
    {
        return new EventEnvelope(eventId, "sample.order.placed", 1_700_000_000_000, 1, "order-service", null, new byte[] { 1 }, null);
    }

    [Fact]
    public async Task PublishAsync_ShouldDispatchToSubscribersInSubscriptionOrder()
    {
        // 安排
        InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();
        List<int> dispatchOrder = new List<int>();
        eventPublisher.Subscribe(_ => dispatchOrder.Add(1));
        eventPublisher.Subscribe(_ => dispatchOrder.Add(2));
        eventPublisher.Subscribe(_ => dispatchOrder.Add(3));

        // 执行
        await eventPublisher.PublishAsync(CreateValidEnvelope("event-1"));

        // 断言：按订阅顺序同步分发
        Assert.Equal(new List<int> { 1, 2, 3 }, dispatchOrder);
    }

    [Fact]
    public async Task PublishAsync_SubscriberThrows_ShouldPropagateAndStopFollowingSubscribers()
    {
        // 安排
        InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();
        List<int> dispatchOrder = new List<int>();
        eventPublisher.Subscribe(_ => dispatchOrder.Add(1));
        eventPublisher.Subscribe(_ => throw new InvalidOperationException("订阅者故障。"));
        eventPublisher.Subscribe(_ => dispatchOrder.Add(3));

        // 执行 + 断言：异常向上传播，后续订阅者不再收到分发
        await Assert.ThrowsAsync<InvalidOperationException>(() => eventPublisher.PublishAsync(CreateValidEnvelope("event-1")));
        Assert.Equal(new List<int> { 1 }, dispatchOrder);
    }

    [Fact]
    public async Task PublishAsync_InvalidEnvelope_ShouldThrowBeforeDispatch()
    {
        // 安排
        InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();
        int dispatchCount = 0;
        eventPublisher.Subscribe(_ => dispatchCount++);
        EventEnvelope invalidEnvelope = new EventEnvelope("", "sample.order.placed", 1_700_000_000_000, 1, "order-service", null, new byte[] { 1 }, null);

        // 执行 + 断言：先校验后分发
        await Assert.ThrowsAsync<ArgumentException>(() => eventPublisher.PublishAsync(invalidEnvelope));
        Assert.Equal(0, dispatchCount);
    }

    [Fact]
    public async Task PublishAsync_NullEnvelope_ShouldThrowArgumentNullException()
    {
        // 安排
        InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();

        // 执行 + 断言
        await Assert.ThrowsAsync<ArgumentNullException>(() => eventPublisher.PublishAsync(null!));
    }

    [Fact]
    public void Subscribe_NullSubscriber_ShouldThrowArgumentNullException()
    {
        // 安排
        InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();

        // 执行 + 断言
        Assert.Throws<ArgumentNullException>(() => eventPublisher.Subscribe(null!));
    }

    [Fact]
    public async Task PublishAsync_NoSubscriber_ShouldCompleteWithoutError()
    {
        // 安排
        InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();

        // 执行 + 断言：无订阅者时发布成功
        await eventPublisher.PublishAsync(CreateValidEnvelope("event-1"));
    }
}
