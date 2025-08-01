// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameFrameX.Foundation.Json;

/// <summary>
/// 处理特殊浮点值（NaN、Infinity、-Infinity）的 JSON 转换器 (float 版本)
/// </summary>
public class SpecialFloatingPointConverterFloat : JsonConverter<float>
{
    /// <summary>
    /// 反序列化 JSON 时，将字符串或数值类型的特殊浮点值（如 "NaN"、"Infinity"、"-Infinity"）转换为对应的 float 类型值。
    /// </summary>
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (string.Equals(stringValue, "NaN", StringComparison.OrdinalIgnoreCase))
            {
                return float.NaN;
            }

            if (string.Equals(stringValue, "Infinity", StringComparison.OrdinalIgnoreCase))
            {
                return float.PositiveInfinity;
            }

            if (string.Equals(stringValue, "-Infinity", StringComparison.OrdinalIgnoreCase))
            {
                return float.NegativeInfinity;
            }

            // 尝试从字符串解析数值
            if (float.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetSingle();
        }
        else if (reader.TokenType == JsonTokenType.Null)
        {
            return 0;
        }
        else
        {
            // 尝试处理非标准格式，如直接的NaN、Infinity等
            try
            {
                string propertyName = reader.GetString();
                if (string.Equals(propertyName, "NaN", StringComparison.OrdinalIgnoreCase))
                {
                    return float.NaN;
                }

                if (string.Equals(propertyName, "Infinity", StringComparison.OrdinalIgnoreCase))
                {
                    return float.PositiveInfinity;
                }

                if (string.Equals(propertyName, "-Infinity", StringComparison.OrdinalIgnoreCase))
                {
                    return float.NegativeInfinity;
                }
            }
            catch
            {
                // 忽略异常，继续尝试其他方式
            }
        }

        // 如果无法解析，返回默认值
        return 0;
    }

    /// <summary>
    /// 序列化 float 类型为 JSON 时，将特殊浮点值（NaN、Infinity、-Infinity）以字符串形式写入，其他数值正常写入。
    /// </summary>
    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        if (float.IsNaN(value))
        {
            writer.WriteStringValue("NaN");
        }
        else if (float.IsPositiveInfinity(value))
        {
            writer.WriteStringValue("Infinity");
        }
        else if (float.IsNegativeInfinity(value))
        {
            writer.WriteStringValue("-Infinity");
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }
}