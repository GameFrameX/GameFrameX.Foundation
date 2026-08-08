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

using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 链路追踪标识（TrackId）HTTP 中间件。
/// <para>
/// 在请求管道入口解析或生成 TrackId，将其写入 <see cref="TrackIdContext"/>（供统一结果包装器读取并填充响应）、
/// 回写响应头，并通过 <see cref="LogContext"/> 推入 Serilog 日志上下文，使本次请求处理链路产生的所有日志
/// 都带上同一个 TrackId 属性。
/// </para>
/// </summary>
/// <remarks>
/// Track identifier (TrackId) HTTP middleware.
/// <para>
/// At the request pipeline entry it resolves or generates a TrackId, writes it to <see cref="TrackIdContext"/>
/// (so unified result wrappers can fill the response), echoes it back via the response header, and pushes it onto
/// the Serilog <see cref="LogContext"/> so every log emitted along the request chain carries the same TrackId property.
/// </para>
/// </remarks>
public sealed class TrackIdMiddleware
{
    /// <summary>
    /// TrackId 在请求/响应头中使用的标准名称。
    /// </summary>
    /// <remarks>
    /// The standard header name used for the TrackId in both requests and responses.
    /// </remarks>
    public const string HeaderName = "X-Track-Id";

    private const string LogContextTrackIdProperty = "TrackId";

    private readonly RequestDelegate _next;

    /// <summary>
    /// 初始化 <see cref="TrackIdMiddleware"/> 的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TrackIdMiddleware"/> class.
    /// </remarks>
    /// <param name="next">管道中的下一个委托 / The next delegate in the pipeline</param>
    public TrackIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 执行中间件：解析 TrackId、写入上下文与响应头、推入日志上下文后调用后续管道。
    /// </summary>
    /// <remarks>
    /// Executes the middleware: resolves the TrackId, writes it to the context and response header,
    /// pushes it onto the log context, then invokes the next delegate.
    /// </remarks>
    /// <param name="context">当前 HTTP 上下文 / The current HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var trackId = ResolveTrackId(context.Request.Headers[HeaderName]);
        TrackIdContext.Set(trackId);
        context.Response.Headers[HeaderName] = trackId;

        using (LogContext.PushProperty(LogContextTrackIdProperty, trackId))
        {
            await _next(context);
        }
    }

    /// <summary>
    /// 从请求头值解析 TrackId：存在且非空白时透传，否则生成新的短 TrackId。
    /// </summary>
    /// <remarks>
    /// Resolves the TrackId from the request header value: passes it through when present and non-blank,
    /// otherwise generates a new short TrackId.
    /// </remarks>
    /// <param name="headerValue">请求头中的 TrackId 值（可能为空）/ The TrackId value from the request header (may be empty)</param>
    /// <returns>解析得到的 TrackId / The resolved TrackId</returns>
    private static string ResolveTrackId(string headerValue)
    {
        if (!string.IsNullOrWhiteSpace(headerValue))
        {
            return headerValue;
        }

        return TrackIdContext.Generate();
    }
}
