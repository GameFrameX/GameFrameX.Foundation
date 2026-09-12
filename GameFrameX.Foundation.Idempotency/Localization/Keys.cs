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

namespace GameFrameX.Foundation.Idempotency.Localization;

/// <summary>
/// Idempotency 模块本地化资源键常量定义。
/// </summary>
/// <remarks>
/// 这个类定义了 Idempotency 模块中所有可本地化字符串的键常量。
/// 使用常量可以避免字符串硬编码，提高代码的可维护性和类型安全性。
/// <para>
/// This class defines the key constants of all localizable strings in the Idempotency module.
/// Using constants avoids hardcoded strings and improves maintainability and type safety.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// // 在代码中使用本地化键常量 / Use a localization key constant in code
/// throw new ArgumentException(
///     LocalizationService.GetString(LocalizationKeys.Exceptions.IdempotencyScopeCannotBeBlank),
///     nameof(idempotencyScope));
/// </code>
/// </example>
public static class LocalizationKeys
{
    /// <summary>
    /// 异常消息资源键 / Exception message resource keys。
    /// </summary>
    public static class Exceptions
    {
        /// <summary>幂等作用域不能为空白 / The idempotency scope cannot be blank.</summary>
        public const string IdempotencyScopeCannotBeBlank = "Idempotency.Exceptions.IdempotencyScopeCannotBeBlank";

        /// <summary>幂等键不能为空白 / The idempotency key cannot be blank.</summary>
        public const string IdempotencyKeyCannotBeBlank = "Idempotency.Exceptions.IdempotencyKeyCannotBeBlank";

        /// <summary>请求摘要不能为空白 / The request digest cannot be blank.</summary>
        public const string RequestDigestCannotBeBlank = "Idempotency.Exceptions.RequestDigestCannotBeBlank";

        /// <summary>幂等记录标识不能为 null / The idempotency record identifier cannot be null.</summary>
        public const string IdentifierCannotBeNull = "Idempotency.Exceptions.IdentifierCannotBeNull";

        /// <summary>待占位的幂等记录不能为 null / The idempotency record to occupy cannot be null.</summary>
        public const string RecordCannotBeNull = "Idempotency.Exceptions.RecordCannotBeNull";

        /// <summary>幂等占位请求不能为 null / The idempotency begin request cannot be null.</summary>
        public const string BeginRequestCannotBeNull = "Idempotency.Exceptions.BeginRequestCannotBeNull";

        /// <summary>幂等完成请求不能为 null / The idempotency completion request cannot be null.</summary>
        public const string CompletionRequestCannotBeNull = "Idempotency.Exceptions.CompletionRequestCannotBeNull";

        /// <summary>幂等失败请求不能为 null / The idempotency failure request cannot be null.</summary>
        public const string FailureRequestCannotBeNull = "Idempotency.Exceptions.FailureRequestCannotBeNull";

        /// <summary>过期记录清理请求不能为 null / The expired-record removal request cannot be null.</summary>
        public const string RemovalRequestCannotBeNull = "Idempotency.Exceptions.RemovalRequestCannotBeNull";

        /// <summary>保留时长必须大于 0 毫秒 / The retention duration must be greater than 0 milliseconds.</summary>
        public const string RetentionMillisecondsMustBePositive = "Idempotency.Exceptions.RetentionMillisecondsMustBePositive";

        /// <summary>并发等待超时必须不小于 0 毫秒 / The concurrent wait timeout must not be less than 0 milliseconds.</summary>
        public const string ConcurrentWaitTimeoutMustNotBeNegative = "Idempotency.Exceptions.ConcurrentWaitTimeoutMustNotBeNegative";

        /// <summary>幂等记录存储不能为 null / The idempotency store cannot be null.</summary>
        public const string StoreCannotBeNull = "Idempotency.Exceptions.StoreCannotBeNull";

        /// <summary>时间源不能为 null / The time source cannot be null.</summary>
        public const string ClockCannotBeNull = "Idempotency.Exceptions.ClockCannotBeNull";

        /// <summary>幂等记录不存在，无法落为成功 / No idempotency record found to mark as completed.</summary>
        public const string RecordNotFoundForCompletion = "Idempotency.Exceptions.RecordNotFoundForCompletion";

        /// <summary>幂等记录不存在，无法落为失败 / No idempotency record found to mark as failed.</summary>
        public const string RecordNotFoundForFailure = "Idempotency.Exceptions.RecordNotFoundForFailure";

        /// <summary>字段集合不能为 null / The field collection cannot be null.</summary>
        public const string FieldsCannotBeNull = "Idempotency.Exceptions.FieldsCannotBeNull";

        /// <summary>SHA-256 摘要计算失败 / Failed to compute the SHA-256 digest.</summary>
        public const string DigestComputationFailed = "Idempotency.Exceptions.DigestComputationFailed";

        /// <summary>事件标识不能为空白 / The event identifier (EventId) cannot be blank.</summary>
        public const string EventIdCannotBeBlank = "Idempotency.Exceptions.EventIdCannotBeBlank";

        /// <summary>事件类型不能为空白 / The event type (EventType) cannot be blank.</summary>
        public const string EventTypeCannotBeBlank = "Idempotency.Exceptions.EventTypeCannotBeBlank";

        /// <summary>来源模块不能为空白 / The event source (Source) cannot be blank.</summary>
        public const string SourceCannotBeBlank = "Idempotency.Exceptions.SourceCannotBeBlank";

        /// <summary>结构版本必须不小于 1 / The schema version (SchemaVersion) must be at least 1.</summary>
        public const string SchemaVersionMustBeAtLeastOne = "Idempotency.Exceptions.SchemaVersionMustBeAtLeastOne";

        /// <summary>事件订阅回调不能为 null / The event subscriber callback cannot be null.</summary>
        public const string SubscriberCannotBeNull = "Idempotency.Exceptions.SubscriberCannotBeNull";

        /// <summary>事件信封不能为 null / The event envelope cannot be null.</summary>
        public const string EnvelopeCannotBeNull = "Idempotency.Exceptions.EnvelopeCannotBeNull";

        /// <summary>去重容量必须大于 0 / The deduplication capacity must be greater than 0.</summary>
        public const string CapacityMustBePositive = "Idempotency.Exceptions.CapacityMustBePositive";
    }
}
