// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// 资源提供者接口 - 本地化资源的基础抽象
/// </summary>
/// <remarks>
/// 定义了获取本地化字符串的基本契约，所有本地化资源提供者都应实现此接口。
/// 实现类可以基于不同的资源存储方式，如程序集资源、文件、数据库等。
/// </remarks>
public interface IResourceProvider
{
    /// <summary>
    /// 获取本地化字符串
    /// </summary>
    /// <param name="key">资源键，用于标识特定的本地化字符串</param>
    /// <returns>
    /// 如果找到对应的本地化字符串，返回本地化值；
    /// 如果未找到，返回传入的资源键作为后备值
    /// </returns>
    /// <remarks>
    /// 实现者应该确保线程安全性，支持并发调用。
    /// 当传入的键为 null 或空字符串时，应直接返回该键。
    /// </remarks>
    /// <example>
    /// <code>
    /// var provider = new MyResourceProvider();
    /// var message = provider.GetString("User.Welcome");
    /// // 返回：如果找到 "User.Welcome" 对应的本地化字符串，否则返回 "User.Welcome"
    /// </code>
    /// </example>
    string GetString(string key);
}

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