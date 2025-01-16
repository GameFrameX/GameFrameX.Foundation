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