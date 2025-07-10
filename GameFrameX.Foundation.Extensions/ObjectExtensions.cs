// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 守护.用于判断参数
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// 检查对象是否为null
    /// </summary>
    /// <param name="self">要检查的对象</param>
    /// <returns>如果对象为null，则返回true；否则返回false</returns>
    public static bool IsNull(this object self)
    {
        return self == null;
    }

    /// <summary>
    /// 检查对象是否不为null
    /// </summary>
    /// <param name="self">要检查的对象</param>
    /// <returns>如果对象不为null，则返回true；否则返回false</returns>
    public static bool IsNotNull(this object self)
    {
        return !self.IsNull();
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
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {minValue} and less than {maxValue}.");
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this int value, int minValue = 0, int maxValue = int.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this uint value, uint minValue = 0, uint maxValue = uint.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            return false;
        }

        return true;
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
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {minValue} and less than {maxValue}.");
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this long value, long minValue = 0, long maxValue = long.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            return false;
        }

        return true;
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
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {minValue} and less than {maxValue}.");
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this ulong value, ulong minValue = 0, ulong maxValue = ulong.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            return false;
        }

        return true;
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
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {minValue} and less than {maxValue}.");
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this short value, short minValue = 0, short maxValue = short.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            return false;
        }

        return true;
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
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {minValue} and less than {maxValue}.");
        }
    }

    /// <summary>
    /// 判断参数是否在最大值和最小值之间
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="minValue">最小值</param>
    /// <param name="maxValue">最大值</param>
    /// <returns>返回是否在范围内</returns>
    public static bool IsRange(this ushort value, ushort minValue = 0, ushort maxValue = ushort.MaxValue)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            return false;
        }

        return true;
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
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(minValue, maxValue, nameof(minValue));
        if (value <= minValue || value >= maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be greater than {minValue} and less than {maxValue}.");
        }
    }
}