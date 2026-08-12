using System;
using System.Reflection;
using GameFrameX.Foundation.Orm.Attribute;
using Xunit;

namespace GameFrameX.Foundation.Tests.Orm
{
    /// <summary>
    /// PartitionTableAttribute 边界测试:3 个构造重载 Theory、默认值、可写性、partitionKey 校验、partitionCount 校验、枚举覆盖、AttributeUsage、反射。
    /// </summary>
    /// <remarks>
    /// 构造重载签名(共 3 个):
    /// 1. PartitionTableAttribute(string partitionKey, PartitionType partitionType)
    /// 2. PartitionTableAttribute(string partitionKey, PartitionType partitionType, PartitionInterval interval)
    /// 3. PartitionTableAttribute(string partitionKey, PartitionType partitionType, int partitionCount)
    /// </remarks>
    public class PartitionTableAttributeTests
    {
        // ============================================================
        // 重载 1: (string partitionKey, PartitionType partitionType)
        // ============================================================

        [Theory]
        [InlineData(PartitionType.Range)]
        [InlineData(PartitionType.List)]
        [InlineData(PartitionType.Hash)]
        [InlineData(PartitionType.Composite)]
        public void TwoArgCtor_ShouldStoreKeyAndType_AndApplyDefaults(PartitionType partitionType)
        {
            // Arrange & Act
            var attribute = new PartitionTableAttribute("CreateDate", partitionType);

            // Assert - 入参
            Assert.Equal("CreateDate", attribute.PartitionKey);
            Assert.Equal(partitionType, attribute.PartitionType);

            // Assert - 默认值
            Assert.Equal(PartitionInterval.Monthly, attribute.Interval);
            Assert.Equal(4, attribute.PartitionCount);
            Assert.Null(attribute.PartitionValues);
            Assert.True(attribute.AutoCreatePartition);
            Assert.Equal(0, attribute.RetentionDays);
        }

        // ============================================================
        // 重载 2: (string partitionKey, PartitionType partitionType, PartitionInterval interval)
        // ============================================================

        [Theory]
        [InlineData(PartitionInterval.Daily)]
        [InlineData(PartitionInterval.Weekly)]
        [InlineData(PartitionInterval.Monthly)]
        [InlineData(PartitionInterval.Quarterly)]
        [InlineData(PartitionInterval.Yearly)]
        [InlineData(PartitionInterval.Custom)]
        public void IntervalCtor_ShouldStoreInterval_AndKeepOtherDefaults(PartitionInterval interval)
        {
            // Arrange & Act
            var attribute = new PartitionTableAttribute("CreateDate", PartitionType.Range, interval);

            // Assert
            Assert.Equal(interval, attribute.Interval);
            Assert.Equal("CreateDate", attribute.PartitionKey);
            Assert.Equal(PartitionType.Range, attribute.PartitionType);

            // Assert - 未被覆盖的默认值
            Assert.Equal(4, attribute.PartitionCount);
            Assert.Null(attribute.PartitionValues);
            Assert.True(attribute.AutoCreatePartition);
            Assert.Equal(0, attribute.RetentionDays);
        }

        // ============================================================
        // 重载 3: (string partitionKey, PartitionType partitionType, int partitionCount)
        // ============================================================

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(64)]
        public void CountCtor_ShouldStorePartitionCount_AndKeepOtherDefaults(int count)
        {
            // Arrange & Act
            var attribute = new PartitionTableAttribute("UserId", PartitionType.Hash, count);

            // Assert
            Assert.Equal(count, attribute.PartitionCount);
            Assert.Equal("UserId", attribute.PartitionKey);
            Assert.Equal(PartitionType.Hash, attribute.PartitionType);

            // Assert - 未被覆盖的默认值(注意:此重载不动 Interval,Interval 仍为默认 Monthly)
            Assert.Equal(PartitionInterval.Monthly, attribute.Interval);
            Assert.Null(attribute.PartitionValues);
            Assert.True(attribute.AutoCreatePartition);
            Assert.Equal(0, attribute.RetentionDays);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void CountCtor_WithNonPositiveCount_ShouldThrowArgumentOutOfRangeException(int count)
        {
            // 期望健壮行为:分区数量必须为正数
            // (源码当前容忍,列入源码缺陷清单,由主进程修源码)
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new PartitionTableAttribute("UserId", PartitionType.Hash, count));
        }

        // ============================================================
        // partitionKey 校验(null / 空 / 空白)
        // ============================================================

