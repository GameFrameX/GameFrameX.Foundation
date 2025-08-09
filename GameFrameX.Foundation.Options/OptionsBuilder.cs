// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Reflection;
using GameFrameX.Foundation.Options.Attributes;

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项构建器，用于从命令行参数和环境变量构建配置选项
/// </summary>
public class OptionsBuilder
{
    #region 静态便捷方法

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项（使用默认设置）
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的配置选项对象</returns>
    public static TOptions Create<TOptions>(string[] args, bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(args);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项（完整参数控制）
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="boolFormat">布尔参数格式</param>
    /// <param name="ensurePrefixedKeys">是否确保参数键都有前缀</param>
    /// <param name="useEnvironmentVariables">是否使用环境变量</param>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的配置选项对象</returns>
    public static TOptions Create<TOptions>(
        string[] args,
        BoolArgumentFormat boolFormat,
        bool ensurePrefixedKeys = true,
        bool useEnvironmentVariables = true,
        bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(args, boolFormat, ensurePrefixedKeys, useEnvironmentVariables);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项（仅使用命令行参数，不使用环境变量）
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的配置选项对象</returns>
    public static TOptions CreateFromArgsOnly<TOptions>(string[] args, bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(args, useEnvironmentVariables: false);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：从环境变量构建配置选项（不使用命令行参数）
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的配置选项对象</returns>
    public static TOptions CreateFromEnvironmentOnly<TOptions>(bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(Array.Empty<string>(), useEnvironmentVariables: true);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：创建默认配置选项（仅使用默认值，不使用命令行参数和环境变量）
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <returns>构建的配置选项对象</returns>
    public static TOptions CreateDefault<TOptions>() where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(Array.Empty<string>(), useEnvironmentVariables: false);
        return builder.Build(skipValidation: true);
    }

    /// <summary>
    /// 静态方法：尝试从命令行参数构建配置选项，如果失败则返回默认配置
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="result">构建结果</param>
    /// <param name="error">错误信息（如果构建失败）</param>
    /// <returns>是否构建成功</returns>
    public static bool TryCreate<TOptions>(string[] args, out TOptions result, out string error) where TOptions : class, new()
    {
        try
        {
            result = Create<TOptions>(args);
            error = null;
            return true;
        }
        catch (Exception ex)
        {
            result = CreateDefault<TOptions>();
            error = ex.Message;
            return false;
        }
    }

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项并启用调试输出
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的配置选项对象</returns>
    public static TOptions CreateWithDebug<TOptions>(string[] args, bool skipValidation = false) where TOptions : class, new()
    {
        // 先打印调试信息
        OptionsDebugger.PrintStructuredArguments(args, typeof(TOptions));

        // 创建配置选项
        var result = Create<TOptions>(args, skipValidation);

        // 打印解析结果
        OptionsDebugger.PrintParsedOptions(result);

        return result;
    }

    #endregion
}

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
        _converter = new CommandLineArgumentConverter
        {
            BoolFormat = boolFormat,
            EnsurePrefixedKeys = ensurePrefixedKeys
        };
    }

    /// <summary>
    /// 构建选项对象
    /// </summary>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的选项对象</returns>
    public T Build(bool skipValidation = false)
    {
        try
        {
            // 创建默认实例
            var result = Activator.CreateInstance<T>();

            // 应用默认值
            ApplyDefaultValues(result);

            // 处理命令行参数和环境变量
            var options = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            // 添加环境变量（优先级较低）
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
                try
                {
                    // 转换为标准格式
                    var standardArgs = _converter.ConvertToStandardFormat(_args);

                    // 转换为选项字典
                    var argsOptions = ConvertToOptionsDictionary(standardArgs);

                    foreach (var kvp in argsOptions)
                    {
                        options[kvp.Key] = kvp.Value;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"处理命令行参数时发生错误: {ex.Message}", ex);
                }
            }

            // 将选项应用到结果对象
            ApplyOptions(result, options);

            // 验证必需的选项
            if (!skipValidation)
            {
                ValidateRequiredOptions(result);
            }

            return result;
        }
        catch (Exception ex)
        {
            // 发生异常时抛出异常
            throw new ArgumentException($"构建选项时发生错误: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 应用默认值
    /// </summary>
    /// <param name="target">目标对象</param>
    private void ApplyDefaultValues(T target)
    {
        var properties = typeof(T).GetProperties()
                                  .Where(p => p.CanWrite)
                                  .ToList();

        foreach (var property in properties)
        {
            // 检查是否有默认值特性
            var defaultValueAttr = property.GetCustomAttributes<DefaultValueAttribute>().FirstOrDefault();
            if (defaultValueAttr != null && defaultValueAttr.Value != null)
            {
                try
                {
                    // 转换并设置默认值
                    var convertedValue = Convert.ChangeType(defaultValueAttr.Value, property.PropertyType);
                    property.SetValue(target, convertedValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"设置属性 {property.Name} 的默认值时发生错误: {ex.Message}");
                }
            }

            // 检查选项特性中的默认值
            var optionAttrs = property.GetCustomAttributes<OptionAttribute>().ToList();
            foreach (var optionAttr in optionAttrs)
            {
                if (optionAttr != null && optionAttr.DefaultValue != null && defaultValueAttr == null)
                {
                    try
                    {
                        // 转换并设置默认值
                        var convertedValue = Convert.ChangeType(optionAttr.DefaultValue, property.PropertyType);
                        property.SetValue(target, convertedValue);
                        break; // 只应用第一个找到的默认值
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"设置属性 {property.Name} 的默认值时发生错误: {ex.Message}");
                    }
                }
            }

            // 检查是否有标志选项特性
            var flagOptionAttr = property.GetCustomAttributes<FlagOptionAttribute>().FirstOrDefault();
            if (flagOptionAttr != null && property.PropertyType == typeof(bool))
            {
                // 标志选项默认为 false
                property.SetValue(target, false);
            }
        }
    }

    /// <summary>
    /// 验证必需的选项
    /// </summary>
    /// <param name="target">目标对象</param>
    private void ValidateRequiredOptions(T target)
    {
        var properties = typeof(T).GetProperties();
        var missingOptions = new List<string>();

        foreach (var property in properties)
        {
            bool isRequired = false;
            string optionName = property.Name.ToLowerInvariant().Replace("_", "-");

            // 检查是否有必需选项特性
            var requiredOptionAttrs = property.GetCustomAttributes<RequiredOptionAttribute>().ToList();
            foreach (var requiredOptionAttr in requiredOptionAttrs)
            {
                if (requiredOptionAttr.Required)
                {
                    isRequired = true;
                    if (!string.IsNullOrEmpty(requiredOptionAttr.LongName))
                    {
                        optionName = requiredOptionAttr.LongName;
                    }

                    break;
                }
            }

            // 检查选项特性中的必需标志
            if (!isRequired)
            {
                var optionAttrs = property.GetCustomAttributes<OptionAttribute>().ToList();
                foreach (var optionAttr in optionAttrs)
                {
                    if (optionAttr.Required && !(optionAttr is RequiredOptionAttribute))
                    {
                        isRequired = true;
                        if (!string.IsNullOrEmpty(optionAttr.LongName))
                        {
                            optionName = optionAttr.LongName;
                        }

                        break;
                    }
                }
            }

            // 如果是必需的，检查值
            if (isRequired)
            {
                var value = property.GetValue(target);
                if (value == null || (value is string strValue && string.IsNullOrEmpty(strValue)))
                {
                    missingOptions.Add(optionName);
                }
            }
        }

        if (missingOptions.Count > 0)
        {
            throw new ArgumentException($"缺少必需的选项: {string.Join(", ", missingOptions)}");
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
            var properties = typeof(T).GetProperties();
            var envVarMappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            // 收集环境变量映射
            foreach (var property in properties)
            {
                var envVarAttrs = property.GetCustomAttributes<EnvironmentVariableAttribute>().ToList();
                foreach (var envVarAttr in envVarAttrs)
                {
                    if (envVarAttr != null && !string.IsNullOrEmpty(envVarAttr.Name))
                    {
                        envVarMappings[envVarAttr.Name] = property;
                    }
                }

                var optionAttrs = property.GetCustomAttributes<OptionAttribute>().ToList();
                foreach (var optionAttr in optionAttrs)
                {
                    if (!string.IsNullOrEmpty(optionAttr.EnvironmentVariable))
                    {
                        envVarMappings[optionAttr.EnvironmentVariable] = property;
                    }
                }
            }

            // 处理环境变量
            foreach (var key in envVars.Keys)
            {
                if (key == null) continue;

                var keyStr = key.ToString();
                var value = envVars[key]?.ToString();

                if (!string.IsNullOrEmpty(value))
                {
                    // 检查是否有映射
                    if (envVarMappings.TryGetValue(keyStr, out var property))
                    {
                        // 处理布尔值
                        if (property.PropertyType == typeof(bool) && IsBooleanValue(value))
                        {
                            result[property.Name] = ParseBooleanValue(value);
                        }
                        else
                        {
                            result[property.Name] = value;
                        }
                    }
                    else
                    {
                        // 尝试直接匹配属性名
                        var normalizedKey = NormalizePropertyName(keyStr);
                        var matchedProperty = properties.FirstOrDefault(p =>
                                                                            string.Equals(p.Name, normalizedKey, StringComparison.OrdinalIgnoreCase));

                        if (matchedProperty != null)
                        {
                            // 处理布尔值
                            if (matchedProperty.PropertyType == typeof(bool) && IsBooleanValue(value))
                            {
                                result[matchedProperty.Name] = ParseBooleanValue(value);
                            }
                            else
                            {
                                result[matchedProperty.Name] = value;
                            }
                        }
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
    /// 将标准格式参数转换为选项字典
    /// </summary>
    /// <param name="standardArgs">标准格式参数列表</param>
    /// <returns>选项字典</returns>
    private Dictionary<string, object> ConvertToOptionsDictionary(List<string> standardArgs)
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < standardArgs.Count; i++)
        {
            var arg = standardArgs[i];

            if (string.IsNullOrEmpty(arg))
            {
                continue;
            }

            // 处理键值对格式 (--key=value)
            if (arg.Contains("="))
            {
                var parts = arg.Split(new[] { '=' }, 2);
                var key = NormalizeKey(parts[0]);
                var value = parts[1];

                result[key] = value;
                continue;
            }

            // 处理分离格式 (--key value)
            if (arg.StartsWith("-"))
            {
                var key = NormalizeKey(arg);

                // 检查是否有值
                if (i < standardArgs.Count - 1 && !standardArgs[i + 1].StartsWith("-"))
                {
                    var value = standardArgs[i + 1];
                    // 如果值为null，不添加到字典中，这样会使用默认值
                    if (value != null)
                    {
                        result[key] = value;
                    }

                    // 如果值为null，不添加键值对，让属性保持默认值
                    i++; // 跳过已处理的值
                }
                else
                {
                    // 检查这个键是否对应布尔属性
                    if (IsBooleanProperty(key))
                    {
                        // 布尔标志，没有值
                        result[key] = true;
                    }
                    // 对于非布尔属性，如果没有值就不添加到字典中，使用默认值
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 检查指定的键是否对应布尔属性
    /// </summary>
    /// <param name="key">参数键</param>
    /// <returns>如果对应布尔属性则返回true，否则返回false</returns>
    private bool IsBooleanProperty(string key)
    {
        var properties = typeof(T).GetProperties();
        var optionMappings = GetOptionMappings();

        // 首先尝试通过选项映射查找属性
        if (optionMappings.TryGetValue(key, out var propertyName))
        {
            var property = properties.FirstOrDefault(p =>
                                                         string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
            return property?.PropertyType == typeof(bool) || property?.PropertyType == typeof(bool?);
        }

        // 如果没有找到，尝试标准化键名查找
        string normalizedKey = NormalizePropertyName(key);
        var matchedProperty = properties.FirstOrDefault(p =>
                                                            string.Equals(p.Name, normalizedKey, StringComparison.OrdinalIgnoreCase));

        return matchedProperty?.PropertyType == typeof(bool) || matchedProperty?.PropertyType == typeof(bool?);
    }

    /// <summary>
    /// 获取选项映射
    /// </summary>
    /// <returns>选项映射字典</returns>
    private Dictionary<string, string> GetOptionMappings()
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            // 处理所有 OptionAttribute 及其派生类
            var optionAttrs = property.GetCustomAttributes(typeof(OptionAttribute), true)
                                      .Cast<OptionAttribute>()
                                      .ToList();

            foreach (var optionAttr in optionAttrs)
            {
                // 添加长名称映射
                if (!string.IsNullOrEmpty(optionAttr.LongName))
                {
                    result[optionAttr.LongName] = property.Name;
                }

                // 添加短名称映射
                if (optionAttr.HasShortName)
                {
                    result[optionAttr.ShortName.ToString()] = property.Name;
                }
            }

            // 默认使用属性名作为选项名
            result[property.Name.ToLowerInvariant()] = property.Name;
            result[property.Name.ToLowerInvariant().Replace("_", "-")] = property.Name;
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

        // 获取选项映射
        var optionMappings = GetOptionMappings();

        foreach (var kvp in options)
        {
            PropertyInfo property = null;

            // 首先尝试通过选项映射查找属性
            if (optionMappings.TryGetValue(kvp.Key, out var propertyName))
            {
                property = properties.FirstOrDefault(p =>
                                                         string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
            }

            // 如果没有找到，尝试标准化键名查找
            if (property == null)
            {
                string normalizedKey = NormalizePropertyName(kvp.Key);
                property = properties.FirstOrDefault(p =>
                                                         string.Equals(p.Name, normalizedKey, StringComparison.OrdinalIgnoreCase));
            }

            if (property != null)
            {
                try
                {
                    // 如果值为 null，跳过设置，保持默认值
                    if (kvp.Value == null)
                    {
                        continue;
                    }

                    // 获取字符串值
                    string stringValue = kvp.Value.ToString();

                    // 根据目标类型进行转换
                    object convertedValue = null;

                    if (property.PropertyType == typeof(string))
                    {
                        // 对于字符串类型，即使是空字符串也要设置
                        convertedValue = stringValue;
                    }
                    else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                    {
                        if (!string.IsNullOrEmpty(stringValue) && int.TryParse(stringValue, out int intValue))
                        {
                            convertedValue = intValue;
                        }
                    }
                    else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                    {
                        // 处理布尔值
                        if (kvp.Value is bool boolValue)
                        {
                            convertedValue = boolValue;
                        }
                        else if (string.IsNullOrEmpty(stringValue))
                        {
                            // 空字符串对于布尔值跳过，保持默认值
                            continue;
                        }
                        else if (stringValue.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = true;
                        }
                        else if (stringValue.Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = false;
                        }
                        else if (stringValue.Equals("1") || stringValue.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                                 stringValue.Equals("on", StringComparison.OrdinalIgnoreCase))
                        {
                            convertedValue = true;
                        }
                        else if (stringValue.Equals("0") || stringValue.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                                 stringValue.Equals("off", StringComparison.OrdinalIgnoreCase))
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
                        if (!string.IsNullOrEmpty(stringValue) && double.TryParse(stringValue, out double doubleValue))
                        {
                            convertedValue = doubleValue;
                        }
                    }
                    else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
                    {
                        if (!string.IsNullOrEmpty(stringValue) && float.TryParse(stringValue, out float floatValue))
                        {
                            convertedValue = floatValue;
                        }
                    }
                    else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        if (!string.IsNullOrEmpty(stringValue) && decimal.TryParse(stringValue, out decimal decimalValue))
                        {
                            convertedValue = decimalValue;
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        if (!string.IsNullOrEmpty(stringValue) && DateTime.TryParse(stringValue, out DateTime dateTimeValue))
                        {
                            convertedValue = dateTimeValue;
                        }
                    }
                    else if (property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
                    {
                        if (!string.IsNullOrEmpty(stringValue) && Guid.TryParse(stringValue, out Guid guidValue))
                        {
                            convertedValue = guidValue;
                        }
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        if (!string.IsNullOrEmpty(stringValue))
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
                    }
                    else
                    {
                        // 尝试使用 Convert 类进行转换
                        if (!string.IsNullOrEmpty(stringValue))
                        {
                            try
                            {
                                convertedValue = Convert.ChangeType(stringValue, property.PropertyType);
                            }
                            catch
                            {
                                // 转换失败，使用默认值
                            }
                        }
                    }

                    // 如果转换成功，设置属性值
                    if (convertedValue != null || property.PropertyType == typeof(string))
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