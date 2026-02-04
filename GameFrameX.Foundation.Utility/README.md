# GameFrameX.Foundation.Utility

通用实用工具库，提供与框架无关的辅助功能。

## 功能模块

### 1. ConsoleHelper
提供控制台相关的辅助功能，例如在控制台中显示不同颜色的文本。

**使用示例：**
```csharp
ConsoleHelper.WriteLine("这是一条普通消息。");
ConsoleHelper.WriteInfo("这是一条信息消息。");
ConsoleHelper.WriteSuccess("操作成功！");
ConsoleHelper.WriteWarning("这是一个警告。");
ConsoleHelper.WriteError("发生了一个错误。");
```

### 2. EnvironmentHelper
提供与应用程序运行环境相关的辅助功能。

**使用示例：**
```csharp
// 判断当前是否为开发环境
if (EnvironmentHelper.IsDevelopment())
{
    // 执行仅在开发环境中运行的代码
}

// 获取当前环境名称
string environmentName = EnvironmentHelper.GetEnvironmentName();
```

### 3. Snowflake ID 生成器
基于 Snowflake 算法的分布式唯一 ID 生成器。

**核心组件：**
- `IdWorker`: Snowflake ID 生成器核心类。
- `SnowFlakeIdHelper`: `IdWorker` 的静态封装，提供更便捷的调用方式。

**使用示例：**
```csharp
// 初始化 IdWorker (通常在应用程序启动时进行一次)
// 参数：workerId, dataCenterId
new IdWorker(1, 1);

// 生成一个新的唯一 ID
long newId = SnowFlakeIdHelper.NewId;
```

### 4. TimerHelper
提供强大的时间处理功能，支持自定义时区、Unix时间戳转换、日期边界计算等。

**核心特性：**

- **时区支持**:
  - `CurrentTimeZone`: 获取或设置当前时区（默认为系统本地时区）。
  - 所有涉及"本地时间"的计算均基于此配置，而非服务器系统时区。

- **时间戳处理**:
  - `UnixTimeSeconds()`: 获取当前 UTC 时间的秒级时间戳。
  - `UnixTimeMilliseconds()`: 获取当前 UTC 时间的毫秒级时间戳。
  - `TimestampToDateTime()` / `MillisecondsTimeStampToDateTime()`: 时间戳转 DateTime。

- **日期边界计算**:
  - `GetTodayStartTime()` / `GetTodayEndTime()`
  - `GetWeekStartTime()` / `GetWeekEndTime()`
  - `GetMonthStartTime()` / `GetMonthEndTime()`
  - `GetYearStartTime()` / `GetYearEndTime()`

- **时间差与经过时间**:
  - `GetElapsedSeconds(timestamp)`: 计算指定时间戳到现在的经过秒数（高效，无 DateTime 转换）。
  - `GetTimeDifference(start, end)`: 计算两个时间的时间差。

- **测试辅助**:
  - `SetTimeOffset()`: 设置时间偏移量，用于模拟过去或未来的时间。

**使用示例：**
```csharp
// 1. 设置时区 (可选，默认为系统本地时区)
TimerHelper.SetTimeZone(TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

// 2. 获取当前时间戳 (UTC)
long timestamp = TimerHelper.UnixTimeSeconds();

// 3. 计算经过时间 (高效)
long elapsed = TimerHelper.GetElapsedSeconds(timestamp);

// 4. 获取特定日期的边界
DateTime weekStart = TimerHelper.GetWeekStartTime(); // 本周一开始时间 (基于 CurrentTimeZone)
DateTime weekEnd = TimerHelper.GetWeekEndTime();     // 本周日结束时间 (基于 CurrentTimeZone)

// 5. 判断是否为同一周
bool isSameWeek = TimerHelper.IsNowSameWeek(lastLoginTime);
```
