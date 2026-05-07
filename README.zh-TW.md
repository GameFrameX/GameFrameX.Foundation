<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX.Foundation

[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation?label=version&color=green)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使**

[📖 文檔](https://gameframex.doc.alianblank.com) • [🚀 快速開始](#-快速開始)

---

🌐 **語言**: [English](README.md) | [簡體中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

---

</div>

### 📦 程序集概覽

| 程式集                                      | 功能描述          | NuGet 包名                                   | 版本                                                                                                                                                                | 下載次數                                                                                                                                                               |
|------------------------------------------|---------------|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| GameFrameX.Foundation.Encryption         | 加密工具庫         | `GameFrameX.Foundation.Encryption`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 |
| GameFrameX.Foundation.Extensions         | 擴展方法庫         | `GameFrameX.Foundation.Extensions`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 |
| GameFrameX.Foundation.Hash               | 哈希工具庫         | `GameFrameX.Foundation.Hash`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             |
| GameFrameX.Foundation.Http.Extension     | HttpClient 擴展 | `GameFrameX.Foundation.Http.Extension`     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         |
| GameFrameX.Foundation.Http.Normalization | HTTP 消息標準化    | `GameFrameX.Foundation.Http.Normalization` | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) |
| GameFrameX.Foundation.Json               | JSON 序列化工具    | `GameFrameX.Foundation.Json`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             |
| GameFrameX.Foundation.Localization       | 本地化框架         | `GameFrameX.Foundation.Localization`       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)             |
| GameFrameX.Foundation.Logger             | Serilog 日誌配置  | `GameFrameX.Foundation.Logger`             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         |
| GameFrameX.Foundation.Options            | 命令列參數處理       | `GameFrameX.Foundation.Options`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       |
| GameFrameX.Foundation.Orm.Attribute      | ORM 特性標註      | `GameFrameX.Foundation.Orm.Attribute`      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           |
| GameFrameX.Foundation.Orm.Entity         | ORM 實體基類      | `GameFrameX.Foundation.Orm.Entity`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 |
| GameFrameX.Foundation.Utility            | 通用工具類         | `GameFrameX.Foundation.Utility`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       |

GameFrameX 的基礎工具庫，提供了一系列高性能、易用的基礎組件和工具類，涵蓋加密、哈希、HTTP、JSON、日誌等常用功能。

## 🚀 快速開始

### 安裝

通過 NuGet 包管理器安裝所需的組件：

```bash
# 安裝加密工具庫
dotnet add package GameFrameX.Foundation.Encryption

# 安裝擴展方法庫
dotnet add package GameFrameX.Foundation.Extensions

# 安裝哈希工具庫
dotnet add package GameFrameX.Foundation.Hash

# 安裝 JSON 工具庫
dotnet add package GameFrameX.Foundation.Json

# 安裝本地化框架
dotnet add package GameFrameX.Foundation.Localization

# 安裝日誌工具庫
dotnet add package GameFrameX.Foundation.Logger

# 安裝命令行參數處理庫
dotnet add package GameFrameX.Foundation.Options

# 安裝 HTTP 擴展
dotnet add package GameFrameX.Foundation.Http.Extension

# 安裝 HTTP 消息標準化
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

// 擴展方法使用
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.RandomElement(); // 隨機獲取元素
var isNullOrEmpty = myString.IsNullOrEmpty(); // 字符串檢查

// 字符串擴展
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // URL安全Base64
string centered = "Hello".CenterAlignedText(20); // 居中對齊

// 對象驗證
object obj = GetSomeObject();
obj.ThrowIfNull(nameof(obj)); // 空值檢查
int value = 50;
value.CheckRange(1, 100); // 範圍檢查

// 高性能字節操作
Span<byte> buffer = stackalloc byte[8];
int offset = 0;
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);

// 雙向字典
var biDict = new BidirectionalDictionary<string, int>();
biDict.TryAdd("one", 1);
if (biDict.TryGetKey(1, out string key)) { /* 反向查找 */ }

// 命令行參數處理
var builder = new OptionsBuilder<AppConfig>(args);
var config = builder.Build();

// SHA-256 哈希
string hash = Sha256Helper.ComputeHash("Hello World");

// JSON 序列化
string json = JsonHelper.Serialize(myObject);
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 本地化字符串獲取
var successMessage = LocalizationService.GetString("Success");
var errorMessage = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
var formattedMessage = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);

// 日誌記錄
LogHandler.Create(LogOptions.Default);
LogHelper.Info("應用程序啟動");
```

## 📚 詳細文檔

### 🧩 擴展方法庫 (GameFrameX.Foundation.Extensions)

提供豐富的擴展方法集合，增強 .NET 基礎類型的功能，提高開發效率和代碼可讀性。

#### 核心組件概覽

| 組件           | 文件名                                                               | 主要功能                           |
|--------------|-------------------------------------------------------------------|--------------------------------|
| **集合擴展**     | `CollectionExtensions.cs`                                         | 為各種集合類型提供便捷操作方法                |
| **字符串擴展**    | `StringExtensions.cs`                                             | 增強字符串處理能力，包含URL安全Base64、居中對齊等  |
| **對象擴展**     | `ObjectExtensions.cs`                                             | 提供對象驗證和數值範圍檢查                  |
| **類型擴展**     | `TypeExtensions.cs`                                               | 類型檢查和反射相關擴展方法                  |
| **枚舉擴展**     | `IEnumerableExtensions.cs`                                        | LINQ 增強和集合操作，支持交集、差集等          |
| **字典擴展**     | `IDictionaryExtensions.cs`                                        | 字典操作增強，支持合併、條件移除等              |
| **列表擴展**     | `ListExtensions.cs`                                               | 列表特定的擴展方法                      |
| **字節擴展**     | `ByteExtensions.cs`                                               | 字節數組操作，包含子數組提取等                |
| **Span擴展**   | `SpanExtensions.cs`                                               | 高性能內存操作，支持各種數據類型讀寫，包含大端序和小端序支持 |
| **只讀Span擴展** | `ReadOnlySpanExtensions.cs`                                       | 只讀內存的高性能讀取操作                   |
| **序列讀取器擴展**  | `SequenceReaderExtensions.cs`                                     | 序列數據的便捷讀取方法                    |
| **雙向字典**     | `BidirectionalDictionary.cs`                                      | 支持雙向查找的字典實現                    |
| **查找表**      | `LookupX.cs`                                                      | 增強的一對多關係查找表                    |
| **併發隊列**     | `ConcurrentLimitedQueue.cs`                                       | 線程安全的有限容量隊列                    |
| **可空字典**     | `NullableDictionary.cs`<br/>`NullableConcurrentDictionary.cs`     | 支持空值的字典實現                      |
| **可釋放字典**    | `DisposableDictionary.cs`<br/>`DisposableConcurrentDictionary.cs` | 值可被自動釋放的字典                     |
| **常量定義**     | `ConstBaseTypeSize.cs`                                            | 基礎數據類型字節大小常量                   |
| **空對象模式**    | `NullObject.cs`                                                   | 類型安全的空對象實現                     |
| **自定義異常**    | `ArgumentAlreadyException.cs`                                     | 參數已存在異常類型                      |

#### 集合擴展功能

```csharp
using GameFrameX.Foundation.Extensions;

// 集合操作
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.RandomElement(); // 隨機獲取元素
var isEmpty = list.IsNullOrEmpty(); // 檢查是否為空

// 字典擴展
var dict = new Dictionary<string, int>();
dict.Merge("key", 10, (old, new) => old + new); // 合併值
var value = dict.GetOrAdd("key", k => 42); // 獲取或添加
dict.RemoveIf((k, v) => v > 100); // 條件移除

// HashSet 擴展
var hashSet = new HashSet<int>();
hashSet.AddRange(new[] { 1, 2, 3, 4, 5 }); // 批量添加
```

#### 字符串擴展功能

```csharp
// 字符串檢查
string text = "Hello World";
bool isEmpty = text.IsNullOrEmpty();
bool isEmptyOrWhitespace = text.IsNullOrEmptyOrWhiteSpace();
bool hasContent = text.IsNotNullOrEmptyOrWhiteSpace();

// 字符串處理
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // 轉換為 URL 安全格式
string restored = urlSafe.FromUrlSafeBase64(); // 還原標準格式

// 字符串操作
string centered = "Hello".CenterAlignedText(20); // 居中對齊
string cleaned = "Hello World   ".RemoveWhiteSpace(); // 移除空白字符
string trimmed = "Hello!".RemoveSuffix('!'); // 移除後綴

// 字符重複
string repeated = 'A'.RepeatChar(5); // "AAAAA"
```

