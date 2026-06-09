using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供执行语义明确的异步集合扩展。
/// </summary>
public static class AsyncEnumerableExtensions
{
    /// <summary>
    /// 按输入顺序逐项执行异步操作。
    /// </summary>
    public static async Task ForEachSequentialAsync<T>(
        this IEnumerable<T> source,
        Func<T, CancellationToken, ValueTask> action,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);

        foreach (var item in source)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await action(item, cancellationToken);
        }
    }

    /// <summary>
    /// 使用受控并行度逐项执行异步操作。
    /// </summary>
    public static Task ForEachParallelAsync<T>(
        this IEnumerable<T> source,
        Func<T, CancellationToken, ValueTask> action,
        int maxDegreeOfParallelism,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(action);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxDegreeOfParallelism, 0);

        return Parallel.ForEachAsync(
            source,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
                CancellationToken = cancellationToken
            },
            action);
    }

    /// <summary>
    /// 使用受控并行度执行异步转换，可选择保持输入顺序。
    /// </summary>
    public static async Task<TResult[]> SelectParallelAsync<T, TResult>(
        this IEnumerable<T> source,
        Func<T, CancellationToken, ValueTask<TResult>> selector,
        int maxDegreeOfParallelism,
        bool preserveOrder = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(maxDegreeOfParallelism, 0);

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxDegreeOfParallelism,
            CancellationToken = cancellationToken
        };

        if (!preserveOrder)
        {
            var completionOrderResults = new ConcurrentQueue<TResult>();
            await Parallel.ForEachAsync(
                source,
                options,
                async (item, token) => completionOrderResults.Enqueue(await selector(item, token)));
            return completionOrderResults.ToArray();
        }

        var items = source as IReadOnlyList<T> ?? source.ToList();
        var results = new TResult[items.Count];
        await Parallel.ForEachAsync(
            Enumerable.Range(0, items.Count),
            options,
            async (index, token) => results[index] = await selector(items[index], token));
        return results;
    }
}
