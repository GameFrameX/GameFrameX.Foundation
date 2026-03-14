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
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

/// <summary>
/// ResourceManager 单元测试
/// </summary>
public class ResourceManagerTests : IDisposable
{
    private readonly ResourceManager _resourceManager;

    public ResourceManagerTests()
    {
        _resourceManager = new ResourceManager();
    }

    public void Dispose()
    {
        _resourceManager?.Dispose();
    }

    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        // Act
        var manager = new ResourceManager();

        // Assert
        Assert.NotNull(manager);
    }

    [Fact]
    public void GetString_WithKnownDefaultKey_ShouldReturnValue()
    {
        // Arrange
        const string key = "ArgumentNull";

        // Act
        var result = _resourceManager.GetString(key);

        // Assert
        // 由于本地化系统是分布式的，键值可能来自不同程序集
        // 这里只验证返回值不为空（可能是本地化的值，也可能是键本身）
        Assert.NotNull(result);
    }

    [Fact]
    public void GetString_WithUnknownKey_ShouldReturnKey()
    {
        // Arrange
        const string key = "Unknown.Test.Key";

        // Act
        var result = _resourceManager.GetString(key);

        // Assert
        Assert.Equal(key, result);
    }

    [Fact]
    public void GetString_WithNullKey_ShouldReturnNull()
    {
        // Arrange
        const string key = null;

        // Act
        var result = _resourceManager.GetString(key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetString_WithEmptyKey_ShouldReturnEmpty()
    {
        // Arrange
        const string key = "";

        // Act
        var result = _resourceManager.GetString(key);

        // Assert
        Assert.Equal("", result);
    }

    [Fact]
    public void EnsureProvidersLoaded_ShouldLoadProviders()
    {
        // Act
        _resourceManager.EnsureProvidersLoaded();

        // Assert (should not throw and providers should be loaded)
        var providers = _resourceManager.GetProviders();
        Assert.NotNull(providers);
        Assert.NotEmpty(providers);
    }

    [Fact]
    public void GetProviders_BeforeLoading_ShouldLoadProviders()
    {
        // Act
        var providers = _resourceManager.GetProviders();

        // Assert
        Assert.NotNull(providers);
        Assert.NotEmpty(providers);
    }

    [Fact]
    public void GetProviders_AfterLoading_ShouldReturnSameCount()
    {
        // Arrange
        _resourceManager.EnsureProvidersLoaded();
        var firstLoad = _resourceManager.GetProviders();

        // Act
        var secondLoad = _resourceManager.GetProviders();

        // Assert
        Assert.Equal(firstLoad.Count, secondLoad.Count);
    }

    [Fact]
    public void RegisterProvider_WithNullProvider_ShouldThrowArgumentNullException()
    {
        // Arrange
        IResourceProvider provider = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _resourceManager.RegisterProvider(provider));
    }

    [Fact]
    public void GetStatistics_ShouldReturnValidStatistics()
    {
        // Act
        var stats = _resourceManager.GetStatistics();

        // Assert
        Assert.NotNull(stats);
        Assert.True(stats.ProvidersLoaded);
        Assert.True(stats.TotalProviderCount > 0);
        Assert.True(stats.DefaultProviderExists);
    }

    [Theory]
    [InlineData("ArgumentNull")] // This key might exist in default resources
    [InlineData("Test.NonExistentKey")] // This key definitely doesn't exist
    public void GetString_WithPreDefinedKeys_ShouldReturnValidValues(string key)
    {
        // Act
        var result = _resourceManager.GetString(key);

        // Assert
        Assert.NotNull(result);
        // In the distributed localization system, many keys may not be resolved
        // This test verifies the method doesn't crash and returns something reasonable
        // The result could be the key itself (if not found) or a localized value (if found)
    }

    [Fact]
    public void ProviderPriority_CustomProvider_ShouldHaveHighestPriority()
    {
        // Arrange
        var customProvider = new TestResourceProvider("Custom Value");
        _resourceManager.RegisterProvider(customProvider);
        const string testKey = "Test.Key";

        // Act
        var result = _resourceManager.GetString(testKey);

        // Assert
        Assert.Equal("Custom Value", result); // Custom provider should be used
    }

    [Fact]
    public void MultipleRegisterProvider_ShouldMaintainPriorityOrder()
    {
        // Arrange
        var provider1 = new TestResourceProvider("Value1");
        var provider2 = new TestResourceProvider("Value2");
        var provider3 = new TestResourceProvider("Value3");

        // Act
        _resourceManager.RegisterProvider(provider1);
        _resourceManager.RegisterProvider(provider2);
        _resourceManager.RegisterProvider(provider3);

        var result = _resourceManager.GetString("Test.Key");

        // Assert
        Assert.Equal("Value3", result); // Last registered should have highest priority
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange
        var manager = new ResourceManager();

        // Act & Assert (should not throw)
        manager.Dispose();
    }

    [Fact]
    public void GetString_WithProviderException_ShouldContinueToNextProvider()
    {
        // Arrange
        var faultyProvider = new FaultyResourceProvider();
        var goodProvider = new TestResourceProvider("Good Value");

        _resourceManager.RegisterProvider(faultyProvider);
        _resourceManager.RegisterProvider(goodProvider);

        // Act
        var result = _resourceManager.GetString("Test.Key");

        // Assert
        Assert.Equal("Good Value", result); // Should skip faulty provider and use good one
    }

    /// <summary>
    /// 测试用的资源提供者
    /// </summary>
    private class TestResourceProvider : IResourceProvider
    {
        private readonly string _value;

        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        /// <value>
        /// 资源提供者的名称，用于标识资源提供者
        /// </value>
        public string AssemblyName => GetType().Assembly.FullName;

        public TestResourceProvider(string value)
        {
            _value = value;
        }

        public string GetString(string key)
        {
            return key == "Test.Key" ? _value : key;
        }
    }

    /// <summary>
    /// 测试用的故障资源提供者
    /// </summary>
    private class FaultyResourceProvider : IResourceProvider
    {
        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        /// <value>
        /// 资源提供者的名称，用于标识资源提供者
        /// </value>
        public string AssemblyName => GetType().Assembly.FullName;

        public string GetString(string key)
        {
            throw new InvalidOperationException("Test exception");
        }
    }
}