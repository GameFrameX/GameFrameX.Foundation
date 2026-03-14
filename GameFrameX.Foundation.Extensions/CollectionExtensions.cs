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

using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供集合类型的扩展方法
/// </summary>
public static class CollectionExtensions
{
    #region ICollectionExtensions

    /// <summary>
    /// 检查集合是否为 null 或空。
    /// </summary>
    /// <typeparam name="T">集合元素的类型。</typeparam>
    /// <param name="self">要检查的集合。</param>
    /// <returns>如果集合为 null 或空，则为 true；否则为 false。</returns>
    public static bool IsNullOrEmpty<T>(this ICollection<T> self)
    {
        return self is not { Count: > 0 };
    }

    #endregion

    /// <summary>
    /// 将一个可枚举集合的元素添加到哈希集合中。
    /// </summary>
    /// <typeparam name="T">哈希集合元素的类型。</typeparam>
    /// <param name="hashSet">要添加元素的哈希集合，不能为 null。</param>
    /// <param name="enumerable">要添加的元素的可枚举集合，不能为 null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="hashSet"/> 或 <paramref name="enumerable"/> 为 null 时抛出</exception>
    /// <remarks>此方法会将指定集合中的所有元素添加到目标哈希集合中。如果元素已存在，则会被忽略。</remarks>
    public static void AddRangeValues<T>(this HashSet<T> hashSet, IEnumerable<T> enumerable)
    {
        ArgumentNullException.ThrowIfNull(hashSet, nameof(hashSet));
        ArgumentNullException.ThrowIfNull(enumerable, nameof(enumerable));

        foreach (var item in enumerable)
        {
            hashSet.Add(item);
        }
    }

    #region DictionaryExtensions

