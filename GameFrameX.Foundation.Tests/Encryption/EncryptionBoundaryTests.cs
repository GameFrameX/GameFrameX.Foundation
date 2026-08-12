using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Encryption;
using Xunit;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// Encryption 模块边界测试：AesHelper / Sm2Helper / Sm4Helper / DsaHelper。
/// 覆盖错误密钥、篡改密文、非法输入异常类型、中文/Emoji 往返等场景。
/// </summary>
/// <remarks>
/// AesHelper 加密模式：AES-GCM（认证加密），密钥通过 PBKDF2(SHA256, 600000) 派生 32 字节。
/// SM2/SM4 基于自定义 / BouncyCastle 实现。
/// DsaHelper 静态 SignData(byte[], string) 捕获 CryptographicException/XmlException 后返回 null。
/// </remarks>
public class EncryptionBoundaryTests
{
    // ========================================================================
    // AesHelper 边界测试
    // 加密模式：AES-GCM
    // 格式：[Version(1) | Salt(16) | Nonce(12) | Tag(16) | 密文]，头部 HeaderSize = 45 字节
    // ========================================================================

    /// <summary>
    /// 用 keyA 加密的密文，用 keyB 解密时 GCM Tag 验证失败 → CryptographicException。
    /// AesHelper 的密钥是任意长度字符串（通过 PBKDF2 派生 32 字节 AES-GCM 密钥）。
    /// </summary>
    [Fact]
    public void AesDecrypt_WithWrongKey_ShouldThrowCryptographicException()
    {
        // Arrange
        var plainText = "AES-GCM wrong key boundary test";
        var keyA = "KeyAlpha-11111111";
        var keyB = "KeyBeta-22222222";
        var encryptedBytes = AesHelper.Encrypt(Encoding.UTF8.GetBytes(plainText), keyA);

        // Act & Assert — 错误密钥导致 GCM Tag 校验失败，抛 CryptographicException 家族异常
        Assert.ThrowsAny<CryptographicException>(() => AesHelper.Decrypt(encryptedBytes, keyB));
    }

    /// <summary>
    /// 密文长度小于 HeaderSize（45 字节）→ ArgumentException。
    /// </summary>
    [Fact]
    public void AesDecrypt_WithPayloadShorterThanHeader_ShouldThrowArgumentException()
    {
        // Arrange — 10 字节，远小于 HeaderSize=45
        var shortPayload = new byte[] { 2, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AesHelper.Decrypt(shortPayload, "anyKey"));
    }

    /// <summary>
    /// 版本字节不等于 CurrentVersion(2) → CryptographicException。
    /// 源码：AesHelper.Decrypt 中 if (decryptByte[0] != CurrentVersion) throw new CryptographicException(...)。
    /// </summary>
    [Fact]
    public void AesDecrypt_WithUnsupportedVersion_ShouldThrowCryptographicException()
    {
        // Arrange — 构造长度 >= HeaderSize 但版本号错误的 payload
        var payload = new byte[50];
        payload[0] = 99; // 非法版本号（合法版本为 2）
        for (int i = 1; i < payload.Length; i++)
        {
            payload[i] = (byte)i;
        }

        // Act & Assert
        Assert.Throws<CryptographicException>(() => AesHelper.Decrypt(payload, "anyKey"));
    }

    // ========================================================================
    // Sm2Helper 边界测试
    // 非法 hex 字符 → BouncyCastle Hex.Decode 抛异常（BC 内部类型，非 FormatException/CryptographicException）。
    // 字节数组 Decrypt 最小密文长度 = 97 字节（65 C1 + 0 C2 + 32 C3）。
    // ========================================================================

    /// <summary>
    /// 中文明文加解密往返测试。
    /// </summary>
    [Fact]
    public void Sm2EncryptDecrypt_ChineseText_ShouldRoundtrip()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var plainText = "这是一段中文测试文本，用于验证SM2加解密的正确性。";

