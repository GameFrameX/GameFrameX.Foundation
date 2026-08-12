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

using GameFrameX.Foundation.Logger;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Xunit;

namespace GameFrameX.Foundation.Tests.Logger;

/// <summary>
/// 敏感日志脱敏组件（SensitiveLogPropertyPolicy / SensitiveLogEventEnricher）的边界与契约测试。
/// </summary>
[Collection("LogHelperSerialCollection")]
public sealed class SensitiveLogBoundaryTests
{
    // ==================== SensitiveLogPropertyPolicy ====================

    /// <summary>
    /// 自定义构造函数传入自定义字段名与替换文本，验证命中与属性值。
    /// </summary>
    [Fact]
    public void Constructor_WithCustomNamesAndReplacement_ShouldUseCustomValues()
    {
        // Arrange
        var policy = new SensitiveLogPropertyPolicy(new[] { "Pwd", "Secret" }, "[MASKED]");

        // Act & Assert
        Assert.True(policy.ShouldRedact("Pwd"));
        Assert.True(policy.ShouldRedact("Secret"));
        Assert.False(policy.ShouldRedact("NonSensitive"));
        Assert.Equal("[MASKED]", policy.Replacement);
    }

    /// <summary>
    /// ShouldRedact 对 null/空/空白字段名短路返回 false。
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldRedact_WithNullOrWhitespaceName_ShouldReturnFalse(string propertyName)
    {
        // Arrange
        var policy = new SensitiveLogPropertyPolicy(new[] { "password" });

        // Act
        var result = policy.ShouldRedact(propertyName);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// ShouldRedact 使用 OrdinalIgnoreCase 匹配，大小写不敏感。
    /// </summary>
    [Theory]
    [InlineData("PASSWORD")]
    [InlineData("Password")]
    [InlineData("pAsSwOrD")]
    public void ShouldRedact_ShouldBeCaseInsensitive(string propertyName)
    {
        // Arrange
        var policy = new SensitiveLogPropertyPolicy(new[] { "password" });

        // Act
        var result = policy.ShouldRedact(propertyName);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// replacement 为 null 或空字符串时回退到默认 "[REDACTED]"。
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_WithNullOrEmptyReplacement_ShouldFallbackToDefault(string replacement)
    {
        // Arrange & Act
        var policy = new SensitiveLogPropertyPolicy(new[] { "password" }, replacement);

        // Assert
        Assert.Equal("[REDACTED]", policy.Replacement);
    }

    /// <summary>
    /// 源码使用 string.IsNullOrEmpty 判断回退（非 IsNullOrWhiteSpace），
    /// 故仅含空白字符的 replacement 不会被回退。此测试验证当前行为。
    /// </summary>
    [Fact]
    public void Constructor_WithWhitespaceOnlyReplacement_ShouldNotFallback()
    {
        // Arrange & Act
        var policy = new SensitiveLogPropertyPolicy(new[] { "password" }, "   ");

        // Assert — IsNullOrEmpty 对纯空白返回 false，Replacement 保留原始值
        Assert.Equal("   ", policy.Replacement);
    }

    /// <summary>
    /// 构造时传入的字段名列表中的 null/空/空白条目会被过滤。
    /// </summary>
    [Fact]
    public void Constructor_WithWhitespaceOnlyNames_ShouldBeFiltered()
    {
        // Arrange & Act
        var policy = new SensitiveLogPropertyPolicy(new[] { "password", "", "   ", null });

        // Assert — 有效字段名命中，无效的被过滤
        Assert.True(policy.ShouldRedact("password"));
        Assert.False(policy.ShouldRedact(""));
        Assert.False(policy.ShouldRedact("   "));
    }

    // ==================== SensitiveLogEventEnricher ====================

    /// <summary>
    /// 传入 null policy 时构造器应回退到 Default 策略。
    /// </summary>
    [Fact]
    public void Constructor_WithNullPolicy_ShouldUseDefault()
    {
        // Arrange
        var enricher = new SensitiveLogEventEnricher(null);
        var logEvent = CreateLogEvent(new LogEventProperty("Password", new ScalarValue("secret")));

        // Act
        enricher.Enrich(logEvent, new StubPropertyFactory());

        // Assert — Default 策略包含 "password"，脱敏为默认替换文本
        Assert.Equal("[REDACTED]", GetScalarValue(logEvent, "Password"));
    }

    /// <summary>
    /// 自定义 policy 的 Enricher 应使用自定义替换文本脱敏敏感字段。
    /// </summary>
    [Fact]
    public void Enrich_WithCustomPolicy_ShouldRedactWithCustomReplacement()
    {
        // Arrange
        var policy = new SensitiveLogPropertyPolicy(new[] { "Password" }, "[MASKED]");
        var enricher = new SensitiveLogEventEnricher(policy);
        var logEvent = CreateLogEvent(
            new LogEventProperty("Password", new ScalarValue("plain-password")),
            new LogEventProperty("UserName", new ScalarValue("alice")));

        // Act
        enricher.Enrich(logEvent, new StubPropertyFactory());

        // Assert
        Assert.Equal("[MASKED]", GetScalarValue(logEvent, "Password"));
        Assert.Equal("alice", GetScalarValue(logEvent, "UserName"));
    }

    /// <summary>
    /// Enrich 对 null LogEvent 抛出 ArgumentNullException。
    /// </summary>
    [Fact]
    public void Enrich_WithNullLogEvent_ShouldThrowArgumentNullException()
    {
        // Arrange
        var enricher = new SensitiveLogEventEnricher();

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => enricher.Enrich(null, new StubPropertyFactory()));
        Assert.Equal("logEvent", ex.ParamName);
    }

    /// <summary>
    /// Enrich 对 null propertyFactory 抛出 ArgumentNullException。
    /// </summary>
    [Fact]
    public void Enrich_WithNullPropertyFactory_ShouldThrowArgumentNullException()
    {
        // Arrange
        var enricher = new SensitiveLogEventEnricher();
        var logEvent = CreateLogEvent();

        // Act & Assert
        var ex = Assert.Throws<ArgumentNullException>(() => enricher.Enrich(logEvent, null));
        Assert.Equal("propertyFactory", ex.ParamName);
    }

    /// <summary>
    /// RedactValue 的 SequenceValue 分支：对序列中嵌套的 StructureValue 元素逐元素递归脱敏。
    /// </summary>
    [Fact]
    public void Enrich_WithSequenceValue_ShouldRedactNestedElementsRecursively()
    {
        // Arrange
        var policy = new SensitiveLogPropertyPolicy(new[] { "Password", "Secret" }, "[MASKED]");
        var enricher = new SensitiveLogEventEnricher(policy);

        var sequenceValue = new SequenceValue(new LogEventPropertyValue[]
        {
            new StructureValue(new[]
            {
                new LogEventProperty("UserName", new ScalarValue("alice")),
                new LogEventProperty("Password", new ScalarValue("plain-password")),
            }),
            new StructureValue(new[]
            {
                new LogEventProperty("UserName", new ScalarValue("bob")),
                new LogEventProperty("Secret", new ScalarValue("top-secret")),
            }),
        });
        var logEvent = CreateLogEvent(new LogEventProperty("Users", sequenceValue));

        // Act
        enricher.Enrich(logEvent, new StubPropertyFactory());

        // Assert
        var redactedSequence = Assert.IsType<SequenceValue>(logEvent.Properties["Users"]);
        var elements = redactedSequence.Elements.ToList();
        Assert.Equal(2, elements.Count);

        var firstStructure = Assert.IsType<StructureValue>(elements[0]);
        Assert.Equal("alice", GetStructureScalarValue(firstStructure, "UserName"));
        Assert.Equal("[MASKED]", GetStructureScalarValue(firstStructure, "Password"));

        var secondStructure = Assert.IsType<StructureValue>(elements[1]);
        Assert.Equal("bob", GetStructureScalarValue(secondStructure, "UserName"));
        Assert.Equal("[MASKED]", GetStructureScalarValue(secondStructure, "Secret"));
    }

    // ==================== 辅助方法 ====================

    private static LogEvent CreateLogEvent(params LogEventProperty[] properties)
    {
        var messageTemplate = new MessageTemplate(
            "test",
            new MessageTemplateToken[] { new TextToken("test") });
        return new LogEvent(
            DateTimeOffset.UtcNow,
            LogEventLevel.Information,
            null,
            messageTemplate,
            properties);
    }

    private static object GetScalarValue(LogEvent logEvent, string propertyName)
    {
        Assert.True(logEvent.Properties.TryGetValue(propertyName, out var propertyValue), $"Missing property {propertyName}");
        var scalar = Assert.IsType<ScalarValue>(propertyValue);
        return scalar.Value;
    }

    private static object GetStructureScalarValue(StructureValue structureValue, string propertyName)
    {
        var property = structureValue.Properties.Single(item => item.Name == propertyName);
        var scalar = Assert.IsType<ScalarValue>(property.Value);
        return scalar.Value;
    }

    /// <summary>
    /// 最小化的 ILogEventPropertyFactory 桩，Enrich 方法内部未实际使用该工厂
    /// （仅做 null 守卫），此处仅为满足非 null 参数约束。
    /// </summary>
    private sealed class StubPropertyFactory : ILogEventPropertyFactory
    {
        public LogEventProperty CreateProperty(string name, object value, bool destructureObjects = false)
        {
            return new LogEventProperty(name, new ScalarValue(value));
        }
    }
}
