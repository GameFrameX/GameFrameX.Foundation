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

[Collection("LogHelperSerialCollection")]
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
            LogType = "gfx-725-app",
            LogSavePath = nested,
            IsWriteToFile = true,
            IsConsole = false,
        };

        var logger = LogHandler.Create(options, isDefault: false);

        Assert.NotNull(logger);
        Assert.True(Directory.Exists(nested), "ApplyFile should ensure the log folder exists when file logging is enabled.");
    }

    [Fact]
    public void Create_WithWriteToFileDisabled_ShouldNotCreateLogDirectory()
    {
        // GFX-725: IsWriteToFile=false 时不应触碰日志目录（连目录都不创建）。
        var nested = Path.Combine(_tempDirectory, "skipped", "logs");
        Assert.False(Directory.Exists(nested));

        var options = new LogOptions("logs")
        {
            LogType = "gfx-725-app",
            LogSavePath = nested,
            IsWriteToFile = false,
            IsConsole = false,
        };

        var logger = LogHandler.Create(options, isDefault: false);

        Assert.NotNull(logger);
        Assert.False(Directory.Exists(nested), "File logging disabled — the log directory must not be created.");
    }

    [Fact]
    public void Create_WhenLogDirectoryCreationDenied_ShouldDisableFileSinkAndContinue()
    {
        // GFX-725: 目录创建抛 UnauthorizedAccessException 时降级（禁用文件 sink、Console 警告、进程继续）。
        // EACCES 仅在 Unix 非 root 环境可复现；先探测，无法复现（root / Windows）则跳过本用例。
        if (!OperatingSystem.IsLinux() && !OperatingSystem.IsMacOS())
        {
            return;
        }

        var lockedRoot = Path.Combine(_tempDirectory, "locked");
        Directory.CreateDirectory(lockedRoot);
        var originalMode = File.GetUnixFileMode(lockedRoot);
        File.SetUnixFileMode(lockedRoot, originalMode & ~(UnixFileMode.UserWrite | UnixFileMode.GroupWrite | UnixFileMode.OtherWrite));
        try
        {
            try
            {
                var probe = Path.Combine(lockedRoot, "probe");
                Directory.CreateDirectory(probe);
                Directory.Delete(probe);
                return; // 权限位被环境绕过（如 root 运行），无法复现，跳过。
            }
            catch (UnauthorizedAccessException)
            {
                // 权限位生效，可复现降级路径。
            }

            var options = new LogOptions("logs")
            {
                LogType = "gfx-725-locked",
                LogSavePath = lockedRoot,
                IsWriteToFile = true,
                IsConsole = false,
            };

            var logger = LogHandler.Create(options, isDefault: false);

            Assert.NotNull(logger);
            var expectedDir = Path.Combine(lockedRoot, "gfx-725-locked");
            Assert.False(Directory.Exists(expectedDir), "Degraded path must not create the log directory.");
        }
        finally
        {
            File.SetUnixFileMode(lockedRoot, originalMode);
        }
    }
}

[CollectionDefinition(nameof(LogHandlerCreateTestsCollection))]
public sealed class LogHandlerCreateTestsCollection
{
}
