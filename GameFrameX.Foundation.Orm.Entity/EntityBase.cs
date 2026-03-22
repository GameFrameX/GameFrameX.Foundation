// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameFrameX.Foundation.Orm.Entity.Filter;

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 框架实体基类。
/// </summary>
/// <remarks>
/// Framework entity base class.
/// </remarks>
public abstract class EntityBase : EntityBaseId, ISafeDeletedFilter, IVersionedEntity, ISafeEnabledFilter, ISafeCreatedFilter, ISafeUpdateFilter
{
    /// <summary>
    /// 获取或设置创建人Id。
    /// </summary>
    /// <remarks>
    /// Gets or sets the creator ID.
    /// </remarks>
    /// <value>创建人Id / Creator ID</value>
    [Description("创建人Id")]
    public virtual long? CreatedId { get; set; }

    /// <summary>
    /// 获取或设置创建时间。
    /// </summary>
    /// <remarks>
    /// Gets or sets the creation time.
    /// </remarks>
    /// <value>创建时间 / Creation time</value>
    [Description("创建时间")]
    public virtual long CreatedTime { get; set; }

    /// <summary>
    /// 获取或设置创建人姓名。
    /// </summary>
    /// <remarks>
    /// Gets or sets the creator name.
    /// </remarks>
    /// <value>创建人姓名 / Creator name</value>
    [Description("创建人姓名")]
    [MaxLength(512)]
    public virtual string? CreatedName { get; set; }

    /// <summary>
    /// 获取或设置更新次数。
    /// </summary>
    /// <remarks>
    /// Gets or sets the update count.
    /// </remarks>
    /// <value>更新次数 / Update count</value>
    [Description("更新次数")]
    public virtual int? UpdateCount { get; set; }

    /// <summary>
    /// 获取或设置更新时间。
    /// </summary>
    /// <remarks>
    /// Gets or sets the update time.
    /// </remarks>
    /// <value>更新时间 / Update time</value>
    [Description("更新时间")]
    public virtual long? UpdateTime { get; set; }

    /// <summary>
    /// 获取或设置更新人Id。
    /// </summary>
    /// <remarks>
    /// Gets or sets the updater ID.
    /// </remarks>
    /// <value>更新人Id / Updater ID</value>
    [Description("更新人Id")]
    public virtual long? UpdatedId { get; set; }

    /// <summary>
    /// 获取或设置更新人姓名。
    /// </summary>
    /// <remarks>
    /// Gets or sets the updater name.
    /// </remarks>
    /// <value>更新人姓名 / Updater name</value>
    [Description("更新人姓名")]
    [MaxLength(512)]
    public virtual string? UpdatedName { get; set; }

    /// <summary>
    /// 获取或设置软删除标记。
    /// </summary>
    /// <remarks>
    /// Gets or sets the soft delete flag.
    /// </remarks>
    /// <value><c>true</c> 表示已删除；<c>false</c> 表示未删除；<c>null</c> 表示未设置（未删除） / <c>true</c> for deleted; <c>false</c> for not deleted; <c>null</c> for unset (not deleted)</value>
    [Description("软删除标记,true:删除,false:未删除,null:未设置(未删除)")]
    public virtual bool? IsDeleted { get; set; } = false;

    /// <summary>
    /// 获取或设置删除时间。
    /// </summary>
    /// <remarks>
    /// Gets or sets the deletion time.
    /// </remarks>
    /// <value>删除时间 / Deletion time</value>
    [Description("删除时间")]
    public virtual long? DeleteTime { get; set; }

    /// <summary>
    /// 获取或设置删除人Id。
    /// </summary>
    /// <remarks>
    /// Gets or sets the deleter ID.
    /// </remarks>
    /// <value>删除人Id / Deleter ID</value>
    [Description("删除人Id")]
    public virtual long? DeletedId { get; set; }

    /// <summary>
    /// 获取或设置删除人姓名。
    /// </summary>
    /// <remarks>
    /// Gets or sets the deleter name.
    /// </remarks>
    /// <value>删除人姓名 / Deleter name</value>
    [Description("删除人姓名")]
    [MaxLength(512)]
    public virtual string? DeletedName { get; set; }

    /// <summary>
    /// 获取或设置版本号（用于乐观锁）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the version number (used for optimistic locking).
    /// </remarks>
    /// <value>版本号（用于乐观锁） / Version number (for optimistic locking)</value>
    [Description("版本号（用于乐观锁）")]
    public virtual long? Version { get; set; } = 0;

    /// <summary>
    /// 获取或设置是否启用该实体或功能的标识。
    /// </summary>
    /// <remarks>
    /// Gets or sets the flag indicating whether the entity or feature is enabled.
    /// </remarks>
    /// <value><c>true</c> 表示启用；<c>false</c> 表示禁用；<c>null</c> 表示未设置（启用） / <c>true</c> for enabled; <c>false</c> for disabled; <c>null</c> for unset (enabled)</value>
    [Description("是否启用,true:启用，false:禁用，null:未设置(启用)")]
    public virtual bool? IsEnabled { get; set; }
}

