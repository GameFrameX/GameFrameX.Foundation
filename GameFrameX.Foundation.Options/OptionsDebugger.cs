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

using System.Reflection;
using System.Text;

namespace GameFrameX.Foundation.Options
{
    /// <summary>
    /// 选项调试器，用于打印和调试命令行参数。
    /// </summary>
    /// <remarks>
    /// Options debugger for printing and debugging command-line arguments and parsed options.
    /// Provides formatted console output with support for wide characters (CJK, emoji, etc.).
    /// </remarks>
    public static class OptionsDebugger
    {
        private const string OptionHeader = "选项 (Option)";
        private const string ValueHeader = "值 (Value)";
        private const string RequiredHeader = "必需 (Required)";
        private const string TypeNameHeader = "类型 (Type)";
        private const string DescriptionHeader = "描述 (Description)";
        private const string DefaultValueHeader = "默认值 (Default)";
        private const string RequiredYesLabel = "是 (Yes)";
        private const string RequiredNoLabel = "否 (No)";
        private const string NoDescriptionLabel = "无描述 (No Description)";
        private const string NoOptionAttributeLabel = "无选项特性 (No Option Attribute)";
        private const int MaxDisplayElements = 5;
        private const int DefaultConsoleWidth = 120;

        /// <summary>
        /// 打印解析完成后的选项对象。
        /// </summary>
        /// <remarks>
        /// Prints the parsed options object to the console in a formatted table.
        /// </remarks>
        /// <typeparam name="T">选项类型 / Options type</typeparam>
        /// <param name="options">解析后的选项对象 / Parsed options object</param>
        public static void PrintParsedOptions<T>(T options) where T : class
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  Command-line parameter and parsed configuration object information  ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            try
            {
                // 使用反射获取所有属性
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


                // 计算最大显示宽度
                int maxWidth = 0;
                var optionInfos = new List<(PropertyInfo property, string displayName, Attributes.OptionAttribute optionAttribute)>();

                foreach (var property in properties.OrderBy(p => p.Name))
                {
                    var attributes = property.GetCustomAttributes(true);
                    var optionAttribute = attributes.OfType<Attributes.OptionAttribute>().FirstOrDefault();
                

                    string displayName;
                    if (optionAttribute != null)
                    {
                        var longName = !string.IsNullOrEmpty(optionAttribute.LongName) ? optionAttribute.LongName : property.Name.ToLower();
                        displayName = $"--{longName}";
                    }
                    else
                    {
                        displayName = property.Name;
                    }

                    maxWidth = Math.Max(maxWidth, displayName.Length);
                    optionInfos.Add((property, displayName, optionAttribute));
                }

                // 添加2个字符的缓冲空间
                maxWidth += 2;

                // 使用计算出的最大宽度进行格式化输出（表格样式）
                var rows = new List<(string Name, string Value, string Required, string TypeName, string Description, string DefaultValue)>();
                int nameWidth = Math.Max(OptionHeader.Length, maxWidth);
                int valueWidth = ValueHeader.Length;
                int requiredWidth = RequiredHeader.Length;
                int typeWidth = TypeNameHeader.Length;
                int descWidth = DescriptionHeader.Length;
                int defaultWidth = DefaultValueHeader.Length;

                foreach (var (property, displayName, optionAttribute) in optionInfos)
                {
                    var value = property.GetValue(options);
                    var displayValue = FormatPropertyValue(value) ?? string.Empty;
                    var typeName = GetFriendlyTypeName(property.PropertyType) ?? string.Empty;
                    var required = optionAttribute != null ? (optionAttribute.Required ? RequiredYesLabel : RequiredNoLabel) : string.Empty;
                    var description = optionAttribute != null ? (optionAttribute.Description ?? NoDescriptionLabel) : NoOptionAttributeLabel;
                    var defaultVal = optionAttribute?.DefaultValue?.ToString() ?? string.Empty;

                    nameWidth = Math.Max(nameWidth, displayName.Length);
                    valueWidth = Math.Max(valueWidth, displayValue.Length);
                    requiredWidth = Math.Max(requiredWidth, required.Length);
                    typeWidth = Math.Max(typeWidth, typeName.Length);
                    descWidth = Math.Max(descWidth, description.Length);
                    defaultWidth = Math.Max(defaultWidth, defaultVal.Length);
                    
                    rows.Add((displayName, displayValue, required, typeName, description, defaultVal));
                }

                // 重新基于“显示宽度”计算各列宽度，中文字符按双列宽
                int hdName = GetDisplayWidth(OptionHeader);
                int hdValue = GetDisplayWidth(ValueHeader);
                int hdRequired = GetDisplayWidth(RequiredHeader);
                int hdType = GetDisplayWidth(TypeNameHeader);
                int hdDesc = GetDisplayWidth(DescriptionHeader);
                int hdDefault = GetDisplayWidth(DefaultValueHeader);

                nameWidth = Math.Max(hdName, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Name)) : 0);
                valueWidth = Math.Max(hdValue, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Value)) : 0);
                requiredWidth = Math.Max(hdRequired, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Required)) : 0);
                typeWidth = Math.Max(hdType, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.TypeName)) : 0);
                descWidth = Math.Max(hdDesc, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Description)) : 0);
                defaultWidth = Math.Max(hdDefault, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.DefaultValue)) : 0);

                // 限制每列最大宽度，但不得小于表头显示宽度
                int Limit(int width, int max) => Math.Min(width, max);
                int nameMax = Math.Max(24, hdName);
                int valueMax = Math.Max(30, hdValue);
                int requiredMax = Math.Max(2, hdRequired);
                int typeMax = Math.Max(18, hdType);
                int descMax = Math.Max(40, hdDesc);
                int defaultMax = Math.Max(20, hdDefault);

                nameWidth = Limit(nameWidth, nameMax);
                valueWidth = Limit(valueWidth, valueMax);
                requiredWidth = Limit(requiredWidth, requiredMax);
                typeWidth = Limit(typeWidth, typeMax);
                descWidth = Limit(descWidth, descMax);
                defaultWidth = Limit(defaultWidth, defaultMax);

                // 记录各列最小宽度（不得压缩到小于表头显示宽度）
                int minNameWidth = hdName;
                int minValueWidth = hdValue;
                int minRequiredWidth = hdRequired;
                int minTypeWidth = hdType;
                int minDescWidth = hdDesc;
                int minDefaultWidth = hdDefault;

                // 根据控制台宽度自适应整体表格宽度，确保整齐对齐
                int columnsCount = 6;
                int CalculateTotalWidth() => nameWidth + valueWidth + requiredWidth + typeWidth + descWidth + defaultWidth + (2 * columnsCount) + (columnsCount + 1);
                int consoleWidth = 0;
                try
                {
                    consoleWidth = Math.Max(60, Math.Min(Console.BufferWidth, Console.WindowWidth));
                }
                catch
                {
                    consoleWidth = DefaultConsoleWidth;
                }

                int maxTableWidth = Math.Max(60, consoleWidth - 1);
                while (CalculateTotalWidth() > maxTableWidth)
                {
                    if (descWidth > minDescWidth)
                    {
                        descWidth--;
                        continue;
                    }


                    if (valueWidth > minValueWidth)
                    {
                        valueWidth--;
                        continue;
                    }

                    if (nameWidth > minNameWidth)
                    {
                        nameWidth--;
                        continue;
                    }

                    if (typeWidth > minTypeWidth)
                    {
                        typeWidth--;
                        continue;
                    }

                    if (defaultWidth > minDefaultWidth)
                    {
                        defaultWidth--;
                        continue;
                    }

                    // 已无法继续压缩而不破坏表头完整展示，退出
                    break;
                }

                string BuildBorder(char left, char sep, char right, char fill)
                {
                    // 使用 string.Create 优化：单次分配创建边框字符串
                    // Use string.Create for optimization: create border string with single allocation
                    int totalLength = 1 + (nameWidth + 2) + 1 + (valueWidth + 2) + 1 + (requiredWidth + 2) + 1 + (typeWidth + 2) + 1 + (descWidth + 2) + 1 + (defaultWidth + 2) + 1;
                    return string.Create(totalLength, (left, sep, right, fill, nameWidth, valueWidth, requiredWidth, typeWidth, descWidth, defaultWidth), static (span, state) =>
                    {
                        int pos = 0;
                        var (l, s, r, f, nw, vw, rw, tw, dw, dfw) = state;

                        span[pos++] = l;
                        span.Slice(pos, nw + 2).Fill(f);
                        pos += nw + 2;
                        span[pos++] = s;
                        span.Slice(pos, vw + 2).Fill(f);
                        pos += vw + 2;
                        span[pos++] = s;
                        span.Slice(pos, rw + 2).Fill(f);
                        pos += rw + 2;
                        span[pos++] = s;
                        span.Slice(pos, tw + 2).Fill(f);
                        pos += tw + 2;
                        span[pos++] = s;
                        span.Slice(pos, dw + 2).Fill(f);
                        pos += dw + 2;
                        span[pos++] = s;
                        span.Slice(pos, dfw + 2).Fill(f);
                        pos += dfw + 2;
                        span[pos] = r;
                    });
                }


                // 打印表头
                Console.WriteLine(BuildBorder('┌', '┬', '┐', '─'));
                Console.WriteLine($"│ {TruncPadDisplay(OptionHeader, nameWidth)} │ {TruncPadDisplay(ValueHeader, valueWidth)} │ {CenterPadDisplay(RequiredHeader, requiredWidth)} │ {TruncPadDisplay(TypeNameHeader, typeWidth)} │ {TruncPadDisplay(DescriptionHeader, descWidth)} │ {TruncPadDisplay(DefaultValueHeader, defaultWidth)} │");
                Console.WriteLine(BuildBorder('├', '┼', '┤', '─'));

                // 打印数据行
                foreach (var row in rows)
                {
                    var nameLines = WrapToDisplayLines(row.Name, nameWidth);
                    var valueLines = WrapToDisplayLines(row.Value, valueWidth);
                    var reqText = CenterPadDisplay(row.Required, requiredWidth);
                    var typeLines = WrapToDisplayLines(row.TypeName, typeWidth);
                    var descLines = WrapToDisplayLines(row.Description, descWidth);
                    var defLines = WrapToDisplayLines(row.DefaultValue, defaultWidth);

                    int lineCount = new[]
                    {
                        nameLines.Count,
                        valueLines.Count,
                        1,
                        typeLines.Count,
                        descLines.Count,
                        defLines.Count
                    }.Max();

                    for (int i = 0; i < lineCount; i++)
                    {
                        string nameLine = i < nameLines.Count ? nameLines[i] : new string(' ', nameWidth);
                        string valueLine = i < valueLines.Count ? valueLines[i] : new string(' ', valueWidth);
                        string reqLine = i == 0 ? reqText : new string(' ', requiredWidth);
                        string typeLine = i < typeLines.Count ? typeLines[i] : new string(' ', typeWidth);
                        string descLine = i < descLines.Count ? descLines[i] : new string(' ', descWidth);
                        string defLine = i < defLines.Count ? defLines[i] : new string(' ', defaultWidth);

                        Console.WriteLine($"│ {nameLine} │ {valueLine} │ {reqLine} │ {typeLine} │ {descLine} │ {defLine} │");
                    }
                }

                // 底部边框
                Console.WriteLine(BuildBorder('└', '┴', '┘', '─'));

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when printing a configuration object: {ex.Message}");
                Console.WriteLine(ex);
            }

            Console.WriteLine();
        }

        static string CenterPadDisplay(string s, int width)
        {
            var t = TruncPadDisplay(s, width);
            int w = GetDisplayWidth(t);
            if (w >= width)
            {
                return t;
            }

            int pad = width - w;
            int left = pad / 2;
            int right = pad - left;
            return new string(' ', left) + t + new string(' ', right);
        }

        static string TruncPadDisplay(string s, int width)
        {
            s ??= string.Empty;
            int w = 0;
            var sb = new StringBuilder();
            foreach (var rune in s.EnumerateRunes())
            {
                int rw = IsWideCharacter(rune.Value) ? 2 : 1;
                if (w + rw > width)
                {
                    if (width > 1)
                    {
                        sb.Append('…');
                    }

                    break;
                }

                sb.Append(rune.ToString());
                w += rw;
            }

            while (GetDisplayWidth(sb.ToString()) < width)
            {
                sb.Append(' ');
            }

            return sb.ToString();
        }

        // 将文本按显示宽度拆分为多行，保证每行宽度填满
        static List<string> WrapToDisplayLines(string s, int width)
        {
            var lines = new List<string>();
            s ??= string.Empty;
            if (width <= 0)
            {
                lines.Add(string.Empty);
                return lines;
            }

            var sb = new StringBuilder();
            int w = 0;
            foreach (var rune in s.EnumerateRunes())
            {
                int rw = IsWideCharacter(rune.Value) ? 2 : 1;

                if (w + rw > width)
                {
                    while (GetDisplayWidth(sb.ToString()) < width)
                    {
                        sb.Append(' ');
                    }

                    lines.Add(sb.ToString());
                    sb.Clear();
                    w = 0;
                }

                sb.Append(rune.ToString());
                w += rw;
            }

            // 收尾：加入最后一行，并填充到指定宽度
            while (GetDisplayWidth(sb.ToString()) < width)
            {
                sb.Append(' ');
            }

            lines.Add(sb.ToString());
            if (lines.Count == 0)
            {
                lines.Add(new string(' ', width));
            }

            return lines;
        }

        // 显示宽度相关函数：中文及全角字符按双列宽处理
        // Display width function: CJK and fullwidth characters are treated as double-width
        static int GetDisplayWidth(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            int w = 0;
            foreach (var rune in s.EnumerateRunes())
            {
                w += IsWideCharacter(rune.Value) ? 2 : 1;
            }

            return w;
        }

        /// <summary>
        /// 判断字符是否为宽字符（在终端中占用两个字符宽度）
        /// Determines if a character is wide (occupies two character widths in terminal)
        /// </summary>
        /// <param name="codePoint">Unicode 码点 / Unicode code point</param>
        /// <returns>是否为宽字符 / Whether the character is wide</returns>
        static bool IsWideCharacter(int codePoint)
        {
            // 基于 Unicode 标准的宽字符范围判断
            // Based on Unicode standard wide character ranges
            return codePoint switch
            {
                // CJK 统一汉字 / CJK Unified Ideographs
                >= 0x4E00 and <= 0x9FFF => true,
                // CJK 扩展 A / CJK Extension A
                >= 0x3400 and <= 0x4DBF => true,
                // CJK 扩展 B-F / CJK Extensions B-F
                >= 0x20000 and <= 0x2CEAF => true,
                // CJK 扩展 G / CJK Extension G
                >= 0x30000 and <= 0x3134F => true,
                // CJK 兼容汉字 / CJK Compatibility Ideographs
                >= 0xF900 and <= 0xFAFF => true,
                // CJK 兼容补充 / CJK Compatibility Supplement
                >= 0x2F800 and <= 0x2FA1F => true,
                // CJK 符号和标点 / CJK Symbols & Punctuation
                >= 0x3000 and <= 0x303F => true,
                // 平假名 / Hiragana
                >= 0x3040 and <= 0x309F => true,
                // 片假名 / Katakana
                >= 0x30A0 and <= 0x30FF => true,
                // 日文兼容片假名 / Katakana Phonetic Extensions
                >= 0x31F0 and <= 0x31FF => true,
                // 韩文字母 / Hangul
                >= 0xAC00 and <= 0xD7AF => true,
                // 韩文兼容字母 / Hangul Jamo
                >= 0x1100 and <= 0x11FF => true,
                // 全角 ASCII 变体 / Fullwidth ASCII variants
                >= 0xFF01 and <= 0xFF60 => true,
                // 全角符号 / Fullwidth symbols
                >= 0xFFE0 and <= 0xFFE6 => true,
                // 箭头符号 / Arrows
                >= 0x2190 and <= 0x21FF => true,
                // 数学运算符 / Mathematical Operators
                >= 0x2200 and <= 0x22FF => true,
                // 制表符 / Box Drawing
                >= 0x2500 and <= 0x257F => true,
                // 方块元素 / Block Elements
                >= 0x2580 and <= 0x259F => true,
                // 几何图形 / Geometric Shapes
                >= 0x25A0 and <= 0x25FF => true,
                // 杂项符号 / Miscellaneous Symbols
                >= 0x2600 and <= 0x26FF => true,
                // 丁贝符 / Dingbats
                >= 0x2700 and <= 0x27BF => true,
                // 表情符号 / Emoji & Symbols
                >= 0x1F000 and <= 0x1FAFF => true,
                // 音乐符号 / Musical Symbols
                >= 0x1D000 and <= 0x1D24F => true,
                // 古代符号 / Ancient Symbols
                >= 0x10100 and <= 0x1013F => true,
                // 货币符号 / Currency Symbols (部分为宽字符)
                >= 0x20A0 and <= 0x20CF => true,
                // 字母式符号 / Letterlike Symbols
                >= 0x2100 and <= 0x214F => true,
                // 数字形式 / Number Forms
                >= 0x2150 and <= 0x218F => true,
                // 泰文 / Thai (部分为宽字符)
                >= 0x0E01 and <= 0x0E7F => true,
                // 藏文 / Tibetan
                >= 0x0F00 and <= 0x0FFF => true,
                // 蒙古文 / Mongolian
                >= 0x1800 and <= 0x18AF => true,
                // 彝文 / Yi
                >= 0xA000 and <= 0xA48F => true,
                // 傈僳文 / Lisu
                >= 0xA4D0 and <= 0xA4FF => true,
                // 预设：窄字符 / Default: narrow character
                _ => false
            };
        }

        /// <summary>
        /// 格式化属性值用于显示。
        /// </summary>
        /// <remarks>
        /// Formats the property value for display in the console.
        /// </remarks>
        /// <param name="value">属性值 / Property value</param>
        /// <returns>格式化后的字符串 / Formatted string</returns>
        private static string FormatPropertyValue(object value)
        {
            if (value == null)
            {
                return "<null>";
            }

            if (value is string str)
            {
                return $"\"{str}\"";
            }

            if (value is bool)
            {
                return value.ToString().ToLower();
            }

            if (value.GetType().IsArray)
            {
                var array = (Array)value;
                var elements = new List<string>();
                for (int i = 0; i < Math.Min(array.Length, MaxDisplayElements); i++)
                {
                    elements.Add(array.GetValue(i)?.ToString() ?? "null");
                }

                var result = $"[{string.Join(", ", elements)}]";
                if (array.Length > MaxDisplayElements)
                {
                    result += $" (Total {array.Length} elements / 共{array.Length}个元素)";
                }

                return result;
            }

            if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (System.Collections.IList)value;
                var elements = new List<string>();
                for (int i = 0; i < Math.Min(list.Count, MaxDisplayElements); i++)
                {
                    elements.Add(list[i]?.ToString() ?? "null");
                }

                var result = $"[{string.Join(", ", elements)}]";
                if (list.Count > MaxDisplayElements)
                {
                    result += $" (Total {list.Count} elements / 共{list.Count}个元素)";
                }

                return result;
            }

            return value.ToString();
        }

        /// <summary>
        /// 获取友好的类型名称。
        /// </summary>
        /// <remarks>
        /// Gets a user-friendly type name for display purposes.
        /// </remarks>
        /// <param name="type">类型 / Type</param>
        /// <returns>友好的类型名称 / Friendly type name</returns>
        private static string GetFriendlyTypeName(Type type)
        {
            if (type == typeof(string))
            {
                return nameof(String);
            }

            if (type == typeof(byte))
            {
                return nameof(Byte);
            }

            if (type == typeof(short) || type == typeof(ushort))
            {
                return nameof(Int16);
            }

            if (type == typeof(int) || type == typeof(uint))
            {
                return nameof(Int32);
            }

            if (type == typeof(bool))
            {
                return nameof(Boolean);
            }

            if (type == typeof(double))
            {
                return nameof(Double);
            }

            if (type == typeof(float))
            {
                return nameof(Single);
            }

            if (type == typeof(long) || type == typeof(ulong))
            {
                return nameof(Int64);
            }

            if (type == typeof(DateTime))
            {
                return nameof(DateTime);
            }

            if (type.IsArray)
            {
                return $"Array of {GetFriendlyTypeName(type.GetElementType())}";
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return $"{GetFriendlyTypeName(type.GetGenericArguments()[0])} list";
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"Nullable<{GetFriendlyTypeName(type.GetGenericArguments()[0])}>";
            }

            return type.Name;
        }
    }
}
