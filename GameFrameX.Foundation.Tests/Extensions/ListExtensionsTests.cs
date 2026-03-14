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
using System.Threading.Tasks;
using Xunit;
using GameFrameX.Foundation.Extensions;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// ListExtensions 扩展类单元测试
/// </summary>
public class ListExtensionsTests
{
    #region ForEachAsync List Tests

    [Fact]
    public async Task ForEachAsync_List_ValidInput_ShouldExecuteForAllElements()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };
        var results = new List<int>();

        // Act
        await list.ForEachAsync(async item =>
        {
            await Task.Delay(1); // Simulate async work
            results.Add(item * 2);
        });

        // Assert
        Assert.Equal(new[] { 2, 4, 6, 8, 10 }, results);
    }

    [Fact]
    public async Task ForEachAsync_List_EmptyList_ShouldNotExecute()
    {
        // Arrange
        var list = new List<int>();
        var executed = false;

        // Act
        await list.ForEachAsync(async item =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public async Task ForEachAsync_List_NullList_ShouldThrowArgumentNullException()
    {
        // Arrange
        List<int> list = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            list.ForEachAsync(async item => await Task.Delay(1)));
        Assert.Equal("list", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_List_NullFunc_ShouldThrowArgumentNullException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        Func<int, Task> func = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            list.ForEachAsync(func));
        Assert.Equal("func", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_List_ExceptionInFunc_ShouldPropagateException()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };
        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            list.ForEachAsync(async item =>
            {
                await Task.Delay(1);
                if (item != 2)
                {
                }
                else
                {
                    throw expectedException;
                }
            }));
        Assert.Equal(expectedException.Message, actualException.Message);
    }

    #endregion

    #region ForEachAsync IEnumerable Tests

    [Fact]
    public async Task ForEachAsync_IEnumerable_ValidInput_ShouldExecuteForAllElements()
    {
        // Arrange
        IEnumerable<string> source = new[] { "a", "b", "c" };
        var results = new List<string>();

        // Act
        await source.ForEachAsync(async item =>
        {
            await Task.Delay(1); // Simulate async work
            results.Add(item.ToUpper());
        });

        // Assert
        Assert.Equal(new[] { "A", "B", "C" }, results);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_EmptyEnumerable_ShouldNotExecute()
    {
        // Arrange
        IEnumerable<int> source = Enumerable.Empty<int>();
        var executed = false;

        // Act
        await source.ForEachAsync(async item =>
        {
            await Task.Delay(1);
            executed = true;
        });

        // Assert
        Assert.False(executed);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_NullSource_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> source = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            source.ForEachAsync(async item => await Task.Delay(1)));
        Assert.Equal("source", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_NullAction_ShouldThrowArgumentNullException()
    {
        // Arrange
        IEnumerable<int> source = new[] { 1, 2, 3 };
        Func<int, Task> action = null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => 
            source.ForEachAsync(action));
        Assert.Equal("action", exception.ParamName);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_ExceptionInAction_ShouldPropagateException()
    {
        // Arrange
        IEnumerable<int> source = new[] { 1, 2, 3 };
        var expectedException = new InvalidOperationException("Test exception");

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            source.ForEachAsync(async item =>
            {
                await Task.Delay(1);
                if (item == 2)
                {
                    throw expectedException;
                }
            }));
        Assert.Equal(expectedException.Message, actualException.Message);
    }

    [Fact]
    public async Task ForEachAsync_IEnumerable_WithLinqQuery_ShouldWork()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var evenNumbers = numbers.Where(x => x % 2 == 0);
        var results = new List<int>();

        // Act
        await evenNumbers.ForEachAsync(async item =>
        {
            await Task.Delay(1);
            results.Add(item * 10);
        });

        // Assert
        Assert.Equal(new[] { 20, 40 }, results);
    }

    #endregion
}