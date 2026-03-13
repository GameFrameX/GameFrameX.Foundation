# GameFrameX.Foundation.Encryption 代码审查报告

> **审查日期**：2026-03-13
> **审查范围**：`GameFrameX.Foundation.Encryption` 目录下所有源码文件
> **审查维度**：正确性、安全性、健壮性、性能、代码规范、API 设计

---

## 📂 目录结构

```
GameFrameX.Foundation.Encryption/
├── GameFrameX.Foundation.Encryption.csproj
├── AesHelper.cs
├── RsaHelper.cs
├── DsaHelper.cs
├── XorHelper.cs
├── Sm2Helper.cs
├── Sm4Helper.cs
├── Localization/
│   └── Keys.cs
└── Sm/
    ├── Cipher.cs
    ├── SM2.cs
    ├── SM2Util.cs
    ├── SM3.cs          ← 实际类名 SupportClass，文件名与内容不符
    ├── SM3Util.cs
    ├── SM4.cs
    ├── SM4Util.cs
    ├── Sm3Digest.cs
    ├── Sm4Context.cs
    └── GeneralDigest.cs
```

---

## 📊 问题汇总

| 严重级别 | 数量 |
|---------|:----:|
| 🚨 Critical | **12** |
| ⚠️ Warning  | **15** |
| ℹ️ Info     | **11** |
| **合计**   | **38** |

---

## 🚨 Critical（严重问题）

### C-01 · `AesHelper.cs` 行 96–99, 189–192 — 加密/解密失败时静默吞异常返回 `null`

```csharp
catch (Exception ex)
{
    Console.WriteLine(ex);   // 仅打印到控制台
}
return encryptedBytes;       // encryptedBytes 仍为 null
```

`encryptedBytes` 初始化为 `null`（行 72）；如果加密过程抛出任何异常，方法静默返回 `null`。
调用方（行 37）的 `Convert.ToBase64String(null)` 立即抛 `ArgumentNullException`，**掩盖了真正的错误原因**。解密方法同理（行 165, 195）。

**风险**：数据丢失 + 错误原因被掩盖
**修复**：移除 `catch` 块，让异常自然传播；或重新抛出 `throw;`

---

### C-02 · `AesHelper.cs` 行 74–76, 167–169 — IV 和 Salt 硬编码固定值

```csharp
var iv   = new byte[] { 224, 131, 122, ... };
var salt = new byte[] { 234, 231, 123, ... };
```

- **固定 IV**：使相同明文 + 相同密钥始终产生相同密文，破坏 CBC 模式语义安全性
- **固定 Salt**：使 PBKDF2 派生的密钥可被彩虹表预计算

**风险**：密码学安全性根基被破坏
**修复**：每次加密时随机生成 IV/Salt，并将其作为前缀附在密文前方；解密时从密文头部读取

---

### C-03 · `AesHelper.cs` 行 84, 177 — PBKDF2 迭代次数过低（1000 次）

```csharp
new Rfc2898DeriveBytes(encryptKey, salt, 1000, HashAlgorithmName.SHA256)
```

OWASP 2023 建议 PBKDF2-SHA256 最少 **600,000** 次迭代。1000 次在现代硬件上可在毫秒内暴力破解。

**风险**：密钥可被快速暴力穷举
**修复**：将迭代次数提高至至少 600,000

---

### C-04 · `RsaHelper.cs` 行 448, 489, 534, 579 — 签名/验签使用 SHA1

```csharp
rsa.SignData(dataToSign, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
```

SHA-1 已在 2017 年被实际碰撞攻击证明不安全（SHAttered），NIST/CA-Browser Forum 已弃用。
同文件的 `Sign()`/`Verify()` 静态方法使用 SHA256，但 `SignData`/`VerifyData` 家族仍使用 SHA1，**API 安全性不一致**。

**风险**：已知可利用的密码学漏洞，签名可被伪造
**修复**：统一改为 `HashAlgorithmName.SHA256`

---

### C-05 · `RsaHelper.cs` 行 149, 173, 229 — RSA 加密使用 PKCS1v1.5 填充

```csharp
rsa.Encrypt(chunk, RSAEncryptionPadding.Pkcs1);
```

PKCS#1 v1.5 填充易受 **Bleichenbacher padding oracle 攻击**。

**风险**：在网络场景下可被逐步解密密文
**修复**：改为 `RSAEncryptionPadding.OaepSHA256`

---

