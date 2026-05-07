<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX.Foundation

[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation?label=version&color=green)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援**

[📖 ドキュメント](https://gameframex.doc.alianblank.com) • [🚀 クイックスタート](#-クイックスタート)

---

🌐 **言語**: [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

---

</div>

### 📦 アセンブリ概要

| アセンブリ                                      | 機能説明          | NuGet 包名                                   | バージョン                                                                                                                                                                | ダウンロード数                                                                                                                                                               |
|------------------------------------------|---------------|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| GameFrameX.Foundation.Encryption         | 暗号化ライブラリ         | `GameFrameX.Foundation.Encryption`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 |
| GameFrameX.Foundation.Extensions         | 拡張メソッドライブラリ         | `GameFrameX.Foundation.Extensions`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 |
| GameFrameX.Foundation.Hash               | ハッシュライブラリ         | `GameFrameX.Foundation.Hash`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             |
| GameFrameX.Foundation.Http.Extension     | HttpClient 拡張 | `GameFrameX.Foundation.Http.Extension`     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         |
| GameFrameX.Foundation.Http.Normalization | HTTP メッセージ標準化    | `GameFrameX.Foundation.Http.Normalization` | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) |
| GameFrameX.Foundation.Json               | JSON シリアライザ    | `GameFrameX.Foundation.Json`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             |
| GameFrameX.Foundation.Localization       | ローカリゼーション         | `GameFrameX.Foundation.Localization`       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)             |
| GameFrameX.Foundation.Logger             | Serilog ロガー設定  | `GameFrameX.Foundation.Logger`             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         |
| GameFrameX.Foundation.Options            | CLI 引数パーサー       | `GameFrameX.Foundation.Options`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       |
| GameFrameX.Foundation.Orm.Attribute      | ORM 属性マーク      | `GameFrameX.Foundation.Orm.Attribute`      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           |
| GameFrameX.Foundation.Orm.Entity         | ORM エンティティ基底      | `GameFrameX.Foundation.Orm.Entity`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 |
| GameFrameX.Foundation.Utility            | ユーティリティクラス         | `GameFrameX.Foundation.Utility`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       |

GameFrameX の基盤ツールライブラリは、暗号化、ハッシュ、HTTP、JSON、ロギングなど、一般的な機能をカバーする高パフォーマンスで使いやすいコンポーネント群を提供します。

## 🚀 クイックスタート

### インストール

NuGet パッケージマネージャーで必要なコンポーネントをインストール：

```bash
# 暗号化ライブラリのインストール
dotnet add package GameFrameX.Foundation.Encryption

# 拡張メソッドライブラリのインストール
dotnet add package GameFrameX.Foundation.Extensions

# ハッシュライブラリのインストール
dotnet add package GameFrameX.Foundation.Hash

# JSONライブラリのインストール
dotnet add package GameFrameX.Foundation.Json

# ローカライゼーションフレームワークのインストール
dotnet add package GameFrameX.Foundation.Localization

# ロギングライブラリのインストール
dotnet add package GameFrameX.Foundation.Logger

# CLI引数パーサーのインストール
dotnet add package GameFrameX.Foundation.Options

# HTTP拡張のインストール
dotnet add package GameFrameX.Foundation.Http.Extension

# HTTPメッセージ標準化のインストール
dotnet add package GameFrameX.Foundation.Http.Normalization
```

### 基本的な使い方

```csharp
using GameFrameX.Foundation.Encryption;
using GameFrameX.Foundation.Extensions;
using GameFrameX.Foundation.Hash;
using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Logger;
using GameFrameX.Foundation.Options;

// AES暗号化
string encrypted = AesHelper.Encrypt("Hello World", "your-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-key");

// 拡張メソッド使用
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.RandomElement(); // ランダムに要素を取得
var isNullOrEmpty = myString.IsNullOrEmpty(); // 文字列チェック

// 文字列拡張
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // URLセーフBase64
string centered = "Hello".CenterAlignedText(20); // 中央揃え

// オブジェクト検証
object obj = GetSomeObject();
obj.ThrowIfNull(nameof(obj)); // nullチェック
int value = 50;
value.CheckRange(1, 100); // 範囲チェック

// 高性能バイト操作
Span<byte> buffer = stackalloc byte[8];
int offset = 0;
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);

// 双方向ディクショナリ
var biDict = new BidirectionalDictionary<string, int>();
biDict.TryAdd("one", 1);
if (biDict.TryGetKey(1, out string key)) { /* 逆引き検索 */ }

// コマンドライン引数処理
var builder = new OptionsBuilder<AppConfig>(args);
var config = builder.Build();

// SHA-256ハッシュ
string hash = Sha256Helper.ComputeHash("Hello World");

// JSONシリアライズ
string json = JsonHelper.Serialize(myObject);
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// ローカライズ文字列の取得
var successMessage = LocalizationService.GetString("Success");
var errorMessage = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
var formattedMessage = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);

// ログ記録
LogHandler.Create(LogOptions.Default);
LogHelper.Info("アプリケーション起動");
```

## 📚 詳細ドキュメント

### 🧩 拡張メソッドライブラリ (GameFrameX.Foundation.Extensions)

.NET 基本型の機能を強化する豊富な拡張メソッドコレクションで、開発効率とコードの可読性を向上させます。

#### コアコンポーネント概要

| コンポーネント | ファイル | 説明 |
|--------------|-------------------------------------------------------------------|--------------------------------|
| **コレクション拡張**     | `CollectionExtensions.cs`                                         | 各種コレクション型に便利な操作メソッドを提供 |
| **文字列拡張**    | `StringExtensions.cs`                                             | 文字列処理の強化、URLセーフBase64、中央揃え等を含む |
| **オブジェクト拡張**     | `ObjectExtensions.cs`                                             | オブジェクト検証と数値範囲チェックを提供 |
| **型拡張**     | `TypeExtensions.cs`                                               | 型チェックとリフレクション関連の拡張メソッド |
| **列挙型拡張**     | `IEnumerableExtensions.cs`                                        | LINQ拡張とコレクション操作、積集合・差集合等をサポート |
| **ディクショナリ拡張**     | `IDictionaryExtensions.cs`                                        | ディクショナリ操作の強化、マージ・条件削除等をサポート |
| **リスト拡張**     | `ListExtensions.cs`                                               | リスト固有の拡張メソッド |
| **バイト拡張**     | `ByteExtensions.cs`                                               | バイト配列操作、サブ配列抽出等を含む |
| **Span 拡張**   | `SpanExtensions.cs`                                               | 高性能メモリ操作、各種データ型の読み書き、ビッグエンディアン・リトルエンディアン対応 |
| **ReadOnlySpan 拡張** | `ReadOnlySpanExtensions.cs`                                       | 読み取り専用メモリの高性能読み取り操作 |
| **シーケンスリーダー拡張**  | `SequenceReaderExtensions.cs`                                     | シーケンスデータの便利な読み取りメソッド |
| **双方向ディクショナリ**     | `BidirectionalDictionary.cs`                                      | 双方向検索をサポートするディクショナリ実装 |
| **ルックアップテーブル**      | `LookupX.cs`                                                      | 拡張された一対多関係ルックアップテーブル |
| **並行キュー**     | `ConcurrentLimitedQueue.cs`                                       | スレッドセーフな有限容量キュー |
| **Null 許容ディクショナリ**     | `NullableDictionary.cs`<br/>`NullableConcurrentDictionary.cs`     | null値をサポートするディクショナリ実装 |
| **破棄可能ディクショナリ**    | `DisposableDictionary.cs`<br/>`DisposableConcurrentDictionary.cs` | 値が自動的に破棄されるディクショナリ |
| **定数定義**     | `ConstBaseTypeSize.cs`                                            | 基本データ型のバイトサイズ定数 |
| **Null オブジェクト**    | `NullObject.cs`                                                   | 型安全なNullオブジェクト実装 |
| **カスタム例外**    | `ArgumentAlreadyException.cs`                                     | パラメータ既存在例外タイプ |

#### コレクション拡張

```csharp
using GameFrameX.Foundation.Extensions;

// コレクション操作
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.RandomElement(); // ランダムに要素を取得
var isEmpty = list.IsNullOrEmpty(); // 空かどうかを確認

// ディクショナリ拡張
var dict = new Dictionary<string, int>();
dict.Merge("key", 10, (old, new) => old + new); // 値のマージ
var value = dict.GetOrAdd("key", k => 42); // 取得または追加
dict.RemoveIf((k, v) => v > 100); // 条件付き削除

// HashSet拡張
var hashSet = new HashSet<int>();
hashSet.AddRange(new[] { 1, 2, 3, 4, 5 }); // 一括追加
```

#### 文字列拡張

```csharp
// 文字列チェック
string text = "Hello World";
bool isEmpty = text.IsNullOrEmpty();
bool isEmptyOrWhitespace = text.IsNullOrEmptyOrWhiteSpace();
bool hasContent = text.IsNotNullOrEmptyOrWhiteSpace();

// 文字列処理
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // URLセーフ形式に変換
string restored = urlSafe.FromUrlSafeBase64(); // 標準形式に復元

// 文字列操作
string centered = "Hello".CenterAlignedText(20); // 中央揃え
string cleaned = "Hello World   ".RemoveWhiteSpace(); // 空白文字の削除
string trimmed = "Hello!".RemoveSuffix('!'); // サフィックスの削除

// 文字の繰り返し
string repeated = 'A'.RepeatChar(5); // "AAAAA"
```

#### オブジェクト検証と範囲チェック

```csharp
// nullチェック
object obj = GetSomeObject();
if (obj.IsNotNull())
{
    // オブジェクトがnullでない場合の処理
}

// パラメータ検証
obj.ThrowIfNull(nameof(obj)); // nullの場合に例外をスロー

// 数値範囲チェック
int value = 50;
value.CheckRange(1, 100); // 範囲チェック、超過時に例外をスロー
bool inRange = value.IsRange(1, 100); // 範囲内かどうかを確認

// 複数の数値型をサポート
uint uintValue = 25;
uintValue.CheckRange(0, 50);

long longValue = 1000;
longValue.CheckRange(500, 2000);
```

#### 型チェック拡張

```csharp
// ジェネリックインターフェースチェック
Type listType = typeof(List<string>);
Type genericListType = typeof(List<>);
bool implementsGeneric = listType.HasImplementedRawGeneric(genericListType);

// インターフェース実装チェック
Type stringType = typeof(string);
Type comparableType = typeof(IComparable);
bool implementsInterface = stringType.IsImplWithInterface(comparableType);
```

