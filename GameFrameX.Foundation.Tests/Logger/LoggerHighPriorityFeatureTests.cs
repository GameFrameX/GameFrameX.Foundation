using GameFrameX.Foundation.Logger;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger;

public sealed class LoggerHighPriorityFeatureTests
{
    [Fact]
    public void CreateLoggerConfiguration_ShouldRedactDefaultSensitivePropertiesBeforeSink()
    {
        var sink = new CapturingSink();
        var logger = LogHandler.CreateLoggerConfiguration()
                               .WriteTo.Sink(sink)
                               .CreateLogger();

        logger.Information(
            "login {Password} {Token} {Authorization} {Cookie} {UserName}",
            "plain-password",
            "raw-token",
            "Bearer raw-token",
            "session=raw",
            "alice");

        var logEvent = Assert.Single(sink.Events);
        Assert.Equal("[REDACTED]", GetScalarValue(logEvent, "Password"));
        Assert.Equal("[REDACTED]", GetScalarValue(logEvent, "Token"));
        Assert.Equal("[REDACTED]", GetScalarValue(logEvent, "Authorization"));
        Assert.Equal("[REDACTED]", GetScalarValue(logEvent, "Cookie"));
        Assert.Equal("alice", GetScalarValue(logEvent, "UserName"));
    }

    [Fact]
    public void CreateLoggerConfiguration_ShouldRedactNestedSensitivePropertiesBeforeSink()
    {
        var sink = new CapturingSink();
        var logger = LogHandler.CreateLoggerConfiguration()
                               .WriteTo.Sink(sink)
                               .CreateLogger();

        logger.Information(
            "payload {@Payload}",
            new
            {
                UserName = "alice",
                Password = "plain-password",
                Metadata = new Dictionary<string, object>
                {
                    ["authorization"] = "Bearer raw-token",
                    ["safe"] = "visible",
                },
            });

        var logEvent = Assert.Single(sink.Events);
        var payload = Assert.IsType<StructureValue>(logEvent.Properties["Payload"]);
        Assert.Equal("alice", GetStructureScalarValue(payload, "UserName"));
        Assert.Equal("[REDACTED]", GetStructureScalarValue(payload, "Password"));

        var metadata = Assert.IsType<DictionaryValue>(payload.Properties.Single(property => property.Name == "Metadata").Value);
        Assert.Equal("[REDACTED]", GetDictionaryScalarValue(metadata, "authorization"));
        Assert.Equal("visible", GetDictionaryScalarValue(metadata, "safe"));
    }

    private static object GetScalarValue(LogEvent logEvent, string propertyName)
    {
        Assert.True(logEvent.Properties.TryGetValue(propertyName, out var propertyValue), $"Missing property {propertyName}");
        var scalar = Assert.IsType<ScalarValue>(propertyValue);
        return scalar.Value!;
    }

    private static object GetStructureScalarValue(StructureValue structureValue, string propertyName)
    {
        var property = structureValue.Properties.Single(item => item.Name == propertyName);
        var scalar = Assert.IsType<ScalarValue>(property.Value);
        return scalar.Value!;
    }

    private static object GetDictionaryScalarValue(DictionaryValue dictionaryValue, string key)
    {
        var element = dictionaryValue.Elements.Single(item => string.Equals(item.Key.Value as string, key, StringComparison.Ordinal));
        var scalar = Assert.IsType<ScalarValue>(element.Value);
        return scalar.Value!;
    }

    private sealed class CapturingSink : ILogEventSink
    {
        private readonly List<LogEvent> _events = new();

        public IReadOnlyList<LogEvent> Events
        {
            get { return _events; }
        }

        public void Emit(LogEvent logEvent)
        {
            _events.Add(logEvent);
        }
    }
}
