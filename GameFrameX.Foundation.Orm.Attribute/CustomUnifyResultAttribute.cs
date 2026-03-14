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