#### LINQ 拡張

```csharp
// 積集合の操作
var list1 = new[] { 1, 2, 3, 4, 5 };
var list2 = new[] { 3, 4, 5, 6, 7 };
var intersection = list1.IntersectBy(list2, x => x); // キーで積集合を取得

// 複数セットの積集合
var collections = new[] { list1, list2, new[] { 4, 5, 6 } };
var allIntersection = collections.IntersectAll(); // 全セットの積集合

// 差集合の操作
var difference = list1.ExceptBy(list2, (x, y) => x == y);

// 一括追加
var collection = new List<int>();
collection.AddRange(1, 2, 3, 4, 5); // paramsパラメータを使用
collection.AddRange(new[] { 6, 7, 8 }); // 配列を使用
```

#### 双方向ディクショナリ

```csharp
// 双方向ディクショナリを作成
var biDict = new BidirectionalDictionary<string, int>();

// キー・バリューペアを追加
biDict.TryAdd("one", 1);
biDict.TryAdd("two", 2);

// 双方向検索
if (biDict.TryGetValue("one", out int value))
{
    Console.WriteLine($"Key 'one' maps to {value}");
}

if (biDict.TryGetKey(1, out string key))
{
    Console.WriteLine($"Value 1 maps to '{key}'");
}

// ディクショナリをクリア
biDict.Clear();
```

#### 高性能拡張

```csharp
// Span と ReadOnlySpan 拡張
ReadOnlySpan<byte> span = stackalloc byte[] { 1, 2, 3, 4, 5 };
// Spanに対する高性能操作拡張を提供

// シーケンスリーダー拡張
// SequenceReaderに便利な読み取りメソッドを提供
```

#### バイト操作拡張

```csharp
// バイト配列拡張
byte[] data = { 1, 2, 3, 4, 5 };
byte[] subArray = data.SubArray(1, 3); // サブ配列を取得

// Span と ReadOnlySpan 拡張 - 高性能字节操作
Span<byte> buffer = stackalloc byte[16];
int offset = 0;

// 各種データ型の書き込み（ビッグエンディアン・リトルエンディアン対応）
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);
buffer.WriteUIntBigEndianValue(12345u, ref offset); // ビッグエンディアンで書き込み
buffer.WriteFloatBigEndianValue(3.14f, ref offset); // ビッグエンディアンで書き込み

// データ型の読み取り
offset = 0;
uint value = buffer.ReadUIntValue(ref offset);
float floatValue = buffer.ReadFloatValue(ref offset);
uint bigEndianValue = buffer.ReadUIntBigEndianValue(ref offset); // ビッグエンディアンで読み取り

// ReadOnlySpan 読み取り操作
ReadOnlySpan<byte> readBuffer = buffer;
offset = 0;
uint readValue = readBuffer.ReadUIntValue(ref offset);
float readFloatValue = readBuffer.ReadFloatBigEndianValue(ref offset);
```

#### シーケンスリーダー拡張

```csharp
// SequenceReaderに便利な読み取りメソッドを提供
// 長さプレフィックス付きバイト配列の読み取りをサポート
// 非破壊読み取りのためのTryPeekメソッドを提供
```

#### 特殊ユーティリティクラス

- **ConstBaseTypeSize**: 基本データ型のバイトサイズ定数定義、全.NET基本型のバイトサイズを含む
- **NullObject**: Nullオブジェクトパターンの実装、型安全なNullオブジェクトを提供
- **NullableConcurrentDictionary**: null値をサポートするスレッドセーフな並行ディクショナリ
- **NullableDictionary**: null値をサポートする通常のディクショナリ
- **LookupX**: 拡張されたルックアップテーブル実装、一対多関係マッピングをサポート
- **ArgumentAlreadyException**: パラメータ既存在例外、パラメータ検証シナリオで使用
- **ConcurrentLimitedQueue**: スレッドセーフな有限容量キュー、最古の要素を自動削除
- **DisposableConcurrentDictionary/DisposableDictionary**: 値が自動的に破棄されるディクショナリタイプ

### 🔐 暗号化ライブラリ (GameFrameX.Foundation.Encryption)

データの安全な送信と保存を確保する複数の暗号化アルゴリズムの実装を提供します。

#### サポートされるアルゴリズム

- **AES 暗号化** (`AesHelper`): 対称暗号化アルゴリズム。文字列とバイト配列をサポート
- **RSA 暗号化** (`RsaHelper`): 非対称暗号化アルゴリズム。キーペア生成、暗号化/復号、デジタル署名をサポート
- **DSA 署名** (`DsaHelper`): デジタル署名アルゴリズム。署名と検証をサポート
- **SM2/SM4 加密** (`Sm2Helper`/`Sm4Helper`): 国密アルゴリズム实现
    - SM2: 非対称暗号化アルゴリズム
    - SM4: 対称暗号化アルゴリズム。ECB/CBC モードをサポート
- **XOR 暗号化** (`XorHelper`): XOR 暗号化。高速暗号化と完全暗号化モードをサポート

#### 使用例

```csharp
// AES暗号化
string encrypted = AesHelper.Encrypt("機密データ", "your-secret-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-secret-key");

// RSA暗号化
var keys = RsaHelper.Make();
string encrypted = RsaHelper.Encrypt(keys["publicKey"], "Hello World");
string decrypted = RsaHelper.Decrypt(keys["privateKey"], encrypted);

// SM4暗号化
string encrypted = Sm4Helper.EncryptCbc("your-key", "Hello World");
string decrypted = Sm4Helper.DecryptCbc("your-key", encrypted);
```

### 🔗 ハッシュライブラリ (GameFrameX.Foundation.Hash)

データ整合性検証や高速検索などに適した複数のハッシュアルゴリズムの実装を提供します。

#### サポートされるアルゴリズム

- **MD5** (`Md5Helper`): 128ビットハッシュ値。ソルト対応
- **SHA シリーズ**:
    - SHA-1 (`Sha1Helper`): 160ビットハッシュ値
    - SHA-256 (`Sha256Helper`): 256ビットハッシュ値
    - SHA-512 (`Sha512Helper`): 512ビットハッシュ値
- **HMAC-SHA256** (`HmacSha256Helper`): キーベースのメッセージ認証コード
- **CRC チェックサム** (`CrcHelper`): CRC32/CRC64 巡回冗長検査
- **MurmurHash3** (`MurmurHash3Helper`): 高性能非暗号化ハッシュ
- **xxHash** (`XxHashHelper`): 超高パフォーマンスハッシュアルゴリズム。32/64/128ビット対応

#### 使用例

```csharp
// MD5ハッシュ
string md5Hash = Md5Helper.Hash("Hello World");
string saltedHash = Md5Helper.HashWithSalt("Hello World", "salt");

// SHA-256ハッシュ
string sha256Hash = Sha256Helper.ComputeHash("Hello World");

// HMAC-SHA256
string hmacHash = HmacSha256Helper.Hash("message", "secret-key");

// xxHash（高性能）
ulong xxHash = XxHashHelper.Hash64("Hello World");
```

### 🌐 HTTP ツール

#### HTTP 拡張 (GameFrameX.Foundation.Http.Extension)

HttpClient の便利な拡張メソッドで、JSON データの送受信を簡素化します。

```csharp
// POST JSONリクエスト
string response = await httpClient.PostJsonToStringAsync<MyClass>(url, myObject);
```

#### HTTP メッセージ標準化 (GameFrameX.Foundation.Http.Normalization)

`code`、`message`、`data` フィールドを含む統一 HTTP レスポンス形式を提供し、GameFrameX エコシステムに適用します。

### 📄 JSON シリアライズ (GameFrameX.Foundation.Json)

`System.Text.Json` ベースの高性能シリアライズツールで、最適化されたデフォルト設定を提供します。

#### 特徴

- 高性能シリアライズ/デシリアライズ
- 列挙型を文字列としてシリアライズ
- null 値プロパティを無視
- 循環参照を無視
- プロパティ名の大文字小文字を区別しない
- フォーマット済みとコンパクトの2つの出力モード

#### 使用例

```csharp
// シリアライズ
string json = JsonHelper.Serialize(myObject);
string formattedJson = JsonHelper.Serialize(myObject, JsonHelper.FormatOptions);

// デシリアライズ
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 安全なデシリアライズ
if (JsonHelper.TryDeserialize<MyClass>(json, out var result))
{
    // 処理結果
}
```

### 🌐 ローカリゼーションフレームワーク (GameFrameX.Foundation.Localization)

軽量で高性能なローカリゼーションソリューション。ゼロ設定使用とレイジーロードをサポートし、GameFrameX.Foundation エコシステム全体の統一ローカリゼーションを提供します。

#### 主な特徴

- **ゼロ設定使用**: 初期化設定なしで、ローカライズリソースを自動検出・読み込み
- **レイジーロード機構**: 初回使用時にのみリソースを読み込み、起動パフォーマンスに優れる
- **多言語サポート**: 中国語（簡体）と英語を内蔵し、更多言語に拡張可能
- **スレッドセーフ**: 同時アクセスをサポートし、マルチスレッド環境に適用
- **高い拡張性**: カスタムリソースプロバイダーをサポート、柔軟な優先度管理
- **優先度解決**: カスタムプロバイダー > アセンブリリソース > デフォルトリソース

#### コアコンポーネント

| コンポーネント | ファイル | 説明 |
|------------|-------------------------------|---------------------|
| **ローカリゼーションサービス**  | `LocalizationService.cs`      | 统一的本地化入口点，提供静态方法API |
| **リソースマネージャー**  | `ResourceManager.cs`          | 管理多个资源提供者，实现优先级解析   |
| **デフォルトプロバイダー**  | `DefaultResourceProvider.cs`  | 提供英文默认消息，包含50+常用消息  |
| **アセンブリプロバイダー** | `AssemblyResourceProvider.cs` | 从.resx文件加载本地化资源     |

#### 基本的な使い方

```csharp
using GameFrameX.Foundation.Localization.Core;

// シンプルなローカライズ文字列を取得
var successMessage = LocalizationService.GetString("Success");
Console.WriteLine(successMessage); // 現在のカルチャに基づいて表示 "Success" 或 "成功"

// パラメータ付きのフォーマットメッセージ
var errorMessage = LocalizationService.GetString("ArgumentNull", "username");
Console.WriteLine(errorMessage); // "Value cannot be null. (Parameter 'username')"

// キーが存在しない場合、キー名自体を返す
var unknown = LocalizationService.GetString("Some.Unknown.Key");
Console.WriteLine(unknown); // 出力: "Some.Unknown.Key"
```

