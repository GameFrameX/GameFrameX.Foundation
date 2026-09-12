using System;
using System.Collections.Generic;
using GameFrameX.Foundation.Idempotency;
using Xunit;

namespace GameFrameX.Foundation.Idempotency.Tests;

/// <summary>
/// 规范化文本测试：确定性、键序无关、排除名单、空值跳过。
/// </summary>
public class CanonicalTextTests
{
    [Fact]
    public void BuildCanonicalText_ShouldSortKeysAscendingAndTerminateEachLine()
    {
        // 安排 + 执行
        string canonicalText = CanonicalText.BuildCanonicalText(new Dictionary<string, string>
        {
            { "zebra", "26" },
            { "apple", "1" },
            { "mango", "13" },
        });

        // 断言：键升序、逐行 "键=值\n"
        Assert.Equal("apple=1\nmango=13\nzebra=26\n", canonicalText);
    }

    [Fact]
    public void BuildCanonicalText_DifferentFieldOrder_ShouldProduceSameText()
    {
        // 安排：同一字段集合两种传入顺序
        List<KeyValuePair<string, string>> forwardFields = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("a", "1"),
            new KeyValuePair<string, string>("b", "2"),
            new KeyValuePair<string, string>("c", "3"),
        };
        List<KeyValuePair<string, string>> backwardFields = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("c", "3"),
            new KeyValuePair<string, string>("b", "2"),
            new KeyValuePair<string, string>("a", "1"),
        };

        // 执行 + 断言：键序无关
        Assert.Equal(CanonicalText.BuildCanonicalText(forwardFields), CanonicalText.BuildCanonicalText(backwardFields));
    }

    [Fact]
    public void BuildCanonicalText_ExcludedFieldNames_ShouldSkipMatchedFields()
    {
        // 安排
        List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("amount", "100"),
            new KeyValuePair<string, string>("requestTime", "999"),
            new KeyValuePair<string, string>("channel", "web"),
        };
        HashSet<string> excludedFieldNames = new HashSet<string> { "requestTime" };

        // 执行
        string canonicalText = CanonicalText.BuildCanonicalText(fields, excludedFieldNames);

        // 断言：排除名单命中的键不参与规范化
        Assert.Equal("amount=100\nchannel=web\n", canonicalText);
    }

    [Fact]
    public void BuildCanonicalText_NullFieldValue_ShouldSkipField()
    {
        // 安排
        List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("a", null!),
            new KeyValuePair<string, string>("b", "2"),
        };

        // 执行
        string canonicalText = CanonicalText.BuildCanonicalText(fields);

        // 断言：空值字段被跳过
        Assert.Equal("b=2\n", canonicalText);
    }

    [Fact]
    public void BuildCanonicalText_AllFieldsSkipped_ShouldReturnEmptyText()
    {
        // 安排：唯一字段被排除
        List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("a", "1"),
        };

        // 执行 + 断言
        Assert.Equal(string.Empty, CanonicalText.BuildCanonicalText(fields, new HashSet<string> { "a" }));
    }

    [Fact]
    public void BuildCanonicalText_NullFields_ShouldThrowArgumentNullException()
    {
        // 执行 + 断言
        Assert.Throws<ArgumentNullException>(() => CanonicalText.BuildCanonicalText(null!));
    }
}
