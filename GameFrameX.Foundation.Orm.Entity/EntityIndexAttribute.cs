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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 实体索引特性，用于标记需要建立索引的实体属性。
/// 支持设置索引名称、是否唯一以及排序方式。
/// 该特性主要用于ORM框架中的实体类属性，用于指示数据库表字段需要建立索引以提高查询性能。
/// </summary>
/// <remarks>
/// <para>索引是数据库中用于提高查询性能的重要机制。通过在实体属性上应用此特性，可以指示ORM框架在数据库表中为对应字段创建索引。</para>
/// <para>索引类型包括：</para>
/// <list type="bullet">
/// <item><description>普通索引：允许重复值，用于提高查询速度</description></item>
/// <item><description>唯一索引：不允许重复值，既保证数据唯一性又提高查询速度</description></item>
/// </list>
/// <para>排序方式包括：</para>
/// <list type="bullet">
/// <item><description>升序（ASC）：数据按从小到大排列，默认方式</description></item>
/// <item><description>降序（DESC）：数据按从大到小排列</description></item>
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
/// <seealso cref="System.Attribute"/>
/// <seealso cref="System.AttributeUsageAttribute"/>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class EntityIndexAttribute : Attribute
{
    /// <summary>
    /// 获取或设置该索引是否为唯一索引。
    /// </summary>
    /// <value>
    /// 如果为 <c>true</c>，表示该索引为唯一索引，不允许重复值；
    /// 如果为 <c>false</c>，表示该索引为普通索引，允许重复值。
    /// 默认值为 <c>false</c>。
    /// </value>
    /// <remarks>
    /// <para>唯一索引不仅能提高查询性能，还能保证数据的唯一性约束。</para>
    /// <para>当设置为唯一索引时，数据库会在插入或更新数据时检查该字段值的唯一性。</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// [EntityIndex(Name = "IX_User_Email", Unique = true)]
    /// public string Email { get; set; }
    /// </code>
    /// </example>
    public bool Unique { get; set; } = false;

    /// <summary>
    /// 获取或设置索引的名称。
    /// </summary>
    /// <value>
    /// 索引的名称字符串。不能为 <c>null</c> 或空字符串。
    /// </value>
    /// <remarks>
    /// <para>索引名称在数据库中必须唯一，建议使用有意义的命名规则。</para>
    /// <para>推荐的命名规则：IX_表名_字段名，例如：IX_User_Email</para>
    /// <para>如果未指定名称，某些ORM框架可能会按照默认规则自动生成索引名称。</para>
    /// </remarks>
    /// <exception cref="System.ArgumentException">当设置的值为 <c>null</c> 或空字符串时抛出。</exception>
    /// <example>
    /// <code>
    /// [EntityIndex(Name = "IX_User_CreateTime")]
    /// public DateTime CreateTime { get; set; }
    /// </code>
    /// </example>
    public string Name { get; set; }

    /// <summary>
    /// 获取或设置该索引的排序方向。
    /// </summary>
    /// <value>
    /// 如果为 <c>true</c>，表示升序排列（ASC）；
    /// 如果为 <c>false</c>，表示降序排列（DESC）。
    /// 默认值为 <c>true</c>。
    /// </value>
    /// <remarks>
    /// <para>排序方向影响索引的存储顺序和查询性能。</para>
    /// <para>升序索引适合范围查询中的最小值查找，降序索引适合最大值查找。</para>
    /// <para>对于时间字段，如果经常查询最新记录，建议使用降序索引。</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // 升序索引，适合查询最早的记录
    /// [EntityIndex(Name = "IX_User_CreateTime_ASC", IsAscending = true)]
    /// public DateTime CreateTime { get; set; }
    /// 
    /// // 降序索引，适合查询最新的记录
    /// [EntityIndex(Name = "IX_User_LastLoginTime_DESC", IsAscending = false)]
    /// public DateTime LastLoginTime { get; set; }
    /// </code>
    /// </example>
    public bool IsAscending { get; set; } = true;

    /// <summary>
    /// 初始化 <see cref="EntityIndexAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">索引的名称。不能为 <c>null</c>、空字符串或仅包含空白字符。</param>
    /// <exception cref="System.ArgumentException">当 <paramref name="name"/> 为 <c>null</c>、空字符串或仅包含空白字符时抛出。</exception>
    /// <remarks>
    /// <para>此构造函数创建一个具有指定名称的索引特性实例。</para>
    /// <para>默认情况下，创建的索引为非唯一索引（<see cref="Unique"/> = <c>false</c>）且为升序排列（<see cref="IsAscending"/> = <c>true</c>）。</para>
    /// <para>如果需要创建唯一索引或降序索引，请在创建实例后设置相应的属性值。</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // 创建普通索引
    /// [EntityIndex("IX_User_Email")]
    /// public string Email { get; set; }
    /// 
    /// // 创建唯一索引
    /// [EntityIndex("IX_User_Username", Unique = true)]
    /// public string Username { get; set; }
    /// 
    /// // 创建降序索引
    /// [EntityIndex("IX_User_CreateTime", IsAscending = false)]
    /// public DateTime CreateTime { get; set; }
    /// </code>
    /// </example>
    /// <seealso cref="Name"/>
    /// <seealso cref="Unique"/>
    /// <seealso cref="IsAscending"/>
    public EntityIndexAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        this.Name = name;
    }
}