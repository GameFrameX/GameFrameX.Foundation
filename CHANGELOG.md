## [2.2.4] - 2026-03-17

### Bug Fixes

- 添加缺失的 Build 步骤

### Ci

- 合并 NuGet 发布步骤到 release 工作流

## [2.2.3] - 2026-03-17

### Refactor

- CreatedTime 改为非空类型

### Ci

- 添加自动打 tag 和发布版本的工作流
- 升级 GitHub Actions 版本

## [2.3.0.3] - 2026-03-17

### Features

- 添加无长度前缀的字节/字符串读写方法
- 添加毫秒级时间戳转 TimeSpan 的方法

### Miscellaneous Tasks

- 添加 .claude 和 scripts 到 gitignore

### Testing

- 添加无长度前缀读写方法的单元测试

## [2.3.0.2] - 2026-03-17

### Documentation

- 完善 SpanExtensions 文档注释并拆分文件

### Features

- 添加原始类型读写扩展方法

## [2.3.0.1] - 2026-03-17

### Bug Fixes

- 修复Unix时间戳计算未包含时间偏移的问题
- 修复GetCrossDays方法调用错误的重载方法
- 修复时区相关方法命名不一致问题
- 修复时区相关方法命名不一致问题
- 修复时间戳计算中缺少TimeOffset偏移的问题
- 修正年份时间戳方法使用错误的转换函数
- 修复重置时间偏移时未正确设置默认值的问题
- 修正时间戳方法名称以准确反映包含时区偏移
- 修正带偏移的Unix时间计算方法
- 修复获取UTC月份起止时间的方法调用错误
- 修正获取UTC时间的方法名错误
- 统一使用GetNowWithUtc方法获取当前时间
- 在DisposableAction中调用GC.SuppressFinalize
- 修复时间函数委托的线程安全问题
- 将标记特性的 AllowMultiple 改为 false
- 修复线程安全和参数验证问题
- IsSuccess 属性添加 JsonIgnore 特性禁止序列化
- 修复资源泄漏、timeout 失效及参数校验缺陷
- 修复AES加密安全缺陷并重命名解密方法
- 修复RSA签名算法、加密填充及资源泄漏
- 替换已弃用DSACryptoServiceProvider并实现资源释放
- 修复国密算法多项安全与质量缺陷
- 修复 Write 系列越界静默失败及 ReadFloat/ReadDouble 副作用
- 修复 CheckRange/IsRange 开区间语义错误
- 移除 GetDisplayWidth 错误的 Unicode 范围
- 修复 WriteBytesWithoutLength null 处理及 ToDefaultString 跨平台编码
- IsNull/IsNotNull 改用 is null 模式匹配
- 异步方法添加 ConfigureAwait(false)
- 修复 CrcHelper 异常处理问题
- 修复空输入处理与 Sha256Helper 行为一致
- 修复竞态条件并优化代码
- 修复无效密钥处理和跨平台兼容性
- 修复 ToDefaultString 方法编码不一致问题
- 修复 WriteBytesValue 方法 null 处理不一致问题
- 修复 ConvertToUnderLine 方法缺少 this 关键字
- 修复 DirectoryExtensions.CreateAsDirectory 无限递归 bug
- 修复 ObjectExtensions.IsNull/IsNotNull 泛型约束问题
- 修复 NullObject.CompareTo 潜在空引用问题
- 为 IsNull/IsNotNull 添加 NotNullWhen 特性
- 修复线程安全和异常处理问题
- 修复输入验证和 DateTimeKind 问题
- 修复 XML 注释并使用本地化星期名称
- 修复 EntitySelectBase 泛型约束错误
- 修复 EntityBase 设计问题
- 修复 OptionsProvider 线程安全和可见性问题
- 改进异常处理并添加调试输出

### Documentation

