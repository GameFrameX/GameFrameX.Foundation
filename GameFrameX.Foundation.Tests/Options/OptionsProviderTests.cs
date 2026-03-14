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
using GameFrameX.Foundation.Options;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options
{
    /// <summary>
    /// OptionsProvider 类的单元测试
    /// </summary>
    public class OptionsProviderTests
    {
        /// <summary>
        /// 测试配置类
        /// </summary>
        public class TestConfig
        {
            public string Host { get; set; } = "localhost";
            public int Port { get; set; } = 8080;
            public bool Debug { get; set; } = false;
        }

        /// <summary>
        /// 测试基本的获取选项功能
        /// </summary>
        [Fact]
        public void GetOptions_WithValidArgs_ShouldReturnConfigObject()
        {
            // 准备测试数据
            var args = new[] { "--host", "example.com", "--port", "9090", "--debug" };
            
            // 初始化选项提供者
            OptionsProvider.Initialize(args);
            
            // 执行测试
            var config = OptionsProvider.GetOptions<TestConfig>();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("example.com", config.Host);
            Assert.Equal(9090, config.Port);
            Assert.True(config.Debug);
        }

        /// <summary>
        /// 测试空参数数组
        /// </summary>
        [Fact]
        public void GetOptions_WithEmptyArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            string[] args = Array.Empty<string>();
            
            // 初始化选项提供者
            OptionsProvider.Initialize(args);
            
            // 执行测试
            var config = OptionsProvider.GetOptions<TestConfig>();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
            Assert.False(config.Debug); // 默认值
        }

        /// <summary>
        /// 测试 null 参数数组
        /// </summary>
        [Fact]
        public void GetOptions_WithNullArgs_ShouldReturnDefaultConfigObject()
        {
            // 准备测试数据
            string[] args = null;
            
            // 初始化选项提供者
            OptionsProvider.Initialize(args);
            
            // 执行测试
            var config = OptionsProvider.GetOptions<TestConfig>();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
            Assert.False(config.Debug); // 默认值
        }

        /// <summary>
        /// 测试自定义 Bool 格式
        /// </summary>
        [Fact]
        public void GetOptions_WithCustomBoolFormat_ShouldHandleCorrectly()
        {
            // 准备测试数据
            var args = new[] { "--debug=true" };
            
            // 创建选项构建器
            var builder = new OptionsBuilder<TestConfig>(args, BoolArgumentFormat.KeyValue);
            
            // 执行测试
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.True(config.Debug);
        }

        /// <summary>
        /// 测试禁用前缀处理
        /// </summary>
        [Fact]
        public void GetOptions_WithDisabledPrefixHandling_ShouldNotAddPrefixes()
        {
            // 准备测试数据
            var args = new[] { "host", "example.com", "port", "9090" };
            
            // 创建选项构建器
            var builder = new OptionsBuilder<TestConfig>(args, ensurePrefixedKeys: false);
            
            // 执行测试
            var config = builder.Build();
            
            // 验证结果
            Assert.NotNull(config);
            Assert.Equal("localhost", config.Host); // 默认值
            Assert.Equal(8080, config.Port); // 默认值
        }

        /// <summary>
        /// 测试环境变量
        /// </summary>
        [Fact]
        public void GetOptions_WithEnvironmentVariables_ShouldIncludeThemInConfig()
        {
            // 准备测试数据
            var args = new[] { "--host", "example.com" };
            
            try
            {
                // 设置环境变量
                Environment.SetEnvironmentVariable("PORT", "7070");
                Environment.SetEnvironmentVariable("DEBUG", "true");
                
                // 初始化选项提供者
                OptionsProvider.Initialize(args);
                
                // 执行测试
                var config = OptionsProvider.GetOptions<TestConfig>();
                
                // 验证结果
                Assert.NotNull(config);
                Assert.Equal("example.com", config.Host); // 来自命令行参数
                Assert.Equal(7070, config.Port); // 来自环境变量
                Assert.True(config.Debug); // 来自环境变量
            }
            finally
            {
                // 清理环境变量
                Environment.SetEnvironmentVariable("PORT", null);
                Environment.SetEnvironmentVariable("DEBUG", null);
            }
        }
    }
}