### C-06 · `SM4Util.cs` 行 16 — 默认密钥硬编码在源码

```csharp
public string secretKey = "1814546261730461";
```

如果调用者忘记设置 `secretKey`，将使用这个公开可知的密钥进行加密，安全性为零。

**风险**：误用时等同于明文传输
**修复**：移除默认值，强制调用方传入密钥；或在使用前检查是否为默认值并抛出异常

---

### C-07 · `DsaHelper.cs` 行 13, 34 — 使用已弃用的 `DSACryptoServiceProvider`

```csharp
private readonly DSACryptoServiceProvider _dsa;
```

`DSACryptoServiceProvider` 仅支持 1024-bit DSA（FIPS 186-2），已被 NIST 弃用。
此外，在非 Windows 平台（macOS、Linux）上不可用，会抛 `PlatformNotSupportedException`。

**风险**：弃用算法 + 跨平台不兼容
**修复**：改用 `DSA.Create(2048)` 或使用 ECDsa（Ed25519）

---

### C-08 · `Sm3Digest.cs` 行 282–294 — Span 重载为空实现

```csharp
public override void BlockUpdate(ReadOnlySpan<byte> input)
{
    // 空实现！
}

public override int DoFinal(Span<byte> output)
{
    return DigestLength;  // 不写入任何数据！
}
```

如果有代码通过 `IDigest` 接口的 Span 重载调用 SM3，将**静默产出全零哈希值**，完整性验证完全失效。

**风险**：数据完整性验证失效，且无任何错误提示
**修复**：实现完整的 Span 重载逻辑，或抛出 `NotImplementedException`

---

### C-09 · `SM4.cs` 行 294–298 — PKCS7 反填充无验证（Padding Oracle 漏洞）

```csharp
int p = input[^1];
ret = new byte[input.Length - p];
Array.Copy(input, 0, ret, 0, input.Length - p);
```

解密反填充时直接信任最后一个字节作为填充长度，缺少：
1. `p` 是否在 `1–16` 范围内的验证
2. 所有填充字节是否一致的验证

当 `p > input.Length` 时，导致 `ArgumentOutOfRangeException`（负数组长度）。

**风险**：Padding Oracle 攻击面 + 异常崩溃
**修复**：添加填充值范围校验和一致性校验，验证失败时抛 `CryptographicException`

---

### C-10 · `RsaHelper.cs` 行 279, 334, 390, 446, 532 — `RSA.Create()` 未被 `using` 包裹

```csharp
var rsa = RSA.Create();      // 没有 using
rsa.FromXmlString(publicKey);
```

多个静态方法（`Make`、`Encrypt`、`Decrypt`、`SignData`、`VerifyData`）均未 Dispose，造成非托管资源泄漏。

**风险**：非托管资源泄漏，高频调用下可能耗尽系统密钥资源
**修复**：改为 `using var rsa = RSA.Create();`

---

### C-11 · `DsaHelper.cs` 行 34, 48, 67, 139 — `DSACryptoServiceProvider` 未 Dispose

```csharp
new DSACryptoServiceProvider()  // 未 Dispose
```

`DsaHelper` 类持有 `_dsa` 字段但未实现 `IDisposable`，外部无法主动释放。

**风险**：非托管资源泄漏
**修复**：实现 `IDisposable`，在 `Dispose()` 中调用 `_dsa.Dispose()`

---

### C-12 · `RsaHelper.cs` 行 265–268 — 构造函数异常路径资源泄漏

```csharp
public RsaHelper(string key)
{
    var rsa = RSA.Create();
    rsa.FromXmlString(key);   // 若抛 CryptographicException，rsa 泄漏
    _rsa = rsa;
}
```

`RsaHelper` 持有 `_rsa` 但**未实现 `IDisposable`**，外部无法释放。

**风险**：构造失败时资源泄漏，成功后也无法释放
**修复**：实现 `IDisposable`，使用 try/catch 包裹构造逻辑并在失败时立即 Dispose

---

## ⚠️ Warning（警告）

### W-01 · `RsaHelper.cs` 行 450–453, 491–494 — 裸 `catch` 吞掉所有异常返回 `null`

```csharp
catch
{
    return null;
}
```

裸 `catch` 会吞掉 `OutOfMemoryException`、`StackOverflowException` 等致命异常。
调用方取回 `null` 后 `Convert.ToBase64String(null)` 立即 `NullReferenceException`，错误链更难排查。

