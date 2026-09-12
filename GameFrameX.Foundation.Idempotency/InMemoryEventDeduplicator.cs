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
/// 进程内的 <see cref="IEventDeduplicator"/> 默认实现 / The in-process default implementation of <see cref="IEventDeduplicator"/>.
/// </summary>
/// <remarks>
/// 有界容量、按插入顺序逐出：登记数达到容量后，最早登记的标识被逐出并允许其再次通过。
/// <para>
/// <b>尽力而为边界：数据仅存活于当前进程内存，且逐出后旧标识可能再次通过。</b>
/// 本实现只能抑制短窗口内的重复消费，不提供跨进程、跨重启或超长窗口的强去重语义；
/// 强语义场景必须替换为持久化实现。
/// </para>
/// <para>线程安全。</para>
/// <para>
/// Bounded capacity with insertion-order eviction: once the registration count reaches the capacity, the earliest
/// registered identifier is evicted and may pass again.
/// <b>Best-effort boundary: data lives only in the current process's memory, and an evicted old identifier may pass
/// again.</b> This implementation only suppresses duplicates within a short window; it provides no strong
/// deduplication semantics across processes, restarts or very long windows — strong-semantics scenarios must switch
/// to a persistent implementation.
/// Thread-safe.
/// </para>
/// </remarks>
public sealed class InMemoryEventDeduplicator : IEventDeduplicator
{
    /// <summary>
    /// 默认容量：100 000 个事件标识 / The default capacity: 100,000 event identifiers.
    /// </summary>
    public const int DefaultCapacity = 100_000;

    private readonly object _consumeLock = new object();
    private readonly HashSet<string> _consumedEventIds;
    private readonly Queue<string> _consumptionOrderQueue = new Queue<string>();
    private readonly int _capacity;

    /// <summary>
    /// 初始化 <see cref="InMemoryEventDeduplicator"/> / Initializes <see cref="InMemoryEventDeduplicator"/>.
    /// </summary>
    /// <param name="capacity">可记忆的事件标识容量，达到后按插入顺序逐出最早登记的标识 / The capacity of memorable event identifiers; once reached, the earliest registered identifier is evicted in insertion order.</param>
    /// <exception cref="ArgumentException"><paramref name="capacity"/> 不大于 0 / <paramref name="capacity"/> is not greater than 0.</exception>
    public InMemoryEventDeduplicator(int capacity = DefaultCapacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.CapacityMustBePositive), nameof(capacity));
        }

        _capacity = capacity;
        _consumedEventIds = new HashSet<string>(capacity);
    }

    /// <inheritdoc />
    public bool TryConsume(string eventId)
    {
        if (string.IsNullOrWhiteSpace(eventId))
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.EventIdCannotBeBlank), nameof(eventId));
        }

        lock (_consumeLock)
        {
            if (_consumedEventIds.Contains(eventId))
            {
                return false;
            }

            if (_consumedEventIds.Count >= _capacity)
            {
                string evictedEventId = _consumptionOrderQueue.Dequeue();
                _consumedEventIds.Remove(evictedEventId);
            }

            _consumedEventIds.Add(eventId);
            _consumptionOrderQueue.Enqueue(eventId);
            return true;
        }
    }
}