    /// <summary>
    /// 合并字典中的键值对。如果字典中已存在指定的键，则使用指定的函数对原有值和新值进行合并；否则直接添加键值对。
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="self">要合并的字典，不能为 null。</param>
    /// <param name="key">要添加或合并的键，不能为 null（如果 TKey 是引用类型）。</param>
    /// <param name="v">要添加或合并的值。</param>
    /// <param name="func">用于合并值的函数，接收两个参数：现有值和新值，返回合并后的结果，不能为 null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/>（如果是引用类型）、<paramref name="v"/>（如果是引用类型）或 <paramref name="func"/> 为 null 时抛出</exception>
    /// <remarks>
    /// 当键已存在时，将调用提供的合并函数来组合现有值和新值。
    /// 当键不存在时，直接将新值添加到字典中。
    /// </remarks>
    public static void Merge<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue v, Func<TValue, TValue, TValue> func)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        if (v == null && !typeof(TValue).IsValueType)
        {
            throw new ArgumentNullException(nameof(v));
        }

        ArgumentNullException.ThrowIfNull(func, nameof(func));

        self[key] = self.TryGetValue(key, out var value) ? func(value, v) : v;
    }

    /// <summary>
    /// 获取指定键的值，如果字典中不存在该键，则使用指定的函数获取值并添加到字典中。
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="self">要操作的字典，不能为 null。</param>
    /// <param name="key">要获取值的键，不能为 null（如果 TKey 是引用类型）。</param>
    /// <param name="valueGetter">用于获取值的函数，接收键作为参数并返回对应的值，不能为 null。</param>
    /// <returns>指定键的值。如果键不存在，则返回新创建的值。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/>（如果是引用类型）或 <paramref name="valueGetter"/> 为 null 时抛出</exception>
    public static TValue GetOrAddValue<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TKey, TValue> valueGetter)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(valueGetter, nameof(valueGetter));

        if (!self.TryGetValue(key, out var value))
        {
            value = valueGetter(key);
            self[key] = value;
        }

        return value;
    }

    /// <summary>
    /// 获取指定键的值，如果字典中不存在该键，则使用无参构造函数创建一个新的值并添加到字典中。
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型，必须包含无参构造函数。</typeparam>
    /// <param name="self">要操作的字典，不能为 null。</param>
    /// <param name="key">要获取值的键，不能为 null（如果 TKey 是引用类型）。</param>
    /// <returns>指定键的值。如果键不存在，则返回新创建的值。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="key"/> 为 null 时抛出</exception>
    public static TValue GetOrAddValue<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key) where TValue : new()
    {
        return GetOrAddValue(self, key, _ => new TValue());
    }

    /// <summary>
    /// 根据指定条件从字典中移除键值对。
    /// </summary>
    /// <typeparam name="TKey">键的类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="self">要操作的字典，不能为 null。</param>
    /// <param name="predict">判断是否移除键值对的条件，接收键和值作为参数，返回true表示需要移除，不能为 null。</param>
    /// <returns>移除的键值对数量。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="predict"/> 为 null 时抛出</exception>
    /// <remarks>
    /// 此方法会遍历字典中的所有键值对，对每一对调用predict函数进行判断。
    /// 如果predict返回true，则移除该键值对。
    /// 为避免在遍历时修改集合导致的异常，此方法会先收集要移除的键，然后统一移除。
    /// </remarks>
    public static int RemoveIf<TKey, TValue>(this Dictionary<TKey, TValue> self, Func<TKey, TValue, bool> predict)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(predict, nameof(predict));

        var keysToRemove = new List<TKey>();
        foreach (var kv in self)
        {
            if (predict(kv.Key, kv.Value))
            {
                keysToRemove.Add(kv.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            self.Remove(key);
        }

        return keysToRemove.Count;
    }

    #endregion

    #region List<T>

    /// <summary>
    /// 从列表中随机获取一个元素。
    /// </summary>
    /// <typeparam name="T">列表元素的类型。</typeparam>
    /// <param name="list">要随机的列表，不能为 null 且不能为空。</param>
    /// <returns>随机选择的列表元素。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 <paramref name="list"/> 为空时抛出</exception>
    /// <remarks>使用System.Random.Shared来生成随机数，确保线程安全。</remarks>
    public static T RandomElement<T>(this List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        if (list.Count == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.ListCannotBeEmpty), nameof(list));
        }

        var n = list.Count;
        var index = System.Random.Shared.Next(n);
        return list[index];
    }

    /// <summary>
    /// 从列表中随机选择一个元素。
    /// </summary>
    /// <typeparam name="T">列表元素的类型。</typeparam>
    /// <param name="list">要从中选择元素的列表，不能为 null 且不能为空。</param>
    /// <param name="random">用于生成随机数的 Random 实例，不能为 null。</param>
    /// <returns>从列表中随机选择的元素。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 或 <paramref name="random"/> 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 <paramref name="list"/> 为空时抛出</exception>
    public static T RandomElement<T>(this List<T> list, Random random)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(random, nameof(random));

        if (list.Count == 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.ListCannotBeEmpty), nameof(list));
        }

        return list[random.Next(list.Count)];
    }

    /// <summary>
    /// 打乱列表中的元素顺序（洗牌）。
    /// </summary>
    /// <typeparam name="T">列表元素的类型。</typeparam>
    /// <param name="list">要打乱顺序的列表，不能为 null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为 null 时抛出</exception>
    /// <remarks>
    /// 使用Fisher-Yates洗牌算法实现列表随机排序。
    /// 该方法会直接修改原列表的顺序。
    /// </remarks>
    public static void Shuffle<T>(this List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));

        var n = list.Count;
        for (var i = 0; i < n; i++)
        {
            var rand = System.Random.Shared.Next(i, n);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    /// <summary>
    /// 从列表中移除满足条件的所有元素。
    /// </summary>
    /// <typeparam name="T">列表元素的类型。</typeparam>
    /// <param name="list">要操作的列表，不能为 null。</param>
    /// <param name="condition">用于判断元素是否满足移除条件的委托，返回true表示需要移除，不能为 null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 或 <paramref name="condition"/> 为 null 时抛出</exception>
    /// <remarks>
    /// 此方法使用 List{T}.RemoveAll 方法一次性移除所有满足条件的元素。
    /// </remarks>
    public static void RemoveIf<T>(this List<T> list, Predicate<T> condition)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        list.RemoveAll(condition);
    }

    #endregion
}