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
//  Any legal disputes or liabilities arising from secondary development based on this project
//  本组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository: https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Logger;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger;

/// <summary>
/// LogOptions.SinkConfigurators 外部 sink 注入点（C77）的行为测试：
/// 默认空集合、按注册顺序装配、事件可达、ToString 安全投影。
/// </summary>
public sealed class ExternalSinkConfiguratorTests : IDisposable
{
    private readonly string _tempDirectory;

    public ExternalSinkConfiguratorTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "GameFrameX.Foundation.Tests", "ExternalSinkConfigurator", Guid.NewGuid().ToString("N"));
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }
        catch
        {
            // 忽略清理失败，临时目录由操作系统回收。
        }

        GC.SuppressFinalize(this);
    }

    private LogOptions CreateOptions()
    {
        return new LogOptions(_tempDirectory)
        {
            LogType = "UnitTest",
            IsWriteToFile = false,
            IsConsole = false,
        };
    }

    /// <summary>
    /// 同步记录型 sink：用于断言外部注册的 sink 确实接收到日志事件。
    /// </summary>
    private sealed class RecordingSink : ILogEventSink
    {
        public List<LogEvent> Events { get; } = new();

        public void Emit(LogEvent logEvent)
        {
            lock (Events)
            {
                Events.Add(logEvent);
            }
        }
    }

    [Fact]
    public void SinkConfigurators_ByDefault_ShouldBeEmpty()
    {
        var options = CreateOptions();

        Assert.Empty(options.SinkConfigurators);
    }

    [Fact]
    public void Create_WithExternalSink_ShouldDeliverEventsToRegisteredSink()
    {
        var options = CreateOptions();
        var sink = new RecordingSink();

        // 外部组件注入点接收完整 LoggerConfiguration（Enrich 与 WriteTo 均可达）。
        options.SinkConfigurators.Add(loggerConfiguration => loggerConfiguration.WriteTo.Sink(sink));

        var logger = LogHandler.Create(options, isDefault: false);

        logger.Information("hello external sink");

        Assert.NotEmpty(sink.Events);
        Assert.Contains(sink.Events, e => e.RenderMessage() == "hello external sink");
    }

    [Fact]
    public void Create_WithMultipleExternalSinks_ShouldApplyAllInRegistrationOrder()
    {
        var options = CreateOptions();
        var first = new RecordingSink();
        var second = new RecordingSink();
        var order = new List<int>();

        options.SinkConfigurators.Add(_ => order.Add(1));
        options.SinkConfigurators.Add(_ => order.Add(2));
        options.SinkConfigurators.Add(loggerConfiguration => loggerConfiguration.WriteTo.Sink(first));
        options.SinkConfigurators.Add(loggerConfiguration => loggerConfiguration.WriteTo.Sink(second));

        var logger = LogHandler.Create(options, isDefault: false);
        logger.Warning("multi sink");

        // 按注册顺序装配，且两个 sink 均生效。
        Assert.Equal(new[] { 1, 2 }, order);
        Assert.NotEmpty(first.Events);
        Assert.NotEmpty(second.Events);
    }

    [Fact]
    public void Create_WithoutExternalSinks_ShouldBehaveAsBefore()
    {
        var options = CreateOptions();

        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));

        // 未注册外部 sink 时行为与拆分前一致：正常创建，无 Mongo/Loki 依赖。
        Assert.Null(exception);
    }

    [Fact]
    public void ExternalSink_CanUseEnrichfacet_BeforeWriting()
    {
        var options = CreateOptions();
        var sink = new RecordingSink();

        // 注入点提供完整 LoggerConfiguration：组件可先 Enrich 再 WriteTo（Loki 组件的 property 富化即依赖此能力）。
        options.SinkConfigurators.Add(loggerConfiguration =>
        {
            loggerConfiguration.Enrich.WithProperty("ComponentName", "fake-component");
            loggerConfiguration.WriteTo.Sink(sink);
        });

        var logger = LogHandler.Create(options, isDefault: false);
        logger.Information("enriched event");

        Assert.NotEmpty(sink.Events);
        Assert.Contains(sink.Events, e => e.Properties.ContainsKey("ComponentName"));
    }

    [Fact]
    public void LogOptions_ToString_ShouldExcludeSinkConfiguratorDelegates()
    {
        var options = CreateOptions();
        options.SinkConfigurators.Add((LoggerConfiguration _) => { });

        var text = options.ToString();

        // 安全投影不序列化委托列表，也不抛异常。
        Assert.NotNull(text);
        Assert.DoesNotContain("SinkConfigurators", text, StringComparison.Ordinal);
        Assert.Contains("LogType", text, StringComparison.Ordinal);
    }
}
