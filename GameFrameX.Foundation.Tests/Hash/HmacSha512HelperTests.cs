using System.Text;
using GameFrameX.Foundation.Hash;
using Xunit;

namespace GameFrameX.Foundation.Tests.Hash;

/// <summary>
/// HMAC-SHA512 哈希算法单元测试
/// </summary>
public class HmacSha512HelperTests
{
    private const string TestString = "Hello, World!";
    private const string TestStringChinese = "你好，世界！";
    private const string EmptyString = "";
    private const string LongString = "这是一个很长的测试字符串，用来测试HMAC-SHA512哈希算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。";
    private const string TestKey = "test-key";
    private const string TestKeyChinese = "测试密钥";
    private const string EmptyKey = "";
    private const string LongKey = "这是一个很长的测试密钥，用来测试HMAC-SHA512算法在使用较长密钥时的行为和正确性。";

    // ==================== Hash(string, string) ====================

    [Fact]
    public void Hash_ValidStringAndKey_ShouldReturnConsistentHash()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;

        // Act
        var hash1 = HmacSha512Helper.Hash(message, key);
        var hash2 = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash1);
        Assert.NotEmpty(hash1);
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash_EmptyMessage_ShouldReturnValidHash()
    {
        // Arrange
        var message = EmptyString;
        var key = TestKey;

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_EmptyKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestString;
        var key = EmptyKey;

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_EmptyMessageAndKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = EmptyString;
        var key = EmptyKey;

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_ChineseMessageAndKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = TestStringChinese;
        var key = TestKeyChinese;

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_LongMessageAndKey_ShouldReturnValidHash()
    {
        // Arrange
        var message = LongString;
        var key = LongKey;

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
    }

    [Fact]
    public void Hash_UnicodeCharacters_ShouldReturnValidHash()
    {
        // Arrange
        var message = "🌟🚀💻🎉";
        var key = "🔑🛡️🔐";

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(64, bytes.Length);
    }

    [Fact]
    public void Hash_DifferentMessages_ShouldReturnDifferentHashes()
    {
        // Arrange
        var key = TestKey;

        // Act
        var hash1 = HmacSha512Helper.Hash("Message 1", key);
        var hash2 = HmacSha512Helper.Hash("Message 2", key);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_DifferentKeys_ShouldReturnDifferentHashes()
    {
        // Arrange
        var message = TestString;

        // Act
        var hash1 = HmacSha512Helper.Hash(message, "Key 1");
        var hash2 = HmacSha512Helper.Hash(message, "Key 2");

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void Hash_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        string message = null;
        var key = TestKey;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Hash(message, key));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Hash_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var message = TestString;
        string key = null;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Hash(message, key));
        Assert.Equal("key", ex.ParamName);
    }

    [Fact]
    public void Hash_ValidBase64Output_ShouldDecodeTo64Bytes()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(64, bytes.Length); // HMAC-SHA512 输出应为64字节
    }

    [Theory]
    [InlineData("test", "key")]
    [InlineData("Hello World", "secret")]
    [InlineData("测试消息", "密钥")]
    [InlineData("", "key")]
    [InlineData("message", "")]
    [InlineData("", "")]
    public void Hash_VariousInputs_ShouldReturn64ByteHashes(string message, string key)
    {
        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(64, bytes.Length);
    }

    // ==================== Hash(byte[], byte[]) ====================

    [Fact]
    public void Hash_ByteArray_ShouldMatchStringOverload()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var expected = HmacSha512Helper.Hash(TestString, TestKey);

        // Act
        var hash = HmacSha512Helper.Hash(messageBytes, keyBytes);

        // Assert
        Assert.Equal(expected, hash);
    }

    [Fact]
    public void Hash_NullMessageBytes_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] message = null;
        var key = Encoding.UTF8.GetBytes(TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Hash(message, key));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Hash_NullKeyBytes_ShouldThrowArgumentNullException()
    {
        // Arrange
        var message = Encoding.UTF8.GetBytes(TestString);
        byte[] key = null;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Hash(message, key));
        Assert.Equal("key", ex.ParamName);
    }

    [Fact]
    public void Hash_EmptyByteArrays_ShouldReturnValidHash()
    {
        // Arrange
        var message = Array.Empty<byte>();
        var key = Array.Empty<byte>();

        // Act
        var hash = HmacSha512Helper.Hash(message, key);

        // Assert
        Assert.NotNull(hash);
        Assert.NotEmpty(hash);
        var bytes = Convert.FromBase64String(hash);
        Assert.Equal(64, bytes.Length);
    }

    // ==================== Hash(Stream, byte[]) ====================

    [Fact]
    public void Hash_Stream_ShouldMatchByteArrayOverload()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var expected = HmacSha512Helper.Hash(messageBytes, keyBytes);

        // Act
        string hash;
        using (var stream = new MemoryStream(messageBytes))
        {
            hash = HmacSha512Helper.Hash(stream, keyBytes);
        }

        // Assert
        Assert.Equal(expected, hash);
    }

    [Fact]
    public void Hash_NullMessageStream_ShouldThrowArgumentNullException()
    {
        // Arrange
        Stream message = null;
        var key = Encoding.UTF8.GetBytes(TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Hash(message, key));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Hash_NullKeyStream_ShouldThrowArgumentNullException()
    {
        // Arrange
        using (var message = new MemoryStream(Encoding.UTF8.GetBytes(TestString)))
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Hash(message, null));
            Assert.Equal("key", ex.ParamName);
        }
    }

    // ==================== HashAsync(Stream, byte[], CancellationToken) ====================

    [Fact]
    public async Task HashAsync_ValidStream_ShouldReturnCorrectHash()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var expected = HmacSha512Helper.Hash(messageBytes, keyBytes);

        // Act
        string hash;
        using (var stream = new MemoryStream(messageBytes))
        {
            hash = await HmacSha512Helper.HashAsync(stream, keyBytes);
        }

        // Assert
        Assert.Equal(expected, hash);
    }

    [Fact]
    public async Task HashAsync_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        Stream message = null;
        var key = Encoding.UTF8.GetBytes(TestKey);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => HmacSha512Helper.HashAsync(message, key));
    }

    [Fact]
    public async Task HashAsync_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        byte[] key = null;

        // Act & Assert
        using (var stream = new MemoryStream(messageBytes))
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => HmacSha512Helper.HashAsync(stream, key));
        }
    }

    [Fact]
    public async Task HashAsync_WithCanceledToken_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        using (var cts = new CancellationTokenSource())
        {
            cts.Cancel();

            // Act & Assert
            using (var stream = new MemoryStream(messageBytes))
            {
                await Assert.ThrowsAnyAsync<OperationCanceledException>(
                    () => HmacSha512Helper.HashAsync(stream, keyBytes, cts.Token));
            }
        }
    }

    // ==================== Verify(string, string, string) ====================

    [Fact]
    public void Verify_StringOverload_CorrectHash_ShouldReturnTrue()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;
        var hash = HmacSha512Helper.Hash(message, key);

        // Act
        var result = HmacSha512Helper.Verify(message, key, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_StringOverload_TamperedHash_ShouldReturnFalse()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;
        var tamperedHash = HmacSha512Helper.Hash("different message", key);

        // Act
        var result = HmacSha512Helper.Verify(message, key, tamperedHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_StringOverload_InvalidBase64_ShouldReturnFalse()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;

        // Act
        var result = HmacSha512Helper.Verify(message, key, "not-base64");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_StringOverload_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        string message = null;
        var key = TestKey;
        var hash = HmacSha512Helper.Hash(TestString, TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, hash));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Verify_StringOverload_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var message = TestString;
        string key = null;
        var hash = HmacSha512Helper.Hash(TestString, TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, hash));
        Assert.Equal("key", ex.ParamName);
    }

    [Fact]
    public void Verify_StringOverload_NullExpectedHash_ShouldThrowArgumentNullException()
    {
        // Arrange
        var message = TestString;
        var key = TestKey;

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, null));
        Assert.Equal("expectedHash", ex.ParamName);
    }

    // ==================== Verify(byte[], byte[], string) ====================

    [Fact]
    public void Verify_ByteArrayOverload_CorrectHash_ShouldReturnTrue()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var hash = HmacSha512Helper.Hash(messageBytes, keyBytes);

        // Act
        var result = HmacSha512Helper.Verify(messageBytes, keyBytes, hash);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_ByteArrayOverload_TamperedHash_ShouldReturnFalse()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var tamperedHash = HmacSha512Helper.Hash(Encoding.UTF8.GetBytes("different"), keyBytes);

        // Act
        var result = HmacSha512Helper.Verify(messageBytes, keyBytes, tamperedHash);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_ByteArrayOverload_InvalidBase64_ShouldReturnFalse()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        // Act
        var result = HmacSha512Helper.Verify(messageBytes, keyBytes, "not-base64");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_ByteArrayOverload_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        byte[] message = null;
        var key = Encoding.UTF8.GetBytes(TestKey);
        var hash = HmacSha512Helper.Hash(TestString, TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, hash));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Verify_ByteArrayOverload_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        var message = Encoding.UTF8.GetBytes(TestString);
        byte[] key = null;
        var hash = HmacSha512Helper.Hash(TestString, TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, hash));
        Assert.Equal("key", ex.ParamName);
    }

    [Fact]
    public void Verify_ByteArrayOverload_NullExpectedHash_ShouldThrowArgumentNullException()
    {
        // Arrange
        var message = Encoding.UTF8.GetBytes(TestString);
        var key = Encoding.UTF8.GetBytes(TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, null));
        Assert.Equal("expectedHash", ex.ParamName);
    }

    // ==================== Verify(Stream, byte[], string) ====================

    [Fact]
    public void Verify_StreamOverload_CorrectHash_ShouldReturnTrue()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var hash = HmacSha512Helper.Hash(messageBytes, keyBytes);

        // Act
        bool result;
        using (var stream = new MemoryStream(messageBytes))
        {
            result = HmacSha512Helper.Verify(stream, keyBytes, hash);
        }

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Verify_StreamOverload_TamperedHash_ShouldReturnFalse()
    {
        // Arrange
        var messageBytes = Encoding.UTF8.GetBytes(TestString);
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var tamperedHash = HmacSha512Helper.Hash(Encoding.UTF8.GetBytes("different"), keyBytes);

        // Act
        bool result;
        using (var stream = new MemoryStream(messageBytes))
        {
            result = HmacSha512Helper.Verify(stream, keyBytes, tamperedHash);
        }

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Verify_StreamOverload_NullMessage_ShouldThrowArgumentNullException()
    {
        // Arrange
        Stream message = null;
        var key = Encoding.UTF8.GetBytes(TestKey);
        var hash = HmacSha512Helper.Hash(TestString, TestKey);

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, hash));
        Assert.Equal("message", ex.ParamName);
    }

    [Fact]
    public void Verify_StreamOverload_NullKey_ShouldThrowArgumentNullException()
    {
        // Arrange
        using (var message = new MemoryStream(Encoding.UTF8.GetBytes(TestString)))
        {
            var hash = HmacSha512Helper.Hash(TestString, TestKey);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, null, hash));
            Assert.Equal("key", ex.ParamName);
        }
    }

    [Fact]
    public void Verify_StreamOverload_NullExpectedHash_ShouldThrowArgumentNullException()
    {
        // Arrange
        using (var message = new MemoryStream(Encoding.UTF8.GetBytes(TestString)))
        {
            var key = Encoding.UTF8.GetBytes(TestKey);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => HmacSha512Helper.Verify(message, key, null));
            Assert.Equal("expectedHash", ex.ParamName);
        }
    }
}
