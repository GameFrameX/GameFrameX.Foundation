using System.Reflection;
using System.Text;

namespace GameFrameX.Foundation.Options
{
    /// <summary>
    /// 选项调试器，用于打印和调试命令行参数
    /// </summary>
    public static class OptionsDebugger
    {
        const string OptionHeader = "选项 (Option)";
        const string ValueHeader = "值 (Value)";
        const string RequiredHeader = "必需 (Required)";
        const string TypeNameHeader = "类型 (Type)";
        const string DescriptionHeader = "描述 (Description)";
        const string DefaultValueHeader = "默认值 (Default)";
        const string HelpTextHeader = "帮助文本 (Help)";
        const string RequiredYesLabel = "是 (Yes)";
        const string RequiredNoLabel = "否 (No)";
        const string NoDescriptionLabel = "无描述 (No Description)";
        const string NoOptionAttributeLabel = "无选项特性 (No Option Attribute)";

        /// <summary>
        /// 打印解析完成后的选项对象
        /// </summary>
        /// <typeparam name="T">选项类型</typeparam>
        /// <param name="options">解析后的选项对象</param>
        public static void PrintParsedOptions<T>(T options) where T : class
        {
            try
            {
                // 使用反射获取所有属性
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


                // 计算最大显示宽度
                int maxWidth = 0;
                var optionInfos = new List<(PropertyInfo property, string displayName, Attributes.OptionAttribute optionAttribute, Attributes.HelpTextAttribute helpTextAttribute)>();

                foreach (var property in properties.OrderBy(p => p.Name))
                {
                    var attributes = property.GetCustomAttributes(true);
                    var optionAttribute = attributes.OfType<Attributes.OptionAttribute>().FirstOrDefault();
                    var helpTextAttribute = attributes.OfType<Attributes.HelpTextAttribute>().FirstOrDefault();

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
                    optionInfos.Add((property, displayName, optionAttribute, helpTextAttribute));
                }

                // 添加2个字符的缓冲空间
                maxWidth += 2;

                // 使用计算出的最大宽度进行格式化输出（表格样式）
                var rows = new List<(string Name, string Value, string Required, string TypeName, string Description, string DefaultValue, string HelpText)>();
                int nameWidth = Math.Max(OptionHeader.Length, maxWidth);
                int valueWidth = ValueHeader.Length;
                int requiredWidth = RequiredHeader.Length;
                int typeWidth = TypeNameHeader.Length;
                int descWidth = DescriptionHeader.Length;
                int defaultWidth = DefaultValueHeader.Length;
                int helpWidth = HelpTextHeader.Length;

                foreach (var (property, displayName, optionAttribute, helpTextAttribute) in optionInfos)
                {
                    var value = property.GetValue(options);
                    var displayValue = FormatPropertyValue(value) ?? string.Empty;
                    var typeName = GetFriendlyTypeName(property.PropertyType) ?? string.Empty;
                    var required = optionAttribute != null ? (optionAttribute.Required ? RequiredYesLabel : RequiredNoLabel) : string.Empty;
                    var description = optionAttribute != null ? (optionAttribute.Description ?? NoDescriptionLabel) : NoOptionAttributeLabel;
                    var defaultVal = optionAttribute?.DefaultValue?.ToString() ?? string.Empty;
                    var helpText = helpTextAttribute?.HelpText ?? string.Empty;

                    nameWidth = Math.Max(nameWidth, displayName.Length);
                    valueWidth = Math.Max(valueWidth, displayValue.Length);
                    requiredWidth = Math.Max(requiredWidth, required.Length);
                    typeWidth = Math.Max(typeWidth, typeName.Length);
                    descWidth = Math.Max(descWidth, description.Length);
                    defaultWidth = Math.Max(defaultWidth, defaultVal.Length);
                    helpWidth = Math.Max(helpWidth, helpText.Length);

                    rows.Add((displayName, displayValue, required, typeName, description, defaultVal, helpText));
                }

                // 重新基于“显示宽度”计算各列宽度，中文字符按双列宽
                int hdName = GetDisplayWidth(OptionHeader);
                int hdValue = GetDisplayWidth(ValueHeader);
                int hdRequired = GetDisplayWidth(RequiredHeader);
                int hdType = GetDisplayWidth(TypeNameHeader);
                int hdDesc = GetDisplayWidth(DescriptionHeader);
                int hdDefault = GetDisplayWidth(DefaultValueHeader);
                int hdHelp = GetDisplayWidth(HelpTextHeader);

                nameWidth = Math.Max(hdName, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Name)) : 0);
                valueWidth = Math.Max(hdValue, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Value)) : 0);
                requiredWidth = Math.Max(hdRequired, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Required)) : 0);
                typeWidth = Math.Max(hdType, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.TypeName)) : 0);
                descWidth = Math.Max(hdDesc, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.Description)) : 0);
                defaultWidth = Math.Max(hdDefault, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.DefaultValue)) : 0);
                helpWidth = Math.Max(hdHelp, rows.Count > 0 ? rows.Max(r => GetDisplayWidth(r.HelpText)) : 0);

                // 限制每列最大宽度，但不得小于表头显示宽度
                int Limit(int width, int max) => Math.Min(width, max);
                int nameMax = Math.Max(24, hdName);
                int valueMax = Math.Max(30, hdValue);
                int requiredMax = Math.Max(2, hdRequired);
                int typeMax = Math.Max(18, hdType);
                int descMax = Math.Max(40, hdDesc);
                int defaultMax = Math.Max(20, hdDefault);
                int helpMax = Math.Max(30, hdHelp);

                nameWidth = Limit(nameWidth, nameMax);
                valueWidth = Limit(valueWidth, valueMax);
                requiredWidth = Limit(requiredWidth, requiredMax);
                typeWidth = Limit(typeWidth, typeMax);
                descWidth = Limit(descWidth, descMax);
                defaultWidth = Limit(defaultWidth, defaultMax);
                helpWidth = Limit(helpWidth, helpMax);

                // 记录各列最小宽度（不得压缩到小于表头显示宽度）
                int minNameWidth = hdName;
                int minValueWidth = hdValue;
                int minRequiredWidth = hdRequired;
                int minTypeWidth = hdType;
                int minDescWidth = hdDesc;
                int minDefaultWidth = hdDefault;
                int minHelpWidth = hdHelp;

                // 根据控制台宽度自适应整体表格宽度，确保整齐对齐
                int columnsCount = 7;
                int CalculateTotalWidth() => nameWidth + valueWidth + requiredWidth + typeWidth + descWidth + defaultWidth + helpWidth + (2 * columnsCount) + (columnsCount + 1);
                int consoleWidth = 0;
                try
                {
                    consoleWidth = Math.Max(60, Math.Min(Console.BufferWidth, Console.WindowWidth));
                }
                catch
                {
                    consoleWidth = 120;
                }

                int maxTableWidth = Math.Max(60, consoleWidth - 1);

                // 计算最终表格宽度，用于上方/下方的双线边框和居中标题
                int tableWidth = CalculateTotalWidth();
                string topDouble = "╔" + new string('═', Math.Max(0, tableWidth - 2)) + "╗";
                string bottomDouble = "╚" + new string('═', Math.Max(0, tableWidth - 2)) + "╝";
                string headerText = "Command-line parameter and parsed configuration object information";
                string centeredHeader = CenterPadDisplay(headerText, Math.Max(0, tableWidth - 2));

                Console.WriteLine(topDouble);
                Console.WriteLine($"║{centeredHeader}║");
                Console.WriteLine(bottomDouble);
                Console.WriteLine();

                while (CalculateTotalWidth() > maxTableWidth)
                {
                    if (descWidth > minDescWidth)
                    {
                        descWidth--;
                        continue;
                    }

                    if (helpWidth > minHelpWidth)
                    {
                        helpWidth--;
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
                    return string.Concat(
                        left,
                        new string(fill, nameWidth + 2), sep,
                        new string(fill, valueWidth + 2), sep,
                        new string(fill, requiredWidth + 2), sep,
                        new string(fill, typeWidth + 2), sep,
                        new string(fill, descWidth + 2), sep,
                        new string(fill, defaultWidth + 2), sep,
                        new string(fill, helpWidth + 2),
                        right
                    );
                }


                // 打印表头
                Console.WriteLine(BuildBorder('┌', '┬', '┐', '─'));
                Console.WriteLine($"│ {TruncPadDisplay(OptionHeader, nameWidth)} │ {TruncPadDisplay(ValueHeader, valueWidth)} │ {CenterPadDisplay(RequiredHeader, requiredWidth)} │ {TruncPadDisplay(TypeNameHeader, typeWidth)} │ {TruncPadDisplay(DescriptionHeader, descWidth)} │ {TruncPadDisplay(DefaultValueHeader, defaultWidth)} │ {TruncPadDisplay(HelpTextHeader, helpWidth)} │");
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
                    var helpLines = WrapToDisplayLines(row.HelpText, helpWidth);

                    int lineCount = new[]
                    {
                        nameLines.Count,
                        valueLines.Count,
                        1,
                        typeLines.Count,
                        descLines.Count,
                        defLines.Count,
                        helpLines.Count
                    }.Max();

                    for (int i = 0; i < lineCount; i++)
                    {
                        string nameLine = i < nameLines.Count ? nameLines[i] : new string(' ', nameWidth);
                        string valueLine = i < valueLines.Count ? valueLines[i] : new string(' ', valueWidth);
                        string reqLine = i == 0 ? reqText : new string(' ', requiredWidth);
                        string typeLine = i < typeLines.Count ? typeLines[i] : new string(' ', typeWidth);
                        string descLine = i < descLines.Count ? descLines[i] : new string(' ', descWidth);
                        string defLine = i < defLines.Count ? defLines[i] : new string(' ', defaultWidth);
                        string helpLine = i < helpLines.Count ? helpLines[i] : new string(' ', helpWidth);

                        Console.WriteLine($"│ {nameLine} │ {valueLine} │ {reqLine} │ {typeLine} │ {descLine} │ {defLine} │ {helpLine} │");
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
                int v = rune.Value;
                bool wide =
                    (v >= 0x4E00 && v <= 0x9FFF) ||
                    (v >= 0x3400 && v <= 0x4DBF) ||
                    (v >= 0x3000 && v <= 0x303F) ||
                    (v >= 0x3040 && v <= 0x309F) ||
                    (v >= 0x30A0 && v <= 0x30FF) ||
                    (v >= 0xAC00 && v <= 0xD7AF) ||
                    (v >= 0xF900 && v <= 0xFAFF) ||
                    (v >= 0xFF01 && v <= 0xFF60) ||
                    (v >= 0xFFE0 && v <= 0xFFE6) ||
                    (v >= 0x1F300 && v <= 0x1FAFF);
                int rw = wide ? 2 : 1;
                if (w + rw > width)
                {
                    if (width > 1) sb.Append('…');
                    break;
                }

                sb.Append(rune.ToString());
                w += rw;
            }

            while (GetDisplayWidth(sb.ToString()) < width) sb.Append(' ');
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
                int v = rune.Value;
                bool wide =
                    (v >= 0x4E00 && v <= 0x9FFF) ||
                    (v >= 0x3400 && v <= 0x4DBF) ||
                    (v >= 0x3000 && v <= 0x303F) ||
                    (v >= 0x3040 && v <= 0x309F) ||
                    (v >= 0x30A0 && v <= 0x30FF) ||
                    (v >= 0xAC00 && v <= 0xD7AF) ||
                    (v >= 0xF900 && v <= 0xFAFF) ||
                    (v >= 0xFF01 && v <= 0xFF60) ||
                    (v >= 0xFFE0 && v <= 0xFFE6) ||
                    (v >= 0x1F300 && v <= 0x1FAFF);
                int rw = wide ? 2 : 1;

                if (w + rw > width)
                {
                    while (GetDisplayWidth(sb.ToString()) < width) sb.Append(' ');
                    lines.Add(sb.ToString());
                    sb.Clear();
                    w = 0;
                }

                sb.Append(rune.ToString());
                w += rw;
            }

            // 收尾：加入最后一行，并填充到指定宽度
            while (GetDisplayWidth(sb.ToString()) < width) sb.Append(' ');
            lines.Add(sb.ToString());
            if (lines.Count == 0)
            {
                lines.Add(new string(' ', width));
            }

            return lines;
        }

        // 显示宽度相关函数：中文及全角字符按双列宽处理
        static int GetDisplayWidth(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return 0;
            }

            int w = 0;
            foreach (var rune in s.EnumerateRunes())
            {
                int v = rune.Value;
                bool wide =
                    (v >= 0x4E00 && v <= 0x9FFF) || // CJK Unified Ideographs
                    (v >= 0x3400 && v <= 0x4DBF) || // CJK Ext A
                    (v >= 0x3000 && v <= 0x303F) || // CJK Symbols & Punctuation
                    (v >= 0x3040 && v <= 0x309F) || // Hiragana
                    (v >= 0x30A0 && v <= 0x30FF) || // Katakana
                    (v >= 0xAC00 && v <= 0xD7AF) || // Hangul Syllables
                    (v >= 0xF900 && v <= 0xFAFF) || // CJK Compatibility Ideographs
                    (v >= 0xFF01 && v <= 0xFF60) || // Fullwidth ASCII variants
                    (v >= 0xFFE0 && v <= 0xFFE6) || // Fullwidth symbols
                    (v >= 0x1F300 && v <= 0x1FAFF); // Emoji & Symbols (approx)
                w += wide ? 2 : 1;
            }

            return w;
        }

        /// <summary>
        /// 格式化属性值用于显示
        /// </summary>
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
                for (int i = 0; i < Math.Min(array.Length, 5); i++)
                {
                    elements.Add(array.GetValue(i)?.ToString() ?? "null");
                }

                var result = $"[{string.Join(", ", elements)}]";
                if (array.Length > 5)
                {
                    result += $" (共{array.Length}个元素)";
                }

                return result;
            }

            if (value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (System.Collections.IList)value;
                var elements = new List<string>();
                for (int i = 0; i < Math.Min(list.Count, 5); i++)
                {
                    elements.Add(list[i]?.ToString() ?? "null");
                }

                var result = $"[{string.Join(", ", elements)}]";
                if (list.Count > 5)
                {
                    result += $" (共{list.Count}个元素)";
                }

                return result;
            }

            return value.ToString();
        }

        /// <summary>
        /// 获取友好的类型名称
        /// </summary>
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
                return $"Array of {GetFriendlyTypeName(type.GetElementType())} ";
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
