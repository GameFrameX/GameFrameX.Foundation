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

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 命令行参数转换器
/// </summary>
public sealed class CommandLineArgumentConverter
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
                    // 如果值部分为空（如 --key=），则使用空字符串
                    var value = parts.Length > 1 ? parts[1] : string.Empty;

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
                        if (BoolFormat == BoolArgumentFormat.Flag && BooleanParser.IsBooleanValue(nextArg))
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
            throw new ArgumentException($"处理命令行参数时发生错误 (An error occurred while processing command-line arguments): {ex.Message}", ex);
        }
    }
}
