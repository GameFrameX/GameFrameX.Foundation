# 启动参数解析器

一个强大的命令行参数和环境变量解析库，支持将命令行参数和环境变量自动映射到强类型配置对象。

## 特性

- ✅ **参数优先级处理**: 命令行参数 > 环境变量 > 默认值
- ✅ **泛型支持**: 支持任意强类型配置类
- ✅ **多种启动方式兼容**: 支持Docker、exe、shell等启动方式
- ✅ **自动前缀处理**: 自动为参数添加`--`前缀
- ✅ **布尔参数支持**: 支持多种布尔参数格式
- ✅ **环境变量映射**: 自动映射环境变量到配置属性
- ✅ **类型转换**: 自动转换字符串参数到目标类型
- ✅ **特性支持**: 支持丰富的配置特性
- ✅ **智能调试模式**: 默认启用参数调试输出，便于部署时验证配置
- ✅ **环境感知**: 根据运行环境自动调整调试输出行为

## 快速开始

### 1. 定义配置类

```csharp
public class AppConfig
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    public bool Debug { get; set; } = false;
    public string LogLevel { get; set; } = "info";
    public double Timeout { get; set; } = 30.5;
}
```

### 2. 使用OptionsProvider（推荐）

```csharp
using GameFrameX.Foundation.Options;

class Program
{
    static void Main(string[] args)
    {
        // 初始化选项提供者
        OptionsProvider.Initialize(args);
        
        // 获取配置对象（默认启用调试输出）
        var config = OptionsProvider.GetOptions<AppConfig>();
        
        // 使用配置
        Console.WriteLine($"服务器: {config.Host}:{config.Port}");
        Console.WriteLine($"调试模式: {config.Debug}");
        Console.WriteLine($"日志级别: {config.LogLevel}");
        Console.WriteLine($"超时时间: {config.Timeout}秒");
    }
}
```

### 3. 使用OptionsBuilder（传统方式）

```csharp
using GameFrameX.Foundation.Options;

class Program
{
    static void Main(string[] args)
    {
        // 创建选项构建器
        var builder = new OptionsBuilder<AppConfig>(args);
        
        // 构建配置对象
        var config = builder.Build();
        
        // 使用配置
        Console.WriteLine($"服务器: {config.Host}:{config.Port}");
    }
}
```

## 智能调试模式

### 默认行为

从现在开始，**参数调试输出默认启用**，这意味着程序启动时会自动显示：

- 📋 原始命令行参数
- ⚙️ 所有可用选项定义
- 🔗 参数映射分析
- 📄 解析后的配置对象（包括JSON格式）

### 运行示例

```bash
myapp.exe --host example.com --port 9090 --debug
```

输出：

```
╔══════════════════════════════════════════════════════════════╗
║                    命令行参数解析调试信息                      ║
╚══════════════════════════════════════════════════════════════╝

📋 原始命令行参数:
   参数数量: 3
   [0] --host
   [1] example.com
   [2] --port
   [3] 9090
   [4] --debug

⚙️  可用选项定义:
   --host           : 服务器主机地址
     类型: 字符串, 必需: false
     默认值: localhost

   --port           : 服务器端口号
     类型: 整数, 必需: false
     默认值: 8080

🔗 参数映射分析:
   识别的选项:
   --host → host = example.com
   --port → port = 9090
   --debug → debug = <无值>

╔══════════════════════════════════════════════════════════════╗
║                    解析后的配置对象信息                        ║
╚══════════════════════════════════════════════════════════════╝

配置类型: AppConfig
属性数量: 5

  Debug                : true                           (布尔值)
  Host                 : "example.com"                  (字符串)
  LogLevel             : "info"                         (字符串)
  Port                 : 9090                           (整数)
  Timeout              : 30.5                           (浮点数)

📄 JSON格式表示:
{
  "Host": "example.com",
  "Port": 9090,
  "Debug": true,
  "LogLevel": "info",
  "Timeout": 30.5
}
```

