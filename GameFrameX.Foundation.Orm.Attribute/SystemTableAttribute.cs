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
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class SystemTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="SystemTableAttribute"/> 类的新实例
    /// </summary>
    public SystemTableAttribute()
    {
    }
}
