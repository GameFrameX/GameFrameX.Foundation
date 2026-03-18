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

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项提供者，用于获取和缓存配置选项。
/// </summary>
/// <remarks>
/// Options provider for retrieving and caching configuration options.
/// This class provides thread-safe caching of configuration options and supports automatic debug output detection.
/// </remarks>
public static class OptionsProvider
{
    private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, object> OptionsCache = new();
    private static readonly object _lock = new();
    private static volatile string[] _args;

    /// <summary>
    /// 初始化选项提供者。
    /// </summary>
    /// <remarks>
    /// Initializes the options provider with the specified command-line arguments.
    /// </remarks>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    public static void Initialize(string[] args)
    {
        lock (_lock)
        {
            _args = args ?? Array.Empty<string>();
            OptionsCache.Clear();
        }
    }

    /// <summary>
    /// 检查是否应该启用调试输出。
    /// </summary>
    /// <remarks>
    /// Checks whether debug output should be enabled based on the provided setting, environment variables, and runtime environment.
    /// </remarks>
    /// <param name="enableDebugOutput">用户指定的调试输出设置 / User-specified debug output setting</param>
    /// <returns>最终的调试输出设置 / Final debug output setting</returns>
    private static bool ShouldEnableDebugOutput(bool? enableDebugOutput = null)
    {
        // 如果用户明确指定了设置，使用用户设置
        if (enableDebugOutput.HasValue)
        {
            return enableDebugOutput.Value;
        }

        // 检查环境变量 GAMEFRAMEX_OPTIONS_DEBUG
        var envDebug = Environment.GetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG");
        if (!string.IsNullOrEmpty(envDebug))
        {
            if (bool.TryParse(envDebug, out bool envValue))
            {
                return envValue;
            }
            // 支持更多格式
            var normalizedEnvDebug = envDebug.Trim().ToLowerInvariant();
            if (normalizedEnvDebug is "1" or "yes" or "on" or "enable" or "enabled")
            {
                return true;
            }
            if (normalizedEnvDebug is "0" or "no" or "off" or "disable" or "disabled")
            {
                return false;
            }
        }

        // 检查是否在开发环境中（通过常见的开发环境变量）
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                         ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                         ?? Environment.GetEnvironmentVariable("ENVIRONMENT");

        if (!string.IsNullOrEmpty(environment))
        {
            var normalizedEnv = environment.Trim().ToLowerInvariant();
            // 在开发和测试环境中默认启用调试
            if (normalizedEnv is "development" or "dev" or "test" or "testing" or "debug")
            {
                return true;
            }
            // 在生产环境中默认禁用调试
            if (normalizedEnv is "production" or "prod" or "release")
            {
                return false;
            }
        }

        // 默认启用调试输出（便于部署时验证配置）
        return true;
    }

    /// <summary>
    /// 获取选项。
    /// </summary>
    /// <remarks>
    /// Gets the options object of the specified type. Uses cached instance if available.
    /// </remarks>
    /// <typeparam name="T">选项类型 / Options type</typeparam>
    /// <param name="skipValidation">是否跳过验证 / Whether to skip validation</param>
    /// <param name="enableDebugOutput">是否启用调试输出（<c>null</c> 表示使用自动检测） / Whether to enable debug output (<c>null</c> means auto-detect)</param>
    /// <returns>选项对象 / Options object</returns>
    public static T GetOptions<T>(bool skipValidation = false, bool? enableDebugOutput = null) where T : class, new()
    {
        var type = typeof(T);
        var shouldDebug = ShouldEnableDebugOutput(enableDebugOutput);

        // 第一次检查缓存（无锁快速路径）
        if (OptionsCache.TryGetValue(type, out var cachedOptions))
        {
            var cachedResult = (T)cachedOptions;

            // 如果启用调试输出，打印缓存的选项对象
            if (shouldDebug)
            {
                Console.WriteLine("⚠️  使用缓存的配置对象 (Using cached configuration object)");
                OptionsDebugger.PrintParsedOptions(cachedResult);
            }

            return cachedResult;
        }

        // 缓存未命中，进入锁保护区域
        string[] args;
        lock (_lock)
        {
            // 双重检查：再次检查缓存（可能在等待锁时被其他线程填充）
            if (OptionsCache.TryGetValue(type, out cachedOptions))
            {
                return (T)cachedOptions;
            }

            // 获取参数（在同一个锁内，确保与 Initialize 同步）
            args = _args ?? Array.Empty<string>();
        }

        // 在锁外构建选项（避免长时间持有锁）
        var builder = new OptionsBuilder<T>(args);
        var options = builder.Build(skipValidation);

        // 缓存选项（使用 GetOrAdd 确保线程安全）
        var finalOptions = OptionsCache.GetOrAdd(type, options);

        // 如果是刚添加的，使用新构建的 options；如果是其他线程已添加的，使用缓存的
        var result = ReferenceEquals(finalOptions, options) ? options : (T)finalOptions;

        // 如果启用调试输出，打印解析后的选项对象
        if (shouldDebug)
        {
            OptionsDebugger.PrintParsedOptions(result);
        }

        return result;
    }

