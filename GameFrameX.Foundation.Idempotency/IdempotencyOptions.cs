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
/// 失败记录的重放策略 / The replay policy for failed records.
/// </summary>
/// <remarks>
/// 决定 <see cref="IdempotencyCoordinator.BeginAsync"/> 遇到 <see cref="IdempotencyStatus.Failed"/> 记录时的行为：
/// 将失败原样回放给调用方，还是允许重新执行。
/// <para>
/// Decides how <see cref="IdempotencyCoordinator.BeginAsync"/> behaves when it meets a
/// <see cref="IdempotencyStatus.Failed"/> record: replay the failure as-is to the caller, or allow re-execution.
/// </para>
/// </remarks>
public enum FailedReplayPolicy
{
    /// <summary>
    /// 回放失败：返回 <see cref="IdempotencyDecisionKind.Replay"/>，调用方依据存量记录的状态映射回原错误 / Replay the failure: returns <see cref="IdempotencyDecisionKind.Replay"/> and the caller maps the stored record's state back to the original error.
    /// </summary>
    ReplayError = 0,

    /// <summary>
    /// 重新执行：返回 <see cref="IdempotencyDecisionKind.Execute"/>，覆盖占位后再次执行业务 / Re-execute: returns <see cref="IdempotencyDecisionKind.Execute"/> and the business runs again after an overwriting occupation.
    /// </summary>
    ReExecute = 1,
}

/// <summary>
/// 幂等协调器选项 / Options for the idempotency coordinator.
/// </summary>
/// <remarks>
/// 默认值：保留时长 24 小时、失败重放策略 <see cref="FailedReplayPolicy.ReplayError"/>、并发等待超时 5 秒。
/// <para>
/// 全部时长一律为 <see langword="long"/> 毫秒；属性赋值即校验，
/// <see cref="RetentionMilliseconds"/> 必须大于 0、<see cref="ConcurrentWaitTimeoutMilliseconds"/> 必须不小于 0，
/// 非法赋值抛出 <see cref="ArgumentException"/>。
/// </para>
/// <para>
/// Defaults: a 24-hour retention duration, the <see cref="FailedReplayPolicy.ReplayError"/> failed-replay policy,
/// and a 5-second concurrent wait timeout.
/// Every duration is a <see langword="long"/> millisecond value; assignment is validated on the spot —
/// <see cref="RetentionMilliseconds"/> must be greater than 0 and <see cref="ConcurrentWaitTimeoutMilliseconds"/>
/// must not be less than 0, otherwise an <see cref="ArgumentException"/> is thrown.
/// </para>
/// </remarks>
public sealed class IdempotencyOptions
{
    /// <summary>
    /// 保留时长的默认值：86 400 000 毫秒（24 小时）/ The default retention duration: 86,400,000 milliseconds (24 hours).
    /// </summary>
    public const long DefaultRetentionMilliseconds = 86_400_000L;

    /// <summary>
    /// 并发等待超时的默认值：5 000 毫秒（5 秒）/ The default concurrent wait timeout: 5,000 milliseconds (5 seconds).
    /// </summary>
    public const long DefaultConcurrentWaitTimeoutMilliseconds = 5_000L;

    private long _retentionMilliseconds = DefaultRetentionMilliseconds;
    private long _concurrentWaitTimeoutMilliseconds = DefaultConcurrentWaitTimeoutMilliseconds;

    /// <summary>
    /// 初始化 <see cref="IdempotencyOptions"/>，全部选项取默认值 / Initializes <see cref="IdempotencyOptions"/> with all options at their defaults.
    /// </summary>
    public IdempotencyOptions()
    {
    }

    /// <summary>
    /// 幂等记录的保留时长（毫秒）/ The retention duration of idempotency records (milliseconds).
    /// </summary>
    /// <remarks>
    /// 记录的过期时刻 = 创建时刻 + 本值；当前时刻超过过期时刻后记录视为不存在，可被新占位覆盖。
    /// 必须大于 0，非法赋值抛出 <see cref="ArgumentException"/>。
    /// <para>
    /// A record's expiration time = its creation time + this value; once the current time passes the expiration time,
    /// the record is treated as absent and may be overwritten by a new occupation.
    /// Must be greater than 0; an invalid assignment throws <see cref="ArgumentException"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">赋值不大于 0 / The assigned value is not greater than 0.</exception>
    public long RetentionMilliseconds
    {
        get
        {
            return _retentionMilliseconds;
        }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.RetentionMillisecondsMustBePositive), nameof(value));
            }

            _retentionMilliseconds = value;
        }
    }

    /// <summary>
    /// 失败记录的重放策略，默认 <see cref="FailedReplayPolicy.ReplayError"/> / The replay policy for failed records; defaults to <see cref="FailedReplayPolicy.ReplayError"/>.
    /// </summary>
    public FailedReplayPolicy FailedReplayPolicy
    {
        get;
        set;
    }

    /// <summary>
    /// 等待并发占位方的超时时长（毫秒）/ The timeout for waiting on a concurrent occupant (milliseconds).
    /// </summary>
    /// <remarks>
    /// 遇到 <see cref="IdempotencyStatus.Processing"/> 记录时最多等待该时长后重判；
    /// 仍为执行中则返回 <see cref="IdempotencyDecisionKind.Busy"/>。设为 0 表示不等待、立即判定。
    /// 必须不小于 0，非法赋值抛出 <see cref="ArgumentException"/>。
    /// <para>
    /// Upon meeting a <see cref="IdempotencyStatus.Processing"/> record the coordinator waits at most this long
    /// before re-judging; if the record is still processing it returns <see cref="IdempotencyDecisionKind.Busy"/>.
    /// Setting 0 means no waiting — judge immediately. Must not be less than 0;
    /// an invalid assignment throws <see cref="ArgumentException"/>.
    /// </para>
    /// </remarks>
    /// <exception cref="ArgumentException">赋值小于 0 / The assigned value is less than 0.</exception>
    public long ConcurrentWaitTimeoutMilliseconds
    {
        get
        {
            return _concurrentWaitTimeoutMilliseconds;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.ConcurrentWaitTimeoutMustNotBeNegative), nameof(value));
            }

            _concurrentWaitTimeoutMilliseconds = value;
        }
    }
}
