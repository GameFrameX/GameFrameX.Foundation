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

    /// <summary>
    /// 获取当前本地时区时间的 Unix 时间戳（秒级精度）。
    /// </summary>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示从 Unix 纪元（1970-01-01 00:00:00 UTC）到当前本地时间的秒数。
    /// </returns>
    /// <remarks>
    /// 此方法执行以下步骤：
    /// 1. 获取当前本地时区时间（<see cref="DateTime.Now"/>）
    /// 2. 创建 <see cref="DateTimeOffset"/> 对象以保留时区信息
    /// 3. 转换为 Unix 时间戳（秒级精度）
    /// 
    /// 与 <see cref="UnixTimeSeconds"/> 方法的区别：
    /// - 本方法基于本地时区时间计算
    /// - <see cref="UnixTimeSeconds"/> 基于 UTC 时间计算
    /// 
    /// 适用场景：需要考虑本地时区的时间戳生成，如用户界面显示、本地日志记录等。
    /// </remarks>
    /// <example>
    /// <code>
    /// // 获取当前本地时区的时间戳
    /// long localTimestamp = TimerHelper.NowTimeSeconds();
    /// Console.WriteLine($"本地时区时间戳（秒）: {localTimestamp}");
    /// 
    /// // 与UTC时间戳对比
    /// long utcTimestamp = TimerHelper.UnixTimeSeconds();
    /// long timezoneOffset = localTimestamp - utcTimestamp;
    /// Console.WriteLine($"时区偏移（秒）: {timezoneOffset}");
    /// </code>
    /// </example>
    /// <seealso cref="UnixTimeSeconds"/>
    /// <seealso cref="NowTimeMilliseconds"/>
    /// <seealso cref="EpochLocal"/>
    public static long NowTimeSeconds()
    {
        return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 获取当前本地时区时间的 Unix 时间戳（毫秒级精度）。
    /// </summary>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示从 Unix 纪元（1970-01-01 00:00:00 UTC）到当前本地时间的毫秒数。
    /// </returns>
    /// <remarks>
    /// 此方法执行以下步骤：
    /// 1. 获取当前本地时区时间（<see cref="DateTime.Now"/>）
    /// 2. 创建 <see cref="DateTimeOffset"/> 对象以保留时区信息
    /// 3. 转换为 Unix 时间戳（毫秒级精度）
    /// 
    /// 与 <see cref="UnixTimeMilliseconds"/> 方法的区别：
    /// - 本方法基于本地时区时间计算
    /// - <see cref="UnixTimeMilliseconds"/> 基于 UTC 时间计算
    /// 
    /// 毫秒级精度适用于：
    /// - 高精度时间计算和比较
    /// - 性能监控和基准测试
    /// - 需要精确时间差计算的场景
    /// </remarks>
    /// <example>
    /// <code>
    /// // 获取当前本地时区的毫秒时间戳
    /// long localTimestamp = TimerHelper.NowTimeMilliseconds();
    /// Console.WriteLine($"本地时区时间戳（毫秒）: {localTimestamp}");
    /// 
    /// // 计算代码执行时间
    /// long startTime = TimerHelper.NowTimeMilliseconds();
    /// // ... 执行某些操作
    /// long endTime = TimerHelper.NowTimeMilliseconds();
    /// Console.WriteLine($"执行时间: {endTime - startTime} 毫秒");
    /// </code>
    /// </example>
    /// <seealso cref="UnixTimeMilliseconds"/>
    /// <seealso cref="NowTimeSeconds"/>
    /// <seealso cref="EpochLocal"/>
    public static long NowTimeMilliseconds()
    {
        return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
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

    /// <summary>
    /// 将 Unix 时间戳（秒级）转换为 .NET 刻度数（Ticks）。
    /// </summary>
    /// <param name="timestampSeconds">Unix 时间戳，表示从 1970年1月1日 00:00:00 UTC 以来经过的秒数。</param>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示从公元1年1月1日 00:00:00 以来的刻度数。
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 当 <paramref name="timestampSeconds"/> 超出 <see cref="DateTime"/> 有效范围时抛出此异常。
    /// 有效范围：-62135596800 到 253402300799 秒。
    /// </exception>
    /// <remarks>
    /// 此方法执行以下转换：
    /// 1. 验证时间戳是否在 <see cref="DateTime"/> 的有效范围内
    /// 2. 将 Unix 时间戳转换为 .NET 刻度数
    /// 3. 使用 <see cref="EpochUtc"/> 作为基准点进行计算
    /// 
    /// 转换公式：刻度数 = timestampSeconds × 10,000,000 + EpochUtc.Ticks
    /// 
    /// .NET 刻度数说明：
    /// - 1 刻度 = 100 纳秒
    /// - 1 秒 = 10,000,000 刻度（<see cref="TimeSpan.TicksPerSecond"/>）
    /// - 刻度数从公元1年1月1日 00:00:00 开始计算
    /// 
    /// 适用场景：
    /// - 将 Unix 时间戳转换为 .NET DateTime 对象
    /// - 高精度时间计算和比较
    /// - 时间数据的序列化和反序列化
    /// </remarks>
    /// <example>
    /// <code>
    /// // 转换当前时间戳
    /// long currentTimestamp = TimerHelper.UnixTimeSeconds();
    /// long ticks = TimerHelper.TimestampToTicks(currentTimestamp);
    /// DateTime dateTime = new DateTime(ticks);
    /// Console.WriteLine($"转换后的时间: {dateTime}");
    /// 
    /// // 转换特定时间戳
    /// long timestamp = 1609459200; // 2021-01-01 00:00:00 UTC
    /// long ticksValue = TimerHelper.TimestampToTicks(timestamp);
    /// DateTime specificDate = new DateTime(ticksValue);
    /// Console.WriteLine($"2021年元旦: {specificDate}");
    /// 
    /// // 处理边界值
    /// try
    /// {
    ///     long invalidTimestamp = long.MaxValue;
    ///     TimerHelper.TimestampToTicks(invalidTimestamp); // 抛出异常
    /// }
    /// catch (ArgumentOutOfRangeException ex)
    /// {
    ///     Console.WriteLine($"时间戳超出范围: {ex.Message}");
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="TimestampMillisToTicks"/>
    /// <seealso cref="EpochUtc"/>
    /// <seealso cref="TimeSpan.TicksPerSecond"/>
    /// <seealso cref="DateTime"/>
    public static long TimestampToTicks(long timestampSeconds)
    {
        if (timestampSeconds < -62135596800L || timestampSeconds > 253402300799L)
        {
            throw new ArgumentOutOfRangeException(nameof(timestampSeconds), "Timestamp is out of valid range for DateTime conversion.");
        }

        // 将Unix时间戳转换为刻度数，每秒等于10000000刻度
        // 使用TimeHelper.EpochUtc.Ticks确保与项目中其他时间计算保持一致
        return timestampSeconds * TimeSpan.TicksPerSecond + EpochUtc.Ticks;
    }

    /// <summary>
    /// 将 Unix 时间戳（毫秒级）转换为 .NET 刻度数（Ticks）。
    /// </summary>
    /// <param name="timestampMillisSeconds">Unix 毫秒时间戳，表示从 1970年1月1日 00:00:00 UTC 以来经过的毫秒数。</param>
    /// <returns>
    /// 返回一个 <see cref="long"/> 值，表示从公元1年1月1日 00:00:00 以来的刻度数。
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 当 <paramref name="timestampMillisSeconds"/> 超出 <see cref="DateTime"/> 有效范围时抛出此异常。
    /// 有效范围：-62135596800000 到 253402300799999 毫秒。
    /// </exception>
    /// <remarks>
    /// 此方法执行以下转换：
    /// 1. 验证毫秒时间戳是否在 <see cref="DateTime"/> 的有效范围内
    /// 2. 将 Unix 毫秒时间戳转换为 .NET 刻度数
    /// 3. 使用 <see cref="EpochUtc"/> 作为基准点进行计算
    /// 
    /// 转换公式：刻度数 = timestampMillisSeconds × 10,000 + EpochUtc.Ticks
    /// 
    /// .NET 刻度数说明：
    /// - 1 刻度 = 100 纳秒
    /// - 1 毫秒 = 10,000 刻度（<see cref="TimeSpan.TicksPerMillisecond"/>）
    /// - 刻度数从公元1年1月1日 00:00:00 开始计算
    /// 
    /// 与 <see cref="TimestampToTicks"/> 的区别：
    /// - 本方法处理毫秒级精度的时间戳
    /// - <see cref="TimestampToTicks"/> 处理秒级精度的时间戳
    /// - 毫秒级提供更高的时间精度
    /// 
    /// 适用场景：
    /// - 高精度时间戳转换
    /// - JavaScript 时间戳转换（JavaScript 使用毫秒时间戳）
    /// - 需要精确时间计算的场景
    /// </remarks>
    /// <example>
    /// <code>
    /// // 转换当前毫秒时间戳
    /// long currentMillisTimestamp = TimerHelper.UnixTimeMilliseconds();
    /// long ticks = TimerHelper.TimestampMillisToTicks(currentMillisTimestamp);
    /// DateTime dateTime = new DateTime(ticks);
    /// Console.WriteLine($"转换后的时间: {dateTime:yyyy-MM-dd HH:mm:ss.fff}");
    /// 
    /// // 转换JavaScript时间戳
    /// long jsTimestamp = 1609459200000; // 2021-01-01 00:00:00.000 UTC
    /// long ticksValue = TimerHelper.TimestampMillisToTicks(jsTimestamp);
    /// DateTime jsDate = new DateTime(ticksValue);
    /// Console.WriteLine($"JavaScript时间: {jsDate}");
    /// 
    /// // 精度对比
    /// long secondsTimestamp = 1609459200; // 秒级
    /// long millisTimestamp = 1609459200123; // 毫秒级
    /// 
    /// DateTime fromSeconds = new DateTime(TimerHelper.TimestampToTicks(secondsTimestamp));
    /// DateTime fromMillis = new DateTime(TimerHelper.TimestampMillisToTicks(millisTimestamp));
    /// 
    /// Console.WriteLine($"秒级精度: {fromSeconds:yyyy-MM-dd HH:mm:ss.fff}");
    /// Console.WriteLine($"毫秒级精度: {fromMillis:yyyy-MM-dd HH:mm:ss.fff}");
    /// </code>
    /// </example>
    /// <seealso cref="TimestampToTicks"/>
    /// <seealso cref="EpochUtc"/>
    /// <seealso cref="TimeSpan.TicksPerMillisecond"/>
    /// <seealso cref="DateTime"/>
    public static long TimestampMillisToTicks(long timestampMillisSeconds)
    {
        if (timestampMillisSeconds < -62135596800000L || timestampMillisSeconds > 253402300799999L)
        {
            throw new ArgumentOutOfRangeException(nameof(timestampMillisSeconds), "Timestamp is out of valid range for DateTime conversion.");
        }

        // 将Unix毫秒时间戳转换为刻度数，每毫秒等于10000刻度
        // 使用TimeHelper.EpochUtc.Ticks确保与项目中其他时间计算保持一致
        return timestampMillisSeconds * TimeSpan.TicksPerMillisecond + EpochUtc.Ticks;
    }

    /// <summary>
    /// 判断两个 <see cref="DateTime"/> 对象是否表示同一天。
    /// </summary>
    /// <param name="time1">要比较的第一个时间。例如：2024-01-10 14:30:00</param>
    /// <param name="time2">要比较的第二个时间。例如：2024-01-10 18:45:00</param>
    /// <returns>
    /// 如果两个时间是同一天，则返回 <c>true</c>；否则返回 <c>false</c>。
    /// </returns>
    /// <remarks>
    /// 此方法执行以下比较逻辑：
    /// 1. 使用 <see cref="DateTime.Date"/> 属性获取日期部分（忽略时间部分）
    /// 2. 分别比较年、月、日三个组成部分
    /// 3. 只有当年、月、日都相同时才返回 <c>true</c>
    /// 
    /// 重要特性：
    /// - 忽略具体的时、分、秒、毫秒等时间部分
    /// - 忽略时区差异，直接使用 <see cref="DateTime"/> 中存储的日期值
    /// - 不进行时区转换，基于原始 <see cref="DateTime"/> 值进行比较
    /// 
    /// 性能优化：
    /// - 使用直接的整数比较（Year、Month、Day）
    /// - 避免创建新的 <see cref="DateTime"/> 对象
    /// - 比使用 <c>time1.Date == time2.Date</c> 更高效
    /// 
    /// 适用场景：
    /// - 日程安排和事件管理
    /// - 日志按日期分组
    /// - 统计同一天的数据
    /// - 用户界面中的日期筛选
    /// </remarks>
    /// <example>
    /// <code>
    /// // 同一天的不同时间
    /// DateTime morning = new DateTime(2024, 1, 10, 8, 30, 0);
    /// DateTime evening = new DateTime(2024, 1, 10, 20, 45, 30);
    /// bool sameDay1 = TimerHelper.IsSameDay(morning, evening);
    /// Console.WriteLine($"早晨和晚上是同一天: {sameDay1}"); // True
    /// 
    /// // 不同天的时间
    /// DateTime today = new DateTime(2024, 1, 10, 23, 59, 59);
    /// DateTime tomorrow = new DateTime(2024, 1, 11, 0, 0, 1);
    /// bool sameDay2 = TimerHelper.IsSameDay(today, tomorrow);
    /// Console.WriteLine($"今天和明天是同一天: {sameDay2}"); // False
    /// 
    /// // 跨年比较
    /// DateTime lastYear = new DateTime(2023, 12, 31, 12, 0, 0);
    /// DateTime thisYear = new DateTime(2024, 1, 1, 12, 0, 0);
    /// bool sameDay3 = TimerHelper.IsSameDay(lastYear, thisYear);
    /// Console.WriteLine($"跨年日期是同一天: {sameDay3}"); // False
    /// 
    /// // 实际应用：按日期分组日志
    /// List&lt;DateTime&gt; logTimes = new List&lt;DateTime&gt;
    /// {
    ///     new DateTime(2024, 1, 10, 9, 0, 0),
    ///     new DateTime(2024, 1, 10, 15, 30, 0),
    ///     new DateTime(2024, 1, 11, 10, 0, 0)
    /// };
    /// 
    /// DateTime targetDate = new DateTime(2024, 1, 10);
    /// var sameDayLogs = logTimes.Where(log =&gt; TimerHelper.IsSameDay(log, targetDate)).ToList();
    /// Console.WriteLine($"2024-01-10 的日志数量: {sameDayLogs.Count}"); // 2
    /// </code>
    /// </example>
    /// <seealso cref="DateTime.Date"/>
    /// <seealso cref="DateTime.Year"/>
    /// <seealso cref="DateTime.Month"/>
    /// <seealso cref="DateTime.Day"/>
    public static bool IsSameDay(DateTime time1, DateTime time2)
    {
        return time1.Date.Year == time2.Date.Year && time1.Date.Month == time2.Date.Month && time1.Date.Day == time2.Date.Day;
    }
}