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

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 敏感字段特性，用于标记字段在列表展示/导出/日志中自动脱敏。
/// </summary>
/// <remarks>
/// Sensitive field attribute for marking fields to be automatically masked in list views, exports, and logs.
/// 标记后，序列化/导出/日志写入时按指定策略对字段值打码，防止敏感信息泄露，
/// 实现“库内明文、出库脱敏”。
/// </remarks>
/// <example>
/// <code>
/// public class GamePlayer
/// {
///     public long Id { get; set; }
///
///     [SensitiveField(MaskingStrategy.Phone)]
///     public string Phone { get; set; }
///
///     [SensitiveField(MaskingStrategy.IdCard)]
///     public string IdCard { get; set; }
///
///     [SensitiveField(MaskingStrategy.Email)]
///     public string Email { get; set; }
///
///     [SensitiveField(MaskingStrategy.Custom, KeepPrefix = 1, KeepSuffix = 1)]
///     public string Nickname { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class SensitiveFieldAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置脱敏策略。
    /// </summary>
    /// <remarks>
    /// Gets or sets the masking strategy applied to the field value.
    /// </remarks>
    /// <value>字段值的脱敏策略，默认为 <see cref="MaskingStrategy.None"/> / Masking strategy for the field value, default is <see cref="MaskingStrategy.None"/></value>
    public MaskingStrategy Strategy { get; set; } = MaskingStrategy.None;

    /// <summary>
    /// 获取或设置保留前缀字符数（仅 <see cref="MaskingStrategy.Custom"/> 策略生效）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the number of leading characters to keep. Only effective when strategy is <see cref="MaskingStrategy.Custom"/>.
    /// </remarks>
    /// <value>保留前缀的字符数量 / Number of leading characters to keep</value>
    public int KeepPrefix { get; set; }

    /// <summary>
    /// 获取或设置保留后缀字符数（仅 <see cref="MaskingStrategy.Custom"/> 策略生效）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the number of trailing characters to keep. Only effective when strategy is <see cref="MaskingStrategy.Custom"/>.
    /// </remarks>
    /// <value>保留后缀的字符数量 / Number of trailing characters to keep</value>
    public int KeepSuffix { get; set; }

    /// <summary>
    /// 获取或设置自定义脱敏正则表达式（仅 <see cref="MaskingStrategy.Custom"/> 策略生效）。
    /// </summary>
    /// <remarks>
    /// Gets or sets the custom regex pattern for masking. Only effective when strategy is <see cref="MaskingStrategy.Custom"/>.
    /// </remarks>
    /// <value>自定义脱敏正则表达式，可为空 / Custom regex pattern for masking, can be null</value>
    public string? CustomPattern { get; set; }

    /// <summary>
    /// 初始化 <see cref="SensitiveFieldAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SensitiveFieldAttribute"/> class.
    /// </remarks>
    public SensitiveFieldAttribute()
    {
    }

    /// <summary>
    /// 初始化 <see cref="SensitiveFieldAttribute"/> 类的新实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SensitiveFieldAttribute"/> class with the specified masking strategy.
    /// </remarks>
    /// <param name="strategy">脱敏策略 / The masking strategy</param>
    public SensitiveFieldAttribute(MaskingStrategy strategy)
    {
        Strategy = strategy;
    }
}

/// <summary>
/// 脱敏策略枚举。
/// </summary>
/// <remarks>
/// Masking strategy enumeration.
/// </remarks>
public enum MaskingStrategy
{
    /// <summary>
    /// 不脱敏。
    /// </summary>
    /// <remarks>
    /// No masking.
    /// </remarks>
    None = 0,

    /// <summary>
    /// 手机号脱敏（保留前 3 位和后 4 位）。
    /// </summary>
    /// <remarks>
    /// Phone number masking (keep first 3 and last 4 digits).
    /// </remarks>
    Phone = 1,

    /// <summary>
    /// 身份证号脱敏（保留前 6 位和后 4 位）。
    /// </summary>
    /// <remarks>
    /// ID card number masking (keep first 6 and last 4 digits).
    /// </remarks>
    IdCard = 2,

    /// <summary>
    /// 邮箱脱敏（保留首位字符与域名）。
    /// </summary>
    /// <remarks>
    /// Email masking (keep first character and domain).
    /// </remarks>
    Email = 3,

    /// <summary>
    /// 银行卡号脱敏（保留后 4 位）。
    /// </summary>
    /// <remarks>
    /// Bank card number masking (keep last 4 digits).
    /// </remarks>
    BankCard = 4,

    /// <summary>
    /// 姓名脱敏（保留姓氏）。
    /// </summary>
    /// <remarks>
    /// Person name masking (keep the surname).
    /// </remarks>
    Name = 5,

    /// <summary>
    /// 地址脱敏（保留省市区，详细地址打码）。
    /// </summary>
    /// <remarks>
    /// Address masking (keep region, mask detail).
    /// </remarks>
    Address = 6,

    /// <summary>
    /// 自定义脱敏规则（配合 KeepPrefix/KeepSuffix/CustomPattern）。
    /// </summary>
    /// <remarks>
    /// Custom masking rule (used with KeepPrefix/KeepSuffix/CustomPattern).
    /// </remarks>
    Custom = 7
}
