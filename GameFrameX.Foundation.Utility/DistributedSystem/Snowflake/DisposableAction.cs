namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// 可释放的动作包装器，实现了 <see cref="IDisposable"/> 接口
/// </summary>
/// <remarks>
/// 该类用于包装一个 <see cref="Action"/> 委托，当对象被释放时自动执行该动作。
/// 常用于需要在特定作用域结束时执行清理操作的场景。
/// </remarks>
/// <example>
/// <code>
/// using (var disposableAction = new DisposableAction(() => Console.WriteLine("清理完成")))
/// {
///     // 执行一些操作
/// }
/// // 当离开 using 作用域时，会自动执行传入的动作
/// </code>
/// </example>
public class DisposableAction : IDisposable
{
    /// <summary>
    /// 存储要在释放时执行的动作
    /// </summary>
    private readonly Action _action;

    /// <summary>
    /// 初始化 <see cref="DisposableAction"/> 类的新实例
    /// </summary>
    /// <param name="action">要在对象释放时执行的动作</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="action"/> 为 null 时抛出</exception>
    /// <remarks>
    /// 传入的动作将在调用 <see cref="Dispose"/> 方法时执行
    /// </remarks>
    public DisposableAction(Action action)
    {
        ArgumentNullException.ThrowIfNull(action, nameof(action));
        _action = action;
    }

    /// <summary>
    /// 释放资源并执行构造函数中传入的动作
    /// </summary>
    /// <remarks>
    /// 该方法实现了 <see cref="IDisposable.Dispose"/> 接口，
    /// 会执行在构造函数中传入的 <see cref="Action"/> 委托
    /// </remarks>
    public void Dispose()
    {
        _action();
    }
}