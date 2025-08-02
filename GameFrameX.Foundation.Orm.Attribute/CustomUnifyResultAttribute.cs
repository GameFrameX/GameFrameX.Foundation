﻿// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 自定义统一结果特性，用于标记需要自定义结果统一处理的类或方法
/// </summary>
/// <remarks>
/// 此特性用于在ORM框架中标识需要进行自定义结果统一处理的元素。
/// 当应用此特性时，框架会根据指定的名称应用相应的结果统一策略，
/// 例如数据格式转换、结果包装、异常处理等。
/// </remarks>
/// <example>
/// <code>
/// [CustomUnifyResult("ApiResponse")]
/// public class UserController
/// {
///     public User GetUser(int id) => userService.GetById(id);
/// }
/// 
/// [CustomUnifyResult("DatabaseResult")]
/// public class UserRepository
/// {
///     public List&lt;User&gt; GetUsers() => database.Query&lt;User&gt;();
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public sealed class CustomUnifyResultAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置自定义统一结果处理器的名称
    /// </summary>
    /// <value>统一结果处理器的名称标识符</value>
    public string Name { get; set; }

    /// <summary>
    /// 初始化 <see cref="CustomUnifyResultAttribute"/> 类的新实例
    /// </summary>
    /// <param name="name">自定义统一结果处理器的名称，用于标识使用哪种结果统一策略</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="name"/> 为 null 时抛出</exception>
    public CustomUnifyResultAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
