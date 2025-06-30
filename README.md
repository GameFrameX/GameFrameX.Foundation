# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-6.0%2B-purple.svg)](https://dotnet.microsoft.com/)

### 📊 NuGet 包状态

| 包名 | 版本 | 下载次数 |
|------|------|----------|
| GameFrameX.Foundation.Encryption | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/) |
| GameFrameX.Foundation.Hash | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/) |
| GameFrameX.Foundation.Http.Extension | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/) |
| GameFrameX.Foundation.Http.Normalization | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) |
| GameFrameX.Foundation.Json | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/) |
| GameFrameX.Foundation.Logger | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/) |

GameFrameX 的基础工具库，提供了一系列高性能、易用的基础组件和工具类，涵盖加密、哈希、HTTP、JSON、日志等常用功能。

## 📦 程序集概览

| 程序集 | 功能描述 | NuGet 包名 |
|--------|----------|------------|
| GameFrameX.Foundation.Encryption | 加密工具库 | `GameFrameX.Foundation.Encryption` |
| GameFrameX.Foundation.Hash | 哈希工具库 | `GameFrameX.Foundation.Hash` |
| GameFrameX.Foundation.Http.Extension | HttpClient 扩展 | `GameFrameX.Foundation.Http.Extension` |
| GameFrameX.Foundation.Http.Normalization | HTTP 消息标准化 | `GameFrameX.Foundation.Http.Normalization` |
| GameFrameX.Foundation.Json | JSON 序列化工具 | `GameFrameX.Foundation.Json` |
| GameFrameX.Foundation.Logger | Serilog 日志配置 | `GameFrameX.Foundation.Logger` |

## 🚀 快速开始

### 安装

通过 NuGet 包管理器安装所需的组件：

```bash
# 安装加密工具库
dotnet add package GameFrameX.Foundation.Encryption

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
using GameFrameX.Foundation.Hash;
using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Logger;

// AES 加密
string encrypted = AesHelper.Encrypt("Hello World", "your-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-key");

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

项目包含完整的单元测试，确保代码质量和功能正确性。

```bash
# 运行所有测试
dotnet test

# 运行特定测试
dotnet test --filter "ClassName=XxHashHelperTests"
```

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

- [GameFrameX 官网](https://gameframex.com)
- [文档中心](https://docs.gameframex.com)
- [问题反馈](https://github.com/GameFrameX/GameFrameX.Foundation/issues)

---

**GameFrameX.Foundation** - 让开发更简单，让代码更优雅！

