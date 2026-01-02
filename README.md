# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)

### 📦 程序集概览

| 程序集                                      | 功能描述          | NuGet 包名                                   | 版本                                                                                                                                                                | 下载次数                                                                                                                                                               |
|------------------------------------------|---------------|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| GameFrameX.Foundation.Encryption         | 加密工具库         | `GameFrameX.Foundation.Encryption`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 |
| GameFrameX.Foundation.Extensions         | 扩展方法库         | `GameFrameX.Foundation.Extensions`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 |
| GameFrameX.Foundation.Hash               | 哈希工具库         | `GameFrameX.Foundation.Hash`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             |
| GameFrameX.Foundation.Http.Extension     | HttpClient 扩展 | `GameFrameX.Foundation.Http.Extension`     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         |
| GameFrameX.Foundation.Http.Normalization | HTTP 消息标准化    | `GameFrameX.Foundation.Http.Normalization` | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) |
| GameFrameX.Foundation.Json               | JSON 序列化工具    | `GameFrameX.Foundation.Json`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             |
| GameFrameX.Foundation.Localization      | 本地化框架         | `GameFrameX.Foundation.Localization`      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)                 |
| GameFrameX.Foundation.Logger             | Serilog 日志配置  | `GameFrameX.Foundation.Logger`             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         |
| GameFrameX.Foundation.Options            | 命令行参数处理       | `GameFrameX.Foundation.Options`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       |
| GameFrameX.Foundation.Orm.Attribute      | ORM 特性标注      | `GameFrameX.Foundation.Orm.Attribute`      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           |
| GameFrameX.Foundation.Orm.Entity         | ORM 实体基类      | `GameFrameX.Foundation.Orm.Entity`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 |
| GameFrameX.Foundation.Utility            | 通用工具类         | `GameFrameX.Foundation.Utility`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       |

GameFrameX 的基础工具库，提供了一系列高性能、易用的基础组件和工具类，涵盖加密、哈希、HTTP、JSON、日志等常用功能。

## 🚀 快速开始

### 安装

通过 NuGet 包管理器安装所需的组件：

```bash
# 安装加密工具库
dotnet add package GameFrameX.Foundation.Encryption

# 安装扩展方法库
dotnet add package GameFrameX.Foundation.Extensions

# 安装哈希工具库
dotnet add package GameFrameX.Foundation.Hash

# 安装 JSON 工具库
dotnet add package GameFrameX.Foundation.Json

# 安装本地化框架
dotnet add package GameFrameX.Foundation.Localization

# 安装日志工具库
dotnet add package GameFrameX.Foundation.Logger

# 安装命令行参数处理库
dotnet add package GameFrameX.Foundation.Options

# 安装 HTTP 扩展
dotnet add package GameFrameX.Foundation.Http.Extension

# 安装 HTTP 消息标准化
dotnet add package GameFrameX.Foundation.Http.Normalization
```

### 基本使用

```csharp
using GameFrameX.Foundation.Encryption;
using GameFrameX.Foundation.Extensions;
using GameFrameX.Foundation.Hash;
using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Logger;
using GameFrameX.Foundation.Options;

// AES 加密
string encrypted = AesHelper.Encrypt("Hello World", "your-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-key");

// 扩展方法使用
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.Random(); // 随机获取元素
var isNullOrEmpty = myString.IsNullOrEmpty(); // 字符串检查

// 字符串扩展
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // URL安全Base64
string centered = "Hello".CenterAlignedText(20); // 居中对齐

// 对象验证
object obj = GetSomeObject();
obj.ThrowIfNull(nameof(obj)); // 空值检查
int value = 50;
value.CheckRange(1, 100); // 范围检查

// 高性能字节操作
Span<byte> buffer = stackalloc byte[8];
int offset = 0;
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);

// 双向字典
var biDict = new BidirectionalDictionary<string, int>();
biDict.TryAdd("one", 1);
if (biDict.TryGetKey(1, out string key)) { /* 反向查找 */ }

// 命令行参数处理
var builder = new OptionsBuilder<AppConfig>(args);
var config = builder.Build();

// SHA-256 哈希
string hash = Sha256Helper.ComputeHash("Hello World");

// JSON 序列化
string json = JsonHelper.Serialize(myObject);
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 本地化字符串获取
var successMessage = LocalizationService.GetString("Success");
var errorMessage = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
var formattedMessage = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);

// 日志记录
LogHandler.Create(LogOptions.Default);
LogHelper.Info("应用程序启动");
```

## 📚 详细文档

### 🧩 扩展方法库 (GameFrameX.Foundation.Extensions)

提供丰富的扩展方法集合，增强 .NET 基础类型的功能，提高开发效率和代码可读性。

#### 核心组件概览

| 组件           | 文件名                                                               | 主要功能                           |
|--------------|-------------------------------------------------------------------|--------------------------------|
| **集合扩展**     | `CollectionExtensions.cs`                                         | 为各种集合类型提供便捷操作方法                |
| **字符串扩展**    | `StringExtensions.cs`                                             | 增强字符串处理能力，包含URL安全Base64、居中对齐等  |
| **对象扩展**     | `ObjectExtensions.cs`                                             | 提供对象验证和数值范围检查                  |
| **类型扩展**     | `TypeExtensions.cs`                                               | 类型检查和反射相关扩展方法                  |
| **枚举扩展**     | `IEnumerableExtensions.cs`                                        | LINQ 增强和集合操作，支持交集、差集等          |
| **字典扩展**     | `IDictionaryExtensions.cs`                                        | 字典操作增强，支持合并、条件移除等              |
| **列表扩展**     | `ListExtensions.cs`                                               | 列表特定的扩展方法                      |
| **字节扩展**     | `ByteExtensions.cs`                                               | 字节数组操作，包含子数组提取等                |
| **Span扩展**   | `SpanExtensions.cs`                                               | 高性能内存操作，支持各种数据类型读写，包含大端序和小端序支持 |
| **只读Span扩展** | `ReadOnlySpanExtensions.cs`                                       | 只读内存的高性能读取操作                   |
| **序列读取器扩展**  | `SequenceReaderExtensions.cs`                                     | 序列数据的便捷读取方法                    |
| **双向字典**     | `BidirectionalDictionary.cs`                                      | 支持双向查找的字典实现                    |
| **查找表**      | `LookupX.cs`                                                      | 增强的一对多关系查找表                    |
| **并发队列**     | `ConcurrentLimitedQueue.cs`                                       | 线程安全的有限容量队列                    |
| **可空字典**     | `NullableDictionary.cs`<br/>`NullableConcurrentDictionary.cs`     | 支持空值的字典实现                      |
| **可释放字典**    | `DisposableDictionary.cs`<br/>`DisposableConcurrentDictionary.cs` | 值可被自动释放的字典                     |
| **常量定义**     | `ConstBaseTypeSize.cs`                                            | 基础数据类型字节大小常量                   |
| **空对象模式**    | `NullObject.cs`                                                   | 类型安全的空对象实现                     |
| **自定义异常**    | `ArgumentAlreadyException.cs`                                     | 参数已存在异常类型                      |

#### 集合扩展功能

```csharp
using GameFrameX.Foundation.Extensions;

// 集合操作
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.Random(); // 随机获取元素
var isEmpty = list.IsNullOrEmpty(); // 检查是否为空

// 字典扩展
var dict = new Dictionary<string, int>();
dict.Merge("key", 10, (old, new) => old + new); // 合并值
var value = dict.GetOrAdd("key", k => 42); // 获取或添加
dict.RemoveIf((k, v) => v > 100); // 条件移除

// HashSet 扩展
var hashSet = new HashSet<int>();
hashSet.AddRange(new[] { 1, 2, 3, 4, 5 }); // 批量添加
```

#### 字符串扩展功能

```csharp
// 字符串检查
string text = "Hello World";
bool isEmpty = text.IsNullOrEmpty();
bool isEmptyOrWhitespace = text.IsNullOrEmptyOrWhiteSpace();
bool hasContent = text.IsNotNullOrEmptyOrWhiteSpace();

// 字符串处理
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // 转换为 URL 安全格式
string restored = urlSafe.FromUrlSafeBase64(); // 还原标准格式

// 字符串操作
string centered = "Hello".CenterAlignedText(20); // 居中对齐
string cleaned = "Hello World   ".RemoveWhiteSpace(); // 移除空白字符
string trimmed = "Hello!".RemoveSuffix('!'); // 移除后缀

// 字符重复
string repeated = 'A'.RepeatChar(5); // "AAAAA"
```

#### 对象验证和范围检查

```csharp
// 空值检查
object obj = GetSomeObject();
if (obj.IsNotNull())
{
    // 对象不为空时的处理
}

// 参数验证
obj.ThrowIfNull(nameof(obj)); // 为空时抛出异常

// 数值范围检查
int value = 50;
value.CheckRange(1, 100); // 检查范围，超出时抛出异常
bool inRange = value.IsRange(1, 100); // 检查是否在范围内

// 支持多种数值类型
uint uintValue = 25;
uintValue.CheckRange(0, 50);

long longValue = 1000;
longValue.CheckRange(500, 2000);
```

#### 类型检查扩展

```csharp
// 泛型接口检查
Type listType = typeof(List<string>);
Type genericListType = typeof(List<>);
bool implementsGeneric = listType.HasImplementedRawGeneric(genericListType);

// 接口实现检查
Type stringType = typeof(string);
Type comparableType = typeof(IComparable);
bool implementsInterface = stringType.IsImplWithInterface(comparableType);
```

