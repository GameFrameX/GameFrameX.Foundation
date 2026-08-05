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
        private const int TableColumnsCount = 6;

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
                var (optionInfos, maxNameWidth) = CollectOptionInfos<T>();
                var layout = BuildTableLayout(options, optionInfos, maxNameWidth);
                ApplyDisplayWidths(layout);
                FitLayoutToConsole(layout);
                PrintTable(layout);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when printing a configuration object: {ex.Message}");
                Console.WriteLine(ex);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 使用反射收集选项类型的属性元数据，并计算最长显示名宽度（含 2 字符缓冲）。
        /// </summary>
        private static (List<(PropertyInfo Property, string DisplayName, Attributes.OptionAttribute OptionAttribute)> Infos, int MaxNameWidth) CollectOptionInfos<T>()
        {
            // 使用反射获取所有属性
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var optionInfos = new List<(PropertyInfo Property, string DisplayName, Attributes.OptionAttribute OptionAttribute)>();
            int maxWidth = 0;

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
            return (optionInfos, maxWidth);
        }

        /// <summary>
        /// 构建表格数据行并初始化各列字符宽度初值。
        /// </summary>
        private static TableLayout BuildTableLayout<T>(T options, IReadOnlyList<(PropertyInfo Property, string DisplayName, Attributes.OptionAttribute OptionAttribute)> optionInfos, int maxNameWidth)
        {
            var layout = new TableLayout
            {
                NameWidth = Math.Max(OptionHeader.Length, maxNameWidth),
                ValueWidth = ValueHeader.Length,
                RequiredWidth = RequiredHeader.Length,
                TypeWidth = TypeNameHeader.Length,
                DescWidth = DescriptionHeader.Length,
                DefaultWidth = DefaultValueHeader.Length
            };

            foreach (var (property, displayName, optionAttribute) in optionInfos)
            {
                var row = BuildRow(options, property, displayName, optionAttribute);
                layout.NameWidth = Math.Max(layout.NameWidth, row.Name.Length);
                layout.ValueWidth = Math.Max(layout.ValueWidth, row.Value.Length);
                layout.RequiredWidth = Math.Max(layout.RequiredWidth, row.Required.Length);
                layout.TypeWidth = Math.Max(layout.TypeWidth, row.TypeName.Length);
                layout.DescWidth = Math.Max(layout.DescWidth, row.Description.Length);
                layout.DefaultWidth = Math.Max(layout.DefaultWidth, row.DefaultValue.Length);
                layout.Rows.Add(row);
            }

            return layout;
        }

        /// <summary>
        /// 格式化单个属性对应的表格行字段值。
        /// </summary>
        private static (string Name, string Value, string Required, string TypeName, string Description, string DefaultValue) BuildRow<T>(T options, PropertyInfo property, string displayName, Attributes.OptionAttribute optionAttribute)
        {
            var value = property.GetValue(options);
            var isSensitive = optionAttribute?.Sensitive == true;
            var displayValue = FormatPropertyValue(value, isSensitive) ?? string.Empty;
            var typeName = GetFriendlyTypeName(property.PropertyType) ?? string.Empty;
            var required = optionAttribute != null ? (optionAttribute.Required ? RequiredYesLabel : RequiredNoLabel) : string.Empty;
            var description = optionAttribute != null ? (optionAttribute.Description ?? NoDescriptionLabel) : NoOptionAttributeLabel;
            var defaultVal = optionAttribute?.DefaultValue == null ? string.Empty : FormatPropertyValue(optionAttribute.DefaultValue, isSensitive);

            return (displayName, displayValue, required, typeName, description, defaultVal);
        }

        /// <summary>
        /// 重新基于“显示宽度”计算各列宽度（中文字符按双列宽），并施加列宽上限。
        /// </summary>
        private static void ApplyDisplayWidths(TableLayout layout)
        {
            int hdName = GetDisplayWidth(OptionHeader);
            int hdValue = GetDisplayWidth(ValueHeader);
            int hdRequired = GetDisplayWidth(RequiredHeader);
            int hdType = GetDisplayWidth(TypeNameHeader);
            int hdDesc = GetDisplayWidth(DescriptionHeader);
            int hdDefault = GetDisplayWidth(DefaultValueHeader);

            layout.NameWidth = Math.Max(hdName, layout.Rows.Count > 0 ? layout.Rows.Max(r => GetDisplayWidth(r.Name)) : 0);
            layout.ValueWidth = Math.Max(hdValue, layout.Rows.Count > 0 ? layout.Rows.Max(r => GetDisplayWidth(r.Value)) : 0);
            layout.RequiredWidth = Math.Max(hdRequired, layout.Rows.Count > 0 ? layout.Rows.Max(r => GetDisplayWidth(r.Required)) : 0);
            layout.TypeWidth = Math.Max(hdType, layout.Rows.Count > 0 ? layout.Rows.Max(r => GetDisplayWidth(r.TypeName)) : 0);
            layout.DescWidth = Math.Max(hdDesc, layout.Rows.Count > 0 ? layout.Rows.Max(r => GetDisplayWidth(r.Description)) : 0);
            layout.DefaultWidth = Math.Max(hdDefault, layout.Rows.Count > 0 ? layout.Rows.Max(r => GetDisplayWidth(r.DefaultValue)) : 0);

            // 限制每列最大宽度，但不得小于表头显示宽度
            layout.NameWidth = Math.Min(layout.NameWidth, Math.Max(24, hdName));
            layout.ValueWidth = Math.Min(layout.ValueWidth, Math.Max(30, hdValue));
            layout.RequiredWidth = Math.Min(layout.RequiredWidth, Math.Max(2, hdRequired));
            layout.TypeWidth = Math.Min(layout.TypeWidth, Math.Max(18, hdType));
            layout.DescWidth = Math.Min(layout.DescWidth, Math.Max(40, hdDesc));
            layout.DefaultWidth = Math.Min(layout.DefaultWidth, Math.Max(20, hdDefault));
        }

        /// <summary>
        /// 根据控制台宽度自适应整体表格宽度，按优先级逐列收缩直到塞入或全部触底。
        /// </summary>
        private static void FitLayoutToConsole(TableLayout layout)
        {
            int consoleWidth = GetConsoleWidth();
            int maxTableWidth = Math.Max(60, consoleWidth - 1);
            int descFloor = GetDisplayWidth(DescriptionHeader);
            int valueFloor = GetDisplayWidth(ValueHeader);
            int nameFloor = GetDisplayWidth(OptionHeader);
            int typeFloor = GetDisplayWidth(TypeNameHeader);
            int defaultFloor = GetDisplayWidth(DefaultValueHeader);

            while (layout.TotalWidth > maxTableWidth)
            {
                if (!TryShrinkOneColumn(layout, descFloor, valueFloor, nameFloor, typeFloor, defaultFloor))
                {
                    // 已无法继续压缩而不破坏表头完整展示，退出
                    break;
                }
            }
        }

        /// <summary>
        /// 读取当前控制台宽度，异常时回退默认宽度。
        /// </summary>
        private static int GetConsoleWidth()
        {
            try
            {
                return Math.Max(60, Math.Min(Console.BufferWidth, Console.WindowWidth));
            }
            catch
            {
                return DefaultConsoleWidth;
            }
        }

        /// <summary>
        /// 按 desc → value → name → type → default 优先级尝试收缩一列；成功返回 true，全部触底返回 false。
        /// </summary>
        private static bool TryShrinkOneColumn(TableLayout layout, int descFloor, int valueFloor, int nameFloor, int typeFloor, int defaultFloor)
        {
            if (layout.DescWidth > descFloor)
            {
                layout.DescWidth--;
                return true;
            }

            if (layout.ValueWidth > valueFloor)
            {
                layout.ValueWidth--;
                return true;
            }

            if (layout.NameWidth > nameFloor)
            {
                layout.NameWidth--;
                return true;
            }

            if (layout.TypeWidth > typeFloor)
            {
                layout.TypeWidth--;
                return true;
            }

            if (layout.DefaultWidth > defaultFloor)
            {
                layout.DefaultWidth--;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 按计算好的列宽打印表头、数据行与底部边框。
        /// </summary>
        private static void PrintTable(TableLayout layout)
        {
            // 打印表头
            Console.WriteLine(BuildBorder('┌', '┬', '┐', '─', layout));
            Console.WriteLine($"│ {TruncPadDisplay(OptionHeader, layout.NameWidth)} │ {TruncPadDisplay(ValueHeader, layout.ValueWidth)} │ {CenterPadDisplay(RequiredHeader, layout.RequiredWidth)} │ {TruncPadDisplay(TypeNameHeader, layout.TypeWidth)} │ {TruncPadDisplay(DescriptionHeader, layout.DescWidth)} │ {TruncPadDisplay(DefaultValueHeader, layout.DefaultWidth)} │");
            Console.WriteLine(BuildBorder('├', '┼', '┤', '─', layout));

            // 打印数据行
            foreach (var row in layout.Rows)
            {
                var nameLines = WrapToDisplayLines(row.Name, layout.NameWidth);
                var valueLines = WrapToDisplayLines(row.Value, layout.ValueWidth);
                var reqText = CenterPadDisplay(row.Required, layout.RequiredWidth);
                var typeLines = WrapToDisplayLines(row.TypeName, layout.TypeWidth);
                var descLines = WrapToDisplayLines(row.Description, layout.DescWidth);
                var defLines = WrapToDisplayLines(row.DefaultValue, layout.DefaultWidth);

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
                    string nameLine = LineOrBlank(nameLines, i, layout.NameWidth);
                    string valueLine = LineOrBlank(valueLines, i, layout.ValueWidth);
                    string reqLine = i == 0 ? reqText : new string(' ', layout.RequiredWidth);
                    string typeLine = LineOrBlank(typeLines, i, layout.TypeWidth);
                    string descLine = LineOrBlank(descLines, i, layout.DescWidth);
                    string defLine = LineOrBlank(defLines, i, layout.DefaultWidth);

                    Console.WriteLine($"│ {nameLine} │ {valueLine} │ {reqLine} │ {typeLine} │ {descLine} │ {defLine} │");
                }
            }

            // 底部边框
            Console.WriteLine(BuildBorder('└', '┴', '┘', '─', layout));

            Console.WriteLine();
        }

        /// <summary>
        /// 返回多行折叠后的第 index 行，越界则返回指定宽度的空格填充。
        /// </summary>
        private static string LineOrBlank(List<string> lines, int index, int width)
        {
            return index < lines.Count ? lines[index] : new string(' ', width);
        }

        /// <summary>
        /// 使用 string.Create 优化：单次分配创建边框字符串。
        /// Use string.Create for optimization: create border string with single allocation
        /// </summary>
        private static string BuildBorder(char left, char sep, char right, char fill, TableLayout layout)
        {
            int totalLength = 1 + (layout.NameWidth + 2) + 1 + (layout.ValueWidth + 2) + 1 + (layout.RequiredWidth + 2) + 1 + (layout.TypeWidth + 2) + 1 + (layout.DescWidth + 2) + 1 + (layout.DefaultWidth + 2) + 1;
            return string.Create(totalLength, (left, sep, right, fill, layout.NameWidth, layout.ValueWidth, layout.RequiredWidth, layout.TypeWidth, layout.DescWidth, layout.DefaultWidth), static (span, state) =>
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

        /// <summary>
        /// 解析结果表格的排版布局：6 列显示宽度 + 数据行集合，在排版各阶段逐步修正 / 收缩。
        /// </summary>
        private sealed class TableLayout
        {
            public List<(string Name, string Value, string Required, string TypeName, string Description, string DefaultValue)> Rows
                = new List<(string Name, string Value, string Required, string TypeName, string Description, string DefaultValue)>();

            public int NameWidth;
            public int ValueWidth;
            public int RequiredWidth;
            public int TypeWidth;
            public int DescWidth;
            public int DefaultWidth;

            /// <summary>
            /// 表格整体渲染宽度：6 列宽度之和 + 列内边距（每列 2）+ 列分隔符（列数 + 1）。
            /// </summary>
            public int TotalWidth => NameWidth + ValueWidth + RequiredWidth + TypeWidth + DescWidth + DefaultWidth + (2 * TableColumnsCount) + (TableColumnsCount + 1);
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

        // 基于 Unicode 标准的宽字符范围表（中文 / 全角 / 部分符号 / emoji 等按终端双列宽处理）
        // Wide character range table based on Unicode standard (CJK / fullwidth / some symbols / emoji treated as double-width in terminal)
        private static readonly (int Min, int Max)[] WideCharacterRanges = new (int Min, int Max)[]
        {
            // CJK 统一汉字 / CJK Unified Ideographs
            (0x4E00, 0x9FFF),
            // CJK 扩展 A / CJK Extension A
            (0x3400, 0x4DBF),
            // CJK 扩展 B-F / CJK Extensions B-F
            (0x20000, 0x2CEAF),
            // CJK 扩展 G / CJK Extension G
            (0x30000, 0x3134F),
            // CJK 兼容汉字 / CJK Compatibility Ideographs
            (0xF900, 0xFAFF),
            // CJK 兼容补充 / CJK Compatibility Supplement
            (0x2F800, 0x2FA1F),
            // CJK 符号和标点 / CJK Symbols & Punctuation
            (0x3000, 0x303F),
            // 平假名 / Hiragana
            (0x3040, 0x309F),
            // 片假名 / Katakana
            (0x30A0, 0x30FF),
            // 日文兼容片假名 / Katakana Phonetic Extensions
            (0x31F0, 0x31FF),
            // 韩文字母 / Hangul
            (0xAC00, 0xD7AF),
            // 韩文兼容字母 / Hangul Jamo
            (0x1100, 0x11FF),
            // 全角 ASCII 变体 / Fullwidth ASCII variants
            (0xFF01, 0xFF60),
            // 全角符号 / Fullwidth symbols
            (0xFFE0, 0xFFE6),
            // 箭头符号 / Arrows
            (0x2190, 0x21FF),
            // 数学运算符 / Mathematical Operators
            (0x2200, 0x22FF),
            // 制表符 / Box Drawing
            (0x2500, 0x257F),
            // 方块元素 / Block Elements
            (0x2580, 0x259F),
            // 几何图形 / Geometric Shapes
            (0x25A0, 0x25FF),
            // 杂项符号 / Miscellaneous Symbols
            (0x2600, 0x26FF),
            // 丁贝符 / Dingbats
            (0x2700, 0x27BF),
            // 表情符号 / Emoji & Symbols
            (0x1F000, 0x1FAFF),
            // 音乐符号 / Musical Symbols
            (0x1D000, 0x1D24F),
            // 古代符号 / Ancient Symbols
            (0x10100, 0x1013F),
            // 货币符号 / Currency Symbols (部分为宽字符)
            (0x20A0, 0x20CF),
            // 字母式符号 / Letterlike Symbols
            (0x2100, 0x214F),
            // 数字形式 / Number Forms
            (0x2150, 0x218F),
            // 泰文 / Thai (部分为宽字符)
            (0x0E01, 0x0E7F),
            // 藏文 / Tibetan
            (0x0F00, 0x0FFF),
            // 蒙古文 / Mongolian
            (0x1800, 0x18AF),
            // 彝文 / Yi
            (0xA000, 0xA48F),
            // 傈僳文 / Lisu
            (0xA4D0, 0xA4FF)
        };

        /// <summary>
        /// 判断字符是否为宽字符（在终端中占用两个字符宽度）
        /// Determines if a character is wide (occupies two character widths in terminal)
        /// </summary>
        /// <param name="codePoint">Unicode 码点 / Unicode code point</param>
        /// <returns>是否为宽字符 / Whether the character is wide</returns>
        static bool IsWideCharacter(int codePoint)
        {
            foreach (var range in WideCharacterRanges)
            {
                if (codePoint >= range.Min && codePoint <= range.Max)
                {
                    return true;
                }
            }

            // 预设：窄字符 / Default: narrow character
            return false;
        }

        /// <summary>
        /// 格式化属性值用于显示。
        /// </summary>
        /// <remarks>
        /// Formats the property value for display in the console.
        /// </remarks>
        /// <param name="value">属性值 / Property value</param>
        /// <param name="isSensitive">是否为敏感值 / Whether the value is sensitive</param>
        /// <returns>格式化后的字符串 / Formatted string</returns>
        private static string FormatPropertyValue(object value, bool isSensitive = false)
        {
            if (value == null)
            {
                return "<null>";
            }

            if (isSensitive)
            {
                return "[REDACTED]";
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
        /// 基本类型 → 友好类型名的映射表 / Mapping of primitive types to friendly type names.
        /// </summary>
        private static readonly Dictionary<Type, string> FriendlyTypeNames = new Dictionary<Type, string>
        {
            { typeof(string), nameof(String) },
            { typeof(byte), nameof(Byte) },
            { typeof(short), nameof(Int16) },
            { typeof(ushort), nameof(Int16) },
            { typeof(int), nameof(Int32) },
            { typeof(uint), nameof(Int32) },
            { typeof(bool), nameof(Boolean) },
            { typeof(double), nameof(Double) },
            { typeof(float), nameof(Single) },
            { typeof(long), nameof(Int64) },
            { typeof(ulong), nameof(Int64) },
            { typeof(DateTime), nameof(DateTime) },
        };

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
            if (FriendlyTypeNames.TryGetValue(type, out var friendlyName))
            {
                return friendlyName;
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
