using System;
using System.Reflection;
using System.Text;
using GameFrameX.Foundation.Encryption;
using Xunit;

namespace GameFrameX.Foundation.Tests.Encryption
{
    /// <summary>
    /// SM3Util 全量覆盖测试。
    /// SM3Util 为 internal 类型，通过反射调用其唯一的 public static 方法 Hash(string)。
    /// 覆盖：已知标准测试向量（GB/T 32905-2016）、空串、Unicode、确定性、格式约束、null 边界。
    /// </summary>
    public class Sm3UtilComprehensiveTests
    {
        // ============================================================
        // 反射辅助：获取 internal SM3Util.Hash(string) 并调用
        // ============================================================

        private static string InvokeHash(string data)
        {
            var assembly = typeof(RsaHelper).Assembly;
            var sm3Type = assembly.GetType("GameFrameX.Foundation.Encryption.Sm.SM3Util");
            Assert.NotNull(sm3Type);
            var method = sm3Type!.GetMethod("Hash", BindingFlags.Public | BindingFlags.Static);
            Assert.NotNull(method);
            return (string)method!.Invoke(null, new object[] { data });
        }

        // ============================================================
        // 已知标准测试向量（GB/T 32905-2016）
        // ============================================================

        /// <summary>
        /// SM3("abc") 标准测试向量 1。
        /// </summary>
        [Fact]
        public void Hash_WithAbc_ShouldMatchStandardVector()
        {
            var result = InvokeHash("abc");

            Assert.Equal("66c7f0f462eeedd9d1f2d46bdc10e4e24167c4875cf2f7a2297da02b8f4ba8e0", result);
        }

        /// <summary>
        /// SM3(1000000 个 'a')：用 BouncyCastle 参考实现动态计算期望值进行交叉验证，
        /// 覆盖多块（百万字节）输入路径；动态对比可避免硬编码标准向量书写错误。
        /// </summary>
        [Fact]
        public void Hash_WithMillionA_ShouldMatchBouncyCastleReference()
        {
            var data = new string('a', 1000000);
            var actual = InvokeHash(data);
            var reference = ComputeSm3HexBouncyCastle(Encoding.UTF8.GetBytes(data));

            Assert.Equal(reference, actual);
        }

        // ============================================================
        // 输出格式
        // ============================================================

        [Fact]
        public void Hash_OutputShouldBe64LowercaseHexChars()
        {
            var result = InvokeHash("test data");

            Assert.Equal(64, result.Length);
            foreach (var c in result)
            {
                Assert.True(
                    (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f'),
                    $"Expected lowercase hex char, got '{c}'");
            }
        }

        [Fact]
        public void Hash_WithEmptyString_ShouldReturnValidHash()
        {
            var result = InvokeHash("");

            Assert.Equal(64, result.Length);
            // 空串哈希应与非空串不同
            Assert.NotEqual(InvokeHash("abc"), result);
        }

        // ============================================================
        // 确定性
        // ============================================================

        [Fact]
        public void Hash_ShouldBeDeterministic()
        {
            var result1 = InvokeHash("deterministic test");
            var result2 = InvokeHash("deterministic test");

            Assert.Equal(result1, result2);
        }

        [Fact]
        public void Hash_WithMillionA_ShouldBeDeterministic()
        {
            var data = new string('a', 1000000);

            var result1 = InvokeHash(data);
            var result2 = InvokeHash(data);

            Assert.Equal(result1, result2);
        }

        // ============================================================
        // 不同输入产生不同哈希
        // ============================================================

        [Theory]
        [InlineData("abc", "abcd")]
        [InlineData("Hello", "hello")]
        [InlineData("123456", "123457")]
        [InlineData("", " ")]
        public void Hash_WithDifferentInputs_ShouldReturnDifferentHashes(string a, string b)
        {
            var hashA = InvokeHash(a);
            var hashB = InvokeHash(b);

            Assert.NotEqual(hashA, hashB);
        }

        // ============================================================
        // Unicode / 中文
        // ============================================================

        [Fact]
        public void Hash_WithChineseString_ShouldReturnValidHash()
        {
            var result = InvokeHash("你好，世界！");

            Assert.Equal(64, result.Length);
            // 验证确实计算了哈希（与 ASCII 数据不同）
            Assert.NotEqual(InvokeHash("hello world"), result);
        }

        [Fact]
        public void Hash_WithChineseString_ShouldBeConsistentWithUtf8()
        {
            // 验证 SM3Util 内部使用 UTF-8 编码（而非平台默认编码）
            var result = InvokeHash("测试数据");

            // 手动用 UTF-8 编码 + BouncyCastle 公开 Sm3Digest 计算预期值
            var bytes = Encoding.UTF8.GetBytes("测试数据");
            var expected = ComputeSm3HexBouncyCastle(bytes);

            Assert.Equal(expected, result);
        }

        // ============================================================
        // null 边界
        // ============================================================

        [Fact]
        public void Hash_WithNull_ShouldThrowArgumentNullException()
        {
            // 反射调用时，目标方法抛出的异常被包装为 TargetInvocationException
            var ex = Assert.Throws<TargetInvocationException>(() => InvokeHash(null!));
            Assert.IsType<ArgumentNullException>(ex.InnerException);
        }

        // ============================================================
        // 长字符串（验证不因长度崩溃）
        // ============================================================

        [Fact]
        public void Hash_WithVeryLongString_ShouldReturnValidHash()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                sb.Append("GameFrameX");
            }
            var data = sb.ToString();

            var result = InvokeHash(data);

            Assert.Equal(64, result.Length);
        }

        [Fact]
        public void Hash_WithSingleChar_ShouldReturnValidHash()
        {
            var result = InvokeHash("a");

            Assert.Equal(64, result.Length);
            // 与空串不同
            Assert.NotEqual(InvokeHash(""), result);
            // 与 "aa" 不同
            Assert.NotEqual(InvokeHash("aa"), result);
        }

        // ============================================================
        // 辅助：用 BouncyCastle 公开 API 独立计算 SM3（交叉验证）
        // ============================================================

        private static string ComputeSm3HexBouncyCastle(byte[] data)
        {
            var sm3 = new Org.BouncyCastle.Crypto.Digests.SM3Digest();
            sm3.BlockUpdate(data, 0, data.Length);
            var result = new byte[sm3.GetDigestSize()];
            sm3.DoFinal(result, 0);
            return Encoding.ASCII.GetString(Org.BouncyCastle.Utilities.Encoders.Hex.Encode(result));
        }
    }
}