#### 對象驗證和範圍檢查

```csharp
// 空值檢查
object obj = GetSomeObject();
if (obj.IsNotNull())
{
    // 對象不為空時的處理
}

// 參數驗證
obj.ThrowIfNull(nameof(obj)); // 為空時拋出異常

// 數值範圍檢查
int value = 50;
value.CheckRange(1, 100); // 檢查範圍，超出時拋出異常
bool inRange = value.IsRange(1, 100); // 檢查是否在範圍內

// 支持多種數值類型
uint uintValue = 25;
uintValue.CheckRange(0, 50);

long longValue = 1000;
longValue.CheckRange(500, 2000);
```

#### 類型檢查擴展

```csharp
// 泛型接口檢查
Type listType = typeof(List<string>);
Type genericListType = typeof(List<>);
bool implementsGeneric = listType.HasImplementedRawGeneric(genericListType);

// 接口實現檢查
Type stringType = typeof(string);
Type comparableType = typeof(IComparable);
bool implementsInterface = stringType.IsImplWithInterface(comparableType);
```

#### LINQ 增強擴展

```csharp
// 交集操作
var list1 = new[] { 1, 2, 3, 4, 5 };
var list2 = new[] { 3, 4, 5, 6, 7 };
var intersection = list1.IntersectBy(list2, x => x); // 按鍵取交集

// 多集合交集
var collections = new[] { list1, list2, new[] { 4, 5, 6 } };
var allIntersection = collections.IntersectAll(); // 所有集合的交集

// 差集操作
var difference = list1.ExceptBy(list2, (x, y) => x == y);

// 批量添加
var collection = new List<int>();
collection.AddRange(1, 2, 3, 4, 5); // 使用 params 參數
collection.AddRange(new[] { 6, 7, 8 }); // 使用數組
```

#### 雙向字典

```csharp
// 創建雙向字典
var biDict = new BidirectionalDictionary<string, int>();

// 添加鍵值對
biDict.TryAdd("one", 1);
biDict.TryAdd("two", 2);

// 雙向查找
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

#### 高性能擴展

```csharp
// Span 和 ReadOnlySpan 擴展
ReadOnlySpan<byte> span = stackalloc byte[] { 1, 2, 3, 4, 5 };
// 提供針對 Span 的高性能操作擴展

// 序列讀取器擴展
// 為 SequenceReader 提供便捷的讀取方法
```

#### 字節操作擴展

```csharp
// 字節數組擴展
byte[] data = { 1, 2, 3, 4, 5 };
byte[] subArray = data.SubArray(1, 3); // 獲取子數組

// Span 和 ReadOnlySpan 擴展 - 高性能字節操作
Span<byte> buffer = stackalloc byte[16];
int offset = 0;

// 寫入各種數據類型（支持大端序和小端序）
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);
buffer.WriteUIntBigEndianValue(12345u, ref offset); // 大端序寫入
buffer.WriteFloatBigEndianValue(3.14f, ref offset); // 大端序寫入

// 讀取數據類型
offset = 0;
uint value = buffer.ReadUIntValue(ref offset);
float floatValue = buffer.ReadFloatValue(ref offset);
uint bigEndianValue = buffer.ReadUIntBigEndianValue(ref offset); // 大端序讀取

// ReadOnlySpan 讀取操作
ReadOnlySpan<byte> readBuffer = buffer;
offset = 0;
uint readValue = readBuffer.ReadUIntValue(ref offset);
float readFloatValue = readBuffer.ReadFloatBigEndianValue(ref offset);
```

#### 序列讀取器擴展

```csharp
// 為 SequenceReader 提供便捷的讀取方法
// 支持帶長度前綴的字節數組讀取
// 提供 TryPeek 方法進行非破壞性讀取
```

#### 特殊工具類

- **ConstBaseTypeSize**: 基礎數據類型字節大小常量定義，包含所有.NET基礎類型的字節大小
- **NullObject**: 空對象模式實現，提供類型安全的空對象
- **NullableConcurrentDictionary**: 支持空值的線程安全併發字典
- **NullableDictionary**: 支持空值的普通字典
- **LookupX**: 增強的查找表實現，支持一對多關係映射
- **ArgumentAlreadyException**: 參數已存在異常，用於參數驗證場景
- **ConcurrentLimitedQueue**: 線程安全的有限容量隊列，自動移除最舊元素
- **DisposableConcurrentDictionary/DisposableDictionary**: 值可被自動釋放的字典類型

### 🔐 加密工具庫 (GameFrameX.Foundation.Encryption)

提供多種加密算法的實現，確保數據安全傳輸和存儲。

#### 支持的算法

- **AES 加密** (`AesHelper`): 對稱加密算法，支持字符串和字節數組
- **RSA 加密** (`RsaHelper`): 非對稱加密算法，支持密鑰對生成、加密解密、數字簽名
- **DSA 簽名** (`DsaHelper`): 數字簽名算法，支持簽名和驗證
- **SM2/SM4 加密** (`Sm2Helper`/`Sm4Helper`): 國密算法實現
    - SM2: 非對稱加密算法
    - SM4: 對稱加密算法，支持 ECB/CBC 模式
- **XOR 加密** (`XorHelper`): 異或加密，支持快速加密和完整加密模式

#### 使用示例

```csharp
// AES 加密
string encrypted = AesHelper.Encrypt("敏感數據", "your-secret-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-secret-key");

// RSA 加密
var keys = RsaHelper.Make();
string encrypted = RsaHelper.Encrypt(keys["publicKey"], "Hello World");
string decrypted = RsaHelper.Decrypt(keys["privateKey"], encrypted);

// SM4 加密
string encrypted = Sm4Helper.EncryptCbc("your-key", "Hello World");
string decrypted = Sm4Helper.DecryptCbc("your-key", encrypted);
```

### 🔗 哈希工具庫 (GameFrameX.Foundation.Hash)

提供多種哈希算法實現，適用於數據完整性校驗、快速查找等場景。

#### 支持的算法

- **MD5** (`Md5Helper`): 128位哈希值，支持加鹽
- **SHA 系列**:
    - SHA-1 (`Sha1Helper`): 160位哈希值
    - SHA-256 (`Sha256Helper`): 256位哈希值
    - SHA-512 (`Sha512Helper`): 512位哈希值
- **HMAC-SHA256** (`HmacSha256Helper`): 基於密鑰的消息認證碼
- **CRC 校驗** (`CrcHelper`): CRC32/CRC64 循環冗餘校驗
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

### 🌐 HTTP 工具庫

#### HTTP 擴展 (GameFrameX.Foundation.Http.Extension)

為 HttpClient 提供便捷的擴展方法，簡化 JSON 數據的發送和接收。

```csharp
// POST JSON 請求
string response = await httpClient.PostJsonToStringAsync<MyClass>(url, myObject);
```

#### HTTP 消息標準化 (GameFrameX.Foundation.Http.Normalization)

提供統一的 HTTP 響應格式，包含 `code`、`message` 和 `data` 字段，適用於 GameFrameX 生態系統。

### 📄 JSON 序列化 (GameFrameX.Foundation.Json)

基於 `System.Text.Json` 的高性能序列化工具，提供優化的預設配置。

#### 特性

- 高性能序列化/反序列化
- 枚舉序列化為字符串
- 忽略 null 值屬性
- 忽略循環引用
- 屬性名稱大小寫不敏感
- 提供格式化和緊湊兩種輸出模式

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
    // 處理結果
}
```

### 🌐 本地化框架 (GameFrameX.Foundation.Localization)

提供輕量級、高性能的本地化解決方案，支持零配置使用和懶加載機制，為整個 GameFrameX.Foundation 生態系統提供統一的本地化支持。

#### 主要特性

- **零配置使用**: 無需任何初始化配置，自動發現和加載本地化資源
- **懶加載機制**: 首次使用時才加載資源，啟動性能優異
- **多語言支持**: 內置中文（簡體）和英文支持，可擴展更多語言
- **線程安全**: 支持併發訪問，適用於多線程環境
- **高度可擴展**: 支持自定義資源提供者，靈活的優先級管理
- **優先級解析**: 自定義提供者 > 程序集資源 > 預設資源

