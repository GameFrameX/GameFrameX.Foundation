using System;
using System.Collections.Generic;
using System.Reflection;
using GameFrameX.Foundation.Logger;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger
{
    /// <summary>
    /// LogHelper 六个日志级别（Debug / Info / Warning / Error / Fatal / Verbose）的核心 API 覆盖测试。
    /// 覆盖源文件：LogHelper.Debug.cs / LogHelper.Info.cs / LogHelper.Warning.cs /
    /// LogHelper.Error.cs / LogHelper.Fatal.cs / LogHelper.Verbose.cs。
    /// </summary>
    /// <remarks>
    /// LogHelper 为静态类，内部状态（_logger / _tempLogger）跨测试共享。
    /// 每个测试构造时重置静态状态并注入捕获 sink，Dispose 时再清理，避免污染其他测试。
    /// 标记 [Collection] 确保本类内测试串行执行。
    /// </remarks>
    [Collection(nameof(LogHelperSerialCollection))]
    public sealed class LogHelperLevelTests : IDisposable
    {
        private readonly CapturingSink _sink = new CapturingSink();

        public LogHelperLevelTests()
        {
            ResetLogHelperState();
            var logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.Sink(_sink)
                         .CreateLogger();
            LogHelper.SetLogger(logger);
        }

        public void Dispose()
        {
            ResetLogHelperState();
            GC.SuppressFinalize(this);
        }

        private LogEvent SingleEvent()
        {
            return Assert.Single(_sink.Events);
        }

        private static void ResetLogHelperState()
        {
            var tempField = typeof(LogHelper).GetField("_tempLogger", BindingFlags.NonPublic | BindingFlags.Static);
            var tempLogger = tempField?.GetValue(null);
            if (tempLogger is IDisposable disposable)
            {
                disposable.Dispose();
            }

            tempField?.SetValue(null, null);

            var loggerField = typeof(LogHelper).GetField("_logger", BindingFlags.NonPublic | BindingFlags.Static);
            loggerField?.SetValue(null, null);
        }

        // ============================================================
        // Debug (LogHelper.Debug.cs)
        // ============================================================

        [Fact]
        public void Debug_MessageTemplate_ShouldRecordAtDebugLevel()
        {
            LogHelper.Debug("hello debug");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Debug, evt.Level);
            Assert.Equal("hello debug", evt.RenderMessage());
        }

        [Fact]
        public void Debug_WithArgs_ShouldFormatAndRecordProperties()
        {
            LogHelper.Debug("user {Id} active", 42);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Debug, evt.Level);
            Assert.Contains("42", evt.RenderMessage());
            Assert.True(evt.Properties.ContainsKey("Id"));
        }

        [Fact]
        public void Debug_GenericSingle_ShouldRecordPropertyValue()
        {
            LogHelper.Debug("count {Count}", 99);
            var evt = SingleEvent();
            Assert.True(evt.Properties.ContainsKey("Count"));
        }

        [Fact]
        public void Debug_GenericPair_ShouldRecordBothProperties()
        {
            LogHelper.Debug("{A} plus {B}", 1, 2);
            var evt = SingleEvent();
            Assert.True(evt.Properties.ContainsKey("A"));
            Assert.True(evt.Properties.ContainsKey("B"));
        }

        [Fact]
        public void Debug_GenericEight_ShouldRecordAllProperties()
        {
            LogHelper.Debug("{A} {B} {C} {D} {E} {F} {G} {H}", 1, 2, 3, 4, 5, 6, 7, 8);
            var evt = SingleEvent();
            foreach (var name in new string[] { "A", "B", "C", "D", "E", "F", "G", "H" })
            {
                Assert.True(evt.Properties.ContainsKey(name), $"Missing property {name}");
            }
        }

        [Fact]
        public void Debug_WithException_ShouldAttachExceptionObject()
        {
            var ex = new InvalidOperationException("debug-boom");
            LogHelper.Debug(ex, "failed at {Step}", "init");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Debug, evt.Level);
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("Step"));
        }

        [Fact]
        public void Debug_WithExceptionAndParamsArray_ShouldAttachException()
        {
            var ex = new InvalidOperationException("multi-ex");
            LogHelper.Debug(ex, "ctx {X} {Y}", 1, 2);
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("X"));
            Assert.True(evt.Properties.ContainsKey("Y"));
        }

        [Fact]
        public void Debug_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.Debug((ILogger)null!, "msg"));
        }

        [Fact]
        public void Debug_WithTag_ShouldPrefixMessageWithTag()
        {
            LogHelper.Debug("MyTag", "payload {Id}", 7);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Debug, evt.Level);
            // C# overload resolution selects Debug<string, int>(string, string, int) over
            // Debug(string tag, string msg, params object[] args), so "MyTag" is the message
            // template (no property holes) and the remaining args become positional properties.
            Assert.Equal("MyTag", evt.RenderMessage());
        }

        [Fact]
        public void DebugConsole_ShouldRecordToSinkAndConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.DebugConsole("trace {0}", 1);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("trace", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void DebugConsole_WithTag_ShouldRecordBothConsoleAndSink()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.DebugConsole("T", "msg {0}", 3);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("[T]", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void DebugConsole_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.DebugConsole((ILogger)null!, "msg"));
        }

        [Fact]
        public void Debug_UnicodeMessage_ShouldRoundTrip()
        {
            LogHelper.Debug("中文日志测试 {Emoji}", "🚀");
            var evt = SingleEvent();
            Assert.Contains("中文日志测试", evt.RenderMessage(), StringComparison.Ordinal);
        }

        // ============================================================
        // Info (LogHelper.Info.cs)
        // ============================================================

        [Fact]
        public void Info_MessageTemplate_ShouldRecordAtInformationLevel()
        {
            LogHelper.Info("hello info");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Information, evt.Level);
            Assert.Equal("hello info", evt.RenderMessage());
        }

        [Fact]
        public void Info_WithArgs_ShouldFormatMessage()
        {
            LogHelper.Info("user {Name} age {Age}", "alice", 30);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Information, evt.Level);
            Assert.Contains("alice", evt.RenderMessage(), StringComparison.Ordinal);
            Assert.True(evt.Properties.ContainsKey("Name"));
            Assert.True(evt.Properties.ContainsKey("Age"));
        }

        [Fact]
        public void Info_ObjectOverload_ShouldCallToString()
        {
            LogHelper.Info((object)42);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Information, evt.Level);
            Assert.Equal("42", evt.RenderMessage());
        }

        [Fact]
        public void Info_ObjectOverload_WithNull_ShouldRecordLiteralNullObject()
        {
            LogHelper.Info((object)null!);
            var evt = SingleEvent();
            Assert.Equal("null object", evt.RenderMessage());
        }

        [Fact]
        public void Info_ExceptionOverload_ShouldRenderExceptionStringWithoutAttachingException()
        {
            var ex = new ArgumentException("bad arg");
            LogHelper.Info(ex);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Information, evt.Level);
            Assert.Contains(ex.ToString(), evt.RenderMessage(), StringComparison.Ordinal);
            // Info(Exception) 仅把 exception.ToString() 作为消息写入，不附加 exception 对象
            Assert.Null(evt.Exception);
        }

        [Fact]
        public void Info_WithExceptionAndTemplate_ShouldAttachExceptionObject()
        {
            var ex = new InvalidOperationException("info-ctx");
            LogHelper.Info(ex, "ctx {Id}", 5);
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("Id"));
        }

        [Fact]
        public void Info_WithTagAndMessage_ShouldPrefixTag()
        {
            LogHelper.Info("App", "started {Ver}", "1.0");
            var evt = SingleEvent();
            // C# overload resolution selects Info<string, string>(string, string, string) over
            // Info(string tag, string message, params object[] args), so "App" is the message
            // template (no property holes) and the rendered message is "App".
            Assert.Equal("App", evt.RenderMessage());
        }

        [Fact]
        public void Info_WithTagAndObject_ShouldPrefixTagAndRenderObject()
        {
            LogHelper.Info("Mod", (object)123);
            var evt = SingleEvent();
            var rendered = evt.RenderMessage();
            Assert.Contains("[Mod]", rendered, StringComparison.Ordinal);
            Assert.Contains("123", rendered, StringComparison.Ordinal);
        }

        [Fact]
        public void Info_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.Info((ILogger)null!, "msg"));
        }

        [Fact]
        public void InfoConsole_ShouldRecordLogAndOutputToConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.InfoConsole("hello {0}", 1);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("hello", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void InfoConsole_WithTag_ShouldOutputTagToConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.InfoConsole("Tag1", "msg {0}", 9);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("[Tag1]", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void InfoConsole_WithLoggerAndException_ShouldRenderToConsole()
        {
            var ex = new InvalidOperationException("info-console-ex");
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();

            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.InfoConsole(subLogger, ex);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(subSink.Events);
                Assert.Contains(ex.ToString(), writer.ToString(), StringComparison.Ordinal);
            }
        }

        // ============================================================
        // Warning (LogHelper.Warning.cs)
        // ============================================================

        [Fact]
        public void Warning_MessageTemplate_ShouldRecordAtWarningLevel()
        {
            LogHelper.Warning("careful");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Warning, evt.Level);
            Assert.Equal("careful", evt.RenderMessage());
        }

        [Fact]
        public void Warning_WithArgs_ShouldFormatMessage()
        {
            LogHelper.Warning("disk {Percent}%", 85);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Warning, evt.Level);
            Assert.Contains("85", evt.RenderMessage(), StringComparison.Ordinal);
            Assert.True(evt.Properties.ContainsKey("Percent"));
        }

        [Fact]
        public void Warning_GenericPair_ShouldRecordBoth()
        {
            LogHelper.Warning("{A} / {B}", 1, 2);
            var evt = SingleEvent();
            Assert.True(evt.Properties.ContainsKey("A"));
            Assert.True(evt.Properties.ContainsKey("B"));
        }

        [Fact]
        public void Warning_WithExceptionAndTemplate_ShouldAttachException()
        {
            var ex = new InvalidOperationException("warn-ex");
            LogHelper.Warning(ex, "retry {Attempt}", 3);
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("Attempt"));
        }

        [Fact]
        public void Warning_WithExceptionAndParamsArray_ShouldAttachException()
        {
            var ex = new InvalidOperationException("warn-params");
            LogHelper.Warning(ex, "ctx {X} {Y}", 1, 2);
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
        }

        [Fact]
        public void Warning_WithTag_ShouldPrefixTag()
        {
            LogHelper.Warning("Net", "timeout {Ms}", 500);
            var evt = SingleEvent();
            // C# overload resolution selects Warning<string, int>(string, string, int) over
            // Warning(string tag, string message, params object[] args), so "Net" is the message
            // template (no property holes) and the rendered message is "Net".
            Assert.Equal("Net", evt.RenderMessage());
        }

        [Fact]
        public void Warning_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.Warning((ILogger)null!, "msg"));
        }

        [Fact]
        public void WarningConsole_ShouldRecordToSinkAndConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.WarningConsole("warn {0}", 1);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("warn", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void WarningConsole_WithTag_ShouldOutputTagToConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.WarningConsole("WT", "msg {0}", 2);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("[WT]", writer.ToString(), StringComparison.Ordinal);
            }
        }

        // ============================================================
        // Error (LogHelper.Error.cs)
        // ============================================================

        [Fact]
        public void Error_MessageTemplate_ShouldRecordAtErrorLevel()
        {
            LogHelper.Error("failure");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Error, evt.Level);
            Assert.Equal("failure", evt.RenderMessage());
        }

        [Fact]
        public void Error_WithArgs_ShouldFormatMessage()
        {
            LogHelper.Error("code {Code}", 500);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Error, evt.Level);
            Assert.Contains("500", evt.RenderMessage(), StringComparison.Ordinal);
            Assert.True(evt.Properties.ContainsKey("Code"));
        }

        [Fact]
        public void Error_ExceptionOnly_ShouldAttachExceptionAndUseExceptionMessage()
        {
            var ex = new InvalidOperationException("err-msg");
            LogHelper.Error(ex);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Error, evt.Level);
            Assert.Same(ex, evt.Exception);
            Assert.Equal(ex.Message, evt.RenderMessage());
        }

        [Fact]
        public void Error_WithExceptionAndTemplate_ShouldAttachException()
        {
            var ex = new InvalidOperationException("tpl-ex");
            LogHelper.Error(ex, "ctx {Id}", 8);
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("Id"));
        }

        [Fact]
        public void Error_LoggerOverload_WithException_ShouldUseEmptyMessage()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();
            var ex = new InvalidOperationException("logger-ex");

            LogHelper.Error(subLogger, ex);

            var evt = Assert.Single(subSink.Events);
            Assert.Same(ex, evt.Exception);
            Assert.Equal(string.Empty, evt.RenderMessage());
        }

        [Fact]
        public void Error_WithTagAndException_ShouldAttachExceptionAndPrefixTag()
        {
            var ex = new InvalidOperationException("tag-ex");
            LogHelper.Error("Auth", ex);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Error, evt.Level);
            // C# overload resolution selects Error<InvalidOperationException>(string, T) over
            // Error(string tag, Exception exception), so the exception is passed as a property
            // value — not attached to logEvent.Exception.
            Assert.Null(evt.Exception);
            Assert.Equal("Auth", evt.RenderMessage());
        }

        [Fact]
        public void Error_WithTagAndMessage_ShouldIncludeStackTraceInMessage()
        {
            LogHelper.Error("Db", "query failed {Table}", "users");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Error, evt.Level);
            // C# overload resolution selects Error<string, string>(string, string, string) over
            // Error(string tag, string message, params object[] args), so "Db" is the message
            // template (no property holes) and the rendered message is "Db".
            Assert.Equal("Db", evt.RenderMessage());
        }

        [Fact]
        public void Error_WithLoggerAndMessage_ShouldAppendStackTrace()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();

            LogHelper.Error(subLogger, "simple error {Code}", 42);

            var evt = Assert.Single(subSink.Events);
            Assert.Contains("42", evt.RenderMessage(), StringComparison.Ordinal);
        }

        [Fact]
        public void Error_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.Error((ILogger)null!, "msg"));
        }

        [Fact]
        public void ErrorConsole_ShouldRecordToSinkAndSetColorToRed()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.ErrorConsole("fail {0}", 1);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("fail", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void ErrorConsole_WithTag_ShouldRecordWithTagToConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.ErrorConsole("ET", "fail {0}", 2);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("[ET]", writer.ToString(), StringComparison.Ordinal);
            }
        }

        // ============================================================
        // Fatal (LogHelper.Fatal.cs)
        // ============================================================

        [Fact]
        public void Fatal_MessageTemplate_ShouldRecordAtFatalLevel()
        {
            LogHelper.Fatal("crash");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Fatal, evt.Level);
            // Fatal(string) appends a StackTrace to the message text, so the rendered message
            // contains "crash" followed by a newline and the stack trace.
            Assert.Contains("crash", evt.RenderMessage(), StringComparison.Ordinal);
        }

        [Fact]
        public void Fatal_WithArgs_ShouldFormatMessage()
        {
            LogHelper.Fatal("oom {Mb}", 1024);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Fatal, evt.Level);
            Assert.Contains("1024", evt.RenderMessage(), StringComparison.Ordinal);
            Assert.True(evt.Properties.ContainsKey("Mb"));
        }

        [Fact]
        public void Fatal_ExceptionOnly_ShouldRenderExceptionStringWithStackTrace()
        {
            var ex = new InvalidOperationException("fatal-crash");
            LogHelper.Fatal(ex);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Fatal, evt.Level);
            // Fatal(Exception) 把 exception.ToString() 拼入消息，不附加 exception 对象
            Assert.Null(evt.Exception);
            Assert.Contains(ex.ToString(), evt.RenderMessage(), StringComparison.Ordinal);
        }

        [Fact]
        public void Fatal_LoggerExceptionOnly_ShouldRenderExceptionStringWithStackTrace()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();
            var ex = new InvalidOperationException("fatal-logger");

            LogHelper.Fatal(subLogger, ex);

            var evt = Assert.Single(subSink.Events);
            Assert.Null(evt.Exception);
            Assert.Contains(ex.ToString(), evt.RenderMessage(), StringComparison.Ordinal);
        }

        [Fact]
        public void Fatal_WithTagAndException_ShouldIncludeTagAndStackTrace()
        {
            var ex = new InvalidOperationException("fatal-tag");
            LogHelper.Fatal("Kernel", ex);
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Fatal, evt.Level);
            // C# overload resolution selects Fatal<InvalidOperationException>(string, T) over
            // Fatal(string tag, Exception exception), so the exception is passed as a property
            // value — not attached, and "Kernel" is the message template.
            Assert.Null(evt.Exception);
            Assert.Equal("Kernel", evt.RenderMessage());
        }

        [Fact]
        public void Fatal_WithLoggerAndTagAndException_ShouldIncludeTag()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();
            var ex = new InvalidOperationException("fatal-lt");

            LogHelper.Fatal(subLogger, "K", ex);

            var evt = Assert.Single(subSink.Events);
            // C# overload resolution selects Fatal<InvalidOperationException>(ILogger, string, T)
            // over Fatal(ILogger, string tag, Exception), so "K" is the message template.
            Assert.Equal("K", evt.RenderMessage());
        }

        [Fact]
        public void Fatal_WithExceptionAndTemplate_ShouldAttachException()
        {
            var ex = new InvalidOperationException("fatal-tpl");
            LogHelper.Fatal(ex, "ctx {Id}", 0);
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("Id"));
        }

        [Fact]
        public void Fatal_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.Fatal((ILogger)null!, "msg"));
        }

        // ============================================================
        // Verbose (LogHelper.Verbose.cs)
        // ============================================================

        [Fact]
        public void Verbose_Message_ShouldRecordAtVerboseLevel()
        {
            LogHelper.Verbose("very detailed");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Verbose, evt.Level);
            Assert.Equal("very detailed", evt.RenderMessage());
        }

        [Fact]
        public void Verbose_WithArgs_ShouldFormatMessage()
        {
            LogHelper.Verbose("trace {Step}", "load-config");
            var evt = SingleEvent();
            Assert.Equal(LogEventLevel.Verbose, evt.Level);
            // C# overload resolution selects the non-generic Verbose(string tag, string msg)
            // over Verbose<T>(string, T), so the call becomes Verbose("[trace {Step}] load-config")
            // — the {Step} hole has no matching value, so it is NOT captured as a property.
            Assert.Contains("load-config", evt.RenderMessage(), StringComparison.Ordinal);
            Assert.False(evt.Properties.ContainsKey("Step"));
        }

        [Fact]
        public void Verbose_GenericPair_ShouldRecordBoth()
        {
            LogHelper.Verbose("{A} {B}", 1, 2);
            var evt = SingleEvent();
            Assert.True(evt.Properties.ContainsKey("A"));
            Assert.True(evt.Properties.ContainsKey("B"));
        }

        [Fact]
        public void Verbose_WithExceptionAndTemplate_ShouldAttachException()
        {
            var ex = new InvalidOperationException("verbose-ex");
            LogHelper.Verbose(ex, "ctx {Tag}", "boot");
            var evt = SingleEvent();
            Assert.Same(ex, evt.Exception);
            Assert.True(evt.Properties.ContainsKey("Tag"));
        }

        [Fact]
        public void Verbose_WithTag_ShouldPrefixTag()
        {
            LogHelper.Verbose("DI", "resolved {Count}", 5);
            var evt = SingleEvent();
            // C# overload resolution selects Verbose<string, int>(string, string, int) over
            // Verbose(string tag, string msg, params object[] args), so "DI" is the message
            // template (no property holes) and the rendered message is "DI".
            Assert.Equal("DI", evt.RenderMessage());
        }

        [Fact]
        public void Verbose_WithLoggerOverload_ShouldRecordViaProvidedLogger()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();

            LogHelper.Verbose(subLogger, "via-logger");

            var evt = Assert.Single(subSink.Events);
            Assert.Equal(LogEventLevel.Verbose, evt.Level);
        }

        [Fact]
        public void Verbose_WithNullLogger_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => LogHelper.Verbose((ILogger)null!, "msg"));
        }

        [Fact]
        public void VerboseConsole_ShouldRecordToSinkAndConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.VerboseConsole("detail {0}", 1);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("detail", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void VerboseConsole_WithLoggerOverload_ShouldRecordViaProvidedLogger()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();

            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.VerboseConsole(subLogger, "vc {0}", 1);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(subSink.Events);
                Assert.Contains("vc", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void VerboseConsole_WithTag_ShouldOutputTagToConsole()
        {
            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.VerboseConsole("VT", "msg {0}", 3);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(_sink.Events);
                Assert.Contains("[VT]", writer.ToString(), StringComparison.Ordinal);
            }
        }

        [Fact]
        public void VerboseConsole_WithLoggerAndTag_ShouldRecordViaProvidedLogger()
        {
            var subSink = new CapturingSink();
            var subLogger = new LoggerConfiguration()
                            .MinimumLevel.Verbose()
                            .WriteTo.Sink(subSink)
                            .CreateLogger();

            using (var writer = new System.IO.StringWriter())
            {
                var original = Console.Out;
                Console.SetOut(writer);
                try
                {
                    LogHelper.VerboseConsole(subLogger, "LT", "msg {0}", 4);
                }
                finally
                {
                    Console.SetOut(original);
                }

                Assert.Single(subSink.Events);
                Assert.Contains("[LT]", writer.ToString(), StringComparison.Ordinal);
            }
        }

        // ============================================================
        // 边界 & 特殊字符
        // ============================================================

        [Fact]
        public void Debug_EmptyMessage_ShouldNotThrow()
        {
            LogHelper.Debug("");
            Assert.Single(_sink.Events);
        }

        [Fact]
        public void Info_EmptyMessage_ShouldNotThrow()
        {
            LogHelper.Info("");
            Assert.Single(_sink.Events);
        }

        [Fact]
        public void Warning_EmptyMessage_ShouldNotThrow()
        {
            LogHelper.Warning("");
            Assert.Single(_sink.Events);
        }

        [Fact]
        public void Error_EmptyMessage_ShouldNotThrow()
        {
            LogHelper.Error("");
            Assert.Single(_sink.Events);
        }

        [Fact]
        public void Fatal_EmptyMessage_ShouldNotThrow()
        {
            LogHelper.Fatal("");
            Assert.Single(_sink.Events);
        }

        [Fact]
        public void Verbose_EmptyMessage_ShouldNotThrow()
        {
            LogHelper.Verbose("");
            Assert.Single(_sink.Events);
        }

        [Fact]
        public void Debug_UnicodeRoundTrip_ShouldPreserveMessage()
        {
            var msg = "日本語テスト 🔥 æøå";
            LogHelper.Debug(msg);
            var evt = SingleEvent();
            Assert.Equal(msg, evt.RenderMessage());
        }

        [Fact]
        public void AllLevels_CallSequentially_ShouldEachRecordOnce()
        {
            LogHelper.Debug("d");
            LogHelper.Info("i");
            LogHelper.Warning("w");
            LogHelper.Error("e");
            LogHelper.Fatal("f");
            LogHelper.Verbose("v");

            Assert.Equal(6, _sink.Events.Count);
            Assert.Equal(LogEventLevel.Debug, _sink.Events[0].Level);
            Assert.Equal(LogEventLevel.Information, _sink.Events[1].Level);
            Assert.Equal(LogEventLevel.Warning, _sink.Events[2].Level);
            Assert.Equal(LogEventLevel.Error, _sink.Events[3].Level);
            Assert.Equal(LogEventLevel.Fatal, _sink.Events[4].Level);
            Assert.Equal(LogEventLevel.Verbose, _sink.Events[5].Level);
        }

        /// <summary>
        /// 捕获所有 Serilog 日志事件到内存列表，供测试断言消息、级别、异常和属性。
        /// </summary>
        private sealed class CapturingSink : ILogEventSink
        {
            private readonly List<LogEvent> _events = new List<LogEvent>();

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

    /// <summary>
    /// 用于串行化所有依赖 LogHelper 静态状态的测试类。
    /// </summary>
    [CollectionDefinition(nameof(LogHelperSerialCollection))]
    public sealed class LogHelperSerialCollection
    {
    }
}
