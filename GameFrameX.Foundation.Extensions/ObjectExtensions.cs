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

using System.Diagnostics.CodeAnalysis;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 守护.用于判断参数
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// 检查引用类型对象是否为null
    /// </summary>
    /// <typeparam name="T">对象的类型，必须为引用类型</typeparam>
    /// <param name="self">要检查的对象</param>
    /// <returns>如果对象为null，则返回true；否则返回false</returns>
    /// <remarks>
    /// 此方法仅适用于引用类型，对值类型调用此方法会导致编译错误。
    /// 这是为了避免对值类型（永远不会为null）调用此方法产生误导性结果。
    /// </remarks>
    public static bool IsNull<T>([NotNullWhen(false)] this T self) where T : class
    {
        return self is null;
    }

    /// <summary>
    /// 检查引用类型对象是否不为null
    /// </summary>
    /// <typeparam name="T">对象的类型，必须为引用类型</typeparam>
    /// <param name="self">要检查的对象</param>
    /// <returns>如果对象不为null，则返回true；否则返回false</returns>
    /// <remarks>
    /// 此方法仅适用于引用类型，对值类型调用此方法会导致编译错误。
    /// 这是为了避免对值类型（永远不会为null）调用此方法产生误导性结果。
    /// </remarks>
    public static bool IsNotNull<T>([NotNullWhen(true)] this T self) where T : class
    {
        return self is not null;
    }

    /// <summary>
    /// 检查对象是否为null，如果为null则抛出异常
    /// </summary>
    /// <param name="self">要检查的对象</param>
    /// <param name="paramName">参数名称</param>
    /// <exception cref="ArgumentNullException">当对象为null时抛出</exception>
    public static void ThrowIfNull(this object self, string paramName)
    {
        ArgumentNullException.ThrowIfNull(paramName, nameof(paramName));
        ArgumentNullException.ThrowIfNull(self, paramName);
    }


    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数不在范围内时,引发参数超出范围异常</exception>
    public static void CheckRange(this int value, int minValue = 0, int maxValue = int.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.ValueMustBeBetween, value, minValue, maxValue));
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间（闭区间）
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this int value, int minValue = 0, int maxValue = int.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        return value >= minValue && value <= maxValue;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间（闭区间）
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this uint value, uint minValue = 0, uint maxValue = uint.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        return value >= minValue && value <= maxValue;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间，如果不在范围内则抛出异常
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数不在范围内时,引发参数超出范围异常</exception>
    public static void CheckRange(this uint value, uint minValue = 0, uint maxValue = uint.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.ValueMustBeBetween, value, minValue, maxValue));
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间（闭区间）
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this long value, long minValue = 0, long maxValue = long.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        return value >= minValue && value <= maxValue;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间，如果不在范围内则抛出异常
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数不在范围内时,引发参数超出范围异常</exception>
    public static void CheckRange(this long value, long minValue = 0, long maxValue = long.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.ValueMustBeBetween, value, minValue, maxValue));
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间（闭区间）
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this ulong value, ulong minValue = 0, ulong maxValue = ulong.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        return value >= minValue && value <= maxValue;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间，如果不在范围内则抛出异常
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数不在范围内时,引发参数超出范围异常</exception>
    public static void CheckRange(this ulong value, ulong minValue = 0, ulong maxValue = ulong.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.ValueMustBeBetween, value, minValue, maxValue));
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间（闭区间）
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this short value, short minValue = 0, short maxValue = short.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        return value >= minValue && value <= maxValue;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间，如果不在范围内则抛出异常
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数不在范围内时,引发参数超出范围异常</exception>
    public static void CheckRange(this short value, short minValue = 0, short maxValue = short.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.ValueMustBeBetween, value, minValue, maxValue));
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间（闭区间）
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this ushort value, ushort minValue = 0, ushort maxValue = ushort.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        return value >= minValue && value <= maxValue;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间，如果不在范围内则抛出异常
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <exception cref="ArgumentOutOfRangeException">当参数不在范围内时,引发参数超出范围异常</exception>
    public static void CheckRange(this ushort value, ushort minValue = 0, ushort maxValue = ushort.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(minValue, maxValue, nameof(minValue));
        if (value < minValue || value > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), LocalizationService.GetString(LocalizationKeys.Exceptions.ValueMustBeBetween, value, minValue, maxValue));
        }
    }
}