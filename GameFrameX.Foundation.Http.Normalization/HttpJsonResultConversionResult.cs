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

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// HTTP JSON 结果转换失败阶段。
/// </summary>
public enum HttpJsonResultConversionFailureStage
{
    /// <summary>
    /// 无转换失败。
    /// </summary>
    None,

    /// <summary>
    /// 响应包装对象反序列化失败。
    /// </summary>
    ResultDeserialization,

    /// <summary>
    /// data 字段反序列化失败。
    /// </summary>
    DataDeserialization,
}

/// <summary>
/// HTTP JSON 结果转换诊断信息。
/// </summary>
/// <typeparam name="T">目标 data 类型。</typeparam>
public sealed class HttpJsonResultConversionResult<T>
{
    /// <summary>
    /// 初始化转换诊断信息。
    /// </summary>
    public HttpJsonResultConversionResult(
        bool succeeded,
        HttpJsonResultData<T> result,
        int errorCode,
        string errorMessage,
        HttpJsonResultConversionFailureStage failureStage,
        string exceptionType)
    {
        Succeeded = succeeded;
        Result = result;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage ?? string.Empty;
        FailureStage = failureStage;
        ExceptionType = exceptionType ?? string.Empty;
    }

    /// <summary>
    /// 转换是否成功。业务响应失败但结构有效时仍为 true。
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// 转换后的结果。
    /// </summary>
    public HttpJsonResultData<T> Result { get; }

    /// <summary>
    /// 诊断错误码。转换成功时等于结果码。
    /// </summary>
    public int ErrorCode { get; }

    /// <summary>
    /// 脱敏后的诊断消息，不包含原始响应体。
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// 转换失败阶段。
    /// </summary>
    public HttpJsonResultConversionFailureStage FailureStage { get; }

    /// <summary>
    /// 异常类型名称。无异常时为空。
    /// </summary>
    public string ExceptionType { get; }
}
