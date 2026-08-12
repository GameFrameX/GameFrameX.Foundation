namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 值可被Dispose的字典类型。
/// </summary>
/// <remarks>
/// A dictionary type whose values can be disposed.
/// </remarks>
/// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
/// <typeparam name="TValue">值的类型，必须实现IDisposable接口 / The type of the value, must implement IDisposable interface.</typeparam>
public class DisposableDictionary<TKey, TValue> : NullableDictionary<TKey, TValue>, IDisposable where TValue : IDisposable
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
    /// 初始化一个新的 <see cref="DisposableDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableDictionary{TKey, TValue}" /> class.
    /// </remarks>
    public DisposableDictionary()
    {
    }

    /// <summary>
    /// 使用指定的默认值初始化一个新的 <see cref="DisposableDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableDictionary{TKey, TValue}" /> class with the specified default value.
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value to return when a key does not exist.</param>
    public DisposableDictionary(TValue fallbackValue)
    {
        FallbackValue = fallbackValue;
    }

    /// <summary>
    /// 使用指定的字典初始化一个新的 <see cref="DisposableDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableDictionary{TKey, TValue}" /> class with the specified dictionary.
    /// </remarks>
    /// <param name="dictionary">用于初始化的字典，不能为 null / The dictionary to initialize with, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dictionary"/> 为 null 时抛出 / Thrown when <paramref name="dictionary"/> is null.</exception>
    public DisposableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));

        foreach (var kvp in dictionary)
        {
            this[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// 使用指定的初始容量初始化一个新的 <see cref="DisposableDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableDictionary{TKey, TValue}" /> class with the specified initial capacity.
    /// </remarks>
    /// <param name="capacity">字典的初始容量，必须大于等于 0 / The initial capacity of the dictionary, must be greater than or equal to 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="capacity"/> is less than 0.</exception>
    public DisposableDictionary(int capacity) : base(capacity)
    {
    }

    /// <summary>
    /// 使用指定的字典初始化一个新的 <see cref="DisposableDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableDictionary{TKey, TValue}" /> class with the specified dictionary.
    /// </remarks>
    /// <param name="dictionary">用于初始化的字典，不能为 null / The dictionary to initialize with, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dictionary"/> 为 null 时抛出 / Thrown when <paramref name="dictionary"/> is null.</exception>
    public DisposableDictionary(IDictionary<NullObject<TKey>, TValue> dictionary) : base(dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));
    }

    /// <summary>
    /// 获取或设置指定键的值。Dispose 之后调用 set 访问器将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value associated with the specified key. The set accessor throws <see cref="ObjectDisposedException"/> after the instance has been disposed.
    /// </remarks>
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
                throw new ObjectDisposedException(nameof(DisposableDictionary<TKey, TValue>));
            }
            base[key] = value;
        }
    }

    /// <summary>
    /// 向字典中添加指定的键值对。Dispose 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <remarks>
    /// Adds the specified key and value to the dictionary. Throws <see cref="ObjectDisposedException"/> after the instance has been disposed.
    /// </remarks>
    /// <param name="key">要添加的键 / The key to add.</param>
    /// <param name="value">要添加的值 / The value to add.</param>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override void Add(TKey key, TValue value)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(DisposableDictionary<TKey, TValue>));
        }
        base.Add(key, value);
    }

    /// <summary>
    /// 从字典中移除指定键的值。Dispose 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <remarks>
    /// Removes the value with the specified key from the dictionary. Throws <see cref="ObjectDisposedException"/> after the instance has been disposed.
    /// </remarks>
    /// <param name="key">要移除的键 / The key to remove.</param>
    /// <returns>如果成功移除则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the element is successfully removed; otherwise <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override bool Remove(TKey key)
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(DisposableDictionary<TKey, TValue>));
        }
        return base.Remove(key);
    }

    /// <summary>
    /// 返回循环访问字典的枚举器。Dispose 之后调用将抛出 <see cref="ObjectDisposedException"/>。
    /// </summary>
    /// <remarks>
    /// Returns an enumerator that iterates through the dictionary. Throws <see cref="ObjectDisposedException"/> after the instance has been disposed.
    /// </remarks>
    /// <returns>用于循环访问字典的枚举器 / An enumerator for the dictionary.</returns>
    /// <exception cref="ObjectDisposedException">在实例释放后调用时抛出 / Thrown when called after the instance has been disposed.</exception>
    public override Dictionary<NullObject<TKey>, TValue>.Enumerator GetEnumerator()
    {
        if (_isDisposed)
        {
            throw new ObjectDisposedException(nameof(DisposableDictionary<TKey, TValue>));
        }
        return base.GetEnumerator();
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    /// <remarks>
    /// Releases all resources used by this instance.
    /// </remarks>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        Dispose(true);
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    /// <remarks>
    /// Releases the unmanaged resources used by the <see cref="DisposableDictionary{TKey, TValue}"/> and optionally releases the managed resources.
    /// </remarks>
    /// <param name="disposing">指示是否应释放托管资源 / true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        foreach (var value in Values.Where(v => v != null))
        {
            try
            {
                value.Dispose();
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
