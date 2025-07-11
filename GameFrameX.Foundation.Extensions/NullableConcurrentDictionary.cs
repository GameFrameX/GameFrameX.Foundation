// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 支持null键和值的并发字典类型
/// </summary>
/// <typeparam name="TKey">键的类型，可以为任意类型，包括引用类型和值类型</typeparam>
/// <typeparam name="TValue">值的类型，可以为任意类型，包括引用类型和值类型</typeparam>
public class NullableConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<NullObject<TKey>, TValue>
{
    /// <summary>
    /// 当键不存在时返回的默认值
    /// </summary>
    private TValue _fallbackValue;

    /// <summary>
    /// 初始化一个空的 NullableConcurrentDictionary 实例
    /// </summary>
    public NullableConcurrentDictionary()
    {
        // FallbackValue 将使用字段的默认值
    }

    /// <summary>
    /// 使用指定的初始容量初始化 NullableConcurrentDictionary 实例
    /// 注意：当 TValue 为 int 类型时，请使用 WithCapacity 静态方法来避免与 fallbackValue 构造函数的歧义
    /// </summary>
    /// <param name="capacity">字典的初始容量，必须大于等于0</param>
    /// <exception cref="ArgumentOutOfRangeException">当 capacity 小于 0 时抛出</exception>
    public NullableConcurrentDictionary(int capacity) : base(Environment.ProcessorCount, capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
    }

