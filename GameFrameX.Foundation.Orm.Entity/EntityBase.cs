// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.ComponentModel;
using GameFrameX.Foundation.Orm.Entity.Filter;

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 框架实体基类
/// </summary>
public abstract class EntityBase : EntityBaseId, ISafeDeletedFilter, IVersionedEntity, ISafeEnabledFilter, ISafeCreatedFilter, ISafeUpdateFilter
{
    /// <summary>
    /// 创建人Id
    /// </summary>
    [Description("创建人Id")]
    public virtual long? CreatedId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    public virtual long? CreatedTime { get; set; }

    /// <summary>
    /// 创建人姓名
    /// </summary>
    [Description("创建人姓名")]
    public virtual string? CreatedName { get; set; }

    /// <summary>
    /// 更新次数
    /// </summary>
    [Description("更新次数")]
    public virtual int? UpdateCount { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Description("更新时间")]
    public virtual long? UpdateTime { get; set; }

    /// <summary>
    /// 更新人Id
    /// </summary>
    [Description("更新人Id")]
    public virtual long? UpdatedId { get; set; }

    /// <summary>
    /// 更新人姓名
    /// </summary>
    [Description("更新人姓名")]
    public virtual string? UpdatedName { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    [Description("软删除标记,true:删除,false:未删除,null:未设置(未删除)")]
    public virtual bool? IsDeleted { get; set; } = false;

    /// <summary>
    /// 删除时间
    /// </summary>
    [Description("删除时间")]
    public virtual long? DeleteTime { get; set; }

    /// <summary>
    /// 删除人Id
    /// </summary>
    [Description("删除人Id")]
    public virtual long? DeletedId { get; set; }

    /// <summary>
    /// 删除人姓名
    /// </summary>
    [Description("删除人姓名")]
    public virtual string? DeletedName { get; set; }

    /// <summary>
    /// 版本号（用于乐观锁）
    /// </summary>
    [Description("版本号（用于乐观锁）")]
    public virtual long? Version { get; set; } = 0;

    /// <summary>
    /// 是否启用该实体或功能的标识
    /// </summary>
    /// <value>
    /// true表示启用，false表示禁用，null表示未设置
    /// </value>
    [Description("是否启用,true:启用，false:禁用，null:未设置(启用)")]
    public virtual bool? IsEnabled { get; set; }
}

/// <summary>
/// 泛型框架实体基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class EntityBase<TKey> : EntityBaseId<TKey>, ISafeDeletedFilter, IVersionedEntity, ISafeEnabledFilter, ISafeCreatedFilter, ISafeUpdateFilter
    where TKey : notnull
{
    /// <summary>
    /// 创建人Id
    /// </summary>
    [Description("创建人Id")]
    public virtual long? CreatedId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    public virtual long? CreatedTime { get; set; }

    /// <summary>
    /// 创建人姓名
    /// </summary>
    [Description("创建人姓名")]
    public virtual string? CreatedName { get; set; }

    /// <summary>
    /// 更新次数
    /// </summary>
    [Description("更新次数")]
    public virtual int? UpdateCount { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Description("更新时间")]
    public virtual long? UpdateTime { get; set; }

    /// <summary>
    /// 更新人Id
    /// </summary>
    [Description("更新人Id")]
    public virtual long? UpdatedId { get; set; }

    /// <summary>
    /// 更新人姓名
    /// </summary>
    [Description("更新人姓名")]
    public virtual string? UpdatedName { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    [Description("软删除标记,true:删除,false:未删除,null:未设置(未删除)")]
    public virtual bool? IsDeleted { get; set; } = false;

    /// <summary>
    /// 删除时间
    /// </summary>
    [Description("删除时间")]
    public virtual long? DeleteTime { get; set; }

    /// <summary>
    /// 删除人Id
    /// </summary>
    [Description("删除人Id")]
    public virtual long? DeletedId { get; set; }

    /// <summary>
    /// 删除人姓名
    /// </summary>
    [Description("删除人姓名")]
    public virtual string? DeletedName { get; set; }

    /// <summary>
    /// 版本号（用于乐观锁）
    /// </summary>
    [Description("版本号（用于乐观锁）")]
    public virtual long? Version { get; set; } = 0;

    /// <summary>
    /// 是否启用该实体或功能的标识
    /// </summary>
    /// <value>
    /// true表示启用，false表示禁用，null表示未设置
    /// </value>
    [Description("是否启用,true:启用，false:禁用，null:未设置(启用)")]
    public virtual bool? IsEnabled { get; set; }
}