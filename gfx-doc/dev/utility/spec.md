# utility spec

`GameFrameX.Foundation.Utility` 模块基线说明。提供基础工具能力，当前覆盖时间辅助 `TimerHelper`。

## 范围

- `TimerHelper`：时间戳与 `DateTime` 互转、当前时间获取、日历运算（Day/Week/Month/Year）、时间差、区间、时区与 UTC 双轨 API。
- 时间戳精度：秒级与毫秒级 Unix 时间戳；纪元基准 `EpochUtc`（1970-01-01 00:00:00 UTC）。

## 公共 API 约定

- 非空签名为主，nullable 边界由 nullable 重载覆盖（`null` 短路返回 `null`，非空路径与非空重载逐字节一致）。
- `utc: bool` 参数双轨：`true` 使用 UTC 纪元，`false` 使用当前设置时区（`CurrentTimeZone`）纪元。
- 统一 UTC 为时间戳基准，禁止混入本地时区偏移解释 `Kind=Unspecified` 的 `DateTime`。
- 时间戳互转的 nullable 重载（C1）：`DateTimeToSecond` / `DateTimeToMilliseconds`（`DateTime? → long?`）、`TimestampSecondToDateTime` / `TimeStampMillisecondToDateTime`（`long? → DateTime?`），覆盖秒/毫秒 × 正向/反向，`null` 短路返回 `null`。

## 已归档变更

- C1 — TimerHelper 增加 nullable 重载（秒/毫秒 × 双向），统一下游时间转换入口（`archived/C1-gfx-153-timerhelper-nullable/change.md`）。
