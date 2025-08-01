# GameFrameX.Foundation.Http.Extension

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/license-Apache--2.0-green.svg)](LICENSE)

GameFrameX.Foundation.Http.Extension 是一个为HttpClient提供扩展方法的基础设施库，提供了统一的GET和POST请求接口，简化HTTP请求操作，支持多种数据格式和响应类型。

## 🎯 核心特性

- **GET请求扩展** - 提供多种GET请求方法，支持字符串、字节数组、流等响应格式
- **POST请求扩展** - 支持JSON、表单、文件等多种POST请求方式
- **类型安全** - 泛型支持，确保数据类型安全
- **灵活配置** - 支持自定义请求头、超时时间、序列化选项
- **多种响应格式** - 支持字符串、字节数组、流等多种响应格式
- **文件上传** - 支持单文件和Multipart表单文件上传
- **异步支持** - 全面支持异步操作和取消令牌
- **错误处理** - 完善的参数验证和异常处理

## 📦 安装

```bash
# 通过 NuGet 包管理器安装
Install-Package GameFrameX.Foundation.Http.Extension

# 或通过 .NET CLI 安装
dotnet add package GameFrameX.Foundation.Http.Extension
```

## 🚀 快速开始

### 基本使用

```csharp
using GameFrameX.Foundation.Http.Extension;

// 创建HttpClient实例
using var httpClient = new HttpClient();

// GET请求获取字符串
string response = await httpClient.GetToStringAsync<string>("https://api.example.com/users");
Console.WriteLine(response);

// POST JSON数据
var userData = new { Name = "张三", Age = 25 };
string postResponse = await httpClient.PostJsonToStringAsync("https://api.example.com/users", userData);
Console.WriteLine(postResponse);
```

### 带请求头和超时的请求

```csharp
// 自定义请求头
var headers = new Dictionary<string, string>
{
    ["Authorization"] = "Bearer your-token",
    ["User-Agent"] = "MyApp/1.0"
};

// GET请求带请求头和超时
string response = await httpClient.GetToStringAsync<string>(
    "https://api.example.com/protected", 
    headers, 
    timeout: 30);

// POST请求带请求头和超时
string postResponse = await httpClient.PostJsonToStringAsync(
    "https://api.example.com/data", 
    userData, 
    headers, 
    timeout: 30);
```

## 📋 详细使用指南

### 1. GET请求扩展方法

#### 获取字符串响应

```csharp
// 基本GET请求
string response1 = await httpClient.GetToStringAsync<string>("https://api.example.com/data");

// 带请求头和超时的GET请求
var headers = new Dictionary<string, string>
{
    ["Accept"] = "application/json",
    ["Authorization"] = "Bearer token"
};
string response2 = await httpClient.GetToStringAsync<string>(
    "https://api.example.com/data", 
    headers, 
    timeout: 30);
```

#### 获取字节数组响应

```csharp
// 基本GET请求获取字节数组
byte[] data1 = await httpClient.GetToByteArrayAsync<byte[]>("https://api.example.com/file");

// 带请求头的GET请求获取字节数组
byte[] data2 = await httpClient.GetToByteArrayAsync<byte[]>(
    "https://api.example.com/file", 
    headers, 
    timeout: 60);
```

#### 获取流响应

```csharp
// 基本GET请求获取流
using Stream stream1 = await httpClient.GetToStreamAsync<Stream>("https://api.example.com/download");

// 带请求头的GET请求获取流
using Stream stream2 = await httpClient.GetToStreamAsync<Stream>(
    "https://api.example.com/download", 
    headers, 
    timeout: 120);
```

### 2. POST请求扩展方法

#### JSON数据POST请求

```csharp
// 定义数据模型
public class UserInfo
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}

var user = new UserInfo 
{ 
    Name = "张三", 
    Age = 25, 
    Email = "zhangsan@example.com" 
};

// 基本JSON POST请求
string response1 = await httpClient.PostJsonToStringAsync("https://api.example.com/users", user);

// 带自定义序列化选项的POST请求
var jsonOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};
string response2 = await httpClient.PostJsonToStringAsync(
    "https://api.example.com/users", 
    user, 
    jsonOptions);

// 带请求头和超时的POST请求
string response3 = await httpClient.PostJsonToStringAsync(
    "https://api.example.com/users", 
    user, 
    headers, 
    timeout: 30);

// 完整配置的POST请求
string response4 = await httpClient.PostJsonToStringAsync(
    "https://api.example.com/users", 
    user, 
    headers, 
    jsonOptions, 
    timeout: 30);
```

