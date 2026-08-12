using GameFrameX.Foundation.Localization.Core;
using System.Globalization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization;

/// <summary>
/// ResourceManager.FormatString 与 <see cref="LocalizationFormatErrorBehavior"/> 三档行为的边界测试。
/// 重点覆盖：
/// <list type="bullet">
///   <item>args=null / 空数组时的短路分支（直接返回模板）。</item>
///   <item>ReturnTemplate / ReturnKey / Throw 三档对同一错误模板的不同处理。</item>
///   <item>FormatException 路径下 <see cref="ResourceManagerStatistics.FormatFailureCount"/> 的递增语义（含 Throw 档）。</item>
///   <item>正常格式化路径不应触发统计递增。</item>
/// </list>
/// </summary>
public class ResourceManagerFormatErrorTests : IDisposable
{
    private readonly ResourceManager _resourceManager;

    public ResourceManagerFormatErrorTests()
    {
        _resourceManager = new ResourceManager();
    }

    public void Dispose()
    {
        _resourceManager?.Dispose();
    }

    /// <summary>
    /// 辅助：注册一个对指定 key 返回固定模板的 provider（实现 ICultureResourceProvider 以跳过 culture 过滤）。
    /// </summary>
    private void RegisterTemplate(string key, string template)
    {
        _resourceManager.RegisterProvider(new TemplateProvider(key, template));
    }

    [Fact]
    public void FormatErrorBehavior_DefaultValue_IsReturnTemplate()
    {
        // Assert - 默认值由 ResourceManager 构造时设定
        Assert.Equal(LocalizationFormatErrorBehavior.ReturnTemplate, _resourceManager.FormatErrorBehavior);
    }

    [Fact]
    public void FormatString_WithNullArgs_ShouldShortCircuitAndReturnTemplate()
    {
        // Arrange - 模板故意包含会触发 FormatException 的字符，证明短路分支未进入 try
        const string key = "Format.NullArg";
        const string template = "Hello {1";
        RegisterTemplate(key, template);

        // Act - 显式传 null 以命中 args==null 短路
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, null);

