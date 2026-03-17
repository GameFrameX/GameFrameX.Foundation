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

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 布尔值解析工具类
/// Boolean value parsing utility class
/// </summary>
internal static class BooleanParser
{
    /// <summary>
    /// 检查字符串值是否为布尔类型
    /// Checks if the string value is a boolean type
    /// </summary>
    /// <param name="value">要检查的字符串值 / The string value to check</param>
    /// <returns>如果是布尔类型值则返回true，否则返回false / Returns true if the value is a boolean type, otherwise false</returns>
    public static bool IsBooleanValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalizedValue = value.Trim().ToLowerInvariant();
        return normalizedValue is "true" or "false" or "1" or "0" or "yes" or "no" or "on" or "off";
    }

    /// <summary>
    /// 解析布尔类型字符串值
    /// Parses a boolean string value
    /// </summary>
    /// <param name="value">要解析的字符串值 / The string value to parse</param>
    /// <returns>
    /// 解析后的布尔值。如果值为 null、空白或无法识别，则返回 false。
    /// The parsed boolean value. Returns false if the value is null, whitespace, or unrecognized.
    /// </returns>
    public static bool ParseBooleanValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalizedValue = value.Trim().ToLowerInvariant();
        return normalizedValue is "true" or "1" or "yes" or "on";
    }
}
