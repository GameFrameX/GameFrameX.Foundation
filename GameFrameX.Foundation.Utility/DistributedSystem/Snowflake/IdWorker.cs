/** Copyright 2010-2012 Twitter, Inc.*/

/**
 * An object that generates IDs.
 * This is broken into a separate class in case
 * we ever want to support multiple worker threads
 * per process
 */

namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// 雪花算法ID生成器，用于生成全局唯一的64位长整型ID
/// </summary>
/// <remarks>
/// <para>
/// 雪花算法是Twitter开源的分布式ID生成算法，生成的ID是64位长整型数字。
/// ID的结构如下：
/// </para>
/// <list type="bullet">
/// <item><description>1位符号位（固定为0）</description></item>
/// <item><description>41位时间戳（毫秒级，可使用约69年）</description></item>
/// <item><description>5位数据中心ID（支持32个数据中心）</description></item>
/// <item><description>5位工作节点ID（每个数据中心支持32个工作节点）</description></item>
/// <item><description>12位序列号（每毫秒可生成4096个ID）</description></item>
/// </list>
/// <para>
/// 该算法保证了ID的唯一性、有序性和高性能，适用于分布式系统中的ID生成需求。
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // 创建ID生成器实例
/// var idWorker = new IdWorker(1, 1);
/// 
/// // 生成唯一ID
/// long id1 = idWorker.NextId();
/// long id2 = idWorker.GetID();
/// 
/// Console.WriteLine($"生成的ID: {id1}, {id2}");
/// </code>
/// </example>
/// <seealso cref="SnowFlakeIdHelper"/>
/// <seealso cref="InvalidSystemClock"/>
public class IdWorker
{
    /// <summary>
    /// 工作节点ID占用的位数
    /// </summary>
    const int WorkerIdBits = 5;

    /// <summary>
    /// 数据中心ID占用的位数
    /// </summary>
    const int DatacenterIdBits = 5;

    /// <summary>
    /// 序列号占用的位数
    /// </summary>
    const int SequenceBits = 12;

