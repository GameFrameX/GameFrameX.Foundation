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

using System.Collections.Concurrent;
using System.Globalization;
using System.Threading;
using GameFrameX.Foundation.Localization.Providers;

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// 轻量级资源管理器 - 协调多个资源提供者
/// </summary>
/// <remarks>
/// 该管理器负责管理和协调多个资源提供者，提供统一的本地化字符串获取接口。
/// 支持懒加载机制，只在需要时才加载和初始化资源提供者。
/// 资源提供者按优先级顺序查询，优先级高的提供者优先被查询。
/// </remarks>
/// <example>
/// <code>
/// var manager = new ResourceManager();
/// var message = manager.GetString("Utility.Exceptions.TimestampOutOfRange");
/// </code>
/// </example>
public class ResourceManager
{
    private readonly List<IResourceProvider> _providers;
    private readonly Lazy<ConcurrentDictionary<string, AssemblyResourceProvider>> _assemblyProviders;
    private volatile bool _providersLoaded;
    private readonly object _loadLock;
    private readonly object _providersLock;
    private readonly ConcurrentDictionary<string, long> _missingKeys;
    private long _missingKeyCount;
    private long _formatFailureCount;

    /// <summary>
    /// 初始化 ResourceManager 的新实例
    /// </summary>
    /// <remarks>
    /// 构造函数创建空的提供者列表，但不立即加载资源提供者。
    /// 资源提供者的发现和加载将在首次使用时进行。
    /// </remarks>
    public ResourceManager()
    {
        _providers = new List<IResourceProvider>();
        _assemblyProviders = new Lazy<ConcurrentDictionary<string, AssemblyResourceProvider>>();
        _missingKeys = new ConcurrentDictionary<string, long>();
        _providersLock = new object();
        var kvs = DiscoverAssemblyProviders();
        foreach (var kv in kvs)
        {
            _assemblyProviders.Value.TryAdd(kv.Key, kv.Value);
        }

        _loadLock = new object();
    }

    /// <summary>
    /// Occurs when a resource key cannot be resolved by any provider or fallback culture.
    /// </summary>
    public event EventHandler<MissingResourceEventArgs> MissingKey;

    /// <summary>
    /// Gets or sets the fallback default culture. The invariant culture represents default resources.
    /// </summary>
    public CultureInfo DefaultCulture { get; set; } = CultureInfo.InvariantCulture;

    /// <summary>
    /// Gets or sets whether parent culture fallback is enabled.
    /// </summary>
    public bool CultureFallbackEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the behavior used when parameter formatting fails.
    /// </summary>
    public LocalizationFormatErrorBehavior FormatErrorBehavior { get; set; } = LocalizationFormatErrorBehavior.ReturnTemplate;

    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key">资源键</param>
    /// <returns>
    /// 如果找到对应的本地化字符串，返回该字符串；
    /// 如果所有提供者都未找到，返回传入的资源键
    /// </returns>
    /// <remarks>
    /// 此方法会按提供者的优先级顺序查询：
    /// 1. 首先从程序集资源提供者查找
    /// 2. 最后从默认资源提供者查找
    /// 一旦找到有效的本地化字符串，立即返回
    /// </remarks>
    /// <example>
    /// <code>
    /// var manager = new ResourceManager();
    /// var message = manager.GetString("Utility.Exceptions.TimestampOutOfRange");
    /// // 优先从程序集资源查找，如果找不到则使用默认消息
    /// </code>
    /// </example>
    public string GetString(string key)
    {
        return GetString(key, CultureInfo.CurrentUICulture);
    }

    /// <summary>
    /// 获取指定区域性的本地化字符串
    /// </summary>
    /// <param name="key">资源键</param>
    /// <param name="culture">区域性</param>
    /// <returns>本地化字符串；未找到时返回资源键</returns>
    public string GetString(string key, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }

        EnsureProvidersLoaded();
        culture ??= CultureInfo.CurrentUICulture;

        IResourceProvider[] providers;
        lock (_providersLock)
        {
            providers = _providers.ToArray();
        }

        var fallbackChain = GetCultureFallbackChain(culture);
        foreach (var fallbackCulture in fallbackChain)
        {
            foreach (var provider in providers)
            {
                try
                {
                    if (TryGetProviderString(provider, key, fallbackCulture, out var value))
                    {
                        return value;
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但不中断查询过程，继续尝试下一个提供者
                    System.Diagnostics.Debug.WriteLine($"Provider {provider.GetType().Name} failed: {ex.Message}");
                }
            }
        }

        RecordMissingKey(key, culture, fallbackChain, providers);
        return key; // 所有提供者都没有找到，返回键名
    }

