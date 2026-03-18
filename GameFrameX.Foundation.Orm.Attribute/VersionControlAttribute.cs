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
/// 版本控制特性，用于标记实体类支持数据版本管理功能。
/// </summary>
/// <remarks>
/// Version control attribute for marking entity classes that support data version management functionality.
/// Version control can track data change history and support features such as data rollback, change comparison, and concurrency control.
/// In multi-user concurrent environments, version control is an important mechanism for preventing data conflicts and ensuring data consistency.
/// <para>
/// Advantages of version control:
/// </para>
/// <list type="bullet">
/// <item><description>Concurrency control: Prevents conflicts caused by multiple users modifying data simultaneously</description></item>
/// <item><description>Change tracking: Records complete data change history</description></item>
/// <item><description>Data rollback: Supports restoring data to historical versions</description></item>
/// <item><description>Conflict detection: Automatically detects and handles data modification conflicts</description></item>
/// <item><description>Audit support: Provides detailed data change audit information</description></item>
/// </list>
/// <para>
/// Common version control strategies:
/// </para>
/// <list type="bullet">
/// <item><description>Optimistic locking: Uses version numbers or timestamps for conflict detection</description></item>
/// <item><description>Pessimistic locking: Locks data before modification</description></item>
/// <item><description>Snapshot isolation: Creates data snapshots for each transaction</description></item>
/// <item><description>Incremental versioning: Only records changed fields</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [VersionControl("Version", VersionStrategy.Optimistic)]
/// public class Document
/// {
///     public int Id { get; set; }
///     public string Title { get; set; }
///     public string Content { get; set; }
///     public int Version { get; set; }  // Version number field
///     public DateTime LastModified { get; set; }
///     public string LastModifiedBy { get; set; }
/// }
///
/// [VersionControl("RowVersion", VersionStrategy.Timestamp, EnableHistory = true)]
/// public class UserProfile
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public string Email { get; set; }
///     public byte[] RowVersion { get; set; }  // Timestamp version field
/// }
///
/// [VersionControl("ModifiedTime", VersionStrategy.LastModified, MaxHistoryVersions = 10)]
/// public class ProductInfo
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public decimal Price { get; set; }
///     public DateTime ModifiedTime { get; set; }  // Last modified time
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class VersionControlAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置版本字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the version field name.
    /// </remarks>
    /// <value>用于版本控制的字段名称 / Field name used for version control</value>
    public string VersionField { get; set; }

    /// <summary>
    /// 获取或设置版本控制策略。
    /// </summary>
    /// <remarks>
    /// Gets or sets the version control strategy type.
    /// </remarks>
    /// <value>版本控制的策略类型 / Version control strategy type</value>
    public VersionStrategy Strategy { get; set; }

    /// <summary>
    /// 获取或设置是否启用历史版本记录。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to enable historical version recording.
    /// </remarks>
    /// <value>指示是否保存历史版本数据，默认为 <c>false</c> / Indicates whether to save historical version data, default is <c>false</c></value>
    public bool EnableHistory { get; set; } = false;

    /// <summary>
    /// 获取或设置历史版本表名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the history table name. If null, the default naming convention is used.
    /// </remarks>
    /// <value>存储历史版本的表名称，如果为空则使用默认命名规则 / Table name for storing historical versions, uses default naming convention if null</value>
    public string? HistoryTableName { get; set; }

    /// <summary>
    /// 获取或设置最大历史版本数量。
    /// </summary>
    /// <remarks>
    /// Gets or sets the maximum number of historical versions to retain. 0 means unlimited.
    /// </remarks>
    /// <value>保留的最大历史版本数量，0表示不限制，默认为0 / Maximum number of historical versions to retain, 0 means unlimited, default is 0</value>
    public int MaxHistoryVersions { get; set; } = 0;

    /// <summary>
    /// 获取或设置冲突处理策略。
    /// </summary>
    /// <remarks>
    /// Gets or sets the conflict resolution strategy when version conflicts are detected.
    /// </remarks>
    /// <value>当检测到版本冲突时的处理策略 / Conflict resolution strategy when version conflicts are detected</value>
    public ConflictResolution ConflictHandling { get; set; } = ConflictResolution.ThrowException;

    /// <summary>
    /// 获取或设置是否自动更新版本。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to automatically increment the version number on data updates.
    /// </remarks>
    /// <value>指示是否在数据更新时自动递增版本号，默认为 <c>true</c> / Indicates whether to automatically increment version number on data updates, default is <c>true</c></value>
    public bool AutoUpdateVersion { get; set; } = true;

    /// <summary>
    /// 获取或设置版本比较模式。
    /// </summary>
    /// <remarks>
    /// Gets or sets the version comparison mode for determining version precedence.
    /// </remarks>
    /// <value>版本比较的模式，用于确定版本的新旧关系 / Version comparison mode for determining version precedence</value>
    public VersionComparison ComparisonMode { get; set; } = VersionComparison.Increment;

    /// <summary>
    /// 初始化 <see cref="VersionControlAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VersionControlAttribute"/> class with the specified version field.
    /// </remarks>
    /// <param name="versionField">版本字段名称 / Version field name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="versionField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="versionField"/> is <c>null</c></exception>
    public VersionControlAttribute(string versionField)
    {
        VersionField = versionField ?? throw new ArgumentNullException(nameof(versionField));
        Strategy = VersionStrategy.Optimistic;
    }

    /// <summary>
    /// 初始化 <see cref="VersionControlAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VersionControlAttribute"/> class with the specified version field and strategy.
    /// </remarks>
    /// <param name="versionField">版本字段名称 / Version field name</param>
    /// <param name="strategy">版本控制策略 / Version control strategy</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="versionField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="versionField"/> is <c>null</c></exception>
    public VersionControlAttribute(string versionField, VersionStrategy strategy)
    {
        VersionField = versionField ?? throw new ArgumentNullException(nameof(versionField));
        Strategy = strategy;
    }
}

