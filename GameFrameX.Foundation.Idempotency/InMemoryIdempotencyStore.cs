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

using System.Collections.Concurrent;
using GameFrameX.Foundation.Idempotency.Localization;
using GameFrameX.Foundation.Localization.Core;

namespace GameFrameX.Foundation.Idempotency;

/// <summary>
/// 进程内的 <see cref="IIdempotencyStore"/> 默认实现 / The in-process default implementation of <see cref="IIdempotencyStore"/>.
/// </summary>
/// <remarks>
/// 基于 <see cref="ConcurrentDictionary{TKey, TValue}"/> 与逐键锁实现并发恰一语义，无内部时钟，全部时间由调用方传入。
/// <para>
/// <b>边界声明：数据仅存活于当前进程内存，进程重启即失忆。</b>
/// 适用于单进程防并发重复与单元测试；
/// 需要跨进程、跨重启的幂等正确性时，必须替换为持久化实现（例如数据库存储）。
/// </para>
/// <para>线程安全；无后台清理线程，过期清理由调用方驱动 <see cref="RemoveExpiredRecordsAsync"/>。</para>
/// <para>
/// Implements exactly-once concurrency with a <see cref="ConcurrentDictionary{TKey, TValue}"/> and per-key locks;
/// it has no internal clock — every time value is supplied by the caller.
/// <b>Boundary statement: data lives only in the current process's memory; a process restart wipes it.</b>
/// Suitable for single-process duplicate suppression and unit tests; cross-process or cross-restart idempotency
/// correctness requires a persistent implementation (for example a database store).
/// Thread-safe; no background cleanup thread — expired cleanup is caller-driven via <see cref="RemoveExpiredRecordsAsync"/>.
/// </para>
/// </remarks>
public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<(string IdempotencyScope, string IdempotencyKey), IdempotencyRecord> _records = new ConcurrentDictionary<(string IdempotencyScope, string IdempotencyKey), IdempotencyRecord>();
    private readonly ConcurrentDictionary<(string IdempotencyScope, string IdempotencyKey), object> _keyLocks = new ConcurrentDictionary<(string IdempotencyScope, string IdempotencyKey), object>();

    /// <inheritdoc />
    public Task<bool> TryBeginProcessingAsync(IdempotencyRecord newRecord, CancellationToken cancellationToken = default)
    {
        if (newRecord == null)
        {
            throw new ArgumentNullException(nameof(newRecord), LocalizationService.GetString(LocalizationKeys.Exceptions.RecordCannotBeNull));
        }

        var recordKey = (newRecord.IdempotencyScope, newRecord.IdempotencyKey);
        lock (GetKeyLock(recordKey))
        {
            if (_records.TryGetValue(recordKey, out var existingRecord))
            {
                // 仅两类存量允许被覆盖：已过期（以新记录创建时刻为基准）、已失败（供失败重执行策略覆盖占位）。
                // Only two kinds of existing records may be overwritten: expired ones (judged against the new
                // record's creation time) and failed ones (for the failed-re-execute policy to re-occupy).
                if (existingRecord.ExpiredTime >= newRecord.CreatedTime && existingRecord.Status != IdempotencyStatus.Failed)
                {
                    return Task.FromResult(false);
                }

                _records[recordKey] = newRecord;
                return Task.FromResult(true);
            }

            _records[recordKey] = newRecord;
            return Task.FromResult(true);
        }
    }

    /// <inheritdoc />
    public Task CompleteAsync(IdempotencyCompletionRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), LocalizationService.GetString(LocalizationKeys.Exceptions.CompletionRequestCannotBeNull));
        }

        var recordKey = (request.Identifier.IdempotencyScope, request.Identifier.IdempotencyKey);
        lock (GetKeyLock(recordKey))
        {
            if (!_records.TryGetValue(recordKey, out var existingRecord))
            {
                throw new InvalidOperationException(LocalizationService.GetString(LocalizationKeys.Exceptions.RecordNotFoundForCompletion, request.Identifier.IdempotencyScope, request.Identifier.IdempotencyKey));
            }

            _records[recordKey] = new IdempotencyRecord(existingRecord.IdempotencyScope, existingRecord.IdempotencyKey, existingRecord.RequestDigest, IdempotencyStatus.Completed, request.FirstResponse, existingRecord.CreatedTime, existingRecord.ExpiredTime, request.CompletedTime);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task FailAsync(IdempotencyFailureRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), LocalizationService.GetString(LocalizationKeys.Exceptions.FailureRequestCannotBeNull));
        }

        var recordKey = (request.Identifier.IdempotencyScope, request.Identifier.IdempotencyKey);
        lock (GetKeyLock(recordKey))
        {
            if (!_records.TryGetValue(recordKey, out var existingRecord))
            {
                throw new InvalidOperationException(LocalizationService.GetString(LocalizationKeys.Exceptions.RecordNotFoundForFailure, request.Identifier.IdempotencyScope, request.Identifier.IdempotencyKey));
            }

            _records[recordKey] = new IdempotencyRecord(existingRecord.IdempotencyScope, existingRecord.IdempotencyKey, existingRecord.RequestDigest, IdempotencyStatus.Failed, existingRecord.FirstResponse, existingRecord.CreatedTime, existingRecord.ExpiredTime, request.FailedTime);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IdempotencyRecord?> TryGetRecordAsync(IdempotencyRecordIdentifier identifier, CancellationToken cancellationToken = default)
    {
        if (identifier == null)
        {
            throw new ArgumentNullException(nameof(identifier), LocalizationService.GetString(LocalizationKeys.Exceptions.IdentifierCannotBeNull));
        }

        var recordKey = (identifier.IdempotencyScope, identifier.IdempotencyKey);
        _records.TryGetValue(recordKey, out var record);
        return Task.FromResult<IdempotencyRecord?>(record);
    }

    /// <inheritdoc />
    public Task<int> RemoveExpiredRecordsAsync(ExpiredRecordRemovalRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), LocalizationService.GetString(LocalizationKeys.Exceptions.RemovalRequestCannotBeNull));
        }

        var removedCount = 0;
        foreach (var pair in _records)
        {
            if (pair.Value.ExpiredTime >= request.NowTime)
            {
                continue;
            }

            lock (GetKeyLock(pair.Key))
            {
                if (_records.TryGetValue(pair.Key, out var currentRecord) && currentRecord.ExpiredTime < request.NowTime)
                {
                    if (_records.TryRemove(pair.Key, out _))
                    {
                        removedCount++;
                    }
                }
            }
        }

        return Task.FromResult(removedCount);
    }

    private object GetKeyLock((string IdempotencyScope, string IdempotencyKey) recordKey)
    {
        return _keyLocks.GetOrAdd(recordKey, static key => new object());
    }
}
