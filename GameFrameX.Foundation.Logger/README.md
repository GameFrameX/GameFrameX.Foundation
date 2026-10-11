# GameFrameX.Foundation.Logger

一个基于Serilog的高性能日志记录库，为GameFrameX框架提供统一的日志记录接口和丰富的日志输出功能。

## 特性

- ✅ **多级别日志支持**: 支持Verbose、Debug、Info、Warn、Error、Fatal六个日志级别
- ✅ **多输出目标**: 支持文件、控制台等多种输出方式；MongoDB / Grafana Loki 通过外部 Sink 组件包接入
- ✅ **灵活配置**: 通过LogOptions类提供丰富的配置选项
- ✅ **文件滚动**: 支持按时间间隔和文件大小进行日志文件滚动
- ✅ **标签支持**: 支持为日志添加标签，便于分类和过滤
- ✅ **异常记录**: 专门的异常记录方法，包含完整的堆栈跟踪信息
- ✅ **控制台输出**: 支持同时输出到日志文件和控制台
- ✅ **高性能**: 基于Serilog构建，提供高性能的日志记录能力

## 快速开始

### 1. 基本使用

```csharp
using GameFrameX.Foundation.Logger;

// 使用默认配置初始化日志系统
var logger = LogHandler.Create(LogOptions.Default);

// 记录不同级别的日志
LogHelper.Info("应用程序启动");
LogHelper.Warn("这是一个警告消息");
LogHelper.Error("发生了一个错误");
```

### 2. 自定义配置

```csharp
using GameFrameX.Foundation.Logger;

// 创建自定义日志配置
var logOptions = new LogOptions("mylogs")
{
    LogType = "WebApi",
    LogTagName = "Production",
    LogEventLevel = LogEventLevel.Information,
    IsConsole = true,
    RollingInterval = RollingInterval.Hour,
    FileSizeLimitBytes = 50 * 1024 * 1024, // 50MB
    RetainedFileCountLimit = 7 // 保留7个文件
};

// 初始化日志系统
var logger = LogHandler.Create(logOptions);

// 使用日志
LogHelper.Info("服务器", "服务器启动在端口 {Port}", 8080);
LogHelper.InfoConsole("同时输出到文件和控制台的消息");
```

## 详细使用指南

### 日志级别

支持六个标准的日志级别：

```csharp
// Verbose - 最详细的日志信息
LogHelper.Verbose("详细的调试信息");

// Debug - 调试信息
LogHelper.Debug("调试信息: 变量值 = {Value}", someValue);

// Information - 一般信息
LogHelper.Info("用户 {UserId} 登录成功", userId);

// Warning - 警告信息
LogHelper.Warn("磁盘空间不足，剩余: {FreeSpace}MB", freeSpace);

// Error - 错误信息
LogHelper.Error("数据库连接失败: {Error}", errorMessage);

// Fatal - 致命错误
LogHelper.Fatal("应用程序即将崩溃: {Reason}", reason);
```

### 异常记录

专门的异常记录方法，自动包含堆栈跟踪：

```csharp
try
{
    // 可能抛出异常的代码
    DoSomething();
}
catch (Exception ex)
{
    // 记录异常
    LogHelper.Error(ex);
    
    // 带标签的异常记录
    LogHelper.Error("数据库", ex);
    
    // 自定义异常消息
    LogHelper.Error("处理用户请求时发生错误: {Message}", ex.Message);
}
```

### 标签支持

为日志添加标签，便于分类和过滤：

```csharp
// 带标签的日志记录
LogHelper.Info("用户管理", "用户 {UserId} 创建成功", userId);
LogHelper.Warn("安全", "检测到可疑登录尝试，IP: {IP}", ipAddress);
LogHelper.Error("支付", "支付处理失败，订单号: {OrderId}", orderId);

// 带标签的控制台输出
LogHelper.InfoConsole("启动", "服务器启动完成，监听端口: {Port}", port);
```

### 控制台输出

支持同时输出到日志文件和控制台：

```csharp
// 仅输出到日志文件
LogHelper.Info("这条消息只会写入日志文件");

// 同时输出到日志文件和控制台
LogHelper.InfoConsole("这条消息会同时显示在控制台和日志文件中");

// 错误消息的控制台输出（红色显示）
LogHelper.ErrorConsole("这是一个错误消息，控制台中会以红色显示");

// 仅输出到控制台（不写入日志文件）
LogHelper.Console("这条消息只会显示在控制台");
```

## 配置选项

### LogOptions 配置类

