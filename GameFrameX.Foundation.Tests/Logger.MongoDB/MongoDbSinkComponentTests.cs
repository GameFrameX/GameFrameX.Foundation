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
using GameFrameX.Foundation.Logger.MongoDB;
using MongoDB.Driver;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger.MongoDB;

/// <summary>
/// Logger.MongoDB 组件包（C77）的装配行为测试：注册、默认值、注入优先级、fail-fast 与脱敏。
/// 全部用例离线执行，不依赖 live MongoDB（MongoClient/GetDatabase 均为惰性连接）。
/// </summary>
public sealed class MongoDbSinkComponentTests : IDisposable
{
    private readonly string _tempDirectory;

    public MongoDbSinkComponentTests()
    {
        // 每个用例独立的临时目录，避免文件 sink 并发冲突（本组用例默认关闭文件输出，仅作兜底）。
        _tempDirectory = Path.Combine(Path.GetTempPath(), "GameFrameX.Foundation.Tests", "LoggerMongoDb", Guid.NewGuid().ToString("N"));
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

    /// <summary>
    /// 构造最小化 LogOptions：关闭文件/控制台/Loki 输出，只保留被测的外部 sink 装配路径。
    /// </summary>
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
    // UseMongoDb 注册行为
    // ============================================================

    [Fact]
    public void UseMongoDb_WithUrl_ShouldRegisterSingleConfiguratorAndStayChainable()
    {
        var options = CreateOptions();

        var returned = options.UseMongoDb("mongodb://127.0.0.1:27017/logs");

        // 返回同一实例以支持链式配置；注册恰好一个 sink 配置器。
        Assert.Same(options, returned);
        Assert.Single(options.SinkConfigurators);
    }

    [Fact]
    public void UseMongoDb_WithNullUrl_ShouldThrowArgumentNullException()
    {
        var options = CreateOptions();

        Assert.Throws<ArgumentNullException>(() => options.UseMongoDb((string)null));
    }

    [Fact]
    public void UseMongoDb_WithWhiteSpaceUrl_ShouldThrowArgumentException()
    {
        var options = CreateOptions();

        Assert.Throws<ArgumentException>(() => options.UseMongoDb("   "));
    }

    [Fact]
    public void UseMongoDb_WithNullDatabase_ShouldThrowArgumentNullException()
    {
        var options = CreateOptions();

        Assert.Throws<ArgumentNullException>(() => options.UseMongoDb((IMongoDatabase)null));
    }

    [Fact]
    public void UseMongoDb_WithConfigure_ShouldApplyCustomValues()
    {
        var options = CreateOptions();

        options.UseMongoDb(sinkOptions =>
        {
            sinkOptions.DatabaseUrl = "mongodb://127.0.0.1:27017/custom";
            sinkOptions.CollectionName = "audit";
            sinkOptions.CappedMaxSizeMb = 100;
            sinkOptions.CappedMaxDocuments = 1000;
        });

        Assert.Single(options.SinkConfigurators);
    }

    // ============================================================
    // MongoDbSinkOptions 默认值与脱敏
    // ============================================================

    [Fact]
    public void MongoDbSinkOptions_Defaults_ShouldMatchComponentContract()
    {
        var sinkOptions = new MongoDbSinkOptions();

        // 组件契约默认值（C77）：连接串默认空 + fail-fast；集合名 "log"；capped 50MB / 50000 条。
        Assert.Equal(string.Empty, sinkOptions.DatabaseUrl);
        Assert.Null(sinkOptions.Database);
        Assert.Equal("log", sinkOptions.CollectionName);
        Assert.Equal(50, sinkOptions.CappedMaxSizeMb);
        Assert.Equal(50000, sinkOptions.CappedMaxDocuments);
        Assert.Null(sinkOptions.RollingInterval);
        Assert.Null(sinkOptions.RestrictedToMinimumLevel);
        Assert.Null(sinkOptions.ConfigureSink);
    }

    [Fact]
    public void MongoDbSinkOptions_ToString_WithCredentials_ShouldRedactUrl()
    {
        var sinkOptions = new MongoDbSinkOptions
        {
            DatabaseUrl = "mongodb://user:secret@127.0.0.1:27017/logs?authSource=admin",
        };

        var text = sinkOptions.ToString();

        // 连接串凭证不得出现在字符串表示中（lesson P0-1 同源约束）。
        Assert.DoesNotContain("secret", text, StringComparison.Ordinal);
        Assert.DoesNotContain("mongodb://", text, StringComparison.Ordinal);
    }

    // ============================================================
    // 装配行为（离线）
    // ============================================================

    [Fact]
    public void Create_WithInjectedDatabase_AndInvalidUrl_ShouldPreferDatabaseAndSucceed()
    {
        var options = CreateOptions();

        // Database 注入优先于 URL：URL 即使是非法值也不应被解析（证明走的是注入路径）。
        var database = new MongoClient("mongodb://127.0.0.1:27017").GetDatabase("logs");
        options.UseMongoDb(sinkOptions =>
        {
            sinkOptions.Database = database;
            sinkOptions.DatabaseUrl = "not-a-valid-mongo-url";
        });

        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));

        Assert.Null(exception);
    }

    [Fact]
    public void Create_WithUrlOnly_ShouldSucceedOffline()
    {
        var options = CreateOptions();

        // 仅 URL：MongoUrl 解析为本地行为，不触发真实连接。
        options.UseMongoDb("mongodb://127.0.0.1:27017/logs");

        var exception = Record.Exception(() => LogHandler.Create(options, isDefault: false));

        Assert.Null(exception);
    }

    [Fact]
    public void Create_WithoutDatabaseAndUrl_ShouldThrowArgumentException()
    {
        var options = CreateOptions();

        // fail-fast（lesson P1-7）：既未注入 Database 也未配置 URL 时，装配阶段即抛参数异常。
        options.UseMongoDb(sinkOptions => { sinkOptions.DatabaseUrl = string.Empty; });

        Assert.Throws<ArgumentException>(() => LogHandler.Create(options, isDefault: false));
    }

    // ============================================================
    // LogOptions 安全投影（lesson P0-1 落实）
    // ============================================================

    [Fact]
    public void LogOptions_ToString_AfterUseMongoDb_ShouldNotLeakUrlPasswordOrDelegates()
    {
        var options = CreateOptions();
        options.UseMongoDb("mongodb://user:secret@127.0.0.1:27017/logs");

        var text = options.ToString();

        // 不抛异常；不泄露 Mongo 连接串、Loki 密码与 SinkConfigurators 委托内容。
        Assert.DoesNotContain("mongodb://", text, StringComparison.Ordinal);
        Assert.DoesNotContain("secret", text, StringComparison.Ordinal);
        Assert.DoesNotContain("SinkConfigurators", text, StringComparison.Ordinal);
        Assert.Contains("LogType", text, StringComparison.Ordinal);
    }
}
