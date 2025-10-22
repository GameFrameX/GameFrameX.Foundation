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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 时间辅助工具类：提供 Unix 时间戳转换和时间基准点常量。
/// </summary>
/// <remarks>
/// 此类提供了常用的时间处理功能，包括：
/// - Unix 纪元时间常量（本地时间和 UTC 时间）
/// - Unix 时间戳获取方法（秒级和毫秒级）
/// 主要用于 ORM 实体中的时间字段处理和时间戳生成。
/// </remarks>
internal class TimerHelper
{
    /// <summary>
    /// Unix 纪元时间：1970-01-01 00:00:00 本地时间。
    /// </summary>
    /// <value>
    /// 表示 Unix 纪元开始时间的 <see cref="DateTime"/> 对象，时区为本地时间。
    /// </value>
    /// <remarks>
    /// 此常量用于本地时间与 Unix 时间戳之间的转换计算。
    /// Unix 纪元是计算机系统中时间戳的起始参考点。
    /// </remarks>
    /// <seealso cref="EpochUtc"/>
    public static readonly DateTime EpochLocal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);

    /// <summary>
    /// Unix 纪元时间：1970-01-01 00:00:00 UTC 时间。
    /// </summary>
    /// <value>
    /// 表示 Unix 纪元开始时间的 <see cref="DateTime"/> 对象，时区为 UTC。
    /// </value>
    /// <remarks>
    /// 此常量用于 UTC 时间与 Unix 时间戳之间的转换计算。
    /// UTC 时间是国际标准时间，不受时区影响，推荐在跨时区应用中使用。
    /// </remarks>
    /// <seealso cref="EpochLocal"/>
    public static readonly DateTime EpochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// 获取当前 UTC 时间的 Unix 时间戳（秒级精度）。
    /// </summary>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示从 Unix 纪元（1970-01-01 00:00:00 UTC）到当前时间的秒数。
    /// </returns>
    /// <remarks>
    /// 此方法返回的时间戳精度为秒级，适用于不需要高精度时间的场景。
    /// 时间戳基于 UTC 时间计算，避免了时区转换的复杂性。
    /// </remarks>
    /// <example>
    /// <code>
    /// long timestamp = TimerHelper.UnixTimeSeconds();
    /// Console.WriteLine($"当前 Unix 时间戳（秒）: {timestamp}");
    /// </code>
    /// </example>
    /// <seealso cref="UnixTimeMilliseconds"/>
    public static long UnixTimeSeconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 获取当前 UTC 时间的 Unix 时间戳（毫秒级精度）。
    /// </summary>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示从 Unix 纪元（1970-01-01 00:00:00 UTC）到当前时间的毫秒数。
    /// </returns>
    /// <remarks>
    /// 此方法返回的时间戳精度为毫秒级，适用于需要高精度时间的场景，如日志记录、性能监控等。
    /// 时间戳基于 UTC 时间计算，确保在不同时区环境下的一致性。
    /// </remarks>
    /// <example>
    /// <code>
    /// long timestamp = TimerHelper.UnixTimeMilliseconds();
    /// Console.WriteLine($"当前 Unix 时间戳（毫秒）: {timestamp}");
    /// </code>
    /// </example>
    /// <seealso cref="UnixTimeSeconds"/>
    public static long UnixTimeMilliseconds()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }
}