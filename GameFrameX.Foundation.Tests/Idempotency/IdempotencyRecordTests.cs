using System;
using System.Collections.Generic;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 幂等域模型构造校验测试：记录、标识、占位请求的非空白约束。
/// </summary>
public class IdempotencyRecordTests
{
    [Theory]
    [InlineData(null, "key-1", "digest")]
    [InlineData("", "key-1", "digest")]
    [InlineData("   ", "key-1", "digest")]
    [InlineData("scope", null, "digest")]
    [InlineData("scope", "", "digest")]
    [InlineData("scope", "   ", "digest")]
    [InlineData("scope", "key-1", null)]
    [InlineData("scope", "key-1", "")]
    [InlineData("scope", "key-1", "   ")]
    public void RecordConstructor_BlankStringField_ShouldThrowArgumentException(string? idempotencyScope, string? idempotencyKey, string? requestDigest)
    {
        // 执行 + 断言
        Assert.Throws<ArgumentException>(() => new IdempotencyRecord(idempotencyScope!, idempotencyKey!, requestDigest!, IdempotencyStatus.Processing, null, 1000, 2000, null));
    }

    [Fact]
    public void RecordConstructor_ValidArguments_ShouldExposeAllFields()
    {
        // 安排
        byte[] firstResponseBytes = new byte[] { 7, 8 };

        // 执行
        IdempotencyRecord record = new IdempotencyRecord("scope", "key-1", "digest", IdempotencyStatus.Completed, firstResponseBytes, 1000, 2000, 1500);

        // 断言
        Assert.Equal("scope", record.IdempotencyScope);
        Assert.Equal("key-1", record.IdempotencyKey);
        Assert.Equal("digest", record.RequestDigest);
        Assert.Equal(IdempotencyStatus.Completed, record.Status);
        Assert.Equal(firstResponseBytes, record.FirstResponse!.Value.ToArray());
        Assert.Equal(1000, record.CreatedTime);
        Assert.Equal(2000, record.ExpiredTime);
        Assert.Equal(1500, record.FinishedTime);
    }

    [Theory]
    [InlineData(null, "key-1")]
    [InlineData("", "key-1")]
    [InlineData("scope", null)]
    [InlineData("scope", " ")]
    public void IdentifierConstructor_BlankStringField_ShouldThrowArgumentException(string? idempotencyScope, string? idempotencyKey)
    {
        // 执行 + 断言
        Assert.Throws<ArgumentException>(() => new IdempotencyRecordIdentifier(idempotencyScope!, idempotencyKey!));
    }

    [Theory]
    [InlineData(null, "key-1", "digest")]
    [InlineData("scope", " ", "digest")]
    [InlineData("scope", "key-1", "")]
    public void BeginRequestConstructor_BlankStringField_ShouldThrowArgumentException(string? idempotencyScope, string? idempotencyKey, string? requestDigest)
    {
        // 执行 + 断言
        Assert.Throws<ArgumentException>(() => new IdempotencyBeginRequest(idempotencyScope!, idempotencyKey!, requestDigest!));
    }

    [Fact]
    public void CompletionRequestConstructor_NullIdentifier_ShouldThrowArgumentNullException()
    {
        // 执行 + 断言
        Assert.Throws<ArgumentNullException>(() => new IdempotencyCompletionRequest(null!, new byte[] { 1 }, 1000));
    }

    [Fact]
    public void FailureRequestConstructor_NullIdentifier_ShouldThrowArgumentNullException()
    {
        // 执行 + 断言
        Assert.Throws<ArgumentNullException>(() => new IdempotencyFailureRequest(null!, 1000));
    }
}
