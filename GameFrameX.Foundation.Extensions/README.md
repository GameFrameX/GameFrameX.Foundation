# GameFrameX.Foundation.Extensions

GameFrameX.Foundation.Extensions 是一个功能丰富的 .NET 扩展方法库，提供了大量实用的扩展方法和工具类，用于简化日常开发工作。该库包含字符串处理、集合操作、类型转换、对象操作等多个方面的扩展功能。

## 特性

- 🔧 **丰富的扩展方法** - 提供字符串、集合、对象、类型等多种扩展方法
- 🚀 **高性能** - 优化的算法实现，确保高效执行
- 🛡️ **类型安全** - 完整的泛型支持和类型检查
- 📦 **轻量级** - 无外部依赖，易于集成
- 🔄 **双向映射** - 提供双向字典等高级数据结构
- 🎯 **线程安全** - 部分组件支持并发操作

## 安装

```bash
dotnet add package GameFrameX.Foundation.Extensions
```

## 主要组件

### 1. 字符串扩展 (StringExtensions)

提供丰富的字符串处理扩展方法：

```csharp
using GameFrameX.Foundation.Extensions;

// 字符串验证
string email = "user@example.com";
bool isValid = email.IsValidEmail(); // true

string url = "https://www.example.com";
bool isValidUrl = url.IsValidUrl(); // true

// 字符串转换
string text = "hello world";
string camelCase = text.ToCamelCase(); // "helloWorld"
string pascalCase = text.ToPascalCase(); // "HelloWorld"
string kebabCase = text.ToKebabCase(); // "hello-world"

// 字符串截取
string longText = "This is a very long text";
string truncated = longText.Truncate(10); // "This is a..."
string truncatedCustom = longText.Truncate(10, "***"); // "This is a***"

// 安全转换
string numberStr = "123";
int number = numberStr.ToIntOrDefault(); // 123
int defaultValue = "abc".ToIntOrDefault(999); // 999

// 字符串清理
string dirtyText = "  Hello\t\nWorld  ";
string cleaned = dirtyText.CleanWhitespace(); // "Hello World"

// Base64 编码/解码
string original = "Hello World";
string encoded = original.ToBase64(); // "SGVsbG8gV29ybGQ="
string decoded = encoded.FromBase64(); // "Hello World"
```

### 2. 集合扩展 (CollectionExtensions & IEnumerableExtensions)

提供强大的集合操作扩展：

```csharp
using GameFrameX.Foundation.Extensions;

// 集合判断
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
bool isEmpty = numbers.IsNullOrEmpty(); // false
bool hasElements = numbers.IsNotNullOrEmpty(); // true

// 安全操作
List<string> items = null;
items.AddIfNotNull("test"); // 不会抛出异常

List<string> validItems = new List<string> { "a", "b", "c" };
validItems.AddIfNotNull("d"); // 添加成功

// 批量操作
var moreItems = new[] { "e", "f", "g" };
validItems.AddRange(moreItems);

// 去重操作
var duplicates = new[] { 1, 2, 2, 3, 3, 4 };
var unique = duplicates.Distinct().ToList(); // [1, 2, 3, 4]

// 分页操作
var allItems = Enumerable.Range(1, 100);
var page1 = allItems.Skip(0).Take(10); // 第1页，每页10条
var page2 = allItems.Skip(10).Take(10); // 第2页，每页10条

// 随机选择
var randomItem = numbers.RandomElement(); // 随机选择一个元素
var randomItems = numbers.RandomElements(3); // 随机选择3个元素

// 分组操作
var people = new[]
{
    new { Name = "Alice", Age = 25 },
    new { Name = "Bob", Age = 30 },
    new { Name = "Charlie", Age = 25 }
};
var groupedByAge = people.GroupBy(p => p.Age);
```

### 3. 对象扩展 (ObjectExtensions)

提供对象操作和转换的扩展方法：

