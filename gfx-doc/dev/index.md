# index

项目级模块、change 总览和短 ID + slug 路由表。

## Modules

| Module | Spec | Changes |
|---|---|---|
| encryption | `encryption/spec.md` | `encryption/changes/index.md` |
| extensions | `extensions/spec.md` | `extensions/changes/index.md` |
| hash | `hash/spec.md` | `hash/changes/index.md` |
| http | `http/spec.md` | `http/changes/index.md` |
| json | `json/spec.md` | `json/changes/index.md` |
| localization | `localization/spec.md` | `localization/changes/index.md` |
| logger | `logger/spec.md` | `logger/changes/index.md` |
| utility | `utility/spec.md` | `utility/changes/index.md` |

## Change Tracking

| ID | Slug | Type | Status | Module | Path | Parent | Depends On | Blocks | Backlog | Updated |
|---|---|---|---|---|---|---|---|---|---|---|
| C1 | gfx-153-timerhelper-nullable | change | archived | utility | `utility/archived/C1-gfx-153-timerhelper-nullable/change.md` | none | none | none | none | 2026-07-25 |

## Notes

- `ID` is project-unique and is the preferred input for `/gfx-kernel-execute`, `/gfx-kernel-verify`, and `/gfx-kernel-archive`.
- `Slug` is the human-readable directory name suffix and should remain stable once published.
- `Parent` records ownership/derivation; it is not an execution dependency.
- `Depends On` records execution or archive prerequisites and must not create cycles.
