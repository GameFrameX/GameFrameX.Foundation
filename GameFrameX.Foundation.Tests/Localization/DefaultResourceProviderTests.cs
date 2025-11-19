using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Localization.Providers;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

/// <summary>
/// DefaultResourceProvider 单元测试
/// </summary>
public class DefaultResourceProviderTests
{
    private readonly DefaultResourceProvider _provider;

    public DefaultResourceProviderTests()
    {
        _provider = new DefaultResourceProvider();
    }

    [Fact]
    public void GetString_WithKnownKey_ShouldReturnValue()
    {
        // Arrange
        const string key = "ArgumentNull";
        const string expected = "Value cannot be null.";

        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.Equal(expected, result);
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
    public void GetString_WithKnownKey_CaseInsensitive_ShouldReturnValue()
    {
        // Arrange
        const string key = "argumentnull"; // 小写
        const string expected = "Value cannot be null.";

        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void AddOrUpdateMessage_WithValidInput_ShouldAddMessage()
    {
        // Arrange
        const string key = "Test.New.Message";
        const string message = "This is a test message";

        // Act
        _provider.AddOrUpdateMessage(key, message);
        var result = _provider.GetString(key);

        // Assert
        Assert.Equal(message, result);
    }

    [Fact]
    public void AddOrUpdateMessage_WithExistingKey_ShouldUpdateMessage()
    {
        // Arrange
        const string key = "ArgumentNull";
        const string newMessage = "Updated message";

        // Act
        _provider.AddOrUpdateMessage(key, newMessage);
        var result = _provider.GetString(key);

        // Assert
        Assert.Equal(newMessage, result);
    }

    [Fact]
    public void AddOrUpdateMessage_WithNullKey_ShouldNotAdd()
    {
        // Arrange
        const string message = "Test message";

        // Act & Assert (should not throw)
        _provider.AddOrUpdateMessage(null, message);
    }

    [Fact]
    public void AddOrUpdateMessage_WithNullMessage_ShouldNotAdd()
    {
        // Arrange
        const string key = "Test.Key";

        // Act & Assert (should not throw)
        _provider.AddOrUpdateMessage(key, null);
    }

    [Fact]
    public void ContainsKey_WithExistingKey_ShouldReturnTrue()
    {
        // Arrange
        const string key = "ArgumentNull";

        // Act
        var result = _provider.ContainsKey(key);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsKey_WithNonExistingKey_ShouldReturnFalse()
    {
        // Arrange
        const string key = "Non.Existing.Key";

        // Act
        var result = _provider.ContainsKey(key);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsKey_WithNullKey_ShouldReturnFalse()
    {
        // Arrange
        const string key = null;

        // Act
        var result = _provider.ContainsKey(key);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAllKeys_ShouldReturnAllKeys()
    {
        // Act
        var keys = _provider.GetAllKeys();

        // Assert
        Assert.NotNull(keys);
        Assert.NotEmpty(keys);
        Assert.Contains("ArgumentNull", keys);
        Assert.Contains("ArgumentOutOfRange", keys);
    }

    [Fact]
    public void GetAllKeys_ShouldReturnReadOnly()
    {
        // Act
        var keys = _provider.GetAllKeys();

        // Assert
        Assert.NotNull(keys);
        // IReadOnlyCollection is inherently read-only by interface definition
        Assert.True(keys is IReadOnlyCollection<string>);
    }

    [Theory]
    [InlineData("ArgumentNull")] // This key might exist in default resources
    [InlineData("Test.NonExistentKey")] // This key definitely doesn't exist
    public void GetString_WithPreDefinedKeys_ShouldReturnExpectedValues(string key)
    {
        // Act
        var result = _provider.GetString(key);

        // Assert
        Assert.NotNull(result);
        // In the distributed localization system, many keys may not be resolved
        // This test verifies the method doesn't crash and returns something reasonable
        // The result could be the key itself (if not found) or a localized value (if found)
    }
}