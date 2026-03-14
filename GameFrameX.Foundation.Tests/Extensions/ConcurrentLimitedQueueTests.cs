using System;
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
    public void Constructor_WithEmptyList_ShouldCreateEmptyQueue()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var queue = new ConcurrentLimitedQueue<int>(list);

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(0, queue.Limit);
        Assert.Empty(queue);
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
        Assert.Equal(3, queue.Count); // Existing elements should remain
    }

    #endregion
}