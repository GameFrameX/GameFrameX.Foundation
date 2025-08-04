using System.ComponentModel.DataAnnotations;

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 框架实体基类Id
/// </summary>
public abstract class EntityBaseId : IEntity<long>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Key]
    [Required]
    [Editable(false, AllowInitialValue = true)]
    [System.ComponentModel.Description("主键Id")]
    public virtual long Id { get; set; }
}

/// <summary>
/// 泛型实体基类Id
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class EntityBaseId<TKey> : IEntity<TKey>
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Key]
    [Required]
    [Editable(false, AllowInitialValue = true)]
    [System.ComponentModel.Description("主键Id")]
    public virtual TKey Id { get; set; } = default!;
}