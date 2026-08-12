using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GameFrameX.Foundation.Orm.Entity;
using GameFrameX.Foundation.Orm.Entity.Filter;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    /// <summary>
    /// EntitySelectBase 全量覆盖测试：默认值、可写性、接口实现、Attribute、与 EntityBase 的差异。
    /// </summary>
    public class EntitySelectBaseTests
    {
        // ============================================================
        // 默认值
        // ============================================================

        [Fact]
        public void EntitySelectBase_Name_ShouldDefaultNull()
        {
            var entity = new ConcreteEntitySelectBase();

            Assert.Null(entity.Name);
        }

        [Fact]
        public void EntitySelectBase_Description_ShouldDefaultNull()
        {
            var entity = new ConcreteEntitySelectBase();

            Assert.Null(entity.Description);
        }

        [Fact]
        public void EntitySelectBase_InheritedProperties_ShouldHaveEntityBaseDefaults()
        {
            var entity = new ConcreteEntitySelectBase();

            Assert.Equal(0, entity.Id);
            Assert.False(entity.IsDeleted);
            Assert.Equal(0, entity.RowVersion);
            Assert.Null(entity.IsEnabled);
            Assert.Null(entity.CreatedId);
            Assert.Equal(0, entity.CreatedTime);
            Assert.Null(entity.CreatedName);
            Assert.Null(entity.UpdateCount);
            Assert.Null(entity.UpdateTime);
            Assert.Null(entity.UpdatedId);
            Assert.Null(entity.UpdatedName);
            Assert.Null(entity.DeleteTime);
            Assert.Null(entity.DeletedId);
            Assert.Null(entity.DeletedName);
        }

        // ============================================================
        // 可写性
        // ============================================================

        [Fact]
        public void EntitySelectBase_Name_ShouldBeMutable()
        {
            var entity = new ConcreteEntitySelectBase
            {
                Name = "订单"
            };

            Assert.Equal("订单", entity.Name);
        }

        [Fact]
        public void EntitySelectBase_Description_ShouldBeMutable()
        {
            var entity = new ConcreteEntitySelectBase
            {
                Description = "订单详情"
            };

            Assert.Equal("订单详情", entity.Description);
        }

        [Fact]
        public void EntitySelectBase_AllProperties_ShouldBeMutable()
        {
            var entity = new ConcreteEntitySelectBase
            {
                Id = 1,
                Name = "App",
                Description = "描述",
                IsDeleted = true,
                RowVersion = 10,
                IsEnabled = true
            };

            Assert.Equal(1, entity.Id);
            Assert.Equal("App", entity.Name);
            Assert.Equal("描述", entity.Description);
            Assert.True(entity.IsDeleted);
            Assert.Equal(10, entity.RowVersion);
            Assert.True(entity.IsEnabled);
        }

        // ============================================================
        // Virtual
        // ============================================================

        [Fact]
        public void EntitySelectBase_Name_ShouldBeVirtual()
        {
            var property = typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Name));

            Assert.NotNull(property);
            Assert.True(property!.GetMethod != null && property.GetMethod.IsVirtual);
            Assert.True(property.SetMethod != null && property.SetMethod.IsVirtual);
        }

        [Fact]
        public void EntitySelectBase_Description_ShouldBeVirtual()
        {
            var property = typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Description));

            Assert.NotNull(property);
            Assert.True(property!.GetMethod != null && property.GetMethod.IsVirtual);
            Assert.True(property.SetMethod != null && property.SetMethod.IsVirtual);
        }

        // ============================================================
        // 接口实现
        // ============================================================

        [Fact]
        public void EntitySelectBase_ShouldImplementISelectFilter()
        {
            Assert.True(typeof(ISelectFilter).IsAssignableFrom(typeof(EntitySelectBase)));
        }

        [Fact]
        public void EntitySelectBase_ShouldInheritEntityBase()
        {
            Assert.True(typeof(EntityBase).IsAssignableFrom(typeof(EntitySelectBase)));
        }

        [Fact]
        public void EntitySelectBase_ShouldImplementAllInheritedInterfaces()
        {
            var type = typeof(EntitySelectBase);

            Assert.True(typeof(ISelectFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeCreatedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeDeletedFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeEnabledFilter).IsAssignableFrom(type));
            Assert.True(typeof(ISafeUpdateFilter).IsAssignableFrom(type));
            Assert.True(typeof(IVersionedEntity).IsAssignableFrom(type));
            Assert.True(typeof(IEntity<long>).IsAssignableFrom(type));
            Assert.True(typeof(IEntity).IsAssignableFrom(type));
        }

        [Fact]
        public void EntitySelectBase_ShouldNotImplementTenantInterfaces()
        {
            Assert.False(typeof(ITenantIdFilter).IsAssignableFrom(typeof(EntitySelectBase)));
            Assert.False(typeof(IOrganizationIdFilter).IsAssignableFrom(typeof(EntitySelectBase)));
        }

        // ============================================================
        // Attribute 验证
        // ============================================================

        [Fact]
        public void EntitySelectBase_Name_ShouldHaveMaxLength512()
        {
            var property = typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Name));

            Assert.NotNull(property);
            var maxLength = property!.GetCustomAttribute<MaxLengthAttribute>();

            Assert.NotNull(maxLength);
            Assert.Equal(512, maxLength!.Length);
        }

        [Fact]
        public void EntitySelectBase_Description_ShouldHaveMaxLength4096()
        {
            var property = typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Description));

            Assert.NotNull(property);
            var maxLength = property!.GetCustomAttribute<MaxLengthAttribute>();

            Assert.NotNull(maxLength);
            Assert.Equal(4096, maxLength!.Length);
        }

        [Fact]
        public void EntitySelectBase_Name_ShouldHaveDescriptionAttribute()
        {
            var property = typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Name));

            Assert.NotNull(property);
            var desc = property!.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

            Assert.NotNull(desc);
            Assert.Equal("名称", desc!.Description);
        }

        [Fact]
        public void EntitySelectBase_Description_ShouldHaveDescriptionAttribute()
        {
            var property = typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Description));

            Assert.NotNull(property);
            var desc = property!.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();

            Assert.NotNull(desc);
            Assert.Equal("详细描述", desc!.Description);
        }

        // ============================================================
        // EntitySelectBase<TKey> — 泛型版
        // ============================================================

        [Fact]
        public void EntitySelectBaseGeneric_Name_ShouldDefaultNull()
        {
            var entity = new ConcreteEntitySelectBaseGuid();

            Assert.Null(entity.Name);
            Assert.Null(entity.Description);
        }

        [Fact]
        public void EntitySelectBaseGeneric_Name_ShouldBeMutable()
        {
            var entity = new ConcreteEntitySelectBaseGuid
            {
                Name = "App",
                Description = "desc"
            };

            Assert.Equal("App", entity.Name);
            Assert.Equal("desc", entity.Description);
        }

        [Fact]
        public void EntitySelectBaseGeneric_ShouldImplementISelectFilter()
        {
            Assert.True(typeof(ISelectFilter).IsAssignableFrom(typeof(EntitySelectBase<Guid>)));
        }

        [Fact]
        public void EntitySelectBaseGeneric_ShouldInheritEntityBaseGeneric()
        {
            Assert.True(typeof(EntityBase<Guid>).IsAssignableFrom(typeof(EntitySelectBase<Guid>)));
        }

        [Fact]
        public void EntitySelectBaseGeneric_Name_ShouldHaveMaxLength512()
        {
            var property = typeof(EntitySelectBase<Guid>).GetProperty(nameof(EntitySelectBase<Guid>.Name));

            Assert.NotNull(property);
            var maxLength = property!.GetCustomAttribute<MaxLengthAttribute>();

            Assert.NotNull(maxLength);
            Assert.Equal(512, maxLength!.Length);
        }

        [Fact]
        public void EntitySelectBaseGeneric_Description_ShouldHaveMaxLength4096()
        {
            var property = typeof(EntitySelectBase<Guid>).GetProperty(nameof(EntitySelectBase<Guid>.Description));

            Assert.NotNull(property);
            var maxLength = property!.GetCustomAttribute<MaxLengthAttribute>();

            Assert.NotNull(maxLength);
            Assert.Equal(4096, maxLength!.Length);
        }

        // ============================================================
        // EntitySelectBase vs EntityBase 差异
        // ============================================================

        [Fact]
        public void EntitySelectBase_ShouldAddNameAndDescriptionBeyondEntityBase()
        {
            Assert.NotNull(typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Name)));
            Assert.NotNull(typeof(EntitySelectBase).GetProperty(nameof(EntitySelectBase.Description)));
            Assert.Null(typeof(EntityBase).GetProperty("Name"));
            Assert.Null(typeof(EntityBase).GetProperty("Description"));
        }

        [Fact]
        public void EntitySelectBase_ShouldBeAbstract()
        {
            Assert.True(typeof(EntitySelectBase).IsAbstract);
        }

        // ============================================================
        // 测试辅助类型
        // ============================================================

        private sealed class ConcreteEntitySelectBase : EntitySelectBase
        {
        }

        private sealed class ConcreteEntitySelectBaseGuid : EntitySelectBase<Guid>
        {
        }
    }
}
