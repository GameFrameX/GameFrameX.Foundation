// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;
using System.Collections.Generic;

namespace GameFrameX.Foundation.Options;

/// <summary>
/// 选项提供者，用于获取和缓存配置选项
/// </summary>
public class OptionsProvider
{
    private static readonly Dictionary<Type, object> _optionsCache = new Dictionary<Type, object>();
    private static string[] _args;

    /// <summary>
    /// 初始化选项提供者
    /// </summary>
    /// <param name="args">命令行参数</param>
    public static void Initialize(string[] args)
    {
        _args = args ?? Array.Empty<string>();
        _optionsCache.Clear();
    }

    /// <summary>
    /// 获取选项
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="skipValidation">是否跳过验证</param>
    /// <returns>选项对象</returns>
    public static T GetOptions<T>(bool skipValidation = false) where T : class, new()
    {
        var type = typeof(T);

        // 如果缓存中已存在，直接返回
        if (_optionsCache.TryGetValue(type, out var cachedOptions))
        {
            return (T)cachedOptions;
        }

        // 创建选项构建器
        var builder = new OptionsBuilder<T>(_args ?? Array.Empty<string>());

        // 构建选项
        var options = builder.Build(skipValidation);

        // 缓存选项
        _optionsCache[type] = options;

        return options;
    }

    /// <summary>
    /// 清除缓存
    /// </summary>
    public static void ClearCache()
    {
        _optionsCache.Clear();
    }

    /// <summary>
    /// 从缓存中移除指定类型的选项
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    public static void RemoveFromCache<T>() where T : class
    {
        var type = typeof(T);
        if (_optionsCache.ContainsKey(type))
        {
            _optionsCache.Remove(type);
        }
    }
}