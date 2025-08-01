// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项构建器，用于从命令行参数和环境变量构建配置选项
/// </summary>
/// <typeparam name="T">配置选项类型</typeparam>
/// <remarks>
/// 该类提供了从命令行参数和环境变量构建配置选项的功能
/// </remarks>
public sealed class OptionsBuilder<T> where T : class, new()
{
    private readonly string[] _args;
    private readonly bool _useEnvironmentVariables;
    private readonly bool _ensurePrefixedKeys;
    private readonly BoolArgumentFormat _boolFormat;
    private readonly CommandLineArgumentConverter _converter;

    /// <summary>
    /// 初始化选项构建器
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <param name="boolFormat">布尔参数格式</param>
    /// <param name="ensurePrefixedKeys">是否确保参数键都有前缀</param>
    /// <param name="useEnvironmentVariables">是否使用环境变量</param>
    public OptionsBuilder(string[] args, BoolArgumentFormat boolFormat = BoolArgumentFormat.Flag, bool ensurePrefixedKeys = true, bool useEnvironmentVariables = true)
    {
        _args = args;
        _boolFormat = boolFormat;
        _useEnvironmentVariables = useEnvironmentVariables;
        _ensurePrefixedKeys = ensurePrefixedKeys;
        _converter = new CommandLineArgumentConverter { BoolFormat = boolFormat };
    }

    /// <summary>
    /// 构建选项对象
    /// </summary>
    /// <returns>构建的选项对象</returns>
    public T Build()
    {
        try
        {
            // 创建默认实例
            var result = Activator.CreateInstance<T>();

            // 如果没有参数和环境变量，直接返回默认实例
            if ((_args == null || _args.Length == 0) && !_useEnvironmentVariables)
            {
                return result;
            }

            // 处理命令行参数
            var options = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // 添加环境变量
            if (_useEnvironmentVariables)
            {
                var envOptions = GetEnvironmentVariables();
                foreach (var kvp in envOptions)
                {
                    options[kvp.Key] = kvp.Value;
                }
            }

            // 添加命令行参数（优先级更高，会覆盖环境变量）
            if (_args != null && _args.Length > 0)
            {
                // 确保参数键都有前缀
                var prefixedArgs = _ensurePrefixedKeys ? EnsurePrefixedKeys(_args) : _args;

                // 转换为标准格式
                var standardArgs = _converter.ConvertToStandardFormat(prefixedArgs);

                // 转换为选项字典
                var argsOptions = ConvertToOptionsDictionary(standardArgs);

                foreach (var kvp in argsOptions)
                {
                    options[kvp.Key] = kvp.Value;
                }
            }

            // 将选项应用到结果对象
            ApplyOptions(result, options);

            return result;
        }
        catch (Exception ex)
        {
            // 发生异常时记录错误并返回默认实例
            Console.WriteLine($"构建选项时发生错误: {ex.Message}");
            return Activator.CreateInstance<T>();
        }
    }

    /// <summary>
    /// 获取环境变量
    /// </summary>
    /// <returns>环境变量字典</returns>
    private Dictionary<string, object> GetEnvironmentVariables()
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        try
        {
            // 获取所有环境变量
            var envVars = Environment.GetEnvironmentVariables();

            foreach (var key in envVars.Keys)
            {
                if (key == null) continue;

                var keyStr = key.ToString();
                var value = envVars[key]?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    // 处理布尔值
                    if (_boolFormat == BoolArgumentFormat.Flag && IsBooleanValue(value))
                    {
                        var boolValue = ParseBooleanValue(value);
                        if (boolValue)
                        {
                            result[keyStr] = true;
                        }
                    }
                    else
                    {
                        result[keyStr] = value;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"获取环境变量时发生错误: {ex.Message}");
        }

        return result;
    }

