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
using System.Diagnostics;
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace GameFrameX.Foundation.Options.Examples.Demos
{
    /// <summary>
    /// 对比演示配置类
    /// </summary>
    public class ComparisonDemoConfig
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
        /// 数据库连接字符串
        /// </summary>
        [OptionAttribute("database-url", Required = true)]
        public string DatabaseUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// 对比演示类
    /// </summary>
    public static class ComparisonDemo
    {
        /// <summary>
        /// 运行对比演示
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void Run(string[] args)
        {
            Console.WriteLine("=== 对比演示 ===");
            Console.WriteLine();

            // 如果没有参数，使用默认测试参数
            if (args.Length == 0)
            {
                Console.WriteLine("📝 使用默认测试参数...");
                args = new[] { "--app-name", "ComparisonDemo", "--host", "compare.example.com", "--port", "9090", "--debug", "--database-url", "postgresql://localhost:5432/comparedb" };
                Console.WriteLine($"   参数: {string.Join(" ", args)}");
                Console.WriteLine();
            }

            try
            {
                Console.WriteLine("⚖️  传统方式 vs 新方式对比");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                CompareTraditionalVsNew(args);
                Console.WriteLine();

                Console.WriteLine("🚀 性能对比测试");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                PerformanceComparison(args);
                Console.WriteLine();

                Console.WriteLine("📊 功能特性对比");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                FeatureComparison();
                Console.WriteLine();

                Console.WriteLine("💡 使用场景推荐");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                UsageRecommendations();
                Console.WriteLine();

                Console.WriteLine("🔄 迁移指南");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                MigrationGuide();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 演示过程中发生错误: {ex.Message}");
                Console.WriteLine($"   堆栈跟踪: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// 对比传统方式与新方式
        /// </summary>
        /// <param name="args">命令行参数</param>
        private static void CompareTraditionalVsNew(string[] args)
        {
            Console.WriteLine("🔄 传统方式（需要创建对象）:");
            Console.WriteLine("```csharp");
            Console.WriteLine("var builder = new OptionsBuilder<AppConfig>(args);");
            Console.WriteLine("var config = builder.Build();");
            Console.WriteLine("```");
            Console.WriteLine();
            
            // 传统方式
            var builder = new OptionsBuilder<ComparisonDemoConfig>(args);
            var traditionalConfig = builder.Build(skipValidation: true);
            
            Console.WriteLine("✅ 传统方式结果:");
            PrintConfig("传统方式", traditionalConfig);
            Console.WriteLine();

            Console.WriteLine("⚡ 新的静态方法（一行搞定）:");
            Console.WriteLine("```csharp");
            Console.WriteLine("var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine("```");
            Console.WriteLine();
            
            // 静态方法
            var staticConfig = OptionsBuilder.Create<ComparisonDemoConfig>(args, skipValidation: true);
            
            Console.WriteLine("✅ 静态方法结果:");
            PrintConfig("静态方法", staticConfig);
            Console.WriteLine();

            Console.WriteLine("🎯 OptionsProvider 方式（推荐）:");
            Console.WriteLine("```csharp");
            Console.WriteLine("OptionsProvider.Initialize(args);");
            Console.WriteLine("var config = OptionsProvider.GetOptions<AppConfig>();");
            Console.WriteLine("```");
            Console.WriteLine();
            
            // OptionsProvider 方式
            OptionsProvider.Initialize(args);
            var providerConfig = OptionsProvider.GetOptions<ComparisonDemoConfig>(enableDebugOutput: false, skipValidation: true);
            
            Console.WriteLine("✅ OptionsProvider 结果:");
            PrintConfig("OptionsProvider", providerConfig);
            Console.WriteLine();

            Console.WriteLine("📈 代码简洁度对比:");
            Console.WriteLine("   传统方式: 2行代码");
            Console.WriteLine("   静态方法: 1行代码 ⭐");
            Console.WriteLine("   OptionsProvider: 2行代码（但支持全局缓存）⭐⭐");
        }

        /// <summary>
        /// 性能对比测试
        /// </summary>
        /// <param name="args">命令行参数</param>
        private static void PerformanceComparison(string[] args)
        {
            const int iterations = 1000;
            
            Console.WriteLine($"📊 执行 {iterations} 次解析的性能对比:");
            Console.WriteLine();

            // 预热
            for (int i = 0; i < 10; i++)
            {
                var _ = OptionsBuilder.Create<ComparisonDemoConfig>(args, skipValidation: true);
            }

            // 测试传统方式
            var sw1 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var builder = new OptionsBuilder<ComparisonDemoConfig>(args);
                var config = builder.Build(skipValidation: true);
            }
            sw1.Stop();

            // 测试静态方法
            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var config = OptionsBuilder.Create<ComparisonDemoConfig>(args, skipValidation: true);
            }
            sw2.Stop();

            // 测试 OptionsProvider（带缓存）
            OptionsProvider.Initialize(args);
            var sw3 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var config = OptionsProvider.GetOptions<ComparisonDemoConfig>(enableDebugOutput: false, skipValidation: true);
            }
            sw3.Stop();

            Console.WriteLine("⏱️  性能测试结果:");
            Console.WriteLine($"   传统方式: {sw1.ElapsedMilliseconds}ms (平均 {sw1.ElapsedMilliseconds / (double)iterations:F3}ms/次)");
            Console.WriteLine($"   静态方法: {sw2.ElapsedMilliseconds}ms (平均 {sw2.ElapsedMilliseconds / (double)iterations:F3}ms/次)");
            Console.WriteLine($"   OptionsProvider: {sw3.ElapsedMilliseconds}ms (平均 {sw3.ElapsedMilliseconds / (double)iterations:F3}ms/次) ⭐");
            Console.WriteLine();

            var improvement1 = ((double)sw1.ElapsedMilliseconds - sw2.ElapsedMilliseconds) / sw1.ElapsedMilliseconds * 100;
            var improvement2 = ((double)sw1.ElapsedMilliseconds - sw3.ElapsedMilliseconds) / sw1.ElapsedMilliseconds * 100;

            Console.WriteLine("📈 性能提升:");
            Console.WriteLine($"   静态方法相比传统方式: {improvement1:F1}%");
            Console.WriteLine($"   OptionsProvider相比传统方式: {improvement2:F1}% (得益于缓存机制)");
            Console.WriteLine();

            Console.WriteLine("💡 性能说明:");
            Console.WriteLine("   ✅ 静态方法减少了对象创建开销");
            Console.WriteLine("   ✅ OptionsProvider 的缓存机制在多次调用时性能最佳");
            Console.WriteLine("   ✅ 首次解析后，OptionsProvider 几乎无额外开销");
        }

        /// <summary>
        /// 功能特性对比
        /// </summary>
        private static void FeatureComparison()
        {
            Console.WriteLine("📋 功能特性对比表:");
            Console.WriteLine();
            Console.WriteLine("┌─────────────────────────┬─────────────┬─────────────┬─────────────────┐");
            Console.WriteLine("│ 功能特性                │ 传统方式    │ 静态方法    │ OptionsProvider │");
            Console.WriteLine("├─────────────────────────┼─────────────┼─────────────┼─────────────────┤");
            Console.WriteLine("│ 代码简洁度              │ ⭐⭐        │ ⭐⭐⭐      │ ⭐⭐⭐          │");
            Console.WriteLine("│ 性能表现                │ ⭐⭐        │ ⭐⭐⭐      │ ⭐⭐⭐⭐        │");
            Console.WriteLine("│ 内存使用                │ ⭐⭐        │ ⭐⭐⭐      │ ⭐⭐⭐⭐        │");
            Console.WriteLine("│ 调试支持                │ ❌          │ ✅          │ ✅              │");
            Console.WriteLine("│ 缓存机制                │ ❌          │ ❌          │ ✅              │");
            Console.WriteLine("│ 全局配置                │ ❌          │ ❌          │ ✅              │");
            Console.WriteLine("│ 环境感知                │ ❌          │ ❌          │ ✅              │");
            Console.WriteLine("│ 多种创建方式            │ ❌          │ ✅          │ ✅              │");
            Console.WriteLine("│ 安全创建                │ ❌          │ ✅          │ ✅              │");
            Console.WriteLine("│ 向后兼容                │ ✅          │ ✅          │ ✅              │");
            Console.WriteLine("│ 学习成本                │ 低          │ 低          │ 中              │");
            Console.WriteLine("└─────────────────────────┴─────────────┴─────────────┴─────────────────┘");
            Console.WriteLine();

            Console.WriteLine("🎯 详细说明:");
            Console.WriteLine();
            Console.WriteLine("📌 传统方式 (OptionsBuilder 实例):");
            Console.WriteLine("   ✅ 最基础的使用方式，学习成本低");
            Console.WriteLine("   ✅ 完全向后兼容");
            Console.WriteLine("   ❌ 需要创建中间对象");
            Console.WriteLine("   ❌ 代码相对冗长");
            Console.WriteLine();

            Console.WriteLine("📌 静态方法 (OptionsBuilder.Create):");
            Console.WriteLine("   ✅ 代码简洁，一行搞定");
            Console.WriteLine("   ✅ 提供多种创建方式");
            Console.WriteLine("   ✅ 支持安全创建 (TryCreate)");
            Console.WriteLine("   ✅ 支持调试模式 (CreateWithDebug)");
            Console.WriteLine("   ❌ 每次调用都重新解析");
            Console.WriteLine();

            Console.WriteLine("📌 OptionsProvider (推荐):");
            Console.WriteLine("   ✅ 全局配置管理");
            Console.WriteLine("   ✅ 智能缓存机制");
            Console.WriteLine("   ✅ 环境感知调试");
            Console.WriteLine("   ✅ 默认启用调试模式");
            Console.WriteLine("   ✅ 多种解析方式");
            Console.WriteLine("   ⚠️  需要先初始化");
        }

        /// <summary>
        /// 使用场景推荐
        /// </summary>
        private static void UsageRecommendations()
        {
            Console.WriteLine("💡 不同场景的推荐方案:");
            Console.WriteLine();

            Console.WriteLine("🎯 场景1: 简单的控制台应用");
            Console.WriteLine("   推荐: 静态方法 ⭐⭐⭐");
            Console.WriteLine("   理由: 代码简洁，一次性使用");
            Console.WriteLine("   示例: var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine();

            Console.WriteLine("🎯 场景2: Web应用或服务");
            Console.WriteLine("   推荐: OptionsProvider ⭐⭐⭐⭐");
            Console.WriteLine("   理由: 全局配置，缓存机制，调试友好");
            Console.WriteLine("   示例: OptionsProvider.Initialize(args); // 启动时");
            Console.WriteLine("         var config = OptionsProvider.GetOptions<AppConfig>(); // 使用时");
            Console.WriteLine();

            Console.WriteLine("🎯 场景3: 微服务架构");
            Console.WriteLine("   推荐: OptionsProvider ⭐⭐⭐⭐");
            Console.WriteLine("   理由: 环境感知，调试模式，容器友好");
            Console.WriteLine("   示例: 自动根据环境启用/禁用调试输出");
            Console.WriteLine();

            Console.WriteLine("🎯 场景4: 开发调试");
            Console.WriteLine("   推荐: CreateWithDebug 或 OptionsProvider ⭐⭐⭐⭐");
            Console.WriteLine("   理由: 详细的调试信息");
            Console.WriteLine("   示例: var config = OptionsBuilder<AppConfig>.CreateWithDebug(args);");
            Console.WriteLine();

            Console.WriteLine("🎯 场景5: 单元测试");
            Console.WriteLine("   推荐: CreateDefault 或 CreateFromArgsOnly ⭐⭐⭐");
            Console.WriteLine("   理由: 隔离环境变量，可预测的行为");
            Console.WriteLine("   示例: var config = OptionsBuilder<AppConfig>.CreateDefault();");
            Console.WriteLine();

            Console.WriteLine("🎯 场景6: 库或框架开发");
            Console.WriteLine("   推荐: 传统方式 ⭐⭐⭐");
            Console.WriteLine("   理由: 最大兼容性，用户可控");
            Console.WriteLine("   示例: 让用户选择使用方式");
            Console.WriteLine();

            Console.WriteLine("🎯 场景7: 生产环境部署");
            Console.WriteLine("   推荐: OptionsProvider + 环境变量控制 ⭐⭐⭐⭐");
            Console.WriteLine("   理由: 可通过环境变量控制调试输出");
            Console.WriteLine("   示例: export GAMEFRAMEX_OPTIONS_DEBUG=false");
        }

        /// <summary>
        /// 迁移指南
        /// </summary>
        private static void MigrationGuide()
        {
            Console.WriteLine("🔄 从传统方式迁移到新方式:");
            Console.WriteLine();

            Console.WriteLine("📋 步骤1: 简单替换（最小改动）");
            Console.WriteLine("   旧代码:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   var builder = new OptionsBuilder<AppConfig>(args);");
            Console.WriteLine("   var config = builder.Build();");
            Console.WriteLine("   ```");
            Console.WriteLine();
            Console.WriteLine("   新代码:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine("   ```");
            Console.WriteLine();

            Console.WriteLine("📋 步骤2: 升级到 OptionsProvider（推荐）");
            Console.WriteLine("   在应用启动时:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   // Program.cs 或 Startup.cs");
            Console.WriteLine("   OptionsProvider.Initialize(args);");
            Console.WriteLine("   ```");
            Console.WriteLine();
            Console.WriteLine("   在需要配置的地方:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   var config = OptionsProvider.GetOptions<AppConfig>();");
            Console.WriteLine("   ```");
            Console.WriteLine();

            Console.WriteLine("📋 步骤3: 启用调试模式（开发环境）");
            Console.WriteLine("   开发环境:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   #if DEBUG");
            Console.WriteLine("       var config = OptionsBuilder<AppConfig>.CreateWithDebug(args);");
            Console.WriteLine("   #else");
            Console.WriteLine("       var config = OptionsBuilder<AppConfig>.Create(args);");
            Console.WriteLine("   #endif");
            Console.WriteLine("   ```");
            Console.WriteLine();
            Console.WriteLine("   或使用 OptionsProvider 的环境感知:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   // 自动根据环境启用调试");
            Console.WriteLine("   var config = OptionsProvider.GetOptions<AppConfig>();");
            Console.WriteLine("   ```");
            Console.WriteLine();

            Console.WriteLine("📋 步骤4: 错误处理优化");
            Console.WriteLine("   使用安全创建:");
            Console.WriteLine("   ```csharp");
            Console.WriteLine("   if (OptionsBuilder<AppConfig>.TryCreate(args, out var config, out var error))");
            Console.WriteLine("   {");
            Console.WriteLine("       // 使用 config");
            Console.WriteLine("   }");
            Console.WriteLine("   else");
            Console.WriteLine("   {");
            Console.WriteLine("       Console.WriteLine($\"配置错误: {error}\");");
            Console.WriteLine("       // config 包含默认配置");
            Console.WriteLine("   }");
            Console.WriteLine("   ```");
            Console.WriteLine();

            Console.WriteLine("⚠️  迁移注意事项:");
            Console.WriteLine("   ✅ 所有新方法都向后兼容");
            Console.WriteLine("   ✅ 可以逐步迁移，不需要一次性全部更改");
            Console.WriteLine("   ✅ 原有的特性和功能保持不变");
            Console.WriteLine("   ⚠️  OptionsProvider 需要先初始化");
            Console.WriteLine("   ⚠️  调试模式默认启用，生产环境可能需要禁用");
        }

        /// <summary>
        /// 打印配置信息
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="config">配置对象</param>
        private static void PrintConfig(string method, ComparisonDemoConfig config)
        {
            Console.WriteLine($"   应用名称: {config.AppName}");
            Console.WriteLine($"   服务器: {config.Host}:{config.Port}");
            Console.WriteLine($"   调试模式: {config.Debug}");
            Console.WriteLine($"   日志级别: {config.LogLevel}");
            Console.WriteLine($"   数据库URL: {config.DatabaseUrl}");
        }
    }
}
