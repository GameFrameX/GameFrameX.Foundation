namespace GameFrameX.Foundation.Encryption.Sm;

/// <summary>
/// 提供无符号右移位运算的支持类
/// </summary>
internal sealed class SupportClass
{
    /// <summary>
    /// 对指定数字执行无符号右移位运算
    /// </summary>
    /// <param name="number">要操作的数字</param>
    /// <param name="bits">要移位的位数</param>
    /// <returns>移位运算的结果</returns>
    public static int UrShift(int number, int bits)
    {
        if (number >= 0)
        {
            return number >> bits;
        }
        else
        {
            return (number >> bits) + (2 << ~bits);
        }
    }

    /// <summary>
    /// 对指定数字执行无符号右移位运算
    /// </summary>
    /// <param name="number">要操作的数字</param>
    /// <param name="bits">要移位的位数</param>
    /// <returns>移位运算的结果</returns>
    public static int URShift(int number, long bits)
    {
        return UrShift(number, (int)bits);
    }

    /// <summary>
    /// 对指定数字执行无符号右移位运算
    /// </summary>
    /// <param name="number">要操作的数字</param>
    /// <param name="bits">要移位的位数</param>
    /// <returns>移位运算的结果</returns>
    public static long URShift(long number, int bits)
    {
        if (number >= 0)
            return number >> bits;
        else
            return (number >> bits) + (2L << ~bits);
    }

    /// <summary>
    /// 对指定数字执行无符号右移位运算
    /// </summary>
    /// <param name="number">要操作的数字</param>
    /// <param name="bits">要移位的位数</param>
    /// <returns>移位运算的结果</returns>
    public static long URShift(long number, long bits)
    {
        return URShift(number, (int)bits);
    }
}