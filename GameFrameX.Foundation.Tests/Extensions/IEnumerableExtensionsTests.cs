using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// IEnumerableExtensions 扩展类单元测试
/// </summary>
public class IEnumerableExtensionsTests
{
    #region IntersectBy Tests

    [Fact]
    public void IntersectBy_WithCondition_ShouldReturnCorrectIntersection()
    {
        // Arrange
        var first = new[] { 1, 2, 3, 4, 5 };
        var second = new[] { "2", "4", "6" };

        // Act
        var result = first.IntersectByComparer(second, (f, s) => f.ToString() == s).ToList();

        // Assert
        Assert.Equal(new[] { 2, 4 }, result);
    }



    #endregion

    #region IntersectAll Tests

    [Fact]
    public void IntersectAll_ShouldReturnCorrectIntersection()
    {
        // Arrange
        var collections = new[]
        {
            new[] { 1, 2, 3, 4 },
            new[] { 2, 3, 4, 5 },
            new[] { 3, 4, 5, 6 }
        };

        // Act
        var result = collections.IntersectAllComparer().ToList();

        // Assert
        Assert.Equal(new[] { 3, 4 }, result);
    }

    [Fact]
    public void IntersectAll_WithKeySelector_ShouldReturnCorrectIntersection()
    {
        // Arrange
        var collections = new[]
        {
            new[] { 1, 2, 3, 4 },
            new[] { 2, 3, 4, 5 },
            new[] { 3, 4, 5, 6 }
        };

        // Act
        var result = collections.IntersectAllComparer(x => x).ToList();

        // Assert
        Assert.Equal(new[] { 3, 4 }, result);
    }

    #endregion

    #region ExceptBy Tests

    [Fact]
    public void ExceptBy_WithCondition_ShouldReturnCorrectDifference()
    {
        // Arrange
        var first = new[] { 1, 2, 3, 4, 5 };
        var second = new[] { "2", "4" };

        // Act
        var result = first.ExceptByExpression(second, (f, s) => f.ToString() == s).ToList();

        // Assert
        Assert.Equal(new[] { 1, 3, 5 }, result);
    }



    #endregion



    #region AddRange Tests