    /// <summary>
    /// 工作节点ID的最大值（31）
    /// </summary>
    const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);

    /// <summary>
    /// 数据中心ID的最大值（31）
    /// </summary>
    const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);

    /// <summary>
    /// 工作节点ID左移位数（12位）
    /// </summary>
    private const int WorkerIdShift = SequenceBits;

    /// <summary>
    /// 数据中心ID左移位数（17位）
    /// </summary>
    private const int DatacenterIdShift = SequenceBits + WorkerIdBits;

    /// <summary>
    /// 时间戳左移位数（22位）
    /// </summary>
    public const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

    /// <summary>
    /// 序列号掩码（4095），用于获取序列号的低12位
    /// </summary>
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);

    /// <summary>
    /// 当前序列号
    /// </summary>
    private long _sequence = 0L;

    /// <summary>
    /// 上次生成ID的时间戳
    /// </summary>
    private long _lastTimestamp = -1L;

    /// <summary>
    /// 初始化 <see cref="IdWorker"/> 类的新实例
    /// </summary>
    /// <param name="workerId">工作节点ID，取值范围为 0 到 31</param>
    /// <param name="dataCenterId">数据中心ID，取值范围为 0 到 31</param>
    /// <param name="sequence">初始序列号，默认为 0</param>
    /// <param name="baseTime">开始时间</param>
    /// <exception cref="ArgumentException">
    /// 当 <paramref name="workerId"/> 超出有效范围（0-31）时抛出
    /// </exception>
    /// <exception cref="ArgumentException">
    /// 当 <paramref name="dataCenterId"/> 超出有效范围（0-31）时抛出
    /// </exception>
    /// <remarks>
    /// 工作节点ID和数据中心ID的组合必须在分布式环境中保持唯一，
    /// 以确保生成的ID在整个系统中的唯一性。
    /// </remarks>
    /// <example>
    /// <code>
    /// // 创建工作节点ID为1，数据中心ID为1的ID生成器
    /// var idWorker = new IdWorker(1, 1);
    /// 
    /// // 创建带有初始序列号的ID生成器
    /// var idWorkerWithSequence = new IdWorker(2, 1, 100);
    /// </code>
    /// </example>
    public IdWorker(long workerId, long dataCenterId, long baseTime = 1288834974657L, long sequence = 0L)
    {
        WorkerId = workerId;
        DataCenterId = dataCenterId;
        BaseTime = baseTime;
        _sequence = sequence;

        // sanity check for workerId
        if (workerId > MaxWorkerId || workerId < 0)
        {
            throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");
        }

        if (dataCenterId > MaxDatacenterId || dataCenterId < 0)
        {
            throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
        }
    }

    /// <summary>
    /// 获取工作节点ID
    /// </summary>
    /// <value>
    /// 当前实例的工作节点ID
    /// </value>
    /// <remarks>
    /// 工作节点ID在实例创建后不可更改，用于标识不同的工作节点
    /// </remarks>
    public long WorkerId { get; protected set; }

    /// <summary>
    /// 获取数据中心ID
    /// </summary>
    /// <value>
    /// 当前实例的数据中心ID
    /// </value>
    /// <remarks>
    /// 数据中心ID在实例创建后不可更改，用于标识不同的数据中心
    /// </remarks>
    public long DataCenterId { get; protected set; }

    /// <summary>
    /// 起始时间
    /// </summary>
    public long BaseTime { get; }

    /// <summary>
    /// 获取或设置当前序列号
    /// </summary>
    /// <value>
    /// 当前的序列号值
    /// </value>
    /// <remarks>
    /// 序列号用于在同一毫秒内生成多个ID时进行区分，
    /// 内部设置器主要用于测试和特殊场景
    /// </remarks>
    public long Sequence
    {
        get { return _sequence; }
        internal set { _sequence = value; }
    }

    /// <summary>
    /// 用于线程同步的锁对象
    /// </summary>
    readonly object _lock = new Object();

    /// <summary>
    /// 生成下一个唯一ID
    /// </summary>
    /// <returns>生成的64位唯一ID</returns>
    /// <exception cref="InvalidSystemClock">
    /// 当系统时钟出现回退时抛出此异常
    /// </exception>
    /// <remarks>
    /// <para>
    /// 该方法是线程安全的，使用锁机制确保在多线程环境下的正确性。
    /// ID生成的逻辑如下：
    /// </para>
    /// <list type="number">
    /// <item><description>获取当前时间戳</description></item>
    /// <item><description>检查时钟是否回退，如果回退则抛出异常</description></item>
    /// <item><description>如果在同一毫秒内，序列号递增</description></item>
    /// <item><description>如果序列号溢出，等待下一毫秒</description></item>
    /// <item><description>组装最终的64位ID</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// var idWorker = new IdWorker(1, 1);
    /// 
    /// // 生成多个ID
    /// for (int i = 0; i &lt; 10; i++)
    /// {
    ///     long id = idWorker.NextId();
    ///     Console.WriteLine($"生成的ID: {id}");
    /// }
    /// </code>
    /// </example>
    public virtual long NextId()
    {
        lock (_lock)
        {
            var timestamp = TimeGenerator();

            if (timestamp < _lastTimestamp)
            {
                //exceptionCounter.incr(1);
                //log.Error("clock is moving backwards.  Rejecting requests until %d.", _lastTimestamp);
                throw new InvalidSystemClock($"服务器时间出现回退你可以使用StaticConfig.CustomSnowFlakeTimeErrorFunc=【自定义方法】处理让他不报错返回新ID,Clock moved backwards.  Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
            }

            if (_lastTimestamp == timestamp)
            {
                _sequence = (_sequence + 1) & SequenceMask;
                if (_sequence == 0)
                {
                    timestamp = WaitNextMillis(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;
            var id = ((timestamp - BaseTime) << TimestampLeftShift) |
                     (DataCenterId << DatacenterIdShift) |
                     (WorkerId << WorkerIdShift) | _sequence;

            return id;
        }
    }

    /// <summary>
    /// 等待直到下一毫秒
    /// </summary>
    /// <param name="lastTimestamp">上次生成ID的时间戳</param>
    /// <returns>下一毫秒的时间戳</returns>
    /// <remarks>
    /// 当序列号在当前毫秒内用尽时，该方法会阻塞等待直到下一毫秒，
    /// 以确保生成的ID保持时间有序性
    /// </remarks>
    protected virtual long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGenerator();
        while (timestamp <= lastTimestamp)
        {
            timestamp = TimeGenerator();
        }

        return timestamp;
    }

    /// <summary>
    /// 获取当前时间戳（毫秒）
    /// </summary>
    /// <returns>当前时间戳，以毫秒为单位</returns>
    /// <remarks>
    /// 该方法调用 <see cref="TimeSystem.CurrentTimeMillis"/> 来获取当前时间戳，
    /// 可以通过 <see cref="TimeSystem.StubCurrentTime(Func{long})"/> 方法在测试中进行模拟
    /// </remarks>
    /// <seealso cref="TimeSystem.CurrentTimeMillis"/>
    protected virtual long TimeGenerator()
    {
        return TimeSystem.CurrentTimeMillis();
    }
}