    /// <summary>
    /// 使用指定的并发级别和初始容量初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="concurrencyLevel">并发级别，必须大于0</param>
    /// <param name="capacity">字典的初始容量，必须大于等于0</param>
    /// <exception cref="ArgumentOutOfRangeException">当 concurrencyLevel 小于等于 0 或 capacity 小于 0 时抛出</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
    }

    /// <summary>
    /// 创建一个带有指定初始容量的 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="capacity">字典的初始容量，必须大于等于0</param>
    /// <returns>新的 NullableConcurrentDictionary 实例</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 capacity 小于 0 时抛出</exception>
    public static NullableConcurrentDictionary<TKey, TValue> WithCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        return new NullableConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);
    }

    /// <summary>
    /// 使用指定的默认值初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值</param>
    public NullableConcurrentDictionary(TValue fallbackValue) : base()
    {
        _fallbackValue = fallbackValue;
    }

    /// <summary>
    /// 使用指定的默认值初始化 NullableConcurrentDictionary 实例（明确指定为 fallback 值）
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值</param>
    public NullableConcurrentDictionary(FallbackValue<TValue> fallbackValue) : base()
    {
        _fallbackValue = fallbackValue.Value;
    }

    /// <summary>
    /// 使用指定的比较器初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="comparer">用于键的比较器，不能为null</param>
    /// <exception cref="ArgumentNullException">当 comparer 为 null 时抛出</exception>
    public NullableConcurrentDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 使用指定的并发级别、初始容量和比较器初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="concurrencyLevel">并发级别，必须大于0</param>
    /// <param name="capacity">字典的初始容量，必须大于等于0</param>
    /// <param name="comparer">用于键的比较器，不能为null</param>
    /// <exception cref="ArgumentOutOfRangeException">当 concurrencyLevel 小于等于 0 或 capacity 小于 0 时抛出</exception>
    /// <exception cref="ArgumentNullException">当 comparer 为 null 时抛出</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<NullObject<TKey>> comparer) : base(concurrencyLevel, capacity, comparer)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 使用指定的字典初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="collection">用于初始化字典的键值对集合，不能为null</param>
    /// <exception cref="ArgumentNullException">当 collection 为 null 时抛出</exception>
    public NullableConcurrentDictionary(IEnumerable<KeyValuePair<NullObject<TKey>, TValue>> collection) : base(collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
    }

    /// <summary>
    /// 使用指定的字典和比较器初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="collection">用于初始化字典的键值对集合，不能为null</param>
    /// <param name="comparer">用于键的比较器，不能为null</param>
    /// <exception cref="ArgumentNullException">当 collection 或 comparer 为 null 时抛出</exception>
    public NullableConcurrentDictionary(IEnumerable<KeyValuePair<NullObject<TKey>, TValue>> collection, IEqualityComparer<NullObject<TKey>> comparer) : base(collection, comparer)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 使用指定的并发级别、字典和比较器初始化 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="concurrencyLevel">并发级别，必须大于0</param>
    /// <param name="collection">用于初始化字典的键值对集合，不能为null</param>
    /// <param name="comparer">用于键的比较器，不能为null</param>
    /// <exception cref="ArgumentOutOfRangeException">当 concurrencyLevel 小于等于 0 时抛出</exception>
    /// <exception cref="ArgumentNullException">当 collection 或 comparer 为 null 时抛出</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<NullObject<TKey>, TValue>> collection, IEqualityComparer<NullObject<TKey>> comparer) : base(concurrencyLevel, collection, comparer)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 创建一个带有指定默认值的 NullableConcurrentDictionary 实例
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值</param>
    /// <returns>新的 NullableConcurrentDictionary 实例</returns>
    public static NullableConcurrentDictionary<TKey, TValue> WithFallbackValue(TValue fallbackValue)
    {
        var dictionary = new NullableConcurrentDictionary<TKey, TValue>();
        dictionary._fallbackValue = fallbackValue;
        return dictionary;
    }

    /// <summary>
    /// 获取或设置当键不存在时返回的默认值。
    /// </summary>
    public TValue FallbackValue 
    { 
        get => _fallbackValue; 
        set => _fallbackValue = value; 
    }

    /// <summary>
    /// 获取或设置指定键的值。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <returns>如果找到该键，则返回对应的值；否则返回默认值。</returns>
    public new TValue this[NullObject<TKey> key]
    {
        get
        {
            return base.TryGetValue(key, out var value) ? value : FallbackValue;
        }
        set
        {
            base[key] = value;
        }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <param name="condition">用于筛选键值对的条件，不能为null。</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="condition"/>为null时抛出。</exception>
    public TValue this[Func<KeyValuePair<TKey, TValue>, bool> condition]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(new KeyValuePair<TKey, TValue>(pair.Key.Item, pair.Value))))
            {
                return pair.Value;
            }

            return FallbackValue;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(new KeyValuePair<TKey, TValue>(pair.Key.Item, pair.Value))))
            {
                this[pair.Key] = value;
            }
        }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <param name="condition">用于筛选键值对的条件，不能为null。</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="condition"/>为null时抛出。</exception>
    public TValue this[Func<TKey, TValue, bool> condition]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(pair.Key.Item, pair.Value)))
            {
                return pair.Value;
            }

            return FallbackValue;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(pair.Key.Item, pair.Value)))
            {
                this[pair.Key] = value;
            }
        }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <param name="condition">用于筛选键的条件，不能为null。</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="condition"/>为null时抛出。</exception>
    public TValue this[Func<TKey, bool> condition]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(pair.Key.Item)))
            {
                return pair.Value;
            }

            return FallbackValue;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(pair.Key.Item)))
            {
                this[pair.Key] = value;
            }
        }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <param name="condition">用于筛选值的条件，不能为null。</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="condition"/>为null时抛出。</exception>
    public TValue this[Func<TValue, bool> condition]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(pair.Value)))
            {
                return pair.Value;
            }

            return FallbackValue;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(condition, nameof(condition));

            foreach (var pair in this.Where(pair => condition(pair.Value)))
            {
                this[pair.Key] = value;
            }
        }
    }

    /// <summary>
    /// 获取或设置指定键的值。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <returns>如果找到该键，则返回对应的值；否则返回默认值。</returns>
    public TValue this[TKey key]
    {
        get { return base.TryGetValue(new NullObject<TKey>(key), out var value) ? value : FallbackValue; }
        set { base[new NullObject<TKey>(key)] = value; }
    }

    /// <summary>
    /// 判断字典是否包含指定的键
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <returns>如果包含则返回 true，否则返回 false</returns>
    public bool ContainsKey(TKey key)
    {
        return base.ContainsKey(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 向字典中添加键值对
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <param name="value">值，可以为null（如果TValue是引用类型）</param>
    public void Add(TKey key, TValue value)
    {
        base[new NullObject<TKey>(key)] = value;
    }

    /// <summary>
    /// 尝试向字典中添加键值对
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <param name="value">值，可以为null（如果TValue是引用类型）</param>
    /// <returns>如果成功添加则返回 true，否则返回 false</returns>
    public bool TryAdd(TKey key, TValue value)
    {
        return base.TryAdd(new NullObject<TKey>(key), value);
    }

    /// <summary>
    /// 从字典中移除指定的键
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <returns>如果成功移除则返回 true，否则返回 false</returns>
    public bool Remove(TKey key)
    {
        return base.TryRemove(new NullObject<TKey>(key), out _);
    }

    /// <summary>
    /// 尝试从字典中移除指定的键
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <param name="value">输出参数，存储被移除的值</param>
    /// <returns>如果成功移除则返回 true，否则返回 false</returns>
    public bool TryRemove(TKey key, out TValue value)
    {
        return base.TryRemove(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 尝试更新字典中指定键的值
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <param name="newValue">新值，可以为null（如果TValue是引用类型）</param>
    /// <param name="comparisonValue">比较值，可以为null（如果TValue是引用类型）</param>
    /// <returns>如果成功更新则返回 true，否则返回 false</returns>
    public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
    {
        return base.TryUpdate(new NullObject<TKey>(key), newValue, comparisonValue);
    }

    /// <summary>
    /// 尝试获取指定键的值
    /// </summary>
    /// <param name="key">键，可以为null</param>
    /// <param name="value">输出参数，存储找到的值</param>
    /// <returns>如果找到则返回 true，否则返回 false</returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return base.TryGetValue(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 从 <see cref="Dictionary{TKey, TValue}" /> 隐式转换为 <see cref="NullableConcurrentDictionary{TKey, TValue}" />。
    /// </summary>
    /// <param name="dic">要转换的字典，不能为null。</param>
    /// <returns>转换后的<see cref="NullableConcurrentDictionary{TKey, TValue}"/>实例。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dic"/>为null时抛出。</exception>
    public static implicit operator NullableConcurrentDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        var nullableDictionary = new NullableConcurrentDictionary<TKey, TValue>();
        foreach (var p in dic)
        {
            nullableDictionary[p.Key] = p.Value;
        }

        return nullableDictionary;
    }

    /// <summary>
    /// 从 <see cref="ConcurrentDictionary{TKey, TValue}" /> 隐式转换为 <see cref="NullableConcurrentDictionary{TKey, TValue}" />。
    /// </summary>
    /// <param name="dic">要转换的并发字典，不能为null。</param>
    /// <returns>转换后的<see cref="NullableConcurrentDictionary{TKey, TValue}"/>实例。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dic"/>为null时抛出。</exception>
    public static implicit operator NullableConcurrentDictionary<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        var nullableDictionary = new NullableConcurrentDictionary<TKey, TValue>();
        foreach (var p in dic)
        {
            nullableDictionary[p.Key] = p.Value;
        }

        return nullableDictionary;
    }

    /// <summary>
    /// 从 <see cref="NullableConcurrentDictionary{TKey, TValue}" /> 隐式转换为 <see cref="ConcurrentDictionary{TKey, TValue}" />。
    /// </summary>
    /// <param name="dic">要转换的字典，不能为null。</param>
    /// <returns>转换后的<see cref="ConcurrentDictionary{TKey, TValue}"/>实例。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="dic"/>为null时抛出。</exception>
    public static implicit operator ConcurrentDictionary<TKey, TValue>(NullableConcurrentDictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        var concurrentDictionary = new ConcurrentDictionary<TKey, TValue>();
        foreach (var p in dic)
        {
            concurrentDictionary[p.Key.Item] = p.Value;
        }

        return concurrentDictionary;
    }
}