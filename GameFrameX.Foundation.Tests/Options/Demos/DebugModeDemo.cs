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
    /// 调试模式演示配置类
    /// </summary>
    public class DebugModeDemoConfig
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
        [EnvironmentVariableAttribute("SERVER_HOST")]
        public string Host { get; set; } = "localhost";

        /// <summary>
        /// 服务器端口号
        /// </summary>
        [OptionAttribute("port", DefaultValue = 8080)]
        [EnvironmentVariableAttribute("SERVER_PORT")]
        public int Port { get; set; } = 8080;

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        [FlagOptionAttribute("debug")]
        [EnvironmentVariableAttribute("DEBUG")]
        public bool Debug { get; set; } = false;

        /// <summary>
        /// 日志级别
        /// </summary>
        [OptionAttribute("log-level", DefaultValue = "Info")]
        [EnvironmentVariableAttribute("LOG_LEVEL")]
        public string LogLevel { get; set; } = "Info";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [OptionAttribute("database-url", Required = true)]
        [EnvironmentVariableAttribute("DATABASE_URL")]
        public string DatabaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// API密钥
        /// </summary>
        [OptionAttribute("api-key")]
        [EnvironmentVariableAttribute("API_KEY")]
        public string? ApiKey { get; set; }

        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        [OptionAttribute("timeout", DefaultValue = 30.0)]
        public double Timeout { get; set; } = 30.0;
    }

    /// <summary>
    /// 调试模式演示
    /// </summary>
    public static class DebugModeDemo
    {
        /// <summary>
        /// 运行调试模式演示
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void Run(string[] args)
        {
            Console.WriteLine("=== 调试模式演示 ===");
            Console.WriteLine();

            // 如果没有参数，使用默认测试参数
            if (args.Length == 0)
            {
                Console.WriteLine("📝 使用默认测试参数...");
                args = new[] { "--app-name", "DebugDemo", "--host", "debug.example.com", "--port", "9090", "--debug", "--database-url", "postgresql://localhost:5432/debugdb" };
                Console.WriteLine($"   参数: {string.Join(" ", args)}");
                Console.WriteLine();
            }

            try
            {
                Console.WriteLine("🔧 测试1: 默认调试模式（应该显示调试信息）");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // 初始化选项提供者
                OptionsProvider.Initialize(args);
                
                // 获取配置（使用默认调试设置）
                var options1 = OptionsProvider.GetOptions<DebugModeDemoConfig>();
                
                Console.WriteLine("✅ 测试1完成！");
                Console.WriteLine();
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Console.WriteLine("🔧 测试2: 强制禁用调试输出");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // 清除缓存以重新解析
                OptionsProvider.ClearCache();
                
                // 获取配置（禁用调试输出）
                var options2 = OptionsProvider.GetOptions<DebugModeDemoConfig>(enableDebugOutput: false);
                
                Console.WriteLine("✅ 测试2完成！（应该没有调试信息）");
                Console.WriteLine();
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Console.WriteLine("🔧 测试3: 通过环境变量控制调试模式");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // 设置环境变量禁用调试
                Console.WriteLine("📋 设置环境变量: GAMEFRAMEX_OPTIONS_DEBUG=false");
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", "false");
                
                // 清除缓存
                OptionsProvider.ClearCache();
                
                // 获取配置（应该不显示调试信息）
                var options3 = OptionsProvider.GetOptions<DebugModeDemoConfig>();
                
                Console.WriteLine("✅ 测试3完成！（通过环境变量禁用调试）");
                Console.WriteLine();
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Console.WriteLine("🔧 测试4: 使用静默模式");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // 重置环境变量
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", null);
                
                // 使用静默模式
                var options4 = OptionsProvider.ParseSilent<DebugModeDemoConfig>(args);
                
                Console.WriteLine("✅ 测试4完成！（静默模式）");
                Console.WriteLine();
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Console.WriteLine("🔧 测试5: 使用强制调试模式");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // 使用强制调试模式
                var options5 = OptionsProvider.ParseWithDebug<DebugModeDemoConfig>(args);
                
                Console.WriteLine("✅ 测试5完成！（强制调试模式）");
                Console.WriteLine();
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Console.WriteLine("🔧 测试6: 使用OptionsBuilder的调试方法");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();
                
                // 使用OptionsBuilder的调试方法
                var options6 = OptionsBuilder.CreateWithDebug<DebugModeDemoConfig>(args);
                
                Console.WriteLine("✅ 测试6完成！（OptionsBuilder调试方法）");
                Console.WriteLine();

                // 显示最终配置
                Console.WriteLine("🎉 所有测试完成！最终配置:");
                PrintConfig(options6);
                Console.WriteLine();
                Console.WriteLine($"📊 当前调试模式状态: {(OptionsProvider.IsDebugModeEnabled() ? "启用" : "禁用")}");
                Console.WriteLine();

                Console.WriteLine("🌍 演示环境感知功能");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateEnvironmentAwareness();
                Console.WriteLine();

                Console.WriteLine("⚙️  演示调试输出控制");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                DemonstrateDebugOutputControl();

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
        private static void PrintConfig(DebugModeDemoConfig config)
        {
            Console.WriteLine($"   应用名称: {config.AppName}");
            Console.WriteLine($"   服务器: {config.Host}:{config.Port}");
            Console.WriteLine($"   调试模式: {config.Debug}");
            Console.WriteLine($"   日志级别: {config.LogLevel}");
            Console.WriteLine($"   数据库URL: {config.DatabaseUrl}");
            Console.WriteLine($"   API密钥: {config.ApiKey ?? "未设置"}");
            Console.WriteLine($"   超时时间: {config.Timeout}秒");
        }

        /// <summary>
        /// 演示环境感知功能
        /// </summary>
        private static void DemonstrateEnvironmentAwareness()
        {
            string[] testArgs = { "--app-name", "EnvTestApp", "--database-url", "test://db" };

            // 测试不同环境
            string[] environments = { "Development", "Production", "Test", "Staging" };

            foreach (var env in environments)
            {
                Console.WriteLine($"🌍 测试环境: {env}");
                
                // 设置环境变量
                Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", env);
                Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", null); // 清除调试设置
                
                // 清除缓存
                OptionsProvider.ClearCache();
                
                // 检查调试模式状态
                bool debugEnabled = OptionsProvider.IsDebugModeEnabled();
                Console.WriteLine($"   调试模式: {(debugEnabled ? "启用" : "禁用")}");
                
                if (debugEnabled)
                {
                    Console.WriteLine("   （将显示调试信息）");
                }
                else
                {
                    Console.WriteLine("   （静默运行）");
                }
                
                Console.WriteLine();
            }

            // 重置环境
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
        }

        /// <summary>
        /// 演示调试输出控制
        /// </summary>
        private static void DemonstrateDebugOutputControl()
        {
            string[] testArgs = { "--app-name", "ControlTest", "--database-url", "test://control" };

            Console.WriteLine("📋 测试不同的调试控制方式:");
            Console.WriteLine();

            // 1. 全局设置调试模式
            Console.WriteLine("🔧 方式1: 全局设置调试模式");
            OptionsProvider.SetGlobalDebugMode(true);
            Console.WriteLine($"   设置后状态: {(OptionsProvider.IsDebugModeEnabled() ? "启用" : "禁用")}");
            Console.WriteLine();

            // 2. 通过环境变量控制
            Console.WriteLine("🔧 方式2: 通过环境变量控制");
            Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", "false");
            Console.WriteLine($"   设置环境变量后状态: {(OptionsProvider.IsDebugModeEnabled() ? "启用" : "禁用")}");
            Console.WriteLine();

            // 3. 方法参数覆盖
            Console.WriteLine("🔧 方式3: 方法参数覆盖");
            Console.WriteLine("   即使全局禁用，也可以通过参数强制启用:");
            
            OptionsProvider.ClearCache();
            var config = OptionsProvider.GetOptions<DebugModeDemoConfig>(enableDebugOutput: true, skipValidation: true);
            Console.WriteLine("   ✅ 强制启用调试输出完成");
            Console.WriteLine();

            // 4. 不同方法的调试行为
            Console.WriteLine("🔧 方式4: 不同方法的调试行为");
            Console.WriteLine();
            
            Console.WriteLine("   ParseSilent - 始终静默:");
            var silentConfig = OptionsProvider.ParseSilent<DebugModeDemoConfig>(testArgs);
            Console.WriteLine("   ✅ 静默解析完成");
            Console.WriteLine();
            
            Console.WriteLine("   ParseWithDebug - 始终显示调试:");
            var debugConfig = OptionsProvider.ParseWithDebug<DebugModeDemoConfig>(testArgs);
            Console.WriteLine("   ✅ 调试解析完成");
            Console.WriteLine();

            // 清理
            Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", null);
            OptionsProvider.SetGlobalDebugMode(false);
            
            Console.WriteLine("🎯 调试控制优先级（从高到低）:");
            Console.WriteLine("   1. 方法参数 (enableDebugOutput)");
            Console.WriteLine("   2. 环境变量 (GAMEFRAMEX_OPTIONS_DEBUG)");
            Console.WriteLine("   3. 全局设置 (SetGlobalDebugMode)");
            Console.WriteLine("   4. 环境检测 (Development/Production)");
            Console.WriteLine("   5. 默认行为 (启用)");
        }
    }
}
