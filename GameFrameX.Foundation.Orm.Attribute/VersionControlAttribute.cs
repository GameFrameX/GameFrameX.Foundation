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
/// 版本控制特性，用于标记实体类支持数据版本管理功能
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体支持版本控制功能。
/// 版本控制可以跟踪数据的变更历史，支持数据回滚、变更对比、并发控制等功能。
/// 在多用户并发环境中，版本控制是防止数据冲突和保证数据一致性的重要机制。
/// 
/// 版本控制的优势：
/// - 并发控制：防止多用户同时修改数据造成的冲突
/// - 变更跟踪：记录数据的完整变更历史
/// - 数据回滚：支持将数据恢复到历史版本
/// - 冲突检测：自动检测并处理数据修改冲突
/// - 审计支持：提供详细的数据变更审计信息
/// 
/// 常见版本控制策略：
/// - 乐观锁：使用版本号或时间戳检测冲突
/// - 悲观锁：在修改前锁定数据
/// - 快照隔离：为每个事务创建数据快照
/// - 增量版本：只记录变更的字段
/// </remarks>
/// <example>
/// <code>
/// [VersionControl("Version", VersionStrategy.Optimistic)]
/// public class Document
/// {
///     public int Id { get; set; }
///     public string Title { get; set; }
///     public string Content { get; set; }
///     public int Version { get; set; }  // 版本号字段
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
///     public byte[] RowVersion { get; set; }  // 时间戳版本字段
/// }
/// 
/// [VersionControl("ModifiedTime", VersionStrategy.LastModified, MaxHistoryVersions = 10)]
/// public class ProductInfo
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public decimal Price { get; set; }
///     public DateTime ModifiedTime { get; set; }  // 最后修改时间
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class VersionControlAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置版本字段名称
    /// </summary>
    /// <value>用于版本控制的字段名称</value>
    public string VersionField { get; set; }

    /// <summary>
    /// 获取或设置版本控制策略
    /// </summary>
    /// <value>版本控制的策略类型</value>
    public VersionStrategy Strategy { get; set; }

    /// <summary>
    /// 获取或设置是否启用历史版本记录
    /// </summary>
    /// <value>指示是否保存历史版本数据，默认为 false</value>
    public bool EnableHistory { get; set; } = false;

    /// <summary>
    /// 获取或设置历史版本表名称
    /// </summary>
    /// <value>存储历史版本的表名称，如果为空则使用默认命名规则</value>
    public string? HistoryTableName { get; set; }

    /// <summary>
    /// 获取或设置最大历史版本数量
    /// </summary>
    /// <value>保留的最大历史版本数量，0表示不限制，默认为0</value>
    public int MaxHistoryVersions { get; set; } = 0;

    /// <summary>
    /// 获取或设置冲突处理策略
    /// </summary>
    /// <value>当检测到版本冲突时的处理策略</value>
    public ConflictResolution ConflictHandling { get; set; } = ConflictResolution.ThrowException;

    /// <summary>
    /// 获取或设置是否自动更新版本
    /// </summary>
    /// <value>指示是否在数据更新时自动递增版本号，默认为 true</value>
    public bool AutoUpdateVersion { get; set; } = true;

    /// <summary>
    /// 获取或设置版本比较模式
    /// </summary>
    /// <value>版本比较的模式，用于确定版本的新旧关系</value>
    public VersionComparison ComparisonMode { get; set; } = VersionComparison.Increment;

    /// <summary>
    /// 初始化 <see cref="VersionControlAttribute"/> 类的新实例
    /// </summary>
    /// <param name="versionField">版本字段名称</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="versionField"/> 为 null 时抛出</exception>
    public VersionControlAttribute(string versionField)
    {
        VersionField = versionField ?? throw new ArgumentNullException(nameof(versionField));
        Strategy = VersionStrategy.Optimistic;
    }

    /// <summary>
    /// 初始化 <see cref="VersionControlAttribute"/> 类的新实例
    /// </summary>
    /// <param name="versionField">版本字段名称</param>
    /// <param name="strategy">版本控制策略</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="versionField"/> 为 null 时抛出</exception>
    public VersionControlAttribute(string versionField, VersionStrategy strategy)
    {
        VersionField = versionField ?? throw new ArgumentNullException(nameof(versionField));
        Strategy = strategy;
    }
}

/// <summary>
/// 版本控制策略枚举
/// </summary>
public enum VersionStrategy
{
    /// <summary>
    /// 乐观锁：使用版本号进行冲突检测
    /// </summary>
    Optimistic = 0,

    /// <summary>
    /// 悲观锁：在修改前锁定数据
    /// </summary>
    Pessimistic = 1,

    /// <summary>
    /// 时间戳：使用时间戳进行版本控制
    /// </summary>
    Timestamp = 2,

    /// <summary>
    /// 最后修改时间：基于最后修改时间的版本控制
    /// </summary>
    LastModified = 3,

    /// <summary>
    /// 哈希值：基于数据哈希值的版本控制
    /// </summary>
    Hash = 4,

    /// <summary>
    /// 自定义：使用自定义的版本控制逻辑
    /// </summary>
    Custom = 5
}

/// <summary>
/// 冲突解决策略枚举
/// </summary>
public enum ConflictResolution
{
    /// <summary>
    /// 抛出异常
    /// </summary>
    ThrowException = 0,

    /// <summary>
    /// 自动重试
    /// </summary>
    AutoRetry = 1,

    /// <summary>
    /// 强制覆盖
    /// </summary>
    ForceOverwrite = 2,

    /// <summary>
    /// 合并变更
    /// </summary>
    MergeChanges = 3,

    /// <summary>
    /// 用户选择
    /// </summary>
    UserChoice = 4,

    /// <summary>
    /// 自定义处理
    /// </summary>
    Custom = 5
}

/// <summary>
/// 版本比较模式枚举
/// </summary>
public enum VersionComparison
{
    /// <summary>
    /// 递增数值：版本号递增比较
    /// </summary>
    Increment = 0,

    /// <summary>
    /// 时间戳：基于时间戳比较
    /// </summary>
    Timestamp = 1,

    /// <summary>
    /// 字符串：基于字符串比较
    /// </summary>
    String = 2,

    /// <summary>
    /// 自定义：使用自定义比较逻辑
    /// </summary>
    Custom = 3
}