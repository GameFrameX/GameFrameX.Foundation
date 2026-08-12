using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

/// <summary>
/// DisposableAction 边界单元测试：覆盖 null action 构造、Dispose 执行一次、
/// 多次 Dispose 幂等性（Interlocked.CompareExchange）、不同实例互不影响。
/// </summary>
public sealed class DisposableActionTests
{
    [Fact]
    public void Constructor_NullAction_ShouldThrowArgumentNullException()
    {
        // Arrange
        Action nullAction = null;

        // Act + Assert
        Assert.Throws<ArgumentNullException>(() => new DisposableAction(nullAction));
    }

    [Fact]
    public void Dispose_ShouldExecuteActionExactlyOnce()
    {
        // Arrange
        var executedCount = 0;
        var disposable = new DisposableAction(() => executedCount++);

        // Act
        disposable.Dispose();

        // Assert
        Assert.Equal(1, executedCount);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldBeIdempotent()
    {
        // Arrange
        var executedCount = 0;
        var disposable = new DisposableAction(() => executedCount++);

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        // 源码使用 Interlocked.CompareExchange(ref _disposed, 1, 0) 保证幂等
        Assert.Equal(1, executedCount);
    }

    [Fact]
    public void Dispose_DifferentInstances_ShouldNotInterfere()
    {
        // Arrange
        var firstCount = 0;
        var secondCount = 0;
        var first = new DisposableAction(() => firstCount++);
        var second = new DisposableAction(() => secondCount++);

        // Act - 仅释放第一个实例
        first.Dispose();

        // Assert - 第二个实例的动作不应被触发
        Assert.Equal(1, firstCount);
        Assert.Equal(0, secondCount);

        // Act - 再释放第二个实例
        second.Dispose();

        // Assert - 两个实例各自独立计数
        Assert.Equal(1, firstCount);
        Assert.Equal(1, secondCount);

        // Act - 再次释放两个实例（幂等）
        first.Dispose();
        second.Dispose();

        // Assert - 计数不应变化
        Assert.Equal(1, firstCount);
        Assert.Equal(1, secondCount);
    }

    [Fact]
    public void Dispose_UsingBlock_ShouldExecuteActionOnScopeExit()
    {
        // Arrange
        var executedCount = 0;

        // Act
        using (var disposable = new DisposableAction(() => executedCount++))
        {
            Assert.Equal(0, executedCount);
        }

        // Assert - 离开 using 作用域后动作应执行一次
        Assert.Equal(1, executedCount);
    }
}
