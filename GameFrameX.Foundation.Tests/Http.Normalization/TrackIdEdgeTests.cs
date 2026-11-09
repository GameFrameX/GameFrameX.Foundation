using GameFrameX.Foundation.Http.Normalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Normalization;

/// <summary>
/// TrackIdContext 与 TrackIdMiddleware 的边界与逆向测试。
/// </summary>
public sealed class TrackIdEdgeTests
{
    // === TrackIdContext 边界 ===

    [Fact]
    public void Current_InitiallyNull_InFreshAsyncFlow()
    {
        Assert.Null(TrackIdContext.Current);
    }

    [Fact]
    public void SetNull_ClearsCurrent()
    {
        TrackIdContext.Set("x");
        Assert.Equal("x", TrackIdContext.Current);

        TrackIdContext.Set(null);
        Assert.Null(TrackIdContext.Current);
    }

    [Fact]
    public void Generate_AlwaysUrlSafe_NoPlusSlashEquals()
    {
        // 批量验证生成器输出始终 URL 安全（可放 URL/header/查询串）
        for (var i = 0; i < 200; i++)
        {
            var id = TrackIdContext.Generate();
            Assert.Equal(22, id.Length);
            Assert.DoesNotContain("+", id);
            Assert.DoesNotContain("/", id);
            Assert.DoesNotContain("=", id);
        }
    }

    [Fact]
    public async Task ParallelTasks_HaveIndependentTrackIdContexts()
    {
        // 逆向：并行 Task 各自的 TrackIdContext 互不污染（AsyncLocal 上下文隔离）
        var t1 = Task.Run(async () =>
        {
            TrackIdContext.Set("task-1");
            await Task.Delay(20);
            return TrackIdContext.Current;
        });
        var t2 = Task.Run(async () =>
        {
            TrackIdContext.Set("task-2");
            await Task.Delay(20);
            return TrackIdContext.Current;
        });

        var results = await Task.WhenAll(t1, t2);

        Assert.Equal("task-1", results[0]);
        Assert.Equal("task-2", results[1]);
    }

    // === Middleware 边界 ===

    private static async Task<IHost> CreateServerAsync(RequestDelegate handler)
    {
        var host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.Configure(app =>
                {
                    app.UseTrackId();
                    app.Run(handler);
                });
            })
            .Build();

        await host.StartAsync();
        return host;
    }

    [Fact]
    public async Task Middleware_BlankHeader_GeneratesNewId()
    {
        // 逆向：空白 header 值应视为无，生成新 ID（而非透传空白）
        using (var host = await CreateServerAsync(async ctx =>
        {
            await ctx.Response.WriteAsync(TrackIdContext.Current ?? string.Empty);
        }))
        {
            using (var client = host.GetTestClient())
            {
                client.DefaultRequestHeaders.Add(TrackIdMiddleware.HeaderName, "   ");

                var response = await client.GetAsync("/", cancellationToken: TestContext.Current.CancellationToken);
                var body = await response.Content.ReadAsStringAsync(cancellationToken: TestContext.Current.CancellationToken);

                Assert.Equal(22, body.Length);
                Assert.Matches("^[A-Za-z0-9_-]{22}$", body);
            }
        }
    }

    [Fact]
    public async Task Middleware_DistinctRequests_GetDistinctTrackIds()
    {
        using (var host = await CreateServerAsync(async ctx =>
        {
            ctx.Response.StatusCode = 200;
            await ctx.Response.WriteAsync(TrackIdContext.Current ?? string.Empty);
        }))
        {
            using (var client = host.GetTestClient())
            {
                var id1 = await (await client.GetAsync("/", cancellationToken: TestContext.Current.CancellationToken)).Content.ReadAsStringAsync(cancellationToken: TestContext.Current.CancellationToken);
                var id2 = await (await client.GetAsync("/", cancellationToken: TestContext.Current.CancellationToken)).Content.ReadAsStringAsync(cancellationToken: TestContext.Current.CancellationToken);

                Assert.NotEqual(id1, id2);
                Assert.Equal(22, id1.Length);
                Assert.Equal(22, id2.Length);
            }
        }
    }

    [Fact]
    public async Task Middleware_NextThrows_ExceptionPropagates_NotSwallowed()
    {
        // 逆向：下游抛异常时，中间件不得吞掉异常
        using (var host = await CreateServerAsync(ctx => throw new InvalidOperationException("boom")))
        {
            using (var client = host.GetTestClient())
            {
                await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetAsync("/", cancellationToken: TestContext.Current.CancellationToken));
            }
        }
    }
}
