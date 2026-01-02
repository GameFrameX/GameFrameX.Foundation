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
提供强大的时间处理功能，涵盖时间戳、日期计算、时间范围判断等。

**核心功能：**
- **时间戳转换**: `ToTimestamp()` / `ToDateTime()`
- **时间偏移**: `ToTimeOffset()`
- **时间范围判断**: `IsBetween()`
- **获取当前时间**: `GetCurrentTime()`
- **日期计算**:
  - `GetDayBegin()` / `GetDayEnd()`
  - `GetWeekBegin()` / `GetWeekEnd()`
  - `GetMonthBegin()` / `GetMonthEnd()`
  - `GetYearBegin()` / `GetYearEnd()`
- **时间差计算**: `GetTimeDifference()`

**使用示例：**
```csharp
// 获取当前时间戳
long timestamp = TimerHelper.ToTimestamp(DateTime.Now);

// 判断某个时间是否在指定范围内
bool isInRange = TimerHelper.IsBetween(DateTime.Now, startTime, endTime);

// 获取本周的开始和结束时间
DateTime weekStart = TimerHelper.GetWeekBegin(DateTime.Now);
DateTime weekEnd = TimerHelper.GetWeekEnd(DateTime.Now);
```
