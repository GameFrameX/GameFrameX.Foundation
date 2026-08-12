using System;
using System.Reflection;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// GrafanaLokiLabelTagAttribute 单元测试：
    /// 默认构造、AttributeUsage 元数据、反射可应用性、与 OptionAttribute 的关系。
    /// 该特性无成员，主要断言其元数据契约与反射可见性。
    /// </summary>
    public class GrafanaLokiLabelTagAttributeTests
    {
        // ============================================================
        // 构造与继承
        // ============================================================

        [Fact]
        public void Constructor_Parameterless_ShouldCreateInstance()
        {
            var attr = new GrafanaLokiLabelTagAttribute();
            Assert.NotNull(attr);
        }

        [Fact]
        public void Attribute_ShouldInheritFromAttribute()
        {
            var attr = new GrafanaLokiLabelTagAttribute();
            Assert.IsAssignableFrom<Attribute>(attr);
        }

        [Fact]
        public void Attribute_ShouldBeSealed()
        {
            Assert.True(typeof(GrafanaLokiLabelTagAttribute).IsSealed);
        }

        // ============================================================
        // AttributeUsage 元数据
        // ============================================================

        [Fact]
        public void AttributeUsage_ShouldTargetPropertyOnly()
        {
            var usage = typeof(GrafanaLokiLabelTagAttribute)
                .GetCustomAttribute<AttributeUsageAttribute>();
            Assert.NotNull(usage);
            Assert.Equal(AttributeTargets.Property, usage.ValidOn);
        }

        [Fact]
        public void AttributeUsage_ShouldNotAllowMultiple()
        {
            var usage = typeof(GrafanaLokiLabelTagAttribute)
                .GetCustomAttribute<AttributeUsageAttribute>();
            Assert.NotNull(usage);
            Assert.False(usage.AllowMultiple);
        }

        [Fact]
        public void AttributeUsage_ShouldBeInherited()
        {
            var usage = typeof(GrafanaLokiLabelTagAttribute)
                .GetCustomAttribute<AttributeUsageAttribute>();
            Assert.NotNull(usage);
            Assert.True(usage.Inherited);
        }

        // ============================================================
        // 反射可应用性
        // ============================================================

        [Fact]
        public void Attribute_AppliedToProperty_ShouldBeRetrievableViaReflection()
        {
            var property = typeof(LokiAnnotatedConfig)
                .GetProperty(nameof(LokiAnnotatedConfig.ServiceName));
            var attr = property.GetCustomAttribute<GrafanaLokiLabelTagAttribute>();
            Assert.NotNull(attr);
        }

        [Fact]
        public void Attribute_AppliedToMultipleProperties_ShouldBeRetrievableIndependently()
        {
            var serviceProp = typeof(LokiAnnotatedConfig)
                .GetProperty(nameof(LokiAnnotatedConfig.ServiceName))
                .GetCustomAttribute<GrafanaLokiLabelTagAttribute>();
            var envProp = typeof(LokiAnnotatedConfig)
                .GetProperty(nameof(LokiAnnotatedConfig.Environment))
                .GetCustomAttribute<GrafanaLokiLabelTagAttribute>();
            Assert.NotNull(serviceProp);
            Assert.NotNull(envProp);
            Assert.NotSame(serviceProp, envProp);
        }

        [Fact]
        public void Attribute_NotAppliedToProperty_ShouldReturnNull()
        {
            var attr = typeof(LokiAnnotatedConfig)
                .GetProperty(nameof(LokiAnnotatedConfig.UnannotatedValue))
                .GetCustomAttribute<GrafanaLokiLabelTagAttribute>();
            Assert.Null(attr);
        }

        // ============================================================
        // 测试辅助类
        // ============================================================

        private class LokiAnnotatedConfig
        {
            [GrafanaLokiLabelTag]
            public string ServiceName { get; set; }

            [GrafanaLokiLabelTag]
            public string Environment { get; set; }

            public string UnannotatedValue { get; set; }
        }
    }
}
