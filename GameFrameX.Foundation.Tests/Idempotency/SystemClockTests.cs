using System.Threading;
using GameFrameX.Foundation.Idempotency;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 默认时钟测试：与 Utility 包 TimerHelper 的单一时间源一致性。
/// </summary>
public class SystemClockTests
{
    [Fact]
    public void UtcNowTime_ShouldStayConsistentWithTimerHelperUnixTimeMilliseconds()
    {
        // 安排
        SystemClock clock = SystemClock.Instance;

        // 执行：紧邻两次取时
        long helperTimeBefore = TimerHelper.UnixTimeMilliseconds();
        long clockTime = clock.UtcNowTime;
        long helperTimeAfter = TimerHelper.UnixTimeMilliseconds();

        // 断言：协调器时钟读数落在 TimerHelper 前后读数区间内（毫秒级窗口）
        Assert.InRange(clockTime, helperTimeBefore, helperTimeAfter + 1);
    }

    [Fact]
    public void Instance_ShouldBeSharedSingleton()
    {
        // 安排 + 执行 + 断言：全局单例
        Assert.Same(SystemClock.Instance, SystemClock.Instance);
    }
}
