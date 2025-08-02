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

### 2. 使用OptionsBuilder

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
        Console.WriteLine($"调试模式: {config.Debug}");
        Console.WriteLine($"日志级别: {config.LogLevel}");
        Console.WriteLine($"超时时间: {config.Timeout}秒");
    }
}
```

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
# Docker运行
docker run myapp --host example.com --port 9090 --debug

# 或使用环境变量
docker run -e HOST=example.com -e PORT=9090 -e DEBUG=true myapp
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
    var builder = new OptionsBuilder<AppConfig>(args);
    var config = builder.Build();
    
    // 使用配置启动应用
    StartApplication(config);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"配置错误: {ex.Message}");
    Environment.Exit(1);
}
```

### 3. Docker集成

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = new OptionsBuilder<AppConfig>(args);
        var config = builder.Build();
        
        // 在Docker中，通常使用环境变量
        // 在开发中，通常使用命令行参数
        
        var app = CreateApplication(config);
        app.Run();
    }
}
```

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
    command: ["--log-level", "info"]
```

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
                var builder = new OptionsBuilder<ServerConfig>(args);
                var config = builder.Build();

                Console.WriteLine("服务器配置:");
                Console.WriteLine($"  主机: {config.Host}");
                Console.WriteLine($"  端口: {config.Port}");
                Console.WriteLine($"  调试: {config.Debug}");
                Console.WriteLine($"  数据库: {config.DatabaseUrl}");
                Console.WriteLine($"  超时: {config.Timeout}秒");

                // 启动服务器
                StartServer(config);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"配置错误: {ex.Message}");
                ShowHelp();
                Environment.Exit(1);
            }
        }

        static void StartServer(ServerConfig config)
        {
            // 服务器启动逻辑
            Console.WriteLine($"服务器启动在 {config.Host}:{config.Port}");
        }

        static void ShowHelp()
        {
            Console.WriteLine("用法:");
            Console.WriteLine("  myapp.exe --host <主机> --port <端口> --database-url <数据库URL> [选项]");
            Console.WriteLine();
            Console.WriteLine("选项:");
            Console.WriteLine("  -h, --host <主机>           服务器主机地址 (默认: localhost)");
            Console.WriteLine("  -p, --port <端口>           服务器端口号 (默认: 8080)");
            Console.WriteLine("  -d, --debug                 启用调试模式");
            Console.WriteLine("      --database-url <URL>    数据库连接字符串 (必需)");
            Console.WriteLine("      --timeout <秒>          请求超时时间 (默认: 30.0)");
        }
    }
}
```
