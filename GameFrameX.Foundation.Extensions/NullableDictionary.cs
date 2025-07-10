// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 用于明确指定 NullableDictionary 的 fallback 值的包装类型
/// </summary>
/// <typeparam name="TValue">值的类型</typeparam>
public readonly struct FallbackValue<TValue>
{
    public TValue Value { get; }
    
    public FallbackValue(TValue value)
    {
        Value = value;
    }
    
    public static implicit operator FallbackValue<TValue>(TValue value)
    {
        return new FallbackValue<TValue>(value);
    }
}

/// <summary>
/// 可空字典，支持 null 键和自定义默认值
/// </summary>
/// <typeparam name="TKey">键的类型</typeparam>
/// <typeparam name="TValue">值的类型</typeparam>
public class NullableDictionary<TKey, TValue> : Dictionary<NullObject<TKey>, TValue>
{
    /// <summary>
    /// 当键不存在时返回的默认值的私有字段
    /// </summary>
    private TValue _fallbackValue;

    /// <summary>
    /// 初始化一个空的 NullableDictionary 实例
    /// </summary>
    public NullableDictionary()
    {
        // FallbackValue 将使用字段的默认值
    }

    /// <summary>
    /// 使用指定的初始容量初始化 NullableDictionary 实例
    /// 注意：当 TValue 为 int 类型时，请使用 WithCapacity 静态方法来避免与 fallbackValue 构造函数的歧义
    /// </summary>
    /// <param name="capacity">字典的初始容量</param>
    /// <exception cref="ArgumentOutOfRangeException">当 capacity 小于 0 时抛出</exception>
    public NullableDictionary(int capacity) : base(capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
    }

    /// <summary>
    /// 创建一个带有指定初始容量的 NullableDictionary 实例
    /// </summary>
    /// <param name="capacity">字典的初始容量</param>
    /// <returns>新的 NullableDictionary 实例</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 capacity 小于 0 时抛出</exception>
    public static NullableDictionary<TKey, TValue> WithCapacity(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        return new NullableDictionary<TKey, TValue>(capacity);
    }

    /// <summary>
    /// 使用指定的默认值初始化 NullableDictionary 实例
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值</param>
    public NullableDictionary(TValue fallbackValue) : base()
    {
        _fallbackValue = fallbackValue;
    }

    /// <summary>
    /// 使用指定的默认值初始化 NullableDictionary 实例（明确指定为 fallback 值）
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值</param>
    public NullableDictionary(FallbackValue<TValue> fallbackValue) : base()
    {
        _fallbackValue = fallbackValue.Value;
    }

    /// <summary>
    /// 创建一个带有指定默认值的 NullableDictionary 实例
    /// </summary>
    /// <param name="fallbackValue">当键不存在时返回的默认值</param>
    /// <returns>新的 NullableDictionary 实例</returns>
    public static NullableDictionary<TKey, TValue> WithFallbackValue(TValue fallbackValue)
    {
        var dictionary = new NullableDictionary<TKey, TValue>();
        dictionary._fallbackValue = fallbackValue;
        return dictionary;
    }

