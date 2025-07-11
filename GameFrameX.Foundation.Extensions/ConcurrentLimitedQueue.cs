using System;
using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 定长队列，当队列达到指定长度时，新元素入队会自动移除最旧的元素。
/// </summary>
/// <typeparam name="T">队列中元素的类型。</typeparam>
public class ConcurrentLimitedQueue<T> : ConcurrentQueue<T>
{
    /// <summary>
    /// 初始化一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例，指定队列的最大长度。
    /// </summary>
    /// <param name="limit">队列的最大长度，必须大于0。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="limit"/> 小于或等于0时抛出。</exception>
    public ConcurrentLimitedQueue(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
        Limit = limit;
    }

    /// <summary>
    /// 使用指定的集合初始化一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例，并设置队列的最大长度为集合的元素数量。
    /// </summary>
    /// <param name="list">用于初始化队列的集合，不能为null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为null时抛出。</exception>
    public ConcurrentLimitedQueue(IEnumerable<T> list) : base(list ?? throw new ArgumentNullException(nameof(list)))
    {
        Limit = list.Count();
    }

    /// <summary>
    /// 队列的最大长度。
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// 将一个列表隐式转换为 <see cref="ConcurrentLimitedQueue{T}" />。
    /// </summary>
    /// <param name="list">要转换的列表，不能为null。</param>
    /// <returns>一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为null时抛出。</exception>
    public static implicit operator ConcurrentLimitedQueue<T>(List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        return new ConcurrentLimitedQueue<T>(list);
    }

    /// <summary>
    /// 将一个元素添加到队列中。如果队列已满，则移除最旧的元素。
    /// </summary>
    /// <param name="item">要添加的元素。</param>
    public new void Enqueue(T item)
    {
        while (Count >= Limit)
        {
            TryDequeue(out _);
        }

        base.Enqueue(item);
    }
}