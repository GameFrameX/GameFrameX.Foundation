namespace GameFrameX.Foundation.Orm.Entity.Filter;

/// <summary>
/// 更新标记
/// </summary>
public interface ISafeUpdateFilter
{
    /// <summary>
    /// 更新次数
    /// </summary>
    int? UpdateCount { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    long? UpdateTime { get; set; }

    /// <summary>
    /// 更新人Id
    /// </summary>
    long? UpdatedId { get; set; }

    /// <summary>
    /// 更新人姓名
    /// </summary>
    string? UpdatedName { get; set; }
}