- 更新参数注释以明确默认时区行为
- 添加 TimerHelper 成对函数测试结果报告
- 修复示例代码中错误的泛型调用
- 添加XorHelper安全声明注释
- 添加代码审查报告
- 添加 CODE_REVIEW.md 代码审查报告
- 添加 CODE_REVIEW.md 代码审查报告
- 添加版权声明和接口说明
- 更新多个文件的许可证头部信息
- 添加版权声明头部
- 完善 BooleanParser.ParseBooleanValue 返回值说明
- 添加预初始化日志功能使用说明

### Features

- 添加基于UTC的月份时间计算方法
- 添加 UTC 时区的年度时间计算方法
- 添加周时间工具方法并重构时区相关命名
- 添加日期时间工具方法并重构代码结构
- 扩展月份时间工具方法并统一命名规范
- 添加通用的 IsSameWeek 方法并统一时间戳转换
- 补充 TimerHelper UTC 和 TimeZone 成对函数
- 公开 UnicodeJsonEncoder 并添加中文和 Emoji 序列化测试
- 完善 HttpJsonResultData 工厂方法并修复中文转义
- 添加带消息的错误方法重载并保护 IsSuccess 属性
- 新增 PUT、PATCH、DELETE、HEAD、OPTIONS HTTP 扩展方法
- 为 LittleEndian 添加 bytes 和 string 读写方法
- 添加小时范围和星期名称本地化资源
- 添加临时日志记录器支持初始化前日志缓冲

### Miscellaneous Tasks

- 删除未使用的TimerHelper.Current.cs文件
- 添加 CODE_REVIEW 文件到 gitignore 并清理
- 删除已注释的无用代码
- 移除CNB仓库引用并添加缺失的using指令

### Performance

- 优化 ToHash 方法避免装箱开销
- 优化 ByteExtensions 性能并统一异常处理
- 缓存 GetOptionMappings 结果避免重复计算
- 优化 BuildBorder 并改进宽字符判断

### Refactor

- 重构 TimerHelper 类的时间方法到独立文件
- 重构时间工具类，按功能拆分文件
- 重构时间差计算方法并分离关注点
- 重构年份时间方法并分离时区相关逻辑
- 重构周相关时间计算方法并分离关注点
- 重构时间戳工具方法以提高代码组织性
- 将时间偏移方法拆分到独立文件中
- 重构月份时间方法，分离时区相关逻辑
- 重命名 GetCrossDays 方法为 GetCrossDaysUtc
- 重命名时间戳参数以提高可读性
- 重命名时间戳转换方法并移除冗余方法
- 移除带时区时间戳的TimeSpan转换方法
- 将 CurrentTimeZone 属性改为完整属性声明
- 重命名时间转换方法并改进时区处理
- 重构时间工具类的方法命名和位置
- 合并时间戳转换方法并移除无用引用
- 重命名时间差计算方法参数以提高可读性
- 重命名参数以提升代码可读性
- 将 GetTodayStartTime 重命名为 GetTodayStartTimeWithUtc
- 统一使用TimestampSecondToDateTime方法转换时间戳
- 将 TimerHelper 类改为静态类
- 重命名 CurrentTimeWithUtcTime 为 CurrentTimeWithUtc
- 精简时间工具类的 XML 文档注释
- 重构时间工具类中的年份时间戳方法
- 将GetUtcNow替换为GetNowWithUtc以统一命名
- 重命名 GetUtcNow 为 GetNowWithUtc 以明确含义
- 将 LogSavePath 属性从私有设置器改为公共设置器
- 将静态字段重构为属性并更新起始时间
- 使用常量替代默认基准时间的魔数
- 优化实体接口并统一字段为可空类型
- 改进线程安全并清理代码
- 移除 LogHelper.Warn.cs 文件
- 提取共享常量到 HttpJsonResultHelper
- 提取常量到独立的内部类 HttpJsonResultConstants
- IsSuccess 改为只读属性，根据 Code 自动计算
- 提取 IHttpJsonResult 统一接口
- 重命名SM3.cs为SupportClass.cs与类名保持一致
- SpanExtensions BigEndian 写入统一改用 BinaryPrimitives
- 优化 StringExtensions 性能及正确性
- 优化 ByteExtensions 和 SpanExtensions 性能
- 删除被注释掉的异步方法代码
- 统一UTC和TimeZone函数命名规范
- 移除不必要的 unsafe 关键字，使用 Span 替代指针操作
- 将静态方法改为扩展方法
- 提取边界检查辅助方法
- 统一异常消息本地化处理
- 重构 ByteExtensions 并添加 LittleEndian 支持
- 重命名 BigEndian 方法以统一命名规范
- 拆分 ByteExtensions 为多个文件
- 重命名集合扩展方法并优化 RemoveIf
- 标记 ReflectionExtensions 为过时
- ReadOnlySpanExtensions 使用本地化异常消息
- TypeExtensions 统一参数检查模式
- 移除已过时的 ReflectionExtensions 类
- 添加泛型 notnull 约束增强类型安全
- 提取布尔值解析逻辑并优化反射性能
- 代码质量改进

