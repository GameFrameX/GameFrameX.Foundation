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
/// 导入导出特性，用于标记实体类是否支持 Excel 导入导出，以及字段级的列映射。
/// </summary>
/// <remarks>
/// Import/export attribute for marking entity classes that support Excel import/export, and for field-level column mapping.
/// 类级标记控制表级开关（是否允许导入/导出、工作表名称）；
/// 字段级标记控制列定义（列显示名、顺序、是否参与导入/导出）。
/// <para>
/// 用法约定：
/// </para>
/// <list type="bullet">
/// <item><description>类级：<see cref="ImportEnabled"/>/<see cref="ExportEnabled"/> 控制表级开关，<see cref="SheetName"/> 指定工作表名</description></item>
/// <item><description>字段级：<see cref="DisplayName"/> 指定列名，<see cref="Order"/> 指定列顺序，<see cref="IgnoreImport"/>/<see cref="IgnoreExport"/> 控制是否参与导入/导出</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [ImportExport(SheetName = "游戏订单")]
/// public class GameOrder
/// {
///     [ImportExport("订单编号", Order = 1)]
///     public string OrderNo { get; set; }
///
///     [ImportExport("金额", Order = 2)]
///     public decimal Amount { get; set; }
///
///     [ImportExport(IgnoreImport = true, IgnoreExport = true)]
///     public long CreatedUserId { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class ImportExportAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置是否允许导入（类级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether import is allowed (class-level).
    /// </remarks>
    /// <value>指示该表是否允许 Excel 导入，默认为 <c>true</c> / Indicates whether Excel import is allowed for this table, default is <c>true</c></value>
    public bool ImportEnabled { get; set; } = true;

    /// <summary>
    /// 获取或设置是否允许导出（类级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether export is allowed (class-level).
    /// </remarks>
    /// <value>指示该表是否允许 Excel 导出，默认为 <c>true</c> / Indicates whether Excel export is allowed for this table, default is <c>true</c></value>
    public bool ExportEnabled { get; set; } = true;

    /// <summary>
    /// 获取或设置工作表名称（类级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the worksheet name (class-level). If null, the entity type name is used.
    /// </remarks>
    /// <value>Excel 工作表名称，为空则使用实体类型名 / Excel worksheet name, uses entity type name if null</value>
    public string? SheetName { get; set; }

    /// <summary>
    /// 获取或设置列显示名称（字段级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the column display name (field-level).
    /// </remarks>
    /// <value>Excel 列显示名称，为空则使用字段名 / Excel column display name, uses field name if null</value>
    public string? DisplayName { get; set; }

    /// <summary>
    /// 获取或设置列顺序（字段级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the column order (field-level). Smaller values come first; 0 means undefined.
    /// </remarks>
    /// <value>Excel 列顺序，值小的在前，0 表示未指定 / Excel column order, smaller first, 0 means undefined</value>
    public int Order { get; set; }

    /// <summary>
    /// 获取或设置是否忽略导入（字段级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to ignore this field during import (field-level).
    /// </remarks>
    /// <value>指示导入时是否跳过该字段，默认为 <c>false</c> / Indicates whether to skip this field on import, default is <c>false</c></value>
    public bool IgnoreImport { get; set; }

    /// <summary>
    /// 获取或设置是否忽略导出（字段级）。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to ignore this field during export (field-level).
    /// </remarks>
    /// <value>指示导出时是否跳过该字段，默认为 <c>false</c> / Indicates whether to skip this field on export, default is <c>false</c></value>
    public bool IgnoreExport { get; set; }

    /// <summary>
    /// 初始化 <see cref="ImportExportAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ImportExportAttribute"/> class.
    /// </remarks>
    public ImportExportAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="ImportExportAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ImportExportAttribute"/> class with the specified column display name.
    /// </remarks>
    /// <param name="displayName">列显示名称 / Column display name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="displayName"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="displayName"/> is <c>null</c></exception>
    public ImportExportAttribute(string displayName)
    {
        DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
    }
}
