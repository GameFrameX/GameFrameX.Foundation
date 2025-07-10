// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 已经存在参数重复异常
/// 用于表示当参数值已经存在或重复时抛出的异常
/// </summary>
public sealed class ArgumentAlreadyException : Exception
{
    /// <summary>
    /// 已经存在参数重复异常构造函数
    /// </summary>
    /// <param name="message">异常消息，描述具体的参数重复问题，不能为 null、空字符串或仅包含空白字符</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="message"/> 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 <paramref name="message"/> 为空字符串或仅包含空白字符时抛出</exception>
    public ArgumentAlreadyException(string message) : base(message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
    }

    /// <summary>
    /// 抛出参数重复异常的静态辅助方法
    /// </summary>
    /// <param name="message">异常消息，描述具体的参数重复问题，不能为 null、空字符串或仅包含空白字符</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="message"/> 为 null 时抛出</exception>
    /// <exception cref="ArgumentException">当 <paramref name="message"/> 为空字符串或仅包含空白字符时抛出</exception>
    /// <exception cref="ArgumentAlreadyException">当调用此方法时总是抛出此异常</exception>
    public static void Throw(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));
        throw new ArgumentAlreadyException(message);
    }
}