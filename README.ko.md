<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# GameFrameX.Foundation

[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation?label=version&color=green)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현**

[📖 문서](https://gameframex.doc.alianblank.com) • [🚀 빠른 시작](#-빠른-시작)

---

🌐 **언어**: [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

---

</div>

### 📦 어셈블리 개요

| 어셈블리                                      | 기능 설명          | NuGet 包名                                   | 버전                                                                                                                                                                | 다운로드 수                                                                                                                                                               |
|------------------------------------------|---------------|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| GameFrameX.Foundation.Encryption         | 암호화 라이브러리         | `GameFrameX.Foundation.Encryption`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption/)                 |
| GameFrameX.Foundation.Extensions         | 확장 메서드 라이브러리         | `GameFrameX.Foundation.Extensions`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions/)                 |
| GameFrameX.Foundation.Hash               | 해시 라이브러리         | `GameFrameX.Foundation.Hash`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash/)                             |
| GameFrameX.Foundation.Http.Extension     | HttpClient 확장 | `GameFrameX.Foundation.Http.Extension`     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension/)         |
| GameFrameX.Foundation.Http.Normalization | HTTP 메시지 표준화    | `GameFrameX.Foundation.Http.Normalization` | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization/) |
| GameFrameX.Foundation.Json               | JSON 직렬화 도구    | `GameFrameX.Foundation.Json`               | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Json/)                             |
| GameFrameX.Foundation.Localization       | 현지화 프레임워크         | `GameFrameX.Foundation.Localization`       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization/)             |
| GameFrameX.Foundation.Logger             | Serilog 로거 설정  | `GameFrameX.Foundation.Logger`             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger/)                         |
| GameFrameX.Foundation.Options            | CLI 인수 파서       | `GameFrameX.Foundation.Options`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Options/)                       |
| GameFrameX.Foundation.Orm.Attribute      | ORM 속성 마크      | `GameFrameX.Foundation.Orm.Attribute`      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute/)           |
| GameFrameX.Foundation.Orm.Entity         | ORM 엔티티 기본      | `GameFrameX.Foundation.Orm.Entity`         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity/)                 |
| GameFrameX.Foundation.Utility            | 유틸리티 클래스         | `GameFrameX.Foundation.Utility`            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility.svg)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility/)                       |

GameFrameX의 기반 유틸리티 라이브러리는 암호화, 해시, HTTP, JSON, 로깅 등 일반적인 기능을 다루는 고성능의 사용하기 쉬운 컴포넌트 모음을 제공합니다.

## 🚀 빠른 시작

### 설치

NuGet 패키지 매니저로 필요한 컴포넌트를 설치하세요:

```bash
# 암호화 라이브러리 설치
dotnet add package GameFrameX.Foundation.Encryption

# 확장 메서드 라이브러리 설치
dotnet add package GameFrameX.Foundation.Extensions

# 해시 라이브러리 설치
dotnet add package GameFrameX.Foundation.Hash

# JSON 라이브러리 설치
dotnet add package GameFrameX.Foundation.Json

# 현지화 프레임워크 설치
dotnet add package GameFrameX.Foundation.Localization

# 로깅 라이브러리 설치
dotnet add package GameFrameX.Foundation.Logger

# CLI 매개변수 파서 설치
dotnet add package GameFrameX.Foundation.Options

# HTTP 확장 설치
dotnet add package GameFrameX.Foundation.Http.Extension

# HTTP 메시지 표준화 설치
dotnet add package GameFrameX.Foundation.Http.Normalization
```

### 기본 사용법

```csharp
using GameFrameX.Foundation.Encryption;
using GameFrameX.Foundation.Extensions;
using GameFrameX.Foundation.Hash;
using GameFrameX.Foundation.Json;
using GameFrameX.Foundation.Localization.Core;
using GameFrameX.Foundation.Logger;
using GameFrameX.Foundation.Options;

// AES 암호화
string encrypted = AesHelper.Encrypt("Hello World", "your-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-key");

// 확장 메서드使用
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.RandomElement(); // 무작위 요소 가져오기
var isNullOrEmpty = myString.IsNullOrEmpty(); // 문자열 검사

// 문자열 확장
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // URL 안전 Base64
string centered = "Hello".CenterAlignedText(20); // 가운데 정렬

// 객체 검증
object obj = GetSomeObject();
obj.ThrowIfNull(nameof(obj)); // null 검사
int value = 50;
value.CheckRange(1, 100); // 범위 검사

// 고성능 바이트 연산
Span<byte> buffer = stackalloc byte[8];
int offset = 0;
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);

// 양방향 딕셔너리
var biDict = new BidirectionalDictionary<string, int>();
biDict.TryAdd("one", 1);
if (biDict.TryGetKey(1, out string key)) { /* 역방향 조회 */ }

// 명령줄 매개변수 처리
var builder = new OptionsBuilder<AppConfig>(args);
var config = builder.Build();

// SHA-256 해시
string hash = Sha256Helper.ComputeHash("Hello World");

// JSON 직렬화
string json = JsonHelper.Serialize(myObject);
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 현지화 문자열 가져오기
var successMessage = LocalizationService.GetString("Success");
var errorMessage = LocalizationService.GetString("Utility.Exceptions.TimestampOutOfRange");
var formattedMessage = LocalizationService.GetString("Encryption.InvalidKeySize", 128, 256);

// 로그 기록
LogHandler.Create(LogOptions.Default);
LogHelper.Info("애플리케이션 시작");
```

## 📚 상세 문서

### 🧩 확장 메서드 라이브러리 (GameFrameX.Foundation.Extensions)

.NET 기본 형식의 기능을 강화하는 풍부한 확장 메서드 컬렉션으로 개발 효율성과 코드 가독성을 향상시킵니다.

#### 핵심 컴포넌트 개요

| 컴포넌트 | 파일 | 설명 |
|--------------|-------------------------------------------------------------------|--------------------------------|
| **컬렉션 확장**     | `CollectionExtensions.cs`                                         | 다양한 컬렉션 타입에 편리한 연산 메서드 제공 |
| **문자열 확장**    | `StringExtensions.cs`                                             | 문자열 처리 능력 향상, URL 안전 Base64, 가운데 정렬 등 포함 |
| **객체 확장**     | `ObjectExtensions.cs`                                             | 객체 검증 및 숫자 범위 검사 제공 |
| **형식 확장**     | `TypeExtensions.cs`                                               | 타입 검사 및 리플렉션 관련 확장 메서드 |
| **열거형 확장**     | `IEnumerableExtensions.cs`                                        | LINQ 향상 및 컬렉션 연산, 교집합, 차집합 등 지원 |
| **딕셔너리 확장**     | `IDictionaryExtensions.cs`                                        | 딕셔너리 연산 향상, 병합, 조건부 제거 등 지원 |
| **리스트 확장**     | `ListExtensions.cs`                                               | 리스트 특정 확장 메서드 |
| **바이트 확장**     | `ByteExtensions.cs`                                               | 바이트 배열 연산, 하위 배열 추출 등 포함 |
| **Span 확장**   | `SpanExtensions.cs`                                               | 고성능 메모리 연산, 다양한 데이터 타입 읽기/쓰기, 빅 엔디안 및 리틀 엔디안 지원 |
| **ReadOnlySpan 확장** | `ReadOnlySpanExtensions.cs`                                       | 읽기 전용 메모리의 고성능 읽기 연산 |
| **시퀀스 리더 확장**  | `SequenceReaderExtensions.cs`                                     | 시퀀스 데이터의 편리한 읽기 메서드 |
| **양방향 딕셔너리**     | `BidirectionalDictionary.cs`                                      | 양방향 조회를 지원하는 딕셔너리 구현 |
| **룩업 테이블**      | `LookupX.cs`                                                      | 향상된 일대다 관계 룩업 테이블 |
| **동시성 큐**     | `ConcurrentLimitedQueue.cs`                                       | 스레드 안전한 제한 용량 큐 |
| **Null 허용 딕셔너리**     | `NullableDictionary.cs`<br/>`NullableConcurrentDictionary.cs`     | null 값을 지원하는 딕셔너리 구현 |
| **삭제 가능 딕셔너리**    | `DisposableDictionary.cs`<br/>`DisposableConcurrentDictionary.cs` | 값이 자동으로 해제되는 딕셔너리 |
| **상수 정의**     | `ConstBaseTypeSize.cs`                                            | 기본 데이터 타입 바이트 크기 상수 |
| **Null 객체**    | `NullObject.cs`                                                   | 타입 안전한 Null 객체 구현 |
| **커스텀 예외**    | `ArgumentAlreadyException.cs`                                     | 매개변수 이미 존재 예외 타입 |

#### 컬렉션 확장

```csharp
using GameFrameX.Foundation.Extensions;

// 컬렉션 연산
var list = new List<int> { 1, 2, 3, 4, 5 };
var randomItem = list.RandomElement(); // 무작위 요소 가져오기
var isEmpty = list.IsNullOrEmpty(); // 비어 있는지 확인

// 딕셔너리 확장
var dict = new Dictionary<string, int>();
dict.Merge("key", 10, (old, new) => old + new); // 값 병합
var value = dict.GetOrAdd("key", k => 42); // 가져오기 또는 추가
dict.RemoveIf((k, v) => v > 100); // 조건부 제거

// HashSet 확장
var hashSet = new HashSet<int>();
hashSet.AddRange(new[] { 1, 2, 3, 4, 5 }); // 일괄 추가
```

#### 문자열 확장

```csharp
// 문자열 검사
string text = "Hello World";
bool isEmpty = text.IsNullOrEmpty();
bool isEmptyOrWhitespace = text.IsNullOrEmptyOrWhiteSpace();
bool hasContent = text.IsNotNullOrEmptyOrWhiteSpace();

// 문자열 처리
string base64 = "SGVsbG8gV29ybGQ=";
string urlSafe = base64.ToUrlSafeBase64(); // URL 안전 형식으로 변환
string restored = urlSafe.FromUrlSafeBase64(); // 표준 형식으로 복원

// 문자열 연산
string centered = "Hello".CenterAlignedText(20); // 가운데 정렬
string cleaned = "Hello World   ".RemoveWhiteSpace(); // 공백 문자 제거
string trimmed = "Hello!".RemoveSuffix('!'); // 접미사 제거

// 문자 반복
string repeated = 'A'.RepeatChar(5); // "AAAAA"
```