### 控制调试输出

#### 1. 通过环境变量控制

```bash
# 禁用调试输出
export GAMEFRAMEX_OPTIONS_DEBUG=false
myapp.exe --host example.com

# 启用调试输出
export GAMEFRAMEX_OPTIONS_DEBUG=true
myapp.exe --host example.com

# 支持多种格式
export GAMEFRAMEX_OPTIONS_DEBUG=0        # 禁用
export GAMEFRAMEX_OPTIONS_DEBUG=no       # 禁用
export GAMEFRAMEX_OPTIONS_DEBUG=off      # 禁用
export GAMEFRAMEX_OPTIONS_DEBUG=disable  # 禁用

export GAMEFRAMEX_OPTIONS_DEBUG=1        # 启用
export GAMEFRAMEX_OPTIONS_DEBUG=yes      # 启用
export GAMEFRAMEX_OPTIONS_DEBUG=on       # 启用
export GAMEFRAMEX_OPTIONS_DEBUG=enable   # 启用
```

#### 2. 通过代码控制

```csharp
// 强制启用调试输出
var config = OptionsProvider.GetOptions<AppConfig>(enableDebugOutput: true);

// 强制禁用调试输出
var config = OptionsProvider.GetOptions<AppConfig>(enableDebugOutput: false);

// 使用自动检测（默认行为）
var config = OptionsProvider.GetOptions<AppConfig>();

// 静默模式（禁用调试）
var config = OptionsProvider.ParseSilent<AppConfig>(args);

// 强制调试模式
var config = OptionsProvider.ParseWithDebug<AppConfig>(args);
```

#### 3. 全局设置

```csharp
// 设置全局调试模式
OptionsProvider.SetGlobalDebugMode(false);

// 检查当前调试模式状态
bool isDebugEnabled = OptionsProvider.IsDebugModeEnabled();
```

### 环境感知

系统会根据运行环境自动调整调试输出：

```csharp
// 开发环境 - 默认启用调试
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development
export ENVIRONMENT=Development

// 生产环境 - 默认禁用调试
export ASPNETCORE_ENVIRONMENT=Production
export DOTNET_ENVIRONMENT=Production
export ENVIRONMENT=Production
```

支持的环境值：

- **启用调试**: `Development`, `Dev`, `Test`, `Testing`, `Debug`
- **禁用调试**: `Production`, `Prod`, `Release`

## 使用方式

### 命令行参数

支持多种参数格式：

```bash
# 键值对格式
myapp.exe --host=example.com --port=9090 --debug=true

# 分离格式
myapp.exe --host example.com --port 9090 --debug true

# 布尔标志格式
myapp.exe --host example.com --port 9090 --debug

# 混合格式
myapp.exe --host=example.com --port 9090 --debug
```

### 环境变量

```bash
# 设置环境变量
export HOST=example.com
export PORT=9090
export DEBUG=true

# 运行程序
myapp.exe
```

### Docker支持

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

```bash
# Docker运行（会显示调试信息）
docker run myapp --host example.com --port 9090 --debug

# 或使用环境变量禁用调试
docker run -e GAMEFRAMEX_OPTIONS_DEBUG=false myapp --host example.com
```

## 高级特性

### 使用特性配置

```csharp
using GameFrameX.Foundation.Options.Attributes;

public class AdvancedConfig
{
    [Option("h", "host", Required = false, DefaultValue = "localhost")]
    [HelpText("服务器主机地址")]
    public string Host { get; set; }

    [Option("p", "port", Required = true)]
    [HelpText("服务器端口号")]
    public int Port { get; set; }

    [FlagOption("d", "debug")]
    [HelpText("启用调试模式")]
    public bool Debug { get; set; }

    [RequiredOption("api-key", Required = true)]
    [EnvironmentVariable("API_KEY")]
    [HelpText("API密钥")]
    public string ApiKey { get; set; }

    [DefaultValue(30.0)]
    public double Timeout { get; set; }
}
```

