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

using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameFrameX.Foundation.Json;

/// <summary>
/// 处理特殊浮点值的 JSON 文档转换器。
/// 用于处理直接在 JSON 中出现的特殊浮点值（如 NaN、Infinity）。
/// </summary>
/// <remarks>
/// JSON document converter for handling special floating-point values.
/// Used to handle special floating-point values (such as NaN, Infinity) that appear directly in JSON.
/// </remarks>
public class SpecialFloatingPointDocumentConverter : JsonConverter<JsonDocument>
{
    /// <summary>
    /// 读取并处理包含特殊浮点值的 JSON，返回 JsonDocument 对象。
    /// </summary>
    /// <remarks>
    /// Reads and processes JSON containing special floating-point values, returning a JsonDocument object.
    /// </remarks>
    /// <param name="reader">JSON 读取器 / The JSON reader</param>
    /// <param name="typeToConvert">要转换的类型 / The type to convert</param>
    /// <param name="options">序列化选项 / Serialization options</param>
    /// <returns>读取的 JsonDocument 对象 / The read JsonDocument object</returns>
    public override JsonDocument Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument document = JsonDocument.Parse(GetModifiedJson(reader)))
        {
            // 创建一个新的JsonDocument副本
            return JsonDocument.Parse(document.RootElement.GetRawText());
        }
    }

    /// <summary>
    /// 将 JsonDocument 对象写入 JSON，保留特殊浮点值（如 NaN、Infinity）。
    /// </summary>
    /// <remarks>
    /// Writes a JsonDocument object to JSON, preserving special floating-point values (such as NaN, Infinity).
    /// </remarks>
    /// <param name="writer">JSON 写入器 / The JSON writer</param>
    /// <param name="value">要写入的 JsonDocument 对象 / The JsonDocument object to write</param>
    /// <param name="options">序列化选项 / Serialization options</param>
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