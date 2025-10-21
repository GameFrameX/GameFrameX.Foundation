namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// 雪花算法单例类，提供全局唯一的 ID 生成器实例
/// </summary>
/// <remarks>
/// 该类实现了单例模式，确保整个应用程序中只有一个 <see cref="IdWorker"/> 实例。
/// 使用双重检查锁定模式来保证线程安全性和性能。
/// 默认使用 WorkId = 1 和 DatacenterId = 1 来初始化 ID 生成器。
/// </remarks>
/// <example>
/// <code>
/// // 获取单例实例并生成ID
/// var idWorker = SnowFlakeIdHelper.Instance;
/// long uniqueId = idWorker.NextId();
/// 
/// // 自定义工作节点ID和数据中心ID
/// SnowFlakeIdHelper.WorkId = 2;
/// SnowFlakeIdHelper.DatacenterId = 3;
/// var customIdWorker = SnowFlakeIdHelper.Instance; // 将使用新的配置
/// </code>
/// </example>
/// <seealso cref="IdWorker"/>
public static class SnowFlakeIdHelper
{
    /// <summary>
    /// 全局UTC起始时间，用作计数器的基准时间点
    /// 设置为2025年1月1日0时0分0秒(UTC)
    /// </summary>
    public static readonly DateTime UtcTimeStart = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 1970-01-01 00:00:00 UTC 时间
    /// </summary>
    public static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 用于线程同步的锁对象
    /// </summary>
    private static readonly object LockObject = new object();

    /// <summary>
    /// 时间戳起始点（2010-11-04 09:42:54），Twitter雪花算法的起始时间
    /// </summary>
    /// <value>
    /// 以毫秒为单位的时间戳起始点
    /// </value>
    public static long BaseTime = (UtcTimeStart - EpochTime).Milliseconds;

    /// <summary>
    /// 内部的 IdWorker 实例
    /// </summary>
    private static IdWorker _worker;

    /// <summary>
    /// 工作节点ID，默认值为 1
    /// </summary>
    /// <value>
    /// 工作节点的唯一标识符，取值范围为 0 到 31
    /// </value>
    /// <remarks>
    /// 该值在创建 <see cref="IdWorker"/> 实例之前可以修改。
    /// 一旦实例创建后，修改此值不会影响已创建的实例。
    /// </remarks>
    public static int WorkId = 1;

    /// <summary>
    /// 数据中心ID，默认值为 1
    /// </summary>
    /// <value>
    /// 数据中心的唯一标识符，取值范围为 0 到 31
    /// </value>
    /// <remarks>
    /// 该值在创建 <see cref="IdWorker"/> 实例之前可以修改。
    /// 一旦实例创建后，修改此值不会影响已创建的实例。
    /// </remarks>
    public static int DatacenterId = 1;

    /// <summary>
    /// 静态构造函数
    /// </summary>
    /// <remarks>
    /// 静态构造函数确保类型初始化的线程安全性
    /// </remarks>
    static SnowFlakeIdHelper()
    {
    }

    /// <summary>
    /// 获取 <see cref="IdWorker"/> 的单例实例
    /// </summary>
    /// <value>
    /// 全局唯一的 <see cref="IdWorker"/> 实例
    /// </value>
    /// <remarks>
    /// 使用双重检查锁定模式实现线程安全的单例模式。
    /// 实例使用当前的 <see cref="WorkId"/> 和 <see cref="DatacenterId"/> 值进行初始化。
    /// </remarks>
    /// <returns>
    /// 返回 <see cref="IdWorker"/> 的单例实例
    /// </returns>
    /// <example>
    /// <code>
    /// var worker = SnowFlakeIdHelper.Instance;
    /// long id = worker.NextId();
    /// </code>
    /// </example>
    public static IdWorker Instance
    {
        get
        {
            if (_worker == null)
            {
                lock (LockObject)
                {
                    if (_worker == null)
                    {
                        _worker = new IdWorker(WorkId, DatacenterId, BaseTime);
                    }
                }
            }

            return _worker;
        }
    }
}