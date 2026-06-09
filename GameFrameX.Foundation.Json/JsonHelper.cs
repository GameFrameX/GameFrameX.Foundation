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

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Unicode;

namespace GameFrameX.Foundation.Json;

/// <summary>
/// JSON序列化和反序列化的帮助类。
/// 提供了默认和格式化两种序列化配置选项，支持枚举字符串转换、忽略null值、忽略循环引用等功能。
/// </summary>
/// <remarks>
/// Helper class for JSON serialization and deserialization.
/// Provides default and formatted serialization configuration options, supports enum string conversion, ignoring null values, ignoring circular references, and other features.
/// </remarks>
public static class JsonHelper
{
    static JsonHelper()
    {
        DefaultOptions.MakeReadOnly(true);
        FormatOptions.MakeReadOnly(true);
    }

    /// <summary>
    /// 默认序列化配置。
    /// 包含以下特性:
    /// <list type="bullet">
    /// <item><description>忽略null值属性 - 序列化时不输出值为null的属性</description></item>
    /// <item><description>忽略循环引用 - 避免因对象间循环引用导致的序列化异常</description></item>
    /// <item><description>属性名称大小写不敏感 - 反序列化时属性名称匹配不区分大小写</description></item>
    /// <item><description>枚举值序列化为字符串 - 枚举值输出为其名称字符串而非数字</description></item>
    /// <item><description>允许从字符串读取数字 - 支持将字符串形式的数字反序列化为数值类型</description></item>
    /// <item><description>支持特殊浮点数值 - 可处理NaN、Infinity等特殊浮点数</description></item>
    /// <item><description>允许JSON注释 - 可以包含并跳过注释内容</description></item>
    /// <item><description>允许尾随逗号 - JSON对象或数组的最后一项后可以有逗号</description></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Default serialization configuration.
    /// Includes the following features:
    /// <list type="bullet">
    /// <item><description>Ignore null value properties - Do not output properties with null values during serialization</description></item>
    /// <item><description>Ignore circular references - Avoid serialization exceptions caused by circular references between objects</description></item>
    /// <item><description>Case-insensitive property names - Property name matching is case-insensitive during deserialization</description></item>
    /// <item><description>Serialize enum values as strings - Enum values are output as their name strings instead of numbers</description></item>
    /// <item><description>Allow reading numbers from strings - Support deserializing string-form numbers to numeric types</description></item>
    /// <item><description>Support special floating-point values - Can handle special floating-point numbers like NaN, Infinity</description></item>
    /// <item><description>Allow JSON comments - Can contain and skip comment content</description></item>
    /// <item><description>Allow trailing commas - A comma can follow the last item in a JSON object or array</description></item>
    /// </list>
    /// </remarks>
    /// <value>默认的 <see cref="JsonSerializerOptions"/> 实例 / Default <see cref="JsonSerializerOptions"/> instance</value>
    public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
    {
        // 忽略 null 值
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // 忽略循环引用
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        // 忽略注释
        ReadCommentHandling = JsonCommentHandling.Skip,
        // 使用 UnicodeJsonEncoder.Singleton 进行编码，不转义中文字符和Emoji
        Encoder = UnicodeJsonEncoder.Singleton,
        // 不使用属性名称转换
        PropertyNamingPolicy = null,
        // 允许以逗号结尾
        AllowTrailingCommas = true,
        // 不区分大小写
        PropertyNameCaseInsensitive = true,
        // 如何处理序列化和反序列化未知类型。
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        // 数字处理选项:
        // - AllowReadingFromString: 允许从字符串中读取数字
        // - AllowNamedFloatingPointLiterals: 允许特殊浮点数值(如 NaN, Infinity)
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
        // 添加自定义转换器
        Converters =
        {
            new JsonStringEnumConverter(), // 处理枚举为字符串
            new SpecialFloatingPointConverter(), // 处理特殊浮点值 (double)
            new SpecialFloatingPointConverterFloat(), // 处理特殊浮点值 (float)
            new SpecialFloatingPointDocumentConverter(), // 处理JSON文档中的特殊浮点值
        },
    };

