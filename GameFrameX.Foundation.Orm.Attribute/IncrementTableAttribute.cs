// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 增量表特性，用于标记实体类对应的数据库表支持增量操作
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表支持增量数据处理。
/// 在ORM框架中，当实体类标记了此特性时，框架会启用增量表相关的功能，
/// 例如增量数据同步、变更数据捕获(CDC)、数据版本控制等。
/// 
/// 增量表通常用于以下场景：
/// - 数据仓库的ETL过程中的增量数据加载
/// - 数据同步场景中只处理变更的数据
/// - 大数据量表的性能优化
/// - 数据备份和恢复的增量策略
/// </remarks>
/// <example>
/// <code>
/// [IncrementTable]
/// public class UserActivity
/// {
///     public int Id { get; set; }
///     public int UserId { get; set; }
///     public string Activity { get; set; }
///     public DateTime Timestamp { get; set; }  // 用于增量判断的时间戳
/// }
/// 
/// [IncrementTable]
/// public class ProductInventory
/// {
///     public int ProductId { get; set; }
///     public int Quantity { get; set; }
///     public DateTime LastModified { get; set; }  // 增量更新标识
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class IncrementTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="IncrementTableAttribute"/> 类的新实例
    /// </summary>
    public IncrementTableAttribute()
    {
    }
}
