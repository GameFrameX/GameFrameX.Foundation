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
/// 支持 null 键和值的并发字典类型。
/// </summary>
/// <remarks>
/// A concurrent dictionary type that supports null keys and values.
/// </remarks>
/// <typeparam name="TKey">键的类型，可以为任意类型，包括引用类型和值类型 / The type of keys, can be any type including reference types and value types.</typeparam>
/// <typeparam name="TValue">值的类型，可以为任意类型，包括引用类型和值类型 / The type of values, can be any type including reference types and value types.</typeparam>
public class NullableConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<NullObject<TKey>, TValue>
{
    /// <summary>
    /// 当键不存在时返回的默认值。
    /// </summary>
    private TValue _fallbackValue;

    /// <summary>
    /// 初始化一个空的 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes an empty NullableConcurrentDictionary instance.
    /// </remarks>
    public NullableConcurrentDictionary()
    {
        // FallbackValue 将使用字段的默认值
    }

    /// <summary>
    /// 使用指定的初始容量初始化 NullableConcurrentDictionary 实例。
    /// 注意：当 TValue 为 int 类型时，请使用 WithCapacity 静态方法来避免与 fallbackValue 构造函数的歧义。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified initial capacity.
    /// Note: When TValue is int, use the WithCapacity static method to avoid ambiguity with the fallbackValue constructor.
    /// </remarks>
    /// <param name="capacity">字典的初始容量，必须大于等于 0 / The initial capacity of the dictionary, must be greater than or equal to 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="capacity"/> is less than 0.</exception>
    public NullableConcurrentDictionary(int capacity) : base(Environment.ProcessorCount, capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
    }