#### LINQ 增强扩展

```csharp
// 交集操作
var list1 = new[] { 1, 2, 3, 4, 5 };
var list2 = new[] { 3, 4, 5, 6, 7 };
var intersection = list1.IntersectBy(list2, x => x); // 按键取交集

// 多集合交集
var collections = new[] { list1, list2, new[] { 4, 5, 6 } };
var allIntersection = collections.IntersectAll(); // 所有集合的交集

// 差集操作
var difference = list1.ExceptBy(list2, (x, y) => x == y);

// 批量添加
var collection = new List<int>();
collection.AddRange(1, 2, 3, 4, 5); // 使用 params 参数
collection.AddRange(new[] { 6, 7, 8 }); // 使用数组
```

#### 双向字典

```csharp
// 创建双向字典
var biDict = new BidirectionalDictionary<string, int>();

// 添加键值对
biDict.TryAdd("one", 1);
biDict.TryAdd("two", 2);

// 双向查找
if (biDict.TryGetValue("one", out int value))
{
    Console.WriteLine($"Key 'one' maps to {value}");
}

if (biDict.TryGetKey(1, out string key))
{
    Console.WriteLine($"Value 1 maps to '{key}'");
}

// 清空字典
biDict.Clear();
```

#### 高性能扩展

```csharp
// Span 和 ReadOnlySpan 扩展
ReadOnlySpan<byte> span = stackalloc byte[] { 1, 2, 3, 4, 5 };
// 提供针对 Span 的高性能操作扩展

// 序列读取器扩展
// 为 SequenceReader 提供便捷的读取方法
```

#### 字节操作扩展

```csharp
// 字节数组扩展
byte[] data = { 1, 2, 3, 4, 5 };
byte[] subArray = data.SubArray(1, 3); // 获取子数组

// Span 和 ReadOnlySpan 扩展 - 高性能字节操作
Span<byte> buffer = stackalloc byte[16];
int offset = 0;

// 写入各种数据类型（支持大端序和小端序）
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);
buffer.WriteUIntBigEndianValue(12345u, ref offset); // 大端序写入
buffer.WriteFloatBigEndianValue(3.14f, ref offset); // 大端序写入

// 读取数据类型
offset = 0;
uint value = buffer.ReadUIntValue(ref offset);
float floatValue = buffer.ReadFloatValue(ref offset);
uint bigEndianValue = buffer.ReadUIntBigEndianValue(ref offset); // 大端序读取

// ReadOnlySpan 读取操作
ReadOnlySpan<byte> readBuffer = buffer;
offset = 0;
uint readValue = readBuffer.ReadUIntValue(ref offset);
float readFloatValue = readBuffer.ReadFloatBigEndianValue(ref offset);
```

#### 序列读取器扩展

```csharp
// 为 SequenceReader 提供便捷的读取方法
// 支持带长度前缀的字节数组读取
// 提供 TryPeek 方法进行非破坏性读取
```

#### 特殊工具类

- **ConstBaseTypeSize**: 基础数据类型字节大小常量定义，包含所有.NET基础类型的字节大小
- **NullObject**: 空对象模式实现，提供类型安全的空对象
- **NullableConcurrentDictionary**: 支持空值的线程安全并发字典
- **NullableDictionary**: 支持空值的普通字典
- **LookupX**: 增强的查找表实现，支持一对多关系映射
- **ArgumentAlreadyException**: 参数已存在异常，用于参数验证场景
- **ConcurrentLimitedQueue**: 线程安全的有限容量队列，自动移除最旧元素
- **DisposableConcurrentDictionary/DisposableDictionary**: 值可被自动释放的字典类型

### 🔐 加密工具库 (GameFrameX.Foundation.Encryption)

提供多种加密算法的实现，确保数据安全传输和存储。

#### 支持的算法

- **AES 加密** (`AesHelper`): 对称加密算法，支持字符串和字节数组
- **RSA 加密** (`RsaHelper`): 非对称加密算法，支持密钥对生成、加密解密、数字签名
- **DSA 签名** (`DsaHelper`): 数字签名算法，支持签名和验证
- **SM2/SM4 加密** (`Sm2Helper`/`Sm4Helper`): 国密算法实现
    - SM2: 非对称加密算法
    - SM4: 对称加密算法，支持 ECB/CBC 模式
- **XOR 加密** (`XorHelper`): 异或加密，支持快速加密和完整加密模式

#### 使用示例

```csharp
// AES 加密
string encrypted = AesHelper.Encrypt("敏感数据", "your-secret-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-secret-key");

// RSA 加密
var keys = RsaHelper.Make();
string encrypted = RsaHelper.Encrypt(keys["publicKey"], "Hello World");
string decrypted = RsaHelper.Decrypt(keys["privateKey"], encrypted);

// SM4 加密
string encrypted = Sm4Helper.EncryptCbc("your-key", "Hello World");
string decrypted = Sm4Helper.DecryptCbc("your-key", encrypted);
```

### 🔗 哈希工具库 (GameFrameX.Foundation.Hash)

提供多种哈希算法实现，适用于数据完整性校验、快速查找等场景。

#### 支持的算法

- **MD5** (`Md5Helper`): 128位哈希值，支持加盐
- **SHA 系列**:
    - SHA-1 (`Sha1Helper`): 160位哈希值
    - SHA-256 (`Sha256Helper`): 256位哈希值
    - SHA-512 (`Sha512Helper`): 512位哈希值
- **HMAC-SHA256** (`HmacSha256Helper`): 基于密钥的消息认证码
- **CRC 校验** (`CrcHelper`): CRC32/CRC64 循环冗余校验
- **MurmurHash3** (`MurmurHash3Helper`): 高性能非加密哈希
- **xxHash** (`XxHashHelper`): 超高性能哈希算法，支持32/64/128位

#### 使用示例

```csharp
// MD5 哈希
string md5Hash = Md5Helper.Hash("Hello World");
string saltedHash = Md5Helper.HashWithSalt("Hello World", "salt");

// SHA-256 哈希
string sha256Hash = Sha256Helper.ComputeHash("Hello World");

// HMAC-SHA256
string hmacHash = HmacSha256Helper.Hash("message", "secret-key");

// xxHash (高性能)
ulong xxHash = XxHashHelper.Hash64("Hello World");
```

### 🌐 HTTP 工具库

#### HTTP 扩展 (GameFrameX.Foundation.Http.Extension)

为 HttpClient 提供便捷的扩展方法，简化 JSON 数据的发送和接收。

```csharp
// POST JSON 请求
string response = await httpClient.PostJsonToStringAsync<MyClass>(url, myObject);
```

#### HTTP 消息标准化 (GameFrameX.Foundation.Http.Normalization)

提供统一的 HTTP 响应格式，包含 `code`、`message` 和 `data` 字段，适用于 GameFrameX 生态系统。

### 📄 JSON 序列化 (GameFrameX.Foundation.Json)

基于 `System.Text.Json` 的高性能序列化工具，提供优化的默认配置。

#### 特性

- 高性能序列化/反序列化
- 枚举序列化为字符串
- 忽略 null 值属性
- 忽略循环引用
- 属性名称大小写不敏感
- 提供格式化和紧凑两种输出模式

#### 使用示例

```csharp
// 序列化
string json = JsonHelper.Serialize(myObject);
string formattedJson = JsonHelper.Serialize(myObject, JsonHelper.FormatOptions);

// 反序列化
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 安全的反序列化
if (JsonHelper.TryDeserialize<MyClass>(json, out var result))
{
    // 处理结果
}
```

### 🌐 本地化框架 (GameFrameX.Foundation.Localization)

提供轻量级、高性能的本地化解决方案，支持零配置使用和懒加载机制，为整个 GameFrameX.Foundation 生态系统提供统一的本地化支持。

#### 主要特性

- **零配置使用**: 无需任何初始化配置，自动发现和加载本地化资源
- **懒加载机制**: 首次使用时才加载资源，启动性能优异
- **多语言支持**: 内置中文（简体）和英文支持，可扩展更多语言
- **线程安全**: 支持并发访问，适用于多线程环境
- **高度可扩展**: 支持自定义资源提供者，灵活的优先级管理
- **优先级解析**: 自定义提供者 > 程序集资源 > 默认资源

#### 核心组件

| 组件 | 文件名 | 功能 |
|------|--------|------|
| **本地化服务** | `LocalizationService.cs` | 统一的本地化入口点，提供静态方法API |
| **资源管理器** | `ResourceManager.cs` | 管理多个资源提供者，实现优先级解析 |
| **默认提供者** | `DefaultResourceProvider.cs` | 提供英文默认消息，包含50+常用消息 |
| **程序集提供者** | `AssemblyResourceProvider.cs` | 从.resx文件加载本地化资源 |

#### 基础使用

```csharp
using GameFrameX.Foundation.Localization.Core;

// 获取简单的本地化字符串
var successMessage = LocalizationService.GetString("Success");
Console.WriteLine(successMessage); // 根据当前文化显示 "Success" 或 "成功"

// 带参数的格式化消息
var errorMessage = LocalizationService.GetString("ArgumentNull", "username");
Console.WriteLine(errorMessage); // "Value cannot be null. (Parameter 'username')"

// 如果键不存在，返回键名本身
var unknown = LocalizationService.GetString("Some.Unknown.Key");
Console.WriteLine(unknown); // 输出: "Some.Unknown.Key"
```

