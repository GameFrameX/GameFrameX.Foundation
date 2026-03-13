# GameFrameX.Foundation.Hash 代码审查报告

> 审查日期：2026-03-13
> 审查范围：`GameFrameX.Foundation.Hash` 全部源文件（10 个 .cs）
> 严重级别：🔴 Critical / 🟡 Warning / 🔵 Info

---

## 一、项目概览

| 项目 | 说明 |
|------|------|
| 目标框架 | net6.0 / net8.0 / net9.0 / net10.0 |
| 许可证 | MIT + Apache 2.0 双许可 |
| 不安全代码 | `AllowUnsafeBlocks=true` |
| 强名称签名 | 使用 SNK 密钥 |
| 依赖项 | Standart.Hash.xxHash 4.0.5 |
| 源文件数 | 10 个 |

### 文件清单

| 文件 | 分类 | 行数（约） |
|------|------|-----------|
| `Md5Helper.cs` | 加密哈希 | 179 |
| `Sha1Helper.cs` | 加密哈希 | 138 |
| `Sha256Helper.cs` | 加密哈希 | 124 |
| `Sha512Helper.cs` | 加密哈希 | 119 |
| `HmacSha256Helper.cs` | 加密哈希 | 33 |
| `MurmurHash3Helper.cs` | 非加密哈希 | 138 |
| `XxHashHelper.cs` | 非加密哈希 | 488 |
| `CrcHelper.cs` | CRC 校验 | 246 |
| `CrcHelper.Crc32.cs` | CRC 校验 | 82 |
| `CrcHelper.Crc64.cs` | CRC 校验 | 592 |

---

## 二、问题汇总

| ID | 级别 | 文件 | 问题简述 |
|----|------|------|---------|
| C-01 | 🔴 | CrcHelper.Crc64.cs | NonCryptographicHashAbstract 使用 throw new ArgumentException("destination") 缺少参数名 | ✅ 已修复 |
| C-02 | 🔴 | CrcHelper.cs | GetCrc32 方法使用过时的 ArgumentNullException 构造方式 | ✅ 已修复 |
| W-01 | 🟡 | Md5Helper.cs | HashWithSalt 方法参数命名与内部变量名相同（input），存在变量遮蔽 |
| W-02 | 🟡 | Md5Helper.cs | ToHash 方法对每个字节调用 ToString，产生大量装箱开销 |
| W-03 | 🟡 | Sha256Helper.cs | 空字符串处理返回空字符串的哈希值，而非抛出异常或返回默认值 |
| W-04 | 🟡 | Sha1Helper.cs | ComputeHash 对空输入返回 string.Empty，与同项目其他类行为不一致 |
| W-05 | 🟡 | XxHashHelper.cs | InternalXxHashHelper 内部类使用 unsafe 指针但未标记 unsafe 方法 |
| W-06 | 🟡 | CrcHelper.cs | GetCrc32 方法混用新旧异常抛出方式，代码风格不一致 |
| W-07 | 🟡 | CrcHelper.Crc64.cs | 大量被注释掉的异步方法代码，增加维护成本 |
| I-01 | 🔵 | Md5Helper.cs | Md5Cryptography 使用静态字段，非线程安全 | ✅ 已修复 |
| I-02 | 🔵 | XxHashHelper.cs | Hash128 方法可考虑添加 Span<byte> 重载 | ✅ 已修复 |
| I-03 | 🔵 | CrcHelper.cs | GetCrc32Bytes 可考虑使用 BitConverter | ✅ 已修复 |

---

## 三、详细问题说明

### 🔴 C-01 — CrcHelper.Crc64.cs：ArgumentException 缺少参数名

**位置：** `NonCryptographicHashAbstract.ThrowDestinationTooShort()`

**问题：** 抛出 `ArgumentException` 时只传入字符串 "destination"，缺少 `paramName` 参数，导致异常信息不完整，调试困难。

```csharp
// 当前（有问题）
private protected static void ThrowDestinationTooShort()
{
    throw new ArgumentException("destination");
}

// 建议修复
private protected static void ThrowDestinationTooShort()
{
    throw new ArgumentException("destination", nameof(destination));
}
```

---

### 🔴 C-02 — CrcHelper.cs：过时的异常抛出方式

**位置：** `GetCrc32(byte[] bytes)`、`GetCrc32(Stream stream)`

**问题：** 使用旧的 `throw new ArgumentNullException(nameof(xxx), @"...")` 构造方式，应统一使用 `ArgumentNullException.ThrowIfNull`。

```csharp
// 当前（不一致）
if (bytes == null)
{
    throw new ArgumentNullException(nameof(bytes), @"Bytes is invalid.");
}

// 建议统一风格
ArgumentNullException.ThrowIfNull(bytes, nameof(bytes));
```

---

### 🟡 W-01 — Md5Helper.cs：变量遮蔽

**位置：** `HashWithSalt(string input, string salt, bool isUpper)`

**问题：** 方法参数名为 `input`，内部使用相同名称的局部变量，存在变量遮蔽。

