using System;
using System.Collections.Generic;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// BidirectionalDictionary 补充边界测试：TryUpdateByKey / TryUpdateByValue 对不存在项的返回值、
/// null 参数保护、no-op 更新、值/键 冲突时不修改字典、TryRemove 不存在项行为。
/// </summary>
public class BidirectionalDictionaryAdditionalTests
{
    // ============================================================
    // TryUpdateByKey — 不存在的 key
    // ============================================================

    [Fact]
    public void TryUpdateByKey_NonExistentKey_ShouldReturnFalseAndNotModifyDictionary()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act
        var result = dictionary.TryUpdateByKey("nonExistentKey", "newValue");

        // Assert — 不存在时应返回 false，字典内容不变
        Assert.False(result);
        Assert.Single(dictionary);
        Assert.True(dictionary.TryGetValue("key1", out var value));
        Assert.Equal("value1", value);
        Assert.True(dictionary.TryGetKey("value1", out var key));
        Assert.Equal("key1", key);
    }

    [Fact]
    public void TryUpdateByKey_EmptyDictionary_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act
        var result = dictionary.TryUpdateByKey("key1", "value1");

        // Assert
        Assert.False(result);
        Assert.Empty(dictionary);
    }

    // ============================================================
    // TryUpdateByValue — 不存在的 value
    // ============================================================

    [Fact]
    public void TryUpdateByValue_NonExistentValue_ShouldReturnFalseAndNotModifyDictionary()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act
        var result = dictionary.TryUpdateByValue("nonExistentValue", "newKey");

        // Assert — 不存在时应返回 false，字典内容不变
        Assert.False(result);
        Assert.Single(dictionary);
        Assert.True(dictionary.TryGetValue("key1", out var value));
        Assert.Equal("value1", value);
        Assert.True(dictionary.TryGetKey("value1", out var key));
        Assert.Equal("key1", key);
    }

    [Fact]
    public void TryUpdateByValue_EmptyDictionary_ShouldReturnFalse()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();

        // Act
        var result = dictionary.TryUpdateByValue("value1", "key1");

        // Assert
        Assert.False(result);
        Assert.Empty(dictionary);
    }

    // ============================================================
    // null 参数保护
    // ============================================================

    [Fact]
    public void TryUpdateByKey_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryUpdateByKey(null, "value2"));
    }

    [Fact]
    public void TryUpdateByKey_NullNewValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryUpdateByKey("key1", null));
    }

    [Fact]
    public void TryUpdateByValue_NullValue_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryUpdateByValue(null, "key2"));
    }

    [Fact]
    public void TryUpdateByValue_NullNewKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.TryUpdateByValue("value1", null));
    }

    // ============================================================
    // no-op 更新（新值/新键与旧值/旧键相同）
    // ============================================================

    [Fact]
    public void TryUpdateByKey_NewValueEqualsOldValue_ShouldReturnTrueWithoutModification()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act — 用相同的 value 更新
        var result = dictionary.TryUpdateByKey("key1", "value1");

        // Assert — 新值与旧值相同时应返回 true，字典不变
        Assert.True(result);
        Assert.True(dictionary.TryGetValue("key1", out var value));
        Assert.Equal("value1", value);
        Assert.True(dictionary.TryGetKey("value1", out var key));
        Assert.Equal("key1", key);
        Assert.Single(dictionary);
    }

    [Fact]
    public void TryUpdateByValue_NewKeyEqualsOldKey_ShouldReturnTrueWithoutModification()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act — 用相同的 key 更新
        var result = dictionary.TryUpdateByValue("value1", "key1");

        // Assert — 新键与旧键相同时应返回 true，字典不变
        Assert.True(result);
        Assert.True(dictionary.TryGetValue("key1", out var value));
        Assert.Equal("value1", value);
        Assert.True(dictionary.TryGetKey("value1", out var key));
        Assert.Equal("key1", key);
        Assert.Single(dictionary);
    }

    // ============================================================
    // 值/键冲突 — 新值/新键已被其他映射占用
    // ============================================================

    [Fact]
    public void TryUpdateByKey_NewValueBelongsToDifferentKey_ShouldReturnFalseAndNotModify()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");
        dictionary.TryAdd("key2", "value2");

        // Act — 将 key1 更新为 value2，但 value2 已被 key2 占用
        var result = dictionary.TryUpdateByKey("key1", "value2");

        // Assert — 值已被其他键占用时应返回 false，字典不变
        Assert.False(result);
        Assert.Equal(2, dictionary.Count);
        Assert.True(dictionary.TryGetValue("key1", out var v1));
        Assert.Equal("value1", v1);
        Assert.True(dictionary.TryGetValue("key2", out var v2));
        Assert.Equal("value2", v2);
    }

    [Fact]
    public void TryUpdateByValue_NewKeyBelongsToDifferentValue_ShouldReturnFalseAndNotModify()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");
        dictionary.TryAdd("key2", "value2");

        // Act — 将 value1 更新为 key2，但 key2 已被 value2 占用
        var result = dictionary.TryUpdateByValue("value1", "key2");

        // Assert — 键已被其他值占用时应返回 false，字典不变
        Assert.False(result);
        Assert.Equal(2, dictionary.Count);
        Assert.True(dictionary.TryGetValue("key1", out var v1));
        Assert.Equal("value1", v1);
        Assert.True(dictionary.TryGetValue("key2", out var v2));
        Assert.Equal("value2", v2);
    }

    // ============================================================
    // TryRemove — 不存在的 key/value
    // ============================================================

    [Fact]
    public void TryRemoveKey_NonExistentKey_ShouldReturnFalseAndNotModifyDictionary()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act
        var result = dictionary.TryRemoveKey("nonExistentKey", out var value);

        // Assert
        Assert.False(result);
        Assert.Null(value); // 未找到时 out 参数为默认值
        Assert.Single(dictionary);
        Assert.True(dictionary.ContainsValue("value1"));
    }

    [Fact]
    public void TryRemoveValue_NonExistentValue_ShouldReturnFalseAndNotModifyDictionary()
    {
        // Arrange
        var dictionary = new BidirectionalDictionary<string, string>();
        dictionary.TryAdd("key1", "value1");

        // Act
        var result = dictionary.TryRemoveValue("nonExistentValue", out var key);

        // Assert
        Assert.False(result);
        Assert.Null(key); // 未找到时 out 参数为默认值
        Assert.Single(dictionary);
        Assert.True(dictionary.ContainsKey("key1"));
    }
}
