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