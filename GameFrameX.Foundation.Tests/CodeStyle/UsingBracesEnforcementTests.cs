using System.Text.RegularExpressions;
using Xunit;

namespace GameFrameX.Foundation.Tests.CodeStyle;

/// <summary>
/// csharp-braces 规则强制测试：所有 <c>using</c> 语句（含 <c>await using</c>）的语句体
/// 必须是带大括号的块，禁止堆叠 using 或单语句体。
/// </summary>
/// <remarks>
/// IDE0011（csharp_prefer_braces）不检测 using 语句，构建期 editorconfig 无法覆盖，
/// 故以源码扫描方式在测试期强制；规则来源：~/.agents/rules/csharp-braces.md。
/// </remarks>
public sealed partial class UsingBracesEnforcementTests
{
    [GeneratedRegex(@"^(\s*)(await\s+)?using\s*\(")]
    private static partial Regex UsingStatementPattern();

    [Fact]
    public void AllUsingStatements_UseBlockBodies()
    {
        var repositoryRoot = FindRepositoryRoot();
        if (repositoryRoot is null)
        {
            // 运行环境未携带源码仓（如以发布产物执行测试）时跳过扫描
            return;
        }

        var violations = new List<string>();
        foreach (var filePath in EnumerateCSharpFiles(repositoryRoot))
        {
            CollectViolations(filePath, violations);
        }

        Assert.True(violations.Count == 0,
            "以下 using 语句体未使用大括号块（csharp-braces 规则）：\n" + string.Join("\n", violations));
    }

    private static void CollectViolations(string filePath, List<string> violations)
    {
        var lines = File.ReadAllLines(filePath);
        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith("//", StringComparison.Ordinal))
            {
                continue;
            }

            var match = UsingStatementPattern().Match(line);
            if (!match.Success)
            {
                continue;
            }

            // 定位 using 头部的闭合括号（可能跨行）
            var depth = 0;
            var closeLine = -1;
            var closeColumn = -1;
            for (var cursor = index; cursor < lines.Length; cursor++)
            {
                var start = cursor == index ? match.Length - 1 : 0;
                for (var column = start; column < lines[cursor].Length; column++)
                {
                    var character = lines[cursor][column];
                    if (character == '(')
                    {
                        depth++;
                    }
                    else if (character == ')')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            closeLine = cursor;
                            closeColumn = column;
                            break;
                        }
                    }
                }

                if (closeLine >= 0)
                {
                    break;
                }
            }

            if (closeLine < 0)
            {
                continue;
            }

            var remainder = lines[closeLine][(closeColumn + 1)..].TrimStart();
            if (remainder.Length == 0)
            {
                var next = closeLine + 1;
                while (next < lines.Length && lines[next].Trim().Length == 0)
                {
                    next++;
                }

                remainder = next < lines.Length ? lines[next].TrimStart() : string.Empty;
            }

            if (!remainder.StartsWith("{", StringComparison.Ordinal))
            {
                violations.Add($"{filePath}({index + 1}): {trimmed}");
            }
        }
    }

    private static IEnumerable<string> EnumerateCSharpFiles(string repositoryRoot)
    {
        var excludedDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "bin", "obj", ".git", "art", "TestResults" };
        var pending = new Queue<string>();
        pending.Enqueue(repositoryRoot);
        while (pending.Count > 0)
        {
            var directory = pending.Dequeue();
            var name = Path.GetFileName(directory);
            if (excludedDirectories.Contains(name))
            {
                continue;
            }

            foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
            {
                if (Directory.Exists(entry))
                {
                    pending.Enqueue(entry);
                }
                else if (entry.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                {
                    yield return entry;
                }
            }
        }
    }

    private static string? FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (Directory.EnumerateFiles(directory.FullName, "GameFrameX.Foundation.slnx").Any())
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
