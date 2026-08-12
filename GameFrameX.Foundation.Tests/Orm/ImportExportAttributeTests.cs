using System;
using System.Reflection;
using GameFrameX.Foundation.Orm.Attribute;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    /// <summary>
    /// ImportExportAttribute 边界测试:默认值、构造重载、属性赋值往返、空白字符串健壮性、AttributeUsage 元数据、反射可应用性。
    /// </summary>
    /// <remarks>
    /// 完整 public 属性清单(共 7 个):
    /// 类级 - ImportEnabled/ExportEnabled/SheetName
    /// 字段级 - DisplayName/Order/IgnoreImport/IgnoreExport
    /// </remarks>
    public class ImportExportAttributeTests
    {
        // ============================================================
        // 无参构造:默认值
        // ============================================================

        [Fact]
        public void ParameterlessConstructor_ShouldExposeAllDocumentedDefaults()
        {
            // Arrange & Act
            var attribute = new ImportExportAttribute();

            // Assert - 类级默认值
            Assert.True(attribute.ImportEnabled);
            Assert.True(attribute.ExportEnabled);
            Assert.Null(attribute.SheetName);

            // Assert - 字段级默认值
            Assert.Null(attribute.DisplayName);
            Assert.Equal(0, attribute.Order);
            Assert.False(attribute.IgnoreImport);
            Assert.False(attribute.IgnoreExport);
        }

        [Fact]
        public void Type_ShouldBeSealed()
        {
            Assert.True(typeof(ImportExportAttribute).IsSealed);
        }

        // ============================================================
        // DisplayName 构造重载
        // ============================================================

        [Theory]
        [InlineData("订单编号")]
        [InlineData("Code")]
        [InlineData("  含空白  ")] // 当前 ctor 不做 Trim,按原样保留
        public void DisplayNameConstructor_ShouldStoreDisplayNameAsProvided(string displayName)
        {
            // Arrange & Act
            var attribute = new ImportExportAttribute(displayName);

            // Assert
            Assert.Equal(displayName, attribute.DisplayName);

            // 其余属性仍为默认值
            Assert.True(attribute.ImportEnabled);
            Assert.True(attribute.ExportEnabled);
            Assert.Null(attribute.SheetName);
            Assert.Equal(0, attribute.Order);
            Assert.False(attribute.IgnoreImport);
            Assert.False(attribute.IgnoreExport);
        }

        [Fact]
        public void DisplayNameConstructor_WithNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert - 源码已校验,锁定此行为
            var ex = Assert.Throws<ArgumentNullException>(() => new ImportExportAttribute(null!));
            Assert.Equal("displayName", ex.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void DisplayNameConstructor_WithEmptyOrWhiteSpace_ShouldThrowArgumentException(string displayName)
        {
            // 期望健壮行为:空白列显示名无意义,应抛 ArgumentException
            // (源码当前仅校验 null,未校验空白 -> 列入源码缺陷清单,由主进程修源码)
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ImportExportAttribute(displayName));
        }

        // ============================================================
        // 属性赋值往返
        // ============================================================

        [Fact]
        public void AllProperties_ShouldBeMutable_AndRoundTrip()
        {
            // Arrange
            var attribute = new ImportExportAttribute();

            // Act - 类级属性
            attribute.ImportEnabled = false;
            attribute.ExportEnabled = false;
            attribute.SheetName = "游戏订单";

            // Act - 字段级属性
            attribute.DisplayName = "订单编号";
            attribute.Order = 7;
            attribute.IgnoreImport = true;
            attribute.IgnoreExport = true;

            // Assert - 类级
            Assert.False(attribute.ImportEnabled);
            Assert.False(attribute.ExportEnabled);
            Assert.Equal("游戏订单", attribute.SheetName);

            // Assert - 字段级
            Assert.Equal("订单编号", attribute.DisplayName);
            Assert.Equal(7, attribute.Order);
            Assert.True(attribute.IgnoreImport);
            Assert.True(attribute.IgnoreExport);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void SheetName_Setter_ShouldTolerateNullOrWhitespace_AsCurrentDesign(string? sheetName)
        {
            // 源码当前行为:SheetName 是普通可空属性,无 ctor / setter 校验
            // 此测试锁定当前容忍设计(类级标记,空表示使用实体类型名,有降级路径)
            // Arrange
            var attribute = new ImportExportAttribute();

            // Act
            attribute.SheetName = sheetName;

            // Assert
            Assert.Equal(sheetName, attribute.SheetName);
        }

        [Theory]
        [InlineData(-1)]            // 负数:源码当前容忍,业务上可用于"强制靠后"
        [InlineData(0)]             // 0 表示未指定
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void Order_ShouldAcceptAnyInt_AsCurrentDesign(int order)
        {
            // Arrange
            var attribute = new ImportExportAttribute();

            // Act
            attribute.Order = order;

            // Assert
            Assert.Equal(order, attribute.Order);
        }

        [Fact]
        public void IgnoreImport_AndIgnoreExport_ShouldBeIndependent()
        {
            // Arrange - 验证两个独立开关可以单独使用
            var onlyImport = new ImportExportAttribute { IgnoreImport = true, IgnoreExport = false };
            var onlyExport = new ImportExportAttribute { IgnoreImport = false, IgnoreExport = true };
            var both = new ImportExportAttribute { IgnoreImport = true, IgnoreExport = true };

            // Assert
            Assert.True(onlyImport.IgnoreImport);
            Assert.False(onlyImport.IgnoreExport);

            Assert.False(onlyExport.IgnoreImport);
            Assert.True(onlyExport.IgnoreExport);

            Assert.True(both.IgnoreImport);
            Assert.True(both.IgnoreExport);
        }

        // ============================================================
        // AttributeUsage 元数据
        // ============================================================

        [Fact]
        public void AttributeUsage_ShouldBeClassAndProperty_NotAllowMultiple_Inherited()
        {
            AssertUsage<ImportExportAttribute>(AttributeTargets.Class | AttributeTargets.Property, false, true);
        }

        // ============================================================
        // 反射可应用性
        // ============================================================

        [Fact]
        public void ShouldBeApplicableToClassViaReflection()
        {
            // Arrange
            var type = typeof(AnnotatedClass);

            // Act
            var attribute = type.GetCustomAttribute<ImportExportAttribute>();

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("游戏订单表", attribute!.SheetName);
            Assert.True(attribute.ImportEnabled);
            Assert.True(attribute.ExportEnabled);
        }

        [Fact]
        public void ShouldBeApplicableToPropertyViaReflection()
        {
            // Arrange
            var property = typeof(AnnotatedClass).GetProperty(nameof(AnnotatedClass.OrderNo));

            // Act
            var attribute = property!.GetCustomAttribute<ImportExportAttribute>();

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("订单编号", attribute!.DisplayName);
            Assert.Equal(1, attribute.Order);
        }

        [Fact]
        public void AllowMultipleFalse_OnlyOneAttributeRetrievablePerTarget()
        {
            // Arrange - AllowMultiple=false,同目标只能取到一个
            var type = typeof(AnnotatedClass);

            // Act
            var attributes = type.GetCustomAttributes<ImportExportAttribute>();

            // Assert
            Assert.Single(attributes);
        }

        // ============================================================
        // 测试辅助 + 嵌套测试实体
        // ============================================================

        private static void AssertUsage<TAttribute>(AttributeTargets expectedOn, bool allowMultiple, bool inherited)
            where TAttribute : Attribute
        {
            var usage = typeof(TAttribute).GetCustomAttribute<AttributeUsageAttribute>();

            Assert.NotNull(usage);
            Assert.Equal(expectedOn, usage!.ValidOn);
            Assert.Equal(allowMultiple, usage.AllowMultiple);
            Assert.Equal(inherited, usage.Inherited);
        }

        [ImportExport(SheetName = "游戏订单表")]
        private sealed class AnnotatedClass
        {
            [ImportExport("订单编号", Order = 1)]
            public string OrderNo { get; set; } = string.Empty;
        }
    }
}
