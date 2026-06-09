using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;
using Xunit;

namespace GameFrameX.Foundation.Tests.Options;

public sealed class OptionsDebuggerSensitiveOutputTests
{
    private sealed class SensitiveConfig
    {
        [Option("api-key", DefaultValue = "default-secret", Sensitive = true)]
        public string ApiKey { get; set; } = "runtime-secret";

        [Option("name", DefaultValue = "visible-default")]
        public string Name { get; set; } = "visible";
    }

    [Fact]
    public void PrintParsedOptions_ShouldRedactSensitiveValueAndDefaultValue()
    {
        var originalOut = Console.Out;
        using var writer = new StringWriter();

        try
        {
            Console.SetOut(writer);

            OptionsDebugger.PrintParsedOptions(new SensitiveConfig());
        }
        finally
        {
            Console.SetOut(originalOut);
        }

        var output = writer.ToString();
        Assert.DoesNotContain("runtime-secret", output, StringComparison.Ordinal);
        Assert.DoesNotContain("default-secret", output, StringComparison.Ordinal);
        Assert.Contains("[REDACTED]", output, StringComparison.Ordinal);
        Assert.Contains("visible", output, StringComparison.Ordinal);
        Assert.Contains("visible-default", output, StringComparison.Ordinal);
    }
}
