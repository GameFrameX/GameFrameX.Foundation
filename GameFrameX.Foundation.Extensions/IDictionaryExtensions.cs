using System.Collections.Concurrent;

namespace GameFrameX.Foundation.Extensions;

/// <summary>
/// 字典扩展
/// </summary>
public static class IDictionaryExtensions
{
    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self">目标字典</param>
    /// <param name="that">另一个字典集</param>
    /// <exception cref="ArgumentNullException">当 self 或 that 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self">目标字典</param>
    /// <param name="that">另一个字典集</param>
    /// <exception cref="ArgumentNullException">当 self 或 that 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self">目标字典</param>
    /// <param name="that">另一个字典集</param>
    /// <exception cref="ArgumentNullException">当 self 或 that 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValue">更新时的值</param>
    /// <exception cref="ArgumentNullException">当 self 或 key 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValue">更新时的值</param>
    /// <exception cref="ArgumentNullException">当 self 或 key 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValue">更新时的值</param>
    /// <exception cref="ArgumentNullException">当 self 或 key 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、that 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、that 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、that 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加时的操作</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key、addValueFactory 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加时的操作</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key、addValueFactory 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加时的操作</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key、addValueFactory 或 updateValueFactory 为 null 时抛出</exception>
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
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key 或 updateValueFactory 为 null 时抛出</exception>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = await updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    /// <exception cref="ArgumentNullException">当 self、key 或 updateValueFactory 为 null 时抛出</exception>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        ArgumentNullException.ThrowIfNull(self, nameof(self));
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(updateValueFactory, nameof(updateValueFactory));

