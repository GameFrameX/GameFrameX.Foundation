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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Logger;

/// <summary>
/// 控制台日志显示辅助类
/// </summary>
/// <remarks>
/// <para>
/// 提供了一系列方法用于在控制台中以美观的框架格式显示标题、配置信息和分隔线。
/// 支持自定义框架宽度，并能够自动处理文本居中对齐和字符宽度计算。
/// </para>
/// <para>
/// 主要功能包括：
/// <list type="bullet">
/// <item><description>显示带双线边框的大标题（支持主标题和最多两个子标题）</description></item>
/// <item><description>显示带标题的配置信息框</description></item>
/// <item><description>显示分隔线（可选择包含标题文本）</description></item>
/// <item><description>自定义框架宽度设置</description></item>
/// <item><description>自动处理中英文混合文本的居中对齐</description></item>
/// </list>
/// </para>
/// <para>
/// 使用示例：
/// <code>
/// var console = new LogConsole();
/// console.SetFrameLength(80);
/// console.ShowMaxTitle("系统启动", "GameFrameX", "v1.0.0");
/// console.ShowOption("配置文件", "config.json");
/// console.ShowLineTitle("初始化完成");
/// </code>
/// </para>
/// </remarks>
internal sealed class LogConsole
{
    /// <summary>
    /// 输出框线的长度，默认为76个字符
    /// </summary>
    private int _frameLength = 76;

    /// <summary>
    /// 设置输出框架的宽度
    /// </summary>
    /// <param name="length">框架宽度，必须大于0。建议设置为终端宽度或合适的显示宽度（如80、100等）</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="length"/> 小于等于0时抛出</exception>
    /// <remarks>
    /// 框架宽度影响所有输出方法的显示效果，包括标题居中对齐和边框绘制。
    /// 设置过小的宽度可能导致文本显示异常，建议最小宽度为20个字符。
    /// </remarks>
    public void SetFrameLength(int length)
    {
        // 验证参数有效性，确保框架宽度为正数
        ArgumentOutOfRangeException.ThrowIfGreaterThan(0, length, nameof(length));
        _frameLength = length;
    }

    /// <summary>
    /// 显示带边框的大标题，支持主标题和最多两个子标题
    /// </summary>
    /// <param name="title">主标题文本，必须提供且不能为null</param>
    /// <param name="title2">第一个子标题文本，可选。如果为空字符串、null或仅包含空白字符则不显示</param>
    /// <param name="title3">第二个子标题文本，可选。如果为空字符串、null或仅包含空白字符则不显示</param>
    /// <remarks>
    /// <para>
    /// 该方法会创建一个带有双线边框（╔═══╗ 和 ╚═══╝）的标题框。
    /// 所有标题文本都会在框架内居中显示，自动处理中英文混合文本的宽度计算。
    /// </para>
    /// <para>
    /// 子标题的显示逻辑：
    /// <list type="number">
    /// <item><description>主标题始终显示</description></item>
    /// <item><description>只有当title2不为空时才显示第一个子标题</description></item>
    /// <item><description>只有当title3不为空时才显示第二个子标题</description></item>
    /// <item><description>子标题按顺序显示，每个标题占用一行</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// 输出后会自动添加一个空行以便与后续内容分隔。
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // 显示单个标题
    /// console.ShowMaxTitle("系统启动");
    /// 
    /// // 显示主标题和一个子标题
    /// console.ShowMaxTitle("GameFrameX", "日志系统");
    /// 
    /// // 显示完整的三行标题
    /// console.ShowMaxTitle("GameFrameX", "日志系统", "v1.0.0");
    /// </code>
    /// </example>
    public void ShowMaxTitle(string title, string title2 = "", string title3 = "")
    {
        // 生成顶部边框字符串，长度为框架宽度减去左右边框字符
        string character = '═'.RepeatChar(_frameLength - 2);
        Console.WriteLine($"╔{character}╗");
        WriteTitle(title);

        // 检查并显示第一个子标题
        if (title2.IsNotNullOrEmptyOrWhiteSpace())
        {
            WriteTitle(title2);
        }

        // 检查并显示第二个子标题
        if (title3.IsNotNullOrEmptyOrWhiteSpace())
        {
            WriteTitle(title3);
        }

        // 输出底部边框并添加空行分隔
        Console.WriteLine($"╚{character}╝");
        Console.WriteLine();
    }

