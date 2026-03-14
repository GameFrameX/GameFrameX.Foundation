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