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

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 常量型变量的字节数。
/// 提供了常用基础数据类型的字节大小常量值。
/// </summary>
/// <remarks>
/// Byte sizes of constant types.
/// Provides constant values for the byte sizes of common base data types.
/// </remarks>
public static class ConstBaseTypeSize
{
    /// <summary>
    /// 整型变量（32 位）的字节数。
    /// 取值范围：-2,147,483,648 到 2,147,483,647。
    /// </summary>
    /// <remarks>
    /// Byte size of integer variable (32-bit).
    /// Value range: -2,147,483,648 to 2,147,483,647.
    /// </remarks>
    public const int IntSize = sizeof(int);

    /// <summary>
    /// 短整型变量（16 位）的字节数。
    /// 取值范围：-32,768 到 32,767。
    /// </summary>
    /// <remarks>
    /// Byte size of short integer variable (16-bit).
    /// Value range: -32,768 to 32,767.
    /// </remarks>
    public const int ShortSize = sizeof(short);

    /// <summary>
    /// 长整型变量（64 位）的字节数。
    /// 取值范围：-9,223,372,036,854,775,808 到 9,223,372,036,854,775,807。
    /// </summary>
    /// <remarks>
    /// Byte size of long integer variable (64-bit).
    /// Value range: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807.
    /// </remarks>
    public const int LongSize = sizeof(long);

    /// <summary>
    /// 单精度浮点型变量的字节数。
    /// 精度：约 7 位十进制数字。
    /// 取值范围：±1.5 x 10^-45 到 ±3.4 x 10^38。
    /// </summary>
    /// <remarks>
    /// Byte size of single-precision floating-point variable.
    /// Precision: approximately 7 decimal digits.
    /// Value range: ±1.5 x 10^-45 to ±3.4 x 10^38.
    /// </remarks>
    public const int FloatSize = sizeof(float);

    /// <summary>
    /// 双精度浮点型变量的字节数。
    /// 精度：约 15-17 位十进制数字。
    /// 取值范围：±5.0 × 10^-324 到 ±1.7 × 10^308。
    /// </summary>
    /// <remarks>
    /// Byte size of double-precision floating-point variable.
    /// Precision: approximately 15-17 decimal digits.
    /// Value range: ±5.0 × 10^-324 to ±1.7 × 10^308.
    /// </remarks>
    public const int DoubleSize = sizeof(double);

    /// <summary>
    /// 字节型变量（8 位）的字节数。
    /// 取值范围：0 到 255。
    /// 常用于处理二进制数据和文件操作。
    /// </summary>
    /// <remarks>
    /// Byte size of byte variable (8-bit).
    /// Value range: 0 to 255.
    /// Commonly used for handling binary data and file operations.
    /// </remarks>
    public const int ByteSize = sizeof(byte);

    /// <summary>
    /// 有符号字节类型变量的字节数。
    /// 取值范围：-128 到 127。
    /// </summary>
    /// <remarks>
    /// Byte size of signed byte variable.
    /// Value range: -128 to 127.
    /// </remarks>
    public const int SbyteSize = sizeof(sbyte);

    /// <summary>
    /// 布尔型变量的字节数。
    /// 仅存储 true 或 false 两种状态。
    /// </summary>
    /// <remarks>
    /// Byte size of boolean variable.
    /// Stores only two states: true or false.
    /// </remarks>
    public const int BoolSize = sizeof(bool);

    /// <summary>
    /// 无符号整型变量（32 位）的字节数。
    /// 取值范围：0 到 4,294,967,295。
    /// </summary>
    /// <remarks>
    /// Byte size of unsigned integer variable (32-bit).
    /// Value range: 0 to 4,294,967,295.
    /// </remarks>
    public const int UIntSize = sizeof(uint);

    /// <summary>
    /// 无符号短整型变量（16 位）的字节数。
    /// 取值范围：0 到 65,535。
    /// </summary>
    /// <remarks>
    /// Byte size of unsigned short integer variable (16-bit).
    /// Value range: 0 to 65,535.
    /// </remarks>
    public const int UShortSize = sizeof(ushort);

    /// <summary>
    /// 无符号长整型变量（64 位）的字节数。
    /// 取值范围：0 到 18,446,744,073,709,551,615。
    /// </summary>
    /// <remarks>
    /// Byte size of unsigned long integer variable (64-bit).
    /// Value range: 0 to 18,446,744,073,709,551,615.
    /// </remarks>
    public const int ULongSize = sizeof(ulong);

    /// <summary>
    /// 字符型变量（16 位 Unicode）的字节数。
    /// 取值范围：U+0000 到 U+FFFF。
    /// 用于存储单个 Unicode 字符。
    /// </summary>
    /// <remarks>
    /// Byte size of character variable (16-bit Unicode).
    /// Value range: U+0000 to U+FFFF.
    /// Used for storing a single Unicode character.
    /// </remarks>
    public const int CharSize = sizeof(char);

    /// <summary>
    /// 高精度十进制浮点型变量的字节数。
    /// 精度：28-29 位十进制数字。
    /// 取值范围：±1.0 × 10^-28 到 ±7.9228 × 10^28。
    /// 适用于金融计算等需要高精度的场景。
    /// </summary>
    /// <remarks>
    /// Byte size of high-precision decimal floating-point variable.
    /// Precision: 28-29 decimal digits.
    /// Value range: ±1.0 × 10^-28 to ±7.9228 × 10^28.
    /// Suitable for scenarios requiring high precision such as financial calculations.
    /// </remarks>
    public const int DecimalSize = sizeof(decimal);

    /// <summary>
    /// 日期时间类型变量的字节数。
    /// 表示从公元 1 年 1 月 1 日午夜 12:00:00 到 9999 年 12 月 31 日晚上 11:59:59 之间的日期和时间。
    /// 精度：100 纳秒（0.1 微秒）。
    /// </summary>
    /// <remarks>
    /// Byte size of DateTime variable.
    /// Represents dates and times from midnight 12:00:00, January 1, 1 AD to 11:59:59 PM, December 31, 9999 AD.
    /// Precision: 100 nanoseconds (0.1 microseconds).
    /// </remarks>
    public const int DateTimeSize = 8; // DateTime 是 8 字节

    /// <summary>
    /// 全局唯一标识符（GUID）的字节数。
    /// 128 位（16 字节）的唯一标识符。
    /// 格式：xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx。
    /// </summary>
    /// <remarks>
    /// Byte size of Globally Unique Identifier (GUID).
    /// 128-bit (16-byte) unique identifier.
    /// Format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.
    /// </remarks>
    public const int GuidSize = 16; // Guid 是 16 字节

    /// <summary>
    /// 时间跨度类型变量的字节数。
    /// 表示时间间隔，精度为 100 纳秒（0.1 微秒）。
    /// 取值范围：-10,675,199 天到 10,675,199 天。
    /// </summary>
    /// <remarks>
    /// Byte size of TimeSpan variable.
    /// Represents a time interval with a precision of 100 nanoseconds (0.1 microseconds).
    /// Value range: -10,675,199 days to 10,675,199 days.
    /// </remarks>
    public const int TimeSpanSize = 8; // TimeSpan 是 8 字节
}