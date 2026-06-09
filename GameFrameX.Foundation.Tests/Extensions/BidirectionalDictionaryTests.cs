using System;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// Tests for <see cref="BidirectionalDictionary{TKey, TValue}"/> class.
/// </summary>
public class BidirectionalDictionaryTests
{
    [Fact]
    public void Constructor_NegativeInitialCapacity_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new BidirectionalDictionary<string, int>(-1));
    }

    [Fact]
    public void Constructor_ZeroInitialCapacity_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var dictionary = new BidirectionalDictionary<string, int>(0);
        Assert.NotNull(dictionary);
    }

    [Fact]
    public void Constructor_PositiveInitialCapacity_ShouldNotThrow()
    {
        // Arrange & Act & Assert
        var dictionary = new BidirectionalDictionary<string, int>(10);
        Assert.NotNull(dictionary);
    }

    [Fact]
    public void TryGetKey_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryGetKey(null, out _));
    }

    [Fact]
    public void TryGetKey_ValidValue_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act & Assert
        var result = dictionary.TryGetKey("value1", out var key);
        Assert.True(result);
        Assert.Equal("key1", key);
    }

    [Fact]
    public void TryGetValue_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryGetValue(null, out _));
    }

    [Fact]
    public void TryGetValue_ValidKey_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act & Assert
        var result = dictionary.TryGetValue("key1", out var value);
        Assert.True(result);
        Assert.Equal("value1", value);
    }

    [Fact]
    public void TryAdd_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryAdd(null, "value1"));
    }

    [Fact]
    public void TryAdd_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryAdd("key1", null));
    }

    [Fact]
    public void TryAdd_ValidKeyValue_ShouldNotThrow()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act & Assert
        var result = dictionary.TryAdd("key1", "value1");
        Assert.True(result);
    }

    [Fact]
    public void TryAdd_DuplicateKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act
        var result = dictionary.TryAdd("key1", "value2");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryAdd_DuplicateValue_ShouldReturnFalseAndKeepMappingsConsistent()
    {
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        var result = dictionary.TryAdd("key2", "value1");

        Assert.False(result);
        Assert.False(dictionary.ContainsKey("key2"));
        Assert.True(dictionary.TryGetKey("value1", out var key));
        Assert.Equal("key1", key);
        Assert.Single(dictionary);
    }

    [Fact]
    public void TryUpdateByKey_ShouldUpdateBothMappings()
    {
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        Assert.True(dictionary.TryUpdateByKey("key1", "value2"));

        Assert.False(dictionary.ContainsValue("value1"));
        Assert.True(dictionary.TryGetValue("key1", out var value));
        Assert.Equal("value2", value);
        Assert.True(dictionary.TryGetKey("value2", out var key));
        Assert.Equal("key1", key);
    }

    [Fact]
    public void TryUpdateByValue_ShouldUpdateBothMappings()
    {
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        Assert.True(dictionary.TryUpdateByValue("value1", "key2"));

        Assert.False(dictionary.ContainsKey("key1"));
        Assert.True(dictionary.TryGetValue("key2", out var value));
        Assert.Equal("value1", value);
    }

    [Fact]
    public void TryUpdate_ToExistingMapping_ShouldFailWithoutChangingState()
    {
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");
        dictionary.TryAdd("key2", "value2");

        Assert.False(dictionary.TryUpdateByKey("key1", "value2"));
        Assert.False(dictionary.TryUpdateByValue("value1", "key2"));

        Assert.Equal("value1", Assert.Single(dictionary.Where(x => x.Key == "key1")).Value);
        Assert.Equal(2, dictionary.Count);
    }

    [Fact]
    public void Constructor_WithComparers_ShouldUseComparerInBothDirections()
    {
        var dictionary = new BidirectionalDictionary<string, string>(
            0,
            StringComparer.OrdinalIgnoreCase,
            StringComparer.OrdinalIgnoreCase);

        Assert.True(dictionary.TryAdd("Key", "Value"));
        Assert.False(dictionary.TryAdd("KEY", "Other"));
        Assert.False(dictionary.TryAdd("Other", "VALUE"));
        Assert.True(dictionary.ContainsKey("key"));
        Assert.True(dictionary.ContainsValue("value"));
    }

    [Fact]
    public void Clear_ShouldRemoveAllEntries()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");
        dictionary.TryAdd("key2", "value2");

        // Act
        dictionary.Clear();

        // Assert
        Assert.False(dictionary.TryGetValue("key1", out _));
        Assert.False(dictionary.TryGetKey("value1", out _));
    }

    [Fact]
    public void BidirectionalMapping_ShouldWorkCorrectly()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, int>();
        dictionary.TryAdd("one", 1);
        dictionary.TryAdd("two", 2);

        // Act & Assert
        Assert.True(dictionary.TryGetValue("one", out var value1));
        Assert.Equal(1, value1);

        Assert.True(dictionary.TryGetKey(1, out var key1));
        Assert.Equal("one", key1);

        Assert.True(dictionary.TryGetValue("two", out var value2));
        Assert.Equal(2, value2);

        Assert.True(dictionary.TryGetKey(2, out var key2));
        Assert.Equal("two", key2);
    }
}
