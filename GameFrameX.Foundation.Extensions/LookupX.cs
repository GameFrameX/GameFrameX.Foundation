// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections;
using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 表示键和元素之间的多对多关系的集合。
/// </summary>
/// <typeparam name="TKey">键的类型。</typeparam>
/// <typeparam name="TElement">元素的类型。</typeparam>
public class LookupX<TKey, TElement> : IEnumerable<List<TElement>>
{
    private readonly IDictionary<TKey, List<TElement>> _dictionary;

    /// <summary>
    /// 使用指定的字典初始化一个新的 <see cref="LookupX{TKey, TElement}" /> 实例。
    /// </summary>
    /// <param name="dic">用于存储键和元素列表的字典。</param>
    /// <exception cref="ArgumentNullException">The dictionary parameter is null.</exception>
    public LookupX(IDictionary<TKey, List<TElement>> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        _dictionary = dic;
    }

    /// <summary>
    /// 使用指定的并发字典初始化一个新的 <see cref="LookupX{TKey, TElement}" /> 实例。
    /// </summary>
    /// <param name="dic">用于存储键和元素列表的并发字典。</param>
    /// <exception cref="ArgumentNullException">The dictionary parameter is null.</exception>
    public LookupX(ConcurrentDictionary<TKey, List<TElement>> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        _dictionary = dic;
    }

    /// <summary>
    /// 获取集合中的键值对数量。
    /// </summary>
    public int Count
    {
        get { return _dictionary.Count; }
    }

    /// <summary>
    /// 获取与指定键关联的元素列表。
    /// 如果键不存在，则返回一个空的元素列表。
    /// </summary>
    /// <param name="key">要查找的键。</param>
    /// <returns>与指定键关联的元素列表。</returns>
    /// <exception cref="ArgumentNullException">The key parameter is null.</exception>
    public List<TElement> this[TKey key]
    {
        get
        {
            ArgumentNullException.ThrowIfNull(key, nameof(key));
            return _dictionary.TryGetValue(key, out var value) ? value : [];
        }
    }

    /// <summary>
    /// 返回一个枚举器，该枚举器可以遍历集合中的每个元素列表。
    /// </summary>
    /// <returns>一个枚举器，该枚举器可以遍历集合中的每个元素列表。</returns>
    public IEnumerator<List<TElement>> GetEnumerator()
    {
        return _dictionary.Values.GetEnumerator();
    }

    /// <summary>
    /// 返回一个枚举器，该枚举器可以遍历集合中的每个元素列表。
    /// </summary>
    /// <returns>一个枚举器，该枚举器可以遍历集合中的每个元素列表。</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// 判断集合中是否包含指定的键。
    /// </summary>
    /// <param name="key">要检查的键。</param>
    /// <returns>如果集合中包含指定的键，则返回 true；否则返回 false。</returns>
    /// <exception cref="ArgumentNullException">The key parameter is null.</exception>
    public bool Contains(TKey key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        return _dictionary.ContainsKey(key);
    }
}