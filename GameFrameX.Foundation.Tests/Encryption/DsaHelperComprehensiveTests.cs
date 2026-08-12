using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using GameFrameX.Foundation.Encryption;
using Xunit;

namespace GameFrameX.Foundation.Tests.Encryption
{
    /// <summary>
    /// DSA 数字签名全量补充测试。
    /// 覆盖现有 DsaHelperTests 未覆盖的路径：DSA 实例构造函数、Dispose、
    /// byte[] 实例重载、跨密钥验证、篡改签名、参数校验（null/空）。
    /// 注意：macOS 不支持 DSA 密钥生成（dotnet/runtime#41874），
    /// 需要 DSA 实例的测试在 macOS 上跳过。
    /// </summary>
    public class DsaHelperComprehensiveTests
    {
        private const string TestData = "Hello, DSA comprehensive test!";
        private const string LongTestData = "这是一段用于测试 DSA 签名的较长文本，包含中文、English 混合内容以及数字 1234567890，用于验证签名算法在较长输入下的正确性。";

        /// <summary>
        /// macOS 不支持 DSA 密钥生成（dotnet/runtime#41874）。
        /// </summary>
        private static bool IsDsaSupported => !RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        // ============================================================
        // 构造函数：DsaHelper(DSA dsa)
        // ============================================================

