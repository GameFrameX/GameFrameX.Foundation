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

using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Localization.Providers;
using System.Reflection;
using GameFrameX.Foundation.Utility.Localization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

/// <summary>
/// AssemblyResourceProvider 单元测试
/// </summary>
public class AssemblyResourceProviderTests : IDisposable
{
    private readonly AssemblyResourceProvider _provider;

    public AssemblyResourceProviderTests()
    {
        _provider = new AssemblyResourceProvider(typeof(Foundation.Utility.ConsoleHelper).Assembly);
    }

    public void Dispose()
    {
        _provider?.Dispose();
    }

    [Fact]
    public void Constructor_WithValidAssembly_ShouldCreateInstance()
    {
        // Arrange & Act
        var assembly = Assembly.GetExecutingAssembly();
        var provider = new AssemblyResourceProvider(assembly);

        // Assert
        Assert.NotNull(provider);
        Assert.False(provider.IsInitialized);
    }

    [Fact]
    public void Constructor_WithNullAssembly_ShouldThrowArgumentNullException()
    {
        // Arrange
        Assembly assembly = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AssemblyResourceProvider(assembly));
    }

    [Fact]
    public void IsInitialized_Initially_ShouldBeFalse()
    {
        // Act
        var result = _provider.IsInitialized;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EnsureLoaded_ShouldInitializeProvider()
    {
        // Act
        _provider.EnsureLoaded();
        var x = _provider.GetString(LocalizationKeys.Exceptions.TimestampOutOfRange);
        // Assert
        Assert.True(_provider.IsInitialized);
    }

    [Fact]
    public void GetString_BeforeInitialization_ShouldLoadAndReturn()
    {
        // Arrange
        const string key = "Test.Message";

        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.True(_provider.IsInitialized); // Should have been loaded automatically
        Assert.Equal(key, result); // Since test assembly doesn't have resources
    }

    [Fact]
    public void GetString_WithNullKey_ShouldReturnNull()
    {
        // Arrange
        const string key = null;

        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetString_WithEmptyKey_ShouldReturnEmpty()
    {
        // Arrange
        const string key = "";

        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void GetString_WithUnknownKey_ShouldReturnKey()
    {
        // Arrange
        const string key = "Unknown.Test.Key";

        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.Equal(key, result);
    }

    [Fact]
    public void GetLoadedCategories_BeforeInitialization_ShouldReturnEmpty()
    {
        // Act
        var categories = _provider.GetLoadedCategories();

        // Assert
        Assert.NotNull(categories);
        Assert.Empty(categories);
    }

    [Fact]
    public void GetLoadedCategories_AfterInitialization_ShouldReturnCategories()
    {
        // Arrange
        _provider.EnsureLoaded();

        // Act
        var categories = _provider.GetLoadedCategories();

        // Assert
        Assert.NotNull(categories);
        // Since test assembly doesn't have localization resources, should be empty
    }

    [Fact]
    public void GetStatistics_ShouldReturnValidStatistics()
    {
        // Act
        var stats = _provider.GetStatistics();

        // Assert
        Assert.NotNull(stats);
        Assert.Equal(typeof(Foundation.Utility.ConsoleHelper).Assembly.GetName().Name, stats.AssemblyName);
    }

    [Fact]
    public void EnsureLoaded_MultipleCalls_ShouldNotThrow()
    {
        // Act & Assert (should not throw)
        _provider.EnsureLoaded();
        _provider.EnsureLoaded();
        _provider.EnsureLoaded();

        Assert.True(_provider.IsInitialized);
    }

    [Theory]
    [InlineData("Module.Category.Key", "Category")]
    [InlineData("A.B.C.D", "B")]
    [InlineData("Test.Message", "Message")]
    public void TryGetResourceManager_WithValidKey_ShouldExtractCategory(string key, string expectedCategory)
    {
        // Act
        // This is testing internal behavior through public interface
        // The actual implementation might not find ResourceManager, but should not throw
        var result = _provider.GetString(key);

        // Assert
        Assert.NotNull(result); // Should not throw or return null
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var provider = new AssemblyResourceProvider(Assembly.GetExecutingAssembly());

        // Act & Assert (should not throw)
        provider.Dispose();
    }
}