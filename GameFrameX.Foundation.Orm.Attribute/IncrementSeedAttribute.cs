// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 增量种子特性，用于标记实体类支持自增种子值功能
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体在数据库中使用自增种子值作为主键或唯一标识。
/// 在ORM框架中，当实体类标记了此特性时，框架会自动处理主键的自增逻辑，
/// 确保每次插入新记录时都能获得唯一的递增标识符。
/// 
/// 通常用于以下场景：
/// - 数据库表的主键使用自增整数
/// - 需要保证插入顺序的业务场景
/// - 分布式环境下的唯一ID生成
/// </remarks>
/// <example>
/// <code>
/// [IncrementSeed]
/// public class User
/// {
///     public int Id { get; set; }  // 自增主键
///     public string Name { get; set; }
///     public DateTime CreateTime { get; set; }
/// }
/// 
/// [IncrementSeed]
/// public class OrderLog
/// {
///     public long LogId { get; set; }  // 自增日志ID
///     public string Content { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class IncrementSeedAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="IncrementSeedAttribute"/> 类的新实例
    /// </summary>
    public IncrementSeedAttribute()
    {
    }
}
