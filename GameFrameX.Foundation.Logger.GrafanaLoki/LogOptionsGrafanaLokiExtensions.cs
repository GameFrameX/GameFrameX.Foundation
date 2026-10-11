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
//  Any legal disputes or liabilities arising from secondary development based on this project
//  本组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository: https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.IO.Compression;
using System.Net.Http.Headers;
using GameFrameX.Foundation.Logger;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace GameFrameX.Foundation.Logger.GrafanaLoki;

/// <summary>
/// LogOptions 的 Grafana Loki 组件接入扩展：把 Loki sink 以外部组件方式插入日志系统。
/// </summary>
/// <remarks>
/// Extensions on <see cref="LogOptions"/> that plug the Grafana Loki sink into the logging system as an external component.
/// 用法：<code>LogHandler.Create(new LogOptions().UseGrafanaLoki("http://loki:3100"))</code>。
/// </remarks>
public static class LogOptionsGrafanaLokiExtensions
{
    /// <summary>
    /// 注册 Grafana Loki sink（便捷重载，仅指定服务地址，其余使用组件默认值）。
    /// </summary>
    /// <param name="logOptions">宿主日志配置选项 / The host log options</param>
    /// <param name="url">Grafana Loki 服务地址 / The Grafana Loki service address</param>
    /// <returns>传入的 <paramref name="logOptions"/>，支持链式配置 / The same <paramref name="logOptions"/> for fluent chaining</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="logOptions"/> 或 <paramref name="url"/> 为 null 时抛出 / Thrown when <paramref name="logOptions"/> or <paramref name="url"/> is null</exception>
    /// <exception cref="ArgumentException">当 <paramref name="url"/> 为空白字符时抛出 / Thrown when <paramref name="url"/> is whitespace</exception>
    public static LogOptions UseGrafanaLoki(this LogOptions logOptions, string url)
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        return logOptions.UseGrafanaLoki(sinkOptions => { sinkOptions.Url = url; });
    }

    /// <summary>
    /// 注册 Grafana Loki sink（完整配置重载：label、附加属性、凭证、压缩与批量参数）。
    /// </summary>
    /// <param name="logOptions">宿主日志配置选项 / The host log options</param>
    /// <param name="configure">组件配置回调 / The component configuration callback</param>
    /// <returns>传入的 <paramref name="logOptions"/>，支持链式配置 / The same <paramref name="logOptions"/> for fluent chaining</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="logOptions"/> 或 <paramref name="configure"/> 为 null 时抛出 / Thrown when <paramref name="logOptions"/> or <paramref name="configure"/> is null</exception>
    public static LogOptions UseGrafanaLoki(this LogOptions logOptions, Action<GrafanaLokiSinkOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(logOptions);
        ArgumentNullException.ThrowIfNull(configure);

        var sinkOptions = new GrafanaLokiSinkOptions();
        configure(sinkOptions);

        // 向核心包的统一外部 sink 注入点注册装配委托（C77 组件化）。
        logOptions.SinkConfigurators.Add(loggerConfiguration => AddGrafanaLokiSink(loggerConfiguration, logOptions, sinkOptions));

        return logOptions;
    }

    /// <summary>
    /// 装配 Grafana Loki sink：property 富化、label 转换、凭证构造、按压缩开关选择 HTTP handler。
    /// </summary>
    /// <param name="loggerConfiguration">Serilog 配置（Enrich 与 WriteTo 均可达） / The Serilog configuration (both Enrich and WriteTo reachable)</param>
    /// <param name="hostOptions">宿主日志配置选项（最低级别跟随其 LogEventLevel） / The host log options (minimum level follows its LogEventLevel)</param>
    /// <param name="sinkOptions">组件配置选项 / The component options</param>
    private static void AddGrafanaLokiSink(LoggerConfiguration loggerConfiguration, LogOptions hostOptions, GrafanaLokiSinkOptions sinkOptions)
    {
        // property 富化：空 key / 空值跳过（语义自原 LogHandler.ApplyLokiProperties 迁移）。
        if (sinkOptions.Properties != null)
        {
            foreach (var property in sinkOptions.Properties)
            {
                if (string.IsNullOrWhiteSpace(property.Key) || string.IsNullOrWhiteSpace(property.Value))
                {
                    continue;
                }

                loggerConfiguration.Enrich.WithProperty(property.Key, property.Value);
            }
        }

        var labels = BuildLabels(sinkOptions);
        var credentials = BuildCredentials(sinkOptions);
        var minimumLevel = sinkOptions.RestrictedToMinimumLevel ?? hostOptions.LogEventLevel;

        if (sinkOptions.CompressionEnabled)
        {
            loggerConfiguration.WriteTo.GrafanaLoki(
                sinkOptions.Url,
                labels: labels,
                credentials: credentials,
                batchSizeLimit: sinkOptions.BatchSizeLimit,
                queueLimit: sinkOptions.QueueLimit,
                period: TimeSpan.FromSeconds(sinkOptions.Period),
                httpMessageHandler: new LokiGzipHandler(CompressionLevel.Optimal),
                restrictedToMinimumLevel: minimumLevel);
        }
        else
        {
            loggerConfiguration.WriteTo.GrafanaLoki(
                sinkOptions.Url,
                labels: labels,
                credentials: credentials,
                batchSizeLimit: sinkOptions.BatchSizeLimit,
                queueLimit: sinkOptions.QueueLimit,
                period: TimeSpan.FromSeconds(sinkOptions.Period),
                restrictedToMinimumLevel: minimumLevel);
        }
    }

    /// <summary>
    /// 将 Labels 字典转换为 Serilog Loki 所需的标签数组；null 视为无标签。
    /// </summary>
    /// <param name="sinkOptions">组件配置选项 / The component options</param>
    /// <returns>Loki 标签数组 / The Loki label array</returns>
    private static LokiLabel[] BuildLabels(GrafanaLokiSinkOptions sinkOptions)
    {
        if (sinkOptions.Labels == null || sinkOptions.Labels.Count == 0)
        {
            return Array.Empty<LokiLabel>();
        }

        var labels = new List<LokiLabel>(sinkOptions.Labels.Count);
        foreach (var kv in sinkOptions.Labels)
        {
            labels.Add(new LokiLabel
            {
                Key = kv.Key,
                Value = kv.Value,
            });
        }

        return labels.ToArray();
    }

    /// <summary>
    /// 根据用户名/密码构造 LokiCredentials；任一为空返回 null（不启用认证）。
    /// </summary>
    /// <param name="sinkOptions">组件配置选项 / The component options</param>
    /// <returns>Loki 凭证或 null / Loki credentials or null</returns>
    private static LokiCredentials BuildCredentials(GrafanaLokiSinkOptions sinkOptions)
    {
        if (string.IsNullOrWhiteSpace(sinkOptions.UserName) || string.IsNullOrWhiteSpace(sinkOptions.Password))
        {
            return null;
        }

        return new LokiCredentials
        {
            Login = sinkOptions.UserName,
            Password = sinkOptions.Password,
        };
    }
}

/// <summary>
/// Grafana Loki 请求的 Gzip 压缩 HTTP handler（自原 LogHandler.LokiGzipHandler 迁移，C77 组件化）。
/// </summary>
/// <remarks>
/// Gzip-compressing HTTP handler for Grafana Loki requests (migrated from the original LogHandler.LokiGzipHandler).
/// </remarks>
internal sealed class LokiGzipHandler : DelegatingHandler
{
    private readonly CompressionLevel _compressionLevel;

    public LokiGzipHandler(CompressionLevel compressionLevel) : base(new HttpClientHandler())
    {
        _compressionLevel = compressionLevel;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var bytes = await request.Content.ReadAsByteArrayAsync(cancellationToken);
            using (var stream = new MemoryStream())
            {
                await using (var gzipStream = new GZipStream(stream, _compressionLevel, leaveOpen: true))
                {
                    await gzipStream.WriteAsync(bytes, cancellationToken);
                }

                request.Content = new ByteArrayContent(stream.ToArray());
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
                {
                    CharSet = "utf-8",
                };
                request.Content.Headers.ContentEncoding.Add("gzip");
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