/// <summary>
/// 版本控制策略枚举。
/// </summary>
/// <remarks>
/// Version control strategy enumeration.
/// </remarks>
public enum VersionStrategy
{
    /// <summary>
    /// 乐观锁：使用版本号进行冲突检测。
    /// </summary>
    /// <remarks>
    /// Optimistic locking: Uses version numbers for conflict detection.
    /// </remarks>
    Optimistic = 0,

    /// <summary>
    /// 悲观锁：在修改前锁定数据。
    /// </summary>
    /// <remarks>
    /// Pessimistic locking: Locks data before modification.
    /// </remarks>
    Pessimistic = 1,

    /// <summary>
    /// 时间戳：使用时间戳进行版本控制。
    /// </summary>
    /// <remarks>
    /// Timestamp: Uses timestamps for version control.
    /// </remarks>
    Timestamp = 2,

    /// <summary>
    /// 最后修改时间：基于最后修改时间的版本控制。
    /// </summary>
    /// <remarks>
    /// Last modified: Version control based on last modified time.
    /// </remarks>
    LastModified = 3,

    /// <summary>
    /// 哈希值：基于数据哈希值的版本控制。
    /// </summary>
    /// <remarks>
    /// Hash: Version control based on data hash values.
    /// </remarks>
    Hash = 4,

    /// <summary>
    /// 自定义：使用自定义的版本控制逻辑。
    /// </summary>
    /// <remarks>
    /// Custom: Uses custom version control logic.
    /// </remarks>
    Custom = 5
}

/// <summary>
/// 冲突解决策略枚举。
/// </summary>
/// <remarks>
/// Conflict resolution strategy enumeration.
/// </remarks>
public enum ConflictResolution
{
    /// <summary>
    /// 抛出异常。
    /// </summary>
    /// <remarks>
    /// Throw exception.
    /// </remarks>
    ThrowException = 0,

    /// <summary>
    /// 自动重试。
    /// </summary>
    /// <remarks>
    /// Auto retry.
    /// </remarks>
    AutoRetry = 1,

    /// <summary>
    /// 强制覆盖。
    /// </summary>
    /// <remarks>
    /// Force overwrite.
    /// </remarks>
    ForceOverwrite = 2,

    /// <summary>
    /// 合并变更。
    /// </summary>
    /// <remarks>
    /// Merge changes.
    /// </remarks>
    MergeChanges = 3,

    /// <summary>
    /// 用户选择。
    /// </summary>
    /// <remarks>
    /// User choice.
    /// </remarks>
    UserChoice = 4,

    /// <summary>
    /// 自定义处理。
    /// </summary>
    /// <remarks>
    /// Custom handling.
    /// </remarks>
    Custom = 5
}

/// <summary>
/// 版本比较模式枚举。
/// </summary>
/// <remarks>
/// Version comparison mode enumeration.
/// </remarks>
public enum VersionComparison
{
    /// <summary>
    /// 递增数值：版本号递增比较。
    /// </summary>
    /// <remarks>
    /// Increment: Version number increment comparison.
    /// </remarks>
    Increment = 0,

    /// <summary>
    /// 时间戳：基于时间戳比较。
    /// </summary>
    /// <remarks>
    /// Timestamp: Timestamp-based comparison.
    /// </remarks>
    Timestamp = 1,

    /// <summary>
    /// 字符串：基于字符串比较。
    /// </summary>
    /// <remarks>
    /// String: String-based comparison.
    /// </remarks>
    String = 2,

    /// <summary>
    /// 自定义：使用自定义比较逻辑。
    /// </summary>
    /// <remarks>
    /// Custom: Uses custom comparison logic.
    /// </remarks>
    Custom = 3
}
