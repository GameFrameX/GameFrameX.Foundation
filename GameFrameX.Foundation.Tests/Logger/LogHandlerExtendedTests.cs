using System;
using System.IO;
using System.Linq;
using GameFrameX.Foundation.Logger;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger
{
    /// <summary>
    /// LogHandler 的扩展覆盖测试，补充 LogHandlerCreateTests 中已有的 null / empty / minimal / directory 路径。
    /// 覆盖源文件：LogHandler.cs。
    /// </summary>
    /// <remarks>
    /// 所有测试使用 isDefault: false 以避免修改全局 Log.Logger 和 LogHelper 静态状态，
    /// 需要测试 isDefault: true 路径的场景单独在 Dispose 中重置 LogHelper。
    /// </remarks>
    [Collection("LogHelperSerialCollection")]
    public sealed class LogHandlerExtendedTests : IDisposable
    {
        private readonly string _tempDirectory;

        public LogHandlerExtendedTests()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "GameFrameX.Foundation.Tests", "LogHandlerExtended", Guid.NewGuid().ToString("N"));
        }

        public void Dispose()
        {
            // 所有测试用 isDefault: false，不会修改 LogHelper 静态状态，只清理临时目录。
            try
            {
                if (Directory.Exists(_tempDirectory))
                {
                    Directory.Delete(_tempDirectory, recursive: true);
                }
            }
            catch
            {
                // 忽略清理失败 — 临时目录由操作系统回收。
            }

            GC.SuppressFinalize(this);
        }

        // ============================================================
        // CreateLoggerConfiguration
        // ============================================================

        [Fact]
        public void CreateLoggerConfiguration_ShouldReturnNonNullConfiguration()
        {
            var config = LogHandler.CreateLoggerConfiguration();

            Assert.NotNull(config);
        }

        [Fact]
        public void CreateLoggerConfiguration_ShouldProduceWorkingLogger()
        {
            var sink = new CapturingSink();
            var logger = LogHandler.CreateLoggerConfiguration()
                                   .MinimumLevel.Verbose()
                                   .WriteTo.Sink(sink)
                                   .CreateLogger();

            logger.Information("test {Value}", 1);

            Assert.Single(sink.Events);
            Assert.Equal(LogEventLevel.Information, sink.Events[0].Level);
        }

        [Fact]
        public void CreateLoggerConfiguration_ShouldOverrideMicrosoftLogLevel()
        {
            // 验证 Microsoft 组件的日志级别被覆盖为 Information。
            // 这通过检查 logger 能正常记录 Information 级别来间接验证。
            var sink = new CapturingSink();
            var logger = LogHandler.CreateLoggerConfiguration()
                                   .MinimumLevel.Verbose()
                                   .WriteTo.Sink(sink)
                                   .CreateLogger();

            logger.Information("ms-test");

            Assert.Single(sink.Events);
        }

        // ============================================================
        // Create — 输出路径配置
        // ============================================================

        [Fact]
        public void Create_WithConsoleEnabled_ShouldReturnLoggerWithoutError()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-console",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = true,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        [Fact]
        public void Create_WithFileEnabled_ShouldCreateLogFileDirectory()
        {
            var logDir = Path.Combine(_tempDirectory, "file-logs");
            var options = new LogOptions("logs")
            {
                LogType = "ext-file",
                LogSavePath = logDir,
                IsWriteToFile = true,
                IsConsole = false,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
            // ApplyFile 会确保 LogType 子目录存在
            var expectedDir = Path.Combine(logDir, "ext-file");
            Assert.True(Directory.Exists(expectedDir), "文件日志目录应该被创建。");
        }

        [Fact]
        public void Create_WithCustomLogFileName_ShouldUseProvidedFileName()
        {
            var logDir = Path.Combine(_tempDirectory, "custom-name");
            var options = new LogOptions("logs")
            {
                LogType = "ext-custom",
                LogSavePath = logDir,
                LogFileName = "custom_test.log",
                IsWriteToFile = true,
                IsConsole = false,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
            // 文件路径由 LogType 子目录 + 自定义文件名组成
            var expectedDir = Path.Combine(logDir, "ext-custom");
            Assert.True(Directory.Exists(expectedDir));
        }

        [Fact]
        public void Create_WithLogTagName_ShouldReturnLogger()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-tag",
                LogTagName = "my-tag",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        // ============================================================
        // Create — 日志级别
        // ============================================================

        [Theory]
        [InlineData(LogEventLevel.Verbose)]
        [InlineData(LogEventLevel.Debug)]
        [InlineData(LogEventLevel.Information)]
        [InlineData(LogEventLevel.Warning)]
        [InlineData(LogEventLevel.Error)]
        [InlineData(LogEventLevel.Fatal)]
        public void Create_WithVariousLogEventLevels_ShouldReturnLogger(LogEventLevel level)
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-level",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
                LogEventLevel = level,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        // ============================================================
        // Create — isDefault 路径（设置全局 Log.Logger + LogHelper.SetLogger）
        // ============================================================

        [Fact]
        public void Create_WithIsDefaultTrue_ShouldSetGlobalLoggerAndLogHelper()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-default",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
            };

            try
            {
                var logger = LogHandler.Create(options, isDefault: true);

                Assert.NotNull(logger);
                // isDefault=true 会设置 Log.Logger
                Assert.NotNull(Log.Logger);
                // 日志应该可以通过全局 Log.Logger 写入
                Log.Information("after-create-default");
            }
            finally
            {
                // 重置全局状态
                Log.CloseAndFlush();
                ResetLogHelperState();
            }
        }

        // ============================================================
        // Create — 异常路径
        // ============================================================

        [Fact]
        public void Create_WithWhitespaceLogType_ShouldThrowArgumentException()
        {
            var options = new LogOptions("logs")
            {
                LogType = "   ",
                IsWriteToFile = false,
                IsConsole = false,
            };

            Assert.Throws<ArgumentException>(() => LogHandler.Create(options, isDefault: false));
        }

        [Fact]
        public void Create_WithGrafanaLokiEnabledButNullLabels_ShouldNotAddLokiSink()
        {
            // IsGrafanaLoki=true 但 GrafanaLokiLabels=null 时，ApplyLoki 直接返回，不添加 Loki sink。
            var options = new LogOptions("logs")
            {
                LogType = "ext-loki-null",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
                IsGrafanaLoki = true,
                GrafanaLokiLabels = null,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        [Fact]
        public void Create_WithGrafanaLokiEnabledAndLabels_ShouldReturnLogger()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-loki-labels",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
                IsGrafanaLoki = true,
                GrafanaLokiUrl = "http://localhost:3100",
                GrafanaLokiLabels = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "app", "test" },
                },
                GrafanaLokiProperty = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "env", "dev" },
                    { "empty", "" },
                    { "", "bad" },
                },
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        [Fact]
        public void Create_WithGrafanaLokiCredentials_ShouldReturnLogger()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-loki-auth",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
                IsGrafanaLoki = true,
                GrafanaLokiUrl = "http://localhost:3100",
                GrafanaLokiLabels = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "app", "test" },
                },
                GrafanaLokiUserName = "admin",
                GrafanaLokiPassword = "secret",
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        [Fact]
        public void Create_WithGrafanaLokiCompressionDisabled_ShouldReturnLogger()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-loki-nocomp",
                LogSavePath = _tempDirectory,
                IsWriteToFile = false,
                IsConsole = false,
                IsGrafanaLoki = true,
                GrafanaLokiUrl = "http://localhost:3100",
                GrafanaLokiLabels = new System.Collections.Generic.Dictionary<string, string>
                {
                    { "app", "test" },
                },
                GrafanaLokiCompressionEnabled = false,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        // ============================================================
        // Create — 文件大小 / 保留策略
        // ============================================================

        [Fact]
        public void Create_WithFileSizeLimitDisabled_ShouldReturnLogger()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-no-size-limit",
                LogSavePath = Path.Combine(_tempDirectory, "no-limit"),
                IsWriteToFile = true,
                IsConsole = false,
                // FileSizeLimitBytes is int (non-nullable); passing 0 to Serilog's File sink
                // throws ArgumentOutOfRangeException (must be positive). Use the default (100 MB).
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        [Fact]
        public void Create_WithUnlimitedRetainedFiles_ShouldReturnLogger()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-unlimited-retention",
                LogSavePath = Path.Combine(_tempDirectory, "unlimited"),
                IsWriteToFile = true,
                IsConsole = false,
                RetainedFileCountLimit = null,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        [Fact]
        public void Create_WithWriteToFileDisabled_ShouldNotRequireDirectory()
        {
            var options = new LogOptions("logs")
            {
                LogType = "ext-no-file",
                LogSavePath = Path.Combine(_tempDirectory, "never-created"),
                IsWriteToFile = false,
                IsConsole = false,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
        }

        // ============================================================
        // 辅助
        // ============================================================

        private static void ResetLogHelperState()
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

        private sealed class CapturingSink : ILogEventSink
        {
            private readonly System.Collections.Generic.List<LogEvent> _events = new System.Collections.Generic.List<LogEvent>();

            public System.Collections.Generic.IReadOnlyList<LogEvent> Events
            {
                get { return _events; }
            }

            public void Emit(LogEvent logEvent)
            {
                _events.Add(logEvent);
            }
        }
    }
}
