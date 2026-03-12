# TimerHelperPairTests 测试结果报告

**测试日期:** 2026-03-12
**测试框架:** net8.0, net9.0, net10.0
**测试时区:** UTC+12
**测试总数:** 47
**通过数:** 47
**失败数:** 0

---

## 测试结果汇总

| 框架 | 通过 | 失败 | 总计 | 状态 |
|------|------|------|------|------|
| net8.0 | 47 | 0 | 47 | ✅ 全部通过 |
| net9.0 | 47 | 0 | 47 | ✅ 全部通过 |
| net10.0 | 47 | 0 | 47 | ✅ 全部通过 |

---

## 详细测试结果

### 1. Current - GetNow

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetNow_UtcAndTimeZone_ShouldHaveCorrectOffset` | ✅ 通过 | 验证 UTC 和 TimeZone 时间差约12小时 |

### 2. Current - CurrentTime

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `CurrentTime_UtcAndTimeZone_ShouldReturnValidFormat` | ✅ 通过 | 验证时间格式为6位数字字符串 |

### 3. Current - GetElapsedSeconds/GetElapsedMilliseconds

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetElapsedSeconds_UtcAndTimeZone_ShouldBeConsistent` | ✅ 通过 | 验证经过秒数计算一致性 |
| `GetElapsedMilliseconds_UtcAndTimeZone_ShouldBeConsistent` | ✅ 通过 | 验证经过毫秒数计算一致性 |

### 4. Day - Today

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetTodayStart_Utc_ShouldReturnZeroHour` | ✅ 通过 | UTC 今天开始时间为00:00:00 |
| `GetTodayStart_TimeZone_ShouldReturnZeroHour` | ✅ 通过 | TimeZone 今天开始时间为00:00:00 |
| `GetTodayEnd_Utc_ShouldReturnLastSecond` | ✅ 通过 | UTC 今天结束时间为23:59:59 |
| `GetTodayEnd_TimeZone_ShouldReturnLastSecond` | ✅ 通过 | TimeZone 今天结束时间为23:59:59 |

### 5. Day - Tomorrow

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetTomorrowStart_Utc_ShouldReturnZeroHour` | ✅ 通过 | UTC 明天开始时间为00:00:00，比今天晚1天 |
| `GetTomorrowStart_TimeZone_ShouldReturnZeroHour` | ✅ 通过 | TimeZone 明天开始时间为00:00:00，比今天晚1天 |

### 6. Week - UTC Functions

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetWeekStart_Utc_ShouldReturnMonday` | ✅ 通过 | UTC 本周开始时间为周一00:00:00 |
| `GetWeekEnd_Utc_ShouldReturnSunday` | ✅ 通过 | UTC 本周结束时间为周日23:59:59 |
| `GetNextWeekStart_Utc_ShouldReturnMonday` | ✅ 通过 | UTC 下周开始时间为周一，比本周晚7天 |
| `GetStartTimestampOfWeek_Utc_ShouldReturnCorrectTimestamp` | ✅ 通过 | UTC 指定日期所在周开始时间戳正确 |
| `GetEndTimestampOfWeek_Utc_ShouldReturnCorrectTimestamp` | ✅ 通过 | UTC 指定日期所在周结束时间戳正确 |

### 7. Week - TimeZone Functions

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetWeekStart_TimeZone_ShouldReturnMonday` | ✅ 通过 | TimeZone 本周开始时间为周一00:00 |
| `GetWeekEnd_TimeZone_ShouldReturnSunday` | ✅ 通过 | TimeZone 本周结束时间为周日23:00 |
| `GetNextWeekStart_TimeZone_ShouldReturnMonday` | ✅ 通过 | TimeZone 下周开始时间为周一，比本周晚7天 |
| `GetDayOfWeekTimeWithTimeZone_AllDays_ShouldReturnCorrectDay` | ✅ 通过 | TimeZone 获取本周指定星期几时间正确（7天全部测试） |

#### GetDayOfWeekTimeWithTimeZone 测试详情

| 参数 (DayOfWeek) | 状态 |
|------------------|------|
| Sunday | ✅ 通过 |
| Monday | ✅ 通过 |
| Tuesday | ✅ 通过 |
| Wednesday | ✅ 通过 |
| Thursday | ✅ 通过 |
| Friday | ✅ 通过 |
| Saturday | ✅ 通过 |

