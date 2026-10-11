<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160" />

# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-blue.svg)](LICENSE)
[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com/ko/server/foundation)

[![Discord](https://img.shields.io/badge/-5865F2?logo=discord&logoColor=white)](https://discord.gg/VDWUjWMDw9)
[![GitHub](https://img.shields.io/badge/-181717?logo=github&logoColor=white)](https://github.com/GameFrameX/gameframex)
[![Bilibili](https://img.shields.io/badge/-00A1D6?logo=bilibili&logoColor=white)](https://www.bilibili.com/video/BV1yrpeepEn7)
[![Gitee](https://img.shields.io/badge/-C71D23?logo=gitee&logoColor=white)](https://gitee.com/GameFrameX/gameframex)

**인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현**

<br />

[문서](https://gameframex.doc.alianblank.com/ko/server/foundation) · [빠른 시작](https://gameframex.doc.alianblank.com/ko/server/foundation) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 프로젝트 개요

GameFrameX.Foundation은 GameFrameX 서버 프로젝트의 횡단 관심사 인프라를 제공하는 .NET 10 클래스 라이브러리의 모듈식 모음으로, 로컬라이제이션, JSON, 옵션 파싱, ORM 어트리뷰트, 확장 기능, 암호화, 해싱, 유틸리티, 로깅, HTTP 헬퍼, 응답 정규화, ORM 기본 엔티티, 멱등성을 다룹니다.

### 기능

| 패키지 | 설명 | 버전 | 다운로드 |
|--------------------------------------------|----------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `GameFrameX.Foundation.Localization`       | 경량 로컬라이제이션 지원                    | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             |
| `GameFrameX.Foundation.Json`               | 통합 JSON 직렬화                      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             |
| `GameFrameX.Foundation.Options`            | CLI 인자 파서                        | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       |
| `GameFrameX.Foundation.Orm.Attribute`      | ORM 어트리뷰트 정의                     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           |
| `GameFrameX.Foundation.Extensions`         | 확장 메서드와 헬퍼                       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 |
| `GameFrameX.Foundation.Encryption`         | 암호화 알고리즘                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 |
| `GameFrameX.Foundation.Hash`               | 해시 알고리즘                          | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             |
| `GameFrameX.Foundation.Utility`            | 범용 유틸리티                          | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       |
| `GameFrameX.Foundation.Logger`             | 통합 로깅                            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         |
| `GameFrameX.Foundation.Logger.MongoDB`     | MongoDB 로깅 싱크                     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.MongoDB?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.MongoDB)     | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.MongoDB?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.MongoDB)     |
| `GameFrameX.Foundation.Logger.GrafanaLoki` | Grafana Loki 로깅 싱크                | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.GrafanaLoki?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.GrafanaLoki) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.GrafanaLoki?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.GrafanaLoki) |
| `GameFrameX.Foundation.Http.Extension`     | HTTP 클라이언트 확장                    | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         |
| `GameFrameX.Foundation.Http.Normalization` | HTTP 응답 정규화                      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) |
| `GameFrameX.Foundation.Orm.Entity`         | ORM 기본 엔티티 클래스                   | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 |
| `GameFrameX.Foundation.Idempotency`        | 멱등성과 이벤트 엔벨로프                    | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Idempotency?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Idempotency?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               |

## 빠른 시작

### 설치

필요한 패키지를 설치합니다:

```bash
dotnet add package GameFrameX.Foundation.Extensions
dotnet add package GameFrameX.Foundation.Encryption
dotnet add package GameFrameX.Foundation.Logger
dotnet add package GameFrameX.Foundation.Logger.MongoDB
dotnet add package GameFrameX.Foundation.Logger.GrafanaLoki
```

## 사용 예시

```csharp
using GameFrameX.Foundation.Extensions;
using GameFrameX.Foundation.Encryption;
using GameFrameX.Foundation.Hash;

// Extension methods
var bytes = "Hello World".ToBytes();
var hex = bytes.ToHexString();

// Encryption
var encrypted = AesHelper.Encrypt(Encoding.UTF8.GetBytes("secret"), key, iv);
var decrypted = AesHelper.Decrypt(encrypted, key, iv);

// Hash
var md5 = Md5Helper.ComputeHash("Hello World");
var sha256 = ShaHelper.ComputeSha256("Hello World");
```

## 아키텍처

설계 원칙과 의존성 그래프는 전체 [아키텍처 문서](https://gameframex.doc.alianblank.com/ko/server/foundation)를 참조하세요.

## 플랫폼 지원

- .NET 8.0 / 9.0 / 10.0
- 지원 OS: Windows, macOS, Linux

## 의존성

| 모듈 | 패키지 | 설명 | 의존성 | 문서 |
|--------------------|--------------------------------------------|---------------------------------------------------------|---------------------|---------------------------------------------------------------------------------------|
| Localization       | `GameFrameX.Foundation.Localization`       | 지연 로딩 메커니즘을 갖춘 경량 로컬라이제이션                               | 의존성 없음              | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/localization)       |
| JSON               | `GameFrameX.Foundation.Json`               | 통합 JSON 직렬화/역직렬화 인터페이스                                  | 의존성 없음              | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/json)               |
| Options            | `GameFrameX.Foundation.Options`            | 커맨드라인 옵션/구성 빌더                                          | 의존성 없음              | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/options)            |
| ORM Attribute      | `GameFrameX.Foundation.Orm.Attribute`      | 감사, 캐시, 소프트 삭제, 멀티 테넌트, 데이터 범위, 민감 정보 마스킹, 트리, 가져오기/내보내기용 ORM 어트리뷰트 정의                                              | 의존성 없음              | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/orm-attribute)      |
| Extensions         | `GameFrameX.Foundation.Extensions`         | 핵심 확장 메서드와 컬렉션 헬퍼                                       | → Localization      | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/extensions)         |
| Encryption         | `GameFrameX.Foundation.Encryption`         | AES, RSA, DSA, SM2/SM4, XOR 암호화                         | → Localization      | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/encryption)         |
| Hash               | `GameFrameX.Foundation.Hash`               | CRC32/64, MD5, SHA, HMAC, MurmurHash3, xxHash           | → Localization      | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/hash)               |
| Utility            | `GameFrameX.Foundation.Utility`            | 콘솔, 환경, 시간, 스노우플레이크 ID 유틸리티                             | → Localization      | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/utility)            |
| Logger             | `GameFrameX.Foundation.Logger`             | Serilog 콘솔/파일 싱크를 갖춘 통합 로깅                              | → Extensions + JSON | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/logger)             |
| Logger.MongoDB     | `GameFrameX.Foundation.Logger.MongoDB`     | Logger용 MongoDB 로깅 싱크                                    | → Logger           | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/logger)             |
| Logger.GrafanaLoki | `GameFrameX.Foundation.Logger.GrafanaLoki` | Logger용 Grafana Loki 로깅 싱크                               | → Logger           | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/logger)             |
| HTTP Extension     | `GameFrameX.Foundation.Http.Extension`     | HTTP 클라이언트 확장 메서드                                       | → JSON              | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/http-extension)     |
| HTTP Normalization | `GameFrameX.Foundation.Http.Normalization` | 표준화된 HTTP JSON 응답 구조                                    | → Logger            | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/http-normalization) |
| ORM Entity         | `GameFrameX.Foundation.Orm.Entity`         | 엔터프라이즈 기능을 갖춘 ORM 기본 엔티티 클래스                            | → Utility           | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/orm-entity)         |
| Idempotency        | `GameFrameX.Foundation.Idempotency`        | 멱등 점유/리플레이 판정과 범용 이벤트 엔벨로프                                           | → Utility           | [문서](https://gameframex.doc.alianblank.com/ko/server/foundation/idempotency)        |

## 문서 및 자료

- [문서](https://gameframex.doc.alianblank.com)
- [GitHub 저장소](https://github.com/GameFrameX/GameFrameX.Foundation)
- [이슈 트래커](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- [GameFrameX 공식 사이트](https://gameframex.doc.alianblank.com)

## 커뮤니티 및 지원

[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/GameFrameX/gameframex)
[![Discord](https://img.shields.io/badge/Discord-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/VDWUjWMDw9)
[<img src="https://cdn.jsdelivr.net/npm/devicon@2/icons/linkedin/linkedin-original.svg" height="28" alt="LinkedIn" />](https://www.linkedin.com/in/alianblank)
[![Reddit](https://img.shields.io/badge/Reddit-FF4500?style=for-the-badge&logo=reddit&logoColor=white)](https://www.reddit.com/r/GameFrameX/)
[![X](https://img.shields.io/badge/X-000000?style=for-the-badge&logo=x&logoColor=white)](https://x.com/alian_blank)
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/channel/UCD9QhSFJ5xZkn5NTSV-DVAw)
[![Bluesky](https://img.shields.io/badge/Bluesky-0285FF?style=for-the-badge&logo=bluesky&logoColor=white)](https://bsky.app/profile/alianblank.bsky.social)
[![Bilibili](https://img.shields.io/badge/Bilibili-00A1D6?style=for-the-badge&logo=bilibili&logoColor=white)](https://www.bilibili.com/video/BV1yrpeepEn7)
[![Gitee](https://img.shields.io/badge/Gitee-C71D23?style=for-the-badge&logo=gitee&logoColor=white)](https://gitee.com/GameFrameX/gameframex)
![QQ](https://img.shields.io/badge/QQ-467608841%2F233840761-EB1923?style=for-the-badge&logo=qq&logoColor=white)

## 변경 로그

GameFrameX.Foundation의 버전 히스토리는 [CHANGELOG.md](CHANGELOG.md)를 참조하세요.

## 라이선스

자세한 내용은 [LICENSE.md](LICENSE) 파일을 참조하세요.

<!--
EN: See [LICENSE.md](LICENSE) for license information.
zh-CN: 详见 [LICENSE.md](LICENSE) 文件。
zh-TW: 詳見 [LICENSE.md](LICENSE) 檔案。
ja: 詳しくは [LICENSE.md](LICENSE) をご参照ください。
ko: 자세한 내용은 [LICENSE.md](LICENSE) 파일을 참조하세요.
-->