### Styling

- 调整 Logo 格式并添加 Cnb 仓库链接
- 统一代码注释格式并添加 CNB 仓库链接
- 统一代码格式和风格

### Testing

- 修复时区相关测试中的方法名和断言
- 移除已弃用的TimeSpanWithTimeZoneTimestamp测试方法
- 注释掉时间工具类的测试文件
- 重写 HTTP 扩展单元测试为行为驱动测试
- 新增 HTTP 扩展集成测试（httpbin.org 真实端点）
- 补充XorHelper测试并修复AesHelper测试编译错误
- 更新 Sha1Helper 测试用例
- 同步更新函数命名
- 修复加密相关测试用例
- 修复扩展方法测试用例
- 修复本地化服务测试用例
- 更新测试以适配 API 变更
- 添加 LittleEndian bytes/string 方法的单元测试
- 添加 ConvertToUnderLine 方法的单元测试
- 添加代码审查修复的单元测试
- 添加临时日志记录器单元测试

### Build

- 更新 coverlet.collector 依赖版本至 6.0.4
- 启用可空引用类型分析
- 添加 InternalsVisibleTo 支持测试访问内部成员

## [2.2.2] - 2026-02-04

### Features

- 添加包含时区偏移的 Unix 时间戳方法
- 添加基于时区的 Unix 时间戳转换方法
- 添加基于时区的年度时间戳计算方法
- 添加基于时区的周时间戳计算方法
- 添加带时区偏移的时间戳计算方法
- 添加基于时区的月份时间戳方法

### Testing

- 添加时区相关方法的单元测试

## [2.2.1] - 2026-02-04

### Bug Fixes

- 修正 GetNow 方法以使用 CurrentTimeZone 进行时区转换
- 修正时区处理确保时间戳计算正确
- 修正年份相关时间戳计算以使用当前时区
- 修复周时间戳计算中的时区偏移问题
- 修复月份时间戳计算中的时区处理问题
- 修复时间戳转换时区处理错误
- 修复时间戳转换方法中的时区处理逻辑
- 在JSON反序列化错误日志中添加错误信息描述

### Documentation

- 在年份时间方法注释中明确引用当前时区
- 更新周时间方法文档以明确时区引用
- 更新注释以明确使用当前时区而非本地时间
- 统一文档中关于时区的描述术语
- 更新时间戳方法注释中的时区描述
- 更新 TimerHelper 的 README 文档以反映 API 变更

### Features

- 添加时区支持到 TimerHelper

### Refactor

- 替换 DateTime.Now/UtcNow 为 GetNow/GetUtcNow 方法调用
- 移除基于本地时区的时间戳方法
- 统一时间差计算方法使用UTC时间戳

### Testing

- 添加 TimerHelper 时区功能测试用例

## [2.1.2] - 2026-01-28

### Bug Fixes

- 修复JSON序列化时中文字符和Emoji被转义的问题

### Testing

- 添加中文字符序列化不应被转义的测试用例

## [2.1.1.2] - 2026-01-22

### Features

- 添加RSA签名与验签功能支持PKCS#8和PKCS#1格式

## [2.1.0.1] - 2026-01-22

### Bug Fixes

