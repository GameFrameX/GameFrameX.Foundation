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
    /// 默认构造函数，初始化日志配置对象。
    /// </summary>
    public LogOptions(string logPathName = "logs")
    {
        if (string.IsNullOrEmpty(logPathName.Trim()))
        {
            logPathName = "logs";
        }

        LogSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", logPathName);
    }

    /// <summary>
    /// 默认配置对象，提供一个默认的日志配置实例。
    /// </summary>
    /// <remarks>
    /// 使用此静态实例可以快速获取一个包含默认设置的日志配置对象。
    /// </remarks>
    public static readonly LogOptions Default = new LogOptions();

    /// <summary>
    /// 是否写入文件，默认为 true。
    /// </summary>
    public bool IsWriteToFile { get; set; } = true;

    /// <summary>
    /// 服务器类型，用于标识日志来源的服务器类型。
    /// </summary>
    /// <remarks>
    /// 可以用来区分不同服务器产生的日志，便于日志的分类和管理。
    /// </remarks>
    public string LogType { get; set; }

    /// <summary>
    /// 日志标签名，用于标识日志的名称或描述。
    /// </summary>
    /// <remarks>
    /// 可以用来区分不同服务器产生的日志，便于日志的分类和管理。
    /// </remarks>
    public string LogTagName { get; set; } = "";

    /// <summary>
    /// 是否写入数据库，默认为 false。
    /// </summary>
    public bool IsWriteToMongoDb { get; set; } = false;

    /// <summary>
    /// MongoDB 数据库连接地址，用于保存日志数据
    /// </summary>
    /// <remarks>
    /// <para>
    /// 当 <see cref="IsWriteToMongoDb"/> 设置为 true 时，此属性必须配置有效的 MongoDB 连接字符串。
    /// 连接字符串应包含数据库服务器地址、端口、数据库名称和认证信息。
    /// </para>
    /// <para>
    /// 连接字符串格式：mongodb://[username:password@]host[:port]/database[?options]
    /// </para>
    /// <para>
    /// 常用配置示例：
    /// <list type="bullet">
    /// <item><description>本地无认证：mongodb://localhost:27017/gameserver</description></item>
    /// <item><description>本地有认证：mongodb://user:password@localhost:27017/gameserver?authSource=admin</description></item>
    /// <item><description>远程服务器：mongodb://user:password@192.168.1.100:27017/gameserver?authSource=admin</description></item>
    /// <item><description>副本集：mongodb://user:password@host1:27017,host2:27017/gameserver?replicaSet=rs0&amp;authSource=admin</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// 注意：确保 MongoDB 服务器已启动且网络连接正常，否则日志写入将失败。
    /// </para>
    /// </remarks>
    /// <value>默认值为本地 MongoDB 实例：mongodb://127.0.0.1:27017/gameserver?authSource=admin</value>
    public string MongoDbDatabaseUrl { get; set; } = "mongodb://127.0.0.1:27017/gameserver?authSource=admin";

    /// <summary>
    /// mongodb 创建的上限集合的最大总大小（MB）
    /// </summary>
    public int MongoDbCappedMaxSizeMb { get; set; } = 50;

    /// <summary>
    /// mongodb 创建的上限集合的最大文档数。
    /// </summary>
    public int MongoDbCappedMaxDocuments { get; set; } = 50000;

    /// <summary>
    /// 日志存储路径，为 应用程序运行目录下的子目录/logs。
    /// </summary>
    /// <remarks>
    /// 日志文件的存储位置，是绝对路径。
    /// </remarks>
    public string LogSavePath { get; private set; }

    /// <summary>
    /// 日志文件名，为空时使用默认名称。
    /// </summary>
    public string LogFileName { get; set; } = string.Empty;

    /// <summary>
    /// 是否输出到控制台，默认为 true。
    /// </summary>
    /// <remarks>
    /// 控制日志是否同时在控制台显示，便于开发调试。
    /// </remarks>
    public bool IsConsole { get; set; } = true;

    /// <summary>
    /// 是否输出到 GrafanaLoki，默认为 false。
    /// </summary>
    public bool IsGrafanaLoki { get; set; } = false;

    /// <summary>
    /// GrafanaLoki 服务地址，默认为 http://localhost:3100。
    /// </summary>
    public string GrafanaLokiUrl { get; set; } = "http://localhost:3100";

    /// <summary>
    /// GrafanaLoki 标签
    /// </summary>
    public Dictionary<string, string> GrafanaLokiLabels { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// GrafanaLoki 其他属性
    /// </summary>
    public Dictionary<string, string> GrafanaLokiProperty { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// GrafanaLoki 用户名
    /// </summary>
    public string GrafanaLokiUserName { get; set; }

    /// <summary>
    /// GrafanaLoki 密码
    /// </summary>
    public string GrafanaLokiPassword { get; set; }

    /// <summary>
    /// 是否启用 GrafanaLoki 数据压缩，默认为 true
    /// </summary>
    /// <remarks>
    /// 启用压缩可以减少网络传输的数据量，提高传输效率
    /// </remarks>
    public bool GrafanaLokiCompressionEnabled { get; set; } = true;

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
    /// 控制台日志输出格式模板，默认格式为 "[时:分:秒 级别][标签名]消息内容"。
    /// </summary>
    /// <remarks>
    /// 支持的占位符包括：
    /// - {Timestamp:HH:mm:ss} - 时间戳（时:分:秒格式）
    /// - {Level:u3} - 日志级别（3个字符大写）
    /// - {TagName} - 日志标签名称
    /// - {Message:lj} - 日志消息内容（左对齐）
    /// - {NewLine} - 换行符
    /// - {Exception} - 异常信息
    /// </remarks>
    public string ConsoleOutputTemplate { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}][{TagName}]{Message:lj}{NewLine}{Exception}";

    /// <summary>
    /// 文件日志输出格式模板，默认格式为 "完整时间戳 [级别][友好名称] 消息内容"。
    /// </summary>
    /// <remarks>
    /// 支持的占位符包括：
    /// - {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} - 完整时间戳（包含毫秒和时区）
    /// - {Level:u3} - 日志级别（3个字符大写）
    /// - {TagName} - 日志标签名称
    /// - {Message:lj} - 日志消息内容（左对齐）
    /// - {NewLine} - 换行符
    /// - {Exception} - 异常信息
    /// </remarks>
    public string FileOutputTemplate { get; set; } = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}][{TagName}]{Message:lj}{NewLine}{Exception}";

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