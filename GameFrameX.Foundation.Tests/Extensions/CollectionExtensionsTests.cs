using System;
using System.Collections.Generic;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// Tests for <see cref="CollectionExtensions"/> class.
/// </summary>
public class CollectionExtensionsTests
{
    #region IsNullOrEmpty Tests

    [Fact]
    public void IsNullOrEmpty_NullCollection_ShouldReturnTrue()
    {
        // Arrange
        ICollection<string> collection = null;

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_EmptyCollection_ShouldReturnTrue()
    {
        // Arrange
        var collection = new List<string>();

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsNullOrEmpty_NonEmptyCollection_ShouldReturnFalse()
    {
        // Arrange
        var collection = new List<string> { "item" };

        // Act
        var result = collection.IsNullOrEmpty();

        // Assert
        Assert.False(result);
    }

    #endregion

    #region AddRange Tests

    [Fact]
    public void AddRange_NullEnumerable_ShouldThrowArgumentNullException()
    {
        // Arrange
        var hashSet = new HashSet<string>();
        IEnumerable<string> enumerable = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => hashSet.AddRange(enumerable));
    }

    #endregion

    #region Merge Tests

    [Fact]
    public void Merge_NullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<string, int> dictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.Merge("key", 1, (a, b) => a + b));
    }

    [Fact]
    public void Merge_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.Merge(null, 1, (a, b) => a + b));
    }

    [Fact]
    public void Merge_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.Merge("key", null, (a, b) => a + b));
    }

    [Fact]
    public void Merge_NullFunction_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.Merge("key", 1, null));
    }

    [Fact]
    public void Merge_NewKey_ShouldAddValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act
        dictionary.Merge("key1", 5, (a, b) => a + b);

        // Assert
        Assert.Equal(5, dictionary["key1"]);
    }

    [Fact]
    public void Merge_ExistingKey_ShouldMergeValues()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { ["key1"] = 3 };

        // Act
        dictionary.Merge("key1", 5, (a, b) => a + b);

        // Assert
        Assert.Equal(8, dictionary["key1"]);
    }

    #endregion

    #region GetOrAdd Tests

    [Fact]
    public void GetOrAdd_NullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<string, int> dictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd("key", k => 1));
    }

    [Fact]
    public void GetOrAdd_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd(null, k => 1));
    }

    [Fact]
    public void GetOrAdd_NullValueGetter_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd("key", (Func<string, int>)null));
    }

    [Fact]
    public void GetOrAdd_ExistingKey_ShouldReturnExistingValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { ["key1"] = 5 };

        // Act
        var result = dictionary.GetOrAdd("key1", k => 10);

        // Assert
        Assert.Equal(5, result);
        Assert.Equal(5, dictionary["key1"]);
    }

    [Fact]
    public void GetOrAdd_NewKey_ShouldAddAndReturnNewValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act
        var result = dictionary.GetOrAdd("key1", k => 10);

        // Assert
        Assert.Equal(10, result);
        Assert.Equal(10, dictionary["key1"]);
    }

    [Fact]
    public void GetOrAdd_WithNew_NullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<string, List<int>> dictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd("key"));
    }

    [Fact]
    public void GetOrAdd_WithNew_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAdd(null));
    }

    [Fact]
    public void GetOrAdd_WithNew_NewKey_ShouldCreateNewInstance()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();

        // Act
        var result = dictionary.GetOrAdd("key1");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<int>>(result);
        Assert.Same(result, dictionary["key1"]);
    }

    #endregion

    #region RemoveIf Dictionary Tests

    [Fact]
    public void RemoveIf_Dictionary_NullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<string, int> dictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.RemoveIf((k, v) => true));
    }

    [Fact]
    public void RemoveIf_Dictionary_NullPredicate_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.RemoveIf(null));
    }

    [Fact]
    public void RemoveIf_Dictionary_ValidParameters_ShouldRemoveMatchingItems()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>
        {
            ["key1"] = 1,
            ["key2"] = 2,
            ["key3"] = 3
        };

        // Act
        var removedCount = dictionary.RemoveIf((k, v) => v > 1);

        // Assert
        Assert.Equal(2, removedCount);
        Assert.Single(dictionary);
        Assert.True(dictionary.ContainsKey("key1"));
    }

    #endregion

    #region Random Tests

    [Fact]
    public void Random_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<string> list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.Random());
    }

    [Fact]
    public void Random_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<string>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => list.Random());
    }

    [Fact]
    public void Random_ValidList_ShouldReturnItem()
    {
        // Arrange
        var list = new List<string> { "item1", "item2", "item3" };

        // Act
        var result = list.Random();

        // Assert
        Assert.Contains(result, list);
    }

    #endregion

    #region Upset Tests

    [Fact]
    public void Upset_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<string> list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.Upset());
    }

    [Fact]
    public void Upset_ValidList_ShouldNotThrow()
    {
        // Arrange
        var list = new List<string> { "item1", "item2", "item3" };
        var originalItems = new List<string>(list);

        // Act
        list.Upset();

        // Assert
        Assert.Equal(originalItems.Count, list.Count);
        foreach (var item in originalItems)
        {
            Assert.Contains(item, list);
        }
    }

    [Fact]
    public void Upset_EmptyList_ShouldNotThrow()
    {
        // Arrange
        var list = new List<string>();

        // Act & Assert
        list.Upset(); // Should not throw
        Assert.Empty(list);
    }

    #endregion

    #region RemoveIf List Tests

    [Fact]
    public void RemoveIf_List_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<string> list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.RemoveIf(x => true));
    }

    [Fact]
    public void RemoveIf_List_NullCondition_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new List<string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.RemoveIf(null));
    }

    [Fact]
    public void RemoveIf_List_ValidParameters_ShouldRemoveMatchingItems()
    {
        // Arrange
        var list = new List<string> { "apple", "banana", "apricot", "cherry" };

        // Act
        list.RemoveIf(x => x.StartsWith("a"));

        // Assert
        Assert.Equal(2, list.Count);
        Assert.Contains("banana", list);
        Assert.Contains("cherry", list);
    }

    #endregion
}