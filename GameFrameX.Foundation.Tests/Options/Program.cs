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

using GameFrameX.Foundation.Options.Examples.Demos;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// GameFrameX Foundation Options 示例程序主入口
    /// </summary>
    public static class ProgramOptions
    {
        /// <summary>
        /// 程序主入口点
        /// </summary>
        /// <param name="args">命令行参数</param>
        public static void Entry(string[] args)
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