using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

public sealed class AsyncDisposableDictionaryTests
{
    private sealed class TestAsyncDisposable : IAsyncDisposable
    {
        private readonly bool _throwOnDispose;

        public TestAsyncDisposable(bool throwOnDispose = false)
        {
            _throwOnDispose = throwOnDispose;
        }

        public int DisposeCount { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposeCount++;
            return _throwOnDispose
                ? ValueTask.FromException(new InvalidOperationException("dispose failed"))
                : ValueTask.CompletedTask;
        }
    }

    [Fact]
    public async Task DisposeAsync_DisposesAllValuesAndIsIdempotent()
    {
        var first = new TestAsyncDisposable();
        var second = new TestAsyncDisposable();
        var dictionary = new AsyncDisposableDictionary<string, TestAsyncDisposable>
        {
            ["first"] = first,
            ["second"] = second
        };

        await dictionary.DisposeAsync();
        await dictionary.DisposeAsync();

        Assert.True(dictionary.IsDisposed);
        Assert.Equal(1, first.DisposeCount);
        Assert.Equal(1, second.DisposeCount);
    }

    [Fact]
    public async Task DisposeAsync_WhenOneValueThrows_ContinuesAndReportsError()
    {
        var throwing = new TestAsyncDisposable(true);
        var remaining = new TestAsyncDisposable();
        Exception captured = null;
        var dictionary = new AsyncDisposableConcurrentDictionary<string, TestAsyncDisposable>
        {
            ["throwing"] = throwing,
            ["remaining"] = remaining,
            DisposalErrorHandler = (_, exception) => captured = exception
        };

        await dictionary.DisposeAsync();

        Assert.Equal(1, throwing.DisposeCount);
        Assert.Equal(1, remaining.DisposeCount);
        Assert.IsType<InvalidOperationException>(captured);
    }
}