```csharp
using GameFrameX.Foundation.Extensions;

// 空值检查
object obj = null;
bool isNull = obj.IsNull(); // true
bool isNotNull = obj.IsNotNull(); // false

// 类型检查和转换
object value = "123";
bool isString = value.Is<string>(); // true
string stringValue = value.As<string>(); // "123"

// 安全转换
object numberObj = 42;
int? intValue = numberObj.AsOrDefault<int>(); // 42
string stringFromInt = numberObj.AsOrDefault<string>(); // null

// 深拷贝（如果对象支持序列化）
var original = new { Name = "Test", Value = 123 };
var copy = original.DeepClone(); // 深拷贝对象

// 属性复制
public class Source { public string Name { get; set; } public int Age { get; set; } }
public class Target { public string Name { get; set; } public int Age { get; set; } }

var source = new Source { Name = "Alice", Age = 25 };
var target = new Target();
source.CopyPropertiesTo(target); // target.Name = "Alice", target.Age = 25
```

### 4. 类型扩展 (TypeExtensions)

提供类型信息和反射操作的扩展：

```csharp
using GameFrameX.Foundation.Extensions;

// 类型检查
Type stringType = typeof(string);
bool isNullable = stringType.IsNullable(); // false

Type nullableIntType = typeof(int?);
bool isNullableInt = nullableIntType.IsNullable(); // true

// 获取默认值
Type intType = typeof(int);
object defaultValue = intType.GetDefaultValue(); // 0

// 类型比较
bool isAssignable = typeof(string).IsAssignableFrom(typeof(object)); // false
bool isAssignableReverse = typeof(object).IsAssignableFrom(typeof(string)); // true

// 获取泛型参数
Type listType = typeof(List<string>);
Type[] genericArgs = listType.GetGenericArguments(); // [typeof(string)]

// 检查是否为集合类型
bool isList = typeof(List<int>).IsCollection(); // true
bool isArray = typeof(int[]).IsCollection(); // true
bool isString = typeof(string).IsCollection(); // false (字符串不被视为集合)
```

### 5. 字节扩展 (ByteExtensions)

提供字节数组处理的扩展方法：

```csharp
using GameFrameX.Foundation.Extensions;

// 字节数组转换
byte[] bytes = { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello" in ASCII
string text = bytes.ToUtf8String(); // "Hello"
string hex = bytes.ToHexString(); // "48656C6C6F"
string hexWithSeparator = bytes.ToHexString("-"); // "48-65-6C-6C-6F"

// 字符串转字节数组
string message = "Hello World";
byte[] utf8Bytes = message.ToUtf8Bytes();
byte[] asciiBytes = message.ToAsciiBytes();

// 十六进制字符串转字节数组
string hexString = "48656C6C6F";
byte[] fromHex = hexString.FromHexString(); // [0x48, 0x65, 0x6C, 0x6C, 0x6F]

// Base64 转换
byte[] data = { 1, 2, 3, 4, 5 };
string base64 = data.ToBase64String(); // "AQIDBAU="
byte[] fromBase64 = base64.FromBase64String(); // [1, 2, 3, 4, 5]
```

### 6. 双向字典 (BidirectionalDictionary)

提供键值双向映射的高效数据结构：

```csharp
using GameFrameX.Foundation.Extensions;

// 创建双向字典
var biDict = new BidirectionalDictionary<string, int>();

// 添加键值对
bool added1 = biDict.TryAdd("one", 1); // true
bool added2 = biDict.TryAdd("two", 2); // true
bool added3 = biDict.TryAdd("one", 3); // false - 键已存在
bool added4 = biDict.TryAdd("three", 1); // false - 值已存在

// 双向查找
if (biDict.TryGetValue("one", out int value))
{
    Console.WriteLine($"Key 'one' maps to value {value}"); // 输出: 1
}

if (biDict.TryGetKey(2, out string key))
{
    Console.WriteLine($"Value 2 maps to key '{key}'"); // 输出: "two"
}

// 清空字典
biDict.Clear();

// 使用初始容量优化性能
var optimizedBiDict = new BidirectionalDictionary<string, int>(100);
```

### 7. 并发限制队列 (ConcurrentLimitedQueue)

提供线程安全的定长队列实现：

```csharp
using GameFrameX.Foundation.Extensions;

// 创建定长队列
var queue = new ConcurrentLimitedQueue<string>(3); // 最大容量为3

// 添加元素
queue.Enqueue("first");
queue.Enqueue("second");
queue.Enqueue("third");
Console.WriteLine(queue.Count); // 3

// 添加第4个元素，会自动移除最旧的元素
queue.Enqueue("fourth");
Console.WriteLine(queue.Count); // 仍然是3

// 检查队列内容
if (queue.TryDequeue(out string item))
{
    Console.WriteLine(item); // "second" (first被自动移除了)
}

// 从现有集合创建
var initialData = new List<string> { "a", "b", "c" };
var queueFromList = new ConcurrentLimitedQueue<string>(initialData);

// 隐式转换
List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
ConcurrentLimitedQueue<int> numberQueue = numbers; // 隐式转换

// 动态调整限制
queue.Limit = 5; // 调整队列最大容量
```

