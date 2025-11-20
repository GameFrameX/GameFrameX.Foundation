namespace GameFrameX.Foundation.Orm.Entity.Filter;

/// <summary>
/// 更新标记
/// </summary>
public interface ISafeUpdateFilter
{
    /// <summary>
    /// 更新次数
    /// </summary>
    public int UpdateCount { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public long UpdateTime { get; set; }
}