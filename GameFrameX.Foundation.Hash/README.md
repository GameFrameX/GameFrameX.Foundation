# GameFrameX.Foundation.Hash

[![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](https://github.com/GameFrameX/GameFrameX/blob/main/LICENSE)

GameFrameX.Foundation.Hash 是 GameFrameX 框架的基础设施库，提供了多种高性能哈希算法的统一接口。该库支持常用的加密哈希算法（MD5、SHA系列）和高性能非加密哈希算法（xxHash、MurmurHash3、CRC等）。

## 🎯 核心特性

- **多种哈希算法支持** - MD5、SHA-1、SHA-256、SHA-512、xxHash、MurmurHash3、CRC32/64、HMAC-SHA256
- **高性能实现** - 基于.NET原生算法和优化的第三方库
- **统一API设计** - 所有算法提供一致的调用接口
- **多种输入格式** - 支持字符串、字节数组、流和文件路径
- **类型安全** - 完整的参数验证和异常处理
- **加盐支持** - MD5等算法支持加盐哈希
- **验证功能** - 内置哈希值验证方法

## 📦 安装

```bash
dotnet add package GameFrameX.Foundation.Hash
```

## 🚀 快速开始

### MD5 哈希

```csharp
using GameFrameX.Foundation.Hash;

// 字符串哈希
string text = "Hello World";
string hash = Md5Helper.Hash(text);
Console.WriteLine(hash); // 输出: b10a8db164e0754105b7a99be72e3fe5

// 加盐哈希
string saltedHash = Md5Helper.HashWithSalt(text, "salt123");

// 文件哈希
string fileHash = Md5Helper.HashByFilePath("path/to/file.txt");

// 验证哈希
bool isValid = Md5Helper.IsVerify(text, hash);
```

### SHA-256 哈希

```csharp
using GameFrameX.Foundation.Hash;

// 字符串哈希
string text = "Hello World";
string hash = Sha256Helper.ComputeHash(text);

// 文件哈希
string fileHash = Sha256Helper.ComputeFileHash("path/to/file.txt");

// 验证哈希
bool isValid = Sha256Helper.VerifyHash(text, hash);
```

### xxHash 高性能哈希

```csharp
using GameFrameX.Foundation.Hash;

// 32位哈希
uint hash32 = XxHashHelper.Hash32("Hello World");

// 64位哈希
ulong hash64 = XxHashHelper.Hash64("Hello World");

// 128位哈希
uint128 hash128 = XxHashHelper.Hash128("Hello World");

// 类型哈希
uint typeHash = XxHashHelper.Hash32<MyClass>();
```

## 📖 详细使用指南

### MD5 哈希算法

MD5Helper 提供了完整的MD5哈希功能：

```csharp
// 基本哈希
string hash = Md5Helper.Hash("input text");

// 大写格式
string upperHash = Md5Helper.Hash("input text", isUpper: true);

// 字节数组哈希
byte[] data = Encoding.UTF8.GetBytes("input text");
string hash = Md5Helper.Hash(data);

// 流哈希
using var stream = new MemoryStream(data);
string hash = Md5Helper.Hash(stream);

// 加盐哈希（字符串盐）
string saltedHash = Md5Helper.HashWithSalt("input", "salt");

// 加盐哈希（字节数组盐）
byte[] salt = Encoding.UTF8.GetBytes("salt");
string saltedHash = Md5Helper.HashWithSalt("input", salt);

// 验证哈希
bool isValid = Md5Helper.IsVerify("input", hash);
bool isSaltedValid = Md5Helper.IsVerifyWithSalt("input", "salt", saltedHash);
```

### SHA 系列哈希算法

#### SHA-256

```csharp
// 基本哈希
string hash = Sha256Helper.ComputeHash("input text");

// 指定编码
string hash = Sha256Helper.ComputeHash("input text", Encoding.ASCII);

// 字节数组哈希
byte[] data = Encoding.UTF8.GetBytes("input text");
string hash = Sha256Helper.ComputeHash(data);

// 文件哈希
string fileHash = Sha256Helper.ComputeFileHash("path/to/file.txt");

// 验证哈希
bool isValid = Sha256Helper.VerifyHash("input text", hash);
bool isFileValid = Sha256Helper.VerifyFileHash("path/to/file.txt", fileHash);
```

#### SHA-1 和 SHA-512

```csharp
// SHA-1
string sha1Hash = Sha1Helper.ComputeHash("input text");
bool sha1Valid = Sha1Helper.VerifyHash("input text", sha1Hash);

// SHA-512
string sha512Hash = Sha512Helper.ComputeHash("input text");
bool sha512Valid = Sha512Helper.VerifyHash("input text", sha512Hash);
```

### xxHash 高性能哈希

xxHash 是专为高性能设计的非加密哈希算法：

```csharp
// 32位哈希
uint hash32 = XxHashHelper.Hash32("input text");
uint hash32FromBytes = XxHashHelper.Hash32(Encoding.UTF8.GetBytes("input"));

// 64位哈希
ulong hash64 = XxHashHelper.Hash64("input text");
ulong hash64FromBytes = XxHashHelper.Hash64(Encoding.UTF8.GetBytes("input"));

// 128位哈希
uint128 hash128 = XxHashHelper.Hash128("input text");
uint128 hash128FromBytes = XxHashHelper.Hash128(Encoding.UTF8.GetBytes("input"));

// 指定长度的128位哈希
byte[] data = Encoding.UTF8.GetBytes("input text");
uint128 hash128Limited = XxHashHelper.Hash128(data, 5); // 只使用前5个字节

// 类型哈希
uint typeHash32 = XxHashHelper.Hash32<string>();
ulong typeHash64 = XxHashHelper.Hash64<MyClass>();

// 检查128位哈希是否为默认值
bool isDefault = XxHashHelper.IsDefault(hash128);
```

### MurmurHash3 算法

```csharp
// 32位 MurmurHash3
uint murmurHash = MurmurHash3Helper.Hash32("input text");

// 指定种子值
uint murmurHashWithSeed = MurmurHash3Helper.Hash32("input text", seed: 12345);

// 字节数组哈希
byte[] data = Encoding.UTF8.GetBytes("input text");
uint murmurHashFromBytes = MurmurHash3Helper.Hash32(data);
```

### CRC 校验算法

#### CRC32

```csharp
// 基本CRC32
uint crc32 = CrcHelper.Crc32("input text");

// 字节数组CRC32
byte[] data = Encoding.UTF8.GetBytes("input text");
uint crc32FromBytes = CrcHelper.Crc32(data);

// 流CRC32
using var stream = new MemoryStream(data);
uint crc32FromStream = CrcHelper.Crc32(stream);
```

#### CRC64

```csharp
// 基本CRC64
ulong crc64 = CrcHelper.Crc64("input text");

// 字节数组CRC64
byte[] data = Encoding.UTF8.GetBytes("input text");
ulong crc64FromBytes = CrcHelper.Crc64(data);
```

### HMAC-SHA256 算法

```csharp
// 基本HMAC-SHA256
string hmac = HmacSha256Helper.ComputeHash("input text", "secret key");

// 字节数组输入
byte[] data = Encoding.UTF8.GetBytes("input text");
byte[] key = Encoding.UTF8.GetBytes("secret key");
string hmacFromBytes = HmacSha256Helper.ComputeHash(data, key);

// 验证HMAC
bool isValid = HmacSha256Helper.VerifyHash("input text", "secret key", hmac);
```

## 🎨 高级用法

### 批量哈希计算

```csharp
// 批量计算多个字符串的哈希值
var inputs = new[] { "text1", "text2", "text3" };
var hashes = inputs.Select(Md5Helper.Hash).ToArray();

// 批量验证
var results = inputs.Zip(hashes, Md5Helper.IsVerify).ToArray();
```

### 文件完整性校验

```csharp
public class FileIntegrityChecker
{
    public static bool VerifyFileIntegrity(string filePath, string expectedHash)
    {
        if (!File.Exists(filePath))
            return false;
            
        var actualHash = Sha256Helper.ComputeFileHash(filePath);
        return Sha256Helper.VerifyFileHash(filePath, expectedHash);
    }
    
    public static Dictionary<string, string> ComputeDirectoryHashes(string directoryPath)
    {
        var hashes = new Dictionary<string, string>();
        var files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
        
        foreach (var file in files)
        {
            hashes[file] = Sha256Helper.ComputeFileHash(file);
        }
        
        return hashes;
    }
}
```

### 密码哈希最佳实践

```csharp
public class PasswordHasher
{
    private static readonly Random Random = new Random();
    
    public static string HashPassword(string password)
    {
        // 生成随机盐
        var salt = GenerateRandomSalt();
        var hash = Md5Helper.HashWithSalt(password, salt);
        
        // 返回盐和哈希的组合
        return $"{salt}:{hash}";
    }
    
    public static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split(':');
        if (parts.Length != 2) return false;
        
        var salt = parts[0];
        var hash = parts[1];
        
        return Md5Helper.IsVerifyWithSalt(password, salt, hash);
    }
    
    private static string GenerateRandomSalt()
    {
        var bytes = new byte[16];
        Random.NextBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
```

### 性能基准测试

```csharp
public class HashPerformanceTest
{
    public static void BenchmarkHashAlgorithms(string input)
    {
        var sw = Stopwatch.StartNew();
        
        // MD5
        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            Md5Helper.Hash(input);
        }
        Console.WriteLine($"MD5: {sw.ElapsedMilliseconds}ms");
        
        // SHA-256
        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            Sha256Helper.ComputeHash(input);
        }
        Console.WriteLine($"SHA-256: {sw.ElapsedMilliseconds}ms");
        
        // xxHash32
        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            XxHashHelper.Hash32(input);
        }
        Console.WriteLine($"xxHash32: {sw.ElapsedMilliseconds}ms");
        
        // xxHash64
        sw.Restart();
        for (int i = 0; i < 100000; i++)
        {
            XxHashHelper.Hash64(input);
        }
        Console.WriteLine($"xxHash64: {sw.ElapsedMilliseconds}ms");
    }
}
```

## 💡 最佳实践

### 算法选择指南

1. **加密安全场景**
   - 密码存储：使用 SHA-256 或更高强度算法
   - 数字签名：使用 SHA-256 或 SHA-512
   - 避免使用 MD5 和 SHA-1（已不安全）

2. **高性能场景**
   - 哈希表：使用 xxHash32 或 xxHash64
   - 数据完整性校验：使用 CRC32 或 CRC64
   - 缓存键生成：使用 xxHash 系列

3. **兼容性场景**
   - 与旧系统兼容：可能需要使用 MD5
   - 标准协议：根据协议要求选择算法

### 安全注意事项

```csharp
// ❌ 不安全：直接哈希密码
string unsafeHash = Md5Helper.Hash(password);

// ✅ 安全：使用盐值
string salt = GenerateRandomSalt();
string safeHash = Sha256Helper.ComputeHash(password + salt);

// ✅ 更安全：使用专门的密码哈希算法（如 bcrypt、scrypt、Argon2）
// 注意：本库主要提供通用哈希算法，密码存储建议使用专门的密码哈希库
```

### 性能优化建议

```csharp
// ✅ 重用字节数组避免重复编码
byte[] data = Encoding.UTF8.GetBytes(input);
string md5Hash = Md5Helper.Hash(data);
string sha256Hash = Sha256Helper.ComputeHash(data);

// ✅ 对于大文件，使用流式处理
using var fileStream = File.OpenRead(largeFilePath);
string hash = Md5Helper.Hash(fileStream);

// ✅ 批量操作时考虑并行处理
var hashes = inputs.AsParallel()
    .Select(input => new { Input = input, Hash = XxHashHelper.Hash64(input) })
    .ToArray();
```

### 错误处理

```csharp
public static class SafeHashHelper
{
    public static string SafeComputeFileHash(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"文件不存在: {filePath}");
            }
            
            return Sha256Helper.ComputeFileHash(filePath);
        }
        catch (UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"没有权限访问文件: {filePath}");
        }
        catch (IOException ex)
        {
            throw new InvalidOperationException($"读取文件时发生IO错误: {ex.Message}");
        }
    }
}
```

## 🔧 配置选项

### 自定义编码

```csharp
// 使用不同的字符编码
string hash1 = Sha256Helper.ComputeHash("测试文本", Encoding.UTF8);
string hash2 = Sha256Helper.ComputeHash("测试文本", Encoding.Unicode);
string hash3 = Sha256Helper.ComputeHash("测试文本", Encoding.ASCII);
```

### xxHash 种子值

```csharp
// MurmurHash3 支持自定义种子值
uint hash1 = MurmurHash3Helper.Hash32("input", seed: 0);
uint hash2 = MurmurHash3Helper.Hash32("input", seed: 12345);
// 相同输入，不同种子会产生不同的哈希值
```

## 🔍 故障排除

### 常见问题

**Q: MD5 哈希结果与在线工具不一致？**
```csharp
// 确保使用相同的编码和格式
string input = "Hello World";
string hash = Md5Helper.Hash(input, isUpper: false); // 小写
string upperHash = Md5Helper.Hash(input, isUpper: true); // 大写
```

**Q: 文件哈希计算失败？**
```csharp
// 检查文件是否存在和权限
if (!File.Exists(filePath))
{
    Console.WriteLine("文件不存在");
    return;
}

try
{
    string hash = Sha256Helper.ComputeFileHash(filePath);
}
catch (UnauthorizedAccessException)
{
    Console.WriteLine("没有文件访问权限");
}
```

**Q: 大文件哈希计算内存占用过高？**
```csharp
// 使用流式处理而不是一次性读取整个文件
using var fileStream = File.OpenRead(largeFilePath);
string hash = Md5Helper.Hash(fileStream);
```

### 调试技巧

```csharp
// 启用详细日志记录
public static class HashDebugHelper
{
    public static void DebugHash(string input)
    {
        Console.WriteLine($"输入: {input}");
        Console.WriteLine($"UTF8字节: {string.Join(",", Encoding.UTF8.GetBytes(input))}");
        Console.WriteLine($"MD5: {Md5Helper.Hash(input)}");
        Console.WriteLine($"SHA256: {Sha256Helper.ComputeHash(input)}");
        Console.WriteLine($"xxHash32: {XxHashHelper.Hash32(input)}");
        Console.WriteLine($"xxHash64: {XxHashHelper.Hash64(input)}");
    }
}
```

## 📊 性能对比

| 算法 | 安全性 | 性能 | 输出长度 | 适用场景 |
|------|--------|------|----------|----------|
| MD5 | ❌ 低 | ⭐⭐⭐ | 32字符 | 兼容性需求 |
| SHA-1 | ⚠️ 中 | ⭐⭐ | 40字符 | 兼容性需求 |
| SHA-256 | ✅ 高 | ⭐⭐ | 64字符 | 安全哈希 |
| SHA-512 | ✅ 高 | ⭐ | 128字符 | 高安全需求 |
| xxHash32 | ❌ 无 | ⭐⭐⭐⭐⭐ | 8字符 | 高性能场景 |
| xxHash64 | ❌ 无 | ⭐⭐⭐⭐⭐ | 16字符 | 高性能场景 |
| CRC32 | ❌ 无 | ⭐⭐⭐⭐ | 8字符 | 数据校验 |
| HMAC-SHA256 | ✅ 高 | ⭐⭐ | 64字符 | 消息认证 |

## 📄 许可证

本项目采用 [Apache License 2.0](https://github.com/GameFrameX/GameFrameX/blob/main/LICENSE) 许可证。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目。

## 📞 支持

- 📖 [文档主页](https://gameframex.doc.alianblank.com/)
- 🐛 [问题反馈](https://github.com/GameFrameX/GameFrameX/issues)
- 💬 [讨论区](https://github.com/GameFrameX/GameFrameX/discussions)