        if (!self.TryAdd(key, addValue))
        {
            self[key] = await updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key">键</param>
    /// <param name="addValue">添加时的值</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, TValue addValue, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, addValue))
        {
            self[key] = await updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static Task AddOrUpdateAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return that.ForeachAsync(item => AddOrUpdateAsync(self, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static Task AddOrUpdateAsync<TKey, TValue>(this NullableDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return that.ForeachAsync(item => AddOrUpdateAsync(self, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static Task AddOrUpdateAsync<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return that.ForeachAsync(item => AddOrUpdateAsync(self, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加时的操作</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TKey, Task<TValue>> addValueFactory, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, await addValueFactory(key)))
        {
            self[key] = await updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加时的操作</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableDictionary<TKey, TValue> self, TKey key, Func<TKey, Task<TValue>> addValueFactory, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, await addValueFactory(key)))
        {
            self[key] = await updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加时的操作</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static async Task<TValue> AddOrUpdateAsync<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, TKey key, Func<TKey, Task<TValue>> addValueFactory, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        if (!self.TryAdd(key, await addValueFactory(key)))
        {
            self[key] = await updateValueFactory(key, self[key]);
        }

        return self[key];
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        foreach (var item in self)
        {
            that[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    public static void AddOrUpdateTo<TKey, TValue>(this NullableDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        foreach (var item in self)
        {
            that[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    public static void AddOrUpdateTo<TKey, TValue>(this NullableConcurrentDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that)
    {
        foreach (var item in self)
        {
            that[item.Key] = item.Value;
        }
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        foreach (var item in self)
        {
            AddOrUpdate(that, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        foreach (var item in self)
        {
            AddOrUpdate(that, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static void AddOrUpdateTo<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableConcurrentDictionary<TKey, TValue> that, Func<TKey, TValue, TValue> updateValueFactory)
    {
        foreach (var item in self)
        {
            AddOrUpdate(that, item.Key, item.Value, updateValueFactory);
        }
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static Task AddOrUpdateToAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, IDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return self.ForeachAsync(item => AddOrUpdateAsync(that, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static Task AddOrUpdateToAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return self.ForeachAsync(item => AddOrUpdateAsync(that, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 添加或更新键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="that">另一个字典集</param>
    /// <param name="updateValueFactory">更新时的操作</param>
    public static Task AddOrUpdateToAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, NullableConcurrentDictionary<TKey, TValue> that, Func<TKey, TValue, Task<TValue>> updateValueFactory)
    {
        return self.ForeachAsync(item => AddOrUpdateAsync(that, item.Key, item.Value, updateValueFactory));
    }

    /// <summary>
    /// 获取或添加
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="self">目标字典</param>
    /// <param name="key">键</param>
    /// <param name="addValueFactory">添加值的工厂方法</param>
    /// <exception cref="ArgumentNullException">当 self、key 或 addValueFactory 为 null 时抛出</exception>
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
    /// 获取或添加
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <param name="addValue"></param>
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue addValue)
    {
        return self.TryAdd(key, addValue) ? addValue : self[key];
    }

    /// <summary>
    /// 获取或添加
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <param name="addValueFactory"></param>
    public static async Task<TValue> GetOrAddAsync<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<Task<TValue>> addValueFactory)
    {
        if (!self.ContainsKey(key))
        {
            self[key] = await addValueFactory();
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
    /// 遍历字典
    /// </summary>
    /// <param name="dic">目标字典</param>
    /// <param name="action">回调方法</param>
    /// <exception cref="ArgumentNullException">当 dic 或 action 为 null 时抛出</exception>
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
    /// 异步遍历字典
    /// </summary>
    /// <param name="dic">目标字典</param>
    /// <param name="action">回调方法</param>
    /// <exception cref="ArgumentNullException">当 dic 或 action 为 null 时抛出</exception>
    public static Task ForEachAsync<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey, TValue, Task> action)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));
        ArgumentNullException.ThrowIfNull(action, nameof(action));

        return dic.ForeachAsync(x => action(x.Key, x.Value));
    }

    /// <summary>
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <exception cref="ArgumentNullException">当 source 或 keySelector 为 null 时抛出</exception>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
    /// <exception cref="ArgumentNullException">当 source 或 keySelector 为 null 时抛出</exception>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <exception cref="ArgumentNullException">当 source、keySelector 或 elementSelector 为 null 时抛出</exception>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
    /// <exception cref="ArgumentNullException">当 source、keySelector 或 elementSelector 为 null 时抛出</exception>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    public static async Task<NullableDictionary<TKey, TElement>> ToDictionarySafetyAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector)
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new NullableDictionary<TKey, TElement>(items.Count);
        await items.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    public static async Task<DisposableDictionary<TKey, TElement>> ToDisposableDictionarySafetyAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector) where TElement : IDisposable
    {
        var items = source as IList<TSource> ?? source.ToList();
        var dic = new DisposableDictionary<TKey, TElement>(items.Count);
        await items.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="keySelector">键选择器</param>
    /// <returns>转换后的NullableConcurrentDictionary</returns>
    /// <exception cref="ArgumentNullException">当 source 或 keySelector 为 null 时抛出</exception>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TElement">元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <returns>转换后的NullableConcurrentDictionary</returns>
    /// <exception cref="ArgumentNullException">当 source、keySelector 或 elementSelector 为 null 时抛出</exception>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue"></param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    public static async Task<NullableConcurrentDictionary<TKey, TElement>> ToConcurrentDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector)
    {
        var dic = new ConcurrentDictionary<TKey, TElement>();
        await source.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue"></param>
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
    /// 安全的转换成字典集
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    public static async Task<DisposableConcurrentDictionary<TKey, TElement>> ToDisposableConcurrentDictionaryAsync<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, Task<TElement>> elementSelector) where TElement : IDisposable
    {
        var dic = new DisposableConcurrentDictionary<TKey, TElement>();
        await source.ForeachAsync(async item => dic[keySelector(item)] = await elementSelector(item));
        return dic;
    }

    /// <summary>
    /// 安全的转换成字典集
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
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
    /// 转换为Lookup
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="keySelector">键选择器</param>
    /// <returns>转换后的LookupX</returns>
    /// <exception cref="ArgumentNullException">当 source 或 keySelector 为 null 时抛出</exception>
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
    /// 转换为Lookup
    /// </summary>
    /// <typeparam name="TSource">源类型</typeparam>
    /// <typeparam name="TKey">键类型</typeparam>
    /// <typeparam name="TElement">元素类型</typeparam>
    /// <param name="source">源集合</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
    /// <returns>转换后的LookupX</returns>
    /// <exception cref="ArgumentNullException">当 source、keySelector 或 elementSelector 为 null 时抛出</exception>
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
    /// 转换为Lookup
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="elementSelector">值选择器</param>
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
    /// 转换成并发字典集合
    /// </summary>
    /// <param name="dic">源字典</param>
    /// <returns>转换后的NullableConcurrentDictionary</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
    public static NullableConcurrentDictionary<TKey, TValue> AsConcurrentDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        return dic;
    }

    /// <summary>
    /// 转换成并发字典集合
    /// </summary>
    /// <param name="dic">源字典</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
    /// <returns>转换后的NullableConcurrentDictionary</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
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
    /// 转换成普通字典集合
    /// </summary>
    /// <param name="dic">源并发字典</param>
    /// <returns>转换后的NullableDictionary</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
    public static NullableDictionary<TKey, TValue> AsDictionary<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dic)
    {
        ArgumentNullException.ThrowIfNull(dic, nameof(dic));

        return dic;
    }

    /// <summary>
    /// 转换成普通字典集合
    /// </summary>
    /// <param name="dic">源并发字典</param>
    /// <param name="defaultValue">键未找到时的默认值</param>
    /// <returns>转换后的NullableDictionary</returns>
    /// <exception cref="ArgumentNullException">当 dic 为 null 时抛出</exception>
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