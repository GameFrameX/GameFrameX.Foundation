using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GameFrameX.Foundation.Utility.DistributedSystem.Snowflake;
using SqlSugar;

namespace GameFrameX.Foundation.Orm.Entity;

/// <summary>
/// SqlSugar 辅助工具：集中处理 AOP、命名转换、列元数据等通用逻辑。
/// </summary>
public static class SqlSugarHelper
{
    /// <summary>
    /// 是否启用驼峰转下划线的命名转换。
    /// 当使用MongoDB时，不要启用此选项。
    /// </summary>
    /// <remarks>
    /// 开启后，会对表名与列名进行驼峰转下划线处理（仅在当前值不包含下划线时），以统一数据库命名风格。
    /// </remarks>
    public static bool IsEnableTableNameUnderLine = false;

    /// <summary>
    /// 是否启用“列名”的驼峰转下划线命名转换。
    /// 当使用MongoDB时，不要启用此选项。
    /// </summary>
    /// <remarks>
    /// 启用后在 <see cref="EntityService"/> 内对非忽略列进行转换（当前列名不含下划线时），
    /// 与 <see cref="IsEnableTableNameUnderLine"/> 分别控制表名与列名两类转换。
    /// </remarks>
    public static bool IsEnableColumnNameUnderLine = false;

    /// <summary>
    /// 数据执行后的 AOP 钩子委托。
    /// </summary>
    /// <value>
    /// 在数据执行操作完成后调用的委托，接收原始值和实体列上下文信息作为参数。
    /// </value>
    /// <remarks>
    /// 此委托在 <see cref="AopDataExecuting"/> 方法的最后阶段被调用，
    /// 可用于执行数据操作后的自定义逻辑，如日志记录、缓存更新等。
    /// </remarks>
    /// <seealso cref="AopDataExecutingBefore"/>
    /// <seealso cref="AopDataExecuting"/>
    public static Action<object, DataFilterModel> AopDataExecutingAfter = null!;

    /// <summary>
    /// 数据执行前的 AOP 钩子委托。
    /// </summary>
    /// <value>
    /// 在数据执行操作开始前调用的委托，接收原始值和实体列上下文信息作为参数。
    /// </value>
    /// <remarks>
    /// 此委托在 <see cref="AopDataExecuting"/> 方法的开始阶段被调用，
    /// 可用于执行数据操作前的自定义逻辑，如数据验证、权限检查等。
    /// </remarks>
    /// <seealso cref="AopDataExecutingAfter"/>
    /// <seealso cref="AopDataExecuting"/>
    public static Action<object, DataFilterModel> AopDataExecutingBefore = null!;

