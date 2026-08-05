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
//  CNB Repository: https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Logger;
using Serilog;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger;

[Collection(nameof(LogHandlerCreateTestsCollection))]
public sealed class LogHandlerCreateTests : IDisposable
{
    private readonly string _tempDirectory;

    public LogHandlerCreateTests()
    {
        // Use a unique temp directory per test so concurrent runs / file sinks don't collide.
        _tempDirectory = Path.Combine(Path.GetTempPath(), "GameFrameX.Foundation.Tests", "LogHandlerCreate", Guid.NewGuid().ToString("N"));
    }

    public void Dispose()
    {
        // All tests pass isDefault: false, so Log.Logger is never mutated.
        // Clean up the per-test temp directory only.
        try
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
        }
        catch
        {
            // Ignore cleanup failures — temp directory will be reaped by the OS.
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public void Create_WithNullLogOptions_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => LogHandler.Create(null, isDefault: false));
    }

    [Fact]
    public void Create_WithEmptyLogType_ShouldThrowArgumentException()
    {
        var options = new LogOptions("logs")
        {
            LogType = "",
            IsWriteToFile = false,
            IsConsole = false,
        };

        Assert.Throws<ArgumentException>(() => LogHandler.Create(options, isDefault: false));
    }

    [Fact]
    public void Create_WithMinimalOptions_ShouldReturnUsableLogger()
    {
        var options = new LogOptions("logs")
        {
            LogType = "gfx-186-app",
            LogTagName = "gfx-186",
            LogSavePath = _tempDirectory,
            IsWriteToFile = false,
            IsConsole = false,
        };

        var logger = LogHandler.Create(options, isDefault: false);

        Assert.NotNull(logger);
    }

    [Fact]
    public void Create_ShouldInvokeConfigurationAction()
    {
        var options = new LogOptions("logs")
        {
            LogType = "gfx-186-app",
            LogTagName = "gfx-186",
            LogSavePath = _tempDirectory,
            IsWriteToFile = false,
            IsConsole = false,
        };

        var captured = false;
        var logger = LogHandler.Create(options, isDefault: false, configurationAction: _ => { captured = true; });

        Assert.NotNull(logger);
        Assert.True(captured, "configurationAction should be invoked exactly once.");
    }

    [Fact]
    public void Create_WithNonExistentSaveDirectory_ShouldCreateItAutomatically()
    {
        // Use a deeply nested directory that definitely does not exist beforehand.
        var nested = Path.Combine(_tempDirectory, "nested", "logs");
        Assert.False(Directory.Exists(nested));

        var options = new LogOptions("logs")
        {
            LogType = "gfx-186-app",
            LogSavePath = nested,
            IsWriteToFile = false,
            IsConsole = false,
        };

        var logger = LogHandler.Create(options, isDefault: false);

        Assert.NotNull(logger);
        Assert.True(Directory.Exists(nested), "ResolveLogPath should ensure the log folder exists.");
    }
}

[CollectionDefinition(nameof(LogHandlerCreateTestsCollection))]
public sealed class LogHandlerCreateTestsCollection
{
}
