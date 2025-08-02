using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

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
            Console.WriteLine("║                    命令行参数解析调试信息                      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // 打印原始参数
            PrintRawArguments(args);
            
            // 打印可用选项定义
            PrintAvailableOptions(optionsType);
            
            // 打印参数解析映射
            PrintArgumentMapping(args);
            
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
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
            Console.WriteLine("║                    解析后的配置对象信息                        ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // 使用反射获取所有属性
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                Console.WriteLine($"配置类型: {typeof(T).Name}");
                Console.WriteLine($"属性数量: {properties.Length}");
                Console.WriteLine();

                // 打印每个属性的值
                foreach (var property in properties.OrderBy(p => p.Name))
                {
                    try
                    {
                        var value = property.GetValue(options);
                        var displayValue = FormatPropertyValue(value);
                        var propertyType = property.PropertyType;
                        
                        Console.WriteLine($"  {property.Name,-20} : {displayValue,-30} ({GetFriendlyTypeName(propertyType)})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"  {property.Name,-20} : <获取值时出错: {ex.Message}>");
                    }
                }

                Console.WriteLine();
                
                // 尝试序列化为JSON格式显示
                PrintJsonRepresentation(options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"打印配置对象时出错: {ex.Message}");
            }

            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine();
        }

        /// <summary>
        /// 打印原始命令行参数
        /// </summary>
        private static void PrintRawArguments(string[] args)
        {
            Console.WriteLine("📋 原始命令行参数:");
            Console.WriteLine($"   参数数量: {args.Length}");
            
            if (args.Length == 0)
            {
                Console.WriteLine("   (无参数)");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine($"   [{i}] {args[i]}");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// 打印可用的选项定义
        /// </summary>
        private static void PrintAvailableOptions(Type optionsType)
        {
            Console.WriteLine("⚙️  可用选项定义:");
            
            var properties = optionsType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var property in properties.OrderBy(p => p.Name))
            {
                var attributes = property.GetCustomAttributes(true);
                var optionAttribute = attributes.OfType<Attributes.OptionAttribute>().FirstOrDefault();
                
                if (optionAttribute != null)
                {
                    var shortName = optionAttribute.HasShortName ? optionAttribute.ShortName.ToString() : "";
                    var longName = !string.IsNullOrEmpty(optionAttribute.LongName) ? optionAttribute.LongName : property.Name.ToLower();
                    
                    Console.WriteLine($"   --{longName,-15} {(optionAttribute.HasShortName ? $"(-{shortName})" : "")} : {optionAttribute.Description ?? "无描述"}");
                    Console.WriteLine($"     类型: {GetFriendlyTypeName(property.PropertyType)}, 必需: {optionAttribute.Required}");
                    
                    if (optionAttribute.DefaultValue != null)
                    {
                        Console.WriteLine($"     默认值: {optionAttribute.DefaultValue}");
                    }
                }
                else
                {
                    Console.WriteLine($"   {property.Name,-20} : (无选项特性)");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 打印参数映射关系
        /// </summary>
        private static void PrintArgumentMapping(string[] args)
        {
            Console.WriteLine("🔗 参数映射分析:");
            
            var mappings = new List<string>();
            var unrecognized = new List<string>();
            
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                
                if (arg.StartsWith("--"))
                {
                    var optionName = arg.Substring(2);
                    var value = i + 1 < args.Length && !args[i + 1].StartsWith("-") ? args[i + 1] : "<无值>";
                    mappings.Add($"   {arg} → {optionName} = {value}");
                    if (value != "<无值>") i++; // 跳过值参数
                }
                else if (arg.StartsWith("-"))
                {
                    var optionName = arg.Substring(1);
                    var value = i + 1 < args.Length && !args[i + 1].StartsWith("-") ? args[i + 1] : "<无值>";
                    mappings.Add($"   {arg} → {optionName} = {value}");
                    if (value != "<无值>") i++; // 跳过值参数
                }
                else
                {
                    unrecognized.Add(arg);
                }
            }
            
            if (mappings.Any())
            {
                Console.WriteLine("   识别的选项:");
                mappings.ForEach(Console.WriteLine);
            }
            
            if (unrecognized.Any())
            {
                Console.WriteLine("   未识别的参数:");
                unrecognized.ForEach(arg => Console.WriteLine($"   ⚠️  {arg}"));
            }
            
            if (!mappings.Any() && !unrecognized.Any())
            {
                Console.WriteLine("   (无参数需要映射)");
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// 格式化属性值用于显示
        /// </summary>
        private static string FormatPropertyValue(object value)
        {
            if (value == null)
                return "<null>";
                
            if (value is string str)
                return $"\"{str}\"";
                
            if (value is bool)
                return value.ToString().ToLower();
                
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
                    result += $" (共{array.Length}个元素)";
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
                    result += $" (共{list.Count}个元素)";
                return result;
            }
            
            return value.ToString();
        }

        /// <summary>
        /// 获取友好的类型名称
        /// </summary>
        private static string GetFriendlyTypeName(Type type)
        {
            if (type == typeof(string)) return "字符串";
            if (type == typeof(int)) return "整数";
            if (type == typeof(bool)) return "布尔值";
            if (type == typeof(double)) return "浮点数";
            if (type == typeof(float)) return "单精度浮点数";
            if (type == typeof(long)) return "长整数";
            if (type == typeof(DateTime)) return "日期时间";
            
            if (type.IsArray)
                return $"{GetFriendlyTypeName(type.GetElementType())}数组";
                
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return $"{GetFriendlyTypeName(type.GetGenericArguments()[0])}列表";
                
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return $"可空{GetFriendlyTypeName(type.GetGenericArguments()[0])}";
            
            return type.Name;
        }

        /// <summary>
        /// 打印JSON格式的对象表示
        /// </summary>
        private static void PrintJsonRepresentation<T>(T options)
        {
            try
            {
                Console.WriteLine("📄 JSON格式表示:");
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                
                var json = JsonSerializer.Serialize(options, jsonOptions);
                Console.WriteLine(json);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   无法序列化为JSON: {ex.Message}");
                Console.WriteLine();
            }
        }
    }
}