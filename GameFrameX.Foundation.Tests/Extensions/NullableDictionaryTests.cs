using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// NullableDictionary&lt;TKey, TValue&gt; 类单元测试
/// </summary>
public class NullableDictionaryTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_Default_ShouldCreateEmptyDictionary()
    {
        // Arrange & Act
        var dictionary = new NullableDictionary<string, int>();

        // Assert
        Assert.Empty(dictionary);
        Assert.Equal(default(int), dictionary["nonexistent"]);
    }

    [Fact]
    public void Constructor_WithFallbackValue_ShouldSetFallbackValue()
    {
        // Arrange
        var fallbackValue = 42;
        FallbackValue<int> fb = fallbackValue;

        // Act
        var dictionary = new NullableDictionary<string, int>(fb);

        // Assert
        Assert.Empty(dictionary);
        Assert.Equal(fallbackValue, dictionary["nonexistent"]);
    }

    [Fact]
    public void Constructor_WithCapacity_ShouldCreateDictionaryWithCapacity()
    {
        // Arrange
        var capacity = 10;

        // Act
        var dictionary = new NullableDictionary<string, int>(capacity);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var capacity = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new NullableDictionary<string, int>(capacity));
    }

    [Fact]
    public void Constructor_WithComparer_ShouldCreateDictionaryWithComparer()
    {
        // Arrange
        var comparer = EqualityComparer<NullObject<string>>.Default;

        // Act
        var dictionary = new NullableDictionary<string, int>(comparer);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithNullComparer_ShouldUseDefaultComparer()
    {
        // Arrange
        IEqualityComparer<NullObject<string>> comparer = null;

        // Act
        var dictionary = new NullableDictionary<string, int>(comparer);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithCapacityAndComparer_ShouldCreateDictionary()
    {
        // Arrange
        var capacity = 10;
        var comparer = EqualityComparer<NullObject<string>>.Default;

        // Act
        var dictionary = new NullableDictionary<string, int>(capacity, comparer);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithDictionary_ShouldCopyDictionary()
    {
        // Arrange
        var sourceDictionary = new Dictionary<NullObject<string>, int>
        {
            { new NullObject<string>("key1"), 1 },
            { new NullObject<string>("key2"), 2 }
        };

        // Act
        var dictionary = new NullableDictionary<string, int>(sourceDictionary);

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal(1, dictionary["key1"]);
        Assert.Equal(2, dictionary["key2"]);
    }

    [Fact]
    public void Constructor_WithNullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        IDictionary<NullObject<string>, int> dictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new NullableDictionary<string, int>(dictionary));
    }

    [Fact]
    public void Constructor_WithDictionaryAndComparer_ShouldCopyDictionary()
    {
        // Arrange
        var sourceDictionary = new Dictionary<NullObject<string>, int>
        {
            { new NullObject<string>("key1"), 1 },
            { new NullObject<string>("key2"), 2 }
        };
        var comparer = EqualityComparer<NullObject<string>>.Default;

        // Act
        var dictionary = new NullableDictionary<string, int>(sourceDictionary, comparer);

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal(1, dictionary["key1"]);
        Assert.Equal(2, dictionary["key2"]);
    }

    #endregion

    #region Indexer Tests

    [Fact]
    public void Indexer_NullObjectKey_GetExistingValue_ShouldReturnValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        var key = new NullObject<string>("test");
        dictionary[key] = 42;

        // Act
        var result = dictionary[key];

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void Indexer_NullObjectKey_GetNonExistingValue_ShouldReturnFallbackValue()
    {
        // Arrange
        var fallbackValue = 99;
        FallbackValue<int> fb = fallbackValue;
        var dictionary = new NullableDictionary<string, int>(fb);
        var key = new NullObject<string>("nonexistent");

        // Act
        var result = dictionary[key];

        // Assert
        Assert.Equal(fallbackValue, result);
    }

    [Fact]
    public void Indexer_TKey_GetExistingValue_ShouldReturnValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary["test"];

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void Indexer_TKey_GetNonExistingValue_ShouldReturnFallbackValue()
    {
        // Arrange
        var fallbackValue = 99;
        FallbackValue<int> fb = fallbackValue;
        var dictionary = new NullableDictionary<string, int>(fb);

        // Act
        var result = dictionary["nonexistent"];

        // Assert
        Assert.Equal(fallbackValue, result);
    }

    [Fact]
    public void Indexer_TKey_SetValue_ShouldSetValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        dictionary["test"] = 42;

        // Assert
        Assert.Equal(42, dictionary["test"]);
    }

    [Fact]
    public void Indexer_TKey_WithNullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        string nullKey = null;
        dictionary[nullKey] = 42;
        var result = dictionary[nullKey];

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void Indexer_ConditionKeyValuePair_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<KeyValuePair<string, int>, bool> condition = pair => pair.Key == "key2";

        // Act
        var result = dictionary[condition];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionKeyValuePair_WithNullCondition_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        Func<KeyValuePair<string, int>, bool> condition = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary[condition]);
    }

    [Fact]
    public void Indexer_ConditionKeyValue_WithNullCondition_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        Func<string, int, bool> condition = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary[condition]);
    }

    [Fact]
    public void Indexer_ConditionKey_WithNullCondition_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        Func<string, bool> condition = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary[condition]);
    }

    [Fact]
    public void Indexer_ConditionValue_WithNullCondition_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        Func<int, bool> condition = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary[condition]);
    }

    [Fact]
    public void Indexer_ConditionKeyValue_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;

        // Act
        var result = dictionary[(key, value) => key == "key2" && value == 2];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionKey_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;

        // Act
        var result = dictionary[key => key == "key2"];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionValue_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;

        // Act
        var result = dictionary[value => value == 2];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionKeyValue_SetValue_ShouldUpdateMatchingItems()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<string, int, bool> condition = (key, value) => key == "key1";

        // Act
        dictionary[condition] = 100;

        // Assert
        Assert.Equal(100, dictionary["key1"]);
    }

    [Fact]
    public void Indexer_ConditionKey_SetValue_ShouldUpdateMatchingItems()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<string, bool> condition = key => key == "key1";

        // Act
        dictionary[condition] = 100;

        // Assert
        Assert.Equal(100, dictionary["key1"]);
    }

    [Fact]
    public void Indexer_ConditionValue_SetValue_ShouldUpdateMatchingItems()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<int, bool> condition = value => value == 1;

        // Act
        dictionary[condition] = 100;

        // Assert
        Assert.Equal(100, dictionary["key1"]);
    }

    [Fact]
    public void Indexer_ConditionKeyValuePair_SetValue_ShouldUpdateMatchingItems()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<KeyValuePair<string, int>, bool> condition = pair => pair.Key == "key1";

        // Act
        dictionary[condition] = 100;

        // Assert
        Assert.Equal(100, dictionary["key1"]);
    }

    #endregion

    #region ContainsKey Tests

    [Fact]
    public void ContainsKey_ExistingKey_ShouldReturnTrue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary.ContainsKey("test");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsKey_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        var result = dictionary.ContainsKey("nonexistent");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsKey_NullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        string nullKey = null;
        dictionary[nullKey] = 42;

        // Act
        var result = dictionary.ContainsKey(nullKey);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region Add Tests

    [Fact]
    public void Add_ValidKeyValue_ShouldAddToDict()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        dictionary.Add("test", 42);

        // Assert
        Assert.Equal(42, dictionary["test"]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void Add_NullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        string nullKey = null;
        dictionary.Add(nullKey, 42);

        // Assert
        Assert.Equal(42, dictionary[nullKey]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void Add_DuplicateKey_ShouldThrowArgumentException()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary.Add("test", 42);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => dictionary.Add("test", 43));
    }

    #endregion

    #region Remove Tests

    [Fact]
    public void Remove_ExistingKey_ShouldReturnTrueAndRemoveKey()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary.Remove("test");

        // Assert
        Assert.True(result);
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Remove_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        var result = dictionary.Remove("nonexistent");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Remove_NullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        string nullKey = null;
        dictionary[nullKey] = 42;

        // Act
        var result = dictionary.Remove(nullKey);

        // Assert
        Assert.True(result);
        Assert.Empty(dictionary);
    }

    #endregion

    #region TryGetValue Tests

    [Fact]
    public void TryGetValue_ExistingKey_ShouldReturnTrueAndValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary.TryGetValue("test", out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TryGetValue_NonExistingKey_ShouldReturnFalseAndDefaultValue()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();

        // Act
        var result = dictionary.TryGetValue("nonexistent", out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default(int), value);
    }

    [Fact]
    public void TryGetValue_NullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableDictionary<string, int>();
        string nullKey = null;
        dictionary[nullKey] = 42;

        // Act
        var result = dictionary.TryGetValue(nullKey, out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
    }

    #endregion

    #region Implicit Conversion Tests

    [Fact]
    public void ImplicitConversion_FromDictionary_ShouldConvertCorrectly()
    {
        // Arrange
        var sourceDictionary = new Dictionary<string, int>
        {
            { "key1", 1 },
            { "key2", 2 }
        };

        // Act
        NullableDictionary<string, int> nullableDictionary = sourceDictionary;

        // Assert
        Assert.Equal(2, nullableDictionary.Count);
        Assert.Equal(1, nullableDictionary["key1"]);
        Assert.Equal(2, nullableDictionary["key2"]);
    }

    [Fact]
    public void ImplicitConversion_FromConcurrentDictionary_ShouldConvertCorrectly()
    {
        // Arrange
        var sourceDictionary = new ConcurrentDictionary<string, int>();
        sourceDictionary["key1"] = 1;
        sourceDictionary["key2"] = 2;

        // Act
        NullableDictionary<string, int> nullableDictionary = sourceDictionary;

        // Assert
        Assert.Equal(2, nullableDictionary.Count);
        Assert.Equal(1, nullableDictionary["key1"]);
        Assert.Equal(2, nullableDictionary["key2"]);
    }

    [Fact]
    public void ImplicitConversion_ToDictionary_ShouldConvertCorrectly()
    {
        // Arrange
        var nullableDictionary = new NullableDictionary<string, int>();
        nullableDictionary["key1"] = 1;
        nullableDictionary["key2"] = 2;

        // Act
        Dictionary<string, int> dictionary = nullableDictionary;

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal(1, dictionary["key1"]);
        Assert.Equal(2, dictionary["key2"]);
    }

    [Fact]
    public void ImplicitConversion_FromNullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<string, int> sourceDictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            NullableDictionary<string, int> nullableDictionary = sourceDictionary;
        });
    }

    [Fact]
    public void ImplicitConversion_FromNullConcurrentDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        ConcurrentDictionary<string, int> sourceDictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            NullableDictionary<string, int> nullableDictionary = sourceDictionary;
        });
    }

    [Fact]
    public void ImplicitConversion_ToNullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        NullableDictionary<string, int> nullableDictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            Dictionary<string, int> dictionary = nullableDictionary;
        });
    }

    #endregion
}