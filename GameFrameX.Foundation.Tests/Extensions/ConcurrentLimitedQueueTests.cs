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

using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ConcurrentLimitedQueue&lt;T&gt; 类单元测试
/// </summary>
public class ConcurrentLimitedQueueTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidLimit_ShouldCreateQueue()
    {
        // Arrange
        var limit = 5;

        // Act
        var queue = new ConcurrentLimitedQueue<int>(limit);

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(limit, queue.Limit);
        Assert.Empty(queue);
    }

    [Fact]
    public void Constructor_WithZeroLimit_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var limit = 0;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentLimitedQueue<int>(limit));
        Assert.Equal("limit", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNegativeLimit_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var limit = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => new ConcurrentLimitedQueue<int>(limit));
        Assert.Equal("limit", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithValidList_ShouldCreateQueue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        var queue = new ConcurrentLimitedQueue<int>(list);

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(list.Count, queue.Limit);
        Assert.Equal(list.Count, queue.Count);
    }

    [Fact]
    public void Constructor_WithNullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> list = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new ConcurrentLimitedQueue<int>(list));
        Assert.Equal("list", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithEmptyList_ShouldCreateEmptyQueue()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var queue = new ConcurrentLimitedQueue<int>(list);

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(0, queue.Limit);
        Assert.Empty(queue);
    }

    #endregion

    #region Implicit Operator Tests

    [Fact]
    public void ImplicitOperator_WithValidList_ShouldCreateQueue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        ConcurrentLimitedQueue<int> queue = list;

        // Assert
        Assert.NotNull(queue);
        Assert.Equal(list.Count, queue.Limit);
        Assert.Equal(list.Count, queue.Count);
    }

    [Fact]
    public void ImplicitOperator_WithNullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<int> list = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
        {
            ConcurrentLimitedQueue<int> queue = list;
        });
        Assert.Equal("list", exception.ParamName);
    }

    #endregion

    #region Enqueue Tests

    [Fact]
    public void Enqueue_WithinLimit_ShouldAddElement()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(3);

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Assert
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Enqueue_AtLimit_ShouldAddElementWithoutRemoving()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(2);
        queue.Enqueue(1);

        // Act
        queue.Enqueue(2);

        // Assert
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Enqueue_ExceedingLimit_ShouldRemoveOldestElement()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(2);
        queue.Enqueue(1);
        queue.Enqueue(2);

        // Act
        queue.Enqueue(3);

        // Assert
        Assert.Equal(2, queue.Count);
        
        // Verify that the oldest element (1) was removed
        var items = queue.ToArray();
        Assert.Contains(2, items);
        Assert.Contains(3, items);
        Assert.DoesNotContain(1, items);
    }

    [Fact]
    public void Enqueue_MultipleElementsExceedingLimit_ShouldMaintainLimit()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(3);

        // Act
        for (int i = 1; i <= 10; i++)
        {
            queue.Enqueue(i);
        }

        // Assert
        Assert.Equal(3, queue.Count);
        
        // Verify that only the last 3 elements remain
        var items = queue.ToArray();
        Assert.Contains(8, items);
        Assert.Contains(9, items);
        Assert.Contains(10, items);
    }

    [Fact]
    public void Enqueue_WithLimitOne_ShouldAlwaysContainOnlyLastElement()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(1);

        // Act
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Assert
        Assert.Single(queue);
        Assert.Equal(3, queue.ToArray()[0]);
    }

    #endregion

    #region Limit Property Tests

    [Fact]
    public void Limit_SetValidValue_ShouldUpdateLimit()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(5);
        var newLimit = 10;

        // Act
        queue.Limit = newLimit;

        // Assert
        Assert.Equal(newLimit, queue.Limit);
    }

    [Fact]
    public void Limit_SetToSmallerValue_ShouldNotAffectExistingElements()
    {
        // Arrange
        var queue = new ConcurrentLimitedQueue<int>(5);
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // Act
        queue.Limit = 2;

        // Assert
        Assert.Equal(2, queue.Limit);
        Assert.Equal(3, queue.Count); // Existing elements should remain
    }

    #endregion
}