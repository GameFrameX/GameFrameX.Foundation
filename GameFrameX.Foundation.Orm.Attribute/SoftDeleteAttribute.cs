// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Orm.Attribute;

/// <summary>
/// 软删除特性，用于标记实体类支持软删除功能
/// </summary>
/// <remarks>
/// 此特性应用于实体类，用于标识该实体支持软删除功能。
/// 软删除是指在删除数据时不真正从数据库中移除记录，而是通过标记字段来表示数据已被删除。
/// 这样可以保留数据的完整性，支持数据恢复，并满足审计要求。
/// 
/// 软删除的优势：
/// - 数据安全：避免误删除造成的数据丢失
/// - 审计跟踪：保留删除操作的历史记录
/// - 数据恢复：支持已删除数据的恢复
/// - 关联完整性：保持外键关联的完整性
/// </remarks>
/// <example>
/// <code>
/// [SoftDelete("IsDeleted", "DeletedTime")]
/// public class User
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public bool IsDeleted { get; set; }
///     public DateTime? DeletedTime { get; set; }
///     public string? DeletedBy { get; set; }
/// }
/// 
/// [SoftDelete("Status", DeletedValue = "DELETED")]
/// public class Product
/// {
///     public int Id { get; set; }
///     public string Name { get; set; }
///     public string Status { get; set; } = "ACTIVE";
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class SoftDeleteAttribute : System.Attribute
{
    /// <summary>
    /// 获取或设置软删除标记字段名称
    /// </summary>
    /// <value>用于标记记录是否被删除的字段名称</value>
    public string DeletedField { get; set; }

    /// <summary>
    /// 获取或设置删除时间字段名称
    /// </summary>
    /// <value>记录删除时间的字段名称，可以为空</value>
    public string? DeletedTimeField { get; set; }

    /// <summary>
    /// 获取或设置删除用户字段名称
    /// </summary>
    /// <value>记录删除操作用户的字段名称，可以为空</value>
    public string? DeletedByField { get; set; }

    /// <summary>
    /// 获取或设置表示已删除的值
    /// </summary>
    /// <value>标记字段中表示已删除状态的值，默认为 true</value>
    public object DeletedValue { get; set; } = true;

    /// <summary>
    /// 获取或设置表示未删除的值
    /// </summary>
    /// <value>标记字段中表示未删除状态的值，默认为 false</value>
    public object NotDeletedValue { get; set; } = false;

    /// <summary>
    /// 获取或设置是否在查询时自动过滤已删除记录
    /// </summary>
    /// <value>指示是否在查询时自动排除已删除的记录，默认为 true</value>
    public bool AutoFilter { get; set; } = true;

    /// <summary>
    /// 初始化 <see cref="SoftDeleteAttribute"/> 类的新实例
    /// </summary>
    /// <param name="deletedField">软删除标记字段名称</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="deletedField"/> 为 null 时抛出</exception>
    public SoftDeleteAttribute(string deletedField)
    {
        DeletedField = deletedField ?? throw new ArgumentNullException(nameof(deletedField));
    }

    /// <summary>
    /// 初始化 <see cref="SoftDeleteAttribute"/> 类的新实例
    /// </summary>
    /// <param name="deletedField">软删除标记字段名称</param>
    /// <param name="deletedTimeField">删除时间字段名称</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="deletedField"/> 为 null 时抛出</exception>
    public SoftDeleteAttribute(string deletedField, string? deletedTimeField)
    {
        DeletedField = deletedField ?? throw new ArgumentNullException(nameof(deletedField));
        DeletedTimeField = deletedTimeField;
    }

    /// <summary>
    /// 初始化 <see cref="SoftDeleteAttribute"/> 类的新实例
    /// </summary>
    /// <param name="deletedField">软删除标记字段名称</param>
    /// <param name="deletedTimeField">删除时间字段名称</param>
    /// <param name="deletedByField">删除用户字段名称</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="deletedField"/> 为 null 时抛出</exception>
    public SoftDeleteAttribute(string deletedField, string? deletedTimeField, string? deletedByField)
    {
        DeletedField = deletedField ?? throw new ArgumentNullException(nameof(deletedField));
        DeletedTimeField = deletedTimeField;
        DeletedByField = deletedByField;
    }
}