#### 异常处理中的本地化

```csharp
using GameFrameX.Foundation.Utility.Localization;

public class UserService
{
    public void ValidateUserInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException(
                LocalizationService.GetString(LocalizationKeys.Exceptions.TimestampOutOfRange),
                nameof(input));
        }

        // 其他验证逻辑...
    }
}
```

#### 模块集成本地化

##### 1. 定义本地化键

```csharp
// YourModule/Localization/Keys.cs
namespace GameFrameX.Foundation.YourModule.Localization;

public static class LocalizationKeys
{
    public static class Validation
    {
        public const string EmailRequired = "YourModule.Validation.EmailRequired";
        public const string EmailInvalid = "YourModule.Validation.EmailInvalid";
    }

    public static class Messages
    {
        public const string UserCreated = "YourModule.Messages.UserCreated";
        public const string OperationFailed = "YourModule.Messages.OperationFailed";
    }
}
```

##### 2. 创建资源文件

在项目中创建 `Localization/Messages/Resources.resx` 和 `Localization/Messages/Resources.zh-CN.resx`：

```xml
<!-- Resources.resx (默认英文) -->
<root>
  <data name="YourModule.Validation.EmailRequired" xml:space="preserve">
    <value>Email address is required</value>
  </data>
  <data name="YourModule.Messages.UserCreated" xml:space="preserve">
    <value>User '{0}' has been created successfully</value>
  </data>
</root>
```

```xml
<!-- Resources.zh-CN.resx (中文) -->
<root>
  <data name="YourModule.Validation.EmailRequired" xml:space="preserve">
    <value>邮箱地址是必填项</value>
  </data>
  <data name="YourModule.Messages.UserCreated" xml:space="preserve">
    <value>用户 '{0}' 已成功创建</value>
  </data>
</root>
```

##### 3. 在业务逻辑中使用

```csharp
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.YourModule.Localization;

public class UserService
{
    public void CreateUser(UserDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Email))
        {
            throw new ValidationException(
                LocalizationService.GetString(LocalizationKeys.Validation.EmailRequired));
        }

        // 创建用户逻辑...

        var successMessage = LocalizationService.GetString(
            LocalizationKeys.Messages.UserCreated, userDto.Username);
        Console.WriteLine(successMessage);
    }
}
```

#### 自定义资源提供者

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
        var culture = CultureInfo.CurrentCulture.Name;
        var sql = "SELECT localized_text FROM localization_strings WHERE key = @key AND culture = @culture";
        return _connection.ExecuteScalar<string>(sql, new { key, culture });
    }
}

// 注册自定义提供者（具有最高优先级）
var dbProvider = new DatabaseResourceProvider(yourDbConnection);
LocalizationService.RegisterProvider(dbProvider);
```

#### 预加载和性能优化

```csharp
// 应用启动时预加载所有本地化资源（可选）
LocalizationService.EnsureLoaded();

// 获取本地化系统统计信息
var stats = LocalizationService.GetStatistics();
Console.WriteLine($"提供者已加载: {stats.ProvidersLoaded}");
Console.WriteLine($"总提供者数量: {stats.TotalProviderCount}");
Console.WriteLine($"程序集提供者数量: {stats.AssemblyProviderCount}");

// 获取所有提供者信息
var providers = LocalizationService.GetProviders();
foreach (var provider in providers)
{
    Console.WriteLine($"提供者: {provider.GetType().Name}");
}
```

#### 资源命名约定

- **模式**: `{模块名}.{类别}.{具体键名}`
- **示例**:
  - `Utility.Exceptions.TimestampOutOfRange`
  - `Encryption.InvalidKeySize`
  - `Authentication.UserNotFound`
  - `Success`
  - `ArgumentNull`

#### 已集成的模块

目前以下模块已完成本地化集成：

| 模块 | 本地化键数量 | 状态 |
|------|-------------|------|
| GameFrameX.Foundation.Utility | 4 | ✅ 完成 |
| GameFrameX.Foundation.Encryption | 20+ | ✅ 完成 |
| GameFrameX.Foundation.Extensions | 7 | ✅ 完成 |
| GameFrameX.Foundation.Hash | 2 | ✅ 完成 |

#### 高级功能

##### 动态语言切换

```csharp
public void SwitchLanguage(string cultureCode)
{
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

    // 可选：预加载新语言的资源
    LocalizationService.EnsureLoaded();
}
```

##### 监控和诊断

```csharp
public class LocalizationDiagnostics
{
    public void PrintStatus()
    {
        var stats = LocalizationService.GetStatistics();
        Console.WriteLine("=== 本地化系统状态 ===");
        Console.WriteLine($"提供者已加载: {stats.ProvidersLoaded}");
        Console.WriteLine($"总提供者数量: {stats.TotalProviderCount}");

        var providers = LocalizationService.GetProviders();
        foreach (var provider in providers)
        {
            Console.WriteLine($"- {provider.GetType().Name}");
        }
    }
}
```

#### 最佳实践

1. **键命名规范**: 使用 `{模块名}.{类别}.{具体键名}` 的命名模式
2. **参数化消息**: 使用 `string.Format` 格式支持参数替换
3. **异常处理**: 在异常消息中集成本地化支持
4. **性能优化**: 应用启动时可选择预加载资源
5. **测试验证**: 为本地化功能编写单元测试

#### 配置项目文件

确保项目文件包含本地化资源文件：

```xml
<PropertyGroup>
  <EnableDefaultEmbeddedResourceItems>false</EnableDefaultEmbeddedResourceItems>
</PropertyGroup>

<ItemGroup>
  <EmbeddedResource Include="Localization\Messages\*.resx" />
</ItemGroup>
```

更多详细信息请参考：
- [本地化框架完整文档](GameFrameX.Foundation.Localization/README.Localization.md)
- [使用示例和最佳实践](GameFrameX.Foundation.Localization/USAGE_EXAMPLES.md)

### �️ ORM 实体基类 (GameFrameX.Foundation.Orm.Entity)

提供ORM框架的实体基类和接口定义，支持审计跟踪、软删除、乐观锁等企业级功能。

#### 核心组件概览

| 组件           | 文件名                   | 主要功能                          |
|--------------|-----------------------|-------------------------------|
| **实体基类**     | `EntityBase.cs`       | 完整功能的实体基类，包含ID、审计、软删除、版本控制等功能 |
| **实体基类(泛型)** | `EntityBaseId.cs`     | 支持自定义主键类型的实体基类                |
| **实体接口**     | `IEntity.cs`          | 基础实体接口定义，提供ID属性               |
| **审计接口**     | `IAuditableEntity.cs` | 审计功能接口，定义创建时间、更新时间、操作用户等审计字段  |

#### 实体基类功能

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 继承EntityBase的实体类自动获得完整的企业级功能
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // 以下属性由EntityBase提供：
    // - long Id                    // 主键ID
    // - DateTime CreateTime        // 创建时间
    // - DateTime UpdateTime        // 更新时间
    // - long CreateUserId          // 创建用户ID
    // - long UpdateUserId          // 更新用户ID
    // - string CreateUserName      // 创建用户名
    // - string UpdateUserName      // 更新用户名
    // - bool IsDelete              // 软删除标记
    // - long Version               // 乐观锁版本号
    // - bool IsEnabled             // 启用状态
}

// 使用示例
var user = new User
{
    Username = "john_doe",
    Email = "john@example.com",
    PasswordHash = "hashed_password",
    CreateTime = DateTime.UtcNow,
    CreateUserId = 1,
    CreateUserName = "admin",
    IsEnabled = true
};
```

#### 自定义主键类型

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 使用字符串作为主键
public class Product : EntityBaseId<string>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    // Id属性类型为string，由EntityBaseId<string>提供
}

// 使用Guid作为主键
public class Order : EntityBaseId<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Id属性类型为Guid，由EntityBaseId<Guid>提供
}

// 使用示例
var product = new Product
{
    Id = "PROD-001",
    Name = "笔记本电脑",
    Price = 5999.99m,
    Description = "高性能笔记本电脑"
};

var order = new Order
{
    Id = Guid.NewGuid(),
    OrderNumber = "ORD-20240101-001",
    TotalAmount = 5999.99m,
    OrderDate = DateTime.UtcNow
};
```

#### 接口实现

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 实现基础实体接口
public class Category : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// 实现审计接口
public class AuditableCategory : IEntity<int>, IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // IAuditableEntity接口要求的属性
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public long CreateUserId { get; set; }
    public long UpdateUserId { get; set; }
    public string CreateUserName { get; set; }
    public string UpdateUserName { get; set; }
}
```

#### 企业级功能详解

##### 1. 审计跟踪 (Audit Trail)

```csharp
// EntityBase自动提供审计字段
public class Document : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
}

// 在业务逻辑中设置审计信息
var document = new Document
{
    Title = "重要文档",
    Content = "文档内容...",
    CreateTime = DateTime.UtcNow,
    CreateUserId = currentUser.Id,
    CreateUserName = currentUser.Username,
    UpdateTime = DateTime.UtcNow,
    UpdateUserId = currentUser.Id,
    UpdateUserName = currentUser.Username
};

// 更新时自动维护审计信息
document.Content = "更新后的内容";
document.UpdateTime = DateTime.UtcNow;
document.UpdateUserId = currentUser.Id;
document.UpdateUserName = currentUser.Username;
document.Version++; // 乐观锁版本递增
```

