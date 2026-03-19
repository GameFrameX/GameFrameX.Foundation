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

using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Extensions.Localization;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 表示可为空的对象。
/// </summary>
/// <remarks>
/// Represents an object that can be null.
/// </remarks>
/// <typeparam name="T">对象的类型 / The type of the object.</typeparam>
public readonly record struct NullObject<T> : IComparable, IComparable<T>, IEquatable<NullObject<T>>
{
    /// <summary>
    /// 初始化一个新的 <see cref="NullObject{T}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NullObject{T}" /> struct.
    /// </remarks>
    /// <param name="item">对象的值 / The value of the object.</param>
    public NullObject(T item)
    {
        Item = item;
    }

    /// <summary>
    /// 获取一个表示空值的 <see cref="NullObject{T}" /> 实例。
    /// </summary>
    /// <remarks>
    /// Gets a <see cref="NullObject{T}" /> instance representing a null value.
    /// </remarks>
    public static NullObject<T> Null
    {
        get { return new NullObject<T>(); }
    }

    /// <summary>
    /// 获取对象的值。
    /// </summary>
    /// <remarks>
    /// Gets the value of the object.
    /// </remarks>
    /// <value>对象的值 / The value of the object.</value>
    public T Item { get; }

    /// <summary>
    /// 比较当前对象与另一个对象。
    /// </summary>
    /// <remarks>
    /// Compares the current object with another object.
    /// </remarks>
    /// <param name="value">要比较的对象 / The object to compare with.</param>
    /// <returns>一个整数，指示当前对象与 <paramref name="value" /> 的相对顺序 / An integer indicating the relative order of the current object and <paramref name="value" />.</returns>
    /// <exception cref="ArgumentException">当 Item 为 null 且无法进行比较时抛出 / Thrown when Item is null and cannot be compared.</exception>
    public int CompareTo(object value)
    {
        if (value is null)
        {
            return Item is null ? 0 : 1;
        }

        if (value is NullObject<T> nullObject)
        {
            if (Item is null && nullObject.Item is null)
            {
                return 0;
            }

            if (Item is null)
            {
                return -1;
            }

            if (nullObject.Item is null)
            {
                return 1;
            }

            if (Item is IComparable comparable)
            {
                return comparable.CompareTo(nullObject.Item);
            }

            return string.Compare(Item?.ToString(), nullObject.Item?.ToString(), StringComparison.Ordinal);
        }

        if (value is T directValue)
        {
            return CompareTo(directValue);
        }

        throw new ArgumentException(LocalizationService.GetString(LocalizationKeys.Exceptions.ObjectTypeMismatch), nameof(value));
    }

    /// <summary>
    /// 比较当前对象与同一类型的另一个对象。
    /// </summary>
    /// <remarks>
    /// Compares the current object with another object of the same type.
    /// </remarks>
    /// <param name="other">要比较的对象 / The object to compare with.</param>
    /// <returns>一个整数，指示当前对象与 <paramref name="other" /> 的相对顺序 / An integer indicating the relative order of the current object and <paramref name="other" />.</returns>
    /// <exception cref="ArgumentException">当 Item 为 null 且无法进行比较时抛出 / Thrown when Item is null and cannot be compared.</exception>
    public int CompareTo(T other)
    {
        if (Item is null && other is null)
        {
            return 0;
        }

        if (Item is null)
        {
            return -1;
        }

        if (other is null)
        {
            return 1;
        }

        if (Item is IComparable comparable)
        {
            return comparable.CompareTo(other);
        }

        return string.Compare(Item.ToString(), other.ToString(), StringComparison.Ordinal);
    }

    /// <summary>
    /// 指示当前对象是否等于同一类型的另一个对象。
    /// </summary>
    /// <remarks>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </remarks>
    /// <param name="other">一个与此对象进行比较的对象 / An object to compare with this object.</param>
    /// <returns>如果当前对象等于 <paramref name="other" /> 参数，则为 <c>true</c>；否则为 <c>false</c> / <c>true</c> if the current object equals the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
    public bool Equals(NullObject<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Item, other.Item);
    }

    /// <summary>
    /// 将 <see cref="NullObject{T}" /> 隐式转换为类型 <typeparamref name="T" />。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a <see cref="NullObject{T}" /> to type <typeparamref name="T" />.
    /// </remarks>
    /// <param name="nullObject">要转换的 <see cref="NullObject{T}" /> 实例 / The <see cref="NullObject{T}" /> instance to convert.</param>
    public static implicit operator T(NullObject<T> nullObject)
    {
        return nullObject.Item;
    }

    /// <summary>
    /// 将类型 <typeparamref name="T" /> 隐式转换为 <see cref="NullObject{T}" />。
    /// </summary>
    /// <remarks>
    /// Implicitly converts a value of type <typeparamref name="T" /> to <see cref="NullObject{T}" />.
    /// </remarks>
    /// <param name="item">要转换的值 / The value to convert.</param>
    public static implicit operator NullObject<T>(T item)
    {
        return new NullObject<T>(item);
    }

    /// <summary>
    /// 返回对象的字符串表示形式。
    /// </summary>
    /// <remarks>
    /// Returns the string representation of the object.
    /// </remarks>
    /// <returns>对象的字符串表示形式，如果对象为 null，则返回 "NULL" / The string representation of the object, or "NULL" if the object is null.</returns>
    public override string ToString()
    {
        return Item != null ? Item.ToString() : "NULL";
    }

    /// <summary>
    /// 返回当前对象的哈希代码。
    /// </summary>
    /// <remarks>
    /// Returns the hash code of the current object.
    /// </remarks>
    /// <returns>当前对象的哈希代码 / The hash code of the current object.</returns>
    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Item);
    }
}