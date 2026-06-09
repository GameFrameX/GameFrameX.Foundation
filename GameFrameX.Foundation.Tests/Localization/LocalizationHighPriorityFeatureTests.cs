using GameFrameX.Foundation.Localization.Core;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

public class LocalizationHighPriorityFeatureTests
{
    [Fact]
    public void GetCultureFallbackChain_WithSpecificCulture_ShouldReturnParentChainThenDefault()
    {
        var manager = new ResourceManager();

        var chain = manager.GetCultureFallbackChain(new CultureInfo("zh-Hans-CN"));

        Assert.Equal(new[] { "zh-Hans-CN", "zh-Hans", "zh", "" }, chain.Select(culture => culture.Name));
    }

    [Fact]
    public void GetString_WithExplicitCulture_ShouldUseFallbackChainWithoutCurrentUICulture()
    {
        var manager = new ResourceManager();
        manager.RegisterProvider(new CultureResourceProvider(("Greeting", "zh-Hans", "你好")));
        var originalCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            var result = manager.GetString("Greeting", new CultureInfo("zh-Hans-CN"));

            Assert.Equal("你好", result);
            Assert.Equal("en-US", CultureInfo.CurrentUICulture.Name);
        }
        finally
        {
            CultureInfo.CurrentUICulture = originalCulture;
        }
    }

    [Fact]
    public void GetString_WithMissingKey_ShouldRaiseEventAndUpdateStatistics()
    {
        var manager = new ResourceManager();
        MissingResourceEventArgs eventArgs = null;
        manager.MissingKey += (_, args) => eventArgs = args;

        var result = manager.GetString("Missing.Key", new CultureInfo("fr-CA"));
        var statistics = manager.GetStatistics();

        Assert.Equal("Missing.Key", result);
        Assert.NotNull(eventArgs);
        Assert.Equal("Missing.Key", eventArgs.Key);
        Assert.Equal("fr-CA", eventArgs.Culture.Name);
        Assert.True(statistics.MissingKeyCount >= 1);
        Assert.Contains(statistics.MissingKeys, item => item.Key == "Missing.Key");
    }

    [Fact]
    public void FormatString_WithArguments_ShouldUseUnifiedFormattingApi()
    {
        var manager = new ResourceManager();
        manager.RegisterProvider(new CultureResourceProvider(("Greeting", "", "Hello {0}")));

        var result = manager.FormatString("Greeting", CultureInfo.InvariantCulture, "GameFrameX");

        Assert.Equal("Hello GameFrameX", result);
    }

    [Fact]
    public void FormatString_WithInvalidTemplate_ShouldReturnTemplateAndTrackFailure()
    {
        var manager = new ResourceManager();
        manager.RegisterProvider(new CultureResourceProvider(("Bad.Template", "", "Hello {0")));

        var result = manager.FormatString("Bad.Template", CultureInfo.InvariantCulture, "GameFrameX");
        var statistics = manager.GetStatistics();

        Assert.Equal("Hello {0", result);
        Assert.True(statistics.FormatFailureCount >= 1);
    }

    [Fact]
    public void GameFrameXStringLocalizer_ShouldExposeLocalizedStringMetadata()
    {
        var manager = new ResourceManager();
        manager.RegisterProvider(new CultureResourceProvider(("Known.Key", "", "Known value")));
        IStringLocalizer localizer = new GameFrameXStringLocalizer(manager);

        var found = localizer["Known.Key"];
        var missing = localizer["Missing.Key"];
        var formatted = localizer["Known.Key", "ignored"];

        Assert.Equal("Known value", found.Value);
        Assert.False(found.ResourceNotFound);
        Assert.Equal("Missing.Key", missing.Value);
        Assert.True(missing.ResourceNotFound);
        Assert.Equal("Known value", formatted.Value);
    }

    private sealed class CultureResourceProvider : ICultureResourceProvider
    {
        private readonly Dictionary<(string Key, string Culture), string> _values;

        public CultureResourceProvider(params (string Key, string Culture, string Value)[] values)
        {
            _values = values.ToDictionary(item => (item.Key, item.Culture), item => item.Value);
        }

        public string AssemblyName => nameof(CultureResourceProvider);

        public string GetString(string key)
        {
            return GetString(key, CultureInfo.CurrentUICulture);
        }

        public string GetString(string key, CultureInfo culture)
        {
            return _values.TryGetValue((key, culture?.Name ?? string.Empty), out var value) ? value : key;
        }
    }
}
