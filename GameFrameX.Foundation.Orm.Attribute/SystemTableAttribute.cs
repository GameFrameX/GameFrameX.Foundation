// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 系统表特性，用于标记实体类对应的数据库表为系统表
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表是系统表。
/// 在ORM框架中，当实体类标记了此特性时，框架会启用系统表相关的功能，
/// 例如特殊的权限控制、系统级别的缓存策略、特殊的备份和恢复策略等。
/// 
/// 系统表通常具有以下特征：
/// - 存储系统核心配置和元数据
/// - 对数据一致性和完整性要求极高
/// - 通常需要特殊的访问权限控制
/// - 可能需要特殊的备份和恢复策略
/// - 数据变更需要严格的审计和监控
/// - 通常不允许普通用户直接操作
/// </remarks>
/// <example>
/// <code>
/// [SystemTable]
/// public class SystemConfiguration
/// {
///     public int Id { get; set; }
///     public string ConfigKey { get; set; }    // 配置键
///     public string ConfigValue { get; set; }  // 配置值
///     public string Description { get; set; }  // 配置描述
///     public bool IsActive { get; set; }       // 是否启用
///     public DateTime CreatedTime { get; set; }
///     public DateTime? ModifiedTime { get; set; }
/// }
/// 
/// [SystemTable]
/// public class SystemPermission
/// {
///     public int Id { get; set; }
///     public string PermissionCode { get; set; }  // 权限代码
///     public string PermissionName { get; set; }  // 权限名称
///     public string Module { get; set; }          // 所属模块
///     public int Level { get; set; }              // 权限级别
///     public bool IsSystemLevel { get; set; }     // 是否系统级权限
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SystemTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="SystemTableAttribute"/> 类的新实例
    /// </summary>
    public SystemTableAttribute()
    {
    }
}
