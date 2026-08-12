using GameFrameX.Foundation.Localization.Core;
using System.Globalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

/// <summary>
/// ResourceManager 的 MissingKey 事件与 <see cref="ResourceManagerStatistics"/> 缺失键统计边界测试。
/// 补充覆盖 LocalizationHighPriorityFeatureTests 已有用例之外的：
/// <list type="bullet">
///   <item><see cref="MissingResourceEventArgs.FallbackChain"/> / <see cref="MissingResourceEventArgs.ProviderNames"/> 字段填充。</item>
///   <item>同一 key 多次查询时 MissingKeys[key] 的累加语义。</item>
///   <item>无订阅者时统计仍应递增（事件触发与统计解耦）。</item>
///   <item>已知 key 不应触发事件。</item>
///   <item><see cref="LocalizationService.MissingKey"/> 静态事件正确路由到实例。</item>
/// </list>
/// </summary>
public class ResourceManagerMissingKeyTests : IDisposable
{
    private readonly ResourceManager _resourceManager;

    public ResourceManagerMissingKeyTests()
    {
        _resourceManager = new ResourceManager();
    }

    public void Dispose()
    {
        _resourceManager?.Dispose();
    }

    [Fact]
    public void MissingKey_Event_FallbackChain_ContainsQueriedCultureAndParents()
    {
        // Arrange
        MissingResourceEventArgs captured = null;
        _resourceManager.MissingKey += (_, args) => captured = args;
        var requestedCulture = new CultureInfo("zh-Hans-CN");

        // Act
        _resourceManager.GetString("Missing.FallbackChain.Key", requestedCulture);

        // Assert - fallback chain 应至少包含请求的 culture 自身
        Assert.NotNull(captured);
        Assert.NotEmpty(captured.FallbackChain);
        Assert.Contains(captured.FallbackChain, c => c.Name == "zh-Hans-CN");
        // 默认 CultureFallbackEnabled=true，应包含 parent chain 直至 InvariantCulture
        Assert.Contains(captured.FallbackChain, c => c.Name == string.Empty);
    }

    [Fact]
    public void MissingKey_Event_ProviderNames_ContainsAllRegisteredProviderAssemblies()
    {
        // Arrange - 注册两个 provider 以验证 ProviderNames 列表
        _resourceManager.RegisterProvider(new NamedProvider("ProviderA"));
        _resourceManager.RegisterProvider(new NamedProvider("ProviderB"));
        MissingResourceEventArgs captured = null;
        _resourceManager.MissingKey += (_, args) => captured = args;

        // Act
        _resourceManager.GetString("Missing.Providers.Key", CultureInfo.InvariantCulture);

        // Assert - ProviderNames 是所有参与查询的 provider 名称（即便未命中）
        Assert.NotNull(captured);
        Assert.NotNull(captured.ProviderNames);
        Assert.Contains("ProviderA", captured.ProviderNames);
        Assert.Contains("ProviderB", captured.ProviderNames);
    }

    [Fact]
    public void MissingKey_Count_IncrementsEvenWithoutSubscriber()
    {
        // Arrange - 不订阅事件
        var before = _resourceManager.GetStatistics().MissingKeyCount;

        // Act
        _resourceManager.GetString("Missing.NoSubscriber.Key", CultureInfo.InvariantCulture);

        // Assert - 统计字段与事件解耦，无订阅者也应递增
        var after = _resourceManager.GetStatistics().MissingKeyCount;
        Assert.True(after > before, $"MissingKeyCount 应在无订阅者时递增, before={before}, after={after}");
    }

    [Fact]
    public void MissingKey_PerKeyCount_AccumulatesAcrossRepeatedLookups()
    {
        // Arrange - 同一 key 查询 3 次
        const string key = "Missing.Repeated.Key";
        _resourceManager.GetString(key, CultureInfo.InvariantCulture);
        _resourceManager.GetString(key, CultureInfo.InvariantCulture);

        // Act
        _resourceManager.GetString(key, CultureInfo.InvariantCulture);

        // Assert - MissingKeys[key] 应累加（>=3，考虑并行测试污染容差）
        var stats = _resourceManager.GetStatistics();
        Assert.True(stats.MissingKeys.TryGetValue(key, out var count), $"MissingKeys 应包含 key={key}");
        Assert.True(count >= 3, $"同 key 多次查询应累加，实际 count={count}");
    }

