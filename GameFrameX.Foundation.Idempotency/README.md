# GameFrameX.Foundation.Idempotency

[![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Idempotency.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency/)
[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](https://github.com/GameFrameX/GameFrameX/blob/main/LICENSE)

GameFrameX.Foundation.Idempotency 是 GameFrameX 框架的幂等与事件信封基础库，交付两条原语：

- **幂等**：占位 → 执行 → 回放。同一业务意图（作用域 + 键 + 请求摘要）在并发与重试下恰好执行一次，重复请求回放首次响应。
- **事件信封**：`EventEnvelope` 标准载体 + 发布抽象 + 消费端去重，为跨进程、跨模块的事件传递提供统一结构。

全部协作点均为"接口 + 进程内默认实现"，生产环境替换为持久化实现即可获得跨进程、跨重启的正确性。

## 🎯 核心特性

- **判定语义完备** - 协调器对一次占位请求给出四种裁决：执行 / 回放 / 冲突 / 忙碌，并发恰一由存储契约保证
- **七个协作点全部可替换** - 存储、时钟、选项、键生成器、摘要器、事件发布器、去重器
- **时间规范统一** - 全包 `long` UTC 毫秒；时间点 `*Time`、时长 `*Milliseconds`，取时唯一入口 `IClock`
- **领域无关** - 作用域与键对包不透明，不含任何具体业务词汇
- **事件信封不可变** - `Payload` 不透明字节 + `Attributes` 独立快照，构造后外部修改不影响信封
- **异常消息本地化** - 内置中英双语资源，经 `LocalizationService` 按 culture 解析

## 📦 安装

```bash
dotnet add package GameFrameX.Foundation.Idempotency
```

## 🚀 快速开始

### 幂等占位：Begin → Execute → Complete

```csharp
using System.Text;
using GameFrameX.Foundation.Idempotency;

// 组合协调器：存储 + 时钟（选项、键生成器、摘要器走默认值）
InMemoryIdempotencyStore idempotencyStore = new InMemoryIdempotencyStore();
IdempotencyCoordinator coordinator = new IdempotencyCoordinator(idempotencyStore, SystemClock.Instance);

// 请求摘要：字段规范化文本 → SHA-256 → Base64（排除不应参与摘要的字段，如时间戳）
string canonicalText = CanonicalText.BuildCanonicalText(
    new Dictionary<string, string>
    {
        { "orderId", "202609120001" },
        { "amount", "99.00" },
    });
ReadOnlyMemory<byte> canonicalPayload = Encoding.UTF8.GetBytes(canonicalText);
string requestDigest = Sha256RequestDigester.Instance.ComputeDigest(canonicalPayload);

// 发起占位判定
IdempotencyDecision decision = await coordinator.BeginAsync(
    new IdempotencyBeginRequest("order.create", "202609120001", requestDigest));

switch (decision.DecisionKind)
{
    case IdempotencyDecisionKind.Execute:
        // 首次见到该意图：执行业务，结束后落定记录
        ReadOnlyMemory<byte> firstResponse = Encoding.UTF8.GetBytes("{\"ok\":true}");
        await DoBusinessAsync();
        await coordinator.CompleteAsync(
            new IdempotencyCompletionRequest(
                new IdempotencyRecordIdentifier("order.create", "202609120001"),
                firstResponse, 0));
        break;

    case IdempotencyDecisionKind.Replay:
        // 已执行过：回放首次响应或原错误，不得再次执行
        if (decision.ExistingRecord!.Status == IdempotencyStatus.Completed)
        {
            byte[] replayed = decision.ExistingRecord.FirstResponse!.Value.ToArray();
        }

        break;

    case IdempotencyDecisionKind.Conflict:
        // 同一键下请求摘要不一致：拒绝执行，由调用方决定报错或换键
        break;

    case IdempotencyDecisionKind.Busy:
        // 并发占位方在等待超时内未结束：稍后重试
        break;
}
```

### 事件信封：构造 → 发布 → 消费去重

```csharp
using System.Text;
using GameFrameX.Foundation.Idempotency;

InMemoryEventPublisher eventPublisher = new InMemoryEventPublisher();
InMemoryEventDeduplicator eventDeduplicator = new InMemoryEventDeduplicator();

eventPublisher.Subscribe(envelope =>
{
    // 消费端先去重再处理：重复事件直接跳过
    if (!eventDeduplicator.TryConsume(envelope.EventId))
    {
        return;
    }

    ProcessPayload(envelope.Payload);
});

EventEnvelope envelope = new EventEnvelope(
    eventId: GuidEventIdGenerator.Instance.NewEventId(),
    eventType: "order.created",
    occurredTime: SystemClock.Instance.UtcNowTime,
    schemaVersion: 1,
    source: "order-service",
    correlationId: null,
    payload: Encoding.UTF8.GetBytes("{\"orderId\":\"202609120001\"}"),
    attributes: null);

await eventPublisher.PublishAsync(envelope);
```

## 📋 判定语义表

`IdempotencyCoordinator.BeginAsync` 的全部分支：

| 存量记录状态 | 请求摘要 | 裁决 | 说明 |
|---|---|---|---|
| 无记录 | - | `Execute` | 占位为执行中 |
| 已完成 | 相同 | `Replay` | 携带首次响应回放，不得再次执行 |
| 已完成 | 不同 | `Conflict` | 键冲突，拒绝执行 |
| 已失败（策略 ReplayError） | - | `Replay` | 调用方映射回原错误 |
| 已失败（策略 ReExecute） | - | `Execute` | 覆盖占位后再次执行 |
| 执行中（等待不超时） | - | 等待重判 | 周期性重读记录，状态变化即重判 |
| 执行中（等待超时） | - | `Busy` | 并发占位方长时间未结束，稍后重试 |
| 任意状态（已过期） | - | `Execute` | 视为无记录，新占位覆盖 |

占位被并发方抢占时按上表重判一次，仍被抢占返回 `Busy`。

## 🔌 七个协作点（接口 + 默认实现）

| 协作点 | 接口 | 进程内默认实现 | 生产替换示例 |
|---|---|---|---|
| 幂等存储 | `IIdempotencyStore` | `InMemoryIdempotencyStore` | 数据库存储（必须保证并发恰一） |
| 时间源 | `IClock` | `SystemClock` | 偏移时钟 / 虚拟时钟 |
| 幂等选项 | `IdempotencyOptions` | 直接使用 | 按业务调整保留时长与失败策略 |
| 键生成器 | `IIdempotencyKeyGenerator` | `GuidIdempotencyKeyGenerator` | 业务键直接充当 |
| 请求摘要器 | `IRequestDigester` | `Sha256RequestDigester` | 其他确定性哈希 |
| 事件发布器 | `IEventPublisher` | `InMemoryEventPublisher` | 消息队列发布器 |
| 事件去重器 | `IEventDeduplicator` | `InMemoryEventDeduplicator` | 持久化去重表 |

## 🧭 选型指引：作用域 / 键 / 请求摘要

- **`IdempotencyScope`（作用域）**：业务隔离维度。对包不透明，格式由使用方定义（如 `"order.create"`）；作用域之间的隔离由使用方负责。
- **`IdempotencyKey`（键）**：标识一次业务意图。有天然业务键（订单号）时直接使用；无业务键或键可能过粗时用 `IIdempotencyKeyGenerator` 生成并由调用方持久化绑定。
- **`RequestDigest`（请求摘要）**：识别同一键下请求内容是否一致。相同 → 同一意图可重放；不同 → 键冲突拒绝执行。用 `CanonicalText.BuildCanonicalText` + `IRequestDigester` 计算，记得排除时间戳、随机数等不应参与判定的字段。

## ⏱️ 命名与时间规范

- 时间点一律 `long` UTC 毫秒，命名 `<过去分词>Time`（`CreatedTime` / `ExpiredTime` / `FinishedTime` / `OccurredTime` 等）
- 时长一律 `long` 毫秒，命名 `<名词>Milliseconds`（`RetentionMilliseconds` / `ConcurrentWaitTimeoutMilliseconds`）
- 包内不使用任何日期结构类型，取时唯一入口 `IClock`（默认实现委托 `TimerHelper.UnixTimeMilliseconds()`）

## ⚠️ 默认实现边界（必读）

`InMemoryIdempotencyStore` 与 `InMemoryEventDeduplicator` 的数据仅存活于当前进程内存：

- 进程重启即失忆；去重器容量逐出后旧标识可能再次通过
- 适用于单进程防并发重复与单元测试
- **跨进程、跨重启的正确性必须替换为持久化实现**（如数据库存储 + 过期索引）

## 📐 扩展契约

外部实现 `IIdempotencyStore` 时的硬性要求：

1. **并发恰一**：同一标识（作用域 + 键）的并发 `TryBeginProcessingAsync` 调用中，存量为执行中或已成功（且未过期）时恰好一个返回 `true`
2. **两类法定覆盖**：存量已过期（以新记录创建时刻为基准）或存量为 `Failed` 时，新记录必须覆盖旧记录并返回 `true`
3. **跨进程、跨重启保持判定结果**；支持 `RemoveExpiredRecordsAsync` 语义或声明等价机制
4. **签名扩展只允许通过给参数对象追加字段实现**，不改变方法签名

## 📦 依赖

| 依赖 | 用途 |
|---|---|
| `GameFrameX.Foundation.Utility` | 时间源（`TimerHelper`，传递引入 Localization） |

零外部 NuGet 包依赖。
