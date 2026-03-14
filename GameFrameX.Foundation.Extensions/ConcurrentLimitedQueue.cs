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

using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 定长队列，当队列达到指定长度时，新元素入队会自动移除最旧的元素。
/// </summary>
/// <typeparam name="T">队列中元素的类型。</typeparam>
public class ConcurrentLimitedQueue<T> : ConcurrentQueue<T>
{
    /// <summary>
    /// 初始化一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例，指定队列的最大长度。
    /// </summary>
    /// <param name="limit">队列的最大长度，必须大于0。</param>
    /// <exception cref="ArgumentOutOfRangeException">当 <paramref name="limit"/> 小于或等于0时抛出。</exception>
    public ConcurrentLimitedQueue(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
        Limit = limit;
    }

    /// <summary>
    /// 使用指定的集合初始化一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例，并设置队列的最大长度为集合的元素数量。
    /// </summary>
    /// <param name="list">用于初始化队列的集合，不能为null。</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为null时抛出。</exception>
    public ConcurrentLimitedQueue(IEnumerable<T> list) : base(list ?? throw new ArgumentNullException(nameof(list)))
    {
        Limit = list.Count();
    }

    /// <summary>
    /// 队列的最大长度。
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// 将一个列表隐式转换为 <see cref="ConcurrentLimitedQueue{T}" />。
    /// </summary>
    /// <param name="list">要转换的列表，不能为null。</param>
    /// <returns>一个新的 <see cref="ConcurrentLimitedQueue{T}" /> 实例。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="list"/> 为null时抛出。</exception>
    public static implicit operator ConcurrentLimitedQueue<T>(List<T> list)
    {
        ArgumentNullException.ThrowIfNull(list, nameof(list));
        return new ConcurrentLimitedQueue<T>(list);
    }

    /// <summary>
    /// 将一个元素添加到队列中。如果队列已满，则移除最旧的元素。
    /// </summary>
    /// <param name="item">要添加的元素。</param>
    public new void Enqueue(T item)
    {
        while (Count >= Limit)
        {
            TryDequeue(out _);
        }

        base.Enqueue(item);
    }
}