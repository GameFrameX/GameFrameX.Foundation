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

using System.Text;
using System.Text.RegularExpressions;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字符串类型的扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for string types.
/// </remarks>
public static partial class StringExtensions
{
    /// <summary>
    /// 将 Base64 字符串转换为 URL 安全格式。
    /// </summary>
    /// <remarks>
    /// Converts a Base64 string to URL-safe format.
    /// This method replaces the + and / characters in the Base64 string with - and _ characters respectively, and removes the padding character =, making it suitable for URL transmission.
    /// Complies with RFC 4648 Base64URL encoding specification.
    /// </remarks>
    /// <param name="value">要处理的 Base64 字符串，不能为 <c>null</c> / The Base64 string to process, cannot be <c>null</c>.</param>
    /// <returns>URL 安全的 Base64 字符串 / A URL-safe Base64 string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public static string ToUrlSafeBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return value.Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }

    /// <summary>
    /// 将 Base64 字符串转换为 URL 安全格式。
    /// </summary>
    /// <remarks>
    /// Converts a Base64 string to URL-safe format.
    /// This method replaces the + and / characters in the Base64 string with - and _ characters respectively.
    /// When removePadding is true, removes the padding character =; when false, keeps the padding character =.
    /// Complies with RFC 4648 Base64URL encoding specification.
    /// </remarks>
    /// <param name="value">要处理的 Base64 字符串，不能为 <c>null</c> / The Base64 string to process, cannot be <c>null</c>.</param>
    /// <param name="removePadding">是否移除填充字符 = / Whether to remove the padding character =.</param>
    /// <returns>URL 安全的 Base64 字符串 / A URL-safe Base64 string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public static string ToUrlSafeBase64(this string value, bool removePadding)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        var result = value.Replace("+", "-").Replace("/", "_");
        return removePadding ? result.TrimEnd('=') : result;
    }

    /// <summary>
    /// 将 URL 安全的 Base64 字符串转换回标准 Base64 格式。
    /// </summary>
    /// <remarks>
    /// Converts a URL-safe Base64 string back to standard Base64 format.
    /// This method is the reverse operation of ToUrlSafeBase64, replacing - and _ characters back to + and / characters respectively, and adding padding character = if needed.
    /// Used to restore URL-safe Base64 strings to standard Base64 format.
    /// </remarks>
    /// <param name="value">要处理的 URL 安全 Base64 字符串，不能为 <c>null</c> / The URL-safe Base64 string to process, cannot be <c>null</c>.</param>
    /// <returns>标准格式的 Base64 字符串 / A standard format Base64 string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public static string FromUrlSafeBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        var result = value.Replace("-", "+").Replace("_", "/");

        // 添加必要的填充字符
        var padding = 4 - (result.Length % 4);
        if (padding < 4)
        {
            result = result.PadRight(result.Length + padding, '=');
        }

        return result;
    }

    /// <summary>
    /// 将 URL 安全的 Base64 字符串转换回标准 Base64 格式。
    /// </summary>
    /// <remarks>
    /// Converts a URL-safe Base64 string back to standard Base64 format.
    /// This method is the reverse operation of ToUrlSafeBase64, replacing - and _ characters back to + and / characters respectively.
    /// When addPadding is true, automatically adds the necessary padding character =; when false, does not add padding.
    /// Used to restore URL-safe Base64 strings to standard Base64 format.
    /// </remarks>
    /// <param name="value">要处理的 URL 安全 Base64 字符串，不能为 <c>null</c> / The URL-safe Base64 string to process, cannot be <c>null</c>.</param>
    /// <param name="addPadding">是否自动添加填充字符 = / Whether to automatically add the padding character =.</param>
    /// <returns>标准格式的 Base64 字符串 / A standard format Base64 string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="value"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="value"/> is <c>null</c>.</exception>
    public static string FromUrlSafeBase64(this string value, bool addPadding)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        var result = value.Replace("-", "+").Replace("_", "/");

        if (addPadding)
        {
            // 添加必要的填充字符
            var padding = 4 - (result.Length % 4);
            if (padding < 4)
            {
                result = result.PadRight(result.Length + padding, '=');
            }
        }

        return result;
    }


    /// <summary>
    /// 匹配中文字符的正则表达式。
    /// </summary>
    /// <remarks>
    /// Regex for matching Chinese characters.
    /// Matching range includes basic Chinese characters (0x4e00-0x9fa5), excluding rare characters, variant characters, and other extended Chinese characters.
    /// </remarks>
    private static readonly Regex CnReg = new(@"[\u4e00-\u9fa5]");

    /// <summary>
    /// 计算字符串的显示宽度，汉字等宽字符算作 2 个单位宽度，其他字符算作 1 个单位宽度。
    /// </summary>
    /// <remarks>
    /// Calculates the display width of a string, where wide characters like Chinese are counted as 2 unit widths, and other characters as 1 unit width.
    /// This method is mainly used for text alignment in console or monospace font environments.
    /// Supported Chinese character ranges include:
    /// - Basic CJK Unified Ideographs (\u4e00-\u9fff)
    /// - CJK Unified Ideographs Extension A (\u3400-\u4dbf)
    /// - CJK Unified Ideographs Extension B (\u20000-\u2a6df), but in C# since char is 16-bit, high surrogate pairs are simplified
    /// Other characters (English, numbers, punctuation, etc.) are counted as 1 unit width.
    /// </remarks>
    /// <param name="text">要计算宽度的字符串 / The string to calculate width for.</param>
    /// <returns>字符串的显示宽度。汉字等宽字符计为 2，其他字符计为 1 / The display width of the string. Chinese characters count as 2, other characters as 1.</returns>
    public static int GetDisplayWidth(this string text)
    {
        int total = 0;
        foreach (var c in text)
        {
            var isChineseChar = (c >= '\u4e00' && c <= '\u9fff') || (c >= '\u3400' && c <= '\u4dbf');
            total += isChineseChar ? 2 : 1;
        }

        return total;
    }

    /// <summary>
    /// 将字符串转换为下划线命名法。
    /// </summary>
    /// <remarks>
    /// Converts a string to snake_case (underscore naming convention).
    /// This method adds an underscore before each uppercase letter in the string and converts to uppercase or lowercase based on the isToUpper parameter.
    /// For example: "HelloWorld" converts to "hello_world", "IsValid" converts to "is_valid".
    /// When the string already contains underscores, returns the original string directly.
    /// </remarks>
    /// <param name="str">要转换的字符串，不能为 <c>null</c> / The string to convert, cannot be <c>null</c>.</param>
    /// <param name="isToUpper">是否将下划线转换为大写，默认值为 <c>false</c> / Whether to convert to uppercase, defaults to <c>false</c>.</param>
    /// <returns>下划线命名法的字符串 / A snake_case string.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="str"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="str"/> is <c>null</c>.</exception>
    public static string ConvertToUnderLine(this string str, bool isToUpper = false)
    {
        ArgumentNullException.ThrowIfNull(str, nameof(str));
        if (str.Contains('_'))
        {
            return str;
        }

        if (isToUpper)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{x}" : x.ToString())).ToUpper();
        }

        return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? $"_{x}" : x.ToString())).ToLower();
    }

    /// <summary>
    /// 重复指定字符指定次数。
    /// </summary>
    /// <remarks>
    /// Repeats a specified character a specified number of times.
    /// Uses the string constructor for better performance.
    /// When count is 0, returns an empty string.
    /// </remarks>
    /// <param name="c">要重复的字符 / The character to repeat.</param>
    /// <param name="count">重复次数，必须为非负数 / The number of times to repeat, must be non-negative.</param>
    /// <returns>由指定字符重复指定次数组成的新字符串 / A new string composed of the specified character repeated the specified number of times.</returns>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="count"/> 为负数时抛出 / Thrown when <paramref name="count"/> is negative.</exception>
    public static string RepeatChar(this char c, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
        return new string(c, count);
    }

    /// <summary>
    /// 获取在指定宽度内左对齐的文本。
    /// </summary>
    /// <remarks>
    /// Gets left-aligned text within the specified width.
    /// If the specified width is less than the text length, the text length will be used as the width.
    /// Text is left-aligned with spaces padded on the right to the specified width.
    /// </remarks>
    /// <param name="text">要左对齐的文本，不能为 <c>null</c> / The text to left-align, cannot be <c>null</c>.</param>
    /// <param name="width">总宽度，必须为非负数 / The total width, must be non-negative.</param>
    /// <returns>右侧填充空格的左对齐文本 / Left-aligned text with spaces padded on the right.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="text"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="text"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="width"/> 为负数时抛出 / Thrown when <paramref name="width"/> is negative.</exception>
    public static string LeftAlignedText(this string text, int width)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        ArgumentOutOfRangeException.ThrowIfNegative(width, nameof(width));

        if (width < text.Length)
        {
            width = text.Length;
        }

        var rightSpaces = width - text.Length;
        var paddedText = text + new string(' ', rightSpaces);
        return paddedText;
    }

    /// <summary>
    /// 获取在指定宽度内右对齐的文本。
    /// </summary>
    /// <remarks>
    /// Gets right-aligned text within the specified width.
    /// If the specified width is less than the text length, the text length will be used as the width.
    /// Text is right-aligned with spaces padded on the left to the specified width.
    /// </remarks>
    /// <param name="text">要右对齐的文本，不能为 <c>null</c> / The text to right-align, cannot be <c>null</c>.</param>
    /// <param name="width">总宽度，必须为非负数 / The total width, must be non-negative.</param>
    /// <returns>左侧填充空格的右对齐文本 / Right-aligned text with spaces padded on the left.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="text"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="text"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="width"/> 为负数时抛出 / Thrown when <paramref name="width"/> is negative.</exception>
    public static string RightAlignedText(this string text, int width)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        ArgumentOutOfRangeException.ThrowIfNegative(width, nameof(width));

        if (width < text.Length)
        {
            width = text.Length;
        }

        var leftSpaces = width - text.Length;
        var paddedText = new string(' ', leftSpaces) + text;
        return paddedText;
    }

    /// <summary>
    /// 获取在指定宽度内居中对齐的文本。
    /// </summary>
    /// <remarks>
    /// Gets center-aligned text within the specified width.
    /// If the specified width is less than the text length, the text length will be used as the width.
    /// When the text length is odd and the total width is even, the number of spaces on the right will be one less than on the left.
    /// </remarks>
    /// <param name="text">要居中对齐的文本，不能为 <c>null</c> / The text to center-align, cannot be <c>null</c>.</param>
    /// <param name="width">总宽度，必须为非负数 / The total width, must be non-negative.</param>
    /// <returns>两侧填充空格的居中对齐文本 / Center-aligned text with spaces padded on both sides.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="text"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="text"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="width"/> 为负数时抛出 / Thrown when <paramref name="width"/> is negative.</exception>
    public static string CenterAlignedText(this string text, int width)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        ArgumentOutOfRangeException.ThrowIfNegative(width, nameof(width));

        if (width < text.Length)
        {
            width = text.Length;
        }

        var leftSpaces = (width - text.Length) / 2;
        var rightSpaces = width - text.Length - leftSpaces; // 确保左右空格总数等于width-text.Length
        var paddedText = new string(' ', leftSpaces) + text + new string(' ', rightSpaces);
        return paddedText;
    }

    /// <summary>
    /// 从字符串末尾移除指定字符（如果存在）。
    /// </summary>
    /// <remarks>
    /// Removes the specified character from the end of the string if it exists.
    /// This method only removes a single specified character from the end of the string.
    /// If you need to remove multiple characters, use the RemoveSuffix method with string parameter.
    /// </remarks>
    /// <param name="self">要处理的字符串 / The string to process.</param>
    /// <param name="toRemove">要移除的字符 / The character to remove.</param>
    /// <returns>移除指定字符后的字符串。如果字符串为 null 或空，或不以指定字符结尾，则返回原字符串 / The string with the specified character removed. Returns the original string if it is null or empty, or does not end with the specified character.</returns>
    public static string RemoveSuffix(this string self, char toRemove)
    {
        return self.IsNullOrEmpty() ? self : self.EndsWith(toRemove) ? self.Substring(0, self.Length - 1) : self;
    }

    /// <summary>
    /// 从字符串末尾移除指定的子字符串（如果存在）。
    /// </summary>
    /// <remarks>
    /// Removes the specified substring from the end of the string if it exists.
    /// This method can remove a substring of any length from the end of the string.
    /// If the substring to remove is null or empty, the original string is returned.
    /// Comparison is case-sensitive.
    /// </remarks>
    /// <param name="self">要处理的字符串 / The string to process.</param>
    /// <param name="toRemove">要移除的子字符串 / The substring to remove.</param>
    /// <returns>移除指定子字符串后的字符串。如果字符串为 null 或空，或不以指定子字符串结尾，则返回原字符串 / The string with the specified substring removed. Returns the original string if it is null or empty, or does not end with the specified substring.</returns>
    public static string RemoveSuffix(this string self, string toRemove)
    {
        if (self.IsNullOrEmpty() || toRemove.IsNullOrEmpty())
        {
            return self;
        }

        return self.EndsWith(toRemove) ? self.Substring(0, self.Length - toRemove.Length) : self;
    }

    /// <summary>
    /// 移除字符串中的所有空白字符。
    /// </summary>
    /// <remarks>
    /// Removes all whitespace characters from the string.
    /// Whitespace characters include: spaces, tabs, newlines, etc.
    /// Uses StringBuilder for better performance with large strings.
    /// </remarks>
    /// <param name="self">要处理的字符串 / The string to process.</param>
    /// <returns>移除所有空白字符后的字符串。如果输入为 null 或空，则返回原字符串 / The string with all whitespace characters removed. Returns the original string if input is null or empty.</returns>
    public static string RemoveWhiteSpace(this string self)
    {
        if (self.IsNullOrEmpty()) return self;
        var sb = new StringBuilder(self.Length);
        foreach (var c in self)
        {
            if (!char.IsWhiteSpace(c)) sb.Append(c);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 检查字符串是否为 null 或空字符串。
    /// </summary>
    /// <remarks>
    /// Checks whether the string is null or empty.
    /// This is an extension method version of string.IsNullOrEmpty.
    /// An empty string refers to a string with length 0.
    /// </remarks>
    /// <param name="str">要检查的字符串 / The string to check.</param>
    /// <returns>如果字符串为 null 或空字符串，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the string is null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 检查字符串是否为 null、空字符串或仅包含空白字符。
    /// </summary>
    /// <remarks>
    /// Checks whether the string is null, empty, or contains only whitespace characters.
    /// Combines IsNullOrEmpty and IsNullOrWhiteSpace checks.
    /// Better performance than calling both methods separately.
    /// </remarks>
    /// <param name="str">要检查的字符串 / The string to check.</param>
    /// <returns>如果字符串为 null、空字符串或仅包含空白字符，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the string is null, empty, or contains only whitespace; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmptyOrWhiteSpace(this string str)
    {
        return str.IsNullOrEmpty() || str.IsNullOrWhiteSpace();
    }

    /// <summary>
    /// 检查字符串是否不为 null、空字符串且不仅包含空白字符。
    /// </summary>
    /// <remarks>
    /// Checks whether the string is not null, not empty, and does not contain only whitespace characters.
    /// This is the logical negation of IsNullOrEmptyOrWhiteSpace.
    /// Used in scenarios where you need to confirm that a string contains actual content.
    /// </remarks>
    /// <param name="str">要检查的字符串 / The string to check.</param>
    /// <returns>如果字符串不为 null、不为空字符串且不仅包含空白字符，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the string is not null, not empty, and does not contain only whitespace; otherwise, <c>false</c>.</returns>
    public static bool IsNotNullOrEmptyOrWhiteSpace(this string str)
    {
        return !str.IsNullOrEmptyOrWhiteSpace();
    }

    /// <summary>
    /// 检查字符串是否不为 null 且不为空字符串。
    /// </summary>
    /// <remarks>
    /// Checks whether the string is not null and not empty.
    /// This is the logical negation of IsNullOrEmpty.
    /// Commonly used for parameter validation.
    /// </remarks>
    /// <param name="str">要检查的字符串 / The string to check.</param>
    /// <returns>如果字符串不为 null 且不为空字符串，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the string is not null and not empty; otherwise, <c>false</c>.</returns>
    public static bool IsNotNullOrEmpty(this string str)
    {
        return !str.IsNullOrEmpty();
    }

    /// <summary>
    /// 检查字符串是否为 null 或仅包含空白字符。
    /// </summary>
    /// <remarks>
    /// Checks whether the string is null or contains only whitespace characters.
    /// This is an extension method version of string.IsNullOrWhiteSpace.
    /// Whitespace characters include spaces, tabs, newlines, etc.
    /// </remarks>
    /// <param name="str">要检查的字符串 / The string to check.</param>
    /// <returns>如果字符串为 null 或仅包含空白字符，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the string is null or contains only whitespace; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 检查字符串是否不为 null 且不仅包含空白字符。
    /// </summary>
    /// <remarks>
    /// Checks whether the string is not null and does not contain only whitespace characters.
    /// This is the logical negation of IsNullOrWhiteSpace.
    /// Used in scenarios where you need to confirm that a string contains non-whitespace characters.
    /// Whitespace characters include spaces, tabs, newlines, etc.
    /// </remarks>
    /// <param name="str">要检查的字符串 / The string to check.</param>
    /// <returns>如果字符串不为 null 且不仅包含空白字符，则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the string is not null and does not contain only whitespace; otherwise, <c>false</c>.</returns>
    public static bool IsNotNullOrWhiteSpace(this string str)
    {
        return !str.IsNullOrWhiteSpace();
    }

    /// <summary>
    /// 将字符串按指定分隔符拆分为整数数组。
    /// </summary>
    /// <remarks>
    /// Splits a string by the specified separator and converts to an integer array.
    /// Uses int.TryParse for safe number conversion.
    /// If a part fails to convert, the corresponding position will keep the default value 0.
    /// Suitable for processing numeric sequence strings like "1+2+3".
    /// Returns a new array without modifying the original string.
    /// </remarks>
    /// <param name="str">要拆分的字符串 / The string to split.</param>
    /// <param name="sep">分隔符，默认为 '+' / The separator character, defaults to '+'.</param>
    /// <returns>拆分并转换后的整数数组。如果字符串为 null 或空，则返回空数组 / The split and converted integer array. Returns an empty array if the string is null or empty.</returns>
    public static int[] SplitToIntArray(this string str, char sep = '+')
    {
        if (string.IsNullOrEmpty(str))
        {
            return Array.Empty<int>();
        }

        var arr = str.Split(sep);
        var ret = new int[arr.Length];
        for (var i = 0; i < arr.Length; ++i)
        {
            if (int.TryParse(arr[i], out var t))
            {
                ret[i] = t;
            }
        }

        return ret;
    }

    /// <summary>
    /// 将字符串按两级分隔符拆分为二维整数数组。
    /// </summary>
    /// <remarks>
    /// Splits a string by two levels of separators into a two-dimensional integer array.
    /// Suitable for processing two-dimensional numeric sequence strings like "1+2;3+4;5+6".
    /// Uses SplitToIntArray for the second level split, maintaining consistent number conversion logic.
    /// If a row fails to convert, the corresponding position will return an empty array.
    /// Returns a new array without modifying the original string.
    /// </remarks>
    /// <param name="str">要拆分的字符串 / The string to split.</param>
    /// <param name="sep1">第一级分隔符，默认为 ';' / The first level separator, defaults to ';'.</param>
    /// <param name="sep2">第二级分隔符，默认为 '+' / The second level separator, defaults to '+'.</param>
    /// <returns>拆分并转换后的二维整数数组。如果字符串为 null 或空，则返回空数组 / The split and converted two-dimensional integer array. Returns an empty array if the string is null or empty.</returns>
    public static int[][] SplitTo2IntArray(this string str, char sep1 = ';', char sep2 = '+')
    {
        if (string.IsNullOrEmpty(str))
        {
            return Array.Empty<int[]>();
        }

        var arr = str.Split(sep1);
        if (arr.Length <= 0)
        {
            return Array.Empty<int[]>();
        }

        var ret = new int[arr.Length][];

        for (var i = 0; i < arr.Length; ++i)
        {
            ret[i] = arr[i].SplitToIntArray(sep2);
        }

        return ret;
    }

    [GeneratedRegex(@"^_+")]
    private static partial Regex LeadingUnderscoresRegex();

    [GeneratedRegex(@"([a-z0-9])([A-Z])")]
    private static partial Regex SnakeCaseRegex();

    /// <summary>
    /// 将驼峰命名法字符串转换为蛇形命名法（下划线分隔的小写形式）。
    /// </summary>
    /// <remarks>
    /// Converts a camelCase string to snake_case (underscore-separated lowercase format).
    /// Preserves leading underscores in the string.
    /// Inserts underscores between lowercase letters or digits followed by uppercase letters.
    /// Finally converts to all lowercase.
    /// Suitable for database field naming convention conversion.
    /// </remarks>
    /// <param name="input">要转换的字符串 / The string to convert.</param>
    /// <returns>转换后的蛇形命名法字符串。如果输入为 null 或空，则返回原字符串 / The converted snake_case string. Returns the original string if input is null or empty.</returns>
    public static string ConvertToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var startUnderscores = LeadingUnderscoresRegex().Match(input).Value;
        return startUnderscores + SnakeCaseRegex().Replace(input.TrimStart('_'), "$1_$2").ToLower();
    }

    /// <summary>
    /// 从字符串中移除所有中文字符。
    /// </summary>
    /// <remarks>
    /// Removes all Chinese characters from the string.
    /// Uses pre-compiled regular expressions for better performance.
    /// Only removes basic Chinese characters (0x4e00-0x9fa5).
    /// Does not remove Chinese punctuation.
    /// If the string does not contain Chinese characters, the original string is returned.
    /// </remarks>
    /// <param name="self">要处理的字符串 / The string to process.</param>
    /// <returns>移除所有中文字符后的字符串 / The string with all Chinese characters removed.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 为 null 时抛出 / Thrown when <paramref name="self"/> is null.</exception>
    public static string TrimZhCn(this string self)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        return CnReg.Replace(self, string.Empty);
    }

    /// <summary>
    /// 快速比较两个字符串是否相等，从末尾开始比较。
    /// </summary>
    /// <remarks>
    /// Quickly compares two strings for equality, starting from the end.
    /// Comparing from the end may be faster in some scenarios.
    /// Comparing length first can quickly identify unequal cases.
    /// Case-sensitive comparison.
    /// </remarks>
    /// <param name="self">当前字符串 / The current string.</param>
    /// <param name="target">要比较的目标字符串 / The target string to compare.</param>
    /// <returns>如果两个字符串相等则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the strings are equal; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="target"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="target"/> is null.</exception>
    public static bool EqualsFast(this string self, string target)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        if (self.Length != target.Length)
        {
            return false;
        }

        int ap = self.Length - 1;
        int bp = target.Length - 1;

        while (ap >= 0 && bp >= 0 && self[ap] == target[bp])
        {
            ap--;
            bp--;
        }

        return (bp < 0);
    }

    /// <summary>
    /// 快速检查字符串是否以指定字符串结尾，从末尾开始比较。
    /// </summary>
    /// <remarks>
    /// Quickly checks if a string ends with the specified string, comparing from the end.
    /// Comparing from the end can detect mismatches faster.
    /// Case-sensitive comparison.
    /// Suitable for scenarios with a large number of string suffix checks.
    /// </remarks>
    /// <param name="self">当前字符串 / The current string.</param>
    /// <param name="target">要检查的结尾字符串 / The suffix string to check.</param>
    /// <returns>如果字符串以指定字符串结尾则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the string ends with the specified string; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="target"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="target"/> is null.</exception>
    public static bool EndsWithFast(this string self, string target)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        int ap = self.Length - 1;
        int bp = target.Length - 1;

        while (ap >= 0 && bp >= 0 && self[ap] == target[bp])
        {
            ap--;
            bp--;
        }

        return (bp < 0);
    }

    /// <summary>
    /// 快速检查字符串是否以指定字符串开头，从开头开始比较。
    /// </summary>
    /// <remarks>
    /// Quickly checks if a string starts with the specified string, comparing from the beginning.
    /// Compares character by character from the beginning until a mismatch is found.
    /// Case-sensitive comparison.
    /// Suitable for scenarios with a large number of string prefix checks.
    /// </remarks>
    /// <param name="self">当前字符串 / The current string.</param>
    /// <param name="target">要检查的开头字符串 / The prefix string to check.</param>
    /// <returns>如果字符串以指定字符串开头则返回 <c>true</c>，否则返回 <c>false</c> / <c>true</c> if the string starts with the specified string; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="target"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="target"/> is null.</exception>
    public static bool StartsWithFast(this string self, string target)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        int aLen = self.Length;
        int bLen = target.Length;

        int ap = 0;
        int bp = 0;

        while (ap < aLen && bp < bLen && self[ap] == target[bp])
        {
            ap++;
            bp++;
        }

        return (bp == bLen);
    }
}