### 8. Month - UTC Functions

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetMonthStart_Utc_ShouldReturnFirstDay` | ✅ 通过 | UTC 本月开始时间为1号00:00 |
| `GetMonthEnd_Utc_ShouldReturnLastDay` | ✅ 通过 | UTC 本月结束时间为最后一天23:59:59 |
| `GetNextMonthStart_Utc_ShouldReturnFirstDay` | ✅ 通过 | UTC 下月开始时间为1号00:00 |

### 9. Month - TimeZone Functions

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetMonthStart_TimeZone_ShouldReturnFirstDay` | ✅ 通过 | TimeZone 本月开始时间为1号00:00 |
| `GetMonthEnd_TimeZone_ShouldReturnLastDay` | ✅ 通过 | TimeZone 本月结束时间为最后一天23:59:59 |
| `GetNextMonthStart_TimeZone_ShouldReturnFirstDay` | ✅ 通过 | TimeZone 下月开始时间为1号00:00 |

### 10. Year - UTC Functions

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetYearStart_Utc_ShouldReturnJanuary1st` | ✅ 通过 | UTC 今年开始时间为1月1日00:00 |
| `GetYearEnd_Utc_ShouldReturnDecember31st` | ✅ 通过 | UTC 今年结束时间为12月31日23:59:59 |
| `GetNextYearStart_Utc_ShouldReturnJanuary1st` | ✅ 通过 | UTC 明年开始时间为1月1日，比今年晚1年 |
| `GetStartTimestampOfYear_Utc_ShouldReturnCorrectTimestamp` | ✅ 通过 | UTC 指定日期所在年开始时间戳正确 |
| `GetEndTimestampOfYear_Utc_ShouldReturnCorrectTimestamp` | ✅ 通过 | UTC 指定日期所在年结束时间戳正确 |

### 11. Year - TimeZone Functions

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetYearStart_TimeZone_ShouldReturnJanuary1st` | ✅ 通过 | TimeZone 今年开始时间为1月1日00:00 |
| `GetNextYearStart_TimeZone_ShouldReturnJanuary1st` | ✅ 通过 | TimeZone 明年开始时间为1月1日，比今年晚1年 |

