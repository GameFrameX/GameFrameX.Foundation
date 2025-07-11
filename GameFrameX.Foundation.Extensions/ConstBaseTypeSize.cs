// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 常量型变量的字节数
/// 提供了常用基础数据类型的字节大小常量值
/// </summary>
public static class ConstBaseTypeSize
{
    /// <summary>
    /// 整型变量（32 位）的字节数。
    /// 取值范围：-2,147,483,648 到 2,147,483,647
    /// </summary>
    public const int IntSize = sizeof(int);

    /// <summary>
    /// 短整型变量（16 位）的字节数。
    /// 取值范围：-32,768 到 32,767
    /// </summary>
    public const int ShortSize = sizeof(short);

    /// <summary>
    /// 长整型变量（64 位）的字节数。
    /// 取值范围：-9,223,372,036,854,775,808 到 9,223,372,036,854,775,807
    /// </summary>
    public const int LongSize = sizeof(long);

    /// <summary>
    /// 单精度浮点型变量的字节数。
    /// 精度：约7位十进制数字
    /// 取值范围：±1.5 x 10^-45 到 ±3.4 x 10^38
    /// </summary>
    public const int FloatSize = sizeof(float);

    /// <summary>
    /// 双精度浮点型变量的字节数。
    /// 精度：约15-17位十进制数字
    /// 取值范围：±5.0 × 10^-324 到 ±1.7 × 10^308
    /// </summary>
    public const int DoubleSize = sizeof(double);

    /// <summary>
    /// 字节型变量（8 位）的字节数。
    /// 取值范围：0 到 255
    /// 常用于处理二进制数据和文件操作
    /// </summary>
    public const int ByteSize = sizeof(byte);

    /// <summary>
    /// 有符号字节类型变量的字节数。
    /// 取值范围：-128 到 127
    /// </summary>
    public const int SbyteSize = sizeof(sbyte);

    /// <summary>
    /// 布尔型变量的字节数。
    /// 仅存储 true 或 false 两种状态
    /// </summary>
    public const int BoolSize = sizeof(bool);

    /// <summary>
    /// 无符号整型变量（32 位）的字节数。
    /// 取值范围：0 到 4,294,967,295
    /// </summary>
    public const int UIntSize = sizeof(uint);

    /// <summary>
    /// 无符号短整型变量（16 位）的字节数。
    /// 取值范围：0 到 65,535
    /// </summary>
    public const int UShortSize = sizeof(ushort);

    /// <summary>
    /// 无符号长整型变量（64 位）的字节数。
    /// 取值范围：0 到 18,446,744,073,709,551,615
    /// </summary>
    public const int ULongSize = sizeof(ulong);

    /// <summary>
    /// 字符型变量（16 位 Unicode）的字节数。
    /// 取值范围：U+0000 到 U+FFFF
    /// 用于存储单个 Unicode 字符
    /// </summary>
    public const int CharSize = sizeof(char);

    /// <summary>
    /// 高精度十进制浮点型变量的字节数。
    /// 精度：28-29位十进制数字
    /// 取值范围：±1.0 × 10^-28 到 ±7.9228 × 10^28
    /// 适用于金融计算等需要高精度的场景
    /// </summary>
    public const int DecimalSize = sizeof(decimal);

    /// <summary>
    /// 日期时间类型变量的字节数。
    /// 表示从公元1年1月1日午夜12:00:00到9999年12月31日晚上11:59:59之间的日期和时间
    /// 精度：100纳秒（0.1微秒）
    /// </summary>
    public const int DateTimeSize = 8; // DateTime 是 8 字节

    /// <summary>
    /// 全局唯一标识符（GUID）的字节数。
    /// 128位（16字节）的唯一标识符
    /// 格式：xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    /// </summary>
    public const int GuidSize = 16; // Guid 是 16 字节

    /// <summary>
    /// 时间跨度类型变量的字节数。
    /// 表示时间间隔，精度为100纳秒（0.1微秒）
    /// 取值范围：-10,675,199天到10,675,199天
    /// </summary>
    public const int TimeSpanSize = 8; // TimeSpan 是 8 字节
}