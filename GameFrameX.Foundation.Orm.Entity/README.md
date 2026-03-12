# GameFrameX.Foundation.Orm.Entity

基础 ORM 实体定义库，提供不依赖任何第三方框架的实体基类和接口。

## 功能特性

### 核心接口

- **IEntity**: 实体基础接口
- **IEntity<TKey>**: 带主键的实体接口，支持泛型主键类型
- **IVersionedEntity**: 版本控制实体接口，支持乐观锁

### 过滤器接口

- **ISafeCreatedFilter**: 创建信息过滤器
  - `CreatedId` (long?): 创建人Id
  - `CreatedTime` (long?): 创建时间
  - `CreatedName` (string?): 创建人姓名

- **ISafeUpdateFilter**: 更新信息过滤器
  - `UpdateCount` (int?): 更新次数
  - `UpdateTime` (long?): 更新时间
  - `UpdatedId` (long?): 更新人Id
  - `UpdatedName` (string?): 更新人姓名

- **ISafeDeletedFilter**: 软删除过滤器
  - `IsDeleted` (bool?): 是否删除
  - `DeleteTime` (long?): 删除时间
  - `DeletedId` (long?): 删除人Id
  - `DeletedName` (string?): 删除人姓名

- **ISafeEnabledFilter**: 启用状态过滤器
  - `IsEnabled` (bool?): 是否启用

- **ITenantIdFilter**: 租户ID过滤器
  - `TenantId` (long?): 租户Id

- **ISelectFilter**: 搜索查询过滤器
  - `Name` (string?): 名称
  - `Description` (string?): 详细描述

- **IOrganizationIdFilter**: 机构ID过滤器
  - `CreateOrganizationId` (long?): 创建者部门Id

### 实体基类

#### EntityBaseId / EntityBaseId<TKey>
- 提供主键字段
- 支持 long 类型主键和泛型主键
- 实现 IEntity 接口

#### EntityBase / EntityBase<TKey>
- 继承自 EntityBaseId
- 实现 `ISafeCreatedFilter`、`ISafeUpdateFilter`、`ISafeDeletedFilter`、`ISafeEnabledFilter`、`IVersionedEntity`
- 所有字段均为可空类型
- 支持版本控制（乐观锁）

#### EntityTenantBase / EntityTenantBase<TKey>
- 继承自 EntityBase
- 实现 `ITenantIdFilter`
- 支持多租户架构

#### EntitySelectBase
- 继承自 EntityBase
- 实现 `ISelectFilter` 接口
- 包含名称和描述字段，适用于需要搜索的实体

## 使用示例

### 基础实体使用

```csharp
public class User : EntityBase
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
```

### 使用不同主键类型

```csharp
public class Product : EntityBase<Guid>
{
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
}
```

### 多租户实体

```csharp
public class Order : EntityTenantBase
{
    public string? OrderNumber { get; set; }
    public decimal? Amount { get; set; }
}
```

### 可搜索实体

```csharp
public class Category : EntitySelectBase
{
    public int? SortOrder { get; set; }
}
```

## 设计原则

- **单一职责**: 每个接口和类都有明确的职责
- **开放封闭**: 通过继承和接口实现扩展功能
- **依赖倒置**: 依赖抽象接口而非具体实现
- **无框架依赖**: 仅使用 .NET 标准库，不依赖任何 ORM 框架
- **可空类型**: 所有字段均为可空类型，提供更好的灵活性