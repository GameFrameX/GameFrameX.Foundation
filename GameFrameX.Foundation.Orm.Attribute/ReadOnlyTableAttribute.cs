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
/// 只读表特性，用于标记实体类对应的数据库表为只读表。
/// </summary>
/// <remarks>
/// Read-only table attribute for marking entity classes whose corresponding database tables are read-only.
/// When an entity class is marked with this attribute, the ORM framework disables write operations (INSERT, UPDATE, DELETE) on the table,
/// and can enable corresponding query optimization strategies such as query caching and read-write splitting.
/// <para>
/// Read-only tables are typically used in the following scenarios:
/// </para>
/// <list type="bullet">
/// <item><description>Static configuration data tables</description></item>
/// <item><description>Historical data archive tables</description></item>
/// <item><description>Data warehouse dimension tables</description></item>
/// <item><description>Report and statistics data tables</description></item>
/// <item><description>Data views from third-party systems</description></item>
/// <item><description>Basic data dictionary tables</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [ReadOnlyTable(EnableCache = true, CacheMinutes = 60)]
/// public class CountryCode
/// {
///     public string Code { get; set; }
///     public string Name { get; set; }
///     public string Region { get; set; }
/// }
///
/// [ReadOnlyTable(AllowRefresh = true)]
/// public class StaticConfiguration
/// {
///     public string Key { get; set; }
///     public string Value { get; set; }
///     public string Description { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ReadOnlyTableAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置是否启用缓存。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to enable caching for the read-only table.
    /// </remarks>
    /// <value>指示是否对只读表启用查询缓存，默认为 <c>true</c> / Indicates whether to enable query caching for read-only table, default is <c>true</c></value>
    public bool EnableCache { get; set; } = true;

    /// <summary>
    /// 获取或设置缓存时间（分钟）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the cache duration in minutes for query results.
    /// </remarks>
    /// <value>查询结果的缓存时间，单位为分钟，默认为60分钟 / Cache duration for query results in minutes, default is 60 minutes</value>
    public int CacheMinutes { get; set; } = 60;

    /// <summary>
    /// 获取或设置是否允许刷新数据。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to allow refreshing read-only table data through special operations.
    /// </remarks>
    /// <value>指示是否允许通过特殊操作刷新只读表数据，默认为 <c>false</c> / Indicates whether to allow refreshing read-only table data through special operations, default is <c>false</c></value>
    public bool AllowRefresh { get; set; } = false;

    /// <summary>
    /// 获取或设置错误处理策略。
    /// </summary>
    /// <remarks>
    /// Gets or sets the error handling strategy when attempting write operations on read-only tables.
    /// </remarks>
    /// <value>当尝试对只读表进行写操作时的错误处理策略 / Error handling strategy when attempting write operations on read-only tables</value>
    public ReadOnlyErrorHandling ErrorHandling { get; set; } = ReadOnlyErrorHandling.ThrowException;

    /// <summary>
    /// 获取或设置自定义错误消息。
    /// </summary>
    /// <remarks>
    /// Gets or sets the custom error message displayed when read-only constraint is violated.
    /// </remarks>
    /// <value>当违反只读约束时显示的自定义错误消息 / Custom error message displayed when read-only constraint is violated</value>
    public string? CustomErrorMessage { get; set; }

    /// <summary>
    /// 初始化 <see cref="ReadOnlyTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReadOnlyTableAttribute"/> class.
    /// </remarks>
    public ReadOnlyTableAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="ReadOnlyTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ReadOnlyTableAttribute"/> class with the specified cache settings.
    /// </remarks>
    /// <param name="enableCache">是否启用缓存 / Whether to enable caching</param>
    /// <param name="cacheMinutes">缓存时间（分钟） / Cache duration in minutes</param>
    public ReadOnlyTableAttribute(bool enableCache, int cacheMinutes = 60)
    {
        EnableCache = enableCache;
        CacheMinutes = cacheMinutes;
    }
}

/// <summary>
/// 只读表错误处理策略枚举。
/// </summary>
/// <remarks>
/// Read-only table error handling strategy enumeration.
/// </remarks>
public enum ReadOnlyErrorHandling
{
    /// <summary>
    /// 抛出异常。
    /// </summary>
    /// <remarks>
    /// Throw exception.
    /// </remarks>
    ThrowException = 0,

    /// <summary>
    /// 静默忽略。
    /// </summary>
    /// <remarks>
    /// Silently ignore.
    /// </remarks>
    SilentIgnore = 1,

    /// <summary>
    /// 记录警告日志。
    /// </summary>
    /// <remarks>
    /// Log warning.
    /// </remarks>
    LogWarning = 2,

    /// <summary>
    /// 自定义处理。
    /// </summary>
    /// <remarks>
    /// Custom handling.
    /// </remarks>
    Custom = 3
}
