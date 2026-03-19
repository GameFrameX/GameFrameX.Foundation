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

using System.Linq;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 类型扩展类，提供了一系列用于类型检查和判断的扩展方法。
/// </summary>
/// <remarks>
/// Provides extension methods for type checking and validation.
/// </remarks>
public static partial class TypeExtensions
{
    /// <summary>
    /// 判断类型是否实现了指定的接口。
    /// </summary>
    /// <remarks>
    /// Checks whether the type implements the specified interface.
    /// This method checks both direct and indirect interface implementations by recursively examining the type's inheritance chain.
    /// </remarks>
    /// <param name="targetType">要检查的类型，不能为null / The type to check, cannot be null.</param>
    /// <param name="interfaceType">目标接口类型，不能为null / The interface type to check for, cannot be null.</param>
    /// <returns>如果类型实现了指定的接口，则返回true；否则返回false / true if the type implements the specified interface; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="targetType"/> 或 <paramref name="interfaceType"/> 为 null 时抛出 / Thrown when <paramref name="targetType"/> or <paramref name="interfaceType"/> is null.</exception>
    /// <exception cref="ArgumentException">当 <paramref name="interfaceType"/> 不是接口类型时抛出 / Thrown when <paramref name="interfaceType"/> is not an interface type.</exception>
    public static bool HasInterface(Type targetType, Type interfaceType)
    {
        ArgumentNullException.ThrowIfNull(targetType, nameof(targetType));
        ArgumentNullException.ThrowIfNull(interfaceType, nameof(interfaceType));

        if (!interfaceType.IsInterface)
        {
            throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.TargetTypeMustBeInterface), nameof(interfaceType));
        }

        return interfaceType.IsAssignableFrom(targetType);
    }

    /// <summary>
    /// 判断类型是否实现某个泛型接口或继承自某个泛型类。
    /// </summary>
    /// <remarks>
    /// Checks whether the type implements a specific generic interface or inherits from a specific generic class.
    /// This method recursively checks:
    /// 1. All interfaces implemented by the type
    /// 2. The inheritance chain of base classes
    /// 3. For generic types, extracts the generic type definition for comparison
    /// </remarks>
    /// <param name="type">要检查的类型，不能为null / The type to check, cannot be null.</param>
    /// <param name="generic">目标泛型类型，不能为null / The generic type to check for, cannot be null.</param>
    /// <returns>如果类型实现了指定的泛型接口或继承自指定的泛型类，则返回true；否则返回false / true if the type implements the specified generic interface or inherits from the specified generic class; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="type"/> 或 <paramref name="generic"/> 为 null 时抛出 / Thrown when <paramref name="type"/> or <paramref name="generic"/> is null.</exception>
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
    /// <remarks>
    /// Checks whether a type implements the specified interface.
    /// This method is used to check if a concrete type implements a target interface.
    ///
    /// Usage scenarios:
    /// 1. In reflection to determine if a type can be used for a specific interface implementation
    /// 2. In dependency injection scenarios to validate service implementation
    /// 3. In plugin systems to check if a plugin type meets requirements
    ///
    /// Parameter combinations:
    /// - directOnly=true, checkIndirectInterfaces=false: Only check directly implemented interfaces
    /// - directOnly=false, checkIndirectInterfaces=false: Check all implemented interfaces without checking interface inheritance
    /// - directOnly=false, checkIndirectInterfaces=true: Check all implemented interfaces and interface inheritance
    /// </remarks>
    /// <param name="self">要判断的类型。必须是非空的具体类型 / The type to check. Must be a non-null concrete type.</param>
    /// <param name="target">要判断的接口类型。必须是非空的接口类型 / The interface type to check for. Must be a non-null interface type.</param>
    /// <param name="directOnly">是否只检查直接实现的接口，不检查继承的接口 / Whether to only check directly implemented interfaces without checking inherited interfaces.</param>
    /// <param name="checkIndirectInterfaces">是否检查接口之间的继承关系 / Whether to check inheritance relationships between interfaces.</param>
    /// <returns>如果self实现了target接口，则返回true；否则返回false / true if self implements the target interface; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="target"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="target"/> is null.</exception>
    /// <exception cref="ArgumentException">当 <paramref name="target"/> 不是接口类型时抛出 / Thrown when <paramref name="target"/> is not an interface type.</exception>
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