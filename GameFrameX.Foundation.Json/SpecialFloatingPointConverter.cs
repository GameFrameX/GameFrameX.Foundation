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

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameFrameX.Foundation.Json;

/// <summary>
/// 处理特殊浮点值（NaN、Infinity、-Infinity）的 JSON 转换器。
/// </summary>
/// <remarks>
/// JSON converter for handling special floating-point values (NaN, Infinity, -Infinity).
/// </remarks>
public class SpecialFloatingPointConverter : JsonConverter<double>
{
    /// <summary>
    /// 读取 JSON 中的 double 类型数据，支持特殊浮点值（NaN、Infinity、-Infinity）。
    /// </summary>
    /// <remarks>
    /// Reads double type data from JSON, supporting special floating-point values (NaN, Infinity, -Infinity).
    /// </remarks>
    /// <param name="reader">JSON 读取器 / The JSON reader</param>
    /// <param name="typeToConvert">要转换的类型 / The type to convert</param>
    /// <param name="options">序列化选项 / Serialization options</param>
    /// <returns>读取的 double 值 / The read double value</returns>
    public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string stringValue = reader.GetString();
            if (string.Equals(stringValue, "NaN", StringComparison.OrdinalIgnoreCase))
            {
                return double.NaN;
            }

            if (string.Equals(stringValue, "Infinity", StringComparison.OrdinalIgnoreCase))
            {
                return double.PositiveInfinity;
            }

            if (string.Equals(stringValue, "-Infinity", StringComparison.OrdinalIgnoreCase))
            {
                return double.NegativeInfinity;
            }

            // 尝试从字符串解析数值
            if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
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
                    return double.NaN;
                }

                if (string.Equals(propertyName, "Infinity", StringComparison.OrdinalIgnoreCase))
                {
                    return double.PositiveInfinity;
                }

                if (string.Equals(propertyName, "-Infinity", StringComparison.OrdinalIgnoreCase))
                {
                    return double.NegativeInfinity;
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
    /// 将 double 类型的特殊值（NaN、Infinity、-Infinity）以字符串形式写入 JSON，其他数值正常写入。
    /// </summary>
    /// <remarks>
    /// Writes special values of double type (NaN, Infinity, -Infinity) as strings to JSON, while writing other values normally.
    /// </remarks>
    /// <param name="writer">JSON 写入器 / The JSON writer</param>
    /// <param name="value">要写入的 double 值 / The double value to write</param>
    /// <param name="options">序列化选项 / Serialization options</param>
    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        if (double.IsNaN(value))
        {
            writer.WriteStringValue("NaN");
        }
        else if (double.IsPositiveInfinity(value))
        {
            writer.WriteStringValue("Infinity");
        }
        else if (double.IsNegativeInfinity(value))
        {
            writer.WriteStringValue("-Infinity");
        }
        else
        {
            writer.WriteNumberValue(value);
        }
    }
}