    [Fact]
    public void AddRange_ICollection_ShouldAddAllElements()
    {
        // Arrange
        var collection = new List<int> { 1, 2 };
        var values = new[] { 3, 4, 5 };

        // Act
        collection.AddRange(values);

        // Assert
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, collection);
    }

    [Fact]
    public void AddRange_ConcurrentBag_ShouldAddAllElements()
    {
        // Arrange
        var bag = new ConcurrentBag<int> { 1, 2 };
        var values = new[] { 3, 4, 5 };

        // Act
        bag.AddRangeValues(values);

        // Assert
        Assert.Equal(5, bag.Count);
        Assert.Contains(3, bag);
        Assert.Contains(4, bag);
        Assert.Contains(5, bag);
    }

    [Fact]
    public void AddRange_ConcurrentQueue_ShouldAddAllElements()
    {
        // Arrange
        var queue = new ConcurrentQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        var values = new[] { 3, 4, 5 };

        // Act
        queue.AddRangeValues(values);

        // Assert
        Assert.Equal(5, queue.Count);
    }

    #endregion

    #region ForEach Tests

    [Fact]
    public void ForEach_ShouldExecuteActionForAllElements()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        var result = new List<int>();

        // Act
        source.ForEach(x => result.Add(x * 2));

        // Assert
        Assert.Equal(new[] { 2, 4, 6 }, result);
    }

    #endregion

    #region SelectAsync Tests

    [Fact]
    public async Task SelectAsync_ShouldReturnCorrectResults()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };

        // Act
        var result = await source.SelectAsync(async x => 
        {
            await Task.Delay(10);
            return x * 2;
        });

        // Assert
        Assert.Equal(new[] { 2, 4, 6 }, result);
    }

    [Fact]
    public async Task SelectAsync_WithIndex_ShouldReturnCorrectResults()
    {
        // Arrange
        var source = new[] { 10, 20, 30 };

        // Act
        var result = await source.SelectAsync(async (x, index) => 
        {
            await Task.Delay(10);
            return x + index;
        });

        // Assert
        Assert.Equal(new[] { 10, 21, 32 }, result);
    }

    #endregion

    #region MaxOrDefault Tests

    [Fact]
    public void MaxOrDefault_EmptySequence_ShouldReturnDefault()
    {
        // Arrange
        var source = new int[0];

        // Act
        var result = source.MaxOrDefaultValue();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void MaxOrDefault_WithDefaultValue_ShouldReturnDefaultValue()
    {
        // Arrange
        var source = new int[0];

        // Act
        var result = source.MaxOrDefaultValue(-1);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void MaxOrDefault_WithSelector_ShouldReturnMaxValue()
    {
        // Arrange
        var source = new[] { "a", "abc", "ab" };

        // Act
        var result = source.MaxOrDefaultValue(x => x.Length);

        // Assert
        Assert.Equal(3, result);
    }

    #endregion

    #region MinOrDefault Tests

    [Fact]
    public void MinOrDefault_EmptySequence_ShouldReturnDefault()
    {
        // Arrange
        var source = new int[0];

        // Act
        var result = source.MinOrDefaultValue();

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void MinOrDefault_WithDefaultValue_ShouldReturnDefaultValue()
    {
        // Arrange
        var source = new int[0];

        // Act
        var result = source.MinOrDefaultValue(-1);

        // Assert
        Assert.Equal(-1, result);
    }

    [Fact]
    public void MinOrDefault_WithSelector_ShouldReturnMinValue()
    {
        // Arrange
        var source = new[] { "abc", "a", "ab" };

        // Act
        var result = source.MinOrDefaultValue(x => x.Length);

        // Assert
        Assert.Equal(1, result);
    }

    #endregion

    #region StandardDeviation Tests

    [Fact]
    public void StandardDeviation_ShouldCalculateCorrectValue()
    {
        // Arrange
        var source = new[] { 1.0, 2.0, 3.0, 4.0, 5.0 };

        // Act
        var result = source.StandardDeviation();

        // Assert
        Assert.True(Math.Abs(result - 1.4142135623730951) < 0.0001);
    }

    [Fact]
    public void StandardDeviation_SingleElement_ShouldReturnZero()
    {
        // Arrange
        var source = new[] { 5.0 };

        // Act
        var result = source.StandardDeviation();

        // Assert
        Assert.Equal(0.0, result);
    }

    #endregion

    #region OrderByRandom Tests

    [Fact]
    public void OrderByRandom_ShouldReturnAllElements()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = source.OrderByRandom().ToList();

        // Assert
        Assert.Equal(5, result.Count);
        Assert.All(source, item => Assert.Contains(item, result));
    }

    #endregion

    #region SequenceEqual Tests

    [Fact]
    public void SequenceEqual_WithCondition_ShouldReturnTrue()
    {
        // Arrange
        var first = new[] { 1, 2, 3 };
        var second = new[] { 1, 2, 3 };

        // Act
        var result = first.SequenceEqualSameType(second, (x, y) => x == y);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SequenceEqual_DifferentTypes_ShouldReturnTrue()
    {
        // Arrange
        var first = new[] { 1, 2, 3 };
        var second = new[] { "1", "2", "3" };

        // Act
        var result = first.SequenceEqual(second, (x, y) => x.ToString() == y);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region CompareChanges Tests

    [Fact]
    public void CompareChanges_ShouldReturnCorrectChanges()
    {
        // Arrange
        var first = new[] { 1, 2, 3, 4 };
        var second = new[] { 2, 3, 5, 6 };

        // Act
        var (adds, removes, updates) = first.CompareChanges(second, (x, y) => x == y);

        // Assert
        Assert.Equal(new[] { 1, 4 }, adds);
        Assert.Equal(new[] { 5, 6 }, removes);
        Assert.Equal(new[] { 2, 3 }, updates);
    }

    #endregion

    #region AsNotNull Tests

    [Fact]
    public void AsNotNull_NullList_ShouldReturnEmptyList()
    {
        // Arrange
        List<int> list = null;

        // Act
        var result = list.AsNotNull();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void AsNotNull_NonNullList_ShouldReturnSameList()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var result = list.AsNotNull();

        // Assert
        Assert.Same(list, result);
    }

    #endregion

    #region WhereIf Tests

    [Fact]
    public void WhereIf_ConditionTrue_ShouldApplyFilter()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = source.WhereIf(true, x => x > 3).ToList();

        // Assert
        Assert.Equal(new[] { 4, 5 }, result);
    }

    [Fact]
    public void WhereIf_ConditionFalse_ShouldNotApplyFilter()
    {
        // Arrange
        var source = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = source.WhereIf(false, x => x > 3).ToList();

        // Assert
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, result);
    }

    #endregion

    #region ChangeIndex Tests

    [Fact]
    public void ChangeIndex_ShouldMoveElementToNewPosition()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        list.ChangeIndex(3, 0);

        // Assert
        Assert.Equal(new[] { 3, 1, 2, 4, 5 }, list);
    }

    [Fact]
    public void ChangeIndex_WithCondition_ShouldMoveElementToNewPosition()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        list.ChangeIndex(x => x == 4, 1);

        // Assert
        Assert.Equal(new[] { 1, 4, 2, 3, 5 }, list);
    }

    [Fact]
    public void ChangeIndex_NullItem_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new List<string> { "a", "b", "c" };
        string item = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.ChangeIndex(item, 0));
    }

    #endregion
}