#### 获取不同格式的POST响应

```csharp
// 获取字节数组响应
byte[] responseBytes = await httpClient.PostJsonToByteArrayAsync(
    "https://api.example.com/data", 
    user);

// 获取流响应
using Stream responseStream = await httpClient.PostJsonToStreamAsync(
    "https://api.example.com/data", 
    user);
```

#### 表单数据POST请求

```csharp
// 表单数据
var formData = new Dictionary<string, string>
{
    ["username"] = "zhangsan",
    ["password"] = "123456",
    ["email"] = "zhangsan@example.com"
};

// 基本表单POST请求
string response1 = await httpClient.PostFormToStringAsync(
    "https://api.example.com/login", 
    formData);

// 带请求头和超时的表单POST请求
string response2 = await httpClient.PostFormToStringAsync(
    "https://api.example.com/login", 
    formData, 
    headers, 
    timeout: 30);
```

### 3. 文件上传

#### 单文件上传

```csharp
// 基本文件上传
string response1 = await httpClient.PostFileToStringAsync(
    "https://api.example.com/upload", 
    @"C:\temp\document.pdf");

// 带请求头和超时的文件上传
string response2 = await httpClient.PostFileToStringAsync(
    "https://api.example.com/upload", 
    @"C:\temp\document.pdf", 
    headers, 
    timeout: 300);
```

#### Multipart表单文件上传

```csharp
// 额外的表单数据
var additionalData = new Dictionary<string, string>
{
    ["description"] = "用户头像",
    ["category"] = "avatar"
};

// Multipart文件上传
string response = await httpClient.PostMultipartFileToStringAsync(
    "https://api.example.com/upload", 
    "file",                    // 文件字段名
    @"C:\temp\avatar.jpg",     // 文件路径
    additionalData);           // 额外表单数据
```

## 🎨 高级用法

### 1. 自定义JSON序列化配置

```csharp
public static class CustomJsonOptions
{
    public static readonly JsonSerializerOptions CamelCase = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    public static readonly JsonSerializerOptions SnakeCase = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = false
    };
}

// 使用自定义序列化选项
string response = await httpClient.PostJsonToStringAsync(
    "https://api.example.com/data", 
    userData, 
    CustomJsonOptions.CamelCase);
```

### 2. 批量请求处理

```csharp
public class BatchRequestProcessor
{
    private readonly HttpClient httpClient;
    
    public BatchRequestProcessor(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<List<string>> ProcessBatchGetRequests(List<string> urls)
    {
        var tasks = urls.Select(url => 
            httpClient.GetToStringAsync<string>(url)).ToList();
        
        return (await Task.WhenAll(tasks)).ToList();
    }
    
    public async Task<List<string>> ProcessBatchPostRequests<T>(
        string baseUrl, 
        List<T> dataList)
    {
        var tasks = dataList.Select(data => 
            httpClient.PostJsonToStringAsync(baseUrl, data)).ToList();
        
        return (await Task.WhenAll(tasks)).ToList();
    }
}
```

### 3. 重试机制

```csharp
public static class HttpClientRetryExtensions
{
    public static async Task<string> GetWithRetryAsync<T>(
        this HttpClient httpClient, 
        string url, 
        int maxRetries = 3, 
        TimeSpan delay = default)
    {
        if (delay == default) delay = TimeSpan.FromSeconds(1);
        
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                return await httpClient.GetToStringAsync<T>(url);
            }
            catch (HttpRequestException) when (i < maxRetries - 1)
            {
                await Task.Delay(delay);
                delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2); // 指数退避
            }
        }
        
        throw new InvalidOperationException($"请求失败，已重试 {maxRetries} 次");
    }
}
```

### 4. 响应缓存

