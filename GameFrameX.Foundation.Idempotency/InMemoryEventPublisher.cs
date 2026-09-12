// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 Apache License 2.0 许可证分发，
//  This project is distributed under the Apache License 2.0,
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
//  CNB Repository:    https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Idempotency.Localization;
using GameFrameX.Foundation.Localization.Core;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 进程内的 <see cref="IEventPublisher"/> 默认实现 / The in-process default implementation of <see cref="IEventPublisher"/>.
/// </summary>
/// <remarks>
/// 发布前先以 <see cref="EventEnvelope.EnsureValid"/> 校验信封，再按订阅先后顺序同步调用全部订阅者。
/// <para>
/// <b>异常传播策略：</b>任一订阅者抛出异常时立即中断后续订阅者的分发，异常原样向上传播；
/// 已完成分发的订阅者不回滚。
/// </para>
/// <para><see cref="Subscribe"/> 线程安全；分发基于订阅快照，分发期间的并发订阅不影响本次分发。</para>
/// <para>
/// Validates the envelope with <see cref="EventEnvelope.EnsureValid"/> before publishing, then invokes all
/// subscribers synchronously in subscription order.
/// <b>Exception propagation policy:</b> when any subscriber throws, dispatching to subsequent subscribers stops
/// immediately and the exception propagates as-is; subscribers already dispatched are not rolled back.
/// <see cref="Subscribe"/> is thread-safe; dispatching works on a snapshot, so concurrent subscriptions during a
/// dispatch do not affect it.
/// </para>
/// </remarks>
public sealed class InMemoryEventPublisher : IEventPublisher
{
    private readonly object _subscribersLock = new object();
    private readonly List<Action<EventEnvelope>> _subscribers = new List<Action<EventEnvelope>>();

    /// <summary>
    /// 订阅事件：新订阅者排在既有订阅者之后 / Subscribes to events: new subscribers are appended after existing ones.
    /// </summary>
    /// <remarks>
    /// 线程安全，可在任意时刻调用；同一订阅者可重复订阅、按注册顺序多次收到分发。
    /// <para>
    /// Thread-safe and callable at any time; the same subscriber may subscribe multiple times and receives
    /// dispatches once per registration, in registration order.
    /// </para>
    /// </remarks>
    /// <param name="subscriber">事件订阅回调 / The event subscriber callback.</param>
    /// <exception cref="ArgumentNullException"><paramref name="subscriber"/> 为 <see langword="null"/> / <paramref name="subscriber"/> is <see langword="null"/>.</exception>
    public void Subscribe(Action<EventEnvelope> subscriber)
    {
        if (subscriber == null)
        {
            throw new ArgumentNullException(nameof(subscriber), LocalizationService.GetString(LocalizationKeys.Exceptions.SubscriberCannotBeNull));
        }

        lock (_subscribersLock)
        {
            _subscribers.Add(subscriber);
        }
    }

    /// <inheritdoc />
    public Task PublishAsync(EventEnvelope envelope, CancellationToken cancellationToken = default)
    {
        if (envelope == null)
        {
            throw new ArgumentNullException(nameof(envelope), LocalizationService.GetString(LocalizationKeys.Exceptions.EnvelopeCannotBeNull));
        }

        envelope.EnsureValid();

        Action<EventEnvelope>[] subscriberSnapshot;
        lock (_subscribersLock)
        {
            subscriberSnapshot = _subscribers.ToArray();
        }

        foreach (Action<EventEnvelope> subscriber in subscriberSnapshot)
        {
            subscriber(envelope);
        }

        return Task.CompletedTask;
    }
}
