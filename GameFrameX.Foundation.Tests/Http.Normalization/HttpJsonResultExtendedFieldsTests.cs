using System.Text.Json;
using GameFrameX.Foundation.Http.Normalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

public sealed class HttpJsonResultExtendedFieldsTests
{
    private sealed class Payload
    {
        public string Name { get; set; }
    }

    [Fact]
    public void ErrorCode_Null_IsNotSerialized()
    {
        var result = HttpJsonResultData<Payload>.Success();

        using var json = JsonDocument.Parse(result.ToString());
        Assert.False(json.RootElement.TryGetProperty("errorCode", out _));
    }

    [Fact]
    public void ErrorCode_WhenSet_IsSerialized()
    {
        var result = HttpJsonResultData<Payload>.Fail("bad");
        result.ErrorCode = "VALIDATION.FAILED";

        using var json = JsonDocument.Parse(result.ToString());
        Assert.Equal("VALIDATION.FAILED", json.RootElement.GetProperty("errorCode").GetString());
    }

    [Fact]
    public void Type_Null_IsNotSerialized()
    {
        var result = HttpJsonResultData<Payload>.Success();

        using var json = JsonDocument.Parse(result.ToString());
        Assert.False(json.RootElement.TryGetProperty("type", out _));
    }

    [Fact]
    public void Type_WhenSet_IsSerialized()
    {
        var result = HttpJsonResultData<Payload>.Success();
        result.Type = "warning";

        using var json = JsonDocument.Parse(result.ToString());
        Assert.Equal("warning", json.RootElement.GetProperty("type").GetString());
    }

    [Fact]
    public void Time_AutoFilled_OnCreation_WithinNowRange()
    {
        var before = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var result = HttpJsonResultData<Payload>.Success();
        var after = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        using var json = JsonDocument.Parse(result.ToString());
        var time = json.RootElement.GetProperty("time").GetInt64();

        Assert.True(time > 0);
        Assert.InRange(time, before, after);
    }

    [Fact]
    public void ExtendedFields_FlowsThroughToHttpJsonResultData()
    {
        var result = HttpJsonResultData<Payload>.Fail(400, "bad");
        result.ErrorCode = "E001";
        result.Type = "error";

        var converted = result.ToString().ToHttpJsonResultData<object>();

        Assert.Equal("E001", converted.ErrorCode);
        Assert.Equal("error", converted.Type);
        Assert.True(converted.Time > 0);
        Assert.Equal(result.Time, converted.Time);
    }

    [Fact]
    public void Extras_Null_IsNotSerialized()
    {
        var result = HttpJsonResultData<Payload>.Success();

        using var json = JsonDocument.Parse(result.ToString());
        Assert.False(json.RootElement.TryGetProperty("extras", out _));
    }

    [Fact]
    public void Extras_WhenSet_IsSerializedAsObject()
    {
        var result = HttpJsonResultData<Payload>.Success();
        result.Extras = new { Page = 1, Total = 100 };

        using var json = JsonDocument.Parse(result.ToString());
        var extras = json.RootElement.GetProperty("extras");
        Assert.Equal(JsonValueKind.Object, extras.ValueKind);
        Assert.Equal(1, extras.GetProperty("Page").GetInt32());
        Assert.Equal(100, extras.GetProperty("Total").GetInt32());
    }

    [Fact]
    public void Extras_FlowsThroughToHttpJsonResultData()
    {
        var result = HttpJsonResultData<Payload>.Fail(400, "bad");
        result.Extras = new { Hint = "retry" };

        var converted = result.ToString().ToHttpJsonResultData<object>();

        Assert.NotNull(converted.Extras);
    }
}