### 构建器选项

```csharp
var builder = new OptionsBuilder<AppConfig>(
    args: args,
    boolFormat: BoolArgumentFormat.Flag,        // 布尔参数格式
    ensurePrefixedKeys: true,                   // 确保参数有前缀
    useEnvironmentVariables: true              // 使用环境变量
);

var config = builder.Build(skipValidation: false); // 是否跳过验证
```

## 参数优先级

参数按以下优先级应用（高优先级覆盖低优先级）：

1. **命令行参数** (最高优先级)
2. **环境变量**
3. **默认值** (最低优先级)

### 示例

```csharp
public class Config
{
    public string Host { get; set; } = "localhost";  // 默认值
    public int Port { get; set; } = 8080;           // 默认值
}
```

```bash
# 设置环境变量
export HOST=env.example.com
export PORT=7070

# 运行程序（命令行参数覆盖环境变量）
myapp.exe --host cmd.example.com

# 结果：
# Host = "cmd.example.com"  (来自命令行参数)
# Port = 7070               (来自环境变量)
```

## 布尔参数处理

支持多种布尔参数格式：

```bash
# 标志格式（推荐）
myapp.exe --debug                    # debug = true

# 键值对格式
myapp.exe --debug=true               # debug = true
myapp.exe --debug=false              # debug = false

# 分离格式
myapp.exe --debug true               # debug = true
myapp.exe --debug false              # debug = false

# 支持的布尔值
true, false, 1, 0, yes, no, on, off
```

## 类型转换

自动支持以下类型转换：

- `string` - 直接使用
- `int`, `int?` - 整数转换
- `bool`, `bool?` - 布尔值转换
- `double`, `double?` - 双精度浮点数转换
- `float`, `float?` - 单精度浮点数转换
- `decimal`, `decimal?` - 十进制数转换
- `DateTime`, `DateTime?` - 日期时间转换
- `Guid`, `Guid?` - GUID转换
- `Enum` - 枚举转换

### 示例

```csharp
public class TypedConfig
{
    public int Port { get; set; }
    public bool Debug { get; set; }
    public DateTime StartTime { get; set; }
    public LogLevel Level { get; set; }  // 枚举
}

public enum LogLevel
{
    Debug, Info, Warning, Error
}
```

```bash
myapp.exe --port 9090 --debug true --start-time "2024-01-01 10:00:00" --level Info
```

## 错误处理

### 必需参数验证

```csharp
public class Config
{
    [RequiredOption("api-key", Required = true)]
    public string ApiKey { get; set; }
}
```

如果缺少必需参数，会抛出 `ArgumentException`：

```
缺少必需的选项: api-key
```

### 类型转换错误

当参数值无法转换为目标类型时，会使用默认值并在控制台输出警告信息。

## 最佳实践

### 1. 配置类设计

```csharp
public class AppConfig
{
    // 使用有意义的默认值
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    
    // 布尔属性默认为false
    public bool Debug { get; set; } = false;
    
    // 使用特性提供更多信息
    [RequiredOption("database-url", Required = true)]
    [EnvironmentVariable("DATABASE_URL")]
    public string DatabaseUrl { get; set; }
}
```

### 2. 错误处理

```csharp
try
{
    OptionsProvider.Initialize(args);
    var config = OptionsProvider.GetOptions<AppConfig>();
    
    // 使用配置启动应用
    StartApplication(config);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"配置错误: {ex.Message}");
    Environment.Exit(1);
}
```

### 3. 生产环境部署

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        // 在生产环境中禁用调试输出
        if (IsProductionEnvironment())
        {
            OptionsProvider.SetGlobalDebugMode(false);
        }
        
        OptionsProvider.Initialize(args);
        var config = OptionsProvider.GetOptions<AppConfig>();
        
        var app = CreateApplication(config);
        app.Run();
    }
    
    private static bool IsProductionEnvironment()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return string.Equals(env, "Production", StringComparison.OrdinalIgnoreCase);
    }
}
```

### 4. Docker集成

```yaml
# docker-compose.yml
version: '3.8'
services:
  myapp:
    image: myapp:latest
    environment:
      - HOST=0.0.0.0
      - PORT=8080
      - DEBUG=false
      - ASPNETCORE_ENVIRONMENT=Production
      - GAMEFRAMEX_OPTIONS_DEBUG=false  # 禁用参数调试
    command: ["--log-level", "info"]
