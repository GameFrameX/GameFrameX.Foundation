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
/// 租户表特性，用于标记实体类对应的数据库表为租户级别表（数据按租户隔离）。
/// </summary>
/// <remarks>
/// Tenant table attribute for marking entity classes whose corresponding database tables are tenant-level tables
/// (data is isolated per tenant).
/// When an entity class is marked with this attribute, the ORM framework enables tenant isolation features,
/// such as automatically injecting tenant filters on queries, forcing tenant identifiers on inserts,
/// and preventing cross-tenant access, so that each tenant can only see and operate on its own data.
/// <para>
/// Tenant tables typically have the following characteristics:
/// </para>
/// <list type="bullet">
/// <item><description>Each row belongs to a specific tenant via a tenant identifier field</description></item>
/// <item><description>Queries are automatically filtered by the current tenant context</description></item>
/// <item><description>Inserts are automatically stamped with the current tenant identifier</description></item>
/// <item><description>Cross-tenant access requires explicit elevation or super-admin scope</description></item>
/// <item><description>This is the tenant-scoped counterpart of <see cref="SystemTableAttribute"/>, which marks globally shared tables</description></item>
/// </list>
/// <para>
/// Relationship with <see cref="SystemTableAttribute"/>:
/// A table should be marked with exactly one of the two. <see cref="SystemTableAttribute"/> marks global,
/// tenant-agnostic data (super-admin scope), while this attribute marks tenant-isolated business data.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// [TenantTable]
/// public class GameOrder
/// {
///     public long Id { get; set; }
///     public long TenantId { get; set; }   // Tenant identifier
///     public string OrderNo { get; set; }  // Order number
///     public decimal Amount { get; set; }
///     public DateTime CreatedTime { get; set; }
/// }
///
/// [TenantTable]
/// public class GameArea
/// {
///     public long Id { get; set; }
///     public long TenantId { get; set; }   // Tenant identifier
///     public string AreaName { get; set; } // Area name
///     public bool IsActive { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class TenantTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="TenantTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TenantTableAttribute"/> class.
    /// </remarks>
    public TenantTableAttribute()
    {
    }
}