#### 例外処理でのローカリゼーション

```csharp
using GameFrameX.Foundation.Utility.Localization;

public class UserService
{
    public void ValidateUserInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException(
                LocalizationService.GetString(LocalizationKeys.Exceptions.TimestampOutOfRange),
                nameof(input));
        }

        // その他の検証ロジック...
    }
}
```

#### モジュールローカリゼーション統合

##### 1. ローカリゼーションキーの定義

```csharp
// YourModule/Localization/Keys.cs
namespace GameFrameX.Foundation.YourModule.Localization;

public static class LocalizationKeys
{
    public static class Validation
    {
        public const string EmailRequired = "YourModule.Validation.EmailRequired";
        public const string EmailInvalid = "YourModule.Validation.EmailInvalid";
    }

    public static class Messages
    {
        public const string UserCreated = "YourModule.Messages.UserCreated";
        public const string OperationFailed = "YourModule.Messages.OperationFailed";
    }
}
```

##### 2. リソースファイルの作成

プロジェクトに `Localization/Messages/Resources.resx` と `Localization/Messages/Resources.zh-CN.resx` を作成：

```xml
<!-- Resources.resx (デフォルト英語) -->
<root>
  <data name="YourModule.Validation.EmailRequired" xml:space="preserve">
    <value>Email address is required</value>
  </data>
  <data name="YourModule.Messages.UserCreated" xml:space="preserve">
    <value>User '{0}' has been created successfully</value>
  </data>
</root>
```

```xml
<!-- Resources.zh-CN.resx (中国語) -->
<root>
  <data name="YourModule.Validation.EmailRequired" xml:space="preserve">
    <value>メールアドレスは必須項目です</value>
  </data>
  <data name="YourModule.Messages.UserCreated" xml:space="preserve">
    <value>ユーザー '{0}' が正常に作成されました</value>
  </data>
</root>
```

##### 3. ビジネスロジックでの使用

```csharp
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.YourModule.Localization;

public class UserService
{
    public void CreateUser(UserDto userDto)
    {
        if (string.IsNullOrEmpty(userDto.Email))
        {
            throw new ValidationException(
                LocalizationService.GetString(LocalizationKeys.Validation.EmailRequired));
        }

        // ユーザー作成ロジック...

        var successMessage = LocalizationService.GetString(
            LocalizationKeys.Messages.UserCreated, userDto.Username);
        Console.WriteLine(successMessage);
    }
}
```

#### カスタムリソースプロバイダー

```csharp
public class DatabaseResourceProvider : IResourceProvider
{
    private readonly IDbConnection _connection;

    public DatabaseResourceProvider(IDbConnection connection)
    {
        _connection = connection;
    }

    public string GetString(string key)
    {
        var culture = CultureInfo.CurrentCulture.Name;
        var sql = "SELECT localized_text FROM localization_strings WHERE key = @key AND culture = @culture";
        return _connection.ExecuteScalar<string>(sql, new { key, culture });
    }
}

// カスタムプロバイダーを登録（最高優先度）
var dbProvider = new DatabaseResourceProvider(yourDbConnection);
LocalizationService.RegisterProvider(dbProvider);
```

#### プリロードとパフォーマンス最適化

```csharp
// アプリケーション起動時に全ローカライズリソースをプリロード（オプション）
LocalizationService.EnsureLoaded();

// ローカライズシステムの統計情報を取得
var stats = LocalizationService.GetStatistics();
Console.WriteLine($"プロバイダー読み込み済み: {stats.ProvidersLoaded}");
Console.WriteLine($"総プロバイダー数: {stats.TotalProviderCount}");
Console.WriteLine($"アセンブリプロバイダー数: {stats.AssemblyProviderCount}");

// 全プロバイダー情報を取得
var providers = LocalizationService.GetProviders();
foreach (var provider in providers)
{
    Console.WriteLine($"プロバイダー: {provider.GetType().Name}");
}
```

#### リソース命名規則

- **模式**: `{モジュール名}.{类别}.{具体键名}`
- **例**:
    - `Utility.Exceptions.TimestampOutOfRange`
    - `Encryption.InvalidKeySize`
    - `Authentication.UserNotFound`
    - `Success`
    - `ArgumentNull`

#### 統合済みモジュール

目前以下モジュール已完成本地化集成：

| モジュール                               | ローカリゼーションキー数 | 状態   |
|----------------------------------|--------|------|
| GameFrameX.Foundation.Utility    | 4      | ✅ 完了 |
| GameFrameX.Foundation.Encryption | 20+    | ✅ 完了 |
| GameFrameX.Foundation.Extensions | 7      | ✅ 完了 |
| GameFrameX.Foundation.Hash       | 2      | ✅ 完了 |

#### 高度な機能

##### 動的言語切り替え

```csharp
public void SwitchLanguage(string cultureCode)
{
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

    // オプション：新しい言語のリソースをプリロード
    LocalizationService.EnsureLoaded();
}
```

##### モニタリングと診断

```csharp
public class LocalizationDiagnostics
{
    public void PrintStatus()
    {
        var stats = LocalizationService.GetStatistics();
        Console.WriteLine("=== ローカライズシステム状態 ===");
        Console.WriteLine($"プロバイダー読み込み済み: {stats.ProvidersLoaded}");
        Console.WriteLine($"総プロバイダー数: {stats.TotalProviderCount}");

        var providers = LocalizationService.GetProviders();
        foreach (var provider in providers)
        {
            Console.WriteLine($"- {provider.GetType().Name}");
        }
    }
}
```

#### ベストプラクティス

1. **键命名规范**: 使用 `{モジュール名}.{类别}.{具体键名}` 的命名模式
2. **パラメータ付きメッセージ**: `string.Format`形式でパラメータ置換をサポート
3. **例外処理**: 例外メッセージにローカライズサポートを統合
4. **パフォーマンス最適化**: アプリケーション起動時にリソースのプリロードを選択可能
5. **テスト検証**: ローカライズ機能の単体テストを作成

#### プロジェクトファイルの設定

プロジェクトファイルにローカリゼーションリソースが含まれていることを確認：

```xml
<PropertyGroup>
  <EnableDefaultEmbeddedResourceItems>false</EnableDefaultEmbeddedResourceItems>
</PropertyGroup>

<ItemGroup>
  <EmbeddedResource Include="Localization\Messages\*.resx" />
</ItemGroup>
```

詳細は以下を参照：

- [ローカリゼーション完整文档](GameFrameX.Foundation.Localization/README.Localization.md)
- [使用例とベストプラクティス](GameFrameX.Foundation.Localization/USAGE_EXAMPLES.md)

### �️ ORM エンティティ基底 (GameFrameX.Foundation.Orm.Entity)

ORM エンティティ基底クラスとインターフェース定義。監査トレイル、論理削除、楽観的ロックなどのエンタープライズ機能をサポートします。

#### コアコンポーネント概要

| コンポーネント | ファイル名 | 主な機能 |
|--------------|-----------------------|-------------------------------|
| **エンティティ基底クラス**     | `EntityBase.cs`       | 完整功能的实体基类，包含ID、审计、软删除、版本控制等功能 |
| **エンティティ基底クラス(ジェネリック)** | `EntityBaseId.cs`     | 支持自定义主键类型的实体基类                |
| **エンティティインターフェース**     | `IEntity.cs`          | 基础实体接口定义，提供ID属性               |
| **監査インターフェース**     | `IAuditableEntity.cs` | 审计功能接口，定义创建时间、更新时间、操作ユーザー等审计字段  |

#### エンティティ基底クラスの機能

```csharp
using GameFrameX.Foundation.Orm.Entity;

// EntityBaseを継承したエンティティクラスは自動的に完全なエンタープライズ機能を取得
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // 以下のプロパティはEntityBaseから提供：
    // - long Id                    // 主キーID
    // - DateTime CreateTime        // 作成日時
    // - DateTime UpdateTime        // 更新日時
    // - long CreateUserId          // 作成ユーザーID
    // - long UpdateUserId          // 更新ユーザーID
    // - string CreateUserName      // 作成ユーザー名
    // - string UpdateUserName      // 更新ユーザー名
    // - bool IsDelete              // 論理削除フラグ
    // - long Version               // 楽観的ロックバージョン
    // - bool IsEnabled             // 有効状態
}

// 使用例
var user = new User
{
    Username = "john_doe",
    Email = "john@example.com",
    PasswordHash = "hashed_password",
    CreateTime = DateTime.UtcNow,
    CreateUserId = 1,
    CreateUserName = "admin",
    IsEnabled = true
};
```

#### カスタム主キー型

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 文字列を主キーとして使用
public class Product : EntityBaseId<string>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    // Idプロパティの型はstring、EntityBaseId<string>から提供
}

// Guidを主キーとして使用
public class Order : EntityBaseId<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Idプロパティの型はGuid、EntityBaseId<Guid>から提供
}

// 使用例
var product = new Product
{
    Id = "PROD-001",
    Name = "ノートパソコン",
    Price = 5999.99m,
    Description = "高性能ノートパソコン"
};

var order = new Order
{
    Id = Guid.NewGuid(),
    OrderNumber = "ORD-20240101-001",
    TotalAmount = 5999.99m,
    OrderDate = DateTime.UtcNow
};
```

#### インターフェース実装

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 基本エンティティインターフェースを実装
public class Category : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// 監査インターフェースを実装
public class AuditableCategory : IEntity<int>, IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // IAuditableEntityインターフェースが要求するプロパティ
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public long CreateUserId { get; set; }
    public long UpdateUserId { get; set; }
    public string CreateUserName { get; set; }
    public string UpdateUserName { get; set; }
}
```

#### エンタープライズ機能詳細

##### 1. 監査トレイル (Audit Trail)

```csharp
// EntityBaseは自動的に監査フィールドを提供
public class Document : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
}

// ビジネスロジックで監査情報を設定
var document = new Document
{
    Title = "重要なドキュメント",
    Content = "ドキュメント内容...",
    CreateTime = DateTime.UtcNow,
    CreateUserId = currentUser.Id,
    CreateUserName = currentUser.Username,
    UpdateTime = DateTime.UtcNow,
    UpdateUserId = currentUser.Id,
    UpdateUserName = currentUser.Username
};

// 更新時に監査情報を自動保守
document.Content = "更新後の内容";
document.UpdateTime = DateTime.UtcNow;
document.UpdateUserId = currentUser.Id;
document.UpdateUserName = currentUser.Username;
document.Version++; // 楽観的ロックバージョンのインクリメント
```

##### 2. 論理削除 (Soft Delete)

