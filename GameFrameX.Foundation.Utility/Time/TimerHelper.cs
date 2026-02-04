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

namespace GameFrameX.Foundation.Utility;

/// <summary>
/// 时间辅助工具类
/// </summary>
public partial class TimerHelper
{
    /// <summary>
    /// 当前时区，默认为 UTC
    /// </summary>
    private static TimeZoneInfo _currentTimeZone = TimeZoneInfo.Utc;

    /// <summary>
    /// 获取当前时区
    /// </summary>
    public static TimeZoneInfo CurrentTimeZone => _currentTimeZone;

    /// <summary>
    /// 设置当前时区
    /// </summary>
    /// <param name="timeZone">时区信息</param>
    public static void SetTimeZone(TimeZoneInfo timeZone)
    {
        _currentTimeZone = timeZone ?? TimeZoneInfo.Utc;
    }

    /// <summary>
    /// 设置当前时区
    /// </summary>
    /// <param name="timeZoneId">时区ID，如 "China Standard Time" 或 "UTC"</param>
    public static void SetTimeZone(string timeZoneId)
    {
        try
        {
            _currentTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch
        {
            _currentTimeZone = TimeZoneInfo.Utc;
        }
    }

    /// <summary>
    /// Unix 纪元时间：1970-01-01 00:00:00 本地时间。
    /// </summary>
    /// <value>
    /// 表示 Unix 纪元开始时间的 <see cref="DateTime"/> 对象，时区为本地时间。
    /// </value>
    /// <remarks>
    /// 此常量用于本地时间与 Unix 时间戳之间的转换计算。
    /// Unix 纪元是计算机系统中时间戳的起始参考点。
    /// 注意：此字段始终基于系统本地时区，不随 <see cref="CurrentTimeZone"/> 变化。
    /// 如需基于当前设置时区的纪元时间，请使用 TimeZoneInfo.ConvertTime(EpochUtc, CurrentTimeZone)。
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
        return new DateTimeOffset(GetUtcNow()).ToUnixTimeSeconds();
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
        return new DateTimeOffset(GetUtcNow()).ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 获取基于当前设置时区的 Unix 时间戳（秒级精度）。
    /// </summary>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示将当前设置时区的时间视为 UTC 时间时的 Unix 时间戳。
    /// 即：标准 Unix 时间戳 + 时区偏移秒数。
    /// </returns>
    /// <remarks>
    /// 此方法返回的时间戳包含时区偏移量。
    /// 例如：如果当前时区为 UTC+8，则返回的时间戳比标准 UTC 时间戳大 8 小时（28800秒）。
    /// </remarks>
    public static long UnixTimeSecondsWithTimeZone()
    {
        var utcNow = GetUtcNow();
        var offset = CurrentTimeZone.GetUtcOffset(utcNow);
        return new DateTimeOffset(utcNow).ToUnixTimeSeconds() + (long)offset.TotalSeconds;
    }

    /// <summary>
    /// 获取基于当前设置时区的 Unix 时间戳（毫秒级精度）。
    /// </summary>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示将当前设置时区的时间视为 UTC 时间时的 Unix 时间戳。
    /// 即：标准 Unix 时间戳 + 时区偏移毫秒数。
    /// </returns>
    /// <remarks>
    /// 此方法返回的时间戳包含时区偏移量。
    /// 例如：如果当前时区为 UTC+8，则返回的时间戳比标准 UTC 时间戳大 8 小时（28800000毫秒）。
    /// </remarks>
    public static long UnixTimeMillisecondsWithTimeZone()
    {
        var utcNow = GetUtcNow();
        var offset = CurrentTimeZone.GetUtcOffset(utcNow);
        return new DateTimeOffset(utcNow).ToUnixTimeMilliseconds() + (long)offset.TotalMilliseconds;
    }

    /// <summary>
    /// 获取指定时间基于当前设置时区的 Unix 时间戳（秒级精度）。
    /// </summary>
    /// <param name="time">要转换的时间。</param>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示将指定时间视为 UTC 时间时的 Unix 时间戳 + 时区偏移。
    /// </returns>
    /// <remarks>
    /// 此方法会先将输入时间转换为 UTC（如果不是），然后加上当前设置时区的偏移量。
    /// 如果输入时间 Kind 为 Unspecified，则默认视为当前设置时区 (<see cref="CurrentTimeZone"/>) 的时间。
    /// </remarks>
    public static long TimeToSecondsWithTimeZone(DateTime time)
    {
        var utcTime = ConvertToUtc(time);
        var offset = CurrentTimeZone.GetUtcOffset(utcTime);
        return new DateTimeOffset(utcTime).ToUnixTimeSeconds() + (long)offset.TotalSeconds;
    }

    /// <summary>
    /// 获取指定时间基于当前设置时区的 Unix 时间戳（毫秒级精度）。
    /// </summary>
    /// <param name="time">要转换的时间。</param>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示将指定时间视为 UTC 时间时的 Unix 时间戳 + 时区偏移。
    /// </returns>
    /// <remarks>
    /// 此方法会先将输入时间转换为 UTC（如果不是），然后加上当前设置时区的偏移量。
    /// 如果输入时间 Kind 为 Unspecified，则默认视为当前设置时区 (<see cref="CurrentTimeZone"/>) 的时间。
    /// </remarks>
    public static long TimeToMillisecondsWithTimeZone(DateTime time)
    {
        var utcTime = ConvertToUtc(time);
        var offset = CurrentTimeZone.GetUtcOffset(utcTime);
        return new DateTimeOffset(utcTime).ToUnixTimeMilliseconds() + (long)offset.TotalMilliseconds;
    }

    /// <summary>
    /// 将 DateTime 转换为 Unix 时间戳（秒）
    /// 自动处理 DateTime.Kind
    /// Utc -> 使用 Zero 偏移
    /// Local -> 使用 Local 偏移
    /// Unspecified -> 使用 CurrentTimeZone 偏移
    /// </summary>
    private static long DateTimeToUnixTimeSeconds(DateTime time)
    {
        TimeSpan offset;
        if (time.Kind == DateTimeKind.Utc)
        {
            offset = TimeSpan.Zero;
        }
        else if (time.Kind == DateTimeKind.Local)
        {
            offset = TimeZoneInfo.Local.GetUtcOffset(time);
        }
        else
        {
            offset = CurrentTimeZone.GetUtcOffset(time);
        }

        return new DateTimeOffset(time, offset).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 将时间转换为 UTC 时间。
    /// </summary>
    /// <param name="time">要转换的时间。</param>
    /// <returns>转换后的 UTC 时间。</returns>
    /// <remarks>
    /// - 如果 Kind 为 Utc，直接返回。
    /// - 如果 Kind 为 Local，转换为 UTC。
    /// - 如果 Kind 为 Unspecified，视为当前设置时区 (<see cref="CurrentTimeZone"/>) 的时间并转换为 UTC。
    /// </remarks>
    private static DateTime ConvertToUtc(DateTime time)
    {
        if (time.Kind == DateTimeKind.Utc)
        {
            return time;
        }

        if (time.Kind == DateTimeKind.Local)
        {
            return time.ToUniversalTime();
        }

        // Unspecified, assume CurrentTimeZone
        return TimeZoneInfo.ConvertTimeToUtc(time, CurrentTimeZone);
    }

    /// <summary>
    /// 获取指定时间距离纪元时间的毫秒数。
    /// </summary>
    /// <param name="time">要转换的指定时间。</param>
    /// <param name="utc">指定使用的纪元时间类型。如果为 <c>true</c>，使用 UTC 纪元时间；如果为 <c>false</c>，使用本地纪元时间。默认值为 <c>false</c>。</param>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示指定时间距离相应纪元时间的毫秒数。
    /// </returns>
    /// <remarks>
    /// 此方法根据 <paramref name="utc"/> 参数选择不同的纪元时间进行计算：
    /// - 当 <paramref name="utc"/> 为 <c>true</c> 时，使用 <see cref="EpochUtc"/>（1970-01-01 00:00:00 UTC）作为基准
    /// - 当 <paramref name="utc"/> 为 <c>false</c> 时，使用 <see cref="EpochLocal"/>（1970-01-01 00:00:00 本地时间）作为基准
    /// 
    /// 计算公式：毫秒数 = (指定时间 - 纪元时间).TotalMilliseconds
    /// 
    /// 注意事项：
    /// - 如果指定时间早于纪元时间，返回值将为负数
    /// - 毫秒级精度适用于需要高精度时间差计算的场景
    /// </remarks>
    /// <example>
    /// <code>
    /// DateTime now = DateTime.Now;
    /// DateTime utcNow = DateTime.UtcNow;
    /// 
    /// // 使用本地纪元时间计算
    /// long localMillis = TimerHelper.TimeToMilliseconds(now, false);
    /// Console.WriteLine($"距离本地纪元时间: {localMillis} 毫秒");
    /// 
    /// // 使用UTC纪元时间计算
    /// long utcMillis = TimerHelper.TimeToMilliseconds(utcNow, true);
    /// Console.WriteLine($"距离UTC纪元时间: {utcMillis} 毫秒");
    /// 
    /// // 计算历史时间（负值示例）
    /// DateTime historical = new DateTime(1969, 12, 31, 23, 59, 59);
    /// long historicalMillis = TimerHelper.TimeToMilliseconds(historical, true);
    /// Console.WriteLine($"历史时间毫秒数: {historicalMillis}"); // 负值
    /// </code>
    /// </example>
    /// <seealso cref="TimeToSecond"/>
    /// <seealso cref="EpochUtc"/>
    /// <seealso cref="EpochLocal"/>
    public static long TimeToMilliseconds(DateTime time, bool utc = false)
    {
        if (utc)
        {
            return (long)(time - EpochUtc).TotalMilliseconds;
        }

        return (long)(time - EpochLocal).TotalMilliseconds;
    }

    /// <summary>
    /// 获取指定时间距离纪元时间的秒数。
    /// </summary>
    /// <param name="time">要转换的指定时间。</param>
    /// <param name="utc">指定使用的纪元时间类型。如果为 <c>true</c>，使用 UTC 纪元时间；如果为 <c>false</c>，使用本地纪元时间。默认值为 <c>false</c>。</param>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示指定时间距离相应纪元时间的秒数。
    /// </returns>
    /// <remarks>
    /// 此方法根据 <paramref name="utc"/> 参数选择不同的纪元时间进行计算：
    /// - 当 <paramref name="utc"/> 为 <c>true</c> 时，使用 <see cref="EpochUtc"/>（1970-01-01 00:00:00 UTC）作为基准
    /// - 当 <paramref name="utc"/> 为 <c>false</c> 时，使用 <see cref="EpochLocal"/>（1970-01-01 00:00:00 本地时间）作为基准
    /// 
    /// 计算公式：秒数 = (指定时间 - 纪元时间).TotalSeconds
    /// 
    /// 注意事项：
    /// - 如果指定时间早于纪元时间，返回值将为负数
    /// - 秒级精度适用于一般的时间戳计算和存储场景
    /// - 相比毫秒级精度，占用更少的存储空间
    /// </remarks>
    /// <example>
    /// <code>
    /// DateTime now = DateTime.Now;
    /// DateTime utcNow = DateTime.UtcNow;
    /// 
    /// // 使用本地纪元时间计算
    /// long localSeconds = TimerHelper.TimeToSecond(now, false);
    /// Console.WriteLine($"距离本地纪元时间: {localSeconds} 秒");
    /// 
    /// // 使用UTC纪元时间计算
    /// long utcSeconds = TimerHelper.TimeToSecond(utcNow, true);
    /// Console.WriteLine($"距离UTC纪元时间: {utcSeconds} 秒");
    /// 
    /// // 与毫秒级精度对比
    /// long millis = TimerHelper.TimeToMilliseconds(now, false);
    /// Console.WriteLine($"秒级: {localSeconds}, 毫秒级: {millis}");
    /// Console.WriteLine($"精度差异: {millis - localSeconds * 1000} 毫秒");
    /// </code>
    /// </example>
    /// <seealso cref="TimeToMilliseconds"/>
    /// <seealso cref="EpochUtc"/>
    /// <seealso cref="EpochLocal"/>
    public static long TimeToSecond(DateTime time, bool utc = false)
    {
        if (utc)
        {
            return (long)(time - EpochUtc).TotalSeconds;
        }

        return (long)(time - EpochLocal).TotalSeconds;
    }

}