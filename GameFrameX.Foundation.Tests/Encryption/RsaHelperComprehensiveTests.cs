using System;
using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Encryption;
using Xunit;

namespace GameFrameX.Foundation.Tests.Encryption
{
    /// <summary>
    /// RSA 加解密/签名全量补充测试。
    /// 重点覆盖现有 RsaHelperTests 未覆盖的路径：
    /// - Sign/Verify（PKCS#8/PKCS#1 Base64 密钥格式）
    /// - EncryptBase64/DecryptBase64（Base64 密钥 + 分块加解密）
    /// - RsaHelper(RSA rsa) 构造函数
    /// - 构造函数 null/空校验
    /// - Dispose
    /// - 实例 byte[] 重载
    /// - 跨密钥失败、篡改验证、null 边界。
    /// </summary>
    public class RsaHelperComprehensiveTests
    {
        private const string TestData = "RSA comprehensive test data — 中文 + English.";
        private const string ShortData = "Hi";

        // ============================================================
        // 辅助：生成不同格式的密钥对
        // ============================================================

        private static (string privPkcs8, string pubSpki, string privPkcs1, string pubPkcs1)
            GenerateAllKeyFormats()
        {
            using (var rsa = RSA.Create())
            {
                return (
                    Convert.ToBase64String(rsa.ExportPkcs8PrivateKey()),
                    Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo()),
                    Convert.ToBase64String(rsa.ExportRSAPrivateKey()),
                    Convert.ToBase64String(rsa.ExportRSAPublicKey())
                );
            }
        }

        // ============================================================
        // Sign / Verify — PKCS#8 格式
        // ============================================================

        [Fact]
        public void Sign_WithPkcs8PrivateKey_ShouldReturnBase64Signature()
        {
            var keys = GenerateAllKeyFormats();

            var signature = RsaHelper.Sign(keys.privPkcs8, TestData);

            Assert.NotNull(signature);
            Assert.NotEmpty(signature);
            // 验证是有效 Base64
            Convert.FromBase64String(signature);
        }

        [Fact]
        public void SignVerify_WithPkcs8Keys_ShouldRoundtrip()
        {
            var keys = GenerateAllKeyFormats();

            var signature = RsaHelper.Sign(keys.privPkcs8, TestData);
            var isValid = RsaHelper.Verify(keys.pubSpki, TestData, signature);

            Assert.True(isValid);
        }

        // ============================================================
        // Sign / Verify — PKCS#1 格式（验证 fallback 路径）
        // ============================================================

        [Fact]
        public void SignVerify_WithPkcs1Keys_ShouldRoundtrip()
        {
            var keys = GenerateAllKeyFormats();

            var signature = RsaHelper.Sign(keys.privPkcs1, TestData);
            var isValid = RsaHelper.Verify(keys.pubPkcs1, TestData, signature);

            Assert.True(isValid);
        }

        [Fact]
        public void Sign_WithPkcs1Key_ShouldProduceSameResultAsPkcs8FromSameKey()
        {
            var keys = GenerateAllKeyFormats();

            // 两个格式编码的是同一把密钥，因此验签结果应该一致
            var sigPkcs8 = RsaHelper.Sign(keys.privPkcs8, TestData);
            var sigPkcs1 = RsaHelper.Sign(keys.privPkcs1, TestData);

            // RSA PKCS#1 v1.5 签名是确定性的，结果应相同
            Assert.Equal(sigPkcs8, sigPkcs1);

            // 两种签名都能验过
            Assert.True(RsaHelper.Verify(keys.pubSpki, TestData, sigPkcs8));
            Assert.True(RsaHelper.Verify(keys.pubSpki, TestData, sigPkcs1));
        }

        // ============================================================
        // Sign / Verify — 篡改验证
        // ============================================================

        [Fact]
        public void Verify_WithTamperedContent_ShouldReturnFalse()
        {
            var keys = GenerateAllKeyFormats();

            var signature = RsaHelper.Sign(keys.privPkcs8, TestData);
            var isValid = RsaHelper.Verify(keys.pubSpki, TestData + "tampered", signature);

            Assert.False(isValid);
        }

        [Fact]
        public void Verify_WithTamperedSignature_ShouldReturnFalse()
        {
            var keys = GenerateAllKeyFormats();

            var signature = RsaHelper.Sign(keys.privPkcs8, TestData);
            var bytes = Convert.FromBase64String(signature);
            bytes[0] ^= 0xFF;
            var tamperedSig = Convert.ToBase64String(bytes);

            var isValid = RsaHelper.Verify(keys.pubSpki, TestData, tamperedSig);

            Assert.False(isValid);
        }

