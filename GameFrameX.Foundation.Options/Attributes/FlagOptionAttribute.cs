// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Options.Attributes;

/// <summary>
/// 表示一个布尔标志选项的特性，存在即为true，不存在即为false
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FlagOptionAttribute : OptionAttribute
{
    /// <summary>
    /// 初始化 <see cref="FlagOptionAttribute"/> 类的新实例
    /// </summary>
    public FlagOptionAttribute()
    {
        DefaultValue = false;
    }

    /// <summary>
    /// 使用指定的长名称初始化 <see cref="FlagOptionAttribute"/> 类的新实例
    /// </summary>
    /// <param name="longName">选项的长名称</param>
    public FlagOptionAttribute(string longName) : base(longName)
    {
        DefaultValue = false;
    }

    /// <summary>
    /// 使用指定的短名称和长名称初始化 <see cref="FlagOptionAttribute"/> 类的新实例
    /// </summary>
    /// <param name="shortName">选项的短名称（单个字符）</param>
    /// <param name="longName">选项的长名称</param>
    public FlagOptionAttribute(char shortName, string longName) : base(shortName, longName)
    {
        DefaultValue = false;
    }
}