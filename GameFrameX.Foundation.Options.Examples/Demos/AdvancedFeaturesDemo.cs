// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace GameFrameX.Foundation.Options.Examples.Demos
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试级别
        /// </summary>
        Debug,
        
        /// <summary>
        /// 信息级别
        /// </summary>
        Info,
        
        /// <summary>
        /// 警告级别
        /// </summary>
        Warning,
        
        /// <summary>
        /// 错误级别
        /// </summary>
        Error,
        
        /// <summary>
        /// 致命错误级别
        /// </summary>
        Fatal
    }

    /// <summary>
    /// 高级特性演示配置类
    /// </summary>
    public class AdvancedFeaturesDemoConfig
    {
        /// <summary>
        /// 应用程序名称（必需）
        /// </summary>
        [RequiredOptionAttribute("app-name")]
        [HelpTextAttribute("应用程序的名称，用于标识应用")]
        public string AppName { get; set; } = string.Empty;

        /// <summary>
        /// 服务器主机地址
        /// </summary>
        [OptionAttribute("host", DefaultValue = "localhost")]
        [EnvironmentVariableAttribute("SERVER_HOST")]
        [HelpTextAttribute("服务器主机地址，支持IP地址或域名")]
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// 服务器端口号
        /// </summary>
        [OptionAttribute("port", DefaultValue = 8080)]
        [EnvironmentVariableAttribute("SERVER_PORT")]
        [HelpTextAttribute("服务器监听端口号，范围1-65535")]
        public int Port { get; set; } = 8080;

        /// <summary>
        /// 是否启用SSL
        /// </summary>
        [FlagOptionAttribute("ssl")]
        [EnvironmentVariableAttribute("ENABLE_SSL")]
        [HelpTextAttribute("启用SSL/TLS加密连接")]
        public bool EnableSsl { get; set; } = false;

        /// <summary>
        /// 日志级别（枚举类型）
        /// </summary>
        [OptionAttribute("log-level", DefaultValue = LogLevel.Info)]
        [EnvironmentVariableAttribute("LOG_LEVEL")]
        [HelpTextAttribute("日志记录级别：Debug, Info, Warning, Error, Fatal")]
        public LogLevel LogLevel { get; set; } = LogLevel.Info;

        /// <summary>
        /// 超时时间（浮点数类型）
        /// </summary>
        [OptionAttribute("timeout", DefaultValue = 30.5)]
        [EnvironmentVariableAttribute("REQUEST_TIMEOUT")]
        [HelpTextAttribute("请求超时时间，单位为秒，支持小数")]
        public double Timeout { get; set; } = 30.5;

        /// <summary>
        /// 启动时间（日期时间类型）
        /// </summary>
        [OptionAttribute("start-time")]
        [EnvironmentVariableAttribute("START_TIME")]
        [HelpTextAttribute("应用启动时间，格式：yyyy-MM-dd HH:mm:ss")]
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 应用程序GUID
        /// </summary>
        [OptionAttribute("app-id")]
        [EnvironmentVariableAttribute("APP_ID")]
        [HelpTextAttribute("应用程序唯一标识符（GUID格式）")]
        public Guid? AppId { get; set; }

        /// <summary>
        /// 数据库连接字符串（必需）
        /// </summary>
        [RequiredOptionAttribute("database-url")]
        [EnvironmentVariableAttribute("DATABASE_URL")]
        [HelpTextAttribute("数据库连接字符串，必须提供")]
        public string DatabaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// API密钥（可选）
        /// </summary>
        [OptionAttribute("api-key")]
        [EnvironmentVariableAttribute("API_KEY")]
        [HelpTextAttribute("第三方API访问密钥，可选")]
        public string? ApiKey { get; set; }

        /// <summary>
        /// 最大连接数（可空整数）
        /// </summary>
        [OptionAttribute("max-connections", DefaultValue = 100)]
        [EnvironmentVariableAttribute("MAX_CONNECTIONS")]
        [HelpTextAttribute("最大并发连接数，默认100")]
        public int? MaxConnections { get; set; } = 100;

        /// <summary>
        /// 缓存大小（十进制数）
        /// </summary>
        [OptionAttribute("cache-size", DefaultValue = 256.5)]
        [EnvironmentVariableAttribute("CACHE_SIZE")]
        [HelpTextAttribute("缓存大小，单位MB，支持小数")]
        public decimal CacheSize { get; set; } = 256.5m;

        /// <summary>
        /// 是否启用调试模式（可空布尔值）
        /// </summary>
        [FlagOptionAttribute("debug")]
        [EnvironmentVariableAttribute("DEBUG")]
        [HelpTextAttribute("启用调试模式，显示详细日志")]
        public bool? Debug { get; set; }
    }

    /// <summary>
    /// 高级特性演示
    /// </summary>
    public static class AdvancedFeaturesDemo
    {
        /// <summary>
        /// 运行高级特性演示
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void Run(string[] args)
        {
            Console.WriteLine("=== 高级特性演示 ===");
            Console.WriteLine();

            // 如果没有参数，使用默认测试参数
            if (args.Length == 0)
            {
                Console.WriteLine("📝 使用默认测试参数...");
                args = new[] 
                { 
                    "--app-name", "AdvancedDemo",
                    "--host", "advanced.example.com",
                    "--port", "9090",
                    "--ssl",
                    "--log-level", "Debug",
                    "--timeout", "45.5",
                    "--start-time", "2024-01-01 10:30:00",
                    "--app-id", Guid.NewGuid().ToString(),
                    "--database-url", "postgresql://localhost:5432/advanceddb",
                    "--api-key", "sk-1234567890abcdef",
                    "--max-connections", "200",
                    "--cache-size", "512.75",
                    "--debug"
                };
                Console.WriteLine($"   参数: {string.Join(" ", args)}");
                Console.WriteLine();
            }

            try
            {
                Console.WriteLine("🚀 演示高级类型支持");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                // 使用调试模式解析配置
                var config = OptionsBuilder.CreateWithDebug<AdvancedFeaturesDemoConfig>(args);
                
                Console.WriteLine("✅ 解析成功！详细配置信息:");
                PrintDetailedConfig(config);
                Console.WriteLine();

                Console.WriteLine("🎯 演示类型转换能力");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateTypeConversion();
                Console.WriteLine();

                Console.WriteLine("🌍 演示环境变量与命令行参数优先级");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstratePriorityHandling();
                Console.WriteLine();

                Console.WriteLine("⚠️  演示错误处理和验证");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateErrorHandling();
                Console.WriteLine();

                Console.WriteLine("🔧 演示不同布尔参数格式");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateBooleanFormats();
                Console.WriteLine();

                Console.WriteLine("📊 演示特性组合使用");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateAttributeCombinations();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 演示过程中发生错误: {ex.Message}");
                Console.WriteLine($"   堆栈跟踪: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 打印详细配置信息
        /// </summary>
        /// <param name="config">配置对象</param>
        private static void PrintDetailedConfig(AdvancedFeaturesDemoConfig config)
        {
            Console.WriteLine($"   应用名称: {config.AppName} (字符串)");
            Console.WriteLine($"   服务器: {config.Host}:{config.Port} (字符串:整数)");
            Console.WriteLine($"   启用SSL: {config.EnableSsl} (布尔值)");
            Console.WriteLine($"   日志级别: {config.LogLevel} (枚举)");
            Console.WriteLine($"   超时时间: {config.Timeout}秒 (双精度浮点数)");
            Console.WriteLine($"   启动时间: {config.StartTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "未设置"} (可空日期时间)");
            Console.WriteLine($"   应用ID: {config.AppId?.ToString() ?? "未设置"} (可空GUID)");
            Console.WriteLine($"   数据库URL: {config.DatabaseUrl} (字符串)");
            Console.WriteLine($"   API密钥: {config.ApiKey ?? "未设置"} (可空字符串)");
            Console.WriteLine($"   最大连接数: {config.MaxConnections?.ToString() ?? "未设置"} (可空整数)");
            Console.WriteLine($"   缓存大小: {config.CacheSize}MB (十进制数)");
            Console.WriteLine($"   调试模式: {config.Debug?.ToString() ?? "未设置"} (可空布尔值)");
        }

        /// <summary>
        /// 演示类型转换能力
        /// </summary>
        private static void DemonstrateTypeConversion()
        {
            var testCases = new[]
            {
                new { Name = "整数转换", Args = new[] { "--app-name", "TypeTest", "--database-url", "test://db", "--port", "8080", "--max-connections", "150" } },
                new { Name = "浮点数转换", Args = new[] { "--app-name", "TypeTest", "--database-url", "test://db", "--timeout", "25.75", "--cache-size", "1024.25" } },
                new { Name = "枚举转换", Args = new[] { "--app-name", "TypeTest", "--database-url", "test://db", "--log-level", "Warning" } },
                new { Name = "日期时间转换", Args = new[] { "--app-name", "TypeTest", "--database-url", "test://db", "--start-time", "2024-12-25 15:30:45" } },
                new { Name = "GUID转换", Args = new[] { "--app-name", "TypeTest", "--database-url", "test://db", "--app-id", "12345678-1234-1234-1234-123456789abc" } },
                new { Name = "布尔值转换", Args = new[] { "--app-name", "TypeTest", "--database-url", "test://db", "--ssl", "--debug" } }
            };

            foreach (var testCase in testCases)
            {
                Console.WriteLine($"📋 {testCase.Name}:");
                Console.WriteLine($"   参数: {string.Join(" ", testCase.Args)}");
                
                try
                {
                    var config = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(testCase.Args, skipValidation: true);
                    
                    switch (testCase.Name)
                    {
                        case "整数转换":
                            Console.WriteLine($"   结果: Port={config.Port}, MaxConnections={config.MaxConnections}");
                            break;
                        case "浮点数转换":
                            Console.WriteLine($"   结果: Timeout={config.Timeout}, CacheSize={config.CacheSize}");
                            break;
                        case "枚举转换":
                            Console.WriteLine($"   结果: LogLevel={config.LogLevel}");
                            break;
                        case "日期时间转换":
                            Console.WriteLine($"   结果: StartTime={config.StartTime:yyyy-MM-dd HH:mm:ss}");
                            break;
                        case "GUID转换":
                            Console.WriteLine($"   结果: AppId={config.AppId}");
                            break;
                        case "布尔值转换":
                            Console.WriteLine($"   结果: EnableSsl={config.EnableSsl}, Debug={config.Debug}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ❌ 转换失败: {ex.Message}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 演示优先级处理
        /// </summary>
        private static void DemonstratePriorityHandling()
        {
            Console.WriteLine("📋 设置环境变量:");
            Environment.SetEnvironmentVariable("SERVER_HOST", "env.example.com");
            Environment.SetEnvironmentVariable("SERVER_PORT", "7070");
            Environment.SetEnvironmentVariable("LOG_LEVEL", "Error");
            Environment.SetEnvironmentVariable("ENABLE_SSL", "true");
            
            Console.WriteLine("   SERVER_HOST=env.example.com");
            Console.WriteLine("   SERVER_PORT=7070");
            Console.WriteLine("   LOG_LEVEL=Error");
            Console.WriteLine("   ENABLE_SSL=true");
            Console.WriteLine();

            // 测试1: 仅使用环境变量
            Console.WriteLine("🔧 测试1: 仅使用环境变量");
            var config1 = OptionsBuilder.CreateFromEnvironmentOnly<AdvancedFeaturesDemoConfig>(skipValidation: true);
            Console.WriteLine($"   结果: {config1.Host}:{config1.Port}, SSL={config1.EnableSsl}, LogLevel={config1.LogLevel}");
            Console.WriteLine();

            // 测试2: 命令行参数覆盖环境变量
            Console.WriteLine("🔧 测试2: 命令行参数覆盖环境变量");
            var args = new[] { "--app-name", "PriorityTest", "--database-url", "test://db", "--host", "cmd.example.com", "--log-level", "Debug" };
            Console.WriteLine($"   参数: {string.Join(" ", args)}");

            var config2 = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(args, skipValidation: true);
            Console.WriteLine($"   结果: {config2.Host}:{config2.Port}, SSL={config2.EnableSsl}, LogLevel={config2.LogLevel}");
            Console.WriteLine("   说明: Host和LogLevel被命令行参数覆盖，Port和SSL来自环境变量");
            Console.WriteLine();

            // 测试3: 默认值的使用
            Console.WriteLine("🔧 测试3: 默认值的使用");
            var config3 = OptionsBuilder.CreateDefault<AdvancedFeaturesDemoConfig>();
            Console.WriteLine($"   结果: {config3.Host}:{config3.Port}, SSL={config3.EnableSsl}, LogLevel={config3.LogLevel}");
            Console.WriteLine("   说明: 所有值都使用默认值");

            // 清理环境变量
            Environment.SetEnvironmentVariable("SERVER_HOST", null);
            Environment.SetEnvironmentVariable("SERVER_PORT", null);
            Environment.SetEnvironmentVariable("LOG_LEVEL", null);
            Environment.SetEnvironmentVariable("ENABLE_SSL", null);
        }

        /// <summary>
        /// 演示错误处理
        /// </summary>
        private static void DemonstrateErrorHandling()
        {
            Console.WriteLine("📋 测试1: 缺少必需参数");
            var invalidArgs1 = new[] { "--host", "test.com", "--port", "8080" }; // 缺少 app-name 和 database-url
            Console.WriteLine($"   参数: {string.Join(" ", invalidArgs1)}");
            
            try
            {
                var config = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(invalidArgs1);
                Console.WriteLine("   ❌ 应该失败但没有失败");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"   ✅ 正确捕获错误: {ex.Message}");
            }
            Console.WriteLine();

            Console.WriteLine("📋 测试2: 无效的类型转换");
            var invalidArgs2 = new[] { "--app-name", "TypeErrorTest", "--database-url", "test://db", "--port", "invalid_port", "--timeout", "not_a_number" };
            Console.WriteLine($"   参数: {string.Join(" ", invalidArgs2)}");
            
            try
            {
                var config = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(invalidArgs2, skipValidation: true);
                Console.WriteLine($"   结果: Port={config.Port} (使用默认值), Timeout={config.Timeout} (使用默认值)");
                Console.WriteLine("   说明: 无效值被忽略，使用默认值");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ 意外错误: {ex.Message}");
            }
            Console.WriteLine();

            Console.WriteLine("📋 测试3: 无效的枚举值");
            var invalidArgs3 = new[] { "--app-name", "EnumErrorTest", "--database-url", "test://db", "--log-level", "InvalidLevel" };
            Console.WriteLine($"   参数: {string.Join(" ", invalidArgs3)}");
            
            try
            {
                var config = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(invalidArgs3, skipValidation: true);
                Console.WriteLine($"   结果: LogLevel={config.LogLevel} (使用默认值)");
                Console.WriteLine("   说明: 无效枚举值被忽略，使用默认值");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ 意外错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 演示不同布尔参数格式
        /// </summary>
        private static void DemonstrateBooleanFormats()
        {
            var testCases = new[]
            {
                new { Name = "标志格式", Args = new[] { "--app-name", "BoolTest", "--database-url", "test://db", "--ssl", "--debug" } },
                new { Name = "键值对格式(true)", Args = new[] { "--app-name", "BoolTest", "--database-url", "test://db", "--ssl=true", "--debug=true" } },
                new { Name = "键值对格式(false)", Args = new[] { "--app-name", "BoolTest", "--database-url", "test://db", "--ssl=false", "--debug=false" } },
                new { Name = "分离格式(yes/no)", Args = new[] { "--app-name", "BoolTest", "--database-url", "test://db", "--ssl", "yes", "--debug", "no" } },
                new { Name = "分离格式(1/0)", Args = new[] { "--app-name", "BoolTest", "--database-url", "test://db", "--ssl", "1", "--debug", "0" } },
                new { Name = "分离格式(on/off)", Args = new[] { "--app-name", "BoolTest", "--database-url", "test://db", "--ssl", "on", "--debug", "off" } }
            };

            foreach (var testCase in testCases)
            {
                Console.WriteLine($"📋 {testCase.Name}:");
                Console.WriteLine($"   参数: {string.Join(" ", testCase.Args)}");
                
                try
                {
                    var config = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(testCase.Args, skipValidation: true);
                    Console.WriteLine($"   结果: SSL={config.EnableSsl}, Debug={config.Debug}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ❌ 解析失败: {ex.Message}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 演示特性组合使用
        /// </summary>
        private static void DemonstrateAttributeCombinations()
        {
            Console.WriteLine("📋 演示特性组合的强大功能:");
            Console.WriteLine();

            // 设置环境变量
            Environment.SetEnvironmentVariable("SERVER_HOST", "env-host.com");
            Environment.SetEnvironmentVariable("API_KEY", "env-api-key-12345");
            
            Console.WriteLine("🔧 设置的环境变量:");
            Console.WriteLine("   SERVER_HOST=env-host.com");
            Console.WriteLine("   API_KEY=env-api-key-12345");
            Console.WriteLine();

            var args = new[] 
            { 
                "--app-name", "AttributeDemo",
                "--database-url", "postgresql://localhost:5432/demo",
                "--port", "9000",  // 覆盖默认值
                "--ssl",           // 标志选项
                "--log-level", "Warning"  // 枚举选项
                // 注意：没有提供 host 和 api-key，将使用环境变量
            };

            Console.WriteLine("🔧 命令行参数:");
            Console.WriteLine($"   {string.Join(" ", args)}");
            Console.WriteLine();

            var config = OptionsBuilder.Create<AdvancedFeaturesDemoConfig>(args, skipValidation: true);

            Console.WriteLine("✅ 解析结果展示特性组合效果:");
            Console.WriteLine($"   应用名称: {config.AppName} (来自命令行 - RequiredOption)");
            Console.WriteLine($"   主机地址: {config.Host} (来自环境变量 - EnvironmentVariable)");
            Console.WriteLine($"   端口号: {config.Port} (来自命令行 - 覆盖DefaultValue)");
            Console.WriteLine($"   启用SSL: {config.EnableSsl} (来自命令行 - FlagOption)");
            Console.WriteLine($"   日志级别: {config.LogLevel} (来自命令行 - 枚举转换)");
            Console.WriteLine($"   超时时间: {config.Timeout} (使用默认值 - DefaultValue)");
            Console.WriteLine($"   API密钥: {config.ApiKey} (来自环境变量 - EnvironmentVariable)");
            Console.WriteLine($"   数据库URL: {config.DatabaseUrl} (来自命令行 - RequiredOption)");
            Console.WriteLine();

            Console.WriteLine("🎯 特性组合说明:");
            Console.WriteLine("   ✅ RequiredOption + HelpText: 必需参数带说明");
            Console.WriteLine("   ✅ Option + DefaultValue + EnvironmentVariable: 多层级默认值");
            Console.WriteLine("   ✅ FlagOption + EnvironmentVariable: 布尔标志支持环境变量");
            Console.WriteLine("   ✅ Option + 枚举类型: 自动枚举转换");
            Console.WriteLine("   ✅ 可空类型支持: 区分未设置和默认值");

            // 清理环境变量
            Environment.SetEnvironmentVariable("SERVER_HOST", null);
            Environment.SetEnvironmentVariable("API_KEY", null);
        }
    }
}