```csharp
// 論理削除：レコードを実際に削除せず、削除済みとしてマーク
public void SoftDeleteUser(User user)
{
    user.IsDelete = true;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    // データベースに保存、レコードは存在するが削除済みとしてマーク
    dbContext.SaveChanges();
}

// クエリ時に削除済みレコードをフィルター
var activeUsers = dbContext.Users
    .Where(u => !u.IsDelete)
    .ToList();

// 削除済みレコードを復元
public void RestoreUser(User user)
{
    user.IsDelete = false;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    dbContext.SaveChanges();
}
```

##### 3. 楽観的ロック (Optimistic Locking)

```csharp
// Versionフィールドで楽観的ロックを実装
public void UpdateUserWithOptimisticLock(long userId, string newEmail)
{
    var user = dbContext.Users.Find(userId);
    if (user == null) throw new EntityNotFoundException();
    
    var originalVersion = user.Version;
    
    // データを変更
    user.Email = newEmail;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    user.Version++; // バージョン番号のインクリメント
    
    try
    {
        // 保存時にバージョン番号をチェック
        var rowsAffected = dbContext.Database.ExecuteSqlRaw(
            "UPDATE Users SET Email = {0}, UpdateTime = {1}, UpdateUserId = {2}, UpdateUserName = {3}, Version = {4} " +
            "WHERE Id = {5} AND Version = {6}",
            user.Email, user.UpdateTime, user.UpdateUserId, user.UpdateUserName, user.Version, user.Id, originalVersion);
            
        if (rowsAffected == 0)
        {
            throw new ConcurrencyException("データが他のユーザーによって変更されました。更新後にリトライしてください");
        }
    }
    catch (DbUpdateConcurrencyException)
    {
        throw new ConcurrencyException("同時実行の競合、更新後にリトライしてください");
    }
}
```

##### 4. 有効/無効状態管理

```csharp
// IsEnabledフィールドでエンティティの有効状態を管理
public class Feature : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    // IsEnabledはEntityBaseから提供
}

// 有効化/無効化機能
public void ToggleFeature(long featureId, bool enabled)
{
    var feature = dbContext.Features.Find(featureId);
    if (feature == null) throw new EntityNotFoundException();
    
    feature.IsEnabled = enabled;
    feature.UpdateTime = DateTime.UtcNow;
    feature.UpdateUserId = currentUser.Id;
    feature.UpdateUserName = currentUser.Username;
    feature.Version++;
    
    dbContext.SaveChanges();
}

// 有効な機能をクエリ
var enabledFeatures = dbContext.Features
    .Where(f => f.IsEnabled && !f.IsDelete)
    .ToList();
```

#### 完全な使用例

```csharp
using GameFrameX.Foundation.Orm.Entity;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Entities
{
    // ユーザーエンティティ
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        
        // ナビゲーションプロパティ
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    
    // 注文エンティティ（Guid主キーを使用）
    public class Order : EntityBaseId<Guid>
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        // ナビゲーションプロパティ
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 注文項目エンティティ
    public class OrderItem : EntityBase
    {
        public Guid OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        // ナビゲーションプロパティ
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
    
    // 製品エンティティ（文字列主キーを使用）
    public class Product : EntityBaseId<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; }
        
        // ナビゲーションプロパティ
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 分類エンティティ（インターフェース実装）
    public class Category : IEntity<string>, IAuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        
        // IAuditableEntityインターフェースのプロパティ
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        
        // ナビゲーションプロパティ
        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> Children { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}

// ビジネスサービスの例
namespace MyApplication.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        
        public UserService(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<User> CreateUserAsync(string username, string email, string password)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                CreateTime = DateTime.UtcNow,
                UpdateTime = DateTime.UtcNow,
                CreateUserId = currentUser.Id,
                UpdateUserId = currentUser.Id,
                CreateUserName = currentUser.Username,
                UpdateUserName = currentUser.Username,
                IsEnabled = true,
                IsDelete = false,
                Version = 1
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }
        
        public async Task<User> UpdateUserAsync(long userId, string email, string firstName, string lastName)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && !u.IsDelete)
                .FirstOrDefaultAsync();
                
            if (user == null)
                throw new EntityNotFoundException($"ユーザー {userId} 不存在");
            
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var originalVersion = user.Version;
            
            // フィールドを更新
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = currentUser.Id;
            user.UpdateUserName = currentUser.Username;
            user.Version++;
            
            try
            {
                await _context.SaveChangesAsync();
                return user;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException("ユーザー情報が他のユーザーによって変更されました。更新後にリトライしてください");
            }
        }
        
        public async Task SoftDeleteUserAsync(long userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && !u.IsDelete)
                .FirstOrDefaultAsync();
                
            if (user == null)
                throw new EntityNotFoundException($"ユーザー {userId} 不存在");
            
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            user.IsDelete = true;
            user.UpdateTime = DateTime.UtcNow;
            user.UpdateUserId = currentUser.Id;
            user.UpdateUserName = currentUser.Username;
            user.Version++;
            
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Where(u => u.IsEnabled && !u.IsDelete)
                .OrderBy(u => u.CreateTime)
                .ToListAsync();
        }
        
        private string HashPassword(string password)
        {
            // パスワードハッシュロジックを実装
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

### 🏷️ ORM 属性マーク (GameFrameX.Foundation.Orm.Attribute)

ORMフレームワークの属性マークを提供し、エンティティクラスの監査トレイル、キャッシュ戦略、論理削除、バージョン管理などの特殊機能を識別します。

#### コアコンポーネント概要

| コンポーネント | ファイル名 | 主な機能 |
|--------------|------------------------|-----------------------------------------|
| **監査テーブル属性**    | `AuditTableAttribute.cs` | 标记实体类支持审计跟踪功能，レコード数据变更历史                  |
| **キャッシュテーブル属性**    | `CacheTableAttribute.cs` | 标记实体类支持缓存策略，提升数据访问性能                    |
| **論理削除属性**    | `SoftDeleteAttribute.cs` | エンティティクラスの論理削除機能をマーク、物理削除ではなく論理削除                 |
| **バージョン管理属性**   | `VersionControlAttribute.cs` | 标记实体类支持数据版本管理，实现乐观锁和并发控制               |

#### 監査テーブル属性 (AuditTableAttribute)

監査トレイルが必要なエンティティクラスをマークし、データの作成・変更・削除などの操作履歴を自動的に記録します。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// ユーザーテーブルの監査トレイルをマーク
[AuditTable]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBaseに監査フィールドが含まれる：
    // CreateTime, UpdateTime, CreateUserId, UpdateUserId, 
    // CreateUserName, UpdateUserName
}

// 注文テーブルの監査トレイルをマーク
[AuditTable]
public class Order : EntityBase
{
    public string OrderNumber { get; set; }
    public long UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}

// 監査インターセプターの例
public class AuditInterceptor : IDbCommandInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    
    public AuditInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity.GetType().GetCustomAttribute<AuditTableAttribute>() != null)
            .ToList();
            
        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditableEntity auditableEntity)
            {
                var currentUser = _currentUserService.GetCurrentUser();
                var now = DateTime.UtcNow;
                
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreateTime = now;
                        auditableEntity.UpdateTime = now;
                        auditableEntity.CreateUserId = currentUser.Id;
                        auditableEntity.UpdateUserId = currentUser.Id;
                        auditableEntity.CreateUserName = currentUser.Username;
                        auditableEntity.UpdateUserName = currentUser.Username;
                        break;
                        
                    case EntityState.Modified:
                        auditableEntity.UpdateTime = now;
                        auditableEntity.UpdateUserId = currentUser.Id;
                        auditableEntity.UpdateUserName = currentUser.Username;
                        break;
                }
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}
```

#### キャッシュテーブル属性 (CacheTableAttribute)

キャッシュ戦略をサポートするエンティティクラスをマークし、これらのテーブルのデータを自動的にキャッシュ管理します。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 設定テーブルのキャッシュをマーク（変更頻度が低く、キャッシュに適する）
[CacheTable]
public class SystemConfig : EntityBase
{
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

// 辞書テーブルのキャッシュをマーク（辞書データは比較的安定、キャッシュに適する）
[CacheTable]
public class Dictionary : EntityBase
{
    public string DictType { get; set; }
    public string DictKey { get; set; }
    public string DictValue { get; set; }
    public string Description { get; set; }
    public int SortOrder { get; set; }
}

// 権限テーブルのキャッシュをマーク（アクセス頻度が高いが変更は少ない）
[CacheTable]
public class Permission : EntityBase
{
    public string PermissionCode { get; set; }
    public string PermissionName { get; set; }
    public string Description { get; set; }
    public string Module { get; set; }
}

// キャッシュサービスの例
public class CacheService<T> where T : class
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDbContext _dbContext;
    private readonly ILogger<CacheService<T>> _logger;
    
    public CacheService(IMemoryCache memoryCache, IDbContext dbContext, ILogger<CacheService<T>> logger)
    {
        _memoryCache = memoryCache;
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<T>> GetAllAsync()
    {
        var entityType = typeof(T);
        var cacheAttribute = entityType.GetCustomAttribute<CacheTableAttribute>();
        
        if (cacheAttribute == null)
        {
            // キャッシュ未対応、データベースから直接クエリ
            return await _dbContext.Set<T>().ToListAsync();
        }
        
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<T> cachedData))
        {
            _logger.LogDebug($"キャッシュからデータを取得: {cacheKey}");
            return cachedData;
        }
        
        // データベースからクエリしてキャッシュ
        var data = await _dbContext.Set<T>().ToListAsync();
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), // 30分後に期限切れ
            SlidingExpiration = TimeSpan.FromMinutes(5), // 5分スライド有効期限
            Priority = CacheItemPriority.Normal
        };
        
        _memoryCache.Set(cacheKey, data, cacheOptions);
        _logger.LogDebug($"データをキャッシュ済み: {cacheKey}, レコード数: {data.Count}");
        
        return data;
    }
    
    public async Task InvalidateCacheAsync()
    {
        var entityType = typeof(T);
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        _memoryCache.Remove(cacheKey);
        _logger.LogDebug($"キャッシュが無効化: {cacheKey}");
    }
}

