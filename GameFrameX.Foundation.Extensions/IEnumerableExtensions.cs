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
using System.Diagnostics;
using System.Linq.Expressions;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供IEnumerable类型的扩展方法集合。
/// </summary>
/// <remarks>
/// Provides a collection of extension methods for IEnumerable types.
/// </remarks>
public static class IEnumerableExtensions
{
    /// <summary>
    /// 按字段属性判等取交集。此方法允许使用自定义条件比较两个不同类型集合的元素。
    /// </summary>
    /// <remarks>
    /// Gets the intersection by field property comparison. This method allows comparing elements of two different types using a custom condition.
    /// </remarks>
    /// <typeparam name="TFirst">第一个集合的元素类型 / The element type of the first collection.</typeparam>
    /// <typeparam name="TSecond">第二个集合的元素类型 / The element type of the second collection.</typeparam>
    /// <param name="first">第一个集合 / The first collection.</param>
    /// <param name="second">第二个集合 / The second collection.</param>
    /// <param name="condition">用于判断两个元素是否相等的条件，返回true表示元素相等 / The condition to determine if two elements are equal, returns true if elements are equal.</param>
    /// <returns>返回两个集合中满足条件的交集元素 / Returns the intersection elements that satisfy the condition.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/>、<paramref name="second"/> 或 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="first"/>, <paramref name="second"/>, or <paramref name="condition"/> is null.</exception>
    public static IEnumerable<TFirst> IntersectByComparer<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        return first.Where(f => second.Any(s => condition(f, s)));
    }

    /// <summary>
    /// 按字段属性判等取交集。此方法使用默认的相等比较器。
    /// </summary>
    /// <remarks>
    /// Gets the intersection by field property comparison. This method uses the default equality comparer.
    /// </remarks>
    /// <typeparam name="TSource">集合的元素类型 / The element type of the collection.</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型 / The type of the key returned by the key selector.</typeparam>
    /// <param name="first">第一个集合 / The first collection.</param>
    /// <param name="second">第二个集合 / The second collection.</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数 / The function to extract a key from each element.</param>
    /// <returns>返回两个集合中具有相同键的交集元素 / Returns the intersection elements with the same key.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/>、<paramref name="second"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="first"/>, <paramref name="second"/>, or <paramref name="keySelector"/> is null.</exception>
    public static IEnumerable<TSource> IntersectByComparer<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        return first.IntersectByComparer(second, keySelector, null);
    }

    /// <summary>
    /// 按字段属性判等取交集。此方法允许使用自定义的相等比较器。
    /// </summary>
    /// <remarks>
    /// Gets the intersection by field property comparison. This method allows using a custom equality comparer.
    /// </remarks>
    /// <typeparam name="TSource">集合的元素类型 / The element type of the collection.</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型 / The type of the key returned by the key selector.</typeparam>
    /// <param name="first">第一个集合 / The first collection.</param>
    /// <param name="second">第二个集合 / The second collection.</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数 / The function to extract a key from each element.</param>
    /// <param name="comparer">用于比较键的比较器，如果为 null 则使用默认比较器 / The comparer to compare keys, uses default comparer if null.</param>
    /// <returns>返回两个集合中具有相同键的交集元素 / Returns the intersection elements with the same key.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/>、<paramref name="second"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="first"/>, <paramref name="second"/>, or <paramref name="keySelector"/> is null.</exception>
    public static IEnumerable<TSource> IntersectByComparer<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        return IntersectByIterator(first, second, keySelector, comparer);
    }

    /// <summary>
    /// 实现按键选择器取交集的迭代器方法
    /// </summary>
    private static IEnumerable<TSource> IntersectByIterator<TSource, TKey>(IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        var set = new HashSet<TKey>(second.Select(keySelector), comparer);
        foreach (var item in first.Where(source => set.Remove(keySelector(source))))
        {
            yield return item;
        }
    }

    /// <summary>
    /// 多个集合取交集元素。使用默认的相等比较器。
    /// </summary>
    /// <remarks>
    /// Gets the intersection of multiple collections using the default equality comparer.
    /// </remarks>
    /// <typeparam name="T">集合的元素类型 / The element type of the collection.</typeparam>
    /// <param name="source">多个集合的序列 / A sequence of multiple collections.</param>
    /// <returns>返回所有集合的交集元素 / Returns the intersection elements of all collections.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null.</exception>
    public static IEnumerable<T> IntersectAllComparer<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.Aggregate((current, item) => current.Intersect(item));
    }

    /// <summary>
    /// 多个集合按指定键选择器取交集元素。使用默认的相等比较器。
    /// </summary>
    /// <remarks>
    /// Gets the intersection of multiple collections by key selector using the default equality comparer.
    /// </remarks>
    /// <typeparam name="TSource">集合的元素类型 / The element type of the collection.</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型 / The type of the key returned by the key selector.</typeparam>
    /// <param name="source">多个集合的序列 / A sequence of multiple collections.</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数 / A function to extract a key from each element.</param>
    /// <returns>返回所有集合的交集元素 / Returns the intersection elements of all collections.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
    public static IEnumerable<TSource> IntersectAllComparer<TSource, TKey>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        return source.Aggregate((current, item) => current.IntersectByComparer(item, keySelector));
    }

    /// <summary>
    /// 多个集合按指定键选择器取交集元素。允许使用自定义的相等比较器。
    /// </summary>
    /// <remarks>
    /// Gets the intersection of multiple collections by key selector using a custom equality comparer.
    /// </remarks>
    /// <typeparam name="TSource">集合的元素类型 / The element type of the collection.</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型 / The type of the key returned by the key selector.</typeparam>
    /// <param name="source">多个集合的序列 / A sequence of multiple collections.</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数 / A function to extract a key from each element.</param>
    /// <param name="comparer">用于比较键的比较器 / The comparer to compare keys.</param>
    /// <returns>返回所有集合的交集元素 / Returns the intersection elements of all collections.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
    public static IEnumerable<TSource> IntersectAllComparer<TSource, TKey>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        return source.Aggregate((current, item) => current.IntersectByComparer(item, keySelector, comparer));
    }

    /// <summary>
    /// 多个集合取交集元素。允许使用自定义的相等比较器。
    /// </summary>
    /// <remarks>
    /// Gets the intersection of multiple collections using a custom equality comparer.
    /// </remarks>
    /// <typeparam name="T">集合的元素类型 / The element type of the collection.</typeparam>
    /// <param name="source">多个集合的序列 / A sequence of multiple collections.</param>
    /// <param name="comparer">用于比较元素的比较器 / The comparer to compare elements.</param>
    /// <returns>返回所有集合的交集元素 / Returns the intersection elements of all collections.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null.</exception>
    public static IEnumerable<T> IntersectAllComparer<T>(this IEnumerable<IEnumerable<T>> source, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.Aggregate((current, item) => current.Intersect(item, comparer));
    }

    /// <summary>
    /// 按字段属性判等取差集。此方法允许使用自定义条件比较两个不同类型集合的元素。
    /// </summary>
    /// <remarks>
    /// Gets the difference set by field property comparison. This method allows comparing elements of two different types using a custom condition.
    /// </remarks>
    /// <typeparam name="TFirst">第一个集合的元素类型 / The element type of the first collection.</typeparam>
    /// <typeparam name="TSecond">第二个集合的元素类型 / The element type of the second collection.</typeparam>
    /// <param name="first">第一个集合 / The first collection.</param>
    /// <param name="second">第二个集合 / The second collection.</param>
    /// <param name="condition">用于判断两个元素是否相等的条件，返回true表示元素相等 / The condition to determine if two elements are equal, returns true if elements are equal.</param>
    /// <returns>返回第一个集合中不在第二个集合中的元素 / Returns elements in the first collection that are not in the second collection.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/>、<paramref name="second"/> 或 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="first"/>, <paramref name="second"/>, or <paramref name="condition"/> is null.</exception>
    public static IEnumerable<TFirst> ExceptByExpression<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        return first.Where(f => !second.Any(s => condition(f, s)));
    }


    /// <summary>
    /// 向集合中添加多个元素。此方法允许通过params参数传入任意数量的元素。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a collection. This method allows passing any number of elements via params parameter.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection.</typeparam>
    /// <param name="self">要添加元素的集合 / The collection to add elements to.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="values"/> is null.</exception>
    public static void AddRangeValues<T>(this ICollection<T> self, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values)
        {
            self.Add(obj);
        }
    }

    /// <summary>
    /// 向集合中添加多个元素。此方法接受任何可枚举的集合作为输入。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a collection. This method accepts any enumerable collection as input.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection.</typeparam>
    /// <param name="self">要添加元素的集合 / The collection to add elements to.</param>
    /// <param name="values">包含要添加元素的可枚举集合 / The enumerable collection containing elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="values"/> is null.</exception>
    public static void AddRangeValues<T>(this ICollection<T> self, IEnumerable<T> values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values)
        {
            self.Add(obj);
        }
    }

    /// <summary>
    /// 向并发袋中添加多个元素。此方法是线程安全的，适用于多线程环境。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a concurrent bag. This method is thread-safe and suitable for multi-threaded environments.
    /// </remarks>
    /// <typeparam name="T">并发袋中的元素类型 / The element type of the concurrent bag.</typeparam>
    /// <param name="self">要添加元素的并发袋 / The concurrent bag to add elements to.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="values"/> is null.</exception>
    public static void AddRangeValues<T>(this ConcurrentBag<T> self, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values)
        {
            self.Add(obj);
        }
    }

    /// <summary>
    /// 向并发队列中添加多个元素。此方法是线程安全的，适用于多线程环境。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a concurrent queue. This method is thread-safe and suitable for multi-threaded environments.
    /// </remarks>
    /// <typeparam name="T">并发队列中的元素类型 / The element type of the concurrent queue.</typeparam>
    /// <param name="self">要添加元素的并发队列 / The concurrent queue to add elements to.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="values"/> is null.</exception>
    public static void AddRangeValues<T>(this ConcurrentQueue<T> self, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values)
        {
            self.Enqueue(obj);
        }
    }

    /// <summary>
    /// 向集合中添加符合条件的多个元素。此方法允许通过谓词函数筛选要添加的元素。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a collection that satisfy a condition. This method allows filtering elements to add using a predicate function.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection.</typeparam>
    /// <param name="self">要添加元素的集合 / The collection to add elements to.</param>
    /// <param name="predicate">用于筛选元素的条件函数 / The predicate function to filter elements. Returns true if the element should be added.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="predicate"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="predicate"/>, or <paramref name="values"/> is null.</exception>
    public static void AddRangeIf<T>(this ICollection<T> self, Func<T, bool> predicate, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values.Where(predicate))
        {
            self.Add(obj);
        }
    }

    /// <summary>
    /// 向并发袋中添加符合条件的多个元素。此方法是线程安全的，适用于多线程环境。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a concurrent bag that satisfy a condition. This method is thread-safe and suitable for multi-threaded environments.
    /// </remarks>
    /// <typeparam name="T">并发袋中的元素类型 / The element type of the concurrent bag.</typeparam>
    /// <param name="self">要添加元素的并发袋 / The concurrent bag to add elements to.</param>
    /// <param name="predicate">用于筛选元素的条件函数 / The predicate function to filter elements.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="predicate"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="predicate"/>, or <paramref name="values"/> is null.</exception>
    public static void AddRangeIf<T>(this ConcurrentBag<T> self, Func<T, bool> predicate, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values.Where(predicate))
        {
            self.Add(obj);
        }
    }

    /// <summary>
    /// 向并发队列中添加符合条件的多个元素。此方法是线程安全的，适用于多线程环境。
    /// </summary>
    /// <remarks>
    /// Adds multiple elements to a concurrent queue that satisfy a condition. This method is thread-safe and suitable for multi-threaded environments.
    /// </remarks>
    /// <typeparam name="T">并发队列中的元素类型 / The element type of the concurrent queue.</typeparam>
    /// <param name="self">要添加元素的并发队列 / The concurrent queue to add elements to.</param>
    /// <param name="predicate">用于筛选元素的条件函数 / The predicate function to filter elements.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="predicate"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="predicate"/>, or <paramref name="values"/> is null.</exception>
    public static void AddRangeIf<T>(this ConcurrentQueue<T> self, Func<T, bool> predicate, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(predicate, nameof(predicate));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values.Where(predicate))
        {
            self.Enqueue(obj);
        }
    }

    /// <summary>
    /// 向集合中添加不重复的元素。此方法会检查集合中是否已存在相同的元素，只添加不存在的元素。
    /// </summary>
    /// <remarks>
    /// Adds non-duplicate elements to a collection. This method checks if the same element already exists in the collection and only adds elements that don't exist.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection.</typeparam>
    /// <param name="self">要添加元素的集合 / The collection to add elements to.</param>
    /// <param name="values">要添加的元素数组 / The array of elements to add.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="values"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="values"/> is null.</exception>
    public static void AddRangeIfNotContains<T>(this ICollection<T> self, params T[] values)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (var obj in values)
        {
            if (!self.Contains(obj))
            {
                self.Add(obj);
            }
        }
    }

    /// <summary>
    /// 从集合中移除符合条件的元素。此方法会创建一个符合条件的元素列表，然后逐个移除这些元素。
    /// </summary>
    /// <remarks>
    /// Removes elements from a collection that satisfy a condition. This method creates a list of elements that satisfy the condition, then removes them one by one.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection.</typeparam>
    /// <param name="self">要移除元素的集合 / The collection to remove elements from.</param>
    /// <param name="where">用于筛选要移除的元素的条件函数 / The predicate function to filter elements to remove. Returns true if the element should be removed.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="where"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="where"/> is null.</exception>
    public static void RemoveWhere<T>(this ICollection<T> self, Func<T, bool> where)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(where, nameof(where));

        foreach (var obj in self.Where(where).ToList())
        {
            self.Remove(obj);
        }
    }

    /// <summary>
    /// 在满足条件的元素之后添加元素。此方法会在所有满足条件的元素后面插入指定的值。
    /// </summary>
    /// <remarks>
    /// Inserts an element after elements that satisfy the condition. This method inserts the specified value after all elements that meet the condition.
    /// If the matching element is the last element in the collection, the new value will be added to the end of the collection.
    /// The insertion is performed from back to front to ensure correct insertion positions.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection</typeparam>
    /// <param name="self">当前集合 / The current collection</param>
    /// <param name="condition">用于判断元素的条件函数，返回true表示在该元素后插入新值 / The condition function to determine elements, returns true to insert after the element</param>
    /// <param name="value">要插入的值 / The value to insert</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="condition"/> is null</exception>
    public static void InsertAfter<T>(this IList<T> self, Func<T, bool> condition, T value)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        var list = self.Select((item, index) => new
        {
            item,
            index,
        }).Where(p => condition(p.item)).OrderByDescending(p => p.index).Select(t => t.index);
        foreach (var index in list)
        {
            if (index + 1 == self.Count)
            {
                self.Add(value);
            }
            else
            {
                self.Insert(index + 1, value);
            }
        }
    }

    /// <summary>
    /// 在指定索引位置之后添加元素。此方法会在指定索引的元素后面插入新值。
    /// </summary>
    /// <remarks>
    /// Inserts an element after the specified index position. This method inserts a new value after the element at the specified index.
    /// If the specified index position is the last position in the collection, the new value will be added to the end of the collection.
    /// If the specified index does not exist, no operation will be performed.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection</typeparam>
    /// <param name="list">当前集合 / The current collection</param>
    /// <param name="index">要在其后插入新值的索引位置 / The index position after which to insert the new value</param>
    /// <param name="value">要插入的值 / The value to insert</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为 null 时抛出 / Thrown when <paramref name="list"/> is null</exception>
    public static void InsertAfter<T>(this IList<T> list, int index, T value)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        var src = list.Select((v, i) => new
        {
            Value = v,
            Index = i,
        }).Where(p => p.Index == index).OrderByDescending(p => p.Index).Select(t => t.Index);
        foreach (var i in src)
        {
            if (i + 1 == list.Count)
            {
                list.Add(value);
            }
            else
            {
                list.Insert(i + 1, value);
            }
        }
    }

    /// <summary>
    /// 将集合转换为HashSet。此方法允许在转换过程中通过选择器函数转换元素类型。
    /// </summary>
    /// <remarks>
    /// Converts a collection to a HashSet. This method allows converting element types during the conversion process through a selector function.
    /// HashSet automatically removes duplicate elements, ensuring uniqueness of elements in the result collection.
    /// If the selector function returns the same value, only one will be retained.
    /// </remarks>
    /// <typeparam name="T">源集合中的元素类型 / The element type of the source collection</typeparam>
    /// <typeparam name="TResult">目标HashSet中的元素类型 / The element type of the target HashSet</typeparam>
    /// <param name="source">源集合 / The source collection</param>
    /// <param name="selector">用于转换元素类型的选择器函数 / The selector function to convert element types</param>
    /// <returns>包含转换后元素的HashSet集合 / A HashSet containing the converted elements</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static HashSet<TResult> ToHashSet<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return new HashSet<TResult>(source.Select(selector));
    }

    /// <summary>
    /// 遍历IEnumerable集合，并对每个元素执行指定的操作。此方法提供了一种简便的方式来遍历和处理集合元素。
    /// </summary>
    /// <remarks>
    /// Iterates through an IEnumerable collection and performs the specified action on each element. This method provides a convenient way to traverse and process collection elements.
    /// This method is a wrapper for the foreach loop, making code more concise.
    /// Modifying the collection during iteration may cause exceptions.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection</typeparam>
    /// <param name="self">当前集合 / The current collection</param>
    /// <param name="action">要对每个元素执行的操作 / The action to perform on each element</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="action"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="action"/> is null</exception>
    public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        foreach (var o in self)
        {
            action(o);
        }
    }

    /// <summary>
    /// 异步遍历IEnumerable集合，并对每个元素执行异步操作。此方法支持并行处理并可限制最大并行数。
    /// </summary>
    /// <remarks>
    /// Asynchronously iterates through an IEnumerable collection and performs an async operation on each element. This method supports parallel processing with a configurable maximum degree of parallelism.
    /// In debug mode, operations are executed sequentially instead of in parallel.
    /// When the maximum parallel count is reached, it waits for at least one task to complete before continuing with new tasks.
    /// Operations can be cancelled via the cancellationToken.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection</typeparam>
    /// <param name="source">当前集合 / The current collection</param>
    /// <param name="maxParallelCount">同时执行的最大任务数 / The maximum number of concurrent tasks</param>
    /// <param name="action">要对每个元素执行的异步操作 / The async operation to perform on each element</param>
    /// <param name="cancellationToken">用于取消操作的令牌 / The token to cancel the operation</param>
    /// <returns>表示异步操作的任务 / A task representing the async operation</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="action"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="action"/> is null</exception>
    public static async Task ForeachAsync<T>(this IEnumerable<T> source, Func<T, Task> action, int maxParallelCount, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        if (Debugger.IsAttached)
        {
            foreach (var item in source)
            {
                await action(item);
            }

            return;
        }

        var list = new List<Task>();
        foreach (var item in source)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            list.Add(action(item));
            if (list.Count(t => !t.IsCompleted) >= maxParallelCount)
            {
                await Task.WhenAny(list);
                list.RemoveAll(t => t.IsCompleted);
            }
        }

        await Task.WhenAll(list);
    }

    /// <summary>
    /// 异步遍历IEnumerable集合，并对每个元素执行异步操作。此方法会自动根据集合大小确定并行数。
    /// </summary>
    /// <remarks>
    /// Asynchronously iterates through an IEnumerable collection and performs an async operation on each element. This method automatically determines the parallelism based on the collection size.
    /// If source is an ICollection type, its Count will be used as the maximum parallel count.
    /// Otherwise, the collection will be converted to a List first, then its Count will be used as the maximum parallel count.
    /// </remarks>
    /// <typeparam name="T">集合中的元素类型 / The element type of the collection</typeparam>
    /// <param name="source">当前集合 / The current collection</param>
    /// <param name="action">要对每个元素执行的异步操作 / The async operation to perform on each element</param>
    /// <param name="cancellationToken">用于取消操作的令牌 / The token to cancel the operation</param>
    /// <returns>表示异步操作的任务 / A task representing the async operation</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="action"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="action"/> is null</exception>
    public static Task ForeachAsync<T>(this IEnumerable<T> source, Func<T, Task> action, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        if (source is ICollection<T> collection)
        {
            return ForeachAsync(collection, action, collection.Count, cancellationToken);
        }

        var list = source.ToList();
        return ForeachAsync(list, action, list.Count, cancellationToken);
    }

    /// <summary>
    /// 异步选择集合中的每个元素，并返回结果数组。此方法支持将每个元素异步转换为新的类型。
    /// </summary>
    /// <remarks>
    /// Asynchronously selects each element in the collection and returns a result array. This method supports asynchronously converting each element to a new type.
    /// All conversion operations are executed in parallel.
    /// The returned task completes when all conversion operations are finished.
    /// </remarks>
    /// <typeparam name="T">源集合中的元素类型 / The element type of the source collection</typeparam>
    /// <typeparam name="TResult">结果数组中的元素类型 / The element type of the result array</typeparam>
    /// <param name="source">源集合 / The source collection</param>
    /// <param name="selector">用于转换元素的异步选择器函数 / The async selector function to convert elements</param>
    /// <returns>包含转换后元素的数组的任务 / A task containing an array of converted elements</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static Task<TResult[]> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return Task.WhenAll(source.Select(selector));
    }

    /// <summary>
    /// 异步选择集合中的每个元素，并返回结果数组。此方法支持访问元素的索引位置。
    /// </summary>
    /// <remarks>
    /// Asynchronously selects each element in the collection and returns a result array. This method supports accessing the element's index position.
    /// All conversion operations are executed in parallel.
    /// The selector function can access the element's position in the collection through the index parameter.
    /// The returned task completes when all conversion operations are finished.
    /// </remarks>
    /// <typeparam name="T">源集合中的元素类型 / The element type of the source collection</typeparam>
    /// <typeparam name="TResult">结果数组中的元素类型 / The element type of the result array</typeparam>
    /// <param name="source">源集合 / The source collection</param>
    /// <param name="selector">用于转换元素的异步选择器函数，可访问元素索引 / The async selector function to convert elements, with access to element index</param>
    /// <returns>包含转换后元素的数组的任务 / A task containing an array of converted elements</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static Task<TResult[]> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, int, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return Task.WhenAll(source.Select(selector));
    }

    /// <summary>
    /// 异步选择集合中的每个元素，并返回结果列表。此方法支持限制最大并行处理数量。
    /// </summary>
    /// <remarks>
    /// Asynchronously selects each element in the collection and returns a result list. This method supports limiting the maximum parallel processing count.
    /// This method limits the number of parallel tasks; when the maximum parallel count is reached, it waits for some tasks to complete before continuing.
    /// Completed task results are immediately added to the result list while uncompleted tasks continue execution.
    /// </remarks>
    /// <typeparam name="T">源集合中的元素类型 / The element type of the source collection</typeparam>
    /// <typeparam name="TResult">结果列表中的元素类型 / The element type of the result list</typeparam>
    /// <param name="source">源集合 / The source collection</param>
    /// <param name="selector">用于转换每个元素的异步选择器函数 / The async selector function to convert each element</param>
    /// <param name="maxParallelCount">同时处理的最大任务数量 / The maximum number of concurrent tasks</param>
    /// <returns>包含转换后元素的结果列表 / A result list containing the converted elements</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static async Task<List<TResult>> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<TResult>> selector, int maxParallelCount)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        var results = new List<TResult>();
        var tasks = new List<Task<TResult>>();
        foreach (var item in source)
        {
            var task = selector(item);
            tasks.Add(task);
            if (tasks.Count >= maxParallelCount)
            {
                await Task.WhenAny(tasks);
                var completedTasks = tasks.Where(t => t.IsCompleted).ToArray();
                results.AddRange(completedTasks.Select(t => t.Result));
                tasks.RemoveWhere(t => completedTasks.Contains(t));
            }
        }

        results.AddRange(await Task.WhenAll(tasks));
        return results;
    }

    /// <summary>
    /// 异步选择集合中的每个元素，并返回结果列表。此方法支持限制最大并行处理数量，并提供元素索引。
    /// </summary>
    /// <remarks>
    /// Asynchronously selects each element in the collection and returns a result list. This method supports limiting the maximum parallel processing count and provides element index.
    /// This method limits the number of parallel tasks; when the maximum parallel count is reached, it waits for some tasks to complete before continuing.
    /// Completed task results are immediately added to the result list while uncompleted tasks continue execution.
    /// Uses Interlocked.Add to ensure accuracy of index counting in multi-threaded environments.
    /// </remarks>
    /// <typeparam name="T">源集合中的元素类型 / The element type of the source collection</typeparam>
    /// <typeparam name="TResult">结果列表中的元素类型 / The element type of the result list</typeparam>
    /// <param name="source">源集合 / The source collection</param>
    /// <param name="selector">用于转换每个元素的异步选择器函数，接收元素和其索引作为参数 / The async selector function to convert each element, receiving element and its index as parameters</param>
    /// <param name="maxParallelCount">同时处理的最大任务数量 / The maximum number of concurrent tasks</param>
    /// <returns>包含转换后元素的结果列表 / A result list containing the converted elements</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static async Task<List<TResult>> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, int, Task<TResult>> selector, int maxParallelCount)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        var results = new List<TResult>();
        var tasks = new List<Task<TResult>>();
        var index = 0;
        foreach (var item in source)
        {
            var task = selector(item, index);
            tasks.Add(task);
            Interlocked.Add(ref index, 1);
            if (tasks.Count >= maxParallelCount)
            {
                await Task.WhenAny(tasks);
                var completedTasks = tasks.Where(t => t.IsCompleted).ToArray();
                results.AddRange(completedTasks.Select(t => t.Result));
                tasks.RemoveWhere(t => completedTasks.Contains(t));
            }
        }

        results.AddRange(await Task.WhenAll(tasks));
        return results;
    }

    /// <summary>
    /// 异步遍历集合中的每个元素，并执行指定的操作。支持限制最大并行处理数量和取消操作。
    /// </summary>
    /// <remarks>
    /// Asynchronously iterates through each element in the collection and performs the specified operation. Supports limiting maximum parallel processing count and cancellation.
    /// In debug mode (when Debugger.IsAttached is true), operations are executed synchronously in sequence.
    /// In non-debug mode, operations are executed in parallel but limited to the maximum parallel count.
    /// Supports cancellation via cancellationToken.
    /// Uses Interlocked.Add to ensure accuracy of index counting in multi-threaded environments.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="source">要遍历的集合 / The collection to iterate</param>
    /// <param name="selector">要对每个元素执行的异步操作，接收元素和其索引作为参数 / The async operation to perform on each element, receiving element and its index as parameters</param>
    /// <param name="maxParallelCount">同时处理的最大任务数量 / The maximum number of concurrent tasks</param>
    /// <param name="cancellationToken">用于取消操作的取消令牌 / The cancellation token to cancel the operation</param>
    /// <returns>表示异步操作的任务 / A task representing the async operation</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static async Task ForAsync<T>(this IEnumerable<T> source, Func<T, int, Task> selector, int maxParallelCount, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        var index = 0;
        if (Debugger.IsAttached)
        {
            foreach (var item in source)
            {
                await selector(item, index);
                index++;
            }

            return;
        }

        var list = new List<Task>();
        foreach (var item in source)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            list.Add(selector(item, index));
            Interlocked.Add(ref index, 1);
            if (list.Count >= maxParallelCount)
            {
                await Task.WhenAny(list);
                list.RemoveAll(t => t.IsCompleted);
            }
        }

        await Task.WhenAll(list);
    }

    /// <summary>
    /// 异步遍历集合中的每个元素，并执行指定的操作。自动设置最大并行数为集合的大小。
    /// </summary>
    /// <remarks>
    /// Asynchronously iterates through each element in the collection and performs the specified operation. Automatically sets the maximum parallel count to the collection size.
    /// If source is an ICollection&lt;T&gt; type, its Count is used directly as the maximum parallel count.
    /// Otherwise, source is converted to a List and its Count is used as the maximum parallel count.
    /// This ensures all elements are processed while avoiding creating too many parallel tasks.
    ///
    /// Performance notes:
    /// 1. For ICollection&lt;T&gt; types, avoids extra ToList() conversion overhead
    /// 2. Parallelism automatically adapts to collection size, avoiding creating too many tasks
    /// 3. Supports cancellation via CancellationToken
    ///
    /// Usage example:
    /// await collection.ForAsync(async (item, index) => {
    ///     await ProcessItemAsync(item, index);
    /// });
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="source">要遍历的集合 / The collection to iterate</param>
    /// <param name="selector">要对每个元素执行的异步操作，接收元素和其索引作为参数 / The async operation to perform on each element, receiving element and its index as parameters</param>
    /// <param name="cancellationToken">用于取消操作的取消令牌 / The cancellation token to cancel the operation</param>
    /// <returns>表示异步操作的任务 / A task representing the async operation</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static Task ForAsync<T>(this IEnumerable<T> source, Func<T, int, Task> selector, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        if (source is ICollection<T> collection)
        {
            return ForAsync(collection, selector, collection.Count, cancellationToken);
        }

        var list = source.ToList();
        return ForAsync(list, selector, list.Count, cancellationToken);
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。此方法用于LINQ查询。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the default value if the collection is empty. This method is used for LINQ queries.
    /// This method uses DefaultIfEmpty to ensure a default value is returned instead of throwing an exception when the collection is empty.
    /// Suitable for LINQ query scenarios that require safe retrieval of maximum values.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型 / The type of the result</typeparam>
    /// <param name="source">要查询的集合 / The collection to query</param>
    /// <param name="selector">用于从每个元素中提取值的表达式 / The expression to extract values from each element</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TResult的默认值 / The maximum value in the collection, or the default value of TResult if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MaxOrDefaultValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。此方法支持LINQ查询表达式，并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the specified default value if the collection is empty. This method supports LINQ query expressions and allows extracting values to compare through a selector function.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型，必须实现IComparable接口 / The type of the result, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="selector">用于从每个元素中提取值的函数，不能为null / The function to extract values from each element, cannot be null</param>
    /// <param name="defaultValue">集合为空时返回的默认值，可以为null / The default value to return if the collection is empty, can be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值 / The maximum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MaxOrDefaultValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。此方法支持LINQ查询表达式，直接比较集合中的元素。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the default value if the collection is empty. This method supports LINQ query expressions and directly compares elements in the collection.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型，必须实现IComparable接口 / The element type of the collection, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TSource的默认值 / The maximum value in the collection, or the default value of TSource if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MaxOrDefaultValue<TSource>(this IQueryable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。此方法支持LINQ查询表达式，直接比较集合中的元素。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the specified default value if the collection is empty. This method supports LINQ query expressions and directly compares elements in the collection.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型，必须实现IComparable接口 / The element type of the collection, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="defaultValue">集合为空时返回的默认值，可以为null / The default value to return if the collection is empty, can be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值 / The maximum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MaxOrDefaultValue<TSource>(this IQueryable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。此方法用于普通集合，并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the specified default value if the collection is empty. This method is used for regular collections and allows extracting values to compare through a selector function.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型，必须实现IComparable接口 / The type of the result, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="selector">用于从每个元素中提取值的函数，不能为null / The function to extract values from each element, cannot be null</param>
    /// <param name="defaultValue">集合为空时返回的默认值，可以为null / The default value to return if the collection is empty, can be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值 / The maximum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MaxOrDefaultValue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。此方法用于普通集合，并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the default value if the collection is empty. This method is used for regular collections and allows extracting values to compare through a selector function.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型，必须实现IComparable接口 / The type of the result, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="selector">用于从每个元素中提取值的函数，不能为null / The function to extract values from each element, cannot be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TResult的默认值 / The maximum value in the collection, or the default value of TResult if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MaxOrDefaultValue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。此方法用于普通集合，直接比较集合中的元素。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the default value if the collection is empty. This method is used for regular collections and directly compares elements in the collection.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型，必须实现IComparable接口 / The element type of the collection, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TSource的默认值 / The maximum value in the collection, or the default value of TSource if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MaxOrDefaultValue<TSource>(this IEnumerable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。此方法用于普通集合，直接比较集合中的元素。
    /// </summary>
    /// <remarks>
    /// Gets the maximum value in the collection, or returns the specified default value if the collection is empty. This method is used for regular collections and directly compares elements in the collection.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型，必须实现IComparable接口 / The element type of the collection, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="defaultValue">集合为空时返回的默认值，可以为null / The default value to return if the collection is empty, can be null</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值 / The maximum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MaxOrDefaultValue<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回默认值。此方法支持LINQ查询表达式，并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the collection, or returns the default value if the collection is empty. This method supports LINQ query expressions and allows extracting values to compare through a selector function.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型，必须实现IComparable接口 / The type of the result, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="selector">用于从每个元素中提取值的函数，不能为null / The function to extract values from each element, cannot be null</param>
    /// <returns>集合中的最小值，如果集合为空则返回类型TResult的默认值 / The minimum value in the collection, or the default value of TResult if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MinOrDefaultValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回指定的默认值。此方法支持LINQ查询表达式，并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the collection, or returns the specified default value if the collection is empty. This method supports LINQ query expressions and allows extracting values to compare through a selector function.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型，必须实现IComparable接口 / The type of the result, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="selector">用于从每个元素中提取值的函数，不能为null / The function to extract values from each element, cannot be null</param>
    /// <param name="defaultValue">集合为空时返回的默认值，可以为null / The default value to return if the collection is empty, can be null</param>
    /// <returns>集合中的最小值，如果集合为空则返回指定的默认值 / The minimum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MinOrDefaultValue<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回默认值。此方法支持LINQ查询表达式，直接比较集合中的元素。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the collection, or returns the default value if the collection is empty. This method supports LINQ query expressions and directly compares elements in the collection.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型，必须实现IComparable接口 / The element type of the collection, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <returns>集合中的最小值，如果集合为空则返回类型TSource的默认值 / The minimum value in the collection, or the default value of TSource if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MinOrDefaultValue<TSource>(this IQueryable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回指定的默认值。此方法支持LINQ查询表达式，直接比较集合中的元素。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the collection, or returns the specified default value if the collection is empty. This method supports LINQ query expressions and directly compares elements in the collection.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型，必须实现IComparable接口 / The element type of the collection, must implement IComparable interface</typeparam>
    /// <param name="source">要查询的集合，不能为null / The collection to query, cannot be null</param>
    /// <param name="defaultValue">集合为空时返回的默认值，可以为null / The default value to return if the collection is empty, can be null</param>
    /// <returns>集合中的最小值，如果集合为空则返回指定的默认值 / The minimum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MinOrDefaultValue<TSource>(this IQueryable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回默认值。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the collection, or returns the default value if the collection is empty.
    /// This method first uses the selector function to convert TSource type to TResult type,
    /// then if the result sequence is empty, returns the default value of TResult, otherwise returns the minimum value.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型 / The type of the result</typeparam>
    /// <param name="source">要查询的集合 / The collection to query</param>
    /// <param name="selector">用于从每个元素中提取值的函数 / The function to extract values from each element</param>
    /// <returns>集合中的最小值，如果集合为空则返回默认值 / The minimum value in the collection, or the default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MinOrDefaultValue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回指定的默认值。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the collection, or returns the specified default value if the collection is empty.
    /// The difference between this method and MinOrDefault is that it allows specifying a custom default value
    /// instead of using the type's default value. This is particularly useful when dealing with value types.
    /// </remarks>
    /// <typeparam name="TSource">集合中元素的类型 / The element type of the collection</typeparam>
    /// <typeparam name="TResult">结果的类型 / The type of the result</typeparam>
    /// <param name="source">要查询的集合 / The collection to query</param>
    /// <param name="selector">用于从每个元素中提取值的函数 / The function to extract values from each element</param>
    /// <param name="defaultValue">集合为空时返回的默认值 / The default value to return if the collection is empty</param>
    /// <returns>集合中的最小值，如果集合为空则返回指定的默认值 / The minimum value in the collection, or the specified default value if the collection is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="selector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="selector"/> is null</exception>
    public static TResult MinOrDefaultValue<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));

        return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 获取序列中的最小值，如果序列为空，则返回默认值。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the sequence, or returns the default value if the sequence is empty.
    /// This method is a simplified version that operates directly on sequence elements,
    /// without requiring a selector function, suitable for scenarios where sequence elements are compared directly.
    /// </remarks>
    /// <typeparam name="TSource">序列中元素的类型 / The element type of the sequence</typeparam>
    /// <param name="source">要从中获取最小值的序列 / The sequence to get the minimum value from</param>
    /// <returns>序列中的最小值，如果序列为空则返回默认值 / The minimum value in the sequence, or the default value if the sequence is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MinOrDefaultValue<TSource>(this IEnumerable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取序列中的最小值，如果序列为空，则返回指定的默认值。
    /// </summary>
    /// <remarks>
    /// Gets the minimum value in the sequence, or returns the specified default value if the sequence is empty.
    /// This method allows specifying a custom default return value for empty sequences,
    /// avoiding potential issues with using the type's default value.
    /// </remarks>
    /// <typeparam name="TSource">序列中元素的类型 / The element type of the sequence</typeparam>
    /// <param name="source">要从中获取最小值的序列 / The sequence to get the minimum value from</param>
    /// <param name="defaultValue">如果序列为空时返回的默认值 / The default value to return if the sequence is empty</param>
    /// <returns>序列中的最小值，如果序列为空则返回指定的默认值 / The minimum value in the sequence, or the specified default value if the sequence is empty</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static TSource MinOrDefaultValue<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 计算序列的标准差。
    /// </summary>
    /// <remarks>
    /// Calculates the standard deviation of the sequence.
    /// This method uses the following steps to calculate standard deviation:
    /// 1. Calculate the average of the sequence
    /// 2. Calculate the sum of squared differences from the mean
    /// 3. Divide the sum by the number of elements and take the square root
    /// Note: Returns 0 when the sequence has fewer than 2 elements
    /// </remarks>
    /// <param name="source">要计算标准差的双精度浮点数序列 / The sequence of double-precision floating-point numbers to calculate standard deviation</param>
    /// <returns>序列的标准差 / The standard deviation of the sequence</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static double StandardDeviation(this IEnumerable<double> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        double result = 0;
        var list = source as ICollection<double> ?? source.ToList();
        var count = list.Count;
        if (count > 1)
        {
            var avg = list.Average();
            var sum = list.Sum(d => (d - avg) * (d - avg));
            result = Math.Sqrt(sum / count);
        }

        return result;
    }

    /// <summary>
    /// 按随机顺序对序列进行排序。
    /// </summary>
    /// <remarks>
    /// Sorts the sequence in random order.
    /// This method implements random sorting by assigning a random GUID as the sorting key for each element.
    /// Note: The randomness of this method depends on GUID randomness, suitable for general scenarios,
    /// but if higher quality random sorting is needed, it is recommended to use a dedicated random number generator.
    /// </remarks>
    /// <typeparam name="T">序列中元素的类型 / The element type of the sequence</typeparam>
    /// <param name="source">要排序的序列 / The sequence to sort</param>
    /// <returns>按随机顺序排序后的序列 / The sequence sorted in random order</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 为 null 时抛出 / Thrown when <paramref name="source"/> is null</exception>
    public static IOrderedEnumerable<T> OrderByRandom<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));

        return source.OrderBy(_ => Guid.NewGuid());
    }

    /// <summary>
    /// 判断两个相同类型的序列是否相等，使用自定义的比较条件。
    /// </summary>
    /// <remarks>
    /// Determines whether two sequences of the same type are equal using a custom comparison condition.
    /// This method first attempts optimized comparison paths:
    /// 1. If both sequences are collections, compare counts first
    /// 2. If both sequences are lists, use indexed access for comparison
    /// 3. If neither condition is met, use enumerators to compare element by element
    ///
    /// The comparison process ensures:
    /// - Both sequences have equal length
    /// - Elements at the same position satisfy the comparison condition
    /// - Uses custom comparison logic instead of default equality comparison
    ///
    /// Note: This overload is specifically for comparing sequences of the same type. For comparing sequences of different types, use the generic overload version.
    /// </remarks>
    /// <typeparam name="T">序列中元素的类型 / The element type of the sequences</typeparam>
    /// <param name="first">第一个序列 / The first sequence</param>
    /// <param name="second">第二个序列 / The second sequence</param>
    /// <param name="condition">用于比较两个元素的条件 / The condition to compare two elements</param>
    /// <returns>如果两个序列相等则返回 <c>true</c>，否则返回 <c>false</c> / Returns <c>true</c> if the sequences are equal; otherwise <c>false</c></returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/>、<paramref name="second"/> 或 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="first"/>, <paramref name="second"/>, or <paramref name="condition"/> is null</exception>
    public static bool SequenceEqualSameType<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        if (first is ICollection<T> source1 && second is ICollection<T> source2)
        {
            if (source1.Count != source2.Count)
            {
                return false;
            }

            if (source1 is IList<T> list1 && source2 is IList<T> list2)
            {
                var count = source1.Count;
                for (var index = 0; index < count; ++index)
                {
                    if (!condition(list1[index], list2[index]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        using var enumerator1 = first.GetEnumerator();
        using var enumerator2 = second.GetEnumerator();
        while (enumerator1.MoveNext())
        {
            if (!enumerator2.MoveNext() || !condition(enumerator1.Current, enumerator2.Current))
            {
                return false;
            }
        }

        return !enumerator2.MoveNext();
    }

    /// <summary>
    /// 判断两个不同类型的序列是否相等，使用自定义的比较条件。此方法会先尝试使用集合和列表的优化比较方式，如果不可用则使用枚举器逐个比较。
    /// </summary>
    /// <remarks>
    /// Determines whether two sequences of different types are equal using a custom comparison condition. This method first attempts optimized comparison using collections and lists, if unavailable, uses enumerators to compare element by element.
    /// Performance optimization notes:
    /// 1. First checks if they are ICollection for fast length comparison
    /// 2. Then checks if they are IList to support indexed access
    /// 3. Finally uses enumerators for element-by-element comparison
    /// </remarks>
    /// <typeparam name="T1">第一个序列中元素的类型 / The element type of the first sequence</typeparam>
    /// <typeparam name="T2">第二个序列中元素的类型 / The element type of the second sequence</typeparam>
    /// <param name="first">第一个序列 / The first sequence</param>
    /// <param name="second">第二个序列 / The second sequence</param>
    /// <param name="condition">用于比较两个元素的条件，接受两个不同类型的参数并返回bool值 / The condition to compare two elements, accepts two parameters of different types and returns a bool value</param>
    /// <returns>如果两个序列长度相等且对应位置的元素都满足比较条件，则返回 <c>true</c>；否则返回 <c>false</c> / Returns <c>true</c> if sequences have equal length and corresponding elements satisfy the comparison condition; otherwise <c>false</c></returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="first"/>、<paramref name="second"/> 或 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="first"/>, <paramref name="second"/>, or <paramref name="condition"/> is null</exception>
    public static bool SequenceEqual<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        if (first is ICollection<T1> source1 && second is ICollection<T2> source2)
        {
            if (source1.Count != source2.Count)
            {
                return false;
            }

            if (source1 is IList<T1> list1 && source2 is IList<T2> list2)
            {
                var count = source1.Count;
                for (var index = 0; index < count; ++index)
                {
                    if (!condition(list1[index], list2[index]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        using var enumerator1 = first.GetEnumerator();
        using var enumerator2 = second.GetEnumerator();
        while (enumerator1.MoveNext())
        {
            if (!enumerator2.MoveNext() || !condition(enumerator1.Current, enumerator2.Current))
            {
                return false;
            }
        }

        return !enumerator2.MoveNext();
    }

    /// <summary>
    /// 对比两个集合，找出新增的、删除的和修改的项。此方法通过比较条件识别元素的对应关系，适用于版本对比、数据同步等场景。
    /// </summary>
    /// <remarks>
    /// Compares two collections to find added, removed, and modified items. This method identifies element correspondence through comparison conditions, suitable for version comparison, data synchronization, and similar scenarios.
    /// If input collections are null, they will be converted to empty lists for processing.
    /// The three returned lists are all new List instances and will not affect the original data.
    /// </remarks>
    /// <typeparam name="T1">第一个集合中元素的类型 / The element type of the first collection</typeparam>
    /// <typeparam name="T2">第二个集合中元素的类型 / The element type of the second collection</typeparam>
    /// <param name="first">第一个集合，通常表示新数据 / The first collection, usually representing new data</param>
    /// <param name="second">第二个集合，通常表示旧数据 / The second collection, usually representing old data</param>
    /// <param name="condition">用于比较两个元素是否对应的条件函数 / The condition function to determine if two elements correspond</param>
    /// <returns>返回一个元组，包含：
    /// - adds: 在first中存在但在second中不存在的新增项
    /// - remove: 在second中存在但在first中不存在的删除项
    /// - updates: 在两个集合中都存在的更新项
    /// / Returns a tuple containing:
    /// - adds: Items in first but not in second (added items)
    /// - remove: Items in second but not in first (removed items)
    /// - updates: Items in both collections (updated items)</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="condition"/> is null</exception>
    public static (List<T1> adds, List<T2> remove, List<T1> updates) CompareChanges<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        first ??= new List<T1>();
        second ??= new List<T2>();
        var firstSource = first as ICollection<T1> ?? first.ToList();
        var secondSource = second as ICollection<T2> ?? second.ToList();
        var add = firstSource.ExceptByExpression(secondSource, condition).ToList();
        var remove = secondSource.ExceptByExpression(firstSource, (s, f) => condition(f, s)).ToList();
        var update = firstSource.IntersectByComparer(secondSource, condition).ToList();
        return (add, remove, update);
    }

    /// <summary>
    /// 对比两个集合，找出新增的、删除的和修改的项，并返回修改项的详细信息。此方法是CompareChanges的增强版本，提供更详细的修改项信息。
    /// </summary>
    /// <remarks>
    /// Compares two collections to find added, removed, and modified items, returning detailed information about modified items. This method is an enhanced version of CompareChanges, providing more detailed modification information.
    /// The main difference from CompareChanges is the return type of updates, which preserves complete information of modified items in both collections.
    /// Suitable for scenarios requiring detailed tracking of data changes, such as data synchronization, change logging, etc.
    /// </remarks>
    /// <typeparam name="T1">第一个集合中元素的类型 / The element type of the first collection</typeparam>
    /// <typeparam name="T2">第二个集合中元素的类型 / The element type of the second collection</typeparam>
    /// <param name="first">第一个集合，通常表示新数据 / The first collection, usually representing new data</param>
    /// <param name="second">第二个集合，通常表示旧数据 / The second collection, usually representing old data</param>
    /// <param name="condition">用于比较两个元素是否对应的条件函数 / The condition function to determine if two elements correspond</param>
    /// <returns>返回一个元组，包含：
    /// - adds: 在first中存在但在second中不存在的新增项
    /// - remove: 在second中存在但在first中不存在的删除项
    /// - updates: 包含修改项的新旧值对的列表，每一项都是一个元组，包含对应的first和second中的元素
    /// / Returns a tuple containing:
    /// - adds: Items in first but not in second (added items)
    /// - remove: Items in second but not in first (removed items)
    /// - updates: A list of old-new value pairs for modified items, each item is a tuple containing corresponding elements from first and second</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="condition"/> is null</exception>
    public static (List<T1> adds, List<T2> remove, List<(T1 first, T2 second)> updates) CompareChangesPlus<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        first ??= new List<T1>();
        second ??= new List<T2>();
        var firstSource = first as ICollection<T1> ?? first.ToList();
        var secondSource = second as ICollection<T2> ?? second.ToList();
        var add = firstSource.ExceptByExpression(secondSource, condition).ToList();
        var remove = secondSource.ExceptByExpression(firstSource, (s, f) => condition(f, s)).ToList();
        var updates = firstSource.IntersectByComparer(secondSource, condition).Select(t1 => (t1, secondSource.FirstOrDefault(t2 => condition(t1, t2)))).ToList();
        return (add, remove, updates);
    }

    /// <summary>
    /// 将列表声明为非空列表，如果列表为 null，则返回一个新的空列表。此方法用于确保返回值永远不会为null，简化空值检查。
    /// </summary>
    /// <remarks>
    /// Declares a list as non-null, returning a new empty list if the list is null. This method ensures the return value is never null, simplifying null checks.
    /// This method is particularly useful for avoiding null reference exceptions, eliminating the need for null checks before using the list.
    /// </remarks>
    /// <typeparam name="T">列表中元素的类型 / The element type of the list</typeparam>
    /// <param name="list">要检查的列表，可以为null / The list to check, can be null</param>
    /// <returns>原列表如果不为null，则返回原列表；否则返回新的空列表 / The original list if not null; otherwise a new empty list</returns>
    public static List<T> AsNotNull<T>(this List<T> list)
    {
        return list ?? new List<T>();
    }

    /// <summary>
    /// 将集合声明为非空集合，如果集合为 null，则返回一个新的空集合。此方法是AsNotNull的IEnumerable版本。
    /// </summary>
    /// <remarks>
    /// Declares a collection as non-null, returning a new empty collection if the collection is null. This method is the IEnumerable version of AsNotNull.
    /// This method returns an IEnumerable interface, providing better abstraction and interoperability.
    /// Suitable for scenarios that need to handle collection parameters that may be null.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="list">要检查的集合，可以为null / The collection to check, can be null</param>
    /// <returns>原集合如果不为null，则返回原集合；否则返回新的空列表 / The original collection if not null; otherwise a new empty list</returns>
    public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T> list)
    {
        return list ?? new List<T>();
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。此方法提供条件性的Where子句，可以根据布尔值决定是否执行筛选。
    /// </summary>
    /// <remarks>
    /// Executes a filter operation if the condition is met. This method provides a conditional Where clause that can decide whether to filter based on a boolean value.
    /// This method can reduce conditional branches in code, making query logic more concise.
    /// Suitable for dynamic query condition scenarios.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="source">要筛选的集合 / The collection to filter</param>
    /// <param name="condition">决定是否执行筛选的布尔条件 / The boolean condition that determines whether to filter</param>
    /// <param name="where">用于筛选的条件表达式 / The condition expression used for filtering</param>
    /// <returns>如果condition为 <c>true</c>，返回经过where筛选的集合；否则返回原始集合 / If condition is <c>true</c>, returns the filtered collection; otherwise returns the original collection</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="where"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="where"/> is null</exception>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> where)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(where, nameof(where));

        return condition ? source.Where(where) : source;
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。此方法是WhereIf的重载版本，接受一个返回布尔值的委托作为条件。
    /// </summary>
    /// <remarks>
    /// Executes a filter operation if the condition is met. This method is an overload of WhereIf that accepts a delegate returning a boolean value as the condition.
    /// This overload version allows using more complex condition logic.
    /// The condition delegate will be executed when the method is called, providing lazy evaluation capability.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="source">要筛选的集合 / The collection to filter</param>
    /// <param name="condition">返回布尔值的条件表达式 / The condition expression returning a boolean value</param>
    /// <param name="where">用于筛选的条件表达式 / The condition expression used for filtering</param>
    /// <returns>如果condition()返回 <c>true</c>，返回经过where筛选的集合；否则返回原始集合 / If condition() returns <c>true</c>, returns the filtered collection; otherwise returns the original collection</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/>、<paramref name="condition"/> 或 <paramref name="where"/> 为 null 时抛出 / Thrown when <paramref name="source"/>, <paramref name="condition"/>, or <paramref name="where"/> is null</exception>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<bool> condition, Func<T, bool> where)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        ArgumentNullException.ThrowIfNull(where, nameof(where));

        return condition() ? source.Where(where) : source;
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。此方法用于在查询时根据条件动态添加筛选条件。
    /// </summary>
    /// <remarks>
    /// Executes a filter operation if the condition is met. This method is used to dynamically add filter conditions during queries.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="source">要筛选的查询集合 / The query collection to filter</param>
    /// <param name="condition">决定是否执行筛选的布尔条件，当为 <c>true</c> 时执行筛选，为 <c>false</c> 时返回原集合 / The boolean condition that determines whether to filter, executes filter when <c>true</c>, returns original collection when <c>false</c></param>
    /// <param name="where">用于筛选的条件表达式，只有当condition为 <c>true</c> 时才会被执行 / The condition expression used for filtering, only executed when condition is <c>true</c></param>
    /// <returns>筛选后的查询集合，如果条件不满足则返回原查询集合 / The filtered query collection, or the original query collection if the condition is not met</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="where"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="where"/> is null</exception>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> where)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(where, nameof(where));

        return condition ? source.Where(where) : source;
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。此方法允许使用委托函数动态判断是否需要执行筛选。
    /// </summary>
    /// <remarks>
    /// Executes a filter operation if the condition is met. This method allows using a delegate function to dynamically determine whether filtering is needed.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="source">要筛选的查询集合 / The query collection to filter</param>
    /// <param name="condition">决定是否执行筛选的布尔条件表达式，此委托将在执行时被调用以确定是否应用筛选条件 / The boolean condition expression that determines whether to filter, this delegate will be called at execution time to determine whether to apply the filter condition</param>
    /// <param name="where">用于筛选的条件表达式，只有当condition委托返回 <c>true</c> 时才会被执行 / The condition expression used for filtering, only executed when the condition delegate returns <c>true</c></param>
    /// <returns>筛选后的查询集合，如果条件不满足则返回原查询集合 / The filtered query collection, or the original query collection if the condition is not met</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/>、<paramref name="condition"/> 或 <paramref name="where"/> 为 null 时抛出 / Thrown when <paramref name="source"/>, <paramref name="condition"/>, or <paramref name="where"/> is null</exception>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Func<bool> condition, Expression<Func<T, bool>> where)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        ArgumentNullException.ThrowIfNull(where, nameof(where));

        return condition() ? source.Where(where) : source;
    }

    /// <summary>
    /// 改变集合中指定元素的索引位置。此方法会将指定元素移动到新的索引位置，同时保持其他元素的相对顺序。
    /// </summary>
    /// <remarks>
    /// Changes the index position of a specified element in the collection. This method moves the specified element to a new index position while maintaining the relative order of other elements.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="list">要操作的集合，必须是支持随机访问的列表类型 / The collection to operate on, must be a list type supporting random access</param>
    /// <param name="item">要改变索引位置的元素，此元素必须存在于列表中 / The element to change index position, must exist in the list</param>
    /// <param name="index">新的索引位置，如果索引超出范围，将被自动调整到有效范围内 / The new index position, will be automatically adjusted to valid range if out of bounds</param>
    /// <returns>操作后的集合，以支持方法链式调用 / The collection after operation, supporting method chaining</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 或 <paramref name="item"/> 为 null 时抛出 / Thrown when <paramref name="list"/> or <paramref name="item"/> is null</exception>
    public static IList<T> ChangeIndex<T>(this IList<T> list, T item, int index)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(item, nameof(item));

        ChangeIndexInternal(list, item, index);
        return list;
    }

    /// <summary>
    /// 改变集合中满足条件的元素的索引位置。此方法会查找第一个满足条件的元素并移动到新的索引位置。
    /// </summary>
    /// <remarks>
    /// Changes the index position of an element that satisfies the condition. This method finds the first element satisfying the condition and moves it to a new index position.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="list">要操作的集合，必须是支持随机访问的列表类型 / The collection to operate on, must be a list type supporting random access</param>
    /// <param name="condition">用于定位元素的条件表达式，将返回满足此条件的第一个元素 / The condition expression to locate element, returns the first element satisfying this condition</param>
    /// <param name="index">新的索引位置，如果索引超出范围，将被自动调整到有效范围内 / The new index position, will be automatically adjusted to valid range if out of bounds</param>
    /// <returns>操作后的集合，以支持方法链式调用。如果没有找到满足条件的元素，则返回原始集合 / The collection after operation, supporting method chaining. Returns original collection if no element satisfies the condition</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 或 <paramref name="condition"/> 为 null 时抛出 / Thrown when <paramref name="list"/> or <paramref name="condition"/> is null</exception>
    public static IList<T> ChangeIndex<T>(this IList<T> list, Func<T, bool> condition, int index)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));

        var item = list.FirstOrDefault(condition);
        if (item != null)
        {
            ChangeIndexInternal(list, item, index);
        }

        return list;
    }

    /// <summary>
    /// 内部方法，用于实际改变元素的索引位置。此方法确保新索引在有效范围内，并执行实际的重新排序操作。
    /// </summary>
    /// <remarks>
    /// Internal method for actually changing the element's index position. This method ensures the new index is within valid range and performs the actual reordering operation.
    /// This method first removes the specified element from the list, then inserts the element at the new position.
    /// If the specified index exceeds the valid range of the list, it will be automatically adjusted to the nearest valid index.
    /// </remarks>
    /// <typeparam name="T">集合中元素的类型 / The element type of the collection</typeparam>
    /// <param name="list">要操作的集合，必须是支持随机访问的列表类型 / The collection to operate on, must be a list type supporting random access</param>
    /// <param name="item">要改变索引位置的元素，此元素必须存在于列表中 / The element to change index position, must exist in the list</param>
    /// <param name="index">新的索引位置，将被自动调整到0到(列表长度-1)的范围内 / The new index position, will be automatically adjusted to range 0 to (list length - 1)</param>
    private static void ChangeIndexInternal<T>(IList<T> list, T item, int index)
    {
        index = Math.Max(0, index);
        index = Math.Min(list.Count - 1, index);
        list.Remove(item);
        list.Insert(index, item);
    }
}