using System;
using System.Collections.Generic;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// DisposableConcurrentDictionary&lt;TKey, TValue&gt; 类单元测试
/// </summary>
public class DisposableConcurrentDictionaryTests
{
    /// <summary>
    /// 用于测试的可释放对象
    /// </summary>
    private class TestDisposable : IDisposable
    {
        public bool IsDisposed { get; private set; }
        public string Name { get; set; }

        public TestDisposable(string name)
        {
            Name = name;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_Default_ShouldCreateEmptyDictionary()
    {
        // Arrange & Act
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();

        // Assert
        Assert.Empty(dictionary);
        Assert.Null(dictionary.FallbackValue);
    }

    [Fact]
    public void Constructor_WithFallbackValue_ShouldSetFallbackValue()
    {
        // Arrange
        var fallbackValue = new TestDisposable("fallback");

        // Act
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>(fallbackValue);

        // Assert
        Assert.Empty(dictionary);
        Assert.Equal(fallbackValue, dictionary.FallbackValue);
    }

    [Fact]
    public void Constructor_WithConcurrencyLevelAndCapacity_ShouldCreateDictionary()
    {
        // Arrange
        var concurrencyLevel = 4;
        var capacity = 10;

        // Act
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>(concurrencyLevel, capacity);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithNegativeConcurrencyLevel_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var concurrencyLevel = -1;
        var capacity = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new DisposableConcurrentDictionary<string, TestDisposable>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_WithZeroConcurrencyLevel_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var concurrencyLevel = 0;
        var capacity = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new DisposableConcurrentDictionary<string, TestDisposable>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var concurrencyLevel = 4;
        var capacity = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new DisposableConcurrentDictionary<string, TestDisposable>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_WithComparer_ShouldCreateDictionaryWithComparer()
    {
        // Arrange
        var comparer = EqualityComparer<NullObject<string>>.Default;

        // Act
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>(comparer);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithNullComparer_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEqualityComparer<NullObject<string>> comparer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DisposableConcurrentDictionary<string, TestDisposable>(comparer));
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void Dispose_WithValues_ShouldDisposeAllValues()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item1 = new TestDisposable("item1");
        var item2 = new TestDisposable("item2");
        var item3 = new TestDisposable("item3");
        
        dictionary["key1"] = item1;
        dictionary["key2"] = item2;
        dictionary["key3"] = item3;

        // Act
        dictionary.Dispose();

        // Assert
        Assert.True(item1.IsDisposed);
        Assert.True(item2.IsDisposed);
        Assert.True(item3.IsDisposed);
    }

    [Fact]
    public void Dispose_WithNullValues_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item1 = new TestDisposable("item1");
        
        dictionary["key1"] = item1;
        dictionary["key2"] = null;

        // Act & Assert
        dictionary.Dispose(); // Should not throw
        Assert.True(item1.IsDisposed);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item1 = new TestDisposable("item1");
        dictionary["key1"] = item1;

        // Act & Assert
        dictionary.Dispose();
        dictionary.Dispose(); // Should not throw
        Assert.True(item1.IsDisposed);
    }

    [Fact]
    public void Dispose_EmptyDictionary_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();

        // Act & Assert
        dictionary.Dispose(); // Should not throw
    }

    #endregion

    #region Functionality Tests

    [Fact]
    public void AddAndRetrieve_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item = new TestDisposable("test");

        // Act
        dictionary["key"] = item;
        var retrieved = dictionary["key"];

        // Assert
        Assert.Equal(item, retrieved);
        Assert.False(item.IsDisposed);
    }

    [Fact]
    public void TryAdd_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item = new TestDisposable("test");

        // Act
        var result = dictionary.TryAdd("key", item);

        // Assert
        Assert.True(result);
        Assert.Equal(item, dictionary["key"]);
    }

    [Fact]
    public void Remove_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item = new TestDisposable("test");
        dictionary["key"] = item;

        // Act
        var result = dictionary.Remove("key");

        // Assert
        Assert.True(result);
        Assert.False(dictionary.ContainsKey("key"));
        Assert.False(item.IsDisposed); // Remove should not dispose the item
    }

    [Fact]
    public void ContainsKey_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item = new TestDisposable("test");
        dictionary["key"] = item;

        // Act & Assert
        Assert.True(dictionary.ContainsKey("key"));
        Assert.False(dictionary.ContainsKey("nonexistent"));
    }

    [Fact]
    public void TryGetValue_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item = new TestDisposable("test");
        dictionary["key"] = item;

        // Act
        var found = dictionary.TryGetValue("key", out var value);
        var notFound = dictionary.TryGetValue("nonexistent", out var notFoundValue);

        // Assert
        Assert.True(found);
        Assert.Equal(item, value);
        Assert.False(notFound);
        Assert.Null(notFoundValue);
    }

    [Fact]
    public void FallbackValue_ShouldWorkCorrectly()
    {
        // Arrange
        var fallbackItem = new TestDisposable("fallback");
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>(fallbackItem);

        // Act
        var result = dictionary["nonexistent"];

        // Assert
        Assert.Equal(fallbackItem, result);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void DisposeAfterOperations_ShouldDisposeAllItems()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TestDisposable>();
        var item1 = new TestDisposable("item1");
        var item2 = new TestDisposable("item2");
        var item3 = new TestDisposable("item3");

        // Act
        dictionary.TryAdd("key1", item1);
        dictionary["key2"] = item2;
        dictionary.TryAdd("key3", item3);
        dictionary.Remove("key2"); // Remove one item
        dictionary.Dispose();

        // Assert
        Assert.True(item1.IsDisposed);
        Assert.False(item2.IsDisposed); // Was removed before dispose
        Assert.True(item3.IsDisposed);
    }

    #endregion
}