// キャッシュマネージャーの例
public class CacheManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CacheManager> _logger;
    
    public CacheManager(IServiceProvider serviceProvider, ILogger<CacheManager> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task RefreshAllCacheTablesAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var cacheTableTypes = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<CacheTableAttribute>() != null)
            .ToList();
            
        foreach (var type in cacheTableTypes)
        {
            try
            {
                var serviceType = typeof(CacheService<>).MakeGenericType(type);
                var service = _serviceProvider.GetService(serviceType);
                
                if (service != null)
                {
                    var invalidateMethod = serviceType.GetMethod("InvalidateCacheAsync");
                    await (Task)invalidateMethod.Invoke(service, null);
                    
                    var getAllMethod = serviceType.GetMethod("GetAllAsync");
                    await (Task)getAllMethod.Invoke(service, null);
                    
                    _logger.LogInformation($"缓存表 {type.Name} リフレッシュ済み");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"キャッシュテーブルをリフレッシュ {type.Name} でエラーが発生");
            }
        }
    }
}
```

#### 論理削除属性 (SoftDeleteAttribute)

論理削除機能をサポートするエンティティクラスをマークし、削除操作はレコードを物理削除ではなく削除済みとしてマークします。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// ユーザーテーブルの論理削除をマーク
[SoftDelete]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBaseにIsDeleteフィールドが含まれる
}

// 記事テーブルの論理削除をマーク
[SoftDelete]
public class Article : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
    public long AuthorId { get; set; }
    public DateTime PublishTime { get; set; }
}

// 論理削除インターセプター
public class SoftDeleteInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 論理削除エンティティの削除操作を処理
        var softDeleteEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && 
                       e.Entity.GetType().GetCustomAttribute<SoftDeleteAttribute>() != null)
            .ToList();
            
        foreach (var entry in softDeleteEntries)
        {
            // 削除操作を更新操作に変換
            entry.State = EntityState.Modified;
            
            if (entry.Entity is EntityBase entityBase)
            {
                entityBase.IsDelete = true;
                entityBase.UpdateTime = DateTime.UtcNow;
                // 更新ユーザー情報を設定...
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 論理削除クエリフィルター
public static class SoftDeleteQueryExtensions
{
    public static IQueryable<T> WhereNotDeleted<T>(this IQueryable<T> query) 
        where T : class
    {
        var entityType = typeof(T);
        var softDeleteAttribute = entityType.GetCustomAttribute<SoftDeleteAttribute>();
        
        if (softDeleteAttribute != null && typeof(EntityBase).IsAssignableFrom(entityType))
        {
            return query.Where(e => !((EntityBase)(object)e).IsDelete);
        }
        
        return query;
    }
    
    public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query) 
        where T : class
    {
        // 削除済みレコードを含むクエリを返す
        return query;
    }
    
    public static IQueryable<T> OnlyDeleted<T>(this IQueryable<T> query) 
        where T : class
    {
        var entityType = typeof(T);
        var softDeleteAttribute = entityType.GetCustomAttribute<SoftDeleteAttribute>();
        
        if (softDeleteAttribute != null && typeof(EntityBase).IsAssignableFrom(entityType))
        {
            return query.Where(e => ((EntityBase)(object)e).IsDelete);
        }
        
        return query.Where(_ => false); // 論理削除未対応の場合、空の結果を返す
    }
}

// 使用例
public class UserService
{
    private readonly ApplicationDbContext _context;
    
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // アクティブユーザーを取得（削除済みを自動フィルター）
    public async Task<List<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .WhereNotDeleted()
            .ToListAsync();
    }
    
    // 削除済みユーザーを取得
    public async Task<List<User>> GetDeletedUsersAsync()
    {
        return await _context.Users
            .OnlyDeleted()
            .ToListAsync();
    }
    
    // 全ユーザーを取得（削除済みを含む）
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .IncludeDeleted()
            .ToListAsync();
    }
    
    // ユーザーを論理削除
    public async Task SoftDeleteUserAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user); // インターセプターによって論理削除に変換される
            await _context.SaveChangesAsync();
        }
    }
    
    // 削除済みユーザーを復元
    public async Task RestoreUserAsync(long userId)
    {
        var user = await _context.Users
            .IncludeDeleted()
            .FirstOrDefaultAsync(u => u.Id == userId);
            
        if (user != null && user.IsDelete)
        {
            user.IsDelete = false;
            user.UpdateTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
```

#### バージョン管理属性 (VersionControlAttribute)

データバージョン管理をサポートするエンティティクラスをマークし、楽観的ロックと同時実行制御を実現します。

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// ユーザーテーブルのバージョン管理をマーク
[VersionControl]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBaseにVersionフィールドが含まれる
}

// 在庫テーブルのバージョン管理をマーク（売り越し防止）
[VersionControl]
public class Inventory : EntityBase
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public decimal UnitCost { get; set; }
}

// 口座残高テーブルのバージョン管理をマーク（同時操作による残高エラーを防止）
[VersionControl]
public class AccountBalance : EntityBase
{
    public long UserId { get; set; }
    public decimal Balance { get; set; }
    public decimal FrozenAmount { get; set; }
    public string Currency { get; set; }
}

// バージョン管理インターセプター
public class VersionControlInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // バージョン管理エンティティの更新操作を処理
        var versionControlEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified && 
                       e.Entity.GetType().GetCustomAttribute<VersionControlAttribute>() != null)
            .ToList();
            
        foreach (var entry in versionControlEntries)
        {
            if (entry.Entity is EntityBase entityBase)
            {
                // バージョン番号の自動インクリメント
                entityBase.Version++;
                
                // Versionフィールドを変更済みとしてマーク
                entry.Property(nameof(EntityBase.Version)).IsModified = true;
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// バージョン管理サービス
public class VersionControlService<T> where T : EntityBase
{
    private readonly IDbContext _dbContext;
    private readonly ILogger<VersionControlService<T>> _logger;
    
    public VersionControlService(IDbContext dbContext, ILogger<VersionControlService<T>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<T> UpdateWithVersionCheckAsync(long id, Action<T> updateAction, int maxRetries = 3)
    {
        var entityType = typeof(T);
        var versionControlAttribute = entityType.GetCustomAttribute<VersionControlAttribute>();
        
        if (versionControlAttribute == null)
        {
            throw new InvalidOperationException($"实体类型 {entityType.Name} にVersionControlAttributeが未マーク");
        }
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new EntityNotFoundException($"エンティティ {entityType.Name} (ID: {id}) が存在しません");
                }
                
                var originalVersion = entity.Version;
                
                // 更新操作を実行
                updateAction(entity);
                
                // 更新時刻を設定
                entity.UpdateTime = DateTime.UtcNow;
                
                // 変更を保存
                await _dbContext.SaveChangesAsync();
                
                _logger.LogDebug($"实体 {entityType.Name} (ID: {id}) 更新成功，版本从 {originalVersion} から更新 {entity.Version}");
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning($"实体 {entityType.Name} (ID: {id}) バージョンの競合，第 {attempt} 回リトライ");
                
                if (attempt == maxRetries)
                {
                    throw new ConcurrencyException($"实体 {entityType.Name} (ID: {id}) 在 {maxRetries} 回リトライ後もバージョンの競合が存在", ex);
                }
                
                // 最新バージョンを取得するためにエンティティを再読み込み
                _dbContext.Entry(await _dbContext.Set<T>().FindAsync(id)).Reload();
                
                // 一定時間待機後にリトライ
                await Task.Delay(TimeSpan.FromMilliseconds(100 * attempt));
            }
        }
        
        throw new InvalidOperationException("ここには到達しないはず");
    }
}

// 使用例
public class InventoryService
{
    private readonly VersionControlService<Inventory> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public InventoryService(VersionControlService<Inventory> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 在庫を減少（売り越し防止）
    public async Task<bool> ReduceInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"产品 {productId} の在庫レコードが存在しません");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
            {
                if (inv.Quantity < quantity)
                {
                    throw new InsufficientInventoryException($"在庫不足、現在の在庫: {inv.Quantity}，必要: {quantity}");
                }
                
                inv.Quantity -= quantity;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            // バージョンの競合、同時操作が原因の可能性
            throw new ConcurrencyException("在庫の更新に失敗、リトライしてください");
        }
    }
    
    // 在庫を増加
    public async Task AddInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"产品 {productId} の在庫レコードが存在しません");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
        {
            inv.Quantity += quantity;
        });
    }
}

// 口座残高サービスの例
public class AccountBalanceService
{
    private readonly VersionControlService<AccountBalance> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public AccountBalanceService(VersionControlService<AccountBalance> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 残高を減額
    public async Task<bool> DeductBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"ユーザー {userId} 的 {currency} アカウントが存在しません");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
            {
                if (bal.Balance < amount)
                {
                    throw new InsufficientBalanceException($"残高不足、現在の残高: {bal.Balance}，必要: {amount}");
                }
                
                bal.Balance -= amount;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            throw new ConcurrencyException("残高の更新に失敗、リトライしてください");
        }
    }
    
    // 残高を増加
    public async Task AddBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"ユーザー {userId} 的 {currency} アカウントが存在しません");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
        {
            bal.Balance += amount;
        });
    }
}
```

#### 完全な統合例

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Entities
{
    // ユーザーエンティティ：監査、論理削除、バージョン管理をサポート
    [AuditTable]
    [SoftDelete]
    [VersionControl]
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginTime { get; set; }
    }
    
    // システム設定：キャッシュ、監査をサポート
    [CacheTable]
    [AuditTable]
    public class SystemConfig : EntityBase
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
    
    // 在庫レコード：バージョン管理、監査をサポート
    [VersionControl]
    [AuditTable]
    public class Inventory : EntityBase
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public string WarehouseCode { get; set; }
    }
    
    // 注文レコード：監査、論理削除をサポート
    [AuditTable]
    [SoftDelete]
    public class Order : EntityBase
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

// DbContext設定
public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<SystemConfig> SystemConfigs { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(
                new AuditInterceptor(),
                new SoftDeleteInterceptor(),
                new VersionControlInterceptor()
            );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // SoftDeleteAttributeがマークされた全エンティティにグローバルクエリフィルターを追加
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (clrType.GetCustomAttribute<SoftDeleteAttribute>() != null &&
                typeof(EntityBase).IsAssignableFrom(clrType))
            {
                var parameter = Expression.Parameter(clrType, "e");
                var property = Expression.Property(parameter, nameof(EntityBase.IsDelete));
                var condition = Expression.Equal(property, Expression.Constant(false));
                var lambda = Expression.Lambda(condition, parameter);
                
                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
        }
        
        base.OnModelCreating(modelBuilder);
    }
}

// サービス登録
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    services.AddScoped<AuditInterceptor>();
    services.AddScoped<SoftDeleteInterceptor>();
    services.AddScoped<VersionControlInterceptor>();
    
    services.AddScoped(typeof(CacheService<>));
    services.AddScoped(typeof(VersionControlService<>));
    services.AddScoped<CacheManager>();
    
    services.AddMemoryCache();
}
```

