using System.Text.Json;
using GameFrameX.Foundation.Http.Normalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

public sealed class HttpJsonResultTests
{
    private sealed record Payload(string Name, int Count);

    [Fact]
    public void Success_WithObjectData_StoresDataAsJsonString()
    {
        var result = HttpJsonResult.Success(new Payload("alpha", 2));

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Code);
        Assert.Equal(string.Empty, result.Message);
        Assert.Equal("{\"Name\":\"alpha\",\"Count\":2}", result.Data);

        using var json = JsonDocument.Parse(result.ToString());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal(JsonValueKind.String, data.ValueKind);
        Assert.Equal(result.Data, data.GetString());
    }

    [Fact]
    public void GenericSuccess_SerializesDataAsObject()
    {
        var result = HttpJsonResultData<Payload>.Success(new Payload("beta", 3));

        using var json = JsonDocument.Parse(result.ToString());
        var data = json.RootElement.GetProperty("data");
        Assert.Equal(JsonValueKind.Object, data.ValueKind);
        Assert.Equal("beta", data.GetProperty("Name").GetString());
        Assert.Equal(3, data.GetProperty("Count").GetInt32());
    }

    [Fact]
    public void ToHttpJsonResultData_WithSuccessString_DeserializesData()
    {
        var json = HttpJsonResult.Success(new Payload("gamma", 4)).ToString();

        var result = json.ToHttpJsonResultData<Payload>();

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Code);
        Assert.Equal("gamma", result.Data.Name);
        Assert.Equal(4, result.Data.Count);
    }

    [Fact]
    public void ToHttpJsonResultData_WithFailureString_PreservesCodeAndMessage()
    {
        var json = HttpJsonResult.Fail(404, "missing").ToString();

        var result = json.ToHttpJsonResultData<Payload>();

        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Code);
        Assert.Equal("missing", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public void ToHttpJsonResultData_WithInvalidJson_ReturnsDefaultFailure()
    {
        var result = "{not valid json}".ToHttpJsonResultData<Payload>();

        Assert.False(result.IsSuccess);
        Assert.Equal(-1, result.Code);
        Assert.Null(result.Data);
    }
}
