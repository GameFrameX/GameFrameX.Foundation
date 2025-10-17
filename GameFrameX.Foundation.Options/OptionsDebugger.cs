using System.Reflection;

namespace GameFrameX.Foundation.Options
{
    /// <summary>
    /// 选项调试器，用于打印和调试命令行参数
    /// </summary>
    public static class OptionsDebugger
    {
        /// <summary>
        /// 打印结构化的命令行参数信息
        /// </summary>
        /// <param name="args">原始命令行参数</param>
        /// <param name="optionsType">选项类型</param>
        public static void PrintStructuredArguments(string[] args, Type optionsType)
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  Command-line parameter parsing debugging information        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // 打印可用选项定义
            PrintAvailableOptions(optionsType);

            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        /// <summary>
        /// 打印解析完成后的选项对象
        /// </summary>
        /// <typeparam name="T">选项类型</typeparam>
        /// <param name="options">解析后的选项对象</param>
        public static void PrintParsedOptions<T>(T options) where T : class
        {
            Console.WriteLine();
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  parsed configuration object information                     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // 使用反射获取所有属性
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Console.WriteLine($"  配置类型: {typeof(T).Name}    属性数量: {properties.Length}");
                Console.WriteLine();

                // 打印每个属性的值
                foreach (var property in properties.OrderBy(p => p.Name))
                {
                    try
                    {
                        var value = property.GetValue(options);
                        var displayValue = FormatPropertyValue(value);
                        var propertyType = property.PropertyType;

                        Console.WriteLine($"  {property.Name,-40} :{GetFriendlyTypeName(propertyType),-10} :{displayValue,-40}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  {property.Name,-40} : <获取值时出错: {ex.Message}>");
                        Console.WriteLine(ex);
                    }
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打印配置对象时出错: {ex.Message}");
                Console.WriteLine(ex);
            }

            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
        }

        /// <summary>
        /// 打印可用的选项定义
        /// </summary>
        private static void PrintAvailableOptions(Type optionsType)
        {
            Console.WriteLine("⚙  available options to define:");

            var properties = optionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

            // 使用计算出的最大宽度进行格式化输出
            foreach (var (property, displayName, optionAttribute, helpTextAttribute) in optionInfos)
            {
                if (optionAttribute != null)
                {
                    var shortName = optionAttribute.HasShortName ? optionAttribute.ShortName.ToString() : "";
                    Console.WriteLine($"   {displayName.PadRight(maxWidth, ' ')} {(optionAttribute.HasShortName ? $"(-{shortName})" : "")} : 必需: {(optionAttribute.Required ? "是" : "否")}, 类型: {GetFriendlyTypeName(property.PropertyType),-20}, 描述: {optionAttribute.Description ?? "无描述"}  {(optionAttribute.DefaultValue != null ? $"默认值: {optionAttribute.DefaultValue}" : string.Empty)}  {(helpTextAttribute != null ? $"帮助文本: {helpTextAttribute.HelpText}" : string.Empty)}");
                }
                else
                {
                    Console.WriteLine($"   {displayName.PadRight(maxWidth, ' ')} : (无选项特性)");
                }
            }
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
                return "字符串";
            }

            if (type == typeof(byte))
            {
                return "字节";
            }

            if (type == typeof(short) || type == typeof(ushort))
            {
                return "短整数";
            }

            if (type == typeof(int) || type == typeof(uint))
            {
                return "整数";
            }

            if (type == typeof(bool))
            {
                return "布尔值";
            }

            if (type == typeof(double))
            {
                return "浮点数";
            }

            if (type == typeof(float))
            {
                return "单精度浮点数";
            }

            if (type == typeof(long) || type == typeof(ulong))
            {
                return "长整数";
            }

            if (type == typeof(DateTime))
            {
                return "日期时间";
            }

            if (type.IsArray)
            {
                return $"{GetFriendlyTypeName(type.GetElementType())}数组";
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return $"{GetFriendlyTypeName(type.GetGenericArguments()[0])}列表";
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"可空{GetFriendlyTypeName(type.GetGenericArguments()[0])}";
            }

            return type.Name;
        }
    }
}