#### 객체 검증 및 범위 확인

```csharp
// null 검사
object obj = GetSomeObject();
if (obj.IsNotNull())
{
    // 객체가 null이 아닐 때의 처리
}

// 매개변수 검증
obj.ThrowIfNull(nameof(obj)); // null일 때 예외 발생

// 숫자 범위 검사
int value = 50;
value.CheckRange(1, 100); // 범위 검사, 초과 시 예외 발생
bool inRange = value.IsRange(1, 100); // 범위 내에 있는지 확인

// 여러 숫자 타입 지원
uint uintValue = 25;
uintValue.CheckRange(0, 50);

long longValue = 1000;
longValue.CheckRange(500, 2000);
```

#### 형식 검사 확장

```csharp
// 제네릭 인터페이스 검사
Type listType = typeof(List<string>);
Type genericListType = typeof(List<>);
bool implementsGeneric = listType.HasImplementedRawGeneric(genericListType);

// 인터페이스 구현 검사
Type stringType = typeof(string);
Type comparableType = typeof(IComparable);
bool implementsInterface = stringType.IsImplWithInterface(comparableType);
```

#### LINQ 확장

```csharp
// 교집합 연산
var list1 = new[] { 1, 2, 3, 4, 5 };
var list2 = new[] { 3, 4, 5, 6, 7 };
var intersection = list1.IntersectBy(list2, x => x); // 키로 교집합 가져오기

// 다중 집합 교집합
var collections = new[] { list1, list2, new[] { 4, 5, 6 } };
var allIntersection = collections.IntersectAll(); // 모든 집합의 교집합

// 차집합 연산
var difference = list1.ExceptBy(list2, (x, y) => x == y);

// 일괄 추가
var collection = new List<int>();
collection.AddRange(1, 2, 3, 4, 5); // params 매개변수 사용
collection.AddRange(new[] { 6, 7, 8 }); // 배열 사용
```

#### 양방향 딕셔너리

```csharp
// 양방향 딕셔너리 생성
var biDict = new BidirectionalDictionary<string, int>();

// 키-값 쌍 추가
biDict.TryAdd("one", 1);
biDict.TryAdd("two", 2);

// 양방향 조회
if (biDict.TryGetValue("one", out int value))
{
    Console.WriteLine($"Key 'one' maps to {value}");
}

if (biDict.TryGetKey(1, out string key))
{
    Console.WriteLine($"Value 1 maps to '{key}'");
}

// 딕셔너리 비우기
biDict.Clear();
```

#### 고성능 확장

```csharp
// Span 및 ReadOnlySpan 확장
ReadOnlySpan<byte> span = stackalloc byte[] { 1, 2, 3, 4, 5 };
// Span에 대한 고성능 연산 확장 제공

// 시퀀스 리더 확장
// SequenceReader에 편리한 읽기 메서드 제공
```

#### 바이트 조작 확장

```csharp
// 바이트 배열 확장
byte[] data = { 1, 2, 3, 4, 5 };
byte[] subArray = data.SubArray(1, 3); // 하위 배열 가져오기

// Span 및 ReadOnlySpan 확장 - 고성능字节연산
Span<byte> buffer = stackalloc byte[16];
int offset = 0;

// 다양한 데이터 타입 쓰기（빅 엔디안 및 리틀 엔디안 지원）
buffer.WriteUIntValue(12345u, ref offset);
buffer.WriteFloatValue(3.14f, ref offset);
buffer.WriteUIntBigEndianValue(12345u, ref offset); // 빅 엔디안 쓰기
buffer.WriteFloatBigEndianValue(3.14f, ref offset); // 빅 엔디안 쓰기

// 데이터 타입 읽기
offset = 0;
uint value = buffer.ReadUIntValue(ref offset);
float floatValue = buffer.ReadFloatValue(ref offset);
uint bigEndianValue = buffer.ReadUIntBigEndianValue(ref offset); // 빅 엔디안 읽기

// ReadOnlySpan 读取연산
ReadOnlySpan<byte> readBuffer = buffer;
offset = 0;
uint readValue = readBuffer.ReadUIntValue(ref offset);
float readFloatValue = readBuffer.ReadFloatBigEndianValue(ref offset);
```

#### 시퀀스 리더 확장

```csharp
// SequenceReader에 편리한 읽기 메서드 제공
// 길이 접두사가 있는 바이트 배열 읽기 지원
// TryPeek 메서드로 비파괴적 읽기 제공
```

#### 특수 유틸리티 클래스

- **ConstBaseTypeSize**: 기본 데이터 타입의 바이트 크기 상수 정의, 모든 .NET 기본 타입의 바이트 크기 포함
- **NullObject**: Null 객체 패턴 구현, 타입 안전한 Null 객체 제공
- **NullableConcurrentDictionary**: null 값을 지원하는 스레드 안전 동시성 딕셔너리
- **NullableDictionary**: null 값을 지원하는 일반 딕셔너리
- **LookupX**: 향상된 룩업 테이블 구현, 일대다 관계 매핑 지원
- **ArgumentAlreadyException**: 매개변수 이미 존재 예외, 매개변수 검증 시나리오에 사용
- **ConcurrentLimitedQueue**: 스레드 안전한 제한 용량 큐, 가장 오래된 요소 자동 제거
- **DisposableConcurrentDictionary/DisposableDictionary**: 값이 자동으로 해제되는 딕셔너리 타입

### 🔐 암호화 라이브러리 (GameFrameX.Foundation.Encryption)

데이터의 안전한 전송과 저장을 보장하는 여러 암호화 알고리즘 구현을 제공합니다.

#### 지원 알고리즘

- **AES 암호화** (`AesHelper`): 대칭 암호화 알고리즘. 문자열과 바이트 배열 지원
- **RSA 암호화** (`RsaHelper`): 비대칭 암호화 알고리즘. 키 쌍 생성, 암호화/복호화, 디지털 서명 지원
- **DSA 서명** (`DsaHelper`): 디지털 서명 알고리즘. 서명 및 검증 지원
- **SM2/SM4 암호화** (`Sm2Helper`/`Sm4Helper`): 중국 국가 암호 알고리즘 구현
    - SM2: 비대칭 암호화 알고리즘
    - SM4: 대칭 암호화 알고리즘, ECB/CBC 모드 지원
- **XOR 암호화** (`XorHelper`): XOR 암호화. 빠른 암호화 및 완전 암호화 모드 지원

#### 사용 예시

```csharp
// AES 암호화
string encrypted = AesHelper.Encrypt("민감한 데이터", "your-secret-key");
string decrypted = AesHelper.Decrypt(encrypted, "your-secret-key");

// RSA 암호화
var keys = RsaHelper.Make();
string encrypted = RsaHelper.Encrypt(keys["publicKey"], "Hello World");
string decrypted = RsaHelper.Decrypt(keys["privateKey"], encrypted);

// SM4 암호화
string encrypted = Sm4Helper.EncryptCbc("your-key", "Hello World");
string decrypted = Sm4Helper.DecryptCbc("your-key", encrypted);
```

### 🔗 해시 라이브러리 (GameFrameX.Foundation.Hash)

데이터 무결성 검증, 빠른 검색 등에 적합한 여러 해시 알고리즘 구현을 제공합니다.

#### 지원 알고리즘

- **MD5** (`Md5Helper`): 128비트 해시 값. 솔트 지원
- **SHA 시리즈**:
    - SHA-1 (`Sha1Helper`): 160비트 해시값
    - SHA-256 (`Sha256Helper`): 256비트 해시값
    - SHA-512 (`Sha512Helper`): 512비트 해시값
- **HMAC-SHA256** (`HmacSha256Helper`): 키 기반 메시지 인증 코드
- **CRC 체크섬** (`CrcHelper`): CRC32/CRC64 循环冗余校验
- **MurmurHash3** (`MurmurHash3Helper`): 고성능 비암호화 해시
- **xxHash** (`XxHashHelper`): 초고성능 해시 알고리즘. 32/64/128비트 지원

#### 사용 예시

```csharp
// MD5 해시
string md5Hash = Md5Helper.Hash("Hello World");
string saltedHash = Md5Helper.HashWithSalt("Hello World", "salt");

// SHA-256 해시
string sha256Hash = Sha256Helper.ComputeHash("Hello World");

// HMAC-SHA256
string hmacHash = HmacSha256Helper.Hash("message", "secret-key");

// xxHash (고성능)
ulong xxHash = XxHashHelper.Hash64("Hello World");
```

### 🌐 HTTP 도구

#### HTTP 확장 (GameFrameX.Foundation.Http.Extension)

HttpClient의 편리한 확장 메서드로 JSON 데이터 송수신을 간소화합니다.

```csharp
// POST JSON 요청
string response = await httpClient.PostJsonToStringAsync<MyClass>(url, myObject);
```

#### HTTP 메시지 표준화 (GameFrameX.Foundation.Http.Normalization)

`code`, `message`, `data` 필드를 포함하는 통일된 HTTP 응답 형식을 제공합니다.

### 📄 JSON 직렬화 (GameFrameX.Foundation.Json)

`System.Text.Json` 기반의 고성능 직렬화 도구로 최적화된 기본 설정을 제공합니다.

#### 특징

- 고성능 직렬화/역직렬화
- 열거형을 문자열로 직렬화
- null 값 속성 무시
- 순환 참조 무시
- 속성 이름 대소문자 구분 안 함
- 포맷된 출력과 컴팩트 출력 두 가지 모드 제공

#### 사용 예시

```csharp
// 직렬화
string json = JsonHelper.Serialize(myObject);
string formattedJson = JsonHelper.Serialize(myObject, JsonHelper.FormatOptions);

// 역직렬화
MyClass obj = JsonHelper.Deserialize<MyClass>(json);

// 안전한 역직렬화
if (JsonHelper.TryDeserialize<MyClass>(json, out var result))
{
    // 처리 결과
}
```

### 🌐 현지화 프레임워크 (GameFrameX.Foundation.Localization)

가볍고 고성능인 현지화 솔루션. 제로 설정 사용과 지연 로딩을 지원하며 GameFrameX.Foundation 생태계 전체의 통일된 현지화를 제공합니다.

