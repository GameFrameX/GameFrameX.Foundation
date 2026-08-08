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

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 链路追踪标识（TrackId）的异步上下文存取与生成工具。
/// <para>
/// 基于 <see cref="System.Threading.AsyncLocal{T}"/> 实现跨 <c>await</c> 的上下文流动，
/// 供 HTTP 中间件、统一结果包装器与日志记录器在同一请求链路上共享同一个 TrackId。
/// </para>
/// </summary>
/// <remarks>
/// Track identifier (TrackId) async-context accessor and generator.
/// <para>
/// Backed by <see cref="System.Threading.AsyncLocal{T}"/> so the value flows across awaits,
/// letting HTTP middleware, unified result wrappers, and loggers share one TrackId per request chain.
/// </para>
/// </remarks>
public static class TrackIdContext
{
    /// <summary>
    /// 承载当前异步上下文 TrackId 的存储槽。
    /// </summary>
    /// <remarks>
    /// The storage slot holding the TrackId of the current async context.
    /// </remarks>
    private static readonly AsyncLocal<string> TrackIdStorage = new AsyncLocal<string>();

    /// <summary>
    /// 获取当前异步上下文中的 TrackId。
    /// <para>未设置时返回 <c>null</c>。</para>
    /// </summary>
    /// <remarks>
    /// Gets the TrackId of the current async context.
    /// <para>Returns <c>null</c> when not set.</para>
    /// </remarks>
    /// <value>当前 TrackId / The current TrackId</value>
    public static string Current => TrackIdStorage.Value;

    /// <summary>
    /// 设置当前异步上下文的 TrackId。
    /// <para>通常在请求管道入口处调用，使后续 <c>await</c> 延续均可读取到同一个值。</para>
    /// </summary>
    /// <remarks>
    /// Sets the TrackId of the current async context.
    /// <para>Typically called at the request pipeline entry so subsequent await continuations read the same value.</para>
    /// </remarks>
    /// <param name="trackId">要设置的 TrackId / The TrackId to set</param>
    public static void Set(string trackId)
    {
        TrackIdStorage.Value = trackId;
    }

    /// <summary>
    /// 生成一个新的短格式 TrackId。
    /// <para>
    /// 基于 <see cref="System.Guid"/>，编码为 URL 安全的 Base64 字符串（去除填充，<c>+</c>/<c>/</c> 替换为 <c>-</c>/<c>_</c>），
    /// 共 22 个字符。本方法仅负责生成，不会写入当前上下文。
    /// </para>
    /// </summary>
    /// <remarks>
    /// Generates a new short-form TrackId.
    /// <para>
    /// Based on <see cref="System.Guid"/>, encoded as a URL-safe Base64 string (padding stripped, <c>+</c>/<c>/</c> replaced with <c>-</c>/<c>_</c>),
    /// yielding 22 characters. This method only generates a value and does not write it to the current context.
    /// </para>
    /// </remarks>
    /// <returns>URL 安全的短 TrackId 字符串 / A URL-safe short TrackId string</returns>
    public static string Generate()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
