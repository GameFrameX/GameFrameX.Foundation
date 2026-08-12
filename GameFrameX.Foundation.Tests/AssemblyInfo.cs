using Xunit;

// 禁用测试并行执行。
// 本解决方案被测代码大量使用进程级全局静态状态：
//   - LogHelper._logger / _tempLogger（Logger）
//   - TimerHelper.TimeProvider / 时区 / 时间偏移（Utility）
//   - Snowflake worker / SetWorkerIdProvider（Utility）
//   - Console.Out（Logger.LogConsole / Options 调试输出 / Utility.ConsoleHelper）
//   - 进程环境变量（Options 的 GAMEFRAMEX_OPTIONS_DEBUG / ENVIRONMENT）
// xUnit 默认并行执行不同测试类，会引发这些全局状态的非确定性串扰（例如 Options 的
// Console 捕获意外收到 LogConsole 的边框输出）。统一串行执行以保证测试稳定可靠。
[assembly: CollectionBehavior(DisableTestParallelization = true)]
