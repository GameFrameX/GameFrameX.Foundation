<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160" />

# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-blue.svg)](LICENSE)
[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com/zh-TW/server/foundation)

[![Discord](https://img.shields.io/badge/-5865F2?logo=discord&logoColor=white)](https://discord.gg/VDWUjWMDw9)
[![GitHub](https://img.shields.io/badge/-181717?logo=github&logoColor=white)](https://github.com/GameFrameX/gameframex)
[![Bilibili](https://img.shields.io/badge/-00A1D6?logo=bilibili&logoColor=white)](https://www.bilibili.com/video/BV1yrpeepEn7)
[![Gitee](https://img.shields.io/badge/-C71D23?logo=gitee&logoColor=white)](https://gitee.com/GameFrameX/gameframex)

**獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使**

<br />

[文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation) · [快速開始](https://gameframex.doc.alianblank.com/zh-TW/server/foundation) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 項目簡介

GameFrameX.Foundation 是一系列 .NET 10 模組化類別庫，為 GameFrameX 伺服器專案提供橫切基礎設施——涵蓋本地化、JSON、選項解析、ORM 屬性、擴充方法、加密、雜湊、工具方法、日誌、HTTP 輔助、回應標準化、ORM 基礎實體以及冪等性等功能。

### 功能特性

| 套件 | 描述 | 版本 | 下載數 |
|--------------------------------------------|----------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `GameFrameX.Foundation.Localization`       | 輕量級本地化支援                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             |
| `GameFrameX.Foundation.Json`               | 統一的 JSON 序列化                     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             |
| `GameFrameX.Foundation.Options`            | 命令列參數解析器                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       |
| `GameFrameX.Foundation.Orm.Attribute`      | ORM 屬性定義                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           |
| `GameFrameX.Foundation.Extensions`         | 擴充方法與輔助類別                        | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 |
| `GameFrameX.Foundation.Encryption`         | 加密演算法                            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 |
| `GameFrameX.Foundation.Hash`               | 雜湊演算法                            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             |
| `GameFrameX.Foundation.Utility`            | 一般工具集                            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       |
| `GameFrameX.Foundation.Logger`             | 統一日誌                             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         |
| `GameFrameX.Foundation.Logger.MongoDB`     | MongoDB 日誌 Sink                   | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.MongoDB?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.MongoDB)     | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.MongoDB?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.MongoDB)     |
| `GameFrameX.Foundation.Logger.GrafanaLoki` | Grafana Loki 日誌 Sink              | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.GrafanaLoki?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.GrafanaLoki) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.GrafanaLoki?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.GrafanaLoki) |
| `GameFrameX.Foundation.Http.Extension`     | HTTP 用戶端擴充                       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         |
| `GameFrameX.Foundation.Http.Normalization` | HTTP 回應標準化                       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) |
| `GameFrameX.Foundation.Orm.Entity`         | ORM 實體基底類別                       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 |
| `GameFrameX.Foundation.Idempotency`        | 冪等性與事件信封                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Idempotency?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Idempotency?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               |

## 快速開始

### 安裝

請安裝您需要的套件：

```bash
dotnet add package GameFrameX.Foundation.Extensions
dotnet add package GameFrameX.Foundation.Encryption
dotnet add package GameFrameX.Foundation.Logger
dotnet add package GameFrameX.Foundation.Logger.MongoDB
dotnet add package GameFrameX.Foundation.Logger.GrafanaLoki
```

## 使用範例

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

## 架構概覽

請參閱完整[架構文件](https://gameframex.doc.alianblank.com/zh-TW/server/foundation)以了解設計原則與依賴關係圖。

## 平台支援

- .NET 8.0 / 9.0 / 10.0
- 支援作業系統：Windows、macOS、Linux

## 依賴

| 模組 | 套件 | 描述 | 依賴 | 文檔 |
|--------------------|--------------------------------------------|---------------------------------------------------------|---------------------|---------------------------------------------------------------------------------------|
| Localization       | `GameFrameX.Foundation.Localization`       | 支援延遲載入機制的輕量級本地化                                         | 無依賴                 | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/localization)       |
| JSON               | `GameFrameX.Foundation.Json`               | 統一的 JSON 序列化/反序列化介面                                     | 無依賴                 | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/json)               |
| Options            | `GameFrameX.Foundation.Options`            | 命令列選項/設定建構器                                             | 無依賴                 | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/options)            |
| ORM Attribute      | `GameFrameX.Foundation.Orm.Attribute`      | 涵蓋稽核、快取、軟刪除、多租戶、資料範圍、敏感遮罩、樹狀、匯入/匯出的 ORM 屬性定義                                                                        | 無依賴                 | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/orm-attribute)      |
| Extensions         | `GameFrameX.Foundation.Extensions`         | 核心擴充方法與集合輔助類別                                           | → Localization      | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/extensions)         |
| Encryption         | `GameFrameX.Foundation.Encryption`         | AES、RSA、DSA、SM2/SM4、XOR 加密                              | → Localization      | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/encryption)         |
| Hash               | `GameFrameX.Foundation.Hash`               | CRC32/64、MD5、SHA、HMAC、MurmurHash3、xxHash                | → Localization      | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/hash)               |
| Utility            | `GameFrameX.Foundation.Utility`            | 主控台、環境、時間、雪花 ID 工具                                      | → Localization      | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/utility)            |
| Logger             | `GameFrameX.Foundation.Logger`             | 統一日誌，內建 Serilog 主控台與檔案 Sink                             | → Extensions + JSON | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/logger)             |
| Logger.MongoDB     | `GameFrameX.Foundation.Logger.MongoDB`     | Logger 的 MongoDB 日誌 Sink                                 | → Logger           | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/logger)             |
| Logger.GrafanaLoki | `GameFrameX.Foundation.Logger.GrafanaLoki` | Logger 的 Grafana Loki 日誌 Sink                            | → Logger           | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/logger)             |
| HTTP Extension     | `GameFrameX.Foundation.Http.Extension`     | HTTP 用戶端擴充方法                                            | → JSON              | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/http-extension)     |
| HTTP Normalization | `GameFrameX.Foundation.Http.Normalization` | 標準化的 HTTP JSON 回應結構                                     | → Logger            | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/http-normalization) |
| ORM Entity         | `GameFrameX.Foundation.Orm.Entity`         | 具備企業級特性的 ORM 實體基底類別                                     | → Utility           | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/orm-entity)         |
| Idempotency        | `GameFrameX.Foundation.Idempotency`        | 冪等佔用/重放判定與通用事件信封                                                     | → Utility           | [文檔](https://gameframex.doc.alianblank.com/zh-TW/server/foundation/idempotency)        |

## 文檔與資源

- [文檔](https://gameframex.doc.alianblank.com)
- [GitHub 倉庫](https://github.com/GameFrameX/GameFrameX.Foundation)
- [問題追蹤](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- [GameFrameX 官方網站](https://gameframex.doc.alianblank.com)

## 社區與支援

![QQ](https://img.shields.io/badge/QQ-467608841%2F233840761-EB1923?style=for-the-badge&logo=qq&logoColor=white)
[![Bilibili](https://img.shields.io/badge/Bilibili-00A1D6?style=for-the-badge&logo=bilibili&logoColor=white)](https://www.bilibili.com/video/BV1yrpeepEn7)
[![Gitee](https://img.shields.io/badge/Gitee-C71D23?style=for-the-badge&logo=gitee&logoColor=white)](https://gitee.com/GameFrameX/gameframex)
[![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/GameFrameX/gameframex)
[![Discord](https://img.shields.io/badge/Discord-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/VDWUjWMDw9)
[<img src="https://cdn.jsdelivr.net/npm/devicon@2/icons/linkedin/linkedin-original.svg" height="28" alt="LinkedIn" />](https://www.linkedin.com/in/alianblank)
[![Reddit](https://img.shields.io/badge/Reddit-FF4500?style=for-the-badge&logo=reddit&logoColor=white)](https://www.reddit.com/r/GameFrameX/)
[![X](https://img.shields.io/badge/X-000000?style=for-the-badge&logo=x&logoColor=white)](https://x.com/alian_blank)
[![YouTube](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/channel/UCD9QhSFJ5xZkn5NTSV-DVAw)
[![Bluesky](https://img.shields.io/badge/Bluesky-0285FF?style=for-the-badge&logo=bluesky&logoColor=white)](https://bsky.app/profile/alianblank.bsky.social)

## 更新日誌

請參閱 [CHANGELOG.md](CHANGELOG.md) 了解 GameFrameX.Foundation 的版本歷程。

## 開源協議

詳見 [LICENSE.md](LICENSE) 檔案。

<!--
EN: See [LICENSE.md](LICENSE) for license information.
zh-CN: 详见 [LICENSE.md](LICENSE) 文件。
zh-TW: 詳見 [LICENSE.md](LICENSE) 檔案。
ja: 詳しくは [LICENSE.md](LICENSE) をご参照ください。
ko: 자세한 내용은 [LICENSE.md](LICENSE) 파일을 참조하세요.
-->