### 🖊️ ロギングツール库 (GameFrameX.Foundation.Logger)

Serilogベースのログ設定ツールで、シンプルで使いやすいログ記録機能を提供します。

#### 特徴

- 複数のログレベルをサポート (Debug, Info, Warning, Error, Fatal)
- 柔軟な出力設定
- カスタムログプロバイダーをサポート
- ログ自己診断機能
- ✅ **预初始化日志支持**: 手動初期化不要。LogHelper を直接使用可能
- ✅ **日志自动合并**: 初期化前後のログは正式ログシステムに自動マージ

#### 事前初期化ログ機能

正式なログシステムの初期化前に、LogHelperを使用してコンソールにログを出力できます。`LogHandler.Create()`で正式なログを初期化すると、それまでの一時ログは新しいログシステムに自動的にマージされ、ログの損失を防ぎます。

```csharp
class Program
{
    static void Main(string[] args)
    {
        // 初期化なしでLogHelperを直接使用可能
        LogHelper.Info("設定を読み込み中...");
        LogHelper.Debug("パラメータ: {Args}", string.Join(", ", args));
        LogHelper.Warning("設定が存在しません、デフォルト値を使用");

        // 正式なログシステムを初期化
        var logger = LogHandler.Create(options);

        // 以前の一時ログは新しいログシステムに自動マージ済み
        LogHelper.Info("システム起動完了");
    }
}
```

#### 使用例

```csharp
// ログの初期化
LogHandler.Create(LogOptions.Default);

// ログを記録
LogHelper.Debug("デバッグ情報");
LogHelper.Info("通常メッセージ");
LogHelper.Warning("警告メッセージ");
LogHelper.Error("エラーメッセージ");
LogHelper.Fatal("致命的エラー");
```

### ⚙️ 命令行引数処理 (GameFrameX.Foundation.Options)

強力なコマンドライン引数・環境変数解析ライブラリで、コマンドライン引数と環境変数を強く型付けされた設定オブジェクトに自動マッピングします。

#### 特徴

- ✅ **パラメータ優先度処理**: コマンドライン引数 > 环境变量 > 默认值
- ✅ **ジェネリックサポート**: 支持任意强类型配置类
- ✅ **複数起動方式互換**: Docker、exe、shell等の起動方式をサポート
- ✅ **自動プレフィックス処理**: 自动为参数添加`--`前缀
- ✅ **ブールパラメータサポート**: 支持多种布尔参数格式
- ✅ **環境変数マッピング**: 自动映射环境变量到配置属性
- ✅ **型変換**: 文字列パラメータをターゲット型に自動変換
- ✅ **属性サポート**: 支持丰富的配置特性

#### コアコンポーネント

| コンポーネント | 機能説明 |
|--------------------------------|----------------------|
| `CommandLineArgumentConverter` | コマンドライン引数コンバーター、引数処理のコア機能を提供 |
| `OptionsBuilder<T>` | 設定ビルダー、ジェネリック設定オブジェクトの構築に使用 |
| `OptionsProvider` | 設定プロバイダー、設定オブジェクトの取得と管理に使用 |

#### クイックスタート

##### 1. 設定クラスの定義

```csharp
public class AppConfig
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    public bool Debug { get; set; } = false;
    public string LogLevel { get; set; } = "info";
    public double Timeout { get; set; } = 30.5;
}
```

##### 2. OptionsBuilderの使用

```csharp
using GameFrameX.Foundation.Options;

class Program
{
    static void Main(string[] args)
    {
        // オプションビルダーを作成
        var builder = new OptionsBuilder<AppConfig>(args);
        
        // 設定オブジェクトを構築
        var config = builder.Build();
        
        // 設定を使用
        Console.WriteLine($"サーバー: {config.Host}:{config.Port}");
        Console.WriteLine($"デバッグモード: {config.Debug}");
        Console.WriteLine($"ログレベル: {config.LogLevel}");
        Console.WriteLine($"タイムアウト: {config.Timeout}秒");
    }
}
```

#### 使用方法

##### コマンドライン引数

複数の引数形式をサポート：

```bash
# キー・バリュー形式
myapp.exe --host=example.com --port=9090 --debug=true

# セパレータ形式
myapp.exe --host example.com --port 9090 --debug true

# ブールフラグ形式
myapp.exe --host example.com --port 9090 --debug

# 混合形式
myapp.exe --host=example.com --port 9090 --debug
```

##### 環境変数

```bash
# 環境変数の設定
export HOST=example.com
export PORT=9090
export DEBUG=true

# プログラムの実行
myapp.exe
```

##### Dockerサポート

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

```bash
# Dockerでの実行
docker run myapp --host example.com --port 9090 --debug

# または環境変数を使用
docker run -e HOST=example.com -e PORT=9090 -e DEBUG=true myapp
```

#### 高度な機能

##### 属性による設定

```csharp
using GameFrameX.Foundation.Options.Attributes;

public class AdvancedConfig
{
    [Option("h", "host", Required = false, DefaultValue = "localhost")]
    [HelpText("サーバーのホストアドレス")]
    public string Host { get; set; }

    [Option("p", "port", Required = true)]
    [HelpText("服务器ポート号")]
    public int Port { get; set; }

    [FlagOption("d", "debug")]
    [HelpText("デバッグモードを有効化")]
    public bool Debug { get; set; }

    [RequiredOption("api-key", Required = true)]
    [EnvironmentVariable("API_KEY")]
    [HelpText("APIキー")]
    public string ApiKey { get; set; }

    [DefaultValue(30.0)]
    public double Timeout { get; set; }
}
```

##### ビルダーオプション

```csharp
var builder = new OptionsBuilder<AppConfig>(
    args: args,
    boolFormat: BoolArgumentFormat.Flag,        // ブールパラメータ形式
    ensurePrefixedKeys: true,                   // パラメータにプレフィックスがあることを確認
    useEnvironmentVariables: true              // 環境変数を使用
);

var config = builder.Build(skipValidation: false); // 検証をスキップするかどうか
```

#### 引数の優先順位

以下の優先順位でパラメータが適用されます（高優先度が低優先度を上書き）：

1. **コマンドライン引数** (最高優先度)
2. **環境変数**
3. **デフォルト値** (最低優先度)

##### 使用例

```csharp
public class Config
{
    public string Host { get; set; } = "localhost";  // デフォルト値
    public int Port { get; set; } = 8080;           // デフォルト値
}
```

```bash
# 環境変数の設定
export HOST=env.example.com
export PORT=7070

# プログラムの実行（コマンドライン引数覆盖环境变量）
myapp.exe --host cmd.example.com

# 結果：
# Host = "cmd.example.com"  (来自コマンドライン引数)
# Port = 7070               （環境変数から）
```

#### ブール引数処理

複数のブールパラメータ形式をサポート：

```bash
# フラグ形式（推奨）
myapp.exe --debug                    # debug = true

# キー・バリュー形式
myapp.exe --debug=true               # debug = true
myapp.exe --debug=false              # debug = false

# セパレータ形式
myapp.exe --debug true               # debug = true
myapp.exe --debug false              # debug = false

# サポートされるブール値
true, false, 1, 0, yes, no, on, off
```

#### 型変換

以下の型変換を自動サポート：

- `string` - そのまま使用
- `int`, `int?` - 整数変換
- `bool`, `bool?` - ブール値変換
- `double`, `double?` - 倍精度浮動小数点数変換
- `float`, `float?` - 単精度浮動小数点数変換
- `decimal`, `decimal?` - 10進数変換
- `DateTime`, `DateTime?` - 日時変換
- `Guid`, `Guid?` - GUID変換
- `Enum` - 列挙型変換

##### 使用例

```csharp
public class TypedConfig
{
    public int Port { get; set; }
    public bool Debug { get; set; }
    public DateTime StartTime { get; set; }
    public LogLevel Level { get; set; }  // 列挙型
}

public enum LogLevel
{
    Debug, Info, Warning, Error
}
```

```bash
myapp.exe --port 9090 --debug true --start-time "2024-01-01 10:00:00" --level Info
```

#### エラー処理

##### 必須パラメータ検証

```csharp
public class Config
{
    [RequiredOption("api-key", Required = true)]
    public string ApiKey { get; set; }
}
```

必須パラメータが不足している場合、`ArgumentException`がスローされます：

```
必須オプションが不足: api-key
```

##### 型変換エラー

パラメータ値をターゲット型に変換できない場合、デフォルト値が使用され、コンソールに警告メッセージが出力されます。

#### ベストプラクティス

##### 1. 設定クラスの設計

```csharp
public class AppConfig
{
    // 意味のあるデフォルト値を使用
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    
    // ブールプロパティのデフォルトはfalse
    public bool Debug { get; set; } = false;
    
    // 属性を使用して詳細情報を提供
    [RequiredOption("database-url", Required = true)]
    [EnvironmentVariable("DATABASE_URL")]
    public string DatabaseUrl { get; set; }
}
```

##### 2. エラー処理

```csharp
try
{
    var builder = new OptionsBuilder<AppConfig>(args);
    var config = builder.Build();
    
    // 設定を使用启动应用
    StartApplication(config);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"設定エラー: {ex.Message}");
    Environment.Exit(1);
}
```

##### 3. Docker統合

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = new OptionsBuilder<AppConfig>(args);
        var config = builder.Build();
        
        // Dockerでは通常、環境変数を使用
        // 開発では通常、コマンドライン引数を使用
        
        var app = CreateApplication(config);
        app.Run();
    }
}
```

```yaml
# docker-compose.yml
version: '3.8'
services:
  myapp:
    image: myapp:latest
    environment:
      - HOST=0.0.0.0
      - PORT=8080
      - DEBUG=false
    command: [ "--log-level", "info" ]
