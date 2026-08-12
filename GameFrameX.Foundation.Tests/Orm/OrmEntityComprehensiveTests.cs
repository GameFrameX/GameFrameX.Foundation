using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GameFrameX.Foundation.Orm.Entity;
using GameFrameX.Foundation.Orm.Entity.Filter;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    /// <summary>
    /// EntityBase / EntityBaseId / EntityTenantBase / EntityTenantOrganizationBase 全量覆盖测试。
    /// 覆盖完整属性默认值、可写性、virtual、过滤接口实现、属性级 Attribute。
    /// </summary>
    public class OrmEntityComprehensiveTests
    {
        // ============================================================
        // EntityBaseId — 非泛型
        // ============================================================

        [Fact]
        public void EntityBaseId_DefaultId_ShouldBeZero()
        {
            var entity = new ConcreteEntityBaseId();

            Assert.Equal(0, entity.Id);
        }

        [Fact]
        public void EntityBaseId_Id_ShouldBeMutable()
        {
            var entity = new ConcreteEntityBaseId
            {
                Id = 42
            };

            Assert.Equal(42, entity.Id);
        }

        [Fact]
        public void EntityBaseId_Id_ShouldBeVirtual()
        {
            var property = typeof(EntityBaseId).GetProperty(nameof(EntityBaseId.Id));

            Assert.NotNull(property);
            Assert.True(property!.GetMethod != null && property.GetMethod.IsVirtual);
            Assert.True(property.SetMethod != null && property.SetMethod.IsVirtual);
        }

        [Fact]
        public void EntityBaseId_Id_ShouldHaveAllDocumentedAttributes()
        {
            var property = typeof(EntityBaseId).GetProperty(nameof(EntityBaseId.Id));

            Assert.NotNull(property);
            var attrs = property!.GetCustomAttributes(false);

            Assert.Contains(attrs, a => a is KeyAttribute);
            Assert.Contains(attrs, a => a is RequiredAttribute);
            var editable = Assert.Single(attrs, a => a is EditableAttribute) as EditableAttribute;
            Assert.NotNull(editable);
            Assert.False(editable!.AllowEdit);
            Assert.True(editable.AllowInitialValue);
            var desc = Assert.Single(attrs, a => a is System.ComponentModel.DescriptionAttribute) as System.ComponentModel.DescriptionAttribute;
            Assert.NotNull(desc);
            Assert.Equal("主键Id", desc!.Description);
        }

        [Fact]
        public void EntityBaseId_ShouldImplementIEntityLong()
        {
            Assert.True(typeof(IEntity<long>).IsAssignableFrom(typeof(EntityBaseId)));
            Assert.True(typeof(IEntity).IsAssignableFrom(typeof(EntityBaseId)));
        }

        // ============================================================
        // EntityBaseId<TKey> — 泛型
        // ============================================================

        [Fact]
        public void EntityBaseIdGeneric_DefaultId_ShouldBeDefault()
        {
            var entity = new ConcreteEntityBaseIdGuid();

            Assert.Equal(Guid.Empty, entity.Id);
        }

        [Fact]
        public void EntityBaseIdGeneric_Id_ShouldBeMutable()
        {
            var guid = Guid.NewGuid();
            var entity = new ConcreteEntityBaseIdGuid
            {
                Id = guid
            };

            Assert.Equal(guid, entity.Id);
        }

        [Fact]
        public void EntityBaseIdGeneric_ShouldImplementIEntityOfTKey()
        {
            Assert.True(typeof(IEntity<Guid>).IsAssignableFrom(typeof(EntityBaseId<Guid>)));
            Assert.True(typeof(IEntity).IsAssignableFrom(typeof(EntityBaseId<Guid>)));
        }

        [Fact]
        public void EntityBaseIdGeneric_Id_ShouldHaveKeyAndRequired()
        {
            var property = typeof(EntityBaseId<Guid>).GetProperty(nameof(EntityBaseId<Guid>.Id));

            Assert.NotNull(property);
            Assert.Contains(property!.GetCustomAttributes(false), a => a is KeyAttribute);
            Assert.Contains(property.GetCustomAttributes(false), a => a is RequiredAttribute);
        }

        [Fact]
        public void EntityBaseIdGeneric_StringKey_DefaultShouldBeDefault()
        {
            var entity = new ConcreteEntityBaseIdString();

            Assert.Null(entity.Id);
        }

        // ============================================================
        // EntityBase — 完整属性默认值
        // ============================================================

        [Fact]
        public void EntityBase_AllDefaults_ShouldMatchDocumentedValues()
        {
            var entity = new ConcreteEntityBase();

            // EntityBaseId
            Assert.Equal(0, entity.Id);

            // ISafeCreatedFilter
            Assert.Null(entity.CreatedId);
            Assert.Equal(0, entity.CreatedTime);
            Assert.Null(entity.CreatedName);

            // ISafeUpdateFilter
            Assert.Null(entity.UpdateCount);
            Assert.Null(entity.UpdateTime);
            Assert.Null(entity.UpdatedId);
            Assert.Null(entity.UpdatedName);

            // ISafeDeletedFilter
            Assert.False(entity.IsDeleted);
            Assert.Null(entity.DeleteTime);
            Assert.Null(entity.DeletedId);
            Assert.Null(entity.DeletedName);

            // IVersionedEntity
            Assert.Equal(0, entity.RowVersion);

            // ISafeEnabledFilter
            Assert.Null(entity.IsEnabled);
        }

        [Fact]
        public void EntityBase_AllProperties_ShouldBeMutable()
        {
            var entity = new ConcreteEntityBase
            {
                Id = 100,
                CreatedId = 10,
                CreatedTime = 1000L,
                CreatedName = "creator",
                UpdateCount = 5,
                UpdateTime = 2000L,
                UpdatedId = 20,
                UpdatedName = "updater",
                IsDeleted = true,
                DeleteTime = 3000L,
                DeletedId = 30,
                DeletedName = "deleter",
                RowVersion = 99,
                IsEnabled = false
            };

            Assert.Equal(100, entity.Id);
            Assert.Equal(10, entity.CreatedId);
            Assert.Equal(1000L, entity.CreatedTime);
            Assert.Equal("creator", entity.CreatedName);
            Assert.Equal(5, entity.UpdateCount);
            Assert.Equal(2000L, entity.UpdateTime);
            Assert.Equal(20, entity.UpdatedId);
            Assert.Equal("updater", entity.UpdatedName);
            Assert.True(entity.IsDeleted);
            Assert.Equal(3000L, entity.DeleteTime);
            Assert.Equal(30, entity.DeletedId);
            Assert.Equal("deleter", entity.DeletedName);
            Assert.Equal(99, entity.RowVersion);
            Assert.False(entity.IsEnabled);
        }

        [Fact]
        public void EntityBase_AllProperties_ShouldBeVirtual()
        {
            var type = typeof(EntityBase);

            AssertVirtualProperty(type, nameof(EntityBase.CreatedId));
            AssertVirtualProperty(type, nameof(EntityBase.CreatedTime));
            AssertVirtualProperty(type, nameof(EntityBase.CreatedName));
            AssertVirtualProperty(type, nameof(EntityBase.UpdateCount));
            AssertVirtualProperty(type, nameof(EntityBase.UpdateTime));
            AssertVirtualProperty(type, nameof(EntityBase.UpdatedId));
            AssertVirtualProperty(type, nameof(EntityBase.UpdatedName));
            AssertVirtualProperty(type, nameof(EntityBase.IsDeleted));
            AssertVirtualProperty(type, nameof(EntityBase.DeleteTime));
            AssertVirtualProperty(type, nameof(EntityBase.DeletedId));
            AssertVirtualProperty(type, nameof(EntityBase.DeletedName));
            AssertVirtualProperty(type, nameof(EntityBase.RowVersion));
            AssertVirtualProperty(type, nameof(EntityBase.IsEnabled));
        }

        // ============================================================
        // EntityBase — 接口实现断言
        // ============================================================

        [Fact]
        public void EntityBase_ShouldImplementAllFilterInterfaces()
        {
            var type = typeof(EntityBase);

            Assert.True(typeof(ISafeCreatedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeDeletedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeEnabledFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeUpdateFilter).IsAssignableFrom(type));
            Assert.True(typeof(IVersionedEntity).IsAssignableFrom(type));
            Assert.True(typeof(IEntity<long>).IsAssignableFrom(type));
            Assert.True(typeof(IEntity).IsAssignableFrom(type));
        }

        [Fact]
        public void EntityBase_DerivedClass_ShouldAlsoImplementAllFilterInterfaces()
        {
            var type = typeof(ConcreteEntityBase);

            Assert.True(typeof(ISafeCreatedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeDeletedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeEnabledFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeUpdateFilter).IsAssignableFrom(type));
            Assert.True(typeof(IVersionedEntity).IsAssignableFrom(type));
        }

        // ============================================================
        // EntityBase — Description 属性验证
        // ============================================================

        [Theory]
        [InlineData(nameof(EntityBase.CreatedId), "创建人Id")]
        [InlineData(nameof(EntityBase.CreatedTime), "创建时间")]
        [InlineData(nameof(EntityBase.CreatedName), "创建人姓名")]
        [InlineData(nameof(EntityBase.UpdateCount), "更新次数")]
        [InlineData(nameof(EntityBase.UpdateTime), "更新时间")]
        [InlineData(nameof(EntityBase.UpdatedId), "更新人Id")]
        [InlineData(nameof(EntityBase.UpdatedName), "更新人姓名")]
        [InlineData(nameof(EntityBase.IsDeleted), "软删除标记,true:删除,false:未删除,null:未设置(未删除)")]
        [InlineData(nameof(EntityBase.DeleteTime), "删除时间")]
        [InlineData(nameof(EntityBase.DeletedId), "删除人Id")]
        [InlineData(nameof(EntityBase.DeletedName), "删除人姓名")]
        [InlineData(nameof(EntityBase.RowVersion), "行版本号（用于乐观锁）")]
        [InlineData(nameof(EntityBase.IsEnabled), "是否启用,true:启用，false:禁用，null:未设置(启用)")]
        public void EntityBase_Property_ShouldHaveCorrectDescriptionAttribute(string propertyName, string expectedDescription)
        {
            var property = typeof(EntityBase).GetProperty(propertyName);

            Assert.NotNull(property);
            var desc = property!.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

            Assert.NotNull(desc);
            Assert.Equal(expectedDescription, desc!.Description);
        }

        // ============================================================
        // EntityBase — MaxLength 属性验证
        // ============================================================

        [Theory]
        [InlineData(nameof(EntityBase.CreatedName), 512)]
        [InlineData(nameof(EntityBase.UpdatedName), 512)]
        [InlineData(nameof(EntityBase.DeletedName), 512)]
        public void EntityBase_Property_ShouldHaveMaxLengthAttribute(string propertyName, int expectedLength)
        {
            var property = typeof(EntityBase).GetProperty(propertyName);

            Assert.NotNull(property);
            var maxLength = property!.GetCustomAttribute<MaxLengthAttribute>();

            Assert.NotNull(maxLength);
            Assert.Equal(expectedLength, maxLength!.Length);
        }

        [Fact]
        public void EntityBase_PropertiesWithoutMaxLength_ShouldNotHaveMaxLength()
        {
            Assert.Null(typeof(EntityBase).GetProperty(nameof(EntityBase.CreatedId))!.GetCustomAttribute<MaxLengthAttribute>());
            Assert.Null(typeof(EntityBase).GetProperty(nameof(EntityBase.CreatedTime))!.GetCustomAttribute<MaxLengthAttribute>());
            Assert.Null(typeof(EntityBase).GetProperty(nameof(EntityBase.IsDeleted))!.GetCustomAttribute<MaxLengthAttribute>());
        }

        // ============================================================
        // EntityBase<TKey> — 泛型版
        // ============================================================

        [Fact]
        public void EntityBaseGeneric_Defaults_ShouldMatchNonGeneric()
        {
            var entity = new ConcreteEntityBaseGuid();

            Assert.Equal(Guid.Empty, entity.Id);
            Assert.Null(entity.CreatedId);
            Assert.Equal(0, entity.CreatedTime);
            Assert.Null(entity.CreatedName);
            Assert.Null(entity.UpdateCount);
            Assert.Null(entity.UpdateTime);
            Assert.Null(entity.UpdatedId);
            Assert.Null(entity.UpdatedName);
            Assert.False(entity.IsDeleted);
            Assert.Null(entity.DeleteTime);
            Assert.Null(entity.DeletedId);
            Assert.Null(entity.DeletedName);
            Assert.Equal(0, entity.RowVersion);
            Assert.Null(entity.IsEnabled);
        }

        [Fact]
        public void EntityBaseGeneric_ShouldImplementAllFilterInterfaces()
        {
            var type = typeof(EntityBase<Guid>);

            Assert.True(typeof(ISafeCreatedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeDeletedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeEnabledFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeUpdateFilter).IsAssignableFrom(type));
            Assert.True(typeof(IVersionedEntity).IsAssignableFrom(type));
            Assert.True(typeof(IEntity<Guid>).IsAssignableFrom(type));
        }

        [Fact]
        public void EntityBaseGeneric_Properties_ShouldBeMutable()
        {
            var entity = new ConcreteEntityBaseGuid
            {
                Id = Guid.NewGuid(),
                CreatedTime = 555L,
                RowVersion = 77,
                IsDeleted = true,
                IsEnabled = true
            };

            Assert.NotEqual(Guid.Empty, entity.Id);
            Assert.Equal(555L, entity.CreatedTime);
            Assert.Equal(77, entity.RowVersion);
            Assert.True(entity.IsDeleted);
            Assert.True(entity.IsEnabled);
        }

        // ============================================================
        // EntityTenantBase
        // ============================================================

        [Fact]
        public void EntityTenantBase_TenantId_ShouldDefaultNull()
        {
            var entity = new ConcreteEntityTenantBase();

            Assert.Null(entity.TenantId);
        }

        [Fact]
        public void EntityTenantBase_TenantId_ShouldBeMutable()
        {
            var entity = new ConcreteEntityTenantBase
            {
                TenantId = 42
            };

            Assert.Equal(42, entity.TenantId);
        }

        [Fact]
        public void EntityTenantBase_ShouldImplementITenantIdFilter()
        {
            Assert.True(typeof(ITenantIdFilter).IsAssignableFrom(typeof(EntityTenantBase)));
        }

        [Fact]
        public void EntityTenantBase_ShouldInheritEntityBaseInterfaces()
        {
            var type = typeof(EntityTenantBase);

            Assert.True(typeof(EntityBase).IsAssignableFrom(type));
            Assert.True(typeof(ISafeCreatedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeDeletedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeEnabledFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeUpdateFilter).IsAssignableFrom(type));
            Assert.True(typeof(IVersionedEntity).IsAssignableFrom(type));
            Assert.True(typeof(ITenantIdFilter).IsAssignableFrom(type));
        }

        [Fact]
        public void EntityTenantBase_TenantId_ShouldHaveDescriptionAttribute()
        {
            var property = typeof(EntityTenantBase).GetProperty(nameof(EntityTenantBase.TenantId));

            Assert.NotNull(property);
            var desc = property!.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

            Assert.NotNull(desc);
            Assert.Equal("租户Id", desc!.Description);
        }

        [Fact]
        public void EntityTenantBase_TenantId_ShouldBeVirtual()
        {
            AssertVirtualProperty(typeof(EntityTenantBase), nameof(EntityTenantBase.TenantId));
        }

        [Fact]
        public void EntityTenantBase_ShouldInheritEntityBaseDefaults()
        {
            var entity = new ConcreteEntityTenantBase();

            Assert.Equal(0, entity.Id);
            Assert.False(entity.IsDeleted);
            Assert.Equal(0, entity.RowVersion);
            Assert.Null(entity.IsEnabled);
        }

        // ============================================================
        // EntityTenantBase<TKey>
        // ============================================================

        [Fact]
        public void EntityTenantBaseGeneric_ShouldImplementITenantIdFilter()
        {
            Assert.True(typeof(ITenantIdFilter).IsAssignableFrom(typeof(EntityTenantBase<Guid>)));
        }

        [Fact]
        public void EntityTenantBaseGeneric_ShouldInheritEntityBaseGeneric()
        {
            Assert.True(typeof(EntityBase<Guid>).IsAssignableFrom(typeof(EntityTenantBase<Guid>)));
        }

        // ============================================================
        // EntityTenantOrganizationBase
        // ============================================================

        [Fact]
        public void EntityTenantOrganizationBase_CreateOrganizationId_ShouldDefaultNull()
        {
            var entity = new ConcreteEntityTenantOrgBase();

            Assert.Null(entity.CreateOrganizationId);
        }

        [Fact]
        public void EntityTenantOrganizationBase_CreateOrganizationId_ShouldBeMutable()
        {
            var entity = new ConcreteEntityTenantOrgBase
            {
                CreateOrganizationId = 99
            };

            Assert.Equal(99, entity.CreateOrganizationId);
        }

        [Fact]
        public void EntityTenantOrganizationBase_ShouldImplementIOrganizationIdFilter()
        {
            Assert.True(typeof(IOrganizationIdFilter).IsAssignableFrom(typeof(EntityTenantOrganizationBase)));
        }

        [Fact]
        public void EntityTenantOrganizationBase_ShouldInheritAllInterfaces()
        {
            var type = typeof(EntityTenantOrganizationBase);

            Assert.True(typeof(EntityTenantBase).IsAssignableFrom(type));
            Assert.True(typeof(EntityBase).IsAssignableFrom(type));
            Assert.True(typeof(ITenantIdFilter).IsAssignableFrom(type));
            Assert.True(typeof(IOrganizationIdFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeCreatedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeDeletedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeEnabledFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeUpdateFilter).IsAssignableFrom(type));
            Assert.True(typeof(IVersionedEntity).IsAssignableFrom(type));
        }

        [Fact]
        public void EntityTenantOrganizationBase_CreateOrganizationId_ShouldHaveDescriptionAttribute()
        {
            var property = typeof(EntityTenantOrganizationBase).GetProperty(nameof(EntityTenantOrganizationBase.CreateOrganizationId));

            Assert.NotNull(property);
            var desc = property!.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

            Assert.NotNull(desc);
            Assert.Equal("创建者部门Id", desc!.Description);
        }

        [Fact]
        public void EntityTenantOrganizationBase_CreateOrganizationId_ShouldBeVirtual()
        {
            AssertVirtualProperty(typeof(EntityTenantOrganizationBase), nameof(EntityTenantOrganizationBase.CreateOrganizationId));
        }

        [Fact]
        public void EntityTenantOrganizationBase_AllProperties_ShouldBeMutable()
        {
            var entity = new ConcreteEntityTenantOrgBase
            {
                Id = 1,
                TenantId = 2,
                CreateOrganizationId = 3,
                CreatedTime = 4,
                RowVersion = 5,
                IsDeleted = true,
                IsEnabled = false
            };

            Assert.Equal(1, entity.Id);
            Assert.Equal(2, entity.TenantId);
            Assert.Equal(3, entity.CreateOrganizationId);
            Assert.Equal(4, entity.CreatedTime);
            Assert.Equal(5, entity.RowVersion);
            Assert.True(entity.IsDeleted);
            Assert.False(entity.IsEnabled);
        }

        // ============================================================
        // EntityTenantOrganizationBase<TKey>
        // ============================================================

        [Fact]
        public void EntityTenantOrganizationBaseGeneric_ShouldImplementIOrganizationIdFilter()
        {
            Assert.True(typeof(IOrganizationIdFilter).IsAssignableFrom(typeof(EntityTenantOrganizationBase<Guid>)));
        }

        [Fact]
        public void EntityTenantOrganizationBaseGeneric_ShouldInheritEntityTenantBaseGeneric()
        {
            Assert.True(typeof(EntityTenantBase<Guid>).IsAssignableFrom(typeof(EntityTenantOrganizationBase<Guid>)));
        }

        // ============================================================
        // IAuditableEntity 标记
        // ============================================================

        [Fact]
        public void IAuditableEntity_ShouldDefineSixProperties()
        {
            var type = typeof(IAuditableEntity);

            Assert.True(type.IsInterface);
            Assert.Equal(6, type.GetProperties().Length);
            Assert.NotNull(type.GetProperty(nameof(IAuditableEntity.CreateTime)));
            Assert.NotNull(type.GetProperty(nameof(IAuditableEntity.UpdateTime)));
            Assert.NotNull(type.GetProperty(nameof(IAuditableEntity.CreateUserId)));
            Assert.NotNull(type.GetProperty(nameof(IAuditableEntity.CreateUserName)));
            Assert.NotNull(type.GetProperty(nameof(IAuditableEntity.UpdateUserId)));
            Assert.NotNull(type.GetProperty(nameof(IAuditableEntity.UpdateUserName)));
        }

        // ============================================================
        // 测试辅助方法
        // ============================================================

        private static void AssertVirtualProperty(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            Assert.NotNull(property);
            Assert.True(property!.GetMethod != null && property.GetMethod.IsVirtual,
                propertyName + " 的 getter 应为 virtual");
            Assert.True(property.SetMethod != null && property.SetMethod.IsVirtual,
                propertyName + " 的 setter 应为 virtual");
        }

        // ============================================================
        // 测试辅助类型
        // ============================================================

        private sealed class ConcreteEntityBaseId : EntityBaseId
        {
        }

        private sealed class ConcreteEntityBaseIdGuid : EntityBaseId<Guid>
        {
        }

        private sealed class ConcreteEntityBaseIdString : EntityBaseId<string>
        {
        }

        private sealed class ConcreteEntityBase : EntityBase
        {
        }

        private sealed class ConcreteEntityBaseGuid : EntityBase<Guid>
        {
        }

        private sealed class ConcreteEntityTenantBase : EntityTenantBase
        {
        }

        private sealed class ConcreteEntityTenantOrgBase : EntityTenantOrganizationBase
        {
        }
    }
}