#### 核心組件

| 組件         | 文件名                           | 功能                  |
|------------|-------------------------------|---------------------|
| **本地化服務**  | `LocalizationService.cs`      | 統一的本地化入口點，提供靜態方法API |
| **資源管理器**  | `ResourceManager.cs`          | 管理多個資源提供者，實現優先級解析   |
| **預設提供者**  | `DefaultResourceProvider.cs`  | 提供英文預設消息，包含50+常用消息  |
| **程式集提供者** | `AssemblyResourceProvider.cs` | 從.resx文件加載本地化資源     |

#### 基礎使用

```csharp
using GameFrameX.Foundation.Localization.Core;

// 獲取簡單的本地化字符串
var successMessage = LocalizationService.GetString("Success");
Console.WriteLine(successMessage); // 根據當前文化顯示 "Success" 或 "成功"

// 帶參數的格式化消息
var errorMessage = LocalizationService.GetString("ArgumentNull", "username");
Console.WriteLine(errorMessage); // "Value cannot be null. (Parameter 'username')"

// 如果鍵不存在，返回鍵名本身
var unknown = LocalizationService.GetString("Some.Unknown.Key");
Console.WriteLine(unknown); // 輸出: "Some.Unknown.Key"
```

#### 異常處理中的本地化

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

        // 其他驗證邏輯...
    }
}
```

#### 模塊集成本地化

##### 1. 定義本地化鍵

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

##### 2. 創建資源文件

在項目中創建 `Localization/Messages/Resources.resx` 和 `Localization/Messages/Resources.zh-CN.resx`：

```xml
<!-- Resources.resx (默認英文) -->
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
    <value>郵箱地址是必填項</value>
  </data>
  <data name="YourModule.Messages.UserCreated" xml:space="preserve">
    <value>用戶 '{0}' 已成功創建</value>
  </data>
</root>
```

##### 3. 在業務邏輯中使用

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

        // 創建用戶邏輯...

        var successMessage = LocalizationService.GetString(
            LocalizationKeys.Messages.UserCreated, userDto.Username);
        Console.WriteLine(successMessage);
    }
}
```

#### 自定義資源提供者

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

// 註冊自定義提供者（具有最高優先級）
var dbProvider = new DatabaseResourceProvider(yourDbConnection);
LocalizationService.RegisterProvider(dbProvider);
```

#### 預加載和性能優化

```csharp
// 應用啟動時預加載所有本地化資源（可選）
LocalizationService.EnsureLoaded();

// 獲取本地化系統統計信息
var stats = LocalizationService.GetStatistics();
Console.WriteLine($"提供者已加載: {stats.ProvidersLoaded}");
Console.WriteLine($"總提供者數量: {stats.TotalProviderCount}");
Console.WriteLine($"程序集提供者數量: {stats.AssemblyProviderCount}");

// 獲取所有提供者信息
var providers = LocalizationService.GetProviders();
foreach (var provider in providers)
{
    Console.WriteLine($"提供者: {provider.GetType().Name}");
}
```

#### 資源命名約定

- **模式**: `{模塊名}.{類別}.{具體鍵名}`
- **示例**:
    - `Utility.Exceptions.TimestampOutOfRange`
    - `Encryption.InvalidKeySize`
    - `Authentication.UserNotFound`
    - `Success`
    - `ArgumentNull`

#### 已集成的模塊

目前以下模塊已完成本地化集成：

| 模塊                               | 本地化鍵數量 | 狀態   |
|----------------------------------|--------|------|
| GameFrameX.Foundation.Utility    | 4      | ✅ 完成 |
| GameFrameX.Foundation.Encryption | 20+    | ✅ 完成 |
| GameFrameX.Foundation.Extensions | 7      | ✅ 完成 |
| GameFrameX.Foundation.Hash       | 2      | ✅ 完成 |

#### 高級功能

##### 動態語言切換

```csharp
public void SwitchLanguage(string cultureCode)
{
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

    // 可選：預加載新語言的資源
    LocalizationService.EnsureLoaded();
}
```

##### 監控和診斷

```csharp
public class LocalizationDiagnostics
{
    public void PrintStatus()
    {
        var stats = LocalizationService.GetStatistics();
        Console.WriteLine("=== 本地化系統狀態 ===");
        Console.WriteLine($"提供者已加載: {stats.ProvidersLoaded}");
        Console.WriteLine($"總提供者數量: {stats.TotalProviderCount}");

        var providers = LocalizationService.GetProviders();
        foreach (var provider in providers)
        {
            Console.WriteLine($"- {provider.GetType().Name}");
        }
    }
}
```

#### 最佳實踐

1. **鍵命名規範**: 使用 `{模塊名}.{類別}.{具體鍵名}` 的命名模式
2. **參數化消息**: 使用 `string.Format` 格式支持參數替換
3. **異常處理**: 在異常消息中集成本地化支持
4. **性能優化**: 應用啟動時可選擇預加載資源
5. **測試驗證**: 為本地化功能編寫單元測試

#### 配置項目文件

確保項目文件包含本地化資源文件：

```xml
<PropertyGroup>
  <EnableDefaultEmbeddedResourceItems>false</EnableDefaultEmbeddedResourceItems>
</PropertyGroup>

<ItemGroup>
  <EmbeddedResource Include="Localization\Messages\*.resx" />
</ItemGroup>
```

更多詳細信息請參考：

- [本地化框架完整文檔](GameFrameX.Foundation.Localization/README.Localization.md)
- [使用示例和最佳實踐](GameFrameX.Foundation.Localization/USAGE_EXAMPLES.md)

### �️ ORM 實體基類 (GameFrameX.Foundation.Orm.Entity)

提供ORM框架的實體基類和接口定義，支持審計跟蹤、軟刪除、樂觀鎖等企業級功能。

#### 核心組件概覽

| 組件           | 文件名                   | 主要功能                          |
|--------------|-----------------------|-------------------------------|
| **實體基類**     | `EntityBase.cs`       | 完整功能的實體基類，包含ID、審計、軟刪除、版本控制等功能 |
| **實體基類(泛型)** | `EntityBaseId.cs`     | 支持自定義主鍵類型的實體基類                |
| **實體接口**     | `IEntity.cs`          | 基礎實體接口定義，提供ID屬性               |
| **審計接口**     | `IAuditableEntity.cs` | 審計功能接口，定義創建時間、更新時間、操作用戶等審計字段  |

#### 實體基類功能

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 繼承EntityBase的實體類自動獲得完整的企業級功能
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // 以下屬性由EntityBase提供：
    // - long Id                    // 主鍵ID
    // - DateTime CreateTime        // 創建時間
    // - DateTime UpdateTime        // 更新時間
    // - long CreateUserId          // 創建用戶ID
    // - long UpdateUserId          // 更新用戶ID
    // - string CreateUserName      // 創建用戶名
    // - string UpdateUserName      // 更新用戶名
    // - bool IsDelete              // 軟刪除標記
    // - long Version               // 樂觀鎖版本號
    // - bool IsEnabled             // 啟用狀態
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

#### 自定義主鍵類型

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 使用字符串作為主鍵
public class Product : EntityBaseId<string>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    // Id屬性類型為string，由EntityBaseId<string>提供
}

// 使用Guid作為主鍵
public class Order : EntityBaseId<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Id屬性類型為Guid，由EntityBaseId<Guid>提供
}

// 使用示例
var product = new Product
{
    Id = "PROD-001",
    Name = "筆記本電腦",
    Price = 5999.99m,
    Description = "高性能筆記本電腦"
};

var order = new Order
{
    Id = Guid.NewGuid(),
    OrderNumber = "ORD-20240101-001",
    TotalAmount = 5999.99m,
    OrderDate = DateTime.UtcNow
};
```

#### 接口實現

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 實現基礎實體接口
public class Category : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// 實現審計接口
public class AuditableCategory : IEntity<int>, IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // IAuditableEntity接口要求的屬性
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public long CreateUserId { get; set; }
    public long UpdateUserId { get; set; }
    public string CreateUserName { get; set; }
    public string UpdateUserName { get; set; }
}
```

#### 企業級功能詳解

##### 1. 審計跟蹤 (Audit Trail)

```csharp
// EntityBase自動提供審計字段
public class Document : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
}