        [Fact]
        public void Constructor_WithDsaInstance_ShouldSucceed()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    Assert.NotNull(helper);
                }
            }
        }

        [Fact]
        public void Constructor_WithNullDsa_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DsaHelper((DSA)null!));
        }

        // ============================================================
        // 构造函数：DsaHelper(string key) — 参数校验
        // ============================================================

        [Fact]
        public void Constructor_WithNullKey_ShouldThrowArgumentNullException()
        {
            // ArgumentException.ThrowIfNullOrEmpty(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => new DsaHelper((string)null!));
        }

        [Fact]
        public void Constructor_WithEmptyKey_ShouldThrowArgumentException()
        {
            // ArgumentException.ThrowIfNullOrEmpty("") → ArgumentException（精确类型）
            Assert.Throws<ArgumentException>(() => new DsaHelper(""));
        }

        [Fact]
        public void Constructor_WithInvalidKey_ShouldThrow()
        {
            // 无效 XML 可能抛 XmlException / CryptographicException / PlatformNotSupportedException，取决于平台
            var ex = Record.Exception(() => new DsaHelper("not_a_valid_key"));
            Assert.NotNull(ex);
        }

        // ============================================================
        // Dispose（需要 DSA 实例，macOS 跳过）
        // ============================================================

        [Fact]
        public void Dispose_ShouldNotThrow()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                var helper = new DsaHelper(dsa);
                var ex = Record.Exception(() => helper.Dispose());
                Assert.Null(ex);
            }
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldNotThrow()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                var helper = new DsaHelper(dsa);
                var ex = Record.Exception(() =>
                {
                    helper.Dispose();
                    helper.Dispose();
                });
                Assert.Null(ex);
            }
        }

        // ============================================================
        // 实例 byte[] 重载（需要密钥，macOS 跳过）
        // ============================================================

        [Fact]
        public void InstanceSignData_WithByteArray_ShouldReturnSignature()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPair = DsaHelper.Make();
            var dataBytes = Encoding.UTF8.GetBytes(TestData);

            using (var helper = new DsaHelper(keyPair["privatekey"]))
            {
                var signature = helper.SignData(dataBytes);

                Assert.NotNull(signature);
                Assert.NotEmpty(signature);
            }
        }

        [Fact]
        public void InstanceVerifyData_WithByteArray_ShouldReturnTrueForValidSignature()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPair = DsaHelper.Make();
            var dataBytes = Encoding.UTF8.GetBytes(TestData);

            using (var helper = new DsaHelper(keyPair["privatekey"]))
            {
                var signature = helper.SignData(dataBytes);
                var isValid = helper.VerifyData(dataBytes, signature);

                Assert.True(isValid);
            }
        }

        [Fact]
        public void InstanceVerifyData_WithModifiedData_ShouldReturnFalse()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPair = DsaHelper.Make();
            var originalData = Encoding.UTF8.GetBytes(TestData);
            var modifiedData = Encoding.UTF8.GetBytes(TestData + "tampered");

            using (var helper = new DsaHelper(keyPair["privatekey"]))
            {
                var signature = helper.SignData(originalData);
                var isValid = helper.VerifyData(modifiedData, signature);

                Assert.False(isValid);
            }
        }

        [Fact]
        public void InstanceVerifyData_WithTamperedSignature_ShouldReturnFalse()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPair = DsaHelper.Make();
            var dataBytes = Encoding.UTF8.GetBytes(TestData);

            using (var helper = new DsaHelper(keyPair["privatekey"]))
            {
                var signature = helper.SignData(dataBytes);

                // 翻转第一个字节
                var tampered = (byte[])signature.Clone();
                tampered[0] ^= 0xFF;

                var isValid = helper.VerifyData(dataBytes, tampered);

                Assert.False(isValid);
            }
        }

        // ============================================================
        // 实例方法 null 参数校验（macOS 跳过，因为需要 DSA.Create()）
        // ============================================================

        [Fact]
        public void InstanceSignData_ByteArray_WithNull_ShouldThrowArgumentNullException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.SignData((byte[])null!));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_ByteArray_WithNullData_ShouldThrowArgumentNullException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData(null!, new byte[] { 1 }));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_ByteArray_WithNullSignature_ShouldThrowArgumentNullException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData(new byte[] { 1 }, null!));
                }
            }
        }

        [Fact]
        public void InstanceSignData_StringOverload_WithNull_ShouldThrowArgumentNullException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.SignData((string)null!));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_StringOverload_WithNullData_ShouldThrowArgumentNullException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData((string)null!, "c2ln"));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_StringOverload_WithNullSignature_ShouldThrowArgumentNullException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    // ThrowIfNullOrEmpty(null) → ArgumentNullException
                    Assert.Throws<ArgumentNullException>(() => helper.VerifyData("data", null!));
                }
            }
        }

        [Fact]
        public void InstanceVerifyData_StringOverload_WithEmptySignature_ShouldThrowArgumentException()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            using (var dsa = DSA.Create())
            {
                using (var helper = new DsaHelper(dsa))
                {
                    // ThrowIfNullOrEmpty("") → ArgumentException（精确类型）
                    Assert.Throws<ArgumentException>(() => helper.VerifyData("data", ""));
                }
            }
        }

        // ============================================================
        // 静态方法 — 参数校验（不需要 DSA.Create()，全平台运行）
        // ============================================================

        [Fact]
        public void StaticSignData_ByteArray_WithNullData_ShouldThrowArgumentNullException()
        {
            // ArgumentNullException.ThrowIfNull → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => DsaHelper.SignData((byte[])null!, "<key/>"));
        }

        [Fact]
        public void StaticSignData_ByteArray_WithNullKey_ShouldThrowArgumentNullException()
        {
            // ArgumentNullException.ThrowIfNull → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => DsaHelper.SignData(new byte[] { 1 }, null!));
        }

        [Fact]
        public void StaticSignData_StringOverload_WithNullData_ShouldThrowArgumentNullException()
        {
            // ArgumentNullException.ThrowIfNull → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => DsaHelper.SignData((string)null!, "<key/>"));
        }

        [Fact]
        public void StaticVerifyData_ByteArray_WithNullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DsaHelper.VerifyData(null!, new byte[] { 1 }, "<key/>"));
        }

        [Fact]
        public void StaticVerifyData_ByteArray_WithNullSignature_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DsaHelper.VerifyData(new byte[] { 1 }, null!, "<key/>"));
        }

        [Fact]
        public void StaticVerifyData_ByteArray_WithNullKey_ShouldThrowArgumentNullException()
        {
            // ThrowIfNullOrEmpty(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => DsaHelper.VerifyData(new byte[] { 1 }, new byte[] { 1 }, null!));
        }

        [Fact]
        public void StaticVerifyData_ByteArray_WithEmptyKey_ShouldThrowArgumentException()
        {
            // ThrowIfNullOrEmpty("") → ArgumentException（精确类型）
            Assert.Throws<ArgumentException>(() => DsaHelper.VerifyData(new byte[] { 1 }, new byte[] { 1 }, ""));
        }

        [Fact]
        public void StaticVerifyData_StringOverload_WithNullData_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DsaHelper.VerifyData((string)null!, "c2ln", "<key/>"));
        }

        [Fact]
        public void StaticVerifyData_StringOverload_WithNullSignature_ShouldThrowArgumentNullException()
        {
            // ThrowIfNullOrEmpty(null) → ArgumentNullException
            Assert.Throws<ArgumentNullException>(() => DsaHelper.VerifyData("data", null!, "<key/>"));
        }

        [Fact]
        public void StaticVerifyData_StringOverload_WithEmptySignature_ShouldThrowArgumentException()
        {
            // ThrowIfNullOrEmpty("") → ArgumentException（精确类型）
            Assert.Throws<ArgumentException>(() => DsaHelper.VerifyData("data", "", "<key/>"));
        }

        // ============================================================
        // 跨密钥验证（需要密钥生成，macOS 跳过）
        // ============================================================

        [Fact]
        public void VerifyData_WithDifferentKeyPair_ShouldReturnFalse()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPairA = DsaHelper.Make();
            var keyPairB = DsaHelper.Make();
            var dataBytes = Encoding.UTF8.GetBytes(TestData);

            var signature = DsaHelper.SignData(dataBytes, keyPairA["privatekey"]);

            // 用 B 的公钥验证 A 的签名 → false
            var isValid = DsaHelper.VerifyData(dataBytes, signature, keyPairB["publickey"]);

            Assert.False(isValid);
        }

        [Fact]
        public void InstanceVerifyData_WithDifferentKeyInstance_ShouldReturnFalse()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPairA = DsaHelper.Make();
            var keyPairB = DsaHelper.Make();
            var dataBytes = Encoding.UTF8.GetBytes(TestData);

            using (var helperA = new DsaHelper(keyPairA["privatekey"]))
            using (var helperB = new DsaHelper(keyPairB["publickey"]))
            {
                var signature = helperA.SignData(dataBytes);
                var isValid = helperB.VerifyData(dataBytes, signature);

                Assert.False(isValid);
            }
        }

        // ============================================================
        // 长数据往返
        // ============================================================

        [Fact]
        public void InstanceSignVerify_WithLongData_ShouldRoundtrip()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPair = DsaHelper.Make();
            var dataBytes = Encoding.UTF8.GetBytes(LongTestData);

            using (var helper = new DsaHelper(keyPair["privatekey"]))
            {
                var signature = helper.SignData(dataBytes);
                var isValid = helper.VerifyData(dataBytes, signature);

                Assert.True(isValid);
            }
        }

        [Fact]
        public void InstanceSignData_StringOverload_WithLongData_ShouldRoundtrip()
        {
            if (!IsDsaSupported)
            {
                return;
            }

            var keyPair = DsaHelper.Make();

            using (var helper = new DsaHelper(keyPair["privatekey"]))
            {
                var signature = helper.SignData(LongTestData);
                var isValid = helper.VerifyData(LongTestData, signature);

                Assert.True(isValid);
            }
        }
    }
}
