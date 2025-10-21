namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// 表示系统时钟异常的异常类
/// </summary>
/// <remarks>
/// 当系统时钟出现回退或其他时钟相关问题时抛出此异常。
/// 在雪花算法中，时钟回退会导致ID生成出现重复，因此需要特殊处理。
/// </remarks>
/// <example>
/// <code>
/// if (currentTimestamp &lt; lastTimestamp)
/// {
///     throw new InvalidSystemClock("系统时钟出现回退");
/// }
/// </code>
/// </example>
/// <seealso cref="IdWorker"/>
public class InvalidSystemClock : Exception
{
    /// <summary>
    /// 使用指定的错误消息初始化 <see cref="InvalidSystemClock"/> 类的新实例
    /// </summary>
    /// <param name="message">描述错误的消息</param>
    /// <remarks>
    /// 该构造函数调用基类 <see cref="Exception"/> 的构造函数来设置异常消息
    /// </remarks>
    public InvalidSystemClock(string message) : base(message)
    {
    }
}