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
/// 树形结构实体特性，用于标记自引用树形实体（如部门、菜单、区域、字典分类）。
/// </summary>
/// <remarks>
/// Tree entity attribute for marking self-referencing tree-structured entities (e.g. departments, menus, regions, dictionary categories).
/// 标记后，ORM/业务层可识别父节点字段、物化路径、层级、排序等元数据，
/// 进而支持递归查询、路径前缀检索、懒加载子树与整树排序。
/// </remarks>
/// <example>
/// <code>
/// [TreeEntity("ParentId", PathField = "Path", LevelField = "Level", SortField = "Sort", ChildrenField = "Children")]
/// public class SysDept
/// {
///     public long Id { get; set; }
///     public long? ParentId { get; set; }   // 父节点 / Parent node id
///     public string Path { get; set; }      // 物化路径，如 "1.5.12" / Materialized path, e.g. "1.5.12"
///     public int Level { get; set; }        // 层级（根为 1） / Level (root = 1)
///     public int Sort { get; set; }         // 同级排序 / Sibling sort order
///     public string Name { get; set; }
///     public List&lt;SysDept&gt; Children { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class TreeEntityAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置父节点字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the parent identifier field name.
    /// </remarks>
    /// <value>指向父节点主键的字段名称 / Field name pointing to the parent node primary key</value>
    public string ParentIdField { get; set; }

    /// <summary>
    /// 获取或设置物化路径字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the materialized path field name (e.g. "1.5.12"), used for efficient subtree/ancestor queries.
    /// </remarks>
    /// <value>物化路径字段名称，可为空 / Materialized path field name, can be null</value>
    public string? PathField { get; set; }

    /// <summary>
    /// 获取或设置层级字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the tree level field name (root = 1).
    /// </remarks>
    /// <value>层级字段名称，可为空 / Tree level field name, can be null</value>
    public string? LevelField { get; set; }

    /// <summary>
    /// 获取或设置同级排序字段名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the sibling sort field name.
    /// </remarks>
    /// <value>同级排序字段名称，可为空 / Sibling sort field name, can be null</value>
    public string? SortField { get; set; }

    /// <summary>
    /// 获取或设置子节点集合字段名称（用于内存树构建）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the children collection field name (used for in-memory tree building).
    /// </remarks>
    /// <value>子节点集合字段名称，可为空 / Children collection field name, can be null</value>
    public string? ChildrenField { get; set; }

    /// <summary>
    /// 获取或设置最大树深度（0 表示不限制）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the maximum tree depth. 0 means unlimited.
    /// </remarks>
    /// <value>允许的最大树深度，0 表示不限制，默认为 0 / Maximum allowed tree depth, 0 means unlimited, default is 0</value>
    public int MaxDepth { get; set; } = 0;

    /// <summary>
    /// 初始化 <see cref="TreeEntityAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TreeEntityAttribute"/> class with the specified parent id field.
    /// </remarks>
    /// <param name="parentIdField">父节点字段名称 / Parent identifier field name</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="parentIdField"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="parentIdField"/> is <c>null</c></exception>
    public TreeEntityAttribute(string parentIdField)
    {
        ParentIdField = parentIdField ?? throw new ArgumentNullException(nameof(parentIdField));
    }
}
