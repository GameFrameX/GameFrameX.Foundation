using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions
{
    /// <summary>
    /// IEnumerableExtensions 补充覆盖测试：IntersectByComparer(keySelector/comparer)、
    /// IntersectAllComparer(comparer/keySelector+comparer)、AddRangeIf、AddRangeIfNotContains、
    /// RemoveWhere、InsertAfter、ToHashSet、ForeachAsync/ForAsync、CompareChangesPlus、
    /// AsNotNull(IEnumerable)、WhereIf(IQueryable)、null 参数。
    /// </summary>
    public class IEnumerableExtensionsAdditionalTests
    {
        // ============================================================
        // IntersectByComparer with keySelector
        // ============================================================

        [Fact]
        public void IntersectByComparer_KeySelector_ShouldReturnIntersection()
        {
            // Arrange
            var first = new[] { "apple", "banana", "cherry" };
            var second = new[] { "avocado", "blueberry" };

            // Act
            var result = first.IntersectByComparer(second, s => s[0]).ToList();

            // Assert - "apple"(key 'a') 匹配 "avocado"，"banana"(key 'b') 匹配 "blueberry"
            // IntersectByIterator 使用 HashSet.Remove，每个键只能匹配一次，两个键都匹配
            Assert.Equal(2, result.Count);
            Assert.Contains("apple", result);
            Assert.Contains("banana", result);
        }

        [Fact]
        public void IntersectByComparer_KeySelectorWithComparer_ShouldUseComparer()
        {
            // Arrange
            var first = new[] { "Apple", "Banana", "Cherry" };
            var second = new[] { "avocado", "blueberry" };
            var comparer = StringComparer.OrdinalIgnoreCase;

            // Act
            var result = first.IntersectByComparer(second, s => s[0].ToString(), comparer).ToList();

            // Assert
            Assert.Contains("Apple", result);
            Assert.Contains("Banana", result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void IntersectByComparer_KeySelector_NullFirst_ShouldThrow()
        {
            // Arrange
            IEnumerable<string> first = null;
            var second = new[] { "a" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                first.IntersectByComparer(second, s => s).ToList());
        }

        [Fact]
        public void IntersectByComparer_Condition_EmptyResult_ShouldReturnEmpty()
        {
            // Arrange
            var first = new[] { 1, 2, 3 };
            var second = new[] { "x", "y" };

            // Act
            var result = first.IntersectByComparer(second, (f, s) => false).ToList();

            // Assert
            Assert.Empty(result);
        }

        // ============================================================
        // IntersectAllComparer with comparer / keySelector+comparer
        // ============================================================

        [Fact]
        public void IntersectAllComparer_WithComparer_ShouldUseCustomComparer()
        {
            // Arrange
            var collections = new[]
            {
                new[] { "A", "B", "C" },
                new[] { "a", "b", "d" },
            };

            // Act
            var result = collections.IntersectAllComparer(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("A", result); // or "a" - first collection's element wins
            Assert.Contains("B", result);
        }

        [Fact]
        public void IntersectAllComparer_WithKeySelectorAndComparer_ShouldIntersectByKey()
        {
            // Arrange
            var collections = new[]
            {
                new[] { "apple", "banana" },
                new[] { "avocado", "blueberry" },
            };

            // Act
            var result = collections.IntersectAllComparer(s => s[0]).ToList();

            // Assert - both start with 'a' and 'b'
            Assert.Contains("apple", result);
            Assert.Contains("banana", result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void IntersectAllComparer_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<IEnumerable<int>> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.IntersectAllComparer().ToList());
        }

        // ============================================================
        // AddRangeValues (IEnumerable overload)
        // ============================================================

        [Fact]
        public void AddRangeValues_IEnumerable_ShouldAddAllElements()
        {
            // Arrange
            var collection = new List<int> { 1 };
            var values = (IEnumerable<int>)new List<int> { 2, 3 };

            // Act
            collection.AddRangeValues(values);

            // Assert
            Assert.Equal(new int[] { 1, 2, 3 }, collection);
        }

        [Fact]
        public void AddRangeValues_IEnumerable_NullCollection_ShouldThrow()
        {
            // Arrange
            ICollection<int> collection = null;
            var values = new List<int> { 1 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => collection.AddRangeValues(values));
        }

        [Fact]
        public void AddRangeValues_IEnumerable_NullValues_ShouldThrow()
        {
            // Arrange
            var collection = new List<int>();
            IEnumerable<int> values = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => collection.AddRangeValues(values));
        }

        // ============================================================
        // AddRangeIf
        // ============================================================

        [Fact]
        public void AddRangeIf_ICollection_ShouldOnlyAddMatchingElements()
        {
            // Arrange
            var collection = new List<int> { 1 };
            var values = new int[] { 2, 3, 4, 5 };

            // Act
            collection.AddRangeIf(x => x % 2 == 0, values);

            // Assert
            Assert.Equal(new int[] { 1, 2, 4 }, collection);
        }

        [Fact]
        public void AddRangeIf_ICollection_NoMatchingElements_ShouldNotAdd()
        {
            // Arrange
            var collection = new List<int> { 1 };
            var values = new int[] { 1, 3, 5 };

            // Act
            collection.AddRangeIf(x => x > 10, values);

            // Assert
            Assert.Single(collection);
        }

        [Fact]
        public void AddRangeIf_ConcurrentBag_ShouldAddMatchingElements()
        {
            // Arrange
            var bag = new ConcurrentBag<int>();
            var values = new int[] { 1, 2, 3, 4 };

            // Act
            bag.AddRangeIf(x => x > 2, values);

            // Assert
            Assert.Equal(2, bag.Count);
            Assert.Contains(3, bag);
            Assert.Contains(4, bag);
        }

        [Fact]
        public void AddRangeIf_ConcurrentQueue_ShouldAddMatchingElements()
        {
            // Arrange
            var queue = new ConcurrentQueue<int>();
            var values = new int[] { 1, 2, 3 };

            // Act
            queue.AddRangeIf(x => x == 2, values);

            // Assert
            Assert.Single(queue);
            Assert.True(queue.TryDequeue(out var item));
            Assert.Equal(2, item);
        }

        [Fact]
        public void AddRangeIf_NullPredicate_ShouldThrow()
        {
            // Arrange
            var collection = new List<int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => collection.AddRangeIf(null, 1, 2));
        }

        // ============================================================
        // AddRangeIfNotContains
        // ============================================================

        [Fact]
        public void AddRangeIfNotContains_ShouldOnlyAddNonExistingElements()
        {
            // Arrange
            var collection = new List<int> { 1, 2 };

            // Act
            collection.AddRangeIfNotContains(2, 3, 4);

            // Assert
            Assert.Equal(new int[] { 1, 2, 3, 4 }, collection);
        }

        [Fact]
        public void AddRangeIfNotContains_AllDuplicates_ShouldNotAdd()
        {
            // Arrange
            var collection = new List<int> { 1, 2, 3 };

            // Act
            collection.AddRangeIfNotContains(1, 2, 3);

            // Assert
            Assert.Equal(3, collection.Count);
        }

        [Fact]
        public void AddRangeIfNotContains_NullCollection_ShouldThrow()
        {
            // Arrange
            ICollection<int> collection = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => collection.AddRangeIfNotContains(1));
        }

        // ============================================================
        // RemoveWhere
        // ============================================================

        [Fact]
        public void RemoveWhere_ShouldRemoveMatchingElements()
        {
            // Arrange
            var collection = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            collection.RemoveWhere(x => x % 2 == 0);

            // Assert
            Assert.Equal(new int[] { 1, 3, 5 }, collection);
        }

        [Fact]
        public void RemoveWhere_NoMatch_ShouldNotRemove()
        {
            // Arrange
            var collection = new List<int> { 1, 2, 3 };

            // Act
            collection.RemoveWhere(x => x > 10);

            // Assert
            Assert.Equal(3, collection.Count);
        }

        [Fact]
        public void RemoveWhere_NullCollection_ShouldThrow()
        {
            // Arrange
            ICollection<int> collection = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => collection.RemoveWhere(x => true));
        }

        [Fact]
        public void RemoveWhere_NullPredicate_ShouldThrow()
        {
            // Arrange
            var collection = new List<int>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => collection.RemoveWhere(null));
        }

        // ============================================================
        // InsertAfter
        // ============================================================

        [Fact]
        public void InsertAfter_Condition_ShouldInsertAfterMatchingElements()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            list.InsertAfter(x => x == 2, 99);

            // Assert
            Assert.Equal(new int[] { 1, 2, 99, 3 }, list);
        }

        [Fact]
        public void InsertAfter_Condition_LastElement_ShouldAppendToEnd()
        {
            // Arrange
            var list = new List<int> { 1, 2 };

            // Act
            list.InsertAfter(x => x == 2, 99);

            // Assert
            Assert.Equal(new int[] { 1, 2, 99 }, list);
        }

        [Fact]
        public void InsertAfter_Condition_NoMatch_ShouldNotInsert()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act
            list.InsertAfter(x => x == 99, 0);

            // Assert
            Assert.Equal(new int[] { 1, 2, 3 }, list);
        }

        [Fact]
        public void InsertAfter_Condition_NullList_ShouldThrow()
        {
            // Arrange
            IList<int> list = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => list.InsertAfter(x => true, 0));
        }

        [Fact]
        public void InsertAfter_Index_ShouldInsertAfterIndex()
        {
            // Arrange
            var list = new List<int> { 10, 20, 30 };

            // Act
            list.InsertAfter(1, 99);

            // Assert
            Assert.Equal(new int[] { 10, 20, 99, 30 }, list);
        }

        [Fact]
        public void InsertAfter_Index_LastIndex_ShouldAppend()
        {
            // Arrange
            var list = new List<int> { 10, 20 };

            // Act
            list.InsertAfter(1, 99);

            // Assert
            Assert.Equal(new int[] { 10, 20, 99 }, list);
        }

        [Fact]
        public void InsertAfter_Index_OutOfRange_ShouldNotInsert()
        {
            // Arrange
            var list = new List<int> { 10, 20 };

            // Act
            list.InsertAfter(99, 0);

            // Assert
            Assert.Equal(new int[] { 10, 20 }, list);
        }

        [Fact]
        public void InsertAfter_Index_NullList_ShouldThrow()
        {
            // Arrange
            IList<int> list = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => list.InsertAfter(0, 1));
        }

        // ============================================================
        // ToHashSet
        // ============================================================

        [Fact]
        public void ToHashSet_ShouldConvertAndDeduplicate()
        {
            // Arrange
            var source = new[] { 1, 2, 2, 3, 3, 3 };

            // Act
            var result = source.ToHashSet<int, int>(x => x);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }

        [Fact]
        public void ToHashSet_WithSelector_ShouldTransformElements()
        {
            // Arrange
            var source = new[] { "a", "bb", "ccc" };

            // Act
            var result = source.ToHashSet(s => s.Length);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Contains(1, result);
            Assert.Contains(2, result);
            Assert.Contains(3, result);
        }

        [Fact]
        public void ToHashSet_DuplicateKeys_ShouldDeduplicate()
        {
            // Arrange
            var source = new[] { "a", "b", "ab" };

            // Act
            var result = source.ToHashSet(s => s.Length);

            // Assert
            Assert.Equal(2, result.Count); // lengths: 1, 1, 2 -> deduplicated
        }

        [Fact]
        public void ToHashSet_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<int> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.ToHashSet<int, int>(x => x));
        }

        [Fact]
        public void ToHashSet_NullSelector_ShouldThrow()
        {
            // Arrange
            var source = new[] { 1 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.ToHashSet<int, int>(null));
        }

        // ============================================================
        // ForeachAsync
        // ============================================================

        [Fact]
        public async Task ForeachAsync_ShouldProcessAllElements()
        {
            // Arrange
            var source = new[] { 1, 2, 3 };
            var results = new List<int>();

            // Act
            await source.ForeachAsync(async x =>
            {
                await Task.Delay(1);
                lock (results)
                {
                    results.Add(x);
                }
            });

            // Assert
            Assert.Equal(3, results.Count);
            Assert.All(source, item => Assert.Contains(item, results));
        }

        [Fact]
        public async Task ForeachAsync_WithMaxParallel_ShouldProcessAllElements()
        {
            // Arrange
            var source = new[] { 1, 2, 3, 4, 5 };
            var processed = new List<int>();

            // Act
            await source.ForeachAsync(async x =>
            {
                await Task.Delay(1);
                lock (processed)
                {
                    processed.Add(x);
                }
            }, 2);

            // Assert
            Assert.Equal(5, processed.Count);
        }

        [Fact]
        public async Task ForeachAsync_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<int> source = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                source.ForeachAsync(x => Task.CompletedTask));
        }

        [Fact]
        public async Task ForeachAsync_NullAction_ShouldThrow()
        {
            // Arrange
            var source = new[] { 1 };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                source.ForeachAsync(null));
        }

        // ============================================================
        // SelectAsync with maxParallelCount
        // ============================================================

        [Fact]
        public async Task SelectAsync_WithMaxParallel_ShouldReturnAllResults()
        {
            // Arrange
            var source = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = await source.SelectAsync(async x =>
            {
                await Task.Delay(1);
                return x * 2;
            }, 2);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.All(result, item => Assert.True(item % 2 == 0));
        }

        [Fact]
        public async Task SelectAsync_WithIndexAndMaxParallel_ShouldReturnAllResults()
        {
            // Arrange
            var source = new[] { 10, 20, 30 };

            // Act
            var result = await source.SelectAsync(async (x, index) =>
            {
                await Task.Delay(1);
                return x + index;
            }, 2);

            // Assert
            Assert.Contains(10, result);
            Assert.Contains(21, result);
            Assert.Contains(32, result);
        }

        // ============================================================
        // ForAsync
        // ============================================================

        [Fact]
        public async Task ForAsync_ShouldProcessAllElements()
        {
            // Arrange
            var source = new[] { 1, 2, 3 };
            var sum = 0;

            // Act
            await source.ForAsync(async (item, index) =>
            {
                await Task.Delay(1);
                Interlocked.Add(ref sum, item);
            });

            // Assert
            Assert.Equal(6, sum);
        }

        [Fact]
        public async Task ForAsync_WithMaxParallel_ShouldProcessAllElements()
        {
            // Arrange
            var source = new[] { 1, 2, 3, 4, 5 };
            var count = 0;

            // Act
            await source.ForAsync(async (item, index) =>
            {
                await Task.Delay(1);
                Interlocked.Increment(ref count);
            }, 2);

            // Assert
            Assert.Equal(5, count);
        }

        [Fact]
        public async Task ForAsync_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<int> source = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                source.ForAsync((item, index) => Task.CompletedTask));
        }

        [Fact]
        public async Task ForAsync_NullSelector_ShouldThrow()
        {
            // Arrange
            var source = new[] { 1 };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                source.ForAsync(null));
        }

        // ============================================================
        // CompareChangesPlus
        // ============================================================

        [Fact]
        public void CompareChangesPlus_ShouldReturnCorrectChanges()
        {
            // Arrange
            var first = new[] { 1, 2, 3, 4 };
            var second = new[] { 2, 3, 5, 6 };

            // Act
            var (adds, removes, updates) = first.CompareChangesPlus(second, (x, y) => x == y);

            // Assert
            Assert.Equal(new int[] { 1, 4 }, adds);
            Assert.Equal(new int[] { 5, 6 }, removes);
            Assert.Equal(2, updates.Count);
            Assert.Contains((2, 2), updates);
            Assert.Contains((3, 3), updates);
        }

        [Fact]
        public void CompareChangesPlus_WithNullFirst_ShouldTreatAsEmpty()
        {
            // Arrange
            IEnumerable<int> first = null;
            var second = new[] { 1, 2 };

            // Act
            var (adds, removes, updates) = first.CompareChangesPlus(second, (x, y) => x == y);

            // Assert
            Assert.Empty(adds);
            Assert.Equal(new int[] { 1, 2 }, removes);
            Assert.Empty(updates);
        }

        [Fact]
        public void CompareChangesPlus_WithNullSecond_ShouldTreatAsEmpty()
        {
            // Arrange
            var first = new[] { 1, 2 };
            IEnumerable<int> second = null;

            // Act
            var (adds, removes, updates) = first.CompareChangesPlus(second, (x, y) => x == y);

            // Assert
            Assert.Equal(new int[] { 1, 2 }, adds);
            Assert.Empty(removes);
            Assert.Empty(updates);
        }

        [Fact]
        public void CompareChangesPlus_NullCondition_ShouldThrow()
        {
            // Arrange
            var first = new[] { 1 };
            var second = new[] { 2 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                first.CompareChangesPlus<int, int>(second, null));
        }

        [Fact]
        public void CompareChanges_WithNullFirst_ShouldTreatAsEmpty()
        {
            // Arrange
            IEnumerable<int> first = null;
            var second = new[] { 1 };

            // Act
            var (adds, removes, updates) = first.CompareChanges(second, (x, y) => x == y);

            // Assert
            Assert.Empty(adds);
            Assert.Equal(new int[] { 1 }, removes);
            Assert.Empty(updates);
        }

        // ============================================================
        // AsNotNull (IEnumerable overload)
        // ============================================================

        [Fact]
        public void AsNotNull_IEnumerable_Null_ShouldReturnEmpty()
        {
            // Arrange
            IEnumerable<int> source = null;

            // Act
            var result = source.AsNotNull();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void AsNotNull_IEnumerable_NonNull_ShouldReturnSame()
        {
            // Arrange
            IEnumerable<int> source = new List<int> { 1, 2 };

            // Act
            var result = source.AsNotNull();

            // Assert
            Assert.Same(source, result);
        }

        // ============================================================
        // WhereIf (Func<bool> overload)
        // ============================================================

        [Fact]
        public void WhereIf_FuncBool_True_ShouldApplyFilter()
        {
            // Arrange
            var source = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = source.WhereIf(() => true, x => x > 3).ToList();

            // Assert
            Assert.Equal(new int[] { 4, 5 }, result);
        }

        [Fact]
        public void WhereIf_FuncBool_False_ShouldNotApplyFilter()
        {
            // Arrange
            var source = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = source.WhereIf(() => false, x => x > 3).ToList();

            // Assert
            Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, result);
        }

        [Fact]
        public void WhereIf_FuncBool_NullCondition_ShouldThrow()
        {
            // Arrange
            var source = new[] { 1 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                source.WhereIf(null, x => true));
        }

        // ============================================================
        // WhereIf (IQueryable overloads)
        // ============================================================

        [Fact]
        public void WhereIf_IQueryable_BoolTrue_ShouldApplyFilter()
        {
            // Arrange
            IQueryable<int> source = new[] { 1, 2, 3, 4, 5 }.AsQueryable();

            // Act
            var result = source.WhereIf(true, x => x > 3).ToList();

            // Assert
            Assert.Equal(new int[] { 4, 5 }, result);
        }

        [Fact]
        public void WhereIf_IQueryable_BoolFalse_ShouldNotApplyFilter()
        {
            // Arrange
            IQueryable<int> source = new[] { 1, 2, 3 }.AsQueryable();

            // Act
            var result = source.WhereIf(false, x => x > 10).ToList();

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void WhereIf_IQueryable_FuncBool_ShouldWork()
        {
            // Arrange
            IQueryable<int> source = new[] { 1, 2, 3, 4 }.AsQueryable();

            // Act
            var result = source.WhereIf(() => true, x => x % 2 == 0).ToList();

            // Assert
            Assert.Equal(new int[] { 2, 4 }, result);
        }

        [Fact]
        public void WhereIf_IQueryable_NullSource_ShouldThrow()
        {
            // Arrange
            IQueryable<int> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                source.WhereIf(true, x => x > 0));
        }

        // ============================================================
        // ChangeIndex 边界
        // ============================================================

        [Fact]
        public void ChangeIndex_NullList_ShouldThrow()
        {
            // Arrange
            IList<int> list = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => list.ChangeIndex(1, 0));
        }

        [Fact]
        public void ChangeIndex_NullCondition_ShouldThrow()
        {
            // Arrange
            var list = new List<int> { 1, 2 };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => list.ChangeIndex(null, 0));
        }

        [Fact]
        public void ChangeIndex_WithCondition_NoMatch_ShouldReturnUnchanged()
        {
            // 使用引用类型 string，使 FirstOrDefault 在无匹配时返回 null，
            // 从而 ChangeIndex 内部的 item != null 检查为 false，不执行移动操作。
            // Arrange
            var list = new List<string> { "1", "2", "3" };

            // Act
            var result = list.ChangeIndex(x => x == "99", 0);

            // Assert
            Assert.Same(list, result);
            Assert.Equal(new string[] { "1", "2", "3" }, result);
        }

        [Fact]
        public void ChangeIndex_IndexOutOfRange_ShouldClampToValidRange()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act - ChangeIndex 第一个参数是元素值而非索引，移动值 1 到末尾
            list.ChangeIndex(1, 999);

            // Assert - item 1 moved to end
            Assert.Equal(new int[] { 2, 3, 1 }, list);
        }

        [Fact]
        public void ChangeIndex_NegativeIndex_ShouldClampToZero()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };

            // Act - ChangeIndex 第一个参数是元素值而非索引，移动值 3 到开头
            list.ChangeIndex(3, -5);

            // Assert - item 3 moved to beginning
            Assert.Equal(new int[] { 3, 1, 2 }, list);
        }

        // ============================================================
        // MaxOrDefaultValue / MinOrDefaultValue IQueryable 变体
        // ============================================================

        [Fact]
        public void MaxOrDefaultValue_IQueryable_Empty_ShouldReturnDefault()
        {
            // Arrange
            IQueryable<int> source = new int[0].AsQueryable();

            // Act
            var result = source.MaxOrDefaultValue();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void MaxOrDefaultValue_IQueryable_WithSelector_ShouldReturnMax()
        {
            // Arrange
            IQueryable<string> source = new[] { "a", "abc", "ab" }.AsQueryable();

            // Act
            var result = source.MaxOrDefaultValue(s => s.Length);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void MinOrDefaultValue_IQueryable_Empty_ShouldReturnDefault()
        {
            // Arrange
            IQueryable<int> source = new int[0].AsQueryable();

            // Act
            var result = source.MinOrDefaultValue();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void MinOrDefaultValue_IQueryable_WithSelector_ShouldReturnMin()
        {
            // Arrange
            IQueryable<string> source = new[] { "abc", "a", "ab" }.AsQueryable();

            // Act
            var result = source.MinOrDefaultValue(s => s.Length);

            // Assert
            Assert.Equal(1, result);
        }

        // ============================================================
        // StandardDeviation 边界
        // ============================================================

        [Fact]
        public void StandardDeviation_EmptyCollection_ShouldReturnZero()
        {
            // Arrange
            var source = new double[0];

            // Act
            var result = source.StandardDeviation();

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void StandardDeviation_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<double> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.StandardDeviation());
        }

        // ============================================================
        // OrderByRandom 边界
        // ============================================================

        [Fact]
        public void OrderByRandom_EmptySource_ShouldReturnEmpty()
        {
            // Arrange
            var source = new int[0];

            // Act
            var result = source.OrderByRandom().ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void OrderByRandom_NullSource_ShouldThrow()
        {
            // Arrange
            IEnumerable<int> source = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => source.OrderByRandom());
        }
    }
}
