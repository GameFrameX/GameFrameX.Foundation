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

namespace GameFrameX.Foundation.Logger;

/// <summary>
/// 日志帮助类的控制台显示部分
/// </summary>
/// <remarks>
/// 提供了一系列静态方法用于在控制台中显示格式化的标题、配置信息和分隔线。
/// 这是LogHelper类的部分类实现，专门处理控制台输出相关功能。
/// </remarks>
public partial class LogHelper
{
    /// <summary>
    /// 控制台日志显示对象实例，用于处理所有控制台格式化输出
    /// </summary>
    private static readonly LogConsole LogConsoleObject = new();

    /// <summary>
    /// 设置控制台输出框架的宽度
    /// </summary>
    /// <param name="length">框架宽度，必须大于0的整数值</param>
    /// <exception cref="ArgumentNullException">当length为null时抛出</exception>
    /// <remarks>
    /// 此设置会影响所有后续的控制台输出格式，包括标题框、配置信息框和分隔线的宽度。
    /// </remarks>
    public static void SetFrameLength(int length)
    {
        ArgumentNullException.ThrowIfNull(length, nameof(length));
        LogConsoleObject.SetFrameLength(length);
    }

    /// <summary>
    /// 显示带边框的大标题，支持主标题和最多两个子标题
    /// </summary>
    /// <param name="title">主标题文本，不能为null</param>
    /// <param name="title2">第一个子标题文本，可选参数</param>
    /// <param name="title3">第二个子标题文本，可选参数</param>
    /// <exception cref="ArgumentNullException">当任何参数为null时抛出</exception>
    /// <remarks>
    /// 使用双线框字符（╔╗╚╝║）创建美观的标题显示效果。
    /// 子标题只有在非空时才会显示，标题文本会自动居中对齐。
    /// </remarks>
    public static void ShowMaxTitle(string title, string title2 = "", string title3 = "")
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(title2, nameof(title2));
        ArgumentNullException.ThrowIfNull(title3, nameof(title3));
        LogConsoleObject.ShowMaxTitle(title, title2, title3);
    }

    /// <summary>
    /// 显示带标题的配置信息框
    /// </summary>
    /// <param name="title">配置项标题，不能为null</param>
    /// <param name="content">配置内容对象，将调用ToString()方法显示，不能为null</param>
    /// <exception cref="ArgumentNullException">当任何参数为null时抛出</exception>
    /// <remarks>
    /// 创建一个带有标题栏的信息框，标题会嵌入到顶部边框中，内容显示在框内。
    /// 适用于显示系统配置、参数设置等结构化信息。
    /// </remarks>
    public static void ShowOption(string title, object content)
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        ArgumentNullException.ThrowIfNull(content, nameof(content));
        LogConsoleObject.ShowOption(title, content);
    }

    /// <summary>
    /// 显示分隔线，可选择是否包含标题文本
    /// </summary>
    /// <param name="title">分隔线中的标题文本，为空字符串时显示纯分隔线</param>
    /// <exception cref="ArgumentNullException">当title为null时抛出</exception>
    /// <remarks>
    /// 当title为空时，显示纯分隔线；当title有内容时，标题会嵌入到分隔线中间。
    /// 如果标题过长超出框架宽度，会使用简化的三等号包围格式。
    /// 常用于分隔不同的信息区块或标记程序执行的不同阶段。
    /// </remarks>
    public static void ShowLineTitle(string title = "")
    {
        ArgumentNullException.ThrowIfNull(title, nameof(title));
        LogConsoleObject.ShowLineTitle(title);
    }
}