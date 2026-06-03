# GameFrameX.Foundation.Localization 本地化框架说明

## 概述

GameFrameX.Foundation.Localization 是 GameFrameX.Foundation 框架的本地化基础设施库，提供了轻量级、高性能的本地化解决方案。该框架采用懒加载机制，支持零配置使用，并具备优秀的线程安全特性。

## 主要特性

### 🚀 **零配置使用**

- 无需任何初始化配置
- 自动发现和加载本地化资源
- 开箱即用的简单API

### ⚡ **高性能设计**

- 懒加载机制，首次使用时才加载资源
- 多层缓存优化访问性能
- 线程安全的并发访问支持

### 🌍 **多语言支持**

- 内置中文（简体）和英文支持
- 可扩展的支持更多语言
- 智能的语言回退机制

### 🔧 **高度可扩展**

- 支持自定义资源提供者
- 灵活的优先级管理
- 模块化的组件设计

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

### 资源文件组织

```
{程序集名称}/Localization/Messages/Resources.{文化代码}.resx

示例:
GameFrameX.Foundation.Localization/Localization/Messages/Resources.zh-CN.resx
GameFrameX.Foundation.Utility/Localization/Messages/Resources.resx
GameFrameX.Foundation.Encryption/Localization/Messages/Resources.zh-CN.resx
```

## 详细使用指南

### 1. 在现有模块中集成本地化

#### 步骤1：定义本地化键

在您的模块中创建 `Localization/Keys.cs`：

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

创建 `Localization/Messages/Resources.resx`（默认英文）：

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="YourModule.Exceptions.InvalidArgument" xml:space="preserve">
    <value>Invalid argument provided for {0}</value>
  </data>
  <data name="YourModule.Exceptions.OperationFailed" xml:space="preserve">
    <value>Operation failed: {0}</value>
  </data>
  <data name="YourModule.Messages.Success" xml:space="preserve">
    <value>Operation completed successfully</value>
  </data>
  <data name="YourModule.Messages.Processing" xml:space="preserve">
    <value>Processing {0} items...</value>
  </data>
</root>
```

创建 `Localization/Messages/Resources.zh-CN.resx`（中文）：

```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="YourModule.Exceptions.InvalidArgument" xml:space="preserve">
    <value>为 {0} 提供的参数无效</value>
  </data>
  <data name="YourModule.Exceptions.OperationFailed" xml:space="preserve">
    <value>操作失败：{0}</value>
  </data>
  <data name="YourModule.Messages.Success" xml:space="preserve">
    <value>操作成功完成</value>
  </data>
  <data name="YourModule.Messages.Processing" xml:space="preserve">
    <value>正在处理 {0} 个项目...</value>
  </data>
</root>
```

#### 步骤3：更新项目文件

确保项目文件包含资源文件：

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

        // 使用带参数的本地化消息
        var processingMessage = LocalizationService.GetString(
            LocalizationKeys.Messages.Processing, 100);

        Console.WriteLine(processingMessage);

        // 处理逻辑...

        var successMessage = LocalizationService.GetString(
            LocalizationKeys.Messages.Success);
        Console.WriteLine(successMessage);
    }
}
```

### 2. 自定义资源提供者

如果需要从数据库、API或其他自定义源加载本地化资源，可以实现自定义提供者：

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
        // 从数据库查询本地化字符串
        var sql = "SELECT localized_text FROM localization_strings WHERE key = @key AND culture = @culture";
        return _connection.ExecuteScalar<string>(sql, new { key, culture = CultureInfo.CurrentCulture.Name });
    }
}

// 注册自定义提供者
var dbProvider = new DatabaseResourceProvider(yourDbConnection);
LocalizationService.RegisterProvider(dbProvider);
```

### 3. 监控和统计

```csharp
// 获取本地化系统统计信息
var stats = LocalizationService.GetStatistics();
Console.WriteLine($"提供者已加载: {stats.ProvidersLoaded}");
Console.WriteLine($"总提供者数量: {stats.TotalProviderCount}");
Console.WriteLine($"程序集提供者数量: {stats.AssemblyProviderCount}");
Console.WriteLine($"默认提供者存在: {stats.DefaultProviderExists}");