##### 2. 软删除 (Soft Delete)

```csharp
// 软删除：不真正删除记录，而是标记为已删除
public void SoftDeleteUser(User user)
{
    user.IsDelete = true;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    // 保存到数据库，记录仍然存在但被标记为已删除
    dbContext.SaveChanges();
}

// 查询时过滤已删除的记录
var activeUsers = dbContext.Users
    .Where(u => !u.IsDelete)
    .ToList();

// 恢复已删除的记录
public void RestoreUser(User user)
{
    user.IsDelete = false;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    dbContext.SaveChanges();
}
```

##### 3. 乐观锁 (Optimistic Locking)

```csharp
// 使用Version字段实现乐观锁
public void UpdateUserWithOptimisticLock(long userId, string newEmail)
{
    var user = dbContext.Users.Find(userId);
    if (user == null) throw new EntityNotFoundException();
    
    var originalVersion = user.Version;
    
    // 修改数据
    user.Email = newEmail;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    user.Version++; // 版本号递增
    
    try
    {
        // 保存时检查版本号
        var rowsAffected = dbContext.Database.ExecuteSqlRaw(
            "UPDATE Users SET Email = {0}, UpdateTime = {1}, UpdateUserId = {2}, UpdateUserName = {3}, Version = {4} " +
            "WHERE Id = {5} AND Version = {6}",
            user.Email, user.UpdateTime, user.UpdateUserId, user.UpdateUserName, user.Version, user.Id, originalVersion);
            
        if (rowsAffected == 0)
        {
            throw new ConcurrencyException("数据已被其他用户修改，请刷新后重试");
        }
    }
    catch (DbUpdateConcurrencyException)
    {
        throw new ConcurrencyException("并发冲突，请刷新后重试");
    }
}
```

##### 4. 启用状态管理

```csharp
// 使用IsEnabled字段管理实体的启用状态
public class Feature : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    // IsEnabled由EntityBase提供
}

// 启用/禁用功能
public void ToggleFeature(long featureId, bool enabled)
{
    var feature = dbContext.Features.Find(featureId);
    if (feature == null) throw new EntityNotFoundException();
    
    feature.IsEnabled = enabled;
    feature.UpdateTime = DateTime.UtcNow;
    feature.UpdateUserId = currentUser.Id;
    feature.UpdateUserName = currentUser.Username;
    feature.Version++;
    
    dbContext.SaveChanges();
}

// 查询启用的功能
var enabledFeatures = dbContext.Features
    .Where(f => f.IsEnabled && !f.IsDelete)
    .ToList();
```

#### 完整使用示例

```csharp
using GameFrameX.Foundation.Orm.Entity;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Entities
{
    // 用户实体
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        
        // 导航属性
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    
    // 订单实体（使用Guid主键）
    public class Order : EntityBaseId<Guid>
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        // 导航属性
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 订单项实体
    public class OrderItem : EntityBase
    {
        public Guid OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        // 导航属性
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
    
    // 产品实体（使用字符串主键）
    public class Product : EntityBaseId<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; }
        
        // 导航属性
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 分类实体（实现接口）
    public class Category : IEntity<string>, IAuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        
        // IAuditableEntity接口属性
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        
        // 导航属性
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}

// 业务服务示例
namespace MyApplication.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        
        public UserService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<User> CreateUserAsync(string username, string email, string password)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                CreateUserId = currentUser.Id,
                UpdateUserId = currentUser.Id,
                CreateUserName = currentUser.Username,
                UpdateUserName = currentUser.Username,
                IsEnabled = true,
                IsDelete = false,
                Version = 1
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        
        public async Task<User> UpdateUserAsync(long userId, string email, string firstName, string lastName)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && !u.IsDelete)
                .FirstOrDefaultAsync();
                
            if (user == null)
                throw new EntityNotFoundException($"用户 {userId} 不存在");
            
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var originalVersion = user.Version;
            
            // 更新字段
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = currentUser.Id;
            user.UpdateUserName = currentUser.Username;
            user.Version++;
            
            try
            {
                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException("用户信息已被其他用户修改，请刷新后重试");
            }
        }
        
        public async Task SoftDeleteUserAsync(long userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && !u.IsDelete)
                .FirstOrDefaultAsync();
                
            if (user == null)
                throw new EntityNotFoundException($"用户 {userId} 不存在");
            
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            user.IsDelete = true;
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = currentUser.Id;
            user.UpdateUserName = currentUser.Username;
            user.Version++;
            
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsEnabled && !u.IsDelete)
                .OrderBy(u => u.CreateTime)
                .ToListAsync();
        }
        
        private string HashPassword(string password)
        {
            // 实现密码哈希逻辑
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

### 🏷️ ORM 属性标记 (GameFrameX.Foundation.Orm.Attribute)

提供ORM框架的属性标记，用于标识实体类的特殊功能，如审计跟踪、缓存策略、软删除和版本控制等。

#### 核心组件概览

| 组件           | 文件名                    | 主要功能                                    |
|--------------|------------------------|-----------------------------------------|
| **审计表属性**    | `AuditTableAttribute.cs` | 标记实体类支持审计跟踪功能，记录数据变更历史                  |
| **缓存表属性**    | `CacheTableAttribute.cs` | 标记实体类支持缓存策略，提升数据访问性能                    |
| **软删除属性**    | `SoftDeleteAttribute.cs` | 标记实体类支持软删除功能，逻辑删除而非物理删除                 |
| **版本控制属性**   | `VersionControlAttribute.cs` | 标记实体类支持数据版本管理，实现乐观锁和并发控制               |

#### 审计表属性 (AuditTableAttribute)

用于标记需要进行审计跟踪的实体类，系统会自动记录数据的创建、修改、删除等操作历史。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 标记用户表需要审计跟踪
[AuditTable]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase已包含审计字段：
    // CreateTime, UpdateTime, CreateUserId, UpdateUserId, 
    // CreateUserName, UpdateUserName
}

// 标记订单表需要审计跟踪
[AuditTable]
public class Order : EntityBase
{
    public string OrderNumber { get; set; }
    public long UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}

// 审计拦截器示例
public class AuditInterceptor : IDbCommandInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    
    public AuditInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().GetCustomAttribute<AuditTableAttribute>() != null)
            .ToList();
            
        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditableEntity auditableEntity)
            {
                var currentUser = _currentUserService.GetCurrentUser();
                var now = DateTime.UtcNow;
                
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreateTime = now;
                        auditableEntity.UpdateTime = now;
                        auditableEntity.CreateUserId = currentUser.Id;
                        auditableEntity.UpdateUserId = currentUser.Id;
                        auditableEntity.CreateUserName = currentUser.Username;
                        auditableEntity.UpdateUserName = currentUser.Username;
                        break;
                        
                    case EntityState.Modified:
                        auditableEntity.UpdateTime = now;
                        auditableEntity.UpdateUserId = currentUser.Id;
                        auditableEntity.UpdateUserName = currentUser.Username;
                        break;
                }
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}
```

#### 缓存表属性 (CacheTableAttribute)

用于标记支持缓存策略的实体类，系统会自动对这些表的数据进行缓存管理。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 标记配置表支持缓存（配置数据变化频率低，适合缓存）
[CacheTable]
public class SystemConfig : EntityBase
{
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

// 标记字典表支持缓存（字典数据相对稳定，适合缓存）
[CacheTable]
public class Dictionary : EntityBase
{
    public string DictType { get; set; }
    public string DictKey { get; set; }
    public string DictValue { get; set; }
    public string Description { get; set; }
    public int SortOrder { get; set; }
}

// 标记权限表支持缓存（权限数据访问频繁但变化不频繁）
[CacheTable]
public class Permission : EntityBase
{
    public string PermissionCode { get; set; }
    public string PermissionName { get; set; }
    public string Description { get; set; }
    public string Module { get; set; }
}

// 缓存服务示例
public class CacheService<T> where T : class
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDbContext _dbContext;
    private readonly ILogger<CacheService<T>> _logger;
    
    public CacheService(IMemoryCache memoryCache, IDbContext dbContext, ILogger<CacheService<T>> logger)
    {
        _memoryCache = memoryCache;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        var entityType = typeof(T);
        var cacheAttribute = entityType.GetCustomAttribute<CacheTableAttribute>();
        
        if (cacheAttribute == null)
        {
            // 不支持缓存，直接从数据库查询
            return await _dbContext.Set<T>().ToListAsync();
        }
        
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<T> cachedData))
        {
            _logger.LogDebug($"从缓存获取数据: {cacheKey}");
            return cachedData;
        }
        
        // 从数据库查询并缓存
        var data = await _dbContext.Set<T>().ToListAsync();
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), // 30分钟过期
            SlidingExpiration = TimeSpan.FromMinutes(5), // 5分钟滑动过期
            Priority = CacheItemPriority.Normal
        };
        
        _memoryCache.Set(cacheKey, data, cacheOptions);
        _logger.LogDebug($"数据已缓存: {cacheKey}, 记录数: {data.Count}");
        
        return data;
    }
    
    public async Task InvalidateCacheAsync()
    {
        var entityType = typeof(T);
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        _memoryCache.Remove(cacheKey);
        _logger.LogDebug($"缓存已失效: {cacheKey}");
    }
}

