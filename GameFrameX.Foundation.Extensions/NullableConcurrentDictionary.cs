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
    /// 初始化一个新的 <see cref="NullableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    public NullableConcurrentDictionary()
    {
    }

    /// <summary>
    /// 使用指定的默认值初始化一个新的 <see cref="NullableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值，可以为null（如果TValue是引用类型）。</param>
    public NullableConcurrentDictionary(TValue fallbackValue)
    {
        FallbackValue = fallbackValue;
    }

    /// <summary>
    /// 使用指定的并发级别和初始容量初始化一个新的 <see cref="NullableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <param name="concurrencyLevel">并发级别，必须大于0。</param>
    /// <param name="capacity">初始容量，必须大于0。</param>
    /// <exception cref="ArgumentOutOfRangeException">当<paramref name="concurrencyLevel"/>或<paramref name="capacity"/>小于或等于0时抛出。</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
    {
        if (concurrencyLevel <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(concurrencyLevel), "The concurrency level must be positive.");
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity must be positive.");
        }
    }

    /// <summary>
    /// 使用指定的比较器初始化一个新的 <see cref="NullableConcurrentDictionary{TKey, TValue}" /> 实例。
    /// </summary>
    /// <param name="comparer">用于比较键的比较器，不能为null。</param>
    /// <exception cref="ArgumentNullException">当<paramref name="comparer"/>为null时抛出。</exception>
    public NullableConcurrentDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 获取或设置当键不存在时返回的默认值。
    /// </summary>
    internal TValue FallbackValue { get; set; }

    /// <summary>
    /// 获取或设置指定键的值。
    /// </summary>
    /// <param name="key">键，不能为null。</param>
    /// <returns>如果找到该键，则返回对应的值；否则返回默认值。</returns>
    /// <exception cref="ArgumentNullException">当<paramref name="key"/>为null时抛出。</exception>
    public new TValue this[NullObject<TKey> key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return base.TryGetValue(key, out var value) ? value : FallbackValue;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
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
    /// 判断字典中是否包含指定的键。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <returns>如果包含指定的键，则返回 true；否则返回 false。</returns>
    public bool ContainsKey(TKey key)
    {
        return base.ContainsKey(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 尝试添加一个键值对。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <param name="value">值，可以为null（如果TValue是引用类型）。</param>
    /// <returns>如果成功添加，则返回 true；否则返回 false。</returns>
    public bool TryAdd(TKey key, TValue value)
    {
        return base.TryAdd(new NullObject<TKey>(key), value);
    }

    /// <summary>
    /// 尝试移除一个键值对。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <param name="value">移除的值。如果键存在，则包含被移除的值；否则包含默认值。</param>
    /// <returns>如果成功移除，则返回 true；否则返回 false。</returns>
    public bool TryRemove(TKey key, out TValue value)
    {
        return base.TryRemove(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 尝试更新一个键值对。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <param name="value">新的值，可以为null（如果TValue是引用类型）。</param>
    /// <param name="comparisionValue">比较值，用于确保更新操作的原子性。</param>
    /// <returns>如果成功更新，则返回 true；否则返回 false。</returns>
    public bool TryUpdate(TKey key, TValue value, TValue comparisionValue)
    {
        return base.TryUpdate(new NullObject<TKey>(key), value, comparisionValue);
    }

    /// <summary>
    /// 尝试获取指定键的值。
    /// </summary>
    /// <param name="key">键，可以为null。</param>
    /// <param name="value">获取的值。如果键存在，则包含对应的值；否则包含默认值。</param>
    /// <returns>如果成功获取，则返回 true；否则返回 false。</returns>
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
            concurrentDictionary[p.Key] = p.Value;
        }

        return concurrentDictionary;
    }
}