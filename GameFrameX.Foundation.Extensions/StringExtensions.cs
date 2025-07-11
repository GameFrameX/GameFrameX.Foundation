// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Text;
using System.Text.RegularExpressions;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 提供字符串类型的扩展方法
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 将Base64字符串转换为URL安全格式。
    /// </summary>
    /// <param name="value">要处理的Base64字符串。</param>
    /// <returns>URL安全的Base64字符串。</returns>
    /// <exception cref="ArgumentNullException">当value为null时抛出。</exception>
    /// <remarks>
    /// 此方法将Base64字符串中的+和/字符分别替换为-和_字符，并移除填充字符=，使其适用于URL传输。
    /// 符合RFC 4648标准的Base64URL编码规范。
    /// </remarks>
    public static string ToUrlSafeBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return value.Replace("+", "-").Replace("/", "_").TrimEnd('=');
    }

    /// <summary>
    /// 将URL安全的Base64字符串转换回标准Base64格式。
    /// </summary>
    /// <param name="value">要处理的URL安全Base64字符串。</param>
    /// <returns>标准格式的Base64字符串。</returns>
    /// <exception cref="ArgumentNullException">当value为null时抛出。</exception>
    /// <remarks>
    /// 此方法是ToUrlSafeBase64的反向操作，将-和_字符分别替换回+和/字符，并根据需要添加填充字符=。
    /// 用于将URL安全的Base64字符串还原为标准Base64格式。
    /// </remarks>
    public static string FromUrlSafeBase64(this string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        var result = value.Replace("-", "+").Replace("_", "/");
        
        // 添加必要的填充字符
        var padding = 4 - (result.Length % 4);
        if (padding < 4)
        {
            result = result.PadRight(result.Length + padding, '=');
        }
        
        return result;
    }

    /// <summary>
    /// 匹配中文字符的正则表达式。
    /// </summary>
    /// <remarks>
    /// 匹配范围包括基本汉字(0x4e00-0x9fa5),不包括生僻字、异体字等扩展汉字
    /// </remarks>
    private static readonly Regex CnReg = new(@"[\u4e00-\u9fa5]");

    /// <summary>
    /// 重复指定字符指定次数。
    /// </summary>
    /// <param name="c">要重复的字符。</param>
    /// <param name="count">重复次数。</param>
    /// <returns>由指定字符重复指定次数组成的新字符串。</returns>
    /// <exception cref="ArgumentOutOfRangeException">当count为负数时抛出。</exception>
    /// <remarks>
    /// 使用StringBuilder来提高字符串拼接性能
    /// 当count为0时,返回空字符串
    /// </remarks>
    public static string RepeatChar(this char c, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
        
        var stringBuilder = new StringBuilder();
        stringBuilder.Clear();
        for (var i = 0; i < count; i++)
        {
            stringBuilder.Append(c);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// 获取在指定宽度内居中对齐的文本。
    /// </summary>
    /// <param name="text">要居中对齐的文本。</param>
    /// <param name="width">总宽度。</param>
    /// <returns>两侧填充空格的居中对齐文本。</returns>
    /// <exception cref="ArgumentNullException">当text为null时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">当width为负数时抛出。</exception>
    /// <remarks>
    /// 如果指定的宽度小于文本长度，将使用文本长度作为宽度。
    /// 当文本长度为奇数且总宽度为偶数时，右侧的空格数会比左侧少一个。
    /// </remarks>
    public static string CenterAlignedText(this string text, int width)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));
        ArgumentOutOfRangeException.ThrowIfNegative(width, nameof(width));
        
        if (width < text.Length)
        {
            width = text.Length;
        }

        var leftSpaces = (width - text.Length) / 2;
        var rightSpaces = width - text.Length - leftSpaces; // 确保左右空格总数等于width-text.Length
        var paddedText = new string(' ', leftSpaces) + text + new string(' ', rightSpaces);
        return paddedText;
    }

    /// <summary>
    /// 从字符串末尾移除指定字符（如果存在）。
    /// </summary>
    /// <param name="self">要处理的字符串。</param>
    /// <param name="toRemove">要移除的字符。</param>
    /// <returns>移除指定字符后的字符串。如果字符串为null或空，或不以指定字符结尾，则返回原字符串。</returns>
    /// <remarks>
    /// 此方法仅移除字符串末尾的单个指定字符
    /// 如果需要移除多个字符,请使用string类型的RemoveSuffix方法
    /// </remarks>
    public static string RemoveSuffix(this string self, char toRemove)
    {
        return self.IsNullOrEmpty() ? self : self.EndsWith(toRemove) ? self.Substring(0, self.Length - 1) : self;
    }

    /// <summary>
    /// 从字符串末尾移除指定的子字符串（如果存在）。
    /// </summary>
    /// <param name="self">要处理的字符串。</param>
    /// <param name="toRemove">要移除的子字符串。</param>
    /// <returns>移除指定子字符串后的字符串。如果字符串为null或空，或不以指定子字符串结尾，则返回原字符串。</returns>
    /// <remarks>
    /// 此方法可以移除字符串末尾的任意长度子字符串
    /// 如果要移除的子字符串为null或空，将返回原字符串
    /// 区分大小写比较
    /// </remarks>
    public static string RemoveSuffix(this string self, string toRemove)
    {
        if (self.IsNullOrEmpty() || toRemove.IsNullOrEmpty())
        {
            return self;
        }
        
        return self.EndsWith(toRemove) ? self.Substring(0, self.Length - toRemove.Length) : self;
    }

    /// <summary>
    /// 移除字符串中的所有空白字符。
    /// </summary>
    /// <param name="self">要处理的字符串。</param>
    /// <returns>移除所有空白字符后的字符串。如果输入为null或空，则返回原字符串。</returns>
    /// <remarks>
    /// 空白字符包括：空格、制表符、换行符等
    /// 使用LINQ实现，对于大字符串可能存在性能开销
    /// </remarks>
    public static string RemoveWhiteSpace(this string self)
    {
        return self.IsNullOrEmpty() ? self : new string(self.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    /// 检查字符串是否为 null 或空字符串。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>如果字符串为 null 或空字符串，则返回 true；否则返回 false。</returns>
    /// <remarks>
    /// 是string.IsNullOrEmpty的扩展方法版本
    /// 空字符串指长度为0的字符串
    /// </remarks>
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// 检查字符串是否为 null、空字符串或仅包含空白字符。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>如果字符串为 null、空字符串或仅包含空白字符，则返回 true；否则返回 false。</returns>
    /// <remarks>
    /// 组合了IsNullOrEmpty和IsNullOrWhiteSpace的检查
    /// 比单独调用两个方法性能更好
    /// </remarks>
    public static bool IsNullOrEmptyOrWhiteSpace(this string str)
    {
        return str.IsNullOrEmpty() || str.IsNullOrWhiteSpace();
    }

    /// <summary>
    /// 检查字符串是否不为 null、空字符串且不仅包含空白字符。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>如果字符串不为 null、不为空字符串且不仅包含空白字符，则返回 true；否则返回 false。</returns>
    /// <remarks>
    /// 是IsNullOrEmptyOrWhiteSpace的逻辑取反版本
    /// 用于需要确认字符串包含实际内容的场景
    /// </remarks>
    public static bool IsNotNullOrEmptyOrWhiteSpace(this string str)
    {
        return !str.IsNullOrEmptyOrWhiteSpace();
    }

    /// <summary>
    /// 检查字符串是否不为 null 且不为空字符串。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>如果字符串不为 null 且不为空字符串，则返回 true；否则返回 false。</returns>
    /// <remarks>
    /// 是IsNullOrEmpty的逻辑取反版本
    /// 常用于参数验证
    /// </remarks>
    public static bool IsNotNullOrEmpty(this string str)
    {
        return !str.IsNullOrEmpty();
    }

    /// <summary>
    /// 检查字符串是否为 null 或仅包含空白字符。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>如果字符串为 null 或仅包含空白字符，则返回 true；否则返回 false。</returns>
    /// <remarks>
    /// 是string.IsNullOrWhiteSpace的扩展方法版本
    /// 空白字符包括空格、制表符、换行符等
    /// </remarks>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    /// <summary>
    /// 检查字符串是否不为 null 且不仅包含空白字符。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>如果字符串不为 null 且不仅包含空白字符，则返回 true；否则返回 false。</returns>
    /// <remarks>
    /// 是IsNullOrWhiteSpace的逻辑取反版本
    /// 用于需要确认字符串包含非空白字符的场景
    /// 空白字符包括空格、制表符、换行符等
    /// </remarks>
    public static bool IsNotNullOrWhiteSpace(this string str)
    {
        return !str.IsNullOrWhiteSpace();
    }

    /// <summary>
    /// 验证字符串不为null或空，否则抛出异常。
    /// </summary>
    /// <param name="value">要验证的字符串。</param>
    /// <param name="name">异常消息中的参数名称。</param>
    /// <exception cref="ArgumentException">当字符串为null或空时抛出。</exception>
    /// <remarks>
    /// 常用于方法参数验证。
    /// 抛出的异常包含参数名称，便于问题定位。
    /// 异常消息使用英文以保持一致性。
    /// </remarks>
    public static void CheckNotNullOrEmpty(this string value, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(value, name);
    }

    /// <summary>
    /// 验证字符串不为null、空或仅包含空白字符，否则抛出异常。
    /// </summary>
    /// <param name="value">要验证的字符串。</param>
    /// <param name="name">异常消息中的参数名称。</param>
    /// <exception cref="ArgumentException">当字符串为null、空或仅包含空白字符时抛出。</exception>
    /// <remarks>
    /// 比CheckNotNullOrEmpty更严格的验证。
    /// 确保字符串包含至少一个非空白字符。
    /// 常用于需要确保有效输入内容的场景。
    /// </remarks>
    public static void CheckNotNullOrEmptyOrWhiteSpace(this string value, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, name);
    }

    /// <summary>
    /// 将字符串按指定分隔符拆分为整数数组。
    /// </summary>
    /// <param name="str">要拆分的字符串。</param>
    /// <param name="sep">分隔符，默认为 '+'。</param>
    /// <returns>拆分并转换后的整数数组。如果字符串为null或空，则返回空数组。</returns>
    /// <remarks>
    /// 使用int.TryParse进行安全的数字转换
    /// 如果某个部分转换失败，对应位置将保持默认值0
    /// 适用于处理如"1+2+3"这样的数字序列字符串
    /// 返回的是新数组，不会修改原字符串
    /// </remarks>
    public static int[] SplitToIntArray(this string str, char sep = '+')
    {
        if (string.IsNullOrEmpty(str))
        {
            return Array.Empty<int>();
        }

        var arr = str.Split(sep);
        var ret = new int[arr.Length];
        for (var i = 0; i < arr.Length; ++i)
        {
            if (int.TryParse(arr[i], out var t))
            {
                ret[i] = t;
            }
        }

        return ret;
    }

    /// <summary>
    /// 将字符串按两级分隔符拆分为二维整数数组。
    /// </summary>
    /// <param name="str">要拆分的字符串。</param>
    /// <param name="sep1">第一级分隔符，默认为 ';'。</param>
    /// <param name="sep2">第二级分隔符，默认为 '+'。</param>
    /// <returns>拆分并转换后的二维整数数组。如果字符串为null或空，则返回空数组。</returns>
    /// <remarks>
    /// 适用于处理如"1+2;3+4;5+6"这样的二维数字序列字符串
    /// 使用SplitToIntArray进行第二级拆分，保持一致的数字转换逻辑
    /// 如果某行转换失败，对应位置将返回空数组
    /// 返回的是新数组，不会修改原字符串
    /// </remarks>
    public static int[][] SplitTo2IntArray(this string str, char sep1 = ';', char sep2 = '+')
    {
        if (string.IsNullOrEmpty(str))
        {
            return Array.Empty<int[]>();
        }

        var arr = str.Split(sep1);
        if (arr.Length <= 0)
        {
            return Array.Empty<int[]>();
        }

        var ret = new int[arr.Length][];

        for (var i = 0; i < arr.Length; ++i)
        {
            ret[i] = arr[i].SplitToIntArray(sep2);
        }

        return ret;
    }

    /// <summary>
    /// 根据路径创建目录，支持递归创建。
    /// </summary>
    /// <param name="path">目录路径。</param>
    /// <param name="isFile">是否为文件路径，如果为true，则创建文件所在的目录。默认为false。</param>
    /// <exception cref="ArgumentNullException">当path为null时抛出。</exception>
    /// <remarks>
    /// 如果路径不存在，会递归创建所有必需的父目录
    /// 当isFile为true时，会自动获取文件所在目录路径
    /// 支持相对路径和绝对路径
    /// 如果目录已存在，则不会进行任何操作
    /// </remarks>
    public static void CreateAsDirectory(this string path, bool isFile = false)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));
        
        if (isFile)
        {
            path = Path.GetDirectoryName(path);
        }

        if (!Directory.Exists(path))
        {
            CreateAsDirectory(path, true);
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 将驼峰命名法字符串转换为蛇形命名法（下划线分隔的小写形式）。
    /// </summary>
    /// <param name="input">要转换的字符串。</param>
    /// <returns>转换后的蛇形命名法字符串。如果输入为null或空，则返回原字符串。</returns>
    /// <remarks>
    /// 保留字符串中的前导下划线。
    /// 在小写字母或数字后跟大写字母的位置插入下划线。
    /// 最后转换为全小写形式。
    /// 适用于数据库字段命名约定转换。
    /// </remarks>
    public static string ConvertToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var startUnderscores = Regex.Match(input, @"^_+").Value;
        return startUnderscores + Regex.Replace(input.TrimStart('_'), @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    /// <summary>
    /// 从字符串中移除所有中文字符。
    /// </summary>
    /// <param name="self">要处理的字符串。</param>
    /// <returns>移除所有中文字符后的字符串。</returns>
    /// <exception cref="ArgumentNullException">当self为null时抛出。</exception>
    /// <remarks>
    /// 使用预编译的正则表达式以获得更好的性能。
    /// 仅移除基本中文字符（0x4e00-0x9fa5）。
    /// 不移除中文标点符号。
    /// 如果字符串不包含中文字符，将返回原字符串。
    /// </remarks>
    public static string TrimZhCn(this string self)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        return CnReg.Replace(self, string.Empty);
    }

    /// <summary>
    /// 快速比较两个字符串是否相等，从末尾开始比较。
    /// </summary>
    /// <param name="self">当前字符串。</param>
    /// <param name="target">要比较的目标字符串。</param>
    /// <returns>如果两个字符串相等则返回true，否则返回false。</returns>
    /// <exception cref="ArgumentNullException">当self或target为null时抛出。</exception>
    /// <remarks>
    /// 从字符串末尾开始比较，可能在某些场景下更快
    /// 先比较长度可以快速判断不相等的情况
    /// 区分大小写比较
    /// </remarks>
    public static bool EqualsFast(this string self, string target)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        if (self.Length != target.Length)
        {
            return false;
        }

        int ap = self.Length - 1;
        int bp = target.Length - 1;

        while (ap >= 0 && bp >= 0 && self[ap] == target[bp])
        {
            ap--;
            bp--;
        }

        return (bp < 0);
    }

    /// <summary>
    /// 快速检查字符串是否以指定字符串结尾，从末尾开始比较。
    /// </summary>
    /// <param name="self">当前字符串。</param>
    /// <param name="target">要检查的结尾字符串。</param>
    /// <returns>如果字符串以指定字符串结尾则返回true，否则返回false。</returns>
    /// <exception cref="ArgumentNullException">当self或target为null时抛出。</exception>
    /// <remarks>
    /// 从末尾开始比较可以更快发现不匹配
    /// 区分大小写比较
    /// 适用于大量字符串后缀检查的场景
    /// </remarks>
    public static bool EndsWithFast(this string self, string target)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        
        int ap = self.Length - 1;
        int bp = target.Length - 1;

        while (ap >= 0 && bp >= 0 && self[ap] == target[bp])
        {
            ap--;
            bp--;
        }

        return (bp < 0);
    }

    /// <summary>
    /// 快速检查字符串是否以指定字符串开头，从开头开始比较。
    /// </summary>
    /// <param name="self">当前字符串。</param>
    /// <param name="target">要检查的开头字符串。</param>
    /// <returns>如果字符串以指定字符串开头则返回true，否则返回false。</returns>
    /// <exception cref="ArgumentNullException">当self或target为null时抛出。</exception>
    /// <remarks>
    /// 从开头逐字符比较直到发现不匹配
    /// 区分大小写比较
    /// 适用于大量字符串前缀检查的场景
    /// </remarks>
    public static bool StartsWithFast(this string self, string target)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        
        int aLen = self.Length;
        int bLen = target.Length;

        int ap = 0;
        int bp = 0;

        while (ap < aLen && bp < bLen && self[ap] == target[bp])
        {
            ap++;
            bp++;
        }

        return (bp == bLen);
    }
}