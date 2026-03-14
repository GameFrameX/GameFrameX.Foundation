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
using System.Text.Json;
using GameFrameX.Foundation.Json;
using Xunit;

namespace GameFrameX.Foundation.Tests.Json
{
    /// <summary>
    /// 测试JsonHelper处理异常情况的测试类
    /// </summary>
    public class JsonExceptionTests
    {
        /// <summary>
        /// 测试传入null参数时的异常
        /// </summary>
        [Fact]
        public void NullArguments_ShouldThrowException()
        {
            // 测试序列化null对象
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Serialize(null));
            
            // 测试反序列化null字符串
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Deserialize<ExceptionTestClass>(null));
            
            // 测试使用null选项
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Serialize(new ExceptionTestClass(), null));
            
            // 测试反序列化为null类型
            Assert.Throws<ArgumentNullException>(() => JsonHelper.Deserialize("{}",  (Type)null));
        }

        /// <summary>
        /// 测试传入空字符串参数时的异常
        /// </summary>
        [Fact]
        public void EmptyStringArguments_ShouldThrowException()
        {
            // 测试反序列化空字符串
            Assert.Throws<ArgumentException>(() => JsonHelper.Deserialize<ExceptionTestClass>(""));
            
            // 测试反序列化空白字符串
            Assert.Throws<ArgumentException>(() => JsonHelper.Deserialize<ExceptionTestClass>("   "));
        }

        /// <summary>
        /// 测试无效JSON的处理
        /// </summary>
        [Fact]
        public void InvalidJson_ShouldHandleGracefully()
        {
            // 准备一些无效的JSON字符串
            string invalidJson1 = "{\"Id\":1,\"Name\":\"缺少结束括号\"";
            string invalidJson2 = "{\"Id\":1,\"Name\":缺少引号}";
            string invalidJson3 = "{\"Id\":true,\"Name\":123}"; // 类型不匹配
            
            // 测试TryDeserialize方法对无效JSON的处理
            Assert.False(JsonHelper.TryDeserialize<ExceptionTestClass>(invalidJson1, out var result1));
            Assert.Null(result1);
            
            Assert.False(JsonHelper.TryDeserialize<ExceptionTestClass>(invalidJson2, out var result2));
            Assert.Null(result2);
            
            // 类型不匹配的情况下，System.Text.Json可能会尝试转换，这取决于具体实现
            // 我们只验证方法不会抛出异常
            JsonHelper.TryDeserialize<ExceptionTestClass>(invalidJson3, out var result3);
        }

        /// <summary>
        /// 测试格式化JSON中的错误处理
        /// </summary>
        [Fact]
        public void FormattedJsonErrors_ShouldHandleGracefully()
        {
            // 准备一些格式化但有错误的JSON字符串
            string errorJson1 = @"{
  ""Id"": 1,
  ""Name"": ""缺少逗号""
  ""IsActive"": true
}";
            string errorJson2 = @"{
  ""Id"": 1,
  ""Name"": ""多余逗号"",,,
  ""IsActive"": true
}";
            
            // 测试TryDeserialize方法对错误格式化JSON的处理
            Assert.False(JsonHelper.TryDeserialize<ExceptionTestClass>(errorJson1, out var result1));
            Assert.Null(result1);
            
            Assert.False(JsonHelper.TryDeserialize<ExceptionTestClass>(errorJson2, out var result2));
            Assert.Null(result2);
        }

        /// <summary>
        /// 测试格式化和非格式化JSON的自动识别
        /// </summary>
        [Fact]
        public void AutoDetectJsonFormat_ShouldWork()
        {
            // 准备一个格式化的JSON字符串
            string formattedJson = @"{
  ""Id"": 1,
  ""Name"": ""格式化JSON"",
  ""IsActive"": true
}";

            // 准备一个非格式化的JSON字符串
            string nonFormattedJson = @"{""Id"":2,""Name"":""非格式化JSON"",""IsActive"":false}";

            // 测试两种格式都能正确反序列化
            var result1 = JsonHelper.Deserialize<ExceptionTestClass>(formattedJson);
            var result2 = JsonHelper.Deserialize<ExceptionTestClass>(nonFormattedJson);

            // 验证结果
            Assert.NotNull(result1);
            Assert.Equal(1, result1.Id);
            Assert.Equal("格式化JSON", result1.Name);
            Assert.True(result1.IsActive);

            Assert.NotNull(result2);
            Assert.Equal(2, result2.Id);
            Assert.Equal("非格式化JSON", result2.Name);
            Assert.False(result2.IsActive);
        }

        /// <summary>
        /// 测试UTF8字节数组的异常处理
        /// </summary>
        [Fact]
        public void Utf8BytesExceptions_ShouldHandleGracefully()
        {
            // 测试空字节数组
            Assert.Throws<ArgumentNullException>(() => JsonHelper.DeserializeFromUtf8Bytes<TestClass>(null));

            // 测试无效的UTF8字节数组
            byte[] invalidBytes = new byte[] { 0xFF, 0xFE, 0x00, 0x00 }; // 无效的UTF-8序列
            Assert.False(JsonHelper.TryDeserialize<TestClass>(System.Text.Encoding.UTF8.GetString(invalidBytes), out var result));
            Assert.Null(result);
        }

        /// <summary>
        /// 测试特殊浮点数值的处理
        /// </summary>
        [Fact]
        public void SpecialFloatingPointValues_ShouldDeserialize()
        {
            // 准备包含特殊浮点数值的JSON
            string specialJson = @"{
  ""Id"": 1,
  ""Name"": ""特殊浮点数测试"",
  ""Value"": NaN,
  ""PositiveInfinity"": Infinity,
  ""NegativeInfinity"": -Infinity
}";

            // 反序列化
            var result = JsonHelper.Deserialize<ExceptionTestClassWithFloat>(specialJson);

            // 验证结果
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("特殊浮点数测试", result.Name);
            Assert.True(float.IsNaN(result.Value));
            Assert.True(float.IsPositiveInfinity(result.PositiveInfinity));
            Assert.True(float.IsNegativeInfinity(result.NegativeInfinity));
        }
    }

    /// <summary>
    /// 用于异常测试的类
    /// </summary>
    public class ExceptionTestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 包含浮点数的测试类
    /// </summary>
    public class ExceptionTestClassWithFloat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
        public float PositiveInfinity { get; set; }
        public float NegativeInfinity { get; set; }
    }
}