---

### W-02 · `RsaHelper.cs` 行 120–180 — `EncryptBase64` 中大段代码重复

`catch` 块中的分块加密循环与 `try` 块完全相同，严重违反 DRY 原则。
`catch` 中如果 `ImportRSAPublicKey` 也失败，第一个 `CryptographicException` 将被丢弃。

---

### W-03 · `SM3Util.cs` 行 25 — 使用 `Encoding.Default`

```csharp
byte[] msg1 = Encoding.Default.GetBytes(data);
```

`Encoding.Default` 在 .NET Framework 上取决于系统区域设置，导致相同字符串在不同平台产生不同哈希值。
**修复**：改为 `Encoding.UTF8`

---

### W-04 · `SM2.cs` 行 15–22 — 单例模式非线程安全

```csharp
private static Sm2 _instance;
public static Sm2 Instance
{
    get { return _instance ??= new Sm2(); }
}
```

`??=` 在多线程场景下非原子操作，高并发下可能创建多个实例。
**修复**：改用 `Lazy<Sm2>` 或加锁

---

### W-05 · `Cipher.cs` 行 42 — `_keyOff` 使用 `byte` 类型

```csharp
private byte _keyOff;
```

`_keyOff` 与 `_key.Length`（32）逻辑耦合，`byte` 最大值 255，若逻辑变更可能导致隐含截断。
**修复**：改为 `int`

---

### W-06 · `Cipher.cs` 行 68–85 — `n.ToByteArray()` 重复调用最多 6 次

```csharp
if (n.ToByteArray().Length == 33)           // 调用 1
{
    Array.Copy(n.ToByteArray(), 1, ...);    // 调用 2
}
else if (n.ToByteArray().Length == 32)      // 调用 3
{
    tmpd = n.ToByteArray();                 // 调用 4
}
else
{
    for (int i = 0; i < 32 - n.ToByteArray().Length; i++) // 调用 5
    Array.Copy(n.ToByteArray(), ...);       // 调用 6
}
```

每次调用都分配新的 `byte[]`。**修复**：缓存到局部变量 `var bytes = n.ToByteArray();`

---

### W-07 · `SM4.cs` 行 34, 38 — `GET_ULONG_BE` 运算符优先级问题

```csharp
// & 优先级高于 |，末尾 & 0xff 仅作用于最后一个操作数
n = (b[i] & 0xff) << 24 | ((b[i+1] & 0xff) << 16) | ((b[i+2] & 0xff) << 8) | (b[i+3] & 0xff) & 0xff;
```

末尾 `& 0xff` 因运算符优先级仅约束最后一个 `|` 的操作数，整体表达式含义不清晰。
**修复**：补全括号，明确运算顺序

---

### W-08 · `SM4.cs` 行 77 — `Rotl` 对有符号 `long` 使用算术右移

```csharp
return Shl(x, n) | x >> (32 - n);
```

