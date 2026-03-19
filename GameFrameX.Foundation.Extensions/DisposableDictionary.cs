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
    ~DisposableDictionary()
    {
        Dispose(false);
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
        foreach (var s in Values.Where(v => v != null))
        {
            s.Dispose();
        }
    }
}