// 在業務邏輯中設置審計信息
var document = new Document
{
    Title = "重要文檔",
    Content = "文檔內容...",
    CreateTime = DateTime.UtcNow,
    CreateUserId = currentUser.Id,
    CreateUserName = currentUser.Username,
    UpdateTime = DateTime.UtcNow,
    UpdateUserId = currentUser.Id,
    UpdateUserName = currentUser.Username
};

// 更新時自動維護審計信息
document.Content = "更新後的內容";
document.UpdateTime = DateTime.UtcNow;
document.UpdateUserId = currentUser.Id;
document.UpdateUserName = currentUser.Username;
document.Version++; // 樂觀鎖版本遞增
```

##### 2. 軟刪除 (Soft Delete)

```csharp
// 軟刪除：不真正刪除記錄，而是標記為已刪除
public void SoftDeleteUser(User user)
{
    user.IsDelete = true;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    // 保存到數據庫，記錄仍然存在但被標記為已刪除
    dbContext.SaveChanges();
}

// 查詢時過濾已刪除的記錄
var activeUsers = dbContext.Users
    .Where(u => !u.IsDelete)
    .ToList();

// 恢復已刪除的記錄
public void RestoreUser(User user)
{
    user.IsDelete = false;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    dbContext.SaveChanges();
}
```

##### 3. 樂觀鎖 (Optimistic Locking)

```csharp
// 使用Version字段實現樂觀鎖
public void UpdateUserWithOptimisticLock(long userId, string newEmail)
{
    var user = dbContext.Users.Find(userId);
    if (user == null) throw new EntityNotFoundException();
    
    var originalVersion = user.Version;
    
    // 修改數據
    user.Email = newEmail;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    user.Version++; // 版本號遞增
    
    try
    {
        // 保存時檢查版本號
        var rowsAffected = dbContext.Database.ExecuteSqlRaw(
            "UPDATE Users SET Email = {0}, UpdateTime = {1}, UpdateUserId = {2}, UpdateUserName = {3}, Version = {4} " +
            "WHERE Id = {5} AND Version = {6}",
            user.Email, user.UpdateTime, user.UpdateUserId, user.UpdateUserName, user.Version, user.Id, originalVersion);
            
        if (rowsAffected == 0)
        {
            throw new ConcurrencyException("數據已被其他用戶修改，請刷新後重試");
        }
    }
    catch (DbUpdateConcurrencyException)
    {
        throw new ConcurrencyException("併發衝突，請刷新後重試");
    }
}
```

##### 4. 啟用狀態管理

```csharp
// 使用IsEnabled字段管理實體的啟用狀態
public class Feature : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    // IsEnabled由EntityBase提供
}

// 啟用/禁用功能
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

// 查詢啟用的功能
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
    // 用戶實體
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        
        // 導航屬性
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    
    // 訂單實體（使用Guid主鍵）
    public class Order : EntityBaseId<Guid>
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        // 導航屬性
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 訂單項實體
    public class OrderItem : EntityBase
    {
        public Guid OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        // 導航屬性
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
    
    // 產品實體（使用字符串主鍵）
    public class Product : EntityBaseId<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; }
        
        // 導航屬性
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 分類實體（實現接口）
    public class Category : IEntity<string>, IAuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        
        // IAuditableEntity接口屬性
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        
        // 導航屬性
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

// 業務服務示例
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
                throw new EntityNotFoundException($"用戶 {userId} 不存在");
            
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
                throw new ConcurrencyException("用戶信息已被其他用戶修改，請刷新後重試");
            }
        }
        
        public async Task SoftDeleteUserAsync(long userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && !u.IsDelete)
                .FirstOrDefaultAsync();
                
            if (user == null)
                throw new EntityNotFoundException($"用戶 {userId} 不存在");
            
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
            // 實現密碼哈希邏輯
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

### 🏷️ ORM 屬性標記 (GameFrameX.Foundation.Orm.Attribute)

提供ORM框架的屬性標記，用於標識實體類的特殊功能，如審計跟蹤、緩存策略、軟刪除和版本控制等。

#### 核心組件概覽

| 組件           | 文件名                    | 主要功能                                    |
|--------------|------------------------|-----------------------------------------|
| **審計表屬性**    | `AuditTableAttribute.cs` | 標記實體類支持審計跟蹤功能，記錄數據變更歷史                  |
| **緩存表屬性**    | `CacheTableAttribute.cs` | 標記實體類支持緩存策略，提升數據訪問性能                    |
| **軟刪除屬性**    | `SoftDeleteAttribute.cs` | 標記實體類支持軟刪除功能，邏輯刪除而非物理刪除                 |
| **版本控制屬性**   | `VersionControlAttribute.cs` | 標記實體類支持數據版本管理，實現樂觀鎖和併發控制               |

#### 審計表屬性 (AuditTableAttribute)

用於標記需要進行審計跟蹤的實體類，系統會自動記錄數據的創建、修改、刪除等操作歷史。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 標記用戶表需要審計跟蹤
[AuditTable]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase已包含審計字段：
    // CreateTime, UpdateTime, CreateUserId, UpdateUserId, 
    // CreateUserName, UpdateUserName
}

// 標記訂單表需要審計跟蹤
[AuditTable]
public class Order : EntityBase
{
    public string OrderNumber { get; set; }
    public long UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}

// 審計攔截器示例
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

#### 緩存表屬性 (CacheTableAttribute)

用於標記支持緩存策略的實體類，系統會自動對這些表的數據進行緩存管理。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 標記配置表支持緩存（配置數據變化頻率低，適合緩存）
[CacheTable]
public class SystemConfig : EntityBase
{
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

// 標記字典表支持緩存（字典數據相對穩定，適合緩存）
[CacheTable]
public class Dictionary : EntityBase
{
    public string DictType { get; set; }
    public string DictKey { get; set; }
    public string DictValue { get; set; }
    public string Description { get; set; }
    public int SortOrder { get; set; }
}

// 標記權限表支持緩存（權限數據訪問頻繁但變化不頻繁）
[CacheTable]
public class Permission : EntityBase
{
    public string PermissionCode { get; set; }
    public string PermissionName { get; set; }
    public string Description { get; set; }
    public string Module { get; set; }
}

// 緩存服務示例
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
            // 不支持緩存，直接從數據庫查詢
            return await _dbContext.Set<T>().ToListAsync();
        }
        
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<T> cachedData))
        {
            _logger.LogDebug($"從緩存獲取數據: {cacheKey}");
            return cachedData;
        }
        
        // 從數據庫查詢並緩存
        var data = await _dbContext.Set<T>().ToListAsync();
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), // 30分鐘過期
            SlidingExpiration = TimeSpan.FromMinutes(5), // 5分鐘滑動過期
            Priority = CacheItemPriority.Normal
        };
        
        _memoryCache.Set(cacheKey, data, cacheOptions);
        _logger.LogDebug($"數據已緩存: {cacheKey}, 記錄數: {data.Count}");
        
        return data;
    }
    
    public async Task InvalidateCacheAsync()
    {
        var entityType = typeof(T);
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        _memoryCache.Remove(cacheKey);
        _logger.LogDebug($"緩存已失效: {cacheKey}");
    }
}