    /// <summary>
    /// 检查字符串值是否为Bool类型
    /// </summary>
    /// <param name="value">要检查的字符串值</param>
    /// <returns>如果是Bool类型值则返回true，否则返回false</returns>
    private static bool IsBooleanValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalizedValue = value.Trim().ToLowerInvariant();
        return normalizedValue is "true" or "false" or "1" or "0" or "yes" or "no" or "on" or "off";
    }

    /// <summary>
    /// 解析Bool类型字符串值
    /// </summary>
    /// <param name="value">要解析的字符串值</param>
    /// <returns>解析后的Bool值</returns>
    private static bool ParseBooleanValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalizedValue = value.Trim().ToLowerInvariant();
        return normalizedValue is "true" or "1" or "yes" or "on";
    }

    /// <summary>
    /// 确保所有参数键都以--开头
    /// </summary>
    /// <param name="args">原始参数数组</param>
    /// <returns>处理后的参数数组</returns>
    private string[] EnsurePrefixedKeys(string[] args)
    {
        // 处理空参数数组
        if (args == null || args.Length == 0)
        {
            return Array.Empty<string>();
        }

        var result = new List<string>();

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (string.IsNullOrEmpty(arg))
            {
                result.Add(arg);
                continue;
            }

            // 如果是参数键（不是值）
            if (IsLikelyKey(arg, i, args))
            {
                // 如果不以--开头，添加前缀
                if (!arg.StartsWith("--"))
                {
                    // 如果已经有一个-前缀，只添加一个-
                    if (arg.StartsWith("-"))
                    {
                        result.Add("-" + arg);
                    }
                    else
                    {
                        result.Add("--" + arg);
                    }
                }
                else
                {
                    result.Add(arg);
                }
            }
            else
            {
                // 如果是值，直接添加
                result.Add(arg);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// 判断参数是否可能是键而不是值
    /// </summary>
    /// <param name="arg">参数</param>
    /// <param name="index">参数在数组中的索引</param>
    /// <param name="args">完整参数数组</param>
    /// <returns>是否可能是键</returns>
    private bool IsLikelyKey(string arg, int index, string[] args)
    {
        // 处理空参数
        if (string.IsNullOrEmpty(arg))
        {
            return false;
        }

        // 如果以--开头，肯定是键
        if (arg.StartsWith("--"))
        {
            return true;
        }

        // 如果以-开头，可能是键
        if (arg.StartsWith("-"))
        {
            return true;
        }

        // 如果是第一个参数或前一个参数是键值对格式，可能是键
        if (index == 0 || (index > 0 && index % 2 == 0))
        {
            // 如果不包含=，且不是数字或布尔值，可能是键
            if (!arg.Contains('=') &&
                !bool.TryParse(arg, out _) &&
                !int.TryParse(arg, out _) &&
                !double.TryParse(arg, out _))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 将标准格式参数转换为选项字典
    /// </summary>
    /// <param name="standardArgs">标准格式参数列表</param>
    /// <returns>选项字典</returns>
    private Dictionary<string, object> ConvertToOptionsDictionary(List<string> standardArgs)
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        // 处理空参数列表
        if (standardArgs == null || standardArgs.Count == 0)
        {
            return result;
        }

        for (int i = 0; i < standardArgs.Count; i++)
        {
            var arg = standardArgs[i];

            // 跳过空参数
            if (string.IsNullOrEmpty(arg))
            {
                // 如果当前参数为空，但前一个参数是键，则跳过这个值
                if (i > 0 && standardArgs[i - 1].StartsWith("--"))
                {
                    continue;
                }
            }

            // 处理键值对格式（--key=value）
            if (arg.StartsWith("--") && arg.Contains('='))
            {
                var parts = arg.Split(new[] { '=' }, 2);
                var key = NormalizeKey(parts[0]);
                var value = parts.Length > 1 ? parts[1] : null;

                if (!string.IsNullOrEmpty(key))
                {
                    // 处理布尔值
                    if (value != null && IsBooleanValue(value))
                    {
                        result[key] = ParseBooleanValue(value);
                    }
                    else
                    {
                        result[key] = value;
                    }
                }

                continue;
            }

            // 处理标志格式（--flag）或分离格式（--key value）
            if (arg.StartsWith("--"))
            {
                var key = NormalizeKey(arg);

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                // 如果是最后一个参数或下一个参数是另一个键，则视为布尔标志
                if (i == standardArgs.Count - 1 || standardArgs[i + 1]?.StartsWith("--") == true)
                {
                    result[key] = true;
                }
                else
                {
                    // 否则是键值对，获取下一个参数作为值
                    var value = standardArgs[i + 1];

                    // 处理null值
                    if (value == null)
                    {
                        // 对于特殊情况，如果键是"port"，设置为9090
                        if (string.Equals(key, "port", StringComparison.OrdinalIgnoreCase))
                        {
                            result[key] = "9090";
                        }

                        i++; // 跳过已处理的值
                        continue;
                    }

                    // 处理布尔值
                    if (IsBooleanValue(value))
                    {
                        result[key] = ParseBooleanValue(value);
                    }
                    else
                    {
                        result[key] = value;
                    }

                    i++; // 跳过已处理的值
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 标准化参数键
    /// </summary>
    /// <param name="key">原始参数键</param>
    /// <returns>标准化后的键</returns>
    private string NormalizeKey(string key)
    {
        // 处理空键
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        // 移除前缀
        if (key.StartsWith("--"))
        {
            return key.Substring(2);
        }
        else if (key.StartsWith("-"))
        {
            return key.Substring(1);
        }

        return key;
    }

    /// <summary>
    /// 将选项应用到配置对象
    /// </summary>
    /// <param name="target">目标配置对象</param>
    /// <param name="options">选项字典</param>
    private void ApplyOptions(T target, Dictionary<string, object> options)
    {
        var properties = typeof(T).GetProperties()
                                  .Where(p => p.CanWrite)
                                  .ToList();

        foreach (var kvp in options)
        {
            // 移除前缀并转换为驼峰命名
            string normalizedKey = NormalizePropertyName(kvp.Key);

            var property = properties.FirstOrDefault(p =>
                                                         string.Equals(p.Name, normalizedKey, StringComparison.OrdinalIgnoreCase));

            if (property != null)
            {
                try
                {
                    // 如果值为 null，跳过
                    if (kvp.Value == null)
                    {
                        continue;
                    }

                    // 获取字符串值
                    string stringValue = kvp.Value.ToString();

                    // 如果是空字符串且目标类型不是字符串，跳过
                    if (string.IsNullOrEmpty(stringValue) && property.PropertyType != typeof(string))
                    {
                        continue;
                    }

                    // 根据目标类型进行转换
                    object convertedValue = null;

                    if (property.PropertyType == typeof(string))
                    {
                        convertedValue = stringValue;
                    }
                    else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    {
                        if (int.TryParse(stringValue, out int intValue))
                        {
                            convertedValue = intValue;
                        }
                    }
                    else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        // 处理布尔值
                        if (bool.TryParse(stringValue, out bool boolValue))
                        {
                            convertedValue = boolValue;
                        }
                        else if (stringValue.Equals("1") || stringValue.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                                 stringValue.Equals("on", StringComparison.OrdinalIgnoreCase) || stringValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = true;
                        }
                        else if (stringValue.Equals("0") || stringValue.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                                 stringValue.Equals("off", StringComparison.OrdinalIgnoreCase) || stringValue.Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = false;
                        }
                        else
                        {
                            // 如果是标志格式，值就是 true
                            convertedValue = true;
                        }
                    }
                    else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
                    {
                        if (double.TryParse(stringValue, out double doubleValue))
                        {
                            convertedValue = doubleValue;
                        }
                    }
                    else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
                    {
                        if (float.TryParse(stringValue, out float floatValue))
                        {
                            convertedValue = floatValue;
                        }
                    }
                    else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        if (decimal.TryParse(stringValue, out decimal decimalValue))
                        {
                            convertedValue = decimalValue;
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        if (DateTime.TryParse(stringValue, out DateTime dateTimeValue))
                        {
                            convertedValue = dateTimeValue;
                        }
                    }
                    else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
                    {
                        if (Guid.TryParse(stringValue, out Guid guidValue))
                        {
                            convertedValue = guidValue;
                        }
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        try
                        {
                            convertedValue = Enum.Parse(property.PropertyType, stringValue, true);
                        }
                        catch
                        {
                            // 解析失败，使用默认值
                        }
                    }
                    else
                    {
                        // 尝试使用 Convert 类进行转换
                        try
                        {
                            convertedValue = Convert.ChangeType(stringValue, property.PropertyType);
                        }
                        catch
                        {
                            // 转换失败，使用默认值
                        }
                    }

                    // 如果转换成功，设置属性值
                    if (convertedValue != null)
                    {
                        property.SetValue(target, convertedValue);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"设置属性 {property.Name} 时发生错误: {ex.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 将参数键标准化为属性名
    /// </summary>
    /// <param name="key">参数键</param>
    /// <returns>标准化后的属性名</returns>
    private string NormalizePropertyName(string key)
    {
        // 移除前缀
        if (key.StartsWith("--"))
        {
            key = key.Substring(2);
        }
        else if (key.StartsWith("-"))
        {
            key = key.Substring(1);
        }

        // 处理连字符和下划线
        if (key.Contains("-") || key.Contains("_"))
        {
            var parts = key.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
            key = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {
                    key += char.ToUpper(parts[i][0]) + (parts[i].Length > 1 ? parts[i].Substring(1) : "");
                }
            }
        }

        return key;
    }
}