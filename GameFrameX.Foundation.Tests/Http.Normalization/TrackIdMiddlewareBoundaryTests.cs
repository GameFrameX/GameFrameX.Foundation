using GameFrameX.Foundation.Http.Normalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

/// <summary>
/// <see cref="TrackIdMiddleware"/> 边界测试：超长 header 透传、下游覆盖/追加响应头语义。
/// </summary>
/// <remarks>
/// 源码 <see cref="TrackIdMiddleware.InvokeAsync"/> 第 93 行先于 <c>await _next</c> 用赋值方式
/// <c>context.Response.Headers[HeaderName] = trackId</c> 写入响应头，本身不做任何长度校验或截断。
/// 本测试验证：(1) 中间件对任意长度 header 原样透传；(2) 下游覆盖语义（赋值覆盖 / Append 多值）。
/// </remarks>
public sealed class TrackIdMiddlewareBoundaryTests
{
    private static TestServer CreateServer(RequestDelegate handler)
    {
        var builder = new WebHostBuilder().Configure(app =>
        {
            app.UseTrackId();
            app.Run(handler);
        });
        return new TestServer(builder);
    }

    // === 超长 header：中间件不做长度处理，原样透传 ===

    [Fact]
    public async Task InvokeAsync_OverlongHeader_PassedThrough_NoTruncation()
    {
        // 构造 100KB 的 TrackId，直接通过 DefaultHttpContext 调用中间件，
        // 避开 TestServer/Kestrel/HttpClient 的传输层 header 限制，
        // 纯粹验证中间件自身逻辑：源码无任何长度判断，应原样写入 Response.Headers。
        // Arrange
        var longTrackId = new string('x', 100 * 1024);
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers[TrackIdMiddleware.HeaderName] = longTrackId;

        var middleware = new TrackIdMiddleware(_ => Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(longTrackId, (string)httpContext.Response.Headers[TrackIdMiddleware.HeaderName]);
    }

    [Fact]
    public async Task InvokeAsync_KilobyteHeader_TransportedViaTestServer_WithoutTruncation()
    {
        // 4KB header 通过 TestServer 完整往返，进一步证明中间件不截断、传输层默认允许。
        // Arrange
        var trackId = new string('a', 4 * 1024);
        using (var server = CreateServer(async ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status200OK;
            await ctx.Response.WriteAsync(TrackIdContext.Current ?? string.Empty);
        }))
        {
            using (var client = server.CreateClient())
            {
                client.DefaultRequestHeaders.Add(TrackIdMiddleware.HeaderName, trackId);

                // Act
                var response = await client.GetAsync("/");
                var body = await response.Content.ReadAsStringAsync();

                // Assert
                Assert.Equal(trackId, body);
                Assert.Equal(trackId, response.Headers.GetValues(TrackIdMiddleware.HeaderName).First());
            }
        }
    }

    // === 下游覆盖响应头语义 ===

    [Fact]
    public async Task InvokeAsync_DownstreamAssignsHeader_DownstreamValueWins()
    {
        // 源码第 93 行先设置 Response.Headers[HeaderName] = trackId，再 await _next。
        // 下游用赋值方式（=）再次写入同一 header —— 后写覆盖先写，响应头最终为下游值。
        // 行为含义：中间件对下游覆盖"不强制"，消费者可自由覆盖，但意外覆盖也不会被阻止。
        // Arrange
        const string downstreamValue = "downstream-trace-id";
        using (var server = CreateServer(ctx =>
        {
            ctx.Response.Headers[TrackIdMiddleware.HeaderName] = downstreamValue;
            return Task.CompletedTask;
        }))
        {
            using (var client = server.CreateClient())
            {
                // Act
                var response = await client.GetAsync("/");

                // Assert
                Assert.Equal(
                    downstreamValue,
                    response.Headers.GetValues(TrackIdMiddleware.HeaderName).First());
            }
        }
    }

    [Fact]
    public async Task InvokeAsync_DownstreamAppendsHeader_ResponseHasBothValues()
    {
        // 中间件用赋值（=）写入 header，下游用 Append 追加 —— Response header 变成多值。
        // 验证 Append 语义：保留中间件原值 + 追加下游值，而非覆盖。
        // Arrange
        using (var server = CreateServer(ctx =>
        {
            ctx.Response.Headers.Append(TrackIdMiddleware.HeaderName, "downstream-extra");
            return Task.CompletedTask;
        }))
        {
            using (var client = server.CreateClient())
            {
                // Act
                var response = await client.GetAsync("/");

                // Assert
                var values = response.Headers.GetValues(TrackIdMiddleware.HeaderName).ToArray();
                Assert.Equal(2, values.Length);
                // 第一个是中间件生成的 22 字符短 TrackId
                Assert.Equal(22, values[0].Length);
                // 第二个是下游 Append 的值
                Assert.Equal("downstream-extra", values[1]);
            }
        }
    }

    // === 请求头透传一致性（补充断言 response == request，与生成值区分）===

    [Fact]
    public async Task InvokeAsync_IncomingHeader_ResponseEchoesExactIncomingValue()
    {
        // 已有 TrackIdMiddlewareTests.PreservesIncomingHeader 覆盖透传场景；
        // 此处补充严格断言：响应头值与请求头值完全相等（而非新生成的 22 字符短 ID）。
        // Arrange
        const string incoming = "req-id-boundary-1234567890";
        using (var server = CreateServer(_ => Task.CompletedTask))
        {
            using (var client = server.CreateClient())
            {
                client.DefaultRequestHeaders.Add(TrackIdMiddleware.HeaderName, incoming);

                // Act
                var response = await client.GetAsync("/");

                // Assert
                var responseValue = response.Headers.GetValues(TrackIdMiddleware.HeaderName).First();
                Assert.Equal(incoming, responseValue);
                // 28 字符的自定义 ID，非 22 字符生成的短 ID
                Assert.NotEqual(22, responseValue.Length);
            }
        }
    }
}
