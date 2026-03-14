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
        Assert.Throws<ArgumentNullException>(() => hashSet.AddRangeValues(enumerable));
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
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAddValue("key", k => 1));
    }

    [Fact]
    public void GetOrAdd_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAddValue(null, k => 1));
    }

    [Fact]
    public void GetOrAdd_NullValueGetter_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, int>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAddValue("key", (Func<string, int>)null));
    }

    [Fact]
    public void GetOrAdd_ExistingKey_ShouldReturnExistingValue()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { ["key1"] = 5 };

        // Act
        var result = dictionary.GetOrAddValue("key1", k => 10);

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
        var result = dictionary.GetOrAddValue("key1", k => 10);

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
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAddValue("key"));
    }

    [Fact]
    public void GetOrAdd_WithNew_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dictionary.GetOrAddValue(null));
    }

    [Fact]
    public void GetOrAdd_WithNew_NewKey_ShouldCreateNewInstance()
    {
        // Arrange
        var dictionary = new Dictionary<string, List<int>>();

        // Act
        var result = dictionary.GetOrAddValue("key1");

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

    #region RandomElement Tests

    [Fact]
    public void RandomElement_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<string> list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.RandomElement());
    }

    [Fact]
    public void RandomElement_EmptyList_ShouldThrowArgumentException()
    {
        // Arrange
        var list = new List<string>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => list.RandomElement());
    }

    [Fact]
    public void RandomElement_ValidList_ShouldReturnItem()
    {
        // Arrange
        var list = new List<string> { "item1", "item2", "item3" };

        // Act
        var result = list.RandomElement();

        // Assert
        Assert.Contains(result, list);
    }

    #endregion

    #region Shuffle Tests

    [Fact]
    public void Shuffle_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<string> list = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => list.Shuffle());
    }

    [Fact]
    public void Shuffle_ValidList_ShouldNotThrow()
    {
        // Arrange
        var list = new List<string> { "item1", "item2", "item3" };
        var originalItems = new List<string>(list);

        // Act
        list.Shuffle();

        // Assert
        Assert.Equal(originalItems.Count, list.Count);
        foreach (var item in originalItems)
        {
            Assert.Contains(item, list);
        }
    }

    [Fact]
    public void Shuffle_EmptyList_ShouldNotThrow()
    {
        // Arrange
        var list = new List<string>();

        // Act & Assert
        list.Shuffle(); // Should not throw
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