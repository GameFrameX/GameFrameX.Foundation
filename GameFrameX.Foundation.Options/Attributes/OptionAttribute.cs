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

namespace GameFrameX.Foundation.Options.Attributes;

/// <summary>
/// 表示一个命令行选项的特性。
/// </summary>
/// <remarks>
/// Attribute representing a command-line option.
/// Apply this attribute to properties to mark them as command-line options.
/// </remarks>
/// <example>
/// <code>
/// public class MyOptions
/// {
///     [Option("server", Required = true, Description = "Server address")]
///     public string Server { get; set; }
///
///     [Option("port", DefaultValue = 8080)]
///     public int Port { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property)]
[System.Diagnostics.DebuggerDisplay("LongName = {LongName}, Required = {Required}, DefaultValue = {DefaultValue}")]
public class OptionAttribute : Attribute
{
    /// <summary>
    /// 获取或设置选项的长名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the long name of the option.
    /// </remarks>
    /// <value>选项的长名称（如 "verbose"） / Long name of the option (e.g., "verbose")</value>
    public string LongName { get; set; }

    /// <summary>
    /// 获取或设置选项的默认值。
    /// </summary>
    /// <remarks>
    /// Gets or sets the default value of the option.
    /// </remarks>
    /// <value>选项的默认值 / Default value of the option</value>
    public object DefaultValue { get; set; }

    /// <summary>
    /// 获取或设置选项的描述。
    /// </summary>
    /// <remarks>
    /// Gets or sets the description of the option.
    /// </remarks>
    /// <value>选项的描述信息 / Description of the option</value>
    public string Description { get; set; }

    /// <summary>
    /// 获取或设置选项是否必需。
    /// </summary>
    /// <remarks>
    /// Gets or sets whether the option is required.
    /// </remarks>
    /// <value>如果为 <c>true</c> 则选项必需；否则为可选 / <c>true</c> if the option is required; otherwise optional</value>
    public bool Required { get; set; }

    /// <summary>
    /// 获取或设置环境变量名称。
    /// </summary>
    /// <remarks>
    /// Gets or sets the environment variable name for this option.
    /// </remarks>
    /// <value>关联的环境变量名称 / Associated environment variable name</value>
    public string EnvironmentVariable { get; set; }

    /// <summary>
    /// 初始化 <see cref="OptionAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class.
    /// </remarks>
    public OptionAttribute()
    {
    }

    /// <summary>
    /// 使用指定的长名称初始化 <see cref="OptionAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class with the specified long name.
    /// </remarks>
    /// <param name="longName">选项的长名称 / Long name of the option</param>
    public OptionAttribute(string longName)
    {
        LongName = longName;
    }
}
