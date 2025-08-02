﻿// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 常量特性，用于标记类、方法、属性等为常量定义
/// </summary>
/// <remarks>
/// 此特性可以应用于任何目标元素，用于标识该元素包含常量定义或具有常量性质。
/// 在ORM框架中，可以用于标记数据库常量、配置常量等。
/// </remarks>
/// <example>
/// <code>
/// [Const("DatabaseVersion")]
/// public class DatabaseConstants
/// {
///     public const string Version = "1.0.0";
/// }
/// 
/// [Const("StatusCode")]
/// public enum UserStatus
/// {
///     Active = 1,
///     Inactive = 0
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public sealed class ConstAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置常量的名称
    /// </summary>
    /// <value>常量的名称标识符</value>
    public string Name { get; set; }

    /// <summary>
    /// 初始化 <see cref="ConstAttribute"/> 类的新实例
    /// </summary>
    /// <param name="name">常量的名称，用于标识该常量的用途或类型</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="name"/> 为 null 时抛出</exception>
    public ConstAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
