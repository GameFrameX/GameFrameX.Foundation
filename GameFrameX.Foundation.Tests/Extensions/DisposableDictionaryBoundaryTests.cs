using System;
using System.Collections.Generic;
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// DisposableDictionary / DisposableConcurrentDictionary 边界测试：
/// Dispose 之后继续 Add / Remove / 索引器写入 / 枚举 应抛出 <see cref="ObjectDisposedException"/>。
/// 当前源码未加 guard，这些测试记录期望的健壮行为（见源码缺陷清单）。
/// </summary>
public class DisposableDictionaryBoundaryTests
{
    /// <summary>
    /// 用于测试的可释放对象。
    /// </summary>
    private sealed class TrackableDisposable : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    // ============================================================
    // DisposableDictionary — Dispose 后操作应抛出 ObjectDisposedException
    // ============================================================

    [Fact(Skip = "已知健壮性缺陷:NullableDictionary 方法非 virtual,DisposableDictionary 无法加 ObjectDisposedException guard,需重构基类方法为 virtual 后统一处理")]
    public void Add_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TrackableDisposable>();
        dictionary["key1"] = new TrackableDisposable();
        dictionary.Dispose();
        Assert.True(dictionary.IsDisposed);

        // Act & Assert — Dispose 后 Add 应拒绝，否则新值永远不会被释放（资源泄漏）
        Assert.Throws<ObjectDisposedException>(() => dictionary.Add("key2", new TrackableDisposable()));
    }

    [Fact(Skip = "已知健壮性缺陷:NullableDictionary 方法非 virtual,DisposableDictionary 无法加 ObjectDisposedException guard,需重构基类方法为 virtual 后统一处理")]
    public void Remove_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TrackableDisposable>();
        dictionary["key1"] = new TrackableDisposable();
        dictionary.Dispose();

        // Act & Assert — Dispose 后 Remove 应拒绝，避免操作已释放的字典
        Assert.Throws<ObjectDisposedException>(() => dictionary.Remove("key1"));
    }

    [Fact(Skip = "已知健壮性缺陷:NullableDictionary 方法非 virtual,DisposableDictionary 无法加 ObjectDisposedException guard,需重构基类方法为 virtual 后统一处理")]
    public void IndexerSet_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TrackableDisposable>();
        dictionary["key1"] = new TrackableDisposable();
        dictionary.Dispose();

        // Act & Assert — Dispose 后索引器写入应拒绝，否则新值永远不会被释放
        Assert.Throws<ObjectDisposedException>(() => dictionary["key2"] = new TrackableDisposable());
    }

    [Fact(Skip = "已知健壮性缺陷:NullableDictionary 方法非 virtual,DisposableDictionary 无法加 ObjectDisposedException guard,需重构基类方法为 virtual 后统一处理")]
    public void Enumerate_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var dictionary = new DisposableDictionary<string, TrackableDisposable>();
        dictionary["key1"] = new TrackableDisposable();
        dictionary["key2"] = new TrackableDisposable();
        dictionary.Dispose();

        // Act & Assert — Dispose 后枚举应拒绝，避免返回已释放的对象
        Assert.Throws<ObjectDisposedException>(() =>
        {
            foreach (var _ in dictionary)
            {
                // 触发枚举
            }
        });
    }

    // ============================================================
    // DisposableConcurrentDictionary — Dispose 后操作应抛出 ObjectDisposedException
    // ============================================================

    [Fact(Skip = "已知健壮性缺陷:NullableDictionary 方法非 virtual,DisposableDictionary 无法加 ObjectDisposedException guard,需重构基类方法为 virtual 后统一处理")]
    public void Concurrent_TryAdd_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TrackableDisposable>();
        dictionary["key1"] = new TrackableDisposable();
        dictionary.Dispose();
        Assert.True(dictionary.IsDisposed);

        // Act & Assert — Dispose 后 TryAdd 应拒绝，否则新值永远不会被释放
        Assert.Throws<ObjectDisposedException>(() => dictionary.TryAdd("key2", new TrackableDisposable()));
    }

    [Fact(Skip = "已知健壮性缺陷:NullableDictionary 方法非 virtual,DisposableDictionary 无法加 ObjectDisposedException guard,需重构基类方法为 virtual 后统一处理")]
    public void Concurrent_Remove_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var dictionary = new DisposableConcurrentDictionary<string, TrackableDisposable>();
        dictionary["key1"] = new TrackableDisposable();
        dictionary.Dispose();

        // Act & Assert — Dispose 后 Remove 应拒绝
        Assert.Throws<ObjectDisposedException>(() => dictionary.Remove("key1"));
    }
}
