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

namespace GameFrameX.Foundation.Utility;

/// <summary>
/// 环境帮助器工具类。
/// </summary>
/// <remarks>
/// Environment helper utility class that provides methods to detect the current runtime environment.
/// </remarks>
public static class EnvironmentHelper
{
    /// <summary>
    /// 定义标准环境名称常量。
    /// </summary>
    /// <remarks>
    /// Defines standard environment name constants.
    /// </remarks>
    static class Environments
    {
        /// <summary>
        /// 指定开发环境。
        /// </summary>
        /// <remarks>
        /// Specifies the Development environment. The development environment can enable features that shouldn't be exposed in production. Because of the performance cost, scope validation and dependency validation only happens in development.
        /// </remarks>
        public static readonly string Development = "Development";

        /// <summary>
        /// 指定预发布环境。
        /// </summary>
        /// <remarks>
        /// Specifies the Staging environment. The staging environment can be used to validate app changes before changing the environment to production.
        /// </remarks>
        public static readonly string Staging = "Staging";

        /// <summary>
        /// 指定生产环境。
        /// </summary>
        /// <remarks>
        /// Specifies the Production environment. The production environment should be configured to maximize security, performance, and application robustness.
        /// </remarks>
        public static readonly string Production = "Production";
    }

    /// <summary>
    /// 判断当前环境是否为开发环境。
    /// </summary>
    /// <remarks>
    /// Determines whether the current environment is a development environment by checking if the ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT environment variable is set to Development.
    /// </remarks>
    /// <returns>如果是开发环境则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the current environment is development; otherwise <c>false</c></returns>
    public static bool IsDevelopment()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        return string.Equals(env, Environments.Development, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断当前环境是否为生产环境。
    /// </summary>
    /// <remarks>
    /// Determines whether the current environment is a production environment by checking if the ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT environment variable is set to Production.
    /// </remarks>
    /// <returns>如果是生产环境则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the current environment is production; otherwise <c>false</c></returns>
    public static bool IsProduction()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        return string.Equals(env, Environments.Production, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断当前环境是否为预发布环境。
    /// </summary>
    /// <remarks>
    /// Determines whether the current environment is a staging/testing environment by checking if the ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT environment variable is set to Staging.
    /// </remarks>
    /// <returns>如果是预发布环境则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the current environment is staging; otherwise <c>false</c></returns>
    public static bool IsStaging()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        return string.Equals(env, Environments.Staging, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断当前环境是否与指定的自定义环境名称匹配。
    /// </summary>
    /// <remarks>
    /// Determines whether the current environment matches the specified custom environment name by checking if the ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT environment variable matches the specified environment name.
    /// </remarks>
    /// <param name="environmentName">要检查的环境名称 / The environment name to check</param>
    /// <returns>如果当前环境与指定环境名称匹配则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if the current environment matches the specified environment name; otherwise <c>false</c></returns>
    public static bool IsEnvironment(string environmentName)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        return string.Equals(env, environmentName, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断当前应用是否运行在 Docker 容器中。
    /// </summary>
    /// <remarks>
    /// Determines whether the current application is running in a Docker container by checking if the DOTNET_RUNNING_IN_CONTAINER environment variable exists.
    /// </remarks>
    /// <returns>如果在 Docker 容器中运行则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if running in a Docker container; otherwise <c>false</c></returns>
    public static bool IsDocker()
    {
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"));
    }

    /// <summary>
    /// 判断当前应用是否运行在 Kubernetes 集群中。
    /// </summary>
    /// <remarks>
    /// Determines whether the current application is running in a Kubernetes cluster by checking if the KUBERNETES_SERVICE_HOST environment variable exists.
    /// </remarks>
    /// <returns>如果在 Kubernetes 集群中运行则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if running in a Kubernetes cluster; otherwise <c>false</c></returns>
    public static bool IsKubernetes()
    {
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST"));
    }

    /// <summary>
    /// 获取当前环境名称。
    /// </summary>
    /// <remarks>
    /// Gets the current environment name from environment variables. Returns the value of ASPNETCORE_ENVIRONMENT or DOTNET_ENVIRONMENT, or <c>null</c> if neither is set.
    /// </remarks>
    /// <returns>当前环境名称，如果未设置则返回 <c>null</c> / The current environment name, or <c>null</c> if not set</returns>
    public static string GetEnvironmentName()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                  ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        return env;
    }
}