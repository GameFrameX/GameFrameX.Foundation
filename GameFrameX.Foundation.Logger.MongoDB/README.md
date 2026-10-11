# GameFrameX.Foundation.Logger.MongoDB

`GameFrameX.Foundation.Logger` 的 MongoDB 日志 sink 组件包，通过 `UseMongoDb` 扩展方法将日志写入 MongoDB capped 集合。自 C77 起从核心包 `LogOptions` 拆分而来，核心包不再依赖 MongoDB.Driver / Serilog.Sinks.MongoDB。

## 特性

- ✅ **开箱即用**: 一行 `UseMongoDb("mongodb://...")` 即可启用 MongoDB 日志输出
- ✅ **复用宿主连接**: 支持注入外部 `IMongoDatabase`，复用宿主已配置的连接池 / TLS / 凭证
- ✅ **capped 集合**: 支持配置集合最大体积与最大文档数，自动环形覆盖旧日志
- ✅ **完整适配钩子**: 通过 `ConfigureSink` 直接访问底层 `MongoDBSinkConfiguration`（TTL、批量参数等）
- ✅ **脱敏输出**: 连接串不会出现在选项的字符串表示中，避免凭证泄漏

## 安装

```bash
dotnet add package GameFrameX.Foundation.Logger.MongoDB
```

## 快速开始

```csharp
using GameFrameX.Foundation.Logger;
using GameFrameX.Foundation.Logger.MongoDB;

// 通过连接串启用
var logOptions = new LogOptions().UseMongoDb("mongodb://127.0.0.1:27017/logs");
var logger = LogHandler.Create(logOptions);

LogHelper.Info("应用程序启动");
```

### 迁移示例（旧 → 新）

```csharp
// 旧写法（C77 之前，核心包内置 sink）
var oldOptions = new LogOptions
{
    IsWriteToMongoDb = true,
    MongoDbDatabaseUrl = "mongodb://127.0.0.1:27017/logs"
};

// 新写法（安装本组件包后）
var newOptions = new LogOptions().UseMongoDb("mongodb://127.0.0.1:27017/logs");
```

### 完整配置

```csharp
var logOptions = new LogOptions().UseMongoDb(sinkOptions =>
{
    sinkOptions.DatabaseUrl = "mongodb://127.0.0.1:27017/logs";
    sinkOptions.CollectionName = "log";
    sinkOptions.CappedMaxSizeMb = 100;
    sinkOptions.CappedMaxDocuments = 100000;
    sinkOptions.ConfigureSink = cfg => cfg.SetExpireTTL(TimeSpan.FromDays(7));
});
```

## 配置选项

### MongoDbSinkOptions 配置类

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `DatabaseUrl` | `string` | 空字符串 | MongoDB 连接串；需与 `Database` 至少配置其一，否则装配时抛出异常 |
| `Database` | `IMongoDatabase` | `null` | 外部注入的数据库实例；非空时优先于 `DatabaseUrl`，复用宿主连接池 / TLS / 凭证 |
| `CollectionName` | `string` | `"log"` | 日志集合名称 |
| `CappedMaxSizeMb` | `long` | `50` | capped 集合的最大总大小（MB） |
| `CappedMaxDocuments` | `long?` | `50000` | capped 集合的最大文档数 |
| `RollingInterval` | `RollingInterval?` | `null` | 日志滚动间隔；为 null 时跟随宿主 `LogOptions.RollingInterval` |
| `RestrictedToMinimumLevel` | `LogEventLevel?` | `null` | 该 sink 的最低日志级别；为 null 时跟随宿主 `LogOptions.LogEventLevel` |
| `ConfigureSink` | `Action<MongoDBSinkConfiguration>` | `null` | 底层配置的完整适配钩子（TTL、批量周期、批量条数、InsertManyOptions 等） |

## 依赖项

- **GameFrameX.Foundation.Logger** - 核心日志包
- **Serilog.Sinks.MongoDB** - MongoDB sink 实现
- **MongoDB.Driver** - MongoDB 驱动

## 许可证

本项目遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。

## 贡献

欢迎提交Issue和Pull Request来改进这个项目。

## 更多信息

- 项目主页: https://github.com/GameFrameX
- 文档地址: https://gameframex.doc.alianblank.com
- 作者: AlianBlank, Blank
