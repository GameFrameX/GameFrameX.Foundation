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

using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 字典扩展。
/// </summary>
/// <remarks>
/// Dictionary extension methods.
/// </remarks>
public static class IDictionaryExtensions
{
    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs.
    /// </remarks>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="that"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="that"/> is null.</exception>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(that, nameof(that));

        foreach (var item in that)
        {
            self[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs.
    /// </remarks>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="that"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="that"/> is null.</exception>
    public static void AddOrUpdate<TKey, TValue>(this NullableDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(that, nameof(that));

        foreach (var item in that)
        {
            self[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs.
    /// </remarks>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="that"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="that"/> is null.</exception>
    public static void AddOrUpdate<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(that, nameof(that));

        foreach (var item in that)
        {
            self[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair.
    /// </remarks>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair.
    /// </remarks>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValue">更新时的值 / The value to update.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="key"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="key"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue addValue, TValue updateValue)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = updateValue;
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair.
    /// </remarks>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValue">更新时的值 / The value to update.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="key"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="key"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, TValue addValue, TValue updateValue)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = updateValue;
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValue">更新时的值 / The value to update.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/> 或 <paramref name="key"/> 为 null 时抛出 / Thrown when <paramref name="self"/> or <paramref name="key"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, TValue addValue, TValue updateValue)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = updateValue;
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from another dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="that"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="that"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(that, nameof(that));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        foreach (var item in that)
        {
            AddOrUpdate(self, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from another dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="that"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="that"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static void AddOrUpdate<TKey, TValue>(this NullableDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(that, nameof(that));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        foreach (var item in that)
        {
            AddOrUpdate(self, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from another dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="that"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="that"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static void AddOrUpdate<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(that, nameof(that));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        foreach (var item in that)
        {
            AddOrUpdate(self, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair using factory functions.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加时的操作 / The function to create the value when adding.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/>、<paramref name="addValueFactory"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, <paramref name="addValueFactory"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(addValueFactory, nameof(addValueFactory));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValueFactory(key)))
        {
            self[key] = updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair using factory functions.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加时的操作 / The function to create the value when adding.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/>、<paramref name="addValueFactory"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, <paramref name="addValueFactory"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(addValueFactory, nameof(addValueFactory));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValueFactory(key)))
        {
            self[key] = updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Adds or updates a key-value pair using factory functions.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加时的操作 / The function to create the value when adding.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/>、<paramref name="addValueFactory"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, <paramref name="addValueFactory"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static TValue AddOrUpdate<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(addValueFactory, nameof(addValueFactory));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValueFactory(key)))
        {
            self[key] = updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates a key-value pair.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = await updateValueFactory(key, self[key]).ConfigureAwait(false);
        }

        return self[key];
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates a key-value pair.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/> 或 <paramref name="updateValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, or <paramref name="updateValueFactory"/> is null.</exception>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = await updateValueFactory(key, self[key]).ConfigureAwait(false);
        }

        return self[key];
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates a key-value pair.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">添加时的值 / The value to add.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, addValue))
        {
            self[key] = await updateValueFactory(key, self[key]).ConfigureAwait(false);
        }

        return self[key];
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates key-value pairs from another dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static Task AddOrUpdateAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return that.ForeachAsync(item => AddOrUpdateAsync(self, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates key-value pairs from another dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static Task AddOrUpdateAsync<TKey, TValue>(this NullableDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return that.ForeachAsync(item => AddOrUpdateAsync(self, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates key-value pairs from another dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="that">另一个字典集 / Another dictionary.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static Task AddOrUpdateAsync<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return that.ForeachAsync(item => AddOrUpdateAsync(self, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates a key-value pair using factory functions.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加时的异步操作 / The async function to create the value when adding.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TKey, Task<TValue>> addValueFactory, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, await addValueFactory(key).ConfigureAwait(false)))
        {
            self[key] = await updateValueFactory(key, self[key]).ConfigureAwait(false);
        }

        return self[key];
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates a key-value pair using factory functions.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加时的异步操作 / The async function to create the value when adding.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, Func<TKey, Task<TValue>> addValueFactory, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, await addValueFactory(key).ConfigureAwait(false)))
        {
            self[key] = await updateValueFactory(key, self[key]).ConfigureAwait(false);
        }

        return self[key];
    }

    /// <summary>
    /// 异步添加或更新键值对。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates a key-value pair using factory functions.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加时的异步操作 / The async function to create the value when adding.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, Func<TKey, Task<TValue>> addValueFactory, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, await addValueFactory(key).ConfigureAwait(false)))
        {
            self[key] = await updateValueFactory(key, self[key]).ConfigureAwait(false);
        }

        return self[key];
    }

    /// <summary>
    /// 将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from the current dictionary to the target dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target dictionary.</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        foreach (var item in self)
        {
            that[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from the current NullableDictionary to the target dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target dictionary.</param>
    public static void AddOrUpdateTo<TKey, TValue>(this NullableDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        foreach (var item in self)
        {
            that[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from the current NullableConcurrentDictionary to the target dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target dictionary.</param>
    public static void AddOrUpdateTo<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        foreach (var item in self)
        {
            that[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from the current dictionary to the target dictionary with an update function.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target dictionary.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        foreach (var item in self)
        {
            AddOrUpdate(that, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from the current dictionary to the target NullableDictionary with an update function.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target NullableDictionary.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        foreach (var item in self)
        {
            AddOrUpdate(that, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Adds or updates key-value pairs from the current dictionary to the target NullableConcurrentDictionary with an update function.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target NullableConcurrentDictionary.</param>
    /// <param name="updateValueFactory">更新时的操作 / The function to update the value.</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableConcurrentDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        foreach (var item in self)
        {
            AddOrUpdate(that, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 异步将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates key-value pairs from the current dictionary to the target dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target dictionary.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static Task AddOrUpdateToAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return self.ForeachAsync(item => AddOrUpdateAsync(that, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 异步将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates key-value pairs from the current dictionary to the target NullableDictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target NullableDictionary.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static Task AddOrUpdateToAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return self.ForeachAsync(item => AddOrUpdateAsync(that, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 异步将当前字典的键值对添加或更新到目标字典中。
    /// </summary>
    /// <remarks>
    /// Asynchronously adds or updates key-value pairs from the current dictionary to the target NullableConcurrentDictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">源字典 / The source dictionary.</param>
    /// <param name="that">目标字典 / The target NullableConcurrentDictionary.</param>
    /// <param name="updateValueFactory">更新时的异步操作 / The async function to update the value.</param>
    public static Task AddOrUpdateToAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableConcurrentDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return self.ForeachAsync(item => AddOrUpdateAsync(that, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 获取或添加指定键对应的值。
    /// </summary>
    /// <remarks>
    /// Gets or adds a value for the specified key.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加值的工厂方法 / The factory method to create the value when adding.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="self"/>、<paramref name="key"/> 或 <paramref name="addValueFactory"/> 为 null 时抛出 / Thrown when <paramref name="self"/>, <paramref name="key"/>, or <paramref name="addValueFactory"/> is null.</exception>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TValue> addValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(addValueFactory, nameof(addValueFactory));

        if (!self.ContainsKey(key))
        {
            self[key] = addValueFactory();
        }

        return self[key];
    }

    /// <summary>
    /// 获取或添加指定键对应的值。
    /// </summary>
    /// <remarks>
    /// Gets or adds a value for the specified key.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValue">要添加的值 / The value to add when the key does not exist.</param>
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue addValue)
    {
        return self.TryAdd(key, addValue) ? addValue : self[key];
    }

    /// <summary>
    /// 异步获取或添加指定键对应的值。
    /// </summary>
    /// <remarks>
    /// Asynchronously gets or adds a value for the specified key.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="self">目标字典 / The target dictionary.</param>
    /// <param name="key">键 / The key.</param>
    /// <param name="addValueFactory">添加值的异步工厂方法 / The async factory method to create the value when adding.</param>
    public static async Task<TValue> GetOrAddAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<Task<TValue>> addValueFactory)
    {
        if (!self.ContainsKey(key))
        {
            self[key] = await addValueFactory().ConfigureAwait(false);
        }

        return self[key];
    }

    private static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

        if (dictionary.IsReadOnly || dictionary.ContainsKey(key))
        {
            return false;
        }

        dictionary.Add(key, value);
        return true;
    }

    /// <summary>
    /// 遍历字典并执行指定操作。
    /// </summary>
    /// <remarks>
    /// Iterates through the dictionary and executes the specified action.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="dic">目标字典 / The target dictionary.</param>
    /// <param name="action">回调方法 / The action to execute for each key-value pair.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 或 <paramref name="action"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> or <paramref name="action"/> is null.</exception>
    public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dic, Action<TKey, TValue> action)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        foreach (var item in dic)
        {
            action(item.Key, item.Value);
        }
    }

    /// <summary>
    /// 异步遍历字典并执行指定操作。
    /// </summary>
    /// <remarks>
    /// Asynchronously iterates through the dictionary and executes the specified action.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="dic">目标字典 / The target dictionary.</param>
    /// <param name="action">回调方法 / The async action to execute for each key-value pair.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 或 <paramref name="action"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> or <paramref name="action"/> is null.</exception>
    public static Task ForEachAsync<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey, TValue, Task> action)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        return dic.ForeachAsync(x => action(x.Key, x.Value));
    }

    /// <summary>
    /// 安全地将集合转换为字典，支持重复键。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a dictionary, supporting duplicate keys by overwriting.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
    public static NullableDictionary<TKey, TSource> ToDictionarySafety<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TSource>(items.Count);
        foreach (var item in items)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为字典，支持重复键和默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a dictionary, supporting duplicate keys and a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
    public static NullableDictionary<TKey, TSource> ToDictionarySafety<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TSource>(items.Count)
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in items)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为字典，支持重复键。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a dictionary, supporting duplicate keys by overwriting.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/>、<paramref name="keySelector"/> 或 <paramref name="elementSelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="elementSelector"/> is null.</exception>
    public static NullableDictionary<TKey, TElement> ToDictionarySafety<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        ArgumentNullException.ThrowIfNull(elementSelector, nameof(elementSelector));

        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TElement>(items.Count);
        foreach (var item in items)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为字典，支持重复键和默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a dictionary, supporting duplicate keys and a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/>、<paramref name="keySelector"/> 或 <paramref name="elementSelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="elementSelector"/> is null.</exception>
    public static NullableDictionary<TKey, TElement> ToDictionarySafety<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, TElement defaultValue)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        ArgumentNullException.ThrowIfNull(elementSelector, nameof(elementSelector));

        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TElement>(items.Count)
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in items)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为字典。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a dictionary safely.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    public static async Task<NullableDictionary<TKey, TElement>> ToDictionarySafetyAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector)
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TElement>(items.Count);
        await items.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a dictionary safely with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static async Task<NullableDictionary<TKey, TElement>> ToDictionarySafetyAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector, TElement defaultValue)
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TElement>(items.Count)
        {
            FallbackValue = defaultValue,
        };
        await items.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放字典。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable dictionary.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    public static DisposableDictionary<TKey, TSource> ToDisposableDictionarySafety<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TSource : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TSource>(items.Count);
        foreach (var item in items)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static DisposableDictionary<TKey, TSource> ToDisposableDictionarySafety<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TSource defaultValue) where TSource : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TSource>(items.Count)
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in items)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放字典。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable dictionary.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    public static DisposableDictionary<TKey, TElement> ToDisposableDictionarySafety<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TElement : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TElement>(items.Count);
        foreach (var item in items)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static DisposableDictionary<TKey, TElement> ToDisposableDictionarySafety<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, TElement defaultValue) where TElement : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TElement>(items.Count)
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in items)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为可释放字典。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a disposable dictionary safely.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    public static async Task<DisposableDictionary<TKey, TElement>> ToDisposableDictionarySafetyAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector) where TElement : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TElement>(items.Count);
        await items.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为可释放字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a disposable dictionary safely with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static async Task<DisposableDictionary<TKey, TElement>> ToDisposableDictionarySafetyAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector, TElement defaultValue) where TElement : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TElement>(items.Count)
        {
            FallbackValue = defaultValue,
        };
        await items.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为并发字典。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a concurrent dictionary.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <returns>转换后的 NullableConcurrentDictionary / The converted NullableConcurrentDictionary.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
    public static NullableConcurrentDictionary<TKey, TSource> ToConcurrentDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var dic = new NullableConcurrentDictionary<TKey, TSource>();
        foreach (var item in source)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a concurrent dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static NullableConcurrentDictionary<TKey, TSource> ToConcurrentDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TSource defaultValue)
    {
        var dic = new NullableConcurrentDictionary<TKey, TSource>
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in source)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为并发字典。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a concurrent dictionary.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <returns>转换后的 NullableConcurrentDictionary / The converted NullableConcurrentDictionary.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/>、<paramref name="keySelector"/> 或 <paramref name="elementSelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="elementSelector"/> is null.</exception>
    public static NullableConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        ArgumentNullException.ThrowIfNull(elementSelector, nameof(elementSelector));

        var dic = new NullableConcurrentDictionary<TKey, TElement>();
        foreach (var item in source)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a concurrent dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static NullableConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, TElement defaultValue)
    {
        var dic = new NullableConcurrentDictionary<TKey, TElement>
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in source)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为并发字典。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a concurrent dictionary safely.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    public static async Task<NullableConcurrentDictionary<TKey, TElement>> ToConcurrentDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector)
    {
        var dic = new ConcurrentDictionary<TKey, TElement>();
        await source.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a concurrent dictionary safely with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static async Task<NullableConcurrentDictionary<TKey, TElement>> ToConcurrentDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector, TElement defaultValue)
    {
        var dic = new NullableConcurrentDictionary<TKey, TElement>
        {
            FallbackValue = defaultValue,
        };
        await source.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放并发字典。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable concurrent dictionary.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    public static DisposableConcurrentDictionary<TKey, TSource> ToDisposableConcurrentDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TSource : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TSource>();
        foreach (var item in source)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable concurrent dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static DisposableConcurrentDictionary<TKey, TSource> ToDisposableConcurrentDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, TSource defaultValue) where TSource : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TSource>
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in source)
        {
            dic[keySelector(item)] = item;
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放并发字典。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable concurrent dictionary.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    public static DisposableConcurrentDictionary<TKey, TElement> ToDisposableConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TElement : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TElement>();
        foreach (var item in source)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 安全地将集合转换为可释放并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Safely converts a collection to a disposable concurrent dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static DisposableConcurrentDictionary<TKey, TElement> ToDisposableConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, TElement defaultValue) where TElement : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TElement>
        {
            FallbackValue = defaultValue,
        };
        foreach (var item in source)
        {
            dic[keySelector(item)] = elementSelector(item);
        }

        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为可释放并发字典。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a disposable concurrent dictionary safely.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    public static async Task<DisposableConcurrentDictionary<TKey, TElement>> ToDisposableConcurrentDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector) where TElement : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TElement>();
        await source.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 异步安全地将集合转换为可释放并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a disposable concurrent dictionary safely with a default fallback value.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    public static async Task<DisposableConcurrentDictionary<TKey, TElement>> ToDisposableConcurrentDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector, TElement defaultValue) where TElement : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TElement>
        {
            FallbackValue = defaultValue,
        };
        await source.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 将集合转换为 Lookup 结构。
    /// </summary>
    /// <remarks>
    /// Converts a collection to a Lookup structure.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <returns>转换后的 LookupX / The converted LookupX.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/> 或 <paramref name="keySelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
    public static LookupX<TKey, TSource> ToLookupX<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));

        var items = source as IList<TSource> ?? source.ToList();
        var dic = new Dictionary<TKey, List<TSource>>(items.Count);
        foreach (var item in items)
        {
            var key = keySelector(item);
            if (dic.TryGetValue(key, out var list))
            {
                list.Add(item);
            }
            else
            {
                dic.Add(key, new List<TSource> { item, });
            }
        }

        return new LookupX<TKey, TSource>(dic);
    }

    /// <summary>
    /// 将集合转换为 Lookup 结构。
    /// </summary>
    /// <remarks>
    /// Converts a collection to a Lookup structure.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The function to extract the value from each element.</param>
    /// <returns>转换后的 LookupX / The converted LookupX.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="source"/>、<paramref name="keySelector"/> 或 <paramref name="elementSelector"/> 为 null 时抛出 / Thrown when <paramref name="source"/>, <paramref name="keySelector"/>, or <paramref name="elementSelector"/> is null.</exception>
    public static LookupX<TKey, TElement> ToLookupX<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentNullException.ThrowIfNull(keySelector, nameof(keySelector));
        ArgumentNullException.ThrowIfNull(elementSelector, nameof(elementSelector));

        var items = source as IList<TSource> ?? source.ToList();
        var dic = new Dictionary<TKey, List<TElement>>(items.Count);
        foreach (var item in items)
        {
            var key = keySelector(item);
            if (dic.TryGetValue(key, out var list))
            {
                list.Add(elementSelector(item));
            }
            else
            {
                dic.Add(key, new List<TElement> { elementSelector(item), });
            }
        }

        return new LookupX<TKey, TElement>(dic);
    }

