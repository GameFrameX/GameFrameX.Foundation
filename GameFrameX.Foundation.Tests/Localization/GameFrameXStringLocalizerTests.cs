using System;
using System.Collections.Generic;
using System.Globalization;
using GameFrameX.Foundation.Localization.Core;
using Microsoft.Extensions.Localization;
using Xunit;

namespace GameFrameX.Foundation.Tests.Localization
{
    /// <summary>
    /// GameFrameXStringLocalizer / GameFrameXStringLocalizer&lt;T&gt; 单元测试。
    /// 覆盖：构造函数校验、索引器（无参/带参）、GetAllStrings、Culture 切换、
    /// null/空 key 边界、格式化行为、多次调用一致性、泛型适配器接口兼容。
    /// </summary>
    public class GameFrameXStringLocalizerTests : IDisposable
    {
        private readonly ResourceManager _resourceManager;
        private readonly CultureInfo _originalCulture;

        public GameFrameXStringLocalizerTests()
        {
            _originalCulture = CultureInfo.CurrentUICulture;
            _resourceManager = new ResourceManager();
        }

        public void Dispose()
        {
            CultureInfo.CurrentUICulture = _originalCulture;
            _resourceManager?.Dispose();
        }

        // ============================================================
        // 构造函数
        // ============================================================

