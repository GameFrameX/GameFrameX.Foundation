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

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 分区表特性，用于标记实体类对应的数据库表支持分区存储。
/// </summary>
/// <remarks>
/// Partition table attribute for marking entity classes whose corresponding database tables support partitioning.
/// Partitioned tables can divide large tables into multiple smaller, more manageable parts according to specified rules,
/// thereby improving query performance, simplifying maintenance operations, and optimizing storage management.
/// <para>
/// Advantages of partitioned tables:
/// </para>
/// <list type="bullet">
/// <item><description>Improved query performance: Queries only scan relevant partitions</description></item>
/// <item><description>Simplified maintenance: Maintenance operations can be performed on individual partitions</description></item>
/// <item><description>Parallel processing: Supports parallel queries and operations</description></item>
/// <item><description>Storage optimization: Different partitions can be stored on different storage devices</description></item>
/// <item><description>Data management: Facilitates data archiving and cleanup</description></item>
/// </list>
/// <para>
/// Common partitioning strategies:
/// </para>
/// <list type="bullet">
/// <item><description>Range partitioning: Partitions by numeric or date ranges</description></item>
/// <item><description>List partitioning: Partitions by predefined value lists</description></item>
/// <item><description>Hash partitioning: Partitions by hash values</description></item>
/// <item><description>Composite partitioning: Combines multiple partitioning strategies</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [PartitionTable("CreateDate", PartitionType.Range, PartitionInterval.Monthly)]
/// public class OrderHistory
/// {
///     public int Id { get; set; }
///     public int UserId { get; set; }
///     public decimal Amount { get; set; }
///     public DateTime CreateDate { get; set; }  // Partition key
/// }
///
/// [PartitionTable("Region", PartitionType.List)]
/// public class CustomerData
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public string Region { get; set; }  // Partition key: North, South, East, West
/// }
///
/// [PartitionTable("UserId", PartitionType.Hash, PartitionCount = 16)]
/// public class UserActivity
/// {
///     public long Id { get; set; }
///     public int UserId { get; set; }  // Partition key
///     public string Activity { get; set; }
///     public DateTime Timestamp { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class PartitionTableAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置分区键字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the partition key field name.
    /// </remarks>
    /// <value>用于分区的字段名称 / Field name used for partitioning</value>
    public string PartitionKey { get; set; }

    /// <summary>
    /// 获取或设置分区类型。
    /// </summary>
    /// <remarks>
    /// Gets or sets the partition type strategy.
    /// </remarks>
    /// <value>分区的类型策略 / Partition type strategy</value>
    public PartitionType PartitionType { get; set; }

    /// <summary>
    /// 获取或设置分区间隔。
    /// </summary>
    /// <remarks>
    /// Gets or sets the partition interval. For range partitioning, specifies the time interval.
    /// </remarks>
    /// <value>对于范围分区，指定分区的时间间隔 / For range partitioning, specifies the time interval</value>
    public PartitionInterval Interval { get; set; } = PartitionInterval.Monthly;

    /// <summary>
    /// 获取或设置分区数量。
    /// </summary>
    /// <remarks>
    /// Gets or sets the partition count. For hash partitioning, specifies the number of partitions.
    /// </remarks>
    /// <value>对于哈希分区，指定分区的数量 / For hash partitioning, specifies the number of partitions</value>
    public int PartitionCount { get; set; } = 4;

    /// <summary>
    /// 获取或设置分区值列表。
    /// </summary>
    /// <remarks>
    /// Gets or sets the partition value list. For list partitioning, specifies the comma-separated value list.
    /// </remarks>
    /// <value>对于列表分区，指定分区的值列表，用逗号分隔 / For list partitioning, comma-separated value list</value>
    public string? PartitionValues { get; set; }

    /// <summary>
    /// 获取或设置是否自动创建分区。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to automatically create new partitions.
    /// </remarks>
    /// <value>指示是否自动创建新的分区，默认为 <c>true</c> / Indicates whether to automatically create new partitions, default is <c>true</c></value>
    public bool AutoCreatePartition { get; set; } = true;

    /// <summary>
    /// 获取或设置分区保留期限（天）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the partition retention period in days. Partitions exceeding this period may be automatically cleaned up. 0 means permanent retention.
    /// </remarks>
    /// <value>分区数据的保留天数，超过期限的分区可能被自动清理，0表示永久保留 / Partition data retention days, partitions exceeding period may be auto-cleaned, 0 means permanent</value>
    public int RetentionDays { get; set; } = 0;

    /// <summary>
    /// 初始化 <see cref="PartitionTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PartitionTableAttribute"/> class with the specified partition key and type.
    /// </remarks>
    /// <param name="partitionKey">分区键字段名称 / Partition key field name</param>
    /// <param name="partitionType">分区类型 / Partition type</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="partitionKey"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="partitionKey"/> is <c>null</c></exception>
    public PartitionTableAttribute(string partitionKey, PartitionType partitionType)
    {
        PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        PartitionType = partitionType;
    }

    /// <summary>
    /// 初始化 <see cref="PartitionTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PartitionTableAttribute"/> class with the specified partition key, type, and interval.
    /// </remarks>
    /// <param name="partitionKey">分区键字段名称 / Partition key field name</param>
    /// <param name="partitionType">分区类型 / Partition type</param>
    /// <param name="interval">分区间隔（用于范围分区） / Partition interval (for range partitioning)</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="partitionKey"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="partitionKey"/> is <c>null</c></exception>
    public PartitionTableAttribute(string partitionKey, PartitionType partitionType, PartitionInterval interval)
    {
        PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        PartitionType = partitionType;
        Interval = interval;
    }

    /// <summary>
    /// 初始化 <see cref="PartitionTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="PartitionTableAttribute"/> class with the specified partition key, type, and count.
    /// </remarks>
    /// <param name="partitionKey">分区键字段名称 / Partition key field name</param>
    /// <param name="partitionType">分区类型 / Partition type</param>
    /// <param name="partitionCount">分区数量（用于哈希分区） / Partition count (for hash partitioning)</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="partitionKey"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="partitionKey"/> is <c>null</c></exception>
    public PartitionTableAttribute(string partitionKey, PartitionType partitionType, int partitionCount)
    {
        PartitionKey = partitionKey ?? throw new ArgumentNullException(nameof(partitionKey));
        PartitionType = partitionType;
        PartitionCount = partitionCount;
    }
}