```csharp
var logOptions = new LogOptions("logs") // 日志目录名
{
    // 基本配置
    LogType = "WebServer",              // 服务器类型标识
    LogTagName = "Production",          // 日志标签名
    LogEventLevel = LogEventLevel.Info, // 最低日志级别
    
    // 输出配置
    IsConsole = true,                   // 是否输出到控制台
    
    // 文件配置
    RollingInterval = RollingInterval.Day,    // 滚动间隔（天）
    IsFileSizeLimit = true,                   // 是否限制文件大小
    FileSizeLimitBytes = 100 * 1024 * 1024,   // 文件大小限制（100MB）
    RetainedFileCountLimit = 31 // 保留文件数量（31个）
};
```

### 滚动间隔选项

```csharp
// 支持的滚动间隔
RollingInterval.Infinite    // 不滚动
RollingInterval.Year        // 按年滚动
RollingInterval.Month       // 按月滚动
RollingInterval.Day         // 按天滚动（默认）
RollingInterval.Hour        // 按小时滚动
RollingInterval.Minute      // 按分钟滚动
```

### 日志级别配置

```csharp
// 支持的日志级别
LogEventLevel.Verbose       // 最详细
LogEventLevel.Debug         // 调试（默认）
LogEventLevel.Information   // 信息
LogEventLevel.Warning       // 警告
LogEventLevel.Error         // 错误
LogEventLevel.Fatal         // 致命错误
```

## 高级功能

### 外部 Sink 组件

自 C77 起，MongoDB 与 Grafana Loki sink 已从核心包拆分为独立的组件包，核心包仅保留 Serilog 控制台与文件输出。如需接入 MongoDB 或 Loki，请安装对应的组件包并通过扩展方法启用：

| 组件包 | 安装 | 启用方式 |
|--------|------|----------|
| `GameFrameX.Foundation.Logger.MongoDB` | `dotnet add package GameFrameX.Foundation.Logger.MongoDB` | `new LogOptions().UseMongoDb("mongodb://...")` |
| `GameFrameX.Foundation.Logger.GrafanaLoki` | `dotnet add package GameFrameX.Foundation.Logger.GrafanaLoki` | `new LogOptions().UseGrafanaLoki("http://loki:3100")` |

**迁移示例（旧 → 新）**：

```csharp
// 旧写法（C77 之前，核心包内置 sink）
var oldOptions = new LogOptions
{
    IsWriteToMongoDb = true,
    MongoDbDatabaseUrl = "mongodb://127.0.0.1:27017/logs"
};
var oldLokiOptions = new LogOptions
{
    IsGrafanaLoki = true,
    GrafanaLokiUrl = "http://loki:3100",
    GrafanaLokiLabels = new Dictionary<string, string> { ["app"] = "myapp" }
};

// 新写法（安装组件包后）
using GameFrameX.Foundation.Logger.MongoDB;
using GameFrameX.Foundation.Logger.GrafanaLoki;

var mongoOptions = new LogOptions().UseMongoDb("mongodb://127.0.0.1:27017/logs");
var lokiOptions = new LogOptions().UseGrafanaLoki("http://loki:3100");

// 或通过选项委托进行完整配置
var lokiOptions2 = new LogOptions().UseGrafanaLoki(o =>
{
    o.Url = "http://loki:3100";
    o.Labels = new Dictionary<string, string> { ["app"] = "myapp" };
});
```

此外，核心包提供通用注入点 `LogOptions.SinkConfigurators`（`IList<Action<LoggerConfiguration>>`），可用于注册任意的自定义 Serilog sink。

### 自定义日志配置

支持通过回调函数进行更高级的自定义配置：

```csharp
var logger = LogHandler.Create(logOptions, true, config =>
{
    // 添加自定义的Sink
    config.WriteTo.Email(
        fromEmail: "noreply@example.com",
        toEmail: "admin@example.com",
        outputTemplate: "{Timestamp} [{Level}] {Message}{NewLine}{Exception}",
        restrictedToMinimumLevel: LogEventLevel.Error
    );
    
    // 添加自定义的Enricher
    config.Enrich.WithProperty("MachineName", Environment.MachineName);
    config.Enrich.WithProperty("ProcessId", Environment.ProcessId);
});
```

### 使用自定义Logger实例

```csharp
// 创建多个Logger实例
var webLogger = LogHandler.Create(webLogOptions, false);
var dbLogger = LogHandler.Create(dbLogOptions, false);

// 使用特定的Logger实例
LogHelper.Info(webLogger, "Web请求处理完成");
LogHelper.Error(dbLogger, "数据库连接异常", exception);
```

## 性能优化

### 异步日志刷新

```csharp
// 同步刷新（阻塞）
LogHelper.FlushAndSave();

// 异步刷新（非阻塞）
LogHelper.CloseAndFlushAsync();
```