C# 的 `>>` 对有符号类型执行算术右移，高位填符号位，可能污染高 32 位。
**修复**：改用 `>>>` (C# 11+) 或掩码 `(x >> (32 - n)) & (long)(uint.MaxValue >> n - 1)`

---

### W-09 · `AesHelper.cs` 行 148 — 方法命名不对称

```csharp
// 加密
public static byte[] Encrypt(byte[] encryptByte, string encryptKey)
// 解密
public static byte[] AesDecrypt(byte[] decryptByte, string decryptKey)  // ← 应为 Decrypt
```

**修复**：重命名为 `Decrypt`

---

### W-10 · `RsaHelper.cs` — 静态方法与实例方法混用，API 设计混乱

类既是 `sealed class`（有构造函数和实例字段），又有大量 `static` 方法，且两者使用不同密钥格式（XML 格式 vs Base64 格式），调用者容易混淆。

---

### W-11 · `DsaHelper.cs` 行 63–66 — 参数校验放在 `try` 块内部

```csharp
try
{
    ArgumentNullException.ThrowIfNull(dataToSign, nameof(dataToSign));
    // ...
}
catch
{
    return null;  // ArgumentNullException 被吞掉！
}
```

`ArgumentNullException` 属于编程错误，不应被静默忽略。
**修复**：将参数校验移至 `try` 块之前

---

### W-12 · `SM2Util.cs` 行 188–191 — 解密中硬编码偏移量

```csharp
byte[] c1Bytes = Hex.Decode(... data.Substring(0, 130) ...);
int c2Len = encryptedData.Length - 97;
```

130 = 65 字节 × 2（未压缩 EC 点 hex 编码），若使用压缩点格式则直接失败。
未校验 `encryptedData.Length` 是否大于 97。

---

### W-13 · `Sm4Helper.cs` 行 36 — `throw` 语句缩进缺失

```csharp
else if (!hexString && keyString.Length != 16)
{
throw new ArgumentException(...);  // ← 缺少缩进
}
```

---

### W-14 · `SM4Util.cs` 行 16, 21, 26, 31 — 使用 `public` 字段而非属性

```csharp
public string secretKey = "1814546261730461";
public string iv = "0000000000000000";
public bool hexString = false;
public bool forJavascript = false;
```

违背 .NET 设计准则，应使用属性（Properties）。

---

### W-15 · `Sm4Context.cs` 行 11, 16, 21 — 使用 `public` 字段而非属性

同 W-14。

---

## ℹ️ Info（建议）

### I-01 · `SM3Util.cs` — `Encrypt()` 方法名语义错误

SM3 是**哈希/摘要算法**，不是加密算法。应命名为 `Hash` 或 `ComputeDigest`。

---

### I-02 · `SM3.cs` — 文件名与类名不匹配

文件名为 `SM3.cs`，但实际包含的类是 `SupportClass`。应重命名文件为 `SupportClass.cs`。

---

### I-03 · 命名风格混乱

- `Sm4_Setkey`、`Sm4_one_round`、`Sm4_crypt_ecb` — snake_case，不符合 C# 规范
- `GET_ULONG_BE`、`PUT_ULONG_BE` — 全大写下划线命名
- `ecc_p`、`ecc_a` — snake_case 字段名

应统一改为 PascalCase（方法/属性）和 camelCase（局部变量/私有字段）。

---

### I-04 · `SM3Util.cs` 行 26–34 — 残留注释代码

大量被注释掉的 HMac 相关代码应清理。

---

### I-05 · `Sm3Digest.cs` 行 296–308 — 残留注释测试 `Main` 方法

应移除。

---

### I-06 · `SM4Util.cs` 行 110 — 残留注释代码

```csharp
//return Hex.Encode(encrypted);
```

应清理。

---

### I-07 · `RsaHelper.cs` / `DsaHelper.cs` — `Make()` 返回 `Dictionary<string, string>` 且键名不一致

- `RsaHelper.Make()` 返回键名 `"privateKey"`
- `DsaHelper.Make()` 返回键名 `"privatekey"`（全小写）

应返回强类型 record 或 tuple，并统一键名。

---

### I-08 · `AesHelper.cs` — 字符串重载与 `byte[]` 重载参数校验风格不一致

字符串重载使用 `LocalizationService.GetString()` 获取本地化消息，`byte[]` 重载使用硬编码英文字符串。

---

### I-09 · `RsaHelper.cs` — 异常消息不使用本地化

与 `AesHelper` 风格不一致，应统一使用或统一不使用 `LocalizationService`。

---

### I-10 · `XorHelper.cs` — 文档注释缺少安全声明

XOR 循环加密可通过频率分析轻松破解，文档注释应明确声明**仅用于轻量混淆，不提供安全性保障**。

---

### I-11 · `SM4.cs` — 常量数据应标记为 `static readonly`

`_sboxTable`、`Fk`、`Ck` 是不变数据，应标记为 `static readonly` 以避免每次实例化时重复分配。

---

## 🏆 最高优先级修复（Top 5）

| 优先级 | 编号 | 问题摘要 | 影响 |
|:------:|------|---------|------|
| ① | **C-08** | `Sm3Digest` Span 重载为空实现 | SM3 哈希结果全零，完整性验证完全失效 |
| ② | **C-09** | SM4 PKCS7 反填充无验证 | Padding Oracle 攻击面 + 数组越界崩溃 |
| ③ | **C-01** | AES 静默吞异常返回 `null` | 数据丢失 + 错误原因被掩盖 |
| ④ | **C-02 + C-03** | AES 硬编码 IV/Salt + PBKDF2 仅 1000 次迭代 | 密码学安全性根基被破坏 |
| ⑤ | **C-04 + C-05** | RSA 签名用 SHA-1、加密用 PKCS1v1.5 | 两者均为已知可利用漏洞 |

---

*报告由自动化代码审查工具生成，如有疑问请结合具体业务场景评估修复优先级。*
