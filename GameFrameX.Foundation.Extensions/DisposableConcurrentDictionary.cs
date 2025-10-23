namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 值可被Dispose的并发字典类型
/// </summary>
/// <typeparam name="TKey">键的类型。</typeparam>
/// <typeparam name="TValue">值的类型，必须实现IDisposable接口。</typeparam>
public class DisposableConcurrentDictionary<TKey, TValue> : NullableConcurrentDictionary<TKey, TValue>, IDisposable where TValue : IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// 初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    public DisposableConcurrentDictionary()
    {
    }

    /// <summary>
    /// 使用指定的默认值初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值。</param>
    public DisposableConcurrentDictionary(TValue fallbackValue) : base(fallbackValue)
    {
    }

    /// <summary>
    /// 使用指定的并发级别和初始容量初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <param name="concurrencyLevel">并发级别，即字典可以同时支持的线程数，必须大于0。</param>
    /// <param name="capacity">字典的初始容量，必须大于等于0。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 concurrencyLevel 小于等于 0 或 capacity 小于 0 时抛出。</exception>
    public DisposableConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
    }

    /// <summary>
    /// 使用指定的比较器初始化一个新的 <see cref="DisposableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <param name="comparer">用于比较键的比较器，不能为null。</param>
    /// <exception cref="ArgumentNullException">当 comparer 为 null 时抛出。</exception>
    public DisposableConcurrentDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
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
    ~DisposableConcurrentDictionary()
    {
        Dispose(false);
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    /// <param name="disposing">指示是否应释放托管资源。</param>
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