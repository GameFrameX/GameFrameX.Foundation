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
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class IncrementTableAttribute : System.Attribute
{
    /// <summary>
    /// 初始化 <see cref="IncrementTableAttribute"/> 类的新实例
    /// </summary>
    public IncrementTableAttribute()
    {
    }
}
