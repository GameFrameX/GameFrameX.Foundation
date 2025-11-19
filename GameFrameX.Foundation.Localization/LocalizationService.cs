// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// 简化的本地化服务 - 轻量级实现的静态入口点
/// </summary>
/// <remarks>
/// 提供统一的本地化字符串获取接口，支持懒加载机制。
/// 该服务作为整个本地化系统的主要入口点，隐藏了内部的复杂性。
/// 使用方只需要调用静态方法即可获取本地化字符串，无需关心具体的实现细节。
/// </remarks>
/// <example>
/// <code>
/// // 基础使用
/// var message = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
///
/// // 带参数的格式化消息
/// var formatted = LocalizationService.GetString("Encryption.InvalidKeySize", keySize, expectedSize);
///
/// // 确保资源已加载（可选，通常不需要）
/// LocalizationService.EnsureLoaded();
/// </code>
/// </example>
public static class LocalizationService
{
    private static readonly Lazy<ResourceManager> _instance = new Lazy<ResourceManager>(() => new ResourceManager());

    /// <summary>
    /// 获取本地化服务实例
    /// </summary>
    /// <value>
    /// ResourceManager 的单例实例，提供线程安全的访问。
    /// </value>
    /// <remarks>
    /// 使用 Lazy&lt;T&gt; 确保实例的延迟初始化和线程安全。
    /// </remarks>
    public static ResourceManager Instance
    {
        get { return _instance.Value; }
    }

    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key">资源键，用于标识特定的本地化字符串</param>
    /// <returns>
    /// 如果找到对应的本地化字符串，返回该字符串；
    /// 如果未找到或 key 为 null/空，返回传入的键
    /// </returns>
    /// <remarks>
    /// 这是获取本地化字符串的主要方法。
    /// 自动按优先级顺序查询所有注册的资源提供者
    /// 支持懒加载，首次使用时自动发现和加载资源
    /// 线程安全，支持并发调用
    /// </remarks>
    /// <example>
    /// <code>
    /// var message = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
    /// // 返回：优先从程序集资源查找，如果找不到则使用默认英文消息
    ///
    /// var unknown = LocalizationService.GetString("Some.Unknown.Key");
    /// // 返回："Some.Unknown.Key"
    /// </code>
    /// </example>
    public static string GetString(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }

        return Instance.GetString(key);
    }

    /// <summary>
    /// 获取格式化的本地化字符串
    /// </summary>
    /// <param name="key">资源键</param>
    /// <param name="args">格式化参数</param>
    /// <returns>
    /// 格式化后的本地化字符串
    /// </returns>
    /// <remarks>
    /// 首先获取本地化字符串模板，然后使用 string.Format 进行格式化。
    /// 如果资源键未找到，将使用键名作为模板进行格式化。
    /// </remarks>
    /// <example>
    /// <code>
    /// var message = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);
    /// // 返回："Invalid key size: 128. Expected length: 256."
    ///
    /// var unknown = LocalizationService.GetString("Unknown.Message", "param1", "param2");
    /// // 返回："Unknown.Message param1 param2"
    /// </code>
    /// </example>
    public static string GetString(string key, params object[] args)
    {
        var template = GetString(key);
        return args?.Length > 0 ? string.Format(template, args) : template;
    }

    /// <summary>
    /// 确保所有资源已加载
    /// </summary>
    /// <remarks>
    /// 此方法是可选的，通常情况下不需要手动调用。
    /// 本地化系统采用懒加载机制，会在首次使用时自动加载资源
    /// 调用此方法可以预先加载所有资源，避免首次访问时的延迟
    /// 支持多次调用，不会产生副作用
    /// </remarks>
    /// <example>
    /// <code>
    /// // 应用启动时预加载本地化资源（可选）
    /// LocalizationService.EnsureLoaded();
    ///
    /// // 之后的使用将不会触发资源加载延迟
    /// var message = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
    /// </code>
    /// </example>
    public static void EnsureLoaded()
    {
        Instance.EnsureProvidersLoaded();
    }

    /// <summary>
    /// 获取本地化系统的统计信息
    /// </summary>
    /// <returns>
    /// 包含资源管理器统计信息的对象
    /// </returns>
    /// <remarks>
    /// 可以用于监控和调试本地化系统的状态。
    /// 主要包括：
    /// - 提供者加载状态
    /// - 提供者数量和类型
    /// - 程序集资源加载情况
    /// </remarks>
    /// <example>
    /// <code>
    /// var stats = LocalizationService.GetStatistics();
    /// Console.WriteLine($"Providers loaded: {stats.ProvidersLoaded}");
    /// Console.WriteLine($"Total providers: {stats.TotalProviderCount}");
    /// Console.WriteLine($"Assembly providers: {stats.AssemblyProviderCount}");
    /// Console.WriteLine($"Default provider exists: {stats.DefaultProviderExists}");
    /// </code>
    /// </example>
    public static ResourceManagerStatistics GetStatistics()
    {
        return Instance.GetStatistics();
    }

    /// <summary>
    /// 手动注册自定义资源提供者
    /// </summary>
    /// <param name="provider">要注册的资源提供者</param>
    /// <exception cref="ArgumentNullException">当 provider 为 null 时抛出</exception>
    /// <remarks>
    /// 手动注册的提供者具有最高优先级
    /// 主要用于测试或集成自定义资源提供者
    /// 可以多次调用，后注册的提供者优先级更高
    /// </remarks>
    /// <example>
    /// <code>
    /// var customProvider = new DatabaseResourceProvider();
    /// LocalizationService.RegisterProvider(customProvider);
    ///
    /// customProvider 现在具有最高优先级
    /// var message = LocalizationService.GetString("Custom.Message");
    /// </code>
    /// </example>
    public static void RegisterProvider(IResourceProvider provider)
    {
        Instance.RegisterProvider(provider);
    }

    /// <summary>
    /// 获取所有已注册的资源提供者
    /// </summary>
    /// <returns>
    /// 包含所有资源提供者的只读列表
    /// </returns>
    /// <remarks>
    /// 提供者按优先级排序，索引越小优先级越高
    /// 返回的列表是只读的，不能直接修改
    /// 可以用于调试和监控
    /// </remarks>
    /// <example>
    /// <code>
    /// var providers = LocalizationService.GetProviders();
    /// foreach (var provider in providers)
    /// {
    ///     Console.WriteLine($"Provider: {provider.GetType().Name}");
    /// }
    /// </code>
    /// </example>
    public static IReadOnlyList<IResourceProvider> GetProviders()
    {
        return Instance.GetProviders();
    }

    /// <summary>
    /// 释放本地化系统占用的资源
    /// </summary>
    /// <remarks>
    /// 主要用于应用程序关闭时的清理工作
    /// 调用此方法后，本地化服务将不再可用
    /// 通常不需要手动调用，由应用程序生命周期管理
    /// </remarks>
    /// <example>
    /// <code>
    /// // 应用程序关闭时
    /// LocalizationService.Dispose();
    /// </code>
    /// </example>
    public static void Dispose()
    {
        if (_instance.IsValueCreated)
        {
            _instance.Value.Dispose();
        }
    }
}