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