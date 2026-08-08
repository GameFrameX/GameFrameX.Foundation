using System.Text.Json;
using GameFrameX.Foundation.Http.Normalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

public sealed class TrackIdTests
{
    private sealed class Payload
    {
        public string Name { get; set; }
    }

    [Fact]
    public void TrackId_Default_IsSerializedAsNull()
    {
        var result = HttpJsonResult.Success();

        using var json = JsonDocument.Parse(result.ToString());
        Assert.True(json.RootElement.TryGetProperty("trackId", out var trackId));
        Assert.Equal(JsonValueKind.Null, trackId.ValueKind);
    }

    [Fact]
    public void TrackId_WhenSet_IsSerializedToOutput()
    {
        var result = HttpJsonResult.Success();
        result.TrackId = "abc-123";

        using var json = JsonDocument.Parse(result.ToString());
        Assert.Equal("abc-123", json.RootElement.GetProperty("trackId").GetString());
    }

    [Fact]
    public void TrackId_FlowsThroughToHttpJsonResultData_OnSuccess()
    {
        var result = HttpJsonResult.Success(new Payload { Name = "x" });
        result.TrackId = "tid-success";

        var converted = result.ToString().ToHttpJsonResultData<Payload>();

        Assert.Equal("tid-success", converted.TrackId);
    }

    [Fact]
    public void TrackId_FlowsThroughToHttpJsonResultData_OnFailure()
    {
        var result = HttpJsonResult.Fail(400, "bad");
        result.TrackId = "tid-fail";

        var converted = result.ToString().ToHttpJsonResultData<Payload>();

        Assert.Equal("tid-fail", converted.TrackId);
    }

    [Fact]
    public void TrackIdContext_SetAndCurrent_RoundTrip()
    {
        TrackIdContext.Set("ctx-1");

        Assert.Equal("ctx-1", TrackIdContext.Current);
    }

    [Fact]
    public async Task TrackIdContext_FlowsAcrossAwait()
    {
        TrackIdContext.Set("flow-1");
        await Task.Yield();

        Assert.Equal("flow-1", TrackIdContext.Current);
    }

    [Fact]
    public void TrackIdContext_Generate_IsUniqueAndUrlSafe()
    {
        var a = TrackIdContext.Generate();
        var b = TrackIdContext.Generate();

        Assert.NotEqual(a, b);
        Assert.Equal(22, a.Length);
        Assert.Matches("^[A-Za-z0-9_-]{22}$", a);
    }
}
