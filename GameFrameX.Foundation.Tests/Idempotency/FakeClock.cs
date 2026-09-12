using System.Threading;
using GameFrameX.Foundation.Idempotency;
using GameFrameX.Foundation.Utility;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 测试用时钟：基于真实时间自动推进，支持叠加偏移实现时间快进。
/// </summary>
internal sealed class FakeClock : IClock
{
    private long _offsetMilliseconds;

    public long UtcNowTime
    {
        get
        {
            return TimerHelper.UnixTimeMilliseconds() + Interlocked.Read(ref _offsetMilliseconds);
        }
    }

    /// <summary>
    /// 将时钟快进指定毫秒数。
    /// </summary>
    public void AdvanceMilliseconds(long milliseconds)
    {
        Interlocked.Add(ref _offsetMilliseconds, milliseconds);
    }
}
