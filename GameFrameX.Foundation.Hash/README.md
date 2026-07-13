# GameFrameX.Foundation.Hash

[![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](https://github.com/GameFrameX/GameFrameX/blob/main/LICENSE)

GameFrameX.Foundation.Hash 是 GameFrameX 框架的基础设施库，提供了多种哈希算法的统一接口。该库覆盖两类用途：**通用哈希**（MD5、SHA 系列、xxHash、MurmurHash3、CRC、HMAC）用于校验/签名/缓存键，以及**密码哈希（KDF）**（PBKDF2、bcrypt、scrypt、Argon2id）用于用户密码存储。

## 🎯 核心特性

- **通用哈希算法** - MD5、SHA-1、SHA-256、SHA-512、xxHash、MurmurHash3、CRC32/64、HMAC-SHA256
- **密码哈希（KDF）** - PBKDF2-HMAC-SHA256、bcrypt、scrypt、Argon2id，遵循 OWASP 2023 推荐参数
- **PHC 自描述格式** - 密码哈希串自带算法/参数/盐，`Verify` 按前缀自动识别算法
- **常量时间比较** - 密码哈希 `Verify` 使用 `CryptographicOperations.FixedTimeEquals`，抗计时攻击
- **fail-closed 校验** - 密码哈希对畸形/未知前缀/跨算法前缀统一返回 `false`，仅 `null` 抛异常
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

## 🔐 密码哈希（KDF）

> ⚠️ **重要区分**：通用哈希（MD5/SHA/xxHash）追求"快"，即使加盐也无法抵抗 GPU/ASIC 暴力破解，**绝不应用于用户密码存储**。密码哈希（KDF）刻意"慢"且消耗内存，专门用于口令存储。本库提供 OWASP 2023 推荐的四种 KDF。

### 推荐用法（统一门面）

`PasswordHashHelper` 是统一入口，`Verify` 会按 PHC 前缀**自动识别算法**，无需关心存储串是哪种算法：

```csharp
using GameFrameX.Foundation.Hash;

// 推荐算法：Argon2id（PasswordHashHelper.IsRecommended 仅对 Argon2id 返回 true）
string stored = PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Argon2id, "user-password");

// 验证时无需传算法种类——按存储串前缀自动识别
bool ok = PasswordHashHelper.Verify("user-password", stored);   // true
bool bad = PasswordHashHelper.Verify("wrong-password", stored); // false

// 探测存储串使用的算法（未知前缀返回 null）
PasswordHashAlgorithmKind? kind = PasswordHashHelper.DetectAlgorithm(stored); // Argon2id
```

### 四种算法单独使用

每种 KDF 都有独立的 Helper，提供各自参数的重载：

```csharp
// PBKDF2-HMAC-SHA256（默认 600000 次迭代，输出 32 字节）
string pbkdf2 = Pbkdf2Helper.Hash("password");
bool pbkdf2Ok = Pbkdf2Helper.Verify("password", pbkdf2);

// bcrypt（默认 work factor 12；密码 UTF-8 超过 72 字节：Hash 抛 ArgumentException，Verify 返回 false）
string bcrypt = BcryptHelper.Hash("password");
bool bcryptOk = BcryptHelper.Verify("password", bcrypt);

// scrypt（默认 N=32768, r=8, p=1，输出 32 字节）
string scrypt = ScryptHelper.Hash("password");
bool scryptOk = ScryptHelper.Verify("password", scrypt);

// Argon2id（默认 m=65536 KB, t=3, p=1，输出 32 字节）
string argon2 = Argon2idHelper.Hash("password");
bool argon2Ok = Argon2idHelper.Verify("password", argon2);
```

### 自定义参数

```csharp
// PBKDF2：自定义迭代次数与输出长度
string h1 = Pbkdf2Helper.Hash("password", iterations: 100000, outputBytes: 32);

// bcrypt：自定义 work factor（4-31）
string h2 = BcryptHelper.Hash("password", workFactor: 14);

// scrypt：自定义 N/r/p（N 必须是 2 的幂）
string h3 = ScryptHelper.Hash("password", n: 65536, r: 8, p: 1);

// Argon2id：自定义内存/迭代/并行度
string h4 = Argon2idHelper.Hash("password", memoryKB: 65536, iterations: 3, parallelism: 1);

// 指定盐与参数的确定性重载（用于交叉验证或受控测试）
byte[] salt = new byte[16];
string h5 = Pbkdf2Helper.Hash(
    Encoding.UTF8.GetBytes("password"), salt, iterations: 600000, outputBytes: 32);
```

### PHC 自描述格式

每种 KDF 的存储串都遵循 PHC（Password Hashing Competition）格式，自带算法、参数、盐与哈希：

| 算法 | PHC 前缀 | 示例结构 |
|------|---------|---------|
| PBKDF2 | `$pbkdf2-sha256$` | `$pbkdf2-sha256$600000$<base64-salt>$<base64-hash>` |
| bcrypt | `$2a$` | `$2a$12$<22-char-salt><31-char-hash>` |
| scrypt | `$scrypt$` | `$scrypt$32768$8$1$<base64-salt>$<base64-hash>` |
| Argon2id | `$argon2id$` | `$argon2id$v=19$m=65536,t=3,p=1$<base64-salt>$<base64-hash>` |

### 默认参数（OWASP 2023 推荐）

| 算法 | 默认参数 |
|------|---------|
| PBKDF2 | 迭代 600000 次，输出 32 字节，盐 16 字节 |
| bcrypt | work factor 12 |
| scrypt | N=32768, r=8, p=1，输出 32 字节，盐 16 字节 |
| Argon2id | m=65536 KB(64MB), t=3, p=1，输出 32 字节，盐 16 字节，版本 0x13 |

### 安全行为

- **常量时间比较**：所有 KDF 的 `Verify` 内部派生后用 `CryptographicOperations.FixedTimeEquals` 比较，抗计时攻击。
- **fail-closed**：存储串为 `null` 抛 `ArgumentNullException`；空串、畸形、未知前缀、跨算法前缀统一返回 `false`，不抛异常。
- **bcrypt 72 字节限制**：UTF-8 字节数超过 72 时，`Hash` 抛 `ArgumentException`（`paramName=password`），`Verify` 返回 `false`。
- **参数越界**：迭代次数 < 1、bcrypt work factor 越界（<4 或 >31）、scrypt N 非 2 的幂、r/p < 1、输出长度 < 1 等，抛 `ArgumentOutOfRangeException` 且 `ParamName` 正确。

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

存储用户密码应使用本库的 KDF，而非自行拼接盐与通用哈希。推荐 Argon2id，`Verify` 会按 PHC 前缀自动识别算法：

```csharp
// 注册：哈希密码（推荐 Argon2id，IsRecommended 仅对它返回 true）
string stored = PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Argon2id, password);

// 登录：按存储串前缀自动识别算法并常量时间校验
bool valid = PasswordHashHelper.Verify(password, stored);

// 透明升级：检测旧存储串算法，验证通过后用 Argon2id 重新哈希
PasswordHashAlgorithmKind? algo = PasswordHashHelper.DetectAlgorithm(stored);
if (valid && algo != PasswordHashAlgorithmKind.Argon2id)
{
    stored = PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Argon2id, password);
}
```

> ❌ 不要用 `Md5Helper.HashWithSalt` 或 `Sha256Helper.ComputeHash(password + salt)` 存密码——通用哈希太快，无法抵抗 GPU 暴力破解。

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

1. **密码存储场景（KDF）**
    - 新系统首选：Argon2id（推荐）；其次 scrypt、bcrypt、PBKDF2
    - 通过 `PasswordHashHelper` 统一调用，`Verify` 自动识别算法
    - 通用哈希（MD5/SHA）即使加盐也不适合存密码

2. **加密哈希场景**
    - 数字签名/完整性校验：使用 SHA-256 或 SHA-512
    - 避免使用 MD5 和 SHA-1（已不安全）

3. **高性能场景**
    - 哈希表：使用 xxHash32 或 xxHash64
    - 数据完整性校验：使用 CRC32 或 CRC64
    - 缓存键生成：使用 xxHash 系列

4. **兼容性场景**
    - 与旧系统兼容：可能需要使用 MD5
    - 标准协议：根据协议要求选择算法

### 安全注意事项

```csharp
// ❌ 不安全：直接或加盐用通用哈希存密码（太快，可被 GPU 暴力破解）
string unsafeHash = Md5Helper.Hash(password);
string alsoUnsafe = Sha256Helper.ComputeHash(password + salt);

// ✅ 正确：使用本库的 KDF（Argon2id 推荐），自带随机盐与参数
string stored = PasswordHashHelper.Hash(PasswordHashAlgorithmKind.Argon2id, password);
bool valid = PasswordHashHelper.Verify(password, stored);
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

| 算法          | 安全性  | 性能    | 输出长度  | 适用场景  |
|-------------|------|-------|-------|-------|
| MD5         | ❌ 低  | ⭐⭐⭐   | 32字符  | 兼容性需求 |
| SHA-1       | ⚠️ 中 | ⭐⭐    | 40字符  | 兼容性需求 |
| SHA-256     | ✅ 高  | ⭐⭐    | 64字符  | 安全哈希  |
| SHA-512     | ✅ 高  | ⭐     | 128字符 | 高安全需求 |
| xxHash32    | ❌ 无  | ⭐⭐⭐⭐⭐ | 8字符   | 高性能场景 |
| xxHash64    | ❌ 无  | ⭐⭐⭐⭐⭐ | 16字符  | 高性能场景 |
| CRC32       | ❌ 无  | ⭐⭐⭐⭐  | 8字符   | 数据校验  |
| HMAC-SHA256 | ✅ 高  | ⭐⭐    | 64字符  | 消息认证  |
| Argon2id    | ✅ 高  | 🐢 极慢 | PHC 串  | 密码存储（推荐） |
| scrypt      | ✅ 高  | 🐢 慢  | PHC 串  | 密码存储  |
| bcrypt      | ✅ 高  | 🐢 慢  | PHC 串  | 密码存储  |
| PBKDF2      | ✅ 中高 | 🐢 慢  | PHC 串  | 密码存储/合规 |

## 📄 许可证

本项目采用 [Apache License 2.0](https://github.com/GameFrameX/GameFrameX/blob/main/LICENSE) 许可证。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目。

## 📞 支持

- 📖 [文档主页](https://gameframex.doc.alianblank.com/)
- 🐛 [问题反馈](https://github.com/GameFrameX/GameFrameX/issues)
- 💬 [讨论区](https://github.com/GameFrameX/GameFrameX/discussions)