```

#### 完全な使用例

```csharp
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace MyApp
{
    public class ServerConfig
    {
        [Option("h", "host", DefaultValue = "localhost")]
        [EnvironmentVariable("SERVER_HOST")]
        [HelpText("サーバーのホストアドレス")]
        public string Host { get; set; }

        [Option("p", "port", DefaultValue = 8080)]
        [EnvironmentVariable("SERVER_PORT")]
        [HelpText("服务器ポート号")]
        public int Port { get; set; }

        [FlagOption("d", "debug")]
        [EnvironmentVariable("DEBUG")]
        [HelpText("デバッグモードを有効化")]
        public bool Debug { get; set; }

        [RequiredOption("database-url", Required = true)]
        [EnvironmentVariable("DATABASE_URL")]
        [HelpText("データベース接続文字列")]
        public string DatabaseUrl { get; set; }

        [Option("timeout", DefaultValue = 30.0)]
        [EnvironmentVariable("REQUEST_TIMEOUT")]
        [HelpText("リクエストタイムアウト時間（秒）")]
        public double Timeout { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new OptionsBuilder<ServerConfig>(args);
                var config = builder.Build();

                Console.WriteLine("サーバー設定:");
                Console.WriteLine($"  ホスト: {config.Host}");
                Console.WriteLine($"  ポート: {config.Port}");
                Console.WriteLine($"  デバッグ: {config.Debug}");
                Console.WriteLine($"  データベース: {config.DatabaseUrl}");
                Console.WriteLine($"  タイムアウト: {config.Timeout}秒");

                // サーバーの起動
                StartServer(config);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"設定エラー: {ex.Message}");
                ShowHelp();
                Environment.Exit(1);
            }
        }

        static void StartServer(ServerConfig config)
        {
            // サーバー起動ロジック
            Console.WriteLine($"サーバーを起動 {config.Host}:{config.Port}");
        }

        static void ShowHelp()
        {
            Console.WriteLine("使用方法:");
            Console.WriteLine("  myapp.exe --host <ホスト> --port <ポート> --database-url <データベースURL> [选项]");
            Console.WriteLine();
            Console.WriteLine("オプション:");
            Console.WriteLine("  -h, --host <ホスト>           服务器ホスト地址 (默认: localhost)");
            Console.WriteLine("  -p, --port <ポート>           服务器ポート号 (默认: 8080)");
            Console.WriteLine("  -d, --debug                 デバッグモードを有効化");
            Console.WriteLine("      --database-url <URL>    データベース接続文字列 (必需)");
            Console.WriteLine("      --timeout <秒>          リクエストタイムアウト時間 (默认: 30.0)");
        }
    }
}
```

#### CommandLineArgumentConverter 使用

OptionsBuilderの他に、低レベルのCommandLineArgumentConverterを直接使用することもできます：

```csharp
using GameFrameX.Foundation.Options;

// コンバーターのインスタンスを作成
var converter = new CommandLineArgumentConverter();

// 元のコマンドライン引数
var args = new[] { "--port", "8080", "-h", "localhost" };

// 環境変数の設定（オプション）
Environment.SetEnvironmentVariable("APP_NAME", "MyApplication");
Environment.SetEnvironmentVariable("LOG_LEVEL", "debug-mode");

// 標準形式に変換（コマンドライン引数と環境変数をマージ）
var standardArgs = converter.ConvertToStandardFormat(args);
// 結果: ["--port", "8080", "-h", "localhost", "--APP_NAME", "MyApplication", "--LOG_LEVEL", "debugmode"]

// コマンドライン文字列に変換
var commandLineString = converter.ToCommandLineString(standardArgs);
// 結果: "--port 8080 -h localhost --APP_NAME MyApplication --LOG_LEVEL debugmode"

// 全環境変数を取得
var envVars = converter.GetEnvironmentVariables();
Console.WriteLine($"検出 {envVars.Count} 個の環境変数");
```

##### ブール型パラメータのサポート

`CommandLineArgumentConverter`はブール型パラメータのインテリジェントな識別と処理をサポートし、3つの形式を提供します：

```csharp
using GameFrameX.Foundation.Options;

// ブール型環境変数を設定
Environment.SetEnvironmentVariable("ENABLE_LOGGING", "true");
Environment.SetEnvironmentVariable("DEBUG_MODE", "false");
Environment.SetEnvironmentVariable("VERBOSE", "yes");

var converter = new CommandLineArgumentConverter();

// 1. フラグ形式（デフォルト）- true値のみフラグを追加
converter.BoolFormat = BoolArgumentFormat.Flag;
var flagArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 結果: ["--ENABLE_LOGGING", "--VERBOSE"] （true値のみ含む）

// 2. キー・バリュー形式 - キー・バリューペアを追加
converter.BoolFormat = BoolArgumentFormat.KeyValue;
var keyValueArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 結果: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]

// 3. セパレータ形式 - キーと値を分離
converter.BoolFormat = BoolArgumentFormat.Separated;
var separatedArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 結果: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]
```

サポートされるブール値の形式：

- **True 值**: `"true"`, `"1"`, `"yes"`, `"on"`, `"enabled"` （大文字小文字を区別しない）
- **False 值**: `"false"`, `"0"`, `"no"`, `"off"`, `"disabled"` （大文字小文字を区別しない）

### 🛠️ 汎用ユーティリティクラス (GameFrameX.Foundation.Utility)

コンソール操作、環境管理、時間処理、スノーフレークID生成などの実用的なユーティリティクラス群を提供します。

#### コアコンポーネント概要

| コンポーネント | ファイル | 説明 |
|-----------|------------------------|---------------------------|
| **コンソールヘルパー** | `ConsoleHelper.cs`     | 控制台Logo打印和格式化输出           |
| **環境ヘルパー**  | `EnvironmentHelper.cs` | 环境变量管理和环境类型定义             |
| **タイムヘルパー**  | `TimerHelper.cs`       | Unix时间戳处理和时间转换            |
| **スノーフレーク ID**  | `SnowFlakeIdHelper.cs` | 分布式唯一ID生成器（Snowflakeアルゴリズム实现） |

#### コンソールヘルパー機能

```csharp
using GameFrameX.Foundation.Utility;

// アプリケーションLogoを印刷
ConsoleHelper.PrintLogo();
// フォーマットされたコンソールLogoを出力（アプリケーション起動時のブランド表示）
```

#### 環境管理機能

```csharp
using GameFrameX.Foundation.Utility;

// 現在の環境タイプを取得
string currentEnv = Environments.Development;
Console.WriteLine($"現在の環境: {currentEnv}");

// 環境判定
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
{
    // 開発環境固有のロジック
    Console.WriteLine("開発環境で実行");
}
```

#### 時間処理機能

```csharp
using GameFrameX.Foundation.Utility;

// Unixタイムスタンプ定数
DateTime epochLocal = TimerHelper.EpochLocal;   // ローカルタイムゾーンのUnixエポック時間
DateTime epochUtc = TimerHelper.EpochUtc;       // UTCタイムゾーンのUnixエポック時間

// 現在のUnixタイムスタンプ（秒）を取得
long unixSeconds = TimerHelper.UnixTimeSeconds();
Console.WriteLine($"現在のUnixタイムスタンプ（秒）: {unixSeconds}");

// 現在のUnixタイムスタンプ（ミリ秒）を取得
long unixMilliseconds = TimerHelper.UnixTimeMilliseconds();
Console.WriteLine($"現在のUnixタイムスタンプ（ミリ秒）: {unixMilliseconds}");

// タイムスタンプ変換の例
DateTime currentTime = DateTime.UtcNow;
long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
DateTime restored = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
```

#### スノーフレークIDジェネレーター

```csharp
using GameFrameX.Foundation.Utility;

// デフォルト設定でIDを生成
long id1 = SnowFlakeIdHelper.GenerateId();
long id2 = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"生成されたID: {id1}, {id2}");

// ワーカーノードIDとデータセンターIDを設定
SnowFlakeIdHelper.WorkId = 1;        // ワーカーノードID (0-31)
SnowFlakeIdHelper.DataCenterId = 1;  // データセンターID (0-31)

// 設定後のIDを生成
long configuredId = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"設定後のID: {configuredId}");

// タイムスタンプ関連情報を取得
DateTime utcStart = SnowFlakeIdHelper.UtcTimeStart;  // UTC開始時刻
long epochTime = SnowFlakeIdHelper.EpochTime;        // エポックタイムスタンプ

Console.WriteLine($"スノーフレークID開始時刻: {utcStart}");
Console.WriteLine($"エポックタイムスタンプ: {epochTime}");
```

##### スノーフレークIDアルゴリズム説明

雪花ID（Snowflake）是Twitter开源的分布式ID生成アルゴリズム，具有以下特点：

- **グローバルに一意**: 在分布式环境中保证ID的グローバルに一意性
- **傾向増加**: 生成されたID大致按时间递增，有利于数据库索引
- **高性能**: 単一マシンで毎秒数百万のIDを生成可能
- **依存なし**: 不依赖数据库或其他外部系统

ID構造（64ビット）：

```
0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
|   |                                             |   |       |       |
|   |<-------------- 41ビットタイムスタンプ ---------------->|   |<-5ビット->|<-5ビット->|<--12ビット-->
|                                                 |           |       |
符号ビット(1ビット)                                        |      データセンターID   シーケンス番号
                                                  |      (5ビット)      (12位)
                                               ワーカーノードID
                                                (5ビット)
```

- **1ビット符号ビット**: 常に0
- **41ビットタイムスタンプ**: ミリ秒精度、約69年間使用可能
- **5ビットデータセンターID**: 32個のデータセンターをサポート
- **5ビットワーカーノードID**: 各データセンターは32個のワーカーノードをサポート
- **12ビットシーケンス番号**: 同一ミリ秒内で4096個のIDをサポート

#### 完全な使用例

```csharp
using GameFrameX.Foundation.Utility;

