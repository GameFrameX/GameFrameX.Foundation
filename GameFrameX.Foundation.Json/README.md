# GameFrameX.Foundation.Json

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

GameFrameX.Foundation.Json 是一个基于 System.Text.Json 的高性能 JSON 序列化和反序列化库，提供了丰富的配置选项和特殊值处理能力。

## 🎯 核心特性

- **高性能序列化** - 基于 System.Text.Json 的高性能实现
- **特殊浮点值支持** - 完美处理 NaN、Infinity、-Infinity 等特殊浮点值
- **多种配置选项** - 提供默认和格式化两种预配置选项
- **容错性强** - 多重容错机制，确保序列化/反序列化的稳定性
- **UTF8 字节数组支持** - 支持直接操作 UTF8 字节数组
- **Try 模式** - 提供安全的尝试序列化/反序列化方法
- **枚举字符串化** - 枚举值自动序列化为字符串形式
- **循环引用处理** - 自动忽略循环引用，避免序列化异常

## 📦 安装

```bash
# 通过 NuGet 包管理器安装
Install-Package GameFrameX.Foundation.Json

# 或通过 .NET CLI 安装
dotnet add package GameFrameX.Foundation.Json
```

## 🚀 快速开始

### 基本使用

```csharp
using GameFrameX.Foundation.Json;

// 定义数据模型
public class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public double Score { get; set; }
}

// 序列化对象
var user = new User 
{ 
    Name = "张三", 
    Age = 25, 
    IsActive = true, 
    Score = 95.5 
};

string json = JsonHelper.Serialize(user);
Console.WriteLine(json);
// 输出: {"Name":"张三","Age":25,"IsActive":true,"Score":95.5}

// 反序列化对象
User deserializedUser = JsonHelper.Deserialize<User>(json);
Console.WriteLine($"姓名: {deserializedUser.Name}, 年龄: {deserializedUser.Age}");
```

### 格式化序列化

```csharp
// 生成格式化的 JSON（包含缩进和换行）
string formattedJson = JsonHelper.SerializeFormat(user);
Console.WriteLine(formattedJson);
/* 输出:
{
  "Name": "张三",
  "Age": 25,
  "IsActive": true,
  "Score": 95.5
}
*/
```

## 📋 详细使用指南

### 1. 序列化方法

#### 基本序列化
```csharp
// 使用默认配置序列化
string json = JsonHelper.Serialize(obj);

// 使用自定义配置序列化
var customOptions = new JsonSerializerOptions { WriteIndented = true };
string json = JsonHelper.Serialize(obj, customOptions);

// 格式化序列化（自动缩进）
string formattedJson = JsonHelper.SerializeFormat(obj);
```

#### UTF8 字节数组序列化
```csharp
// 序列化为 UTF8 字节数组
byte[] utf8Bytes = JsonHelper.SerializeToUtf8Bytes(obj);

// 格式化序列化为 UTF8 字节数组
byte[] formattedUtf8Bytes = JsonHelper.SerializeToUtf8BytesFormat(obj);
```

### 2. 反序列化方法

#### 基本反序列化
```csharp
// 泛型反序列化
User user = JsonHelper.Deserialize<User>(json);

// Type 类型反序列化
object obj = JsonHelper.Deserialize(json, typeof(User));

// 使用自定义配置反序列化
User user = JsonHelper.Deserialize<User>(json, customOptions);
```

#### UTF8 字节数组反序列化
```csharp
// 从 UTF8 字节数组反序列化
User user = JsonHelper.DeserializeFromUtf8Bytes<User>(utf8Bytes);

// 使用自定义配置从 UTF8 字节数组反序列化
User user = JsonHelper.DeserializeFromUtf8Bytes<User>(utf8Bytes, customOptions);
```

### 3. 安全的 Try 方法

```csharp
// 安全序列化
if (JsonHelper.TrySerialize(user, out string result))
{
    Console.WriteLine($"序列化成功: {result}");
}
else
{
    Console.WriteLine("序列化失败");
}

// 安全反序列化
if (JsonHelper.TryDeserialize<User>(json, out User user))
{
    Console.WriteLine($"反序列化成功: {user.Name}");
}
else
{
    Console.WriteLine("反序列化失败");
}
```

## ⚙️ 配置选项

### 默认配置 (DefaultOptions)

```csharp
public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
{
    // 忽略 null 值属性
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    // 忽略循环引用
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    // 忽略 JSON 注释
    ReadCommentHandling = JsonCommentHandling.Skip,
    // 使用宽松的 JavaScript 编码器
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    // 允许尾随逗号
    AllowTrailingCommas = true,
    // 属性名称大小写不敏感
    PropertyNameCaseInsensitive = true,
    // 允许从字符串读取数字和特殊浮点值
    NumberHandling = JsonNumberHandling.AllowReadingFromString | 
                    JsonNumberHandling.AllowNamedFloatingPointLiterals,
    // 自定义转换器
    Converters = {
        new JsonStringEnumConverter(), // 枚举字符串转换
        new SpecialFloatingPointConverter(), // 特殊浮点值转换 (double)
        new SpecialFloatingPointConverterFloat(), // 特殊浮点值转换 (float)
        new SpecialFloatingPointDocumentConverter(), // JSON 文档特殊浮点值转换
    }
};
```

### 格式化配置 (FormatOptions)

格式化配置在默认配置基础上增加了 `WriteIndented = true`，用于生成格式化的 JSON 输出。

## 🔧 特殊功能

### 1. 特殊浮点值处理

