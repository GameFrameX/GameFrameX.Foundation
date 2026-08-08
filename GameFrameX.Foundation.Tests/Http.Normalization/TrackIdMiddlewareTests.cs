using System.Linq;
using GameFrameX.Foundation.Http.Normalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

public sealed class TrackIdMiddlewareTests
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

    [Fact]
    public async Task UseTrackId_GeneratesIdAndEchoesInResponseHeader_WhenAbsent()
    {
        using var server = CreateServer(async ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status200OK;
            await ctx.Response.WriteAsync(TrackIdContext.Current ?? string.Empty);
        });
        using var client = server.CreateClient();

        var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        // 中间件为缺失请求头生成 22 字符短 TrackId，并写入异步上下文供业务读取
        Assert.Equal(22, body.Length);
        // 同一个 TrackId 回写到了响应头
        Assert.True(response.Headers.Contains(TrackIdMiddleware.HeaderName));
        Assert.Equal(body, response.Headers.GetValues(TrackIdMiddleware.HeaderName).First());
    }

    [Fact]
    public async Task UseTrackId_PreservesIncomingHeader_InContextAndResponse()
    {
        using var server = CreateServer(async ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status200OK;
            await ctx.Response.WriteAsync(TrackIdContext.Current ?? string.Empty);
        });
        using var client = server.CreateClient();
        client.DefaultRequestHeaders.Add(TrackIdMiddleware.HeaderName, "incoming-id");

        var response = await client.GetAsync("/");
        var body = await response.Content.ReadAsStringAsync();

        // 异步上下文透传了请求头携带的 TrackId（而非新生成）
        Assert.Equal("incoming-id", body);
        // 响应头回写同一个 TrackId
        Assert.Equal("incoming-id", response.Headers.GetValues(TrackIdMiddleware.HeaderName).First());
    }
}
