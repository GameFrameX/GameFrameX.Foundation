// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项提供者，用于获取和缓存配置选项
/// </summary>
public static class OptionsProvider
{
    private static readonly Dictionary<Type, object> OptionsCache = new Dictionary<Type, object>();
    private static string[] _args;

    /// <summary>
    /// 初始化选项提供者
    /// </summary>
    /// <param name="args">命令行参数</param>
    public static void Initialize(string[] args)
    {
        _args = args ?? Array.Empty<string>();
        OptionsCache.Clear();
    }

    /// <summary>
    /// 检查是否应该启用调试输出
    /// </summary>
    /// <param name="enableDebugOutput">用户指定的调试输出设置</param>
    /// <returns>最终的调试输出设置</returns>
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
    /// 获取选项
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="skipValidation">是否跳过验证</param>
    /// <param name="enableDebugOutput">是否启用调试输出（null表示使用自动检测）</param>
    /// <returns>选项对象</returns>
    public static T GetOptions<T>(bool skipValidation = false, bool? enableDebugOutput = null) where T : class, new()
    {
        var type = typeof(T);
        var shouldDebug = ShouldEnableDebugOutput(enableDebugOutput);

        // 如果缓存中已存在，直接返回
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

        // 创建选项构建器
        var builder = new OptionsBuilder<T>(_args ?? Array.Empty<string>());

        // 构建选项
        var options = builder.Build(skipValidation);

        // 缓存选项
        OptionsCache[type] = options;

        // 如果启用调试输出，打印解析后的选项对象
        if (shouldDebug)
        {
            OptionsDebugger.PrintParsedOptions(options);
        }

        return options;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public static void ClearCache()
    {
        OptionsCache.Clear();
    }

    /// <summary>
    /// 从缓存中移除指定类型的选项
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    public static void RemoveFromCache<T>() where T : class
    {
        var type = typeof(T);
        if (OptionsCache.ContainsKey(type))
        {
            OptionsCache.Remove(type);
        }
    }

    /// <summary>
    /// 解析命令行参数并显示调试信息（强制启用调试输出）
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="skipValidation">是否跳过验证</param>
    /// <returns>解析后的选项对象</returns>
    public static T ParseWithDebug<T>(string[] args, bool skipValidation = false) where T : class, new()
    {
        // 初始化参数
        Initialize(args);
        
        // 获取选项并强制启用调试输出
        return GetOptions<T>(skipValidation, enableDebugOutput: true);
    }

    /// <summary>
    /// 解析命令行参数（静默模式，禁用调试输出）
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="skipValidation">是否跳过验证</param>
    /// <returns>解析后的选项对象</returns>
    public static T ParseSilent<T>(string[] args, bool skipValidation = false) where T : class, new()
    {
        // 初始化参数
        Initialize(args);
        
        // 获取选项并禁用调试输出
        return GetOptions<T>(skipValidation, enableDebugOutput: false);
    }

    /// <summary>
    /// 打印已解析的选项对象信息
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="options">选项对象</param>
    public static void PrintOptionsInfo<T>(T options) where T : class
    {
        OptionsDebugger.PrintParsedOptions(options);
    }

    /// <summary>
    /// 设置全局调试模式（通过环境变量）
    /// </summary>
    /// <param name="enabled">是否启用调试模式</param>
    public static void SetGlobalDebugMode(bool enabled)
    {
        Environment.SetEnvironmentVariable("GAMEFRAMEX_OPTIONS_DEBUG", enabled.ToString());
    }

    /// <summary>
    /// 获取当前调试模式状态
    /// </summary>
    /// <returns>当前是否启用调试模式</returns>
    public static bool IsDebugModeEnabled()
    {
        return ShouldEnableDebugOutput();
    }
}
