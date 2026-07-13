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
/// 数据范围特性，用于标记实体类启用行级数据权限过滤。
/// </summary>
/// <remarks>
/// Data scope attribute for marking entity classes to enable row-level data permission filtering.
/// 标记后，ORM 层结合当前用户角色的数据范围配置，在查询时自动注入过滤条件，
/// 使不同角色只能看到授权范围内的数据（全部 / 本部门 / 本部门及下级 / 仅本人 / 自定义）。
/// <para>
/// Supported data scopes:
/// </para>
/// <list type="bullet">
/// <item><description>All：全部数据 / All data</description></item>
/// <item><description>Department：本部门数据 / Current department only</description></item>
/// <item><description>DepartmentAndChild：本部门及下级部门 / Department and its descendants</description></item>
/// <item><description>Self：仅本人数据 / Current user only</description></item>
/// <item><description>Custom：自定义部门集合 / Custom department set</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [DataScope("CreatedUserId")]
/// public class GameOrder
/// {
///     public long Id { get; set; }
///     public long CreatedUserId { get; set; }  // 数据范围锚点字段 / Scope anchor field
///     public string OrderNo { get; set; }
/// }
///
/// [DataScope("CreatedUserId", DefaultScope = DataScope.Department, DepartmentField = "CreatedDeptId")]
/// public class GamePlayer
/// {
///     public long Id { get; set; }
///     public long CreatedUserId { get; set; }
///     public long CreatedDeptId { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class DataScopeAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置数据范围锚点字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the anchor field name used for data scope filtering (e.g. CreatedUserId).
    /// </remarks>
    /// <value>用于数据范围过滤的锚点字段名称 / Anchor field name for data scope filtering</value>
    public string ScopeField { get; set; }

    /// <summary>
    /// 获取或设置默认数据范围。
    /// </summary>
    /// <remarks>
    /// Gets or sets the default data scope applied when no role-specific scope is configured.
    /// </remarks>
    /// <value>未配置角色数据范围时应用的默认范围，默认为 <see cref="DataScope.Self"/> / Default scope applied when no role-specific scope is configured, default is <see cref="DataScope.Self"/></value>
    public DataScope DefaultScope { get; set; } = DataScope.Self;

    /// <summary>
    /// 获取或设置部门锚点字段名称（当数据范围基于部门时使用）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the department anchor field name, used when the data scope is department-based.
    /// </remarks>
    /// <value>基于部门的数据范围使用的锚点字段名称，可为空 / Department anchor field name, can be null</value>
    public string? DepartmentField { get; set; }

    /// <summary>
    /// 初始化 <see cref="DataScopeAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="DataScopeAttribute"/> class with the specified scope field.
    /// </remarks>
    /// <param name="scopeField">数据范围锚点字段名称 / Data scope anchor field name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="scopeField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="scopeField"/> is <c>null</c></exception>
    public DataScopeAttribute(string scopeField)
    {
        ScopeField = scopeField ?? throw new ArgumentNullException(nameof(scopeField));
    }
}

/// <summary>
/// 数据范围枚举。
/// </summary>
/// <remarks>
/// Data scope enumeration.
/// </remarks>
public enum DataScope
{
    /// <summary>
    /// 全部数据。
    /// </summary>
    /// <remarks>
    /// All data.
    /// </remarks>
    All = 0,

    /// <summary>
    /// 本部门数据。
    /// </summary>
    /// <remarks>
    /// Current department only.
    /// </remarks>
    Department = 1,

    /// <summary>
    /// 本部门及下级部门数据。
    /// </summary>
    /// <remarks>
    /// Current department and its descendants.
    /// </remarks>
    DepartmentAndChild = 2,

    /// <summary>
    /// 仅本人数据。
    /// </summary>
    /// <remarks>
    /// Current user only.
    /// </remarks>
    Self = 3,

    /// <summary>
    /// 自定义部门集合。
    /// </summary>
    /// <remarks>
    /// Custom department set.
    /// </remarks>
    Custom = 4
}