    /// <summary>
    /// 使用指定的比较器初始化 NullableDictionary 实例
    /// </summary>
    /// <param name="comparer">用于键的比较器</param>
    public NullableDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        // comparer 可以为 null，Dictionary 会使用默认比较器
    }

    /// <summary>
    /// 使用指定的初始容量和比较器初始化 NullableDictionary 实例
    /// </summary>
    /// <param name="capacity">字典的初始容量</param>
    /// <param name="comparer">用于键的比较器</param>
    /// <exception cref="ArgumentOutOfRangeException">当 capacity 小于 0 时抛出</exception>
    public NullableDictionary(int capacity, IEqualityComparer<NullObject<TKey>> comparer) : base(capacity, comparer)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
    }

    /// <summary>
    /// 使用指定的字典初始化 NullableDictionary 实例
    /// </summary>
    /// <param name="dictionary">用于初始化字典的键值对集合</param>
    /// <exception cref="ArgumentNullException">当 dictionary 为 null 时抛出</exception>
    public NullableDictionary(IDictionary<NullObject<TKey>, TValue> dictionary) : base(dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));
    }

    /// <summary>
    /// 使用指定的字典和比较器初始化 NullableDictionary 实例
    /// </summary>
    /// <param name="dictionary">用于初始化字典的键值对集合</param>
    /// <param name="comparer">用于键的比较器</param>
    /// <exception cref="ArgumentNullException">当 dictionary 为 null 时抛出</exception>
    public NullableDictionary(IDictionary<NullObject<TKey>, TValue> dictionary, IEqualityComparer<NullObject<TKey>> comparer) : base(dictionary, comparer)
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));
    }

    /// <summary>
    /// 当键不存在时返回的默认值
    /// </summary>
    internal TValue FallbackValue 
    { 
        get 
        {
            return _fallbackValue;
        }
        set => _fallbackValue = value; 
    }

    /// <summary>
    /// 获取或设置指定键的值
    /// </summary>
    /// <param name="key">键</param>
    public new TValue this[NullObject<TKey> key]
    {
        get { return TryGetValue(key, out var value) ? value : FallbackValue; }
        set { base[key] = value; }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的值
    /// </summary>
    /// <param name="condition">条件谓词，不能为 null</param>
    /// <returns>匹配条件的第一个值，如果没有匹配项则返回 FallbackValue</returns>
    /// <exception cref="ArgumentNullException">当 condition 为 null 时抛出</exception>
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
    /// 根据条件获取或设置第一个匹配的值
    /// </summary>
    /// <param name="condition">条件谓词，接受键和值作为参数，不能为 null</param>
    /// <returns>匹配条件的第一个值，如果没有匹配项则返回 FallbackValue</returns>
    /// <exception cref="ArgumentNullException">当 condition 为 null 时抛出</exception>
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
    /// 根据条件获取或设置第一个匹配的值
    /// </summary>
    /// <param name="condition">条件谓词，接受键作为参数，不能为 null</param>
    /// <returns>匹配条件的第一个值，如果没有匹配项则返回 FallbackValue</returns>
    /// <exception cref="ArgumentNullException">当 condition 为 null 时抛出</exception>
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
    /// 根据条件获取或设置第一个匹配的值
    /// </summary>
    /// <param name="condition">条件谓词，接受值作为参数，不能为 null</param>
    /// <returns>匹配条件的第一个值，如果没有匹配项则返回 FallbackValue</returns>
    /// <exception cref="ArgumentNullException">当 condition 为 null 时抛出</exception>
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
    /// 获取或设置指定键的值
    /// </summary>
    /// <param name="key">键</param>
    public TValue this[TKey key]
    {
        get { return TryGetValue(new NullObject<TKey>(key), out var value) ? value : FallbackValue; }
        set { base[new NullObject<TKey>(key)] = value; }
    }

    /// <summary>
    /// 判断字典是否包含指定的键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>如果包含则返回 true，否则返回 false</returns>
    public bool ContainsKey(TKey key)
    {
        return base.ContainsKey(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 向字典中添加键值对
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public void Add(TKey key, TValue value)
    {
        base.Add(new NullObject<TKey>(key), value);
    }

    /// <summary>
    /// 从字典中移除指定的键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>如果成功移除则返回 true，否则返回 false</returns>
    public bool Remove(TKey key)
    {
        return base.Remove(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 尝试获取指定键的值
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">输出参数，存储找到的值</param>
    /// <returns>如果找到则返回 true，否则返回 false</returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return base.TryGetValue(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 从 Dictionary&lt;TKey, TValue&gt; 隐式转换为 NullableDictionary&lt;TKey, TValue&gt;
    /// </summary>
    /// <param name="dic">源字典，不能为 null</param>
    /// <returns>转换后的 NullableDictionary 实例</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
    public static implicit operator NullableDictionary<TKey, TValue>(Dictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        
        var nullableDictionary = new NullableDictionary<TKey, TValue>();
        foreach (var p in dic)
        {
            nullableDictionary[p.Key] = p.Value;
        }

        return nullableDictionary;
    }

    /// <summary>
    /// 从 ConcurrentDictionary&lt;TKey, TValue&gt; 隐式转换为 NullableDictionary&lt;TKey, TValue&gt;
    /// </summary>
    /// <param name="dic">源字典，不能为 null</param>
    /// <returns>转换后的 NullableDictionary 实例</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
    public static implicit operator NullableDictionary<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        
        var nullableDictionary = new NullableDictionary<TKey, TValue>();
        foreach (var p in dic)
        {
            nullableDictionary[p.Key] = p.Value;
        }

        return nullableDictionary;
    }

    /// <summary>
    /// 从 NullableDictionary{TKey, TValue} 隐式转换为 Dictionary{TKey, TValue}
    /// </summary>
    /// <param name="dic">源字典，不能为 null</param>
    /// <returns>转换后的 Dictionary 实例</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
    public static implicit operator Dictionary<TKey, TValue>(NullableDictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        
        var dictionary = new Dictionary<TKey, TValue>();
        foreach (var p in dic)
        {
            if (p.Key.Item != null)
            {
                dictionary[p.Key.Item] = p.Value;
            }
        }

        return dictionary;
    }
}