    /// <summary>
    /// Gets the fallback cultures queried for the specified culture.
    /// </summary>
    /// <param name="culture">The starting culture.</param>
    /// <returns>The ordered fallback chain.</returns>
    public IReadOnlyList<CultureInfo> GetCultureFallbackChain(CultureInfo culture)
    {
        culture ??= CultureInfo.CurrentUICulture;

        var cultures = new List<CultureInfo>();
        if (!CultureFallbackEnabled)
        {
            cultures.Add(culture);
            return cultures.AsReadOnly();
        }

        var current = culture;
        while (current != null)
        {
            AddCultureIfMissing(cultures, current);
            if (Equals(current, CultureInfo.InvariantCulture))
            {
                break;
            }

            current = current.Parent;
        }

        if (DefaultCulture != null)
        {
            AddCultureIfMissing(cultures, DefaultCulture);
        }

        AddCultureIfMissing(cultures, CultureInfo.InvariantCulture);
        return cultures.AsReadOnly();
    }

    /// <summary>
    /// Gets and formats a localized string using the current UI culture.
    /// </summary>
    public string FormatString(string key, params object[] args)
    {
        return FormatString(key, CultureInfo.CurrentUICulture, args);
    }

    /// <summary>
    /// Gets and formats a localized string using an explicit culture.
    /// </summary>
    public string FormatString(string key, CultureInfo culture, params object[] args)
    {
        var template = GetString(key, culture);
        if (args == null || args.Length == 0)
        {
            return template;
        }

        try
        {
            return string.Format(culture ?? CultureInfo.CurrentUICulture, template, args);
        }
        catch (FormatException)
        {
            Interlocked.Increment(ref _formatFailureCount);
            if (FormatErrorBehavior == LocalizationFormatErrorBehavior.Throw)
            {
                throw;
            }

            return FormatErrorBehavior switch
            {
                LocalizationFormatErrorBehavior.ReturnKey => key,
                _ => template
            };
        }
    }

    private static void AddCultureIfMissing(ICollection<CultureInfo> cultures, CultureInfo culture)
    {
        if (!cultures.Any(item => string.Equals(item.Name, culture.Name, StringComparison.OrdinalIgnoreCase)))
        {
            cultures.Add(culture);
        }
    }

    private static bool TryGetProviderString(IResourceProvider provider, string key, CultureInfo culture, out string value)
    {
        value = provider is ICultureResourceProvider cultureProvider
                    ? cultureProvider.GetString(key, culture)
                    : string.Equals(culture.Name, CultureInfo.CurrentUICulture.Name, StringComparison.OrdinalIgnoreCase)
                        ? provider.GetString(key)
                        : key;

        return !string.IsNullOrEmpty(value) && value != key;
    }

    private void RecordMissingKey(string key, CultureInfo culture, IReadOnlyList<CultureInfo> fallbackChain, IReadOnlyList<IResourceProvider> providers)
    {
        Interlocked.Increment(ref _missingKeyCount);
        _missingKeys.AddOrUpdate(key, 1, (_, count) => count + 1);

        var handler = MissingKey;
        if (handler == null)
        {
            return;
        }

        var providerNames = providers.Select(provider => provider.AssemblyName).ToList().AsReadOnly();
        handler(this, new MissingResourceEventArgs(key, culture, fallbackChain, providerNames));
    }

    /// <summary>
    /// 确保所有资源提供者已加载并初始化
    /// </summary>
    /// <remarks>
    /// 此方法支持并发调用，使用双重检查锁定模式确保线程安全。
    /// 加载过程：
    /// 1. 首先添加默认资源提供者（最低优先级）
    /// 2. 然后添加发现的程序集资源提供者（较高优先级）
    /// </remarks>
    /// <example>
    /// <code>
    /// var manager = new ResourceManager();
    /// manager.EnsureProvidersLoaded(); // 确保提供者已加载
    /// </code>
    /// </example>
    public void EnsureProvidersLoaded()
    {
        if (_providersLoaded)
        {
            return;
        }

        lock (_loadLock)
        {
            if (_providersLoaded)
            {
                return;
            }

            LoadProviders();
            _providersLoaded = true;
        }
    }

