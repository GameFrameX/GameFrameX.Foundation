using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility;

public sealed class TimerHelperTimeProviderTests : IDisposable
{
    private sealed class FakeTimeProvider : TimeProvider
    {
        private DateTimeOffset _utcNow;

        public FakeTimeProvider(DateTimeOffset utcNow)
        {
            _utcNow = utcNow;
        }

        public void SetUtcNow(DateTimeOffset value)
        {
            _utcNow = value;
        }

        public override DateTimeOffset GetUtcNow()
        {
            return _utcNow;
        }
    }

    private readonly FakeTimeProvider _fakeProvider;
    private readonly DateTimeOffset _fixedTime = new DateTimeOffset(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);

    public TimerHelperTimeProviderTests()
    {
        _fakeProvider = new FakeTimeProvider(_fixedTime);
    }

    public void Dispose()
    {
        TimerHelper.SetTimeProvider(null);
        TimerHelper.SetTimeZone(TimeZoneInfo.Utc);
        TimerHelper.ResetTimeOffset();
    }

    [Fact]
    public void SetTimeProvider_GetNowWithUtc_ReturnsProviderTime()
    {
        TimerHelper.SetTimeProvider(_fakeProvider);

        var result = TimerHelper.GetNowWithUtc();

        Assert.Equal(_fixedTime.DateTime, result, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SetTimeProvider_UnixTimeSeconds_ReturnsExpectedTimestamp()
    {
        TimerHelper.SetTimeProvider(_fakeProvider);

        var result = TimerHelper.UnixTimeSeconds();
        var expected = new DateTimeOffset(_fixedTime.DateTime).ToUnixTimeSeconds();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SetTimeProvider_UnixTimeMilliseconds_ReturnsExpectedTimestamp()
    {
        TimerHelper.SetTimeProvider(_fakeProvider);

        var result = TimerHelper.UnixTimeMilliseconds();
        var expected = new DateTimeOffset(_fixedTime.DateTime).ToUnixTimeMilliseconds();

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SetTimeProvider_Null_RestoresDefaultBehavior()
    {
        TimerHelper.SetTimeProvider(_fakeProvider);
        Assert.Equal(_fixedTime.DateTime, TimerHelper.GetNowWithUtc(), TimeSpan.FromSeconds(1));

        TimerHelper.SetTimeProvider(null);

        var realUtcNow = DateTime.UtcNow;
        var result = TimerHelper.GetNowWithUtc();
        Assert.InRange(result, realUtcNow.AddSeconds(-2), realUtcNow.AddSeconds(2));
    }

    [Fact]
    public void SetTimeProvider_GetNowWithTimeZone_RespectsTimeZone()
    {
        var zone = TimeZoneInfo.CreateCustomTimeZone("Test/UTC+8", TimeSpan.FromHours(8), "Test+8", "Test+8");
        TimerHelper.SetTimeZone(zone);
        TimerHelper.SetTimeProvider(_fakeProvider);

        var result = TimerHelper.GetNowWithTimeZone();

        Assert.Equal(_fixedTime.AddHours(8).DateTime, result, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void SetTimeProvider_TimeOffset_StillApplies()
    {
        TimerHelper.SetTimeProvider(_fakeProvider);
        TimerHelper.SetTimeOffset(100, 100000);

        var seconds = TimerHelper.UnixTimeSeconds();
        var expected = new DateTimeOffset(_fixedTime.DateTime).ToUnixTimeSeconds() + 100;

        Assert.Equal(expected, seconds);
    }

    [Fact]
    public void GetTimeProvider_DefaultIsNull()
    {
        Assert.Null(TimerHelper.GetTimeProvider());
    }

    [Fact]
    public void SetTimeProvider_UpdatesGetTimeProvider()
    {
        TimerHelper.SetTimeProvider(_fakeProvider);

        Assert.Same(_fakeProvider, TimerHelper.GetTimeProvider());
    }
}
