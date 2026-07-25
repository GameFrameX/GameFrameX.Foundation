# change

## 1. Proposal

### Problem

下游项目（如 `GameFrameX.Admin.Unified.Api`，C144 重构后）已将业务 `*Time` 字段统一为 `long?`（UTC 秒级 Unix 时间戳）。在与 Furion.Schedule 的 `Trigger` / `JobDetail`、外部 API、数据库边界交互时，频繁需要 `DateTime? ↔ long?` 的 nullable 双向转换。

当前 `TimerHelper` 仅提供非空签名（`DateTime` / `long`），下游在 nullable 边界被迫自建并行封装（如 Admin 的 `ScheduleTimeBridge`、`DateTimeUtility`），导致：

1. 时间转换入口分裂为多套，违背「唯一时间转换入口」原则；
2. 部分自建封装混入本地时区偏移（`new DateTimeOffset(dt).ToUnixTimeSeconds()` 会按本地时区解释 `Kind=Unspecified` 的 `DateTime`），与项目「统一 UTC」约定冲突；
3. 同一个 `DateTime` 在不同入口得到不同时间戳，最大可差一个时区偏移。

### Goal

在 `GameFrameX.Foundation.Utility` 的 `TimerHelper` 上补齐秒/毫秒 × 正向/反向 共 4 个 nullable 重载，全部委托给现有非空实现，`null` 直接短路返回，使下游可以删除自建封装、统一收敛到本仓库的单一时间转换入口。

### Background

- 关联 Linear issue：GFX-153（P1）。
- 本 change 是下游 Admin 治理任务（删除 `ScheduleTimeBridge`、迁移 `DateTimeUtility`、收敛散落手写转换）的前置门槛；新版本 NuGet 发布后下游才能启动迁移。
- 版本发布由 `.github/workflows/release.yml` 自动化：`feat` conventional commit 触发 minor bump（2.7.x → 2.8.0），自动更新 `Version.props` / `CHANGELOG.md`、发布 NuGet 并打 tag。本 change 不手改 `Version.props`，避免与发布自动化冲突。

## 2. Scope

### In Scope

- 新增 partial 文件 `GameFrameX.Foundation.Utility/Time/TimerHelper.Nullable.cs`，加齐 4 个 nullable 重载：
  - `long? DateTimeToSecond(DateTime? time, bool utc = false)`
  - `long? DateTimeToMilliseconds(DateTime? time, bool utc = false)`
  - `DateTime? TimestampSecondToDateTime(long? utcTimestampSeconds, bool utc = false)`
  - `DateTime? TimeStampMillisecondToDateTime(long? utcTimestampMilliseconds, bool utc = false)`
- 新增测试 `GameFrameX.Foundation.Tests/Utility/TimerHelperNullableTests.cs`：null 短路 + 非 null 与非空重载结果一致（覆盖 `utc: true` / `utc: false`）。

### Out of Scope

- 当前时间获取类（`UnixTimeSeconds` / `UnixTimeMilliseconds` 等无参方法）——返回值天然非空，不加 nullable 重载。
- 时间组件提取 / 日历运算类（`Day` / `Month` / `Year` / `Week` / `Difference` / `Range` 系列）——无 nullable 边界场景。
- `TimestampToTicks` 及带时区偏移变体——下游无 nullable 调用需求。
- 不修改 `Version.props` / `CHANGELOG.md`——由发布自动化负责。
- 不触碰下游 Admin 仓库——下游迁移由下游任务自行处理。

## 3. Spec Delta

### ADDED

- `TimerHelper.DateTimeToSecond(DateTime?, bool)` → `long?`：null 短路返回 null，非空委托 `DateTimeToSecond(DateTime, bool)`。
- `TimerHelper.DateTimeToMilliseconds(DateTime?, bool)` → `long?`：null 短路返回 null，非空委托 `DateTimeToMilliseconds(DateTime, bool)`。
- `TimerHelper.TimestampSecondToDateTime(long?, bool)` → `DateTime?`：null 短路返回 null，非空委托 `TimestampSecondToDateTime(long, bool)`。
- `TimerHelper.TimeStampMillisecondToDateTime(long?, bool)` → `DateTime?`：null 短路返回 null，非空委托 `TimeStampMillisecondToDateTime(long, bool)`。

### MODIFIED

- 无。非空路径与现有方法逐字节一致，零行为变更。

### REMOVED

- 无。

## 4. Tasks

- [x] 建 change.md（ID=C1，Status=planned）并同步模块级 / 项目级索引。
- [x] 新增 `GameFrameX.Foundation.Utility/Time/TimerHelper.Nullable.cs`（4 个 nullable 重载）。
- [x] 新增 `GameFrameX.Foundation.Tests/Utility/TimerHelperNullableTests.cs`（null 短路 + 非 null 等价，覆盖 utc 双分支）。
- [x] `dotnet build` + `dotnet test` 全量通过。
- [x] 以 `feat(utility): ...` conventional commit 提交，触发 release.yml 自动 minor bump（2.7.0 → 2.8.0）+ NuGet 发布 + tag。

## 5. Verification

### Tests

- `TimerHelperNullableTests`：4 个方法 × 2 用例（null 短路 / 非 null 与非空重载一致）= 8 个 `[Fact]`。
- 非 null 等价用例同时断言 `utc: true` 与 `utc: false` 两个分支。
- 复用现有 `[Collection("TimerHelper")]` 串行集合，构造时 `SetTimeZone(Utc)`、`Dispose` 复位，避免污染其它 TimerHelper 测试。

### Acceptance Criteria

- null 输入 4 个方法各返回 null。
- 非 null 输入 4 个方法与非空重载结果完全一致（utc 双分支）。
- `dotnet test` 全量通过，现有 `TimerHelperPairTests` / `TimerHelperTimeZoneTests` 不受影响。
- 版本号 minor bump（2.7.x → 2.8.0）由 release.yml 在 PR 合并到 main 时自动完成，并发布 NuGet + 打 tag。

### Review Notes

- ID: C1
- Title: TimerHelper 增加 nullable 重载（秒/毫秒 × 双向），统一下游时间转换入口
- Slug: gfx-153-timerhelper-nullable
- Type: change
- Status: archived
- Backlog: none
- Parent Change: none
- Derived Changes: []
- Depends On: []
- Blocks: []
- Mode: fast
- Risk: 低——纯新增 partial，非空路径零行为变更；nullable 重载内部直接委托，无新逻辑。
- Dependencies: 无；下游 Admin 迁移任务依赖本 change 发布的新版本 NuGet。
