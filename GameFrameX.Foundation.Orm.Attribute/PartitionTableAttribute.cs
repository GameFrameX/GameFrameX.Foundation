// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 分区表特性，用于标记实体类对应的数据库表支持分区存储
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表支持分区功能。
/// 分区表可以将大表按照指定规则分割成多个较小的、更易管理的部分，
/// 从而提高查询性能、简化维护操作、优化存储管理。
/// 
/// 分区表的优势：
/// - 提高查询性能：查询时只需扫描相关分区
/// - 简化维护：可以对单个分区进行维护操作
/// - 并行处理：支持并行查询和操作
/// - 存储优化：可以将不同分区存储在不同的存储设备上
/// - 数据管理：便于数据归档和清理
/// 
/// 常见分区策略：
/// - 范围分区：按数值或日期范围分区
/// - 列表分区：按预定义的值列表分区
/// - 哈希分区：按哈希值分区
/// - 复合分区：组合多种分区策略
/// </remarks>
/// <example>
/// <code>
/// [PartitionTable("CreateDate", PartitionType.Range, PartitionInterval.Monthly)]
/// public class OrderHistory
/// {
///     public int Id { get; set; }
///     public int UserId { get; set; }
///     public decimal Amount { get; set; }
///     public DateTime CreateDate { get; set; }  // 分区键
/// }
/// 
/// [PartitionTable("Region", PartitionType.List)]
/// public class CustomerData
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public string Region { get; set; }  // 分区键：North, South, East, West
/// }
/// 
/// [PartitionTable("UserId", PartitionType.Hash, PartitionCount = 16)]
/// public class UserActivity
/// {
///     public long Id { get; set; }
///     public int UserId { get; set; }  // 分区键
///     public string Activity { get; set; }
///     public DateTime Timestamp { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PartitionTableAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置分区键字段名称
    /// </summary>
    /// <value>用于分区的字段名称</value>
    public string PartitionKey { get; set; }

    /// <summary>
    /// 获取或设置分区类型
    /// </summary>
    /// <value>分区的类型策略</value>
    public PartitionType PartitionType { get; set; }

    /// <summary>
    /// 获取或设置分区间隔
    /// </summary>
    /// <value>对于范围分区，指定分区的时间间隔</value>
    public PartitionInterval Interval { get; set; } = PartitionInterval.Monthly;

    /// <summary>
    /// 获取或设置分区数量
    /// </summary>
    /// <value>对于哈希分区，指定分区的数量</value>
    public int PartitionCount { get; set; } = 4;

    /// <summary>
    /// 获取或设置分区值列表
    /// </summary>
    /// <value>对于列表分区，指定分区的值列表，用逗号分隔</value>
    public string? PartitionValues { get; set; }

    /// <summary>
    /// 获取或设置是否自动创建分区
    /// </summary>
    /// <value>指示是否自动创建新的分区，默认为 true</value>
    public bool AutoCreatePartition { get; set; } = true;

    /// <summary>
    /// 获取或设置分区保留期限（天）
    /// </summary>
    /// <value>分区数据的保留天数，超过期限的分区可能被自动清理，0表示永久保留</value>
    public int RetentionDays { get; set; } = 0;

    /// <summary>
    /// 初始化 <see cref="PartitionTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="partitionKey">分区键字段名称</param>
    /// <param name="partitionType">分区类型</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="partitionKey"/> 为 null 时抛出</exception>
    public PartitionTableAttribute(string partitionKey, PartitionType partitionType)
    {
        PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        PartitionType = partitionType;
    }

    /// <summary>
    /// 初始化 <see cref="PartitionTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="partitionKey">分区键字段名称</param>
    /// <param name="partitionType">分区类型</param>
    /// <param name="interval">分区间隔（用于范围分区）</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="partitionKey"/> 为 null 时抛出</exception>
    public PartitionTableAttribute(string partitionKey, PartitionType partitionType, PartitionInterval interval)
    {
        PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        PartitionType = partitionType;
        Interval = interval;
    }

    /// <summary>
    /// 初始化 <see cref="PartitionTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="partitionKey">分区键字段名称</param>
    /// <param name="partitionType">分区类型</param>
    /// <param name="partitionCount">分区数量（用于哈希分区）</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="partitionKey"/> 为 null 时抛出</exception>
    public PartitionTableAttribute(string partitionKey, PartitionType partitionType, int partitionCount)
    {
        PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        PartitionType = partitionType;
        PartitionCount = partitionCount;
    }
}

/// <summary>
/// 分区类型枚举
/// </summary>
public enum PartitionType
{
    /// <summary>
    /// 范围分区：按数值或日期范围分区
    /// </summary>
    Range = 0,

    /// <summary>
    /// 列表分区：按预定义的值列表分区
    /// </summary>
    List = 1,

    /// <summary>
    /// 哈希分区：按哈希值均匀分区
    /// </summary>
    Hash = 2,

    /// <summary>
    /// 复合分区：组合多种分区策略
    /// </summary>
    Composite = 3
}

/// <summary>
/// 分区间隔枚举（用于范围分区）
/// </summary>
public enum PartitionInterval
{
    /// <summary>
    /// 按天分区
    /// </summary>
    Daily = 0,

    /// <summary>
    /// 按周分区
    /// </summary>
    Weekly = 1,

    /// <summary>
    /// 按月分区
    /// </summary>
    Monthly = 2,

    /// <summary>
    /// 按季度分区
    /// </summary>
    Quarterly = 3,

    /// <summary>
    /// 按年分区
    /// </summary>
    Yearly = 4,

    /// <summary>
    /// 自定义间隔
    /// </summary>
    Custom = 5
}