    /// <summary>
    /// 清除缓存。
    /// </summary>
    /// <remarks>
    /// Clears all cached options.
    /// </remarks>
    public static void ClearCache()
    {
        OptionsCache.Clear();
    }

    /// <summary>
    /// 从缓存中移除指定类型的选项。
    /// </summary>
    /// <remarks>
    /// Removes the options of the specified type from the cache.
    /// </remarks>
    /// <typeparam name="T">选项类型 / Options type</typeparam>
    public static void RemoveFromCache<T>() where T : class
    {
        var type = typeof(T);
        OptionsCache.TryRemove(type, out _);
    }

    /// <summary>
    /// 解析命令行参数并显示调试信息（强制启用调试输出）。
    /// </summary>
    /// <remarks>
    /// Parses command-line arguments and displays debug information (forces debug output enabled).
    /// </remarks>
    /// <typeparam name="T">选项类型 / Options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="skipValidation">是否跳过验证 / Whether to skip validation</param>
    /// <returns>解析后的选项对象 / Parsed options object</returns>
    public static T ParseWithDebug<T>(string[] args, bool skipValidation = false) where T : class, new()
    {
        // 初始化参数
        Initialize(args);

        // 获取选项并强制启用调试输出
        return GetOptions<T>(skipValidation, enableDebugOutput: true);
    }

    /// <summary>
    /// 解析命令行参数（静默模式，禁用调试输出）。
    /// </summary>
    /// <remarks>
    /// Parses command-line arguments in silent mode (debug output disabled).
    /// </remarks>
    /// <typeparam name="T">选项类型 / Options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="skipValidation">是否跳过验证 / Whether to skip validation</param>
    /// <returns>解析后的选项对象 / Parsed options object</returns>
    public static T ParseSilent<T>(string[] args, bool skipValidation = false) where T : class, new()
    {
        // 初始化参数
        Initialize(args);

        // 获取选项并禁用调试输出
        return GetOptions<T>(skipValidation, enableDebugOutput: false);
    }

    /// <summary>
    /// 打印已解析的选项对象信息。
    /// </summary>
    /// <remarks>
    /// Prints the parsed options object information.
    /// </remarks>
    /// <typeparam name="T">选项类型 / Options type</typeparam>
    /// <param name="options">选项对象 / Options object</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="options"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="options"/> is <c>null</c></exception>
    public static void PrintOptionsInfo<T>(T options) where T : class
    {
        ArgumentNullException.ThrowIfNull(options);
        OptionsDebugger.PrintParsedOptions(options);
    }

    /// <summary>
    /// 设置全局调试模式（通过环境变量）。
    /// </summary>
    /// <remarks>
    /// Sets the global debug mode via environment variable.
    /// </remarks>
    /// <param name="enabled">是否启用调试模式 / Whether to enable debug mode</param>
    public static void SetGlobalDebugMode(bool enabled)
    {
        Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", enabled.ToString());
    }

    /// <summary>
    /// 获取当前调试模式状态。
    /// </summary>
    /// <remarks>
    /// Gets the current debug mode status.
    /// </remarks>
    /// <returns>当前是否启用调试模式 / Whether debug mode is currently enabled</returns>
    public static bool IsDebugModeEnabled()
    {
        return ShouldEnableDebugOutput();
    }
}
