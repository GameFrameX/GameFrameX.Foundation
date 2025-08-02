// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 只读表特性，用于标记实体类对应的数据库表为只读表
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表是只读的。
/// 在ORM框架中，当实体类标记了此特性时，框架会禁用对该表的写入操作（INSERT、UPDATE、DELETE），
/// 并可以启用相应的查询优化策略，如查询缓存、读写分离等。
/// 
/// 只读表通常用于以下场景：
/// - 静态配置数据表
/// - 历史数据归档表
/// - 数据仓库的维度表
/// - 报表和统计数据表
/// - 第三方系统的数据视图
/// - 基础数据字典表
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
    /// 获取或设置是否启用缓存
    /// </summary>
    /// <value>指示是否对只读表启用查询缓存，默认为 true</value>
    public bool EnableCache { get; set; } = true;

    /// <summary>
    /// 获取或设置缓存时间（分钟）
    /// </summary>
    /// <value>查询结果的缓存时间，单位为分钟，默认为60分钟</value>
    public int CacheMinutes { get; set; } = 60;

    /// <summary>
    /// 获取或设置是否允许刷新数据
    /// </summary>
    /// <value>指示是否允许通过特殊操作刷新只读表数据，默认为 false</value>
    public bool AllowRefresh { get; set; } = false;

    /// <summary>
    /// 获取或设置错误处理策略
    /// </summary>
    /// <value>当尝试对只读表进行写操作时的错误处理策略</value>
    public ReadOnlyErrorHandling ErrorHandling { get; set; } = ReadOnlyErrorHandling.ThrowException;

    /// <summary>
    /// 获取或设置自定义错误消息
    /// </summary>
    /// <value>当违反只读约束时显示的自定义错误消息</value>
    public string? CustomErrorMessage { get; set; }

    /// <summary>
    /// 初始化 <see cref="ReadOnlyTableAttribute"/> 类的新实例
    /// </summary>
    public ReadOnlyTableAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="ReadOnlyTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="enableCache">是否启用缓存</param>
    /// <param name="cacheMinutes">缓存时间（分钟）</param>
    public ReadOnlyTableAttribute(bool enableCache, int cacheMinutes = 60)
    {
        EnableCache = enableCache;
        CacheMinutes = cacheMinutes;
    }
}

/// <summary>
/// 只读表错误处理策略枚举
/// </summary>
public enum ReadOnlyErrorHandling
{
    /// <summary>
    /// 抛出异常
    /// </summary>
    ThrowException = 0,

    /// <summary>
    /// 静默忽略
    /// </summary>
    SilentIgnore = 1,

    /// <summary>
    /// 记录警告日志
    /// </summary>
    LogWarning = 2,

    /// <summary>
    /// 自定义处理
    /// </summary>
    Custom = 3
}