#### 주요 특징

- **제로 설정 사용**: 초기화 설정 없이 현지화 리소스 자동 발견 및 로드
- **지연 로드 메커니즘**: 처음 사용할 때만 리소스 로드, 우수한 시작 성능
- **다국어 지원**: 중국어（간체）및 영어 지원 내장, 더 많은 언어로 확장 가능
- **스레드 안전**: 동시 접근 지원, 멀티스레드 환경에 적합
- **높은 확장성**: 사용자 정의 리소스 프로바이더 지원, 유연한 우선순위 관리
- **우선순위 해석**: 사용자 정의 프로바이더 > 어셈블리 리소스 > 기본 리소스

#### 핵심 컴포넌트

| 컴포넌트 | 파일 | 설명 |
|------------|-------------------------------|---------------------|
| **현지화 서비스**  | `LocalizationService.cs`      | 통합 현지화 진입점, 정적 메서드 API 제공 |
| **리소스 매니저**  | `ResourceManager.cs`          | 여러 리소스 프로바이더 관리, 우선순위 해석 구현 |
| **기본 프로바이더**  | `DefaultResourceProvider.cs`  | 提供英文默认消息，包含50+常用消息  |
| **어셈블리 프로바이더** | `AssemblyResourceProvider.cs` | .resx 파일에서 현지화 리소스 로드 |

#### 기본 사용법

```csharp
using GameFrameX.Foundation.Localization.Core;

// 간단한 현지화 문자열 가져오기
var successMessage = LocalizationService.GetString("Success");
Console.WriteLine(successMessage); // 현재 문화권에 따라 표시 "Success" 또는 "성공"

// 매개변수가 있는 형식화된 메시지
var errorMessage = LocalizationService.GetString("ArgumentNull", "username");
Console.WriteLine(errorMessage); // "Value cannot be null. (Parameter 'username')"

// 키가 없으면 키 이름 자체를 반환
var unknown = LocalizationService.GetString("Some.Unknown.Key");
Console.WriteLine(unknown); // 출력: "Some.Unknown.Key"
```

#### 예외 처리에서의 현지화

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

        // 기타 검증 로직...
    }
}
```

#### 모듈 현지화 통합

##### 1. 현지화 키 정의

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

##### 2. 리소스 파일 생성

프로젝트에 `Localization/Messages/Resources.resx` 및 `Localization/Messages/Resources.zh-CN.resx` 생성：

```xml
<!-- Resources.resx (기본 영어) -->
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
<!-- Resources.zh-CN.resx (중국어) -->
<root>
  <data name="YourModule.Validation.EmailRequired" xml:space="preserve">
    <value>이메일 주소는 필수 항목입니다</value>
  </data>
  <data name="YourModule.Messages.UserCreated" xml:space="preserve">
    <value>사용자 '{0}' 已成功创建</value>
  </data>
</root>
```

##### 3. 비즈니스 로직에서 사용

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

        // 사용자 생성 로직...

        var successMessage = LocalizationService.GetString(
            LocalizationKeys.Messages.UserCreated, userDto.Username);
        Console.WriteLine(successMessage);
    }
}
```

#### 커스텀 리소스 프로바이더

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

// 사용자 정의 프로바이더 등록（최고 우선순위）
var dbProvider = new DatabaseResourceProvider(yourDbConnection);
LocalizationService.RegisterProvider(dbProvider);
```

#### 사전 로드 및 성능 최적화

```csharp
// 애플리케이션 시작 시 모든 현지화 리소스 사전 로드（선택 사항）
LocalizationService.EnsureLoaded();

// 현지화 시스템 통계 정보 가져오기
var stats = LocalizationService.GetStatistics();
Console.WriteLine($"프로바이더 로드됨: {stats.ProvidersLoaded}");
Console.WriteLine($"총 프로바이더 수: {stats.TotalProviderCount}");
Console.WriteLine($"어셈블리 프로바이더 수: {stats.AssemblyProviderCount}");

// 모든 프로바이더 정보 가져오기
var providers = LocalizationService.GetProviders();
foreach (var provider in providers)
{
    Console.WriteLine($"프로바이더: {provider.GetType().Name}");
}
```

#### 리소스 명명 규칙

- **模式**: `{모듈名}.{类别}.{具体键名}`
- **예시**:
    - `Utility.Exceptions.TimestampOutOfRange`
    - `Encryption.InvalidKeySize`
    - `Authentication.UserNotFound`
    - `Success`
    - `ArgumentNull`

#### 통합된 모듈

현재 다음 모듈의 현지화 통합이 완료되었습니다：

| 모듈                               | 현지화 키 수 | 상태   |
|----------------------------------|--------|------|
| GameFrameX.Foundation.Utility    | 4      | ✅ 완료 |
| GameFrameX.Foundation.Encryption | 20+    | ✅ 완료 |
| GameFrameX.Foundation.Extensions | 7      | ✅ 완료 |
| GameFrameX.Foundation.Hash       | 2      | ✅ 완료 |

#### 고급 기능

##### 동적 언어 전환

```csharp
public void SwitchLanguage(string cultureCode)
{
    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
    Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureCode);

    // 선택 사항: 새 언어의 리소스 사전 로드
    LocalizationService.EnsureLoaded();
}
```

##### 모니터링 및 진단

```csharp
public class LocalizationDiagnostics
{
    public void PrintStatus()
    {
        var stats = LocalizationService.GetStatistics();
        Console.WriteLine("=== 本地化系统상태 ===");
        Console.WriteLine($"프로바이더 로드됨: {stats.ProvidersLoaded}");
        Console.WriteLine($"총 프로바이더 수: {stats.TotalProviderCount}");

        var providers = LocalizationService.GetProviders();
        foreach (var provider in providers)
        {
            Console.WriteLine($"- {provider.GetType().Name}");
        }
    }
}
```

#### 모범 사례

1. **키 명명 규칙**: \`{모듈명}.{카테고리}.{특정키명}\` 명명 패턴 사용
2. **매개변수화된 메시지**: `string.Format` 형식으로 매개변수 교체 지원
3. **예외 처리**: 예외 메시지에 현지화 지원 통합
4. **성능 최적화**: 애플리케이션 시작 시 리소스 사전 로드 선택 가능
5. **테스트 검증**: 현지화 기능을 위한 단위 테스트 작성

#### 프로젝트 파일 설정

프로젝트 파일에 현지화 리소스가 포함되어 있는지 확인:

```xml
<PropertyGroup>
  <EnableDefaultEmbeddedResourceItems>false</EnableDefaultEmbeddedResourceItems>
</PropertyGroup>

<ItemGroup>
  <EmbeddedResource Include="Localization\Messages\*.resx" />
</ItemGroup>
```

자세한 내용은 다음을 참조:

- [현지화 프레임워크完整文档](GameFrameX.Foundation.Localization/README.Localization.md)
- [사용 예시 및 모범 사례](GameFrameX.Foundation.Localization/USAGE_EXAMPLES.md)

### �️ ORM 엔티티 기본 (GameFrameX.Foundation.Orm.Entity)

ORM 엔티티 기본 클래스와 인터페이스 정의. 감사 추적, 소프트 삭제, 낙관적 잠금 등 엔터프라이즈 기능을 지원합니다.

#### 핵심 컴포넌트 개요

| 컴포넌트 | 파일명 | 주요 기능 |
|--------------|-----------------------|-------------------------------|
| **엔티티 기본 클래스**     | `EntityBase.cs`       | 완전한 기능의 엔티티 기본 클래스, ID, 감사, 소프트 삭제, 버전 관리 등 포함 |
| **엔티티 기본 클래스(제네릭)** | `EntityBaseId.cs`     | 사용자 정의 기본 키 타입을 지원하는 엔티티 기본 클래스 |
| **엔티티 인터페이스**     | `IEntity.cs`          | 基础엔티티接口定义，提供ID属性               |
| **감사 인터페이스**     | `IAuditableEntity.cs` | 감사 기능 인터페이스, 생성 시간, 업데이트 시간, 작업 사용자 등 감사 필드 정의 |

#### 엔티티 기본 클래스 기능

```csharp
using GameFrameX.Foundation.Orm.Entity;

// EntityBase를 상속한 엔티티 클래스는 자동으로 완전한 엔터프라이즈 기능을 획득
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // 다음 속성은 EntityBase에서 제공：
    // - long Id                    // 기본 키 ID
    // - DateTime CreateTime        // 생성 시간
    // - DateTime UpdateTime        // 업데이트 시간
    // - long CreateUserId          // 创建사용자ID
    // - long UpdateUserId          // 更新사용자ID
    // - string CreateUserName      // 创建사용자名
    // - string UpdateUserName      // 更新사용자名
    // - bool IsDelete              // 소프트 삭제 플래그
    // - long Version               // 낙관적 잠금 버전 번호
    // - bool IsEnabled             // 启用상태
}

// 사용 예시
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

#### 커스텀 기본 키 형식

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 문자열을 기본 키로 사용
public class Product : EntityBaseId<string>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    
    // Id 속성 타입은 string, EntityBaseId<string>에서 제공
}

// Guid를 기본 키로 사용
public class Order : EntityBaseId<Guid>
{
    public string OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    
    // Id 속성 타입은 Guid, EntityBaseId<Guid>에서 제공
}

// 사용 예시
var product = new Product
{
    Id = "PROD-001",
    Name = "노트북",
    Price = 5999.99m,
    Description = "고성능笔记本电脑"
};

var order = new Order
{
    Id = Guid.NewGuid(),
    OrderNumber = "ORD-20240101-001",
    TotalAmount = 5999.99m,
    OrderDate = DateTime.UtcNow
};
```

#### 인터페이스 구현

```csharp
using GameFrameX.Foundation.Orm.Entity;

// 기본 엔티티 인터페이스 구현
public class Category : IEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

// 감사 인터페이스 구현
public class AuditableCategory : IEntity<int>, IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    // IAuditableEntity 인터페이스가 요구하는 속성
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public long CreateUserId { get; set; }
    public long UpdateUserId { get; set; }
    public string CreateUserName { get; set; }
    public string UpdateUserName { get; set; }
}
```

#### 엔터프라이즈 기능 상세

##### 1. 감사 추적 (Audit Trail)

```csharp
// EntityBase는 자동으로 감사 필드를 제공
public class Document : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
}

