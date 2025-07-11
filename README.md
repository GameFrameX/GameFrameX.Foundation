# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0%2B-purple.svg)](https://dotnet.microsoft.com/)

### 📦 程序集概览

| 程序集                                      | 功能描述          | NuGet 包名                                   | 版本                                                                                                                                                                | 下载次数                                                                                                                                                               |
|------------------------------------------|---------------|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| GameFrameX.Foundation.Encryption         | 加密工具库         | `GameFrameX.Foundation.Encryption`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 |
| GameFrameX.Foundation.Extensions         | 扩展方法库         | `GameFrameX.Foundation.Extensions`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 |
| GameFrameX.Foundation.Hash               | 哈希工具库         | `GameFrameX.Foundation.Hash`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             |
| GameFrameX.Foundation.Http.Extension     | HttpClient 扩展 | `GameFrameX.Foundation.Http.Extension`     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         |
| GameFrameX.Foundation.Http.Normalization | HTTP 消息标准化    | `GameFrameX.Foundation.Http.Normalization` | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) |
| GameFrameX.Foundation.Json               | JSON 序列化工具    | `GameFrameX.Foundation.Json`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             |
| GameFrameX.Foundation.Logger             | Serilog 日志配置  | `GameFrameX.Foundation.Logger`             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         |

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

# 安装日志工具库
dotnet add package GameFrameX.Foundation.Logger

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
using GameFrameX.Foundation.Logger;

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

// SHA-256 哈希
string hash = Sha256Helper.ComputeHash("Hello World");

// JSON 序列化
string json = JsonHelper.Serialize(myObject);
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 日志记录
LogHandler.Create(LogOptions.Default);
LogHelper.Info("应用程序启动");
```

## 📚 详细文档

### 🧩 扩展方法库 (GameFrameX.Foundation.Extensions)

提供丰富的扩展方法集合，增强 .NET 基础类型的功能，提高开发效率和代码可读性。

#### 核心组件概览

| 组件           | 文件名                                                               | 主要功能                          |
|--------------|-------------------------------------------------------------------|-------------------------------|
| **集合扩展**     | `CollectionExtensions.cs`                                         | 为各种集合类型提供便捷操作方法               |
| **字符串扩展**    | `StringExtensions.cs`                                             | 增强字符串处理能力，包含URL安全Base64、居中对齐等 |
| **对象扩展**     | `ObjectExtensions.cs`                                             | 提供对象验证和数值范围检查                 |
| **类型扩展**     | `TypeExtensions.cs`                                               | 类型检查和反射相关扩展方法                 |
| **枚举扩展**     | `IEnumerableExtensions.cs`                                        | LINQ 增强和集合操作，支持交集、差集等         |
| **字典扩展**     | `IDictionaryExtensions.cs`                                        | 字典操作增强，支持合并、条件移除等             |
| **列表扩展**     | `ListExtensions.cs`                                               | 列表特定的扩展方法                     |
| **字节扩展**     | `ByteExtensions.cs`                                               | 字节数组操作，包含子数组提取等               |
| **Span扩展**   | `SpanExtensions.cs`                                               | 高性能内存操作，支持各种数据类型读写            |
| **只读Span扩展** | `ReadOnlySpanExtensions.cs`                                       | 只读内存的高性能读取操作                  |
| **序列读取器扩展**  | `SequenceReaderExtensions.cs`                                     | 序列数据的便捷读取方法                   |
| **双向字典**     | `BidirectionalDictionary.cs`                                      | 支持双向查找的字典实现                   |
| **查找表**      | `LookupX.cs`                                                      | 增强的一对多关系查找表                   |
| **并发队列**     | `ConcurrentLimitedQueue.cs`                                       | 线程安全的有限容量队列                   |
| **可空字典**     | `NullableDictionary.cs`<br/>`NullableConcurrentDictionary.cs`     | 支持空值的字典实现                     |
| **可释放字典**    | `DisposableDictionary.cs`<br/>`DisposableConcurrentDictionary.cs` | 值可被自动释放的字典                    |
| **常量定义**     | `ConstBaseTypeSize.cs`                                            | 基础数据类型字节大小常量                  |
| **空对象模式**    | `NullObject.cs`                                                   | 类型安全的空对象实现                    |
| **自定义异常**    | `ArgumentAlreadyException.cs`                                     | 参数已存在异常类型                     |

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
Span<byte> buffer = stackalloc byte[8];
int offset = 0;

// 写入各种数据类型
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);

// 读取数据类型
offset = 0;
uint value = buffer.ReadUIntValue(ref offset);
float floatValue = buffer.ReadFloatValue(ref offset);

// ReadOnlySpan 读取操作
ReadOnlySpan<byte> readBuffer = buffer;
offset = 0;
uint readValue = readBuffer.ReadUIntValue(ref offset);
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

### 📝 日志工具库 (GameFrameX.Foundation.Logger)

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

### 运行测试

```bash
# 运行所有测试
dotnet test

# 运行特定模块测试
dotnet test --filter "FullyQualifiedName~Extensions"
dotnet test --filter "FullyQualifiedName~Encryption"
dotnet test --filter "FullyQualifiedName~Hash"

# 运行特定测试类
dotnet test --filter "ClassName=XxHashHelperTests"
dotnet test --filter "ClassName=StringExtensionsTests"

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

## 📋 系统要求

- .NET 6.0 或更高版本
- 支持 Windows、Linux、macOS

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来改进项目。

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 📄 许可证

本项目采用 [MIT License](LICENSE) 许可证。

## 🔗 相关链接

- [GameFrameX 官网](https://gameframex.doc.alianblank.com)
- [文档中心](https://gameframex.doc.alianblank.com)
- [问题反馈](https://github.com/GameFrameX/GameFrameX.Foundation/issues)

---

**GameFrameX.Foundation** - 让开发更简单，让代码更优雅！

