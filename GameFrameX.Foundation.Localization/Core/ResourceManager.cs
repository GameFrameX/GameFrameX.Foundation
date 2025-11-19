// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using GameFrameX.Foundation.Localization.Providers;
using System.Reflection;
using System.Collections.Generic;

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
    private readonly Lazy<AssemblyResourceProvider[]> _assemblyProviders;
    private volatile bool _providersLoaded;
    private readonly object _loadLock;

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
        _assemblyProviders = new Lazy<AssemblyResourceProvider[]>(DiscoverAssemblyProviders);
        _loadLock = new object();
    }

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
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }

        EnsureProvidersLoaded();

        foreach (var provider in _providers)
        {
            try
            {
                var value = provider.GetString(key);
                if (value != key) // 找到了有效的本地化值
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

        return key; // 所有提供者都没有找到，返回键名
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
                _providers.Insert(0, provider); // 插入到列表开头，优先级更高
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
    private static AssemblyResourceProvider[] DiscoverAssemblyProviders()
    {
        try
        {
            var providers = new List<AssemblyResourceProvider>();

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
                    var hasResources = assembly.GetManifestResourceNames()
                                               .Any(name => name.Contains(".Localization.") && name.EndsWith(".resources"));

                    if (hasResources)
                    {
                        providers.Add(new AssemblyResourceProvider(assembly));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to check loaded assembly {assembly.FullName}: {ex.Message}");
                }
            }

            return providers.ToArray();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to discover assembly providers: {ex.Message}");
            return Array.Empty<AssemblyResourceProvider>();
        }
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
        _providers.Insert(0, provider); // 插入到开头，最高优先级
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
        return _providers.ToList().AsReadOnly();
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

        var assemblyProviders = _providers.OfType<AssemblyResourceProvider>().ToList();

        return new ResourceManagerStatistics
        {
            ProvidersLoaded = _providersLoaded,
            TotalProviderCount = _providers.Count,
            DefaultProviderExists = _providers.Any(p => p is AssemblyResourceProvider),
            AssemblyProviderCount = assemblyProviders.Count,
            AssemblyProviders = assemblyProviders.Select(p => p.GetStatistics()).ToList()
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
        foreach (var provider in _providers)
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

        _providers.Clear();
    }
}