using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

public sealed class AsyncEnumerableExtensionsTests
{
    [Fact]
    public async Task ForEachSequentialAsync_ShouldPreserveInputOrder()
    {
        var results = new List<int>();

        await new[] { 1, 2, 3 }.ForEachSequentialAsync((item, _) =>
        {
            results.Add(item);
            return ValueTask.CompletedTask;
        });

        Assert.Equal(new[] { 1, 2, 3 }, results);
    }

    [Fact]
    public async Task ForEachParallelAsync_ShouldRespectMaximumParallelism()
    {
        var current = 0;
        var maximum = 0;

        await Enumerable.Range(0, 20).ForEachParallelAsync(async (_, token) =>
        {
            var active = Interlocked.Increment(ref current);
            UpdateMaximum(ref maximum, active);
            await Task.Delay(5, token);
            Interlocked.Decrement(ref current);
        }, 3);

        Assert.InRange(maximum, 2, 3);
    }

    [Fact]
    public async Task ForEachParallelAsync_ShouldPassCancellationToken()
    {
        using var cancellation = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            Enumerable.Range(0, 10).ForEachParallelAsync(async (_, token) =>
            {
                receivedToken = token;
                cancellation.Cancel();
                await Task.Delay(20, token);
            }, 2, cancellation.Token));

        Assert.True(receivedToken.CanBeCanceled);
        Assert.True(receivedToken.IsCancellationRequested);
    }

    [Fact]
    public async Task SelectParallelAsync_PreserveOrder_ShouldReturnInputOrder()
    {
        var results = await new[] { 3, 1, 2 }.SelectParallelAsync(async (item, token) =>
        {
            await Task.Delay(item * 5, token);
            return item;
        }, 3);

        Assert.Equal(new[] { 3, 1, 2 }, results);
    }

    [Fact]
    public async Task SelectParallelAsync_CompletionOrder_ShouldReturnCompletionOrder()
    {
        var results = await new[] { 3, 1, 2 }.SelectParallelAsync(async (item, token) =>
        {
            await Task.Delay(item * 20, token);
            return item;
        }, 3, preserveOrder: false);

        Assert.Equal(new[] { 1, 2, 3 }, results);
    }

    [Fact]
    public async Task SelectParallelAsync_ShouldEnumerateSourceOnlyOnce()
    {
        var enumerationCount = 0;

        IEnumerable<int> Source()
        {
            enumerationCount++;
            yield return 1;
            yield return 2;
        }

        var results = await Source().SelectParallelAsync(
            (item, _) => ValueTask.FromResult(item * 2),
            2);

        Assert.Equal(new[] { 2, 4 }, results);
        Assert.Equal(1, enumerationCount);
    }

    private static void UpdateMaximum(ref int maximum, int value)
    {
        while (true)
        {
            var current = Volatile.Read(ref maximum);
            if (value <= current || Interlocked.CompareExchange(ref maximum, value, current) == current)
            {
                return;
            }
        }
    }
}