// 緩存管理器示例
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
                    
                    _logger.LogInformation($"緩存表 {type.Name} 已刷新");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"刷新緩存表 {type.Name} 時發生錯誤");
            }
        }
    }
}
```

#### 軟刪除屬性 (SoftDeleteAttribute)

用於標記支持軟刪除功能的實體類，刪除操作會將記錄標記為已刪除而不是物理刪除。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 標記用戶表支持軟刪除
[SoftDelete]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase已包含IsDelete字段
}

// 標記文章表支持軟刪除
[SoftDelete]
public class Article : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
    public long AuthorId { get; set; }
    public DateTime PublishTime { get; set; }
}

// 軟刪除攔截器
public class SoftDeleteInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 處理軟刪除實體的刪除操作
        var softDeleteEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && 
                       e.Entity.GetType().GetCustomAttribute<SoftDeleteAttribute>() != null)
            .ToList();
            
        foreach (var entry in softDeleteEntries)
        {
            // 將刪除操作轉換為更新操作
            entry.State = EntityState.Modified;
            
            if (entry.Entity is EntityBase entityBase)
            {
                entityBase.IsDelete = true;
                entityBase.UpdateTime = DateTime.UtcNow;
                // 設置更新用戶信息...
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 軟刪除查詢過濾器
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
        // 返回包含已刪除記錄的查詢
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
        
        return query.Where(_ => false); // 如果不支持軟刪除，返回空結果
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
    
    // 獲取活躍用戶（自動過濾已刪除）
    public async Task<List<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .WhereNotDeleted()
            .ToListAsync();
    }
    
    // 獲取已刪除用戶
    public async Task<List<User>> GetDeletedUsersAsync()
    {
        return await _context.Users
            .OnlyDeleted()
            .ToListAsync();
    }
    
    // 獲取所有用戶（包含已刪除）
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .IncludeDeleted()
            .ToListAsync();
    }
    
    // 軟刪除用戶
    public async Task SoftDeleteUserAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user); // 會被攔截器轉換為軟刪除
            await _context.SaveChangesAsync();
        }
    }
    
    // 恢復已刪除用戶
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

#### 版本控制屬性 (VersionControlAttribute)

用於標記支持數據版本管理的實體類，實現樂觀鎖和併發控制功能。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 標記用戶表支持版本控制
[VersionControl]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase已包含Version字段
}

// 標記庫存表支持版本控制（防止超賣）
[VersionControl]
public class Inventory : EntityBase
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public decimal UnitCost { get; set; }
}

// 標記賬戶餘額表支持版本控制（防止併發操作導致餘額錯誤）
[VersionControl]
public class AccountBalance : EntityBase
{
    public long UserId { get; set; }
    public decimal Balance { get; set; }
    public decimal FrozenAmount { get; set; }
    public string Currency { get; set; }
}

// 版本控制攔截器
public class VersionControlInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 處理版本控制實體的更新操作
        var versionControlEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified && 
                       e.Entity.GetType().GetCustomAttribute<VersionControlAttribute>() != null)
            .ToList();
            
        foreach (var entry in versionControlEntries)
        {
            if (entry.Entity is EntityBase entityBase)
            {
                // 自動遞增版本號
                entityBase.Version++;
                
                // 標記Version字段為已修改
                entry.Property(nameof(EntityBase.Version)).IsModified = true;
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 版本控制服務
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
            throw new InvalidOperationException($"實體類型 {entityType.Name} 未標記 VersionControlAttribute");
        }
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new EntityNotFoundException($"實體 {entityType.Name} (ID: {id}) 不存在");
                }
                
                var originalVersion = entity.Version;
                
                // 執行更新操作
                updateAction(entity);
                
                // 設置更新時間
                entity.UpdateTime = DateTime.UtcNow;
                
                // 保存更改
                await _dbContext.SaveChangesAsync();
                
                _logger.LogDebug($"實體 {entityType.Name} (ID: {id}) 更新成功，版本從 {originalVersion} 更新到 {entity.Version}");
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning($"實體 {entityType.Name} (ID: {id}) 版本衝突，第 {attempt} 次重試");
                
                if (attempt == maxRetries)
                {
                    throw new ConcurrencyException($"實體 {entityType.Name} (ID: {id}) 在 {maxRetries} 次重試後仍然存在版本衝突", ex);
                }
                
                // 重新加載實體以獲取最新版本
                _dbContext.Entry(await _dbContext.Set<T>().FindAsync(id)).Reload();
                
                // 等待一段時間後重試
                await Task.Delay(TimeSpan.FromMilliseconds(100 * attempt));
            }
        }
        
        throw new InvalidOperationException("不應該到達這裡");
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
    
    // 減少庫存（防止超賣）
    public async Task<bool> ReduceInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"產品 {productId} 的庫存記錄不存在");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
            {
                if (inv.Quantity < quantity)
                {
                    throw new InsufficientInventoryException($"庫存不足，當前庫存: {inv.Quantity}，需要: {quantity}");
                }
                
                inv.Quantity -= quantity;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            // 版本衝突，可能是併發操作導致
            throw new ConcurrencyException("庫存更新失敗，請重試");
        }
    }
    
    // 增加庫存
    public async Task AddInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"產品 {productId} 的庫存記錄不存在");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
        {
            inv.Quantity += quantity;
        });
    }
}

// 賬戶餘額服務示例
public class AccountBalanceService
{
    private readonly VersionControlService<AccountBalance> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public AccountBalanceService(VersionControlService<AccountBalance> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 扣減餘額
    public async Task<bool> DeductBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"用戶 {userId} 的 {currency} 賬戶不存在");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
            {
                if (bal.Balance < amount)
                {
                    throw new InsufficientBalanceException($"餘額不足，當前餘額: {bal.Balance}，需要: {amount}");
                }
                
                bal.Balance -= amount;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            throw new ConcurrencyException("餘額更新失敗，請重試");
        }
    }
    
    // 增加餘額
    public async Task AddBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"用戶 {userId} 的 {currency} 賬戶不存在");
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
    // 用戶實體：支持審計、軟刪除、版本控制
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
    
    // 系統配置：支持緩存、審計
    [CacheTable]
    [AuditTable]
    public class SystemConfig : EntityBase
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
    
    // 庫存記錄：支持版本控制、審計
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
    
    // 訂單記錄：支持審計、軟刪除
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
        // 為所有標記了SoftDeleteAttribute的實體添加全局查詢過濾器
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

// 服務註冊
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

### 🖊️ 日誌工具庫 (GameFrameX.Foundation.Logger)

基於 Serilog 的日誌配置工具，提供簡單易用的日誌記錄功能。

#### 特性

- 支持多種日誌級別 (Debug, Info, Warning, Error, Fatal)
- 靈活的輸出配置
- 支持自定義日誌提供程序
- 提供日誌自我診斷
- ✅ **預初始化日誌支持**: 無需手動初始化，直接使用 LogHelper 即可輸出日誌
- ✅ **日誌自動合併**: 初始化前後的日誌會自動合併到正式日誌系統

#### 預初始化日誌功能

在正式日誌系統初始化之前，可以直接使用 LogHelper 輸出日誌到控制台。當調用 `LogHandler.Create()` 初始化正式日誌後，之前的臨時日誌會自動合併到新日誌系統中，確保日誌不丟失。

```csharp
class Program
{
    static void Main(string[] args)
    {
        // 無需任何初始化，直接使用 LogHelper
        LogHelper.Info("正在加載配置...");
        LogHelper.Debug("參數: {Args}", string.Join(", ", args));
        LogHelper.Warning("配置不存在，使用預設值");

        // 初始化正式日誌系統
        var logger = LogHandler.Create(options);

        // 之前的臨時日誌已自動合併到新日誌
        LogHelper.Info("系統啟動完成");
    }
}
```

#### 使用示例

```csharp
// 初始化日誌
LogHandler.Create(LogOptions.Default);

// 記錄日誌
LogHelper.Debug("調試信息");
LogHelper.Info("普通信息");
LogHelper.Warning("警告信息");
LogHelper.Error("錯誤信息");
LogHelper.Fatal("致命錯誤");
```

### ⚙️ 命令行參數處理 (GameFrameX.Foundation.Options)

一個強大的命令行參數和環境變量解析庫，支持將命令行參數和環境變量自動映射到強類型配置對象。

#### 特性

- ✅ **參數優先級處理**: 命令行參數 > 環境變量 > 默認值
- ✅ **泛型支持**: 支持任意強類型配置類
- ✅ **多種啟動方式兼容**: 支持Docker、exe、shell等啟動方式
- ✅ **自動前綴處理**: 自動為參數添加`--`前綴
- ✅ **布爾參數支持**: 支持多種布爾參數格式
- ✅ **環境變量映射**: 自動映射環境變量到配置屬性
- ✅ **類型轉換**: 自動轉換字符串參數到目標類型
- ✅ **特性支持**: 支持豐富的配置特性

#### 核心組件

| 組件                             | 功能描述                 |
|--------------------------------|----------------------|
| `CommandLineArgumentConverter` | 命令行參數轉換器，提供參數處理的核心功能 |
| `OptionsBuilder<T>`            | 配置構建器，用於構建泛型配置對象     |
| `OptionsProvider`              | 配置提供器，用於獲取和管理配置對象    |

#### 快速開始

##### 1. 定義配置類

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
        // 創建選項構建器
        var builder = new OptionsBuilder<AppConfig>(args);
        
        // 構建配置對象
        var config = builder.Build();
        
        // 使用配置
        Console.WriteLine($"服務器: {config.Host}:{config.Port}");
        Console.WriteLine($"調試模式: {config.Debug}");
        Console.WriteLine($"日誌級別: {config.LogLevel}");
        Console.WriteLine($"超時時間: {config.Timeout}秒");
    }
}
```