```csharp
// 当前（有问题）
public static string HashWithSalt(string input, string salt, bool isUpper = false)
{
    ArgumentNullException.ThrowIfNull(input, nameof(input));
    ArgumentNullException.ThrowIfNull(salt, nameof(salt));
    var saltedInput = input + salt;  // 变量遮蔽
    return Hash(saltedInput, isUpper);
}
```

---

### 🟡 W-02 — Md5Helper.cs：ToHash 方法装箱开销

**位置：** `ToHash` 方法

**问题：** 对每个字节调用 `ToString("X2")`/`ToString("x2")` 会产生装箱操作，性能不佳。

```csharp
// 当前（低效）
foreach (var t in data)
{
    sb.Append(t.ToString("X2")); // 装箱
}

// 建议：使用十六进制转换表或 BitConverter
var hex = isUpper ? "0123456789ABCDEF" : "0123456789abcdef";
foreach (var b in data)
{
    sb.Append(hex[b >> 4]);
    sb.Append(hex[b & 0xF]);
}
```

---

### 🟡 W-03 — Sha256Helper.cs：空字符串处理不一致

**位置：** `ComputeHash(string input)`

**问题：** 对空字符串返回空字符串的 SHA-256 哈希值（`e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855`），这与部分调用方的预期可能不符。

```csharp
// 当前行为
if (string.IsNullOrEmpty(input))
{
    return ComputeHash(Array.Empty<byte>()); // 返回空输入的哈希值
}
```

**建议：** 在文档中明确说明空字符串的处理方式，或考虑添加重载以支持不同行为。

---

### 🟡 W-04 — Sha1Helper.cs：空输入处理不一致

**位置：** `ComputeHash(string input)`、`ComputeHash(byte[] buffer)`

**问题：** 对空字符串/空数组返回 `string.Empty`，与其他 SHA 类的行为不一致。

```csharp
// 当前（不一致）
if (input.Length == 0)
{
    return string.Empty; // 与 Sha256Helper 行为不一致
}
```

---

### 🟡 W-05 — XxHashHelper.cs：未标记 unsafe

**位置：** `InternalXxHashHelper.Hash32`、`Hash64`

**问题：** 使用 `unsafe` 指针操作但方法本身未标记 `unsafe` 修饰符。

```csharp
// 当前（有问题）
[MethodImpl(MethodImplOptions.AggressiveInlining)]
private static uint Hash32(byte* input, int length, uint seed = 0) // 需要 unsafe
```

---

### 🟡 W-06 — CrcHelper.cs：异常抛出风格不一致

**位置：** 多个 `GetCrc32` 重载

**问题：** 部分使用 `ArgumentNullException.ThrowIfNull`，部分使用旧的 `throw new` 方式。

---

### 🟡 W-07 — CrcHelper.Crc64.cs：大量被注释的代码

**位置：** `AppendAsync` 相关方法

**问题：** 保留大量被注释掉的异步方法代码，增加维护成本和理解难度。

---

## 四、优点总结

1. **API 设计统一**：所有加密哈希类提供一致的 `ComputeHash` / `VerifyHash` / `ComputeFileHash` 接口
2. **功能覆盖全面**：支持 MD5、SHA-1、SHA-256、SHA-512、HMAC-SHA256、xxHash、MurmurHash3、CRC32、CRC64
3. **文档完整**：XML 注释详细，包括参数说明、返回值、异常说明
4. **参数校验规范**：统一使用 `ArgumentNullException.ThrowIfNull` 等现代 API
5. **性能优化**：xxHash 使用 unsafe 指针和 AggressiveInlining，MurmurHash3 实现完整
6. **支持多种输入**：字符串、字节数组、流、文件路径
7. **验证功能**：所有加密哈希类都提供 VerifyHash 方法
8. **跨平台支持**：多目标框架 net6.0~net10.0

---

## 五、修复优先级建议

| 优先级 | 问题 ID | 原因 |
|--------|---------|------|
| P0（立即修复） | C-01 | ✅ 已修复 |
| P0（立即修复） | C-02 | ✅ 已修复 |
| P1（近期修复） | W-01 | 变量遮蔽可能导致代码理解困难 |
| P1（近期修复） | W-02 | 性能问题，高频调用时影响明显 |
| P2（计划修复） | W-03、W-04 | 行为一致性问题 |
| P2（计划修复） | W-05 | unsafe 标记缺失 |
| P3（优化） | W-06、W-07、I-01~I-03 | 代码清理和微优化 |

---

## 六、安全注意事项

### 算法选择建议

| 场景 | 推荐算法 | 避免使用 |
|------|----------|----------|
| 密码存储 | SHA-256 / SHA-512 | MD5、SHA-1 |
| 文件完整性校验 | SHA-256、xxHash | MD5（已不安全） |
| 高性能哈希表 | xxHash32 / xxHash64 | 加密哈希 |
| 消息认证 | HMAC-SHA256 | MD5 |
| 网络协议校验 | CRC32 / CRC64 | - |

### 警告标识

项目代码中已对不安全算法添加警告注释：

- `Md5Helper.cs`：注释提醒 MD5 已不再加密安全
- `Sha1Helper.cs`：注释提醒 SHA-1 已不再加密安全
