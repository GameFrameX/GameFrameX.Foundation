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

using System.Collections.Concurrent;
using System.Reflection;
using GameFrameX.Foundation.Options.Attributes;

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项构建器，用于从命令行参数和环境变量构建配置选项。
/// </summary>
/// <remarks>
/// Options builder for creating configuration options from command-line arguments and environment variables.
/// This class provides static convenience methods for creating options objects.
/// </remarks>
public class OptionsBuilder
{
    #region 静态便捷方法

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项（使用默认设置）。
    /// </summary>
    /// <remarks>
    /// Static method: Creates configuration options from command-line arguments using default settings.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="skipValidation">是否跳过必需选项验证 / Whether to skip required option validation</param>
    /// <returns>构建的配置选项对象 / Built configuration options object</returns>
    public static TOptions Create<TOptions>(string[] args, bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(args ?? Array.Empty<string>());
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项（完整参数控制）。
    /// </summary>
    /// <remarks>
    /// Static method: Creates configuration options from command-line arguments with full parameter control.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="boolFormat">布尔参数格式 / Boolean argument format</param>
    /// <param name="ensurePrefixedKeys">是否确保参数键都有前缀 / Whether to ensure all argument keys have prefixes</param>
    /// <param name="useEnvironmentVariables">是否使用环境变量 / Whether to use environment variables</param>
    /// <param name="skipValidation">是否跳过必需选项验证 / Whether to skip required option validation</param>
    /// <returns>构建的配置选项对象 / Built configuration options object</returns>
    public static TOptions Create<TOptions>(
        string[] args,
        BoolArgumentFormat boolFormat,
        bool ensurePrefixedKeys = true,
        bool useEnvironmentVariables = true,
        bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(args ?? Array.Empty<string>(), boolFormat, ensurePrefixedKeys, useEnvironmentVariables);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：从命令行参数构建配置选项（仅使用命令行参数，不使用环境变量）。
    /// </summary>
    /// <remarks>
    /// Static method: Creates configuration options from command-line arguments only, without using environment variables.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="skipValidation">是否跳过必需选项验证 / Whether to skip required option validation</param>
    /// <returns>构建的配置选项对象 / Built configuration options object</returns>
    public static TOptions CreateFromArgsOnly<TOptions>(string[] args, bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(args ?? Array.Empty<string>(), useEnvironmentVariables: false);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：从环境变量构建配置选项（不使用命令行参数）。
    /// </summary>
    /// <remarks>
    /// Static method: Creates configuration options from environment variables only, without using command-line arguments.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <param name="skipValidation">是否跳过必需选项验证 / Whether to skip required option validation</param>
    /// <returns>构建的配置选项对象 / Built configuration options object</returns>
    public static TOptions CreateFromEnvironmentOnly<TOptions>(bool skipValidation = false) where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(Array.Empty<string>(), useEnvironmentVariables: true);
        return builder.Build(skipValidation);
    }

    /// <summary>
    /// 静态方法：创建默认配置选项（仅使用默认值，不使用命令行参数和环境变量）。
    /// </summary>
    /// <remarks>
    /// Static method: Creates default configuration options using only default values, without command-line arguments or environment variables.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <returns>构建的配置选项对象 / Built configuration options object</returns>
    public static TOptions CreateDefault<TOptions>() where TOptions : class, new()
    {
        var builder = new OptionsBuilder<TOptions>(Array.Empty<string>(), useEnvironmentVariables: false);
        return builder.Build(skipValidation: true);
    }

    /// <summary>
    /// 静态方法：尝试从命令行参数构建配置选项，如果失败则返回默认配置。
    /// </summary>
    /// <remarks>
    /// Static method: Attempts to create configuration options from command-line arguments, returns default configuration if failed.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="result">构建结果 / Build result</param>
    /// <param name="error">错误信息（如果构建失败） / Error message (if build failed)</param>
    /// <returns>是否构建成功 / Whether the build was successful</returns>
    public static bool TryCreate<TOptions>(string[] args, out TOptions result, out string error) where TOptions : class, new()
    {
        try
        {
            result = Create<TOptions>(args ?? Array.Empty<string>());
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
    /// 静态方法：从命令行参数构建配置选项并启用调试输出。
    /// </summary>
    /// <remarks>
    /// Static method: Creates configuration options from command-line arguments with debug output enabled.
    /// </remarks>
    /// <typeparam name="TOptions">配置选项类型 / Configuration options type</typeparam>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="skipValidation">是否跳过必需选项验证 / Whether to skip required option validation</param>
    /// <returns>构建的配置选项对象 / Built configuration options object</returns>
    public static TOptions CreateWithDebug<TOptions>(string[] args, bool skipValidation = false) where TOptions : class, new()
    {
        // 创建配置选项
        var result = Create<TOptions>(args ?? Array.Empty<string>(), skipValidation);

        // 打印解析结果
        OptionsDebugger.PrintParsedOptions(result);

        return result;
    }

    #endregion
}

/// <summary>
/// 选项构建器，用于从命令行参数和环境变量构建配置选项。
/// </summary>
/// <remarks>
/// Generic options builder for creating configuration options from command-line arguments and environment variables.
/// This class provides functionality to build configuration options with caching support for property information.
/// </remarks>
/// <typeparam name="T">配置选项类型 / Configuration options type</typeparam>
public sealed class OptionsBuilder<T> where T : class, new()
{
    /// <summary>
    /// 反射结果缓存，用于缓存类型的属性信息。
    /// </summary>
    /// <remarks>
    /// Reflection result cache for caching type property information.
    /// </remarks>
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();

    private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> OptionMappingsCache = new();

    /// <summary>
    /// 获取指定类型的所有属性（带缓存）。
    /// </summary>
    /// <remarks>
    /// Gets all properties of the specified type with caching support.
    /// </remarks>
    /// <param name="type">要获取属性的类型 / The type to get properties for</param>
    /// <returns>属性信息数组 / Array of property information</returns>
    private static PropertyInfo[] GetCachedProperties(Type type)
    {
        return PropertyCache.GetOrAdd(type, t => t.GetProperties());
    }

    private readonly string[] _args;
    private readonly bool _useEnvironmentVariables;
    private readonly CommandLineArgumentConverter _converter;

    /// <summary>
    /// 初始化选项构建器。
    /// </summary>
    /// <remarks>
    /// Initializes the options builder with the specified parameters.
    /// </remarks>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <param name="boolFormat">布尔参数格式 / Boolean argument format</param>
    /// <param name="ensurePrefixedKeys">是否确保参数键都有前缀 / Whether to ensure all argument keys have prefixes</param>
    /// <param name="useEnvironmentVariables">是否使用环境变量 / Whether to use environment variables</param>
    public OptionsBuilder(string[] args, BoolArgumentFormat boolFormat = BoolArgumentFormat.Flag, bool ensurePrefixedKeys = true, bool useEnvironmentVariables = true)
    {
        _args = args ?? Array.Empty<string>();
        _useEnvironmentVariables = useEnvironmentVariables;
        _converter = new CommandLineArgumentConverter
        {
            BoolFormat = boolFormat,
            EnsurePrefixedKeys = ensurePrefixedKeys
        };
    }

    /// <summary>
    /// 构建选项对象。
    /// </summary>
    /// <remarks>
    /// Builds the options object from command-line arguments and environment variables.
    /// </remarks>
    /// <param name="skipValidation">是否跳过必需选项验证 / Whether to skip required option validation</param>
    /// <returns>构建的选项对象 / Built options object</returns>
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
                    throw new ArgumentException($"处理命令行参数时发生错误 (An error occurred while processing command-line arguments): {ex.Message}", ex);
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
            throw new ArgumentException($"构建选项时发生错误 (An error occurred while building options): {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 应用默认值。
    /// </summary>
    /// <remarks>
    /// Applies default values from OptionAttribute to the target object.
    /// </remarks>
    /// <param name="target">目标对象 / Target object</param>
    private void ApplyDefaultValues(T target)
    {
        var properties = GetCachedProperties(typeof(T))
                         .Where(p => p.CanWrite)
                         .ToList();

        foreach (var property in properties)
        {
            // 检查选项特性中的默认值
            var optionAttrs = property.GetCustomAttributes<OptionAttribute>().ToList();
            foreach (var optionAttr in optionAttrs)
            {
                // 命中默认值且成功设置后，只应用第一个找到的默认值即跳出
                if (optionAttr.DefaultValue != null && TrySetPropertyValue(property, target, optionAttr.DefaultValue))
                {
                    break;
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
    /// 尝试将默认值转换并设置到目标属性。
    /// </summary>
    /// <remarks>
    /// Attempts to convert and set the default value to the target property.
    /// Returns false (and keeps the property's default state) when the conversion fails.
    /// </remarks>
    /// <param name="property">目标属性 / Target property</param>
    /// <param name="target">目标对象 / Target object</param>
    /// <param name="defaultValue">默认值 / Default value</param>
    /// <returns>成功设置返回 true；转换失败返回 false / true if set successfully; false if conversion failed</returns>
    private bool TrySetPropertyValue(PropertyInfo property, T target, object defaultValue)
    {
        try
        {
            // 转换并设置默认值
            var convertedValue = Convert.ChangeType(defaultValue, property.PropertyType);
            property.SetValue(target, convertedValue);
            return true;
        }
        catch (InvalidCastException ex)
        {
            // 类型转换失败，保持属性的默认状态
            // Type conversion failed, keep the property's default state
            System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 的默认值时发生类型转换错误: {ex.Message}");
        }
        catch (FormatException ex)
        {
            // 格式转换失败，保持属性的默认状态
            // Format conversion failed, keep the property's default state
            System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 的默认值时发生格式错误: {ex.Message}");
        }
        catch (OverflowException ex)
        {
            // 数值溢出，保持属性的默认状态
            // Numeric overflow, keep the property's default state
            System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 的默认值时发生溢出错误: {ex.Message}");
        }

        return false;
    }

    /// <summary>
    /// 验证必需的选项。
    /// </summary>
    /// <remarks>
    /// Validates that all required options have been set.
    /// </remarks>
    /// <param name="target">目标对象 / Target object</param>
    private void ValidateRequiredOptions(T target)
    {
        var properties = GetCachedProperties(typeof(T));
        var missingOptions = new List<string>();

        foreach (var property in properties)
        {
            bool isRequired = false;
            string optionName = property.Name.ToLowerInvariant().Replace("_", "-");

            // 仅基于 OptionAttribute 的 Required 标志进行校验
            var optionAttrs = property.GetCustomAttributes<OptionAttribute>().ToList();
            foreach (var optionAttr in optionAttrs)
            {
                if (optionAttr.Required)
                {
                    isRequired = true;
                    if (!string.IsNullOrEmpty(optionAttr.LongName))
                    {
                        optionName = optionAttr.LongName;
                    }

                    break;
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
            throw new ArgumentException($"缺少必需的选项 (Missing required options): {string.Join(", ", missingOptions)}");
        }
    }

    /// <summary>
    /// 获取环境变量。
    /// </summary>
    /// <remarks>
    /// Retrieves environment variables and maps them to option properties.
    /// </remarks>
    /// <returns>环境变量字典 / Environment variable dictionary</returns>
    private Dictionary<string, object> GetEnvironmentVariables()
    {
        var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        try
        {
            // 获取所有环境变量
            var envVars = Environment.GetEnvironmentVariables();
            var properties = GetCachedProperties(typeof(T));
            // 收集环境变量映射（显式 EnvironmentVariableAttribute / OptionAttribute.EnvironmentVariable）
            var envVarMappings = BuildEnvironmentVariableMappings(properties);

            // 处理环境变量
            foreach (var key in envVars.Keys)
            {
                if (key == null)
                {
                    continue;
                }

                var keyStr = key.ToString();
                var value = envVars[key]?.ToString();

                // 值为空或 null 时跳过，让属性保持默认值
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                // 优先按显式映射匹配；否则尝试按标准化属性名直接匹配
                if (envVarMappings.TryGetValue(keyStr, out var mappedProperty))
                {
                    SetEnvironmentValue(result, mappedProperty, value);
                }
                else
                {
                    var matchedProperty = FindPropertyByNormalizedName(properties, keyStr);
                    if (matchedProperty != null)
                    {
                        SetEnvironmentValue(result, matchedProperty, value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 忽略环境变量获取错误，返回空字典
            // Ignore environment variable retrieval errors, return empty dictionary
            System.Diagnostics.Debug.WriteLine($"获取环境变量时发生错误: {ex.Message}");
        }

        return result;
    }

    /// <summary>
    /// 构建环境变量名到属性的映射。
    /// </summary>
    /// <remarks>
    /// Builds a mapping from environment variable names to option properties, sourced from
    /// <see cref="EnvironmentVariableAttribute"/> names and <see cref="OptionAttribute.EnvironmentVariable"/> values.
    /// </remarks>
    /// <param name="properties">已缓存的目标类型属性 / Cached properties of the target type</param>
    /// <returns>环境变量名到属性的映射（OrdinalIgnoreCase）/ Mapping from env var names to properties</returns>
    private Dictionary<string, PropertyInfo> BuildEnvironmentVariableMappings(PropertyInfo[] properties)
    {
        var mappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in properties)
        {
            var envVarAttrs = property.GetCustomAttributes<EnvironmentVariableAttribute>().ToList();
            foreach (var envVarAttr in envVarAttrs)
            {
                if (!string.IsNullOrEmpty(envVarAttr.Name))
                {
                    mappings[envVarAttr.Name] = property;
                }
            }

            var optionAttrs = property.GetCustomAttributes<OptionAttribute>().ToList();
            foreach (var optionAttr in optionAttrs)
            {
                if (!string.IsNullOrEmpty(optionAttr.EnvironmentVariable))
                {
                    mappings[optionAttr.EnvironmentVariable] = property;
                }
            }
        }

        return mappings;
    }

    /// <summary>
    /// 按标准化属性名匹配属性。
    /// </summary>
    /// <remarks>
    /// Normalizes the environment variable key and matches it against property names (ordinal ignore-case).
    /// Returns null when the normalized key is empty or no property matches.
    /// </remarks>
    /// <param name="properties">已缓存的目标类型属性 / Cached properties of the target type</param>
    /// <param name="key">环境变量键 / Environment variable key</param>
    /// <returns>匹配到的属性；未匹配返回 null / Matched property, or null if not found</returns>
    private PropertyInfo FindPropertyByNormalizedName(PropertyInfo[] properties, string key)
    {
        var normalizedKey = NormalizePropertyName(key);
        if (string.IsNullOrEmpty(normalizedKey))
        {
            return null;
        }

        return properties.FirstOrDefault(p => string.Equals(p.Name, normalizedKey, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// 将环境变量值写入结果字典。
    /// </summary>
    /// <remarks>
    /// Writes the environment variable value to the result dictionary, parsing it as a boolean for
    /// boolean properties whose value is a recognized boolean literal.
    /// </remarks>
    /// <param name="result">结果字典 / Result dictionary</param>
    /// <param name="property">目标属性 / Target property</param>
    /// <param name="value">环境变量值 / Environment variable value</param>
    private static void SetEnvironmentValue(Dictionary<string, object> result, PropertyInfo property, string value)
    {
        // 处理布尔值
        if (property.PropertyType == typeof(bool) && BooleanParser.IsBooleanValue(value))
        {
            result[property.Name] = BooleanParser.ParseBooleanValue(value);
        }
        else
        {
            result[property.Name] = value;
        }
    }

    /// <summary>
    /// 将标准格式参数转换为选项字典。
    /// </summary>
    /// <remarks>
    /// Converts standard format arguments to an options dictionary.
    /// </remarks>
    /// <param name="standardArgs">标准格式参数列表 / Standard format argument list</param>
    /// <returns>选项字典 / Options dictionary</returns>
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
            if (IsOptionToken(arg))
            {
                var key = NormalizeKey(arg);

                // 检查是否有值
                if (i < standardArgs.Count - 1 && !IsOptionToken(standardArgs[i + 1]))
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

    private static bool IsOptionToken(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        if (!value.StartsWith("-"))
        {
            return false;
        }

        return !decimal.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var parsed) || parsed >= 0;
    }

    /// <summary>
    /// 检查指定的键是否对应布尔属性。
    /// </summary>
    /// <remarks>
    /// Checks if the specified key corresponds to a boolean property.
    /// </remarks>
    /// <param name="key">参数键 / Argument key</param>
    /// <returns>如果对应布尔属性则返回 <c>true</c>；否则返回 <c>false</c> / Returns <c>true</c> if corresponds to a boolean property; otherwise <c>false</c></returns>
    private bool IsBooleanProperty(string key)
    {
        var properties = GetCachedProperties(typeof(T));
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
    /// 获取选项映射（带缓存）。
    /// </summary>
    /// <remarks>
    /// Gets option mappings with caching support.
    /// </remarks>
    /// <returns>选项映射字典 / Option mapping dictionary</returns>
    private Dictionary<string, string> GetOptionMappings()
    {
        return OptionMappingsCache.GetOrAdd(typeof(T), _ =>
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var properties = GetCachedProperties(typeof(T));

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
                }

                // 默认使用属性名作为选项名
                result[property.Name.ToLowerInvariant()] = property.Name;
                result[property.Name.ToLowerInvariant().Replace("_", "-")] = property.Name;
            }

            return result;
        });
    }

    /// <summary>
    /// 标准化参数键。
    /// </summary>
    /// <remarks>
    /// Normalizes the argument key by removing prefixes.
    /// </remarks>
    /// <param name="key">原始参数键 / Original argument key</param>
    /// <returns>标准化后的键 / Normalized key</returns>
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
    /// 将选项应用到配置对象。
    /// </summary>
    /// <remarks>
    /// Applies options from the dictionary to the configuration object.
    /// </remarks>
    /// <param name="target">目标配置对象 / Target configuration object</param>
    /// <param name="options">选项字典 / Options dictionary</param>
    private void ApplyOptions(T target, Dictionary<string, object> options)
    {
        var properties = GetCachedProperties(typeof(T))
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
                    if (kvp.Value == null)
                    {
                        continue;
                    }

                    var convertedValue = ConvertOptionValue(property, kvp.Value);
                    property.SetValue(target, convertedValue);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"选项 {kvp.Key} 的值 '{kvp.Value}' 无法应用到属性 {property.Name}: {ex.Message}", ex);
                }
                catch (TargetInvocationException ex)
                {
                    throw new ArgumentException($"选项 {kvp.Key} 的值 '{kvp.Value}' 无法应用到属性 {property.Name}: {ex.InnerException?.Message ?? ex.Message}", ex);
                }
                catch (FormatException ex)
                {
                    throw new ArgumentException($"选项 {kvp.Key} 的值 '{kvp.Value}' 无法转换为 {property.PropertyType.Name}: {ex.Message}", ex);
                }
                catch (InvalidCastException ex)
                {
                    throw new ArgumentException($"选项 {kvp.Key} 的值 '{kvp.Value}' 无法转换为 {property.PropertyType.Name}: {ex.Message}", ex);
                }
                catch (OverflowException ex)
                {
                    throw new ArgumentException($"选项 {kvp.Key} 的值 '{kvp.Value}' 超出 {property.PropertyType.Name} 的范围: {ex.Message}", ex);
                }
            }
        }
    }

    private static object ConvertOptionValue(PropertyInfo property, object value)
    {
        if (property.PropertyType == typeof(string))
        {
            return value.ToString();
        }

        var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        var stringValue = value.ToString();

        if (string.IsNullOrEmpty(stringValue))
        {
            throw new FormatException("非字符串选项不能使用空值。");
        }

        if (targetType == typeof(bool))
        {
            if (value is bool boolValue)
            {
                return boolValue;
            }

            if (BooleanParser.IsBooleanValue(stringValue))
            {
                return BooleanParser.ParseBooleanValue(stringValue);
            }

            throw new FormatException("布尔值必须是 true/false、1/0、yes/no 或 on/off。");
        }

        if (targetType.IsEnum)
        {
            return Enum.Parse(targetType, stringValue, true);
        }

        if (targetType == typeof(Guid))
        {
            return Guid.Parse(stringValue);
        }

        return Convert.ChangeType(stringValue, targetType, System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 将参数键标准化为属性名。
    /// </summary>
    /// <remarks>
    /// Normalizes the argument key to a property name by removing prefixes and converting hyphens/underscores to PascalCase.
    /// </remarks>
    /// <param name="key">参数键 / Argument key</param>
    /// <returns>标准化后的属性名 / Normalized property name</returns>
    private string NormalizePropertyName(string key)
    {
        // 检查输入参数
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        // 移除前缀
        if (key.StartsWith("--"))
        {
            key = key.Substring(2);
        }
        else if (key.StartsWith("-"))
        {
            key = key.Substring(1);
        }

        // 再次检查处理后的key是否为空
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        // 处理连字符和下划线
        if (key.Contains("-") || key.Contains("_"))
        {
            var parts = key.Split(new[] { '-', '_' }, StringSplitOptions.RemoveEmptyEntries);

            // 如果分割后没有有效部分，返回空字符串
            if (parts.Length == 0)
            {
                return string.Empty;
            }

            var sb = new System.Text.StringBuilder(parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {
                    sb.Append(char.ToUpperInvariant(parts[i][0]));
                    if (parts[i].Length > 1)
                    {
                        sb.Append(parts[i].Substring(1));
                    }
                }
            }

            return sb.ToString();
        }

        return key;
    }
}