using Xunit;
using GameFrameX.Foundation.Http.Extension;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace GameFrameX.Foundation.Tests.Http.Extension;

public class HttpExtensionTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    
    public HttpExtensionTests()
    {
        _httpClient = new HttpClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    [Fact]
    public void HttpClientGetExtension_ShouldHaveGetToStringAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.Name == "GetToStringAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Equal(2, methods.Length); // Should have 2 overloads
    }
    
    [Fact]
    public void HttpClientGetExtension_ShouldHaveGetToByteArrayAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.Name == "GetToByteArrayAsync")
            .ToArray();
        
        // Assert
         Assert.NotEmpty(methods);
         Assert.Equal(2, methods.Length); // Should have 2 overloads
    }
    
    [Fact]
    public void HttpClientGetExtension_ShouldHaveGetToStreamAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.Name == "GetToStreamAsync")
            .ToArray();
        
        // Assert
         Assert.NotEmpty(methods);
         Assert.Equal(2, methods.Length); // Should have 2 overloads
    }
    
    [Fact]
    public void HttpClientGetExtension_ShouldBeStaticClass()
    {
        // Arrange & Act
        var type = typeof(HttpClientGetExtension);
        
        // Assert
        Assert.True(type.IsAbstract && type.IsSealed); // Static class
    }
    
    [Fact]
    public void HttpClientGetExtension_ShouldBeInCorrectNamespace()
    {
        // Arrange & Act
        var type = typeof(HttpClientGetExtension);
        
        // Assert
        Assert.Equal("GameFrameX.Foundation.Http.Extension", type.Namespace);
    }
    
    [Fact]
    public void HttpClientGetExtension_MethodSignatures_ShouldBeCorrect()
    {
        // Arrange & Act
        var getToStringMethods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.Name == "GetToStringAsync")
            .ToArray();
        
        // Assert
        Assert.All(getToStringMethods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<string>), method.ReturnType);
        });
    }
    
    [Fact]
    public void HttpClientGetExtension_GetToByteArrayAsync_ShouldReturnCorrectType()
    {
        // Arrange & Act
        var methods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.Name == "GetToByteArrayAsync")
            .ToArray();
        
        // Assert
        Assert.All(methods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<byte[]>), method.ReturnType);
        });
    }
    
    [Fact]
    public void HttpClientGetExtension_GetToStreamAsync_ShouldReturnCorrectType()
    {
        // Arrange & Act
        var methods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.Name == "GetToStreamAsync")
            .ToArray();
        
        // Assert
        Assert.All(methods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<System.IO.Stream>), method.ReturnType);
        });
    }
    
    [Fact]
    public void HttpClientGetExtension_AllMethods_ShouldBeExtensionMethods()
    {
        // Arrange & Act
        var allMethods = typeof(HttpClientGetExtension).GetMethods()
            .Where(m => m.IsStatic && m.IsPublic && !m.IsSpecialName)
            .ToArray();
        
        // Assert
        Assert.All(allMethods, method =>
        {
            var parameters = method.GetParameters();
            Assert.True(parameters.Length > 0);
            Assert.Equal(typeof(HttpClient), parameters[0].ParameterType);
        });
    }
    
    [Fact]
    public void HttpClientGetExtension_Performance_ShouldLoadQuickly()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        // Act
        var type = typeof(HttpClientGetExtension);
        stopwatch.Stop();
        
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 100, "Type loading should be fast");
        Assert.NotNull(type);
    }

    // HttpClientPostExtension Tests
    
    [Fact]
    public void HttpClientPostExtension_ShouldHavePostJsonToStringAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToStringAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Equal(4, methods.Length); // Should have 4 overloads
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldHavePostJsonToByteArrayAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToByteArrayAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Equal(4, methods.Length); // Should have 4 overloads
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldHavePostJsonToStreamAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToStreamAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Equal(4, methods.Length); // Should have 4 overloads
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldHavePostFormToStringAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostFormToStringAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Equal(2, methods.Length); // Should have 2 overloads
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldHavePostFileToStringAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostFileToStringAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Equal(2, methods.Length); // Should have 2 overloads
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldHavePostMultipartFileToStringAsyncMethod()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostMultipartFileToStringAsync")
            .ToArray();
        
        // Assert
        Assert.NotEmpty(methods);
        Assert.Single(methods); // Should have 1 overload
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldBeStaticClass()
    {
        // Arrange & Act
        var type = typeof(HttpClientPostExtension);
        
        // Assert
        Assert.True(type.IsAbstract && type.IsSealed); // Static class
    }
    
    [Fact]
    public void HttpClientPostExtension_ShouldBeInCorrectNamespace()
    {
        // Arrange & Act
        var type = typeof(HttpClientPostExtension);
        
        // Assert
        Assert.Equal("GameFrameX.Foundation.Http.Extension", type.Namespace);
    }
    
    [Fact]
    public void HttpClientPostExtension_PostJsonToStringAsync_ShouldReturnCorrectType()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToStringAsync")
            .ToArray();
        
        // Assert
        Assert.All(methods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<string>), method.ReturnType);
            Assert.True(method.IsGenericMethodDefinition); // Generic method
        });
    }
    
    [Fact]
    public void HttpClientPostExtension_PostJsonToByteArrayAsync_ShouldReturnCorrectType()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToByteArrayAsync")
            .ToArray();
        
        // Assert
        Assert.All(methods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<byte[]>), method.ReturnType);
            Assert.True(method.IsGenericMethodDefinition); // Generic method
        });
    }
    
    [Fact]
    public void HttpClientPostExtension_PostJsonToStreamAsync_ShouldReturnCorrectType()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToStreamAsync")
            .ToArray();
        
        // Assert
        Assert.All(methods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<Stream>), method.ReturnType);
            Assert.True(method.IsGenericMethodDefinition); // Generic method
        });
    }
    
    [Fact]
    public void HttpClientPostExtension_PostFormToStringAsync_ShouldReturnCorrectType()
    {
        // Arrange & Act
        var methods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostFormToStringAsync")
            .ToArray();
        
        // Assert
        Assert.All(methods, method =>
        {
            Assert.True(method.IsStatic);
            Assert.True(method.IsPublic);
            Assert.Equal(typeof(Task<string>), method.ReturnType);
            Assert.False(method.IsGenericMethodDefinition); // Non-generic method
        });
    }
    
    [Fact]
    public void HttpClientPostExtension_AllMethods_ShouldBeExtensionMethods()
    {
        // Arrange & Act
        var allMethods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.IsStatic && m.IsPublic && !m.IsSpecialName)
            .ToArray();
        
        // Assert
        Assert.All(allMethods, method =>
        {
            var parameters = method.GetParameters();
            Assert.True(parameters.Length > 0);
            Assert.Equal(typeof(HttpClient), parameters[0].ParameterType);
        });
    }
    
    [Fact]
    public void HttpClientPostExtension_JsonMethods_ShouldHaveGenericTypeParameter()
    {
        // Arrange & Act
        var jsonMethods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name.Contains("PostJson"))
            .ToArray();
        
        // Assert
        Assert.All(jsonMethods, method =>
        {
            Assert.True(method.IsGenericMethodDefinition);
            Assert.Single(method.GetGenericArguments()); // Should have one generic type parameter
        });
    }
    
    [Fact]
    public void HttpClientPostExtension_Performance_ShouldLoadQuickly()
    {
        // Arrange
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        // Act
        var type = typeof(HttpClientPostExtension);
        stopwatch.Stop();
        
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 100, "Type loading should be fast");
        Assert.NotNull(type);
    }
    
    [Fact]
    public void HttpClientPostExtension_MethodParameterValidation_ShouldHaveCorrectParameters()
    {
        // Arrange & Act
        var postJsonToStringMethods = typeof(HttpClientPostExtension).GetMethods()
            .Where(m => m.Name == "PostJsonToStringAsync")
            .ToArray();
        
        // Assert
        Assert.All(postJsonToStringMethods, method =>
        {
            var parameters = method.GetParameters();
            Assert.True(parameters.Length >= 3); // At least httpClient, url, data
            Assert.Equal(typeof(HttpClient), parameters[0].ParameterType);
            Assert.Equal(typeof(string), parameters[1].ParameterType);
        });
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}