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
        const string expected = "Value cannot be null.";

        // Act
        var result = _resourceManager.GetString(key);

        // Assert
        Assert.Equal(expected, result);
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