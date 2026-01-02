# GameFrameX.Foundation.Orm.Entity

基础 ORM 实体定义库，提供不依赖任何第三方框架的实体基类和接口。

## 功能特性

### 核心接口

- **IEntity**: 实体基础接口
- **IEntity<TKey>**: 带主键的实体接口，支持泛型主键类型
- **IAuditableEntity**: 可审计实体接口，包含创建/更新时间和用户信息
- **IVersionedEntity**: 版本控制实体接口，支持乐观锁

### 过滤器接口

- **IDeletedFilter**: 软删除过滤器
- **ITenantIdFilter**: 租户ID过滤器
- **ISelectFilter**: 搜索查询过滤器
- **IOrganizationIdFilter**: 机构ID过滤器

### 实体基类

#### EntityBaseId / EntityBaseId<TKey>
- 提供主键字段
- 支持 long 类型主键和泛型主键
- 实现 IEntity 接口

#### EntityBase / EntityBase<TKey>
- 继承自 EntityBaseId
- 实现审计功能（创建时间、更新时间、创建者、修改者）
- 支持软删除
- 支持版本控制（乐观锁）
- 提供泛型版本支持不同主键类型

#### EntityTenantBase / EntityTenantBase<TKey>
- 继承自 EntityBase
- 支持多租户架构
- 包含租户ID字段

#### EntitySelectBase
- 继承自 EntityBase
- 实现 ISelectFilter 接口
- 包含名称和描述字段，适用于需要搜索的实体

## 使用示例

### 基础实体使用

```csharp
public class User : EntityBase
{
    public string UserName { get; set; }
    public string Email { get; set; }
}
```

### 使用不同主键类型

```csharp
public class Product : EntityBase<Guid>
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}
```

### 多租户实体

```csharp
public class Order : EntityTenantBase
{
    public string OrderNumber { get; set; }
    public decimal Amount { get; set; }
}
```

### 可搜索实体

```csharp
public class Category : EntitySelectBase
{
    public int SortOrder { get; set; }
}
```

## 改进点

1. **类型灵活性**: 支持泛型主键类型，不再局限于 long 类型
2. **接口一致性**: 统一接口访问修饰符，提供清晰的契约定义
3. **版本控制**: 新增乐观锁支持，防止并发更新冲突
4. **多租户支持**: 提供专门的多租户实体基类
5. **时间字段优化**: 创建时间设置默认值，不再为可空类型
6. **接口实现**: 实体基类正确实现相关接口，提供更好的类型安全

## 设计原则

- **单一职责**: 每个接口和类都有明确的职责
- **开放封闭**: 通过继承和接口实现扩展功能
- **依赖倒置**: 依赖抽象接口而非具体实现
- **无框架依赖**: 仅使用 .NET 标准库，不依赖任何 ORM 框架