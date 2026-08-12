using System;
using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimeSystem（TimeExtensions.cs）单元测试：
    /// 覆盖 CurrentTimeMillis、CurrentTimeFunc、StubCurrentTime(Func&lt;long&gt;)、StubCurrentTime(long)。
    /// 验证默认实现返回真实 UTC 时间、可替换为自定义函数、作用域结束自动恢复。
    /// </summary>
    [Collection("TimerHelper")]
    public class TimeSystemTests
    {
        #region CurrentTimeMillis 默认行为

        [Fact]
        public void CurrentTimeMillis_Default_ShouldReturnCloseToDateTimeUtcNow()
        {
            var before = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            var actual = TimeSystem.CurrentTimeMillis();
            var after = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            Assert.InRange(actual, before, after + 1);
        }

        [Fact]
        public void CurrentTimeMillis_ShouldBeMonotonicallyIncreasingInSameCall()
        {
            var first = TimeSystem.CurrentTimeMillis();
            var second = TimeSystem.CurrentTimeMillis();

            // 通常第二次 >= 第一次
            Assert.True(second >= first);
        }

        #endregion

        #region CurrentTimeFunc

        [Fact]
        public void CurrentTimeFunc_Get_ShouldReturnNonNullByDefault()
        {
            Assert.NotNull(TimeSystem.CurrentTimeFunc);
        }

        [Fact]
        public void CurrentTimeFunc_Set_ShouldUpdateFunction()
        {
            Func<long> original = TimeSystem.CurrentTimeFunc;

            try
            {
                TimeSystem.CurrentTimeFunc = () => 12345L;

                Assert.Equal(12345L, TimeSystem.CurrentTimeMillis());
            }
            finally
            {
                TimeSystem.CurrentTimeFunc = original;
            }
        }

        [Fact]
        public void CurrentTimeFunc_MultipleSets_LastOneWins()
        {
            Func<long> original = TimeSystem.CurrentTimeFunc;

            try
            {
                TimeSystem.CurrentTimeFunc = () => 1L;
                TimeSystem.CurrentTimeFunc = () => 2L;
                TimeSystem.CurrentTimeFunc = () => 3L;

                Assert.Equal(3L, TimeSystem.CurrentTimeMillis());
            }
            finally
            {
                TimeSystem.CurrentTimeFunc = original;
            }
        }

        #endregion

        #region StubCurrentTime(Func<long>)

        [Fact]
        public void StubCurrentTime_WithFunc_ShouldReplaceTimeFunction()
        {
            const long fixedTime = 1700000000000L;

            using (TimeSystem.StubCurrentTime(() => fixedTime))
            {
                Assert.Equal(fixedTime, TimeSystem.CurrentTimeMillis());
                Assert.Equal(fixedTime, TimeSystem.CurrentTimeMillis());
            }
        }

        [Fact]
        public void StubCurrentTime_WithFunc_AfterDispose_ShouldRestoreOriginal()
        {
            var beforeStub = TimeSystem.CurrentTimeMillis();

            using (TimeSystem.StubCurrentTime(() => 999L))
            {
                Assert.Equal(999L, TimeSystem.CurrentTimeMillis());
            }

            var afterStub = TimeSystem.CurrentTimeMillis();

            // 恢复后应该回到真实时间附近
            Assert.True(afterStub >= beforeStub - 1);
        }

        [Fact]
        public void StubCurrentTime_WithFunc_ShouldReturnDisposable()
        {
            var disposable = TimeSystem.StubCurrentTime(() => 0L);

            Assert.NotNull(disposable);

            disposable.Dispose();
        }

        [Fact]
        public void StubCurrentTime_NestedStubs_ShouldRestoreInLifoOrder()
        {
            var original = TimeSystem.CurrentTimeFunc;

            try
            {
                using (TimeSystem.StubCurrentTime(() => 100L))
                {
                    Assert.Equal(100L, TimeSystem.CurrentTimeMillis());

                    using (TimeSystem.StubCurrentTime(() => 200L))
                    {
                        Assert.Equal(200L, TimeSystem.CurrentTimeMillis());
                    }

                    // 内层 dispose 后应恢复到 100L
                    Assert.Equal(100L, TimeSystem.CurrentTimeMillis());
                }

                // 外层 dispose 后应恢复到原始
                var current = TimeSystem.CurrentTimeMillis();
                Assert.True(current > 1000L);
            }
            finally
            {
                TimeSystem.CurrentTimeFunc = original;
            }
        }

        #endregion

        #region StubCurrentTime(long)

        [Fact]
        public void StubCurrentTime_WithLongValue_ShouldReturnFixedValue()
        {
            const long fixedTime = 1718447400000L;

            using (TimeSystem.StubCurrentTime(fixedTime))
            {
                Assert.Equal(fixedTime, TimeSystem.CurrentTimeMillis());
            }
        }

        [Fact]
        public void StubCurrentTime_WithZero_ShouldReturnZero()
        {
            using (TimeSystem.StubCurrentTime(0L))
            {
                Assert.Equal(0L, TimeSystem.CurrentTimeMillis());
            }
        }

        [Fact]
        public void StubCurrentTime_WithNegative_ShouldReturnNegative()
        {
            using (TimeSystem.StubCurrentTime(-1L))
            {
                Assert.Equal(-1L, TimeSystem.CurrentTimeMillis());
            }
        }

        [Fact]
        public void StubCurrentTime_WithLong_AfterDispose_ShouldRestore()
        {
            long beforeStub = TimeSystem.CurrentTimeMillis();

            using (TimeSystem.StubCurrentTime(1L))
            {
                Assert.Equal(1L, TimeSystem.CurrentTimeMillis());
            }

            long afterStub = TimeSystem.CurrentTimeMillis();

            Assert.True(afterStub >= beforeStub - 1);
        }

        #endregion

        #region 与 IdWorker 集成

        [Fact]
        public void StubCurrentTime_WithIdWorker_ShouldControlGeneratedTimestamp()
        {
            const long fixedTime = 1750000000000L;
            var worker = new IdWorker(1, 1, 1700000000000L);

            using (TimeSystem.StubCurrentTime(fixedTime))
            {
                var id = worker.NextId();
                var info = SnowFlakeIdParser.Parse(id, 1700000000000L);

                Assert.InRange(info.Timestamp.ToUnixTimeMilliseconds(),
                    fixedTime,
                    fixedTime);
            }
        }

        #endregion
    }
}
