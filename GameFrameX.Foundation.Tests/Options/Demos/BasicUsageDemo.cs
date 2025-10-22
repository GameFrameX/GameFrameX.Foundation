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
    /// 基础使用演示配置类
    /// </summary>
    public class BasicDemoConfig
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [RequiredOptionAttribute("app-name")]
        public string AppName { get; set; } = string.Empty;

        /// <summary>
        /// 服务器主机地址
        /// </summary>
        [OptionAttribute("host", DefaultValue = "localhost")]
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// 服务器端口号
        /// </summary>
        [OptionAttribute("port", DefaultValue = 8080)]
        public int Port { get; set; } = 8080;

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        [FlagOptionAttribute("debug")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// 日志级别
        /// </summary>
        [OptionAttribute("log-level", DefaultValue = "Info")]
        public string LogLevel { get; set; } = "Info";

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        [OptionAttribute("timeout", DefaultValue = 30.0)]
        public double Timeout { get; set; } = 30.0;

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [OptionAttribute("database-url")]
        [EnvironmentVariableAttribute("DATABASE_URL")]
        public string? DatabaseUrl { get; set; }
    }

    /// <summary>
    /// 基础使用演示
    /// </summary>
    public static class BasicUsageDemo
    {
        /// <summary>
        /// 运行基础使用演示
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void Run(string[] args)
        {
            Console.WriteLine("=== 基础使用演示 ===");
            Console.WriteLine();

            // 如果没有参数，使用默认测试参数
            if (args.Length == 0)
            {
                Console.WriteLine("📝 使用默认测试参数...");
                args = new[] { "--app-name", "BasicDemo", "--host", "example.com", "--port", "9090", "--debug", "--log-level", "Debug" };
                Console.WriteLine($"   参数: {string.Join(" ", args)}");
                Console.WriteLine();
            }

            try
            {
                Console.WriteLine("🔧 方法1: 使用传统的 OptionsBuilder 方式");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                // 传统方式
                var builder = new OptionsBuilder<BasicDemoConfig>(args);
                var config1 = builder.Build();
                
                Console.WriteLine("✅ 解析成功！配置信息:");
                PrintConfig(config1);
                Console.WriteLine();

                Console.WriteLine("🔧 方法2: 使用新的静态方法");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                // 静态方法
                var config2 = OptionsBuilder.Create<BasicDemoConfig>(args);
                
                Console.WriteLine("✅ 解析成功！配置信息:");
                PrintConfig(config2);
                Console.WriteLine();

                Console.WriteLine("🔧 方法3: 使用 OptionsProvider（推荐）");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                // 使用 OptionsProvider
                OptionsProvider.Initialize(args);
                var config3 = OptionsProvider.GetOptions<BasicDemoConfig>();
                
                Console.WriteLine("✅ 解析成功！配置信息:");
                PrintConfig(config3);
                Console.WriteLine();

                Console.WriteLine("🎯 演示不同参数格式的支持");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateArgumentFormats();
                Console.WriteLine();

                Console.WriteLine("🌍 演示环境变量支持");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateEnvironmentVariables();
                Console.WriteLine();

                Console.WriteLine("⚠️  演示错误处理");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateErrorHandling();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 演示过程中发生错误: {ex.Message}");
                Console.WriteLine($"   堆栈跟踪: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 打印配置信息
        /// </summary>
        /// <param name="config">配置对象</param>
        private static void PrintConfig(BasicDemoConfig config)
        {
            Console.WriteLine($"   应用名称: {config.AppName}");
            Console.WriteLine($"   服务器: {config.Host}:{config.Port}");
            Console.WriteLine($"   调试模式: {config.Debug}");
            Console.WriteLine($"   日志级别: {config.LogLevel}");
            Console.WriteLine($"   超时时间: {config.Timeout}秒");
            Console.WriteLine($"   数据库URL: {config.DatabaseUrl ?? "未设置"}");
        }

        /// <summary>
        /// 演示不同参数格式
        /// </summary>
        private static void DemonstrateArgumentFormats()
        {
            var testCases = new[]
            {
                new { Name = "键值对格式", Args = new[] { "--app-name=FormatTest", "--host=test1.com", "--port=8081" } },
                new { Name = "分离格式", Args = new[] { "--app-name", "FormatTest", "--host", "test2.com", "--port", "8082" } },
                new { Name = "短参数格式", Args = new[] { "--app-name", "FormatTest", "-h", "test3.com", "-p", "8083", "-d" } },
                new { Name = "混合格式", Args = new[] { "--app-name=FormatTest", "-h", "test4.com", "--port=8084", "--debug" } }
            };

            foreach (var testCase in testCases)
            {
                Console.WriteLine($"📋 {testCase.Name}:");
                Console.WriteLine($"   参数: {string.Join(" ", testCase.Args)}");
                
                try
                {
                    var config = OptionsBuilder.Create<BasicDemoConfig>(testCase.Args, skipValidation: true);
                    Console.WriteLine($"   结果: {config.AppName} @ {config.Host}:{config.Port} (调试: {config.Debug})");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"   ❌ 解析失败: {ex.Message}");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 演示环境变量支持
        /// </summary>
        private static void DemonstrateEnvironmentVariables()
        {
            // 设置环境变量
            Environment.SetEnvironmentVariable("DATABASE_URL", "postgresql://localhost:5432/testdb");
            Environment.SetEnvironmentVariable("HOST", "env.example.com");
            Environment.SetEnvironmentVariable("PORT", "7070");

            Console.WriteLine("📋 设置的环境变量:");
            Console.WriteLine("   DATABASE_URL=postgresql://localhost:5432/testdb");
            Console.WriteLine("   HOST=env.example.com");
            Console.WriteLine("   PORT=7070");
            Console.WriteLine();

            // 测试仅使用环境变量
            Console.WriteLine("🔧 仅使用环境变量:");
            try
            {
                var config1 = OptionsBuilder.CreateFromEnvironmentOnly<BasicDemoConfig>(skipValidation: true);
                PrintConfig(config1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 失败: {ex.Message}");
            }
            Console.WriteLine();

            // 测试命令行参数覆盖环境变量
            Console.WriteLine("🔧 命令行参数覆盖环境变量:");
            var args = new[] { "--app-name", "EnvTest", "--host", "cmd.example.com" };
            Console.WriteLine($"   参数: {string.Join(" ", args)}");
            
            try
            {
                var config2 = OptionsBuilder.Create<BasicDemoConfig>(args, skipValidation: true);
                Console.WriteLine("   结果（注意HOST被命令行参数覆盖，PORT来自环境变量）:");
                PrintConfig(config2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 失败: {ex.Message}");
            }

            // 清理环境变量
            Environment.SetEnvironmentVariable("DATABASE_URL", null);
            Environment.SetEnvironmentVariable("HOST", null);
            Environment.SetEnvironmentVariable("PORT", null);
        }

        /// <summary>
        /// 演示错误处理
        /// </summary>
        private static void DemonstrateErrorHandling()
        {
            Console.WriteLine("📋 测试缺少必需参数:");
            var invalidArgs1 = new[] { "--host", "test.com", "--port", "8080" }; // 缺少 app-name
            Console.WriteLine($"   参数: {string.Join(" ", invalidArgs1)}");
            
            try
            {
                var config = OptionsBuilder.Create<BasicDemoConfig>(invalidArgs1);
                Console.WriteLine("   ❌ 应该失败但没有失败");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"   ✅ 正确捕获错误: {ex.Message}");
            }
            Console.WriteLine();

            Console.WriteLine("📋 测试安全创建方法:");
            if (OptionsBuilder.TryCreate<BasicDemoConfig>(invalidArgs1, out var result, out var error))
            {
                Console.WriteLine("   ❌ 应该失败但返回了成功");
            }
            else
            {
                Console.WriteLine($"   ✅ 正确返回失败: {error}");
                Console.WriteLine("   📄 返回的默认配置:");
                PrintConfig(result);
            }
        }
    }
}