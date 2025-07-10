// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供List和IEnumerable的扩展方法
/// </summary>
public static class ListExtensions
{
    /// <summary>
    /// 异步遍历List中的每个元素并执行指定操作
    /// </summary>
    /// <typeparam name="T">列表元素类型</typeparam>
    /// <param name="list">要遍历的列表</param>
    /// <param name="func">要执行的异步操作</param>
    /// <returns>表示异步操作的任务</returns>
    /// <exception cref="ArgumentNullException">当list或func为null时抛出</exception>
    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(func, nameof(func));

        foreach (var value in list)
        {
            await func(value);
        }
    }

    /// <summary>
    /// 异步遍历IEnumerable中的每个元素并执行指定操作
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    /// <param name="source">要遍历的集合</param>
    /// <param name="action">要执行的异步操作</param>
    /// <returns>表示异步操作的任务</returns>
    /// <exception cref="ArgumentNullException">当source或action为null时抛出</exception>
    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        foreach (var value in source)
        {
            await action(value);
        }
    }
}