## 高级用法示例

### 链式调用

扩展方法支持链式调用，让代码更加简洁：

```csharp
var result = "  Hello World  "
    .Trim()
    .ToLowerInvariant()
    .ToPascalCase()
    .Truncate(8); // "HelloWor"

var processedList = new[] { 1, 2, 2, 3, 4, 4, 5 }
    .Where(x => x > 2)
    .Distinct()
    .OrderByDescending(x => x)
    .ToList(); // [5, 4, 3]
```

### 性能优化建议

```csharp
// 1. 使用合适的初始容量
var biDict = new BidirectionalDictionary<string, int>(expectedSize);

// 2. 批量操作优于单个操作
var items = new List<string>();
items.AddRange(newItems); // 优于多次调用 Add

// 3. 使用 TryXxx 方法避免异常
if (biDict.TryGetValue(key, out var value))
{
    // 处理找到的值
}
else
{
    // 处理未找到的情况
}

// 4. 合理使用并发集合
var concurrentQueue = new ConcurrentLimitedQueue<Task>(maxConcurrency);
```

### 错误处理

```csharp
// 安全的类型转换
object unknownValue = GetValueFromSomewhere();
int safeInt = unknownValue.AsOrDefault<int>(); // 转换失败返回默认值

// 安全的字符串操作
string input = null;
string safe = input.IsNullOrEmpty() ? "default" : input.Trim();

// 安全的集合操作
List<string> items = null;
int count = items.IsNullOrEmpty() ? 0 : items.Count;
```

## 最佳实践

### 1. 命名空间使用

```csharp
// 推荐：明确引用命名空间
using GameFrameX.Foundation.Extensions;

// 避免：使用全局 using（除非整个项目都需要）
```

### 2. 性能考虑

```csharp
// 好的做法：预分配容量
var biDict = new BidirectionalDictionary<string, int>(1000);

// 好的做法：使用合适的数据结构
var limitedQueue = new ConcurrentLimitedQueue<LogEntry>(maxLogEntries);

// 避免：在循环中进行昂贵的操作
foreach (var item in items)
{
    // 避免在这里进行复杂的字符串操作或反射
}
```

### 3. 线程安全

```csharp
// ConcurrentLimitedQueue 是线程安全的
var queue = new ConcurrentLimitedQueue<WorkItem>(100);

// 可以在多线程环境中安全使用
Parallel.ForEach(workItems, item =>
{
    queue.Enqueue(item); // 线程安全
});

// BidirectionalDictionary 不是线程安全的，需要外部同步
var biDict = new BidirectionalDictionary<string, int>();
lock (biDict)
{
    biDict.TryAdd(key, value);
}
```

### 4. 内存管理

```csharp
// 及时清理大型集合
largeBidirectionalDictionary.Clear();

// 合理设置队列限制
var logQueue = new ConcurrentLimitedQueue<LogEntry>(1000); // 避免无限增长
```

## 扩展和自定义

如果需要添加自定义扩展方法，建议遵循以下模式：

```csharp
public static class CustomExtensions
{
    public static TResult SafeExecute<T, TResult>(this T obj, Func<T, TResult> func, TResult defaultValue = default)
    {
        try
        {
            return obj != null ? func(obj) : defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}

// 使用示例
string result = someObject.SafeExecute(x => x.ToString().ToUpper(), "DEFAULT");
```

## 许可证

本项目遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。详细信息请参阅源代码树根目录中的 LICENSE 文件。

## 贡献

欢迎提交 Issue 和 Pull Request 来帮助改进这个项目。

## 更新日志

### v1.0.0
- 初始版本发布
- 提供基础的字符串、集合、对象扩展方法
- 实现双向字典和并发限制队列
- 完整的单元测试覆盖

---

GameFrameX.Foundation.Extensions 致力于提供高质量、高性能的扩展方法库，让 .NET 开发更加高效和愉快。