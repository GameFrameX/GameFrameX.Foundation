using System;
using System.Collections.Generic;
using System.Text.Json;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// 专门测试格式化JSON解析功能的测试类
    /// </summary>
    public class FormattedJsonTests
    {
        /// <summary>
        /// 测试标准格式化JSON的解析
        /// </summary>
        [Fact]
        public void StandardFormattedJson_ShouldDeserialize()
        {
            // 准备一个标准格式化的JSON字符串
            string formattedJson = @"{
  ""Id"": 1,
  ""Name"": ""标准格式化JSON"",
  ""IsActive"": true,
  ""CreatedDate"": ""2023-01-01T00:00:00"",
  ""Tags"": [
    ""标准"",
    ""格式化"",
    ""测试""
  ]
}";

            // 反序列化
            var result = JsonHelper.Deserialize<FormattedTestClass>(formattedJson);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("标准格式化JSON", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 1, 1), result.CreatedDate);
            Assert.Equal(3, result.Tags.Count);
        }

        /// <summary>
        /// 测试包含注释的格式化JSON的解析
        /// </summary>
        [Fact]
        public void FormattedJsonWithComments_ShouldDeserialize()
        {
            // 准备一个包含注释的格式化JSON字符串
            string jsonWithComments = @"{
  ""Id"": 2, // 这是ID
  ""Name"": ""包含注释的JSON"", /* 这是名称 */
  ""IsActive"": true,
  ""CreatedDate"": ""2023-02-02T00:00:00"",
  ""Tags"": [
    // 这是标签列表
    ""注释"",
    ""格式化"",
    ""测试""
  ]
}";

            // 反序列化
            var result = JsonHelper.Deserialize<FormattedTestClass>(jsonWithComments);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("包含注释的JSON", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 2, 2), result.CreatedDate);
            Assert.Equal(3, result.Tags.Count);
        }

        /// <summary>
        /// 测试包含尾随逗号的格式化JSON的解析
        /// </summary>
        [Fact]
        public void FormattedJsonWithTrailingCommas_ShouldDeserialize()
        {
            // 准备一个包含尾随逗号的格式化JSON字符串
            string jsonWithTrailingCommas = @"{
  ""Id"": 3,
  ""Name"": ""包含尾随逗号的JSON"",
  ""IsActive"": true,
  ""CreatedDate"": ""2023-03-03T00:00:00"",
  ""Tags"": [
    ""尾随逗号"",
    ""格式化"",
    ""测试"",
  ],
}";

            // 反序列化
            var result = JsonHelper.Deserialize<FormattedTestClass>(jsonWithTrailingCommas);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(3, result.Id);
            Assert.Equal("包含尾随逗号的JSON", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 3, 3), result.CreatedDate);
            Assert.Equal(3, result.Tags.Count);
        }

        /// <summary>
        /// 测试不规则缩进的格式化JSON的解析
        /// </summary>
        [Fact]
        public void FormattedJsonWithIrregularIndentation_ShouldDeserialize()
        {
            // 准备一个不规则缩进的格式化JSON字符串
            string jsonWithIrregularIndentation = @"{
    ""Id"": 4,
  ""Name"": ""不规则缩进的JSON"",
        ""IsActive"": true,
 ""CreatedDate"": ""2023-04-04T00:00:00"",
   ""Tags"": [
  ""不规则缩进"",
      ""格式化"",
 ""测试""
   ]
}";

            // 反序列化
            var result = JsonHelper.Deserialize<FormattedTestClass>(jsonWithIrregularIndentation);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(4, result.Id);
            Assert.Equal("不规则缩进的JSON", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 4, 4), result.CreatedDate);
            Assert.Equal(3, result.Tags.Count);
        }

        /// <summary>
        /// 测试混合格式的JSON的解析（部分格式化，部分未格式化）
        /// </summary>
        [Fact]
        public void MixedFormattedJson_ShouldDeserialize()
        {
            // 准备一个混合格式的JSON字符串
            string mixedJson = @"{
  ""Id"": 5,
  ""Name"": ""混合格式的JSON"",
  ""IsActive"": true,
  ""CreatedDate"": ""2023-05-05T00:00:00"",
  ""Tags"": [""混合"", ""格式化"", ""测试""]
}";

            // 反序列化
            var result = JsonHelper.Deserialize<FormattedTestClass>(mixedJson);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(5, result.Id);
            Assert.Equal("混合格式的JSON", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 5, 5), result.CreatedDate);
            Assert.Equal(3, result.Tags.Count);
        }

        /// <summary>
        /// 测试包含特殊字符的格式化JSON的解析
        /// </summary>
        [Fact]
        public void FormattedJsonWithSpecialCharacters_ShouldDeserialize()
        {
            // 准备一个包含特殊字符的格式化JSON字符串
            string jsonWithSpecialChars = @"{
  ""Id"": 6,
  ""Name"": ""包含特殊字符的JSON：\u4E2D\u6587、\t制表符、\n换行符"",
  ""IsActive"": true,
  ""CreatedDate"": ""2023-06-06T00:00:00"",
  ""Tags"": [
    ""特殊字符"",
    ""格式化"",
    ""测试""
  ]
}";

            // 反序列化
            var result = JsonHelper.Deserialize<FormattedTestClass>(jsonWithSpecialChars);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(6, result.Id);
            Assert.Contains("中文", result.Name);
            Assert.Contains("\t", result.Name);
            Assert.Contains("\n", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 6, 6), result.CreatedDate);
            Assert.Equal(3, result.Tags.Count);
        }

        /// <summary>
        /// 测试从格式化JSON到非格式化JSON的转换
        /// </summary>
        [Fact]
        public void ConvertFormattedToNonFormatted_ShouldWork()
        {
            // 准备一个格式化的JSON字符串
            string formattedJson = @"{
  ""Id"": 7,
  ""Name"": ""格式化到非格式化转换测试"",
  ""IsActive"": true,
  ""CreatedDate"": ""2023-07-07T00:00:00"",
  ""Tags"": [
    ""格式化"",
    ""非格式化"",
    ""转换"",
    ""测试""
  ]
}";

            // 先反序列化
            var obj = JsonHelper.Deserialize<FormattedTestClass>(formattedJson);
            
            // 再序列化为非格式化JSON
            string nonFormattedJson = JsonHelper.Serialize(obj);
            
            // 验证非格式化JSON不包含换行符和多余空格
            Assert.NotNull(nonFormattedJson);
            Assert.NotEmpty(nonFormattedJson);
            Assert.DoesNotContain("\n", nonFormattedJson);
            Assert.DoesNotContain("  ", nonFormattedJson);
            
            // 再次反序列化非格式化JSON
            var result = JsonHelper.Deserialize<FormattedTestClass>(nonFormattedJson);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal("格式化到非格式化转换测试", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 7, 7), result.CreatedDate);
            Assert.Equal(4, result.Tags.Count);
        }

        /// <summary>
        /// 测试从非格式化JSON到格式化JSON的转换
        /// </summary>
        [Fact]
        public void ConvertNonFormattedToFormatted_ShouldWork()
        {
            // 准备一个非格式化的JSON字符串
            string nonFormattedJson = @"{""Id"":8,""Name"":""非格式化到格式化转换测试"",""IsActive"":true,""CreatedDate"":""2023-08-08T00:00:00"",""Tags"":[""非格式化"",""格式化"",""转换"",""测试""]}";

            // 先反序列化
            var obj = JsonHelper.Deserialize<FormattedTestClass>(nonFormattedJson);
            
            // 再序列化为格式化JSON
            string formattedJson = JsonHelper.SerializeFormat(obj);
            
            // 验证格式化JSON包含换行符和缩进
            Assert.NotNull(formattedJson);
            Assert.NotEmpty(formattedJson);
            Assert.Contains("\n", formattedJson);
            Assert.Contains("  ", formattedJson);
            
            // 再次反序列化格式化JSON
            var result = JsonHelper.Deserialize<FormattedTestClass>(formattedJson);
            
            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(8, result.Id);
            Assert.Equal("非格式化到格式化转换测试", result.Name);
            Assert.True(result.IsActive);
            Assert.Equal(new DateTime(2023, 8, 8), result.CreatedDate);
            Assert.Equal(4, result.Tags.Count);
        }
    }

    /// <summary>
    /// 用于格式化测试的类
    /// </summary>
    public class FormattedTestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}