// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.ComponentModel.DataAnnotations.Schema;
using GameFrameX.Foundation.Orm.Entity.Filter;

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 框架实体基类
/// </summary>
public abstract class EntityBase : EntityBaseId, IAuditableEntity, IDeletedFilter, IVersionedEntity
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Column]
    public virtual DateTime CreateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public virtual long? CreateUserId { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    public virtual string? CreateUserName { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    public virtual long? UpdateUserId { get; set; }

    /// <summary>
    /// 修改者姓名
    /// </summary>
    public virtual string? UpdateUserName { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    public virtual bool IsDelete { get; set; } = false;

    /// <summary>
    /// 版本号（用于乐观锁）
    /// </summary>
    public virtual long Version { get; set; } = 1;
}

/// <summary>
/// 泛型框架实体基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class EntityBase<TKey> : EntityBaseId<TKey>, IAuditableEntity, IDeletedFilter, IVersionedEntity
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Column]
    public virtual DateTime CreateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 更新时间
    /// </summary>
    public virtual DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    public virtual long? CreateUserId { get; set; }

    /// <summary>
    /// 创建者姓名
    /// </summary>
    public virtual string? CreateUserName { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    public virtual long? UpdateUserId { get; set; }

    /// <summary>
    /// 修改者姓名
    /// </summary>
    public virtual string? UpdateUserName { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    public virtual bool IsDelete { get; set; } = false;

    /// <summary>
    /// 版本号（用于乐观锁）
    /// </summary>
    public virtual long Version { get; set; } = 1;
}