```

## 调试功能详细说明

### 输出内容说明

1. **原始参数信息**
    - 显示传入的所有命令行参数
    - 参数数量和索引

2. **可用选项定义**
    - 所有配置属性的选项定义
    - 参数类型、是否必需、默认值

3. **参数映射分析**
    - 参数如何映射到配置属性
    - 识别的选项和未识别的参数

4. **解析结果展示**
    - 最终配置对象的所有属性值
    - JSON格式的配置表示

### 使用场景

- **开发调试**: 验证参数解析是否正确
- **部署验证**: 确认生产环境配置是否符合预期
- **问题排查**: 快速定位配置相关问题
- **文档生成**: 自动生成当前配置的文档

## 完整示例

```csharp
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace MyApp
{
    public class ServerConfig
    {
        [Option("h", "host", DefaultValue = "localhost")]
        [EnvironmentVariable("SERVER_HOST")]
        [HelpText("服务器主机地址")]
        public string Host { get; set; }

        [Option("p", "port", DefaultValue = 8080)]
        [EnvironmentVariable("SERVER_PORT")]
        [HelpText("服务器端口号")]
        public int Port { get; set; }

        [FlagOption("d", "debug")]
        [EnvironmentVariable("DEBUG")]
        [HelpText("启用调试模式")]
        public bool Debug { get; set; }

        [RequiredOption("database-url", Required = true)]
        [EnvironmentVariable("DATABASE_URL")]
        [HelpText("数据库连接字符串")]
        public string DatabaseUrl { get; set; }

        [Option("timeout", DefaultValue = 30.0)]
        [EnvironmentVariable("REQUEST_TIMEOUT")]
        [HelpText("请求超时时间（秒）")]
        public double Timeout { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 初始化选项提供者
                OptionsProvider.Initialize(args);
                
                // 获取配置（默认启用调试输出）
                var config = OptionsProvider.GetOptions<ServerConfig>();

                Console.WriteLine("🚀 服务器启动中...");
                Console.WriteLine($"服务器地址: {config.Host}:{config.Port}");
                
                // 启动服务器
                StartServer(config);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ 配置错误: {ex.Message}");
                ShowHelp();
                Environment.Exit(1);
            }
        }

        static void StartServer(ServerConfig config)
        {
            // 服务器启动逻辑
            Console.WriteLine($"✅ 服务器已启动在 {config.Host}:{config.Port}");
        }

        static void ShowHelp()
        {
            Console.WriteLine("用法:");
            Console.WriteLine("  myapp.exe --database-url <数据库URL> [选项]");
            Console.WriteLine();
            Console.WriteLine("选项:");
            Console.WriteLine("  -h, --host <主机>           服务器主机地址 (默认: localhost)");
            Console.WriteLine("  -p, --port <端口>           服务器端口号 (默认: 8080)");
            Console.WriteLine("  -d, --debug                 启用调试模式");
            Console.WriteLine("      --database-url <URL>    数据库连接字符串 (必需)");
            Console.WriteLine("      --timeout <秒>          请求超时时间 (默认: 30.0)");
            Console.WriteLine();
            Console.WriteLine("环境变量:");
            Console.WriteLine("  GAMEFRAMEX_OPTIONS_DEBUG    控制参数调试输出 (true/false)");
            Console.WriteLine("  ASPNETCORE_ENVIRONMENT      运行环境 (Development/Production)");
        }
    }
}
```

现在，每次启动程序时都会自动显示详细的参数解析信息，让你能够第一时间验证配置是否正确！