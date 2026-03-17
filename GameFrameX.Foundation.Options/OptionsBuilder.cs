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
        var builder = new OptionsBuilder<TOptions>(args ?? Array.Empty<string>());
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
        var builder = new OptionsBuilder<TOptions>(args ?? Array.Empty<string>(), boolFormat, ensurePrefixedKeys, useEnvironmentVariables);
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
        var builder = new OptionsBuilder<TOptions>(args ?? Array.Empty<string>(), useEnvironmentVariables: false);
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
    /// 静态方法：从命令行参数构建配置选项并启用调试输出
    /// </summary>
    /// <typeparam name="TOptions">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="skipValidation">是否跳过必需选项验证</param>
    /// <returns>构建的配置选项对象</returns>
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
/// 选项构建器，用于从命令行参数和环境变量构建配置选项
/// </summary>
/// <typeparam name="T">配置选项类型</typeparam>
/// <remarks>
/// 该类提供了从命令行参数和环境变量构建配置选项的功能
/// </remarks>
public sealed class OptionsBuilder<T> where T : class, new()
{
    /// <summary>
    /// 反射结果缓存，用于缓存类型的属性信息
    /// Reflection result cache for caching type property information
    /// </summary>
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> PropertyCache = new();
    private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> OptionMappingsCache = new();

    /// <summary>
    /// 获取指定类型的所有属性（带缓存）
    /// Gets all properties of the specified type (with caching)
    /// </summary>
    /// <param name="type">要获取属性的类型 / The type to get properties for</param>
    /// <returns>属性信息数组 / Array of property information</returns>
    private static PropertyInfo[] GetCachedProperties(Type type)
    {
        return PropertyCache.GetOrAdd(type, t => t.GetProperties());
    }

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
        _args = args ?? Array.Empty<string>();
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
    /// 应用默认值
    /// </summary>
    /// <param name="target">目标对象</param>
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
                if (optionAttr.DefaultValue != null)
                {
                    try
                    {
                        // 转换并设置默认值
                        var convertedValue = Convert.ChangeType(optionAttr.DefaultValue, property.PropertyType);
                        property.SetValue(target, convertedValue);
                        break; // 只应用第一个找到的默认值
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
            var properties = GetCachedProperties(typeof(T));
            var envVarMappings = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            // 收集环境变量映射
            foreach (var property in properties)
            {
                var envVarAttrs = property.GetCustomAttributes<EnvironmentVariableAttribute>().ToList();
                foreach (var envVarAttr in envVarAttrs)
                {
                    if (!string.IsNullOrEmpty(envVarAttr.Name))
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
                if (key == null)
                {
                    continue;
                }

                var keyStr = key.ToString();
                var value = envVars[key]?.ToString();

                // 即使值为空也要处理，因为空值可能是有意义的
                // 检查是否有映射
                if (envVarMappings.TryGetValue(keyStr, out var property))
                {
                    // 如果值为空或null，跳过处理，让属性保持默认值
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

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
                else
                {
                    // 如果值为空或null，跳过处理
                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    // 尝试直接匹配属性名
                    var normalizedKey = NormalizePropertyName(keyStr);

                    // 如果标准化后的键为空，跳过处理
                    if (string.IsNullOrEmpty(normalizedKey))
                    {
                        continue;
                    }

                    var matchedProperty = properties.FirstOrDefault(p =>
                                                                        string.Equals(p.Name, normalizedKey, StringComparison.OrdinalIgnoreCase));

                    if (matchedProperty != null)
                    {
                        // 处理布尔值
                        if (matchedProperty.PropertyType == typeof(bool) && BooleanParser.IsBooleanValue(value))
                        {
                            result[matchedProperty.Name] = BooleanParser.ParseBooleanValue(value);
                        }
                        else
                        {
                            result[matchedProperty.Name] = value;
                        }
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
    /// 获取选项映射（带缓存）
    /// Gets option mappings (with caching)
    /// </summary>
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
                    // 如果值为 null，跳过设置，保持默认值
                    if (kvp.Value == null)
                    {
                        continue;
                    }

                    // 获取字符串值
                    string stringValue = kvp.Value?.ToString();

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
                            catch (FormatException)
                            {
                                // 转换失败，使用默认值
                                // Conversion failed, use default value
                                System.Diagnostics.Debug.WriteLine($"属性 {property.Name} 的值 '{stringValue}' 转换为 {property.PropertyType.Name} 失败");
                            }
                            catch (InvalidCastException)
                            {
                                // 类型转换失败，使用默认值
                                // Type conversion failed, use default value
                                System.Diagnostics.Debug.WriteLine($"属性 {property.Name} 无法将值 '{stringValue}' 转换为 {property.PropertyType.Name}");
                            }
                        }
                    }

                    // 如果转换成功，设置属性值
                    if (convertedValue != null || property.PropertyType == typeof(string))
                    {
                        property.SetValue(target, convertedValue);
                    }
                }
                catch (ArgumentException ex)
                {
                    // 属性设置参数错误，保持属性的默认状态
                    // Property setting argument error, keep the property's default state
                    System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 时发生参数错误: {ex.Message}");
                }
                catch (TargetInvocationException ex)
                {
                    // 属性设置调用错误，保持属性的默认状态
                    // Property setting invocation error, keep the property's default state
                    System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 时发生调用错误: {ex.InnerException?.Message ?? ex.Message}");
                }
                catch (FormatException ex)
                {
                    // 格式转换错误，保持属性的默认状态
                    // Format conversion error, keep the property's default state
                    System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 时发生格式错误: {ex.Message}");
                }
                catch (InvalidCastException ex)
                {
                    // 类型转换错误，保持属性的默认状态
                    // Type conversion error, keep the property's default state
                    System.Diagnostics.Debug.WriteLine($"设置属性 {property.Name} 时发生类型转换错误: {ex.Message}");
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
