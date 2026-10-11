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
using GameFrameX.Foundation.Logger.GrafanaLoki;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger.GrafanaLoki;

/// <summary>
/// Logger.GrafanaLoki 组件包（C77）的装配行为测试：注册、默认值、周期校验、脱敏与离线装配。
/// 覆盖自 LogHandlerExtendedTests 迁移的 4 个 Loki 用例（空标签 / property 过滤 / 凭证 / 压缩）。
/// 全部用例离线执行，不依赖 live Loki（HTTP sink 为周期批量，配置阶段不发起请求）。
/// </summary>
public sealed class GrafanaLokiSinkComponentTests : IDisposable
{
    private readonly string _tempDirectory;

    public GrafanaLokiSinkComponentTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "GameFrameX.Foundation.Tests", "LoggerGrafanaLoki", Guid.NewGuid().ToString("N"));
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

    // ============================================================
    // UseGrafanaLoki 注册行为
    // ============================================================

    [Fact]
    public void UseGrafanaLoki_WithUrl_ShouldRegisterSingleConfiguratorAndStayChainable()
    {
        var options = CreateOptions();

        var returned = options.UseGrafanaLoki("http://loki:3100");

        // 返回同一实例以支持链式配置；注册恰好一个 sink 配置器。
        Assert.Same(options, returned);
        Assert.Single(options.SinkConfigurators);
    }

    [Fact]
    public void UseGrafanaLoki_WithConfigure_ShouldRegisterSingleConfigurator()
    {
        var options = CreateOptions();

        options.UseGrafanaLoki(sinkOptions => { sinkOptions.Url = "http://loki:3100"; });

        Assert.Single(options.SinkConfigurators);
    }

    [Fact]
    public void UseGrafanaLoki_WithNullOptions_ShouldThrowArgumentNullException()
    {
        LogOptions options = null;

        Assert.Throws<ArgumentNullException>(() => options.UseGrafanaLoki("http://loki:3100"));
    }

    [Fact]
    public void UseGrafanaLoki_WithNullUrl_ShouldThrowArgumentNullException()
    {
        var options = CreateOptions();

        Assert.Throws<ArgumentNullException>(() => options.UseGrafanaLoki((string)null));
    }

    [Fact]
    public void UseGrafanaLoki_WithWhiteSpaceUrl_ShouldThrowArgumentException()
    {
        var options = CreateOptions();

        Assert.Throws<ArgumentException>(() => options.UseGrafanaLoki("   "));
    }

    [Fact]
    public void UseGrafanaLoki_WithNullConfigure_ShouldThrowArgumentNullException()
    {
        var options = CreateOptions();

        Assert.Throws<ArgumentNullException>(() => options.UseGrafanaLoki((Action<GrafanaLokiSinkOptions>)null));
    }

    // ============================================================
    // GrafanaLokiSinkOptions 默认值、周期校验与脱敏
    // ============================================================

    [Fact]
    public void GrafanaLokiSinkOptions_Defaults_ShouldMatchComponentContract()
    {
        var sinkOptions = new GrafanaLokiSinkOptions();

        // 组件契约默认值（自原 LogOptions 迁移）：地址 localhost:3100、批量 1000、队列 100000、周期 2 秒、压缩开启。
        Assert.Equal("http://localhost:3100", sinkOptions.Url);
        Assert.Empty(sinkOptions.Labels);
        Assert.Empty(sinkOptions.Properties);
        Assert.Null(sinkOptions.UserName);
        Assert.Null(sinkOptions.Password);
        Assert.True(sinkOptions.CompressionEnabled);
        Assert.Equal(1000, sinkOptions.BatchSizeLimit);
        Assert.Equal(100000, sinkOptions.QueueLimit);
        Assert.Equal(2, sinkOptions.Period);
        Assert.Null(sinkOptions.RestrictedToMinimumLevel);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-5, 2)]
    [InlineData(5, 5)]
    public void GrafanaLokiSinkOptions_Period_WithInvalidOrValidValue_ShouldFallbackOrKeep(int input, int expected)
    {
        var sinkOptions = new GrafanaLokiSinkOptions
        {
            Period = input,
        };

        // 0 或负值强制回退默认 2 秒（校验逻辑自原 LogOptions.GrafanaLokiPeriod 迁移）。
        Assert.Equal(expected, sinkOptions.Period);
    }

    [Fact]
    public void GrafanaLokiSinkOptions_ToString_WithPassword_ShouldRedact()
    {
        var sinkOptions = new GrafanaLokiSinkOptions
        {
            UserName = "admin",
            Password = "loki-secret",
        };

        var text = sinkOptions.ToString();

        // 密码不得出现在字符串表示中（lesson P0-1 同源约束）。
        Assert.DoesNotContain("loki-secret", text, StringComparison.Ordinal);
        Assert.Contains("***", text, StringComparison.Ordinal);
    }

    // ============================================================
    // 装配行为（离线；迁移自 LogHandlerExtendedTests 的 Loki 用例）
    // ============================================================

    [Fact]
    public void Create_WithNullLabels_ShouldStillWireSinkWithEmptyLabels()
    {
        var options = CreateOptions();

        // 语义变化（C77，有意）：旧版 Labels=null 时 ApplyLoki 早退、不装配 Loki sink（flag 模式下的 null 哨兵禁用）；
        // 新版 UseGrafanaLoki 为显式注册，null 视为空标签集合，sink 仍然装配（BuildLabels 返回空数组）。
        options.UseGrafanaLoki(sinkOptions => { sinkOptions.Labels = null; });

        // 显式注册即装配：配置器存在且离线装配成功。
        Assert.Single(options.SinkConfigurators);
        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));
        Assert.Null(exception);
    }

    [Fact]
    public void Create_WithLabelsAndProperties_ShouldSucceedAndSkipEmptyEntries()
    {
        var options = CreateOptions();

        // 原 Create_WithGrafanaLokiEnabledAndLabels：property 含空 key / 空值条目时跳过，装配不失败。
        options.UseGrafanaLoki(sinkOptions =>
        {
            sinkOptions.Url = "http://localhost:3100";
            sinkOptions.Labels = new Dictionary<string, string> { { "app", "test" } };
            sinkOptions.Properties = new Dictionary<string, string>
            {
                { "env", "dev" },
                { "empty", "" },
                { "", "bad" },
            };
        });

        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));

        Assert.Null(exception);
    }

    [Fact]
    public void Create_WithCredentials_ShouldSucceed()
    {
        var options = CreateOptions();

        // 原 Create_WithGrafanaLokiCredentials：用户名 + 密码构成 Basic 认证凭证。
        options.UseGrafanaLoki(sinkOptions =>
        {
            sinkOptions.Url = "http://localhost:3100";
            sinkOptions.Labels = new Dictionary<string, string> { { "app", "test" } };
            sinkOptions.UserName = "admin";
            sinkOptions.Password = "secret";
        });

        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));

        Assert.Null(exception);
    }

    [Fact]
    public void Create_WithCompressionDisabled_ShouldSucceed()
    {
        var options = CreateOptions();

        // 原 Create_WithGrafanaLokiCompressionDisabled：关闭压缩走无 Handler 分支。
        options.UseGrafanaLoki(sinkOptions =>
        {
            sinkOptions.Url = "http://localhost:3100";
            sinkOptions.Labels = new Dictionary<string, string> { { "app", "test" } };
            sinkOptions.CompressionEnabled = false;
        });

        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));

        Assert.Null(exception);
    }

    [Fact]
    public void LogOptions_ToString_AfterUseGrafanaLoki_ShouldNotLeakPasswordOrDelegates()
    {
        var options = CreateOptions();

        options.UseGrafanaLoki(sinkOptions =>
        {
            sinkOptions.Password = "loki-secret";
        });

        var text = options.ToString();

        // 宿主配置投影不泄露组件密码与委托内容。
        Assert.DoesNotContain("loki-secret", text, StringComparison.Ordinal);
        Assert.DoesNotContain("SinkConfigurators", text, StringComparison.Ordinal);
    }
}
