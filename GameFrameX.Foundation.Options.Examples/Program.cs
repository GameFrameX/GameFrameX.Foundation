// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using GameFrameX.Foundation.Options.Examples.Demos;

namespace GameFrameX.Foundation.Options.Examples
{
    /// <summary>
    /// GameFrameX Foundation Options 示例程序主入口
    /// </summary>
    class Program
    {
        /// <summary>
        /// 程序主入口点
        /// </summary>
        /// <param name="args">命令行参数</param>
        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            GameFrameX Foundation Options 示例程序             ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (args.Length == 0)
            {
                ShowMenu();
                return;
            }

            // 解析命令行参数来决定运行哪个示例
            var command = args[0].ToLowerInvariant();
            var demoArgs = args.Length > 1 ? args[1..] : Array.Empty<string>();

            try
            {
                switch (command)
                {
                    case "basic":
                    case "1":
                        Console.WriteLine("🚀 运行基础使用示例...");
                        Console.WriteLine();
                        BasicUsageDemo.Run(demoArgs);
                        break;

                    case "static":
                    case "2":
                        Console.WriteLine("🚀 运行静态方法示例...");
                        Console.WriteLine();
                        StaticMethodsDemo.Run(demoArgs);
                        break;

                    case "debug":
                    case "3":
                        Console.WriteLine("🚀 运行调试模式示例...");
                        Console.WriteLine();
                        DebugModeDemo.Run(demoArgs);
                        break;

                    case "advanced":
                    case "4":
                        Console.WriteLine("🚀 运行高级特性示例...");
                        Console.WriteLine();
                        AdvancedFeaturesDemo.Run(demoArgs);
                        break;

                    case "comparison":
                    case "5":
                        Console.WriteLine("🚀 运行对比演示...");
                        Console.WriteLine();
                        ComparisonDemo.Run(demoArgs);
                        break;

                    case "all":
                        Console.WriteLine("🚀 运行所有示例...");
                        Console.WriteLine();
                        RunAllDemos(demoArgs);
                        break;

                    case "help":
                    case "-h":
                    case "--help":
                        ShowHelp();
                        break;

                    default:
                        Console.WriteLine($"❌ 未知命令: {command}");
                        Console.WriteLine();
                        ShowHelp();
                        Environment.Exit(1);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 运行示例时发生错误: {ex.Message}");
                Console.WriteLine($"   堆栈跟踪: {ex.StackTrace}");
                Environment.Exit(1);
            }

            Console.WriteLine();
            Console.WriteLine("✅ 示例程序运行完成！");
        }

        /// <summary>
        /// 显示交互式菜单
        /// </summary>
        private static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("请选择要运行的示例:");
                Console.WriteLine();
                Console.WriteLine("1. 基础使用示例 (basic)");
                Console.WriteLine("2. 静态方法示例 (static)");
                Console.WriteLine("3. 调试模式示例 (debug)");
                Console.WriteLine("4. 高级特性示例 (advanced)");
                Console.WriteLine("5. 对比演示 (comparison)");
                Console.WriteLine("6. 运行所有示例 (all)");
                Console.WriteLine("7. 显示帮助 (help)");
                Console.WriteLine("0. 退出 (exit)");
                Console.WriteLine();
                Console.Write("请输入选项 (0-7): ");

                var input = Console.ReadLine()?.Trim().ToLowerInvariant();
                Console.WriteLine();

                try
                {
                    switch (input)
                    {
                        case "1":
                        case "basic":
                            BasicUsageDemo.Run(Array.Empty<string>());
                            break;

                        case "2":
                        case "static":
                            StaticMethodsDemo.Run(Array.Empty<string>());
                            break;

                        case "3":
                        case "debug":
                            DebugModeDemo.Run(Array.Empty<string>());
                            break;

                        case "4":
                        case "advanced":
                            AdvancedFeaturesDemo.Run(Array.Empty<string>());
                            break;

                        case "5":
                        case "comparison":
                            ComparisonDemo.Run(Array.Empty<string>());
                            break;

                        case "6":
                        case "all":
                            RunAllDemos(Array.Empty<string>());
                            break;

                        case "7":
                        case "help":
                            ShowHelp();
                            break;

                        case "0":
                        case "exit":
                        case "quit":
                            Console.WriteLine("👋 再见！");
                            return;

                        default:
                            Console.WriteLine($"❌ 无效选项: {input}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ 运行示例时发生错误: {ex.Message}");
                }

                Console.WriteLine();
                Console.WriteLine("按任意键继续...");
                Console.ReadKey();
                Console.Clear();
                
                Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║            GameFrameX Foundation Options 示例程序             ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 运行所有演示
        /// </summary>
        /// <param name="args">命令行参数</param>
        private static void RunAllDemos(string[] args)
        {
            var demos = new (string Name, Action<string[]> Action)[]
            {
                ("基础使用示例", BasicUsageDemo.Run),
                ("静态方法示例", StaticMethodsDemo.Run),
                ("调试模式示例", DebugModeDemo.Run),
                ("高级特性示例", AdvancedFeaturesDemo.Run),
                ("对比演示", ComparisonDemo.Run)
            };

            for (int i = 0; i < demos.Length; i++)
            {
                var (name, action) = demos[i];
                
                Console.WriteLine($"🎯 [{i + 1}/{demos.Length}] {name}");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                
                try
                {
                    action(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ {name} 运行失败: {ex.Message}");
                }
                
                if (i < demos.Length - 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// 显示帮助信息
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("GameFrameX Foundation Options 示例程序");
            Console.WriteLine();
            Console.WriteLine("用法:");
            Console.WriteLine("  GameFrameX.Foundation.Options.Examples.exe [命令] [参数...]");
            Console.WriteLine();
            Console.WriteLine("命令:");
            Console.WriteLine("  basic      运行基础使用示例");
            Console.WriteLine("  static     运行静态方法示例");
            Console.WriteLine("  debug      运行调试模式示例");
            Console.WriteLine("  advanced   运行高级特性示例");
            Console.WriteLine("  comparison 运行对比演示");
            Console.WriteLine("  all        运行所有示例");
            Console.WriteLine("  help       显示此帮助信息");
            Console.WriteLine();
            Console.WriteLine("示例:");
            Console.WriteLine("  # 运行基础示例");
            Console.WriteLine("  GameFrameX.Foundation.Options.Examples.exe basic --app-name MyApp --port 8080");
            Console.WriteLine();
            Console.WriteLine("  # 运行调试模式示例");
            Console.WriteLine("  GameFrameX.Foundation.Options.Examples.exe debug --host example.com --debug");
            Console.WriteLine();
            Console.WriteLine("  # 交互式菜单（无参数启动）");
            Console.WriteLine("  GameFrameX.Foundation.Options.Examples.exe");
            Console.WriteLine();
            Console.WriteLine("注意:");
            Console.WriteLine("  - 如果不提供命令，程序将启动交互式菜单");
            Console.WriteLine("  - 所有示例都支持 --help 参数查看具体用法");
            Console.WriteLine("  - 可以使用数字 1-6 代替命令名称");
        }
    }
}