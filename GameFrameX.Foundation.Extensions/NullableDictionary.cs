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

using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 用于明确指定 NullableDictionary 的 fallback 值的包装类型。
/// </summary>
/// <remarks>
/// A wrapper type used to explicitly specify the fallback value for NullableDictionary.
/// </remarks>
/// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
public readonly struct FallbackValue<TValue>
{
    /// <summary>
    /// 获取包装的值。
    /// </summary>
    /// <remarks>
    /// Gets the wrapped value.
    /// </remarks>
    /// <value>包装的泛型类型 T 的值 / The wrapped value of type T.</value>
    public TValue Value { get; }

    /// <summary>
    /// 初始化一个包含指定值的 FallbackValue。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="FallbackValue{TValue}"/> struct with the specified value.
    /// </remarks>
    /// <param name="value">要包装的值 / The value to wrap.</param>
    public FallbackValue(TValue value)
    {
        Value = value;
    }

    /// <summary>
    /// 将 TValue 隐式转换为 FallbackValue。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a value of type TValue to a <see cref="FallbackValue{TValue}"/>.
    /// </remarks>
    /// <param name="value">要转换的值 / The value to convert.</param>
    /// <returns>一个包含指定值的 FallbackValue 实例 / A <see cref="FallbackValue{TValue}"/> instance containing the specified value.</returns>
    public static implicit operator FallbackValue<TValue>(TValue value)
    {
        return new FallbackValue<TValue>(value);
    }
}

/// <summary>
/// 可空字典，支持 null 键和自定义默认值。
/// </summary>
/// <remarks>
/// A dictionary that supports null keys and custom default values.
/// </remarks>
/// <typeparam name="TKey">键的类型 / The type of the keys in the dictionary.</typeparam>
/// <typeparam name="TValue">值的类型 / The type of the values in the dictionary.</typeparam>
public class NullableDictionary<TKey, TValue> : Dictionary<NullObject<TKey>, TValue>
{
    /// <summary>
    /// 当键不存在时返回的默认值。
    /// </summary>
    /// <remarks>
    /// The default value returned when a key does not exist.
    /// </remarks>
    private TValue _fallbackValue;

    /// <summary>
    /// 初始化一个空的 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes an empty <see cref="NullableDictionary{TKey, TValue}"/> instance.
    /// </remarks>
    public NullableDictionary()
    {
        // FallbackValue 将使用字段的默认值
    }

