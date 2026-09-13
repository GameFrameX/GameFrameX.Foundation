<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160" />

# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-blue.svg)](LICENSE)
[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com/en/server/foundation)

[![Discord](https://img.shields.io/badge/-5865F2?logo=discord&logoColor=white)](https://discord.gg/VDWUjWMDw9)
[![GitHub](https://img.shields.io/badge/-181717?logo=github&logoColor=white)](https://github.com/GameFrameX/gameframex)
[![Bilibili](https://img.shields.io/badge/-00A1D6?logo=bilibili&logoColor=white)](https://www.bilibili.com/video/BV1yrpeepEn7)
[![Gitee](https://img.shields.io/badge/-C71D23?logo=gitee&logoColor=white)](https://gitee.com/GameFrameX/gameframex)

**All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams**

<br />

[Documentation](https://gameframex.doc.alianblank.com/en/server/foundation) · [Quick Start](https://gameframex.doc.alianblank.com/en/server/foundation#getting-started) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Project Overview

GameFrameX.Foundation is a modular collection of .NET 10 class libraries that deliver cross-cutting infrastructure for GameFrameX server projects — covering localization, JSON, options parsing, ORM attributes, extensions, encryption, hashing, utilities, logging, HTTP helpers, response normalization, ORM base entities, and idempotency.

### Features

| Package                                    | Description                      | Version                                                                                                                                                                                | Downloads                                                                                                                                                                                |
|--------------------------------------------|----------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `GameFrameX.Foundation.Localization`       | Lightweight localization support | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             |
| `GameFrameX.Foundation.Json`               | Unified JSON serialization       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             |
| `GameFrameX.Foundation.Options`            | CLI argument parser              | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       |
| `GameFrameX.Foundation.Orm.Attribute`      | ORM attribute definitions        | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           |
| `GameFrameX.Foundation.Extensions`         | Extension methods and helpers    | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 |
| `GameFrameX.Foundation.Encryption`         | Encryption algorithms            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 |
| `GameFrameX.Foundation.Hash`               | Hash algorithms                  | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             |
| `GameFrameX.Foundation.Utility`            | General utilities                | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       |
| `GameFrameX.Foundation.Logger`             | Unified logging                  | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         |
| `GameFrameX.Foundation.Http.Extension`     | HTTP client extensions           | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         |
| `GameFrameX.Foundation.Http.Normalization` | HTTP response normalization      | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) |
| `GameFrameX.Foundation.Orm.Entity`         | ORM entity base classes          | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 |
| `GameFrameX.Foundation.Idempotency`        | Idempotency and event envelope   | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Idempotency?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Idempotency?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               |

## Quick Start

### Installation

Install the packages you need:

```bash
dotnet add package GameFrameX.Foundation.Extensions
dotnet add package GameFrameX.Foundation.Encryption
dotnet add package GameFrameX.Foundation.Logger
```

## Usage Examples

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

## Architecture

See the full [architecture documentation](https://gameframex.doc.alianblank.com/en/server/foundation) for design principles and dependency graph.

## Platform Support

- .NET 8.0 / 9.0 / 10.0
- Supported OS: Windows, macOS, Linux

## Dependencies

| Module             | Package                                    | Description                                             | Dependencies        | Docs                                                                                  |
|--------------------|--------------------------------------------|---------------------------------------------------------|---------------------|---------------------------------------------------------------------------------------|
| Localization       | `GameFrameX.Foundation.Localization`       | Lightweight localization with lazy-loading mechanism    | Zero dependencies   | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/localization)       |
| JSON               | `GameFrameX.Foundation.Json`               | Unified JSON serialization/deserialization interfaces   | Zero dependencies   | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/json)               |
| Options            | `GameFrameX.Foundation.Options`            | Command-line option/configuration builder               | Zero dependencies   | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/options)            |
| ORM Attribute      | `GameFrameX.Foundation.Orm.Attribute`      | ORM attribute definitions for audit, cache, soft-delete, tenant, data scope, sensitive masking, tree, import/export | Zero dependencies   | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/orm-attribute)      |
| Extensions         | `GameFrameX.Foundation.Extensions`         | Core extension methods and collection helpers           | → Localization      | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/extensions)         |
| Encryption         | `GameFrameX.Foundation.Encryption`         | AES, RSA, DSA, SM2/SM4, XOR encryption                  | → Localization      | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/encryption)         |
| Hash               | `GameFrameX.Foundation.Hash`               | CRC32/64, MD5, SHA, HMAC, MurmurHash3, xxHash           | → Localization      | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/hash)               |
| Utility            | `GameFrameX.Foundation.Utility`            | Console, environment, time, Snowflake ID utilities      | → Localization      | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/utility)            |
| Logger             | `GameFrameX.Foundation.Logger`             | Unified logging with Serilog, Loki, MongoDB sinks       | → Extensions + JSON | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/logger)             |
| HTTP Extension     | `GameFrameX.Foundation.Http.Extension`     | HTTP client extension methods                           | → JSON              | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/http-extension)     |
| HTTP Normalization | `GameFrameX.Foundation.Http.Normalization` | Standardized HTTP JSON response structures              | → Logger            | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/http-normalization) |
| ORM Entity         | `GameFrameX.Foundation.Orm.Entity`         | ORM entity base classes with enterprise features        | → Utility           | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/orm-entity)         |
| Idempotency        | `GameFrameX.Foundation.Idempotency`        | Idempotency occupation/replay decisions and universal event envelope | → Utility           | [Docs](https://gameframex.doc.alianblank.com/en/server/foundation/idempotency)        |

## Documentation & Resources

- [Documentation](https://gameframex.doc.alianblank.com)
- [GitHub Repository](https://github.com/GameFrameX/GameFrameX.Foundation)
- [Issue Tracker](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- [GameFrameX Official Site](https://gameframex.doc.alianblank.com)

## Community & Support

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

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the version history of GameFrameX.Foundation.

## License

See [LICENSE.md](LICENSE) for license information.

<!--
EN: See [LICENSE.md](LICENSE) for license information.
zh-CN: 详见 [LICENSE.md](LICENSE) 文件。
zh-TW: 詳見 [LICENSE.md](LICENSE) 檔案。
ja: 詳しくは [LICENSE.md](LICENSE) をご参照ください。
ko: 자세한 내용은 [LICENSE.md](LICENSE) 파일을 참조하세요.
-->
