// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Collections;
using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 定长队列，当队列达到指定长度时，新元素入队会自动移除最旧的元素。
/// </summary>
/// <remarks>
/// A fixed-length queue that automatically removes the oldest elements when new elements are enqueued and the queue reaches its maximum length.
/// </remarks>
/// <typeparam name="T">队列中元素的类型 / The type of elements in the queue.</typeparam>
public class ConcurrentLimitedQueue<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
{
    private readonly ConcurrentQueue<T> _queue;
    private readonly object _syncRoot = new();
    private int _limit;

    /// <summary>
    /// 初始化一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例，指定队列的最大长度。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConcurrentLimitedQueue{T}" /> class with the specified maximum length.
    /// </remarks>
    /// <param name="limit">队列的最大长度，必须大于0 / The maximum number of elements the queue can hold, must be greater than 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="limit"/> 小于或等于0时抛出 / Thrown when <paramref name="limit"/> is less than or equal to 0.</exception>
    public ConcurrentLimitedQueue(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
        _queue = new ConcurrentQueue<T>();
        _limit = limit;
    }

    /// <summary>
    /// 使用指定的集合初始化一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例，并设置队列的最大长度为集合的元素数量。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ConcurrentLimitedQueue{T}" /> class with the specified collection and sets the maximum length to the number of elements in the collection.
    /// </remarks>
    /// <param name="list">用于初始化队列的集合，不能为null / The collection to initialize the queue with, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为 null 时抛出 / Thrown when <paramref name="list"/> is null.</exception>
    public ConcurrentLimitedQueue(IEnumerable<T> list)
    {
        ArgumentNullException.ThrowIfNull(list);

        var items = list as ICollection<T> ?? list.ToArray();
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(items.Count, 0, nameof(list));

        _queue = new ConcurrentQueue<T>(items);
        _limit = items.Count;
    }

    /// <summary>
    /// 队列的最大长度。
    /// </summary>
    /// <remarks>
    /// Gets or sets the maximum number of elements the queue can hold.
    /// </remarks>
    /// <value>队列的最大长度 / The maximum number of elements the queue can hold.</value>
    public int Limit
    {
        get
        {
            lock (_syncRoot)
            {
                return _limit;
            }
        }
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 0, nameof(value));

            lock (_syncRoot)
            {
                _limit = value;
                TrimToLimit();
            }
        }
    }

    /// <inheritdoc />
    public int Count => _queue.Count;

    /// <inheritdoc />
    public bool IsSynchronized => false;

    /// <inheritdoc />
    public object SyncRoot => _syncRoot;

    /// <summary>
    /// 获取队列是否为空。
    /// </summary>
    /// <remarks>
    /// Gets whether the queue is empty.
    /// </remarks>
    public bool IsEmpty => _queue.IsEmpty;

    /// <summary>
    /// 将一个列表隐式转换为 <see cref="ConcurrentLimitedQueue{T}" />。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a <see cref="List{T}"/> to a <see cref="ConcurrentLimitedQueue{T}"/>.
    /// </remarks>
    /// <param name="list">要转换的列表，不能为 null / The list to convert, cannot be null.</param>
    /// <returns>一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例 / A new <see cref="ConcurrentLimitedQueue{T}" /> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为 null 时抛出 / Thrown when <paramref name="list"/> is null.</exception>
    public static implicit operator ConcurrentLimitedQueue<T>(List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        return new ConcurrentLimitedQueue<T>(list);
    }

    /// <summary>
    /// 将一个元素添加到队列中。如果队列已满，则移除最旧的元素。
    /// </summary>
    /// <remarks>
    /// Adds an element to the queue. If the queue is full, the oldest element is removed.
    /// </remarks>
    /// <param name="item">要添加的元素 / The element to add to the queue.</param>
    public void Enqueue(T item)
    {
        lock (_syncRoot)
        {
            TrimToLimit(extraItemCount: 1);
            _queue.Enqueue(item);
        }
    }

    /// <inheritdoc />
    public bool TryAdd(T item)
    {
        Enqueue(item);
        return true;
    }

    /// <summary>
    /// 尝试移除并返回队列开头的对象。
    /// </summary>
    /// <remarks>
    /// Attempts to remove and return the object at the beginning of the queue.
    /// </remarks>
    /// <param name="result">移除的元素 / The removed element.</param>
    /// <returns>移除成功返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if an element was removed; otherwise, <c>false</c>.</returns>
    public bool TryDequeue(out T result)
    {
        return _queue.TryDequeue(out result);
    }

    /// <inheritdoc />
    public bool TryTake(out T item)
    {
        return TryDequeue(out item);
    }

    /// <summary>
    /// 尝试返回队列开头的对象但不移除。
    /// </summary>
    /// <remarks>
    /// Attempts to return the object at the beginning of the queue without removing it.
    /// </remarks>
    /// <param name="result">队列开头的元素 / The element at the beginning of the queue.</param>
    /// <returns>读取成功返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if an element was read; otherwise, <c>false</c>.</returns>
    public bool TryPeek(out T result)
    {
        return _queue.TryPeek(out result);
    }

    /// <summary>
    /// 移除队列中的所有元素。
    /// </summary>
    /// <remarks>
    /// Removes all elements from the queue.
    /// </remarks>
    public void Clear()
    {
        _queue.Clear();
    }

    /// <inheritdoc />
    public T[] ToArray()
    {
        return _queue.ToArray();
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int index)
    {
        _queue.CopyTo(array, index);
    }

    /// <inheritdoc />
    public void CopyTo(Array array, int index)
    {
        ((ICollection)_queue).CopyTo(array, index);
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
        return _queue.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void TrimToLimit(int extraItemCount = 0)
    {
        while (_queue.Count + extraItemCount > _limit)
        {
            if (!_queue.TryDequeue(out _))
            {
                break;
            }
        }
    }
}