#### 使用方式

##### 命令行參數

支持多種參數格式：

```bash
# 鍵值對格式
myapp.exe --host=example.com --port=9090 --debug=true

# 分離格式
myapp.exe --host example.com --port 9090 --debug true

# 布爾標誌格式
myapp.exe --host example.com --port 9090 --debug

# 混合格式
myapp.exe --host=example.com --port 9090 --debug
```

##### 環境變量

```bash
# 設置環境變量
export HOST=example.com
export PORT=9090
export DEBUG=true

# 運行程序
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
# Docker運行
docker run myapp --host example.com --port 9090 --debug

# 或使用環境變量
docker run -e HOST=example.com -e PORT=9090 -e DEBUG=true myapp
```

#### 高級特性

##### 使用特性配置

```csharp
using GameFrameX.Foundation.Options.Attributes;

public class AdvancedConfig
{
    [Option("h", "host", Required = false, DefaultValue = "localhost")]
    [HelpText("服務器主機地址")]
    public string Host { get; set; }

    [Option("p", "port", Required = true)]
    [HelpText("服務器端口號")]
    public int Port { get; set; }

    [FlagOption("d", "debug")]
    [HelpText("啟用調試模式")]
    public bool Debug { get; set; }

    [RequiredOption("api-key", Required = true)]
    [EnvironmentVariable("API_KEY")]
    [HelpText("API密鑰")]
    public string ApiKey { get; set; }

    [DefaultValue(30.0)]
    public double Timeout { get; set; }
}
```

##### 構建器選項

```csharp
var builder = new OptionsBuilder<AppConfig>(
    args: args,
    boolFormat: BoolArgumentFormat.Flag,        // 布爾參數格式
    ensurePrefixedKeys: true,                   // 確保參數有前綴
    useEnvironmentVariables: true              // 使用環境變量
);

var config = builder.Build(skipValidation: false); // 是否跳過驗證
```

#### 參數優先級

參數按以下優先級應用（高優先級覆蓋低優先級）：

1. **命令行參數** (最高優先級)
2. **環境變量**
3. **默認值** (最低優先級)

##### 示例

```csharp
public class Config
{
    public string Host { get; set; } = "localhost";  // 預設值
    public int Port { get; set; } = 8080;           // 預設值
}
```

```bash
# 設置環境變量
export HOST=env.example.com
export PORT=7070

# 運行程序（命令列參數覆蓋環境變量）
myapp.exe --host cmd.example.com

# 結果：
# Host = "cmd.example.com"  (來自命令列參數)
# Port = 7070               (來自環境變量)
```

#### 布爾參數處理

支持多種布爾參數格式：

```bash
# 標誌格式（推薦）
myapp.exe --debug                    # debug = true

# 鍵值對格式
myapp.exe --debug=true               # debug = true
myapp.exe --debug=false              # debug = false

# 分離格式
myapp.exe --debug true               # debug = true
myapp.exe --debug false              # debug = false

# 支持的布爾值
true, false, 1, 0, yes, no, on, off
```

#### 類型轉換

自動支持以下類型轉換：

- `string` - 直接使用
- `int`, `int?` - 整數轉換
- `bool`, `bool?` - 布爾值轉換
- `double`, `double?` - 雙精度浮點數轉換
- `float`, `float?` - 單精度浮點數轉換
- `decimal`, `decimal?` - 十進制數轉換
- `DateTime`, `DateTime?` - 日期時間轉換
- `Guid`, `Guid?` - GUID轉換
- `Enum` - 枚舉轉換

##### 示例

```csharp
public class TypedConfig
{
    public int Port { get; set; }
    public bool Debug { get; set; }
    public DateTime StartTime { get; set; }
    public LogLevel Level { get; set; }  // 枚舉
}

public enum LogLevel
{
    Debug, Info, Warning, Error
}
```

```bash
myapp.exe --port 9090 --debug true --start-time "2024-01-01 10:00:00" --level Info
```

#### 錯誤處理

##### 必需參數驗證

```csharp
public class Config
{
    [RequiredOption("api-key", Required = true)]
    public string ApiKey { get; set; }
}
```

如果缺少必需參數，會拋出 `ArgumentException`：

```
缺少必需的選項: api-key
```

##### 類型轉換錯誤

當參數值無法轉換為目標類型時，會使用默認值並在控制台輸出警告信息。

#### 最佳實踐

##### 1. 配置類設計

```csharp
public class AppConfig
{
    // 使用有意義的預設值
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    
    // 布爾屬性預設為false
    public bool Debug { get; set; } = false;
    
    // 使用特性提供更多信息
    [RequiredOption("database-url", Required = true)]
    [EnvironmentVariable("DATABASE_URL")]
    public string DatabaseUrl { get; set; }
}
```

##### 2. 錯誤處理

```csharp
try
{
    var builder = new OptionsBuilder<AppConfig>(args);
    var config = builder.Build();
    
    // 使用配置啟動應用
    StartApplication(config);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"配置錯誤: {ex.Message}");
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
        
        // 在Docker中，通常使用環境變量
        // 在開發中，通常使用命令列參數
        
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
        [HelpText("服務器主機地址")]
        public string Host { get; set; }

        [Option("p", "port", DefaultValue = 8080)]
        [EnvironmentVariable("SERVER_PORT")]
        [HelpText("服務器端口號")]
        public int Port { get; set; }

        [FlagOption("d", "debug")]
        [EnvironmentVariable("DEBUG")]
        [HelpText("啟用調試模式")]
        public bool Debug { get; set; }

        [RequiredOption("database-url", Required = true)]
        [EnvironmentVariable("DATABASE_URL")]
        [HelpText("數據庫連接字符串")]
        public string DatabaseUrl { get; set; }

        [Option("timeout", DefaultValue = 30.0)]
        [EnvironmentVariable("REQUEST_TIMEOUT")]
        [HelpText("請求超時時間（秒）")]
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

                Console.WriteLine("服務器配置:");
                Console.WriteLine($"  主機: {config.Host}");
                Console.WriteLine($"  端口: {config.Port}");
                Console.WriteLine($"  調試: {config.Debug}");
                Console.WriteLine($"  數據庫: {config.DatabaseUrl}");
                Console.WriteLine($"  超時: {config.Timeout}秒");

                // 啟動服務器
                StartServer(config);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"配置錯誤: {ex.Message}");
                ShowHelp();
                Environment.Exit(1);
            }
        }

        static void StartServer(ServerConfig config)
        {
            // 服務器啟動邏輯
            Console.WriteLine($"服務器啟動在 {config.Host}:{config.Port}");
        }

        static void ShowHelp()
        {
            Console.WriteLine("用法:");
            Console.WriteLine("  myapp.exe --host <主機> --port <端口> --database-url <數據庫URL> [選項]");
            Console.WriteLine();
            Console.WriteLine("選項:");
            Console.WriteLine("  -h, --host <主機>           服務器主機地址 (預設: localhost)");
            Console.WriteLine("  -p, --port <端口>           服務器端口號 (預設: 8080)");
            Console.WriteLine("  -d, --debug                 啟用調試模式");
            Console.WriteLine("      --database-url <URL>    數據庫連接字符串 (必需)");
            Console.WriteLine("      --timeout <秒>          請求超時時間 (預設: 30.0)");
        }
    }
}
```

#### CommandLineArgumentConverter 使用

除了 OptionsBuilder 之外，您也可以直接使用底層的 CommandLineArgumentConverter：

