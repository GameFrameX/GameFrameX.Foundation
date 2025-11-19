# GameFrameX.Foundation.Localization 使用示例

本文档提供了 GameFrameX.Foundation.Localization 框架的实用示例和最佳实践。

## 基础使用示例

### 简单字符串本地化

```csharp
using GameFrameX.Foundation.Localization.Core;

public class BasicExamples
{
    public void ShowBasicUsage()
    {
        // 获取简单的本地化字符串
        var successMessage = LocalizationService.GetString("Success");
        Console.WriteLine(successMessage); // 输出: "Success" 或 "成功"

        // 获取异常消息
        var argumentNullMessage = LocalizationService.GetString("ArgumentNull");
        Console.WriteLine(argumentNullMessage); // 输出: "Value cannot be null."

        // 不存在的键返回键名本身
        var unknown = LocalizationService.GetString("Some.Unknown.Key");
        Console.WriteLine(unknown); // 输出: "Some.Unknown.Key"
    }
}
```

### 带参数的格式化消息

```csharp
public class ParameterizedExamples
{
    public void ShowParameterizedUsage()
    {
        // 格式化错误消息
        var invalidKeySize = LocalizationService.GetString(
            "Encryption.InvalidKeySize", 128, 256);
        Console.WriteLine(invalidKeySize);
        // 输出: "Invalid key size: 128. Expected length: 256."

        // 用户操作消息
        var userAction = LocalizationService.GetString(
            "User.ActionRequired", "张三", "密码修改");
        Console.WriteLine(userAction);
        // 输出: "用户 张三 需要执行操作: 密码修改"
    }
}
```

## 异常处理中的本地化

### 抛出本地化异常

```csharp
using GameFrameX.Foundation.Utility.Localization;

public class ExceptionExamples
{
    public void ValidateTimestamp(long timestamp)
    {
        if (timestamp <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timestamp),
                LocalizationService.GetString(LocalizationKeys.Exceptions.TimestampOutOfRange));
        }
    }

    public void ProcessUserInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException(
                LocalizationService.GetString("ArgumentNull", nameof(input)));
        }
    }
}
```

### 异常消息的本地化处理

```csharp
public class LocalizationErrorHandler
{
    public string GetLocalizedErrorMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentException argEx => LocalizationService.GetString(
                "ArgumentException", argEx.ParamName ?? "Unknown"),
            ArgumentOutOfRangeException aooreEx => LocalizationService.GetString(
                "ArgumentOutOfRange", aooreEx.ActualValue, aooreEx.ParamName ?? "Unknown"),
            InvalidOperationException ioEx => LocalizationService.GetString(
                "InvalidOperation", ioEx.Message),
            _ => LocalizationService.GetString("GenericError", exception.Message)
        };
    }
}
```

## 模块集成示例

### 为新模块添加本地化支持

#### 1. 创建本地化键定义

```csharp
// GameFrameX.Foundation.YourModule/Localization/Keys.cs
namespace GameFrameX.Foundation.YourModule.Localization;

public static class LocalizationKeys
{
    public static class Validation
    {
        public const string EmailRequired = "YourModule.Validation.EmailRequired";
        public const string EmailInvalid = "YourModule.Validation.EmailInvalid";
        public const string PhoneInvalid = "YourModule.Validation.PhoneInvalid";
    }

    public static class Operations
    {
        public const string UserCreated = "YourModule.Operations.UserCreated";
        public const string UserUpdated = "YourModule.Operations.UserUpdated";
        public const string UserDeleted = "YourModule.Operations.UserDeleted";
    }
}
```

#### 2. 在业务逻辑中使用

```csharp
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.YourModule.Localization;

public class UserService
{
    public void CreateUser(UserDto userDto)
    {
        // 验证邮箱
        if (string.IsNullOrEmpty(userDto.Email))
        {
            throw new ValidationException(
                LocalizationService.GetString(LocalizationKeys.Validation.EmailRequired));
        }

        if (!IsValidEmail(userDto.Email))
        {
            throw new ValidationException(
                LocalizationService.GetString(LocalizationKeys.Validation.EmailInvalid, userDto.Email));
        }

        // 创建用户逻辑...

        // 输出成功消息
        var successMessage = LocalizationService.GetString(
            LocalizationKeys.Operations.UserCreated, userDto.Username);
        Console.WriteLine(successMessage);
    }

    private bool IsValidEmail(string email)
    {
        // 邮箱验证逻辑
        return email.Contains("@");
    }
}
```

## 高级使用场景

### 自定义资源提供者

```csharp
public class ApiResourceProvider : IResourceProvider
{
    private readonly HttpClient _httpClient;
    private readonly Dictionary<string, string> _cache = new();
    private readonly string _apiUrl;

    public ApiResourceProvider(HttpClient httpClient, string apiUrl)
    {
        _httpClient = httpClient;
        _apiUrl = apiUrl;
    }

    public string GetString(string key)
    {
        // 先检查缓存
        if (_cache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        // 从API获取
        var culture = CultureInfo.CurrentCulture.Name;
        var url = $"{_apiUrl}/localization/{culture}/{key}";

        try
        {
            var response = _httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var localizedText = response.Content.ReadAsStringAsync().Result;
                _cache[key] = localizedText;
                return localizedText;
            }
        }
        catch
        {
            // API调用失败，返回null让系统使用下一个提供者
        }

        return null;
    }
}

// 注册API提供者
var httpClient = new HttpClient();
var apiProvider = new ApiResourceProvider(httpClient, "https://api.example.com");
LocalizationService.RegisterProvider(apiProvider);
```

### 动态语言切换

