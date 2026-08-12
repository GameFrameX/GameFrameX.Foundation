using GameFrameX.Foundation.Http.Normalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

/// <summary>
/// <see cref="HttpJsonResultConversionResult{T}"/> 构造函数的 null 合并、属性往返与
/// <see cref="HttpJsonResultConversionFailureStage"/> 三档枚举测试。
/// </summary>
/// <remarks>
/// 覆盖构造函数第 77/79 行的 <c>errorMessage ?? string.Empty</c> 与
/// <c>exceptionType ?? string.Empty</c> 合并语义，以及 succeeded=true/false 下各属性的往返保真。
/// </remarks>
public sealed class HttpJsonResultConversionResultTests
{
    private sealed class Payload
    {
        public string Name { get; set; }

        public int Count { get; set; }
    }

    // === null 合并（源码第 77/79 行）===

    [Fact]
    public void Constructor_ErrorMessageNull_NormalizedToEmpty()
    {
        // Arrange
        var result = HttpJsonResultData<Payload>.Success(new Payload { Name = "x", Count = 1 });

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            succeeded: true,
            result: result,
            errorCode: 0,
            errorMessage: null,
            failureStage: HttpJsonResultConversionFailureStage.None,
            exceptionType: "SomeException");

        // Assert
        // 源码第 77 行：errorMessage ?? string.Empty
        Assert.Equal(string.Empty, conversion.ErrorMessage);
        // 其余字段不受影响
        Assert.Equal("SomeException", conversion.ExceptionType);
    }

    [Fact]
    public void Constructor_ExceptionTypeNull_NormalizedToEmpty()
    {
        // Arrange
        var result = HttpJsonResultData<Payload>.Success(new Payload { Name = "x", Count = 1 });

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            succeeded: true,
            result: result,
            errorCode: 0,
            errorMessage: "msg",
            failureStage: HttpJsonResultConversionFailureStage.None,
            exceptionType: null);

        // Assert
        // 源码第 79 行：exceptionType ?? string.Empty
        Assert.Equal(string.Empty, conversion.ExceptionType);
        Assert.Equal("msg", conversion.ErrorMessage);
    }

    [Fact]
    public void Constructor_BothErrorFieldsNull_BothNormalizedToEmpty()
    {
        // Arrange
        var result = HttpJsonResultData<Payload>.Fail(-1, "err");

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            succeeded: false,
            result: result,
            errorCode: -1,
            errorMessage: null,
            failureStage: HttpJsonResultConversionFailureStage.ResultDeserialization,
            exceptionType: null);

        // Assert
        Assert.Equal(string.Empty, conversion.ErrorMessage);
        Assert.Equal(string.Empty, conversion.ExceptionType);
    }

    // === 正常构造属性往返（succeeded=true）===

    [Fact]
    public void Constructor_SucceededTrue_AllPropertiesRoundTrip()
    {
        // Arrange
        var result = HttpJsonResultData<Payload>.Success(new Payload { Name = "beta", Count = 2 });

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            succeeded: true,
            result: result,
            errorCode: 0,
            errorMessage: "ok-msg",
            failureStage: HttpJsonResultConversionFailureStage.None,
            exceptionType: string.Empty);

        // Assert
        Assert.True(conversion.Succeeded);
        Assert.Same(result, conversion.Result);
        Assert.NotNull(conversion.Result);
        Assert.Equal(0, conversion.ErrorCode);
        Assert.Equal("ok-msg", conversion.ErrorMessage);
        Assert.Equal(HttpJsonResultConversionFailureStage.None, conversion.FailureStage);
        Assert.Equal(string.Empty, conversion.ExceptionType);
    }

    // === 正常构造属性往返（succeeded=false）===

    [Fact]
    public void Constructor_SucceededFalse_AllPropertiesRoundTrip()
    {
        // Arrange
        var result = HttpJsonResultData<Payload>.Fail(-1, "fail-msg");

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            succeeded: false,
            result: result,
            errorCode: -1,
            errorMessage: "err-msg",
            failureStage: HttpJsonResultConversionFailureStage.DataDeserialization,
            exceptionType: "JsonException");

        // Assert
        Assert.False(conversion.Succeeded);
        Assert.Same(result, conversion.Result);
        Assert.Equal(-1, conversion.ErrorCode);
        Assert.Equal("err-msg", conversion.ErrorMessage);
        Assert.Equal(HttpJsonResultConversionFailureStage.DataDeserialization, conversion.FailureStage);
        Assert.Equal("JsonException", conversion.ExceptionType);
    }

    [Fact]
    public void Constructor_ResultNonNull_StoredByReference()
    {
        // 逆向：构造函数直接存储 result 引用，不做拷贝/转换
        // Arrange
        var result = HttpJsonResultData<Payload>.Success(new Payload { Name = "ref", Count = 9 });

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            true, result, 0, "", HttpJsonResultConversionFailureStage.None, "");

        // Assert
        Assert.NotNull(conversion.Result);
        Assert.Same(result, conversion.Result);
    }

    [Fact]
    public void Constructor_ErrorCodeArbitrary_PreservedExactly()
    {
        // ErrorCode 是 int，测试非 0/-1 的任意业务码被精确保留
        // Arrange
        var result = HttpJsonResultData<Payload>.Fail(418, "teapot");

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            false, result, 418, "teapot", HttpJsonResultConversionFailureStage.None, "");

        // Assert
        Assert.Equal(418, conversion.ErrorCode);
    }

    // === FailureStage 三档枚举 ===

    [Theory]
    [InlineData(HttpJsonResultConversionFailureStage.None)]
    [InlineData(HttpJsonResultConversionFailureStage.ResultDeserialization)]
    [InlineData(HttpJsonResultConversionFailureStage.DataDeserialization)]
    public void Constructor_FailureStage_AllEnumValuesPreserved(HttpJsonResultConversionFailureStage stage)
    {
        // Arrange
        var result = stage == HttpJsonResultConversionFailureStage.None
            ? HttpJsonResultData<Payload>.Success(new Payload())
            : HttpJsonResultData<Payload>.Fail(-1, "err");

        // Act
        var conversion = new HttpJsonResultConversionResult<Payload>(
            succeeded: stage == HttpJsonResultConversionFailureStage.None,
            result: result,
            errorCode: stage == HttpJsonResultConversionFailureStage.None ? 0 : -1,
            errorMessage: "msg",
            failureStage: stage,
            exceptionType: stage == HttpJsonResultConversionFailureStage.None ? string.Empty : "JsonException");

        // Assert
        Assert.Equal(stage, conversion.FailureStage);
    }
}
