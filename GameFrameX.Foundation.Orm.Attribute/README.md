# GameFrameX.Foundation.Orm.Attribute

GameFrameX ORM 特性库，提供了一套完整的数据库实体标记特性，用于增强 ORM 框架的功能和性能。

## 📋 特性列表

### 🔧 基础特性

#### ConstAttribute
常量特性，用于标记类、方法、属性等为常量定义。

```csharp
[Const("DatabaseVersion")]
public class DatabaseConstants
{
    public const string Version = "1.0.0";
}
```

#### CustomUnifyResultAttribute
自定义统一结果特性，用于标记需要自定义结果统一处理的类或方法。

```csharp
[CustomUnifyResult("ApiResponse")]
public class UserController
{
    public User GetUser(int id) => userService.GetById(id);
}
```

### 🗄️ 表类型特性

#### IncrementSeedAttribute
增量种子特性，标记实体类支持自增种子值功能。

```csharp
[IncrementSeed]
public class User
{
    public int Id { get; set; }  // 自增主键
    public string Name { get; set; }
}
```

#### IncrementTableAttribute
增量表特性，标记实体类对应的数据库表支持增量操作。

```csharp
[IncrementTable]
public class UserActivity
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }  // 用于增量判断
}
```

#### LogTableAttribute
日志表特性，标记实体类对应的数据库表为日志表。

```csharp
[LogTable]
public class UserOperationLog
{
    public long Id { get; set; }
    public string Operation { get; set; }
    public DateTime CreatedTime { get; set; }
}
```

#### SystemTableAttribute
系统表特性，标记实体类对应的数据库表为系统表。

```csharp
[SystemTable]
public class SystemConfiguration
{
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
}
```

### 🚀 性能优化特性

#### CacheTableAttribute
缓存表特性，标记实体类对应的数据库表支持缓存策略。

```csharp
[CacheTable(CacheType = "Redis", ExpireMinutes = 30)]
public class ProductInfo
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

#### ReadOnlyTableAttribute
只读表特性，标记实体类对应的数据库表为只读表。

```csharp
[ReadOnlyTable(EnableCache = true, CacheMinutes = 60)]
public class CountryCode
{
    public string Code { get; set; }
    public string Name { get; set; }
}
```

#### EntityIndexAttribute
索引特性，标记属性或字段需要创建数据库索引。

```csharp
public class User
{
    [EntityIndex("IX_User_Email", Unique = true)]
    public string Email { get; set; }
    
    [EntityIndex("IX_User_Status_CreateTime", IsAscending = true)]
    public string Status { get; set; }
}
```

#### PartitionTableAttribute
分区表特性，标记实体类对应的数据库表支持分区存储。

```csharp
[PartitionTable("CreateDate", PartitionType.Range, PartitionInterval.Monthly)]
public class OrderHistory
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }  // 分区键
}
```

### 🔒 数据管理特性

#### AuditTableAttribute
审计表特性，标记实体类对应的数据库表需要进行审计跟踪。

```csharp
[AuditTable(AuditLevel = AuditLevel.Full, IncludeUserInfo = true)]
public class UserAccount
{
    public int Id { get; set; }
    public string Username { get; set; }
    public decimal Balance { get; set; }
}
```

#### SoftDeleteAttribute
软删除特性，标记实体类支持软删除功能。

```csharp
[SoftDelete("IsDeleted", "DeletedTime")]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedTime { get; set; }
}
```

#### VersionControlAttribute
版本控制特性，标记实体类支持数据版本管理功能。

```csharp
[VersionControl("Version", VersionStrategy.Optimistic)]
public class Document
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Version { get; set; }  // 版本号字段
}
```

## 🎯 使用场景

### 高频查询优化
```csharp
[CacheTable("Redis", 60)]
[ReadOnlyTable(EnableCache = true)]
[EntityIndex("IX_Product_Category")]
public class Product
{
    public int Id { get; set; }
    
    [EntityIndex("IX_Product_Category")]
    public string Category { get; set; }
    
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

### 大数据表管理
```csharp
[PartitionTable("CreateDate", PartitionType.Range, PartitionInterval.Monthly)]
[AuditTable(AuditLevel.ChangesOnly)]
[SoftDelete("IsDeleted", "DeletedTime")]
public class OrderRecord
{
    public long Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreateDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedTime { get; set; }
}
```

### 系统核心表
```csharp
[SystemTable]
[AuditTable(AuditLevel.Full, IncludeUserInfo = true, IncludeIpAddress = true)]
[VersionControl("Version", VersionStrategy.Optimistic)]
public class SystemUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public int Version { get; set; }
}
```

## 📚 特性分类

### 按优先级分类

**高优先级**（常用且实用）：
- `CacheTableAttribute` - 缓存策略
- `AuditTableAttribute` - 审计跟踪  
- `SoftDeleteAttribute` - 软删除
- `ReadOnlyTableAttribute` - 只读优化

**中优先级**（特定场景有用）：
- `EntityIndexAttribute` - 索引管理
- `PartitionTableAttribute` - 分区表
- `VersionControlAttribute` - 版本控制

**基础特性**（框架核心）：
- `ConstAttribute` - 常量标记
- `CustomUnifyResultAttribute` - 结果统一
- `IncrementSeedAttribute` - 自增种子
- `IncrementTableAttribute` - 增量表
- `LogTableAttribute` - 日志表
- `SystemTableAttribute` - 系统表

### 按功能分类

**性能优化类**：
- `CacheTableAttribute`
- `ReadOnlyTableAttribute`
- `EntityIndexAttribute`
- `PartitionTableAttribute`

**数据管理类**：
- `AuditTableAttribute`
- `SoftDeleteAttribute`
- `VersionControlAttribute`
- `LogTableAttribute`

**表类型标识类**：
- `SystemTableAttribute`
- `IncrementTableAttribute`
- `IncrementSeedAttribute`

**框架功能类**：
- `ConstAttribute`
- `CustomUnifyResultAttribute`

## 🔧 安装和使用

1. 引用项目或NuGet包
2. 在实体类上应用相应的特性
3. 配置ORM框架以识别和处理这些特性
4. 根据特性配置相应的数据库策略

## 📖 最佳实践

1. **合理选择特性**：根据实际业务需求选择合适的特性
2. **性能考虑**：缓存和索引特性要考虑内存和存储开销
3. **数据一致性**：审计和版本控制特性要考虑数据一致性要求
4. **维护成本**：复杂特性会增加系统维护成本
5. **测试验证**：充分测试特性的功能和性能影响

## 🤝 贡献

欢迎提交Issue和Pull Request来改进这个特性库。

## 📄 许可证

本项目遵循 MIT 许可证和 Apache 许可证（版本 2.0）。