### 条件日志记录

```csharp
// 避免不必要的字符串格式化
if (logger.IsEnabled(LogEventLevel.Debug))
{
    LogHelper.Debug("复杂的调试信息: {Data}", ExpensiveOperation());
}
```

## 最佳实践

### 1. 结构化日志

使用结构化的日志消息，便于后续分析：

```csharp
// 好的做法 - 结构化日志
LogHelper.Info("用户登录成功，用户ID: {UserId}, IP: {IP}, 耗时: {Duration}ms", 
    userId, ipAddress, duration);

// 避免的做法 - 字符串拼接
LogHelper.Info($"用户登录成功，用户ID: {userId}, IP: {ipAddress}, 耗时: {duration}ms");
```

### 2. 合理使用日志级别

```csharp
// Debug - 开发调试信息
LogHelper.Debug("进入方法 ProcessOrder，参数: {OrderId}", orderId);

// Info - 重要的业务事件
LogHelper.Info("订单创建成功，订单号: {OrderId}, 用户: {UserId}", orderId, userId);

// Warn - 可恢复的问题
LogHelper.Warn("重试连接数据库，第 {Attempt} 次尝试", attemptCount);

// Error - 需要关注的错误
LogHelper.Error("处理支付失败，订单: {OrderId}, 错误: {Error}", orderId, error);

// Fatal - 导致应用程序终止的严重错误
LogHelper.Fatal("数据库连接池耗尽，应用程序即将关闭");
```

### 3. 使用标签分类

```csharp
// 按功能模块分类
LogHelper.Info("用户管理", "用户注册成功: {Email}", email);
LogHelper.Info("订单处理", "订单状态更新: {OrderId} -> {Status}", orderId, status);
LogHelper.Info("支付系统", "支付完成: {Amount} 元", amount);

// 按环境分类
LogHelper.Info("生产环境", "服务器启动完成");
LogHelper.Debug("开发环境", "调试信息: {Data}", debugData);
```

### 4. 异常处理

```csharp
try
{
    await ProcessOrderAsync(orderId);
    LogHelper.Info("订单处理", "订单 {OrderId} 处理完成", orderId);
}
catch (BusinessException ex)
{
    // 业务异常，记录为警告
    LogHelper.Warn("订单处理", "业务规则验证失败: {Message}", ex.Message);
    throw;
}
catch (Exception ex)
{
    // 系统异常，记录为错误
    LogHelper.Error("订单处理", ex);
    throw;
}
```

### 5. 配置管理

```csharp
// 开发环境配置
var devLogOptions = new LogOptions("logs")
{
    LogEventLevel = LogEventLevel.Debug,
    IsConsole = true,
    RollingInterval = RollingInterval.Hour
};

// 生产环境配置
var prodLogOptions = new LogOptions("logs")
{
    LogEventLevel = LogEventLevel.Information,
    IsConsole = false,
    RollingInterval = RollingInterval.Day
    // 如需将生产日志发送到 Grafana Loki，安装 GameFrameX.Foundation.Logger.GrafanaLoki
    // 并改为：new LogOptions("logs").UseGrafanaLoki("http://loki.prod.com:3100")
};

// 根据环境选择配置
var logOptions = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" 
    ? devLogOptions 
    : prodLogOptions;
```

## 故障排除

### 常见问题

1. **日志文件未创建**
    - 检查日志目录权限
    - 确认LogSavePath路径正确
    - 查看控制台是否有Serilog自诊断信息

2. **日志级别过滤不生效**
    - 确认LogEventLevel设置正确
    - 检查是否有多个Logger实例冲突

3. **Grafana Loki连接失败**
    - 确认已安装 GameFrameX.Foundation.Logger.GrafanaLoki 组件包并通过 UseGrafanaLoki 启用
    - 验证GrafanaLoki服务地址和端口
    - 检查网络连接和防火墙设置

### 调试信息

启用Serilog自诊断功能：

```csharp
// LogHandler会自动启用自诊断
// 诊断信息会输出到控制台，格式为: Serilog:SelfLog:{message}
```

## 依赖项

- **Serilog.AspNetCore** (9.0.0) - 核心日志框架
- **Serilog.Sinks.Console** (6.0.0) - 控制台输出
- **Serilog.Sinks.File** (7.0.0) - 文件输出
- **GameFrameX.Foundation.Json** - JSON序列化支持

## 许可证

本项目遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。

## 贡献

欢迎提交Issue和Pull Request来改进这个项目。

## 更多信息

- 项目主页: https://github.com/GameFrameX/GameFrameX
- 文档地址: https://gameframex.doc.alianblank.com
- 作者: AlianBlank, Blank