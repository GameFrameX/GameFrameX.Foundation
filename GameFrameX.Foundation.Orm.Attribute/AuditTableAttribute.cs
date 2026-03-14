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
/// 审计表特性，用于标记实体类对应的数据库表需要进行审计跟踪
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表需要进行审计跟踪。
/// 在ORM框架中，当实体类标记了此特性时，框架会自动记录数据的变更历史，
/// 包括创建、更新、删除等操作的详细信息，用于合规性要求和安全审计。
/// 
/// 审计表通常记录以下信息：
/// - 操作类型（INSERT、UPDATE、DELETE）
/// - 操作时间和操作用户
/// - 变更前后的数据值
/// - 操作来源（IP地址、应用程序等）
/// - 业务上下文信息
/// </remarks>
/// <example>
/// <code>
/// [AuditTable(AuditLevel = AuditLevel.Full, IncludeUserInfo = true)]
/// public class UserAccount
/// {
///     public int Id { get; set; }
///     public string Username { get; set; }
///     public decimal Balance { get; set; }
///     public DateTime LastLoginTime { get; set; }
/// }
/// 
/// [AuditTable(AuditLevel = AuditLevel.ChangesOnly)]
/// public class SystemSettings
/// {
///     public string Key { get; set; }
///     public string Value { get; set; }
///     public bool IsActive { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class AuditTableAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置审计级别
    /// </summary>
    /// <value>审计级别，控制审计的详细程度</value>
    public AuditLevel AuditLevel { get; set; } = AuditLevel.ChangesOnly;

    /// <summary>
    /// 获取或设置是否包含用户信息
    /// </summary>
    /// <value>指示是否在审计记录中包含操作用户信息，默认为 true</value>
    public bool IncludeUserInfo { get; set; } = true;

    /// <summary>
    /// 获取或设置是否包含IP地址
    /// </summary>
    /// <value>指示是否在审计记录中包含客户端IP地址，默认为 false</value>
    public bool IncludeIpAddress { get; set; } = false;

    /// <summary>
    /// 获取或设置审计表名称
    /// </summary>
    /// <value>存储审计记录的表名称，如果为空则使用默认命名规则</value>
    public string? AuditTableName { get; set; }

    /// <summary>
    /// 获取或设置是否启用审计
    /// </summary>
    /// <value>指示是否启用审计功能，默认为 true</value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// 初始化 <see cref="AuditTableAttribute"/> 类的新实例
    /// </summary>
    public AuditTableAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="AuditTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="auditLevel">审计级别</param>
    public AuditTableAttribute(AuditLevel auditLevel)
    {
        AuditLevel = auditLevel;
    }
}

/// <summary>
/// 审计级别枚举
/// </summary>
public enum AuditLevel
{
    /// <summary>
    /// 不进行审计
    /// </summary>
    None = 0,

    /// <summary>
    /// 仅审计数据变更
    /// </summary>
    ChangesOnly = 1,

    /// <summary>
    /// 审计所有操作（包括查询）
    /// </summary>
    Full = 2,

    /// <summary>
    /// 自定义审计规则
    /// </summary>
    Custom = 3
}