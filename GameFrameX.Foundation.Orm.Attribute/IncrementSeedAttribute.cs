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
/// 增量种子特性，用于标记实体类支持自增种子值功能。
/// </summary>
/// <remarks>
/// Increment seed attribute for marking entity classes that support auto-increment seed value functionality.
/// When an entity class is marked with this attribute, the ORM framework automatically handles the primary key auto-increment logic,
/// ensuring that each new record insertion receives a unique incrementing identifier.
/// <para>
/// Typically used in the following scenarios:
/// </para>
/// <list type="bullet">
/// <item><description>Database tables using auto-increment integers as primary keys</description></item>
/// <item><description>Business scenarios requiring insertion order guarantees</description></item>
/// <item><description>Unique ID generation in distributed environments</description></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// [IncrementSeed]
/// public class User
/// {
///     public int Id { get; set; }  // Auto-increment primary key
///     public string Name { get; set; }
///     public DateTime CreateTime { get; set; }
/// }
///
/// [IncrementSeed]
/// public class OrderLog
/// {
///     public long LogId { get; set; }  // Auto-increment log ID
///     public string Content { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class IncrementSeedAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="IncrementSeedAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="IncrementSeedAttribute"/> class.
    /// </remarks>
    public IncrementSeedAttribute()
    {
    }
}
