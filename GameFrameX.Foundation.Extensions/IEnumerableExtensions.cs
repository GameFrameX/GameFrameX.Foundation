// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq.Expressions;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供IEnumerable类型的扩展方法集合
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// 按字段属性判等取交集。此方法允许使用自定义条件比较两个不同类型集合的元素。
    /// </summary>
    /// <typeparam name="TFirst">第一个集合的元素类型</typeparam>
    /// <typeparam name="TSecond">第二个集合的元素类型</typeparam>
    /// <param name="first">第一个集合</param>
    /// <param name="second">第二个集合</param>
    /// <param name="condition">用于判断两个元素是否相等的条件，返回true表示元素相等</param>
    /// <returns>返回两个集合中满足条件的交集元素</returns>
    /// <exception cref="ArgumentNullException">当first、second或condition为null时抛出</exception>
    public static IEnumerable<TFirst> IntersectBy<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        
        return first.Where(f => second.Any(s => condition(f, s)));
    }

    /// <summary>
    /// 按字段属性判等取交集。此方法使用默认的相等比较器。
    /// </summary>
    /// <typeparam name="TSource">集合的元素类型</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型</typeparam>
    /// <param name="first">第一个集合</param>
    /// <param name="second">第二个集合</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数</param>
    /// <returns>返回两个集合中具有相同键的交集元素</returns>
    /// <exception cref="ArgumentNullException">当first、second或keySelector为null时抛出</exception>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        
        return first.IntersectBy(second, keySelector, null);
    }

    /// <summary>
    /// 按字段属性判等取交集。此方法允许使用自定义的相等比较器。
    /// </summary>
    /// <typeparam name="TSource">集合的元素类型</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型</typeparam>
    /// <param name="first">第一个集合</param>
    /// <param name="second">第二个集合</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数</param>
    /// <param name="comparer">用于比较键的比较器，如果为null则使用默认比较器</param>
    /// <returns>返回两个集合中具有相同键的交集元素</returns>
    /// <exception cref="ArgumentNullException">first、second或keySelector为null时抛出</exception>
    public static IEnumerable<TSource> IntersectBy<TSource, TKey>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
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
    /// <typeparam name="T">集合的元素类型</typeparam>
    /// <param name="source">多个集合的序列</param>
    /// <returns>返回所有集合的交集元素</returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出</exception>
    public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.Aggregate((current, item) => current.Intersect(item));
    }

    /// <summary>
    /// 多个集合按指定键选择器取交集元素。使用默认的相等比较器。
    /// </summary>
    /// <typeparam name="TSource">集合的元素类型</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型</typeparam>
    /// <param name="source">多个集合的序列</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数</param>
    /// <returns>返回所有集合的交集元素</returns>
    /// <exception cref="ArgumentNullException">当source或keySelector为null时抛出</exception>
    public static IEnumerable<TSource> IntersectAll<TSource, TKey>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        
        return source.Aggregate((current, item) => current.IntersectBy(item, keySelector));
    }

    /// <summary>
    /// 多个集合按指定键选择器取交集元素。允许使用自定义的相等比较器。
    /// </summary>
    /// <typeparam name="TSource">集合的元素类型</typeparam>
    /// <typeparam name="TKey">键的选择器返回的键类型</typeparam>
    /// <param name="source">多个集合的序列</param>
    /// <param name="keySelector">用于从每个元素中提取键的函数</param>
    /// <param name="comparer">用于比较键的比较器</param>
    /// <returns>返回所有集合的交集元素</returns>
    /// <exception cref="ArgumentNullException">当source或keySelector为null时抛出</exception>
    public static IEnumerable<TSource> IntersectAll<TSource, TKey>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        
        return source.Aggregate((current, item) => current.IntersectBy(item, keySelector, comparer));
    }

    /// <summary>
    /// 多个集合取交集元素。允许使用自定义的相等比较器。
    /// </summary>
    /// <typeparam name="T">集合的元素类型</typeparam>
    /// <param name="source">多个集合的序列</param>
    /// <param name="comparer">用于比较元素的比较器</param>
    /// <returns>返回所有集合的交集元素</returns>
    /// <exception cref="ArgumentNullException">当source为null时抛出</exception>
    public static IEnumerable<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> source, IEqualityComparer<T> comparer)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.Aggregate((current, item) => current.Intersect(item, comparer));
    }

    /// <summary>
    /// 按字段属性判等取差集。此方法允许使用自定义条件比较两个不同类型集合的元素。
    /// </summary>
    /// <typeparam name="TFirst">第一个集合的元素类型</typeparam>
    /// <typeparam name="TSecond">第二个集合的元素类型</typeparam>
    /// <param name="first">第一个集合</param>
    /// <param name="second">第二个集合</param>
    /// <param name="condition">用于判断两个元素是否相等的条件，返回true表示元素相等</param>
    /// <returns>返回第一个集合中不在第二个集合中的元素</returns>
    /// <exception cref="ArgumentNullException">当first、second或condition为null时抛出</exception>
    public static IEnumerable<TFirst> ExceptBy<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(first, nameof(first));
        ArgumentNullException.ThrowIfNull(second, nameof(second));
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        
        return first.Where(f => !second.Any(s => condition(f, s)));
    }







    /// <summary>
    /// 向集合中添加多个元素。此方法允许通过params参数传入任意数量的元素。
    /// </summary>
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">要添加元素的集合</param>
    /// <param name="values">要添加的元素数组。可以是单个元素或多个元素。</param>
    /// <exception cref="ArgumentNullException">当self或values为null时抛出</exception>
    public static void AddRange<T>(this ICollection<T> self, params T[] values)
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">要添加元素的集合</param>
    /// <param name="values">包含要添加元素的可枚举集合</param>
    /// <exception cref="ArgumentNullException">当self或values为null时抛出</exception>
    public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> values)
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
    /// <typeparam name="T">并发袋中的元素类型</typeparam>
    /// <param name="self">要添加元素的并发袋</param>
    /// <param name="values">要添加的元素数组。可以是单个元素或多个元素。</param>
    /// <exception cref="ArgumentNullException">当self或values为null时抛出</exception>
    public static void AddRange<T>(this ConcurrentBag<T> self, params T[] values)
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
    /// <typeparam name="T">并发队列中的元素类型</typeparam>
    /// <param name="self">要添加元素的并发队列</param>
    /// <param name="values">要添加的元素数组。可以是单个元素或多个元素。</param>
    /// <exception cref="ArgumentNullException">当self或values为null时抛出</exception>
    public static void AddRange<T>(this ConcurrentQueue<T> self, params T[] values)
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">要添加元素的集合</param>
    /// <param name="predicate">用于筛选元素的条件函数。返回true表示元素应被添加。</param>
    /// <param name="values">要添加的元素数组</param>
    /// <exception cref="ArgumentNullException">当self、predicate或values为null时抛出</exception>
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
    /// <typeparam name="T">并发袋中的元素类型</typeparam>
    /// <param name="self">要添加元素的并发袋</param>
    /// <param name="predicate">用于筛选元素的条件函数。返回true表示元素应被添加。</param>
    /// <param name="values">要添加的元素数组</param>
    /// <exception cref="ArgumentNullException">当self、predicate或values为null时抛出</exception>
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
    /// <typeparam name="T">并发队列中的元素类型</typeparam>
    /// <param name="self">要添加元素的并发队列</param>
    /// <param name="predicate">用于筛选元素的条件函数。返回true表示元素应被添加。</param>
    /// <param name="values">要添加的元素数组</param>
    /// <exception cref="ArgumentNullException">当self、predicate或values为null时抛出</exception>
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">要添加元素的集合</param>
    /// <param name="values">要添加的元素数组</param>
    /// <exception cref="ArgumentNullException">当self或values为null时抛出</exception>
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">要移除元素的集合</param>
    /// <param name="where">用于筛选要移除的元素的条件函数。返回true表示元素应被移除。</param>
    /// <exception cref="ArgumentNullException">当self或where为null时抛出</exception>
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">当前集合</param>
    /// <param name="condition">用于判断元素的条件函数，返回true表示在该元素后插入新值</param>
    /// <param name="value">要插入的值</param>
    /// <remarks>
    /// 如果满足条件的元素是集合的最后一个元素，则会将新值添加到集合末尾。
    /// 插入操作从后向前进行，以保证插入位置的正确性。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当self或condition为null时抛出</exception>
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="list">当前集合</param>
    /// <param name="index">要在其后插入新值的索引位置</param>
    /// <param name="value">要插入的值</param>
    /// <remarks>
    /// 如果指定的索引位置是集合的最后一个位置，则会将新值添加到集合末尾。
    /// 如果指定的索引不存在，则不会进行任何操作。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当list为null时抛出</exception>
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
    /// <typeparam name="T">源集合中的元素类型</typeparam>
    /// <typeparam name="TResult">目标HashSet中的元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="selector">用于转换元素类型的选择器函数</param>
    /// <returns>包含转换后元素的HashSet集合</returns>
    /// <remarks>
    /// HashSet会自动去除重复元素，确保结果集合中的元素唯一性。
    /// 如果选择器函数返回相同的值，只会保留其中一个。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当source或selector为null时抛出</exception>
    public static HashSet<TResult> ToHashSet<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return new HashSet<TResult>(source.Select(selector));
    }

    /// <summary>
    /// 遍历IEnumerable集合，并对每个元素执行指定的操作。此方法提供了一种简便的方式来遍历和处理集合元素。
    /// </summary>
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="self">当前集合</param>
    /// <param name="action">要对每个元素执行的操作</param>
    /// <remarks>
    /// 此方法是对foreach循环的封装，使代码更简洁。
    /// 在遍历过程中修改集合可能会导致异常。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当self或action为null时抛出</exception>
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="source">当前集合</param>
    /// <param name="maxParallelCount">同时执行的最大任务数</param>
    /// <param name="action">要对每个元素执行的异步操作</param>
    /// <param name="cancellationToken">用于取消操作的令牌</param>
    /// <returns>表示异步操作的任务</returns>
    /// <remarks>
    /// 在调试模式下会按顺序执行操作而不是并行执行。
    /// 当达到最大并行数时，会等待至少一个任务完成后才继续执行新任务。
    /// 可以通过cancellationToken取消正在进行的操作。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当source或action为null时抛出</exception>
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
    /// <typeparam name="T">集合中的元素类型</typeparam>
    /// <param name="source">当前集合</param>
    /// <param name="action">要对每个元素执行的异步操作</param>
    /// <param name="cancellationToken">用于取消操作的令牌</param>
    /// <returns>表示异步操作的任务</returns>
    /// <remarks>
    /// 如果source是ICollection类型，会使用其Count作为最大并行数。
    /// 否则会先将集合转换为List后再使用其Count作为最大并行数。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当source或action为null时抛出</exception>
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
    /// <typeparam name="T">源集合中的元素类型</typeparam>
    /// <typeparam name="TResult">结果数组中的元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="selector">用于转换元素的异步选择器函数</param>
    /// <returns>包含转换后元素的数组的任务</returns>
    /// <remarks>
    /// 所有转换操作会并行执行。
    /// 返回的任务在所有转换操作完成后完成。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当source或selector为null时抛出</exception>
    public static Task<TResult[]> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return Task.WhenAll(source.Select(selector));
    }

    /// <summary>
    /// 异步选择集合中的每个元素，并返回结果数组。此方法支持访问元素的索引位置。
    /// </summary>
    /// <typeparam name="T">源集合中的元素类型</typeparam>
    /// <typeparam name="TResult">结果数组中的元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="selector">用于转换元素的异步选择器函数，可访问元素索引</param>
    /// <returns>包含转换后元素的数组的任务</returns>
    /// <remarks>
    /// 所有转换操作会并行执行。
    /// 选择器函数可以通过索引参数获取元素在集合中的位置。
    /// 返回的任务在所有转换操作完成后完成。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当source或selector为null时抛出</exception>
    public static Task<TResult[]> SelectAsync<T, TResult>(this IEnumerable<T> source, Func<T, int, Task<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return Task.WhenAll(source.Select(selector));
    }

    /// <summary>
    /// 异步选择集合中的每个元素，并返回结果列表。此方法支持限制最大并行处理数量。
    /// </summary>
    /// <typeparam name="T">源集合中的元素类型</typeparam>
    /// <typeparam name="TResult">结果列表中的元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="selector">用于转换每个元素的异步选择器函数</param>
    /// <param name="maxParallelCount">同时处理的最大任务数量</param>
    /// <returns>包含转换后元素的结果列表</returns>
    /// <remarks>
    /// 此方法会限制并行执行的任务数量，当达到最大并行数时会等待部分任务完成后再继续处理。
    /// 已完成的任务结果会立即添加到结果列表中，未完成的任务继续执行。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当source或selector为null时抛出</exception>
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
    /// <typeparam name="T">源集合中的元素类型</typeparam>
    /// <typeparam name="TResult">结果列表中的元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="selector">用于转换每个元素的异步选择器函数，接收元素和其索引作为参数</param>
    /// <param name="maxParallelCount">同时处理的最大任务数量</param>
    /// <returns>包含转换后元素的结果列表</returns>
    /// <remarks>
    /// 此方法会限制并行执行的任务数量，当达到最大并行数时会等待部分任务完成后再继续处理。
    /// 已完成的任务结果会立即添加到结果列表中，未完成的任务继续执行。
    /// 使用Interlocked.Add确保在多线程环境下索引计数的准确性。
    /// </remarks>
    /// <exception cref="ArgumentNullException">source 或 selector 为 null 时抛出此异常</exception>
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
    /// <typeparam name="T">集合中元素的类型</typeparam>
    /// <param name="source">要遍历的集合</param>
    /// <param name="selector">要对每个元素执行的异步操作，接收元素和其索引作为参数</param>
    /// <param name="maxParallelCount">同时处理的最大任务数量</param>
    /// <param name="cancellationToken">用于取消操作的取消令牌</param>
    /// <returns>表示异步操作的任务</returns>
    /// <remarks>
    /// 在调试模式下（Debugger.IsAttached为true时），会按顺序同步执行每个操作。
    /// 在非调试模式下，会并行执行操作，但限制最大并行数量。
    /// 支持通过cancellationToken取消操作。
    /// 使用Interlocked.Add确保在多线程环境下索引计数的准确性。
    /// </remarks>
    /// <exception cref="ArgumentNullException">source 或 selector 为 null 时抛出此异常</exception>
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
    /// <typeparam name="T">集合中元素的类型</typeparam>
    /// <param name="source">要遍历的集合</param>
    /// <param name="selector">要对每个元素执行的异步操作，接收元素和其索引作为参数</param>
    /// <param name="cancellationToken">用于取消操作的取消令牌</param>
    /// <returns>表示异步操作的任务</returns>
    /// <remarks>
    /// 如果source是ICollection<T>类型，直接使用其Count作为最大并行数。
    /// 否则将source转换为List后使用其Count作为最大并行数。
    /// 这样可以确保所有元素都能得到处理，同时避免过多的并行任务。
    /// 
    /// 性能说明:
    /// 1. 对于ICollection<T>类型，避免了额外的ToList()转换开销
    /// 2. 并行度自动适配集合大小，避免创建过多任务
    /// 3. 支持通过CancellationToken取消操作
    /// 
    /// 使用示例:
    /// await collection.ForAsync(async (item, index) => {
    ///     await ProcessItemAsync(item, index);
    /// });
    /// </remarks>
    /// <exception cref="ArgumentNullException">source 或 selector 为 null 时抛出此异常</exception>
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
    /// <typeparam name="TSource">集合中元素的类型</typeparam>
    /// <typeparam name="TResult">结果的类型</typeparam>
    /// <param name="source">要查询的集合</param>
    /// <param name="selector">用于从每个元素中提取值的表达式</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TResult的默认值</returns>
    /// <remarks>
    /// 此方法通过DefaultIfEmpty确保在集合为空时返回默认值而不是抛出异常。
    /// 适用于需要安全获取最大值的LINQ查询场景。
    /// </remarks>
    /// <exception cref="ArgumentNullException">source 或 selector 为 null 时抛出此异常</exception>
    public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。
    /// 此方法支持LINQ查询表达式,并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。不能为null。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。可以为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值。</returns>
    /// <exception cref="ArgumentNullException">source或selector为null时抛出。</exception>
    public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。
    /// 此方法支持LINQ查询表达式,直接比较集合中的元素。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TSource的默认值。</returns>
    /// <exception cref="ArgumentNullException">source为null时抛出。</exception>
    public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。
    /// 此方法支持LINQ查询表达式,直接比较集合中的元素。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。可以为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值。</returns>
    /// <exception cref="ArgumentNullException">source为null时抛出。</exception>
    public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。
    /// 此方法用于普通集合,并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。不能为null。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。可以为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值。</returns>
    /// <exception cref="ArgumentNullException">source或selector为null时抛出。</exception>
    public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。
    /// 此方法用于普通集合,并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。不能为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TResult的默认值。</returns>
    /// <exception cref="ArgumentNullException">source或selector为null时抛出。</exception>
    public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回默认值。
    /// 此方法用于普通集合,直接比较集合中的元素。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回类型TSource的默认值。</returns>
    /// <exception cref="ArgumentNullException">source为null时抛出。</exception>
    public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty().Max();
    }

    /// <summary>
    /// 获取集合中的最大值，如果集合为空则返回指定的默认值。
    /// 此方法用于普通集合,直接比较集合中的元素。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。可以为null。</param>
    /// <returns>集合中的最大值，如果集合为空则返回指定的默认值。</returns>
    /// <exception cref="ArgumentNullException">source为null时抛出。</exception>
    public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty(defaultValue).Max();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回默认值。
    /// 此方法支持LINQ查询表达式,并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。不能为null。</param>
    /// <returns>集合中的最小值，如果集合为空则返回类型TResult的默认值。</returns>
    /// <exception cref="ArgumentNullException">source或selector为null时抛出。</exception>
    public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回指定的默认值。
    /// 此方法支持LINQ查询表达式,并允许通过选择器函数提取要比较的值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。不能为null。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。可以为null。</param>
    /// <returns>集合中的最小值，如果集合为空则返回指定的默认值。</returns>
    /// <exception cref="ArgumentNullException">source或selector为null时抛出。</exception>
    public static TResult MinOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回默认值。
    /// 此方法支持LINQ查询表达式,直接比较集合中的元素。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <returns>集合中的最小值，如果集合为空则返回类型TSource的默认值。</returns>
    /// <exception cref="ArgumentNullException">source为null时抛出。</exception>
    public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回指定的默认值。
    /// 此方法支持LINQ查询表达式,直接比较集合中的元素。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。必须实现IComparable接口。</typeparam>
    /// <param name="source">要查询的集合。不能为null。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。可以为null。</param>
    /// <returns>集合中的最小值，如果集合为空则返回指定的默认值。</returns>
    /// <exception cref="ArgumentNullException">source为null时抛出。</exception>
    public static TSource MinOrDefault<TSource>(this IQueryable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回默认值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。</typeparam>
    /// <param name="source">要查询的集合。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。</param>
    /// <returns>集合中的最小值，如果集合为空则返回默认值。</returns>
    /// <remarks>
    /// 此方法首先使用selector函数将TSource类型转换为TResult类型，
    /// 然后如果结果序列为空，返回TResult的默认值，否则返回最小值。
    /// </remarks>
    public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取集合中的最小值，如果集合为空则返回指定的默认值。
    /// </summary>
    /// <typeparam name="TSource">集合中元素的类型。</typeparam>
    /// <typeparam name="TResult">结果的类型。</typeparam>
    /// <param name="source">要查询的集合。</param>
    /// <param name="selector">用于从每个元素中提取值的函数。</param>
    /// <param name="defaultValue">集合为空时返回的默认值。</param>
    /// <returns>集合中的最小值，如果集合为空则返回指定的默认值。</returns>
    /// <remarks>
    /// 此方法与MinOrDefault的区别在于允许指定自定义的默认值，
    /// 而不是使用类型的默认值。这在处理值类型时特别有用。
    /// </remarks>
    public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(selector, nameof(selector));
        
        return source.Select(selector).DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 获取序列中的最小值，如果序列为空，则返回默认值。
    /// </summary>
    /// <typeparam name="TSource">序列中元素的类型。</typeparam>
    /// <param name="source">要从中获取最小值的序列。</param>
    /// <returns>序列中的最小值，如果序列为空则返回默认值。</returns>
    /// <remarks>
    /// 此方法是直接对序列元素进行操作的简化版本，
    /// 不需要提供选择器函数，适用于直接比较序列元素的场景。
    /// </remarks>
    public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty().Min();
    }

    /// <summary>
    /// 获取序列中的最小值，如果序列为空，则返回指定的默认值。
    /// </summary>
    /// <typeparam name="TSource">序列中元素的类型。</typeparam>
    /// <param name="source">要从中获取最小值的序列。</param>
    /// <param name="defaultValue">如果序列为空时返回的默认值。</param>
    /// <returns>序列中的最小值，如果序列为空则返回指定的默认值。</returns>
    /// <remarks>
    /// 此方法允许为空序列指定一个自定义的默认返回值，
    /// 避免了使用类型默认值可能带来的问题。
    /// </remarks>
    public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.DefaultIfEmpty(defaultValue).Min();
    }

    /// <summary>
    /// 计算序列的标准差。
    /// </summary>
    /// <param name="source">要计算标准差的双精度浮点数序列。</param>
    /// <returns>序列的标准差。</returns>
    /// <remarks>
    /// 此方法使用以下步骤计算标准差:
    /// 1. 计算序列的平均值
    /// 2. 计算每个值与平均值的差的平方和
    /// 3. 将平方和除以元素个数后开平方
    /// 注意：当序列元素少于2个时，返回0
    /// </remarks>
    /// <exception cref="ArgumentNullException">source 为 null 时抛出此异常</exception>
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
    /// <typeparam name="T">序列中元素的类型。</typeparam>
    /// <param name="source">要排序的序列。</param>
    /// <returns>按随机顺序排序后的序列。</returns>
    /// <remarks>
    /// 此方法通过为每个元素分配一个随机的GUID作为排序键来实现随机排序。
    /// 注意：此方法的随机性依赖于GUID的随机性，适用于一般场景，
    /// 但如果需要更高质量的随机排序，建议使用专门的随机数生成器。
    /// </remarks>
    /// <exception cref="ArgumentNullException">source 为 null 时抛出此异常</exception>
    public static IOrderedEnumerable<T> OrderByRandom<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        
        return source.OrderBy(_ => Guid.NewGuid());
    }

    /// <summary>
    /// 判断两个相同类型的序列是否相等，使用自定义的比较条件。
    /// </summary>
    /// <typeparam name="T">序列中元素的类型。</typeparam>
    /// <param name="first">第一个序列。</param>
    /// <param name="second">第二个序列。</param>
    /// <param name="condition">用于比较两个元素的条件。</param>
    /// <returns>如果两个序列相等则返回 true，否则返回 false。</returns>
    /// <remarks>
    /// 此方法首先尝试优化的比较路径：
    /// 1. 如果两个序列都是集合，先比较数量
    /// 2. 如果两个序列都是列表，使用索引访问进行比较
    /// 3. 如果以上条件都不满足，则使用枚举器逐个比较
    /// 
    /// 比较过程会确保：
    /// - 两个序列长度相等
    /// - 相同位置的元素满足比较条件
    /// - 使用自定义的比较逻辑而不是默认的相等比较
    /// 
    /// 注意：此重载专门用于相同类型的序列比较。对于不同类型的序列比较，请使用泛型重载版本。
    /// </remarks>
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
    /// 判断两个不同类型的序列是否相等，使用自定义的比较条件。
    /// 此方法会先尝试使用集合和列表的优化比较方式，如果不可用则使用枚举器逐个比较。
    /// </summary>
    /// <typeparam name="T1">第一个序列中元素的类型。</typeparam>
    /// <typeparam name="T2">第二个序列中元素的类型。</typeparam>
    /// <param name="first">第一个序列。</param>
    /// <param name="second">第二个序列。</param>
    /// <param name="condition">用于比较两个元素的条件，接受两个不同类型的参数并返回bool值。</param>
    /// <returns>如果两个序列长度相等且对应位置的元素都满足比较条件，则返回true；否则返回false。</returns>
    /// <remarks>
    /// 性能优化说明：
    /// 1. 首先检查是否为ICollection以快速比较长度
    /// 2. 然后检查是否为IList以支持索引访问
    /// 3. 最后使用枚举器进行逐个比较
    /// </remarks>
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
    /// 对比两个集合，找出新增的、删除的和修改的项。
    /// 此方法通过比较条件识别元素的对应关系，适用于版本对比、数据同步等场景。
    /// </summary>
    /// <typeparam name="T1">第一个集合中元素的类型。</typeparam>
    /// <typeparam name="T2">第二个集合中元素的类型。</typeparam>
    /// <param name="first">第一个集合，通常表示新数据。</param>
    /// <param name="second">第二个集合，通常表示旧数据。</param>
    /// <param name="condition">用于比较两个元素是否对应的条件函数。</param>
    /// <returns>返回一个元组，包含：
    /// - adds: 在first中存在但在second中不存在的新增项
    /// - remove: 在second中存在但在first中不存在的删除项
    /// - updates: 在两个集合中都存在的更新项</returns>
    /// <remarks>
    /// 如果输入集合为null，会被转换为空列表进行处理。
    /// 返回的三个列表都是新的List实例，不会影响原始数据。
    /// </remarks>
    public static (List<T1> adds, List<T2> remove, List<T1> updates) CompareChanges<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        
        first ??= new List<T1>();
        second ??= new List<T2>();
        var firstSource = first as ICollection<T1> ?? first.ToList();
        var secondSource = second as ICollection<T2> ?? second.ToList();
        var add = firstSource.ExceptBy(secondSource, condition).ToList();
        var remove = secondSource.ExceptBy(firstSource, (s, f) => condition(f, s)).ToList();
        var update = firstSource.IntersectBy(secondSource, condition).ToList();
        return (add, remove, update);
    }

    /// <summary>
    /// 对比两个集合，找出新增的、删除的和修改的项，并返回修改项的详细信息。
    /// 此方法是CompareChanges的增强版本，提供更详细的修改项信息。
    /// </summary>
    /// <typeparam name="T1">第一个集合中元素的类型。</typeparam>
    /// <typeparam name="T2">第二个集合中元素的类型。</typeparam>
    /// <param name="first">第一个集合，通常表示新数据。</param>
    /// <param name="second">第二个集合，通常表示旧数据。</param>
    /// <param name="condition">用于比较两个元素是否对应的条件函数。</param>
    /// <returns>返回一个元组，包含：
    /// - adds: 在first中存在但在second中不存在的新增项
    /// - remove: 在second中存在但在first中不存在的删除项
    /// - updates: 包含修改项的新旧值对的列表，每一项都是一个元组，包含对应的first和second中的元素</returns>
    /// <remarks>
    /// 与CompareChanges的主要区别是updates的返回类型，这里保留了修改项在两个集合中的完整信息。
    /// 适用于需要详细跟踪数据变化的场景，如数据同步、变更记录等。
    /// </remarks>
    public static (List<T1> adds, List<T2> remove, List<(T1 first, T2 second)> updates) CompareChangesPlus<T1, T2>(this IEnumerable<T1> first, IEnumerable<T2> second, Func<T1, T2, bool> condition)
    {
        ArgumentNullException.ThrowIfNull(condition, nameof(condition));
        
        first ??= new List<T1>();
        second ??= new List<T2>();
        var firstSource = first as ICollection<T1> ?? first.ToList();
        var secondSource = second as ICollection<T2> ?? second.ToList();
        var add = firstSource.ExceptBy(secondSource, condition).ToList();
        var remove = secondSource.ExceptBy(firstSource, (s, f) => condition(f, s)).ToList();
        var updates = firstSource.IntersectBy(secondSource, condition).Select(t1 => (t1, secondSource.FirstOrDefault(t2 => condition(t1, t2)))).ToList();
        return (add, remove, updates);
    }

    /// <summary>
    /// 将列表声明为非空列表，如果列表为 null，则返回一个新的空列表。
    /// 此方法用于确保返回值永远不会为null，简化空值检查。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    /// <param name="list">要检查的列表，可以为null。</param>
    /// <returns>原列表如果不为null，则返回原列表；否则返回新的空列表。</returns>
    /// <remarks>
    /// 此方法特别适用于避免空引用异常的场景，可以消除在使用列表前进行null检查的需求。
    /// </remarks>
    public static List<T> AsNotNull<T>(this List<T> list)
    {
        return list ?? new List<T>();
    }

    /// <summary>
    /// 将集合声明为非空集合，如果集合为 null，则返回一个新的空集合。
    /// 此方法是AsNotNull的IEnumerable版本。
    /// </summary>
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="list">要检查的集合，可以为null。</param>
    /// <returns>原集合如果不为null，则返回原集合；否则返回新的空列表。</returns>
    /// <remarks>
    /// 此方法返回IEnumerable接口，提供更好的抽象性和互操作性。
    /// 适用于需要处理可能为null的集合参数的场景。
    /// </remarks>
    public static IEnumerable<T> AsNotNull<T>(this IEnumerable<T> list)
    {
        return list ?? new List<T>();
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。
    /// 此方法提供条件性的Where子句，可以根据布尔值决定是否执行筛选。
    /// </summary>
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="source">要筛选的集合。</param>
    /// <param name="condition">决定是否执行筛选的布尔条件。</param>
    /// <param name="where">用于筛选的条件表达式。</param>
    /// <returns>如果condition为true，返回经过where筛选的集合；否则返回原始集合。</returns>
    /// <remarks>
    /// 此方法可以减少代码中的条件分支，使查询逻辑更简洁。
    /// 适用于动态查询条件的场景。
    /// </remarks>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> where)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(where, nameof(where));
        
        return condition ? source.Where(where) : source;
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。
    /// 此方法是WhereIf的重载版本，接受一个返回布尔值的委托作为条件。
    /// </summary>
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="source">要筛选的集合。</param>
    /// <param name="condition">返回布尔值的条件表达式。</param>
    /// <param name="where">用于筛选的条件表达式。</param>
    /// <returns>如果condition()返回true，返回经过where筛选的集合；否则返回原始集合。</returns>
    /// <remarks>
    /// 此重载版本允许使用更复杂的条件逻辑。
    /// condition委托将在方法调用时执行，提供了延迟评估的能力。
    /// </remarks>
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
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="source">要筛选的查询集合。</param>
    /// <param name="condition">决定是否执行筛选的布尔条件。当为true时执行筛选，为false时返回原集合。</param>
    /// <param name="where">用于筛选的条件表达式。只有当condition为true时才会被执行。</param>
    /// <returns>筛选后的查询集合，如果条件不满足则返回原查询集合。</returns>
    /// <exception cref="ArgumentNullException">当source或where为null时抛出。</exception>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> where)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(where, nameof(where));
        
        return condition ? source.Where(where) : source;
    }

    /// <summary>
    /// 如果满足条件，则执行筛选操作。此方法允许使用委托函数动态判断是否需要执行筛选。
    /// </summary>
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="source">要筛选的查询集合。</param>
    /// <param name="condition">决定是否执行筛选的布尔条件表达式。此委托将在执行时被调用以确定是否应用筛选条件。</param>
    /// <param name="where">用于筛选的条件表达式。只有当condition委托返回true时才会被执行。</param>
    /// <returns>筛选后的查询集合，如果条件不满足则返回原查询集合。</returns>
    /// <exception cref="ArgumentNullException">当source、condition或where为null时抛出。</exception>
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
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="list">要操作的集合。必须是支持随机访问的列表类型。</param>
    /// <param name="item">要改变索引位置的元素。此元素必须存在于列表中。</param>
    /// <param name="index">新的索引位置。如果索引超出范围，将被自动调整到有效范围内。</param>
    /// <exception cref="ArgumentNullException">如果list或item为null，则抛出此异常。</exception>
    /// <returns>操作后的集合，以支持方法链式调用。</returns>
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
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="list">要操作的集合。必须是支持随机访问的列表类型。</param>
    /// <param name="condition">用于定位元素的条件表达式。将返回满足此条件的第一个元素。</param>
    /// <param name="index">新的索引位置。如果索引超出范围，将被自动调整到有效范围内。</param>
    /// <returns>操作后的集合，以支持方法链式调用。如果没有找到满足条件的元素，则返回原始集合。</returns>
    /// <exception cref="ArgumentNullException">如果list或condition为null，则抛出此异常。</exception>
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
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    /// <param name="list">要操作的集合。必须是支持随机访问的列表类型。</param>
    /// <param name="item">要改变索引位置的元素。此元素必须存在于列表中。</param>
    /// <param name="index">新的索引位置。将被自动调整到0到(列表长度-1)的范围内。</param>
    /// <remarks>
    /// 此方法会先从列表中移除指定元素，然后在新的位置插入该元素。
    /// 如果指定的索引超出了列表的有效范围，会自动调整到最近的有效索引。
    /// </remarks>
    private static void ChangeIndexInternal<T>(IList<T> list, T item, int index)
    {
        index = Math.Max(0, index);
        index = Math.Min(list.Count - 1, index);
        list.Remove(item);
        list.Insert(index, item);
    }
}