```csharp
public class LocalizationManager
{
    public void SwitchLanguage(string cultureCode)
    {
        // 保存用户语言偏好
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

        // 可选：预加载新语言的资源
        LocalizationService.EnsureLoaded();
    }

    public void ShowLocalizedMessage()
    {
        var welcomeMessage = LocalizationService.GetString("Welcome");
        var currentDate = DateTime.Now.ToString("D");

        Console.WriteLine($"{welcomeMessage} - {currentDate}");
    }
}
```

### 本地化监控和诊断

```csharp
public class LocalizationDiagnostics
{
    public void PrintLocalizationStatus()
    {
        var stats = LocalizationService.GetStatistics();
        var providers = LocalizationService.GetProviders();

        Console.WriteLine("=== 本地化系统状态 ===");
        Console.WriteLine($"提供者已加载: {stats.ProvidersLoaded}");
        Console.WriteLine($"总提供者数量: {stats.TotalProviderCount}");
        Console.WriteLine($"程序集提供者数量: {stats.AssemblyProviderCount}");
        Console.WriteLine($"默认提供者存在: {stats.DefaultProviderExists}");

        Console.WriteLine("\n=== 提供者列表 ===");
        for (int i = 0; i < providers.Count; i++)
        {
            var provider = providers[i];
            Console.WriteLine($"{i + 1}. {provider.GetType().Name}");
        }
    }

    public void TestCommonKeys()
    {
        var commonKeys = new[]
        {
            "Success",
            "Error",
            "ArgumentNull",
            "InvalidOperation",
            "Utility.Exceptions.TimestampOutOfRange"
        };

        Console.WriteLine("\n=== 常用键测试 ===");
        foreach (var key in commonKeys)
        {
            var value = LocalizationService.GetString(key);
            Console.WriteLine($"{key}: {value}");
        }
    }
}
```

## 测试本地化功能

### 单元测试示例

```csharp
using Xunit;
using GameFrameX.Foundation.Localization.Core;

public class LocalizationTests
{
    [Fact]
    public void GetString_WithKnownKey_ShouldReturnLocalizedValue()
    {
        // Act
        var result = LocalizationService.GetString("Success");

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual("Success", result); // 应该被本地化
    }

    [Fact]
    public void GetString_WithUnknownKey_ShouldReturnKey()
    {
        // Arrange
        const string unknownKey = "Test.Unknown.Key";

        // Act
        var result = LocalizationService.GetString(unknownKey);

        // Assert
        Assert.Equal(unknownKey, result);
    }

    [Fact]
    public void GetString_WithParameters_ShouldFormatCorrectly()
    {
        // Act
        var result = LocalizationService.GetString("ArgumentNull", "testParameter");

        // Assert
        Assert.NotNull(result);
        Assert.Contains("testParameter", result);
    }

    [Fact]
    public void CustomProvider_ShouldOverrideDefault()
    {
        // Arrange
        var customProvider = new TestResourceProvider("Custom Success Message");
        LocalizationService.RegisterProvider(customProvider);

        // Act
        var result = LocalizationService.GetString("Success");

        // Assert
        Assert.Equal("Custom Success Message", result);
    }

    private class TestResourceProvider : IResourceProvider
    {
        private readonly string _customMessage;

        public TestResourceProvider(string customMessage)
        {
            _customMessage = customMessage;
        }

        public string GetString(string key)
        {
            return key == "Success" ? _customMessage : null;
        }
    }
}
```

### 集成测试示例

```csharp
public class LocalizationIntegrationTests
{
    [Fact]
    public void MultipleModules_ShouldUseCorrectLocalization()
    {
        // 测试不同模块的本地化键
        var utilityMessage = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
        var encryptionMessage = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);

        Assert.NotNull(utilityMessage);
        Assert.NotNull(encryptionMessage);
        Assert.Contains("128", encryptionMessage);
    }

    [Fact]
    public void ConcurrentAccess_ShouldBeThreadSafe()
    {
        var tasks = new List<Task<string>>();
        const string key = "Success";

        // 并发调用
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() => LocalizationService.GetString(key)));
        }

        Task.WaitAll(tasks.ToArray());

        // 验证所有结果一致
        var results = tasks.Select(t => t.Result).Distinct().ToArray();
        Assert.Single(results);
    }
}
```

## 性能优化建议

### 1. 预加载策略

```csharp
public class ApplicationStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 应用启动时预加载本地化资源
        LocalizationService.EnsureLoaded();

        // 其他服务配置...
    }
}
```

### 2. 缓存常用消息

```csharp
public class MessageCache
{
    private static readonly Dictionary<string, string> _cache = new();

    public static string GetCachedMessage(string key)
    {
        if (!_cache.TryGetValue(key, out var cached))
        {
            cached = LocalizationService.GetString(key);
            _cache[key] = cached;
        }

        return cached;
    }
}
```

### 3. 批量获取优化

```csharp
public class BatchLocalization
{
    public Dictionary<string, string> GetMultipleMessages(IEnumerable<string> keys)
    {
        return keys.ToDictionary(key => key, LocalizationService.GetString);
    }
}
```

## 总结

通过这些示例，您可以看到 GameFrameX.Foundation.Localization 框架的强大功能和灵活性：

1. **简单易用**：基础用法只需要调用 `GetString` 方法
2. **参数支持**：支持格式化消息和参数替换
3. **异常处理**：完美集成到异常处理流程中
4. **模块化**：易于为新模块添加本地化支持
5. **可扩展**：支持自定义资源提供者
6. **测试友好**：提供完整的测试支持

根据您的具体需求，选择合适的实现方式，确保应用程序具备良好的本地化支持。