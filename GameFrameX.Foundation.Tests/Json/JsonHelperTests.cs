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
using System.Text.Json;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// JsonHelper 类的单元测试
    /// </summary>
    public class JsonHelperTests
    {
        /// <summary>
        /// 测试基本序列化和反序列化功能
        /// </summary>
        [Fact]
        public void BasicSerializeDeserialize_ShouldWork()
        {
            // 准备测试数据
            var testObject = new TestClass
            {
                Id = 1,
                Name = "测试名称",
                IsActive = true,
                CreatedDate = new DateTime(2023, 1, 1),
                Tags = new List<string> { "标签1", "标签2", "标签3" }
            };

            // 序列化
            string json = JsonHelper.Serialize(testObject);
            
            // 验证序列化结果不为空
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            
            // 反序列化
            var deserializedObject = JsonHelper.Deserialize<TestClass>(json);
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.IsActive, deserializedObject.IsActive);
            Assert.Equal(testObject.CreatedDate, deserializedObject.CreatedDate);
            Assert.Equal(testObject.Tags.Count, deserializedObject.Tags.Count);
            for (int i = 0; i < testObject.Tags.Count; i++)
            {
                Assert.Equal(testObject.Tags[i], deserializedObject.Tags[i]);
            }
        }

        /// <summary>
        /// 测试格式化JSON的序列化和反序列化
        /// </summary>
        [Fact]
        public void FormattedJsonSerializeDeserialize_ShouldWork()
        {
            // 准备测试数据
            var testObject = new TestClass
            {
                Id = 2,
                Name = "格式化测试",
                IsActive = true,
                CreatedDate = new DateTime(2023, 2, 2),
                Tags = new List<string> { "格式化", "JSON", "测试" }
            };

            // 使用格式化选项序列化
            string formattedJson = JsonHelper.SerializeFormat(testObject);
            
            // 验证序列化结果不为空且包含换行符（表示已格式化）
            Assert.NotNull(formattedJson);
            Assert.NotEmpty(formattedJson);
            Assert.Contains("\n", formattedJson);
            
            // 反序列化格式化的JSON
            var deserializedObject = JsonHelper.Deserialize<TestClass>(formattedJson);
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.IsActive, deserializedObject.IsActive);
            Assert.Equal(testObject.CreatedDate, deserializedObject.CreatedDate);
            Assert.Equal(testObject.Tags.Count, deserializedObject.Tags.Count);
        }

        /// <summary>
        /// 测试非标准格式化JSON的反序列化
        /// </summary>
        [Fact]
        public void NonStandardFormattedJson_ShouldDeserialize()
        {
            // 准备一个非标准格式化的JSON字符串（包含额外空格、注释和尾随逗号）
            string nonStandardJson = @"{
                ""Id"": 3,
                ""Name"": ""非标准JSON"",  // 这是一个注释
                ""IsActive"": true,
                ""CreatedDate"": ""2023-03-03T00:00:00"",
                ""Tags"": [
                    ""非标准"",
                    ""格式化"",
                    ""测试"", 
                ],
            }";

            // 反序列化非标准格式化的JSON
            var deserializedObject = JsonHelper.Deserialize<TestClass>(nonStandardJson);
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(3, deserializedObject.Id);
            Assert.Equal("非标准JSON", deserializedObject.Name);
            Assert.True(deserializedObject.IsActive);
            Assert.Equal(new DateTime(2023, 3, 3), deserializedObject.CreatedDate);
            Assert.Equal(3, deserializedObject.Tags.Count);
            Assert.Equal("非标准", deserializedObject.Tags[0]);
            Assert.Equal("格式化", deserializedObject.Tags[1]);
            Assert.Equal("测试", deserializedObject.Tags[2]);
        }

        /// <summary>
        /// 测试使用Type参数的反序列化
        /// </summary>
        [Fact]
        public void DeserializeWithType_ShouldWork()
        {
            // 准备测试数据
            var testObject = new TestClass
            {
                Id = 4,
                Name = "Type测试",
                IsActive = true,
                CreatedDate = new DateTime(2023, 4, 4),
                Tags = new List<string> { "Type", "反序列化", "测试" }
            };

            // 序列化
            string json = JsonHelper.Serialize(testObject);
            
            // 使用Type参数反序列化
            var deserializedObject = JsonHelper.Deserialize(json, typeof(TestClass)) as TestClass;
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.IsActive, deserializedObject.IsActive);
            Assert.Equal(testObject.CreatedDate, deserializedObject.CreatedDate);
            Assert.Equal(testObject.Tags.Count, deserializedObject.Tags.Count);
        }

        /// <summary>
        /// 测试UTF8字节数组的序列化和反序列化
        /// </summary>
        [Fact]
        public void Utf8BytesSerializeDeserialize_ShouldWork()
        {
            // 准备测试数据
            var testObject = new TestClass
            {
                Id = 5,
                Name = "UTF8测试",
                IsActive = true,
                CreatedDate = new DateTime(2023, 5, 5),
                Tags = new List<string> { "UTF8", "字节", "测试" }
            };

            // 序列化为UTF8字节数组
            byte[] utf8Bytes = JsonHelper.SerializeToUtf8Bytes(testObject);
            
            // 验证序列化结果不为空
            Assert.NotNull(utf8Bytes);
            Assert.True(utf8Bytes.Length > 0);
            
            // 从UTF8字节数组反序列化
            var deserializedObject = JsonHelper.DeserializeFromUtf8Bytes<TestClass>(utf8Bytes);
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.IsActive, deserializedObject.IsActive);
            Assert.Equal(testObject.CreatedDate, deserializedObject.CreatedDate);
            Assert.Equal(testObject.Tags.Count, deserializedObject.Tags.Count);
        }

        /// <summary>
        /// 测试TryDeserialize方法
        /// </summary>
        [Fact]
        public void TryDeserialize_ShouldHandleValidAndInvalidJson()
        {
            // 准备有效的JSON
            string validJson = @"{""Id"":6,""Name"":""TryDeserialize测试"",""IsActive"":true,""CreatedDate"":""2023-06-06T00:00:00"",""Tags"":[""Try"",""Deserialize"",""测试""]}";
            
            // 准备无效的JSON
            string invalidJson = @"{""Id"":6,""Name"":""无效JSON,""IsActive"":true}"; // 缺少引号
            
            // 测试有效JSON
            bool validResult = JsonHelper.TryDeserialize<TestClass>(validJson, out var validObject);
            
            // 验证有效JSON的结果
            Assert.True(validResult);
            Assert.NotNull(validObject);
            Assert.Equal(6, validObject.Id);
            Assert.Equal("TryDeserialize测试", validObject.Name);
            
            // 测试无效JSON
            bool invalidResult = JsonHelper.TryDeserialize<TestClass>(invalidJson, out var invalidObject);
            
            // 验证无效JSON的结果
            Assert.False(invalidResult);
            Assert.Null(invalidObject);
        }

        /// <summary>
        /// 测试TrySerialize方法
        /// </summary>
        [Fact]
        public void TrySerialize_ShouldHandleValidAndInvalidObjects()
        {
            // 准备有效对象
            var validObject = new TestClass
            {
                Id = 7,
                Name = "TrySerialize测试",
                IsActive = true,
                CreatedDate = new DateTime(2023, 7, 7),
                Tags = new List<string> { "Try", "Serialize", "测试" }
            };
            
            // 测试有效对象
            bool validResult = JsonHelper.TrySerialize(validObject, out var validJson);
            
            // 验证有效对象的结果
            Assert.True(validResult);
            Assert.NotNull(validJson);
            Assert.NotEmpty(validJson);
            
            // 测试空对象
            bool nullResult = JsonHelper.TrySerialize(null, out var nullJson);
            
            // 验证空对象的结果
            Assert.False(nullResult);
            Assert.Null(nullJson);
        }

        /// <summary>
        /// 测试自定义选项的序列化和反序列化
        /// </summary>
        [Fact]
        public void CustomOptions_ShouldWork()
        {
            // 准备测试数据
            var testObject = new TestClass
            {
                Id = 8,
                Name = "自定义选项测试",
                IsActive = true,
                CreatedDate = new DateTime(2023, 8, 8),
                Tags = new List<string> { "自定义", "选项", "测试" }
            };
            
            // 创建自定义选项
            var customOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            // 使用自定义选项序列化
            string json = JsonHelper.Serialize(testObject, customOptions);
            
            // 验证序列化结果不为空且使用了驼峰命名法
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("\"id\":", json); // 验证属性名使用了驼峰命名法
            
            // 使用自定义选项反序列化
            var deserializedObject = JsonHelper.Deserialize<TestClass>(json, customOptions);
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.IsActive, deserializedObject.IsActive);
            Assert.Equal(testObject.CreatedDate, deserializedObject.CreatedDate);
            Assert.Equal(testObject.Tags.Count, deserializedObject.Tags.Count);
        }

        /// <summary>
        /// 测试枚举序列化为字符串
        /// </summary>
        [Fact]
        public void EnumAsString_ShouldWork()
        {
            // 准备测试数据
            var testObject = new TestClassWithEnum
            {
                Id = 9,
                Name = "枚举测试",
                Status = TestStatus.Active
            };
            
            // 序列化
            string json = JsonHelper.Serialize(testObject);
            
            // 验证序列化结果包含枚举的字符串表示
            Assert.NotNull(json);
            Assert.NotEmpty(json);
            Assert.Contains("\"Active\"", json); // 验证枚举值被序列化为字符串
            
            // 反序列化
            var deserializedObject = JsonHelper.Deserialize<TestClassWithEnum>(json);
            
            // 验证反序列化结果
            Assert.NotNull(deserializedObject);
            Assert.Equal(testObject.Id, deserializedObject.Id);
            Assert.Equal(testObject.Name, deserializedObject.Name);
            Assert.Equal(testObject.Status, deserializedObject.Status);
        }

        /// <summary>
        /// 测试中文字符序列化时不应被转义
        /// </summary>
        [Fact]
        public void ChineseCharacterSerialization_ShouldNotBeEscaped()
        {
            // 准备测试数据
            var testObject = new { Name = "测试中文" };

            // 序列化
            string json = JsonHelper.Serialize(testObject);

            // 验证中文字符正常显示
            Assert.Contains("测试中文", json);
        }

        /// <summary>
        /// 测试 Emoji 表情序列化时不应被转义
        /// </summary>
        [Fact]
        public void EmojiSerialization_ShouldNotBeEscaped()
        {
            // 准备测试数据
            var testObject = new { Message = "你好😀👍🎉世界" };

            // 序列化
            string json = JsonHelper.Serialize(testObject);

            // 验证 Emoji 正常显示
            Assert.Contains("你好😀👍🎉世界", json);
        }

        /// <summary>
        /// 测试嵌套 JSON 字符串序列化时中文不应被转义
        /// </summary>
        [Fact]
        public void NestedJsonStringSerialization_ShouldNotEscapeChinese()
        {
            // 场景：属性值是另一个 JSON 序列化后的字符串
            var innerObject = new { Name = "张三", Message = "你好世界" };
            string innerJson = JsonHelper.Serialize(innerObject);

            var outerObject = new { Data = innerJson, Status = "成功" };
            string outerJson = JsonHelper.Serialize(outerObject);

            // 验证内层 JSON 中的中文正常显示
            Assert.Contains("张三", outerJson);
            Assert.Contains("你好世界", outerJson);
            Assert.Contains("成功", outerJson);
        }

        /// <summary>
        /// 测试原始 JSON 字符串作为属性值时中文不应被转义
        /// </summary>
        [Fact]
        public void RawJsonStringProperty_ShouldNotEscapeChinese()
        {
            // 场景：手写的 JSON 字符串作为属性值
            string rawJson = "{\"name\":\"李四\",\"message\":\"测试消息\"}";
            var obj = new { JsonData = rawJson };

            string result = JsonHelper.Serialize(obj);

            // 验证中文正常显示
            Assert.Contains("李四", result);
            Assert.Contains("测试消息", result);
        }

        /// <summary>
        /// 测试包含特殊控制字符时的序列化
        /// </summary>
        [Fact]
        public void SpecialCharactersSerialization_ShouldWork()
        {
            // 准备包含各种特殊字符的测试数据
            var testObject = new
            {
                Quote = "包含\"引号\"",
                Backslash = "包含\\反斜杠",
                Newline = "第一行\n第二行",
                Tab = "制表符\t测试",
                Chinese = "中文测试"
            };

            // 序列化
            string json = JsonHelper.Serialize(testObject);

            // 反序列化验证
            var deserialized = JsonHelper.Deserialize<TestSpecialCharsClass>(json);

            Assert.Equal("包含\"引号\"", deserialized.Quote);
            Assert.Equal("包含\\反斜杠", deserialized.Backslash);
            Assert.Equal("第一行\n第二行", deserialized.Newline);
            Assert.Equal("制表符\t测试", deserialized.Tab);
            Assert.Equal("中文测试", deserialized.Chinese);
        }
    }

    /// <summary>
    /// 用于测试特殊字符的类
    /// </summary>
    public class TestSpecialCharsClass
    {
        public string Quote { get; set; }
        public string Backslash { get; set; }
        public string Newline { get; set; }
        public string Tab { get; set; }
        public string Chinese { get; set; }
    }

    /// <summary>
    /// 用于测试的类
    /// </summary>
    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }

    /// <summary>
    /// 用于测试的枚举
    /// </summary>
    public enum TestStatus
    {
        Inactive = 0,
        Active = 1,
        Suspended = 2
    }

    /// <summary>
    /// 包含枚举的测试类
    /// </summary>
    public class TestClassWithEnum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestStatus Status { get; set; }
    }
}