    /// <summary>
    /// 使用指定的初始容量初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// 注意：当 TValue 为 int 类型时，请使用 <see cref="WithCapacity"/> 静态方法来避免与 fallbackValue 构造函数的歧义。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified initial capacity.
    /// Note: When TValue is int, use the <see cref="WithCapacity"/> static method to avoid ambiguity with the fallbackValue constructor.
    /// </remarks>
    /// <param name="capacity">字典的初始容量 / The initial capacity of the dictionary.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="capacity"/> is less than 0.</exception>
    public NullableDictionary(int capacity) : base(capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        }
    }

    /// <summary>
    /// 创建一个带有指定初始容量的 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Creates a new <see cref="NullableDictionary{TKey, TValue}"/> instance with the specified initial capacity.
    /// </remarks>
    /// <param name="capacity">字典的初始容量 / The initial capacity of the dictionary.</param>
    /// <returns>新的 <see cref="NullableDictionary{TKey, TValue}"/> 实例 / A new <see cref="NullableDictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="capacity"/> is less than 0.</exception>
    public static NullableDictionary<TKey, TValue> WithCapacity(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        }

        return new NullableDictionary<TKey, TValue>(capacity);
    }

    /// <summary>
    /// 使用指定的默认值初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified fallback value.
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value returned when a key does not exist.</param>
    public NullableDictionary(TValue fallbackValue) : base()
    {
        _fallbackValue = fallbackValue;
    }

    /// <summary>
    /// 使用指定的默认值初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例（明确指定为 fallback 值）。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified fallback value (explicitly specified as fallback).
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value returned when a key does not exist.</param>
    public NullableDictionary(FallbackValue<TValue> fallbackValue) : base()
    {
        _fallbackValue = fallbackValue.Value;
    }

    /// <summary>
    /// 创建一个带有指定默认值的 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Creates a new <see cref="NullableDictionary{TKey, TValue}"/> instance with the specified fallback value.
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value returned when a key does not exist.</param>
    /// <returns>新的 <see cref="NullableDictionary{TKey, TValue}"/> 实例 / A new <see cref="NullableDictionary{TKey, TValue}"/> instance.</returns>
    public static NullableDictionary<TKey, TValue> WithFallbackValue(TValue fallbackValue)
    {
        var dictionary = new NullableDictionary<TKey, TValue>();
        dictionary._fallbackValue = fallbackValue;
        return dictionary;
    }

    /// <summary>
    /// 使用指定的比较器初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified comparer.
    /// </remarks>
    /// <param name="comparer">用于键的比较器 / The comparer to use for keys.</param>
    public NullableDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        // comparer 可以为 null，Dictionary 会使用默认比较器
    }

    /// <summary>
    /// 使用指定的初始容量和比较器初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified initial capacity and comparer.
    /// </remarks>
    /// <param name="capacity">字典的初始容量 / The initial capacity of the dictionary.</param>
    /// <param name="comparer">用于键的比较器 / The comparer to use for keys.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="capacity"/> is less than 0.</exception>
    public NullableDictionary(int capacity, IEqualityComparer<NullObject<TKey>> comparer) : base(capacity, comparer)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        }
    }

    /// <summary>
    /// 使用指定的字典初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified dictionary.
    /// </remarks>
    /// <param name="dictionary">用于初始化字典的键值对集合 / The key-value pairs to initialize the dictionary.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dictionary"/> 为 null 时抛出 / Thrown when <paramref name="dictionary"/> is null.</exception>
    public NullableDictionary(IDictionary<NullObject<TKey>, TValue> dictionary) : base(dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));
    }

    /// <summary>
    /// 使用指定的字典和比较器初始化 <see cref="NullableDictionary{TKey, TValue}"/> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of <see cref="NullableDictionary{TKey, TValue}"/> with the specified dictionary and comparer.
    /// </remarks>
    /// <param name="dictionary">用于初始化字典的键值对集合 / The key-value pairs to initialize the dictionary.</param>
    /// <param name="comparer">用于键的比较器 / The comparer to use for keys.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dictionary"/> 为 null 时抛出 / Thrown when <paramref name="dictionary"/> is null.</exception>
    public NullableDictionary(IDictionary<NullObject<TKey>, TValue> dictionary, IEqualityComparer<NullObject<TKey>> comparer) : base(dictionary, comparer)
    {
        ArgumentNullException.ThrowIfNull(dictionary, nameof(dictionary));
    }

    /// <summary>
    /// 获取或设置当键不存在时返回的默认值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the default value returned when a key does not exist.
    /// </remarks>
    /// <value>当键不存在时返回的默认值 / The default value returned when a key does not exist.</value>
    internal TValue FallbackValue
    {
        get { return _fallbackValue; }
        set => _fallbackValue = value;
    }

    /// <summary>
    /// 获取或设置指定键的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value associated with the specified key.
    /// </remarks>
    /// <param name="key">要获取或设置的键 / The key to get or set.</param>
    /// <value>与指定键关联的值；如果键不存在则返回 <see cref="FallbackValue"/> / The value associated with the specified key; returns <see cref="FallbackValue"/> if the key does not exist.</value>
    public new TValue this[NullObject<TKey> key]
    {
        get { return TryGetValue(key, out var value) ? value : FallbackValue; }
        set { base[key] = value; }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the first value that matches the specified condition.
    /// </remarks>
    /// <param name="condition">条件谓词，用于测试每个键值对 / The predicate to test each key-value pair.</param>
    /// <value>匹配条件的第一个值；如果没有匹配项则返回 <see cref="FallbackValue"/> / The first value that matches the condition; returns <see cref="FallbackValue"/> if no match is found.</value>
    /// <exception cref="ArgumentNullException">当 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="condition"/> is null.</exception>
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
    /// 根据条件获取或设置第一个匹配的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the first value that matches the specified condition.
    /// </remarks>
    /// <param name="condition">条件谓词，接受键和值作为参数 / The predicate that accepts key and value as parameters.</param>
    /// <value>匹配条件的第一个值；如果没有匹配项则返回 <see cref="FallbackValue"/> / The first value that matches the condition; returns <see cref="FallbackValue"/> if no match is found.</value>
    /// <exception cref="ArgumentNullException">当 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="condition"/> is null.</exception>
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
    /// 根据条件获取或设置第一个匹配的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the first value that matches the specified condition.
    /// </remarks>
    /// <param name="condition">条件谓词，接受键作为参数 / The predicate that accepts key as parameter.</param>
    /// <value>匹配条件的第一个值；如果没有匹配项则返回 <see cref="FallbackValue"/> / The first value that matches the condition; returns <see cref="FallbackValue"/> if no match is found.</value>
    /// <exception cref="ArgumentNullException">当 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="condition"/> is null.</exception>
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
    /// 根据条件获取或设置第一个匹配的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the first value that matches the specified condition.
    /// </remarks>
    /// <param name="condition">条件谓词，接受值作为参数 / The predicate that accepts value as parameter.</param>
    /// <value>匹配条件的第一个值；如果没有匹配项则返回 <see cref="FallbackValue"/> / The first value that matches the condition; returns <see cref="FallbackValue"/> if no match is found.</value>
    /// <exception cref="ArgumentNullException">当 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="condition"/> is null.</exception>
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
    /// <remarks>
    /// Gets or sets the value associated with the specified key.
    /// </remarks>
    /// <param name="key">要获取或设置的键 / The key to get or set.</param>
    /// <value>与指定键关联的值；如果键不存在则返回 <see cref="FallbackValue"/> / The value associated with the specified key; returns <see cref="FallbackValue"/> if the key does not exist.</value>
    public TValue this[TKey key]
    {
        get { return TryGetValue(new NullObject<TKey>(key), out var value) ? value : FallbackValue; }
        set { base[new NullObject<TKey>(key)] = value; }
    }

    /// <summary>
    /// 判断字典是否包含指定的键。
    /// </summary>
    /// <remarks>
    /// Determines whether the dictionary contains the specified key.
    /// </remarks>
    /// <param name="key">要查找的键 / The key to locate.</param>
    /// <returns>如果字典包含指定的键则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the dictionary contains the specified key; otherwise <c>false</c>.</returns>
    public bool ContainsKey(TKey key)
    {
        return base.ContainsKey(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 向字典中添加指定的键值对。
    /// </summary>
    /// <remarks>
    /// Adds the specified key and value to the dictionary.
    /// </remarks>
    /// <param name="key">要添加的键 / The key to add.</param>
    /// <param name="value">要添加的值 / The value to add.</param>
    public void Add(TKey key, TValue value)
    {
        base.Add(new NullObject<TKey>(key), value);
    }

    /// <summary>
    /// 从字典中移除指定键的值。
    /// </summary>
    /// <remarks>
    /// Removes the value with the specified key from the dictionary.
    /// </remarks>
    /// <param name="key">要移除的键 / The key to remove.</param>
    /// <returns>如果成功移除则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the element is successfully removed; otherwise <c>false</c>.</returns>
    public bool Remove(TKey key)
    {
        return base.Remove(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 尝试获取与指定键关联的值。
    /// </summary>
    /// <remarks>
    /// Attempts to get the value associated with the specified key.
    /// </remarks>
    /// <param name="key">要查找的键 / The key to locate.</param>
    /// <param name="value">输出参数，存储找到的值 / Output parameter that stores the found value.</param>
    /// <returns>如果找到则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the key is found; otherwise <c>false</c>.</returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return base.TryGetValue(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 从 <see cref="Dictionary{TKey, TValue}"/> 隐式转换为 <see cref="NullableDictionary{TKey, TValue}"/>。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a <see cref="Dictionary{TKey, TValue}"/> to a <see cref="NullableDictionary{TKey, TValue}"/>.
    /// </remarks>
    /// <param name="dic">源字典 / The source dictionary.</param>
    /// <returns>转换后的 <see cref="NullableDictionary{TKey, TValue}"/> 实例 / The converted <see cref="NullableDictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
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
    /// 从 <see cref="ConcurrentDictionary{TKey, TValue}"/> 隐式转换为 <see cref="NullableDictionary{TKey, TValue}"/>。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a <see cref="ConcurrentDictionary{TKey, TValue}"/> to a <see cref="NullableDictionary{TKey, TValue}"/>.
    /// </remarks>
    /// <param name="dic">源字典 / The source dictionary.</param>
    /// <returns>转换后的 <see cref="NullableDictionary{TKey, TValue}"/> 实例 / The converted <see cref="NullableDictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
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
    /// 从 <see cref="NullableDictionary{TKey, TValue}"/> 隐式转换为 <see cref="Dictionary{TKey, TValue}"/>。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a <see cref="NullableDictionary{TKey, TValue}"/> to a <see cref="Dictionary{TKey, TValue}"/>.
    /// </remarks>
    /// <param name="dic">源字典 / The source dictionary.</param>
    /// <returns>转换后的 <see cref="Dictionary{TKey, TValue}"/> 实例 / The converted <see cref="Dictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
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