    /// <summary>
    /// 格式化序列化配置。
    /// 在默认配置基础上增加了JSON格式化功能。
    /// 包含以下特性:
    /// <list type="bullet">
    /// <item><description>忽略null值属性 - 序列化时不输出值为null的属性</description></item>
    /// <item><description>忽略循环引用 - 避免因对象间循环引用导致的序列化异常</description></item>
    /// <item><description>属性名称大小写不敏感 - 反序列化时属性名称匹配不区分大小写</description></item>
    /// <item><description>枚举值序列化为字符串 - 枚举值输出为其名称字符串而非数字</description></item>
    /// <item><description>输出格式化的JSON文本 - 包含适当的缩进和换行，提高可读性</description></item>
    /// <item><description>允许从字符串读取数字 - 支持将字符串形式的数字反序列化为数值类型</description></item>
    /// <item><description>支持特殊浮点数值 - 可处理NaN、Infinity等特殊浮点数</description></item>
    /// <item><description>允许JSON注释 - 可以包含并跳过注释内容</description></item>
    /// <item><description>允许尾随逗号 - JSON对象或数组的最后一项后可以有逗号</description></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// Formatted serialization configuration.
    /// Adds JSON formatting functionality on top of the default configuration.
    /// Includes the following features:
    /// <list type="bullet">
    /// <item><description>Ignore null value properties - Do not output properties with null values during serialization</description></item>
    /// <item><description>Ignore circular references - Avoid serialization exceptions caused by circular references between objects</description></item>
    /// <item><description>Case-insensitive property names - Property name matching is case-insensitive during deserialization</description></item>
    /// <item><description>Serialize enum values as strings - Enum values are output as their name strings instead of numbers</description></item>
    /// <item><description>Output formatted JSON text - Contains appropriate indentation and line breaks for improved readability</description></item>
    /// <item><description>Allow reading numbers from strings - Support deserializing string-form numbers to numeric types</description></item>
    /// <item><description>Support special floating-point values - Can handle special floating-point numbers like NaN, Infinity</description></item>
    /// <item><description>Allow JSON comments - Can contain and skip comment content</description></item>
    /// <item><description>Allow trailing commas - A comma can follow the last item in a JSON object or array</description></item>
    /// </list>
    /// </remarks>
    /// <value>格式化的 <see cref="JsonSerializerOptions"/> 实例 / Formatted <see cref="JsonSerializerOptions"/> instance</value>
    public static readonly JsonSerializerOptions FormatOptions = new JsonSerializerOptions
    {
        // 忽略 null 值
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // 忽略循环引用
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        // 忽略注释
        ReadCommentHandling = JsonCommentHandling.Skip,
        // 使用 UnicodeJsonEncoder.Singleton 进行编码，不转义中文字符和Emoji
        Encoder = UnicodeJsonEncoder.Singleton,
        // 不使用属性名称转换
        PropertyNamingPolicy = null,
        // 允许以逗号结尾
        AllowTrailingCommas = true,
        // 不区分大小写
        PropertyNameCaseInsensitive = true,
        // 如何处理序列化和反序列化未知类型。
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        // 数字处理选项:
        // - AllowReadingFromString: 允许从字符串中读取数字
        // - AllowNamedFloatingPointLiterals: 允许特殊浮点数值(如 NaN, Infinity)
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals,
        // 格式化 JSON
        WriteIndented = true,
        // 添加自定义转换器
        Converters =
        {
            new JsonStringEnumConverter(), // 处理枚举为字符串
            new SpecialFloatingPointConverter(), // 处理特殊浮点值 (double)
            new SpecialFloatingPointConverterFloat(), // 处理特殊浮点值 (float)
            new SpecialFloatingPointDocumentConverter(), // 处理JSON文档中的特殊浮点值
        },
    };

    /// <summary>
    /// 创建默认配置的可变副本。
    /// </summary>
    /// <remarks>
    /// Creates a mutable copy of the default options.
    /// </remarks>
    /// <returns>默认配置副本 / A copy of the default options</returns>
    public static JsonSerializerOptions CreateDefaultOptions()
    {
        return new JsonSerializerOptions(DefaultOptions);
    }

    /// <summary>
    /// 创建格式化配置的可变副本。
    /// </summary>
    /// <remarks>
    /// Creates a mutable copy of the formatted options.
    /// </remarks>
    /// <returns>格式化配置副本 / A copy of the formatted options</returns>
    public static JsonSerializerOptions CreateFormatOptions()
    {
        return new JsonSerializerOptions(FormatOptions);
    }

