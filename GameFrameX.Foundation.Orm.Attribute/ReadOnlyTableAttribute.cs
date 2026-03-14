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
/// 只读表特性，用于标记实体类对应的数据库表为只读表
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体对应的数据库表是只读的。
/// 在ORM框架中，当实体类标记了此特性时，框架会禁用对该表的写入操作（INSERT、UPDATE、DELETE），
/// 并可以启用相应的查询优化策略，如查询缓存、读写分离等。
/// 
/// 只读表通常用于以下场景：
/// - 静态配置数据表
/// - 历史数据归档表
/// - 数据仓库的维度表
/// - 报表和统计数据表
/// - 第三方系统的数据视图
/// - 基础数据字典表
/// </remarks>
/// <example>
/// <code>
/// [ReadOnlyTable(EnableCache = true, CacheMinutes = 60)]
/// public class CountryCode
/// {
///     public string Code { get; set; }
///     public string Name { get; set; }
///     public string Region { get; set; }
/// }
/// 
/// [ReadOnlyTable(AllowRefresh = true)]
/// public class StaticConfiguration
/// {
///     public string Key { get; set; }
///     public string Value { get; set; }
///     public string Description { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class ReadOnlyTableAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置是否启用缓存
    /// </summary>
    /// <value>指示是否对只读表启用查询缓存，默认为 true</value>
    public bool EnableCache { get; set; } = true;

    /// <summary>
    /// 获取或设置缓存时间（分钟）
    /// </summary>
    /// <value>查询结果的缓存时间，单位为分钟，默认为60分钟</value>
    public int CacheMinutes { get; set; } = 60;

    /// <summary>
    /// 获取或设置是否允许刷新数据
    /// </summary>
    /// <value>指示是否允许通过特殊操作刷新只读表数据，默认为 false</value>
    public bool AllowRefresh { get; set; } = false;

    /// <summary>
    /// 获取或设置错误处理策略
    /// </summary>
    /// <value>当尝试对只读表进行写操作时的错误处理策略</value>
    public ReadOnlyErrorHandling ErrorHandling { get; set; } = ReadOnlyErrorHandling.ThrowException;

    /// <summary>
    /// 获取或设置自定义错误消息
    /// </summary>
    /// <value>当违反只读约束时显示的自定义错误消息</value>
    public string? CustomErrorMessage { get; set; }

    /// <summary>
    /// 初始化 <see cref="ReadOnlyTableAttribute"/> 类的新实例
    /// </summary>
    public ReadOnlyTableAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="ReadOnlyTableAttribute"/> 类的新实例
    /// </summary>
    /// <param name="enableCache">是否启用缓存</param>
    /// <param name="cacheMinutes">缓存时间（分钟）</param>
    public ReadOnlyTableAttribute(bool enableCache, int cacheMinutes = 60)
    {
        EnableCache = enableCache;
        CacheMinutes = cacheMinutes;
    }
}

/// <summary>
/// 只读表错误处理策略枚举
/// </summary>
public enum ReadOnlyErrorHandling
{
    /// <summary>
    /// 抛出异常
    /// </summary>
    ThrowException = 0,

    /// <summary>
    /// 静默忽略
    /// </summary>
    SilentIgnore = 1,

    /// <summary>
    /// 记录警告日志
    /// </summary>
    LogWarning = 2,

    /// <summary>
    /// 自定义处理
    /// </summary>
    Custom = 3
}