- 将异常消息中的字符串改为逐字字符串

### Features

- 添加支持PKCS#1和PKCS#8格式的RSA加解密方法

### Styling

- 统一异常消息字符串的引号格式

## [2.0.0.2] - 2026-01-02

### Bug Fixes

- 禁用测试项目的程序集签名

## [2.0.0.1] - 2026-01-02

### Refactor

- 集中管理项目配置到Directory.Build.props

## [2.0.0.0] - 2026-01-02

### Documentation

- 添加 GameFrameX.Foundation.Utility 的 README 文档

### Features

- 添加自定义 UnicodeJsonEncoder 并启用不安全代码块

### Miscellaneous Tasks

- 从测试项目中移除Logger文件夹

### Refactor

- 重命名实体类和属性以保持命名一致性

### Build

- 升级项目至.NET 10.0和C# 12.0
- 更新 BouncyCastle.Cryptography 依赖至 2.6.2 版本
- 更新 Serilog 相关依赖包版本

## [1.7.1.11] - 2025-11-20

### Refactor

- 重构实体基类接口并添加安全过滤器

## [1.7.1.10] - 2025-11-19

### Features

- 为资源提供者添加AssemblyName属性防止重复注册

## [1.7.1.9] - 2025-11-19

### Documentation

- 添加目录扩展方法的类注释

### Refactor

- 移除默认资源提供者及相关测试
- 重构资源提供者实现以提高性能和可维护性

## [1.7.1.8] - 2025-11-19

### Documentation

- 修复XML文档注释中的cref格式问题
- 添加本地化框架文档和使用示例

### Features

- 为Utility模块添加多语言支持
- 添加本地化测试模块及相关测试用例
- 添加加密模块本地化支持
- 为扩展模块添加本地化支持
- 为Hash模块添加本地化支持
- 添加本地化服务基础模块

### Refactor

- 为特殊浮点数转换器添加大括号以增强可读性

### Styling

- 优化条件语句的代码格式以提高可读性

## [1.7.1.7] - 2025-11-04

### Refactor

- 移除LogOptions中的模板配置并优化日志处理逻辑

## [1.7.1.6] - 2025-11-04

### Features

- 添加日志类型和自定义文件名支持

## [1.7.1.5] - 2025-11-04

### Refactor

- 移除HelpTextAttribute及相关使用
- 移除未使用的帮助文本列及相关逻辑

### Styling

- 优化控制台输出格式

## [1.7.1.4] - 2025-11-04

### Features

- 添加反射扩展方法集提供统一API行为
- 添加类型接口检查方法
- 添加将字符串转换为下划线命名法的方法

## [1.7.1.3] - 2025-11-04

### Documentation

- 为错误信息和调试输出添加英文翻译

### Refactor

- 移除多余的换行和分隔线输出
- 移除重复的边框打印并重构为独立方法
- 调整日志输出框线默认长度从76到108个字符
- 移除RequiredOptionAttribute并简化必需选项检查逻辑
- 统一使用OptionAttribute替换RequiredOptionAttribute

### Testing

- 移除过时的短参数测试并更新连接字符串参数

## [1.7.1.2] - 2025-11-04

### Features

- 增强选项调试信息的表格化输出显示

### Refactor

- 移除未使用的LogType属性
- 将日志模板中的FriendlyName替换为TagName
- 简化日志输出模板和属性配置
- 优化日志处理器的创建逻辑和参数校验
- 移除调试信息打印代码
- 移除未使用的打印结构化参数方法
- 将类型名称从中文改为使用nameof关键字
- 优化表格列宽计算逻辑并添加多语言支持
- 重构测试程序命名空间和类可见性

## [1.7.1.1] - 2025-11-04

### Features

- 添加目录扩展方法支持递归创建目录
- 添加计算字符串显示宽度的方法
- 添加控制台日志格式化显示功能
- 添加 MongoDB 日志存储支持和日志输出模板配置
- 添加MongoDB日志写入功能
- 添加运行时平台检测工具类及测试

### Refactor