// 缓存管理器示例
public class CacheManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CacheManager> _logger;
    
    public CacheManager(IServiceProvider serviceProvider, ILogger<CacheManager> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task RefreshAllCacheTablesAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var cacheTableTypes = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<CacheTableAttribute>() != null)
            .ToList();
            
        foreach (var type in cacheTableTypes)
        {
            try
            {
                var serviceType = typeof(CacheService<>).MakeGenericType(type);
                var service = _serviceProvider.GetService(serviceType);
                
                if (service != null)
                {
                    var invalidateMethod = serviceType.GetMethod("InvalidateCacheAsync");
                    await (Task)invalidateMethod.Invoke(service, null);
                    
                    var getAllMethod = serviceType.GetMethod("GetAllAsync");
                    await (Task)getAllMethod.Invoke(service, null);
                    
                    _logger.LogInformation($"缓存表 {type.Name} 已刷新");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"刷新缓存表 {type.Name} 时发生错误");
            }
        }
    }
}
```

#### 软删除属性 (SoftDeleteAttribute)

用于标记支持软删除功能的实体类，删除操作会将记录标记为已删除而不是物理删除。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 标记用户表支持软删除
[SoftDelete]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase已包含IsDelete字段
}

// 标记文章表支持软删除
[SoftDelete]
public class Article : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
    public long AuthorId { get; set; }
    public DateTime PublishTime { get; set; }
}

// 软删除拦截器
public class SoftDeleteInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 处理软删除实体的删除操作
        var softDeleteEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && 
                       e.Entity.GetType().GetCustomAttribute<SoftDeleteAttribute>() != null)
            .ToList();
            
        foreach (var entry in softDeleteEntries)
        {
            // 将删除操作转换为更新操作
            entry.State = EntityState.Modified;
            
            if (entry.Entity is EntityBase entityBase)
            {
                entityBase.IsDelete = true;
                entityBase.UpdateTime = DateTime.UtcNow;
                // 设置更新用户信息...
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 软删除查询过滤器
public static class SoftDeleteQueryExtensions
{
    public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> query) 
        where T : class
    {
        var entityType = typeof(T);
        var softDeleteAttribute = entityType.GetCustomAttribute<SoftDeleteAttribute>();
        
        if (softDeleteAttribute != null && typeof(EntityBase).IsAssignableFrom(entityType))
        {
            return query.Where(e => !((EntityBase)(object)e).IsDelete);
        }
        
        return query;
    }
    
    public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query) 
        where T : class
    {
        // 返回包含已删除记录的查询
        return query;
    }
    
    public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query) 
        where T : class
    {
        var entityType = typeof(T);
        var softDeleteAttribute = entityType.GetCustomAttribute<SoftDeleteAttribute>();
        
        if (softDeleteAttribute != null && typeof(EntityBase).IsAssignableFrom(entityType))
        {
            return query.Where(e => ((EntityBase)(object)e).IsDelete);
        }
        
        return query.Where(_ => false); // 如果不支持软删除，返回空结果
    }
}

// 使用示例
public class UserService
{
    private readonly ApplicationDbContext _context;
    
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // 获取活跃用户（自动过滤已删除）
    public async Task<List<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .WhereNotDeleted()
            .ToListAsync();
    }
    
    // 获取已删除用户
    public async Task<List<User>> GetDeletedUsersAsync()
    {
        return await _context.Users
            .OnlyDeleted()
            .ToListAsync();
    }
    
    // 获取所有用户（包含已删除）
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .IncludeDeleted()
            .ToListAsync();
    }
    
    // 软删除用户
    public async Task SoftDeleteUserAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user); // 会被拦截器转换为软删除
            await _context.SaveChangesAsync();
        }
    }
    
    // 恢复已删除用户
    public async Task RestoreUserAsync(long userId)
    {
        var user = await _context.Users
            .IncludeDeleted()
            .FirstOrDefaultAsync(u => u.Id == userId);
            
        if (user != null && user.IsDelete)
        {
            user.IsDelete = false;
            user.UpdateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
```

#### 版本控制属性 (VersionControlAttribute)

用于标记支持数据版本管理的实体类，实现乐观锁和并发控制功能。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 标记用户表支持版本控制
[VersionControl]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase已包含Version字段
}

// 标记库存表支持版本控制（防止超卖）
[VersionControl]
public class Inventory : EntityBase
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public decimal UnitCost { get; set; }
}

// 标记账户余额表支持版本控制（防止并发操作导致余额错误）
[VersionControl]
public class AccountBalance : EntityBase
{
    public long UserId { get; set; }
    public decimal Balance { get; set; }
    public decimal FrozenAmount { get; set; }
    public string Currency { get; set; }
}

// 版本控制拦截器
public class VersionControlInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 处理版本控制实体的更新操作
        var versionControlEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified && 
                       e.Entity.GetType().GetCustomAttribute<VersionControlAttribute>() != null)
            .ToList();
            
        foreach (var entry in versionControlEntries)
        {
            if (entry.Entity is EntityBase entityBase)
            {
                // 自动递增版本号
                entityBase.Version++;
                
                // 标记Version字段为已修改
                entry.Property(nameof(EntityBase.Version)).IsModified = true;
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 版本控制服务
public class VersionControlService<T> where T : EntityBase
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<VersionControlService<T>> _logger;
    
    public VersionControlService(IDbContext dbContext, ILogger<VersionControlService<T>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<T> UpdateWithVersionCheckAsync(long id, Action<T> updateAction, int maxRetries = 3)
    {
        var entityType = typeof(T);
        var versionControlAttribute = entityType.GetCustomAttribute<VersionControlAttribute>();
        
        if (versionControlAttribute == null)
        {
            throw new InvalidOperationException($"实体类型 {entityType.Name} 未标记 VersionControlAttribute");
        }
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new EntityNotFoundException($"实体 {entityType.Name} (ID: {id}) 不存在");
                }
                
                var originalVersion = entity.Version;
                
                // 执行更新操作
                updateAction(entity);
                
                // 设置更新时间
                entity.UpdateTime = DateTime.UtcNow;
                
                // 保存更改
                await _dbContext.SaveChangesAsync();
                
                _logger.LogDebug($"实体 {entityType.Name} (ID: {id}) 更新成功，版本从 {originalVersion} 更新到 {entity.Version}");
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning($"实体 {entityType.Name} (ID: {id}) 版本冲突，第 {attempt} 次重试");
                
                if (attempt == maxRetries)
                {
                    throw new ConcurrencyException($"实体 {entityType.Name} (ID: {id}) 在 {maxRetries} 次重试后仍然存在版本冲突", ex);
                }
                
                // 重新加载实体以获取最新版本
                _dbContext.Entry(await _dbContext.Set<T>().FindAsync(id)).Reload();
                
                // 等待一段时间后重试
                await Task.Delay(TimeSpan.FromMilliseconds(100 * attempt));
            }
        }
        
        throw new InvalidOperationException("不应该到达这里");
    }
}

// 使用示例
public class InventoryService
{
    private readonly VersionControlService<Inventory> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public InventoryService(VersionControlService<Inventory> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 减少库存（防止超卖）
    public async Task<bool> ReduceInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"产品 {productId} 的库存记录不存在");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
            {
                if (inv.Quantity < quantity)
                {
                    throw new InsufficientInventoryException($"库存不足，当前库存: {inv.Quantity}，需要: {quantity}");
                }
                
                inv.Quantity -= quantity;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            // 版本冲突，可能是并发操作导致
            throw new ConcurrencyException("库存更新失败，请重试");
        }
    }
    
    // 增加库存
    public async Task AddInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"产品 {productId} 的库存记录不存在");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
        {
            inv.Quantity += quantity;
        });
    }
}

// 账户余额服务示例
public class AccountBalanceService
{
    private readonly VersionControlService<AccountBalance> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public AccountBalanceService(VersionControlService<AccountBalance> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 扣减余额
    public async Task<bool> DeductBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"用户 {userId} 的 {currency} 账户不存在");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
            {
                if (bal.Balance < amount)
                {
                    throw new InsufficientBalanceException($"余额不足，当前余额: {bal.Balance}，需要: {amount}");
                }
                
                bal.Balance -= amount;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            throw new ConcurrencyException("余额更新失败，请重试");
        }
    }
    
    // 增加余额
    public async Task AddBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"用户 {userId} 的 {currency} 账户不存在");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
        {
            bal.Balance += amount;
        });
    }
}
```

#### 完整集成示例

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Entities
{
    // 用户实体：支持审计、软删除、版本控制
    [AuditTable]
    [SoftDelete]
    [VersionControl]
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
    
    // 系统配置：支持缓存、审计
    [CacheTable]
    [AuditTable]
    public class SystemConfig : EntityBase
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
    
    // 库存记录：支持版本控制、审计
    [VersionControl]
    [AuditTable]
    public class Inventory : EntityBase
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public string WarehouseCode { get; set; }
    }
    
    // 订单记录：支持审计、软删除
    [AuditTable]
    [SoftDelete]
    public class Order : EntityBase
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

// DbContext配置
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<SystemConfig> SystemConfigs { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(
                new AuditInterceptor(),
                new SoftDeleteInterceptor(),
                new VersionControlInterceptor()
            );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 为所有标记了SoftDeleteAttribute的实体添加全局查询过滤器
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType.GetCustomAttribute<SoftDeleteAttribute>() != null &&
                typeof(EntityBase).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "e");
                var property = Expression.Property(parameter, nameof(EntityBase.IsDelete));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                
                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
        }
        
        base.OnModelCreating(modelBuilder);
    }
}

// 服务注册
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    services.AddScoped<AuditInterceptor>();
    services.AddScoped<SoftDeleteInterceptor>();
    services.AddScoped<VersionControlInterceptor>();
    
    services.AddScoped(typeof(CacheService<>));
    services.AddScoped(typeof(VersionControlService<>));
    services.AddScoped<CacheManager>();
    
    services.AddMemoryCache();
}
```

