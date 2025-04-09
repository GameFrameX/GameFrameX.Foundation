using System.Text.Json;
using GameFrameX.Foundation.Logger;

namespace GameFrameX.Foundation.Http.Normalization;

/// <summary>
/// 提供用于处理HTTP JSON结果的辅助方法。
/// </summary>
public static class HttpJsonResultHelper
{
    /// <summary>
    /// 将JSON字符串转换为HttpJsonResultData<T>对象。
    /// 该方法尝试反序列化给定的JSON字符串，并根据HTTP响应的状态码设置IsSuccess属性。
    /// 如果响应成功，Data属性将包含反序列化后的数据对象；否则，Data将为默认值。
    /// </summary>
    /// <typeparam name="T">要反序列化为的对象类型，必须是类并具有无参数构造函数。</typeparam>
    /// <param name="jsonResult">包含HTTP响应的JSON字符串。</param>
    /// <returns>HttpJsonResultData<T>对象，表示反序列化的结果。</returns>
    public static HttpJsonResultData<T> ToHttpJsonResultData<T>(this string jsonResult) where T : class, new()
    {
        HttpJsonResultData<T> resultData = new HttpJsonResultData<T>
        {
            IsSuccess = false,
        };
        try
        {
            // 反序列化JSON字符串为HttpJsonResult对象
            var httpJsonResult = JsonSerializer.Deserialize<HttpJsonResult>(jsonResult);
            // 检查响应码是否表示成功
            if (httpJsonResult.Code != 0)
            {
                resultData.Code = httpJsonResult.Code;
                resultData.Message = httpJsonResult.Message;
                return resultData; // 返回默认的失败结果
            }

            resultData.IsSuccess = true; // 设置成功标志
            // 反序列化数据部分，如果数据为空则返回类型T的默认实例
            resultData.Data = string.IsNullOrEmpty(httpJsonResult.Data) ? new T() : JsonSerializer.Deserialize<T>(httpJsonResult.Data);
        }
        catch (Exception e)
        {
            // 捕获并输出异常信息
            LogHelper.Fatal(e);
        }

        return resultData; // 返回结果数据
    }
}