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

namespace GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;

/// <summary>
/// Snowflake WorkerId 注册信息。
/// </summary>
public sealed class WorkerIdRegistration
{
    /// <summary>
    /// 初始化 WorkerId 注册信息。
    /// </summary>
    public WorkerIdRegistration(string nodeId, long dataCenterId, long workerId, string providerName, DateTimeOffset registeredAtUtc)
    {
        NodeId = nodeId;
        DataCenterId = dataCenterId;
        WorkerId = workerId;
        ProviderName = providerName;
        RegisteredAtUtc = registeredAtUtc;
    }

    /// <summary>
    /// 节点标识。
    /// </summary>
    public string NodeId { get; }

    /// <summary>
    /// 数据中心 ID。
    /// </summary>
    public long DataCenterId { get; }

    /// <summary>
    /// 工作节点 ID。
    /// </summary>
    public long WorkerId { get; }

    /// <summary>
    /// WorkerId 来源名称。
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// 注册时间。
    /// </summary>
    public DateTimeOffset RegisteredAtUtc { get; }
}

/// <summary>
/// WorkerId 冲突检测结果。
/// </summary>
public sealed class WorkerIdConflictResult
{
    /// <summary>
    /// 初始化 WorkerId 冲突检测结果。
    /// </summary>
    public WorkerIdConflictResult(
        string nodeId,
        long dataCenterId,
        long workerId,
        string providerName,
        bool hasConflict,
        string conflictingNodeId)
    {
        NodeId = nodeId;
        DataCenterId = dataCenterId;
        WorkerId = workerId;
        ProviderName = providerName;
        HasConflict = hasConflict;
        ConflictingNodeId = conflictingNodeId;
    }

    /// <summary>
    /// 当前节点标识。
    /// </summary>
    public string NodeId { get; }

    /// <summary>
    /// 数据中心 ID。
    /// </summary>
    public long DataCenterId { get; }

    /// <summary>
    /// 工作节点 ID。
    /// </summary>
    public long WorkerId { get; }

    /// <summary>
    /// WorkerId 来源名称。
    /// </summary>
    public string ProviderName { get; }

    /// <summary>
    /// 是否存在冲突。
    /// </summary>
    public bool HasConflict { get; }

    /// <summary>
    /// 冲突节点标识；无冲突时为 null。
    /// </summary>
    public string ConflictingNodeId { get; }
}

/// <summary>
/// Snowflake WorkerId 冲突检测器。
/// </summary>
/// <remarks>
/// 检测器维护当前进程可见的节点注册表。分布式部署时可将注册结果同步到外部注册中心后再调用此类型进行校验。
/// </remarks>
public sealed class WorkerIdConflictDetector
{
    private readonly ConcurrentDictionary<string, WorkerIdRegistration> _registrations = new ConcurrentDictionary<string, WorkerIdRegistration>(StringComparer.Ordinal);

    /// <summary>
    /// 检查给定节点是否会与已注册节点发生 WorkerId 冲突，不修改注册表。
    /// </summary>
    public WorkerIdConflictResult Check(string nodeId, long dataCenterId, long workerId, string providerName = null)
    {
        Validate(nodeId, dataCenterId, workerId);
        providerName ??= string.Empty;

        var key = CreateKey(dataCenterId, workerId);
        if (_registrations.TryGetValue(key, out var existing) && !string.Equals(existing.NodeId, nodeId, StringComparison.Ordinal))
        {
            return new WorkerIdConflictResult(nodeId, dataCenterId, workerId, providerName, true, existing.NodeId);
        }

        return new WorkerIdConflictResult(nodeId, dataCenterId, workerId, providerName, false, null);
    }

    /// <summary>
    /// 注册节点的 WorkerId，并返回冲突检测结果。
    /// </summary>
    public WorkerIdConflictResult Register(string nodeId, long dataCenterId, long workerId, string providerName = null)
    {
        var check = Check(nodeId, dataCenterId, workerId, providerName);
        if (check.HasConflict)
        {
            return check;
        }

        var registration = new WorkerIdRegistration(nodeId, dataCenterId, workerId, providerName ?? string.Empty, DateTimeOffset.UtcNow);
        var key = CreateKey(dataCenterId, workerId);
        _registrations.AddOrUpdate(key, registration, (_, existing) => string.Equals(existing.NodeId, nodeId, StringComparison.Ordinal) ? registration : existing);

        return Check(nodeId, dataCenterId, workerId, providerName);
    }

    /// <summary>
    /// 获取当前已注册的节点快照。
    /// </summary>
    public IReadOnlyCollection<WorkerIdRegistration> GetRegistrations()
    {
        return _registrations.Values.ToList().AsReadOnly();
    }

    /// <summary>
    /// 清空注册表。
    /// </summary>
    public void Clear()
    {
        _registrations.Clear();
    }

    private static void Validate(string nodeId, long dataCenterId, long workerId)
    {
        if (string.IsNullOrWhiteSpace(nodeId))
        {
            throw new ArgumentException("NodeId cannot be null, empty, or whitespace.", nameof(nodeId));
        }

        if (dataCenterId < 0 || dataCenterId > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(dataCenterId), "DataCenterId must be between 0 and 31.");
        }

        if (workerId < 0 || workerId > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(workerId), "WorkerId must be between 0 and 31.");
        }
    }

    private static string CreateKey(long dataCenterId, long workerId)
    {
        return string.Concat(dataCenterId, ":", workerId);
    }
}
