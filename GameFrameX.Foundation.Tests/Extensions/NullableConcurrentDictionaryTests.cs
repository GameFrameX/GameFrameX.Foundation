// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// NullableConcurrentDictionary&lt;TKey, TValue&gt; 类单元测试
/// </summary>
public class NullableConcurrentDictionaryTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_Default_ShouldCreateEmptyDictionary()
    {
        // Arrange & Act
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Assert
        Assert.Empty(dictionary);
        Assert.Equal(default(int), dictionary["nonexistent"]);
    }

    [Fact]
    public void Constructor_WithFallbackValue_ShouldSetFallbackValue()
    {
        // Arrange
        var fallbackValue = 42;

        // Act
        var dictionary = new NullableConcurrentDictionary<string, int>(new FallbackValue<int>(fallbackValue));

        // Assert
        Assert.Empty(dictionary);
        Assert.Equal(fallbackValue, dictionary["nonexistent"]);
    }

    [Fact]
    public void Constructor_WithConcurrencyLevelAndCapacity_ShouldCreateDictionary()
    {
        // Arrange
        var concurrencyLevel = 4;
        var capacity = 10;

        // Act
        var dictionary = new NullableConcurrentDictionary<string, int>(concurrencyLevel, capacity);

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
        Assert.Throws<ArgumentOutOfRangeException>(() => new NullableConcurrentDictionary<string, int>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_WithZeroConcurrencyLevel_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var concurrencyLevel = 0;
        var capacity = 10;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new NullableConcurrentDictionary<string, int>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_WithNegativeCapacity_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var concurrencyLevel = 4;
        var capacity = -1;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new NullableConcurrentDictionary<string, int>(concurrencyLevel, capacity));
    }

    [Fact]
    public void Constructor_WithZeroCapacity_ShouldCreateEmptyDictionary()
    {
        // Arrange
        var concurrencyLevel = 4;
        var capacity = 0;

        // Act
        var dictionary = new NullableConcurrentDictionary<string, int>(concurrencyLevel, capacity);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithComparer_ShouldCreateDictionaryWithComparer()
    {
        // Arrange
        var comparer = EqualityComparer<NullObject<string>>.Default;

        // Act
        var dictionary = new NullableConcurrentDictionary<string, int>(comparer);

        // Assert
        Assert.Empty(dictionary);
    }

    [Fact]
    public void Constructor_WithNullComparer_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEqualityComparer<NullObject<string>> comparer = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new NullableConcurrentDictionary<string, int>(comparer));
    }

    #endregion

    #region Indexer Tests

    [Fact]
    public void Indexer_NullObjectKey_GetExistingValue_ShouldReturnValue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
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
        var dictionary = new NullableConcurrentDictionary<string, int>(new FallbackValue<int>(fallbackValue));
        var key = new NullObject<string>("nonexistent");

        // Act
        var result = dictionary[key];

        // Assert
        Assert.Equal(fallbackValue, result);
    }

    [Fact]
    public void Indexer_NullObjectKey_SetValue_ShouldSetValue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        var key = new NullObject<string>("test");

        // Act
        dictionary[key] = 42;

        // Assert
        Assert.Equal(42, dictionary[key]);
    }

    [Fact]
    public void Indexer_TKey_WithNullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();

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
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<KeyValuePair<string, int>, bool> condition = kvp => kvp.Value == 2;

        // Act
        var result = dictionary[condition];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionKeyValuePair_WithNullCondition_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        Func<KeyValuePair<string, int>, bool> condition = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary[condition]);
    }

    [Fact]
    public void Indexer_ConditionKeyValue_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<string, int, bool> condition = (key, value) => value == 2;

        // Act
        var result = dictionary[condition];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionKey_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<string, bool> condition = key => key == "key2";

        // Act
        var result = dictionary[condition];

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void Indexer_ConditionValue_ShouldReturnMatchingValue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["key1"] = 1;
        dictionary["key2"] = 2;
        dictionary["key3"] = 3;
        Func<int, bool> condition = value => value == 2;

        // Act
        var result = dictionary[condition];

        // Assert
        Assert.Equal(2, result);
    }

    #endregion

    #region ContainsKey Tests

    [Fact]
    public void ContainsKey_ExistingKey_ShouldReturnTrue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
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
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Act
        var result = dictionary.ContainsKey("nonexistent");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsKey_NullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        string nullKey = null;
        dictionary[nullKey] = 42;

        // Act
        var result = dictionary.ContainsKey(nullKey);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region TryAdd Tests

    [Fact]
    public void TryAdd_ValidKeyValue_ShouldAddToDict()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Act
        var result = dictionary.TryAdd("test", 42);

        // Assert
        Assert.True(result);
        Assert.Equal(42, dictionary["test"]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void TryAdd_NullKey_ShouldWork()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Act
        string nullKey = null;
        var result = dictionary.TryAdd(nullKey, 42);

        // Assert
        Assert.True(result);
        Assert.Equal(42, dictionary[nullKey]);
        Assert.Single(dictionary);
    }

    [Fact]
    public void TryAdd_DuplicateKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary.TryAdd("test", 42);

        // Act
        var result = dictionary.TryAdd("test", 43);

        // Assert
        Assert.False(result);
        Assert.Equal(42, dictionary["test"]); // Original value should remain
    }

    #endregion

    #region TryRemove Tests

    [Fact]
    public void TryRemove_ExistingKey_ShouldReturnTrueAndRemoveKey()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary.TryRemove("test", out var value);

        // Assert
        Assert.True(result);
        Assert.Equal(42, value);
        Assert.Empty(dictionary);
    }

    [Fact]
    public void TryRemove_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Act
        var result = dictionary.TryRemove("nonexistent", out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default(int), value);
    }

    #endregion

    #region TryUpdate Tests

    [Fact]
    public void TryUpdate_ExistingKeyWithCorrectComparison_ShouldReturnTrueAndUpdate()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary.TryUpdate("test", 100, 42);

        // Assert
        Assert.True(result);
        Assert.Equal(100, dictionary["test"]);
    }

    [Fact]
    public void TryUpdate_ExistingKeyWithIncorrectComparison_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
        dictionary["test"] = 42;

        // Act
        var result = dictionary.TryUpdate("test", 100, 99); // Wrong comparison value

        // Assert
        Assert.False(result);
        Assert.Equal(42, dictionary["test"]); // Original value should remain
    }

    [Fact]
    public void TryUpdate_NonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Act
        var result = dictionary.TryUpdate("nonexistent", 100, 42);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region TryGetValue Tests

    [Fact]
    public void TryGetValue_ExistingKey_ShouldReturnTrueAndValue()
    {
        // Arrange
        var dictionary = new NullableConcurrentDictionary<string, int>();
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
        var dictionary = new NullableConcurrentDictionary<string, int>();

        // Act
        var result = dictionary.TryGetValue("nonexistent", out var value);

        // Assert
        Assert.False(result);
        Assert.Equal(default(int), value);
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
        NullableConcurrentDictionary<string, int> nullableDictionary = sourceDictionary;

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
        NullableConcurrentDictionary<string, int> nullableDictionary = sourceDictionary;

        // Assert
        Assert.Equal(2, nullableDictionary.Count);
        Assert.Equal(1, nullableDictionary["key1"]);
        Assert.Equal(2, nullableDictionary["key2"]);
    }

    [Fact]
    public void ImplicitConversion_ToConcurrentDictionary_ShouldConvertCorrectly()
    {
        // Arrange
        var nullableDictionary = new NullableConcurrentDictionary<string, int>();
        nullableDictionary["key1"] = 1;
        nullableDictionary["key2"] = 2;

        // Act
        ConcurrentDictionary<string, int> concurrentDictionary = nullableDictionary;

        // Assert
        Assert.Equal(2, concurrentDictionary.Count);
        Assert.Equal(1, concurrentDictionary["key1"]);
        Assert.Equal(2, concurrentDictionary["key2"]);
    }

    [Fact]
    public void ImplicitConversion_FromNullDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        Dictionary<string, int> sourceDictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            NullableConcurrentDictionary<string, int> nullableDictionary = sourceDictionary;
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
            NullableConcurrentDictionary<string, int> nullableDictionary = sourceDictionary;
        });
    }

    [Fact]
    public void ImplicitConversion_ToNullConcurrentDictionary_ShouldThrowArgumentNullException()
    {
        // Arrange
        NullableConcurrentDictionary<string, int> nullableDictionary = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            ConcurrentDictionary<string, int> concurrentDictionary = nullableDictionary;
        });
    }

    #endregion
}