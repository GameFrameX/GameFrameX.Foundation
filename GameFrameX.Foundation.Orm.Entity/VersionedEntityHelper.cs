// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System;

namespace GameFrameX.Foundation.Orm.Entity
{
    /// <summary>
    /// 版本控制实体扩展方法辅助类。
    /// </summary>
    /// <remarks>
    /// Versioned entity extension method helper class.
    /// </remarks>
    public static class VersionedEntityHelper
    {
        /// <summary>
        /// 递增版本号并返回新值。
        /// </summary>
        /// <remarks>
        /// Increments the version number and returns the new value.
        /// </remarks>
        /// <param name="entity">版本控制实体 / The versioned entity</param>
        /// <returns>递增后的版本号 / The incremented version number</returns>
        public static long IncrementVersion(this IVersionedEntity entity)
        {
            entity.Version = (entity.Version ?? 0) + 1;
            return entity.Version.Value;
        }

        /// <summary>
        /// 检查当前版本号是否与期望版本号冲突。
        /// </summary>
        /// <remarks>
        /// Checks if the current version conflicts with the expected version.
        /// </remarks>
        /// <param name="entity">版本控制实体 / The versioned entity</param>
        /// <param name="expectedVersion">期望的版本号 / The expected version number</param>
        /// <returns>若版本冲突则返回 <c>true</c>；否则返回 <c>false</c> / <c>true</c> if a conflict exists; otherwise <c>false</c></returns>
        public static bool HasVersionConflict(this IVersionedEntity entity, long expectedVersion)
        {
            return entity.Version != expectedVersion;
        }

        /// <summary>
        /// 确保无版本冲突，若冲突则抛出异常。
        /// </summary>
        /// <remarks>
        /// Ensures no version conflict; throws if a conflict is detected.
        /// </remarks>
        /// <param name="entity">版本控制实体 / The versioned entity</param>
        /// <param name="expectedVersion">期望的版本号 / The expected version number</param>
        /// <exception cref="InvalidOperationException">版本冲突时抛出 / Thrown when a version conflict is detected</exception>
        public static void EnsureNoVersionConflict(this IVersionedEntity entity, long expectedVersion)
        {
            if (entity.Version != expectedVersion)
            {
                throw new InvalidOperationException(
                    $"版本冲突：期望版本 {expectedVersion}，实际版本 {entity.Version}。/ Version conflict: expected {expectedVersion}, actual {entity.Version}.");
            }
        }
    }
}
