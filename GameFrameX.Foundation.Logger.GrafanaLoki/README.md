# GameFrameX.Foundation.Logger.GrafanaLoki

`GameFrameX.Foundation.Logger` 的 Grafana Loki 日志 sink 组件包，通过 `UseGrafanaLoki` 扩展方法将日志批量推送到 Grafana Loki 进行集中化日志管理。自 C77 起从核心包 `LogOptions` 拆分而来，核心包不再依赖 Serilog.Sinks.Grafana.Loki。

## 特性

- ✅ **开箱即用**: 一行 `UseGrafanaLoki("http://loki:3100")` 即可启用 Loki 日志输出
- ✅ **标签与附加属性**: 支持为日志事件附加 Loki 标签与全局属性
- ✅ **Basic 认证**: 支持用户名 / 密码认证
- ✅ **批量与背压**: 可配置批量条数、内存队列上限与推送周期，队列写满后丢弃新日志防止内存无限增长
- ✅ **Gzip 压缩**: 默认启用载荷压缩，减少网络传输量
- ✅ **脱敏输出**: 密码在选项字符串表示中固定输出为 "***"

## 安装

```bash
dotnet add package GameFrameX.Foundation.Logger.GrafanaLoki
```

## 快速开始

```csharp
using GameFrameX.Foundation.Logger;
using GameFrameX.Foundation.Logger.GrafanaLoki;

// 通过地址启用
var logOptions = new LogOptions().UseGrafanaLoki("http://loki:3100");
var logger = LogHandler.Create(logOptions);

LogHelper.Info("应用程序启动");
```

### 迁移示例（旧 → 新）

```csharp
// 旧写法（C77 之前，核心包内置 sink）
var oldOptions = new LogOptions
{
    IsGrafanaLoki = true,
    GrafanaLokiUrl = "http://loki.example.com:3100",
    GrafanaLokiLabels = new Dictionary<string, string>
    {
        ["app"] = "myapp",
        ["env"] = "production"
    },
    GrafanaLokiUsername = "admin",
    GrafanaLokiPassword = "password"
};

// 新写法（安装本组件包后）
var newOptions = new LogOptions().UseGrafanaLoki(o =>
{
    o.Url = "http://loki.example.com:3100";
    o.Labels = new Dictionary<string, string>
    {
        ["app"] = "myapp",
        ["env"] = "production"
    };
    o.UserName = "admin";
    o.Password = "password";
});
```

## 配置选项

### GrafanaLokiSinkOptions 配置类

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `Url` | `string` | `http://localhost:3100` | Grafana Loki 服务地址 |
| `Labels` | `Dictionary<string, string>` | 空 | 转换为 sink 的 Loki 标签；为 null 时按空标签处理 |
| `Properties` | `Dictionary<string, string>` | 空 | 附加属性，非空键值对将 Enrich 到全部日志事件；键或值为空白的条目会被跳过 |
| `UserName` | `string` | `null` | Basic 认证用户名；与密码任一为空时不启用认证 |
| `Password` | `string` | `null` | Basic 认证密码；敏感字段，字符串表示中固定脱敏为 "***" |
| `CompressionEnabled` | `bool` | `true` | 是否启用 Gzip 载荷压缩 |
| `BatchSizeLimit` | `int` | `1000` | 单次批量推送的最大日志条数；攒满即触发推送 |
| `QueueLimit` | `int` | `100000` | 内存队列最大长度；写满后新日志将被丢弃（背压保护） |
| `Period` | `int` | `2` | 批量推送周期（秒）；传 0 或负值强制回退为默认 2 秒 |
| `RestrictedToMinimumLevel` | `LogEventLevel?` | `null` | 该 sink 的最低日志级别；为 null 时跟随宿主 `LogOptions.LogEventLevel` |

## 依赖项

- **GameFrameX.Foundation.Logger** - 核心日志包
- **Serilog.Sinks.Grafana.Loki** - Grafana Loki sink 实现

## 许可证

本项目遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。

## 贡献

欢迎提交Issue和Pull Request来改进这个项目。

## 更多信息

- 项目主页: https://github.com/GameFrameX
- 文档地址: https://gameframex.doc.alianblank.com
- 作者: AlianBlank, Blank
