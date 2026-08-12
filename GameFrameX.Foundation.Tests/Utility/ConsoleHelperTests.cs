using System;
using System.IO;
using GameFrameX.Foundation.Utility;
using Xunit;

namespace GameFrameX.Foundation.Tests.Utility
{
    /// <summary>
    /// ConsoleHelper 单元测试：通过重定向 Console.Out 验证 ConsoleLogo 输出内容。
    /// </summary>
    public class ConsoleHelperTests
    {
        [Fact]
        public void ConsoleLogo_ShouldWriteAsciiBannerAndRepositoryLinks()
        {
            // 用固定的原始终端输出做引用，以便最后恢复
            TextWriter originalOut = Console.Out;

            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                try
                {
                    ConsoleHelper.ConsoleLogo();
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                string output = writer.ToString();

                // ASCII Logo 关键片段（取自源码）
                Assert.Contains("GameFrameX", output);
                Assert.Contains("GitHub Repository", output);
                Assert.Contains("https://github.com/GameFrameX/GameFrameX", output);
                Assert.Contains("Gitee Repository", output);
                Assert.Contains("https://gitee.com/GameFrameX/GameFrameX", output);
                Assert.Contains("Cnb Repository", output);
                Assert.Contains("https://cnb.cool/GameFrameX/GameFrameX", output);
                Assert.Contains("Official Documentation", output);
                Assert.Contains("https://gameframex.doc.alianblank.com", output);
            }
        }

        [Fact]
        public void ConsoleLogo_ShouldAlwaysEmitLeadingAndTrailingBlankLines()
        {
            TextWriter originalOut = Console.Out;

            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                try
                {
                    ConsoleHelper.ConsoleLogo();
                }
                finally
                {
                    Console.SetOut(originalOut);
                }

                string output = writer.ToString();

                // 源码开头和结尾各调用了 Console.WriteLine()，所以输出应以换行开头并以双换行结尾
                Assert.StartsWith(Environment.NewLine, output);
                Assert.EndsWith(Environment.NewLine + Environment.NewLine, output);
            }
        }

        [Fact]
        public void ConsoleLogo_ShouldBeCallableMultipleTimesWithoutThrowing()
        {
            TextWriter originalOut = Console.Out;

            using (StringWriter writer = new StringWriter())
            {
                Console.SetOut(writer);

                try
                {
                    var exception = Record.Exception(() =>
                    {
                        ConsoleHelper.ConsoleLogo();
                        ConsoleHelper.ConsoleLogo();
                        ConsoleHelper.ConsoleLogo();
                    });

                    Assert.Null(exception);
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }
        }
    }
}
