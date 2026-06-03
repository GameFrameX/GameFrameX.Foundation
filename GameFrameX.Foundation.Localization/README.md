# GameFrameX.Foundation.Localization

轻量级本地化框架，提供懒加载机制、零配置使用、线程安全的本地化解决方案。

## 安装

```bash
dotnet add package GameFrameX.Foundation.Localization
```

## 主要特性

- **零配置使用**：无需任何初始化配置，自动发现和加载本地化资源
- **高性能设计**：懒加载机制，首次使用时才加载资源，多层缓存优化访问性能
- **多语言支持**：内置中文（简体）和英文支持，可扩展更多语言，智能语言回退机制
- **高度可扩展**：支持自定义资源提供者，灵活的优先级管理，模块化组件设计

## 快速开始

### 基础用法

```csharp
using GameFrameX.Foundation.Localization.Core;

// 获取本地化字符串
var message = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");

// 带参数的格式化消息
var formattedMessage = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);

// 如果键不存在，返回键名本身
var unknown = LocalizationService.GetString("Some.Unknown.Key"); // 返回: "Some.Unknown.Key"
```

### 预加载资源（可选）

```csharp
// 应用启动时预加载所有本地化资源
LocalizationService.EnsureLoaded();

// 之后的使用将没有首次访问延迟
var message = LocalizationService.GetString("ArgumentNull");
```

## 架构设计

### 核心组件

```
GameFrameX.Foundation.Localization
├── Core/                    # 核心接口和管理类
│   ├── IResourceProvider.cs         # 资源提供者接口
│   ├── ResourceManager.cs           # 资源管理器
│   └── ResourceManagerStatistics.cs # 统计信息
└── Providers/               # 具体实现
    ├── DefaultResourceProvider.cs   # 默认资源提供者
    └── AssemblyResourceProvider.cs  # 程序集资源提供者
```

### 资源解析优先级

1. **自定义提供者**（最高优先级）
2. **程序集资源提供者**
3. **默认提供者**（兜底）

## 详细使用指南

### 在现有模块中集成本地化

#### 步骤1：定义本地化键

```csharp
// GameFrameX.Foundation.YourModule/Localization/Keys.cs
namespace GameFrameX.Foundation.YourModule.Localization;

public static class LocalizationKeys
{
    public static class Exceptions
    {
        public const string InvalidArgument = "YourModule.Exceptions.InvalidArgument";
        public const string OperationFailed = "YourModule.Exceptions.OperationFailed";
    }

    public static class Messages
    {
        public const string Success = "YourModule.Messages.Success";
        public const string Processing = "YourModule.Messages.Processing";
    }
}
```

#### 步骤2：创建资源文件

创建 `Localization/Messages/Resources.resx`（默认英文）和 `Resources.zh-CN.resx`（中文）：

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="YourModule.Exceptions.InvalidArgument" xml:space="preserve">
    <value>Invalid argument provided for {0}</value>
  </data>
  <data name="YourModule.Messages.Success" xml:space="preserve">
    <value>Operation completed successfully</value>
  </data>
</root>
```

#### 步骤3：更新项目文件

```xml
<PropertyGroup>
  <EnableDefaultEmbeddedResourceItems>false</EnableDefaultEmbeddedResourceItems>
</PropertyGroup>

<ItemGroup>
  <EmbeddedResource Include="Localization\Messages\*.resx" />
</ItemGroup>
```

#### 步骤4：在代码中使用

```csharp
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.YourModule.Localization;

public class YourService
{
    public void ProcessData(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException(
                LocalizationService.GetString(LocalizationKeys.Exceptions.InvalidArgument, nameof(input)));
        }

        var successMessage = LocalizationService.GetString(LocalizationKeys.Messages.Success);
        Console.WriteLine(successMessage);
    }
}
```

### 自定义资源提供者

```csharp
public class DatabaseResourceProvider : IResourceProvider
{
    private readonly IDbConnection _connection;

    public DatabaseResourceProvider(IDbConnection connection)
    {
        _connection = connection;
    }

    public string GetString(string key)
    {
        var sql = "SELECT localized_text FROM localization_strings WHERE key = @key AND culture = @culture";
        return _connection.ExecuteScalar<string>(sql, new { key, culture = CultureInfo.CurrentCulture.Name });
    }
}

// 注册自定义提供者
var dbProvider = new DatabaseResourceProvider(yourDbConnection);
LocalizationService.RegisterProvider(dbProvider);
```

### 监控和统计

```csharp
var stats = LocalizationService.GetStatistics();
Console.WriteLine($"提供者已加载: {stats.ProvidersLoaded}");
Console.WriteLine($"总提供者数量: {stats.TotalProviderCount}");
Console.WriteLine($"程序集提供者数量: {stats.AssemblyProviderCount}");

var providers = LocalizationService.GetProviders();
foreach (var provider in providers)
{
    Console.WriteLine($"提供者类型: {provider.GetType().Name}");
}
```

### 异常处理中的本地化

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
}
```

### 动态语言切换

```csharp
public class LocalizationManager
{
    public void SwitchLanguage(string cultureCode)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);
        LocalizationService.EnsureLoaded();
    }
}
```

## 资源文件组织

```
{程序集名称}/Localization/Messages/Resources.{文化代码}.resx

示例:
GameFrameX.Foundation.Localization/Localization/Messages/Resources.zh-CN.resx
GameFrameX.Foundation.Utility/Localization/Messages/Resources.resx
GameFrameX.Foundation.Encryption/Localization/Messages/Resources.zh-CN.resx
```

## 已集成的模块

| 模块                               | 本地化键数量 | 状态 |
|----------------------------------|--------|----|
| GameFrameX.Foundation.Utility    | 4      | 完成 |
| GameFrameX.Foundation.Encryption | 20+    | 完成 |
| GameFrameX.Foundation.Extensions | 7      | 完成 |
| GameFrameX.Foundation.Hash       | 2      | 完成 |

## 最佳实践

### 键命名规范

- **模式**：`{模块名}.{类别}.{具体键名}`
- **示例**：`Utility.Exceptions.TimestampOutOfRange`、`Encryption.InvalidKeySize`

### 参数化消息

```csharp
// 资源文件中定义
// <value>用户 '{0}' 的密码无效，长度应在 {1}-{2} 个字符之间</value>

// 代码中使用
var message = LocalizationService.GetString("User.InvalidPassword", username, minLength, maxLength);
```

### 性能优化

```csharp
// 应用启动时预加载（可选）
LocalizationService.EnsureLoaded();
```

## 常见问题

### 如何添加新语言支持？

在相应模块的 `Localization/Messages/` 目录下创建 `Resources.{语言代码}.resx` 文件，例如 `Resources.fr.resx`（法语）、`Resources.ja.resx`（日语）。

### 资源文件没有生效？

检查：

1. 资源文件是否设置为"嵌入的资源"
2. 文件命名是否正确（`Resources.{文化代码}.resx`）
3. 项目文件是否包含资源文件配置
4. 重新编译项目

## 许可证

MIT + Apache 2.0
