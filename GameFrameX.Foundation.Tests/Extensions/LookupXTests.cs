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