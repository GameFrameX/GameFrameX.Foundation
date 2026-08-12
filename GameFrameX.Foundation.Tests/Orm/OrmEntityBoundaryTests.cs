using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GameFrameX.Foundation.Orm.Entity;
using GameFrameX.Foundation.Orm.Entity.Filter;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    /// <summary>
    /// Orm.Entity 模块边界测试。
    /// 覆盖范围：
    /// 1) EntityContractValidator — AmbiguousMatchException 多级继承遮蔽分支
    /// 2) IsTypeCompatible — Nullable/协变/逆变 类型兼容性 Theory
    /// 3) IHasDomainEvents — 接口契约 + null guard + 并发线程安全（参考健壮实现）
    /// 4) VersionedEntityHelper.IncrementRowVersion — long.MaxValue 溢出行为
    /// 源码缺陷对应的测试按期望健壮行为编写，缺陷条目记录于任务报告。
    /// </summary>
    public class OrmEntityBoundaryTests
    {
        // ============================================================
        // 工作项 1: EntityContractValidator — AmbiguousMatchException 分支
        // ------------------------------------------------------------
        // 现有 EntityContractValidatorComprehensiveTests.ValidateType_ShadowedProperty_*
        // 已覆盖单层 new 遮蔽（EntityWithShadowedProperty）。
        // 此处补充多级继承遮蔽：MidShadowEntity 层 + DeeplyShadowedEntity 层
        // 均对 RowVersion 进行 new 遮蔽，进一步验证 GetProperty 在三层同名属性下
        // 触发 AmbiguousMatchException 并记录"多个声明"违反。
        // ============================================================

        [Fact]
        public void ValidateType_DeeplyShadowedProperty_ShouldReportMultipleDeclarations()
        {
            var violations = EntityContractValidator.Validate(typeof(DeeplyShadowedEntity));

            Assert.NotEmpty(violations);
            Assert.Contains(violations, v => v.PropertyName == nameof(IVersionedEntity.RowVersion));
            var rowVersionViolation = violations.First(v => v.PropertyName == nameof(IVersionedEntity.RowVersion));
            Assert.Equal(typeof(IVersionedEntity), rowVersionViolation.InterfaceType);
            Assert.Contains("多个声明", rowVersionViolation.Reason);
            Assert.Contains("multiple declarations", rowVersionViolation.Reason);
            Assert.Contains("遮蔽", rowVersionViolation.Reason);
        }

        [Fact]
        public void ValidateType_DeeplyShadowedProperty_ReasonShouldReferenceValidatedTypeName()
        {
            var violations = EntityContractValidator.Validate(typeof(DeeplyShadowedEntity));
            var rowVersionViolation = violations.First(v => v.PropertyName == nameof(IVersionedEntity.RowVersion));

            Assert.Contains(nameof(DeeplyShadowedEntity), rowVersionViolation.Reason);
        }

        [Fact]
        public void ValidateType_DeeplyShadowedProperty_ShouldReportExactlyOneRowVersionViolation()
        {
            // AmbiguousMatchException 触发后 continue，同一属性名不应产生多条违反
            var violations = EntityContractValidator.Validate(typeof(DeeplyShadowedEntity));
            var rowVersionViolations = violations.Where(v => v.PropertyName == nameof(IVersionedEntity.RowVersion)).ToList();

            Assert.Single(rowVersionViolations);
        }

        [Fact]
        public void ValidateType_MidLevelShadow_ShouldAlsoReportMultipleDeclarations()
        {
            // 中间层（单层遮蔽）也应触发 AmbiguousMatchException
            var violations = EntityContractValidator.Validate(typeof(MidShadowEntity));

            Assert.NotEmpty(violations);
            Assert.Contains(violations, v => v.PropertyName == nameof(IVersionedEntity.RowVersion));
            var violation = violations.First(v => v.PropertyName == nameof(IVersionedEntity.RowVersion));
            Assert.Contains("多个声明", violation.Reason);
        }

        // ============================================================
        // 工作项 2: IsTypeCompatible — 类型兼容性 Theory
        // ------------------------------------------------------------
        // IsTypeCompatible 是 EntityContractValidator 的 private 方法（源码 148-164 行），
        // 通过反射调用。兼容规则：
        //   1) expected == actual → true
        //   2) Nullable.GetUnderlyingType 提取后比较 → true（双向兼容）
        //   3) expected.IsAssignableFrom(actual) → 协变判定
        // ============================================================

        private static bool InvokeIsTypeCompatible(Type expected, Type actual)
        {
            var method = typeof(EntityContractValidator).GetMethod(
                "IsTypeCompatible",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);
            return (bool)method!.Invoke(null, new object[] { expected, actual })!;
        }

        [Theory]
        // 相同类型直接兼容
        [InlineData(typeof(long), typeof(long), true)]
        [InlineData(typeof(long?), typeof(long?), true)]
        [InlineData(typeof(int), typeof(int), true)]
        [InlineData(typeof(int?), typeof(int?), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(object), typeof(object), true)]
        // Nullable 双向与非 Nullable 兼容（Nullable.GetUnderlyingType 路径）
        [InlineData(typeof(long?), typeof(long), true)]
        [InlineData(typeof(long), typeof(long?), true)]
        [InlineData(typeof(int?), typeof(int), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(bool?), typeof(bool), true)]
        [InlineData(typeof(bool), typeof(bool?), true)]
        // 协变兼容（expected 是 actual 的父类型）
        [InlineData(typeof(object), typeof(int), true)]
        [InlineData(typeof(object), typeof(string), true)]
        [InlineData(typeof(object), typeof(long?), true)]
        [InlineData(typeof(ValueType), typeof(int), true)]
        // 不兼容场景
        [InlineData(typeof(int), typeof(object), false)]
        [InlineData(typeof(long), typeof(int), false)]
        [InlineData(typeof(long?), typeof(int?), false)]
        [InlineData(typeof(int?), typeof(long?), false)]
        [InlineData(typeof(string), typeof(int), false)]
        [InlineData(typeof(bool), typeof(int), false)]
        [InlineData(typeof(Guid), typeof(string), false)]
        public void IsTypeCompatible_VariousTypeCombinations(Type expected, Type actual, bool expectedResult)
        {
            var result = InvokeIsTypeCompatible(expected, actual);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsTypeCompatible_SameReferenceType_ShouldReturnTrue()
        {
            Assert.True(InvokeIsTypeCompatible(typeof(EntityBase), typeof(EntityBase)));
        }

        [Fact]
        public void IsTypeCompatible_NullableSymmetric_ForPrimitiveTypes()
        {
            // 对称性：T? ↔ T 双向兼容（对原始数值类型）
            Assert.True(InvokeIsTypeCompatible(typeof(long?), typeof(long)));
            Assert.True(InvokeIsTypeCompatible(typeof(long), typeof(long?)));
            Assert.True(InvokeIsTypeCompatible(typeof(int?), typeof(int)));
            Assert.True(InvokeIsTypeCompatible(typeof(int), typeof(int?)));
        }

        [Fact]
        public void IsTypeCompatible_Covariant_ForSubclassToBase()
        {
            // 子类 → 父类 兼容；父类 → 子类 不兼容
            Assert.True(InvokeIsTypeCompatible(typeof(object), typeof(string)));
            Assert.False(InvokeIsTypeCompatible(typeof(string), typeof(object)));
        }

        [Fact]
        public void IsTypeCompatible_DifferentNullableUnderlyingTypes_ShouldBeIncompatible()
        {
            // long? 与 int? 底层类型不同，且无继承关系，应不兼容
            Assert.False(InvokeIsTypeCompatible(typeof(long?), typeof(int?)));
            Assert.False(InvokeIsTypeCompatible(typeof(int?), typeof(long?)));
        }

        // ============================================================
        // 工作项 3: IHasDomainEvents — 接口契约 + 领域事件健壮性
        // ------------------------------------------------------------
        // 源码事实：GameFrameX.Foundation.Orm.Entity 仅定义 IHasDomainEvents 接口，
        // 未提供任何默认实现（无 AggregateRoot 基类）。
        // 接口仅包含 AddDomainEvent / ClearDomainEvents / DomainEvents，
        // 缺少 RemoveDomainEvent（详见报告）。
        //
        // 以下测试：
        //   a) 接口契约验证（成员存在性、签名、返回类型）
        //   b) 参考健壮实现 RobustDomainEventEntity 的期望行为验证
        //      （null guard 抛 ArgumentNullException + ConcurrentQueue 线程安全）
        // ============================================================

        [Fact]
        public void IHasDomainEvents_Interface_ShouldDeclareContractMembers()
        {
            var type = typeof(IHasDomainEvents);

            Assert.True(type.IsInterface);

            var domainEventsProperty = type.GetProperty(nameof(IHasDomainEvents.DomainEvents));
            Assert.NotNull(domainEventsProperty);
            Assert.Equal(typeof(IReadOnlyList<IDomainEvent>), domainEventsProperty!.PropertyType);
            Assert.True(domainEventsProperty.CanRead);
            Assert.False(domainEventsProperty.CanWrite);

            var addMethod = type.GetMethod(nameof(IHasDomainEvents.AddDomainEvent));
            Assert.NotNull(addMethod);
            Assert.Equal(typeof(void), addMethod!.ReturnType);
            var addParam = Assert.Single(addMethod.GetParameters());
            Assert.Equal(typeof(IDomainEvent), addParam.ParameterType);

            var clearMethod = type.GetMethod(nameof(IHasDomainEvents.ClearDomainEvents));
            Assert.NotNull(clearMethod);
            Assert.Equal(typeof(void), clearMethod!.ReturnType);
            Assert.Empty(clearMethod.GetParameters());
        }

        [Fact]
        public void IHasDomainEvents_Interface_ShouldNotDeclareRemoveDomainEvent()
        {
            // 当前接口未定义 RemoveDomainEvent — 记录为接口设计缺失，见报告
            var removeMethod = typeof(IHasDomainEvents).GetMethod("RemoveDomainEvent");

            Assert.Null(removeMethod);
        }

        [Fact]
        public void IDomainEvent_ShouldBeEmptyMarkerInterface()
        {
            var type = typeof(IDomainEvent);

            Assert.True(type.IsInterface);
            Assert.Empty(type.GetProperties());
            Assert.Empty(type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
        }

        // --- 参考健壮实现：RobustDomainEventEntity 的期望行为验证 ---

        [Fact]
        public void AddDomainEvent_WithNull_ShouldThrowArgumentNullException()
        {
            // 期望行为：AddDomainEvent(null) 应抛 ArgumentNullException
            var entity = new RobustDomainEventEntity();

            var ex = Assert.Throws<ArgumentNullException>(() => entity.AddDomainEvent(null!));

            Assert.Equal("domainEvent", ex.ParamName);
        }

        [Fact]
        public void AddDomainEvent_WithValidEvent_ShouldAppendToDomainEvents()
        {
            var entity = new RobustDomainEventEntity();
            var evt = new TestBoundaryDomainEvent();

            entity.AddDomainEvent(evt);

            var events = entity.DomainEvents;
            Assert.Single(events);
            Assert.Same(evt, events[0]);
        }

        [Fact]
        public void AddDomainEvent_MultipleEvents_ShouldPreserveInsertionOrder()
        {
            var entity = new RobustDomainEventEntity();
            var evt1 = new TestBoundaryDomainEvent();
            var evt2 = new TestBoundaryDomainEvent();
            var evt3 = new TestBoundaryDomainEvent();

            entity.AddDomainEvent(evt1);
            entity.AddDomainEvent(evt2);
            entity.AddDomainEvent(evt3);

            Assert.Equal(3, entity.DomainEvents.Count);
            Assert.Same(evt1, entity.DomainEvents[0]);
            Assert.Same(evt2, entity.DomainEvents[1]);
            Assert.Same(evt3, entity.DomainEvents[2]);
        }

        [Fact]
        public void ClearDomainEvents_AfterAdds_ShouldEmptyCollection()
        {
            var entity = new RobustDomainEventEntity();
            entity.AddDomainEvent(new TestBoundaryDomainEvent());
            entity.AddDomainEvent(new TestBoundaryDomainEvent());

            entity.ClearDomainEvents();

            Assert.Empty(entity.DomainEvents);
        }

        [Fact]
        public void ClearDomainEvents_OnEmptyEntity_ShouldRemainEmpty()
        {
            var entity = new RobustDomainEventEntity();

            entity.ClearDomainEvents();

            Assert.Empty(entity.DomainEvents);
        }

        [Fact]
        public void ClearDomainEvents_ThenAddAgain_ShouldContainOnlyNewEvents()
        {
            var entity = new RobustDomainEventEntity();
            entity.AddDomainEvent(new TestBoundaryDomainEvent());
            entity.ClearDomainEvents();

            var evt = new TestBoundaryDomainEvent();
            entity.AddDomainEvent(evt);

            Assert.Single(entity.DomainEvents);
            Assert.Same(evt, entity.DomainEvents[0]);
        }

        [Fact]
        public void DomainEvents_AfterConstruction_ShouldBeEmpty()
        {
            var entity = new RobustDomainEventEntity();

            Assert.Empty(entity.DomainEvents);
        }

        [Fact]
        public void DomainEvents_ShouldReturnReadOnlyListInstance()
        {
            var entity = new RobustDomainEventEntity();
            entity.AddDomainEvent(new TestBoundaryDomainEvent());

            Assert.IsAssignableFrom<IReadOnlyList<IDomainEvent>>(entity.DomainEvents);
        }

        // --- 并发线程安全测试 ---

        [Fact]
        public void AddDomainEvent_ConcurrentAdds_ShouldNotLoseEvents()
        {
            // 期望行为：多线程并发 AddDomainEvent 应线程安全，不丢事件、不抛异常
            var entity = new RobustDomainEventEntity();
            const int concurrency = 16;
            const int perThread = 500;
            var totalExpected = concurrency * perThread;

            Parallel.For(0, concurrency, threadId =>
            {
                for (var i = 0; i < perThread; i++)
                {
                    entity.AddDomainEvent(new TestBoundaryDomainEvent());
                }
            });

            Assert.Equal(totalExpected, entity.DomainEvents.Count);
        }

        [Fact]
        public void AddDomainEvent_ConcurrentAddsAndClear_ShouldNotCorruptState()
        {
            // 并发 Add + Clear 不应导致崩溃或状态损坏
            var entity = new RobustDomainEventEntity();
            Exception capturedException = null;
            var tasks = new List<Task>();

            tasks.Add(Task.Run(() =>
            {
                for (var i = 0; i < 2000; i++)
                {
                    entity.AddDomainEvent(new TestBoundaryDomainEvent());
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                for (var i = 0; i < 2000; i++)
                {
                    entity.AddDomainEvent(new TestBoundaryDomainEvent());
                }
            }));

            tasks.Add(Task.Run(() =>
            {
                for (var i = 0; i < 50; i++)
                {
                    entity.ClearDomainEvents();
                }
            }));

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ex)
            {
                capturedException = ex;
            }

            Assert.Null(capturedException);
            // 最终状态可读，不抛异常
            _ = entity.DomainEvents.Count;
        }

        // ============================================================
        // 工作项 4: VersionedEntityHelper.IncrementRowVersion 溢出行为
        // ------------------------------------------------------------
        // 源码（VersionedEntityHelper.cs 54-58 行）：
        //   entity.RowVersion = (entity.RowVersion ?? 0) + 1;
        // 默认 unchecked 上下文 → long.MaxValue + 1 静默回绕到 long.MinValue。
        // 期望健壮行为：抛 OverflowException。
        //
        // 期望行为测试（Should*Throw*）会 FAIL，记录于源码缺陷清单。
        // 当前行为测试（CurrentBehavior*）验证实际回绕，用于对比文档化。
        // ============================================================

        [Fact]
        public void IncrementRowVersion_AtLongMaxValue_ShouldThrowOverflowException()
        {
            // 期望行为：溢出应抛 OverflowException，而非静默回绕
            // 【源码缺陷】当前实现不抛异常，静默回绕到 long.MinValue
            var entity = new BoundaryTestEntity();
            entity.RowVersion = long.MaxValue;

            Assert.Throws<OverflowException>(() => entity.IncrementRowVersion());
        }


        [Fact]
        public void IncrementRowVersion_AtNearMaxValue_ShouldReachMaxValue()
        {
            // long.MaxValue - 1 递增到 long.MaxValue 不溢出
            var entity = new BoundaryTestEntity();
            entity.RowVersion = long.MaxValue - 1;

            var newVersion = entity.IncrementRowVersion();

            Assert.Equal(long.MaxValue, newVersion);
            Assert.Equal(long.MaxValue, entity.RowVersion);
        }

        [Fact]
        public void IncrementRowVersion_AtNearMaxValue_ThenOverflow_ShouldThrow()
        {
            // 递增到 long.MaxValue 后再递增应抛 OverflowException
            // 【源码缺陷】当前实现回绕到 long.MinValue
            var entity = new BoundaryTestEntity();
            entity.RowVersion = long.MaxValue - 1;

            entity.IncrementRowVersion();

            Assert.Throws<OverflowException>(() => entity.IncrementRowVersion());
        }

        [Fact]
        public void IncrementRowVersion_AtZero_ShouldReturnOne()
        {
            var entity = new BoundaryTestEntity();
            entity.RowVersion = 0;

            var newVersion = entity.IncrementRowVersion();

            Assert.Equal(1, newVersion);
        }

        [Fact]
        public void IncrementRowVersion_AtNegativeValue_ShouldIncrement()
        {
            // 负值边界：合法 long 值应正常递增
            var entity = new BoundaryTestEntity();
            entity.RowVersion = -1L;

            var newVersion = entity.IncrementRowVersion();

            Assert.Equal(0, newVersion);
        }

        [Fact]
        public void HasRowVersionConflict_AtLongMaxValue_ShouldDetectCorrectly()
        {
            // 边界：long.MaxValue 作为行版本号
            var entity = new BoundaryTestEntity();
            entity.RowVersion = long.MaxValue;

            Assert.False(entity.HasRowVersionConflict(long.MaxValue));
            Assert.True(entity.HasRowVersionConflict(long.MaxValue - 1));
        }

        [Fact]
        public void EnsureNoRowVersionConflict_AtLongMaxValue_ShouldThrowWhenMismatched()
        {
            var entity = new BoundaryTestEntity();
            entity.RowVersion = long.MaxValue;

            Assert.Throws<InvalidOperationException>(() => entity.EnsureNoRowVersionConflict(long.MaxValue - 1));
        }

        [Fact]
        public void EnsureNoRowVersionConflict_AtLongMaxValue_ShouldNotThrowWhenMatched()
        {
            var entity = new BoundaryTestEntity();
            entity.RowVersion = long.MaxValue;

            var ex = Record.Exception(() => entity.EnsureNoRowVersionConflict(long.MaxValue));

            Assert.Null(ex);
        }

        // ============================================================
        // 测试辅助类型
        // ============================================================

        /// <summary>
        /// 中间层遮蔽：将 RowVersion（long?）用 new 遮蔽为 int?。
        /// </summary>
        private class MidShadowEntity : EntityBase
        {
            public new int? RowVersion { get; set; }
        }

        /// <summary>
        /// 叶子层遮蔽：在 MidShadowEntity 基础上再次用 new 遮蔽为 string?。
        /// 多级继承层次中有 3 个同名 public 属性 → AmbiguousMatchException。
        /// </summary>
        private sealed class DeeplyShadowedEntity : MidShadowEntity
        {
            public new string? RowVersion { get; set; }
        }

        /// <summary>
        /// 参考健壮实现：使用 ConcurrentQueue + null guard。
        /// 展示 IHasDomainEvents 接口期望的实现行为：
        ///   - AddDomainEvent(null) 抛 ArgumentNullException
        ///   - 并发 AddDomainEvent 线程安全，不丢事件
        /// 库本身缺少此类默认实现（见报告源码缺陷清单）。
        /// </summary>
        private sealed class RobustDomainEventEntity : IHasDomainEvents
        {
            private readonly ConcurrentQueue<IDomainEvent> _events = new ConcurrentQueue<IDomainEvent>();

            public IReadOnlyList<IDomainEvent> DomainEvents => _events.ToArray();

            public void AddDomainEvent(IDomainEvent domainEvent)
            {
                if (domainEvent == null)
                {
                    throw new ArgumentNullException(nameof(domainEvent));
                }

                _events.Enqueue(domainEvent);
            }

            public void ClearDomainEvents()
            {
                while (_events.TryDequeue(out _))
                {
                }
            }
        }

        private sealed class TestBoundaryDomainEvent : IDomainEvent
        {
        }

        private sealed class BoundaryTestEntity : EntityBase
        {
        }
    }
}