// 비즈니스 로직에서 감사 정보 설정
var document = new Document
{
    Title = "중요 문서",
    Content = "문서 내용...",
    CreateTime = DateTime.UtcNow,
    CreateUserId = currentUser.Id,
    CreateUserName = currentUser.Username,
    UpdateTime = DateTime.UtcNow,
    UpdateUserId = currentUser.Id,
    UpdateUserName = currentUser.Username
};

// 업데이트 시 감사 정보 자동 유지
document.Content = "업데이트된 내용";
document.UpdateTime = DateTime.UtcNow;
document.UpdateUserId = currentUser.Id;
document.UpdateUserName = currentUser.Username;
document.Version++; // 낙관적 잠금 버전 증가
```

##### 2. 소프트 삭제 (Soft Delete)

```csharp
// 소프트 삭제: 레코드를 실제로 삭제하지 않고 삭제됨으로 표시
public void SoftDeleteUser(User user)
{
    user.IsDelete = true;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    // 데이터베이스에 저장, 레코드는 존재하지만 삭제됨으로 표시
    dbContext.SaveChanges();
}

// 쿼리 시 삭제된 레코드 필터링
var activeUsers = dbContext.Users
    .Where(u => !u.IsDelete)
    .ToList();

// 삭제된 레코드 복원
public void RestoreUser(User user)
{
    user.IsDelete = false;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    
    dbContext.SaveChanges();
}
```

##### 3. 낙관적 잠금 (Optimistic Locking)

```csharp
// Version 필드로 낙관적 잠금 구현
public void UpdateUserWithOptimisticLock(long userId, string newEmail)
{
    var user = dbContext.Users.Find(userId);
    if (user == null) throw new EntityNotFoundException();
    
    var originalVersion = user.Version;
    
    // 데이터 수정
    user.Email = newEmail;
    user.UpdateTime = DateTime.UtcNow;
    user.UpdateUserId = currentUser.Id;
    user.UpdateUserName = currentUser.Username;
    user.Version++; // 버전 번호 증가
    
    try
    {
        // 저장 시 버전 번호 확인
        var rowsAffected = dbContext.Database.ExecuteSqlRaw(
            "UPDATE Users SET Email = {0}, UpdateTime = {1}, UpdateUserId = {2}, UpdateUserName = {3}, Version = {4} " +
            "WHERE Id = {5} AND Version = {6}",
            user.Email, user.UpdateTime, user.UpdateUserId, user.UpdateUserName, user.Version, user.Id, originalVersion);
            
        if (rowsAffected == 0)
        {
            throw new ConcurrencyException("데이터가 다른 사용자에 의해 수정되었습니다. 새로고침 후 재시도하세요");
        }
    }
    catch (DbUpdateConcurrencyException)
    {
        throw new ConcurrencyException("동시성 충돌, 새로고침 후 재시도하세요");
    }
}
```

##### 4. 활성/비활성 상태 관리

```csharp
// IsEnabled 필드로 엔티티의 활성화 상태 관리
public class Feature : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    // IsEnabled는 EntityBase에서 제공
}

// 활성화/비활성화 기능
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

// 활성화된 기능 쿼리
var enabledFeatures = dbContext.Features
    .Where(f => f.IsEnabled && !f.IsDelete)
    .ToList();
```

#### 전체 사용 예시

```csharp
using GameFrameX.Foundation.Orm.Entity;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Entities
{
    // 사용자 엔티티
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLoginTime { get; set; }
        
        // 탐색 속성
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
    
    // 주문 엔티티（Guid 기본 키 사용）
    public class Order : EntityBaseId<Guid>
    {
        public string OrderNumber { get; set; }
        public long UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        
        // 탐색 속성
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 주문 항목 엔티티
    public class OrderItem : EntityBase
    {
        public Guid OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        
        // 탐색 속성
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
    
    // 제품 엔티티（문자열 기본 키 사용）
    public class Product : EntityBaseId<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryId { get; set; }
        
        // 탐색 속성
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
    
    // 분류 엔티티（인터페이스 구현）
    public class Category : IEntity<string>, IAuditableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentId { get; set; }
        
        // IAuditableEntity 인터페이스 속성
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
        
        // 탐색 속성
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

// 비즈니스 서비스 예시
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
                throw new EntityNotFoundException($"사용자 {userId} 존재하지 않습니다");
            
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var originalVersion = user.Version;
            
            // 필드 업데이트
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
                throw new ConcurrencyException("사용자 정보가 다른 사용자에 의해 수정되었습니다. 새로고침 후 재시도하세요");
            }
        }
        
        public async Task SoftDeleteUserAsync(long userId)
        {
            var user = await _context.Users
                .Where(u => u.Id == userId && !u.IsDelete)
                .FirstOrDefaultAsync();
                
            if (user == null)
                throw new EntityNotFoundException($"사용자 {userId} 존재하지 않습니다");
            
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
            // 비밀번호 해시 로직 구현
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

### 🏷️ ORM 속성 태그 (GameFrameX.Foundation.Orm.Attribute)

ORM 프레임워크의 속성 태그를 제공하여 감사 추적, 캐시 전략, 소프트 삭제, 버전 관리 등 엔티티 클래스의 특수 기능을 식별합니다.

#### 핵심 컴포넌트 개요

| 컴포넌트 | 파일명 | 주요 기능 |
|--------------|------------------------|-----------------------------------------|
| **감사 테이블 속성**    | `AuditTableAttribute.cs` | 엔티티 클래스의 감사 추적 기능 표시, 데이터 변경 이력 기록 |
| **캐시 테이블 속성**    | `CacheTableAttribute.cs` | 엔티티 클래스의 캐시 전략 표시, 데이터 접근 성능 향상 |
| **소프트 삭제 속성**    | `SoftDeleteAttribute.cs` | 엔티티 클래스의 소프트 삭제 기능 표시, 물리적 삭제 대신 논리적 삭제 |
| **버전 관리 속성**   | `VersionControlAttribute.cs` | 엔티티 클래스의 데이터 버전 관리 표시, 낙관적 잠금 및 동시성 제어 구현 |

#### 감사 테이블 속성 (AuditTableAttribute)

감사 추적이 필요한 엔티티 클래스를 표시하며, 시스템이 자동으로 데이터의 생성, 수정, 삭제 등 작업 이력을 기록합니다.

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 사용자 테이블의 감사 추적 표시
[AuditTable]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase에 감사 필드가 포함됨：
    // CreateTime, UpdateTime, CreateUserId, UpdateUserId, 
    // CreateUserName, UpdateUserName
}

// 주문 테이블의 감사 추적 표시
[AuditTable]
public class Order : EntityBase
{
    public string OrderNumber { get; set; }
    public long UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
}

// 감사 인터셉터 예시
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

#### 캐시 테이블 속성 (CacheTableAttribute)

캐시 전략을 지원하는 엔티티 클래스를 표시하며, 시스템이 자동으로 이러한 테이블의 데이터를 캐시 관리합니다.

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 설정 테이블의 캐시 표시（변경 빈도가 낮아 캐시에 적합）
[CacheTable]
public class SystemConfig : EntityBase
{
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

// 사전 테이블의 캐시 지원 표시（사전 데이터는 비교적 안정적, 캐시에 적합）
[CacheTable]
public class Dictionary : EntityBase
{
    public string DictType { get; set; }
    public string DictKey { get; set; }
    public string DictValue { get; set; }
    public string Description { get; set; }
    public int SortOrder { get; set; }
}

// 권한 테이블의 캐시 표시（접근 빈도가 높지만 변경은 적음）
[CacheTable]
public class Permission : EntityBase
{
    public string PermissionCode { get; set; }
    public string PermissionName { get; set; }
    public string Description { get; set; }
    public string Module { get; set; }
}

// 캐시 서비스 예시
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
            // 캐시 미지원, 데이터베이스에서 직접 쿼리
            return await _dbContext.Set<T>().ToListAsync();
        }
        
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        if (_memoryCache.TryGetValue(cacheKey, out List<T> cachedData))
        {
            _logger.LogDebug($"캐시에서 데이터 가져오기: {cacheKey}");
            return cachedData;
        }
        
        // 데이터베이스에서 쿼리 후 캐시
        var data = await _dbContext.Set<T>().ToListAsync();
        
        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30), // 30분 후 만료
            SlidingExpiration = TimeSpan.FromMinutes(5), // 5분 슬라이딩 만료
            Priority = CacheItemPriority.Normal
        };
        
        _memoryCache.Set(cacheKey, data, cacheOptions);
        _logger.LogDebug($"데이터 캐시됨: {cacheKey}, 레코드 수: {data.Count}");
        
        return data;
    }
    
    public async Task InvalidateCacheAsync()
    {
        var entityType = typeof(T);
        var cacheKey = $"CacheTable_{entityType.Name}_All";
        
        _memoryCache.Remove(cacheKey);
        _logger.LogDebug($"캐시 만료됨: {cacheKey}");
    }
}

