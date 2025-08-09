// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 命令行参数转换器
/// </summary>
public class CommandLineArgumentConverter
{
    /// <summary>
    /// 布尔参数格式
    /// </summary>
    public BoolArgumentFormat BoolFormat { get; set; } = BoolArgumentFormat.Flag;

    /// <summary>
    /// 是否确保键有前缀
    /// </summary>
    public bool EnsurePrefixedKeys { get; set; } = true;

    /// <summary>
    /// 将参数列表转换为命令行字符串
    /// </summary>
    /// <param name="args">参数列表</param>
    /// <returns>格式化的命令行字符串</returns>
    /// <exception cref="ArgumentNullException">如果参数列表为 null</exception>
    public string ToCommandLineString(List<string> args)
    {
        if (args == null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        if (args.Count == 0)
        {
            return string.Empty;
        }

        var result = new List<string>();

        for (int i = 0; i < args.Count; i++)
        {
            var arg = args[i];

            // 如果是选项名
            if (arg.StartsWith("-"))
            {
                result.Add(arg);

                // 如果不是最后一个参数，且下一个参数不是选项
                if (i < args.Count - 1 && !args[i + 1].StartsWith("-"))
                {
                    var value = args[i + 1];

                    // 如果值包含空格，添加引号
                    if (value.Contains(" "))
                    {
                        result.Add($"\"{value}\"");
                    }
                    else
                    {
                        result.Add(value);
                    }

                    i++; // 跳过已处理的值
                }
            }
            else
            {
                // 如果值包含空格，添加引号
                if (arg.Contains(" "))
                {
                    result.Add($"\"{arg}\"");
                }
                else
                {
                    result.Add(arg);
                }
            }
        }

        return string.Join(" ", result);
    }

    /// <summary>
    /// 将命令行参数转换为标准格式
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns>标准格式的参数列表</returns>
    public List<string> ConvertToStandardFormat(string[] args)
    {
        try
        {
            if (args == null || args.Length == 0)
            {
                return new List<string>();
            }

            var result = new List<string>();

            // 处理命令行参数
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                
                // 如果参数为null，跳过
                if (arg == null)
                {
                    continue;
                }

                // 处理键值对格式 (--key=value)
                if (arg.Contains("="))
                {
                    var parts = arg.Split(new[] { '=' }, 2);
                    var key = parts[0];
                    var value = parts[1];

                    // 根据EnsurePrefixedKeys设置处理键
                    if (!key.StartsWith("-") && EnsurePrefixedKeys)
                    {
                        key = "--" + key;
                    }

                    result.Add(key + "=" + value);
                    continue;
                }

                // 如果是空字符串，需要特殊处理
                if (string.IsNullOrEmpty(arg))
                {
                    // 如果前一个参数是键，这个空字符串就是它的值
                    if (result.Count > 0 && result[result.Count - 1].StartsWith("-") && !result[result.Count - 1].Contains("="))
                    {
                        result.Add(""); // 添加空字符串作为值
                    }
                    continue;
                }

                // 根据EnsurePrefixedKeys设置处理参数键
                string argKey;
                if (!arg.StartsWith("-"))
                {
                    if (EnsurePrefixedKeys)
                    {
                        argKey = "--" + arg;
                    }
                    else
                    {
                        argKey = arg;
                    }
                }
                else
                {
                    argKey = arg;
                }

                result.Add(argKey);

                // 检查下一个参数
                if (i < args.Length - 1)
                {
                    var nextArg = args[i + 1];
                    
                    // 如果下一个参数是null，则当前参数被视为布尔标志（没有值）
                    if (nextArg == null)
                    {
                        i++; // 跳过null参数
                        // 不添加值，当前参数将被视为布尔标志
                        continue;
                    }
                    
                    // 如果下一个参数不是选项（不以-开头），则作为当前参数的值
                    if (!nextArg.StartsWith("-"))
                    {
                        // 对于布尔标志格式，检查是否为布尔值
                        if (BoolFormat == BoolArgumentFormat.Flag && IsBooleanValue(nextArg))
                        {
                            // 跳过布尔值，因为标志格式不需要显式值
                            i++;
                        }
                        else
                        {
                            // 添加为普通值（包括空字符串）
                            result.Add(nextArg);
                            i++;
                        }
                    }
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"处理命令行参数时发生错误: {ex.Message}", ex);
        }
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
}