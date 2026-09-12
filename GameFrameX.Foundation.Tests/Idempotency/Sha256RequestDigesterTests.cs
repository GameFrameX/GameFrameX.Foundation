using System;
using System.Text;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// SHA-256 请求摘要测试：确定性、区分度、Base64 输出形态。
/// </summary>
public class Sha256RequestDigesterTests
{
    [Fact]
    public void ComputeDigest_SamePayload_ShouldReturnSameDigest()
    {
        // 安排
        Sha256RequestDigester digester = Sha256RequestDigester.Instance;
        byte[] payload = Encoding.UTF8.GetBytes("amount=100\nchannel=web\n");

        // 执行 + 断言：确定性
        Assert.Equal(digester.ComputeDigest(payload), digester.ComputeDigest(payload));
    }

    [Fact]
    public void ComputeDigest_DifferentPayloads_ShouldReturnDifferentDigests()
    {
        // 安排
        Sha256RequestDigester digester = Sha256RequestDigester.Instance;

        // 执行
        string digestOfAlpha = digester.ComputeDigest(Encoding.UTF8.GetBytes("a=1\n"));
        string digestOfBeta = digester.ComputeDigest(Encoding.UTF8.GetBytes("a=2\n"));

        // 断言：区分度
        Assert.NotEqual(digestOfAlpha, digestOfBeta);
    }

    [Fact]
    public void ComputeDigest_ShouldReturnBase64Of32ByteSha256Hash()
    {
        // 安排
        Sha256RequestDigester digester = Sha256RequestDigester.Instance;

        // 执行
        string digest = digester.ComputeDigest(Encoding.UTF8.GetBytes("k=v\n"));
        byte[] digestBytes = Convert.FromBase64String(digest);

        // 断言：可解码为 32 字节（SHA-256 摘要长度）
        Assert.Equal(32, digestBytes.Length);
    }
}
