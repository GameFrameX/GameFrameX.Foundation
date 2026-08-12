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

using System.Reflection;
using GameFrameX.Foundation.Logger;
using GameFrameX.Foundation.Logger.Internal;
using Serilog.Events;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger;

/// <summary>
/// Logger 模块 null 契约测试：验证 LoggerMemorySink.Emit / InternalTempLogger.FlushTo 对 null 输入必须快速失败。
/// </summary>
/// <remarks>
/// 这些测试表达的是"期望的健壮行为"——null 是编程错误，必须抛 ArgumentNullException 而非静默入队或 NRE。
/// InternalTempLogger 为 internal sealed 类，本项目无 InternalsVisibleTo，需通过反射构造实例并调用其方法。
/// LoggerMemorySink 为 public sealed 类，可直接测试。
/// </remarks>
[Collection("LogHelperSerialCollection")]
public sealed class LoggerNullContractTests : IDisposable
{
    // ==================== LoggerMemorySink.Emit(null) ====================

    /// <summary>
    /// LoggerMemorySink.Emit(null) 必须抛 ArgumentNullException，guard 应在入队前拦截非法输入。
    /// </summary>
    [Fact]
    public void Emit_WithNullLogEvent_ShouldThrowArgumentNullException()
    {
        // Arrange
        var sink = new LoggerMemorySink();

        // Act & Assert — null logEvent 是编程错误，必须快速失败
        var ex = Assert.Throws<ArgumentNullException>(() => sink.Emit(null));
        Assert.Equal("logEvent", ex.ParamName);
        // guard 在入队前触发，队列不应收到任何事件
        Assert.Empty(sink.GetEvents());
    }

    /// <summary>
    /// LoggerMemorySink.Emit(null) guard 必须在回调执行前抛出，回调不应被调用。
    /// </summary>
    [Fact]
    public void Emit_WithNullLogEvent_ShouldThrowBeforeInvokingCallback()
    {
        // Arrange
        int callbackCount = 0;
        var sink = new LoggerMemorySink(_ => callbackCount++);

        // Act & Assert — guard 优先于回调执行
        Assert.Throws<ArgumentNullException>(() => sink.Emit(null));
        Assert.Equal(0, callbackCount);
    }

    // ==================== InternalTempLogger.FlushTo(null) ====================

    /// <summary>
    /// InternalTempLogger.FlushTo(null) 必须抛 ArgumentNullException，guard 应优先于缓冲区状态检查。
    /// </summary>
    [Fact]
    public void FlushTo_WithNull_WhenBufferHasEvents_ShouldThrowArgumentNullException()
    {
        // Arrange — 通过 LogHelper.Info 创建 InternalTempLogger 并写入一条缓冲日志
        ResetLoggerState();
        LogHelper.Info("buffered message");

        var tempLogger = GetTempLoggerField();
        Assert.NotNull(tempLogger);

        MethodInfo flushToMethod = tempLogger.GetType().GetMethod("FlushTo");
        Assert.NotNull(flushToMethod);

        // Act & Assert — targetLogger=null 是编程错误，guard 必须先于 buffer 迭代抛出
        var ex = Assert.Throws<TargetInvocationException>(
            () => flushToMethod.Invoke(tempLogger, new object[] { null }));
        var ane = Assert.IsType<ArgumentNullException>(ex.InnerException);
        Assert.Equal("targetLogger", ane.ParamName);
    }

    /// <summary>
    /// InternalTempLogger.FlushTo(null) 即使缓冲区为空也必须抛 ArgumentNullException，guard 不应依赖缓冲区状态。
    /// </summary>
    [Fact]
    public void FlushTo_WithNull_WhenBufferEmpty_ShouldThrowArgumentNullException()
    {
        // Arrange — 通过反射创建空的 InternalTempLogger（缓冲区为空）
        var tempLoggerType = typeof(LogHelper).Assembly
            .GetType("GameFrameX.Foundation.Logger.Internal.InternalTempLogger");
        Assert.NotNull(tempLoggerType);

        ConstructorInfo constructor = tempLoggerType.GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            Type.EmptyTypes,
            null);
        Assert.NotNull(constructor);

        object tempLogger = constructor.Invoke(null);
        Assert.NotNull(tempLogger);

        MethodInfo flushToMethod = tempLoggerType.GetMethod("FlushTo");
        Assert.NotNull(flushToMethod);

        // Act & Assert — 即使 buffer 为空，null targetLogger 仍必须被 guard 拒绝
        var ex = Assert.Throws<TargetInvocationException>(
            () => flushToMethod.Invoke(tempLogger, new object[] { null }));
        var ane = Assert.IsType<ArgumentNullException>(ex.InnerException);
        Assert.Equal("targetLogger", ane.ParamName);

        // Cleanup
        MethodInfo disposeMethod = tempLoggerType.GetMethod("Dispose");
        disposeMethod.Invoke(tempLogger, null);
    }

    // ==================== 清理与辅助 ====================

    private static object GetTempLoggerField()
    {
        var field = typeof(LogHelper).GetField("_tempLogger", BindingFlags.NonPublic | BindingFlags.Static);
        return field?.GetValue(null);
    }

    private static void ResetLoggerState()
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

    /// <inheritdoc />
    public void Dispose()
    {
        ResetLoggerState();
        GC.SuppressFinalize(this);
    }
}
