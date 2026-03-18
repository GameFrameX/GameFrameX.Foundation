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
/// 实体索引特性，用于标记需要建立索引的实体属性。
/// </summary>
/// <remarks>
/// Entity index attribute for marking entity properties that need to be indexed.
/// Supports setting index name, uniqueness, and sort order.
/// This attribute is primarily used for entity class properties in ORM frameworks to indicate that database table fields need indexes to improve query performance.
/// <para>
/// Indexes are important mechanisms in databases for improving query performance. By applying this attribute to entity properties, you can instruct the ORM framework to create indexes for corresponding fields in database tables.
/// </para>
/// <para>
/// Index types include:
/// </para>
/// <list type="bullet">
/// <item><description>Regular index: Allows duplicate values, used to improve query speed</description></item>
/// <item><description>Unique index: Does not allow duplicate values, ensures data uniqueness while improving query speed</description></item>
/// </list>
/// <para>
/// Sort orders include:
/// </para>
/// <list type="bullet">
/// <item><description>Ascending (ASC): Data sorted from small to large, default order</description></item>
/// <item><description>Descending (DESC): Data sorted from large to small</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// public class User
/// {
///     [EntityIndex(Name = "IX_User_Email", Unique = true)]
///     public string Email { get; set; }
///
///     [EntityIndex(Name = "IX_User_CreateTime", IsAscending = false)]
///     public DateTime CreateTime { get; set; }
///
///     [EntityIndex(Name = "IX_User_Status")]
///     public int Status { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class EntityIndexAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置该索引是否为唯一索引。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether the index is unique.
    /// Unique indexes not only improve query performance but also ensure data uniqueness constraints.
    /// When set as unique, the database checks field value uniqueness during insert or update operations.
    /// </remarks>
    /// <value>
    /// 如果为 <c>true</c>，表示该索引为唯一索引，不允许重复值；
    /// 如果为 <c>false</c>，表示该索引为普通索引，允许重复值。
    /// 默认值为 <c>false</c>。
    /// <para>
    /// If <c>true</c>, indicates a unique index that does not allow duplicate values;
    /// If <c>false</c>, indicates a regular index that allows duplicate values.
    /// Default is <c>false</c>.
    /// </para>
    /// </value>
    public bool Unique { get; set; } = false;

    /// <summary>
    /// 获取或设置索引的名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the index name.
    /// Index names must be unique in the database; meaningful naming conventions are recommended.
    /// Recommended naming convention: IX_TableName_FieldName, e.g., IX_User_Email.
    /// If no name is specified, some ORM frameworks may auto-generate index names using default rules.
    /// </remarks>
    /// <value>索引的名称字符串，不能为 <c>null</c> 或空字符串 / Index name string, cannot be <c>null</c> or empty string</value>
    /// <exception cref="ArgumentException">当设置的值为 <c>null</c> 或空字符串时抛出 / Thrown when the value is <c>null</c> or empty string</exception>
    public string Name { get; set; }

    /// <summary>
    /// 获取或设置该索引的排序方向。
    /// </summary>
    /// <remarks>
    /// Gets or sets the sort direction of the index.
    /// Sort direction affects index storage order and query performance.
    /// Ascending indexes are suitable for minimum value lookups in range queries, while descending indexes are suitable for maximum value lookups.
    /// For time fields, if frequently querying recent records, descending indexes are recommended.
    /// </remarks>
    /// <value>
    /// 如果为 <c>true</c>，表示升序排列（ASC）；
    /// 如果为 <c>false</c>，表示降序排列（DESC）。
    /// 默认值为 <c>true</c>。
    /// <para>
    /// If <c>true</c>, indicates ascending order (ASC);
    /// If <c>false</c>, indicates descending order (DESC).
    /// Default is <c>true</c>.
    /// </para>
    /// </value>
    public bool IsAscending { get; set; } = true;

    /// <summary>
    /// 初始化 <see cref="EntityIndexAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EntityIndexAttribute"/> class with the specified name.
    /// By default, creates a non-unique index (<see cref="Unique"/> = <c>false</c>) with ascending order (<see cref="IsAscending"/> = <c>true</c>).
    /// To create a unique or descending index, set the corresponding property values after creating the instance.
    /// </remarks>
    /// <param name="name">索引的名称，不能为 <c>null</c>、空字符串或仅包含空白字符 / Index name, cannot be <c>null</c>, empty, or whitespace only</param>
    /// <exception cref="ArgumentException">当 <paramref name="name"/> 为 <c>null</c>、空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="name"/> is <c>null</c>, empty, or whitespace only</exception>
    public EntityIndexAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        this.Name = name;
    }
}