// 캐시 관리자 예시
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
                    
                    _logger.LogInformation($"캐시 테이블 {type.Name} 리프레시 완료");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"캐시 테이블 {type.Name} 리프레시 중 오류 발생");
            }
        }
    }
}
```

#### 소프트 삭제 속성 (SoftDeleteAttribute)

소프트 삭제 기능을 지원하는 엔티티 클래스를 표시하며, 삭제 작업은 레코드를 물리적 삭제 대신 삭제됨으로 표시합니다.

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 사용자 테이블의 소프트 삭제 표시
[SoftDelete]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase에 IsDelete 필드가 포함됨
}

// 게시글 테이블의 소프트 삭제 표시
[SoftDelete]
public class Article : EntityBase
{
    public string Title { get; set; }
    public string Content { get; set; }
    public long AuthorId { get; set; }
    public DateTime PublishTime { get; set; }
}

// 소프트 삭제 인터셉터
public class SoftDeleteInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 소프트 삭제 엔티티의 삭제 작업 처리
        var softDeleteEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && 
                       e.Entity.GetType().GetCustomAttribute<SoftDeleteAttribute>() != null)
            .ToList();
            
        foreach (var entry in softDeleteEntries)
        {
            // 삭제 작업을 업데이트 작업으로 변환
            entry.State = EntityState.Modified;
            
            if (entry.Entity is EntityBase entityBase)
            {
                entityBase.IsDelete = true;
                entityBase.UpdateTime = DateTime.UtcNow;
                // 업데이트 사용자 정보 설정...
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 소프트 삭제 쿼리 필터
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
        // 삭제된 레코드를 포함한 쿼리 반환
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
        
        return query.Where(_ => false); // 소프트 삭제 미지원 시 빈 결과 반환
    }
}

// 사용 예시
public class UserService
{
    private readonly ApplicationDbContext _context;
    
    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // 활성 사용자 가져오기（삭제된 항목 자동 필터링）
    public async Task<List<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .WhereNotDeleted()
            .ToListAsync();
    }
    
    // 삭제된 사용자 가져오기
    public async Task<List<User>> GetDeletedUsersAsync()
    {
        return await _context.Users
            .OnlyDeleted()
            .ToListAsync();
    }
    
    // 모든 사용자 가져오기（삭제된 항목 포함）
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .IncludeDeleted()
            .ToListAsync();
    }
    
    // 사용자 소프트 삭제
    public async Task SoftDeleteUserAsync(long userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            _context.Users.Remove(user); // 인터셉터에 의해 소프트 삭제로 변환됨
            await _context.SaveChangesAsync();
        }
    }
    
    // 삭제된 사용자 복원
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

#### 버전 관리 속성 (VersionControlAttribute)

데이터 버전 관리를 지원하는 엔티티 클래스를 표시하며, 낙관적 잠금과 동시성 제어 기능을 구현합니다.

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;

// 사용자 테이블의 버전 관리 표시
[VersionControl]
public class User : EntityBase
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    // EntityBase에 Version 필드가 포함됨
}

// 재고 테이블의 버전 관리 표시（초과 판매 방지）
[VersionControl]
public class Inventory : EntityBase
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public int ReservedQuantity { get; set; }
    public decimal UnitCost { get; set; }
}

// 标记账户余额表支持版本控制（防止并发연산导致余额错误）
[VersionControl]
public class AccountBalance : EntityBase
{
    public long UserId { get; set; }
    public decimal Balance { get; set; }
    public decimal FrozenAmount { get; set; }
    public string Currency { get; set; }
}

// 버전 관리 인터셉터
public class VersionControlInterceptor : IDbCommandInterceptor
{
    public override InterceptionResult<int> NonQueryExecuting(
        DbCommand command, 
        CommandEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context;
        
        // 버전 관리 엔티티의 업데이트 작업 처리
        var versionControlEntries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified && 
                       e.Entity.GetType().GetCustomAttribute<VersionControlAttribute>() != null)
            .ToList();
            
        foreach (var entry in versionControlEntries)
        {
            if (entry.Entity is EntityBase entityBase)
            {
                // 버전 번호 자동 증가
                entityBase.Version++;
                
                // Version 필드를 수정됨으로 표시
                entry.Property(nameof(EntityBase.Version)).IsModified = true;
            }
        }
        
        return base.NonQueryExecuting(command, eventData, result);
    }
}

// 버전 관리 서비스
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
            throw new InvalidOperationException($"엔티티类型 {entityType.Name} 未标记 VersionControlAttribute");
        }
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var entity = await _dbContext.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    throw new EntityNotFoundException($"엔티티 {entityType.Name} (ID: {id}) 존재하지 않습니다");
                }
                
                var originalVersion = entity.Version;
                
                // 执行更新연산
                updateAction(entity);
                
                // 업데이트 시간 설정
                entity.UpdateTime = DateTime.UtcNow;
                
                // 변경사항 저장
                await _dbContext.SaveChangesAsync();
                
                _logger.LogDebug($"엔티티 {entityType.Name} (ID: {id}) 업데이트 성공, 버전 {originalVersion} 에서 업데이트 {entity.Version}");
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning($"엔티티 {entityType.Name} (ID: {id}) 버전 충돌，第 {attempt} 次重试");
                
                if (attempt == maxRetries)
                {
                    throw new ConcurrencyException($"엔티티 {entityType.Name} (ID: {id}) 에서 {maxRetries}회 재시도 후에도 버전 충돌이 존재", ex);
                }
                
                // 최신 버전을 가져오기 위해 엔티티 다시 로드
                _dbContext.Entry(await _dbContext.Set<T>().FindAsync(id)).Reload();
                
                // 일정 시간 대기 후 재시도
                await Task.Delay(TimeSpan.FromMilliseconds(100 * attempt));
            }
        }
        
        throw new InvalidOperationException("여기에 도달하면 안 됨");
    }
}

// 사용 예시
public class InventoryService
{
    private readonly VersionControlService<Inventory> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public InventoryService(VersionControlService<Inventory> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 재고 감소（초과 판매 방지）
    public async Task<bool> ReduceInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"产品 {productId} 의 재고 레코드가 존재하지 않습니다");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
            {
                if (inv.Quantity < quantity)
                {
                    throw new InsufficientInventoryException($"재고 부족, 현재 재고: {inv.Quantity}，필요: {quantity}");
                }
                
                inv.Quantity -= quantity;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            // 버전 충돌, 동시 연산이 원인일 수 있음
            throw new ConcurrencyException("재고 업데이트 실패, 재시도하세요");
        }
    }
    
    // 재고 증가
    public async Task AddInventoryAsync(string productId, int quantity)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.ProductId == productId);
            
        if (inventory == null)
        {
            throw new EntityNotFoundException($"产品 {productId} 의 재고 레코드가 존재하지 않습니다");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(inventory.Id, inv =>
        {
            inv.Quantity += quantity;
        });
    }
}

// 계정 잔액 서비스 예시
public class AccountBalanceService
{
    private readonly VersionControlService<AccountBalance> _versionControlService;
    private readonly ApplicationDbContext _context;
    
    public AccountBalanceService(VersionControlService<AccountBalance> versionControlService, ApplicationDbContext context)
    {
        _versionControlService = versionControlService;
        _context = context;
    }
    
    // 잔액 차감
    public async Task<bool> DeductBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"사용자 {userId} 의 {currency} 계정이 존재하지 않습니다");
        }
        
        try
        {
            await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
            {
                if (bal.Balance < amount)
                {
                    throw new InsufficientBalanceException($"잔액 부족, 현재 잔액: {bal.Balance}，필요: {amount}");
                }
                
                bal.Balance -= amount;
            });
            
            return true;
        }
        catch (ConcurrencyException)
        {
            throw new ConcurrencyException("잔액 업데이트 실패, 재시도하세요");
        }
    }
    
    // 잔액 증가
    public async Task AddBalanceAsync(long userId, decimal amount, string currency = "CNY")
    {
        var balance = await _context.AccountBalances
            .FirstOrDefaultAsync(b => b.UserId == userId && b.Currency == currency);
            
        if (balance == null)
        {
            throw new EntityNotFoundException($"사용자 {userId} 의 {currency} 계정이 존재하지 않습니다");
        }
        
        await _versionControlService.UpdateWithVersionCheckAsync(balance.Id, bal =>
        {
            bal.Balance += amount;
        });
    }
}
```

#### 전체 통합 예시

```csharp
using GameFrameX.Foundation.Orm.Attribute;
using GameFrameX.Foundation.Orm.Entity;
using Microsoft.EntityFrameworkCore;

namespace MyApplication.Entities
{
    // 사용자 엔티티: 감사, 소프트 삭제, 버전 관리 지원
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
    
    // 시스템 설정: 캐시, 감사 지원
    [CacheTable]
    [AuditTable]
    public class SystemConfig : EntityBase
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
    
    // 재고 레코드: 버전 관리, 감사 지원
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
    
    // 주문 레코드: 감사, 소프트 삭제 지원
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

// DbContext 설정
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
        // SoftDeleteAttribute가 표시된 모든 엔티티에 전역 쿼리 필터 추가
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

// 서비스 등록
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

### 🖊️ 로깅 도구库 (GameFrameX.Foundation.Logger)

Serilog 기반의 로그 설정 도구로, 간단하고 사용하기 쉬운 로그 기록 기능을 제공합니다.

#### 특징

- 여러 로그 레벨 지원 (Debug, Info, Warning, Error, Fatal)
- 유연한 출력 설정
- 커스텀 로그 프로바이더 지원
- 로그 자가 진단 제공
- ✅ **预初始化日志支持**: 수동 초기화 불필요. LogHelper 직접 사용 가능
- ✅ **日志自动合并**: 초기화 전후의 로그가 공식 로그 시스템에 자동 병합

#### 사전 초기화 로그 기능

정식 로그 시스템 초기화 전에 LogHelper를 사용하여 콘솔에 로그를 출력할 수 있습니다. `LogHandler.Create()`로 정식 로그를 초기화하면 이전 임시 로그가 새 로그 시스템에 자동으로 병합되어 로그 손실을 방지합니다.

```csharp
class Program
{
    static void Main(string[] args)
    {
        // 초기화 없이 LogHelper를 직접 사용 가능
        LogHelper.Info("설정 로드 중...");
        LogHelper.Debug("매개변수: {Args}", string.Join(", ", args));
        LogHelper.Warning("설정이 없으면 기본값 사용");

        // 정식 로그 시스템 초기화
        var logger = LogHandler.Create(options);

        // 이전 임시 로그가 새 로그 시스템에 자동 병합됨
        LogHelper.Info("시스템 시작 완료");
    }
}
```

#### 사용 예시

```csharp
// 로그 초기화
LogHandler.Create(LogOptions.Default);

