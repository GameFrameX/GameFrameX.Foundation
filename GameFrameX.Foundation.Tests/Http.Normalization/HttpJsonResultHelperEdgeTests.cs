using System.Text.Json;
using GameFrameX.Foundation.Http.Normalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

/// <summary>
/// HttpJsonResultHelper 边界、逆向与往返保真测试。
/// </summary>
public sealed class HttpJsonResultHelperEdgeTests
{
    private sealed class Payload
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    // === 输入边界 ===

    [Fact]
    public void NullInput_ReturnsFailure_DoesNotThrow()
    {
        var result = ((string)null).ToHttpJsonResultData<Payload>();

        Assert.False(result.IsSuccess);
        Assert.Equal(-1, result.Code);
    }

    [Fact]
    public void EmptyJson_ReturnsResultDeserializationFailure()
    {
        var conversion = "".TryToHttpJsonResultData<Payload>();

        Assert.False(conversion.Succeeded);
        Assert.Equal(HttpJsonResultConversionFailureStage.ResultDeserialization, conversion.FailureStage);
    }

    [Fact]
    public void WhitespaceJson_ReturnsResultDeserializationFailure()
    {
        var conversion = "   ".TryToHttpJsonResultData<Payload>();

        Assert.False(conversion.Succeeded);
        Assert.Equal(HttpJsonResultConversionFailureStage.ResultDeserialization, conversion.FailureStage);
    }