namespace MyApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // アプリケーションLogoを印刷
            ConsoleHelper.PrintLogo();
            
            // 実行環境をチェック
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;
            Console.WriteLine($"現在の実行環境: {env}");
            
            // スノーフレークIDジェネレーターを設定
            SnowFlakeIdHelper.WorkId = 1;
            SnowFlakeIdHelper.DataCenterId = 1;
            
            // ユニークIDを生成
            for (int i = 0; i < 5; i++)
            {
                long id = SnowFlakeIdHelper.GenerateId();
                long timestamp = TimerHelper.UnixTimeMilliseconds();
                
                Console.WriteLine($"ID: {id}, タイムスタンプ: {timestamp}");
                
                // ID変化を観察するための短い遅延
                Thread.Sleep(1);
            }
            
            // 時間処理の例
            Console.WriteLine($"Unixエポック時間(UTC): {TimerHelper.EpochUtc}");
            Console.WriteLine($"Unixエポック時間(ローカル): {TimerHelper.EpochLocal}");
            Console.WriteLine($"現在のUnixタイムスタンプ(秒): {TimerHelper.UnixTimeSeconds()}");
            Console.WriteLine($"現在のUnixタイムスタンプ(ミリ秒): {TimerHelper.UnixTimeMilliseconds()}");
        }
    }
}
```

## 🧪 テスト

プロジェクトには完全な単体テストが含まれており、コード品質と機能の正確性を確保しています。全コア機能に対応するテストケースがあり、テストカバレッジは95%以上です。

### テストカバレッジ

#### 🧩 拡張メソッド库测试 (Extensions)

- **ArgumentAlreadyExceptionTests**: パラメータ既存在例外テスト
- **BidirectionalDictionaryTests**: 双方向ディクショナリ機能テスト
- **ByteExtensionTests**: 字节数组拡張メソッド测试
- **CollectionExtensionsTests**: 集合拡張メソッド测试
- **ConcurrentLimitedQueueTests**: 並行制限キューテスト
- **DisposableConcurrentDictionaryTests**: 破棄可能並行ディクショナリテスト
- **DisposableDictionaryTests**: 破棄可能ディクショナリテスト
- **IDictionaryExtensionsTests**: 字典拡張メソッド测试
- **IEnumerableExtensionsTests**: 枚举拡張メソッド测试
- **ListExtensionsTests**: 列表拡張メソッド测试
- **LookupXTests**: ルックアップテーブル機能テスト
- **NullObjectTests**: Nullオブジェクトパターンテスト
- **NullableConcurrentDictionaryTests**: Null許容並行ディクショナリテスト
- **NullableDictionaryTests**: Null許容ディクショナリテスト
- **ObjectExtensionsTests**: 对象拡張メソッド测试
- **ReadOnlySpanExtensionsTests**: 読み取り専用Span拡張テスト
- **SequenceReaderExtensionsTests**: シーケンスリーダー拡張テスト
- **SpanExtensionsTests**: Span拡張メソッド测试
- **StringExtensionsTests**: 字符串拡張メソッド测试
- **TypeExtensionsTests**: 类型拡張メソッド测试

#### 🔐 暗号化ツール库测试 (Encryption)

- **AesHelperTests**: AES加密アルゴリズム测试
- **DsaHelperTests**: DSAデジタル署名テスト
- **RsaHelperTests**: RSA加密アルゴリズム测试
- **Sm2HelperTests**: SM2国密アルゴリズム测试
- **Sm4HelperTests**: SM4国密アルゴリズム测试
- **XorHelperTests**: XOR暗号化テスト

#### 🌐 本地化框架测试 (Localization)

- **LocalizationServiceTests**: ローカライズサービスコア機能テスト
    - シングルトンパターン検証テスト
    - ローカライズ文字列取得テスト
    - パラメータ付きメッセージフォーマットテスト
    - 不明キー処理テスト
    - スレッドセーフ同時実行テスト
- **ResourceManagerTests**: リソースマネージャーテスト
    - プロバイダー優先度テスト
    - レイジーロード機構テスト
    - 統計情報検証テスト
- **DefaultResourceProviderTests**: デフォルトリソースプロバイダーテスト
- **AssemblyResourceProviderTests**: アセンブリリソースプロバイダーテスト
    - .resxファイル読み込みテスト
    - 多文化サポートテスト
    - リソースキャッシュ機構テスト

#### 🔗 ハッシュツール库测试 (Hash)

- **CrcHelperTests**: CRC校验アルゴリズム测试
- **HmacSha256HelperTests**: HMAC-SHA256テスト
- **Md5HelperTests**: MD5哈希アルゴリズム测试
- **MurmurHash3HelperTests**: MurmurHash3アルゴリズム测试
- **Sha1HelperTests**: SHA-1哈希アルゴリズム测试
- **Sha256HelperTests**: SHA-256哈希アルゴリズム测试
- **Sha512HelperTests**: SHA-512哈希アルゴリズム测试
- **XxHashHelperTests**: xxHash高性能ハッシュテスト

#### 🌐 HTTP工具库测试 (Http.Extension)

- **HttpExtensionTests**: HTTP客户端拡張メソッド测试

#### ⚙️ 命令行引数処理测试 (Options)

- **CommandLineArgumentConverterTests**: コマンドライン引数转换器功能测试
    - 空パラメータ配列処理テスト
    - 空パラメータ値処理テスト
    - 重複パラメータ検出テスト
    - 環境変数変換テスト
    - 値クリーンアップ機能テスト
    - 単一ハイフンパラメータ変換テスト
    - コマンドライン文字列生成テスト
    - 環境変数取得テスト
    - 完全なワークフローテスト
    - ブール型引数処理テスト
        - フラグ形式ブールパラメータテスト
        - キー・バリュー形式ブールパラメータテスト
        - セパレータ形式ブールパラメータテスト
        - 複数ブール値形式の解析テスト
        - 非ブール値処理テスト
- **OptionsBuilderTests**: オプションビルダー機能テスト
    - 基本設定ビルドテスト
    - 属性設定テスト
    - 型変換テスト
    - 検証機能テスト
- **OptionsProviderTests**: オプションプロバイダー機能テスト
    - 設定の登録と取得テスト
    - グローバル設定管理テスト

### テストの実行

```bash
# すべてのテストを実行
dotnet test

# 特定モジュールのテストを実行
dotnet test --filter "FullyQualifiedName~Extensions"
dotnet test --filter "FullyQualifiedName~Encryption"
dotnet test --filter "FullyQualifiedName~Hash"
dotnet test --filter "FullyQualifiedName~Localization"
dotnet test --filter "FullyQualifiedName~Options"

# 特定テストクラスを実行
dotnet test --filter "ClassName=XxHashHelperTests"
dotnet test --filter "ClassName=StringExtensionsTests"
dotnet test --filter "ClassName=LocalizationServiceTests"
dotnet test --filter "ClassName=CommandLineArgumentConverterTests"

# テストカバレッジレポートの生成
dotnet test --collect:"XPlat Code Coverage"

# パフォーマンステストの実行
dotnet test --filter "Category=Performance"
```

### テストの特徴

- **全面覆盖**: すべてのパブリックメソッドにテストケースあり
- **边界测试**: null、境界値、例外ケースのテストを含む
- **性能测试**: 主要アルゴリズムのパフォーマンスベンチマーク
- **并发测试**: マルチスレッド環境でのスレッドセーフコンポーネントの正確性を検証
- **兼容性测试**: 異なる .NET バージョンでの互換性を確保

## 🏗️ アーキテクチャ

### 設計原則

- **高性能**: 全コンポーネントがパフォーマンス最適化済み。高同時実行シナリオに対応
- **易用性**: シンプルな API 設計で学習コストを削減
- **可扩展**: モジュール化设计，支持自定义扩展
- **类型安全**: .NET の型システムを活用しランタイムエラーを削減
- **内存友好**: Span<T> や Memory<T> などの最新 .NET 機能でメモリ割り当てを削減

### 依存関係

```
GameFrameX.Foundation.Extensions (コア拡張)
├── GameFrameX.Foundation.Encryption (暗号化ツール)
├── GameFrameX.Foundation.Hash (ハッシュツール)
├── GameFrameX.Foundation.Json (JSON ツール)
├── GameFrameX.Foundation.Logger (ロギングツール)
├── GameFrameX.Foundation.Options (引数処理)
├── GameFrameX.Foundation.Http.Extension (HTTP拡張)
└── GameFrameX.Foundation.Http.Normalization (HTTP標準化)
```

## 🔧 開発ガイド

### 環境要件

- .NET 10.0 以上
- C# 12.0 以上

### プロジェクトのビルド

```bash
# リポジトリのクローン
git clone https://github.com/GameFrameX/GameFrameX.Foundation.git
cd GameFrameX.Foundation

# 依存関係の復元
dotnet restore

# プロジェクトのビルド
dotnet build

# テストの実行
dotnet test
```

### コントリビューションガイド

1. このリポジトリをフォーク
2. 機能ブランチを作成 (`git checkout -b feature/AmazingFeature`)
3. 変更をコミット (`git commit -m 'Add some AmazingFeature'`)
4. ブランチにプッシュ (`git push origin feature/AmazingFeature`)
5. プルリクエストを作成

## 📊 パフォーマンスベンチマーク

### 拡張メソッド性能

| 操作        | 従来手法  | 拡張メソッド  | 性能向上 |
|-----------|-------|-------|------|
| 文字列 null チェック   | 100ns | 15ns  | 85%  |
| コレクションランダム要素取得  | 200ns | 50ns  | 75%  |
| Span バイト操作 | 500ns | 80ns  | 84%  |
| 双方向ディクショナリ検索    | 150ns | 120ns | 20%  |

### 加密アルゴリズム性能

| アルゴリズム       | データサイズ | 暗号化時間   | 復号時間   |
|----------|------|--------|--------|
| AES-256  | 1KB  | 0.05ms | 0.04ms |
| RSA-2048 | 1KB  | 2.1ms  | 0.8ms  |
| SM4      | 1KB  | 0.08ms | 0.07ms |
| XOR      | 1KB  | 0.01ms | 0.01ms |

### 哈希アルゴリズム性能

| アルゴリズム          | データサイズ | 処理時間  | スループット      |
|-------------|------|-------|----------|
| MD5         | 1MB  | 2.1ms | 476MB/s  |
| SHA-256     | 1MB  | 3.8ms | 263MB/s  |
| xxHash64    | 1MB  | 0.8ms | 1.25GB/s |
| MurmurHash3 | 1MB  | 1.2ms | 833MB/s  |

## 📋 システム要件

- .NET 10.0 以上
- Windows、Linux、macOS をサポート

## 🤝 コントリビューション

プロジェクトの改善のために、IssueとPull Requestを歓迎します。

1. プロジェクトをフォーク
2. 機能ブランチを作成 (`git checkout -b feature/AmazingFeature`)
3. 変更をコミット (`git commit -m 'Add some AmazingFeature'`)
4. ブランチにプッシュ (`git push origin feature/AmazingFeature`)
5. プルリクエストを作成

## 🤝 コミュニティサポート

- **イシュー**: [GitHub Issues](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- **機能リクエスト**: [GitHub Discussions](https://github.com/GameFrameX/GameFrameX.Foundation/discussions)
- **ドキュメント貢献**: ドキュメント改善の PR を歓迎

## 📄 ライセンス

このプロジェクトはMITライセンスを採用しています - 詳細は[LICENSE](LICENSE)ファイルをご覧ください。

## 🙏 謝辞

GameFrameX.Foundation に貢献してくださった全ての開発者の皆様に感謝します！

## 🔗 関連リンク

- [GameFrameX 公式サイト](https://gameframex.doc.alianblank.com)
- [ドキュメントセンター](https://gameframex.doc.alianblank.com)

---

<div align="center">

**[⬆ トップに戻る](#gameframexfoundation)**

Made with ❤️ by GameFrameX Team

</div>
