// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Collections;
using System.Text;

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 命令行参数转换器，用于将命令行参数和环境变量组合成标准的命令行参数格式
/// </summary>
/// <remarks>
/// 该类提供了将环境变量转换为命令行参数格式的功能，支持标准的 "--key value" 格式
/// </remarks>
public sealed class CommandLineArgumentConverter
{
    /// <summary>
    /// Bool类型参数的格式设置，默认为标志格式
    /// </summary>
    /// <value>
    /// 获取或设置Bool类型参数的格式，默认值为 <see cref="BoolArgumentFormat.Flag"/>
    /// </value>
    /// <remarks>
    /// 该属性决定了Bool类型的环境变量如何转换为命令行参数格式
    /// </remarks>
    public BoolArgumentFormat BoolFormat { get; set; } = BoolArgumentFormat.Flag;

    /// <summary>
    /// 将命令行参数和环境变量组合成标准的命令行参数列表
    /// </summary>
    /// <param name="args">原始命令行参数数组</param>
    /// <returns>包含原始参数和环境变量的标准格式参数列表</returns>
    /// <remarks>
    /// 该方法会将环境变量转换为 "--key value" 格式，并与原始命令行参数合并。
    /// 如果环境变量的键已经在原始参数中存在，则不会重复添加。
    /// 环境变量的值中的连字符("-")会被移除以避免解析冲突。
    /// Bool类型的环境变量会根据 <see cref="BoolFormat"/> 属性进行特殊处理。
    /// </remarks>
    /// <example>
    /// <code>
    /// var converter = new CommandLineArgumentConverter();
    /// string[] originalArgs = { "--port", "8080" };
    /// // 假设环境变量中有 SERVER_NAME=MyServer, DEBUG=true
    /// var result = converter.ConvertToStandardFormat(originalArgs);
    /// // 结果可能包含: ["--port", "8080", "--SERVER_NAME", "MyServer", "--DEBUG"]
    /// </code>
    /// </example>
    public List<string> ConvertToStandardFormat(string[] args)
    {
        // 检查参数是否为 null
        ArgumentNullException.ThrowIfNull(args, nameof(args));

        var result = new List<string>(args);
        var environmentVariables = Environment.GetEnvironmentVariables();
        var existingKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // 收集已存在的参数键
        for (int i = 0; i < args.Length; i++)
        {
            if (string.IsNullOrEmpty(args[i]))
            {
                continue;
            }

            if (args[i].StartsWith("--"))
            {
                var key = args[i];
                if (key.Contains('='))
                {
                    key = key.Split('=')[0];
                }

                existingKeys.Add(key);
            }
            else if (args[i].StartsWith("-") && !args[i].StartsWith("--"))
            {
                existingKeys.Add("--" + args[i].Substring(1));
            }
        }

        foreach (DictionaryEntry environmentVariable in environmentVariables)
        {
            if (environmentVariable.Key == null || environmentVariable.Value == null)
            {
                continue;
            }

            var keyString = environmentVariable.Key.ToString();
            var valueString = environmentVariable.Value.ToString();

            if (string.IsNullOrWhiteSpace(keyString) || string.IsNullOrWhiteSpace(valueString))
            {
                continue;
            }

            // 标准化键名格式
            var standardKey = keyString.StartsWith("--") ? keyString : "--" + keyString;

            // 检查是否已存在该键
            if (existingKeys.Contains(standardKey))
            {
                continue;
            }

            // 清理值中的连字符
            var cleanedValue = CleanValue(valueString);

            // 检查是否为Bool类型值
            if (IsBooleanValue(valueString))
            {
                var boolValue = ParseBooleanValue(valueString);
                AddBooleanArgument(result, standardKey, boolValue);
            }
            else
            {
                // 根据BoolFormat决定如何添加参数
                if (BoolFormat == BoolArgumentFormat.KeyValue)
                {
                    result.Add($"{standardKey}={cleanedValue}");
                }
                else
                {
                    result.Add(standardKey);
                    result.Add(cleanedValue);
                }
            }

            existingKeys.Add(standardKey);
        }

        return result;
    }