    [Fact]
    public void EmptyObject_CodeDefaultsToZero_TreatedAsSuccess()
    {
        // 缺 code 字段时 int 默认 0 = 成功；data 缺失 → default(T)
        var result = "{}".ToHttpJsonResultData<Payload>();

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Code);
        Assert.Null(result.Data);
    }

    [Fact]
    public void MalformedJson_ReturnsResultDeserializationFailure()
    {
        var conversion = "{broken".TryToHttpJsonResultData<Payload>();

        Assert.False(conversion.Succeeded);
        Assert.Equal(HttpJsonResultConversionFailureStage.ResultDeserialization, conversion.FailureStage);
    }

    [Fact]
    public void ErrorMessage_DoesNotLeakRawPayload()
    {
        // 逆向：错误诊断消息不得包含原始 JSON（避免敏感数据泄漏）
        const string rawJson = "{\"code\":0,\"data\":{\"secret\":\"leak-me\"}}";

        var conversion = rawJson.TryToHttpJsonResultData<Payload>();

        // data 解析失败时，错误消息不含原始 secret
        Assert.DoesNotContain("leak-me", conversion.ErrorMessage, StringComparison.Ordinal);
    }

    // === data 字段边界 ===

    [Fact]
    public void Data_NullValue_ReturnsSuccess_WithDataDefault()
    {
        var result = "{\"code\":0,\"data\":null}".ToHttpJsonResultData<Payload>();

        Assert.True(result.IsSuccess);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Data_MissingField_ReturnsSuccess_WithDataDefault()
    {
        var result = "{\"code\":0}".ToHttpJsonResultData<Payload>();

        Assert.True(result.IsSuccess);
        Assert.Null(result.Data);
    }

    [Fact]
    public void Data_PrimitiveString_ToStringType_Deserializes()
    {
        var result = "{\"code\":0,\"data\":\"hello\"}".ToHttpJsonResultData<string>();

        Assert.True(result.IsSuccess);
        Assert.Equal("hello", result.Data);
    }

    [Fact]
    public void Data_Number_ToObjectType_FailsDataDeserialization()
    {
        // 逆向：data 是数字但 T 是对象 → 类型不匹配，归类为 data 反序列化失败
        var conversion = "{\"code\":0,\"data\":123}".TryToHttpJsonResultData<Payload>();

        Assert.False(conversion.Succeeded);
        Assert.Equal(HttpJsonResultConversionFailureStage.DataDeserialization, conversion.FailureStage);
    }

    [Fact]
    public void Data_Array_ToArrayType_Deserializes()
    {
        var result = "{\"code\":0,\"data\":[1,2,3]}".ToHttpJsonResultData<List<int>>();

        Assert.True(result.IsSuccess);
        Assert.Equal(new List<int> { 1, 2, 3 }, result.Data);
    }

    [Fact]
    public void Data_Array_ToObjectType_FailsDataDeserialization()
    {
        var conversion = "{\"code\":0,\"data\":[1,2,3]}".TryToHttpJsonResultData<Payload>();

        Assert.False(conversion.Succeeded);
        Assert.Equal(HttpJsonResultConversionFailureStage.DataDeserialization, conversion.FailureStage);
    }

    [Fact]
    public void Data_NestedObject_Deserializes()
    {
        var result = "{\"code\":0,\"data\":{\"Name\":\"x\",\"Count\":7}}".ToHttpJsonResultData<Payload>();

        Assert.True(result.IsSuccess);
        Assert.Equal("x", result.Data.Name);
        Assert.Equal(7, result.Data.Count);
    }

    // === 往返保真 ===

    [Fact]
    public void UnicodeAndEmoji_PreservedOnRoundTrip()
    {
        var original = HttpJsonResultData<Payload>.Fail(400, "中文消息 🎮 emoji");
        original.TrackId = "tid-中文";

        var converted = original.ToString().ToHttpJsonResultData<Payload>();

        Assert.Equal("中文消息 🎮 emoji", converted.Message);
        Assert.Equal("tid-中文", converted.TrackId);
    }

    [Fact]
    public void SuccessResponse_AllFieldsRoundTripPreserved()
    {
        var original = HttpJsonResultData<Payload>.Success(new Payload { Name = "x", Count = 5 });
        original.TrackId = "track-1";
        original.ErrorCode = "ERR.X";
        original.Type = "error";
        original.Extras = new { Meta = 42 };

        var converted = original.ToString().ToHttpJsonResultData<Payload>();

        Assert.Equal(0, converted.Code);
        Assert.Equal("x", converted.Data.Name);
        Assert.Equal(5, converted.Data.Count);
        Assert.Equal("track-1", converted.TrackId);
        Assert.Equal("ERR.X", converted.ErrorCode);
        Assert.Equal("error", converted.Type);
        Assert.Equal(original.Time, converted.Time);
        Assert.NotNull(converted.Extras);
    }

    [Fact]
    public void FailureResponse_PreservesAllFields_DataDefault()
    {
        var original = HttpJsonResultData<Payload>.Fail(403, "forbidden");
        original.TrackId = "tid";
        original.ErrorCode = "AUTH.FORBIDDEN";
        original.Type = "error";

        var converted = original.ToString().ToHttpJsonResultData<Payload>();

        Assert.False(converted.IsSuccess);
        Assert.Equal(403, converted.Code);
        Assert.Equal("forbidden", converted.Message);
        Assert.Equal("tid", converted.TrackId);
        Assert.Equal("AUTH.FORBIDDEN", converted.ErrorCode);
        Assert.Equal("error", converted.Type);
        Assert.Null(converted.Data);
    }

    // === 工厂完整性 ===

    [Fact]
    public void AllErrorFactories_ReturnCorrectCode()
    {
        Assert.Equal(400, HttpJsonResultData<Payload>.ValidationError().Code);
        Assert.Equal(401, HttpJsonResultData<Payload>.Unauthorized().Code);
        Assert.Equal(404, HttpJsonResultData<Payload>.NotFound().Code);
        Assert.Equal(500, HttpJsonResultData<Payload>.ServerError().Code);
        Assert.Equal(403, HttpJsonResultData<Payload>.ParamError().Code);
        Assert.Equal(401, HttpJsonResultData<Payload>.Illegal().Code);
    }

    [Fact]
    public void AllFactories_AutoFillTime()
    {
        // 逆向：所有工厂产出的对象都应自动填充 Time（不为 0）
        var before = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var results = new[]
        {
            HttpJsonResultData<Payload>.Success(),
            HttpJsonResultData<Payload>.Fail("x"),
            HttpJsonResultData<Payload>.ValidationError(),
            HttpJsonResultData<Payload>.Unauthorized(),
            HttpJsonResultData<Payload>.NotFound(),
            HttpJsonResultData<Payload>.ServerError(),
            HttpJsonResultData<Payload>.ParamError(),
            HttpJsonResultData<Payload>.Illegal(),
        };

        var after = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        foreach (var r in results)
        {
            Assert.InRange(r.Time, before, after);
        }
    }
}
