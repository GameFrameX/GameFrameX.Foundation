// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using GameFrameX.Foundation.Localization.Core;
using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Localization.Providers;

/// <summary>
/// 默认资源提供者 - 提供基础的本地化消息作为后备
/// </summary>
/// <remarks>
/// 该提供者包含预定义的基础消息，当其他资源提供者无法找到
/// 对应的本地化字符串时，将使用此提供者返回默认值。
/// 主要用作本地化体系的最后一道防线，确保系统总是能返回有意义的消息。
/// </remarks>
public class DefaultResourceProvider : IResourceProvider
{
    private readonly ConcurrentDictionary<string, string> _messages;

    /// <summary>
    /// 初始化 DefaultResourceProvider 的新实例
    /// </summary>
    /// <remarks>
    /// 构造函数中初始化了基础的异常消息和通用消息字典。
    /// 这些消息主要使用英文，确保在任何情况下都有合理的后备值。
    /// </remarks>
    public DefaultResourceProvider()
    {
        _messages = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // 时间相关异常消息
            ["TimerHelper.TimestampOutOfRange"] = "Timestamp is out of valid range for DateTime conversion.",
            ["TimerHelper.TimestampSecondsOutOfRange"] = "Timestamp seconds is out of valid range for DateTime conversion.",
            ["TimerHelper.TimestampMillisOutOfRange"] = "Timestamp milliseconds is out of valid range for DateTime conversion.",

            // 系统时钟相关消息
            ["InvalidSystemClock.ClockMovedBackwards"] = "System clock has moved backwards, unable to generate unique ID.",
            ["InvalidSystemClock.ClockInvalid"] = "System clock is in an invalid state.",

            // 加密相关异常消息
            ["Encryption.InvalidKeySize"] = "Invalid key size: {0}. Expected length: {1}.",
            ["Encryption.InvalidKeyFormat"] = "Invalid key format. Expected {0}, but got {1}.",
            ["Encryption.EncryptionFailed"] = "Encryption operation failed.",
            ["Encryption.DecryptionFailed"] = "Decryption operation failed.",
            ["Encryption.InvalidData"] = "Invalid encrypted data format.",

            // RSA 相关消息
            ["Rsa.InvalidParameters"] = "Invalid RSA parameters provided.",
            ["Rsa.KeyGenerationFailed"] = "Failed to generate RSA key pair.",

            // 哈希相关消息
            ["Hash.EmptyInput"] = "Input data cannot be empty.",
            ["Hash.InvalidAlgorithm"] = "Invalid hash algorithm specified.",

            // HTTP 相关消息
            ["Http.RequestTimeout"] = "HTTP request timeout.",
            ["Http.ConnectionFailed"] = "Failed to establish HTTP connection.",
            ["Http.InvalidResponse"] = "Received invalid HTTP response.",

            // JSON 相关消息
            ["Json.SerializationFailed"] = "JSON serialization failed.",
            ["Json.DeserializationFailed"] = "JSON deserialization failed.",
            ["Json.InvalidFormat"] = "Invalid JSON format.",

            // 扩展方法相关消息
            ["Extensions.CollectionNull"] = "Collection cannot be null.",
            ["Extensions.EnumerableNull"] = "Enumerable cannot be null.",
            ["Extensions.StringNullOrEmpty"] = "String cannot be null or empty.",

            // 通用异常消息
            ["ArgumentNull"] = "Value cannot be null.",
            ["ArgumentOutOfRange"] = "Value is out of range.",
            ["ArgumentEmpty"] = "Value cannot be empty.",
            ["ArgumentInvalid"] = "Invalid argument provided.",
            ["OperationNotSupported"] = "Operation is not supported.",
            ["OperationFailed"] = "Operation failed.",

            // 通用成功和状态消息
            ["Success"] = "Success",
            ["Failure"] = "Failure",
            ["Loading"] = "Loading...",
            ["NotAvailable"] = "Not available",
            ["Unknown"] = "Unknown",
            ["Pending"] = "Pending",
            ["Completed"] = "Completed",
            ["Cancelled"] = "Cancelled",
            ["Timeout"] = "Timeout",
            ["Error"] = "Error",
            ["Warning"] = "Warning",
            ["Information"] = "Information",

            // 验证相关消息
            ["Validation.Required"] = "Field is required.",
            ["Validation.InvalidFormat"] = "Invalid format.",
            ["Validation.OutOfRange"] = "Value is out of valid range.",
            ["Validation.MaxLength"] = "Value exceeds maximum length.",
            ["Validation.MinLength"] = "Value is below minimum length.",

            // 权限相关消息
            ["Permission.Denied"] = "Access denied.",
            ["Permission.Insufficient"] = "Insufficient permissions.",
            ["Permission.Required"] = "Permission required to perform this action.",

            // 网络相关消息
            ["Network.Unreachable"] = "Network is unreachable.",
            ["Network.ConnectionLost"] = "Network connection was lost.",
            ["Network.RequestFailed"] = "Network request failed.",

            // 存储相关消息
            ["Storage.NotFound"] = "Item not found.",
            ["Storage.AlreadyExists"] = "Item already exists.",
            ["Storage.AccessDenied"] = "Storage access denied.",
            ["Storage.InsufficientSpace"] = "Insufficient storage space.",

            // 资源管理消息
            ["Resource.NotFound"] = "Resource not found.",
            ["Resource.Unavailable"] = "Resource is temporarily unavailable.",
            ["Resource.Exhausted"] = "Resource has been exhausted."
        };
    }

    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key">资源键</param>
    /// <returns>
    /// 如果找到对应的默认消息，返回该消息；
    /// 如果未找到，返回传入的资源键作为后备值
    /// </returns>
    /// <remarks>
    /// 此方法是线程安全的，支持并发调用。
    /// 当传入的键为 null 或空字符串时，直接返回该键。
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new DefaultResourceProvider();
    /// var message = provider.GetString("ArgumentNull");
    /// // 返回: "Value cannot be null."
    ///
    /// var unknown = provider.GetString("Some.Unknown.Key");
    /// // 返回: "Some.Unknown.Key"
    /// </code>
    /// </example>
    public string GetString(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return key;
        }

        return _messages.TryGetValue(key, out var value) ? value : key;
    }

    /// <summary>
    /// 添加或更新默认消息
    /// </summary>
    /// <param name="key">资源键</param>
    /// <param name="message">本地化消息</param>
    /// <remarks>
    /// 允许运行时动态添加或更新默认消息。
    /// 此方法是线程安全的。
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new DefaultResourceProvider();
    /// provider.AddOrUpdateMessage("Custom.Message", "This is a custom message");
    /// var message = provider.GetString("Custom.Message");
    /// // 返回: "This is a custom message"
    /// </code>
    /// </example>
    public void AddOrUpdateMessage(string key, string message)
    {
        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(message))
        {
            _messages[key] = message;
        }
    }

    /// <summary>
    /// 检查是否包含指定键的默认消息
    /// </summary>
    /// <param name="key">资源键</param>
    /// <returns>如果包含指定键的消息，返回 true；否则返回 false</returns>
    /// <example>
    /// <code>
    /// var provider = new DefaultResourceProvider();
    /// bool hasKey = provider.ContainsKey("ArgumentNull");
    /// // 返回: true
    ///
    /// bool hasUnknown = provider.ContainsKey("Unknown.Key");
    /// // 返回: false
    /// </code>
    /// </example>
    public bool ContainsKey(string key)
    {
        return !string.IsNullOrEmpty(key) && _messages.ContainsKey(key);
    }

    /// <summary>
    /// 获取所有默认消息键的集合
    /// </summary>
    /// <returns>包含所有消息键的只读集合</returns>
    /// <remarks>
    /// 返回的集合是只读的，不能修改。
    /// 可以用于调试或查看所有可用的默认消息。
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new DefaultResourceProvider();
    /// var allKeys = provider.GetAllKeys();
    /// foreach (var key in allKeys)
    /// {
    ///     Console.WriteLine(key);
    /// }
    /// </code>
    /// </example>
    public IReadOnlyCollection<string> GetAllKeys()
    {
        return _messages.Keys.ToList().AsReadOnly();
    }
}