        [Fact]
        public void AllConstructors_WithNullPartitionKey_ShouldThrowArgumentNullException()
        {
            // Act & Assert - 源码已校验 null,锁定此行为,并断言 ParamName
            var ex1 = Assert.Throws<ArgumentNullException>(() => new PartitionTableAttribute(null!, PartitionType.Range));
            Assert.Equal("partitionKey", ex1.ParamName);

            var ex2 = Assert.Throws<ArgumentNullException>(() => new PartitionTableAttribute(null!, PartitionType.Range, PartitionInterval.Daily));
            Assert.Equal("partitionKey", ex2.ParamName);

            var ex3 = Assert.Throws<ArgumentNullException>(() => new PartitionTableAttribute(null!, PartitionType.Range, 4));
            Assert.Equal("partitionKey", ex3.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void AllConstructors_WithEmptyOrWhitespacePartitionKey_ShouldThrowArgumentException(string partitionKey)
        {
            // 期望健壮行为:分区键为空白字符串无意义,应抛 ArgumentException
            // (源码当前仅校验 null,未校验空白 -> 列入源码缺陷清单,由主进程修源码)
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new PartitionTableAttribute(partitionKey, PartitionType.Range));
            Assert.Throws<ArgumentException>(() => new PartitionTableAttribute(partitionKey, PartitionType.Range, PartitionInterval.Daily));
            Assert.Throws<ArgumentException>(() => new PartitionTableAttribute(partitionKey, PartitionType.Range, 4));
        }

        // ============================================================
        // 属性可写性
        // ============================================================

        [Fact]
        public void Properties_ShouldBeMutable_AndRoundTrip()
        {
            // Arrange
            var attribute = new PartitionTableAttribute("CreateDate", PartitionType.Range);

            // Act
            attribute.Interval = PartitionInterval.Yearly;
            attribute.PartitionCount = 16;
            attribute.PartitionValues = "2024,2025,2026";
            attribute.AutoCreatePartition = false;
            attribute.RetentionDays = 90;

            // Assert
            Assert.Equal(PartitionInterval.Yearly, attribute.Interval);
            Assert.Equal(16, attribute.PartitionCount);
            Assert.Equal("2024,2025,2026", attribute.PartitionValues);
            Assert.False(attribute.AutoCreatePartition);
            Assert.Equal(90, attribute.RetentionDays);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("North,South,East,West")]
        [InlineData("2024,2025,2026")]
        public void PartitionValues_ShouldAcceptNullOrString(string? values)
        {
            // Arrange - List 分区常用,接受任意字符串(运行时由业务解析)
            var attribute = new PartitionTableAttribute("Region", PartitionType.List);

            // Act
            attribute.PartitionValues = values;

            // Assert
            Assert.Equal(values, attribute.PartitionValues);
        }

        [Theory]
        [InlineData(0)]             // 0 表示永久保留(文档行为)
        [InlineData(30)]
        [InlineData(365)]
        [InlineData(int.MaxValue)]
        public void RetentionDays_ShouldAcceptZeroAndPositive(int days)
        {
            // Arrange
            var attribute = new PartitionTableAttribute("CreateDate", PartitionType.Range);

            // Act
            attribute.RetentionDays = days;

            // Assert
            Assert.Equal(days, attribute.RetentionDays);
        }

        // ============================================================
        // AttributeUsage 元数据
        // ============================================================

        [Fact]
        public void Type_ShouldBeSealed_AndAttributeUsageClassOnly()
        {
            Assert.True(typeof(PartitionTableAttribute).IsSealed);
            AssertUsage<PartitionTableAttribute>(AttributeTargets.Class, false, true);
        }

        // ============================================================
        // 反射可应用性
        // ============================================================

        [Fact]
        public void ShouldBeApplicableToClassViaReflection()
        {
            // Arrange
            var type = typeof(AnnotatedPartitionedClass);

            // Act
            var attribute = type.GetCustomAttribute<PartitionTableAttribute>();

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("CreateDate", attribute!.PartitionKey);
            Assert.Equal(PartitionType.Range, attribute.PartitionType);
            Assert.Equal(PartitionInterval.Monthly, attribute.Interval);
        }

        [Fact]
        public void AllowMultipleFalse_OnlyOneAttributeRetrievablePerClass()
        {
            // Arrange
            var type = typeof(AnnotatedPartitionedClass);

            // Act
            var attributes = type.GetCustomAttributes<PartitionTableAttribute>();

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

        [PartitionTable("CreateDate", PartitionType.Range)]
        private sealed class AnnotatedPartitionedClass
        {
            public DateTime CreateDate { get; set; }
        }
    }
}
