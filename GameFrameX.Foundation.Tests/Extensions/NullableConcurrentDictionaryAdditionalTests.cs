using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// NullableConcurrentDictionary 补充覆盖测试：Add/Remove/FallbackValue/WithCapacity/WithFallbackValue
    /// 及条件索引器 setter 路径。
    /// </summary>
    public class NullableConcurrentDictionaryAdditionalTests
    {
        // ============================================================
        // Add 方法
        // ============================================================

        [Fact]
        public void Add_NewKey_ShouldAddToDictionary()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act
            dictionary.Add("key", 42);

            // Assert
            Assert.Equal(42, dictionary["key"]);
            Assert.Single(dictionary);
        }

        [Fact]
        public void Add_NullKey_ShouldWork()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act
            dictionary.Add(null, 42);

            // Assert
            Assert.Equal(42, dictionary[(string)null]);
        }

        // ============================================================
        // Remove 方法
        // ============================================================

        [Fact]
        public void Remove_ExistingKey_ShouldReturnTrueAndRemove()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();
            dictionary["key"] = 42;

            // Act
            var result = dictionary.Remove("key");

            // Assert
            Assert.True(result);
            Assert.Empty(dictionary);
        }

        [Fact]
        public void Remove_NonExistingKey_ShouldReturnFalse()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act
            var result = dictionary.Remove("nonexistent");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Remove_NullKey_ShouldRemoveCorrectly()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();
            dictionary[(string)null] = 42;

            // Act
            var result = dictionary.Remove(null);

            // Assert
            Assert.True(result);
            Assert.Empty(dictionary);
        }

        // ============================================================
        // FallbackValue 属性
        // ============================================================

        [Fact]
        public void FallbackValue_GetSet_ShouldWorkCorrectly()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act
            dictionary.FallbackValue = 99;

            // Assert
            Assert.Equal(99, dictionary.FallbackValue);
            Assert.Equal(99, dictionary["nonexistent"]);
        }

        [Fact]
        public void FallbackValue_Default_ShouldBeDefaultValue()
        {
            // Arrange & Act
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Assert
            Assert.Equal(0, dictionary.FallbackValue);
        }

        // ============================================================
        // WithCapacity 静态方法
        // ============================================================

        [Fact]
        public void WithCapacity_ValidCapacity_ShouldCreateDictionary()
        {
            // Arrange & Act
            var dictionary = NullableConcurrentDictionary<string, int>.WithCapacity(10);

            // Assert
            Assert.Empty(dictionary);
        }

        [Fact]
        public void WithCapacity_ZeroCapacity_ShouldCreateEmptyDictionary()
        {
            // Arrange & Act
            var dictionary = NullableConcurrentDictionary<string, int>.WithCapacity(0);

            // Assert
            Assert.Empty(dictionary);
        }

        [Fact]
        public void WithCapacity_NegativeCapacity_ShouldThrowArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                NullableConcurrentDictionary<string, int>.WithCapacity(-1));
        }

        // ============================================================
        // WithFallbackValue 静态方法
        // ============================================================

        [Fact]
        public void WithFallbackValue_ShouldCreateDictionaryWithFallbackValue()
        {
            // Arrange & Act
            var dictionary = NullableConcurrentDictionary<string, int>.WithFallbackValue(42);

            // Assert
            Assert.Empty(dictionary);
            Assert.Equal(42, dictionary["nonexistent"]);
        }

        // ============================================================
        // 构造函数：capacity
        // ============================================================

        [Fact]
        public void Constructor_WithCapacity_ShouldCreateEmptyDictionary()
        {
            // Arrange & Act
            var dictionary = new NullableConcurrentDictionary<string, string>(5);

            // Assert
            Assert.Empty(dictionary);
        }

        [Fact]
        public void Constructor_WithNegativeCapacity_ShouldThrow()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new NullableConcurrentDictionary<string, string>(-1));
        }

        // ============================================================
        // 构造函数：collection
        // ============================================================

        [Fact]
        public void Constructor_WithCollection_ShouldInitializeFromCollection()
        {
            // Arrange
            var pairs = new List<KeyValuePair<NullObject<string>, int>>
            {
                new KeyValuePair<NullObject<string>, int>(new NullObject<string>("a"), 1),
                new KeyValuePair<NullObject<string>, int>(new NullObject<string>("b"), 2),
            };

            // Act
            var dictionary = new NullableConcurrentDictionary<string, int>(pairs);

            // Assert
            Assert.Equal(2, dictionary.Count);
            Assert.Equal(1, dictionary["a"]);
            Assert.Equal(2, dictionary["b"]);
        }

        [Fact]
        public void Constructor_WithNullCollection_ShouldThrowArgumentNullException()
        {
            // Arrange
            IEnumerable<KeyValuePair<NullObject<string>, int>> collection = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new NullableConcurrentDictionary<string, int>(collection));
        }

        // ============================================================
        // 构造函数：concurrencyLevel + collection + comparer
        // ============================================================

        [Fact]
        public void Constructor_WithConcurrencyLevelCollectionComparer_ShouldInitialize()
        {
            // Arrange
            var pairs = new List<KeyValuePair<NullObject<string>, int>>
            {
                new KeyValuePair<NullObject<string>, int>(new NullObject<string>("a"), 1),
            };

            // Act
            var dictionary = new NullableConcurrentDictionary<string, int>(
                2, pairs, EqualityComparer<NullObject<string>>.Default);

            // Assert
            Assert.Single(dictionary);
            Assert.Equal(1, dictionary["a"]);
        }

        [Fact]
        public void Constructor_ConcurrencyLevelCollectionComparer_ZeroConcurrency_ShouldThrow()
        {
            // Arrange
            var pairs = new List<KeyValuePair<NullObject<string>, int>>();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new NullableConcurrentDictionary<string, int>(
                    0, pairs, EqualityComparer<NullObject<string>>.Default));
        }

        // ============================================================
        // 构造函数：fallbackValue (TValue direct)
        // ============================================================

        [Fact]
        public void Constructor_WithDirectFallbackValue_ShouldSetFallbackValue()
        {
            // 当 TValue 为 int 时，new NullableConcurrentDictionary<string, int>(42) 会匹配 capacity 构造函数。
            // 使用 string 作为 TValue 来正确测试 fallbackValue 构造函数。
            // Arrange & Act
            var dictionary = new NullableConcurrentDictionary<int, string>("fallback");

            // Assert
            Assert.Equal("fallback", dictionary.FallbackValue);
            Assert.Equal("fallback", dictionary[999]);
        }

        // ============================================================
        // 构造函数：concurrencyLevel + capacity + comparer
        // ============================================================

        [Fact]
        public void Constructor_WithConcurrencyLevelCapacityComparer_NullComparer_ShouldThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new NullableConcurrentDictionary<string, int>(2, 10, null));
        }

        // ============================================================
        // 条件索引器 setter
        // ============================================================

        [Fact]
        public void Indexer_ConditionKeyValuePair_Set_ShouldUpdateMatchingEntries()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();
            dictionary["a"] = 1;
            dictionary["b"] = 2;
            dictionary["c"] = 3;

            // Act - set all entries with value > 1 to 100
            dictionary[(Func<KeyValuePair<string, int>, bool>)(kvp => kvp.Value > 1)] = 100;

            // Assert
            Assert.Equal(1, dictionary["a"]);
            Assert.Equal(100, dictionary["b"]);
            Assert.Equal(100, dictionary["c"]);
        }

        [Fact]
        public void Indexer_ConditionKeyValue_Set_ShouldUpdateMatchingEntries()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();
            dictionary["a"] = 1;
            dictionary["b"] = 2;

            // Act
            dictionary[(Func<string, int, bool>)((key, value) => key == "a")] = 99;

            // Assert
            Assert.Equal(99, dictionary["a"]);
            Assert.Equal(2, dictionary["b"]);
        }

        [Fact]
        public void Indexer_ConditionKey_Set_ShouldUpdateMatchingEntries()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();
            dictionary["a"] = 1;
            dictionary["b"] = 2;

            // Act
            dictionary[(Func<string, bool>)(key => key == "b")] = 88;

            // Assert
            Assert.Equal(1, dictionary["a"]);
            Assert.Equal(88, dictionary["b"]);
        }

        [Fact]
        public void Indexer_ConditionValue_Set_ShouldUpdateMatchingEntries()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();
            dictionary["a"] = 1;
            dictionary["b"] = 2;

            // Act
            dictionary[(Func<int, bool>)(value => value == 1)] = 77;

            // Assert
            Assert.Equal(77, dictionary["a"]);
            Assert.Equal(2, dictionary["b"]);
        }

        [Fact]
        public void Indexer_ConditionKeyValuePair_NoMatch_ShouldReturnFallbackValue()
        {
            // 当 TValue 为 int 时，构造函数 int 参数匹配 capacity 重载而非 fallbackValue 重载，
            // 使用 WithFallbackValue 静态方法正确设置 fallback 值。
            // Arrange
            var dictionary = NullableConcurrentDictionary<string, int>.WithFallbackValue(42);
            dictionary["a"] = 1;

            // Act
            var result = dictionary[(Func<KeyValuePair<string, int>, bool>)(kvp => kvp.Value == 999)];

            // Assert
            Assert.Equal(42, result);
        }

        [Fact]
        public void Indexer_ConditionKeyValue_NullCondition_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var x = dictionary[(Func<string, int, bool>)null];
            });
        }

        [Fact]
        public void Indexer_ConditionKey_NullCondition_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var x = dictionary[(Func<string, bool>)null];
            });
        }

        [Fact]
        public void Indexer_ConditionValue_NullCondition_ShouldThrowArgumentNullException()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var x = dictionary[(Func<int, bool>)null];
            });
        }

        // ============================================================
        // TKey 索引器
        // ============================================================

        [Fact]
        public void Indexer_TKey_NonExistingKey_ShouldReturnFallbackValue()
        {
            // 当 TValue 为 int 时，构造函数 int 参数匹配 capacity 重载而非 fallbackValue 重载，
            // 使用 WithFallbackValue 静态方法正确设置 fallback 值。
            // Arrange
            var dictionary = NullableConcurrentDictionary<string, int>.WithFallbackValue(55);

            // Act
            var result = dictionary["missing"];

            // Assert
            Assert.Equal(55, result);
        }

        [Fact]
        public void Indexer_TKey_SetAndGet_ShouldWork()
        {
            // Arrange
            var dictionary = new NullableConcurrentDictionary<string, int>();

            // Act
            dictionary["key"] = 100;

            // Assert
            Assert.Equal(100, dictionary["key"]);
        }
    }
}
