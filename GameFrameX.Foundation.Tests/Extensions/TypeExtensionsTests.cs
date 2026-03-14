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
using GameFrameX.Foundation.Extensions;
using Xunit;

namespace GameFrameX.Foundation.Tests.Extensions;

/// <summary>
/// TypeExtensions 扩展类单元测试
/// </summary>
public class TypeExtensionsTests
{
    #region HasImplementedRawGeneric Tests

    [Fact]
    public void HasImplementedRawGeneric_NullType_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type type = null;
        Type generic = typeof(IList<>);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => type.HasImplementedRawGeneric(generic));
    }

    [Fact]
    public void HasImplementedRawGeneric_NullGeneric_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type type = typeof(List<string>);
        Type generic = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => type.HasImplementedRawGeneric(generic));
    }

    [Fact]
    public void HasImplementedRawGeneric_ImplementsGenericInterface_ShouldReturnTrue()
    {
        // Arrange
        Type type = typeof(List<string>);
        Type generic = typeof(IList<>);

        // Act
        var result = type.HasImplementedRawGeneric(generic);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasImplementedRawGeneric_DoesNotImplementGenericInterface_ShouldReturnFalse()
    {
        // Arrange
        Type type = typeof(string);
        Type generic = typeof(IList<>);

        // Act
        var result = type.HasImplementedRawGeneric(generic);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasImplementedRawGeneric_InheritsFromGenericClass_ShouldReturnTrue()
    {
        // Arrange
        Type type = typeof(TestDerived);
        Type generic = typeof(TestBase<>);

        // Act
        var result = type.HasImplementedRawGeneric(generic);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region IsImplWithInterface Tests

    [Fact]
    public void IsImplWithInterface_NullSelf_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type self = null;
        Type target = typeof(IDisposable);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.IsImplWithInterface(target));
    }

    [Fact]
    public void IsImplWithInterface_NullTarget_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type self = typeof(TestClass);
        Type target = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => self.IsImplWithInterface(target));
    }

    [Fact]
    public void IsImplWithInterface_TargetNotInterface_ShouldThrowArgumentException()
    {
        // Arrange
        Type self = typeof(TestClass);
        Type target = typeof(object); // Not an interface

        // Act & Assert
        Assert.Throws<ArgumentException>(() => self.IsImplWithInterface(target));
    }

    [Fact]
    public void IsImplWithInterface_SelfIsInterface_ShouldReturnFalse()
    {
        // Arrange
        Type self = typeof(IDisposable); // Interface type
        Type target = typeof(IDisposable);

        // Act
        var result = self.IsImplWithInterface(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImplWithInterface_SelfIsAbstract_ShouldReturnFalse()
    {
        // Arrange
        Type self = typeof(AbstractClass); // Abstract class
        Type target = typeof(IDisposable);

        // Act
        var result = self.IsImplWithInterface(target);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImplWithInterface_DirectImplementation_ShouldReturnTrue()
    {
        // Arrange
        Type self = typeof(TestClass); // Directly implements IDisposable
        Type target = typeof(IDisposable);

        // Act
        var result = self.IsImplWithInterface(target, true); // directOnly = true

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsImplWithInterface_IndirectImplementation_DirectOnlyTrue_ShouldReturnFalse()
    {
        // Arrange
        Type self = typeof(TestClass); // Indirectly implements IComparable through IComparable<string>
        Type target = typeof(IComparable);

        // Act
        var result = self.IsImplWithInterface(target, true); // directOnly = true

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsImplWithInterface_IndirectImplementation_CheckIndirectInterfacesFalse_ShouldReturnFalse()
    {
        // Arrange
        Type self = typeof(TestClass); // Indirectly implements IComparable through IComparable<string>
        Type target = typeof(IComparable);

        // Act
        var result = self.IsImplWithInterface(target, false, false); // directOnly = false, checkIndirectInterfaces = false

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsImplWithInterface_IndirectImplementation_CheckIndirectInterfacesTrue_ShouldReturnFalse()
    {
        // Arrange
        Type self = typeof(TestClass); // Indirectly implements IComparable<string> but not IComparable
        Type target = typeof(IComparable);

        // Act
        var result = self.IsImplWithInterface(target, false, true); // directOnly = false, checkIndirectInterfaces = true

        // Assert
        // 移除特殊情况处理后，即使 checkIndirectInterfaces=true，也不会认为 IComparable<T> 实现了 IComparable
        Assert.False(result);
    }
    
    [Fact]
    public void IsImplWithInterface_InterfaceInheritance_CheckIndirectInterfacesFalse_ShouldReturnFalse()
    {
        // Arrange
        Type self = typeof(DerivedInterfaceImplementer); // 实现了IChildInterface，间接实现了IParentInterface
        Type target = typeof(IParentInterface);

        // Act
        // 当checkIndirectInterfaces=false时，不检查接口之间的继承关系
        var result = self.IsImplWithInterface(target, false, false);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsImplWithInterface_InterfaceInheritance_CheckIndirectInterfacesTrue_ShouldReturnTrue()
    {
        // Arrange
        Type self = typeof(DerivedInterfaceImplementer); // 实现了IChildInterface，间接实现了IParentInterface
        Type target = typeof(IParentInterface);

        // Act
        // 当checkIndirectInterfaces=true时，检查接口之间的继承关系
        var result = self.IsImplWithInterface(target, false, true);

        // Assert
        Assert.True(result);
    }

    #endregion

    // Helper classes for testing
    private class TestBase<T> { }

    private class TestDerived : TestBase<string> { }

    private abstract class AbstractClass : IDisposable
    {
        public void Dispose() { }
    }

    private class TestClass : IDisposable, IComparable<string>
    {
        public void Dispose() { }

        public int CompareTo(string other) => 0;
    }
    
    // 用于测试接口继承关系的接口和类
    private interface IParentInterface
    {
        void ParentMethod();
    }
    
    private interface IChildInterface : IParentInterface
    {
        void ChildMethod();
    }
    
    private class DerivedInterfaceImplementer : IChildInterface
    {
        public void ParentMethod() { }
        public void ChildMethod() { }
    }
}