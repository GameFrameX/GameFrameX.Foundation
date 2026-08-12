using GameFrameX.Foundation.Localization.Core;
using System.Globalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

/// <summary>
/// ResourceManager 的 Culture Fallback 链与 Unicode/Emoji 往返边界测试。
/// 补充覆盖 LocalizationHighPriorityFeatureTests 之外的：
/// <list type="bullet">
///   <item><see cref="ResourceManager.CultureFallbackEnabled"/> = false 时 fallback 链仅含当前 culture。</item>
///   <item>null culture 入参回退到 <see cref="CultureInfo.CurrentUICulture"/>。</item>
///   <item>自定义 <see cref="ResourceManager.DefaultCulture"/> 出现在链尾。</item>
///   <item>Unicode / Emoji 的 key 与 value 端到端往返。</item>
///   <item>运行时切换 CultureFallbackEnabled 立即生效。</item>
/// </list>
/// </summary>
public class ResourceManagerCultureFallbackTests : IDisposable
{
    private readonly ResourceManager _resourceManager;

    public ResourceManagerCultureFallbackTests()
    {
        _resourceManager = new ResourceManager();
    }

    public void Dispose()
    {
        _resourceManager?.Dispose();
    }

    [Fact]
    public void GetCultureFallbackChain_Disabled_ReturnsSingleElementChain()
    {
        // Arrange
        _resourceManager.CultureFallbackEnabled = false;
        var requested = new CultureInfo("zh-Hans-CN");

        // Act
        var chain = _resourceManager.GetCultureFallbackChain(requested);

        // Assert - 禁用后链路只含请求的 culture 自身，不含 parent、不含 InvariantCulture
        Assert.Single(chain);
        Assert.Equal("zh-Hans-CN", chain[0].Name);
    }

    [Fact]
    public void GetCultureFallbackChain_Disabled_DoesNotIncludeDefaultOrInvariant()
    {
        // Arrange - 即便 DefaultCulture 设了非默认值，禁用 fallback 时也不应出现
        _resourceManager.CultureFallbackEnabled = false;
        _resourceManager.DefaultCulture = new CultureInfo("en-US");
        var requested = new CultureInfo("fr-FR");

        // Act
        var chain = _resourceManager.GetCultureFallbackChain(requested);

        // Assert
        Assert.Single(chain);
        Assert.DoesNotContain(chain, c => c.Name == "en-US");
        Assert.DoesNotContain(chain, c => c.Name == string.Empty);
    }

    [Fact]
    public void GetCultureFallbackChain_Enabled_IncludesParentChainAndInvariant()
    {
        // Arrange - 默认 CultureFallbackEnabled=true
        var requested = new CultureInfo("zh-Hans-CN");

        // Act
        var chain = _resourceManager.GetCultureFallbackChain(requested);

        // Assert - 链路：zh-Hans-CN → zh-Hans → zh → "" (Invariant)
        var names = chain.Select(c => c.Name).ToArray();
        Assert.Equal(new[] { "zh-Hans-CN", "zh-Hans", "zh", string.Empty }, names);
    }

    [Fact]
    public void GetCultureFallbackChain_Enabled_AppendsCustomDefaultCultureIfNotInChain()
    {
        // Arrange - 自定义 DefaultCulture 为 en-US，请求的 culture 链中不含 en-US
        _resourceManager.CultureFallbackEnabled = true;
        _resourceManager.DefaultCulture = new CultureInfo("en-US");
        var requested = new CultureInfo("zh-Hans-CN");

        // Act
        var chain = _resourceManager.GetCultureFallbackChain(requested);

        // Assert - en-US 应被附加（去重后），InvariantCulture 仍在链尾
        Assert.Contains(chain, c => c.Name == "en-US");
        Assert.Equal("en-US", chain[chain.Count - 1].Name);
    }

