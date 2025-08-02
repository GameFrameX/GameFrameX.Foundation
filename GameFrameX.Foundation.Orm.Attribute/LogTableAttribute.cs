// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 日志表特性，用于标记实体类对应的数据库表为日志表
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表是日志表。
/// 在ORM框架中，当实体类标记了此特性时，框架会启用日志表相关的功能，
/// 例如自动记录操作日志、审计跟踪、数据变更历史等。
/// 
/// 日志表通常具有以下特征：
/// - 只允许插入操作，不允许更新和删除
/// - 包含时间戳字段记录操作时间
/// - 包含操作类型字段（INSERT、UPDATE、DELETE等）
/// - 可能包含操作用户信息
/// - 数据量通常较大，需要考虑分区和归档策略
/// </remarks>
/// <example>
/// <code>
/// [LogTable]
/// public class UserOperationLog
/// {
///     public long Id { get; set; }
///     public int UserId { get; set; }
///     public string Operation { get; set; }  // 操作类型
///     public string TableName { get; set; }  // 操作的表名
///     public string OldValue { get; set; }   // 操作前的值
///     public string NewValue { get; set; }   // 操作后的值
///     public DateTime CreatedTime { get; set; }  // 操作时间
///     public string CreatedBy { get; set; }  // 操作用户
/// }
/// 
/// [LogTable]
/// public class SystemErrorLog
/// {
///     public long Id { get; set; }
///     public string ErrorMessage { get; set; }
///     public string StackTrace { get; set; }
///     public string Source { get; set; }
///     public DateTime LogTime { get; set; }
///     public string Level { get; set; }  // ERROR, WARN, INFO等
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class LogTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="LogTableAttribute"/> 类的新实例
    /// </summary>
    public LogTableAttribute()
    {
    }
}
