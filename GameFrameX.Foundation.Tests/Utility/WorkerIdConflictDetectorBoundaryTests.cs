using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

/// <summary>
/// WorkerIdConflictDetector 边界单元测试：覆盖 Validate 异常路径（nodeId 空白、dataCenterId/workerId 越界）、
/// 边界值（0/31）正常通过、Clear 清空注册表、并发 Register 线程安全性。
/// </summary>
[Collection("Snowflake")]
public sealed class WorkerIdConflictDetectorBoundaryTests
{
    #region Validate 异常路径（通过公共 Check/Register 方法触发）

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    public void Check_NodeIdNullOrWhitespace_ShouldThrowArgumentException(string nodeId)
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act + Assert
        Assert.Throws<ArgumentException>(() => detector.Check(nodeId, 1, 1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    public void Check_DataCenterIdOutOfRange_ShouldThrowArgumentOutOfRangeException(long dataCenterId)
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => detector.Check("node", dataCenterId, 1));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    public void Check_WorkerIdOutOfRange_ShouldThrowArgumentOutOfRangeException(long workerId)
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => detector.Check("node", 1, workerId));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(32)]
    public void Register_DataCenterIdOutOfRange_ShouldThrowArgumentOutOfRangeException(long dataCenterId)
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act + Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => detector.Register("node", dataCenterId, 1));
    }

    #endregion

    #region 边界值 0 和 31 不抛异常

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 31)]
    [InlineData(31, 0)]
    [InlineData(31, 31)]
    public void Check_BoundaryValues_ShouldNotThrow(long dataCenterId, long workerId)
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act
        var result = detector.Check("boundary-node", dataCenterId, workerId);

        // Assert
        Assert.False(result.HasConflict);
        Assert.Equal("boundary-node", result.NodeId);
        Assert.Equal(dataCenterId, result.DataCenterId);
        Assert.Equal(workerId, result.WorkerId);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(31, 31)]
    public void Register_BoundaryValues_ShouldSucceed(long dataCenterId, long workerId)
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act
        var result = detector.Register("boundary-node", dataCenterId, workerId);

        // Assert
        Assert.False(result.HasConflict);
        Assert.Single(detector.GetRegistrations());
    }

    #endregion

    #region Clear

    [Fact]
    public void Clear_AfterRegistrations_RegistryShouldBeEmpty()
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();
        detector.Register("node-a", 1, 1);
        detector.Register("node-b", 2, 2);
        detector.Register("node-c", 3, 3);
        Assert.Equal(3, detector.GetRegistrations().Count);

        // Act
        detector.Clear();

        // Assert
        Assert.Empty(detector.GetRegistrations());
    }

    [Fact]
    public void Clear_OnEmptyRegistry_ShouldNotThrow()
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();

        // Act + Assert
        var exception = Record.Exception(() => detector.Clear());
        Assert.Null(exception);
    }

    [Fact]
    public void Clear_AfterClear_CanRegisterAgain()
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();
        detector.Register("node-a", 1, 1);
        detector.Clear();

        // Act - 重新注册相同组合
        var result = detector.Register("node-a", 1, 1);

        // Assert
        Assert.False(result.HasConflict);
        Assert.Single(detector.GetRegistrations());
    }

    #endregion

    #region 并发 Register

    [Fact]
    public void Register_ConcurrentDifferentCombinations_ShouldRegisterAllSafely()
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();
        const int count = 100;

        // Act - 并发注册 100 个不同（dataCenterId, workerId）组合
        // dc = i / 32（0~3），w = i % 32（0~31），保证每个组合唯一
        Parallel.For(0, count, i =>
        {
            var dc = i / 32;
            var w = i % 32;
            var result = detector.Register($"node-{i}", dc, w);
            Assert.False(result.HasConflict, $"意外冲突: node-{i}");
        });

        // Assert - 100 个唯一组合全部注册成功
        Assert.Equal(count, detector.GetRegistrations().Count);
    }

    [Fact]
    public void Register_ConcurrentSameCombination_OnlyOneShouldWin()
    {
        // Arrange
        var detector = new WorkerIdConflictDetector();
        var successCount = 0;
        var lockObj = new object();

        // Act - 多线程竞争注册相同（dataCenterId, workerId）组合
        Parallel.For(0, 20, i =>
        {
            var result = detector.Register($"competing-node-{i}", 5, 5);
            if (!result.HasConflict)
            {
                lock (lockObj)
                {
                    successCount++;
                }
            }
        });

        // Assert - 注册表中应只有 1 个条目（首个成功注册者）
        Assert.Equal(1, detector.GetRegistrations().Count);
        Assert.Equal(1, successCount);
    }

    #endregion
}
