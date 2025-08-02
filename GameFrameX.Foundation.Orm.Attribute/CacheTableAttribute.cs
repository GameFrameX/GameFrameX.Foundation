// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 缓存表特性，用于标记实体类对应的数据库表支持缓存策略
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表支持缓存功能。
/// 在ORM框架中，当实体类标记了此特性时，框架会启用相应的缓存策略，
/// 例如查询结果缓存、实体缓存、分布式缓存等，以提高数据访问性能。
/// 
/// 缓存表通常具有以下特征：
/// - 数据读取频率高，写入频率相对较低
/// - 数据相对稳定，不经常变化
/// - 对查询性能有较高要求
/// - 可以容忍一定程度的数据延迟
/// - 需要考虑缓存失效和更新策略
/// </remarks>
/// <example>
/// <code>
/// [CacheTable(CacheType = "Redis", ExpireMinutes = 30)]
/// public class ProductInfo
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public decimal Price { get; set; }
///     public DateTime LastModified { get; set; }
/// }
/// 
/// [CacheTable(CacheType = "Memory", ExpireMinutes = 10)]
/// public class SystemConfiguration
/// {
///     public string Key { get; set; }
///     public string Value { get; set; }
///     public bool IsActive { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class CacheTableAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置缓存类型
    /// </summary>
    /// <value>缓存类型，如 "Memory"、"Redis"、"Distributed" 等</value>
    public string CacheType { get; set; } = "Memory";

    /// <summary>
    /// 获取或设置缓存过期时间（分钟）
    /// </summary>
    /// <value>缓存过期时间，单位为分钟，默认为30分钟</value>
    public int ExpireMinutes { get; set; } = 30;

    /// <summary>
    /// 获取或设置缓存键前缀
    /// </summary>
    /// <value>缓存键的前缀，用于区分不同的缓存数据</value>
    public string? KeyPrefix { get; set; }

    /// <summary>
    /// 获取或设置是否启用缓存
    /// </summary>
    /// <value>指示是否启用缓存功能，默认为 true</value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 初始化 <see cref="CacheTableAttribute"/> 类的新实例
    /// </summary>
    public CacheTableAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="CacheTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="cacheType">缓存类型</param>
    /// <param name="expireMinutes">缓存过期时间（分钟）</param>
    public CacheTableAttribute(string cacheType, int expireMinutes = 30)
    {
        CacheType = cacheType ?? throw new ArgumentNullException(nameof(cacheType));
        ExpireMinutes = expireMinutes;
    }
}