库内置了对特殊浮点值的完整支持：

```csharp
public class TestData
{
    public double NaNValue { get; set; } = double.NaN;
    public double InfinityValue { get; set; } = double.PositiveInfinity;
    public double NegativeInfinityValue { get; set; } = double.NegativeInfinity;
    public float FloatNaN { get; set; } = float.NaN;
}

var data = new TestData();
string json = JsonHelper.Serialize(data);
// 输出: {"NaNValue":"NaN","InfinityValue":"Infinity","NegativeInfinityValue":"-Infinity","FloatNaN":"NaN"}

TestData deserializedData = JsonHelper.Deserialize<TestData>(json);
// 特殊值正确还原
```

### 2. 枚举处理

```csharp
public enum Status
{
    Active,
    Inactive,
    Pending
}

public class Order
{
    public Status Status { get; set; } = Status.Active;
}

var order = new Order();
string json = JsonHelper.Serialize(order);
// 输出: {"Status":"Active"}  (字符串形式，而非数字)
```

### 3. 容错机制

库提供了多重容错机制：

1. **配置容错** - 默认配置失败时自动尝试格式化配置
2. **特殊值预处理** - 自动处理非标准格式的特殊浮点值
3. **多次尝试** - 失败时进行多种方式的重试

```csharp
// 即使 JSON 包含非标准格式的特殊值，也能正确处理
string problematicJson = @"{""value"": NaN, ""score"": Infinity}";
var result = JsonHelper.Deserialize<Dictionary<string, double>>(problematicJson);
// 成功反序列化，NaN 和 Infinity 被正确处理
```

## 🎨 高级用法

### 自定义配置

```csharp
var customOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
};

// 添加 GameFrameX 的特殊转换器
customOptions.Converters.Add(new SpecialFloatingPointConverter());
customOptions.Converters.Add(new JsonStringEnumConverter());

string json = JsonHelper.Serialize(data, customOptions);
```

### 批量处理

```csharp
var users = new List<User>
{
    new User { Name = "张三", Age = 25 },
    new User { Name = "李四", Age = 30 },
    new User { Name = "王五", Age = 35 }
};

// 序列化列表
string json = JsonHelper.Serialize(users);

// 反序列化列表
List<User> deserializedUsers = JsonHelper.Deserialize<List<User>>(json);
```

### 性能优化

```csharp
// 对于大量数据，使用 UTF8 字节数组可以提高性能
byte[] utf8Data = JsonHelper.SerializeToUtf8Bytes(largeDataSet);

// 直接从字节数组反序列化，避免字符串转换开销
var result = JsonHelper.DeserializeFromUtf8Bytes<LargeDataSet>(utf8Data);
```

## 💡 最佳实践

### 1. 选择合适的序列化方法

```csharp
// 对于调试和日志，使用格式化序列化
string debugJson = JsonHelper.SerializeFormat(debugData);

// 对于网络传输和存储，使用默认序列化（更紧凑）
string compactJson = JsonHelper.Serialize(networkData);

// 对于高性能场景，使用 UTF8 字节数组
byte[] highPerfData = JsonHelper.SerializeToUtf8Bytes(data);
```

### 2. 错误处理

```csharp
// 对于可能失败的操作，使用 Try 方法
if (!JsonHelper.TryDeserialize<User>(userJson, out User user))
{
    // 记录错误日志
    logger.LogError("用户数据反序列化失败: {Json}", userJson);
    // 使用默认值或抛出业务异常
    user = new User { Name = "未知用户" };
}
```

### 3. 数据模型设计

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    
    // 对于可能为 null 的属性，明确标记
    public string ErrorCode { get; set; } = null;
}
```

### 4. 配置管理

```csharp
// 为不同场景创建专用配置
public static class JsonConfigurations
{
    public static readonly JsonSerializerOptions ApiOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public static readonly JsonSerializerOptions LoggingOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}
```

## 🔍 故障排除

### 常见问题

#### 1. 特殊浮点值序列化问题
**问题**: 序列化包含 NaN 或 Infinity 的对象时出错
**解决方案**: 使用 GameFrameX.Foundation.Json，它内置了特殊浮点值处理

#### 2. 循环引用问题
**问题**: 对象间存在循环引用导致序列化失败
**解决方案**: 库的默认配置已启用 `ReferenceHandler.IgnoreCycles`

#### 3. 枚举序列化问题
**问题**: 希望枚举序列化为字符串而非数字
**解决方案**: 库默认包含 `JsonStringEnumConverter`

#### 4. 性能问题
**问题**: 大数据量序列化性能不佳
**解决方案**: 使用 UTF8 字节数组方法，避免字符串转换开销

### 调试技巧

```csharp
// 启用详细的错误信息
try
{
    var result = JsonHelper.Deserialize<ComplexObject>(json);
}
catch (JsonException ex)
{
    Console.WriteLine($"JSON 解析错误: {ex.Message}");
    Console.WriteLine($"错误位置: Line {ex.LineNumber}, Position {ex.BytePositionInLine}");
    Console.WriteLine($"问题路径: {ex.Path}");
}
```

## 📄 许可证

本项目采用 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。详细信息请参阅项目根目录中的 LICENSE 文件。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目。

## 📞 支持

如果您在使用过程中遇到问题，请通过以下方式获取帮助：

- 提交 [GitHub Issue](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- 查看项目文档和示例代码
- 参考单元测试了解更多用法

---

**GameFrameX.Foundation.Json** - 让 JSON 处理更简单、更可靠！