    [Fact]
    public void MissingKey_Event_FiresForEachLookup()
    {
        // Arrange
        var fireCount = 0;
        _resourceManager.MissingKey += (_, __) => Interlocked.Increment(ref fireCount);

        // Act
        _resourceManager.GetString("Missing.MultiFire.A", CultureInfo.InvariantCulture);
        _resourceManager.GetString("Missing.MultiFire.B", CultureInfo.InvariantCulture);
        _resourceManager.GetString("Missing.MultiFire.A", CultureInfo.InvariantCulture);

        // Assert - 每次未命中都应触发一次
        Assert.True(fireCount >= 3, $"事件应触发至少 3 次，实际 fireCount={fireCount}");
    }

    [Fact]
    public void MissingKey_Event_DoesNotFire_ForKnownKey()
    {
        // Arrange - 注册一个对 Known.Key 返回明确值的 provider
        _resourceManager.RegisterProvider(new KnownValueProvider("Known.Key", "Known Value"));
        var fireCount = 0;
        _resourceManager.MissingKey += (_, __) => Interlocked.Increment(ref fireCount);

        // Act
        var result = _resourceManager.GetString("Known.Key", CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal("Known Value", result);
        Assert.Equal(0, fireCount);
    }

    [Fact]
    public void MissingKey_Event_Culture_MatchesExplicitCultureArgument()
    {
        // Arrange
        MissingResourceEventArgs captured = null;
        _resourceManager.MissingKey += (_, args) => captured = args;
        var requested = new CultureInfo("fr-FR");

        // Act
        _resourceManager.GetString("Missing.CultureMatch.Key", requested);

        // Assert - 注意：事件 Culture 是 GetString 入参 culture，而非 fallback 链中的某个
        Assert.NotNull(captured);
        Assert.Equal("fr-FR", captured.Culture.Name);
    }

    [Fact]
    public void LocalizationService_StaticMissingKey_EventRoutesToInstance()
    {
        // Arrange - 静态事件订阅（Lazy ResourceManager 实例路由）
        // 注意：LocalizationService 是全局单例，测试之间共享状态；故只验证"订阅后能收到事件"语义
        MissingResourceEventArgs captured = null;
        EventHandler<MissingResourceEventArgs> handler = (_, args) => captured = args;
        LocalizationService.MissingKey += handler;
        try
        {
            // Act - 用一个极不可能存在的 key
            var uniqueKey = "Missing.Static." + Guid.NewGuid().ToString("N");
            LocalizationService.GetString(uniqueKey);

            // Assert
            Assert.NotNull(captured);
            Assert.Equal(uniqueKey, captured.Key);
        }
        finally
        {
            LocalizationService.MissingKey -= handler;
        }
    }

    /// <summary>
    /// 测试用 provider：AssemblyName 可自定义，对任何 key 都返回 key 本身（模拟未命中）。
    /// </summary>
    private sealed class NamedProvider : ICultureResourceProvider
    {
        private readonly string _assemblyName;

        public NamedProvider(string assemblyName)
        {
            _assemblyName = assemblyName;
        }

        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        public string AssemblyName => _assemblyName;

        public string GetString(string key)
        {
            return key;
        }

        public string GetString(string key, CultureInfo culture)
        {
            return key;
        }
    }

    /// <summary>
    /// 测试用 provider：对指定 key 返回固定 value，其他 key 返回 key 本身。
    /// </summary>
    private sealed class KnownValueProvider : ICultureResourceProvider
    {
        private readonly string _key;
        private readonly string _value;

        public KnownValueProvider(string key, string value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        public string AssemblyName => nameof(KnownValueProvider);

        public string GetString(string key)
        {
            return GetString(key, CultureInfo.CurrentUICulture);
        }

        public string GetString(string key, CultureInfo culture)
        {
            return key == _key ? _value : key;
        }
    }
}
