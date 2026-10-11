<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="GameFrameX Logo" width="160" />

# GameFrameX.Foundation

[![License](https://img.shields.io/badge/license-blue.svg)](LICENSE)
[![Version](https://img.shields.io/github/v/release/GameFrameX/GameFrameX.Foundation)](https://github.com/GameFrameX/GameFrameX.Foundation/releases)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com/zh-CN/server/foundation)

[![Discord](https://img.shields.io/badge/-5865F2?logo=discord&logoColor=white)](https://discord.gg/VDWUjWMDw9)
[![GitHub](https://img.shields.io/badge/-181717?logo=github&logoColor=white)](https://github.com/GameFrameX/gameframex)
[![Bilibili](https://img.shields.io/badge/-00A1D6?logo=bilibili&logoColor=white)](https://www.bilibili.com/video/BV1yrpeepEn7)
[![Gitee](https://img.shields.io/badge/-C71D23?logo=gitee&logoColor=white)](https://gitee.com/GameFrameX/gameframex)

**独立游戏前后端一体化解决方案 · 独立游戏开发者的圆梦大使**

<br />

[文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation) · [快速开始](https://gameframex.doc.alianblank.com/zh-CN/server/foundation) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | **简体中文** | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## 项目简介

GameFrameX.Foundation 是一组模块化的 .NET 类库合集，为 GameFrameX 服务端项目提供横切关注点的基础设施——涵盖本地化、JSON、命令行参数解析、ORM 特性、扩展方法、加密、哈希、实用工具、日志、HTTP 辅助、响应规范化、ORM 基类实体与幂等性。

### 功能特性

| 包名 | 描述 | 版本 | 下载量 |
|--------------------------------------------|----------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `GameFrameX.Foundation.Localization`       | 轻量级本地化支持                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Localization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Localization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Localization)             |
| `GameFrameX.Foundation.Json`               | 统一的 JSON 序列化                     | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Json?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Json?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Json)                             |
| `GameFrameX.Foundation.Options`            | 命令行参数解析器                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Options?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Options?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Options)                       |
| `GameFrameX.Foundation.Orm.Attribute`      | ORM 特性定义                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Attribute?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Attribute?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Attribute)           |
| `GameFrameX.Foundation.Extensions`         | 扩展方法与辅助类                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Extensions?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Extensions?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Extensions)                 |
| `GameFrameX.Foundation.Encryption`         | 加密算法                             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Encryption?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Encryption?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Encryption)                 |
| `GameFrameX.Foundation.Hash`               | 哈希算法                             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Hash?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Hash?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Hash)                             |
| `GameFrameX.Foundation.Utility`            | 通用工具集                            | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Utility?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Utility?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Utility)                       |
| `GameFrameX.Foundation.Logger`             | 统一日志                             | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger)                         |
| `GameFrameX.Foundation.Logger.MongoDB`     | MongoDB 日志 Sink                   | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.MongoDB?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.MongoDB)     | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.MongoDB?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.MongoDB)     |
| `GameFrameX.Foundation.Logger.GrafanaLoki` | Grafana Loki 日志 Sink              | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Logger.GrafanaLoki?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.GrafanaLoki) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Logger.GrafanaLoki?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Logger.GrafanaLoki) |
| `GameFrameX.Foundation.Http.Extension`     | HTTP 客户端扩展                       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Extension?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Extension?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Extension)         |
| `GameFrameX.Foundation.Http.Normalization` | HTTP 响应规范化                       | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Http.Normalization?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Http.Normalization?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Http.Normalization) |
| `GameFrameX.Foundation.Orm.Entity`         | ORM 实体基类                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Orm.Entity?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Orm.Entity?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Orm.Entity)                 |
| `GameFrameX.Foundation.Idempotency`        | 幂等性与事件信封                         | [![NuGet](https://img.shields.io/nuget/v/GameFrameX.Foundation.Idempotency?label=version&color=green)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               | [![NuGet](https://img.shields.io/nuget/dt/GameFrameX.Foundation.Idempotency?label=downloads&color=blue)](https://www.nuget.org/packages/GameFrameX.Foundation.Idempotency)               |

## 快速开始

### 安装

按需安装所需的包：

```bash
dotnet add package GameFrameX.Foundation.Extensions
dotnet add package GameFrameX.Foundation.Encryption
dotnet add package GameFrameX.Foundation.Logger
dotnet add package GameFrameX.Foundation.Logger.MongoDB
dotnet add package GameFrameX.Foundation.Logger.GrafanaLoki
```

## 使用示例

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

## 架构概览

查阅完整的[架构文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation)了解设计原则与依赖关系图。

## 平台支持

- .NET 8.0 / 9.0 / 10.0
- 支持操作系统：Windows、macOS、Linux

## 依赖

| 模块 | 包名 | 描述 | 依赖 | 文档 |
|--------------------|--------------------------------------------|---------------------------------------------------------|---------------------|---------------------------------------------------------------------------------------|
| Localization       | `GameFrameX.Foundation.Localization`       | 支持懒加载机制的轻量级本地化                                          | 无依赖                 | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/localization)       |
| JSON               | `GameFrameX.Foundation.Json`               | 统一的 JSON 序列化/反序列化接口                                     | 无依赖                 | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/json)               |
| Options            | `GameFrameX.Foundation.Options`            | 命令行选项/配置构建器                                             | 无依赖                 | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/options)            |
| ORM Attribute      | `GameFrameX.Foundation.Orm.Attribute`      | 覆盖审计、缓存、软删除、多租户、数据权限、敏感脱敏、树形、导入/导出的 ORM 特性定义                                                                        | 无依赖                 | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/orm-attribute)      |
| Extensions         | `GameFrameX.Foundation.Extensions`         | 核心扩展方法与集合辅助类                                            | → Localization      | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/extensions)         |
| Encryption         | `GameFrameX.Foundation.Encryption`         | AES、RSA、DSA、SM2/SM4、XOR 加密                              | → Localization      | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/encryption)         |
| Hash               | `GameFrameX.Foundation.Hash`               | CRC32/64、MD5、SHA、HMAC、MurmurHash3、xxHash                | → Localization      | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/hash)               |
| Utility            | `GameFrameX.Foundation.Utility`            | 控制台、环境、时间、雪花 ID 工具                                      | → Localization      | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/utility)            |
| Logger             | `GameFrameX.Foundation.Logger`             | 统一日志，内置 Serilog 控制台与文件 Sink                             | → Extensions + JSON | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/logger)             |
| Logger.MongoDB     | `GameFrameX.Foundation.Logger.MongoDB`     | Logger 的 MongoDB 日志 Sink                                 | → Logger           | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/logger)             |
| Logger.GrafanaLoki | `GameFrameX.Foundation.Logger.GrafanaLoki` | Logger 的 Grafana Loki 日志 Sink                            | → Logger           | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/logger)             |
| HTTP Extension     | `GameFrameX.Foundation.Http.Extension`     | HTTP 客户端扩展方法                                            | → JSON              | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/http-extension)     |
| HTTP Normalization | `GameFrameX.Foundation.Http.Normalization` | 标准化的 HTTP JSON 响应结构                                     | → Logger            | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/http-normalization) |
| ORM Entity         | `GameFrameX.Foundation.Orm.Entity`         | 具备企业级特性的 ORM 实体基类                                       | → Utility           | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/orm-entity)         |
| Idempotency        | `GameFrameX.Foundation.Idempotency`        | 幂等占用/重放判定与通用事件信封                                                     | → Utility           | [文档](https://gameframex.doc.alianblank.com/zh-CN/server/foundation/idempotency)        |

## 文档与资源

- [文档](https://gameframex.doc.alianblank.com)
- [GitHub 仓库](https://github.com/GameFrameX/GameFrameX.Foundation)
- [问题追踪](https://github.com/GameFrameX/GameFrameX.Foundation/issues)
- [GameFrameX 官方网站](https://gameframex.doc.alianblank.com)

## 社区与支持

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

## 更新日志

GameFrameX.Foundation 的版本更新历史请参阅 [CHANGELOG.md](CHANGELOG.md)。

## 开源协议

详见 [LICENSE.md](LICENSE) 文件。

<!--
EN: See [LICENSE.md](LICENSE) for license information.
zh-CN: 详见 [LICENSE.md](LICENSE) 文件。
zh-TW: 詳見 [LICENSE.md](LICENSE) 檔案。
ja: 詳しくは [LICENSE.md](LICENSE) をご参照ください。
ko: 자세한 내용은 [LICENSE.md](LICENSE) 파일을 참조하세요.
-->
