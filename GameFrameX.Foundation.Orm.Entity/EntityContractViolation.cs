// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相应法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 Apache License 2.0 单协议分发，
//   This project is licensed solely under the Apache License 2.0,,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root directory of the source code for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any legal disputes and liabilities arising from secondary development based on this project
//   本项目组织与贡献者概不承担。
//   shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository: https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:  https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:  https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// 实体契约违反记录。
/// </summary>
/// <remarks>
/// Represents a single entity contract violation.
/// </remarks>
public sealed class EntityContractViolation
{
    /// <summary>
    /// 获取违反契约的接口类型。
    /// </summary>
    /// <remarks>
    /// Gets the interface type that was violated.
    /// </remarks>
    public Type InterfaceType { get; }

    /// <summary>
    /// 获取缺失或类型不匹配的属性名称。
    /// </summary>
    /// <remarks>
    /// Gets the property name that is missing or has a mismatched type.
    /// </remarks>
    public string PropertyName { get; }

    /// <summary>
    /// 获取违反原因。
    /// </summary>
    /// <remarks>
    /// Gets the reason for the violation.
    /// </remarks>
    public string Reason { get; }

    /// <summary>
    /// 初始化契约违反记录。
    /// </summary>
    /// <remarks>
    /// Initializes a new contract violation record.
    /// </remarks>
    /// <param name="interfaceType">违反的接口类型 / The violated interface type</param>
    /// <param name="propertyName">属性名称 / The property name</param>
    /// <param name="reason">违反原因 / The violation reason</param>
    public EntityContractViolation(Type interfaceType, string propertyName, string reason)
    {
        InterfaceType = interfaceType;
        PropertyName = propertyName;
        Reason = reason;
    }
}