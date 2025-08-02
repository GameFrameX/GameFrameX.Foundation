using System.ComponentModel.DataAnnotations;

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 框架实体基类Id
/// </summary>
public abstract class EntityBaseId : IEntity<long>
{
    /// <summary>
    /// Id
    /// </summary>
    [Key]
    [Required]
    [Editable(false, AllowInitialValue = true)]
    public virtual long Id { get; set; }
}

/// <summary>
/// 泛型实体基类Id
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class EntityBaseId<TKey> : IEntity<TKey>
{
    /// <summary>
    /// Id
    /// </summary>
    [Key]
    [Required]
    [Editable(false, AllowInitialValue = true)]
    public virtual TKey Id { get; set; } = default!;
}