### � 日志工具库 (GameFrameX.Foundation.Logger)

基于 Serilog 的日志配置工具，提供简单易用的日志记录功能。

#### 特性

- 支持多种日志级别 (Debug, Info, Warning, Error, Fatal)
- 灵活的输出配置
- 支持自定义日志提供程序
- 提供日志自我诊断

#### 使用示例

```csharp
// 初始化日志
LogHandler.Create(LogOptions.Default);

// 记录日志
LogHelper.Debug("调试信息");
LogHelper.Info("普通信息");
LogHelper.Warning("警告信息");
LogHelper.Error("错误信息");
LogHelper.Fatal("致命错误");
```

### ⚙️ 命令行参数处理 (GameFrameX.Foundation.Options)

一个强大的命令行参数和环境变量解析库，支持将命令行参数和环境变量自动映射到强类型配置对象。

#### 特性

- ✅ **参数优先级处理**: 命令行参数 > 环境变量 > 默认值
- ✅ **泛型支持**: 支持任意强类型配置类
- ✅ **多种启动方式兼容**: 支持Docker、exe、shell等启动方式
- ✅ **自动前缀处理**: 自动为参数添加`--`前缀
- ✅ **布尔参数支持**: 支持多种布尔参数格式
- ✅ **环境变量映射**: 自动映射环境变量到配置属性
- ✅ **类型转换**: 自动转换字符串参数到目标类型
- ✅ **特性支持**: 支持丰富的配置特性

#### 核心组件

| 组件                             | 功能描述                 |
|--------------------------------|----------------------|
| `CommandLineArgumentConverter` | 命令行参数转换器，提供参数处理的核心功能 |
| `OptionsBuilder<T>`            | 配置构建器，用于构建泛型配置对象     |
| `OptionsProvider`              | 配置提供器，用于获取和管理配置对象    |

#### 快速开始

##### 1. 定义配置类

```csharp
public class AppConfig
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    public bool Debug { get; set; } = false;
    public string LogLevel { get; set; } = "info";
    public double Timeout { get; set; } = 30.5;
}
```

##### 2. 使用OptionsBuilder

```csharp
using GameFrameX.Foundation.Options;

class Program
{
    static void Main(string[] args)
    {
        // 创建选项构建器
        var builder = new OptionsBuilder<AppConfig>(args);
        
        // 构建配置对象
        var config = builder.Build();
        
        // 使用配置
        Console.WriteLine($"服务器: {config.Host}:{config.Port}");
        Console.WriteLine($"调试模式: {config.Debug}");
        Console.WriteLine($"日志级别: {config.LogLevel}");
        Console.WriteLine($"超时时间: {config.Timeout}秒");
    }
}
```

#### 使用方式

##### 命令行参数

支持多种参数格式：

```bash
# 键值对格式
myapp.exe --host=example.com --port=9090 --debug=true

# 分离格式
myapp.exe --host example.com --port 9090 --debug true

# 布尔标志格式
myapp.exe --host example.com --port 9090 --debug

# 混合格式
myapp.exe --host=example.com --port 9090 --debug
```

##### 环境变量

```bash
# 设置环境变量
export HOST=example.com
export PORT=9090
export DEBUG=true

# 运行程序
myapp.exe
```

##### Docker支持

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

```bash
# Docker运行
docker run myapp --host example.com --port 9090 --debug

# 或使用环境变量
docker run -e HOST=example.com -e PORT=9090 -e DEBUG=true myapp
```

#### 高级特性

##### 使用特性配置

```csharp
using GameFrameX.Foundation.Options.Attributes;

public class AdvancedConfig
{
    [Option("h", "host", Required = false, DefaultValue = "localhost")]
    [HelpText("服务器主机地址")]
    public string Host { get; set; }

    [Option("p", "port", Required = true)]
    [HelpText("服务器端口号")]
    public int Port { get; set; }

    [FlagOption("d", "debug")]
    [HelpText("启用调试模式")]
    public bool Debug { get; set; }

    [RequiredOption("api-key", Required = true)]
    [EnvironmentVariable("API_KEY")]
    [HelpText("API密钥")]
    public string ApiKey { get; set; }

    [DefaultValue(30.0)]
    public double Timeout { get; set; }
}
```

##### 构建器选项

```csharp
var builder = new OptionsBuilder<AppConfig>(
    args: args,
    boolFormat: BoolArgumentFormat.Flag,        // 布尔参数格式
    ensurePrefixedKeys: true,                   // 确保参数有前缀
    useEnvironmentVariables: true              // 使用环境变量
);

var config = builder.Build(skipValidation: false); // 是否跳过验证
```

#### 参数优先级

参数按以下优先级应用（高优先级覆盖低优先级）：

1. **命令行参数** (最高优先级)
2. **环境变量**
3. **默认值** (最低优先级)

##### 示例

```csharp
public class Config
{
    public string Host { get; set; } = "localhost";  // 默认值
    public int Port { get; set; } = 8080;           // 默认值
}
```

```bash
# 设置环境变量
export HOST=env.example.com
export PORT=7070

# 运行程序（命令行参数覆盖环境变量）
myapp.exe --host cmd.example.com

# 结果：
# Host = "cmd.example.com"  (来自命令行参数)
# Port = 7070               (来自环境变量)
```

#### 布尔参数处理

支持多种布尔参数格式：

```bash
# 标志格式（推荐）
myapp.exe --debug                    # debug = true

# 键值对格式
myapp.exe --debug=true               # debug = true
myapp.exe --debug=false              # debug = false

# 分离格式
myapp.exe --debug true               # debug = true
myapp.exe --debug false              # debug = false

# 支持的布尔值
true, false, 1, 0, yes, no, on, off
```

#### 类型转换

自动支持以下类型转换：

- `string` - 直接使用
- `int`, `int?` - 整数转换
- `bool`, `bool?` - 布尔值转换
- `double`, `double?` - 双精度浮点数转换
- `float`, `float?` - 单精度浮点数转换
- `decimal`, `decimal?` - 十进制数转换
- `DateTime`, `DateTime?` - 日期时间转换
- `Guid`, `Guid?` - GUID转换
- `Enum` - 枚举转换

##### 示例

```csharp
public class TypedConfig
{
    public int Port { get; set; }
    public bool Debug { get; set; }
    public DateTime StartTime { get; set; }
    public LogLevel Level { get; set; }  // 枚举
}

public enum LogLevel
{
    Debug, Info, Warning, Error
}
```

```bash
myapp.exe --port 9090 --debug true --start-time "2024-01-01 10:00:00" --level Info
```

#### 错误处理

##### 必需参数验证

```csharp
public class Config
{
    [RequiredOption("api-key", Required = true)]
    public string ApiKey { get; set; }
}
```

如果缺少必需参数，会抛出 `ArgumentException`：

```
缺少必需的选项: api-key
```

##### 类型转换错误

当参数值无法转换为目标类型时，会使用默认值并在控制台输出警告信息。

#### 最佳实践

##### 1. 配置类设计

```csharp
public class AppConfig
{
    // 使用有意义的默认值
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    
    // 布尔属性默认为false
    public bool Debug { get; set; } = false;
    
    // 使用特性提供更多信息
    [RequiredOption("database-url", Required = true)]
    [EnvironmentVariable("DATABASE_URL")]
    public string DatabaseUrl { get; set; }
}
```

##### 2. 错误处理

```csharp
try
{
    var builder = new OptionsBuilder<AppConfig>(args);
    var config = builder.Build();
    
    // 使用配置启动应用
    StartApplication(config);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"配置错误: {ex.Message}");
    Environment.Exit(1);
}
```

##### 3. Docker集成

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = new OptionsBuilder<AppConfig>(args);
        var config = builder.Build();
        
        // 在Docker中，通常使用环境变量
        // 在开发中，通常使用命令行参数
        
        var app = CreateApplication(config);
        app.Run();
    }
}
```

```yaml
# docker-compose.yml
version: '3.8'
services:
  myapp:
    image: myapp:latest
    environment:
      - HOST=0.0.0.0
      - PORT=8080
      - DEBUG=false
    command: [ "--log-level", "info" ]
