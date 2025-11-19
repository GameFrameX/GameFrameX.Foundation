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

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// 可懒加载的资源提供者接口
/// </summary>
/// <remarks>
/// 继承自 IResourceProvider，增加了懒加载能力，支持资源的延迟初始化。
/// 实现者可以优化资源加载性能，只在需要时才加载资源。
/// </remarks>
public interface ILazyResourceProvider : IResourceProvider
{
    /// <summary>
    /// 获取一个值，指示资源提供者是否已完成初始化
    /// </summary>
    /// <value>
    /// 如果资源提供者已加载并准备好提供服务，则为 true；否则为 false。
    /// </value>
    bool IsInitialized { get; }

    /// <summary>
    /// 确保资源已加载并初始化
    /// </summary>
    /// <remarks>
    /// 如果资源尚未加载，此方法将触发资源加载过程。
    /// 如果资源已经加载，此方法应该立即返回，不执行额外操作。
    /// 实现者应该确保此方法是线程安全的，可以多次调用。
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new MyLazyResourceProvider();
    /// Console.WriteLine($"Initialized: {provider.IsInitialized}"); // false
    ///
    /// provider.EnsureLoaded();
    /// Console.WriteLine($"Initialized: {provider.IsInitialized}"); // true
    ///
    /// provider.EnsureLoaded(); // 重复调用应该快速返回
    /// </code>
    /// </example>
    void EnsureLoaded();
}