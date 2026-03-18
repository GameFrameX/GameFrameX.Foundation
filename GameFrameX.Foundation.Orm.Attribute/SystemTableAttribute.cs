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
/// 系统表特性，用于标记实体类对应的数据库表为系统表。
/// </summary>
/// <remarks>
/// System table attribute for marking entity classes whose corresponding database tables are system tables.
/// When an entity class is marked with this attribute, the ORM framework enables system table-related features,
/// such as special permission controls, system-level caching strategies, and special backup and recovery strategies.
/// <para>
/// System tables typically have the following characteristics:
/// </para>
/// <list type="bullet">
/// <item><description>Store system core configurations and metadata</description></item>
/// <item><description>High requirements for data consistency and integrity</description></item>
/// <item><description>Usually require special access permission controls</description></item>
/// <item><description>May require special backup and recovery strategies</description></item>
/// <item><description>Data changes require strict auditing and monitoring</description></item>
/// <item><description>Usually do not allow direct operations by ordinary users</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [SystemTable]
/// public class SystemConfiguration
/// {
///     public int Id { get; set; }
///     public string ConfigKey { get; set; }    // Configuration key
///     public string ConfigValue { get; set; }  // Configuration value
///     public string Description { get; set; }  // Configuration description
///     public bool IsActive { get; set; }       // Is active
///     public DateTime CreatedTime { get; set; }
///     public DateTime? ModifiedTime { get; set; }
/// }
///
/// [SystemTable]
/// public class SystemPermission
/// {
///     public int Id { get; set; }
///     public string PermissionCode { get; set; }  // Permission code
///     public string PermissionName { get; set; }  // Permission name
///     public string Module { get; set; }          // Module
///     public int Level { get; set; }              // Permission level
///     public bool IsSystemLevel { get; set; }     // Is system-level permission
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class SystemTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="SystemTableAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SystemTableAttribute"/> class.
    /// </remarks>
    public SystemTableAttribute()
    {
    }
}
