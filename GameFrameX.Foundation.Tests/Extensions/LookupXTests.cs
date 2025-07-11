using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// LookupX&lt;TKey, TElement&gt; 类单元测试
/// </summary>
public class LookupXTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithDictionary_ShouldCreateLookupX()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>
        {
            { "key1", new List<int> { 1, 2, 3 } },
            { "key2", new List<int> { 4, 5 } }
        };

        // Act
        var lookup = new LookupX<string, int>(dictionary);

        // Assert
        Assert.NotNull(lookup);
        Assert.Equal(2, lookup.Count);
    }

    [Fact]
    public void Constructor_WithConcurrentDictionary_ShouldCreateLookupX()
    {
        // Arrange
        var dictionary = new ConcurrentDictionary<string, List<int>>();
        dictionary.TryAdd("key1", new List<int> { 1, 2, 3 });
        dictionary.TryAdd("key2", new List<int> { 4, 5 });

        // Act
        var lookup = new LookupX<string, int>(dictionary);

        // Assert
        Assert.NotNull(lookup);
        Assert.Equal(2, lookup.Count);
    }

    [Fact]
    public void Constructor_WithNullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        IDictionary<string, List<int>> dictionary = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new LookupX<string, int>(dictionary));
        Assert.Equal("dic", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullConcurrentDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        ConcurrentDictionary<string, List<int>> dictionary = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new LookupX<string, int>(dictionary));
        Assert.Equal("dic", exception.ParamName);
    }

    #endregion

    #region Count Tests

    [Fact]
    public void Count_EmptyDictionary_ShouldReturnZero()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var count = lookup.Count;

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void Count_NonEmptyDictionary_ShouldReturnCorrectCount()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>
        {
            { "key1", new List<int> { 1, 2 } },
            { "key2", new List<int> { 3, 4, 5 } },
            { "key3", new List<int>() }
        };
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var count = lookup.Count;

        // Assert
        Assert.Equal(3, count);
    }

    #endregion

    #region Indexer Tests

    [Fact]
    public void Indexer_ExistingKey_ShouldReturnCorrectList()
    {
        // Arrange
        var expectedList = new List<int> { 1, 2, 3 };
        var dictionary = new Dictionary<string, List<int>>
        {
            { "key1", expectedList }
        };
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var result = lookup["key1"];

        // Assert
        Assert.Equal(expectedList, result);
    }

    [Fact]
    public void Indexer_NonExistingKey_ShouldReturnEmptyList()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var result = lookup["nonexistent"];

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void Indexer_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();
        var lookup = new LookupX<string, int>(dictionary);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => lookup[null]);
        Assert.Equal("key", exception.ParamName);
    }

    #endregion

    #region Contains Tests

    [Fact]
    public void Contains_ExistingKey_ShouldReturnTrue()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>
        {
            { "key1", new List<int> { 1, 2, 3 } }
        };
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var result = lookup.Contains("key1");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Contains_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var result = lookup.Contains("nonexistent");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Contains_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();
        var lookup = new LookupX<string, int>(dictionary);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => lookup.Contains(null));
        Assert.Equal("key", exception.ParamName);
    }

    #endregion

    #region GetEnumerator Tests

    [Fact]
    public void GetEnumerator_ShouldEnumerateAllLists()
    {
        // Arrange
        var list1 = new List<int> { 1, 2 };
        var list2 = new List<int> { 3, 4, 5 };
        var dictionary = new Dictionary<string, List<int>>
        {
            { "key1", list1 },
            { "key2", list2 }
        };
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var lists = lookup.ToList();

        // Assert
        Assert.Equal(2, lists.Count);
        Assert.Contains(list1, lists);
        Assert.Contains(list2, lists);
    }

    [Fact]
    public void GetEnumerator_EmptyLookup_ShouldReturnEmptyEnumerator()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var lists = lookup.ToList();

        // Assert
        Assert.Empty(lists);
    }

    [Fact]
    public void GetEnumerator_NonGeneric_ShouldWork()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>
        {
            { "key1", new List<int> { 1, 2 } }
        };
        var lookup = new LookupX<string, int>(dictionary);

        // Act
        var enumerator = ((System.Collections.IEnumerable)lookup).GetEnumerator();
        var hasNext = enumerator.MoveNext();
        var current = enumerator.Current;

        // Assert
        Assert.True(hasNext);
        Assert.NotNull(current);
        Assert.IsType<List<int>>(current);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void LookupX_WithComplexScenario_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<string>>
        {
            { "fruits", new List<string> { "apple", "banana", "orange" } },
            { "colors", new List<string> { "red", "green", "blue" } },
            { "empty", new List<string>() }
        };
        var lookup = new LookupX<string, string>(dictionary);

        // Act & Assert
        Assert.Equal(3, lookup.Count);
        Assert.True(lookup.Contains("fruits"));
        Assert.True(lookup.Contains("colors"));
        Assert.True(lookup.Contains("empty"));
        Assert.False(lookup.Contains("nonexistent"));

        Assert.Equal(3, lookup["fruits"].Count);
        Assert.Equal(3, lookup["colors"].Count);
        Assert.Empty(lookup["empty"]);
        Assert.Empty(lookup["nonexistent"]);

        var allLists = lookup.ToList();
        Assert.Equal(3, allLists.Count);
    }

    #endregion
}