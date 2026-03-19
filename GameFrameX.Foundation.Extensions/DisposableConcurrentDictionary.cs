namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 值可被Dispose的并发字典类型。
/// </summary>
/// <remarks>
/// A concurrent dictionary type whose values can be disposed.
/// </remarks>
/// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
/// <typeparam name="TValue">值的类型，必须实现IDisposable接口 / The type of the value, must implement IDisposable interface.</typeparam>
public class DisposableConcurrentDictionary<TKey, TValue> : NullableConcurrentDictionary<TKey, TValue>, IDisposable where TValue : IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// 初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> class.
    /// </remarks>
    public DisposableConcurrentDictionary()
    {
    }

    /// <summary>
    /// 使用指定的默认值初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> class with the specified default value.
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value to return when a key does not exist.</param>
    public DisposableConcurrentDictionary(TValue fallbackValue) : base(fallbackValue)
    {
    }

    /// <summary>
    /// 使用指定的并发级别和初始容量初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> class with the specified concurrency level and initial capacity.
    /// </remarks>
    /// <param name="concurrencyLevel">并发级别，即字典可以同时支持的线程数，必须大于0 / The number of threads that can access the dictionary concurrently, must be greater than 0.</param>
    /// <param name="capacity">字典的初始容量，必须大于等于0 / The initial number of elements that the dictionary can contain, must be greater than or equal to 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="concurrencyLevel"/> 小于等于 0 或 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="concurrencyLevel"/> is less than or equal to 0 or <paramref name="capacity"/> is less than 0.</exception>
    public DisposableConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
    }

    /// <summary>
    /// 使用指定的比较器初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> class with the specified comparer.
    /// </remarks>
    /// <param name="comparer">用于比较键的比较器，不能为null / The equality comparison implementation to use when comparing keys, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="comparer"/> 为 null 时抛出 / Thrown when <paramref name="comparer"/> is null.</exception>
    public DisposableConcurrentDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
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

        Dispose(true);
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 终结器，确保未释放的资源在对象被垃圾回收时被释放。
    /// </summary>
    /// <remarks>
    /// Finalizer to ensure unmanaged resources are released when the object is garbage collected.
    /// </remarks>
    ~DisposableConcurrentDictionary()
    {
        Dispose(false);
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    /// <remarks>
    /// Releases the unmanaged resources used by the <see cref="DisposableConcurrentDictionary{TKey, TValue}"/> and optionally releases the managed resources.
    /// </remarks>
    /// <param name="disposing">指示是否应释放托管资源 / true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            try
            {
                foreach (var s in Values.Where(v => v != null))
                {
                    s.Dispose();
                }
            }
            catch
            {
                // 忽略释放过程中的异常
            }
        }
    }
}