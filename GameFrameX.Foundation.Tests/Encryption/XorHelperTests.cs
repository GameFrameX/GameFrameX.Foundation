using System.Text;
using Xunit;
using GameFrameX.Foundation.Encryption;

namespace GameFrameX.Foundation.Tests.Encryption;

/// <summary>
/// XOR 加密解密单元测试
/// </summary>
public class XorHelperTests
{
    private const string TestData = "Hello, World! 这是一个测试字符串。";
    private const string ShortTestData = "Test";
    private const string LongTestData = "这是一个很长的测试字符串，用来测试XOR加密算法在处理较长文本时的性能和正确性。包含中文字符和英文字符以及数字123456789。XOR是一种简单但有效的加密方法。";
    private const string EmptyString = "";
    private const string TestKey = "MySecretKey123";
    private const string ShortKey = "Key";
    private const string LongKey = "ThisIsAVeryLongSecretKeyForTesting123456789";
    private const string SingleCharKey = "K";

    [Fact]
    public void GetXorBytes_WithValidKey_ShouldReturnEncryptedBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);

        Assert.NotNull(encryptedBytes);
        Assert.NotEmpty(encryptedBytes);
        Assert.NotEqual(dataBytes, encryptedBytes);
    }

    [Fact]
    public void GetXorBytes_TwiceWithSameKey_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_Roundtrip_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithLongData_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(LongTestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithEmptyData_ShouldReturnEmptyArray()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(EmptyString);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithShortKey_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(ShortKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithLongKey_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(LongKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithSingleCharKey_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(SingleCharKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetQuickXorBytes_WithValidKey_ShouldReturnEncryptedBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        var encryptedBytes = XorHelper.GetQuickXorBytes(dataBytes, keyBytes);

        Assert.NotNull(encryptedBytes);
        Assert.NotEmpty(encryptedBytes);
        Assert.Equal(dataBytes.Length, encryptedBytes.Length);
        Assert.NotEqual(dataBytes, encryptedBytes);
    }

    [Fact]
    public void GetQuickXorBytes_TwiceWithSameKey_ShouldReturnOriginalBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData);

        var encryptedBytes = XorHelper.GetQuickXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetQuickXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    /// <summary>
    /// Quick 模式只加密前 220 字节，超出部分应保持不变
    /// </summary>
    [Fact]
    public void GetQuickXorBytes_DataLongerThanQuickEncryptLength_ShouldOnlyEncryptFirst220Bytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        // 构造超过 220 字节的数据
        var dataBytes = new byte[300];
        for (int i = 0; i < dataBytes.Length; i++) dataBytes[i] = (byte)(i % 256);

        var encryptedBytes = XorHelper.GetQuickXorBytes(dataBytes, keyBytes);

        // 前 220 字节应被加密（与原始不同）
        Assert.NotEqual(dataBytes[..220], encryptedBytes[..220]);
        // 220 字节之后应保持不变
        Assert.Equal(dataBytes[220..], encryptedBytes[220..]);
    }

    /// <summary>
    /// Quick 模式数据短于 220 字节时，全部数据都应被加密
    /// </summary>
    [Fact]
    public void GetQuickXorBytes_DataShorterThanQuickEncryptLength_ShouldEncryptAllBytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(ShortTestData); // 远小于 220 字节

        var encryptedBytes = XorHelper.GetQuickXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetQuickXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(dataBytes.Length, encryptedBytes.Length);
    }

    [Fact]
    public void GetSelfXorBytes_ShouldModifyOriginalArray()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var originalBytes = Encoding.UTF8.GetBytes(TestData);
        var dataBytes = new byte[originalBytes.Length];
        Array.Copy(originalBytes, dataBytes, originalBytes.Length);

        XorHelper.GetSelfXorBytes(dataBytes, keyBytes);
        XorHelper.GetSelfXorBytes(dataBytes, keyBytes);

        Assert.Equal(originalBytes, dataBytes);
    }

    /// <summary>
    /// GetQuickSelfXorBytes 应原地修改数组，且只修改前 220 字节
    /// </summary>
    [Fact]
    public void GetQuickSelfXorBytes_ShouldOnlyModifyFirst220Bytes()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = new byte[300];
        for (int i = 0; i < dataBytes.Length; i++) dataBytes[i] = (byte)(i % 256);
        var original = (byte[])dataBytes.Clone();

        XorHelper.GetQuickSelfXorBytes(dataBytes, keyBytes);

        // 前 220 字节应被修改
        Assert.NotEqual(original[..220], dataBytes[..220]);
        // 220 字节之后应保持不变
        Assert.Equal(original[220..], dataBytes[220..]);
    }

    /// <summary>
    /// GetQuickSelfXorBytes 两次调用应还原原始数据
    /// </summary>
    [Fact]
    public void GetQuickSelfXorBytes_TwiceWithSameKey_ShouldRestoreOriginal()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = new byte[300];
        for (int i = 0; i < dataBytes.Length; i++) dataBytes[i] = (byte)(i % 256);
        var original = (byte[])dataBytes.Clone();

        XorHelper.GetQuickSelfXorBytes(dataBytes, keyBytes);
        XorHelper.GetQuickSelfXorBytes(dataBytes, keyBytes);

        Assert.Equal(original, dataBytes);
    }

    // ── GetXorBytes(byte[], int, int, byte[]) 重载 ──────────────────────────

    /// <summary>
    /// 带 offset/length 的重载应只加密指定范围，其余字节不变
    /// </summary>
    [Fact]
    public void GetXorBytes_WithOffsetAndLength_ShouldOnlyEncryptSpecifiedRange()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        var result = XorHelper.GetXorBytes(dataBytes, 2, 4, keyBytes);

        // 前 2 字节不变
        Assert.Equal(dataBytes[0], result[0]);
        Assert.Equal(dataBytes[1], result[1]);
        // 中间 4 字节被加密
        Assert.NotEqual(dataBytes[2..6], result[2..6]);
        // 后 2 字节不变
        Assert.Equal(dataBytes[6], result[6]);
        Assert.Equal(dataBytes[7], result[7]);
    }

    [Fact]
    public void GetXorBytes_WithOffsetAndLength_Roundtrip_ShouldRestoreOriginal()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encrypted = XorHelper.GetXorBytes(dataBytes, 0, dataBytes.Length, keyBytes);
        var decrypted = XorHelper.GetXorBytes(encrypted, 0, encrypted.Length, keyBytes);

        Assert.Equal(dataBytes, decrypted);
    }

    [Fact]
    public void GetXorBytes_WithNegativeStartIndex_ShouldThrowArgumentOutOfRangeException()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            XorHelper.GetXorBytes(dataBytes, -1, 4, keyBytes));
    }

    [Fact]
    public void GetXorBytes_WithNegativeLength_ShouldThrowArgumentOutOfRangeException()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            XorHelper.GetXorBytes(dataBytes, 0, -1, keyBytes));
    }

    [Fact]
    public void GetXorBytes_WithOffsetPlusLengthExceedingArray_ShouldThrowArgumentOutOfRangeException()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = new byte[10];

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            XorHelper.GetXorBytes(dataBytes, 5, 10, keyBytes));
    }

    // ── 参数校验 ────────────────────────────────────────────────────────────

    [Fact]
    public void GetXorBytes_WithNullKey_ShouldThrowArgumentNullException()
    {
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentNullException>(() => XorHelper.GetXorBytes(dataBytes, null));
    }

    [Fact]
    public void GetXorBytes_WithNullData_ShouldReturnNull()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        var result = XorHelper.GetXorBytes(null, keyBytes);

        Assert.Null(result);
    }

    [Fact]
    public void GetSelfXorBytes_WithNullKey_ShouldThrowArgumentNullException()
    {
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentNullException>(() => XorHelper.GetSelfXorBytes(dataBytes, null));
    }

    [Fact]
    public void GetSelfXorBytes_WithNullData_ShouldNotThrow()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        XorHelper.GetSelfXorBytes(null, keyBytes); // 不应抛出
    }

    [Fact]
    public void GetXorBytes_WithEmptyKey_ShouldThrowArgumentException()
    {
        var emptyKeyBytes = Array.Empty<byte>();
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentException>(() => XorHelper.GetXorBytes(dataBytes, emptyKeyBytes));
    }

    [Fact]
    public void GetSelfXorBytes_WithEmptyKey_ShouldThrowArgumentException()
    {
        var emptyKeyBytes = Array.Empty<byte>();
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentException>(() => XorHelper.GetSelfXorBytes(dataBytes, emptyKeyBytes));
    }

    [Fact]
    public void GetQuickXorBytes_WithNullData_ShouldThrowArgumentNullException()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);

        Assert.Throws<ArgumentNullException>(() => XorHelper.GetQuickXorBytes(null, keyBytes));
    }

    [Fact]
    public void GetQuickXorBytes_WithNullKey_ShouldThrowArgumentNullException()
    {
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentNullException>(() => XorHelper.GetQuickXorBytes(dataBytes, null));
    }

    [Fact]
    public void GetQuickXorBytes_WithEmptyKey_ShouldThrowArgumentException()
    {
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        Assert.Throws<ArgumentException>(() => XorHelper.GetQuickXorBytes(dataBytes, Array.Empty<byte>()));
    }

    // ── 其他行为验证 ─────────────────────────────────────────────────────────

    [Fact]
    public void GetXorBytes_WithDifferentKeys_ShouldProduceDifferentResults()
    {
        var key1Bytes = Encoding.UTF8.GetBytes("Key1");
        var key2Bytes = Encoding.UTF8.GetBytes("Key2");
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encrypted1 = XorHelper.GetXorBytes(dataBytes, key1Bytes);
        var encrypted2 = XorHelper.GetXorBytes(dataBytes, key2Bytes);

        Assert.NotEqual(encrypted1, encrypted2);
    }

    [Fact]
    public void GetXorBytes_SameDataAndKey_ShouldProduceSameResult()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encrypted1 = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var encrypted2 = XorHelper.GetXorBytes(dataBytes, keyBytes);

        Assert.Equal(encrypted1, encrypted2);
    }

    [Fact]
    public void XorOperation_IsReversible()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
    }

    [Fact]
    public void GetXorBytes_WithUnicodeCharacters_ShouldHandleCorrectly()
    {
        var unicodeData = "Hello 世界 🌍 Ñoël";
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(unicodeData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(unicodeData, Encoding.UTF8.GetString(decryptedBytes));
    }

    [Fact]
    public void GetXorBytes_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        var specialData = "!@#$%^&*()_+-=[]{}|;':,.<>?/~`";
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(specialData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(specialData, Encoding.UTF8.GetString(decryptedBytes));
    }

    [Fact]
    public void GetXorBytes_KeyLongerThanData_ShouldWork()
    {
        var shortData = "Hi";
        var longKey = "ThisIsAVeryLongKeyThatIsLongerThanTheData";
        var keyBytes = Encoding.UTF8.GetBytes(longKey);
        var dataBytes = Encoding.UTF8.GetBytes(shortData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(shortData, Encoding.UTF8.GetString(decryptedBytes));
    }

    [Fact]
    public void GetXorBytes_DataLongerThanKey_ShouldWork()
    {
        var longData = "This is a very long piece of data that is much longer than the key";
        var shortKey = "Key";
        var keyBytes = Encoding.UTF8.GetBytes(shortKey);
        var dataBytes = Encoding.UTF8.GetBytes(longData);

        var encryptedBytes = XorHelper.GetXorBytes(dataBytes, keyBytes);
        var decryptedBytes = XorHelper.GetXorBytes(encryptedBytes, keyBytes);

        Assert.Equal(dataBytes, decryptedBytes);
        Assert.Equal(longData, Encoding.UTF8.GetString(decryptedBytes));
    }

    /// <summary>
    /// GetXorBytes 不应修改原始数组（返回新数组）
    /// </summary>
    [Fact]
    public void GetXorBytes_ShouldNotModifyOriginalArray()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var original = (byte[])dataBytes.Clone();

        XorHelper.GetXorBytes(dataBytes, keyBytes);

        Assert.Equal(original, dataBytes);
    }

    /// <summary>
    /// GetSelfXorBytes 应原地修改数组（不分配新数组）
    /// </summary>
    [Fact]
    public void GetSelfXorBytes_ShouldModifyArrayInPlace()
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestKey);
        var dataBytes = Encoding.UTF8.GetBytes(TestData);
        var original = (byte[])dataBytes.Clone();

        XorHelper.GetSelfXorBytes(dataBytes, keyBytes);

        Assert.NotEqual(original, dataBytes);
    }

    /// <summary>
    /// codeIndex 从 startIndex % codeLength 开始，验证 offset 非零时密钥循环正确
    /// </summary>
    [Fact]
    public void GetSelfXorBytes_WithOffset_KeyCycleStartsFromOffsetModKeyLength()
    {
        var keyBytes = new byte[] { 0xAA, 0xBB, 0xCC };
        var data1 = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 };
        var data2 = (byte[])data1.Clone();

        // 对 data1 全部加密
        XorHelper.GetSelfXorBytes(data1, 0, 5, keyBytes);
        // 对 data2 只加密后 3 字节（offset=2）
        XorHelper.GetSelfXorBytes(data2, 2, 3, keyBytes);

        // data2[0..2] 不变
        Assert.Equal(0x01, data2[0]);
        Assert.Equal(0x02, data2[1]);
        // data2[2..5] 应与 data1[2..5] 相同（相同 offset，相同密钥循环起点）
        Assert.Equal(data1[2], data2[2]);
        Assert.Equal(data1[3], data2[3]);
        Assert.Equal(data1[4], data2[4]);
    }
}