    /// <summary>
    /// 异步将集合转换为 Lookup 结构。
    /// </summary>
    /// <remarks>
    /// Asynchronously converts a collection to a Lookup structure.
    /// </remarks>
    /// <typeparam name="TSource">源元素的类型 / The type of the source element.</typeparam>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TElement">元素的类型 / The type of the element.</typeparam>
    /// <param name="source">源集合 / The source collection.</param>
    /// <param name="keySelector">键选择器 / The function to extract the key from each element.</param>
    /// <param name="elementSelector">值选择器 / The async function to extract the value from each element.</param>
    public static async Task<LookupX<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector)
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new ConcurrentDictionary<TKey, List<TElement>>();
        await items.ForeachAsync(async item =>
        {
            var key = keySelector(item);
            if (dic.TryGetValue(key, out var list))
            {
                list.Add(await elementSelector(item));
            }
            else
            {
                dic.TryAdd(key, new List<TElement> { await elementSelector(item), });
            }
        });
        return new LookupX<TKey, TElement>(dic);
    }

    /// <summary>
    /// 将普通字典转换为并发字典。
    /// </summary>
    /// <remarks>
    /// Converts a regular dictionary to a concurrent dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="dic">源字典 / The source dictionary.</param>
    /// <returns>转换后的 NullableConcurrentDictionary / The converted NullableConcurrentDictionary.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
    public static NullableConcurrentDictionary<TKey, TValue> AsConcurrentDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        return dic;
    }

    /// <summary>
    /// 将普通字典转换为并发字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Converts a regular dictionary to a concurrent dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="dic">源字典 / The source dictionary.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    /// <returns>转换后的 NullableConcurrentDictionary / The converted NullableConcurrentDictionary.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
    public static NullableConcurrentDictionary<TKey, TValue> AsConcurrentDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dic, TValue defaultValue)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        var nullableDictionary = new NullableConcurrentDictionary<TKey, TValue>
        {
            FallbackValue = defaultValue,
        };
        foreach (var p in dic)
        {
            nullableDictionary[p.Key] = p.Value;
        }

        return nullableDictionary;
    }

    /// <summary>
    /// 将并发字典转换为普通字典。
    /// </summary>
    /// <remarks>
    /// Converts a concurrent dictionary to a regular dictionary.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="dic">源并发字典 / The source concurrent dictionary.</param>
    /// <returns>转换后的 NullableDictionary / The converted NullableDictionary.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
    public static NullableDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        return dic;
    }

    /// <summary>
    /// 将并发字典转换为普通字典，支持默认值。
    /// </summary>
    /// <remarks>
    /// Converts a concurrent dictionary to a regular dictionary with a default fallback value.
    /// </remarks>
    /// <typeparam name="TKey">键的类型 / The type of the key.</typeparam>
    /// <typeparam name="TValue">值的类型 / The type of the value.</typeparam>
    /// <param name="dic">源并发字典 / The source concurrent dictionary.</param>
    /// <param name="defaultValue">键未找到时的默认值 / The default value when a key is not found.</param>
    /// <returns>转换后的 NullableDictionary / The converted NullableDictionary.</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="dic"/> 为 null 时抛出 / Thrown when <paramref name="dic"/> is null.</exception>
    public static NullableDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic, TValue defaultValue)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        var nullableDictionary = new NullableDictionary<TKey, TValue>
        {
            FallbackValue = defaultValue,
        };
        foreach (var p in dic)
        {
            nullableDictionary[p.Key] = p.Value;
        }

        return nullableDictionary;
    }
}