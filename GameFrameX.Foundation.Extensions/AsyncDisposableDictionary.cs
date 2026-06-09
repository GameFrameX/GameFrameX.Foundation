namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 值支持异步释放的字典。
/// </summary>
public class AsyncDisposableDictionary<TKey, TValue> : NullableDictionary<TKey, TValue>, IAsyncDisposable
    where TValue : IAsyncDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// 获取集合是否已经释放。
    /// </summary>
    public bool IsDisposed => _isDisposed;

    /// <summary>
    /// 获取或设置单个值释放失败时调用的处理器。
    /// </summary>
    public Action<TValue, Exception> DisposalErrorHandler { get; set; }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        foreach (var value in Values.Where(v => v != null))
        {
            try
            {
                await value.DisposeAsync();
            }
            catch (Exception exception)
            {
                TryHandleDisposalError(value, exception);
            }
        }
    }

    private void TryHandleDisposalError(TValue value, Exception exception)
    {
        try
        {
            DisposalErrorHandler?.Invoke(value, exception);
        }
        catch
        {
            // Error handlers must not prevent the remaining values from being disposed.
        }
    }
}
