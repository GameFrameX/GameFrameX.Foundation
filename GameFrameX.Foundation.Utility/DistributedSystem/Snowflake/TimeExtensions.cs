namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// 系统时间相关的静态工具类
/// </summary>
/// <remarks>
/// 提供获取当前时间戳的功能，并支持时间函数的模拟和替换，主要用于雪花算法中的时间戳生成。
/// 该类允许在测试环境中替换时间函数，以便进行单元测试。
/// </remarks>
/// <example>
/// <code>
/// // 获取当前时间戳
/// long currentTime = TimeSystem.CurrentTimeMillis();
/// 
/// // 在测试中模拟特定时间
/// using (TimeSystem.StubCurrentTime(1609459200000L))
/// {
///     // 在这个作用域内，CurrentTimeMillis() 将返回固定值
///     long fixedTime = System.CurrentTimeMillis(); // 返回 1609459200000L
/// }
/// </code>
/// </example>
public static class TimeSystem
{
    /// <summary>
    /// 当前时间函数委托，默认使用内部实现的时间函数
    /// </summary>
    /// <value>
    /// 返回当前时间戳（毫秒）的函数委托
    /// </value>
    /// <remarks>
    /// 可以通过 <see cref="StubCurrentTime(Func{long})"/> 方法替换此委托以进行测试
    /// </remarks>
    public static Func<long> CurrentTimeFunc = InternalCurrentTimeMillis;

    /// <summary>
    /// 获取当前时间戳（毫秒）
    /// </summary>
    /// <returns>当前时间戳，以毫秒为单位</returns>
    /// <remarks>
    /// 该方法调用 <see cref="CurrentTimeFunc"/> 委托来获取时间戳
    /// </remarks>
    public static long CurrentTimeMillis()
    {
        return CurrentTimeFunc();
    }

    /// <summary>
    /// 临时替换当前时间函数，返回可释放的对象用于恢复原始函数
    /// </summary>
    /// <param name="func">要使用的时间函数</param>
    /// <returns>实现了 <see cref="IDisposable"/> 的对象，释放时会恢复原始时间函数</returns>
    /// <remarks>
    /// 该方法主要用于单元测试，允许模拟特定的时间行为。
    /// 当返回的 <see cref="IDisposable"/> 对象被释放时，会自动恢复到默认的时间函数。
    /// </remarks>
    /// <example>
    /// <code>
    /// using (System.StubCurrentTime(() => 1609459200000L))
    /// {
    ///     // 在此作用域内使用自定义时间函数
    /// }
    /// // 离开作用域后自动恢复默认时间函数
    /// </code>
    /// </example>
    public static IDisposable StubCurrentTime(Func<long> func)
    {
        CurrentTimeFunc = func;
        return new DisposableAction(() => { CurrentTimeFunc = InternalCurrentTimeMillis; });
    }

    /// <summary>
    /// 临时替换当前时间为固定值，返回可释放的对象用于恢复原始函数
    /// </summary>
    /// <param name="millis">要返回的固定时间戳（毫秒）</param>
    /// <returns>实现了 <see cref="IDisposable"/> 的对象，释放时会恢复原始时间函数</returns>
    /// <remarks>
    /// 该方法是 <see cref="StubCurrentTime(Func{long})"/> 的便捷重载，
    /// 用于设置固定的时间戳值，常用于单元测试中模拟特定时间点。
    /// </remarks>
    /// <example>
    /// <code>
    /// using (System.StubCurrentTime(1609459200000L))
    /// {
    ///     long time = System.CurrentTimeMillis(); // 始终返回 1609459200000L
    /// }
    /// </code>
    /// </example>
    public static IDisposable StubCurrentTime(long millis)
    {
        CurrentTimeFunc = () => millis;
        return new DisposableAction(() => { CurrentTimeFunc = InternalCurrentTimeMillis; });
    }

    /// <summary>
    /// 1970年1月1日 UTC 时间的 DateTime 表示
    /// </summary>
    /// <value>
    /// Unix 时间戳的起始时间点
    /// </value>
    private static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 内部实现的获取当前时间戳的方法
    /// </summary>
    /// <returns>当前 UTC 时间相对于 1970年1月1日的毫秒数</returns>
    /// <remarks>
    /// 该方法计算当前 UTC 时间与 Unix 时间戳起始点的时间差，并转换为毫秒
    /// </remarks>
    private static long InternalCurrentTimeMillis()
    {
        return (long)(DateTime.UtcNow - Jan1St1970).TotalMilliseconds;
    }
}