using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// IDictionaryExtensions 补充覆盖测试：batch AddOrUpdate、工厂重载、Async 变体、
    /// AddOrUpdateTo、GetOrAdd(Dictionary)、GetOrAddAsync、ForEachAsync、
    /// ToDictionarySafety/ToDisposableDictionary/ToConcurrentDictionary 变体、
    /// ToLookupX(elementSelector)、AsConcurrentDictionary/AsDictionary(defaultValue)。
    /// </summary>
    public class IDictionaryExtensionsAdditionalTests
    {
        // ============================================================
        // AddOrUpdate(IDictionary, IDictionary) - 批量合并
        // ============================================================

        [Fact]
        public void AddOrUpdate_BatchDictionary_ShouldMergeAllEntries()
        {
            // Arrange
            var target = new Dictionary<string, int>
            {
                { "a", 1 },
            };
            var source = new Dictionary<string, int>
            {
                { "a", 10 },
                { "b", 20 },
            };

            // Act
            target.AddOrUpdate(source);

            // Assert
            Assert.Equal(10, target["a"]); // updated
            Assert.Equal(20, target["b"]); // added
            Assert.Equal(2, target.Count);
        }

        [Fact]
        public void AddOrUpdate_BatchDictionary_NullSelf_ShouldThrow()
        {
            // Arrange
            IDictionary<string, int> target = null;
            var source = new Dictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.AddOrUpdate(source));
        }

        [Fact]
        public void AddOrUpdate_BatchDictionary_NullSource_ShouldThrow()
        {
            // Arrange
            var target = new Dictionary<string, int>();
            IDictionary<string, int> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.AddOrUpdate(source));
        }

        [Fact]
        public void AddOrUpdate_BatchDictionary_EmptySource_ShouldNotModifyTarget()
        {
            // Arrange
            var target = new Dictionary<string, int> { { "a", 1 } };
            var source = new Dictionary<string, int>();

            // Act
            target.AddOrUpdate(source);

            // Assert
            Assert.Single(target);
            Assert.Equal(1, target["a"]);
        }

        // ============================================================
        // AddOrUpdate(key, addValue, updateValue)
        // ============================================================

        [Fact]
        public void AddOrUpdate_KeyAddValueUpdateValue_NewKey_ShouldAdd()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = dict.AddOrUpdate("key", 5, 99);

            // Assert
            Assert.Equal(5, result);
            Assert.Equal(5, dict["key"]);
        }

        [Fact]
        public void AddOrUpdate_KeyAddValueUpdateValue_ExistingKey_ShouldUpdate()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 1 } };

            // Act
            var result = dict.AddOrUpdate("key", 5, 99);

            // Assert
            Assert.Equal(99, result);
            Assert.Equal(99, dict["key"]);
        }

        [Fact]
        public void AddOrUpdate_KeyAddValueUpdateValue_NullKey_ShouldThrow()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dict.AddOrUpdate(null, 5, 99));
        }

        // ============================================================
        // AddOrUpdate(key, addValueFactory, updateValueFactory)
        // ============================================================

        [Fact]
        public void AddOrUpdate_KeyFactories_NewKey_ShouldUseAddFactory()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = dict.AddOrUpdate("key", k => 42, (k, v) => v + 1);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void AddOrUpdate_KeyFactories_ExistingKey_ShouldUseUpdateFactory()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 10 } };

            // Act
            var result = dict.AddOrUpdate("key", k => 42, (k, v) => v + 5);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void AddOrUpdate_KeyFactories_NullAddFactory_ShouldThrow()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dict.AddOrUpdate("key", null, (k, v) => v));
        }

        // ============================================================
        // AddOrUpdate(IDictionary, IDictionary, updateValueFactory)
        // ============================================================

        [Fact]
        public void AddOrUpdate_BatchWithFactory_ShouldApplyFactoryOnConflict()
        {
            // Arrange
            var target = new Dictionary<string, int> { { "a", 1 } };
            var source = new Dictionary<string, int> { { "a", 10 }, { "b", 20 } };

            // Act
            target.AddOrUpdate(source, (key, existing) => existing + 100);

            // Assert
            Assert.Equal(101, target["a"]); // 1 + 100
            Assert.Equal(20, target["b"]);  // new add
        }

        [Fact]
        public void AddOrUpdate_BatchWithFactory_NullFactory_ShouldThrow()
        {
            // Arrange
            var target = new Dictionary<string, int>();
            var source = new Dictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => target.AddOrUpdate(source, null));
        }

        // ============================================================
        // AddOrUpdate on NullableDictionary
        // ============================================================

        [Fact]
        public void AddOrUpdate_NullableDictionary_KeyAddValueFactory_ShouldWork()
        {
            // Arrange
            var dict = new NullableDictionary<string, int>();

            // Act
            var result = dict.AddOrUpdate("key", 5, (k, v) => v + 1);

            // Assert
            Assert.Equal(5, result);
            Assert.Equal(5, dict["key"]);
        }

        [Fact]
        public void AddOrUpdate_NullableDictionary_BatchDictionary_ShouldMerge()
        {
            // Arrange
            var dict = new NullableDictionary<string, int> { { "a", 1 } };
            var source = new Dictionary<string, int> { { "a", 10 }, { "b", 20 } };

            // Act
            dict.AddOrUpdate(source);

            // Assert
            Assert.Equal(10, dict["a"]);
            Assert.Equal(20, dict["b"]);
        }

        [Fact]
        public void AddOrUpdate_NullableDictionary_NullSelf_ShouldThrow()
        {
            // Arrange
            NullableDictionary<string, int> dict = null;
            var source = new Dictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => dict.AddOrUpdate(source));
        }

        // ============================================================
        // AddOrUpdate on NullableConcurrentDictionary
        // ============================================================

        [Fact]
        public void AddOrUpdate_NullableConcurrentDictionary_KeyAddValueFactory_ShouldWork()
        {
            // Arrange
            var dict = new NullableConcurrentDictionary<string, int>();

            // Act
            var result = dict.AddOrUpdate("key", 5, (k, v) => v + 1);

            // Assert
            Assert.Equal(5, result);
            Assert.Equal(5, dict["key"]);
        }

        [Fact]
        public void AddOrUpdate_NullableConcurrentDictionary_KeyFactories_ShouldWork()
        {
            // Arrange
            var dict = new NullableConcurrentDictionary<string, int>();

            // Act
            var result = dict.AddOrUpdate("key", k => 42, (k, v) => v + 1);

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void AddOrUpdate_NullableConcurrentDictionary_BatchDictionary_ShouldMerge()
        {
            // Arrange
            var dict = new NullableConcurrentDictionary<string, int>();
            var source = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };

            // Act
            dict.AddOrUpdate(source);

            // Assert
            Assert.Equal(1, dict["a"]);
            Assert.Equal(2, dict["b"]);
        }

        // ============================================================
        // AddOrUpdateAsync
        // ============================================================

        [Fact]
        public async Task AddOrUpdateAsync_IDictionary_NewKey_ShouldAdd()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = await dict.AddOrUpdateAsync("key", 5, (k, v) => Task.FromResult(v + 1));

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task AddOrUpdateAsync_IDictionary_ExistingKey_ShouldUpdate()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 10 } };

            // Act
            var result = await dict.AddOrUpdateAsync("key", 5, (k, v) => Task.FromResult(v * 2));

            // Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public async Task AddOrUpdateAsync_IDictionary_NullKey_ShouldThrow()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                dict.AddOrUpdateAsync(null, 5, (k, v) => Task.FromResult(v)));
        }

        [Fact]
        public async Task AddOrUpdateAsync_NullableDictionary_ShouldWork()
        {
            // Arrange
            var dict = new NullableDictionary<string, int>();

            // Act
            var result = await dict.AddOrUpdateAsync("key", 5, (k, v) => Task.FromResult(v + 1));

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task AddOrUpdateAsync_NullableConcurrentDictionary_ShouldWork()
        {
            // Arrange
            var dict = new NullableConcurrentDictionary<string, int>();

            // Act
            var result = await dict.AddOrUpdateAsync("key", 5, (k, v) => Task.FromResult(v + 1));

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WithFactories_NewKey_ShouldUseAddFactory()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = await dict.AddOrUpdateAsync(
                "key", k => Task.FromResult(42), (k, v) => Task.FromResult(v + 1));

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WithFactories_ExistingKey_ShouldUseUpdateFactory()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 10 } };

            // Act
            var result = await dict.AddOrUpdateAsync(
                "key", k => Task.FromResult(42), (k, v) => Task.FromResult(v + 5));

            // Assert
            Assert.Equal(15, result);
        }

        // ============================================================
        // AddOrUpdateTo
        // ============================================================

        [Fact]
        public void AddOrUpdateTo_ShouldCopyEntriesToTarget()
        {
            // Arrange
            var source = new Dictionary<string, int> { { "a", 1 }, { "b", 2 } };
            var target = new Dictionary<string, int> { { "c", 3 } };

            // Act
            source.AddOrUpdateTo(target);

            // Assert
            Assert.Equal(3, target.Count);
            Assert.Equal(1, target["a"]);
            Assert.Equal(2, target["b"]);
            Assert.Equal(3, target["c"]);
        }

        [Fact]
        public void AddOrUpdateTo_WithFactory_ShouldApplyFactoryOnConflict()
        {
            // Arrange
            var source = new Dictionary<string, int> { { "a", 10 } };
            var target = new Dictionary<string, int> { { "a", 1 } };

            // Act
            source.AddOrUpdateTo((IDictionary<string, int>)target, (key, existing) => existing + 100);

            // Assert
            Assert.Equal(101, target["a"]);
        }

        [Fact]
        public void AddOrUpdateTo_NullableConcurrentDictionaryTarget_ShouldWork()
        {
            // Arrange
            var source = new Dictionary<string, int> { { "a", 1 } };
            var target = new NullableConcurrentDictionary<string, int>();

            // Act
            source.AddOrUpdateTo(target, (key, existing) => existing + 1);

            // Assert
            Assert.Equal(1, target["a"]);
        }

        // ============================================================
        // AddOrUpdateToAsync
        // ============================================================

        [Fact]
        public async Task AddOrUpdateToAsync_ShouldCopyEntriesToTarget()
        {
            // Arrange
            var source = new Dictionary<string, int> { { "a", 1 } };
            var target = new Dictionary<string, int>();

            // Act
            await source.AddOrUpdateToAsync((IDictionary<string, int>)target, (k, v) => Task.FromResult(v + 1));

            // Assert
            Assert.Equal(1, target["a"]);
        }

        // ============================================================
        // GetOrAdd(Dictionary, key, addValue)
        // ============================================================

        [Fact]
        public void GetOrAdd_Dictionary_KeyValue_NewKey_ShouldAdd()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = dict.GetOrAdd("key", 42);

            // Assert
            Assert.Equal(42, result);
            Assert.Equal(42, dict["key"]);
        }

        [Fact]
        public void GetOrAdd_Dictionary_KeyValue_ExistingKey_ShouldReturnExisting()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 10 } };

            // Act
            var result = dict.GetOrAdd("key", 42);

            // Assert
            Assert.Equal(10, result);
        }

        // ============================================================
        // GetOrAddAsync
        // ============================================================

        [Fact]
        public async Task GetOrAddAsync_NewKey_ShouldAddFromFactory()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act
            var result = await dict.GetOrAddAsync("key", () => Task.FromResult(42));

            // Assert
            Assert.Equal(42, result);
            Assert.Equal(42, dict["key"]);
        }

        [Fact]
        public async Task GetOrAddAsync_ExistingKey_ShouldReturnExisting()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 10 } };

            // Act
            var result = await dict.GetOrAddAsync("key", () => Task.FromResult(42));

            // Assert
            Assert.Equal(10, result);
        }

        // ============================================================
        // ForEachAsync
        // ============================================================

        [Fact]
        public async Task ForEachAsync_IDictionary_ShouldExecuteActionForAllEntries()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "a", 1 }, { "b", 2 }, { "c", 3 } };
            var sum = 0;

            // Act
            await dict.ForEachAsync((key, value) =>
            {
                sum += value;
                return Task.CompletedTask;
            });

            // Assert
            Assert.Equal(6, sum);
        }

        [Fact]
        public async Task ForEachAsync_IDictionary_NullDict_ShouldThrow()
        {
            // Arrange
            IDictionary<string, int> dict = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                dict.ForEachAsync((k, v) => Task.CompletedTask));
        }

        [Fact]
        public async Task ForEachAsync_IDictionary_NullAction_ShouldThrow()
        {
            // Arrange
            var dict = new Dictionary<string, int>();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                dict.ForEachAsync(null));
        }

        // ============================================================
        // ToDictionarySafety 变体
        // ============================================================

        [Fact]
        public void ToDictionarySafety_WithDefaultValue_ShouldSetFallbackValue()
        {
            // Arrange
            var source = new[] { "a", "bb" };

            // Act
            var result = source.ToDictionarySafety(s => s.Length, "default");

            // Assert - FallbackValue is internal on NullableDictionary, verify via missing-key behavior
            Assert.Equal("default", result[999]);
            Assert.Equal("a", result[1]);
            Assert.Equal("bb", result[2]);
        }

        [Fact]
        public void ToDictionarySafety_WithElementSelector_ShouldUseElementSelector()
        {
            // Arrange
            var source = new[] { "a", "bb" };

            // Act
            var result = source.ToDictionarySafety(s => s.Length, s => s.ToUpperInvariant());

            // Assert
            Assert.Equal("A", result[1]);
            Assert.Equal("BB", result[2]);
        }

        [Fact]
        public void ToDictionarySafety_WithElementSelectorAndDefault_ShouldWork()
        {
            // Arrange
            var source = new[] { "a", "bb" };

            // Act
            var result = source.ToDictionarySafety(s => s.Length, s => s.Length, 99);

            // Assert - FallbackValue is internal on NullableDictionary, verify via missing-key behavior
            Assert.Equal(99, result[999]);
            Assert.Equal(1, result[1]);
            Assert.Equal(2, result[2]);
        }

        [Fact]
        public void ToDictionarySafety_EmptySource_ShouldReturnEmptyDictionary()
        {
            // Arrange
            var source = new string[0];

            // Act
            var result = source.ToDictionarySafety(s => s.Length);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ToDictionarySafety_NullElementSelector_ShouldThrow()
        {
            // Arrange
            var source = new[] { "a" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                source.ToDictionarySafety<string, int, int>(s => s.Length, null));
        }

        // ============================================================
        // ToConcurrentDictionary 变体
        // ============================================================

        [Fact]
        public void ToConcurrentDictionary_WithElementSelector_ShouldUseElementSelector()
        {
            // Arrange
            var source = new[] { "a", "bb" };

            // Act
            var result = source.ToConcurrentDictionary(s => s.Length, s => s.ToUpperInvariant());

            // Assert
            Assert.Equal("A", result[1]);
            Assert.Equal("BB", result[2]);
        }

        [Fact]
        public void ToConcurrentDictionary_WithDefaultValue_ShouldSetFallback()
        {
            // Arrange
            var source = new[] { "a" };

            // Act
            var result = source.ToConcurrentDictionary(s => s.Length, "fallback");

            // Assert
            Assert.Equal("fallback", result.FallbackValue);
            Assert.Equal("a", result[1]);
        }

        [Fact]
        public void ToConcurrentDictionary_WithElementSelectorAndDefault_ShouldWork()
        {
            // Arrange
            var source = new[] { "a", "bb" };

            // Act
            var result = source.ToConcurrentDictionary(s => s.Length, s => s.Length, -1);

            // Assert
            Assert.Equal(-1, result.FallbackValue);
            Assert.Equal(1, result[1]);
            Assert.Equal(2, result[2]);
        }

        // ============================================================
        // AsConcurrentDictionary with default
        // ============================================================

        [Fact]
        public void AsConcurrentDictionary_WithDefaultValue_ShouldSetFallback()
        {
            // Arrange
            var dict = new Dictionary<string, int> { { "key", 1 } };

            // Act
            var result = dict.AsConcurrentDictionary(99);

            // Assert
            Assert.Equal(99, result.FallbackValue);
            Assert.Equal(1, result["key"]);
        }

        // ============================================================
        // AsDictionary with default
        // ============================================================

        [Fact]
        public void AsDictionary_WithDefaultValue_ShouldSetFallback()
        {
            // Arrange
            var dict = new ConcurrentDictionary<string, int>();
            dict["key"] = 1;

            // Act
            var result = dict.AsDictionary(99);

            // Assert - FallbackValue is internal on NullableDictionary, verify via missing-key behavior
            Assert.Equal(99, result["nonexistent"]);
            Assert.Equal(1, result["key"]);
        }

        // ============================================================
        // ToLookupX with elementSelector
        // ============================================================

        [Fact]
        public void ToLookupX_WithElementSelector_ShouldUseElementSelector()
        {
            // Arrange
            var source = new[] { "a", "b", "cc" };

            // Act
            var result = source.ToLookupX(s => s.Length, s => s.ToUpperInvariant());

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("A", result[1]);
            Assert.Contains("B", result[1]);
            Assert.Contains("CC", result[2]);
        }

        [Fact]
        public void ToLookupX_WithElementSelector_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<string> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                source.ToLookupX(s => s.Length, s => s));
        }

        [Fact]
        public void ToLookupX_WithElementSelector_NullElementSelector_ShouldThrow()
        {
            // Arrange
            var source = new[] { "a" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                source.ToLookupX<string, int, string>(s => s.Length, null));
        }
    }
}
