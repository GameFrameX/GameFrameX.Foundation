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
/// 表示键和元素之间的多对多关系的集合。
/// </summary>
/// <remarks>
/// A collection that represents a many-to-many relationship between keys and elements.
/// Provides an efficient way to store and retrieve multiple elements associated with a specific key.
/// </remarks>
/// <typeparam name="TKey">键的泛型类型 / The generic type of keys.</typeparam>
/// <typeparam name="TElement">元素的泛型类型 / The generic type of elements.</typeparam>
public class LookupX<TKey, TElement> : IEnumerable<List<TElement>>
{
    private readonly IDictionary<TKey, List<TElement>> _dictionary;

    /// <summary>
    /// 使用指定的字典初始化一个新的 <see cref="LookupX&lt;TKey, TElement&gt;" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LookupX&lt;TKey, TElement&gt;" /> class with the specified dictionary.
    /// </remarks>
    /// <param name="dic">用于存储键和元素列表的字典，不能为 null / The dictionary used to store keys and element lists, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
    public LookupX(IDictionary<TKey, List<TElement>> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        _dictionary = dic;
    }

    /// <summary>
    /// 使用指定的并发字典初始化一个新的 <see cref="LookupX&lt;TKey, TElement&gt;" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LookupX&lt;TKey, TElement&gt;" /> class with the specified concurrent dictionary.
    /// </remarks>
    /// <param name="dic">用于存储键和元素列表的并发字典，不能为 null / The concurrent dictionary used to store keys and element lists, cannot be null.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
    public LookupX(ConcurrentDictionary<TKey, List<TElement>> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        _dictionary = dic;
    }

    /// <summary>
    /// 获取集合中的键值对数量。
    /// </summary>
    /// <remarks>
    /// Gets the number of key-value pairs in the collection.
    /// </remarks>
    /// <value>表示集合中包含的键值对的总数 / The total number of key-value pairs contained in the collection.</value>
    public int Count
    {
        get { return _dictionary.Count; }
    }

    /// <summary>
    /// 获取与指定键关联的元素列表。
    /// 如果键不存在，则返回一个空的元素列表。
    /// </summary>
    /// <remarks>
    /// Gets the list of elements associated with the specified key.
    /// If the key does not exist, returns an empty element list.
    /// </remarks>
    /// <param name="key">要查找的键，不能为 null / The key to look up, cannot be null.</param>
    /// <returns>与指定键关联的元素列表，如果键不存在，返回空列表 / The list of elements associated with the specified key; returns an empty list if the key does not exist.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 null 时抛出 / Thrown when <paramref name="key"/> is null.</exception>
    public List<TElement> this[TKey key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return _dictionary.TryGetValue(key, out var value) ? value : new List<TElement>();
        }
    }

    /// <summary>
    /// 返回一个枚举器，该枚举器可以遍历集合中的每个元素列表。
    /// </summary>
    /// <remarks>
    /// Returns an enumerator that can iterate through each element list in the collection.
    /// </remarks>
    /// <returns>一个 <see cref="IEnumerator{T}"/> 枚举器，用于遍历集合中的每个元素列表 / An <see cref="IEnumerator{T}"/> enumerator for iterating through each element list in the collection.</returns>
    public IEnumerator<List<TElement>> GetEnumerator()
    {
        return _dictionary.Values.GetEnumerator();
    }

    /// <summary>
    /// 返回一个非泛型枚举器，该枚举器可以遍历集合中的每个元素列表。
    /// </summary>
    /// <remarks>
    /// Returns a non-generic enumerator that can iterate through each element list in the collection.
    /// </remarks>
    /// <returns>一个 <see cref="IEnumerator"/> 枚举器，用于遍历集合中的每个元素列表 / An <see cref="IEnumerator"/> enumerator for iterating through each element list in the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// 判断集合中是否包含指定的键。
    /// </summary>
    /// <remarks>
    /// Determines whether the collection contains the specified key.
    /// </remarks>
    /// <param name="key">要检查的键，不能为 null / The key to check, cannot be null.</param>
    /// <returns>如果集合中包含指定的键，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the collection contains the specified key; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="key"/> 为 null 时抛出 / Thrown when <paramref name="key"/> is null.</exception>
    public bool Contains(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return _dictionary.ContainsKey(key);
    }
}