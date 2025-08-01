// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameFrameX.Foundation.Json;

/// <summary>
/// 处理特殊浮点值的JSON文档转换器
/// 用于处理直接在JSON中出现的特殊浮点值（如NaN、Infinity）
/// </summary>
public class SpecialFloatingPointDocumentConverter : JsonConverter<JsonDocument>
{
    /// <summary>
    /// 读取并处理包含特殊浮点值的JSON，返回JsonDocument对象
    /// </summary>
    public override JsonDocument Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument document = JsonDocument.Parse(GetModifiedJson(reader)))
        {
            // 创建一个新的JsonDocument副本
            return JsonDocument.Parse(document.RootElement.GetRawText());
        }
    }

    /// <summary>
    /// 将JsonDocument对象写入JSON，保留特殊浮点值（如NaN、Infinity）
    /// </summary>
    public override void Write(Utf8JsonWriter writer, JsonDocument value, JsonSerializerOptions options)
    {
        // 直接写入原始JSON
        value.WriteTo(writer);
    }

    private string GetModifiedJson(Utf8JsonReader reader)
    {
        // 读取整个JSON内容
        using (var ms = new System.IO.MemoryStream())
        {
            using (var jsonWriter = new Utf8JsonWriter(ms))
            {
                while (reader.Read())
                {
                    // 处理特殊浮点值
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        string propertyName = reader.GetString();
                        jsonWriter.WritePropertyName(propertyName);

                        // 读取下一个token
                        reader.Read();

                        // 检查是否是特殊浮点值
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            string value = reader.GetString();
                            if (string.Equals(value, "NaN", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(value, "Infinity", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(value, "-Infinity", StringComparison.OrdinalIgnoreCase))
                            {
                                jsonWriter.WriteStringValue(value);
                                continue;
                            }
                        }

                        // 复制当前token
                        JsonTokenType tokenType = reader.TokenType;
                        switch (tokenType)
                        {
                            case JsonTokenType.String:
                                jsonWriter.WriteStringValue(reader.GetString());
                                break;
                            case JsonTokenType.Number:
                                jsonWriter.WriteNumberValue(reader.GetDouble());
                                break;
                            case JsonTokenType.True:
                                jsonWriter.WriteBooleanValue(true);
                                break;
                            case JsonTokenType.False:
                                jsonWriter.WriteBooleanValue(false);
                                break;
                            case JsonTokenType.Null:
                                jsonWriter.WriteNullValue();
                                break;
                            default:
                                // 处理其他类型
                                break;
                        }
                    }
                    else
                    {
                        // 复制其他token
                        JsonTokenType tokenType = reader.TokenType;
                        switch (tokenType)
                        {
                            case JsonTokenType.StartObject:
                                jsonWriter.WriteStartObject();
                                break;
                            case JsonTokenType.EndObject:
                                jsonWriter.WriteEndObject();
                                break;
                            case JsonTokenType.StartArray:
                                jsonWriter.WriteStartArray();
                                break;
                            case JsonTokenType.EndArray:
                                jsonWriter.WriteEndArray();
                                break;
                            default:
                                // 处理其他类型
                                break;
                        }
                    }
                }
            }

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}