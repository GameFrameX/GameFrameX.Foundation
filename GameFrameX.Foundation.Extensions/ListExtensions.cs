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
/// 提供 List 和 IEnumerable 的扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for List and IEnumerable.
/// </remarks>
public static class ListExtensions
{
    /// <summary>
    /// 异步遍历 List 中的每个元素并执行指定操作。
    /// </summary>
    /// <remarks>
    /// Asynchronically iterates through each element in the list and executes the specified operation.
    /// </remarks>
    /// <typeparam name="T">列表元素类型 / The type of list elements.</typeparam>
    /// <param name="list">要遍历的列表，不能为 <c>null</c> / The list to iterate, cannot be <c>null</c>.</param>
    /// <param name="func">要执行的异步操作，不能为 <c>null</c> / The asynchronous operation to execute, cannot be <c>null</c>.</param>
    /// <returns>表示异步操作的任务 / A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 或 <paramref name="func"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="list"/> or <paramref name="func"/> is <c>null</c>.</exception>
    public static async Task ForEachAsync<T>(this List<T> list, Func<T, Task> func)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        ArgumentNullException.ThrowIfNull(func, nameof(func));

        foreach (var value in list)
        {
            await func(value);
        }
    }

    /// <summary>
    /// 异步遍历 IEnumerable 中的每个元素并执行指定操作。
    /// </summary>
    /// <remarks>
    /// Asynchronously iterates through each element in the IEnumerable and executes the specified operation.
    /// </remarks>
    /// <typeparam name="T">集合元素类型 / The type of collection elements.</typeparam>
    /// <param name="source">要遍历的集合，不能为 <c>null</c> / The collection to iterate, cannot be <c>null</c>.</param>
    /// <param name="action">要执行的异步操作，不能为 <c>null</c> / The asynchronous operation to execute, cannot be <c>null</c>.</param>
    /// <returns>表示异步操作的任务 / A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="action"/> 为 <c>null</c> 时抛出 / Thrown when <paramref name="source"/> or <paramref name="action"/> is <c>null</c>.</exception>
    public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        foreach (var value in source)
        {
            await action(value);
        }
    }
}