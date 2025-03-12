using GameFrameX.Foundation.Json;
using Serilog;
using Serilog.Events;

namespace GameFrameX.Foundation.Logger;
/// <summary>
/// 日志配置类，用于配置日志的相关选项。
/// </summary>
/// <remarks>
/// 该类提供了一系列日志相关的配置选项，包括日志存储路径、输出级别、文件大小限制等。
/// 通过这些配置可以灵活控制日志的输出行为和存储方式。
/// </remarks>
public sealed class LogOptions
{
    /// <summary>
    /// 默认配置对象，提供一个默认的日志配置实例。
    /// </summary>
    /// <remarks>
    /// 使用此静态实例可以快速获取一个包含默认设置的日志配置对象。
    /// </remarks>
    public static readonly LogOptions Default = new LogOptions();

    /// <summary>
    /// 服务器类型，用于标识日志来源的服务器类型。
    /// </summary>
    /// <remarks>
    /// 可以用来区分不同服务器产生的日志，便于日志的分类和管理。
    /// </remarks>
    public string LogType { get; set; }

    /// <summary>
    /// 日志存储路径，默认为 ./logs。
    /// </summary>
    /// <remarks>
    /// 指定日志文件的存储位置，可以是相对路径或绝对路径。
    /// </remarks>
    public string LogSavePath { get; set; } = "./logs/";

    /// <summary>
    /// 是否输出到控制台，默认为 true。
    /// </summary>
    /// <remarks>
    /// 控制日志是否同时在控制台显示，便于开发调试。
    /// </remarks>
    public bool IsConsole { get; set; } = true;

    /// <summary>
    /// 日志滚动间隔，默认为每天（Day）。
    /// </summary>
    /// <remarks>
    /// 决定日志文件创建新文件的时间间隔，可以是小时、天、月等。
    /// </remarks>
    public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;

    /// <summary>
    /// 日志输出级别，默认为 Debug。
    /// </summary>
    /// <remarks>
    /// 控制日志输出的最低级别，低于此级别的日志将不会被记录。
    /// </remarks>
    public LogEventLevel LogEventLevel { get; set; } = LogEventLevel.Debug;

    /// <summary>
    /// 是否限制单个文件大小，默认为 true。
    /// </summary>
    /// <remarks>
    /// 启用此选项可以防止单个日志文件过大。
    /// </remarks>
    public bool IsFileSizeLimit { get; set; } = true;

    /// <summary>
    /// 日志单个文件大小限制，默认为 100MB。
    /// 当 IsFileSizeLimit 为 true 时有效。
    /// </summary>
    /// <remarks>
    /// 当日志文件达到此大小限制时，将创建新的日志文件继续写入。
    /// </remarks>
    public int FileSizeLimitBytes { get; set; } = 100 * 1024 * 1024;

    /// <summary>
    /// 日志文件保留数量限制 默认为 31 个文件,即 31 天的日志文件
    /// 当 设置值为 null 时不限制文件数量
    /// </summary>
    /// <remarks>
    /// 用于控制历史日志文件的数量，防止占用过多磁盘空间。
    /// </remarks>
    public int? RetainedFileCountLimit { get; set; } = 31;

    /// <summary>
    /// 返回日志配置对象的 JSON 字符串表示形式。
    /// </summary>
    /// <returns>JSON 字符串表示形式。</returns>
    /// <remarks>
    /// 将当前配置对象序列化为JSON格式，便于配置的存储和传输。
    /// </remarks>
    public override string ToString()
    {
        return JsonHelper.SerializeFormat(this);
    }
}