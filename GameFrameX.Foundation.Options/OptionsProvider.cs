// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项提供器，用于从命令行参数和环境变量获取配置选项
/// </summary>
/// <remarks>
/// 该类提供了从命令行参数和环境变量获取配置选项的静态方法
/// </remarks>
public static class OptionsProvider
{
    /// <summary>
    /// 从命令行参数和环境变量获取配置选项
    /// </summary>
    /// <typeparam name="T">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="boolFormat">布尔参数格式</param>
    /// <param name="ensurePrefixedKeys">是否确保参数键都有前缀</param>
    /// <returns>配置选项对象</returns>
    public static T GetOptions<T>(string[] args, BoolArgumentFormat boolFormat = BoolArgumentFormat.Flag, bool ensurePrefixedKeys = true) where T : class, new()
    {
        var builder = new OptionsBuilder<T>(args, boolFormat, ensurePrefixedKeys);
        return builder.Build();
    }

    /// <summary>
    /// 从环境变量获取配置选项
    /// </summary>
    /// <typeparam name="T">配置选项类型</typeparam>
    /// <returns>配置选项对象</returns>
    public static T GetOptionsFromEnvironment<T>() where T : class, new()
    {
        var builder = new OptionsBuilder<T>(null);
        return builder.Build();
    }

    /// <summary>
    /// 从命令行参数获取配置选项（不使用环境变量）
    /// </summary>
    /// <typeparam name="T">配置选项类型</typeparam>
    /// <param name="args">命令行参数</param>
    /// <param name="boolFormat">布尔参数格式</param>
    /// <param name="ensurePrefixedKeys">是否确保参数键都有前缀</param>
    /// <returns>配置选项对象</returns>
    public static T GetOptionsFromArgs<T>(string[] args, BoolArgumentFormat boolFormat = BoolArgumentFormat.Flag, bool ensurePrefixedKeys = true) where T : class, new()
    {
        var builder = new OptionsBuilder<T>(args, boolFormat, ensurePrefixedKeys, useEnvironmentVariables: false);
        return builder.Build();
    }
}