```csharp
using GameFrameX.Foundation.Options;

// 創建轉換器實例
var converter = new CommandLineArgumentConverter();

// 原始命令列參數
var args = new[] { "--port", "8080", "-h", "localhost" };

// 設置環境變量（可選）
Environment.SetEnvironmentVariable("APP_NAME", "MyApplication");
Environment.SetEnvironmentVariable("LOG_LEVEL", "debug-mode");

// 轉換為標準格式（合併命令列參數和環境變量）
var standardArgs = converter.ConvertToStandardFormat(args);
// 結果: ["--port", "8080", "-h", "localhost", "--APP_NAME", "MyApplication", "--LOG_LEVEL", "debugmode"]

// 轉換為命令列字符串
var commandLineString = converter.ToCommandLineString(standardArgs);
// 結果: "--port 8080 -h localhost --APP_NAME MyApplication --LOG_LEVEL debugmode"

// 獲取所有環境變量
var envVars = converter.GetEnvironmentVariables();
Console.WriteLine($"檢測到 {envVars.Count} 個環境變量");
```

##### 布爾類型參數支持

`CommandLineArgumentConverter` 支持智能識別和處理布爾類型參數，提供三種格式：

```csharp
using GameFrameX.Foundation.Options;

// 設置布爾類型環境變量
Environment.SetEnvironmentVariable("ENABLE_LOGGING", "true");
Environment.SetEnvironmentVariable("DEBUG_MODE", "false");
Environment.SetEnvironmentVariable("VERBOSE", "yes");

var converter = new CommandLineArgumentConverter();

// 1. 標誌格式 (預設) - 只為 true 值添加標誌
converter.BoolFormat = BoolArgumentFormat.Flag;
var flagArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 結果: ["--ENABLE_LOGGING", "--VERBOSE"] (只包含 true 值)

// 2. 鍵值對格式 - 添加鍵值對
converter.BoolFormat = BoolArgumentFormat.KeyValue;
var keyValueArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 結果: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]

// 3. 分離格式 - 鍵和值分開
converter.BoolFormat = BoolArgumentFormat.Separated;
var separatedArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 結果: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]
```

支持的布爾值格式：

- **True 值**: `"true"`, `"1"`, `"yes"`, `"on"`, `"enabled"` (不區分大小寫)
- **False 值**: `"false"`, `"0"`, `"no"`, `"off"`, `"disabled"` (不區分大小寫)

### 🛠️ 通用工具類 (GameFrameX.Foundation.Utility)

提供一系列實用的工具類，包含控制台操作、環境管理、時間處理和雪花ID生成等功能。

#### 核心組件概覽

| 組件        | 文件名                    | 主要功能                      |
|-----------|------------------------|---------------------------|
| **控制台助手** | `ConsoleHelper.cs`     | 控制台Logo打印和格式化輸出           |
| **環境助手**  | `EnvironmentHelper.cs` | 環境變量管理和環境類型定義             |
| **時間助手**  | `TimerHelper.cs`       | Unix時間戳處理和時間轉換            |
| **雪花ID**  | `SnowFlakeIdHelper.cs` | 分佈式唯一ID生成器（Snowflake算法實現） |

#### 控制台助手功能

```csharp
using GameFrameX.Foundation.Utility;

// 打印應用程序Logo
ConsoleHelper.PrintLogo();
// 輸出格式化的控制台Logo，用於應用程序啟動時的品牌展示
```

#### 環境管理功能

```csharp
using GameFrameX.Foundation.Utility;

// 獲取當前環境類型
string currentEnv = Environments.Development;
Console.WriteLine($"當前環境: {currentEnv}");

// 環境判斷
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
{
    // 開發環境特定邏輯
    Console.WriteLine("運行在開發環境");
}
```

#### 時間處理功能

```csharp
using GameFrameX.Foundation.Utility;

// Unix時間戳常量
DateTime epochLocal = TimerHelper.EpochLocal;   // 本地時區的Unix紀元時間
DateTime epochUtc = TimerHelper.EpochUtc;       // UTC時區的Unix紀元時間

// 獲取當前Unix時間戳（秒）
long unixSeconds = TimerHelper.UnixTimeSeconds();
Console.WriteLine($"當前Unix時間戳（秒）: {unixSeconds}");

// 獲取當前Unix時間戳（毫秒）
long unixMilliseconds = TimerHelper.UnixTimeMilliseconds();
Console.WriteLine($"當前Unix時間戳（毫秒）: {unixMilliseconds}");

// 時間戳轉換示例
DateTime currentTime = DateTime.UtcNow;
long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
DateTime restored = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
```

#### 雪花ID生成器

```csharp
using GameFrameX.Foundation.Utility;

// 使用預設配置生成ID
long id1 = SnowFlakeIdHelper.GenerateId();
long id2 = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"生成的ID: {id1}, {id2}");

// 配置工作節點ID和數據中心ID
SnowFlakeIdHelper.WorkId = 1;        // 工作節點ID (0-31)
SnowFlakeIdHelper.DataCenterId = 1;  // 數據中心ID (0-31)

// 生成配置後的ID
long configuredId = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"配置後的ID: {configuredId}");

// 獲取時間戳相關信息
DateTime utcStart = SnowFlakeIdHelper.UtcTimeStart;  // UTC起始時間
long epochTime = SnowFlakeIdHelper.EpochTime;        // 紀元時間戳

Console.WriteLine($"雪花ID起始時間: {utcStart}");
Console.WriteLine($"紀元時間戳: {epochTime}");
```

##### 雪花ID算法說明

雪花ID（Snowflake）是Twitter開源的分佈式ID生成算法，具有以下特點：

- **全局唯一**: 在分佈式環境中保證ID的全局唯一性
- **趨勢遞增**: 生成的ID大致按時間遞增，有利於數據庫索引
- **高性能**: 單機每秒可生成數百萬個ID
- **無依賴**: 不依賴數據庫或其他外部系統

ID結構（64位）：

```
0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
|   |                                             |   |       |       |
|   |<-------------- 41位時間戳 ---------------->|   |<-5位->|<-5位->|<--12位-->
|                                                 |           |       |
符號位(1位)                                        |      數據中心ID   序列號
                                                  |      (5位)      (12位)
                                               工作節點ID
                                                (5位)
```

- **1位符號位**: 固定為0
- **41位時間戳**: 精確到毫秒，可使用約69年
- **5位數據中心ID**: 支持32個數據中心
- **5位工作節點ID**: 每個數據中心支持32個工作節點
- **12位序列號**: 同一毫秒內支持4096個ID

#### 完整使用示例

```csharp
using GameFrameX.Foundation.Utility;

namespace MyApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // 打印應用程序Logo
            ConsoleHelper.PrintLogo();
            
            // 檢查運行環境
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;
            Console.WriteLine($"當前運行環境: {env}");
            
            // 配置雪花ID生成器
            SnowFlakeIdHelper.WorkId = 1;
            SnowFlakeIdHelper.DataCenterId = 1;
            
            // 生成唯一ID
            for (int i = 0; i < 5; i++)
            {
                long id = SnowFlakeIdHelper.GenerateId();
                long timestamp = TimerHelper.UnixTimeMilliseconds();
                
                Console.WriteLine($"ID: {id}, 時間戳: {timestamp}");
                
                // 短暫延遲以觀察ID變化
                Thread.Sleep(1);
            }
            
            // 時間處理示例
            Console.WriteLine($"Unix紀元時間(UTC): {TimerHelper.EpochUtc}");
            Console.WriteLine($"Unix紀元時間(本地): {TimerHelper.EpochLocal}");
            Console.WriteLine($"當前Unix時間戳(秒): {TimerHelper.UnixTimeSeconds()}");
            Console.WriteLine($"當前Unix時間戳(毫秒): {TimerHelper.UnixTimeMilliseconds()}");
        }
    }
}
```

## 🧪 測試

項目包含完整的單元測試，確保代碼質量和功能正確性。所有核心功能都有對應的測試用例，測試覆蓋率達到95%以上。

### 測試覆蓋範圍

#### 🧩 擴展方法庫測試 (Extensions)

- **ArgumentAlreadyExceptionTests**: 參數已存在異常測試
- **BidirectionalDictionaryTests**: 雙向字典功能測試
- **ByteExtensionTests**: 字節數組擴展方法測試
- **CollectionExtensionsTests**: 集合擴展方法測試
- **ConcurrentLimitedQueueTests**: 併發限制隊列測試
- **DisposableConcurrentDictionaryTests**: 可釋放併發字典測試
- **DisposableDictionaryTests**: 可釋放字典測試
- **IDictionaryExtensionsTests**: 字典擴展方法測試
- **IEnumerableExtensionsTests**: 枚舉擴展方法測試
- **ListExtensionsTests**: 列表擴展方法測試
- **LookupXTests**: 查找表功能測試
- **NullObjectTests**: 空對象模式測試
- **NullableConcurrentDictionaryTests**: 可空併發字典測試
- **NullableDictionaryTests**: 可空字典測試
- **ObjectExtensionsTests**: 對象擴展方法測試
- **ReadOnlySpanExtensionsTests**: 只讀Span擴展測試
- **SequenceReaderExtensionsTests**: 序列讀取器擴展測試
- **SpanExtensionsTests**: Span擴展方法測試
- **StringExtensionsTests**: 字符串擴展方法測試
- **TypeExtensionsTests**: 類型擴展方法測試