    /// <summary>
    /// 加载所有资源提供者
    /// </summary>
    /// <remarks>
    /// 加载顺序很重要：
    /// 1. 默认资源提供者作为后备（最低优先级）
    /// 2. 程序集资源提供者优先级更高
    /// </remarks>
    private void LoadProviders()
    {
        try
        {
            // 2. 添加程序集资源提供者（优先级更高）
            var assemblyProviders = _assemblyProviders.Value;
            foreach (var provider in assemblyProviders)
            {
                lock (_providersLock)
                {
                    if (_providers.Exists(m => m.AssemblyName == provider.Value.AssemblyName))
                    {
                        continue;
                    }

                    _providers.Insert(0, provider.Value); // 插入到列表开头，优先级更高
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load providers: {ex.Message}");
        }
    }

    /// <summary>
    /// 发现并创建程序集资源提供者
    /// </summary>
    /// <returns>程序集资源提供者数组</returns>
    /// <remarks>
    /// 扫描当前应用程序域中已加载的所有程序集，
    /// 查找包含本地化资源的程序集，并创建相应的资源提供者。
    /// 只处理已加载到内存中的程序集，不会主动加载额外的程序集。
    /// </remarks>
    private static ConcurrentDictionary<string, AssemblyResourceProvider> DiscoverAssemblyProviders()
    {
        var providers = new ConcurrentDictionary<string, AssemblyResourceProvider>();
        try
        {
            // 获取当前应用程序域中已加载的所有 GameFrameX 程序集
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                                            .Where(assembly =>
                                                       assembly.FullName?.StartsWith("GameFrameX.") == true &&
                                                       !assembly.IsDynamic)
                                            .ToList();

            foreach (var assembly in loadedAssemblies)
            {
                try
                {
                    // 检查程序集是否包含本地化资源
                    var hasResources = assembly.GetManifestResourceNames().Any(name => name.Contains(".Localization.") && name.EndsWith(".resources"));
                    if (hasResources)
                    {
                        if (!providers.TryAdd(assembly.FullName, new AssemblyResourceProvider(assembly)))
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to add assembly provider for {assembly.FullName}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to check loaded assembly {assembly.FullName}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to discover assembly providers: {ex.Message}");
        }

        return providers;
    }

    /// <summary>
    /// 手动注册资源提供者
    /// </summary>
    /// <param name="provider">要注册的资源提供者</param>
    /// <exception cref="ArgumentNullException">当 provider 为 null 时抛出</exception>
    /// <remarks>
    /// 手动注册的提供者会被插入到提供者列表的开头，具有最高优先级。
    /// 此方法主要用于测试或自定义资源提供者的集成。
    /// </remarks>
    /// <example>
    /// <code>
    /// var manager = new ResourceManager();
    /// var customProvider = new MyCustomResourceProvider();
    /// manager.RegisterProvider(customProvider);
    /// // customProvider 现在具有最高优先级
    /// </code>
    /// </example>
    public void RegisterProvider(IResourceProvider provider)
    {
        if (provider == null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        EnsureProvidersLoaded();
        lock (_providersLock)
        {
            _providers.Insert(0, provider); // 插入到开头，最高优先级
        }
    }

    /// <summary>
    /// 获取所有已注册的资源提供者
    /// </summary>
    /// <returns>包含所有资源提供者的只读集合</returns>
    /// <remarks>
    /// 提供者按优先级排序，索引越小优先级越高。
    /// 返回的集合是只读的，不能直接修改。
    /// </remarks>
    /// <example>
    /// <code>
    /// var manager = new ResourceManager();
    /// manager.EnsureProvidersLoaded();
    /// var providers = manager.GetProviders();
    /// foreach (var provider in providers)
    /// {
    ///     Console.WriteLine($"Provider: {provider.GetType().Name}");
    /// }
    /// </code>
    /// </example>
    public IReadOnlyList<IResourceProvider> GetProviders()
    {
        EnsureProvidersLoaded();
        lock (_providersLock)
        {
            return _providers.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// 获取资源管理器的统计信息
    /// </summary>
    /// <returns>包含统计信息的对象</returns>
    /// <remarks>
    /// 可以用于监控和调试本地化系统的状态。
    /// </remarks>
    /// <example>
    /// <code>
    /// var manager = new ResourceManager();
    /// var stats = manager.GetStatistics();
    /// Console.WriteLine($"Providers loaded: {stats.ProvidersLoaded}");
    /// Console.WriteLine($"Assembly providers: {stats.AssemblyProviderCount}");
    /// </code>
    /// </example>
    public ResourceManagerStatistics GetStatistics()
    {
        EnsureProvidersLoaded();

        IResourceProvider[] providers;
        lock (_providersLock)
        {
            providers = _providers.ToArray();
        }

        var assemblyProviders = providers.OfType<AssemblyResourceProvider>().ToList();

        return new ResourceManagerStatistics
        {
            ProvidersLoaded = _providersLoaded,
            TotalProviderCount = providers.Length,
            DefaultProviderExists = assemblyProviders.Count > 0,
            AssemblyProviderCount = assemblyProviders.Count,
            AssemblyProviders = assemblyProviders.Select(p => p.GetStatistics()).ToList(),
            MissingKeyCount = Interlocked.Read(ref _missingKeyCount),
            MissingKeys = _missingKeys.ToDictionary(item => item.Key, item => item.Value),
            FormatFailureCount = Interlocked.Read(ref _formatFailureCount)
        };
    }

    /// <summary>
    /// 释放所有资源提供者占用的资源
    /// </summary>
    /// <remarks>
    /// 调用所有实现了 IDisposable 的资源提供者的 Dispose 方法。
    /// 调用此方法后，资源管理器将不再可用。
    /// </remarks>
    public void Dispose()
    {
        IResourceProvider[] providers;
        lock (_providersLock)
        {
            providers = _providers.ToArray();
            _providers.Clear();
        }

        foreach (var provider in providers)
        {
            if (provider is IDisposable disposable)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error disposing provider {provider.GetType().Name}: {ex.Message}");
                }
            }
        }
    }
}
