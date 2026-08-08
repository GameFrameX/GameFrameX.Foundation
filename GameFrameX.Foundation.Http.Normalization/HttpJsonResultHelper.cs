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
    /// <typeparam name="T">泛型参数T，表示要反序列化的目标类型 / Generic parameter T representing the target type to deserialize</typeparam>
    /// <param name="jsonResult">需要转换的JSON字符串 / The JSON string to convert</param>
    /// <returns>返回转换后的HttpJsonResultData对象，包含反序列化结果和状态信息 / The converted HttpJsonResultData object containing the deserialized result and status information</returns>
    public static HttpJsonResultData<T> ToHttpJsonResultData<T>(this string jsonResult)
    {
        var conversion = jsonResult.TryToHttpJsonResultData<T>();
        if (conversion.Succeeded)
        {
            return conversion.Result;
        }

        return new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.FailCode,
        };
    }

    /// <summary>
    /// 尝试将JSON字符串转换为HttpJsonResultData对象，并返回可直接消费的转换诊断信息。
    /// </summary>
    /// <typeparam name="T">泛型参数T，表示要反序列化的目标类型 / Generic parameter T representing the target type to deserialize</typeparam>
    /// <param name="jsonResult">需要转换的JSON字符串 / The JSON string to convert</param>
    /// <returns>转换结果与诊断信息 / Conversion result and diagnostics</returns>
    public static HttpJsonResultConversionResult<T> TryToHttpJsonResultData<T>(this string jsonResult)
    {
        HttpJsonResult httpJsonResult;
        try
        {
            httpJsonResult = JsonHelper.Deserialize<HttpJsonResult>(jsonResult);
        }
        catch (Exception e)
        {
            return CreateFailure<T>(
                HttpJsonResultConversionFailureStage.ResultDeserialization,
                "Failed to deserialize HTTP JSON result.",
                e);
        }

        if (httpJsonResult == null)
        {
            return CreateFailure<T>(
                HttpJsonResultConversionFailureStage.ResultDeserialization,
                "Failed to deserialize HTTP JSON result.",
                null);
        }

        if (httpJsonResult.Code != HttpJsonResultConstants.SuccessCode)
        {
            var failureResult = new HttpJsonResultData<T>
            {
                Code = httpJsonResult.Code,
                Message = httpJsonResult.Message,
                TrackId = httpJsonResult.TrackId,
                ErrorCode = httpJsonResult.ErrorCode,
                Type = httpJsonResult.Type,
                Time = httpJsonResult.Time,
                Extras = httpJsonResult.Extras,
            };

            return new HttpJsonResultConversionResult<T>(
                true,
                failureResult,
                failureResult.Code,
                failureResult.Message,
                HttpJsonResultConversionFailureStage.None,
                string.Empty);
        }

        try
        {
            var successResult = new HttpJsonResultData<T>
            {
                Code = HttpJsonResultConstants.SuccessCode,
                Message = httpJsonResult.Message ?? string.Empty,
                Data = string.IsNullOrEmpty(httpJsonResult.Data) ? default : JsonHelper.Deserialize<T>(httpJsonResult.Data),
                TrackId = httpJsonResult.TrackId,
                ErrorCode = httpJsonResult.ErrorCode,
                Type = httpJsonResult.Type,
                Time = httpJsonResult.Time,
                Extras = httpJsonResult.Extras,
            };

            return new HttpJsonResultConversionResult<T>(
                true,
                successResult,
                successResult.Code,
                successResult.Message,
                HttpJsonResultConversionFailureStage.None,
                string.Empty);
        }
        catch (Exception e)
        {
            return CreateFailure<T>(
                HttpJsonResultConversionFailureStage.DataDeserialization,
                "Failed to deserialize HTTP JSON result data.",
                e);
        }
    }

    private static HttpJsonResultConversionResult<T> CreateFailure<T>(
        HttpJsonResultConversionFailureStage failureStage,
        string errorMessage,
        Exception exception)
    {
        var result = new HttpJsonResultData<T>
        {
            Code = HttpJsonResultConstants.FailCode,
            Message = errorMessage,
        };

        return new HttpJsonResultConversionResult<T>(
            false,
            result,
            result.Code,
            errorMessage,
            failureStage,
            exception?.GetType().Name ?? string.Empty);
    }
}
