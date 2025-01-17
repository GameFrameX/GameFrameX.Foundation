# GameFrameX.Foundation

GameFrameX 的基建库, 提供了一些基础的扩展方法和工具类.

## HTTP 消息结构标准化组件 (GameFrameX.Foundation.Http.Normalization)

该组件提供了 HTTP 消息结构标准化的功能, 让消息的格式更加统一.

服务器返回的消息包含 `code` 和 `message` 和 `data`, 但是客户端需要统一的返回格式, 需要进行格式化.所以这个组件提供了格式化的功能. 适用于GameFrameX 的整个生态标准

## 加密工具库 (GameFrameX.Foundation.Encryption)

该库提供了多种加密算法的实现，包括：

### AES 加密 (AesHelper)

提供 AES 对称加密算法的实现：

- 支持字符串和字节数组的加密/解密
- 使用 Rijndael 算法作为 AES 标准的实现
- 提供高安全级别的加密方案

### RSA 加密 (RsaHelper)

提供 RSA 非对称加密算法的实现：

- 支持密钥对生成
- 支持公钥加密/私钥解密
- 支持数字签名和验证
- 支持字符串和字节数组操作

### DSA 签名 (DsaHelper)

提供 DSA 数字签名算法的实现：

- 支持密钥对生成
- 支持数字签名和验证
- 支持字符串和字节数组操作

### SM2/SM4 加密 (Sm2Helper/Sm4Helper)

提供国密 SM2/SM4 算法的实现：

- SM2: 非对称加密算法
    - 支持密钥对生成
    - 支持加密/解密操作
- SM4: 对称加密算法
    - 支持 ECB/CBC 加密模式
    - 支持 JavaScript 兼容模式
    - 支持十六进制密钥

### XOR 加密 (XorHelper)

提供异或加密算法的实现：

- 支持快速加密模式（仅加密前220字节）
- 支持完整加密模式
- 支持指定范围加密
- 内存优化设计，支持原地加密

### 使用示例

```csharp
// AES 加密示例
string encrypted = AesHelper.Encrypt("Hello World", "your-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-key");
// RSA 加密示例
var keys = RsaHelper.Make();
string encrypted = RsaHelper.Encrypt(keys["publicKey"], "Hello World");
string decrypted = RsaHelper.Decrypt(keys["privateKey"], encrypted);
// SM4 加密示例
string encrypted = Sm4Helper.EncryptCbc("your-key", "Hello World");
string decrypted = Sm4Helper.DecryptCbc("your-key", encrypted);
```

## 哈希工具库 (GameFrameX.Foundation.Hash)

该库提供了多种哈希算法的实现，包括：

### MD5 哈希 (Md5Helper)

- 提供字符串、流、文件和字节数组的MD5哈希计算
- 支持加盐哈希
- 支持哈希值验证
- 注：MD5已不再被认为是加密安全的，建议在安全要求较高的场景使用SHA-256或更高强度的算法

### SHA 系列哈希

- SHA-1 (Sha1Helper): 生成160位(20字节)哈希值
- SHA-256 (Sha256Helper): 生成256位(32字节)哈希值
- SHA-512 (Sha512Helper): 生成512位(64字节)哈希值
- 支持字符串、字节数组和文件的哈希计算与验证
- 支持自定义编码

### HMAC-SHA256 (HmacSha256Helper)

- 基于密钥的哈希消息认证码
- 结合SHA-256哈希函数和密钥
- 返回Base64编码的哈希值

### CRC 校验 (CrcHelper)

- CRC32: 32位循环冗余校验
- CRC64: 64位循环冗余校验，基于ECMA-182标准
- 支持流式处理
- 支持字节数组和文件处理

### MurmurHash3 (MurmurHash3Helper)

- 非加密型高性能哈希算法
- 32位版本实现
- 支持自定义种子值
- 适用于哈希表等场景

### xxHash (XxHashHelper)

- 提供32位、64位和128位哈希值计算
- 高性能非加密型哈希算法
- 支持字符串、字节数组和类型哈希
- 适用于需要快速哈希计算的场景

### 使用示例

```csharp
// MD5哈希示例
string md5Hash = Md5Helper.Hash("Hello World");
string saltedHash = Md5Helper.HashWithSalt("Hello World", "salt");
// SHA-256哈希示例
string sha256Hash = Sha256Helper.ComputeHash("Hello World");
// HMAC-SHA256示例
string hmacHash = HmacSha256Helper.Hash("message", "key");
// CRC32校验示例
int crc32 = CrcHelper.GetCrc32("Hello World"u8.ToArray());
// MurmurHash3示例
uint murmurHash = MurmurHash3Helper.Hash("Hello World");
// xxHash示例
ulong xxHash = XxHashHelper.Hash64("Hello World");
```