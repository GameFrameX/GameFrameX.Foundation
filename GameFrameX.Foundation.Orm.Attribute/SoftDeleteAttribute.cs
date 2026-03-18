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
/// 软删除特性，用于标记实体类支持软删除功能。
/// </summary>
/// <remarks>
/// Soft delete attribute for marking entity classes that support soft delete functionality.
/// Soft delete means that when deleting data, records are not actually removed from the database but are marked as deleted through a field.
/// This preserves data integrity, supports data recovery, and meets auditing requirements.
/// <para>
/// Advantages of soft delete:
/// </para>
/// <list type="bullet">
/// <item><description>Data security: Avoids data loss from accidental deletion</description></item>
/// <item><description>Audit trail: Preserves history of delete operations</description></item>
/// <item><description>Data recovery: Supports recovery of deleted data</description></item>
/// <item><description>Referential integrity: Maintains foreign key relationship integrity</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [SoftDelete("IsDeleted", "DeletedTime")]
/// public class User
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public bool IsDeleted { get; set; }
///     public DateTime? DeletedTime { get; set; }
///     public string? DeletedBy { get; set; }
/// }
///
/// [SoftDelete("Status", DeletedValue = "DELETED")]
/// public class Product
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public string Status { get; set; } = "ACTIVE";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class SoftDeleteAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置软删除标记字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the field name for marking records as deleted.
    /// </remarks>
    /// <value>用于标记记录是否被删除的字段名称 / Field name for marking whether a record is deleted</value>
    public string DeletedField { get; set; }

    /// <summary>
    /// 获取或设置删除时间字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the field name for recording deletion time.
    /// </remarks>
    /// <value>记录删除时间的字段名称，可以为空 / Field name for recording deletion time, can be null</value>
    public string? DeletedTimeField { get; set; }

    /// <summary>
    /// 获取或设置删除用户字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the field name for recording the user who performed the deletion.
    /// </remarks>
    /// <value>记录删除操作用户的字段名称，可以为空 / Field name for recording the user who performed deletion, can be null</value>
    public string? DeletedByField { get; set; }

    /// <summary>
    /// 获取或设置表示已删除的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value representing the deleted state in the marker field.
    /// </remarks>
    /// <value>标记字段中表示已删除状态的值，默认为 <c>true</c> / Value representing deleted state in marker field, default is <c>true</c></value>
    public object DeletedValue { get; set; } = true;

    /// <summary>
    /// 获取或设置表示未删除的值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the value representing the not-deleted state in the marker field.
    /// </remarks>
    /// <value>标记字段中表示未删除状态的值，默认为 <c>false</c> / Value representing not-deleted state in marker field, default is <c>false</c></value>
    public object NotDeletedValue { get; set; } = false;

    /// <summary>
    /// 获取或设置是否在查询时自动过滤已删除记录。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to automatically filter out deleted records in queries.
    /// </remarks>
    /// <value>指示是否在查询时自动排除已删除的记录，默认为 <c>true</c> / Indicates whether to automatically exclude deleted records in queries, default is <c>true</c></value>
    public bool AutoFilter { get; set; } = true;

    /// <summary>
    /// 初始化 <see cref="SoftDeleteAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SoftDeleteAttribute"/> class with the specified deleted field.
    /// </remarks>
    /// <param name="deletedField">软删除标记字段名称 / Soft delete marker field name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="deletedField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="deletedField"/> is <c>null</c></exception>
    public SoftDeleteAttribute(string deletedField)
    {
        DeletedField = deletedField ?? throw new ArgumentNullException(nameof(deletedField));
    }

    /// <summary>
    /// 初始化 <see cref="SoftDeleteAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SoftDeleteAttribute"/> class with the specified deleted field and time field.
    /// </remarks>
    /// <param name="deletedField">软删除标记字段名称 / Soft delete marker field name</param>
    /// <param name="deletedTimeField">删除时间字段名称 / Deletion time field name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="deletedField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="deletedField"/> is <c>null</c></exception>
    public SoftDeleteAttribute(string deletedField, string? deletedTimeField)
    {
        DeletedField = deletedField ?? throw new ArgumentNullException(nameof(deletedField));
        DeletedTimeField = deletedTimeField;
    }

    /// <summary>
    /// 初始化 <see cref="SoftDeleteAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SoftDeleteAttribute"/> class with the specified deleted field, time field, and user field.
    /// </remarks>
    /// <param name="deletedField">软删除标记字段名称 / Soft delete marker field name</param>
    /// <param name="deletedTimeField">删除时间字段名称 / Deletion time field name</param>
    /// <param name="deletedByField">删除用户字段名称 / Deletion user field name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="deletedField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="deletedField"/> is <c>null</c></exception>
    public SoftDeleteAttribute(string deletedField, string? deletedTimeField, string? deletedByField)
    {
        DeletedField = deletedField ?? throw new ArgumentNullException(nameof(deletedField));
        DeletedTimeField = deletedTimeField;
        DeletedByField = deletedByField;
    }
}
