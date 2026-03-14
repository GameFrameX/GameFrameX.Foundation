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
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class LogTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="LogTableAttribute"/> 类的新实例
    /// </summary>
    public LogTableAttribute()
    {
    }
}