        [Fact]
        public void Verify_WithDifferentKeyPair_ShouldReturnFalse()
        {
            var keysA = GenerateAllKeyFormats();
            var keysB = GenerateAllKeyFormats();

            var signature = RsaHelper.Sign(keysA.privPkcs8, TestData);
            var isValid = RsaHelper.Verify(keysB.pubSpki, TestData, signature);

            Assert.False(isValid);
        }

        // ============================================================
        // Sign / Verify — 参数校验
        // 注意：ThrowIfNullOrEmpty(null) → ArgumentNullException；ThrowIfNullOrEmpty("") → ArgumentException
        // xUnit Assert.Throws<T> 精确匹配类型（不含派生类），需区分。
        // ============================================================

        [Fact]
        public void Sign_WithNullPrivateKey_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Sign(null!, TestData));
        }

        [Fact]
        public void Sign_WithEmptyPrivateKey_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RsaHelper.Sign("", TestData));
        }

        [Fact]
        public void Sign_WithNullContent_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Sign("dGVzdA==", null!));
        }

        [Fact]
        public void Sign_WithEmptyContent_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RsaHelper.Sign("dGVzdA==", ""));
        }

        [Fact]
        public void Verify_WithNullPublicKey_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Verify(null!, TestData, "dGVzdA=="));
        }

        [Fact]
        public void Verify_WithNullContent_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Verify("dGVzdA==", null!, "dGVzdA=="));
        }

        [Fact]
        public void Verify_WithNullSign_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Verify("dGVzdA==", TestData, null!));
        }

        // ============================================================
        // EncryptBase64 / DecryptBase64 — PKCS#8 格式
        // ============================================================

        [Fact]
        public void EncryptBase64DecryptBase64_WithPkcs8Keys_ShouldRoundtrip()
        {
            var keys = GenerateAllKeyFormats();

            var encrypted = RsaHelper.EncryptBase64(keys.pubSpki, TestData);
            var decrypted = RsaHelper.DecryptBase64(keys.privPkcs8, encrypted);

            Assert.Equal(TestData, decrypted);
        }

        [Fact]
        public void EncryptBase64_WithPkcs8Key_ShouldReturnBase64String()
        {
            var keys = GenerateAllKeyFormats();

            var encrypted = RsaHelper.EncryptBase64(keys.pubSpki, ShortData);

            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
            Assert.NotEqual(ShortData, encrypted);
            Convert.FromBase64String(encrypted);
        }

        // ============================================================
        // EncryptBase64 / DecryptBase64 — PKCS#1 格式（fallback 路径）
        // ============================================================

        [Fact]
        public void EncryptBase64DecryptBase64_WithPkcs1Keys_ShouldRoundtrip()
        {
            var keys = GenerateAllKeyFormats();

            var encrypted = RsaHelper.EncryptBase64(keys.pubPkcs1, TestData);
            var decrypted = RsaHelper.DecryptBase64(keys.privPkcs1, encrypted);

            Assert.Equal(TestData, decrypted);
        }

        // ============================================================
        // EncryptBase64 / DecryptBase64 — 大数据分块加解密
        // ============================================================

        [Fact]
        public void EncryptBase64DecryptBase64_WithLargeData_ShouldRoundtrip()
        {
            var keys = GenerateAllKeyFormats();

            // 构造超过单个 OAEP-SHA256 块上限的数据（2048 位密钥上限约 190 字节）
            var sb = new StringBuilder();
            for (int i = 0; i < 500; i++)
            {
                sb.Append("BigBlockData_");
            }
            var largeData = sb.ToString();

            Assert.True(Encoding.UTF8.GetByteCount(largeData) > 256);

            var encrypted = RsaHelper.EncryptBase64(keys.pubSpki, largeData);
            var decrypted = RsaHelper.DecryptBase64(keys.privPkcs8, encrypted);

            Assert.Equal(largeData, decrypted);
        }

        [Fact]
        public void EncryptBase64DecryptBase64_WithExactBoundaryData_ShouldRoundtrip()
        {
            var keys = GenerateAllKeyFormats();

            // 构造接近块上限的数据（测试边界条件）
            var data = new string('x', 190);

            var encrypted = RsaHelper.EncryptBase64(keys.pubSpki, data);
            var decrypted = RsaHelper.DecryptBase64(keys.privPkcs8, encrypted);

            Assert.Equal(data, decrypted);
        }

        // ============================================================
        // EncryptBase64 / DecryptBase64 — 参数校验
        // ============================================================

        [Fact]
        public void EncryptBase64_WithNullPublicKey_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.EncryptBase64(null!, TestData));
        }

        [Fact]
        public void EncryptBase64_WithEmptyContent_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RsaHelper.EncryptBase64("dGVzdA==", ""));
        }

        [Fact]
        public void DecryptBase64_WithNullPrivateKey_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.DecryptBase64(null!, "dGVzdA=="));
        }

        [Fact]
        public void DecryptBase64_WithEmptyContent_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => RsaHelper.DecryptBase64("dGVzdA==", ""));
        }

        [Fact]
        public void DecryptBase64_WithMismatchedKey_ShouldThrowCryptographicException()
        {
            var keysA = GenerateAllKeyFormats();
            var keysB = GenerateAllKeyFormats();

            var encrypted = RsaHelper.EncryptBase64(keysA.pubSpki, TestData);

            // 用 B 的私钥解密 A 的公钥加密的数据 → OAEP unpadding 失败
            Assert.ThrowsAny<CryptographicException>(() => RsaHelper.DecryptBase64(keysB.privPkcs8, encrypted));
        }

        // ============================================================
        // 构造函数：RsaHelper(RSA rsa)
        // ============================================================

        [Fact]
        public void Constructor_WithRsaInstance_ShouldSucceed()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.NotNull(helper);
                }
            }
        }

        [Fact]
        public void Constructor_WithNullRsa_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RsaHelper((RSA)null!));
        }

        // ============================================================
        // 构造函数：RsaHelper(string key) — null/空校验
        // ============================================================

        [Fact]
        public void Constructor_WithNullKey_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RsaHelper((string)null!));
        }

        [Fact]
        public void Constructor_WithEmptyKey_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new RsaHelper(""));
        }

        // ============================================================
        // Dispose
        // ============================================================

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            using (var rsa = RSA.Create())
            {
                var helper = new RsaHelper(rsa);
                var ex = Record.Exception(() => helper.Dispose());
                Assert.Null(ex);
            }
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            using (var rsa = RSA.Create())
            {
                var helper = new RsaHelper(rsa);
                var ex = Record.Exception(() =>
                {
                    helper.Dispose();
                    helper.Dispose();
                });
                Assert.Null(ex);
            }
        }

        // ============================================================
        // 实例 byte[] 重载（通过 RSA 实例构造）
        // ============================================================

        [Fact]
        public void InstanceEncrypt_WithByteArray_ShouldReturnEncryptedBytes()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    var data = Encoding.UTF8.GetBytes(ShortData);

                    var encrypted = helper.Encrypt(data);

                    Assert.NotNull(encrypted);
                    Assert.NotEmpty(encrypted);
                    Assert.NotEqual(data, encrypted);
                }
            }
        }

        [Fact]
        public void InstanceDecrypt_WithByteArray_ShouldReturnOriginalBytes()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    var data = Encoding.UTF8.GetBytes(ShortData);

                    var encrypted = helper.Encrypt(data);
                    var decrypted = helper.Decrypt(encrypted);

                    Assert.Equal(data, decrypted);
                }
            }
        }

        [Fact]
        public void InstanceEncryptDecrypt_StringRoundtrip_ShouldReturnOriginalText()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    var encrypted = helper.Encrypt(TestData);
                    var decrypted = helper.Decrypt(encrypted);

                    Assert.Equal(TestData, decrypted);
                }
            }
        }

        [Fact]
        public void InstanceSignVerify_ByteArrayRoundtrip_ShouldSucceed()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    var data = Encoding.UTF8.GetBytes(TestData);

                    var signature = helper.SignData(data);
                    var isValid = helper.VerifyData(data, signature);

                    Assert.True(isValid);
                }
            }
        }

        [Fact]
        public void InstanceSignVerify_StringRoundtrip_ShouldSucceed()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    var signature = helper.SignData(TestData);
                    var isValid = helper.VerifyData(TestData, signature);

                    Assert.True(isValid);
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_WithModifiedData_ShouldReturnFalse()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    var signature = helper.SignData(TestData);
                    var isValid = helper.VerifyData(TestData + "modified", signature);

                    Assert.False(isValid);
                }
            }
        }

        // ============================================================
        // 实例方法 — null 参数校验
        // ============================================================

        [Fact]
        public void InstanceEncrypt_ByteArray_WithNull_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.Encrypt((byte[])null!));
                }
            }
        }

        [Fact]
        public void InstanceDecrypt_ByteArray_WithNull_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.Decrypt((byte[])null!));
                }
            }
        }

        [Fact]
        public void InstanceEncrypt_String_WithNull_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    // ThrowIfNullOrEmpty(null) → ArgumentNullException
                    Assert.Throws<ArgumentNullException>(() => helper.Encrypt((string)null!));
                }
            }
        }

        [Fact]
        public void InstanceEncrypt_String_WithEmpty_ShouldThrowArgumentException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    // ThrowIfNullOrEmpty("") → ArgumentException
                    Assert.Throws<ArgumentException>(() => helper.Encrypt(""));
                }
            }
        }

        [Fact]
        public void InstanceDecrypt_String_WithNull_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.Decrypt((string)null!));
                }
            }
        }

        [Fact]
        public void InstanceDecrypt_String_WithEmpty_ShouldThrowArgumentException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentException>(() => helper.Decrypt(""));
                }
            }
        }

        [Fact]
        public void InstanceSignData_ByteArray_WithNull_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.SignData((byte[])null!));
                }
            }
        }

        [Fact]
        public void InstanceSignData_String_WithNull_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.SignData((string)null!));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_ByteArray_WithNullData_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData((byte[])null!, new byte[] { 1 }));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_ByteArray_WithNullSignature_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData(new byte[] { 1 }, null!));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_String_WithNullData_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData((string)null!, "dGVzdA=="));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_String_WithNullSignature_ShouldThrowArgumentNullException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    // ThrowIfNullOrEmpty(null) → ArgumentNullException
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData("data", null!));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_String_WithEmptySignature_ShouldThrowArgumentException()
        {
            using (var rsa = RSA.Create())
            {
                using (var helper = new RsaHelper(rsa))
                {
                    // ThrowIfNullOrEmpty("") → ArgumentException
                    Assert.Throws<ArgumentException>(() => helper.VerifyData("data", ""));
                }
            }
        }

        // ============================================================
        // 静态方法 — XML 格式补充参数校验
        // ============================================================

        [Fact]
        public void StaticEncrypt_StringOverload_WithNullContent_ShouldThrowArgumentNullException()
        {
            // ThrowIfNullOrEmpty(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Encrypt("dGVzdA==", (string)null!));
        }

        [Fact]
        public void StaticEncrypt_ByteArrayOverload_WithNullContent_ShouldThrowArgumentNullException()
        {
            // ThrowIfNull(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Encrypt("dGVzdA==", (byte[])null!));
        }

        [Fact]
        public void StaticDecrypt_StringOverload_WithNullContent_ShouldThrowArgumentNullException()
        {
            // ThrowIfNullOrEmpty(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Decrypt("dGVzdA==", (string)null!));
        }

        [Fact]
        public void StaticDecrypt_ByteArrayOverload_WithNullContent_ShouldThrowArgumentNullException()
        {
            // ThrowIfNull(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => RsaHelper.Decrypt("dGVzdA==", (byte[])null!));
        }

        [Fact]
        public void StaticSignData_ByteArray_WithNullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.SignData((byte[])null!, "dGVzdA=="));
        }

        [Fact]
        public void StaticSignData_String_WithNullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.SignData((string)null!, "dGVzdA=="));
        }

        [Fact]
        public void StaticVerifyData_ByteArray_WithNullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.VerifyData(null!, new byte[] { 1 }, "dGVzdA=="));
        }

        [Fact]
        public void StaticVerifyData_ByteArray_WithNullSignature_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.VerifyData(new byte[] { 1 }, null!, "dGVzdA=="));
        }

        [Fact]
        public void StaticRsaVerifyData_WithNullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RsaHelper.RsaVerifyData(null!, "dGVzdA==", "dGVzdA=="));
        }

        [Fact]
        public void StaticRsaVerifyData_WithNullSignature_ShouldThrowArgumentNullException()
        {
            // ThrowIfNullOrEmpty(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => RsaHelper.RsaVerifyData("data", null!, "dGVzdA=="));
        }
    }
}