```csharp
public class CachedHttpClient
{
    private readonly HttpClient httpClient;
    private readonly MemoryCache cache;
    
    public CachedHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 100
        });
    }
    
    public async Task<string> GetWithCacheAsync<T>(
        string url, 
        TimeSpan? expiration = null)
    {
        if (cache.TryGetValue(url, out string cachedResponse))
        {
            return cachedResponse;
        }
        
        var response = await httpClient.GetToStringAsync<T>(url);
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5),
            Size = 1
        };
        
        cache.Set(url, response, cacheOptions);
        return response;
    }
}
```

## 💡 最佳实践

### 1. HttpClient生命周期管理

```csharp
// 推荐：使用IHttpClientFactory
public class ApiService
{
    private readonly HttpClient httpClient;
    
    public ApiService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("ApiClient");
    }
    
    public async Task<string> GetDataAsync()
    {
        return await httpClient.GetToStringAsync<string>("https://api.example.com/data");
    }
}

// 在Startup.cs或Program.cs中注册
services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://api.example.com/");
    client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
});
```

### 2. 统一错误处理

```csharp
public class ApiClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<ApiClient> logger;
    
    public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }
    
    public async Task<T> GetAsync<T>(string url) where T : class
    {
        try
        {
            var response = await httpClient.GetToStringAsync<T>(url);
            return JsonSerializer.Deserialize<T>(response);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP请求失败: {Url}", url);
            throw new ApiException($"请求失败: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex)
        {
            logger.LogError(ex, "请求超时: {Url}", url);
            throw new ApiException("请求超时", ex);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "JSON反序列化失败: {Url}", url);
            throw new ApiException("数据格式错误", ex);
        }
    }
}

public class ApiException : Exception
{
    public ApiException(string message) : base(message) { }
    public ApiException(string message, Exception innerException) : base(message, innerException) { }
}
```

### 3. 配置管理

```csharp
public class ApiConfiguration
{
    public string BaseUrl { get; set; }
    public int TimeoutSeconds { get; set; } = 30;
    public Dictionary<string, string> DefaultHeaders { get; set; } = new();
}

public class ConfiguredApiClient
{
    private readonly HttpClient httpClient;
    private readonly ApiConfiguration config;
    
    public ConfiguredApiClient(HttpClient httpClient, IOptions<ApiConfiguration> config)
    {
        this.httpClient = httpClient;
        this.config = config.Value;
        
        // 应用配置
        httpClient.BaseAddress = new Uri(this.config.BaseUrl);
        httpClient.Timeout = TimeSpan.FromSeconds(this.config.TimeoutSeconds);
        
        foreach (var header in this.config.DefaultHeaders)
        {
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }
    
    public async Task<string> GetAsync(string endpoint)
    {
        return await httpClient.GetToStringAsync<string>(endpoint);
    }
}
```

### 4. 请求/响应日志记录

```csharp
public class LoggingHttpClient
{
    private readonly HttpClient httpClient;
    private readonly ILogger<LoggingHttpClient> logger;
    
    public LoggingHttpClient(HttpClient httpClient, ILogger<LoggingHttpClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }
    
    public async Task<string> GetWithLoggingAsync<T>(string url)
    {
        var stopwatch = Stopwatch.StartNew();
        
        logger.LogInformation("开始GET请求: {Url}", url);
        
        try
        {
            var response = await httpClient.GetToStringAsync<T>(url);
            
            stopwatch.Stop();
            logger.LogInformation("GET请求成功: {Url}, 耗时: {ElapsedMs}ms, 响应长度: {Length}", 
                url, stopwatch.ElapsedMilliseconds, response.Length);
            
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex, "GET请求失败: {Url}, 耗时: {ElapsedMs}ms", 
                url, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
```

## 🔧 扩展功能

### 1. 自定义响应处理器

```csharp
public static class HttpClientResponseExtensions
{
    public static async Task<ApiResponse<T>> GetApiResponseAsync<T>(
        this HttpClient httpClient, 
        string url) where T : class
    {
        try
        {
            var response = await httpClient.GetToStringAsync<T>(url);
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(response);
            return apiResponse;
        }
        catch (Exception ex)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            };
        }
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
```

