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
    private static readonly object LockObject = new();

    /// <summary>
    /// 时间戳起始点（2025-01-01 00:00:00 UTC）
    /// </summary>
    /// <value>
    /// 以毫秒为单位的时间戳起始点
    /// </value>
    /// <remarks>
    /// <para>
    /// ⚠️ 注意：必须在访问 <see cref="Instance"/> 之前设置此属性。一旦实例创建后，修改此属性将不会生效。
    /// </para>
    /// <para>
    /// 雪花算法使用 41 位时间戳，默认起始时间为 2025-01-01 00:00:00 UTC，可使用约 69 年。
    /// 如需使用其他起始时间（如 2020-01-01），请在首次访问 <see cref="Instance"/> 前设置此属性。
    /// </para>
    /// <example>
    /// <code>
    /// // 正确用法：在访问 Instance 前设置
    /// SnowFlakeIdHelper.BaseTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks / 10000;
    /// var id = SnowFlakeIdHelper.Instance.NextId();
    /// </code>
    /// </example>
    /// </remarks>
    public static long BaseTime { get; set; } = (long)(UtcTimeStart - EpochTime).TotalMilliseconds;

    /// <summary>
    /// 内部的 IdWorker 实例
    /// </summary>
    private static IdWorker _worker;

    /// <summary>
    /// 工作节点ID提供者。设置后将在首次访问 Instance 时自动解析 WorkerId。
    /// </summary>
    /// <remarks>
    /// Worker ID provider. When set, WorkerId is resolved automatically before Instance creation.
    /// Must be set before the first access to <see cref="Instance"/>.
    /// </remarks>
    private static IWorkerIdProvider _workerIdProvider;

    /// <summary>
    /// WorkerId 冲突检测器。
    /// </summary>
    private static WorkerIdConflictDetector _workerIdConflictDetector = new WorkerIdConflictDetector();

    /// <summary>
    /// 最近一次 WorkerId 注册冲突检测结果。
    /// </summary>
    public static WorkerIdConflictResult LastWorkerIdConflict { get; private set; }

    /// <summary>
    /// 设置工作节点ID提供者。必须在首次访问 <see cref="Instance"/> 之前调用。
    /// </summary>
    /// <remarks>
    /// Sets the worker ID provider. Must be called before the first access to <see cref="Instance"/>.
    /// </remarks>
    /// <param name="provider">工作节点ID提供者 / Worker ID provider</param>
    public static void SetWorkerIdProvider(IWorkerIdProvider provider)
    {
        _workerIdProvider = provider;
    }

    /// <summary>
    /// 设置 WorkerId 冲突检测器。传入 null 将恢复默认进程内检测器。
    /// </summary>
    /// <param name="detector">冲突检测器 / Conflict detector</param>
    public static void SetWorkerIdConflictDetector(WorkerIdConflictDetector detector)
    {
        _workerIdConflictDetector = detector ?? new WorkerIdConflictDetector();
        LastWorkerIdConflict = null;
    }

    /// <summary>
    /// 检查当前 Snowflake 配置是否与已注册节点冲突，不修改注册表。
    /// </summary>
    /// <param name="nodeId">节点标识，默认使用机器名 / Node id, machine name by default</param>
    /// <returns>冲突检测结果 / Conflict detection result</returns>
    public static WorkerIdConflictResult CheckWorkerIdConflict(string nodeId = null)
    {
        nodeId ??= Environment.MachineName;
        return _workerIdConflictDetector.Check(nodeId, DataCenterId, WorkId, _workerIdProvider?.Name ?? "Manual");
    }

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
    public static int WorkId { get; set; } = 1;

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
    public static int DataCenterId { get; set; } = 1;

    /// <summary>
    /// 获取 <see cref="IdWorker"/> 的单例实例
    /// </summary>
    /// <value>
    /// 全局唯一的 <see cref="IdWorker"/> 实例
    /// </value>
    /// <remarks>
    /// 使用双重检查锁定模式实现线程安全的单例模式。
    /// 实例使用当前的 <see cref="WorkId"/> 和 <see cref="DataCenterId"/> 值进行初始化。
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
                        if (_workerIdProvider != null)
                        {
                            WorkId = (int)_workerIdProvider.GetWorkerId();
                        }

                        LastWorkerIdConflict = _workerIdConflictDetector.Register(Environment.MachineName, DataCenterId, WorkId, _workerIdProvider?.Name ?? "Manual");
                        _worker = new IdWorker(WorkId, DataCenterId, BaseTime);
                    }
                }
            }

            return _worker;
        }
    }
}