    /// <summary>
    /// 在框架内居中显示标题文本
    /// </summary>
    /// <param name="title">要显示的标题文本，不能为null</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="title"/> 为null时抛出</exception>
    /// <remarks>
    /// <para>
    /// 该方法会自动计算文本的显示宽度（考虑中英文字符的不同宽度），
    /// 然后在当前设置的框架宽度内进行居中对齐。
    /// </para>
    /// <para>
    /// 居中算法：
    /// <list type="number">
    /// <item><description>计算文本的实际显示宽度</description></item>
    /// <item><description>计算剩余空间并平均分配到左右两侧</description></item>
    /// <item><description>如果剩余空间为奇数，在文本后添加一个空格以保持对称</description></item>
    /// <item><description>使用║字符作为左右边框</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    private void WriteTitle(string title)
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        // 获取文本的实际显示宽度（中文字符宽度为2，英文字符宽度为1）
        var stringWidth = title.GetDisplayWidth();
        var remaining = _frameLength - stringWidth - 2;
        var surplus = remaining % 2;
        // 如果剩余空间为奇数，在文本后添加一个空格以保持左右对称
        if (surplus > 0)
        {
            title += " ";
        }

        remaining /= 2;
        string padding = ' '.RepeatChar(remaining);
        Console.WriteLine($"║{padding}{title}{padding}║");
    }

    /// <summary>
    /// 显示带标题的配置信息框
    /// </summary>
    /// <param name="title">配置项标题，不能为null</param>
    /// <param name="content">配置内容，将调用ToString()方法显示，不能为null</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="title"/> 或 <paramref name="content"/> 为null时抛出</exception>
    /// <remarks>
    /// <para>
    /// 该方法会创建一个带有标题的配置信息显示框，格式如下：
    /// <code>
    /// ╔═══配置标题═══╗
    /// 配置内容
    /// ╚═════════════╝
    /// </code>
    /// </para>
    /// <para>
    /// 标题会在顶部边框中居中显示，内容直接输出在框架内部。
    /// 输出后会自动添加一个空行以便与后续内容分隔。
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// console.ShowOption("数据库连接", "Server=localhost;Database=GameFrameX");
    /// console.ShowOption("日志级别", LogLevel.Information);
    /// </code>
    /// </example>
    public void ShowOption(string title, object content)
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        // 计算标题的显示宽度并生成居中的顶部边框
        int stringWidth = title.GetDisplayWidth();
        int remaining = _frameLength - stringWidth - 2;
        remaining /= 2;
        string padding = '═'.RepeatChar(remaining);
        // 生成底部边框，使用完整的等号字符填充
        string character = '═'.RepeatChar(_frameLength - 2);
        Console.WriteLine($"╔{padding}{title}{padding}╗");
        Console.WriteLine(content);
        Console.WriteLine($"╚{character}╝");
        Console.WriteLine();
    }

    /// <summary>
    /// 显示分隔线，可选择是否包含标题文本
    /// </summary>
    /// <param name="title">分隔线中的标题文本，为空时显示纯分隔线。不能为null</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="title"/> 为null时抛出</exception>
    /// <remarks>
    /// <para>
    /// 该方法用于在控制台输出中创建视觉分隔效果，支持两种模式：
    /// </para>
    /// <para>
    /// 1. 纯分隔线模式（title为空字符串）：
    /// <code>
    /// ═══════════════════════════════════════
    /// </code>
    /// </para>
    /// <para>
    /// 2. 带标题的分隔线模式：
    /// <code>
    /// ═══════════标题文本═══════════
    /// </code>
    /// </para>
    /// <para>
    /// 如果标题文本过长（超出框架宽度-4个字符），会使用简化格式：═══标题═══
    /// 输出后会自动添加一个空行以便与后续内容分隔。
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// // 显示纯分隔线
    /// console.ShowLineTitle("");
    /// 
    /// // 显示带标题的分隔线
    /// console.ShowLineTitle("初始化完成");
    /// console.ShowLineTitle("系统配置");
    /// </code>
    /// </example>
    public void ShowLineTitle(string title = "")
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        var stringWidth = title.GetDisplayWidth();
        // 如果标题过长，使用简单的三等号包围格式
        if (stringWidth - 4 > _frameLength)
        {
            Console.WriteLine($"═══{title}═══");
        }
        else
        {
            // 计算剩余空间并平均分配到标题两侧
            var remaining = _frameLength - stringWidth;
            remaining /= 2;
            var padding = '═'.RepeatChar(remaining);
            // 输出居中的分隔线，标题两侧用等号字符填充
            Console.WriteLine((padding + title + padding));
        }

        // 添加空行以便与后续内容分隔
        Console.WriteLine();
    }
}