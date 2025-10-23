// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 双向字典，实现键和值的双向映射。支持在两个方向上进行快速查找。
/// 注意：键和值都必须是唯一的，不允许重复。
/// </summary>
/// <typeparam name="TKey">键的类型。必须是可以作为字典键的类型。</typeparam>
/// <typeparam name="TValue">值的类型。必须是可以作为字典键的类型。</typeparam>
public class BidirectionalDictionary<TKey, TValue>
{
    /// <summary>
    /// 存储键到值的映射关系
    /// </summary>
    private readonly Dictionary<TKey, TValue> _forwardDictionary;

    /// <summary>
    /// 存储值到键的映射关系
    /// </summary>
    private readonly Dictionary<TValue, TKey> _reverseDictionary;

    /// <summary>
    /// 初始化 <see cref="BidirectionalDictionary{TKey, TValue}" /> 类的新实例。
    /// </summary>
    /// <param name="initialCapacity">初始容量，用于预分配内部字典的空间。默认值为8，必须大于等于0。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="initialCapacity"/> 小于0时抛出</exception>
    /// <remarks>
    /// 合理设置初始容量可以避免频繁的内部扩容操作，提高性能。
    /// </remarks>
    public BidirectionalDictionary(int initialCapacity = 8)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(initialCapacity, nameof(initialCapacity));
        _forwardDictionary = new Dictionary<TKey, TValue>(initialCapacity);
        _reverseDictionary = new Dictionary<TValue, TKey>(initialCapacity);
    }

    /// <summary>
    /// 尝试根据值获取对应的键。
    /// </summary>
    /// <param name="value">要查找的值，不能为 null（如果 TValue 是引用类型）。</param>
    /// <param name="key">查找到的键。如果未找到对应的键，将设置为默认值。</param>
    /// <returns>如果成功找到键，则为 true；否则为 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 null 且 TValue 是引用类型时抛出</exception>
    /// <remarks>
    /// 此操作的时间复杂度为 O(1)。
    /// </remarks>
    public bool TryGetKey(TValue value, out TKey key)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return _reverseDictionary.TryGetValue(value, out key);
    }

    /// <summary>
    /// 尝试根据键获取对应的值。
    /// </summary>
    /// <param name="key">要查找的键，不能为 null（如果 TKey 是引用类型）。</param>
    /// <param name="value">查找到的值。如果未找到对应的值，将设置为默认值。</param>
    /// <returns>如果成功找到值，则为 true；否则为 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 null 且 TKey 是引用类型时抛出</exception>
    /// <remarks>
    /// 此操作的时间复杂度为 O(1)。
    /// </remarks>
    public bool TryGetValue(TKey key, out TValue value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return _forwardDictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// 清空双向字典中的所有键值对。
    /// </summary>
    /// <remarks>
    /// 此操作会同时清空正向和反向的映射关系。
    /// 清空后，字典的容量会保持不变，但所有元素都会被移除。
    /// </remarks>
    public void Clear()
    {
        _forwardDictionary.Clear();
        _reverseDictionary.Clear();
    }

    /// <summary>
    /// 尝试向双向字典中添加键值对。
    /// </summary>
    /// <param name="key">要添加的键，不能为 null（如果 TKey 是引用类型）。</param>
    /// <param name="value">要添加的值，不能为 null（如果 TValue 是引用类型）。</param>
    /// <returns>如果成功添加键值对，则为 true；否则为 false。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 null 且对应类型是引用类型时抛出</exception>
    /// <remarks>
    /// 如果键或值已存在，则添加失败并返回 false。
    /// 添加成功时会同时更新正向和反向的映射关系。
    /// 此操作的时间复杂度为 O(1)。
    /// </remarks>
    public bool TryAdd(TKey key, TValue value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        if (_forwardDictionary.TryAdd(key, value))
        {
            _reverseDictionary.Add(value, key);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 尝试根据键移除键值对
    /// </summary>
    /// <param name="key">要移除的键，不能为 null（如果 TKey 是引用类型）。</param>
    /// <param name="value">查找到的值。如果未找到对应的值，将设置为默认值。</param>
    /// <returns>如果成功找到值并移除键值对，则为 true；否则为 false。</returns>
    public bool TryRemoveKey(TKey key, out TValue value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        if (_forwardDictionary.Remove(key, out value))
        {
            _reverseDictionary.Remove(value);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 尝试根据值移除键值对
    /// </summary>
    /// <param name="value">要移除的值，不能为 null（如果 TValue 是引用类型）。</param>
    /// <param name="key">查找到的键。如果未找到对应的值，将设置为默认值。</param>
    /// <returns>如果成功找到值并移除键值对，则为 true；否则为 false。</returns>
    public bool TryRemoveValue(TValue value, out TKey key)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        if (_reverseDictionary.Remove(value, out key))
        {
            _forwardDictionary.Remove(key);
            return true;
        }

        return false;
    }
}