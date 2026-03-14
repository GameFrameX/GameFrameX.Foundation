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