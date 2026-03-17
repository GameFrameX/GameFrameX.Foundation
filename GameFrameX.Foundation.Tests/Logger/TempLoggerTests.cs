// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Collections.Concurrent;
using GameFrameX.Foundation.Logger;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger;

public class TempLoggerTests : IDisposable
{
    private readonly ConcurrentQueue<LogEvent> _capturedEvents = new();

    public TempLoggerTests()
    {
    }

    [Fact]
    public void Debug_WithNoLoggerInitialized_ShouldCreateTempLogger()
    {
        LogHelper.Debug("Test message");
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void Info_WithNoLoggerInitialized_ShouldCreateTempLogger()
    {
        LogHelper.Info("Test message");
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void Warning_WithNoLoggerInitialized_ShouldCreateTempLogger()
    {
        LogHelper.Warning("Test message");
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void Error_WithNoLoggerInitialized_ShouldCreateTempLogger()
    {
        LogHelper.Error("Test message");
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void Fatal_WithNoLoggerInitialized_ShouldCreateTempLogger()
    {
        LogHelper.Fatal("Test message");
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void Debug_WithMessageAndArgs_ShouldFormatMessage()
    {
        LogHelper.Debug("Value: {Value}", 123);
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void Info_WithMessageAndArgs_ShouldFormatMessage()
    {
        LogHelper.Info("User: {User}, Age: {Age}", "John", 25);
        Assert.NotNull(GetTempLoggerField());
    }

    [Fact]
    public void SetLogger_WithTempLoggerExisting_ShouldFlushTempLogs()
    {
        LogHelper.Info("Temp log before initialization");

        var capturedEvents = new ConcurrentQueue<LogEvent>();
        var testLogger = new LoggerConfiguration()
            .WriteTo.Sink(new TestSink(capturedEvents))
            .CreateLogger();

        LogHelper.SetLogger(testLogger);

        LogHelper.Info("Log after initialization");

        Assert.Equal(2, capturedEvents.Count);
        Assert.Equal("Temp log before initialization", capturedEvents.ElementAt(0).RenderMessage());
        Assert.Equal("Log after initialization", capturedEvents.ElementAt(1).RenderMessage());
    }

    [Fact]
    public void SetLogger_WithTempLoggerExisting_ShouldDisposeTempLogger()
    {
        LogHelper.Info("Temp log");

        var testLogger = new LoggerConfiguration()
            .WriteTo.Sink(new NullSink())
            .CreateLogger();

        LogHelper.SetLogger(testLogger);

        Assert.Null(GetTempLoggerField());
    }

    [Fact]
    public void SetLogger_WithMultipleLogs_ShouldFlushAllLogs()
    {
        ResetLoggerState();

        LogHelper.Info("Log 1");
        LogHelper.Debug("Log 2");
        LogHelper.Warning("Log 3");

        var capturedEvents = new ConcurrentQueue<LogEvent>();
        var testLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Sink(new TestSink(capturedEvents))
            .CreateLogger();

        LogHelper.SetLogger(testLogger);

        Assert.Equal(3, capturedEvents.Count);
    }

    [Fact]
    public void SetLogger_ThenLog_ShouldUseNewLogger()
    {
        var capturedEvents = new ConcurrentQueue<LogEvent>();
        var testLogger = new LoggerConfiguration()
            .WriteTo.Sink(new TestSink(capturedEvents))
            .CreateLogger();

        LogHelper.SetLogger(testLogger);

        LogHelper.Info("After SetLogger");

        Assert.Single(capturedEvents);
        Assert.Equal("After SetLogger", capturedEvents.First().RenderMessage());
    }

    [Fact]
    public void SetLogger_WithNullLogger_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LogHelper.SetLogger(null!));
    }

    private static object? GetTempLoggerField()
    {
        var field = typeof(LogHelper).GetField("_tempLogger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        return field?.GetValue(null);
    }

    private static void ResetLoggerState()
    {
        var tempField = typeof(LogHelper).GetField("_tempLogger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var tempLogger = tempField?.GetValue(null);
        if (tempLogger is IDisposable disposable)
        {
            disposable.Dispose();
        }
        tempField?.SetValue(null, null);

        var loggerField = typeof(LogHelper).GetField("_logger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        loggerField?.SetValue(null, null);
    }

    public void Dispose()
    {
        var field = typeof(LogHelper).GetField("_tempLogger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        var tempLogger = field?.GetValue(null);
        if (tempLogger is IDisposable disposable)
        {
            disposable.Dispose();
        }

        var loggerField = typeof(LogHelper).GetField("_logger", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        loggerField?.SetValue(null, null);

        field?.SetValue(null, null);
        loggerField?.SetValue(null, null);

        GC.SuppressFinalize(this);
    }

    private sealed class TestSink : ILogEventSink
    {
        private readonly ConcurrentQueue<LogEvent> _events;

        public TestSink(ConcurrentQueue<LogEvent> events)
        {
            _events = events;
        }

        public void Emit(LogEvent logEvent)
        {
            _events.Enqueue(logEvent);
        }
    }

    private sealed class NullSink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
        }
    }
}
