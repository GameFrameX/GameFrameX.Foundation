using System;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// TimerHelper.TimeOffset.UTC.cs / TimeOffset.TimeZone.cs 单元测试：
    /// 覆盖 UnixTimeSecondsWithOffset / UnixTimeMillisecondsWithOffset（已废弃但保留）以及
    /// UnixTimeSecondsWithTimeZoneOffset / UnixTimeMillisecondsWithTimeZoneOffset。
    /// </summary>
#pragma warning disable CS0618 // 测试故意调用已废弃的 UnixTimeSecondsWithOffset / UnixTimeMillisecondsWithOffset
    [Collection("TimerHelper")]
    public class TimerHelperTimeOffsetAdditionalTests : IDisposable
    {
        private static readonly DateTime FixedUtcNow =
            new DateTime(2024, 6, 15, 10, 30, 45, DateTimeKind.Utc);

        private sealed class FakeTimeProvider : TimeProvider
        {
            private readonly DateTimeOffset _utcNow;

            public FakeTimeProvider(DateTimeOffset utcNow)
            {
                _utcNow = utcNow;
            }

            public override DateTimeOffset GetUtcNow()
            {
                return _utcNow;
            }
        }

        public TimerHelperTimeOffsetAdditionalTests()
        {
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
            TimerHelper.SetTimeProvider(new FakeTimeProvider(new DateTimeOffset(FixedUtcNow)));
        }

        public void Dispose()
        {
            TimerHelper.SetTimeProvider(null);
            TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
            TimerHelper.ResetTimeOffset();
        }

        #region UnixTimeSecondsWithOffset / UnixTimeMillisecondsWithOffset (已废弃)

        [Fact]
        public void UnixTimeSecondsWithOffset_ShouldEqualUnixTimeSecondsByDefault()
        {
            Assert.Equal(TimerHelper.UnixTimeSeconds(), TimerHelper.UnixTimeSecondsWithOffset());
        }

        [Fact]
        public void UnixTimeMillisecondsWithOffset_ShouldEqualUnixTimeMillisecondsByDefault()
        {
            Assert.Equal(TimerHelper.UnixTimeMilliseconds(), TimerHelper.UnixTimeMillisecondsWithOffset());
        }

        [Fact]
        public void UnixTimeSecondsWithOffset_ShouldIncludeTimeOffsetSeconds()
        {
            TimerHelper.SetTimeOffset(120L, 120000L);
            var baseline = new DateTimeOffset(FixedUtcNow).ToUnixTimeSeconds();

            Assert.Equal(baseline + 120L, TimerHelper.UnixTimeSecondsWithOffset());
        }

        [Fact]
        public void UnixTimeMillisecondsWithOffset_ShouldIncludeTimeOffsetMilliseconds()
        {
            TimerHelper.SetTimeOffset(0L, 1234L);
            var baseline = new DateTimeOffset(FixedUtcNow).ToUnixTimeMilliseconds();

            Assert.Equal(baseline + 1234L, TimerHelper.UnixTimeMillisecondsWithOffset());
        }

        [Fact]
        public void UnixTimeSecondsWithOffset_NegativeOffset_ShouldReduceTimestamp()
        {
            TimerHelper.SetTimeOffset(-60L, -60000L);
            var baseline = new DateTimeOffset(FixedUtcNow).ToUnixTimeSeconds();

            Assert.Equal(baseline - 60L, TimerHelper.UnixTimeSecondsWithOffset());
        }

        #endregion

        #region UnixTimeSecondsWithTimeZoneOffset / UnixTimeMillisecondsWithTimeZoneOffset

        [Fact]
        public void UnixTimeSecondsWithTimeZoneOffset_WithPlus8_ShouldBeStableWithinAbsoluteInstant()
        {
            // 验证 TimeOffset 能正确叠加；不与 baseline 绝对比较以避免环境差异
            var zone = TimeZoneInfo.CreateCustomTimeZone("Test+8", TimeSpan.FromHours(8), "Test+8", "Test+8");
            TimerHelper.SetTimeZone(zone);
            var baseline = TimerHelper.UnixTimeSecondsWithTimeZoneOffset();

            TimerHelper.SetTimeOffset(500L, 500000L);

            Assert.Equal(baseline + 500L, TimerHelper.UnixTimeSecondsWithTimeZoneOffset());
        }

        [Fact]
        public void UnixTimeMillisecondsWithTimeZoneOffset_WithPlus8_ShouldBeStableWithinAbsoluteInstant()
        {
            // 验证 TimeOffset 能正确叠加；不与 baseline 绝对比较以避免环境差异
            var zone = TimeZoneInfo.CreateCustomTimeZone("Test+8", TimeSpan.FromHours(8), "Test+8", "Test+8");
            TimerHelper.SetTimeZone(zone);
            var baseline = TimerHelper.UnixTimeMillisecondsWithTimeZoneOffset();

            TimerHelper.SetTimeOffset(0L, 1234L);

            Assert.Equal(baseline + 1234L, TimerHelper.UnixTimeMillisecondsWithTimeZoneOffset());
        }

        #endregion
    }
}
#pragma warning restore CS0618
