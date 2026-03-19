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

using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Logger;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 提供用于处理HTTP JSON结果的扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for handling HTTP JSON results.
/// </remarks>
public static class HttpJsonResultHelper
{
    /// <summary>
    /// 将JSON字符串转换为HttpJsonResultData对象。
    /// </summary>
    /// <remarks>
    /// Converts a JSON string to an HttpJsonResultData object.
    /// This method will:
    /// 1. Attempt to deserialize the JSON string into an HttpJsonResult object
    /// 2. Determine if the request was successful based on the response code (IsSuccess property is automatically calculated based on Code==0)
    /// 3. If successful (Code=0), deserialize the Data field into the generic type T
    /// 4. If failed, preserve the error message and set the Data field to the default value
    /// </remarks>
    /// <typeparam name="T">泛型参数T，表示要反序列化的目标类型，必须是引用类型且包含无参构造函数 / Generic parameter T representing the target type to deserialize, must be a reference type with a parameterless constructor</typeparam>
    /// <param name="jsonResult">需要转换的JSON字符串 / The JSON string to convert</param>
    /// <returns>返回转换后的HttpJsonResultData对象，包含反序列化结果和状态信息 / The converted HttpJsonResultData object containing the deserialized result and status information</returns>
    public static HttpJsonResultData<T> ToHttpJsonResultData<T>(this string jsonResult) where T : class, new()
    {
        // 创建默认结果对象，Code初始化为失败状态
        HttpJsonResultData<T> resultData = new()
        {
            Code = HttpJsonResultConstants.FailCode,
        };
        try
        {
            // 反序列化JSON字符串为HttpJsonResult对象
            var httpJsonResult = JsonHelper.Deserialize<HttpJsonResult>(jsonResult);
            // 检查响应码是否表示成功
            if (httpJsonResult.Code != HttpJsonResultConstants.SuccessCode)
            {
                resultData.Code = httpJsonResult.Code;
                resultData.Message = httpJsonResult.Message;
                return resultData; // 返回失败结果
            }

            // 设置成功状态（IsSuccess会自动根据Code==0返回true）
            resultData.Code = HttpJsonResultConstants.SuccessCode;
            // 反序列化数据部分，如果数据为空则返回类型T的默认实例
            resultData.Data = string.IsNullOrEmpty(httpJsonResult.Data) ? new T() : JsonHelper.Deserialize<T>(httpJsonResult.Data);
        }
        catch (Exception e)
        {
            // 捕获并输出异常信息
            LogHelper.Fatal(e, "JSON Deserialize Error: {ErrorMessage}", e.Message);
        }

        return resultData; // 返回结果数据
    }
}
