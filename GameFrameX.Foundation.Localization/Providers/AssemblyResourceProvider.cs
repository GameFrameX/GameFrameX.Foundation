// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using GameFrameX.Foundation.Localization.Core;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace GameFrameX.Foundation.Localization.Providers;

/// <summary>
/// 程序集资源提供者 - 支持懒加载的本地化资源提供者
/// </summary>
/// <remarks>
/// 该提供者从程序集的所有嵌入 .resources 文件中加载本地化字符串。
/// 支持懒加载机制，只在首次使用时扫描和加载程序集资源。
/// 支持多种资源文件命名约定，包括：
/// - Localization.Messages 目录下的资源文件
/// - 其他任何符合 .NET 标准的资源文件
/// 提供更大的灵活性，允许加载程序集中的所有本地化资源。
/// </remarks>
/// <example>
/// <code>
/// // 创建程序集资源提供者
/// var assembly = Assembly.GetExecutingAssembly();
/// var provider = new AssemblyResourceProvider(assembly);
///
/// // 获取本地化字符串（会触发懒加载）
/// var message = provider.GetString("MyApp.Exceptions.InvalidArgument");
/// </code>
/// </example>
public class AssemblyResourceProvider : ILazyResourceProvider, IDisposable
{
    /// <summary>
    /// 获取当前程序集的完整名称
    /// </summary>
    public string AssemblyName
    {
        get { return _assembly.FullName; }
    }

    private readonly Assembly _assembly;
    private readonly List<System.Resources.ResourceManager> _resourceManagers;
    private volatile bool _isInitialized;
    private readonly object _initLock;

    /// <summary>
    /// 获取一个值，指示资源提供者是否已完成初始化
    /// </summary>
    /// <value>
    /// 如果资源提供者已加载并准备好提供服务，则为 true；否则为 false。
    /// </value>
    public bool IsInitialized
    {
        get { return _isInitialized; }
    }

    /// <summary>
    /// 初始化 AssemblyResourceProvider 的新实例
    /// </summary>
    /// <param name="assembly">包含本地化资源的程序集</param>
    /// <exception cref="ArgumentNullException">当 assembly 为 null 时抛出</exception>
    /// <remarks>
    /// 构造函数只保存程序集引用，不会立即加载资源。
    /// 资源的扫描和加载将在首次调用 GetString 或 EnsureLoaded 时进行。
    /// </remarks>
    /// <example>
    /// <code>
    /// var assembly = Assembly.GetExecutingAssembly();
    /// var provider = new AssemblyResourceProvider(assembly);
    /// </code>
    /// </example>
    public AssemblyResourceProvider(Assembly assembly)
    {
        _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        _resourceManagers = new List<System.Resources.ResourceManager>();
        _initLock = new object();
    }

    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key">资源键，格式通常为 "模块名.类别.具体键名"</param>
    /// <returns>
    /// 如果找到对应的本地化字符串，返回该字符串；
    /// 如果未找到，返回传入的资源键
    /// </returns>
    /// <remarks>
    /// 此方法会自动触发资源加载（如果尚未加载）。
    /// 资源键的格式应符合约定：ModuleName.Category.SpecificKey
    /// 例如：Utility.Exceptions.TimestampOutOfRange
    ///
    /// 方法会按以下顺序查找资源：
    /// 1. 当前文化的资源
    /// 2. 非特定文化的资源（作为后备）
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new AssemblyResourceProvider(Assembly.GetExecutingAssembly());
    /// var message = provider.GetString("Utility.Exceptions.TimestampOutOfRange");
    /// // 返回：找到对应文化的本地化字符串，如果找不到则返回 "Utility.Exceptions.TimestampOutOfRange"
    /// </code>
    /// </example>
    public string GetString(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }

        EnsureLoaded();