// 로그 기록
LogHelper.Debug("디버그 정보");
LogHelper.Info("일반 메시지");
LogHelper.Warning("경고 메시지");
LogHelper.Error("오류 메시지");
LogHelper.Fatal("치명적 오류");
```

### ⚙️ 命令行매개변수 처리 (GameFrameX.Foundation.Options)

강력한 명령줄 매개변수 및 환경 변수 파싱 라이브러리로, 명령줄 매개변수와 환경 변수를 강타입 설정 객체에 자동 매핑합니다.

#### 특징

- ✅ **매개변수 우선순위 처리**: 命令行参数 > 环境变量 > 默认值
- ✅ **제네릭 지원**: 支持任意强类型配置类
- ✅ **다양한 시작 방식 호환**: 支持Docker、exe、shell等启动方式
- ✅ **자동 접두사 처리**: 매개변수에 자동으로 `--` 접두사 추가
- ✅ **불리언 매개변수 지원**: 여러 불리언 매개변수 형식 지원
- ✅ **환경 변수 매핑**: 自动映射环境变量到配置属性
- ✅ **형식 변환**: 自动转换字符串参数到目标类型
- ✅ **속성 지원**: 풍부한 설정 속성 지원

#### 핵심 컴포넌트

| 컴포넌트 | 기능 설명 |
|--------------------------------|----------------------|
| `CommandLineArgumentConverter` | 명령줄 매개변수 변환기, 매개변수 처리의 핵심 기능 제공 |
| `OptionsBuilder<T>`            | 설정 빌더, 제네릭 설정 객체 빌드에 사용 |
| `OptionsProvider`              | 설정 프로바이더, 설정 객체의 가져오기 및 관리에 사용 |

#### 빠른 시작

##### 1. 설정 클래스 정의

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

##### 2. OptionsBuilder 사용

```csharp
using GameFrameX.Foundation.Options;

class Program
{
    static void Main(string[] args)
    {
        // 옵션 빌더 생성
        var builder = new OptionsBuilder<AppConfig>(args);
        
        // 설정 객체 빌드
        var config = builder.Build();
        
        // 설정 사용
        Console.WriteLine($"서버: {config.Host}:{config.Port}");
        Console.WriteLine($"디버그 모드: {config.Debug}");
        Console.WriteLine($"로그 레벨: {config.LogLevel}");
        Console.WriteLine($"타임아웃: {config.Timeout}秒");
    }
}
```

#### 사용 방법

##### 명령줄 매개변수

여러 매개변수 형식을 지원：

```bash
# 키-값 형식
myapp.exe --host=example.com --port=9090 --debug=true

# 구분자 형식
myapp.exe --host example.com --port 9090 --debug true

# 불리언 플래그 형식
myapp.exe --host example.com --port 9090 --debug

# 혼합 형식
myapp.exe --host=example.com --port 9090 --debug
```

##### 환경 변수

```bash
# 환경 변수 설정
export HOST=example.com
export PORT=9090
export DEBUG=true

# 프로그램 실행
myapp.exe
```

##### Docker 지원

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0
COPY . /app
WORKDIR /app
ENTRYPOINT ["dotnet", "MyApp.dll"]
```

```bash
# Docker 실행
docker run myapp --host example.com --port 9090 --debug

# 또는 환경 변수 사용
docker run -e HOST=example.com -e PORT=9090 -e DEBUG=true myapp
```

#### 고급 기능

##### 속성을 사용한 설정

```csharp
using GameFrameX.Foundation.Options.Attributes;

public class AdvancedConfig
{
    [Option("h", "host", Required = false, DefaultValue = "localhost")]
    [HelpText("서버 호스트 주소")]
    public string Host { get; set; }

    [Option("p", "port", Required = true)]
    [HelpText("服务器포트号")]
    public int Port { get; set; }

    [FlagOption("d", "debug")]
    [HelpText("디버그 모드 활성화")]
    public bool Debug { get; set; }

    [RequiredOption("api-key", Required = true)]
    [EnvironmentVariable("API_KEY")]
    [HelpText("API 키")]
    public string ApiKey { get; set; }

    [DefaultValue(30.0)]
    public double Timeout { get; set; }
}
```

##### 빌더 옵션

```csharp
var builder = new OptionsBuilder<AppConfig>(
    args: args,
    boolFormat: BoolArgumentFormat.Flag,        // 불리언 매개변수 형식
    ensurePrefixedKeys: true,                   // 매개변수에 접두사가 있는지 확인
    useEnvironmentVariables: true              // 환경 변수 사용
);

var config = builder.Build(skipValidation: false); // 검증 건너뛸지 여부
```

#### 매개변수 우선순위

다음 우선순위로 매개변수가 적용됩니다（높은 우선순위가 낮은 우선순위를 덮어씀）：

1. **명령줄 매개변수** (최고 우선순위)
2. **환경 변수**
3. **기본값** (최저 우선순위)

##### 예시

```csharp
public class Config
{
    public string Host { get; set; } = "localhost";  // 기본값
    public int Port { get; set; } = 8080;           // 기본값
}
```

```bash
# 환경 변수 설정
export HOST=env.example.com
export PORT=7070

# 프로그램 실행（命令行参数覆盖环境变量）
myapp.exe --host cmd.example.com

# 결과：
# Host = "cmd.example.com"  （명령줄 매개변수에서）
# Port = 7070               （환경 변수에서）
```

#### 불리언 매개변수 처리

여러 불리언 매개변수 형식을 지원：

```bash
# 플래그 형식（권장）
myapp.exe --debug                    # debug = true

# 키-값 형식
myapp.exe --debug=true               # debug = true
myapp.exe --debug=false              # debug = false

# 구분자 형식
myapp.exe --debug true               # debug = true
myapp.exe --debug false              # debug = false

# 지원되는 불리언 값
true, false, 1, 0, yes, no, on, off
```

#### 형식 변환

다음 형식 변환을 자동 지원：

- `string` - 그대로 사용
- `int`, `int?` - 정수 변환
- `bool`, `bool?` - 불리언 값 변환
- `double`, `double?` - 배정밀도 부동소수점 변환
- `float`, `float?` - 단정밀도 부동소수점 변환
- `decimal`, `decimal?` - 10진수 변환
- `DateTime`, `DateTime?` - 날짜/시간 변환
- `Guid`, `Guid?` - GUID 변환
- `Enum` - 열거형 변환

##### 예시

```csharp
public class TypedConfig
{
    public int Port { get; set; }
    public bool Debug { get; set; }
    public DateTime StartTime { get; set; }
    public LogLevel Level { get; set; }  // 열거형
}

public enum LogLevel
{
    Debug, Info, Warning, Error
}
```

```bash
myapp.exe --port 9090 --debug true --start-time "2024-01-01 10:00:00" --level Info
```

#### 오류 처리

##### 필수 매개변수 검증

```csharp
public class Config
{
    [RequiredOption("api-key", Required = true)]
    public string ApiKey { get; set; }
}
```

필수 매개변수가 누락되면 `ArgumentException`이 발생합니다：

```
필수 옵션 누락: api-key
```

##### 형식 변환 오류

매개변수 값을 대상 타입으로 변환할 수 없으면 기본값이 사용되고 콘솔에 경고 메시지가 출력됩니다.

#### 모범 사례

##### 1. 설정 클래스 설계

```csharp
public class AppConfig
{
    // 의미 있는 기본값 사용
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8080;
    
    // 불리언 속성의 기본값은 false
    public bool Debug { get; set; } = false;
    
    // 속성을 사용하여 더 많은 정보 제공
    [RequiredOption("database-url", Required = true)]
    [EnvironmentVariable("DATABASE_URL")]
    public string DatabaseUrl { get; set; }
}
```

##### 2. 오류 처리

```csharp
try
{
    var builder = new OptionsBuilder<AppConfig>(args);
    var config = builder.Build();
    
    // 설정 사용启动应用
    StartApplication(config);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"설정 오류: {ex.Message}");
    Environment.Exit(1);
}
```

##### 3. Docker 통합

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = new OptionsBuilder<AppConfig>(args);
        var config = builder.Build();
        
        // Docker에서는 일반적으로 환경 변수 사용
        // 개발에서는 일반적으로 명령줄 매개변수 사용
        
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

#### 전체 예시

```csharp
using GameFrameX.Foundation.Options;
using GameFrameX.Foundation.Options.Attributes;

namespace MyApp
{
    public class ServerConfig
    {
        [Option("h", "host", DefaultValue = "localhost")]
        [EnvironmentVariable("SERVER_HOST")]
        [HelpText("서버 호스트 주소")]
        public string Host { get; set; }

        [Option("p", "port", DefaultValue = 8080)]
        [EnvironmentVariable("SERVER_PORT")]
        [HelpText("服务器포트号")]
        public int Port { get; set; }

        [FlagOption("d", "debug")]
        [EnvironmentVariable("DEBUG")]
        [HelpText("디버그 모드 활성화")]
        public bool Debug { get; set; }

        [RequiredOption("database-url", Required = true)]
        [EnvironmentVariable("DATABASE_URL")]
        [HelpText("데이터베이스 연결 문자열")]
        public string DatabaseUrl { get; set; }

        [Option("timeout", DefaultValue = 30.0)]
        [EnvironmentVariable("REQUEST_TIMEOUT")]
        [HelpText("요청 타임아웃 시간（초）")]
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

                Console.WriteLine("서버 설정:");
                Console.WriteLine($"  호스트: {config.Host}");
                Console.WriteLine($"  포트: {config.Port}");
                Console.WriteLine($"  디버그: {config.Debug}");
                Console.WriteLine($"  데이터베이스: {config.DatabaseUrl}");
                Console.WriteLine($"  타임아웃: {config.Timeout}초");

                // 서버 시작
                StartServer(config);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"설정 오류: {ex.Message}");
                ShowHelp();
                Environment.Exit(1);
            }
        }

        static void StartServer(ServerConfig config)
        {
            // 서버 시작 로직
            Console.WriteLine($"서버 시작 {config.Host}:{config.Port}");
        }

        static void ShowHelp()
        {
            Console.WriteLine("사용법:");
            Console.WriteLine("  myapp.exe --host <호스트> --port <포트> --database-url <데이터베이스 URL> [选项]");
            Console.WriteLine();
            Console.WriteLine("옵션:");
            Console.WriteLine("  -h, --host <호스트>           服务器호스트地址 (默认: localhost)");
            Console.WriteLine("  -p, --port <포트>           服务器포트号 (默认: 8080)");
            Console.WriteLine("  -d, --debug                 디버그 모드 활성화");
            Console.WriteLine("      --database-url <URL>    데이터베이스 연결 문자열 (必需)");
            Console.WriteLine("      --timeout <秒>          요청 타임아웃 시간 (默认: 30.0)");
        }
    }
}
```

#### CommandLineArgumentConverter 사용

OptionsBuilder 외에도 저수준 CommandLineArgumentConverter를 직접 사용할 수 있습니다：