    [Fact]
    public void GetCultureFallbackChain_NullCulture_FallsBackToCurrentUICulture()
    {
        // Arrange - 固定 CurrentUICulture 以便断言
        var original = CultureInfo.CurrentUICulture;
        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo("de-DE");

            // Act
            var chain = _resourceManager.GetCultureFallbackChain(null);

            // Assert - 链首应是 CurrentUICulture
            Assert.NotEmpty(chain);
            Assert.Equal("de-DE", chain[0].Name);
        }
        finally
        {
            CultureInfo.CurrentUICulture = original;
        }
    }

    [Fact]
    public void GetCultureFallbackChain_InvariantCultureRequest_ReturnsInvariantOnly()
    {
        // Arrange
        // Act
        var chain = _resourceManager.GetCultureFallbackChain(CultureInfo.InvariantCulture);

        // Assert - Invariant 的 Parent 仍是 Invariant，链路去重后只剩一个
        Assert.Single(chain);
        Assert.Equal(string.Empty, chain[0].Name);
    }

    [Fact]
    public void CultureFallbackEnabled_ToggledAtRuntime_ChangesSubsequentChainShape()
    {
        // Arrange
        var requested = new CultureInfo("zh-Hans-CN");

        // Act - 先启用
        _resourceManager.CultureFallbackEnabled = true;
        var enabledChain = _resourceManager.GetCultureFallbackChain(requested);

        // 再禁用
        _resourceManager.CultureFallbackEnabled = false;
        var disabledChain = _resourceManager.GetCultureFallbackChain(requested);

        // Assert - 切换立即生效
        Assert.True(enabledChain.Count > 1);
        Assert.Single(disabledChain);
    }

    [Fact]
    public void GetString_UnicodeKey_RoundTripsWhenMissing()
    {
        // Arrange - 未注册的 Unicode key（中文、西里尔字母混合）
        const string unicodeKey = "错误.Ключ.缺失";

        // Act
        var result = _resourceManager.GetString(unicodeKey, CultureInfo.InvariantCulture);

        // Assert - 未命中时原样返回 key（不损坏 Unicode 字符）
        Assert.Equal(unicodeKey, result);
    }

    [Fact]
    public void GetString_UnicodeValue_ReturnedByProvider()
    {
        // Arrange - provider 注册 Unicode 值
        const string key = "Greeting.中文";
        const string value = "你好，世界！Привет！";
        _resourceManager.RegisterProvider(new FixedValueProvider(key, value));

        // Act
        var result = _resourceManager.GetString(key, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(value, result);
    }

    [Fact]
    public void GetString_EmojiKeyAndValue_RoundTrip()
    {
        // Arrange - Emoji 作为 key 和 value（4 字节 UTF-16 surrogate pair）
        const string emojiKey = "Game.🎯.成就";
        const string emojiValue = "🎉 恭喜达成 🏆";
        _resourceManager.RegisterProvider(new FixedValueProvider(emojiKey, emojiValue));

        // Act
        var result = _resourceManager.GetString(emojiKey, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(emojiValue, result);
    }

    [Fact]
    public void GetString_CultureFallback_FallsBackToParentCultureValue()
    {
        // Arrange - 只为 parent culture "zh" 注册值，请求 "zh-Hans-CN" 应回退命中
        _resourceManager.RegisterProvider(new CultureScopedProvider("zh", "你好"));
        var requested = new CultureInfo("zh-Hans-CN");

        // Act
        var result = _resourceManager.GetString("Any.Key", requested);

        // Assert - fallback 链依次查询，zh-Hans-CN 未命中 → zh 命中
        Assert.Equal("你好", result);
    }

    [Fact]
    public void GetString_CultureFallbackDisabled_DoesNotHitParentCultureValue()
    {
        // Arrange - 只为 parent culture "zh" 注册值，禁用 fallback
        _resourceManager.RegisterProvider(new CultureScopedProvider("zh", "你好"));
        _resourceManager.CultureFallbackEnabled = false;
        var requested = new CultureInfo("zh-Hans-CN");

        // Act
        var result = _resourceManager.GetString("Any.Key", requested);

        // Assert - 禁用 fallback 后只查 zh-Hans-CN，未命中则返回 key
        Assert.Equal("Any.Key", result);
    }

    /// <summary>
    /// 测试用 provider：对指定 key 返回固定 value，其他 key 返回 key 本身。忽略 culture。
    /// </summary>
    private sealed class FixedValueProvider : ICultureResourceProvider
    {
        private readonly string _key;
        private readonly string _value;

        public FixedValueProvider(string key, string value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        public string AssemblyName => nameof(FixedValueProvider);

        public string GetString(string key)
        {
            return GetString(key, CultureInfo.CurrentUICulture);
        }

        public string GetString(string key, CultureInfo culture)
        {
            return key == _key ? _value : key;
        }
    }

    /// <summary>
    /// 测试用 provider：对任意 key 在指定 culture 下返回固定 value，其他 culture 返回 key。
    /// </summary>
    private sealed class CultureScopedProvider : ICultureResourceProvider
    {
        private readonly string _cultureName;
        private readonly string _value;

        public CultureScopedProvider(string cultureName, string value)
        {
            _cultureName = cultureName;
            _value = value;
        }

        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        public string AssemblyName => nameof(CultureScopedProvider);

        public string GetString(string key)
        {
            return GetString(key, CultureInfo.CurrentUICulture);
        }

        public string GetString(string key, CultureInfo culture)
        {
            return culture != null && culture.Name == _cultureName ? _value : key;
        }
    }
}
