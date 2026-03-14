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