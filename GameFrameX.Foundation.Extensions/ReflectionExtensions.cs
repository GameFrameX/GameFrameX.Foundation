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
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System.Reflection;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 反射扩展类，提供了一系列用于反射操作的扩展方法
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// 为 <see cref="System.Type"/> 与反射相关操作提供轻量扩展，统一不同运行时的 API 行为。
    /// </summary>
    /// <remarks>
    /// 该扩展集主要封装 <see cref="System.Reflection.TypeInfo"/> 在不同目标框架上的差异，
    /// 以便在 .NET Framework 与 .NET（Core/5+）之间获得一致的反射体验。
    /// </remarks>
    /// <seealso cref="System.Type"/>
    /// <seealso cref="System.Reflection.TypeInfo"/>
    /// <summary>
    /// 获取类型的 TypeInfo 相关行为的统一表示。对于 .NET 中 <see cref="System.Type"/> 即 <see cref="System.Reflection.TypeInfo"/> 的场景，直接返回输入类型。
    /// </summary>
    /// <param name="typeInfo">要获取元信息的类型实例。</param>
    /// <returns>与 <see cref="System.Reflection.TypeInfo"/> 行为兼容的类型对象（此处返回 <see cref="System.Type"/>）。</returns>
    /// <remarks>
    /// 在部分框架中 <see cref="System.Type"/> 与 <see cref="System.Reflection.TypeInfo"/> 一致；该方法提供统一调用入口。
    /// </remarks>
    /// <exception cref="System.NullReferenceException">当 <paramref name="typeInfo"/> 为 <see langword="null"/> 时。</exception>
    /// <seealso cref="System.Reflection.TypeInfo"/>
    public static Type GetTypeInfo(this Type typeInfo)
    {
        return typeInfo;
    }

    /// <summary>
    /// 获取类型的泛型参数集合。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <returns>包含泛型参数的 <see>
    ///     <cref>System.Type[]</cref>
    /// </see>
    /// ；非泛型类型返回空数组。</returns>
    /// <remarks>
    /// 示例：
    /// - 对 <see cref="System.Collections.Generic.List{T}"/>，其 {T} 将作为一个元素返回；
    /// - 对 <see cref="System.Nullable{T}"/>，返回对应的 {T}；
    /// - 非泛型类型返回长度为 0 的数组。
    /// </remarks>
    /// <exception cref="System.NullReferenceException">当 <paramref name="type"/> 为 <see langword="null"/> 时。</exception>
    /// <seealso cref="System.Type.GetGenericArguments"/>
    /// <seealso cref="System.Nullable{T}"/>
    /// <seealso cref="System.Collections.Generic.List{T}"/>
    public static Type[] GetGenericArguments(this Type type)
    {
        var reval = type.GetTypeInfo().GetGenericArguments();
        return reval;
    }

    /// <summary>
    /// 判断类型是否为泛型类型。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <returns>若为泛型类型返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    /// <remarks>
    /// 开放泛型与封闭泛型均视为泛型类型，例如 <see cref="System.Collections.Generic.List{T}"/> 与 <see>
    ///     <cref>System.Collections.Generic.List{System.String}</cref>
    /// </see>
    /// 。
    /// </remarks>
    /// <exception cref="System.NullReferenceException">当 <paramref name="type"/> 为 <see langword="null"/> 时。</exception>
    /// <seealso cref="System.Type.IsGenericType"/>
    public static bool IsGenericType(this Type type)
    {
        var reval = type.GetTypeInfo().IsGenericType;
        return reval;
    }

    /// <summary>
    /// 获取类型的公共实例属性集合。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <returns>属性数组 <see>
    ///     <cref>System.Reflection.PropertyInfo[]</cref>
    /// </see>
    /// 。</returns>
    /// <remarks>
    /// 等价于 <see>
    ///     <cref>System.Reflection.TypeInfo.GetProperties</cref>
    /// </see>
    /// 的行为。
    /// </remarks>
    /// <exception cref="System.NullReferenceException">当 <paramref name="type"/> 为 <see langword="null"/> 时。</exception>
    /// <seealso cref="System.Reflection.PropertyInfo"/>
    /// <seealso cref="System.Type.GetProperties()"/>
    public static PropertyInfo[] GetProperties(this Type type)
    {
        var reval = type.GetTypeInfo().GetProperties();
        return reval;
    }

    /// <summary>
    /// 按名称获取类型的公共实例属性。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <param name="name">属性名称，区分大小写。</param>
    /// <returns>匹配的 <see cref="System.Reflection.PropertyInfo"/>；未找到时返回 <see langword="null"/>。</returns>
    /// <remarks>
    /// 当存在多个重名属性（极少见）时，可能产生 <see cref="AmbiguousMatchException"/>。
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">当 <paramref name="name"/> 为 <see langword="null"/> 时。</exception>
    /// <exception cref="AmbiguousMatchException">当找到多个具有相同名称的属性时。</exception>
    /// <seealso cref="System.Reflection.PropertyInfo"/>
    /// <seealso cref="System.Type.GetProperty(System.String)"/>
    public static PropertyInfo GetProperty(this Type type, string name)
    {
        var reval = type.GetTypeInfo().GetProperty(name);
        return reval;
    }

    /// <summary>
    /// 按名称获取类型的公共实例字段。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <param name="name">字段名称，区分大小写。</param>
    /// <returns>匹配的 <see cref="System.Reflection.FieldInfo"/>；未找到时返回 <see langword="null"/>。</returns>
    /// <remarks>
    /// 当存在多个重名字段时，可能产生 <see cref="AmbiguousMatchException"/>。
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">当 <paramref name="name"/> 为 <see langword="null"/> 时。</exception>
    /// <exception cref="AmbiguousMatchException">当找到多个具有相同名称的字段时。</exception>
    /// <seealso cref="System.Reflection.FieldInfo"/>
    /// <seealso cref="System.Type.GetField(System.String)"/>
    public static FieldInfo GetField(this Type type, string name)
    {
        var reval = type.GetTypeInfo().GetField(name);
        return reval;
    }

    /// <summary>
    /// 判断类型是否为枚举类型。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <returns>若为枚举类型返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    /// <exception cref="System.NullReferenceException">当 <paramref name="type"/> 为 <see langword="null"/> 时。</exception>
    /// <seealso cref="System.Enum"/>
    public static bool IsEnum(this Type type)
    {
        var reval = type.GetTypeInfo().IsEnum;
        return reval;
    }

    /// <summary>
    /// 按名称获取类型的公共实例方法。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <param name="name">方法名称，区分大小写。</param>
    /// <returns>匹配的 <see cref="System.Reflection.MethodInfo"/>；未找到时返回 <see langword="null"/>。</returns>
    /// <remarks>
    /// 当存在多个重名方法且无法唯一选择时会抛出 <see cref="AmbiguousMatchException"/>。
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">当 <paramref name="name"/> 为 <see langword="null"/> 时。</exception>
    /// <exception cref="AmbiguousMatchException">当找到多个具有相同名称的方法时。</exception>
    /// <seealso cref="System.Reflection.MethodInfo"/>
    /// <seealso cref="System.Type.GetMethod(System.String)"/>
    public static MethodInfo GetMethod(this Type type, string name)
    {
        var reval = type.GetTypeInfo().GetMethod(name);
        return reval;
    }

    /// <summary>
    /// 使用参数类型签名获取类型的公共实例方法。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <param name="name">方法名称。</param>
    /// <param name="types">参数类型数组 <see>
    ///     <cref>System.Type[]</cref>
    /// </see>
    /// 。</param>
    /// <returns>匹配的 <see cref="System.Reflection.MethodInfo"/>；未找到时返回 <see langword="null"/>。</returns>
    /// <remarks>
    /// 传入 <paramref name="types"/> 可用于在重载中进行精确匹配；数组类型请使用 <see>
    ///     <cref>System.Type[]</cref>
    /// </see>
    /// ；
    /// 引用参数类型请使用 <c>System.Type&amp;</c>；指针参数类型请使用 <c>System.Type*</c>。
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">当 <paramref name="name"/> 或 <paramref name="types"/> 为 <see langword="null"/> 时。</exception>
    /// <exception cref="AmbiguousMatchException">当找到多个具有相同签名的方法时。</exception>
    /// <seealso>
    ///     <cref>System.Type[]</cref>
    /// </seealso>
    /// <seealso cref="System.Type.GetMethod(System.String,System.Type[])"/>
    public static MethodInfo GetMethod(this Type type, string name, Type[] types)
    {
        var reval = type.GetTypeInfo().GetMethod(name, types);
        return reval;
    }

    /// <summary>
    /// 使用参数类型签名获取类型的公共构造函数。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <param name="types">参数类型数组 <see>
    ///     <cref>System.Type[]</cref>
    /// </see>
    /// 。</param>
    /// <returns>匹配的 <see cref="System.Reflection.ConstructorInfo"/>；未找到时返回 <see langword="null"/>。</returns>
    /// <remarks>
    /// 传入 <paramref name="types"/> 可用于在重载中精确选择构造函数；引用参数类型请使用 <c>System.Type&amp;</c>，指针参数类型请使用 <c>System.Type*</c>。
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">当 <paramref name="types"/> 为 <see langword="null"/> 时。</exception>
    /// <exception cref="AmbiguousMatchException">当找到多个具有相同签名的构造函数时。</exception>
    /// <seealso cref="System.Reflection.ConstructorInfo"/>
    /// <seealso cref="System.Type.GetConstructor(System.Type[])"/>
    public static ConstructorInfo GetConstructor(this Type type, Type[] types)
    {
        var reval = type.GetTypeInfo().GetConstructor(types);
        return reval;
    }

    /// <summary>
    /// 判断类型是否为值类型。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <returns>若为值类型返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    /// <remarks>
    /// 包含结构体与枚举；不包含类与接口。
    /// </remarks>
    /// <seealso cref="System.ValueType"/>
    public static bool IsValueType(this Type type)
    {
        return type.GetTypeInfo().IsValueType;
    }

    /// <summary>
    /// 判断类型是否为类类型（引用类型）。
    /// </summary>
    /// <param name="type">要检查的类型。</param>
    /// <returns>若为类返回 <see langword="true"/>，否则返回 <see langword="false"/>。</returns>
    /// <remarks>
    /// 该方法封装 <see cref="System.Type.IsClass"/>；对于 <c>struct</c>/<c>enum</c> 返回 <see langword="false"/>。
    /// </remarks>
    /// <seealso cref="System.Type.IsClass"/>
    public static bool IsEntity(this Type type)
    {
        return type.GetTypeInfo().IsClass;
    }

    /// <summary>
    /// 获取方法的声明（反射）类型。
    /// </summary>
    /// <param name="method">方法元信息。</param>
    /// <returns>该方法的声明类型 <see cref="System.Type"/>。</returns>
    /// <exception cref="System.NullReferenceException">当 <paramref name="method"/> 为 <see langword="null"/> 时。</exception>
    /// <seealso cref="System.Reflection.MethodInfo.ReflectedType"/>
    public static Type ReflectedType(this MethodInfo method)
    {
        return method.ReflectedType;
    }
}