```csharp
using GameFrameX.Foundation.Options;

// 변환기 인스턴스 생성
var converter = new CommandLineArgumentConverter();

// 원본 명령줄 매개변수
var args = new[] { "--port", "8080", "-h", "localhost" };

// 환경 변수 설정（선택 사항）
Environment.SetEnvironmentVariable("APP_NAME", "MyApplication");
Environment.SetEnvironmentVariable("LOG_LEVEL", "debug-mode");

// 표준 형식으로 변환（명령줄 매개변수와 환경 변수 병합）
var standardArgs = converter.ConvertToStandardFormat(args);
// 결과: ["--port", "8080", "-h", "localhost", "--APP_NAME", "MyApplication", "--LOG_LEVEL", "debugmode"]

// 명령줄 문자열로 변환
var commandLineString = converter.ToCommandLineString(standardArgs);
// 결과: "--port 8080 -h localhost --APP_NAME MyApplication --LOG_LEVEL debugmode"

// 모든 환경 변수 가져오기
var envVars = converter.GetEnvironmentVariables();
Console.WriteLine($"감지 {envVars.Count} 개의 환경 변수");
```

##### 불리언 타입 매개변수 지원

`CommandLineArgumentConverter`는 불리언 타입 매개변수의 지능적인 인식과 처리를 지원하며, 세 가지 형식을 제공합니다：

```csharp
using GameFrameX.Foundation.Options;

// 불리언 타입 환경 변수 설정
Environment.SetEnvironmentVariable("ENABLE_LOGGING", "true");
Environment.SetEnvironmentVariable("DEBUG_MODE", "false");
Environment.SetEnvironmentVariable("VERBOSE", "yes");

var converter = new CommandLineArgumentConverter();

// 1. 플래그 형식（기본값）- true 값에만 플래그 추가
converter.BoolFormat = BoolArgumentFormat.Flag;
var flagArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 결과: ["--ENABLE_LOGGING", "--VERBOSE"] （true 값만 포함）

// 2. 키-값 형식 - 키-값 쌍 추가
converter.BoolFormat = BoolArgumentFormat.KeyValue;
var keyValueArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 결과: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]

// 3. 구분자 형식 - 키와 값 분리
converter.BoolFormat = BoolArgumentFormat.Separated;
var separatedArgs = converter.ConvertToStandardFormat(Array.Empty<string>());
// 결과: ["--ENABLE_LOGGING", "true", "--DEBUG_MODE", "false", "--VERBOSE", "true"]
```

지원되는 불리언 값 형식：

- **True 값**: `"true"`, `"1"`, `"yes"`, `"on"`, `"enabled"` (대소문자 구분 없음)
- **False 값**: `"false"`, `"0"`, `"no"`, `"off"`, `"disabled"` (대소문자 구분 없음)

### 🛠️ 유틸리티 클래스 (GameFrameX.Foundation.Utility)

콘솔 작업, 환경 관리, 시간 처리, 스노우플레이크 ID 생성 등의 실용적인 유틸리티 클래스 모음을 제공합니다.

#### 핵심 컴포넌트 개요

| 컴포넌트 | 파일 | 설명 |
|-----------|------------------------|---------------------------|
| **콘솔 헬퍼** | `ConsoleHelper.cs`     | 콘솔 로고 출력 및 형식화된 출력 |
| **환경 헬퍼**  | `EnvironmentHelper.cs` | 환경 변수 관리 및 환경 타입 정의 |
| **타임 헬퍼**  | `TimerHelper.cs`       | Unix 타임스탬프 처리 및 시간 변환 |
| **스노우플레이크 ID**  | `SnowFlakeIdHelper.cs` | 分布式唯一ID生成器（Snowflake알고리즘实现） |

#### 콘솔 헬퍼 기능

```csharp
using GameFrameX.Foundation.Utility;

// 애플리케이션 로고 출력
ConsoleHelper.PrintLogo();
// 형식화된 콘솔 로고 출력（애플리케이션 시작 시 브랜드 표시）
```

#### 환경 관리 기능

```csharp
using GameFrameX.Foundation.Utility;

// 현재 환경 유형 가져오기
string currentEnv = Environments.Development;
Console.WriteLine($"현재 환경: {currentEnv}");

// 환경 판단
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
{
    // 개발 환경 특정 로직
    Console.WriteLine("개발 환경에서 실행");
}
```

#### 시간 처리 기능

```csharp
using GameFrameX.Foundation.Utility;

// Unix 타임스탬프 상수
DateTime epochLocal = TimerHelper.EpochLocal;   // 로컬 시간대의 Unix 에포크 시간
DateTime epochUtc = TimerHelper.EpochUtc;       // UTC 시간대의 Unix 에포크 시간

// 현재 Unix 타임스탬프（초）가져오기
long unixSeconds = TimerHelper.UnixTimeSeconds();
Console.WriteLine($"현재 Unix 타임스탬프（초）: {unixSeconds}");

// 현재 Unix 타임스탬프（밀리초）가져오기
long unixMilliseconds = TimerHelper.UnixTimeMilliseconds();
Console.WriteLine($"현재 Unix 타임스탬프（밀리초）: {unixMilliseconds}");

// 타임스탬프 변환 예시
DateTime currentTime = DateTime.UtcNow;
long timestamp = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
DateTime restored = DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
```

#### 스노우플레이크 ID 생성기

```csharp
using GameFrameX.Foundation.Utility;

// 기본 설정으로 ID 생성
long id1 = SnowFlakeIdHelper.GenerateId();
long id2 = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"생성된 ID: {id1}, {id2}");

// 워커 노드 ID 및 데이터 센터 ID 설정
SnowFlakeIdHelper.WorkId = 1;        // 워커 노드 ID (0-31)
SnowFlakeIdHelper.DataCenterId = 1;  // 데이터 센터 ID (0-31)

// 설정 후 ID 생성
long configuredId = SnowFlakeIdHelper.GenerateId();
Console.WriteLine($"설정 후 ID: {configuredId}");

// 타임스탬프 관련 정보 가져오기
DateTime utcStart = SnowFlakeIdHelper.UtcTimeStart;  // UTC 시작 시간
long epochTime = SnowFlakeIdHelper.EpochTime;        // 에포크 타임스탬프

Console.WriteLine($"스노우플레이크 ID 시작 시간: {utcStart}");
Console.WriteLine($"에포크 타임스탬프: {epochTime}");
```

##### 스노우플레이크 ID 알고리즘 설명

스노우플레이크 ID（Snowflake）는 Twitter가 오픈소스로 공개한 분산 ID 생성 알고리즘으로, 다음과 같은 특징이 있습니다：

- **전역 고유**: 분산 환경에서 ID의 전역 고유성 보장
- **증가 추세**: 생성된 ID는 대략 시간순으로 증가하여 데이터베이스 인덱싱에 유리
- **고성능**: 단일 머신에서 초당 수백만 개의 ID 생성 가능
- **의존성 없음**: 데이터베이스나 다른 외부 시스템에 의존하지 않음

ID 구조（64비트）：

```
0 - 0000000000 0000000000 0000000000 0000000000 0 - 00000 - 00000 - 000000000000
|   |                                             |   |       |       |
|   |<-------------- 41비트 타임스탬프 ---------------->|   |<-5비트->|<-5비트->|<--12비트-->
|                                                 |           |       |
부호 비트(1비트)                                        |      데이터 센터 ID   시퀀스 번호
                                                  |      (5비트)      (12位)
                                               워커 노드 ID
                                                (5비트)
```

- **1비트 부호 비트**: 항상 0
- **41비트 타임스탬프**: 밀리초 정밀도, 약 69년간 사용 가능
- **5비트 데이터 센터 ID**: 32개의 데이터 센터 지원
- **5비트 워커 노드 ID**: 각 데이터 센터당 32개의 워커 노드 지원
- **12비트 시퀀스 번호**: 동일 밀리초 내 4096개의 ID 지원

#### 전체 사용 예시

```csharp
using GameFrameX.Foundation.Utility;

namespace MyApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // 애플리케이션 로고 출력
            ConsoleHelper.PrintLogo();
            
            // 실행 환경 확인
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? Environments.Development;
            Console.WriteLine($"현재 실행 환경: {env}");
            
            // 스노우플레이크 ID 생성기 설정
            SnowFlakeIdHelper.WorkId = 1;
            SnowFlakeIdHelper.DataCenterId = 1;
            
            // 고유 ID 생성
            for (int i = 0; i < 5; i++)
            {
                long id = SnowFlakeIdHelper.GenerateId();
                long timestamp = TimerHelper.UnixTimeMilliseconds();
                
                Console.WriteLine($"ID: {id}, 타임스탬프: {timestamp}");
                
                // ID 변화 관찰을 위한 짧은 지연
                Thread.Sleep(1);
            }
            
            // 시간 처리 예시
            Console.WriteLine($"Unix 에포크 시간(UTC): {TimerHelper.EpochUtc}");
            Console.WriteLine($"Unix 에포크 시간(로컬): {TimerHelper.EpochLocal}");
            Console.WriteLine($"현재 Unix 타임스탬프(초): {TimerHelper.UnixTimeSeconds()}");
            Console.WriteLine($"현재 Unix 타임스탬프(밀리초): {TimerHelper.UnixTimeMilliseconds()}");
        }
    }
}
```

## 🧪 테스트

프로젝트에는 완전한 단위 테스트가 포함되어 있어 코드 품질과 기능 정확성을 보장합니다. 모든 핵심 기능에 해당하는 테스트 케이스가 있으며, 테스트 커버리지는 95% 이상입니다.

### 테스트 커버리지

#### 🧩 확장 메서드库测试 (Extensions)