    /// <summary>
    /// 将对象序列化为JSON字符串。
    /// 使用默认序列化配置(DefaultOptions)。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a JSON string.
    /// Uses the default serialization configuration (DefaultOptions).
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <returns>序列化后的JSON字符串 / The serialized JSON string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="obj"/> 为 null 时抛出 / Thrown when <paramref name="obj"/> is null</exception>
    public static string Serialize(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        var json = JsonSerializer.Serialize(obj, DefaultOptions);
        return json;
    }

    /// <summary>
    /// 将对象序列化为JSON字符串。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a JSON string.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <returns>序列化后的JSON字符串 / The serialized JSON string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="obj"/> 或 <paramref name="options"/> 为 null 时抛出 / Thrown when <paramref name="obj"/> or <paramref name="options"/> is null</exception>
    public static string Serialize(object obj, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        var json = JsonSerializer.Serialize(obj, options);
        return json;
    }

    /// <summary>
    /// 使用源生成元数据将对象序列化为JSON字符串。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a JSON string using source-generated metadata.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>序列化后的JSON字符串 / The serialized JSON string</returns>
    public static string Serialize<T>(T obj, JsonTypeInfo<T> jsonTypeInfo)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        ArgumentNullException.ThrowIfNull(jsonTypeInfo, nameof(jsonTypeInfo));
        return JsonSerializer.Serialize(obj, jsonTypeInfo);
    }

    /// <summary>
    /// 将对象异步序列化到UTF8 JSON流。
    /// </summary>
    /// <remarks>
    /// Asynchronously serializes an object to a UTF-8 JSON stream.
    /// </remarks>
    /// <param name="stream">目标流 / Target stream</param>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="options">序列化配置 / Serialization options</param>
    /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>异步任务 / Async task</returns>
    public static Task SerializeAsync<T>(Stream stream, T obj, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        return JsonSerializer.SerializeAsync(stream, obj, options ?? DefaultOptions, cancellationToken);
    }

    /// <summary>
    /// 使用源生成元数据将对象异步序列化到UTF8 JSON流。
    /// </summary>
    /// <remarks>
    /// Asynchronously serializes an object to a UTF-8 JSON stream using source-generated metadata.
    /// </remarks>
    /// <param name="stream">目标流 / Target stream</param>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>异步任务 / Async task</returns>
    public static Task SerializeAsync<T>(Stream stream, T obj, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        ArgumentNullException.ThrowIfNull(jsonTypeInfo, nameof(jsonTypeInfo));
        return JsonSerializer.SerializeAsync(stream, obj, jsonTypeInfo, cancellationToken);
    }

    /// <summary>
    /// 将对象序列化为格式化的JSON字符串。
    /// 使用格式化序列化配置(FormatOptions)，生成的JSON包含适当的缩进和换行。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a formatted JSON string.
    /// Uses the formatted serialization configuration (FormatOptions), the generated JSON contains appropriate indentation and line breaks.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <returns>格式化后的JSON字符串 / The formatted JSON string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="obj"/> 为 null 时抛出 / Thrown when <paramref name="obj"/> is null</exception>
    public static string SerializeFormat(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        var json = JsonSerializer.Serialize(obj, FormatOptions);
        return json;
    }

    /// <summary>
    /// 将对象序列化为UTF8编码的字节数组。
    /// 使用默认序列化配置(DefaultOptions)。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a UTF-8 encoded byte array.
    /// Uses the default serialization configuration (DefaultOptions).
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <returns>序列化后的UTF8字节数组 / The serialized UTF-8 byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="obj"/> 为 null 时抛出 / Thrown when <paramref name="obj"/> is null</exception>
    public static byte[] SerializeToUtf8Bytes(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        return JsonSerializer.SerializeToUtf8Bytes(obj, DefaultOptions);
    }

    /// <summary>
    /// 将对象序列化为UTF8编码的字节数组。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a UTF-8 encoded byte array.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <returns>序列化后的UTF8字节数组 / The serialized UTF-8 byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="obj"/> 或 <paramref name="options"/> 为 null 时抛出 / Thrown when <paramref name="obj"/> or <paramref name="options"/> is null</exception>
    public static byte[] SerializeToUtf8Bytes(object obj, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        return JsonSerializer.SerializeToUtf8Bytes(obj, options);
    }

    /// <summary>
    /// 使用源生成元数据将对象序列化为UTF8编码的字节数组。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a UTF-8 encoded byte array using source-generated metadata.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>序列化后的UTF8字节数组 / The serialized UTF-8 byte array</returns>
    public static byte[] SerializeToUtf8Bytes<T>(T obj, JsonTypeInfo<T> jsonTypeInfo)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        ArgumentNullException.ThrowIfNull(jsonTypeInfo, nameof(jsonTypeInfo));
        return JsonSerializer.SerializeToUtf8Bytes(obj, jsonTypeInfo);
    }

    /// <summary>
    /// 将对象序列化为格式化的UTF8编码字节数组。
    /// 使用格式化序列化配置(FormatOptions)。
    /// </summary>
    /// <remarks>
    /// Serializes an object to a formatted UTF-8 encoded byte array.
    /// Uses the formatted serialization configuration (FormatOptions).
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <returns>序列化后的UTF8字节数组 / The serialized UTF-8 byte array</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="obj"/> 为 null 时抛出 / Thrown when <paramref name="obj"/> is null</exception>
    public static byte[] SerializeToUtf8BytesFormat(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        return JsonSerializer.SerializeToUtf8Bytes(obj, FormatOptions);
    }

    /// <summary>
    /// 反序列化JSON字符串为指定类型的对象。
    /// 使用默认序列化配置(DefaultOptions)，如果因特殊浮点值失败，会预处理后重试。
    /// </summary>
    /// <remarks>
    /// Deserializes a JSON string to an object of the specified type.
    /// Uses the default serialization configuration (DefaultOptions). If it fails due to special floating-point values,
    /// it preprocesses them and retries with DefaultOptions. If both attempts fail, the original exception is thrown.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <typeparam name="T">目标类型 / The target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="json"/> 为 null 时抛出 / Thrown when <paramref name="json"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="json"/> 为空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="json"/> is an empty string or contains only whitespace characters</exception>
    public static T Deserialize<T>(string json)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));
        ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));
        ArgumentException.ThrowIfNullOrWhiteSpace(json, nameof(json));
        try
        {
            return JsonSerializer.Deserialize<T>(json, DefaultOptions);
        }
        catch (JsonException) when (json.Contains("NaN") || json.Contains("Infinity"))
        {
            // 特殊浮点值（NaN, Infinity, -Infinity）需要预处理
            string processedJson = PreprocessSpecialFloatingPointValues(json);
            return JsonSerializer.Deserialize<T>(processedJson, DefaultOptions);
        }
    }

    /// <summary>
    /// 使用源生成元数据反序列化JSON字符串。
    /// </summary>
    /// <remarks>
    /// Deserializes a JSON string using source-generated metadata.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    public static T Deserialize<T>(string json, JsonTypeInfo<T> jsonTypeInfo)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));
        ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));
        ArgumentException.ThrowIfNullOrWhiteSpace(json, nameof(json));
        ArgumentNullException.ThrowIfNull(jsonTypeInfo, nameof(jsonTypeInfo));
        try
        {
            return JsonSerializer.Deserialize(json, jsonTypeInfo);
        }
        catch (JsonException) when (json.Contains("NaN") || json.Contains("Infinity"))
        {
            string processedJson = PreprocessSpecialFloatingPointValues(json);
            return JsonSerializer.Deserialize(processedJson, jsonTypeInfo);
        }
    }

    /// <summary>
    /// 从UTF8 JSON流异步反序列化为指定类型的对象。
    /// </summary>
    /// <remarks>
    /// Asynchronously deserializes a UTF-8 JSON stream to an object of the specified type.
    /// </remarks>
    /// <param name="stream">源流 / Source stream</param>
    /// <param name="options">序列化配置 / Serialization options</param>
    /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    public static ValueTask<T> DeserializeAsync<T>(Stream stream, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        return JsonSerializer.DeserializeAsync<T>(stream, options ?? DefaultOptions, cancellationToken);
    }

    /// <summary>
    /// 使用源生成元数据从UTF8 JSON流异步反序列化为指定类型的对象。
    /// </summary>
    /// <remarks>
    /// Asynchronously deserializes a UTF-8 JSON stream using source-generated metadata.
    /// </remarks>
    /// <param name="stream">源流 / Source stream</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <param name="cancellationToken">取消令牌 / Cancellation token</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    public static ValueTask<T> DeserializeAsync<T>(Stream stream, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream, nameof(stream));
        ArgumentNullException.ThrowIfNull(jsonTypeInfo, nameof(jsonTypeInfo));
        return JsonSerializer.DeserializeAsync(stream, jsonTypeInfo, cancellationToken);
    }

    /// <summary>
    /// 预处理JSON字符串中的特殊浮点值。
    /// 将NaN、Infinity、-Infinity转换为带引号的形式。
    /// </summary>
    /// <remarks>
    /// Preprocesses special floating-point values in the JSON string.
    /// Converts NaN, Infinity, -Infinity to quoted form.
    /// </remarks>
    /// <param name="json">原始JSON字符串 / The original JSON string</param>
    /// <returns>处理后的JSON字符串 / The processed JSON string</returns>
    private static string PreprocessSpecialFloatingPointValues(string json)
    {
        // 替换非字符串形式的特殊浮点值为带引号的形式
        // 注意：这种简单的替换可能在某些复杂情况下不完全准确，但对大多数情况应该有效
        json = System.Text.RegularExpressions.Regex.Replace(
            json,
            @":\s*(NaN|Infinity|-Infinity)\s*([,}])",
            ": \"$1\"$2",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return json;
    }

    /// <summary>
    /// 将JSON字符串反序列化为指定类型的对象。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Deserializes a JSON string to an object of the specified type.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <typeparam name="T">目标类型 / The target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="json"/> 或 <paramref name="options"/> 为 null 时抛出 / Thrown when <paramref name="json"/> or <paramref name="options"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="json"/> 为空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="json"/> is an empty string or contains only whitespace characters</exception>
    public static T Deserialize<T>(string json, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));
        ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));
        ArgumentException.ThrowIfNullOrWhiteSpace(json, nameof(json));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        return JsonSerializer.Deserialize<T>(json, options);
    }

    /// <summary>
    /// 将JSON字符串反序列化为指定Type类型的对象。
    /// 使用默认序列化配置(DefaultOptions)，如果因特殊浮点值失败，会预处理后重试。
    /// </summary>
    /// <remarks>
    /// Deserializes a JSON string to an object of the specified Type.
    /// Uses the default serialization configuration (DefaultOptions). If it fails due to special floating-point values,
    /// it preprocesses them and retries with DefaultOptions. If both attempts fail, the original exception is thrown.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="type">目标类型的Type对象 / The Type object of the target type</param>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="json"/> 或 <paramref name="type"/> 为 null 时抛出 / Thrown when <paramref name="json"/> or <paramref name="type"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="json"/> 为空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="json"/> is an empty string or contains only whitespace characters</exception>
    public static object Deserialize(string json, Type type)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));
        ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));
        ArgumentException.ThrowIfNullOrWhiteSpace(json, nameof(json));
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        try
        {
            return JsonSerializer.Deserialize(json, type, DefaultOptions);
        }
        catch (JsonException) when (json.Contains("NaN") || json.Contains("Infinity"))
        {
            // 特殊浮点值（NaN, Infinity, -Infinity）需要预处理
            string processedJson = PreprocessSpecialFloatingPointValues(json);
            return JsonSerializer.Deserialize(processedJson, type, DefaultOptions);
        }
    }

    /// <summary>
    /// 将JSON字符串反序列化为指定Type类型的对象。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Deserializes a JSON string to an object of the specified Type.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="type">目标类型的Type对象 / The Type object of the target type</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="json"/>、<paramref name="type"/> 或 <paramref name="options"/> 为 null 时抛出 / Thrown when <paramref name="json"/>, <paramref name="type"/>, or <paramref name="options"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="json"/> 为空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="json"/> is an empty string or contains only whitespace characters</exception>
    public static object Deserialize(string json, Type type, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));
        ArgumentException.ThrowIfNullOrEmpty(json, nameof(json));
        ArgumentException.ThrowIfNullOrWhiteSpace(json, nameof(json));
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        return JsonSerializer.Deserialize(json, type, options);
    }

    /// <summary>
    /// 从UTF8编码的字节数组反序列化为指定类型的对象。
    /// 使用默认序列化配置(DefaultOptions)，如果使用默认配置失败，会尝试使用格式化配置(FormatOptions)。
    /// </summary>
    /// <remarks>
    /// Deserializes from a UTF-8 encoded byte array to an object of the specified type.
    /// Uses the default serialization configuration (DefaultOptions), if the default configuration fails, it will try to use the formatted configuration (FormatOptions).
    /// </remarks>
    /// <param name="utf8Bytes">UTF8编码的JSON字节数组 / The UTF-8 encoded JSON byte array</param>
    /// <typeparam name="T">目标类型 / The target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="utf8Bytes"/> 为 null 时抛出 / Thrown when <paramref name="utf8Bytes"/> is null</exception>
    public static T DeserializeFromUtf8Bytes<T>(byte[] utf8Bytes)
    {
        ArgumentNullException.ThrowIfNull(utf8Bytes, nameof(utf8Bytes));
        try
        {
            return JsonSerializer.Deserialize<T>(utf8Bytes, DefaultOptions);
        }
        catch
        {
            try
            {
                // 如果使用默认配置失败，尝试使用格式化配置
                return JsonSerializer.Deserialize<T>(utf8Bytes, FormatOptions);
            }
            catch
            {
                // 如果两者都失败，尝试预处理特殊浮点值
                string json = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                string processedJson = PreprocessSpecialFloatingPointValues(json);
                byte[] processedBytes = System.Text.Encoding.UTF8.GetBytes(processedJson);

                try
                {
                    return JsonSerializer.Deserialize<T>(processedBytes, DefaultOptions);
                }
                catch
                {
                    // 最后尝试使用格式化配置
                    return JsonSerializer.Deserialize<T>(processedBytes, FormatOptions);
                }
            }
        }
    }

    /// <summary>
    /// 从UTF8编码的字节数组反序列化为指定类型的对象。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Deserializes from a UTF-8 encoded byte array to an object of the specified type.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="utf8Bytes">UTF8编码的JSON字节数组 / The UTF-8 encoded JSON byte array</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <typeparam name="T">目标类型 / The target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="utf8Bytes"/> 或 <paramref name="options"/> 为 null 时抛出 / Thrown when <paramref name="utf8Bytes"/> or <paramref name="options"/> is null</exception>
    public static T DeserializeFromUtf8Bytes<T>(byte[] utf8Bytes, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(utf8Bytes, nameof(utf8Bytes));
        ArgumentNullException.ThrowIfNull(options, nameof(options));
        return JsonSerializer.Deserialize<T>(utf8Bytes, options);
    }

    /// <summary>
    /// 使用源生成元数据从UTF8编码的字节数组反序列化为指定类型的对象。
    /// </summary>
    /// <remarks>
    /// Deserializes from a UTF-8 encoded byte array using source-generated metadata.
    /// </remarks>
    /// <param name="utf8Bytes">UTF8编码的JSON字节数组 / The UTF-8 encoded JSON byte array</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化后的对象实例 / The deserialized object instance</returns>
    public static T DeserializeFromUtf8Bytes<T>(byte[] utf8Bytes, JsonTypeInfo<T> jsonTypeInfo)
    {
        ArgumentNullException.ThrowIfNull(utf8Bytes, nameof(utf8Bytes));
        ArgumentNullException.ThrowIfNull(jsonTypeInfo, nameof(jsonTypeInfo));
        return JsonSerializer.Deserialize(utf8Bytes, jsonTypeInfo);
    }

    /// <summary>
    /// 尝试将JSON字符串反序列化为指定类型的对象。
    /// 如果反序列化失败，返回false并将result设置为null。
    /// 使用默认序列化配置(DefaultOptions)。
    /// </summary>
    /// <remarks>
    /// Attempts to deserialize a JSON string to an object of the specified type.
    /// If deserialization fails, returns false and sets result to null.
    /// Uses the default serialization configuration (DefaultOptions).
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="result">反序列化结果，如果失败则为null / The deserialization result, null if failed</param>
    /// <typeparam name="T">目标类型 / The target type</typeparam>
    /// <returns>反序列化是否成功 / Whether the deserialization was successful</returns>
    public static bool TryDeserialize<T>(string json, out T result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        try
        {
            result = Deserialize<T>(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 尝试将JSON字符串反序列化为指定类型的对象。
    /// 如果反序列化失败，返回false并将result设置为null。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Attempts to deserialize a JSON string to an object of the specified type.
    /// If deserialization fails, returns false and sets result to null.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="result">反序列化结果，如果失败则为null / The deserialization result, null if failed</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <typeparam name="T">目标类型 / The target type</typeparam>
    /// <returns>反序列化是否成功 / Whether the deserialization was successful</returns>
    public static bool TryDeserialize<T>(string json, out T result, JsonSerializerOptions options)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        try
        {
            result = Deserialize<T>(json, options);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 尝试反序列化JSON字符串，失败时返回错误详情。
    /// </summary>
    /// <remarks>
    /// Attempts to deserialize a JSON string and returns error details on failure.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="result">反序列化结果 / Deserialization result</param>
    /// <param name="error">错误详情 / Error details</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化是否成功 / Whether deserialization succeeded</returns>
    public static bool TryDeserialize<T>(string json, out T result, out JsonHelperError error)
    {
        return TryDeserialize(json, out result, DefaultOptions, out error);
    }

    /// <summary>
    /// 使用自定义配置尝试反序列化JSON字符串，失败时返回错误详情。
    /// </summary>
    /// <remarks>
    /// Attempts to deserialize a JSON string with custom options and returns error details on failure.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="result">反序列化结果 / Deserialization result</param>
    /// <param name="options">自定义序列化配置 / Custom serialization options</param>
    /// <param name="error">错误详情 / Error details</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化是否成功 / Whether deserialization succeeded</returns>
    public static bool TryDeserialize<T>(string json, out T result, JsonSerializerOptions options, out JsonHelperError error)
    {
        result = default;
        error = null;

        if (string.IsNullOrWhiteSpace(json))
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentException), "JSON input cannot be null, empty, or whitespace.");
            return false;
        }

        if (options == null)
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentNullException), "Serialization options cannot be null.");
            return false;
        }

        try
        {
            result = Deserialize<T>(json, options);
            return true;
        }
        catch (Exception exception)
        {
            error = CreateError(exception, JsonHelperErrorKind.InvalidInput);
            return false;
        }
    }

    /// <summary>
    /// 使用源生成元数据尝试反序列化JSON字符串，失败时返回错误详情。
    /// </summary>
    /// <remarks>
    /// Attempts to deserialize a JSON string using source-generated metadata and returns error details on failure.
    /// </remarks>
    /// <param name="json">需要反序列化的JSON字符串 / The JSON string to deserialize</param>
    /// <param name="result">反序列化结果 / Deserialization result</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <param name="error">错误详情 / Error details</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>反序列化是否成功 / Whether deserialization succeeded</returns>
    public static bool TryDeserialize<T>(string json, out T result, JsonTypeInfo<T> jsonTypeInfo, out JsonHelperError error)
    {
        result = default;
        error = null;

        if (string.IsNullOrWhiteSpace(json))
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentException), "JSON input cannot be null, empty, or whitespace.");
            return false;
        }

        if (jsonTypeInfo == null)
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentNullException), "JSON type info cannot be null.");
            return false;
        }

        try
        {
            result = Deserialize(json, jsonTypeInfo);
            return true;
        }
        catch (Exception exception)
        {
            error = CreateError(exception, JsonHelperErrorKind.InvalidInput);
            return false;
        }
    }

    /// <summary>
    /// 尝试将对象序列化为JSON字符串。
    /// 如果序列化失败，返回false并将result设置为null。
    /// 使用默认序列化配置(DefaultOptions)。
    /// </summary>
    /// <remarks>
    /// Attempts to serialize an object to a JSON string.
    /// If serialization fails, returns false and sets result to null.
    /// Uses the default serialization configuration (DefaultOptions).
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="result">序列化结果，如果失败则为null / The serialization result, null if failed</param>
    /// <returns>序列化是否成功 / Whether the serialization was successful</returns>
    public static bool TrySerialize(object obj, out string result)
    {
        result = null;
        if (obj == null)
        {
            return false;
        }

        try
        {
            result = Serialize(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 尝试将对象序列化为JSON字符串。
    /// 如果序列化失败，返回false并将result设置为null。
    /// 使用自定义序列化配置。
    /// </summary>
    /// <remarks>
    /// Attempts to serialize an object to a JSON string.
    /// If serialization fails, returns false and sets result to null.
    /// Uses custom serialization configuration.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="result">序列化结果，如果失败则为null / The serialization result, null if failed</param>
    /// <param name="options">自定义序列化配置 / Custom serialization configuration</param>
    /// <returns>序列化是否成功 / Whether the serialization was successful</returns>
    public static bool TrySerialize(object obj, out string result, JsonSerializerOptions options)
    {
        result = null;
        if (obj == null)
        {
            return false;
        }

        try
        {
            result = Serialize(obj, options);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 尝试将对象序列化为JSON字符串，失败时返回错误详情。
    /// </summary>
    /// <remarks>
    /// Attempts to serialize an object to a JSON string and returns error details on failure.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="result">序列化结果 / Serialization result</param>
    /// <param name="error">错误详情 / Error details</param>
    /// <returns>序列化是否成功 / Whether serialization succeeded</returns>
    public static bool TrySerialize(object obj, out string result, out JsonHelperError error)
    {
        return TrySerialize(obj, out result, DefaultOptions, out error);
    }

    /// <summary>
    /// 使用自定义配置尝试将对象序列化为JSON字符串，失败时返回错误详情。
    /// </summary>
    /// <remarks>
    /// Attempts to serialize an object to a JSON string with custom options and returns error details on failure.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="result">序列化结果 / Serialization result</param>
    /// <param name="options">自定义序列化配置 / Custom serialization options</param>
    /// <param name="error">错误详情 / Error details</param>
    /// <returns>序列化是否成功 / Whether serialization succeeded</returns>
    public static bool TrySerialize(object obj, out string result, JsonSerializerOptions options, out JsonHelperError error)
    {
        result = null;
        error = null;

        if (obj == null)
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentNullException), "Object to serialize cannot be null.");
            return false;
        }

        if (options == null)
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentNullException), "Serialization options cannot be null.");
            return false;
        }

        try
        {
            result = Serialize(obj, options);
            return true;
        }
        catch (Exception exception)
        {
            error = CreateError(exception, JsonHelperErrorKind.Serialization);
            return false;
        }
    }

    /// <summary>
    /// 使用源生成元数据尝试将对象序列化为JSON字符串，失败时返回错误详情。
    /// </summary>
    /// <remarks>
    /// Attempts to serialize an object to a JSON string using source-generated metadata and returns error details on failure.
    /// </remarks>
    /// <param name="obj">需要序列化的对象 / The object to serialize</param>
    /// <param name="result">序列化结果 / Serialization result</param>
    /// <param name="jsonTypeInfo">源生成类型元数据 / Source-generated type metadata</param>
    /// <param name="error">错误详情 / Error details</param>
    /// <typeparam name="T">目标类型 / Target type</typeparam>
    /// <returns>序列化是否成功 / Whether serialization succeeded</returns>
    public static bool TrySerialize<T>(T obj, out string result, JsonTypeInfo<T> jsonTypeInfo, out JsonHelperError error)
    {
        result = null;
        error = null;

        if (obj == null)
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentNullException), "Object to serialize cannot be null.");
            return false;
        }

        if (jsonTypeInfo == null)
        {
            error = new JsonHelperError(JsonHelperErrorKind.InvalidInput, typeof(ArgumentNullException), "JSON type info cannot be null.");
            return false;
        }

        try
        {
            result = Serialize(obj, jsonTypeInfo);
            return true;
        }
        catch (Exception exception)
        {
            error = CreateError(exception, JsonHelperErrorKind.Serialization);
            return false;
        }
    }

    private static JsonHelperError CreateError(Exception exception, JsonHelperErrorKind fallbackKind)
    {
        if (exception is JsonException jsonException)
        {
            var kind = IsTypeMismatch(jsonException) ? JsonHelperErrorKind.TypeMismatch : fallbackKind;
            return new JsonHelperError(kind, exception.GetType(), jsonException.Message, jsonException.Path, jsonException.LineNumber, jsonException.BytePositionInLine);
        }

        return new JsonHelperError(fallbackKind == JsonHelperErrorKind.None ? JsonHelperErrorKind.Unknown : fallbackKind, exception.GetType(), exception.Message);
    }

    private static bool IsTypeMismatch(JsonException exception)
    {
        if (string.IsNullOrWhiteSpace(exception.Path))
        {
            return false;
        }

        return exception.Message.Contains("convert", StringComparison.OrdinalIgnoreCase);
    }
}