    /// <summary>
    /// 清理参数值，移除可能导致解析问题的字符
    /// </summary>
    /// <param name="value">原始参数值</param>
    /// <returns>清理后的参数值</returns>
    /// <remarks>
    /// 该方法会移除值中的连字符("-")以避免命令行解析器将其误认为是参数标识符
    /// </remarks>
    private static string CleanValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        var stringBuilder = new StringBuilder(value.Length);
        foreach (char c in value)
        {
            if (c != '-')
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 检查字符串值是否为Bool类型
    /// </summary>
    /// <param name="value">要检查的字符串值</param>
    /// <returns>如果是Bool类型值则返回true，否则返回false</returns>
    /// <remarks>
    /// 该方法识别常见的Bool类型表示：true, false, 1, 0, yes, no, on, off（不区分大小写）
    /// </remarks>
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
    /// <remarks>
    /// 该方法将字符串转换为Bool值：true, 1, yes, on 被视为true；false, 0, no, off 被视为false
    /// </remarks>
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
    /// 根据Bool格式设置添加Bool类型参数
    /// </summary>
    /// <param name="arguments">参数列表</param>
    /// <param name="key">参数键</param>
    /// <param name="value">Bool值</param>
    /// <remarks>
    /// 该方法根据 <see cref="BoolFormat"/> 属性的设置，以不同格式添加Bool类型参数
    /// </remarks>
    private void AddBooleanArgument(List<string> arguments, string key, bool value)
    {
        switch (BoolFormat)
        {
            case BoolArgumentFormat.Flag:
                // 标志格式：只有为true时才添加参数
                if (value)
                {
                    arguments.Add(key);
                }

                break;

            case BoolArgumentFormat.KeyValue:
                // 键值对格式：使用等号连接
                arguments.Add($"{key}={value.ToString().ToLowerInvariant()}");
                break;

            case BoolArgumentFormat.Separated:
                // 分离格式：键和值分开
                arguments.Add(key);
                arguments.Add(value.ToString().ToLowerInvariant());
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(BoolFormat), BoolFormat, "不支持的Bool参数格式");
        }
    }

    /// <summary>
    /// 将参数列表转换为命令行字符串
    /// </summary>
    /// <param name="arguments">参数列表</param>
    /// <returns>格式化的命令行字符串</returns>
    /// <remarks>
    /// 该方法将参数列表转换为可以在命令行中使用的字符串格式，
    /// 包含空格的参数值会被双引号包围
    /// </remarks>
    /// <example>
    /// <code>
    /// var converter = new CommandLineArgumentConverter();
    /// var args = new List&lt;string&gt; { "--port", "8080", "--name", "My Server" };
    /// var commandLine = converter.ToCommandLineString(args);
    /// // 结果: --port 8080 --name "My Server"
    /// </code>
    /// </example>
    public string ToCommandLineString(List<string> arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments, nameof(arguments));
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < arguments.Count; i++)
        {
            if (i > 0)
            {
                stringBuilder.Append(' ');
            }

            var arg = arguments[i];
            if (arg.Contains(' ') && !arg.StartsWith("--"))
            {
                stringBuilder.Append('"').Append(arg).Append('"');
            }
            else
            {
                stringBuilder.Append(arg);
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 获取所有环境变量的键值对
    /// </summary>
    /// <returns>环境变量的字典</returns>
    /// <remarks>
    /// 该方法返回当前进程的所有环境变量，键和值都转换为字符串格式
    /// </remarks>
    public Dictionary<string, string> GetEnvironmentVariables()
    {
        var result = new Dictionary<string, string>();
        var environmentVariables = Environment.GetEnvironmentVariables();

        foreach (DictionaryEntry entry in environmentVariables)
        {
            if (entry.Value != null)
            {
                result[entry.Key.ToString() ?? string.Empty] = entry.Value.ToString();
            }
        }

        return result;
    }
}