using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ConcurrentLimitedQueue&lt;T&gt; 类单元测试
/// </summary>
public class ConcurrentLimitedQueueTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidLimit_ShouldCreateQueue()
    {
        // Arrange
        var limit = 5;

        // Act
        var queue = new ConcurrentLimitedQueue<int>(limit);

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(limit, queue.Limit);
        Assert.Empty(queue);
    }

    [Fact]
    public void Constructor_WithZeroLimit_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var limit = 0;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentLimitedQueue<int>(limit));
        Assert.Equal("limit", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeLimit_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var limit = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentLimitedQueue<int>(limit));
        Assert.Equal("limit", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidList_ShouldCreateQueue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var queue = new ConcurrentLimitedQueue<int>(list);

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(list.Count, queue.Limit);
        Assert.Equal(list.Count, queue.Count);
    }

    [Fact]
    public void Constructor_WithNullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> list = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new ConcurrentLimitedQueue<int>(list));
        Assert.Equal("list", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithEmptyList_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var list = new List<int>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentLimitedQueue<int>(list));
        Assert.Equal("list", exception.ParamName);
    }

    #endregion

    #region Implicit Operator Tests

    [Fact]
    public void ImplicitOperator_WithValidList_ShouldCreateQueue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        ConcurrentLimitedQueue<int> queue = list;

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(list.Count, queue.Limit);
        Assert.Equal(list.Count, queue.Count);
    }

    [Fact]
    public void ImplicitOperator_WithNullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<int> list = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
        {
            ConcurrentLimitedQueue<int> queue = list;
        });
        Assert.Equal("list", exception.ParamName);
    }

    #endregion

    #region Enqueue Tests

    [Fact]
    public void Enqueue_WithinLimit_ShouldAddElement()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(3);

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Assert
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Enqueue_AtLimit_ShouldAddElementWithoutRemoving()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(2);
        queue.Enqueue(1);

        // Act
        queue.Enqueue(2);

        // Assert
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Enqueue_ExceedingLimit_ShouldRemoveOldestElement()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(2);
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act
        queue.Enqueue(3);

        // Assert
        Assert.Equal(2, queue.Count);

        // Verify that the oldest element (1) was removed
        var items = queue.ToArray();
        Assert.Contains(2, items);
        Assert.Contains(3, items);
        Assert.DoesNotContain(1, items);
    }

    [Fact]
    public void Enqueue_MultipleElementsExceedingLimit_ShouldMaintainLimit()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(3);

        // Act
        for (int i = 1; i <= 10; i++)
        {
            queue.Enqueue(i);
        }

        // Assert
        Assert.Equal(3, queue.Count);

        // Verify that only the last 3 elements remain
        var items = queue.ToArray();
        Assert.Contains(8, items);
        Assert.Contains(9, items);
        Assert.Contains(10, items);
    }

    [Fact]
    public void Enqueue_WithLimitOne_ShouldAlwaysContainOnlyLastElement()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(1);

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Assert
        Assert.Single(queue);
        Assert.Equal(3, queue.ToArray()[0]);
    }

    [Fact]
    public void TryAdd_ThroughProducerConsumerCollection_ShouldMaintainLimit()
    {
        // Arrange
        IProducerConsumerCollection<int> queue = new ConcurrentLimitedQueue<int>(2);

        // Act
        queue.TryAdd(1);
        queue.TryAdd(2);
        queue.TryAdd(3);

        // Assert
        Assert.Equal(2, queue.Count);
        var items = queue.ToArray();
        Assert.DoesNotContain(1, items);
        Assert.Contains(2, items);
        Assert.Contains(3, items);
    }

    [Theory]
    [InlineData(LimitedQueueOverflowStrategy.DropNewest)]
    [InlineData(LimitedQueueOverflowStrategy.RejectNewItem)]
    public void TryEnqueue_WhenStrategyRejectsNewItem_ShouldKeepExistingItems(
        LimitedQueueOverflowStrategy strategy)
    {
        var queue = new ConcurrentLimitedQueue<int>(2, strategy);
        queue.Enqueue(1);
        queue.Enqueue(2);

        var added = queue.TryEnqueue(3, out var discarded);

        Assert.False(added);
        Assert.Equal(new[] { 1, 2 }, queue.ToArray());
        Assert.Equal(strategy == LimitedQueueOverflowStrategy.DropNewest ? 3 : 0, discarded);
    }

    [Fact]
    public void TryEnqueue_DropOldest_ShouldReturnDiscardedItemAndNotify()
    {
        var queue = new ConcurrentLimitedQueue<int>(2);
        QueueDiscardedItem<int>? notification = null;
        queue.ItemDiscarded = item => notification = item;
        queue.Enqueue(1);
        queue.Enqueue(2);

        var added = queue.TryEnqueue(3, out var discarded);

        Assert.True(added);
        Assert.Equal(1, discarded);
        Assert.Equal(new[] { 2, 3 }, queue.ToArray());
        Assert.Equal(1, notification?.Item);
        Assert.Equal(LimitedQueueDiscardReason.Overflow, notification?.Reason);
    }

    [Fact]
    public void Enqueue_ThrowStrategy_WhenFull_ShouldThrowWithoutChangingQueue()
    {
        var queue = new ConcurrentLimitedQueue<int>(1, LimitedQueueOverflowStrategy.Throw);
        queue.Enqueue(1);

        Assert.Throws<InvalidOperationException>(() => queue.Enqueue(2));
        Assert.Equal(new[] { 1 }, queue.ToArray());
    }

    [Fact]
    public void ItemDiscarded_CallbackThrows_ShouldNotCorruptQueue()
    {
        var queue = new ConcurrentLimitedQueue<int>(1);
        queue.ItemDiscarded = _ => throw new InvalidOperationException();
        queue.Enqueue(1);

        queue.Enqueue(2);

        Assert.Equal(new[] { 2 }, queue.ToArray());
    }

    #endregion

    #region Limit Property Tests

    [Fact]
    public void Limit_SetValidValue_ShouldUpdateLimit()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(5);
        var newLimit = 10;

        // Act
        queue.Limit = newLimit;

        // Assert
        Assert.Equal(newLimit, queue.Limit);
    }

    [Fact]
    public void Limit_SetToSmallerValue_ShouldNotAffectExistingElements()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(5);
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        queue.Limit = 2;

        // Assert
        Assert.Equal(2, queue.Limit);
        Assert.Equal(2, queue.Count);
        Assert.Equal(new[] { 2, 3 }, queue.ToArray());
    }

    [Fact]
    public void SetLimit_WhenReduced_ShouldReturnAndNotifyDiscardedItems()
    {
        var queue = new ConcurrentLimitedQueue<int>(4);
        var notifications = new List<QueueDiscardedItem<int>>();
        queue.ItemDiscarded = notifications.Add;
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);

        var discarded = queue.SetLimit(2);

        Assert.Equal(new[] { 1, 2 }, discarded);
        Assert.Equal(new[] { 3, 4 }, queue.ToArray());
        Assert.All(notifications, item => Assert.Equal(LimitedQueueDiscardReason.LimitReduced, item.Reason));
    }

    [Fact]
    public async Task ConcurrentEnqueue_ShouldNeverExceedLimit()
    {
        var queue = new ConcurrentLimitedQueue<int>(25);

        await Task.WhenAll(Enumerable.Range(0, 500).Select(i => Task.Run(() => queue.Enqueue(i))));

        Assert.Equal(25, queue.Count);
    }

    [Fact]
    public void Limit_SetInvalidValue_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(5);

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => queue.Limit = 0);
        Assert.Equal("value", exception.ParamName);
    }

    #endregion
}