// 获取所有提供者信息
var providers = LocalizationService.GetProviders();
foreach (var provider in providers)
{
    Console.WriteLine($"提供者类型: {provider.GetType().Name}");
}
```

## 最佳实践

### 1. 键命名规范

- **模式**：`{模块名}.{类别}.{具体键名}`
- **示例**：
    - `Utility.Exceptions.TimestampOutOfRange`
    - `Encryption.InvalidKeySize`
    - `Authentication.UserNotFound`

### 2. 参数化消息

对于包含变量的消息，使用 `string.Format` 格式：

```csharp
// 资源文件
<data name="User.InvalidPassword" xml:space="preserve">
  <value>用户 '{0}' 的密码无效，长度应在 {1}-{2} 个字符之间</value>
</data>

// 代码中使用
var message = LocalizationService.GetString("User.InvalidPassword", username, minLength, maxLength);
```

### 3. 异常处理

```csharp
try
{
    var localized = LocalizationService.GetString(key);
}
catch (Exception ex)
{
    // 记录错误，但不中断程序执行
    _logger.LogWarning(ex, "获取本地化字符串失败: {Key}", key);

    // 返回键名作为兜底
    return key;
}
```

### 4. 性能优化

```csharp
// 应用启动时预加载（可选）
LocalizationService.EnsureLoaded();

// 避免频繁获取统计信息，仅在需要时调用
var stats = LocalizationService.GetStatistics();
```

## 已集成的模块

目前以下模块已完成本地化集成：

| 模块                               | 本地化键数量 | 状态   |
|----------------------------------|--------|------|
| GameFrameX.Foundation.Utility    | 4      | ✅ 完成 |
| GameFrameX.Foundation.Encryption | 20+    | ✅ 完成 |
| GameFrameX.Foundation.Extensions | 7      | ✅ 完成 |
| GameFrameX.Foundation.Hash       | 2      | ✅ 完成 |

## 常见问题

### Q: 如何添加新语言支持？

A: 在相应模块的 `Localization/Messages/` 目录下创建 `Resources.{语言代码}.resx` 文件，例如：

- `Resources.fr.resx`（法语）
- `Resources.ja.resx`（日语）
- `Resources.de.resx`（德语）

### Q: 资源文件没有生效怎么办？

A: 检查以下几点：

1. 资源文件是否设置为"嵌入的资源"
2. 文件命名是否正确（`Resources.{文化代码}.resx`）
3. 项目文件是否包含资源文件配置
4. 重新编译项目

### Q: 如何调试本地化问题？

A: 使用统计信息和提供者列表进行调试：

```csharp
var stats = LocalizationService.GetStatistics();
var providers = LocalizationService.GetProviders();

Console.WriteLine("本地化系统状态:");
Console.WriteLine($"提供者已加载: {stats.ProvidersLoaded}");
Console.WriteLine($"总提供者数量: {stats.TotalProviderCount}");

foreach (var provider in providers)
{
    Console.WriteLine($"提供者: {provider.GetType().Name}");
}
```

### Q: 自定义提供者的优先级如何？

A: 手动注册的提供者具有最高优先级，会覆盖程序集资源和默认资源。

## 版本信息

- **当前版本**: 1.0.1
- **目标框架**: .NET 10.0
- **许可证**: Apache-2.0
- **包标识**: GameFrameX.Foundation.Localization

## 更多资源

- **项目主页**: https://github.com/GameFrameX/GameFrameX
- **框架文档**: https://gameframex.doc.alianblank.com
- **问题反馈**: https://github.com/GameFrameX/GameFrameX/issues

## 总结

GameFrameX.Foundation.Localization 提供了一个完整、高效的本地化解决方案，具备以下优势：

1. **简单易用**：零配置，开箱即用
2. **高性能**：懒加载和多层缓存机制
3. **可靠性强**：完善的错误处理和兜底机制
4. **高度可扩展**：支持自定义资源提供者
5. **无缝集成**：与现有 GameFrameX 模块完美配合
6. **质量保证**：完整的测试覆盖和文档

通过采用这个本地化框架，可以轻松地为 GameFrameX.Foundation 生态系统中的所有模块提供一致、高效的本地化支持。