    /// <summary>
    /// 使用指定的并发级别和初始容量初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified concurrency level and initial capacity.
    /// </remarks>
    /// <param name="concurrencyLevel">并发级别，必须大于 0 / The concurrency level, must be greater than 0.</param>
    /// <param name="capacity">字典的初始容量，必须大于等于 0 / The initial capacity of the dictionary, must be greater than or equal to 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="concurrencyLevel"/> 小于等于 0 或 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="concurrencyLevel"/> is less than or equal to 0 or <paramref name="capacity"/> is less than 0.</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, int capacity) : base(concurrencyLevel, capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
    }

    /// <summary>
    /// 创建一个带有指定初始容量的 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Creates a NullableConcurrentDictionary instance with the specified initial capacity.
    /// </remarks>
    /// <param name="capacity">字典的初始容量，必须大于等于 0 / The initial capacity of the dictionary, must be greater than or equal to 0.</param>
    /// <returns>新的 NullableConcurrentDictionary 实例 / A new NullableConcurrentDictionary instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="capacity"/> is less than 0.</exception>
    public static NullableConcurrentDictionary<TKey, TValue> WithCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        return new NullableConcurrentDictionary<TKey, TValue>(Environment.ProcessorCount, capacity);
    }

    /// <summary>
    /// 使用指定的默认值初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified default value.
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value to return when a key does not exist.</param>
    public NullableConcurrentDictionary(TValue fallbackValue) : base()
    {
        _fallbackValue = fallbackValue;
    }

    /// <summary>
    /// 使用指定的默认值初始化 NullableConcurrentDictionary 实例（明确指定为 fallback 值）。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified default value (explicitly specified as fallback value).
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value to return when a key does not exist.</param>
    public NullableConcurrentDictionary(FallbackValue<TValue> fallbackValue) : base()
    {
        _fallbackValue = fallbackValue.Value;
    }

    /// <summary>
    /// 使用指定的比较器初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified comparer.
    /// </remarks>
    /// <param name="comparer">用于键的比较器，不能为 null / The comparer for keys, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="comparer"/> 为 null 时抛出 / Thrown when <paramref name="comparer"/> is null.</exception>
    public NullableConcurrentDictionary(IEqualityComparer<NullObject<TKey>> comparer) : base(comparer)
    {
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 使用指定的并发级别、初始容量和比较器初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified concurrency level, initial capacity, and comparer.
    /// </remarks>
    /// <param name="concurrencyLevel">并发级别，必须大于 0 / The concurrency level, must be greater than 0.</param>
    /// <param name="capacity">字典的初始容量，必须大于等于 0 / The initial capacity of the dictionary, must be greater than or equal to 0.</param>
    /// <param name="comparer">用于键的比较器，不能为 null / The comparer for keys, cannot be null.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="concurrencyLevel"/> 小于等于 0 或 <paramref name="capacity"/> 小于 0 时抛出 / Thrown when <paramref name="concurrencyLevel"/> is less than or equal to 0 or <paramref name="capacity"/> is less than 0.</exception>
    /// <exception cref="ArgumentNullException">当 <paramref name="comparer"/> 为 null 时抛出 / Thrown when <paramref name="comparer"/> is null.</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<NullObject<TKey>> comparer) : base(concurrencyLevel, capacity, comparer)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(capacity, nameof(capacity));
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 使用指定的字典初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified dictionary.
    /// </remarks>
    /// <param name="collection">用于初始化字典的键值对集合，不能为 null / The key-value pair collection to initialize the dictionary, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="collection"/> 为 null 时抛出 / Thrown when <paramref name="collection"/> is null.</exception>
    public NullableConcurrentDictionary(IEnumerable<KeyValuePair<NullObject<TKey>, TValue>> collection) : base(collection)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
    }

    /// <summary>
    /// 使用指定的字典和比较器初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified dictionary and comparer.
    /// </remarks>
    /// <param name="collection">用于初始化字典的键值对集合，不能为 null / The key-value pair collection to initialize the dictionary, cannot be null.</param>
    /// <param name="comparer">用于键的比较器，不能为 null / The comparer for keys, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="collection"/> 或 <paramref name="comparer"/> 为 null 时抛出 / Thrown when <paramref name="collection"/> or <paramref name="comparer"/> is null.</exception>
    public NullableConcurrentDictionary(IEnumerable<KeyValuePair<NullObject<TKey>, TValue>> collection, IEqualityComparer<NullObject<TKey>> comparer) : base(collection, comparer)
    {
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 使用指定的并发级别、字典和比较器初始化 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a NullableConcurrentDictionary instance with the specified concurrency level, dictionary, and comparer.
    /// </remarks>
    /// <param name="concurrencyLevel">并发级别，必须大于 0 / The concurrency level, must be greater than 0.</param>
    /// <param name="collection">用于初始化字典的键值对集合，不能为 null / The key-value pair collection to initialize the dictionary, cannot be null.</param>
    /// <param name="comparer">用于键的比较器，不能为 null / The comparer for keys, cannot be null.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="concurrencyLevel"/> 小于等于 0 时抛出 / Thrown when <paramref name="concurrencyLevel"/> is less than or equal to 0.</exception>
    /// <exception cref="ArgumentNullException">当 <paramref name="collection"/> 或 <paramref name="comparer"/> 为 null 时抛出 / Thrown when <paramref name="collection"/> or <paramref name="comparer"/> is null.</exception>
    public NullableConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<NullObject<TKey>, TValue>> collection, IEqualityComparer<NullObject<TKey>> comparer) : base(concurrencyLevel, collection, comparer)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(concurrencyLevel, nameof(concurrencyLevel));
        ArgumentNullException.ThrowIfNull(collection, nameof(collection));
        ArgumentNullException.ThrowIfNull(comparer, nameof(comparer));
    }

    /// <summary>
    /// 创建一个带有指定默认值的 NullableConcurrentDictionary 实例。
    /// </summary>
    /// <remarks>
    /// Creates a NullableConcurrentDictionary instance with the specified default value.
    /// </remarks>
    /// <param name="fallbackValue">当键不存在时返回的默认值 / The default value to return when a key does not exist.</param>
    /// <returns>新的 NullableConcurrentDictionary 实例 / A new NullableConcurrentDictionary instance.</returns>
    public static NullableConcurrentDictionary<TKey, TValue> WithFallbackValue(TValue fallbackValue)
    {
        var dictionary = new NullableConcurrentDictionary<TKey, TValue>();
        dictionary._fallbackValue = fallbackValue;
        return dictionary;
    }

    /// <summary>
    /// 获取或设置当键不存在时返回的默认值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the default value returned when a key does not exist.
    /// </remarks>
    public TValue FallbackValue
    {
        get => _fallbackValue;
        set => _fallbackValue = value;
    }

    /// <summary>
    /// 获取或设置指定键的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value for the specified key.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <returns>如果找到该键，则返回对应的值；否则返回默认值 / Returns the value if the key is found; otherwise, returns the default value.</returns>
    public new TValue this[NullObject<TKey> key]
    {
        get { return base.TryGetValue(key, out var value) ? value : FallbackValue; }
        set { base[key] = value; }
    }

    /// <summary>
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value of the first matching key-value pair based on the condition.
    /// </remarks>
    /// <param name="condition">用于筛选键值对的条件，不能为 null / The condition to filter key-value pairs, cannot be null.</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值 / The value of the first key-value pair matching the condition; returns the default value if no match is found.</returns>
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
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value of the first matching key-value pair based on the condition.
    /// </remarks>
    /// <param name="condition">用于筛选键值对的条件，不能为 null / The condition to filter key-value pairs, cannot be null.</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值 / The value of the first key-value pair matching the condition; returns the default value if no match is found.</returns>
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
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value of the first matching key-value pair based on the key condition.
    /// </remarks>
    /// <param name="condition">用于筛选键的条件，不能为 null / The condition to filter keys, cannot be null.</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值 / The value of the first key-value pair matching the condition; returns the default value if no match is found.</returns>
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
    /// 根据条件获取或设置第一个匹配的键值对的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value of the first matching key-value pair based on the value condition.
    /// </remarks>
    /// <param name="condition">用于筛选值的条件，不能为 null / The condition to filter values, cannot be null.</param>
    /// <returns>匹配条件的第一个键值对的值；如果没有匹配项，则返回默认值 / The value of the first key-value pair matching the condition; returns the default value if no match is found.</returns>
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
    /// Gets or sets the value for the specified key.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <returns>如果找到该键，则返回对应的值；否则返回默认值 / Returns the value if the key is found; otherwise, returns the default value.</returns>
    public TValue this[TKey key]
    {
        get { return base.TryGetValue(new NullObject<TKey>(key), out var value) ? value : FallbackValue; }
        set { base[new NullObject<TKey>(key)] = value; }
    }

    /// <summary>
    /// 判断字典是否包含指定的键。
    /// </summary>
    /// <remarks>
    /// Determines whether the dictionary contains the specified key.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <returns>如果包含则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the key is contained; otherwise, <c>false</c>.</returns>
    public bool ContainsKey(TKey key)
    {
        return base.ContainsKey(new NullObject<TKey>(key));
    }

    /// <summary>
    /// 向字典中添加键值对。
    /// </summary>
    /// <remarks>
    /// Adds a key-value pair to the dictionary.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <param name="value">值，可以为 null（如果 TValue 是引用类型） / The value, can be null if TValue is a reference type.</param>
    public void Add(TKey key, TValue value)
    {
        base[new NullObject<TKey>(key)] = value;
    }

    /// <summary>
    /// 尝试向字典中添加键值对。
    /// </summary>
    /// <remarks>
    /// Attempts to add a key-value pair to the dictionary.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <param name="value">值，可以为 null（如果 TValue 是引用类型） / The value, can be null if TValue is a reference type.</param>
    /// <returns>如果成功添加则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if added successfully; otherwise, <c>false</c>.</returns>
    public bool TryAdd(TKey key, TValue value)
    {
        return base.TryAdd(new NullObject<TKey>(key), value);
    }

    /// <summary>
    /// 从字典中移除指定的键。
    /// </summary>
    /// <remarks>
    /// Removes the specified key from the dictionary.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <returns>如果成功移除则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if removed successfully; otherwise, <c>false</c>.</returns>
    public bool Remove(TKey key)
    {
        return base.TryRemove(new NullObject<TKey>(key), out _);
    }

    /// <summary>
    /// 尝试从字典中移除指定的键。
    /// </summary>
    /// <remarks>
    /// Attempts to remove the specified key from the dictionary.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <param name="value">输出参数，存储被移除的值 / Output parameter storing the removed value.</param>
    /// <returns>如果成功移除则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if removed successfully; otherwise, <c>false</c>.</returns>
    public bool TryRemove(TKey key, out TValue value)
    {
        return base.TryRemove(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 尝试更新字典中指定键的值。
    /// </summary>
    /// <remarks>
    /// Attempts to update the value for the specified key in the dictionary.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <param name="newValue">新值，可以为 null（如果 TValue 是引用类型） / The new value, can be null if TValue is a reference type.</param>
    /// <param name="comparisonValue">比较值，可以为 null（如果 TValue 是引用类型） / The comparison value, can be null if TValue is a reference type.</param>
    /// <returns>如果成功更新则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if updated successfully; otherwise, <c>false</c>.</returns>
    public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
    {
        return base.TryUpdate(new NullObject<TKey>(key), newValue, comparisonValue);
    }

    /// <summary>
    /// 尝试获取指定键的值。
    /// </summary>
    /// <remarks>
    /// Attempts to get the value for the specified key.
    /// </remarks>
    /// <param name="key">键，可以为 null / The key, can be null.</param>
    /// <param name="value">输出参数，存储找到的值 / Output parameter storing the found value.</param>
    /// <returns>如果找到则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if found; otherwise, <c>false</c>.</returns>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return base.TryGetValue(new NullObject<TKey>(key), out value);
    }

    /// <summary>
    /// 从 <see cref="Dictionary{TKey, TValue}" /> 隐式转换为 <see cref="NullableConcurrentDictionary{TKey, TValue}" />。
    /// </summary>
    /// <remarks>
    /// Implicitly converts from <see cref="Dictionary{TKey, TValue}" /> to <see cref="NullableConcurrentDictionary{TKey, TValue}" />.
    /// </remarks>
    /// <param name="dic">要转换的字典，不能为 null / The dictionary to convert, cannot be null.</param>
    /// <returns>转换后的 <see cref="NullableConcurrentDictionary{TKey, TValue}"/> 实例 / A converted <see cref="NullableConcurrentDictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
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
    /// <remarks>
    /// Implicitly converts from <see cref="ConcurrentDictionary{TKey, TValue}" /> to <see cref="NullableConcurrentDictionary{TKey, TValue}" />.
    /// </remarks>
    /// <param name="dic">要转换的并发字典，不能为 null / The concurrent dictionary to convert, cannot be null.</param>
    /// <returns>转换后的 <see cref="NullableConcurrentDictionary{TKey, TValue}"/> 实例 / A converted <see cref="NullableConcurrentDictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
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
    /// <remarks>
    /// Implicitly converts from <see cref="NullableConcurrentDictionary{TKey, TValue}" /> to <see cref="ConcurrentDictionary{TKey, TValue}" />.
    /// </remarks>
    /// <param name="dic">要转换的字典，不能为 null / The dictionary to convert, cannot be null.</param>
    /// <returns>转换后的 <see cref="ConcurrentDictionary{TKey, TValue}"/> 实例 / A converted <see cref="ConcurrentDictionary{TKey, TValue}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
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