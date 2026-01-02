# GameFrameX.Foundation.Options 示例程序集

这个程序集包含了 GameFrameX.Foundation.Options 库的完整使用示例和演示代码。

## 项目结构

```
GameFrameX.Foundation.Options.Examples/
├── Program.cs                          # 主程序入口
├── Demos/                              # 演示代码目录
│   ├── BasicUsageDemo.cs              # 基础使用演示
│   ├── StaticMethodsDemo.cs           # 静态方法演示
│   ├── DebugModeDemo.cs               # 调试模式演示
│   ├── AdvancedFeaturesDemo.cs        # 高级特性演示
│   └── ComparisonDemo.cs              # 对比演示
└── README.md                          # 本文件
```

## 运行示例

### 1. 编译项目

```bash
dotnet build GameFrameX.Foundation.Options.Examples
```

### 2. 运行所有演示

```bash
dotnet run --project GameFrameX.Foundation.Options.Examples
```

### 3. 运行特定演示

```bash
# 基础使用演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- basic

# 静态方法演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- static

# 调试模式演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- debug

# 高级特性演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- advanced

# 对比演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- comparison
```

### 4. 使用自定义参数运行

```bash
# 使用自定义参数运行基础演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- basic --app-name "我的应用" --host "example.com" --port 9090 --debug

# 使用自定义参数运行高级演示
dotnet run --project GameFrameX.Foundation.Options.Examples -- advanced --app-name "高级演示" --database-url "postgresql://localhost:5432/demo" --ssl --log-level Warning
```

## 演示内容

### 📋 基础使用演示 (BasicUsageDemo)

展示最基本的使用方式：
- 定义配置类
- 使用 OptionsBuilder 解析参数
- 处理不同数据类型
- 基本的错误处理

**运行命令:**
```bash
dotnet run --project GameFrameX.Foundation.Options.Examples -- basic
```

### ⚡ 静态方法演示 (StaticMethodsDemo)

展示新增的静态便捷方法：
- `Create()` - 基本创建
- `CreateWithDebug()` - 调试模式创建
- `CreateFromArgsOnly()` - 仅使用命令行参数
- `CreateFromEnvironmentOnly()` - 仅使用环境变量
- `CreateDefault()` - 使用默认值
- `TryCreate()` - 安全创建

**运行命令:**
```bash
dotnet run --project GameFrameX.Foundation.Options.Examples -- static
```

### 🐛 调试模式演示 (DebugModeDemo)

展示调试功能：
- 默认启用的调试模式
- 环境变量控制调试输出
- 结构化参数打印
- 配置对象详细信息

**运行命令:**
```bash
dotnet run --project GameFrameX.Foundation.Options.Examples -- debug
```

### 🚀 高级特性演示 (AdvancedFeaturesDemo)

展示高级功能和特性：
- 复杂数据类型支持（枚举、日期时间、GUID等）
- 特性组合使用
- 环境变量与命令行参数优先级
- 错误处理和验证
- 不同布尔参数格式

**运行命令:**
```bash
dotnet run --project GameFrameX.Foundation.Options.Examples -- advanced
```

### ⚖️ 对比演示 (ComparisonDemo)

展示不同使用方式的对比：
- 传统方式 vs 静态方法 vs OptionsProvider
- 性能对比测试
- 功能特性对比
- 使用场景推荐
- 迁移指南

**运行命令:**
```bash
dotnet run --project GameFrameX.Foundation.Options.Examples -- comparison
```

## 学习路径

### 🎯 初学者

1. **基础使用演示** - 了解基本概念和使用方法
2. **静态方法演示** - 学习更简洁的使用方式
3. **调试模式演示** - 掌握调试和问题排查

### 🎯 进阶用户

1. **高级特性演示** - 掌握复杂场景的处理
2. **对比演示** - 了解不同方式的优缺点
3. **查看源码** - 深入理解实现原理

### 🎯 架构师/团队负责人

1. **对比演示** - 选择适合团队的使用方式
2. **性能测试** - 了解性能特征
3. **迁移指南** - 制定迁移计划

## 常见使用模式

### 🔧 简单控制台应用

```csharp
// 最简单的使用方式
var config = OptionsBuilder<AppConfig>.Create(args);
```

### 🔧 Web应用/服务

```csharp
// 启动时初始化
OptionsProvider.Initialize(args);

// 使用时获取配置
var config = OptionsProvider.GetOptions<AppConfig>();
```

### 🔧 开发调试

```csharp
// 启用详细调试信息
var config = OptionsBuilder<AppConfig>.CreateWithDebug(args);
```

### 🔧 生产环境

```csharp
// 通过环境变量控制调试输出
// export GAMEFRAMEX_OPTIONS_DEBUG=false
var config = OptionsProvider.GetOptions<AppConfig>();
```

### 🔧 单元测试

```csharp
// 使用默认值，不受环境变量影响
var config = OptionsBuilder<AppConfig>.CreateDefault();
```

## 环境变量控制

### 调试输出控制

```bash
# 启用调试输出
export GAMEFRAMEX_OPTIONS_DEBUG=true

# 禁用调试输出
export GAMEFRAMEX_OPTIONS_DEBUG=false
```

### 环境检测

```bash
# 设置环境类型（影响默认调试行为）
export ASPNETCORE_ENVIRONMENT=Development  # 默认启用调试
export ASPNETCORE_ENVIRONMENT=Production   # 默认禁用调试
```

## 故障排除

### 编译错误

如果遇到编译错误，请确保：
1. 已安装 .NET 10.0 SDK
2. 已正确引用 GameFrameX.Foundation.Options 项目
3. 所有依赖项都已还原

```bash
dotnet restore GameFrameX.Foundation.Options.Examples
```

### 运行时错误

如果遇到运行时错误：
1. 检查必需参数是否提供
2. 检查参数格式是否正确
3. 启用调试模式查看详细信息

```bash
# 启用调试模式
export GAMEFRAMEX_OPTIONS_DEBUG=true
dotnet run --project GameFrameX.Foundation.Options.Examples -- [demo-name]
```

## 贡献指南

如果你想添加新的演示或改进现有演示：

1. 在 `Demos/` 目录下创建新的演示类
2. 在 `Program.cs` 中添加对应的命令处理
3. 更新本 README 文件
4. 确保代码遵循项目的编码规范
5. 添加适当的注释和文档

## 许可证

本项目遵循与 GameFrameX.Foundation.Options 相同的许可证。