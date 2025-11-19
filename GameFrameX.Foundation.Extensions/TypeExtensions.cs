// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

using System.Linq;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 类型扩展类，提供了一系列用于类型检查和判断的扩展方法
/// </summary>
public static partial class TypeExtensions
{
    /// <summary>
    /// 判断类型是否实现了指定的接口
    /// </summary>
    /// <param name="targetType">要检查的类型，不能为null</param>
    /// <param name="interfaceType">目标接口类型，不能为null</param>
    /// <returns>
    /// 如果类型实现了指定的接口，则返回true；
    /// 否则返回false
    /// </returns>
    /// <remarks>
    /// 此方法会检查类型是否直接或间接地实现了指定的接口。
    /// 它会递归检查类型的继承链和接口实现。
    /// </remarks>
    /// <exception cref="ArgumentNullException">当<paramref name="targetType"/>或<paramref name="interfaceType"/>为null时抛出</exception>
    public static bool HasInterface(Type targetType, Type interfaceType)
    {
        if (targetType == null || interfaceType == null || !interfaceType.IsInterface)
        {
            return false;
        }

        return interfaceType.IsAssignableFrom(targetType);
    }

    /// <summary>
    /// 判断类型是否实现某个泛型接口或继承自某个泛型类
    /// </summary>
    /// <param name="type">要检查的类型，不能为null</param>
    /// <param name="generic">目标泛型类型，不能为null</param>
    /// <returns>
    /// 如果类型实现了指定的泛型接口或继承自指定的泛型类，则返回true；
    /// 否则返回false
    /// </returns>
    /// <remarks>
    /// 此方法会递归检查类型的继承链和接口实现：
    /// 1. 首先检查类型实现的所有接口
    /// 2. 然后沿着继承链向上检查基类
    /// 3. 对于泛型类型，会提取其泛型类型定义进行比较
    /// </remarks>
    /// <exception cref="ArgumentNullException">当<paramref name="type"/>或<paramref name="generic"/>为null时抛出</exception>
    public static bool HasImplementedRawGeneric(this Type type, Type generic)
    {
        // 参数检查
        ArgumentNullException.ThrowIfNull(type, nameof(type));
        ArgumentNullException.ThrowIfNull(generic, nameof(generic));

        // 检查接口类型
        var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
        if (isTheRawGenericType)
        {
            return true;
        }

        // 检查类型
        while (type != null && type != typeof(object))
        {
            isTheRawGenericType = IsTheRawGenericType(type);
            if (isTheRawGenericType)
            {
                return true;
            }

            type = type.BaseType;
        }

        return false;

        // 判断逻辑
        bool IsTheRawGenericType(Type rawGenericType)
        {
            return generic == (rawGenericType.IsGenericType ? rawGenericType.GetGenericTypeDefinition() : rawGenericType);
        }
    }

    /// <summary>
    /// 判断类型是否实现了指定的接口。
    /// 此方法用于检查一个具体类型是否实现了目标接口。
    /// </summary>
    /// <param name="self">要判断的类型。必须是非空的具体类型。</param>
    /// <param name="target">要判断的接口类型。必须是非空的接口类型。</param>
    /// <param name="directOnly">是否只检查直接实现的接口，不检查继承的接口。
    /// 当设置为true时，只检查直接实现的接口；
    /// 当设置为false时，同时检查继承链上的所有接口。</param>
    /// <param name="checkIndirectInterfaces">是否检查接口之间的继承关系。
    /// 当设置为true时，会检查接口之间的继承关系；
    /// 当设置为false时，只检查类型直接实现的接口列表。</param>
    /// <returns>
    /// 如果self实现了target接口（根据directOnly和checkIndirectInterfaces参数决定检查范围），则返回true；
    /// 否则返回false
    /// </returns>
    /// <remarks>
    /// 使用场景示例：
    /// 1. 在反射中判断某个类型是否可以用于特定接口的实现
    /// 2. 在依赖注入场景中验证服务实现的有效性
    /// 3. 在插件系统中检查插件类型是否符合要求
    /// 
    /// 参数组合效果：
    /// - directOnly=true, checkIndirectInterfaces=false：只检查类型直接实现的接口
    /// - directOnly=false, checkIndirectInterfaces=false：检查类型实现的所有接口，但不检查接口之间的继承关系
    /// - directOnly=false, checkIndirectInterfaces=true：检查类型实现的所有接口，并检查接口之间的继承关系
    /// </remarks>
    /// <exception cref="ArgumentNullException">当<paramref name="self"/>或<paramref name="target"/>为null时抛出</exception>
    /// <exception cref="ArgumentException">当<paramref name="target"/>不是接口类型时抛出</exception>
    public static bool IsImplWithInterface(this Type self, Type target, bool directOnly = false, bool checkIndirectInterfaces = true)
    {
        // 参数有效性检查
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(target, nameof(target));

        // 确保target是接口类型
        if (!target.IsInterface)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.TargetTypeMustBeInterface), nameof(target));
        }

        // 检查是否是接口类型或抽象类型
        if (self.IsInterface || self.IsAbstract)
        {
            return false;
        }

        // 获取类型实现的所有接口列表
        var allInterfaces = self.GetInterfaces();

        // 只检查直接实现的接口
        if (directOnly)
        {
            return allInterfaces.Any(i => i == target);
        }

        // 获取直接实现的接口（排除接口继承的接口）
        var directInterfaces = GetDirectInterfaces(self);

        // 如果不检查接口间的继承关系，只检查直接实现的接口
        if (!checkIndirectInterfaces)
        {
            return directInterfaces.Contains(target);
        }

        // 检查所有实现的接口
        if (allInterfaces.Contains(target))
        {
            return true;
        }

        // 检查接口间的继承关系
        foreach (var interfaceType in allInterfaces)
        {
            if (IsInterfaceImplemented(interfaceType, target))
            {
                return true;
            }
        }


        return false;
    }

    /// <summary>
    /// 获取类型直接实现的接口（排除从接口继承的接口）
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    static Type[] GetDirectInterfaces(Type type)
    {
        var allInterfaces = type.GetInterfaces();
        var inheritedInterfaces = allInterfaces
                                  .SelectMany(i => i.GetInterfaces())
                                  .Distinct()
                                  .ToArray();

        return allInterfaces
               .Except(inheritedInterfaces)
               .ToArray();
    }

    /// <summary>
    /// 递归检查接口是否实现了目标接口
    /// </summary>
    /// <param name="interfaceType"></param>
    /// <param name="targetInterface"></param>
    /// <returns></returns>
    static bool IsInterfaceImplemented(Type interfaceType, Type targetInterface)
    {
        if (interfaceType == targetInterface)
        {
            return true;
        }

        // 检查泛型接口的特殊情况
        if (interfaceType.IsGenericType && targetInterface.IsGenericType == false)
        {
            // 例如：IComparable<T>实现了IComparable
            var baseInterfaces = interfaceType.GetInterfaces();
            return baseInterfaces.Any(i => i == targetInterface || IsInterfaceImplemented(i, targetInterface));
        }

        // 常规接口继承检查
        return interfaceType.GetInterfaces().Any(i => i == targetInterface || IsInterfaceImplemented(i, targetInterface));
    }
}