using System;
using System.Collections.Generic;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// DisposableDictionary&lt;TKey, TValue&gt; 类单元测试
/// </summary>
public class DisposableDictionaryTests
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
        var dictionary = new DisposableDictionary<string, TestDisposable>();

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithFallbackValue_ShouldSetFallbackValue()
    {
        // Arrange
        var fallbackValue = new TestDisposable("fallback");

        // Act
        var dictionary = new DisposableDictionary<string, TestDisposable>(fallbackValue);

        // Assert
        Assert.Equal(fallbackValue, dictionary["nonexistent"]);
    }

    [Fact]
    public void Constructor_WithCapacity_ShouldCreateDictionaryWithCapacity()
    {
        // Arrange & Act
        var dictionary = new DisposableDictionary<string, TestDisposable>(10);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithDictionary_ShouldCopyValues()
    {
        // Arrange
        var source = new Dictionary<string, TestDisposable>
        {
            { "key1", new TestDisposable("value1") },
            { "key2", new TestDisposable("value2") }
        };

        // Act
        var dictionary = new DisposableDictionary<string, TestDisposable>(source);

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal("value1", dictionary["key1"].Name);
        Assert.Equal("value2", dictionary["key2"].Name);
    }

    #endregion

    #region Dispose Tests

    [Fact]
    public void Dispose_ShouldDisposeAllValues()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        var value1 = new TestDisposable("value1");
        var value2 = new TestDisposable("value2");
        dictionary["key1"] = value1;
        dictionary["key2"] = value2;

        // Act
        dictionary.Dispose();

        // Assert
        Assert.True(value1.IsDisposed);
        Assert.True(value2.IsDisposed);
    }

    [Fact]
    public void Dispose_WithNullValues_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        dictionary["key1"] = new TestDisposable("value1");
        dictionary["key2"] = null;

        // Act & Assert
        dictionary.Dispose(); // Should not throw
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        var value = new TestDisposable("value");
        dictionary["key"] = value;

        // Act & Assert
        dictionary.Dispose();
        dictionary.Dispose(); // Should not throw
        Assert.True(value.IsDisposed);
    }

    [Fact]
    public void Finalizer_ShouldDisposeValues()
    {
        // Arrange
        var value = new TestDisposable("value");
        
        // Act
        CreateAndAbandonDictionary(value);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        // Assert
        Assert.True(value.IsDisposed);
    }

    private static void CreateAndAbandonDictionary(TestDisposable value)
    {
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        dictionary["key"] = value;
        // Dictionary goes out of scope and becomes eligible for finalization
    }

    #endregion

    #region Inheritance Tests

    [Fact]
    public void InheritsFromNullableDictionary_ShouldSupportNullKeys()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        var value = new TestDisposable("value");

        // Act
        dictionary[(string)null] = value;

        // Assert
        Assert.Equal(value, dictionary[(string)null]);
    }

    [Fact]
    public void InheritsFromNullableDictionary_ShouldSupportFallbackValue()
    {
        // Arrange
        var fallbackValue = new TestDisposable("fallback");
        var dictionary = new DisposableDictionary<string, TestDisposable>(fallbackValue);

        // Act
        var result = dictionary["nonexistent"];

        // Assert
        Assert.Equal(fallbackValue, result);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void AddAndDispose_ShouldDisposeAddedValues()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        var value1 = new TestDisposable("value1");
        var value2 = new TestDisposable("value2");

        // Act
        dictionary.Add("key1", value1);
        dictionary["key2"] = value2;
        dictionary.Dispose();

        // Assert
        Assert.True(value1.IsDisposed);
        Assert.True(value2.IsDisposed);
    }

    [Fact]
    public void RemoveAndDispose_ShouldOnlyDisposeRemainingValues()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TestDisposable>();
        var value1 = new TestDisposable("value1");
        var value2 = new TestDisposable("value2");
        dictionary["key1"] = value1;
        dictionary["key2"] = value2;

        // Act
        dictionary.Remove("key1");
        dictionary.Dispose();

        // Assert
        Assert.False(value1.IsDisposed); // Removed before dispose
        Assert.True(value2.IsDisposed);  // Still in dictionary when disposed
    }

    #endregion
}