```

#### 完整示例

```csharp
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace MyApp
{
    public class ServerConfig
    {
        [Option("h", "host", DefaultValue = "localhost")]
        [EnvironmentVariable("SERVER_HOST")]
        [HelpText("服务器主机地址")]
        public string Host { get; set; }

        [Option("p", "port", DefaultValue = 8080)]
        [EnvironmentVariable("SERVER_PORT")]
        [HelpText("服务器端口号")]
        public int Port { get; set; }

        [FlagOption("d", "debug")]
        [EnvironmentVariable("DEBUG")]
        [HelpText("启用调试模式")]
        public bool Debug { get; set; }

        [RequiredOption("database-url", Required = true)]
        [EnvironmentVariable("DATABASE_URL")]
        [HelpText("数据库连接字符串")]
        public string DatabaseUrl { get; set; }

        [Option("timeout", DefaultValue = 30.0)]
        [EnvironmentVariable("REQUEST_TIMEOUT")]
        [HelpText("请求超时时间（秒）")]
        public double Timeout { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new OptionsBuilder<ServerConfig>(args);
                var config = builder.Build();

                Console.WriteLine("服务器配置:");
                Console.WriteLine($"  主机: {config.Host}");
                Console.WriteLine($"  端口: {config.Port}");
                Console.WriteLine($"  调试: {config.Debug}");
                Console.WriteLine($"  数据库: {config.DatabaseUrl}");
                Console.WriteLine($"  超时: {config.Timeout}秒");

                // 启动服务器
                StartServer(config);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"配置错误: {ex.Message}");
                ShowHelp();
                Environment.Exit(1);
            }
        }

        static void StartServer(ServerConfig config)
        {
            // 服务器启动逻辑
            Console.WriteLine($"服务器启动在 {config.Host}:{config.Port}");
        }

        static void ShowHelp()
        {
            Console.WriteLine("用法:");
            Console.WriteLine("  myapp.exe --host <主机> --port <端口> --database-url <数据库URL> [选项]");
            Console.WriteLine();
            Console.WriteLine("选项:");
            Console.WriteLine("  -h, --host <主机>           服务器主机地址 (默认: localhost)");
            Console.WriteLine("  -p, --port <端口>           服务器端口号 (默认: 8080)");
            Console.WriteLine("  -d, --debug                 启用调试模式");
            Console.WriteLine("      --database-url <URL>    数据库连接字符串 (必需)");
            Console.WriteLine("      --timeout <秒>          请求超时时间 (默认: 30.0)");
        }
    }
}
```

#### CommandLineArgumentConverter 使用

除了 OptionsBuilder 之外，您也可以直接使用底层的 CommandLineArgumentConverter：

```csharp
using GameFrameX.Foundation.Options;

// 创建转换器实例
var converter = new CommandLineArgumentConverter();

// 原始命令行参数
var args = new[] { "--port", "8080", "-h", "localhost" };

// 设置环境变量（可选）
Environment.SetEnvironmentVariable("APP_NAME", "MyApplication");
Environment.SetEnvironmentVariable("LOG_LEVEL", "debug-mode");

// 转换为标准格式（合并命令行参数和环境变量）
var standardArgs = converter.ConvertToStandardFormat(args);
// 结果: ["--port", "8080", "-h", "localhost", "--APP_NAME", "MyApplication", "--LOG_LEVEL", "debugmode"]

// 转换为命令行字符串
var commandLineString = converter.ToCommandLineString(standardArgs);
// 结果: "--port 8080 -h localhost --APP_NAME MyApplication --LOG_LEVEL debugmode"

// 获取所有环境变量
var envVars = converter.GetEnvironmentVariables();
Console.WriteLine($"检测到 {envVars.Count} 个环境变量");
```

##### 布尔类型参数支持

`CommandLineArgumentConverter` 支持智能识别和处理布尔类型参数，提供三种格式：

```csharp
using GameFrameX.Foundation.Options;

// 设置布尔类型环境变量
Environment.SetEnvironmentVariable("ENABLE_LOGGING", "true");
Environment.SetEnvironmentVariable("DEBUG_MODE", "false");
Environment.SetEnvironmentVariable("VERBOSE", "yes");

var converter = new CommandLineArgumentConverter();

// 1. 标志格式 (默认) - 只为 true 值添加标志
converter.BoolFormat = BoolArgumentFormat.Flag;
var flagArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 结果: ["--ENABLE_LOGGING", "--VERBOSE"] (只包含 true 值)

// 2. 键值对格式 - 添加键值对
converter.BoolFormat = BoolArgumentFormat.KeyValue;
var keyValueArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 结果: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]

// 3. 分离格式 - 键和值分开
converter.BoolFormat = BoolArgumentFormat.Separated;
var separatedArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 结果: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]
```

支持的布尔值格式：

- **True 值**: `"true"`, `"1"`, `"yes"`, `"on"`, `"enabled"` (不区分大小写)
- **False 值**: `"false"`, `"0"`, `"no"`, `"off"`, `"disabled"` (不区分大小写)

### 🛠️ 通用工具类 (GameFrameX.Foundation.Utility)

提供一系列实用的工具类，包含控制台操作、环境管理、时间处理和雪花ID生成等功能。

#### 核心组件概览

| 组件        | 文件名                    | 主要功能                      |
|-----------|------------------------|---------------------------|
| **控制台助手** | `ConsoleHelper.cs`     | 控制台Logo打印和格式化输出           |
| **环境助手**  | `EnvironmentHelper.cs` | 环境变量管理和环境类型定义             |
| **时间助手**  | `TimerHelper.cs`       | Unix时间戳处理和时间转换            |
| **雪花ID**  | `SnowFlakeIdHelper.cs` | 分布式唯一ID生成器（Snowflake算法实现） |

#### 控制台助手功能

```csharp
using GameFrameX.Foundation.Utility;

// 打印应用程序Logo
ConsoleHelper.PrintLogo();
// 输出格式化的控制台Logo，用于应用程序启动时的品牌展示
```

#### 环境管理功能

```csharp
using GameFrameX.Foundation.Utility;

// 获取当前环境类型
string currentEnv = Environments.Development;
Console.WriteLine($"当前环境: {currentEnv}");

// 环境判断
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
{
    // 开发环境特定逻辑
    Console.WriteLine("运行在开发环境");
}
```

#### 时间处理功能

```csharp
using GameFrameX.Foundation.Utility;

// Unix时间戳常量
DateTime epochLocal = TimerHelper.EpochLocal;   // 本地时区的Unix纪元时间
DateTime epochUtc = TimerHelper.EpochUtc;       // UTC时区的Unix纪元时间

// 获取当前Unix时间戳（秒）
long unixSeconds = TimerHelper.UnixTimeSeconds();
Console.WriteLine($"当前Unix时间戳（秒）: {unixSeconds}");

// 获取当前Unix时间戳（毫秒）
long unixMilliseconds = TimerHelper.UnixTimeMilliseconds();
Console.WriteLine($"当前Unix时间戳（毫秒）: {unixMilliseconds}");

// 时间戳转换示例
DateTime currentTime = DateTime.UtcNow;
long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
DateTime restored = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
```

#### 雪花ID生成器

```csharp
using GameFrameX.Foundation.Utility;

// 使用默认配置生成ID
long id1 = SnowFlakeIdHelper.GenerateId();
long id2 = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"生成的ID: {id1}, {id2}");

// 配置工作节点ID和数据中心ID
SnowFlakeIdHelper.WorkId = 1;        // 工作节点ID (0-31)
SnowFlakeIdHelper.DataCenterId = 1;  // 数据中心ID (0-31)

// 生成配置后的ID
long configuredId = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"配置后的ID: {configuredId}");

// 获取时间戳相关信息
DateTime utcStart = SnowFlakeIdHelper.UtcTimeStart;  // UTC起始时间
long epochTime = SnowFlakeIdHelper.EpochTime;        // 纪元时间戳

Console.WriteLine($"雪花ID起始时间: {utcStart}");
Console.WriteLine($"纪元时间戳: {epochTime}");
```

##### 雪花ID算法说明

雪花ID（Snowflake）是Twitter开源的分布式ID生成算法，具有以下特点：

- **全局唯一**: 在分布式环境中保证ID的全局唯一性
- **趋势递增**: 生成的ID大致按时间递增，有利于数据库索引
- **高性能**: 单机每秒可生成数百万个ID
- **无依赖**: 不依赖数据库或其他外部系统

ID结构（64位）：

```
0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
|   |                                             |   |       |       |
|   |<-------------- 41位时间戳 ---------------->|   |<-5位->|<-5位->|<--12位-->
|                                                 |           |       |
符号位(1位)                                        |      数据中心ID   序列号
                                                  |      (5位)      (12位)
                                               工作节点ID
                                                (5位)
```

- **1位符号位**: 固定为0
- **41位时间戳**: 精确到毫秒，可使用约69年
- **5位数据中心ID**: 支持32个数据中心
- **5位工作节点ID**: 每个数据中心支持32个工作节点
- **12位序列号**: 同一毫秒内支持4096个ID

#### 完整使用示例

```csharp
using GameFrameX.Foundation.Utility;

