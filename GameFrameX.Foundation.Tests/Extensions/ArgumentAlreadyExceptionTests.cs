using System;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ArgumentAlreadyException 异常类单元测试
/// </summary>
public class ArgumentAlreadyExceptionTests
{
    [Fact]
    public void Constructor_ValidMessage_ShouldCreateException()
    {
        // Arrange
        var message = "Parameter already exists";

        // Act
        var exception = new ArgumentAlreadyException(message);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.IsType<ArgumentAlreadyException>(exception);
    }

    [Fact]
    public void Constructor_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        string nullMessage = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new ArgumentAlreadyException(nullMessage));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Constructor_EmptyMessage_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyMessage = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ArgumentAlreadyException(emptyMessage));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhitespaceMessage_ShouldThrowArgumentException()
    {
        // Arrange
        var whitespaceMessage = "   ";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new ArgumentAlreadyException(whitespaceMessage));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Throw_ValidMessage_ShouldThrowArgumentAlreadyException()
    {
        // Arrange
        var message = "Parameter already exists";

        // Act & Assert
        var exception = Assert.Throws<ArgumentAlreadyException>(() => ArgumentAlreadyException.Throw(message));
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Throw_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        string nullMessage = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => ArgumentAlreadyException.Throw(nullMessage));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Throw_EmptyMessage_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyMessage = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ArgumentAlreadyException.Throw(emptyMessage));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void Throw_WhitespaceMessage_ShouldThrowArgumentException()
    {
        // Arrange
        var whitespaceMessage = "   ";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ArgumentAlreadyException.Throw(whitespaceMessage));
        Assert.Equal("message", exception.ParamName);
    }

    [Fact]
    public void ArgumentAlreadyException_ShouldInheritFromException()
    {
        // Arrange
        var message = "Test message";
        var exception = new ArgumentAlreadyException(message);

        // Act & Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }

    [Fact]
    public void ArgumentAlreadyException_ShouldBeSealed()
    {
        // Arrange & Act
        var type = typeof(ArgumentAlreadyException);

        // Assert
        Assert.True(type.IsSealed);
    }
}