        [Fact]
        public void Constructor_WithNullResourceManager_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GameFrameXStringLocalizer(null!));
        }

        [Fact]
        public void Constructor_WithValidResourceManager_ShouldCreateInstance()
        {
            var localizer = new GameFrameXStringLocalizer(_resourceManager);

            Assert.NotNull(localizer);
            Assert.IsAssignableFrom<IStringLocalizer>(localizer);
        }

        // ============================================================
        // 索引器 [name]（无参数）
        // ============================================================

        [Fact]
        public void Indexer_NoArgs_WithKnownKey_ShouldReturnLocalizedValueAndResourceNotFoundFalse()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Welcome", "", "Hello World")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Welcome"];

            Assert.Equal("Hello World", result.Value);
            Assert.False(result.ResourceNotFound);
            Assert.Equal("Welcome", result.Name);
        }

        [Fact]
        public void Indexer_NoArgs_WithKnownKey_ShouldSetSearchedLocationToResourceManagerTypeName()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Welcome", "", "Hello")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Welcome"];

            Assert.Equal(typeof(ResourceManager).FullName, result.SearchedLocation);
        }

        [Fact]
        public void Indexer_NoArgs_WithUnknownKey_ShouldReturnKeyAsValueAndResourceNotFoundTrue()
        {
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Non.Existent.Key.12345"];

            Assert.Equal("Non.Existent.Key.12345", result.Value);
            Assert.True(result.ResourceNotFound);
            Assert.Equal("Non.Existent.Key.12345", result.Name);
        }

        [Fact]
        public void Indexer_NoArgs_WithNullKey_ShouldThrowArgumentNullException()
        {
            // 源码在构造 LocalizedString 时传入 null name，LocalizedString 构造函数会抛 ArgumentNullException。
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            Assert.Throws<ArgumentNullException>(() => localizer[null!]);
        }

        [Fact]
        public void Indexer_NoArgs_WithEmptyKey_ShouldReturnEmptyValueAndResourceNotFoundTrue()
        {
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer[""];

            Assert.Equal("", result.Value);
            Assert.True(result.ResourceNotFound);
        }

        // ============================================================
        // 索引器 [name, params object[] arguments]
        // ============================================================

        [Fact]
        public void Indexer_WithArgs_WithKnownTemplateAndSingleArg_ShouldReturnFormattedString()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Greeting", "", "Hello {0}")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Greeting", "GameFrameX"];

            Assert.Equal("Hello GameFrameX", result.Value);
            Assert.False(result.ResourceNotFound);
        }

        [Fact]
        public void Indexer_WithArgs_WithKnownTemplateAndMultipleArgs_ShouldFormatAllPlaceholders()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Multi", "", "{0} + {1} = {2}")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Multi", 1, 2, 3];

            Assert.Equal("1 + 2 = 3", result.Value);
        }

        [Fact]
        public void Indexer_WithArgs_WithKnownTemplateAndNameMatchesInput()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Greeting", "", "Hello {0}")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Greeting", "World"];

            Assert.Equal("Greeting", result.Name);
        }

        [Fact]
        public void Indexer_WithArgs_WithNullArguments_ShouldReturnTemplateWithoutFormatting()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Greeting", "", "Hello {0}")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Greeting", null!];

            Assert.Equal("Hello {0}", result.Value);
            Assert.False(result.ResourceNotFound);
        }

        [Fact]
        public void Indexer_WithArgs_WithEmptyArgumentsArray_ShouldReturnTemplateWithoutFormatting()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Greeting", "", "Hello {0}")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Greeting", new object[0]];

            Assert.Equal("Hello {0}", result.Value);
            Assert.False(result.ResourceNotFound);
        }

        [Fact]
        public void Indexer_WithArgs_WithMissingKey_ShouldReturnKeyAndResourceNotFoundTrue()
        {
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Missing.Key.With.Args", "arg"];

            // 缺失 key 时 template == key，string.Format("Missing.Key.With.Args", "arg") 无占位符，返回 key 本身
            Assert.Equal("Missing.Key.With.Args", result.Value);
            Assert.True(result.ResourceNotFound);
        }

        [Fact]
        public void Indexer_WithArgs_WithInvalidTemplate_ShouldReturnTemplateByDefault()
        {
            // 默认 FormatErrorBehavior = ReturnTemplate（不抛异常，返回模板）
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Bad.Template", "", "Hello {0")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer["Bad.Template", "World"];

            Assert.Equal("Hello {0", result.Value);
            Assert.False(result.ResourceNotFound); // 模板已找到，仅格式化失败
        }

        // ============================================================
        // GetAllStrings
        // ============================================================

        [Fact]
        public void GetAllStrings_WithIncludeParentCulturesTrue_ShouldReturnEmptyCollection()
        {
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer.GetAllStrings(true);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void GetAllStrings_WithIncludeParentCulturesFalse_ShouldReturnEmptyCollection()
        {
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var result = localizer.GetAllStrings(false);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // ============================================================
        // Culture 切换
        // ============================================================

        [Fact]
        public void Indexer_NoArgs_WithCultureChange_ShouldReturnCultureSpecificValue()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Greeting", "", "Hello"),
                ("Greeting", "zh-Hans", "你好")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
            var invariantResult = localizer["Greeting"];
            Assert.Equal("Hello", invariantResult.Value);

            CultureInfo.CurrentUICulture = new CultureInfo("zh-Hans");
            var zhResult = localizer["Greeting"];
            Assert.Equal("你好", zhResult.Value);
        }

        [Fact]
        public void Indexer_WithArgs_WithCultureChange_ShouldFormatUsingCurrentCulture()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Price", "", "Price: {0:C}")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            var usResult = localizer["Price", 42.0];
            Assert.Contains("42", usResult.Value);

            CultureInfo.CurrentUICulture = new CultureInfo("zh-CN");
            var cnResult = localizer["Price", 42.0];
            Assert.Contains("42", cnResult.Value);
        }

        // ============================================================
        // 一致性
        // ============================================================

        [Fact]
        public void Indexer_NoArgs_MultipleCallsWithSameKey_ShouldReturnConsistentResults()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Stable.Key", "", "ConsistentValue")));
            IStringLocalizer localizer = new GameFrameXStringLocalizer(_resourceManager);

            var first = localizer["Stable.Key"];
            var second = localizer["Stable.Key"];
            var third = localizer["Stable.Key"];

            Assert.Equal(first.Value, second.Value);
            Assert.Equal(second.Value, third.Value);
            Assert.Equal(first.ResourceNotFound, second.ResourceNotFound);
            Assert.Equal(second.ResourceNotFound, third.ResourceNotFound);
            Assert.Equal(first.SearchedLocation, second.SearchedLocation);
        }

        // ============================================================
        // 泛型 GameFrameXStringLocalizer<T>
        // ============================================================

        [Fact]
        public void GenericLocalizer_ShouldImplementIStringLocalizerOfT()
        {
            var localizer = new GameFrameXStringLocalizer<TestMarker>(_resourceManager);

            Assert.IsAssignableFrom<IStringLocalizer<TestMarker>>(localizer);
            Assert.IsAssignableFrom<IStringLocalizer>(localizer);
        }

        [Fact]
        public void GenericLocalizer_ShouldBeSealed()
        {
            Assert.True(typeof(GameFrameXStringLocalizer<TestMarker>).IsSealed);
        }

        [Fact]
        public void GenericLocalizer_ShouldInheritFromNonGenericLocalizer()
        {
            var localizer = new GameFrameXStringLocalizer<TestMarker>(_resourceManager);

            Assert.IsType<GameFrameXStringLocalizer<TestMarker>>(localizer);
            Assert.IsAssignableFrom<GameFrameXStringLocalizer>(localizer);
        }

        [Fact]
        public void GenericLocalizer_ShouldBehaveSameAsNonGeneric_ForKnownKey()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Shared.Key", "", "SharedValue")));
            var genericLocalizer = new GameFrameXStringLocalizer<TestMarker>(_resourceManager);
            var nonGenericLocalizer = new GameFrameXStringLocalizer(_resourceManager);

            var genericResult = genericLocalizer["Shared.Key"];
            var nonGenericResult = nonGenericLocalizer["Shared.Key"];

            Assert.Equal(nonGenericResult.Value, genericResult.Value);
            Assert.Equal(nonGenericResult.ResourceNotFound, genericResult.ResourceNotFound);
            Assert.Equal(nonGenericResult.SearchedLocation, genericResult.SearchedLocation);
            Assert.Equal(nonGenericResult.Name, genericResult.Name);
        }

        [Fact]
        public void GenericLocalizer_WithArgs_ShouldFormatSameAsNonGeneric()
        {
            _resourceManager.RegisterProvider(new TestCultureProvider(
                ("Fmt", "", "Value={0}")));
            var genericLocalizer = new GameFrameXStringLocalizer<TestMarker>(_resourceManager);
            var nonGenericLocalizer = new GameFrameXStringLocalizer(_resourceManager);

            var genericResult = genericLocalizer["Fmt", 99];
            var nonGenericResult = nonGenericLocalizer["Fmt", 99];

            Assert.Equal(nonGenericResult.Value, genericResult.Value);
            Assert.Equal("Value=99", genericResult.Value);
        }

        [Fact]
        public void GenericLocalizer_GetAllStrings_ShouldReturnEmptyCollection()
        {
            var localizer = new GameFrameXStringLocalizer<TestMarker>(_resourceManager);

            var result = localizer.GetAllStrings(true);

            Assert.Empty(result);
        }

        [Fact]
        public void GenericLocalizer_WithUnknownKey_ShouldReturnKeyAndResourceNotFoundTrue()
        {
            var localizer = new GameFrameXStringLocalizer<TestMarker>(_resourceManager);

            var result = localizer["Unknown.Generic.Key"];

            Assert.Equal("Unknown.Generic.Key", result.Value);
            Assert.True(result.ResourceNotFound);
        }

        // ============================================================
        // 测试辅助类型
        // ============================================================

        /// <summary>
        /// 标记类型，用于泛型 localizer 测试。
        /// </summary>
        private sealed class TestMarker
        {
        }

        /// <summary>
        /// 测试用的 culture-aware 资源提供者，按 (key, cultureName) 存储本地化值。
        /// cultureName 为空字符串 "" 对应 InvariantCulture。
        /// </summary>
        private sealed class TestCultureProvider : ICultureResourceProvider
        {
            private readonly Dictionary<(string Key, string Culture), string> _values;

            public TestCultureProvider(params (string Key, string Culture, string Value)[] values)
            {
                _values = new Dictionary<(string, string), string>();
                foreach (var item in values)
                {
                    _values[(item.Key, item.Culture)] = item.Value;
                }
            }

            public string AssemblyName
            {
                get { return nameof(TestCultureProvider); }
            }

            public string GetString(string key)
            {
                return GetString(key, CultureInfo.CurrentUICulture);
            }

            public string GetString(string key, CultureInfo culture)
            {
                var cultureName = culture != null ? culture.Name : string.Empty;
                if (_values.TryGetValue((key, cultureName), out var value))
                {
                    return value;
                }

                return key;
            }
        }
    }
}