/// <summary>
/// 泛型框架实体基类。
/// </summary>
/// <remarks>
/// Generic framework entity base class.
/// </remarks>
/// <typeparam name="TKey">主键类型 / Primary key type</typeparam>
public abstract class EntityBase<TKey> : EntityBaseId<TKey>, ISafeDeletedFilter, IVersionedEntity, ISafeEnabledFilter, ISafeCreatedFilter, ISafeUpdateFilter
    where TKey : notnull
{
    /// <summary>
    /// 获取或设置创建人Id。
    /// </summary>
    /// <remarks>
    /// Gets or sets the creator ID.
    /// </remarks>
    /// <value>创建人Id / Creator ID</value>
    [Description("创建人Id")]
    public virtual long? CreatedId { get; set; }

    /// <summary>
    /// 获取或设置创建时间。
    /// </summary>
    /// <remarks>
    /// Gets or sets the creation time.
    /// </remarks>
    /// <value>创建时间 / Creation time</value>
    [Description("创建时间")]
    public virtual long CreatedTime { get; set; }

    /// <summary>
    /// 获取或设置创建人姓名。
    /// </summary>
    /// <remarks>
    /// Gets or sets the creator name.
    /// </remarks>
    /// <value>创建人姓名 / Creator name</value>
    [Description("创建人姓名")]
    [MaxLength(512)]
    public virtual string? CreatedName { get; set; }

    /// <summary>
    /// 获取或设置更新次数。
    /// </summary>
    /// <remarks>
    /// Gets or sets the update count.
    /// </remarks>
    /// <value>更新次数 / Update count</value>
    [Description("更新次数")]
    public virtual int? UpdateCount { get; set; }

    /// <summary>
    /// 获取或设置更新时间。
    /// </summary>
    /// <remarks>
    /// Gets or sets the update time.
    /// </remarks>
    /// <value>更新时间 / Update time</value>
    [Description("更新时间")]
    public virtual long? UpdateTime { get; set; }

    /// <summary>
    /// 获取或设置更新人Id。
    /// </summary>
    /// <remarks>
    /// Gets or sets the updater ID.
    /// </remarks>
    /// <value>更新人Id / Updater ID</value>
    [Description("更新人Id")]
    public virtual long? UpdatedId { get; set; }

    /// <summary>
    /// 获取或设置更新人姓名。
    /// </summary>
    /// <remarks>
    /// Gets or sets the updater name.
    /// </remarks>
    /// <value>更新人姓名 / Updater name</value>
    [Description("更新人姓名")]
    [MaxLength(512)]
    public virtual string? UpdatedName { get; set; }

    /// <summary>
    /// 获取或设置软删除标记。
    /// </summary>
    /// <remarks>
    /// Gets or sets the soft delete flag.
    /// </remarks>
    /// <value><c>true</c> 表示已删除；<c>false</c> 表示未删除；<c>null</c> 表示未设置（未删除） / <c>true</c> for deleted; <c>false</c> for not deleted; <c>null</c> for unset (not deleted)</value>
    [Description("软删除标记,true:删除,false:未删除,null:未设置(未删除)")]
    public virtual bool? IsDeleted { get; set; } = false;

    /// <summary>
    /// 获取或设置删除时间。
    /// </summary>
    /// <remarks>
    /// Gets or sets the deletion time.
    /// </remarks>
    /// <value>删除时间 / Deletion time</value>
    [Description("删除时间")]
    public virtual long? DeleteTime { get; set; }

    /// <summary>
    /// 获取或设置删除人Id。
    /// </summary>
    /// <remarks>
    /// Gets or sets the deleter ID.
    /// </remarks>
    /// <value>删除人Id / Deleter ID</value>
    [Description("删除人Id")]
    public virtual long? DeletedId { get; set; }

    /// <summary>
    /// 获取或设置删除人姓名。
    /// </summary>
    /// <remarks>
    /// Gets or sets the deleter name.
    /// </remarks>
    /// <value>删除人姓名 / Deleter name</value>
    [Description("删除人姓名")]
    [MaxLength(512)]
    public virtual string? DeletedName { get; set; }

    /// <summary>
    /// 获取或设置版本号（用于乐观锁）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the version number (used for optimistic locking).
    /// </remarks>
    /// <value>版本号（用于乐观锁） / Version number (for optimistic locking)</value>
    [Description("版本号（用于乐观锁）")]
    public virtual long? Version { get; set; } = 0;

    /// <summary>
    /// 获取或设置是否启用该实体或功能的标识。
    /// </summary>
    /// <remarks>
    /// Gets or sets the flag indicating whether the entity or feature is enabled.
    /// </remarks>
    /// <value><c>true</c> 表示启用；<c>false</c> 表示禁用；<c>null</c> 表示未设置（启用） / <c>true</c> for enabled; <c>false</c> for disabled; <c>null</c> for unset (enabled)</value>
    [Description("是否启用,true:启用，false:禁用，null:未设置(启用)")]
    public virtual bool? IsEnabled { get; set; }
}
