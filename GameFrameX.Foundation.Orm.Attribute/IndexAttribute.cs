// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 索引特性，用于标记属性或字段需要创建数据库索引
/// </summary>
/// <remarks>
/// 此特性应用于属性或字段，用于指示ORM框架在该字段上创建数据库索引。
/// 索引可以显著提高查询性能，但会增加写入操作的开销和存储空间。
/// 
/// 索引的使用原则：
/// - 经常用于WHERE条件的字段
/// - 经常用于JOIN操作的字段
/// - 经常用于ORDER BY的字段
/// - 唯一性约束字段
/// - 外键字段
/// 
/// 注意事项：
/// - 过多的索引会影响写入性能
/// - 复合索引的字段顺序很重要
/// - 定期维护和优化索引
/// </remarks>
/// <example>
/// <code>
/// public class User
/// {
///     public int Id { get; set; }
///     
///     [Index("IX_User_Email", IsUnique = true)]
///     public string Email { get; set; }
///     
///     [Index("IX_User_Name")]
///     public string Name { get; set; }
///     
///     [Index("IX_User_Status_CreateTime", Order = 1)]
///     public string Status { get; set; }
///     
///     [Index("IX_User_Status_CreateTime", Order = 2)]
///     public DateTime CreateTime { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
public sealed class IndexAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置索引名称
    /// </summary>
    /// <value>索引的名称，如果为空则使用默认命名规则</value>
    public string? IndexName { get; set; }

    /// <summary>
    /// 获取或设置是否为唯一索引
    /// </summary>
    /// <value>指示是否创建唯一索引，默认为 false</value>
    public bool IsUnique { get; set; } = false;

    /// <summary>
    /// 获取或设置是否为聚集索引
    /// </summary>
    /// <value>指示是否创建聚集索引，默认为 false</value>
    public bool IsClustered { get; set; } = false;

    /// <summary>
    /// 获取或设置索引中字段的排序顺序
    /// </summary>
    /// <value>在复合索引中字段的排序顺序，数值越小优先级越高</value>
    public int Order { get; set; } = 0;

    /// <summary>
    /// 获取或设置索引的排序方向
    /// </summary>
    /// <value>索引字段的排序方向，默认为升序</value>
    public IndexSortDirection SortDirection { get; set; } = IndexSortDirection.Ascending;

    /// <summary>
    /// 获取或设置填充因子
    /// </summary>
    /// <value>索引页的填充因子，取值范围1-100，0表示使用默认值</value>
    public int FillFactor { get; set; } = 0;

    /// <summary>
    /// 获取或设置是否包含列
    /// </summary>
    /// <value>指示该字段是否作为包含列添加到索引中，默认为 false</value>
    public bool IsIncluded { get; set; } = false;

    /// <summary>
    /// 初始化 <see cref="IndexAttribute"/> 类的新实例
    /// </summary>
    public IndexAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="IndexAttribute"/> 类的新实例
    /// </summary>
    /// <param name="indexName">索引名称</param>
    public IndexAttribute(string indexName)
    {
        IndexName = indexName;
    }

    /// <summary>
    /// 初始化 <see cref="IndexAttribute"/> 类的新实例
    /// </summary>
    /// <param name="indexName">索引名称</param>
    /// <param name="isUnique">是否为唯一索引</param>
    public IndexAttribute(string indexName, bool isUnique)
    {
        IndexName = indexName;
        IsUnique = isUnique;
    }
}

/// <summary>
/// 索引排序方向枚举
/// </summary>
public enum IndexSortDirection
{
    /// <summary>
    /// 升序
    /// </summary>
    Ascending = 0,

    /// <summary>
    /// 降序
    /// </summary>
    Descending = 1
}