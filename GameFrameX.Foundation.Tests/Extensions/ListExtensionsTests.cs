using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ListExtensions 扩展类单元测试
/// </summary>
public class ListExtensionsTests
{
    #region ForEachAsync List Tests

    [Fact]
    public async Task ForEachAsync_List_ValidInput_ShouldExecuteForAllElements()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var results = new List<int>();

        // Act
        await list.ForEachAsync(async item =>
        {
            await Task.Delay(1); // Simulate async work
            results.Add(item * 2);
        });

        // Assert
        Assert.Equal(new[] { 2, 4, 6, 8, 10 }, results);
    }

    [Fact]
    public async Task ForEachAsync_List_EmptyList_ShouldNotExecute()
    {
        // Arrange
        var list = new List<int>();
        var executed = false;

        // Act
        await list.ForEachAsync(async item =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public async Task ForEachAsync_List_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<int> list = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            list.ForEachAsync(async item => await Task.Delay(1)));
        Assert.Equal("list", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_List_NullFunc_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        Func<int, Task> func = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            list.ForEachAsync(func));
        Assert.Equal("func", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_List_ExceptionInFunc_ShouldPropagateException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            list.ForEachAsync(async item =>
            {
                await Task.Delay(1);
                if (item != 2)
                {
                }
                else
                {
                    throw expectedException;
                }
            }));
        Assert.Equal(expectedException.Message, actualException.Message);
    }

    #endregion

    #region ForEachAsync IEnumerable Tests

    [Fact]
    public async Task ForEachAsync_IEnumerable_ValidInput_ShouldExecuteForAllElements()
    {
        // Arrange
        IEnumerable<string> source = new[] { "a", "b", "c" };
        var results = new List<string>();

        // Act
        await source.ForEachAsync(async item =>
        {
            await Task.Delay(1); // Simulate async work
            results.Add(item.ToUpper());
        });

        // Assert
        Assert.Equal(new[] { "A", "B", "C" }, results);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_EmptyEnumerable_ShouldNotExecute()
    {
        // Arrange
        IEnumerable<int> source = Enumerable.Empty<int>();
        var executed = false;

        // Act
        await source.ForEachAsync(async item =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_NullSource_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> source = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            source.ForEachAsync(async item => await Task.Delay(1)));
        Assert.Equal("source", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_NullAction_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> source = new[] { 1, 2, 3 };
        Func<int, Task> action = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            source.ForEachAsync(action));
        Assert.Equal("action", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_ExceptionInAction_ShouldPropagateException()
    {
        // Arrange
        IEnumerable<int> source = new[] { 1, 2, 3 };
        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            source.ForEachAsync(async item =>
            {
                await Task.Delay(1);
                if (item == 2)
                {
                    throw expectedException;
                }
            }));
        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_WithLinqQuery_ShouldWork()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var evenNumbers = numbers.Where(x => x % 2 == 0);
        var results = new List<int>();

        // Act
        await evenNumbers.ForEachAsync(async item =>
        {
            await Task.Delay(1);
            results.Add(item * 10);
        });

        // Assert
        Assert.Equal(new[] { 20, 40 }, results);
    }

    #endregion
}