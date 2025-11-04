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

using System.Runtime.InteropServices;

namespace GameFrameX.Foundation.Utility;

/// <summary>
/// 平台运行时帮助类
/// Runtime platform helper that detects the current operating system.
/// </summary>
/// <remarks>
/// Provides simple boolean properties to query Linux, macOS (OSX), Windows, and FreeBSD.
/// Values are evaluated via <see cref="System.Runtime.InteropServices.RuntimeInformation"/> with <see cref="System.Runtime.InteropServices.OSPlatform"/>.
/// </remarks>
/// <example>
/// <code>
/// if (RuntimePlatformHelper.IsWindows)
/// {
///     // Windows-specific logic
/// }
/// else if (RuntimePlatformHelper.IsLinux)
/// {
///     // Linux-specific logic
/// }
/// </code>
/// </example>
/// <seealso cref="System.Runtime.InteropServices.RuntimeInformation"/>
/// <seealso cref="System.Runtime.InteropServices.OSPlatform"/>
public static class RuntimePlatformHelper
{
    /// <summary>
    /// 是否是Linux
    /// Indicates whether the current OS is Linux.
    /// </summary>
    /// <value>Returns <c>true</c> when running on Linux; otherwise <c>false</c>.</value>
    /// <remarks>Computed from <see cref="System.Runtime.InteropServices.RuntimeInformation"/> using <see cref="System.Runtime.InteropServices.OSPlatform.Linux"/>. This property is read-only.</remarks>
    /// <example>
    /// <code>
    /// bool onLinux = RuntimePlatformHelper.IsLinux;
    /// </code>
    /// </example>
    /// <seealso cref="System.Runtime.InteropServices.OSPlatform"/>
    public static bool IsLinux
    {
        get { return RuntimeInformation.IsOSPlatform(OSPlatform.Linux); }
    }

    /// <summary>
    /// 是否是Mac
    /// Indicates whether the current OS is macOS (OSX).
    /// </summary>
    /// <value>Returns <c>true</c> when running on macOS; otherwise <c>false</c>.</value>
    /// <remarks>Computed from <see cref="System.Runtime.InteropServices.RuntimeInformation"/> using <see cref="System.Runtime.InteropServices.OSPlatform.OSX"/>. This property is read-only.</remarks>
    /// <example>
    /// <code>
    /// if (RuntimePlatformHelper.IsOsx)
    /// {
    ///     // macOS specific behavior
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="System.Runtime.InteropServices.OSPlatform"/>
    public static bool IsOsx
    {
        get { return RuntimeInformation.IsOSPlatform(OSPlatform.OSX); }
    }

    /// <summary>
    /// 是否是Windows
    /// Indicates whether the current OS is Windows.
    /// </summary>
    /// <value>Returns <c>true</c> when running on Windows; otherwise <c>false</c>.</value>
    /// <remarks>Computed from <see cref="System.Runtime.InteropServices.RuntimeInformation"/> using <see cref="System.Runtime.InteropServices.OSPlatform.Windows"/>. This property is read-only.</remarks>
    /// <example>
    /// <code>
    /// if (RuntimePlatformHelper.IsWindows)
    /// {
    ///     // Enable windows-specific features
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="System.Runtime.InteropServices.OSPlatform"/>
    public static bool IsWindows
    {
        get { return RuntimeInformation.IsOSPlatform(OSPlatform.Windows); }
    }

    /// <summary>
    /// 是否是FreeBSD
    /// Indicates whether the current OS is FreeBSD.
    /// </summary>
    /// <value>Returns <c>true</c> when running on FreeBSD; otherwise <c>false</c>.</value>
    /// <remarks>Computed from <see cref="System.Runtime.InteropServices.RuntimeInformation"/> using <see cref="System.Runtime.InteropServices.OSPlatform.FreeBSD"/>. This property is read-only.</remarks>
    /// <example>
    /// <code>
    /// bool onBsd = RuntimePlatformHelper.IsFreeBsd;
    /// </code>
    /// </example>
    /// <seealso cref="System.Runtime.InteropServices.OSPlatform"/>
    public static bool IsFreeBsd
    {
        get { return RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD); }
    }
}