        if (TryGetResourceManager(key, out var resourceManager))
        {
            try
            {
                var culture = CultureInfo.CurrentUICulture;
                var value = resourceManager.GetString(key, culture);


                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }

                // 尝试使用非特定文化作为后备
                if (!Equals(culture, CultureInfo.InvariantCulture))
                {
                    value = resourceManager.GetString(key, CultureInfo.InvariantCulture);

                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                }
            }
            catch (MissingManifestResourceException)
            {
                // 资源文件不存在，继续其他逻辑
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，使用键作为后备
                System.Diagnostics.Debug.WriteLine($"Failed to load resource '{key}': {ex.Message}");
            }
        }

        return key; // 返回键名作为后备
    }

    /// <summary>
    /// 确保资源已加载并初始化
    /// </summary>
    /// <remarks>
    /// 如果资源尚未加载，此方法将扫描程序集中的所有 .resources 文件并创建相应的 ResourceManager。
    /// 该方法支持并发调用，使用双重检查锁定模式确保线程安全。
    /// 扫描过程会查找所有以 .resources 结尾的资源文件，支持多种命名约定：
    /// - [AssemblyName].Localization.Messages.Resources.[Culture].resources (标准格式)
    /// - [AssemblyName].Localization.Messages.Resources.resources (默认资源)
    /// - [AssemblyName].[Path].Resources.[Culture].resources (自定义路径)
    /// - 其他任何标准的 .NET 资源文件命名约定
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new AssemblyResourceProvider(Assembly.GetExecutingAssembly());
    /// Console.WriteLine($"Initialized: {provider.IsInitialized}"); // false
    ///
    /// provider.EnsureLoaded();
    /// Console.WriteLine($"Initialized: {provider.IsInitialized}"); // true
    ///
    /// // 重复调用应该快速返回
    /// provider.EnsureLoaded();
    /// </code>
    /// </example>
    public void EnsureLoaded()
    {
        if (_isInitialized)
        {
            return;
        }

        lock (_initLock)
        {
            if (_isInitialized)
            {
                return;
            }

            try
            {
                LoadResources();
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，允许系统继续运行
                System.Diagnostics.Debug.WriteLine($"Failed to load resources for {_assembly.FullName}: {ex.Message}");
                _isInitialized = true; // 标记为已初始化，避免重复尝试
            }
        }
    }

    /// <summary>
    /// 加载程序集中的资源文件
    /// </summary>
    /// <remarks>
    /// 扫描程序集清单资源，查找所有的 .resources 文件。
    /// 支持标准化的 Resources.resx 命名约定：
    /// - [AssemblyName].Localization.Messages.Resources.[Culture].resources (标准格式)
    /// - [AssemblyName].Localization.Messages.Resources.resources (默认资源)
    /// - [AssemblyName].[CustomPath].Resources.[Culture].resources (自定义路径)
    /// - 其他任何符合 .NET 资源文件命名约定的文件
    ///
    /// 对于标准格式，系统会智能提取资源类别：
    /// - 优先使用 "Messages" 作为类别
    /// - 其次使用程序集名后的第一个非 "Localization" 部分
    /// - 最终使用 "Default" 作为后备类别
    /// </remarks>
    private void LoadResources()
    {
        try
        {
            var resourceNames = _assembly.GetManifestResourceNames();

            var list = resourceNames.Where(name => name.EndsWith(".resources")).ToList();

            foreach (var resourceName in list)
            {
                try
                {
                    var parts = resourceName.Split('.');

                    // 尝试从不同命名约定中提取类别和基础名称
                    // 策略1：标准Resources.resx命名约定
                    // 例如：MyAssembly.Localization.Messages.Resources.resources 或 MyAssembly.Localization.Messages.Resources.zh-CN.resources
                    if (parts.Length < 3)
                    {
                        continue;
                    }

                    // 寻找文化标识符（如 zh-CN, en-US 等）
                    var secondLastPart = parts[^2]; // 可能是文化名或"Resources"
                    string baseName;

                    if (secondLastPart.Equals("Resources", StringComparison.OrdinalIgnoreCase))
                    {
                        // 格式：程序集名.[路径].Resources.resources (默认资源)
                        // 例如：GameFrameX.Foundation.Utility.Localization.Messages.Resources.resources
                        var pathParts = parts.Take(parts.Length - 1).ToList(); // 移除resources
                        baseName = string.Join(".", pathParts);
                    }
                    else if (secondLastPart.Contains('-') && secondLastPart.Length >= 2) // 文化名 (zh-CN, en-US等)
                    {
                        // 格式：程序集名.[路径].Resources.文化名.resources
                        // 例如：GameFrameX.Foundation.Utility.Localization.Messages.Resources.zh-CN.resources
                        var pathParts = parts.Take(parts.Length - 2).ToList(); // 移除文化名和resources

                        // 确保倒数第二部分是Resources
                        if (pathParts.Count >= 1 && pathParts.Last().Equals("Resources", StringComparison.OrdinalIgnoreCase))
                        {
                            baseName = string.Join(".", pathParts);
                        }
                        else
                        {
                            // 如果格式不匹配，跳过这个资源
                            continue;
                        }
                    }
                    else
                    {
                        // 其他格式：程序集名.类别.resources (保持向后兼容)
                        baseName = string.Join(".", parts.Take(parts.Length - 1));
                    }

                    if (_resourceManagers.All(x => x.BaseName != baseName))
                    {
                        _resourceManagers.Add(new System.Resources.ResourceManager(baseName, _assembly));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to process resource '{resourceName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to scan assembly resources: {ex.Message}");
        }
    }

    /// <summary>
    /// 尝试获取指定键对应的资源管理器
    /// </summary>
    /// <param name="key">资源键</param>
    /// <param name="resourceManager">输出的资源管理器</param>
    /// <returns>如果找到对应的资源管理器，返回 true；否则返回 false</returns>
    /// <remarks>
    /// 资源键格式预期为：ModuleName.Category.SpecificKey
    /// 其中 ModuleName 对应程序集名，用于查找对应的资源管理器
    /// 例如：Utility.Exceptions.TimestampOutOfRange → 使用 "GameFrameX" 查找
    /// </remarks>
    private bool TryGetResourceManager(string key, out System.Resources.ResourceManager resourceManager)
    {
        // 直接遍历所有已加载的ResourceManager，查找包含该键的资源管理器
        foreach (var kvp in _resourceManagers)
        {
            var category = kvp.BaseName;
            resourceManager = kvp;

            try
            {
                // 尝试直接获取资源（让ResourceManager根据当前文化自动选择）
                var testValue = resourceManager.GetString(key, CultureInfo.CurrentUICulture);
                if (!string.IsNullOrEmpty(testValue) && testValue != key)
                {
                    // 找到了有效的本地化值
                    return true;
                }

                // 如果特定文化没有找到，尝试使用默认文化
                var defaultValue = resourceManager.GetString(key, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(defaultValue) && defaultValue != key)
                {
                    // 找到了默认值
                    return true;
                }
            }
            catch (Exception ex)
            {
                // 忽略错误，继续尝试下一个ResourceManager
                System.Diagnostics.Debug.WriteLine($"Failed to check resource in manager '{category}': {ex.Message}");
            }
        }

        resourceManager = null;
        return false;
    }

    /// <summary>
    /// 获取所有已加载的资源管理器类别
    /// </summary>
    /// <returns>包含所有类别名的集合</returns>
    /// <remarks>
    /// 可以用于调试，查看哪些资源类别已经加载。
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new AssemblyResourceProvider(Assembly.GetExecutingAssembly());
    /// provider.EnsureLoaded();
    /// var categories = provider.GetLoadedCategories();
    /// foreach (var category in categories)
    /// {
    ///     Console.WriteLine($"Loaded category: {category}");
    /// }
    /// </code>
    /// </example>
    public IReadOnlyCollection<string> GetLoadedCategories()
    {
        return _resourceManagers.Select(x => x.BaseName).ToList().AsReadOnly();
    }

    /// <summary>
    /// 获取资源提供者的统计信息
    /// </summary>
    /// <returns>包含统计信息的对象</returns>
    /// <remarks>
    /// 可以用于监控和调试本地化资源的使用情况。
    /// </remarks>
    public AssemblyResourceProviderStatistics GetStatistics()
    {
        return new AssemblyResourceProviderStatistics
        {
            IsInitialized = _isInitialized,
            AssemblyName = _assembly.GetName().Name,
            LoadedCategoriesCount = _resourceManagers.Count,
            LoadedCategories = _resourceManagers.Select(x => x.BaseName).ToList()
        };
    }

    /// <summary>
    /// 释放资源管理器占用的资源
    /// </summary>
    /// <remarks>
    /// 释放所有 ResourceManager 的资源，清理内部缓存。
    /// 调用此方法后，提供者将不再可用。
    /// </remarks>
    public void Dispose()
    {
        try
        {
            foreach (var manager in _resourceManagers)
            {
                manager?.ReleaseAllResources();
            }

            _resourceManagers.Clear();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error during disposal: {ex.Message}");
        }
    }
}

/// <summary>
/// 程序集资源提供者统计信息
/// </summary>
/// <remarks>
/// 包含程序集资源提供者的运行时统计信息，
/// 用于监控和调试本地化资源的使用情况。
/// </remarks>
public class AssemblyResourceProviderStatistics
{
    /// <summary>
    /// 获取或设置资源提供者是否已初始化
    /// </summary>
    public bool IsInitialized { get; set; }

    /// <summary>
    /// 获取或设置程序集名称
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// 获取或设置已加载的资源类别数量
    /// </summary>
    public int LoadedCategoriesCount { get; set; }

    /// <summary>
    /// 获取或设置已加载的资源类别列表
    /// </summary>
    public List<string> LoadedCategories { get; set; } = new List<string>();
}