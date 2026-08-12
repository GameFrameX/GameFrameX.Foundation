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

    /// <summary>
    /// 获取或设置指定键的值。DisposeAsync 之后调用 set 访问器将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <param name="key">要获取或设置的键 / The key to get or set.</param>
    /// <value>与指定键关联的值 / The value associated with the specified key.</value>
    /// <exception cref="ObjectDisposedException">在实例释放后执行 set 操作时抛出 / Thrown when performing a set operation after the instance has been disposed.</exception>
    public override TValue this[TKey key]
    {
        get { return base[key]; }
        set
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(AsyncDisposableDictionary<TKey, TValue>));
            }
            base[key] = value;
        }
    }

    /// <summary>
    /// 向字典中添加指定的键值对。DisposeAsync 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <param name="key">要添加的键 / The key to add.</param>
    /// <param name="value">要添加的值 / The value to add.</param>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override void Add(TKey key, TValue value)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(AsyncDisposableDictionary<TKey, TValue>));
        }
        base.Add(key, value);
    }

    /// <summary>
    /// 从字典中移除指定键的值。DisposeAsync 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <param name="key">要移除的键 / The key to remove.</param>
    /// <returns>如果成功移除则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the element is successfully removed; otherwise <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override bool Remove(TKey key)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(AsyncDisposableDictionary<TKey, TValue>));
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
