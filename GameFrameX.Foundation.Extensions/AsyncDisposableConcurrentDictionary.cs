namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 值支持异步释放的并发字典。
/// </summary>
public class AsyncDisposableConcurrentDictionary<TKey, TValue> : NullableConcurrentDictionary<TKey, TValue>, IAsyncDisposable
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

    /// <summary>
    /// 尝试向字典中添加键值对。DisposeAsync 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <param name="value">值，可以为 null（如果 TValue 是引用类型） / The value, can be null if TValue is a reference type.</param>
    /// <returns>如果成功添加则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if added successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override bool TryAdd(TKey key, TValue value)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(AsyncDisposableConcurrentDictionary<TKey, TValue>));
        }
        return base.TryAdd(key, value);
    }

    /// <summary>
    /// 从字典中移除指定的键。DisposeAsync 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <returns>如果成功移除则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if removed successfully; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override bool Remove(TKey key)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(AsyncDisposableConcurrentDictionary<TKey, TValue>));
        }
        return base.Remove(key);
    }

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
