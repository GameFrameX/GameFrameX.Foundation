using System.Text.Json;
using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Logger;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 提供用于处理HTTP JSON结果的辅助方法。
/// </summary>
public static class HttpJsonResultHelper
{
    /// <summary>
    /// 将JSON字符串转换为HttpJsonResultData对象
    /// </summary>
    /// <typeparam name="T">泛型参数T，表示要反序列化的目标类型，必须是引用类型且包含无参构造函数</typeparam>
    /// <param name="jsonResult">需要转换的JSON字符串</param>
    /// <returns>返回转换后的HttpJsonResultData对象，包含反序列化结果和状态信息</returns>
    /// <remarks>
    /// 该方法会:
    /// 1. 尝试将JSON字符串反序列化为HttpJsonResult对象
    /// 2. 根据响应码(Code)判断请求是否成功
    /// 3. 如果成功(Code=0)，则将Data字段反序列化为泛型类型T
    /// 4. 如果失败，则保留错误信息，Data字段为默认值
    /// </remarks>
    public static HttpJsonResultData<T> ToHttpJsonResultData<T>(this string jsonResult) where T : class, new()
    {
        HttpJsonResultData<T> resultData = new HttpJsonResultData<T>
        {
            IsSuccess = false,
        };
        try
        {
            // 反序列化JSON字符串为HttpJsonResult对象
            var httpJsonResult = JsonHelper.Deserialize<HttpJsonResult>(jsonResult);
            // 检查响应码是否表示成功
            if (httpJsonResult.Code != 0)
            {
                resultData.Code = httpJsonResult.Code;
                resultData.Message = httpJsonResult.Message;
                return resultData; // 返回默认的失败结果
            }

            resultData.IsSuccess = true; // 设置成功标志
            // 反序列化数据部分，如果数据为空则返回类型T的默认实例
            resultData.Data = string.IsNullOrEmpty(httpJsonResult.Data) ? new T() : JsonHelper.Deserialize<T>(httpJsonResult.Data);
        }
        catch (Exception e)
        {
            // 捕获并输出异常信息
            LogHelper.Fatal(e, "JSON Deserialize Error {error}");
        }

        return resultData; // 返回结果数据
    }
}