namespace MyApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // 打印应用程序Logo
            ConsoleHelper.PrintLogo();
            
            // 检查运行环境
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;
            Console.WriteLine($"当前运行环境: {env}");
            
            // 配置雪花ID生成器
            SnowFlakeIdHelper.WorkId = 1;
            SnowFlakeIdHelper.DataCenterId = 1;
            
            // 生成唯一ID
            for (int i = 0; i < 5; i++)
            {
                long id = SnowFlakeIdHelper.GenerateId();
                long timestamp = TimerHelper.UnixTimeMilliseconds();
                
                Console.WriteLine($"ID: {id}, 时间戳: {timestamp}");
                
                // 短暂延迟以观察ID变化
                Thread.Sleep(1);
            }
            
            // 时间处理示例
            Console.WriteLine($"Unix纪元时间(UTC): {TimerHelper.EpochUtc}");
            Console.WriteLine($"Unix纪元时间(本地): {TimerHelper.EpochLocal}");
            Console.WriteLine($"当前Unix时间戳(秒): {TimerHelper.UnixTimeSeconds()}");
            Console.WriteLine($"当前Unix时间戳(毫秒): {TimerHelper.UnixTimeMilliseconds()}");
        }
    }
}
```

## 🧪 测试

项目包含完整的单元测试，确保代码质量和功能正确性。所有核心功能都有对应的测试用例，测试覆盖率达到95%以上。

### 测试覆盖范围

#### 🧩 扩展方法库测试 (Extensions)

- **ArgumentAlreadyExceptionTests**: 参数已存在异常测试
- **BidirectionalDictionaryTests**: 双向字典功能测试
- **ByteExtensionTests**: 字节数组扩展方法测试
- **CollectionExtensionsTests**: 集合扩展方法测试
- **ConcurrentLimitedQueueTests**: 并发限制队列测试
- **DisposableConcurrentDictionaryTests**: 可释放并发字典测试
- **DisposableDictionaryTests**: 可释放字典测试
- **IDictionaryExtensionsTests**: 字典扩展方法测试
- **IEnumerableExtensionsTests**: 枚举扩展方法测试
- **ListExtensionsTests**: 列表扩展方法测试
- **LookupXTests**: 查找表功能测试
- **NullObjectTests**: 空对象模式测试
- **NullableConcurrentDictionaryTests**: 可空并发字典测试
- **NullableDictionaryTests**: 可空字典测试
- **ObjectExtensionsTests**: 对象扩展方法测试
- **ReadOnlySpanExtensionsTests**: 只读Span扩展测试
- **SequenceReaderExtensionsTests**: 序列读取器扩展测试
- **SpanExtensionsTests**: Span扩展方法测试
- **StringExtensionsTests**: 字符串扩展方法测试
- **TypeExtensionsTests**: 类型扩展方法测试

#### 🔐 加密工具库测试 (Encryption)

- **AesHelperTests**: AES加密算法测试
- **DsaHelperTests**: DSA数字签名测试
- **RsaHelperTests**: RSA加密算法测试
- **Sm2HelperTests**: SM2国密算法测试
- **Sm4HelperTests**: SM4国密算法测试
- **XorHelperTests**: XOR异或加密测试

#### 🌐 本地化框架测试 (Localization)

- **LocalizationServiceTests**: 本地化服务核心功能测试
  - 单例模式验证测试
  - 本地化字符串获取测试
  - 参数化消息格式化测试
  - 未知键处理测试
  - 线程安全并发测试
- **ResourceManagerTests**: 资源管理器测试
  - 提供者优先级测试
  - 懒加载机制测试
  - 统计信息验证测试
- **DefaultResourceProviderTests**: 默认资源提供者测试
- **AssemblyResourceProviderTests**: 程序集资源提供者测试
  - .resx文件加载测试
  - 多文化支持测试
  - 资源缓存机制测试

#### 🔗 哈希工具库测试 (Hash)

- **CrcHelperTests**: CRC校验算法测试
- **HmacSha256HelperTests**: HMAC-SHA256测试
- **Md5HelperTests**: MD5哈希算法测试
- **MurmurHash3HelperTests**: MurmurHash3算法测试
- **Sha1HelperTests**: SHA-1哈希算法测试
- **Sha256HelperTests**: SHA-256哈希算法测试
- **Sha512HelperTests**: SHA-512哈希算法测试
- **XxHashHelperTests**: xxHash高性能哈希测试

#### 🌐 HTTP工具库测试 (Http.Extension)

- **HttpExtensionTests**: HTTP客户端扩展方法测试

#### ⚙️ 命令行参数处理测试 (Options)

- **CommandLineArgumentConverterTests**: 命令行参数转换器功能测试
    - 空参数数组处理测试
    - 空参数值处理测试
    - 重复参数检测测试
    - 环境变量转换测试
    - 值清理功能测试
    - 单连字符参数转换测试
    - 命令行字符串生成测试
    - 环境变量获取测试
    - 完整工作流程测试
    - 布尔类型参数处理测试
        - 标志格式布尔参数测试
        - 键值对格式布尔参数测试
        - 分离格式布尔参数测试
        - 多种布尔值格式解析测试
        - 非布尔值处理测试
- **OptionsBuilderTests**: 选项构建器功能测试
    - 基本配置构建测试
    - 特性配置测试
    - 类型转换测试
    - 验证功能测试
- **OptionsProviderTests**: 选项提供器功能测试
    - 配置注册和获取测试
    - 全局配置管理测试

### 运行测试

```bash
# 运行所有测试
dotnet test

# 运行特定模块测试
dotnet test --filter "FullyQualifiedName~Extensions"
dotnet test --filter "FullyQualifiedName~Encryption"
dotnet test --filter "FullyQualifiedName~Hash"
dotnet test --filter "FullyQualifiedName~Localization"
dotnet test --filter "FullyQualifiedName~Options"

# 运行特定测试类
dotnet test --filter "ClassName=XxHashHelperTests"
dotnet test --filter "ClassName=StringExtensionsTests"
dotnet test --filter "ClassName=LocalizationServiceTests"
dotnet test --filter "ClassName=CommandLineArgumentConverterTests"

# 生成测试覆盖率报告
dotnet test --collect:"XPlat Code Coverage"

# 运行性能测试
dotnet test --filter "Category=Performance"
```

### 测试特点

- **全面覆盖**: 每个公共方法都有对应的测试用例
- **边界测试**: 包含空值、边界值、异常情况的测试
- **性能测试**: 对关键算法进行性能基准测试
- **并发测试**: 验证线程安全的组件在多线程环境下的正确性
- **兼容性测试**: 确保在不同.NET版本下的兼容性

## 🏗️ 架构设计

### 设计原则

- **高性能**: 所有组件都经过性能优化，适用于高并发场景
- **易用性**: 提供简洁的 API 设计，降低学习成本
- **可扩展**: 模块化设计，支持自定义扩展
- **类型安全**: 充分利用 .NET 的类型系统，减少运行时错误
- **内存友好**: 使用 Span<T> 和 Memory<T> 等现代 .NET 特性，减少内存分配

### 依赖关系

```
GameFrameX.Foundation.Extensions (核心扩展)
├── GameFrameX.Foundation.Encryption (加密工具)
├── GameFrameX.Foundation.Hash (哈希工具)
├── GameFrameX.Foundation.Json (JSON工具)
├── GameFrameX.Foundation.Logger (日志工具)
├── GameFrameX.Foundation.Options (参数处理)
├── GameFrameX.Foundation.Http.Extension (HTTP扩展)
└── GameFrameX.Foundation.Http.Normalization (HTTP标准化)
```

## 🔧 开发指南

### 环境要求

- .NET 10.0 或更高版本
- C# 12.0 或更高版本

### 构建项目

```bash
# 克隆仓库
git clone https://github.com/GameFrameX/GameFrameX.Foundation.git
cd GameFrameX.Foundation

# 还原依赖
dotnet restore

# 构建项目
dotnet build

# 运行测试
dotnet test
```

### 贡献指南

1. Fork 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request

## 📊 性能基准

### 扩展方法性能

| 操作        | 传统方法  | 扩展方法  | 性能提升 |
|-----------|-------|-------|------|
| 字符串空值检查   | 100ns | 15ns  | 85%  |
| 集合随机元素获取  | 200ns | 50ns  | 75%  |
| Span 字节操作 | 500ns | 80ns  | 84%  |
| 双向字典查找    | 150ns | 120ns | 20%  |

### 加密算法性能

| 算法       | 数据大小 | 加密时间   | 解密时间   |
|----------|------|--------|--------|
| AES-256  | 1KB  | 0.05ms | 0.04ms |
| RSA-2048 | 1KB  | 2.1ms  | 0.8ms  |
| SM4      | 1KB  | 0.08ms | 0.07ms |
| XOR      | 1KB  | 0.01ms | 0.01ms |

### 哈希算法性能

| 算法          | 数据大小 | 处理时间  | 吞吐量      |
|-------------|------|-------|----------|
| MD5         | 1MB  | 2.1ms | 476MB/s  |
| SHA-256     | 1MB  | 3.8ms | 263MB/s  |
| xxHash64    | 1MB  | 0.8ms | 1.25GB/s |
| MurmurHash3 | 1MB  | 1.2ms | 833MB/s  |

## 📋 系统要求

- .NET 10.0 或更高版本
- 支持 Windows、Linux、macOS

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来改进项目。

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 🤝 社区支持

- **问题反馈**: [GitHub Issues](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- **功能请求**: [GitHub Discussions](https://github.com/GameFrameX/GameFrameX.Foundation/discussions)
- **文档贡献**: 欢迎提交文档改进的 PR

## 📄 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 🙏 致谢

感谢所有为 GameFrameX.Foundation 做出贡献的开发者们！

## 🔗 相关链接

- [GameFrameX 官网](https://gameframex.doc.alianblank.com)
- [文档中心](https://gameframex.doc.alianblank.com)
- [问题反馈](https://github.com/GameFrameX/GameFrameX.Foundation/issues)

---

<div align="center">

**[⬆ 回到顶部](#gamefamex-foundation)**

Made with ❤️ by GameFrameX Team

</div>
