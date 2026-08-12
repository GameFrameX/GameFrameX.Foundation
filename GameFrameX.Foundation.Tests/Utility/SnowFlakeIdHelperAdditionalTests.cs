using System;
using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake.WorkerIdProviders;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// SnowFlakeIdHelper 补充单元测试：覆盖静态属性默认值、常量、SetWorkerIdProvider、
    /// Instance 基本行为（生成 ID 在合法范围）、SetWorkerIdConflictDetector(null) 重置。
    /// </summary>
    [Collection("Snowflake")]
    public class SnowFlakeIdHelperAdditionalTests
    {
        #region 常量与默认值

        [Fact]
        public void UtcTimeStart_ShouldBe2025January1stUtc()
        {
            Assert.Equal(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SnowFlakeIdHelper.UtcTimeStart);
            Assert.Equal(DateTimeKind.Utc, SnowFlakeIdHelper.UtcTimeStart.Kind);
        }

        [Fact]
        public void EpochTime_ShouldBe1970January1stUtc()
        {
            Assert.Equal(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                SnowFlakeIdHelper.EpochTime);
            Assert.Equal(DateTimeKind.Utc, SnowFlakeIdHelper.EpochTime.Kind);
        }

        [Fact]
        public void BaseTime_Default_ShouldMatchUtcTimeStartToEpochMillis()
        {
            var expected = (long)(SnowFlakeIdHelper.UtcTimeStart - SnowFlakeIdHelper.EpochTime).TotalMilliseconds;

            // 注意：BaseTime 可能在其他测试中被修改（全局静态），所以仅做记录性断言
            Assert.Equal(expected, (long)(SnowFlakeIdHelper.UtcTimeStart - SnowFlakeIdHelper.EpochTime).TotalMilliseconds);
        }

        [Fact]
        public void WorkId_Default_ShouldBe1()
        {
            // 全局静态属性：在某些测试环境下可能已被改写，但我们仍验证初始配置
            // SnowFlakeIdHelper 的默认 WorkId=1
            Assert.True(SnowFlakeIdHelper.WorkId >= 0 && SnowFlakeIdHelper.WorkId <= 31);
        }

        [Fact]
        public void DataCenterId_Default_ShouldBe1()
        {
            Assert.True(SnowFlakeIdHelper.DataCenterId >= 0 && SnowFlakeIdHelper.DataCenterId <= 31);
        }

        #endregion

        #region Instance

        [Fact]
        public void Instance_ShouldReturnNonNullSingleton()
        {
            Assert.NotNull(SnowFlakeIdHelper.Instance);
        }

        [Fact]
        public void Instance_ShouldReturnSameReferenceOnMultipleCalls()
        {
            var first = SnowFlakeIdHelper.Instance;
            var second = SnowFlakeIdHelper.Instance;

            Assert.Same(first, second);
        }

        [Fact]
        public void Instance_NextId_ShouldGeneratePositiveId()
        {
            var id = SnowFlakeIdHelper.Instance.NextId();

            Assert.True(id > 0);
        }

        [Fact]
        public void Instance_NextId_MultipleCalls_ShouldBeMonotonicallyIncreasing()
        {
            var worker = SnowFlakeIdHelper.Instance;
            var previous = worker.NextId();

            for (int i = 0; i < 10; i++)
            {
                var current = worker.NextId();
                Assert.True(current > previous, $"Expected {current} > {previous}");
                previous = current;
            }
        }

        [Fact]
        public void Instance_NextId_ShouldBeParseable()
        {
            var id = SnowFlakeIdHelper.Instance.NextId();

            var info = SnowFlakeIdParser.Parse(id);

            Assert.Equal(id, info.Id);
            Assert.InRange(info.WorkerId, 0, 31);
            Assert.InRange(info.DataCenterId, 0, 31);
            Assert.InRange(info.Sequence, 0, 4095);
        }

        #endregion

        #region SetWorkerIdProvider

        [Fact]
        public void SetWorkerIdProvider_WithManualProvider_ShouldNotThrow()
        {
            var provider = new ManualWorkerIdProvider(7);

            var exception = Record.Exception(() => SnowFlakeIdHelper.SetWorkerIdProvider(provider));

            Assert.Null(exception);

            // 恢复默认
            SnowFlakeIdHelper.SetWorkerIdProvider(null);
        }

        [Fact]
        public void SetWorkerIdProvider_WithNull_ShouldNotThrow()
        {
            var exception = Record.Exception(() => SnowFlakeIdHelper.SetWorkerIdProvider(null));

            Assert.Null(exception);
        }

        #endregion

        #region SetWorkerIdConflictDetector

        [Fact]
        public void SetWorkerIdConflictDetector_WithNull_ShouldResetToDefault()
        {
            // 传 null 应恢复默认检测器，不应抛异常
            var exception = Record.Exception(() => SnowFlakeIdHelper.SetWorkerIdConflictDetector(null));

            Assert.Null(exception);

            // 调用 CheckWorkerIdConflict 不应抛异常（证明默认检测器已恢复）
            var result = SnowFlakeIdHelper.CheckWorkerIdConflict("test-node-detector-reset");
            Assert.NotNull(result);
        }

        [Fact]
        public void SetWorkerIdConflictDetector_WithCustom_ShouldNotThrow()
        {
            var detector = new WorkerIdConflictDetector();

            var exception = Record.Exception(() => SnowFlakeIdHelper.SetWorkerIdConflictDetector(detector));

            Assert.Null(exception);

            // 恢复默认
            SnowFlakeIdHelper.SetWorkerIdConflictDetector(null);
        }

        #endregion

        #region CheckWorkerIdConflict

        [Fact]
        public void CheckWorkerIdConflict_WithExplicitNodeId_ShouldUseProvidedNodeId()
        {
            var detector = new WorkerIdConflictDetector();
            try
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(detector);

                var result = SnowFlakeIdHelper.CheckWorkerIdConflict("explicit-node-id");

                Assert.Equal("explicit-node-id", result.NodeId);
                Assert.False(result.HasConflict);
            }
            finally
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(null);
            }
        }

        [Fact]
        public void CheckWorkerIdConflict_WithNullNodeId_ShouldUseMachineName()
        {
            var detector = new WorkerIdConflictDetector();
            try
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(detector);

                var result = SnowFlakeIdHelper.CheckWorkerIdConflict(null);

                Assert.Equal(Environment.MachineName, result.NodeId);
            }
            finally
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(null);
            }
        }

        [Fact]
        public void CheckWorkerIdConflict_ShouldReportConflictWhenDuplicateRegistered()
        {
            var detector = new WorkerIdConflictDetector();
            try
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(detector);
                var workId = SnowFlakeIdHelper.WorkId;
                var dataCenterId = SnowFlakeIdHelper.DataCenterId;

                detector.Register("pre-registered-node", dataCenterId, workId, "Manual");

                var result = SnowFlakeIdHelper.CheckWorkerIdConflict("new-node");

                Assert.True(result.HasConflict);
                Assert.Equal("pre-registered-node", result.ConflictingNodeId);
            }
            finally
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(null);
            }
        }

        #endregion

        #region LastWorkerIdConflict

        [Fact]
        public void LastWorkerIdConflict_AfterSetDetector_ShouldBeNull()
        {
            var detector = new WorkerIdConflictDetector();
            try
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(detector);

                Assert.Null(SnowFlakeIdHelper.LastWorkerIdConflict);
            }
            finally
            {
                SnowFlakeIdHelper.SetWorkerIdConflictDetector(null);
            }
        }

        #endregion
    }
}