- **ArgumentAlreadyExceptionTests**: 매개변수 이미 존재 예외 테스트
- **BidirectionalDictionaryTests**: 양방향 딕셔너리 기능 테스트
- **ByteExtensionTests**: 字节数组확장 메서드测试
- **CollectionExtensionsTests**: 集合확장 메서드测试
- **ConcurrentLimitedQueueTests**: 동시성 제한 큐 테스트
- **DisposableConcurrentDictionaryTests**: 삭제 가능 동시성 딕셔너리 테스트
- **DisposableDictionaryTests**: 삭제 가능 딕셔너리 테스트
- **IDictionaryExtensionsTests**: 字典확장 메서드测试
- **IEnumerableExtensionsTests**: 枚举확장 메서드测试
- **ListExtensionsTests**: 列表확장 메서드测试
- **LookupXTests**: 룩업 테이블 기능 테스트
- **NullObjectTests**: Null 객체 패턴 테스트
- **NullableConcurrentDictionaryTests**: Null 허용 동시성 딕셔너리 테스트
- **NullableDictionaryTests**: Null 허용 딕셔너리 테스트
- **ObjectExtensionsTests**: 객체 확장 메서드 테스트
- **ReadOnlySpanExtensionsTests**: 읽기 전용 Span 확장 테스트
- **SequenceReaderExtensionsTests**: 시퀀스 리더 확장 테스트
- **SpanExtensionsTests**: Span확장 메서드测试
- **StringExtensionsTests**: 字符串확장 메서드测试
- **TypeExtensionsTests**: 类型확장 메서드测试

#### 🔐 암호화 도구库测试 (Encryption)

- **AesHelperTests**: AES加密알고리즘测试
- **DsaHelperTests**: DSA 디지털 서명 테스트
- **RsaHelperTests**: RSA加密알고리즘测试
- **Sm2HelperTests**: SM2国密알고리즘测试
- **Sm4HelperTests**: SM4国密알고리즘测试
- **XorHelperTests**: XOR 암호화 테스트

#### 🌐 本地化框架测试 (Localization)

- **LocalizationServiceTests**: 현지화 서비스 핵심 기능 테스트
    - 싱글톤 패턴 검증 테스트
    - 현지화 문자열 가져오기 테스트
    - 매개변수화된 메시지 형식화 테스트
    - 알 수 없는 키 처리 테스트
    - 스레드 안전 동시성 테스트
- **ResourceManagerTests**: 리소스 관리자 테스트
    - 프로바이더 우선순위 테스트
    - 지연 로드 메커니즘 테스트
    - 통계 정보 검증 테스트
- **DefaultResourceProviderTests**: 기본 리소스 프로바이더 테스트
- **AssemblyResourceProviderTests**: 어셈블리 리소스 프로바이더 테스트
    - .resx 파일 로드 테스트
    - 다문화 지원 테스트
    - 리소스 캐시 메커니즘 테스트

#### 🔗 해시 도구库测试 (Hash)

- **CrcHelperTests**: CRC校验알고리즘测试
- **HmacSha256HelperTests**: HMAC-SHA256 테스트
- **Md5HelperTests**: MD5哈希알고리즘测试
- **MurmurHash3HelperTests**: MurmurHash3알고리즘测试
- **Sha1HelperTests**: SHA-1哈希알고리즘测试
- **Sha256HelperTests**: SHA-256哈希알고리즘测试
- **Sha512HelperTests**: SHA-512哈希알고리즘测试
- **XxHashHelperTests**: xxHash고성능哈希测试

#### 🌐 HTTP工具库测试 (Http.Extension)

- **HttpExtensionTests**: HTTP客户端확장 메서드测试

#### ⚙️ 命令行매개변수 처리测试 (Options)

- **CommandLineArgumentConverterTests**: 명령줄 매개변수 변환기 기능 테스트
    - 빈 매개변수 배열 처리 테스트
    - 빈 매개변수 값 처리 테스트
    - 중복 매개변수 감지 테스트
    - 환경 변수 변환 테스트
    - 값 정리 기능 테스트
    - 단일 하이픈 매개변수 변환 테스트
    - 명령줄 문자열 생성 테스트
    - 환경 변수 가져오기 테스트
    - 전체 워크플로 테스트
    - 布尔类型매개변수 처리测试
        - 플래그 형식 불리언 매개변수 테스트
        - 키-값 형식 불리언 매개변수 테스트
        - 구분자 형식 불리언 매개변수 테스트
        - 다양한 불리언 값 형식 파싱 테스트
        - 비불리언 값 처리 테스트
- **OptionsBuilderTests**: 옵션 빌더 기능 테스트
    - 기본 설정 빌드 테스트
    - 속성 설정 테스트
    - 형식 변환测试
    - 검증 기능 테스트
- **OptionsProviderTests**: 옵션 프로바이더 기능 테스트
    - 설정 등록 및 가져오기 테스트
    - 전역 설정 관리 테스트

### 테스트 실행

```bash
# 모든 테스트 실행
dotnet test

# 특정 모듈 테스트 실행
dotnet test --filter "FullyQualifiedName~Extensions"
dotnet test --filter "FullyQualifiedName~Encryption"
dotnet test --filter "FullyQualifiedName~Hash"
dotnet test --filter "FullyQualifiedName~Localization"
dotnet test --filter "FullyQualifiedName~Options"

# 특정 테스트 클래스 실행
dotnet test --filter "ClassName=XxHashHelperTests"
dotnet test --filter "ClassName=StringExtensionsTests"
dotnet test --filter "ClassName=LocalizationServiceTests"
dotnet test --filter "ClassName=CommandLineArgumentConverterTests"

# 테스트 커버리지 보고서 생성
dotnet test --collect:"XPlat Code Coverage"

# 성능 테스트 실행
dotnet test --filter "Category=Performance"
```

### 테스트 특징

- **全面覆盖**: 모든 public 메서드에 테스트 케이스 있음
- **边界测试**: null, 경계값, 예외 케이스 테스트 포함
- **성능 테스트**: 핵심 알고리즘 성능 벤치마크
- **并发测试**: 멀티스레드 환경에서 스레드 안전 컴포넌트의 정확성 검증
- **兼容性测试**: 다양한 .NET 버전에서의 호환성 보장

## 🏗️ 아키텍처

### 설계 원칙

- **고성능**: 모든 컴포넌트가 성능 최적화됨. 고동시성 시나리오에 적합
- **易用性**: 간결한 API 설계로 학습 비용 절감
- **可扩展**: 모듈化设计，支持自定义扩展
- **类型安全**: .NET 형식 시스템을 활용하여 런타임 오류 감소
- **内存友好**: Span<T> 및 Memory<T> 등 최신 .NET 기능으로 메모리 할당 감소

### 의존 관계

```
GameFrameX.Foundation.Extensions (핵심 확장)
├── GameFrameX.Foundation.Encryption (암호화 도구)
├── GameFrameX.Foundation.Hash (해시 도구)
├── GameFrameX.Foundation.Json (JSON 도구)
├── GameFrameX.Foundation.Logger (로깅 도구)
├── GameFrameX.Foundation.Options (매개변수 처리)
├── GameFrameX.Foundation.Http.Extension (HTTP 확장)
└── GameFrameX.Foundation.Http.Normalization (HTTP 표준화)
```

## 🔧 개발 가이드

### 환경 요구사항

- .NET 10.0 이상
- C# 12.0 이상

### 프로젝트 빌드

```bash
# 저장소 클론
git clone https://github.com/GameFrameX/GameFrameX.Foundation.git
cd GameFrameX.Foundation

# 의존성 복원
dotnet restore

# 프로젝트 빌드
dotnet build

# 테스트 실행
dotnet test
```

### 기여 가이드

1. 이 저장소를 포크
2. 기능 브랜치 생성 (`git checkout -b feature/AmazingFeature`)
3. 변경사항 커밋 (`git commit -m 'Add some AmazingFeature'`)
4. 브랜치에 푸시 (`git push origin feature/AmazingFeature`)
5. 풀 리퀘스트 생성

## 📊 성능 벤치마크

### 확장 메서드 성능

| 연산        | 기존 방법  | 확장 메서드  | 성능 향상 |
|-----------|-------|-------|------|
| 문자열 null 확인   | 100ns | 15ns  | 85%  |
| 컬렉션 무작위 요소  | 200ns | 50ns  | 75%  |
| Span 字节연산 | 500ns | 80ns  | 84%  |
| 양방향 딕셔너리 조회    | 150ns | 120ns | 20%  |

### 암호화 알고리즘 성능

| 알고리즘       | 데이터 크기 | 암호화 시간   | 복호화 시간   |
|----------|------|--------|--------|
| AES-256  | 1KB  | 0.05ms | 0.04ms |
| RSA-2048 | 1KB  | 2.1ms  | 0.8ms  |
| SM4      | 1KB  | 0.08ms | 0.07ms |
| XOR      | 1KB  | 0.01ms | 0.01ms |

### 해시 알고리즘 성능

| 알고리즘          | 데이터 크기 | 처리 시간  | 처리량      |
|-------------|------|-------|----------|
| MD5         | 1MB  | 2.1ms | 476MB/s  |
| SHA-256     | 1MB  | 3.8ms | 263MB/s  |
| xxHash64    | 1MB  | 0.8ms | 1.25GB/s |
| MurmurHash3 | 1MB  | 1.2ms | 833MB/s  |

## 📋 시스템 요구사항

- .NET 10.0 이상
- Windows, Linux, macOS 지원

## 🤝 기여

프로젝트 개선을 위한 Issue 및 Pull Request를 환영합니다.

1. 프로젝트를 포크
2. 기능 브랜치 생성 (`git checkout -b feature/AmazingFeature`)
3. 변경사항 커밋 (`git commit -m 'Add some AmazingFeature'`)
4. 브랜치에 푸시 (`git push origin feature/AmazingFeature`)
5. 풀 리퀘스트 생성

## 🤝 커뮤니티 지원

- **이슈**: [GitHub Issues](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- **기능 요청**: [GitHub Discussions](https://github.com/GameFrameX/GameFrameX.Foundation/discussions)
- **문서 기여**: 문서 개선 PR을 환영

## 📄 라이선스

이 프로젝트는 MIT 라이선스를 채택하고 있습니다 - 자세한 내용은 [LICENSE](LICENSE) 파일을 참조하세요.

## 🙏 감사의 말

GameFrameX.Foundation 에 기여해 주신 모든 개발자분들께 감사드립니다!

## 🔗 관련 링크

- [GameFrameX 공식 사이트](https://gameframex.doc.alianblank.com)
- [문서 센터](https://gameframex.doc.alianblank.com)

---

<div align="center">

**[⬆ 맨 위로](#gameframexfoundation)**

Made with ❤️ by GameFrameX Team

</div>
