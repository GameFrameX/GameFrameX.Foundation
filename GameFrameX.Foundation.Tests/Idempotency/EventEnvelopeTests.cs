using System;
using System.Collections.Generic;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 事件信封测试：校验全分支、元数据只读隔离、载荷字节一致。
/// </summary>
public class EventEnvelopeTests
{
    private static EventEnvelope CreateValidEnvelope()
    {
        return new EventEnvelope("event-1", "sample.order.placed", 1_700_000_000_000, 1, "order-service", null, new byte[] { 1, 2 }, null);
    }

    [Fact]
    public void EnsureValid_ValidEnvelope_ShouldNotThrow()
    {
        // 安排：合法信封，可选字段取空值
        EventEnvelope envelope = CreateValidEnvelope();

        // 执行 + 断言
        envelope.EnsureValid();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EnsureValid_BlankEventId_ShouldThrowArgumentException(string? eventId)
    {
        // 安排
        EventEnvelope envelope = new EventEnvelope(eventId!, "sample.order.placed", 1_700_000_000_000, 1, "order-service", null, new byte[] { 1 }, null);

        // 执行 + 断言：异常消息指明字段名
        ArgumentException exception = Assert.Throws<ArgumentException>(() => envelope.EnsureValid());
        Assert.Contains("EventId", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EnsureValid_BlankEventType_ShouldThrowArgumentException(string? eventType)
    {
        // 安排
        EventEnvelope envelope = new EventEnvelope("event-1", eventType!, 1_700_000_000_000, 1, "order-service", null, new byte[] { 1 }, null);

        // 执行 + 断言
        ArgumentException exception = Assert.Throws<ArgumentException>(() => envelope.EnsureValid());
        Assert.Contains("EventType", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EnsureValid_BlankSource_ShouldThrowArgumentException(string? source)
    {
        // 安排
        EventEnvelope envelope = new EventEnvelope("event-1", "sample.order.placed", 1_700_000_000_000, 1, source!, null, new byte[] { 1 }, null);

        // 执行 + 断言
        ArgumentException exception = Assert.Throws<ArgumentException>(() => envelope.EnsureValid());
        Assert.Contains("Source", exception.Message);
    }

    [Fact]
    public void EnsureValid_SchemaVersionBelowOne_ShouldThrowArgumentException()
    {
        // 安排
        EventEnvelope envelope = new EventEnvelope("event-1", "sample.order.placed", 1_700_000_000_000, 0, "order-service", null, new byte[] { 1 }, null);

        // 执行 + 断言
        ArgumentException exception = Assert.Throws<ArgumentException>(() => envelope.EnsureValid());
        Assert.Contains("SchemaVersion", exception.Message);
    }

    [Fact]
    public void Attributes_NullAttributes_ShouldMapToEmptyCollection()
    {
        // 安排 + 执行
        EventEnvelope envelope = CreateValidEnvelope();

        // 断言
        Assert.Empty(envelope.Attributes);
    }

    [Fact]
    public void Attributes_ExternalDictionaryMutation_ShouldNotAffectEnvelope()
    {
        // 安排
        Dictionary<string, string> sourceAttributes = new Dictionary<string, string>
        {
            { "region", "north" },
        };
        EventEnvelope envelope = new EventEnvelope("event-1", "sample.order.placed", 1_700_000_000_000, 1, "order-service", null, new byte[] { 1 }, sourceAttributes);

        // 执行：构造后修改原字典
        sourceAttributes["region"] = "south";
        sourceAttributes.Add("extra", "value");

        // 断言：信封持有独立快照
        Assert.Single(envelope.Attributes);
        Assert.Equal("north", envelope.Attributes["region"]);
        Assert.False(envelope.Attributes.ContainsKey("extra"));
    }

    [Fact]
    public void Payload_ShouldReflectConstructedBytes()
    {
        // 安排
        byte[] payloadBytes = new byte[] { 10, 20, 30, 40 };

        // 执行
        EventEnvelope envelope = new EventEnvelope("event-1", "sample.order.placed", 1_700_000_000_000, 1, "order-service", "trace-1", payloadBytes, null);

        // 断言：只读视图反映构造字节
        Assert.Equal(payloadBytes, envelope.Payload.ToArray());
        Assert.Equal(4, envelope.Payload.Length);
    }
}