### 2. 请求拦截器

```csharp
public class InterceptorHttpClient
{
    private readonly HttpClient httpClient;
    private readonly List<Func<HttpRequestMessage, Task>> requestInterceptors;
    private readonly List<Func<HttpResponseMessage, Task>> responseInterceptors;
    
    public InterceptorHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.requestInterceptors = new List<Func<HttpRequestMessage, Task>>();
        this.responseInterceptors = new List<Func<HttpResponseMessage, Task>>();
    }
    
    public void AddRequestInterceptor(Func<HttpRequestMessage, Task> interceptor)
    {
        requestInterceptors.Add(interceptor);
    }
    
    public void AddResponseInterceptor(Func<HttpResponseMessage, Task> interceptor)
    {
        responseInterceptors.Add(interceptor);
    }
    
    public async Task<string> GetWithInterceptorsAsync(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        
        // 执行请求拦截器
        foreach (var interceptor in requestInterceptors)
        {
            await interceptor(request);
        }
        
        var response = await httpClient.SendAsync(request);
        
        // 执行响应拦截器
        foreach (var interceptor in responseInterceptors)
        {
            await interceptor(response);
        }
        
        return await response.Content.ReadAsStringAsync();
    }
}
```

### 3. 并发限制

```csharp
public class ThrottledHttpClient
{
    private readonly HttpClient httpClient;
    private readonly SemaphoreSlim semaphore;
    
    public ThrottledHttpClient(HttpClient httpClient, int maxConcurrency = 10)
    {
        this.httpClient = httpClient;
        this.semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
    }
    
    public async Task<string> GetWithThrottleAsync<T>(string url)
    {
        await semaphore.WaitAsync();
        try
        {
            return await httpClient.GetToStringAsync<T>(url);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
```

## 🔍 故障排除

### 常见问题

#### 1. 超时问题
**问题**: 请求经常超时
**解决方案**: 适当增加超时时间，使用重试机制

```csharp
// 增加超时时间
string response = await httpClient.GetToStringAsync<string>(url, headers, timeout: 120);

// 使用重试机制
string response = await httpClient.GetWithRetryAsync<string>(url, maxRetries: 3);
```

#### 2. 内存泄漏问题
**问题**: HttpClient使用不当导致内存泄漏
**解决方案**: 使用IHttpClientFactory或正确管理HttpClient生命周期

```csharp
// 推荐：使用IHttpClientFactory
services.AddHttpClient<ApiService>();

// 或者：正确使用using语句
using var httpClient = new HttpClient();
```

#### 3. 序列化问题
**问题**: JSON序列化/反序列化失败
**解决方案**: 检查数据模型和序列化选项

```csharp
// 使用自定义序列化选项
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

string response = await httpClient.PostJsonToStringAsync(url, data, options);
```

### 调试技巧

```csharp
// 启用详细日志
public static class HttpClientDebugExtensions
{
    public static async Task<string> GetWithDebugAsync<T>(
        this HttpClient httpClient, 
        string url, 
        ILogger logger = null)
    {
        logger?.LogDebug("发送GET请求到: {Url}", url);
        
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var response = await httpClient.GetToStringAsync<T>(url);
            stopwatch.Stop();
            
            logger?.LogDebug("GET请求成功: {Url}, 耗时: {ElapsedMs}ms, 响应长度: {Length}", 
                url, stopwatch.ElapsedMilliseconds, response.Length);
            
            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger?.LogError(ex, "GET请求失败: {Url}, 耗时: {ElapsedMs}ms", 
                url, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}
```

## 📄 许可证

本项目采用 Apache 许可证（版本 2.0）进行分发和使用。详细信息请参阅项目根目录中的 LICENSE 文件。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目。

## 📞 支持

如果您在使用过程中遇到问题，请通过以下方式获取帮助：

- 提交 [GitHub Issue](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- 查看项目文档: https://gameframex.doc.alianblank.com
- 参考单元测试了解更多用法

---

**GameFrameX.Foundation.Http.Extension** - 让HTTP请求更简单、更统一！