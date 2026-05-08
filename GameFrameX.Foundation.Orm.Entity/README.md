# GameFrameX.Foundation.Orm.Entity

基础 ORM 实体定义库，提供不依赖任何第三方框架的实体基类和接口，支持审计追踪、软删除、乐观锁等企业级功能。

## 安装

```bash
dotnet add package GameFrameX.Foundation.Orm.Entity
```

## 核心组件

| 组件 | 文件 | 说明 |
|------|------|------|
| EntityBase | `EntityBase.cs` | 全功能实体基类（ID、审计、软删除、版本控制） |
| EntityBaseId (Generic) | `EntityBaseId.cs` | 自定义主键类型实体基类 |
| IEntity | `IEntity.cs` | 基础实体接口 |
| IAuditableEntity | `IAuditableEntity.cs` | 审计接口（创建/更新时间和用户） |

## 功能特性

### 核心接口

- **IEntity**: 实体基础接口
- **IEntity<TKey>**: 带主键的实体接口，支持泛型主键类型
- **IVersionedEntity**: 版本控制实体接口，支持乐观锁

### 过滤器接口

- **ISafeCreatedFilter**: 创建信息过滤器（`CreatedId`、`CreatedTime`、`CreatedName`）
- **ISafeUpdateFilter**: 更新信息过滤器（`UpdateCount`、`UpdateTime`、`UpdatedId`、`UpdatedName`）
- **ISafeDeletedFilter**: 软删除过滤器（`IsDeleted`、`DeleteTime`、`DeletedId`、`DeletedName`）
- **ISafeEnabledFilter**: 启用状态过滤器（`IsEnabled`）
- **ITenantIdFilter**: 租户ID过滤器（`TenantId`）
- **ISelectFilter**: 搜索查询过滤器（`Name`、`Description`）
- **IOrganizationIdFilter**: 机构ID过滤器（`CreateOrganizationId`）

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
using GameFrameX.Foundation.Orm.Entity;

// Classes inheriting EntityBase automatically gain full enterprise features
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    // EntityBase 自动提供的属性:
    // - long Id                    // 主键
    // - DateTime CreateTime        // 创建时间
    // - DateTime UpdateTime        // 更新时间
    // - long CreateUserId          // 创建人 ID
    // - long UpdateUserId          // 更新人 ID
    // - string CreateUserName      // 创建人姓名
    // - string UpdateUserName      // 更新人姓名
    // - bool IsDelete              // 软删除标记
    // - long Version               // 乐观锁版本
    // - bool IsEnabled             // 启用状态
}

var user = new User
{
    Username = "john_doe",
    Email = "john@example.com",
    PasswordHash = "hashed_password",
    CreateTime = DateTime.UtcNow,
    CreateUserId = 1,
    CreateUserName = "admin",
    IsEnabled = true
};
```

### 自定义主键类型

```csharp
// 使用 string 作为主键
public class Product : EntityBaseId<string>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}

// 使用 Guid 作为主键
public class Order : EntityBaseId<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}

var product = new Product
{
    Id = "PROD-001",
    Name = "Laptop",
    Price = 5999.99m,
    Description = "High performance laptop"
};

var order = new Order
{
    Id = Guid.NewGuid(),
    OrderNumber = "ORD-20240101-001",
    TotalAmount = 5999.99m,
    OrderDate = DateTime.UtcNow
};
```

### 接口实现

```csharp
// 实现基础实体接口
public class Category : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// 实现审计接口
public class AuditableCategory : IEntity<int>, IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // IAuditableEntity 接口属性
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public long CreateUserId { get; set; }
    public long UpdateUserId { get; set; }
    public string CreateUserName { get; set; }
    public string UpdateUserName { get; set; }
}
```

### 多租户实体

```csharp
public class TenantOrder : EntityTenantBase
{
    public string OrderNumber { get; set; }
    public decimal Amount { get; set; }
}
```

### 可搜索实体

```csharp
public class SearchableCategory : EntitySelectBase
{
    public int? SortOrder { get; set; }
}
```

## 企业级功能

### 审计追踪

```csharp
public class Document : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
}

var document = new Document
{
    Title = "Important Document",
    Content = "Document content...",
    CreateTime = DateTime.UtcNow,
    CreateUserId = currentUser.Id,
    CreateUserName = currentUser.Username,
    UpdateTime = DateTime.UtcNow,
    UpdateUserId = currentUser.Id,
    UpdateUserName = currentUser.Username
};

// 更新时自动维护审计信息
document.Content = "Updated content";
document.UpdateTime = DateTime.UtcNow;
document.UpdateUserId = currentUser.Id;
document.UpdateUserName = currentUser.Username;
document.Version++; // 乐观锁版本递增
```

### 软删除

```csharp
// 软删除：标记为已删除而非物理删除
public void SoftDeleteUser(User user)
{
    user.IsDelete = true;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    dbContext.SaveChanges();
}

// 查询时过滤已删除记录
var activeUsers = dbContext.Users
    .Where(u => !u.IsDelete)
    .ToList();

// 恢复已删除记录
public void RestoreUser(User user)
{
    user.IsDelete = false;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    dbContext.SaveChanges();
}
```

### 乐观锁

```csharp
public void UpdateUserWithOptimisticLock(long userId, string newEmail)
{
    var user = dbContext.Users.Find(userId);
    var originalVersion = user.Version;

    user.Email = newEmail;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    user.Version++;

    try
    {
        var rowsAffected = dbContext.Database.ExecuteSqlRaw(
            "UPDATE Users SET Email = {0}, Version = {1} WHERE Id = {2} AND Version = {3}",
            user.Email, user.Version, user.Id, originalVersion);

        if (rowsAffected == 0)
            throw new ConcurrencyException("Data modified by another user, please refresh and retry");
    }
    catch (DbUpdateConcurrencyException)
    {
        throw new ConcurrencyException("Concurrency conflict, please refresh and retry");
    }
}
```

### 启用/禁用状态管理

```csharp
public class Feature : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public void ToggleFeature(long featureId, bool enabled)
{
    var feature = dbContext.Features.Find(featureId);
    feature.IsEnabled = enabled;
    feature.UpdateTime = DateTime.UtcNow;
    feature.Version++;
    dbContext.SaveChanges();
}

var enabledFeatures = dbContext.Features
    .Where(f => f.IsEnabled && !f.IsDelete)
    .ToList();
```

## 设计原则

- **单一职责**：每个接口和类都有明确的职责
- **开放封闭**：通过继承和接口实现扩展功能
- **依赖倒置**：依赖抽象接口而非具体实现
- **无框架依赖**：仅使用 .NET 标准库，不依赖任何 ORM 框架
- **可空类型**：所有字段均为可空类型，提供更好的灵活性

## 许可证

MIT + Apache 2.0