        // Act
        var encrypted = Sm2Helper.Encrypt(publicKey, plainText);
        var decrypted = Sm2Helper.Decrypt(privateKey, encrypted);

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    /// <summary>
    /// Emoji 明文加解密往返测试。
    /// </summary>
    [Fact]
    public void Sm2EncryptDecrypt_EmojiText_ShouldRoundtrip()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out string privateKey);
        var plainText = "Emoji test: 🚀🌟💻🎮🔥🎲🏆🎵📱💡";

        // Act
        var encrypted = Sm2Helper.Encrypt(publicKey, plainText);
        var decrypted = Sm2Helper.Decrypt(privateKey, encrypted);

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    /// <summary>
    /// 字节数组 Decrypt：密文长度 < 97 字节（最小长度）→ ArgumentException。
    /// 源码：Sm2Util.Decrypt(byte[], byte[]) 中 if (encryptedData.Length < minEncryptedLen) throw。
    /// </summary>
    [Fact]
    public void Sm2Decrypt_ByteArray_WithShortEncryptedData_ShouldThrowArgumentException()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out _, out string privateKey);
        var privateKeyBytes = Convert.FromHexString(privateKey);
        var shortEncryptedData = new byte[50]; // < minEncryptedLen=97

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Sm2Helper.Decrypt(privateKeyBytes, shortEncryptedData));
    }

    /// <summary>
    /// 非法 hex 公钥（含非 hex 字符）→ BouncyCastle Hex.Decode 抛出异常。
    /// 异常来自 BouncyCastle 内部，非 FormatException 也非 CryptographicException。
    /// </summary>
    [Fact]
    public void Sm2Encrypt_WithNonHexPublicKey_ShouldThrow()
    {
        // Arrange
        var nonHexKey = "XYZ:not_a_hex_key!!";

        // Act & Assert — BouncyCastle Hex.Decode 异常类型为 BC 内部类型（IOException）
        Assert.ThrowsAny<Exception>(() => Sm2Helper.Encrypt(nonHexKey, "test"));
    }

    /// <summary>
    /// 非法 hex 私钥（含非 hex 字符）→ BouncyCastle Hex.Decode 抛出异常。
    /// </summary>
    [Fact]
    public void Sm2Decrypt_WithNonHexPrivateKey_ShouldThrow()
    {
        // Arrange
        Sm2Helper.GenerateKeyPair(out string publicKey, out _);
        var encrypted = Sm2Helper.Encrypt(publicKey, "test");
        var nonHexPrivateKey = "ZZnot_a_hex_key!!";

        // Act & Assert
        Assert.ThrowsAny<Exception>(() => Sm2Helper.Decrypt(nonHexPrivateKey, encrypted));
    }

    // ========================================================================
    // Sm4Helper 边界测试
    // PKCS7 填充验证在 Sm4.Padding 中（抛 CryptographicException）。
    // 篡改密文后解密 → 填充验证失败 → CryptographicException。
    // ========================================================================

    /// <summary>
    /// CBC 模式：翻转密文最后一字节后解密 → PKCS7 填充验证失败 → CryptographicException。
    /// 源码：Sm4.Padding(byte[], Sm4Decrypt) 中验证 padding byte 范围和一致性，不通过则 throw。
    /// </summary>
    [Fact]
    public void Sm4DecryptCbc_WithTamperedCiphertext_ShouldThrowCryptographicException()
    {
        // Arrange
        var key = "0123456789ABCDEF0123456789ABCDEF"; // 32 hex chars
        var plainText = "SM4 CBC tampered ciphertext test with enough padding blocks!!";
        var encryptedHex = Sm4Helper.EncryptCbc(key, plainText, hexString: true);
        var tamperedHex = TamperLastByte(encryptedHex);

        // Act & Assert
        Assert.Throws<CryptographicException>(() => Sm4Helper.DecryptCbc(key, tamperedHex, hexString: true));
    }

    /// <summary>
    /// ECB 模式：翻转密文最后一字节后解密 → PKCS7 填充验证失败 → CryptographicException。
    /// </summary>
    [Fact]
    public void Sm4DecryptEcb_WithTamperedCiphertext_ShouldThrowCryptographicException()
    {
        // Arrange
        var key = "0123456789ABCDEF0123456789ABCDEF";
        var plainText = "SM4 ECB tampered ciphertext test with enough padding blocks!!";
#pragma warning disable CS0618
        var encryptedHex = Sm4Helper.EncryptEcb(key, plainText, hexString: true);
        var tamperedHex = TamperLastByte(encryptedHex);

        // Act & Assert
        Assert.Throws<CryptographicException>(() => Sm4Helper.DecryptEcb(key, tamperedHex, hexString: true));
#pragma warning restore CS0618
    }

    /// <summary>
    /// CBC 模式中文明文加解密往返。
    /// </summary>
    [Fact]
    public void Sm4EncryptDecryptCbc_ChineseText_ShouldRoundtrip()
    {
        // Arrange
        var key = "0123456789ABCDEF0123456789ABCDEF";
        var plainText = "SM4对称加密中文测试——游戏帧率优化";

        // Act
        var encrypted = Sm4Helper.EncryptCbc(key, plainText, hexString: true);
        var decrypted = Sm4Helper.DecryptCbc(key, encrypted, hexString: true);

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    /// <summary>
    /// CBC 模式 Emoji 明文加解密往返。
    /// </summary>
    [Fact]
    public void Sm4EncryptDecryptCbc_EmojiText_ShouldRoundtrip()
    {
        // Arrange
        var key = "0123456789ABCDEF0123456789ABCDEF";
        var plainText = "游戏 🎮 服务器 🌐 SM4 🔐";

        // Act
        var encrypted = Sm4Helper.EncryptCbc(key, plainText, hexString: true);
        var decrypted = Sm4Helper.DecryptCbc(key, encrypted, hexString: true);

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    /// <summary>
    /// ECB 模式中文明文加解密往返。
    /// </summary>
    [Fact]
    public void Sm4EncryptDecryptEcb_ChineseText_ShouldRoundtrip()
    {
        // Arrange
        var key = "0123456789ABCDEF0123456789ABCDEF";
        var plainText = "SM4-ECB中文往返测试——区块链";

        // Act
#pragma warning disable CS0618
        var encrypted = Sm4Helper.EncryptEcb(key, plainText, hexString: true);
        var decrypted = Sm4Helper.DecryptEcb(key, encrypted, hexString: true);
#pragma warning restore CS0618

        // Assert
        Assert.Equal(plainText, decrypted);
    }

    // ========================================================================
    // DsaHelper 边界测试
    // 静态 SignData(byte[], string)：catch (CryptographicException) + catch (XmlException) → return null。
    // 静态 SignData(string, string)：byte[] 版本返回 null 时包装抛 CryptographicException。
    // 实例 SignData 不接受 key 参数（构造函数已加载），无法测试无效密钥路径。
    // 以下测试全平台可运行（不依赖 DSA 密钥生成，macOS 也能跑）。
    // ========================================================================

    /// <summary>
    /// 空字符串私钥 → FromXmlString("") 抛 XmlException → catch → 返回 null。
    /// 验证空字符串不被 ArgumentNullException.ThrowIfNull 拦截，进入 catch 路径。
    /// </summary>
    [Fact]
    public void DsaSignData_ByteArray_WithEmptyStringKey_ShouldReturnNull()
    {
        // Arrange
        var dataBytes = Encoding.UTF8.GetBytes("DSA empty key test");

        // Act
        var result = DsaHelper.SignData(dataBytes, "");

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// 合法 XML 但非 DSAKeyValue 根元素 → FromXmlString 抛 CryptographicException → catch → 返回 null。
    /// </summary>
    [Fact]
    public void DsaSignData_ByteArray_WithValidXmlNotDsaKey_ShouldReturnNull()
    {
        // Arrange
        var dataBytes = Encoding.UTF8.GetBytes("DSA non-DSA XML test");
        var validXmlNotDsa = "<Root><Data>not a DSA key</Data></Root>";

        // Act
        var result = DsaHelper.SignData(dataBytes, validXmlNotDsa);

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// string 重载：空字符串私钥 → byte[] 版本返回 null → 包装抛 CryptographicException。
    /// 源码：SignData(string) 中 if (res == null) throw new CryptographicException(...)。
    /// </summary>
    [Fact]
    public void DsaSignData_String_WithEmptyStringKey_ShouldThrowCryptographicException()
    {
        // Act & Assert
        Assert.Throws<CryptographicException>(() => DsaHelper.SignData("DSA test", ""));
    }

    /// <summary>
    /// string 重载：合法 XML 但非 DSA 密钥 → byte[] 版本返回 null → 包装抛 CryptographicException。
    /// </summary>
    [Fact]
    public void DsaSignData_String_WithValidXmlNotDsaKey_ShouldThrowCryptographicException()
    {
        // Arrange
        var validXmlNotDsa = "<Root><Data>not a DSA key</Data></Root>";

        // Act & Assert
        Assert.Throws<CryptographicException>(() => DsaHelper.SignData("DSA test", validXmlNotDsa));
    }

    /// <summary>
    /// 静态 VerifyData：合法 XML 但非 DSA 公钥 → catch → 返回 false。
    /// 与 SignData 对称，验证 VerifyData 的 catch 块也正确处理无效密钥。
    /// </summary>
    [Fact]
    public void DsaVerifyData_ByteArray_WithValidXmlNotDsaKey_ShouldReturnFalse()
    {
        // Arrange
        var dataBytes = Encoding.UTF8.GetBytes("DSA verify test");
        var signatureBytes = new byte[] { 1, 2, 3, 4 };
        var validXmlNotDsa = "<Root><Data>not a DSA key</Data></Root>";

        // Act
        var result = DsaHelper.VerifyData(dataBytes, signatureBytes, validXmlNotDsa);

        // Assert
        Assert.False(result);
    }

    // ========================================================================
    // 辅助方法
    // ========================================================================

    /// <summary>
    /// 翻转 hex 字符串最后一字节的高 4 位，保持 hex 合法性。
    /// 篡改最后一个密文字节，使解密后最后一个块的填充字节为乱码，
    /// 从而触发 PKCS7 填充验证失败。
    /// </summary>
    private static string TamperLastByte(string hex)
    {
        var chars = hex.ToCharArray();
        int pos = chars.Length - 2; // 最后一字节的高 hex 字符位置
        chars[pos] = chars[pos] == '0' ? '1' : '0';
        return new string(chars);
    }
}