- 移除字符串扩展中创建目录的方法
- 统一 GrafanaLoki 用户名属性命名
- 优化日志配置和MongoDB相关属性命名

### Testing

- 添加表达式扩展和计时器扩展的单元测试

## [1.7.1.0] - 2025-10-23

### Bug Fixes

- 修复LookupX索引器返回空列表时创建新实例的问题

### Documentation

- 完善README文档，新增三个程序集的详细说明

### Features

- 添加 Timer 扩展方法 Reset
- 添加表达式树的逻辑运算扩展方法

### Refactor

- 移除冗余的System命名空间引用并简化Math调用
- 重命名参数以提高代码可读性
- 移除WriteStringValue方法中的unsafe修饰符
- 将CustomExpressionVisitor类合并到ExpressionExtension文件中
- 移除未使用的命名空间引用
- 移除未使用的System命名空间引用
- 移除未使用的命名空间引用
- 移除不必要的System命名空间引用
- 移除 WriteStringValue 方法中的不安全代码
- 移除 WriteSByteValue 方法中的不安全代码
- 移除 WriteBoolValue 方法中的不安全代码
- 优化Timer的Reset方法实现

### Testing

- 添加 TimeHelper 类的单元测试

## [1.7.0.7] - 2025-10-22

### Refactor

- 重命名时间相关方法并改为部分类

## [1.7.0.6] - 2025-10-22

### Features

- 新增TimerHelper工具类及相关功能实现
- 添加Options模块测试示例程序

### Refactor

- 重命名 IndexAttribute 为 EntityIndexAttribute 并优化功能

## [1.7.0.5] - 2025-10-22

### Bug Fixes

- 禁用项目的可空引用类型功能

### Features

- 添加实体索引特性用于标记需要建立索引的属性

### Refactor

- 移除废弃的SqlSugarHelper工具类
- 将 TimerHelper 从 Foundation.Orm.Entity 移动到 Foundation.Utility
- 移除Version属性的SqlSugar版本验证注解

### Build

- 移除不再需要的SqlSugar包依赖

## [1.7.0.4] - 2025-10-22

### Features

- 添加时间辅助工具类用于时间戳处理
- 支持多种类型的时间字段自动填充

## [1.7.0.3] - 2025-10-21

### Refactor

- 将DatacenterId重命名为DataCenterId以保持命名一致性

## [1.7.0.2] - 2025-10-21

### Bug Fixes

- 修正雪花算法基础时间计算错误

### Refactor

- 修改雪花算法基准时间配置方式

## [1.7.0.1] - 2025-10-21

### Bug Fixes

- 修复日志目录创建逻辑以确保路径正确
- 修改日志保存路径，添加data目录
- 修复控制台输出格式中缺少冒号的问题
- 修复乐观锁版本号初始值并添加验证注解

### Documentation

- 将日志配置信息的控制台输出从中文改为英文
- 将调试信息中的中文文本翻译为英文

### Features

- 添加对更多数值类型的本地化支持
- 实现雪花算法ID生成器及相关工具类
- 添加控制台辅助类用于打印logo和项目信息
- 添加环境帮助类用于检测运行环境
- 添加 GameFrameX.Foundation.Utility 项目
- 添加 SqlSugar 辅助工具类处理 AOP 和实体映射

### Refactor

- 移除短名称支持以简化选项属性
- 移除短名称显示并调整类型对齐宽度
- 提取日志配置创建逻辑到独立方法

### Styling

- 调整控制台输出格式以增强可读性
- 调整控制台输出格式以改善可读性

### Build

- 添加 SqlSugar 和 MongoDB 依赖包

## [1.6.0.0] - 2025-10-11

### Bug Fixes

- 添加异常堆栈信息输出以增强调试能力
- 在异常处理中添加完整的异常堆栈信息输出
- 修复环境变量处理和属性名标准化逻辑

### Refactor

- 移除 DefaultValueAttribute 并统一使用 OptionAttribute 的 DefaultValue 参数

### Testing

- 添加环境变量值为空时的默认值测试用例

## [1.0.0] - 2025-01-16

