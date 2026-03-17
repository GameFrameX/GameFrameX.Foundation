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
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace GameFrameX.Foundation.Logger.Internal;

/// <summary>
/// 内存日志事件接收器，用于临时缓冲日志事件。
/// </summary>
/// <remarks>
/// <para>该类在日志系统正式初始化前使用，用于：</para>
/// <list type="number">
/// <item><description>缓存日志事件到内存队列</description></item>
/// <item><description>通过回调函数实时输出日志到控制台</description></item>
/// <item><description>支持将缓存的日志事件批量转移到正式日志系统</description></item>
/// </list>
/// <para>当正式日志系统初始化完成后，通过 <see cref="GetEvents"/> 方法获取所有缓存的事件并转移到新日志器。</para>
/// </remarks>
public sealed class LoggerMemorySink : ILogEventSink
{
    private readonly ConcurrentQueue<LogEvent> _events = new();
    private readonly Action<LogEvent> _onEmit;

    /// <summary>
    /// 初始化 <see cref="LoggerMemorySink"/> 类的新实例。
    /// </summary>
    /// <param name="onEmit">可选的回调函数，在每个日志事件被接收时触发。</param>
    /// <remarks>
    /// 如果提供了 <paramref name="onEmit"/> 回调，每次调用 <see cref="Emit"/> 时都会触发该回调，
    /// 可用于实时输出日志到控制台。
    /// </remarks>
    public LoggerMemorySink(Action<LogEvent> onEmit = null)
    {
        _onEmit = onEmit;
    }

    /// <summary>
    /// 接收一个日志事件。
    /// </summary>
    /// <param name="logEvent">要接收的日志事件。</param>
    /// <remarks>
    /// 将日志事件加入内部队列，并可选地触发回调函数。
    /// </remarks>
    public void Emit(LogEvent logEvent)
    {
        _events.Enqueue(logEvent);
        _onEmit?.Invoke(logEvent);
    }

    /// <summary>
    /// 获取并移除所有缓存的日志事件。
    /// </summary>
    /// <returns>包含所有缓存日志事件的枚举器。</returns>
    /// <remarks>
    /// 调用此方法后，内部队列会被清空。
    /// 该方法主要用于将临时缓存的日志事件转移到正式日志系统。
    /// </remarks>
    public IEnumerable<LogEvent> GetEvents()
    {
        while (_events.TryDequeue(out var evt))
        {
            yield return evt;
        }
    }
}