/// <summary>
/// 分区类型枚举。
/// </summary>
/// <remarks>
/// Partition type enumeration.
/// </remarks>
public enum PartitionType
{
    /// <summary>
    /// 范围分区：按数值或日期范围分区。
    /// </summary>
    /// <remarks>
    /// Range partitioning: Partitions by numeric or date ranges.
    /// </remarks>
    Range = 0,

    /// <summary>
    /// 列表分区：按预定义的值列表分区。
    /// </summary>
    /// <remarks>
    /// List partitioning: Partitions by predefined value lists.
    /// </remarks>
    List = 1,

    /// <summary>
    /// 哈希分区：按哈希值均匀分区。
    /// </summary>
    /// <remarks>
    /// Hash partitioning: Partitions evenly by hash values.
    /// </remarks>
    Hash = 2,

    /// <summary>
    /// 复合分区：组合多种分区策略。
    /// </summary>
    /// <remarks>
    /// Composite partitioning: Combines multiple partitioning strategies.
    /// </remarks>
    Composite = 3
}

/// <summary>
/// 分区间隔枚举（用于范围分区）。
/// </summary>
/// <remarks>
/// Partition interval enumeration (for range partitioning).
/// </remarks>
public enum PartitionInterval
{
    /// <summary>
    /// 按天分区。
    /// </summary>
    /// <remarks>
    /// Daily partitioning.
    /// </remarks>
    Daily = 0,

    /// <summary>
    /// 按周分区。
    /// </summary>
    /// <remarks>
    /// Weekly partitioning.
    /// </remarks>
    Weekly = 1,

    /// <summary>
    /// 按月分区。
    /// </summary>
    /// <remarks>
    /// Monthly partitioning.
    /// </remarks>
    Monthly = 2,

    /// <summary>
    /// 按季度分区。
    /// </summary>
    /// <remarks>
    /// Quarterly partitioning.
    /// </remarks>
    Quarterly = 3,

    /// <summary>
    /// 按年分区。
    /// </summary>
    /// <remarks>
    /// Yearly partitioning.
    /// </remarks>
    Yearly = 4,

    /// <summary>
    /// 自定义间隔。
    /// </summary>
    /// <remarks>
    /// Custom interval.
    /// </remarks>
    Custom = 5
}
