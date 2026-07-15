using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GameFrameX.Foundation.Orm.Entity;
using GameFrameX.Foundation.Orm.Entity.Filter;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    public class OrmEntityTests
    {
        [Fact]
        public void EntityBase_ShouldExposeDocumentedDefaultStates()
        {
            var entity = new TestEntity();

            Assert.Equal(0, entity.Id);
            Assert.False(entity.IsDeleted);
            Assert.Equal(0, entity.RowVersion);
            Assert.Null(entity.IsEnabled);
        }

        [Fact]
        public void EntityBaseId_ShouldExposeKeyAndRequiredMetadata()
        {
            var property = typeof(EntityBaseId).GetProperty(nameof(EntityBaseId.Id));

            Assert.NotNull(property);
            Assert.Contains(property!.GetCustomAttributes(false), attribute => attribute is KeyAttribute);
            Assert.Contains(property.GetCustomAttributes(false), attribute => attribute is RequiredAttribute);
        }

        #region 领域事件接口测试

        [Fact]
        public void IHasDomainEvents_ShouldAddAndClearEvents()
        {
            var entity = new TestDomainEventEntity();
            var evt1 = new TestDomainEvent();
            var evt2 = new TestDomainEvent();

            Assert.Empty(entity.DomainEvents);

            entity.AddDomainEvent(evt1);
            entity.AddDomainEvent(evt2);
            Assert.Equal(2, entity.DomainEvents.Count);
            Assert.Same(evt1, entity.DomainEvents[0]);
            Assert.Same(evt2, entity.DomainEvents[1]);

            entity.ClearDomainEvents();
            Assert.Empty(entity.DomainEvents);
        }

        [Fact]
        public void IDomainEvent_ShouldBeMarkerInterface()
        {
            Assert.True(typeof(IDomainEvent).IsInterface);
            Assert.Empty(typeof(IDomainEvent).GetProperties());
            Assert.Empty(typeof(IDomainEvent).GetMethods());
        }

        #endregion

        #region 版本并发辅助方法测试

        [Fact]
        public void IncrementRowVersion_ShouldIncrementFromNull()
        {
            var entity = new TestEntity();
            entity.RowVersion = null;

            var newRowVersion = entity.IncrementRowVersion();

            Assert.Equal(1, newRowVersion);
            Assert.Equal(1, entity.RowVersion);
        }

        [Fact]
        public void IncrementRowVersion_ShouldIncrementFromZero()
        {
            var entity = new TestEntity();

            var newRowVersion = entity.IncrementRowVersion();

            Assert.Equal(1, newRowVersion);
            Assert.Equal(1, entity.RowVersion);
        }

        [Fact]
        public void IncrementRowVersion_ShouldIncrementSequentially()
        {
            var entity = new TestEntity();

            entity.IncrementRowVersion();
            entity.IncrementRowVersion();
            var newRowVersion = entity.IncrementRowVersion();

            Assert.Equal(3, newRowVersion);
            Assert.Equal(3, entity.RowVersion);
        }

        [Fact]
        public void HasRowVersionConflict_ShouldReturnTrueWhenMismatched()
        {
            var entity = new TestEntity();
            entity.RowVersion = 5;

            Assert.True(entity.HasRowVersionConflict(3));
        }

        [Fact]
        public void HasRowVersionConflict_ShouldReturnFalseWhenMatched()
        {
            var entity = new TestEntity();
            entity.RowVersion = 5;

            Assert.False(entity.HasRowVersionConflict(5));
        }

        [Fact]
        public void EnsureNoRowVersionConflict_ShouldNotThrowWhenMatched()
        {
            var entity = new TestEntity();
            entity.RowVersion = 5;

            entity.EnsureNoRowVersionConflict(5);
        }

        [Fact]
        public void EnsureNoRowVersionConflict_ShouldThrowWhenMismatched()
        {
            var entity = new TestEntity();
            entity.RowVersion = 5;

            Assert.Throws<InvalidOperationException>(() => entity.EnsureNoRowVersionConflict(3));
        }

        #endregion

        #region Tenant/Organization 组合基类测试

        [Fact]
        public void EntityTenantOrganizationBase_ShouldHaveTenantIdAndOrganizationId()
        {
            var entity = new TestTenantOrgEntity();

            Assert.Null(entity.TenantId);
            Assert.Null(entity.CreateOrganizationId);
            Assert.Equal(0, entity.Id);
            Assert.False(entity.IsDeleted);
        }

        [Fact]
        public void EntityTenantOrganizationBase_ShouldImplementBothFilters()
        {
            var type = typeof(TestTenantOrgEntity);

            Assert.True(typeof(ITenantIdFilter).IsAssignableFrom(type));
            Assert.True(typeof(IOrganizationIdFilter).IsAssignableFrom(type));
            Assert.True(typeof(EntityTenantBase).IsAssignableFrom(type));
        }

        #endregion

        #region 实体契约验证器测试

        [Fact]
        public void EntityContractValidator_ValidEntity_ShouldReturnNoViolations()
        {
            var violations = EntityContractValidator.Validate<EntityBase>();

            Assert.Empty(violations);
        }

        [Fact]
        public void EntityContractValidator_EntityTenantBase_ShouldReturnNoViolations()
        {
            var violations = EntityContractValidator.Validate<EntityTenantBase>();

            Assert.Empty(violations);
        }

        [Fact]
        public void EntityContractValidator_EntityTenantOrganizationBase_ShouldReturnNoViolations()
        {
            var violations = EntityContractValidator.Validate<EntityTenantOrganizationBase>();

            Assert.Empty(violations);
        }

        [Fact]
        public void EntityContractValidator_NonEntityType_ShouldReturnNoViolations()
        {
            var violations = EntityContractValidator.Validate<string>();

            Assert.Empty(violations);
        }

        [Fact]
        public void EntityContractValidator_ShadowedProperty_ShouldReportTypeMismatch()
        {
            var violations = EntityContractValidator.Validate<BadEntityShadowedProperty>();

            Assert.NotEmpty(violations);
            Assert.Contains(violations, v => v.PropertyName == nameof(IVersionedEntity.RowVersion));
        }

        [Fact]
        public void EntityContractValidator_NullType_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => EntityContractValidator.Validate(null!));
        }

        [Fact]
        public void EntityContractValidator_GenericValidate_ShouldWork()
        {
            var violations = EntityContractValidator.Validate<EntityBase>();

            Assert.Empty(violations);
        }

        [Fact]
        public void EntityContractViolation_ShouldStoreCorrectValues()
        {
            var violation = new EntityContractViolation(
                typeof(IVersionedEntity),
                "RowVersion",
                "test reason");

            Assert.Equal(typeof(IVersionedEntity), violation.InterfaceType);
            Assert.Equal("RowVersion", violation.PropertyName);
            Assert.Equal("test reason", violation.Reason);
        }

        #endregion

        #region 测试辅助类型

        private sealed class TestEntity : EntityBase
        {
        }

        private sealed class TestDomainEvent : IDomainEvent
        {
        }

        private sealed class TestDomainEventEntity : IHasDomainEvents
        {
            private readonly List<IDomainEvent> _events = new List<IDomainEvent>();

            public IReadOnlyList<IDomainEvent> DomainEvents => _events;

            public void AddDomainEvent(IDomainEvent domainEvent)
            {
                _events.Add(domainEvent);
            }

            public void ClearDomainEvents()
            {
                _events.Clear();
            }
        }

        private sealed class TestTenantOrgEntity : EntityTenantOrganizationBase
        {
        }

        /// <summary>
        /// 使用 new 关键字遮蔽 RowVersion 属性为错误类型，用于测试契约验证器。
        /// </summary>
        private class BadEntityShadowedProperty : EntityBase
        {
            public new string? RowVersion { get; set; }
        }

        #endregion
    }
}