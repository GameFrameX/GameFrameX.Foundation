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

using System;
using Serilog;
using Serilog.Events;
using Serilog.Core;

namespace GameFrameX.Foundation.Logger.Internal;

/// <summary>
/// 内部临时日志记录器，用于在日志系统初始化前临时存储日志事件。
/// </summary>
/// <remarks>
/// This class provides a temporary logger that buffers log events before the main logging system is initialized.
/// It allows log messages to be captured early in the application lifecycle and then flushed to the main logger once available.
/// </remarks>
internal sealed class InternalTempLogger : IDisposable
{
    private readonly ILogger _logger;
    private readonly LoggerMemorySink _buffer;
    private bool _isFlushed;
    private bool _isDisposed;

    /// <summary>
    /// 获取日志记录器实例。 / Gets the logger instance.
    /// </summary>
    /// <value>The ILogger instance.</value>
    public ILogger Logger
    {
        get { return _logger; }
    }

    /// <summary>
    /// 获取日志内存缓冲区。 / Gets the log memory buffer.
    /// </summary>
    /// <value>The LoggerMemorySink instance.</value>
    public LoggerMemorySink Buffer
    {
        get { return _buffer; }
    }

    /// <summary>
    /// 初始化内部临时日志记录器的新实例。
    /// </summary>
    /// <remarks>
    /// Creates a new internal temporary logger with a memory sink that captures all log events at verbose level.
    /// </remarks>
    public InternalTempLogger()
    {
        _buffer = new LoggerMemorySink(OnLogEvent);
        _logger = new LoggerConfiguration()
                  .MinimumLevel.Verbose()
                  .WriteTo.Sink(_buffer)
                  .CreateLogger();
    }

    /// <summary>
    /// 处理日志事件的回调方法。 / Callback method for handling log events.
    /// </summary>
    /// <param name="evt">日志事件 / The log event</param>
    private void OnLogEvent(LogEvent evt)
    {
        Console.WriteLine(evt.RenderMessage());
    }

    /// <summary>
    /// 将缓冲的日志事件刷新到目标日志记录器。
    /// </summary>
    /// <param name="targetLogger">目标日志记录器实例 / The target logger instance</param>
    /// <remarks>
    /// Writes all buffered log events to the target logger. This method can only be called once;
    /// subsequent calls will be ignored. Also ignores calls if the logger has been disposed.
    /// </remarks>
    public void FlushTo(ILogger targetLogger)
    {
        if (_isFlushed || _isDisposed)
        {
            return;
        }

        foreach (var evt in _buffer.GetEvents())
        {
            targetLogger.Write(evt);
        }

        _isFlushed = true;
    }

    /// <summary>
    /// 释放临时日志记录器使用的资源。
    /// </summary>
    /// <remarks>
    /// Disposes the internal logger if it implements IDisposable. This method can only be called once;
    /// subsequent calls will be ignored.
    /// </remarks>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        if (_logger is IDisposable disposable)
        {
            disposable.Dispose();
        }

        _isDisposed = true;
    }
}