        // Assert
        Assert.Equal(template, result);
        Assert.Equal(0, _resourceManager.GetStatistics().FormatFailureCount);
    }

    [Fact]
    public void FormatString_WithEmptyArgs_ShouldShortCircuitAndReturnTemplate()
    {
        // Arrange
        const string key = "Format.EmptyArg";
        const string template = "Plain Template";
        RegisterTemplate(key, template);

        // Act - 不传任何 args，编译器生成 Array.Empty<object>()
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture);

        // Assert
        Assert.Equal(template, result);
    }

    [Fact]
    public void FormatString_WithExplicitCultureNullArgs_ShouldShortCircuitWithoutFormatting()
    {
        // Arrange - 三参数重载 (key, culture, args)，显式传 null args
        const string key = "Format.CultureNullArg";
        const string template = "Template {0";
        RegisterTemplate(key, template);

        // Act
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, null);

        // Assert
        Assert.Equal(template, result);
    }

    [Fact]
    public void FormatString_ReturnTemplate_ShouldReturnOriginalTemplate()
    {
        // Arrange
        const string key = "Format.ReturnTemplate";
        const string template = "Hello {1"; // 未闭合 brace 触发 FormatException
        RegisterTemplate(key, template);
        _resourceManager.FormatErrorBehavior = LocalizationFormatErrorBehavior.ReturnTemplate;

        // Act
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, "Arg0");

        // Assert
        Assert.Equal(template, result);
    }

    [Fact]
    public void FormatString_ReturnKey_ShouldReturnKey()
    {
        // Arrange
        const string key = "Format.ReturnKey";
        const string template = "Hello {1";
        RegisterTemplate(key, template);
        _resourceManager.FormatErrorBehavior = LocalizationFormatErrorBehavior.ReturnKey;

        // Act
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, "Arg0");

        // Assert
        Assert.Equal(key, result);
    }

    [Fact]
    public void FormatString_Throw_ShouldRethrowFormatException()
    {
        // Arrange
        const string key = "Format.Throw";
        const string template = "Hello {1";
        RegisterTemplate(key, template);
        _resourceManager.FormatErrorBehavior = LocalizationFormatErrorBehavior.Throw;

        // Act & Assert - 直接重新抛出原始 FormatException（非包装异常）
        var ex = Assert.Throws<FormatException>(() => _resourceManager.FormatString(key, CultureInfo.InvariantCulture, "Arg0"));
        Assert.NotNull(ex);
    }

    [Fact]
    public void FormatString_OutOfBoundsPlaceholder_ShouldTriggerErrorPath()
    {
        // Arrange - 模板要求 index=1 但只传一个 arg，string.Format 会抛 FormatException
        const string key = "Format.OutOfBounds";
        const string template = "Hello {1}";
        RegisterTemplate(key, template);
        _resourceManager.FormatErrorBehavior = LocalizationFormatErrorBehavior.ReturnKey;

        // Act
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, "OnlyArg0");

        // Assert
        Assert.Equal(key, result);
    }

    [Theory]
    [InlineData(LocalizationFormatErrorBehavior.ReturnTemplate)]
    [InlineData(LocalizationFormatErrorBehavior.ReturnKey)]
    public void FormatString_NonThrowBehaviors_ShouldIncrementFormatFailureCount(LocalizationFormatErrorBehavior behavior)
    {
        // Arrange - 每次测试用独立 ResourceManager 避免跨测试统计污染
        const string key = "Format.NonThrow.Count";
        const string template = "Hello {1";
        using (var manager = new ResourceManager())
        {
            manager.RegisterProvider(new TemplateProvider(key, template));
            manager.FormatErrorBehavior = behavior;
            var before = manager.GetStatistics().FormatFailureCount;

            // Act
            manager.FormatString(key, CultureInfo.InvariantCulture, "Arg0");

            // Assert
            var after = manager.GetStatistics().FormatFailureCount;
            Assert.True(after > before, $"FormatFailureCount 应递增，behavior={behavior}, before={before}, after={after}");
        }
    }

    [Fact]
    public void FormatString_Throw_ShouldIncrementFormatFailureCountBeforeRethrowing()
    {
        // Arrange - Throw 档在抛出前应先递增计数
        const string key = "Format.Throw.Count";
        const string template = "Hello {1";
        using (var manager = new ResourceManager())
        {
            manager.RegisterProvider(new TemplateProvider(key, template));
            manager.FormatErrorBehavior = LocalizationFormatErrorBehavior.Throw;
            var before = manager.GetStatistics().FormatFailureCount;

            // Act
            try
            {
                manager.FormatString(key, CultureInfo.InvariantCulture, "Arg0");
            }
            catch (FormatException)
            {
                // 预期抛出，吞掉以便后续断言
            }

            // Assert - 即使抛出，计数也应当 +1
            var after = manager.GetStatistics().FormatFailureCount;
            Assert.True(after > before, $"Throw 档也应在抛出前递增 FormatFailureCount, before={before}, after={after}");
        }
    }

    [Fact]
    public void FormatString_ValidTemplate_ShouldNotIncrementFailureCount()
    {
        // Arrange
        const string key = "Format.Valid";
        const string template = "Hello {0}";
        RegisterTemplate(key, template);
        var before = _resourceManager.GetStatistics().FormatFailureCount;

        // Act
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, "World");

        // Assert
        Assert.Equal("Hello World", result);
        Assert.Equal(before, _resourceManager.GetStatistics().FormatFailureCount);
    }

    [Fact]
    public void FormatString_TwoArgsOverload_ShouldRouteToCultureOverload()
    {
        // Arrange - 验证 FormatString(key, args) 重载内部转发到 (key, CurrentUICulture, args)
        const string key = "Format.TwoArgsOverload";
        const string template = "Hello {0}";
        RegisterTemplate(key, template);

        // Act
        var result = _resourceManager.FormatString(key, "World");

        // Assert
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void FormatString_UnknownKey_FallsBackToKeyAsTemplate()
    {
        // Arrange - key 未注册时 GetString 返回 key 本身，FormatString 拿到的 template=key
        const string key = "Format.Unknown.Key.No.Provider";
        _resourceManager.FormatErrorBehavior = LocalizationFormatErrorBehavior.ReturnKey;

        // Act - key 字符串本身无占位符，会进入 string.Format 但不抛
        var result = _resourceManager.FormatString(key, CultureInfo.InvariantCulture, "Arg0");

        // Assert - key 不含占位符，string.Format 直接返回 key 原文
        Assert.Equal(key, result);
    }

    /// <summary>
    /// 测试用模板 provider：对指定 key 返回固定模板字符串（忽略 culture 差异）。
    /// </summary>
    private sealed class TemplateProvider : ICultureResourceProvider
    {
        private readonly string _key;
        private readonly string _template;

        public TemplateProvider(string key, string template)
        {
            _key = key;
            _template = template;
        }

        /// <summary>
        /// 获取资源提供者的名称
        /// </summary>
        public string AssemblyName => nameof(TemplateProvider);

        public string GetString(string key)
        {
            return GetString(key, CultureInfo.CurrentUICulture);
        }

        public string GetString(string key, CultureInfo culture)
        {
            return key == _key ? _template : key;
        }
    }
}
