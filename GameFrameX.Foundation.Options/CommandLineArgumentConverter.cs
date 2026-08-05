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
/// 命令行参数转换器。
/// </summary>
/// <remarks>
/// Command-line argument converter for transforming and standardizing command-line arguments.
/// </remarks>
public sealed class CommandLineArgumentConverter
{
    /// <summary>
    /// 获取或设置布尔参数格式。
    /// </summary>
    /// <remarks>
    /// Gets or sets the boolean argument format.
    /// </remarks>
    /// <value>布尔参数格式 / Boolean argument format</value>
    public BoolArgumentFormat BoolFormat { get; set; } = BoolArgumentFormat.Flag;

    /// <summary>
    /// 获取或设置是否确保键有前缀。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether to ensure all keys have prefixes.
    /// </remarks>
    /// <value>指示是否确保键有前缀，默认为 <c>true</c> / Indicates whether to ensure keys have prefixes, default is <c>true</c></value>
    public bool EnsurePrefixedKeys { get; set; } = true;

    /// <summary>
    /// 将参数列表转换为命令行字符串。
    /// </summary>
    /// <remarks>
    /// Converts a list of arguments to a command-line string.
    /// </remarks>
    /// <param name="args">参数列表 / List of arguments</param>
    /// <returns>格式化的命令行字符串 / Formatted command-line string</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="args"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="args"/> is <c>null</c></exception>
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
            if (IsOptionToken(arg))
            {
                result.Add(arg);

                // 如果不是最后一个参数，且下一个参数不是选项，则消耗它作为值
                i = ConsumeOptionValueIfPresent(result, args, i);
            }
            else
            {
                // 非选项 token：含空格时用双引号包裹
                AppendTokenWithQuotingIfNeeded(result, arg);
            }
        }

        return string.Join(" ", result);
    }

    /// <summary>
    /// 若当前选项 token 后紧跟一个非选项 token，则把它作为值追加并返回新的索引；否则返回原索引。
    /// </summary>
    /// <remarks>
    /// Inspects <c>args[currentIndex + 1]</c>: when the token exists and is not
    /// another option token, appends it via
    /// <see cref="AppendTokenWithQuotingIfNeeded"/> and returns
    /// <c>currentIndex + 1</c> so the caller's <c>for</c> increment lands past
    /// the consumed value. When no token follows, or the next token is itself
    /// an option token, returns <c>currentIndex</c> unchanged.
    /// </remarks>
    /// <param name="result">累积结果列表 / Accumulated result list</param>
    /// <param name="args">完整参数列表 / Full argument list</param>
    /// <param name="currentIndex">当前选项 token 的索引 / Index of the current option token</param>
    /// <returns>消耗下一参数后的索引 / Index after possibly consuming the next argument</returns>
    private int ConsumeOptionValueIfPresent(List<string> result, IReadOnlyList<string> args, int currentIndex)
    {
        if (currentIndex >= args.Count - 1)
        {
            return currentIndex;
        }

        var nextArg = args[currentIndex + 1];

        // 下一个参数是选项时不作为值消耗
        if (IsOptionToken(nextArg))
        {
            return currentIndex;
        }

        AppendTokenWithQuotingIfNeeded(result, nextArg);
        return currentIndex + 1;
    }

    /// <summary>
    /// 将 token 追加到结果列表：含空格的 token 用双引号包裹，否则原样追加。
    /// </summary>
    /// <remarks>
    /// Appends <paramref name="value"/> to <paramref name="result"/>. When
    /// the value contains a space, it is wrapped in double quotes to preserve
    /// the original argument boundary on the rebuilt command line.
    /// </remarks>
    /// <param name="result">累积结果列表 / Accumulated result list</param>
    /// <param name="value">待追加的 token / Token to append</param>
    private static void AppendTokenWithQuotingIfNeeded(List<string> result, string value)
    {
        // 如果值包含空格，添加引号
        if (value.Contains(" "))
        {
            result.Add($"\"{value}\"");
        }
        else
        {
            result.Add(value);
        }
    }

    /// <summary>
    /// 将命令行参数转换为标准格式。
    /// </summary>
    /// <remarks>
    /// Converts command-line arguments to standard format.
    /// </remarks>
    /// <param name="args">命令行参数 / Command-line arguments</param>
    /// <returns>标准格式的参数列表 / Standardized argument list</returns>
    public List<string> ConvertToStandardFormat(string[] args)
    {
        try
        {
            if (args == null || args.Length == 0)
            {
                return new List<string>();
            }

            var result = new List<string>();

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
                    result.Add(ApplyKeyValuePair(arg));
                    continue;
                }

                // 如果是空字符串，需要特殊处理
                if (string.IsNullOrEmpty(arg))
                {
                    AppendEmptyStringAsValueIfPrecededByKey(result);
                    continue;
                }

                // 根据EnsurePrefixedKeys设置处理参数键
                result.Add(NormalizeKey(arg));

                // 检查并消耗下一个参数作为值
                i = AttachValueIfPresent(args, i, result);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"处理命令行参数时发生错误 (An error occurred while processing command-line arguments): {ex.Message}", ex);
        }
    }

    /// <summary>
    /// 处理 \`--key=value\` 形式的参数，自动按 <see cref="EnsurePrefixedKeys"/> 修复键前缀。
    /// </summary>
    /// <remarks>
    /// Handles a \`--key=value\` token: splits at the first <c>=</c>, keeps the
    /// value part intact (empty when missing, e.g. <c>--key=</c>), and prefixes
    /// the key with <c>--</c> when <see cref="EnsurePrefixedKeys"/> is true
    /// and the key has no <c>-</c> prefix yet.
    /// </remarks>
    /// <param name="arg">原始键值对参数 / Raw key-value argument</param>
    /// <returns>规范化后的键值对字符串 / Normalized key-value string</returns>
    private string ApplyKeyValuePair(string arg)
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

        return key + "=" + value;
    }

    /// <summary>
    /// 若当前空字符串前一项是未取值的键（<c>--xxx</c> 且不含 <c>=</c>），则把空串作为它的值追加。
    /// </summary>
    /// <remarks>
    /// When the current token is an empty string and the previously emitted
    /// result is a bare option token (starts with <c>-</c> and contains no
    /// <c>=</c>), appends the empty string as that option's value.
    /// </remarks>
    /// <param name="result">累积结果列表 / Accumulated result list</param>
    private static void AppendEmptyStringAsValueIfPrecededByKey(List<string> result)
    {
        if (result.Count == 0)
        {
            return;
        }

        var last = result[result.Count - 1];
        if (last.StartsWith("-") && !last.Contains("="))
        {
            result.Add(""); // 添加空字符串作为值
        }
    }

    /// <summary>
    /// 根据 <see cref="IsOptionToken"/> 与 <see cref="EnsurePrefixedKeys"/> 生成统一的键字符串。
    /// </summary>
    /// <remarks>
    /// Produces the canonical form of an option key: tokens already starting
    /// with <c>-</c> are returned as-is; non-option tokens get a <c>--</c>
    /// prefix only when <see cref="EnsurePrefixedKeys"/> is true.
    /// </remarks>
    /// <param name="arg">原始参数 / Raw argument</param>
    /// <returns>规范化后的键 / Normalized key</returns>
    private string NormalizeKey(string arg)
    {
        if (IsOptionToken(arg))
        {
            return arg;
        }

        return EnsurePrefixedKeys ? "--" + arg : arg;
    }

    /// <summary>
    /// 若下一个参数存在且适合作为当前键的值，则消耗它并返回新的索引；否则返回原索引。
    /// </summary>
    /// <remarks>
    /// Attempts to consume <c>args[currentIndex + 1]</c> as the value of the
    /// current key. The next token is treated as a value when it is non-null,
    /// not another option token, and (under <see cref="BoolArgumentFormat.Flag"/>)
    /// not a recognized boolean value — in which case the boolean is skipped
    /// instead of being appended. Returns the new <c>currentIndex</c> so the
    /// caller's <c>for</c> increment lands past the consumed token.
    /// </remarks>
    /// <param name="args">完整参数数组 / Full argument array</param>
    /// <param name="currentIndex">当前索引 / Current index</param>
    /// <param name="result">累积结果列表 / Accumulated result list</param>
    /// <returns>消耗下一参数后的索引 / Index after possibly consuming the next argument</returns>
    private int AttachValueIfPresent(string[] args, int currentIndex, List<string> result)
    {
        if (currentIndex >= args.Length - 1)
        {
            return currentIndex;
        }

        var nextArg = args[currentIndex + 1];

        // 如果下一个参数是null，则当前参数被视为布尔标志（没有值）
        if (nextArg == null)
        {
            return currentIndex + 1;
        }

        // 如果下一个参数是选项（以-开头），不作为值消耗
        if (IsOptionToken(nextArg))
        {
            return currentIndex;
        }

        // 对于布尔标志格式，跳过被识别为布尔字面量的值
        if (BoolFormat == BoolArgumentFormat.Flag && BooleanParser.IsBooleanValue(nextArg))
        {
            return currentIndex + 1;
        }

        // 添加为普通值（包括空字符串）
        result.Add(nextArg);
        return currentIndex + 1;
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

        return !IsNegativeNumber(value);
    }

    private static bool IsNegativeNumber(string value)
    {
        return decimal.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var parsed) && parsed < 0;
    }
}