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
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace GameFrameX.Foundation.Options.Examples.Demos
{
    /// <summary>
    /// 静态方法演示配置类
    /// </summary>
    public class StaticMethodsDemoConfig
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [OptionAttribute("app-name", Required = true)]
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
        /// 最大连接数
        /// </summary>
        [OptionAttribute("max-connections", DefaultValue = 100)]
        public int MaxConnections { get; set; } = 100;
    }

    /// <summary>
    /// 静态方法演示
    /// </summary>
    public static class StaticMethodsDemo
    {
        /// <summary>
        /// 运行静态方法演示
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void Run(string[] args)
        {
            Console.WriteLine("=== 静态方法演示 ===");
            Console.WriteLine();

            // 如果没有参数，使用默认测试参数
            if (args.Length == 0)
            {
                Console.WriteLine("📝 使用默认测试参数...");
                args = new[] { "--app-name", "StaticDemo", "--host", "static.example.com", "--port", "9090", "--debug", "--log-level", "Debug" };
                Console.WriteLine($"   参数: {string.Join(" ", args)}");
                Console.WriteLine();
            }

            try
            {
                Console.WriteLine("🚀 演示所有静态便捷方法");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                // 1. 基本静态方法
                Console.WriteLine("📋 方法1: OptionsBuilder<T>.Create(args) - 最简单的使用方式");
                var config1 = OptionsBuilder.Create<StaticMethodsDemoConfig>(args);
                PrintConfig("Create", config1);
                Console.WriteLine();

                // 2. 完整参数控制
                Console.WriteLine("📋 方法2: OptionsBuilder<T>.Create(args, boolFormat, ...) - 完整参数控制");
                var config2 = OptionsBuilder.Create<StaticMethodsDemoConfig>(
                    args,
                    BoolArgumentFormat.Flag,
                    ensurePrefixedKeys: true,
                    useEnvironmentVariables: true,
                    skipValidation: false);
                PrintConfig("Create (完整参数)", config2);
                Console.WriteLine();

                // 3. 仅使用命令行参数
                Console.WriteLine("📋 方法3: OptionsBuilder<T>.CreateFromArgsOnly(args) - 仅使用命令行参数");
                var config3 = OptionsBuilder.CreateFromArgsOnly<StaticMethodsDemoConfig>(args);
                PrintConfig("CreateFromArgsOnly", config3);
                Console.WriteLine();

                // 4. 仅使用环境变量
                Console.WriteLine("📋 方法4: OptionsBuilder<T>.CreateFromEnvironmentOnly() - 仅使用环境变量");
                // 设置一些环境变量用于测试
                Environment.SetEnvironmentVariable("APP_NAME", "EnvApp");
                Environment.SetEnvironmentVariable("HOST", "env.example.com");
                Environment.SetEnvironmentVariable("PORT", "7070");

                var config4 = OptionsBuilder.CreateFromEnvironmentOnly<StaticMethodsDemoConfig>(skipValidation: true);
                PrintConfig("CreateFromEnvironmentOnly", config4);
                Console.WriteLine();

                // 5. 创建默认配置
                Console.WriteLine("📋 方法5: OptionsBuilder<T>.CreateDefault() - 仅使用默认值");
                var config5 = OptionsBuilder.CreateDefault<StaticMethodsDemoConfig>();
                PrintConfig("CreateDefault", config5);
                Console.WriteLine();

                // 6. 安全创建方法
                Console.WriteLine("📋 方法6: OptionsBuilder<T>.TryCreate(args, out result, out error) - 安全创建");
                if (OptionsBuilder.TryCreate<StaticMethodsDemoConfig>(args, out var config6, out var error))
                {
                    Console.WriteLine("✅ 创建成功!");
                    PrintConfig("TryCreate", config6);
                }
                else
                {
                    Console.WriteLine($"❌ 创建失败: {error}");
                    Console.WriteLine("🔄 使用默认配置:");
                    PrintConfig("TryCreate (默认)", config6);
                }

                Console.WriteLine();

                // 7. 带调试输出的创建
                Console.WriteLine("📋 方法7: OptionsBuilder<T>.CreateWithDebug(args) - 带调试输出");
                Console.WriteLine("   (注意：这个方法会显示详细的调试信息)");
                Console.WriteLine();
                var config7 = OptionsBuilder.CreateWithDebug<StaticMethodsDemoConfig>(args);
                Console.WriteLine("✅ CreateWithDebug 完成!");
                Console.WriteLine();

                // 清理环境变量
                Environment.SetEnvironmentVariable("APP_NAME", null);
                Environment.SetEnvironmentVariable("HOST", null);
                Environment.SetEnvironmentVariable("PORT", null);

                Console.WriteLine("🎯 使用场景演示");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateUsageScenarios();
                Console.WriteLine();

                Console.WriteLine("⚖️  传统方式 vs 静态方法对比");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                CompareWithTraditionalWay(args);
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
        /// <param name="method">方法名称</param>
        /// <param name="config">配置对象</param>
        private static void PrintConfig(string method, StaticMethodsDemoConfig config)
        {
            Console.WriteLine($"✅ {method} 结果:");
            Console.WriteLine($"   应用名称: {config.AppName ?? "null"}");
            Console.WriteLine($"   服务器: {config.Host}:{config.Port}");
            Console.WriteLine($"   调试模式: {config.Debug}");
            Console.WriteLine($"   日志级别: {config.LogLevel}");
            Console.WriteLine($"   超时时间: {config.Timeout}秒");
            Console.WriteLine($"   最大连接数: {config.MaxConnections}");
        }

        /// <summary>
        /// 演示不同使用场景
        /// </summary>
        private static void DemonstrateUsageScenarios()
        {
            Console.WriteLine("📋 场景1: 简单的控制台应用");
            Console.WriteLine("```csharp");
            Console.WriteLine("static void Main(string[] args)");
            Console.WriteLine("{");
            Console.WriteLine("    var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine("    StartApplication(config);");
            Console.WriteLine("}");
            Console.WriteLine("```");
            Console.WriteLine();

            Console.WriteLine("📋 场景2: Web应用启动");
            Console.WriteLine("```csharp");
            Console.WriteLine("static void Main(string[] args)");
            Console.WriteLine("{");
            Console.WriteLine("    if (OptionsBuilder<ServerConfig>.TryCreate(args, out var config, out var error))");
            Console.WriteLine("    {");
            Console.WriteLine("        StartWebServer(config);");
            Console.WriteLine("    }");
            Console.WriteLine("    else");
            Console.WriteLine("    {");
            Console.WriteLine("        Console.WriteLine($\"配置错误: {error}\");");
            Console.WriteLine("        Environment.Exit(1);");
            Console.WriteLine("    }");
            Console.WriteLine("}");
            Console.WriteLine("```");
            Console.WriteLine();

            Console.WriteLine("📋 场景3: 开发调试");
            Console.WriteLine("```csharp");
            Console.WriteLine("#if DEBUG");
            Console.WriteLine("    var config = OptionsBuilder<AppConfig>.CreateWithDebug(args);");
            Console.WriteLine("#else");
            Console.WriteLine("    var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine("#endif");
            Console.WriteLine("```");
            Console.WriteLine();

            Console.WriteLine("📋 场景4: 微服务配置");
            Console.WriteLine("```csharp");
            Console.WriteLine("// 优先使用环境变量（适合容器化部署）");
            Console.WriteLine("var config = OptionsBuilder<MicroserviceConfig>.Create(");
            Console.WriteLine("    args,");
            Console.WriteLine("    BoolArgumentFormat.Flag,");
            Console.WriteLine("    ensurePrefixedKeys: true,");
            Console.WriteLine("    useEnvironmentVariables: true");
            Console.WriteLine(");");
            Console.WriteLine("```");
            Console.WriteLine();

            Console.WriteLine("📋 场景5: 测试环境");
            Console.WriteLine("```csharp");
            Console.WriteLine("// 仅使用默认值，不受外部参数影响");
            Console.WriteLine("var testConfig = OptionsBuilder<TestConfig>.CreateDefault();");
            Console.WriteLine("```");
        }

        /// <summary>
        /// 与传统方式对比
        /// </summary>
        /// <param name="args">命令行参数</param>
        private static void CompareWithTraditionalWay(string[] args)
        {
            Console.WriteLine("🔄 传统方式（需要创建对象）:");
            Console.WriteLine("```csharp");
            Console.WriteLine("var builder = new OptionsBuilder<AppConfig>(args);");
            Console.WriteLine("var config = builder.Build();");
            Console.WriteLine("```");

            // 传统方式
            var startTime1 = DateTime.Now;
            var builder = new OptionsBuilder<StaticMethodsDemoConfig>(args);
            var traditionalConfig = builder.Build(skipValidation: true);
            var elapsed1 = DateTime.Now - startTime1;

            PrintConfig("传统方式", traditionalConfig);
            Console.WriteLine($"   执行时间: {elapsed1.TotalMilliseconds:F2}ms");
            Console.WriteLine();

            Console.WriteLine("⚡ 新的静态方法（一行搞定）:");
            Console.WriteLine("```csharp");
            Console.WriteLine("var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine("```");

            // 静态方法
            var startTime2 = DateTime.Now;
            var staticConfig = OptionsBuilder.Create<StaticMethodsDemoConfig>(args, skipValidation: true);
            var elapsed2 = DateTime.Now - startTime2;

            PrintConfig("静态方法", staticConfig);
            Console.WriteLine($"   执行时间: {elapsed2.TotalMilliseconds:F2}ms");
            Console.WriteLine();

            Console.WriteLine("🎯 优势总结:");
            Console.WriteLine("   ✅ 代码更简洁 - 从两行代码减少到一行");
            Console.WriteLine("   ✅ 减少对象创建 - 直接返回结果，无需中间对象");
            Console.WriteLine("   ✅ 更直观的API - 方法名清楚表达意图");
            Console.WriteLine("   ✅ 支持多种场景 - 提供专门的方法应对不同需求");
            Console.WriteLine("   ✅ 保持向后兼容 - 原有的实例方法依然可用");
            Console.WriteLine("   ✅ 类型安全 - 泛型支持，编译时类型检查");
        }
    }
}
