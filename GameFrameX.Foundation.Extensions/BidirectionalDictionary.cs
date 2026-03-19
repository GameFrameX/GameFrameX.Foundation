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

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 双向字典，实现键和值的双向映射。支持在两个方向上进行快速查找。
/// 注意：键和值都必须是唯一的，不允许重复。
/// </summary>
/// <remarks>
/// A bidirectional dictionary that implements bidirectional mapping between keys and values.
/// Supports fast lookup in both directions.
/// Note: Both keys and values must be unique, duplicates are not allowed.
/// </remarks>
/// <typeparam name="TKey">键的类型，必须是可以作为字典键的类型 / The type of keys, must be a valid dictionary key type.</typeparam>
/// <typeparam name="TValue">值的类型，必须是可以作为字典键的类型 / The type of values, must be a valid dictionary key type.</typeparam>
public class BidirectionalDictionary<TKey, TValue>
{
 /// <summary>
    /// 存储键到值的映射关系。
    /// </summary>
    private readonly Dictionary<TKey, TValue> _forwardDictionary;

 /// <summary>
    /// 存储值到键的映射关系。
    /// </summary>
    private readonly Dictionary<TValue, TKey> _reverseDictionary;

    /// <summary>
    /// 初始化 <see cref="BidirectionalDictionary{TKey, TValue}" /> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="BidirectionalDictionary{TKey, TValue}" /> class.
    /// Setting a reasonable initial capacity can avoid frequent internal expansion and improve performance.
    /// </remarks>
    /// <param name="initialCapacity">初始容量，用于预分配内部字典的空间，默认值为 8，必须大于等于 0 / The initial capacity used to pre-allocate space for the internal dictionaries, defaults to 8, must be greater than or equal to 0.</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="initialCapacity"/> 小于 0 时抛出 / Thrown when <paramref name="initialCapacity"/> is less than 0.</exception>
    public BidirectionalDictionary(int initialCapacity = 8)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(initialCapacity, nameof(initialCapacity));
        _forwardDictionary = new Dictionary<TKey, TValue>(initialCapacity);
        _reverseDictionary = new Dictionary<TValue, TKey>(initialCapacity);
    }

    /// <summary>
    /// 尝试根据值获取对应的键。
    /// </summary>
    /// <remarks>
    /// Tries to get the key associated with the specified value.
    /// This operation has a time complexity of O(1).
    /// </remarks>
    /// <param name="value">要查找的值，不能为 null（如果 TValue 是引用类型） / The value to find, cannot be null if TValue is a reference type.</param>
    /// <param name="key">查找到的键。如果未找到对应的键，将设置为默认值 / The found key, will be set to the default value if no corresponding key is found.</param>
    /// <returns>如果成功找到键，则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the key is found; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 null 且 TValue 是引用类型时抛出 / Thrown when <paramref name="value"/> is null and TValue is a reference type.</exception>
    public bool TryGetKey(TValue value, out TKey key)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return _reverseDictionary.TryGetValue(value, out key);
    }

    /// <summary>
    /// 尝试根据键获取对应的值。
    /// </summary>
    /// <remarks>
    /// Tries to get the value associated with the specified key.
    /// This operation has a time complexity of O(1).
    /// </remarks>
    /// <param name="key">要查找的键，不能为 null（如果 TKey 是引用类型） / The key to find, cannot be null if TKey is a reference type.</param>
    /// <param name="value">查找到的值。如果未找到对应的值，将设置为默认值 / The found value, will be set to the default value if no corresponding value is found.</param>
    /// <returns>如果成功找到值，则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the value is found; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 null 且 TKey 是引用类型时抛出 / Thrown when <paramref name="key"/> is null and TKey is a reference type.</exception>
    public bool TryGetValue(TKey key, out TValue value)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return _forwardDictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// 清空双向字典中的所有键值对。
    /// </summary>
    /// <remarks>
    /// Clears all key-value pairs from the bidirectional dictionary.
    /// This operation clears both forward and reverse mappings.
    /// After clearing, the capacity of the dictionary remains the same, but all elements are removed.
    /// </remarks>
    public void Clear()
    {
        _forwardDictionary.Clear();
        _reverseDictionary.Clear();
    }

    /// <summary>
    /// 尝试向双向字典中添加键值对。
    /// </summary>
    /// <remarks>
    /// Tries to add a key-value pair to the bidirectional dictionary.
    /// If the key or value already exists, the addition fails and returns false.
    /// When successfully added, both forward and reverse mappings are updated.
    /// This operation has a time complexity of O(1).
    /// </remarks>
    /// <param name="key">要添加的键，不能为 null（如果 TKey 是引用类型） / The key to add, cannot be null if TKey is a reference type.</param>
    /// <param name="value">要添加的值，不能为 null（如果 TValue 是引用类型） / The value to add, cannot be null if TValue is a reference type.</param>
    /// <returns>如果成功添加键值对，则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the key-value pair was added; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 或 <paramref name="value"/> 为 null 且对应类型是引用类型时抛出 / Thrown when <paramref name="key"/> or <paramref name="value"/> is null and the corresponding type is a reference type.</exception>
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
    /// 尝试根据键移除键值对。
    /// </summary>
    /// <remarks>
    /// Tries to remove a key-value pair by key.
    /// </remarks>
    /// <param name="key">要移除的键，不能为 null（如果 TKey 是引用类型） / The key to remove, cannot be null if TKey is a reference type.</param>
    /// <param name="value">查找到的值。如果未找到对应的值，将设置为默认值 / The found value, will be set to the default value if no corresponding value is found.</param>
    /// <returns>如果成功找到值并移除键值对，则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the value was found and the key-value pair was removed; otherwise, <c>false</c>.</returns>
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
    /// 尝试根据值移除键值对。
    /// </summary>
    /// <remarks>
    /// Tries to remove a key-value pair by value.
    /// </remarks>
    /// <param name="value">要移除的值，不能为 null（如果 TValue 是引用类型） / The value to remove, cannot be null if TValue is a reference type.</param>
    /// <param name="key">查找到的键。如果未找到对应的值，将设置为默认值 / The found key, will be set to the default value if no corresponding value is found.</param>
    /// <returns>如果成功找到值并移除键值对，则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the value was found and the key-value pair was removed; otherwise, <c>false</c>.</returns>
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