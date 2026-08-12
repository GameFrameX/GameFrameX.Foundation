using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// AsyncDisposableDictionary 补充覆盖测试：空字典释放、DisposalErrorHandler 回调、
    /// handler 抛异常时不阻断剩余释放。
    /// </summary>
    public class AsyncDisposableDictionaryAdditionalTests
    {
        private sealed class TrackableAsyncDisposable : IAsyncDisposable
        {
            private readonly bool _throwOnDispose;

            public TrackableAsyncDisposable(bool throwOnDispose = false)
            {
                _throwOnDispose = throwOnDispose;
            }

            public bool IsDisposed { get; private set; }

            public ValueTask DisposeAsync()
            {
                IsDisposed = true;
                if (_throwOnDispose)
                {
                    return ValueTask.FromException(new InvalidOperationException("boom"));
                }

                return ValueTask.CompletedTask;
            }
        }

        [Fact]
        public async Task DisposeAsync_EmptyDictionary_ShouldSucceedAndSetIsDisposed()
        {
            // Arrange
            var dictionary = new AsyncDisposableDictionary<string, TrackableAsyncDisposable>();

            // Act
            await dictionary.DisposeAsync();

            // Assert
            Assert.True(dictionary.IsDisposed);
        }

        [Fact]
        public async Task DisposeAsync_WithDisposalErrorHandler_ShouldInvokeHandler()
        {
            // Arrange
            var throwing = new TrackableAsyncDisposable(throwOnDispose: true);
            var normal = new TrackableAsyncDisposable(throwOnDispose: false);
            Exception captured = null;
            TrackableAsyncDisposable capturedValue = null;

            var dictionary = new AsyncDisposableDictionary<string, TrackableAsyncDisposable>
            {
                DisposalErrorHandler = (value, exception) =>
                {
                    capturedValue = value;
                    captured = exception;
                },
            };
            dictionary["throwing"] = throwing;
            dictionary["normal"] = normal;

            // Act
            await dictionary.DisposeAsync();

            // Assert
            Assert.True(throwing.IsDisposed);
            Assert.True(normal.IsDisposed);
            Assert.Same(throwing, capturedValue);
            Assert.IsType<InvalidOperationException>(captured);
        }

        [Fact]
        public async Task DisposeAsync_HandlerItselfThrows_ShouldNotPreventRemainingDisposal()
        {
            // Arrange
            var first = new TrackableAsyncDisposable(throwOnDispose: true);
            var second = new TrackableAsyncDisposable(throwOnDispose: false);

            var dictionary = new AsyncDisposableDictionary<string, TrackableAsyncDisposable>
            {
                DisposalErrorHandler = (_, __) => throw new InvalidOperationException("handler exploded"),
            };
            dictionary["first"] = first;
            dictionary["second"] = second;

            // Act - should not throw despite handler throwing
            await dictionary.DisposeAsync();

            // Assert - both values should have been disposed
            Assert.True(first.IsDisposed);
            Assert.True(second.IsDisposed);
        }

        [Fact]
        public async Task DisposeAsync_NoErrorHandler_ShouldNotThrowWhenValueThrows()
        {
            // Arrange
            var throwing = new TrackableAsyncDisposable(throwOnDispose: true);
            var dictionary = new AsyncDisposableDictionary<string, TrackableAsyncDisposable>();
            dictionary["throwing"] = throwing;

            // Act - without error handler, exception is swallowed internally
            await dictionary.DisposeAsync();

            // Assert
            Assert.True(dictionary.IsDisposed);
            Assert.True(throwing.IsDisposed);
        }

        [Fact]
        public async Task DisposeAsync_CalledTwice_ShouldOnlyDisposeOnce()
        {
            // Arrange
            var disposable = new TrackableAsyncDisposable();
            var dictionary = new AsyncDisposableDictionary<string, TrackableAsyncDisposable>();
            dictionary["key"] = disposable;

            // Act
            await dictionary.DisposeAsync();
            await dictionary.DisposeAsync();

            // Assert
            Assert.True(disposable.IsDisposed);
        }

        [Fact]
        public void DisposalErrorHandler_DefaultValue_ShouldBeNull()
        {
            // Arrange & Act
            var dictionary = new AsyncDisposableDictionary<string, TrackableAsyncDisposable>();

            // Assert
            Assert.Null(dictionary.DisposalErrorHandler);
        }
    }
}
