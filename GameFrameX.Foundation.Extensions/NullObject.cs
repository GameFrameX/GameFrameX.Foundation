// GameFrameX 组织下的以及组织衍生的项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
// 
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE 文件。
// 
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 表示可为空的对象。
/// </summary>
/// <typeparam name="T">对象的类型。</typeparam>
public readonly record struct NullObject<T> : IComparable, IComparable<T>, IEquatable<NullObject<T>>
{
    /// <summary>
    /// 初始化一个新的 <see cref="NullObject{T}" /> 实例。
    /// </summary>
    /// <param name="item">对象的值。</param>
    public NullObject(T item)
    {
        Item = item;
    }

    /// <summary>
    /// 获取一个表示空值的 <see cref="NullObject{T}" /> 实例。
    /// </summary>
    public static NullObject<T> Null
    {
        get { return new NullObject<T>(); }
    }

    /// <summary>
    /// 获取对象的值。
    /// </summary>
    public T Item { get; }

    /// <summary>
    /// 比较当前对象与另一个对象。
    /// </summary>
    /// <param name="value">要比较的对象。</param>
    /// <returns>一个整数，指示当前对象与 <paramref name="value" /> 的相对顺序。</returns>
    public int CompareTo(object value)
    {
        if (value is NullObject<T> nullObject)
        {
            if (nullObject.Item is IComparable c)
            {
                return ((IComparable)Item).CompareTo(c);
            }

            return string.Compare(Item.ToString(), nullObject.Item.ToString(), StringComparison.Ordinal);
        }

        return 1;
    }

    /// <summary>
    /// 比较当前对象与同一类型的另一个对象。
    /// </summary>
    /// <param name="other">要比较的对象。</param>
    /// <returns>一个整数，指示当前对象与 <paramref name="other" /> 的相对顺序。</returns>
    public int CompareTo(T other)
    {
        if (other is IComparable c)
        {
            return ((IComparable)Item).CompareTo(c);
        }

        return Item.ToString().CompareTo(other.ToString());
    }

    /// <summary>
    /// 指示当前对象是否等于同一类型的另一个对象。
    /// </summary>
    /// <param name="other">一个与此对象进行比较的对象。</param>
    /// <returns>如果当前对象等于 <paramref name="other" /> 参数，则为 true；否则为 false。</returns>
    public bool Equals(NullObject<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Item, other.Item);
    }

    /// <summary>
    /// 将 <see cref="NullObject{T}" /> 隐式转换为类型 <typeparamref name="T" />。
    /// </summary>
    /// <param name="nullObject">要转换的 <see cref="NullObject{T}" /> 实例。</param>
    public static implicit operator T(NullObject<T> nullObject)
    {
        return nullObject.Item;
    }

    /// <summary>
    /// 将类型 <typeparamref name="T" /> 隐式转换为 <see cref="NullObject{T}" />。
    /// </summary>
    /// <param name="item">要转换的值。</param>
    public static implicit operator NullObject<T>(T item)
    {
        return new NullObject<T>(item);
    }

    /// <summary>
    /// 返回对象的字符串表示形式。
    /// </summary>
    /// <returns>对象的字符串表示形式，如果对象为 null，则返回 "NULL"。</returns>
    public override string ToString()
    {
        return Item != null ? Item.ToString() : "NULL";
    }

    /// <summary>
    /// 返回当前对象的哈希代码。
    /// </summary>
    /// <returns>当前对象的哈希代码。</returns>
    public override int GetHashCode()
    {
        return EqualityComparer<T>.Default.GetHashCode(Item);
    }
}