### 12. Difference - TimeDifference

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetTimeDifferenceWithTimeZone_ShouldReturnCorrectDifference` | ✅ 通过 | 计算两个时间戳差值约2小时 |
| `GetTimeDifferenceMillisecondWithTimeZone_ShouldReturnCorrectDifference` | ✅ 通过 | 计算两个毫秒时间戳差值约5秒 |

### 13. Difference - FromNow

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `GetTimeDifferenceFromNow_DateTime_ShouldReturnCorrectDifference` | ✅ 通过 | 计算指定时间到当前时间差约3小时 |
| `GetTimeDifferenceFromNow_Timestamp_ShouldReturnCorrectDifference` | ✅ 通过 | 计算指定时间戳到当前时间差约30分钟 |
| `GetTimeDifferenceFromNowMs_ShouldReturnCorrectDifference` | ✅ 通过 | 计算指定毫秒时间戳到当前时间差约45秒 |
| `GetElapsedSeconds_DateTime_ShouldReturnCorrectSeconds` | ✅ 通过 | 计算指定时间到当前时间经过约120秒 |

### 14. Timestamp - TimeSpan

| 测试函数 | 状态 | 说明 |
|----------|------|------|
| `TimeSpanWithTimestamp_UtcAndTimeZone_ShouldReturnCorrectTimeSpan` | ✅ 通过 | 时间戳3600转换为1小时TimeSpan |
| `TimeSpanWithTimestamp_WithZero_ShouldReturnZero` | ✅ 通过 | 时间戳0转换为Zero TimeSpan |
| `TimeSpanWithTimestamp_WithOutOfRange_ShouldThrowException` | ✅ 通过 | 无效时间戳抛出ArgumentOutOfRangeException |

---

## 测试覆盖的函数列表

### UTC 函数

| 函数名 | 测试覆盖 |
|--------|----------|
| `GetNowWithUtc` | ✅ |
| `CurrentTimeWithUtc` | ✅ |
| `CurrentTimeWithUtcFullString` | ✅ |
| `GetElapsedSeconds` | ✅ |
| `GetElapsedMilliseconds` | ✅ |
| `GetTodayStartTimeWithUtc` | ✅ |
| `GetTodayEndTimeWithUtc` | ✅ |
| `GetTodayStartTimestampWithUtc` | ✅ |
| `GetTomorrowStartTimeWithUtc` | ✅ |
| `GetWeekStartTimeWithUtc` | ✅ |
| `GetWeekEndTimeWithUtc` | ✅ |
| `GetWeekStartTimestampWithUtc` | ✅ |
| `GetWeekEndTimestampWithUtc` | ✅ |
| `GetNextWeekStartTimeWithUtc` | ✅ |
| `GetNextWeekStartTimestampWithUtc` | ✅ |
| `GetNextWeekEndTimeWithUtc` | ✅ |
| `GetNextWeekEndTimestampWithUtc` | ✅ |
| `GetStartTimestampOfWeekWithUtc` | ✅ |
| `GetEndTimestampOfWeekWithUtc` | ✅ |
| `GetMonthStartTimeWithUtc` | ✅ |
| `GetMonthEndTimeWithUtc` | ✅ |
| `GetNextMonthStartTimeWithUtc` | ✅ |
| `GetYearStartTimeWithUtc` | ✅ |
| `GetYearEndTimeWithUtc` | ✅ |
| `GetYearStartTimestampWithUtc` | ✅ |
| `GetNextYearStartTimeWithUtc` | ✅ |
| `GetStartTimestampOfYearWithUtc` | ✅ |
| `GetEndTimestampOfYearWithUtc` | ✅ |
| `TimeSpanWithTimestamp` | ✅ |

### TimeZone 函数

| 函数名 | 测试覆盖 |
|--------|----------|
| `GetNowWithTimeZone` | ✅ |
| `CurrentTimeWithTimeZone` | ✅ |
| `CurrentTimeWithTimeZoneFullString` | ✅ |
| `GetElapsedSecondsWithTimeZone` | ✅ |
| `GetElapsedMillisecondsWithTimeZone` | ✅ |
| `GetTodayStartTimeWithTimeZone` | ✅ |
| `GetTodayEndTimeWithTimeZone` | ✅ |
| `GetTodayStartTimestampWithTimeZone` | ✅ |
| `GetTomorrowStartTimeWithTimeZone` | ✅ |
| `GetWeekStartTimeWithTimeZone` | ✅ |
| `GetWeekEndTimeWithTimeZone` | ✅ |
| `GetWeekStartTimestampWithTimeZone` | ✅ |
| `GetWeekEndTimestampWithTimeZone` | ✅ |
| `GetNextWeekStartTimeWithTimeZone` | ✅ |
| `GetNextWeekStartTimestampWithTimeZone` | ✅ |
| `GetNextWeekEndTimeWithTimeZone` | ✅ |
| `GetNextWeekEndTimestampWithTimeZone` | ✅ |
| `GetStartTimestampOfWeekWithTimeZone` | ✅ |
| `GetEndTimestampOfWeekWithTimeZone` | ✅ |
| `GetDayOfWeekTimeWithTimeZone` | ✅ |
| `GetMonthStartTimeWithTimeZone` | ✅ |
| `GetMonthEndTimeWithTimeZone` | ✅ |
| `GetStartTimestampOfMonthWithTimeZone` | ✅ |
| `GetEndTimestampOfMonthWithTimeZone` | ✅ |
| `GetNextMonthStartTimeWithTimeZone` | ✅ |
| `GetYearStartTimeWithTimeZone` | ✅ |
| `GetYearStartTimestampWithTimeZone` | ✅ |
| `GetNextYearStartTimeWithTimeZone` | ✅ |
| `GetStartTimestampOfYearWithTimeZone` | ✅ |
| `GetEndTimestampOfYearWithTimeZone` | ✅ |
| `GetTimeDifferenceWithTimeZone` | ✅ |
| `GetTimeDifferenceMillisecondWithTimeZone` | ✅ |
| `GetTimeDifferenceFromNowWithTimeZone` (DateTime) | ✅ |
| `GetTimeDifferenceFromNowWithTimeZone` (long) | ✅ |
| `GetTimeDifferenceFromNowMsWithTimeZone` | ✅ |
| `GetElapsedSecondsWithTimeZone` (DateTime) | ✅ |
| `TimeSpanWithTimestampWithTimeZone` | ✅ |

---

## 结论

所有 47 个测试用例在 net8.0、net9.0 和 net10.0 三个目标框架上均通过测试。测试覆盖了：

- **Current**: 时间获取、格式化、经过时间计算
- **Day**: 今天/明天的开始和结束时间
- **Week**: 本周/下周的开始和结束时间，指定星期几获取
- **Month**: 本月/下月的开始和结束时间
- **Year**: 今年/明年的开始和结束时间
- **Difference**: 时间差计算
- **Timestamp**: 时间戳与TimeSpan转换

UTC 和 TimeZone 版本的函数都已正确实现并通过配对测试验证。
