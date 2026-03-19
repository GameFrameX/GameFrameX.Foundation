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

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 已经存在参数重复异常。
/// 用于表示当参数值已经存在或重复时抛出的异常。
/// </summary>
/// <remarks>
/// Exception thrown when an argument value already exists or is duplicated.
/// </remarks>
public sealed class ArgumentAlreadyException : Exception
{
    /// <summary>
    /// 已经存在参数重复异常构造函数。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ArgumentAlreadyException"/> class with a specified error message.
    /// </remarks>
    /// <param name="message">异常消息，描述具体的参数重复问题，不能为 null、空字符串或仅包含空白字符 / The message that describes the error, cannot be null, empty, or whitespace.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="message"/> 为 null 时抛出 / Thrown when <paramref name="message"/> is null.</exception>
    /// <exception cref="ArgumentException">当 <paramref name="message"/> 为空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="message"/> is empty or whitespace.</exception>
    public ArgumentAlreadyException(string message) : base(message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
    }

    /// <summary>
    /// 抛出参数重复异常的静态辅助方法。
    /// </summary>
    /// <remarks>
    /// Throws an <see cref="ArgumentAlreadyException"/> with the specified message.
    /// </remarks>
    /// <param name="message">异常消息，描述具体的参数重复问题，不能为 null、空字符串或仅包含空白字符 / The message that describes the error, cannot be null, empty, or whitespace.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="message"/> 为 null 时抛出 / Thrown when <paramref name="message"/> is null.</exception>
    /// <exception cref="ArgumentException">当 <paramref name="message"/> 为空字符串或仅包含空白字符时抛出 / Thrown when <paramref name="message"/> is empty or whitespace.</exception>
    /// <exception cref="ArgumentAlreadyException">当调用此方法时总是抛出此异常 / Always thrown when this method is called.</exception>
    public static void Throw(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        throw new ArgumentAlreadyException(message);
    }
}