    /// <summary>
    /// 数据执行阶段的 AOP 钩子：在插入与更新时自动处理主键与审计字段。
    /// </summary>
    /// <param name="oldValue">原始值</param>
    /// <param name="entityInfo">当前操作的实体列上下文信息</param>
    /// <remarks>
    /// 插入（InsertByObject）：
    /// - 如果主键为空，根据类型自动生成主键（long→雪花 ID，Guid→新 Guid，string→Guid 字符串）。
    /// - 自动设置 <see cref="EntityBase.CreateTime"/> 为当前 UTC 时间。
    /// 更新（UpdateByObject）：
    /// - 自动设置 <see cref="EntityBase.UpdateTime"/> 为当前 UTC 时间。
    /// </remarks>
    public static void AopDataExecuting(object oldValue, DataFilterModel entityInfo)
    {
        AopDataExecutingBefore?.Invoke(oldValue, entityInfo);

        if (entityInfo.OperationType == DataFilterType.InsertByObject)
        {
            // 如果是主键列，且未赋值，则按类型生成主键值
            if (entityInfo.EntityColumnInfo.IsPrimarykey)
            {
                if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                {
                    // long 主键：使用雪花 ID
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (long)id == 0)
                    {
                        entityInfo.SetValue(SnowFlakeIdHelper.Instance.NextId());
                    }
                }
                else if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(Guid))
                {
                    // Guid 主键：使用新 Guid
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (long)id == 0)
                    {
                        entityInfo.SetValue(Guid.NewGuid());
                    }
                }
                else if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(string))
                {
                    // string 主键：使用 Guid 字符串（去短横）
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (long)id == 0)
                    {
                        entityInfo.SetValue(Guid.NewGuid().ToString("N"));
                    }
                }
            }

            // 自动填充创建时间
            if (entityInfo.PropertyName == nameof(EntityBase.CreateTime))
            {
                entityInfo.SetValue(DateTime.UtcNow);
            }
        }

        if (entityInfo.OperationType == DataFilterType.UpdateByObject)
        {
            // 自动填充更新时间
            if (entityInfo.PropertyName == nameof(EntityBase.UpdateTime))
            {
                entityInfo.SetValue(DateTime.UtcNow);
            }
        }

        AopDataExecutingAfter?.Invoke(oldValue, entityInfo);
    }

    /// <summary>
    /// 实体名称服务：处理表名映射与删除保护。
    /// </summary>
    /// <param name="type">实体类型</param>
    /// <param name="entity">实体元信息</param>
    /// <remarks>
    /// - 设置 <see cref="EntityInfo.IsDisabledDelete"/> 防止删除非 SqlSugar 创建的列。
    /// - 当启用 <see cref="IsEnableTableNameUnderLine"/> 时，按需将驼峰命名转换为下划线。
    /// </remarks>
    public static void EntityNameService(Type type, EntityInfo entity)
    {
        entity.IsDisabledDelete = true; // 禁止删除非 sqlsugar 创建的列

        if (IsEnableTableNameUnderLine)
        {
            // 对所有表名进行驼峰转下划线处理（仅在当前值不包含下划线时）
            if (!entity.DbTableName.Contains('_'))
            {
                entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
            }
        }
    }

    /// <summary>
    /// 实体列服务：处理主键、可空性、列注释与命名转换。
    /// </summary>
    /// <param name="type">属性反射信息</param>
    /// <param name="column">实体列元信息</param>
    /// <remarks>
    /// - 检查 <see cref="KeyAttribute"/> 标记以设定主键，并强制主键不可空。
    /// - 可空性判定：优先基于泛型类型 <c>Nullable&lt;&gt;</c> 或 <see cref="NullabilityInfoContext"/>；
    ///   若存在 <see cref="RequiredAttribute"/> 则覆盖为不可空。
    /// - 列注释：如果存在 <see cref="DescriptionAttribute"/>，则设置列描述。
    /// - 命名转换：启用 <see cref="IsEnableColumnNameUnderLine"/> 时，非忽略列按需驼峰转下划线。
    /// </remarks>
    public static void EntityService(PropertyInfo type, EntityColumnInfo column)
    {
        // 处理主键标识
        if (type.GetCustomAttributes<KeyAttribute>().Any())
        {
            column.IsPrimarykey = true;
        }

        // 主键不能为空
        if (column.IsPrimarykey)
        {
            column.IsNullable = false;
        }
        else
        {
            // 处理非主键的可空性 - 先检查泛型可空类型
            if (type.PropertyType.IsGenericType && type.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                column.IsNullable = true;
            }
            // 再检查引用类型的可空性注解（C# 8 可空上下文）
            else if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
            {
                column.IsNullable = true;
            }

            // Required 特性会覆盖之前的可空设置
            if (type.GetCustomAttributes<RequiredAttribute>().Any())
            {
                column.IsNullable = false;
            }
        }

        // 处理注释（列描述）
        if (type.GetCustomAttributes<DescriptionAttribute>().Any())
        {
            column.ColumnDescription = type.GetCustomAttributes<DescriptionAttribute>().First().Description;
        }

        if (IsEnableColumnNameUnderLine)
        {
            // 对所有列名进行驼峰转下划线处理（除了被忽略的列，且当前列名不包含下划线）
            if (!column.IsIgnore && !column.DbColumnName.Contains('_'))
            {
                column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线
            }
        }
    }
}