#### 🔐 加密工具庫測試 (Encryption)

- **AesHelperTests**: AES加密算法測試
- **DsaHelperTests**: DSA數字簽名測試
- **RsaHelperTests**: RSA加密算法測試
- **Sm2HelperTests**: SM2國密算法測試
- **Sm4HelperTests**: SM4國密算法測試
- **XorHelperTests**: XOR異或加密測試

#### 🌐 本地化框架測試 (Localization)

- **LocalizationServiceTests**: 本地化服務核心功能測試
    - 單例模式驗證測試
    - 本地化字符串獲取測試
    - 參數化消息格式化測試
    - 未知鍵處理測試
    - 線程安全併發測試
- **ResourceManagerTests**: 資源管理器測試
    - 提供者優先級測試
    - 懶加載機制測試
    - 統計信息驗證測試
- **DefaultResourceProviderTests**: 默認資源提供者測試
- **AssemblyResourceProviderTests**: 程序集資源提供者測試
    - .resx文件加載測試
    - 多文化支持測試
    - 資源緩存機制測試

#### 🔗 哈希工具庫測試 (Hash)

- **CrcHelperTests**: CRC校驗算法測試
- **HmacSha256HelperTests**: HMAC-SHA256測試
- **Md5HelperTests**: MD5哈希算法測試
- **MurmurHash3HelperTests**: MurmurHash3算法測試
- **Sha1HelperTests**: SHA-1哈希算法測試
- **Sha256HelperTests**: SHA-256哈希算法測試
- **Sha512HelperTests**: SHA-512哈希算法測試
- **XxHashHelperTests**: xxHash高性能哈希測試

#### 🌐 HTTP工具庫測試 (Http.Extension)

- **HttpExtensionTests**: HTTP客戶端擴展方法測試

#### ⚙️ 命令行參數處理測試 (Options)

- **CommandLineArgumentConverterTests**: 命令行參數轉換器功能測試
    - 空參數數組處理測試
    - 空參數值處理測試
    - 重複參數檢測測試
    - 環境變量轉換測試
    - 值清理功能測試
    - 單連字符參數轉換測試
    - 命令行字符串生成測試
    - 環境變量獲取測試
    - 完整工作流程測試
    - 布爾類型參數處理測試
        - 標誌格式布爾參數測試
        - 鍵值對格式布爾參數測試
        - 分離格式布爾參數測試
        - 多種布爾值格式解析測試
        - 非布爾值處理測試
- **OptionsBuilderTests**: 選項構建器功能測試
    - 基本配置構建測試
    - 特性配置測試
    - 類型轉換測試
    - 驗證功能測試
- **OptionsProviderTests**: 選項提供器功能測試
    - 配置註冊和獲取測試
    - 全局配置管理測試

### 運行測試

```bash
# 運行所有測試
dotnet test

# 運行特定模塊測試
dotnet test --filter "FullyQualifiedName~Extensions"
dotnet test --filter "FullyQualifiedName~Encryption"
dotnet test --filter "FullyQualifiedName~Hash"
dotnet test --filter "FullyQualifiedName~Localization"
dotnet test --filter "FullyQualifiedName~Options"

# 運行特定測試類
dotnet test --filter "ClassName=XxHashHelperTests"
dotnet test --filter "ClassName=StringExtensionsTests"
dotnet test --filter "ClassName=LocalizationServiceTests"
dotnet test --filter "ClassName=CommandLineArgumentConverterTests"

# 生成測試覆蓋率報告
dotnet test --collect:"XPlat Code Coverage"

# 運行性能測試
dotnet test --filter "Category=Performance"
```

### 測試特點

- **全面覆蓋**: 每個公共方法都有對應的測試用例
- **邊界測試**: 包含空值、邊界值、異常情況的測試
- **性能測試**: 對關鍵算法進行性能基準測試
- **併發測試**: 驗證線程安全的組件在多線程環境下的正確性
- **兼容性測試**: 確保在不同.NET版本下的兼容性

## 🏗️ 架構設計

### 設計原則

- **高性能**: 所有組件都經過性能優化，適用於高併發場景
- **易用性**: 提供簡潔的 API 設計，降低學習成本
- **可擴展**: 模塊化設計，支持自定義擴展
- **類型安全**: 充分利用 .NET 的類型系統，減少運行時錯誤
- **內存友好**: 使用 Span<T> 和 Memory<T> 等現代 .NET 特性，減少內存分配

### 依賴關係

```
GameFrameX.Foundation.Extensions (核心擴展)
├── GameFrameX.Foundation.Encryption (加密工具)
├── GameFrameX.Foundation.Hash (哈希工具)
├── GameFrameX.Foundation.Json (JSON工具)
├── GameFrameX.Foundation.Logger (日誌工具)
├── GameFrameX.Foundation.Options (參數處理)
├── GameFrameX.Foundation.Http.Extension (HTTP擴展)
└── GameFrameX.Foundation.Http.Normalization (HTTP標準化)
```

## 🔧 開發指南

### 環境要求

- .NET 10.0 或更高版本
- C# 12.0 或更高版本

### 構建項目

```bash
# 克隆倉庫
git clone https://github.com/GameFrameX/GameFrameX.Foundation.git
cd GameFrameX.Foundation

# 還原依賴
dotnet restore

# 構建項目
dotnet build

# 運行測試
dotnet test
```

### 貢獻指南

1. Fork 本倉庫
2. 創建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 開啟 Pull Request

## 📊 性能基準

### 擴展方法性能

| 操作        | 傳統方法  | 擴展方法  | 性能提升 |
|-----------|-------|-------|------|
| 字符串空值檢查   | 100ns | 15ns  | 85%  |
| 集合隨機元素獲取  | 200ns | 50ns  | 75%  |
| Span 字節操作 | 500ns | 80ns  | 84%  |
| 雙向字典查找    | 150ns | 120ns | 20%  |

### 加密算法性能

| 算法       | 數據大小 | 加密時間   | 解密時間   |
|----------|------|--------|--------|
| AES-256  | 1KB  | 0.05ms | 0.04ms |
| RSA-2048 | 1KB  | 2.1ms  | 0.8ms  |
| SM4      | 1KB  | 0.08ms | 0.07ms |
| XOR      | 1KB  | 0.01ms | 0.01ms |

### 哈希算法性能

| 算法          | 數據大小 | 處理時間  | 吞吐量      |
|-------------|------|-------|----------|
| MD5         | 1MB  | 2.1ms | 476MB/s  |
| SHA-256     | 1MB  | 3.8ms | 263MB/s  |
| xxHash64    | 1MB  | 0.8ms | 1.25GB/s |
| MurmurHash3 | 1MB  | 1.2ms | 833MB/s  |

## 📋 系統要求

- .NET 10.0 或更高版本
- 支持 Windows、Linux、macOS

## 🤝 貢獻

歡迎提交 Issue 和 Pull Request 來改進項目。

1. Fork 項目
2. 創建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打開 Pull Request

## 🤝 社區支持

- **問題反饋**: [GitHub Issues](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- **功能請求**: [GitHub Discussions](https://github.com/GameFrameX/GameFrameX.Foundation/discussions)
- **文檔貢獻**: 歡迎提交文檔改進的 PR

## 📄 許可證

本項目採用 MIT 許可證 - 查看 [LICENSE](LICENSE) 文件瞭解詳情。

## 🙏 致謝

感謝所有為 GameFrameX.Foundation 做出貢獻的開發者們！

## 🔗 相關鏈接

- [GameFrameX 官網](https://gameframex.doc.alianblank.com)
- [文檔中心](https://gameframex.doc.alianblank.com)

---

<div align="center">

**[⬆ 回到頂部](#gameframexfoundation)**

Made with ❤️ by GameFrameX Team

</div>
